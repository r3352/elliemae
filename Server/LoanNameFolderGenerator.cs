// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanNameFolderGenerator
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataAccess;
using System;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class LoanNameFolderGenerator
  {
    private static Regex largeFolderRegex = new Regex("\\[Folder#[0-9]+\\]", RegexOptions.IgnoreCase);

    private static string genLoanNameFolder(int loanNameFolderNumber)
    {
      return loanNameFolderNumber > 0 ? "[Folder#" + (object) loanNameFolderNumber + "]" : "";
    }

    private static int getLoanCountFromFS(
      string loanFolder,
      int loanNameFolderNumber,
      bool createFolder)
    {
      return LoanNameFolderGenerator.getLoanCountFromFS(loanFolder, LoanNameFolderGenerator.genLoanNameFolder(loanNameFolderNumber), createFolder);
    }

    private static int getLoanCountFromFS(
      string loanFolder,
      string loanNameFolder,
      bool createFolder)
    {
      string str = Path.Combine(ClientContext.GetCurrent().Settings.LoansDir, loanFolder);
      if (loanNameFolder != null && loanNameFolder != "")
        str = Path.Combine(str, loanNameFolder);
      if (Directory.Exists(str))
      {
        string[] directories = Directory.GetDirectories(str);
        if (loanNameFolder != null && loanNameFolder != "")
          return directories.Length;
        int loanCountFromFs = 0;
        for (int index = 0; index < directories.Length; ++index)
        {
          if (!LoanNameFolderGenerator.IsLoanNameFolder(directories[index]))
            ++loanCountFromFs;
        }
        return loanCountFromFs;
      }
      if (!createFolder)
        return -1;
      Directory.CreateDirectory(str);
      return 0;
    }

    private static int getLoanCountFromDB(
      string loanFolder,
      int loanNameFolderNumber,
      bool setCountIfNull)
    {
      return LoanNameFolderGenerator.getLoanCountFromDB(loanFolder, LoanNameFolderGenerator.genLoanNameFolder(loanNameFolderNumber), setCountIfNull);
    }

    private static int getLoanCountFromDB(
      string loanFolder,
      string loanNameFolder,
      bool setCountIfNull)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select loanCount from LoanCounts");
      dbQueryBuilder.AppendLine("\twhere loanFolder = " + SQL.Encode((object) loanFolder));
      dbQueryBuilder.AppendLine("\tand loanNameFolder = " + SQL.Encode((object) loanNameFolder));
      object loanCountFromDb = dbQueryBuilder.ExecuteScalar();
      if (loanCountFromDb != null)
        return (int) loanCountFromDb;
      return setCountIfNull ? LoanNameFolderGenerator.ResetLoanCount(loanFolder, loanNameFolder) : -1;
    }

    private static int getNextNumber(string loanFolder)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select nextNumber from LoanNameFolderUniqueNumber");
      dbQueryBuilder.AppendLine("\twhere loanFolder like '" + SQL.Escape(loanFolder) + "'");
      object obj = dbQueryBuilder.ExecuteScalar();
      int nextNumber = 1;
      if (obj != null)
        nextNumber = (int) obj;
      return nextNumber;
    }

    internal static int GetMaxEntriesInAFolder(ClientContext context)
    {
      object obj = context.Cache.Get("Unpublished.MaxEntriesInAFolder");
      int o;
      if (obj != null)
      {
        o = (int) obj;
      }
      else
      {
        o = (int) context.Settings.GetServerSetting("Unpublished.MaxEntriesInAFolder");
        context.Cache.Put("Unpublished.MaxEntriesInAFolder", (object) o);
      }
      return o;
    }

    public static int GetMaxEntriesInAFolder()
    {
      return LoanNameFolderGenerator.GetMaxEntriesInAFolder(ClientContext.GetCurrent());
    }

    public static string GenerateLoanNameFolder(string loanFolder)
    {
      ClientContext current = ClientContext.GetCurrent();
      int entriesInAfolder = LoanNameFolderGenerator.GetMaxEntriesInAFolder(current);
      string path2 = (string) null;
      string path1 = Path.Combine(current.Settings.LoansDir, loanFolder);
      int nextNumber = LoanNameFolderGenerator.getNextNumber(loanFolder);
      for (int loanNameFolderNumber = 0; loanNameFolderNumber < nextNumber; ++loanNameFolderNumber)
      {
        if (LoanNameFolderGenerator.getLoanCountFromDB(loanFolder, loanNameFolderNumber, true) < entriesInAfolder)
          return LoanNameFolderGenerator.genLoanNameFolder(loanNameFolderNumber);
      }
      if (path2 == null)
      {
        do
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder((IClientContext) current);
          dbQueryBuilder.Declare("@loanFolder", "varchar(100)");
          dbQueryBuilder.Declare("@nextNumber", "int");
          dbQueryBuilder.SelectVar("@loanFolder", (object) loanFolder);
          dbQueryBuilder.AppendLine("select @nextNumber = nextNumber from LoanNameFolderUniqueNumber");
          dbQueryBuilder.AppendLine("\twhere loanFolder like '" + SQL.Escape(loanFolder) + "'");
          dbQueryBuilder.AppendLine("if (@nextNumber is null)");
          dbQueryBuilder.AppendLine("begin");
          dbQueryBuilder.AppendLine("\tselect @nextNumber = 1");
          dbQueryBuilder.AppendLine("\tinsert into LoanNameFolderUniqueNumber (loanFolder, nextNumber)");
          dbQueryBuilder.AppendLine("\t\tvalues (@loanFolder, @nextNumber + 1)");
          dbQueryBuilder.AppendLine("end");
          dbQueryBuilder.AppendLine("else");
          dbQueryBuilder.AppendLine("begin");
          dbQueryBuilder.AppendLine("\tupdate LoanNameFolderUniqueNumber set nextNumber = (nextNumber + 1)");
          dbQueryBuilder.AppendLine("\t\twhere loanFolder like '" + SQL.Escape(loanFolder) + "'");
          dbQueryBuilder.AppendLine("end");
          dbQueryBuilder.AppendLine("select @nextNumber as nextNumber");
          path2 = LoanNameFolderGenerator.genLoanNameFolder((int) dbQueryBuilder.ExecuteScalar());
        }
        while (Directory.Exists(Path.Combine(path1, path2)));
      }
      string path = Path.Combine(path1, path2);
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      return path2;
    }

    public static void ResetLoanCount(string loanFolder)
    {
      LoanNameFolderGenerator.DeleteLoanCounts(loanFolder);
      LoanNameFolderGenerator.ResetLoanCount(loanFolder, "");
      DirectoryInfo[] directories = new DirectoryInfo(Path.Combine(ClientContext.GetCurrent().Settings.LoansDir, loanFolder)).GetDirectories();
      for (int index = 0; index < directories.Length; ++index)
      {
        if (LoanNameFolderGenerator.IsLoanNameFolder(directories[index].Name))
          LoanNameFolderGenerator.ResetLoanCount(loanFolder, directories[index].Name);
      }
    }

    public static int ResetLoanCount(string loanFolder, int loanNameFolderNumber)
    {
      return LoanNameFolderGenerator.ResetLoanCount(loanFolder, LoanNameFolderGenerator.genLoanNameFolder(loanNameFolderNumber));
    }

    public static int ResetLoanCount(string loanFolder, string loanNameFolder)
    {
      int loanCountFromFs = LoanNameFolderGenerator.getLoanCountFromFS(loanFolder, loanNameFolder, true);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Declare("@loanCount", "int");
      dbQueryBuilder.AppendLine("select @loanCount = loanCount from LoanCounts");
      dbQueryBuilder.AppendLine("\twhere loanFolder = " + SQL.Encode((object) loanFolder));
      dbQueryBuilder.AppendLine("\tand loanNameFolder = " + SQL.Encode((object) loanNameFolder));
      dbQueryBuilder.AppendLine("if (@loanCount is null)");
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.AppendLine("\tinsert into LoanCounts (loanFolder, loanNameFolder, loanCount)");
      dbQueryBuilder.AppendLine("\t\tvalues (" + SQL.Encode((object) loanFolder) + ", " + SQL.Encode((object) loanNameFolder) + ", " + (object) loanCountFromFs + ")");
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.AppendLine("else");
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.AppendLine("\tupdate LoanCounts set loanCount = " + (object) loanCountFromFs);
      dbQueryBuilder.AppendLine("\t\twhere loanFolder = " + SQL.Encode((object) loanFolder) + " and loanNameFolder = " + SQL.Encode((object) loanNameFolder));
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.ExecuteNonQuery();
      return loanCountFromFs;
    }

    public static int ChangeLoanCount(string loanFolder, int loanNameFolderNumber, int delta)
    {
      return LoanNameFolderGenerator.ChangeLoanCount(loanFolder, LoanNameFolderGenerator.genLoanNameFolder(loanNameFolderNumber), delta);
    }

    public static int ChangeLoanCount(string loanFolder, string loanNameFolder, int delta)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Declare("@loanCount", "int");
      dbQueryBuilder.AppendLine("select @loanCount = loanCount from LoanCounts");
      dbQueryBuilder.AppendLine("\twhere loanFolder = " + SQL.Encode((object) loanFolder));
      dbQueryBuilder.AppendLine("\tand loanNameFolder = " + SQL.Encode((object) loanNameFolder));
      dbQueryBuilder.AppendLine("if (@loanCount is not null)");
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.AppendLine("\tselect @loanCount = @loanCount + " + (object) delta);
      dbQueryBuilder.AppendLine("\tupdate LoanCounts set loanCount = @loanCount");
      dbQueryBuilder.AppendLine("\t\twhere loanFolder = " + SQL.Encode((object) loanFolder) + " and loanNameFolder = " + SQL.Encode((object) loanNameFolder));
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.AppendLine("select @loanCount as loanCount");
      object obj = dbQueryBuilder.ExecuteScalar();
      return obj == null || obj == DBNull.Value || (int) obj < 0 ? LoanNameFolderGenerator.ResetLoanCount(loanFolder, loanNameFolder) : (int) obj;
    }

    public static void DeleteLoanFolder(string folderName)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from LoanNameFolderUniqueNumber where loanFolder = " + SQL.Encode((object) folderName));
      dbQueryBuilder.AppendLine("delete from LoanCounts where loanFolder = " + SQL.Encode((object) folderName));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteLoanCounts(string folderName)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from LoanCounts where loanFolder = " + SQL.Encode((object) folderName));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static string GetLoanNameFolderPart(string loanName)
    {
      int length = loanName.IndexOf("\\");
      if (length < 0)
        return "";
      string folderName = loanName.Substring(0, length);
      return LoanNameFolderGenerator.IsLoanNameFolder(folderName) ? folderName : throw new Exception("LoanNameFolderGenerator.GetLoanNameFolderPart(): invalid loanName '" + loanName + "'");
    }

    public static bool IsLoanNameFolder(string folderName)
    {
      if (folderName.IndexOf("[Folder#", StringComparison.CurrentCultureIgnoreCase) < 0)
        return false;
      while (folderName.EndsWith("\\"))
        folderName = folderName.Substring(0, folderName.Length - 1);
      int num = folderName.LastIndexOf("\\");
      if (num >= 0)
        folderName = folderName.Substring(num + 1);
      return LoanNameFolderGenerator.largeFolderRegex.Match(folderName).Success;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanNumberGenerator
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataAccess;
using System;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class LoanNumberGenerator
  {
    private const string className = "LoanNumberGenerator�";
    public static readonly int MaxLength = 18;

    private LoanNumberGenerator()
    {
    }

    public static string GetNextLoanNumber(OrgInfo sourceOrg)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("declare @valueStr varchar(" + (object) 9 + ")");
        dbQueryBuilder.AppendLine("declare @value decimal(" + (object) 10 + ", 0)");
        dbQueryBuilder.AppendLine("declare @nextValueStr varchar(" + (object) 9 + ")");
        dbQueryBuilder.AppendLine("declare @useBranchNumbering bit");
        dbQueryBuilder.AppendLine("declare @branchCode varchar(10)");
        dbQueryBuilder.AppendLine("select @useBranchNumbering = 0");
        dbQueryBuilder.AppendLine("select @branchCode = ''");
        if (sourceOrg != null && sourceOrg.OrgCode != "")
          dbQueryBuilder.AppendLine("select @useBranchNumbering = 1, @branchCode = org_code from BranchLoanNumber with (rowlock, xlock) where (enabled = 1) and (next_number is not null) and (org_code = " + SQL.Encode((object) sourceOrg.OrgCode) + ")");
        dbQueryBuilder.AppendLine("if @useBranchNumbering = 0");
        dbQueryBuilder.AppendLine("    select @valueStr = NextNumber from LoanNumber with (tablockx)");
        dbQueryBuilder.AppendLine("else");
        dbQueryBuilder.AppendLine("    select @valueStr = next_number from BranchLoanNumber with (rowlock, xlock) where org_code = @branchCode");
        dbQueryBuilder.AppendLine("select @value = convert(decimal(" + (object) 10 + ", 0), @valueStr)");
        dbQueryBuilder.AppendLine("select @nextValueStr = substring(replace(str(@value + 1, " + (object) 10 + ", 0), ' ', '0'), " + (object) 10 + " - len(@valueStr) + 1, len(@valueStr))");
        dbQueryBuilder.AppendLine("if @useBranchNumbering = 0");
        dbQueryBuilder.AppendLine("    update LoanNumber set NextNumber = @nextValueStr");
        dbQueryBuilder.AppendLine("else");
        dbQueryBuilder.AppendLine("    update BranchLoanNumber set next_number = @nextValueStr where org_code = @branchCode");
        dbQueryBuilder.AppendLine("select (case @useBranchNumbering when 1 then '1' else UseOrgCode end) as UseOrgCode, UseYear, UseMonth, Prefix, Suffix, @valueStr as NextNumber from LoanNumber");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.Serialized);
        if (dataRowCollection.Count == 0)
          Err.Raise(TraceLevel.Error, nameof (LoanNumberGenerator), (ServerException) new ServerDataException("Error in LoanNumber table: no rows returned"));
        DataRow dataRow = dataRowCollection[0];
        string str = string.Empty;
        if (dataRow["UseOrgCode"].ToString() == "1")
          str = sourceOrg == null ? "" : sourceOrg.OrgCode;
        if (dataRow["UseYear"].ToString() == "1")
          str += DateTime.Now.Year.ToString("00").Substring(2, 2);
        if (dataRow["UseMonth"].ToString() == "1")
          str += DateTime.Now.Month.ToString("00");
        string nextLoanNumber = str + dataRow["Prefix"].ToString() + dataRow["NextNumber"].ToString() + dataRow["Suffix"].ToString();
        if (nextLoanNumber.Length > LoanNumberGenerator.MaxLength)
          nextLoanNumber = nextLoanNumber.Substring(0, LoanNumberGenerator.MaxLength);
        TraceLog.WriteVerbose(nameof (LoanNumberGenerator), "Generated Loan Number \"" + nextLoanNumber + "\"");
        return nextLoanNumber;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanNumberGenerator), ex);
        return (string) null;
      }
    }

    public static LoanNumberingInfo GetLoanNumberingInfo()
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("LoanNumber"));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
          Err.Raise(TraceLevel.Error, nameof (LoanNumberGenerator), (ServerException) new ServerDataException("Error in LoanNumber table: no rows returned"));
        DataRow dataRow = dataRowCollection[0];
        return new LoanNumberingInfo(dataRow["Prefix"].ToString(), dataRow["NextNumber"].ToString(), dataRow["Suffix"].ToString(), dataRow["UseOrgCode"].ToString() == "1", dataRow["UseMonth"].ToString() == "1", dataRow["UseYear"].ToString() == "1");
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanNumberGenerator), ex);
        return (LoanNumberingInfo) null;
      }
    }

    public static void SetLoanNumberingInfo(LoanNumberingInfo info)
    {
      try
      {
        DbValueList values = new DbValueList();
        values.Add("Prefix", (object) info.Prefix);
        values.Add("Suffix", (object) info.Suffix);
        values.Add("NextNumber", (object) info.NextNumber);
        values.Add("UseOrgCode", (object) info.UseOrgCode, (IDbEncoder) DbEncoding.Flag);
        values.Add("UseMonth", (object) info.UseMonth, (IDbEncoder) DbEncoding.Flag);
        values.Add("UseYear", (object) info.UseYear, (IDbEncoder) DbEncoding.Flag);
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Update(DbAccessManager.GetTable("LoanNumber"), values, new DbValueList());
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanNumberGenerator), ex);
      }
    }

    public static BranchLoanNumberingInfo GetBranchLoanNumberingInfo(string orgCode)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("BranchLoanNumber"), new DbValue("org_code", (object) orgCode));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count == 0 ? (BranchLoanNumberingInfo) null : LoanNumberGenerator.dataRowToBranchLoanNumberingInfo(dataRowCollection[0]);
    }

    public static void SaveBranchLoanNumberingInfo(BranchLoanNumberingInfo info)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("BranchLoanNumber");
      dbQueryBuilder.DeleteFrom(table, new DbValue("org_code", (object) info.OrgCode));
      dbQueryBuilder.InsertInto(table, LoanNumberGenerator.createDbValueList(info), true, false);
      dbQueryBuilder.Execute();
    }

    public static BranchLoanNumberingInfo[] GetAllBranchLoanNumberingInfo(bool onlyInUse)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (onlyInUse)
      {
        dbQueryBuilder.AppendLine("select distinct oc.org_code, IsNull(bln.enabled, 0) as enabled, bln.next_number from org_chart oc left outer join BranchLoanNumber bln on oc.org_code = bln.org_code where IsNull(oc.org_code, '') <> ''");
      }
      else
      {
        dbQueryBuilder.AppendLine("select distinct * from");
        dbQueryBuilder.AppendLine("\t(select oc.org_code, IsNull(bln.enabled, 0) as enabled, bln.next_number from org_chart oc left outer join BranchLoanNumber bln on oc.org_code = bln.org_code where IsNull(oc.org_code, '') <> '') t1");
        dbQueryBuilder.AppendLine("\tunion");
        dbQueryBuilder.AppendLine("\t(select org_code, enabled, next_number from BranchLoanNumber)");
      }
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      BranchLoanNumberingInfo[] loanNumberingInfo = new BranchLoanNumberingInfo[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        loanNumberingInfo[index] = LoanNumberGenerator.dataRowToBranchLoanNumberingInfo(dataRowCollection[index]);
      return loanNumberingInfo;
    }

    private static BranchLoanNumberingInfo dataRowToBranchLoanNumberingInfo(DataRow r)
    {
      return new BranchLoanNumberingInfo(r["org_code"].ToString(), (bool) r["enabled"], (string) SQL.Decode(r["next_number"], (object) ""));
    }

    private static DbValueList createDbValueList(BranchLoanNumberingInfo info)
    {
      return new DbValueList()
      {
        {
          "org_code",
          (object) info.OrgCode
        },
        {
          "enabled",
          (object) info.Enabled,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "next_number",
          (object) info.NextNumber
        }
      };
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.DraftLoanFolder
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class DraftLoanFolder : IComparable
  {
    private const string className = "ApplicationFolder�";
    private string name;
    private DirectoryInfo applicationFolder;

    public DraftLoanFolder(string name)
    {
      this.name = name;
      if (name.IndexOf("\\") >= 0)
        this.applicationFolder = new DirectoryInfo(name);
      else
        this.applicationFolder = DraftLoanFolder.getLoanFolder(name);
    }

    public string Name => this.name;

    public DraftLoanFolder(DirectoryInfo dirInfo)
    {
      this.name = dirInfo.Name;
      this.applicationFolder = dirInfo;
    }

    private static DirectoryInfo getLoanFolder(string name)
    {
      return new DirectoryInfo(Path.Combine(ClientContext.GetCurrent().Settings.DraftLoansDir, name));
    }

    public BinaryObject ReadRawLoanData(string loanName, ILoanSettings loanSettings = null)
    {
      string dataFilePath = this.getDataFilePath(loanName, loanSettings);
      try
      {
        BinaryObject binaryObject = (BinaryObject) null;
        using (DataFile latestVersion = FileStore.GetLatestVersion(dataFilePath))
        {
          if (!latestVersion.Exists)
            return (BinaryObject) null;
          binaryObject = latestVersion.GetData();
        }
        TraceLog.WriteVerbose("ApplicationFolder", "Read LoanData for loan \"" + this.applicationFolder.Name + loanName + "\" from " + dataFilePath);
        return binaryObject;
      }
      catch (Exception ex)
      {
        Err.Raise(TraceLevel.Warning, "ApplicationFolder", new ServerException("Cannot deserialize loan data file '" + dataFilePath + "': " + ex.Message));
        return (BinaryObject) null;
      }
    }

    private string getDataFilePath(string loanName, ILoanSettings loanSettings = null)
    {
      return loanSettings != null && loanSettings.ComplianceSettings.Contains((object) "DeferredProcess.LoanFolder") && loanSettings.ComplianceSettings.Contains((object) "DeferredProcess.LoanFileName") ? Path.Combine(loanSettings.ComplianceSettings[(object) "DeferredProcess.LoanFolder"] as string, loanSettings.ComplianceSettings[(object) "DeferredProcess.LoanFileName"] as string) : Path.Combine(this.getLoanPath(loanName), "loan.em");
    }

    public LoanData ReadLoanData(string loanName, ILoanSettings loanSettings, string loanFolder)
    {
      using (BinaryObject data = this.ReadRawLoanData(loanName, loanSettings))
        return data == null ? (LoanData) null : new LoanDataFormatter().Deserialize(data, loanSettings, loanFolder: loanFolder);
    }

    public LoanEventLogList ReadLoanEventLog(string loanName)
    {
      using (BinaryObject data = this.ReadRawLoanEvent(loanName))
        return data == null ? new LoanEventLogList() : new LoanDataFormatter().DeserializeLoanEventLog(data);
    }

    public BinaryObject ReadRawLoanEvent(string loanName)
    {
      string dataEventFilePath = this.getDataEventFilePath(loanName);
      try
      {
        BinaryObject binaryObject = (BinaryObject) null;
        using (DataFile latestVersion = FileStore.GetLatestVersion(dataEventFilePath))
        {
          if (!latestVersion.Exists)
            return (BinaryObject) null;
          binaryObject = latestVersion.GetData();
        }
        TraceLog.WriteVerbose("ApplicationFolder", "Read LoanData for loan \"" + this.applicationFolder.Name + "\\" + loanName + "\" from " + dataEventFilePath);
        return binaryObject;
      }
      catch (Exception ex)
      {
        Err.Raise(TraceLevel.Warning, "ApplicationFolder", new ServerException("Cannot deserialize loan data file '" + dataEventFilePath + "': " + ex.Message));
        return (BinaryObject) null;
      }
    }

    private string getDataEventFilePath(string loanName)
    {
      return Path.Combine(this.getLoanPath(loanName), "LoanEventLog.em");
    }

    private void ensureDirectoryExists(string filePath)
    {
      string directoryName = Path.GetDirectoryName(filePath);
      if (Directory.Exists(directoryName))
        return;
      Directory.CreateDirectory(directoryName);
    }

    private void ensureSafeLoanFolderName(string loanName)
    {
      LoanIdentity loanIdentity = new LoanIdentity(this.name, loanName);
      string[] strArray = loanName.Split('\\');
      if (!(strArray[strArray.Length - 1].Replace(".", "") == ""))
        return;
      Err.Raise(TraceLevel.Error, "ApplicationFolder", new ServerException("The specified loan name is unsafe for this operation"));
    }

    public void WriteLoanData(string loanName, LoanData data)
    {
      this.WriteLoanData(loanName, data, false);
    }

    public void WriteLoanData(
      string loanName,
      LoanData data,
      bool allowOverwrite,
      ILoanSettings loanSettings = null)
    {
      this.WriteRawLoanData(loanName, new LoanDataFormatter().Serialize(data, false), allowOverwrite, loanSettings);
    }

    private string getDataFilePath(string loanName)
    {
      return Path.Combine(this.getLoanPath(loanName), "loan.em");
    }

    public string GetFullLoanFilePath(string loanName)
    {
      return Path.Combine(this.getLoanPath(loanName), "loan.em");
    }

    private string getLoanPath(string loanName)
    {
      string str = "\\" + loanName.Substring(1, 13).Replace("{", "").Replace("}", "");
      return Path.Combine(Path.Combine(ClientContext.GetCurrent().Settings.DraftLoansDir + str, loanName));
    }

    public void WriteRawLoanData(
      string loanName,
      BinaryObject data,
      bool allowOverwrite,
      ILoanSettings loanSettings = null)
    {
      string dataFilePath = this.getDataFilePath(loanName);
      if (!allowOverwrite && File.Exists(dataFilePath))
        throw new DuplicateObjectException("Cannot serialize loan file to '" + dataFilePath + "'. A loan file already exists at that path.", ObjectType.Loan, (object) new LoanIdentity(this.applicationFolder.Name, loanName));
      try
      {
        this.ensureDirectoryExists(dataFilePath);
        using (DataFile dataFile = FileStore.CheckOut(dataFilePath, MutexAccess.Write))
          dataFile.CheckIn(data, loanSettings);
        TraceLog.WriteVerbose("ApplicationFolder", "Wrote LoanData for loan \"" + this.applicationFolder.Name + "\\" + loanName + "\" to " + dataFilePath);
      }
      catch (Exception ex)
      {
        Err.Raise(TraceLevel.Warning, "ApplicationFolder", new ServerException("Cannot serialize loan data file '" + dataFilePath + "': " + ex.Message));
      }
    }

    public int CompareTo(object obj)
    {
      if (!(obj is LoanFolder))
        return -1;
      LoanFolder loanFolder = (LoanFolder) obj;
      return string.Compare(this.Name.ToUpper(), loanFolder.Name.ToUpper());
    }
  }
}

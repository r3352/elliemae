// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SystemAuditTrail.LoanFileAuditRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.SystemAuditTrail
{
  [Serializable]
  public class LoanFileAuditRecord : SystemAuditRecord
  {
    private string source;
    private string loanFolder;
    private string loanNumber;
    private string loanGuid;
    private string borrowerLastName;
    private string borrowerFirstName;
    private string address;
    private string appName;
    private DateTime autoSaveDateTime;
    private int fileVersionNumber;

    public LoanFileAuditRecord(
      string userID,
      string userFullName,
      ActionType actionType,
      DateTime dateTime,
      string loanGuid,
      string loanFolder,
      string loanNumber,
      string borrowerLastName,
      string borrowerFirstName,
      string address,
      string fileSource,
      string appName,
      int fileVersionNumber)
      : base(userID, userFullName, actionType, dateTime, AuditObjectType.Loan)
    {
      this.loanFolder = loanFolder;
      this.loanGuid = loanGuid;
      this.loanNumber = loanNumber;
      this.borrowerFirstName = borrowerFirstName;
      this.borrowerLastName = borrowerLastName;
      this.address = address;
      this.source = fileSource;
      this.appName = appName;
      if (appName.ToUpper() == "ENCOMPASS")
        this.appName = "Encompass SmartClient";
      else if (appName.ToUpper() == "MOBILE")
        this.appName = "Encompass Mobile";
      else if (appName.ToUpper().StartsWith("API."))
        this.appName = "Encompass SDK";
      this.fileVersionNumber = fileVersionNumber;
    }

    public LoanFileAuditRecord(
      string userID,
      string userFullName,
      ActionType actionType,
      DateTime dateTime,
      string loanGuid,
      string loanFolder,
      string loanNumber,
      string borrowerLastName,
      string borrowerFirstName,
      string address,
      string fileSource,
      string appName)
      : base(userID, userFullName, actionType, dateTime, AuditObjectType.Loan)
    {
      this.loanFolder = loanFolder;
      this.loanGuid = loanGuid;
      this.loanNumber = loanNumber;
      this.borrowerFirstName = borrowerFirstName;
      this.borrowerLastName = borrowerLastName;
      this.address = address;
      this.source = fileSource;
      this.appName = appName;
      if (appName.ToUpper() == "ENCOMPASS")
        this.appName = "Encompass SmartClient";
      else if (appName.ToUpper() == "MOBILE")
        this.appName = "Encompass Mobile";
      else if (appName.ToUpper().StartsWith("API."))
        this.appName = "Encompass SDK";
      this.fileVersionNumber = 0;
    }

    public LoanFileAuditRecord(
      string userID,
      string userFullName,
      ActionType actionType,
      DateTime dateTime,
      string loanGuid,
      string loanFolder,
      string loanNumber,
      string borrowerLastName,
      string borrowerFirstName,
      string address,
      string fileSource,
      string appName,
      int fileVersionNumber,
      string impersonatedUserID,
      string impersonatedUserFullName)
      : this(userID, userFullName, actionType, dateTime, loanGuid, loanFolder, loanNumber, borrowerLastName, borrowerFirstName, address, fileSource, appName, fileVersionNumber)
    {
      this.impersonatedUserID = impersonatedUserID;
      this.impersonatedUserFullName = impersonatedUserFullName;
    }

    public DateTime AutoSaveDateTime
    {
      get => this.autoSaveDateTime;
      set => this.autoSaveDateTime = value;
    }

    public string LoanFileSource
    {
      get => this.source;
      set => this.source = value;
    }

    public string Address
    {
      get => this.address;
      set => this.address = value;
    }

    public string BorrowerLastName
    {
      get => this.borrowerLastName;
      set => this.borrowerLastName = value;
    }

    public string BorrowerFirstName
    {
      get => this.borrowerFirstName;
      set => this.borrowerFirstName = value;
    }

    public string LoanFolder
    {
      get => this.loanFolder;
      set => this.loanFolder = value;
    }

    public string LoanGuid
    {
      get => this.loanGuid;
      set => this.loanGuid = value;
    }

    public string LoanNumber
    {
      get => this.loanNumber;
      set => this.loanNumber = value;
    }

    public string AppName
    {
      get => this.appName;
      set => this.appName = value;
    }

    public int FileVersionNumber
    {
      get => this.fileVersionNumber;
      set => this.fileVersionNumber = value;
    }

    public override string[] ColumnHeaders
    {
      get
      {
        return new string[9]
        {
          "User ID",
          "Action Type",
          "DateTime",
          "Loan GUID",
          "Borrower Last Name",
          "Loan File Source",
          "Loan Number",
          "Modified In",
          "Additional Info"
        };
      }
    }

    public override string[] ColumnContents
    {
      get
      {
        string str = "";
        if (this.autoSaveDateTime != DateTime.MinValue)
          str = "Autosave " + this.autoSaveDateTime.ToString("MM/dd/yyyy HH:mm:ss");
        return new string[9]
        {
          this.userID,
          this.actionType.ToString(),
          this.dateTime.ToString("MM/dd/yyyy HH:mm:ss"),
          this.loanGuid,
          this.borrowerLastName,
          this.LoanFileSource,
          this.LoanNumber,
          this.appName,
          str
        };
      }
    }

    public override string ContentToCSV
    {
      get
      {
        return string.Join(",", this.CSVEncode(this.userID), this.CSVEncode(this.actionType.ToString()), this.CSVEncode(this.dateTime.ToString("MM/dd/yyyy HH:mm:ss")), this.CSVEncode(this.loanGuid), this.CSVEncode(this.borrowerLastName), this.CSVEncode(this.LoanFileSource), this.CSVEncode(this.LoanNumber), this.CSVEncode(this.appName));
      }
    }
  }
}

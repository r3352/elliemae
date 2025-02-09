// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SystemAuditTrail.UserProfileAuditRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.SystemAuditTrail
{
  [Serializable]
  public class UserProfileAuditRecord : SystemAuditRecord
  {
    private string userAccountID = "";
    private string userAccountName = "";

    public UserProfileAuditRecord(
      string loginUserID,
      string loginUserFullName,
      ActionType actionType,
      DateTime dateTime,
      string userID,
      string userFullName)
      : base(loginUserID, loginUserFullName, actionType, dateTime, AuditObjectType.User)
    {
      this.userAccountID = userID;
      this.userAccountName = userFullName;
    }

    public string UserAccountID
    {
      get => this.userAccountID;
      set => this.userAccountID = value;
    }

    public string UserAccountName
    {
      get => this.userAccountName;
      set => this.userAccountName = value;
    }

    public override string[] ColumnHeaders
    {
      get
      {
        return new string[5]
        {
          "Performed by",
          "Action Type",
          "DateTime",
          "User ID",
          "User Name"
        };
      }
    }

    public override string[] ColumnContents
    {
      get
      {
        return new string[5]
        {
          this.userID,
          this.actionType.ToString(),
          this.dateTime.ToString("MM/dd/yyyy HH:mm:ss"),
          this.UserAccountID,
          this.UserAccountName
        };
      }
    }

    public override string ContentToCSV
    {
      get
      {
        return string.Join(",", this.CSVEncode(this.userID), this.CSVEncode(this.actionType.ToString()), this.CSVEncode(this.dateTime.ToString("MM/dd/yyyy HH:mm:ss")), this.CSVEncode(this.UserAccountID), this.CSVEncode(this.UserAccountName));
      }
    }
  }
}

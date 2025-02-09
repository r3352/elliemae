// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SystemAuditTrail.UserGroupAuditRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.SystemAuditTrail
{
  [Serializable]
  public class UserGroupAuditRecord : SystemAuditRecord
  {
    private int userGroupID;
    private string userGroupName = "";

    public UserGroupAuditRecord(
      string invokeUserID,
      string invokeUserFullName,
      ActionType action,
      DateTime loginTime,
      int userGroupID,
      string userGroupName)
      : base(invokeUserID, invokeUserFullName, action, loginTime, AuditObjectType.UserGroup)
    {
      this.userGroupID = userGroupID;
      this.userGroupName = userGroupName;
    }

    public UserGroupAuditRecord(
      string invokeUserID,
      string invokeUserFullName,
      ActionType action,
      int userGroupID,
      string userGroupName)
      : base(invokeUserID, invokeUserFullName, action, AuditObjectType.UserGroup)
    {
      this.userGroupID = userGroupID;
      this.userGroupName = userGroupName;
    }

    public int UserGroupID
    {
      get => this.userGroupID;
      set => this.userGroupID = value;
    }

    public string UserGroupName
    {
      get => this.userGroupName;
      set => this.userGroupName = value;
    }

    public override string[] ColumnHeaders
    {
      get
      {
        return new string[5]
        {
          "Performed by User",
          "Action Type",
          "DateTime",
          "UserGroup ID",
          "UserGroup Name"
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
          this.UserGroupID.ToString(),
          this.UserGroupName
        };
      }
    }

    public override string ContentToCSV
    {
      get
      {
        return string.Join(",", this.CSVEncode(this.userID), this.CSVEncode(this.actionType.ToString()), this.CSVEncode(this.dateTime.ToString("MM/dd/yyyy HH:mm:ss")), this.CSVEncode(this.UserGroupID.ToString()), this.CSVEncode(this.UserGroupName));
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SystemAuditTrail.FailedUserLoginAuditRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.SystemAuditTrail
{
  [Serializable]
  public class FailedUserLoginAuditRecord : SystemAuditRecord
  {
    private string ipAddress = "";

    public FailedUserLoginAuditRecord(
      string targetUserID,
      ActionType actionType,
      string ipAddress,
      DateTime timeStamp)
      : base(targetUserID, "", actionType, timeStamp, AuditObjectType.None)
    {
      this.ipAddress = ipAddress;
    }

    public string TargetUserID
    {
      get => this.userID;
      set => this.userID = value;
    }

    public string IPAddress
    {
      get => this.ipAddress;
      set => this.ipAddress = value;
    }

    public override string[] ColumnHeaders
    {
      get
      {
        return new string[4]
        {
          "Target UserID",
          "Action Type",
          "IP Address",
          "DateTime"
        };
      }
    }

    public override string[] ColumnContents
    {
      get
      {
        return new string[4]
        {
          this.userID,
          this.translateAction(this.actionType),
          this.ipAddress,
          this.dateTime.ToString("MM/dd/yyyy HH:mm:ss")
        };
      }
    }

    public override string ContentToCSV
    {
      get
      {
        return string.Join(",", this.CSVEncode(this.userID), this.CSVEncode(this.translateAction(this.actionType)), this.CSVEncode(this.ipAddress), this.CSVEncode(this.dateTime.ToString("MM/dd/yyyy HH:mm:ss")));
      }
    }

    private string translateAction(ActionType type)
    {
      switch (type)
      {
        case ActionType.FailedLoginUserNotFound:
          return "User Not Found";
        case ActionType.FailedLoginPasswordMismatch:
          return "Password Mismatch";
        case ActionType.FailedLoginUserDisabled:
          return "User Disabled";
        case ActionType.FailedLoginLoginDisabled:
          return "Login Disabled";
        case ActionType.FailedLoginUserLocked:
          return "User Locked";
        case ActionType.FailedLoginPersonaNotFound:
          return "Persona Not Found";
        case ActionType.IPBlocked:
          return "IP Blocked";
        case ActionType.FailedLoginServerBusy:
          return "Server Busy";
        default:
          return "";
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SystemAuditTrail.UserLoginAuditRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.SystemAuditTrail
{
  [Serializable]
  public class UserLoginAuditRecord : SystemAuditRecord
  {
    private string loginUserID = "";
    private string loginUserName = "";
    private string ipAddress = "";
    private string machineName = "";

    public UserLoginAuditRecord(
      string invokeUserID,
      string invokeUserFullName,
      ActionType action,
      DateTime loginTime,
      string loginUserID,
      string loginUserName,
      string ipAddress,
      string machineName)
      : base(invokeUserID, invokeUserFullName, action, loginTime, AuditObjectType.User)
    {
      this.loginUserID = loginUserID;
      this.loginUserName = loginUserName;
      this.ipAddress = ipAddress;
      this.machineName = machineName;
    }

    public string LoginUserID
    {
      get => this.loginUserID;
      set => this.loginUserID = value;
    }

    public string LoginUserName
    {
      get => this.loginUserName;
      set => this.loginUserName = value;
    }

    public string IPAddress
    {
      get => this.ipAddress;
      set => this.ipAddress = value;
    }

    public string MachineName
    {
      get => this.machineName;
      set => this.machineName = value;
    }

    public override string[] ColumnHeaders
    {
      get
      {
        return new string[7]
        {
          "Performed by User",
          "Action Type",
          "DateTime",
          "User ID",
          "User Name",
          "IP Address",
          "Machine Name"
        };
      }
    }

    public override string[] ColumnContents
    {
      get
      {
        return new string[7]
        {
          this.userID,
          this.actionType.ToString(),
          this.dateTime.ToString("MM/dd/yyyy HH:mm:ss"),
          this.LoginUserID,
          this.LoginUserName,
          this.IPAddress,
          this.MachineName
        };
      }
    }

    public override string ContentToCSV
    {
      get
      {
        return string.Join(",", this.CSVEncode(this.userID), this.CSVEncode(this.actionType.ToString()), this.CSVEncode(this.dateTime.ToString("MM/dd/yyyy HH:mm:ss")), this.CSVEncode(this.LoginUserID), this.CSVEncode(this.LoginUserName), this.CSVEncode(this.IPAddress), this.CSVEncode(this.MachineName));
      }
    }
  }
}

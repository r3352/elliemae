// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SystemAuditTrail.UserLogoutAuditRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.SystemAuditTrail
{
  [Serializable]
  public class UserLogoutAuditRecord : SystemAuditRecord
  {
    private double duration;
    private string logoutReason = "";
    private string logoutUserID = "";
    private string logoutUserName = "";

    public UserLogoutAuditRecord(
      string invokeUserID,
      string invokeUserFullName,
      ActionType action,
      DateTime loginTime,
      string logoutUserID,
      string logoutUserName,
      string reason)
      : base(invokeUserID, invokeUserFullName, action, DateTime.Now, AuditObjectType.User)
    {
      this.duration = DateTime.Now.Subtract(loginTime).TotalSeconds;
      this.logoutUserID = logoutUserID;
      this.logoutUserName = logoutUserName;
      this.logoutReason = reason;
    }

    public UserLogoutAuditRecord(
      string invokeUserID,
      string invokeUserFullName,
      ActionType action,
      DateTime loginTime,
      string logoutUserID,
      string logoutUserName,
      string reason,
      double duration)
      : base(invokeUserID, invokeUserFullName, action, loginTime, AuditObjectType.User)
    {
      this.duration = duration;
      this.logoutReason = reason;
      this.logoutUserID = logoutUserID;
      this.logoutUserName = logoutUserName;
    }

    public double Duration
    {
      get => this.duration;
      set => this.duration = value;
    }

    public string LogoutReason
    {
      get => this.logoutReason;
      set => this.logoutReason = value;
    }

    public string LogoutUserID
    {
      get => this.logoutUserID;
      set => this.logoutUserID = value;
    }

    public string LogoutUserName
    {
      get => this.logoutUserName;
      set => this.logoutUserName = value;
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
          "Duration",
          "LogoutReason"
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
          this.LogoutUserID,
          this.LogoutUserName,
          this.getDuration(),
          this.LogoutReason
        };
      }
    }

    public override string ContentToCSV
    {
      get
      {
        return string.Join(",", this.CSVEncode(this.userID), this.CSVEncode(this.actionType.ToString()), this.CSVEncode(this.dateTime.ToString("MM/dd/yyyy HH:mm:ss")), this.CSVEncode(this.LogoutUserID), this.CSVEncode(this.LogoutUserName), this.CSVEncode(this.getDuration()), this.CSVEncode(this.LogoutReason));
      }
    }

    private string getDuration()
    {
      TimeSpan timeSpan = new TimeSpan(0, 0, Convert.ToInt32(this.Duration));
      string duration = "";
      if (timeSpan.Days > 0)
        duration = timeSpan.Days <= 1 ? timeSpan.Days.ToString() + " day " : timeSpan.Days.ToString() + " days ";
      if (timeSpan.Hours > 0)
        duration = timeSpan.Hours <= 1 ? duration + (object) timeSpan.Hours + " hour " : duration + (object) timeSpan.Hours + " hours ";
      if (timeSpan.Minutes > 0)
        duration = timeSpan.Minutes <= 1 ? duration + (object) timeSpan.Minutes + " Minute " : duration + (object) timeSpan.Minutes + " Minutes ";
      if (timeSpan.Seconds > 0)
        duration = timeSpan.Seconds <= 1 ? duration + (object) timeSpan.Seconds + " Second " : duration + (object) timeSpan.Seconds + " Seconds ";
      return duration;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SystemAuditTrail.HMDAAuditRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.SystemAuditTrail
{
  [Serializable]
  public class HMDAAuditRecord : SystemAuditRecord
  {
    private int profileID;
    private string profileName = "";

    public HMDAAuditRecord(
      string invokeUserID,
      string invokeUserFullName,
      ActionType action,
      DateTime loginTime,
      int profileID,
      AuditObjectType type,
      string profileName)
      : base(invokeUserID, invokeUserFullName, action, loginTime, type)
    {
      this.profileID = profileID;
      this.profileName = profileName;
    }

    public override string[] ColumnHeaders
    {
      get
      {
        return new string[4]
        {
          "Performed by User",
          "Action Type",
          "Date/Time",
          "HMDA Profile Name"
        };
      }
    }

    public int ProfileID
    {
      get => this.profileID;
      set => this.profileID = value;
    }

    public string ProfileName
    {
      get => this.profileName;
      set => this.profileName = value;
    }

    public override string[] ColumnContents
    {
      get
      {
        return new string[4]
        {
          this.userID,
          this.actionType.ToString(),
          this.dateTime.ToString("MM/dd/yyyy HH:mm:ss"),
          this.ProfileName
        };
      }
    }

    public override string ContentToCSV
    {
      get
      {
        return string.Join(",", this.CSVEncode(this.userID), this.CSVEncode(this.actionType.ToString()), this.CSVEncode(this.dateTime.ToString("MM/dd/yyyy HH:mm:ss")), this.CSVEncode(this.ProfileName));
      }
    }
  }
}

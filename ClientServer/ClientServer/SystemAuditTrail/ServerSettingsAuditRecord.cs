// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SystemAuditTrail.ServerSettingsAuditRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.SystemAuditTrail
{
  [Serializable]
  public class ServerSettingsAuditRecord : SystemAuditRecord
  {
    private ServerSettingsCategory category;
    private AdminToolsServerSetting settingName;
    private string oldSettingValue = "";
    private string newSettingValue = "";

    public ServerSettingsAuditRecord(
      string invokeUserID,
      string invokeUserFullName,
      ActionType action,
      DateTime loginTime,
      AuditObjectType type,
      ServerSettingsCategory category,
      AdminToolsServerSetting settingName,
      string oldSettingValue = "�",
      string newSettingValue = "�")
      : base(invokeUserID, invokeUserFullName, action, loginTime, type)
    {
      this.category = category;
      this.settingName = settingName;
      this.oldSettingValue = oldSettingValue;
      this.newSettingValue = newSettingValue;
    }

    public ServerSettingsCategory Category
    {
      get => this.category;
      set => this.category = value;
    }

    public AdminToolsServerSetting SettingName
    {
      get => this.settingName;
      set => this.settingName = value;
    }

    public string OldSettingValue
    {
      get => this.oldSettingValue;
      set => this.oldSettingValue = value;
    }

    public string NewSettingValue
    {
      get => this.newSettingValue;
      set => this.newSettingValue = value;
    }

    public override string[] ColumnHeaders
    {
      get
      {
        return new string[3]
        {
          "Performed by User",
          "Action Type",
          "DateTime"
        };
      }
    }

    public override string[] ColumnContents
    {
      get
      {
        return new string[3]
        {
          this.userID,
          this.actionType.ToString(),
          this.dateTime.ToString("MM/dd/yyyy HH:mm:ss")
        };
      }
    }

    public override string ContentToCSV
    {
      get
      {
        return string.Join(",", this.CSVEncode(this.userID), this.CSVEncode(this.actionType.ToString()), this.CSVEncode(this.dateTime.ToString("MM/dd/yyyy HH:mm:ss")));
      }
    }
  }
}

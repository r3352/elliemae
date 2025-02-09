// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.AlertSetupData
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class AlertSetupData
  {
    public Hashtable MilestoneAlertMessages;
    public RoleInfo[] Roles;
    public AlertConfig[] AlertConfigList;
    [NonSerialized]
    private Hashtable roleNames;

    public Hashtable RoleNames
    {
      get
      {
        if (this.roleNames == null)
        {
          this.roleNames = new Hashtable();
          foreach (RoleInfo role in this.Roles)
            this.roleNames[(object) role.ID] = (object) role.Name;
        }
        return this.roleNames;
      }
    }

    public AlertConfig GetAlertConfig(int alertId)
    {
      foreach (AlertConfig alertConfig in this.AlertConfigList)
      {
        if (alertConfig.AlertID == alertId)
          return alertConfig;
      }
      return (AlertConfig) null;
    }

    public AlertConfig GetAlertConfigByName(string alertName)
    {
      foreach (AlertConfig alertConfig in this.AlertConfigList)
      {
        if (string.Compare(alertConfig.Definition.Name, alertName, true) == 0)
          return alertConfig;
      }
      return (AlertConfig) null;
    }

    public string GetAlertTypeDescription(int alertId)
    {
      if (Alerts.IsStandardAlert(alertId))
        return StandardAlerts.GetDefinition(alertId).Name;
      AlertConfig alertConfig = this.GetAlertConfig(alertId);
      return alertConfig != null ? alertConfig.Definition.Name : "Unknown Alert";
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Alerts
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class Alerts
  {
    public static bool IsStandardAlert(int alertId)
    {
      return StandardAlerts.GetDefinition(alertId) != null;
    }

    public static bool IsWorkflowAlert(int alertId)
    {
      AlertDefinition definition = StandardAlerts.GetDefinition(alertId);
      return definition != null && definition.Category == AlertCategory.Workflow;
    }

    public static bool IsCustomAlert(int alertId) => !Alerts.IsStandardAlert(alertId);

    public static bool IsRegulationAlert(int alertId)
    {
      AlertDefinition definition = StandardAlerts.GetDefinition(alertId);
      return definition != null && definition.Category == AlertCategory.Regulation;
    }

    public static AlertConfig GetAlertForCriterionName(
      string criterionName,
      AlertConfig[] alertConfigs)
    {
      foreach (AlertConfig alertConfig in alertConfigs)
      {
        if (string.Compare(alertConfig.Definition.GetCriterionName(), criterionName, true) == 0)
          return alertConfig;
      }
      return (AlertConfig) null;
    }

    public static AlertDefinition GetStandardAlertForCriterionName(string criterionName)
    {
      foreach (AlertDefinition forCriterionName in StandardAlerts.All)
      {
        if (string.Compare(forCriterionName.GetCriterionName(), criterionName, true) == 0)
          return forCriterionName;
      }
      return (AlertDefinition) null;
    }

    public static bool IsAlertCriterionName(string criterionName)
    {
      return criterionName.ToLower().StartsWith("alert.");
    }
  }
}

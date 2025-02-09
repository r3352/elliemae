// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.AlertPanels
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Log.Alerts;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public static class AlertPanels
  {
    public static Control GetAlertPanel(PipelineInfo.Alert alert)
    {
      switch ((StandardAlertID) alert.AlertID)
      {
        case StandardAlertID.RediscloseTILRateChange:
          return (Control) new RediscloseTILAlertPanel(alert);
        case StandardAlertID.InitialDisclosures:
          return (Control) new InitialDisclosureAlertPanel(alert);
        case StandardAlertID.HUD1ToleranceViolation:
          return (Control) new HUDToleranceViolationAlertPanel(alert);
        case StandardAlertID.ClosingDateViolation:
          return (Control) new ClosingDateViolationAlertPanel(alert);
        case StandardAlertID.RediscloseGFEChangedCircumstances:
          return (Control) new ChangedCircumstancesAlertPanel(alert);
        case StandardAlertID.AUSDataDiscrepancyAlert:
          return (Control) new AUSDataDiscrepencyAlertPanel(alert);
        case StandardAlertID.RediscloseLEChangedCircumstances:
          return (Control) new ChangedCircumstanceDisclosureRequirementsAlertPanel(alert);
        case StandardAlertID.RediscloseLERateLock:
          return (Control) new AtLockDisclosureRequirementsAlertPanel(alert);
        case StandardAlertID.RediscloseCDChangedCircumstances:
          return (Control) new ChangedCircumstancesAlertPanel(alert);
        case StandardAlertID.RediscloseCDAPR_Product_Prepay:
          return (Control) new RediscloseTILAlertPanel(alert);
        case StandardAlertID.GoodFaithFeeVarianceViolation:
          return (Control) new GoodFaithFeeVarianceViolationAlertFeeDetailPanel(alert);
        case StandardAlertID.eSignconsentNotYetReceived:
          return (Control) new eSignConsentAlertPanel(alert);
        case StandardAlertID.VADiscountChargeViolation:
          return (Control) new VADiscountChargeViolationAlertPanel(alert);
        case StandardAlertID.ThreeDayDisclosureRequirements:
          return (Control) new ThreeDayDisclosureRequirementsAlertPanel(alert);
        case StandardAlertID.AtAppDisclosureRequirements:
          return (Control) new AtAppDisclosureRequirementsAlertPanel(alert);
        default:
          if (EllieMae.EMLite.DataEngine.Alerts.IsRegulationAlert(alert.AlertID))
            return (Control) new AlertPanel(alert);
          return EllieMae.EMLite.DataEngine.Alerts.IsCustomAlert(alert.AlertID) ? (Control) new CustomAlertPanel(alert) : (Control) null;
      }
    }
  }
}

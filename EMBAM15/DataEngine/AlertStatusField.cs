// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.AlertStatusField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class AlertStatusField : VirtualField
  {
    private AlertStatusProperty property;
    private string alertName;

    internal AlertStatusField(AlertStatusProperty property, string description, FieldFormat format)
      : base("Log.Alert." + (object) property, description, format, FieldInstanceSpecifierType.CustomAlert)
    {
      this.property = property;
    }

    internal AlertStatusField(AlertStatusField parentField, string alertName)
      : base((VirtualField) parentField, (object) alertName)
    {
      this.property = parentField.property;
      this.alertName = alertName;
    }

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new AlertStatusField(this, instanceSpecifier.ToString());
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.AlertFields;

    protected override string Evaluate(LoanData loan)
    {
      AlertConfig alertConfigByName = loan.Settings.AlertSetupData.GetAlertConfigByName(this.alertName);
      if (alertConfigByName == null || !(alertConfigByName.Definition is CustomAlert definition))
        return "";
      AlertStatus alertStatus = loan.GetAlertStatus(definition.Guid);
      if (alertStatus == null)
        return "";
      switch (this.property)
      {
        case AlertStatusProperty.DateActivated:
          return !(alertStatus.ActivationTime == DateTime.MinValue) ? Utils.FormatDateValue(alertStatus.ActivationTime.ToString(), true) : "";
        case AlertStatusProperty.ActivatedBy:
          return alertStatus.ActivationUser ?? "";
        case AlertStatusProperty.DateCleared:
          return !(alertStatus.DismissalTime == DateTime.MinValue) ? Utils.FormatDateValue(alertStatus.DismissalTime.ToString(), true) : "";
        case AlertStatusProperty.ClearedBy:
          return alertStatus.DismissalUser ?? "";
        default:
          return "";
      }
    }

    public string Name => this.alertName;

    public AlertStatusProperty Property => this.property;
  }
}

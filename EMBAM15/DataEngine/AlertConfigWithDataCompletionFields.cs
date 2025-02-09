// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.AlertConfigWithDataCompletionFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class AlertConfigWithDataCompletionFields : AlertConfig
  {
    private AlertDataCompletionFieldCollection standardDataCompletionActiveFields = new AlertDataCompletionFieldCollection();
    private AlertDataCompletionFieldCollection customDataCompletionFields = new AlertDataCompletionFieldCollection();

    public AlertDataCompletionFieldCollection StandardDataCompletionActiveFields
    {
      get => this.standardDataCompletionActiveFields;
    }

    public AlertDataCompletionFieldCollection CustomDataCompletionFields
    {
      get => this.customDataCompletionFields;
    }

    public List<string> DataCompletionFields
    {
      get
      {
        List<string> completionFields = new List<string>();
        completionFields.AddRange(this.standardDataCompletionActiveFields.Select<AlertDataCompletionField, string>((Func<AlertDataCompletionField, string>) (a => a.FieldID)));
        completionFields.AddRange(this.customDataCompletionFields.Select<AlertDataCompletionField, string>((Func<AlertDataCompletionField, string>) (c => c.FieldID)));
        return completionFields;
      }
    }

    public AlertConfigWithDataCompletionFields(
      RegulationAlertWithDataCompletionFields alertDef,
      string message,
      int daysBefore,
      bool alertEnabled,
      bool notificationEnabled,
      List<string> milestoneGuidList,
      List<int> notificationRoleList,
      List<string> notificationUserList,
      List<string> triggerFieldList,
      AlertDataCompletionFieldCollection dataCompletionFields)
      : base((AlertDefinition) alertDef, message, daysBefore, alertEnabled, notificationEnabled, milestoneGuidList, notificationRoleList, notificationUserList, triggerFieldList)
    {
      if (dataCompletionFields == null || dataCompletionFields.Count <= 0)
        return;
      this.standardDataCompletionActiveFields.AddRange((IEnumerable<AlertDataCompletionField>) dataCompletionFields.FindAll((Predicate<AlertDataCompletionField>) (f => f.FieldType != 0)));
      this.customDataCompletionFields.AddRange((IEnumerable<AlertDataCompletionField>) dataCompletionFields.FindAll((Predicate<AlertDataCompletionField>) (f => f.FieldType == AlertDataCompletionFieldType.Custom)));
    }

    public bool AddField(string fieldID, out AlertDataCompletionField field)
    {
      field = (AlertDataCompletionField) null;
      if (this.IsExistingDataCompletionField(fieldID))
        return false;
      field = new AlertDataCompletionField(fieldID, AlertDataCompletionFieldType.Custom, false, false);
      this.customDataCompletionFields.AddField(field);
      return true;
    }

    public bool RemoveField(AlertDataCompletionField field)
    {
      if (field.ReadOnly)
        return false;
      if (this.standardDataCompletionActiveFields.Exists(field.FieldID))
        return this.standardDataCompletionActiveFields.RemoveField(field.FieldID);
      return this.customDataCompletionFields.Exists(field.FieldID) && this.customDataCompletionFields.RemoveField(field.FieldID);
    }

    public bool IsExistingDataCompletionField(string fieldID)
    {
      return this.standardDataCompletionActiveFields.Exists(fieldID) || this.customDataCompletionFields.Exists(fieldID);
    }

    public new object Clone()
    {
      AlertConfig alertConfig = (AlertConfig) base.Clone();
      AlertDataCompletionFieldCollection dataCompletionFields = new AlertDataCompletionFieldCollection();
      dataCompletionFields.AddRange((IEnumerable<AlertDataCompletionField>) this.standardDataCompletionActiveFields.Clone());
      dataCompletionFields.AddRange((IEnumerable<AlertDataCompletionField>) this.customDataCompletionFields.Clone());
      return (object) new AlertConfigWithDataCompletionFields(this.Definition is RegulationAlertWithDataCompletionFields definition ? (RegulationAlertWithDataCompletionFields) definition.Clone() : (RegulationAlertWithDataCompletionFields) (object) null, this.Message, this.DaysBefore, this.AlertEnabled, this.NotificationEnabled, alertConfig.MilestoneGuidList, alertConfig.NotificationRoleList, alertConfig.NotificationUserList, alertConfig.TriggerFieldList, dataCompletionFields);
    }
  }
}

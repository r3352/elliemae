// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.VirtualField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public abstract class VirtualField : FieldDefinition
  {
    private string description;
    private FieldFormat format;
    private string rolodex;

    protected VirtualField(string fieldId, string description, FieldFormat format)
      : this(fieldId, description, format, FieldInstanceSpecifierType.None)
    {
    }

    protected VirtualField(
      string fieldId,
      string description,
      FieldFormat format,
      FieldInstanceSpecifierType instanceSpecifierType)
      : base(fieldId, instanceSpecifierType)
    {
      this.description = description;
      this.format = format;
    }

    protected VirtualField(VirtualField parentField, object instanceSpecifier)
      : base((FieldDefinition) parentField, instanceSpecifier)
    {
      if (parentField.FieldID.StartsWith("Enhanced") || parentField.FieldID.StartsWith("ENHOP"))
      {
        string delimiterValue = "~%cbiz%~";
        if (!instanceSpecifier.ToString().Trim().Contains(delimiterValue))
          delimiterValue = ".";
        string[] strArray = this.deserializeInstanceSpecifier(instanceSpecifier.ToString(), delimiterValue);
        if (parentField.FieldID.StartsWith("ENHOP."))
        {
          if (strArray.Length >= 4)
            this.description = "List of " + this.findDispositionDescriptionType(strArray[2]) + " conditions where " + parentField.description + " is " + strArray[3] + ", of condition type " + strArray[1] + " and print type " + strArray[0];
        }
        else if (parentField.FieldID.StartsWith("Enhanced."))
        {
          if (strArray.Length >= 2)
            this.description = parentField.Description + " - of conditon type " + strArray[0] + " and condition name " + strArray[1];
          if (parentField.FieldID.StartsWith("Enhanced.TOW") && strArray.Length >= 3)
            this.description = this.description + " for role " + strArray[2];
          else if ((parentField.FieldID.StartsWith("Enhanced.ADBY") || parentField.FieldID.StartsWith("Enhanced.ADTE")) && strArray.Length >= 3)
            this.description = this.description + " for tracking status " + strArray[2];
        }
      }
      else
        this.description = parentField.Description + " - " + instanceSpecifier;
      this.format = parentField.Format;
    }

    private string findDispositionDescriptionType(string val)
    {
      string dispositionDescriptionType = "";
      if (string.IsNullOrEmpty(val))
        return dispositionDescriptionType;
      switch (val)
      {
        case "OP":
          dispositionDescriptionType = "Open";
          break;
        case "CL":
          dispositionDescriptionType = "Closed";
          break;
        case "OC":
          dispositionDescriptionType = "both Open and Closed";
          break;
      }
      return dispositionDescriptionType;
    }

    public override bool AllowEdit => false;

    public override FieldFormat Format
    {
      get => this.format;
      set => this.format = value;
    }

    public override bool AllowInReportingDatabase => true;

    public override string Description => this.description;

    public override string Rolodex
    {
      get => this.rolodex;
      set => this.rolodex = value;
    }

    protected abstract string Evaluate(LoanData loan);

    protected virtual string Evaluate(LoanData loan, string id) => "";

    protected virtual FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      throw new NotSupportedException("This operation is not supported by this field type");
    }

    public override string GetInstanceID(object instanceSpecifier)
    {
      if (!this.MultiInstance)
        throw new InvalidOperationException("The current field is not multi-instance");
      if (!this.FieldID.StartsWith("Enhanced") && !this.FieldID.StartsWith("ENHOP"))
        return this.FieldID + "." + instanceSpecifier;
      string[] strArray = this.deserializeInstanceSpecifier(instanceSpecifier.ToString(), "~%cbiz%~");
      if (!instanceSpecifier.ToString().Trim().Contains("~%cbiz%~"))
        return this.FieldID + "." + instanceSpecifier;
      string instanceId = this.FieldID;
      foreach (string str in strArray)
        instanceId = instanceId + "." + str;
      return instanceId;
    }

    public string[] deserializeInstanceSpecifier(string instanceSpecifier, string delimiterValue)
    {
      string[] separator = new string[1]{ delimiterValue };
      return instanceSpecifier.Split(separator, StringSplitOptions.None);
    }

    public override FieldDefinition CreateInstanceWithID(string fieldId)
    {
      if (!this.MultiInstance)
        throw new InvalidOperationException("The current field is not multi-instance");
      if (!fieldId.StartsWith(this.FieldID + ".", StringComparison.CurrentCultureIgnoreCase))
        return (FieldDefinition) null;
      return fieldId.Length < this.FieldID.Length + 2 ? (FieldDefinition) null : this.CreateInstanceFromSpecifier((object) fieldId.Substring(this.FieldID.Length + 1));
    }

    public override FieldDefinition CreateInstance(object instanceSpecifier)
    {
      if (!this.MultiInstance)
        throw new InvalidOperationException("The current field is not multi-instance");
      return this.CreateInstanceFromSpecifier(instanceSpecifier);
    }

    public override string GetValue(LoanData loan)
    {
      if (this.MultiInstance)
        throw new InvalidOperationException("Field is multi-instance");
      return this.Evaluate(loan);
    }

    public override string GetValue(LoanData loan, string id)
    {
      if (this.MultiInstance)
        throw new InvalidOperationException("Field is multi-instance");
      return this.Evaluate(loan, id);
    }

    public override void SetValue(LoanData loan, string value)
    {
      throw new InvalidOperationException("Cannot set value on virtual field");
    }

    public string FormatDate(DateTime date)
    {
      return date.Date == DateTime.MinValue.Date || date.Date == DateTime.MaxValue.Date ? "" : date.ToString("MM/dd/yyyy");
    }

    public string FormatDateTime(DateTime date)
    {
      return date.Date == DateTime.MinValue.Date || date.Date == DateTime.MaxValue.Date ? "" : date.ToString("MM/dd/yyyy hh:mm tt");
    }

    public string FormatBool(bool value) => !value ? "N" : "Y";

    internal static string GetMultiInstanceParentID(string fieldId)
    {
      for (int length = fieldId.LastIndexOf("."); length > 0; length = fieldId.LastIndexOf(".", length - 1))
      {
        string fieldId1 = fieldId.Substring(0, length);
        if (VirtualFields.Contains(fieldId1, false))
          return fieldId1;
      }
      return (string) null;
    }

    internal bool CheckEnhancedConditionType(EnhancedConditionLog enLog, string logType)
    {
      return string.Equals(enLog.EnhancedConditionType, logType, StringComparison.OrdinalIgnoreCase);
    }

    internal bool CheckEnhancedConditionStatus(EnhancedConditionLog log, string statusType)
    {
      foreach (StatusTrackingEntry statusTrackingEntry in log.Trackings.GetStatusTrackingEntries())
      {
        if (string.Equals(statusTrackingEntry.Status, statusType, StringComparison.OrdinalIgnoreCase))
          return true;
      }
      return false;
    }

    internal bool CheckEnhancedLogPriorTo(EnhancedConditionLog enLog, string priorToValue)
    {
      return string.Equals(enLog.PriorTo, priorToValue, StringComparison.OrdinalIgnoreCase);
    }
  }
}

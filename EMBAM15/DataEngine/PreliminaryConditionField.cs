// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.PreliminaryConditionField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class PreliminaryConditionField : VirtualField
  {
    private const string FieldPrefix = "PreliminaryCondition";
    private PreliminaryConditionProperty property;
    private bool allowInReportingDb = true;

    public PreliminaryConditionField(
      PreliminaryConditionProperty property,
      string description,
      FieldFormat format)
      : this(property, description, format, true)
    {
    }

    public PreliminaryConditionField(
      PreliminaryConditionProperty property,
      string description,
      FieldFormat format,
      bool allowInReportingDb)
      : base("PreliminaryCondition." + property.ToString(), description, format, FieldInstanceSpecifierType.PreliminaryCondition)
    {
      this.property = property;
      this.allowInReportingDb = allowInReportingDb;
    }

    public PreliminaryConditionField(PreliminaryConditionField parent, string documentTitle)
      : base((VirtualField) parent, (object) documentTitle)
    {
      this.property = parent.property;
      this.allowInReportingDb = parent.AllowInReportingDatabase;
    }

    public override VirtualFieldType VirtualFieldType
    {
      get => VirtualFieldType.PreliminaryConditionFields;
    }

    public override bool AllowInReportingDatabase => this.allowInReportingDb;

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new PreliminaryConditionField(this, string.Concat(instanceSpecifier));
    }

    protected override string Evaluate(LoanData loan)
    {
      PreliminaryConditionLog conditionLog = this.getConditionLog(loan);
      if (conditionLog == null)
        return "";
      switch (this.property)
      {
        case PreliminaryConditionProperty.Title:
          return conditionLog.Title;
        case PreliminaryConditionProperty.Category:
          return conditionLog.Category;
        case PreliminaryConditionProperty.Description:
          return conditionLog.Description;
        case PreliminaryConditionProperty.AddedBy:
          return conditionLog.AddedBy;
        case PreliminaryConditionProperty.DateAdded:
          return this.FormatDate(conditionLog.DateAdded);
        case PreliminaryConditionProperty.Fulfilled:
          return this.FormatBool(conditionLog.Fulfilled);
        case PreliminaryConditionProperty.FulfilledBy:
          return conditionLog.FulfilledBy;
        case PreliminaryConditionProperty.DateFulfilled:
          return this.FormatDate(conditionLog.DateFulfilled);
        case PreliminaryConditionProperty.Status:
          return conditionLog.StatusDescription;
        case PreliminaryConditionProperty.PriorTo:
          return conditionLog.PriorTo;
        case PreliminaryConditionProperty.Comments:
          return conditionLog.Comments.ToString();
        default:
          return "";
      }
    }

    private PreliminaryConditionLog getConditionLog(LoanData loan)
    {
      LogRecordBase[] allRecordsOfType = loan.GetLogList().GetAllRecordsOfType(typeof (PreliminaryConditionLog));
      PreliminaryConditionLog conditionLog = (PreliminaryConditionLog) null;
      foreach (PreliminaryConditionLog preliminaryConditionLog in allRecordsOfType)
      {
        if (string.Compare(preliminaryConditionLog.Title, string.Concat(this.InstanceSpecifier), true) == 0)
          conditionLog = preliminaryConditionLog;
      }
      return conditionLog;
    }

    public PreliminaryConditionProperty Property => this.property;
  }
}

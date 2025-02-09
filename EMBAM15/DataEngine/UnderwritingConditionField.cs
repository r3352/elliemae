// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.UnderwritingConditionField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class UnderwritingConditionField : VirtualField
  {
    private const string FieldPrefix = "Condition";
    private UnderwritingConditionProperty property;
    private bool allowInReportingDb = true;

    public UnderwritingConditionProperty Property => this.property;

    public UnderwritingConditionField(
      UnderwritingConditionProperty property,
      string description,
      FieldFormat format)
      : this(property, description, format, true)
    {
    }

    public UnderwritingConditionField(
      UnderwritingConditionProperty property,
      string description,
      FieldFormat format,
      bool allowInReportingDb)
      : base("Condition." + property.ToString(), description, format, FieldInstanceSpecifierType.UnderwritingCondition)
    {
      this.property = property;
      this.allowInReportingDb = allowInReportingDb;
    }

    public UnderwritingConditionField(UnderwritingConditionField parent, string documentTitle)
      : base((VirtualField) parent, (object) documentTitle)
    {
      this.property = parent.property;
      this.allowInReportingDb = parent.AllowInReportingDatabase;
    }

    public override VirtualFieldType VirtualFieldType
    {
      get => VirtualFieldType.UnderwritingConditionFields;
    }

    public override bool AllowInReportingDatabase => this.allowInReportingDb;

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new UnderwritingConditionField(this, string.Concat(instanceSpecifier));
    }

    protected override string Evaluate(LoanData loan)
    {
      UnderwritingConditionLog conditionLog = this.getConditionLog(loan);
      if (conditionLog == null)
        return "";
      switch (this.property)
      {
        case UnderwritingConditionProperty.Title:
          return conditionLog.Title;
        case UnderwritingConditionProperty.Category:
          return conditionLog.Category;
        case UnderwritingConditionProperty.Description:
          return conditionLog.Description;
        case UnderwritingConditionProperty.AddedBy:
          return conditionLog.AddedBy;
        case UnderwritingConditionProperty.DateAdded:
          return this.FormatDate(conditionLog.DateAdded);
        case UnderwritingConditionProperty.IsCleared:
          return this.FormatBool(conditionLog.Cleared);
        case UnderwritingConditionProperty.ClearedBy:
          return conditionLog.ClearedBy;
        case UnderwritingConditionProperty.DateCleared:
          return this.FormatDate(conditionLog.DateCleared);
        case UnderwritingConditionProperty.IsReceived:
          return this.FormatBool(conditionLog.Received);
        case UnderwritingConditionProperty.ReceivedBy:
          return conditionLog.ReceivedBy;
        case UnderwritingConditionProperty.DateReceived:
          return this.FormatDate(conditionLog.DateReceived);
        case UnderwritingConditionProperty.IsReviewed:
          return this.FormatBool(conditionLog.Reviewed);
        case UnderwritingConditionProperty.ReviewedBy:
          return conditionLog.ReviewedBy;
        case UnderwritingConditionProperty.DateReviewed:
          return this.FormatDate(conditionLog.DateReviewed);
        case UnderwritingConditionProperty.IsWaived:
          return this.FormatBool(conditionLog.Waived);
        case UnderwritingConditionProperty.WaivedBy:
          return conditionLog.WaivedBy;
        case UnderwritingConditionProperty.DateWaived:
          return this.FormatDate(conditionLog.DateWaived);
        case UnderwritingConditionProperty.Status:
          return conditionLog.StatusDescription;
        case UnderwritingConditionProperty.PriorTo:
          return conditionLog.PriorTo;
        case UnderwritingConditionProperty.Comments:
          return conditionLog.Comments.ToString();
        case UnderwritingConditionProperty.AllowToClear:
          return this.FormatBool(conditionLog.AllowToClear);
        case UnderwritingConditionProperty.IsInternal:
          return this.FormatBool(conditionLog.IsInternal);
        default:
          return "";
      }
    }

    private UnderwritingConditionLog getConditionLog(LoanData loan)
    {
      LogRecordBase[] allRecordsOfType = loan.GetLogList().GetAllRecordsOfType(typeof (UnderwritingConditionLog));
      UnderwritingConditionLog conditionLog = (UnderwritingConditionLog) null;
      foreach (UnderwritingConditionLog underwritingConditionLog in allRecordsOfType)
      {
        if (string.Compare(underwritingConditionLog.Title, string.Concat(this.InstanceSpecifier), true) == 0)
          conditionLog = underwritingConditionLog;
      }
      return conditionLog;
    }
  }
}

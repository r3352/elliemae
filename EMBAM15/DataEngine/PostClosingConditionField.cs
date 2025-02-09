// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.PostClosingConditionField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class PostClosingConditionField : VirtualField
  {
    private const string FieldPrefix = "PostCondition";
    private PostClosingConditionProperty property;

    public PostClosingConditionField(
      PostClosingConditionProperty property,
      string description,
      FieldFormat format)
      : base("PostCondition." + property.ToString(), description, format, FieldInstanceSpecifierType.PostClosingCondition)
    {
      this.property = property;
    }

    public PostClosingConditionField(PostClosingConditionField parent, string documentTitle)
      : base((VirtualField) parent, (object) documentTitle)
    {
      this.property = parent.property;
    }

    public override VirtualFieldType VirtualFieldType
    {
      get => VirtualFieldType.PostClosingConditionFields;
    }

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new PostClosingConditionField(this, string.Concat(instanceSpecifier));
    }

    protected override string Evaluate(LoanData loan)
    {
      PostClosingConditionLog conditionLog = this.getConditionLog(loan);
      if (conditionLog == null)
        return "";
      switch (this.property)
      {
        case PostClosingConditionProperty.Title:
          return conditionLog.Title;
        case PostClosingConditionProperty.Source:
          return conditionLog.Source;
        case PostClosingConditionProperty.Description:
          return conditionLog.Description;
        case PostClosingConditionProperty.AddedBy:
          return conditionLog.AddedBy;
        case PostClosingConditionProperty.DateAdded:
          return this.FormatDate(conditionLog.DateAdded);
        case PostClosingConditionProperty.IsCleared:
          return this.FormatBool(conditionLog.Cleared);
        case PostClosingConditionProperty.ClearedBy:
          return conditionLog.ClearedBy;
        case PostClosingConditionProperty.DateCleared:
          return this.FormatDate(conditionLog.DateCleared);
        case PostClosingConditionProperty.IsReceived:
          return this.FormatBool(conditionLog.Received);
        case PostClosingConditionProperty.ReceivedBy:
          return conditionLog.ReceivedBy;
        case PostClosingConditionProperty.DateReceived:
          return this.FormatDate(conditionLog.DateReceived);
        case PostClosingConditionProperty.IsRequested:
          return this.FormatBool(conditionLog.Requested);
        case PostClosingConditionProperty.RequestedBy:
          return conditionLog.RequestedBy;
        case PostClosingConditionProperty.DateRequested:
          return this.FormatDate(conditionLog.DateRequested);
        case PostClosingConditionProperty.IsRerequested:
          return this.FormatBool(conditionLog.Rerequested);
        case PostClosingConditionProperty.RerequestedBy:
          return conditionLog.RerequestedBy;
        case PostClosingConditionProperty.DateRerequested:
          return this.FormatDate(conditionLog.DateRerequested);
        case PostClosingConditionProperty.IsSent:
          return this.FormatBool(conditionLog.Sent);
        case PostClosingConditionProperty.SentBy:
          return conditionLog.SentBy;
        case PostClosingConditionProperty.DateSent:
          return this.FormatDate(conditionLog.DateSent);
        case PostClosingConditionProperty.Recipient:
          return conditionLog.Recipient;
        case PostClosingConditionProperty.DateExpected:
          return this.FormatDate(conditionLog.DateExpected);
        case PostClosingConditionProperty.Status:
          return conditionLog.StatusDescription;
        case PostClosingConditionProperty.Comments:
          return conditionLog.Comments.ToString();
        case PostClosingConditionProperty.RequestedFrom:
          return conditionLog.RequestedFrom;
        case PostClosingConditionProperty.PrintInternally:
          return this.FormatBool(conditionLog.IsInternal);
        case PostClosingConditionProperty.PrintExternally:
          return this.FormatBool(conditionLog.IsExternal);
        default:
          return "";
      }
    }

    public override bool AppliesToEdition(EncompassEdition edition)
    {
      return edition == EncompassEdition.Banker;
    }

    private PostClosingConditionLog getConditionLog(LoanData loan)
    {
      LogRecordBase[] allRecordsOfType = loan.GetLogList().GetAllRecordsOfType(typeof (PostClosingConditionLog));
      PostClosingConditionLog conditionLog = (PostClosingConditionLog) null;
      foreach (PostClosingConditionLog closingConditionLog in allRecordsOfType)
      {
        if (string.Compare(closingConditionLog.Title, string.Concat(this.InstanceSpecifier), true) == 0)
          conditionLog = closingConditionLog;
      }
      return conditionLog;
    }

    public PostClosingConditionProperty Property => this.property;
  }
}

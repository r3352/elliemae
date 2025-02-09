// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DocumentTrackingField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class DocumentTrackingField : VirtualField
  {
    private const string FieldPrefix = "Document";
    private DocumentTrackingProperty property;

    public DocumentTrackingField(
      DocumentTrackingProperty property,
      string description,
      FieldFormat format)
      : base("Document." + property.ToString(), description, format, FieldInstanceSpecifierType.Document)
    {
      this.property = property;
    }

    public DocumentTrackingField(DocumentTrackingField parent, string documentTitle)
      : base((VirtualField) parent, (object) documentTitle)
    {
      this.property = parent.property;
    }

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new DocumentTrackingField(this, string.Concat(instanceSpecifier));
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.DocumentTrackingFields;

    protected override string Evaluate(LoanData loan)
    {
      return this.EvaluateForDocumentLog(this.GetDocumentLog(loan, (LogRecordBase[]) null));
    }

    public string EvaluateUsingCachedDocumentLogs(LogRecordBase[] cachedDocumentLogs)
    {
      return this.EvaluateForDocumentLog(this.GetDocumentLog((LoanData) null, cachedDocumentLogs));
    }

    private string EvaluateForDocumentLog(DocumentLog doc)
    {
      if (doc == null)
        return "";
      switch (this.property)
      {
        case DocumentTrackingProperty.Title:
          return doc.Title;
        case DocumentTrackingProperty.Milestone:
          return doc.Stage;
        case DocumentTrackingProperty.Company:
          return doc.RequestedFrom;
        case DocumentTrackingProperty.DateOrdered:
          return this.FormatDate(doc.DateRequested);
        case DocumentTrackingProperty.DateReordered:
          return this.FormatDate(doc.DateRerequested);
        case DocumentTrackingProperty.DateReceived:
          return this.FormatDate(doc.DateReceived);
        case DocumentTrackingProperty.DaysToReceived:
          return string.Concat((object) doc.DaysDue);
        case DocumentTrackingProperty.ExpectedDate:
          return this.FormatDate(doc.DateExpected);
        case DocumentTrackingProperty.DaysToExpiration:
          return string.Concat((object) doc.DaysTillExpire);
        case DocumentTrackingProperty.ExpirationDate:
          return this.FormatDate(doc.DateExpires);
        case DocumentTrackingProperty.Status:
          return doc.Status;
        case DocumentTrackingProperty.Comments:
          return doc.Comments.ToString();
        case DocumentTrackingProperty.DateUnderwritingReady:
          return this.FormatDate(doc.DateUnderwritingReady);
        case DocumentTrackingProperty.DateShippingReady:
          return this.FormatDate(doc.DateShippingReady);
        case DocumentTrackingProperty.DateReviewed:
          return this.FormatDate(doc.DateReviewed);
        default:
          return "";
      }
    }

    public DocumentLog GetDocumentLog(LoanData loan, LogRecordBase[] cachedDocumentLogs)
    {
      string title = this.InstanceSpecifier.ToString();
      DocumentLog[] documentLogArray = cachedDocumentLogs != null ? LogList.GetDocumentsByTitle(title, cachedDocumentLogs) : loan.GetLogList().GetDocumentsByTitle(title);
      return documentLogArray.Length == 0 ? (DocumentLog) null : documentLogArray[documentLogArray.Length - 1];
    }

    public static FieldFormat GetPropertyFormat(DocumentTrackingProperty property)
    {
      switch (property)
      {
        case DocumentTrackingProperty.DateOrdered:
        case DocumentTrackingProperty.DateReordered:
        case DocumentTrackingProperty.DateReceived:
        case DocumentTrackingProperty.ExpectedDate:
        case DocumentTrackingProperty.ExpirationDate:
        case DocumentTrackingProperty.DateUnderwritingReady:
        case DocumentTrackingProperty.DateShippingReady:
        case DocumentTrackingProperty.DateReviewed:
          return FieldFormat.DATE;
        case DocumentTrackingProperty.DaysToReceived:
        case DocumentTrackingProperty.DaysToExpiration:
          return FieldFormat.INTEGER;
        default:
          return FieldFormat.STRING;
      }
    }

    public DocumentTrackingProperty Property => this.property;
  }
}

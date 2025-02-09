// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DocumentTrackingFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class DocumentTrackingFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static DocumentTrackingFields()
    {
      DocumentTrackingFields.All.Add((FieldDefinition) new DocumentTrackingField(DocumentTrackingProperty.Title, "Document Title", FieldFormat.STRING));
      DocumentTrackingFields.All.Add((FieldDefinition) new DocumentTrackingField(DocumentTrackingProperty.Milestone, "Document Milestone", FieldFormat.STRING));
      DocumentTrackingFields.All.Add((FieldDefinition) new DocumentTrackingField(DocumentTrackingProperty.Company, "Document Company", FieldFormat.STRING));
      DocumentTrackingFields.All.Add((FieldDefinition) new DocumentTrackingField(DocumentTrackingProperty.DateOrdered, "Document Date Ordered", FieldFormat.DATE));
      DocumentTrackingFields.All.Add((FieldDefinition) new DocumentTrackingField(DocumentTrackingProperty.DateReordered, "Document Date Reordered", FieldFormat.DATE));
      DocumentTrackingFields.All.Add((FieldDefinition) new DocumentTrackingField(DocumentTrackingProperty.DateReceived, "Document Date Received", FieldFormat.DATE));
      DocumentTrackingFields.All.Add((FieldDefinition) new DocumentTrackingField(DocumentTrackingProperty.DaysToReceived, "Document Days To Received", FieldFormat.STRING));
      DocumentTrackingFields.All.Add((FieldDefinition) new DocumentTrackingField(DocumentTrackingProperty.ExpectedDate, "Document Expected Date", FieldFormat.DATE));
      DocumentTrackingFields.All.Add((FieldDefinition) new DocumentTrackingField(DocumentTrackingProperty.DaysToExpiration, "Document Days To Expiration", FieldFormat.STRING));
      DocumentTrackingFields.All.Add((FieldDefinition) new DocumentTrackingField(DocumentTrackingProperty.ExpirationDate, "Document Expiration Date", FieldFormat.DATE));
      DocumentTrackingFields.All.Add((FieldDefinition) new DocumentTrackingField(DocumentTrackingProperty.Status, "Document Status", FieldFormat.STRING));
      DocumentTrackingFields.All.Add((FieldDefinition) new DocumentTrackingField(DocumentTrackingProperty.Comments, "Document Comments", FieldFormat.STRING));
      DocumentTrackingFields.All.Add((FieldDefinition) new DocumentTrackingField(DocumentTrackingProperty.DateUnderwritingReady, "Document Date Underwriting Ready", FieldFormat.DATE));
      DocumentTrackingFields.All.Add((FieldDefinition) new DocumentTrackingField(DocumentTrackingProperty.DateShippingReady, "Document Date Shipping Ready", FieldFormat.DATE));
      DocumentTrackingFields.All.Add((FieldDefinition) new DocumentTrackingField(DocumentTrackingProperty.DateReviewed, "Document Date Reviewed", FieldFormat.DATE));
    }
  }
}

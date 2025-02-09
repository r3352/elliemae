// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.EDisclosureTrackingFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class EDisclosureTrackingFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static EDisclosureTrackingFields()
    {
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.Comments, "Disclosure Comments", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.DisclosureMethod, "Disclosure Method", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.FulfilledBy, "Disclosure Fullfilled By", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.SentDate, "eDisclosure Date Time Sent", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.BorrowerDateViewMessage, "eDisclosure Date Borrowers Viewed Message", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.BorrowerDateAccepted, "eDisclosure Date Borrowers Accepted Consent Form", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.BorrowerDateRejected, "eDisclosure Date Borrowers Rejected Consent Form", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.BorrowerDateESigned, "eDisclosure Date Borrowers eSigned Disclosures", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.CoBorrowerDateViewMessage, "eDisclosure Date CoBorrowers Viewed Message", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.CoBorrowerDateAccepted, "eDisclosure Date CoBorrowers Accepted Consent Form", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.CoBorrowerDateRejected, "eDisclosure Date CoBorrowers Rejected Consent Form", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.CoBorrowerDateESigned, "eDisclosure Date CoBorrowers eSigned Disclosures", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.DateTimeFulfilled, "eDisclosure Date Time Fulfilled", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.FulfillmentMethod, "eDisclosure Fulfillment Method", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.FulfillmentComment, "eDisclosure Fulfillment Comment", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.FulfillmentOrderedBy, "eDisclosure Fulfillment Fullfilled By", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.DisclosureCount, "eDisclosure Count", FieldFormat.INTEGER, FieldInstanceSpecifierType.None));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.BorrowerName, "eDisclosure Borrower's Name", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.BorrowerEmailAddress, "eDisclosure Borrower's Email Address", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.BorrowerIPAddressAccepted, "eDisclosure IP Address Borrowers Accepted Consent Form", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.BorrowerIPAddressRejected, "eDisclosure IP Address Borrowers Rejected Consent Form", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.BorrowerDateAuthenticated, "eDisclosure Date Borrowers Authenticated", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.BorrowerIPAddressAuthenticated, "eDisclosure IP Address Borrowers Authenticated", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.BorrowerIPAddressESigned, "eDisclosure IP Address Borrowers eSigned Disclosures", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.CoBorrowerName, "eDisclosure CoBorrower's Name", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.CoBorrowerEmailAddress, "eDisclosure CoBorrower's Email Address", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.CoBorrowerIPAddressAccepted, "eDisclosure IP Address CoBorrowers Accepted Consent Form", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.CoBorrowerIPAddressRejected, "eDisclosure IP Address CoBorrowers Rejected Consent Form", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.CoBorrowerDateAuthenticated, "eDisclosure Date CoBorrowers Authenticated", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.CoBorrowerIPAddressAuthenticated, "eDisclosure IP Address CoBorrowers Authenticated", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.CoBorrowerIPAddressESigned, "eDisclosure IP Address CoBorrowers eSigned Disclosures", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.LoanOriginatorName, "eDisclosure Loan Originator's Name", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.LoanOriginatorDateViewMessage, "eDisclosure Date Loan Originator Viewed Message", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.LoanOriginatorDateESigned, "eDisclosure Date Loan Originator eSigned Disclosures", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.LoanOriginatorIPAddressESigned, "eDisclosure IP Address Loan Originator eSigned Disclosures", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.BorrowerDocumentViewedDate, "eDisclosure Borrower Document Viewed date", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.CoBorrowerDocumentViewedDate, "eDisclosure Co-Borrower Document Viewed date", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.FulfillmentTrackingNumber, "eDisclosure Fulfillment Tracking Number", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.BorrowerInformationalViewedDate, "eDisclosure Borrower Informational Viewed date", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.BorrowerInformationalViewedIP, "eDisclosure Borrower Informational Viewed IP", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.BorrowerInformationalCompletedDate, "eDisclosure Borrower Informational Completed date", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.BorrowerInformationalCompletedIP, "eDisclosure Borrower Informational Completed IP", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.CoBorrowerInformationalViewedDate, "eDisclosure Co-Borrower Informational Viewed date", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.CoBorrowerInformationalViewedIP, "eDisclosure Co-Borrower Informational Viewed IP", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.CoBorrowerInformationalCompletedDate, "eDisclosure Co-Borrower Informational Completed date", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.CoBorrowerInformationalCompletedIP, "eDisclosure Co-Borrower Informational Completed IP", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.LoanOriginatorInformationalViewedDate, "eDisclosure Loan Originator Informational Viewed date", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.LoanOriginatorInformationalViewedIP, "eDisclosure Loan Originator Informational Viewed IP", FieldFormat.STRING));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.LoanOriginatorInformationalCompletedDate, "eDisclosure Loan Originator Informational Completed date", FieldFormat.DATETIME));
      EDisclosureTrackingFields.All.Add((FieldDefinition) new EDisclosureTrackingField(EDisclosureProperty.LoanOriginatorInformationalCompletedIP, "eDisclosure Loan Originator Informational Completed IP", FieldFormat.STRING));
    }
  }
}

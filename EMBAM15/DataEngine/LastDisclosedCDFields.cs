// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LastDisclosedCDFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class LastDisclosedCDFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static LastDisclosedCDFields()
    {
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.DisclosureType, "CD Disclosure Type", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.SentDate, "CD Sent Date", FieldFormat.DATE));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.SentBy, "CD Sent By", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.SentMethod, "CD Sent Method", FieldFormat.STRING, new string[6]
      {
        "U.S. Mail",
        "In Person",
        "Email",
        "eFolder eDisclosures",
        "Fax",
        "Other"
      }));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.OtherMethod, "CD Other Method", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.BorrowerReceivedMethod, "CD Borrower Received Method", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.BorrowerReceivedOtherMethod, "CD Borrower Received Other Method", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.BorrowerPresumedReceivedDate, "CD Borrower Presumed Received Date", FieldFormat.DATE));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.BorrowerActualReceivedDate, "CD Borrower Actual Received Date", FieldFormat.DATE));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.BorrowerType, "CD Borrower Type", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.CoBorrowerReceivedMethod, "CD CoBorrower Received Method", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.CoBorrowerReceivedOtherMethod, "CD CoBorrower Received Other Method", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.CoBorrowerPresumedReceivedDate, "CD CoBorrower Presumed Received Date", FieldFormat.DATE));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.CoBorrowerActualReceivedDate, "CD CoBorrower Actual Received Date", FieldFormat.DATE));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.CoBorrowerType, "CD CoBorrower Type", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.LoanSnapshot_DisclosedAPR, "CD Loan Snapshot - Disclosed APR", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.LoanSnapshot_DisclosedRate, "CD Loan Snapshot - Disclosed Rate", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.LoanSnapshot_LoanProgram, "CD Loan Snapshot - Loan Program", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.LoanSnapshot_LoanAmount, "CD Loan Snapshot - Loan Amount", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.LoanSnapshot_FinanceCharge, "CD Loan Snapshot - Finance Charge", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.LoanSnapshot_ApplicationDate, "CD Loan Snapshot - Application Date", FieldFormat.DATE));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.ReasonChangeinAPR, "CD Reason - Change in APR?", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.ReasonChangeinLoanProduct, "CD Reason - Change in Loan Product?", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.ReasonPrepaymentPenaltyAdded, "CD Reason - Prepayment Penalty Added?", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.ReasonChangeinSettlementCharges, "CD Reason - Change in Settlement Charges?", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.ReasonChangedCircumstanceEligibility, "CD Reason - Changed Circumstance - Eligibility?", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.Reason_RevisionsrequestedbytheConsumer, "CD Reason - Revisions requested by the Consumer?", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.Reason_InterestRatedependentcharges, "CD Reason - Interest Rate dependent charges?", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.Reason24hourAdvancedPreview, "CD Reason - 24-hour Advanced Preview?", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.ReasonToleranceCure, "CD Reason - Tolerance Cure?", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.ReasonClericalErrorCorrection, "CD Reason - Clerical Error Correction?", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.Reason_Other, "CD Reason - Other?", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.OtherReason, "CD Other Reason", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.ChangedCircumstanceDetails, "CD Changed Circumstance Details", FieldFormat.STRING));
      LastDisclosedCDFields.All.Add((FieldDefinition) new CDDisclosureField(DisclosureProperty2015.ReasonComments, "CD Reason Comments", FieldFormat.STRING));
    }
  }
}

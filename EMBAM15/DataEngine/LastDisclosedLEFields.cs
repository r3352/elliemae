// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LastDisclosedLEFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class LastDisclosedLEFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static LastDisclosedLEFields()
    {
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.DisclosedbyBroker, "LE Disclosed by Broker?", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.DisclosureType, "LE Disclosure Type", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.SentDate, "LE Sent Date", FieldFormat.DATE));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.SentBy, "LE Sent By", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.SentMethod, "LE Sent Method", FieldFormat.STRING, new string[6]
      {
        "U.S. Mail",
        "In Person",
        "Email",
        "eFolder eDisclosures",
        "Fax",
        "Other"
      }));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.OtherMethod, "LE Other Method", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.IntenttoProceed, "LE Intent to Proceed?", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.IntentDate, "LE Intent Date", FieldFormat.DATE));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.IntentReceivedBy, "LE Intent Received By", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.IntentReceivedMethod, "LE Intent Received Method", FieldFormat.STRING, new string[6]
      {
        "U.S. Mail",
        "In Person",
        "Email",
        "eFolder eDisclosures",
        "Fax",
        "Other"
      }));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.IntentReceivedOtherMethod, "LE Intent Received Other Method", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.IntentComments, "LE Intent Comments", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.BorrowerReceivedMethod, "LE Borrower Received Method", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.BorrowerReceivedOtherMethod, "LE Borrower Received Other Method", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.BorrowerPresumedReceivedDate, "LE Borrower Presumed Received Date", FieldFormat.DATE));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.BorrowerActualReceivedDate, "LE Borrower Actual Received Date", FieldFormat.DATE));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.BorrowerType, "LE Borrower Type", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.CoBorrowerReceivedMethod, "LE CoBorrower Received Method", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.CoBorrowerReceivedOtherMethod, "LE CoBorrower Received Other Method", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.CoBorrowerPresumedReceivedDate, "LE CoBorrower Presumed Received Date", FieldFormat.DATE));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.CoBorrowerActualReceivedDate, "LE CoBorrower Actual Received Date", FieldFormat.DATE));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.CoBorrowerType, "LE CoBorrower Type", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.LoanSnapshot_DisclosedAPR, "LE Loan Snapshot - Disclosed APR", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.LoanSnapshot_DisclosedRate, "LE Loan Snapshot - Disclosed Rate", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.LoanSnapshot_LoanProgram, "LE Loan Snapshot - Loan Program", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.LoanSnapshot_LoanAmount, "LE Loan Snapshot - Loan Amount", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.LoanSnapshot_FinanceCharge, "LE Loan Snapshot - Finance Charge", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.LoanSnapshot_ApplicationDate, "LE Loan Snapshot - Application Date", FieldFormat.DATE));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.Reason_ChangedCircumstance_SettmentCharges, "LE Reason - Changed Circumstance - Settlement Charges?", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.Reason_ChangedCircumstance_Eligibility, "LE Reason - Changed Circumstance - Eligibility?", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.Reason_RevisionsrequestedbytheConsumer, "LE Reason - Revisions requested by the Consumer?", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.Reason_InterestRatedependentcharges, "LE Reason - Interest Rate dependent charges?", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.Reason_Expiration, "LE Reason - Expiration?", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.Reason_DelayedSettmentonConstructionLoans, "LE Reason - Delayed Settlement on Construction Loans?", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.Reason_Other, "LE Reason - Other?", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.OtherReason, "LE Other Reason", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.ChangedCircumstanceDetails, "LE Changed Circumstance Details", FieldFormat.STRING));
      LastDisclosedLEFields.All.Add((FieldDefinition) new LEDisclosureField(DisclosureProperty2015.ReasonComments, "LE Reason Comments", FieldFormat.STRING));
    }
  }
}

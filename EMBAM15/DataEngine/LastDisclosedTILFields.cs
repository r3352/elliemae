// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LastDisclosedTILFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class LastDisclosedTILFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static LastDisclosedTILFields()
    {
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.SentDate, "TIL Disclosure Sent Date", FieldFormat.DATE));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.DisclosedBy, "TIL Disclosure Disclosed By", FieldFormat.STRING));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.DeliveryMethod, "TIL Disclosure Delivery Method", FieldFormat.STRING, new string[5]
      {
        "U.S. Mail",
        "In Person",
        "Other",
        "Fax",
        "eFolder eDisclosures"
      }));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.ReceivedDate, "TIL Disclosure Received Date", FieldFormat.DATETIME));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.Comments, "TIL Disclosure Comments", FieldFormat.STRING));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.BorrowerName, "TIL Disclosure Borrower Name", FieldFormat.STRING));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.CoBorrowerName, "TIL Disclosure CoBorrower Name", FieldFormat.STRING));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.PropertyAddress, "TIL Disclosure Property Address", FieldFormat.STRING));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.PropertyCity, "TIL Disclosure Property City", FieldFormat.STRING));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.PropertyState, "TIL Disclosure Property State", FieldFormat.STRING));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.PropertyZip, "TIL Disclosure Property Zip", FieldFormat.STRING));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.DisclosedAPR, "TIL Disclosure Disclosed APR", FieldFormat.STRING));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.LoanProgram, "TIL Disclosure Loan Program", FieldFormat.STRING));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.LoanAmount, "TIL Disclosure Loan Amount", FieldFormat.STRING));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.FinanceCharge, "TIL Disclosure Finance Charge", FieldFormat.STRING));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.ApplicationDate, "TIL Disclosure Application Date", FieldFormat.DATE));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.ExternalDisclosedAPRField, "TIL External Disclosure APR", FieldFormat.DECIMAL_3));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.ExternalFinanceChargeField, "TIL External Disclosure Finance Charge", FieldFormat.DECIMAL_2));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.ExternalByField, "TIL External Disclosure Disclosed By", FieldFormat.STRING));
      LastDisclosedTILFields.All.Add((FieldDefinition) new TILDisclosureField(DisclosureProperty.ExternalDisclosedMethod, "TIL External Disclosure Method", FieldFormat.STRING));
    }
  }
}

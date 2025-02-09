// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LastDisclosedGFEFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class LastDisclosedGFEFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static LastDisclosedGFEFields()
    {
      foreach (string fieldId in DisclosureTrackingLog.HUDGFEFIELDS)
      {
        FieldDefinition baseField = StandardFields.All[fieldId];
        if (baseField != null)
          LastDisclosedGFEFields.All.Add((FieldDefinition) new LastDisclosedGFEField(baseField));
      }
      LastDisclosedGFEFields.All.Add((FieldDefinition) new GFEDisclosureField(DisclosureProperty.SentDate, "GFE Disclosure Sent Date", FieldFormat.DATE));
      LastDisclosedGFEFields.All.Add((FieldDefinition) new GFEDisclosureField(DisclosureProperty.DisclosedBy, "GFE Disclosure Disclosed By", FieldFormat.STRING));
      LastDisclosedGFEFields.All.Add((FieldDefinition) new GFEDisclosureField(DisclosureProperty.DeliveryMethod, "GFE Disclosure Delivery Method", FieldFormat.STRING, new string[5]
      {
        "U.S. Mail",
        "In Person",
        "Other",
        "Fax",
        "eFolder eDisclosures"
      }));
      LastDisclosedGFEFields.All.Add((FieldDefinition) new GFEDisclosureField(DisclosureProperty.ReceivedDate, "GFE Disclosure Received Date", FieldFormat.DATETIME));
      LastDisclosedGFEFields.All.Add((FieldDefinition) new GFEDisclosureField(DisclosureProperty.Comments, "GFE Disclosure Comments", FieldFormat.STRING));
      LastDisclosedGFEFields.All.Add((FieldDefinition) new GFEDisclosureField(DisclosureProperty.BorrowerName, "GFE Disclosure Borrower Name", FieldFormat.STRING));
      LastDisclosedGFEFields.All.Add((FieldDefinition) new GFEDisclosureField(DisclosureProperty.CoBorrowerName, "GFE Disclosure CoBorrower Name", FieldFormat.STRING));
      LastDisclosedGFEFields.All.Add((FieldDefinition) new GFEDisclosureField(DisclosureProperty.PropertyAddress, "GFE Disclosure Property Address", FieldFormat.STRING));
      LastDisclosedGFEFields.All.Add((FieldDefinition) new GFEDisclosureField(DisclosureProperty.PropertyCity, "GFE Disclosure Property City", FieldFormat.STRING));
      LastDisclosedGFEFields.All.Add((FieldDefinition) new GFEDisclosureField(DisclosureProperty.PropertyState, "GFE Disclosure Property State", FieldFormat.STRING));
      LastDisclosedGFEFields.All.Add((FieldDefinition) new GFEDisclosureField(DisclosureProperty.PropertyZip, "GFE Disclosure Property Zip", FieldFormat.STRING));
      LastDisclosedGFEFields.All.Add((FieldDefinition) new GFEDisclosureField(DisclosureProperty.LoanProgram, "GFE Disclosure Loan Program", FieldFormat.STRING));
      LastDisclosedGFEFields.All.Add((FieldDefinition) new GFEDisclosureField(DisclosureProperty.LoanAmount, "GFE Disclosure Loan Amount", FieldFormat.STRING));
      LastDisclosedGFEFields.All.Add((FieldDefinition) new GFEDisclosureField(DisclosureProperty.FinanceCharge, "GFE Disclosure Finance Charge", FieldFormat.STRING));
      LastDisclosedGFEFields.All.Add((FieldDefinition) new GFEDisclosureField(DisclosureProperty.ApplicationDate, "GFE Disclosure Application Date", FieldFormat.DATE));
    }
  }
}

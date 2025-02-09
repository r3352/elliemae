// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanAssociateFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class LoanAssociateFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static LoanAssociateFields()
    {
      LoanAssociateFields.All.Add((FieldDefinition) new CurrentLoanAssociateField());
      LoanAssociateFields.All.Add((FieldDefinition) new CurrentLoanAssociateNameField());
      LoanAssociateFields.All.Add((FieldDefinition) new CurrentLoanAssociateTitleField());
      LoanAssociateFields.All.Add((FieldDefinition) new CurrentLoanAssociateAPIClientIDField());
      LoanAssociateFields.All.Add((FieldDefinition) new PreviousLoanAssociateField());
      LoanAssociateFields.All.Add((FieldDefinition) new PreviousLoanAssociateNameField());
      LoanAssociateFields.All.Add((FieldDefinition) new PreviousLoanAssociateTitleField());
      LoanAssociateFields.All.Add((FieldDefinition) new PreviousLoanAssociateAPIClientIDField());
      LoanAssociateFields.All.Add((FieldDefinition) new LoanAssociateField(LoanAssociateProperty.UserID, "Loan Team Member User ID", FieldFormat.STRING));
      LoanAssociateFields.All.Add((FieldDefinition) new LoanAssociateField(LoanAssociateProperty.Name, "Loan Team Member Name", FieldFormat.STRING));
      LoanAssociateFields.All.Add((FieldDefinition) new LoanAssociateField(LoanAssociateProperty.Title, "Loan Team Member Title", FieldFormat.STRING));
      LoanAssociateFields.All.Add((FieldDefinition) new LoanAssociateField(LoanAssociateProperty.Email, "Loan Team Member Email", FieldFormat.STRING));
      LoanAssociateFields.All.Add((FieldDefinition) new LoanAssociateField(LoanAssociateProperty.Phone, "Loan Team Member Phone", FieldFormat.STRING));
      LoanAssociateFields.All.Add((FieldDefinition) new LoanAssociateField(LoanAssociateProperty.Cell, "Loan Team Member Cell Phone", FieldFormat.STRING));
      LoanAssociateFields.All.Add((FieldDefinition) new LoanAssociateField(LoanAssociateProperty.Fax, "Loan Team Member Fax", FieldFormat.STRING));
      LoanAssociateFields.All.Add((FieldDefinition) new LoanAssociateField(LoanAssociateProperty.AssociateType, "Loan Team Member Type", new string[2]
      {
        "User",
        "Group"
      }));
      LoanAssociateFields.All.Add((FieldDefinition) new LoanAssociateField(LoanAssociateProperty.LoanAssociateAccess, "Loan Team Member Access", FieldFormat.YN));
      LoanAssociateFields.All.Add((FieldDefinition) new LoanAssociateField(LoanAssociateProperty.APIClientID, "Loan Team Member API Client ID", FieldFormat.STRING));
    }
  }
}

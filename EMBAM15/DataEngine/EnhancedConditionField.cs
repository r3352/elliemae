// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.EnhancedConditionField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class EnhancedConditionField
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static EnhancedConditionField()
    {
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.NAME, "Condition Name", FieldFormat.STRING));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.IDES, "Internal Description", FieldFormat.STRING));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.EDES, "External Description", FieldFormat.STRING));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.PAIR, "For Borrower Pair", FieldFormat.STRING));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.TYPE, "Condition Type", FieldFormat.STRING));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.SRC, "Source", FieldFormat.STRING));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.RDTL, "Recipient Details", FieldFormat.STRING));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.PTO, "Prior To", FieldFormat.STRING));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.CAT, "Category", FieldFormat.STRING));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.SCON, "Source Of Condition", FieldFormat.STRING));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.SDTE, "Effective Start Date", FieldFormat.DATE));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.EDTE, "Effective End Date", FieldFormat.DATE));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.IID, "Internal ID", FieldFormat.STRING));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.EID, "External ID", FieldFormat.STRING));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.PINT, "Print Internally", FieldFormat.YN));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.PEXT, "Print Externally", FieldFormat.YN));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.TOW, "Tracking Owner", FieldFormat.STRING));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.ICOM, "Internal Comment", FieldFormat.STRING));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.ECOM, "External Comment", FieldFormat.STRING));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.ADBY, "Status Added by User", FieldFormat.STRING));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.ADTE, "Date of Status Added", FieldFormat.DATETIME));
      EnhancedConditionField.All.Add((FieldDefinition) new EnhancedConditionSingleAttributeField(EnhancedConditionSingleProperty.OWN, "Condition Owner", FieldFormat.STRING));
    }
  }
}

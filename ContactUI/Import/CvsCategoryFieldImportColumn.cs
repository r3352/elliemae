// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.CvsCategoryFieldImportColumn
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI.CustomFields;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class CvsCategoryFieldImportColumn : CsvImportColumn
  {
    private CustomFieldsType customFieldsType;
    private string categoryName = string.Empty;
    private CustomFieldDefinition fieldDefinition;

    public CustomFieldsType CustomFieldsType => this.customFieldsType;

    public int FieldId => this.fieldDefinition.FieldId;

    public FieldFormat FieldFormat => this.fieldDefinition.FieldFormat;

    public CvsCategoryFieldImportColumn(
      CustomFieldsType customFieldsType,
      string categoryName,
      CustomFieldDefinition fieldDefinition)
      : base(fieldDefinition.FieldDescription, (CustomFieldsType.BizCategoryStandard == customFieldsType ? "StandardCategoryField" : "CustomCategoryField") + "." + categoryName + "." + fieldDefinition.FieldDescription, fieldDefinition.FieldFormat, 4096)
    {
      this.customFieldsType = customFieldsType;
      this.categoryName = categoryName;
      this.fieldDefinition = fieldDefinition;
    }

    public override string FormatValue(string value)
    {
      if (FieldFormat.DROPDOWNLIST != this.fieldDefinition.FieldFormat)
        return base.FormatValue(value);
      foreach (CustomFieldOptionDefinition customFieldOption in (CollectionBase) this.fieldDefinition.CustomFieldOptions)
      {
        if (string.Compare(value, customFieldOption.OptionValue, true) == 0)
          return customFieldOption.OptionValue;
      }
      return string.Empty;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.CvsCustomFieldImportColumn
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class CvsCustomFieldImportColumn : CsvImportColumn
  {
    private ContactCustomFieldInfo fieldInfo;

    public CvsCustomFieldImportColumn(ContactCustomFieldInfo fieldInfo)
      : base(fieldInfo.Label, "CustomField." + fieldInfo.Label, fieldInfo.FieldType, 4096)
    {
      this.fieldInfo = fieldInfo;
    }

    public int FieldID => this.fieldInfo.LabelID;

    public override string FormatValue(string value)
    {
      value = value.Trim();
      if (this.fieldInfo.FieldType != FieldFormat.DROPDOWNLIST)
        return base.FormatValue(value);
      for (int index = 0; index < this.fieldInfo.FieldOptions.Length; ++index)
      {
        if (string.Compare(value, this.fieldInfo.FieldOptions[index], true) == 0)
          return this.fieldInfo.FieldOptions[index];
      }
      return "";
    }
  }
}

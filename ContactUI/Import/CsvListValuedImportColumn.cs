// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.CsvListValuedImportColumn
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Common;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class CsvListValuedImportColumn : CsvImportColumn
  {
    private string[] listValues;
    private string defaultValue;
    private Hashtable nameLookup = CollectionsUtil.CreateCaseInsensitiveHashtable();

    public CsvListValuedImportColumn(
      string propertyName,
      string description,
      string[] listValues,
      string defaultValue)
      : base(propertyName, description, FieldFormat.STRING)
    {
      this.listValues = listValues;
      this.defaultValue = defaultValue;
      for (int index = 0; index < listValues.Length; ++index)
        this.nameLookup[(object) listValues[index]] = (object) listValues[index];
    }

    public override string FormatValue(string value)
    {
      return this.nameLookup.Contains((object) value.Trim()) ? this.nameLookup[(object) value.Trim()].ToString() : this.defaultValue;
    }
  }
}

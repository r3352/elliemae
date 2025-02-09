// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.CsvMappedImportColumn
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Common;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class CsvMappedImportColumn : CsvImportColumn
  {
    private IDictionary mapping;
    private string defaultValue;

    public CsvMappedImportColumn(
      string propertyName,
      string description,
      IDictionary mapping,
      string defaultValue)
      : base(propertyName, description, FieldFormat.STRING)
    {
      this.mapping = mapping;
      this.defaultValue = defaultValue;
    }

    public override string FormatValue(string value)
    {
      return this.mapping.Contains((object) value.Trim()) ? value.Trim() : this.defaultValue;
    }

    public override string ToPropertyValue(string value)
    {
      return string.Concat(this.mapping[(object) this.FormatValue(value)]);
    }
  }
}

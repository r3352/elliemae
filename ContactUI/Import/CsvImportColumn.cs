// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.CsvImportColumn
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class CsvImportColumn
  {
    public static CsvImportColumn Unassigned = new CsvImportColumn("", "(Unassigned)", FieldFormat.STRING);
    public static readonly DateTime MinDate = new DateTime(1900, 1, 1, 0, 0, 0);
    public static readonly DateTime MaxDate = new DateTime(2079, 6, 1, 0, 0, 0);
    public string PropertySource;
    public string PropertyName;
    public string Description;
    public FieldFormat Format;
    private int MaxLength;

    public CsvImportColumn(string propertyName, string description, FieldFormat format)
      : this(propertyName, description, format, -1)
    {
    }

    public CsvImportColumn(
      string propertyName,
      string description,
      FieldFormat format,
      int maxLength)
    {
      int length = propertyName.IndexOf('.');
      if (length > 0)
      {
        this.PropertySource = propertyName.Substring(0, length);
        this.PropertyName = propertyName.Substring(length + 1);
      }
      else
      {
        this.PropertySource = "Contact";
        this.PropertyName = propertyName;
      }
      this.Description = description;
      this.Format = format;
      this.MaxLength = maxLength;
    }

    public override string ToString() => this.Description;

    public virtual string FormatValue(string value)
    {
      try
      {
        if (this.Format == FieldFormat.DATE && this.isValidDate(value))
          return DateTime.Parse(value).ToShortDateString();
        bool needsUpdate = false;
        string s = Utils.FormatInput(value, this.Format, ref needsUpdate);
        if (this.Format == FieldFormat.DATE && !this.isValidDate(s))
          s = "";
        else if (this.Format == FieldFormat.INTEGER && !this.isValidInteger(s))
          s = "";
        return this.MaxLength > 0 && s.Length > this.MaxLength ? s.Substring(0, this.MaxLength) : s;
      }
      catch
      {
        return "";
      }
    }

    public virtual string ToPropertyValue(string value) => this.FormatValue(value);

    private bool isValidDate(string s)
    {
      try
      {
        DateTime dateTime = DateTime.Parse(s);
        return !(dateTime < CsvImportColumn.MinDate) && !(dateTime > CsvImportColumn.MaxDate);
      }
      catch
      {
        return false;
      }
    }

    private bool isValidInteger(string s)
    {
      try
      {
        Convert.ToInt32(s);
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}

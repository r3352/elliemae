// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FieldOption
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class FieldOption
  {
    public static readonly FieldOption Empty = new FieldOption("", "");
    private string text = "";
    private string value = "";
    private bool? booleanValue;
    private string dbValue = "";

    internal FieldOption(XmlElement e)
    {
      this.text = (e.InnerText ?? "").Trim();
      this.value = !e.HasAttribute(nameof (Value)) ? this.text : e.GetAttribute(nameof (Value)).Trim();
      if (e.HasAttribute(nameof (BooleanValue)))
      {
        bool result;
        this.booleanValue = bool.TryParse(e.GetAttribute(nameof (BooleanValue)).Trim(), out result) ? new bool?(result) : new bool?();
      }
      this.dbValue = e.Attributes["DbValue"] == null ? this.value : e.GetAttribute("DbValue");
    }

    public FieldOption(string text, string value)
      : this(text, value, value)
    {
    }

    public FieldOption(string text, string value, string dbValue)
    {
      this.text = text;
      this.value = value;
      this.dbValue = dbValue;
    }

    public string Value => this.value;

    public bool? BooleanValue => this.booleanValue;

    public string Text => this.text;

    public string ReportingDatabaseValue => this.dbValue;

    public override string ToString() => this.Text;

    public override bool Equals(object obj)
    {
      return obj is FieldOption fieldOption && this.Value == fieldOption.Value;
    }

    public override int GetHashCode() => this.Value.GetHashCode();
  }
}

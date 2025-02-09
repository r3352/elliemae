// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.HTMLFont
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using mshtml;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  public struct HTMLFont
  {
    private string family;
    private int size;
    private string sizeUnits;
    private bool italics;
    private bool bold;
    private bool underline;

    public HTMLFont(string family, int size, bool bold, bool italics, bool underline)
    {
      this.family = family;
      this.size = size;
      this.bold = bold;
      this.italics = italics;
      this.underline = underline;
      this.sizeUnits = "px";
    }

    internal HTMLFont(IHTMLElement2 element)
    {
      this.family = HTMLFont.normalizeFontName(element.currentStyle.fontFamily);
      this.italics = element.currentStyle.fontStyle.ToLower() == "italic";
      this.bold = element.currentStyle.fontWeight.ToString() == "700";
      this.underline = element.currentStyle.textDecoration.ToLower() == nameof (underline);
      string s = element.currentStyle.fontSize.ToString();
      if (s.EndsWith("px"))
      {
        this.sizeUnits = "px";
        this.size = int.Parse(s.Substring(0, s.Length - 2));
      }
      else if (s.EndsWith("pt"))
      {
        this.sizeUnits = "pt";
        this.size = int.Parse(s.Substring(0, s.Length - 2));
      }
      else
      {
        this.size = int.Parse(s);
        this.sizeUnits = "";
      }
    }

    public string Family
    {
      get => this.family;
      set => this.family = value;
    }

    public int Size
    {
      get => this.size;
      set => this.size = value;
    }

    public bool Italics
    {
      get => this.italics;
      set => this.italics = value;
    }

    public bool Bold
    {
      get => this.bold;
      set => this.bold = value;
    }

    public bool Underline
    {
      get => this.underline;
      set => this.underline = value;
    }

    public GraphicsUnit Unit
    {
      get
      {
        return this.sizeUnits == "px" || !(this.sizeUnits == "pt") ? GraphicsUnit.Pixel : GraphicsUnit.Point;
      }
    }

    public override string ToString()
    {
      string str1 = this.family + ", " + (object) this.size + this.sizeUnits;
      string str2 = "";
      if (this.bold)
        str2 = str2 + (str2 == "" ? "" : ", ") + "bold";
      if (this.italics)
        str2 = str2 + (str2 == "" ? "" : ", ") + "italics";
      if (this.underline)
        str2 = str2 + (str2 == "" ? "" : ", ") + "underline";
      return str1 + (str2 == "" ? "" : " (" + str2 + ")");
    }

    internal void ApplyToElement(IHTMLElement e)
    {
      e.style.fontFamily = this.family;
      e.style.fontSize = (object) (this.size.ToString() + this.sizeUnits);
      e.style.fontWeight = this.bold ? "700" : "";
      e.style.fontStyle = this.italics ? "italic" : "";
      e.style.textDecoration = this.underline ? "underline" : "";
    }

    private static string normalizeFontName(string name)
    {
      return name.Substring(0, 1).ToUpper() + name.Substring(1);
    }
  }
}

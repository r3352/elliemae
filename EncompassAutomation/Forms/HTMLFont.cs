// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.HTMLFont
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using mshtml;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>Represents a Font used within an Encompass Form.</summary>
  public struct HTMLFont
  {
    private string family;
    private int size;
    private string sizeUnits;
    private bool italics;
    private bool bold;
    private bool underline;

    /// <summary>Constructor for an HTMLFont object.</summary>
    /// <param name="family">The font family name.</param>
    /// <param name="size">The font size, in pixels</param>
    /// <param name="bold">Flag to indicate if it should be bold-faced.</param>
    /// <param name="italics">Flag to indicate if it should be italicized.</param>
    /// <param name="underline">Flag to indicate if it should be underlined.</param>
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

    /// <summary>Gets or sets the font family.</summary>
    public string Family
    {
      get => this.family;
      set => this.family = value;
    }

    /// <summary>Gets ot sets the font size.</summary>
    public int Size
    {
      get => this.size;
      set => this.size = value;
    }

    /// <summary>Gets or sets whether the font is italicized.</summary>
    public bool Italics
    {
      get => this.italics;
      set => this.italics = value;
    }

    /// <summary>Gets or sets whether the font is bold-faced.</summary>
    public bool Bold
    {
      get => this.bold;
      set => this.bold = value;
    }

    /// <summary>Gets or sets whether the font is underlined.</summary>
    public bool Underline
    {
      get => this.underline;
      set => this.underline = value;
    }

    /// <summary>Gets the graphics unit used for the font.</summary>
    public GraphicsUnit Unit
    {
      get
      {
        return this.sizeUnits == "px" || !(this.sizeUnits == "pt") ? GraphicsUnit.Pixel : GraphicsUnit.Point;
      }
    }

    /// <summary>Provides a string representation of the font.</summary>
    /// <returns></returns>
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

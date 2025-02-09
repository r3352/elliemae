// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.FieldOption
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Summary description for FieldOption.</summary>
  public class FieldOption : IFieldOption
  {
    private string text = "";
    private string value = "";

    internal FieldOption(string text, string value)
    {
      this.text = text;
      this.value = value;
    }

    /// <summary>Gets the value for the option.</summary>
    public string Value => this.value;

    /// <summary>Gets the display text for the option.</summary>
    public string Text => this.text;
  }
}

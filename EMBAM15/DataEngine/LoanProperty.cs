// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanProperty
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class LoanProperty
  {
    private string category;
    private string attribute;
    private string value;

    public LoanProperty(string category, string attribute, string value)
    {
      this.category = category;
      this.attribute = attribute;
      this.value = value;
    }

    public string Category
    {
      get => this.category;
      set => this.Value = value;
    }

    public string Attribute
    {
      get => this.attribute;
      set => this.Value = value;
    }

    public string Value
    {
      get => this.value;
      set => this.Value = value;
    }

    public override bool Equals(object other)
    {
      LoanProperty loanProperty = (LoanProperty) other;
      return this.category.ToLower() == loanProperty.category.ToLower() && this.attribute.ToLower() == loanProperty.attribute.ToLower();
    }

    public override int GetHashCode() => this.category.GetHashCode() ^ this.attribute.GetHashCode();
  }
}

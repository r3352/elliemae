// Decompiled with JetBrains decompiler
// Type: Elli.Common.ModelPaths.Parsing.FragmentIndexToken
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Text;

#nullable disable
namespace Elli.Common.ModelPaths.Parsing
{
  public class FragmentIndexToken
  {
    public static readonly FragmentIndexToken PlaceholderIndexToken = new FragmentIndexToken("%", FragmentIndexType.Placeholder);
    private readonly int _hashCode;

    public FragmentIndexToken(string value, FragmentIndexType type)
    {
      this.Value = value;
      this.Type = type;
      this._hashCode = StringComparer.OrdinalIgnoreCase.GetHashCode(this.Value) * 17 + this.Type.GetHashCode();
    }

    public FragmentIndexToken(StringBuilder sb, FragmentIndexType type)
      : this(sb.ToString(), type)
    {
      sb.Clear();
    }

    public string Value { get; }

    public FragmentIndexType Type { get; }

    public override bool Equals(object obj)
    {
      return obj is FragmentIndexToken fragmentIndexToken && string.Equals(this.Value, fragmentIndexToken.Value, StringComparison.OrdinalIgnoreCase) && this.Type == fragmentIndexToken.Type;
    }

    public override int GetHashCode() => this._hashCode;

    public static bool Equals(FragmentIndexToken a, FragmentIndexToken b)
    {
      if (a == null && b == null)
        return true;
      return a != null && b != null && a.Equals((object) b);
    }

    public void ToString(StringBuilder sb) => sb.Append("[").Append(this.Value).Append("]");
  }
}

// Decompiled with JetBrains decompiler
// Type: Elli.Common.ModelPaths.Parsing.QualifierToken
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Text;

#nullable disable
namespace Elli.Common.ModelPaths.Parsing
{
  public class QualifierToken
  {
    private int _hashCode;
    private bool _hashDirty;

    public QualifierToken(string propertyName)
    {
      this.PropertyName = propertyName;
      this._hashDirty = true;
    }

    public QualifierToken(StringBuilder sb)
      : this(sb.ToString())
    {
      sb.Clear();
    }

    public string PropertyName { get; }

    public string Value { get; private set; }

    public QualifierValueType ValueType { get; private set; }

    public QualifierToken SetValue(StringBuilder sb, QualifierValueType type)
    {
      this.SetValue(sb.ToString(), type);
      sb.Clear();
      return this;
    }

    public QualifierToken SetValue(string value, QualifierValueType type)
    {
      this.Value = value;
      this.ValueType = type;
      this._hashDirty = true;
      return this;
    }

    public static int GetHashCode(string propertyName, string value)
    {
      return QualifierToken.GetHashCode(StringComparer.OrdinalIgnoreCase.GetHashCode(propertyName), value);
    }

    public static int GetHashCode(int propertyNameHash, string value)
    {
      int num = propertyNameHash * 17;
      if (!string.IsNullOrEmpty(value))
        num += StringComparer.OrdinalIgnoreCase.GetHashCode(value);
      return num * num;
    }

    public override bool Equals(object obj)
    {
      return obj is QualifierToken qualifierToken && string.Equals(this.PropertyName, qualifierToken.PropertyName, StringComparison.OrdinalIgnoreCase) && string.Equals(this.Value, qualifierToken.Value, StringComparison.OrdinalIgnoreCase) && this.ValueType == qualifierToken.ValueType;
    }

    public override int GetHashCode()
    {
      if (this._hashDirty)
      {
        this._hashCode = QualifierToken.GetHashCode(this.PropertyName, this.Value);
        this._hashDirty = false;
      }
      return this._hashCode;
    }

    public void ToString(StringBuilder sb)
    {
      string str = this.ValueType == QualifierValueType.String ? "'" + this.Value + "'" : this.Value;
      sb.Append(this.PropertyName).Append(" == ").Append(str);
    }
  }
}

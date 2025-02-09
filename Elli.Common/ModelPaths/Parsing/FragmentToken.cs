// Decompiled with JetBrains decompiler
// Type: Elli.Common.ModelPaths.Parsing.FragmentToken
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace Elli.Common.ModelPaths.Parsing
{
  public class FragmentToken
  {
    private List<QualifierToken> _qualifiers = new List<QualifierToken>();
    private int _indexAgnosticHashCode;
    private string _fullPath;
    private string _propertyTypeName;
    private QualifierToken _ofTypeQualifier;
    private bool hashDirty;
    private static readonly Dictionary<string, string> _propertyTypeNames = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      {
        "Applications",
        "Application"
      },
      {
        "CurrentApplication",
        "Application"
      },
      {
        "CoBorrower",
        "Borrower"
      },
      {
        "Eem",
        "EnergyEfficientMortgage"
      },
      {
        "LOCompensation",
        "ElliLOCompensation"
      },
      {
        "GiftsGrants",
        "GiftGrant"
      },
      {
        "TQLReports",
        "TQLReportInformation"
      },
      {
        "CustomFields",
        "CustomField"
      }
    };

    public FragmentToken(StringBuilder sb)
      : this(sb.ToString())
    {
      sb.Clear();
    }

    public FragmentToken(string propertyName) => this.ChangePropertyName(propertyName);

    public string PropertyName { get; private set; }

    public string PropertyTypeName
    {
      get
      {
        return this._propertyTypeName ?? (this._propertyTypeName = FragmentToken.GetPropertyTypeName(this.PropertyName, this.Index != null || this.Qualifiers.Any<QualifierToken>()));
      }
    }

    public IEnumerable<QualifierToken> Qualifiers => (IEnumerable<QualifierToken>) this._qualifiers;

    public FragmentIndexToken Index { get; private set; }

    public FragmentToken ChangePropertyName(string propertyName)
    {
      this.PropertyName = propertyName;
      this.ResetHashCode();
      return this;
    }

    public FragmentToken AddQualifier(QualifierToken token)
    {
      if (token.ValueType == QualifierValueType.OfType)
        this._ofTypeQualifier = token;
      this._qualifiers.Add(token);
      this.ResetHashCode();
      return this;
    }

    public FragmentToken RemoveQualifier(QualifierToken token)
    {
      if (this._qualifiers.Remove(token) && token.ValueType == QualifierValueType.OfType)
        this._ofTypeQualifier = (QualifierToken) null;
      this.ResetHashCode();
      return this;
    }

    public bool ContainsOfTypeQualifier(out QualifierToken ofTypeQualifier)
    {
      return (ofTypeQualifier = this._ofTypeQualifier) != null;
    }

    public FragmentToken SetIndex(FragmentIndexToken index)
    {
      this.Index = index;
      this.ResetHashCode();
      return this;
    }

    public override bool Equals(object obj)
    {
      return obj is FragmentToken fragmentToken && string.Equals(this.PropertyName, fragmentToken.PropertyName, StringComparison.OrdinalIgnoreCase) && new HashSet<QualifierToken>((IEnumerable<QualifierToken>) this._qualifiers).SetEquals((IEnumerable<QualifierToken>) new HashSet<QualifierToken>((IEnumerable<QualifierToken>) fragmentToken._qualifiers)) && FragmentIndexToken.Equals(this.Index, fragmentToken.Index);
    }

    public override int GetHashCode()
    {
      this.RecalculateHashCode();
      return this.Index != null ? this._indexAgnosticHashCode + this.Index.GetHashCode() * 17 : this._indexAgnosticHashCode;
    }

    private void ResetHashCode()
    {
      this._fullPath = (string) null;
      this._propertyTypeName = (string) null;
      this.hashDirty = true;
    }

    private void RecalculateHashCode()
    {
      if (!this.hashDirty)
        return;
      this._indexAgnosticHashCode = StringComparer.OrdinalIgnoreCase.GetHashCode(this.PropertyName) * 289;
      int num = 0;
      if (this._qualifiers.Any<QualifierToken>())
      {
        foreach (QualifierToken qualifier in this._qualifiers)
          num += qualifier.GetHashCode();
      }
      this._indexAgnosticHashCode += num;
      this.hashDirty = false;
    }

    public void ToString(StringBuilder sb, bool skipIndex)
    {
      sb.Append(this.PropertyName);
      if (this._qualifiers.Any<QualifierToken>())
      {
        sb.Append("[(");
        this._qualifiers[0].ToString(sb);
        for (int index = 1; index < this._qualifiers.Count; ++index)
        {
          sb.Append(" && ");
          this._qualifiers[index].ToString(sb);
        }
        sb.Append(")]");
      }
      if (skipIndex || this.Index == null)
        return;
      this.Index.ToString(sb);
    }

    public override string ToString()
    {
      if (this._fullPath == null)
      {
        StringBuilder sb = new StringBuilder();
        this.ToString(sb, false);
        this._fullPath = sb.ToString();
      }
      return this._fullPath;
    }

    public int GetIndexAgnosticHashCode()
    {
      this.RecalculateHashCode();
      return this._indexAgnosticHashCode;
    }

    public static string GetPropertyTypeName(string propertyName, bool isCollection)
    {
      string propertyTypeName;
      if (FragmentToken._propertyTypeNames.TryGetValue(propertyName, out propertyTypeName))
        return propertyTypeName;
      if (!isCollection)
        return propertyName;
      int length = propertyName.Length;
      if (length > 2 && propertyName[length - 3] == 'i' && propertyName[length - 2] == 'e' && propertyName[length - 1] == 's')
      {
        Span<char> span = stackalloc char[propertyName.Length - 2];
        int index;
        for (index = 0; index < propertyName.Length - 3; ++index)
          span[index] = propertyName[index];
        span[index] = 'y';
        return span.ToString();
      }
      if (length <= 1 || propertyName[length - 1] != 's')
        return propertyName;
      Span<char> span1 = stackalloc char[propertyName.Length - 1];
      for (int index = 0; index < propertyName.Length - 1; ++index)
        span1[index] = propertyName[index];
      return span1.ToString();
    }

    public int GetIntIndex()
    {
      return FragmentToken.GetIntIndex(this.Qualifiers.Any<QualifierToken>(), this.Index?.Value, this.Index?.Type);
    }

    public static int GetIntIndex(
      bool hasQualifiers,
      string strIndexInPath,
      FragmentIndexType? fragmentIndexType)
    {
      if (!hasQualifiers && strIndexInPath == null)
        return FragmentIndexType.None.ToIndex();
      if (strIndexInPath == null)
        return 0;
      int num1 = (int) fragmentIndexType.Value;
      int result;
      if (num1 > 1 || !int.TryParse(strIndexInPath, out result))
        result = 0;
      int num2 = result - num1;
      return num2 != -1 ? num2 : 0;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Elli.Common.ModelPaths.Parsing.ModelPath
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace Elli.Common.ModelPaths.Parsing
{
  public class ModelPath : IModelPath, IEnumerable<FragmentToken>, IEnumerable
  {
    private readonly List<FragmentToken> _fragments = new List<FragmentToken>();
    private int _indexAgnosticModelHash;
    private int _currentIndexAgnosticHash;
    private string _fullPath;
    private ApplicationLookupData _applicationLookupData = new ApplicationLookupData()
    {
      Index = FragmentIndexType.None.ToIndex()
    };
    private int _collectionIndex = FragmentIndexType.None.ToIndex();
    private CustomFieldlookupData _customFieldlookupData;
    private static readonly Dictionary<string, Func<ModelPath, FragmentToken, bool>> FragmentCorrections = new Dictionary<string, Func<ModelPath, FragmentToken, bool>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      {
        "BuySideAdjustments",
        (Func<ModelPath, FragmentToken, bool>) ((modelPath, currentFragment) =>
        {
          currentFragment.ChangePropertyName("PriceAdjustments");
          currentFragment.AddQualifier(new QualifierToken("RateLockAdjustmentType").SetValue("BuySide", QualifierValueType.String));
          return false;
        })
      },
      {
        "LockRequestAdjustments",
        (Func<ModelPath, FragmentToken, bool>) ((modelPath, currentFragment) =>
        {
          currentFragment.ChangePropertyName("PriceAdjustments");
          currentFragment.AddQualifier(new QualifierToken("RateLockAdjustmentType").SetValue("Request", QualifierValueType.String));
          return false;
        })
      },
      {
        "SellSideAdjustments",
        (Func<ModelPath, FragmentToken, bool>) ((modelPath, currentFragment) =>
        {
          currentFragment.ChangePropertyName("PriceAdjustments");
          currentFragment.AddQualifier(new QualifierToken("RateLockAdjustmentType").SetValue("SellSide", QualifierValueType.String));
          return false;
        })
      },
      {
        "CompSideAdjustments",
        (Func<ModelPath, FragmentToken, bool>) ((modelPath, currentFragment) =>
        {
          currentFragment.ChangePropertyName("PriceAdjustments");
          currentFragment.AddQualifier(new QualifierToken("RateLockAdjustmentType").SetValue("CompSide", QualifierValueType.String));
          return false;
        })
      },
      {
        "CurrentAdjustments",
        (Func<ModelPath, FragmentToken, bool>) ((modelPath, currentFragment) =>
        {
          currentFragment.ChangePropertyName("PriceAdjustments");
          currentFragment.AddQualifier(new QualifierToken("RateLockAdjustmentType").SetValue("Current", QualifierValueType.String));
          return false;
        })
      },
      {
        "CustomFields",
        (Func<ModelPath, FragmentToken, bool>) ((modelPath, currentFragment) =>
        {
          QualifierToken qualifierToken = currentFragment.Qualifiers.FirstOrDefault<QualifierToken>((Func<QualifierToken, bool>) (q => string.Equals(q.PropertyName, "FieldName", StringComparison.OrdinalIgnoreCase)));
          if (qualifierToken != null)
            modelPath._customFieldlookupData = new CustomFieldlookupData()
            {
              FieldName = qualifierToken.Value
            };
          else if (currentFragment.Index != null && currentFragment.Index.Type != FragmentIndexType.Placeholder)
            modelPath._customFieldlookupData = new CustomFieldlookupData()
            {
              FieldName = currentFragment.Index.Value.PadLeft(2, '0') + "FV"
            };
          return false;
        })
      },
      {
        "Applications",
        (Func<ModelPath, FragmentToken, bool>) ((modelPath, currentFragment) =>
        {
          if (currentFragment.Index == null)
          {
            if (currentFragment.Qualifiers.Any<QualifierToken>())
            {
              modelPath._applicationLookupData.Index = 0;
              QualifierToken token1 = currentFragment.Qualifiers.FirstOrDefault<QualifierToken>((Func<QualifierToken, bool>) (q => string.Equals(q.PropertyName, "EntityId", StringComparison.OrdinalIgnoreCase)));
              if (token1 != null)
              {
                modelPath._applicationLookupData.EntityId = token1.Value;
                currentFragment.RemoveQualifier(token1);
              }
              QualifierToken token2 = currentFragment.Qualifiers.FirstOrDefault<QualifierToken>((Func<QualifierToken, bool>) (q => string.Equals(q.PropertyName, "ApplicationId", StringComparison.OrdinalIgnoreCase)));
              if (token2 != null)
              {
                modelPath._applicationLookupData.ApplicationId = token2.Value;
                currentFragment.RemoveQualifier(token2);
              }
            }
          }
          else
          {
            currentFragment.SetIndex(new FragmentIndexToken(currentFragment.Index.Value, FragmentIndexType.FixedZeroBased));
            modelPath._applicationLookupData.Index = currentFragment.GetIntIndex();
          }
          return true;
        })
      },
      {
        "CurrentApplication",
        (Func<ModelPath, FragmentToken, bool>) ((modelPath, currentFragment) =>
        {
          currentFragment.ChangePropertyName("Applications");
          currentFragment.SetIndex(new FragmentIndexToken(string.Empty, FragmentIndexType.CurrentApplication));
          modelPath._applicationLookupData.Index = currentFragment.GetIntIndex();
          return true;
        })
      },
      {
        "MilestoneTemplateLogs",
        (Func<ModelPath, FragmentToken, bool>) ((modelPath, currentFragment) =>
        {
          currentFragment.ChangePropertyName("LogRecords");
          currentFragment.AddQualifier(new QualifierToken("GetType().Name").SetValue("MilestoneTemplateLog", QualifierValueType.OfType));
          currentFragment.SetIndex(new FragmentIndexToken(currentFragment.Index.Value, FragmentIndexType.FixedZeroBased));
          return false;
        })
      },
      {
        "Residences",
        (Func<ModelPath, FragmentToken, bool>) ((modelPath, currentFragment) =>
        {
          if (currentFragment.Qualifiers.Any<QualifierToken>((Func<QualifierToken, bool>) (q => string.Equals("MailingAddressIndicator", q.PropertyName, StringComparison.OrdinalIgnoreCase) && string.Equals("true", q.Value, StringComparison.OrdinalIgnoreCase))))
            currentFragment.SetIndex(new FragmentIndexToken("0", FragmentIndexType.FixedZeroBased));
          return false;
        })
      },
      {
        "Assets",
        (Func<ModelPath, FragmentToken, bool>) ((modelPath, currentFragment) =>
        {
          if (currentFragment.Qualifiers.Any<QualifierToken>())
            currentFragment.AddQualifier(new QualifierToken("IsVod").SetValue("false", QualifierValueType.Bool));
          else
            currentFragment.AddQualifier(new QualifierToken("IsVod").SetValue("true", QualifierValueType.Bool));
          return false;
        })
      },
      {
        "Income",
        (Func<ModelPath, FragmentToken, bool>) ((modelPath, currentFragment) =>
        {
          QualifierToken token = currentFragment.Qualifiers.FirstOrDefault<QualifierToken>((Func<QualifierToken, bool>) (q => q.PropertyName.Equals("OtherIncomeIndex", StringComparison.OrdinalIgnoreCase)));
          if (token != null)
          {
            currentFragment.RemoveQualifier(token);
            currentFragment.SetIndex(new FragmentIndexToken(token.Value, FragmentIndexType.FixedOneBased));
          }
          return false;
        })
      },
      {
        "Liabilities",
        (Func<ModelPath, FragmentToken, bool>) ((modelPath, currentFragment) =>
        {
          QualifierToken qualifierToken = currentFragment.Qualifiers.FirstOrDefault<QualifierToken>((Func<QualifierToken, bool>) (q => q.PropertyName.Equals("LiabilityType", StringComparison.OrdinalIgnoreCase)));
          QualifierToken token = currentFragment.Qualifiers.FirstOrDefault<QualifierToken>((Func<QualifierToken, bool>) (q => q.PropertyName.Equals("AccountIndicator", StringComparison.OrdinalIgnoreCase)));
          if (qualifierToken != null && (string.Equals(qualifierToken.Value, "JobRelatedExpenses", StringComparison.OrdinalIgnoreCase) || string.Equals(qualifierToken.Value, "Alimony", StringComparison.OrdinalIgnoreCase)))
          {
            if (token != null)
              currentFragment.RemoveQualifier(token);
            currentFragment.AddQualifier(new QualifierToken("AccountIndicator").SetValue("false", QualifierValueType.Bool));
          }
          return false;
        })
      },
      {
        "Gfe2010Fees",
        (Func<ModelPath, FragmentToken, bool>) ((modelPath, currentFragment) =>
        {
          if (!currentFragment.Qualifiers.Any<QualifierToken>((Func<QualifierToken, bool>) (q => q.PropertyName.Equals("Gfe2010FeeIndex"))))
            currentFragment.AddQualifier(new QualifierToken("Gfe2010FeeIndex").SetValue("0", QualifierValueType.String));
          if (!currentFragment.Qualifiers.Any<QualifierToken>((Func<QualifierToken, bool>) (q => q.PropertyName.Equals("Gfe2010FeeType"))))
            currentFragment.AddQualifier(new QualifierToken("Gfe2010FeeType").SetValue("Undefined", QualifierValueType.String));
          return false;
        })
      },
      {
        "Gfe2010WholePocs",
        (Func<ModelPath, FragmentToken, bool>) ((modelPath, currentFragment) =>
        {
          if (!currentFragment.Qualifiers.Any<QualifierToken>((Func<QualifierToken, bool>) (q => q.PropertyName.Equals("Gfe2010WholePocIndex"))))
            currentFragment.AddQualifier(new QualifierToken("Gfe2010WholePocIndex").SetValue("0", QualifierValueType.Numeric));
          return false;
        })
      },
      {
        "Gfe2010FwbcFwscs",
        (Func<ModelPath, FragmentToken, bool>) ((modelPath, currentFragment) =>
        {
          if (!currentFragment.Qualifiers.Any<QualifierToken>((Func<QualifierToken, bool>) (q => q.PropertyName.Equals("Gfe2010FwbcFwscIndex"))))
            currentFragment.AddQualifier(new QualifierToken("Gfe2010FwbcFwscIndex").SetValue("0", QualifierValueType.Numeric));
          if (!currentFragment.Qualifiers.Any<QualifierToken>((Func<QualifierToken, bool>) (q => q.PropertyName.Equals("LineNumber"))))
            currentFragment.AddQualifier(new QualifierToken("LineNumber").SetValue("0", QualifierValueType.Numeric));
          if (!currentFragment.Qualifiers.Any<QualifierToken>((Func<QualifierToken, bool>) (q => q.PropertyName.Equals("LineLetter"))))
            currentFragment.AddQualifier(new QualifierToken("LineLetter").SetValue((string) null, QualifierValueType.Null));
          return false;
        })
      },
      {
        "GfeFees",
        (Func<ModelPath, FragmentToken, bool>) ((modelPath, currentFragment) =>
        {
          if (!currentFragment.Qualifiers.Any<QualifierToken>((Func<QualifierToken, bool>) (q => q.PropertyName.Equals("GfeFeeIndex"))))
            currentFragment.AddQualifier(new QualifierToken("GfeFeeIndex").SetValue("0", QualifierValueType.Numeric));
          return false;
        })
      }
    };

    public int Count => this._fragments.Count;

    public FragmentToken this[int i] => this._fragments[i];

    public override bool Equals(object obj)
    {
      switch (obj)
      {
        case ModelPath modelPath1:
          if (this._fragments.Count != modelPath1._fragments.Count)
            return false;
          for (int index = 0; index < 0; ++index)
          {
            if (!this._fragments[index].Equals((object) modelPath1._fragments[index]))
              return false;
          }
          return true;
        case IModelPath modelPath2:
          if (this.GetIndexAgnosticModelHashCode() != modelPath2.GetIndexAgnosticModelHashCode() || this.GetPropertyHashCode() != modelPath2.GetPropertyHashCode())
            return false;
          ApplicationLookupData applicationLookupData1 = this.GetApplicationLookupData(0);
          ApplicationLookupData applicationLookupData2 = modelPath2.GetApplicationLookupData(0);
          return applicationLookupData1.Index == applicationLookupData2.Index && !(applicationLookupData1.EntityId != applicationLookupData2.EntityId) && !(applicationLookupData1.ApplicationId != applicationLookupData2.ApplicationId) && this.GetCollectionIndex(10) == modelPath2.GetCollectionIndex(10);
        default:
          return false;
      }
    }

    public ModelPath Add(FragmentToken fragment)
    {
      bool isForApplication;
      this.MakeCorrections(fragment, out isForApplication);
      if (!isForApplication && fragment.Index != null)
        this._collectionIndex = fragment.GetIntIndex();
      this._fragments.Add(fragment);
      this._indexAgnosticModelHash = this._indexAgnosticModelHash * 17 + this._currentIndexAgnosticHash;
      this._currentIndexAgnosticHash = fragment.GetIndexAgnosticHashCode();
      this._fullPath = (string) null;
      return this;
    }

    public override int GetHashCode()
    {
      int hashCode = 0;
      for (int index = 0; index < this._fragments.Count; ++index)
        hashCode = hashCode * 17 + this._fragments[index].GetHashCode();
      return hashCode;
    }

    public int GetModelHashCode()
    {
      int modelHashCode = 0;
      for (int index = 0; index < this._fragments.Count - 1; ++index)
        modelHashCode = modelHashCode * 17 + this._fragments[index].GetHashCode();
      return modelHashCode;
    }

    public int GetPropertyHashCode() => this.GetPropertyHashCode(out string _, out string _);

    public int GetPropertyHashCode(out string propertyName)
    {
      return this.GetPropertyHashCode(out string _, out propertyName);
    }

    public int GetPropertyHashCode(out string modelTypeName, out string propertyName)
    {
      FragmentToken fragment1 = this._fragments[this._fragments.Count - 2];
      QualifierToken ofTypeQualifier;
      modelTypeName = !fragment1.ContainsOfTypeQualifier(out ofTypeQualifier) ? fragment1.PropertyTypeName : ofTypeQualifier.Value;
      FragmentToken fragment2 = this._fragments[this._fragments.Count - 1];
      propertyName = fragment2.PropertyName;
      return StringComparer.OrdinalIgnoreCase.GetHashCode(modelTypeName) * 17 + StringComparer.OrdinalIgnoreCase.GetHashCode(propertyName);
    }

    public int GetIndexAgnosticModelHashCode() => this._indexAgnosticModelHash;

    public ApplicationLookupData GetApplicationLookupData(int requestedIndex)
    {
      if (this._applicationLookupData.Index < 0)
        this._applicationLookupData.Index = requestedIndex;
      return this._applicationLookupData;
    }

    public bool TryGetCustomFieldLookupData(out CustomFieldlookupData customFieldlookupData)
    {
      customFieldlookupData = this._customFieldlookupData;
      return this._customFieldlookupData != null;
    }

    public int GetCollectionIndex(int requestedIndex)
    {
      return this._collectionIndex >= 0 ? this._collectionIndex : requestedIndex;
    }

    public int GetCollectionIndex() => this._collectionIndex;

    public void ToString(StringBuilder sb)
    {
      this._fragments[0].ToString(sb, false);
      for (int index = 1; index < this._fragments.Count; ++index)
      {
        sb.Append('.');
        this._fragments[index].ToString(sb, false);
      }
    }

    public override string ToString()
    {
      if (this._fullPath == null)
      {
        StringBuilder sb = new StringBuilder();
        this.ToString(sb);
        this._fullPath = sb.ToString();
      }
      return this._fullPath;
    }

    public string GetIndexAgnosticPathForModel()
    {
      StringBuilder sb = new StringBuilder();
      this._fragments[0].ToString(sb, false);
      for (int index = 1; index < this._fragments.Count - 1; ++index)
      {
        sb.Append('.');
        this._fragments[index].ToString(sb, true);
      }
      return sb.ToString();
    }

    private void MakeCorrections(FragmentToken currentFragment, out bool isForApplication)
    {
      isForApplication = false;
      Func<ModelPath, FragmentToken, bool> func;
      if (ModelPath.FragmentCorrections.TryGetValue(currentFragment.PropertyName, out func))
        isForApplication = func(this, currentFragment);
      if (!currentFragment.Qualifiers.Any<QualifierToken>() || currentFragment.Index != null)
        return;
      currentFragment.SetIndex(new FragmentIndexToken("1", FragmentIndexType.FixedOneBased));
    }

    public IEnumerator<FragmentToken> GetEnumerator()
    {
      return (IEnumerator<FragmentToken>) this._fragments.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._fragments.GetEnumerator();
  }
}

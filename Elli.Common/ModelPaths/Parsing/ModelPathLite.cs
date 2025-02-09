// Decompiled with JetBrains decompiler
// Type: Elli.Common.ModelPaths.Parsing.ModelPathLite
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Elli.Common.ModelPaths.Parsing
{
  public class ModelPathLite : IModelPath, IModelPathBuilder<ModelPathLite, ModelPathLite>
  {
    private static readonly int BuySideAdjustmentsQualifierHash = new QualifierToken("RateLockAdjustmentType").SetValue("BuySide", QualifierValueType.String).GetHashCode();
    private static readonly int LockRequestAdjustmentsQualifierHash = new QualifierToken("RateLockAdjustmentType").SetValue("Request", QualifierValueType.String).GetHashCode();
    private static readonly int SellSideAdjustmentsQualifierHash = new QualifierToken("RateLockAdjustmentType").SetValue("SellSide", QualifierValueType.String).GetHashCode();
    private static readonly int CompSideAdjustmentsQualifierHash = new QualifierToken("RateLockAdjustmentType").SetValue("CompSide", QualifierValueType.String).GetHashCode();
    private static readonly int CurrentAdjustmentsQualifierHash = new QualifierToken("RateLockAdjustmentType").SetValue("Current", QualifierValueType.String).GetHashCode();
    private static readonly int MilestoneTemplateLogQualifierHash = new QualifierToken("GetType().Name").SetValue("MilestoneTemplateLog", QualifierValueType.OfType).GetHashCode();
    private static readonly int IsVodTrueQualifierHash = new QualifierToken("IsVod").SetValue("true", QualifierValueType.Bool).GetHashCode();
    private static readonly int IsVodFalseQualifierHash = new QualifierToken("IsVod").SetValue("false", QualifierValueType.Bool).GetHashCode();
    private static readonly int AccountIndicatorTrueQualifierHash = new QualifierToken("AccountIndicator").SetValue("true", QualifierValueType.Bool).GetHashCode();
    private static readonly int AccountIndicatorFalseQualifierHash = new QualifierToken("AccountIndicator").SetValue("false", QualifierValueType.Bool).GetHashCode();
    private static readonly int Gfe2010FeeIndex0QualifierHash = new QualifierToken("Gfe2010FeeIndex").SetValue("0", QualifierValueType.String).GetHashCode();
    private static readonly int Gfe2010FeeTypeUndefinedQualifierHash = new QualifierToken("Gfe2010FeeType").SetValue("Undefined", QualifierValueType.String).GetHashCode();
    private static readonly int Gfe2010WholePocIndex0QualifierHash = new QualifierToken("Gfe2010WholePocIndex").SetValue("0", QualifierValueType.Numeric).GetHashCode();
    private static readonly int Gfe2010FwbcFwscIndex0QualifierHash = new QualifierToken("Gfe2010FwbcFwscIndex").SetValue("0", QualifierValueType.Numeric).GetHashCode();
    private static readonly int LineNumber0QualifierHash = new QualifierToken("LineNumber").SetValue("0", QualifierValueType.Numeric).GetHashCode();
    private static readonly int LineLetterNullQualifierHash = new QualifierToken("LineLetter").SetValue((string) null, QualifierValueType.Null).GetHashCode();
    private static readonly int GfeFeeIndex0QualifierHash = new QualifierToken("GfeFeeIndex").SetValue("0", QualifierValueType.Numeric).GetHashCode();
    private int _indexAgnosticModelHash;
    private int _indexAgnosticCurrentFragmentHash;
    private ApplicationLookupData _applicationLookupData = new ApplicationLookupData()
    {
      Index = FragmentIndexType.None.ToIndex()
    };
    private CustomFieldlookupData _customFieldlookupData;
    private int _collectionIndex = FragmentIndexType.None.ToIndex();
    private string _propertyName;
    private string _modelName;
    private bool _isCollection;
    private bool _hasQualifiers;
    private int _qualifierHash;
    private string _qualifierName;
    private bool _isVodSet;
    private bool _liabilityTypeQualifierSet;
    private bool _gfe2010FeeIndexSet;
    private bool _gfe2010FeeTypeSet;
    private bool _gfe2010WholePocIndexSet;
    private bool _gfe2010FwbcFwscIndexSet;
    private bool _lineNumberSet;
    private bool _lineLetterSet;
    private bool _gfeFeeIndexSet;
    private string _originalPropertyName;
    private static readonly Dictionary<string, Action<ModelPathLite>> FragmentCorrections = new Dictionary<string, Action<ModelPathLite>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      {
        "Assets",
        (Action<ModelPathLite>) (mpd =>
        {
          if (!mpd._isVodSet)
          {
            if (mpd._qualifierHash == 0)
              mpd._qualifierHash += ModelPathLite.IsVodTrueQualifierHash;
            else
              mpd._qualifierHash += ModelPathLite.IsVodFalseQualifierHash;
          }
          mpd._isVodSet = false;
        })
      },
      {
        "Liabilities",
        (Action<ModelPathLite>) (mpd =>
        {
          if (mpd._liabilityTypeQualifierSet)
            mpd._qualifierHash += ModelPathLite.AccountIndicatorFalseQualifierHash;
          else
            mpd._qualifierHash += ModelPathLite.AccountIndicatorTrueQualifierHash;
          mpd._liabilityTypeQualifierSet = false;
        })
      },
      {
        "Gfe2010Fees",
        (Action<ModelPathLite>) (mpd =>
        {
          if (!mpd._gfe2010FeeIndexSet)
            mpd._qualifierHash += ModelPathLite.Gfe2010FeeIndex0QualifierHash;
          mpd._gfe2010FeeIndexSet = false;
          if (!mpd._gfe2010FeeTypeSet)
            mpd._qualifierHash += ModelPathLite.Gfe2010FeeTypeUndefinedQualifierHash;
          mpd._gfe2010FeeTypeSet = false;
        })
      },
      {
        "Gfe2010WholePocs",
        (Action<ModelPathLite>) (mpd =>
        {
          if (!mpd._gfe2010WholePocIndexSet)
            mpd._qualifierHash += ModelPathLite.Gfe2010WholePocIndex0QualifierHash;
          mpd._gfe2010WholePocIndexSet = false;
        })
      },
      {
        "Gfe2010FwbcFwscs",
        (Action<ModelPathLite>) (mpd =>
        {
          if (!mpd._gfe2010FwbcFwscIndexSet)
            mpd._qualifierHash += ModelPathLite.Gfe2010FwbcFwscIndex0QualifierHash;
          mpd._gfe2010FwbcFwscIndexSet = false;
          if (!mpd._lineNumberSet)
            mpd._qualifierHash += ModelPathLite.LineNumber0QualifierHash;
          mpd._lineNumberSet = false;
          if (!mpd._lineLetterSet)
            mpd._qualifierHash += ModelPathLite.LineLetterNullQualifierHash;
          mpd._lineLetterSet = false;
        })
      },
      {
        "GfeFees",
        (Action<ModelPathLite>) (mpd =>
        {
          if (!mpd._gfeFeeIndexSet)
            mpd._qualifierHash += ModelPathLite.GfeFeeIndex0QualifierHash;
          mpd._gfeFeeIndexSet = false;
        })
      }
    };
    private static readonly Dictionary<string, Func<ModelPathLite, string>> PropertyNameBasedCorrections = new Dictionary<string, Func<ModelPathLite, string>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      {
        "BuySideAdjustments",
        (Func<ModelPathLite, string>) (mpd =>
        {
          mpd._qualifierHash = ModelPathLite.BuySideAdjustmentsQualifierHash;
          return "PriceAdjustments";
        })
      },
      {
        "LockRequestAdjustments",
        (Func<ModelPathLite, string>) (mpd =>
        {
          mpd._qualifierHash = ModelPathLite.LockRequestAdjustmentsQualifierHash;
          return "PriceAdjustments";
        })
      },
      {
        "SellSideAdjustments",
        (Func<ModelPathLite, string>) (mpd =>
        {
          mpd._qualifierHash = ModelPathLite.SellSideAdjustmentsQualifierHash;
          return "PriceAdjustments";
        })
      },
      {
        "CompSideAdjustments",
        (Func<ModelPathLite, string>) (mpd =>
        {
          mpd._qualifierHash = ModelPathLite.CompSideAdjustmentsQualifierHash;
          return "PriceAdjustments";
        })
      },
      {
        "CurrentAdjustments",
        (Func<ModelPathLite, string>) (mpd =>
        {
          mpd._qualifierHash = ModelPathLite.CurrentAdjustmentsQualifierHash;
          return "PriceAdjustments";
        })
      },
      {
        "CurrentApplication",
        (Func<ModelPathLite, string>) (mpd =>
        {
          mpd._applicationLookupData.Index = FragmentIndexType.CurrentApplication.ToIndex();
          return "Applications";
        })
      },
      {
        "MilestoneTemplateLogs",
        (Func<ModelPathLite, string>) (mpd =>
        {
          mpd._qualifierHash = ModelPathLite.MilestoneTemplateLogQualifierHash;
          mpd._originalPropertyName = "MilestoneTemplateLogs";
          return "LogRecords";
        })
      }
    };
    private readonly Dictionary<string, Action<ModelPathLite, string, FragmentIndexType>> IndexBasedCorrections = new Dictionary<string, Action<ModelPathLite, string, FragmentIndexType>>()
    {
      {
        "Applications",
        (Action<ModelPathLite, string, FragmentIndexType>) ((mpd, indexValue, indexType) => mpd._applicationLookupData.Index = FragmentToken.GetIntIndex(false, indexValue, new FragmentIndexType?(FragmentIndexType.FixedZeroBased)))
      },
      {
        "LogRecords",
        (Action<ModelPathLite, string, FragmentIndexType>) ((mpd, indexValue, indexType) => mpd._collectionIndex = FragmentToken.GetIntIndex(false, indexValue, new FragmentIndexType?(FragmentIndexType.FixedZeroBased)))
      },
      {
        "CustomFields",
        (Action<ModelPathLite, string, FragmentIndexType>) ((mpd, indexValue, indexType) =>
        {
          if (indexType == FragmentIndexType.Placeholder)
          {
            mpd._collectionIndex = FragmentToken.GetIntIndex(mpd._hasQualifiers, indexValue, new FragmentIndexType?(indexType));
          }
          else
          {
            mpd._customFieldlookupData = new CustomFieldlookupData()
            {
              FieldName = "CUST" + indexValue.PadLeft(2, '0') + "FV"
            };
            mpd._qualifierHash = 0;
            mpd._collectionIndex = FragmentIndexType.None.ToIndex();
          }
        })
      }
    };
    private static readonly Dictionary<string, Func<ModelPathLite, string, bool>> QualifierValueBasedCorrections = new Dictionary<string, Func<ModelPathLite, string, bool>>()
    {
      {
        "Applications",
        (Func<ModelPathLite, string, bool>) ((mpd, qvalue) =>
        {
          if (string.Equals(mpd._qualifierName, "EntityId", StringComparison.OrdinalIgnoreCase))
          {
            mpd._applicationLookupData.EntityId = qvalue;
            mpd._applicationLookupData.Index = 0;
            mpd._qualifierHash = 0;
          }
          else if (string.Equals(mpd._qualifierName, "ApplicationId", StringComparison.OrdinalIgnoreCase))
          {
            mpd._applicationLookupData.ApplicationId = qvalue;
            mpd._applicationLookupData.Index = 0;
            mpd._qualifierHash = 0;
          }
          return false;
        })
      },
      {
        "CustomFields",
        (Func<ModelPathLite, string, bool>) ((mpd, qvalue) =>
        {
          if (string.Equals(mpd._qualifierName, "FieldName", StringComparison.OrdinalIgnoreCase))
          {
            mpd._customFieldlookupData = new CustomFieldlookupData()
            {
              FieldName = qvalue
            };
            mpd._qualifierHash = 0;
          }
          return false;
        })
      },
      {
        "Residences",
        (Func<ModelPathLite, string, bool>) ((mpd, qvalue) =>
        {
          if (string.Equals(mpd._qualifierName, "MailingAddressIndicator", StringComparison.OrdinalIgnoreCase) && string.Equals(qvalue, "true", StringComparison.OrdinalIgnoreCase))
            mpd._collectionIndex = 0;
          return true;
        })
      },
      {
        "Assets",
        (Func<ModelPathLite, string, bool>) ((mpd, qvalue) =>
        {
          if (string.Equals(mpd._qualifierName, "IsVod", StringComparison.OrdinalIgnoreCase))
            mpd._isVodSet = true;
          return true;
        })
      },
      {
        "Income",
        (Func<ModelPathLite, string, bool>) ((mpd, qvalue) =>
        {
          if (!string.Equals(mpd._qualifierName, "OtherIncomeIndex", StringComparison.OrdinalIgnoreCase))
            return true;
          mpd._collectionIndex = FragmentToken.GetIntIndex(true, qvalue, new FragmentIndexType?(FragmentIndexType.FixedOneBased));
          return false;
        })
      },
      {
        "Liabilities",
        (Func<ModelPathLite, string, bool>) ((mpd, qvalue) =>
        {
          if (string.Equals(mpd._qualifierName, "LiabilityType", StringComparison.OrdinalIgnoreCase))
          {
            mpd._liabilityTypeQualifierSet = string.Equals(qvalue, "JobRelatedExpenses", StringComparison.OrdinalIgnoreCase) || string.Equals(qvalue, "Alimony", StringComparison.OrdinalIgnoreCase);
            return true;
          }
          return !string.Equals(mpd._qualifierName, "AccountIndicator", StringComparison.OrdinalIgnoreCase);
        })
      },
      {
        "Gfe2010Fees",
        (Func<ModelPathLite, string, bool>) ((mpd, qvalue) =>
        {
          if (string.Equals(mpd._qualifierName, "Gfe2010FeeIndex", StringComparison.OrdinalIgnoreCase))
            mpd._gfe2010FeeIndexSet = true;
          if (string.Equals(mpd._qualifierName, "Gfe2010FeeType", StringComparison.OrdinalIgnoreCase))
            mpd._gfe2010FeeTypeSet = true;
          return true;
        })
      },
      {
        "Gfe2010WholePocs",
        (Func<ModelPathLite, string, bool>) ((mpd, qvalue) =>
        {
          if (string.Equals(mpd._qualifierName, "Gfe2010WholePocIndex", StringComparison.OrdinalIgnoreCase))
            mpd._gfe2010WholePocIndexSet = true;
          return true;
        })
      },
      {
        "Gfe2010FwbcFwscs",
        (Func<ModelPathLite, string, bool>) ((mpd, qvalue) =>
        {
          if (string.Equals(mpd._qualifierName, "Gfe2010FwbcFwscIndex", StringComparison.OrdinalIgnoreCase))
            mpd._gfe2010FwbcFwscIndexSet = true;
          if (string.Equals(mpd._qualifierName, "LineNumber", StringComparison.OrdinalIgnoreCase))
            mpd._lineNumberSet = true;
          if (string.Equals(mpd._qualifierName, "LineLetter", StringComparison.OrdinalIgnoreCase))
            mpd._lineLetterSet = true;
          return true;
        })
      },
      {
        "GfeFees",
        (Func<ModelPathLite, string, bool>) ((mpd, qvalue) =>
        {
          if (string.Equals(mpd._qualifierName, "GfeFeeIndex", StringComparison.OrdinalIgnoreCase))
            mpd._gfeFeeIndexSet = true;
          return true;
        })
      }
    };

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

    public int GetPropertyHashCode() => this.GetPropertyHashCode(out string _);

    public int GetPropertyHashCode(out string propertyName)
    {
      propertyName = this._propertyName;
      return StringComparer.OrdinalIgnoreCase.GetHashCode(FragmentToken.GetPropertyTypeName(this._modelName, this._isCollection)) * 17 + StringComparer.OrdinalIgnoreCase.GetHashCode(propertyName);
    }

    public ModelPathLite StartFragment(StringBuilder tokenValue)
    {
      if (!string.IsNullOrEmpty(this._propertyName))
      {
        Action<ModelPathLite> action;
        if (ModelPathLite.FragmentCorrections.TryGetValue(this._propertyName, out action))
          action(this);
        this._indexAgnosticModelHash = this._indexAgnosticModelHash * 17 + this._indexAgnosticCurrentFragmentHash + this._qualifierHash;
        this._modelName = this._originalPropertyName ?? this._propertyName;
        this._originalPropertyName = (string) null;
      }
      string key = tokenValue.ToString();
      Func<ModelPathLite, string> func;
      this._propertyName = !ModelPathLite.PropertyNameBasedCorrections.TryGetValue(key, out func) ? key : func(this);
      this._indexAgnosticCurrentFragmentHash = StringComparer.OrdinalIgnoreCase.GetHashCode(this._propertyName) * 289;
      return this;
    }

    public ModelPathLite SetFragmentIndex(FragmentIndexToken index)
    {
      Action<ModelPathLite, string, FragmentIndexType> action;
      if (this.IndexBasedCorrections.TryGetValue(this._propertyName, out action))
        action(this, index.Value, index.Type);
      else if (this._collectionIndex < 0)
        this._collectionIndex = FragmentToken.GetIntIndex(this._hasQualifiers, index.Value, new FragmentIndexType?(index.Type));
      return this;
    }

    public ModelPathLite SetFragmentIndex(
      StringBuilder tokenValue,
      FragmentIndexType fragmentIndexType)
    {
      string strIndexInPath = tokenValue.ToString();
      Action<ModelPathLite, string, FragmentIndexType> action;
      if (this.IndexBasedCorrections.TryGetValue(this._propertyName, out action))
        action(this, strIndexInPath, fragmentIndexType);
      else if (this._collectionIndex < 0)
        this._collectionIndex = FragmentToken.GetIntIndex(this._hasQualifiers, strIndexInPath, new FragmentIndexType?(fragmentIndexType));
      return this;
    }

    public ModelPathLite StartQualifier(StringBuilder tokenValue)
    {
      this._qualifierName = tokenValue.ToString();
      return this;
    }

    public ModelPathLite SetQualifierValue(
      StringBuilder tokenValue,
      QualifierValueType qualifierValueType)
    {
      string str = tokenValue.ToString();
      Func<ModelPathLite, string, bool> func;
      if (!ModelPathLite.QualifierValueBasedCorrections.TryGetValue(this._propertyName, out func) || func(this, str))
        this._qualifierHash += QualifierToken.GetHashCode(this._qualifierName, str);
      return this;
    }

    public ModelPathLite AddFragment() => this;

    public ModelPathLite AddQualifierToFragment() => this;

    public ModelPathLite Build()
    {
      if (this._qualifierHash != 0)
      {
        this._hasQualifiers = this._isCollection = true;
        if (this._collectionIndex == FragmentIndexType.None.ToIndex())
          this._collectionIndex = 0;
      }
      else
      {
        this._hasQualifiers = false;
        this._isCollection = this._collectionIndex >= -2;
      }
      return this;
    }

    public override int GetHashCode() => base.GetHashCode();

    public override bool Equals(object obj)
    {
      if (!(obj is IModelPath modelPath) || this.GetIndexAgnosticModelHashCode() != modelPath.GetIndexAgnosticModelHashCode() || this.GetPropertyHashCode() != modelPath.GetPropertyHashCode())
        return false;
      ApplicationLookupData applicationLookupData1 = this.GetApplicationLookupData(0);
      ApplicationLookupData applicationLookupData2 = modelPath.GetApplicationLookupData(0);
      return applicationLookupData1.Index == applicationLookupData2.Index && !(applicationLookupData1.EntityId != applicationLookupData2.EntityId) && !(applicationLookupData1.ApplicationId != applicationLookupData2.ApplicationId) && this.GetCollectionIndex(10) == modelPath.GetCollectionIndex(10);
    }
  }
}

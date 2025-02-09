// Decompiled with JetBrains decompiler
// Type: Elli.Common.Fields.EncompassField
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Elli.Common.Fields
{
  public class EncompassField : ICloneable
  {
    private string _category;

    public EncompassField()
    {
      this.Options = (IList<EncompassFieldOption>) new List<EncompassFieldOption>();
    }

    public bool AllowEdit { get; set; }

    public bool AllowReporting { get; set; }

    public string Category
    {
      get => this._category;
      set
      {
        this._category = value;
        this.HasValidCategory = string.Compare(value, "none", true) != 0;
      }
    }

    public bool HasValidCategory { get; private set; }

    public string DbField { get; set; }

    public string Description { get; set; }

    public string Format { get; set; }

    public string DataType { get; set; }

    public bool MultiInstance { get; set; }

    public bool OptionsRequired { get; set; }

    public int MaxLength { get; set; }

    public string ID { get; set; }

    public IList<EncompassFieldOption> Options { get; protected set; }

    public string XPath { get; set; }

    public string ModelPath { get; set; }

    public string GetMultiInstanceFieldIdForIndex(int index)
    {
      if (!this.MultiInstance)
        throw new InvalidOperationException("This is not a MultiInstance field.");
      if (index < 1 || index > 99)
        throw new ArgumentOutOfRangeException(nameof (index), "Index should be between 1 and 99.");
      if (this.ID.StartsWith("DD", StringComparison.OrdinalIgnoreCase))
        index = EncompassField.AdjustDepositAssetIndex(this.XPath, index);
      int length = this.ID.IndexOf("00", StringComparison.Ordinal);
      string str1 = this.ID.Substring(0, length);
      string str2 = this.ID.Substring(length + "00".Length, this.ID.Length - length - "00".Length);
      string str3 = index.ToString((IFormatProvider) CultureInfo.InvariantCulture).PadLeft("00".Length, '0');
      return string.Format("{0}{1}{2}", (object) str1, (object) str3, (object) str2);
    }

    public string GetRepeatableFieldModelPath(int index, string fieldId)
    {
      string str = string.Empty;
      if (fieldId.Length >= 2)
        str = fieldId.Substring(0, 2);
      if (!this.MultiInstance)
        throw new InvalidOperationException("This is not a MultiInstance field.");
      int num = str == "FL" || str == "DD" || str == "FM" || str == "SP" || str == "HC" || str == "AB" ? 999 : 99;
      if (index < 1 || index > num)
        throw new ArgumentOutOfRangeException(nameof (index), "Index should be between 1 and" + num.ToString() + ".");
      if (!this.HasModelPath())
        return (string) null;
      if (this.ID.StartsWith("DD", StringComparison.OrdinalIgnoreCase))
        index = EncompassField.AdjustDepositAssetIndex(this.XPath, index);
      return this.ModelPath.Replace("[%]", string.Format("[{0}]", (object) index));
    }

    public string GetRepeatableFieldIdForIndex(int index)
    {
      if (!this.MultiInstance)
        throw new InvalidOperationException("This is not a MultiInstance field.");
      if (index < 1 || index > 99)
        throw new ArgumentOutOfRangeException(nameof (index), "Index should be between 1 and 99.");
      if (this.ID.StartsWith("DD", StringComparison.OrdinalIgnoreCase))
        index = EncompassField.AdjustDepositAssetIndex(this.XPath, index);
      int length = this.ID.IndexOf("00", StringComparison.Ordinal);
      string str1 = this.ID.Substring(0, length);
      string str2 = this.ID.Substring(length + "00".Length, this.ID.Length - length - "00".Length);
      string str3 = index.ToString((IFormatProvider) CultureInfo.InvariantCulture).PadLeft("00".Length, '0');
      return string.Format("{0}{1}{2}", (object) str1, (object) str3, (object) str2);
    }

    public static int AdjustDepositAssetIndex(string xpath, int index)
    {
      if (xpath.Contains("NameInAccount1") || xpath.Contains("AccountIdentifier1") || xpath.Contains("CashOrMarketValueAmount1") || xpath.Contains("DEPOSIT[%]/@Type1"))
        return index * 4 - 3;
      if (xpath.Contains("NameInAccount2") || xpath.Contains("AccountIdentifier2") || xpath.Contains("CashOrMarketValueAmount2") || xpath.Contains("DEPOSIT[%]/@Type2"))
        return index * 4 - 2;
      if (xpath.Contains("NameInAccount3") || xpath.Contains("AccountIdentifier3") || xpath.Contains("CashOrMarketValueAmount3") || xpath.Contains("DEPOSIT[%]/@Type3"))
        return index * 4 - 1;
      return xpath.Contains("NameInAccount4") || xpath.Contains("AccountIdentifier4") || xpath.Contains("CashOrMarketValueAmount4") || xpath.Contains("DEPOSIT[%]/@Type4") ? index * 4 : index * 4 - 3;
    }

    public object Clone()
    {
      return (object) new EncompassField()
      {
        AllowEdit = this.AllowEdit,
        AllowReporting = this.AllowReporting,
        Category = this.Category,
        DbField = this.DbField,
        Description = this.Description,
        Format = this.Format,
        ID = this.ID,
        MultiInstance = this.MultiInstance,
        OptionsRequired = this.OptionsRequired,
        MaxLength = this.MaxLength,
        XPath = this.XPath,
        ModelPath = this.ModelPath
      };
    }
  }
}

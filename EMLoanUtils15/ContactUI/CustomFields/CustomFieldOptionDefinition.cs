// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CustomFields.CustomFieldOptionDefinition
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer.CustomFields;
using System;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CustomFields
{
  [Serializable]
  public class CustomFieldOptionDefinition : BusinessBase, IDisposable
  {
    private CustomFieldOptionDefinitionInfo customFieldOptionDefinitionInfo;

    public int FieldId => this.customFieldOptionDefinitionInfo.FieldId;

    public int OptionNumber
    {
      get => this.customFieldOptionDefinitionInfo.OptionNumber;
      set
      {
        if (this.customFieldOptionDefinitionInfo.OptionNumber == value)
          return;
        this.customFieldOptionDefinitionInfo.OptionNumber = value;
        this.BrokenRules.Assert("OptionNumberRequired", "Option number is a required field", this.customFieldOptionDefinitionInfo.OptionNumber == 0);
        this.MarkDirty();
      }
    }

    public string OptionValue
    {
      get => this.customFieldOptionDefinitionInfo.OptionValue;
      set
      {
        value = value == null ? string.Empty : value.Trim();
        if (!(this.customFieldOptionDefinitionInfo.OptionValue != value))
          return;
        this.customFieldOptionDefinitionInfo.OptionValue = value;
        this.BrokenRules.Assert("OptionValueRequired", "Option value is a required field", this.customFieldOptionDefinitionInfo.OptionValue.Length < 1);
        this.BrokenRules.Assert("OptionValueLength", "Option value exceeds 50 characters", this.customFieldOptionDefinitionInfo.OptionValue.Length > 50);
        this.MarkDirty();
      }
    }

    internal CustomFieldOptionDefinitionInfo GetInfo()
    {
      return new CustomFieldOptionDefinitionInfo()
      {
        FieldId = this.FieldId,
        OptionNumber = this.OptionNumber,
        OptionValue = this.OptionValue,
        IsNew = this.IsNew,
        IsDirty = this.IsDirty,
        IsDeleted = this.IsDeleted
      };
    }

    internal void SetInfo(
      CustomFieldOptionDefinitionInfo customFieldOptionDefinitionInfo)
    {
      if (customFieldOptionDefinitionInfo == null)
        return;
      this.customFieldOptionDefinitionInfo = customFieldOptionDefinitionInfo;
    }

    public override string ToString()
    {
      return string.Format("CustomFieldOption[{0},{1}]", (object) this.FieldId, (object) this.OptionValue);
    }

    public bool Equals(
      CustomFieldOptionDefinition customFieldOptionDefinition)
    {
      return this.FieldId.Equals(customFieldOptionDefinition.FieldId);
    }

    public new static bool Equals(object objA, object objB)
    {
      return objA is CustomFieldOptionDefinition && objB is CustomFieldOptionDefinition && ((CustomFieldOptionDefinition) objA).Equals((CustomFieldOptionDefinition) objB);
    }

    public override bool Equals(object obj)
    {
      return obj is CustomFieldOptionDefinition && this.Equals((CustomFieldOptionDefinition) obj);
    }

    public override int GetHashCode() => this.FieldId.GetHashCode();

    public static CustomFieldOptionDefinition NewCustomFieldOptionDefinition()
    {
      return new CustomFieldOptionDefinition();
    }

    public static CustomFieldOptionDefinition NewCustomFieldOptionDefinition(
      CustomFieldOptionDefinitionInfo customFieldOptionDefinitionInfo)
    {
      return new CustomFieldOptionDefinition(customFieldOptionDefinitionInfo);
    }

    private CustomFieldOptionDefinition()
    {
      this.MarkAsChild();
      this.MarkNew();
      this.customFieldOptionDefinitionInfo = new CustomFieldOptionDefinitionInfo(0, 0, string.Empty);
      this.BrokenRules.Assert("OptionNumberRequired", "Option number is a required field", this.customFieldOptionDefinitionInfo.OptionNumber == 0);
      this.BrokenRules.Assert("OptionValueRequired", "Option value is a required field", this.customFieldOptionDefinitionInfo.OptionValue.Length < 1);
    }

    private CustomFieldOptionDefinition(
      CustomFieldOptionDefinitionInfo customFieldOptionDefinitionInfo)
    {
      this.MarkAsChild();
      this.MarkOld();
      this.SetInfo(customFieldOptionDefinitionInfo);
    }

    public void Dispose()
    {
    }
  }
}

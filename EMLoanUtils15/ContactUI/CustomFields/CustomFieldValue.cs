// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CustomFields.CustomFieldValue
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CustomFields
{
  [Serializable]
  public class CustomFieldValue : BusinessBase, IDisposable
  {
    private CustomFieldValueInfo customFieldValueInfo;

    public int FieldId => this.customFieldValueInfo.FieldId;

    public int ContactId => this.customFieldValueInfo.ContactId;

    public FieldFormat FieldFormat => this.customFieldValueInfo.FieldFormat;

    public string FieldValue
    {
      get => this.customFieldValueInfo.FieldValue;
      set
      {
        value = value == null ? string.Empty : value.Trim();
        if (!(this.customFieldValueInfo.FieldValue != value))
          return;
        this.customFieldValueInfo.FieldValue = value;
        this.BrokenRules.Assert("FieldValueRequired", "Field value is a required field", this.customFieldValueInfo.FieldValue.Length < 1);
        this.BrokenRules.Assert("FieldValueLength", "Field value exceeds 4096 characters", this.customFieldValueInfo.FieldValue.Length > 4096);
        this.MarkDirty();
      }
    }

    internal CustomFieldValueInfo GetInfo()
    {
      this.customFieldValueInfo.IsNew = this.IsNew;
      this.customFieldValueInfo.IsDirty = this.IsDirty;
      this.customFieldValueInfo.IsDeleted = this.IsDeleted;
      return this.customFieldValueInfo;
    }

    internal void SetInfo(CustomFieldValueInfo customFieldValueInfo)
    {
      if (customFieldValueInfo == null)
        return;
      this.customFieldValueInfo = customFieldValueInfo;
    }

    public override string ToString()
    {
      return string.Format("CustomFieldValue[{0},{1}]", (object) this.FieldId, (object) this.ContactId);
    }

    public bool Equals(CustomFieldValue customFieldValue)
    {
      return this.FieldId.Equals(customFieldValue.FieldId) && this.ContactId.Equals(customFieldValue.ContactId);
    }

    public new static bool Equals(object objA, object objB)
    {
      return objA is CustomFieldValue && objB is CustomFieldValue && ((CustomFieldValue) objA).Equals((CustomFieldValue) objB);
    }

    public override bool Equals(object obj)
    {
      return obj is CustomFieldValue && this.Equals((CustomFieldValue) obj);
    }

    public override int GetHashCode() => this.ToString().GetHashCode();

    public static CustomFieldValue NewCustomFieldValue(
      int fieldId,
      int contactId,
      FieldFormat fieldFormat)
    {
      return new CustomFieldValue(fieldId, contactId, fieldFormat);
    }

    public static CustomFieldValue NewCustomFieldValue(CustomFieldValueInfo customFieldValueInfo)
    {
      return new CustomFieldValue(customFieldValueInfo);
    }

    private CustomFieldValue(int fieldId, int contactId, FieldFormat fieldFormat)
    {
      this.MarkNew();
      this.customFieldValueInfo = new CustomFieldValueInfo(fieldId, contactId, fieldFormat, string.Empty);
      this.BrokenRules.Assert("FieldFormatRequired", "Field format is a required field", this.customFieldValueInfo.FieldFormat == FieldFormat.NONE);
      this.BrokenRules.Assert("FieldValueRequired", "Field value is a required field", this.customFieldValueInfo.FieldValue.Length < 1);
      this.BrokenRules.Assert("FieldValueLength", "Field value exceeds 4096 characters", this.customFieldValueInfo.FieldValue.Length > 4096);
    }

    private CustomFieldValue(CustomFieldValueInfo customFieldValueInfo)
    {
      this.MarkOld();
      this.SetInfo(customFieldValueInfo);
    }

    public void Dispose()
    {
    }
  }
}

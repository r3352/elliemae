// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CustomFields.CustomFieldMapping
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
  public class CustomFieldMapping : BusinessBase, IDisposable
  {
    private CustomFieldMappingInfo customFieldMappingInfo;

    public CustomFieldsType CustomFieldsType => this.customFieldMappingInfo.CustomFieldsType;

    public int CategoryId => this.customFieldMappingInfo.CategoryId;

    public int FieldNumber => this.customFieldMappingInfo.FieldNumber;

    public int RecordId => this.customFieldMappingInfo.RecordId;

    public FieldFormat FieldFormat => this.customFieldMappingInfo.FieldFormat;

    public string LoanFieldId
    {
      get => this.customFieldMappingInfo.LoanFieldId;
      set
      {
        value = value == null ? string.Empty : value.Trim();
        if (!(this.customFieldMappingInfo.LoanFieldId != value))
          return;
        this.customFieldMappingInfo.LoanFieldId = value;
        this.MarkDirty();
      }
    }

    public bool TwoWayTransfer
    {
      get => this.customFieldMappingInfo.TwoWayTransfer;
      set
      {
        if (this.customFieldMappingInfo.TwoWayTransfer == value)
          return;
        this.customFieldMappingInfo.TwoWayTransfer = value;
        this.MarkDirty();
      }
    }

    internal void SetInfo(CustomFieldMappingInfo customFieldMappingInfo)
    {
      if (customFieldMappingInfo == null)
        return;
      this.customFieldMappingInfo = customFieldMappingInfo;
    }

    public override string ToString()
    {
      return string.Format("CustomFieldMapping[{0},{1},{2}]", (object) this.CustomFieldsType, (object) this.CategoryId, (object) this.FieldNumber);
    }

    public bool Equals(CustomFieldMapping CustomFieldMapping)
    {
      if (this.CustomFieldsType.Equals((object) CustomFieldMapping.CustomFieldsType))
      {
        int num = this.CategoryId;
        if (num.Equals(CustomFieldMapping.CategoryId))
        {
          num = this.FieldNumber;
          return num.Equals(CustomFieldMapping.FieldNumber);
        }
      }
      return false;
    }

    public new static bool Equals(object objA, object objB)
    {
      return objA is CustomFieldMapping && objB is CustomFieldMapping && ((CustomFieldMapping) objA).Equals((CustomFieldMapping) objB);
    }

    public override bool Equals(object obj)
    {
      return obj is CustomFieldMapping && this.Equals((CustomFieldMapping) obj);
    }

    public override int GetHashCode() => this.FieldNumber.GetHashCode();

    public static CustomFieldMapping NewCustomFieldMapping(
      CustomFieldsType customFieldsType,
      int categoryId,
      int fieldNumber,
      int recordId,
      FieldFormat fieldFormat,
      string loanFieldId,
      bool twoWayTransfer)
    {
      return new CustomFieldMapping(customFieldsType, categoryId, fieldNumber, recordId, fieldFormat, loanFieldId, twoWayTransfer);
    }

    public static CustomFieldMapping NewCustomFieldMapping(
      CustomFieldMappingInfo customFieldMappingInfo)
    {
      return new CustomFieldMapping(customFieldMappingInfo);
    }

    private CustomFieldMapping(
      CustomFieldsType customFieldsType,
      int categoryId,
      int fieldNumber,
      int recordId,
      FieldFormat fieldFormat,
      string loanFieldId,
      bool twoWayTransfer)
    {
      this.MarkAsChild();
      this.MarkNew();
      this.customFieldMappingInfo = new CustomFieldMappingInfo(customFieldsType, categoryId, fieldNumber, recordId, fieldFormat, loanFieldId, twoWayTransfer);
    }

    private CustomFieldMapping(CustomFieldMappingInfo customFieldMappingInfo)
    {
      this.MarkAsChild();
      this.MarkOld();
      this.SetInfo(customFieldMappingInfo);
    }

    public void Dispose()
    {
    }
  }
}

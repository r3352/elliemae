// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CustomFields.CustomFieldDefinition
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.Common;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CustomFields
{
  [Serializable]
  public class CustomFieldDefinition : BusinessBase, IDisposable
  {
    private FieldFormat originalFieldFormat;
    private CustomFieldDefinitionInfo customFieldDefinitionInfo;
    [NotUndoable]
    private CustomFieldOptionDefinitionCollection customFieldOptions = CustomFieldOptionDefinitionCollection.NewCustomFieldOptionDefinitionCollection();

    public int FieldId => this.customFieldDefinitionInfo.FieldId;

    public int CategoryId => this.customFieldDefinitionInfo.CategoryId;

    public int FieldNumber => this.customFieldDefinitionInfo.FieldNumber;

    public string FieldDescription
    {
      get => this.customFieldDefinitionInfo.FieldDescription;
      set
      {
        value = value == null ? string.Empty : value.Trim();
        if (!(this.customFieldDefinitionInfo.FieldDescription != value))
          return;
        this.customFieldDefinitionInfo.FieldDescription = value;
        this.BrokenRules.Assert("FieldDescriptionRequired", "Field description is a required field", this.customFieldDefinitionInfo.FieldDescription.Length < 1);
        this.BrokenRules.Assert("FieldDescriptionLength", "Field description exceeds 50 characters", this.customFieldDefinitionInfo.FieldDescription.Length > 50);
        this.MarkDirty();
      }
    }

    public FieldFormat FieldFormat
    {
      get => this.customFieldDefinitionInfo.FieldFormat;
      set
      {
        if (this.customFieldDefinitionInfo.FieldFormat == value)
          return;
        this.customFieldDefinitionInfo.FieldFormat = value;
        this.BrokenRules.Assert("FieldFormatRequired", "Field format is a required field", this.customFieldDefinitionInfo.FieldFormat == FieldFormat.NONE);
        this.MarkDirty();
      }
    }

    public string LoanFieldId
    {
      get => this.customFieldDefinitionInfo.LoanFieldId;
      set
      {
        value = value == null ? string.Empty : value.Trim();
        if (!(this.customFieldDefinitionInfo.LoanFieldId != value))
          return;
        this.customFieldDefinitionInfo.LoanFieldId = value;
        this.BrokenRules.Assert("LoanFieldIdLength", "Loan Field Id exceeds 30 characters", this.customFieldDefinitionInfo.LoanFieldId.Length > 30);
        this.MarkDirty();
      }
    }

    public bool TwoWayTransfer
    {
      get => this.customFieldDefinitionInfo.TwoWayTransfer;
      set
      {
        if (this.customFieldDefinitionInfo.TwoWayTransfer == value)
          return;
        this.customFieldDefinitionInfo.TwoWayTransfer = value;
        this.MarkDirty();
      }
    }

    public CustomFieldOptionDefinitionCollection CustomFieldOptions
    {
      get => this.customFieldOptions;
      set => this.customFieldOptions = value;
    }

    public bool FieldFormatChanged()
    {
      return !this.IsNew && this.originalFieldFormat != this.customFieldDefinitionInfo.FieldFormat;
    }

    internal CustomFieldDefinitionInfo GetDirtyInfo()
    {
      CustomFieldDefinitionInfo dirtyInfo = new CustomFieldDefinitionInfo();
      dirtyInfo.FieldId = this.FieldId;
      dirtyInfo.CategoryId = this.CategoryId;
      dirtyInfo.FieldNumber = this.FieldNumber;
      dirtyInfo.FieldDescription = this.FieldDescription;
      dirtyInfo.FieldFormat = this.FieldFormat;
      dirtyInfo.LoanFieldId = this.LoanFieldId;
      dirtyInfo.TwoWayTransfer = this.TwoWayTransfer;
      dirtyInfo.IsNew = this.IsNew;
      dirtyInfo.IsDirty = this.IsDirty;
      dirtyInfo.IsDeleted = this.IsDeleted;
      if (!this.IsDeleted)
      {
        dirtyInfo.CustomFieldOptionDefinitions = new CustomFieldOptionDefinitionInfo[this.customFieldOptions.Count];
        int num = 0;
        foreach (CustomFieldOptionDefinition customFieldOption in (CollectionBase) this.customFieldOptions)
          dirtyInfo.CustomFieldOptionDefinitions[num++] = customFieldOption.GetInfo();
      }
      return dirtyInfo;
    }

    internal void SetInfo(
      CustomFieldDefinitionInfo customFieldDefinitionInfo)
    {
      if (customFieldDefinitionInfo == null)
        return;
      this.customFieldDefinitionInfo = customFieldDefinitionInfo;
      this.customFieldOptions = CustomFieldOptionDefinitionCollection.NewCustomFieldOptionDefinitionCollection();
      if (customFieldDefinitionInfo.CustomFieldOptionDefinitions == null)
        return;
      foreach (CustomFieldOptionDefinitionInfo optionDefinition in customFieldDefinitionInfo.CustomFieldOptionDefinitions)
        this.customFieldOptions.Add(CustomFieldOptionDefinition.NewCustomFieldOptionDefinition(optionDefinition));
    }

    public override string ToString() => string.Format("CustomField[{0}]", (object) this.FieldId);

    public bool Equals(CustomFieldDefinition customFieldDefinition)
    {
      return this.FieldNumber.Equals(customFieldDefinition.FieldNumber);
    }

    public new static bool Equals(object objA, object objB)
    {
      return objA is CustomFieldDefinition && objB is CustomFieldDefinition && ((CustomFieldDefinition) objA).Equals((CustomFieldDefinition) objB);
    }

    public override bool Equals(object obj)
    {
      return obj is CustomFieldDefinition && this.Equals((CustomFieldDefinition) obj);
    }

    public override int GetHashCode() => this.FieldId.GetHashCode();

    public override bool IsValid
    {
      get
      {
        this.BrokenRules.Assert("FieldOptionRequired", "One or more field options are required", (FieldFormat.DROPDOWN == this.customFieldDefinitionInfo.FieldFormat || FieldFormat.DROPDOWNLIST == this.customFieldDefinitionInfo.FieldFormat) && this.CustomFieldOptions.Count == 0);
        return base.IsValid && this.customFieldOptions.IsValid;
      }
    }

    public override bool IsDirty => base.IsDirty || this.customFieldOptions.IsDirty;

    public override string GetBrokenRulesString()
    {
      string str = "Custom Field " + (object) this.FieldNumber + "\n";
      if (!base.IsValid)
        return str + base.GetBrokenRulesString();
      foreach (BusinessBase customFieldOption in (CollectionBase) this.customFieldOptions)
      {
        string brokenRulesString = customFieldOption.GetBrokenRulesString();
        if (string.Empty != brokenRulesString)
          return str + brokenRulesString;
      }
      return string.Empty;
    }

    public static CustomFieldDefinition NewCustomFieldDefinition(int categoryId, int fieldNumber)
    {
      return new CustomFieldDefinition(categoryId, fieldNumber);
    }

    public static CustomFieldDefinition NewCustomFieldDefinition(
      CustomFieldDefinitionInfo customFieldDefinitionInfo)
    {
      return new CustomFieldDefinition(customFieldDefinitionInfo);
    }

    private CustomFieldDefinition(int categoryId, int fieldNumber)
    {
      this.MarkAsChild();
      this.MarkNew();
      this.customFieldDefinitionInfo = new CustomFieldDefinitionInfo(0, categoryId, fieldNumber, string.Empty, FieldFormat.NONE, string.Empty, false, (CustomFieldOptionDefinitionInfo[]) null);
      this.BrokenRules.Assert("FieldDescriptionRequired", "Field description is a required field", this.customFieldDefinitionInfo.FieldDescription.Length < 1);
      this.BrokenRules.Assert("FieldFormatRequired", "Field format is a required field", this.customFieldDefinitionInfo.FieldFormat == FieldFormat.NONE);
    }

    private CustomFieldDefinition(
      CustomFieldDefinitionInfo customFieldDefinitionInfo)
    {
      this.MarkAsChild();
      this.MarkOld();
      this.SetInfo(customFieldDefinitionInfo);
      this.originalFieldFormat = this.FieldFormat;
    }

    public void Dispose()
    {
    }
  }
}

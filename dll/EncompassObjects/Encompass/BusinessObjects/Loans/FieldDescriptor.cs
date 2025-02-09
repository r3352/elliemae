// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.FieldDescriptor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class FieldDescriptor : IFieldDescriptor, IComparable
  {
    public static readonly FieldDescriptor Empty = new FieldDescriptor(FieldDefinition.Empty);
    private FieldDescriptor parentField;
    private FieldDefinition fieldDef;
    private string fieldId;
    private int fieldIdAsInt;
    private FieldOptions options;

    internal FieldDescriptor(FieldDefinition fieldDef)
    {
      this.fieldDef = fieldDef;
      this.fieldId = fieldDef.FieldID;
      this.fieldIdAsInt = FieldDescriptor.parseFieldId(fieldDef.FieldID);
      this.options = new FieldOptions(fieldDef);
    }

    internal FieldDescriptor(FieldDescriptor parentField, FieldDefinition fieldDef)
      : this(fieldDef)
    {
      this.parentField = parentField;
    }

    public string FieldID => this.fieldId;

    public LoanFieldFormat Format => (LoanFieldFormat) this.fieldDef.Format;

    public string Description => this.fieldDef.Description;

    public FieldOptions Options => this.options;

    public int MaxLength => this.fieldDef.MaxLength;

    public bool ReadOnly => !this.fieldDef.AllowEdit;

    public bool RequiresExclusiveLock => this.fieldDef.RequiresExclusiveLock;

    public bool MultiInstance
    {
      get => this.fieldDef.MultiInstance && this.fieldDef.FieldID == this.fieldId;
    }

    public MultiInstanceSpecifierType InstanceSpecifierType
    {
      get => (MultiInstanceSpecifierType) this.fieldDef.InstanceSpecifierType;
    }

    public bool IsBorrowerPairSpecific
    {
      get => this.fieldDef.Category == 2 || this.fieldDef.Category == 3;
    }

    public object InstanceSpecifier => this.fieldDef.InstanceSpecifier;

    public bool IsInstance => this.fieldDef.IsInstance;

    public FieldDescriptor ParentDescriptor
    {
      get
      {
        if (this.parentField != null)
          return this.parentField;
        if (this.fieldDef.ParentField == null)
          return (FieldDescriptor) null;
        this.parentField = new FieldDescriptor(this.fieldDef.ParentField);
        return this.parentField;
      }
    }

    public bool IsCustomField => CustomFieldInfo.IsCustomFieldID(this.FieldID);

    public bool IsVirtualField => this.fieldDef is VirtualField;

    public bool IsNumeric() => this.fieldDef.IsNumeric();

    public bool IsDateValued() => this.fieldDef.IsDateValued();

    public string GetFieldInstanceID(object instanceIndexOrSpecifier)
    {
      if (!this.MultiInstance)
        throw new InvalidOperationException("The current field is not multi-instance");
      return this.fieldDef.GetInstanceID(instanceIndexOrSpecifier);
    }

    public FieldDescriptor GetInstanceDescriptor(object instanceIndexOrSpecifier)
    {
      if (!this.MultiInstance)
        throw new InvalidOperationException("The current field is not multi-instance");
      return new FieldDescriptor(this, this.fieldDef.CreateInstance(instanceIndexOrSpecifier));
    }

    public bool AppliesToEdition(EncompassEdition edition)
    {
      if (edition == EncompassEdition.Banker)
        return this.fieldDef.AppliesToEdition((EncompassEdition) 5);
      if (edition == EncompassEdition.Broker)
        return this.fieldDef.AppliesToEdition((EncompassEdition) 3);
      throw new ArgumentException("Invalid EncompassEdition specified.");
    }

    public override string ToString()
    {
      if (this.FieldID == "")
        return "(Unassigned)";
      return this.Description == "" ? this.FieldID : this.Description + " (" + this.FieldID + ")";
    }

    public override bool Equals(object obj)
    {
      FieldDescriptor objA = obj as FieldDescriptor;
      return !object.Equals((object) objA, (object) null) && objA.FieldID == this.FieldID;
    }

    public override int GetHashCode() => this.FieldID.GetHashCode();

    public int CompareTo(object obj)
    {
      if (this.FieldID == "")
        return -1;
      if (!(obj is FieldDescriptor fieldDescriptor))
        return 0;
      if (fieldDescriptor.FieldID == "")
        return 1;
      if (this.fieldIdAsInt != -1 && fieldDescriptor.fieldIdAsInt == -1)
        return -1;
      if (this.fieldIdAsInt == -1 && fieldDescriptor.fieldIdAsInt != -1)
        return 1;
      return this.fieldIdAsInt != -1 ? this.fieldIdAsInt - fieldDescriptor.fieldIdAsInt : string.Compare(this.FieldID, fieldDescriptor.FieldID, true);
    }

    public string FormatValue(string value) => this.fieldDef.FormatValue(value);

    public string UnformatValue(string value) => this.fieldDef.UnformatValue(value);

    public object ConvertToNativeType(string value)
    {
      try
      {
        return this.fieldDef.ToNativeValue(value, true);
      }
      catch
      {
        return (object) null;
      }
    }

    public string ValidateInput(string value) => this.fieldDef.ValidateInput(value);

    public static FieldDescriptor CreateUndefined(string fieldId)
    {
      return new FieldDescriptor((FieldDefinition) new UndefinedField(fieldId, "(Unknown)"));
    }

    private static int parseFieldId(string fieldId)
    {
      try
      {
        return char.IsDigit(fieldId[0]) ? int.Parse(fieldId) : -1;
      }
      catch
      {
        return -1;
      }
    }
  }
}

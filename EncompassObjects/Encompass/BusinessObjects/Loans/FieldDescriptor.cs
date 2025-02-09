// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.FieldDescriptor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Summary description for FieldDescriptor.</summary>
  public class FieldDescriptor : IFieldDescriptor, IComparable
  {
    /// <summary>Public static data</summary>
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

    /// <summary>
    /// Returns the ID of the field for which this is the descriptor.
    /// </summary>
    public string FieldID => this.fieldId;

    /// <summary>Returns the format of the underlying field.</summary>
    public LoanFieldFormat Format => (LoanFieldFormat) this.fieldDef.Format;

    /// <summary>Provides a text description of the field.</summary>
    public string Description => this.fieldDef.Description;

    /// <summary>Gets the set of pre-defined options for this field.</summary>
    public FieldOptions Options => this.options;

    /// <summary>Gets the maximum length for string-based fields.</summary>
    /// <remarks>A value of 0 means there is no maximum length.</remarks>
    public int MaxLength => this.fieldDef.MaxLength;

    /// <summary>
    /// Gets a flag indicating if this field value cannot only be read through the API.
    /// </summary>
    public bool ReadOnly => !this.fieldDef.AllowEdit;

    /// <summary>
    /// Gets a flag indicating if you must have an exclusive lock on the loan to modify the field.
    /// </summary>
    public bool RequiresExclusiveLock => this.fieldDef.RequiresExclusiveLock;

    /// <summary>Indicates if this field is a multi-instance field.</summary>
    public bool MultiInstance
    {
      get => this.fieldDef.MultiInstance && this.fieldDef.FieldID == this.fieldId;
    }

    /// <summary>
    /// Gets the type of selected used to create an instance of a multi-instance field.
    /// </summary>
    public MultiInstanceSpecifierType InstanceSpecifierType
    {
      get => (MultiInstanceSpecifierType) this.fieldDef.InstanceSpecifierType;
    }

    /// <summary>Gets the boolean value IsBorrowerPairSpecific</summary>
    public bool IsBorrowerPairSpecific
    {
      get
      {
        return this.fieldDef.Category == FieldCategory.Borrower || this.fieldDef.Category == FieldCategory.Coborrower;
      }
    }

    /// <summary>
    /// Gets the instance specifier for the current field, if this is an instance of a multi-instance
    /// field.
    /// </summary>
    public object InstanceSpecifier => this.fieldDef.InstanceSpecifier;

    /// <summary>
    /// Indicates if the descriptor is an instance of a multi-instance field descriptor.
    /// </summary>
    public bool IsInstance => this.fieldDef.IsInstance;

    /// <summary>
    /// Gets the parent field descriptor for a multi-instance field.
    /// </summary>
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

    /// <summary>Indicates if this field is a custom field.</summary>
    public bool IsCustomField => CustomFieldInfo.IsCustomFieldID(this.FieldID);

    /// <summary>Indicates if this field is a virtual field.</summary>
    /// <remarks>A virtual field is a means of using a Field ID to access data that is
    /// not field data, such as Document Tracking or Milestone data. All Virtual fields are
    /// read-only.</remarks>
    public bool IsVirtualField => this.fieldDef is VirtualField;

    /// <summary>
    /// Indicates if the underlying value type is numeric (integer or decimal).
    /// </summary>
    /// <returns></returns>
    public bool IsNumeric() => this.fieldDef.IsNumeric();

    /// <summary>
    /// Indicates if the underlying value type is numeric (integer or decimal).
    /// </summary>
    /// <returns></returns>
    public bool IsDateValued() => this.fieldDef.IsDateValued();

    /// <summary>
    /// Returns the Field ID for a particular instance of a multi-instance field.
    /// </summary>
    /// <param name="instanceIndexOrSpecifier">The index or other specifier type to indicate
    /// the desired instance of a multi-instance field.</param>
    /// <returns>This method can only be called if the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.FieldDescriptor.MultiInstance" /> property
    /// is <c>true</c>.</returns>
    public string GetFieldInstanceID(object instanceIndexOrSpecifier)
    {
      if (!this.MultiInstance)
        throw new InvalidOperationException("The current field is not multi-instance");
      return this.fieldDef.GetInstanceID(instanceIndexOrSpecifier);
    }

    /// <summary>
    /// Returns a FieldDescriptor for a single instance of a multi-instance field.
    /// </summary>
    /// <param name="instanceIndexOrSpecifier">The index or other specifier type to indicate
    /// the desired instance of a multi-instance field.</param>
    /// <returns>A FieldDescriptor for the field instance.</returns>
    public FieldDescriptor GetInstanceDescriptor(object instanceIndexOrSpecifier)
    {
      if (!this.MultiInstance)
        throw new InvalidOperationException("The current field is not multi-instance");
      return new FieldDescriptor(this, this.fieldDef.CreateInstance(instanceIndexOrSpecifier));
    }

    /// <summary>
    /// Indicates if the specified field is available in the specified edition of Encompass.
    /// </summary>
    /// <param name="edition">The <see cref="T:EllieMae.Encompass.Client.EncompassEdition" /> for which the applicability is to be tested.</param>
    /// <returns>Returns <c>true</c> if the field is applicable, <c>false</c> otherwise.</returns>
    public bool AppliesToEdition(EllieMae.Encompass.Client.EncompassEdition edition)
    {
      if (edition == EllieMae.Encompass.Client.EncompassEdition.Banker)
        return this.fieldDef.AppliesToEdition(EllieMae.EMLite.Common.Licensing.EncompassEdition.Banker);
      if (edition == EllieMae.Encompass.Client.EncompassEdition.Broker)
        return this.fieldDef.AppliesToEdition(EllieMae.EMLite.Common.Licensing.EncompassEdition.Broker);
      throw new ArgumentException("Invalid EncompassEdition specified.");
    }

    /// <summary>
    /// Provides a string representation of the field descriptor.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      if (this.FieldID == "")
        return "(Unassigned)";
      return this.Description == "" ? this.FieldID : this.Description + " (" + this.FieldID + ")";
    }

    /// <summary>
    /// Checkes for equality between two field descriptors using their Field IDs.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
      FieldDescriptor objA = obj as FieldDescriptor;
      return !object.Equals((object) objA, (object) null) && objA.FieldID == this.FieldID;
    }

    /// <summary>Provides a hash code for the Field Descriptor.</summary>
    /// <returns></returns>
    public override int GetHashCode() => this.FieldID.GetHashCode();

    /// <summary>
    /// Provides a comparison between field descriptors for sorting based on the field ID.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
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

    /// <summary>Applies the field's formatting to a value.</summary>
    /// <param name="value"></param>
    /// <returns>Returns the value properly formatted for display based on the field type.</returns>
    public string FormatValue(string value) => this.fieldDef.FormatValue(value);

    /// <summary>Removes field formatting from a string.</summary>
    /// <param name="value">The value to be unformatted.</param>
    /// <returns>Returns the value stripped of all formatting characters.</returns>
    public string UnformatValue(string value) => this.fieldDef.UnformatValue(value);

    /// <summary>
    /// Converts a string value into a type based on the field's <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.FieldDescriptor.Format" />.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <returns>Returns a value of the type specified by the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.FieldDescriptor.Format" /> (Int32, Decimal,
    /// DateTime or String). If the value cannot be converted to the native type, a <c>null</c> value
    /// is returned.</returns>
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

    /// <summary>
    /// Validates that an input string is consistent with the field format.
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <returns>A re-formatted value that can be saved into the underlying loan.</returns>
    public string ValidateInput(string value) => this.fieldDef.ValidateInput(value);

    /// <summary>
    /// Allows for creation of a field descriptor. This method is meant for internal use within
    /// the Encompass application framework only.
    /// </summary>
    /// <param name="fieldId"></param>
    /// <returns></returns>
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

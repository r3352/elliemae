// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ReadOnlyField
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Represents a simple, read-only field value.</summary>
  public class ReadOnlyField : Field, IReadOnlyField
  {
    private string fieldValue;

    internal ReadOnlyField(string fieldId, string fieldValue, FieldDescriptor descriptor)
      : base(fieldId, descriptor)
    {
      this.fieldValue = fieldValue;
    }

    /// <summary>
    /// Override the Unformatted value to return the value of the lock request field.
    /// </summary>
    public override string UnformattedValue => this.fieldValue ?? "";

    /// <summary>
    /// Gets or sets the value of the field thru the ILockRequestField interface.
    /// </summary>
    /// <remarks>This method exists primarilly for COM-based clients which cannot marshal values
    /// properly to the object-valued Value property.</remarks>
    string IReadOnlyField.Value => this.FormattedValue;

    /// <summary>Sets the value of the field in the snapshot.</summary>
    internal override void setFieldValue(string value)
    {
      throw new Exception("The field is read-only and cannot be modified.");
    }
  }
}

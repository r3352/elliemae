// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequestField
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a field in a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequest" /> snapshot.
  /// </summary>
  public class LockRequestField : Field, ILockRequestField
  {
    private Hashtable fieldValues;

    internal LockRequestField(Hashtable fieldValueCollection, FieldDescriptor descriptor)
      : base(descriptor)
    {
      this.fieldValues = fieldValueCollection;
    }

    /// <summary>
    /// Override the Unformatted value to return the value of the lock request field.
    /// </summary>
    public override string UnformattedValue => string.Concat(this.fieldValues[(object) this.ID]);

    /// <summary>
    /// Gets or sets the value of the field thru the ILockRequestField interface.
    /// </summary>
    /// <remarks>This method exists primarilly for COM-based clients which cannot marshal values
    /// properly to the object-valued Value property.</remarks>
    string ILockRequestField.Value
    {
      get => this.FormattedValue;
      set => this.Value = (object) value;
    }

    /// <summary>Sets the value of the field in the snapshot.</summary>
    internal override void setFieldValue(string value)
    {
      this.fieldValues[(object) this.ID] = (object) value;
    }
  }
}

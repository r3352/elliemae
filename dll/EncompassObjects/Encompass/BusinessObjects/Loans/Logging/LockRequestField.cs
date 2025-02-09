// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequestField
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LockRequestField : Field, ILockRequestField
  {
    private Hashtable fieldValues;

    internal LockRequestField(Hashtable fieldValueCollection, FieldDescriptor descriptor)
      : base(descriptor)
    {
      this.fieldValues = fieldValueCollection;
    }

    public override string UnformattedValue => string.Concat(this.fieldValues[(object) this.ID]);

    string ILockRequestField.Value
    {
      get => this.FormattedValue;
      set => this.Value = (object) value;
    }

    internal override void setFieldValue(string value)
    {
      this.fieldValues[(object) this.ID] = (object) value;
    }
  }
}

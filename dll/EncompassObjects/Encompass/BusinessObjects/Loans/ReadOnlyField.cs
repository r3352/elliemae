// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ReadOnlyField
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class ReadOnlyField : Field, IReadOnlyField
  {
    private string fieldValue;

    internal ReadOnlyField(string fieldId, string fieldValue, FieldDescriptor descriptor)
      : base(fieldId, descriptor)
    {
      this.fieldValue = fieldValue;
    }

    public override string UnformattedValue => this.fieldValue ?? "";

    string IReadOnlyField.Value => this.FormattedValue;

    internal override void setFieldValue(string value)
    {
      throw new Exception("The field is read-only and cannot be modified.");
    }
  }
}

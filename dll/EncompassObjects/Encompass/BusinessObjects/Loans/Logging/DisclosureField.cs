// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.DisclosureField
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class DisclosureField : Field, IDisclosureField
  {
    private string unformattedValue;

    internal DisclosureField(string value, FieldDescriptor descriptor)
      : base(descriptor)
    {
      this.unformattedValue = value;
    }

    public override string UnformattedValue => this.unformattedValue;

    string IDisclosureField.Value => this.FormattedValue;

    internal override void setFieldValue(string value)
    {
      throw new Exception("This field is read-only and cannot be modified.");
    }
  }
}

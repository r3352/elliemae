// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Field
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public abstract class Field : IField
  {
    private FieldDescriptor descriptor;
    private string fieldId;

    internal Field(FieldDescriptor descriptor)
    {
      this.descriptor = descriptor;
      this.fieldId = this.descriptor.FieldID;
    }

    internal Field(string fieldId, FieldDescriptor descriptor)
    {
      this.descriptor = descriptor;
      this.fieldId = fieldId;
    }

    public string ID => this.fieldId;

    public LoanFieldFormat Format => this.descriptor.Format;

    public abstract string UnformattedValue { get; }

    public string FormattedValue => this.descriptor.FormatValue(this.UnformattedValue);

    public object Value
    {
      get => this.descriptor.ConvertToNativeType(this.UnformattedValue);
      set => this.setFieldValue(this.descriptor.ValidateInput(string.Concat(value)));
    }

    internal abstract void setFieldValue(string value);

    string IField.Value
    {
      get => this.FormattedValue;
      set => this.Value = (object) value;
    }

    public bool IsEmpty() => this.UnformattedValue == "";

    public FieldDescriptor Descriptor => this.descriptor;

    public override string ToString() => this.FormattedValue;

    public int ToInt() => Utils.ParseInt((object) this.UnformattedValue, 0);

    public Decimal ToDecimal() => Utils.ParseDecimal((object) this.UnformattedValue, 0M);

    public DateTime ToDate()
    {
      return this.Format == LoanFieldFormat.MONTHDAY ? Utils.ParseMonthDay((object) this.UnformattedValue, DateTime.MinValue) : Utils.ParseDate((object) this.UnformattedValue, DateTime.MinValue);
    }
  }
}

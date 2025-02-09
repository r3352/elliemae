// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.NonExpandableControlTypeConverter
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  public class NonExpandableControlTypeConverter : TypeConverter
  {
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
      return sourceType == typeof (string) || base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(
      ITypeDescriptorContext context,
      CultureInfo culture,
      object value)
    {
      if (value == null)
        return (object) null;
      return value is string ? (object) (context as Control).Form.FindControl(string.Concat(value)) : base.ConvertFrom(context, culture, value);
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
      return destinationType == typeof (string) || base.CanConvertTo(context, destinationType);
    }

    public override object ConvertTo(
      ITypeDescriptorContext context,
      CultureInfo culture,
      object value,
      Type destinationType)
    {
      return destinationType == typeof (string) ? (object) string.Concat(value) : base.ConvertTo(context, culture, value, destinationType);
    }
  }
}

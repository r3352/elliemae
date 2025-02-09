// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Design.NonExpandableControlTypeConverter
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace EllieMae.Encompass.Forms.Design
{
  /// <summary>
  /// TypeConverter implementation that prevents a propert from expanding in the Property Grid.
  /// </summary>
  /// <exclude />
  public class NonExpandableControlTypeConverter : TypeConverter
  {
    /// <summary>
    /// Determines if the specified type specified by the sourceType can be converted by this converter.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="sourceType"></param>
    /// <returns></returns>
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
      return sourceType == typeof (string) || base.CanConvertFrom(context, sourceType);
    }

    /// <summary>Converts an object from one type to another.</summary>
    /// <param name="context"></param>
    /// <param name="culture"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public override object ConvertFrom(
      ITypeDescriptorContext context,
      CultureInfo culture,
      object value)
    {
      if (value == null)
        return (object) null;
      return value is string ? (object) (context as Control).Form.FindControl(string.Concat(value)) : base.ConvertFrom(context, culture, value);
    }

    /// <summary>
    /// Indicates if converstions from the component type to the specified destinationType are permitted.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="destinationType"></param>
    /// <returns></returns>
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
      return destinationType == typeof (string) || base.CanConvertTo(context, destinationType);
    }

    /// <summary>Performs conversion to the specified detination type.</summary>
    /// <param name="context"></param>
    /// <param name="culture"></param>
    /// <param name="value"></param>
    /// <param name="destinationType"></param>
    /// <returns></returns>
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

// Decompiled with JetBrains decompiler
// Type: Elli.Common.Serialization.AttributeWriter
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Xml.Linq;

#nullable disable
namespace Elli.Common.Serialization
{
  public class AttributeWriter
  {
    private XElement baseElement;

    public AttributeWriter(XElement e)
    {
      this.baseElement = e != null ? e : throw new ArgumentNullException("XElement", "Element cannot be null");
    }

    public void Write(string attributeName, object value)
    {
      string str = AttributeWriter.ValueToString(value);
      if (!(str != ""))
        return;
      this.baseElement.SetAttributeValue((XName) attributeName, (object) str);
    }

    public static string ValueToString(object value)
    {
      if (object.Equals(value, (object) null))
        return "";
      switch (value)
      {
        case DateTime date:
          return AttributeWriter.dateToString(date);
        case bool flag:
          return AttributeWriter.boolToString(flag);
        default:
          return string.Concat(value);
      }
    }

    private static string dateToString(DateTime date)
    {
      if (date.Date == DateTime.MinValue.Date)
        return "";
      if (date.Date == date)
        return date.ToString("yyyy-MM-dd");
      return date.Kind == DateTimeKind.Unspecified ? date.ToString("yyyy-MM-dd HH:mm:ss") : TimeZoneInfo.ConvertTimeToUtc(date).ToString("u");
    }

    public static string boolToString(bool value) => !value ? "N" : "Y";
  }
}

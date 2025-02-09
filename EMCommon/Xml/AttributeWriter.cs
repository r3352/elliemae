// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Xml.AttributeWriter
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common.TimeZones;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Xml
{
  public class AttributeWriter
  {
    private XmlElement baseElement;

    public AttributeWriter(XmlElement e)
    {
      this.baseElement = e != null ? e : throw new ArgumentNullException("XmlElement", "Element cannot be null");
    }

    public void Write(string attributeName, object value, AttributeWriterOptions opts = AttributeWriterOptions.Default)
    {
      string str = AttributeWriter.ValueToString(value, opts);
      if (!(str != ""))
        return;
      this.baseElement.SetAttribute(attributeName, str);
    }

    public static string ValueToString(object value, AttributeWriterOptions opts = AttributeWriterOptions.Default)
    {
      switch (value)
      {
        case DateTime date:
          return AttributeWriter.dateToString(date, opts == AttributeWriterOptions.IncludeMilliSeconds);
        case DateTimeWithZone dateTimeWithZone:
          return AttributeWriter.dateTimeWithZoneToString(dateTimeWithZone, opts == AttributeWriterOptions.IncludeMilliSeconds);
        case bool flag:
          return AttributeWriter.boolToString(flag);
        default:
          return string.Concat(value);
      }
    }

    public static string dateToString(DateTime date, bool includeMilliseconds = false)
    {
      if (date.Date == DateTime.MinValue.Date)
        return "";
      if (date.Date == date)
        return date.ToString("yyyy-MM-dd");
      return date.Kind == DateTimeKind.Unspecified ? (includeMilliseconds ? date.ToString("yyyy-MM-dd HH:mm:ss.fff") : date.ToString("yyyy-MM-dd HH:mm:ss")) : (includeMilliseconds ? date.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss.fffZ") : date.ToUniversalTime().ToString("u"));
    }

    private static string dateTimeWithZoneToString(
      DateTimeWithZone dateTimeWithZone,
      bool includeMilliseconds = false)
    {
      DateTime dateTime1 = dateTimeWithZone.DateTime;
      DateTime date1 = dateTime1.Date;
      dateTime1 = DateTime.MinValue;
      DateTime date2 = dateTime1.Date;
      if (date1 == date2)
        return "";
      DateTime dateTime2 = dateTimeWithZone.DateTime;
      if (dateTime2.Date == dateTimeWithZone.DateTime)
      {
        dateTime2 = dateTimeWithZone.DateTime;
        return dateTime2.ToString("yyyy-MM-dd");
      }
      if (includeMilliseconds)
      {
        dateTime2 = TimeZoneInfo.ConvertTimeToUtc(dateTimeWithZone.DateTime, dateTimeWithZone.TimeZone);
        return dateTime2.ToString("yyyy-MM-dd HH:mm:ss.fffZ");
      }
      dateTime2 = TimeZoneInfo.ConvertTimeToUtc(dateTimeWithZone.DateTime, dateTimeWithZone.TimeZone);
      return dateTime2.ToString("u");
    }

    public static string boolToString(bool value) => !value ? "N" : "Y";
  }
}

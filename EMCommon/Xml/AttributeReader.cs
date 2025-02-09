// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Xml.AttributeReader
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.TimeZones;
using System;
using System.Globalization;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Xml
{
  public class AttributeReader
  {
    private XmlElement baseElement;

    public AttributeReader(XmlElement e)
    {
      this.baseElement = e != null ? e : throw new ArgumentNullException("XmlElement", "Element cannot be null");
    }

    public string GetString(string attributeName, bool throwOnMissing)
    {
      XmlAttribute attributeNode = this.baseElement.GetAttributeNode(attributeName);
      if (attributeNode == null & throwOnMissing)
        throw new XmlException("Missing required attribute '" + attributeName + "' from element '" + this.baseElement.Name + "'");
      return attributeNode?.Value;
    }

    public string GetString(string attributeName, string defaultValue)
    {
      return this.GetString(attributeName, false) ?? defaultValue;
    }

    public string GetString(string attributeName) => this.GetString(attributeName, "");

    public int GetInteger(string attributeName, bool throwOnMissing)
    {
      return (int) this.GetValue(attributeName, typeof (int), throwOnMissing);
    }

    public int GetInteger(string attributeName, int defaultValue)
    {
      return (int) this.GetValueWithDefault(attributeName, typeof (int), (object) defaultValue);
    }

    public int GetInteger(string attributeName) => this.GetInteger(attributeName, 0);

    public int? GetNullableInteger(string attributeName)
    {
      return (int?) this.GetValueWithDefault(attributeName, typeof (int?), (object) null);
    }

    public long GetLong(string attributeName, bool throwOnMissing)
    {
      return (long) this.GetValue(attributeName, typeof (long), throwOnMissing);
    }

    public long GetLong(string attributeName, long defaultValue)
    {
      return (long) this.GetValueWithDefault(attributeName, typeof (long), (object) defaultValue);
    }

    public long GetLong(string attributeName) => this.GetLong(attributeName, 0L);

    public float GetFloat(string attributeName) => this.GetFloat(attributeName, 0.0f);

    public float GetFloat(string attributeName, bool throwOnMissing)
    {
      return (float) this.GetValue(attributeName, typeof (float), throwOnMissing);
    }

    public float GetFloat(string attributeName, float defaultValue)
    {
      return (float) this.GetValueWithDefault(attributeName, typeof (float), (object) defaultValue);
    }

    public double GetDouble(string attributeName, bool throwOnMissing)
    {
      return (double) this.GetValue(attributeName, typeof (double), throwOnMissing);
    }

    public double GetDouble(string attributeName, double defaultValue)
    {
      return (double) this.GetValueWithDefault(attributeName, typeof (double), (object) defaultValue);
    }

    public Decimal GetDecimal(string attributeName, bool throwOnMissing)
    {
      return (Decimal) this.GetValue(attributeName, typeof (Decimal), throwOnMissing);
    }

    public Decimal GetDecimal(string attributeName, Decimal defaultValue)
    {
      return (Decimal) this.GetValueWithDefault(attributeName, typeof (Decimal), (object) defaultValue);
    }

    public DateTime GetDate(string attributeName) => this.GetDate(attributeName, DateTime.MinValue);

    public DateTimeWithZone GetDateTimeWithZone(string attributeName, System.TimeZoneInfo timeZoneInfo)
    {
      DateTime date = this.GetDate(attributeName, DateTime.MinValue);
      return date.Date == date || date.Kind == DateTimeKind.Unspecified ? DateTimeWithZone.Create(date, timeZoneInfo) : DateTimeWithZone.ConvertToTimeZone(date, timeZoneInfo);
    }

    public DateTime GetDate(string attributeName, bool throwOnMissing)
    {
      string valueAsString = this.GetString(attributeName, throwOnMissing);
      try
      {
        return AttributeReader.ParseDateTime(valueAsString);
      }
      catch
      {
        throw new XmlException("Invalid date format for attribute '" + attributeName + "' of element '" + this.baseElement.Name + "'");
      }
    }

    public DateTime GetDate(string attributeName, DateTime defaultValue)
    {
      string valueAsString = this.GetString(attributeName, false);
      if ((valueAsString ?? "") == "")
        return defaultValue;
      try
      {
        return AttributeReader.ParseDateTime(valueAsString);
      }
      catch
      {
        throw new XmlException("Invalid date format for attribute '" + attributeName + "' of element '" + this.baseElement.Name + "'");
      }
    }

    public DateTime GetUtcDate(string attributeName)
    {
      return this.GetUtcDate(attributeName, DateTime.MinValue);
    }

    public DateTime GetUtcDate(string attributeName, DateTime defaultValue)
    {
      string valueAsString = this.GetString(attributeName, false);
      if (valueAsString != null)
      {
        if (!((valueAsString ?? "") == ""))
        {
          try
          {
            DateTime? utcDateTime = AttributeReader.ParseUtcDateTime(valueAsString);
            return !utcDateTime.HasValue ? defaultValue : utcDateTime.Value;
          }
          catch
          {
            throw new XmlException("Invalid date format for attribute '" + attributeName + "' of element '" + this.baseElement.Name + "'");
          }
        }
      }
      return defaultValue;
    }

    public DateTime? GetNullableUtcDate(string attributeName)
    {
      string valueAsString = this.GetString(attributeName, false);
      if (string.IsNullOrEmpty(valueAsString))
        return new DateTime?();
      try
      {
        return AttributeReader.ParseUtcDateTime(valueAsString);
      }
      catch
      {
        throw new XmlException("Invalid date format for attribute '" + attributeName + "' of element '" + this.baseElement.Name + "'");
      }
    }

    public Guid GetGuid(string attributeName, Guid defaultValue)
    {
      string g = this.GetString(attributeName, false);
      if ((g ?? "") == "")
        return defaultValue;
      try
      {
        return new Guid(g);
      }
      catch
      {
        throw new XmlException("Invalid guid format for attribute '" + attributeName + "' of element '" + this.baseElement.Name + "'");
      }
    }

    public bool GetBoolean(string attributeName, bool defaultValue)
    {
      string str = this.GetString(attributeName, defaultValue ? "Y" : "N");
      return str == "Y" || str == "1";
    }

    public bool GetBoolean(string attributeName) => this.GetBoolean(attributeName, false);

    public object GetValue(string attributeName, Type type, bool throwOnMissing)
    {
      return this.changeType(attributeName, this.GetString(attributeName, throwOnMissing), type);
    }

    public object GetValueWithDefault(string attributeName, Type type, object defaultValue)
    {
      string str = this.GetString(attributeName, false);
      switch (str)
      {
        case null:
          return defaultValue;
        case "":
          if (type != typeof (string))
            return defaultValue;
          break;
      }
      return this.changeType(attributeName, str, type);
    }

    private object changeType(string attributeName, string value, Type targetType)
    {
      try
      {
        Type underlyingType = Nullable.GetUnderlyingType(targetType);
        if (underlyingType != (Type) null)
        {
          if (value == null)
            return (object) null;
          targetType = underlyingType;
        }
        return Convert.ChangeType((object) value, targetType);
      }
      catch
      {
        throw new XmlException("Invalid value format for attribute '" + attributeName + "' of element '" + this.baseElement.Name + "'");
      }
    }

    public static DateTime ParseDateTime(string valueAsString)
    {
      try
      {
        if (string.Equals(valueAsString, Utils.UTCDbMaxDate))
          return DateTime.MaxValue;
        DateTime dateTime;
        return FastDateTimeParser.TryParse(valueAsString, DateTimeStyles.None, out dateTime) ? dateTime : DateTime.Parse(valueAsString, Utils.StandardDateFormatProvider, DateTimeStyles.AllowWhiteSpaces);
      }
      catch
      {
        throw new Exception("Invalid date format for value '" + valueAsString + "'");
      }
    }

    public static DateTime? ParseUtcDateTime(string valueAsString)
    {
      try
      {
        DateTime dateTime;
        if (FastDateTimeParser.TryParse(valueAsString, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out dateTime))
          return new DateTime?(dateTime);
        if (!DateTime.TryParse(valueAsString, out dateTime))
          return new DateTime?();
        if (dateTime < Utils.DbMinDate || dateTime > Utils.DbMaxDate)
          return new DateTime?();
        System.TimeZoneInfo systemTimeZoneById = System.TimeZoneInfo.FindSystemTimeZoneById(System.TimeZoneInfo.Local.Id);
        if (dateTime.Date == dateTime || dateTime.Kind == DateTimeKind.Unspecified)
          return new DateTime?(new DateTime(dateTime.Ticks, DateTimeKind.Utc));
        return !systemTimeZoneById.IsInvalidTime(dateTime) ? new DateTime?(System.TimeZoneInfo.ConvertTimeToUtc(dateTime)) : new DateTime?(System.TimeZoneInfo.ConvertTimeToUtc(dateTime.AddHours(1.0)));
      }
      catch
      {
        throw new Exception("Invalid date format for value '" + valueAsString + "'");
      }
    }
  }
}

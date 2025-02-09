// Decompiled with JetBrains decompiler
// Type: Elli.Common.Serialization.AttributeReader
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using Elli.Common.Extensions;
using Elli.Common.Globalization;
using System;
using System.Globalization;
using System.Xml.Linq;

#nullable disable
namespace Elli.Common.Serialization
{
  public class AttributeReader
  {
    private XElement baseElement;

    public AttributeReader(XElement e)
    {
      this.baseElement = e != null ? e : throw new ArgumentNullException("XElement", "Element cannot be null");
    }

    public string GetString(string attributeName, bool throwOnMissing)
    {
      XAttribute xattribute = this.baseElement.Attribute((XName) attributeName);
      if (xattribute == null & throwOnMissing)
        throw new FormatException("Missing required attribute '" + attributeName + "' from element '" + (object) this.baseElement.Name + "'");
      return xattribute?.Value;
    }

    public string GetString(string attributeName, string defaultValue)
    {
      return this.GetString(attributeName, false) ?? defaultValue;
    }

    public string GetString(string attributeName) => this.GetString(attributeName, "");

    public int GetInteger(string attributeName, bool throwOnMissing)
    {
      return this.GetValue<int>(attributeName, throwOnMissing);
    }

    public int GetInteger(string attributeName, int defaultValue)
    {
      return this.GetValueWithDefault<int>(attributeName, defaultValue);
    }

    public int GetInteger(string attributeName) => this.GetInteger(attributeName, 0);

    public long GetLong(string attributeName, bool throwOnMissing)
    {
      return this.GetValue<long>(attributeName, throwOnMissing);
    }

    public long GetLong(string attributeName, long defaultValue)
    {
      return this.GetValueWithDefault<long>(attributeName, defaultValue);
    }

    public long GetLong(string attributeName) => this.GetLong(attributeName, 0L);

    public float GetFloat(string attributeName) => this.GetFloat(attributeName, 0.0f);

    public float GetFloat(string attributeName, bool throwOnMissing)
    {
      return this.GetValue<float>(attributeName, throwOnMissing);
    }

    public float GetFloat(string attributeName, float defaultValue)
    {
      return this.GetValueWithDefault<float>(attributeName, defaultValue);
    }

    public double GetDouble(string attributeName, bool throwOnMissing)
    {
      return this.GetValue<double>(attributeName, throwOnMissing);
    }

    public double GetDouble(string attributeName, double defaultValue)
    {
      return this.GetValueWithDefault<double>(attributeName, defaultValue);
    }

    public Decimal GetDecimal(string attributeName, bool throwOnMissing)
    {
      return this.GetValue<Decimal>(attributeName, throwOnMissing);
    }

    public Decimal GetDecimal(string attributeName, Decimal defaultValue)
    {
      return this.GetValueWithDefault<Decimal>(attributeName, defaultValue);
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
        throw new FormatException("Invalid date format for attribute '" + attributeName + "' of element '" + (object) this.baseElement.Name + "'");
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
        throw new FormatException("Invalid date format for attribute '" + attributeName + "' of element '" + (object) this.baseElement.Name + "'");
      }
    }

    public DateTime GetDate(string attributeName) => this.GetDate(attributeName, DateTime.MinValue);

    public DateTime? GetNullableDate(string attributeName)
    {
      DateTime date = this.GetDate(attributeName, DateTime.MinValue);
      return date == DateTime.MinValue ? new DateTime?() : new DateTime?(date);
    }

    public bool GetBoolean(string attributeName, bool defaultValue)
    {
      string str = this.GetString(attributeName, defaultValue ? "Y" : "N");
      return str == "Y" || str == "1";
    }

    public bool GetBoolean(string attributeName) => this.GetBoolean(attributeName, false);

    public Guid? GetNullableGuid(string attributeName)
    {
      string input = this.GetString(attributeName, "");
      if (input == "")
        return new Guid?();
      Guid result;
      return !Guid.TryParse(input, out result) ? new Guid?() : new Guid?(result);
    }

    public Guid GetGuid(string attributeName)
    {
      return (this.GetNullableGuid(attributeName) ?? throw new FormatException("The attribute '" + attributeName + "' cannot be parsed to a Guid")).Value;
    }

    public T GetEnum<T>(string attributeName, T defaultValue) where T : struct
    {
      string str = this.GetString(attributeName, (string) null);
      if (str == null)
        return defaultValue;
      T result = default (T);
      return Enum.TryParse<T>(str, out result) ? result : defaultValue;
    }

    public T GetEnum<T>(string attributeName) where T : struct
    {
      return this.GetEnum<T>(attributeName, default (T));
    }

    public object GetValue(Type propertyType, string attributeName)
    {
      if (propertyType.IsNullableType())
        return this.GetValue(propertyType.GetNullableValueType(), attributeName);
      XAttribute xattribute = this.baseElement.Attribute((XName) attributeName);
      if (xattribute == null)
        return (object) null;
      if (propertyType == typeof (string))
        return (object) this.GetString(attributeName);
      if (propertyType == typeof (int))
        return (object) this.GetInteger(attributeName, true);
      if (propertyType == typeof (bool))
        return (object) this.GetBoolean(attributeName);
      if (propertyType == typeof (DateTime))
        return (object) this.GetDate(attributeName, true);
      if (propertyType == typeof (Decimal))
        return (object) this.GetDecimal(attributeName, true);
      if (propertyType == typeof (double))
        return (object) this.GetDouble(attributeName, true);
      if (propertyType == typeof (float))
        return (object) this.GetFloat(attributeName, true);
      if (propertyType == typeof (long))
        return (object) this.GetLong(attributeName, true);
      if (propertyType == typeof (Guid))
        return (object) this.GetGuid(attributeName);
      return propertyType.IsEnum ? this.GetEnum(attributeName, propertyType) : this.changeType(propertyType, attributeName, xattribute.Value);
    }

    private object GetEnum(string attributeName, Type enumType)
    {
      string str = this.GetString(attributeName, "");
      return Enum.Parse(enumType, str);
    }

    protected T GetValue<T>(string attributeName, bool throwOnMissing) where T : IConvertible
    {
      return this.changeType<T>(attributeName, this.GetString(attributeName, throwOnMissing));
    }

    protected T GetValueWithDefault<T>(string attributeName, T defaultValue) where T : IConvertible
    {
      string str = this.GetString(attributeName, false);
      switch (str)
      {
        case null:
          return defaultValue;
        case "":
          if (typeof (T) != typeof (string))
            return defaultValue;
          break;
      }
      return this.changeType<T>(attributeName, str);
    }

    protected T changeType<T>(string attributeName, string value) where T : IConvertible
    {
      try
      {
        return (T) Convert.ChangeType((object) value, typeof (T));
      }
      catch
      {
        throw new FormatException("Invalid value format for attribute '" + attributeName + "' of element '" + (object) this.baseElement.Name + "'");
      }
    }

    private object changeType(Type type, string attributeName, string value)
    {
      try
      {
        return Convert.ChangeType((object) value, type);
      }
      catch
      {
        throw new FormatException("Invalid value format for attribute '" + attributeName + "' of element '" + (object) this.baseElement.Name + "'");
      }
    }

    public static DateTime ParseDateTime(string valueAsString)
    {
      try
      {
        return DateTime.Parse(valueAsString, DateTimeConverter.StandardDateFormatProvider, DateTimeStyles.AllowWhiteSpaces);
      }
      catch
      {
        throw new Exception("Invalid date format for value '" + valueAsString + "'");
      }
    }
  }
}

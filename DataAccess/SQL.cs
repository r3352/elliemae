// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.SQL
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public sealed class SQL
  {
    public static readonly DateTime MinSmallDatetime = new DateTime(1900, 1, 1, 0, 0, 0);
    public static readonly DateTime MaxSmallDatetime = new DateTime(2079, 6, 1, 0, 0, 0);
    public static readonly DateTime MinDatetime = new DateTime(1753, 1, 1, 0, 0, 0);
    public static readonly DateTime MaxDatetime = new DateTime(9999, 12, 31, 11, 59, 59);

    private SQL()
    {
    }

    public static string Encode(object value)
    {
      switch (value)
      {
        case null:
        case DBNull _:
          return "NULL";
        case string _:
          return SQL.EncodeString((string) value);
        case DateTime dateTime:
          return SQL.EncodeDateTime(dateTime);
        case bool flag:
          return SQL.EncodeBoolean(flag);
        case Array _:
          return SQL.EncodeArray((Array) value);
        case ICollection _:
          return SQL.EncodeArray((Array) new ArrayList((ICollection) value).ToArray());
        default:
          return value.ToString();
      }
    }

    public static string Encode(object value, object nullValue)
    {
      return value == null || object.Equals(value, nullValue) ? "NULL" : SQL.Encode(value);
    }

    public static string[] EncodeString(string[] values, bool encloseQuotes)
    {
      if (values == null)
        return values;
      List<string> stringList = new List<string>();
      for (int index = 0; index < values.Length; ++index)
        stringList.Add(SQL.EncodeString(values[index], encloseQuotes));
      return stringList.ToArray();
    }

    public static string EncodeString(string value, bool encloseQuotes)
    {
      if (value == null)
        return "NULL";
      return encloseQuotes ? "'" + value.Replace("'", "''") + "'" : value.Replace("'", "''");
    }

    public static string EncodeString(string value) => SQL.EncodeString(value, true);

    public static string EncodeString(string value, string nullValue)
    {
      return SQL.EncodeString(value, -1, nullValue);
    }

    public static string EncodeString(string value, int maxLength)
    {
      return SQL.EncodeString(value, maxLength, (string) null);
    }

    public static string EncodeString(string value, int maxLength, bool encloseQuotes)
    {
      return maxLength > 0 && value.Length > maxLength ? SQL.EncodeString(value.Substring(0, maxLength), encloseQuotes) : SQL.EncodeString(value, encloseQuotes);
    }

    public static string EncodeString(string value, int maxLength, string nullValue)
    {
      if (value == nullValue)
        return "NULL";
      return maxLength > 0 && value.Length > maxLength ? SQL.EncodeString(value.Substring(0, maxLength)) : SQL.EncodeString(value);
    }

    public static string EncodeBooleanToYN(bool? value)
    {
      return !value.GetValueOrDefault() ? "'N'" : "'Y'";
    }

    public static string EncodeBoolean(bool value) => !value ? "'F'" : "'T'";

    public static string EncodeBooleanFull(bool value) => !value ? "'False'" : "'True'";

    public static string EncodeFlag(bool value) => !value ? "0" : "1";

    public static string EncodeDateTime(DateTime value, DateTime nullValue)
    {
      return SQL.EncodeDateTime(value, nullValue, false);
    }

    public static string EncodeDateTime(
      DateTime value,
      DateTime nullValue,
      bool useSmallDateTimeFormat)
    {
      if (value == nullValue || useSmallDateTimeFormat && (value < SQL.MinSmallDatetime || value > SQL.MaxSmallDatetime) || value < SQL.MinDatetime || value > SQL.MaxDatetime)
        return "NULL";
      return useSmallDateTimeFormat ? "'" + string.Format("{0:0000}{1:00}{2:00} {3:00}:{4:00}", (object) value.Year, (object) value.Month, (object) value.Day, (object) value.Hour, (object) value.Minute) + "'" : "'" + string.Format("{0:0000}{1:00}{2:00} {3:00}:{4:00}:{5:00}.{6:000}", (object) value.Year, (object) value.Month, (object) value.Day, (object) value.Hour, (object) value.Minute, (object) value.Second, (object) value.Millisecond) + "'";
    }

    public static string EncodeDateTime(DateTime value)
    {
      return SQL.EncodeDateTime(value, DateTime.MinValue, false);
    }

    public static string EncodeDateTime(DateTime value, bool useSmallDateTimeFormat)
    {
      return SQL.EncodeDateTime(value, DateTime.MinValue, useSmallDateTimeFormat);
    }

    public static string EncodeDateRange(Range<DateTime> dateRange)
    {
      return SQL.EncodeDateRange(dateRange, false);
    }

    public static string EncodeDateRange(Range<DateTime> dateRange, bool useSmallDateTimeFormat)
    {
      if (dateRange == null)
        return "IS NULL";
      if (dateRange.Minimum > DateTime.MinValue && dateRange.Maximum < DateTime.MaxValue)
        return "BETWEEN " + SQL.EncodeDateTime(dateRange.Minimum, useSmallDateTimeFormat) + " AND " + SQL.EncodeDateTime(dateRange.Maximum.AddMinutes(-1.0), useSmallDateTimeFormat);
      if (dateRange.Minimum > DateTime.MinValue)
        return ">= " + SQL.EncodeDateTime(dateRange.Minimum, useSmallDateTimeFormat);
      return dateRange.Maximum < DateTime.MaxValue ? "< " + SQL.EncodeDateTime(dateRange.Maximum, useSmallDateTimeFormat) : "IS NOT NULL";
    }

    public static string EncodeArray(Array data)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int lowerBound = data.GetLowerBound(0);
      int upperBound = data.GetUpperBound(0);
      for (int index = lowerBound; index <= upperBound; ++index)
      {
        stringBuilder.Append(SQL.Encode(data.GetValue(index)));
        if (index < upperBound)
          stringBuilder.Append(",");
      }
      return stringBuilder.ToString();
    }

    public static string EncodeToSHA1(string data)
    {
      return Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.Unicode.GetBytes(data)));
    }

    public static string EncodeToMD5(string data)
    {
      return Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(Encoding.Unicode.GetBytes(data)));
    }

    public static object EncodeDecimal(string value)
    {
      return string.IsNullOrEmpty(value) ? (object) "NULL" : (object) Decimal.Parse(value);
    }

    public static string Escape(string value)
    {
      value = value.Replace("'", "''");
      value = value.Replace("[", "[[]");
      value = value.Replace("%", "[%]");
      value = value.Replace("_", "[_]");
      return value;
    }

    public static object Decode(object value) => SQL.Decode(value, (object) null);

    public static object Decode(object value, object defaultValue)
    {
      switch (value)
      {
        case DBNull _:
        case null:
          return defaultValue;
        default:
          return value;
      }
    }

    public static string DecodeString(object value)
    {
      return string.Concat(SQL.Decode(value, (object) ""));
    }

    public static string DecodeString(object value, string defaultValue)
    {
      return string.Concat(SQL.Decode(value, (object) defaultValue));
    }

    public static DateTime DecodeDateTime(object value)
    {
      return Convert.ToDateTime(SQL.Decode(value, (object) DateTime.MinValue));
    }

    public static DateTime DecodeDateTimeWithKind(object value, DateTimeKind dateTimeKind)
    {
      switch (value)
      {
        case DBNull _:
        case null:
          return DateTime.MinValue;
        default:
          return DateTime.SpecifyKind(Convert.ToDateTime(value), dateTimeKind);
      }
    }

    public static DateTime DecodeDateTimeWithKind(
      object value,
      DateTimeKind dateTimeKind,
      DateTime defaultValue)
    {
      switch (value)
      {
        case DBNull _:
        case null:
          return defaultValue;
        default:
          return DateTime.SpecifyKind(Convert.ToDateTime(value), dateTimeKind);
      }
    }

    public static DateTime DecodeDateTime(object value, DateTime defaultValue)
    {
      return Convert.ToDateTime(SQL.Decode(value, (object) defaultValue));
    }

    public static int DecodeInt(object value) => Convert.ToInt32(SQL.Decode(value, (object) 0));

    public static int DecodeInt(object value, int defaultValue)
    {
      return Convert.ToInt32(SQL.Decode(value, (object) defaultValue));
    }

    public static long DecodeLong(object value) => Convert.ToInt64(SQL.Decode(value, (object) 0L));

    public static long DecodeLong(object value, long defaultValue)
    {
      return Convert.ToInt64(SQL.Decode(value, (object) defaultValue));
    }

    public static Decimal DecodeDecimal(object value)
    {
      return Convert.ToDecimal(SQL.Decode(value, (object) 0M));
    }

    public static Decimal DecodeDecimal(object value, Decimal defaultValue)
    {
      return Convert.ToDecimal(SQL.Decode(value, (object) defaultValue));
    }

    public static bool DecodeBoolean(object value) => SQL.DecodeBoolean(value, false);

    public static bool DecodeBoolean(object value, bool defaultValue)
    {
      object obj = SQL.Decode(value, (object) defaultValue);
      switch (obj)
      {
        case bool flag:
          return flag;
        case string _:
          return obj.ToString().StartsWith("Y") || obj.ToString().StartsWith("T");
        default:
          return Convert.ToInt32(obj).Equals(1);
      }
    }

    public static float DecodeSingle(object value)
    {
      return Convert.ToSingle(SQL.Decode(value, (object) 0.0f));
    }

    public static float DecodeSingle(object value, float defaultValue)
    {
      return Convert.ToSingle(SQL.Decode(value, (object) defaultValue));
    }

    public static double DecodeDouble(object value)
    {
      return Convert.ToDouble(SQL.Decode(value, (object) 0));
    }

    public static double DecodeDouble(object value, double defaultValue)
    {
      return Convert.ToDouble(SQL.Decode(value, (object) defaultValue));
    }

    public static T DecodeEnum<T>(object value)
    {
      return value is string ? (T) Enum.Parse(typeof (T), (string) value, true) : (T) Enum.ToObject(typeof (T), value);
    }

    public static T DecodeEnum<T>(object value, T defaultValue)
    {
      switch (value)
      {
        case null:
        case DBNull _:
          return defaultValue;
        default:
          try
          {
            return SQL.DecodeEnum<T>(value);
          }
          catch
          {
            return defaultValue;
          }
      }
    }

    public static string ToOrderByClause(SortField[] sortFields)
    {
      return SQL.ToOrderByClause(sortFields, (ICriterionTranslator) null);
    }

    public static string ToOrderByClause(
      SortField[] sortFields,
      ICriterionTranslator fieldTranslator)
    {
      if (sortFields == null || sortFields.Length == 0)
        return "";
      string str = "";
      for (int index = 0; index < sortFields.Length; ++index)
        str = str + (index == 0 ? "" : ", ") + sortFields[index].ToSQLClause(fieldTranslator);
      return "order by " + str;
    }

    public static string EncodeSysName(string name)
    {
      string str = "";
      for (int index = 0; index < name.Length; ++index)
        str = char.IsLetterOrDigit(name[index]) || name[index] == '_' ? str + name[index].ToString() : str + "_";
      return str;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Common.Utility
// Assembly: Elli.CalculationEngine.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BBD0C9BB-76EB-4848-9A1B-D338F49271A1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Common.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Elli.CalculationEngine.Common
{
  public class Utility
  {
    public static readonly IFormatProvider StandardDateFormatProvider;
    private static List<string> pathWhiteList = new List<string>();
    private static HashSet<string> calculationIdentityWhitelist = new HashSet<string>();
    private static HashSet<string> assemblyNameList = new HashSet<string>();

    public static bool FilePathHasInvalidChars(string path)
    {
      return !string.IsNullOrEmpty(path) && path.IndexOfAny(Path.GetInvalidPathChars()) >= 0;
    }

    public static void AddCalculationToIdentityWhitelist(string id)
    {
    }

    public static void ClearIdentityWhitelist() => Utility.calculationIdentityWhitelist.Clear();

    public static void AddPathToWhiteList(string path)
    {
      if (Utility.pathWhiteList.Contains(path))
        return;
      Utility.pathWhiteList.Add(path);
    }

    public static void ClearPathWhiteList() => Utility.pathWhiteList.Clear();

    public static void AddAssemblyNameToAssemblyList(string id)
    {
      if (Utility.assemblyNameList.Contains(id))
        return;
      Utility.assemblyNameList.Add(id);
    }

    public static void ClearAssemblyList() => Utility.assemblyNameList.Clear();

    public static string ValidateAssemblyPath(string path)
    {
      string empty = string.Empty;
      if (string.IsNullOrEmpty(path))
        throw new Exception(string.Format("Path Validation Error: Path {0} is null or empty.", (object) path));
      string fullPath = path.IndexOfAny(Path.GetInvalidPathChars()) < 0 ? Path.GetFullPath(path) : throw new Exception(string.Format("Path Validation Error: Path {0} contains invalid characters.", (object) path));
      if (Utility.pathWhiteList.Exists((Predicate<string>) (p => fullPath.StartsWith(p))))
        return fullPath;
      throw new Exception(string.Format("Path Validation Error: Path {0} does not start with a verified path.", (object) path));
    }

    public static string ValidateCalculationIdentity(string id) => id;

    public static string ValidateAssemblyName(string assemblyName)
    {
      string empty = string.Empty;
      if (Utility.assemblyNameList.Contains(assemblyName))
        return assemblyName;
      string str = string.Empty;
      foreach (string assemblyName1 in Utility.assemblyNameList)
        str = str + ", " + assemblyName1;
      throw new Exception(string.Format("Assembly Name Validation Error: '{0}' is not a valid assembly name. ({1})", (object) assemblyName, (object) str));
    }

    public static string ApplyFieldFormatting(string value, string format, bool formatEmptyValues)
    {
      try
      {
        if (!formatEmptyValues && value == "")
          return "";
        switch (format)
        {
          case "DATE":
            return value == "" ? "//" : Utility.FormatDateValue(value);
          case "DATETIME":
            return value == "" ? "//" : Utility.FormatDateValue(value, true);
          case "DECIMAL":
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString();
            break;
          case "DECIMAL_1":
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString("N1");
            break;
          case "DECIMAL_10":
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString("N10");
            break;
          case "DECIMAL_2":
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString("N2");
            break;
          case "DECIMAL_3":
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString("N3");
            break;
          case "DECIMAL_4":
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString("N4");
            break;
          case "DECIMAL_5":
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString("N5");
            break;
          case "DECIMAL_6":
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString("N6");
            break;
          case "DECIMAL_7":
            if (value != "")
              return Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null).ToString("N7");
            break;
          case "INTEGER":
            if (value != "")
            {
              Decimal num = Decimal.Parse(value, NumberStyles.Any, (IFormatProvider) null);
              return num == 0M && value == "0" ? "0" : num.ToString("#,#");
            }
            break;
        }
        return value;
      }
      catch
      {
        return "";
      }
    }

    public static string ConvertToInternalValue(string value, ValueType type)
    {
      if (value == "")
        return "";
      switch (type)
      {
        case ValueType.Integer:
          return value == "0" ? value : Utility.ParseDecimal((object) value, true).ToString("#");
        case ValueType.Decimal:
          return Utility.ParseDecimal((object) value, true).ToString();
        case ValueType.Date:
          return Utility.FormatDateValue(value);
        case ValueType.DateTime:
          return Utility.DateTimeToString(Utility.ParseDate((object) value));
        default:
          return value;
      }
    }

    public static object ConvertToNativeValue(string value, ValueType type, bool throwOnError)
    {
      switch (type)
      {
        case ValueType.Integer:
          try
          {
            return (object) Utility.ParseInt((object) value, true);
          }
          catch
          {
            if (!throwOnError)
              return (object) 0;
            throw;
          }
        case ValueType.Decimal:
          return (object) Utility.ParseDecimal((object) value, throwOnError);
        case ValueType.Date:
          return (object) Utility.ParseDate((object) value, throwOnError);
        case ValueType.DateTime:
          return (object) Utility.ParseDate((object) value, throwOnError);
        case ValueType.Boolean:
          return (object) Utility.ParseBoolean((object) value, throwOnError, new bool?(false));
        case ValueType.NullableBoolean:
          return (object) Utility.ParseBoolean((object) value, throwOnError, new bool?());
        case ValueType.NullableDecimal:
          return (object) Utility.ParseNullableDecimal((object) value, throwOnError);
        case ValueType.NullableInteger:
          try
          {
            return (object) Utility.ParseNullableInt((object) value, true);
          }
          catch
          {
            if (!throwOnError)
              return (object) null;
            throw;
          }
        case ValueType.NullableDate:
          return (object) Utility.ParseNullableDate((object) value, throwOnError, new DateTime?());
        case ValueType.NullableDateTime:
          return (object) Utility.ParseNullableDate((object) value, throwOnError, new DateTime?());
        case ValueType.Short:
          try
          {
            return (object) Utility.ParseShort((object) value, true);
          }
          catch
          {
            if (!throwOnError)
              return (object) 0;
            throw;
          }
        case ValueType.Byte:
          try
          {
            return (object) Utility.ParseByte((object) value, true);
          }
          catch
          {
            if (!throwOnError)
              return (object) 0;
            throw;
          }
        case ValueType.NullableShort:
          try
          {
            return (object) Utility.ParseNullableShort((object) value, true);
          }
          catch
          {
            if (!throwOnError)
              return (object) null;
            throw;
          }
        default:
          return (object) value ?? (object) "";
      }
    }

    public static object ConvertToNativeValue(string value, ValueType type, object defaultValue)
    {
      try
      {
        return Utility.ConvertToNativeValue(value, type, true);
      }
      catch
      {
        return defaultValue;
      }
    }

    public static string DateTimeToString(DateTime date)
    {
      return date.Kind == DateTimeKind.Unspecified ? date.ToString("yyyy-MM-dd HH:mm:ss") : date.ToUniversalTime().ToString("u");
    }

    public static string FormatDateValue(string val) => Utility.FormatDateValue(val, false);

    public static string FormatDateValue(string val, bool includeTime)
    {
      if (val.Length > 0 && val.Length < 4 && val != "//")
        throw new FormatException("Date format is invalid.");
      if (val.Length == 0 || val == "//")
        return "//";
      if (val.Length == 4 && Utility.ParseInt((object) val, -1) > 0)
        val = "01/01/" + val;
      if (val.Length < 6)
        throw new FormatException("Date format is invalid.");
      if (Utility.ParseInt((object) val, -1) > 0)
        val = val.Substring(0, 2) + "/" + val.Substring(2, 2) + "/" + val.Substring(4);
      DateTime date = Utility.ParseDate((object) val, true);
      if (date.Year < 1900 || date.Year > 2199)
        throw new FormatException("Date year range must be between 1900 and 2199");
      if (includeTime && date.Second > 0)
        return date.ToString("MM/dd/yyyy hh:mm:ss tt");
      return includeTime ? date.ToString("MM/dd/yyyy hh:mm tt") : date.ToString("MM/dd/yyyy");
    }

    public static string GetExecutingAssemblyPath()
    {
      string location = Assembly.GetExecutingAssembly().Location;
      int length1 = location.LastIndexOf("\\");
      string str = location.Substring(0, length1);
      int length2 = str.LastIndexOf("Debug");
      return length2 == -1 ? str : str.Substring(0, length2);
    }

    public static bool? ParseBoolean(object value, bool throwOnError, bool? defaultValue)
    {
      try
      {
        if (value == null)
          return defaultValue;
        if (!(value is string))
          return new bool?(Convert.ToBoolean(value));
        if (string.IsNullOrEmpty((string) value))
          return defaultValue;
        if (((string) value).Trim().ToUpper() == "Y")
          return new bool?(true);
        if (((string) value).Trim().ToUpper() == "N")
          return new bool?(false);
        bool result;
        if (bool.TryParse((string) value, out result))
          return new bool?(result);
      }
      catch
      {
      }
      if (throwOnError)
        throw new FormatException("The value '" + value + "' could not be converted to a boolean.");
      return defaultValue;
    }

    public static int GetTotalTimeSpanDays(DateTime d1, DateTime d2, bool includeBeginDate = false)
    {
      if (d1 == DateTime.MinValue || d2 == DateTime.MinValue)
        return 0;
      int totalDays = (int) d2.Subtract(d1).TotalDays;
      return includeBeginDate && totalDays >= 0 ? totalDays + 1 : totalDays;
    }

    public static DateTime ParseDate(object value) => Utility.ParseDate(value, false);

    public static DateTime ParseDate(object value, DateTime defaultValue)
    {
      return Utility.ParseDate(value, false, defaultValue);
    }

    public static DateTime ParseDate(object value, bool throwOnError)
    {
      return Utility.ParseDate(value, throwOnError, DateTime.MinValue);
    }

    public static DateTime ParseDate(object value, bool throwOnError, DateTime defaultValue)
    {
      try
      {
        if (value is string)
        {
          if (!string.IsNullOrEmpty((string) value))
          {
            if ((string) value != "//")
            {
              DateTime result;
              if (DateTime.TryParse((string) value, Utility.StandardDateFormatProvider, DateTimeStyles.AllowWhiteSpaces, out result))
                return result;
            }
          }
        }
        else if (value != null)
          return Convert.ToDateTime(value);
      }
      catch
      {
        if (throwOnError)
          throw new FormatException("The value '" + value + "' could not be converted to a valid date.");
      }
      return defaultValue;
    }

    public static DateTime? ParseNullableDate(
      object value,
      bool throwOnError,
      DateTime? defaultValue)
    {
      try
      {
        if (value is string)
        {
          if (!string.IsNullOrEmpty((string) value))
          {
            if ((string) value != "//")
            {
              DateTime result;
              if (DateTime.TryParse((string) value, Utility.StandardDateFormatProvider, DateTimeStyles.AllowWhiteSpaces, out result))
                return new DateTime?(result);
            }
          }
        }
        else
          return value != null ? new DateTime?(Convert.ToDateTime(value)) : new DateTime?();
      }
      catch
      {
        if (throwOnError)
          throw new FormatException("The value '" + value + "' could not be converted to a valid date.");
      }
      return defaultValue;
    }

    public static bool IsDate(object value)
    {
      if (value is string && (string.IsNullOrEmpty((string) value) || (string) value == "//"))
        return false;
      try
      {
        Utility.ParseDate(value, true);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static Decimal ParseDecimal(
      object value,
      Decimal defaultValue,
      int roundingDecimalPlaces)
    {
      try
      {
        return Utility.ParseDecimal(value, true, roundingDecimalPlaces);
      }
      catch
      {
        return defaultValue;
      }
    }

    public static Decimal ParseDecimal(object value, Decimal defaultValue)
    {
      return Utility.ParseDecimal(value, false, defaultValue);
    }

    public static Decimal ParseDecimal(object value, bool throwOnError, int roundingDecimalPlaces)
    {
      return Math.Round(Utility.ParseDecimal(value, throwOnError), roundingDecimalPlaces, MidpointRounding.AwayFromZero);
    }

    public static Decimal? ParseNullableDecimal(
      object value,
      bool throwOnError = false,
      Decimal? defaultValue = null)
    {
      try
      {
        if (value is string)
        {
          if (!string.IsNullOrEmpty((string) value))
          {
            Decimal result;
            if (Decimal.TryParse((string) value, NumberStyles.Any, Utility.StandardDateFormatProvider, out result))
              return new Decimal?(result);
          }
        }
        else if (value != null)
          return new Decimal?(Convert.ToDecimal(value));
      }
      catch
      {
        if (throwOnError)
          throw new FormatException("The value '" + value + "' cannot be converted to a numeric value.");
      }
      return defaultValue;
    }

    public static Decimal ParseDecimal(object value, bool throwOnError = false, Decimal defaultValue = 0M)
    {
      try
      {
        if (value is string)
        {
          if (!string.IsNullOrEmpty((string) value))
          {
            Decimal result;
            if (Decimal.TryParse((string) value, NumberStyles.Any, Utility.StandardDateFormatProvider, out result))
              return result;
          }
        }
        else if (value != null)
          return Convert.ToDecimal(value);
      }
      catch
      {
        if (throwOnError)
          throw new FormatException("The value '" + value + "' cannot be converted to a numeric value.");
      }
      return defaultValue;
    }

    public static bool IsDecimal(object value)
    {
      if (value is string && string.IsNullOrEmpty((string) value))
        return false;
      try
      {
        Utility.ParseDecimal(value, true);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static double ParseDouble(object value, double defaultValue)
    {
      return Utility.ParseDouble(value, false, defaultValue);
    }

    public static double ParseDouble(object value, bool throwOnError = false, double defaultValue = 0.0)
    {
      try
      {
        if (value is string)
        {
          if (!string.IsNullOrEmpty((string) value))
          {
            double result;
            if (double.TryParse((string) value, NumberStyles.Any, Utility.StandardDateFormatProvider, out result))
              return result;
          }
        }
        else if (value != null)
          return Convert.ToDouble(value);
      }
      catch
      {
        if (throwOnError)
          throw new FormatException("The value '" + value + "' cannot be converted to a numeric value.");
      }
      return defaultValue;
    }

    public static bool IsDouble(object value)
    {
      if (value is string && string.IsNullOrEmpty((string) value))
        return false;
      try
      {
        Utility.ParseDouble(value, true);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static int ParseInt(object value, int defaultValue)
    {
      return Utility.ParseInt(value, false, defaultValue);
    }

    public static int? ParseNullableInt(object value, bool throwOnError = false, int? defaultValue = null)
    {
      try
      {
        if (value is string)
        {
          if (!string.IsNullOrEmpty((string) value))
          {
            if (Utility.IsIntegerChar(((string) value)[0]))
            {
              int result;
              if (int.TryParse(value.ToString(), NumberStyles.Any, Utility.StandardDateFormatProvider, out result))
                return new int?(result);
            }
          }
        }
        else if (value != null)
          return new int?(Convert.ToInt32(value));
      }
      catch
      {
        if (throwOnError)
          throw new FormatException("The value '" + value + "' cannot be converted to an integer.");
      }
      return defaultValue;
    }

    public static int ParseInt(object value, bool throwOnError = false, int defaultValue = 0)
    {
      try
      {
        if (value is string)
        {
          if (!string.IsNullOrEmpty((string) value))
          {
            if (Utility.IsIntegerChar(((string) value)[0]))
            {
              int result;
              if (int.TryParse(value.ToString(), NumberStyles.Any, Utility.StandardDateFormatProvider, out result))
                return result;
            }
          }
        }
        else if (value != null)
          return Convert.ToInt32(value);
      }
      catch
      {
        if (throwOnError)
          throw new FormatException("The value '" + value + "' cannot be converted to an integer.");
      }
      return defaultValue;
    }

    public static bool IsInt(object value)
    {
      if (value is string && (string.IsNullOrEmpty((string) value) || !Utility.IsIntegerChar(((string) value)[0])))
        return false;
      try
      {
        Utility.ParseInt(value, true);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static byte ParseByte(object value, byte defaultValue)
    {
      return Utility.ParseByte(value, false, defaultValue);
    }

    public static byte ParseByte(object value, bool throwOnError = false, byte defaultValue = 0)
    {
      try
      {
        if (value is string)
        {
          if (!string.IsNullOrEmpty((string) value))
          {
            if (Utility.IsIntegerChar(((string) value)[0]))
            {
              byte result;
              if (byte.TryParse(value.ToString(), NumberStyles.Any, Utility.StandardDateFormatProvider, out result))
                return result;
            }
          }
        }
        else if (value != null)
          return Convert.ToByte(value);
      }
      catch
      {
        if (throwOnError)
          throw new FormatException("The value '" + value + "' cannot be converted to a short.");
      }
      return defaultValue;
    }

    public static bool IsByte(object value)
    {
      if (value is string && (string.IsNullOrEmpty((string) value) || !Utility.IsIntegerChar(((string) value)[0])))
        return false;
      try
      {
        int num = (int) Utility.ParseByte(value, true);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static short ParseShort(object value, short defaultValue)
    {
      return Utility.ParseShort(value, false, defaultValue);
    }

    public static short ParseShort(object value, bool throwOnError = false, short defaultValue = 0)
    {
      try
      {
        if (value is string)
        {
          if (!string.IsNullOrEmpty((string) value))
          {
            if (Utility.IsIntegerChar(((string) value)[0]))
            {
              short result;
              if (short.TryParse(value.ToString(), NumberStyles.Any, Utility.StandardDateFormatProvider, out result))
                return result;
            }
          }
        }
        else if (value != null)
          return Convert.ToInt16(value);
      }
      catch
      {
        if (throwOnError)
          throw new FormatException("The value '" + value + "' cannot be converted to a short.");
      }
      return defaultValue;
    }

    public static short? ParseNullableShort(object value, bool throwOnError = false, short? defaultValue = null)
    {
      try
      {
        if (value is string)
        {
          if (!string.IsNullOrEmpty((string) value))
          {
            if (Utility.IsIntegerChar(((string) value)[0]))
            {
              short result;
              if (short.TryParse(value.ToString(), NumberStyles.Any, Utility.StandardDateFormatProvider, out result))
                return new short?(result);
            }
          }
        }
        else if (value != null)
          return new short?(Convert.ToInt16(value));
      }
      catch
      {
        if (throwOnError)
          throw new FormatException("The value '" + value + "' cannot be converted to a short.");
      }
      return defaultValue;
    }

    public static bool IsShort(object value)
    {
      if (value is string && (string.IsNullOrEmpty((string) value) || !Utility.IsIntegerChar(((string) value)[0])))
        return false;
      try
      {
        int num = (int) Utility.ParseShort(value, true);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static long ParseLong(object value, long defaultValue)
    {
      return Utility.ParseLong(value, false, defaultValue);
    }

    public static long ParseLong(object value, bool throwOnError = false, long defaultValue = 0)
    {
      try
      {
        if (value is string)
        {
          if (!string.IsNullOrEmpty((string) value))
          {
            long result;
            if (long.TryParse(value.ToString(), NumberStyles.Any, Utility.StandardDateFormatProvider, out result))
              return result;
          }
        }
        else if (value != null)
          return Convert.ToInt64(value);
      }
      catch
      {
        if (throwOnError)
          throw new FormatException("The value '" + value + "' cannot be converted to a numeric value.");
      }
      return defaultValue;
    }

    public static string UnformatValue(string value, ValueType type)
    {
      if ((value ?? "") == "")
        return "";
      value = Utility.ConvertToInternalValue(value, type);
      return (type == ValueType.Date || type == ValueType.DateTime) && value == "//" ? "" : value;
    }

    public static bool IsArrayMatch<T>(T[] lhs, T[] rhs)
    {
      bool flag = lhs == rhs;
      if (lhs != null && lhs.GetType().IsArray)
        flag = ((IEnumerable<T>) lhs).SequenceEqual<T>((IEnumerable<T>) rhs);
      return flag;
    }

    public static bool IsListMatch(IList lhs, IList rhs)
    {
      bool flag = lhs == rhs;
      if (lhs != null && (lhs != null ? lhs.Count : 0) == (rhs != null ? rhs.Count : 0))
      {
        for (int index = 0; index < lhs.Count; ++index)
        {
          flag = Utility.IsObjectMatch(lhs[index], rhs[index]);
          if (!flag)
            return flag;
        }
      }
      return flag;
    }

    public static bool IsObjectMatch(object lhs, object rhs)
    {
      bool flag = lhs == rhs;
      if (lhs != null)
      {
        if (rhs == null)
          return false;
        Type type = lhs.GetType();
        if (typeof (IEnumerable<object>).IsAssignableFrom(type))
          return ((IEnumerable<object>) lhs).SequenceEqual<object>((IEnumerable<object>) rhs);
        if (type.IsArray)
          return Utility.IsListMatch((IList) lhs, (IList) rhs);
        if (lhs is Dictionary<string, object>)
        {
          Dictionary<string, object> first = (Dictionary<string, object>) lhs;
          Dictionary<string, object> second = (Dictionary<string, object>) rhs;
          return first.Count == second.Count && !first.Except<KeyValuePair<string, object>>((IEnumerable<KeyValuePair<string, object>>) second).Any<KeyValuePair<string, object>>();
        }
        switch (type.ToString())
        {
          case "System.Decimal":
            flag = Convert.ToDecimal(lhs) == Convert.ToDecimal(rhs);
            break;
          case "System.String":
            flag = Convert.ToString(lhs) == Convert.ToString(rhs);
            break;
          case "System.Int16":
            flag = (int) Convert.ToInt16(lhs) == (int) Convert.ToInt16(rhs);
            break;
          case "System.Int32":
            flag = Convert.ToInt32(lhs) == Convert.ToInt32(rhs);
            break;
          case "System.Boolean":
            flag = Convert.ToBoolean(lhs) == Convert.ToBoolean(rhs);
            break;
          case "System.DateTime":
            flag = Convert.ToDateTime(lhs) == Convert.ToDateTime(rhs);
            break;
          default:
            Tracing.Log(TraceLevel.Info, nameof (Utility), lhs.GetType().ToString());
            break;
        }
      }
      return flag;
    }

    private static bool IsIntegerChar(char c) => char.IsDigit(c) || c == '-';
  }
}

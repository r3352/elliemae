// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CoreFunctions.CoreFunctionsBase
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using Elli.CalculationEngine.Core.DataSource;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.CalculationEngine.Core.CoreFunctions
{
  public class CoreFunctionsBase
  {
    public static Dictionary<string, Decimal> EnumerableDictionary = new Dictionary<string, Decimal>();

    public static bool IsObjectMatch(object lhs, object rhs) => Utility.IsObjectMatch(lhs, rhs);

    public static bool IsArrayMatch<T>(T[] lhs, T[] rhs) => Utility.IsArrayMatch<T>(lhs, rhs);

    public static bool IsBooleanMatch(object lhs, object rhs)
    {
      bool? nullable1 = CoreFunctionsBase.XBoolean(lhs);
      bool? nullable2 = CoreFunctionsBase.XBoolean(rhs);
      return nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue;
    }

    public static bool IsDecimalMatch(object lhs, object rhs)
    {
      return CoreFunctionsBase.XDec(lhs) == CoreFunctionsBase.XDec(rhs);
    }

    public static bool IsStringMatch(object lhs, object rhs)
    {
      return CoreFunctionsBase.XString(lhs) == CoreFunctionsBase.XString(rhs);
    }

    public static bool IsDateMatch(object lhs, object rhs)
    {
      return CoreFunctionsBase.XDate(lhs) == CoreFunctionsBase.XDate(rhs);
    }

    public static bool IsLongMatch(object lhs, object rhs)
    {
      return CoreFunctionsBase.XLong(lhs) == CoreFunctionsBase.XLong(rhs);
    }

    public static bool IsShortMatch(object lhs, object rhs)
    {
      return (int) CoreFunctionsBase.XShort(lhs) == (int) CoreFunctionsBase.XShort(rhs);
    }

    public static bool IsDate(object value) => Utility.IsDate(value);

    public static bool IsDecimal(object value) => Utility.IsDecimal(value);

    public static bool IsEmpty(object x) => string.Concat(x) == "";

    public static bool IsInteger(object value) => Utility.IsInt(value);

    public static object IfEmpty(object x, object emptyVal)
    {
      return CoreFunctionsBase.IsEmpty(x) ? emptyVal : x;
    }

    public static bool? XBoolean(object x) => CoreFunctionsBase.XBoolean(x, new bool?());

    public static bool? XBoolean(object x, bool? defaultValue)
    {
      return Utility.ParseBoolean(x, true, defaultValue);
    }

    public static Decimal Dec(object x) => CoreFunctionsBase.XDec(x);

    public static Decimal XDec(object x) => CoreFunctionsBase.XDec(x, 0M);

    public static Decimal XDec(object x, Decimal defaultValue)
    {
      try
      {
        return Utility.ParseDecimal(x, true);
      }
      catch
      {
        return defaultValue;
      }
    }

    public static int CastToInt(Decimal x) => CoreFunctionsBase.CastToInt(x, 0);

    public static int CastToInt(Decimal x, int defaultValue)
    {
      try
      {
        return (int) x;
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        return defaultValue;
      }
    }

    public static int CastToIntFromDouble(double x) => CoreFunctionsBase.CastToIntFromDouble(x, 0);

    public static int CastToIntFromDouble(double x, int defaultValue)
    {
      try
      {
        return (int) x;
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        return defaultValue;
      }
    }

    public static int XInt(object x) => CoreFunctionsBase.XInt(x, 0);

    public static int XInt(object x, int defaultValue)
    {
      try
      {
        return Utility.ParseInt(x, true);
      }
      catch
      {
        return defaultValue;
      }
    }

    public static long CastToLong(Decimal x) => CoreFunctionsBase.CastToLong(x, 0L);

    public static long CastToLong(Decimal x, long defaultValue)
    {
      try
      {
        return (long) x;
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        return defaultValue;
      }
    }

    public static long XLong(object x) => CoreFunctionsBase.XLong(x, 0);

    public static long XLong(object x, int defaultValue)
    {
      try
      {
        return Utility.ParseLong(x, true);
      }
      catch
      {
        return (long) defaultValue;
      }
    }

    public static byte XByte(object x) => CoreFunctionsBase.XByte(x, (byte) 0);

    public static byte XByte(object x, byte defaultValue)
    {
      try
      {
        return Utility.ParseByte(x, true);
      }
      catch
      {
        return defaultValue;
      }
    }

    public static short XShort(object x) => CoreFunctionsBase.XShort(x, (short) 0);

    public static short XShort(object x, short defaultValue)
    {
      try
      {
        return Utility.ParseShort(x, true);
      }
      catch
      {
        return defaultValue;
      }
    }

    public static DateTime XDate(object x) => CoreFunctionsBase.XDate(x, DateTime.MinValue);

    public static DateTime XDate(object x, DateTime defaultValue)
    {
      try
      {
        return Utility.ParseDate(x, true);
      }
      catch
      {
        return defaultValue;
      }
    }

    public static DateTime XDateToTimeZone(object x, string timeZone)
    {
      return CoreFunctionsBase.XDateToTimeZone(x, timeZone, DateTime.MinValue);
    }

    public static DateTime XDateToTimeZone(object x, string timeZone, DateTime defaultValue)
    {
      try
      {
        return TimeZoneInfo.ConvertTimeFromUtc(Utility.ParseDate(x, true), TimeZoneInfo.FindSystemTimeZoneById(timeZone));
      }
      catch
      {
        return defaultValue;
      }
    }

    public static object Sum(params object[] args)
    {
      if (args.Length == 0)
        return (object) 0;
      double num = 0.0;
      for (int index = 0; index < args.Length; ++index)
      {
        try
        {
          num += Convert.ToDouble(args[index]);
        }
        catch
        {
          return (object) "";
        }
      }
      return (object) num;
    }

    public static object SumAny(params object[] args)
    {
      if (args.Length == 0)
        return (object) 0;
      double num = 0.0;
      bool flag = false;
      for (int index = 0; index < args.Length; ++index)
      {
        try
        {
          num += Convert.ToDouble(args[index]);
          flag = true;
        }
        catch
        {
        }
      }
      return !flag ? (object) "" : (object) num;
    }

    public static object Diff(object x, object y)
    {
      try
      {
        return (object) (Convert.ToDouble(x) - Convert.ToDouble(y));
      }
      catch
      {
        return (object) "";
      }
    }

    public static object Mult(params object[] args)
    {
      if (args.Length == 0)
        return (object) "";
      double num = 1.0;
      for (int index = 0; index < args.Length; ++index)
      {
        try
        {
          num *= Convert.ToDouble(args[index]);
        }
        catch
        {
          return (object) "";
        }
      }
      return (object) num;
    }

    public static object MultAny(params object[] args)
    {
      if (args.Length == 0)
        return (object) "";
      double num = 1.0;
      bool flag = false;
      for (int index = 0; index < args.Length; ++index)
      {
        try
        {
          num *= Convert.ToDouble(args[index]);
          flag = true;
        }
        catch
        {
        }
      }
      return !flag ? (object) "" : (object) num;
    }

    public static object Div(object x, object y)
    {
      try
      {
        return (object) (Convert.ToDouble(x) / Convert.ToDouble(y));
      }
      catch
      {
        return (object) "";
      }
    }

    public static int IntDiv(int dividend, int divisor) => dividend / divisor;

    public static object Abs(object a)
    {
      try
      {
        return (object) Math.Abs(Convert.ToDecimal(a));
      }
      catch
      {
        return (object) "";
      }
    }

    public static object Min(object a, object b)
    {
      try
      {
        return (object) Math.Min(Convert.ToDecimal(a), Convert.ToDecimal(b));
      }
      catch
      {
      }
      try
      {
        return (object) Convert.ToDecimal(a);
      }
      catch
      {
      }
      try
      {
        return (object) Convert.ToDecimal(b);
      }
      catch
      {
      }
      return (object) "";
    }

    public static object Min(params object[] args)
    {
      if (args.Length == 0)
        return (object) "";
      object b = CoreFunctionsBase.Min(args[0], (object) "");
      for (int index = 1; index < args.Length; ++index)
        b = CoreFunctionsBase.Min(args[index], b);
      return b;
    }

    public static object Max(object a, object b)
    {
      try
      {
        return (object) Math.Max(Convert.ToDecimal(a), Convert.ToDecimal(b));
      }
      catch
      {
      }
      try
      {
        return (object) Convert.ToDecimal(a);
      }
      catch
      {
      }
      try
      {
        return (object) Convert.ToDecimal(b);
      }
      catch
      {
      }
      return (object) "";
    }

    public static object Max(params object[] args)
    {
      if (args.Length == 0)
        return (object) "";
      object b = CoreFunctionsBase.Max(args[0], (object) "");
      for (int index = 1; index < args.Length; ++index)
        b = CoreFunctionsBase.Max(args[index], b);
      return b;
    }

    public static object Median(params object[] args)
    {
      if (args.Length == 0)
        return (object) "";
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < args.Length; ++index)
      {
        try
        {
          arrayList.Add((object) Convert.ToDecimal(args[index]));
        }
        catch
        {
        }
      }
      if (arrayList.Count == 0)
        return (object) "";
      arrayList.Sort();
      return arrayList.Count % 2 == 1 ? arrayList[arrayList.Count / 2] : (object) ((Convert.ToDecimal(arrayList[arrayList.Count / 2 - 1]) + Convert.ToDecimal(arrayList[arrayList.Count / 2])) / 2M);
    }

    public static object LMedian(params object[] args)
    {
      if (args.Length == 0)
        return (object) "";
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < args.Length; ++index)
      {
        try
        {
          arrayList.Add((object) Convert.ToDecimal(args[index]));
        }
        catch
        {
        }
      }
      if (arrayList.Count == 0)
        return (object) "";
      arrayList.Sort();
      return arrayList.Count % 2 == 1 ? arrayList[arrayList.Count / 2] : arrayList[arrayList.Count / 2 - 1];
    }

    public static object UMedian(params object[] args)
    {
      if (args.Length == 0)
        return (object) "";
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < args.Length; ++index)
      {
        try
        {
          arrayList.Add((object) Convert.ToDecimal(args[index]));
        }
        catch
        {
        }
      }
      if (arrayList.Count == 0)
        return (object) "";
      arrayList.Sort();
      return arrayList[arrayList.Count / 2];
    }

    public static object Sqrt(object a)
    {
      try
      {
        return (object) Math.Sqrt(Convert.ToDouble(a));
      }
      catch
      {
        return (object) "";
      }
    }

    public static object Log(object a)
    {
      try
      {
        return (object) Math.Log(Convert.ToDouble(a));
      }
      catch
      {
        return (object) "";
      }
    }

    public static object Log10(object a)
    {
      try
      {
        return (object) Math.Log10(Convert.ToDouble(a));
      }
      catch
      {
        return (object) "";
      }
    }

    public static object Exp(object a)
    {
      try
      {
        return (object) Math.Exp(Convert.ToDouble(a));
      }
      catch
      {
        return (object) "";
      }
    }

    public static object Sgn(object a)
    {
      try
      {
        return (object) Math.Sign(Convert.ToDouble(a));
      }
      catch
      {
        return (object) "";
      }
    }

    public static object ArithmeticRoundingUp(int val, int baseFraction)
    {
      int num = val;
      try
      {
        num = val / baseFraction + Utility.ParseInt((object) (val % baseFraction > 0));
      }
      catch
      {
      }
      return (object) num;
    }

    public static object ArithmeticRound(object a, object precision)
    {
      try
      {
        return (object) Math.Round(Convert.ToDecimal(Convert.ToDouble(a)), Convert.ToInt32(precision), MidpointRounding.AwayFromZero);
      }
      catch
      {
        return (object) "";
      }
    }

    public static object Round(object a, object precision)
    {
      try
      {
        return (object) Math.Round(Convert.ToDecimal(Convert.ToDouble(a)), Convert.ToInt32(precision), MidpointRounding.ToEven);
      }
      catch
      {
        return (object) "";
      }
    }

    public static object RoundWithDouble(double a, object precision)
    {
      try
      {
        return (object) Math.Round(a, Convert.ToInt32(precision), MidpointRounding.ToEven);
      }
      catch
      {
        return (object) "";
      }
    }

    public static object Truncate(object val, object precision)
    {
      try
      {
        int int32 = Convert.ToInt32(precision);
        return (object) (Math.Truncate((double) ((Decimal) val * (Decimal) Math.Pow(10.0, (double) int32))) / Math.Pow(10.0, (double) int32));
      }
      catch
      {
        return (object) "";
      }
    }

    public static object TruncateToCents(object val)
    {
      try
      {
        return (object) (Math.Truncate(100M * Convert.ToDecimal(val)) / 100M);
      }
      catch
      {
        return (object) "";
      }
    }

    public static object Pow(object a, object power)
    {
      try
      {
        return (object) Math.Pow(Convert.ToDouble(a), Convert.ToDouble(power));
      }
      catch
      {
        return (object) "";
      }
    }

    public static object Ceiling(object a)
    {
      try
      {
        return (object) Math.Ceiling(Convert.ToDecimal(a));
      }
      catch
      {
        return (object) "";
      }
    }

    public static object XDateAdd(string interval, object numberObj, object dateObj)
    {
      try
      {
        DateTime date = Utility.ParseDate(dateObj, true);
        double Number = Utility.ParseDouble(numberObj, true);
        return (object) DateAndTime.DateAdd(interval, Number, (object) date);
      }
      catch
      {
        return (object) "";
      }
    }

    public static object XDateDiff(string interval, object date1Obj, object date2Obj)
    {
      try
      {
        DateTime date1 = Utility.ParseDate(date1Obj, true);
        DateTime date2 = Utility.ParseDate(date2Obj, true);
        return (object) DateAndTime.DateDiff(interval, (object) date1, (object) date2);
      }
      catch
      {
        return (object) "";
      }
    }

    public static object XTotalTimeSpanDays(
      object date1Obj,
      object date2Obj,
      bool includeBeginDate = false)
    {
      try
      {
        return (object) Utility.GetTotalTimeSpanDays(Utility.ParseDate(date1Obj, true), Utility.ParseDate(date2Obj, true), includeBeginDate);
      }
      catch
      {
        return (object) "";
      }
    }

    public static object XDaysInMonth(object dateObj)
    {
      try
      {
        DateTime date = Utility.ParseDate(dateObj, true);
        return (object) DateTime.DaysInMonth(date.Year, date.Month);
      }
      catch
      {
        return (object) "";
      }
    }

    public static string XDateString(object x)
    {
      string str = x.ToString();
      try
      {
        if (str.Contains("/"))
        {
          DateTime date = Utility.ParseDate(x, true);
          if (date != DateTime.MinValue)
            str = date.ToString("MM/dd/yyyy");
        }
        return str;
      }
      catch
      {
        return str;
      }
    }

    public static string TransformSettingTimezoneToStandardTimezone(
      string timezoneSetting,
      bool isDaylightSavingTime)
    {
      string standardTimezone;
      switch (timezoneSetting)
      {
        case "(UTC -05:00) Eastern Time":
          standardTimezone = !isDaylightSavingTime ? "EST" : "EDT";
          break;
        case "(UTC -06:00) Central Time":
          standardTimezone = !isDaylightSavingTime ? "CST" : "CDT";
          break;
        case "(UTC -07:00) Arizona Time":
          standardTimezone = !isDaylightSavingTime ? "MST" : "PDT";
          break;
        case "(UTC -07:00) Mountain Time":
          standardTimezone = !isDaylightSavingTime ? "MST" : "MDT";
          break;
        case "(UTC -08:00) Pacific Time":
          standardTimezone = !isDaylightSavingTime ? "PST" : "PDT";
          break;
        case "(UTC -09:00) Alaska Time":
          standardTimezone = !isDaylightSavingTime ? "AKST" : "AKDT";
          break;
        case "(UTC -10:00) Hawaii Time":
          standardTimezone = "HST";
          break;
        default:
          standardTimezone = !isDaylightSavingTime ? "EST" : "EDT";
          break;
      }
      return standardTimezone;
    }

    public static bool IsDaylightSavingTime(object dateObj)
    {
      try
      {
        return TimeZoneInfo.Local.IsDaylightSavingTime(Utility.ParseDate(dateObj, true));
      }
      catch
      {
        return false;
      }
    }

    public static DateTime TruncateDate(DateTime dateTime)
    {
      return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Unspecified);
    }

    public static object Range(object value, params object[] rangeItems)
    {
      try
      {
        for (int index = 0; index < rangeItems.Length; ++index)
        {
          if (!Utility.IsDecimal(value) ? (!Utility.IsDate(value) ? string.Compare(string.Concat(value), string.Concat(rangeItems[index]), true) < 0 : Utility.ParseDate(value, true) < Utility.ParseDate(rangeItems[index], true)) : Utility.ParseDecimal(value, true) < Utility.ParseDecimal(rangeItems[index], true))
            return (object) index;
        }
        return (object) rangeItems.Length;
      }
      catch
      {
      }
      return (object) "";
    }

    public static object RangeLow(object value, params object[] rangeItems)
    {
      try
      {
        for (int index = 0; index < rangeItems.Length; ++index)
        {
          if (!Utility.IsDecimal(value) ? (!Utility.IsDate(value) ? string.Compare(string.Concat(value), string.Concat(rangeItems[index]), true) <= 0 : Utility.ParseDate(value, true) <= Utility.ParseDate(rangeItems[index], true)) : Utility.ParseDecimal(value, true) <= Utility.ParseDecimal(rangeItems[index], true))
            return (object) index;
        }
        return (object) rangeItems.Length;
      }
      catch
      {
      }
      return (object) "";
    }

    public static object Match(object value, params object[] values)
    {
      try
      {
        for (int index = 0; index < values.Length; ++index)
        {
          if (value.Equals(values[index]))
            return (object) index;
        }
      }
      catch
      {
      }
      return (object) -1;
    }

    public static object Pick(int index, params object[] values)
    {
      return index < 0 || index > values.Length - 1 ? (object) null : values[index];
    }

    public static int Count(params object[] items)
    {
      int num = 0;
      for (int index = 0; index < items.Length; ++index)
      {
        if (!CoreFunctionsBase.IsEmpty(items[index]))
          ++num;
      }
      return num;
    }

    public static Decimal SumEnumerable(IEnumerable<object> items)
    {
      if (!items.Any<object>())
        return 0M;
      Decimal num = 0M;
      foreach (object obj in items)
        num += Convert.ToDecimal(obj);
      return num;
    }

    public static Decimal SumAnyEnumerable(string key, IEnumerable<object> items)
    {
      Decimal num = 0M;
      if (!CoreFunctionsBase.EnumerableDictionary.TryGetValue(key, out num))
      {
        num = CoreFunctionsBase.SumAnyEnumerable(items);
        CoreFunctionsBase.EnumerableDictionary.Add(key, num);
      }
      return num;
    }

    public static Decimal SumAnyEnumerable(IEnumerable<object> items)
    {
      if (items == null)
        return 0M;
      Decimal num = 0M;
      foreach (object obj in items)
      {
        if (obj != null)
        {
          num = !(obj is string) ? items.Sum<object>((Func<object, Decimal>) (x => x == null ? 0M : (Decimal) Convert.ChangeType(x, typeof (Decimal)))) : items.Cast<string>().Sum<string>((Func<string, Decimal>) (e => e == null || string.IsNullOrEmpty(e) || !char.IsNumber(e.First<char>()) ? 0M : Convert.ToDecimal(e)));
          break;
        }
      }
      return num;
    }

    public static Decimal SumAnyEnumerable(IEnumerable<Decimal?> items)
    {
      Decimal num = 0M;
      if (items != null)
        num = items.Sum<Decimal?>((Func<Decimal?, Decimal>) (x => !x.HasValue ? 0M : (Decimal) Convert.ChangeType((object) x, typeof (Decimal))));
      return num;
    }

    public static Decimal SumAnyEnumerable(string key, IEnumerable<Decimal?> items)
    {
      Decimal num = 0M;
      if (!CoreFunctionsBase.EnumerableDictionary.TryGetValue(key, out num))
      {
        if (items != null)
          num = items.Sum<Decimal?>((Func<Decimal?, Decimal>) (x => !x.HasValue ? 0M : (Decimal) Convert.ChangeType((object) x, typeof (Decimal))));
        CoreFunctionsBase.EnumerableDictionary.Add(key, num);
      }
      return num;
    }

    public static Decimal SumAnyEnumerable(IEnumerable<Decimal> items)
    {
      Decimal num = 0M;
      if (items != null)
        num = items.Sum();
      return num;
    }

    public static Decimal SumAnyEnumerable(string key, IEnumerable<Decimal> items)
    {
      Decimal num = 0M;
      if (!CoreFunctionsBase.EnumerableDictionary.TryGetValue(key, out num))
      {
        if (items == null)
          return 0M;
        using (IEnumerator<Decimal> enumerator = items.GetEnumerator())
        {
          if (enumerator.MoveNext())
          {
            Decimal current = enumerator.Current;
            num = items.Sum<Decimal>((Func<Decimal, Decimal>) (x => (Decimal) Convert.ChangeType((object) x, typeof (Decimal))));
          }
        }
        CoreFunctionsBase.EnumerableDictionary.Add(key, num);
      }
      return num;
    }

    public static Decimal SumAnyEnumerable(IEnumerable<short?> items)
    {
      Decimal num = 0M;
      if (items == null)
        return 0M;
      foreach (short? nullable in items)
      {
        if (nullable.HasValue)
        {
          num = (Decimal) items.Sum<short?>((Func<short?, int>) (x => !x.HasValue ? 0 : (int) (short) Convert.ChangeType((object) x, typeof (short))));
          break;
        }
      }
      return num;
    }

    public static Decimal SumAnyEnumerable(string key, IEnumerable<short?> items)
    {
      Decimal num = 0M;
      if (!CoreFunctionsBase.EnumerableDictionary.TryGetValue(key, out num))
      {
        if (items != null)
          num = (Decimal) items.Sum<short?>((Func<short?, int>) (x => !x.HasValue ? 0 : (int) (short) Convert.ChangeType((object) x, typeof (short))));
        CoreFunctionsBase.EnumerableDictionary.Add(key, num);
      }
      return num;
    }

    public static Decimal SumAnyEnumerable(IEnumerable<short> items)
    {
      Decimal num = 0M;
      if (items != null)
        num = (Decimal) items.Sum<short>((Func<short, int>) (x => (int) (short) Convert.ChangeType((object) x, typeof (short))));
      return num;
    }

    public static Decimal SumAnyEnumerable(string key, IEnumerable<short> items)
    {
      Decimal num = 0M;
      if (!CoreFunctionsBase.EnumerableDictionary.TryGetValue(key, out num))
      {
        if (items != null)
          num = (Decimal) items.Sum<short>((Func<short, int>) (x => (int) (short) Convert.ChangeType((object) x, typeof (short))));
        CoreFunctionsBase.EnumerableDictionary.Add(key, num);
      }
      return num;
    }

    public static Decimal SumAnyEnumerable(IEnumerable<int?> items)
    {
      Decimal num = 0M;
      if (items != null)
        num = (Decimal) items.Sum<int?>((Func<int?, int>) (x => !x.HasValue ? 0 : (int) Convert.ChangeType((object) x, typeof (int))));
      return num;
    }

    public static Decimal SumAnyEnumerable(string key, IEnumerable<int?> items)
    {
      Decimal num = 0M;
      if (!CoreFunctionsBase.EnumerableDictionary.TryGetValue(key, out num))
      {
        if (items != null)
          num = (Decimal) items.Sum<int?>((Func<int?, int>) (x => !x.HasValue ? 0 : (int) Convert.ChangeType((object) x, typeof (int))));
        CoreFunctionsBase.EnumerableDictionary.Add(key, num);
      }
      return num;
    }

    public static Decimal SumAnyEnumerable(IEnumerable<int> items)
    {
      Decimal num = 0M;
      if (items != null)
        num = (Decimal) items.Sum();
      return num;
    }

    public static Decimal SumAnyEnumerable(string key, IEnumerable<int> items)
    {
      Decimal num = 0M;
      if (!CoreFunctionsBase.EnumerableDictionary.TryGetValue(key, out num))
      {
        if (items != null)
          num = (Decimal) items.Sum();
        CoreFunctionsBase.EnumerableDictionary.Add(key, num);
      }
      return num;
    }

    public static Decimal SumAnyEnumerable(IEnumerable<string> items)
    {
      Decimal num = 0M;
      if (items != null)
        num = items.Sum<string>((Func<string, Decimal>) (e => e == null || string.IsNullOrEmpty(e) || !char.IsNumber(e.First<char>()) ? 0M : Convert.ToDecimal(e)));
      return num;
    }

    public static Decimal SumAnyEnumerable(string key, IEnumerable<string> items)
    {
      Decimal num = 0M;
      if (!CoreFunctionsBase.EnumerableDictionary.TryGetValue(key, out num))
      {
        if (items != null)
          num = items.Sum<string>((Func<string, Decimal>) (e => e == null || string.IsNullOrEmpty(e) || !char.IsNumber(e.First<char>()) ? 0M : Convert.ToDecimal(e)));
        CoreFunctionsBase.EnumerableDictionary.Add(key, num);
      }
      return num;
    }

    public static object MultEnumerable(IEnumerable<object> items)
    {
      if (items == null || !items.Any<object>())
        return (object) "";
      Decimal num = 1M;
      foreach (object x in items)
      {
        try
        {
          num *= CoreFunctionsBase.XDec(x);
        }
        catch
        {
          return (object) "";
        }
      }
      return (object) num;
    }

    public static object MultAnyEnumerable(IEnumerable<object> items)
    {
      if (items == null)
        return (object) "";
      double num = 1.0;
      bool flag = false;
      foreach (object obj in items)
      {
        try
        {
          num *= Convert.ToDouble(obj);
          flag = true;
        }
        catch
        {
        }
      }
      return !flag ? (object) "" : (object) num;
    }

    public static object MinEnumerable(IEnumerable<object> items)
    {
      if (items == null || !items.Any<object>())
        return (object) "";
      object b = CoreFunctionsBase.Min(items.FirstOrDefault<object>(), (object) "");
      foreach (object a in items)
        b = CoreFunctionsBase.Min(a, b);
      return b;
    }

    public static object MaxEnumerable(IEnumerable<object> items)
    {
      if (items == null || !items.Any<object>())
        return (object) "";
      object b = CoreFunctionsBase.Max(items.FirstOrDefault<object>(), (object) "");
      foreach (object a in items)
        b = CoreFunctionsBase.Max(a, b);
      return b;
    }

    public static object MaxEnumerable(IEnumerable<Decimal?> items)
    {
      if (items == null || !items.Any<Decimal?>())
        return (object) "";
      object b = CoreFunctionsBase.Max((object) items.FirstOrDefault<Decimal?>(), (object) "");
      foreach (Decimal? a in items)
        b = CoreFunctionsBase.Max((object) a, b);
      return b;
    }

    public static object MaxEnumerable(IEnumerable<Decimal> items)
    {
      return CoreFunctionsBase.MaxEnumerable(items.Cast<Decimal?>());
    }

    public static object MedianEnumerable(IEnumerable<object> items)
    {
      if (items == null || !items.Any<object>())
        return (object) "";
      ArrayList arrayList = new ArrayList();
      foreach (object obj in items)
      {
        try
        {
          arrayList.Add((object) Convert.ToDecimal(obj));
        }
        catch
        {
        }
      }
      if (arrayList.Count == 0)
        return (object) "";
      arrayList.Sort();
      return arrayList.Count % 2 == 1 ? arrayList[arrayList.Count / 2] : (object) ((Convert.ToDecimal(arrayList[arrayList.Count / 2 - 1]) + Convert.ToDecimal(arrayList[arrayList.Count / 2])) / 2M);
    }

    public static object LMedianEnumerable(IEnumerable<object> items)
    {
      if (items == null || !items.Any<object>())
        return (object) "";
      ArrayList arrayList = new ArrayList();
      foreach (object obj in items)
      {
        try
        {
          arrayList.Add((object) Convert.ToDecimal(obj));
        }
        catch
        {
        }
      }
      if (arrayList.Count == 0)
        return (object) "";
      arrayList.Sort();
      return arrayList.Count % 2 == 1 ? arrayList[arrayList.Count / 2] : arrayList[arrayList.Count / 2 - 1];
    }

    public static object UMedianEnumerable(IEnumerable<object> items)
    {
      if (items == null || !items.Any<object>())
        return (object) "";
      ArrayList arrayList = new ArrayList();
      foreach (object obj in items)
      {
        try
        {
          arrayList.Add((object) Convert.ToDecimal(obj));
        }
        catch
        {
        }
      }
      if (arrayList.Count == 0)
        return (object) "";
      arrayList.Sort();
      return arrayList[arrayList.Count / 2];
    }

    public static string ApplyFieldFormatting(string value, string format)
    {
      return CoreFunctionsBase.ApplyFieldFormatting(value, format, true);
    }

    public static string ApplyFieldFormatting(string value, string format, bool formatEmptyValues)
    {
      return Utility.ApplyFieldFormatting(value, format, formatEmptyValues);
    }

    public static string FormatGuid(object guid, string format = "")
    {
      try
      {
        guid.ToString();
        return ((Guid) guid).ToString(format);
      }
      catch
      {
        return "";
      }
    }

    public static string FormatDate(object date, string format)
    {
      try
      {
        return Utility.ParseDate(date, true).ToString(format);
      }
      catch
      {
        return "";
      }
    }

    public static string FormatInt(object value, string format)
    {
      try
      {
        return Utility.ParseInt(value, true).ToString(format);
      }
      catch
      {
        return "";
      }
    }

    public static string FormatDec(object value, string format)
    {
      try
      {
        return Utility.ParseDecimal(value, true).ToString(format);
      }
      catch
      {
        return "";
      }
    }

    public static string XString(object value)
    {
      try
      {
        return value == null ? "" : value.ToString();
      }
      catch
      {
        return "";
      }
    }

    public static string Int2Text(object value)
    {
      try
      {
        return NumberToTextConverter.ToString(Utility.ParseLong(value, true));
      }
      catch
      {
        return "";
      }
    }

    public static string Dec2Text(object value) => CoreFunctionsBase.Dec2Text(value, false);

    public static string Dec2Text(object value, bool dollarsAndCents)
    {
      try
      {
        Decimal num = Utility.ParseDecimal(value, true);
        NumberToTextOption numberToTextOption = NumberToTextOption.TwoDecimalPlaces;
        if (dollarsAndCents)
          numberToTextOption |= NumberToTextOption.DollarsAndCents;
        int options = (int) numberToTextOption;
        return NumberToTextConverter.ToString(num, (NumberToTextOption) options);
      }
      catch
      {
        return "";
      }
    }

    public static string Money2Text(object value) => CoreFunctionsBase.Dec2Text(value, true);

    public static bool IsNull(DataEntityWrapper entity) => entity == null || entity.IsNull();
  }
}

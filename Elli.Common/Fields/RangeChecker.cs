// Decompiled with JetBrains decompiler
// Type: Elli.Common.Fields.RangeChecker
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using Elli.Common.Diagnostics;
using System;

#nullable disable
namespace Elli.Common.Fields
{
  public static class RangeChecker
  {
    public static bool IsInRange(object value, object minValue, object maxValue)
    {
      if (value == null)
        return false;
      try
      {
        Type type = value.GetType();
        if (type == typeof (byte) || type == typeof (int) || type == typeof (long) || type == typeof (short) || type == typeof (Decimal) || type == typeof (double) || type == typeof (float))
        {
          Decimal num1 = (Decimal) Convert.ChangeType(value, typeof (Decimal));
          Decimal num2 = !RangeChecker.IsNullorEmpty(minValue) ? (Decimal) Convert.ChangeType(minValue, typeof (Decimal)) : Decimal.MinValue;
          Decimal num3 = !RangeChecker.IsNullorEmpty(maxValue) ? (Decimal) Convert.ChangeType(maxValue, typeof (Decimal)) : Decimal.MaxValue;
          Decimal num4 = num1;
          return num2 <= num4 && num1 <= num3;
        }
        if (type == typeof (DateTime))
        {
          DateTime dateTime1 = (DateTime) Convert.ChangeType(value, typeof (DateTime));
          DateTime dateTime2 = !RangeChecker.IsNullorEmpty(minValue) ? (DateTime) Convert.ChangeType(minValue, typeof (DateTime)) : DateTime.MinValue;
          DateTime dateTime3 = !RangeChecker.IsNullorEmpty(maxValue) ? (DateTime) Convert.ChangeType(maxValue, typeof (DateTime)) : DateTime.MaxValue;
          DateTime dateTime4 = dateTime1;
          return dateTime2 <= dateTime4 && dateTime1 <= dateTime3;
        }
        if (type == typeof (string))
        {
          Decimal result1;
          if (Decimal.TryParse(value.ToString(), out result1))
          {
            Decimal num5 = !RangeChecker.IsNullorEmpty(minValue) ? (Decimal) Convert.ChangeType(minValue, typeof (Decimal)) : Decimal.MinValue;
            Decimal num6 = !RangeChecker.IsNullorEmpty(maxValue) ? (Decimal) Convert.ChangeType(maxValue, typeof (Decimal)) : Decimal.MaxValue;
            Decimal num7 = result1;
            return num5 <= num7 && result1 <= num6;
          }
          DateTime result2;
          if (DateTime.TryParse(value.ToString(), out result2))
          {
            DateTime dateTime5 = !RangeChecker.IsNullorEmpty(minValue) ? (DateTime) Convert.ChangeType(minValue, typeof (DateTime)) : DateTime.MinValue;
            DateTime dateTime6 = !RangeChecker.IsNullorEmpty(maxValue) ? (DateTime) Convert.ChangeType(maxValue, typeof (DateTime)) : DateTime.MaxValue;
            DateTime dateTime7 = result2;
            return dateTime5 <= dateTime7 && result2 <= dateTime6;
          }
        }
        Logger.LogDebug(string.Format("Value '{0}' of type '{1}' cannot be checked for range between '{2}' and '{3}'.", value, (object) value.GetType(), minValue, maxValue));
        return false;
      }
      catch (Exception ex)
      {
        throw new ArgumentException(string.Format("Value '{0}' of type '{1}' cannot be checked for range between '{2}' and '{3}'.", value, (object) value.GetType(), minValue, maxValue), ex);
      }
    }

    public static bool IsNullorEmpty(object value)
    {
      return object.Equals(value, (object) null) || object.Equals(value, (object) "");
    }
  }
}

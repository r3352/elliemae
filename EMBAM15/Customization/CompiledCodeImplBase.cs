// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Customization.CompiledCodeImplBase
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using Microsoft.VisualBasic;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Customization
{
  public abstract class CompiledCodeImplBase : MarshalByRefObject
  {
    protected bool IsEmpty(object x) => string.Concat(x) == "";

    protected object IfEmpty(object x, object emptyVal) => this.IsEmpty(x) ? emptyVal : x;

    protected Decimal Dec(object x) => this.XDec(x);

    protected Decimal XDec(object x) => this.XDec(x, 0M);

    protected Decimal XDec(object x, Decimal defaultValue)
    {
      try
      {
        return Utils.ParseDecimal(x, true);
      }
      catch
      {
        return defaultValue;
      }
    }

    protected int XInt(object x) => this.XInt(x, 0);

    protected int XInt(object x, int defaultValue)
    {
      try
      {
        return Utils.ParseInt(x, true);
      }
      catch
      {
        return defaultValue;
      }
    }

    protected DateTime XDate(object x) => this.XDate(x, DateTime.MinValue);

    protected DateTime XDate(object x, DateTime defaultValue)
    {
      try
      {
        return Utils.ParseDate(x, true);
      }
      catch
      {
        return defaultValue;
      }
    }

    protected DateTime XMonthDay(object x, DateTime defaultValue)
    {
      try
      {
        return Utils.ParseMonthDay(x, true);
      }
      catch
      {
        return defaultValue;
      }
    }

    protected DateTime XMonthDay(object x) => this.XMonthDay(x, DateTime.MinValue);

    protected object Sum(params object[] args)
    {
      if (args.Length == 0)
        return (object) "";
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

    protected object SumAny(params object[] args)
    {
      if (args.Length == 0)
        return (object) "";
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

    protected object Diff(object x, object y)
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

    protected object Mult(params object[] args)
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

    protected object MultAny(params object[] args)
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

    protected object Div(object x, object y)
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

    protected object Abs(object a)
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

    protected object Min(object a, object b)
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

    protected object Min(params object[] args)
    {
      if (args.Length == 0)
        return (object) "";
      object b = this.Min(args[0], (object) "");
      for (int index = 1; index < args.Length; ++index)
        b = this.Min(args[index], b);
      return b;
    }

    protected object Max(object a, object b)
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

    protected object Max(params object[] args)
    {
      if (args.Length == 0)
        return (object) "";
      object b = this.Max(args[0], (object) "");
      for (int index = 1; index < args.Length; ++index)
        b = this.Max(args[index], b);
      return b;
    }

    protected object Median(params object[] args)
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

    protected object LMedian(params object[] args)
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

    protected object UMedian(params object[] args)
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

    protected object Sqrt(object a)
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

    protected object Log(object a)
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

    protected object Log10(object a)
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

    protected object Exp(object a)
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

    protected object Sgn(object a)
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

    protected object Round(object a, object precision)
    {
      try
      {
        return (object) Math.Round(Convert.ToDecimal(a), Convert.ToInt32(precision));
      }
      catch
      {
        return (object) "";
      }
    }

    protected object Trunc(object a, object precision)
    {
      try
      {
        Decimal d = Convert.ToDecimal(a);
        int int32 = Convert.ToInt32(precision);
        for (int index = 0; index < int32; ++index)
          d *= 10M;
        Decimal num = Decimal.Truncate(d);
        for (int index = 0; index < int32; ++index)
          num /= 10M;
        return (object) num;
      }
      catch
      {
        return (object) "";
      }
    }

    protected object Pow(object a, object power)
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

    public object XDateAdd(string interval, object numberObj, object dateObj)
    {
      try
      {
        DateTime date = Utils.ParseDate(dateObj, true);
        double Number = Utils.ParseDouble(numberObj, true);
        return (object) DateAndTime.DateAdd(interval, Number, (object) date);
      }
      catch
      {
        return (object) "";
      }
    }

    public object XDateDiff(string interval, object date1Obj, object date2Obj)
    {
      try
      {
        DateTime date1 = Utils.ParseDate(date1Obj, true);
        DateTime date2 = Utils.ParseDate(date2Obj, true);
        return (object) DateAndTime.DateDiff(interval, (object) date1, (object) date2);
      }
      catch
      {
        return (object) "";
      }
    }

    protected object Range(object value, params object[] rangeItems)
    {
      try
      {
        for (int index = 0; index < rangeItems.Length; ++index)
        {
          if (!Utils.IsDecimal(value) ? (!Utils.IsDate(value) ? string.Compare(string.Concat(value), string.Concat(rangeItems[index]), true) < 0 : Utils.ParseDate(value, true) < Utils.ParseDate(rangeItems[index], true)) : Utils.ParseDecimal(value, true) < Utils.ParseDecimal(rangeItems[index], true))
            return (object) index;
        }
        return (object) rangeItems.Length;
      }
      catch
      {
      }
      return (object) "";
    }

    protected object RangeLow(object value, params object[] rangeItems)
    {
      try
      {
        for (int index = 0; index < rangeItems.Length; ++index)
        {
          if (!Utils.IsDecimal(value) ? (!Utils.IsDate(value) ? string.Compare(string.Concat(value), string.Concat(rangeItems[index]), true) <= 0 : Utils.ParseDate(value, true) <= Utils.ParseDate(rangeItems[index], true)) : Utils.ParseDecimal(value, true) <= Utils.ParseDecimal(rangeItems[index], true))
            return (object) index;
        }
        return (object) rangeItems.Length;
      }
      catch
      {
      }
      return (object) "";
    }

    protected object Match(object value, params object[] values)
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

    protected object Pick(int index, params object[] values)
    {
      return index < 0 || index > values.Length - 1 ? (object) null : values[index];
    }

    protected int Count(params object[] items)
    {
      int num = 0;
      for (int index = 0; index < items.Length; ++index)
      {
        if (!this.IsEmpty(items[index]))
          ++num;
      }
      return num;
    }

    protected string FormatDate(object date, string format)
    {
      try
      {
        return Utils.ParseDate(date, true).ToString(format);
      }
      catch
      {
        return "";
      }
    }

    protected string FormatInt(object value, string format)
    {
      try
      {
        return Utils.ParseInt(value, true).ToString(format);
      }
      catch
      {
        return "";
      }
    }

    protected string FormatDec(object value, string format)
    {
      try
      {
        return Utils.ParseDecimal(value, true).ToString(format);
      }
      catch
      {
        return "";
      }
    }

    protected string Int2Text(object value)
    {
      try
      {
        return NumberToTextConverter.ToString(Utils.ParseLong(value, true));
      }
      catch
      {
        return "";
      }
    }

    protected string Dec2Text(object value) => this.Dec2Text(value, false);

    protected string Dec2Text(object value, bool dollarsAndCents)
    {
      try
      {
        Decimal num = Utils.ParseDecimal(value, true);
        NumberToTextOption options = NumberToTextOption.TwoDecimalPlaces;
        if (dollarsAndCents)
          options |= NumberToTextOption.DollarsAndCents;
        return NumberToTextConverter.ToString(num, options);
      }
      catch
      {
        return "";
      }
    }

    protected string Money2Text(object value) => this.Dec2Text(value, true);

    public override object InitializeLifetimeService() => (object) null;
  }
}

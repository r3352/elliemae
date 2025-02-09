// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Range`1
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [Serializable]
  public class Range<T> : IXmlSerializable
  {
    private T minValue;
    private T maxValue;

    public Range()
      : this(default (T), default (T))
    {
    }

    public Range(T minValue, T maxValue)
    {
      this.minValue = ((IComparable<T>) (object) minValue).CompareTo(maxValue) <= 0 ? minValue : throw new ArgumentException("Minimum value must be <= maximum value");
      this.maxValue = maxValue;
    }

    public Range(XmlSerializationInfo info)
    {
      try
      {
        this.minValue = (T) info.GetValue("min", typeof (T));
      }
      catch
      {
        this.minValue = this.ValueTypeMinimum;
      }
      try
      {
        this.maxValue = (T) info.GetValue("max", typeof (T));
      }
      catch
      {
        this.maxValue = this.ValueTypeMaximum;
      }
    }

    public T Minimum => this.minValue;

    public T Maximum => this.maxValue;

    public T ValueTypeMinimum => (T) Range<T>.getSmallestValueOfType();

    public T ValueTypeMaximum => (T) this.getLargestValueOfType();

    public bool IsInRange(T value)
    {
      IComparable<T> minValue = (IComparable<T>) (object) this.minValue;
      IComparable<T> maxValue = (IComparable<T>) (object) this.maxValue;
      return minValue.CompareTo(value) <= 0 && maxValue.CompareTo(value) >= 0;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      if (!this.minValue.Equals(Range<T>.getSmallestValueOfType()))
        info.AddValue("min", (object) this.minValue);
      if (this.maxValue.Equals(this.getLargestValueOfType()))
        return;
      info.AddValue("max", (object) this.maxValue);
    }

    public static Range<T> Parse(string minText, string maxText, T defaultMin, T defaultMax)
    {
      T minValue = defaultMin;
      T maxValue = defaultMax;
      try
      {
        minValue = (T) Convert.ChangeType((object) minText, typeof (T));
      }
      catch
      {
      }
      try
      {
        maxValue = (T) Convert.ChangeType((object) maxText, typeof (T));
      }
      catch
      {
      }
      return new Range<T>(minValue, maxValue);
    }

    private static object getSmallestValueOfType()
    {
      if (typeof (T) == typeof (Decimal))
        return (object) Decimal.MinValue;
      if (typeof (T) == typeof (int))
        return (object) int.MinValue;
      if (typeof (T) == typeof (long))
        return (object) long.MinValue;
      if (typeof (T) == typeof (float))
        return (object) float.MinValue;
      if (typeof (T) == typeof (double))
        return (object) double.MinValue;
      return typeof (T) == typeof (DateTime) ? (object) DateTime.MinValue : (object) null;
    }

    private object getLargestValueOfType()
    {
      if (typeof (T) == typeof (Decimal))
        return (object) Decimal.MaxValue;
      if (typeof (T) == typeof (int))
        return (object) int.MaxValue;
      if (typeof (T) == typeof (long))
        return (object) long.MaxValue;
      if (typeof (T) == typeof (float))
        return (object) float.MaxValue;
      if (typeof (T) == typeof (double))
        return (object) double.MaxValue;
      return typeof (T) == typeof (DateTime) ? (object) DateTime.MaxValue : (object) null;
    }
  }
}

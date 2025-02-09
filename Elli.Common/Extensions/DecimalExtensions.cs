// Decompiled with JetBrains decompiler
// Type: Elli.Common.Extensions.DecimalExtensions
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;

#nullable disable
namespace Elli.Common.Extensions
{
  public static class DecimalExtensions
  {
    private static Decimal[] PositivePowersOfTen = new Decimal[13]
    {
      1M,
      10M,
      100M,
      1000M,
      10000M,
      100000M,
      1000000M,
      10000000M,
      100000000M,
      1000000000M,
      10000000000M,
      100000000000M,
      1000000000000M
    };
    private static Decimal[] NegativePowersOfTen = new Decimal[13]
    {
      1M,
      0.1M,
      0.01M,
      0.001M,
      0.0001M,
      0.00001M,
      0.000001M,
      0.0000001M,
      0.00000001M,
      0.000000001M,
      0.0000000001M,
      0.00000000001M,
      0.000000000001M
    };

    public static Decimal TruncateToPrecision(this Decimal value, int decimalPlaces)
    {
      Decimal num1 = Math.Truncate(value);
      Decimal num2 = value - num1;
      Decimal num3 = (Decimal) Math.Pow(10.0, (double) decimalPlaces);
      Decimal num4 = num3;
      Decimal num5 = Math.Truncate(num2 * num4) / num3;
      return num1 + num5;
    }

    public static Decimal RoundToPrecision(this Decimal value, int decimalPlaces)
    {
      return Decimal.Round(value * DecimalExtensions.PositivePowersOfTen[decimalPlaces], 0, MidpointRounding.AwayFromZero) * DecimalExtensions.NegativePowersOfTen[decimalPlaces];
    }

    public static string ToDecimalString(this Decimal value, int numOfDecimals)
    {
      return value.ToString("N" + (object) numOfDecimals);
    }

    public static string ToDecimalString(this Decimal? value, int numOfDecimals)
    {
      return value.HasValue ? value.Value.ToString("N" + (object) numOfDecimals) : string.Empty;
    }

    public static bool IsNotNullOrZero(this Decimal? value) => !value.IsNullOrZero();

    public static bool IsNullOrZero(this Decimal? value)
    {
      if (!value.HasValue)
        return true;
      Decimal? nullable = value;
      Decimal num = 0M;
      return nullable.GetValueOrDefault() == num & nullable.HasValue;
    }
  }
}

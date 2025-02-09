// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Common.NumberToTextConverter
// Assembly: Elli.CalculationEngine.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BBD0C9BB-76EB-4848-9A1B-D338F49271A1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Common.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.CalculationEngine.Common
{
  public static class NumberToTextConverter
  {
    private static readonly string[] decimalGroupNames = new string[5]
    {
      "",
      " Thousand",
      " Million",
      " Billion",
      " Trillion"
    };
    private static readonly string[] belowTwentyNumberNames = new string[20]
    {
      "Zero",
      "One",
      "Two",
      "Three",
      "Four",
      "Five",
      "Six",
      "Seven",
      "Eight",
      "Nine",
      "Ten",
      "Eleven",
      "Twelve",
      "Thirteen",
      "Fourteen",
      "Fifteen",
      "Sixteen",
      "Seventeen",
      "Eighteen",
      "Nineteen"
    };
    private static readonly string[] tensNames = new string[10]
    {
      "",
      "",
      "Twenty",
      "Thirty",
      "Forty",
      "Fifty",
      "Sixty",
      "Seventy",
      "Eighty",
      "Ninety"
    };

    public static string ToString(Decimal value)
    {
      return NumberToTextConverter.ToString(value, NumberToTextOption.None);
    }

    public static string ToString(Decimal value, NumberToTextOption options)
    {
      string str1 = NumberToTextConverter.ToString(Convert.ToInt64(Math.Floor(value)), NumberToTextConverter.isOptionSelected(options, NumberToTextOption.BlankIfZero));
      if (NumberToTextConverter.isOptionSelected(options, NumberToTextOption.DollarsAndCents) && str1 != "")
        str1 += " Dollars";
      if (NumberToTextConverter.isOptionSelected(options, NumberToTextOption.TwoDecimalPlaces))
      {
        string str2 = NumberToTextConverter.ToString((long) NumberToTextConverter.getFractionalPortion(value, 2), NumberToTextConverter.isOptionSelected(options, NumberToTextOption.BlankIfZero));
        if (str2 != "")
        {
          if (str1 != "")
            str1 += " and ";
          str1 = !NumberToTextConverter.isOptionSelected(options, NumberToTextOption.DollarsAndCents) ? str1 + str2 + " Hundredths" : str1 + str2 + " Cents";
        }
      }
      return str1;
    }

    public static string ToString(long value) => NumberToTextConverter.ToString(value, false);

    public static string ToString(long value, bool blankIfZero)
    {
      int[] numArray = NumberToTextConverter.splitDecimalGroups(value);
      if (numArray.Length > NumberToTextConverter.decimalGroupNames.Length)
        throw new OverflowException("The specified value exceeds the maximum of 999,999,999,999,999");
      string str = "";
      for (int index = numArray.Length - 1; index >= 0; --index)
      {
        if (numArray[index] > 0)
        {
          if (str != "")
            str += " ";
          str = str + NumberToTextConverter.threeDigitValueToString(numArray[index]) + NumberToTextConverter.decimalGroupNames[index];
        }
      }
      if (str == "" && !blankIfZero)
        str = NumberToTextConverter.belowTwentyNumberNames[0];
      return str;
    }

    private static bool isOptionSelected(
      NumberToTextOption options,
      NumberToTextOption optionToCheck)
    {
      return (options & optionToCheck) != 0;
    }

    private static int getFractionalPortion(Decimal value, int decimalPlaces)
    {
      string[] strArray = value.ToString("N" + (object) decimalPlaces).Split('.');
      return strArray.Length < 2 || strArray[1].Length == 0 ? 0 : int.Parse(strArray[1]);
    }

    private static int[] splitDecimalGroups(long value)
    {
      string s = value.ToString();
      List<int> intList = new List<int>();
      while (s.Length > 0)
      {
        if (s.Length < 3)
        {
          intList.Add(int.Parse(s));
          s = "";
        }
        else
        {
          intList.Add(int.Parse(s.Substring(s.Length - 3, 3)));
          s = s.Substring(0, s.Length - 3);
        }
      }
      return intList.ToArray();
    }

    private static string threeDigitValueToString(int value)
    {
      if (value < 100)
        return NumberToTextConverter.twoDigitValueToString(value);
      return value % 100 == 0 ? NumberToTextConverter.belowTwentyNumberNames[value / 100] + " Hundred" : NumberToTextConverter.belowTwentyNumberNames[value / 100] + " Hundred " + NumberToTextConverter.twoDigitValueToString(value % 100);
    }

    private static string twoDigitValueToString(int value)
    {
      if (value < 20)
        return NumberToTextConverter.belowTwentyNumberNames[value];
      return value % 10 == 0 ? NumberToTextConverter.tensNames[value / 10] : NumberToTextConverter.tensNames[value / 10] + "-" + NumberToTextConverter.belowTwentyNumberNames[value % 10];
    }
  }
}

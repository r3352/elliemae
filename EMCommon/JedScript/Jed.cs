// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.JedScript.Jed
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.JedScript
{
  public class Jed
  {
    private const byte DDMask = 7;
    private const byte DSMask = 8;
    private const byte CommaMask = 16;
    private const byte NIEMask = 32;
    private const byte ZFMask = 192;
    public const byte NoFormat = 0;
    public const byte NoDD = 0;
    public const byte OneDD = 1;
    public const byte TwoDD = 2;
    public const byte ThreeDD = 3;
    public const byte FourDD = 4;
    public const byte DS = 8;
    public const byte Comma = 16;
    public const byte NIE = 32;
    public const byte ZF1 = 0;
    public const byte ZF2 = 64;
    public const byte ZF3 = 128;
    public const byte ZF4 = 192;
    private static readonly Regex notNumberOrDot = new Regex("[^0-9.-]");
    private static string[] NumberName = new string[20]
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
      "Fiftheen",
      "Sixteen",
      "Seventeen",
      "Eighteen",
      "Nineteen"
    };
    private static string[] NumberNamety = new string[10]
    {
      "dummy",
      "dummy",
      "Twenty",
      "Thirty",
      "Fourty",
      "Fifty",
      "Sixty",
      "Seventy",
      "Eighty",
      "Ninety"
    };

    public static string NF(double inValue, byte flag, int padding)
    {
      if (((int) flag & 32) == 32)
        return Jed.PutNumberInEnglish(inValue);
      if (inValue == 0.0)
      {
        switch ((int) flag & 192)
        {
          case 64:
            return "0.00";
          case 128:
            return "-0-";
          case 192:
            return string.Empty;
        }
      }
      NumberFormatInfo provider = new NumberFormatInfo();
      switch ((int) flag & 7)
      {
        case 0:
          provider.CurrencyDecimalDigits = 0;
          break;
        case 1:
          provider.CurrencyDecimalDigits = 1;
          break;
        case 2:
          provider.CurrencyDecimalDigits = 2;
          break;
        case 3:
          provider.CurrencyDecimalDigits = 3;
          break;
        case 4:
          provider.CurrencyDecimalDigits = 4;
          break;
      }
      provider.CurrencySymbol = ((int) flag & 8) != 8 ? string.Empty : "$";
      provider.CurrencyGroupSeparator = ((int) flag & 16) != 16 ? string.Empty : ",";
      provider.CurrencyNegativePattern = 12;
      return inValue.ToString("c", (IFormatProvider) provider);
    }

    public static string BF(bool bValue, string strValue) => bValue ? strValue : "";

    public static string BF(bool bValue, string trueValue, string falseValue)
    {
      return bValue ? trueValue : falseValue;
    }

    public static double BF(bool bValue, double trueValue, double falseValue)
    {
      return bValue ? trueValue : falseValue;
    }

    public static string Date()
    {
      DateTime now = DateTime.Now;
      return now.Month.ToString() + "/" + now.Day.ToString() + "/" + now.Year.ToString();
    }

    public static double S2N(string strValue)
    {
      if (strValue == null || strValue == string.Empty)
        return 0.0;
      strValue = Jed.notNumberOrDot.Replace(strValue, "");
      try
      {
        return Convert.ToDouble(strValue);
      }
      catch
      {
        return 0.0;
      }
    }

    public static string GetPhoneNo(string phoneNum)
    {
      return phoneNum == null || phoneNum == string.Empty || phoneNum.Length <= 4 ? string.Empty : phoneNum.Substring(4);
    }

    public static string GetPhoneNoWithoutExt(string phoneNum)
    {
      if (phoneNum == null || phoneNum == string.Empty || phoneNum.Length <= 4)
        return string.Empty;
      return phoneNum.Substring(4).Length > 8 ? phoneNum.Substring(4, 8) : phoneNum.Substring(4);
    }

    public static string GetAreaCode(string phoneNum)
    {
      return phoneNum == null || phoneNum == string.Empty || phoneNum.Length < 3 ? string.Empty : phoneNum.Substring(0, 3);
    }

    public static string GetPhoneExt(string phoneNum)
    {
      return phoneNum == null || phoneNum == string.Empty || phoneNum.Length < 14 ? string.Empty : phoneNum.Substring(13);
    }

    public static string Min(string numStr)
    {
      return numStr == null || numStr == string.Empty ? string.Empty : Jed.S2N(numStr).ToString();
    }

    public static string Min(string numStr1, string numStr2)
    {
      if (numStr2 == null || numStr2 == string.Empty)
        return Jed.Min(numStr1);
      return numStr1 == null || numStr1 == string.Empty ? Jed.Min(numStr2) : Math.Min(Jed.S2N(numStr1), Jed.S2N(numStr2)).ToString();
    }

    public static string Min(string numStr1, string numStr2, string numStr3)
    {
      if (numStr3 == null || numStr3 == string.Empty)
        return Jed.Min(numStr1, numStr2);
      if (numStr2 == null || numStr2 == string.Empty)
        return Jed.Min(numStr1, numStr3);
      if (numStr1 == null || numStr1 == string.Empty)
        return Jed.Min(numStr2, numStr3);
      double val1 = Jed.S2N(numStr1);
      double val2_1 = Jed.S2N(numStr2);
      double val2_2 = Jed.S2N(numStr3);
      return Math.Min(Math.Min(val1, val2_1), val2_2).ToString();
    }

    public static double Num() => 0.0;

    public static double Num(double dValue) => dValue;

    private static string PutSmallIntegerInEnglish(double dValue)
    {
      if (dValue == 0.0)
        return string.Empty;
      string str1 = string.Empty;
      int num = (int) dValue;
      int index1 = num / 100;
      if (index1 != 0)
        str1 = str1 + Jed.NumberName[index1] + " Hundred ";
      int index2 = num % 100;
      if (index2 == 0)
        return str1;
      if (index2 < 20)
        return str1 + Jed.NumberName[index2] + " ";
      int index3 = index2 / 10;
      int index4 = index2 % 10;
      string str2;
      if (index4 == 0)
        str2 = str1 + Jed.NumberNamety[index3] + " ";
      else
        str2 = str1 + Jed.NumberNamety[index3] + " " + Jed.NumberName[index4] + " ";
      return str2;
    }

    private static string PutNumberInEnglish(double dValue)
    {
      double num = Math.Floor(dValue);
      if (num == 0.0)
        return "Zero";
      string str1 = Jed.PutSmallIntegerInEnglish(Math.Floor(num / 1000000.0));
      Console.WriteLine(str1);
      string str2 = Jed.PutSmallIntegerInEnglish(Math.Floor(num % 1000000.0 / 1000.0));
      Console.WriteLine(str2);
      string str3 = Jed.PutSmallIntegerInEnglish(num % 1000.0);
      Console.WriteLine(str3);
      string str4 = string.Empty;
      if (str1 != string.Empty)
        str4 = str1 + "Million ";
      if (str2 != string.Empty)
        str4 = str4 + str2 + "Thousand ";
      return str4 + str3;
    }
  }
}

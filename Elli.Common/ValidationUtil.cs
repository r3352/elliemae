// Decompiled with JetBrains decompiler
// Type: Elli.Common.ValidationUtil
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;

#nullable disable
namespace Elli.Common
{
  public class ValidationUtil
  {
    public static void ValidateUtc(DateTime? dateTime)
    {
      if (dateTime.HasValue && dateTime.Value.Kind != DateTimeKind.Utc)
        throw new InvalidTimeZoneException("The specified DateTime must be a UTC datetime value.");
    }

    public static int[] GetVersionInfo(string versionString)
    {
      int[] versionInfo = new int[4];
      if (string.IsNullOrEmpty(versionString))
        return versionInfo;
      string[] strArray = versionString.Split('.');
      if (strArray.Length != 0)
        versionInfo[0] = ValidationUtil.getNumber(strArray[0]);
      if (strArray.Length > 1)
        versionInfo[1] = ValidationUtil.getNumber(strArray[1]);
      if (strArray.Length > 2)
        versionInfo[2] = ValidationUtil.getNumber(strArray[2]);
      if (strArray.Length > 3)
        versionInfo[3] = ValidationUtil.getNumber(strArray[3]);
      return versionInfo;
    }

    private static int getNumber(string input)
    {
      string s = "";
      foreach (char c in input)
      {
        if (char.IsDigit(c))
          s += c.ToString();
        else
          break;
      }
      return int.Parse(s);
    }
  }
}

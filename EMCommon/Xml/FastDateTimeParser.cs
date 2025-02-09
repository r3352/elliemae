// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Xml.FastDateTimeParser
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common;
using System;
using System.Configuration;
using System.Globalization;

#nullable disable
namespace EllieMae.EMLite.Xml
{
  public class FastDateTimeParser
  {
    public static bool Enabled = Utils.ParseBoolean((object) ConfigurationManager.AppSettings["FastDateTimeParser.Enabled"]);

    public static bool TryParse(string s, DateTimeStyles styles, out DateTime dateTime)
    {
      dateTime = DateTime.MinValue;
      int yyyy;
      int MM;
      int dd;
      if (!FastDateTimeParser.Enabled || string.IsNullOrEmpty(s) || s.Length != 10 && s.Length != 19 && s.Length != 20 && s.Length != 23 && s.Length != 24 && s.Length != 26 || s[4] != '-' || s[7] != '-' || s.Length >= 19 && (s[13] != ':' || s[16] != ':') || s.Length == 20 && s[19] != 'Z' || s.Length > 20 && s[19] != '.' || s.Length == 24 && s[23] != 'Z' || s.Length == 26 && (s[23] != ' ' || s[24] != 'A' && s[24] != 'P' || s[25] != 'M') || !FastDateTimeParser.TryGetNumber(s, 0, 4, out yyyy) || !FastDateTimeParser.TryGetNumber(s, 5, 2, out MM) || !FastDateTimeParser.TryGetNumber(s, 8, 2, out dd))
        return false;
      if (s.Length == 10)
        return FastDateTimeParser.TryGetDate(yyyy, MM, dd, 0, 0, 0, 0, styles, DateTimeKind.Unspecified, out dateTime);
      int hh;
      int mm;
      int ss;
      if (!FastDateTimeParser.TryGetNumber(s, 11, 2, out hh) || !FastDateTimeParser.TryGetNumber(s, 14, 2, out mm) || !FastDateTimeParser.TryGetNumber(s, 17, 2, out ss))
        return false;
      if (s.Length == 19)
        return FastDateTimeParser.TryGetDate(yyyy, MM, dd, hh, mm, ss, 0, styles, DateTimeKind.Unspecified, out dateTime);
      if (s.Length == 20)
        return FastDateTimeParser.TryGetDate(yyyy, MM, dd, hh, mm, ss, 0, styles, DateTimeKind.Utc, out dateTime);
      int ms;
      if (!FastDateTimeParser.TryGetNumber(s, 20, 3, out ms))
        return false;
      if (s.Length == 23)
        return FastDateTimeParser.TryGetDate(yyyy, MM, dd, hh, mm, ss, ms, styles, DateTimeKind.Unspecified, out dateTime);
      if (s.Length == 24)
        return FastDateTimeParser.TryGetDate(yyyy, MM, dd, hh, mm, ss, ms, styles, DateTimeKind.Utc, out dateTime);
      if (s[24] == 'P' && hh < 12)
        hh += 12;
      if (s[24] == 'A' && hh == 12)
        hh = 0;
      return FastDateTimeParser.TryGetDate(yyyy, MM, dd, hh, mm, ss, ms, styles, DateTimeKind.Unspecified, out dateTime);
    }

    private static bool TryGetDate(
      int yyyy,
      int MM,
      int dd,
      int hh,
      int mm,
      int ss,
      int ms,
      DateTimeStyles styles,
      DateTimeKind kind,
      out DateTime date)
    {
      try
      {
        if (kind == DateTimeKind.Unspecified)
        {
          if ((styles & DateTimeStyles.AssumeUniversal) != DateTimeStyles.None)
            kind = DateTimeKind.Utc;
          if ((styles & DateTimeStyles.AssumeLocal) != DateTimeStyles.None)
            kind = DateTimeKind.Local;
        }
        date = new DateTime(yyyy, MM, dd, hh, mm, ss, ms, kind);
        if (kind == DateTimeKind.Utc && (styles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.None)
          date = date.ToLocalTime();
        if (kind == DateTimeKind.Local && (styles & DateTimeStyles.AdjustToUniversal) != DateTimeStyles.None)
          date = date.ToUniversalTime();
        return true;
      }
      catch
      {
        date = DateTime.MinValue;
        return false;
      }
    }

    private static bool TryGetNumber(string s, int start, int len, out int value)
    {
      value = 0;
      int num1 = start + len;
      for (int index = start; index < num1; ++index)
      {
        int num2 = (int) s[index] - 48;
        switch (num2)
        {
          case 0:
          case 1:
          case 2:
          case 3:
          case 4:
          case 5:
          case 6:
          case 7:
          case 8:
          case 9:
            value = value * 10 + num2;
            continue;
          default:
            value = int.MinValue;
            return false;
        }
      }
      return true;
    }
  }
}

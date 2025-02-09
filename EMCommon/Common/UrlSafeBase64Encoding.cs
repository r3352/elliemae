// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UrlSafeBase64Encoding
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public static class UrlSafeBase64Encoding
  {
    public static string Encode(string input)
    {
      if (input == null)
        return (string) null;
      return Convert.ToBase64String(Encoding.UTF8.GetBytes(input)).TrimEnd('=').Replace("+", "-").Replace("/", "_");
    }

    public static string Decode(string input)
    {
      if (input == null)
        return (string) null;
      string s = input.Replace("-", "+").Replace("_", "/");
      switch (s.Length % 4)
      {
        case 0:
          try
          {
            return Encoding.UTF8.GetString(Convert.FromBase64String(s));
          }
          catch (Exception ex)
          {
            return (string) null;
          }
        case 2:
          s += "==";
          goto case 0;
        case 3:
          s += "=";
          goto case 0;
        default:
          return (string) null;
      }
    }
  }
}

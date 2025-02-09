// Decompiled with JetBrains decompiler
// Type: Jose.Base64Url
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System;

#nullable disable
namespace Jose
{
  public static class Base64Url
  {
    public static string Encode(byte[] input)
    {
      return Convert.ToBase64String(input).Split('=')[0].Replace('+', '-').Replace('/', '_');
    }

    public static byte[] Decode(string input)
    {
      string s = input.Replace('-', '+').Replace('_', '/');
      switch (s.Length % 4)
      {
        case 0:
          return Convert.FromBase64String(s);
        case 2:
          s += "==";
          goto case 0;
        case 3:
          s += "=";
          goto case 0;
        default:
          throw new Exception("Illegal base64url string!");
      }
    }
  }
}

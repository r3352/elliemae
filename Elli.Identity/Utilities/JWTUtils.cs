// Decompiled with JetBrains decompiler
// Type: Elli.Identity.Utilities.JWTUtils
// Assembly: Elli.Identity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0ECC974-8E0E-42B3-88D3-2EAE5F37B212
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Identity.dll

using Elli.Identity.Key;
using Elli.Identity.Provider;
using Jose;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Elli.Identity.Utilities
{
  public static class JWTUtils
  {
    private static readonly string OAPI_TOKEN_ISSUER = "urn:elli:service:ids";

    public static ITokenProvider GetTokenProvider(string token, ITokenKeyFactory keyFactory)
    {
      string[] strArray = token.Split('.');
      if (strArray.Length < 3)
        throw new InvalidSecurityTokenException("Invalid token");
      IDictionary<string, object> dictionary;
      try
      {
        dictionary = new JSSerializerMapper().Parse<IDictionary<string, object>>(Encoding.UTF8.GetString(JWTUtils.Decode(strArray[1])));
        if (dictionary == null)
          throw new Exception();
      }
      catch
      {
        throw new InvalidSecurityTokenException("Invalid token");
      }
      if (!dictionary.ContainsKey("iss"))
        throw new InvalidSecurityTokenException("Invalid issuer");
      return string.Compare((string) dictionary["iss"], JWTUtils.OAPI_TOKEN_ISSUER, StringComparison.InvariantCultureIgnoreCase) == 0 ? (ITokenProvider) new ElliOapiJwtTokenProvider(keyFactory) : (ITokenProvider) new ElliJwtTokenProvider(keyFactory);
    }

    private static byte[] Decode(string payload)
    {
      string s = payload.Replace('-', '+').Replace('_', '/');
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
          throw new ArgumentOutOfRangeException(nameof (payload), "Illegal base64url string!");
      }
    }
  }
}

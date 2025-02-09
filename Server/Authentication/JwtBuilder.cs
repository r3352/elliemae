// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Authentication.JwtBuilder
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server.Authentication
{
  internal class JwtBuilder
  {
    private readonly JWTKey _key;
    private readonly string _iss;
    private readonly string _sub;

    public JwtBuilder(JWTKey key, string issuer, string subject)
    {
      this._key = key;
      this._iss = issuer;
      this._sub = subject;
    }

    public string Create(string audience, string scope, Dictionary<string, object> claims)
    {
      DateTime dateTime1 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
      DateTime utcNow = DateTime.UtcNow;
      DateTime dateTime2 = utcNow.AddSeconds(300.0);
      TimeSpan timeSpan1 = utcNow - dateTime1;
      TimeSpan timeSpan2 = dateTime2 - dateTime1;
      Dictionary<string, object> dictionary1 = new Dictionary<string, object>();
      dictionary1.Add("alg", (object) this._key.alg);
      dictionary1.Add("typ", (object) "JWT");
      Dictionary<string, object> dictionary2 = new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      dictionary2.Add("sub", (object) this._sub);
      dictionary2.Add("aud", this.GetAudience(audience));
      dictionary2.Add(nameof (scope), (object) scope);
      dictionary2.Add("iss", (object) this._iss);
      dictionary2.Add("jti", (object) Guid.NewGuid().ToString("D"));
      dictionary2.Add("iat", (object) Convert.ToInt32(timeSpan1.TotalSeconds));
      dictionary2.Add("exp", (object) Convert.ToInt32(timeSpan2.TotalSeconds));
      dictionary2.Add("elli_idt", (object) "Application");
      if (claims != null)
      {
        foreach (KeyValuePair<string, object> claim in claims)
          dictionary2.Add(claim.Key, claim.Value);
      }
      string stringToSign = this.Base64UrlEncode(dictionary1) + "." + this.Base64UrlEncode(dictionary2);
      return stringToSign + "." + this.GenerateSignature(this._key, stringToSign);
    }

    private object GetAudience(string audience)
    {
      if (!audience.Contains(","))
        return (object) audience;
      return (object) audience.Split(',');
    }

    private string Base64UrlEncode(Dictionary<string, object> dictionary)
    {
      JsonSerializer jsonSerializer = JsonSerializer.CreateDefault();
      StringBuilder sb = new StringBuilder();
      using (StringWriter stringWriter = new StringWriter(sb))
      {
        using (JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) stringWriter))
          jsonSerializer.Serialize((JsonWriter) jsonTextWriter, (object) dictionary);
      }
      return this.Base64UrlEncode(new UTF8Encoding(false).GetBytes(sb.ToString()));
    }

    private string Base64UrlEncode(byte[] bytes)
    {
      return Convert.ToBase64String(bytes).Split('=')[0].Replace('+', '-').Replace('/', '_');
    }

    private string GenerateSignature(JWTKey key, string stringToSign)
    {
      if (!string.Equals(key.alg, "HS256", StringComparison.OrdinalIgnoreCase))
        throw new NotSupportedException("Unsupported 'alg' value of " + key.alg);
      byte[] bytes = new UTF8Encoding(false).GetBytes(stringToSign);
      return this.Base64UrlEncode(new HMACSHA256(Convert.FromBase64String(key.k)).ComputeHash(bytes));
    }
  }
}

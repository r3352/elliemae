// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.CapsilonAIQ.S3Signers.SignerForAuthorizationHeader
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Server.CapsilonAIQ.S3Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server.CapsilonAIQ.S3Signers
{
  public class SignerForAuthorizationHeader : SignerBase
  {
    public string ComputeSignature(
      IDictionary<string, string> headers,
      string queryParameters,
      string bodyHash,
      string awsAccessKey,
      string awsSecretKey)
    {
      DateTime utcNow = DateTime.UtcNow;
      string str1 = utcNow.ToString("yyyyMMddTHHmmssZ", (IFormatProvider) CultureInfo.InvariantCulture);
      headers.Add("x-aiq-date", str1);
      string str2 = this.EndpointUri.Host;
      if (!this.EndpointUri.IsDefaultPort)
        str2 = str2 + ":" + (object) this.EndpointUri.Port;
      headers.Add("host", str2);
      string canonicalizedHeaderNames = this.CanonicalizeHeaderNames(headers);
      string canonicalizedHeaders = this.CanonicalizeHeaders(headers);
      string empty = string.Empty;
      if (!string.IsNullOrEmpty(queryParameters))
      {
        Dictionary<string, string> dictionary = ((IEnumerable<string>) queryParameters.Split('&')).Select<string, string[]>((Func<string, string[]>) (p => p.Split('='))).ToDictionary<string[], string, string>((Func<string[], string>) (nameval => nameval[0]), (Func<string[], string>) (nameval => nameval.Length <= 1 ? "" : nameval[1]));
        StringBuilder stringBuilder = new StringBuilder();
        List<string> stringList = new List<string>((IEnumerable<string>) dictionary.Keys);
        stringList.Sort((IComparer<string>) StringComparer.Ordinal);
        foreach (string str3 in stringList)
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append("&");
          stringBuilder.AppendFormat("{0}={1}", (object) HttpHelpers.UrlEncode(str3), (object) HttpHelpers.UrlEncode(dictionary[str3]));
        }
        empty = stringBuilder.ToString();
      }
      string s = this.CanonicalizeRequest(this.EndpointUri, this.HttpMethod, empty, canonicalizedHeaderNames, canonicalizedHeaders, bodyHash);
      byte[] hash = SignerBase.CanonicalRequestHashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(s));
      StringBuilder stringBuilder1 = new StringBuilder();
      string date = utcNow.ToString("yyyyMMdd", (IFormatProvider) CultureInfo.InvariantCulture);
      string str4 = string.Format("{0}/{1}/{2}/{3}", (object) date, (object) this.Region, (object) this.Service, (object) this.TERMINATOR);
      stringBuilder1.AppendFormat("{0}-{1}\n{2}\n{3}\n", (object) "AIQ1", (object) "HMAC-SHA256", (object) str1, (object) str4);
      stringBuilder1.Append(SignerBase.ToHexString(hash, true));
      KeyedHashAlgorithm keyedHashAlgorithm = KeyedHashAlgorithm.Create("HMACSHA256");
      keyedHashAlgorithm.Key = this.DeriveSigningKey("HMACSHA256", awsSecretKey, this.Region, date, this.Service);
      string hexString = SignerBase.ToHexString(keyedHashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(stringBuilder1.ToString())), true);
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder2.AppendFormat("{0}-{1} ", (object) "AIQ1", (object) "HMAC-SHA256");
      stringBuilder2.AppendFormat("Credential={0}/{1}, ", (object) awsAccessKey, (object) str4);
      stringBuilder2.AppendFormat("SignedHeaders={0}, ", (object) canonicalizedHeaderNames);
      stringBuilder2.AppendFormat("Signature={0}", (object) hexString);
      return stringBuilder2.ToString();
    }
  }
}

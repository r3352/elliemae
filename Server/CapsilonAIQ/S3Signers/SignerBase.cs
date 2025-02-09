// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.CapsilonAIQ.S3Signers.SignerBase
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Server.CapsilonAIQ.S3Utility;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.EMLite.Server.CapsilonAIQ.S3Signers
{
  public abstract class SignerBase
  {
    public const string EMPTY_BODY_SHA256 = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855�";
    public const string SCHEME = "AIQ1�";
    public const string ALGORITHM = "HMAC-SHA256�";
    public const string ISO8601BasicFormat = "yyyyMMddTHHmmssZ�";
    public const string DateStringFormat = "yyyyMMdd�";
    public const string X_Amz_Algorithm = "X-Amz-Algorithm�";
    public const string X_Amz_Credential = "X-Amz-Credential�";
    public const string X_Amz_SignedHeaders = "X-Amz-SignedHeaders�";
    public const string X_AIQ_DATE = "x-aiq-date�";
    public const string X_Amz_Signature = "X-Amz-Signature�";
    public const string X_Amz_Expires = "X-Amz-Expires�";
    public const string X_AIQ_CONTENT_SHA_256 = "x-aiq-content-sha256�";
    public const string X_Amz_Decoded_Content_Length = "X-Amz-Decoded-Content-Length�";
    public const string X_Amz_Meta_UUID = "X-Amz-Meta-UUID�";
    public const string HMACSHA256 = "HMACSHA256�";
    protected static readonly Regex CompressWhitespaceRegex = new Regex("\\s+");
    public static HashAlgorithm CanonicalRequestHashAlgorithm = HashAlgorithm.Create("SHA-256");

    public Uri EndpointUri { get; set; }

    public string HttpMethod { get; set; }

    public string Service { get; set; }

    public string Region { get; set; }

    public string TERMINATOR { get; set; }

    protected string CanonicalizeHeaderNames(IDictionary<string, string> headers)
    {
      List<string> stringList = new List<string>((IEnumerable<string>) headers.Keys);
      stringList.Sort((IComparer<string>) StringComparer.OrdinalIgnoreCase);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string str in stringList)
      {
        if (stringBuilder.Length > 0)
          stringBuilder.Append(";");
        stringBuilder.Append(str.ToLower());
      }
      return stringBuilder.ToString();
    }

    protected virtual string CanonicalizeHeaders(IDictionary<string, string> headers)
    {
      if (headers == null || headers.Count == 0)
        return string.Empty;
      SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
      foreach (string key in (IEnumerable<string>) headers.Keys)
        sortedDictionary.Add(key.ToLower(), headers[key]);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string key in sortedDictionary.Keys)
      {
        string str = SignerBase.CompressWhitespaceRegex.Replace(sortedDictionary[key], " ");
        stringBuilder.AppendFormat("{0}:{1}\n", (object) key, (object) str.Trim());
      }
      return stringBuilder.ToString();
    }

    protected string CanonicalizeRequest(
      Uri endpointUri,
      string httpMethod,
      string queryParameters,
      string canonicalizedHeaderNames,
      string canonicalizedHeaders,
      string bodyHash)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("{0}\n", (object) httpMethod);
      stringBuilder.AppendFormat("{0}\n", (object) this.CanonicalResourcePath(endpointUri));
      stringBuilder.AppendFormat("{0}\n", (object) queryParameters);
      stringBuilder.AppendFormat("{0}\n", (object) canonicalizedHeaders);
      stringBuilder.AppendFormat("{0}\n", (object) canonicalizedHeaderNames);
      stringBuilder.Append(bodyHash);
      return stringBuilder.ToString();
    }

    protected string CanonicalResourcePath(Uri endpointUri)
    {
      return string.IsNullOrEmpty(endpointUri.AbsolutePath) ? "/" : HttpHelpers.UrlEncode(endpointUri.AbsolutePath, true);
    }

    protected byte[] DeriveSigningKey(
      string algorithm,
      string awsSecretAccessKey,
      string region,
      string date,
      string service)
    {
      char[] charArray = ("AIQ1" + awsSecretAccessKey).ToCharArray();
      byte[] keyedHash1 = this.ComputeKeyedHash(algorithm, Encoding.UTF8.GetBytes(charArray), Encoding.UTF8.GetBytes(date));
      byte[] keyedHash2 = this.ComputeKeyedHash(algorithm, keyedHash1, Encoding.UTF8.GetBytes(region));
      byte[] keyedHash3 = this.ComputeKeyedHash(algorithm, keyedHash2, Encoding.UTF8.GetBytes(service));
      return this.ComputeKeyedHash(algorithm, keyedHash3, Encoding.UTF8.GetBytes(this.TERMINATOR));
    }

    protected byte[] ComputeKeyedHash(string algorithm, byte[] key, byte[] data)
    {
      KeyedHashAlgorithm keyedHashAlgorithm = KeyedHashAlgorithm.Create(algorithm);
      keyedHashAlgorithm.Key = key;
      return keyedHashAlgorithm.ComputeHash(data);
    }

    public static string ToHexString(byte[] data, bool lowercase)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < data.Length; ++index)
        stringBuilder.Append(data[index].ToString(lowercase ? "x2" : "X2"));
      return stringBuilder.ToString();
    }
  }
}

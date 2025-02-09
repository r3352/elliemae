// Decompiled with JetBrains decompiler
// Type: Elli.Identity.ElliOapiJwtTokenProvider
// Assembly: Elli.Identity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0ECC974-8E0E-42B3-88D3-2EAE5F37B212
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Identity.dll

using Elli.Identity.Key;
using Elli.Identity.Provider;
using Elli.Identity.Utilities;
using Jose;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.SecurityTokenService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Principal;

#nullable disable
namespace Elli.Identity
{
  public class ElliOapiJwtTokenProvider : ITokenProvider
  {
    private const string IssuerName = "urn:elli:service:ids";
    private readonly ITokenKeyFactory _keyFactory;
    private object _signatureKey;
    private object _validationKey;

    public ElliOapiJwtTokenProvider(ITokenKeyFactory keyFactory) => this._keyFactory = keyFactory;

    public ElliToken IssueToken(
      IEnumerable<ElliClaim> claims,
      string audience,
      DateTime expiration)
    {
      return this.IssueToken((IDictionary<string, object>) Enumerable.ToDictionary<ElliClaim, string, object>(claims, (Func<ElliClaim, string>) (p => p.Type.ToLower()), (Func<ElliClaim, object>) (p => (object) p.Value)), audience, expiration);
    }

    public ElliToken IssueToken(
      IDictionary<string, object> claims,
      string audience,
      DateTime expiration)
    {
      if (this._signatureKey == null)
        this._signatureKey = this._keyFactory.GetSignatureKey();
      if (this._signatureKey == null)
        throw new CryptographicException("Can't issue a security token when the signature key is null.");
      Dictionary<string, object> payload = new Dictionary<string, object>(claims);
      if (payload.ContainsKey("exp"))
        payload.Remove("exp");
      if (payload.ContainsKey("iss"))
        payload.Remove("iss");
      payload.Add("exp", (object) DateTimeUtils.ToEpochTime(expiration));
      payload.Add("iss", (object) "urn:elli:service:ids");
      payload.Add("aud", (object) audience);
      string str = JWT.Encode((object) payload, this._signatureKey, this.GetAlgorithm(this._keyFactory.Algorithm));
      return !string.IsNullOrWhiteSpace(str) ? new ElliToken()
      {
        Token = str,
        TokenType = TokenType.Jwt
      } : throw new SecurityException("An error occured while generating the token. Token is empty");
    }

    public ElliToken IssueToken(
      IDictionary<string, object> claims,
      IEnumerable<string> serviceAudience,
      DateTime expiration)
    {
      return this.IssueToken(claims, string.Join(",", serviceAudience), expiration);
    }

    public IPrincipal ValidateToken(ElliToken token, string audience)
    {
      if (token.TokenType != TokenType.Jwt)
        throw new NotSupportedException("Invalid token type submitted to provider.");
      if (this._validationKey == null)
        this._validationKey = this._keyFactory.GetValidationKey();
      IDictionary<string, object> dictionary = JWT.Decode<IDictionary<string, object>>(token.Token, this._validationKey);
      if (new DateTime(1970, 1, 1).AddSeconds((double) (int) dictionary["exp"]) < DateTime.UtcNow)
        throw new Microsoft.IdentityModel.Tokens.InvalidSecurityTokenException("Token has already expired.");
      if (string.Compare((string) dictionary["iss"], "urn:elli:service:ids", StringComparison.InvariantCultureIgnoreCase) != 0)
        throw new Microsoft.IdentityModel.Tokens.InvalidSecurityTokenException("Invalid issuer.");
      string[] strArray;
      try
      {
        if (dictionary["aud"].GetType().IsArray)
          strArray = Enumerable.Select<object, string>(((IEnumerable) dictionary["aud"]).Cast<object>(), (Func<object, string>) (x => x.ToString())).ToArray<string>();
        else
          strArray = ((string) dictionary["aud"]).Split(',');
      }
      catch (Exception ex)
      {
        throw new FailedRequiredClaimsException("Missing/Invalid audience claim.");
      }
      if (Enumerable.All<string>((IEnumerable<string>) strArray, (Func<string, bool>) (p => string.CompareOrdinal(p, audience) != 0)))
        throw new Microsoft.IdentityModel.Tokens.InvalidSecurityTokenException("Invalid audience.");
      return (IPrincipal) ClaimsPrincipal.CreateFromIdentity((IIdentity) new ClaimsIdentity(Enumerable.Select<KeyValuePair<string, object>, Claim>((IEnumerable<KeyValuePair<string, object>>) dictionary, (Func<KeyValuePair<string, object>, Claim>) (x => new Claim(x.Key, Convert.ToString(x.Value)))), "Signature", "elli_uid", (string) null));
    }

    public IDictionary<string, object> ValidateToken(string token, string audience)
    {
      if (this._validationKey == null)
        this._validationKey = this._keyFactory.GetValidationKey();
      IDictionary<string, object> dictionary = JWT.Decode<IDictionary<string, object>>(token, this._validationKey);
      if (new DateTime(1970, 1, 1).AddSeconds((double) (int) dictionary["exp"]) < DateTime.UtcNow)
        throw new Microsoft.IdentityModel.Tokens.InvalidSecurityTokenException("Token has already expired.");
      if (string.Compare((string) dictionary["iss"], "urn:elli:service:ids", StringComparison.InvariantCultureIgnoreCase) != 0)
        throw new Microsoft.IdentityModel.Tokens.InvalidSecurityTokenException("Invalid issuer.");
      string[] strArray;
      try
      {
        if (dictionary["aud"].GetType().IsArray)
          strArray = Enumerable.Select<object, string>(((IEnumerable) dictionary["aud"]).Cast<object>(), (Func<object, string>) (x => x.ToString())).ToArray<string>();
        else
          strArray = ((string) dictionary["aud"]).Split(',');
      }
      catch (Exception ex)
      {
        throw new FailedRequiredClaimsException("Missing/Invalid audience claim.");
      }
      if (Enumerable.All<string>((IEnumerable<string>) strArray, (Func<string, bool>) (p => string.CompareOrdinal(p, audience) != 0)))
        throw new Microsoft.IdentityModel.Tokens.InvalidSecurityTokenException("Invalid audience.");
      return dictionary;
    }

    public DateTime TokenExpires(IDictionary<string, object> claims)
    {
      return DateTimeUtils.FromEpochTime((int) claims["exp"]);
    }

    private JwsAlgorithm GetAlgorithm(KeyAlgorithm alg)
    {
      switch (alg)
      {
        case KeyAlgorithm.RS256:
          return JwsAlgorithm.RS256;
        case KeyAlgorithm.RS512:
          return JwsAlgorithm.RS512;
        case KeyAlgorithm.HS256:
          return JwsAlgorithm.HS256;
        case KeyAlgorithm.HS512:
          return JwsAlgorithm.HS512;
        default:
          throw new ArgumentOutOfRangeException(nameof (alg), (object) alg, (string) null);
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Elli.Identity.TokenManager
// Assembly: Elli.Identity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0ECC974-8E0E-42B3-88D3-2EAE5F37B212
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Identity.dll

using Elli.Identity.Key;
using Elli.Identity.Provider;
using Jose;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Principal;

#nullable disable
namespace Elli.Identity
{
  public class TokenManager
  {
    private ITokenProvider _tokenProvider;
    private readonly ITokenKeyFactory _keyFactory;
    private readonly string OAPI_TOKEN_ISSUER = "urn:elli:service:ids";

    public TokenManager(ITokenProvider tokenProvider, ITokenKeyFactory keyFactory)
    {
      this._keyFactory = keyFactory;
    }

    public IPrincipal ValidateToken(ElliToken token, string serviceName)
    {
      this._tokenProvider = this.GetIssuer(token.Token);
      return this._tokenProvider.ValidateToken(token, serviceName);
    }

    private ITokenProvider GetIssuer(string token)
    {
      IDictionary<string, object> dictionary = JWT.Decode<IDictionary<string, object>>(token);
      if (!dictionary.ContainsKey("iss"))
        throw new InvalidSecurityTokenException("Invalid issuer");
      return string.Compare((string) dictionary["iss"], this.OAPI_TOKEN_ISSUER, StringComparison.InvariantCultureIgnoreCase) == 0 ? (ITokenProvider) new ElliOapiJwtTokenProvider(this._keyFactory) : (ITokenProvider) new ElliJwtTokenProvider(this._keyFactory);
    }
  }
}

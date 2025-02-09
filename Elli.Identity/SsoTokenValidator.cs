// Decompiled with JetBrains decompiler
// Type: Elli.Identity.SsoTokenValidator
// Assembly: Elli.Identity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0ECC974-8E0E-42B3-88D3-2EAE5F37B212
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Identity.dll

using Elli.Identity.Key;
using Elli.Identity.Provider;
using Elli.Identity.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;

#nullable disable
namespace Elli.Identity
{
  public class SsoTokenValidator : ITokenValidator
  {
    private ITokenProvider _tokenProvider;
    private readonly ITokenKeyFactory _keyFactory;

    public SsoTokenValidator(ITokenProvider tokenProvider)
      : this()
    {
      this._tokenProvider = tokenProvider;
    }

    public SsoTokenValidator()
    {
      if (this._keyFactory != null)
        return;
      this._keyFactory = (ITokenKeyFactory) new X509StoreKeyFactory();
    }

    public SsoTokenValidator(ITokenKeyFactory keyFactory) => this._keyFactory = keyFactory;

    public ElliIdentity Validate(string token, string serviceName)
    {
      this._tokenProvider = JWTUtils.GetTokenProvider(token, this._keyFactory);
      IDictionary<string, object> claims = this._tokenProvider.ValidateToken(token, serviceName);
      object clientId = (object) "";
      object instanceId = (object) "";
      object userId;
      if (!(this._tokenProvider is ElliOapiJwtTokenProvider))
      {
        if (!claims.TryGetValue("elli_uid", out userId) || !claims.TryGetValue("elli_inst", out instanceId) || !claims.TryGetValue("elli_cid", out clientId))
        {
          Debug.Write("Missing ElliUserId, ElliInstance, Expiration or ClientId claims.");
          throw new IdentityNotMappedException("Invalid or missing claims.");
        }
      }
      else if (!claims.TryGetValue("elli_uid", out userId))
      {
        Debug.Write("Missing ElliUserId claim.");
        throw new IdentityNotMappedException("Invalid or missing claims.");
      }
      object issuer;
      if (!claims.TryGetValue("iss", out issuer))
        issuer = (object) "";
      DateTime dateTime = this._tokenProvider.TokenExpires(claims);
      return new ElliIdentity((string) userId, (string) instanceId, (string) clientId, "Signature", true, (string) issuer, new DateTime?(dateTime), claims);
    }
  }
}

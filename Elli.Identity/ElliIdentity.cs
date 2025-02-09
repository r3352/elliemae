// Decompiled with JetBrains decompiler
// Type: Elli.Identity.ElliIdentity
// Assembly: Elli.Identity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0ECC974-8E0E-42B3-88D3-2EAE5F37B212
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Identity.dll

using System;
using System.Collections.Generic;
using System.Security.Principal;

#nullable disable
namespace Elli.Identity
{
  public class ElliIdentity : IIdentity
  {
    public ElliIdentity(
      string userId,
      string instanceId,
      string clientId,
      string authenticationType,
      bool isAuthenticated,
      string issuer,
      DateTime? expires,
      IDictionary<string, object> claims)
    {
      this.UserId = userId;
      this.InstanceId = instanceId;
      this.AuthenticationType = authenticationType;
      this.IsAuthenticated = isAuthenticated;
      this.Issuer = issuer;
      this.Claims = claims;
      this.ClientId = clientId;
      this.ExpiresUtc = expires;
      this.Name = ElliIdentity.GetEncompassName(this.InstanceId, this.UserId);
    }

    public string Name { get; private set; }

    public string UserId { get; private set; }

    public string InstanceId { get; private set; }

    public string ClientId { get; private set; }

    public string AuthenticationType { get; private set; }

    public bool IsAuthenticated { get; private set; }

    public string Issuer { get; private set; }

    public DateTime? ExpiresUtc { get; private set; }

    public IDictionary<string, object> Claims { get; private set; }

    public static string GetEncompassName(string userId, string instanceId)
    {
      return string.IsNullOrWhiteSpace(instanceId) ? "Encompass\\" + userId : string.Format("Encompass\\{0}\\{1}", (object) instanceId, (object) userId);
    }
  }
}

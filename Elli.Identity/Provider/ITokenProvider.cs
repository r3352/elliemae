// Decompiled with JetBrains decompiler
// Type: Elli.Identity.Provider.ITokenProvider
// Assembly: Elli.Identity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0ECC974-8E0E-42B3-88D3-2EAE5F37B212
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Identity.dll

using System;
using System.Collections.Generic;
using System.Security.Principal;

#nullable disable
namespace Elli.Identity.Provider
{
  public interface ITokenProvider
  {
    ElliToken IssueToken(IEnumerable<ElliClaim> claims, string audience, DateTime expiration);

    IPrincipal ValidateToken(ElliToken token, string audience);

    ElliToken IssueToken(IDictionary<string, object> claims, string audience, DateTime expiration);

    IDictionary<string, object> ValidateToken(string token, string audience);

    DateTime TokenExpires(IDictionary<string, object> claims);

    ElliToken IssueToken(
      IDictionary<string, object> claims,
      IEnumerable<string> serviceAudience,
      DateTime expiration);
  }
}

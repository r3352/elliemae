// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerCommon.ApiScopes
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.RemotingServices;
using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerCommon
{
  public static class ApiScopes
  {
    public static string GetApiPlatform(Scope apiScope)
    {
      switch (apiScope)
      {
        case Scope.apiplatform:
          return "API Platform";
        case Scope.cc:
          return "Consumer Connect";
        case Scope.ccap:
          return "Consumer Connect: Admin Portal";
        case Scope.ccbp:
          return "Consumer Connect: Borrower Portal";
        case Scope.crm:
          return "Customer Relationship Management";
        case Scope.dc:
          return "Developer Connect";
        case Scope.dc2:
          return "Data Connect";
        case Scope.email:
          return "Email Address Access";
        case Scope.lc:
          return "Loan Connect";
        case Scope.loc:
          return "Loan Officer Connect";
        case Scope.lp:
          return "Lending Platform";
        case Scope.mfa:
          return "Multi-factor Authentication";
        case Scope.pc:
          return "Partner Connect";
        case Scope.pcapi:
          return "Partner Connect: API";
        case Scope.pcpp:
          return "Partner Connect: Partner Portal";
        case Scope.pcwebhook:
          return "Partner Connect: Webhook";
        case Scope.profile:
          return "Profile Access";
        case Scope.rc:
          return "Resource Center";
        case Scope.tpoc:
          return "TPO Connect";
        case Scope.ef:
          return "eFolder";
        case Scope.epps:
          return "EPPS";
        default:
          return "Other";
      }
    }

    public static string GetAppName(UserInfo _userInfo)
    {
      Scope result = Scope.none;
      string empty = string.Empty;
      if (_userInfo.BorrowerContext != null && _userInfo.BorrowerContext.Claims != null)
      {
        ClaimCollection claims = _userInfo.BorrowerContext.Claims;
        if (((IEnumerable<Claim>) claims).Where<Claim>((Func<Claim, bool>) (x => x.ClaimType == "scope")).Count<Claim>() > 0)
          empty = ((IEnumerable<Claim>) claims).Where<Claim>((Func<Claim, bool>) (x => x.ClaimType == "scope")).Select<Claim, string>((Func<Claim, string>) (x => x.Value)).First<string>().ToString();
        Enum.TryParse<Scope>(empty, out result);
      }
      return ApiScopes.GetApiPlatform(result);
    }
  }
}

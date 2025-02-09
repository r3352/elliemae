// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.SecurityManagerUtil
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using System.Diagnostics;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class SecurityManagerUtil
  {
    private const string className = "SecurityManager";

    public static void Demand(bool isValid)
    {
      if (isValid)
        return;
      Err.Raise(TraceLevel.Warning, "SecurityManager", (ServerException) new SecurityException("Access Denied"));
    }

    public static void DemandLoanRights(UserInfo userInfo, Loan loan, LoanInfo.Right rights)
    {
      SecurityManagerUtil.Demand(SecurityManagerUtil.HasLoanRights(userInfo, loan, rights));
    }

    public static void DemandFeatureAccess(
      UserInfo userInfo,
      IFeaturesAclManager mngr,
      AclFeature feature)
    {
      SecurityManagerUtil.Demand(SecurityManagerUtil.HasFeatureAccess(userInfo, mngr, feature));
    }

    public static bool HasLoanRights(UserInfo userInfo, Loan loan, LoanInfo.Right rights)
    {
      return rights == LoanInfo.Right.Read ? SecurityManagerUtil.GetEffectiveLoanRights(userInfo, loan) != 0 : (SecurityManagerUtil.GetEffectiveLoanRights(userInfo, loan) & rights) == rights;
    }

    public static LoanInfo.Right GetEffectiveLoanRights(UserInfo userInfo, Loan loan)
    {
      return UserInfo.IsSuperAdministrator(userInfo.Userid, userInfo.UserPersonas) || userInfo.Userid == "(trusted)" ? LoanInfo.Right.FullRight : Pipeline.GetEffectiveLoanAccessRights(loan.Identity.Guid, userInfo);
    }

    public static bool HasFeatureAccess(
      UserInfo userInfo,
      IFeaturesAclManager mngr,
      AclFeature feature)
    {
      return UserInfo.IsSuperAdministrator(userInfo.Userid, userInfo.UserPersonas) || mngr.CheckPermission(feature, userInfo);
    }

    public static bool IsRootAdministrator(UserInfo userInfo)
    {
      return userInfo.IsAdministrator() && OrganizationStore.RootOrganizationID == userInfo.OrgId;
    }
  }
}

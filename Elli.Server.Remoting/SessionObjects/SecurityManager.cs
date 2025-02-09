// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.SecurityManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using System.Diagnostics;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class SecurityManager : SessionBoundObject, ISecurityManager
  {
    private const string className = "SecurityManager";
    private UserInfo cachedInfo;

    public SecurityManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (SecurityManager).ToLower());
      this.RefreshUser();
      return this;
    }

    public virtual void Demand(bool isValid)
    {
      if (isValid)
        return;
      Err.Raise(TraceLevel.Warning, nameof (SecurityManager), (ServerException) new SecurityException("Access Denied", this.Session.SessionInfo));
    }

    public virtual bool IsTrustedUser() => this.getUser().Userid == "(trusted)";

    public virtual bool IsRootAdministrator()
    {
      return SecurityManagerUtil.IsRootAdministrator(this.getUser());
    }

    public virtual bool IsSuperAdministrator()
    {
      UserInfo user = this.getUser();
      return UserInfo.IsSuperAdministrator(user.Userid, user.UserPersonas);
    }

    public virtual bool IsAdministrator() => this.getUser().IsAdministrator();

    public virtual bool IsAdministrator(int orgId)
    {
      UserInfo user = this.getUser();
      if (!user.IsAdministrator())
        return false;
      return orgId == user.OrgId || OrganizationStore.IsDescendentOfOrg(user.OrgId, orgId);
    }

    public virtual bool IsSupervisor(UserInfo userInfo)
    {
      return this.IsRootAdministrator() || this.IsAdministrator() && this.getUser().OrgId == userInfo.OrgId || OrganizationStore.IsDescendentOfOrg(this.getUser().OrgId, userInfo.OrgId);
    }

    public virtual void DemandSupervisor(UserInfo userInfo)
    {
      this.Demand(this.IsSupervisor(userInfo));
    }

    public virtual void DemandRootAdministrator() => this.Demand(this.IsRootAdministrator());

    public virtual void DemandSuperAdministrator() => this.Demand(this.IsSuperAdministrator());

    public virtual void DemandAdministrator() => this.Demand(this.IsAdministrator());

    public virtual void DemandAdministrator(int orgId) => this.Demand(this.IsAdministrator(orgId));

    public virtual bool HasFeatureAccess(AclFeature feature)
    {
      return SecurityManagerUtil.HasFeatureAccess(this.getUser(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), feature);
    }

    public virtual void DemandFeatureAccess(AclFeature feature)
    {
      this.Demand(this.HasFeatureAccess(feature));
    }

    public virtual bool HasLoanRights(Loan loan, LoanInfo.Right rights)
    {
      return this.HasLoanRights(this.getUser(), loan, rights);
    }

    public virtual bool HasLoanRights(UserInfo userInfo, Loan loan, LoanInfo.Right rights)
    {
      return SecurityManagerUtil.HasLoanRights(userInfo, loan, rights);
    }

    public virtual void DemandLoanRights(Loan loan, LoanInfo.Right rights)
    {
      this.Demand(this.HasLoanRights(loan, rights));
    }

    public virtual LoanInfo.Right GetEffectiveLoanRights(Loan loan)
    {
      return this.GetEffectiveLoanRights(this.getUser(), loan);
    }

    public virtual LoanInfo.Right GetEffectiveLoanRights(UserInfo userInfo, Loan loan)
    {
      return SecurityManagerUtil.GetEffectiveLoanRights(userInfo, loan);
    }

    public virtual bool HasContactRights(IContact contact) => true;

    public virtual void DemandContactRights(IContact contact)
    {
      this.Demand(this.HasContactRights(contact));
    }

    public virtual bool IsAssignable() => true;

    public virtual void DemandAssignable() => this.Demand(this.IsAssignable());

    public virtual void RefreshUser()
    {
      lock (this)
      {
        this.cachedInfo = (UserInfo) null;
        if (this.Session.Context.Settings.CacheSetting != CacheSetting.Disabled)
          return;
        this.cachedInfo = UserStore.GetLatestVersion(this.Session.UserID).UserInfo;
      }
    }

    private UserInfo getUser()
    {
      lock (this)
      {
        if (this.cachedInfo != (UserInfo) null)
          return this.cachedInfo;
      }
      return UserStore.GetLatestVersion(this.Session.UserID).UserInfo;
    }
  }
}

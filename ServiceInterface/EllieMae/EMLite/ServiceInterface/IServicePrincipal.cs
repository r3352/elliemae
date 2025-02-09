// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ServiceInterface.IServicePrincipal
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;

#nullable disable
namespace EllieMae.EMLite.ServiceInterface
{
  public interface IServicePrincipal : IContextBoundObject, IPrincipal
  {
    UserIdentity Identity { get; }

    UserInfo GetUserInfo();

    UserInfo GetAuditUserInfo();

    bool IsSuperAdministrator();

    bool IsAdministrator();

    bool SkipPersonaChecks { get; set; }

    Hashtable CheckPermissions(AclFeature[] features);

    Hashtable CheckPermissions(AclFeature[] features, bool IsForDRSClaims);

    Dictionary<AclFeature, int> CheckConfigPermissions(AclFeature[] features);

    Dictionary<string, bool> GetMilestonePermissions(AclMilestone milestoneAcl);

    Dictionary<string, bool> GetExportServicesPermissions();

    Dictionary<string, bool> GetFormsPermissions();

    bool HasBusinessRuleAccessPermission(BizRuleType businessRuleType);
  }
}

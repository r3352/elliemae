// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.ACL
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using System;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class ACL
  {
    private Sessions.Session session;

    internal ACL(Sessions.Session session) => this.session = session;

    public object GetAclManager(AclCategory category)
    {
      switch (category)
      {
        case AclCategory.Features:
          return (object) this.session.FeaturesAclManager;
        case AclCategory.InputForms:
          return (object) this.session.InputFormsAclManager;
        case AclCategory.Milestones:
          return (object) this.session.MilestonesAclManager;
        case AclCategory.LoanFolderMove:
          return (object) this.session.LoanFoldersAclManager;
        case AclCategory.ToolsGrantWriteAccess:
          return (object) this.session.ToolsAclManager;
        case AclCategory.MilestonesFreeRole:
          return (object) this.session.MilestoneFreeRoleAclManager;
        case AclCategory.Services:
          return (object) this.session.ServicesAclManager;
        case AclCategory.FieldAccess:
          return (object) this.session.FieldAccessAclManager;
        case AclCategory.PersonaPipelineView:
          return (object) this.session.PipelineViewAclManager;
        case AclCategory.LoanDuplicationTemplates:
          return (object) this.session.LoanDuplicationAclManager;
        case AclCategory.ExportServices:
          return (object) this.session.ExportServicesAclManager;
        case AclCategory.FeatureConfigs:
          return (object) this.session.FeatureConfigsAclManager;
        case AclCategory.InvestorServices:
          return (object) this.session.InvestorServieAclManager;
        case AclCategory.EnhancedConditions:
          return (object) this.session.EnhancedConditionAclManager;
        case AclCategory.LOConnectCustomServices:
          return (object) this.session.LoConnectServicesAclManager;
        case AclCategory.StandardWebforms:
          return (object) this.session.StandardWebFormAclManager;
        default:
          throw new Exception("Unknown category " + category.ToString());
      }
    }

    public void RefreshAclManagerInstances()
    {
      this.session.ResetFeaturesAclManager();
      this.session.ResetInputFormsAclManager();
      this.session.ResetMilestonesAclManager();
      this.session.ResetLoanFoldersAclManager();
      this.session.ResetToolsAclManager();
      this.session.ResetMilestoneFreeRoleAclManager();
      this.session.ResetServicesAclManager();
      this.session.ResetExportServicesAclManager();
      this.session.ResetFieldAccessAclManager();
      this.session.ResetInvestorServieAclManager();
    }

    public bool IsAuthorizedForFeature(AclFeature feature)
    {
      return this.session.UserInfo.IsSuperAdministrator() || FeaturesAclManager.Instance.GetUserApplicationRight(feature);
    }
  }
}

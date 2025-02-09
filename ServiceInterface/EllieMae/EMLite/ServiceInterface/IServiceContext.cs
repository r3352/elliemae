// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ServiceInterface.IServiceContext
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ServiceInterface.Services;
using System;

#nullable disable
namespace EllieMae.EMLite.ServiceInterface
{
  public interface IServiceContext : IDisposable
  {
    string InstanceName { get; }

    IDataCache RequestCache { get; }

    IServicePrincipal Principal { get; }

    ILoanService LoanService { get; }

    IPipelineService PipelineService { get; }

    IEdmPlatformService EdmPlatformService { get; }

    IFeatureAclService FeatureAclService { get; }

    IDocumentService DocumentService { get; }

    IFileAttachmentService FileAttachmentService { get; }

    IConditionService ConditionService { get; }

    ILoanHistoryService LoanHistoryService { get; }

    ILoanFolderService LoanFolderService { get; }

    IConfigurationSettingService ConfigurationSettingService { get; }

    IServerSettingService ServerSettingService { get; }

    IUserSettingService UserSettingService { get; }

    ITemplateSettingService TemplateSettingService { get; }

    IOrganizationSettingsService OrganizationSettingsService { get; }

    ILocationLookupService LocationLookupService { get; }

    IFieldSearchService FieldSearchService { get; }

    IProductPricingSettingService ProductPricingSettingService { get; }

    ILockComparisonFieldService LockComparisonFieldsService { get; }

    IWorkflowService WorkflowService { get; }

    IExternalCompanyService ExternalCompanyService { get; }

    IAuthenticationService AuthenticationService { get; }

    ISessionService SessionService { get; }

    void ReassignPrincipal(UserIdentity uid);
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ServiceInterface.IPipelineService
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using Elli.Domain.Mortgage;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ServiceInterface
{
  public interface IPipelineService : IContextBoundObject
  {
    IList<PipelineItem> GetPipelineItems(
      IList<Guid> loanGuids,
      IList<string> fields,
      PipelineData pipelineData = PipelineData.Fields);

    List<Guid> GetPipeline(bool isExternalOrganization = false);

    List<Guid> GetPipeline(
      out int totalLoanCount,
      List<PipelineField> fields,
      string loanFolder,
      List<PipelineSortField> sortFields,
      List<FilterCriterion> filter,
      int maxResults,
      bool isExternalOrganization = false,
      Ownership ownership = Ownership.All,
      string externalOrgId = "");

    IEnumerable<PipelineAlertInfo> GetAlertRecords(PipelineInfo pipelineInfo);

    IEnumerable<PipelineMilestoneInfo> GetMilestoneRecords(PipelineInfo pipelineInfo);

    IEnumerable<PipelineInfo> GetPipelineInfo(
      IEnumerable<Guid> loanGuids,
      IEnumerable<string> fieldsRequested,
      PipelineData pipelineData = PipelineData.Fields,
      bool isExternalOrganization = true);

    ReportFieldDefContainer GetPipelineReportFields(bool applySecurity = true, LoanReportFieldFlags flags = LoanReportFieldFlags.DatabaseFieldsNoAudit);

    PipelineView[] GetCustomPipelineViews(string viewName = null, string userId = null);

    PersonaPipelineView[] GetPersonaPipelineViews(string viewName = null, string personaName = null);

    PipelineView[] GetCustomPipelineViews(List<string> viewNames, string persona);

    PersonaPipelineView[] GetPersonaPipelineViews(List<string> viewNames, string persona);

    PersonaPipelineView[] GetPersonaPipelineViews(List<int> personaIds);

    int SaveCustomPipelineViews(PipelineView[] customPipelineViews, bool isCreate, string userId = null);

    int SavePersonaPipelineViews(PersonaPipelineView[] personaPipelineViews);

    string GetPipelineDefaultViewName();

    bool SavePipelineDefaultViewName(PipelineView view);

    bool SavePipelineDefaultViewName(PersonaPipelineView view);

    bool RenamePipelineView(PipelineView oldView, string newViewName);

    bool DeletePipelineView(PipelineView view, string userId = null);

    bool DuplicatePipelineView(object view, string newViewName);

    IList<string> GetLoanMoveToFolders();

    IList<string> GetLoanMoveFromFolders();

    int GetLoanMailboxMessageCountForUser();

    void UpdateLoanAlerts(string guid, IList<PipelineInfo.Alert> loanAlertList);

    AlertConfig GetAlertConfig(string AlertCriterionName);
  }
}

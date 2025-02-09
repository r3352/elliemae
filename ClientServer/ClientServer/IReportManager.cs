// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IReportManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IReportManager
  {
    ReportResults QueryLoansForReport(
      LoanReportParameters reportParams,
      IServerProgressFeedback feedback);

    ReportResults QueryContactsForReport(
      ContactReportParameters reportParams,
      IServerProgressFeedback feedback);

    ReportResults QueryTradesForReport(
      TradeReportParameters reportParams,
      IServerProgressFeedback feedback);

    ReportResults QueryTpoSettingsForReport(
      TpoSettingsReportParameters reportParams,
      IServerProgressFeedback feedback);

    ReportSettings GetReportSettings(FileSystemEntry entry);

    ReportSettings GetReportSettings(FileSystemEntry entry, bool applySecurity);

    Dictionary<FileSystemEntry, ReportSettings> GetFileSystemEntries(
      FileSystemEntry[] fileSystemEntryList);

    void SaveReportSettings(
      Dictionary<FileSystemEntry, ReportSettings> fileSystemEntryList);

    string[] GetMilestoneNamesByReportFilePaths(string[] computedFilePaths);

    bool ReportSettingsObjectExists(FileSystemEntry entry);

    bool ReportSettingsObjectExistsOfAnyType(FileSystemEntry entry);

    void SaveReportSettings(FileSystemEntry entry, ReportSettings settings);

    void DeleteReportSettingsObject(FileSystemEntry entry);

    void MoveReportSettingsObject(FileSystemEntry source, FileSystemEntry target);

    void CreateReportSettingsFolder(FileSystemEntry entry);

    void CopyReportSettingsObject(FileSystemEntry source, FileSystemEntry target);

    FileSystemEntry[] GetReportDirEntries(FileSystemEntry parentEntry);

    FileSystemEntry[] GetAllPublicReportDirEntries();

    FileSystemEntry[] GetFilteredReportDirEntries(FileSystemEntry parentEntry);

    DashboardViewInfo[] GetDashboardViews(DashboardViewCollectionCriteria criteria);

    DashboardViewInfo GetDashboardView(int viewId);

    DashboardViewInfo GetDashboardView(string viewGuid);

    bool CheckDashboardViewExists(string viewGuid);

    DashboardViewInfo SaveDashboardView(DashboardViewInfo viewInfo);

    DashboardViewInfo SyncDashboardView(
      string sourceViewGuid,
      int targetViewID,
      string targetViewName,
      string targetViewGuid);

    DashboardViewInfo SyncDashboardView(
      string sourceViewGuid,
      int targetViewID,
      string targetViewName,
      string targetViewGuid,
      string targetTemplatePath);

    DashboardViewInfo[] GetReferencedDashboardViews(
      string snapshotPath,
      bool publicDashboardViewOnly);

    void SyncDashboardView(FileSystemEntry source, FileSystemEntry target);

    void DeleteDashboardView(int viewId);

    bool IsTemplateReferenced(string templatePath);

    bool IsTemplateReferencedByGlobalView(string templatePath);

    DataTable QueryDataForDashboardReport(
      DashboardDataCriteria dashboardReportCriteria,
      bool isExternalOrganization,
      bool excludeArchiveLoans = false);

    string[] GetDashboardSnapshotPathsByViewTemplatePaths(FileSystemEntry[] fileSystemEntries);

    void AddToRecentReports(FileSystemEntry fsEntry);

    FileSystemEntry[] GetRecentReports(int count);

    MilestoneStatistics[] GetMilestoneStatistics(
      QueryCriterion loanFilter,
      bool isExternalOrganization);

    MilestoneStatistics[] GetMilestoneStatistics(
      QueryCriterion loanFilter,
      bool isExternalOrganization,
      bool useReadReplica);

    UserLoanStatistics[] GetLOStatistics(
      QueryCriterion loanFilter,
      Range<DateTime> dateRange,
      bool isExternalOrganization);

    UserLoanStatistics[] GetLOStatistics(
      QueryCriterion loanFilter,
      Range<DateTime> dateRange,
      bool isExternalOrganization,
      bool useReadReplica);

    string GetSettingsRptXML(string userId, SettingsRptJobInfo jobInfo);

    SettingsRptJobInfo GetSettingsRptInfo(string reportID);

    Dictionary<string, string> GetSettingsReportParameters(string reportID);

    List<string> GetReportFilters(string reportID);

    SettingsRptJobInfo[] GetSettingsRptJobs(string jobstatus);

    SettingsRptJobInfo[] GetFilteredSettingsRptJobs(
      string reportName,
      string reportType,
      string createdBy,
      string sortBy,
      int pageIdx,
      out int total,
      int pageSize);

    void DeleteSettingsRptJobs(string reportID, string deleteBy);

    bool CancelSettingsRptJobs(string reportID, string cancelBy, bool cancelComplete);

    NMLSReportData GetNMLSReportData(int year, int quarter, string state, string appSource);

    NMLSReportData[] GetNMLSReportData(int year, int quarter);

    NMLSReportData[] GetNMLSReportData(int year, int quarter, string state);

    NMLSReportData[] GetNMLSReportData(string state);

    void SaveNMLSReportData(NMLSReportData data);

    void SaveNMLSReportData(NMLSReportData[] data);
  }
}

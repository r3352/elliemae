// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.ReportManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.Interception;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class ReportManager : SessionBoundObject, IReportManager
  {
    private const string className = "ReportManager";
    private static string sw = Tracing.SwReportControl;
    private ReportEngine engine;

    public ReportManager Initialize(ISession session)
    {
      this.engine = new ReportEngine(session.UserID);
      this.InitializeInternal(session, nameof (ReportManager).ToLower());
      return this;
    }

    public virtual ReportResults QueryLoansForReport(
      LoanReportParameters reportParams,
      IServerProgressFeedback feedback)
    {
      this.onApiCalled(nameof (ReportManager), nameof (QueryLoansForReport), new object[1]
      {
        (object) reportParams
      });
      try
      {
        return new LoanReportGenerator(this.Session.GetUserInfo(), reportParams, feedback).Generate();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex);
        return (ReportResults) null;
      }
    }

    public virtual FileSystemEntry[] GetAllPublicReportDirEntries()
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetAllPublicReportDirEntries), Array.Empty<object>());
      try
      {
        return ReportSettingsStore.GetAllPublicSystemEntries();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual ReportResults QueryContactsForReport(
      ContactReportParameters reportParams,
      IServerProgressFeedback feedback)
    {
      this.onApiCalled(nameof (ReportManager), nameof (QueryContactsForReport), new object[1]
      {
        (object) reportParams
      });
      try
      {
        return new ContactReportGenerator(this.Session.GetUserInfo(), reportParams, feedback).Generate();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (ReportResults) null;
      }
    }

    public virtual ReportResults QueryTradesForReport(
      TradeReportParameters reportParams,
      IServerProgressFeedback feedback)
    {
      this.onApiCalled(nameof (ReportManager), nameof (QueryTradesForReport), new object[1]
      {
        (object) reportParams
      });
      try
      {
        return new TradeReportGenerator(this.Session.GetUserInfo(), reportParams, feedback).Generate();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (ReportResults) null;
      }
    }

    public virtual ReportResults QueryTpoSettingsForReport(
      TpoSettingsReportParameters reportParams,
      IServerProgressFeedback feedback)
    {
      this.onApiCalled(nameof (ReportManager), nameof (QueryTpoSettingsForReport), new object[1]
      {
        (object) reportParams
      });
      try
      {
        return new TpoSettingsReportGenerator(this.Session.GetUserInfo(), reportParams, feedback).Generate();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (ReportResults) null;
      }
    }

    public virtual void MoveReportSettingsObject(FileSystemEntry source, FileSystemEntry target)
    {
      this.onApiCalled(nameof (ReportManager), nameof (MoveReportSettingsObject), new object[2]
      {
        (object) source,
        (object) target
      });
      if (source == null)
        Err.Raise(TraceLevel.Warning, nameof (ReportManager), (ServerException) new ServerArgumentException("Report folder/file cannot be blank or null", nameof (source), this.Session.SessionInfo));
      if (target == null)
        Err.Raise(TraceLevel.Warning, nameof (ReportManager), (ServerException) new ServerArgumentException("Report folder/file cannot be blank or null", nameof (target), this.Session.SessionInfo));
      if (source.Type == FileSystemEntry.Types.Folder && target.Type == FileSystemEntry.Types.File)
        Err.Raise(TraceLevel.Warning, nameof (ReportManager), (ServerException) new ServerArgumentException("Target of copy must be folder is source is folder must have same type", nameof (target), this.Session.SessionInfo));
      try
      {
        ReportSettingsAccessor.MoveReportSettingsObject(source, target, this.Session.UserID, this.Session.GetUserInfo().FullName);
        TraceLog.WriteInfo(nameof (ReportManager), this.formatMsg("Report \"" + (object) source + "\" moved to " + (object) target));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual Dictionary<FileSystemEntry, ReportSettings> GetFileSystemEntries(
      FileSystemEntry[] fileSystemEntryList)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetFileSystemEntries), Array.Empty<object>());
      try
      {
        return ReportSettingsAccessor.GetFileSystemEntries(fileSystemEntryList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (Dictionary<FileSystemEntry, ReportSettings>) null;
      }
    }

    public virtual ReportSettings GetReportSettings(FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetReportSettings), new object[1]
      {
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ReportManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        return ReportSettingsAccessor.GetReportSettings(entry);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (ReportSettings) null;
      }
    }

    public virtual ReportSettings GetReportSettings(FileSystemEntry entry, bool applySecurity)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetReportSettings), new object[2]
      {
        (object) entry,
        (object) applySecurity
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ReportManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        if (AclGroupFileAccessor.GetUserFileFolderAccess(AclFileType.Reports, entry, this.Session.UserID) == AclResourceAccess.None)
          throw new SecurityException("Access Denied");
        using (ReportSettingsFile reportSettingsFile = ReportSettingsStore.CheckOut(entry))
          return reportSettingsFile.Exists ? reportSettingsFile.Settings : (ReportSettings) null;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (ReportSettings) null;
      }
    }

    public virtual bool ReportSettingsObjectExists(FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ReportManager), nameof (ReportSettingsObjectExists), new object[1]
      {
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ReportManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        return ReportSettingsStore.Exists(entry);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool ReportSettingsObjectExistsOfAnyType(FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ReportManager), nameof (ReportSettingsObjectExistsOfAnyType), new object[1]
      {
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ReportManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        return ReportSettingsStore.ExistsOfAnyType(entry);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual void SaveReportSettings(FileSystemEntry entry, ReportSettings settings)
    {
      this.onApiCalled(nameof (ReportManager), nameof (SaveReportSettings), new object[2]
      {
        (object) entry,
        (object) settings
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ReportManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        ReportSettingsAccessor.SaveReportSettings(entry, settings, this.Session.UserID, this.Session.GetUserInfo().FullName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SaveReportSettings(
      Dictionary<FileSystemEntry, ReportSettings> fileSystemEntryList)
    {
      this.onApiCalled(nameof (ReportManager), nameof (SaveReportSettings), Array.Empty<object>());
      try
      {
        ReportSettingsAccessor.SaveReportSettings(fileSystemEntryList, this.Session.UserID, this.Session.GetUserInfo().FullName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string[] GetMilestoneNamesByReportFilePaths(string[] computedFilePaths)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetMilestoneNamesByReportFilePaths), (object[]) computedFilePaths);
      if (computedFilePaths == null)
        return (string[]) null;
      try
      {
        return ReportSettingsAccessor.GetMilestoneNamesByReportFilePaths(computedFilePaths);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual void DeleteReportSettingsObject(FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ReportManager), nameof (DeleteReportSettingsObject), new object[1]
      {
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ReportManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        ReportSettingsAccessor.DeleteReportSettingsObject(entry, this.Session.UserID, this.Session.GetUserInfo().FullName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void CreateReportSettingsFolder(FileSystemEntry entry)
    {
      this.onApiCalled(nameof (ReportManager), nameof (CreateReportSettingsFolder), new object[1]
      {
        (object) entry
      });
      if (entry == null)
        Err.Raise(TraceLevel.Warning, nameof (ReportManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (entry), this.Session.SessionInfo));
      try
      {
        ReportSettingsStore.CreateFolder(entry);
        TraceLog.WriteInfo(nameof (ReportManager), this.formatMsg("Report folder \"" + (object) entry + "\" created"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void CopyReportSettingsObject(FileSystemEntry source, FileSystemEntry target)
    {
      this.onApiCalled(nameof (ReportManager), nameof (CopyReportSettingsObject), new object[2]
      {
        (object) source,
        (object) target
      });
      if (source == null)
        Err.Raise(TraceLevel.Warning, nameof (ReportManager), (ServerException) new ServerArgumentException("Source cannot be blank or null", nameof (source), this.Session.SessionInfo));
      if (target == null)
        Err.Raise(TraceLevel.Warning, nameof (ReportManager), (ServerException) new ServerArgumentException("Target cannot be blank or null", nameof (target), this.Session.SessionInfo));
      if (source.Type == FileSystemEntry.Types.Folder && target.Type == FileSystemEntry.Types.File)
        Err.Raise(TraceLevel.Warning, nameof (ReportManager), (ServerException) new ServerArgumentException("Target of copy must be folder is source is folder must have same type", nameof (target), this.Session.SessionInfo));
      try
      {
        ReportSettingsAccessor.CopyReportSettingsObject(source, target, this.Session.UserID, this.Session.GetUserInfo().FullName);
        TraceLog.WriteInfo(nameof (ReportManager), this.formatMsg("The Report \"" + (object) source + "\" moved to " + (object) target));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual FileSystemEntry[] GetReportDirEntries(FileSystemEntry parentFolder)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetReportDirEntries), new object[1]
      {
        (object) parentFolder
      });
      if (parentFolder == null)
        Err.Raise(TraceLevel.Warning, nameof (ReportManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (parentFolder), this.Session.SessionInfo));
      try
      {
        return ReportSettingsStore.GetDirectoryEntries(parentFolder);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual FileSystemEntry[] GetFilteredReportDirEntries(FileSystemEntry parentFolder)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetFilteredReportDirEntries), new object[1]
      {
        (object) parentFolder
      });
      if (parentFolder == null)
        Err.Raise(TraceLevel.Warning, nameof (ReportManager), (ServerException) new ServerArgumentException("Specified file system entry cannot be null", nameof (parentFolder), this.Session.SessionInfo));
      try
      {
        return ReportSettingsStore.GetDirectoryEntries(this.Session.GetUserInfo(), parentFolder);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual MilestoneStatistics[] GetMilestoneStatistics(
      QueryCriterion filter,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetMilestoneStatistics), new object[1]
      {
        (object) filter
      });
      try
      {
        return this.engine.GetMilestoneStatistics(filter, isExternalOrganization);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (MilestoneStatistics[]) null;
      }
    }

    public virtual MilestoneStatistics[] GetMilestoneStatistics(
      QueryCriterion filter,
      bool isExternalOrganization,
      bool fromHomePage)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetMilestoneStatistics), new object[2]
      {
        (object) filter,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.HomePage)
      });
      try
      {
        return this.engine.GetMilestoneStatistics(filter, isExternalOrganization, fromHomePage);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (MilestoneStatistics[]) null;
      }
    }

    public virtual UserLoanStatistics[] GetLOStatistics(
      QueryCriterion filter,
      Range<DateTime> dateRange,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetLOStatistics), new object[2]
      {
        (object) filter,
        (object) dateRange
      });
      try
      {
        return this.engine.GetLOStatistics(filter, dateRange, isExternalOrganization);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (UserLoanStatistics[]) null;
      }
    }

    public virtual UserLoanStatistics[] GetLOStatistics(
      QueryCriterion filter,
      Range<DateTime> dateRange,
      bool isExternalOrganization,
      bool fromHomePage)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetLOStatistics), new object[3]
      {
        (object) filter,
        (object) dateRange,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.HomePage)
      });
      try
      {
        return this.engine.GetLOStatistics(filter, dateRange, isExternalOrganization, fromHomePage);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (UserLoanStatistics[]) null;
      }
    }

    public virtual DashboardViewInfo[] GetDashboardViews(DashboardViewCollectionCriteria criteria)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetDashboardViews), Array.Empty<object>());
      try
      {
        return EllieMae.EMLite.Server.Dashboard.GetDashboardViews(criteria);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (DashboardViewInfo[]) null;
      }
    }

    public virtual DashboardViewInfo[] GetReferencedDashboardViews(
      string snapshotPath,
      bool publicDashboardViewOnly)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetReferencedDashboardViews), new object[1]
      {
        (object) snapshotPath
      });
      try
      {
        DashboardViewInfo[] referencedDashboardViews = EllieMae.EMLite.Server.Dashboard.GetReferencedDashboardViews(snapshotPath);
        if (!publicDashboardViewOnly)
          return referencedDashboardViews;
        FileSystemEntry[] publicFileEntries = TemplateSettingsStore.GetAllPublicFileEntries(TemplateSettingsType.DashboardViewTemplate, true);
        if (publicFileEntries == null || publicFileEntries.Length == 0)
          return (DashboardViewInfo[]) null;
        ConfigurationManager configurationManager = InterceptionUtils.NewInstance<ConfigurationManager>().Initialize(this.Session);
        this.onApiCalled(nameof (ReportManager), "GetReferencedDashboardViews:Internal call to configuration manager begin", Array.Empty<object>());
        List<DashboardViewInfo> dashboardViewInfoList = new List<DashboardViewInfo>();
        foreach (FileSystemEntry entry in publicFileEntries)
        {
          DashboardViewTemplate templateSettings = (DashboardViewTemplate) configurationManager.GetTemplateSettings(TemplateSettingsType.DashboardViewTemplate, entry);
          foreach (DashboardViewInfo dashboardViewInfo in referencedDashboardViews)
          {
            if (dashboardViewInfo.Guid.ToString() == templateSettings.ViewGuid)
            {
              dashboardViewInfoList.Add(dashboardViewInfo);
              break;
            }
          }
        }
        this.onApiCalled(nameof (ReportManager), "GetReferencedDashboardViews:Internal call to configuration manager end", Array.Empty<object>());
        return dashboardViewInfoList.ToArray();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (DashboardViewInfo[]) null;
      }
    }

    public virtual DashboardViewInfo GetDashboardView(int viewId)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetDashboardView), Array.Empty<object>());
      try
      {
        return EllieMae.EMLite.Server.Dashboard.GetDashboardView(viewId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (DashboardViewInfo) null;
      }
    }

    public virtual DashboardViewInfo GetDashboardView(string viewGuid)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetDashboardView), Array.Empty<object>());
      try
      {
        return EllieMae.EMLite.Server.Dashboard.GetDashboardView(viewGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (DashboardViewInfo) null;
      }
    }

    public virtual bool CheckDashboardViewExists(string templatePath)
    {
      this.onApiCalled(nameof (ReportManager), "GetDashboardView", Array.Empty<object>());
      try
      {
        return EllieMae.EMLite.Server.Dashboard.CheckDashboardViewExists(templatePath);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual DashboardViewInfo SaveDashboardView(DashboardViewInfo viewInfo)
    {
      this.onApiCalled(nameof (ReportManager), nameof (SaveDashboardView), Array.Empty<object>());
      try
      {
        return EllieMae.EMLite.Server.Dashboard.SaveDashboardView(viewInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (DashboardViewInfo) null;
      }
    }

    public virtual DashboardViewInfo SyncDashboardView(
      string sourceViewGuid,
      int targetViewID,
      string targetViewName,
      string targetViewGuid)
    {
      return this.SyncDashboardView(sourceViewGuid, targetViewID, targetViewName, targetViewGuid, string.Empty);
    }

    public virtual DashboardViewInfo SyncDashboardView(
      string sourceViewGuid,
      int targetViewID,
      string targetViewName,
      string targetViewGuid,
      string targetTemplatePath)
    {
      this.onApiCalled(nameof (ReportManager), nameof (SyncDashboardView), new object[4]
      {
        (object) sourceViewGuid,
        (object) targetViewID,
        (object) targetViewName,
        (object) targetViewGuid
      });
      try
      {
        return EllieMae.EMLite.Server.Dashboard.SyncDashboardView(sourceViewGuid, targetViewID, targetViewName, targetViewGuid, targetTemplatePath);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (DashboardViewInfo) null;
      }
    }

    public virtual void SyncDashboardView(FileSystemEntry source, FileSystemEntry target)
    {
      this.onApiCalled(nameof (ReportManager), nameof (SyncDashboardView), new object[2]
      {
        (object) source,
        (object) target
      });
      try
      {
        Dictionary<string, FileSystemEntry> dictionary1 = new Dictionary<string, FileSystemEntry>();
        Dictionary<string, FileSystemEntry> dictionary2 = new Dictionary<string, FileSystemEntry>();
        if (source.Type == FileSystemEntry.Types.File)
        {
          new FileSystemEntry[1][0] = source;
          dictionary1.Add(source.Path.Replace(source.ParentFolder.Path, ""), source);
        }
        else
        {
          foreach (FileSystemEntry directoryEntry in TemplateSettingsStore.GetDirectoryEntries(this.Session.GetUserInfo(), TemplateSettingsType.DashboardViewTemplate, source, FileSystemEntry.Types.File, true, true))
          {
            if (directoryEntry.Path == source.Path)
              dictionary1.Add(directoryEntry.Path, directoryEntry);
            else
              dictionary1.Add(directoryEntry.Path.Replace(source.Path, ""), directoryEntry);
          }
        }
        if (target.Type == FileSystemEntry.Types.File)
        {
          new FileSystemEntry[1][0] = target;
          dictionary2.Add(source.Path.Replace(source.ParentFolder.Path, ""), target);
        }
        else
        {
          foreach (FileSystemEntry directoryEntry in TemplateSettingsStore.GetDirectoryEntries(this.Session.GetUserInfo(), TemplateSettingsType.DashboardViewTemplate, target, FileSystemEntry.Types.File, true, true))
          {
            if (directoryEntry.Path == target.Path)
              dictionary2.Add(target.Path, directoryEntry);
            else
              dictionary2.Add(directoryEntry.Path.Replace(target.Path, ""), directoryEntry);
          }
        }
        int num = 0;
        this.onApiCalled(nameof (ReportManager), "SyncDashboardView: Internal calls to ConfigurationManager", Array.Empty<object>());
        ConfigurationManager configurationManager = InterceptionUtils.NewInstance<ConfigurationManager>().Initialize(this.Session);
        foreach (string key in dictionary2.Keys)
        {
          if (dictionary1.ContainsKey(key))
          {
            DashboardViewTemplate templateSettings = (DashboardViewTemplate) configurationManager.GetTemplateSettings(TemplateSettingsType.DashboardViewTemplate, dictionary1[key]);
            if (templateSettings != null)
            {
              string viewGuid = templateSettings.ViewGuid;
              string name = dictionary2[key].Name;
              string guid = Guid.NewGuid().ToString();
              DashboardViewTemplate data = new DashboardViewTemplate(guid, name);
              configurationManager.SaveTemplateSettings(TemplateSettingsType.DashboardViewTemplate, dictionary2[key], (BinaryObject) (BinaryConvertibleObject) data);
              int targetViewID = num;
              string targetViewName = name;
              string targetViewGuid = guid;
              string targetTemplatePath = dictionary2[key].ToString();
              EllieMae.EMLite.Server.Dashboard.SyncDashboardView(viewGuid, targetViewID, targetViewName, targetViewGuid, targetTemplatePath);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteDashboardView(int viewId)
    {
      this.onApiCalled(nameof (ReportManager), nameof (DeleteDashboardView), Array.Empty<object>());
      try
      {
        EllieMae.EMLite.Server.Dashboard.DeleteDashboardView(viewId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool IsTemplateReferenced(string templatePath)
    {
      this.onApiCalled(nameof (ReportManager), nameof (IsTemplateReferenced), Array.Empty<object>());
      try
      {
        return EllieMae.EMLite.Server.Dashboard.IsTemplateReferenced(templatePath);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return true;
      }
    }

    public virtual bool IsTemplateReferencedByGlobalView(string templatePath)
    {
      this.onApiCalled(nameof (ReportManager), nameof (IsTemplateReferencedByGlobalView), Array.Empty<object>());
      try
      {
        return EllieMae.EMLite.Server.Dashboard.IsTemplateReferencedByGlobalView(templatePath);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return true;
      }
    }

    public virtual DataTable QueryDataForDashboardReport(
      DashboardDataCriteria dashboardReportCriteria,
      bool isExternalOrganization,
      bool excludeArchiveLoans = false)
    {
      this.onApiCalled(nameof (ReportManager), nameof (QueryDataForDashboardReport), new object[1]
      {
        (object) dashboardReportCriteria
      });
      try
      {
        dashboardReportCriteria.CurrentUser = this.Session.GetUserInfo();
        DataTable dataTable = EllieMae.EMLite.Server.Dashboard.QueryDataForDashboardReport(dashboardReportCriteria, isExternalOrganization, excludeArchiveLoans);
        if (dataTable != null)
        {
          foreach (DataColumn column in (InternalDataCollectionBase) dataTable.Columns)
          {
            if (column.DataType == typeof (DateTime) && column.DateTimeMode == DataSetDateTime.UnspecifiedLocal)
              column.DateTimeMode = DataSetDateTime.Unspecified;
          }
        }
        return dataTable;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex);
        return (DataTable) null;
      }
    }

    public virtual string[] GetDashboardSnapshotPathsByViewTemplatePaths(
      FileSystemEntry[] fileSystemEntries)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetDashboardSnapshotPathsByViewTemplatePaths), (object[]) fileSystemEntries);
      try
      {
        return EllieMae.EMLite.Server.Dashboard.GetDashboardSnapshotPathsByViewTemplatePaths(fileSystemEntries);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex);
        return (string[]) null;
      }
    }

    public virtual void AddToRecentReports(FileSystemEntry fsEntry)
    {
      this.onApiCalled(nameof (ReportManager), nameof (AddToRecentReports), new object[1]
      {
        (object) fsEntry
      });
      try
      {
        using (User latestVersion = UserStore.GetLatestVersion(this.Session.UserID))
          latestVersion.AddToRecentReports(fsEntry);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual FileSystemEntry[] GetRecentReports(int count)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetRecentReports), new object[1]
      {
        (object) count
      });
      try
      {
        using (User latestVersion = UserStore.GetLatestVersion(this.Session.UserID))
          return latestVersion.GetRecentReports(count);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual string GetSettingsRptXML(string userId, SettingsRptJobInfo jobInfo)
    {
      this.onApiCalled(nameof (ReportManager), "GetXMLSettingsRpt", new object[1]
      {
        (object) userId
      });
      try
      {
        using (User user = UserStore.CheckOut(userId))
        {
          if (!user.Exists)
            Err.Raise(TraceLevel.Warning, nameof (ReportManager), (ServerException) new ObjectNotFoundException("The specified user does not exist", ObjectType.User, (object) userId));
          return SettingsReportAccessor.getXMLSettingsRpt(jobInfo);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (string) null;
      }
    }

    public virtual Dictionary<string, string> GetSettingsReportParameters(string reportID)
    {
      this.onApiCalled(nameof (ReportManager), "GetXMLSettingsReportParameters", new object[1]
      {
        (object) reportID
      });
      try
      {
        return SettingsReportAccessor.getReportParameters((int) Convert.ToInt16(reportID));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (Dictionary<string, string>) null;
      }
    }

    public virtual SettingsRptJobInfo GetSettingsRptInfo(string reportID)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetSettingsRptInfo), new object[1]
      {
        (object) reportID
      });
      try
      {
        return SettingsReportAccessor.getSettingsRptInfo(reportID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (SettingsRptJobInfo) null;
      }
    }

    public virtual SettingsRptJobInfo[] GetSettingsRptJobs(string jobstatus)
    {
      this.onApiCalled(nameof (ReportManager), "GetSettingsRptInfo", new object[1]
      {
        (object) "ALL"
      });
      try
      {
        return SettingsReportAccessor.GetSettingsRptJobs(jobstatus);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (SettingsRptJobInfo[]) null;
      }
    }

    public virtual Dictionary<string, string> GetReportParameters(string reportID)
    {
      int int16 = (int) Convert.ToInt16(reportID);
      this.onApiCalled(nameof (ReportManager), nameof (GetReportParameters), new object[1]
      {
        (object) reportID
      });
      try
      {
        return SettingsReportAccessor.getReportParameters(int16);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (Dictionary<string, string>) null;
      }
    }

    public virtual List<string> GetReportFilters(string reportID)
    {
      int int16 = (int) Convert.ToInt16(reportID);
      this.onApiCalled(nameof (ReportManager), nameof (GetReportFilters), new object[1]
      {
        (object) reportID
      });
      try
      {
        return SettingsReportAccessor.getReportFilters(int16);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (List<string>) null;
      }
    }

    public virtual void DeleteSettingsRptJobs(string reportID, string deleteBy)
    {
      this.onApiCalled(nameof (ReportManager), "DeleteSettingsRpt", new object[1]
      {
        (object) "ALL"
      });
      try
      {
        SettingsReportAccessor.DeleteSettingsRptJobs(reportID, deleteBy);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool CancelSettingsRptJobs(
      string reportID,
      string cancelBy,
      bool cancelcomplete)
    {
      this.onApiCalled(nameof (ReportManager), "CancelSettingsRpt", new object[1]
      {
        (object) "ALL"
      });
      try
      {
        return SettingsReportAccessor.CancelSettingsRptJobs(reportID, cancelBy, cancelcomplete);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
      }
      return false;
    }

    public virtual SettingsRptJobInfo[] GetFilteredSettingsRptJobs(
      string reportName,
      string reportType,
      string createdBy,
      string sortBy,
      int pageIdx,
      out int total,
      int pageSize)
    {
      this.onApiCalled(nameof (ReportManager), "GetSettingsRptInfo", new object[1]
      {
        (object) "ALL"
      });
      try
      {
        return SettingsReportAccessor.GetFilteredSettingsRptJobs(reportName, reportType, createdBy, sortBy, pageIdx, out total, pageSize);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        total = 0;
        return (SettingsRptJobInfo[]) null;
      }
    }

    public virtual NMLSReportData[] GetNMLSReportData(int year, int quarter)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetNMLSReportData), new object[2]
      {
        (object) year,
        (object) quarter
      });
      try
      {
        return ReportEngine.GetNMLSReportData(year, quarter);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (NMLSReportData[]) null;
      }
    }

    public virtual NMLSReportData[] GetNMLSReportData(int year, int quarter, string state)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetNMLSReportData), new object[3]
      {
        (object) year,
        (object) quarter,
        (object) state
      });
      try
      {
        return ReportEngine.GetNMLSReportData(year, quarter, state);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (NMLSReportData[]) null;
      }
    }

    public virtual NMLSReportData GetNMLSReportData(
      int year,
      int quarter,
      string state,
      string appSource)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetNMLSReportData), new object[4]
      {
        (object) year,
        (object) quarter,
        (object) state,
        (object) appSource
      });
      try
      {
        return ReportEngine.GetNMLSReportData(year, quarter, state, appSource);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (NMLSReportData) null;
      }
    }

    public virtual NMLSReportData[] GetNMLSReportData(string state)
    {
      this.onApiCalled(nameof (ReportManager), nameof (GetNMLSReportData), new object[1]
      {
        (object) state
      });
      try
      {
        return ReportEngine.GetNMLSReportData(state);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
        return (NMLSReportData[]) null;
      }
    }

    public virtual void SaveNMLSReportData(NMLSReportData data)
    {
      this.onApiCalled(nameof (ReportManager), nameof (SaveNMLSReportData), new object[1]
      {
        (object) data
      });
      try
      {
        ReportEngine.SaveNMLSReportData(data);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SaveNMLSReportData(NMLSReportData[] data)
    {
      this.onApiCalled(nameof (ReportManager), nameof (SaveNMLSReportData), (object[]) data);
      try
      {
        ReportEngine.SaveNMLSReportData(data);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ReportManager), ex, this.Session.SessionInfo);
      }
    }
  }
}

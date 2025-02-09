// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.ManagementSession
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.DirectoryServices.Contracts.Services;
using Elli.DirectoryServices.Proxies;
using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.ServerTasks;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Configuration;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.Configuration;
using EllieMae.EMLite.Server.ServerCommon;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServerObjects.eFolder;
using EllieMae.EMLite.Server.Tasks;
using EllieMae.EMLite.VersionInterface15;
using Encompass.Diagnostics.Config;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class ManagementSession : ClientSession, IManagementSession, IClientSession
  {
    private const string className = "ManagementSession";
    public static readonly SessionManager Sessions = new SessionManager((IClientContext) null);
    private static System.Version serverVersion = (System.Version) null;
    private ArrayList tracingInstances = new ArrayList();
    private const string name = "ManagmentSession";

    public ManagementSession(LoginParameters loginParams, IServerCallback callback)
      : base(loginParams, Guid.NewGuid().ToString(), callback)
    {
      ManagementSession.Sessions.AddSession((IClientSession) this, (IConnectionManager) new ConnectionManagerWrapper());
    }

    public override string UserID => "sysadmin";

    public ServerProcessInfo GetServerProcessInfo()
    {
      this.onApiCalled(nameof (GetServerProcessInfo));
      try
      {
        ArrayList arrayList = new ArrayList();
        foreach (string allInstanceName in EnGlobalSettings.GetAllInstanceNames(false))
        {
          EnGlobalSettings enGlobalSettings = new EnGlobalSettings(allInstanceName);
          int sessionCount = 0;
          int sessionObjCount = 0;
          if (!enGlobalSettings.Disabled)
          {
            ClientContext clientContext = ClientContext.Get(allInstanceName);
            if (clientContext != null)
            {
              sessionCount = clientContext.Sessions.SessionCount;
              sessionObjCount = clientContext.Sessions.SessionObjectCount;
            }
          }
          ServerInstanceInfo serverInstanceInfo = new ServerInstanceInfo(allInstanceName, !enGlobalSettings.Disabled, sessionCount, sessionObjCount);
          arrayList.Add((object) serverInstanceInfo);
        }
        if (ManagementSession.serverVersion == (System.Version) null)
        {
          try
          {
            string str = Assembly.GetExecutingAssembly().CodeBase;
            if (str.ToLower().StartsWith("file:///"))
              str = str.Substring("file:///".Length);
            ManagementSession.serverVersion = new System.Version(FileVersionInfo.GetVersionInfo(str.Replace(Assembly.GetExecutingAssembly().GetName().Name, "Server")).FileVersion);
          }
          catch
          {
            ManagementSession.serverVersion = new System.Version("0.0.0.0");
          }
        }
        return new ServerProcessInfo(EncompassServer.StartTime, (ServerInstanceInfo[]) arrayList.ToArray(typeof (ServerInstanceInfo)), StandardFields.Instance.FileVersion, ManagementSession.serverVersion);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (ServerProcessInfo) null;
      }
    }

    public void RefreshCache(string instanceName, bool async)
    {
      this.onApiCalled(nameof (RefreshCache), (object) instanceName, (object) async);
      try
      {
        ClientContext.Get(instanceName, false)?.RefreshCache(async);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public System.Version GetEncompassFieldListVersion()
    {
      this.onApiCalled(nameof (GetEncompassFieldListVersion));
      try
      {
        return StandardFields.Instance.FileVersion;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (System.Version) null;
      }
    }

    public System.Version ReloadEncompassFieldList()
    {
      this.onApiCalled(nameof (ReloadEncompassFieldList));
      try
      {
        StandardFields.Instance.Reload(true);
        return StandardFields.Instance.FileVersion;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (System.Version) null;
      }
    }

    public Dictionary<string, string[]> GetSessionObjectNames(string instanceName, string userid)
    {
      this.onApiCalled(nameof (GetSessionObjectNames), (object) instanceName);
      try
      {
        return ClientContext.Open(instanceName, false).Sessions.GetSessionObjectNames(userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (Dictionary<string, string[]>) null;
      }
    }

    public SessionInfo[] GetAllSessionInfo(string instanceName)
    {
      this.onApiCalled(nameof (GetAllSessionInfo), (object) instanceName);
      try
      {
        return ClientContext.Open(instanceName, false).Sessions.GetAllSessionInfo();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (SessionInfo[]) null;
      }
    }

    public SessionInfo[] GetAllSessionInfoFromDB(string instanceName)
    {
      this.onApiCalled(nameof (GetAllSessionInfoFromDB), (object) instanceName);
      try
      {
        return ClientContext.Open(instanceName, false).Sessions.GetAllSessionInfoFromDB();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (SessionInfo[]) null;
      }
    }

    public int GetSqlRead(string instanceName)
    {
      this.onApiCalled(nameof (GetSqlRead), (object) instanceName);
      try
      {
        return ClientContext.Open(instanceName, false).Settings.GetSqlRead();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return -1;
      }
    }

    public void SetSqlRead(string instanceName, int sqlRead)
    {
      this.onApiCalled(nameof (SetSqlRead), (object) instanceName, (object) sqlRead);
      try
      {
        ClientContext.Open(instanceName, false).Settings.SetSqlRead(sqlRead);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public SessionDiagnostics GetSessionDiagnostics(string instanceName, string sessionId)
    {
      this.onApiCalled(nameof (GetSessionDiagnostics), (object) instanceName, (object) sessionId);
      try
      {
        return !(ClientContext.Open(instanceName, false).Sessions.GetSession(sessionId) is Session session) ? (SessionDiagnostics) null : session.Diagnostics.Clone();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (SessionDiagnostics) null;
      }
    }

    public SessionDiagnostics[] GetAllSessionDiagnostics(string instanceName)
    {
      this.onApiCalled(nameof (GetAllSessionDiagnostics), (object) instanceName);
      try
      {
        ClientContext clientContext = ClientContext.Open(instanceName, false);
        List<SessionDiagnostics> sessionDiagnosticsList = new List<SessionDiagnostics>();
        foreach (ClientSession session1 in clientContext.Sessions)
        {
          if (session1 is Session session2)
            sessionDiagnosticsList.Add(session2.Diagnostics.Clone());
        }
        return sessionDiagnosticsList.ToArray();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (SessionDiagnostics[]) null;
      }
    }

    public string[] GetAllInstanceNames(bool enabledOnly)
    {
      this.onApiCalled(nameof (GetAllInstanceNames), (object) enabledOnly);
      try
      {
        return EnGlobalSettings.GetAllInstanceNames(enabledOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (string[]) null;
      }
    }

    public string GetDefaultInstanceName()
    {
      this.onApiCalled(nameof (GetDefaultInstanceName));
      try
      {
        return EnConfigurationSettings.InstanceName;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (string) null;
      }
    }

    public void Enable(string instanceName)
    {
      this.onApiCalled(nameof (Enable), (object) instanceName);
      try
      {
        throw new NotSupportedException("The function is no longer supported");
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public void TerminateSession(string instanceName, string sessionId)
    {
      this.onApiCalled(nameof (TerminateSession), (object) instanceName, (object) sessionId);
      try
      {
        ClientContext.Open(instanceName, false).Sessions.TerminateSession(sessionId, true, true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public void TerminateAllSessions(string instanceName)
    {
      this.onApiCalled(nameof (TerminateAllSessions), (object) instanceName);
      try
      {
        ClientContext.Open(instanceName, false).Sessions.TerminateAllSessions(DisconnectEventArgument.Force, false);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public bool PingSession(string instanceName, string sessionId, TimeSpan timeout)
    {
      this.onApiCalled(nameof (PingSession), (object) instanceName, (object) sessionId, (object) timeout);
      try
      {
        IClientSession session = ClientContext.Open(instanceName, false).Sessions.GetSession(sessionId);
        if (session == null)
          Err.Raise(nameof (ManagementSession), (ServerException) new ServerArgumentException("Invalid session ID specified"));
        return session.Ping(timeout);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return false;
      }
    }

    public void Disable(string instanceName, bool terminateSessions)
    {
      this.onApiCalled(nameof (Disable), (object) instanceName, (object) terminateSessions);
      try
      {
        throw new NotSupportedException("This function is no longer supported");
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public ProcessStatus GetStatus(string instanceName)
    {
      this.onApiCalled(nameof (GetStatus), (object) instanceName);
      try
      {
        ClientContext clientContext = ClientContext.Open(instanceName, false);
        if (clientContext.Sessions.SessionCount > 0)
          return ProcessStatus.Active;
        return clientContext.Settings.Disabled ? ProcessStatus.Disabled : ProcessStatus.Inactive;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return ProcessStatus.Unknown;
      }
    }

    public int GetSessionCount(string instanceName)
    {
      this.onApiCalled(nameof (GetSessionCount), (object) instanceName);
      try
      {
        return ClientContext.Open(instanceName, false).Sessions.SessionCount;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return 0;
      }
    }

    public List<Tuple<string, string, string>> VerifyXDBFieldsNotInSyncWithLoan(
      string instanceName,
      string loanGuid)
    {
      List<Tuple<string, string, string>> tupleList = new List<Tuple<string, string, string>>();
      using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        using (Loan latestVersion = LoanStore.GetLatestVersion(loanGuid))
        {
          LoanData loanData = latestVersion.LoanData;
          LoanXDBTableList loanXdbTableList = LoanXDBStore.GetLoanXDBTableList();
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.AppendLine("DECLARE @loanXRefId INT = 0;");
          dbQueryBuilder.AppendLine("SELECT @loanXRefId = XRefID FROM LoanXRef WHERE LoanGuid = '" + loanGuid + "';");
          for (int i = 0; i < loanXdbTableList.TableCount; ++i)
          {
            LoanXDBTable tableAt = loanXdbTableList.GetTableAt(i);
            DbTableInfo dynamicTable = EllieMae.EMLite.Server.DbAccessManager.GetDynamicTable(tableAt.TableName);
            dbQueryBuilder.AppendLine("SELECT " + string.Join(",", tableAt.GetFieldNames()) + " FROM " + dynamicTable.Name + " WHERE XRefId = @loanXRefId;");
          }
          DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
          for (int index = 0; index < dataSet.Tables.Count; ++index)
          {
            DataRowCollection rows = dataSet.Tables[index].Rows;
            if (rows != null && rows.Count != 0)
            {
              DataRow dataRow = rows[0];
              foreach (LoanXDBField loanXdbField in loanXdbTableList.GetTableAt(index))
              {
                string fieldForReportingDb = loanData.GetSimpleFieldForReportingDB(loanXdbField.FieldIDWithCoMortgagor);
                string s = dataRow[loanXdbField.ColumnName]?.ToString();
                switch (loanXdbField.FieldType)
                {
                  case LoanXDBTableList.TableTypes.IsNumeric:
                    double result1 = 0.0;
                    double result2 = 0.0;
                    double.TryParse(string.Format("{0:N6}", (object) fieldForReportingDb), out result1);
                    double.TryParse(string.Format("{0:N6}", (object) s), out result2);
                    if (result1 != result2)
                    {
                      tupleList.Add(new Tuple<string, string, string>(loanXdbField.FieldID, s, fieldForReportingDb));
                      continue;
                    }
                    continue;
                  case LoanXDBTableList.TableTypes.IsDate:
                    DateTime result3 = DateTime.MinValue;
                    DateTime result4 = DateTime.MinValue;
                    DateTime.TryParse(fieldForReportingDb, out result3);
                    DateTime.TryParse(s, out result4);
                    if (result4 != result3)
                    {
                      tupleList.Add(new Tuple<string, string, string>(loanXdbField.FieldID, s, fieldForReportingDb));
                      continue;
                    }
                    continue;
                  default:
                    int length1 = s.Length > loanXdbField.FieldSizeToInteger ? loanXdbField.FieldSizeToInteger : s.Length;
                    int length2 = fieldForReportingDb.Length > loanXdbField.FieldSizeToInteger ? loanXdbField.FieldSizeToInteger : fieldForReportingDb.Length;
                    if (!fieldForReportingDb.Substring(0, length2).Equals(s.Substring(0, length1), StringComparison.InvariantCultureIgnoreCase))
                    {
                      tupleList.Add(new Tuple<string, string, string>(loanXdbField.FieldID, s, fieldForReportingDb));
                      continue;
                    }
                    continue;
                }
              }
            }
          }
        }
      }
      return tupleList;
    }

    public void VerifyConfiguration(string instanceName)
    {
      this.onApiCalled(nameof (VerifyConfiguration), (object) instanceName);
      try
      {
        JedVersion compatibilityVersion = EncompassServer.GetServerCompatibilityVersion();
        JedVersion version = VersionInformation.CurrentVersion.Version;
        if (compatibilityVersion != version)
          Err.Raise(nameof (ManagementSession), (ServerException) new ServerDataException("The Server Assembly Version (" + (object) compatibilityVersion + ") does not match the EncompassVersion.xml version (" + (object) version + ")"));
        ClientContext context = ClientContext.Open(instanceName, false);
        EncompassSystemInfo encompassSystemInfo = EncompassSystemDbAccessor.GetEncompassSystemInfo((IClientContext) context);
        if (encompassSystemInfo.DbVersion != VersionInformation.CurrentVersion.Version.ToString())
          Err.Raise(nameof (ManagementSession), (ServerException) new ServerDataException("The database schema version (" + encompassSystemInfo.DbVersion + ") does not match installed software version"));
        LicenseInfo serverLicense = Company.GetServerLicense((IClientContext) context);
        if (serverLicense == null)
          Err.Raise(nameof (ManagementSession), (ServerException) new LicenseException((LicenseInfo) null, LicenseExceptionType.InvalidLicense, "Invalid or missing license information"));
        if (!serverLicense.Enabled)
          Err.Raise(nameof (ManagementSession), (ServerException) new LicenseException(serverLicense, LicenseExceptionType.AccountDisabled, "Inactive instance License"));
        if (serverLicense.Version < VersionInformation.CurrentVersion.Version)
          Err.Raise((IClientContext) context, nameof (ManagementSession), (ServerException) new LicenseException(serverLicense, LicenseExceptionType.InvalidLicense, "License version ('" + (object) serverLicense.Version + "') is incompatible with software version ('" + (object) VersionInformation.CurrentVersion.Version + "'"));
        if (serverLicense.UserLimit < 0)
          Err.Raise((IClientContext) context, nameof (ManagementSession), (ServerException) new LicenseException(serverLicense, LicenseExceptionType.InvalidLicense, "License user limit (" + (object) serverLicense.UserLimit + ") is invalid."));
        using (context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        {
          using (User latestVersion = UserStore.GetLatestVersion("admin"))
          {
            if (!latestVersion.Exists)
              Err.Raise(nameof (ManagementSession), new ServerException("Admin user account cannot be opened"));
            UserInfo userInfo = latestVersion.UserInfo;
          }
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.Append("select top 1 [Guid] from [LoanSummary] where LastModified is not null order by LastModified desc");
          DataRowCollection rows = dbQueryBuilder.ExecuteTableQuery().Rows;
          if (rows.Count <= 0)
            return;
          string guid = rows[0]["Guid"].ToString();
          try
          {
            using (Loan latestVersion = LoanStore.GetLatestVersion(guid))
            {
              if (latestVersion.LoanData != null)
                return;
              Err.Raise(nameof (ManagementSession), new ServerException("Cannot open loan data for loan '" + guid + "' in folder " + latestVersion.Identity.LoanFolder));
            }
          }
          catch (Exception ex)
          {
            Err.Raise(nameof (ManagementSession), new ServerException("Cannot open loan data for loan '" + guid + "': " + ex.Message));
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public void CreateInstance(string instanceName, DataSourceInfo dataSource)
    {
      this.onApiCalled(nameof (CreateInstance), (object) instanceName);
      try
      {
        throw new NotSupportedException("This function is no longer supported");
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public void DeleteInstance(string instanceName)
    {
      this.onApiCalled(nameof (DeleteInstance), (object) instanceName);
      try
      {
        ClientContext clientContext = ClientContext.Get(instanceName, false);
        if (clientContext != null && clientContext.Sessions.SessionCount > 0)
          Err.Raise(nameof (ManagementSession), new ServerException("The instance '" + instanceName + "' is active and cannot be deleted"));
        EnGlobalSettings.DeleteInstance(instanceName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public DataSourceInfo GetDataSource(string instanceName)
    {
      this.onApiCalled(nameof (GetDataSource), (object) instanceName);
      try
      {
        EnGlobalSettings enGlobalSettings = new EnGlobalSettings(instanceName);
        if (!enGlobalSettings.Exists())
          Err.Raise(nameof (ManagementSession), new ServerException("The specified instance name is not valid"));
        return new DataSourceInfo(enGlobalSettings.AppLogDirectory, enGlobalSettings.EncompassDataDirectory, enGlobalSettings.DatabaseServer, enGlobalSettings.DatabaseName, enGlobalSettings.DatabaseUserID, "", enGlobalSettings.DatabaseAGListener, this.IsMultiTargetEnabled(), this.IsSingleTargetEnabled(), this.GetSingleLogFileBaseDir());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (DataSourceInfo) null;
      }
    }

    public string Echo(string text)
    {
      this.onApiCalled(nameof (Echo), (object) text);
      return text;
    }

    public void GarbageCollect()
    {
      this.onApiCalled(nameof (GarbageCollect));
      try
      {
        GC.Collect();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public void SetGlobalTraceLevel(TraceLevel level)
    {
      this.onApiCalled("SetTraceLevel", (object) level);
      try
      {
        TraceLog.TraceLevel = level;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public void BroadcastMessage(string instanceName, Message message)
    {
      this.onApiCalled(nameof (BroadcastMessage), (object) instanceName, (object) message);
      try
      {
        ClientContext.Open(instanceName, false).Sessions.BroadcastMessage(message.Clone(this.SessionInfo), true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public void SendMessage(string instanceName, string sessionId, Message message)
    {
      this.onApiCalled(nameof (SendMessage), (object) instanceName, (object) sessionId, (object) message);
      try
      {
        ClientContext.Open(instanceName, false).Sessions.SendMessage(message.Clone(this.SessionInfo), sessionId, true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public CompanyInfo GetCompanyInfo(string instanceName)
    {
      this.onApiCalled(nameof (GetCompanyInfo), (object) instanceName);
      try
      {
        return Company.GetCompanyInfo((IClientContext) ClientContext.Open(instanceName, false));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (CompanyInfo) null;
      }
    }

    public string GetCompanySetting(string instanceName, string section, string key)
    {
      this.onApiCalled(nameof (GetCompanySetting), (object) instanceName, (object) section, (object) key);
      try
      {
        return Company.GetCompanySetting((IClientContext) ClientContext.Open(instanceName, false), section, key);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (string) null;
      }
    }

    public bool MigratePipelineView(string instanceName)
    {
      this.onApiCalled(nameof (MigratePipelineView), (object) instanceName);
      try
      {
        PipelineViewXmlToDbMigrationManager.MigratePipelineViewFromXmlToDB(instanceName);
        return true;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return false;
      }
    }

    public bool MigrateEfolderViewsFromFilesToDB(
      string instanceName,
      TemplateSettingsType templateSettingsType)
    {
      this.onApiCalled(nameof (MigrateEfolderViewsFromFilesToDB), (object) instanceName);
      try
      {
        using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
          return TemplateFilesToDBMigrationManager.MigrateEFolderViewsFromFilesToDB(instanceName, templateSettingsType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return false;
      }
    }

    public bool isPipelineMigrationRequired(string instanceName)
    {
      using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("select value from company_settings where attribute = 'MigrateUserPipelineView'");
        if (!(dbQueryBuilder.ExecuteScalar()?.ToString() == "1"))
          return false;
        TraceLog.WriteVerbose("ManagmentSession", " Migrating User Pipeline View Xml To DB is Skipping as flag is 1.");
        return true;
      }
    }

    public void updatePipelineMigrationRunningFlag(
      string instanceName,
      bool updateMigrationTimeFlag)
    {
      using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("UPDATE company_settings set value = '0' where attribute = 'MigrationViewInProcess'");
        if (updateMigrationTimeFlag)
        {
          dbQueryBuilder.AppendLine("IF EXISTS (SELECT * FROM [company_settings] WHERE category = 'MIGRATION' AND attribute = 'MigrateUserPipelineView' AND value = '1')");
          dbQueryBuilder.AppendLine("BEGIN");
          dbQueryBuilder.AppendLine("IF NOT EXISTS (SELECT * FROM [company_settings] WHERE category = 'MIGRATION' AND attribute = 'UserPipelineViewMigrationTime')");
          dbQueryBuilder.AppendLine("BEGIN");
          dbQueryBuilder.AppendLine("insert into[company_settings](category, attribute, value) values('MIGRATION', 'UserPipelineViewMigrationTime', '" + DateTime.Now.ToString() + "')");
          dbQueryBuilder.AppendLine("END");
          dbQueryBuilder.AppendLine("ELSE");
          dbQueryBuilder.AppendLine("BEGIN");
          dbQueryBuilder.AppendLine("UPDATE company_settings set value = '" + DateTime.Now.ToString() + "' where attribute = 'UserPipelineViewMigrationTime'");
          dbQueryBuilder.AppendLine("END");
          dbQueryBuilder.AppendLine("END");
        }
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    public bool isPipelineMigrationRunning(string instanceName)
    {
      using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("select value from company_settings where attribute = 'MigrationViewInProcess'");
        string str = dbQueryBuilder.ExecuteScalar()?.ToString();
        if (string.IsNullOrEmpty(str))
        {
          dbQueryBuilder.Reset();
          dbQueryBuilder.AppendLine("Insert into company_settings values('MIGRATION','MigrationViewInProcess','1')");
          dbQueryBuilder.ExecuteNonQuery();
        }
        else
        {
          if (str.ToString() == "1")
            return true;
          dbQueryBuilder.Reset();
          dbQueryBuilder.AppendLine("UPDATE company_settings set value = '1' where attribute = 'MigrationViewInProcess'");
          dbQueryBuilder.ExecuteNonQuery();
        }
        return false;
      }
    }

    private string readNodeValue(XmlDocument xmlDocument, string attName)
    {
      string str = xmlDocument.SelectSingleNode("objdata/element/element[@name='" + attName + "']")?.InnerXml;
      if (string.IsNullOrEmpty(str) || !(attName == "filter"))
        return str;
      str = "<objdata><element name = \"root\" >" + str + "</element></objdata > ";
      return str;
    }

    public void UpdatePipelineViewExtOrgId(string instanceName)
    {
      using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        EnGlobalSettings enGlobalSettings = new EnGlobalSettings(instanceName);
        if (!enGlobalSettings.Exists())
          Err.Raise(nameof (ManagementSession), new ServerException("The specified instance name is not valid"));
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        TraceLog.WriteVerbose("ManagmentSession", "Start  Update Pipeline View ExtOrgId");
        Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
        try
        {
          foreach (UserInfo allUser in User.GetAllUsers((string) null))
            insensitiveHashtable.Add((object) allUser.Userid, (object) null);
          TraceLog.WriteVerbose("ManagmentSession", "Getting list of all users");
          dbQueryBuilder.Reset();
          dbQueryBuilder.AppendLine("select viewID,userid, name,orgType, externalOrgId FROM Acl_UserPipelineViews where viewID in (Select Max(viewID)  from Acl_UserPipelineViews group by userid,name)");
          DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
          Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
          if (dataTable.Rows.Count > 0)
          {
            foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
              dictionary.Add(row["userid"].ToString() + "_" + row["name"].ToString(), new List<string>()
              {
                row["externalOrgId"]?.ToString(),
                row["viewID"].ToString(),
                row["orgType"]?.ToString()
              });
          }
          string str1 = Path.Combine(enGlobalSettings.AppDataDirectory, "Users");
          TraceLog.WriteVerbose("ManagmentSession", "Path for migration : " + enGlobalSettings.AppDataDirectory);
          int num1 = 0;
          string empty1 = string.Empty;
          string empty2 = string.Empty;
          foreach (string enumerateDirectory in Directory.EnumerateDirectories(str1))
          {
            string str2 = enumerateDirectory + "\\TemplateSettings\\PipelineView";
            TraceLog.WriteVerbose("ManagmentSession", "searchPath : " + str2);
            if (Directory.Exists(str2))
            {
              foreach (string enumerateFile in Directory.EnumerateFiles(str2))
              {
                try
                {
                  string filename = Path.Combine(str1, str2, enumerateFile);
                  ++num1;
                  string key1 = enumerateDirectory.Substring(enumerateDirectory.LastIndexOf("\\") + 1);
                  int num2 = enumerateFile.LastIndexOf("\\");
                  string empty3 = string.Empty;
                  if (num2 >= 0)
                  {
                    string str3 = key1 + "-" + enumerateFile.Substring(num2 + 1);
                  }
                  XmlDocument xmlDocument = new XmlDocument();
                  xmlDocument.Load(filename);
                  if (insensitiveHashtable.ContainsKey((object) key1))
                  {
                    string key2 = key1 + "_" + this.readNodeValue(xmlDocument, "name");
                    string str4 = this.readNodeValue(xmlDocument, "externalOrgId");
                    if (this.readNodeValue(xmlDocument, "orgType") == "TPO")
                    {
                      if (dictionary[key2][2] == "TPO")
                      {
                        if (!string.IsNullOrEmpty(str4))
                        {
                          if (dictionary.ContainsKey(key2))
                          {
                            if (!(dictionary[key2][0] == "0"))
                            {
                              if (!string.IsNullOrEmpty(dictionary[key2][0]))
                                goto label_32;
                            }
                            this.updateExtOrgIdToDB(xmlDocument, dictionary[key2][1]);
                          }
                        }
                      }
                    }
                  }
                }
                catch (Exception ex)
                {
                  TraceLog.WriteError("ManagmentSession", "Error Updating external Org ID from XML file: " + enumerateFile + " with Error : " + ex.Message);
                  Err.Reraise(nameof (ManagementSession), ex);
                }
label_32:
                TraceLog.WriteVerbose("ManagmentSession", "Completed Update Pipeline View ExtOrgId");
              }
            }
          }
        }
        catch (Exception ex)
        {
          TraceLog.WriteError("ManagmentSession", "Error Updating external Org ID with Error : " + ex.Message);
          Err.Reraise(nameof (ManagementSession), ex);
        }
      }
    }

    private string GetFilterData(string filter)
    {
      return string.IsNullOrEmpty(filter) || string.Compare(filter, "<objdata><element name=\"root\" /></objdata>", StringComparison.CurrentCultureIgnoreCase) == 0 ? "" : filter;
    }

    public string EncodeString(string value)
    {
      return value == null ? "NULL" : "'" + value.Replace("'", "''") + "'";
    }

    private bool updateExtOrgIdToDB(XmlDocument xmlDocument, string viewID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("Update [Acl_UserPipelineViews]");
        dbQueryBuilder.AppendLine("Set [externalOrgId] = " + this.EncodeString(this.readNodeValue(xmlDocument, "externalOrgId")));
        dbQueryBuilder.AppendLine("Set [LastModifiedDate] = " + SQL.EncodeDateTime(DateTime.UtcNow));
        dbQueryBuilder.AppendLine("Set [LastModifiedBy] = " + SQL.EncodeString("<system>"));
        dbQueryBuilder.AppendLine("Where [viewID] = '" + viewID + "'");
        dbQueryBuilder.ExecuteNonQuery();
        return true;
      }
      catch (Exception ex)
      {
        TraceLog.WriteError("ManagmentSession", "Error Update External Org ID SQL : " + ex.Message + ". Skipping " + this.readNodeValue(xmlDocument, "name") + " xml file.");
      }
      TraceLog.WriteVerbose("ManagmentSession", "Return false");
      return false;
    }

    private static string returnColumnValue(XmlNode xn, string attName)
    {
      return xn.SelectSingleNode("element[@name='" + attName + "']")?.InnerText;
    }

    public LicenseInfo GetServerLicense(string instanceName)
    {
      this.onApiCalled(nameof (GetServerLicense), (object) instanceName);
      try
      {
        return Company.GetServerLicense((IClientContext) ClientContext.Open(instanceName, false));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (LicenseInfo) null;
      }
    }

    public int GetEnabledUserCount(string instanceName)
    {
      this.onApiCalled(nameof (GetEnabledUserCount), (object) instanceName);
      try
      {
        return User.GetEnabledUserCount((IClientContext) ClientContext.Open(instanceName, false));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return 0;
      }
    }

    public void SetDebug(bool enabled)
    {
      this.onApiCalled(nameof (SetDebug), (object) enabled);
      try
      {
        EnConfigurationSettings.GlobalSettings.Debug = enabled;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public bool IsDebugEnabled()
    {
      this.onApiCalled(nameof (IsDebugEnabled));
      try
      {
        return EnConfigurationSettings.GlobalSettings.Debug;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return false;
      }
    }

    public int GetErrorCount(string instanceName)
    {
      this.onApiCalled(nameof (GetErrorCount), (object) instanceName);
      try
      {
        return ClientContext.Open(instanceName, false).TraceLog.GetErrorCount();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return 0;
      }
    }

    public string[] GetErrors(string instanceName)
    {
      this.onApiCalled(nameof (GetErrors), (object) instanceName);
      try
      {
        return ClientContext.Open(instanceName, false).TraceLog.GetErrors();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (string[]) null;
      }
    }

    public void ClearErrors(string instanceName)
    {
      this.onApiCalled(nameof (ClearErrors), (object) instanceName);
      try
      {
        ClientContext.Open(instanceName, false).TraceLog.ClearErrors();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public int GetSystemAlertCount()
    {
      this.onApiCalled(nameof (GetSystemAlertCount));
      try
      {
        return EncompassServer.GetSystemAlerts().Length;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return 0;
      }
    }

    public SystemAlert[] GetSystemAlerts()
    {
      this.onApiCalled(nameof (GetSystemAlerts));
      try
      {
        return EncompassServer.GetSystemAlerts();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (SystemAlert[]) null;
      }
    }

    public void ClearSystemAlerts()
    {
      this.onApiCalled(nameof (ClearSystemAlerts));
      try
      {
        EncompassServer.ClearSystemAlerts();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public void RegisterForEvents(Type eventType)
    {
      this.onApiCalled(nameof (RegisterForEvents), (object) eventType);
      if (eventType == (Type) null)
        Err.Raise(TraceLevel.Warning, nameof (ManagementSession), (ServerException) new ServerArgumentException("Event type cannot be null", nameof (eventType)));
      try
      {
        if (!typeof (ServerMonitorEvent).IsAssignableFrom(eventType))
          Err.Raise(TraceLevel.Warning, nameof (ManagementSession), (ServerException) new ServerArgumentException("Event type must derive from ServerMonitorEvent class", nameof (eventType)));
        ManagementSession.Sessions.Events.RegisterListener(eventType, (IClientSession) this);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public void UnregisterForEvents(Type eventType)
    {
      this.onApiCalled(nameof (UnregisterForEvents), (object) eventType);
      if (eventType == (Type) null)
        Err.Raise(TraceLevel.Warning, nameof (ManagementSession), (ServerException) new ServerArgumentException("Event type cannot be null", nameof (eventType)));
      try
      {
        if (!typeof (ServerMonitorEvent).IsAssignableFrom(eventType))
          Err.Raise(TraceLevel.Warning, nameof (ManagementSession), (ServerException) new ServerArgumentException("Event type must derive from ServerMonitorEvent class", nameof (eventType)));
        ManagementSession.Sessions.Events.UnregisterListener(eventType, (IClientSession) this);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public void RegisterForTracing(string instanceName, TraceLevel traceLevel)
    {
      this.onApiCalled(nameof (RegisterForTracing), (object) instanceName, (object) traceLevel);
      try
      {
        ClientContext.Open(instanceName, false).Sessions.Tracing.RegisterListener(traceLevel, (IClientSession) this);
        this.tracingInstances.Add((object) instanceName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public void UnregisterForTracing(string instanceName)
    {
      this.onApiCalled(nameof (UnregisterForTracing), (object) instanceName);
      try
      {
        ClientContext.Open(instanceName, false).Sessions.Tracing.UnregisterListener((IClientSession) this);
        this.tracingInstances.Remove((object) instanceName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public UserInfo[] GetAllUsers(string instanceName)
    {
      this.onApiCalled(nameof (GetAllUsers), (object) instanceName);
      try
      {
        using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
          return User.GetAllUsers((string) null);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (UserInfo[]) null;
      }
    }

    public void EnableUser(string instanceName, string userId)
    {
      this.onApiCalled(nameof (EnableUser), (object) instanceName, (object) userId);
      try
      {
        using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        {
          using (User user = UserStore.CheckOut(userId))
          {
            if (!user.Exists)
              Err.Raise(nameof (ManagementSession), (ServerException) new ObjectNotFoundException("Invalid user ID", ObjectType.User, (object) userId));
            user.UserInfo.Status = UserInfo.UserStatusEnum.Enabled;
            user.CheckIn(this.SessionInfo.UserID, false);
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public void DisableUser(string instanceName, string userId)
    {
      this.onApiCalled("EnableUser", (object) instanceName, (object) userId);
      try
      {
        using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        {
          using (User user = UserStore.CheckOut(userId))
          {
            if (!user.Exists)
              Err.Raise(nameof (ManagementSession), (ServerException) new ObjectNotFoundException("Invalid user ID", ObjectType.User, (object) userId));
            user.UserInfo.Status = UserInfo.UserStatusEnum.Disabled;
            user.CheckIn(this.SessionInfo.UserID, false);
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public void DefragmentDatabase(string instanceName, IServerProgressFeedback feedback)
    {
      this.onApiCalled(nameof (DefragmentDatabase), (object) instanceName);
      try
      {
        using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
          EllieMae.EMLite.Server.DbAccessManager.DefragmentDatabase(feedback);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public int GetDbFragmentationLevel(string instanceName)
    {
      this.onApiCalled("GetMaxDbFragmentationLevel", (object) instanceName);
      try
      {
        using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
          return EllieMae.EMLite.Server.DbAccessManager.GetAvgFragmentationLevel();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return -1;
      }
    }

    public int GetDbConsistencyErrorCount(string instanceName)
    {
      this.onApiCalled(nameof (GetDbConsistencyErrorCount), (object) instanceName);
      try
      {
        using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
          return EllieMae.EMLite.Server.DbAccessManager.GetConsistencyErrorCount();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return -1;
      }
    }

    public DbSize GetDbSize(string instanceName)
    {
      this.onApiCalled(nameof (GetDbSize), (object) instanceName);
      try
      {
        using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
          return EllieMae.EMLite.Server.DbAccessManager.GetDatabaseSize();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (DbSize) null;
      }
    }

    public DbUsageInfo GetDbUsageInfo(string instanceName)
    {
      this.onApiCalled(nameof (GetDbUsageInfo), (object) instanceName);
      try
      {
        using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
          return EllieMae.EMLite.Server.DbAccessManager.GetUsageInfo();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (DbUsageInfo) null;
      }
    }

    public void ResetEPassMessages(string instanceName)
    {
      this.onApiCalled(nameof (ResetEPassMessages), (object) instanceName);
      try
      {
        using (ClientContext.Open(instanceName, false).MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
          EPassMessages.ResetMessages();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public void ResetClientContextTraceLog(string instanceName)
    {
      this.onApiCalled(nameof (ResetClientContextTraceLog), (object) instanceName);
      try
      {
        ClientContext.Open(instanceName, false).ResetTraceLog();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public string GetEncInstallDir(string instanceName)
    {
      this.onApiCalled(nameof (GetEncInstallDir), (object) instanceName);
      try
      {
        return ClientContext.Open(instanceName, false).Settings.ApplicationDir;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
      return (string) null;
    }

    public string GetEncLogDir(string instanceName)
    {
      this.onApiCalled(nameof (GetEncLogDir), (object) instanceName);
      try
      {
        return ClientContext.Open(instanceName, false).Settings.LogDir;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
      return (string) null;
    }

    public string GetEncDataDir(string instanceName)
    {
      this.onApiCalled(nameof (GetEncDataDir), (object) instanceName);
      try
      {
        return ClientContext.Open(instanceName, false).Settings.EncompassDataDir;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
      return (string) null;
    }

    public string[] GetCachedDataSource(string instanceName)
    {
      return new string[3]
      {
        this.GetEncDataDir(instanceName) ?? "",
        this.getDbConnectionStringWithMaskedPassword(instanceName) ?? "",
        this.GetEncLogDir(instanceName) ?? ""
      };
    }

    private string getDbConnectionStringWithMaskedPassword(string instanceName)
    {
      string withMaskedPassword = string.Empty;
      try
      {
        withMaskedPassword = this.getSqlConnString(instanceName);
        if (string.IsNullOrWhiteSpace(withMaskedPassword))
          return (string) null;
        int length = withMaskedPassword.IndexOf("Password=", StringComparison.CurrentCultureIgnoreCase);
        if (length >= 0)
        {
          string str1 = "********";
          string str2 = withMaskedPassword.Substring(0, length);
          string str3 = withMaskedPassword.Substring(length + "Password=".Length);
          int num = str3.IndexOf(";");
          string str4 = num < 0 ? "" : str3.Substring(num + 1);
          return str2 + "HashCode=" + str1 + ";" + str4;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
      return withMaskedPassword;
    }

    public string GetDbConnectionString(string instanceName)
    {
      return this.getDbConnectionString(instanceName);
    }

    private string getDbConnectionString(string instanceName)
    {
      this.onApiCalled("GetDbConnectionString", (object) instanceName);
      try
      {
        string sqlConnString = this.getSqlConnString(instanceName);
        if (sqlConnString == null)
          return (string) null;
        int length = sqlConnString.IndexOf("Password=", StringComparison.CurrentCultureIgnoreCase);
        if (length >= 0)
        {
          string d = (this.getSqlPwdFromConnString(sqlConnString) ?? "").Trim();
          string str1 = !(d != "") ? "********" : XT.ESB64(d, KB.KB64);
          string str2 = sqlConnString.Substring(0, length);
          string str3 = sqlConnString.Substring(length + "Password=".Length);
          int num = str3.IndexOf(";");
          string str4 = num < 0 ? "" : str3.Substring(num + 1);
          return str2 + "HashCode=" + str1 + ";" + str4;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
      return (string) null;
    }

    private string getSqlConnString(string instanceName)
    {
      return ClientContext.Open(instanceName, false).Settings.ConnectionString;
    }

    private string getSqlPwd(string instanceName)
    {
      string sqlConnString = this.getSqlConnString(instanceName);
      return sqlConnString == null ? (string) null : this.getSqlPwdFromConnString(sqlConnString);
    }

    private string getSqlPwdFromConnString(string connString)
    {
      if (connString == null)
        return (string) null;
      string pwdFromConnString = (string) null;
      int startIndex = connString.IndexOf("Password=", StringComparison.CurrentCultureIgnoreCase);
      if (startIndex >= 0)
      {
        int num = connString.Substring(startIndex).IndexOf(";");
        pwdFromConnString = num >= 0 ? connString.Substring(startIndex + "Password=".Length, num - "Password=".Length) : connString.Substring(startIndex + "Password=".Length);
      }
      return pwdFromConnString;
    }

    public void SendReconnectMessageAndStopServer()
    {
      RemotableEncompassServer.Stop(DisconnectEventArgument.Reconnect);
      Process.GetCurrentProcess().Kill();
    }

    public void RenewServerLic(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (RenewServerLic), (object) instanceName);
      if (string.IsNullOrWhiteSpace(instanceName))
        Err.Raise(TraceLevel.Error, nameof (ManagementSession), (ServerException) new ServerArgumentException("Instance name cannot be null while updating server lic", "category"));
      try
      {
        ClientContext context = ClientContext.Open(instanceName, false);
        using (context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
          LicenseManager.RefreshServerLicense((IClientContext) context);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public void RefreshBillingCalc(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (RefreshBillingCalc), (object) instanceName);
      if (string.IsNullOrWhiteSpace(instanceName))
        Err.Raise(TraceLevel.Error, nameof (ManagementSession), (ServerException) new ServerArgumentException("Instance name cannot be null while refreshing billing calc"));
      try
      {
        ClientContext context = ClientContext.Open(instanceName, false);
        using (context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        {
          bool isBillingModelEnabled;
          LicenseManager.RefreshBillingCalculation((IClientContext) context, out isBillingModelEnabled);
          if (!isBillingModelEnabled)
            throw new Exception("License.BillingModel not enabled for the instance");
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public ServerTaskProcessorStatus[] GetTaskProcessorStatuses()
    {
      this.onApiCalled(nameof (GetTaskProcessorStatuses));
      try
      {
        return TaskQueue.GetProcessorStatuses();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (ServerTaskProcessorStatus[]) null;
      }
    }

    public string StartTaskProcessor()
    {
      this.onApiCalled(nameof (StartTaskProcessor));
      try
      {
        return TaskQueue.AddTaskProcessor();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (string) null;
      }
    }

    public void ResetTaskProcessors()
    {
      this.onApiCalled(nameof (ResetTaskProcessors));
      try
      {
        TaskQueue.ResetTaskProcessors();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public object GetServerSetting(string instanceName, string path)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (GetServerSetting), (object) instanceName, (object) path);
      if ((path ?? "") == null)
        Err.Raise(TraceLevel.Warning, nameof (ManagementSession), (ServerException) new ServerArgumentException("Setting path cannot be null", nameof (path)));
      try
      {
        ClientContext clientContext = ClientContext.Open(instanceName, false);
        using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
          return clientContext.Settings.GetServerSetting(path, true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (object) null;
      }
    }

    public IDictionary GetServerSettings(string instanceName, string category)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (GetServerSettings), (object) instanceName, (object) category);
      if ((category ?? "") == null)
        Err.Raise(TraceLevel.Warning, nameof (ManagementSession), (ServerException) new ServerArgumentException("Setting category cannot be null", nameof (category)));
      try
      {
        ClientContext clientContext = ClientContext.Open(instanceName, false);
        using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
          return clientContext.Settings.GetServerSettings(category, true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (IDictionary) null;
      }
    }

    public void UpdateServerSetting(string instanceName, string path, object value)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (UpdateServerSetting), (object) instanceName, (object) path, value);
      if ((path ?? "") == null)
        Err.Raise(TraceLevel.Warning, nameof (ManagementSession), (ServerException) new ServerArgumentException("Setting path cannot be null", nameof (path)));
      if (value == null)
        Err.Raise(TraceLevel.Warning, nameof (ManagementSession), (ServerException) new ServerArgumentException("Setting value cannot be null", nameof (value)));
      try
      {
        ClientContext clientContext = ClientContext.Open(instanceName, false);
        using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
          clientContext.Settings.SetServerSetting(path, value, true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public int GetNumConcurrentLogins(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (GetNumConcurrentLogins), (object) instanceName);
      try
      {
        return ClientContext.Open(instanceName, false).Cache.GetNumConcurrentLogins();
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ManagementSession), "Error getting NoConcurrentLogins: " + ex.Message);
        return -1;
      }
    }

    public int GetMaxConcurrentLogins(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (GetMaxConcurrentLogins), (object) instanceName);
      try
      {
        return ClientContext.Open(instanceName, false).Cache.GetMaxConcurrentLogins();
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ManagementSession), "Error getting MaxConcurrentLogins: " + ex.Message);
        return -2;
      }
    }

    public void SetMaxConcurrentLogins(string instanceName, int val, bool updateDatabase = false)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (SetMaxConcurrentLogins), (object) instanceName);
      try
      {
        ClientContext clientContext = ClientContext.Open(instanceName, false);
        clientContext.Cache.SetMaxConcurrentLogins(val, updateDatabase ? (IClientContext) clientContext : (IClientContext) null);
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ManagementSession), "Error setting MaxConcurrentLogins: " + ex.Message);
      }
    }

    public string GetUseReaderWriterLockSlim(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (GetUseReaderWriterLockSlim), (object) instanceName);
      try
      {
        return SmartClientUtils._UseReaderWriterLockSlim.ToString() + ", " + (object) SmartClientUtils.UseReaderWriterLockSlim_ + ", " + (SmartClientUtils.UseReaderWriterLockSlim ? (object) "True" : (object) "False") + ", " + (SmartClientUtils.LockSlimNoRecursion ? (object) "True" : (object) "False") + ", " + (SmartClientUtils.LockSlimNoRecursionStandardFields ? (object) "True" : (object) "False");
      }
      catch (Exception ex)
      {
        string message = "Error getting UseReaderWriterLockSlim: " + ex.Message;
        TraceLog.WriteWarning(nameof (ManagementSession), message);
        return message;
      }
    }

    public void ResetUseReaderWriterLockSlim(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (ResetUseReaderWriterLockSlim), (object) instanceName);
      try
      {
        SmartClientUtils.ResetUseReaderWriterLockSlim();
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ManagementSession), "Error resetting UseReaderWriterLockSlim: " + ex.Message);
      }
    }

    public string[] GetMacAddresses()
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (GetMacAddresses));
      try
      {
        return SystemUtil.MacAddresses;
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ManagementSession), "Error getting Mac Addresses: " + ex.Message);
        return (string[]) null;
      }
    }

    public string[] GetServerCacheNames(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) "GetServerCacheKeys", (object) instanceName);
      try
      {
        return ClientContext.Open(instanceName, false).Cache.GetKeys();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (string[]) null;
      }
    }

    public ServerInternalsInfo GetLoanXDBStoreCacheInfo(string instanceName, string fieldID = null)
    {
      this.onApiCalled(nameof (ManagementSession), (object) "GetLoanXDBCacheInfo", (object) instanceName);
      try
      {
        object obj = ClientContext.Open(instanceName, false).Cache.Get("LoanXDBStore");
        if (obj == null)
          return (ServerInternalsInfo) null;
        ServerInternalsInfo xdbStoreCacheInfo = new ServerInternalsInfo();
        xdbStoreCacheInfo.SetHeadersIfNotAlreadySet(new string[3]
        {
          "Field ID w/ CoMortgagor",
          "Table Name",
          "Column Name"
        });
        foreach (LoanXDBField allField in ((LoanXDBTableList) obj).GetAllFields())
        {
          fieldID = (fieldID ?? "").Trim();
          if (fieldID == "" || allField.FieldID.IndexOf(fieldID, StringComparison.CurrentCultureIgnoreCase) >= 0)
            xdbStoreCacheInfo.Add(new string[3]
            {
              allField.FieldIDWithCoMortgagor,
              allField.TableName,
              allField.ColumnName
            });
        }
        return xdbStoreCacheInfo;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (ServerInternalsInfo) null;
      }
    }

    public ServerInternalsInfo GetServerCacheInfo(string instanceName, string cacheName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (GetServerCacheInfo), (object) instanceName, (object) cacheName);
      try
      {
        ServerInternalsInfo serverCacheInfo = new ServerInternalsInfo();
        object obj1 = ClientContext.Open(instanceName, false).Cache.Get(cacheName);
        if (obj1 == null)
          return (ServerInternalsInfo) null;
        if (obj1.GetType() == typeof (LoanXDBTableList))
        {
          serverCacheInfo.SetHeadersIfNotAlreadySet(new string[2]
          {
            "Table Name",
            "Field Count"
          });
          LoanXDBTableList loanXdbTableList = (LoanXDBTableList) obj1;
          for (int i = 0; i < loanXdbTableList.TableCount; ++i)
          {
            LoanXDBTable tableAt = loanXdbTableList.GetTableAt(i);
            serverCacheInfo.Add(new string[2]
            {
              tableAt.TableName,
              string.Concat((object) tableAt.FieldCount)
            });
          }
        }
        else if (obj1.GetType() == typeof (EnableDisableSetting))
        {
          serverCacheInfo.SetHeadersIfNotAlreadySet(new string[1]
          {
            cacheName
          });
          EnableDisableSetting enableDisableSetting = (EnableDisableSetting) obj1;
          serverCacheInfo.Add(new string[1]
          {
            enableDisableSetting == EnableDisableSetting.Enabled ? "Enabled" : "Disabled"
          });
        }
        else if (obj1.GetType() == typeof (IPRange[]))
        {
          serverCacheInfo.SetHeadersIfNotAlreadySet(new string[3]
          {
            "User ID",
            "Start IP",
            "End IP"
          });
          foreach (IPRange ipRange in (IPRange[]) obj1)
            serverCacheInfo.Add(new string[3]
            {
              ipRange.Userid,
              ipRange.StartIP,
              ipRange.EndIP
            });
        }
        else if (obj1.GetType() == typeof (ERDBCache))
        {
          serverCacheInfo.SetHeadersIfNotAlreadySet(new string[2]
          {
            "Name",
            "Value"
          });
          ERDBCache erdbCache = (ERDBCache) obj1;
          serverCacheInfo.Add(new string[2]
          {
            "Client ID",
            erdbCache.ClientID
          });
          serverCacheInfo.Add(new string[2]
          {
            "System ID",
            erdbCache.EncompassSystemID
          });
          serverCacheInfo.Add(new string[2]
          {
            "ERDB Server",
            erdbCache.ERDBServer
          });
          serverCacheInfo.Add(new string[2]
          {
            "ERDB Server Port",
            string.Concat((object) erdbCache.ERDBServerPort)
          });
        }
        else if (obj1.GetType() == typeof (Dictionary<string, object>))
        {
          serverCacheInfo.SetHeadersIfNotAlreadySet(new string[2]
          {
            "Name",
            "Value"
          });
          Dictionary<string, object> dictionary = (Dictionary<string, object>) obj1;
          foreach (string key in dictionary.Keys)
          {
            object obj2 = dictionary[key];
            serverCacheInfo.Add(new string[2]
            {
              key,
              key != "DbPwd" ? string.Concat(obj2) : "********"
            });
          }
        }
        else if (obj1.GetType() == typeof (Hashtable))
        {
          serverCacheInfo.SetHeadersIfNotAlreadySet(new string[2]
          {
            "Name",
            "Value"
          });
          Hashtable hashtable = (Hashtable) obj1;
          foreach (object key in (IEnumerable) hashtable.Keys)
          {
            object obj3 = hashtable[key];
            serverCacheInfo.Add(new string[2]
            {
              string.Concat(key),
              string.Concat(obj3)
            });
          }
        }
        else if (obj1.GetType() == typeof (string))
        {
          serverCacheInfo.SetHeadersIfNotAlreadySet(new string[1]
          {
            cacheName
          });
          string str = (string) obj1;
          serverCacheInfo.Add(new string[1]{ str });
        }
        else if (obj1.GetType() == typeof (bool))
        {
          serverCacheInfo.SetHeadersIfNotAlreadySet(new string[1]
          {
            cacheName
          });
          bool flag = (bool) obj1;
          serverCacheInfo.Add(new string[1]
          {
            flag ? "True" : "False"
          });
        }
        else
        {
          if (!(obj1.GetType() == typeof (int)) && !(obj1.GetType() == typeof (int)))
            throw new Exception(obj1.GetType().ToString() + ": type not handled");
          serverCacheInfo.SetHeadersIfNotAlreadySet(new string[1]
          {
            cacheName
          });
          int num = (int) obj1;
          serverCacheInfo.Add(new string[1]
          {
            string.Concat((object) num)
          });
        }
        return serverCacheInfo;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (ServerInternalsInfo) null;
      }
    }

    public void TerminateS2SClientSession(string instanceName, string s2sSessionID)
    {
      this.onApiCalled(nameof (TerminateS2SClientSession), (object) instanceName, (object) s2sSessionID);
      try
      {
        ClientContext.Open(instanceName, false).Sessions.TerminateS2SClientSession(s2sSessionID, "Terminated by management session");
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public void EndS2SSession(string serverUri)
    {
      this.onApiCalled(nameof (EndS2SSession), (object) serverUri);
      try
      {
        SessionManager.EndS2SSession(serverUri);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public ServerInternalsInfo GetAllS2SSessionInfo(string instanceName)
    {
      this.onApiCalled(nameof (GetAllS2SSessionInfo), (object) instanceName);
      try
      {
        S2SSession[] allS2Ssessions = ((SessionManager) ClientContext.Open(instanceName, false).Sessions).GetAllS2SSessions();
        ServerInternalsInfo allS2SsessionInfo = new ServerInternalsInfo();
        allS2SsessionInfo.SetHeadersIfNotAlreadySet(new string[4]
        {
          "Remote Server",
          "Is Connected",
          "Encompass System ID",
          "Userid"
        });
        foreach (S2SSession s2Ssession in allS2Ssessions)
          allS2SsessionInfo.Add(new string[4]
          {
            s2Ssession.RemoteServer,
            s2Ssession.IsConnected ? "True" : "False",
            s2Ssession.EncompassSystemID,
            s2Ssession.UserID
          });
        return allS2SsessionInfo;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (ServerInternalsInfo) null;
      }
    }

    public ServerInternalsInfo GetAllS2SClientSessionInfo(string instanceName)
    {
      this.onApiCalled(nameof (GetAllS2SClientSessionInfo), (object) instanceName);
      try
      {
        IClientSession[] s2SclientSessions = ClientContext.Open(instanceName, false).Sessions.GetAllS2SClientSessions();
        ServerInternalsInfo sclientSessionInfo = new ServerInternalsInfo();
        sclientSessionInfo.SetHeadersIfNotAlreadySet(new string[5]
        {
          "Session ID",
          "Host Name",
          "Server Uri",
          "Login Time",
          "Userid"
        });
        foreach (ClientSession clientSession in s2SclientSessions)
          sclientSessionInfo.Add(new string[5]
          {
            clientSession.SessionID,
            clientSession.Hostname,
            clientSession.Server.Uri.AbsoluteUri,
            clientSession.LoginTime.ToString(),
            clientSession.UserID
          });
        return sclientSessionInfo;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (ServerInternalsInfo) null;
      }
    }

    public string GetEncompassDbFullVersion(string instanceName)
    {
      this.onApiCalled(nameof (GetEncompassDbFullVersion), (object) instanceName);
      string encompassDbFullVersion = "";
      try
      {
        encompassDbFullVersion = EncompassSystemDbAccessor.GetEncompassSystemInfo((IClientContext) ClientContext.Get(instanceName)).DbFullVersion;
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ManagementSession), "Error getting Encompass System Info: " + ex.Message);
      }
      return encompassDbFullVersion;
    }

    public int GetRebuildReportingDbThreadCount(string instanceName)
    {
      this.onApiCalled(nameof (GetRebuildReportingDbThreadCount), (object) instanceName);
      int reportingDbThreadCount = 0;
      try
      {
        reportingDbThreadCount = (int) ClientContext.Get(instanceName).Settings.GetServerSetting("Internal.RebuildReportingDbThreadCount");
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ManagementSession), "Error getting RebuildReportingDbThreadCount: " + ex.Message);
      }
      return reportingDbThreadCount;
    }

    public void SetRebuildReportingDbThreadCount(string instanceName, int val)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (SetRebuildReportingDbThreadCount), (object) instanceName);
      try
      {
        ClientContext.Open(instanceName, false).Settings.SetServerSetting("Internal.RebuildReportingDbThreadCount", (object) val);
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ManagementSession), "Error setting RebuildReportintDbThreadCount: " + ex.Message);
      }
    }

    public string GetCacheStoreSource(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (GetCacheStoreSource), (object) instanceName);
      try
      {
        ClientContext clientContext = ClientContext.Open(instanceName, false);
        return clientContext.Cache.CacheStoreSource == CacheStoreSource.HazelCast ? "Hazelcast" : (clientContext.Cache.CacheStoreSource == CacheStoreSource.InProcess ? "InProcess" : "Unknown");
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ManagementSession), "Error getting cache store source: " + ex.Message);
        return ex.Message;
      }
    }

    public string ResetCacheStore(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) "ResetCacheStoreSource", (object) instanceName);
      try
      {
        ClientContext clientContext = ClientContext.Open(instanceName, false);
        clientContext.ResetCacheStore(true);
        return clientContext.Cache.CacheStoreSource.ToString();
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ManagementSession), "Error resetting cache store source: " + ex.Message);
        return ex.Message;
      }
    }

    public string[] GetCacheKeys(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (GetCacheKeys), (object) instanceName);
      try
      {
        return ClientContext.Open(instanceName, false).Cache.GetKeys();
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public object GetCachedValue(string key, string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (GetCachedValue), (object) instanceName);
      try
      {
        return ClientContext.Open(instanceName, false).Cache.Get(key);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public void RemoveKey(string key, string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (RemoveKey), (object) instanceName);
      try
      {
        ClientContext.Open(instanceName, false).Cache.Remove(key);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public RegistryKey OpenEncompassRegistryKey(string name, bool wow6432Node)
    {
      try
      {
        string name1 = Path.Combine(wow6432Node ? "Software\\Wow6432Node\\Ellie Mae" : "Software\\Ellie Mae", name);
        return Registry.LocalMachine.OpenSubKey(name1, false);
      }
      catch
      {
        return (RegistryKey) null;
      }
    }

    public int CacheLockTimeout(string instanceName)
    {
      int num = -1;
      try
      {
        num = EllieMae.EMLite.Server.ServerGlobals.LockTimeoutDuringGetCache;
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ManagementSession), "Error getting cache lock timeout: " + ex.Message);
      }
      return num;
    }

    public bool? CacheRegetFromCache(string instanceName)
    {
      bool? nullable = new bool?();
      try
      {
        nullable = new bool?(EllieMae.EMLite.Server.ServerGlobals.CacheRegetFromCache);
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ManagementSession), "Error getting CacheRegetFromCache setting: " + ex.Message);
      }
      return nullable;
    }

    public bool? CacheRegetFromDB(string instanceName)
    {
      bool? nullable = new bool?();
      try
      {
        nullable = new bool?(EllieMae.EMLite.Server.ServerGlobals.CacheRegetFromDB);
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ManagementSession), "Error getting CacheRegetFromDB setting: " + ex.Message);
      }
      return nullable;
    }

    public object ServerGlobal(string instanceName, string varname)
    {
      object obj = (object) null;
      try
      {
        PropertyInfo property1 = typeof (EllieMae.EMLite.Server.ServerGlobals).GetProperty(varname);
        if ((PropertyInfo) null != property1)
        {
          obj = property1.GetValue((object) null);
        }
        else
        {
          EnGlobalSettings globalSettings = EnConfigurationSettings.GlobalSettings;
          PropertyInfo property2 = globalSettings.GetType().GetProperty(varname);
          if ((PropertyInfo) null != property2)
            obj = property2.GetValue((object) globalSettings);
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ManagementSession), "Error getting Server Global Setting: '" + varname + "' - " + ex.Message);
      }
      return obj;
    }

    public string GetCacheStats(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (GetCacheStats), (object) instanceName);
      string str = (string) null;
      try
      {
        ClientContext clientContext = ClientContext.Open(instanceName, false);
        if (clientContext.Cache != null)
          str = clientContext.Cache.GetStats();
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ManagementSession), "Error getting cache stats: " + ex.Message);
      }
      return str ?? "No Cache Present";
    }

    public int GetMaxRecentLoans(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (GetMaxRecentLoans), (object) instanceName);
      try
      {
        object obj = ClientContext.Open(instanceName, false).Cache.Get("MaxRecentLoans");
        return obj == null ? -1 : (int) obj;
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ManagementSession), "Error getting MaxRecentLoans: " + ex.Message);
        return -2;
      }
    }

    public void ResetMaxRecentLoans(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (ResetMaxRecentLoans), (object) instanceName);
      try
      {
        ClientContext.Open(instanceName, false).Cache.Remove("MaxRecentLoans");
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ManagementSession), "Error getting MaxRecentLoans: " + ex.Message);
      }
    }

    public Tuple<int, int> GetServerActivityTimeoutTimerInterval()
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (GetServerActivityTimeoutTimerInterval));
      TimeSpan timeSpan = EllieMae.EMLite.Server.ServerGlobals.ServerActivityTimeout;
      int totalSeconds1 = (int) timeSpan.TotalSeconds;
      timeSpan = EllieMae.EMLite.Server.ServerGlobals.ServerActivityTimerInterval;
      int totalSeconds2 = (int) timeSpan.TotalSeconds;
      return new Tuple<int, int>(totalSeconds1, totalSeconds2);
    }

    public Tuple<int, int> SetServerActivityTimeoutTimerInterval(int timeout, int timerInterval)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (SetServerActivityTimeoutTimerInterval), (object) timeout, (object) timerInterval);
      TimeSpan timeSpan;
      if (timeout < 0)
      {
        timeSpan = EllieMae.EMLite.Server.ServerGlobals.ServerActivityTimeout;
        timeout = (int) timeSpan.TotalSeconds;
      }
      if (timerInterval < 0)
      {
        timeSpan = EllieMae.EMLite.Server.ServerGlobals.ServerActivityTimerInterval;
        timerInterval = (int) timeSpan.TotalSeconds;
      }
      EnConfigurationSettings.GlobalSettings.ServerActivityTimeout = TimeSpan.FromSeconds((double) timeout);
      EnConfigurationSettings.GlobalSettings.ServerActivityTimerInterval = TimeSpan.FromSeconds((double) timerInterval);
      timeSpan = EnConfigurationSettings.GlobalSettings.ServerActivityTimeout;
      int totalSeconds1 = (int) timeSpan.TotalSeconds;
      timeSpan = EnConfigurationSettings.GlobalSettings.ServerActivityTimerInterval;
      int totalSeconds2 = (int) timeSpan.TotalSeconds;
      return new Tuple<int, int>(totalSeconds1, totalSeconds2);
    }

    public Dictionary<string, bool> CompareRegistryAndDSConfigurations(
      string instanceName,
      string dsUrl,
      string[] ignores)
    {
      this.onApiCalled("CompareDirectoryServiceConfiguration", (object) instanceName);
      try
      {
        return new RegistryInstanceConfiguration(instanceName).CompareWith((IInstanceConfiguration) new DirectoryInstanceConfiguration(instanceName, (IDirectoryService) new DirectoryServiceClient(dsUrl)), ignores);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
        return (Dictionary<string, bool>) null;
      }
    }

    public void CopyRegistryConfigsToDS(string instanceName, string dsUrl, string[] ignores)
    {
      this.onApiCalled("CompareDirectoryServiceConfiguration", (object) instanceName);
      try
      {
        new RegistryInstanceConfiguration(instanceName).CopyTo((IInstanceConfiguration) new DSWriterInstanceConfiguration(instanceName, (IDirectoryService) new DirectoryServiceClient(dsUrl)), ignores);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ManagementSession), ex);
      }
    }

    public override void Disconnect()
    {
      foreach (string tracingInstance in this.tracingInstances)
      {
        try
        {
          ClientContext.Open(tracingInstance, false).Sessions.Tracing.UnregisterListener((IClientSession) this);
        }
        catch
        {
        }
      }
      ManagementSession.Sessions.RemoveSession((IClientSession) this, (IConnectionManager) new ConnectionManagerWrapper());
      base.Disconnect();
    }

    private void onApiCalled(string apiName, params object[] parms)
    {
      try
      {
        TraceLog.WriteApi(nameof (ManagementSession), apiName, parms);
      }
      catch
      {
      }
    }

    private ClientContext CreateConcurrentUpdateContext(string instanceName)
    {
      ClientContext clientContext = ClientContext.Open(instanceName, true);
      if (clientContext == null)
        throw new TypeInitializationException(typeof (ClientContext).FullName, new Exception("Context could not initialized for the given instance."));
      return clientContext.IsConcurrentUpdateNotificationEnabled ? clientContext : throw new InvalidOperationException("Concurrent update notification is not enabled for this instance.");
    }

    public string SendConcurrentUpdateNotification(
      string instanceName,
      string sessionId,
      string loanGuid)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (SendConcurrentUpdateNotification), (object) instanceName, (object) sessionId, (object) loanGuid);
      ClientContext concurrentUpdateContext = this.CreateConcurrentUpdateContext(instanceName);
      using (concurrentUpdateContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        return concurrentUpdateContext.ConcurrentUpdateNotificationHandler.SendNotification(sessionId, loanGuid, DateTime.Now.ToUniversalTime(), "").ToString();
    }

    public void ClearConcurrentUpdateNotificationCache(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (ClearConcurrentUpdateNotificationCache), (object) instanceName);
      ClientContext concurrentUpdateContext = this.CreateConcurrentUpdateContext(instanceName);
      using (concurrentUpdateContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        concurrentUpdateContext.Sessions.ClearConcurrentUpdateNotificationCache();
    }

    public void ClearConcurrentUpdateNotificationCache(string instanceName, string sessionId)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (ClearConcurrentUpdateNotificationCache), (object) instanceName, (object) sessionId);
      ClientContext concurrentUpdateContext = this.CreateConcurrentUpdateContext(instanceName);
      using (concurrentUpdateContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        concurrentUpdateContext.Sessions.ClearConcurrentUpdateNotificationCache(sessionId);
    }

    public string GetNotificationSubscriptionStatus(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (GetNotificationSubscriptionStatus), (object) instanceName);
      string subscriptionStatus = "Subscribed: False";
      ClientContext clientContext = ClientContext.Open(instanceName, true);
      if (clientContext == null)
        throw new TypeInitializationException(typeof (ClientContext).FullName, new Exception("Context could not initialized for the given instance."));
      using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        IConcurrentUpdateNotificationHandler notificationHandler = clientContext.ConcurrentUpdateNotificationHandler;
        if (notificationHandler != null)
        {
          string notificationStatus = notificationHandler.GetNotificationStatus();
          subscriptionStatus = "Subscribed: True " + Environment.NewLine + " Subscription Time: " + clientContext.ConcurrentUpdateNotificationSubscriptionTime.ToString() + " " + Environment.NewLine + " " + notificationStatus;
        }
      }
      return subscriptionStatus;
    }

    public void ResetConcurrentUpdateNotificationSubscription(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) "ResetConcurrentUpdateNotificationSubcsription", (object) instanceName);
      ClientContext clientContext = ClientContext.Open(instanceName, true);
      if (clientContext == null)
        throw new TypeInitializationException(typeof (ClientContext).FullName, new Exception("Context could not initialized for the given instance."));
      using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        clientContext.Cache.Remove(Company.GetCacheKey("FEATURE"));
        clientContext.ResetConcurrentUpdateNotificationSubscription();
      }
    }

    private ClientContext CreateTradeLoanUpdateContext(string instanceName)
    {
      ClientContext clientContext = ClientContext.Open(instanceName, true);
      if (clientContext == null)
        throw new TypeInitializationException(typeof (ClientContext).FullName, new Exception("Context could not initialized for the given instance."));
      return clientContext.IsTradeLoanUpdateNotificationEnabled ? clientContext : throw new InvalidOperationException("Trade Loan update notification is not enabled for this instance.");
    }

    public void ResetTradeLoanUpdateNotificationSubscription(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) "ResetTradeLoanUpdateNotificationSubcsription", (object) instanceName);
      ClientContext clientContext = ClientContext.Open(instanceName, true);
      if (clientContext == null)
        throw new TypeInitializationException(typeof (ClientContext).FullName, new Exception("Context could not initialized for the given instance."));
      using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        clientContext.Cache.Remove(Company.GetCacheKey("FEATURE"));
        clientContext.ResetTradeLoanUpdateNotificationSubscription();
      }
    }

    public string SendTradeLoanUpdateNotification(
      string instanceName,
      string sessionId,
      string tradeId,
      string tradeStatus = "")
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (SendTradeLoanUpdateNotification), (object) instanceName, (object) sessionId, (object) tradeId, (object) tradeStatus);
      ClientContext loanUpdateContext = this.CreateTradeLoanUpdateContext(instanceName);
      using (loanUpdateContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        return loanUpdateContext.TradeLoanUpdateNotificationHandler.NotifyEncompassClient(sessionId, tradeId, tradeStatus, DateTime.Now.ToUniversalTime(), "").ToString();
    }

    public string GetTradeLoanUpdateNotificationSubscriptionStatus(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (GetTradeLoanUpdateNotificationSubscriptionStatus), (object) instanceName);
      string subscriptionStatus = "Subscribed: False";
      ClientContext clientContext = ClientContext.Open(instanceName, true);
      if (clientContext == null)
        throw new TypeInitializationException(typeof (ClientContext).FullName, new Exception("Context could not initialized for the given instance."));
      using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        ITradeLoanUpdateNotificationHandler notificationHandler = clientContext.TradeLoanUpdateNotificationHandler;
        if (notificationHandler != null)
        {
          string notificationStatus = notificationHandler.GetTradeLoanUpdateNotificationStatus();
          subscriptionStatus = "Subscribed: True " + Environment.NewLine + " Subscription Time: " + clientContext.TradeLoanUpdateNotificationSubscriptionTime.ToString() + " " + Environment.NewLine + " " + notificationStatus;
        }
      }
      return subscriptionStatus;
    }

    private bool IsMultiTargetEnabled()
    {
      return DiagConfig<ServerDiagConfigData>.Instance.ConfigData.LogListeners.ClassicLog.MultiFileTarget.Enabled;
    }

    private bool IsSingleTargetEnabled()
    {
      return DiagConfig<ServerDiagConfigData>.Instance.ConfigData.LogListeners.ClassicLog.SingleFileTarget.Enabled;
    }

    private string GetSingleLogFileBaseDir()
    {
      return DiagConfig<ServerDiagConfigData>.Instance.ConfigData.LogListeners.ClassicLog.SingleFileTarget.BaseDir;
    }

    public void ResetLockComparisonFieldsNotificationSubscription(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (ResetLockComparisonFieldsNotificationSubscription), (object) instanceName);
      ClientContext clientContext = ClientContext.Open(instanceName, true);
      if (clientContext == null)
        throw new TypeInitializationException(typeof (ClientContext).FullName, new Exception("Context could not initialized for the given instance."));
      using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        clientContext.ResetLockComparisonFieldsNotificationSubscription();
    }

    public string GetLockComparisonFieldsNotificationSubscriptionStatus(string instanceName)
    {
      this.onApiCalled(nameof (ManagementSession), (object) nameof (GetLockComparisonFieldsNotificationSubscriptionStatus), (object) instanceName);
      string subscriptionStatus = "Subscribed: False";
      ClientContext clientContext = ClientContext.Open(instanceName, true);
      if (clientContext == null)
        throw new TypeInitializationException(typeof (ClientContext).FullName, new Exception("Context could not initialized for the given instance."));
      using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        ILockComparisonFieldsNotificationHandler notificationHandler = clientContext.LockComparisonFieldsNotificationHandler;
        if (notificationHandler != null)
        {
          string notificationStatus = notificationHandler.GetLockComparisonFieldsNotificationStatus();
          subscriptionStatus = "Subscribed: True " + Environment.NewLine + " Subscription Time: " + clientContext.LockComparisonFieldsNotificationSubscriptionTime.ToString() + " " + Environment.NewLine + " " + notificationStatus;
        }
      }
      return subscriptionStatus;
    }
  }
}

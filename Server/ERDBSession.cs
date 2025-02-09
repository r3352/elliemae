// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ERDBSession
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils;
using EllieMae.EMLite.ReportingDbUtils.Basics;
using EllieMae.EMLite.ReportingDbUtils.Query;
using Encompass.Diagnostics;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ERDBSession
  {
    private const string className = "ERDBSession�";
    public static readonly string failureNotification = "ERDBFailureNotification";
    internal static readonly string registrationInfo = "ERDBRegistrationInfo";
    internal static string regRootWithInstance = "Software\\Ellie Mae\\Encompass" + (EnConfigurationSettings.InstanceName == "" ? "" : "$" + EnConfigurationSettings.InstanceName);
    private static object notificationLock = new object();

    internal static string GetDbConnectionString(bool useERDB, ClientContext context)
    {
      return useERDB && context.UseERDB ? string.Empty : context.Settings.ConnectionString;
    }

    internal static string getERDBConnectionString(ClientContext context)
    {
      Dictionary<string, object> registrationInfo = ERDBSession.GetERDBRegistrationInfo((IClientContext) context);
      return ConfigSettings.GetDbConnectionString((string) registrationInfo["DbServer"], (string) registrationInfo["DbName"], (string) registrationInfo["DbLogin"], (string) registrationInfo["DbPwd"]);
    }

    public static Dictionary<string, object> GetERDBRegistrationInfo(IClientContext context)
    {
      Dictionary<string, object> registrationInfo = (Dictionary<string, object>) context.Cache.Get(ERDBSession.registrationInfo);
      if (registrationInfo != null)
        return registrationInfo;
      IDictionary serverSettings = context.Settings.GetServerSettings(ERDBSession.registrationInfo);
      string str1 = (string) serverSettings[(object) (ERDBSession.registrationInfo + ".AppServer")];
      Dictionary<string, object> o;
      if (str1 != null)
      {
        o = new Dictionary<string, object>();
        o.Add("AppServer", (object) str1);
        o.Add("Port", serverSettings[(object) (ERDBSession.registrationInfo + ".Port")]);
        o.Add("EncDataDir", serverSettings[(object) (ERDBSession.registrationInfo + ".EncDataDir")]);
        o.Add("DbServer", serverSettings[(object) (ERDBSession.registrationInfo + ".DbServer")]);
        o.Add("DbName", serverSettings[(object) (ERDBSession.registrationInfo + ".DbName")]);
        o.Add("DbLogin", serverSettings[(object) (ERDBSession.registrationInfo + ".DbLogin")]);
        string str2 = XT.DSB64((string) serverSettings[(object) (ERDBSession.registrationInfo + ".DbPwd")], KB.KB64);
        o.Add("DbPwd", (object) str2);
      }
      else
      {
        o = RegistryUtil.GetRegistryValues(Registry.LocalMachine, ERDBSession.regRootWithInstance + "\\ERDB\\" + context.ClientID, "AppServer", "Port", "EncDataDir", "DbServer", "DbName", "DbLogin", "DbPwd", "MigratedToDB");
        string appServerPort = (string) o["Port"];
        if ((appServerPort ?? "").Trim() == "")
          o["Port"] = (object) "11099";
        string encDataDir = (string) o["EncDataDir"];
        if ((encDataDir ?? "").Trim() == "")
          o["EncDataDir"] = (object) EnConfigurationSettings.GlobalSettings.EncompassDataDirectory;
        string appServer = (string) o["AppServer"];
        string dbServer = (string) o["DbServer"];
        string dbName = (string) o["DbName"];
        string dbLogin = (string) o["DbLogin"];
        string dbPwd = XT.DSB64((string) o["DbPwd"], KB.KB64);
        o["DbPwd"] = (object) dbPwd;
        string clientId = context.ClientID;
        ERDBSession.UpdateERDBRegistrationInfo(context, clientId, appServer, appServerPort, encDataDir, dbServer, dbName, dbLogin, dbPwd);
      }
      context.Cache.Put(ERDBSession.registrationInfo, (object) o);
      return o;
    }

    public static void UpdateERDBRegistrationInfo(
      IClientContext context,
      string clientID,
      string appServer,
      int appServerPort,
      string encDataDir,
      string dbServer,
      string dbName,
      string dbLogin,
      string dbPwd)
    {
      ERDBSession.UpdateERDBRegistrationInfo(context, clientID, appServer, string.Concat((object) appServerPort), encDataDir, dbServer, dbName, dbLogin, dbPwd);
    }

    internal static void UpdateERDBRegistrationInfo(
      IClientContext context,
      string clientID,
      string appServer,
      string appServerPort,
      string encDataDir,
      string dbServer,
      string dbName,
      string dbLogin,
      string dbPwd)
    {
      context.Cache.Remove(ERDBSession.registrationInfo);
      context.ClearERDBCache();
      context.Settings.SetServerSettings((IDictionary) new Dictionary<string, string>()
      {
        {
          ERDBSession.registrationInfo + ".AppServer",
          (appServer ?? "").Trim()
        },
        {
          ERDBSession.registrationInfo + ".Port",
          (appServerPort ?? "").Trim()
        },
        {
          ERDBSession.registrationInfo + ".EncDataDir",
          (encDataDir ?? "").Trim()
        },
        {
          ERDBSession.registrationInfo + ".DbServer",
          (dbServer ?? "").Trim()
        },
        {
          ERDBSession.registrationInfo + ".DbName",
          (dbName ?? "").Trim()
        },
        {
          ERDBSession.registrationInfo + ".DbLogin",
          (dbLogin ?? "").Trim()
        },
        {
          ERDBSession.registrationInfo + ".DbPwd",
          XT.ESB64((dbPwd ?? "").Trim(), KB.KB64)
        }
      });
      RegistryUtil.SetRegistryValue(Registry.LocalMachine, ERDBSession.regRootWithInstance + "\\ERDB\\" + clientID, "AppServer", (object) "MigratedToDB");
    }

    internal static LoanXDBStatusInfo GetLoanXDBStatus(bool useERDB, ClientContext context)
    {
      try
      {
        return LoanXDBStoreUtil.GetLoanXDBStatusFromDatabase(ERDBSession.GetDbConnectionString(useERDB, context), context.Settings.DbServerType);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ERDBSession), "Error getting loan XDB status: " + ex.Message);
        return (LoanXDBStatusInfo) null;
      }
    }

    internal static void SetDbLoanXDBTableList(
      bool useERDB,
      ClientContext context,
      LoanXDBTableList tableList,
      string userId)
    {
      try
      {
        LoanXDBStoreUtil.SetDbLoanXDBTableList(ERDBSession.GetDbConnectionString(useERDB, context), context.Settings.DbServerType, tableList, useERDB, userId);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ERDBSession), "Error setting loan XDB table list: " + ex.Message);
        throw ex;
      }
    }

    internal static LoanXDBTableList GetLoanXDBTableListFromDatabase(
      bool useERDB,
      ClientContext context)
    {
      try
      {
        return LoanXDBStoreUtil.GetLoanXDBTableListFromDatabase(ERDBSession.GetDbConnectionString(useERDB, context), context.Settings.DbServerType);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ERDBSession), "Error getting loan XDB table list: " + ex.Message);
        return (LoanXDBTableList) null;
      }
    }

    internal static void RefreshReportingData(
      bool useERDB,
      ClientContext context,
      int xRefId,
      string dbTableName,
      List<LoanXDBField> tableFields,
      string[] fieldValues)
    {
      try
      {
        string encDbConnectionString = (string) null;
        if (useERDB)
          encDbConnectionString = ERDBSession.GetDbConnectionString(false, context);
        LoanUtil.RefreshReportingData(ERDBSession.GetDbConnectionString(useERDB, context), context.Settings.DbServerType, xRefId, dbTableName, tableFields, fieldValues, encDbConnectionString);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ERDBSession), "Error refreshing reporting data: " + ex.Message);
        throw;
      }
    }

    internal static LoanReportGeneratorUtil.ReturnResult GenerateFromReportingDb(
      bool useERDB,
      ClientContext context,
      UserInfo currentUser,
      LoanQuery queryEngine,
      Dictionary<string, PipelineInfo> pinfos,
      Dictionary<string, string[]> loanData,
      LoanReportParameters parameters,
      string identityQuery,
      IServerProgressFeedback feedback)
    {
      try
      {
        string connectionString;
        if (useERDB && context.UseERDB)
        {
          connectionString = ERDBSession.getERDBConnectionString(context);
        }
        else
        {
          connectionString = context.Settings.GetSqlConnectionString(Utils.EnableReadReplicaForModule);
          if (context.Settings.IsReadReplicaUseAGListener)
            DiagUtility.LogManager.GetLogger("SQL.ReadReplica").Write(Encompass.Diagnostics.Logging.LogLevel.INFO, nameof (ERDBSession), "GenerateFromReportingDb by UserId: " + currentUser.Userid);
        }
        return LoanReportGeneratorUtil.GenerateFromReportingDb(connectionString, context.Settings.DbServerType, queryEngine, pinfos, loanData, parameters, identityQuery, feedback);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ERDBSession), "Error generating report: " + ex.Message);
        throw;
      }
    }

    internal static void ApplyReportingDatabaseChanges(
      bool useERDB,
      ClientContext context,
      LoanXDBTableList tableList,
      string updateQuery,
      string userId)
    {
      try
      {
        string connectionString = ERDBSession.GetDbConnectionString(useERDB, context);
        string empty = string.Empty;
        string str = !useERDB ? LoanXDBStoreUtil.BuildDbLoanXDBTableListUpdateScript(connectionString, context.Settings.DbServerType, tableList, userId) : LoanXDBStoreUtil.BuildDbLoanXDBTableListUpdateScriptForErdb(connectionString, context.Settings.DbServerType, tableList);
        updateQuery += str;
        if (!LoanXDBStoreUtil.ApplyChangesToDb(connectionString, updateQuery))
          return;
        tableList.ResetLastModifiedDate();
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ERDBSession), "Error applying reporting database changes: " + ex.Message);
        throw ex;
      }
    }

    internal static void ClearDbPendingFields(bool useERDB, ClientContext context, string userId)
    {
      try
      {
        LoanXDBStoreUtil.ClearDbPendingFields(ERDBSession.GetDbConnectionString(useERDB, context), useERDB, userId);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ERDBSession), "Error clearing reporting DB pending fields: " + ex.Message);
        throw ex;
      }
    }

    internal static void ReleaseLock(bool useERDB, ClientContext context, bool updateTimestamp)
    {
      try
      {
        LoanXDBStoreUtil.ReleaseLock(ERDBSession.GetDbConnectionString(useERDB, context), context.Settings.DbServerType, updateTimestamp);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ERDBSession), "Error releasing reporting DB lock: " + ex.Message);
        throw ex;
      }
    }

    internal static int AcquireLock(bool useERDB, ClientContext context, string validationKey)
    {
      try
      {
        return LoanXDBStoreUtil.AcquireLock(ERDBSession.GetDbConnectionString(useERDB, context), context.Settings.DbServerType, validationKey);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ERDBSession), "Error acquiring reporting DB lock: " + ex.Message);
        return -1;
      }
    }

    internal static void ResetReportingDatabase(
      bool useERDB,
      ClientContext context,
      LoanXDBTableList tableList,
      bool keepTables,
      IServerProgressFeedback feedback)
    {
      try
      {
        LoanXDBStoreUtil.ResetReportingDatabase(ERDBSession.GetDbConnectionString(useERDB, context), context.Settings.DbServerType, tableList, keepTables, feedback);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ERDBSession), "Error reseting reporting database: " + ex.Message);
        throw ex;
      }
    }

    internal static void DeleteLoanFromDatabase(
      bool useERDB,
      ClientContext context,
      string loanGUID,
      int xrefId,
      bool deleteAllData)
    {
      try
      {
        LoanUtil.DeleteLoanFromDatabase(useERDB, ERDBSession.GetDbConnectionString(useERDB, context), context.Settings.DbServerType, loanGUID, xrefId, deleteAllData);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ERDBSession), "Error deleting loan from database: " + ex.Message);
        throw ex;
      }
    }

    internal static void DeleteLoanFromXDB(
      bool useERDB,
      ClientContext context,
      string loanGUID,
      int xrefId,
      string[] tableNames)
    {
      try
      {
        LoanUtil.DeleteLoanFromXDB(ERDBSession.GetDbConnectionString(useERDB, context), loanGUID, xrefId, tableNames);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ERDBSession), "Error deleting loan from XDB database: " + ex.Message);
        throw ex;
      }
    }

    internal static void InsertIntoERDBJobQueue(
      ClientContext context,
      string loanGUID,
      string loanFolder,
      string loanName,
      int xRefID,
      bool isArchive,
      DateTime lastModified,
      bool wakeUpERDBJobProcessor)
    {
      if (!context.UseERDB)
        return;
      try
      {
        ERDBJobQueueAccessor.Enqueue(context.Settings.ConnectionString, loanGUID, loanFolder, loanName, xRefID, isArchive, lastModified);
        TraceLog.WriteVerbose(nameof (ERDBSession), "ERDBJobQueue enqueue done");
        context.TraceLog.WriteVerbose(nameof (ERDBSession), "ERDBJobQueue enqueue done");
      }
      catch (Exception ex)
      {
        string message = "Unable to insert request to ERDBJobQueue of the Encompass SQL database: " + ex.Message;
        TraceLog.WriteError(nameof (ERDBSession), message);
        context.TraceLog.WriteError(nameof (ERDBSession), message);
      }
      if (!wakeUpERDBJobProcessor)
        return;
      ERDBSession.WakeUpJobProcessor(context);
    }

    public static void WakeUpJobProcessor(ClientContext context)
    {
      string message = "Creating WakeUpJobProcessor thread.";
      TraceLog.WriteVerbose(nameof (ERDBSession), message);
      context.TraceLog.WriteVerbose(nameof (ERDBSession), message);
      new Thread(new ParameterizedThreadStart(ERDBSession.wakeUpJobProcessor)).Start((object) context);
    }

    private static void wakeUpJobProcessor(object param)
    {
      ClientContext context = param as ClientContext;
      using (context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        if (!context.UseERDB)
          return;
        string message1 = "Try waking up job processor.";
        TraceLog.WriteVerbose(nameof (ERDBSession), message1);
        context.TraceLog.WriteVerbose(nameof (ERDBSession), message1);
        int num = 2;
        while (num > 0)
        {
          --num;
          try
          {
            string appSetting = EnConfigurationSettings.AppSettings["ERDBWebServiceUrl"];
            context.ERDBSession.WakeUpJobProcessor(appSetting);
          }
          catch (ServerDataException ex)
          {
            string upper = ex.Message.ToUpper();
            if (upper.IndexOf("SQL") >= 0 || upper.IndexOf("QUERY") >= 0 || upper.IndexOf("CONNECTION") >= 0)
            {
              ERDBSession.handleConnectionError(context, false);
              context.TraceLog.WriteError(nameof (ERDBSession), "Error waking up loan job processor: " + ex.Message);
              break;
            }
            context.TraceLog.WriteError(nameof (ERDBSession), "Unknown error while waking up loan job processor: " + ex.Message);
            break;
          }
          catch (Exception ex)
          {
            if (num > 0)
            {
              context.ResetRemotingInterfaces();
            }
            else
            {
              ERDBSession.handleConnectionError(context, true);
              string message2 = "Unhandled exception happened while waking up ERDB job processor: " + ex.Message;
              TraceLog.WriteError(nameof (ERDBSession), message2);
              context.TraceLog.WriteError(nameof (ERDBSession), message2);
              break;
            }
          }
        }
      }
    }

    private static void handleConnectionError(ClientContext context, bool erdbAppServerIssue)
    {
      if (erdbAppServerIssue)
        context.ResetRemotingInterfaces();
      lock (ERDBSession.notificationLock)
      {
        IDictionary serverSettings = context.Settings.GetServerSettings(ERDBSession.failureNotification);
        string smtpHost = (string) serverSettings[(object) (ERDBSession.failureNotification + ".SMTPServer")];
        int smtpPort = (int) serverSettings[(object) (ERDBSession.failureNotification + ".SMTPPort")];
        string smtpUserName = (string) serverSettings[(object) (ERDBSession.failureNotification + ".SMTPUserName")];
        string smtpPassword = XT.DSB64((string) serverSettings[(object) (ERDBSession.failureNotification + ".SMTPPassword")], KB.KB64);
        string fromEmail = (string) serverSettings[(object) (ERDBSession.failureNotification + ".FromEmail")];
        string toEmails = (string) serverSettings[(object) (ERDBSession.failureNotification + ".Email")];
        bool enableSSL = (bool) serverSettings[(object) (ERDBSession.failureNotification + ".SMTPUseSSL")];
        int hours = (int) serverSettings[(object) (ERDBSession.failureNotification + ".EmailDeliveryInterval")];
        if (!(DateTime.Now - (DateTime) serverSettings[(object) (ERDBSession.failureNotification + ".LastNotificationEmailSent")] > new TimeSpan(hours, 0, 0)))
          return;
        try
        {
          if (!((smtpHost ?? "").Trim() != "") || !((toEmails ?? "").Trim() != ""))
            return;
          string subject = "Encompass ERDB " + (erdbAppServerIssue ? "Application" : "SQL") + " Server " + (erdbAppServerIssue ? "(" + context.ERDBServer + ") " : "") + "Failure Notification (Client ID: " + context.ERDBCache.ClientID + ")";
          string body = "Client ID: " + context.ERDBCache.ClientID + "\r\n\r\nThis is a message from Encompass. Your External Reporting Database (ERDB) " + (erdbAppServerIssue ? "application/processing" : "SQL") + " server may not be up and running. Please check the server" + (erdbAppServerIssue ? " '" + context.ERDBServer + "'" : "") + ".";
          SmtpUtil.SendMail(smtpHost, smtpPort, smtpUserName, smtpPassword, fromEmail, toEmails, subject, body, false, enableSSL);
          context.Settings.SetServerSetting(ERDBSession.failureNotification + ".LastNotificationEmailSent", (object) DateTime.Now);
        }
        catch (Exception ex)
        {
          context.TraceLog.WriteError(nameof (ERDBSession), "Error sending ERDB server failure notification email: " + ex.Message + ".");
        }
      }
    }
  }
}

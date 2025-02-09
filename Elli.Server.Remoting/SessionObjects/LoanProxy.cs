// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.LoanProxy
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.ElliEnum;
using Elli.Interface;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.SkyDrive;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using Encompass.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class LoanProxy : SessionBoundObject, ILoan, IDisposable, IRemoteCallLogDecorator
  {
    private const string className = "LoanProxy";
    private static readonly string sw = Tracing.SwDataEngine;
    private LoanManager mngr;
    private string guid;

    public LoanProxy Initialize(ISession session, string guid, LoanManager mngr)
    {
      this.InitializeInternal(session);
      this.guid = guid;
      this.mngr = mngr;
      return this;
    }

    public string Guid => this.guid;

    public IStageLoanHistoryManager StageLoanHistoryManager { get; set; }

    public virtual LoanIdentity GetIdentity()
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetIdentity), Array.Empty<object>());
      try
      {
        return this.getLatestVersion().Identity;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (LoanIdentity) null;
      }
    }

    public virtual LoanData GetLoanData(bool isExternalOrganization)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetLoanData), Array.Empty<object>());
      return this.GetLoanDataInternal(isExternalOrganization);
    }

    public virtual LoanData GetLoanDataInternal(bool isExternalOrganization)
    {
      try
      {
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("LoanProxy.GetLoanData", 89, nameof (GetLoanDataInternal), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanProxy.cs"))
        {
          performanceMeter.AddVariable("User", (object) this.Session.UserID);
          performanceMeter.AddVariable("Guid", (object) this.Guid);
          ILoanSettings loanSettings = LoanConfiguration.GetLoanSettings(this.Session.GetUserInfo());
          using (Loan latestVersion = this.getLatestVersion())
          {
            LoanData loanData = latestVersion.GetLoanData(loanSettings, false);
            if (loanData.StageHistoryManager != null)
              this.StageLoanHistoryManager = loanData.StageHistoryManager;
            string simpleField = loanData.GetSimpleField("HMDA.X100");
            if (!string.IsNullOrEmpty(simpleField))
            {
              HMDAProfile hmdaProfileById = HMDAProfileDbAccessor.GetHMDAProfileById(Utils.ParseInt((object) simpleField));
              if (hmdaProfileById != null)
              {
                loanData.Settings.HMDAInfo = new HMDAInformation(hmdaProfileById.HMDAProfileSetting);
                loanData.Settings.HMDAInfo.HMDAProfileID = hmdaProfileById.HMDAProfileID.ToString();
              }
            }
            loanData.BaseLastModified = Loan.GetLastModifiedFromDB(latestVersion.Identity.Guid);
            return loanData;
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (LoanData) null;
      }
    }

    public virtual string[] SelectFields(string[] fieldIds)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (SelectFields), new object[2]
      {
        (object) fieldIds,
        (object) this.Guid
      });
      try
      {
        using (Loan latestVersion = this.getLatestVersion())
        {
          string[] fields = latestVersion.SelectFields(fieldIds);
          this.RecordFieldSize(fields);
          return fields;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual string SelectField(string fieldId)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (SelectField), new object[1]
      {
        (object) fieldId
      });
      try
      {
        using (Loan latestVersion = this.getLatestVersion())
          return latestVersion.SelectField(fieldId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (string) null;
      }
    }

    private void RecordFieldSize(string[] fields)
    {
      int num = 0;
      for (int index = 0; index < fields.Length; ++index)
      {
        int byteCount = Encoding.Unicode.GetByteCount(fields[index]);
        num += byteCount;
      }
      if (num == 0)
        return;
      string p = "<Field Values:" + num.ToString() + " byte>";
      ClientContext.GetCurrent().AddParm((object) p);
    }

    public virtual void AddLoanEventLog(LoanEventLogList loanEventLogList)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (AddLoanEventLog), new object[1]
      {
        (object) loanEventLogList
      });
      try
      {
        UserInfo userInfo = this.Session.GetUserInfo();
        using (Loan loan = this.checkOut())
        {
          loan.AddLoanEventLog(loanEventLogList, userInfo, this.Session.SessionID);
          loan.CheckIn(userInfo, false, this.Session.SessionID);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SaveSystemLogs(LoanData data)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (SaveSystemLogs), new object[1]
      {
        (object) data
      });
      try
      {
        UserInfo userInfo = this.Session.GetUserInfo();
        using (Loan loan = this.checkOut(LoanInfo.Right.Access))
        {
          this.demandLocked(loan, LoanInfo.LockReason.OpenForWork);
          loan.Import(data, false);
          loan.CheckIn(userInfo, false, this.Session.SessionID);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SaveWithoutAuditRecord(LoanData data) => this.save(data, false, false);

    public virtual int Save(
      LoanData data,
      string clientStackTrack = "",
      bool enableRateLockValidation = false,
      bool enableBackupLoanFile = false)
    {
      return this.Save(data, false, clientStackTrack, enableRateLockValidation, enableBackupLoanFile);
    }

    public virtual int Save(
      LoanData data,
      bool unlock,
      string clientStackTrack = "",
      bool enableRateLockValidation = false,
      bool enableBackupLoanFile = false)
    {
      return this.save(data, unlock, true, clientStackTrack, enableRateLockValidation, enableBackupLoanFile);
    }

    private int save(
      LoanData data,
      bool unlock,
      bool recordAuditRecord,
      string clientStackTrack = "",
      bool enableRateLockValidation = false,
      bool enableBackupLoanFile = false)
    {
      int fileVersionNumber = -1;
      this.onApiCalled(nameof (LoanProxy), "Save", new object[3]
      {
        (object) data,
        (object) unlock,
        (object) this.Guid
      });
      try
      {
        enableRateLockValidation = true;
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("LoanProxy.SaveLoan", 271, nameof (save), "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\LoanProxy.cs"))
        {
          performanceMeter.AddVariable("User", (object) this.Session.UserID);
          performanceMeter.AddVariable("Guid", (object) this.Guid);
          ILoanSnapshotProvider provider = data.SnapshotProvider ?? (ILoanSnapshotProvider) new LoanSnapshotProvider(this);
          DateTime autoSaveDateTime = data.AutoSaveDateTime;
          bool isAutoSaveFlag = data.IsAutoSaveFlag;
          data.Parse();
          LoanData data1 = (LoanData) data.Clone();
          if (data.StageHistoryManager != null)
            data1.AttachStageLoanHistoryManager(data.StageHistoryManager);
          else if (this.StageLoanHistoryManager != null)
            data1.AttachStageLoanHistoryManager(this.StageLoanHistoryManager);
          data1.AttachSnapshotProvider(provider);
          if (isAutoSaveFlag)
          {
            data1.IsAutoSaveFlag = isAutoSaveFlag;
            data1.AutoSaveDateTime = autoSaveDateTime;
          }
          UserInfo userInfo = this.Session.GetUserInfo();
          LoanIdentity id = (LoanIdentity) null;
          using (Loan loan = this.checkOut(LoanInfo.Right.Access))
          {
            this.demandLocked(loan, LoanInfo.LockReason.OpenForWork);
            string loanFolder = loan.Identity.LoanFolder;
            string loanName = loan.Identity.LoanName;
            DirectoryInfo directoryInfo = new DirectoryInfo(ClientContext.GetCurrent().Settings.GetLoanFolderPath(loanFolder, loanName, false));
            if (!Directory.Exists(directoryInfo.ToString()))
              throw new Exception("File does not exist in the same location");
            if (ClientContext.GetCurrent().AllowConcurrentEditing)
            {
              try
              {
                if (this.GetPipelineInfo(false).LastModified > data1.BaseLastModified)
                  Err.Raise(TraceLevel.Info, nameof (LoanProxy), (ServerException) new ObjectModifiedException("Loan file has been modified by another user and cannot be updated."));
              }
              catch (ObjectNotFoundException ex)
              {
              }
            }
            try
            {
              if (data1.ClearAIQIncomeAnalyzerAlert)
                ((IConfigurationManager) this.Session.GetObject("ConfigurationManager"))?.DeleteEPassMessages(new string[1]
                {
                  data1.AiqIncomeEpassMessageID
                });
            }
            catch (Exception ex)
            {
              Tracing.Log(LoanProxy.sw, nameof (LoanProxy), TraceLevel.Warning, "UserID:" + this.Session.UserID + "; Deleting Income Analyzer green alert" + ex.Message);
            }
            DisclosureTrackingLogException ex1 = (DisclosureTrackingLogException) null;
            bool serverSetting = (bool) this.Session.Context.Settings.GetServerSetting("Policies.AutoRecoverDTLogs", true);
            bool flag1 = false;
            string msg = "";
            bool flag2 = false;
            try
            {
              List<IDisclosureTracking2015Log> source1 = new List<IDisclosureTracking2015Log>((IEnumerable<IDisclosureTracking2015Log>) loan.GetLoanData(data1.Settings).GetLogList().GetAllIDisclosureTracking2015Log(false));
              List<IDisclosureTracking2015Log> clientList = new List<IDisclosureTracking2015Log>((IEnumerable<IDisclosureTracking2015Log>) data1.GetLogList().GetAllIDisclosureTracking2015Log(false));
              Func<IDisclosureTracking2015Log, bool> predicate = (Func<IDisclosureTracking2015Log, bool>) (x => !clientList.Contains(x));
              IEnumerable<IDisclosureTracking2015Log> source2 = source1.Where<IDisclosureTracking2015Log>(predicate);
              if (source2 != null)
              {
                if (source2.Count<IDisclosureTracking2015Log>() > 0)
                {
                  enableBackupLoanFile = true;
                  ex1 = DisclosureTrackingLogException.GetDisclosureTracking2015LogException(new List<IDisclosureTracking2015Log>((IEnumerable<IDisclosureTracking2015Log>) source2.ToArray<IDisclosureTracking2015Log>()));
                  string message = "UserID:" + this.Session.UserID + "; Instance ID:" + ClientContext.GetCurrent().ClientID + "; Server loanGUID: " + loan.GetLoanData(data1.Settings).GUID + "; Server loanNumber:" + loan.GetLoanData(data1.Settings).LoanNumber + "; Client loanNumber:" + data1.LoanNumber + "; Disclosure Tracking Records Recovered :" + Environment.NewLine + ex1.MissingIDisclosureTracking2015LogData();
                  msg = "UserID:" + this.Session.UserID + "; Instance ID:" + ClientContext.GetCurrent().ClientID + "; Server loanGUID: " + loan.GetLoanData(data1.Settings).GUID + "; Server loanNumber:" + loan.GetLoanData(data1.Settings).LoanNumber + "; Client loanNumber:" + data1.LoanNumber + "; Disclosure Tracking Records Dropped :" + Environment.NewLine + ex1.MissingIDisclosureTracking2015LogData();
                  if (serverSetting)
                  {
                    foreach (LogRecordBase rec in source2)
                    {
                      data.GetLogList().AddRecord(rec);
                      data1.GetLogList().AddRecord(rec);
                    }
                    flag2 = true;
                    Err.Raise(TraceLevel.Warning, nameof (LoanProxy), (ServerException) new ServerDataException(message));
                  }
                  else
                    flag1 = (bool) this.Session.Context.Settings.GetServerSetting("Policies.CheckDisclosureTracking", true);
                }
              }
            }
            catch (Exception ex2)
            {
              Tracing.Log(LoanProxy.sw, nameof (LoanProxy), TraceLevel.Warning, "UserID:" + this.Session.UserID + "; Saving Disclosure Tracking Logs" + ex2.Message);
            }
            if (flag1 && ex1 != null)
              Err.Raise(TraceLevel.Error, nameof (LoanProxy), (ServerException) ex1);
            if (enableRateLockValidation)
            {
              string str = "";
              try
              {
                List<LockRequestLog> source3 = new List<LockRequestLog>((IEnumerable<LockRequestLog>) loan.GetLoanData(data1.Settings).GetLogList().GetAllLockRequests(true));
                List<LockRequestLog> clientRequest = new List<LockRequestLog>((IEnumerable<LockRequestLog>) data1.GetLogList().GetAllLockRequests(true));
                Func<LockRequestLog, bool> predicate1 = (Func<LockRequestLog, bool>) (x => !clientRequest.Contains(x));
                IEnumerable<LockRequestLog> source4 = source3.Where<LockRequestLog>(predicate1);
                if (source4 != null && source4.Count<LockRequestLog>() > 0)
                {
                  XmlDocument xmlDocument = new XmlDocument();
                  foreach (LockRequestLog rec in source4.ToArray<LockRequestLog>())
                  {
                    XmlElement element = xmlDocument.CreateElement("LockRequestLog");
                    rec.ToXml(element);
                    str = str + Environment.NewLine + element.OuterXml;
                    data.GetLogList().AddRecord((LogRecordBase) rec);
                    data1.GetLogList().AddRecord((LogRecordBase) rec);
                  }
                }
                List<LockConfirmLog> source5 = new List<LockConfirmLog>((IEnumerable<LockConfirmLog>) loan.GetLoanData(data1.Settings).GetLogList().GetAllConfirmLocks());
                List<LockConfirmLog> clientConfirm = new List<LockConfirmLog>((IEnumerable<LockConfirmLog>) data1.GetLogList().GetAllConfirmLocks());
                Func<LockConfirmLog, bool> predicate2 = (Func<LockConfirmLog, bool>) (x => !clientConfirm.Contains(x));
                IEnumerable<LockConfirmLog> source6 = source5.Where<LockConfirmLog>(predicate2);
                if (source6 != null && source6.Count<LockConfirmLog>() > 0)
                {
                  XmlDocument xmlDocument = new XmlDocument();
                  foreach (LockConfirmLog rec in source6.ToArray<LockConfirmLog>())
                  {
                    XmlElement element = xmlDocument.CreateElement("LockConfirmLog");
                    rec.ToXml(element);
                    str = str + Environment.NewLine + element.OuterXml;
                    data.GetLogList().AddRecord((LogRecordBase) rec);
                    data1.GetLogList().AddRecord((LogRecordBase) rec);
                  }
                }
                List<LockCancellationLog> source7 = new List<LockCancellationLog>((IEnumerable<LockCancellationLog>) loan.GetLoanData(data1.Settings).GetLogList().GetAllCancellationLocks());
                List<LockCancellationLog> clientCancel = new List<LockCancellationLog>((IEnumerable<LockCancellationLog>) data1.GetLogList().GetAllCancellationLocks());
                Func<LockCancellationLog, bool> predicate3 = (Func<LockCancellationLog, bool>) (x => !clientCancel.Contains(x));
                IEnumerable<LockCancellationLog> source8 = source7.Where<LockCancellationLog>(predicate3);
                if (source8 != null && source8.Count<LockCancellationLog>() > 0)
                {
                  XmlDocument xmlDocument = new XmlDocument();
                  foreach (LockCancellationLog rec in source8.ToArray<LockCancellationLog>())
                  {
                    XmlElement element = xmlDocument.CreateElement("LockCancellationLog");
                    rec.ToXml(element);
                    str = str + Environment.NewLine + element.OuterXml;
                    data.GetLogList().AddRecord((LogRecordBase) rec);
                    data1.GetLogList().AddRecord((LogRecordBase) rec);
                  }
                }
                List<LockDenialLog> source9 = new List<LockDenialLog>((IEnumerable<LockDenialLog>) loan.GetLoanData(data1.Settings).GetLogList().GetAllDenialLockLog());
                List<LockDenialLog> clientDenial = new List<LockDenialLog>((IEnumerable<LockDenialLog>) data1.GetLogList().GetAllDenialLockLog());
                Func<LockDenialLog, bool> predicate4 = (Func<LockDenialLog, bool>) (x => !clientDenial.Contains(x));
                IEnumerable<LockDenialLog> source10 = source9.Where<LockDenialLog>(predicate4);
                if (source10 != null && source10.Count<LockDenialLog>() > 0)
                {
                  XmlDocument xmlDocument = new XmlDocument();
                  foreach (LockDenialLog rec in source10.ToArray<LockDenialLog>())
                  {
                    XmlElement element = xmlDocument.CreateElement("LockDenialLog");
                    rec.ToXml(element);
                    str = str + Environment.NewLine + element.OuterXml;
                    data.GetLogList().AddRecord((LogRecordBase) rec);
                    data1.GetLogList().AddRecord((LogRecordBase) rec);
                  }
                }
                List<LockRemovedLog> source11 = new List<LockRemovedLog>((IEnumerable<LockRemovedLog>) loan.GetLoanData(data1.Settings).GetLogList().GetAllRemovalLocks());
                List<LockRemovedLog> clientRemoved = new List<LockRemovedLog>((IEnumerable<LockRemovedLog>) data1.GetLogList().GetAllRemovalLocks());
                Func<LockRemovedLog, bool> predicate5 = (Func<LockRemovedLog, bool>) (x => !clientRemoved.Contains(x));
                IEnumerable<LockRemovedLog> source12 = source11.Where<LockRemovedLog>(predicate5);
                if (source12 != null && source12.Count<LockRemovedLog>() > 0)
                {
                  XmlDocument xmlDocument = new XmlDocument();
                  foreach (LockRemovedLog rec in source12.ToArray<LockRemovedLog>())
                  {
                    XmlElement element = xmlDocument.CreateElement("LockRemovedLog");
                    rec.ToXml(element);
                    str = str + Environment.NewLine + element.OuterXml;
                    data.GetLogList().AddRecord((LogRecordBase) rec);
                    data1.GetLogList().AddRecord((LogRecordBase) rec);
                  }
                }
                List<LockValidationLog> source13 = new List<LockValidationLog>((IEnumerable<LockValidationLog>) loan.GetLoanData(data1.Settings).GetLogList().GetAllValidationLocks());
                List<LockValidationLog> clientValidation = new List<LockValidationLog>((IEnumerable<LockValidationLog>) data1.GetLogList().GetAllValidationLocks());
                Func<LockValidationLog, bool> predicate6 = (Func<LockValidationLog, bool>) (x => !clientValidation.Contains(x));
                IEnumerable<LockValidationLog> source14 = source13.Where<LockValidationLog>(predicate6);
                if (source14 != null && source14.Count<LockValidationLog>() > 0)
                {
                  XmlDocument xmlDocument = new XmlDocument();
                  foreach (LockValidationLog rec in source14.ToArray<LockValidationLog>())
                  {
                    XmlElement element = xmlDocument.CreateElement("LockValidationLog");
                    rec.ToXml(element);
                    str = str + Environment.NewLine + element.OuterXml;
                    data.GetLogList().AddRecord((LogRecordBase) rec);
                    data1.GetLogList().AddRecord((LogRecordBase) rec);
                  }
                }
                List<LockVoidLog> source15 = new List<LockVoidLog>((IEnumerable<LockVoidLog>) loan.GetLoanData(data1.Settings).GetLogList().GetAllVoidLocks());
                List<LockVoidLog> clientVoid = new List<LockVoidLog>((IEnumerable<LockVoidLog>) data1.GetLogList().GetAllVoidLocks());
                Func<LockVoidLog, bool> predicate7 = (Func<LockVoidLog, bool>) (x => !clientVoid.Contains(x));
                IEnumerable<LockVoidLog> source16 = source15.Where<LockVoidLog>(predicate7);
                if (source16 != null && source16.Count<LockVoidLog>() > 0)
                {
                  XmlDocument xmlDocument = new XmlDocument();
                  foreach (LockVoidLog rec in source16.ToArray<LockVoidLog>())
                  {
                    XmlElement element = xmlDocument.CreateElement("LockVoidLog");
                    rec.ToXml(element);
                    str = str + Environment.NewLine + element.OuterXml;
                    data.GetLogList().AddRecord((LogRecordBase) rec);
                    data1.GetLogList().AddRecord((LogRecordBase) rec);
                  }
                }
                if (!string.IsNullOrEmpty(str))
                {
                  string stackTrace = Tracing.GetStackTrace();
                  StringBuilder stringBuilder = new StringBuilder();
                  stringBuilder.AppendLine("UserID:" + this.Session.UserID + "; Dropped Lock Request and Dropped Lock Confirm Log:");
                  stringBuilder.AppendLine(str);
                  stringBuilder.AppendLine("Server Stack Trace:");
                  stringBuilder.AppendLine(stackTrace);
                  stringBuilder.AppendLine("Client Stack Trace:");
                  stringBuilder.AppendLine(clientStackTrack);
                  Tracing.Log(LoanProxy.sw, nameof (LoanProxy), TraceLevel.Error, stringBuilder.ToString());
                  TraceLog.WriteError(nameof (LoanProxy), stringBuilder.ToString());
                  enableBackupLoanFile = true;
                }
              }
              catch (Exception ex3)
              {
                Tracing.Log(LoanProxy.sw, nameof (LoanProxy), TraceLevel.Error, "UserID:" + this.Session.UserID + "; Error validating disclosure tracking logs:" + ex3.Message);
              }
            }
            loan.Import(data1, false);
            if (unlock)
              loan.Unlock(this.Session.SessionID);
            id = loan.Identity;
            DateTime now;
            if (enableBackupLoanFile)
            {
              string fullName = directoryInfo.FullName;
              now = DateTime.Now;
              string targetFileName = "loan_beforesave_" + now.ToString("MMddyyyyhhmmss") + ".em";
              this.BackupLoanFile(fullName, targetFileName);
            }
            bool allowDeferrable = true;
            if (this.mngr != null)
              allowDeferrable = this.mngr.AllowDeferrable();
            fileVersionNumber = loan.CheckIn(userInfo, false, this.Session.SessionID, allowDeferrable);
            if (fileVersionNumber <= 0)
              return fileVersionNumber;
            if (msg != "" && !flag2)
              Tracing.Log(LoanProxy.sw, nameof (LoanProxy), TraceLevel.Error, msg);
            if (enableBackupLoanFile)
            {
              string fullName = directoryInfo.FullName;
              now = DateTime.Now;
              string targetFileName = "loan_aftersave_" + now.ToString("MMddyyyyhhmmss") + ".em";
              this.BackupLoanFile(fullName, targetFileName);
            }
          }
          if (recordAuditRecord)
          {
            LoanFileAuditRecord record = new LoanFileAuditRecord(this.Session.UserID, userInfo.FullName, ActionType.LoanModified, DateTime.Now, data1.GUID, id.LoanFolder, data1.LoanNumber, data1.GetField("37"), data1.GetField("36"), data1.GetField("11") + " " + data1.GetField("12") + ", " + data1.GetField("14") + " " + data1.GetField("15"), data1.GetField("2024"), this.Session.LoginParams.AppName, fileVersionNumber);
            if (data1.IsAutoSaveFlag)
              record.AutoSaveDateTime = data1.AutoSaveDateTime;
            SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) record);
          }
          this.raiseEvent(LoanEventType.Saved, id);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
      return fileVersionNumber;
    }

    private void BackupLoanFile(string loanFolder, string targetFileName)
    {
      try
      {
        string path1 = loanFolder;
        string str = loanFolder + "\\BackupForRecapture";
        if (!Directory.Exists(str))
          Directory.CreateDirectory(str);
        File.Copy(Path.Combine(path1, "loan.em"), Path.Combine(str, targetFileName), true);
      }
      catch (Exception ex)
      {
        TraceLog.WriteVerbose(nameof (LoanProxy), "Error on BackupLoanFile(" + targetFileName + "): " + ex.Message);
      }
    }

    public virtual void Delete(bool isExternalOrganization)
    {
      this.Delete(true, isExternalOrganization);
    }

    public virtual void Delete(bool demandAdmin, bool isExternalOrganization)
    {
      this.Delete(demandAdmin, false, isExternalOrganization);
    }

    public virtual void Delete(
      bool demandAdmin,
      bool skipSystemLogging,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (Delete), Array.Empty<object>());
      try
      {
        if (demandAdmin)
        {
          UserInfo userInfo = this.Session.GetUser().GetUserInfo();
          if ((!FeaturesAclDbAccessor.CheckPermission(AclFeature.LoanMgmt_TF_Delete, userInfo) ? 0 : (FeaturesAclDbAccessor.CheckPermission(AclFeature.LoanMgmt_Delete, userInfo) ? 1 : 0)) == 0)
            this.Security.Demand(false);
        }
        UserInfo userInfo1 = this.Session.GetUserInfo();
        LoanIdentity id = (LoanIdentity) null;
        using (Loan loan = this.checkOut(LoanInfo.Right.Access))
        {
          this.demandUnlocked(loan);
          id = loan.Identity;
          string guid = loan.LoanData.GUID;
          string field1 = loan.LoanData.GetField("37");
          string field2 = loan.LoanData.GetField("2024");
          string loanNumber = loan.LoanData.LoanNumber;
          string field3 = loan.LoanData.GetField("36");
          string loanFolder = loan.Identity.LoanFolder;
          string address = loan.LoanData.GetField("11") + " " + loan.LoanData.GetField("12") + ", " + loan.LoanData.GetField("14") + " " + loan.LoanData.GetField("15");
          loan.Delete(false, userInfo1, isExternalOrganization);
          if (!skipSystemLogging)
            SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new LoanFileAuditRecord(this.Session.UserID, userInfo1.FullName, ActionType.LoanPermanentDeleted, DateTime.Now, guid, loanFolder, loanNumber, field1, field3, address, field2, this.Session.LoginParams.AppName));
        }
        this.raiseEvent(LoanEventType.Deleted, id);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual void Close() => this.Close(false);

    public virtual void Close(bool unlock)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (Close), new object[2]
      {
        (object) unlock,
        (object) this.Guid
      });
      try
      {
        if (unlock)
        {
          UserInfo userInfo = this.Session.GetUserInfo();
          using (Loan loan = this.checkOut())
          {
            if (loan.IsLockedBySession(this.Session.SessionID))
            {
              loan.Unlock(this.Session.SessionID);
              loan.CheckIn(userInfo, false, this.Session.SessionID, true);
            }
          }
        }
        this.raiseEvent(LoanEventType.Completed, this.getLatestVersion().Identity);
        base.Dispose();
      }
      catch (Exception ex)
      {
        TraceLog.WriteException(nameof (LoanProxy), ex);
      }
    }

    public virtual bool SentToProcessing()
    {
      this.onApiCalled(nameof (LoanProxy), nameof (SentToProcessing), Array.Empty<object>());
      try
      {
        using (Loan latestVersion = this.getLatestVersion())
          return latestVersion.SentToProcessing();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual PipelineInfo GetPipelineInfo(bool isExternalOrganization)
    {
      return this.GetPipelineInfo(isExternalOrganization, 0);
    }

    public virtual PipelineInfo GetPipelineInfo(bool isExternalOrganization, int sqlRead)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetPipelineInfo), Array.Empty<object>());
      return this.GetPipelineInfoInternal(isExternalOrganization, sqlRead);
    }

    public virtual PipelineInfo GetPipelineInfoInternal(bool isExternalOrganization, int sqlRead)
    {
      try
      {
        using (Loan latestVersion = this.getLatestVersion())
          return latestVersion.GetPipelineInfo(this.Session.GetUserInfo(), isExternalOrganization, sqlRead);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (PipelineInfo) null;
      }
    }

    public virtual PipelineInfo GetPipelineInfo(
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetPipelineInfo), new object[2]
      {
        (object) fields,
        (object) dataToInclude
      });
      try
      {
        using (Loan latestVersion = this.getLatestVersion())
          return latestVersion.GetPipelineInfo(this.Session.GetUserInfo(), fields, dataToInclude, isExternalOrganization);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (PipelineInfo) null;
      }
    }

    public override void Dispose() => this.Close();

    public virtual EllieMae.EMLite.ClientServer.LoanAssociateInfo[] GetLoanAssociates(
      bool milestoneRolesOnly)
    {
      return this.GetLoanAssociates(milestoneRolesOnly, false);
    }

    public virtual EllieMae.EMLite.ClientServer.LoanAssociateInfo[] GetLoanAssociates(
      bool milestoneRolesOnly,
      bool resolveUsers)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetLoanAssociates), new object[2]
      {
        (object) milestoneRolesOnly,
        (object) resolveUsers
      });
      try
      {
        return Loan.GetLoanAssociates(this.guid, milestoneRolesOnly, resolveUsers);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (EllieMae.EMLite.ClientServer.LoanAssociateInfo[]) null;
      }
    }

    public virtual LoanInfo.Right GetRights() => this.GetRights(this.Session.UserID);

    public virtual LoanInfo.Right GetRights(string userId)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetRights), new object[1]
      {
        (object) userId
      });
      try
      {
        return ((SecurityManager) this.Security).GetEffectiveLoanRights(UserStore.GetLatestVersion(userId).UserInfo, this.getLatestVersion());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return LoanInfo.Right.NoRight;
      }
    }

    public virtual LoanInfo.Right GetAssignedRights()
    {
      return this.GetAssignedRights(this.Session.UserID);
    }

    public virtual LoanInfo.Right GetAssignedRights(string userId)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetAssignedRights), new object[1]
      {
        (object) userId
      });
      try
      {
        return this.getLatestVersion().GetRightsForUser(userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return LoanInfo.Right.NoRight;
      }
    }

    public virtual bool HasRights(LoanInfo.Right rights)
    {
      return this.HasRights(this.Session.UserID, rights);
    }

    public virtual bool HasRights(string userId, LoanInfo.Right rights)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (HasRights), new object[2]
      {
        (object) userId,
        (object) rights
      });
      try
      {
        return (((SecurityManager) this.Security).GetEffectiveLoanRights(UserStore.GetLatestVersion(this.Session.SessionID).UserInfo, this.getLatestVersion()) & rights) == rights;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool HasAssignedRights(LoanInfo.Right rights)
    {
      return this.HasAssignedRights(this.Session.UserID, rights);
    }

    public virtual bool HasAssignedRights(string userId, LoanInfo.Right rights)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (HasAssignedRights), new object[2]
      {
        (object) userId,
        (object) rights
      });
      try
      {
        return (this.getLatestVersion().GetRightsForUser(userId) & rights) == rights;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual Hashtable GetAllAssignedRights()
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetAllAssignedRights), Array.Empty<object>());
      try
      {
        return this.getLatestVersion().GetAllUserRights();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual void AddRights(string userId, LoanInfo.Right rights)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (AddRights), new object[2]
      {
        (object) userId,
        (object) rights
      });
      try
      {
        UserInfo userInfo = this.Session.GetUserInfo();
        using (Loan loan = this.checkOut(LoanInfo.Right.Assignment))
        {
          loan.AddRightsForUser(userId, rights);
          loan.CheckIn(userInfo, false, this.Session.SessionID);
          this.raiseEvent(LoanEventType.PermissionsChanged, loan.Identity);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex);
      }
    }

    public virtual void RemoveRights(string userId, LoanInfo.Right rights)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (RemoveRights), new object[2]
      {
        (object) userId,
        (object) rights
      });
      try
      {
        UserInfo userInfo = this.Session.GetUserInfo();
        using (Loan loan = this.checkOut(LoanInfo.Right.Assignment))
        {
          loan.RemoveRightsForUser(userId, rights);
          loan.CheckIn(userInfo, false, this.Session.SessionID);
          this.raiseEvent(LoanEventType.PermissionsChanged, loan.Identity);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex);
      }
    }

    public virtual void SetRights(string userId, LoanInfo.Right rights)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (SetRights), new object[2]
      {
        (object) userId,
        (object) rights
      });
      try
      {
        UserInfo userInfo = this.Session.GetUserInfo();
        using (Loan loan = this.checkOut(LoanInfo.Right.Assignment))
        {
          loan.SetRightsForUser(userId, rights);
          loan.CheckIn(userInfo, false, this.Session.SessionID);
          this.raiseEvent(LoanEventType.PermissionsChanged, loan.Identity);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex);
      }
    }

    public virtual void AddIntoRights(Hashtable rights)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (AddIntoRights), new object[1]
      {
        (object) rights
      });
      try
      {
        UserInfo userInfo = this.Session.GetUserInfo();
        using (Loan loan = this.checkOut(LoanInfo.Right.Assignment))
        {
          loan.AddIntoUserRights(rights);
          loan.CheckIn(userInfo, false, this.Session.SessionID);
          this.raiseEvent(LoanEventType.PermissionsChanged, loan.Identity);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual void RemoveFromRights(Hashtable rights)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (RemoveFromRights), new object[1]
      {
        (object) rights
      });
      try
      {
        UserInfo userInfo = this.Session.GetUserInfo();
        using (Loan loan = this.checkOut(LoanInfo.Right.Assignment))
        {
          loan.RemoveFromUserRights(rights);
          loan.CheckIn(userInfo, false, this.Session.SessionID);
          this.raiseEvent(LoanEventType.PermissionsChanged, loan.Identity);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual void ReplaceAllRights(Hashtable rights)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (ReplaceAllRights), new object[1]
      {
        (object) rights
      });
      try
      {
        UserInfo userInfo = this.Session.GetUserInfo();
        using (Loan loan = this.checkOut(LoanInfo.Right.Assignment))
        {
          loan.ReplaceAllUserRights(rights);
          loan.CheckIn(userInfo, false, this.Session.SessionID);
          this.raiseEvent(LoanEventType.PermissionsChanged, loan.Identity);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    private int getMaxRecentLoans(IClientContext context)
    {
      object serverSetting = context.Cache.Get("MaxRecentLoans");
      if (serverSetting == null)
      {
        serverSetting = context.Settings.GetServerSetting("Internal.MaxRecentLoans");
        context.Cache.Put("MaxRecentLoans", (object) (int) serverSetting);
      }
      return (int) serverSetting;
    }

    public virtual void Lock(LoanInfo.LockReason reason, LockInfo.ExclusiveLock exclusive)
    {
      this.Lock(reason, exclusive, false);
    }

    public virtual void Lock(
      LoanInfo.LockReason reason,
      LockInfo.ExclusiveLock exclusive,
      bool addToRecentLoans)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (Lock), new object[2]
      {
        (object) reason,
        (object) this.Guid
      });
      this.LockInternal(reason, exclusive, addToRecentLoans);
    }

    public virtual void LockInternal(
      LoanInfo.LockReason reason,
      LockInfo.ExclusiveLock exclusive,
      bool addToRecentLoans)
    {
      try
      {
        if (LoanLockAccessor.IsLoanLockDbEnabled && !ClientContext.GetCurrent().AllowConcurrentEditing)
        {
          if (exclusive == LockInfo.ExclusiveLock.ExclusiveA)
            exclusive = LockInfo.ExclusiveLock.Exclusive;
          LoanLockAccessor.updateLock(new LockInfo(this.guid, this.Session.UserID, (string) null, (string) null, this.Session.SessionID, this.Session.Server.ToString(), reason, DateTime.Now, exclusive));
        }
        else
        {
          UserInfo userInfo = this.Session.GetUserInfo();
          LoanInfo.Right requiredRights = LoanInfo.Right.Access;
          if (ClientContext.GetCurrent().AllowConcurrentEditing && (exclusive == LockInfo.ExclusiveLock.ReleaseExclusive || exclusive == LockInfo.ExclusiveLock.ReleaseExclusiveA))
            requiredRights = LoanInfo.Right.Read;
          using (Loan loan = this.checkOut(requiredRights))
          {
            if (exclusive == LockInfo.ExclusiveLock.NGSharedLock)
            {
              foreach (LockInfo currentLock in loan.CurrentLocks)
              {
                if (!(currentLock.LoginSessionID == this.Session.SessionID))
                {
                  if (currentLock.Exclusive == LockInfo.ExclusiveLock.Exclusive || currentLock.Exclusive == LockInfo.ExclusiveLock.ExclusiveA || currentLock.Exclusive == LockInfo.ExclusiveLock.Both)
                    Err.Raise(TraceLevel.Info, nameof (LoanProxy), (ServerException) new ExclusiveLockException(new LockInfo[1]
                    {
                      currentLock
                    }));
                  if (currentLock.Exclusive == LockInfo.ExclusiveLock.Nonexclusive && currentLock.LockedFor != LoanInfo.LockReason.NotLocked)
                    Err.Raise(TraceLevel.Info, nameof (LoanProxy), (ServerException) new ExclusiveLockException(new LockInfo[1]
                    {
                      currentLock
                    }));
                }
              }
            }
            else if (ClientContext.GetCurrent().AllowConcurrentEditing)
            {
              foreach (LockInfo currentLock in loan.CurrentLocks)
              {
                if (!(currentLock.LoginSessionID == this.Session.SessionID) && (currentLock.Exclusive == LockInfo.ExclusiveLock.Exclusive || currentLock.Exclusive == LockInfo.ExclusiveLock.ExclusiveA || currentLock.Exclusive == LockInfo.ExclusiveLock.Both || currentLock.Exclusive == LockInfo.ExclusiveLock.NGSharedLock))
                  Err.Raise(TraceLevel.Info, nameof (LoanProxy), (ServerException) new ExclusiveLockException(new LockInfo[1]
                  {
                    currentLock
                  }));
              }
            }
            else if (loan.Locked)
            {
              LockInfo lockInfo = loan.GetLockInfo();
              if (lockInfo.Exclusive == LockInfo.ExclusiveLock.NGSharedLock)
                Err.Raise(TraceLevel.Info, nameof (LoanProxy), (ServerException) new ExclusiveLockException(new LockInfo[1]
                {
                  lockInfo
                }));
              if (!(lockInfo.LoginSessionID == this.Session.SessionID) || !(lockInfo.LockedBy == userInfo.Userid))
                Err.Raise(TraceLevel.Info, nameof (LoanProxy), (ServerException) new LockException(loan.GetLockInfo()));
            }
            this.Security.DemandAssignable();
            loan.Lock(this.Session.SessionInfo, reason, exclusive);
            loan.SaveLoanData = false;
            loan.CheckIn(userInfo, false, this.Session.SessionID);
            this.raiseEvent(LoanEventType.Locked, loan.Identity);
          }
          int maxRecentLoans = this.getMaxRecentLoans((IClientContext) ClientContext.GetCurrent());
          if (!addToRecentLoans)
            return;
          if (maxRecentLoans <= 0)
          {
            using (User user = UserStore.CheckOut(userInfo.Userid))
            {
              user.AddToRecentLoans(this.guid);
              user.CheckIn(this.Session.UserID, false);
            }
          }
          else
          {
            using (User latestVersion = UserStore.GetLatestVersion(this.Session.UserID))
              latestVersion.AddRemoveRecentLoans(this.guid, maxRecentLoans);
          }
        }
      }
      catch (ExclusiveLockException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual void Unlock() => this.Unlock(false);

    public virtual void Unlock(bool force) => this.Unlock(force, (string) null);

    public virtual void Unlock(bool force, string sessionID)
    {
      this.Unlock(force, sessionID, false);
    }

    public virtual void Unlock(bool force, string sessionID, bool unlockAll)
    {
      try
      {
        this.onApiCalled(nameof (LoanProxy), nameof (Unlock), new object[2]
        {
          (object) force,
          (object) this.Guid
        });
        if (LoanLockAccessor.IsLoanLockDbEnabled && !ClientContext.GetCurrent().AllowConcurrentEditing)
        {
          string loginSessionId = this.Session.SessionID;
          if (force | unlockAll)
            loginSessionId = string.Empty;
          LoanLockAccessor.removeLock(this.guid, loginSessionId, this.Session.UserID);
        }
        else
        {
          UserInfo userInfo = this.Session.GetUserInfo();
          using (Loan loan = this.checkOut())
          {
            if (!force && !loan.Locked)
              return;
            if (!loan.IsLockedByUser(this.Session.UserID) && !force)
              Err.Raise(TraceLevel.Info, nameof (LoanProxy), (ServerException) new LockException(loan.GetLockInfo(sessionID != null ? sessionID : this.Session.SessionID)));
            string sessionID1 = (string) null;
            LockInfo lockInfo = loan.GetLockInfo(sessionID != null ? sessionID : this.Session.SessionID);
            if (ClientContext.GetCurrent().AllowConcurrentEditing || lockInfo.Exclusive == LockInfo.ExclusiveLock.NGSharedLock)
              sessionID1 = sessionID != null ? sessionID : this.Session.SessionID;
            if (force & unlockAll)
              loan.Unlock((string) null);
            else
              loan.Unlock(sessionID1);
            loan.CheckIn(userInfo, false, this.Session.SessionID);
            this.raiseEvent(LoanEventType.Unlocked, loan.Identity);
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool HasOwnership()
    {
      this.onApiCalled(nameof (LoanProxy), nameof (HasOwnership), Array.Empty<object>());
      return this.GetLockInfo(this.Session.UserID).LockedBy == this.Session.UserID;
    }

    public virtual LockInfo GetLockInfo()
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetLockInfo), Array.Empty<object>());
      try
      {
        return this.getLatestVersion().GetLockInfo();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (LockInfo) null;
      }
    }

    public virtual LockInfo GetLockInfo(string userid)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetLockInfo), Array.Empty<object>());
      try
      {
        return this.getLatestVersion().GetLockInfo(userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (LockInfo) null;
      }
    }

    public virtual LockInfo[] GetAllLockInfo()
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetAllLockInfo), Array.Empty<object>());
      try
      {
        return this.getLatestVersion().GetAllLockInfo();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (LockInfo[]) null;
      }
    }

    public virtual LockInfo[] CurrentLocks
    {
      get
      {
        this.onApiCalled(nameof (LoanProxy), nameof (CurrentLocks), Array.Empty<object>());
        try
        {
          return this.getLatestVersion().CurrentLocks;
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
          return (LockInfo[]) null;
        }
      }
    }

    public virtual DateTime GetLastModifiedDate(bool fromDB) => this.GetLastModifiedDate(fromDB, 0);

    public virtual DateTime GetLastModifiedDate(bool fromDB, int sqlRead)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetLastModifiedDate), Array.Empty<object>());
      try
      {
        return fromDB ? Loan.GetLastModifiedFromDB(this.guid) : this.getLatestVersion().LastModified;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return DateTime.MinValue;
      }
    }

    public virtual void AddToRecentLoans()
    {
      this.onApiCalled(nameof (LoanProxy), nameof (AddToRecentLoans), Array.Empty<object>());
      try
      {
        int maxRecentLoans = this.getMaxRecentLoans((IClientContext) ClientContext.GetCurrent());
        using (User latestVersion = UserStore.GetLatestVersion(this.Session.UserID))
          latestVersion.AddRemoveRecentLoans(this.guid, maxRecentLoans);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual void Move(string loanFolder, DuplicateLoanAction dupAction)
    {
      this.Move(loanFolder, "", dupAction);
    }

    public virtual void Move(string loanFolder, string loanName, DuplicateLoanAction dupAction)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (Move), new object[4]
      {
        (object) loanFolder,
        (object) loanName,
        (object) dupAction,
        (object) this.Guid
      });
      try
      {
        LoanIdentity loanIdentity = (LoanIdentity) null;
        LoanIdentity id = (LoanIdentity) null;
        LoanData loanData = (LoanData) null;
        DateTime now = DateTime.Now;
        using (Loan loan = this.checkOut(LoanInfo.Right.Access))
        {
          object[] objArray1 = new object[5]
          {
            (object) "MOVE: Acquired LoanSyncRoot and Loan lock while moving loan ",
            (object) loan.Identity,
            (object) ". Elapsed time = ",
            null,
            null
          };
          double totalMilliseconds = (DateTime.Now - now).TotalMilliseconds;
          objArray1[3] = (object) totalMilliseconds.ToString("0");
          objArray1[4] = (object) "ms";
          TraceLog.WriteInfo(nameof (LoanProxy), string.Concat(objArray1));
          loanIdentity = loan.Identity;
          loanData = loan.LoanData;
          object[] objArray2 = new object[5]
          {
            (object) "MOVE: Deserialized LoanData object for loan ",
            (object) loanIdentity,
            (object) ". Elapsed time = ",
            null,
            null
          };
          totalMilliseconds = (DateTime.Now - now).TotalMilliseconds;
          objArray2[3] = (object) totalMilliseconds.ToString("0");
          objArray2[4] = (object) "ms";
          TraceLog.WriteInfo(nameof (LoanProxy), string.Concat(objArray2));
          loanName = loanIdentity.Guid;
          LoanIdentity objectId = Loan.LookupIdentity(loanFolder, loanName);
          if (objectId != (LoanIdentity) null)
          {
            if (objectId.Guid == loanIdentity.Guid)
            {
              Err.Raise(TraceLevel.Warning, nameof (LoanProxy), new ServerException("Cannot move loan over top of itself."));
            }
            else
            {
              switch (dupAction)
              {
                case DuplicateLoanAction.Overwrite:
                  this.mngr.DeleteLoan(objectId.Guid);
                  break;
                case DuplicateLoanAction.Fail:
                  Err.Raise(TraceLevel.Warning, nameof (LoanProxy), (ServerException) new DuplicateObjectException("Cannot move over top existing loan '" + objectId.ToString() + "'", ObjectType.Loan, (object) objectId));
                  break;
              }
            }
          }
          object[] objArray3 = new object[7]
          {
            (object) "MOVE: Determined new loan name of ",
            (object) loanName,
            (object) " for loan ",
            (object) loanIdentity,
            (object) ". Elapsed time = ",
            null,
            null
          };
          TimeSpan timeSpan = DateTime.Now - now;
          totalMilliseconds = timeSpan.TotalMilliseconds;
          objArray3[5] = (object) totalMilliseconds.ToString("0");
          objArray3[6] = (object) "ms";
          TraceLog.WriteInfo(nameof (LoanProxy), string.Concat(objArray3));
          string fullLoanFilePath1 = new LoanFolder(loanFolder).GetFullLoanFilePath(loanName);
          string path = fullLoanFilePath1.Substring(0, fullLoanFilePath1.LastIndexOf("\\"));
          string fullLoanFilePath2 = new LoanFolder(loan.Identity.LoanFolder).GetFullLoanFilePath(loanName);
          string str = fullLoanFilePath2.Substring(0, fullLoanFilePath2.LastIndexOf("\\"));
          if (Directory.Exists(path))
            TraceLog.WriteError(nameof (LoanProxy), string.Format("Loan file overwritten at destination {0} from source {1} during loan move as loan file already exist clientID {2} userID {3}. Loan Guid: {4}", (object) path, (object) str, (object) this.Session.Context.ClientID, (object) this.Session.UserID, (object) this.Guid));
          loan.Move(loanFolder, loanName, this.Session.GetUserInfo());
          object[] objArray4 = new object[7]
          {
            (object) "MOVE: Moved files for loan ",
            (object) loanIdentity,
            (object) " to ",
            (object) loan.Identity,
            (object) ". Elapsed time = ",
            null,
            null
          };
          timeSpan = DateTime.Now - now;
          totalMilliseconds = timeSpan.TotalMilliseconds;
          objArray4[5] = (object) totalMilliseconds.ToString("0");
          objArray4[6] = (object) "ms";
          TraceLog.WriteInfo(nameof (LoanProxy), string.Concat(objArray4));
          id = loan.Identity;
        }
        if (loanFolder == SystemSettings.TrashFolder)
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new LoanFileAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.LoanDeleted, DateTime.Now, loanData.GUID, loanIdentity.LoanFolder, loanData.LoanNumber, loanData.GetField("37"), loanData.GetField("36"), loanData.GetField("11") + " " + loanData.GetField("12") + ", " + loanData.GetField("14") + " " + loanData.GetField("15"), loanData.GetField("2024"), this.Session.LoginParams.AppName));
        else if (loanIdentity.LoanFolder == SystemSettings.TrashFolder)
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new LoanFileAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.LoanRestored, DateTime.Now, loanData.GUID, loanIdentity.LoanFolder, loanData.LoanNumber, loanData.GetField("37"), loanData.GetField("36"), loanData.GetField("11") + " " + loanData.GetField("12") + ", " + loanData.GetField("14") + " " + loanData.GetField("15"), loanData.GetField("2024"), this.Session.LoginParams.AppName));
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new LoanFileAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.LoanMoved, DateTime.Now, loanData.GUID, loanIdentity.LoanFolder, loanData.LoanNumber, loanData.GetField("37"), loanData.GetField("36"), loanData.GetField("11") + " " + loanData.GetField("12") + ", " + loanData.GetField("14") + " " + loanData.GetField("15"), loanData.GetField("2024"), this.Session.LoginParams.AppName));
        TraceLog.WriteInfo(nameof (LoanProxy), "MOVE: Recorded audit trail record for " + (object) id + ". Elapsed time = " + (DateTime.Now - now).TotalMilliseconds.ToString("0") + "ms");
        this.raiseEvent(LoanEventType.Moved, id);
        TraceLog.WriteInfo(nameof (LoanProxy), "MOVE: Raised notification event for loan " + (object) id + ". Elapsed time = " + (DateTime.Now - now).TotalMilliseconds.ToString("0") + "ms");
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (LoanProxy), string.Format("Error occured during loan move opration. Loan Guid: {0}, Exception type: {1}", (object) this.Guid, (object) ex.GetFullStackTrace()));
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual BinaryObject Export()
    {
      this.onApiCalled(nameof (LoanProxy), nameof (Export), Array.Empty<object>());
      try
      {
        using (Loan loan = this.checkOut())
        {
          BinaryObject o = loan.Export();
          loan.UndoCheckout();
          this.raiseEvent(LoanEventType.Exported, loan.Identity);
          return BinaryObject.Marshal(o);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual void Import(LoanData data)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (Import), new object[1]
      {
        (object) data
      });
      try
      {
        data.Parse();
        data = (LoanData) data.Clone();
        UserInfo userInfo = this.Session.GetUserInfo();
        using (Loan loan = this.checkOut(LoanInfo.Right.Access))
        {
          this.demandLocked(loan);
          loan.Import(data, true);
          LoanData loanData = loan.LoanData;
          loan.CheckIn(userInfo, false, this.Session.SessionID, true);
          this.raiseEvent(LoanEventType.Imported, loan.Identity);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual void Import(BinaryObject data, string loanFolder = null)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (Import), new object[1]
      {
        (object) data
      });
      try
      {
        this.Import(new LoanDataFormatter().Deserialize(data, loanFolder));
        data.DisposeDeserialized();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetLoanPropertySetting(LoanProperty lp)
    {
      try
      {
        using (Loan latestVersion = this.getLatestVersion())
        {
          this.onApiCalled(nameof (LoanProxy), nameof (SetLoanPropertySetting), new object[1]
          {
            (object) latestVersion.Identity.Guid
          });
          latestVersion.SetLoanPropertySetting(lp);
        }
      }
      catch (Exception ex)
      {
        this.onApiCalled(nameof (LoanProxy), nameof (SetLoanPropertySetting), new object[2]
        {
          (object) "",
          (object) "Failed"
        });
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual LoanProperty[] GetLoanPropertySettings()
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetLoanPropertySettings), Array.Empty<object>());
      return this.GetLoanPropertySettingsInternal();
    }

    public virtual LoanProperty[] GetLoanPropertySettingsInternal()
    {
      try
      {
        using (Loan latestVersion = this.getLatestVersion())
          return latestVersion.GetLoanPropertySettings();
      }
      catch (Exception ex)
      {
        this.onApiCalled(nameof (LoanProxy), "GetLoanPropertySettings", new object[2]
        {
          (object) "",
          (object) "Failed"
        });
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (LoanProperty[]) null;
      }
    }

    public virtual string[] GetSupportingDataKeysOnCIFs()
    {
      this.onApiCalled(nameof (LoanProxy), "GetSupportingDataKeys", Array.Empty<object>());
      try
      {
        using (Loan latestVersion = this.getLatestVersion())
          return latestVersion.GetSupportingDataKeys();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual BinaryObject GetSupportingDataOnCIFs(string key)
    {
      try
      {
        using (Loan latestVersion = this.getLatestVersion())
        {
          BinaryObject supportingData = latestVersion.GetSupportingData(key, false);
          if (supportingData != null)
            this.onApiCalled(nameof (LoanProxy), "GetSupportingData", new object[2]
            {
              (object) key,
              (object) supportingData
            });
          TraceLog.WriteInfo(nameof (LoanProxy), this.formatMsg("Retrieved supporting data \"" + key + "\" for loan " + this.guid));
          return BinaryObject.Marshal(supportingData);
        }
      }
      catch (Exception ex)
      {
        this.onApiCalled(nameof (LoanProxy), "GetSupportingData", new object[2]
        {
          (object) key,
          (object) "Failed"
        });
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual SnapshotObject GetSupportingSnapshotData(
      LogSnapshotType type,
      System.Guid snapshotGuid,
      string fileNameAsKey)
    {
      try
      {
        using (Loan latestVersion = this.getLatestVersion())
        {
          SnapshotObject supportingSnapshotData = latestVersion.GetSupportingSnapshotData(type, snapshotGuid, fileNameAsKey);
          if (supportingSnapshotData != null)
            this.onApiCalled(nameof (LoanProxy), nameof (GetSupportingSnapshotData), new object[2]
            {
              (object) (snapshotGuid.ToString() + "," + fileNameAsKey),
              (object) supportingSnapshotData
            });
          TraceLog.WriteInfo(nameof (LoanProxy), this.formatMsg("Retrieved supporting snapshot data \"" + (object) snapshotGuid + "," + fileNameAsKey + "\" for loan " + this.guid));
          return supportingSnapshotData;
        }
      }
      catch (Exception ex)
      {
        this.onApiCalled(nameof (LoanProxy), nameof (GetSupportingSnapshotData), new object[2]
        {
          (object) (snapshotGuid.ToString() + "," + fileNameAsKey),
          (object) "Failed"
        });
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (SnapshotObject) null;
      }
    }

    public virtual void SaveSupportingDataOnCIFs(string key, BinaryObject data)
    {
      this.onApiCalled(nameof (LoanProxy), "SaveSupportingData", new object[3]
      {
        (object) key,
        (object) data,
        (object) this.Guid
      });
      data?.Download();
      try
      {
        using (Loan latestVersion = this.getLatestVersion(LoanInfo.Right.Access))
          latestVersion.SaveSupportingData(key, data);
        data?.DisposeDeserialized();
        TraceLog.WriteInfo(nameof (LoanProxy), this.formatMsg("Saved supporting data \"" + key + "\" for loan " + this.guid));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SaveSupportingSnapshotData(
      LogSnapshotType type,
      System.Guid snapshotGuid,
      string fileNameAsKey,
      SnapshotObject data)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (SaveSupportingSnapshotData), new object[3]
      {
        (object) fileNameAsKey,
        (object) data,
        (object) this.Guid
      });
      try
      {
        using (Loan loan = this.checkOut(LoanInfo.Right.Access))
          loan.SaveSupportingSnapshotData(type, snapshotGuid, fileNameAsKey, data);
        TraceLog.WriteInfo(nameof (LoanProxy), this.formatMsg("Saved supporting data \"" + fileNameAsKey + "\" for loan " + this.guid));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual BinaryObject GetSupportingLinkedDataOnCIFs(
      string loanFolder,
      string loanName,
      string key)
    {
      try
      {
        using (Loan latestVersion = this.getLatestVersion())
        {
          BinaryObject supportingLinkedData = latestVersion.GetSupportingLinkedData(loanFolder, loanName, key);
          if (supportingLinkedData != null)
            this.onApiCalled(nameof (LoanProxy), "GetSupportingLinkedData", new object[2]
            {
              (object) key,
              (object) supportingLinkedData
            });
          TraceLog.WriteInfo(nameof (LoanProxy), this.formatMsg("Retrieved supporting aux   \"" + key + "\" for loan " + this.guid));
          return BinaryObject.Marshal(supportingLinkedData);
        }
      }
      catch (Exception ex)
      {
        this.onApiCalled(nameof (LoanProxy), "GetSupportingLinkedData", new object[2]
        {
          (object) key,
          (object) "Failed"
        });
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual void SaveSupportingLinkedDataOnCIFs(
      string loanFolder,
      string loanName,
      string key,
      BinaryObject data)
    {
      this.onApiCalled(nameof (LoanProxy), "SaveSupportingLinkedData", new object[3]
      {
        (object) key,
        (object) data,
        (object) this.Guid
      });
      data?.Download();
      try
      {
        using (Loan loan = this.checkOut(LoanInfo.Right.Access))
          loan.SaveSupportingLinkedData(loanFolder, loanName, key, data);
        data?.DisposeDeserialized();
        TraceLog.WriteInfo(nameof (LoanProxy), this.formatMsg("Saved supporting aux data \"" + key + "\" for loan " + this.guid));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AppendSupportingDataOnCIFs(string key, BinaryObject data)
    {
      this.onApiCalled(nameof (LoanProxy), "AppendSupportingData", new object[2]
      {
        (object) key,
        (object) data
      });
      if (data == null)
        Err.Raise(nameof (LoanProxy), (ServerException) new ServerArgumentException("The data object cannot be null", nameof (data), this.Session.SessionInfo));
      data.Download();
      try
      {
        using (Loan loan = this.checkOut(LoanInfo.Right.Access))
          loan.AppendSupportingData(key, data);
        data.DisposeDeserialized();
        TraceLog.WriteInfo(nameof (LoanProxy), this.formatMsg("Appended supporting data \"" + key + "\" for loan " + this.guid));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual bool SupportingDataExistsOnCIFs(string key)
    {
      this.onApiCalled(nameof (LoanProxy), "SupportingDataExists", new object[1]
      {
        (object) key
      });
      try
      {
        using (Loan latestVersion = this.getLatestVersion())
          return latestVersion.SupportingDataExists(key);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool DeleteSupportingDataOnCIFs(string key)
    {
      this.onApiCalled(nameof (LoanProxy), "DeleteSupportingData", new object[1]
      {
        (object) key
      });
      try
      {
        using (Loan loan = this.checkOut())
        {
          if (!loan.SupportingDataExists(key))
            return false;
          loan.SaveSupportingData(key, (BinaryObject) null);
          return true;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual int DeleteSupportingDataOnCIFs(string[] keys)
    {
      this.onApiCalled(nameof (LoanProxy), "DeleteSupportingData", (object[]) keys);
      try
      {
        int num = 0;
        using (Loan loan = this.checkOut())
        {
          foreach (string key in keys)
          {
            if (loan.SupportingDataExists(key))
            {
              loan.SaveSupportingData(key, (BinaryObject) null);
              ++num;
            }
          }
        }
        return num;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual Dictionary<string, string> GetLoanSnapshot(
      LogSnapshotType type,
      System.Guid snapshotGuid,
      bool ucdExists)
    {
      this.onApiCalled(nameof (LoanProxy), "GetLoanSnapshots", new object[2]
      {
        (object) type,
        (object) this.guid
      });
      using (Loan latestVersion = this.getLatestVersion())
        return LoanLogSnapshotStore.GetLoanSnapshot(latestVersion, type, snapshotGuid, ucdExists);
    }

    public virtual Dictionary<string, Dictionary<string, string>> GetLoanSnapshots(
      LogSnapshotType type,
      Dictionary<string, bool> snapshotGuids)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetLoanSnapshots), new object[2]
      {
        (object) type,
        (object) snapshotGuids
      });
      using (Loan latestVersion = this.getLatestVersion())
        return LoanLogSnapshotStore.GetLoanSnapshots(latestVersion, type, snapshotGuids);
    }

    public virtual LoanHistoryEntry[] GetLoanHistory(string[] objectList)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetLoanHistory), Array.Empty<object>());
      try
      {
        using (Loan loan = this.checkOut())
          return loan.GetLoanHistory(objectList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (LoanHistoryEntry[]) null;
      }
    }

    public virtual void AppendLoanHistory(LoanHistoryEntry[] entryList)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (AppendLoanHistory), Array.Empty<object>());
      try
      {
        using (Loan loan = this.checkOut())
          loan.AppendLoanHistory(entryList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual ZipReader CreateZipReader(string key)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (CreateZipReader), new object[1]
      {
        (object) key
      });
      try
      {
        using (Loan latestVersion = this.getLatestVersion())
        {
          ZipReader zipReader = latestVersion.CreateZipReader(key);
          zipReader.FileDownloaded += new EventHandler<FileDownloadEventArgs>(this.zipReader_FileDownloaded);
          return zipReader;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (ZipReader) null;
      }
    }

    private void zipReader_FileDownloaded(object sender, FileDownloadEventArgs e)
    {
      this.zipReaderFileDownloadedEvent(sender, e);
    }

    protected virtual void zipReaderFileDownloadedEvent(object sender, FileDownloadEventArgs e)
    {
      this.onApiCalled(nameof (LoanProxy), "FileUpload", new object[3]
      {
        (object) e.Name,
        (object) ("<BinaryObject:" + e.Size.ToString() + "byte>"),
        (object) ("<Execution Time:" + e.Time.ToString() + "ms>")
      });
    }

    public virtual ZipWriter CreateZipWriter(string key, int compressionLevel)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (CreateZipWriter), new object[2]
      {
        (object) key,
        (object) compressionLevel
      });
      try
      {
        using (Loan loan = this.checkOut())
        {
          ZipWriter zipWriter = loan.CreateZipWriter(key, compressionLevel);
          zipWriter.FileUploaded += new ZipWriter.FileUploadEventHandler(this.zipWriter_FileUploaded);
          return zipWriter;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (ZipWriter) null;
      }
    }

    private void zipWriter_FileUploaded(object source, FileUploadEventArgs args)
    {
      this.zipWriterFileUploadedEvent(source, args);
    }

    protected virtual void zipWriterFileUploadedEvent(object source, FileUploadEventArgs args)
    {
      this.onApiCalled(nameof (LoanProxy), "FileUpload", new object[4]
      {
        (object) args.Name,
        (object) ("<BinaryObject:" + args.Size.ToString() + " byte>"),
        (object) ("<Execution Time:" + args.Time.ToString() + "ms>"),
        (object) this.Guid
      });
    }

    public virtual FileAttachment[] GetFileAttachments()
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetFileAttachments), Array.Empty<object>());
      try
      {
        using (Loan latestVersion = this.getLatestVersion())
          return latestVersion.GetFileAttachments();
      }
      catch (XmlException ex)
      {
        Err.Raise(TraceLevel.Error, nameof (LoanProxy), new ServerException("The attachments.xml for the loan GUID = '" + this.Guid + "' is corrupted.", (Exception) ex));
        return (FileAttachment[]) null;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (FileAttachment[]) null;
      }
    }

    public virtual void ReplaceBackgroundAttachment(FileAttachment attachment)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (ReplaceBackgroundAttachment), Array.Empty<object>());
      try
      {
        using (Loan loan = this.checkOut())
          loan.ReplaceBackgroundAttachment(attachment);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SaveFileAttachments(FileAttachment[] attachmentList)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (SaveFileAttachments), Array.Empty<object>());
      try
      {
        using (Loan loan = this.checkOut())
          loan.SaveFileAttachments(attachmentList);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
      }
    }

    public virtual SkyDriveUrl GetSkyDriveUrlForGet(string fileKey)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetSkyDriveUrlForGet), new object[2]
      {
        (object) this.guid,
        (object) fileKey
      });
      try
      {
        return Loan.GetSkyDriveUrlForGet(this.guid, fileKey, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (SkyDriveUrl) null;
      }
    }

    public virtual SkyDriveUrl GetSkyDriveUrlForPut(
      string fileKey,
      string contentType,
      bool useSkyDriveClassic = false)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetSkyDriveUrlForPut), new object[2]
      {
        (object) this.guid,
        (object) fileKey
      });
      try
      {
        return Loan.GetSkyDriveUrlForPut(this.guid, fileKey, contentType, this.Session.UserID, useSkyDriveClassic);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (SkyDriveUrl) null;
      }
    }

    public virtual SkyDriveUrl GetSkyDriveUrlForMeta(string objectId, string fileName = null)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetSkyDriveUrlForMeta), new object[1]
      {
        (object) objectId
      });
      try
      {
        return Loan.GetSkyDriveUrlForMeta(objectId, this.Session.UserID, fileName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (SkyDriveUrl) null;
      }
    }

    public virtual SkyDriveUrl GetSkyDriveUrlForObject(string objectId)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetSkyDriveUrlForObject), new object[1]
      {
        (object) objectId
      });
      try
      {
        return Loan.GetSkyDriveUrlForGet(objectId, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (SkyDriveUrl) null;
      }
    }

    public virtual List<SkyDriveUrl> GetSkyDriveUrlForObjects(string[] objectIds)
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetSkyDriveUrlForObjects), (object[]) objectIds);
      try
      {
        return Loan.GetSkyDriveUrlsForGet(objectIds, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (List<SkyDriveUrl>) null;
      }
    }

    public virtual string[] GetSkyDriveSupportingDataKeys()
    {
      this.onApiCalled(nameof (LoanProxy), nameof (GetSkyDriveSupportingDataKeys), new object[1]
      {
        (object) this.guid
      });
      try
      {
        return Loan.GetSkyDriveSupportingDataKeys(this.guid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanProxy), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    private Loan checkOut()
    {
      Loan loan = LoanStore.CheckOut(this.guid);
      if (!loan.Exists)
      {
        loan.UndoCheckout();
        Err.Raise(TraceLevel.Warning, nameof (LoanProxy), (ServerException) new ObjectNotFoundException("Loan '" + this.Guid + "' no longer exists.", ObjectType.Loan, (object) this.Guid));
      }
      return loan;
    }

    private Loan checkOut(LoanInfo.Right requiredRights)
    {
      Loan loan = this.checkOut();
      try
      {
        ((SecurityManager) this.Security).DemandLoanRights(loan, requiredRights);
        return loan;
      }
      catch
      {
        loan.UndoCheckout();
        throw;
      }
    }

    private Loan getLatestVersion()
    {
      Loan latestVersion = LoanStore.GetLatestVersion(this.guid);
      if (latestVersion.Exists)
        return latestVersion;
      Err.Raise(TraceLevel.Warning, nameof (LoanProxy), (ServerException) new ObjectNotFoundException("Loan '" + this.Guid + "' no longer exists.", ObjectType.Loan, (object) this.Guid));
      return latestVersion;
    }

    private Loan getLatestVersion(LoanInfo.Right requiredRights)
    {
      Loan latestVersion = this.getLatestVersion();
      ((SecurityManager) this.Security).DemandLoanRights(latestVersion, requiredRights);
      return latestVersion;
    }

    private void demandUnlocked(Loan loan)
    {
      if (!loan.Locked)
        return;
      Err.Raise(TraceLevel.Warning, nameof (LoanProxy), (ServerException) new LockException(loan.GetLockInfo((string) null)));
    }

    private void demandLocked(Loan loan)
    {
      if (LoanLockAccessor.IsLoanLockDbEnabled)
      {
        if (LoanLockAccessor.IsLoanLockedBySessionId(loan.Identity.Guid, this.Session.SessionID))
          return;
        Err.Raise(TraceLevel.Info, nameof (LoanProxy), (ServerException) new LockException("Object must be locked for this operation", loan.GetLockInfo((string) null)));
      }
      else
      {
        if (loan.IsLockedBySession(this.Session.SessionID))
          return;
        Err.Raise(TraceLevel.Info, nameof (LoanProxy), (ServerException) new LockException("Object must be locked for this operation", loan.GetLockInfo((string) null)));
      }
    }

    private void demandLocked(Loan loan, LoanInfo.LockReason reason)
    {
      if (LoanLockAccessor.IsLoanLockDbEnabled)
      {
        if (LoanLockAccessor.IsLoanLockedBySessionId(loan.Identity.Guid, this.Session.SessionID))
          return;
        Err.Raise(TraceLevel.Info, nameof (LoanProxy), (ServerException) new LockException("Object must be locked for this operation", loan.GetLockInfo((string) null)));
      }
      else
      {
        if (!loan.IsLockedBySession(this.Session.SessionID))
          Err.Raise(TraceLevel.Info, nameof (LoanProxy), (ServerException) new LockException("Object must be locked for this operation", loan.GetLockInfo((string) null)));
        if (loan.Locked)
          return;
        Err.Raise(TraceLevel.Info, nameof (LoanProxy), (ServerException) new LockException("Invalid lock type held for this operation", loan.GetLockInfo((string) null)));
      }
    }

    private void raiseEvent(LoanEventType eventType, LoanIdentity id)
    {
      LoanEvent e = new LoanEvent(eventType, id, this.Session.SessionInfo);
      TraceLog.WriteInfo(nameof (LoanProxy), e.ToString());
      EncompassServer.RaiseEvent(this.Session.Context, (ServerEvent) e);
    }

    public virtual void SaveLockSnapshotRecapture(LockSnapshotRecapture lockSnapshotRecapture)
    {
      new LockSnapshotRecaptureStore().Save(lockSnapshotRecapture);
    }

    public void Decorate(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      if (string.IsNullOrEmpty(this.guid))
        return;
      log.Set<string>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.LoanId, this.guid);
    }
  }
}

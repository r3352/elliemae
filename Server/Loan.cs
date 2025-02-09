// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Loan
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using com.elliemae.services.eventbus.models;
using Elli.Common;
using Elli.Domain.FileFormats;
using Elli.ElliEnum;
using Elli.Interface;
using EllieMae.EMLite.Cache;
using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.MessageServices.Event;
using EllieMae.EMLite.ClientServer.MessageServices.Kafka.Event;
using EllieMae.EMLite.ClientServer.MessageServices.Message.Alerts;
using EllieMae.EMLite.ClientServer.MessageServices.Message.Common;
using EllieMae.EMLite.ClientServer.MessageServices.Message.Document;
using EllieMae.EMLite.ClientServer.SkyDrive;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils;
using EllieMae.EMLite.ReportingDbUtils.Query;
using EllieMae.EMLite.Server.Cache;
using EllieMae.EMLite.Server.ServerCommon;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using EllieMae.EMLite.Server.ServerObjects.Deferrables;
using EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime;
using EllieMae.EMLite.Server.ServerObjects.eFolder;
using EllieMae.EMLite.Server.ServerObjects.LoanEvent;
using EllieMae.EMLite.Server.ServiceObjects.KafkaEvent;
using EllieMae.EMLite.Server.SkyDrive;
using EllieMae.EMLite.ServiceInterface;
using EllieMae.EMLite.Trading;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Metrics;
using Microsoft.IdentityModel.Claims;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class Loan : IDisposable
  {
    private const string className = "Loan�";
    public const string LoanDataFilename = "loan.em�";
    public const string DeferredLoanNameTag = "_^_�";
    public const string VersionNameTag = "_loan.em�";
    private const int UnspecifiedLoanVersionNumber = -999;
    public const int MaxAuditFieldLength = 8000;
    public static readonly string SyncRootKey = "Loan.SyncRoot";
    private static readonly TimeSpan reportingDbLockTimeout = TimeSpan.FromSeconds(60.0);
    private static readonly bool _skipLoanLockValidationAtCheckin;
    private ICacheLock<bool?> innerLock;
    private LoanIdentity id;
    private DraftLoanIdentity draftId;
    private LoanServerInfo serverInfo;
    private DraftLoanServerInfo serverDraftInfo;
    private LoanData loanData;
    private LoanData _readOnlyLoanData;
    private bool rightsModified;
    private bool saveLoanData;
    private bool forceLastModified;
    private bool forceRebuild;
    private bool isRebuidFlow;
    private bool exists;
    private readonly bool _readOnlyLoan;
    private bool disposed;
    private const string VIRTUAL_COL_VALUES = "NGSHAREDLOCK!!�";
    private string _xdbInstances = "BE11172647,BE11172646,BE11063388a,BE11105680i,BE11105680c,BE912034";
    private bool _xdbLogs;
    private static string sw = Tracing.SwOutsideLoan;
    private readonly int _specifiedLoanVersionNumber = -999;
    private IMetricRecorder metricRecorder = (IMetricRecorder) DisabledMetricRecorder.Instance;
    private LoanData _loanDataOfSpecifiedVersion;
    private static bool currentLoanEventPublishEnabledState = false;
    private static bool currentStateReported = false;
    private object lockObject = new object();

    static Loan()
    {
      bool result;
      Loan._skipLoanLockValidationAtCheckin = bool.TryParse(ConfigurationManager.AppSettings["SkipLoanLockValidationAtCheckin"], out result) && result;
    }

    public Loan(ICacheLock<bool?> innerLock, bool draftIden)
    {
      this.innerLock = innerLock;
      if (draftIden)
      {
        DraftLoanIdentity draftLoanIdentity = Loan.LookupDraftIdentity(innerLock.Identifier.ToString());
        if (draftLoanIdentity != (DraftLoanIdentity) null)
        {
          this.draftId = draftLoanIdentity;
          this.exists = true;
        }
        else
          this.draftId = new DraftLoanIdentity(innerLock.Identifier.ToString());
        this.id = new LoanIdentity(innerLock.Identifier.ToString());
      }
      else
      {
        LoanIdentity loanIdentity = Loan.LookupIdentity(innerLock.Identifier.ToString());
        if (loanIdentity != (LoanIdentity) null)
        {
          this.id = loanIdentity;
          this.exists = true;
        }
        else
          this.id = new LoanIdentity(innerLock.Identifier.ToString());
      }
    }

    public Loan(DraftLoanIdentity draftLoanIdentity)
    {
      this.draftId = draftLoanIdentity;
      this.exists = true;
    }

    public Loan(string guid)
    {
      LoanIdentity loanIdentity = Loan.LookupIdentity(guid);
      if (loanIdentity != (LoanIdentity) null)
      {
        this.id = loanIdentity;
        this.exists = true;
      }
      else
      {
        this.id = new LoanIdentity(guid);
        this.draftId = new DraftLoanIdentity(guid);
      }
      this._readOnlyLoan = true;
    }

    public Loan(LoanIdentity identity) => this.id = identity;

    public Loan(LoanIdentity identity, bool exists)
      : this(identity)
    {
      this.exists = exists;
    }

    public Loan(string guid, int specifiedLoanVersionNumber)
      : this(guid)
    {
      this._specifiedLoanVersionNumber = specifiedLoanVersionNumber;
    }

    public Loan(LoanIdentity identity, int specifiedLoanVersionNumber, bool exists)
      : this(identity, exists)
    {
      this._specifiedLoanVersionNumber = specifiedLoanVersionNumber;
    }

    public int SpecifiedLoanVersionNumber => this._specifiedLoanVersionNumber;

    public bool Exists => this.exists;

    public bool IsReadOnlyLoan => this._readOnlyLoan;

    public LoanIdentity Identity => this.id;

    public DraftLoanIdentity DraftIdentity => this.draftId;

    public LockInfo[] CurrentLocks => this.info.CurrentLocks;

    public string[] LockedBySessions
    {
      get
      {
        this.validateExists();
        return this.info.GetLockedBySessions();
      }
    }

    public string[] LockedByUsers
    {
      get
      {
        this.validateExists();
        return this.info.GetLockedByUsers();
      }
    }

    public bool IsLockedBySession(string sessionID)
    {
      bool flag = false;
      foreach (string lockedBySession in this.LockedBySessions)
      {
        if (lockedBySession == sessionID)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    public bool IsLockedByUser(string userid)
    {
      bool flag = false;
      foreach (string lockedByUser in this.LockedByUsers)
      {
        if (lockedByUser == userid)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    public bool SaveLoanData
    {
      set => this.saveLoanData = value;
    }

    public bool ForceRebuild
    {
      get => this.forceRebuild;
      set => this.forceRebuild = value;
    }

    public bool IsRebuildFlow
    {
      get => this.isRebuidFlow;
      set => this.isRebuidFlow = value;
    }

    public bool Locked
    {
      get
      {
        this.validateExists();
        return this.info.Locked;
      }
    }

    public void SetMetricRecorder(IMetricRecorder metricRecorder)
    {
      this.metricRecorder = metricRecorder;
    }

    public bool LockedBySession(string sessionID)
    {
      this.validateExists();
      return this.info.IsLockedBySession(sessionID);
    }

    public DateTime LastModified
    {
      get
      {
        this.validateExists();
        return this.info.LastModified;
      }
      set
      {
        this.validateInstance();
        this.info.LastModified = value;
        this.forceLastModified = true;
      }
    }

    public DateTime LastModifiedUTC
    {
      get
      {
        DateTime lastModified = this.LastModified;
        return this.LastModified != DateTime.MinValue ? this.LastModified.ToUniversalTime() : DateTime.UtcNow;
      }
    }

    public EllieMae.EMLite.ClientServer.LoanAssociateInfo[] GetLoanAssociates(
      bool milestoneRolesOnly)
    {
      return Loan.GetLoanAssociates(this.Identity.Guid, milestoneRolesOnly, false);
    }

    public EllieMae.EMLite.ClientServer.LoanAssociateInfo[] GetLoanAssociates(
      bool milestoneRolesOnly,
      bool resolveUsers)
    {
      return Loan.GetLoanAssociates(this.Identity.Guid, milestoneRolesOnly, resolveUsers);
    }

    public bool IsAccessibleWithinSubOrgs(int parentOrgId)
    {
      return this.IsAccessibleInAnyOrg(OrganizationStore.GetDescendentsOfOrg(parentOrgId));
    }

    public bool IsAccessibleInAnyOrg(int[] orgList)
    {
      return this.IsAccessibleInAnyOrg(orgList, LoanInfo.Right.Access);
    }

    public bool IsAccessibleInAnyOrg(int[] orgList, LoanInfo.Right rights)
    {
      return (this.GetMaxAccessibilityWithinAnyOrg(orgList) & rights) == rights;
    }

    public LoanInfo.Right GetMaxAccessibilityWithinAnyOrg(int[] orgList)
    {
      this.validateExists();
      if (orgList.Length == 0)
        return LoanInfo.Right.NoRight;
      string str = "";
      for (int index = 0; index < orgList.Length; ++index)
        str = str + orgList[index].ToString() + (index == orgList.Length - 1 ? "" : ", ");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select distinct r.rights from UserLoanAccessRights r, users u");
      dbQueryBuilder.AppendLine("where (r.userid = u.userid)");
      dbQueryBuilder.AppendLine("      and (r.guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.id.Guid) + ")");
      dbQueryBuilder.AppendLine("      and (u.org_id in (" + str + "))");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      LoanInfo.Right accessibilityWithinAnyOrg = LoanInfo.Right.NoRight;
      for (int index = 0; index < dataRowCollection.Count; ++index)
        accessibilityWithinAnyOrg |= (LoanInfo.Right) dataRowCollection[index]["rights"];
      return accessibilityWithinAnyOrg;
    }

    public LoanInfo.Right GetRightsForUser(string userId)
    {
      this.validateExists();
      return !this.info.UserRightPairs.ContainsKey((object) userId) ? LoanInfo.Right.NoRight : (LoanInfo.Right) this.info.UserRightPairs[(object) userId];
    }

    public Hashtable GetAllUserRights()
    {
      this.validateExists();
      return (Hashtable) this.info.UserRightPairs.Clone();
    }

    public bool DoesUserHaveRights(string userId, LoanInfo.Right rights)
    {
      this.validateExists();
      return (this.GetRightsForUser(userId) & rights) == rights;
    }

    public void AddRightsForUser(string userId, LoanInfo.Right right)
    {
      this.validateInstance();
      Loan.validateRightsInfo(userId, right);
      if (right == LoanInfo.Right.NoRight)
        return;
      this.onBeforeRightsChanged();
      if (!this.info.UserRightPairs.ContainsKey((object) userId))
        this.info.UserRightPairs[(object) userId] = (object) right;
      else
        this.info.UserRightPairs[(object) userId] = (object) ((LoanInfo.Right) this.info.UserRightPairs[(object) userId] | right);
    }

    public LoanProperty[] GetLoanPropertySettings()
    {
      return LoanPropertySettingsAccessor.GetLoanPropertySettings(this.id.Guid);
    }

    public void SetRightsForUser(string userId, LoanInfo.Right right)
    {
      this.validateInstance();
      Loan.validateRightsInfo(userId, right);
      this.onBeforeRightsChanged();
      this.info.UserRightPairs[(object) userId] = (object) right;
      if ((LoanInfo.Right) this.info.UserRightPairs[(object) userId] != LoanInfo.Right.NoRight)
        return;
      this.info.UserRightPairs.Remove((object) userId);
    }

    public void RemoveRightsForUser(string userId, LoanInfo.Right right)
    {
      this.validateInstance();
      Loan.validateRightsInfo(userId, right);
      if (right == LoanInfo.Right.NoRight || !this.info.UserRightPairs.ContainsKey((object) userId))
        return;
      this.onBeforeRightsChanged();
      this.info.UserRightPairs[(object) userId] = (object) (int) ((LoanInfo.Right) this.info.UserRightPairs[(object) userId] & ((LoanInfo.Right) 255 ^ right));
      if ((LoanInfo.Right) this.info.UserRightPairs[(object) userId] != LoanInfo.Right.NoRight)
        return;
      this.info.UserRightPairs.Remove((object) userId);
    }

    public void ReplaceAllUserRights(Hashtable rightsInfo)
    {
      this.validateInstance();
      this.onBeforeRightsChanged();
      this.info.UserRightPairs.Clear();
      foreach (DictionaryEntry dictionaryEntry in rightsInfo)
        this.SetRightsForUser(dictionaryEntry.Key.ToString(), (LoanInfo.Right) dictionaryEntry.Value);
    }

    public void AddIntoUserRights(Hashtable rightsInfo)
    {
      this.validateInstance();
      foreach (DictionaryEntry dictionaryEntry in rightsInfo)
        this.AddRightsForUser(dictionaryEntry.Key.ToString(), (LoanInfo.Right) dictionaryEntry.Value);
    }

    public void RemoveFromUserRights(Hashtable rightsInfo)
    {
      this.validateInstance();
      foreach (DictionaryEntry dictionaryEntry in rightsInfo)
        this.RemoveRightsForUser(dictionaryEntry.Key.ToString(), (LoanInfo.Right) dictionaryEntry.Value);
    }

    public LoanProperties GetProperties()
    {
      return new LoanProperties(this.id, new LoanFolder(this.id.LoanFolder).GetLoanSize(this.id.LoanName));
    }

    public void Lock(
      SessionInfo sessionInfo,
      LoanInfo.LockReason reason,
      LockInfo.ExclusiveLock exclusive)
    {
      this.validateInstance();
      this.info.AddLockInfo(new LockInfo(this.id.Guid, sessionInfo.UserID, (string) null, (string) null, sessionInfo.SessionID, sessionInfo.Server.ToString(), reason, DateTime.Now, exclusive));
    }

    public void Lock(LockInfo newLock)
    {
      this.validateInstance();
      this.info.AddLockInfo((LockInfo) newLock.Clone());
    }

    public string LockSessionLess(LockInfo newLock)
    {
      this.validateInstance();
      LockInfo lockInfo = (LockInfo) newLock.CloneForSessionLess();
      this.info.AddLockInfo(lockInfo);
      return lockInfo.LoginSessionID;
    }

    public void Unlock()
    {
      this.validateInstance();
      this.info.Unlock();
    }

    public void Unlock(string sessionID)
    {
      this.validateInstance();
      this.info.RemoveLockInfo(sessionID);
    }

    public LockInfo GetLockInfo()
    {
      this.validateExists();
      return Loan.readDatabaseLock(this.id, (string) null, true);
    }

    public LockInfo GetLockInfo(string sessionID)
    {
      this.validateExists();
      return Loan.readDatabaseLock(this.id, sessionID, false);
    }

    public LockInfo[] GetAllLockInfo()
    {
      this.validateExists();
      return Loan.readAllDatabaseLocks(this.id);
    }

    public LoanData LoanData
    {
      get
      {
        if (!this._readOnlyLoan)
          this.validateInstance();
        if (this.loanData == null)
        {
          this.loanData = this.deserializeLoanData(this.id, -999);
          if (this.loanData == null)
            Err.Raise(TraceLevel.Error, nameof (Loan), (ServerException) new ObjectNotFoundException("Loan data file is missing or corrupt", ObjectType.Loan, (object) this.Identity));
        }
        this.saveLoanData = true;
        return this.loanData;
      }
    }

    public LoanData LoanDataOfSpecifiedVersion
    {
      get
      {
        if (this.SpecifiedLoanVersionNumber < 1)
          return (LoanData) null;
        if (this._loanDataOfSpecifiedVersion == null)
        {
          this._loanDataOfSpecifiedVersion = this.deserializeLoanData(this.id, this.SpecifiedLoanVersionNumber, onlySavedData: true);
          if (this._loanDataOfSpecifiedVersion == null)
            Err.Raise(TraceLevel.Error, nameof (Loan), (ServerException) new ObjectNotFoundException("Loan data file of version " + (object) this.SpecifiedLoanVersionNumber + "is missing or corrupt", ObjectType.Loan, (object) this.Identity));
        }
        return this._loanDataOfSpecifiedVersion;
      }
    }

    public LoanData ReadOnlyLoanData
    {
      get
      {
        if (this._readOnlyLoanData == null)
          this._readOnlyLoanData = this.deserializeLoanData(this.id, -999);
        if (this._readOnlyLoanData == null)
          Err.Raise(TraceLevel.Error, nameof (Loan), (ServerException) new ObjectNotFoundException("Loan data file is missing or corrupt", ObjectType.Loan, (object) this.Identity));
        return this._readOnlyLoanData;
      }
    }

    public LoanData DraftLoanData
    {
      get
      {
        this.validateInstance();
        if (this.loanData == null)
        {
          this.loanData = this.deserializeDraftLoanData(this.draftId);
          if (this.loanData == null)
            Err.Raise(TraceLevel.Error, nameof (Loan), (ServerException) new ObjectNotFoundException("Loan data file is missing or corrupt", ObjectType.Loan, (object) this.DraftIdentity));
        }
        this.saveLoanData = true;
        return this.loanData;
      }
    }

    public LoanData LoadLoanData(ILoanSettings loanSettings, bool bLoanLocked = false)
    {
      return bLoanLocked ? this.deserializeLoanData(this.id, -999, loanSettings) : new LoanFolder(this.id.LoanFolder).ReadLoanData(this.id.LoanName, loanSettings, loanFolder: this.id.LoanFolder);
    }

    public LoanData GetLoanData(
      ILoanSettings loanSettings,
      bool shouldValidateInstance = true,
      bool needToSave = true,
      bool onlySavedData = false)
    {
      if (shouldValidateInstance)
        this.validateInstance();
      if (this.loanData != null && !needToSave)
        return this.loanData;
      if (this.loanData == null)
      {
        this.loanData = this.deserializeLoanData(this.id, -999, loanSettings, onlySavedData);
        if (this.loanData == null)
          Err.Raise(TraceLevel.Error, nameof (Loan), (ServerException) new ObjectNotFoundException("Loan data file is missing or corrupt", ObjectType.Loan, (object) this.Identity));
      }
      this.saveLoanData = true;
      return this.loanData;
    }

    public BinaryObject Export()
    {
      this.validateInstance();
      return new LoanDataFormatter().Serialize(this.LoanData, false);
    }

    public void ImportDraft(LoanData data, bool preservePriorLoanNumbering)
    {
      this.validateInstance();
      if (data == null)
        Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("Cannot set loan data to null"));
      if (this.draftId.Guid != data.GUID)
        data.GUID = this.draftId.Guid;
      if (preservePriorLoanNumbering && this.loanData != null)
        data.LoanNumber = this.LoanData.LoanNumber;
      LoanBatchUpdateAccessor.ApplyBatchUpdatesToLoan(data);
      this.loanData = data;
      this.saveLoanData = true;
    }

    public void Import(LoanData data, bool preservePriorLoanNumbering)
    {
      this.validateInstance();
      if (data == null)
        Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("Cannot set loan data to null"));
      if (this.id.Guid != data.GUID)
        data.GUID = this.id.Guid;
      if (preservePriorLoanNumbering && this.loanData != null)
        data.LoanNumber = this.LoanData.LoanNumber;
      LoanBatchUpdateAccessor.ApplyBatchUpdatesToLoan(data);
      this.loanData = data;
      this.saveLoanData = true;
    }

    public bool SentToProcessing()
    {
      this.validateExists();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("LoanSummary"), new string[1]
      {
        "DateSentToProcessing"
      }, new DbValue("Guid", (object) this.id.Guid));
      return !(dbQueryBuilder.ExecuteScalar() is DBNull);
    }

    public string[] GetSupportingDataKeys()
    {
      this.validateExists();
      DirectoryInfo directoryInfo = new DirectoryInfo(ClientContext.GetCurrent().Settings.GetLoanFolderPath(this.id.LoanFolder, this.id.LoanName));
      ArrayList arrayList = new ArrayList();
      foreach (FileInfo file in directoryInfo.GetFiles())
      {
        if (file.Name.ToLower() != "loan.em".ToLower() && file.Extension.ToLower() != "encbak")
          arrayList.Add((object) file.Name);
      }
      return (string[]) arrayList.ToArray(typeof (string));
    }

    public BinaryObject GetSupportingData(string key, bool Lock = true)
    {
      this.validateExists();
      string path = string.Empty;
      try
      {
        path = this.getSupportingDataFilename(key);
        using (DataFile dataFile = Lock ? FileStore.CheckOut(path) : FileStore.GetLatestVersion(path))
          return !dataFile.Exists ? (BinaryObject) null : dataFile.GetData();
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (Loan), string.Format("File don't exist. File: {0}  clientID {1} Key: {2} exception : {3}", (object) path, (object) ClientContext.CurrentRequest.Context.ClientID, (object) key, (object) ex.StackTrace));
        return (BinaryObject) null;
      }
    }

    public bool HasSupportingDataOnDisk(LoanIdentity loanIdentity, string key)
    {
      this.validateExists();
      string path = string.Empty;
      try
      {
        path = this.getSupportingDataFilename(loanIdentity, key);
        using (DataFile latestVersion = FileStore.GetLatestVersion(path))
          return latestVersion.Exists;
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (Loan), string.Format("File don't exist. File: {0}  clientID {1} Key: {2} exception : {3}", (object) path, (object) ClientContext.CurrentRequest.Context.ClientID, (object) key, (object) ex.StackTrace));
        return false;
      }
    }

    public SnapshotObject GetSupportingSnapshotData(
      LogSnapshotType type,
      Guid snapshotGuid,
      string fileNameAsKey,
      bool Lock = true)
    {
      this.validateExists();
      switch ((StorageMode) ClientContext.GetCurrent().Settings.GetStorageSetting("DataStore.StorageMode"))
      {
        case StorageMode.PostgresOnly:
        case StorageMode.BothFileSystemPostgresMaster:
          using (ILoanDataAccessor service = DataAccessFramework.Runtime.CreateService<ILoanDataAccessor>())
            return service.GetLoanSnapshotObject(new Guid(this.id.Guid), snapshotGuid, type);
        default:
          BinaryObject supportingData = this.GetSupportingData(fileNameAsKey, Lock);
          if (supportingData == null)
            return (SnapshotObject) null;
          return new SnapshotObject()
          {
            Type = type,
            ParentId = snapshotGuid,
            Data = supportingData.ToString()
          };
      }
    }

    public BinaryObject GetSupportingLinkedData(string loanFolder, string loanName, string key)
    {
      this.validateExists();
      using (DataFile dataFile = FileStore.CheckOut(this.getSupportingLinkedDataFilename(loanFolder, loanName, key)))
        return !dataFile.Exists ? (BinaryObject) null : dataFile.GetData();
    }

    public void SaveSupportingLinkedData(
      string loanFolder,
      string loanName,
      string key,
      BinaryObject data)
    {
      this.validateInstance();
      using (DataFile dataFile = FileStore.CheckOut(this.getSupportingLinkedDataFilename(loanFolder, loanName, key), MutexAccess.Write))
      {
        if (data == null && dataFile.Exists)
          dataFile.Delete();
        else
          dataFile.CheckIn(data);
      }
    }

    public void SaveSupportingData(string key, BinaryObject data, bool ValidateIfFileExists = false)
    {
      this.validateInstance();
      using (DataFile dataFile = FileStore.CheckOut(this.getSupportingDataFilename(key), MutexAccess.Write))
      {
        if (data == null && dataFile.Exists)
          dataFile.Delete();
        else
          dataFile.CheckIn(data, (ILoanSettings) null, ValidateIfFileExists);
      }
    }

    public void SaveSupportingDataDTUCD(string key, BinaryObject data)
    {
      this.validateInstance();
      string supportingDataFilename = this.getSupportingDataFilename(key);
      if (data == null)
        TraceLog.WriteInfo(nameof (Loan), "UCD snapshot data is empty.");
      using (DataFile dataFile = FileStore.CheckOut(supportingDataFilename, MutexAccess.Write))
      {
        if (data == null && dataFile.Exists)
          TraceLog.WriteInfo(nameof (Loan), "UCD snapshot data is empty and file exists.");
        else
          dataFile.CheckIn(data);
      }
    }

    public void SaveSupportingSnapshotData(
      LogSnapshotType type,
      Guid snapshotGuid,
      string fileNameAsKey,
      SnapshotObject data)
    {
      this.validateInstance();
      StorageMode storageSetting = (StorageMode) ClientContext.GetCurrent().Settings.GetStorageSetting("DataStore.StorageMode");
      Exception exception1 = (Exception) null;
      Exception exception2 = (Exception) null;
      if (storageSetting == StorageMode.PostgresOnly || storageSetting == StorageMode.BothFileSystemPostgresMaster || storageSetting == StorageMode.BothPostgresFileSystemMaster)
      {
        try
        {
          using (ILoanDataAccessor service = DataAccessFramework.Runtime.CreateService<ILoanDataAccessor>())
            service.SaveLoanSnapshotObject(new Guid(this.id.Guid), snapshotGuid, type, data);
        }
        catch (Exception ex)
        {
          exception1 = ex;
        }
      }
      if (storageSetting != StorageMode.PostgresOnly)
      {
        try
        {
          if (type == LogSnapshotType.DisclosureTrackingUCD)
            this.SaveSupportingDataDTUCD(fileNameAsKey, data.ToBinaryObject());
          else
            this.SaveSupportingData(fileNameAsKey, data.ToBinaryObject());
        }
        catch (Exception ex)
        {
          exception2 = ex;
        }
      }
      if (exception1 != null || exception2 != null)
        throw new Exception(exception1.ToString() + "\n" + (object) exception2);
    }

    public void AppendSupportingData(string key, BinaryObject data)
    {
      this.validateInstance();
      using (DataFile dataFile = FileStore.CheckOut(this.getSupportingDataFilename(key), MutexAccess.Write))
      {
        if (!dataFile.Exists)
          dataFile.CheckIn(data);
        else
          dataFile.Append(data, false);
      }
    }

    public bool SupportingDataExists(string key)
    {
      this.validateExists();
      return File.Exists(this.getSupportingDataFilename(key));
    }

    public void AddLoanEventLog(
      LoanEventLogList loanEventLogList,
      UserInfo currentUser,
      string sessionId)
    {
      this.validateInstance();
      StorageMode storageSetting = (StorageMode) ClientContext.GetCurrent().Settings.GetStorageSetting("DataStore.StorageMode");
      LoanFolder loanFolder = new LoanFolder(this.id.LoanFolder);
      if (storageSetting != StorageMode.DatabaseOnly)
      {
        LoanEventLogList logList = loanFolder.ReadLoanEventLog(this.id);
        foreach (LogRecordBase allNonSystemLog in loanEventLogList.GetAllNonSystemLogs())
          logList.InsertNonSystemLog(allNonSystemLog);
        loanFolder.WriteLoanEventLog(this.Identity, logList);
      }
      string fullLoanFilePath = loanFolder.GetFullLoanFilePath(this.id.LoanName);
      bool isSourceEncompass = EncompassServer.ServerMode != EncompassServerMode.Service;
      this.publishLoanEvent(currentUser.Userid, LoanEventType.Update, Guid.Parse(this.id.Guid), sessionId, fullLoanFilePath, this.loanData == null ? 0 : this.loanData.LoanVersionNumber, this.id.LoanFolder, isSourceEncompass, this.LastModifiedUTC);
    }

    public LoanHistoryEntry[] GetLoanHistory(string[] objectList)
    {
      this.validateExists();
      return LoanHistoryStore.GetHistory(this.getSupportingDataFilename("LoanHistory.xml"), objectList);
    }

    public void AppendLoanHistory(LoanHistoryEntry[] entryList)
    {
      this.validateInstance();
      LoanHistoryStore.AppendHistory(this.getSupportingDataFilename("LoanHistory.xml"), entryList);
    }

    public ZipReader CreateZipReader(string key)
    {
      this.validateInstance();
      return new ZipReader(this.getSupportingDataFilename(key));
    }

    public ZipWriter CreateZipWriter(string key, int compressionLevel)
    {
      this.validateInstance();
      return new ZipWriter(this.getSupportingDataFilename(key), compressionLevel);
    }

    public FileAttachment[] GetFileAttachments()
    {
      this.validateExists();
      return FileAttachmentStore.GetFileAttachments(this, true, (IAttachmentXmlProviderFactory) new AttachmentXmlProviderFactory(ClientContext.GetCurrent().Settings));
    }

    public void ReplaceBackgroundAttachment(FileAttachment attachment)
    {
      this.validateInstance();
      FileAttachmentStore.ReplaceBackgroundAttachment(this, attachment, (IAttachmentXmlProviderFactory) new AttachmentXmlProviderFactory(ClientContext.GetCurrent().Settings));
    }

    public void SaveFileAttachments(FileAttachment[] attachmentList)
    {
      this.validateInstance();
      FileAttachmentStore.SaveFileAttachments(this, attachmentList, (IAttachmentXmlProviderFactory) new AttachmentXmlProviderFactory(ClientContext.GetCurrent().Settings));
    }

    public static SkyDriveUrl GetSkyDriveUrlForGet(string objectId, string userId)
    {
      Task<SkyDriveUrl> skyDriveUrlForGet = new SkyDriveRestClient((IClientContext) ClientContext.GetCurrent(), userId).GetSkyDriveUrlForGet(objectId);
      Task.WaitAll((Task) skyDriveUrlForGet);
      return skyDriveUrlForGet.Result;
    }

    public static List<SkyDriveUrl> GetSkyDriveUrlsForGet(string[] objectIds, string userId)
    {
      Task<List<SkyDriveUrl>> skyDriveUrlsForGet = new SkyDriveRestClient((IClientContext) ClientContext.GetCurrent(), userId).GetSkyDriveUrlsForGet(objectIds);
      Task.WaitAll((Task) skyDriveUrlsForGet);
      return skyDriveUrlsForGet.Result;
    }

    public static SkyDriveUrl GetSkyDriveUrlForGet(string loanGuid, string fileKey, string userId)
    {
      Task<SkyDriveUrl> skyDriveUrlForGet = new SkyDriveRestClient((IClientContext) ClientContext.GetCurrent(), userId).GetSkyDriveUrlForGet(loanGuid, fileKey);
      Task.WaitAll((Task) skyDriveUrlForGet);
      return skyDriveUrlForGet.Result;
    }

    public static SkyDriveUrl GetSkyDriveUrlForPut(
      string loanGuid,
      string fileKey,
      string contentType,
      string userId,
      bool useSkyDriveClassic = false)
    {
      Task<SkyDriveUrl> skyDriveUrlForPut = new SkyDriveRestClient((IClientContext) ClientContext.GetCurrent(), userId).GetSkyDriveUrlForPut(loanGuid, fileKey, contentType, useSkyDriveClassic);
      Task.WaitAll((Task) skyDriveUrlForPut);
      return skyDriveUrlForPut.Result;
    }

    public static SkyDriveUrl GetSkyDriveUrlForMeta(
      string objectId,
      string userId,
      string fileName = null)
    {
      Task<SkyDriveUrl> skyDriveUrlForMeta = new SkyDriveRestClient((IClientContext) ClientContext.GetCurrent(), userId).GetSkyDriveUrlForMeta(objectId, fileName);
      Task.WaitAll((Task) skyDriveUrlForMeta);
      return skyDriveUrlForMeta.Result;
    }

    public static string[] GetSkyDriveSupportingDataKeys(string loanGuid)
    {
      Task<string[]> skyDriveFileKeys = new SkyDriveRestClient((IClientContext) ClientContext.GetCurrent()).GetSkyDriveFileKeys(loanGuid);
      Task.WaitAll((Task) skyDriveFileKeys);
      return skyDriveFileKeys.Result;
    }

    public string[] SelectFields(string[] fieldIds)
    {
      return this.SelectFields(this.id, this.LastModified, this.LoanData, fieldIds);
    }

    public string SelectField(string fieldId) => this.LoanData.GetField(fieldId);

    public string[] DraftSelectFields(string[] fieldIds)
    {
      return this.DraftSelectFields(this.draftId, this.LastModified, this.LoanData, fieldIds);
    }

    public string[] DraftSelectFields(
      DraftLoanIdentity loanIdentity,
      DateTime lastModified,
      LoanData loanData,
      string[] fieldIds)
    {
      string[] strArray = new string[fieldIds.Length];
      PipelineInfo pipelineInfo = (PipelineInfo) null;
      bool flag = false;
      string empty = string.Empty;
      for (int index = 0; index < fieldIds.Length; ++index)
      {
        string fieldIdWithPairInfo = fieldIds[index].ToUpper();
        if (fieldIdWithPairInfo.ToUpper().StartsWith("FIELDS."))
          fieldIdWithPairInfo = fieldIdWithPairInfo.Substring(7);
        FieldPairInfo fieldPairInfo = FieldPairParser.ParseFieldPairInfo(fieldIdWithPairInfo);
        int num = Utils.ParseInt((object) fieldPairInfo.FieldID);
        if (fieldIdWithPairInfo.ToLower().StartsWith("audittrail"))
        {
          flag = true;
        }
        else
        {
          switch (fieldIdWithPairInfo)
          {
            case "LOANFOLDER":
              if (pipelineInfo == null)
                pipelineInfo = this.GetPipelineFields(false);
              strArray[index] = pipelineInfo == null ? string.Empty : pipelineInfo.LoanFolder;
              break;
            case "5016":
              if (pipelineInfo == null)
                pipelineInfo = this.GetPipelineFields(false);
              strArray[index] = pipelineInfo == null ? string.Empty : pipelineInfo.IsArchived;
              break;
            case "EDRSXML":
              strArray[index] = loanData.ToXml(true);
              break;
            case "_LPID":
            case "_LOID":
            case "_CLID":
              strArray[index] = loanData.GetField(fieldIds[index].Substring(1).ToUpper());
              break;
            case "LOANLASTMODIFIED":
              if (this.info != null)
                strArray[index] = this.info.LastModified.ToString();
              else if (pipelineInfo == null)
                pipelineInfo = this.GetPipelineFields(false);
              if (pipelineInfo != null && pipelineInfo.Info != null && pipelineInfo.Info.ContainsKey((object) "LastModified"))
              {
                strArray[index] = pipelineInfo.Info[(object) "LastModified"].ToString();
                break;
              }
              break;
            default:
              if (num >= 2028 && num <= 2030 || num >= 2088 && num <= 2144 || num >= 2148 && num <= 2205 || num >= 2219 && num <= 2292 || num >= 2295 && num <= 2297 || num >= 2414 && num <= 2447 || num >= 2448 && num <= 2481 || num >= 2482 && num <= 2515 || num >= 2647 && num <= 2689 || num == 2206 || num == 2215 || num == 2218 || num == 2592 || num == 3055 || num == 3123)
              {
                strArray[index] = loanData.GetField("LOCKRATE." + fieldPairInfo.FieldID);
                break;
              }
              if (fieldPairInfo.PairIndex >= 1)
              {
                strArray[index] = loanData.GetField(fieldIds[index]);
                break;
              }
              string id = fieldIds[index];
              if (id.StartsWith("Fields."))
                id = id.Replace("Fields.", "");
              strArray[index] = loanData.GetField(id, 0);
              break;
          }
          if (fieldIdWithPairInfo == "1393" && (strArray[index] == string.Empty || strArray[index] == null))
          {
            strArray[index] = "Active Loan";
          }
          else
          {
            switch (fieldIdWithPairInfo)
            {
              case "663":
                strArray[index] = !(strArray[index] == "Y") ? "does not" : "does";
                continue;
              case "420":
                if (strArray[index] == "FirstLien")
                {
                  strArray[index] = "First Lien";
                  continue;
                }
                if (strArray[index] == "SecondLien")
                {
                  strArray[index] = "Second Lien";
                  continue;
                }
                continue;
              default:
                if ((fieldIdWithPairInfo.StartsWith("HTD") || fieldIdWithPairInfo.StartsWith("HTR")) && fieldIdWithPairInfo.Length == 7 && fieldIdWithPairInfo.EndsWith("01"))
                {
                  strArray[index] = strArray[index].Replace(",", "");
                  continue;
                }
                continue;
            }
          }
        }
      }
      if (flag)
      {
        string str1 = "";
        string str2 = "";
        string str3 = "";
        LoanXDBAuditField[] reportingLoanXdbField = LoanXDBStore.GetAuditTrailReportingLoanXDBField();
        Hashtable hashtable = new Hashtable();
        bool[] flagArray = new bool[fieldIds.Length];
        foreach (LoanXDBAuditField loanXdbAuditField in reportingLoanXdbField)
          hashtable.Add((object) loanXdbAuditField.ReportingCriterionName, (object) loanXdbAuditField);
        for (int index = 0; index < fieldIds.Length; ++index)
        {
          if (hashtable.ContainsKey((object) fieldIds[index]))
          {
            LoanXDBAuditField loanXdbAuditField = (LoanXDBAuditField) hashtable[(object) fieldIds[index]];
            string str4 = loanXdbAuditField.ColumnName + "_" + loanXdbAuditField.DatabaseField.ColumnName;
            if (str1 == string.Empty)
              str1 = str4 + "." + loanXdbAuditField.ColumnName + " as " + str4;
            else
              str1 = str1 + ", " + str4 + "." + loanXdbAuditField.ColumnName + " as " + str4;
            if (loanXdbAuditField.FieldType == LoanXDBTableList.TableTypes.IsDate)
              flagArray[index] = true;
            if (str2 != "")
              str2 += " AND ";
            str2 = str2 + "(" + str4 + ".ModifiedDTTM = (Select Max(ModifiedDTTM) From AuditTrail where FieldXRef = " + (object) loanXdbAuditField.DatabaseField.FieldXRefID + " and LoanXRef = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.Identity.XrefId) + "))";
            if (str3 != "")
              str3 += ", ";
            str3 = str3 + "(select * from AuditTrail where FieldXRef = " + (object) loanXdbAuditField.DatabaseField.FieldXRefID + ") as " + str4;
          }
        }
        if (str3 != "")
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          string text = "SELECT " + str1 + " FROM " + str3 + " WHERE " + str2;
          dbQueryBuilder.Append(text);
          DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
          if (dataRowCollection != null && dataRowCollection.Count > 0)
          {
            for (int index = 0; index < fieldIds.Length; ++index)
            {
              if (hashtable.ContainsKey((object) fieldIds[index]))
              {
                LoanXDBAuditField loanXdbAuditField = (LoanXDBAuditField) hashtable[(object) fieldIds[index]];
                string columnName = loanXdbAuditField.ColumnName + "_" + loanXdbAuditField.DatabaseField.ColumnName;
                string str5 = string.Concat(dataRowCollection[0][columnName]);
                if (flagArray[index])
                {
                  DateTime dateTime = DateTime.Parse(string.Concat(EllieMae.EMLite.DataAccess.SQL.Decode((object) str5, (object) DateTime.MinValue)));
                  strArray[index] = !(dateTime != DateTime.MinValue) ? string.Empty : dateTime.ToString("G");
                }
                else
                  strArray[index] = str5;
              }
            }
          }
        }
      }
      return strArray;
    }

    private string SelectField(
      LoanData loanData,
      bool useFieldTrackerValues,
      string fieldId,
      int? borIndex = null)
    {
      FieldChangeInfo fieldChangeInfo;
      if (useFieldTrackerValues && loanData.FieldChanges.TryGetValue(fieldId, out fieldChangeInfo))
        return fieldChangeInfo.NewValue;
      return !borIndex.HasValue ? loanData.GetField(fieldId) : loanData.GetField(fieldId, borIndex.Value);
    }

    public string[] SelectFields(
      LoanIdentity loanIdentity,
      DateTime lastModified,
      LoanData loanData,
      string[] fieldIds)
    {
      string[] strArray = new string[fieldIds.Length];
      PipelineInfo pipelineInfo = (PipelineInfo) null;
      bool flag = false;
      string empty = string.Empty;
      FieldChangeTracker fieldChangeTracker = loanData.FieldChangeTracker;
      bool useFieldTrackerValues = fieldChangeTracker != null && fieldChangeTracker.UseFieldChangesValues;
      for (int index = 0; index < fieldIds.Length; ++index)
      {
        string str = fieldIds[index].ToUpper();
        if (str.ToUpper().StartsWith("FIELDS."))
          str = str.Substring(7);
        FieldPairInfo fieldPairInfo = FieldPairParser.ParseFieldPairInfo(str);
        int num = Utils.ParseInt((object) fieldPairInfo.FieldID);
        if (str.ToLower().StartsWith("audittrail"))
        {
          flag = true;
        }
        else
        {
          switch (str)
          {
            case "LOANFOLDER":
              if (pipelineInfo == null)
                pipelineInfo = this.GetPipelineFields(false);
              strArray[index] = pipelineInfo == null ? string.Empty : pipelineInfo.LoanFolder;
              break;
            case "5016":
              if (pipelineInfo == null)
                pipelineInfo = this.GetPipelineFields(false);
              strArray[index] = pipelineInfo == null ? string.Empty : pipelineInfo.IsArchived;
              break;
            case "EDRSXML":
              strArray[index] = loanData.ToXml(true);
              break;
            case "_LPID":
            case "_LOID":
            case "_CLID":
              strArray[index] = this.SelectField(loanData, useFieldTrackerValues, fieldIds[index].Substring(1).ToUpper());
              break;
            case "LOANLASTMODIFIED":
              if (this.info != null)
                strArray[index] = this.info.LastModified.ToString();
              else if (pipelineInfo == null)
                pipelineInfo = this.GetPipelineFields(false);
              if (pipelineInfo != null && pipelineInfo.Info != null && pipelineInfo.Info.ContainsKey((object) "LastModified"))
              {
                strArray[index] = pipelineInfo.Info[(object) "LastModified"].ToString();
                break;
              }
              break;
            default:
              if (num >= 2028 && num <= 2030 || num >= 2088 && num <= 2144 || num >= 2148 && num <= 2205 || num >= 2219 && num <= 2292 || num >= 2295 && num <= 2297 || num >= 2414 && num <= 2447 || num >= 2448 && num <= 2481 || num >= 2482 && num <= 2515 || num >= 2647 && num <= 2689 || num == 2206 || num == 2215 || num == 2218 || num == 2592 || num == 3055 || num == 4751 || num == 3123 || num >= 3371 && num <= 3378 || num >= 4753 && num <= 4784 || num == 4787 || num == 4789 || num == 4790 || num == 4791 || num == 4857 || num == 4858)
              {
                strArray[index] = this.SelectField(loanData, useFieldTrackerValues, "LOCKRATE." + fieldPairInfo.FieldID);
                break;
              }
              if (fieldPairInfo.PairIndex >= 1)
              {
                strArray[index] = this.SelectField(loanData, useFieldTrackerValues, str);
                break;
              }
              string fieldId = fieldIds[index];
              if (fieldId.StartsWith("Fields."))
                fieldId = fieldId.Replace("Fields.", "");
              strArray[index] = this.SelectField(loanData, useFieldTrackerValues, fieldId, new int?(0));
              break;
          }
          if (str == "1393" && (strArray[index] == string.Empty || strArray[index] == null))
          {
            strArray[index] = "Active Loan";
          }
          else
          {
            switch (str)
            {
              case "663":
                strArray[index] = !(strArray[index] == "Y") ? "does not" : "does";
                continue;
              case "420":
                if (strArray[index] == "FirstLien")
                {
                  strArray[index] = "First Lien";
                  continue;
                }
                if (strArray[index] == "SecondLien")
                {
                  strArray[index] = "Second Lien";
                  continue;
                }
                continue;
              default:
                if ((str.StartsWith("HTD") || str.StartsWith("HTR")) && str.Length == 7 && str.EndsWith("01"))
                {
                  strArray[index] = strArray[index].Replace(",", "");
                  continue;
                }
                continue;
            }
          }
        }
      }
      if (flag)
      {
        string str1 = "";
        string str2 = "";
        string str3 = "";
        LoanXDBAuditField[] reportingLoanXdbField = LoanXDBStore.GetAuditTrailReportingLoanXDBField();
        Hashtable hashtable = new Hashtable();
        bool[] flagArray = new bool[fieldIds.Length];
        foreach (LoanXDBAuditField loanXdbAuditField in reportingLoanXdbField)
          hashtable.Add((object) loanXdbAuditField.ReportingCriterionName, (object) loanXdbAuditField);
        for (int index = 0; index < fieldIds.Length; ++index)
        {
          if (hashtable.ContainsKey((object) fieldIds[index]))
          {
            LoanXDBAuditField loanXdbAuditField = (LoanXDBAuditField) hashtable[(object) fieldIds[index]];
            string str4 = loanXdbAuditField.ColumnName + "_" + loanXdbAuditField.DatabaseField.ColumnName;
            if (str1 == string.Empty)
              str1 = str4 + "." + loanXdbAuditField.ColumnName + " as " + str4;
            else
              str1 = str1 + ", " + str4 + "." + loanXdbAuditField.ColumnName + " as " + str4;
            if (loanXdbAuditField.FieldType == LoanXDBTableList.TableTypes.IsDate)
              flagArray[index] = true;
            if (str2 != "")
              str2 += " AND ";
            str2 = str2 + "(" + str4 + ".ModifiedDTTM = (Select Max(ModifiedDTTM) From AuditTrail where FieldXRef = " + (object) loanXdbAuditField.DatabaseField.FieldXRefID + " and LoanXRef = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.Identity.XrefId) + "))";
            if (str3 != "")
              str3 += ", ";
            str3 = str3 + "(select atr.*, u.first_name as FirstName, u.last_name as LastName from AuditTrail atr left outer join users u on atr.UserID = u.userid where FieldXRef = " + (object) loanXdbAuditField.DatabaseField.FieldXRefID + ") as " + str4;
          }
        }
        if (str3 != "")
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          string text = "SELECT " + str1 + " FROM " + str3 + " WHERE " + str2;
          dbQueryBuilder.Append(text);
          DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
          if (dataRowCollection != null && dataRowCollection.Count > 0)
          {
            for (int index = 0; index < fieldIds.Length; ++index)
            {
              if (hashtable.ContainsKey((object) fieldIds[index]))
              {
                LoanXDBAuditField loanXdbAuditField = (LoanXDBAuditField) hashtable[(object) fieldIds[index]];
                string columnName = loanXdbAuditField.ColumnName + "_" + loanXdbAuditField.DatabaseField.ColumnName;
                string str5 = string.Concat(dataRowCollection[0][columnName]);
                if (flagArray[index])
                {
                  DateTime dateTime = DateTime.Parse(string.Concat(EllieMae.EMLite.DataAccess.SQL.Decode((object) str5, (object) DateTime.MinValue)));
                  strArray[index] = !(dateTime != DateTime.MinValue) ? string.Empty : dateTime.ToString("G");
                }
                else
                  strArray[index] = str5;
              }
            }
          }
        }
      }
      return strArray;
    }

    private string dateToReport(DateTime date)
    {
      return date != DateTime.MinValue && date != DateTime.MaxValue ? date.ToString("MM/dd/yyyy") : "";
    }

    public PipelineInfo GetPipelineInfo(UserInfo userInfo, bool isExternalOrganization)
    {
      return this.GetPipelineInfo(userInfo, isExternalOrganization, 0);
    }

    public PipelineInfo GetPipelineInfo(
      UserInfo userInfo,
      bool isExternalOrganization,
      int sqlRead)
    {
      return this.GetPipelineInfo(userInfo, (string[]) null, PipelineData.All, isExternalOrganization, sqlRead);
    }

    public PipelineInfo GetPipelineInfo(
      UserInfo userInfo,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization)
    {
      return this.GetPipelineInfo(userInfo, fields, dataToInclude, isExternalOrganization, 0);
    }

    public PipelineInfo GetPipelineInfo(
      UserInfo userInfo,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      int sqlRead)
    {
      this.validateExists();
      return Pipeline.GetPipelineInfo(userInfo, this.id.Guid, fields, dataToInclude, isExternalOrganization, sqlRead);
    }

    public PipelineInfo GetPipelineFields(bool isExternalOrganization)
    {
      return this.GetPipelineInfo((UserInfo) null, (string[]) null, PipelineData.Fields, isExternalOrganization);
    }

    public PipelineInfo GetPipelineFields(bool isExternalOrganization, int sqlRead)
    {
      return this.GetPipelineInfo((UserInfo) null, (string[]) null, PipelineData.Fields, isExternalOrganization, sqlRead);
    }

    public void SyncLastModifiedDate()
    {
      this.validateInstance();
      DateTime modificationDate = new LoanFolder(this.id.LoanFolder).GetLoanDataModificationDate(this.id.LoanName);
      if (!(modificationDate != DateTime.MinValue))
        return;
      this.LastModified = modificationDate;
    }

    public void CreateNew(
      string folderName,
      string loanName,
      LoanServerInfo info,
      LoanData data,
      UserInfo currentUser,
      string sessionId,
      bool allowDeferrable = false,
      string auditUserId = null)
    {
      this.CreateNew(folderName, loanName, info, data, currentUser, false, sessionId, allowDeferrable, auditUserId);
    }

    public void CreateNew(
      string folderName,
      string loanName,
      LoanServerInfo info,
      LoanData data,
      UserInfo currentUser,
      bool allowOverwrite,
      string sessionId,
      bool allowDeferrable = false,
      string auditUserId = null)
    {
      this.validateInstance(false);
      if (this.Exists)
        Err.Raise(TraceLevel.Warning, nameof (Loan), new ServerException("Cannot create new loan with existing Guid '" + this.id.Guid + "'"));
      if ((folderName ?? "") == "")
        Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("Invalid Loan Folder for new loan"));
      if ((loanName ?? "") == "")
        Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("Invalid Loan Name for new loan"));
      if (info == null)
        Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("Invalid LoanServerInfo for new loan"));
      if (data == null)
        Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("Invalid LoanData for new loan"));
      LoanIdentity objectId = Loan.LookupIdentity(folderName, loanName);
      if (objectId != (LoanIdentity) null)
        Err.Raise(TraceLevel.Warning, nameof (Loan), (ServerException) new DuplicateObjectException("Cannot overwrite existing loan", ObjectType.Loan, (object) objectId));
      PerformanceMeter current = PerformanceMeter.Current;
      LoanIdentity newIdentity = new LoanIdentity(folderName, loanName, this.id.Guid);
      if (this.isRebuidFlow)
        this.id = newIdentity;
      if (data.GUID != this.id.Guid)
        data.GUID = this.id.Guid;
      this.grantFileStarterFullRights(data, info);
      current.AddCheckpoint("CreateNew: Granted File Starter access rights to loan...", 1876, nameof (CreateNew), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
      this.info = info;
      this.rightsModified = false;
      this.loanData = data;
      this.exists = true;
      string str = (string) null;
      Thread thread = (Thread) null;
      current.AddCheckpoint("CreateNew: Acquired Read lock for Reporting Database...", 1888, nameof (CreateNew), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
      try
      {
        Loan.serializeLoanData(newIdentity, data, allowOverwrite, currentUser, sessionId);
        current.AddCheckpoint("CreateNew: Serialized loan data to disk...", 1894, nameof (CreateNew), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        Loan.writeDatabaseLock(newIdentity, info);
        current.AddCheckpoint("CreateNew: Wrote loan lock to database...", 1896, nameof (CreateNew), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        Loan.updateDatabaseRights(newIdentity, info.UserRightPairs);
        current.AddCheckpoint("CreateNew: Wrote loan access rights to database...", 1898, nameof (CreateNew), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        info.IsLockModified = false;
        info.LastModified = DateTime.Now;
        ClientContext currentContext = ClientContext.GetCurrent();
        if (data.IsDraft)
        {
          Loan.updateDraftLoanSummary(newIdentity, data, currentUser, info.LastModified);
        }
        else
        {
          if (Loan.IsAsyncSaveEnabled())
          {
            thread = new Thread((ThreadStart) (() => this.updateLoanSummaryAsync(newIdentity, data, currentUser, info.LastModified, info.LastModified, currentContext, allowDeferrable)));
            thread.Start();
          }
          else
            Loan.updateLoanSummary(newIdentity, data, currentUser, info.LastModified, info.LastModified);
          this.setLoanPropertySettings(this.isRebuidFlow, this.HasSupportingDataOnDisk(newIdentity, FileAttachmentStore.AttachmentFileName));
        }
      }
      catch
      {
        this.destroyLoan(newIdentity, false, false, currentUser);
        throw;
      }
      try
      {
        if (LoanNameFolderGenerator.GetMaxEntriesInAFolder(ClientContext.GetCurrent()) > 0)
          LoanNameFolderGenerator.ChangeLoanCount(folderName, LoanNameFolderGenerator.GetLoanNameFolderPart(loanName), 1);
        current.AddCheckpoint("CreateNew: Updated loan count in folder ...", 1954, nameof (CreateNew), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
      }
      catch
      {
      }
      this.loanData.FixMilestoneIDs(WorkflowBpmDbAccessor.GetCompleteMilestoneNameToGUID());
      current.AddCheckpoint("CreateNew: Completed Milestone ID fixups ...", 1963, nameof (CreateNew), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
      thread?.Join();
      this.id = Loan.LookupIdentity(this.id.Guid);
      if (currentUser != (UserInfo) null && !data.IsDraft)
      {
        if (allowDeferrable)
        {
          using (DeferrableLoanTransactionContext transactionContext = new DeferrableLoanTransactionContext(DeferrableProcessorRole.Publisher, currentUser, this, auditUserId: auditUserId))
          {
            transactionContext.SetUrn("elli", "encompass", ClientContext.GetCurrent().InstanceName, "", "CreateLoan", currentUser.Userid).SetCurrentLoanData(this.loanData).SetXDBModifiedTime(info.LastModified).SetAuditModifiedTime(DateTime.MinValue).SetAuditCurrentTime(DateTime.Now);
            using (DeferrableLoanTransaction deferrableLoanTransaction = new DeferrableLoanTransaction((IDeferrableTransactionContext) transactionContext))
            {
              deferrableLoanTransaction.Initialize(current);
              deferrableLoanTransaction.CurrentContext.DataBag.Set("AfterLoanFileName", (object) string.Format("{0:D5}_{1}", (object) data.LoanVersionNumber, (object) "loan.em"));
              deferrableLoanTransaction.Complete(new DeferrableType?());
              str = deferrableLoanTransaction.GetError(new RealTimeReportingDbProcessor((DeferrableLoanTransaction) null).GetKey());
            }
          }
        }
        else
        {
          str = this.updateLoanXDBTables(info.LastModified, (LoanData) null);
          current.AddCheckpoint("CreateNew: Updated Reporting Database tables ...", 2004, nameof (CreateNew), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
          this.updateAuditTrail(currentUser, (LoanData) null, DateTime.MinValue, auditUserId);
          current.AddCheckpoint("CreateNew: Added Audit Trail records ...", 2006, nameof (CreateNew), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        }
      }
      this.innerLock.UndoCheckout();
      current.AddCheckpoint("CreateNew: Checked loan into LoanStore ...", 2012, nameof (CreateNew), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
      if (!data.IsDraft)
      {
        string fullLoanFilePath = new LoanFolder(this.id.LoanFolder).GetFullLoanFilePath(this.id.LoanName);
        this.publishLoanEvent(currentUser.Userid, LoanEventType.Create, Guid.Parse(this.id.Guid), sessionId, fullLoanFilePath, this.loanData == null ? 0 : this.loanData.LoanVersionNumber, this.id.LoanFolder, !allowDeferrable, this.LastModifiedUTC);
        current.AddCheckpoint("CreateNew: Published Loan Event...", 2023, nameof (CreateNew), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        this.InitiateLoanBillingProcess((LoanData) null, this.loanData, currentUser, LoanEventType.Create);
      }
      if (str == null)
        return;
      Err.Raise(nameof (Loan), new ServerException("Error updating reporting database:\r\n" + str));
    }

    private void InitiateLoanBillingProcess(
      LoanData priorData,
      LoanData loanData,
      UserInfo currentUser,
      LoanEventType loanEventType)
    {
      try
      {
        TransactionLog.ReasonCode reasonCode;
        if (EncompassServer.ServerMode != EncompassServerMode.Service || !this.IsClosedLoanBillingActivated(priorData, loanData, out reasonCode))
          return;
        this.PublishLoanBillingKafkaEvent(this.id.Guid, loanEventType, currentUser, new TransactionLog(loanData, currentUser, this.id, reasonCode));
        PerformanceMeter.Current.AddCheckpoint("CheckIn: Published Loan billing Event...", 2048, nameof (InitiateLoanBillingProcess), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("Exception in InitiateLoanBillingProcess for loanId - {0}. Exception Message - {1}, details - {2}, Source - {3}", (object) this.id.Guid, (object) ex.Message, (object) ex.StackTrace, (object) ex.Source));
      }
    }

    private void EvaluateDocumentReceivedAndPublishEvent(
      LoanData priorData,
      LoanData loanData,
      string userId)
    {
      try
      {
        DocumentReceivedEvent documentReceived;
        if (!this.IsDocumentAssignedWithAttachment(priorData, loanData, out documentReceived))
          return;
        this.PublishDocumentReceivedEventKafka(this.id.Guid, userId, documentReceived);
        PerformanceMeter.Current.AddCheckpoint("CheckIn: Published DocumentReceived Event...", 2072, nameof (EvaluateDocumentReceivedAndPublishEvent), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("Exception in EvaluateDocumentReceivedAndPublishEvent for loanId - {0}. Exception Message - {1}, details - {2}, Source - {3}", (object) this.id.Guid, (object) ex.Message, (object) ex.StackTrace, (object) ex.Source));
      }
    }

    private void setLoanPropertySettings(bool isRebuild = false, bool hasAttachmentsOnDisk = false)
    {
      ILogger logger = DiagUtility.LogManager.GetLogger("PipelineRebuild");
      int num = isRebuild & hasAttachmentsOnDisk ? 1 : 0;
      string[] strArray = new string[1]{ "LoanStorage" };
      foreach (LoanProperty fromCompanySetting in Loan.getLoanPropertySettingsFromCompanySettings(num != 0, strArray))
      {
        if (isRebuild && fromCompanySetting.Category.Equals("LoanStorage", StringComparison.InvariantCultureIgnoreCase) && fromCompanySetting.Attribute.Equals("SupportingData", StringComparison.InvariantCultureIgnoreCase))
        {
          string storageType = this.CalculateStorageType();
          if (!string.IsNullOrEmpty(storageType))
          {
            logger.Write(Encompass.Diagnostics.Logging.LogLevel.INFO, nameof (Loan), "Updating LoanStorage via CalculateStorageType for loanid:'" + this.loanData.GUID + "' SupportingData set to: '" + storageType + "'");
            this.SetLoanPropertySetting(new LoanProperty("LoanStorage", "SupportingData", storageType));
            continue;
          }
        }
        this.SetLoanPropertySetting(fromCompanySetting);
      }
    }

    public void SetLoanPropertySetting(LoanProperty lp)
    {
      LoanPropertySettingsAccessor.AppendUpdateRecord(this.id.Guid, lp);
    }

    private static LoanProperty[] getLoanPropertySettingsFromCompanySettings(
      bool hasAttachmentsOnDisk,
      params string[] sections)
    {
      Dictionary<string, Hashtable> companySettings = Company.GetCompanySettings(sections);
      List<LoanProperty> loanPropertyList = new List<LoanProperty>();
      Hashtable hashtable;
      if (!companySettings.TryGetValue("LoanStorage", out hashtable) || hashtable == null || !hashtable.ContainsKey((object) "SupportingData"))
      {
        LoanProperty loanProperty = new LoanProperty("LoanStorage", "SupportingData", "CIFS");
        loanPropertyList.Add(loanProperty);
      }
      if (hasAttachmentsOnDisk && hashtable != null && hashtable.ContainsKey((object) "AttachmentsMetaData") && hashtable[(object) "AttachmentsMetaData"].ToString().Equals("DB", StringComparison.InvariantCultureIgnoreCase))
      {
        LoanProperty loanProperty = new LoanProperty("LoanStorage", "AttachmentsMetaData", "CIFS");
        loanPropertyList.Add(loanProperty);
        hashtable.Remove((object) "AttachmentsMetaData");
      }
      foreach (KeyValuePair<string, Hashtable> keyValuePair in companySettings)
      {
        foreach (DictionaryEntry dictionaryEntry in keyValuePair.Value)
        {
          LoanProperty loanProperty = new LoanProperty(keyValuePair.Key, dictionaryEntry.Key.ToString(), dictionaryEntry.Value.ToString());
          loanPropertyList.Add(loanProperty);
        }
      }
      return loanPropertyList.ToArray();
    }

    private string CalculateStorageType()
    {
      FileAttachment[] fileAttachments = this.GetFileAttachments();
      string empty = string.Empty;
      Hashtable companySettings = Company.GetCompanySettings("LoanStorage");
      string str = "";
      if (companySettings.Count > 0 && companySettings.Contains((object) "SupportingData"))
        str = companySettings[(object) "SupportingData"].ToString();
      if (!string.IsNullOrWhiteSpace(str) && str.Equals("SkyDriveClassic", StringComparison.InvariantCultureIgnoreCase))
        return "SkyDriveClassic";
      List<string> first = new List<string>();
      if (fileAttachments != null && ((IEnumerable<FileAttachment>) fileAttachments).Count<FileAttachment>() > 0)
      {
        foreach (FileAttachment fileAttachment in fileAttachments)
        {
          switch (fileAttachment)
          {
            case CloudAttachment _:
              return "SkyDrive";
            case NativeAttachment _:
              if (!first.Contains(fileAttachment.ID))
              {
                first.Add(fileAttachment.ID);
                break;
              }
              break;
            case ImageAttachment _:
              PageImageCollection pages = ((ImageAttachment) fileAttachment).Pages;
              foreach (PageImage pageImage in ((ImageAttachment) fileAttachment).Pages.ToArray())
              {
                if (!first.Contains(pageImage.ZipKey))
                  first.Add(pageImage.ZipKey);
              }
              break;
          }
        }
        List<string> list = ((IEnumerable<string>) this.GetSupportingDataKeys()).ToList<string>();
        for (int index = list.Count - 1; index >= 0; --index)
        {
          if (Utils.IsCIFsOnlyFile(list[index]))
            list.RemoveAt(index);
        }
        return first.Intersect<string>((IEnumerable<string>) Loan.GetSkyDriveSupportingDataKeys(this.loanData.GUID)).Count<string>() >= first.Intersect<string>((IEnumerable<string>) list).Count<string>() ? "SkyDriveLite" : "CIFS";
      }
      int num1 = ((IEnumerable<string>) Loan.GetSkyDriveSupportingDataKeys(this.loanData.GUID)).Count<string>();
      string[] supportingDataKeys = this.GetSupportingDataKeys();
      int num2 = 0;
      foreach (string file in supportingDataKeys)
      {
        if (!Utils.IsCIFsOnlyFile(file))
          ++num2;
      }
      if (num1 == 0 && num2 == 0)
        return string.Empty;
      return num1 >= num2 ? (!string.IsNullOrWhiteSpace(str) && str.Equals("SkyDrive", StringComparison.InvariantCultureIgnoreCase) ? "SkyDrive" : "SkyDriveLite") : (!string.IsNullOrWhiteSpace(str) && str.Equals("SkyDriveClassic", StringComparison.InvariantCultureIgnoreCase) ? "SkyDriveClassic" : "CIFS");
    }

    public void LoanApplicationSubmit(
      string folderName,
      string loanName,
      LoanServerInfo info,
      LoanData data,
      UserInfo currentUser,
      string sessionId)
    {
      this.LoanApplicationSubmit(folderName, loanName, info, data, currentUser, false, sessionId);
    }

    public void LoanApplicationSubmit(
      string folderName,
      string loanName,
      LoanServerInfo info,
      LoanData data,
      UserInfo currentUser,
      bool allowOverwrite,
      string sessionId)
    {
      this.validateInstance(false);
      if (data == null)
        Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("(LoanApplicationSubmit) Invalid LoanData for new loan. LoanId: " + loanName));
      if (this.Exists)
      {
        data.DefrdRetryAllowed = false;
        data.DefrdReSubmitAllowed = false;
        data.DefrdBorrowerActionRequired = false;
        data.DefrdLenderActionRequired = false;
        data.DefrdEncompassLevelActionRequired = false;
        data.DefrdLoanAppSubmitEventRequired = false;
        Err.Raise(TraceLevel.Warning, nameof (Loan), new ServerException("(LoanApplicationSubmit) Cannot create new loan with existing Guid '" + this.id.Guid + "'"));
      }
      if ((folderName ?? "") == "")
      {
        data.DefrdRetryAllowed = false;
        data.DefrdReSubmitAllowed = true;
        data.DefrdBorrowerActionRequired = true;
        data.DefrdLenderActionRequired = true;
        data.DefrdEncompassLevelActionRequired = false;
        data.DefrdLoanAppSubmitEventRequired = false;
        Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("(LoanApplicationSubmit) Invalid (blank) Loan Folder for new loan. LoanId: " + loanName));
      }
      if ((loanName ?? "") == "")
      {
        data.DefrdRetryAllowed = false;
        data.DefrdReSubmitAllowed = true;
        data.DefrdBorrowerActionRequired = true;
        data.DefrdLenderActionRequired = true;
        data.DefrdEncompassLevelActionRequired = false;
        data.DefrdLoanAppSubmitEventRequired = false;
        Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("(LoanApplicationSubmit) Invalid (blank) Loan Name for new loan. LoanId: " + loanName));
      }
      if (info == null)
      {
        data.DefrdRetryAllowed = true;
        data.DefrdReSubmitAllowed = true;
        data.DefrdBorrowerActionRequired = false;
        data.DefrdLenderActionRequired = false;
        data.DefrdEncompassLevelActionRequired = false;
        data.DefrdLoanAppSubmitEventRequired = false;
        Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("(LoanApplicationSubmit) Invalid LoanServerInfo for new loan. LoanId: " + loanName));
      }
      LoanIdentity objectId = Loan.LookupIdentity(folderName, loanName);
      if (objectId != (LoanIdentity) null)
      {
        data.DefrdRetryAllowed = false;
        data.DefrdReSubmitAllowed = false;
        data.DefrdBorrowerActionRequired = false;
        data.DefrdLenderActionRequired = false;
        data.DefrdEncompassLevelActionRequired = false;
        data.DefrdLoanAppSubmitEventRequired = false;
        Err.Raise(TraceLevel.Warning, nameof (Loan), (ServerException) new DuplicateObjectException("(LoanApplicationSubmit) Cannot overwrite existing loan already exists in the DB. LoanId: " + loanName, ObjectType.Loan, (object) objectId));
      }
      PerformanceMeter current = PerformanceMeter.Current;
      LoanIdentity id = new LoanIdentity(folderName, loanName, this.id.Guid);
      if (data.GUID != this.id.Guid)
        data.GUID = this.id.Guid;
      this.grantFileStarterFullRights(data, info);
      current.AddCheckpoint("LoanApplicationSubmit: Granted File Starter access rights to loan...", 2340, nameof (LoanApplicationSubmit), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
      current.AddCheckpoint("LoanApplicationSubmit: Acquired Read lock for Reporting Database...", 2343, nameof (LoanApplicationSubmit), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
      int num1 = 1;
      try
      {
        data.DefrdRetryAllowed = false;
        data.DefrdReSubmitAllowed = false;
        data.DefrdBorrowerActionRequired = false;
        data.DefrdLenderActionRequired = true;
        data.DefrdEncompassLevelActionRequired = true;
        try
        {
          Loan.serializeLoanData(id, data, allowOverwrite, currentUser, sessionId);
          current.AddCheckpoint("LoanApplicationSubmit: Serialized loan data to disk...", 2361, nameof (LoanApplicationSubmit), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (Loan), string.Format("(LoanApplicationSubmit) Error creating Loan.em file. LoanId:{0}, Source: {1}, Exception: {2}", (object) this.id.Guid, (object) ex.Source, (object) ex));
          throw;
        }
        try
        {
          Loan.writeDatabaseLock(id, info);
          current.AddCheckpoint("LoanApplicationSubmit: Wrote loan lock to database...", 2372, nameof (LoanApplicationSubmit), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
          Loan.updateDatabaseRights(id, info.UserRightPairs);
          current.AddCheckpoint("LoanApplicationSubmit: Wrote loan access rights to database...", 2374, nameof (LoanApplicationSubmit), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
          info.IsLockModified = false;
          info.LastModified = DateTime.Now;
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (Loan), string.Format("(LoanApplicationSubmit) Error on writeDatabaseLock or updateDatabaseRights. LoanId:{0}, Source: {1}, Exception: {2}", (object) this.id.Guid, (object) ex.Source, (object) ex));
          throw;
        }
        try
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          dbQueryBuilder.Append("IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TestAtomicSubmission') BEGIN SELECT * FROM TestAtomicSubmission END ELSE BEGIN SELECT 0 AS ThrowSQLError, 1  AS DeleteLoanFileOnError END ");
          DataRow dataRow = dbQueryBuilder.ExecuteRowQuery();
          int num2 = (int) EllieMae.EMLite.DataAccess.SQL.Decode(dataRow["ThrowSQLError"], (object) 0);
          num1 = (int) EllieMae.EMLite.DataAccess.SQL.Decode(dataRow["DeleteLoanFileOnError"], (object) 1);
          if (num2 == 1)
          {
            dbQueryBuilder.Remove(0, dbQueryBuilder.Length);
            throw new Loan.SQLTestException("SQL query failed due to transaction blocking after retrying 10 times. (Simulated error)");
          }
          Loan.updateLoanSummary(id, data, currentUser, info.LastModified, info.LastModified);
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (Loan), string.Format("(LoanApplicationSubmit) Error updating the LoanSummary table. LoanId:{0}, Exception: {1}, Source: {2}", (object) this.id.Guid, (object) ex.Source, (object) ex));
          throw;
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("(LoanApplicationSubmit) Error creating Loan.em file or writing to the SQL DB for LoanId: {0}, LoanFolder: {1}, LO: {2}, EncompassVersion: {3}, Source: {4}, Exception: {5}", (object) this.id.Guid, (object) folderName, (object) currentUser.userName, (object) data.EncompassVersion, (object) ex.Source, (object) ex));
        bool deleteAllData = num1 == 1;
        this.destroyLoan(id, deleteAllData, false, currentUser);
        TraceLog.WriteError(nameof (Loan), string.Format("(LoanApplicationSubmit) For error creating Loan.em file or writing to the SQL DB called the method destroyLoan and it {0} LoanId: {1}, LoanFolder: {2}, LO: {3}, EncompassVersion: {4}, Source: {5}, Exception: {6}", deleteAllData ? (object) "WILL delete the Loan.em file" : (object) "will NOT delete the Loan.em file", (object) this.id.Guid, (object) folderName, (object) currentUser.userName, (object) data.EncompassVersion, (object) ex.Source, (object) ex));
        data.DefrdRetryAllowed = true;
        data.DefrdReSubmitAllowed = true;
        data.DefrdBorrowerActionRequired = true;
        data.DefrdLenderActionRequired = true;
        data.DefrdEncompassLevelActionRequired = false;
        data.DefrdLoanAppSubmitEventRequired = false;
        throw;
      }
      try
      {
        this.setLoanPropertySettings();
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("(LoanApplicationSubmit) Exception logged and ignored when setting the Loan Properties Settings for Skydrive support, this error will be swallowed. LoanId: {0}, LoanFolder: {1}, LO: {2}, EncompassVersion: {3}, Source: {4}, Exception: {5}", (object) this.id.Guid, (object) folderName, (object) currentUser.userName, (object) data.EncompassVersion, (object) ex.Source, (object) ex));
      }
      try
      {
        if (LoanNameFolderGenerator.GetMaxEntriesInAFolder(ClientContext.GetCurrent()) > 0)
          LoanNameFolderGenerator.ChangeLoanCount(folderName, LoanNameFolderGenerator.GetLoanNameFolderPart(loanName), 1);
        current.AddCheckpoint("LoanApplicationSubmit: Updated loan count in folder ...", 2454, nameof (LoanApplicationSubmit), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("(LoanApplicationSubmit) Exception logged and ignored in LoanNameFolderGenerator methods for LoanId: {0}, LoanFolder: {1}, LO: {2}, EncompassVersion: {3}, Source: {4}, Exception: {5} ", (object) this.id.Guid, (object) folderName, (object) currentUser.userName, (object) data.EncompassVersion, (object) ex.Source, (object) ex));
      }
      this.info = info;
      this.rightsModified = false;
      this.loanData = data;
      this.exists = true;
      try
      {
        if (!string.IsNullOrWhiteSpace(ClientContext.GetCurrent().InstanceName) && this._xdbInstances.IndexOf(ClientContext.GetCurrent().InstanceName, StringComparison.OrdinalIgnoreCase) >= 0)
        {
          this._xdbLogs = true;
          TraceLog.WriteWarning(nameof (Loan), "updateLoanXDBTables: Enables logs for updating reporting database for " + ClientContext.GetCurrent().InstanceName);
        }
        this.loanData.FixMilestoneIDs(WorkflowBpmDbAccessor.GetCompleteMilestoneNameToGUID());
        current.AddCheckpoint("LoanApplicationSubmit: Completed Milestone ID fixups ...", 2484, nameof (LoanApplicationSubmit), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        this.id = Loan.LookupIdentity(this.id.Guid);
        string str = this.updateLoanXDBTables(info.LastModified, (LoanData) null);
        current.AddCheckpoint("LoanApplicationSubmit: (LoanApplicationSubmit) Updated Reporting Database tables.", 2488, nameof (LoanApplicationSubmit), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        this.updateAuditTrail(currentUser, (LoanData) null, DateTime.MinValue, (string) null);
        current.AddCheckpoint("LoanApplicationSubmit: Added Audit Trail records ...", 2490, nameof (LoanApplicationSubmit), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        this.innerLock.UndoCheckout();
        current.AddCheckpoint("LoanApplicationSubmit: Checked loan into LoanStore ...", 2492, nameof (LoanApplicationSubmit), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        if (str != null)
          TraceLog.WriteError(nameof (Loan), string.Format("(LoanApplicationSubmit)  Error updating reporting database during draft loan submission. Time: {0}, Instance ID: {1}, LoanId: {2}, User ID: {3}  ErrorMessage: {4}", (object) DateTime.Now.ToString("G"), (object) ClientContext.GetCurrent().ClientID, (object) this.id.Guid, (object) currentUser.Userid, (object) str));
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("(LoanApplicationSubmit)  Error updating reporting database or audit trail during draft loan submission. Time: {0}, Instance ID: {1}, LoanIds: {2}, User ID: {3} Message: {4} Source: {5},  StackTrace: {6} ", (object) DateTime.Now.ToString("G"), (object) ClientContext.GetCurrent().ClientID, (object) this.id.Guid, (object) currentUser.Userid, (object) ex.Message, (object) ex.Source, (object) ex.StackTrace));
      }
      data.DefrdLoanAppSubmitEventRequired = true;
      try
      {
        string fullLoanFilePath = new LoanFolder(this.id.LoanFolder).GetFullLoanFilePath(this.id.LoanName);
        bool batchApplied = true;
        this.publishLoanEvent(currentUser.Userid, LoanEventType.Create, Guid.Parse(this.id.Guid), sessionId, fullLoanFilePath, this.loanData == null ? 0 : this.loanData.LoanVersionNumber, this.id.LoanFolder, false, this.LastModifiedUTC, batchApplied: batchApplied);
        current.AddCheckpoint("CheckIn: Published Loan Event...", 2515, nameof (LoanApplicationSubmit), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        TraceLog.WriteInfo(nameof (Loan), string.Format("(LoanApplicationSubmit)  Published event to Data Lake for applicationId = {0} ", (object) this.id.Guid));
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("(LoanApplicationSubmit)  Unknow error while publishing event to Data Lake for applicationId = {0} Error Message = {1} Stack Trace = {2}", (object) this.id.Guid, (object) ex.Message, (object) ex.StackTrace));
      }
      string str1 = "BE11176273,BE11176272,BE11105680i,BE11105680c,BE11209541,BE11063388c,BE11176282,DEBE11170913";
      if (string.IsNullOrWhiteSpace(ClientContext.GetCurrent().InstanceName) || str1.IndexOf(ClientContext.GetCurrent().InstanceName, StringComparison.OrdinalIgnoreCase) < 0)
        return;
      TraceLog.WriteWarning(nameof (Loan), string.Format("LoanApplicationSubmit - Loan Created date: {0}", (object) this.loanData.GetField("2025")));
    }

    public void ImportNew(
      string folderName,
      string loanName,
      LoanServerInfo info,
      BinaryObject importData,
      UserInfo currentUser,
      string sessionId)
    {
      LoanData data = new LoanDataFormatter().Deserialize(importData, folderName);
      this.CreateNew(folderName, loanName, info, data, currentUser, sessionId, true);
    }

    public void Move(string folderName, string loanName, UserInfo currentUser)
    {
      this.validateInstance();
      if ((folderName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (Loan), new ServerException("Invalid folder name in Move method"));
      if ((loanName ?? "") == "")
        loanName = this.id.LoanName;
      LoanFolder sourceLoanFolder = new LoanFolder(this.id.LoanFolder);
      LoanFolder targetLoanFolder = new LoanFolder(folderName);
      LoanIdentity newId = new LoanIdentity(folderName, loanName, this.id.Guid, this.id.XrefId);
      LoanIdentity objectId = Loan.LookupIdentity(folderName, loanName);
      if (objectId != (LoanIdentity) null)
        Err.Raise(TraceLevel.Warning, nameof (Loan), (ServerException) new DuplicateObjectException("Cannot move loan on top of existing loan", ObjectType.Loan, (object) objectId));
      LoanIdentity id = this.id;
      if (!Loan.updateLoanLocationTable(newId.Guid, this.id.LoanFolder, newId.LoanFolder))
      {
        Err.Raise(TraceLevel.Warning, nameof (Loan), (ServerException) new DuplicateObjectException("Failed to update move location record.", ObjectType.Loan, (object) objectId));
      }
      else
      {
        this.moveLoanFiles(newId, currentUser);
        this.id = newId;
        try
        {
          Loan.updateLoanIdentity(newId);
        }
        catch
        {
          this.moveLoanFiles(id, currentUser);
          this.id = id;
          Err.Raise(TraceLevel.Warning, nameof (Loan), (ServerException) new DuplicateObjectException("Loan has been moved back due to failed Loan Identity update", ObjectType.Loan, (object) id));
        }
        this.insertAuditTrailForLoanFolder(currentUser, sourceLoanFolder, targetLoanFolder);
        string str = this.updateLoanXDBTables(this.LastModified, this.LoanData, true);
        string fullLoanFilePath = new LoanFolder(this.id.LoanFolder).GetFullLoanFilePath(this.id.LoanName);
        bool isSourceEncompass = EncompassServer.ServerMode != EncompassServerMode.Service;
        this.publishLoanEvent(currentUser.Userid, LoanEventType.Move, Guid.Parse(this.id.Guid), "", fullLoanFilePath, this.loanData == null ? 0 : this.loanData.LoanVersionNumber, this.id.LoanFolder, isSourceEncompass, this.LastModifiedUTC);
        this.innerLock.UndoCheckout();
        if (sourceLoanFolder.FolderType != targetLoanFolder.FolderType)
          this.updateAlertsBasedOnLoanFolderType(currentUser);
        if (str == null)
          return;
        Err.Raise(nameof (Loan), new ServerException("Error updating reporting database:\r\n" + str));
      }
    }

    private void loadServerInfo()
    {
      if (this.serverInfo != null)
        return;
      this.serverInfo = Loan.getServerInfoFromDatabase(this.id);
    }

    private void loadDraftServerInfo()
    {
      if (this.serverDraftInfo != null)
        return;
      this.serverDraftInfo = Loan.getServerDraftInfoFromDatabase(this.draftId);
    }

    private LoanServerInfo info
    {
      get
      {
        this.loadServerInfo();
        return this.serverInfo;
      }
      set => this.serverInfo = value;
    }

    private DraftLoanServerInfo draftInfo
    {
      get
      {
        this.loadDraftServerInfo();
        return this.serverDraftInfo;
      }
      set => this.serverDraftInfo = value;
    }

    private void insertAuditTrailForLoanFolder(
      UserInfo currentUser,
      LoanFolder sourceLoanFolder,
      LoanFolder targetLoanFolder)
    {
      LoanXDBTableList loanXdbTableList = LoanXDBStore.GetLoanXDBTableList();
      if (loanXdbTableList == null)
        return;
      foreach (LoanXDBField allField in loanXdbTableList.GetAllFields())
      {
        if (allField.Auditable && allField.FieldID == "LOANFOLDER")
        {
          AuditTrailAccessor.InsertAuditRecord(this.id.XrefId, allField.FieldXRefID, currentUser, (object) sourceLoanFolder.Name, (object) targetLoanFolder.Name, DateTime.Now);
          break;
        }
      }
    }

    private void updateAlertsBasedOnLoanFolderType(UserInfo currentUser)
    {
      PipelineInfo pipelineInfo = this.LoanData.ToPipelineInfo((IAlertMonitor) Loan.getLoanAlertMonitor(this.LoanData));
      PipelineInfo.Alert[] existingLoanAlerts = Loan.GetExistingLoanAlerts(this.id.Guid);
      DbQueryBuilder sql = new DbQueryBuilder();
      sql.DeleteFrom(DbAccessManager.GetTable("LoanAlerts"), new DbValue("LoanXRefId", (object) this.id.XrefId));
      Loan._constructAlertUpdateQuery(sql, currentUser, this.id, pipelineInfo, true, existingLoanAlerts);
      sql.ExecuteNonQuery();
      foreach (AlertChange loanAlertChange in Loan.GetLoanAlertChanges(this.id, pipelineInfo, existingLoanAlerts, currentUser.userName))
        Loan.PublishLoanAlertChangeKafkaEvent(this.id.Guid, currentUser.Userid, DateTime.UtcNow, new List<AlertChange>()
        {
          loanAlertChange
        });
    }

    public void Delete(UserInfo currentUser, bool isExternalOrganization)
    {
      this.Delete(false, currentUser, isExternalOrganization);
    }

    public void Delete(bool preserveFiles, UserInfo currentUser, bool isExternalOrganization)
    {
      this.validateInstance();
      ClientContext current = ClientContext.GetCurrent();
      bool isSourceEncompass = EncompassServer.ServerMode != EncompassServerMode.Service;
      bool serverSetting = (bool) current.Settings.GetServerSettings("Policies")[(object) "Policies.WebHookEnabled"];
      string eventSequenceNumber = Loan.GetEventSequenceNumber();
      string loanId = this.id.Guid.ToString();
      if (this.Locked)
        Err.Raise(TraceLevel.Warning, nameof (Loan), new ServerException("Cannot delete a loan while it is locked"));
      if (!preserveFiles && currentUser == (UserInfo) null)
        Err.Raise(TraceLevel.Error, nameof (Loan), (ServerException) new SecurityException("A value User is required to permanently delete a loan"));
      this.publishLoanDeleteKafkaEvent(this.id.Guid.ToString(), LoanEventType.Delete.ToString(), eventSequenceNumber, currentUser.Userid.ToString(), isSourceEncompass);
      if (loanId != null && loanId.StartsWith("{") && loanId.EndsWith("}"))
        loanId = loanId.TrimStart('{').TrimEnd('}');
      this.PublishLoanEventKafka(loanId, currentUser.Userid.ToString(), isSourceEncompass, serverSetting, LoanEventType.Delete, this.LastModifiedUTC, this.loanData == null ? 0 : this.loanData.LoanVersionNumber, (string) null);
      if (!preserveFiles)
        LoanTrades.DeleteLoanFromTrades(this.id.Guid, currentUser, isExternalOrganization);
      this.destroyLoan(this.id, !preserveFiles, true, currentUser);
      if (this.innerLock != null)
        this.innerLock.UndoCheckout();
      this.Dispose();
    }

    public void ChangeIdentity(LoanIdentity loanId)
    {
      this.validateExists();
      if (this.id.Guid != loanId.Guid)
        throw new Exception("New identity must have same GUID as prior identity");
      this.id = loanId;
    }

    public int CheckIn(
      UserInfo currentUser,
      bool isExternalOrganization,
      string sessionId,
      bool allowDeferrable = false,
      string auditUserId = null,
      string lockId = null)
    {
      return this.CheckIn(false, currentUser, isExternalOrganization, sessionId, allowDeferrable, auditUserId, lockId);
    }

    public int CheckIn(
      bool keepCheckedOut,
      UserInfo currentUser,
      bool isExternalOrganization,
      string sessionId,
      bool allowDeferrable = false,
      string auditUserId = null,
      string lockId = null)
    {
      this.validateInstance();
      return this.CheckIn((LoanData) null, keepCheckedOut, currentUser, isExternalOrganization, sessionId, allowDeferrable, auditUserId, lockId);
    }

    public int CheckIn(
      LoanData newData,
      UserInfo currentUser,
      bool isExternalOrganization,
      string sessionId,
      bool allowDeferrable = false,
      string auditUserId = null,
      string lockId = null)
    {
      return this.CheckIn(newData, currentUser, true, isExternalOrganization, sessionId, allowDeferrable, auditUserId, lockId);
    }

    public int CheckIn(
      LoanData newData,
      UserInfo currentUser,
      bool wakeUpERDBJobProcessor,
      bool isExternalOrganization,
      string sessionId,
      bool allowDeferrable = false,
      string auditUserId = null,
      string lockId = null)
    {
      return this.CheckIn(newData, false, currentUser, wakeUpERDBJobProcessor, isExternalOrganization, sessionId, allowDeferrable, auditUserId, lockId);
    }

    public int CheckIn(
      LoanData newData,
      bool keepCheckedOut,
      UserInfo currentUser,
      bool isExternalOrganization,
      string sessionId,
      bool allowDeferrable = false,
      string auditUserId = null,
      string lockId = null)
    {
      return this.CheckIn(newData, keepCheckedOut, currentUser, true, isExternalOrganization, sessionId, allowDeferrable, auditUserId, lockId);
    }

    public int CheckIn(
      LoanData newData,
      bool keepCheckedOut,
      UserInfo currentUser,
      bool wakeUpERDBJobProcessor,
      bool isExternalOrganization,
      string sessionId,
      bool allowDeferrable = false,
      string auditUserId = null,
      string lockId = null)
    {
      return this.CheckIn(newData, keepCheckedOut, currentUser, true, wakeUpERDBJobProcessor, isExternalOrganization, sessionId, allowDeferrable, auditUserId, lockId);
    }

    public void CheckInDraft(UserInfo currentUser, bool isExternalOrganization, string sessionId)
    {
      this.CheckInDraft(false, currentUser, isExternalOrganization, sessionId);
    }

    public void CheckInDraft(
      bool keepCheckedOut,
      UserInfo currentUser,
      bool isExternalOrganization,
      string sessionId)
    {
      this.validateInstance();
      this.CheckInDraft((LoanData) null, keepCheckedOut, currentUser, isExternalOrganization, sessionId);
    }

    public void CheckInDraft(
      LoanData newData,
      UserInfo currentUser,
      bool isExternalOrganization,
      string sessionId)
    {
      this.CheckInDraft(newData, currentUser, true, isExternalOrganization, sessionId);
    }

    public void CheckInDraft(
      LoanData newData,
      UserInfo currentUser,
      bool wakeUpERDBJobProcessor,
      bool isExternalOrganization,
      string sessionId)
    {
      this.CheckInDraft(newData, false, currentUser, wakeUpERDBJobProcessor, isExternalOrganization, sessionId);
    }

    public void CheckInDraft(
      LoanData newData,
      bool keepCheckedOut,
      UserInfo currentUser,
      bool isExternalOrganization,
      string sessionId)
    {
      this.CheckInDraft(newData, keepCheckedOut, currentUser, true, isExternalOrganization, sessionId);
    }

    public void CheckInDraft(
      LoanData newData,
      bool keepCheckedOut,
      UserInfo currentUser,
      bool wakeUpERDBJobProcessor,
      bool isExternalOrganization,
      string sessionId)
    {
      this.CheckInDraft(newData, keepCheckedOut, currentUser, true, wakeUpERDBJobProcessor, isExternalOrganization, sessionId);
    }

    public void CheckInDraft(
      LoanData newData,
      bool keepCheckedOut,
      UserInfo currentUser,
      bool updateERDB,
      bool wakeUpERDBJobProcessor,
      bool isExternalOrganization,
      string sessionId)
    {
      PerformanceMeter current = PerformanceMeter.Current;
      this.validateInstance();
      if (newData != null)
        this.ImportDraft(newData, false);
      if (this.draftInfo.IsLockModified)
      {
        Loan.writeDatabaseLock(this.id, this.info);
        this.info.IsLockModified = false;
        current.AddCheckpoint("CheckIn: Wrote database lock info to database...", 2842, nameof (CheckInDraft), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
      }
      if (this.rightsModified)
      {
        Loan.updateDraftDatabaseRights(this.draftId, this.draftInfo.UserRightPairs);
        current.AddCheckpoint("CheckIn: Updated loan rights in database...", 2849, nameof (CheckInDraft), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
      }
      string str = (string) null;
      if (this.saveLoanData)
      {
        current.AddCheckpoint("CheckIn: Acquired Reporting Database read lock...", 2857, nameof (CheckInDraft), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        ILoanSettings loanSettings = LoanConfiguration.GetLoanSettings();
        this.deserializeDraftApplicationLoanData(this.draftId, loanSettings);
        current.AddCheckpoint("CheckIn: Deserialized prior LoanData object...", 2862, nameof (CheckInDraft), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        DateTime lastModified = this.draftInfo.LastModified;
        if (!this.forceLastModified)
          this.draftInfo.LastModified = DateTime.Now;
        Loan.updateDraftApplicationLoanSummary(this.draftId, this.loanData, currentUser, this.draftInfo.LastModified);
        this.draftId = Loan.LookupDraftIdentity(this.draftId.Guid);
        current.AddCheckpoint("CheckIn: Updated ApplicationSummary table...", 2869, nameof (CheckInDraft), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        Loan.serializeDraftLoanData(this.draftId, this.loanData, true, currentUser, sessionId, loanSettings);
        current.AddCheckpoint("CheckIn: Serialized loan file to disk ...", 2914, nameof (CheckInDraft), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        new DraftLoanFolder(this.draftId.LoanFolder).GetFullLoanFilePath(this.draftId.LoanName);
        this.saveLoanData = false;
        this.forceRebuild = false;
      }
      this.rightsModified = false;
      if (!keepCheckedOut)
        this.innerLock.UndoCheckout();
      current.AddCheckpoint("CheckIn: Checked loan into LoanStore ...", 2941, nameof (CheckInDraft), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
      if (!keepCheckedOut)
        this.Dispose();
      if (str == null)
        return;
      Err.Raise(nameof (Loan), new ServerException("Error updating reporting database:\r\n" + str));
    }

    public int CheckIn(
      LoanData newData,
      bool keepCheckedOut,
      UserInfo currentUser,
      bool updateERDB,
      bool wakeUpERDBJobProcessor,
      bool isExternalOrganization,
      string sessionId,
      bool allowDeferrable = false,
      string auditUserId = null,
      string lockId = null)
    {
      PerformanceMeter meter = PerformanceMeter.Current;
      this.validateInstance();
      if (string.IsNullOrWhiteSpace(lockId))
        lockId = sessionId;
      int num = -1;
      if (newData != null)
        this.Import(newData, false);
      if (this.info.IsLockModified)
      {
        Loan.writeDatabaseLock(this.id, this.info);
        this.info.IsLockModified = false;
        meter.AddCheckpoint("CheckIn: Wrote database lock info to database...", 2972, nameof (CheckIn), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
      }
      if (this.rightsModified)
      {
        Loan.updateDatabaseRights(this.id, this.info.UserRightPairs);
        meter.AddCheckpoint("CheckIn: Updated loan rights in database...", 2979, nameof (CheckIn), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
      }
      bool batchApplied = false;
      string str = (string) null;
      if (this.saveLoanData)
      {
        LoanData priorData = (LoanData) null;
        meter.AddCheckpoint("CheckIn: Acquired Reporting Database read lock...", 2988, nameof (CheckIn), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        ILoanSettings settings = LoanConfiguration.GetLoanSettings();
        if (!this.forceRebuild)
        {
          priorData = this.deserializeLoanData(this.id, -999, settings, true);
          meter.AddCheckpoint("CheckIn: Deserialized prior LoanData object...", 2999, nameof (CheckIn), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        }
        LoanData loanData = priorData == null ? this.deserializeLoanData(this.id, -999, settings) : priorData;
        if (LoanLockAccessor.IsLoanLockDbEnabled && !LoanLockAccessor.IsLoanLockedBySessionId(this.id.Guid, lockId))
        {
          if (!Loan._skipLoanLockValidationAtCheckin)
            Err.Raise("Loan.CheckIn", (ServerException) new LockException("Loan is not currently locked for the session"));
          TraceLog.WriteWarning(nameof (Loan), "Skipping this error, Loan '" + this.id.Guid + "' is not currently locked for the session. Stack Trace: " + Tracing.GetStackTrace());
        }
        if (loanData != null && newData != null && newData.BatchAppliedSinceLastSave)
        {
          batchApplied = true;
          if (newData != null && newData.GetBatchUpdateSequenceNum() != loanData.GetBatchUpdateSequenceNum())
          {
            TraceLog.WriteInfo(nameof (Loan), "Loan was modified due to batch update since last load. Merging the data.");
            LoanBatchUpdateAccessor.ApplyBatchUpdatesToLoan(newData);
          }
        }
        if (this.loanData == null || loanData == null || this.loanData.LoanVersionNumber != loanData.LoanVersionNumber)
        {
          TraceLog.WriteError(nameof (Loan), string.Format("Wrong Loan File Version. Time:{0}, Instance ID:{1}, Loan GUID:{2}, User ID:{3}, Client Sequence Number:{4}, Server Sequence Number:{5}, at {6}.CheckIn()", (object) DateTime.Now.ToString("G"), (object) ClientContext.GetCurrent().ClientID, (object) loanData.GUID, (object) currentUser.Userid, (object) this.loanData.LoanVersionNumber, (object) loanData.LoanVersionNumber, (object) nameof (Loan)));
          return num;
        }
        num = this.loanData.LoanVersionNumber = loanData.LoanVersionNumber + 1;
        DateTime lastModifedDTTM = this.info.LastModified;
        if (!this.forceLastModified)
          this.info.LastModified = DateTime.Now;
        Thread thread = (Thread) null;
        ClientContext currentContext = ClientContext.GetCurrent();
        bool insertLoanSummary = this.deleteInsertLoanSummary;
        TraceLog.WriteDebug("Loan-Save Checkin()", "SaveDeleteInsertLoanSummary is :" + insertLoanSummary.ToString());
        if (Loan.IsAsyncSaveEnabled())
        {
          thread = new Thread((ThreadStart) (() => this.updateLoanSummaryAsync(this.id, this.loanData, currentUser, this.info.LastModified, lastModifedDTTM, currentContext, allowDeferrable, this.forceRebuild ? (LoanData) null : priorData, this.deleteInsertLoanSummary)));
          thread.Start();
        }
        else
          Loan.updateLoanSummary(this.id, this.loanData, currentUser, this.info.LastModified, lastModifedDTTM, this.deleteInsertLoanSummary);
        if (allowDeferrable)
        {
          using (DeferrableLoanTransactionContext transactionContext = new DeferrableLoanTransactionContext(DeferrableProcessorRole.Publisher, currentUser, this, settings, auditUserId))
          {
            transactionContext.SetUrn("elli", "encompass", ClientContext.GetCurrent().InstanceName, "", "CheckInLoan", currentUser.Userid).SetPriorLoanData(priorData).SetCurrentLoanData(this.loanData).SetXDBModifiedTime(this.info.LastModified).SetAuditModifiedTime(lastModifedDTTM).SetAuditCurrentTime(DateTime.Now).SetUpdateForceRebuild(this.forceRebuild);
            using (DeferrableLoanTransaction deferrableLoanTransaction = new DeferrableLoanTransaction((IDeferrableTransactionContext) transactionContext))
            {
              thread?.Join();
              this.id = Loan.LookupIdentity(this.id.Guid);
              PerformanceMeter performanceMeter = meter;
              insertLoanSummary = this.deleteInsertLoanSummary;
              string description = "CheckIn: Updated LoanSummary table...Delete/Insert = " + insertLoanSummary.ToString();
              performanceMeter.AddCheckpoint(description, 3097, nameof (CheckIn), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
              deferrableLoanTransaction.InjectProcessorAfter(new string[2]
              {
                new RealTimeAuditTrailProcessor((DeferrableLoanTransaction) null).GetKey(),
                new DeferredAuditTrailProcessor((DeferrableLoanTransaction) null).GetKey()
              }, (IDeferrableProcessor) new DeferrableDynamicProcessor("serializeLoanData")
              {
                ProcessorHandler = (Action) (() =>
                {
                  Loan.serializeLoanData(this.id, this.loanData, true, currentUser, sessionId, settings);
                  meter.AddCheckpoint("CheckIn: Serialized loan file to disk ...", 3113, nameof (CheckIn), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
                })
              });
              deferrableLoanTransaction.Initialize(meter);
              deferrableLoanTransaction.CurrentContext.DataBag.Set("PriorLoanFileName", (object) string.Format("{0:D5}_{1}", (object) (num - 1), (object) "loan.em"));
              deferrableLoanTransaction.CurrentContext.DataBag.Set("AfterLoanFileName", (object) string.Format("{0:D5}_{1}", (object) num, (object) "loan.em"));
              deferrableLoanTransaction.Complete(new DeferrableType?());
              str = deferrableLoanTransaction.GetError(new RealTimeReportingDbProcessor((DeferrableLoanTransaction) null).GetKey());
            }
          }
        }
        else
        {
          bool flag1 = Company.GetCompanySetting("FEATURE", "UpdateXDBBasedOnFieldChanges").ToLower() == "true";
          bool flag2 = EncompassServer.ServerMode != EncompassServerMode.Service;
          bool changeTrackingForXdb = this.loanData.IgnoreFieldChangeTrackingForXDB;
          bool flag3 = flag1 & flag2 && !changeTrackingForXdb && !this.forceRebuild;
          ILogger logger = DiagUtility.LogManager.GetLogger("LoanXDBUpdate");
          using (ClientContext.GetCurrent().MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
            logger.Write(Encompass.Diagnostics.Logging.LogLevel.INFO, "UpdateLoanXDBTablesUsingFieldChangeTracker", string.Format("UpdateXDBBasedOnFieldChanges: {0} --> DBFlagForUpdateXDBBasedOnFieldChanges:{1}, EncompassServer:{2}, IgnoreFieldChangeTracking:{3}, forceRebuild:{4}", (object) flag3, (object) flag1, (object) flag2, (object) changeTrackingForXdb, (object) this.forceRebuild));
          Stopwatch stopwatch = Stopwatch.StartNew();
          if (flag3)
          {
            str = this.updateLoanXBTablesBasedOnFieldChanges(this.info.LastModified);
            stopwatch.Stop();
            logger.Write(Encompass.Diagnostics.Logging.LogLevel.INFO, "LoanXDB", string.Format("XDB based on field change tracker: Total execution time: {0} ms", (object) stopwatch.ElapsedMilliseconds));
          }
          else
          {
            str = this.updateLoanXDBTables(this.info.LastModified, this.forceRebuild ? (LoanData) null : priorData);
            stopwatch.Stop();
            logger.Write(Encompass.Diagnostics.Logging.LogLevel.INFO, "LoanXDB", string.Format("XDB based on loan comparision: Total execution time: {0} ms", (object) stopwatch.ElapsedMilliseconds));
          }
          meter.AddCheckpoint("CheckIn: Updated Repoting Database tables...", 3153, nameof (CheckIn), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
          if (currentUser != (UserInfo) null && !this.forceRebuild)
          {
            this.updateAuditTrail(currentUser, priorData, lastModifedDTTM, auditUserId);
            meter.AddCheckpoint("CheckIn: Added audit trail records to database ...", 3158, nameof (CheckIn), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
          }
          thread?.Join();
          this.id = Loan.LookupIdentity(this.id.Guid);
          Loan.serializeLoanData(this.id, this.loanData, true, currentUser, sessionId, settings);
          meter.AddCheckpoint("CheckIn: Serialized loan file to disk ...", 3168, nameof (CheckIn), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        }
        IStageLoanHistoryManager stageHistoryManager = this.loanData.StageHistoryManager;
        LoanHistoryEntry[] array = stageHistoryManager != null ? stageHistoryManager.HistoryEntries.OfType<LoanHistoryEntry>().ToArray<LoanHistoryEntry>() : (LoanHistoryEntry[]) null;
        if (array != null && ((IEnumerable<LoanHistoryEntry>) array).Any<LoanHistoryEntry>())
        {
          this.AppendLoanHistory(array);
          this.loanData.StageHistoryManager.ClearHistoryEntrie();
        }
        LoanTrades.RecalculateProfitFromLoan(this.loanData.GUID, isExternalOrganization);
        meter.AddCheckpoint("CheckIn: Re-calculated trade profits from loan ...", 3182, nameof (CheckIn), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        this.saveLoanData = false;
        this.forceRebuild = false;
        string fullLoanFilePath = new LoanFolder(this.id.LoanFolder).GetFullLoanFilePath(this.id.LoanName);
        this.publishLoanEvent(currentUser.Userid, LoanEventType.Update, Guid.Parse(this.id.Guid), sessionId, fullLoanFilePath, this.loanData == null ? 0 : this.loanData.LoanVersionNumber, this.id.LoanFolder, !allowDeferrable, this.LastModifiedUTC, batchApplied: batchApplied);
        meter.AddCheckpoint("CheckIn: Published Loan Event...", 3192, nameof (CheckIn), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
        this.InitiateLoanBillingProcess(priorData, this.loanData, currentUser, LoanEventType.Update);
        this.EvaluateDocumentReceivedAndPublishEvent(priorData, this.loanData, currentUser.Userid);
      }
      this.rightsModified = false;
      if (!keepCheckedOut)
        this.innerLock.UndoCheckout();
      meter.AddCheckpoint("CheckIn: Checked loan into LoanStore ...", 3205, nameof (CheckIn), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\Loan.cs");
      if (!keepCheckedOut)
        this.Dispose();
      if (str != null)
        TraceLog.WriteError(nameof (Loan), string.Format("[ReportingDb]:update failed for loanId-{0},xrefId-{1},serverLoanVersion-{2},Exception-{3}", (object) this.id.Guid, (object) this.id.XrefId, (object) num, (object) str));
      return num;
    }

    private bool deleteInsertLoanSummary
    {
      get
      {
        bool o = true;
        try
        {
          string name = "LoanSaveDeleteInsertLoanSummary";
          ClientContext current = ClientContext.GetCurrent();
          object obj = current.Cache.Get(name);
          if (obj != null)
          {
            o = (bool) obj;
          }
          else
          {
            o = (bool) current.Settings.GetServerSetting("Internal." + name);
            current.Cache.Put(name, (object) o);
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(Loan.sw, nameof (Loan), TraceLevel.Warning, "Error getting the LoanSaveDeleteInsertLoanSummary setting: " + ex.Message);
        }
        return o;
      }
    }

    public void UndoCheckout()
    {
      if (this.innerLock == null)
        return;
      this.innerLock.UndoCheckout();
      this.Dispose();
    }

    public void Dispose()
    {
      this.disposed = true;
      if (this.innerLock == null)
        return;
      this.innerLock.Dispose();
      if (this.innerLock != null)
        this.innerLock = (ICacheLock<bool?>) null;
      if (this.loanData == null)
        return;
      this.loanData = (LoanData) null;
    }

    private void validateInstance() => this.validateInstance(true);

    private void validateInstance(bool requireExists)
    {
      if (this.disposed)
        Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("Attempt to access disposed loan object"));
      if (!requireExists)
        return;
      this.validateExists();
    }

    private void validateExists()
    {
      if (this.Exists)
        return;
      Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("Object does not exist"));
    }

    private string getSupportingDataFilename(string key)
    {
      return this.getSupportingDataFilename(this.id, key);
    }

    private string getSupportingDataFilename(LoanIdentity loanIdentity, string key)
    {
      if (!DataFile.IsValidFilename(key))
        Err.Raise(TraceLevel.Warning, nameof (Loan), new ServerException("Invalid supporting data key"));
      IServerSettings settings = ClientContext.GetCurrent().Settings;
      if (key.ToLower() == "loan.em".ToLower())
        Err.Raise(TraceLevel.Warning, nameof (Loan), new ServerException("Invalid supporting data key"));
      return settings.GetLoansFilePath(loanIdentity.LoanFolder + "\\" + loanIdentity.LoanName + "\\" + key);
    }

    private string getSupportingLinkedDataFilename(string loanFolder, string loanName, string key)
    {
      if (!DataFile.IsValidFilename(key))
        Err.Raise(TraceLevel.Warning, nameof (Loan), new ServerException("Invalid supporting data key"));
      IServerSettings settings = ClientContext.GetCurrent().Settings;
      if (key.ToLower() == "loan.em".ToLower())
        Err.Raise(TraceLevel.Warning, nameof (Loan), new ServerException("Invalid supporting data key"));
      return settings.GetLoansFilePath(loanFolder + "\\" + loanName + "\\" + key);
    }

    private static void validateRightsInfo(string userId, LoanInfo.Right right)
    {
      if ((userId ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (Loan), new ServerException("Invalid user ID for rights operation"));
      if (Enum.IsDefined(typeof (LoanInfo.Right), (object) right))
        return;
      Err.Raise(TraceLevel.Warning, nameof (Loan), new ServerException("Invalid value for rights operation"));
    }

    private static void updateLastModifiedDate(LoanIdentity id, DateTime lastModified)
    {
      DbValueList values = new DbValueList();
      values.Add("LastModified", (object) lastModified);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Update(DbAccessManager.GetTable("LoanSummary"), values, new DbValue("Guid", (object) id.Guid));
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static int getLoanXrefID(string guid)
    {
      DbValue key = new DbValue("Guid", (object) guid);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("LoanSummary"), new string[1]
      {
        "XrefId"
      }, key);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count == 0 ? -1 : (int) dataRowCollection[0]["XrefId"];
    }

    public void RefreshReportingData(
      bool useERDB,
      LoanXDBField[] fields,
      CustomFieldCalculators calcs,
      CustomCodeContextDataProvider serviceContext,
      UserInfo currentUser)
    {
      if (fields.Length == 0)
        return;
      Dictionary<string, List<LoanXDBField>> dictionary = new Dictionary<string, List<LoanXDBField>>();
      List<string> stringList = new List<string>();
      foreach (LoanXDBField field in fields)
      {
        if (!dictionary.ContainsKey(field.TableName))
          dictionary[field.TableName] = new List<LoanXDBField>();
        dictionary[field.TableName].Add(field);
        if (CustomFieldInfo.IsCustomFieldID(field.FieldID))
          stringList.Add(field.FieldID);
      }
      if (calcs != null && stringList.Count > 0)
      {
        using (CustomCalculationContext context = new CustomCalculationContext(currentUser, this.LoanData, (IServerDataProvider) serviceContext))
          calcs.InvokeCalculations(context, stringList.ToArray());
      }
      foreach (string key in dictionary.Keys)
      {
        List<LoanXDBField> tableFields = dictionary[key];
        string[] fieldIds = new string[tableFields.Count];
        for (int index = 0; index < tableFields.Count; ++index)
          fieldIds[index] = tableFields[index].FieldIDWithCoMortgagor;
        string[] fieldValues = this.SelectFields(fieldIds);
        try
        {
          ERDBSession.RefreshReportingData(useERDB, ClientContext.GetCurrent(), this.Identity.XrefId, key, tableFields, fieldValues);
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (Loan), "Error updating Reporting DB for loan " + (object) this.id + ": " + (object) ex);
        }
      }
    }

    public string InternalUpdateLoanXDBTables(
      DateTime lastModified,
      LoanData priorLoanData,
      LoanData currentLoanData)
    {
      bool flag = Company.GetCompanySetting("FEATURE", "UpdateXDBBasedOnFieldChanges").ToLower() == "true" && EncompassServer.ServerMode != EncompassServerMode.Service && !this.forceRebuild;
      string empty = string.Empty;
      string str = !flag || this.loanData.IgnoreFieldChangeTrackingForXDB || priorLoanData == null ? this.updateLoanXDBTables(lastModified, priorLoanData, false, currentLoanData) + this.updateEFolderReportingTables(currentLoanData) : this.updateLoanXBTablesBasedOnFieldChanges(this.info.LastModified);
      return !string.IsNullOrWhiteSpace(str) ? str : (string) null;
    }

    public string InternalUpdateLoanXDBTablesForEBS(DateTime lastModified)
    {
      if (this.loanData.IgnoreFieldChangeTrackingForXDB)
        throw new Exception("To use realtime RDB updates in EBS IgnoreFieldChangeTrackingForXDB cannot be set to true");
      using (this.metricRecorder.StartTimer("syncRdbUpdate"))
        return this.updateLoanXBTablesBasedOnFieldChanges(lastModified);
    }

    public string InternalUpdateDraftLoanXDBTables(
      DateTime lastModified,
      LoanData priorLoanData,
      LoanData currentLoanData)
    {
      return this.updateDraftLoanXDBTables(lastModified, priorLoanData, false, currentLoanData);
    }

    private string updateLoanXDBTables(DateTime lastModified, LoanData priorData)
    {
      string str = this.updateLoanXDBTables(lastModified, priorData, false) + this.updateEFolderReportingTables();
      return !string.IsNullOrWhiteSpace(str) ? str : (string) null;
    }

    private string updateLoanXBTablesBasedOnFieldChanges(DateTime lastModified)
    {
      string errorMessage = (string) null;
      int num = 2;
      while (num-- > 0)
      {
        LoanXDBTableList loanXdbTableList = LoanXDBStore.GetLoanXDBTableList();
        try
        {
          LoanUtil.UpdateLoanXDBTablesUsingFieldChangeTracker((IClientContext) ClientContext.GetCurrent(), this.metricRecorder, this.id, lastModified, this.loanData, loanXdbTableList, new EllieMae.EMLite.ReportingDbUtils.SelectFields(this.SelectFields), out errorMessage);
        }
        catch (Exception ex)
        {
          if (!ex.Message.Contains("Invalid column name") || num <= 0)
            throw;
          else
            errorMessage = ex.Message;
        }
        if (errorMessage != null && errorMessage.Contains("Invalid column name"))
          ClientContext.GetCurrent()?.Cache?.Remove("LoanXDBStore" + (ClientContext.GetCurrent().UseERDB ? "|ERDB" : ""));
        else
          break;
      }
      errorMessage += this.updateEFolderReportingTables();
      return !string.IsNullOrWhiteSpace(errorMessage) ? errorMessage : (string) null;
    }

    private string updateDraftLoanXDBTables(DateTime lastModified, LoanData priorData)
    {
      return this.updateDraftLoanXDBTables(lastModified, priorData, false);
    }

    private string updateDraftLoanXDBTables(
      DateTime lastModified,
      LoanData priorData,
      bool loanFolderChanged,
      LoanData currentData = null)
    {
      string errorMessage = (string) null;
      LoanXDBTableList loanXdbTableList = LoanXDBStore.GetLoanXDBTableList();
      if (currentData == null)
        currentData = this.loanData;
      LoanUtil.UpdateDraftLoanXDBTables(ClientContext.GetCurrent().Settings.ConnectionString, ClientContext.GetCurrent().Settings.DbServerType, this.draftId, lastModified, currentData, priorData, loanXdbTableList, new EllieMae.EMLite.ReportingDbUtils.DraftSelectFields(this.DraftSelectFields), loanFolderChanged, out errorMessage);
      return errorMessage;
    }

    private string updateLoanXDBTables(
      DateTime lastModified,
      LoanData priorData,
      bool loanFolderChanged,
      LoanData currentData = null)
    {
      int num = 2;
      LoanUtil.IsCcLoggingEnabled = false;
      if (HttpContext.Current?.Request != null)
      {
        LoanUtil.IsCcLoggingEnabled = string.Equals(HttpContext.Current.Request.Headers["X-Message-Priority"], "CC-High", StringComparison.OrdinalIgnoreCase);
        LoanUtil.xdbLogs = this._xdbLogs;
      }
      bool isMarshallEnabled = string.Equals(Company.GetCompanySetting("LICENSE", "AIQINTEGRATIONMODEL"), "Marshall", StringComparison.InvariantCultureIgnoreCase);
      string errorMessage = (string) null;
      while (num-- > 0)
      {
        try
        {
          if (currentData == null)
            currentData = this.loanData;
          if (currentData.SnapshotProvider == null)
            currentData.AttachSnapshotProvider((ILoanSnapshotProvider) new LoanSnapshotProvider(this));
          errorMessage = (string) null;
          LoanXDBTableList loanXdbTableList = LoanXDBStore.GetLoanXDBTableList();
          LoanUtil.UpdateLoanXDBTables((IClientContext) ClientContext.GetCurrent(), this.id, lastModified, currentData, priorData, loanXdbTableList, new EllieMae.EMLite.ReportingDbUtils.SelectFields(this.SelectFields), loanFolderChanged, isMarshallEnabled, out errorMessage);
          if (errorMessage != null)
          {
            if (num <= 0)
              return errorMessage;
            if (errorMessage.Contains("Invalid column name"))
              ClientContext.GetCurrent()?.Cache?.Remove("LoanXDBStore" + (ClientContext.GetCurrent().UseERDB ? "|ERDB" : ""));
          }
          else
            break;
        }
        catch (Exception ex)
        {
          if (num <= 0)
          {
            TraceLog.WriteError(nameof (Loan), string.Format("[ReportingDb]: update failed for loanId-{0},xrefId-{1},Exception-{2}", (object) this.id.Guid, (object) this.id.XrefId, (object) ex.Message));
            throw ex;
          }
        }
      }
      return errorMessage;
    }

    public void InternalUpdateAuditTrail(
      UserInfo currentUser,
      DateTime auditModifiedTime,
      DateTime currentTime,
      LoanData priorLoanData,
      LoanData currentLoanData,
      string auditUserId)
    {
      this.updateAuditTrail(currentUser, auditModifiedTime, currentTime, priorLoanData, currentLoanData, auditUserId);
    }

    private void updateAuditTrail(
      UserInfo currentUser,
      LoanData priorData,
      DateTime lastModifiedDTTM,
      string auditUserId)
    {
      this.updateAuditTrail(currentUser, lastModifiedDTTM, DateTime.Now, priorData, this.loanData, auditUserId);
    }

    private void updateAuditTrail(
      UserInfo currentUser,
      DateTime lastModifiedDTTM,
      DateTime currentTime,
      LoanData priorData,
      LoanData currentLoanData,
      string auditUserId)
    {
      if (currentUser == (UserInfo) null)
      {
        TraceLog.WriteWarning(nameof (Loan), "NULL user passed to updateAuditTrail. Update skipped.");
      }
      else
      {
        TraceLog.WriteVerbose(nameof (Loan), "Starting update of audit trail fields for loan " + (object) this.id);
        LoanXDBTableList loanXdbTableList = LoanXDBStore.GetLoanXDBTableList();
        if (loanXdbTableList == null)
          return;
        string[] tableNameList = LoanXDBStore.GetTableNameList();
        if (tableNameList == null || tableNameList.Length == 0)
          return;
        DateTime now = DateTime.Now;
        int num = 0;
        Dictionary<int, DbValueList> auditTrailValueDictionary = new Dictionary<int, DbValueList>();
        foreach (LoanXDBField allField in loanXdbTableList.GetAllFields())
        {
          if (allField.Auditable)
          {
            object obj1 = priorData == null ? (object) "" : (object) priorData.GetSimpleField(allField.FieldIDWithCoMortgagor);
            object obj2 = (object) currentLoanData.GetSimpleField(allField.FieldIDWithCoMortgagor);
            if (allField.FieldID == "1393" && string.IsNullOrEmpty(obj1 as string))
              obj1 = (object) "Active Loan";
            if (allField.FieldID == "1393" && string.IsNullOrEmpty(obj2 as string))
              obj2 = (object) "Active Loan";
            if (allField.FieldID.ToUpper() == "LOANLASTMODIFIED")
            {
              obj1 = lastModifiedDTTM == DateTime.MinValue ? (object) null : (object) lastModifiedDTTM;
              obj2 = (object) currentTime;
            }
            if (!object.Equals(obj1, obj2))
            {
              FieldFormat format = currentLoanData.GetFormat(allField.FieldID);
              try
              {
                obj2 = Utils.ConvertToNativeValue(string.Concat(obj2), format, true);
              }
              catch
              {
              }
              try
              {
                obj1 = Utils.ConvertToNativeValue(string.Concat(obj1), format, true);
              }
              catch
              {
              }
              try
              {
                if (!object.Equals(obj2, obj1))
                {
                  if (allField.FieldType == LoanXDBTableList.TableTypes.IsString)
                  {
                    string str1 = obj1?.ToString();
                    string str2 = obj2?.ToString();
                    if (str1 != null && str1.Length > 8000)
                      obj1 = (object) str1.Substring(0, 8000);
                    if (str2 != null && str2.Length > 8000)
                      obj2 = (object) str2.Substring(0, 8000);
                  }
                  auditTrailValueDictionary[allField.FieldXRefID] = AuditTrailAccessor.GenerateAuditRecordDbValueList(this.id.XrefId, allField.FieldXRefID, currentUser, obj1, obj2, now, auditUserId);
                  ++num;
                }
              }
              catch (Exception ex)
              {
                TraceLog.WriteError(nameof (Loan), string.Format("[AuditTrail]: update failed for fieldId-{0}, loanId-{1},xrefId-{2},Exception-{3}", (object) allField.FieldIDWithCoMortgagor, (object) this.id.Guid, (object) this.id.XrefId, (object) ex.StackTrace));
              }
            }
          }
        }
        AuditTrailAccessor.AppendAuditRecord(auditTrailValueDictionary, this.id);
        TimeSpan timeSpan = DateTime.Now - now;
        TraceLog.WriteVerbose(nameof (Loan), "Audit trail update of " + (object) num + " fields completed in " + (object) timeSpan.TotalMilliseconds + "ms");
      }
    }

    private static LoanAlertMonitor getLoanAlertMonitor(LoanData loan)
    {
      if (loan.AlertMonitor != null)
        return (LoanAlertMonitor) loan.AlertMonitor;
      BusinessCalendar businessCalendar = (BusinessCalendar) null;
      BusinessCalendar postalCalendar = (BusinessCalendar) null;
      foreach (AlertConfig alertConfig in loan.Settings.AlertSetupData.AlertConfigList)
      {
        if (alertConfig.Definition is CustomAlert)
        {
          CustomAlert definition = (CustomAlert) alertConfig.Definition;
          if (definition.DateAdjustment != 0)
          {
            if (definition.AdjustmentDayType == DayType.Business && businessCalendar == null)
              businessCalendar = BusinessCalendarAccessor.GetBusinessCalendar(CalendarType.Business);
            else if (definition.AdjustmentDayType == DayType.Postal && postalCalendar == null)
              postalCalendar = BusinessCalendarAccessor.GetBusinessCalendar(CalendarType.Postal);
          }
        }
      }
      return new LoanAlertMonitor(businessCalendar, postalCalendar, loan.Settings.AlertSetupData);
    }

    private void updateLoanSummaryAsync(
      LoanIdentity id,
      LoanData data,
      UserInfo currentUser,
      DateTime newModifiedDate,
      DateTime oldModifiedDate,
      ClientContext clientContext,
      bool allowDeferrable,
      LoanData priorData = null,
      bool deleteInsert = true)
    {
      using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        Loan.updateLoanSummary(id, data, currentUser, newModifiedDate, oldModifiedDate, deleteInsert);
        if (!allowDeferrable || DeferrableManager.IsActivityDeferrableAllowed(2))
          return;
        this.id = Loan.LookupIdentity(id.Guid);
        this.updateLoanXDBTables(newModifiedDate, priorData);
      }
    }

    private static bool IsAsyncSaveEnabled()
    {
      return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableAsyncSave"]);
    }

    private static void updateLoanSummary(
      LoanIdentity id,
      LoanData data,
      UserInfo currentUser,
      DateTime newModifiedDate,
      DateTime oldModifiedDate,
      bool deleteInsert = true)
    {
      LoanAlertMonitor loanAlertMonitor = Loan.getLoanAlertMonitor(data);
      PipelineInfo pipelineInfo = data.ToPipelineInfo((IAlertMonitor) loanAlertMonitor);
      DbQueryBuilder sql = new DbQueryBuilder();
      DbAccessManager dbAccessManager = new DbAccessManager();
      SqlCommand sqlCmd = new SqlCommand();
      List<SqlParameter> sqlParameterList = new List<SqlParameter>();
      DbValue dbValue1 = new DbValue("guid", (object) id.Guid);
      DbTableInfo table1 = DbAccessManager.GetTable("LoanXRef");
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("LoanGuid", (object) id.Guid);
      sql.Declare("@xrefId", "int");
      sql.IfNotExists(table1, dbValueList);
      sql.Begin();
      sql.InsertInto(table1, dbValueList, true, false);
      sql.SelectIdentity("@xrefId");
      sql.End();
      sql.Else();
      sql.AppendLine("  select @xrefId = XRefID from LoanXRef where LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) id.Guid));
      sql.DeleteFrom(DbAccessManager.GetTable("LoanAlerts"), new DbValue("LoanXRefId", (object) "@xrefId", (IDbEncoder) DbEncoding.None));
      DbTableInfo table2 = DbAccessManager.GetTable("LoanSummary");
      DbValueList values1 = new DbValueList();
      values1.Add(dbValue1);
      values1.Add("LoanName", (object) id.LoanName);
      values1.Add("LoanFolder", (object) id.LoanFolder);
      values1.Add("XRefID", (object) "@xrefId", (IDbEncoder) DbEncoding.None);
      values1.Add("LastModified", (object) newModifiedDate);
      values1.Add("LoanVersionNumber", (object) data.LoanVersionNumber);
      values1.Add("IsArchived", (object) (data.GetField("5016").ToLower() == "y" ? 1 : 0));
      if (deleteInsert)
      {
        sql.DeleteFrom(DbAccessManager.GetTable("LoanSummary"), dbValue1);
        sql.InsertInto(table2, values1, true, false);
      }
      else
      {
        sql.IfNotExists(table2, dbValue1);
        sql.Begin();
        sql.InsertInto(table2, values1, true, false);
        sql.End();
        DbValue dbValue2 = new DbValue("contactGuid", (object) id.Guid);
        DbValue key = new DbValue("loanRefId", (object) id.XrefId);
        sql.DeleteFrom(DbAccessManager.GetTable("BorrowerLoans"), key);
        sql.DeleteFrom(DbAccessManager.GetTable("BizPartnerLoans"), key);
      }
      if (pipelineInfo.NeedFixMilestoneIDs)
      {
        Hashtable milestoneNameToGuid = WorkflowBpmDbAccessor.GetCompleteMilestoneNameToGUID();
        pipelineInfo.FixMilestoneIDs(milestoneNameToGuid);
      }
      Hashtable values2 = (Hashtable) pipelineInfo.Info.Clone();
      if (values2[(object) "Guid"].ToString() != id.Guid)
        Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("Guid mismatch while saving loan summary data."));
      values2.Remove((object) "Guid");
      if (deleteInsert)
        values2.Remove((object) "LastModified");
      else
        values2[(object) "LastModified"] = (object) newModifiedDate;
      sql.Declare("@recId", "int");
      sql.Update(table2, new DbValueList(values2), dbValue1);
      if (pipelineInfo.Milestones.Length != 0)
      {
        DbTableInfo table3 = DbAccessManager.GetTable("LoanMilestones");
        if (deleteInsert)
        {
          for (int index = 0; index < pipelineInfo.Milestones.Length; ++index)
          {
            PipelineInfo.MilestoneInfo milestone = pipelineInfo.Milestones[index];
            sql.InsertInto(table3, new DbValueList()
            {
              {
                "guid",
                (object) id.Guid
              },
              {
                "milestoneID",
                (object) milestone.MilestoneID
              },
              {
                "milestoneName",
                (object) milestone.MilestoneName
              },
              {
                "associateGuid",
                (object) milestone.AssociateGuid
              },
              {
                "finished",
                milestone.Finished ? (object) "1" : (object) "0"
              },
              {
                "reviewed",
                milestone.Reviewed ? (object) "1" : (object) "0"
              },
              {
                "roleID",
                (object) milestone.RoleID
              },
              {
                "order",
                (object) string.Concat((object) index)
              },
              {
                "dateStarted",
                (object) milestone.DateStarted
              },
              {
                "dateCompleted",
                (object) milestone.DateCompleted
              }
            }, true, false);
          }
        }
        else
        {
          DataTable dtTable = new DataTable();
          dtTable.Columns.AddRange(new DataColumn[12]
          {
            new DataColumn("Guid", typeof (string)),
            new DataColumn("milestoneID", typeof (string)),
            new DataColumn("milestoneName", typeof (string)),
            new DataColumn("finished", typeof (int)),
            new DataColumn("reviewed", typeof (int)),
            new DataColumn("roleID", typeof (int)),
            new DataColumn("order", typeof (int)),
            new DataColumn("dateStarted", typeof (DateTime)),
            new DataColumn("dateCompleted", typeof (DateTime)),
            new DataColumn("duration", typeof (int)),
            new DataColumn("associateGuid", typeof (string)),
            new DataColumn("LastModified", typeof (DateTime))
          });
          for (int index = 0; index < pipelineInfo.Milestones.Length; ++index)
          {
            PipelineInfo.MilestoneInfo milestone = pipelineInfo.Milestones[index];
            DbValueList values3 = new DbValueList();
            values3.Add("guid", (object) id.Guid);
            values3.Add("milestoneID", (object) milestone.MilestoneID);
            values3.Add("milestoneName", (object) milestone.MilestoneName);
            values3.Add("associateGuid", (object) milestone.AssociateGuid);
            values3.Add("finished", (object) (milestone.Finished ? 1 : 0));
            values3.Add("reviewed", (object) (milestone.Reviewed ? 1 : 0));
            values3.Add("roleID", (object) milestone.RoleID);
            values3.Add("order", (object) string.Concat((object) index));
            if (milestone.DateStarted != DateTime.MinValue)
              values3.Add("dateStarted", (object) milestone.DateStarted);
            else
              values3.Add("dateStarted", (object) DBNull.Value);
            if (milestone.DateCompleted != DateTime.MinValue)
              values3.Add("dateCompleted", (object) milestone.DateCompleted);
            else
              values3.Add("dateCompleted", (object) DBNull.Value);
            sql.InsertIntoDataTable(dtTable, table3, values3);
          }
          SqlParameter sqlParameter = new SqlParameter("@LoanMilestones", (object) dtTable);
          sqlParameter.SqlDbType = SqlDbType.Structured;
          sqlParameter.TypeName = "typ_LoanMilestones";
          sql.AppendLine("EXEC UpdateLoanMilestones @LoanMilestones");
          sqlParameterList.Add(sqlParameter);
        }
      }
      if (pipelineInfo.LoanAssociates.Length != 0)
      {
        DbTableInfo table4 = DbAccessManager.GetTable("LoanAssociates");
        PipelineInfo.LoanAssociateInfo currentLoanAssociate = pipelineInfo.GetCurrentLoanAssociate();
        if (deleteInsert)
        {
          foreach (PipelineInfo.LoanAssociateInfo loanAssociate in pipelineInfo.LoanAssociates)
            sql.InsertInto(table4, new DbValueList()
            {
              {
                "Guid",
                (object) id.Guid
              },
              {
                "AssociateGuid",
                (object) loanAssociate.AssociateGuid
              },
              {
                "RoleID",
                (object) loanAssociate.RoleID
              },
              {
                "MilestoneID",
                (object) loanAssociate.MilestoneID
              },
              {
                "UserID",
                (object) loanAssociate.UserID
              },
              {
                "GroupID",
                (object) loanAssociate.GroupID,
                (IDbEncoder) DbEncoding.MinusOneAsNull
              },
              {
                "AssociateType",
                (object) (int) loanAssociate.AssociateType
              },
              {
                "Name",
                (object) loanAssociate.AssociateName
              },
              {
                "Email",
                (object) loanAssociate.AssociateEmail
              },
              {
                "Phone",
                (object) loanAssociate.AssociatePhone
              },
              {
                "Fax",
                (object) loanAssociate.AssociateFax
              },
              {
                "AllowWrites",
                (object) loanAssociate.WriteAccess,
                (IDbEncoder) DbEncoding.Flag
              },
              {
                "IsCurrent",
                (object) (currentLoanAssociate == loanAssociate),
                (IDbEncoder) DbEncoding.Flag
              },
              {
                "APIClientID",
                (object) loanAssociate.aPIClientID
              }
            }, true, false);
        }
        else
        {
          DataTable dtTable = new DataTable();
          dtTable.Columns.AddRange(new DataColumn[15]
          {
            new DataColumn("Guid", typeof (string)),
            new DataColumn("AssociateGuid", typeof (string)),
            new DataColumn("RoleID", typeof (int)),
            new DataColumn("MilestoneID", typeof (string)),
            new DataColumn("AssociateType", typeof (int)),
            new DataColumn("UserID", typeof (string)),
            new DataColumn("GroupID", typeof (int)),
            new DataColumn("Name", typeof (string)),
            new DataColumn("Email", typeof (string)),
            new DataColumn("Phone", typeof (string)),
            new DataColumn("Fax", typeof (string)),
            new DataColumn("AllowWrites", typeof (bool)),
            new DataColumn("IsCurrent", typeof (bool)),
            new DataColumn("APIClientID", typeof (string)),
            new DataColumn("LastModified", typeof (DateTime))
          });
          foreach (PipelineInfo.LoanAssociateInfo loanAssociate in pipelineInfo.LoanAssociates)
          {
            DbValueList values4 = new DbValueList();
            values4.Add("Guid", (object) id.Guid);
            values4.Add("AssociateGuid", (object) loanAssociate.AssociateGuid);
            values4.Add("RoleID", (object) loanAssociate.RoleID);
            values4.Add("MilestoneID", (object) loanAssociate.MilestoneID);
            values4.Add("UserID", (object) loanAssociate.UserID);
            if (loanAssociate.GroupID == -1)
              values4.Add("GroupID", (object) DBNull.Value);
            else
              values4.Add("GroupID", (object) loanAssociate.GroupID);
            values4.Add("AssociateType", (object) (int) loanAssociate.AssociateType);
            values4.Add("Name", (object) loanAssociate.AssociateName);
            values4.Add("Email", (object) loanAssociate.AssociateEmail);
            values4.Add("Phone", (object) loanAssociate.AssociatePhone);
            values4.Add("Fax", (object) loanAssociate.AssociateFax);
            values4.Add("AllowWrites", (object) loanAssociate.WriteAccess, (IDbEncoder) DbEncoding.Flag);
            values4.Add("IsCurrent", (object) (currentLoanAssociate == loanAssociate), (IDbEncoder) DbEncoding.Flag);
            values4.Add("APIClientID", (object) loanAssociate.aPIClientID);
            sql.InsertIntoDataTable(dtTable, table4, values4);
          }
          SqlParameter sqlParameter = new SqlParameter("@LoanAssociates", (object) dtTable);
          sqlParameter.SqlDbType = SqlDbType.Structured;
          sqlParameter.TypeName = "typ_LoanAssociates";
          sql.AppendLine("EXEC UpdateLoanAssociates @LoanAssociates");
          sqlParameterList.Add(sqlParameter);
        }
      }
      if (pipelineInfo.Borrowers.Length != 0)
      {
        DbTableInfo table5 = DbAccessManager.GetTable("LoanBorrowers");
        if (deleteInsert)
        {
          foreach (PipelineInfo.Borrower borrower in pipelineInfo.Borrowers)
          {
            DbValueList values5 = new DbValueList();
            values5.Add("Guid", (object) id.Guid);
            values5.Add("PairIndex", (object) borrower.PairIndex);
            values5.Add("BorrowerType", (object) (int) borrower.BorrowerType);
            values5.Add("FirstName", (object) borrower.FirstName);
            values5.Add("LastName", (object) borrower.LastName);
            values5.Add("HomePhone", (object) borrower.HomePhone);
            values5.Add("WorkPhone", (object) borrower.WorkPhone);
            values5.Add("CellPhone", (object) borrower.CellPhone);
            values5.Add("Email", (object) borrower.Email);
            values5.Add("WorkEmail", (object) borrower.WorkEmail);
            if ((borrower.SSN ?? "") != "")
              values5.Add("SSN", (object) EllieMae.EMLite.DataAccess.SQL.EncodeToSHA1(borrower.SSN));
            sql.InsertInto(table5, values5, true, false);
          }
        }
        else
        {
          DataTable dtTable = new DataTable();
          dtTable.Columns.AddRange(new DataColumn[13]
          {
            new DataColumn("Guid", typeof (string)),
            new DataColumn("PairIndex", typeof (int)),
            new DataColumn("BorrowerType", typeof (int)),
            new DataColumn("FirstName", typeof (string)),
            new DataColumn("LastName", typeof (string)),
            new DataColumn("HomePhone", typeof (string)),
            new DataColumn("WorkPhone", typeof (string)),
            new DataColumn("CellPhone", typeof (string)),
            new DataColumn("Email", typeof (string)),
            new DataColumn("SSN", typeof (string)),
            new DataColumn("ID", typeof (int)),
            new DataColumn("WorkEmail", typeof (string)),
            new DataColumn("LastModified", typeof (DateTime))
          });
          foreach (PipelineInfo.Borrower borrower in pipelineInfo.Borrowers)
          {
            DbValueList values6 = new DbValueList();
            values6.Add("Guid", (object) id.Guid);
            values6.Add("PairIndex", (object) borrower.PairIndex);
            values6.Add("BorrowerType", (object) (int) borrower.BorrowerType);
            values6.Add("FirstName", (object) borrower.FirstName);
            values6.Add("LastName", (object) borrower.LastName);
            values6.Add("HomePhone", (object) borrower.HomePhone);
            values6.Add("WorkPhone", (object) borrower.WorkPhone);
            values6.Add("CellPhone", (object) borrower.CellPhone);
            values6.Add("Email", (object) borrower.Email);
            values6.Add("WorkEmail", (object) borrower.WorkEmail);
            if ((borrower.SSN ?? "") != "")
              values6.Add("SSN", (object) EllieMae.EMLite.DataAccess.SQL.EncodeToSHA1(borrower.SSN));
            sql.InsertIntoDataTable(dtTable, table5, values6);
          }
          SqlParameter sqlParameter = new SqlParameter("@LoanBorrowers", (object) dtTable);
          sqlParameter.SqlDbType = SqlDbType.Structured;
          sqlParameter.TypeName = "typ_LoanBorrowers";
          sql.AppendLine("EXEC UpdateLoanBorrowers @LoanBorrowers");
          sqlParameterList.Add(sqlParameter);
        }
      }
      CRMLog[] allCrmMapping = data.GetLogList().GetAllCRMMapping();
      BorrowerPair[] borrowerPairs = data.GetBorrowerPairs();
      if (allCrmMapping.Length != 0)
      {
        DbTableInfo table6 = DbAccessManager.GetTable("BorrowerLoans");
        DbTableInfo table7 = DbAccessManager.GetTable("BizPartnerLoans");
        foreach (CRMLog crmLog in allCrmMapping)
        {
          if (crmLog.MappingType == CRMLogType.BorrowerContact)
          {
            for (int index = 0; index < borrowerPairs.Length; ++index)
            {
              BorrowerPair borrowerPair = borrowerPairs[index];
              DbValueList values7 = new DbValueList();
              values7.Add("ContactGuid", (object) crmLog.ContactGuid);
              values7.Add("loanRefId", (object) "@xrefId", (IDbEncoder) DbEncoding.None);
              values7.Add("PairIndex", (object) index);
              if (borrowerPair.Borrower.Id == crmLog.MappingID)
              {
                values7.Add("RoleType", (object) 0);
                sql.InsertInto(table6, values7, true, false);
              }
              else if (borrowerPair.CoBorrower.Id == crmLog.MappingID)
              {
                values7.Add("RoleType", (object) 1);
                sql.InsertInto(table6, values7, true, false);
              }
            }
          }
          else
            sql.InsertInto(table7, new DbValueList()
            {
              {
                "ContactGuid",
                (object) crmLog.ContactGuid
              },
              {
                "loanRefId",
                (object) "@xrefId",
                (IDbEncoder) DbEncoding.None
              },
              {
                "RoleType",
                (object) (int) crmLog.RoleType
              },
              {
                "MappingID",
                (object) crmLog.MappingID
              }
            }, true, false);
        }
      }
      PipelineInfo.Alert[] existingLoanAlerts = Loan.GetExistingLoanAlerts(id.Guid);
      Loan._constructAlertUpdateQuery(sql, currentUser, id, pipelineInfo, false, existingLoanAlerts);
      sqlCmd.CommandText = sql.ToString();
      sqlCmd.Parameters.AddRange(sqlParameterList.ToArray());
      dbAccessManager.ExecuteNonQuery((IDbCommand) sqlCmd);
      foreach (AlertChange loanAlertChange in Loan.GetLoanAlertChanges(id, pipelineInfo, existingLoanAlerts, currentUser.userName))
        Loan.PublishLoanAlertChangeKafkaEvent(id.Guid, currentUser.Userid, newModifiedDate, new List<AlertChange>()
        {
          loanAlertChange
        });
      LoanSummaryExtension extension = new LoanSummaryExtension();
      extension.CorrespondentTradeGuid = pipelineInfo.GetField("CorrespondentTradeGuid") as string;
      extension.TradeGuid = pipelineInfo.GetField("TradeGuid") as string;
      extension.Guid = pipelineInfo.GetField("Guid") as string;
      extension.LoanNumber = pipelineInfo.GetField("LoanNumber") as string;
      extension.LoanAmount = Utils.ParseDecimal(pipelineInfo.GetField("TotalLoanAmount"));
      extension.InvestorStatus = string.Concat(pipelineInfo.GetField("InvestorStatus"));
      extension.InvestorStatusDate = Utils.ParseDate(pipelineInfo.GetField("InvestorStatusDate"), DateTime.Now);
      extension.PurchasedPrinciple = Utils.ParseDecimal((object) data.GetField("3579"));
      try
      {
        extension.PurchaseDate = Utils.ParseDate((object) data.GetField("3567"), true);
      }
      catch
      {
      }
      try
      {
        extension.ReceivedDate = Utils.ParseDate((object) data.GetField("3917"), true);
      }
      catch
      {
      }
      try
      {
        extension.RejectedDate = Utils.ParseDate((object) data.GetField("3940"), true);
      }
      catch
      {
      }
      extension.TpoCompanyId = pipelineInfo.GetField("TPOCompanyID") as string;
      if ((string.Equals(pipelineInfo.GetField("RateIsLocked") as string, "Y") || string.Equals(data.GetField("4532"), "Y")) && !string.IsNullOrEmpty(data.GetField("3967")))
        extension.DeliveryType = Utils.GetEnumValueFromDescription<CorrespondentMasterDeliveryType>(data.GetField("3967"));
      try
      {
        extension.SubmittedForReviewDate = Utils.ParseDate((object) data.GetField("4119"), true);
      }
      catch
      {
        extension.SubmittedForReviewDate = DateTime.MinValue;
      }
      try
      {
        extension.PurchaseSuspenseDate = Utils.ParseDate((object) data.GetField("3918"), true);
      }
      catch
      {
        extension.PurchaseSuspenseDate = DateTime.MinValue;
      }
      try
      {
        extension.PurchaseApprovalDate = Utils.ParseDate((object) data.GetField("3920"), true);
      }
      catch
      {
        extension.PurchaseApprovalDate = DateTime.MinValue;
      }
      try
      {
        extension.ClearedForPurchaseDate = Utils.ParseDate((object) data.GetField("3921"), true);
      }
      catch
      {
        extension.ClearedForPurchaseDate = DateTime.MinValue;
      }
      try
      {
        extension.CancelledDate = Utils.ParseDate((object) data.GetField("4207"), true);
      }
      catch
      {
        extension.CancelledDate = DateTime.MinValue;
      }
      try
      {
        extension.VoidedDate = Utils.ParseDate((object) data.GetField("4208"), true);
      }
      catch
      {
        extension.VoidedDate = DateTime.MinValue;
      }
      try
      {
        extension.WithdrawalRequestedDate = Utils.ParseDate((object) data.GetField("4242"), true);
      }
      catch
      {
        extension.WithdrawalRequestedDate = DateTime.MinValue;
      }
      extension.LenderCaseIdentifier = data.GetField("305");
      LoanSummaryExtensions.UpdateSummeryExtension(extension);
      try
      {
        if (data.LoanVersionNumber <= 0)
          return;
        string fullLoanFilePath = new LoanFolder(id.LoanFolder).GetFullLoanFilePath(id.LoanName);
        string str1 = fullLoanFilePath.Replace(new FileInfo(fullLoanFilePath).Name, "Versions");
        string str2 = str1 + "\\" + string.Format("{0:D5}_", (object) data.LoanVersionNumber) + "loan.em";
        bool flag = data.LoanVersionNumber > 1 && File.Exists(str1 + "\\" + string.Format("{0:D5}_", (object) (data.LoanVersionNumber - 1)).ToString() + "loan.em");
        string str3 = EllieMae.EMLite.DataAccess.SQL.Encode((object) id.Guid);
        string str4 = EllieMae.EMLite.DataAccess.SQL.Encode((object) newModifiedDate);
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        if (data.LoanVersionNumber > 1 & flag)
          dbQueryBuilder.AppendFormat(" update LoanVersionDetails set DateUpdated={0} where Guid={1} AND VersionNumber={2}", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) DateTime.Now), (object) str3, (object) (data.LoanVersionNumber - 1));
        else if (data.LoanVersionNumber > 1 && !flag)
          dbQueryBuilder.AppendFormat(" insert into LoanVersionDetails(Guid, VersionNumber, DateCreated, DateUpdated) values ({0}, {1}, {2}, {3})", (object) str3, (object) (data.LoanVersionNumber - 1), (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) oldModifiedDate), (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) DateTime.Now));
        dbQueryBuilder.AppendFormat(" insert into LoanVersionDetails(Guid, VersionNumber, DateCreated, DateUpdated) values ({0}, {1}, {2}, {3})", (object) str3, (object) data.LoanVersionNumber, (object) str4, (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) DateTime.Now));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), "Error updating version details table: " + ex.Message);
      }
    }

    public static bool PublishLoanAlertChangeKafkaEvent(
      string loanId,
      string userId,
      DateTime loanModifiedTime,
      List<AlertChange> alertChanges)
    {
      ClientContext current = ClientContext.GetCurrent();
      if (!Utils.ParseBoolean((object) Company.GetCompanySetting("FEATURE", "PublishLoanAlertChange")))
        return false;
      bool flag = false;
      try
      {
        bool isSourceEncompass = EncompassServer.ServerMode != EncompassServerMode.Service;
        TraceLog.WriteInfo(nameof (Loan), string.Format("PublishLoanAlertChangeKafkaEvent : Values of isSourceEncompassRequest - {0} ", (object) isSourceEncompass));
        Enums.Source source = isSourceEncompass ? Enums.Source.URN_ELLI_SERVICE_ENCOMPASS : Enums.Source.URN_ELLI_SERVICE_EBS;
        WebHooksEvent queueEvent = new WebHooksEvent("serviceId", current.InstanceName, "siteId", "alertChange", userId, source, loanModifiedTime);
        if (loanId != null && loanId.StartsWith("{") && loanId.EndsWith("}"))
          loanId = loanId.TrimStart('{').TrimEnd('}');
        queueEvent.StandardMessage.EntityId = loanId;
        AlertChangeEvent resourceList = new AlertChangeEvent()
        {
          AlertChanges = alertChanges
        };
        if (resourceList.AlertChanges != null)
          queueEvent.AddAlertChangeKafkaMessage(loanId, Guid.NewGuid().ToString(), current.ClientID, current.InstanceName, resourceList, isSourceEncompass);
        if (queueEvent.QueueMessages.Count > 0)
        {
          IMessageQueueEventService queueEventService = (IMessageQueueEventService) new MessageQueueEventService();
          IMessageQueueProcessor processor = (IMessageQueueProcessor) new KafkaProcessor();
          queueEventService.MessageQueueProducer((MessageQueueEvent) queueEvent, processor);
          flag = true;
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("Exception publishing alert change webhook loanEvent to kafka for loanId - {0}. Exception details {1}", (object) loanId, (object) ex.StackTrace));
      }
      return flag;
    }

    private static void updateDraftLoanSummary(
      LoanIdentity id,
      LoanData data,
      UserInfo currentUser,
      DateTime lastModified)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      if (currentUser.BorrowerContext.Claims != null)
      {
        ClaimCollection claims = currentUser.BorrowerContext.Claims;
        int num1 = ((IEnumerable<Claim>) claims).Where<Claim>((System.Func<Claim, bool>) (x => x.ClaimType == "elli_idt" && x.Value == "Consumer")).Count<Claim>();
        int num2 = ((IEnumerable<Claim>) claims).Where<Claim>((System.Func<Claim, bool>) (x => x.ClaimType == "elli_idt" && x.Value == "Application")).Count<Claim>();
        if (num1 > 0)
        {
          empty1 = ((IEnumerable<Claim>) claims).Where<Claim>((System.Func<Claim, bool>) (x => x.ClaimType == "sub")).Select<Claim, string>((System.Func<Claim, string>) (x => x.Value)).First<string>().ToString();
          if (((IEnumerable<Claim>) claims).Where<Claim>((System.Func<Claim, bool>) (x => x.ClaimType == "site_id")).Select<Claim, string>((System.Func<Claim, string>) (x => x.Value)).Count<string>() <= 0)
            throw new Exception("The SiteId should be present for a Borrower token");
          empty3 = ((IEnumerable<Claim>) claims).Where<Claim>((System.Func<Claim, bool>) (x => x.ClaimType == "site_id")).Select<Claim, string>((System.Func<Claim, string>) (x => x.Value)).First<string>().ToString();
        }
        if (num2 > 0)
        {
          if (((IEnumerable<Claim>) claims).Where<Claim>((System.Func<Claim, bool>) (x => x.ClaimType == "site_id")).Select<Claim, string>((System.Func<Claim, string>) (x => x.Value)).Count<string>() <= 0)
            throw new Exception("The SiteId should be present for an Anonymous User token");
          empty3 = ((IEnumerable<Claim>) claims).Where<Claim>((System.Func<Claim, bool>) (x => x.ClaimType == "site_id")).Select<Claim, string>((System.Func<Claim, string>) (x => x.Value)).First<string>().ToString();
        }
      }
      data.GetBorrowerPairs();
      PipelineInfo pipelineInfo = data.ToPipelineInfo();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (string.IsNullOrEmpty(pipelineInfo.Info[(object) "loanOfficerId"].ToString()))
        throw new Exception("Your request should contain a contact with Type as LOAN_OFFICER (LoginId field is the LoanOfficer Id inside Contacts)");
      DbTableInfo table = DbAccessManager.GetTable("ApplicationSummary");
      DbValueList values = new DbValueList();
      values.Add("Guid", (object) id.LoanName);
      values.Add("Status", (object) "Draft");
      values.Add("LastModified", (object) DateTime.Now);
      values.Add("LoanOfficerId", pipelineInfo.Info[(object) "loanOfficerId"]);
      values.Add("LoanOfficerName", pipelineInfo.Info[(object) "LoanOfficerName"]);
      values.Add("Address1", pipelineInfo.Info[(object) "Address1"]);
      values.Add("City", pipelineInfo.Info[(object) "City"]);
      values.Add("State", pipelineInfo.Info[(object) "State"]);
      values.Add("Zip", pipelineInfo.Info[(object) "Zip"]);
      values.Add("SiteId", (object) empty3);
      values.Add("LenderId", (object) empty2);
      values.Add("ConsumerId", (object) empty1);
      values.Add("LoanType", pipelineInfo.Info[(object) "LoanType"]);
      values.Add("LoanAmount", pipelineInfo.Info[(object) "LoanAmount"]);
      values.Add("EstimatedValue", pipelineInfo.Info[(object) "EstimatedValue"]);
      values.Add("Income", pipelineInfo.Info[(object) "TotalMonthlyIncome"]);
      values.Add("LoanPurpose", pipelineInfo.Info[(object) "LoanPurpose"]);
      if (pipelineInfo.Borrowers.Length != 0)
      {
        foreach (PipelineInfo.Borrower borrower in pipelineInfo.Borrowers)
        {
          if (borrower.BorrowerType == LoanBorrowerType.Borrower)
          {
            values.Add("BorrowerHomePhone", (object) borrower.HomePhone);
            values.Add("BorrowerEmail", (object) borrower.Email);
          }
          else if (borrower.BorrowerType == LoanBorrowerType.Coborrower)
          {
            values.Add("CoBorrowerHomePhone", (object) borrower.HomePhone);
            values.Add("CoBorrowerEmail", (object) borrower.Email);
          }
        }
      }
      values.Add("Created", (object) DateTime.Now);
      values.Add("RESPA6", (object) Loan.CalculateRESPAFields(pipelineInfo, data));
      if (pipelineInfo.Borrowers.Length != 0)
      {
        foreach (PipelineInfo.Borrower borrower in pipelineInfo.Borrowers)
        {
          if (borrower.PairIndex == 1)
          {
            if (borrower.BorrowerType == LoanBorrowerType.Borrower)
            {
              values.Add("BorrowerFirstName", (object) borrower.FirstName);
              values.Add("BorrowerLastName", (object) borrower.LastName);
              if ((borrower.SSN ?? "") != "")
                values.Add("BorrowerSSN", (object) EllieMae.EMLite.DataAccess.SQL.EncodeToSHA1(borrower.SSN));
            }
            else if (borrower.BorrowerType == LoanBorrowerType.Coborrower)
            {
              values.Add("CoBorrowerFirstName", (object) borrower.FirstName);
              values.Add("CoBorrowerLastName", (object) borrower.LastName);
              if ((borrower.SSN ?? "") != "")
                values.Add("CoBorrowerSSN", (object) EllieMae.EMLite.DataAccess.SQL.EncodeToSHA1(borrower.SSN));
            }
          }
        }
      }
      dbQueryBuilder.InsertInto(table, values, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static bool AuthCheckDraftLoan(UserInfo currentUser, string draftLoanGuid)
    {
      string empty1 = string.Empty;
      string str = string.Empty;
      string empty2 = string.Empty;
      if (currentUser.BorrowerContext.Claims != null)
      {
        ClaimCollection claims = currentUser.BorrowerContext.Claims;
        int num1 = ((IEnumerable<Claim>) claims).Where<Claim>((System.Func<Claim, bool>) (x => x.ClaimType == "elli_idt" && x.Value == "Consumer")).Count<Claim>();
        int num2 = ((IEnumerable<Claim>) claims).Where<Claim>((System.Func<Claim, bool>) (x => x.ClaimType == "elli_idt" && x.Value == "Enterprise")).Count<Claim>();
        int num3 = ((IEnumerable<Claim>) claims).Where<Claim>((System.Func<Claim, bool>) (x => x.ClaimType == "elli_idt" && x.Value == "Application")).Count<Claim>();
        if (num1 > 0)
          empty1 = ((IEnumerable<Claim>) claims).Where<Claim>((System.Func<Claim, bool>) (x => x.ClaimType == "sub")).Select<Claim, string>((System.Func<Claim, string>) (x => x.Value)).First<string>().ToString();
        if (num2 > 0)
          str = UserIdentity.Parse(((IEnumerable<Claim>) claims).Where<Claim>((System.Func<Claim, bool>) (x => x.ClaimType == "elli_uid")).Select<Claim, string>((System.Func<Claim, string>) (x => x.Value)).First<string>().ToString().ToString()).UserID;
        if (num3 > 0)
          return false;
      }
      if (!string.IsNullOrEmpty(empty1))
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select consumerid from ApplicationSummary where consumerid = '" + empty1 + "' and Guid = '" + draftLoanGuid + "'");
        return dbQueryBuilder.Execute().Count != 0;
      }
      if (string.IsNullOrEmpty(str))
        return false;
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      dbQueryBuilder1.AppendLine("select loanofficerid from ApplicationSummary where loanofficerid = '" + str + "' and Guid = '" + draftLoanGuid + "'");
      return dbQueryBuilder1.Execute().Count != 0;
    }

    private static void updateDraftApplicationLoanSummary(
      DraftLoanIdentity id,
      LoanData data,
      UserInfo currentUser,
      DateTime lastModified)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      if (currentUser.BorrowerContext.Claims != null)
      {
        ClaimCollection claims = currentUser.BorrowerContext.Claims;
        if (((IEnumerable<Claim>) claims).Where<Claim>((System.Func<Claim, bool>) (x => x.ClaimType == "elli_idt" && x.Value == "Consumer")).Count<Claim>() > 0)
        {
          empty1 = ((IEnumerable<Claim>) claims).Where<Claim>((System.Func<Claim, bool>) (x => x.ClaimType == "sub")).Select<Claim, string>((System.Func<Claim, string>) (x => x.Value)).First<string>().ToString();
          if (((IEnumerable<Claim>) claims).Where<Claim>((System.Func<Claim, bool>) (x => x.ClaimType == "site_id")).Select<Claim, string>((System.Func<Claim, string>) (x => x.Value)).Count<string>() <= 0)
            throw new Exception("The SiteId should be present for a Borrower token");
          empty2 = ((IEnumerable<Claim>) claims).Where<Claim>((System.Func<Claim, bool>) (x => x.ClaimType == "site_id")).Select<Claim, string>((System.Func<Claim, string>) (x => x.Value)).First<string>().ToString();
        }
      }
      data.GetBorrowerPairs();
      PipelineInfo pipelineInfo = data.ToPipelineInfo();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("ApplicationSummary");
      DbValueList values = new DbValueList();
      DbValue key = new DbValue("guid", (object) id.Guid);
      DbTableInfo table2 = DbAccessManager.GetTable("LoanXRef");
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("LoanGuid", (object) id.Guid);
      dbQueryBuilder.Declare("@xrefId", "int");
      dbQueryBuilder.IfNotExists(table2, dbValueList);
      dbQueryBuilder.Begin();
      dbQueryBuilder.InsertInto(table2, dbValueList, true, false);
      dbQueryBuilder.SelectIdentity("@xrefId");
      dbQueryBuilder.End();
      dbQueryBuilder.Else();
      dbQueryBuilder.AppendLine("  select @xrefId = XRefID from LoanXRef where LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) id.Guid));
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("LoanAlerts"), new DbValue("LoanXRefId", (object) "@xrefId", (IDbEncoder) DbEncoding.None));
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("ApplicationSummary"), key);
      values.Add("Guid", (object) id.LoanName);
      values.Add("Status", data.IsDraftLoanSubmitted ? (object) "Submit" : (object) "Draft");
      values.Add("LastModified", (object) DateTime.Now);
      values.Add("LoanOfficerId", pipelineInfo.Info[(object) "loanOfficerId"]);
      values.Add("LoanOfficerName", pipelineInfo.Info[(object) "LoanOfficerName"]);
      values.Add("Address1", pipelineInfo.Info[(object) "Address1"]);
      values.Add("City", pipelineInfo.Info[(object) "City"]);
      values.Add("State", pipelineInfo.Info[(object) "State"]);
      values.Add("Zip", pipelineInfo.Info[(object) "Zip"]);
      values.Add("SiteId", (object) empty2);
      values.Add("ConsumerId", (object) empty1);
      values.Add("LoanType", pipelineInfo.Info[(object) "LoanType"]);
      values.Add("LoanAmount", pipelineInfo.Info[(object) "LoanAmount"]);
      values.Add("EstimatedValue", pipelineInfo.Info[(object) "EstimatedValue"]);
      values.Add("Income", pipelineInfo.Info[(object) "TotalMonthlyIncome"]);
      values.Add("LoanPurpose", pipelineInfo.Info[(object) "LoanPurpose"]);
      if (pipelineInfo.Borrowers.Length != 0)
      {
        foreach (PipelineInfo.Borrower borrower in pipelineInfo.Borrowers)
        {
          if (borrower.BorrowerType == LoanBorrowerType.Borrower)
          {
            values.Add("BorrowerHomePhone", (object) borrower.HomePhone);
            values.Add("BorrowerEmail", (object) borrower.Email);
          }
          else if (borrower.BorrowerType == LoanBorrowerType.Coborrower)
          {
            values.Add("CoBorrowerHomePhone", (object) borrower.HomePhone);
            values.Add("CoBorrowerEmail", (object) borrower.Email);
          }
        }
      }
      values.Add("Created", (object) DateTime.Now);
      values.Add("RESPA6", (object) Loan.CalculateRESPAFields(pipelineInfo, data));
      if (pipelineInfo.Borrowers.Length != 0)
      {
        foreach (PipelineInfo.Borrower borrower in pipelineInfo.Borrowers)
        {
          if (borrower.PairIndex == 1)
          {
            if (borrower.BorrowerType == LoanBorrowerType.Borrower)
            {
              values.Add("BorrowerFirstName", (object) borrower.FirstName);
              values.Add("BorrowerLastName", (object) borrower.LastName);
              if ((borrower.SSN ?? "") != "")
                values.Add("BorrowerSSN", (object) EllieMae.EMLite.DataAccess.SQL.EncodeToSHA1(borrower.SSN));
            }
            else if (borrower.BorrowerType == LoanBorrowerType.Coborrower)
            {
              values.Add("CoBorrowerFirstName", (object) borrower.FirstName);
              values.Add("CoBorrowerLastName", (object) borrower.LastName);
              if ((borrower.SSN ?? "") != "")
                values.Add("CoBorrowerSSN", (object) EllieMae.EMLite.DataAccess.SQL.EncodeToSHA1(borrower.SSN));
            }
          }
        }
      }
      dbQueryBuilder.InsertInto(table1, values, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static bool getLoanIsArchive(int xrefId)
    {
      DbValue key = new DbValue("XrefId", (object) xrefId);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("LoanSummary"), new string[1]
      {
        "IsArchived"
      }, key);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count != 0 && (bool) dataRowCollection[0]["IsArchived"];
    }

    private string updateEFolderReportingTables(LoanData currentData = null)
    {
      if (this._xdbLogs)
        TraceLog.WriteWarning(nameof (Loan), string.Format("updateEFolderReportingTables - Starting update of efolder reporting fields for the loan: {0}", (object) this.id));
      TraceLog.WriteVerbose(nameof (Loan), "Starting update of efolder reporting fields for loan " + (object) this.id);
      DateTime now = DateTime.Now;
      string str = (string) null;
      string errorMessage = string.Empty;
      DbQueryBuilder sql = new DbQueryBuilder();
      try
      {
        if (currentData == null)
          currentData = this.loanData;
        Loan._constructDocumentsUpdateQuery(sql, currentData, this.id);
        Loan._constructEnhancedConditionsUpdateQuery(sql, currentData, this.id, out errorMessage);
        if (!string.IsNullOrWhiteSpace(errorMessage))
          TraceLog.WriteVerbose(nameof (Loan), errorMessage);
        if (sql.Length > 0)
          sql.ExecuteNonQuery();
        TraceLog.WriteVerbose(nameof (Loan), "Update of efolder reporting tables completed in " + (object) (DateTime.Now - now).TotalMilliseconds + "ms");
        if (this._xdbLogs)
          TraceLog.WriteWarning(nameof (Loan), string.Format("updateEFolderReportingTables - Update of efolder reporting tables completed in: {0}", (object) this.id));
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("[ReportingDb]:update efolder reporting table failed for loanId-{0},xrefId-{1},Exception-{2}", (object) this.id.Guid, (object) this.id.XrefId, (object) ex.Message));
        str = ex.Message;
        if (this._xdbLogs)
          TraceLog.WriteWarning(nameof (Loan), string.Format("updateEFolderReportingTables - update efolder reporting table failed for loanId: {0}", (object) this.id));
      }
      return str;
    }

    internal static void _constructDocumentsUpdateQuery(
      DbQueryBuilder sql,
      LoanData loanData,
      LoanIdentity loanIdentity)
    {
      DocumentLog[] allDocuments = loanData.GetLogList().GetAllDocuments();
      if (allDocuments == null || allDocuments.Length == 0)
        return;
      DocumentReportingAccessor.GetAddUpdateDocumentsSQL(ref sql, allDocuments, loanIdentity.XrefId);
    }

    internal static void _constructEnhancedConditionsUpdateQuery(
      DbQueryBuilder sql,
      LoanData loanData,
      LoanIdentity loanIdentity,
      out string errorMessage)
    {
      errorMessage = string.Empty;
      EnhancedConditionLog[] enhancedConditions = loanData.GetLogList().GetAllEnhancedConditions(includeRemoved: true);
      if (enhancedConditions == null || enhancedConditions.Length == 0)
        return;
      EnhancedConditionReportingAccessor.GetAddUpdateEnhancedConditionsSQL(ref sql, enhancedConditions, out errorMessage, loanIdentity.XrefId);
    }

    public static PipelineInfo.Alert[] GetExistingLoanAlerts(string loanGuid)
    {
      return AlertConfigAccessor.GetLoanAlerts(loanGuid);
    }

    public static IEnumerable<AlertChange> GetLoanAlertChanges(
      LoanIdentity loanIdentity,
      PipelineInfo pinfo,
      PipelineInfo.Alert[] priorAlerts,
      string username,
      bool forceClearAlerts = false)
    {
      try
      {
        if (!Utils.ParseBoolean((object) Company.GetCompanySetting("FEATURE", "PublishLoanAlertChange")))
          return (IEnumerable<AlertChange>) new List<AlertChange>();
        LoanFolder loanFolder = new LoanFolder(loanIdentity.LoanFolder);
        bool flag1 = Utils.ParseDate(pinfo.GetField("DateCompleted")) != DateTime.MinValue;
        bool flag2 = LoanStatusMap.IsAdverseStatus(string.Concat(pinfo.GetField("ActionTaken")));
        List<AlertChange> loanAlertChanges = new List<AlertChange>();
        Dictionary<string, EllieMae.EMLite.Workflow.Milestone> milestonesDictionary = WorkflowBpmDbAccessor.GetMilestones(true).ToDictionary<EllieMae.EMLite.Workflow.Milestone, string>((System.Func<EllieMae.EMLite.Workflow.Milestone, string>) (m => m.MilestoneID));
        Dictionary<int, AlertConfig> alertConfigDictionary = ((IEnumerable<AlertConfig>) AlertConfigAccessor.GetAlertConfigList()).ToDictionary<AlertConfig, int>((System.Func<AlertConfig, int>) (a => a.AlertID));
        IEnumerable<PipelineInfo.Alert> currentAlerts = ((IEnumerable<PipelineInfo.Alert>) pinfo.Alerts).Where<PipelineInfo.Alert>((System.Func<PipelineInfo.Alert, bool>) (a => alertConfigDictionary[a.AlertID].Definition.Category == AlertCategory.Regulation));
        IEnumerable<PipelineInfo.Alert> dbAlerts = ((IEnumerable<PipelineInfo.Alert>) priorAlerts).Where<PipelineInfo.Alert>((System.Func<PipelineInfo.Alert, bool>) (a => alertConfigDictionary[a.AlertID].Definition.Category == AlertCategory.Regulation));
        if (((((loanFolder.FolderType == LoanFolderInfo.LoanFolderType.Archive ? 1 : (loanFolder.FolderType == LoanFolderInfo.LoanFolderType.Trash ? 1 : 0)) | (flag1 ? 1 : 0) | (flag2 ? 1 : 0)) != 0 ? 1 : (pinfo.Alerts.Length == 0 ? 1 : 0)) | (forceClearAlerts ? 1 : 0)) != 0)
        {
          loanAlertChanges.AddRange(dbAlerts.Where<PipelineInfo.Alert>((System.Func<PipelineInfo.Alert, bool>) (a => alertConfigDictionary.ContainsKey(a.AlertID))).Where<PipelineInfo.Alert>((System.Func<PipelineInfo.Alert, bool>) (a => alertConfigDictionary[a.AlertID].AlertEnabled)).Select<PipelineInfo.Alert, AlertChange>((System.Func<PipelineInfo.Alert, AlertChange>) (a => AlertChange.GetAlertChange(a, alertConfigDictionary[a.AlertID], EllieMae.EMLite.ClientServer.MessageServices.Message.Alerts.AlertStatus.Cleared, milestonesDictionary, username))));
          return (IEnumerable<AlertChange>) loanAlertChanges;
        }
        IEnumerable<AlertChange> collection1 = currentAlerts.Where<PipelineInfo.Alert>((System.Func<PipelineInfo.Alert, bool>) (a => !dbAlerts.Any<PipelineInfo.Alert>((System.Func<PipelineInfo.Alert, bool>) (pa => Loan.isMatching(a, pa))))).Where<PipelineInfo.Alert>((System.Func<PipelineInfo.Alert, bool>) (a => alertConfigDictionary.ContainsKey(a.AlertID))).Where<PipelineInfo.Alert>((System.Func<PipelineInfo.Alert, bool>) (a => alertConfigDictionary[a.AlertID].AlertEnabled)).Select<PipelineInfo.Alert, AlertChange>((System.Func<PipelineInfo.Alert, AlertChange>) (a => AlertChange.GetAlertChange(a, alertConfigDictionary[a.AlertID], EllieMae.EMLite.ClientServer.MessageServices.Message.Alerts.AlertStatus.Open, milestonesDictionary, username)));
        loanAlertChanges.AddRange(collection1);
        IEnumerable<AlertChange> collection2 = dbAlerts.Where<PipelineInfo.Alert>((System.Func<PipelineInfo.Alert, bool>) (a => !currentAlerts.Any<PipelineInfo.Alert>((System.Func<PipelineInfo.Alert, bool>) (pa => Loan.isMatching(a, pa))))).Where<PipelineInfo.Alert>((System.Func<PipelineInfo.Alert, bool>) (a => alertConfigDictionary.ContainsKey(a.AlertID))).Where<PipelineInfo.Alert>((System.Func<PipelineInfo.Alert, bool>) (a => alertConfigDictionary[a.AlertID].AlertEnabled)).Select<PipelineInfo.Alert, AlertChange>((System.Func<PipelineInfo.Alert, AlertChange>) (a => AlertChange.GetAlertChange(a, alertConfigDictionary[a.AlertID], EllieMae.EMLite.ClientServer.MessageServices.Message.Alerts.AlertStatus.Cleared, milestonesDictionary, username)));
        loanAlertChanges.AddRange(collection2);
        IEnumerable<AlertChange> collection3 = currentAlerts.Join(dbAlerts, a => new
        {
          AlertID = a.AlertID,
          AlertTargetID = a.AlertTargetID,
          Event = a.Event,
          UserID_DefaultEmpty = a.UserID_DefaultEmpty,
          GroupID = a.GroupID
        }, dba => new
        {
          AlertID = dba.AlertID,
          AlertTargetID = dba.AlertTargetID,
          Event = dba.Event,
          UserID_DefaultEmpty = dba.UserID_DefaultEmpty,
          GroupID = dba.GroupID
        }, (a, dba) => new{ Current = a, Prev = dba }).Where(o => o.Current.Date.Date != o.Prev.Date.Date).Where(a => alertConfigDictionary[a.Current.AlertID].AlertEnabled).Select(o => AlertChange.GetAlertChange(o.Current, alertConfigDictionary[o.Current.AlertID], EllieMae.EMLite.ClientServer.MessageServices.Message.Alerts.AlertStatus.Open, milestonesDictionary, username));
        loanAlertChanges.AddRange(collection3);
        return (IEnumerable<AlertChange>) loanAlertChanges;
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("Exception while identifying alert changes for loanId - {0}. Exception details {1}", (object) loanIdentity.Guid, (object) ex.StackTrace));
        return (IEnumerable<AlertChange>) new List<AlertChange>();
      }
    }

    internal static void _constructAlertUpdateQuery(
      DbQueryBuilder sql,
      UserInfo currentUser,
      LoanIdentity loanIdentity,
      PipelineInfo pinfo,
      bool addSelectXrefId,
      PipelineInfo.Alert[] priorAlerts)
    {
      LoanFolder loanFolder = new LoanFolder(loanIdentity.LoanFolder);
      bool flag1 = Utils.ParseDate(pinfo.GetField("DateCompleted")) != DateTime.MinValue;
      bool flag2 = LoanStatusMap.IsAdverseStatus(string.Concat(pinfo.GetField("ActionTaken")));
      if (((loanFolder.FolderType == LoanFolderInfo.LoanFolderType.Archive ? 1 : (loanFolder.FolderType == LoanFolderInfo.LoanFolderType.Trash ? 1 : 0)) | (flag1 ? 1 : 0) | (flag2 ? 1 : 0)) != 0 || pinfo.Alerts.Length == 0)
        return;
      AlertConfig[] alertConfigList = AlertConfigAccessor.GetAlertConfigList();
      DbTableInfo table = DbAccessManager.GetTable("LoanAlerts");
      if (addSelectXrefId)
      {
        sql.Declare("@xrefId", "int");
        sql.AppendLine("select @xrefId = XRefId from LoanXRef where LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanIdentity.Guid));
      }
      List<DbValueList> values = new List<DbValueList>();
      foreach (PipelineInfo.Alert alert in pinfo.Alerts)
      {
        if (alert.DisplayStatus != 0)
        {
          PipelineInfo.Alert matchingAlert = Loan.findMatchingAlert(priorAlerts, alert);
          bool flag3 = true;
          if (matchingAlert != null && matchingAlert.Date.Date == alert.Date.Date)
          {
            flag3 = false;
            Loan.mergeAlertData(alert, matchingAlert);
          }
          DbValueList dbValueList = new DbValueList();
          dbValueList.Add("LoanXRefId", (object) "@xrefId", (IDbEncoder) DbEncoding.None);
          dbValueList.Add("AlertType", (object) alert.AlertID);
          if (alert.Date.Date == DateTime.MinValue)
            alert.Date = DateTime.Now;
          dbValueList.Add("AlertDate", (object) alert.Date.Date);
          dbValueList.Add("UserID", (object) alert.UserID, (IDbEncoder) DbEncoding.EmptyStringAsNull);
          dbValueList.Add("GroupID", (object) alert.GroupID, (IDbEncoder) DbEncoding.MinusOneAsNull);
          dbValueList.Add("MilestoneID", (object) alert.MilestoneID);
          dbValueList.Add("DisplayStatus", (object) alert.DisplayStatus);
          dbValueList.Add("SnoozeStartDTTM", (object) alert.SnoozeStartDTTM);
          dbValueList.Add("SnoozeDuration", (object) alert.SnoozeDuration);
          if (alert.SnoozeStartDTTM == DateTime.MinValue)
            dbValueList.Add("ActivationDTTM", (object) null);
          else
            dbValueList.Add("ActivationDTTM", (object) alert.SnoozeStartDTTM.AddMinutes((double) alert.SnoozeDuration));
          dbValueList.Add("Event", (object) (alert.Event ?? ""));
          dbValueList.Add("Status", (object) (alert.Status ?? ""));
          dbValueList.Add("UniqueID", (object) alert.AlertTargetID);
          dbValueList.Add("CurrentMilestoneID", (object) alert.CurrentMileStoneID);
          values.Add(dbValueList);
          if (flag3 && currentUser != (UserInfo) null)
            Loan.createNotificationForAlert(pinfo, alert, currentUser, Loan.getAlertConfigByID(alert.AlertID, alertConfigList));
        }
      }
      DbVersion dbVersion;
      using (DbAccessManager dbAccessManager = new DbAccessManager())
        dbVersion = dbAccessManager.GetDbVersion();
      sql.InsertInto(table, values, dbVersion);
    }

    private static AlertConfig getAlertConfigByID(int alertId, AlertConfig[] configs)
    {
      foreach (AlertConfig config in configs)
      {
        if (config.AlertID == alertId)
          return config;
      }
      return (AlertConfig) null;
    }

    private static bool createNotificationForAlert(
      PipelineInfo pinfo,
      PipelineInfo.Alert alert,
      UserInfo currentUser,
      AlertConfig alertConfig)
    {
      switch ((StandardAlertID) alert.AlertID)
      {
        case StandardAlertID.MilestoneFinished:
        case StandardAlertID.RateLockConfirm:
        case StandardAlertID.RateLockRequested:
        case StandardAlertID.RateLockDenied:
        case StandardAlertID.eFolderUpdate:
        case StandardAlertID.RateLockCancellationRequested:
        case StandardAlertID.RateLockCancelled:
        case StandardAlertID.LockVoided:
          if (Loan.isNotificationEnabled(pinfo, alert, alertConfig))
            Loan.createUserNotification(pinfo, alert, currentUser, alertConfig);
          return true;
        default:
          return false;
      }
    }

    private static bool isNotificationEnabled(
      PipelineInfo pinfo,
      PipelineInfo.Alert alert,
      AlertConfig alertConfig)
    {
      if (alertConfig == null)
        return false;
      PipelineInfo.MilestoneInfo milestoneInfo = alert.AlertID != 0 ? pinfo.GetCurrentMilestone() : pinfo.GetMilestoneByID(alert.AlertTargetID);
      return milestoneInfo != null && alertConfig.NotificationEnabled && alertConfig.MilestoneGuidList.Contains(milestoneInfo.MilestoneID);
    }

    private static void createUserNotification(
      PipelineInfo pinfo,
      PipelineInfo.Alert alert,
      UserInfo currentUser,
      AlertConfig alertConfig)
    {
      List<string> stringList = new List<string>();
      List<int> intList = new List<int>();
      if (alertConfig.Definition.NotificationType == AlertNotificationType.Configurable)
      {
        foreach (PipelineInfo.LoanAssociateInfo loanAssociate in pinfo.LoanAssociates)
        {
          if (alertConfig.NotificationRoleList.Contains(loanAssociate.RoleID))
          {
            if (loanAssociate.AssociateType == LoanAssociateType.User && !stringList.Contains(loanAssociate.UserID))
              stringList.Add(loanAssociate.UserID);
            else if (loanAssociate.AssociateType == LoanAssociateType.Group && !intList.Contains(loanAssociate.GroupID))
              intList.Add(loanAssociate.GroupID);
          }
        }
        foreach (string notificationUser in alertConfig.NotificationUserList)
        {
          if (!stringList.Contains(notificationUser))
            stringList.Add(notificationUser);
        }
      }
      else if ((alert.UserID ?? "") != "")
        stringList.Add(alert.UserID);
      else if (alert.GroupID >= 0)
      {
        intList.Add(alert.GroupID);
      }
      else
      {
        foreach (PipelineInfo.LoanAssociateInfo loanAssociate in pinfo.LoanAssociates)
        {
          if (loanAssociate.AssociateType == LoanAssociateType.User && !stringList.Contains(loanAssociate.UserID))
            stringList.Add(loanAssociate.UserID);
          else if (loanAssociate.AssociateType == LoanAssociateType.Group && !intList.Contains(loanAssociate.GroupID))
            intList.Add(loanAssociate.GroupID);
        }
      }
      foreach (int groupID in intList)
      {
        foreach (string str in AclGroupAccessor.GetUsersInGroup(groupID, true))
        {
          if (!stringList.Contains(str))
            stringList.Add(str);
        }
      }
      foreach (string recipientUserId in stringList)
      {
        if (recipientUserId != currentUser.Userid)
          Loan.createUserNotification(recipientUserId, pinfo, alert, currentUser);
      }
    }

    private static void createUserNotification(
      string recipientUserId,
      PipelineInfo pinfo,
      PipelineInfo.Alert alert,
      UserInfo currentUser)
    {
      UserNotifications.Send((UserNotification) new LoanAlertNotification(recipientUserId, pinfo, alert, currentUser));
    }

    public PipelineInfo.Alert[] GetLoanAlerts(string guid)
    {
      return AlertConfigAccessor.GetLoanAlerts(guid);
    }

    public void MergeAlertsData(
      string guid,
      PipelineInfo.Alert[] newAlerts,
      PipelineInfo.Alert[] priorAlerts)
    {
      foreach (PipelineInfo.Alert newAlert in newAlerts)
      {
        PipelineInfo.Alert matchingAlert = Loan.findMatchingAlert(priorAlerts, newAlert);
        if (matchingAlert != null)
          Loan.mergeAlertData(newAlert, matchingAlert);
      }
      AlertConfigAccessor.UpdateLoanAlerts(guid, newAlerts);
    }

    private static bool isMatching(PipelineInfo.Alert source, PipelineInfo.Alert target)
    {
      return (source.AlertTargetID ?? "") == (target.AlertTargetID ?? "") && source.AlertID == target.AlertID && (source.Event ?? "") == (target.Event ?? "") && (source.UserID ?? "") == (target.UserID ?? "") && source.GroupID == target.GroupID;
    }

    private static PipelineInfo.Alert findMatchingAlert(
      PipelineInfo.Alert[] searchSet,
      PipelineInfo.Alert alertToMatch)
    {
      foreach (PipelineInfo.Alert search in searchSet)
      {
        if ((search.AlertTargetID ?? "") == (alertToMatch.AlertTargetID ?? "") && search.AlertID == alertToMatch.AlertID && (search.Event ?? "") == (alertToMatch.Event ?? "") && (search.UserID ?? "") == (alertToMatch.UserID ?? "") && search.GroupID == alertToMatch.GroupID)
          return search;
      }
      return (PipelineInfo.Alert) null;
    }

    private static void mergeAlertData(PipelineInfo.Alert newAlert, PipelineInfo.Alert priorAlert)
    {
      if (newAlert.DisplayStatus != 0 && priorAlert.DisplayStatus != 0)
        newAlert.DisplayStatus = priorAlert.DisplayStatus;
      newAlert.SnoozeStartDTTM = priorAlert.SnoozeStartDTTM;
      newAlert.SnoozeDuration = priorAlert.SnoozeDuration;
    }

    private void moveLoanFiles(LoanIdentity newId, UserInfo currentUser)
    {
      StorageMode storageSetting = (StorageMode) ClientContext.GetCurrent().Settings.GetStorageSetting("DataStore.StorageMode");
      LoanFolder loanFolder = new LoanFolder(this.id.LoanFolder);
      LoanFolder target = new LoanFolder(newId.LoanFolder);
      if (storageSetting == StorageMode.MongoFileSystemMaster || storageSetting == StorageMode.MongoMaster || storageSetting == StorageMode.MongoOnly || storageSetting == StorageMode.PostgresOnly || storageSetting == StorageMode.BothFileSystemPostgresMaster || storageSetting == StorageMode.BothPostgresFileSystemMaster)
        this.MoveLoanToAnotherFolder(newId, target, currentUser);
      if (storageSetting != StorageMode.MongoOnly && storageSetting != StorageMode.DatabaseOnly && storageSetting != StorageMode.PostgresOnly)
      {
        loanFolder.MoveLoanFiles(this.id.LoanName, newId);
        Loan.deleteLoanFiles(this.id);
      }
      if (LoanNameFolderGenerator.GetMaxEntriesInAFolder(ClientContext.GetCurrent()) <= 0)
        return;
      LoanNameFolderGenerator.ChangeLoanCount(newId.LoanFolder, LoanNameFolderGenerator.GetLoanNameFolderPart(newId.LoanName), 1);
      LoanNameFolderGenerator.ChangeLoanCount(this.id.LoanFolder, LoanNameFolderGenerator.GetLoanNameFolderPart(this.id.LoanName), -1);
    }

    private void DeleteLoanFromDataStore(LoanIdentity identity, UserInfo currentUser)
    {
      ClientContext.GetCurrent();
      IServerSettings settings = ClientContext.GetCurrent().Settings;
      Guid result;
      Guid.TryParse(identity.Guid, out result);
      try
      {
        using (ILoanDataAccessor service = DataAccessFramework.Runtime.CreateService<ILoanDataAccessor>())
          service.DeleteLoan(result);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("Unable to delete selected loan.  Loan GUID={0}.  User={1}.  Stack={2}", (object) result, (object) currentUser.Userid, (object) ex));
        if ((bool) settings.GetStorageSetting("DataStore.CaptureErrors"))
          Loan.logDataStoreLoanError(this.LoanData, ex, settings);
        switch ((StorageMode) settings.GetStorageSetting("DataStore.StorageMode"))
        {
          case StorageMode.BothDatabaseMaster:
          case StorageMode.DatabaseOnly:
          case StorageMode.PostgresOnly:
          case StorageMode.BothPostgresFileSystemMaster:
          case StorageMode.BothFileSystemPostgresMaster:
            throw;
          default:
            if (!(bool) settings.GetStorageSetting("DataStore.VerboseErrors"))
              break;
            throw;
        }
      }
    }

    private void MoveLoanToAnotherFolder(
      LoanIdentity identity,
      LoanFolder target,
      UserInfo currentUser)
    {
      if (!target.Exists)
        Err.Raise(TraceLevel.Error, nameof (Loan), (ServerException) new ObjectNotFoundException("Target folder does not exist.", ObjectType.LoanFolder, (object) target.Name));
      IServerSettings settings = ClientContext.GetCurrent().Settings;
      Guid result;
      Guid.TryParse(identity.Guid, out result);
      try
      {
        using (ILoanDataAccessor service = DataAccessFramework.Runtime.CreateService<ILoanDataAccessor>())
          service.MoveLoanToFolder(result, identity.LoanFolder, currentUser.Userid);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("Failed to move loan to another folder.  Loan GUID={0}.  User={1}.  Stack={2}", (object) result, (object) currentUser.Userid, (object) ex));
        if ((bool) settings.GetStorageSetting("DataStore.CaptureErrors"))
          Loan.logDataStoreLoanError(this.LoanData, ex, settings);
        switch ((StorageMode) settings.GetStorageSetting("DataStore.StorageMode"))
        {
          case StorageMode.BothDatabaseMaster:
          case StorageMode.DatabaseOnly:
            throw;
          default:
            if (!(bool) settings.GetStorageSetting("DataStore.VerboseErrors"))
              break;
            throw;
        }
      }
    }

    private static void updateLoanIdentity(LoanIdentity newId)
    {
      DbValueList values = new DbValueList();
      values.Add("LoanFolder", (object) newId.LoanFolder);
      values.Add("LoanName", (object) newId.LoanName);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Update(DbAccessManager.GetTable("LoanSummary"), values, new DbValue("Guid", (object) newId.Guid));
      dbQueryBuilder.ExecuteNonQuery();
      LoanFolder.CreateLoanFolderEntryinDB(newId.LoanFolder);
    }

    private static bool updateLoanLocationTable(
      string guid,
      string sourceFolder,
      string targetFolder)
    {
      try
      {
        DbValueList values = new DbValueList();
        values.Add("SourceFolder", (object) sourceFolder);
        values.Add("TargetFolder", (object) targetFolder);
        values.Add("LastModified", (object) DateTime.Now);
        DbTableInfo table = DbAccessManager.GetTable("LoanFolderLocation");
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select SourceFolder, TargetFolder from LoanFolderLocation where Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid));
        if (dbQueryBuilder.Execute().Count == 0)
        {
          values.Add("Guid", (object) guid);
          dbQueryBuilder.InsertInto(table, values, true, false);
        }
        else
          dbQueryBuilder.Update(table, values, new DbValue("Guid", (object) guid));
        dbQueryBuilder.ExecuteNonQuery();
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public static string SyncLoanFolder(string guid, bool cleanup)
    {
      try
      {
        string str = (string) null;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        if (cleanup)
        {
          dbQueryBuilder.AppendLine("Delete from LoanFolderLocation where Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid));
          dbQueryBuilder.ExecuteNonQuery();
          string.Format("A selected loan folder location has been changed.");
          return (string) null;
        }
        LoanIdentity loanIdentity = Loan.LookupIdentity(guid);
        dbQueryBuilder.AppendLine("select SourceFolder, TargetFolder from LoanFolderLocation where Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanIdentity.Guid));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
          return (string) null;
        DataRow dataRow = dataRowCollection[0];
        if (loanIdentity.LoanFolder.Equals(dataRow["SourceFolder"].ToString()))
        {
          if (Utils.LoanFileExists(ClientContext.GetCurrent().Settings.LoansDir, dataRow["TargetFolder"].ToString(), loanIdentity.LoanName))
          {
            str = string.Format("A problem occurred when this loan was moved from the {0} folder to the {1} folder. It is currently in the {1} folder and will be opened from there. The relevant database information has been updated to prevent this warning from displaying again and to ensure this loan's folder location is consistent across your Encompass system.", (object) loanIdentity.LoanFolder, (object) dataRow["TargetFolder"].ToString());
            Loan.updateLoanIdentity(new LoanIdentity(dataRow["TargetFolder"].ToString(), loanIdentity.LoanName, loanIdentity.Guid, loanIdentity.XrefId));
          }
        }
        else if (loanIdentity.LoanFolder.Equals(dataRow["TargetFolder"].ToString()) && Utils.LoanFileExists(ClientContext.GetCurrent().Settings.LoansDir, dataRow["SourceFolder"].ToString(), loanIdentity.LoanName))
        {
          str = string.Format("A problem occurred when this loan was moved from the {1} folder to the {0} folder. It is currently in the {1} folder and will be opened from there. The relevant database information has been updated to prevent this warning from displaying again and to ensure this loan's folder location is consistent across your Encompass system.", (object) loanIdentity.LoanFolder, (object) dataRow["SourceFolder"].ToString());
          Loan.updateLoanIdentity(new LoanIdentity(dataRow["SourceFolder"].ToString(), loanIdentity.LoanName, loanIdentity.Guid, loanIdentity.XrefId));
          Loan.updateLoanLocationTable(loanIdentity.Guid, dataRow["SourceFolder"].ToString(), dataRow["SourceFolder"].ToString());
        }
        return str;
      }
      catch (Exception ex)
      {
        return (string) null;
      }
    }

    public static string GenerateUniqueName(string loanFolder, string loanName, bool Generate = false)
    {
      string loanName1 = loanName;
      int length = loanName.LastIndexOf("#");
      if (length >= 0)
        loanName = loanName.Substring(0, length);
      string str;
      for (; Loan.LookupIdentity(loanFolder, loanName1) != (LoanIdentity) null | Generate; loanName1 = loanName.Length + str.Length <= 64 ? loanName + str : loanName.Substring(0, 64 - str.Length) + str)
      {
        Generate = false;
        str = "#" + Loan.getNextLoanNumber(loanFolder, loanName).ToString();
      }
      return loanName1;
    }

    private static int getNextLoanNumber(string folderName, string loanName)
    {
      DbAccessManager.GetTable("LoanNameUniqueNumber");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Declare("@folderName", "varchar(100)");
      dbQueryBuilder.Declare("@loanName", "varchar(64)");
      dbQueryBuilder.Declare("@nextId", "int");
      dbQueryBuilder.SelectVar("@folderName", (object) folderName);
      dbQueryBuilder.SelectVar("@loanName", (object) loanName);
      dbQueryBuilder.AppendLine("select @nextId = NextNumber from LoanNameUniqueNumber where (LoanFolder = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) folderName) + ") and (LoanName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanName) + ")");
      dbQueryBuilder.AppendLine("if (@nextId is null)");
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.AppendLine("   select @nextId = 1");
      dbQueryBuilder.AppendLine("   insert into LoanNameUniqueNumber (LoanFolder, LoanName, NextNumber) values (@folderName, @loanName, @nextId + 1)");
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.AppendLine("else");
      dbQueryBuilder.AppendLine("   update LoanNameUniqueNumber set NextNumber = (NextNumber + 1) where (LoanFolder = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) folderName) + ") and (LoanName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanName) + ")");
      dbQueryBuilder.AppendLine("select @nextId as NextID");
      return (int) dbQueryBuilder.ExecuteScalar();
    }

    [PgReady]
    public static LoanIdentity LookupIdentity(string folderName, string loanName)
    {
      LoanIdentity loanIdentity = new LoanIdentity(folderName, loanName);
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.Append("select Guid, LoanName, LoanFolder, XrefID from LoanSummary where LoanFolder = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanIdentity.LoanFolder.Trim()) + " and LoanName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanIdentity.LoanName.Trim()) + ";");
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
        {
          TraceLog.WriteInfo(nameof (Loan), "LoanIdentity() - No LoanSummary entry for: Folder Name '" + loanIdentity.LoanFolder + "', Loan Name '" + loanIdentity.LoanName + "'.");
          return (LoanIdentity) null;
        }
        DataRow dataRow = dataRowCollection[0];
        return new LoanIdentity(dataRow["LoanFolder"].ToString(), dataRow["LoanName"].ToString(), dataRow["Guid"].ToString(), int.Parse(dataRow["XrefID"].ToString()));
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select Guid, LoanName, LoanFolder, XrefID,IsArchived from LoanSummary where LoanFolder = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanIdentity.LoanFolder) + " and LoanName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanIdentity.LoanName));
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      if (dataRowCollection1.Count == 0)
      {
        TraceLog.WriteInfo(nameof (Loan), "LoanIdentity() - No LoanSummary entry for: Folder Name '" + loanIdentity.LoanFolder + "', Loan Name '" + loanIdentity.LoanName + "'.");
        return (LoanIdentity) null;
      }
      DataRow dataRow1 = dataRowCollection1[0];
      return new LoanIdentity(dataRow1["LoanFolder"].ToString(), dataRow1["LoanName"].ToString(), dataRow1["Guid"].ToString(), (bool) dataRow1["IsArchived"], (int) dataRow1["XrefID"]);
    }

    public static string LookupLoanOfficerEmail(string loanOfficerId)
    {
      string empty = string.Empty;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select email from users where userid = '" + loanOfficerId + "'");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count == 0 ? (string) null : dataRowCollection[0].ItemArray[0].ToString();
    }

    [PgReady]
    public static LoanIdentity LookupIdentity(string guid)
    {
      return Loan.LookupIdentity(guid, out string _);
    }

    [PgReady]
    public static LoanIdentity LookupIdentity(string guid, out string linkLoanGuid)
    {
      LoanIdentity loanIdentity = new LoanIdentity(guid);
      linkLoanGuid = (string) null;
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select Guid, LoanName, LoanFolder, XRefID, LinkGuid from LoanSummary where Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanIdentity.Guid));
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
          return (LoanIdentity) null;
        DataRow dataRow = dataRowCollection[0];
        linkLoanGuid = dataRow["LinkGuid"].ToString();
        return new LoanIdentity(dataRow["LoanFolder"].ToString(), dataRow["LoanName"].ToString(), dataRow["Guid"].ToString(), int.Parse(dataRow["XrefId"].ToString()));
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select Guid, LoanName, LoanFolder, XRefID, LinkGuid, IsArchived from LoanSummary where Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanIdentity.Guid));
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      if (dataRowCollection1.Count == 0)
        return (LoanIdentity) null;
      DataRow dataRow1 = dataRowCollection1[0];
      linkLoanGuid = dataRow1["LinkGuid"].ToString();
      return new LoanIdentity(dataRow1["LoanFolder"].ToString(), dataRow1["LoanName"].ToString(), dataRow1["Guid"].ToString(), (bool) dataRow1["IsArchived"], (int) dataRow1["XrefId"]);
    }

    public static DraftLoanIdentity LookupDraftIdentity(string guid)
    {
      DraftLoanIdentity draftLoanIdentity = new DraftLoanIdentity(guid);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select Guid from ApplicationSummary where Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) draftLoanIdentity.Guid));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection.Count == 0)
        return (DraftLoanIdentity) null;
      DataRow dataRow = dataRowCollection[0];
      return new DraftLoanIdentity("Applications\\" + guid.Substring(1, 13).Replace("{", "").Replace("}", ""), guid, dataRow["Guid"].ToString().Trim(), -1);
    }

    internal static bool IncludeCrashedSessionLoanLocks
    {
      get
      {
        try
        {
          return (bool) ClientContext.GetCurrent().Settings.GetServerSetting("Unpublished.IncludeCrashedSessionLoanLocks");
        }
        catch
        {
          return false;
        }
      }
    }

    public static LockInfo[] GetCurrentLocks(
      bool includeCrashedSessionLocks,
      UserInfo userInfo,
      bool isExternalOrganization)
    {
      return Loan.GetCurrentLocks(includeCrashedSessionLocks, (string) null, userInfo, isExternalOrganization);
    }

    public static LockInfo[] GetCurrentLocks(
      bool includeCrashedSessionLocks,
      string folderName,
      UserInfo userInfo,
      bool isExternalOrganization)
    {
      return Loan.GetCurrentLocks(includeCrashedSessionLocks, folderName, userInfo, false, isExternalOrganization);
    }

    public static LockInfo[] GetCurrentLocks(
      bool includeCrashedSessionLocks,
      string folderName,
      UserInfo userInfo,
      bool refreshCache,
      bool isExternalOrganization)
    {
      if (refreshCache)
        ClientContext.GetCurrent()?.Cache.Remove("LoanStore");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (!string.IsNullOrWhiteSpace(folderName))
      {
        dbQueryBuilder.AppendLine("-- Create a temp table for the loan folders for filtering");
        dbQueryBuilder.AppendLine("\r\nif object_id('tempdb..#loanFolders', 'U') is not null\r\n    drop table #loanFolders\r\ncreate table #loanFolders(name varchar(250) primary key)");
        dbQueryBuilder.AppendLine(string.Format("insert into #loanFolders values ({0})", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) folderName)));
      }
      string str = "";
      if (Loan.IncludeCrashedSessionLoanLocks | includeCrashedSessionLocks)
        str = " left";
      dbQueryBuilder.AppendLine("select L.*, S.SessionID, S.ServerUri, U.first_name, U.last_name from LoanLock L" + str + " join Sessions S on L.loginSessionID = S.SessionID");
      dbQueryBuilder.AppendLine("    left join users U on L.lockedby = U.userid");
      dbQueryBuilder.AppendLine("    where (lockedfor <> " + (object) 0 + ")");
      string userVisibleIdQuery = Pipeline.GetUserVisibleIDQuery(userInfo, folderName, LoanInfo.Right.Access, isExternalOrganization);
      if (userVisibleIdQuery != "")
        dbQueryBuilder.AppendLine("   and (Guid in (" + userVisibleIdQuery + "))");
      return Loan.dataRowsToLockInfo(dbQueryBuilder.Execute(DbTransactionType.Snapshot));
    }

    public static void RemoveOrphanedLoanData()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      List<string> stringList = new List<string>((IEnumerable<string>) LoanFolder.GetAllLoanFolderNames(true));
      foreach (string stringToMatch in LoanFolder.GetAllLoanFolderNamesFromDatabase(true))
      {
        if (stringList.Find(new Predicate<string>(new StringPredicate(stringToMatch, true).Compare)) == null)
        {
          dbQueryBuilder.Reset();
          dbQueryBuilder.Append("delete from LoanSummary where LoanFolder = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) stringToMatch));
          dbQueryBuilder.ExecuteNonQuery();
        }
      }
      dbQueryBuilder.Reset();
      dbQueryBuilder.AppendLine("delete r from recent_loans r");
      dbQueryBuilder.AppendLine("where not exists (select 1 from LoanSummary l where l.guid = r.guid)");
      dbQueryBuilder.ExecuteNonQuery();
      dbQueryBuilder.Reset();
      dbQueryBuilder.AppendLine("delete loan_rights");
      dbQueryBuilder.AppendLine("from loan_rights inner join LoanAssociateUsers lam on loan_rights.Guid = lam.Guid and loan_rights.UserId = lam.UserID");
      dbQueryBuilder.AppendLine("where loan_rights.rights = 1");
      dbQueryBuilder.ExecuteNonQuery(TimeSpan.FromMinutes(10.0), DbTransactionType.None);
      string[] tableNameList = LoanXDBStore.GetTableNameList();
      if (tableNameList == null || tableNameList.Length == 0)
        return;
      for (int index = 0; index < tableNameList.Length; ++index)
      {
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("delete from " + tableNameList[index] + " where XRefID not in (select XRefID from LoanSummary)");
        try
        {
          dbQueryBuilder.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (Loan), "Error while removing orphans from table " + tableNameList[index] + ": " + (object) ex);
        }
      }
    }

    private void onBeforeRightsChanged() => this.rightsModified = true;

    private void grantFileStarterFullRights(LoanData data, LoanServerInfo info)
    {
      foreach (LoanAssociateLog allLoanAssociate in data.GetLogList().GetAllLoanAssociates())
      {
        if (allLoanAssociate.RoleID == RoleInfo.FileStarter.RoleID && allLoanAssociate.LoanAssociateID != "")
        {
          this.onBeforeRightsChanged();
          info.SetRight(allLoanAssociate.LoanAssociateID, LoanInfo.Right.FullRight);
          break;
        }
      }
    }

    private void destroyLoan(
      LoanIdentity id,
      bool deleteAllData,
      bool propogateErrors,
      UserInfo currentUser)
    {
      StorageMode storageSetting = (StorageMode) ClientContext.GetCurrent().Settings.GetStorageSetting("DataStore.StorageMode");
      try
      {
        Loan.deleteLoanFromDatabase(false, id, deleteAllData);
        Loan.deleteLoanFromXDB(id);
        this.deleteLoanFromEFolderReporting(id);
        if (LoanNameFolderGenerator.GetMaxEntriesInAFolder(ClientContext.GetCurrent()) > 0)
          LoanNameFolderGenerator.ChangeLoanCount(id.LoanFolder, LoanNameFolderGenerator.GetLoanNameFolderPart(id.LoanName), -1);
        if (storageSetting != StorageMode.FileSystemOnly)
          this.DeleteLoanFromDataStore(id, currentUser);
        if (storageSetting == StorageMode.MongoOnly || storageSetting == StorageMode.DatabaseOnly || storageSetting == StorageMode.PostgresOnly || !deleteAllData)
          return;
        Loan.deleteLoanFiles(id);
        Loan.deleteLoanFromAuditTrail(id);
      }
      catch
      {
        if (!propogateErrors)
          return;
        throw;
      }
    }

    private static void deleteLoanFromXDB(LoanIdentity id)
    {
      try
      {
        string[] tableNameList = LoanXDBStore.GetTableNameList();
        ERDBSession.DeleteLoanFromXDB(false, ClientContext.GetCurrent(), id.Guid, id.XrefId, tableNameList);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), "Error deleting loan from XDB database (" + (object) id + "): " + ex.Message);
      }
    }

    private void deleteLoanFromEFolderReporting(LoanIdentity id)
    {
      TraceLog.WriteVerbose(nameof (Loan), "Starting delete of efolder reporting fields for loan " + (object) this.id);
      DateTime now = DateTime.Now;
      DbQueryBuilder sql = new DbQueryBuilder();
      try
      {
        EnhancedConditionReportingAccessor.GetDeleteEnhancedConditionForLoanSQL(ref sql, id.XrefId);
        DocumentReportingAccessor.GetDeleteDocumentForLoanSQL(ref sql, id.XrefId);
        if (sql.Length > 0)
          sql.ExecuteNonQuery();
        TraceLog.WriteVerbose(nameof (Loan), "Delete of efolder reporting table records for loan " + (object) this.id + " completed in " + (object) (DateTime.Now - now).TotalMilliseconds + "ms.");
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), "Error deleting efolder reporting table records for loan " + (object) this.id + " : " + (object) ex);
      }
    }

    private static void deleteLoanFromAuditTrail(LoanIdentity id)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("DELETE a_t FROM AuditTrail a_t INNER JOIN LoanXRef lr ON lr.XRefID = a_t.LoanXRef WHERE lr.LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) id.Guid));
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static void deleteLoanFromDatabase(bool useERDB, LoanIdentity id, bool deleteAllData)
    {
      try
      {
        ERDBSession.DeleteLoanFromDatabase(useERDB, ClientContext.GetCurrent(), id.Guid, id.XrefId, deleteAllData);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), "Error deleting loan from database (" + (object) id + "): " + ex.Message);
      }
    }

    private static void updateDraftDatabaseRights(DraftLoanIdentity id, Hashtable newRights)
    {
      DbValue key = new DbValue("Guid", (object) id.Guid);
      DbTableInfo table1 = DbAccessManager.GetTable("loan_rights");
      DbTableInfo table2 = DbAccessManager.GetTable("users");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.DeleteFrom(table1, key);
      foreach (DictionaryEntry newRight in newRights)
      {
        DbValueList values = new DbValueList();
        values.Add("guid", (object) id.Guid);
        values.Add("userid", newRight.Key);
        values.Add("rights", (object) (int) newRight.Value);
        dbQueryBuilder.IfExists(table2, new DbValue("userid", (object) newRight.Key.ToString()));
        dbQueryBuilder.InsertInto(table1, values, true, false);
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static void updateDatabaseRights(LoanIdentity id, Hashtable newRights)
    {
      DbValue key = new DbValue("Guid", (object) id.Guid);
      DbTableInfo table1 = DbAccessManager.GetTable("loan_rights");
      DbTableInfo table2 = DbAccessManager.GetTable("users");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.DeleteFrom(table1, key);
      foreach (DictionaryEntry newRight in newRights)
      {
        DbValueList values = new DbValueList();
        values.Add("guid", (object) id.Guid);
        values.Add("userid", newRight.Key);
        values.Add("rights", (object) (int) newRight.Value);
        dbQueryBuilder.IfExists(table2, new DbValue("userid", (object) newRight.Key.ToString()));
        dbQueryBuilder.InsertInto(table1, values, true, false);
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static Hashtable dataRowsToUserRights(DataRowCollection rows)
    {
      Hashtable userRights = new Hashtable();
      foreach (DataRow row in (InternalDataCollectionBase) rows)
      {
        try
        {
          userRights.Add((object) row["userid"].ToString(), (object) int.Parse(row["rights"].ToString()));
        }
        catch
        {
          TraceLog.WriteError(nameof (Loan), "Invalid value in loan_rights table");
        }
      }
      return userRights;
    }

    private static LoanServerInfo getServerInfoFromDatabase(LoanIdentity id)
    {
      LoanServerInfo infoFromDatabase = new LoanServerInfo(id.Guid);
      DbValue key = new DbValue("Guid", (object) id.Guid);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string str = "";
      if (Loan.IncludeCrashedSessionLoanLocks)
        str = " left";
      dbQueryBuilder.AppendLine("select L.*, U.first_name, U.last_name from [LoanLock] L left join [users] U on L.lockedby = U.userid" + str + " join [Sessions] S on L.loginSessionID = S.SessionID where L.[Guid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) id.Guid));
      dbQueryBuilder.AppendLine(" Union ");
      dbQueryBuilder.AppendLine("select L.*, U.first_name, U.last_name from [LoanLock] L left join [users] U on L.lockedby = U.userid where L.[IsSessionLess] = 1 AND L.[Guid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) id.Guid));
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("loan_rights"), new string[2]
      {
        "userid",
        "rights"
      }, key);
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("LoanSummary"), new string[1]
      {
        "LastModified"
      }, key);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet.Tables.Count < 3)
        Err.Raise(TraceLevel.Error, nameof (Loan), (ServerException) new ServerDataException("Error retrieving loan server info for loan " + id.ToString()));
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
      {
        LockInfo lockInfo = Loan.dataRowToLockInfo(row, false);
        infoFromDatabase.AddLockInfo(lockInfo);
      }
      DataRowCollection rows1 = dataSet.Tables[1].Rows;
      infoFromDatabase.UserRightPairs = Loan.dataRowsToUserRights(rows1);
      DataRowCollection rows2 = dataSet.Tables[2].Rows;
      if (rows2.Count > 0)
        infoFromDatabase.LastModified = (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(rows2[0]["LastModified"], (object) DateTime.MinValue);
      infoFromDatabase.IsLockModified = false;
      return infoFromDatabase;
    }

    private static DraftLoanServerInfo getServerDraftInfoFromDatabase(DraftLoanIdentity id)
    {
      DraftLoanServerInfo infoFromDatabase = new DraftLoanServerInfo(id.Guid);
      DbValue key = new DbValue("Guid", (object) id.Guid);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string str = "";
      if (Loan.IncludeCrashedSessionLoanLocks)
        str = " left";
      dbQueryBuilder.AppendLine("select L.*, U.first_name, U.last_name from [LoanLock] L left join [users] U on L.lockedby = U.userid" + str + " join [Sessions] S on L.loginSessionID = S.SessionID where L.[Guid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) id.Guid));
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("loan_rights"), new string[2]
      {
        "userid",
        "rights"
      }, key);
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("ApplicationSummary"), new string[1]
      {
        "LastModified"
      }, key);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet.Tables.Count < 3)
        Err.Raise(TraceLevel.Error, nameof (Loan), (ServerException) new ServerDataException("Error retrieving loan server info for loan " + id.ToString()));
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
      {
        LockInfo lockInfo = Loan.dataRowToLockInfo(row, false);
        infoFromDatabase.AddLockInfo(lockInfo);
      }
      DataRowCollection rows1 = dataSet.Tables[1].Rows;
      infoFromDatabase.UserRightPairs = Loan.dataRowsToUserRights(rows1);
      DataRowCollection rows2 = dataSet.Tables[2].Rows;
      if (rows2.Count > 0)
        infoFromDatabase.LastModified = (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(rows2[0]["LastModified"], (object) DateTime.MinValue);
      infoFromDatabase.IsLockModified = false;
      return infoFromDatabase;
    }

    private static DbQueryBuilder buildReadDBLockQuery(string loanGuid)
    {
      string str = "";
      if (Loan.IncludeCrashedSessionLoanLocks)
        str = " left";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select LoanLock.*, Sessions.SessionID, Sessions.ServerUri, users.first_name, users.last_name");
      dbQueryBuilder.AppendLine("from LoanLock" + str + " join Sessions on LoanLock.loginSessionID = Sessions.SessionID");
      dbQueryBuilder.AppendLine("left outer join users on LoanLock.lockedby = users.userid");
      dbQueryBuilder.AppendLine("where LoanLock.guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanGuid));
      dbQueryBuilder.AppendLine(" Union ");
      dbQueryBuilder.AppendLine("select LoanLock.*, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) "NGSHAREDLOCK!!") + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) "NGSHAREDLOCK!!") + ",  users.first_name, users.last_name from LoanLock");
      dbQueryBuilder.AppendLine("left outer join users on LoanLock.lockedby = users.userid");
      dbQueryBuilder.AppendLine("where LoanLock.IsSessionLess = '1' AND LoanLock.guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanGuid));
      return dbQueryBuilder;
    }

    private static LockInfo readDatabaseLock(LoanIdentity id, string sessionID, bool returnAny)
    {
      DataRowCollection dataRowCollection = Loan.buildReadDBLockQuery(id.Guid).Execute();
      LockInfo lockInfo = (LockInfo) null;
      if (dataRowCollection.Count == 0)
      {
        lockInfo = new LockInfo(id.Guid);
      }
      else
      {
        bool flag = false;
        if (sessionID != null)
        {
          foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
          {
            if ((string) r["loginSessionID"] == sessionID)
            {
              lockInfo = Loan.dataRowToLockInfo(r);
              flag = true;
              break;
            }
          }
          if (!flag)
            lockInfo = !returnAny ? new LockInfo(id.Guid) : Loan.dataRowToLockInfo(dataRowCollection[0]);
        }
        else
          lockInfo = Loan.dataRowToLockInfo(dataRowCollection[0]);
      }
      return lockInfo;
    }

    private static LockInfo[] readAllDatabaseLocks(LoanIdentity id)
    {
      DataRowCollection dataRowCollection = Loan.buildReadDBLockQuery(id.Guid).Execute();
      if (dataRowCollection.Count == 0)
        return new LockInfo[0];
      LockInfo[] lockInfoArray = new LockInfo[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        lockInfoArray[index] = Loan.dataRowToLockInfo(dataRowCollection[index]);
      return lockInfoArray;
    }

    private static LockInfo[] dataRowsToLockInfo(DataRowCollection rows)
    {
      return Loan.dataRowsToLockInfo(rows, true);
    }

    private static LockInfo[] dataRowsToLockInfo(DataRowCollection rows, bool includeServerUri)
    {
      LockInfo[] lockInfo = new LockInfo[rows.Count];
      for (int index = 0; index < rows.Count; ++index)
        lockInfo[index] = Loan.dataRowToLockInfo(rows[index], includeServerUri);
      return lockInfo;
    }

    private static LockInfo dataRowToLockInfo(DataRow r) => Loan.dataRowToLockInfo(r, true);

    private static LockInfo dataRowToLockInfo(DataRow r, bool includeServerUri)
    {
      string lockedByFirstName = r["first_name"] == DBNull.Value ? "" : r["first_name"].ToString();
      string lockedByLastName = r["last_name"] == DBNull.Value ? "" : r["last_name"].ToString();
      return includeServerUri ? new LockInfo(r["guid"].ToString(), r["lockedby"].ToString(), lockedByFirstName, lockedByLastName, EllieMae.EMLite.DataAccess.SQL.DecodeString(r["loginSessionID"], (string) null), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["ServerUri"], (string) null), (LoanInfo.LockReason) r["lockedfor"], (DateTime) r["locktime"], r["SessionID"] is DBNull ? LockInfo.LockOwnerLoggedOn.False : LockInfo.LockOwnerLoggedOn.True, (LockInfo.ExclusiveLock) (byte) r["exclusive"], (bool) r["IsSessionLess"]) : new LockInfo(r["guid"].ToString(), r["lockedby"].ToString(), lockedByFirstName, lockedByLastName, EllieMae.EMLite.DataAccess.SQL.DecodeString(r["loginSessionID"], (string) null), (string) null, (LoanInfo.LockReason) r["lockedfor"], (DateTime) r["locktime"], (LockInfo.ExclusiveLock) (byte) r["exclusive"], (bool) r["IsSessionLess"]);
    }

    private static void buildWriteQuery(DbQueryBuilder sql, LockInfo lockInfo)
    {
      if (lockInfo.LockedFor == LoanInfo.LockReason.NotLocked)
      {
        if (lockInfo.LoginSessionID == "")
          sql.AppendLine("delete from LoanLock where guid = @loanguid");
        else
          sql.AppendLine("delete from LoanLock where guid = @loanguid and loginSessionID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LoginSessionID));
        sql.AppendLine("if not exists (select 1 from LoanLock where guid = @loanguid)");
        sql.AppendLine("begin");
        sql.AppendLine("    insert into LoanLock (guid, lockedby, loginSessionID, lockedfor, exclusive) values (@loanguid, '', '', " + (object) 0 + ", " + (object) 0 + ")");
        sql.AppendLine("end");
      }
      else
      {
        if (lockInfo.Exclusive == LockInfo.ExclusiveLock.Both)
          throw new Exception("Cannot set both exclusive types at the same time");
        if (ClientContext.GetCurrent().AllowConcurrentEditing && (lockInfo.LoginSessionID ?? "").Trim() == "")
          throw new Exception("Login session ID should not be null");
        if (!ClientContext.GetCurrent().AllowConcurrentEditing || lockInfo.Exclusive == LockInfo.ExclusiveLock.Nonexclusive || lockInfo.Exclusive == LockInfo.ExclusiveLock.NGSharedLock)
        {
          sql.AppendLine("delete from LoanLock where guid = @loanguid and lockedfor = 0");
          sql.AppendLine("if exists (select 1 from LoanLock where guid = @loanguid and loginSessionID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LoginSessionID) + ")");
          sql.AppendLine("   update LoanLock set lockedby = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LockedBy) + ", ");
          sql.AppendLine("       lockedfor = " + (object) (int) lockInfo.LockedFor + ", ");
          sql.AppendLine("       exclusive = " + (object) (int) lockInfo.Exclusive);
          sql.AppendLine("   where guid = @loanguid and loginSessionID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LoginSessionID));
          sql.AppendLine("else");
          sql.AppendLine("   insert into LoanLock (guid, lockedby, loginSessionID, lockedfor, exclusive, issessionless) values (");
          sql.AppendLine("       @loanguid,");
          sql.AppendLine("       " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LockedBy) + ",");
          sql.AppendLine("       " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LoginSessionID) + ",");
          sql.AppendLine("       " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) lockInfo.LockedFor) + ",");
          sql.AppendLine("       " + (object) (int) lockInfo.Exclusive + ",");
          sql.AppendLine("       " + (object) Convert.ToInt32(lockInfo.IsSessionLess) + ")");
        }
        else
        {
          string str = "";
          if (Loan.IncludeCrashedSessionLoanLocks)
            str = " left";
          if (lockInfo.Exclusive == LockInfo.ExclusiveLock.Exclusive || lockInfo.Exclusive == LockInfo.ExclusiveLock.ExclusiveA)
          {
            DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
            if (ClientContext.GetCurrent().ExclusiveLockCurrLoginsOnly)
            {
              dbQueryBuilder.AppendLine("select L.*, U.first_name, U.last_name from [LoanLock] L" + str + " join [Sessions] S on L.loginSessionID = S.SessionID");
              dbQueryBuilder.AppendLine("    left join [users] U on L.lockedby = U.userid");
              dbQueryBuilder.AppendLine("    where L.[guid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.GUID) + " and L.[lockedfor] <> " + (object) 0 + " and L.[loginSessionID] <> " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LoginSessionID));
            }
            else
            {
              dbQueryBuilder.AppendLine("select L.*, U.first_name, U.last_name from [LoanLock] L");
              dbQueryBuilder.AppendLine("    left join [users] U on L.lockedby = U.userid");
              dbQueryBuilder.AppendLine("    " + str + " join [Sessions] S on L.loginSessionID = S.SessionID");
              dbQueryBuilder.AppendLine("    where L.[guid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.GUID) + " and L.[lockedfor] <> " + (object) 0 + " and L.[loginSessionID] <> " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LoginSessionID));
            }
            DataRowCollection rows = dbQueryBuilder.Execute();
            if (rows.Count > 0)
              throw new ExclusiveLockException(Loan.dataRowsToLockInfo(rows, false));
          }
          sql.AppendLine("select @currExclusive = exclusive from [LoanLock] where guid = @loanguid and lockedfor <> 0 and loginSessionID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LoginSessionID));
          if (lockInfo.Exclusive == LockInfo.ExclusiveLock.ReleaseExclusive)
          {
            sql.AppendLine("update [LoanLock] set exclusive = @currExclusive + " + (object) (int) lockInfo.Exclusive);
            sql.AppendLine("    where guid = @loanguid");
            sql.AppendLine("    and loginSessionID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LoginSessionID));
            sql.AppendLine("    and lockedfor <> 0");
            sql.AppendLine("    and exclusive <> " + (object) 0 + " and exclusive <> " + (object) 2);
          }
          else if (lockInfo.Exclusive == LockInfo.ExclusiveLock.ReleaseExclusiveA)
          {
            sql.AppendLine("update [LoanLock] set exclusive = @currExclusive + " + (object) (int) lockInfo.Exclusive);
            sql.AppendLine("    where guid = @loanguid");
            sql.AppendLine("    and loginSessionID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LoginSessionID));
            sql.AppendLine("    and lockedfor <> 0");
            sql.AppendLine("    and exclusive <> " + (object) 0 + " and exclusive <> " + (object) 1);
          }
          else if (lockInfo.Exclusive == LockInfo.ExclusiveLock.Exclusive)
          {
            sql.AppendLine("if exists (select 1 from [LoanLock] where guid = @loanguid and loginSessionID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LoginSessionID) + ")");
            sql.AppendLine("begin");
            sql.AppendLine("    update [LoanLock] set lockedfor = " + (object) 1 + ", exclusive = @currExclusive + " + (object) (int) lockInfo.Exclusive);
            sql.AppendLine("        where guid = @loanguid");
            sql.AppendLine("        and loginSessionID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LoginSessionID));
            sql.AppendLine("        and exclusive <> " + (object) 1 + " and exclusive <> " + (object) 3);
            sql.AppendLine("end");
            sql.AppendLine("else");
            sql.AppendLine("begin");
            sql.AppendLine("    insert into LoanLock (guid, lockedby, loginSessionID, lockedfor, exclusive) values (");
            sql.AppendLine("        @loanguid,");
            sql.AppendLine("        " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LockedBy) + ",");
            sql.AppendLine("        " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LoginSessionID) + ",");
            sql.AppendLine("        " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) lockInfo.LockedFor) + ",");
            sql.AppendLine("        " + (object) (int) lockInfo.Exclusive + ")");
            sql.AppendLine("end");
          }
          else
          {
            if (lockInfo.Exclusive != LockInfo.ExclusiveLock.ExclusiveA)
              return;
            sql.AppendLine("if exists (select 1 from [LoanLock] where guid = @loanguid and loginSessionID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LoginSessionID) + ")");
            sql.AppendLine("begin");
            sql.AppendLine("    update [LoanLock] set lockedfor = " + (object) 1 + ", exclusive = @currExclusive + " + (object) (int) lockInfo.Exclusive);
            sql.AppendLine("        where guid = @loanguid");
            sql.AppendLine("        and loginSessionID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LoginSessionID));
            sql.AppendLine("        and exclusive <> " + (object) 2 + " and exclusive <> " + (object) 3);
            sql.AppendLine("end");
            sql.AppendLine("else");
            sql.AppendLine("begin");
            sql.AppendLine("    insert into LoanLock (guid, lockedby, loginSessionID, lockedfor, exclusive) values (");
            sql.AppendLine("        @loanguid,");
            sql.AppendLine("        " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LockedBy) + ",");
            sql.AppendLine("        " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LoginSessionID) + ",");
            sql.AppendLine("        " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) lockInfo.LockedFor) + ",");
            sql.AppendLine("        " + (object) (int) lockInfo.Exclusive + ")");
            sql.AppendLine("end");
          }
        }
      }
    }

    private static void writeDatabaseLock(LoanIdentity id, LoanServerInfo info)
    {
      DbQueryBuilder sql = new DbQueryBuilder();
      sql.Declare("@loanguid", "varchar(38)");
      sql.SelectVar("@loanguid", (object) id.Guid);
      LockInfo.ExclusiveLock exclusiveLock = LockInfo.ExclusiveLock.Nonexclusive;
      foreach (LockInfo currentLock in info.CurrentLocks)
      {
        if (currentLock.Exclusive == LockInfo.ExclusiveLock.NGSharedLock)
        {
          exclusiveLock = LockInfo.ExclusiveLock.NGSharedLock;
          break;
        }
      }
      if (ClientContext.GetCurrent().AllowConcurrentEditing || exclusiveLock == LockInfo.ExclusiveLock.NGSharedLock)
        sql.AppendLine("declare @currExclusive tinyint");
      if (!ClientContext.GetCurrent().AllowConcurrentEditing)
      {
        if (exclusiveLock != LockInfo.ExclusiveLock.NGSharedLock)
        {
          try
          {
            if (!info.Locked)
            {
              foreach (string sessionID in info.Changes.Distinct<string>())
              {
                LockInfo lockInfo = info.GetLockInfo(sessionID);
                Loan.buildWriteQuery(sql, lockInfo);
              }
            }
            else
            {
              string str = "";
              if (Loan.IncludeCrashedSessionLoanLocks)
                str = " left";
              LockInfo lockInfo = !info.Locked ? new LockInfo(info.LoanGUID) : info.GetLockInfo(info.Changes[0]);
              sql.AppendLine("if exists (select 1 from LoanLock" + str + " join Sessions on LoanLock.loginSessionID = Sessions.SessionID where (guid = @loanguid) and (lockedfor <> 0) and (lockedby <> " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LockedBy) + ") and (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) lockInfo.LockedFor) + " > 0))");
              sql.AppendLine("begin");
              sql.AppendLine("    raiserror('Loan is currently locked by another user', 16, 1)");
              sql.AppendLine("end");
              sql.AppendLine("else if not exists (select 1 from LoanLock" + str + " join Sessions on LoanLock.loginSessionID = Sessions.SessionID where (guid = @loanguid) and (lockedfor = " + (object) (int) lockInfo.LockedFor + ") and (lockedby = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LockedBy) + "))");
              sql.AppendLine("begin");
              sql.AppendLine("    delete from LoanLock where guid = @loanguid");
              sql.AppendLine("    insert into LoanLock (guid, lockedby, loginSessionID, lockedfor, exclusive) values (");
              sql.AppendLine("        @loanguid,");
              sql.AppendLine("        " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LockedBy) + ",");
              sql.AppendLine("        " + EllieMae.EMLite.DataAccess.SQL.Encode((object) lockInfo.LoginSessionID) + ",");
              sql.AppendLine("        " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (int) lockInfo.LockedFor) + ",");
              sql.AppendLine("        " + (object) 1 + ")");
              sql.AppendLine("end");
            }
            sql.ExecuteNonQuery();
            return;
          }
          catch (ServerDataException ex)
          {
            if (ex.Message.IndexOf("is currently locked") > 0)
            {
              Err.Raise(TraceLevel.Info, nameof (Loan), (ServerException) new LockException(Loan.readDatabaseLock(id, (string) null, true)));
              return;
            }
            Err.Reraise(nameof (Loan), (Exception) ex);
            return;
          }
        }
      }
      foreach (string sessionID in info.Changes.Distinct<string>())
      {
        LockInfo lockInfo = info.GetLockInfo(sessionID);
        Loan.buildWriteQuery(sql, lockInfo);
      }
      sql.ExecuteNonQuery();
    }

    private static void serializeDraftLoanData(
      DraftLoanIdentity id,
      LoanData data,
      bool allowOverwrite,
      UserInfo currentUser,
      string sessionId,
      ILoanSettings loanSettings = null)
    {
      StorageMode storageSetting = (StorageMode) ClientContext.GetCurrent().Settings.GetStorageSetting("DataStore.StorageMode");
      Exception exception1 = (Exception) null;
      try
      {
        if (storageSetting != StorageMode.FileSystemOnly)
          Loan.serializeDraftLoanDataToDataStore(data, currentUser, id, sessionId);
      }
      catch (Exception ex)
      {
        exception1 = ex;
      }
      Exception exception2 = (Exception) null;
      try
      {
        if (storageSetting != StorageMode.DatabaseOnly)
        {
          if (storageSetting != StorageMode.MongoOnly)
          {
            if (storageSetting != StorageMode.PostgresOnly)
              new DraftLoanFolder(ServerUtil.Truncate(id.Guid.ToString(), 1, 13)).WriteLoanData(id.LoanName, data, allowOverwrite, loanSettings);
          }
        }
      }
      catch (Exception ex)
      {
        exception2 = ex;
      }
      if (exception1 != null || exception2 != null)
        throw new Exception(exception1.ToString() + "\n" + (object) exception2);
    }

    private static void serializeLoanData(
      LoanIdentity id,
      LoanData data,
      bool allowOverwrite,
      UserInfo currentUser,
      string sessionId,
      ILoanSettings loanSettings = null)
    {
      StorageMode storageSetting = (StorageMode) ClientContext.GetCurrent().Settings.GetStorageSetting("DataStore.StorageMode");
      Exception exception1 = (Exception) null;
      try
      {
        if (storageSetting != StorageMode.FileSystemOnly)
          Loan.serializeLoanDataToDataStore(data, currentUser, id, sessionId);
      }
      catch (Exception ex)
      {
        exception1 = ex;
      }
      Exception exception2 = (Exception) null;
      try
      {
        if (storageSetting != StorageMode.DatabaseOnly)
        {
          if (storageSetting != StorageMode.MongoOnly)
          {
            if (storageSetting != StorageMode.PostgresOnly)
            {
              if (data.IsDraft)
                new DraftLoanFolder(ServerUtil.Truncate(id.Guid.ToString(), 1, 13)).WriteLoanData(id.LoanName, data, allowOverwrite, loanSettings);
              else
                new LoanFolder(id.LoanFolder).WriteLoanData(id.LoanName, data, allowOverwrite, loanSettings);
            }
          }
        }
      }
      catch (Exception ex)
      {
        exception2 = ex;
      }
      if (exception1 != null || exception2 != null)
        throw new Exception(exception1.ToString() + "\n" + (object) exception2);
    }

    private static void serializeDraftLoanDataToDataStore(
      LoanData data,
      UserInfo currentUser,
      DraftLoanIdentity id,
      string sessionId)
    {
      IServerSettings settings = ClientContext.GetCurrent().Settings;
      try
      {
        string xml = data.ToXml(true);
        using (ILoanDataAccessor service = DataAccessFramework.Runtime.CreateService<ILoanDataAccessor>())
          service.SaveLoan(xml, currentUser, id.LoanFolder, id.LoanName, LoanFileFormatType.Encompass360);
      }
      catch (Exception ex)
      {
        StorageMode storageSetting = (StorageMode) settings.GetStorageSetting("DataStore.StorageMode");
        string dataStoreName = "Active-Store";
        if (storageSetting == StorageMode.PostgresOnly || storageSetting != StorageMode.BothPostgresFileSystemMaster && storageSetting != StorageMode.BothFileSystemPostgresMaster)
          dataStoreName = "Postgres";
        Loan.DataStoreErrorLog(dataStoreName, currentUser.Userid, sessionId, data, ex, settings);
      }
    }

    private static void serializeLoanDataToDataStore(
      LoanData data,
      UserInfo currentUser,
      LoanIdentity id,
      string sessionId)
    {
      IServerSettings settings = ClientContext.GetCurrent().Settings;
      try
      {
        string xml = data.ToXml(true);
        using (ILoanDataAccessor service = DataAccessFramework.Runtime.CreateService<ILoanDataAccessor>())
          service.SaveLoan(xml, currentUser, id.LoanFolder, id.LoanName, LoanFileFormatType.Encompass360);
      }
      catch (Exception ex)
      {
        StorageMode storageSetting = (StorageMode) settings.GetStorageSetting("DataStore.StorageMode");
        string dataStoreName = "Active-Store";
        if (storageSetting == StorageMode.PostgresOnly || storageSetting != StorageMode.BothPostgresFileSystemMaster && storageSetting != StorageMode.BothFileSystemPostgresMaster)
          dataStoreName = "Postgres";
        Loan.DataStoreErrorLog(dataStoreName, currentUser.Userid, sessionId, data, ex, settings);
      }
    }

    private static void DataStoreErrorLog(
      string dataStoreName,
      string userId,
      string sessionId,
      LoanData data,
      Exception ex,
      IServerSettings settings)
    {
      TraceLog.WriteError(nameof (Loan), string.Format("Failed to write loan to database.  DataStore={0}.  Loan GUID={1}.  User={2}.  Session={3}.  Stack={4}", (object) dataStoreName, (object) data.GUID, (object) userId, (object) sessionId, (object) ex));
      if ((bool) settings.GetStorageSetting("DataStore.CaptureErrors"))
        Loan.logDataStoreLoanError(data, ex, settings);
      switch ((StorageMode) settings.GetStorageSetting("DataStore.StorageMode"))
      {
        case StorageMode.BothDatabaseMaster:
        case StorageMode.DatabaseOnly:
        case StorageMode.MongoOnly:
        case StorageMode.MongoMaster:
        case StorageMode.PostgresOnly:
        case StorageMode.BothFileSystemPostgresMaster:
          throw ex;
        default:
          if (!(bool) settings.GetStorageSetting("DataStore.VerboseErrors"))
            break;
          throw ex;
      }
    }

    private static void logDataStoreLoanError(
      LoanData data,
      Exception err,
      IServerSettings settings)
    {
      try
      {
        string logFolderPath = settings.GetLogFolderPath("SqlLoanErrors");
        string path = Path.Combine(logFolderPath, data.GUID + ".em");
        new LoanDataFormatter().Serialize(data, true).Write(path);
        File.WriteAllText(Path.Combine(logFolderPath, data.GUID + ".err"), err.ToString());
        TraceLog.WriteInfo(nameof (Loan), "Wrote failed loan " + data.GUID + " to SqlLoanErrors folder");
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), "Failed to write failed loan to SqlLoanErrors folder: " + (object) ex);
      }
    }

    private LoanData deserializeLoanDataFromDataStore(string loanId)
    {
      IServerSettings settings = ClientContext.GetCurrent().Settings;
      try
      {
        string xmlStr = "";
        using (ILoanDataAccessor service = DataAccessFramework.Runtime.CreateService<ILoanDataAccessor>())
          xmlStr = service.GetLoan(new Guid(loanId), LoanFileFormatType.Encompass360);
        return !string.IsNullOrWhiteSpace(xmlStr) ? new LoanDataFormatter().Deserialize(xmlStr) : (LoanData) null;
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("Failed to read loan from database.  Loan GUID={0}.   Stack={1}", (object) loanId, (object) ex));
        throw;
      }
    }

    public LoanData InternalDeserializeLoanData(
      LoanIdentity id,
      ILoanSettings loanSettings = null,
      bool onlySavedData = false)
    {
      return this.deserializeLoanData(id, -999, loanSettings, onlySavedData);
    }

    public LoanData InternalDeserializeDraftLoanData(
      DraftLoanIdentity id,
      ILoanSettings loanSettings = null)
    {
      return this.deserializeDraftLoanData(id, loanSettings);
    }

    private LoanData deserializeDraftApplicationLoanData(
      DraftLoanIdentity id,
      ILoanSettings loanSettings = null)
    {
      StorageMode storageSetting = (StorageMode) ClientContext.GetCurrent().Settings.GetStorageSetting("DataStore.StorageMode");
      LoanData data = storageSetting == StorageMode.BothDatabaseMaster || storageSetting == StorageMode.DatabaseOnly || storageSetting == StorageMode.MongoMaster || storageSetting == StorageMode.MongoOnly ? this.deserializeLoanDataFromDataStore(id.Guid) ?? new DraftLoanFolder(id.LoanFolder).ReadLoanData(id.LoanName, loanSettings, id.LoanFolder) : new DraftLoanFolder(id.LoanFolder).ReadLoanData(id.LoanName, loanSettings, id.LoanFolder);
      if (data == null)
        return (LoanData) null;
      if (data.GUID != id.Guid)
        data.GUID = id.Guid;
      LogList logList = data.GetLogList();
      bool flag = false;
      DocumentLog[] allDocuments = logList.GetAllDocuments();
      Dictionary<string, DocumentLog> dictionary = new Dictionary<string, DocumentLog>();
      foreach (DocumentLog documentLog in allDocuments)
      {
        if (!documentLog.Files.IsMigrated)
        {
          flag = true;
          dictionary.Add(documentLog.Guid, documentLog);
        }
      }
      if (flag)
      {
        Tracing.Log(Loan.sw, nameof (Loan), TraceLevel.Info, "Migrating files");
        foreach (FileAttachment fileAttachment in FileAttachmentStore.GetFileAttachments(this, true))
        {
          if (!string.IsNullOrEmpty(fileAttachment.DocumentID) && dictionary.ContainsKey(fileAttachment.DocumentID))
            dictionary[fileAttachment.DocumentID].Files.Import(fileAttachment.ID, fileAttachment.IsActive);
        }
        foreach (KeyValuePair<string, DocumentLog> keyValuePair in dictionary)
          keyValuePair.Value.Files.IsMigrated = true;
      }
      if (logList.GetNumberOfMilestones() == 0)
        Loan.PrepareSystemSpecificInformation(WorkflowBpmDbAccessor.GetMilestones(true).ToList<EllieMae.EMLite.Workflow.Milestone>(), data);
      else
        data.FixMilestoneIDs(WorkflowBpmDbAccessor.GetCompleteMilestoneNameToGUID());
      if (storageSetting != StorageMode.BothDatabaseMaster && storageSetting != StorageMode.DatabaseOnly)
      {
        LoanEventLogList loanEventLogList = new DraftLoanFolder(id.LoanFolder).ReadLoanEventLog(id.LoanName);
        if (loanEventLogList != null)
        {
          foreach (LogRecordBase allNonSystemLog in loanEventLogList.GetAllNonSystemLogs())
          {
            data.MergeRequired = true;
            data.GetLogList().AddRecord(allNonSystemLog, false);
          }
        }
      }
      return data;
    }

    private LoanData deserializeLoanData(
      LoanIdentity id,
      int version,
      ILoanSettings loanSettings = null,
      bool onlySavedData = false)
    {
      try
      {
        StorageMode storageSetting = (StorageMode) ClientContext.GetCurrent().Settings.GetStorageSetting("DataStore.StorageMode");
        LoanData data = storageSetting == StorageMode.BothDatabaseMaster || storageSetting == StorageMode.DatabaseOnly || storageSetting == StorageMode.MongoMaster || storageSetting == StorageMode.MongoOnly || storageSetting == StorageMode.PostgresOnly || storageSetting == StorageMode.BothFileSystemPostgresMaster ? this.deserializeLoanDataFromDataStore(id.Guid) ?? new LoanFolder(id.LoanFolder).ReadLoanData(id.LoanName, loanSettings, onlySavedData, id.LoanFolder) : (version != -999 ? new LoanFolder(id.LoanFolder).ReadLoanDataOfVersion(id.LoanName, version, loanSettings, true, id.LoanFolder) : new LoanFolder(id.LoanFolder).ReadLoanData(id.LoanName, loanSettings, onlySavedData, id.LoanFolder));
        if (data == null)
          return (LoanData) null;
        if (data.GUID != id.Guid)
          data.GUID = id.Guid;
        if (version != -999)
          return data;
        if (data.GetLogList().GetNumberOfMilestones() == 0)
          Loan.PrepareSystemSpecificInformation(WorkflowBpmDbAccessor.GetMilestones(true).ToList<EllieMae.EMLite.Workflow.Milestone>(), data);
        else
          data.FixMilestoneIDs(WorkflowBpmDbAccessor.GetCompleteMilestoneNameToGUID());
        if (storageSetting != StorageMode.BothDatabaseMaster && storageSetting != StorageMode.DatabaseOnly)
        {
          LoanEventLogList loanEventLogList = new LoanFolder(id.LoanFolder).ReadLoanEventLog(id);
          if (loanEventLogList != null)
          {
            foreach (LogRecordBase allNonSystemLog in loanEventLogList.GetAllNonSystemLogs())
            {
              data.MergeRequired = true;
              data.GetLogList().AddRecord(allNonSystemLog, false);
            }
          }
        }
        return data;
      }
      catch (ServerException ex)
      {
        if (ex.InnerException != null && ex.InnerException is XmlException)
          Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("Error deserializing loan data, GUID = " + id.Guid, ex.InnerException));
        else
          Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("Error deserializing loan data", (Exception) ex));
        return (LoanData) null;
      }
      catch (XmlException ex)
      {
        Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("Error deserializing loan data, GUID = " + id.Guid, (Exception) ex));
        return (LoanData) null;
      }
    }

    private LoanData deserializeDraftLoanData(DraftLoanIdentity id, ILoanSettings loanSettings = null)
    {
      try
      {
        ClientContext current = ClientContext.GetCurrent();
        StorageMode storageSetting = (StorageMode) current.Settings.GetStorageSetting("DataStore.StorageMode");
        LoanData data = storageSetting == StorageMode.BothDatabaseMaster || storageSetting == StorageMode.DatabaseOnly || storageSetting == StorageMode.MongoMaster || storageSetting == StorageMode.MongoOnly ? this.deserializeLoanDataFromDataStore(id.Guid) ?? new DraftLoanFolder(id.LoanFolder).ReadLoanData(id.LoanName, loanSettings, id.LoanFolder) : new DraftLoanFolder(id.LoanFolder).ReadLoanData(id.LoanName, loanSettings, id.LoanFolder);
        if (data == null)
          return (LoanData) null;
        if (data.GUID != id.Guid)
          data.GUID = id.Guid;
        LogList logList = data.GetLogList();
        bool flag = false;
        DocumentLog[] allDocuments = logList.GetAllDocuments();
        Dictionary<string, DocumentLog> dictionary = new Dictionary<string, DocumentLog>();
        foreach (DocumentLog rec in allDocuments)
        {
          if (!rec.Files.IsMigrated)
          {
            flag = true;
            if (!dictionary.ContainsKey(rec.Guid))
            {
              dictionary.Add(rec.Guid, rec);
            }
            else
            {
              logList.RemoveRecord((LogRecordBase) rec, true);
              XmlElement element = new XmlDocument().CreateElement("DeletedDocumentLog");
              rec.ToXml(element);
              TraceLog.WriteWarning(nameof (Loan), string.Format("Removing duplicate document log: {0}", (object) element.InnerXml));
            }
          }
        }
        if (flag)
        {
          Tracing.Log(Loan.sw, nameof (Loan), TraceLevel.Info, "Migrating files");
          foreach (FileAttachment fileAttachment in FileAttachmentStore.GetFileAttachments(this, true, (IAttachmentXmlProviderFactory) new AttachmentXmlProviderFactory(current.Settings)))
          {
            if (!string.IsNullOrEmpty(fileAttachment.DocumentID) && dictionary.ContainsKey(fileAttachment.DocumentID))
              dictionary[fileAttachment.DocumentID].Files.Import(fileAttachment.ID, fileAttachment.IsActive);
          }
          foreach (KeyValuePair<string, DocumentLog> keyValuePair in dictionary)
            keyValuePair.Value.Files.IsMigrated = true;
        }
        if (logList.GetNumberOfMilestones() == 0)
          Loan.PrepareSystemSpecificInformation(WorkflowBpmDbAccessor.GetMilestones(true).ToList<EllieMae.EMLite.Workflow.Milestone>(), data);
        else
          data.FixMilestoneIDs(WorkflowBpmDbAccessor.GetCompleteMilestoneNameToGUID());
        return data;
      }
      catch (ServerException ex)
      {
        if (ex.InnerException != null && ex.InnerException is XmlException)
          Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("Error deserializing loan data, GUID = " + id.Guid, ex.InnerException));
        else
          Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("Error deserializing loan data", (Exception) ex));
        return (LoanData) null;
      }
      catch (XmlException ex)
      {
        Err.Raise(TraceLevel.Error, nameof (Loan), new ServerException("Error deserializing loan data, GUID = " + id.Guid, (Exception) ex));
        return (LoanData) null;
      }
    }

    public static EllieMae.EMLite.ClientServer.LoanAssociateInfo[] GetLoanAssociates(
      string guid,
      bool milestoneRolesOnly,
      bool resolveUsers)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (resolveUsers)
        dbQueryBuilder.AppendLine("select * from LoanAssociateUserDetails where Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid));
      else
        dbQueryBuilder.AppendLine("select * from LoanAssociateDetails where Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid));
      if (milestoneRolesOnly)
        dbQueryBuilder.AppendLine("    and IsNull(MilestoneID, '') <> ''");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      EllieMae.EMLite.ClientServer.LoanAssociateInfo[] loanAssociates = new EllieMae.EMLite.ClientServer.LoanAssociateInfo[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
      {
        DataRow dataRow = dataRowCollection[index];
        loanAssociates[index] = new EllieMae.EMLite.ClientServer.LoanAssociateInfo(string.Concat(dataRow["AssociateGuid"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Guid"]), EllieMae.EMLite.DataAccess.SQL.DecodeEnum<LoanAssociateType>(dataRow["AssociateType"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["MilestoneID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["MilestoneName"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["Order"], -1), EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["RoleID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["RoleName"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["RoleAbbr"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["AllowWrites"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["UserID"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["GroupID"], -1), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["FullName"], ""), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["First_Name"], ""), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Last_Name"], ""), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Email"], ""), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Phone"], ""));
      }
      return loanAssociates;
    }

    private static void PrepareSystemSpecificInformation(List<EllieMae.EMLite.Workflow.Milestone> msList, LoanData data)
    {
      LogList logList = data.GetLogList();
      foreach (EllieMae.EMLite.Workflow.Milestone ms in msList)
      {
        logList.AddMilestone(ms.Name, ms.DefaultDays, -1, ms.TPOConnectStatus, ms.ConsumerStatus);
        logList.GetMilestone(ms.Name).MilestoneID = ms.MilestoneID;
      }
      MilestoneLog milestone = logList.GetMilestone("Started");
      milestone.Done = true;
      milestone.Date = DateTime.Now.Date;
      data.Dirty = true;
      data.WriteXml(Stream.Null, data.ContentAccess, true, true);
      data.Dirty = false;
    }

    private static void deleteLoanFiles(LoanIdentity id)
    {
      try
      {
        new LoanFolder(id.LoanFolder).DeleteLoanFiles(id.LoanName);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), "Delete loan from filesystem failed for loan Path: " + new LoanFolder(id.LoanFolder).GetFullLoanFilePath(id.LoanName) + ", Loan Name: " + id.LoanName + ", exception - " + ex.Message);
      }
    }

    public static void ReplaceSessionID(string prevSessionID, string newSessionID)
    {
      if (string.IsNullOrWhiteSpace(prevSessionID))
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update [LoanLock] set [loginSessionID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) newSessionID) + " where [loginSessionID] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) prevSessionID));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static bool AddWebCenterImportID(
      string importID,
      string emSiteID,
      string loanGUID,
      DateTime importDateTime,
      string whoImports)
    {
      if (importID == string.Empty)
        return false;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("DELETE FROM TPOImportIDs");
        dbQueryBuilder.AppendLine("WHERE LoanImportId = '" + importID + "'");
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        TraceLog.WriteInfo(nameof (Loan), "AddWebCenterImportID: Cannot delete TPO WenCenter import date/time record '" + importID + "' from table 'TPOImportIDs'. Error: " + ex.Message);
      }
      dbQueryBuilder.Reset();
      try
      {
        dbQueryBuilder.AppendLine("INSERT INTO TPOImportIDs (LoanImportId, EMSiteId, ImportTime, LoanGUID, WhoImports) values ('" + importID + "', '" + emSiteID + "', '" + importDateTime.ToString("MM/dd/yyyy hh:mm:ss tt") + "', '" + loanGUID + "', '" + whoImports + "')");
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        TraceLog.WriteInfo(nameof (Loan), "AddWebCenterImportID: Cannot insert a new TPO WenCenter import date/time record '" + importID + "' to table 'TPOImportIDs'. Error: " + ex.Message);
      }
      return true;
    }

    public static WebCenterImpotStatus GetWebCenterImportStatus(string loanGUID)
    {
      if (loanGUID == string.Empty)
        return (WebCenterImpotStatus) null;
      WebCenterImpotStatus centerImportStatus = new WebCenterImpotStatus();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("SELECT * FROM TPOImportIDs");
        dbQueryBuilder.AppendLine("WHERE LoanGUID = '" + loanGUID + "'");
        IEnumerator enumerator = dbQueryBuilder.Execute().GetEnumerator();
        try
        {
          if (enumerator.MoveNext())
          {
            DataRow current = (DataRow) enumerator.Current;
            centerImportStatus.ImportID = Convert.ToString(current["LoanImportId"]);
            centerImportStatus.EMSiteID = Convert.ToString(current["EMSiteId"]);
            centerImportStatus.ImportDateTime = Convert.ToDateTime(current["ImportTime"]);
            centerImportStatus.LoanGUID = Convert.ToString(current["LoanGUID"]);
            centerImportStatus.WhoImports = Convert.ToString(current["WhoImports"]);
          }
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
        return centerImportStatus;
      }
      catch (Exception ex)
      {
        TraceLog.WriteInfo(nameof (Loan), "GetWebCenterImportStatus: Cannot read TPO WenCenter import date/time record '" + loanGUID + "' from table 'TPOImportIDs'. Error: " + ex.Message);
      }
      return (WebCenterImpotStatus) null;
    }

    public static void DeleteWebCenterImportID(string importID)
    {
      if (importID == string.Empty)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("DELETE FROM TPOImportIDs");
        dbQueryBuilder.AppendLine("WHERE LoanImportId = '" + importID + "'");
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        TraceLog.WriteInfo(nameof (Loan), "DeleteWebCenterImportID: Cannot delete TPO WenCenter import date/time record '" + importID + "' from table 'TPOImportIDs'. Error: " + ex.Message);
      }
    }

    public static PipelineInfo[] GetLoansByLoanNumbers(List<string> loanNumbers)
    {
      ArrayList arrayList = new ArrayList();
      string str = "'" + string.Join("', '", loanNumbers.ToArray()) + "'";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select ls.Guid, ls.LoanNumber, LoanStatus, CurrentMilestoneName, TotalLoanAmount, BorrowerFirstName, BorrowerLastName, LoanRate, LoanFolder, TradeNumber, InvestorStatus, Investor, CorrespondentTradeNumber, ls.TPOCompanyID, RateIsLocked, loanChannel, WithdrawnDate, LoanSummaryExtension.VoidedDate, LockStatus");
      dbQueryBuilder.AppendLine("from LoanSummary ls left outer join LoanSummaryExtension on ls.guid = LoanSummaryExtension.guid");
      dbQueryBuilder.AppendLine("where ls.LoanNumber in (" + str + ")");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet != null && dataSet.Tables.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
        {
          Hashtable info = new Hashtable();
          foreach (DataColumn column in (InternalDataCollectionBase) row.Table.Columns)
          {
            string criterionName = QueryEngine.ColumnNameToCriterionName(column.ColumnName);
            info[(object) criterionName] = EllieMae.EMLite.DataAccess.SQL.Decode(row[column.ColumnName]);
          }
          arrayList.Add((object) new PipelineInfo(info, (PipelineInfo.Borrower[]) null, (PipelineInfo.Alert[]) null, (PipelineInfo.LoanAssociateInfo[]) null, (LockInfo) null, (Hashtable) null, (PipelineInfo.MilestoneInfo[]) null, (PipelineInfo.TradeInfo) null, new PipelineInfo.TradeInfo[0], new string[0]));
        }
      }
      return (PipelineInfo[]) arrayList.ToArray(typeof (PipelineInfo));
    }

    public static DateTime GetLastModifiedFromDB(string loanGuid)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select [LastModified] from [LoanSummary] where [Guid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanGuid));
        return (DateTime) dbQueryBuilder.ExecuteScalar();
      }
      catch (Exception ex)
      {
        throw new Exception("Unable to get loan LastModified from DB for loan '" + loanGuid + "': " + ex.Message);
      }
    }

    public void HandleDeferrableProcessAsSubscriber(IDeferrableTransactionContext transactionContext)
    {
      this.exists = true;
      using (DeferrableLoanTransaction lockObj = new DeferrableLoanTransaction(transactionContext))
      {
        string str = Guid.NewGuid().ToString();
        using (this.innerLock = (ICacheLock<bool?>) new CacheLock<bool?>(str, (object) str, new bool?(), (IDisposable) lockObj, (ICacheStore) null, false))
        {
          lockObj.Initialize((PerformanceMeter) null);
          lockObj.Complete(new DeferrableType?(DeferrableType.RealTime));
        }
      }
    }

    public static LoanServerInfo InternalGetServerInfoFromDatabase(LoanIdentity id)
    {
      return Loan.getServerInfoFromDatabase(id);
    }

    public void InternalPrepareDeferredInstance()
    {
      this.exists = true;
      string str = Guid.NewGuid().ToString();
      this.innerLock = (ICacheLock<bool?>) new CacheLock<bool?>(str, (object) str, new bool?(), (IDisposable) null, (ICacheStore) null, false);
    }

    private static int CalculateRESPAFields(PipelineInfo pinfo, LoanData data)
    {
      int respaFields = 0;
      if (!string.IsNullOrEmpty(data.GetField("4000").ToString()) && !string.IsNullOrEmpty(data.GetField("4002").ToString()))
        ++respaFields;
      if (!string.IsNullOrEmpty(pinfo.Info[(object) "TotalMonthlyIncome"].ToString()))
        ++respaFields;
      if (!string.IsNullOrEmpty(data.GetField("65").ToString()))
        ++respaFields;
      if (!string.IsNullOrEmpty(pinfo.Info[(object) "Address1"].ToString()) && !string.IsNullOrEmpty(pinfo.Info[(object) "City"].ToString()) && !string.IsNullOrEmpty(pinfo.Info[(object) "State"].ToString()) && !string.IsNullOrEmpty(pinfo.Info[(object) "Zip"].ToString()))
        ++respaFields;
      if (!string.IsNullOrEmpty(pinfo.Info[(object) "EstimatedValue"].ToString()))
        ++respaFields;
      if (!string.IsNullOrEmpty(pinfo.Info[(object) "LoanAmount"].ToString()))
        ++respaFields;
      return respaFields;
    }

    public static void CreateLoanErrorInfo(
      DbValueList loanErrorInfoValues,
      bool useCustomColForBusinessRuleData = false)
    {
      DbTableInfo table = DbAccessManager.GetTable("LoanErrorInfo");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (!loanErrorInfoValues.Contains("Id"))
        loanErrorInfoValues.Add("Id", (object) Guid.NewGuid().ToString("B"));
      if (!loanErrorInfoValues.Contains("CreatedDate"))
        loanErrorInfoValues.Add("CreatedDate", (object) DateTime.UtcNow);
      if (useCustomColForBusinessRuleData && loanErrorInfoValues.Contains("BusinessRule"))
      {
        int index1 = -1;
        int index2 = -1;
        int num = 0;
        for (int index3 = 0; index3 < loanErrorInfoValues.Count; ++index3)
        {
          if (loanErrorInfoValues[index3].ColumnName == "BusinessRule")
          {
            index1 = index3;
            ++num;
          }
          if (loanErrorInfoValues[index3].ColumnName == "Custom")
          {
            index2 = index3;
            ++num;
          }
          if (num == 2)
            break;
        }
        string str1 = "BusinessRule|" + loanErrorInfoValues[index1].Encode(table["BusinessRule"]);
        string str2 = index2 != -1 ? loanErrorInfoValues[index2].Encode(table["Custom"]) : (string) null;
        if (str2 != null && str2.ToUpper() != "NULL")
          str1 = str1 + "::Custom|" + str2;
        if (loanErrorInfoValues.Contains("Custom"))
          loanErrorInfoValues.Remove("Custom");
        loanErrorInfoValues.Add("Custom", (object) str1);
        loanErrorInfoValues.Remove("BusinessRule");
      }
      dbQueryBuilder.InsertInto(table, loanErrorInfoValues, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static DataRowCollection GetLoanErrorInfo(
      string userId,
      string loanId,
      DateTime? start,
      DateTime? end,
      string firstName,
      string lastName,
      string filters = "�",
      bool adjustDate = false)
    {
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      string companySetting = Company.GetCompanySetting("migration", "AccessibleLoanOwnerOrgs");
      dbQueryBuilder1.AppendLine("Select * from LoanErrorInfo as lei");
      dbQueryBuilder1.AppendLine("where (lei.LoanGuid is NULL) or lei.LoanGuid in (Select Guid from " + (string.IsNullOrWhiteSpace(companySetting) ? "FN_GetUsersAccessibleLoans" : "FN_GetUsersAccessibleLoans_org") + "('" + userId + "', NULL))");
      if (!string.IsNullOrWhiteSpace(loanId))
      {
        if (loanId.IndexOf("{", StringComparison.Ordinal) < 0)
          loanId = "{" + loanId + "}";
        dbQueryBuilder1.AppendLine("and lei.LoanGuid = '" + loanId + "'");
      }
      if (!string.IsNullOrWhiteSpace(firstName))
        dbQueryBuilder1.AppendLine("and lei.BorrowerFirstName = '" + firstName + "'");
      if (!string.IsNullOrWhiteSpace(lastName))
        dbQueryBuilder1.AppendLine("and lei.BorrowerLastName = '" + lastName + "'");
      DateTime date;
      if (start.HasValue)
      {
        DbQueryBuilder dbQueryBuilder2 = dbQueryBuilder1;
        date = start.Value;
        date = date.Date;
        string text = "and lei.CreatedDate >= '" + date.ToString("yyyy-MM-dd") + "'";
        dbQueryBuilder2.AppendLine(text);
      }
      if (end.HasValue)
      {
        if (adjustDate)
        {
          ref DateTime? local = ref end;
          date = end.Value;
          DateTime dateTime = date.AddDays(1.0);
          local = new DateTime?(dateTime);
        }
        DbQueryBuilder dbQueryBuilder3 = dbQueryBuilder1;
        date = end.Value;
        date = date.Date;
        string text = "and lei.CreatedDate < '" + date.ToString("yyyy-MM-dd") + "'";
        dbQueryBuilder3.AppendLine(text);
      }
      if (string.IsNullOrWhiteSpace(filters))
      {
        dbQueryBuilder1.AppendLine("Order by CreatedDate desc");
      }
      else
      {
        bool flag = false;
        filters = filters.Trim().ToLower();
        if (filters.StartsWith("-"))
        {
          flag = true;
          filters = filters.Replace("-", "");
        }
        if (filters == "createddate")
          dbQueryBuilder1.AppendLine(flag ? "Order by CreatedDate desc" : "Order by CreatedDate asc");
        else
          dbQueryBuilder1.AppendLine("Order by CreatedDate desc");
      }
      return dbQueryBuilder1.Execute();
    }

    public static DataRowCollection GetSubmitLoanBorrowers(List<string> loanIds)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT lb.Guid, lb.BorrowerType, lb.CellPhone, lb.Email, lb.FirstName, ");
      dbQueryBuilder.AppendLine("lb.HomePhone, lb.ID, lb.LastName, lb.PairIndex, lb.SSN, ");
      dbQueryBuilder.AppendLine("lb.WorkEmail, lb.WorkPhone");
      dbQueryBuilder.AppendLine("FROM LoanBorrowers lb ");
      dbQueryBuilder.AppendLine("WHERE  lb.GUID in (" + Loan.concatGuids(loanIds) + ")");
      return dbQueryBuilder.Execute();
    }

    public static DataRowCollection GetSubmitLoanSummary(
      List<string> LoanIds,
      int start,
      int limit,
      string sorts,
      string filters)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * FROM    ( SELECT ROW_NUMBER() OVER ( ORDER BY ls.Status " + Loan.getSorts(sorts) + ", DateFileOpened desc ) AS RowNum, COUNT(*) OVER() AS TotalCount,");
      dbQueryBuilder.AppendLine("ls.Guid, ls.LoanNumber, ls.BorrowerFirstName, ls.BorrowerLastName, lb1.HomePhone as [BorrowerHomePhone], lb1.Email as [BorrowerEmail], lb1.ID as [BorrowerId],");
      dbQueryBuilder.AppendLine("ls.CoBorrowerFirstName, ls.CoBorrowerLastName, lb2.HomePhone as [CoBorrowerHomePhone], lb2.Email as [CoBorrowerEmail], lb2.ID as [CoBorrowerId],");
      dbQueryBuilder.AppendLine("ls.LastModified, ls.LoanOfficerName, ls.LoanAmount, ls.LoanOfficerId, ls.DateOfEstimatedCompletion as [EstimatedClosingDate],ls.DateCreated,");
      dbQueryBuilder.AppendLine("ls.Address1 as [StreetAddress], ls.City, ls.State, ls.Zip as [PostalCode], ls.LoanRate as [RateLock], ls.LoanPurpose, ls.LoanType,");
      dbQueryBuilder.AppendLine("u2.email as [LoanOfficerEmail], u2.phone as [LoanOfficerPhone],");
      dbQueryBuilder.AppendLine("ls.LoanProcessorName, ls.LoanProcessorId, u1.email as [LoanProcessorEmail], u1.phone as [LoanProcessorPhone],");
      dbQueryBuilder.AppendLine("ls.loanStatus as [LoanStatus]");
      dbQueryBuilder.AppendLine(" from LoanSummary as ls with (nolock) ");
      dbQueryBuilder.AppendLine("left outer join LoanBorrowers as lb1 with (nolock) on (lb1.Guid = ls.Guid and lb1.BorrowerType = 1 and lb1.PairIndex = 1) ");
      dbQueryBuilder.AppendLine("left outer join LoanBorrowers as lb2 with (nolock) on (lb2.Guid = ls.Guid and lb2.BorrowerType = 2 and lb2.PairIndex = 1) ");
      dbQueryBuilder.AppendLine("left outer join Users as u1 with (nolock) on ls.LoanProcessorId = u1.userid ");
      dbQueryBuilder.AppendLine("left outer join Users as u2 with (nolock) on ls.LoanOfficerId = u2.userid ");
      if (LoanIds == null || LoanIds.Count == 0)
        dbQueryBuilder.AppendLine("where 1=1");
      else
        dbQueryBuilder.AppendLine(" where ls.Guid in (" + Loan.concatGuids(LoanIds) + ")");
      dbQueryBuilder.AppendLine(Loan.getFilters(filters));
      dbQueryBuilder.AppendLine(") AS RowConstrainedResult WHERE   RowNum >= " + start.ToString() + " AND RowNum <= " + (limit + start - 1).ToString() + " ORDER BY RowNum");
      return dbQueryBuilder.Execute();
    }

    private static string concatGuids(List<string> LoanIds)
    {
      string str = string.Empty;
      int num = 0;
      foreach (string loanId in LoanIds)
      {
        str = num != 0 ? str + ",'{" + loanId + "}'" : "'{" + loanId + "}'";
        ++num;
      }
      return str;
    }

    private static string getSorts(string sorts)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (string.IsNullOrEmpty(sorts))
        return string.Empty;
      foreach (string str1 in sorts.Split(",".ToCharArray()))
      {
        if (!string.IsNullOrEmpty(str1))
        {
          bool flag = false;
          string empty = string.Empty;
          string str2;
          if (str1[0].ToString() == "-")
          {
            flag = true;
            str2 = str1.Replace("-", "");
          }
          else
            str2 = str1;
          switch (str2.ToLower())
          {
            case "applicationstartdate":
              stringBuilder.Append(",ls.DateCreated");
              if (flag)
              {
                stringBuilder.Append(" desc");
                continue;
              }
              continue;
            case "borrower.emailaddresstext":
            case "borroweremailaddresstext":
              stringBuilder.Append(",lb1.Email");
              if (flag)
              {
                stringBuilder.Append(" desc");
                continue;
              }
              continue;
            case "borrower.firstname":
            case "borrowerfirstname":
              stringBuilder.Append(",ls.BorrowerFirstName");
              if (flag)
              {
                stringBuilder.Append(" desc");
                continue;
              }
              continue;
            case "borrower.lastname":
            case "borrowerlastname":
              stringBuilder.Append(",ls.BorrowerlastName");
              if (flag)
              {
                stringBuilder.Append(" desc");
                continue;
              }
              continue;
            case "borrowername":
              stringBuilder.Append(",ls.BorrowerFirstName + ' ' + ls.BorrowerlastName");
              if (flag)
              {
                stringBuilder.Append(" desc");
                continue;
              }
              continue;
            case "coborrower.emailaddresstext":
            case "coborroweremailaddresstext":
              stringBuilder.Append(",lb2.Email");
              if (flag)
              {
                stringBuilder.Append(" desc");
                continue;
              }
              continue;
            case "coborrower.firstname":
            case "coborrowerfirstname":
              stringBuilder.Append(",ls.CoBorrowerFirstName");
              if (flag)
              {
                stringBuilder.Append(" desc");
                continue;
              }
              continue;
            case "coborrower.lastname":
            case "coborrowerlastname":
              stringBuilder.Append(",ls.CoBorrowerLastName");
              if (flag)
              {
                stringBuilder.Append(" desc");
                continue;
              }
              continue;
            case "lastmodified":
              stringBuilder.Append(",ls.LastModified");
              if (flag)
              {
                stringBuilder.Append(" desc");
                continue;
              }
              continue;
            case "loanamount":
              stringBuilder.Append(",ls.LoanAmount");
              if (flag)
              {
                stringBuilder.Append(" desc");
                continue;
              }
              continue;
            case "loannumber":
              stringBuilder.Append(",ls.LoanNumber");
              if (flag)
              {
                stringBuilder.Append(" desc");
                continue;
              }
              continue;
            case "loanofficername":
              stringBuilder.Append(",ls.LoanOfficerName");
              if (flag)
              {
                stringBuilder.Append(" desc");
                continue;
              }
              continue;
            case "subjectpropertyaddress":
              stringBuilder.Append(",ls.Address1 + ' ' + ls.City + ' ' + ls.State + ' ' + ls.Zip");
              if (flag)
              {
                stringBuilder.Append(" desc");
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
      return stringBuilder.ToString();
    }

    private static string getFilters(string filters)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (string.IsNullOrEmpty(filters))
        return string.Empty;
      stringBuilder.Append("and (");
      string[] strArray1 = filters.Split(",".ToCharArray());
      int num1 = 0;
      foreach (string str1 in strArray1)
      {
        string[] strArray2 = str1.Split("::".ToCharArray());
        if (num1 > 0)
          stringBuilder.Append(" and ");
        switch (strArray2[0].ToLower())
        {
          case "borrower.emailaddresstext":
          case "borroweremailaddresstext":
            stringBuilder.Append("lb1.Email='" + strArray2[2] + "'");
            ++num1;
            break;
          case "borrower.firstname":
          case "borrowerfirstname":
            stringBuilder.Append("ls.BorrowerFirstName='" + strArray2[2] + "'");
            ++num1;
            break;
          case "borrower.lastname":
          case "borrowerlastname":
            stringBuilder.Append("ls.BorrowerlastName='" + strArray2[2] + "'");
            ++num1;
            break;
          case "borrowername":
            stringBuilder.Append("ls.BorrowerFirstName + ' ' + ls.BorrowerlastName like '%" + strArray2[2] + "%'");
            ++num1;
            break;
          case "coborrower.emailaddresstext":
          case "coborroweremailaddresstext":
            stringBuilder.Append("lb2.Email='" + strArray2[2] + "'");
            ++num1;
            break;
          case "coborrower.firstname":
          case "coborrowerfirstname":
            stringBuilder.Append("ls.CoBorrowerFirstName='" + strArray2[2] + "'");
            ++num1;
            break;
          case "coborrower.lastname":
          case "coborrowerlastname":
            stringBuilder.Append("ls.CoBorrowerLastName='" + strArray2[2] + "'");
            ++num1;
            break;
          case "loanamount":
            stringBuilder.Append("ls.LoanAmount='" + strArray2[2] + "'");
            ++num1;
            break;
          case "loannumber":
            stringBuilder.Append("ls.LoanNumber like '%" + strArray2[2] + "%'");
            ++num1;
            break;
          case "loannumber|exactmatch":
            stringBuilder.Append("ls.LoanNumber='" + strArray2[2] + "'");
            ++num1;
            break;
          case "loanofficername":
            stringBuilder.Append("ls.LoanOfficerName like'%" + strArray2[2] + "%'");
            ++num1;
            break;
          case "name":
            string[] strArray3 = strArray2[2].Split(" ".ToCharArray());
            int num2 = 0;
            stringBuilder.Append(" ( ");
            foreach (string str2 in strArray3)
            {
              if (num2 > 0)
                stringBuilder.Append(" or ");
              stringBuilder.Append("( ls.BorrowerFirstName like '%" + str2 + "%'");
              stringBuilder.Append(" or ls.BorrowerlastName like '%" + str2 + "%'");
              stringBuilder.Append(" or ls.CoBorrowerFirstName like '%" + str2 + "%'");
              stringBuilder.Append(" or ls.CoBorrowerLastName like '%" + str2 + "%') ");
              ++num2;
            }
            stringBuilder.Append(" ) ");
            ++num1;
            break;
          case "subjectpropertyaddress":
            stringBuilder.Append("ls.Address1 + ' ' + ls.City + ' ' + ls.State + ' ' + ls.Zip like '%" + strArray2[2] + "%'");
            ++num1;
            break;
        }
      }
      stringBuilder.Append(")");
      return num1 == 0 ? string.Empty : stringBuilder.ToString();
    }

    private static string getDraftFilters(string filters)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (string.IsNullOrEmpty(filters))
        return string.Empty;
      stringBuilder.Append("and (");
      string[] strArray1 = filters.Split(",".ToCharArray());
      int num1 = 0;
      foreach (string str1 in strArray1)
      {
        string[] strArray2 = str1.Split("::".ToCharArray());
        if (num1 > 0)
          stringBuilder.Append(" and ");
        switch (strArray2[0].ToLower())
        {
          case "borrower.firstname":
            stringBuilder.Append("BorrowerFirstName='" + strArray2[2] + "'");
            ++num1;
            break;
          case "borrower.lastname":
            stringBuilder.Append("BorrowerlastName='" + strArray2[2] + "'");
            ++num1;
            break;
          case "coborrower.firstname":
            stringBuilder.Append("CoBorrowerFirstName='" + strArray2[2] + "'");
            ++num1;
            break;
          case "coborrower.lastname":
            stringBuilder.Append("CoBorrowerLastName='" + strArray2[2] + "'");
            ++num1;
            break;
          case "name":
            string[] strArray3 = strArray2[2].Split(" ".ToCharArray());
            int num2 = 0;
            stringBuilder.Append(" ( ");
            foreach (string str2 in strArray3)
            {
              if (num2 > 0)
                stringBuilder.Append(" or ");
              stringBuilder.Append("( BorrowerFirstName like '%" + str2 + "%'");
              stringBuilder.Append(" or BorrowerlastName like '%" + str2 + "%'");
              stringBuilder.Append(" or CoBorrowerFirstName like '%" + str2 + "%'");
              stringBuilder.Append(" or CoBorrowerLastName like '%" + str2 + "%') ");
              ++num2;
            }
            stringBuilder.Append(" ) ");
            ++num1;
            break;
          case "siteid":
            stringBuilder.Append(" SiteId='" + strArray2[2] + "'");
            ++num1;
            break;
        }
      }
      stringBuilder.Append(")");
      return stringBuilder.ToString();
    }

    private void publishLoanEvent(
      string userId,
      LoanEventType loanEventType,
      Guid loanId,
      string sessionId,
      string loanFileLocation,
      int loanVersionNumber,
      string loanFolder,
      bool isSourceEncompass,
      DateTime loanModifiedTime,
      bool skipWebhookRmqMessageForCC = false,
      bool batchApplied = false,
      bool eccLoanAppSubmitFlow = false)
    {
      if (loanVersionNumber <= 0)
      {
        TraceLog.WriteWarning(nameof (Loan), string.Format("Skipping loan webhook event for the loan = {0}, versionnumber = {4}, loanEventType = {1}, loanModifiedTime = {2}, userId = {3} ", (object) loanId, (object) loanEventType, (object) loanModifiedTime, (object) userId, (object) loanVersionNumber));
      }
      else
      {
        ClientContext current = ClientContext.GetCurrent();
        bool isSourceEncompass1 = EncompassServer.ServerMode != EncompassServerMode.Service;
        try
        {
          IDictionary serverSettings = current.Settings.GetServerSettings("Policies");
          bool flag1 = (bool) serverSettings[(object) "Policies.LoanEventPublishingEnable"];
          int num1 = (int) serverSettings[(object) "Policies.LoanEventPublishDelayMs"];
          bool dataLakeEnabled = (bool) serverSettings[(object) "Policies.DataLakeEnabled"];
          bool dataLakeUseGenericIngestEndpoint = (bool) serverSettings[(object) "Policies.DataLakeUseGenericIngestEndPoint"];
          bool webHookEnabled = (bool) serverSettings[(object) "Policies.WebHookEnabled"];
          int num2 = (int) serverSettings[(object) "Policies.PublishLoanEventConfirmTimeout"];
          bool flag2 = (bool) serverSettings[(object) "Policies.ENABLEGEICOINTEGRATION"];
          bool flag3 = false;
          if (!Loan.currentStateReported || Loan.currentLoanEventPublishEnabledState != flag1)
          {
            lock (this.lockObject)
            {
              Loan.currentLoanEventPublishEnabledState = flag1;
              Loan.currentStateReported = true;
            }
          }
          if (loanVersionNumber > 0)
          {
            string path = loanFileLocation.Replace(new FileInfo(loanFileLocation).Name, "Versions") + "\\" + string.Format("{0:D5}_", (object) loanVersionNumber) + "loan.em";
            if (File.Exists(path))
              loanFileLocation = path;
            else
              TraceLog.WriteWarning(nameof (Loan), string.Format("Version file : {0} does not exist", (object) path));
          }
          string loanFileLocation1 = string.Format("\\Loans\\{0}\\", (object) loanFolder) + "{" + (object) loanId + "}";
          flag3 = this.PublishLoanEventKafka(loanId.ToString(), userId, isSourceEncompass, webHookEnabled, loanEventType, loanModifiedTime, loanVersionNumber, loanFileLocation1);
          this.publishLoanSaveKafkaEvent(loanId.ToString(), loanVersionNumber.ToString(), loanEventType.ToString(), loanFileLocation, dataLakeEnabled, dataLakeUseGenericIngestEndpoint, loanFolder, batchApplied, userId, isSourceEncompass, loanModifiedTime, Loan.GetEventSequenceNumber());
          if (!flag2 || !Utils.ParseBoolean((object) (this.loanData?.GetField("ConsumerHIOrderEligible") ?? "false")))
            return;
          this.PublishLoanChangeHoiKafkaEvent(loanId.ToString(), userId, isSourceEncompass1);
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (Loan), string.Format("Failed to publish loan event.  Loan GUID={0}  User={1}  Session={2}  LoanEventType={3}  Stack={4}", (object) loanId, (object) userId, (object) sessionId, (object) loanEventType, (object) ex));
        }
      }
    }

    private void PublishLoanChangeHoiKafkaEvent(
      string loanId,
      string userId,
      bool isSourceEncompass)
    {
      ClientContext current = ClientContext.GetCurrent();
      try
      {
        LoanChangeEvent queueEvent = new LoanChangeEvent(current.InstanceName, loanId, userId, isSourceEncompass ? Enums.Source.URN_ELLI_SERVICE_ENCOMPASS : Enums.Source.URN_ELLI_SERVICE_EBS);
        queueEvent.AddKafkaMessage(loanId, current.InstanceName, current.ClientID, userId, isSourceEncompass);
        if (queueEvent.QueueMessages.Count <= 0)
          return;
        IMessageQueueEventService queueEventService = (IMessageQueueEventService) new MessageQueueEventService();
        IMessageQueueProcessor processor = (IMessageQueueProcessor) new KafkaProcessor();
        queueEventService.MessageQueueProducer((MessageQueueEvent) queueEvent, processor);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("Exception publishing loan change Event to kafka for loanId - {0}. Exception Message - {1}, details - {2}, Source - {3}", (object) loanId, (object) ex.Message, (object) ex.StackTrace, (object) ex.Source));
      }
    }

    private void publishLoanSaveKafkaEvent(
      string loanId,
      string loanVersionNumber,
      string loanEventType,
      string loanFileLocation,
      bool dataLakeEnabled,
      bool dataLakeUseGenericIngestEndpoint,
      string loanFolder,
      bool batchApplied,
      string auditUserId,
      bool isSourceEncompass,
      DateTime loanModifiedTime,
      string eventSequenceNumber)
    {
      ClientContext current = ClientContext.GetCurrent();
      try
      {
        LoanSaveEvent queueEvent = new LoanSaveEvent("serviceId", current.InstanceName, "siteId", "loan", auditUserId, isSourceEncompass ? Enums.Source.URN_ELLI_SERVICE_ENCOMPASS : Enums.Source.URN_ELLI_SERVICE_EBS, loanModifiedTime);
        queueEvent.AddKafkaMessage(loanId, loanVersionNumber, loanEventType, loanFileLocation, dataLakeEnabled, dataLakeUseGenericIngestEndpoint, current.ClientID, loanFolder, eventSequenceNumber, batchApplied, auditUserId, ClientContext.CurrentRequest.CorrelationId);
        if (queueEvent.QueueMessages.Count <= 0)
          return;
        IMessageQueueEventService queueEventService = (IMessageQueueEventService) new MessageQueueEventService();
        IMessageQueueProcessor processor = (IMessageQueueProcessor) new KafkaProcessor();
        queueEventService.MessageQueueProducer((MessageQueueEvent) queueEvent, processor);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("Exception publishing loan save Event to kafka for loanId - {0}. Exception Message - {1}, details - {2}, Source - {3}", (object) loanId, (object) ex.Message, (object) ex.StackTrace, (object) ex.Source));
      }
    }

    private void publishLoanDeleteKafkaEvent(
      string loanId,
      string loanEventType,
      string eventSequenceNumber,
      string auditUserId,
      bool isSourceEncompass)
    {
      ClientContext current = ClientContext.GetCurrent();
      try
      {
        LoanDeleteEvent queueEvent = new LoanDeleteEvent("serviceId", current.InstanceName, "siteId", "loan", auditUserId, isSourceEncompass ? Enums.Source.URN_ELLI_SERVICE_ENCOMPASS : Enums.Source.URN_ELLI_SERVICE_EBS, this.LastModifiedUTC);
        if (loanId != null && loanId.StartsWith("{") && loanId.EndsWith("}"))
          loanId = loanId.TrimStart('{').TrimEnd('}');
        queueEvent.AddKafkaMessage(loanId, loanEventType, current.ClientID, eventSequenceNumber, auditUserId);
        if (queueEvent.QueueMessages.Count <= 0)
          return;
        IMessageQueueEventService queueEventService = (IMessageQueueEventService) new MessageQueueEventService();
        IMessageQueueProcessor processor = (IMessageQueueProcessor) new KafkaProcessor();
        queueEventService.MessageQueueProducer((MessageQueueEvent) queueEvent, processor);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("Exception publishing loan delete Event to kafka for loanId - {0}. Exception Message - {1}, details - {2}, Source - {3}", (object) loanId, (object) ex.Message, (object) ex.StackTrace, (object) ex.Source));
      }
    }

    private bool PublishLoanEventKafka(
      string loanId,
      string userId,
      bool isSourceEncompass,
      bool webHookEnabled,
      LoanEventType loanEventType,
      DateTime loanModifiedTime,
      int loanVersionNumber,
      string loanFileLocation)
    {
      ClientContext current = ClientContext.GetCurrent();
      bool serverSetting = (bool) current.Settings.GetServerSettings("License")[(object) "License.EnableAIQLicense"];
      bool flag1 = false;
      try
      {
        TraceLog.WriteInfo(nameof (Loan), string.Format("PublishLoanEventKafka : Values of isSourceEncompassRequest - {0}", (object) isSourceEncompass));
        WebHooksEvent queueEvent = new WebHooksEvent("serviceId", current.InstanceName, (string) null, loanEventType.ToString(), userId, EncompassServer.ServerMode != EncompassServerMode.Service ? Enums.Source.URN_ELLI_SERVICE_ENCOMPASS : Enums.Source.URN_ELLI_SERVICE_EBS, loanModifiedTime);
        bool flag2 = string.Equals(loanEventType.ToString(), "Create", StringComparison.InvariantCultureIgnoreCase) || string.Equals(loanEventType.ToString(), "Update", StringComparison.InvariantCultureIgnoreCase);
        string source = isSourceEncompass ? SourceEnum.Encompass.ToString().ToLower() : SourceEnum.EBS.ToString().ToLower();
        StandardMessage standardMessage = queueEvent.StandardMessage;
        LoanIdentity id = this.id;
        string str;
        if ((object) id == null)
          str = (string) null;
        else
          str = id.Guid.Trim('{', '}');
        standardMessage.EntityId = str;
        queueEvent.AddKafkaMessage(ClientContext.CurrentRequest.CorrelationId, nameof (Loan), webHookEnabled, loanId, current.ClientID, Enums.Type.LOAN_EVENT_WEBHOOKS, flag2 ? loanVersionNumber : 0, flag2 ? loanFileLocation : (string) null, source, loanEventType.ToString(), serverSetting);
        if (queueEvent.QueueMessages.Count > 0)
        {
          IMessageQueueEventService queueEventService = (IMessageQueueEventService) new MessageQueueEventService();
          IMessageQueueProcessor processor = (IMessageQueueProcessor) new KafkaProcessor();
          queueEventService.MessageQueueProducer((MessageQueueEvent) queueEvent, processor);
          flag1 = true;
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("Exception publishing loanEvent to kafka for loanId - {0}. Exception details {1}", (object) loanId, (object) ex.StackTrace));
      }
      return flag1;
    }

    private void PublishLoanBillingKafkaEvent(
      string loanId,
      LoanEventType loanEventType,
      UserInfo currentUser,
      TransactionLog transactionLog)
    {
      ClientContext current = ClientContext.GetCurrent();
      try
      {
        string instanceName = current.InstanceName;
        string loanId1;
        if (loanId == null)
          loanId1 = (string) null;
        else
          loanId1 = loanId.Trim('{', '}');
        string userId = currentUser?.Userid ?? "";
        DateTime lastModifiedUtc = this.LastModifiedUTC;
        LoanBillingEvent queueEvent = new LoanBillingEvent(instanceName, loanId1, userId, Enums.Source.URN_ELLI_SERVICE_EBS, lastModifiedUtc);
        queueEvent.AddKafkaMessage(ClientContext.CurrentRequest.CorrelationId, loanEventType.ToString(), TransactionLog.MapTransactionLog(transactionLog));
        if (queueEvent.QueueMessages.Count <= 0)
          return;
        IMessageQueueEventService queueEventService = (IMessageQueueEventService) new MessageQueueEventService();
        IMessageQueueProcessor processor = (IMessageQueueProcessor) new KafkaProcessor();
        queueEventService.MessageQueueProducer((MessageQueueEvent) queueEvent, processor);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("Exception publishing loan billing Event to kafka for loanId - {0}. Exception Message - {1}, details - {2}, Source - {3}", (object) loanId, (object) ex.Message, (object) ex.StackTrace, (object) ex.Source));
      }
    }

    public bool PublishDocumentReceivedEventKafka(
      string loanId,
      string userId,
      DocumentReceivedEvent documentReceived)
    {
      bool flag1 = false;
      ClientContext current = ClientContext.GetCurrent();
      bool flag2 = EncompassServer.ServerMode != EncompassServerMode.Service;
      try
      {
        if (documentReceived != null)
        {
          WebHooksEvent queueEvent = new WebHooksEvent("serviceId", current.InstanceName, "siteId", (string) null, userId, flag2 ? Enums.Source.URN_ELLI_SERVICE_ENCOMPASS : Enums.Source.URN_ELLI_SERVICE_EBS, DateTime.Now);
          WebHooksEvent webHooksEvent = queueEvent;
          string loanId1;
          if (loanId == null)
            loanId1 = (string) null;
          else
            loanId1 = loanId.Trim('{', '}');
          string correlationId = ClientContext.CurrentRequest.CorrelationId;
          string userId1 = userId;
          string clientId = current.ClientID;
          string instanceName = current.InstanceName;
          DocumentReceivedEvent documentReceived1 = documentReceived;
          int num = flag2 ? 1 : 0;
          webHooksEvent.AddDocumentReceivedMessage(loanId1, correlationId, userId1, clientId, instanceName, documentReceived1, num != 0);
          if (loanId != null && loanId.StartsWith("{") && loanId.EndsWith("}"))
            loanId = loanId.TrimStart('{').TrimEnd('}');
          queueEvent.StandardMessage.EntityId = loanId;
          IMessageQueueEventService queueEventService = (IMessageQueueEventService) new MessageQueueEventService();
          IMessageQueueProcessor processor = (IMessageQueueProcessor) new KafkaProcessor();
          queueEventService.MessageQueueProducer((MessageQueueEvent) queueEvent, processor);
          flag1 = true;
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Loan), string.Format("Exception publishing Document Received Event to kafka for loanId - {0}. Exception Message - {1}, details - {2}, Source - {3}", (object) loanId, (object) ex.Message, (object) ex.StackTrace, (object) ex.Source));
      }
      return flag1;
    }

    public static EllieMae.EMLite.ClientServer.LoanAssociateInfo[] GetLoanAssociatedUsers(
      string guid,
      string userId = null)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select la.guid, la.associateGuid, la.MilestoneID, la.RoleID, la.AssociateType, la.UserID, la.GroupID, la.Name, la.Email,");
      dbQueryBuilder.AppendLine(" la.phone, la.fax, la.allowWrites, rl.RoleName, rl.RoleAbbr from LoanAssociateUsers la left outer join roles rl ");
      dbQueryBuilder.AppendLine(" on la.roleid = rl.roleid where Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid));
      if (!string.IsNullOrEmpty(userId))
        dbQueryBuilder.AppendLine(" and userID = '" + userId + "'");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      EllieMae.EMLite.ClientServer.LoanAssociateInfo[] loanAssociatedUsers = new EllieMae.EMLite.ClientServer.LoanAssociateInfo[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
      {
        DataRow dataRow = dataRowCollection[index];
        loanAssociatedUsers[index] = new EllieMae.EMLite.ClientServer.LoanAssociateInfo(string.Concat(dataRow["AssociateGuid"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Guid"]), EllieMae.EMLite.DataAccess.SQL.DecodeEnum<LoanAssociateType>(dataRow["AssociateType"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["MilestoneID"]), "", -1, EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["RoleID"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["RoleName"]), "", EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["AllowWrites"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["UserID"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["GroupID"], -1), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Name"], ""), "", "", EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Email"], ""), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Phone"], ""));
      }
      return loanAssociatedUsers;
    }

    private static string GetEventSequenceNumber()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("SELECT DATEDIFF(s, '1970-01-01 00:00:00', GetUtcDate()) as Date");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection != null && dataRowCollection.Count > 0 ? dataRowCollection[0]["Date"].ToString() : throw new Exception("Unable to get event sequence number from sql server");
    }

    private bool IsClosedLoanBillingActivated(
      LoanData priorData,
      LoanData currentData,
      out TransactionLog.ReasonCode reasonCode)
    {
      reasonCode = TransactionLog.ReasonCode.None;
      if ((BillingModel) ClientContext.GetCurrent().Settings.GetServerSetting("License.BillingModel") != BillingModel.ClosedLoan)
        return false;
      if (priorData != null && currentData != null)
      {
        reasonCode = TransactionLog.GetActivationReason(currentData);
        if (priorData.GetSimpleField("3260") != this.LoanData.GetSimpleField("3260") || priorData.GetSimpleField("BILLINGCATEGORY") != this.LoanData.GetSimpleField("BILLINGCATEGORY") || TransactionLog.GetActivationReason(priorData) != reasonCode)
          return true;
      }
      else if (priorData == null && currentData != null)
      {
        reasonCode = TransactionLog.GetActivationReason(currentData);
        if (Utils.ParseDate((object) currentData.GetSimpleField("3260")) != DateTime.MinValue || !string.IsNullOrEmpty(this.LoanData.GetSimpleField("BILLINGCATEGORY")) || reasonCode != TransactionLog.ReasonCode.None)
          return true;
      }
      return false;
    }

    private bool IsDocumentAssignedWithAttachment(
      LoanData priorData,
      LoanData currentData,
      out DocumentReceivedEvent documentReceived)
    {
      documentReceived = (DocumentReceivedEvent) null;
      if (priorData != null && currentData != null)
      {
        List<DocumentLog> list1 = ((IEnumerable<DocumentLog>) currentData.GetLogList().GetAllDocuments(true, false)).ToList<DocumentLog>();
        List<DocumentLog> list2 = ((IEnumerable<DocumentLog>) priorData.GetLogList().GetAllDocuments(true, false)).ToList<DocumentLog>();
        foreach (DocumentLog documentLog1 in list1)
        {
          DocumentLog currentDocLog = documentLog1;
          if (currentDocLog.Files.HasActiveFile)
          {
            DocumentLog documentLog2 = list2.FirstOrDefault<DocumentLog>((System.Func<DocumentLog, bool>) (x => x.Guid.Equals(currentDocLog.Guid, StringComparison.CurrentCultureIgnoreCase)));
            if (documentLog2 == null || !documentLog2.Files.HasActiveFile)
            {
              List<EllieMae.EMLite.ClientServer.MessageServices.Message.Attachment.Attachment> attachmentList = new List<EllieMae.EMLite.ClientServer.MessageServices.Message.Attachment.Attachment>();
              foreach (FileAttachment fileAttachment in FileAttachmentStore.GetFileAttachments(this, true, (IAttachmentXmlProviderFactory) new AttachmentXmlProviderFactory(ClientContext.GetCurrent().Settings)))
              {
                if (currentDocLog.Files.Contains(fileAttachment.ID))
                {
                  EllieMae.EMLite.ClientServer.MessageServices.Message.Attachment.Attachment attachment = new EllieMae.EMLite.ClientServer.MessageServices.Message.Attachment.Attachment()
                  {
                    Id = fileAttachment.ID,
                    Title = fileAttachment.Title
                  };
                  attachmentList.Add(attachment);
                }
              }
              DocumentReceived documentReceived1 = new DocumentReceived()
              {
                Id = currentDocLog.Guid,
                Title = currentDocLog.Title,
                DocumentReceivedDate = currentDocLog.DateReceived,
                Application = this.AddApplication(currentDocLog, currentData),
                Attachments = attachmentList
              };
              documentReceived = new DocumentReceivedEvent()
              {
                documentReceived = documentReceived1
              };
              return true;
            }
          }
        }
      }
      return false;
    }

    private ApplicationReference AddApplication(DocumentLog currentDocLog, LoanData loanData)
    {
      if (currentDocLog == null || currentDocLog.PairId == null)
        return (ApplicationReference) null;
      if (string.Equals(currentDocLog.PairId, "All", StringComparison.CurrentCultureIgnoreCase))
      {
        ApplicationReference applicationReference = new ApplicationReference();
        applicationReference.EntityId = "All";
        applicationReference.EntityName = "All";
        applicationReference.EntityType = EllieMae.EMLite.ClientServer.Trading.EntityRefTypeContract.Application;
        applicationReference.LegacyId = "All";
        return applicationReference;
      }
      BorrowerPair borrowerPair = loanData.GetBorrowerPair(currentDocLog.PairId);
      if (borrowerPair == null)
        return (ApplicationReference) null;
      ApplicationReference applicationReference1 = new ApplicationReference();
      applicationReference1.EntityId = borrowerPair.Borrower.EID;
      applicationReference1.EntityName = this.FormatBorrowerName(borrowerPair);
      applicationReference1.EntityType = EllieMae.EMLite.ClientServer.Trading.EntityRefTypeContract.Application;
      applicationReference1.LegacyId = currentDocLog.PairId;
      return applicationReference1;
    }

    private string FormatBorrowerName(BorrowerPair borr)
    {
      string str1 = borr.Borrower.ToString();
      string str2 = borr.CoBorrower.ToString();
      if (string.IsNullOrWhiteSpace(str2))
        return str1;
      return string.IsNullOrWhiteSpace(str1) ? str2 : str1 + " and " + str2;
    }

    public static bool isLoanArchived(string GUID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT  ls.IsArchived LoanArchive, lf.archive FolderArchive FROM LoanSummary ls Left join LoanFolder lf on ls.LoanFolder = lf.folderName where ls.Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) GUID));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      bool flag = false;
      if (dataRowCollection != null && dataRowCollection.Count > 0)
        flag = (bool) dataRowCollection[0]["LoanArchive"] || dataRowCollection[0]["FolderArchive"].ToString() == "Y";
      return flag;
    }

    public class SQLTestException : Exception
    {
      private int _number;

      public int Number => this._number;

      public SQLTestException(string message, int number)
        : base(message)
      {
        this._number = number;
      }

      public SQLTestException(string message)
        : base(message)
      {
      }
    }
  }
}

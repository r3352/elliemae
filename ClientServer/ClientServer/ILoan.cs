// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ILoan
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Elli.ElliEnum;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.SkyDrive;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ILoan : IDisposable
  {
    string Guid { get; }

    LoanIdentity GetIdentity();

    LoanData GetLoanData(bool isExternalOrganization);

    LoanData GetLoanDataInternal(bool isExternalOrganization);

    int Save(
      LoanData data,
      string clientStackTrack = "�",
      bool enableRateLockValidation = false,
      bool enableBackupLoanFile = false);

    int Save(
      LoanData data,
      bool unlock,
      string clientStackTrack = "�",
      bool enableRateLockValidation = false,
      bool enableBackupLoanFile = false);

    void SaveWithoutAuditRecord(LoanData data);

    void Delete(bool isExternalOrganization);

    void Delete(bool demandAdmin, bool isExternalOrganization);

    void Delete(bool demandAdmin, bool skipSystemLogging, bool isExternalOrganization);

    void SaveSystemLogs(LoanData data);

    void AddLoanEventLog(LoanEventLogList loanEventLog);

    string[] SelectFields(string[] fieldIds);

    string SelectField(string fieldId);

    LockInfo GetLockInfo();

    LockInfo GetLockInfo(string userid);

    LockInfo[] GetAllLockInfo();

    LockInfo[] CurrentLocks { get; }

    void Lock(LoanInfo.LockReason reason, LockInfo.ExclusiveLock exclusive);

    void Lock(LoanInfo.LockReason reason, LockInfo.ExclusiveLock exclusive, bool addToRecentLoans);

    void LockInternal(
      LoanInfo.LockReason reason,
      LockInfo.ExclusiveLock exclusive,
      bool addToRecentLoans);

    bool HasOwnership();

    void Unlock();

    void Unlock(bool force);

    void Unlock(bool force, string sessionID);

    void Unlock(bool force, string sessionID, bool unlockAll);

    bool SentToProcessing();

    void Move(string loanFolder, DuplicateLoanAction dupAction);

    void Move(string loanFolder, string loanName, DuplicateLoanAction dupAction);

    BinaryObject Export();

    void Import(BinaryObject data, string loanFolder);

    void Import(LoanData data);

    string[] GetSupportingDataKeysOnCIFs();

    BinaryObject GetSupportingDataOnCIFs(string key);

    SnapshotObject GetSupportingSnapshotData(
      LogSnapshotType type,
      System.Guid snapshotGuid,
      string fileNameAsKey);

    BinaryObject GetSupportingLinkedDataOnCIFs(string loanFolder, string loanName, string key);

    void SaveSupportingDataOnCIFs(string key, BinaryObject data);

    void SaveSupportingSnapshotData(
      LogSnapshotType type,
      System.Guid snapshotGuid,
      string fileNameAsKey,
      SnapshotObject data);

    void SaveSupportingLinkedDataOnCIFs(
      string loanFolder,
      string loanName,
      string key,
      BinaryObject data);

    void AppendSupportingDataOnCIFs(string key, BinaryObject data);

    bool SupportingDataExistsOnCIFs(string key);

    bool DeleteSupportingDataOnCIFs(string key);

    int DeleteSupportingDataOnCIFs(string[] keys);

    Dictionary<string, string> GetLoanSnapshot(
      LogSnapshotType type,
      System.Guid snapshotGuid,
      bool ucdExists);

    Dictionary<string, Dictionary<string, string>> GetLoanSnapshots(
      LogSnapshotType type,
      Dictionary<string, bool> snapshotGuids);

    PipelineInfo GetPipelineInfo(bool isExternalOrganization);

    PipelineInfo GetPipelineInfo(bool isExternalOrganization, int sqlRead);

    PipelineInfo GetPipelineInfoInternal(bool isExternalOrganization, int sqlRead);

    PipelineInfo GetPipelineInfo(
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization);

    DateTime GetLastModifiedDate(bool fromDB);

    DateTime GetLastModifiedDate(bool fromDB, int sqlRead);

    void AddToRecentLoans();

    LoanAssociateInfo[] GetLoanAssociates(bool milestoneRolesOnly);

    LoanAssociateInfo[] GetLoanAssociates(bool milestoneRolesOnly, bool resolveUsers);

    LoanInfo.Right GetRights();

    LoanInfo.Right GetRights(string userId);

    LoanInfo.Right GetAssignedRights();

    LoanInfo.Right GetAssignedRights(string userId);

    bool HasRights(LoanInfo.Right rights);

    bool HasRights(string userId, LoanInfo.Right rights);

    bool HasAssignedRights(LoanInfo.Right rights);

    bool HasAssignedRights(string userId, LoanInfo.Right rights);

    Hashtable GetAllAssignedRights();

    void AddRights(string userId, LoanInfo.Right rights);

    void RemoveRights(string userId, LoanInfo.Right rights);

    void SetRights(string userId, LoanInfo.Right rights);

    void AddIntoRights(Hashtable rights);

    void RemoveFromRights(Hashtable rights);

    void ReplaceAllRights(Hashtable rights);

    void Close();

    void Close(bool unlock);

    LoanHistoryEntry[] GetLoanHistory(string[] objectList);

    void AppendLoanHistory(LoanHistoryEntry[] entryList);

    ZipReader CreateZipReader(string key);

    ZipWriter CreateZipWriter(string key, int compressionLevel);

    FileAttachment[] GetFileAttachments();

    void ReplaceBackgroundAttachment(FileAttachment attachment);

    void SaveFileAttachments(FileAttachment[] attachmentList);

    SkyDriveUrl GetSkyDriveUrlForGet(string fileKey);

    SkyDriveUrl GetSkyDriveUrlForPut(string fileKey, string contentType, bool useSkyDriveClassic = false);

    SkyDriveUrl GetSkyDriveUrlForMeta(string objectId, string fileName = null);

    SkyDriveUrl GetSkyDriveUrlForObject(string objectId);

    List<SkyDriveUrl> GetSkyDriveUrlForObjects(string[] objectIds);

    string[] GetSkyDriveSupportingDataKeys();

    void SaveLockSnapshotRecapture(LockSnapshotRecapture lockSnapshotRecapture);

    void SetLoanPropertySetting(LoanProperty lp);

    LoanProperty[] GetLoanPropertySettings();

    LoanProperty[] GetLoanPropertySettingsInternal();
  }
}

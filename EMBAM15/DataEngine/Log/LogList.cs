// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.LogList
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.Workflow;
using EllieMae.EMLite.Xml;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class LogList
  {
    private const string className = "LogList";
    private const string DEFAULT_MILESTONE_STAGE = "Started";
    private static readonly string sw = Tracing.SwDataEngine;
    private static Hashtable recordTypes = (Hashtable) null;
    private ArrayList msList = new ArrayList();
    private Hashtable msTbl = new Hashtable();
    private MilestoneTemplate template;
    private bool msLock;
    private bool msDateLock;
    private Dictionary<string, CRMLog> crmTbl = new Dictionary<string, CRMLog>();
    private ArrayList records = new ArrayList();
    private Hashtable recordTbl = new Hashtable();
    private ArrayList removedRecords = new ArrayList();
    private Hashtable removedRecordTbl = new Hashtable();
    private bool isDirty;
    private bool loaded;
    private LoanData loan;
    private IMilestoneDateCalculator milestoneDateCalculator;
    private bool showDates;

    internal LogList(LoanData loan, IMilestoneDateCalculator milestoneDateCalculator)
    {
      this.loan = loan;
      this.loaded = true;
      this.milestoneDateCalculator = milestoneDateCalculator;
    }

    internal LogList(
      LoanData loan,
      string[] msArr,
      IMilestoneDateCalculator milestoneDateCalculator)
      : this(loan, milestoneDateCalculator)
    {
      this.loaded = false;
      if (msArr != null && msArr.Length != 0)
      {
        foreach (string str in msArr)
        {
          MilestoneLog milestoneLog = new MilestoneLog(str, this, loan.MilestoneDateTimeType, milestoneDateCalculator);
          this.msList.Add((object) milestoneLog);
          this.msTbl[(object) str] = (object) milestoneLog;
        }
      }
      this.loaded = true;
    }

    internal LogList(
      LoanData loan,
      string systemId,
      XmlElement systemLog,
      XmlElement nonSystemLog,
      IMilestoneDateCalculator milestoneDateCalculator)
      : this(loan, milestoneDateCalculator)
    {
      PerformanceMeter.Current.AddCheckpoint("Starting the LogList Constructor", 86, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\BAM\\DataEngine\\Log\\LogList.cs");
      this.loaded = false;
      this.parseLogRecords(nonSystemLog);
      PerformanceMeter.Current.AddCheckpoint("Finished parsing non system log records", 92, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\BAM\\DataEngine\\Log\\LogList.cs");
      this.parseCRMRecords(systemLog.SelectNodes("CRM"));
      PerformanceMeter.Current.AddCheckpoint("Finished parsing CRM log records", 96, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\BAM\\DataEngine\\Log\\LogList.cs");
      this.parseSystemLog(systemLog);
      PerformanceMeter.Current.AddCheckpoint("Finished parsing system log records", 99, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\BAM\\DataEngine\\Log\\LogList.cs");
      this.CleanUpMilestoneDates();
      PerformanceMeter.Current.AddCheckpoint("Finished cleaning up milestone dates", 103, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\BAM\\DataEngine\\Log\\LogList.cs");
      this.migrateRegistrationLogs();
      PerformanceMeter.Current.AddCheckpoint("Finished migrating registration logs", 106, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\BAM\\DataEngine\\Log\\LogList.cs");
      this.MarkAsClean();
      this.loaded = true;
      PerformanceMeter.Current.AddCheckpoint("Finished the LogList Constructor", 112, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\BAM\\DataEngine\\Log\\LogList.cs");
    }

    public LogList(LoanData loan, string xmlStr, IMilestoneDateCalculator milestoneDateCalculator)
    {
      this.loan = loan;
      this.milestoneDateCalculator = milestoneDateCalculator;
      if (xmlStr != "")
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlStr);
        XmlElement logRecordRoot = (XmlElement) xmlDocument.SelectSingleNode("LOG");
        if (logRecordRoot != null)
          this.parseLogRecords(logRecordRoot);
      }
      this.MarkAsClean();
      this.loaded = true;
    }

    public void ReAssignCustomMileStones()
    {
      bool flag = true;
      foreach (DocumentLog allDocument in this.GetAllDocuments())
      {
        string stage = allDocument.Stage;
        for (int index = 0; index < this.GetAllMilestones().Length; ++index)
        {
          if (this.GetAllMilestones()[index].Stage == stage)
          {
            flag = true;
            break;
          }
          flag = false;
        }
        if (!flag)
          allDocument.Stage = this.GetCurrentMilestone().Stage;
      }
    }

    public void ReAssignTasksMilestones()
    {
      foreach (MilestoneTaskLog milestoneTaskLog in this.GetAllMilestoneTaskLogs())
      {
        if (this.GetMilestone(milestoneTaskLog.Stage) == null)
          milestoneTaskLog.Stage = this.GetCurrentMilestone().Stage;
      }
    }

    public LoanData Loan => this.loan;

    internal bool Loaded => this.loaded;

    public string NextStage
    {
      get
      {
        string str = string.Empty;
        for (int index = this.msList.Count - 1; index >= 0; --index)
        {
          MilestoneLog ms = (MilestoneLog) this.msList[index];
          if (ms.Done)
          {
            str = index == this.msList.Count - 1 ? ms.Stage : ((MilestoneLog) this.msList[index + 1]).Stage;
            break;
          }
        }
        return !(str == string.Empty) ? str : ((MilestoneLog) this.msList[0]).Stage;
      }
    }

    public bool AddRecord(LogRecordBase rec) => this.AddRecord(rec, true);

    public bool AddRecord(LogRecordBase rec, bool markAsDirty)
    {
      if (this.recordTbl.Contains((object) rec.Guid))
        return false;
      this.appendRecord(rec);
      rec.AttachToLog(this);
      this.CleanUpMilestoneDates();
      if (markAsDirty)
      {
        rec.OnRecordAdd();
        this.isDirty = true;
      }
      if (rec is IDisclosureTrackingLog)
      {
        string field = this.loan.GetField("COMPLIANCEVERSION.X1");
        if (string.Compare(field, "No Log", true) == 0 || field == "")
          this.loan.SetField("COMPLIANCEVERSION.X1", this.loan.CurrentLoanVersion);
        if (rec is IDisclosureTracking2015Log && this.loan.IsLocked("LE1.X9"))
          this.loan.RemoveCurrentLock("LE1.X9");
      }
      this.loan.NotifyLogRecordAdded(rec);
      return true;
    }

    public bool AddCRMMapping(
      string mappingID,
      CRMLogType mappingType,
      string contactGuid,
      CRMRoleType roleType)
    {
      return this.AddCRMMapping(new CRMLog(mappingID, mappingType, contactGuid, this, roleType));
    }

    public bool AddCRMMapping(CRMLog rec)
    {
      if (this.crmTbl == null)
        this.crmTbl = new Dictionary<string, CRMLog>();
      if (this.crmTbl.ContainsKey(rec.MappingID))
        this.crmTbl.Remove(rec.MappingID);
      this.crmTbl.Add(rec.MappingID, rec);
      rec.AttachToLog(this);
      this.CleanUpMilestoneDates();
      this.isDirty = true;
      return true;
    }

    public void RemoveRecord(LogRecordBase rec) => this.RemoveRecord(rec, false);

    public void RemoveRecord(LogRecordBase rec, bool suppressHistory)
    {
      if (!this.records.Contains((object) rec))
        throw new InvalidOperationException("The specified log record is not a part of this loan.");
      if (!this.loan.AccessRules.IsLogEntryDeletable(rec))
        throw new DataAccessViolationException("User is not authorized to delete this record");
      switch (rec)
      {
        case LockRequestLog _:
        case LockCancellationLog _:
        case LockDenialLog _:
        case LockRemovedLog _:
        case LockValidationLog _:
        case LockVoidLog _:
          Tracing.Log(LogList.sw, nameof (LogList), TraceLevel.Error, string.Format("Secondary Logs cannot be removed from the loan. Log Type: {0}. {1}", (object) rec.GetType().Name, (object) new StackTrace()));
          return;
        case VerifLog _:
          this.loan.RemoveDocTrackLink(((VerifLog) rec).Id);
          break;
        case DisclosureTrackingBase _:
          using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass"))
          {
            if (registryKey != null && !(string.Concat(registryKey.GetValue("DisclosureTrackingLogAlert")) == ""))
              return;
            throw new InvalidOperationException("DisclosureTrackingLog can not be removed from the loan.");
          }
      }
      foreach (LogRecordBase record in this.records)
        record.RemoveReferencesTo(rec);
      this.records.Remove((object) rec);
      this.recordTbl.Remove((object) rec.Guid);
      if (rec.SupportsLoanHistory() && !suppressHistory)
      {
        this.appendRemovedRecord(rec);
        switch (rec)
        {
          case DocumentLog _:
            rec.TrackChange("Doc deleted");
            (rec as DocumentLog).UnmarkAsFinalCD(this.Loan);
            break;
          case ConditionLog _:
            rec.TrackChange("Condition deleted");
            break;
        }
      }
      this.loan.NotifyLogRecordRemoved(rec);
      this.isDirty = true;
    }

    public void RemoveCRMMapping(CRMLog rec)
    {
      this.crmTbl.Remove(rec.MappingID);
      this.isDirty = true;
    }

    public void RemoveCRMMapping(string contactGuid)
    {
      foreach (string key in this.crmTbl.Keys)
      {
        if (this.crmTbl[key].ContactGuid == contactGuid)
        {
          this.crmTbl.Remove(key);
          this.isDirty = true;
          break;
        }
      }
    }

    public bool CauseBorrowerIntentToCProceedViolation(
      string logGuid,
      DateTime newDisclosureSentTime,
      bool isDisclosed,
      DateTime newReceivedDate)
    {
      if (this.loan.GetField("3164") != "Y" || !Utils.IsDate((object) this.loan.GetField("3197")))
        return false;
      DisclosureTrackingLog[] disclosureTrackingLog1 = this.GetAllDisclosureTrackingLog(false);
      DisclosureTrackingLog disclosureTrackingLog2 = (DisclosureTrackingLog) null;
      DisclosureTrackingLog disclosureTrackingLog3 = (DisclosureTrackingLog) null;
      foreach (DisclosureTrackingLog disclosureTrackingLog4 in disclosureTrackingLog1)
      {
        if ((disclosureTrackingLog4.IsDisclosed || !(disclosureTrackingLog4.Guid != logGuid)) && (!(disclosureTrackingLog4.Guid == logGuid) || isDisclosed) && (disclosureTrackingLog4.DisclosedForGFE || disclosureTrackingLog4.DisclosedForTIL))
        {
          if (disclosureTrackingLog4.DisclosedForGFE)
          {
            if (disclosureTrackingLog2 == null && disclosureTrackingLog4.DisclosedForGFE)
              disclosureTrackingLog2 = disclosureTrackingLog4;
            else if (disclosureTrackingLog4.Guid != logGuid)
            {
              if (disclosureTrackingLog2.Guid == logGuid)
              {
                if (newDisclosureSentTime > disclosureTrackingLog4.DisclosedDate)
                  disclosureTrackingLog2 = disclosureTrackingLog4;
              }
              else if (disclosureTrackingLog2.DisclosedDate > disclosureTrackingLog4.DisclosedDate)
                disclosureTrackingLog2 = disclosureTrackingLog4;
            }
            else if (disclosureTrackingLog2.DisclosedDate > newDisclosureSentTime)
              disclosureTrackingLog2 = disclosureTrackingLog4;
          }
          if (disclosureTrackingLog4.DisclosedForTIL)
          {
            if (disclosureTrackingLog3 == null && disclosureTrackingLog4.DisclosedForTIL)
              disclosureTrackingLog3 = disclosureTrackingLog4;
            else if (disclosureTrackingLog4.Guid != logGuid)
            {
              if (disclosureTrackingLog3.Guid == logGuid)
              {
                if (newDisclosureSentTime > disclosureTrackingLog4.DisclosedDate)
                  disclosureTrackingLog3 = disclosureTrackingLog4;
              }
              else if (disclosureTrackingLog3.DisclosedDate > disclosureTrackingLog4.DisclosedDate)
                disclosureTrackingLog3 = disclosureTrackingLog4;
            }
            else if (disclosureTrackingLog3.DisclosedDate > newDisclosureSentTime)
              disclosureTrackingLog3 = disclosureTrackingLog4;
          }
        }
      }
      DateTime dateTime1 = DateTime.MinValue;
      DateTime dateTime2 = DateTime.MinValue;
      if (disclosureTrackingLog2 != null)
        dateTime1 = disclosureTrackingLog2.Guid == logGuid ? newReceivedDate : disclosureTrackingLog2.ReceivedDate;
      if (disclosureTrackingLog3 != null)
        dateTime2 = disclosureTrackingLog3.Guid == logGuid ? newReceivedDate : disclosureTrackingLog3.ReceivedDate;
      DateTime date = Utils.ParseDate((object) this.loan.GetField("3197"), DateTime.MinValue);
      if ((dateTime1 == DateTime.MinValue || dateTime2 == DateTime.MinValue) && date != DateTime.MinValue)
        return true;
      DateTime dateTime3 = dateTime1 > dateTime2 ? dateTime1 : dateTime2;
      return date < dateTime3;
    }

    public DisclosureTrackingLog GetInitialDisclosureTrackingLog(
      DisclosureTrackingLog.DisclosureTrackingType type)
    {
      return this.GetLatestDisclosureTrackingLog(false, type);
    }

    public DisclosureTrackingLog GetRedisclosureTrackingLog(
      DisclosureTrackingLog.DisclosureTrackingType type)
    {
      return this.GetLatestDisclosureTrackingLog(true, type);
    }

    private DisclosureTrackingLog GetLatestDisclosureTrackingLog(
      bool isRedisclosure,
      DisclosureTrackingLog.DisclosureTrackingType type)
    {
      DisclosureTrackingLog[] disclosureTrackingLog1 = this.GetAllDisclosureTrackingLog(true);
      DisclosureTrackingLog disclosureTrackingLog2 = (DisclosureTrackingLog) null;
      DisclosureTrackingLog disclosureTrackingLog3 = (DisclosureTrackingLog) null;
      foreach (DisclosureTrackingLog disclosureTrackingLog4 in disclosureTrackingLog1)
      {
        if (type == DisclosureTrackingLog.DisclosureTrackingType.GFE && disclosureTrackingLog4.DisclosedForGFE || type == DisclosureTrackingLog.DisclosureTrackingType.TIL && disclosureTrackingLog4.DisclosedForTIL)
        {
          if (disclosureTrackingLog2 == null)
            disclosureTrackingLog2 = disclosureTrackingLog4;
          else if (disclosureTrackingLog3 == null)
          {
            if (disclosureTrackingLog2.DisclosedDate > disclosureTrackingLog4.DisclosedDate)
            {
              disclosureTrackingLog3 = disclosureTrackingLog2;
              disclosureTrackingLog2 = disclosureTrackingLog4;
            }
            else
              disclosureTrackingLog3 = disclosureTrackingLog4;
          }
          else if (disclosureTrackingLog2.DisclosedDate > disclosureTrackingLog4.DisclosedDate)
            disclosureTrackingLog2 = disclosureTrackingLog4;
          else if (disclosureTrackingLog3.DisclosedDate < disclosureTrackingLog4.DisclosedDate)
            disclosureTrackingLog3 = disclosureTrackingLog4;
        }
      }
      return isRedisclosure ? disclosureTrackingLog3 : disclosureTrackingLog2;
    }

    public DisclosureTracking2015Log[] GetLatestRevisedDisclosureTrackingLogsByType(
      DisclosureTracking2015Log.DisclosureTrackingType leORcd,
      BorrowerPair[] brPairs)
    {
      DisclosureTracking2015Log[] disclosureTracking2015Log = this.GetAllDisclosureTracking2015Log(true, DisclosureTracking2015Log.DisclosureTrackingType.CD);
      List<DisclosureTracking2015Log> disclosureTracking2015LogList = new List<DisclosureTracking2015Log>();
      List<DisclosureTracking2015Log> list = ((IEnumerable<DisclosureTracking2015Log>) this.GetInitialDisclosureTrackingLogsByType(DisclosureTracking2015Log.DisclosureTrackingType.CD)).ToList<DisclosureTracking2015Log>();
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      if (disclosureTracking2015Log.Length == 0 || brPairs.Length == 0)
        return disclosureTracking2015LogList.ToArray();
      foreach (BorrowerPair brPair in brPairs)
        dictionary.Add(brPair.Id, false);
      DateTime dateTime = new DateTime();
      string str1 = "";
      string str2 = "";
      bool flag = false;
      for (int index = disclosureTracking2015Log.Length - 1; index >= 0; --index)
      {
        if (disclosureTracking2015Log[index].DisclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation && !list.Contains(disclosureTracking2015Log[index]))
        {
          if (!flag)
          {
            dateTime = disclosureTracking2015Log[index].DisclosedDate;
            str1 = disclosureTracking2015Log[index].GetDisclosedField("CD1.X69");
            str2 = disclosureTracking2015Log[index].GetDisclosedField("CD1.X1");
            flag = true;
          }
          if (dictionary.ContainsKey(disclosureTracking2015Log[index].BorrowerPairID) && !dictionary[disclosureTracking2015Log[index].BorrowerPairID] && disclosureTracking2015Log[index].GetDisclosedField("CD1.X69") == str1 && disclosureTracking2015Log[index].GetDisclosedField("CD1.X1") == str2)
          {
            disclosureTracking2015LogList.Add(disclosureTracking2015Log[index]);
            dictionary[disclosureTracking2015Log[index].BorrowerPairID] = true;
          }
          if (!dictionary.ContainsValue(false) && disclosureTracking2015Log[index].DisclosedDate < dateTime)
            break;
        }
      }
      return disclosureTracking2015LogList.ToArray();
    }

    public IDisclosureTracking2015Log[] GetLatestRevisedIDisclosureTrackingLogsByType(
      DisclosureTracking2015Log.DisclosureTrackingType leORcd,
      BorrowerPair[] brPairs)
    {
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = this.GetAllIDisclosureTracking2015Log(true, DisclosureTracking2015Log.DisclosureTrackingType.CD);
      List<IDisclosureTracking2015Log> disclosureTracking2015LogList = new List<IDisclosureTracking2015Log>();
      List<IDisclosureTracking2015Log> list = ((IEnumerable<IDisclosureTracking2015Log>) this.GetInitialIDisclosureTracking2015LogsByType(DisclosureTracking2015Log.DisclosureTrackingType.CD)).ToList<IDisclosureTracking2015Log>();
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      if (idisclosureTracking2015Log.Length == 0 || brPairs.Length == 0)
        return disclosureTracking2015LogList.ToArray();
      foreach (BorrowerPair brPair in brPairs)
        dictionary.Add(brPair.Id, false);
      DateTime dateTime = new DateTime();
      string str1 = "";
      string str2 = "";
      bool flag = false;
      for (int index = idisclosureTracking2015Log.Length - 1; index >= 0; --index)
      {
        if (idisclosureTracking2015Log[index].DisclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation && !list.Contains(idisclosureTracking2015Log[index]))
        {
          if (!flag)
          {
            dateTime = idisclosureTracking2015Log[index].DisclosedDate;
            str1 = idisclosureTracking2015Log[index].GetDisclosedField("CD1.X69");
            str2 = idisclosureTracking2015Log[index].GetDisclosedField("CD1.X1");
            flag = true;
          }
          if (dictionary.ContainsKey(idisclosureTracking2015Log[index].BorrowerPairID) && !dictionary[idisclosureTracking2015Log[index].BorrowerPairID] && idisclosureTracking2015Log[index].GetDisclosedField("CD1.X69") == str1 && idisclosureTracking2015Log[index].GetDisclosedField("CD1.X1") == str2)
          {
            disclosureTracking2015LogList.Add(idisclosureTracking2015Log[index]);
            dictionary[idisclosureTracking2015Log[index].BorrowerPairID] = true;
          }
          if (!dictionary.ContainsValue(false) && idisclosureTracking2015Log[index].DisclosedDate < dateTime)
            break;
        }
      }
      return disclosureTracking2015LogList.ToArray();
    }

    public DisclosureTracking2015Log GetDisclosureTrackingLogByType(
      DisclosureTracking2015Log.DisclosureTrackingType leORcd,
      DisclosureTracking2015Log.DisclosureTypeEnum disclosureType,
      bool summaryTimeline = false)
    {
      DisclosureTracking2015Log[] disclosureTracking2015Log1 = this.GetAllDisclosureTracking2015Log(true);
      DisclosureTracking2015Log trackingLogByType1 = (DisclosureTracking2015Log) null;
      DisclosureTracking2015Log trackingLogByType2 = (DisclosureTracking2015Log) null;
      DisclosureTracking2015Log trackingLogByType3 = (DisclosureTracking2015Log) null;
      ArrayList arrayList = new ArrayList();
      foreach (DisclosureTracking2015Log disclosureTracking2015Log2 in disclosureTracking2015Log1)
      {
        bool flag = arrayList.Contains((object) disclosureTracking2015Log2.BorrowerPairID);
        if (leORcd != DisclosureTracking2015Log.DisclosureTrackingType.CD || disclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.Initial && disclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.Revised || disclosureTracking2015Log2.DisclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation && (!summaryTimeline || disclosureTracking2015Log2.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder) && (!summaryTimeline || disclosureTracking2015Log2.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose))
        {
          if (disclosureTracking2015Log2.DisclosedForLE && leORcd == DisclosureTracking2015Log.DisclosureTrackingType.LE || disclosureTracking2015Log2.DisclosedForCD && leORcd == DisclosureTracking2015Log.DisclosureTrackingType.CD)
          {
            if (trackingLogByType1 == null)
              trackingLogByType1 = disclosureTracking2015Log2;
            else if (trackingLogByType2 == null)
            {
              if (trackingLogByType1.CompareDisclosedDate((IDisclosureTracking2015Log) disclosureTracking2015Log2) > 0)
              {
                if (flag)
                  trackingLogByType2 = trackingLogByType1;
                trackingLogByType1 = disclosureTracking2015Log2;
              }
              else if (flag)
                trackingLogByType2 = disclosureTracking2015Log2;
            }
            else if (trackingLogByType1.CompareDisclosedDate((IDisclosureTracking2015Log) disclosureTracking2015Log2) > 0)
              trackingLogByType1 = disclosureTracking2015Log2;
            else if (trackingLogByType2.CompareDisclosedDate((IDisclosureTracking2015Log) disclosureTracking2015Log2) < 0 && flag)
              trackingLogByType2 = disclosureTracking2015Log2;
            if (leORcd == DisclosureTracking2015Log.DisclosureTrackingType.CD && disclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation && disclosureTracking2015Log2.DisclosedForCD && disclosureTracking2015Log2.DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation)
            {
              if (trackingLogByType3 == null)
                trackingLogByType3 = disclosureTracking2015Log2;
              else if (trackingLogByType3.CompareDisclosedDate((IDisclosureTracking2015Log) disclosureTracking2015Log2) < 0)
                trackingLogByType3 = disclosureTracking2015Log2;
            }
          }
          if (disclosureTracking2015Log2.DisclosedForLE && leORcd == DisclosureTracking2015Log.DisclosureTrackingType.LE || disclosureTracking2015Log2.DisclosedForCD && leORcd == DisclosureTracking2015Log.DisclosureTrackingType.CD)
            arrayList.Add((object) disclosureTracking2015Log2.BorrowerPairID);
        }
      }
      switch (disclosureType)
      {
        case DisclosureTracking2015Log.DisclosureTypeEnum.Initial:
          return trackingLogByType1;
        case DisclosureTracking2015Log.DisclosureTypeEnum.Revised:
          return trackingLogByType2;
        case DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation:
          return trackingLogByType3;
        default:
          return (DisclosureTracking2015Log) null;
      }
    }

    public IDisclosureTracking2015Log GetIDisclosureTracking2015LogByType(
      DisclosureTracking2015Log.DisclosureTrackingType leORcd,
      DisclosureTracking2015Log.DisclosureTypeEnum disclosureType,
      bool summaryTimeline = false,
      string borrowerPairId = null)
    {
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = this.GetAllIDisclosureTracking2015Log(true);
      IDisclosureTracking2015Log tracking2015LogByType1 = (IDisclosureTracking2015Log) null;
      IDisclosureTracking2015Log tracking2015LogByType2 = (IDisclosureTracking2015Log) null;
      IDisclosureTracking2015Log tracking2015LogByType3 = (IDisclosureTracking2015Log) null;
      ArrayList arrayList = new ArrayList();
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in idisclosureTracking2015Log)
      {
        if (string.IsNullOrEmpty(borrowerPairId) || !(disclosureTracking2015Log.BorrowerPairID != borrowerPairId))
        {
          bool flag = arrayList.Contains((object) disclosureTracking2015Log.BorrowerPairID);
          if (leORcd != DisclosureTracking2015Log.DisclosureTrackingType.CD || disclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.Initial && disclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.Revised || disclosureTracking2015Log.DisclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation && (!summaryTimeline || disclosureTracking2015Log.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder) && (!summaryTimeline || disclosureTracking2015Log.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose))
          {
            if (disclosureTracking2015Log.DisclosedForLE && leORcd == DisclosureTracking2015Log.DisclosureTrackingType.LE || disclosureTracking2015Log.DisclosedForCD && leORcd == DisclosureTracking2015Log.DisclosureTrackingType.CD)
            {
              if (tracking2015LogByType1 == null)
                tracking2015LogByType1 = disclosureTracking2015Log;
              else if (tracking2015LogByType2 == null)
              {
                if (tracking2015LogByType1.CompareDisclosedDate(disclosureTracking2015Log) > 0)
                {
                  if (flag)
                    tracking2015LogByType2 = tracking2015LogByType1;
                  tracking2015LogByType1 = disclosureTracking2015Log;
                }
                else if (flag)
                  tracking2015LogByType2 = disclosureTracking2015Log;
              }
              else if (tracking2015LogByType1.CompareDisclosedDate(disclosureTracking2015Log) > 0)
                tracking2015LogByType1 = disclosureTracking2015Log;
              else if (tracking2015LogByType2.CompareDisclosedDate(disclosureTracking2015Log) < 0 && flag)
                tracking2015LogByType2 = disclosureTracking2015Log;
              if (leORcd == DisclosureTracking2015Log.DisclosureTrackingType.CD && disclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation && disclosureTracking2015Log.DisclosedForCD && disclosureTracking2015Log.DisclosureType == DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation)
              {
                if (tracking2015LogByType3 == null)
                  tracking2015LogByType3 = disclosureTracking2015Log;
                else if (tracking2015LogByType3.CompareDisclosedDate(disclosureTracking2015Log) < 0)
                  tracking2015LogByType3 = disclosureTracking2015Log;
              }
            }
            if (disclosureTracking2015Log.DisclosedForLE && leORcd == DisclosureTracking2015Log.DisclosureTrackingType.LE || disclosureTracking2015Log.DisclosedForCD && leORcd == DisclosureTracking2015Log.DisclosureTrackingType.CD)
              arrayList.Add((object) disclosureTracking2015Log.BorrowerPairID);
          }
        }
      }
      switch (disclosureType)
      {
        case DisclosureTracking2015Log.DisclosureTypeEnum.Initial:
          return tracking2015LogByType1;
        case DisclosureTracking2015Log.DisclosureTypeEnum.Revised:
          return tracking2015LogByType2;
        case DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation:
          return tracking2015LogByType3;
        default:
          return (IDisclosureTracking2015Log) null;
      }
    }

    public string[] borrowerPairIDs(
      DisclosureTracking2015Log.DisclosureTrackingType type)
    {
      DisclosureTracking2015Log[] disclosureTracking2015Log1 = this.GetAllDisclosureTracking2015Log(true);
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      ArrayList arrayList = new ArrayList();
      if (type == DisclosureTracking2015Log.DisclosureTrackingType.LE)
      {
        foreach (DisclosureTracking2015Log disclosureTracking2015Log2 in disclosureTracking2015Log1)
        {
          if (disclosureTracking2015Log2.DisclosedForLE && !dictionary.ContainsKey(disclosureTracking2015Log2.BorrowerPairID))
          {
            dictionary.Add(disclosureTracking2015Log2.BorrowerPairID, 1);
            arrayList.Add((object) disclosureTracking2015Log2.BorrowerPairID);
          }
        }
      }
      if (type == DisclosureTracking2015Log.DisclosureTrackingType.CD)
      {
        foreach (DisclosureTracking2015Log disclosureTracking2015Log3 in disclosureTracking2015Log1)
        {
          if (disclosureTracking2015Log3.DisclosedForCD && !dictionary.ContainsKey(disclosureTracking2015Log3.BorrowerPairID))
          {
            dictionary.Add(disclosureTracking2015Log3.BorrowerPairID, 1);
            arrayList.Add((object) disclosureTracking2015Log3.BorrowerPairID);
          }
        }
      }
      return (string[]) arrayList.ToArray(typeof (string));
    }

    public string[] borrowerPairIDsForIDisclosureLog(
      DisclosureTracking2015Log.DisclosureTrackingType type)
    {
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = this.GetAllIDisclosureTracking2015Log(true);
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      ArrayList arrayList = new ArrayList();
      if (type == DisclosureTracking2015Log.DisclosureTrackingType.LE)
      {
        foreach (IDisclosureTracking2015Log disclosureTracking2015Log in idisclosureTracking2015Log)
        {
          if (disclosureTracking2015Log.DisclosedForLE && !dictionary.ContainsKey(disclosureTracking2015Log.BorrowerPairID))
          {
            dictionary.Add(disclosureTracking2015Log.BorrowerPairID, 1);
            arrayList.Add((object) disclosureTracking2015Log.BorrowerPairID);
          }
        }
      }
      if (type == DisclosureTracking2015Log.DisclosureTrackingType.CD)
      {
        foreach (IDisclosureTracking2015Log disclosureTracking2015Log in idisclosureTracking2015Log)
        {
          if (disclosureTracking2015Log.DisclosedForCD && !dictionary.ContainsKey(disclosureTracking2015Log.BorrowerPairID))
          {
            dictionary.Add(disclosureTracking2015Log.BorrowerPairID, 1);
            arrayList.Add((object) disclosureTracking2015Log.BorrowerPairID);
          }
        }
      }
      return (string[]) arrayList.ToArray(typeof (string));
    }

    public DisclosureTracking2015Log[] GetInitialDisclosureTrackingLogsByType(
      DisclosureTracking2015Log.DisclosureTrackingType leORcd)
    {
      DisclosureTracking2015Log[] disclosureTracking2015Log1 = this.GetAllDisclosureTracking2015Log(true);
      string[] strArray = this.borrowerPairIDs(leORcd);
      ArrayList arrayList = new ArrayList();
      foreach (string str in strArray)
      {
        DisclosureTracking2015Log disclosureTracking2015Log2 = (DisclosureTracking2015Log) null;
        DisclosureTracking2015Log disclosureTracking2015Log3 = (DisclosureTracking2015Log) null;
        foreach (DisclosureTracking2015Log disclosureTracking2015Log4 in disclosureTracking2015Log1)
        {
          if (disclosureTracking2015Log4.BorrowerPairID == str && (disclosureTracking2015Log4.DisclosedForLE && leORcd == DisclosureTracking2015Log.DisclosureTrackingType.LE || disclosureTracking2015Log4.DisclosedForCD && leORcd == DisclosureTracking2015Log.DisclosureTrackingType.CD) && disclosureTracking2015Log4.DisclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation && (leORcd != DisclosureTracking2015Log.DisclosureTrackingType.CD || disclosureTracking2015Log4.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder) && (leORcd != DisclosureTracking2015Log.DisclosureTrackingType.CD || disclosureTracking2015Log4.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose))
          {
            if (disclosureTracking2015Log2 == null)
              disclosureTracking2015Log2 = disclosureTracking2015Log4;
            else if (disclosureTracking2015Log3 == null)
            {
              if (disclosureTracking2015Log2.CompareDisclosedDate((IDisclosureTracking2015Log) disclosureTracking2015Log4) > 0)
              {
                disclosureTracking2015Log3 = disclosureTracking2015Log2;
                disclosureTracking2015Log2 = disclosureTracking2015Log4;
              }
              else
                disclosureTracking2015Log3 = disclosureTracking2015Log4;
            }
            else if (disclosureTracking2015Log2.CompareDisclosedDate((IDisclosureTracking2015Log) disclosureTracking2015Log4) > 0)
              disclosureTracking2015Log2 = disclosureTracking2015Log4;
            else if (disclosureTracking2015Log3.CompareDisclosedDate((IDisclosureTracking2015Log) disclosureTracking2015Log4) < 0)
              disclosureTracking2015Log3 = disclosureTracking2015Log4;
          }
        }
        if (disclosureTracking2015Log2 != null)
          arrayList.Add((object) disclosureTracking2015Log2);
      }
      return (DisclosureTracking2015Log[]) arrayList.ToArray(typeof (DisclosureTracking2015Log));
    }

    public IDisclosureTracking2015Log[] GetInitialIDisclosureTracking2015LogsByType(
      DisclosureTracking2015Log.DisclosureTrackingType leORcd)
    {
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = this.GetAllIDisclosureTracking2015Log(true);
      string[] strArray = this.borrowerPairIDsForIDisclosureLog(leORcd);
      ArrayList arrayList = new ArrayList();
      foreach (string str in strArray)
      {
        IDisclosureTracking2015Log disclosureTracking2015Log1 = (IDisclosureTracking2015Log) null;
        IDisclosureTracking2015Log disclosureTracking2015Log2 = (IDisclosureTracking2015Log) null;
        foreach (IDisclosureTracking2015Log disclosureTracking2015Log3 in idisclosureTracking2015Log)
        {
          if (disclosureTracking2015Log3.BorrowerPairID == str && (disclosureTracking2015Log3.DisclosedForLE && leORcd == DisclosureTracking2015Log.DisclosureTrackingType.LE || disclosureTracking2015Log3.DisclosedForCD && leORcd == DisclosureTracking2015Log.DisclosureTrackingType.CD) && disclosureTracking2015Log3.DisclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation && (leORcd != DisclosureTracking2015Log.DisclosureTrackingType.CD || disclosureTracking2015Log3.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder) && (leORcd != DisclosureTracking2015Log.DisclosureTrackingType.CD || disclosureTracking2015Log3.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose))
          {
            if (disclosureTracking2015Log1 == null)
              disclosureTracking2015Log1 = disclosureTracking2015Log3;
            else if (disclosureTracking2015Log2 == null)
            {
              if (disclosureTracking2015Log1.CompareDisclosedDate(disclosureTracking2015Log3) > 0)
              {
                disclosureTracking2015Log2 = disclosureTracking2015Log1;
                disclosureTracking2015Log1 = disclosureTracking2015Log3;
              }
              else
                disclosureTracking2015Log2 = disclosureTracking2015Log3;
            }
            else if (disclosureTracking2015Log1.CompareDisclosedDate(disclosureTracking2015Log3) > 0)
              disclosureTracking2015Log1 = disclosureTracking2015Log3;
            else if (disclosureTracking2015Log2.CompareDisclosedDate(disclosureTracking2015Log3) < 0)
              disclosureTracking2015Log2 = disclosureTracking2015Log3;
          }
        }
        if (disclosureTracking2015Log1 != null)
          arrayList.Add((object) disclosureTracking2015Log1);
      }
      return (IDisclosureTracking2015Log[]) arrayList.ToArray(typeof (IDisclosureTracking2015Log));
    }

    public DisclosureTracking2015Log[] GetRevisedDisclosureTrackingLogsByType(
      DisclosureTracking2015Log.DisclosureTrackingType leORcd)
    {
      DisclosureTracking2015Log[] disclosureTracking2015Log1 = this.GetAllDisclosureTracking2015Log(true);
      string[] strArray = this.borrowerPairIDs(leORcd);
      ArrayList arrayList = new ArrayList();
      foreach (string str in strArray)
      {
        DisclosureTracking2015Log disclosureTracking2015Log2 = (DisclosureTracking2015Log) null;
        DisclosureTracking2015Log disclosureTracking2015Log3 = (DisclosureTracking2015Log) null;
        foreach (DisclosureTracking2015Log disclosureTracking2015Log4 in disclosureTracking2015Log1)
        {
          if (disclosureTracking2015Log4.BorrowerPairID == str && (disclosureTracking2015Log4.DisclosedForLE && leORcd == DisclosureTracking2015Log.DisclosureTrackingType.LE || disclosureTracking2015Log4.DisclosedForCD && leORcd == DisclosureTracking2015Log.DisclosureTrackingType.CD))
          {
            if (disclosureTracking2015Log2 == null)
              disclosureTracking2015Log2 = disclosureTracking2015Log4;
            else if (disclosureTracking2015Log3 == null)
            {
              if (disclosureTracking2015Log2.CompareDisclosedDate((IDisclosureTracking2015Log) disclosureTracking2015Log4) > 0)
              {
                disclosureTracking2015Log3 = disclosureTracking2015Log2;
                disclosureTracking2015Log2 = disclosureTracking2015Log4;
              }
              else
                disclosureTracking2015Log3 = disclosureTracking2015Log4;
            }
            else if (disclosureTracking2015Log2.CompareDisclosedDate((IDisclosureTracking2015Log) disclosureTracking2015Log4) > 0)
              disclosureTracking2015Log2 = disclosureTracking2015Log4;
            else if (disclosureTracking2015Log3.CompareDisclosedDate((IDisclosureTracking2015Log) disclosureTracking2015Log4) < 0)
              disclosureTracking2015Log3 = disclosureTracking2015Log4;
          }
        }
        if (disclosureTracking2015Log3 != null)
          arrayList.Add((object) disclosureTracking2015Log3);
      }
      return (DisclosureTracking2015Log[]) arrayList.ToArray(typeof (DisclosureTracking2015Log));
    }

    public IDisclosureTracking2015Log[] GetRevisedIDisclosureTracking2015LogsByType(
      DisclosureTracking2015Log.DisclosureTrackingType leORcd)
    {
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = this.GetAllIDisclosureTracking2015Log(true);
      string[] strArray = this.borrowerPairIDsForIDisclosureLog(leORcd);
      ArrayList arrayList = new ArrayList();
      foreach (string str in strArray)
      {
        IDisclosureTracking2015Log disclosureTracking2015Log1 = (IDisclosureTracking2015Log) null;
        IDisclosureTracking2015Log disclosureTracking2015Log2 = (IDisclosureTracking2015Log) null;
        foreach (IDisclosureTracking2015Log disclosureTracking2015Log3 in idisclosureTracking2015Log)
        {
          if (disclosureTracking2015Log3.BorrowerPairID == str && (disclosureTracking2015Log3.DisclosedForLE && leORcd == DisclosureTracking2015Log.DisclosureTrackingType.LE || disclosureTracking2015Log3.DisclosedForCD && leORcd == DisclosureTracking2015Log.DisclosureTrackingType.CD))
          {
            if (disclosureTracking2015Log1 == null)
              disclosureTracking2015Log1 = disclosureTracking2015Log3;
            else if (disclosureTracking2015Log2 == null)
            {
              if (disclosureTracking2015Log1.CompareDisclosedDate(disclosureTracking2015Log3) > 0)
              {
                disclosureTracking2015Log2 = disclosureTracking2015Log1;
                disclosureTracking2015Log1 = disclosureTracking2015Log3;
              }
              else
                disclosureTracking2015Log2 = disclosureTracking2015Log3;
            }
            else if (disclosureTracking2015Log1.CompareDisclosedDate(disclosureTracking2015Log3) > 0)
              disclosureTracking2015Log1 = disclosureTracking2015Log3;
            else if (disclosureTracking2015Log2.CompareDisclosedDate(disclosureTracking2015Log3) < 0)
              disclosureTracking2015Log2 = disclosureTracking2015Log3;
          }
        }
        if (disclosureTracking2015Log2 != null)
          arrayList.Add((object) disclosureTracking2015Log2);
      }
      return (IDisclosureTracking2015Log[]) arrayList.ToArray(typeof (IDisclosureTracking2015Log));
    }

    public void RemoveVerif(string verifId)
    {
      VerifLog verif = this.GetVerif(verifId);
      if (verif == null)
        return;
      this.RemoveRecord((LogRecordBase) verif);
    }

    public VerifLog GetVerif(string verifId)
    {
      foreach (VerifLog verif in this.GetAllRecordsOfType(typeof (VerifLog)))
      {
        if (verif.Id == verifId)
          return verif;
      }
      return (VerifLog) null;
    }

    public DocumentLog[] GetDocumentsByStage(string stage, IEnumerable<EllieMae.EMLite.Workflow.Milestone> ms)
    {
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (DocumentLog documentLog in this.GetAllRecordsOfType(typeof (DocumentLog)))
      {
        DocumentLog doc = documentLog;
        if (this.GetMilestoneByName(doc.Stage) != null)
        {
          if (doc.Stage == stage)
            documentLogList.Add(doc);
        }
        else
        {
          EllieMae.EMLite.Workflow.Milestone milestone = ms.FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (item => item.Name == doc.Stage));
          if (milestone != null && milestone.MilestoneID == this.GetMilestoneByName(stage).MilestoneID)
            documentLogList.Add(doc);
        }
      }
      return documentLogList.ToArray();
    }

    public DocumentOrderLog[] GetDocumentOrdersByType(DocumentOrderType orderType)
    {
      List<DocumentOrderLog> documentOrderLogList = new List<DocumentOrderLog>();
      foreach (DocumentOrderLog documentOrderLog in this.GetAllRecordsOfType(typeof (DocumentOrderLog)))
      {
        if (documentOrderLog.OrderType == orderType)
          documentOrderLogList.Add(documentOrderLog);
      }
      documentOrderLogList.Sort();
      return documentOrderLogList.ToArray();
    }

    public PrintLog[] GetAllPrintLog()
    {
      List<PrintLog> printLogList = new List<PrintLog>();
      foreach (LogRecordBase record in this.records)
      {
        if (record is PrintLog)
          printLogList.Add((PrintLog) record);
      }
      return printLogList.ToArray();
    }

    public ExportLog[] GetAllExportLog()
    {
      List<ExportLog> exportLogList = new List<ExportLog>();
      foreach (LogRecordBase record in this.records)
      {
        if (record is ExportLog)
          exportLogList.Add((ExportLog) record);
      }
      return exportLogList.ToArray();
    }

    public DisclosureTrackingLog[] GetAllEDisclosureTrackingLog(bool includeDisclosedLogOnly)
    {
      List<DisclosureTrackingLog> disclosureTrackingLogList = new List<DisclosureTrackingLog>();
      foreach (LogRecordBase record in this.records)
      {
        if (record is DisclosureTrackingLog)
        {
          DisclosureTrackingLog disclosureTrackingLog = (DisclosureTrackingLog) record;
          if (disclosureTrackingLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure && (!includeDisclosedLogOnly || disclosureTrackingLog.IsDisclosed))
            disclosureTrackingLogList.Add(disclosureTrackingLog);
        }
      }
      disclosureTrackingLogList.Sort();
      return disclosureTrackingLogList.ToArray();
    }

    public DisclosureTracking2015Log[] GetAllEDisclosureTracking2015Log(bool includeDisclosedLogOnly)
    {
      List<DisclosureTracking2015Log> disclosureTracking2015LogList = new List<DisclosureTracking2015Log>();
      foreach (LogRecordBase record in this.records)
      {
        if (record is DisclosureTracking2015Log)
        {
          DisclosureTracking2015Log disclosureTracking2015Log = (DisclosureTracking2015Log) record;
          if (disclosureTracking2015Log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure && (!includeDisclosedLogOnly || disclosureTracking2015Log.IsDisclosed))
            disclosureTracking2015LogList.Add(disclosureTracking2015Log);
        }
      }
      disclosureTracking2015LogList.Sort();
      return disclosureTracking2015LogList.ToArray();
    }

    public HtmlEmailLog GetHTMLEmailLog(string linkedRefId)
    {
      return Enumerable.OfType<HtmlEmailLog>(this.records.ToArray()).Where<HtmlEmailLog>((Func<HtmlEmailLog, bool>) (log => log.LinkedRefId == linkedRefId)).FirstOrDefault<HtmlEmailLog>();
    }

    public IDisclosureTracking2015Log[] GetAllEDisclosureTracking2015EnhancedLog(
      bool includeDisclosedLogOnly)
    {
      List<IDisclosureTracking2015Log> disclosureTracking2015LogList = new List<IDisclosureTracking2015Log>();
      foreach (LogRecordBase record in this.records)
      {
        if (record is DisclosureTracking2015Log || record is EnhancedDisclosureTracking2015Log)
        {
          IDisclosureTracking2015Log disclosureTracking2015Log = (IDisclosureTracking2015Log) record;
          if (disclosureTracking2015Log.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure && (!includeDisclosedLogOnly || disclosureTracking2015Log.IsDisclosed))
            disclosureTracking2015LogList.Add(disclosureTracking2015Log);
        }
      }
      disclosureTracking2015LogList.Sort();
      return disclosureTracking2015LogList.ToArray();
    }

    public DisclosureTrackingLog[] GetAllDisclosureTrackingLog(bool includeDisclosedLogOnly)
    {
      List<DisclosureTrackingLog> disclosureTrackingLogList = new List<DisclosureTrackingLog>();
      foreach (LogRecordBase record in this.records)
      {
        if (record is DisclosureTrackingLog)
        {
          DisclosureTrackingLog disclosureTrackingLog = (DisclosureTrackingLog) record;
          if (!includeDisclosedLogOnly || disclosureTrackingLog.IsDisclosed)
            disclosureTrackingLogList.Add(disclosureTrackingLog);
        }
      }
      return disclosureTrackingLogList.ToArray();
    }

    public DisclosureTrackingLog[] GetAllDisclosureTrackingLog(
      bool includeDisclosedLogOnly,
      DisclosureTrackingLog.DisclosureTrackingType type)
    {
      List<DisclosureTrackingLog> disclosureTrackingLogList = new List<DisclosureTrackingLog>();
      foreach (LogRecordBase record in this.records)
      {
        if (record is DisclosureTrackingLog)
        {
          DisclosureTrackingLog disclosureTrackingLog = (DisclosureTrackingLog) record;
          if ((!includeDisclosedLogOnly || disclosureTrackingLog.IsDisclosed) && (type != DisclosureTrackingLog.DisclosureTrackingType.GFE || disclosureTrackingLog.DisclosedForGFE) && (type != DisclosureTrackingLog.DisclosureTrackingType.TIL || disclosureTrackingLog.DisclosedForTIL))
            disclosureTrackingLogList.Add(disclosureTrackingLog);
        }
      }
      disclosureTrackingLogList.Sort();
      return disclosureTrackingLogList.ToArray();
    }

    public DisclosureTracking2015Log[] GetAllDisclosureTracking2015Log(
      bool includeDisclosedLogOnly,
      DisclosureTracking2015Log.DisclosureTrackingType type)
    {
      List<DisclosureTracking2015Log> source = new List<DisclosureTracking2015Log>();
      foreach (LogRecordBase record in this.records)
      {
        if (record is DisclosureTracking2015Log)
        {
          DisclosureTracking2015Log disclosureTracking2015Log = (DisclosureTracking2015Log) record;
          if ((!includeDisclosedLogOnly || disclosureTracking2015Log.IsDisclosed) && (type != DisclosureTracking2015Log.DisclosureTrackingType.LE || disclosureTracking2015Log.DisclosedForLE) && (type != DisclosureTracking2015Log.DisclosureTrackingType.CD || disclosureTracking2015Log.DisclosedForCD))
            source.Add(disclosureTracking2015Log);
        }
      }
      try
      {
        return source.OrderByDescending<DisclosureTracking2015Log, DateTime>((Func<DisclosureTracking2015Log, DateTime>) (x => x.DisclosedDate)).ThenByDescending<DisclosureTracking2015Log, DateTime>((Func<DisclosureTracking2015Log, DateTime>) (x => x.DateAdded)).ToArray<DisclosureTracking2015Log>();
      }
      catch (Exception ex)
      {
        return source.ToArray();
      }
    }

    public IDisclosureTracking2015Log[] GetAllIDisclosureTracking2015Log(
      bool includeDisclosedLogOnly,
      DisclosureTracking2015Log.DisclosureTrackingType type)
    {
      List<IDisclosureTracking2015Log> source = new List<IDisclosureTracking2015Log>();
      foreach (LogRecordBase record in this.records)
      {
        if (record is IDisclosureTracking2015Log)
        {
          IDisclosureTracking2015Log disclosureTracking2015Log = (IDisclosureTracking2015Log) record;
          if ((!includeDisclosedLogOnly || disclosureTracking2015Log.IsDisclosed) && (type != DisclosureTracking2015Log.DisclosureTrackingType.LE || disclosureTracking2015Log.DisclosedForLE) && (type != DisclosureTracking2015Log.DisclosureTrackingType.CD || disclosureTracking2015Log.DisclosedForCD))
            source.Add(disclosureTracking2015Log);
        }
      }
      try
      {
        return source.OrderByDescending<IDisclosureTracking2015Log, DateTime>((Func<IDisclosureTracking2015Log, DateTime>) (x => x.DisclosedDate)).ThenByDescending<IDisclosureTracking2015Log, DateTime>((Func<IDisclosureTracking2015Log, DateTime>) (x => x.DateAdded)).ToArray<IDisclosureTracking2015Log>();
      }
      catch (Exception ex)
      {
        return source.ToArray();
      }
    }

    public IDisclosureTracking2015Log[] GetIDisclosureTracking2015LogsByBorrowerPairId(
      bool includeDisclosedLogOnly,
      DisclosureTracking2015Log.DisclosureTrackingType type,
      string borrowerPairId)
    {
      List<IDisclosureTracking2015Log> disclosureTracking2015LogList = new List<IDisclosureTracking2015Log>();
      foreach (LogRecordBase record in this.records)
      {
        if (record is IDisclosureTracking2015Log)
        {
          IDisclosureTracking2015Log disclosureTracking2015Log = (IDisclosureTracking2015Log) record;
          if ((string.IsNullOrEmpty(borrowerPairId) || !(disclosureTracking2015Log.BorrowerPairID != borrowerPairId)) && (!includeDisclosedLogOnly || disclosureTracking2015Log.IsDisclosed) && (type != DisclosureTracking2015Log.DisclosureTrackingType.LE || disclosureTracking2015Log.DisclosedForLE) && (type != DisclosureTracking2015Log.DisclosureTrackingType.CD || disclosureTracking2015Log.DisclosedForCD))
            disclosureTracking2015LogList.Add(disclosureTracking2015Log);
        }
      }
      return disclosureTracking2015LogList.ToArray();
    }

    public DisclosureTrackingLog GetLatestDisclosureTrackingLog(
      DisclosureTrackingLog.DisclosureTrackingType type)
    {
      DisclosureTrackingLog disclosureTrackingLog1 = (DisclosureTrackingLog) null;
      foreach (LogRecordBase record in this.records)
      {
        if (record is DisclosureTrackingLog)
        {
          DisclosureTrackingLog disclosureTrackingLog2 = (DisclosureTrackingLog) record;
          if (disclosureTrackingLog2.IsDisclosed && (type != DisclosureTrackingLog.DisclosureTrackingType.GFE || disclosureTrackingLog2.DisclosedForGFE) && (type != DisclosureTrackingLog.DisclosureTrackingType.TIL || disclosureTrackingLog2.DisclosedForTIL))
          {
            if (disclosureTrackingLog1 == null)
              disclosureTrackingLog1 = disclosureTrackingLog2;
            else if (disclosureTrackingLog1.DisclosedDate.Date < disclosureTrackingLog2.DisclosedDate.Date)
              disclosureTrackingLog1 = disclosureTrackingLog2;
            else if (disclosureTrackingLog1.DisclosedDate.Date == disclosureTrackingLog2.DisclosedDate.Date && disclosureTrackingLog1.DateAdded < disclosureTrackingLog2.DateAdded)
              disclosureTrackingLog1 = disclosureTrackingLog2;
          }
        }
      }
      return disclosureTrackingLog1;
    }

    public DisclosureTracking2015Log GetDisclosureTracking2015LogByGUID(string guid)
    {
      return (DisclosureTracking2015Log) this.GetRecordByID(guid);
    }

    public IDisclosureTracking2015Log GetIDisclosureTracking2015LogByGUID(string guid)
    {
      return (IDisclosureTracking2015Log) this.GetRecordByID(guid);
    }

    public DisclosureTracking2015Log GetLatestDisclosureTracking2015Log(
      DisclosureTracking2015Log.DisclosureTrackingType type,
      bool summaryTimeline = false)
    {
      DisclosureTracking2015Log disclosureTracking2015Log1 = (DisclosureTracking2015Log) null;
      foreach (LogRecordBase record in this.records)
      {
        if (record is DisclosureTracking2015Log)
        {
          DisclosureTracking2015Log disclosureTracking2015Log2 = (DisclosureTracking2015Log) record;
          if (disclosureTracking2015Log2.IsDisclosed && (!(type == DisclosureTracking2015Log.DisclosureTrackingType.CD & summaryTimeline) || disclosureTracking2015Log2.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder) && (!(type == DisclosureTracking2015Log.DisclosureTrackingType.CD & summaryTimeline) || disclosureTracking2015Log2.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose) && (type != DisclosureTracking2015Log.DisclosureTrackingType.CD || disclosureTracking2015Log2.DisclosedForCD) && (type != DisclosureTracking2015Log.DisclosureTrackingType.LE || disclosureTracking2015Log2.DisclosedForLE))
          {
            if (disclosureTracking2015Log1 == null)
              disclosureTracking2015Log1 = disclosureTracking2015Log2;
            else if (disclosureTracking2015Log1.DisclosedDate.Date < disclosureTracking2015Log2.DisclosedDate.Date)
              disclosureTracking2015Log1 = disclosureTracking2015Log2;
            else if (disclosureTracking2015Log1.DisclosedDate.Date == disclosureTracking2015Log2.DisclosedDate.Date && disclosureTracking2015Log1.DateAdded < disclosureTracking2015Log2.DateAdded)
              disclosureTracking2015Log1 = disclosureTracking2015Log2;
          }
        }
      }
      return disclosureTracking2015Log1;
    }

    public IDisclosureTracking2015Log GetLatestIDisclosureTracking2015Log(
      DisclosureTracking2015Log.DisclosureTrackingType type,
      bool summaryTimeline = false)
    {
      IDisclosureTracking2015Log idisclosureTracking2015Log = (IDisclosureTracking2015Log) null;
      foreach (LogRecordBase record in this.records)
      {
        if (record is IDisclosureTracking2015Log)
        {
          IDisclosureTracking2015Log disclosureTracking2015Log = (IDisclosureTracking2015Log) record;
          if (disclosureTracking2015Log.IsDisclosed && (!(type == DisclosureTracking2015Log.DisclosureTrackingType.CD & summaryTimeline) || disclosureTracking2015Log.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder) && (!(type == DisclosureTracking2015Log.DisclosureTrackingType.CD & summaryTimeline) || disclosureTracking2015Log.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose) && (type != DisclosureTracking2015Log.DisclosureTrackingType.CD || disclosureTracking2015Log.DisclosedForCD) && (type != DisclosureTracking2015Log.DisclosureTrackingType.LE || disclosureTracking2015Log.DisclosedForLE))
          {
            if (idisclosureTracking2015Log == null)
              idisclosureTracking2015Log = disclosureTracking2015Log;
            else if (idisclosureTracking2015Log.DisclosedDate.Date < disclosureTracking2015Log.DisclosedDate.Date)
              idisclosureTracking2015Log = disclosureTracking2015Log;
            else if (idisclosureTracking2015Log.DisclosedDate.Date == disclosureTracking2015Log.DisclosedDate.Date && idisclosureTracking2015Log.DateAdded < disclosureTracking2015Log.DateAdded)
              idisclosureTracking2015Log = disclosureTracking2015Log;
          }
        }
      }
      return idisclosureTracking2015Log;
    }

    public DisclosureTracking2015Log[] GetAllDisclosureTracking2015Log(bool includeDisclosedLogOnly)
    {
      List<DisclosureTracking2015Log> disclosureTracking2015LogList = new List<DisclosureTracking2015Log>();
      foreach (LogRecordBase record in this.records)
      {
        if (record is DisclosureTracking2015Log)
        {
          DisclosureTracking2015Log disclosureTracking2015Log = (DisclosureTracking2015Log) record;
          if (!includeDisclosedLogOnly || disclosureTracking2015Log.IsDisclosed)
            disclosureTracking2015LogList.Add(disclosureTracking2015Log);
        }
      }
      return disclosureTracking2015LogList.ToArray();
    }

    public IDisclosureTracking2015Log[] GetAllIDisclosureTracking2015Log(
      bool includeDisclosedLogOnly)
    {
      List<IDisclosureTracking2015Log> disclosureTracking2015LogList = new List<IDisclosureTracking2015Log>();
      foreach (LogRecordBase record in this.records)
      {
        if (record is IDisclosureTracking2015Log)
        {
          IDisclosureTracking2015Log disclosureTracking2015Log = (IDisclosureTracking2015Log) record;
          if (!includeDisclosedLogOnly || disclosureTracking2015Log.IsDisclosed)
            disclosureTracking2015LogList.Add(disclosureTracking2015Log);
        }
      }
      return disclosureTracking2015LogList.ToArray();
    }

    public DisclosureTracking2015Log[] GetAllDisclosureTracking2015LogWithCoc()
    {
      List<DisclosureTracking2015Log> disclosureTracking2015LogList = new List<DisclosureTracking2015Log>();
      bool flag = false;
      foreach (LogRecordBase record in this.records)
      {
        if (record is DisclosureTracking2015Log)
        {
          DisclosureTracking2015Log disclosureTracking2015Log = (DisclosureTracking2015Log) record;
          string disclosedField1 = disclosureTracking2015Log.GetDisclosedField("3168");
          string disclosedField2 = disclosureTracking2015Log.GetDisclosedField("CD1.X61");
          if (flag && disclosureTracking2015Log.IsDisclosed || disclosureTracking2015Log.IsDisclosed && (disclosedField1.Equals("Y") || disclosedField2.Equals("Y")))
            disclosureTracking2015LogList.Add(disclosureTracking2015Log);
          if (!disclosureTracking2015Log.IsDisclosed && (disclosedField1.Equals("Y") || disclosedField2.Equals("Y")))
            flag = true;
        }
      }
      return disclosureTracking2015LogList.ToArray();
    }

    public IDisclosureTracking2015Log[] GetAllIDisclosureTracking2015LogWithCoc()
    {
      List<IDisclosureTracking2015Log> disclosureTracking2015LogList = new List<IDisclosureTracking2015Log>();
      bool flag = false;
      foreach (LogRecordBase record in this.records)
      {
        if (record is DisclosureTracking2015Log || record is EnhancedDisclosureTracking2015Log)
        {
          IDisclosureTracking2015Log disclosureTracking2015Log = (IDisclosureTracking2015Log) record;
          string disclosedField1 = disclosureTracking2015Log.GetDisclosedField("3168");
          string disclosedField2 = disclosureTracking2015Log.GetDisclosedField("CD1.X61");
          if (flag && disclosureTracking2015Log.IsDisclosed || disclosureTracking2015Log.IsDisclosed && (disclosedField1.Equals("Y") || disclosedField2.Equals("Y")))
            disclosureTracking2015LogList.Add(disclosureTracking2015Log);
          if (!disclosureTracking2015Log.IsDisclosed && (disclosedField1.Equals("Y") || disclosedField2.Equals("Y")))
            flag = true;
        }
      }
      return disclosureTracking2015LogList.ToArray();
    }

    public bool IsThereAny2015DisclosureTracking()
    {
      foreach (LogRecordBase record in this.records)
      {
        if (record is IDisclosureTracking2015Log && ((IDisclosureTracking2015Log) record).IsDisclosed)
          return true;
      }
      return false;
    }

    public bool IsThereAnyDisclosureTracking()
    {
      foreach (LogRecordBase record in this.records)
      {
        if (record is IDisclosureTrackingLog)
          return true;
      }
      return false;
    }

    public IDisclosureTracking2015Log GetFirst2015IDisclosureTracking()
    {
      foreach (LogRecordBase record in this.records)
      {
        if (record is IDisclosureTracking2015Log)
        {
          IDisclosureTracking2015Log idisclosureTracking = (IDisclosureTracking2015Log) record;
          if (idisclosureTracking.IsDisclosed)
            return idisclosureTracking;
        }
      }
      return (IDisclosureTracking2015Log) null;
    }

    public DisclosureTracking2015Log GetFirst2015DisclosureTracking()
    {
      foreach (LogRecordBase record in this.records)
      {
        if (record is DisclosureTracking2015Log)
        {
          DisclosureTracking2015Log disclosureTracking = (DisclosureTracking2015Log) record;
          if (disclosureTracking.IsDisclosed)
            return disclosureTracking;
        }
      }
      return (DisclosureTracking2015Log) null;
    }

    public IDisclosureTracking2015Log GetEarliestSSPLIDisclosureTracking2015Log()
    {
      IDisclosureTracking2015Log disclosureTracking2015Log1 = (IDisclosureTracking2015Log) null;
      foreach (LogRecordBase record in this.records)
      {
        if (record is IDisclosureTracking2015Log)
        {
          IDisclosureTracking2015Log disclosureTracking2015Log2 = (IDisclosureTracking2015Log) record;
          if (disclosureTracking2015Log2.IsDisclosed && (disclosureTracking2015Log2.ProviderListSent || disclosureTracking2015Log2.ProviderListNoFeeSent))
          {
            if (disclosureTracking2015Log1 == null)
              disclosureTracking2015Log1 = disclosureTracking2015Log2;
            else if (disclosureTracking2015Log1.DisclosedDate.Date > disclosureTracking2015Log2.DisclosedDate.Date)
              disclosureTracking2015Log1 = disclosureTracking2015Log2;
            else if (disclosureTracking2015Log1.DisclosedDate.Date == disclosureTracking2015Log2.DisclosedDate.Date && disclosureTracking2015Log1.DateAdded > disclosureTracking2015Log2.DateAdded)
              disclosureTracking2015Log1 = disclosureTracking2015Log2;
          }
        }
      }
      return disclosureTracking2015Log1;
    }

    public DisclosureTracking2015Log GetEarliestSSPLDisclosureTracking2015Log()
    {
      DisclosureTracking2015Log disclosureTracking2015Log1 = (DisclosureTracking2015Log) null;
      foreach (LogRecordBase record in this.records)
      {
        if (record is DisclosureTracking2015Log)
        {
          DisclosureTracking2015Log disclosureTracking2015Log2 = (DisclosureTracking2015Log) record;
          if (disclosureTracking2015Log2.IsDisclosed && (disclosureTracking2015Log2.ProviderListSent || disclosureTracking2015Log2.ProviderListNoFeeSent))
          {
            if (disclosureTracking2015Log1 == null)
              disclosureTracking2015Log1 = disclosureTracking2015Log2;
            else if (disclosureTracking2015Log1.DisclosedDate.Date > disclosureTracking2015Log2.DisclosedDate.Date)
              disclosureTracking2015Log1 = disclosureTracking2015Log2;
            else if (disclosureTracking2015Log1.DisclosedDate.Date == disclosureTracking2015Log2.DisclosedDate.Date && disclosureTracking2015Log1.DateAdded > disclosureTracking2015Log2.DateAdded)
              disclosureTracking2015Log1 = disclosureTracking2015Log2;
          }
        }
      }
      return disclosureTracking2015Log1;
    }

    public DisclosureTracking2015Log GetEarliestSafeHarborDisclosureTracking2015Log()
    {
      DisclosureTracking2015Log disclosureTracking2015Log1 = (DisclosureTracking2015Log) null;
      foreach (LogRecordBase record in this.records)
      {
        if (record is DisclosureTracking2015Log)
        {
          DisclosureTracking2015Log disclosureTracking2015Log2 = (DisclosureTracking2015Log) record;
          if (disclosureTracking2015Log2.IsDisclosed && disclosureTracking2015Log2.DisclosedForSafeHarbor)
          {
            if (disclosureTracking2015Log1 == null)
              disclosureTracking2015Log1 = disclosureTracking2015Log2;
            else if (disclosureTracking2015Log1.DisclosedDate.Date > disclosureTracking2015Log2.DisclosedDate.Date)
              disclosureTracking2015Log1 = disclosureTracking2015Log2;
            else if (disclosureTracking2015Log1.DisclosedDate.Date == disclosureTracking2015Log2.DisclosedDate.Date && disclosureTracking2015Log1.DateAdded > disclosureTracking2015Log2.DateAdded)
              disclosureTracking2015Log1 = disclosureTracking2015Log2;
          }
        }
      }
      return disclosureTracking2015Log1;
    }

    public IDisclosureTracking2015Log GetEarliestSafeHarborIDisclosureTracking2015Log()
    {
      IDisclosureTracking2015Log idisclosureTracking2015Log = (IDisclosureTracking2015Log) null;
      foreach (LogRecordBase record in this.records)
      {
        if (record is IDisclosureTracking2015Log)
        {
          IDisclosureTracking2015Log disclosureTracking2015Log = (IDisclosureTracking2015Log) record;
          if (disclosureTracking2015Log.IsDisclosed && disclosureTracking2015Log.DisclosedForSafeHarbor)
          {
            if (idisclosureTracking2015Log == null)
              idisclosureTracking2015Log = disclosureTracking2015Log;
            else if (idisclosureTracking2015Log.DisclosedDate.Date > disclosureTracking2015Log.DisclosedDate.Date)
              idisclosureTracking2015Log = disclosureTracking2015Log;
            else if (idisclosureTracking2015Log.DisclosedDate.Date == disclosureTracking2015Log.DisclosedDate.Date && idisclosureTracking2015Log.DateAdded > disclosureTracking2015Log.DateAdded)
              idisclosureTracking2015Log = disclosureTracking2015Log;
          }
        }
      }
      return idisclosureTracking2015Log;
    }

    public DocumentLog[] GetDocumentsByTitle(string title)
    {
      return LogList.GetDocumentsByTitle(title, this.GetAllRecordsOfType(typeof (DocumentLog)));
    }

    public static DocumentLog[] GetDocumentsByTitle(
      string title,
      LogRecordBase[] cachedDocumentLogs)
    {
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (DocumentLog cachedDocumentLog in cachedDocumentLogs)
      {
        if (string.Compare(cachedDocumentLog.Title, title, true) == 0)
          documentLogList.Add(cachedDocumentLog);
      }
      return documentLogList.ToArray();
    }

    public VerifLog[] GetAllVerifs()
    {
      return (VerifLog[]) this.GetAllRecordsOfType(typeof (VerifLog), true);
    }

    public int GetMilestoneIndex(string stage) => this.msList.IndexOf(this.msTbl[(object) stage]);

    internal LogRecordBase GetRecordAt(int i) => (LogRecordBase) this.records[i];

    public int GetNumberOfRecords() => this.records.Count;

    public int GetNumberOfMilestones() => this.msList.Count;

    public MilestoneLog GetMilestoneAt(int i)
    {
      return i > -1 ? (MilestoneLog) this.msList[i] : (MilestoneLog) null;
    }

    public MilestoneLog[] GetAllMilestones()
    {
      return (MilestoneLog[]) this.msList.ToArray(typeof (MilestoneLog));
    }

    public CRMLog[] GetAllCRMMapping()
    {
      return new List<CRMLog>((IEnumerable<CRMLog>) this.crmTbl.Values).ToArray();
    }

    public CRMLog GetCRMMapping(string mappingID)
    {
      return this.crmTbl.ContainsKey(mappingID) ? this.crmTbl[mappingID] : (CRMLog) null;
    }

    public int GetCRMMappingCount() => this.crmTbl.Count;

    public MilestoneLog GetCurrentMilestone()
    {
      foreach (MilestoneLog ms in this.msList)
      {
        if (!ms.Done)
          return ms;
      }
      return (MilestoneLog) null;
    }

    public MilestoneLog GetLastCompletedMilestone()
    {
      for (int index = this.msList.Count - 1; index >= 0; --index)
      {
        MilestoneLog ms = (MilestoneLog) this.msList[index];
        if (ms.Done)
          return ms;
      }
      throw new Exception("There are no completed milestones in the current loan");
    }

    public DateTime GetAdjustedMilestoneCompletionDate(int milestoneIndex, DateTime startTime)
    {
      DateTime milestoneCompletionDate = startTime;
      for (int i = 0; i < milestoneIndex; ++i)
      {
        MilestoneLog milestoneAt = this.GetMilestoneAt(i);
        if (milestoneAt.Done && milestoneAt.Date > milestoneCompletionDate)
          milestoneCompletionDate = milestoneAt.Date;
      }
      return milestoneCompletionDate;
    }

    public LogRecordBase[] GetAllRecordsOfType(Type recordType)
    {
      return this.GetAllRecordsOfType(recordType, true);
    }

    public LogRecordBase[] GetAllRecordsOfType(Type recordType, bool checkAccess)
    {
      return this.GetAllRecordsOfType(recordType, checkAccess, false);
    }

    public LogRecordBase[] GetAllRecordsOfType(
      Type recordType,
      bool checkAccess,
      bool includeRemoved)
    {
      ArrayList arrayList = new ArrayList();
      foreach (LogRecordBase record in this.records)
      {
        if (recordType.IsAssignableFrom(record.GetType()) && (!checkAccess || this.loan.AccessRules.IsLogEntryAccessible(record)))
          arrayList.Add((object) record);
      }
      if (includeRemoved)
      {
        foreach (LogRecordBase removedRecord in this.removedRecords)
        {
          if (recordType.IsAssignableFrom(removedRecord.GetType()) && (!checkAccess || this.loan.AccessRules.IsLogEntryAccessible(removedRecord)))
            arrayList.Add((object) removedRecord);
        }
      }
      if (recordType.IsAssignableFrom(typeof (MilestoneLog)))
      {
        foreach (LogRecordBase ms in this.msList)
          arrayList.Add((object) ms);
      }
      arrayList.Sort();
      return (LogRecordBase[]) arrayList.ToArray(recordType);
    }

    public LogRecordBase GetRecordByID(string guid) => this.GetRecordByID(guid, true);

    public LogRecordBase GetRecordByID(string guid, bool checkAccess)
    {
      return this.GetRecordByID(guid, checkAccess, false);
    }

    public LogRecordBase GetRecordByID(string guid, bool checkAccess, bool includeRemoved)
    {
      LogRecordBase logEntry = (LogRecordBase) null;
      if (this.recordTbl.ContainsKey((object) guid))
        logEntry = this.recordTbl[(object) guid] as LogRecordBase;
      else if (includeRemoved && this.removedRecordTbl.ContainsKey((object) guid))
        logEntry = this.removedRecordTbl[(object) guid] as LogRecordBase;
      if (logEntry == null)
        return (LogRecordBase) null;
      return checkAccess && !this.loan.AccessRules.IsLogEntryAccessible(logEntry) ? (LogRecordBase) null : logEntry;
    }

    public bool ContainsRecord(string guid) => this.recordTbl.Contains((object) guid);

    public bool IsRecordAccessible(string guid)
    {
      return !(this.recordTbl[(object) guid] is LogRecordBase logEntry) || this.loan.AccessRules.IsLogEntryAccessible(logEntry);
    }

    public bool IsRecordRemoved(string guid) => this.removedRecordTbl.ContainsKey((object) guid);

    public DocumentLog[] GetAllDocuments() => this.GetAllDocuments(true);

    public DocumentLog[] GetAllDocuments(bool checkAccess)
    {
      return this.GetAllDocuments(checkAccess, false);
    }

    public DocumentLog[] GetAllDocuments(bool checkAccess, bool includeRemoved)
    {
      return (DocumentLog[]) this.GetAllRecordsOfType(typeof (DocumentLog), checkAccess, includeRemoved);
    }

    public ConversationLog[] GetAllConversations()
    {
      return (ConversationLog[]) this.GetAllRecordsOfType(typeof (ConversationLog));
    }

    public GetIndexLog[] GetAllIndexHistory()
    {
      return (GetIndexLog[]) this.GetAllRecordsOfType(typeof (GetIndexLog));
    }

    public string GetLockCurrentStatus()
    {
      string lockCurrentStatus = string.Empty;
      DateTime date = Utils.ParseDate((object) this.loan.GetField("762"));
      LockRequestLog confirmLockRequest = this.GetLastNotConfirmLockRequest();
      Hashtable hashtable = (Hashtable) null;
      if (confirmLockRequest != null)
      {
        if (string.Compare(confirmLockRequest.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested), true) == 0)
          lockCurrentStatus = "Active Request";
        LockRequestLog lastConfirmedLock = this.GetLastConfirmedLock();
        if (lastConfirmedLock != null)
          hashtable = lastConfirmedLock.GetLockRequestSnapshot();
      }
      else
      {
        LockRequestLog currentLockRequest = this.GetCurrentLockRequest();
        if (currentLockRequest != null)
        {
          LockConfirmLog confirmLockLog = this.GetConfirmLockLog();
          if (confirmLockLog == null || confirmLockLog.RequestGUID != currentLockRequest.Guid)
            lockCurrentStatus = "Active Request";
          if (currentLockRequest.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RequestDenied))
            lockCurrentStatus = "Denied";
          if (currentLockRequest.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked))
            lockCurrentStatus = !(date == DateTime.MinValue) ? (date.Date.Subtract(DateTime.Today).Days <= 0 ? "Lock Expired" : "Locked") : "Locked";
          hashtable = currentLockRequest.GetLockRequestSnapshot();
        }
        else
        {
          LockDenialLog denialLockLog = this.GetDenialLockLog();
          if (denialLockLog != null)
          {
            lockCurrentStatus = "Denied";
            hashtable = this.GetLockRequest(denialLockLog.RequestGUID).GetLockRequestSnapshot();
          }
        }
      }
      if (hashtable != null && (lockCurrentStatus == "Active Request" || lockCurrentStatus == "Denied") && hashtable.ContainsKey((object) "2592") && hashtable[(object) "2592"].ToString() != string.Empty && (!hashtable.ContainsKey((object) "2286") || hashtable[(object) "2286"].ToString() == string.Empty) && date != DateTime.MinValue && DateTime.Today >= date.Date)
      {
        MilestoneLog completedMilestone = this.GetLastCompletedMilestone();
        if (completedMilestone.Stage == "Completion" && !completedMilestone.Done)
          lockCurrentStatus = "Long Loan";
      }
      if (Utils.ParseDate((object) this.loan.GetField("2014")) != DateTime.MinValue)
        lockCurrentStatus = "Shipped";
      if (Utils.ParseDate((object) this.loan.GetField("2370")) != DateTime.MinValue)
        lockCurrentStatus = "Purchased";
      return lockCurrentStatus;
    }

    public LockConfirmLog[] GetAllConfirmLocks()
    {
      return (LockConfirmLog[]) this.GetAllRecordsOfType(typeof (LockConfirmLog));
    }

    public LockCancellationLog[] GetAllCancellationLocks()
    {
      return (LockCancellationLog[]) this.GetAllRecordsOfType(typeof (LockCancellationLog));
    }

    public LockRemovedLog[] GetAllRemovalLocks()
    {
      return (LockRemovedLog[]) this.GetAllRecordsOfType(typeof (LockRemovedLog));
    }

    public LockValidationLog[] GetAllValidationLocks()
    {
      return (LockValidationLog[]) this.GetAllRecordsOfType(typeof (LockValidationLog));
    }

    public LockVoidLog[] GetAllVoidLocks()
    {
      return (LockVoidLog[]) this.GetAllRecordsOfType(typeof (LockVoidLog));
    }

    public LockConfirmLog[] GetAllConfirmExtsForCurrentLock()
    {
      List<LockConfirmLog> lockConfirmLogList = new List<LockConfirmLog>();
      LockConfirmLog lockConfirmation = this.GetCurrentLockConfirmation();
      if (lockConfirmation == null)
        return lockConfirmLogList.ToArray();
      LockRequestLog lockRequest1 = this.GetLockRequest(lockConfirmation.RequestGUID);
      if (lockRequest1 == null)
        return lockConfirmLogList.ToArray();
      foreach (LockConfirmLog allConfirmLock in this.GetAllConfirmLocks())
      {
        LockRequestLog lockRequest2 = this.GetLockRequest(allConfirmLock.RequestGUID);
        if (lockRequest2.IsLockExtension && !lockRequest2.IsRelock && this.GetInitialLockRequest(lockRequest2).Guid == this.GetInitialLockRequest(lockRequest1).Guid)
          lockConfirmLogList.Add(allConfirmLock);
      }
      return lockConfirmLogList.ToArray();
    }

    public int GetCurrentExtNumber() => this.GetCurrentExtNumber((Hashtable) null);

    public int GetCurrentExtNumber(Hashtable snapshot)
    {
      if (snapshot == null)
      {
        LockConfirmLog lockConfirmation = this.GetCurrentLockConfirmation();
        if (lockConfirmation == null)
          return 0;
        LockRequestLog lockRequest = this.GetLockRequest(lockConfirmation.RequestGUID);
        if (lockRequest == null)
          return 0;
        snapshot = lockRequest.GetLockRequestSnapshot();
      }
      return snapshot == null || snapshot[(object) "3433"] == null ? 0 : Utils.ParseInt(snapshot[(object) "3433"], 0);
    }

    public int GetCurrentExtCumulatedDays() => this.GetCurrentExtCumulatedDays((Hashtable) null);

    public int GetCurrentExtCumulatedDays(Hashtable snapshot)
    {
      if (snapshot == null)
      {
        LockConfirmLog lockConfirmation = this.GetCurrentLockConfirmation();
        if (lockConfirmation == null)
          return 0;
        LockRequestLog lockRequest = this.GetLockRequest(lockConfirmation.RequestGUID);
        if (lockRequest == null)
          return 0;
        snapshot = lockRequest.GetLockRequestSnapshot();
      }
      return snapshot == null || snapshot[(object) "3431"] == null ? 0 : Utils.ParseInt(snapshot[(object) "3431"], 0);
    }

    public LockRequestLog GetInitialLockRequest(LockRequestLog log)
    {
      return !string.IsNullOrEmpty(log?.ParentLockGuid) ? this.GetInitialLockRequest(this.GetLockRequest(log.ParentLockGuid)) : log;
    }

    public LockConfirmLog[] GetAllConfirmExtLocks()
    {
      List<LockConfirmLog> lockConfirmLogList = new List<LockConfirmLog>();
      foreach (LockConfirmLog allConfirmLock in this.GetAllConfirmLocks())
      {
        if (this.GetLockRequest(allConfirmLock.RequestGUID).IsLockExtension)
          lockConfirmLogList.Add(allConfirmLock);
      }
      return lockConfirmLogList.ToArray();
    }

    public LockRequestLog[] GetAllLockRequests() => this.GetAllLockRequests(true);

    public LockRequestLog[] GetAllLockRequests(bool includeFakeRequests)
    {
      LockRequestLog[] allRecordsOfType = (LockRequestLog[]) this.GetAllRecordsOfType(typeof (LockRequestLog));
      if (includeFakeRequests)
        return allRecordsOfType;
      ArrayList arrayList = new ArrayList();
      foreach (LockRequestLog lockRequestLog in allRecordsOfType)
      {
        if (!lockRequestLog.IsFakeRequest)
          arrayList.Add((object) lockRequestLog);
      }
      return (LockRequestLog[]) arrayList.ToArray(typeof (LockRequestLog));
    }

    public LockRequestLog GetLockRequest(string requestGUID)
    {
      LockRequestLog[] allRecordsOfType = (LockRequestLog[]) this.GetAllRecordsOfType(typeof (LockRequestLog));
      if (allRecordsOfType == null)
        return (LockRequestLog) null;
      if (allRecordsOfType.Length == 0)
        return (LockRequestLog) null;
      for (int index = 0; index < allRecordsOfType.Length; ++index)
      {
        if (allRecordsOfType[index].Guid == requestGUID)
          return allRecordsOfType[index];
      }
      return (LockRequestLog) null;
    }

    public LockRequestLog GetLastNotConfirmLockRequest()
    {
      foreach (LockRequestLog confirmLockRequest in this.GetAllRecordsOfType(typeof (LockRequestLog)))
      {
        if (confirmLockRequest.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested))
          return confirmLockRequest;
      }
      return (LockRequestLog) null;
    }

    public LockRequestLog GetLastConfirmedLock(bool checkForVoid = true)
    {
      if (checkForVoid)
      {
        foreach (LockRequestLog lastConfirmedLock in this.GetAllRecordsOfType(typeof (LockRequestLog)))
        {
          if (!lastConfirmedLock.Voided && lastConfirmedLock.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked))
            return lastConfirmedLock;
        }
        foreach (LockRequestLog lastConfirmedLock in this.GetAllRecordsOfType(typeof (LockRequestLog)))
        {
          if (!lastConfirmedLock.Voided && lastConfirmedLock.LockRequestStatus == RateLockRequestStatus.OldLock)
            return lastConfirmedLock;
        }
      }
      else
      {
        foreach (LockRequestLog lastConfirmedLock in this.GetAllRecordsOfType(typeof (LockRequestLog)))
        {
          if (lastConfirmedLock.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked))
            return lastConfirmedLock;
        }
      }
      return (LockRequestLog) null;
    }

    public LockRequestLog GetCurrentLockRequest()
    {
      foreach (LockRequestLog currentLockRequest in this.GetAllRecordsOfType(typeof (LockRequestLog)))
      {
        if (currentLockRequest.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked) || currentLockRequest.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested))
          return currentLockRequest;
      }
      return (LockRequestLog) null;
    }

    public LockRequestLog GetCurrentLockOrRequest()
    {
      LogRecordBase[] allRecordsOfType = this.GetAllRecordsOfType(typeof (LockRequestLog));
      for (int index = allRecordsOfType.Length - 1; index >= 0; --index)
      {
        LockRequestLog currentLockOrRequest = (LockRequestLog) allRecordsOfType[index];
        if (currentLockOrRequest.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked) || currentLockOrRequest.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested))
          return currentLockOrRequest;
      }
      return (LockRequestLog) null;
    }

    public LockConfirmLog GetCurrentLockConfirmation()
    {
      LockRequestLog lastConfirmedLock = this.GetLastConfirmedLock();
      if (lastConfirmedLock == null)
        return (LockConfirmLog) null;
      foreach (LockConfirmLog lockConfirmation in this.GetAllRecordsOfType(typeof (LockConfirmLog)))
      {
        if (lockConfirmation.RequestGUID == lastConfirmedLock.Guid)
          return lockConfirmation;
      }
      return (LockConfirmLog) null;
    }

    public LockConfirmLog GetMostRecentConfirmForCurrentLock()
    {
      LockRequestLog lastConfirmedLock = this.GetLastConfirmedLock();
      if (lastConfirmedLock == null)
        return (LockConfirmLog) null;
      LockConfirmLog confirmForCurrentLock = (LockConfirmLog) null;
      foreach (LockConfirmLog lockConfirmLog in this.GetAllRecordsOfType(typeof (LockConfirmLog)))
      {
        if (lockConfirmLog.RequestGUID == lastConfirmedLock.Guid)
        {
          if (confirmForCurrentLock == null)
            confirmForCurrentLock = lockConfirmLog;
          else if (confirmForCurrentLock.DateTimeConfirmed < lockConfirmLog.DateTimeConfirmed)
            confirmForCurrentLock = lockConfirmLog;
        }
      }
      return confirmForCurrentLock;
    }

    public LockRequestLog GetMostRecentLockRequest()
    {
      LockRequestLog recentLockRequest = (LockRequestLog) null;
      foreach (LockRequestLog lockRequestLog in this.GetAllRecordsOfType(typeof (LockRequestLog)))
      {
        if (lockRequestLog.DateTimeRequested != DateTime.MinValue && (recentLockRequest == null || lockRequestLog.DateTimeRequested > recentLockRequest.DateTimeRequested))
          recentLockRequest = lockRequestLog;
      }
      return recentLockRequest;
    }

    public LockConfirmLog GetMostRecentConfirmedLock()
    {
      LockConfirmLog recentConfirmedLock = (LockConfirmLog) null;
      foreach (LockConfirmLog lockConfirmLog in this.GetAllRecordsOfType(typeof (LockConfirmLog)))
      {
        if (lockConfirmLog.DateTimeConfirmed != DateTime.MinValue && (recentConfirmedLock == null || lockConfirmLog.DateTimeConfirmed > recentConfirmedLock.DateTimeConfirmed))
          recentConfirmedLock = lockConfirmLog;
      }
      return recentConfirmedLock;
    }

    public LockCancellationLog GetMostRecentLockCancellation()
    {
      LockCancellationLog lockCancellation = (LockCancellationLog) null;
      foreach (LockCancellationLog lockCancellationLog in this.GetAllRecordsOfType(typeof (LockCancellationLog)))
      {
        if (lockCancellationLog.Date != DateTime.MinValue && (lockCancellation == null || lockCancellationLog.Date > lockCancellation.Date))
          lockCancellation = lockCancellationLog;
      }
      return lockCancellation;
    }

    public LockDenialLog[] GetAllDenialLockLog()
    {
      return (LockDenialLog[]) this.GetAllRecordsOfType(typeof (LockDenialLog));
    }

    public LockDenialLog GetDenialLockLog()
    {
      LockDenialLog[] allRecordsOfType = (LockDenialLog[]) this.GetAllRecordsOfType(typeof (LockDenialLog));
      if (allRecordsOfType == null)
        return (LockDenialLog) null;
      if (allRecordsOfType.Length == 0)
        return (LockDenialLog) null;
      DateTime dateTime = DateTime.MinValue;
      DateTime minValue = DateTime.MinValue;
      int index1 = 0;
      try
      {
        for (int index2 = 0; index2 < allRecordsOfType.Length; ++index2)
        {
          DateTime date = Utils.ParseDate((object) (allRecordsOfType[index2].Date.ToString("MM/dd/yyyy") + " " + allRecordsOfType[index2].TimeDenied));
          if (dateTime == DateTime.MinValue)
            dateTime = date;
          else if (dateTime <= date)
          {
            dateTime = date;
            index1 = index2;
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LogList.sw, nameof (LogList), TraceLevel.Error, "(GetDenialLockLog) Cannot get denial lock log. Error: " + ex.Message);
        return (LockDenialLog) null;
      }
      return allRecordsOfType[index1];
    }

    public LogRecordBase GetLastLockAlertLog()
    {
      LockRequestLog currentLockRequest = this.GetCurrentLockRequest();
      LockConfirmLog confirmLockLog = this.GetConfirmLockLog();
      LockDenialLog denialLockLog = this.GetDenialLockLog();
      LockCancellationLog lockCancellation = this.GetMostRecentLockCancellation();
      LockRemovedLog[] allRecordsOfType = (LockRemovedLog[]) this.GetAllRecordsOfType(typeof (LockRemovedLog));
      LockRemovedLog lockRemovedLog = (LockRemovedLog) null;
      if (allRecordsOfType != null && ((IEnumerable<LockRemovedLog>) allRecordsOfType).Count<LockRemovedLog>() > 0)
        lockRemovedLog = ((IEnumerable<LockRemovedLog>) allRecordsOfType).Where<LockRemovedLog>((Func<LockRemovedLog, bool>) (l => l.Date != DateTime.MinValue)).OrderByDescending<LockRemovedLog, DateTime>((Func<LockRemovedLog, DateTime>) (l => l.Date)).First<LockRemovedLog>();
      DateTime dateTime1 = currentLockRequest != null ? currentLockRequest.DateTimeRequested : DateTime.MinValue;
      DateTime dateTime2 = confirmLockLog != null ? confirmLockLog.DateTimeConfirmed : DateTime.MinValue;
      DateTime dateTime3 = denialLockLog != null ? denialLockLog.DateTimeDenied : DateTime.MinValue;
      DateTime dateTime4 = lockCancellation != null ? lockCancellation.DateTimeCancelled : DateTime.MinValue;
      DateTime dateTime5 = lockRemovedLog != null ? lockRemovedLog.DateTimeRemoved : DateTime.MinValue;
      if (dateTime5 > dateTime4 && dateTime5 > dateTime3 && dateTime5 > dateTime2 && dateTime5 > dateTime1)
        return !lockRemovedLog.AlertLoanOfficer ? (LogRecordBase) null : (LogRecordBase) lockRemovedLog;
      LockRequestLog lockRequest1 = lockCancellation == null ? (LockRequestLog) null : this.GetLockRequest(lockCancellation.RequestGUID);
      if (lockRequest1 != null && lockRequest1.RequestedStatus == "Cancelled" && (currentLockRequest == null || lockRequest1.Date > currentLockRequest.Date))
        return !lockCancellation.AlertLoanOfficer ? (LogRecordBase) null : (LogRecordBase) lockCancellation;
      if (currentLockRequest != null && currentLockRequest.LockRequestStatus == RateLockRequestStatus.Requested)
        return (LogRecordBase) currentLockRequest;
      LockRequestLog lockRequest2 = confirmLockLog == null ? (LockRequestLog) null : this.GetLockRequest(confirmLockLog.RequestGUID);
      LockRequestLog lockRequest3 = denialLockLog == null ? (LockRequestLog) null : this.GetLockRequest(denialLockLog.RequestGUID);
      if (confirmLockLog != null && denialLockLog == null)
        return !confirmLockLog.AlertLoanOfficer ? (LogRecordBase) null : (LogRecordBase) confirmLockLog;
      if (confirmLockLog == null && denialLockLog != null)
        return !denialLockLog.AlertLoanOfficer ? (LogRecordBase) null : (LogRecordBase) denialLockLog;
      if (confirmLockLog == null || denialLockLog == null)
        return (LogRecordBase) null;
      return lockRequest2.LockRequestStatus == RateLockRequestStatus.RateLocked ? (!confirmLockLog.AlertLoanOfficer ? (LogRecordBase) null : (LogRecordBase) confirmLockLog) : (lockRequest2 != lockRequest3 && lockRequest2.DateTimeRequested > lockRequest3.DateTimeRequested ? (!confirmLockLog.AlertLoanOfficer ? (LogRecordBase) null : (LogRecordBase) confirmLockLog) : (lockRequest2 != lockRequest3 ? (!denialLockLog.AlertLoanOfficer ? (LogRecordBase) null : (LogRecordBase) denialLockLog) : (lockRequest2.LockRequestStatus == RateLockRequestStatus.RequestDenied ? (!denialLockLog.AlertLoanOfficer ? (LogRecordBase) null : (LogRecordBase) denialLockLog) : (!confirmLockLog.AlertLoanOfficer ? (LogRecordBase) null : (LogRecordBase) confirmLockLog))));
    }

    public LockConfirmLog GetConfirmLockLogForRequestLog(string requestLogGuid)
    {
      LockConfirmLog[] allRecordsOfType = (LockConfirmLog[]) this.GetAllRecordsOfType(typeof (LockConfirmLog));
      if (allRecordsOfType == null)
        return (LockConfirmLog) null;
      if (allRecordsOfType.Length == 0)
        return (LockConfirmLog) null;
      LockConfirmLog logForRequestLog = (LockConfirmLog) null;
      try
      {
        DateTime dateTime = DateTime.MinValue;
        foreach (LockConfirmLog lockConfirmLog in allRecordsOfType)
        {
          if (lockConfirmLog.RequestGUID == requestLogGuid && lockConfirmLog.Date > dateTime)
          {
            dateTime = lockConfirmLog.Date;
            logForRequestLog = lockConfirmLog;
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LogList.sw, nameof (LogList), TraceLevel.Error, "(GetConfirmLockLogForRequestLog) Cannot get confirmation lock log. Error: " + ex.Message);
        return (LockConfirmLog) null;
      }
      return logForRequestLog;
    }

    public LockCancellationLog GetCancellationLogForRequestLog(string requestLogGuid)
    {
      LockCancellationLog[] allRecordsOfType = (LockCancellationLog[]) this.GetAllRecordsOfType(typeof (LockCancellationLog));
      if (allRecordsOfType == null)
        return (LockCancellationLog) null;
      if (allRecordsOfType.Length == 0)
        return (LockCancellationLog) null;
      try
      {
        foreach (LockCancellationLog logForRequestLog in allRecordsOfType)
        {
          if (logForRequestLog.RequestGUID == requestLogGuid)
            return logForRequestLog;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LogList.sw, nameof (LogList), TraceLevel.Error, "(GetCancellationLogForRequestLog) Cannot get cancellation lock log. Error: " + ex.Message);
        return (LockCancellationLog) null;
      }
      return (LockCancellationLog) null;
    }

    public StatusOnlineLog[] GetAllStatusOnlineLogs()
    {
      List<StatusOnlineLog> statusOnlineLogList = new List<StatusOnlineLog>();
      foreach (LogRecordBase record in this.records)
      {
        if (record is StatusOnlineLog)
          statusOnlineLogList.Add((StatusOnlineLog) record);
      }
      statusOnlineLogList.Sort();
      return statusOnlineLogList.ToArray();
    }

    public LockRemovedLog GetLockRemovedLogForRequestLog(string requestLogGuid)
    {
      LockRemovedLog[] allRecordsOfType = (LockRemovedLog[]) this.GetAllRecordsOfType(typeof (LockRemovedLog));
      if (allRecordsOfType == null)
        return (LockRemovedLog) null;
      if (allRecordsOfType.Length == 0)
        return (LockRemovedLog) null;
      try
      {
        foreach (LockRemovedLog logForRequestLog in allRecordsOfType)
        {
          if (logForRequestLog.RequestGUID == requestLogGuid)
            return logForRequestLog;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LogList.sw, nameof (LogList), TraceLevel.Error, "(GetLockRemovedLogForRequestLog) Cannot get remove lock log. Error: " + ex.Message);
        return (LockRemovedLog) null;
      }
      return (LockRemovedLog) null;
    }

    public LockVoidLog GetLockVoidLogForRequestLog(string requestLogGuid)
    {
      LockVoidLog[] allRecordsOfType = (LockVoidLog[]) this.GetAllRecordsOfType(typeof (LockVoidLog));
      if (allRecordsOfType == null)
        return (LockVoidLog) null;
      if (allRecordsOfType.Length == 0)
        return (LockVoidLog) null;
      try
      {
        foreach (LockVoidLog logForRequestLog in allRecordsOfType)
        {
          if (logForRequestLog.RequestGUID == requestLogGuid)
            return logForRequestLog;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LogList.sw, nameof (LogList), TraceLevel.Error, "(GetLockVoidLogForRequestLog) Cannot get lock void log. Error: " + ex.Message);
        return (LockVoidLog) null;
      }
      return (LockVoidLog) null;
    }

    public LockConfirmLog GetConfirmLockLog()
    {
      LockConfirmLog[] allRecordsOfType = (LockConfirmLog[]) this.GetAllRecordsOfType(typeof (LockConfirmLog));
      if (allRecordsOfType == null)
        return (LockConfirmLog) null;
      if (allRecordsOfType.Length == 0)
        return (LockConfirmLog) null;
      DateTime dateTime = DateTime.MinValue;
      DateTime minValue = DateTime.MinValue;
      int index1 = 0;
      try
      {
        for (int index2 = 0; index2 < allRecordsOfType.Length; ++index2)
        {
          DateTime date = Utils.ParseDate((object) allRecordsOfType[index2].DateConfirmed);
          if (dateTime == DateTime.MinValue)
            dateTime = date;
          else if (dateTime <= date)
          {
            dateTime = date;
            index1 = index2;
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LogList.sw, nameof (LogList), TraceLevel.Error, "(GetDenialLockLog) Cannot get confirmation lock log. Error: " + ex.Message);
        return (LockConfirmLog) null;
      }
      LockConfirmLog lockConfirmLog = allRecordsOfType[index1];
      return this.GetLockRequest(lockConfirmLog.RequestGUID).RequestedStatus == "Denied" ? (LockConfirmLog) null : lockConfirmLog;
    }

    public LockRequestLog GetCurrentConfirmedLockRequest()
    {
      LockRequestLog confirmedLockRequest = (LockRequestLog) null;
      foreach (LockRequestLog lockRequestLog in this.GetAllRecordsOfType(typeof (LockRequestLog)))
      {
        if (lockRequestLog.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked))
          confirmedLockRequest = lockRequestLog;
      }
      if (confirmedLockRequest == null)
        return (LockRequestLog) null;
      foreach (LockConfirmLog lockConfirmLog in this.GetAllRecordsOfType(typeof (LockConfirmLog)))
      {
        if (lockConfirmLog.RequestGUID == confirmedLockRequest.Guid)
          return confirmedLockRequest;
      }
      return (LockRequestLog) null;
    }

    public DownloadLog[] GetAllDownloads()
    {
      return (DownloadLog[]) this.GetAllRecordsOfType(typeof (DownloadLog));
    }

    public MilestoneFreeRoleLog[] GetAllMilestoneFreeRoles()
    {
      return (MilestoneFreeRoleLog[]) this.GetAllRecordsOfType(typeof (MilestoneFreeRoleLog));
    }

    public LoanAssociateLog[] GetAllLoanAssociates()
    {
      return (LoanAssociateLog[]) this.GetAllRecordsOfType(typeof (LoanAssociateLog));
    }

    public EnhancedConditionLog[] GetAllEnhancedConditions(bool checkAccess = true, bool includeRemoved = false)
    {
      return (EnhancedConditionLog[]) this.GetAllRecordsOfType(typeof (EnhancedConditionLog), checkAccess, includeRemoved);
    }

    public ConditionLog[] GetAllConditions() => this.GetAllConditions(true);

    public ConditionLog[] GetAllConditions(bool checkAccess)
    {
      return this.GetAllConditions(checkAccess, false);
    }

    public ConditionLog[] GetAllConditions(bool checkAccess, bool includeRemoved)
    {
      return (ConditionLog[]) this.GetAllRecordsOfType(typeof (ConditionLog), checkAccess, includeRemoved);
    }

    public ConditionLog[] GetAllConditions(ConditionType conditionType)
    {
      return this.GetAllConditions(conditionType, true);
    }

    public ConditionLog[] GetAllConditions(ConditionType conditionType, bool checkAccess)
    {
      return this.GetAllConditions(conditionType, checkAccess, false);
    }

    public ConditionLog[] GetAllConditions(
      ConditionType conditionType,
      bool checkAccess,
      bool includeRemoved)
    {
      switch (conditionType)
      {
        case ConditionType.Underwriting:
          return (ConditionLog[]) this.GetAllRecordsOfType(typeof (UnderwritingConditionLog), checkAccess, includeRemoved);
        case ConditionType.PostClosing:
          return (ConditionLog[]) this.GetAllRecordsOfType(typeof (PostClosingConditionLog), checkAccess, includeRemoved);
        case ConditionType.Preliminary:
          return (ConditionLog[]) this.GetAllRecordsOfType(typeof (PreliminaryConditionLog), checkAccess, includeRemoved);
        case ConditionType.Sell:
          return (ConditionLog[]) this.GetAllRecordsOfType(typeof (SellConditionLog), checkAccess, includeRemoved);
        case ConditionType.Enhanced:
          return (ConditionLog[]) this.GetAllRecordsOfType(typeof (EnhancedConditionLog), checkAccess, includeRemoved);
        default:
          return (ConditionLog[]) null;
      }
    }

    public DocumentLog[] GetAllePASSDocuments()
    {
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (DocumentLog documentLog in this.GetAllRecordsOfType(typeof (DocumentLog)))
      {
        if (documentLog.IsePASS && documentLog.Status != "needed")
          documentLogList.Add(documentLog);
      }
      return documentLogList.ToArray();
    }

    public ComplianceTestLog[] GetAllComplianceTests()
    {
      return (ComplianceTestLog[]) this.GetAllRecordsOfType(typeof (ComplianceTestLog), true);
    }

    public FannieServiceDuLog[] GetAllFannieServiceDUs()
    {
      return (FannieServiceDuLog[]) this.GetAllRecordsOfType(typeof (FannieServiceDuLog), true);
    }

    public FannieServiceEcLog[] GetAllFannieServiceECs()
    {
      return (FannieServiceEcLog[]) this.GetAllRecordsOfType(typeof (FannieServiceEcLog), true);
    }

    public FreddieServiceLpaLog[] GetAllFreddieServiceLPAs()
    {
      return (FreddieServiceLpaLog[]) this.GetAllRecordsOfType(typeof (FreddieServiceLpaLog), true);
    }

    public FreddieServiceLqaLog[] GetAllFreddieServiceLQAs()
    {
      return (FreddieServiceLqaLog[]) this.GetAllRecordsOfType(typeof (FreddieServiceLqaLog), true);
    }

    public MIServiceArchLog[] GetAllMIServiceArchs()
    {
      return (MIServiceArchLog[]) this.GetAllRecordsOfType(typeof (MIServiceArchLog), true);
    }

    public MIServiceRadianLog[] GetAllMIServiceRadians()
    {
      return (MIServiceRadianLog[]) this.GetAllRecordsOfType(typeof (MIServiceRadianLog), true);
    }

    public MIServiceMgicLog[] GetAllMIServiceMgics()
    {
      return (MIServiceMgicLog[]) this.GetAllRecordsOfType(typeof (MIServiceMgicLog), true);
    }

    public LogRecordBase[] GetAllDatedRecords()
    {
      SortedList sort = new SortedList();
      foreach (MilestoneLog ms in this.msList)
        sort[(object) this.getSortKey(sort, ms.GetSortDate())] = (object) ms;
      foreach (LogRecordBase record in this.records)
      {
        if (this.loan.AccessRules.IsLogEntryAccessible(record))
        {
          LogAlert mostCriticalAlert = record.AlertList.GetMostCriticalAlert();
          if (mostCriticalAlert != null)
            sort[(object) this.getSortKey(sort, mostCriticalAlert.GetSortDate())] = (object) record;
          else if (record.IncludeInLog())
            sort[(object) this.getSortKey(sort, record.GetSortDate())] = (object) record;
        }
      }
      LogRecordBase[] allDatedRecords = new LogRecordBase[sort.Count];
      sort.Values.CopyTo((Array) allDatedRecords, 0);
      return allDatedRecords;
    }

    public MilestoneLog GetMilestone(string stage) => (MilestoneLog) this.msTbl[(object) stage];

    public MilestoneLog GetMilestoneByID(string milestoneID)
    {
      foreach (MilestoneLog milestoneById in (IEnumerable) this.msTbl.Values)
      {
        if (milestoneById.MilestoneID == milestoneID)
          return milestoneById;
      }
      return (MilestoneLog) null;
    }

    public MilestoneLog GetMilestoneByName(string stage)
    {
      foreach (MilestoneLog milestoneByName in (IEnumerable) this.msTbl.Values)
      {
        if (string.Compare(milestoneByName.Stage, stage, true) == 0)
          return milestoneByName;
      }
      return (MilestoneLog) null;
    }

    public bool IsDirty()
    {
      if (this.isDirty)
        return true;
      foreach (LogRecordBase ms in this.msList)
      {
        if (ms.IsDirty())
          return true;
      }
      foreach (LogRecordBase record in this.records)
      {
        if (record.IsDirty())
          return true;
      }
      foreach (CRMLog crmLog in this.crmTbl.Values)
      {
        if (crmLog.IsDirty())
          return true;
      }
      return false;
    }

    public void MarkAsClean()
    {
      foreach (LogRecordBase ms in this.msList)
        ms.MarkAsClean();
      foreach (LogRecordBase record in this.records)
        record.MarkAsClean();
      this.isDirty = false;
    }

    public MilestoneLog InsertMilestone(int index, string ms)
    {
      MilestoneLog milestoneLog1 = (MilestoneLog) this.msTbl[(object) ms];
      if (milestoneLog1 != null)
        return milestoneLog1;
      MilestoneLog milestoneLog2 = new MilestoneLog(ms, this, this.loan.MilestoneDateTimeType, this.milestoneDateCalculator);
      this.msList.Insert(index, (object) milestoneLog2);
      this.msTbl[(object) ms] = (object) milestoneLog2;
      this.isDirty = true;
      return milestoneLog2;
    }

    public MilestoneLog AddMilestone(string ms) => this.AddMilestone(ms, 0, -1);

    public MilestoneLog AddMilestone(string ms, int days, int sortIndex)
    {
      MilestoneLog milestoneLog = (MilestoneLog) this.msTbl[(object) ms];
      if (milestoneLog != null)
        return milestoneLog;
      MilestoneLog msLog = new MilestoneLog(ms, this, this.loan.MilestoneDateTimeType, this.milestoneDateCalculator);
      msLog.Days = days;
      msLog.SortIndex = sortIndex;
      this.addMilestone(msLog);
      this.isDirty = true;
      return msLog;
    }

    public MilestoneLog AddMilestone(
      string ms,
      int days,
      int sortIndex,
      string tpoConnectStatus,
      string consumerStatus)
    {
      MilestoneLog milestoneLog = (MilestoneLog) this.msTbl[(object) ms];
      if (milestoneLog != null)
        return milestoneLog;
      MilestoneLog msLog = new MilestoneLog(ms, this, this.loan.MilestoneDateTimeType, this.milestoneDateCalculator);
      msLog.Days = days;
      msLog.SortIndex = sortIndex;
      msLog.TPOConnectStatus = tpoConnectStatus;
      msLog.ConsumerStatus = consumerStatus;
      this.addMilestone(msLog);
      this.isDirty = true;
      return msLog;
    }

    public MilestoneTemplate MilestoneTemplate
    {
      get => this.template;
      set
      {
        this.template = value;
        MilestoneTemplateLog[] allRecordsOfType = (MilestoneTemplateLog[]) this.GetAllRecordsOfType(typeof (MilestoneTemplateLog));
        if (allRecordsOfType.Length != 0)
        {
          foreach (LogRecordBase rec in allRecordsOfType)
            this.RemoveRecord(rec);
        }
        this.AddRecord((LogRecordBase) new MilestoneTemplateLog(this, this.template));
        this.isDirty = true;
      }
    }

    public bool MSLock
    {
      get => this.template == null || this.msLock;
      set
      {
        this.msLock = value;
        this.isDirty = true;
      }
    }

    public bool MSDateLock
    {
      get => this.template != null && this.msDateLock;
      set
      {
        this.msDateLock = value;
        this.isDirty = true;
      }
    }

    public bool ShowDatesInLog
    {
      get => !this.showDates ? this.GetMilestoneAt(1).Done : this.showDates;
      set => this.showDates = value;
    }

    public MilestoneLog AddMilestone(string ms, int days, string Guid)
    {
      return this.AddMilestone(ms, days, Guid, "", "");
    }

    public MilestoneLog AddMilestone(
      string ms,
      int days,
      string Guid,
      string tpoConnectStatus,
      string consumerStatus)
    {
      MilestoneLog milestoneLog = (MilestoneLog) this.msTbl[(object) ms];
      if (milestoneLog != null)
        return milestoneLog;
      MilestoneLog msLog = new MilestoneLog(Guid, ms, this, this.loan.MilestoneDateTimeType, this.milestoneDateCalculator);
      msLog.Days = days;
      msLog.TPOConnectStatus = tpoConnectStatus;
      msLog.ConsumerStatus = consumerStatus;
      this.addMilestone(msLog);
      this.isDirty = true;
      return msLog;
    }

    public MilestoneLog AddMilestone(string ms, int days, string Guid, int sortIndex)
    {
      return this.AddMilestone(ms, days, Guid, sortIndex, "", "");
    }

    public MilestoneLog AddMilestone(
      string ms,
      int days,
      string Guid,
      int sortIndex,
      string tpoConnectStatus,
      string consumerStatus)
    {
      MilestoneLog milestoneLog = (MilestoneLog) this.msTbl[(object) ms];
      if (milestoneLog != null)
        return milestoneLog;
      MilestoneLog msLog = new MilestoneLog(Guid, ms, this, this.loan.MilestoneDateTimeType, this.milestoneDateCalculator);
      msLog.Days = days;
      msLog.SortIndex = sortIndex;
      msLog.TPOConnectStatus = tpoConnectStatus;
      msLog.ConsumerStatus = consumerStatus;
      this.addMilestone(msLog);
      this.isDirty = true;
      return msLog;
    }

    public void RemoveMilestone(int i) => this.removeMilestone(i);

    private void addMilestone(MilestoneLog msLog)
    {
      this.msList.Add((object) msLog);
      this.msTbl[(object) msLog.Stage] = (object) msLog;
      this.recordTbl[(object) msLog.Guid] = (object) msLog;
    }

    private void removeMilestone(int i)
    {
      for (int index = i; index < this.msList.Count; ++index)
      {
        MilestoneLog ms = (MilestoneLog) this.msList[index];
        if (this.msTbl.ContainsKey((object) ms.Stage))
          this.msTbl.Remove((object) ms.Stage);
        if (this.recordTbl.ContainsKey((object) ms.Guid))
          this.recordTbl.Remove((object) ms.Guid);
      }
      this.msList.RemoveRange(i, this.msList.Count - i);
    }

    public RegistrationLog GetLatestRegistrationLog()
    {
      RegistrationLog[] allRecordsOfType = (RegistrationLog[]) this.GetAllRecordsOfType(typeof (RegistrationLog));
      if (allRecordsOfType.Length == 0)
        return (RegistrationLog) null;
      for (int index = 0; index < allRecordsOfType.Length; ++index)
      {
        if (allRecordsOfType[index].IsCurrent)
          return allRecordsOfType[index];
      }
      return (RegistrationLog) null;
    }

    internal void SortMilestones()
    {
    }

    public LoanAssociateLog[] GetAssignedAssociates(bool includeFileStarter)
    {
      ArrayList arrayList = new ArrayList();
      foreach (LoanAssociateLog allLoanAssociate in this.GetAllLoanAssociates())
      {
        if ((allLoanAssociate.LoanAssociateID ?? "") != "")
        {
          if (includeFileStarter && allLoanAssociate.RoleID == RoleInfo.FileStarter.ID)
            arrayList.Add((object) allLoanAssociate);
          else if (allLoanAssociate.RoleID > RoleInfo.FileStarter.ID)
            arrayList.Add((object) allLoanAssociate);
        }
      }
      return (LoanAssociateLog[]) arrayList.ToArray(typeof (LoanAssociateLog));
    }

    public LoanAssociateLog[] GetAssignedAssociates(int roleId)
    {
      ArrayList arrayList = new ArrayList();
      foreach (LoanAssociateLog allLoanAssociate in this.GetAllLoanAssociates())
      {
        if (allLoanAssociate.RoleID == roleId && (allLoanAssociate.LoanAssociateID ?? "") != "")
          arrayList.Add((object) allLoanAssociate);
      }
      return (LoanAssociateLog[]) arrayList.ToArray(typeof (LoanAssociateLog));
    }

    public bool IsUserInAssignedRole(string userId, int roleId)
    {
      foreach (MilestoneLog allMilestone in this.GetAllMilestones())
      {
        if (allMilestone.RoleID == roleId && allMilestone.LoanAssociateID == userId)
          return true;
      }
      foreach (MilestoneFreeRoleLog milestoneFreeRole in this.GetAllMilestoneFreeRoles())
      {
        if (milestoneFreeRole.RoleID == roleId && milestoneFreeRole.LoanAssociateID == userId)
          return true;
      }
      return false;
    }

    public MilestoneTaskLog[] GetAllMilestoneTaskLogs(string milestoneStage)
    {
      SortedList sortedList = new SortedList();
      foreach (LogRecordBase record in this.records)
      {
        if (record is MilestoneTaskLog)
        {
          MilestoneTaskLog milestoneTaskLog = (MilestoneTaskLog) record;
          if (milestoneStage == null || !(milestoneTaskLog.Stage != milestoneStage))
          {
            string key = this.GetMilestoneIndex(milestoneTaskLog.Stage).ToString() + milestoneTaskLog.TaskName;
            while (sortedList.ContainsKey((object) key))
              key += "1";
            sortedList[(object) key] = (object) milestoneTaskLog;
          }
        }
      }
      MilestoneTaskLog[] milestoneTaskLogs = new MilestoneTaskLog[sortedList.Count];
      sortedList.Values.CopyTo((Array) milestoneTaskLogs, 0);
      return milestoneTaskLogs;
    }

    public MilestoneTaskLog[] GetAllMilestoneTaskLogs()
    {
      List<MilestoneTaskLog> milestoneTaskLogList = new List<MilestoneTaskLog>();
      foreach (LogRecordBase record in this.records)
      {
        if (record is MilestoneTaskLog)
          milestoneTaskLogList.Add((MilestoneTaskLog) record);
      }
      return milestoneTaskLogList.ToArray();
    }

    public DocumentTrackingLog[] GetAllDocumentTrackingLogs()
    {
      List<DocumentTrackingLog> documentTrackingLogList = new List<DocumentTrackingLog>();
      foreach (LogRecordBase record in this.records)
      {
        if (record is DocumentTrackingLog)
          documentTrackingLogList.Add((DocumentTrackingLog) record);
      }
      documentTrackingLogList.Sort();
      return documentTrackingLogList.ToArray();
    }

    public TargetTradeLog[] GetAllTargetTradeLogs()
    {
      List<TargetTradeLog> targetTradeLogList = new List<TargetTradeLog>();
      foreach (LogRecordBase record in this.records)
      {
        if (record is TargetTradeLog)
          targetTradeLogList.Add((TargetTradeLog) record);
      }
      targetTradeLogList.Sort();
      return targetTradeLogList.ToArray();
    }

    public void UpdateMilestoneStatus()
    {
      string val = "Started";
      DateTime dateTime = DateTime.MinValue;
      if (this.msList != null && this.msList.Count > 0)
        dateTime = ((LogRecordBase) this.msList[0]).Date;
      for (int index = this.msList.Count - 1; index >= 1; --index)
      {
        MilestoneLog ms = (MilestoneLog) this.msList[index];
        if (ms.Done)
        {
          val = LogList.GetMilestoneStatus(ms.MilestoneID, ms.DoneText);
          dateTime = ms.Date;
          break;
        }
      }
      this.loan.SetField("MS.STATUS", val);
      if (dateTime != DateTime.MinValue)
        this.loan.SetField("MS.STATUSDATE", dateTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
      else
        this.loan.SetField("MS.STATUSDATE", "");
    }

    public static string GetMilestoneStatus(string milestoneId, string milestoneStage)
    {
      string milestoneStatus = "Started";
      if (Utils.IsInt((object) milestoneId))
      {
        switch (milestoneId)
        {
          case "1":
            milestoneStatus = "File started";
            break;
          case "2":
            milestoneStatus = "Sent to processing";
            break;
          case "3":
            milestoneStatus = "Submitted";
            break;
          case "4":
            milestoneStatus = "Approved";
            break;
          case "5":
            milestoneStatus = "Doc signed";
            break;
          case "6":
            milestoneStatus = "Funded";
            break;
          case "7":
            milestoneStatus = "Completed";
            break;
        }
      }
      else
        milestoneStatus = milestoneStage;
      return milestoneStatus;
    }

    public void CleanUpMilestoneDates()
    {
      DateTime dateTime1 = DateTime.MaxValue;
      foreach (LogRecordBase record in this.records)
      {
        DateTime dateTime2 = record.Date;
        DateTime date1 = dateTime2.Date;
        dateTime2 = DateTime.MinValue;
        DateTime date2 = dateTime2.Date;
        if (date1 != date2 && record.Date < dateTime1)
          dateTime1 = record.Date;
      }
      MilestoneLog milestone = this.GetMilestone("Started");
      if (milestone == null)
        return;
      DateTime dateTime3;
      if (dateTime1.Date != DateTime.MaxValue.Date)
      {
        DateTime date3 = dateTime1.Date;
        dateTime3 = milestone.Date;
        DateTime date4 = dateTime3.Date;
        if (date3 < date4)
          milestone.SetDateInternal(dateTime1.Date);
      }
      bool flag = false;
      for (int index = this.msList.Count - 1; index >= 0; --index)
      {
        MilestoneLog ms = (MilestoneLog) this.msList[index];
        if (ms.Done)
          flag = true;
        else if (flag)
          ms.Done = true;
      }
      DateTime newDate = DateTime.MaxValue;
      int num = 0;
      for (int index = 0; index < this.msList.Count; ++index)
      {
        MilestoneLog ms = (MilestoneLog) this.msList[index];
        DateTime date = ms.Date;
        if (!(date == DateTime.MaxValue) && !(date == DateTime.MinValue))
        {
          num = index;
          if (date < newDate)
            newDate = ms.Date;
        }
      }
      if (newDate != DateTime.MaxValue)
      {
        milestone.SetDateInternal(newDate);
      }
      else
      {
        MilestoneLog milestoneLog = milestone;
        dateTime3 = DateTime.Today;
        DateTime date = dateTime3.Date;
        milestoneLog.SetDateInternal(date);
      }
      MilestoneLog milestoneLog1 = milestone;
      for (int index = 1; index <= num; ++index)
      {
        MilestoneLog ms = (MilestoneLog) this.msList[index];
        if (ms.Done && (ms.Date < milestoneLog1.Date || ms.Date == DateTime.MaxValue))
          ms.SetDateInternal(milestoneLog1.Date);
        milestoneLog1 = ms;
      }
      for (int index = num + 1; index < this.msList.Count; ++index)
        ((MilestoneLog) this.msList[index]).SetDateInternal(DateTime.MaxValue);
      TimeSpan timeSpan;
      for (int index = 0; index < this.msList.Count - 1; ++index)
      {
        MilestoneLog ms1 = (MilestoneLog) this.msList[index];
        MilestoneLog ms2 = (MilestoneLog) this.msList[index + 1];
        if (ms2.Date == DateTime.MinValue || ms2.Date == DateTime.MaxValue || !ms2.Done || !ms1.Done)
        {
          if (ms1.Duration != -1)
            ms1.Duration = -1;
        }
        else
        {
          dateTime3 = ms1.Date;
          DateTime date = dateTime3.Date;
          dateTime3 = ms2.Date;
          timeSpan = dateTime3.Date.Subtract(date);
          if (ms1.Duration != timeSpan.Days)
            ms1.Duration = timeSpan.Days;
        }
      }
      string val = "";
      if (this.msList != null)
      {
        MilestoneLog ms3 = (MilestoneLog) this.msList[this.msList.Count - 1];
        if (ms3.Done && ms3.Date != DateTime.MinValue && ms3.Date != DateTime.MaxValue)
        {
          MilestoneLog ms4 = (MilestoneLog) this.msList[0];
          if (ms4.Date != DateTime.MinValue && ms4.Date != DateTime.MaxValue && ms4.Done)
          {
            dateTime3 = ms4.Date;
            DateTime date5 = dateTime3.Date;
            dateTime3 = ms3.Date;
            DateTime date6 = dateTime3.Date;
            dateTime3 = ms3.Date;
            dateTime3 = dateTime3.Date;
            timeSpan = dateTime3.Subtract(ms4.Date.Date);
            val = timeSpan.Days.ToString();
          }
        }
      }
      if (!(val != this.loan.GetField("MS.LOANDURATION")))
        return;
      this.loan.SetCurrentField("MS.LOANDURATION", val);
    }

    internal ArrayList GetUnfilteredRecordsOfType(Type recordType)
    {
      ArrayList unfilteredRecordsOfType = new ArrayList();
      foreach (LogRecordBase record in this.records)
      {
        if (recordType.IsAssignableFrom(record.GetType()))
          unfilteredRecordsOfType.Add((object) record);
      }
      return unfilteredRecordsOfType;
    }

    private long getSortKey(SortedList sort, DateTime date)
    {
      if (date == DateTime.MaxValue)
        date -= new TimeSpan(100, 0, 0, 0);
      long ticks = date.Ticks;
      while (sort.ContainsKey((object) ticks))
        ++ticks;
      return ticks;
    }

    private void appendRecord(LogRecordBase rec)
    {
      this.records.Add((object) rec);
      this.recordTbl[(object) rec.Guid] = (object) rec;
    }

    private void appendRemovedRecord(LogRecordBase rec)
    {
      switch (rec)
      {
        case LockRequestLog _:
        case LockCancellationLog _:
        case LockDenialLog _:
        case LockRemovedLog _:
        case LockValidationLog _:
        case LockVoidLog _:
          Tracing.Log(LogList.sw, nameof (LogList), TraceLevel.Error, string.Format("Secondary Logs cannot be removed from the loan. Log Type: {0}. {1}", (object) rec.GetType().Name, (object) new StackTrace()));
          break;
        case DisclosureTrackingBase _:
          using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass"))
          {
            if (registryKey != null && !(string.Concat(registryKey.GetValue("DisclosureTrackingLogAlert")) == ""))
              break;
            throw new InvalidOperationException("DisclosureTrackingLog can not be removed from the loan.");
          }
        default:
          this.removedRecords.Add((object) rec);
          this.removedRecordTbl[(object) rec.Guid] = (object) rec;
          break;
      }
    }

    private void migrateRegistrationLogs()
    {
      RegistrationLog[] allRecordsOfType = (RegistrationLog[]) this.GetAllRecordsOfType(typeof (RegistrationLog));
      if (allRecordsOfType.Length == 0)
        return;
      foreach (RegistrationLog registrationLog in allRecordsOfType)
      {
        if (registrationLog.IsCurrent)
          return;
      }
      DateTime date = Utils.ParseDate((object) this.loan.GetField("2824"));
      for (int index = allRecordsOfType.Length - 1; index >= 0; --index)
      {
        if (allRecordsOfType[index].ExpiredDate == date)
        {
          allRecordsOfType[index].SetCurrent(true);
          break;
        }
      }
    }

    public void ToXml(
      XmlElement systemRoot,
      XmlElement nonSystemRoot,
      bool includeLoanOperationLog)
    {
      this.writeMilestoneXml(systemRoot);
      this.writeSystemLogEntries(systemRoot);
      this.writeNonSystemLogEntries(nonSystemRoot, includeLoanOperationLog);
      this.writeCRMEntries(systemRoot);
    }

    private void writeMilestoneXml(XmlElement milestoneRoot)
    {
      for (int i = 0; i < this.GetNumberOfMilestones(); ++i)
      {
        XmlElement element = milestoneRoot.OwnerDocument.CreateElement("Milestone");
        MilestoneLog milestoneAt = this.GetMilestoneAt(i);
        milestoneAt.ToXml(element);
        milestoneAt.UnsetNew();
        milestoneRoot.AppendChild((XmlNode) element);
      }
    }

    private void writeMilestoneTemplateXml(XmlElement milestoneRoot)
    {
      if (this.template == null)
        return;
      MilestoneTemplate template = this.template;
      XmlElement element = milestoneRoot.OwnerDocument.CreateElement("MilestoneTemplate");
      template.ToXml(element, this.MSLock, this.MSDateLock);
      milestoneRoot.AppendChild((XmlNode) element);
    }

    private void writeSystemLogEntries(XmlElement logRoot)
    {
      foreach (LogRecordBase record in this.records)
      {
        if (record.IsSystemSpecific())
          logRoot.AppendChild((XmlNode) this.createXmlRecord(logRoot, record));
      }
      XmlElement xmlElement = (XmlElement) null;
      foreach (LogRecordBase removedRecord in this.removedRecords)
      {
        if (removedRecord.IsSystemSpecific())
        {
          if (xmlElement == null)
          {
            xmlElement = logRoot.OwnerDocument.CreateElement("Removed");
            logRoot.AppendChild((XmlNode) xmlElement);
          }
          xmlElement.AppendChild((XmlNode) this.createXmlRecord(xmlElement, removedRecord));
        }
      }
    }

    private void writeNonSystemLogEntries(XmlElement logRoot, bool includeLoanOperationalLog)
    {
      foreach (LogRecordBase record in this.records)
      {
        if ((includeLoanOperationalLog || !record.IsLoanOperationalLog) && !record.IsSystemSpecific())
          logRoot.AppendChild((XmlNode) this.createXmlRecord(logRoot, record));
      }
      XmlElement xmlElement = (XmlElement) null;
      foreach (LogRecordBase removedRecord in this.removedRecords)
      {
        if ((includeLoanOperationalLog || !removedRecord.IsLoanOperationalLog) && !removedRecord.IsSystemSpecific())
        {
          if (xmlElement == null)
          {
            xmlElement = logRoot.OwnerDocument.CreateElement("Removed");
            logRoot.AppendChild((XmlNode) xmlElement);
          }
          xmlElement.AppendChild((XmlNode) this.createXmlRecord(xmlElement, removedRecord));
        }
      }
    }

    private void writeCRMEntries(XmlElement logRoot)
    {
      foreach (string key in this.crmTbl.Keys)
      {
        CRMLog logEntry = this.crmTbl[key];
        logRoot.AppendChild((XmlNode) this.createXmlRecord(logRoot, logEntry));
      }
    }

    private XmlElement createXmlRecord(XmlElement parent, LogRecordBase logEntry)
    {
      XmlElement e = !(logEntry.GetType() == typeof (MilestoneTemplateLog)) ? parent.OwnerDocument.CreateElement("Record") : parent.OwnerDocument.CreateElement("MilestoneTemplate");
      logEntry.ToXml(e);
      logEntry.UnsetNew();
      return e;
    }

    private XmlElement createXmlRecord(XmlElement parent, CRMLog logEntry)
    {
      XmlElement element = parent.OwnerDocument.CreateElement("CRM");
      logEntry.ToXml(element);
      return element;
    }

    private void parseLogRecords(XmlElement logRecordRoot)
    {
      ArrayList arrayList = new ArrayList();
      bool flag1 = false;
      bool flag2 = false;
      foreach (XmlElement selectNode in logRecordRoot.SelectNodes("Record"))
      {
        LogRecordBase rec = this.deserializeRecord(selectNode);
        if (rec != null)
        {
          if (rec is IDisclosureTracking2015Log)
          {
            arrayList.Add((object) rec);
            if (!flag1 && rec is EnhancedDisclosureTracking2015Log)
              flag1 = true;
            if (!flag2 && rec is DisclosureTracking2015Log)
              flag2 = true;
          }
          else
            this.appendRecord(rec);
        }
      }
      if (arrayList.Count > 0)
      {
        if (flag1 & flag2)
          arrayList.Sort();
        for (int index = 0; index < arrayList.Count; ++index)
          this.appendRecord((LogRecordBase) arrayList[index]);
      }
      foreach (XmlElement selectNode in logRecordRoot.SelectNodes("MilestoneTemplate"))
      {
        LogRecordBase rec = this.deserializeRecord(selectNode);
        if (rec != null)
          this.appendRecord(rec);
      }
      foreach (XmlElement selectNode in logRecordRoot.SelectNodes("Removed/Record"))
      {
        LogRecordBase rec = this.deserializeRecord(selectNode);
        if (rec != null)
          this.appendRemovedRecord(rec);
      }
    }

    private void parseCRMRecords(XmlNodeList records)
    {
      foreach (XmlElement record in records)
      {
        CRMLog crmLog = new CRMLog(this, record);
        if (crmLog != null)
          this.crmTbl[crmLog.MappingID] = crmLog;
      }
    }

    private void parseSystemLog(XmlElement milestoneRoot)
    {
      XmlNode e1 = milestoneRoot.SelectSingleNode("MilestoneTemplate");
      XmlNodeList milestones = milestoneRoot.SelectNodes("Milestone");
      this.template = new MilestoneTemplate((XmlElement) e1, milestones, (XmlNodeList) null);
      this.MSLock = new AttributeReader((XmlElement) e1).GetBoolean("IsTemplateLocked");
      this.MSDateLock = new AttributeReader((XmlElement) e1).GetBoolean("IsTemplateDatesLocked");
      foreach (XmlElement e2 in milestones)
        this.addMilestone(new MilestoneLog(this, e2, this.loan.MilestoneDateTimeType, this.milestoneDateCalculator));
      this.parseLogRecords(milestoneRoot);
    }

    private LogRecordBase deserializeRecord(XmlElement e)
    {
      string typeName = e.GetAttribute("Type") ?? "";
      if (typeName == "")
        return (LogRecordBase) null;
      Type typeOfRecord = LogList.getTypeOfRecord(typeName);
      if (typeOfRecord == (Type) null)
        return (LogRecordBase) null;
      try
      {
        ConstructorInfo constructor = typeOfRecord.GetConstructor(new Type[2]
        {
          this.GetType(),
          e.GetType()
        });
        if (constructor == (ConstructorInfo) null)
          return (LogRecordBase) null;
        return (LogRecordBase) constructor.Invoke(new object[2]
        {
          (object) this,
          (object) e
        });
      }
      catch (Exception ex)
      {
        Tracing.Log(LogList.sw, nameof (LogList), TraceLevel.Error, "Error deserializing log entry of type '" + typeName + "': " + (object) ex);
        if (typeName == "DisclosureTracking2015" || typeName == "EnhancedDisclosureTracking2015")
          Tracing.SendDTLogErrorMessageToServer(TraceLevel.Warning, "Disclosure Tracking - Error deserializng log entry of type '" + typeName + "' LoanGuid:" + this.loan.GUID + (object) ex);
        return (LogRecordBase) null;
      }
    }

    private static Type getTypeOfRecord(string typeName)
    {
      lock (typeof (LogList))
      {
        if (LogList.recordTypes == null)
          LogList.recordTypes = LogList.loadLogRecordTypes();
      }
      return (Type) LogList.recordTypes[(object) typeName];
    }

    private static Hashtable loadLogRecordTypes()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      Type type1 = typeof (LogRecordBase);
      foreach (Type type2 in Assembly.GetExecutingAssembly().GetTypes())
      {
        if (type1.IsAssignableFrom(type2))
        {
          if (type2 != type1)
          {
            try
            {
              FieldInfo field = type2.GetField("XmlType", BindingFlags.Static | BindingFlags.Public);
              if (field != (FieldInfo) null)
              {
                string key = (string) field.GetValue((object) null);
                insensitiveHashtable[(object) key] = (object) type2;
              }
            }
            catch
            {
            }
          }
        }
      }
      return insensitiveHashtable;
    }
  }
}

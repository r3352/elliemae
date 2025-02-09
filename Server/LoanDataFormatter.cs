// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanDataFormatter
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Serialization;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.JedLib;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using EllieMae.EMLite.Workflow;
using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class LoanDataFormatter
  {
    private const string className = "LoanDataFormatter�";
    private static b jed;

    static LoanDataFormatter()
    {
      a.a("3299879874903");
      LoanDataFormatter.jed = a.b("89rw372ttr0W3840");
    }

    public BinaryObject Serialize(LoanData data, bool includeOperationalData)
    {
      return this.Serialize(data, includeOperationalData, true);
    }

    public BinaryObject Serialize(
      LoanData data,
      bool includeOperationalData,
      bool checkContentAccess)
    {
      if (checkContentAccess && data.ContentAccess != LoanContentAccess.FullAccess)
        throw new Exception("LoanDataFormatter.Serialize() called for a loan with restricted access rights.");
      return new BinaryObject(data.ToStream(includeOperationalData), false);
    }

    public LoanEventLogList DeserializeLoanEventLog(BinaryObject data)
    {
      return new LoanEventLogList(this.ReadXmlData(data));
    }

    public BinaryObject SerializeLoanEventLog(LoanEventLogList loanEventLogList)
    {
      return new BinaryObject(loanEventLogList.ToStream(), false);
    }

    public LoanData Deserialize(BinaryObject data, string loanFolder = null)
    {
      return this.Deserialize(data, (ILoanSettings) null, loanFolder: loanFolder);
    }

    public LoanData Deserialize(
      BinaryObject data,
      ILoanSettings loanSettings,
      bool onlySavedData = false,
      string loanFolder = null)
    {
      return this.Deserialize(this.ReadXmlData(data), loanSettings, onlySavedData, loanFolder);
    }

    public LoanData Deserialize(XmlDocument xmlDoc, ILoanSettings loanSettings)
    {
      return this.Deserialize(xmlDoc, loanSettings, false);
    }

    public LoanData Deserialize(string xmlStr)
    {
      return this.Deserialize(this.LoadXmlFromStream(xmlStr.ToUtf8StreamWithBOM()), (ILoanSettings) null, false);
    }

    public LoanData Deserialize(
      XmlDocument loanDoc,
      ILoanSettings loanSettings,
      bool onlySavedData,
      string loanFolder = null)
    {
      try
      {
        if (loanSettings == null)
          loanSettings = LoanConfiguration.GetLoanSettings();
        LoanData loanData;
        if (!onlySavedData)
        {
          ConcurrentUpdateManager.DeleteAppliedMergeUpdatesForLoan(loanDoc, loanSettings.SystemID);
          IStageLoanHistoryManager loanHistories;
          ConcurrentUpdateManager.ApplyMergeUpdatesToLoan(loanDoc, loanSettings.SystemID, out loanHistories);
          loanData = new LoanData(loanDoc, loanSettings);
          if ((loanHistories != null ? new bool?(loanHistories.HistoryEntries.OfType<LoanHistoryEntry>().Any<LoanHistoryEntry>()) : new bool?()).Value)
            loanData.AttachStageLoanHistoryManager(loanHistories);
          LoanBatchUpdateAccessor.DeleteAppliedUpdatesForLoan(loanData);
          LoanBatchUpdateAccessor.ApplyBatchUpdatesToLoan(loanData);
        }
        else
          loanData = new LoanData(loanDoc, loanSettings);
        try
        {
          if (loanFolder != null && LoanFolder.GetLoanFolderType(loanFolder) == LoanFolderInfo.LoanFolderType.Archive)
          {
            loanData.SetField("5016", "Y");
          }
          else
          {
            string field = loanData.GetField("5016");
            if (!string.IsNullOrWhiteSpace(field))
            {
              if (!(field != "Y"))
                goto label_16;
            }
            if (Loan.isLoanArchived(loanData.GetField("GUID")))
              loanData.SetField("5016", "Y");
            else
              loanData.SetField("5016", "N");
          }
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (LoanDataFormatter), "Error in evaluation of field 5016 in deserialize with Error : " + ex.Message);
        }
label_16:
        if (loanData.GetLogList().GetNumberOfMilestones() == 0)
          this.prepareSystemSpecificInformation(loanData);
        LoanBatchUpdateAccessor.MigrateTpoClassicLoansToTpoConnect(loanData);
        return loanData;
      }
      catch (Exception ex)
      {
        Err.Raise(TraceLevel.Error, nameof (LoanDataFormatter), new ServerException("Error deserializing loan data", ex));
        return (LoanData) null;
      }
    }

    public XmlDocument ReadXmlData(BinaryObject possiblyEncodedData)
    {
      Stream stream = possiblyEncodedData.AsStream();
      try
      {
        return this.LoadXmlFromStream(stream);
      }
      catch
      {
        return this.LoadXmlFromEncodedStream(stream);
      }
    }

    public XmlDocument LoadXmlFromStream(Stream stream)
    {
      XmlDocument xmlDocument = new XmlDocument();
      using (TextReader reader = XmlHelper.CreateReader(stream))
        xmlDocument.Load(reader);
      return xmlDocument;
    }

    private XmlDocument LoadXmlFromEncodedStream(Stream encoded)
    {
      byte[] buffer;
      lock (LoanDataFormatter.jed)
      {
        LoanDataFormatter.jed.b();
        encoded.Seek(0L, SeekOrigin.Begin);
        buffer = LoanDataFormatter.jed.b(encoded);
      }
      using (MemoryStream memoryStream = StreamHelper.NewMemoryStream(buffer))
        return this.LoadXmlFromStream((Stream) memoryStream);
    }

    private void addRoleToMilestones(LoanData loan)
    {
      Hashtable milestoneRoles = WorkflowBpmDbAccessor.GetMilestoneRoles();
      MilestoneLog[] allMilestones = loan.GetLogList().GetAllMilestones();
      for (int index = 1; index < allMilestones.Length; ++index)
      {
        MilestoneLog milestoneLog = allMilestones[index];
        RoleSummaryInfo roleSummaryInfo = (RoleSummaryInfo) milestoneRoles[(object) milestoneLog.MilestoneID];
        if (roleSummaryInfo == null)
        {
          milestoneLog.RoleID = -1;
          milestoneLog.RoleName = "";
        }
        else
        {
          milestoneLog.RoleID = roleSummaryInfo.RoleID;
          milestoneLog.RoleName = roleSummaryInfo.RoleName;
        }
        milestoneLog.MarkAsClean();
      }
    }

    private void prepareSystemSpecificInformation(LoanData data)
    {
      LogList logList = data.GetLogList();
      MilestoneTemplate.TemplateMilestones tms = data.GetDefaultTemplate().SequentialMilestones;
      List<EllieMae.EMLite.Workflow.Milestone> milestones = WorkflowBpmDbAccessor.GetMilestones(true);
      for (int i = 0; i < tms.Count(); ++i)
      {
        EllieMae.EMLite.Workflow.Milestone milestone = milestones.Find((Predicate<EllieMae.EMLite.Workflow.Milestone>) (item => item.MilestoneID == tms[i].MilestoneID));
        if (milestone != null && !milestone.Archived)
          logList.AddMilestone(milestone.Name, milestone.DefaultDays, milestone.MilestoneID, milestone.TPOConnectStatus, milestone.ConsumerStatus);
      }
      MilestoneLog milestone1 = logList.GetMilestone("Started");
      milestone1.Done = true;
      milestone1.Date = DateTime.Now.Date;
      milestone1.Reviewed = true;
      this.addRoleToMilestones(data);
      data.WriteXml(Stream.Null, data.ContentAccess, true, true);
    }

    public void PrepareSystemSpecificInformationFromPlatform(LoanData data)
    {
      LogList logList = data.GetLogList();
      MilestoneTemplate.TemplateMilestones tms = data.GetDefaultTemplate().SequentialMilestones;
      List<EllieMae.EMLite.Workflow.Milestone> milestones = WorkflowBpmDbAccessor.GetMilestones(true);
      for (int i = 0; i < tms.Count(); ++i)
      {
        EllieMae.EMLite.Workflow.Milestone milestone = milestones.Find((Predicate<EllieMae.EMLite.Workflow.Milestone>) (item => item.MilestoneID == tms[i].MilestoneID));
        if (milestone != null && !milestone.Archived)
          logList.AddMilestone(milestone.Name, milestone.DefaultDays, milestone.MilestoneID, milestone.TPOConnectStatus, milestone.ConsumerStatus);
      }
      MilestoneLog milestone1 = logList.GetMilestone("Started");
      milestone1.Done = true;
      milestone1.Date = DateTime.Now.Date;
      milestone1.Reviewed = true;
      this.addRoleToMilestones(data);
    }

    public XmlDocument AddMissingEntityIds(BinaryObject data, Action<BinaryObject> updatefile)
    {
      XmlDocument xmlDoc = this.ReadXmlData(data);
      if (Mapping.AddMissingEntityIds(xmlDoc))
      {
        Mapping.RemoveInvalidCharFromAllRoles(xmlDoc);
        MemoryStream inputStream = StreamHelper.NewMemoryStream();
        using (XmlWriter writer = XmlHelper.CreateWriter((Stream) inputStream))
        {
          xmlDoc.Save(writer);
          inputStream.Seek(0L, SeekOrigin.Begin);
        }
        using (BinaryObject binaryObject = new BinaryObject((Stream) inputStream, false))
          updatefile(binaryObject);
      }
      return xmlDoc;
    }
  }
}

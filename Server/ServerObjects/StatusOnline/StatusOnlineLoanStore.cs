// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.StatusOnline.StatusOnlineLoanStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.Server.ServerObjects.eFolder;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.StatusOnline
{
  public class StatusOnlineLoanStore
  {
    private const string className = "StatusOnlineLoanStore�";

    public static StatusOnlineLoanSetup GetSetup(LoanIdentity loanid, bool checkTriggers)
    {
      string xmlFilePath = StatusOnlineLoanStore.getXmlFilePath(loanid);
      BinaryObject binaryObject = (BinaryObject) null;
      using (DataFile latestVersion = FileStore.GetLatestVersion(xmlFilePath))
      {
        if (latestVersion.Exists)
          binaryObject = latestVersion.GetData();
      }
      StatusOnlineLoanSetup loanSetup;
      try
      {
        loanSetup = binaryObject == null ? StatusOnlineLoanStore.migrateLoanStatusData(loanid) : (StatusOnlineLoanSetup) new XmlSerializer().Deserialize(binaryObject.AsStream(), typeof (StatusOnlineLoanSetup));
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (StatusOnlineLoanStore), "Error deserializing xml file: " + xmlFilePath + ": " + (object) ex);
        loanSetup = new StatusOnlineLoanSetup();
      }
      finally
      {
        binaryObject?.Dispose();
      }
      if (checkTriggers)
        StatusOnlineLoanStore.checkLoanTriggers(loanid, loanSetup);
      return loanSetup;
    }

    public static StatusOnlineLoanSetup SaveTriggers(
      LoanIdentity loanid,
      StatusOnlineTrigger[] triggerList)
    {
      StatusOnlineLoanSetup setup = StatusOnlineLoanStore.GetSetup(loanid, false);
      foreach (StatusOnlineTrigger trigger in triggerList)
        setup.Triggers.Add(trigger);
      StatusOnlineLoanStore.SaveSetup(loanid, setup);
      return setup;
    }

    public static StatusOnlineLoanSetup DeleteTriggers(
      LoanIdentity loanid,
      StatusOnlineTrigger[] triggerList)
    {
      StatusOnlineLoanSetup setup = StatusOnlineLoanStore.GetSetup(loanid, false);
      foreach (StatusOnlineTrigger trigger in triggerList)
        setup.Triggers.Remove(trigger);
      StatusOnlineLoanStore.SaveSetup(loanid, setup);
      return setup;
    }

    public static StatusOnlineLoanSetup SetShowPrompt(LoanIdentity loanid, bool showPrompt)
    {
      StatusOnlineLoanSetup setup = StatusOnlineLoanStore.GetSetup(loanid, false);
      setup.ShowPrompt = showPrompt;
      StatusOnlineLoanStore.SaveSetup(loanid, setup);
      return setup;
    }

    private static void checkLoanTriggers(LoanIdentity loanid, StatusOnlineLoanSetup loanSetup)
    {
      StatusOnlineTrigger[] unpublishedTriggers = loanSetup.Triggers.GetUnpublishedTriggers();
      if (unpublishedTriggers.Length == 0 && loanSetup.AppliedPersonalTriggers)
        return;
      if (string.IsNullOrEmpty(loanid.Guid))
      {
        loanid = Loan.LookupIdentity(loanid.LoanFolder, loanid.LoanName);
        if (loanid == (LoanIdentity) null)
          return;
      }
      LoanData loanData = (LoanData) null;
      using (Loan loan = LoanStore.CheckOut(loanid.Guid))
      {
        if (loan.Exists)
          loanData = loan.LoanData;
      }
      if (loanData == null)
        return;
      bool flag = false;
      if (!loanSetup.AppliedPersonalTriggers)
      {
        string simpleField = loanData.GetSimpleField("LOID");
        if (!string.IsNullOrEmpty(simpleField))
        {
          UserInfo userInfo = (UserInfo) null;
          using (User latestVersion = UserStore.GetLatestVersion(simpleField))
          {
            if (latestVersion.Exists)
              userInfo = latestVersion.UserInfo;
          }
          if (userInfo != (UserInfo) null && userInfo.PersonalStatusOnline)
          {
            foreach (StatusOnlineTrigger trigger in (CollectionBase) StatusOnlineStore.GetSetup(simpleField).Triggers)
              loanSetup.Triggers.Add(trigger);
          }
          loanSetup.AppliedPersonalTriggers = true;
          flag = true;
        }
      }
      LogList logList = loanData.GetLogList();
      DocumentTrackingSetup documentTrackingSetup = (DocumentTrackingSetup) null;
      foreach (StatusOnlineTrigger trigger in unpublishedTriggers)
      {
        DateTime dateTime = trigger.DateTriggered;
        switch (trigger.RequirementType)
        {
          case TriggerRequirementType.Milestone:
            dateTime = StatusOnlineLoanStore.checkMilestone(logList, trigger.RequirementData);
            break;
          case TriggerRequirementType.MilestoneLog:
            dateTime = StatusOnlineLoanStore.checkMilestone(logList, trigger.RequirementData);
            break;
          case TriggerRequirementType.DocumentTemplate:
            if (documentTrackingSetup == null)
              documentTrackingSetup = DocumentTrackingConfiguration.GetDocumentTrackingSetup();
            DocumentTemplate byId = documentTrackingSetup.GetByID(trigger.RequirementData);
            dateTime = byId == null ? DateTime.MinValue : StatusOnlineLoanStore.checkDocumentName(logList, byId.Name);
            break;
          case TriggerRequirementType.DocumentLog:
            dateTime = StatusOnlineLoanStore.checkDocumentLog(logList, trigger.RequirementData);
            break;
          case TriggerRequirementType.Fields:
            dateTime = StatusOnlineLoanStore.checkFields(loanData, trigger);
            break;
          case TriggerRequirementType.DocumentName:
            dateTime = StatusOnlineLoanStore.checkDocumentName(logList, trigger.RequirementData);
            break;
        }
        if (trigger.DateTriggered != dateTime)
        {
          trigger.DateTriggered = dateTime;
          flag = true;
        }
      }
      if (!flag)
        return;
      StatusOnlineLoanStore.SaveSetup(loanid, loanSetup);
    }

    private static DateTime checkMilestone(LogList logList, string milestoneID)
    {
      MilestoneLog milestoneById = logList.GetMilestoneByID(milestoneID);
      return milestoneById != null && milestoneById.Done ? milestoneById.Date.Date : DateTime.MinValue;
    }

    private static DateTime checkDocumentName(LogList logList, string docTitle)
    {
      DocumentLog[] documentsByTitle = logList.GetDocumentsByTitle(docTitle);
      return documentsByTitle.Length >= 1 && documentsByTitle[0].Received ? documentsByTitle[0].DateReceived.Date : DateTime.MinValue;
    }

    private static DateTime checkDocumentLog(LogList logList, string docID)
    {
      return logList.GetRecordByID(docID, false) is DocumentLog recordById && recordById.Received ? recordById.DateReceived.Date : DateTime.MinValue;
    }

    private static DateTime checkFields(LoanData loanData, StatusOnlineTrigger trigger)
    {
      string[] strArray = trigger.RequirementData.Split(',');
      bool flag = true;
      foreach (string id in strArray)
      {
        if (loanData.GetSimpleField(id) == string.Empty)
          flag = false;
      }
      if (!flag)
        return DateTime.MinValue;
      return trigger.DateTriggered == DateTime.MinValue ? DateTime.Now.Date : trigger.DateTriggered;
    }

    public static void SaveSetup(LoanIdentity loanid, StatusOnlineLoanSetup loanSetup)
    {
      string xmlFilePath = StatusOnlineLoanStore.getXmlFilePath(loanid);
      XmlDataStore.Serialize((object) loanSetup, xmlFilePath);
    }

    private static string getXmlFilePath(LoanIdentity loanid)
    {
      return ClientContext.GetCurrent().Settings.GetLoanFilePath(loanid.LoanFolder, loanid.LoanName, "StatusOnline.xml", false);
    }

    private static StatusOnlineLoanSetup migrateLoanStatusData(LoanIdentity loanid)
    {
      if (string.IsNullOrEmpty(loanid.Guid))
      {
        loanid = Loan.LookupIdentity(loanid.LoanFolder, loanid.LoanName);
        if (loanid == (LoanIdentity) null)
          throw new Exception("Unable to lookup the loan identity.");
      }
      LoanData loanData = (LoanData) null;
      using (Loan loan = LoanStore.CheckOut(loanid.Guid))
      {
        if (loan.Exists)
          loanData = loan.LoanData;
      }
      if (loanData == null)
        throw new Exception("Unable to get loan data: " + loanid.ToString());
      LoanAssociateLog[] assignedAssociates = loanData.GetLogList().GetAssignedAssociates(RoleInfo.FileStarter.ID);
      string ownerID = (string) null;
      if (assignedAssociates.Length != 0)
        ownerID = assignedAssociates[0].LoanAssociateID;
      LoanStatusCollection statusCollection = LoanStatusCollectionStore.Get(loanid);
      LoanStatusSettingsForLoan loanSettings = LoanStatusSettingsForLoanStore.Get(loanid);
      StatusOnlineLoanSetup loanSetup = statusCollection.MigrateData(ownerID, loanSettings, loanData);
      StatusOnlineLoanStore.SaveSetup(loanid, loanSetup);
      return loanSetup;
    }
  }
}

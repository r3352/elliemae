// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.StatusOnline.StatusOnlineManager
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.StatusOnline
{
  public class StatusOnlineManager
  {
    private const string className = "StatusOnlineManager";
    private static readonly string sw = Tracing.SwStatusOnline;
    private const string tpoConnectField = "2024";
    private const string tpoConnectFieldName = "Encompass TPO Connect";

    public static void CheckStatusOnline(LoanDataMgr loanDataMgr)
    {
      if (!Modules.IsModuleAvailableForUser(EncompassModule.StatusOnline, false))
        return;
      LoanIdentity loanIdentity = Session.LoanManager.GetLoanIdentity(loanDataMgr.LoanData.GUID);
      if (loanIdentity == (LoanIdentity) null)
        return;
      StatusOnlineLoanSetup statusOnlineSetup = Session.LoanManager.GetStatusOnlineSetup(loanIdentity);
      if (!statusOnlineSetup.ShowPrompt)
        return;
      List<StatusOnlineTrigger> automaticTriggers1;
      List<StatusOnlineTrigger> manualTriggers1;
      List<StatusOnlineTrigger> publishedTriggers1;
      StatusOnlineManager.getTriggers(loanDataMgr, statusOnlineSetup, TriggerPortalType.WebCenter, out automaticTriggers1, out manualTriggers1, out publishedTriggers1);
      List<StatusOnlineTrigger> automaticTriggers2;
      List<StatusOnlineTrigger> manualTriggers2;
      List<StatusOnlineTrigger> publishedTriggers2;
      StatusOnlineManager.getTriggers(loanDataMgr, statusOnlineSetup, TriggerPortalType.TPOWC, out automaticTriggers2, out manualTriggers2, out publishedTriggers2);
      if (automaticTriggers1.Count > 0)
      {
        bool flag = false;
        if (loanDataMgr.IsPlatformLoan(true, true))
          flag = StatusOnlineManager.PublishCCTriggers(loanDataMgr, statusOnlineSetup, automaticTriggers1, publishedTriggers1, statusOnlineSetup.ShowPrompt);
        else if (!string.IsNullOrEmpty(loanDataMgr.WCNotAllowedMessage))
        {
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, loanDataMgr.WCNotAllowedMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (!flag)
          return;
      }
      if (automaticTriggers2.Count > 0 && !StatusOnlineManager.PublishTriggers(loanDataMgr, statusOnlineSetup, automaticTriggers2, publishedTriggers2, statusOnlineSetup.ShowPrompt) || manualTriggers1.Count <= 0 && manualTriggers2.Count <= 0 || Utils.Dialog((IWin32Window) Form.ActiveForm, "The status of the currently open loan has changed. Would you like to notify borrowers and partners?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      using (StatusOnlineDialog statusOnlineDialog = new StatusOnlineDialog(loanDataMgr, statusOnlineSetup))
      {
        int num = (int) statusOnlineDialog.ShowDialog((IWin32Window) Form.ActiveForm);
      }
    }

    public static bool PublishTriggers(
      LoanDataMgr loanDataMgr,
      StatusOnlineLoanSetup statusOnlineLoanSetup,
      List<StatusOnlineTrigger> newTriggers,
      List<StatusOnlineTrigger> publishedTriggers,
      bool showPrompt)
    {
      List<StatusOnlineTrigger> statusOnlineTriggerList1 = new List<StatusOnlineTrigger>();
      List<StatusOnlineTrigger> publishTriggerList = StatusOnlineManager.getPublishTriggerList(newTriggers, publishedTriggers);
      List<StatusOnlineTrigger> statusOnlineTriggerList2 = new List<StatusOnlineTrigger>();
      List<StatusOnlineTrigger> automaticEmailTriggers = new List<StatusOnlineTrigger>();
      foreach (StatusOnlineTrigger newTrigger in newTriggers)
      {
        if (!string.IsNullOrEmpty(newTrigger.EmailTemplate))
        {
          if (newTrigger.UpdateType == TriggerUpdateType.Automatic)
            automaticEmailTriggers.Add(newTrigger);
          else if (newTrigger.UpdateType == TriggerUpdateType.Manual)
            statusOnlineTriggerList2.Add(newTrigger);
        }
      }
      string empty = string.Empty;
      StatusOnlineClient statusOnlineClient = new StatusOnlineClient(loanDataMgr);
      string packageGUID = statusOnlineClient.Publish(statusOnlineLoanSetup, publishTriggerList, showPrompt);
      if (string.IsNullOrEmpty(packageGUID))
        return false;
      if (automaticEmailTriggers.Count > 0)
        statusOnlineClient.SendAutomaticEmails(packageGUID, automaticEmailTriggers);
      foreach (StatusOnlineTrigger statusTrigger in statusOnlineTriggerList2)
      {
        using (SendEmailDialog sendEmailDialog = new SendEmailDialog(loanDataMgr, packageGUID, statusTrigger))
        {
          int num = (int) sendEmailDialog.ShowDialog((IWin32Window) Form.ActiveForm);
        }
      }
      return true;
    }

    public static bool PublishCCTriggers(
      LoanDataMgr loanDataMgr,
      StatusOnlineLoanSetup statusOnlineLoanSetup,
      List<StatusOnlineTrigger> newTriggers,
      List<StatusOnlineTrigger> publishedTriggers,
      bool showPrompt)
    {
      List<StatusOnlineTrigger> statusOnlineTriggerList1 = new List<StatusOnlineTrigger>();
      List<StatusOnlineTrigger> publishTriggerList = StatusOnlineManager.getPublishTriggerList(newTriggers, publishedTriggers);
      List<StatusOnlineTrigger> statusOnlineTriggerList2 = new List<StatusOnlineTrigger>();
      List<StatusOnlineTrigger> automaticEmailTriggers = new List<StatusOnlineTrigger>();
      foreach (StatusOnlineTrigger newTrigger in newTriggers)
      {
        if (!string.IsNullOrEmpty(newTrigger.EmailTemplate))
        {
          if (newTrigger.UpdateType == TriggerUpdateType.Automatic)
            automaticEmailTriggers.Add(newTrigger);
          else if (newTrigger.UpdateType == TriggerUpdateType.Manual)
            statusOnlineTriggerList2.Add(newTrigger);
        }
      }
      StatusOnlineClientCC statusOnlineClientCc = Session.StartupInfo.OtpSupport ? (StatusOnlineClientCC) new OTPStatusOnlineClientCC(loanDataMgr) : new StatusOnlineClientCC(loanDataMgr);
      statusOnlineClientCc.updatePublishedTriggers(publishTriggerList, showPrompt);
      if (automaticEmailTriggers.Count > 0)
        statusOnlineClientCc.SendAutomaticEmails(automaticEmailTriggers);
      foreach (StatusOnlineTrigger statusTrigger in statusOnlineTriggerList2)
      {
        if (Session.StartupInfo.OtpSupport)
        {
          using (OTPSendEmailDialogCC sendEmailDialogCc = new OTPSendEmailDialogCC(loanDataMgr, statusTrigger))
          {
            int num = (int) sendEmailDialogCc.ShowDialog((IWin32Window) Form.ActiveForm);
          }
        }
        else
        {
          using (SendEmailDialogCC sendEmailDialogCc = new SendEmailDialogCC(loanDataMgr, statusTrigger))
          {
            int num = (int) sendEmailDialogCc.ShowDialog((IWin32Window) Form.ActiveForm);
          }
        }
      }
      return true;
    }

    public static bool IsTPOConnectLoan(LoanDataMgr loanDataMgr)
    {
      return loanDataMgr.LoanData.GetSimpleField("2024").ToUpper() == "Encompass TPO Connect".ToUpper();
    }

    public static void GetTriggerSender(
      LoanDataMgr loanDataMgr,
      TriggerEmailFromType from,
      string ownerID,
      out string userid,
      out string email,
      out string name)
    {
      Tracing.Log(StatusOnlineManager.sw, TraceLevel.Verbose, nameof (StatusOnlineManager), "Inside Get Trigger Sender");
      userid = string.Empty;
      email = string.Empty;
      name = string.Empty;
      switch (from)
      {
        case TriggerEmailFromType.CurrentUser:
          Tracing.Log(StatusOnlineManager.sw, TraceLevel.Verbose, nameof (StatusOnlineManager), "Getting Current User");
          userid = Session.UserInfo.Userid;
          email = Session.UserInfo.Email;
          name = Session.UserInfo.FullName;
          Tracing.Log(StatusOnlineManager.sw, TraceLevel.Verbose, nameof (StatusOnlineManager), "User id: " + userid + " Email: " + email + " Name: " + name);
          break;
        case TriggerEmailFromType.LoanOfficer:
          Tracing.Log(StatusOnlineManager.sw, TraceLevel.Verbose, nameof (StatusOnlineManager), "Getting Loan Officer");
          userid = loanDataMgr.LoanData.GetField("LOID");
          if (string.IsNullOrEmpty(userid))
            break;
          UserInfo user1 = Session.OrganizationManager.GetUser(userid);
          if (!(user1 != (UserInfo) null))
            break;
          userid = user1.Userid;
          email = user1.Email;
          name = user1.FullName;
          Tracing.Log(StatusOnlineManager.sw, TraceLevel.Verbose, nameof (StatusOnlineManager), "User id: " + userid + " Email: " + email + " Name: " + name);
          break;
        case TriggerEmailFromType.FileStarter:
          Tracing.Log(StatusOnlineManager.sw, TraceLevel.Verbose, nameof (StatusOnlineManager), "Getting File starter");
          foreach (MilestoneLog allMilestone in loanDataMgr.LoanData.GetLogList().GetAllMilestones())
          {
            if (allMilestone.Stage == "Started")
            {
              userid = allMilestone.LoanAssociateID;
              email = allMilestone.LoanAssociateEmail;
              name = allMilestone.LoanAssociateName;
              Tracing.Log(StatusOnlineManager.sw, TraceLevel.Verbose, nameof (StatusOnlineManager), "User id: " + userid + " Email: " + email + " Name: " + name);
              break;
            }
          }
          break;
        case TriggerEmailFromType.Owner:
          if (string.IsNullOrEmpty(ownerID))
            break;
          Tracing.Log(StatusOnlineManager.sw, TraceLevel.Verbose, nameof (StatusOnlineManager), "Getting Owner");
          UserInfo user2 = Session.OrganizationManager.GetUser(ownerID);
          if (!(user2 != (UserInfo) null))
            break;
          userid = user2.Userid;
          email = user2.Email;
          name = user2.FullName;
          Tracing.Log(StatusOnlineManager.sw, TraceLevel.Verbose, nameof (StatusOnlineManager), "User id: " + userid + " Email: " + email + " Name: " + name);
          break;
      }
    }

    public static string GetTriggerRecipients(
      LoanDataMgr loanDataMgr,
      string[] toContacts,
      bool sendToAllBorrowerPair = false)
    {
      Tracing.Log(StatusOnlineManager.sw, TraceLevel.Verbose, nameof (StatusOnlineManager), "Inside Get Trigger Recipients");
      string empty = string.Empty;
      string triggerRecipients = string.Empty;
      List<string> stringList = new List<string>();
      bool flag = !string.IsNullOrEmpty(loanDataMgr.LoanData.GetField("TPO.X63"));
      if (!flag)
        flag = !string.IsNullOrEmpty(loanDataMgr.LoanData.GetField("TPO.X76"));
      foreach (string toContact in toContacts)
      {
        if (!Utils.ValidateEmail(toContact))
        {
          if (toContact == "NBO")
          {
            BorrowerPair currentBorrowerPair = loanDataMgr.LoanData.CurrentBorrowerPair;
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            VestingPartyFields[] vestingPartyFields = loanDataMgr.LoanData.GetVestingPartyFields(false);
            for (int index = 1; index <= vestingPartyFields.Length; ++index)
            {
              string simpleField1 = loanDataMgr.LoanData.GetSimpleField("TR" + index.ToString("00") + "05");
              string simpleField2 = loanDataMgr.LoanData.GetSimpleField("TR" + index.ToString("00") + "10");
              string simpleField3 = loanDataMgr.LoanData.GetSimpleField("TR" + index.ToString("00") + "99");
              if (!dictionary.ContainsKey(simpleField2) && currentBorrowerPair.Id == simpleField1)
                dictionary.Add(simpleField2, simpleField3);
            }
            int borrowingOwnerContact = loanDataMgr.LoanData.GetNumberOfNonBorrowingOwnerContact();
            for (int index = 1; index <= borrowingOwnerContact; ++index)
            {
              string simpleField4 = loanDataMgr.LoanData.GetSimpleField("NBOC" + index.ToString("00") + "99");
              if (!string.IsNullOrEmpty(simpleField4) && dictionary.ContainsKey(simpleField4))
              {
                string simpleField5 = loanDataMgr.LoanData.GetSimpleField("NBOC" + index.ToString("00") + "11");
                if (Utils.ValidateEmail(simpleField5))
                  triggerRecipients = triggerRecipients + simpleField5 + ";";
              }
            }
          }
          else
          {
            string id = toContact;
            if (flag)
            {
              switch (id.ToUpper())
              {
                case "CX.TPO.LOEMAIL":
                  id = "TPO.X63";
                  break;
                case "CX.TPO.LPEMAIL":
                  id = "TPO.X76";
                  break;
              }
            }
            else
            {
              switch (id.ToUpper())
              {
                case "TPO.X63":
                  id = "CX.TPO.LOEMAIL";
                  break;
                case "TPO.X76":
                  id = "CX.TPO.LPEMAIL";
                  break;
              }
            }
            if (sendToAllBorrowerPair)
            {
              foreach (BorrowerPair borrowerPair in loanDataMgr.LoanData.GetBorrowerPairs())
              {
                loanDataMgr.LoanData.SetBorrowerPair(borrowerPair);
                string field = loanDataMgr.LoanData.GetField(id);
                if (!string.IsNullOrEmpty(field))
                  triggerRecipients = triggerRecipients + field + ";";
              }
            }
            else
            {
              string field = loanDataMgr.LoanData.GetField(id);
              if (!string.IsNullOrEmpty(field))
                triggerRecipients = triggerRecipients + field + ";";
            }
          }
        }
        else
          triggerRecipients = triggerRecipients + toContact + ";";
      }
      if (!string.IsNullOrEmpty(triggerRecipients))
        triggerRecipients = triggerRecipients.Substring(0, triggerRecipients.Length - 1);
      Tracing.Log(StatusOnlineManager.sw, TraceLevel.Verbose, nameof (StatusOnlineManager), "Trigger Recipients: " + triggerRecipients);
      return triggerRecipients;
    }

    private static void getTriggers(
      LoanDataMgr loanDataMgr,
      StatusOnlineLoanSetup statusOnlineLoanSetup,
      TriggerPortalType portalType,
      out List<StatusOnlineTrigger> automaticTriggers,
      out List<StatusOnlineTrigger> manualTriggers,
      out List<StatusOnlineTrigger> publishedTriggers)
    {
      automaticTriggers = new List<StatusOnlineTrigger>();
      manualTriggers = new List<StatusOnlineTrigger>();
      publishedTriggers = new List<StatusOnlineTrigger>();
      if (portalType == TriggerPortalType.TPOWC)
      {
        string str = (string) null;
        try
        {
          str = loanDataMgr.LoanData.GetField("TPO.X1");
        }
        catch
        {
        }
        if (string.IsNullOrEmpty(str))
          return;
      }
      foreach (StatusOnlineTrigger trigger in (CollectionBase) statusOnlineLoanSetup.Triggers)
      {
        if (trigger.PortalType == portalType)
        {
          if (trigger.DatePublished == DateTime.MinValue)
          {
            if (trigger.DateTriggered != DateTime.MinValue || trigger.RequirementType == TriggerRequirementType.None)
            {
              if (trigger.UpdateType == TriggerUpdateType.Automatic)
                automaticTriggers.Add(trigger);
              if (trigger.ReminderType == TriggerReminderType.RemindOnExit && trigger.UpdateType == TriggerUpdateType.Manual)
                manualTriggers.Add(trigger);
            }
          }
          else
            publishedTriggers.Add(trigger);
        }
      }
    }

    private static List<StatusOnlineTrigger> getPublishTriggerList(
      List<StatusOnlineTrigger> unpublishedTriggers,
      List<StatusOnlineTrigger> publishedTriggers)
    {
      DateTime date = DateTime.Now.Date;
      List<StatusOnlineTrigger> publishTriggerList = new List<StatusOnlineTrigger>();
      foreach (StatusOnlineTrigger unpublishedTrigger in unpublishedTriggers)
      {
        if (unpublishedTrigger.DateTriggered == DateTime.MinValue)
          unpublishedTrigger.DateTriggered = date;
        unpublishedTrigger.DatePublished = date;
      }
      foreach (StatusOnlineTrigger publishedTrigger in publishedTriggers)
        publishedTrigger.DatePublished = date;
      publishTriggerList.AddRange((IEnumerable<StatusOnlineTrigger>) unpublishedTriggers);
      publishTriggerList.AddRange((IEnumerable<StatusOnlineTrigger>) publishedTriggers);
      return publishTriggerList;
    }
  }
}

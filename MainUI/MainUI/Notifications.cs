// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.Notifications
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public static class Notifications
  {
    private const string className = "Notifications";
    private static string sw = Tracing.SwCommon;
    private static bool displayingImportShippingDetails = false;
    private static bool displayingImportFundingDetails = false;
    private static bool displayingImportDocumentDetails = false;
    public const string AutoImportedConditionsMessage = "Conditions Details Auto Imported";

    public static bool PerformAction(UserNotification notification)
    {
      switch (notification)
      {
        case EPassMessageNotification _:
          return Notifications.performEPassMessageAction(notification as EPassMessageNotification);
        case LoanAlertNotification _:
          return Notifications.peformLoanAlertAction(notification as LoanAlertNotification);
        default:
          return false;
      }
    }

    public static bool PerformAction(EPassMessageAction action, string messageDescription = null)
    {
      switch (action.ActionType)
      {
        case EPassMessageActionType.Command:
          return Notifications.processMessageCommand((EPassMessageCommandAction) action, messageDescription);
        case EPassMessageActionType.EPass:
          return Notifications.processEPassSignature(((EPassMessageSignatureAction) action).EPassSignature);
        case EPassMessageActionType.URL:
          return Notifications.processMessageUrl(((EPassMessageUrlAction) action).URL);
        case EPassMessageActionType.AIQIncomeComparison:
          return Notifications.processAIQIncomeComparison((AIQIncomeComparisonAction) action);
        case EPassMessageActionType.AIQAssetComparison:
          return Notifications.processAIQAssetComparison((AIQAssetComparisonAction) action);
        case EPassMessageActionType.AIQAUSComparison:
          return Notifications.processAIQAUSComparison((AIQAUSComparisonAction) action);
        case EPassMessageActionType.AIQAuditComparison:
          return Notifications.processAIQAuditComparison((AIQAuditComparisonAction) action);
        default:
          return false;
      }
    }

    private static bool performEPassMessageAction(EPassMessageNotification ntf)
    {
      try
      {
        if ((ntf.Message.LoanGuid ?? "") != "" && !Notifications.openLoan(ntf.Message.LoanGuid))
          return false;
        if (ntf.Message.MessageType == "FILETRANSFER")
        {
          Notifications.openLoanMailbox();
          return true;
        }
        EPassMessageAction action = ntf.Message.GetAction();
        return action == null || Notifications.PerformAction(action, ntf.Message.Description);
      }
      catch (NotSupportedException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.Application, "The action required by this message is not supported in this version of Encompass.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
    }

    private static void openLeadCenter()
    {
      IBorrowerContacts service = Session.Application.GetService<IBorrowerContacts>();
      if (service == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.Application, "You do not have the necessary rights to access the Borrower Contacts feature. Contact your System Administrator for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        service.ShowLeadMailbox();
    }

    private static void retrieveEPassDocument(string epassSignature)
    {
      DocumentLog doc = (DocumentLog) null;
      foreach (DocumentLog allDocument in Session.LoanData.GetLogList().GetAllDocuments())
      {
        if (epassSignature.EndsWith(allDocument.EPASSSignature, StringComparison.CurrentCultureIgnoreCase))
        {
          doc = allDocument;
          break;
        }
      }
      IEFolder service = Session.Application.GetService<IEFolder>();
      if (doc == null)
        service.Retrieve(Session.LoanDataMgr, Session.DefaultInstance);
      else
        service.Retrieve(Session.LoanDataMgr, doc, Session.DefaultInstance);
    }

    private static void openDisclosureTracking(string packageId)
    {
      Session.Application.GetService<ILoanEditor>().ShoweDisclosureTrackingRecord(packageId);
    }

    private static void openEClose()
    {
      Session.Application.GetService<IEFolder>().LaunchEClose(Session.LoanDataMgr);
    }

    private static void openENote()
    {
      Session.Application.GetService<IEFolder>().LaunchENote(Session.LoanDataMgr);
    }

    private static void openEDMDownloadScreen()
    {
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(Session.LoanDataMgr);
      if (!folderAccessRights.CanAccessDocumentTab || !folderAccessRights.CanRetrieveDocuments)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.Application, "You do not have the necessary rights to retrieve the documents. Contact your System Administrator for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        Session.Application.GetService<IEFolder>().Retrieve(Session.LoanDataMgr, Session.DefaultInstance);
    }

    private static void openEfolderWithDocumentTab()
    {
      Session.Application.GetService<IEFolder>().ShowEfolderDialogWithDocumentTab(Session.LoanDataMgr, Session.DefaultInstance);
    }

    private static void openLoanMailbox()
    {
      Session.Application.GetService<IEncompassApplication>().OpenLoanMailbox();
    }

    private static void importConditions(
      EPassMessageCommandAction action,
      string messageDescription)
    {
      Session.Application.GetService<IEFolder>().ImportConditions(Session.LoanDataMgr, !string.IsNullOrEmpty(messageDescription) && messageDescription == "Conditions Details Auto Imported");
    }

    private static bool processMessageUrl(string url)
    {
      try
      {
        Session.Application.GetService<IEPass>()?.Navigate(url);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Notifications.sw, nameof (Notifications), TraceLevel.Error, "Error loading URL '" + url + "': " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) Session.Application, "Encompass cannot perform the requested action. The required screen is not available.");
        return false;
      }
    }

    private static bool processMessageCommand(
      EPassMessageCommandAction action,
      string messageDescription = null)
    {
      switch (action.Command.ToLower())
      {
        case "eclose":
          Notifications.openEClose();
          break;
        case "edisclosure":
          if (action.Parameters.ContainsKey("PackageGUID"))
          {
            Notifications.openDisclosureTracking(action.Parameters["PackageGUID"]);
            break;
          }
          break;
        case "edm":
          Notifications.openEDMDownloadScreen();
          break;
        case "efolder":
          Notifications.openEfolderWithDocumentTab();
          break;
        case "enote":
          Notifications.openENote();
          break;
        case "epassdoc":
          Notifications.retrieveEPassDocument(action.Parameters["Signature"]);
          break;
        case "investorconnect_conditions":
          Notifications.importConditions(action, messageDescription);
          break;
        case "investorconnect_funding":
          Notifications.importFundingDetails();
          break;
        case "investorconnect_shipping":
          Notifications.importShippingDetails();
          break;
        case "leads":
          Notifications.openLeadCenter();
          break;
        case "mailbox":
          Notifications.openLoanMailbox();
          break;
        default:
          if (!action.Command.ToLower().Contains("investorconnect_investordocuments"))
            return Notifications.setCurrentActivity(action.Command);
          Notifications.importInvestorDocumentDetails();
          return true;
      }
      return true;
    }

    private static bool setCurrentActivity(string activityName)
    {
      try
      {
        EncompassActivity activity = (EncompassActivity) Enum.Parse(typeof (EncompassActivity), activityName, true);
        return Session.Application.GetService<IEncompassApplication>().SetCurrentActivity(activity);
      }
      catch (Exception ex)
      {
        Tracing.Log(Notifications.sw, nameof (Notifications), TraceLevel.Error, "Error setting current activity '" + activityName + "': " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "Encompass cannot perform the requested action. You may not have sufficient access or it may not be supported by this version of Encompass.");
        return false;
      }
    }

    private static bool processEPassSignature(string signature)
    {
      Session.Application.GetService<IEPass>().ProcessURL(signature);
      return true;
    }

    private static bool peformLoanAlertAction(LoanAlertNotification ntf)
    {
      if (!Notifications.openLoan(ntf.LoanGuid))
        return false;
      if (ntf.Alert.AlertID == 0)
        Notifications.displayMilestoneLog(ntf.Alert.AlertTargetID);
      else if (ntf.Alert.AlertID == 30)
        Notifications.displayEFolderDocument(ntf.Alert.LogRecordID);
      else if (Session.ACL.IsAuthorizedForFeature(AclFeature.ToolsTab_SecondaryRegistration))
        Notifications.displayRateLockTool();
      else
        Notifications.displayLogRecord(ntf.Alert.LogRecordID);
      return true;
    }

    private static bool openLoan(string guid)
    {
      ILoanConsole service = Session.Application.GetService<ILoanConsole>();
      if (service == null)
        return false;
      return Session.LoanData != null && string.Compare(Session.LoanData.GUID, guid, true) == 0 || service.OpenLoan(guid, LoanInfo.LockReason.OpenForWork, true);
    }

    private static void displayMilestoneLog(string milestoneId)
    {
      MilestoneLog milestoneById = Session.LoanData.GetLogList().GetMilestoneByID(milestoneId);
      if (milestoneById == null)
        return;
      Session.Application.GetService<ILoanEditor>().ShowMilestoneWorksheet(milestoneById);
    }

    private static void displayEFolderDocument(string documentId)
    {
      if (!(Session.LoanData.GetLogList().GetRecordByID(documentId) is DocumentLog recordById))
        return;
      Session.Application.GetService<IEFolder>().View(Session.LoanDataMgr, recordById);
    }

    private static void displayRateLockTool()
    {
      Session.Application.GetService<ILoanEditor>().OpenForm("Secondary Registration");
    }

    private static void displayLogRecord(string recordId)
    {
      LogRecordBase recordById = Session.LoanData.GetLogList().GetRecordByID(recordId);
      if (recordById == null)
        return;
      Session.Application.GetService<ILoanEditor>().OpenLogRecord(recordById);
    }

    private static void importShippingDetails()
    {
      if (!Session.LoanDataMgr.Writable)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Encompass cannot perform this operation because the loan is in read-only mode. You must unlock the loan or contact your administrator for further details.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Notifications.displayingImportShippingDetails)
          return;
        try
        {
          Notifications.displayingImportShippingDetails = true;
          using (Form shippingStatusForm = ImportShippingStatusFactory.GetImportShippingStatusForm(Session.LoanDataMgr))
          {
            if (shippingStatusForm == null)
              return;
            shippingStatusForm.ShowDialog((IWin32Window) Session.MainForm);
            if (!ImportShippingStatusFactory.Success)
              return;
            DocTrackingUtils.IsImport = true;
            Session.Application.GetService<ILoanEditor>().OpenForm("Collateral Tracking");
            DocTrackingUtils.IsImport = false;
          }
        }
        catch
        {
          throw;
        }
        finally
        {
          Notifications.displayingImportShippingDetails = false;
        }
      }
    }

    private static void importFundingDetails()
    {
      if (!Session.LoanDataMgr.Writable)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Encompass cannot perform this operation because the loan is in read-only mode. You must unlock the loan or contact your administrator for further details.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Notifications.displayingImportFundingDetails)
          return;
        try
        {
          Notifications.displayingImportFundingDetails = true;
          using (Form importFundingForm = ImportFundingFactory.GetImportFundingForm(Session.LoanDataMgr))
          {
            if (importFundingForm == null)
              return;
            importFundingForm.ShowDialog((IWin32Window) Session.MainForm);
            if (!ImportFundingFactory.Success)
              return;
            Session.Application.GetService<ILoanEditor>().OpenForm("Funding Worksheet");
          }
        }
        catch
        {
          throw;
        }
        finally
        {
          Notifications.displayingImportFundingDetails = false;
        }
      }
    }

    private static void importInvestorDocumentDetails()
    {
      if (Notifications.displayingImportDocumentDetails)
        return;
      try
      {
        Notifications.displayingImportDocumentDetails = true;
        Session.Application.GetService<IEFolder>().ImportDocuments();
      }
      catch
      {
        throw;
      }
      finally
      {
        Notifications.displayingImportDocumentDetails = false;
      }
    }

    private static bool processAIQIncomeComparison(AIQIncomeComparisonAction action)
    {
      Session.Application.GetService<ILoanEditor>().ShowAIQAnalyzerMessage("Income", action.AlertDateTime, action.Description, action.MessageID);
      return true;
    }

    private static bool processAIQAssetComparison(AIQAssetComparisonAction action)
    {
      Session.Application.GetService<ILoanEditor>().ShowAIQAnalyzerMessage("Asset", action.AlertDateTime, action.Description, action.MessageID);
      return true;
    }

    private static bool processAIQAUSComparison(AIQAUSComparisonAction action)
    {
      Session.Application.GetService<ILoanEditor>().ShowAIQAnalyzerMessage("AUS", action.AlertDateTime, action.Description, action.MessageID);
      return true;
    }

    private static bool processAIQAuditComparison(AIQAuditComparisonAction action)
    {
      Session.Application.GetService<ILoanEditor>().ShowAIQAnalyzerMessage("Audit", action.AlertDateTime, action.Description, action.MessageID);
      return true;
    }
  }
}

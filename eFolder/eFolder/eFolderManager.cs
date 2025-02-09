// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eFolderManager
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Elli.Web.Host.SSF.Context;
using Elli.Web.Host.SSF.UI;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.SkyDrive;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.eFolder.Conditions;
using EllieMae.EMLite.eFolder.Documents;
using EllieMae.EMLite.eFolder.eDelivery;
using EllieMae.EMLite.eFolder.eDelivery.HelperMethods;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.eFolder.LoanCenter;
using EllieMae.EMLite.eFolder.ThinThick;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.JedScriptEngine.DisclosureTracking;
using EllieMae.EMLite.LoanUtils.EnhancedConditions;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.LoanUtils.SkyDrive;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

#nullable disable
namespace EllieMae.EMLite.eFolder
{
  public class eFolderManager : IEFolder, IMessageFilter
  {
    private const string className = "eFolderManager";
    private const string NON_CONSUMER_CONNECT_SITEID = "0";
    private static readonly string sw = Tracing.SwEFolder;
    private string packageId = "";
    private Dictionary<string, string> BarCodes = new Dictionary<string, string>();
    private IProgressFeedback feedback;
    private const string ErrorMessage = "Your connection to the Encompass Server was lost and the electronic document package was not sent. Please try again.";

    [DllImport("user32.dll")]
    private static extern IntPtr WindowFromPoint(Point pt);

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

    public string mergedPDFFilePath { get; set; }

    public LoanDataMgr loanDataManager { get; set; }

    public string loanPackageId { get; set; }

    public string loanrecordGuid { get; set; }

    public static void RegisterService()
    {
      eFolderManager service = new eFolderManager();
      Session.Application.RegisterService((object) service, typeof (IEFolder));
      Application.AddMessageFilter((IMessageFilter) service);
    }

    public FileAttachment[] Add(LoanDataMgr loanDataMgr, FormItemInfo[] formList)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrAdd", "DOCS eFolderManager.Add method");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 97, nameof (Add), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (Session.SessionObjects.RuntimeEnvironment == EllieMae.EMLite.Common.RuntimeEnvironment.Hosted && !Modules.IsModuleAvailableForUser(EncompassModule.EDM, true))
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT", 104, nameof (Add), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return (FileAttachment[]) null;
        }
        if (!new eFolderAccessRights(loanDataMgr).CanAttachForms)
        {
          PerformanceMeter.Current.AddCheckpoint("BEFORE Utils.Dialog - You do not have rights to attach forms", 114, nameof (Add), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You do not have rights to attach forms to the eFolder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          PerformanceMeter.Current.AddCheckpoint("EXIT AFTER Utils.Dialog - You do not have rights to attach forms", 117, nameof (Add), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return (FileAttachment[]) null;
        }
        FormExport formExport = new FormExport(loanDataMgr);
        EllieMae.EMLite.LoanServices.Bam bam = new EllieMae.EMLite.LoanServices.Bam(loanDataMgr);
        formExport.EntityList = bam.GetCompanyDisclosureEntities();
        try
        {
          bool skipLockRequestSync = loanDataMgr.Calculator.SkipLockRequestSync;
          loanDataMgr.Calculator.SkipLockRequestSync = true;
          loanDataMgr.Calculator.CalculateAll(false);
          loanDataMgr.Calculator.SkipLockRequestSync = skipLockRequestSync;
        }
        catch (Exception ex)
        {
          Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Error, ex.ToString());
        }
        loanDataMgr.Calculator.IsCalcAllRequired = false;
        List<FileAttachment> fileAttachmentList = new List<FileAttachment>();
        foreach (FormItemInfo form in formList)
        {
          string filepath = formExport.ExportForms(form, false);
          if (filepath != null)
          {
            FileAttachment fileAttachment = loanDataMgr.FileAttachments.AddAttachment(AddReasonType.Forms, filepath, form.GroupName, (DocumentLog) null);
            fileAttachmentList.Add(fileAttachment);
          }
        }
        loanDataMgr.Calculator.IsCalcAllRequired = true;
        if (Session.SessionObjects.RuntimeEnvironment == EllieMae.EMLite.Common.RuntimeEnvironment.Hosted)
          Transaction.Log(loanDataMgr, "Form");
        return fileAttachmentList.ToArray();
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 167, nameof (Add), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    public DocumentLog[] AppendDocumentSet(LoanDataMgr loanDataMgr)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrAppndDocSet", "DOCS eFolderManager.AppendDocumentSet local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 184, nameof (AppendDocumentSet), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (!new eFolderAccessRights(loanDataMgr).CanAddDocuments)
        {
          PerformanceMeter.Current.AddCheckpoint("BEFORE Utils.Dialog - You do not have rights", 190, nameof (AppendDocumentSet), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You do not have rights to add documents.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          PerformanceMeter.Current.AddCheckpoint("EXIT AFTER Utils.Dialog - You do not have rights", 193, nameof (AppendDocumentSet), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return (DocumentLog[]) null;
        }
        FileSystemEntry fileEntry = (FileSystemEntry) null;
        using (TemplateSelectionDialog templateSelectionDialog = new TemplateSelectionDialog(Session.DefaultInstance, EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet, (FileSystemEntry) null, false))
        {
          PerformanceMeter.Current.AddCheckpoint("new TemplateSelectionDialog", 201, nameof (AppendDocumentSet), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("BEFORE TemplateSelectionDialog ShowDialog", 203, nameof (AppendDocumentSet), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = templateSelectionDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER TemplateSelectionDialog ShowDialog - " + dialogResult.ToString(), 205, nameof (AppendDocumentSet), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          if (dialogResult == DialogResult.Cancel)
            return (DocumentLog[]) null;
          fileEntry = templateSelectionDialog.SelectedItem;
        }
        using (DocumentSetsDialog documentSetsDialog = new DocumentSetsDialog(loanDataMgr, fileEntry))
        {
          PerformanceMeter.Current.AddCheckpoint("new DocumentSetsDialog", 216, nameof (AppendDocumentSet), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("BEFORE DocumentSetsDialog ShowDialog", 218, nameof (AppendDocumentSet), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = documentSetsDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER DocumentSetsDialog ShowDialog - " + dialogResult.ToString(), 220, nameof (AppendDocumentSet), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return dialogResult == DialogResult.Cancel ? (DocumentLog[]) null : documentSetsDialog.Documents;
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 230, nameof (AppendDocumentSet), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    public DocumentLog[] SelectDocuments(LoanDataMgr loanDataMgr)
    {
      return this.SelectDocuments(loanDataMgr, loanDataMgr.LoanData.GetLogList().GetAllDocuments());
    }

    public DocumentLog[] SelectDocuments(LoanDataMgr loanDataMgr, DocumentLog[] docList)
    {
      DocumentLog[] selectedDocuments = eFolderDialog.GetSelectedDocuments();
      return this.SelectDocuments(loanDataMgr, docList, selectedDocuments, false);
    }

    public DocumentLog[] SelectDocuments(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      bool allowContinue)
    {
      DocumentLog[] selectedDocuments = eFolderDialog.GetSelectedDocuments();
      return this.SelectDocuments(loanDataMgr, docList, selectedDocuments, allowContinue);
    }

    public DocumentLog[] SelectDocuments(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      DocumentLog[] defaultList)
    {
      FileSystemEntry defaultStackingEntry = eFolderDialog.SelectedStackingOrder();
      return this.SelectDocuments(loanDataMgr, docList, defaultList, defaultStackingEntry, false);
    }

    public DocumentLog[] SelectDocuments(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      DocumentLog[] defaultList,
      bool allowContinue)
    {
      FileSystemEntry defaultStackingEntry = eFolderDialog.SelectedStackingOrder();
      return this.SelectDocuments(loanDataMgr, docList, defaultList, defaultStackingEntry, allowContinue);
    }

    public DocumentLog[] SelectDocuments(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      DocumentLog[] defaultList,
      FileSystemEntry defaultStackingEntry)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrSlctDcs1", "DOCS eFolderManager.SelectDocuments 1 local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 296, nameof (SelectDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        using (SelectDocumentsDialog selectDocumentsDialog = new SelectDocumentsDialog(loanDataMgr, docList, defaultList, defaultStackingEntry, false))
        {
          PerformanceMeter.Current.AddCheckpoint("new SelectDocumentsDialog", 301, nameof (SelectDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("BEFORE SelectDocumentsDialog ShowDialog", 303, nameof (SelectDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = selectDocumentsDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER SelectDocumentsDialog ShowDialog - " + dialogResult.ToString(), 305, nameof (SelectDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return dialogResult == DialogResult.OK ? selectDocumentsDialog.Documents : (DocumentLog[]) null;
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 314, nameof (SelectDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    public DocumentLog[] SelectDocuments(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      DocumentLog[] defaultList,
      FileSystemEntry defaultStackingEntry,
      bool allowContinue)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrSlctDcs2", "DOCS eFolderManager.SelectDocuments 2 local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 327, nameof (SelectDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        using (SelectDocumentsDialog selectDocumentsDialog = new SelectDocumentsDialog(loanDataMgr, docList, defaultList, defaultStackingEntry, allowContinue))
        {
          PerformanceMeter.Current.AddCheckpoint("new SelectDocumentsDialog", 331, nameof (SelectDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("BEFORE SelectDocumentsDialog ShowDialog", 333, nameof (SelectDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = selectDocumentsDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER SelectDocumentsDialog ShowDialog - " + dialogResult.ToString(), 335, nameof (SelectDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return dialogResult == DialogResult.OK ? selectDocumentsDialog.Documents : (DocumentLog[]) null;
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 344, nameof (SelectDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    public string SelectEVaultDocument(LoanDataMgr loanDataMgr)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrSlctEVltDoc", "DOCS eFolderManager.SelectEVaultDocument local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 357, nameof (SelectEVaultDocument), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (loanDataMgr.IsPlatformLoan())
        {
          using (eDeliveryRetrieveDialog deliveryRetrieveDialog = new eDeliveryRetrieveDialog(loanDataMgr))
          {
            PerformanceMeter.Current.AddCheckpoint("new eDeliveryRetrieveDialog", 364, nameof (SelectEVaultDocument), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            PerformanceMeter.Current.AddCheckpoint("BEFORE eDeliveryRetrieveDialog ShowDialog", 366, nameof (SelectEVaultDocument), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            DialogResult dialogResult = deliveryRetrieveDialog.ShowDialog((IWin32Window) Form.ActiveForm);
            PerformanceMeter.Current.AddCheckpoint("AFTER eDeliveryRetrieveDialog ShowDialog - " + dialogResult.ToString(), 368, nameof (SelectEVaultDocument), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            return dialogResult == DialogResult.OK ? deliveryRetrieveDialog.AuditPath : (string) null;
          }
        }
        else
        {
          using (EVaultRetrieveDialog evaultRetrieveDialog = new EVaultRetrieveDialog(loanDataMgr))
          {
            PerformanceMeter.Current.AddCheckpoint("new EVaultRetrieveDialog", 380, nameof (SelectEVaultDocument), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            PerformanceMeter.Current.AddCheckpoint("BEFORE EVaultRetrieveDialog ShowDialog", 382, nameof (SelectEVaultDocument), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            DialogResult dialogResult = evaultRetrieveDialog.ShowDialog((IWin32Window) Form.ActiveForm);
            PerformanceMeter.Current.AddCheckpoint("AFTER EVaultRetrieveDialog ShowDialog - " + dialogResult.ToString(), 384, nameof (SelectEVaultDocument), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            return dialogResult == DialogResult.OK ? evaultRetrieveDialog.AuditPath : (string) null;
          }
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 394, nameof (SelectEVaultDocument), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    public bool PreviewClosing(LoanDataMgr loanDataMgr, string[] pdfList, string[] titleList)
    {
      DocumentLog[] closingDocuments = this.getClosingDocuments(loanDataMgr, titleList);
      return this.PreviewClosing(loanDataMgr, pdfList, closingDocuments);
    }

    public bool PreviewClosing(LoanDataMgr loanDataMgr, string[] pdfList, DocumentLog[] docList)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrPrvwClsng", "DOCS eFolderManager.PreviewClosing local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 421, nameof (PreviewClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        bool barcodeDocs = BarcodeSetup.GetBarcodeSetup(Session.ISession).CheckClosingDocuments(loanDataMgr.LoanData);
        string guidString = Guid.NewGuid().ToString();
        string pdf = this.createPdf((string) null, pdfList, docList, barcodeDocs);
        if (pdf == null)
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT pdfFile == null", 433, nameof (PreviewClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, guidString, "", "", "Preview Closing");
        using (PdfPreviewDialog pdfPreviewDialog = new PdfPreviewDialog(pdf, true, true, true))
        {
          PerformanceMeter.Current.AddCheckpoint("new PdfPreviewDialog", 442, nameof (PreviewClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("BEFORE PdfPreviewDialog ShowDialog", 445, nameof (PreviewClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = pdfPreviewDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER PdfPreviewDialog ShowDialog - " + dialogResult.ToString(), 447, nameof (PreviewClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          if (dialogResult == DialogResult.Yes)
            return this.SendClosing(loanDataMgr, pdfList, docList);
          if (dialogResult == DialogResult.Ignore)
          {
            this.updateDocumentTracking(loanDataMgr, pdfList, docList, false, true, false, (ConditionLog) null);
            return loanDataMgr.Save(false);
          }
          this.updateDocumentTracking(loanDataMgr, pdfList, docList, false, true, false, (ConditionLog) null);
          foreach (DocumentLog doc in docList)
            loanDataMgr.LoanHistory.TrackChange((LogRecordBase) doc, dialogResult == DialogResult.OK ? "Doc request printed" : "Doc request preview");
          return this.createClosingDocDisclosureTracking2015Log(loanDataMgr, pdf, pdfList, docList, dialogResult == DialogResult.OK ? System.Drawing.Printing.PrintAction.PrintToPrinter : System.Drawing.Printing.PrintAction.PrintToPreview, guidString, "");
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 476, nameof (PreviewClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    public bool PrintClosing(LoanDataMgr loanDataMgr, string[] pdfList, string[] titleList)
    {
      DocumentLog[] closingDocuments = this.getClosingDocuments(loanDataMgr, titleList);
      return this.PrintClosing(loanDataMgr, pdfList, closingDocuments);
    }

    public bool PrintClosing(LoanDataMgr loanDataMgr, string[] pdfList, DocumentLog[] docList)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrPrntClsng", "DOCS eFolderManager.SelectDocuments 1 local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 499, nameof (PrintClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        bool barcodeDocs = BarcodeSetup.GetBarcodeSetup(Session.ISession).CheckClosingDocuments(loanDataMgr.LoanData);
        string str = Guid.NewGuid().ToString();
        string pdf = this.createPdf((string) null, pdfList, docList, barcodeDocs);
        if (pdf == null)
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT pdfFile == null", 511, nameof (PrintClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        using (PdfPrintDialog pdfPrintDialog = new PdfPrintDialog(pdf))
        {
          PerformanceMeter.Current.AddCheckpoint("new PdfPrintDialog", 518, nameof (PrintClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("BEFORE PdfPrintDialog ShowDialog", 520, nameof (PrintClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = pdfPrintDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER PdfPrintDialog ShowDialog - " + dialogResult.ToString(), 522, nameof (PrintClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          if (dialogResult != DialogResult.OK)
          {
            this.removeDisclosureTempLog(loanDataMgr, str);
            PerformanceMeter.Current.AddCheckpoint("EXIT", 526, nameof (PrintClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            return false;
          }
          this.updateDocumentTracking(loanDataMgr, pdfList, docList, false, true, false, (ConditionLog) null);
          foreach (DocumentLog doc in docList)
            loanDataMgr.LoanHistory.TrackChange((LogRecordBase) doc, "Doc request printed");
          return this.createClosingDocDisclosureTracking2015Log(loanDataMgr, pdf, pdfList, docList, System.Drawing.Printing.PrintAction.PrintToPrinter, str, "");
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 545, nameof (PrintClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    public bool ViewClosingOverflow(
      LoanDataMgr loanDataMgr,
      string[] pdfList,
      string[] titleList,
      string selectedTitle,
      Dictionary<string, Dictionary<int, List<string>>> overflowInfo)
    {
      DocumentLog[] closingDocuments = this.getClosingDocuments(loanDataMgr, titleList);
      return this.ViewClosingOverflow(loanDataMgr, pdfList, closingDocuments, selectedTitle, overflowInfo);
    }

    public bool ViewClosingOverflow(
      LoanDataMgr loanDataMgr,
      string[] pdfList,
      DocumentLog[] docList,
      string selectedTitle,
      Dictionary<string, Dictionary<int, List<string>>> overflowInfo)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrViewClsgOvrl", "DOCS eFolderManager.ViewDisclosures local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 565, nameof (ViewClosingOverflow), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        bool barcodeDocs = BarcodeSetup.GetBarcodeSetup(Session.ISession).CheckClosingDocuments(loanDataMgr.LoanData);
        if (this.createPdf((string) null, pdfList, docList, barcodeDocs) == null)
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT pdfFile == null", 574, nameof (ViewClosingOverflow), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        using (PdfFormSelectionDialog formSelectionDialog = new PdfFormSelectionDialog(pdfList, docList, selectedTitle, overflowInfo))
        {
          PerformanceMeter.Current.AddCheckpoint("new PdfFormSelectionDialog", 580, nameof (ViewClosingOverflow), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("BEFORE PdfFormSelectionDialog ShowDialog", 581, nameof (ViewClosingOverflow), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = formSelectionDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER PdfFormSelectionDialog ShowDialog - " + dialogResult.ToString(), 583, nameof (ViewClosingOverflow), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          if (dialogResult != DialogResult.OK)
            return false;
        }
        return true;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 591, nameof (ViewClosingOverflow), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    public bool SendClosing(LoanDataMgr loanDataMgr, string[] pdfList, string[] titleList)
    {
      DocumentLog[] closingDocuments = this.getClosingDocuments(loanDataMgr, titleList);
      return this.SendClosing(loanDataMgr, pdfList, closingDocuments);
    }

    public bool SendClosing(LoanDataMgr loanDataMgr, string[] pdfList, DocumentLog[] docList)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrSendClosing", "DOCS eFolderManager.SendClosing local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 614, nameof (SendClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        bool barcodeDocs = BarcodeSetup.GetBarcodeSetup(Session.ISession).CheckClosingDocuments(loanDataMgr.LoanData);
        string str = Guid.NewGuid().ToString();
        string pdf = this.createPdf((string) null, pdfList, docList, barcodeDocs);
        if (pdf == null)
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT - PDF not created", 626, nameof (SendClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        if (!this.isServerUp("Please contact your Encompass System Administrator about this message. Your connection to the Encompass Server was lost and the electronic document package was not sent. Do you want to retry now?\n\nClicking \"Yes\" will proceed with the transaction if connection with the Encompass server is on. Clicking \"No\" will cancel the transaction - no disclosure will be sent and Disclosure Tracking will not be updated."))
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT - server not up", 637, nameof (SendClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        string pkgID = "";
        if (loanDataMgr.IsPlatformLoan(setCCSiteId: true))
        {
          eDeliveryMessage edeliveryMessage = SendPackageFactory.CreateEDeliveryMessage(loanDataMgr, eDeliveryMessageType.SecureFormTransfer);
          edeliveryMessage.AddDocuments(pdf, docList);
          DialogResult dialogResult;
          using (SendPackageDialog sendPackageDialog = SendPackageFactory.CreateSendPackageDialog(edeliveryMessage))
          {
            DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, str, "", "", "Send Closing");
            PerformanceMeter.Current.AddCheckpoint("BEFORE SendPackageDialog ShowDialog", 653, nameof (SendClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            dialogResult = sendPackageDialog.ShowDialog((IWin32Window) Form.ActiveForm);
            PerformanceMeter.Current.AddCheckpoint("AFTER SendPackageDialog ShowDialog", 655, nameof (SendClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            pkgID = sendPackageDialog.PackageID;
          }
          if (dialogResult == DialogResult.Cancel)
          {
            this.removeDisclosureTempLog(loanDataMgr, str);
            PerformanceMeter.Current.AddCheckpoint("EXIT - DialogResult.Cancel", 672, nameof (SendClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            return false;
          }
          this.packageId = pkgID;
          Session.WriteDTLog += new EventHandler(this.Session_WriteDTLog);
          Session.stopAutoReconnect = true;
          loanDataMgr.SaveAndIgnoreRuleException(false);
          Session.WriteDTLog -= new EventHandler(this.Session_WriteDTLog);
          Session.stopAutoReconnect = false;
          return this.createClosingDocDisclosureTracking2015Log(loanDataMgr, pdf, pdfList, docList, System.Drawing.Printing.PrintAction.PrintToFile, str, pkgID);
        }
        if (!string.IsNullOrEmpty(loanDataMgr.WCNotAllowedMessage))
        {
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, loanDataMgr.WCNotAllowedMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        return false;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 692, nameof (SendClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    public void LaunchEClose(LoanDataMgr loanDataMgr)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrLauncheClose", "DOCS eFolderManager.LaunchEClose local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 702, nameof (LaunchEClose), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        ILoanServices serviceMgr = Session.Application.GetService<ILoanServices>();
        if (((int) Session.SessionObjects.isEclosingAllowed ?? (Task.Run<bool>((Func<Task<bool>>) (() => serviceMgr.IsEClosingAllowed(Session.SessionObjects))).Result ? 1 : 0)) == 0)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) Session.Application, Session.SessionObjects.ecloseMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else if (string.IsNullOrEmpty(loanDataMgr.LoanData.GetSimpleField("PlanCode.ID")) && !loanDataMgr.LoanData.GetSimpleField("1881").StartsWith("C."))
        {
          Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Verbose, "No Plan Code Selected: " + loanDataMgr.LoanData.GUID);
          int num2 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Plan code selection is required prior to proceeding with the document order. Please select the applicable plan code.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          ILoanEditor service = Session.Application.GetService<ILoanEditor>();
          if (service == null)
          {
            Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Error, "GetService<ILoanEditor> failed: " + loanDataMgr.LoanData.GUID);
            int num3 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The eClose feature was unable to acquire an instance of the Loan Editor.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else if (!service.ShowRegulationAlertsOrderDoc())
            Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Verbose, "User cancelled due to Regulation Alerts: " + loanDataMgr.LoanData.GUID);
          else if (!loanDataMgr.Save())
          {
            Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Error, "LoanDataMgr.Save failed: " + loanDataMgr.LoanData.GUID);
            int num4 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Unable to launch the eClose workflow because the loan could not be saved.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            if (!loanDataMgr.UseSkyDrive && !loanDataMgr.UseSkyDriveLite)
            {
              using (SkyDriveLiteMigrationDialog liteMigrationDialog = new SkyDriveLiteMigrationDialog(loanDataMgr))
              {
                if (!liteMigrationDialog.MigrateLoan((IWin32Window) Form.ActiveForm))
                {
                  Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Verbose, "Unable to Migrate Loan to SkyDriveLite: " + loanDataMgr.LoanData.GUID);
                  return;
                }
              }
            }
            PerformanceMeter.Current.AddCheckpoint("Creating ThinThickDialog for eClose", 770, nameof (LaunchEClose), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            using (ThinThickDialog thinThickDialog = new ThinThickDialog(loanDataMgr, loanDataMgr.SessionObjects.StartupInfo.eCloseUrl, ThinThickType.eClose))
            {
              PerformanceMeter.Current.AddCheckpoint("BEFORE ThinThickDialog.ShowDialog", 777, nameof (LaunchEClose), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
              PerformanceMeter.Current.AddCheckpoint("AFTER ThinThickDialog.ShowDialog: " + thinThickDialog.ShowDialog((IWin32Window) Form.ActiveForm).ToString(), 779, nameof (LaunchEClose), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            }
            Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Verbose, "Calling LoanDataMgr.RefreshLoanFromServer: " + loanDataMgr.LoanData.GUID);
            loanDataMgr.refreshLoanFromServer();
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Error, "LaunchEClose failure. Ex: " + ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "An unexpected error occurred during the eClose workflow.\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 795, nameof (LaunchEClose), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    public void LaunchENote(LoanDataMgr loanDataMgr)
    {
      if (Session.SettingsManager.GetServerSetting("eClose.AllowHybridWithENoteClosing").ToString() != EnableDisableSetting.Enabled.ToString())
        Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Warning, "LaunchENote called with AllowHybridWithENoteClosing NOT enabled.");
      else if (!new eFolderAccessRights(loanDataMgr).CanAccessENoteTab)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.Application, "You do not have the necessary rights to access the eNote feature. Contact your System Administrator for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        eFolderDialog.ShowInstance(Session.DefaultInstance);
        eFolderDialog.SelectedTabFromImport(ConditionType.Unknown, (ConditionLog) null);
      }
    }

    private bool createClosingDocDisclosureTracking2015Log(
      LoanDataMgr loanDataMgr,
      string pdfFile,
      string[] pdfList,
      DocumentLog[] docList,
      System.Drawing.Printing.PrintAction action,
      string guidString,
      string pkgID)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 829, nameof (createClosingDocDisclosureTracking2015Log), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        bool forLinkSync = false;
        if (Session.LoanData.LinkedData != null && (Session.LoanData.LinkSyncType == LinkSyncType.ConstructionPrimary || Session.LoanData.LinkSyncType == LinkSyncType.ConstructionLinked))
          forLinkSync = true;
        Guid disclosedGuid = this.uploadeDisclosurePDFforDisclosureTrackingRecord(loanDataMgr, pdfFile, guidString);
        this.updateDocumentTracking(loanDataMgr, pdfList, docList, false, true, false, (ConditionLog) null);
        DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, guidString, disclosedGuid.ToString(), pkgID, "Call updateDisclosureTracking");
        string logGuid = this.updateDisclosureTracking(loanDataMgr, DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder, (string[]) null, docList, disclosedGuid, (string) null, (string) null, action);
        DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, guidString, disclosedGuid.ToString(), pkgID, "New DisclosureTrackingLog:" + logGuid);
        bool disclosureTracking2015Log = loanDataMgr.SaveAndIgnoreRuleException(false, false);
        if (!disclosureTracking2015Log)
          DTErrorLogger.WriteLog(Session.UserID, DateTime.Now, nameof (createClosingDocDisclosureTracking2015Log), Session.CompanyInfo.ClientID, "");
        if (string.IsNullOrEmpty(logGuid))
          return false;
        this.checkExistanceOfDisclosureTrackingLog(loanDataMgr, logGuid, guidString, forLinkSync);
        return disclosureTracking2015Log;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 874, nameof (createClosingDocDisclosureTracking2015Log), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
      }
    }

    private DocumentLog[] getClosingDocuments(LoanDataMgr loanDataMgr, string[] titleList)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 885, nameof (getClosingDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        List<DocumentLog> documentLogList1 = new List<DocumentLog>();
        List<DocumentLog> documentLogList2 = new List<DocumentLog>();
        LogList logList = loanDataMgr.LoanData.GetLogList();
        string id = BorrowerPair.All.Id;
        DocumentLog[] allDocuments = logList.GetAllDocuments();
        foreach (string title in titleList)
        {
          string[] separator = new string[2]{ "\n", "\r\n" };
          string[] source = title.Split(separator, StringSplitOptions.RemoveEmptyEntries);
          string name = source[0];
          string empty = string.Empty;
          if (((IEnumerable<string>) source).Count<string>() > 1)
            empty = source[1];
          DocumentLog documentLog1 = (DocumentLog) null;
          bool flag = false;
          foreach (DocumentLog logEntry in allDocuments)
          {
            if (!(logEntry.Title != name) && !(logEntry.PairId != id))
            {
              foreach (DocumentLog documentLog2 in documentLogList2)
              {
                if (logEntry.Guid == documentLog2.Guid)
                {
                  flag = true;
                  break;
                }
              }
              if (!flag && new eFolderAccessRights(loanDataMgr, (LogRecordBase) logEntry).CanBrowseAttach)
              {
                documentLog1 = logEntry;
                documentLogList2.Add(logEntry);
                break;
              }
              if (flag)
                flag = false;
            }
          }
          if (documentLog1 == null)
          {
            DocumentTemplate byName = loanDataMgr.SystemConfiguration.DocumentTrackingSetup.GetByName(name);
            documentLog1 = byName == null ? new DocumentLog(Session.UserID, id) : byName.CreateLogEntry(Session.UserID, id);
            documentLog1.Title = name;
            documentLog1.Stage = logList.NextStage;
          }
          if (!string.IsNullOrEmpty(empty))
            documentLog1.GroupName = empty;
          documentLogList1.Add(documentLog1);
        }
        return documentLogList1.ToArray();
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 967, nameof (getClosingDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
      }
    }

    public void LaunchEDisclosures(LoanDataMgr loanDataMgr, int height, int width)
    {
      string eDisclosuresUrl = Session.DefaultInstance.StartupInfo.eDisclosuresUrl;
      SSFGuest guestInfo = new SSFGuest()
      {
        uri = eDisclosuresUrl,
        scope = "dis",
        clientId = "09apljdis"
      };
      SSFContext context = SSFContext.Create(loanDataMgr, SSFHostType.Network, guestInfo);
      if (context == null)
        return;
      context.parameters = new Dictionary<string, object>()
      {
        {
          "hostname",
          (object) "smartclient"
        },
        {
          "instanceId",
          (object) Session.ServerIdentity.InstanceName
        },
        {
          "errorMessages",
          (object) new List<string>()
        }
      };
      using (SSFDialog ssfDialog = new SSFDialog(context))
      {
        ssfDialog.Text = "Disclosures";
        ssfDialog.Height = Convert.ToInt32(height);
        ssfDialog.Width = Convert.ToInt32(width);
        ssfDialog.ShowDialog((IWin32Window) Session.MainForm);
      }
    }

    public bool PreviewDisclosures(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      string[] titleList,
      string[] packageList)
    {
      DocumentLog[] disclosureDocuments = this.getDisclosureDocuments(loanDataMgr, titleList);
      return this.PreviewDisclosures(loanDataMgr, coversheetFile, pdfList, packageList, disclosureDocuments, (DocumentLog[]) null, (ConditionLog) null);
    }

    public bool PreviewDisclosures(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      DocumentLog[] docList,
      DocumentLog[] neededList,
      ConditionLog cond)
    {
      return this.PreviewDisclosures(loanDataMgr, coversheetFile, pdfList, (string[]) null, docList, neededList, cond);
    }

    public bool PreviewDisclosures(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      string[] packageList,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      ConditionLog cond)
    {
      bool flag1 = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrPrvwDsclsrs", "DOCS eFolderManager.PreviewDisclosures local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 1033, nameof (PreviewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        bool forLinkSync = false;
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        if (Session.LoanData.LinkedData != null && (Session.LoanData.LinkSyncType == LinkSyncType.ConstructionPrimary || Session.LoanData.LinkSyncType == LinkSyncType.ConstructionLinked))
          forLinkSync = true;
        bool barcodeDocs = BarcodeSetup.GetBarcodeSetup(Session.ISession).CheckOpeningDocuments(loanDataMgr.LoanData);
        string str = Guid.NewGuid().ToString();
        DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, nameof (PreviewDisclosures), str);
        string pdf = this.createPdf(coversheetFile, pdfList, signList, barcodeDocs);
        if (pdf == null)
        {
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "Failed to create PDF file.", str);
          PerformanceMeter.Current.AddCheckpoint("EXIT Failed to create PDF file", 1057, nameof (PreviewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        Guid disclosedGuid = this.uploadeDisclosurePDFforDisclosureTrackingRecord(loanDataMgr, pdf, str);
        DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, str, disclosedGuid.ToString(), "", nameof (PreviewDisclosures));
        using (PdfPreviewDialog pdfPreviewDialog = new PdfPreviewDialog(pdf, true, true, true))
        {
          PerformanceMeter.Current.AddCheckpoint("new PdfPreviewDialog", 1070, nameof (PreviewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("BEFORE PdfPreviewDialog ShowDialog", 1073, nameof (PreviewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = pdfPreviewDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER PdfPreviewDialog ShowDialog - " + dialogResult.ToString(), 1075, nameof (PreviewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "Result from preview:" + Enum.GetName(typeof (DialogResult), (object) dialogResult), str);
          if (dialogResult == DialogResult.Yes)
            return this.SendDisclosures(loanDataMgr, coversheetFile, pdfList, packageList, signList, neededList, cond);
          if (dialogResult == DialogResult.Ignore)
            dialogResult = DialogResult.OK;
          if (dialogResult == DialogResult.OK)
          {
            DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "Create DocumentTracking.", str);
            this.updateDocumentTracking(loanDataMgr, pdfList, signList, true, false, false, cond);
            this.updateDocumentTracking(loanDataMgr, (string[]) null, neededList, true, false, false, cond);
            DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, str, disclosedGuid.ToString(), "", "Call updateDisclosureTracking.");
            string logGuid = this.updateDisclosureTracking(loanDataMgr, DisclosureTrackingBase.DisclosedMethod.ByMail, packageList, signList, disclosedGuid, (string) null, (string) null);
            DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, str, disclosedGuid.ToString(), "", "New DisclosureTrackingLog:" + logGuid);
            PerformanceMeter.Current.AddCheckpoint("Wrote Disclosure Tracking Logs", 1104, nameof (PreviewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            bool flag2 = loanDataMgr.Save(false, false);
            if (!flag2)
              DTErrorLogger.WriteLog(Session.UserID, DateTime.Now, nameof (PreviewDisclosures), Session.CompanyInfo.ClientID, "");
            this.checkExistanceOfDisclosureTrackingLog(loanDataMgr, logGuid, str, forLinkSync);
            return flag2;
          }
          this.removeDisclosureTempLog(loanDataMgr, str);
          PerformanceMeter.Current.AddCheckpoint("removeDisclosureTempLog", 1120, nameof (PreviewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 1128, nameof (PreviewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag1)
          PerformanceMeter.Current.Stop();
      }
    }

    public bool PrintDisclosures(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      string[] titleList,
      string[] packageList)
    {
      DocumentLog[] disclosureDocuments = this.getDisclosureDocuments(loanDataMgr, titleList);
      return this.PrintDisclosures(loanDataMgr, coversheetFile, pdfList, packageList, disclosureDocuments, (DocumentLog[]) null, (ConditionLog) null);
    }

    public bool PrintDisclosures(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      DocumentLog[] docList,
      DocumentLog[] neededList,
      ConditionLog cond)
    {
      return this.PrintDisclosures(loanDataMgr, coversheetFile, pdfList, (string[]) null, docList, neededList, cond);
    }

    public bool PrintDisclosures(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      string[] packageList,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      ConditionLog cond)
    {
      bool flag1 = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrPrntDsclsrs", "DOCS eFolderManager.PrintDisclosures local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 1157, nameof (PrintDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        bool forLinkSync = false;
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        bool flag2 = false;
        if (Session.LoanData.LinkedData != null && (Session.LoanData.LinkSyncType == LinkSyncType.ConstructionPrimary || Session.LoanData.LinkSyncType == LinkSyncType.ConstructionLinked))
          forLinkSync = true;
        bool barcodeDocs = BarcodeSetup.GetBarcodeSetup(Session.ISession).CheckOpeningDocuments(loanDataMgr.LoanData);
        string str = Guid.NewGuid().ToString();
        DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, nameof (PrintDisclosures), str);
        string pdf = this.createPdf(coversheetFile, pdfList, signList, barcodeDocs);
        if (pdf == null)
        {
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "Failed to create PDF file.", str);
          PerformanceMeter.Current.AddCheckpoint("EXIT - ndf not created", 1182, nameof (PrintDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        Guid guid = this.uploadeDisclosurePDFforDisclosureTrackingRecord(loanDataMgr, pdf, str);
        using (PdfPrintDialog pdfPrintDialog = new PdfPrintDialog(pdf))
        {
          PerformanceMeter.Current.AddCheckpoint("BEFORE PdfPrintDialog ShowDialog", 1193, nameof (PrintDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = pdfPrintDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER PdfPrintDialog ShowDialog - " + dialogResult.ToString(), 1195, nameof (PrintDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "Result from print:" + Enum.GetName(typeof (DialogResult), (object) dialogResult), str);
          if (dialogResult != DialogResult.OK)
          {
            this.removeDisclosurePDFforDisclosureTrackingRecord(loanDataMgr, guid);
            this.removeDisclosureTempLog(loanDataMgr, str);
            PerformanceMeter.Current.AddCheckpoint("EXIT result != DialogResult.OK", 1203, nameof (PrintDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            return false;
          }
          this.updateDocumentTracking(loanDataMgr, pdfList, signList, true, false, false, cond);
          this.updateDocumentTracking(loanDataMgr, (string[]) null, neededList, true, false, false, cond);
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "Call updateDisclosureTracking.", str);
          string logGuid = this.updateDisclosureTracking(loanDataMgr, DisclosureTrackingBase.DisclosedMethod.ByMail, packageList, signList, guid, (string) null, (string) null);
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "New DisclosureTrackingLog:" + logGuid, str);
          flag2 = loanDataMgr.Save(false, false);
          if (!flag2)
            DTErrorLogger.WriteLog(Session.UserID, DateTime.Now, nameof (PrintDisclosures), Session.CompanyInfo.ClientID, "");
          this.checkExistanceOfDisclosureTrackingLog(loanDataMgr, logGuid, str, forLinkSync);
        }
        return flag2;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 1233, nameof (PrintDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag1)
          PerformanceMeter.Current.Stop();
      }
    }

    private void checkExistanceOfDisclosureTrackingLog(
      LoanDataMgr loanDataMgr,
      string logGuid,
      string transactionGuidString,
      bool forLinkSync)
    {
      if (logGuid == "")
        return;
      IDisclosureTracking2015Log disclosureTracking2015Log = new List<IDisclosureTracking2015Log>((IEnumerable<IDisclosureTracking2015Log>) loanDataMgr.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(true)).FirstOrDefault<IDisclosureTracking2015Log>((Func<IDisclosureTracking2015Log, bool>) (x => x.Guid == logGuid));
      if (disclosureTracking2015Log == null)
      {
        DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "Log Guid " + logGuid + " is not found.", transactionGuidString);
        Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Error, "DT Record Not Created: " + this.getDisclosureTempLog(loanDataMgr, transactionGuidString));
      }
      else
      {
        DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "Log Guid " + logGuid + " is found.", transactionGuidString);
        this.removeDisclosureTempLog(loanDataMgr, transactionGuidString);
        if (!forLinkSync)
          return;
        this.updateLinkedLog(loanDataMgr.LinkedLoan, logGuid, transactionGuidString, disclosureTracking2015Log.DisclosedDate);
        disclosureTracking2015Log.LinkedGuid = logGuid;
        loanDataMgr.SaveAndIgnoreRuleException(false);
      }
    }

    private void updateLinkedLog(
      LoanDataMgr loanDataMgr,
      string logGuid,
      string transactionGuidString,
      DateTime discloseddt)
    {
      if (logGuid == "")
        return;
      List<IDisclosureTracking2015Log> source = new List<IDisclosureTracking2015Log>((IEnumerable<IDisclosureTracking2015Log>) loanDataMgr.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(true));
      IDisclosureTracking2015Log disclosureTracking2015Log = source.FirstOrDefault<IDisclosureTracking2015Log>((Func<IDisclosureTracking2015Log, bool>) (x => x.Guid == logGuid));
      if (source.FirstOrDefault<IDisclosureTracking2015Log>((Func<IDisclosureTracking2015Log, bool>) (x => x.Guid == logGuid)) == null)
      {
        DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "Log Guid " + logGuid + " is not found.", transactionGuidString);
        Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Error, "DT Record Not Created: " + this.getDisclosureTempLog(loanDataMgr, transactionGuidString));
      }
      else
      {
        this.removeDisclosureTempLog(loanDataMgr, transactionGuidString);
        if (logGuid.Length <= 0)
          return;
        disclosureTracking2015Log.LinkedGuid = logGuid;
        disclosureTracking2015Log.DisclosedDate = discloseddt;
      }
    }

    public bool SendDisclosures(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      string[] titleList,
      string[] packageList)
    {
      DocumentLog[] disclosureDocuments = this.getDisclosureDocuments(loanDataMgr, titleList);
      return this.SendDisclosures(loanDataMgr, coversheetFile, pdfList, packageList, disclosureDocuments, (DocumentLog[]) null, (ConditionLog) null);
    }

    public bool SendDisclosures(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      DocumentLog[] docList,
      DocumentLog[] neededList,
      ConditionLog cond)
    {
      return this.SendDisclosures(loanDataMgr, coversheetFile, pdfList, (string[]) null, docList, neededList, cond);
    }

    public bool SendDisclosures(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      string[] packageList,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      ConditionLog cond)
    {
      bool flag1 = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrSendDisc", "DOCS eFolderManager.SendDisclosures");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 1315, nameof (SendDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        try
        {
          PerformanceMeter.Current.AddVariable("DocumentCount", (object) pdfList.Length);
          PerformanceMeter.Current.AddVariable("SessionId", (object) Session.ISession.SessionID);
          PerformanceMeter.Current.AddVariable("UserId", (object) Session.ISession.UserID);
        }
        catch
        {
        }
        bool forLinkSync = false;
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        if (Session.LoanData.LinkedData != null && (Session.LoanData.LinkSyncType == LinkSyncType.ConstructionPrimary || Session.LoanData.LinkSyncType == LinkSyncType.ConstructionLinked))
          forLinkSync = true;
        if (BarcodeSetup.GetBarcodeSetup(Session.ISession).CheckOpeningDocuments(loanDataMgr.LoanData))
          this.addBarcodes(loanDataMgr, pdfList, signList);
        string str1 = Guid.NewGuid().ToString();
        Task<string> task = Task.Run<string>((Func<string>) (() => this.createPdf(coversheetFile, pdfList)));
        if (!this.isServerUp("Please contact your Encompass System Administrator about this message. Your connection to the Encompass Server was lost and the electronic document package was not sent. Do you want to retry now?\n\nClicking \"Yes\" will proceed with the transaction if connection with the Encompass server is on. Clicking \"No\" will cancel the transaction - no disclosure will be sent and Disclosure Tracking will not be updated."))
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT - not isServerUp", 1351, nameof (SendDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        Task.WaitAll((Task) task);
        string result = task.Result;
        if (result == null || !File.Exists(result))
        {
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "Failed to create PDF file.", str1);
          PerformanceMeter.Current.AddCheckpoint("EXIT - Failed to create Coversheet", 1360, nameof (SendDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        string str2 = "";
        if (loanDataMgr.IsPlatformLoan(setCCSiteId: true))
        {
          DialogResult dialogResult;
          string comments;
          bool isWetSign;
          using (SendPackageDialog sendPackageDialog = SendPackageFactory.CreateSendPackageDialog(eDeliveryMessageType.InitialDisclosures, loanDataMgr, coversheetFile, signList, neededList, pdfList))
          {
            DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, str1, string.Empty, "", "Send Disclosures");
            if (sendPackageDialog.NoOriginatorCancel)
            {
              this.removeDisclosureTempLog(loanDataMgr, str1);
              PerformanceMeter.Current.AddCheckpoint("EXIT - ConsumerComment NoOriginatorCancel", 1377, nameof (SendDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
              return false;
            }
            PerformanceMeter.Current.AddCheckpoint("BEFORE ConsumerComment SendPackageDialog ShowDialog()", 1381, nameof (SendDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            dialogResult = sendPackageDialog.ShowDialog((IWin32Window) Form.ActiveForm);
            PerformanceMeter.Current.AddCheckpoint("AFTER ConsumerComment SendPackageDialog ShowDialog()", 1383, nameof (SendDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            str2 = sendPackageDialog.PackageID;
            comments = sendPackageDialog.Comments;
            isWetSign = sendPackageDialog.IsWetSign;
          }
          this.packageId = str2;
          Session.WriteDTLog += new EventHandler(this.Session_WriteDTLog);
          Session.stopAutoReconnect = true;
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "Result from send:" + Enum.GetName(typeof (DialogResult), (object) dialogResult), str1);
          if (dialogResult != DialogResult.OK)
          {
            this.removeDisclosureTempLog(loanDataMgr, str1);
            Session.WriteDTLog -= new EventHandler(this.Session_WriteDTLog);
            Session.stopAutoReconnect = false;
            PerformanceMeter.Current.AddCheckpoint("EXIT - result: " + dialogResult.ToString(), 1408, nameof (SendDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            return false;
          }
          this.updateDocumentTracking(loanDataMgr, pdfList, signList, true, false, false, cond);
          this.updateDocumentTracking(loanDataMgr, (string[]) null, neededList, true, false, false, cond);
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, str1, string.Empty, str2, "Call updateDisclosureTracking");
          Guid disclosedGuid = this.uploadeDisclosurePDFforDisclosureTrackingRecord(loanDataMgr, result, str1);
          PerformanceMeter.Current.AddCheckpoint("Upload CoverSheet", 1421, nameof (SendDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          string logGuid = this.updateDisclosureTracking(loanDataMgr, DisclosureTrackingBase.DisclosedMethod.eDisclosure, packageList, signList, disclosedGuid, str2, comments, isWetSign);
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, str1, disclosedGuid.ToString(), str2, "New DisclosureTrackingLog:" + logGuid);
          if (string.IsNullOrWhiteSpace(str2))
            RemoteLogger.Write(TraceLevel.Info, string.Format("Instance Id: {0}, Loan GUID: {1}, Date / Time: {2}, User id: {3}, Keyword : \"ePackage Id blank\"", (object) Session.CompanyInfo.ClientID, (object) Session.LoanData.GUID, (object) DateTime.Now, (object) Session.UserID));
          bool flag2 = loanDataMgr.SaveAndIgnoreRuleException(false, false);
          this.checkExistanceOfDisclosureTrackingLog(loanDataMgr, logGuid, str1, forLinkSync);
          Session.WriteDTLog -= new EventHandler(this.Session_WriteDTLog);
          Session.stopAutoReconnect = false;
          return flag2;
        }
        if (!string.IsNullOrEmpty(loanDataMgr.WCNotAllowedMessage))
        {
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, loanDataMgr.WCNotAllowedMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        return false;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 1449, nameof (SendDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag1)
          PerformanceMeter.Current.Stop();
      }
    }

    private void addBarcodes(LoanDataMgr loanDataMgr, string[] pdfList, DocumentLog[] signList)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 1462, nameof (addBarcodes), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (Session.DefaultInstance.StartupInfo.AllowDRSBarCoding)
        {
          List<BarcodeMetadataInput> barcodeMetadataInputs = new List<BarcodeMetadataInput>();
          Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Info, "Starting Parallel Loop for DRS Barcodes: " + (object) pdfList.Length);
          Parallel.For(0, pdfList.Length, (Action<int>) (index =>
          {
            DocumentLog sign = signList[index];
            Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Info, "Checking For Existing DRS Barcode: " + sign.Guid);
            if (this.BarCodes.ContainsKey(sign.Guid))
              return;
            BarcodeMetadata barcodeMetadata = new BarcodeMetadata()
            {
              eFolderDocTitle = sign.Title,
              edsAttributes = new EdsAttributes()
            };
            barcodeMetadata.edsAttributes.documentCategory = sign.GroupName;
            XDocument xdocument = (XDocument) null;
            try
            {
              using (PdfEditor pdfEditor = new PdfEditor(pdfList[index]))
              {
                string barcodeSpec = pdfEditor.BarcodeSpec;
                if (!string.IsNullOrEmpty(barcodeSpec))
                  xdocument = XDocument.Parse(barcodeSpec);
              }
            }
            catch (Exception ex)
            {
              RemoteLogger.Write(TraceLevel.Error, "eFolderManager: Error in addBarcodes. Ex: " + (object) ex);
            }
            if (xdocument != null)
            {
              string str1 = this.ReadXmlAttributeValue(xdocument.Root.Attribute((XName) "RequestGuid"), "RequestGuid");
              if (!string.IsNullOrEmpty(str1))
                barcodeMetadata.edsAttributes.requestId = str1;
              string str2 = this.ReadXmlAttributeValue(xdocument.Root.Attribute((XName) "DocumentID"), "DocumentID");
              if (!string.IsNullOrEmpty(str2))
                barcodeMetadata.edsAttributes.documentId = str2;
              string s1 = this.ReadXmlAttributeValue(xdocument.Root.Attribute((XName) "EdsTemplateVersion"), "EdsTemplateVersion");
              int result1;
              if (!string.IsNullOrEmpty(s1) && int.TryParse(s1, out result1))
                barcodeMetadata.edsAttributes.templateVersion = result1;
              string s2 = this.ReadXmlAttributeValue(xdocument.Root.Attribute((XName) "EdsClassVersion"), "EdsClassVersion");
              int result2;
              if (!string.IsNullOrEmpty(s1) && int.TryParse(s2, out result2))
                barcodeMetadata.edsAttributes.templateClassVersion = result2;
              string str3 = this.ReadXmlAttributeValue(xdocument.Root.Attribute((XName) "EdsTemplateID"), "EdsTemplateID");
              if (!string.IsNullOrEmpty(str3))
                barcodeMetadata.edsAttributes.templateName = str3;
              string str4 = this.ReadXmlAttributeValue(xdocument.Root.Attribute((XName) "InputTracked"), "InputTracked");
              bool result3;
              if (!string.IsNullOrEmpty(str4) && bool.TryParse(str4, out result3))
                barcodeMetadata.edsAttributes.trackingEnabled = result3;
            }
            BarcodeMetadataInput barcodeMetadataInput = new BarcodeMetadataInput();
            barcodeMetadataInput.documentId = sign.Guid;
            DocumentTemplate byName = loanDataMgr.SystemConfiguration.DocumentTrackingSetup.GetByName(sign.Title);
            if (byName != null)
            {
              barcodeMetadataInput.documentName = byName.Source;
              barcodeMetadataInput.documentType = byName.SourceType;
            }
            barcodeMetadataInput.metadata = JsonConvert.SerializeObject((object) barcodeMetadata);
            lock (barcodeMetadataInputs)
            {
              Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Info, "Storing DRS Barcode Metadata: " + sign.Guid);
              barcodeMetadataInputs.Add(barcodeMetadataInput);
            }
          }));
          Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Info, "Checking For DRS Barcodes to Register: " + (object) barcodeMetadataInputs.Count);
          if (barcodeMetadataInputs.Count > 0)
          {
            PerformanceMeter.Current.AddCheckpoint("Get Barcodes From DRS", 1574, nameof (addBarcodes), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            Task<BarcodeIdOutput[]> barcodes = new DRSServiceClient(loanDataMgr).CreateBarcodes(barcodeMetadataInputs.ToArray());
            Task.WaitAll((Task) barcodes);
            foreach (BarcodeIdOutput barcodeIdOutput in barcodes.Result)
              this.BarCodes[barcodeIdOutput.documentId] = barcodeIdOutput.barcodeId;
          }
        }
        PerformanceMeter.Current.AddCheckpoint("Apply Barcodes To Documents", 1588, nameof (addBarcodes), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Info, "Starting Parallel Loop for Applying Barcodes: " + (object) pdfList.Length);
        Parallel.For(0, pdfList.Length, (Action<int>) (index =>
        {
          try
          {
            using (PdfEditor pdfEditor = new PdfEditor(pdfList[index]))
            {
              DocumentLog sign = signList[index];
              if (this.BarCodes.ContainsKey(sign.Guid))
                pdfList[index] = pdfEditor.AddBarcode(sign, this.BarCodes[sign.Guid]);
              else
                pdfList[index] = pdfEditor.AddBarcode(sign);
            }
          }
          catch (Exception ex)
          {
            RemoteLogger.Write(TraceLevel.Error, "eFolderManager: Error in addBarcodes. Ex: " + (object) ex);
          }
        }));
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "eFolderManager: Error in addBarcodes. Ex: " + (object) ex);
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 1624, nameof (addBarcodes), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
      }
    }

    private string ReadXmlAttributeValue(XAttribute att, string attName) => att?.Value;

    private void Session_WriteDTLog(object sender, EventArgs e)
    {
      if (this.packageId == null)
        this.packageId = "";
      DTErrorLogger.WriteLog(Session.UserID, DateTime.Now, "SendDisclosures", Session.CompanyInfo.ClientID, this.packageId);
    }

    private static void CheckServerUp()
    {
      int num = Session.ServerRealTime != DateTime.MinValue ? 1 : 0;
    }

    private static bool RunWithTimeout(ThreadStart threadStart, TimeSpan timeout)
    {
      Thread thread = new Thread(threadStart);
      thread.Start();
      bool flag = thread.Join(timeout);
      if (!flag)
        thread.Abort();
      return flag;
    }

    public bool isServerUp(string msg)
    {
      bool flag1 = false;
      for (bool flag2 = true; flag2; flag2 = true)
      {
        if (eFolderManager.RunWithTimeout(new ThreadStart(eFolderManager.CheckServerUp), TimeSpan.FromSeconds(5.0)))
        {
          flag1 = true;
          break;
        }
        if (Utils.Dialog((IWin32Window) Form.ActiveForm, msg, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        {
          flag1 = false;
          break;
        }
      }
      return flag1;
    }

    public string UpdateSupportingData(
      LoanDataMgr loanDataMgr,
      string loanGuid,
      string packageId,
      string supportingKey)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrViewDcsl", "DOCS eFolderManager.UpdateSupportingData local meter");
      try
      {
        string mergedPdfFile = string.Empty;
        loanGuid = loanGuid.Replace("{", "").Replace("}", "");
        EDeliveryServiceClient eDeliveryClient = new EDeliveryServiceClient(loanDataMgr);
        EllieMae.EMLite.eFolder.eDelivery.Request packageDetails = (EllieMae.EMLite.eFolder.eDelivery.Request) null;
        int num1 = 0;
        this.feedback.Increment(20);
        while (num1 < 5)
        {
          try
          {
            packageDetails = eDeliveryClient.GetPackageDetails(loanGuid, packageId).Result;
            if (packageDetails == null)
              return mergedPdfFile;
            num1 = 6;
          }
          catch (Exception ex)
          {
            ++num1;
            Tracing.Log(eFolderManager.sw, TraceLevel.Error, nameof (eFolderManager), "Exception in UpdateSupportingData : " + ex.ToString());
            if (num1 == 5)
              return (string) null;
          }
        }
        string path = Path.GetTempPath() + packageId;
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        if (directoryInfo.Exists)
          directoryInfo.Delete(true);
        directoryInfo.Create();
        this.VerifyEncompassServerConnection();
        ConcurrentDictionary<string, string> pdfListTobeDownloaded = new ConcurrentDictionary<string, string>();
        foreach (EllieMae.EMLite.eFolder.eDelivery.Document document in packageDetails.documents)
        {
          if (!string.Equals(document.type, "coversheet", StringComparison.OrdinalIgnoreCase) && !string.Equals(document.type, "Needed", StringComparison.OrdinalIgnoreCase))
            pdfListTobeDownloaded.TryAdd(document.id, (string) null);
        }
        this.feedback.Increment(25);
        Parallel.ForEach<EllieMae.EMLite.eFolder.eDelivery.Document>((IEnumerable<EllieMae.EMLite.eFolder.eDelivery.Document>) packageDetails.documents, (Action<EllieMae.EMLite.eFolder.eDelivery.Document>) (document =>
        {
          try
          {
            if (string.Equals(document.type, "coversheet", StringComparison.OrdinalIgnoreCase) || string.Equals(document.type, "Needed", StringComparison.OrdinalIgnoreCase))
              return;
            Task<string> task = eDeliveryClient.DownloadFilesFromMediaServer(packageDetails, loanGuid, packageId, document.id, directoryInfo.FullName);
            pdfListTobeDownloaded[document.id] = task.Result;
          }
          catch (AggregateException ex)
          {
            Tracing.Log(eFolderManager.sw, TraceLevel.Error, nameof (eFolderManager), "Exception in UpdateSupportingData : ");
            foreach (Exception innerException in ex.Flatten().InnerExceptions)
              Tracing.Log(eFolderManager.sw, TraceLevel.Error, nameof (eFolderManager), innerException.ToString());
          }
        }));
        IEnumerable<KeyValuePair<string, string>> source = pdfListTobeDownloaded.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (x => x.Value == null));
        List<KeyValuePair<string, string>> list = source != null ? source.ToList<KeyValuePair<string, string>>() : (List<KeyValuePair<string, string>>) null;
        if (list != null && list.Count<KeyValuePair<string, string>>() > 0)
        {
          int num2 = 0;
          while (num2 < 5)
          {
            try
            {
              ++num2;
              for (int index = list.Count - 1; index >= 0; --index)
              {
                Task<string> task = eDeliveryClient.DownloadFilesFromMediaServer(packageDetails, loanGuid, packageId, list[index].Key, directoryInfo.FullName);
                pdfListTobeDownloaded[list[index].Key] = task.Result;
                list.RemoveAt(index);
              }
              if (list.Count<KeyValuePair<string, string>>() == 0)
                num2 = 6;
            }
            catch (Exception ex)
            {
              Tracing.Log(eFolderManager.sw, TraceLevel.Error, nameof (eFolderManager), "Exception in UpdateSupportingData : " + ex.ToString());
              if (num2 == 5)
                return (string) null;
            }
          }
        }
        this.feedback.Increment(25);
        this.VerifyEncompassServerConnection();
        PdfCreator pdfCreator = new PdfCreator();
        List<string> stringList = new List<string>();
        foreach (EllieMae.EMLite.eFolder.eDelivery.Document document in packageDetails.documents)
        {
          if (pdfListTobeDownloaded.ContainsKey(document.id) && pdfListTobeDownloaded[document.id] != null)
            stringList.Add(pdfListTobeDownloaded[document.id]);
        }
        mergedPdfFile = pdfCreator.MergeFiles(stringList.ToArray());
        this.feedback.Increment(25);
        Task.Run<bool>((Func<Task<bool>>) (() => this.UpdateFilesToStorage(loanDataMgr, supportingKey, mergedPdfFile)));
        directoryInfo = new DirectoryInfo(path);
        if (directoryInfo.Exists)
          directoryInfo.Delete(true);
        this.feedback.Increment(5);
        return mergedPdfFile;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 1839, nameof (UpdateSupportingData), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    public void ViewDisclosures(
      LoanDataMgr loanDataMgr,
      string eDisclosurePackageViewableFile,
      string packageId,
      string recordGuid)
    {
      if (!string.IsNullOrEmpty(eDisclosurePackageViewableFile) || !string.IsNullOrWhiteSpace(eDisclosurePackageViewableFile))
        this.ViewDisclosures(loanDataMgr, eDisclosurePackageViewableFile);
      else if (!loanDataMgr.IsPlatformLoan())
      {
        int num1 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "There are no viewable documents associated with this disclosure tracking entry.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        try
        {
          eDisclosurePackageViewableFile = "Disclosures-" + recordGuid + ".pdf";
          BinaryObject supportingData = loanDataMgr.GetSupportingData(eDisclosurePackageViewableFile);
          if (supportingData != null)
          {
            string nameWithExtension = SystemSettings.GetTempFileNameWithExtension("pdf");
            supportingData.Write(nameWithExtension);
            using (PdfPreviewDialog pdfPreviewDialog = new PdfPreviewDialog(nameWithExtension, true, true, false))
            {
              PerformanceMeter.Current.AddCheckpoint("new PdfPreviewDialog", 1892, nameof (ViewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
              PerformanceMeter.Current.AddCheckpoint("BEFORE PdfPreviewDialog.ShowDialog", 1893, nameof (ViewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
              int num2 = (int) pdfPreviewDialog.ShowDialog((IWin32Window) Form.ActiveForm);
              PerformanceMeter.Current.AddCheckpoint("AFTER PdfPreviewDialog.ShowDialog", 1895, nameof (ViewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            }
          }
          else
          {
            this.loanDataManager = loanDataMgr;
            this.loanPackageId = packageId;
            this.loanrecordGuid = recordGuid;
            using (ProgressDialog progressDialog = new ProgressDialog("Regenerating Documents", new AsynchronousProcess(this.updateSupportingData), (object) null, false))
            {
              progressDialog.Status = "Regenerating Documents...";
              int num3 = (int) progressDialog.ShowDialog();
            }
            if (string.IsNullOrEmpty(this.mergedPDFFilePath) || string.IsNullOrWhiteSpace(this.mergedPDFFilePath))
            {
              int num4 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Error while fetching documents. Please retry after some time.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
              using (PdfPreviewDialog pdfPreviewDialog = new PdfPreviewDialog(this.mergedPDFFilePath, true, true, false))
              {
                PerformanceMeter.Current.AddCheckpoint("new PdfPreviewDialog", 1921, nameof (ViewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
                PerformanceMeter.Current.AddCheckpoint("BEFORE PdfPreviewDialog.ShowDialog", 1922, nameof (ViewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
                int num5 = (int) pdfPreviewDialog.ShowDialog((IWin32Window) Form.ActiveForm);
                PerformanceMeter.Current.AddCheckpoint("AFTER PdfPreviewDialog.ShowDialog", 1924, nameof (ViewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
              }
            }
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(eFolderManager.sw, TraceLevel.Error, nameof (eFolderManager), "Exception in ViewDisclosures : " + ex.ToString());
          int num6 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Error while fetching documents. Please retry after some time.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
    }

    private DialogResult updateSupportingData(object state, IProgressFeedback feedback)
    {
      this.feedback = feedback;
      this.mergedPDFFilePath = this.UpdateSupportingData(this.loanDataManager, this.loanDataManager.LoanData.GUID, this.loanPackageId, this.loanrecordGuid);
      return DialogResult.OK;
    }

    private Task<bool> UpdateFilesToStorage(
      LoanDataMgr loanDataMgr,
      string newSupportingDataKey,
      string mergedPdfFile)
    {
      bool result = false;
      int num = 0;
      while (num < 5)
      {
        using (BinaryObject data = new BinaryObject(mergedPdfFile))
        {
          string key = "Disclosures-" + newSupportingDataKey + ".pdf";
          ++num;
          try
          {
            using (PerformanceMeter.Current.BeginOperation("ViewDisclosures loanDataMgr.SaveSupportingData"))
              loanDataMgr.SaveSupportingData(key, data);
            if (loanDataMgr.GetSupportingData(key) != null)
            {
              result = true;
              num = 6;
            }
          }
          catch (Exception ex)
          {
            result = false;
            Tracing.Log(eFolderManager.sw, TraceLevel.Error, nameof (eFolderManager), "Exception in UpdateFilesToStorage : " + ex.Message);
          }
        }
      }
      return Task.FromResult<bool>(result);
    }

    private void VerifyEncompassServerConnection()
    {
      if (!(Session.ServerRealTime != DateTime.MinValue))
      {
        Tracing.Log(eFolderManager.sw, TraceLevel.Error, nameof (eFolderManager), "Lost connection to encompass server.");
        throw new EncompassServerConnectionException("Your connection to the Encompass Server was lost and the electronic document package was not sent. Please try again.");
      }
    }

    public void ViewDisclosures(LoanDataMgr loanDataMgr, string eDisclosurePackageViewableFile)
    {
      bool flag1 = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrViewDcsl", "DOCS eFolderManager.ViewDisclosures local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 2000, nameof (ViewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (string.IsNullOrEmpty(eDisclosurePackageViewableFile))
        {
          PerformanceMeter.Current.AddCheckpoint("BEFORE Utils.Dialog no viewable documents", 2006, nameof (ViewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "There are no viewable documents associated with this disclosure tracking entry.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          PerformanceMeter.Current.AddCheckpoint("EXIT AFTER Utils.Dialog no viewable documents", 2010, nameof (ViewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        }
        else
        {
          Guid result;
          bool flag2 = Guid.TryParseExact(eDisclosurePackageViewableFile, "D", out result);
          if (!flag2 && eDisclosurePackageViewableFile.Contains<char>('.'))
          {
            string[] strArray = eDisclosurePackageViewableFile.Split('.');
            flag2 = strArray.Length > 1 && Guid.TryParseExact(strArray[1], "D", out result);
          }
          BinaryObject binaryObject;
          if (flag2)
          {
            Task<BinaryObject> task = new SkyDriveStreamingClient(loanDataMgr).GetObject(eDisclosurePackageViewableFile);
            Task.WaitAll((Task) task);
            binaryObject = task.Result;
          }
          else
            binaryObject = loanDataMgr.GetSupportingData(eDisclosurePackageViewableFile);
          if (binaryObject == null)
          {
            PerformanceMeter.Current.AddCheckpoint("BEFORE Utils.Dialog no Supporting Data", 2048, nameof (ViewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "There are no viewable documents associated with this disclosure tracking entry.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            PerformanceMeter.Current.AddCheckpoint("EXIT AFTER Utils.Dialog no Supporting Data", 2052, nameof (ViewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          }
          else
          {
            string nameWithExtension = SystemSettings.GetTempFileNameWithExtension("pdf");
            binaryObject.Write(nameWithExtension);
            using (PdfPreviewDialog pdfPreviewDialog = new PdfPreviewDialog(nameWithExtension, true, true, false))
            {
              PerformanceMeter.Current.AddCheckpoint("new PdfPreviewDialog", 2063, nameof (ViewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
              PerformanceMeter.Current.AddCheckpoint("BEFORE PdfPreviewDialog.ShowDialog", 2064, nameof (ViewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
              int num = (int) pdfPreviewDialog.ShowDialog((IWin32Window) Form.ActiveForm);
              PerformanceMeter.Current.AddCheckpoint("AFTER PdfPreviewDialog.ShowDialog", 2066, nameof (ViewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            }
          }
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 2071, nameof (ViewDisclosures), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag1)
          PerformanceMeter.Current.Stop();
      }
    }

    private void updateDocumentTracking(
      LoanDataMgr loanDataMgr,
      string[] pdfList,
      DocumentLog[] docList,
      bool openingDocument,
      bool closingDocument,
      bool preClosingDocument,
      ConditionLog cond)
    {
      if (docList == null)
        return;
      if (pdfList != null)
      {
        List<DocumentLog> documentLogList = new List<DocumentLog>();
        for (int index = 0; index < pdfList.Length; index++)
        {
          using (PdfEditor pdfEditor = new PdfEditor(pdfList[index]))
          {
            if (pdfEditor.SignatureType == "Informational")
            {
              if (!loanDataMgr.SystemConfiguration.DocumentTrackingSetup.DoNotCreateInfoDocs)
              {
                documentLogList.Add(docList[index]);
                if (loanDataMgr.SystemConfiguration.DocumentTrackingSetup.SaveCopyInfoDocs)
                {
                  if (loanDataMgr.SystemConfiguration.DocumentTrackingSetup.ApplyTimeStampToInfoDocs)
                    pdfList[index] = pdfEditor.ApplyTimeStamp(DateTime.Now);
                  FileAttachment returnAttachment = (FileAttachment) null;
                  if (loanDataMgr.UseSkyDriveClassic)
                    Task.WaitAll((Task) Task.Run<FileAttachment>((Func<FileAttachment>) (() => returnAttachment = loanDataMgr.FileAttachments.AddAttachment(AddReasonType.Forms, pdfList[index], docList[index].Title, docList[index]))));
                  else
                    returnAttachment = loanDataMgr.FileAttachments.AddAttachment(AddReasonType.Forms, pdfList[index], docList[index].Title, docList[index]);
                }
              }
            }
            else
              documentLogList.Add(docList[index]);
          }
        }
        docList = documentLogList.ToArray();
      }
      LogList logList = loanDataMgr.LoanData.GetLogList();
      List<DocumentLog> documentLogList1 = new List<DocumentLog>();
      foreach (DocumentLog doc in docList)
      {
        if (!documentLogList1.Contains(doc))
        {
          if (!doc.IsAttachedToLog)
            logList.AddRecord((LogRecordBase) doc);
          if (openingDocument)
            doc.OpeningDocument = true;
          if (closingDocument)
            doc.ClosingDocument = true;
          if (preClosingDocument)
            doc.PreClosingDocument = true;
          if (!doc.Requested)
            doc.MarkAsRequested(DateTime.Now, Session.UserID);
          else
            doc.MarkAsRerequested(DateTime.Now, Session.UserID);
          if (cond != null)
            doc.Conditions.Add(cond);
          documentLogList1.Add(doc);
        }
      }
    }

    private void removeDisclosurePDFforDisclosureTrackingRecord(
      LoanDataMgr loanDataMgr,
      Guid disclosureGuid)
    {
      try
      {
        if (disclosureGuid == Guid.Empty)
          return;
        string key = "Disclosures-" + disclosureGuid.ToString() + ".pdf";
        loanDataMgr.DeleteSupportingData(key);
      }
      catch
      {
      }
    }

    private void removeDisclosureTempLog(LoanDataMgr loanDataMgr, string guid)
    {
      try
      {
        if (guid == "")
          return;
        string key = "DTtemplog_" + guid + ".txt";
        loanDataMgr.DeleteSupportingData(key);
      }
      catch
      {
      }
    }

    private string getDisclosureTempLog(LoanDataMgr loanDataMgr, string guid)
    {
      try
      {
        if (guid == "")
          return "";
        string key = "DTtemplog_" + guid + ".txt";
        return loanDataMgr.GetSupportingData(key).ToString();
      }
      catch
      {
        return "";
      }
    }

    private Guid uploadeDisclosurePDFforDisclosureTrackingRecord(
      LoanDataMgr loanDataMgr,
      string pdfFile,
      string guidString)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 2240, nameof (uploadeDisclosurePDFforDisclosureTrackingRecord), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        Guid guid = Guid.NewGuid();
        string str = "";
        if (pdfFile != "" && File.Exists(pdfFile))
        {
          bool flag = true;
          try
          {
            if (Session.StartupInfo.PolicySettings.Contains((object) "Policies.SaveDisclosureseFolder"))
              flag = (bool) Session.StartupInfo.PolicySettings[(object) "Policies.SaveDisclosureseFolder"];
          }
          catch
          {
          }
          if (flag)
          {
            int num = 0;
            while (num < 5)
            {
              using (BinaryObject data = new BinaryObject(pdfFile))
              {
                string key = "Disclosures-" + guid.ToString() + ".pdf";
                ++num;
                try
                {
                  using (PerformanceMeter.Current.BeginOperation("uploadeDisclosurePDFforDisclosureTrackingRecord loanDataMgr.SaveSupportingData"))
                    loanDataMgr.SaveSupportingData(key, data);
                  if (loanDataMgr.GetSupportingData(key) != null)
                  {
                    str = "";
                    num = 6;
                  }
                }
                catch (Exception ex)
                {
                  str = ex.Message;
                }
              }
            }
            PerformanceMeter.Current.AddCheckpoint("loanDataMgr.SaveSupportingData - tries:" + (object) num, 2284, nameof (uploadeDisclosurePDFforDisclosureTrackingRecord), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          }
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "SaveDisclosuresPrintMenu:" + (flag ? "Yes" : "No"), guidString);
        }
        else
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "PdfFileIsNull:True", guidString);
        if (str != "")
        {
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "SavePDFError:" + str, guidString);
          guid = Guid.Empty;
        }
        return guid;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 2299, nameof (uploadeDisclosurePDFforDisclosureTrackingRecord), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
      }
    }

    public string updateDisclosureTracking(
      LoanDataMgr loanDataMgr,
      DisclosureTrackingBase.DisclosedMethod method,
      string[] packageList,
      DocumentLog[] docList,
      Guid disclosedGuid,
      string pkgID,
      string comments,
      System.Drawing.Printing.PrintAction action = System.Drawing.Printing.PrintAction.PrintToFile)
    {
      return this.updateDisclosureTracking(loanDataMgr, method, packageList, docList, disclosedGuid, pkgID, comments, false, action);
    }

    public string updateDisclosureTracking(
      LoanDataMgr loanDataMgr,
      DisclosureTrackingBase.DisclosedMethod method,
      string[] packageList,
      DocumentLog[] docList,
      Guid disclosedGuid,
      string pkgID,
      string comments,
      bool isWetSign,
      System.Drawing.Printing.PrintAction action = System.Drawing.Printing.PrintAction.PrintToFile)
    {
      bool flag1 = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrUpdtDsclTrac", "DOCS eFolderManager.updateDisclosureTracking local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN, method:" + method.ToString(), 2319, nameof (updateDisclosureTracking), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
        DisclosureTrackingBase disclosureTrackingBase1 = (DisclosureTrackingBase) null;
        DisclosureTrackingBase disclosureTrackingBase2 = (DisclosureTrackingBase) null;
        bool flag2 = false;
        if (Session.LoanData.LinkedData != null && (Session.LoanData.LinkSyncType == LinkSyncType.ConstructionPrimary || Session.LoanData.LinkSyncType == LinkSyncType.ConstructionLinked))
          flag2 = true;
        try
        {
          dictionary1.Add("Location", "eFolder");
          dictionary1.Add("Checkpoint1", "");
          if (!loanDataMgr.LoanData.Use2010RESPA && !loanDataMgr.LoanData.Use2015RESPA)
          {
            dictionary1.Add("Checkpoint1.1", "");
            DisclosureTrackingLogUtils.WriteLog(dictionary1, loanDataMgr, Session.UserID, disclosedGuid, true, "");
            return "";
          }
          dictionary1.Add("Checkpoint2", "");
          DisclosureRecordSetting disclosureRecordSetting = DisclosureRecordSetting.DonotCreate;
          switch (method)
          {
            case DisclosureTrackingBase.DisclosedMethod.ByMail:
              dictionary1.Add("Checkpoint2.1", "");
              if (Session.StartupInfo.PolicySettings.Contains((object) "Policies.DisclosePrint"))
              {
                disclosureRecordSetting = (DisclosureRecordSetting) Session.StartupInfo.PolicySettings[(object) "Policies.DisclosePrint"];
                dictionary1.Add("Checkpoint2.2", "");
                break;
              }
              break;
            case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
              if (Session.StartupInfo.PolicySettings.Contains((object) "Policies.DiscloseeFolder"))
              {
                disclosureRecordSetting = (bool) Session.StartupInfo.PolicySettings[(object) "Policies.DiscloseeFolder"] ? DisclosureRecordSetting.AutoCreate : DisclosureRecordSetting.DonotCreate;
                dictionary1.Add("Checkpoint2.3", "");
                break;
              }
              break;
            case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
              if (Session.LoanData.Use2015RESPA)
              {
                switch (action)
                {
                  case System.Drawing.Printing.PrintAction.PrintToPreview:
                    if (Session.StartupInfo.PolicySettings.Contains((object) "Policies.DisclosePrintPreview"))
                    {
                      disclosureRecordSetting = (DisclosureRecordSetting) Session.StartupInfo.PolicySettings[(object) "Policies.DisclosePrintPreview"];
                      break;
                    }
                    break;
                  case System.Drawing.Printing.PrintAction.PrintToPrinter:
                    if (Session.StartupInfo.PolicySettings.Contains((object) "Policies.DisclosePrint"))
                    {
                      disclosureRecordSetting = (DisclosureRecordSetting) Session.StartupInfo.PolicySettings[(object) "Policies.DisclosePrint"];
                      break;
                    }
                    break;
                  default:
                    disclosureRecordSetting = DisclosureRecordSetting.AutoCreate;
                    break;
                }
              }
              else
                break;
              break;
            default:
              dictionary1.Add("Checkpoint2.4", "");
              break;
          }
          dictionary1.Add("Checkpoint3", "");
          dictionary1.Add("DisclosureRecordSetting", Enum.GetName(typeof (DisclosureRecordSetting), (object) disclosureRecordSetting));
          if (disclosureRecordSetting == DisclosureRecordSetting.DonotCreate)
          {
            dictionary1.Add("Checkpoint3.1", "");
            DisclosureTrackingLogUtils.WriteLog(dictionary1, loanDataMgr, Session.UserID, disclosedGuid, true, "");
            return "";
          }
          dictionary1.Add("Checkpoint4", "");
          List<DisclosureTrackingFormItem> disclosedFormList = new List<DisclosureTrackingFormItem>();
          if (docList != null)
          {
            dictionary1.Add("Checkpoint4.1", "");
            foreach (DocumentLog doc in docList)
            {
              if (doc != null)
              {
                try
                {
                  DocumentTemplate byName = loanDataMgr.SystemConfiguration.DocumentTrackingSetup.GetByName(doc.Title);
                  DisclosureTrackingFormItem.FormType formType = byName != null ? (!(byName.SourceType == "Standard Form") ? (!(byName.SourceType == "Custom Form") ? (!(byName.SourceType == "Borrower Specific Custom Form") ? (!(byName.SourceType == "Needed") ? DisclosureTrackingFormItem.FormType.None : DisclosureTrackingFormItem.FormType.Needed) : DisclosureTrackingFormItem.FormType.CustomForm) : DisclosureTrackingFormItem.FormType.CustomForm) : DisclosureTrackingFormItem.FormType.StandardForm) : DisclosureTrackingFormItem.FormType.eDisclosure;
                  if (method == DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
                    formType = DisclosureTrackingFormItem.FormType.ClosingDocsOrder;
                  string documentTemplateGuid = string.Empty;
                  if (byName != null)
                    documentTemplateGuid = byName.Guid;
                  DisclosureTrackingFormItem trackingFormItem = new DisclosureTrackingFormItem(doc.Title, formType, documentTemplateGuid);
                  disclosedFormList.Add(trackingFormItem);
                }
                catch (Exception ex)
                {
                  string key1 = "ErrorLoadingDocumentTemplate";
                  if (dictionary1.ContainsKey(key1))
                  {
                    Dictionary<string, string> dictionary2 = dictionary1;
                    string key2 = key1;
                    dictionary2[key2] = dictionary2[key2] + "; Error loading document '" + doc.Title + "':" + ex.Message;
                  }
                  else
                    dictionary1.Add(key1, "Error loading document '" + doc.Title + "':" + ex.Message);
                }
              }
            }
          }
          dictionary1.Add("Checkpoint5", "");
          bool flag3 = false;
          using (DisclosureTrackingDialog disclosureTrackingDialog = new DisclosureTrackingDialog(disclosedFormList, method))
          {
            PerformanceMeter.Current.AddCheckpoint("new DisclosureTrackingDialog", 2455, nameof (updateDisclosureTracking), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            if (disclosureRecordSetting == DisclosureRecordSetting.PromptUser)
            {
              dictionary1.Add("Checkpoint5.1", "");
              PerformanceMeter.Current.AddCheckpoint("BEFOR DisclosureTrackingDialog ShowDialog", 2462, nameof (updateDisclosureTracking), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
              DialogResult dialogResult = disclosureTrackingDialog.ShowDialog((IWin32Window) Form.ActiveForm);
              PerformanceMeter.Current.AddCheckpoint("AFTER DisclosureTrackingDialog ShowDialog - " + dialogResult.ToString(), 2464, nameof (updateDisclosureTracking), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
              if (dialogResult != DialogResult.Yes)
              {
                dictionary1.Add("DeclinedToCreateLog", "Yes");
                DisclosureTrackingLogUtils.WriteLog(dictionary1, loanDataMgr, Session.UserID, disclosedGuid, true, "");
                return "";
              }
              flag3 = true;
            }
            dictionary1.Add("Checkpoint5.2", "");
            if (!flag3)
              flag3 = method != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder ? disclosureTrackingDialog.CreateDisclosureLog(dictionary1) : disclosureTrackingDialog.CreateClosingDisclosureLog(dictionary1);
            if (flag3)
            {
              dictionary1.Add("Checkpoint5.3.1", "");
              try
              {
                if (flag2)
                {
                  if (Session.LoanData.Use2015RESPA)
                  {
                    disclosureTrackingBase1 = (DisclosureTrackingBase) disclosureTrackingDialog.Log2015;
                    disclosureTrackingBase2 = (DisclosureTrackingBase) disclosureTrackingDialog.Log2015_ls;
                  }
                  if (disclosedGuid != Guid.Empty)
                  {
                    disclosureTrackingBase1.SetGuid(disclosedGuid);
                    if (disclosureTrackingBase2 is DisclosureTracking2015Log)
                      ((DisclosureTracking2015Log) disclosureTrackingBase2).LinkedGuid = disclosedGuid.ToString();
                    disclosureTrackingBase2.SetGuid(disclosedGuid);
                  }
                }
                else
                {
                  disclosureTrackingBase1 = !Session.LoanData.Use2015RESPA ? (DisclosureTrackingBase) disclosureTrackingDialog.Log : (DisclosureTrackingBase) disclosureTrackingDialog.Log2015;
                  if (disclosedGuid != Guid.Empty)
                    disclosureTrackingBase1.SetGuid(disclosedGuid);
                }
              }
              catch (Exception ex)
              {
                dictionary1.Add("ErrorAtCheckpoint5.3.1", "Error retreving log:" + ex.Message);
              }
              if (Session.LoanData.Use2015RESPA)
              {
                if (flag2)
                {
                  ((DisclosureTracking2015Log) disclosureTrackingBase2).EDSRequestGuid = loanDataMgr.LoanData.GetField("3900");
                  loanDataMgr.LinkedLoan.AddDisclosureTracking2015toLoanLog((DisclosureTracking2015Log) disclosureTrackingBase2);
                }
                ((DisclosureTracking2015Log) disclosureTrackingBase1).EDSRequestGuid = loanDataMgr.LoanData.GetField("3900");
                loanDataMgr.AddDisclosureTracking2015toLoanLog((DisclosureTracking2015Log) disclosureTrackingBase1);
              }
              dictionary1.Add("Checkpoint5.3.2", "");
              int num = 1;
              while (num < 6)
              {
                try
                {
                  loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) disclosureTrackingBase1);
                  if (flag2)
                    loanDataMgr.LinkedLoan.LoanData.GetLogList().AddRecord((LogRecordBase) disclosureTrackingBase2);
                  num = 6;
                }
                catch (Exception ex)
                {
                  dictionary1.Add("ErrorAtCheckpoint5.3.2withTry" + (object) num, "Error adding newlog to log list:" + ex.Message);
                  ++num;
                }
              }
              dictionary1.Add("Checkpoint5.3.3", "");
              try
              {
                if (packageList != null)
                {
                  List<string> stringList = new List<string>((IEnumerable<string>) packageList);
                  disclosureTrackingBase1.eDisclosureApplicationPackage = stringList.Contains("At Application");
                  disclosureTrackingBase1.eDisclosureThreeDayPackage = stringList.Contains("Three-Day");
                  disclosureTrackingBase1.eDisclosureLockPackage = stringList.Contains("At Lock");
                  disclosureTrackingBase1.eDisclosureApprovalPackage = stringList.Contains("Approval");
                  if (flag2)
                  {
                    disclosureTrackingBase2.eDisclosureApplicationPackage = stringList.Contains("At Application");
                    disclosureTrackingBase2.eDisclosureThreeDayPackage = stringList.Contains("Three-Day");
                    disclosureTrackingBase2.eDisclosureLockPackage = stringList.Contains("At Lock");
                    disclosureTrackingBase2.eDisclosureApprovalPackage = stringList.Contains("Approval");
                  }
                }
              }
              catch (Exception ex)
              {
                dictionary1.Add("ErrorAtCheckpoint5.3.3", "Error assigning eDisclosure information:" + ex.Message);
              }
              dictionary1.Add("Checkpoint5.3.4", "");
              try
              {
                if (pkgID != null && pkgID != string.Empty)
                {
                  disclosureTrackingBase1.eDisclosurePackageID = pkgID;
                  if (flag2)
                    disclosureTrackingBase2.eDisclosurePackageID = pkgID;
                }
                else
                  dictionary1.Add("PackageIDIsNull", "True");
                if (comments != null)
                {
                  disclosureTrackingBase1.eDisclosureDisclosedMessage = comments;
                  if (flag2)
                    disclosureTrackingBase2.eDisclosureDisclosedMessage = comments;
                }
                disclosureTrackingBase1.IsWetSigned = isWetSign;
                if (flag2)
                  disclosureTrackingBase2.IsWetSigned = isWetSign;
              }
              catch (Exception ex)
              {
                dictionary1.Add("ErrorAtCheckpoint5.3.4", "Error assigning packageID:" + ex.Message);
              }
              dictionary1.Add("Checkpoint5.4", "");
              if (disclosedGuid != Guid.Empty)
              {
                disclosureTrackingBase1.eDisclosurePackageViewableFile = "Disclosures-" + disclosedGuid.ToString() + ".pdf";
                if (flag2)
                  disclosureTrackingBase2.eDisclosurePackageViewableFile = "Disclosures-" + disclosedGuid.ToString() + ".pdf";
              }
              try
              {
                dictionary1.Add("ActualLog", DisclosureTrackingLogUtils.GetLog((IDisclosureTrackingLog) disclosureTrackingBase1));
              }
              catch (Exception ex)
              {
                dictionary1.Add("ErrorActualLog", "Error creating actual log:" + ex.Message);
              }
              dictionary1.Add("Checkpoint5.5", "");
              string details = (string) null;
              switch (method)
              {
                case DisclosureTrackingBase.DisclosedMethod.ByMail:
                  details = "eDisclosure printed";
                  break;
                case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
                  details = "eDisclosure sent";
                  break;
              }
              if (docList != null)
              {
                foreach (DocumentLog doc in docList)
                {
                  loanDataMgr.LoanHistory.TrackChange((LogRecordBase) doc, details, (LogRecordBase) disclosureTrackingBase1);
                  if (flag2)
                    loanDataMgr.LinkedLoan.LoanHistory.TrackChange((LogRecordBase) doc, details, (LogRecordBase) disclosureTrackingBase2);
                }
              }
            }
            else
            {
              dictionary1.Add("Error", "Failed to create disclosure tracking record.(Location:1)");
              int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Failed to create disclosure tracking record.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
        }
        catch (Exception ex)
        {
          dictionary1.Add("Error", "Error during creation of disclosure tracking record:" + ex.Message);
        }
        dictionary1.Add("Checkpoint6", "");
        DisclosureTrackingLogUtils.WriteLog(dictionary1, loanDataMgr, Session.UserID, disclosedGuid, true, disclosureTrackingBase1 == null ? "" : disclosureTrackingBase1.Guid);
        if (disclosureTrackingBase1 == null)
          return "";
        try
        {
          Session.Application.GetService<ILoanEditor>()?.ShoweDisclosureTrackingRecord(disclosureTrackingBase1, false);
        }
        catch
        {
        }
        return disclosureTrackingBase1.Guid;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END, method:" + method.ToString(), 2706, nameof (updateDisclosureTracking), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag1)
          PerformanceMeter.Current.Stop();
      }
    }

    private DocumentLog[] getDisclosureDocuments(LoanDataMgr loanDataMgr, string[] titleList)
    {
      List<DocumentLog> documentLogList1 = new List<DocumentLog>();
      List<DocumentLog> documentLogList2 = new List<DocumentLog>();
      LogList logList = loanDataMgr.LoanData.GetLogList();
      string pairId = loanDataMgr.LoanData.PairId;
      DocumentLog[] allDocuments = logList.GetAllDocuments();
      foreach (string title in titleList)
      {
        string[] separator = new string[2]{ "\n", "\r\n" };
        string[] source = title.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        string name = source[0];
        string empty = string.Empty;
        if (((IEnumerable<string>) source).Count<string>() > 1)
          empty = source[1];
        DocumentLog documentLog1 = (DocumentLog) null;
        bool flag = false;
        foreach (DocumentLog logEntry in allDocuments)
        {
          if (!(logEntry.Title != name) && !(logEntry.PairId != pairId))
          {
            foreach (DocumentLog documentLog2 in documentLogList2)
            {
              if (logEntry.Guid == documentLog2.Guid)
              {
                flag = true;
                break;
              }
            }
            if (!flag && new eFolderAccessRights(loanDataMgr, (LogRecordBase) logEntry).CanBrowseAttach)
            {
              documentLog1 = logEntry;
              documentLogList2.Add(logEntry);
              break;
            }
            if (flag)
              flag = false;
          }
        }
        if (documentLog1 == null)
        {
          DocumentTemplate byName = loanDataMgr.SystemConfiguration.DocumentTrackingSetup.GetByName(name);
          documentLog1 = byName == null ? new DocumentLog(Session.UserID, pairId) : byName.CreateLogEntry(Session.UserID, pairId);
          documentLog1.Title = name;
          documentLog1.Stage = logList.NextStage;
        }
        if (!string.IsNullOrEmpty(empty))
          documentLog1.GroupName = empty;
        documentLogList1.Add(documentLog1);
      }
      return documentLogList1.ToArray();
    }

    public bool PreviewPreClosing(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      string[] titleList)
    {
      DocumentLog[] disclosureDocuments = this.getDisclosureDocuments(loanDataMgr, titleList);
      return this.PreviewPreClosing(loanDataMgr, coversheetFile, pdfList, disclosureDocuments);
    }

    public bool PreviewPreClosing(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      DocumentLog[] docList)
    {
      bool flag1 = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrPrvwPreClsng", "DOCS eFolderManager.PreviewPreClosing local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 2817, nameof (PreviewPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        bool forLinkSync = false;
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        if (Session.LoanData.LinkedData != null && (Session.LoanData.LinkSyncType == LinkSyncType.ConstructionPrimary || Session.LoanData.LinkSyncType == LinkSyncType.ConstructionLinked))
          forLinkSync = true;
        bool barcodeDocs = BarcodeSetup.GetBarcodeSetup(Session.ISession).CheckPreClosingDocuments(loanDataMgr.LoanData);
        string str = Guid.NewGuid().ToString();
        string pdf = this.createPdf(coversheetFile, pdfList, docList, barcodeDocs);
        if (pdf == null)
        {
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "Failed to create PDF file.", str);
          PerformanceMeter.Current.AddCheckpoint("EXIT pdfFile == null", 2840, nameof (PreviewPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        Guid disclosedGuid = this.uploadeDisclosurePDFforDisclosureTrackingRecord(loanDataMgr, pdf, str);
        DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, str, disclosedGuid.ToString(), "", nameof (PreviewPreClosing));
        using (PdfPreviewDialog pdfPreviewDialog = new PdfPreviewDialog(pdf, true, true, true))
        {
          PerformanceMeter.Current.AddCheckpoint("new PdfPreviewDialog", 2852, nameof (PreviewPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("BEFORE PdfPreviewDialog.ShowDialog", 2855, nameof (PreviewPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = pdfPreviewDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER PdfPreviewDialog.ShowDialog - " + dialogResult.ToString(), 2857, nameof (PreviewPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "Result from preview:" + Enum.GetName(typeof (DialogResult), (object) dialogResult), str);
          if (dialogResult == DialogResult.Yes)
            return this.SendPreClosing(loanDataMgr, coversheetFile, pdfList, docList);
          if (dialogResult == DialogResult.Ignore)
            dialogResult = DialogResult.OK;
          if (dialogResult == DialogResult.OK)
          {
            this.updateDocumentTracking(loanDataMgr, pdfList, docList, false, false, true, (ConditionLog) null);
            DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, str, disclosedGuid.ToString(), "", "Call updateDisclosureTracking.");
            string logGuid = this.updateDisclosureTracking(loanDataMgr, DisclosureTrackingBase.DisclosedMethod.ByMail, (string[]) null, docList, disclosedGuid, (string) null, (string) null);
            DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, str, disclosedGuid.ToString(), "", "New DisclosureTrackingLog:" + logGuid);
            bool flag2 = loanDataMgr.Save(false, false);
            if (!flag2)
              DTErrorLogger.WriteLog(Session.UserID, DateTime.Now, nameof (PreviewPreClosing), Session.CompanyInfo.ClientID, "");
            this.checkExistanceOfDisclosureTrackingLog(loanDataMgr, logGuid, str, forLinkSync);
            return flag2;
          }
          this.removeDisclosureTempLog(loanDataMgr, str);
          return false;
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 2901, nameof (PreviewPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag1)
          PerformanceMeter.Current.Stop();
      }
    }

    public bool PrintPreClosing(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      string[] titleList)
    {
      DocumentLog[] disclosureDocuments = this.getDisclosureDocuments(loanDataMgr, titleList);
      return this.PrintPreClosing(loanDataMgr, coversheetFile, pdfList, disclosureDocuments);
    }

    public bool PrintPreClosing(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      DocumentLog[] docList)
    {
      bool flag1 = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrPrntPreClsng", "DOCS eFolderManager.PrintPreClosing local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 2924, nameof (PrintPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        bool forLinkSync = false;
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        if (Session.LoanData.LinkedData != null && (Session.LoanData.LinkSyncType == LinkSyncType.ConstructionPrimary || Session.LoanData.LinkSyncType == LinkSyncType.ConstructionLinked))
          forLinkSync = true;
        bool barcodeDocs = BarcodeSetup.GetBarcodeSetup(Session.ISession).CheckPreClosingDocuments(loanDataMgr.LoanData);
        string str = Guid.NewGuid().ToString();
        DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, nameof (PrintPreClosing), str);
        string pdf = this.createPdf(coversheetFile, pdfList, docList, barcodeDocs);
        if (pdf == null)
        {
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "Failed to create PDF file.", str);
          PerformanceMeter.Current.AddCheckpoint("EXIT pdfFile == null", 2947, nameof (PrintPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        Guid guid = this.uploadeDisclosurePDFforDisclosureTrackingRecord(loanDataMgr, pdf, str);
        using (PdfPrintDialog pdfPrintDialog = new PdfPrintDialog(pdf))
        {
          PerformanceMeter.Current.AddCheckpoint("new PdfPrintDialog", 2957, nameof (PrintPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("BEFORE PdfPrintDialog ShowDialog", 2960, nameof (PrintPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = pdfPrintDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER PdfPrintDialog ShowDialog - " + dialogResult.ToString(), 2962, nameof (PrintPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "Result from print:" + Enum.GetName(typeof (DialogResult), (object) dialogResult), str);
          if (dialogResult != DialogResult.OK)
          {
            this.removeDisclosurePDFforDisclosureTrackingRecord(loanDataMgr, guid);
            this.removeDisclosureTempLog(loanDataMgr, str);
            PerformanceMeter.Current.AddCheckpoint("EXIT false", 2970, nameof (PrintPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            return false;
          }
          this.updateDocumentTracking(loanDataMgr, pdfList, docList, false, false, true, (ConditionLog) null);
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, str, guid.ToString(), "", "Call updateDisclosureTracking");
          string logGuid = this.updateDisclosureTracking(loanDataMgr, DisclosureTrackingBase.DisclosedMethod.ByMail, (string[]) null, docList, guid, (string) null, (string) null);
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, str, guid.ToString(), "", "New DisclosureTrackingLog:" + logGuid);
          bool flag2 = loanDataMgr.Save(false, false);
          if (!flag2)
            DTErrorLogger.WriteLog(Session.UserID, DateTime.Now, nameof (PrintPreClosing), Session.CompanyInfo.ClientID, "");
          this.checkExistanceOfDisclosureTrackingLog(loanDataMgr, logGuid, str, forLinkSync);
          return flag2;
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 2995, nameof (PrintPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag1)
          PerformanceMeter.Current.Stop();
      }
    }

    public bool SendPreClosing(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      string[] titleList)
    {
      DocumentLog[] disclosureDocuments = this.getDisclosureDocuments(loanDataMgr, titleList);
      return this.SendPreClosing(loanDataMgr, coversheetFile, pdfList, disclosureDocuments);
    }

    public bool SendPreClosing(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      DocumentLog[] docList)
    {
      bool forLinkSync = false;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      bool flag1 = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrSendClosing", "DOCS eFolderManager.SendPreClosing local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3025, nameof (SendPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (Session.LoanData.LinkedData != null && (Session.LoanData.LinkSyncType == LinkSyncType.ConstructionPrimary || Session.LoanData.LinkSyncType == LinkSyncType.ConstructionLinked))
          forLinkSync = true;
        if (BarcodeSetup.GetBarcodeSetup(Session.ISession).CheckPreClosingDocuments(loanDataMgr.LoanData))
          this.addBarcodes(loanDataMgr, pdfList, docList);
        string str1 = Guid.NewGuid().ToString();
        string pdf = this.createPdf(coversheetFile, pdfList);
        if (pdf == null || !File.Exists(pdf))
        {
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "Failed to create PDF file.", str1);
          PerformanceMeter.Current.AddCheckpoint("EXIT - pdf not created", 3047, nameof (SendPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        Guid guid = this.uploadeDisclosurePDFforDisclosureTrackingRecord(loanDataMgr, pdf, str1);
        if (!this.isServerUp("Please contact your Encompass System Administrator about this message. Your connection to the Encompass Server was lost and the electronic document package was not sent. Do you want to retry now?\n\nClicking \"Yes\" will proceed with the transaction if connection with the Encompass server is on. Clicking \"No\" will cancel the transaction - no disclosure will be sent and Disclosure Tracking will not be updated."))
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT - server not up", 3059, nameof (SendPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        string str2 = "";
        if (loanDataMgr.IsPlatformLoan(setCCSiteId: true))
        {
          DialogResult dialogResult;
          string comments;
          bool isWetSign;
          using (SendPackageDialog sendPackageDialog = SendPackageFactory.CreateSendPackageDialog(loanDataMgr, coversheetFile, docList, (DocumentLog[]) null, pdfList, HtmlEmailTemplateType.ConsumerConnectPreClosing))
          {
            DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, str1, guid.ToString(), "", "Send PreClosing");
            if (sendPackageDialog.NoOriginatorCancel)
            {
              this.removeDisclosurePDFforDisclosureTrackingRecord(loanDataMgr, guid);
              this.removeDisclosureTempLog(loanDataMgr, str1);
              PerformanceMeter.Current.AddCheckpoint("EXIT - dialog.NoOriginatorCancel", 3078, nameof (SendPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
              return false;
            }
            PerformanceMeter.Current.AddCheckpoint("BEFORE SendPackageDialog ShowDialog", 3083, nameof (SendPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            dialogResult = sendPackageDialog.ShowDialog((IWin32Window) Form.ActiveForm);
            PerformanceMeter.Current.AddCheckpoint("AFTER SendPackageDialog ShowDialog", 3085, nameof (SendPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            str2 = sendPackageDialog.PackageID;
            comments = sendPackageDialog.Comments;
            isWetSign = sendPackageDialog.IsWetSign;
          }
          this.packageId = str2;
          Session.WriteDTLog += new EventHandler(this.Session_WriteDTLog);
          Session.stopAutoReconnect = true;
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.UserID, "Result from send:" + Enum.GetName(typeof (DialogResult), (object) dialogResult), str1);
          if (dialogResult != DialogResult.OK)
          {
            this.removeDisclosurePDFforDisclosureTrackingRecord(loanDataMgr, guid);
            this.removeDisclosureTempLog(loanDataMgr, str1);
            Session.WriteDTLog -= new EventHandler(this.Session_WriteDTLog);
            Session.stopAutoReconnect = false;
            PerformanceMeter.Current.AddCheckpoint("EXIT - result != DialogResult.OK", 3114, nameof (SendPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            return false;
          }
          this.updateDocumentTracking(loanDataMgr, pdfList, docList, false, false, true, (ConditionLog) null);
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, str1, guid.ToString(), str2, "Call updateDisclosureTracking.");
          string logGuid = this.updateDisclosureTracking(loanDataMgr, DisclosureTrackingBase.DisclosedMethod.eDisclosure, (string[]) null, docList, guid, str2, comments, isWetSign);
          DisclosureTrackingLogUtils.WriteLog(loanDataMgr, Session.CompanyInfo.ClientID, Session.UserID, str1, guid.ToString(), str2, "New DisclosureTrackingLog:" + logGuid);
          bool flag2 = loanDataMgr.Save(false, false);
          this.checkExistanceOfDisclosureTrackingLog(loanDataMgr, logGuid, str1, forLinkSync);
          Session.WriteDTLog -= new EventHandler(this.Session_WriteDTLog);
          Session.stopAutoReconnect = false;
          return flag2;
        }
        if (!string.IsNullOrEmpty(loanDataMgr.WCNotAllowedMessage))
        {
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, loanDataMgr.WCNotAllowedMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        return false;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3140, nameof (SendPreClosing), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag1)
          PerformanceMeter.Current.Stop();
      }
    }

    public string Export(LoanDataMgr loanDataMgr, ConditionLog cond)
    {
      ConditionLog[] condList = new ConditionLog[1]{ cond };
      return this.Export(loanDataMgr, condList);
    }

    public string Export(LoanDataMgr loanDataMgr, ConditionLog[] condList)
    {
      using (PdfFileBuilder pdfFileBuilder = new PdfFileBuilder(loanDataMgr))
        return pdfFileBuilder.CreateFile(condList);
    }

    public string Export(LoanDataMgr loanDataMgr, DocumentLog doc)
    {
      DocumentLog[] docList = new DocumentLog[1]{ doc };
      return this.Export(loanDataMgr, docList);
    }

    public string Export(LoanDataMgr loanDataMgr, DocumentLog[] docList)
    {
      using (PdfFileBuilder pdfFileBuilder = new PdfFileBuilder(loanDataMgr))
        return pdfFileBuilder.CreateFile(docList);
    }

    public string Export(LoanDataMgr loanDataMgr, FileAttachment file)
    {
      FileAttachment[] fileList = new FileAttachment[1]
      {
        file
      };
      return this.Export(loanDataMgr, fileList);
    }

    public string Export(LoanDataMgr loanDataMgr, FileAttachment[] fileList)
    {
      using (PdfFileBuilder pdfFileBuilder = new PdfFileBuilder(loanDataMgr))
        return pdfFileBuilder.CreateFile(fileList);
    }

    public bool ExportDocuments(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      DocumentExportTemplate exportTemplate)
    {
      bool flag1 = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrEportDocs", "DOCS eFolderManager.ExportDocuments local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3217, nameof (ExportDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        string path2 = string.Empty;
        string exportFileNamePart1 = this.getDocumentExportFileNamePart(loanDataMgr, exportTemplate.FileNameField1, exportTemplate.FileNameText1);
        if (exportFileNamePart1 != string.Empty)
          path2 = exportFileNamePart1;
        string exportFileNamePart2 = this.getDocumentExportFileNamePart(loanDataMgr, exportTemplate.FileNameField2, exportTemplate.FileNameText2);
        if (exportFileNamePart2 != string.Empty)
          path2 = path2 + "_" + exportFileNamePart2;
        string exportFileNamePart3 = this.getDocumentExportFileNamePart(loanDataMgr, exportTemplate.FileNameField3, exportTemplate.FileNameText3);
        if (exportFileNamePart3 != string.Empty)
          path2 = path2 + "_" + exportFileNamePart3;
        if (path2.Replace("_", "") == string.Empty)
          path2 = "NoData";
        string password = (string) null;
        if (exportTemplate.PasswordProtect)
          password = exportTemplate.Password;
        string sourceFileName = (string) null;
        if (exportTemplate.ExportAsZip)
        {
          string str1 = Path.Combine(exportTemplate.ExportLocation, path2) + ".zip";
          if (!Directory.Exists(exportTemplate.ExportLocation))
            Directory.CreateDirectory(exportTemplate.ExportLocation);
          int num1 = 1;
          while (File.Exists(str1))
          {
            str1 = Path.Combine(exportTemplate.ExportLocation, path2 + "-" + num1.ToString() + ".zip");
            ++num1;
          }
          List<string> stringList = new List<string>();
          using (ZipWriter zipWriter = new ZipWriter(str1, 0, password))
          {
            bool flag2 = false;
            foreach (DocumentLog doc in docList)
            {
              using (PdfFileBuilder pdfFileBuilder = new PdfFileBuilder(loanDataMgr, exportTemplate.AnnotationExportType))
              {
                string file = pdfFileBuilder.CreateFile(doc);
                if (file != null)
                {
                  flag2 = true;
                  string str2 = doc.Title;
                  foreach (char invalidFileNameChar in Path.GetInvalidFileNameChars())
                    str2 = str2.Replace(invalidFileNameChar.ToString(), " ");
                  string str3 = Path.Combine(exportTemplate.ExportLocation, str2 + ".pdf");
                  int num2 = 1;
                  while (File.Exists(str3))
                  {
                    str3 = Path.Combine(exportTemplate.ExportLocation, str2 + "-" + num2.ToString() + ".pdf");
                    ++num2;
                  }
                  File.Move(file, str3);
                  zipWriter.AddFile(str3);
                  stringList.Add(str3);
                }
              }
            }
            if (!flag2)
              return false;
            zipWriter.CreateZip();
            foreach (string path in stringList)
              File.Delete(path);
          }
        }
        else
        {
          string str = Path.Combine(exportTemplate.ExportLocation, path2) + ".pdf";
          using (PdfFileBuilder pdfFileBuilder = new PdfFileBuilder(loanDataMgr, exportTemplate.AnnotationExportType, password))
          {
            sourceFileName = pdfFileBuilder.CreateFile(docList);
            if (sourceFileName == null)
            {
              PerformanceMeter.Current.AddCheckpoint("EXIT pdfFile == null", 3338, nameof (ExportDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
              return false;
            }
          }
          if (!Directory.Exists(exportTemplate.ExportLocation))
            Directory.CreateDirectory(exportTemplate.ExportLocation);
          int num = 1;
          while (File.Exists(str))
          {
            str = Path.Combine(exportTemplate.ExportLocation, path2 + "-" + num.ToString() + ".pdf");
            ++num;
          }
          File.Move(sourceFileName, str);
        }
        EDMLog edmLog = new EDMLog(Session.UserInfo.FullName + " (" + Session.UserID + ")");
        List<string> stringList1 = new List<string>();
        foreach (DocumentLog doc in docList)
        {
          stringList1.Add(doc.Title);
          loanDataMgr.LoanHistory.TrackChange((LogRecordBase) doc, "Doc exported", (LogRecordBase) edmLog);
        }
        loanDataMgr.LoanHistory.SavePendingHistory();
        edmLog.Description = "Documents Exported";
        string str4 = "Export Template: " + exportTemplate.TemplateName + "\r\n";
        string str5;
        if (exportTemplate.ExportAsZip)
        {
          str5 = str4 + "Export File Type:  Zip\r\n";
          if (exportTemplate.PasswordProtect)
            str5 += "Password Protected: Yes\r\n";
        }
        else
          str5 = str4 + "Export File Type:  PDF\r\n";
        string str6 = str5 + "Annotations Exported: " + exportTemplate.AnnotationExportType.ToString() + "\r\n" + "Export Location: " + exportTemplate.ExportLocation + "\r\n";
        edmLog.Comments = str6;
        edmLog.Documents = stringList1.ToArray();
        loanDataMgr.AddOperationLog((LogRecordBase) edmLog);
      }
      catch (Exception ex)
      {
        PerformanceMeter.Current.AddCheckpoint("EXIT THROW", 3406, nameof (ExportDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        throw ex;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3411, nameof (ExportDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag1)
          PerformanceMeter.Current.Stop();
      }
      return true;
    }

    private string getDocumentExportFileNamePart(
      LoanDataMgr loanDataMgr,
      ExportFileNameFieldType fieldType,
      string optionalText)
    {
      string str1 = "NoData";
      switch (fieldType)
      {
        case ExportFileNameFieldType.None:
          return string.Empty;
        case ExportFileNameFieldType.TodaysDate:
          return DateTime.Now.ToString("MM-dd-yyyy");
        case ExportFileNameFieldType.SubjectPropertyAddress:
          string str2 = loanDataMgr.LoanData.GetField("11") + ", " + loanDataMgr.LoanData.GetField("12") + ", " + loanDataMgr.LoanData.GetField("14") + " " + loanDataMgr.LoanData.GetField("15");
          if (str2.Replace(",", "") == string.Empty)
            str2 = string.Empty;
          return !string.IsNullOrEmpty(str2) ? str2 : str1;
        case ExportFileNameFieldType.Other:
          return !string.IsNullOrEmpty(optionalText) ? optionalText : str1;
        default:
          int num = (int) fieldType;
          string str3 = loanDataMgr.LoanData.GetField(num.ToString()).Replace("/", "-");
          if (str3.Replace("-", "") == string.Empty)
            str3 = string.Empty;
          return !string.IsNullOrEmpty(str3) ? str3 : str1;
      }
    }

    public DocumentLog[] Request(LoanDataMgr loanDataMgr, DocumentLog doc)
    {
      return this.Request(loanDataMgr, doc, (ConditionLog) null);
    }

    public DocumentLog[] Request(LoanDataMgr loanDataMgr, DocumentLog doc, ConditionLog cond)
    {
      DocumentLog[] docList = new DocumentLog[1]{ doc };
      return this.Request(loanDataMgr, docList, cond);
    }

    public DocumentLog[] Request(LoanDataMgr loanDataMgr, DocumentLog[] docList)
    {
      return this.Request(loanDataMgr, docList, (ConditionLog) null);
    }

    public DocumentLog[] Request(LoanDataMgr loanDataMgr, DocumentLog[] docList, ConditionLog cond)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrRequest", "DOCS eFolderManager.Request local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3496, nameof (Request), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        int num = 0;
        foreach (DocumentLog doc in docList)
        {
          if (Epass.IsEpassDoc(doc.Title))
            ++num;
        }
        if (num == 0)
          return this.requestBorrower(loanDataMgr, docList, cond);
        return num == docList.Length ? this.requestEpass(loanDataMgr, docList) : this.requestConflict(loanDataMgr, docList, cond);
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3514, nameof (Request), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    private DocumentLog[] requestEpass(LoanDataMgr loanDataMgr, DocumentLog[] docList)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3526, nameof (requestEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (!new eFolderAccessRights(loanDataMgr).CanRequestServices)
        {
          PerformanceMeter.Current.AddCheckpoint("BEFORE Utils.Dialog no rights", 3534, nameof (requestEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You do not have rights to request documents from ICE Mortgage Technology Network Services.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          PerformanceMeter.Current.AddCheckpoint("AFTER Utils.Dialog no rights", 3537, nameof (requestEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return (DocumentLog[]) null;
        }
        DocumentLog documentLog = docList[0];
        if (docList.Length > 1)
        {
          using (RequestEpassDialog requestEpassDialog = new RequestEpassDialog(loanDataMgr, docList))
          {
            PerformanceMeter.Current.AddCheckpoint("new RequestEpassDialog", 3548, nameof (requestEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            PerformanceMeter.Current.AddCheckpoint("BEFORE RequestEpassDialog ShowDialog", 3550, nameof (requestEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            DialogResult dialogResult = requestEpassDialog.ShowDialog((IWin32Window) Form.ActiveForm);
            PerformanceMeter.Current.AddCheckpoint("AFTER RequestEpassDialog ShowDialog - " + dialogResult.ToString(), 3552, nameof (requestEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            if (dialogResult == DialogResult.Cancel)
              return (DocumentLog[]) null;
            documentLog = requestEpassDialog.Document;
          }
        }
        Epass.EpassDoc epassDoc = Epass.GetEpassDoc(documentLog.Title);
        if (epassDoc.Url == string.Empty)
        {
          ILoanServices service = Session.Application.GetService<ILoanServices>();
          switch (epassDoc.ShortName)
          {
            case "4506T":
              service.OrderSSNService();
              PerformanceMeter.Current.AddCheckpoint("OrderSSNService", 3572, nameof (requestEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
              break;
            case "Compliance Report":
              service.OrderComplianceReport();
              PerformanceMeter.Current.AddCheckpoint("OrderComplianceReport", 3577, nameof (requestEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
              break;
            case "Fraud":
              service.OrderFraud();
              PerformanceMeter.Current.AddCheckpoint("OrderFraud", 3582, nameof (requestEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
              break;
          }
        }
        else
        {
          IEPass service = Session.Application.GetService<IEPass>();
          PerformanceMeter.Current.AddCheckpoint("BEFORE epassMgr.ProcessURL - " + epassDoc.Url, 3590, nameof (requestEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          if (!service.ProcessURL(epassDoc.Url))
          {
            PerformanceMeter.Current.AddCheckpoint("EXIT AFTER epassMgr.ProcessURL - " + epassDoc.Url, 3593, nameof (requestEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            return (DocumentLog[]) null;
          }
          PerformanceMeter.Current.AddCheckpoint("AFTER epassMgr.ProcessURL - " + epassDoc.Url, 3596, nameof (requestEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        }
        return new DocumentLog[1]{ documentLog };
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3604, nameof (requestEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
      }
    }

    private DocumentLog[] requestBorrower(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      ConditionLog cond)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3615, nameof (requestBorrower), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (!new eFolderAccessRights(loanDataMgr).CanRequestDocuments)
        {
          PerformanceMeter.Current.AddCheckpoint("BEFORE Utils.Dialog no rights", 3623, nameof (requestBorrower), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You do not have rights to request documents from borrower.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          PerformanceMeter.Current.AddCheckpoint("AFTER Utils.Dialog no rights", 3626, nameof (requestBorrower), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return (DocumentLog[]) null;
        }
        using (RequestBorrowerDialog requestBorrowerDialog = new RequestBorrowerDialog(loanDataMgr, docList, cond))
        {
          PerformanceMeter.Current.AddCheckpoint("new RequestBorrowerDialog", 3633, nameof (requestBorrower), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          BorrowerPair currentBorrowerPair = loanDataMgr.LoanData.CurrentBorrowerPair;
          PerformanceMeter.Current.AddCheckpoint("BEFORE RequestBorrowerDialog ShowDialog", 3639, nameof (requestBorrower), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = requestBorrowerDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("BEFORE RequestBorrowerDialog ShowDialog - " + dialogResult.ToString(), 3641, nameof (requestBorrower), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          if (currentBorrowerPair.Id != loanDataMgr.LoanData.PairId)
            loanDataMgr.LoanData.SetBorrowerPair(currentBorrowerPair);
          return dialogResult == DialogResult.OK ? requestBorrowerDialog.Documents : (DocumentLog[]) null;
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3656, nameof (requestBorrower), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
      }
    }

    private DocumentLog[] requestConflict(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      ConditionLog cond)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3667, nameof (requestConflict), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        using (RequestConflictDialog requestConflictDialog = new RequestConflictDialog(loanDataMgr, docList, cond))
        {
          PerformanceMeter.Current.AddCheckpoint("new RequestConflictDialog", 3671, nameof (requestConflict), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          BorrowerPair currentBorrowerPair = loanDataMgr.LoanData.CurrentBorrowerPair;
          PerformanceMeter.Current.AddCheckpoint("BEFORE RequestConflictDialog ShowDialog", 3677, nameof (requestConflict), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = requestConflictDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER RequestConflictDialog ShowDialog - " + dialogResult.ToString(), 3679, nameof (requestConflict), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          if (currentBorrowerPair.Id != loanDataMgr.LoanData.PairId)
            loanDataMgr.LoanData.SetBorrowerPair(currentBorrowerPair);
          return dialogResult == DialogResult.OK ? requestConflictDialog.Documents : (DocumentLog[]) null;
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3694, nameof (requestConflict), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
      }
    }

    public bool PreviewRequest(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      ConditionLog cond)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrPrvwReqst", "DOCS eFolderManager.PreviewRequest local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3706, nameof (PreviewRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        bool barcodeDocs = BarcodeSetup.GetBarcodeSetup(Session.ISession).CheckRequestedDocuments(loanDataMgr.LoanData);
        string pdf = this.createPdf(coversheetFile, pdfList, signList, barcodeDocs);
        if (pdf == null)
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT pdfFile == null", 3715, nameof (PreviewRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        using (PdfPreviewDialog pdfPreviewDialog = new PdfPreviewDialog(pdf, true, true, true))
        {
          PerformanceMeter.Current.AddCheckpoint("new PdfPreviewDialog", 3722, nameof (PreviewRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("BEFORE PdfPreviewDialog ShowDialog", 3725, nameof (PreviewRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = pdfPreviewDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER PdfPreviewDialog ShowDialog - " + (object) dialogResult, 3727, nameof (PreviewRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          switch (dialogResult)
          {
            case DialogResult.OK:
              this.updateDocumentTracking(loanDataMgr, pdfList, signList, false, false, false, cond);
              this.updateDocumentTracking(loanDataMgr, (string[]) null, neededList, false, false, false, cond);
              foreach (DocumentLog sign in signList)
                loanDataMgr.LoanHistory.TrackChange((LogRecordBase) sign, "Doc request printed");
              foreach (DocumentLog needed in neededList)
                loanDataMgr.LoanHistory.TrackChange((LogRecordBase) needed, "Doc request printed");
              return loanDataMgr.Save(false);
            case DialogResult.Yes:
              return this.SendRequest(loanDataMgr, coversheetFile, pdfList, signList, neededList, cond);
          }
        }
        return false;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3756, nameof (PreviewRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    public bool PrintRequest(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      ConditionLog cond)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrPrntRqst", "DOCS eFolderManager.PrintRequest local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3769, nameof (PrintRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        bool barcodeDocs = BarcodeSetup.GetBarcodeSetup(Session.ISession).CheckRequestedDocuments(loanDataMgr.LoanData);
        string pdf = this.createPdf(coversheetFile, pdfList, signList, barcodeDocs);
        if (pdf == null)
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT pdfFile == null", 3778, nameof (PrintRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        using (PdfPrintDialog pdfPrintDialog = new PdfPrintDialog(pdf))
        {
          PerformanceMeter.Current.AddCheckpoint("new PdfPrintDialog", 3785, nameof (PrintRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("BEFORE PdfPrintDialog ShowDialog", 3787, nameof (PrintRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = pdfPrintDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER PdfPrintDialog ShowDialog - " + dialogResult.ToString(), 3789, nameof (PrintRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          if (dialogResult != DialogResult.OK)
            return false;
          this.updateDocumentTracking(loanDataMgr, pdfList, signList, false, false, false, cond);
          this.updateDocumentTracking(loanDataMgr, (string[]) null, neededList, false, false, false, cond);
          foreach (DocumentLog sign in signList)
            loanDataMgr.LoanHistory.TrackChange((LogRecordBase) sign, "Doc request printed");
          foreach (DocumentLog needed in neededList)
            loanDataMgr.LoanHistory.TrackChange((LogRecordBase) needed, "Doc request printed");
          return loanDataMgr.Save(false);
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3809, nameof (PrintRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    public bool SendRequest(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      ConditionLog cond)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrSendRqst", "DOCS eFolderManager.SendRequest local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3822, nameof (SendRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (BarcodeSetup.GetBarcodeSetup(Session.ISession).CheckRequestedDocuments(loanDataMgr.LoanData))
          this.addBarcodes(loanDataMgr, pdfList, signList);
        if (loanDataMgr.IsPlatformLoan(setCCSiteId: true))
        {
          using (SendPackageDialog sendPackageDialog = SendPackageFactory.CreateSendPackageDialog(eDeliveryMessageType.RequestDocuments, loanDataMgr, coversheetFile, signList, neededList, pdfList))
          {
            PerformanceMeter.Current.AddCheckpoint("Consumer Connect Loan, new eDelivery.SendPackageDialog", 3838, nameof (SendRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            if (sendPackageDialog.NoOriginatorCancel)
            {
              PerformanceMeter.Current.AddCheckpoint("EXIT dialog.NoOriginatorCancel", 3841, nameof (SendRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
              return false;
            }
            PerformanceMeter.Current.AddCheckpoint("BEFORE eDelivery.SendPackageDialog ShowDialog", 3846, nameof (SendRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            DialogResult dialogResult = sendPackageDialog.ShowDialog((IWin32Window) Form.ActiveForm);
            PerformanceMeter.Current.AddCheckpoint("AFTER eDelivery.SendPackageDialog ShowDialog - " + dialogResult.ToString(), 3848, nameof (SendRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            if (dialogResult != DialogResult.OK)
              return false;
          }
          this.updateDocumentTracking(loanDataMgr, pdfList, signList, false, false, false, cond);
          this.updateDocumentTracking(loanDataMgr, (string[]) null, neededList, false, false, false, cond);
          return loanDataMgr.Save(false);
        }
        if (!string.IsNullOrEmpty(loanDataMgr.WCNotAllowedMessage))
        {
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, loanDataMgr.WCNotAllowedMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        return false;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3873, nameof (SendRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    public bool SendConsentRequest(LoanDataMgr loanDataMgr)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrSndCons", "DOCS eFolderManager.SendConsentRequest 1 local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 3886, nameof (SendConsentRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (loanDataMgr.IsPlatformLoan(setCCSiteId: true))
        {
          if (loanDataMgr.SessionObjects.StartupInfo.OtpSupport)
          {
            PerformanceMeter.Current.AddCheckpoint("Creating SSFDialog for eConsent", 3894, nameof (SendConsentRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            string str = loanDataMgr.SessionObjects.StartupInfo.eConsentUrl + "index.html";
            SSFGuest guestInfo = new SSFGuest()
            {
              uri = str,
              scope = "ecm",
              clientId = "ia4yo1dh"
            };
            SSFContext context = SSFContext.Create(loanDataMgr, SSFHostType.Network, guestInfo);
            if (context == null)
              return false;
            context.parameters = new Dictionary<string, object>()
            {
              {
                "hostname",
                (object) "smartclient"
              },
              {
                "instanceId",
                (object) Session.ServerIdentity.InstanceName
              },
              {
                "loanGuid",
                (object) loanDataMgr.LoanData.GUID
              },
              {
                "lockId",
                (object) Session.SessionObjects.SessionID
              },
              {
                "oapiBaseUrl",
                (object) Session.SessionObjects.StartupInfo.OAPIGatewayBaseUri
              },
              {
                "errorMessages",
                (object) new List<string>()
              }
            };
            int int32_1;
            int int32_2;
            if (Form.ActiveForm != null)
            {
              Form form = Form.ActiveForm;
              while (form.Owner != null)
                form = form.Owner;
              int32_1 = Convert.ToInt32((double) form.Width * 0.95);
              int32_2 = Convert.ToInt32((double) form.Height * 0.95);
            }
            else
            {
              Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
              int32_1 = Convert.ToInt32((double) workingArea.Width * 0.95);
              workingArea = Screen.PrimaryScreen.WorkingArea;
              int32_2 = Convert.ToInt32((double) workingArea.Height * 0.95);
            }
            using (SSFDialog ssfDialog = new SSFDialog(context))
            {
              ssfDialog.Text = "eConsent";
              ssfDialog.Width = Convert.ToInt32(int32_1);
              ssfDialog.Height = Convert.ToInt32(int32_2);
              ssfDialog.ShowDialog((IWin32Window) Session.MainForm);
            }
            return true;
          }
          if (loanDataMgr.IsNew())
            loanDataMgr.Save();
          using (SendConsentRequestDialogCC consentRequestDialogCc = new SendConsentRequestDialogCC(loanDataMgr))
          {
            PerformanceMeter.Current.AddCheckpoint("Consumer Connect Loan, new SendConsentRequestDialogCC", 3961, nameof (SendConsentRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            PerformanceMeter.Current.AddCheckpoint("BEFORE SendConsentRequestDialogCC ShowDialog", 3963, nameof (SendConsentRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            DialogResult dialogResult = consentRequestDialogCc.ShowDialog((IWin32Window) Form.ActiveForm);
            PerformanceMeter.Current.AddCheckpoint("AFTER SendConsentRequestDialogCC ShowDialog - " + dialogResult.ToString(), 3965, nameof (SendConsentRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            return dialogResult == DialogResult.OK && loanDataMgr.Save(false);
          }
        }
        else
        {
          if (!string.IsNullOrEmpty(loanDataMgr.WCNotAllowedMessage))
          {
            int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, loanDataMgr.WCNotAllowedMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          return false;
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 3986, nameof (SendConsentRequest), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    private string createPdf(string coversheetFile, string[] pdfList)
    {
      List<string> stringList = new List<string>();
      if (coversheetFile != null)
        stringList.Add(coversheetFile);
      stringList.AddRange((IEnumerable<string>) pdfList);
      return new PdfCreator().MergeFiles(stringList.ToArray());
    }

    private string createPdf(
      string coversheetFile,
      string[] pdfList,
      DocumentLog[] docList,
      bool barcodeDocs)
    {
      if (!barcodeDocs)
        return this.createPdf(coversheetFile, pdfList);
      List<string> stringList = new List<string>();
      if (coversheetFile != null)
        stringList.Add(coversheetFile);
      this.addBarcodes(Session.LoanDataMgr, pdfList, docList);
      List<string> collection = new List<string>();
      collection.AddRange((IEnumerable<string>) pdfList);
      stringList.AddRange((IEnumerable<string>) collection);
      return new PdfCreator().MergeFiles(stringList.ToArray());
    }

    public bool Retrieve(LoanDataMgr loanDataMgr, Sessions.Session session)
    {
      DocumentLog[] docList = new DocumentLog[0];
      return this.Retrieve(loanDataMgr, docList, session);
    }

    public bool Retrieve(LoanDataMgr loanDataMgr, DocumentLog doc, Sessions.Session session)
    {
      DocumentLog[] docList = new DocumentLog[1]{ doc };
      return this.Retrieve(loanDataMgr, docList, session);
    }

    public bool Retrieve(LoanDataMgr loanDataMgr, DocumentLog[] docList, Sessions.Session session)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrRetrieve", "DOCS eFolderManager.Retrieve local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 4072, nameof (Retrieve), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        int num = 0;
        foreach (DocumentLog doc in docList)
        {
          if (Epass.IsEpassDoc(doc.Title) || doc.IsePASS)
            ++num;
        }
        if (num == 0)
          return this.retrieveBorrower(loanDataMgr, session);
        return num == docList.Length ? this.retrieveEpass(loanDataMgr, docList) : this.retrieveConflict(loanDataMgr, docList, session);
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 4089, nameof (Retrieve), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    private bool retrieveBorrower(LoanDataMgr loanDataMgr, Sessions.Session session)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 4101, nameof (retrieveBorrower), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (!new eFolderAccessRights(loanDataMgr).CanRetrieveDocuments)
        {
          PerformanceMeter.Current.AddCheckpoint("BEFORE Utils.Dialog no rights", 4108, nameof (retrieveBorrower), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You do not have rights to retrieve documents from borrower.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          PerformanceMeter.Current.AddCheckpoint("EXIT AFTER Utils.Dialog no rights", 4111, nameof (retrieveBorrower), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        using (RetrieveBorrowerDialog retrieveBorrowerDialog = new RetrieveBorrowerDialog(loanDataMgr))
        {
          PerformanceMeter.Current.AddCheckpoint("new RetrieveBorrowerDialog", 4117, nameof (retrieveBorrower), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("BEFORE RetrieveBorrowerDialog ShowDialog", 4120, nameof (retrieveBorrower), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = retrieveBorrowerDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER RetrieveBorrowerDialog ShowDialog - " + dialogResult.ToString(), 4122, nameof (retrieveBorrower), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          if (dialogResult == DialogResult.Cancel)
            return false;
          AllFilesDialog.ShowInstance(loanDataMgr, retrieveBorrowerDialog.Files, session);
          return true;
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 4133, nameof (retrieveBorrower), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
      }
    }

    private bool retrieveEpass(LoanDataMgr loanDataMgr, DocumentLog[] docList)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 4144, nameof (retrieveEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (!new eFolderAccessRights(loanDataMgr).CanRetrieveServices)
        {
          PerformanceMeter.Current.AddCheckpoint("BEFORE Utils.Dialog no rights", 4152, nameof (retrieveEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You do not have rights to retrieve documents from ICE Mortgage Technology Network Services.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          PerformanceMeter.Current.AddCheckpoint("EXIT AFTER Utils.Dialog no rights", 4155, nameof (retrieveEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        DocumentLog doc = docList[0];
        if (docList.Length > 1)
        {
          using (RetrieveEpassDialog retrieveEpassDialog = new RetrieveEpassDialog(loanDataMgr, docList))
          {
            PerformanceMeter.Current.AddCheckpoint("new RetrieveEpassDialog", 4165, nameof (retrieveEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            PerformanceMeter.Current.AddCheckpoint("BEFORE RetrieveEpassDialog ShowDialog", 4167, nameof (retrieveEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            DialogResult dialogResult = retrieveEpassDialog.ShowDialog((IWin32Window) Form.ActiveForm);
            PerformanceMeter.Current.AddCheckpoint("AFTER RetrieveEpassDialog ShowDialog - " + dialogResult.ToString(), 4169, nameof (retrieveEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            if (dialogResult == DialogResult.Cancel)
              return false;
            doc = retrieveEpassDialog.Document;
          }
        }
        return Session.Application.GetService<IEPass>().Retrieve(doc);
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 4184, nameof (retrieveEpass), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
      }
    }

    private bool retrieveConflict(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      Sessions.Session session)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 4195, nameof (retrieveConflict), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        using (RetrieveConflictDialog retrieveConflictDialog = new RetrieveConflictDialog(loanDataMgr, docList))
        {
          PerformanceMeter.Current.AddCheckpoint("new RetrieveConflictDialog", 4200, nameof (retrieveConflict), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("BEFORE RetrieveConflictDialog ShowDialog", 4202, nameof (retrieveConflict), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = retrieveConflictDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER RetrieveConflictDialog ShowDialog - " + dialogResult.ToString(), 4204, nameof (retrieveConflict), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          if (dialogResult == DialogResult.Cancel)
            return false;
          if (retrieveConflictDialog.Document != null)
            return this.Retrieve(loanDataMgr, retrieveConflictDialog.Document, session);
          AllFilesDialog.ShowInstance(loanDataMgr, retrieveConflictDialog.Files, session);
          return true;
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 4219, nameof (retrieveConflict), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
      }
    }

    public bool SendDocuments(LoanDataMgr loanDataMgr, DocumentLog[] docList)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrSendDocs", "DOCS eFolderManager.SendDocuments local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 4235, nameof (SendDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (!Modules.IsModuleAvailableForUser(EncompassModule.EDM, true))
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT module not available", 4239, nameof (SendDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        if (!new eFolderAccessRights(loanDataMgr).CanSendDocuments)
        {
          PerformanceMeter.Current.AddCheckpoint("BEFORE Utils.Dialog no rights", 4248, nameof (SendDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You do not have rights to send documents.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          PerformanceMeter.Current.AddCheckpoint("EXIT AFTER Utils.Dialog no rights", 4251, nameof (SendDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        if (loanDataMgr.IsPlatformLoan(setCCSiteId: true))
        {
          eDeliveryMessage edeliveryMessage = SendPackageFactory.CreateEDeliveryMessage(loanDataMgr, eDeliveryMessageType.SendDocuments);
          edeliveryMessage.Subject = "Loan Documents";
          using (PdfFileBuilder pdfFileBuilder = new PdfFileBuilder(loanDataMgr))
          {
            for (int index = 0; index < docList.Length; ++index)
            {
              string file = pdfFileBuilder.CreateFile(docList[index]);
              if (file == null)
              {
                PerformanceMeter.Current.AddCheckpoint("EXIT pdfFile == null", 4270, nameof (SendDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
                return false;
              }
              edeliveryMessage.AddDocument(file, docList[index]);
            }
          }
          return this.sendMessage(edeliveryMessage);
        }
        if (!string.IsNullOrEmpty(loanDataMgr.WCNotAllowedMessage))
        {
          int num1 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, loanDataMgr.WCNotAllowedMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        return false;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 4292, nameof (SendDocuments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    public bool SendForms(LoanDataMgr loanDataMgr, FormItemInfo[] formList)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrSendForms1", "DOCS eFolderManager.SendForms 1 local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 4305, nameof (SendForms), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (!loanDataMgr.Writable)
        {
          PerformanceMeter.Current.AddCheckpoint("BEFORE Utils.Dialog read only", 4311, nameof (SendForms), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You cannot send documents because the loan was opened in read-only mode.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          PerformanceMeter.Current.AddCheckpoint("EXIT AFTER Utils.Dialog read only", 4315, nameof (SendForms), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        FormExport formExport = new FormExport(loanDataMgr);
        EllieMae.EMLite.LoanServices.Bam bam = new EllieMae.EMLite.LoanServices.Bam(loanDataMgr);
        formExport.EntityList = bam.GetCompanyDisclosureEntities();
        string filepath = formExport.ExportForms(formList, false, true);
        if (filepath == null)
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT pdfFile == null", 4328, nameof (SendForms), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        if (loanDataMgr.IsPlatformLoan(setCCSiteId: true))
        {
          eDeliveryMessage edeliveryMessage = SendPackageFactory.CreateEDeliveryMessage(loanDataMgr, eDeliveryMessageType.SecureFormTransfer);
          edeliveryMessage.AddForms(filepath, formList);
          return this.sendMessage(edeliveryMessage);
        }
        if (!string.IsNullOrEmpty(loanDataMgr.WCNotAllowedMessage))
        {
          int num1 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, loanDataMgr.WCNotAllowedMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        return false;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 4353, nameof (SendForms), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    public bool SendForms(LoanDataMgr loanDataMgr, string[] formList, string pdfFile)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrSendForms2", "DOCS eFolderManager.SendForms 2 local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 4366, nameof (SendForms), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (!loanDataMgr.Writable)
        {
          PerformanceMeter.Current.AddCheckpoint("BEFORE Utils.Dialog read only", 4372, nameof (SendForms), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You cannot send documents because the loan was opened in read-only mode.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          PerformanceMeter.Current.AddCheckpoint("EXIT AFTER Utils.Dialog read only", 4376, nameof (SendForms), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        if (loanDataMgr.IsPlatformLoan(setCCSiteId: true))
        {
          eDeliveryMessage edeliveryMessage = SendPackageFactory.CreateEDeliveryMessage(loanDataMgr, eDeliveryMessageType.SecureFormTransfer);
          edeliveryMessage.AddForms(pdfFile, formList);
          return this.sendMessage(edeliveryMessage);
        }
        if (!string.IsNullOrEmpty(loanDataMgr.WCNotAllowedMessage))
        {
          int num1 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, loanDataMgr.WCNotAllowedMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        return false;
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 4401, nameof (SendForms), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    private bool sendMessage(LoanCenterMessage msg)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN (LoanCenterMessage)", 4413, nameof (sendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (!msg.LoanDataMgr.LockLoanWithExclusiveA())
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT - failed LockLoanWithExclusiveA()", 4418, nameof (sendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        using (SendMessageDialog sendMessageDialog = new SendMessageDialog(msg))
        {
          PerformanceMeter.Current.AddCheckpoint("new SendMessageDialog", 4425, nameof (sendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("BEFORE SendMessageDialog ShowDialog", 4427, nameof (sendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          DialogResult dialogResult = sendMessageDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          PerformanceMeter.Current.AddCheckpoint("AFTER SendMessageDialog ShowDialog - " + dialogResult.ToString(), 4429, nameof (sendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          if (dialogResult == DialogResult.Cancel)
            return false;
          msg.LoanDataMgr.Save();
          return true;
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END (LoanCenterMessage)", 4440, nameof (sendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
      }
    }

    private bool sendMessage(eDeliveryMessage msg)
    {
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN (eDeliveryMessage)", 4451, nameof (sendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (!msg.LoanDataMgr.LockLoanWithExclusiveA())
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT - failed LockLoanWithExclusiveA()", 4456, nameof (sendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return false;
        }
        using (SendPackageDialog sendPackageDialog = SendPackageFactory.CreateSendPackageDialog(msg))
        {
          PerformanceMeter.Current.AddCheckpoint("new eDelivery.SendPackageDialog", 4463, nameof (sendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("BEFORE SendPackageDialog.ShowDialog()", 4465, nameof (sendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          if (sendPackageDialog.ShowDialog((IWin32Window) Form.ActiveForm) == DialogResult.Cancel)
          {
            PerformanceMeter.Current.AddCheckpoint("AFTER SendPackageDialog.ShowDialog() CANCEL", 4469, nameof (sendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
            return false;
          }
          PerformanceMeter.Current.AddCheckpoint("AFTER SendPackageDialog.ShowDialog()", 4472, nameof (sendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          msg.LoanDataMgr.Save();
          PerformanceMeter.Current.AddCheckpoint("LoanDataMgr.Save", 4476, nameof (sendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          return true;
        }
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END (eDeliveryMessage)", 4482, nameof (sendMessage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
      }
    }

    public void View(LoanDataMgr loanDataMgr, PreliminaryConditionLog cond)
    {
      PreliminaryDetailsDialog.ShowInstance(loanDataMgr, cond);
    }

    public void View(LoanDataMgr loanDataMgr, EnhancedConditionLog cond)
    {
      EnhancedConditionType[] enhancedConditionTypeArray = (EnhancedConditionType[]) null;
      EnhancedConditionType conditionType = (EnhancedConditionType) null;
      try
      {
        enhancedConditionTypeArray = new EnhancedConditionsRestClient(loanDataMgr).GetEnhancedConditionTypes(false, false);
      }
      catch (Exception ex)
      {
        Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Error, ex.ToString());
      }
      foreach (EnhancedConditionType enhancedConditionType in enhancedConditionTypeArray)
      {
        if (enhancedConditionType.title == cond.EnhancedConditionType)
          conditionType = enhancedConditionType;
      }
      EnhancedDetailsDialog.ShowInstance(loanDataMgr, cond, conditionType);
    }

    public void View(LoanDataMgr loanDataMgr, UnderwritingConditionLog cond)
    {
      UnderwritingDetailsDialog.ShowInstance(loanDataMgr, cond);
    }

    public void View(LoanDataMgr loanDataMgr, PostClosingConditionLog cond)
    {
      PostClosingDetailsDialog.ShowInstance(loanDataMgr, cond);
    }

    public void View(LoanDataMgr loanDataMgr, SellConditionLog cond)
    {
    }

    public void View(LoanDataMgr loanDataMgr, DocumentLog doc)
    {
      DocumentDetailsDialog.ShowInstance(loanDataMgr, doc);
    }

    public void View(LoanDataMgr loanDataMgr, FileAttachment file, Sessions.Session session)
    {
      FileAttachment[] fileList = new FileAttachment[1]
      {
        file
      };
      this.View(loanDataMgr, fileList, session);
    }

    public void View(LoanDataMgr loanDataMgr, FileAttachment[] fileList, Sessions.Session session)
    {
      AllFilesDialog.ShowInstance(loanDataMgr, fileList, session);
    }

    public bool PreFilterMessage(ref System.Windows.Forms.Message m)
    {
      if (m.Msg == 522)
      {
        Point pt;
        ref Point local = ref pt;
        IntPtr lparam = m.LParam;
        int x = lparam.ToInt32() & (int) ushort.MaxValue;
        lparam = m.LParam;
        int y = lparam.ToInt32() >> 16;
        local = new Point(x, y);
        IntPtr num = eFolderManager.WindowFromPoint(pt);
        if (num != IntPtr.Zero && num != m.HWnd && Control.FromHandle(num) != null)
        {
          eFolderManager.SendMessage(num, m.Msg, m.WParam, m.LParam);
          return true;
        }
      }
      return false;
    }

    public bool Minimize() => eFolderDialog.Minimize();

    public void ImportConditions(LoanDataMgr loanDataMgr, bool isAutoImported)
    {
      if (!loanDataMgr.Writable)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Encompass cannot perform this operation because the loan is in read-only mode. You must unlock the loan or contact your administrator for further details.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (isAutoImported && loanDataMgr.LoanData.EnableEnhancedConditions)
      {
        this.showEnhancedConditionImported(loanDataMgr);
      }
      else
      {
        using (AddSellDialog addSellDialog = new AddSellDialog(loanDataMgr))
        {
          if (addSellDialog.SetImportOnly())
          {
            if (addSellDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
              ;
          }
          else
          {
            int num2 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "You do not have rights to import conditions.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
    }

    private void showEnhancedConditionImported(LoanDataMgr loanDataMgr)
    {
      if (loanDataMgr.Dirty && !Session.Application.GetService<ILoanConsole>().SaveLoan())
        return;
      ImportConditionFactory conditionFactory = new ImportConditionFactory(ConditionType.Sell, loanDataMgr.LoanData, true);
      using (Form importConditionForm = conditionFactory.GetImportConditionForm())
      {
        int num = (int) importConditionForm.ShowDialog();
        if (!conditionFactory.Success)
          return;
        loanDataMgr.Refresh(false);
        eFolderDialog.ShowInstance(Session.DefaultInstance);
        eFolderDialog.SelectedTabFromImport(ConditionType.Enhanced, (ConditionLog) null);
      }
    }

    public bool IsEnhancedConditionTemplateActive(EnhancedConditionTemplate template)
    {
      return template.Active.Value && (string.IsNullOrEmpty(template.StartDate) || !(DateTime.Parse(template.StartDate) > DateTime.Now)) && (string.IsNullOrEmpty(template.EndDate) || !(DateTime.Parse(template.EndDate) < DateTime.Now.Date));
    }

    public bool IsEnhancedConditionAllowedOnLoan(
      LoanDataMgr loanDataMgr,
      EnhancedConditionTemplate template)
    {
      bool flag = false;
      if (template.AllowDuplicate.HasValue && template.AllowDuplicate.Value)
        flag = true;
      if (!flag)
      {
        foreach (ConditionLog enhancedCondition in loanDataMgr.LoanData.GetLogList().GetAllEnhancedConditions())
        {
          if (enhancedCondition.Title == template.Title)
            return false;
        }
        flag = true;
      }
      return flag;
    }

    public void LaunchInvestorDeliveryConditions(LoanDataMgr loanDataMgr, ThinThickType type)
    {
      bool flag = PerformanceMeter.StartNewIfNoCurrentMeter("eFldrMgrLaunchInvestorDelivery", "DOCS eFolderManager.LaunchInvestorDelivery local meter");
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 4750, nameof (LaunchInvestorDeliveryConditions), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (!loanDataMgr.Save())
        {
          Tracing.Log(eFolderManager.sw, nameof (eFolderManager), TraceLevel.Error, "Save loan failed for: " + loanDataMgr.LoanData.GetField("GUID"));
          PerformanceMeter.Current.AddCheckpoint("EXIT THROW - Save Loan Failed", 4755, nameof (LaunchInvestorDeliveryConditions), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          throw new Exception("Save loan Failed.");
        }
        string url = Session.LoanDataMgr.SessionObjects.StartupInfo.CustomImportConditionsPageURL;
        string str = Session.DefaultInstance.StartupInfo.InvestorConnectAppUrl + "/lender";
        switch (type)
        {
          case ThinThickType.ReviewAndImport:
            url = str + "/import/condition";
            break;
          case ThinThickType.ImportAll:
            url = str + "/import/conditions";
            break;
          case ThinThickType.DeliverConditionResponses:
            url = str + "/delivery/condition";
            break;
          case ThinThickType.ConditionDeliveryStatus:
            url = str + "/delivery/condition/status";
            break;
        }
        using (ThinThickDialog thinThickDialog = new ThinThickDialog(loanDataMgr, url, type))
        {
          PerformanceMeter.Current.AddCheckpoint("new ThinThickDialog", 4781, nameof (LaunchInvestorDeliveryConditions), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("BEFORE ThinThickDialog ShowDialog", 4782, nameof (LaunchInvestorDeliveryConditions), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
          PerformanceMeter.Current.AddCheckpoint("AFTER ThinThickDialog ShowDialog - " + thinThickDialog.ShowDialog((IWin32Window) Form.ActiveForm).ToString(), 4784, nameof (LaunchInvestorDeliveryConditions), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        }
        loanDataMgr.refreshLoanFromServer();
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 4791, nameof (LaunchInvestorDeliveryConditions), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eFolderManager.cs");
        if (flag)
          PerformanceMeter.Current.Stop();
      }
    }

    public bool Print(
      LoanDataMgr loanDataMgr,
      string url,
      string authorizationHeader,
      int currentPage)
    {
      SkyDriveUrl url1 = new SkyDriveUrl((string) null, url, authorizationHeader);
      Task<string> task = new SkyDriveStreamingClient(loanDataMgr).DownloadFile(url1, "Attachment.pdf");
      Task.WaitAll((Task) task);
      using (PdfPrintDialog pdfPrintDialog = new PdfPrintDialog(task.Result, currentPage))
      {
        if (pdfPrintDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return false;
      }
      return true;
    }

    public void ImportDocuments()
    {
      using (Form importDocumentForm = ImportDocumentFactory.GetImportDocumentForm(Session.LoanDataMgr))
      {
        if (importDocumentForm == null)
          return;
        importDocumentForm.ShowDialog((IWin32Window) Session.MainForm);
        if (!ImportDocumentFactory.Success)
          return;
        Session.LoanDataMgr.refreshLoanFromServer();
        eFolderDialog.ShowInstance(Session.DefaultInstance);
        eFolderDialog.RefreshLoanContents();
      }
    }

    public void ShowEfolderDialogWithDocumentTab(LoanDataMgr loanDataMgr, Sessions.Session session)
    {
      if (!new eFolderAccessRights(loanDataMgr).CanAccessDocumentTab)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.Application, "You do not have the necessary rights to access the documents. Contact your System Administrator for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        eFolderDialog.ShowInstance(Session.DefaultInstance);
        eFolderDialog.RefreshLoanContents();
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.AutoAssignFilesDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.eFolder.Utilities;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class AutoAssignFilesDialog : Form
  {
    private const string className = "AutoAssignFilesDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanDataMgr loanDataMgr;
    private DocumentTrackingSetup docSetup;
    private DocumentLog[] docList;
    private DocumentLog[] docListAll;
    private Dictionary<string, string> drsBarcodeList;
    private Sessions.Session session;
    private IContainer components;
    private GroupContainer gcAssigned;
    private GridView gvAssigned;
    private GroupContainer gcUnassigned;
    private GridView gvUnassigned;
    private Button btnClose;

    public AutoAssignFilesDialog(LoanDataMgr loanDataMgr, Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.loanDataMgr = loanDataMgr;
      this.docSetup = loanDataMgr.SystemConfiguration.DocumentTrackingSetup;
      this.docList = loanDataMgr.LoanData.GetLogList().GetAllDocuments();
      this.docListAll = loanDataMgr.LoanData.GetLogList().GetAllDocuments(true, true);
      this.drsBarcodeList = new Dictionary<string, string>();
    }

    public void AssignFiles(FileAttachment[] fileList)
    {
      if (!Modules.IsModuleAvailableForUser(EncompassModule.EDM, true) || !this.loanDataMgr.LockLoanWithExclusiveA())
        return;
      if (AutoAssignUtils.IsNGAutoAssignEnabled)
      {
        List<string> source = new List<string>();
        foreach (FileAttachment file in fileList)
        {
          switch (file)
          {
            case CloudAttachment _:
            case NativeAttachment _:
              source.Add(file.ID);
              break;
          }
        }
        if (!source.Any<string>())
        {
          Tracing.Log(AutoAssignFilesDialog.sw, TraceLevel.Verbose, nameof (AutoAssignFilesDialog), "AssignFiles :: No cloud or SDC Native attachments selected for auto assignment" + (object) DateTime.Now);
          return;
        }
        Tracing.Log(AutoAssignFilesDialog.sw, TraceLevel.Verbose, nameof (AutoAssignFilesDialog), "Loan " + this.loanDataMgr.LoanData.GUID + " Dirty: " + (this.loanDataMgr.LoanData.Dirty ? "Yes" : "No"));
        if (this.loanDataMgr.LoanData.Dirty)
        {
          if (!this.loanDataMgr.Save())
          {
            Tracing.Log(AutoAssignFilesDialog.sw, nameof (AutoAssignFilesDialog), TraceLevel.Error, "LoanDataMgr.Save failed: " + this.loanDataMgr.LoanData.GUID);
            int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Unable to Auto Assign because the loan could not be saved.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          Tracing.Log(AutoAssignFilesDialog.sw, TraceLevel.Verbose, nameof (AutoAssignFilesDialog), "LoanDataMgr.Save succeeded: " + this.loanDataMgr.LoanData.GUID);
        }
        GetClassificationJobResponse getClassificationResponse;
        using (EOSClientDialog eosClientDialog = new EOSClientDialog(this.loanDataMgr))
        {
          Tracing.Log(AutoAssignFilesDialog.sw, TraceLevel.Verbose, nameof (AutoAssignFilesDialog), "AssignFiles :: Calling Classification EOS API started" + (object) DateTime.Now);
          getClassificationResponse = eosClientDialog.IdentifyFiles(source.ToArray());
          if (eosClientDialog.IsClassificationFailed)
            return;
        }
        if (getClassificationResponse != null)
        {
          if (string.Equals(getClassificationResponse.status, "Cancelled", StringComparison.InvariantCultureIgnoreCase))
            return;
          using (EOSClientDialog eosClientDialog = new EOSClientDialog(this.loanDataMgr))
          {
            getClassificationResponse = eosClientDialog.SplitAndAssignAttachment(getClassificationResponse.id);
            Tracing.Log(AutoAssignFilesDialog.sw, TraceLevel.Verbose, nameof (AutoAssignFilesDialog), "AssignFiles :: Calling Classification EOS API ended" + (object) DateTime.Now);
            if (eosClientDialog.IsClassificationFailed)
              return;
          }
          this.assignClassificationResponse(getClassificationResponse);
        }
      }
      else
      {
        using (IdentifyFilesDialog identifyFilesDialog = new IdentifyFilesDialog(this.loanDataMgr))
          identifyFilesDialog.IdentifyFiles(fileList);
        using (PerformanceMeter.StartNew("AssignFilesDialog.AssignFiles", fileList.Length.ToString() + " files", 155, nameof (AssignFiles), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Files\\AutoAssignFilesDialog.cs"))
        {
          foreach (FileAttachment file in fileList)
          {
            if (!file.Identities.Complete)
            {
              this.addUnassignedFile(file, "Classification was not completed on all of the pages.");
            }
            else
            {
              switch (file)
              {
                case NativeAttachment _:
                  this.assignAttachment(file);
                  continue;
                case ImageAttachment _:
                  this.processAttachment((ImageAttachment) file);
                  continue;
                case CloudAttachment _:
                  this.processAttachment((CloudAttachment) file);
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
      int num1 = (int) this.ShowDialog((IWin32Window) Form.ActiveForm);
    }

    private void assignAttachment(FileAttachment attachment)
    {
      DocumentIdentity documentIdentity = attachment.Identities.Get(0);
      if (documentIdentity == null || documentIdentity.DocumentType == "Unknown")
      {
        this.addUnassignedFile(attachment, "File not identified");
      }
      else
      {
        DocumentLog[] documentLogArray;
        switch (documentIdentity.DocumentType)
        {
          case "Document":
          case "PartialDocument":
            documentLogArray = this.identifyDocument(documentIdentity.DocumentSource);
            break;
          case "Lucene":
            documentLogArray = this.identifyLucene(documentIdentity.DocumentSource);
            break;
          case "DRSDocument":
            documentLogArray = this.identifyDRSDocument(documentIdentity.DocumentSource);
            break;
          default:
            documentLogArray = this.identifyOther(documentIdentity.DocumentSource);
            break;
        }
        if (documentLogArray.Length == 0)
        {
          string reason = "No matching document found (" + documentIdentity.DocumentSource + ")";
          if (documentIdentity.DocumentType == "Lucene")
            reason += " Multiple borrower pairs exist and can't auto-create document for assignment.";
          this.addUnassignedFile(attachment, reason);
        }
        else if (documentLogArray.Length > 1)
        {
          this.addUnassignedFile(attachment, "Multiple matching documents found (" + documentIdentity.DocumentSource + ")");
        }
        else
        {
          attachment.Title = documentLogArray[0].Title;
          if (!new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) documentLogArray[0]).CanAttachUnassignedFiles)
          {
            this.addUnassignedFile(attachment, "You do not have rights to assign this file (" + documentIdentity.DocumentSource + ")");
          }
          else
          {
            documentLogArray[0].Files.Add(attachment.ID, Session.UserID, true);
            this.gvAssigned.Items.Add(attachment.Title).SubItems[1].Text = documentLogArray[0].ToString();
          }
        }
      }
    }

    private void processAttachment(ImageAttachment attachment)
    {
      SplitFile[] fileSplits = this.calculateFileSplits((FileAttachment) attachment);
      if (fileSplits.Length <= 1)
      {
        bool flag = false;
        foreach (SplitFile splitFile in fileSplits)
        {
          for (int index = 0; index < splitFile.pages.Length; ++index)
          {
            if (index != splitFile.pages[index])
              flag = true;
          }
        }
        if (!flag)
        {
          this.assignAttachment((FileAttachment) attachment);
          return;
        }
      }
      List<PageImage[]> pageImageArrayList = new List<PageImage[]>();
      for (int index = 0; index < fileSplits.Length; ++index)
      {
        List<PageImage> pageImageList = new List<PageImage>();
        foreach (int page in fileSplits[index].pages)
          pageImageList.Add(attachment.Pages[page]);
        pageImageArrayList.Add(pageImageList.ToArray());
      }
      List<ImageAttachment> imageAttachmentList = new List<ImageAttachment>();
      for (int index = 0; index < pageImageArrayList.Count; ++index)
      {
        string title = attachment.Title + " (" + Convert.ToString(index + 1) + ")";
        ImageAttachment imageAttachment = this.loanDataMgr.FileAttachments.SplitAttachment(pageImageArrayList[index], title, (DocumentLog) null);
        imageAttachmentList.Add(imageAttachment);
      }
      this.loanDataMgr.FileAttachments.Remove(RemoveReasonType.Split, (FileAttachment) attachment);
      foreach (FileAttachment attachment1 in imageAttachmentList)
        this.assignAttachment(attachment1);
    }

    private void processAttachment(CloudAttachment attachment)
    {
      SplitFile[] fileSplits = this.calculateFileSplits((FileAttachment) attachment);
      if (fileSplits.Length <= 1)
      {
        bool flag = false;
        foreach (SplitFile splitFile in fileSplits)
        {
          for (int index = 0; index < splitFile.pages.Length; ++index)
          {
            if (index != splitFile.pages[index])
              flag = true;
          }
        }
        if (!flag)
        {
          this.assignAttachment((FileAttachment) attachment);
          return;
        }
      }
      CloudAttachment[] cloudAttachmentArray = (CloudAttachment[]) null;
      using (EOSClientDialog eosClientDialog = new EOSClientDialog(this.loanDataMgr))
        cloudAttachmentArray = eosClientDialog.SplitAttachment(attachment, fileSplits);
      if (cloudAttachmentArray == null)
      {
        this.addUnassignedFile((FileAttachment) attachment, "Unable to split the attachment.");
      }
      else
      {
        foreach (FileAttachment attachment1 in cloudAttachmentArray)
          this.assignAttachment(attachment1);
      }
    }

    private SplitFile[] calculateFileSplits(FileAttachment attachment)
    {
      List<SplitFile> splitFileList = new List<SplitFile>();
      List<int> intList1 = (List<int>) null;
      string str1 = (string) null;
      int num = !(attachment is ImageAttachment) ? attachment.Identities.Count : ((ImageAttachment) attachment).Pages.Count;
      for (int pageIndex = 0; pageIndex < num; ++pageIndex)
      {
        DocumentIdentity documentIdentity = attachment.Identities.Get(pageIndex) ?? new DocumentIdentity(pageIndex, "Unknown", "Unknown");
        string str2 = documentIdentity.DocumentSource;
        if (documentIdentity.DocumentType == "Document")
        {
          if (documentIdentity.DocumentSource.Length == 35)
            str2 = documentIdentity.DocumentSource.Substring(0, 32);
        }
        else if (documentIdentity.DocumentType == "PartialDocument" && documentIdentity.DocumentSource.Length == 7)
          str2 = documentIdentity.DocumentSource.Substring(0, 5);
        if (str2 != str1)
        {
          if (intList1 != null)
            splitFileList.Add(new SplitFile()
            {
              pages = intList1.ToArray()
            });
          str1 = str2;
          intList1 = new List<int>();
        }
        intList1.Add(documentIdentity.PageIndex);
      }
      if (intList1 != null)
        splitFileList.Add(new SplitFile()
        {
          pages = intList1.ToArray()
        });
      foreach (SplitFile splitFile in splitFileList)
      {
        SortedDictionary<int, List<int>> sortedDictionary = new SortedDictionary<int, List<int>>();
        foreach (int page in splitFile.pages)
        {
          int result = 1000 + page;
          DocumentIdentity documentIdentity = attachment.Identities.Get(page);
          if (documentIdentity != null)
          {
            if (documentIdentity.DocumentType == "Document" && documentIdentity.DocumentSource.Length == 35)
              int.TryParse(documentIdentity.DocumentSource.Substring(32, 3), out result);
            else if (documentIdentity.DocumentType == "PartialDocument" && documentIdentity.DocumentSource.Length == 7)
              int.TryParse(documentIdentity.DocumentSource.Substring(5, 2), out result);
          }
          if (!sortedDictionary.ContainsKey(result))
            sortedDictionary.Add(result, new List<int>());
          sortedDictionary[result].Add(page);
        }
        List<int> intList2 = new List<int>();
        foreach (int key in sortedDictionary.Keys)
          intList2.AddRange((IEnumerable<int>) sortedDictionary[key]);
        splitFile.pages = intList2.ToArray();
      }
      return splitFileList.ToArray();
    }

    private void assignClassificationResponse(
      GetClassificationJobResponse getClassificationResponse)
    {
      if (getClassificationResponse?.assigningResults?.assigned != null)
      {
        foreach (Assigned assigned in getClassificationResponse?.assigningResults?.assigned)
        {
          foreach (ResultFile file in assigned?.files)
            this.gvAssigned.Items.Add(file?.entityName).SubItems[1].Text = assigned?.documentFolder?.entityName;
        }
        Tracing.Log(AutoAssignFilesDialog.sw, TraceLevel.Verbose, nameof (AutoAssignFilesDialog), "Calling LoanDataMgr.refreshLoanFromServer");
        this.loanDataMgr.refreshLoanFromServer();
      }
      if (getClassificationResponse?.assigningResults?.unrecognizedAndUnassignedPages == null)
        return;
      foreach (UnrecognizedAndUnassignedPage andUnassignedPage in getClassificationResponse?.assigningResults?.unrecognizedAndUnassignedPages)
      {
        foreach (ResultFile file in andUnassignedPage?.files)
          this.addUnassignedFile(file?.entityName, file?.reason);
      }
    }

    private void addUnassignedFile(FileAttachment file, string reason)
    {
      this.gvUnassigned.Items.Add(file.Title).SubItems[1].Text = reason;
    }

    private void addUnassignedFile(string title, string reason)
    {
      this.gvUnassigned.Items.Add(title).SubItems[1].Text = reason;
    }

    private DocumentLog[] identifyDocument(string identifiedSource)
    {
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      DocumentLog barcodedDocument1 = PdfEditor.GetBarcodedDocument(identifiedSource, this.docList);
      if (barcodedDocument1 != null)
      {
        documentLogList.Add(barcodedDocument1);
      }
      else
      {
        DocumentLog barcodedDocument2 = PdfEditor.GetBarcodedDocument(identifiedSource, this.docListAll);
        if (barcodedDocument2 != null)
        {
          DocumentLog[] documentsByTitle = this.loanDataMgr.LoanData.GetLogList().GetDocumentsByTitle(barcodedDocument2.Title);
          if (documentsByTitle.Length == 0)
          {
            DocumentLog rec = new DocumentLog(Session.UserID, barcodedDocument2.PairId);
            rec.Title = barcodedDocument2.Title;
            rec.Description = barcodedDocument2.Description;
            rec.Stage = barcodedDocument2.Stage;
            rec.DaysDue = barcodedDocument2.DaysDue;
            rec.DaysTillExpire = barcodedDocument2.DaysTillExpire;
            this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) rec);
            this.docList = this.loanDataMgr.LoanData.GetLogList().GetAllDocuments();
            this.docListAll = this.loanDataMgr.LoanData.GetLogList().GetAllDocuments(true, true);
            documentLogList.Add(rec);
          }
          else
          {
            foreach (DocumentLog documentLog in documentsByTitle)
            {
              if (documentLog.PairId == barcodedDocument2.PairId)
                documentLogList.Add(documentLog);
            }
          }
        }
      }
      return documentLogList.ToArray();
    }

    private DocumentLog[] identifyLucene(string identifiedSource)
    {
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (DocumentLog doc in this.docList)
      {
        if (doc.Title == identifiedSource)
          documentLogList.Add(doc);
      }
      if (documentLogList.Count == 0)
      {
        BorrowerPair[] borrowerPairs = this.loanDataMgr.LoanData.GetBorrowerPairs();
        if (borrowerPairs.Length != 1)
          return documentLogList.ToArray();
        DocumentLog rec = new DocumentLog(Session.UserID, borrowerPairs[0].Id);
        rec.Title = identifiedSource;
        this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) rec);
        this.docList = this.loanDataMgr.LoanData.GetLogList().GetAllDocuments();
        this.docListAll = this.loanDataMgr.LoanData.GetLogList().GetAllDocuments(true, true);
        documentLogList.Add(rec);
      }
      return documentLogList.ToArray();
    }

    private DocumentLog[] identifyOther(string identifiedSource)
    {
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (DocumentLog doc in this.docList)
      {
        if (doc.Title == identifiedSource)
          documentLogList.Add(doc);
      }
      return documentLogList.ToArray();
    }

    private DocumentLog[] identifyDRSDocument(string identifiedSource)
    {
      if (Session.DefaultInstance.StartupInfo.AllowDRSBarCoding)
      {
        string str = new Guid(identifiedSource).ToString();
        string identifiedSource1 = string.Empty;
        if (this.drsBarcodeList.ContainsKey(str))
        {
          Tracing.Log(AutoAssignFilesDialog.sw, TraceLevel.Verbose, nameof (AutoAssignFilesDialog), "Using Cached Barcode");
          identifiedSource1 = this.drsBarcodeList[str];
        }
        else
        {
          Tracing.Log(AutoAssignFilesDialog.sw, TraceLevel.Verbose, nameof (AutoAssignFilesDialog), "Creating DRSServiceClient");
          DRSServiceClient drsServiceClient = new DRSServiceClient(this.loanDataMgr);
          Tracing.Log(AutoAssignFilesDialog.sw, TraceLevel.Verbose, nameof (AutoAssignFilesDialog), "Calling GetBarcode");
          Task<DocumentMetadataOutput> barcode = drsServiceClient.GetBarcode(str);
          Tracing.Log(AutoAssignFilesDialog.sw, TraceLevel.Verbose, nameof (AutoAssignFilesDialog), "Waiting for GetBarcode Task");
          Task.WaitAll((Task) barcode);
          if (barcode.Result != null && !string.IsNullOrEmpty(barcode.Result.documentId))
            identifiedSource1 = barcode.Result.documentId;
          this.drsBarcodeList[str] = identifiedSource1;
        }
        if (!string.IsNullOrEmpty(identifiedSource1))
          return this.identifyDocument(identifiedSource1);
      }
      return new List<DocumentLog>().ToArray();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.gcAssigned = new GroupContainer();
      this.gvAssigned = new GridView();
      this.gcUnassigned = new GroupContainer();
      this.gvUnassigned = new GridView();
      this.btnClose = new Button();
      this.gcAssigned.SuspendLayout();
      this.gcUnassigned.SuspendLayout();
      this.SuspendLayout();
      this.gcAssigned.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcAssigned.Controls.Add((Control) this.gvAssigned);
      this.gcAssigned.Dock = DockStyle.Top;
      this.gcAssigned.HeaderForeColor = Color.Green;
      this.gcAssigned.Location = new Point(0, 0);
      this.gcAssigned.Name = "gcAssigned";
      this.gcAssigned.Size = new Size(928, 286);
      this.gcAssigned.TabIndex = 0;
      this.gcAssigned.Text = "Files Successfully Assigned";
      this.gvAssigned.AllowMultiselect = false;
      this.gvAssigned.BorderStyle = BorderStyle.None;
      this.gvAssigned.ClearSelectionsOnEmptyRowClick = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colFile";
      gvColumn1.Text = "File Name";
      gvColumn1.Width = 300;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colDocument";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Document Name";
      gvColumn2.Width = 626;
      this.gvAssigned.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvAssigned.Dock = DockStyle.Fill;
      this.gvAssigned.Location = new Point(1, 26);
      this.gvAssigned.Name = "gvAssigned";
      this.gvAssigned.Size = new Size(926, 260);
      this.gvAssigned.TabIndex = 0;
      this.gvAssigned.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gcUnassigned.Controls.Add((Control) this.gvUnassigned);
      this.gcUnassigned.Dock = DockStyle.Top;
      this.gcUnassigned.HeaderForeColor = Color.Red;
      this.gcUnassigned.Location = new Point(0, 286);
      this.gcUnassigned.Name = "gcUnassigned";
      this.gcUnassigned.Size = new Size(928, 287);
      this.gcUnassigned.TabIndex = 1;
      this.gcUnassigned.Text = "Files Not Successfully Assigned";
      this.gvUnassigned.AllowMultiselect = false;
      this.gvUnassigned.BorderStyle = BorderStyle.None;
      this.gvUnassigned.ClearSelectionsOnEmptyRowClick = false;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colFile";
      gvColumn3.Text = "File Name";
      gvColumn3.Width = 300;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colReason";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "Reason";
      gvColumn4.Width = 626;
      this.gvUnassigned.Columns.AddRange(new GVColumn[2]
      {
        gvColumn3,
        gvColumn4
      });
      this.gvUnassigned.Dock = DockStyle.Fill;
      this.gvUnassigned.Location = new Point(1, 26);
      this.gvUnassigned.Name = "gvUnassigned";
      this.gvUnassigned.Size = new Size(926, 260);
      this.gvUnassigned.TabIndex = 0;
      this.gvUnassigned.TextTrimming = StringTrimming.EllipsisCharacter;
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(844, 582);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 2;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(928, 614);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.gcUnassigned);
      this.Controls.Add((Control) this.gcAssigned);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AutoAssignFilesDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Auto Assign Files";
      this.gcAssigned.ResumeLayout(false);
      this.gcUnassigned.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}

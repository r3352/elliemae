// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.ArchiveDocumentsDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.eFolder.WebServices;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using Microsoft.Web.Services2.Attachments;
using Microsoft.Web.Services2.Dime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class ArchiveDocumentsDialog : Form
  {
    private const string className = "ArchiveDocumentsDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanDataMgr loanDataMgr;
    private GridViewDataManager gvDocumentsMgr;
    private IContainer components;
    private Label lblArchive;
    private Button btnArchive;
    private Button btnCancel;
    private GridView gvDocuments;
    private ToolTip tooltip;

    public ArchiveDocumentsDialog(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.initDocumentList();
      this.loadDocumentList();
    }

    private void initDocumentList()
    {
      this.gvDocumentsMgr = new GridViewDataManager(this.gvDocuments, this.loanDataMgr);
      this.gvDocumentsMgr.CreateLayout(new TableLayout.Column[4]
      {
        GridViewDataManager.HasAttachmentsColumn,
        GridViewDataManager.NameColumn,
        GridViewDataManager.BorrowerColumn,
        GridViewDataManager.ArchivedDateColumn
      });
      this.gvDocuments.Sort(1, SortOrder.Ascending);
    }

    private void loadDocumentList()
    {
      foreach (DocumentLog allDocument in this.loanDataMgr.LoanData.GetLogList().GetAllDocuments())
      {
        if (this.loanDataMgr.FileAttachments.ContainsAttachment(allDocument))
          this.gvDocumentsMgr.AddItem(allDocument);
      }
      this.gvDocuments.ReSort();
    }

    private void gvDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnArchive.Enabled = this.gvDocuments.SelectedItems.Count > 0;
    }

    private void btnArchive_Click(object sender, EventArgs e)
    {
      if (!Modules.IsModuleAvailableForUser(EncompassModule.EDM, true) || !this.loanDataMgr.LockLoanWithExclusiveA())
        return;
      int loanID = this.registerLoan();
      if (loanID == 0 || !Transaction.Log(this.loanDataMgr, "Archive"))
        return;
      List<string> stringList = new List<string>();
      foreach (GVItem selectedItem in this.gvDocuments.SelectedItems)
      {
        string pdfFile = this.createPdfFile((DocumentLog) selectedItem.Tag);
        if (string.IsNullOrEmpty(pdfFile))
          return;
        stringList.Add(pdfFile);
      }
      for (int index = 0; index < stringList.Count; ++index)
        this.uploadDocument(loanID, (DocumentLog) this.gvDocuments.SelectedItems[index].Tag, stringList[index]);
      this.loanDataMgr.Save();
      this.DialogResult = DialogResult.OK;
    }

    private int registerLoan()
    {
      int num1 = 0;
      try
      {
        string clientId = Session.CompanyInfo.ClientID;
        string xml = this.loanDataMgr.LoanData.ToXml();
        using (ArchiveService archiveService = new ArchiveService())
        {
          archiveService.Timeout = 600000;
          Tracing.Log(ArchiveDocumentsDialog.sw, TraceLevel.Verbose, nameof (ArchiveDocumentsDialog), "Registering Loan");
          num1 = archiveService.RegisterLoan(clientId, xml);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(ArchiveDocumentsDialog.sw, TraceLevel.Error, nameof (ArchiveDocumentsDialog), ex.Message);
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to register this loan:\n\n" + ex.Message + "\n\nStack:\n\n" + ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      Tracing.Log(ArchiveDocumentsDialog.sw, TraceLevel.Verbose, nameof (ArchiveDocumentsDialog), "Registered Loan: " + num1.ToString());
      return num1;
    }

    private string createPdfFile(DocumentLog doc)
    {
      using (PdfFileBuilder pdfFileBuilder = new PdfFileBuilder(this.loanDataMgr))
        return pdfFileBuilder.CreateFile(doc);
    }

    private void uploadDocument(int loanID, DocumentLog doc, string pdfFile)
    {
      Tracing.Log(ArchiveDocumentsDialog.sw, TraceLevel.Verbose, nameof (ArchiveDocumentsDialog), "Archiving: " + doc.ToString());
      if (pdfFile == null)
        return;
      try
      {
        if (new FileInfo(pdfFile).Length > 31400000L)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The '" + doc.ToString() + "' document that you are attempting to archive is larger than the maximum file size allowed. You will need to reduce the number of attached documents.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          int pageCount;
          using (PdfEditor pdfEditor = new PdfEditor(pdfFile))
            pageCount = pdfEditor.PageCount;
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.LoadXml("<LOG/>");
          doc.ToXml(xmlDocument.DocumentElement);
          using (ArchiveServiceWse archiveServiceWse = new ArchiveServiceWse())
          {
            archiveServiceWse.Timeout = 600000;
            DimeAttachment dimeAttachment = new DimeAttachment(".pdf", TypeFormat.Unchanged, pdfFile);
            archiveServiceWse.RequestSoapContext.Attachments.Add((Attachment) dimeAttachment);
            if (archiveServiceWse.UploadDocument2(loanID, xmlDocument.OuterXml, pageCount) == -1)
              throw new Exception("The loan has not been registered yet.");
          }
          doc.MarkAsArchived(DateTime.Now, Session.UserID);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(ArchiveDocumentsDialog.sw, TraceLevel.Error, nameof (ArchiveDocumentsDialog), ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to archive the '" + doc.ToString() + "' document:\n\n" + ex.Message + "\n\nStack:\n\n" + ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.lblArchive = new Label();
      this.btnArchive = new Button();
      this.btnCancel = new Button();
      this.gvDocuments = new GridView();
      this.tooltip = new ToolTip(this.components);
      this.SuspendLayout();
      this.lblArchive.AutoSize = true;
      this.lblArchive.Location = new Point(12, 12);
      this.lblArchive.Name = "lblArchive";
      this.lblArchive.Size = new Size(232, 14);
      this.lblArchive.TabIndex = 1;
      this.lblArchive.Text = "Select the documents that you want to archive";
      this.btnArchive.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnArchive.Enabled = false;
      this.btnArchive.Location = new Point(390, 316);
      this.btnArchive.Name = "btnArchive";
      this.btnArchive.Size = new Size(75, 22);
      this.btnArchive.TabIndex = 6;
      this.btnArchive.Text = "Archive";
      this.btnArchive.Click += new EventHandler(this.btnArchive_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(466, 316);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.gvDocuments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvDocuments.ClearSelectionsOnEmptyRowClick = false;
      this.gvDocuments.HoverToolTip = this.tooltip;
      this.gvDocuments.Location = new Point(12, 32);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(528, 276);
      this.gvDocuments.TabIndex = 5;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDocuments.SelectedIndexChanged += new EventHandler(this.gvDocuments_SelectedIndexChanged);
      this.AcceptButton = (IButtonControl) this.btnArchive;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(554, 348);
      this.Controls.Add((Control) this.btnArchive);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.gvDocuments);
      this.Controls.Add((Control) this.lblArchive);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ArchiveDocumentsDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Archive";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

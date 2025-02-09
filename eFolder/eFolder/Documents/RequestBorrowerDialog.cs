// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.RequestBorrowerDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.LoanServices.BamObjects;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class RequestBorrowerDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private ConditionLog cond;
    private GridViewDataManager gvFormsMgr;
    private FormExport formExport;
    private List<GVItem> itemList;
    private DocumentLog[] requestedDocList;
    private IContainer components;
    private GroupContainer gcRequest;
    private GridView gvForms;
    private GradientPanel gradForms;
    private Label lblForms;
    private Label lblRequest;
    private Button btnCancel;
    private ToolTip tooltip;
    private EMHelpLink helpLink;
    private ComboBox cboBorrowerPair;
    private FlowLayoutPanel pnlToolbar;
    private Button btnSend;
    private Button btnPrint;
    private Button btnPreview;
    private VerticalSeparator separator;
    private StandardIconButton btnAddDocument;

    public RequestBorrowerDialog(LoanDataMgr loanDataMgr, DocumentLog[] docList, ConditionLog cond)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.cond = cond;
      this.initBorrowerList();
      this.initDocumentList();
      this.formExport = new FormExport(loanDataMgr);
      this.formExport.EntityList = new Bam(this.loanDataMgr).GetCompanyDisclosureEntities();
      foreach (DocumentLog doc in docList)
        this.addDocumentToList(doc);
      this.refreshDocumentList();
    }

    public DocumentLog[] Documents => this.requestedDocList;

    private void initBorrowerList()
    {
      foreach (object borrowerPair in this.loanDataMgr.LoanData.GetBorrowerPairs())
        this.cboBorrowerPair.Items.Add(borrowerPair);
      this.cboBorrowerPair.SelectedItem = (object) this.loanDataMgr.LoanData.CurrentBorrowerPair;
    }

    private void initDocumentList()
    {
      this.itemList = new List<GVItem>();
      this.gvFormsMgr = new GridViewDataManager(this.gvForms, this.loanDataMgr);
      this.gvFormsMgr.CreateLayout(new TableLayout.Column[5]
      {
        GridViewDataManager.CheckBoxColumn,
        GridViewDataManager.NameColumn,
        GridViewDataManager.SignatureTypeColumn,
        GridViewDataManager.DocStatusColumn,
        GridViewDataManager.DateColumn
      });
    }

    private void addDocumentToList(DocumentLog doc)
    {
      bool flag1 = doc is VerifLog;
      DocumentTemplate byName = this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup.GetByName(doc.Title);
      if (byName != null)
        flag1 = byName.IsPrintable;
      if (flag1)
      {
        string str = "Standard Form";
        if (byName != null)
          str = byName.SourceType;
        foreach (BorrowerPair borrowerPair in this.loanDataMgr.LoanData.GetBorrowerPairs())
        {
          if (!(doc.PairId != borrowerPair.Id) || !(doc.PairId != BorrowerPair.All.Id))
          {
            BorrowerPair currentBorrowerPair = this.loanDataMgr.LoanData.CurrentBorrowerPair;
            if (currentBorrowerPair.Id != borrowerPair.Id)
              this.loanDataMgr.LoanData.SetBorrowerPair(borrowerPair);
            string[] strArray = this.formExport.ExportForms(doc, true);
            if (currentBorrowerPair.Id != borrowerPair.Id)
              this.loanDataMgr.LoanData.SetBorrowerPair(currentBorrowerPair);
            if (strArray != null)
            {
              bool flag2 = strArray.Length > 1;
              foreach (string filepath in strArray)
              {
                DocumentLog doc1 = doc;
                string signatureType;
                using (PdfEditor pdfEditor = new PdfEditor(filepath))
                  signatureType = pdfEditor.SignatureType;
                PdfDocument pdfDocument = new PdfDocument()
                {
                  Title = doc1.Title,
                  Type = str,
                  Path = filepath,
                  PairID = borrowerPair.Id,
                  SignatureType = signatureType,
                  Properties = new Hashtable()
                };
                pdfDocument.Properties[(object) "DocumentLog"] = (object) doc1;
                GVItem gvItem = this.gvFormsMgr.CreateItem(doc1);
                gvItem.SubItems[2].Text = signatureType;
                gvItem.Tag = (object) pdfDocument;
                if (!doc.Received || doc.IsExpired)
                  gvItem.Checked = true;
                this.itemList.Add(gvItem);
                if (flag2)
                  doc = byName == null ? new DocumentLog(Session.UserID, borrowerPair.Id) : byName.CreateLogEntry(Session.UserID, borrowerPair.Id);
              }
            }
          }
        }
      }
      else
      {
        GVItem gvItem = this.gvFormsMgr.CreateItem(doc);
        gvItem.SubItems[2].Text = "Needed";
        if (!doc.Received || doc.IsExpired)
          gvItem.Checked = true;
        this.itemList.Add(gvItem);
      }
    }

    private void refreshDocumentList()
    {
      BorrowerPair borrowerPair = (BorrowerPair) this.cboBorrowerPair.SelectedItem ?? BorrowerPair.All;
      this.gvFormsMgr.ClearItems();
      foreach (GVItem gvItem in this.itemList)
      {
        if (gvItem.Tag is DocumentLog)
        {
          DocumentLog tag = (DocumentLog) gvItem.Tag;
          if (tag.PairId == borrowerPair.Id || tag.PairId == BorrowerPair.All.Id)
            this.gvForms.Items.Add(gvItem);
        }
        else if (gvItem.Tag is PdfDocument && ((PdfDocument) gvItem.Tag).PairID == borrowerPair.Id)
          this.gvForms.Items.Add(gvItem);
      }
      if (this.gvForms.Items.Count > 0)
      {
        this.gvForms.Items[0].Checked = !this.gvForms.Items[0].Checked;
        this.gvForms.Items[0].Checked = !this.gvForms.Items[0].Checked;
      }
      this.gvForms.ReSort();
    }

    private void btnAddDocument_Click(object sender, EventArgs e)
    {
      using (AddRequestDialog addRequestDialog = new AddRequestDialog(this.loanDataMgr, ((BorrowerPair) this.cboBorrowerPair.SelectedItem ?? BorrowerPair.All).Id))
      {
        if (addRequestDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        foreach (DocumentLog document in addRequestDialog.Documents)
          this.addDocumentToList(document);
        this.refreshDocumentList();
      }
    }

    private string createFaxCoversheet()
    {
      BorrowerPair selectedItem = (BorrowerPair) this.cboBorrowerPair.SelectedItem;
      if (selectedItem == null)
        return (string) null;
      BorrowerPair currentBorrowerPair = this.loanDataMgr.LoanData.CurrentBorrowerPair;
      if (currentBorrowerPair.Id != selectedItem.Id)
        this.loanDataMgr.LoanData.SetBorrowerPair(selectedItem);
      string faxCoversheet = this.formExport.ExportFaxCoversheet();
      if (currentBorrowerPair.Id != selectedItem.Id)
        this.loanDataMgr.LoanData.SetBorrowerPair(currentBorrowerPair);
      return faxCoversheet;
    }

    private void btnPreview_Click(object sender, EventArgs e)
    {
      if (!Modules.IsModuleAvailableForUser(EncompassModule.EDM, true))
        return;
      List<DocumentLog> documentLogList1 = new List<DocumentLog>();
      List<DocumentLog> documentLogList2 = new List<DocumentLog>();
      List<DocumentLog> documentLogList3 = new List<DocumentLog>();
      List<string> stringList = new List<string>();
      foreach (GVItem checkedItem in this.gvForms.GetCheckedItems(0))
      {
        DocumentLog documentLog = (DocumentLog) null;
        if (checkedItem.Tag is DocumentLog)
        {
          documentLog = (DocumentLog) checkedItem.Tag;
          documentLogList3.Add(documentLog);
        }
        else if (checkedItem.Tag is PdfDocument)
        {
          PdfDocument tag = (PdfDocument) checkedItem.Tag;
          stringList.Add(tag.Path);
          documentLog = (DocumentLog) tag.Properties[(object) "DocumentLog"];
          documentLogList2.Add(documentLog);
        }
        documentLogList1.Add(documentLog);
      }
      if (documentLogList1.Count == 0)
        return;
      string faxCoversheet = this.createFaxCoversheet();
      if (faxCoversheet == null)
        return;
      bool flag = this.formExport.ContainsDisclosureForm(documentLogList2.ToArray());
      if (!this.loanDataMgr.LockLoanWithExclusiveA())
        return;
      IEFolder service = Session.Application.GetService<IEFolder>();
      if (!(!flag ? service.PreviewRequest(this.loanDataMgr, faxCoversheet, stringList.ToArray(), documentLogList2.ToArray(), documentLogList3.ToArray(), this.cond) : service.PreviewDisclosures(this.loanDataMgr, faxCoversheet, stringList.ToArray(), documentLogList2.ToArray(), documentLogList3.ToArray(), this.cond)))
        return;
      this.requestedDocList = documentLogList1.ToArray();
      this.DialogResult = DialogResult.OK;
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      if (!Modules.IsModuleAvailableForUser(EncompassModule.EDM, true))
        return;
      List<DocumentLog> documentLogList1 = new List<DocumentLog>();
      List<DocumentLog> documentLogList2 = new List<DocumentLog>();
      List<DocumentLog> documentLogList3 = new List<DocumentLog>();
      List<string> stringList = new List<string>();
      foreach (GVItem checkedItem in this.gvForms.GetCheckedItems(0))
      {
        DocumentLog documentLog = (DocumentLog) null;
        if (checkedItem.Tag is DocumentLog)
        {
          documentLog = (DocumentLog) checkedItem.Tag;
          documentLogList3.Add(documentLog);
        }
        else if (checkedItem.Tag is PdfDocument)
        {
          PdfDocument tag = (PdfDocument) checkedItem.Tag;
          stringList.Add(tag.Path);
          documentLog = (DocumentLog) tag.Properties[(object) "DocumentLog"];
          documentLogList2.Add(documentLog);
        }
        documentLogList1.Add(documentLog);
      }
      if (documentLogList1.Count == 0)
        return;
      string faxCoversheet = this.createFaxCoversheet();
      if (faxCoversheet == null)
        return;
      bool flag = this.formExport.ContainsDisclosureForm(documentLogList2.ToArray());
      if (!this.loanDataMgr.LockLoanWithExclusiveA())
        return;
      IEFolder service = Session.Application.GetService<IEFolder>();
      if (!(!flag ? service.PrintRequest(this.loanDataMgr, faxCoversheet, stringList.ToArray(), documentLogList2.ToArray(), documentLogList3.ToArray(), this.cond) : service.PrintDisclosures(this.loanDataMgr, faxCoversheet, stringList.ToArray(), documentLogList2.ToArray(), documentLogList3.ToArray(), this.cond)))
        return;
      this.requestedDocList = documentLogList1.ToArray();
      this.DialogResult = DialogResult.OK;
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
      if (!Modules.IsModuleAvailableForUser(EncompassModule.EDM, true))
        return;
      string guidString = Guid.NewGuid().ToString();
      DisclosureTrackingLogUtils.WriteLog(this.loanDataMgr, Session.UserID, "RequestBorrowerDialog.cs", guidString);
      List<DocumentLog> documentLogList1 = new List<DocumentLog>();
      List<DocumentLog> documentLogList2 = new List<DocumentLog>();
      List<DocumentLog> documentLogList3 = new List<DocumentLog>();
      List<string> stringList = new List<string>();
      foreach (GVItem checkedItem in this.gvForms.GetCheckedItems(0))
      {
        DocumentLog documentLog = (DocumentLog) null;
        if (checkedItem.Tag is DocumentLog)
        {
          documentLog = (DocumentLog) checkedItem.Tag;
          documentLogList3.Add(documentLog);
        }
        else if (checkedItem.Tag is PdfDocument)
        {
          PdfDocument tag = (PdfDocument) checkedItem.Tag;
          stringList.Add(tag.Path);
          documentLog = (DocumentLog) tag.Properties[(object) "DocumentLog"];
          documentLogList2.Add(documentLog);
        }
        documentLogList1.Add(documentLog);
      }
      DisclosureTrackingLogUtils.WriteLog(this.loanDataMgr, Session.UserID, "Finish generating documentlog list.", guidString);
      if (documentLogList1.Count == 0)
        return;
      DisclosureTrackingLogUtils.WriteLog(this.loanDataMgr, Session.UserID, "Create Coversheet.", guidString);
      string faxCoversheet = this.createFaxCoversheet();
      if (faxCoversheet == null)
      {
        DisclosureTrackingLogUtils.WriteLog(this.loanDataMgr, Session.UserID, "Failed to create coversheet.", guidString);
      }
      else
      {
        bool flag1 = this.formExport.ContainsDisclosureForm(documentLogList2.ToArray());
        DisclosureTrackingLogUtils.WriteLog(this.loanDataMgr, Session.UserID, "Contains disclosure form:" + flag1.ToString(), guidString);
        DisclosureTrackingLogUtils.WriteLog(this.loanDataMgr, Session.UserID, "Exclusive lock the loan file.", guidString);
        if (!this.loanDataMgr.LockLoanWithExclusiveA())
        {
          DisclosureTrackingLogUtils.WriteLog(this.loanDataMgr, Session.UserID, "Failed to exclusive lock the loan file.", guidString);
        }
        else
        {
          IEFolder service = Session.Application.GetService<IEFolder>();
          BorrowerPair selectedItem = (BorrowerPair) this.cboBorrowerPair.SelectedItem;
          if (selectedItem == null)
            return;
          BorrowerPair currentBorrowerPair = this.loanDataMgr.LoanData.CurrentBorrowerPair;
          if (currentBorrowerPair.Id != selectedItem.Id)
            this.loanDataMgr.LoanData.SetBorrowerPair(selectedItem);
          bool flag2 = !flag1 ? service.SendRequest(this.loanDataMgr, faxCoversheet, stringList.ToArray(), documentLogList2.ToArray(), documentLogList3.ToArray(), this.cond) : service.SendDisclosures(this.loanDataMgr, faxCoversheet, stringList.ToArray(), documentLogList2.ToArray(), documentLogList3.ToArray(), this.cond);
          if (currentBorrowerPair.Id != selectedItem.Id)
            this.loanDataMgr.LoanData.SetBorrowerPair(currentBorrowerPair);
          if (!flag2)
          {
            DisclosureTrackingLogUtils.WriteLog(this.loanDataMgr, Session.UserID, "User cancelled.", guidString);
          }
          else
          {
            this.requestedDocList = documentLogList1.ToArray();
            this.DialogResult = DialogResult.OK;
          }
        }
      }
    }

    private void RequestBorrowerDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
    }

    private void cboBorrowerPair_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.refreshDocumentList();
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
      this.gcRequest = new GroupContainer();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnSend = new Button();
      this.btnPrint = new Button();
      this.btnPreview = new Button();
      this.separator = new VerticalSeparator();
      this.btnAddDocument = new StandardIconButton();
      this.gvForms = new GridView();
      this.gradForms = new GradientPanel();
      this.cboBorrowerPair = new ComboBox();
      this.lblForms = new Label();
      this.tooltip = new ToolTip(this.components);
      this.lblRequest = new Label();
      this.btnCancel = new Button();
      this.helpLink = new EMHelpLink();
      this.gcRequest.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnAddDocument).BeginInit();
      this.gradForms.SuspendLayout();
      this.SuspendLayout();
      this.gcRequest.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcRequest.Controls.Add((Control) this.pnlToolbar);
      this.gcRequest.Controls.Add((Control) this.gvForms);
      this.gcRequest.Controls.Add((Control) this.gradForms);
      this.gcRequest.HeaderForeColor = SystemColors.ControlText;
      this.gcRequest.Location = new Point(8, 44);
      this.gcRequest.Name = "gcRequest";
      this.gcRequest.Size = new Size(638, 396);
      this.gcRequest.TabIndex = 1;
      this.gcRequest.Text = "Request from Borrower";
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnSend);
      this.pnlToolbar.Controls.Add((Control) this.btnPrint);
      this.pnlToolbar.Controls.Add((Control) this.btnPreview);
      this.pnlToolbar.Controls.Add((Control) this.separator);
      this.pnlToolbar.Controls.Add((Control) this.btnAddDocument);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(436, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(198, 22);
      this.pnlToolbar.TabIndex = 2;
      this.btnSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSend.Location = new Point(150, 0);
      this.btnSend.Margin = new Padding(0);
      this.btnSend.Name = "btnSend";
      this.btnSend.Size = new Size(48, 22);
      this.btnSend.TabIndex = 3;
      this.btnSend.TabStop = false;
      this.btnSend.Text = "Send";
      this.btnSend.UseVisualStyleBackColor = true;
      this.btnSend.Click += new EventHandler(this.btnSend_Click);
      this.btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPrint.Location = new Point(106, 0);
      this.btnPrint.Margin = new Padding(0);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(44, 22);
      this.btnPrint.TabIndex = 2;
      this.btnPrint.TabStop = false;
      this.btnPrint.Text = "Print";
      this.btnPrint.UseVisualStyleBackColor = true;
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnPreview.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPreview.Location = new Point(46, 0);
      this.btnPreview.Margin = new Padding(0);
      this.btnPreview.Name = "btnPreview";
      this.btnPreview.Size = new Size(60, 22);
      this.btnPreview.TabIndex = 1;
      this.btnPreview.TabStop = false;
      this.btnPreview.Text = "Preview";
      this.btnPreview.UseVisualStyleBackColor = true;
      this.btnPreview.Click += new EventHandler(this.btnPreview_Click);
      this.separator.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.separator.Location = new Point(41, 3);
      this.separator.Margin = new Padding(4, 3, 3, 3);
      this.separator.MaximumSize = new Size(2, 16);
      this.separator.MinimumSize = new Size(2, 16);
      this.separator.Name = "separator";
      this.separator.Size = new Size(2, 16);
      this.separator.TabIndex = 0;
      this.separator.TabStop = false;
      this.btnAddDocument.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddDocument.BackColor = Color.Transparent;
      this.btnAddDocument.Location = new Point(21, 3);
      this.btnAddDocument.Margin = new Padding(4, 3, 0, 3);
      this.btnAddDocument.MouseDownImage = (Image) null;
      this.btnAddDocument.Name = "btnAddDocument";
      this.btnAddDocument.Size = new Size(16, 16);
      this.btnAddDocument.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddDocument.TabIndex = 12;
      this.btnAddDocument.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddDocument, "Add Document");
      this.btnAddDocument.Click += new EventHandler(this.btnAddDocument_Click);
      this.gvForms.AllowMultiselect = false;
      this.gvForms.BorderStyle = BorderStyle.None;
      this.gvForms.ClearSelectionsOnEmptyRowClick = false;
      this.gvForms.Dock = DockStyle.Fill;
      this.gvForms.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvForms.Location = new Point(1, 51);
      this.gvForms.Name = "gvForms";
      this.gvForms.Size = new Size(636, 344);
      this.gvForms.TabIndex = 1;
      this.gvForms.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gradForms.Borders = AnchorStyles.Bottom;
      this.gradForms.Controls.Add((Control) this.cboBorrowerPair);
      this.gradForms.Controls.Add((Control) this.lblForms);
      this.gradForms.Dock = DockStyle.Top;
      this.gradForms.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradForms.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradForms.Location = new Point(1, 26);
      this.gradForms.Name = "gradForms";
      this.gradForms.Padding = new Padding(8, 0, 0, 0);
      this.gradForms.Size = new Size(636, 25);
      this.gradForms.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradForms.TabIndex = 0;
      this.cboBorrowerPair.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorrowerPair.FormattingEnabled = true;
      this.cboBorrowerPair.Location = new Point(104, 1);
      this.cboBorrowerPair.Name = "cboBorrowerPair";
      this.cboBorrowerPair.Size = new Size(264, 22);
      this.cboBorrowerPair.TabIndex = 1;
      this.cboBorrowerPair.TabStop = false;
      this.cboBorrowerPair.SelectionChangeCommitted += new EventHandler(this.cboBorrowerPair_SelectionChangeCommitted);
      this.lblForms.BackColor = Color.Transparent;
      this.lblForms.Dock = DockStyle.Fill;
      this.lblForms.Location = new Point(8, 0);
      this.lblForms.Name = "lblForms";
      this.lblForms.Size = new Size(628, 25);
      this.lblForms.TabIndex = 0;
      this.lblForms.Text = "For Borrower Pair";
      this.lblForms.TextAlign = ContentAlignment.MiddleLeft;
      this.lblRequest.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblRequest.Location = new Point(8, 8);
      this.lblRequest.Name = "lblRequest";
      this.lblRequest.Size = new Size(638, 32);
      this.lblRequest.TabIndex = 0;
      this.lblRequest.Text = "Confirm the documents you have selected. All requests will contain the EDM fax coversheet for this loan. You can use the fax coversheet anytime to receive documents electronically for this loan.";
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.CausesValidation = false;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(572, 448);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Request Documents and Settlement Services";
      this.helpLink.Location = new Point(8, 450);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 3;
      this.helpLink.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(654, 478);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblRequest);
      this.Controls.Add((Control) this.gcRequest);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RequestBorrowerDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Request";
      this.KeyDown += new KeyEventHandler(this.RequestBorrowerDialog_KeyDown);
      this.gcRequest.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnAddDocument).EndInit();
      this.gradForms.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}

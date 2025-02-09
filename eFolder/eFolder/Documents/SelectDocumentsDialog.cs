// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.SelectDocumentsDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class SelectDocumentsDialog : Form
  {
    private const string className = "SelectDocumentsDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanDataMgr loanDataMgr;
    private string[] loanguids;
    private DocumentLog[] docList;
    private DocumentLog[] defaultList;
    private FileSystemEntry defaultStackingEntry;
    private GridViewDataManager gvDocumentsMgr;
    private StackingOrderSetTemplate stackingTemplate;
    private bool isListValid;
    private DocumentLog[] selectedList;
    private bool allowContinue;
    private SelectDocumentsReasonType selectDocumentsReasonType;
    private FileSystemEntry defaultExportEntry;
    private DocumentExportTemplate exportTemplate;
    private IContainer components;
    private EMHelpLink helpLink;
    private GradientPanel gradStackingOrder;
    private Label lblExportDropdown;
    private Label lblStackingOrder;
    private ComboBox cboExportTemplate;
    private Label lblInstructions;
    private ComboBox cboStackingOrder;
    private Label lblDropdown;
    private Panel pnlClose;
    private Panel pnlStackingKey;
    private Label lblOptionalDesc;
    private Label lblRequiredDesc;
    private Label lblRequiredBox;
    private Label lblOptionalBox;
    private Button btnPreview;
    private Button btnCancel;
    private EMHelpLink emHelpLink1;
    private Button btnContinue;
    private GroupContainer gcDocuments;
    private GridView gvDocuments;
    private FlowLayoutPanel pnlToolbar;
    private Button btnAddDocs;
    private VerticalSeparator separator1;
    private Button btnUpdateTemplate;

    public SelectDocumentsDialog(
      LoanDataMgr firstLoanDataMgr,
      string[] loanguids,
      FileSystemEntry defaultEntry)
    {
      this.InitializeComponent();
      this.selectDocumentsReasonType = SelectDocumentsReasonType.DocumentExport;
      this.initForm(firstLoanDataMgr, (DocumentLog[]) null, (DocumentLog[]) null, defaultEntry);
      this.loanguids = loanguids;
    }

    public SelectDocumentsDialog(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      DocumentLog[] defaultList,
      FileSystemEntry defaultStackingEntry,
      bool allowContinue)
    {
      this.InitializeComponent();
      this.allowContinue = allowContinue;
      this.selectDocumentsReasonType = SelectDocumentsReasonType.SendToThirdParty;
      string settingFromCache = Session.SessionObjects.GetCompanySettingFromCache("StackingOrder", "Default");
      try
      {
        defaultStackingEntry = FileSystemEntry.Parse(settingFromCache);
      }
      catch
      {
      }
      this.initForm(loanDataMgr, docList, defaultList, defaultStackingEntry);
    }

    public SelectDocumentsDialog(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      DocumentLog[] defaultList,
      FileSystemEntry defaultStackingEntry,
      SelectDocumentsReasonType reasonType)
    {
      this.InitializeComponent();
      this.selectDocumentsReasonType = reasonType;
      this.initForm(loanDataMgr, docList, defaultList, defaultStackingEntry);
    }

    private void initForm(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      DocumentLog[] defaultList,
      FileSystemEntry defaultEntry)
    {
      if (this.selectDocumentsReasonType == SelectDocumentsReasonType.DocumentExport)
      {
        this.Text = "Export Documents";
        this.lblInstructions.Text = "Select an Export Template to apply a stacking order and view the list of documents that will be exported.";
        this.lblStackingOrder.Visible = true;
        this.btnPreview.Visible = false;
        this.btnContinue.Text = "Next >";
        this.btnContinue.Enabled = true;
        this.emHelpLink1.HelpTag = this.helpLink.HelpTag = "DocumentExportAuditReport";
        this.allowContinue = true;
      }
      this.lblExportDropdown.Visible = this.selectDocumentsReasonType == SelectDocumentsReasonType.DocumentExport;
      this.cboExportTemplate.Visible = this.selectDocumentsReasonType == SelectDocumentsReasonType.DocumentExport;
      this.lblDropdown.Visible = this.selectDocumentsReasonType != SelectDocumentsReasonType.DocumentExport;
      this.cboStackingOrder.Visible = this.selectDocumentsReasonType != SelectDocumentsReasonType.DocumentExport;
      this.btnUpdateTemplate.Visible = false;
      if (Session.IsBankerEdition() && new eFolderAccessRights(loanDataMgr).CanUpdateDocumentStackingTemplate)
        this.btnUpdateTemplate.Visible = this.selectDocumentsReasonType != SelectDocumentsReasonType.DocumentExport;
      if (Session.IsBrokerEdition() && Session.UserInfo.IsAdministrator())
        this.btnUpdateTemplate.Visible = this.selectDocumentsReasonType != SelectDocumentsReasonType.DocumentExport;
      this.lblOptionalBox.BackColor = ColorTranslator.FromHtml("#FFF4BF");
      this.lblRequiredBox.BackColor = ColorTranslator.FromHtml("#FDD1D2");
      this.loanDataMgr = loanDataMgr;
      this.docList = docList;
      this.defaultList = defaultList;
      if (this.selectDocumentsReasonType == SelectDocumentsReasonType.DocumentExport)
        this.defaultExportEntry = defaultEntry;
      else
        this.defaultStackingEntry = defaultEntry;
      this.gvDocumentsMgr = new GridViewDataManager(this.gvDocuments, this.loanDataMgr);
      this.initDocumentList();
      this.loadDocumentList();
      if (this.selectDocumentsReasonType == SelectDocumentsReasonType.DocumentExport)
        this.initializeExportTemplate();
      this.initializeStackingOrder();
      this.setButtons();
    }

    public DocumentLog[] Documents => this.selectedList;

    public DocumentExportTemplate DocumentExportTemplate => this.exportTemplate;

    private void initDocumentList()
    {
      this.btnUpdateTemplate.Enabled = false;
      this.pnlStackingKey.Visible = false;
      TableLayout.Column[] columnList;
      if (this.selectDocumentsReasonType == SelectDocumentsReasonType.DocumentExport)
      {
        columnList = new TableLayout.Column[2]
        {
          GridViewDataManager.NameColumn,
          GridViewDataManager.RequiredColumn
        };
      }
      else
      {
        if (this.stackingTemplate == null)
          columnList = new TableLayout.Column[7]
          {
            GridViewDataManager.CheckBoxColumn,
            GridViewDataManager.HasAttachmentsColumn,
            GridViewDataManager.NameColumn,
            GridViewDataManager.RequestedFromColumn,
            GridViewDataManager.BorrowerColumn,
            GridViewDataManager.DocStatusColumn,
            GridViewDataManager.DateColumn
          };
        else
          columnList = new TableLayout.Column[8]
          {
            GridViewDataManager.CheckBoxColumn,
            GridViewDataManager.HasAttachmentsColumn,
            GridViewDataManager.NameColumn,
            GridViewDataManager.RequestedFromColumn,
            GridViewDataManager.BorrowerColumn,
            GridViewDataManager.DocStatusColumn,
            GridViewDataManager.DateColumn,
            GridViewDataManager.RequiredColumn
          };
        this.btnUpdateTemplate.Enabled = this.stackingTemplate != null;
        this.pnlStackingKey.Visible = this.stackingTemplate != null;
      }
      this.gvDocumentsMgr.CreateLayout(columnList);
    }

    private void loadDocumentList()
    {
      this.gvDocuments.Items.Clear();
      List<FileAttachment> fileAttachmentList = new List<FileAttachment>();
      foreach (FileAttachment fileAttachment in this.loanDataMgr.FileAttachments)
        fileAttachmentList.Add(fileAttachment);
      SelectDocuments selectDocuments = new SelectDocuments(fileAttachmentList.ToArray(), this.docList, this.defaultList, this.selectDocumentsReasonType, this.stackingTemplate);
      SelectionDocument[] selectionDocuments = selectDocuments.GetSelectionDocuments();
      this.isListValid = selectDocuments.IsListValid;
      foreach (SelectionDocument selectionDocument in selectionDocuments)
      {
        GVItem gvItem = this.gvDocumentsMgr.AddItem(selectionDocument.DocumentLog);
        if (this.selectDocumentsReasonType == SelectDocumentsReasonType.DocumentExport)
        {
          gvItem.SubItems[0].Value = (object) selectionDocument.DocName;
          gvItem.SubItems[1].Value = (object) selectionDocument.RequiredString;
        }
        else
        {
          gvItem.SubItems[2].Value = (object) selectionDocument.DocName;
          gvItem.Checked = selectionDocument.IsChecked;
          if (this.stackingTemplate != null)
          {
            gvItem.SubItems[0].CheckBoxVisible = selectionDocument.CheckBoxVisible;
            gvItem.SubItems[7].Value = (object) selectionDocument.RequiredString;
            if (selectionDocument.OptionalWarning)
              gvItem.BackColor = this.lblOptionalBox.BackColor;
            if (selectionDocument.RequiredError)
              gvItem.BackColor = this.lblRequiredBox.BackColor;
          }
        }
      }
    }

    private DocumentLog[] getSelectedDocuments()
    {
      if (this.stackingTemplate != null)
        this.validateRequiredStackingItems();
      if (!this.isListValid)
        return new List<DocumentLog>().ToArray();
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocuments.Items)
      {
        DocumentLog tag = (DocumentLog) gvItem.Tag;
        if (gvItem.Checked && tag != null)
          documentLogList.Add(tag);
      }
      return documentLogList.ToArray();
    }

    private void validateRequiredStackingItems()
    {
      this.isListValid = true;
      if (this.selectDocumentsReasonType == SelectDocumentsReasonType.PrintSave)
        return;
      ArrayList requiredDocs = this.stackingTemplate.RequiredDocs;
      foreach (string docName in this.stackingTemplate.DocNames)
      {
        if (requiredDocs.Contains((object) docName))
        {
          bool flag = false;
          foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocuments.Items)
          {
            if (gvItem.Checked)
            {
              DocumentLog tag = (DocumentLog) gvItem.Tag;
              if (tag != null && this.docMatchesStackingItem(tag, docName) && this.loanDataMgr.FileAttachments.ContainsAttachment(tag))
              {
                flag = true;
                break;
              }
            }
          }
          if (!flag)
          {
            this.isListValid = false;
            break;
          }
        }
      }
    }

    private bool docMatchesStackingItem(DocumentLog doc, string stackingName)
    {
      return doc.Title.ToLower() == stackingName.ToLower() || this.stackingTemplate.NDEDocGroups.Contains((object) stackingName) && doc.GroupName.ToLower() == stackingName.ToLower();
    }

    private void gvDocuments_SubItemCheck(object source, GVSubItemEventArgs e) => this.setButtons();

    private void setButtons()
    {
      int length = this.getSelectedDocuments().Length;
      this.btnPreview.Enabled = length > 0;
      this.btnContinue.Enabled = length > 0 || this.allowContinue;
    }

    private string createPdf()
    {
      DocumentLog[] selectedDocuments = this.getSelectedDocuments();
      using (PdfFileBuilder pdfFileBuilder = new PdfFileBuilder(this.loanDataMgr))
        return pdfFileBuilder.CreateFile(selectedDocuments);
    }

    private void btnPreview_Click(object sender, EventArgs e)
    {
      string pdf = this.createPdf();
      if (pdf == null)
        return;
      using (PdfPreviewDialog pdfPreviewDialog = new PdfPreviewDialog(pdf, false, true, false))
      {
        int num = (int) pdfPreviewDialog.ShowDialog((IWin32Window) this);
      }
    }

    private void btnContinue_Click(object sender, EventArgs e)
    {
      this.selectedList = this.getSelectedDocuments();
      if (this.selectDocumentsReasonType == SelectDocumentsReasonType.DocumentExport && this.exportTemplate != null)
      {
        if (!this.exportTemplate.ExportLocationSet)
        {
          FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
          if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            this.exportTemplate.ExportLocation = folderBrowserDialog.SelectedPath;
        }
        if (!string.IsNullOrEmpty(this.exportTemplate.ExportLocation))
        {
          this.Cursor = Cursors.WaitCursor;
          this.btnContinue.Enabled = false;
          this.btnCancel.Enabled = false;
          using (DocumentExportDialog documentExportDialog = new DocumentExportDialog(this.loanguids, this.stackingTemplate, this.exportTemplate))
          {
            this.Cursor = Cursors.Default;
            this.DialogResult = documentExportDialog.ShowDialog((IWin32Window) Form.ActiveForm);
          }
        }
      }
      this.DialogResult = DialogResult.OK;
    }

    private void SelectDocumentsDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
    }

    private void initializeExportTemplate()
    {
      int num = 0;
      if (this.defaultExportEntry != null)
      {
        this.exportTemplate = (DocumentExportTemplate) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentExportTemplate, this.defaultExportEntry);
        if (this.exportTemplate != null)
          num = this.cboExportTemplate.Items.Add((object) new FileSystemEntryListItem(this.defaultExportEntry));
      }
      foreach (FileSystemEntry templateDirEntry in Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentExportTemplate, FileSystemEntry.PublicRoot))
      {
        FileSystemEntryListItem systemEntryListItem = new FileSystemEntryListItem(templateDirEntry);
        if (!this.cboExportTemplate.Items.Contains((object) systemEntryListItem))
          this.cboExportTemplate.Items.Add((object) systemEntryListItem);
      }
      if (this.cboExportTemplate.Items.Count < 1)
        num = -1;
      this.cboExportTemplate.SelectedIndex = num;
    }

    private void cboExportTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboExportTemplate.SelectedItem is FileSystemEntryListItem)
      {
        this.exportTemplate = (DocumentExportTemplate) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentExportTemplate, ((FileSystemEntryListItem) this.cboExportTemplate.SelectedItem).Entry);
        this.setExportTemplateStackingOrder();
        this.gvDocuments.SortOption = GVSortOption.Owner;
        this.gvDocuments.SortIconVisible = false;
        if (this.gvDocuments.Items.Count > 0)
          this.gvDocuments.ReSort();
      }
      else
      {
        this.exportTemplate = (DocumentExportTemplate) null;
        this.gvDocuments.SortOption = GVSortOption.Auto;
        this.gvDocuments.SortIconVisible = true;
      }
      this.initDocumentList();
      this.loadDocumentList();
    }

    private void setExportTemplateStackingOrder()
    {
      if (this.exportTemplate == null)
        return;
      foreach (FileSystemEntry templateDirEntry in Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder, FileSystemEntry.PublicRoot))
      {
        StackingOrderSetTemplate templateSettings = (StackingOrderSetTemplate) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder, templateDirEntry);
        if (templateSettings != null && templateSettings.DocumentStackingTemplateID == this.exportTemplate.DocumentStackingTemplateID)
        {
          this.stackingTemplate = templateSettings;
          this.lblStackingOrder.Text = "Stacking Order: " + this.stackingTemplate.TemplateName;
          break;
        }
      }
    }

    private void initializeStackingOrder()
    {
      if (this.selectDocumentsReasonType == SelectDocumentsReasonType.DocumentExport)
      {
        this.setExportTemplateStackingOrder();
        this.initDocumentList();
        this.loadDocumentList();
      }
      else
      {
        int num = this.cboStackingOrder.Items.Add((object) "None");
        if (this.defaultStackingEntry != null)
          num = this.cboStackingOrder.Items.Add((object) new FileSystemEntryListItem(this.defaultStackingEntry));
        this.cboStackingOrder.SelectedIndex = num;
      }
    }

    private void cboStackingOrder_DropDown(object sender, EventArgs e)
    {
      foreach (FileSystemEntry templateDirEntry in Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder, FileSystemEntry.PublicRoot))
      {
        FileSystemEntryListItem systemEntryListItem = new FileSystemEntryListItem(templateDirEntry);
        if (!this.cboStackingOrder.Items.Contains((object) systemEntryListItem))
          this.cboStackingOrder.Items.Add((object) systemEntryListItem);
      }
      this.cboStackingOrder.DropDown -= new EventHandler(this.cboStackingOrder_DropDown);
    }

    private void cboStackingOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboStackingOrder.SelectedItem is FileSystemEntryListItem)
      {
        this.stackingTemplate = (StackingOrderSetTemplate) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder, ((FileSystemEntryListItem) this.cboStackingOrder.SelectedItem).Entry);
        this.gvDocuments.SortOption = GVSortOption.Owner;
        this.gvDocuments.SortIconVisible = false;
        if (this.gvDocuments.Items.Count > 0)
          this.gvDocuments.ReSort();
      }
      else
      {
        this.stackingTemplate = (StackingOrderSetTemplate) null;
        this.gvDocuments.SortOption = GVSortOption.Auto;
        this.gvDocuments.SortIconVisible = true;
      }
      this.initDocumentList();
      this.loadDocumentList();
      this.setButtons();
    }

    private void btnUpdateTemplate_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Any edits will be applied to the selected master stacking order template. Would you like to proceed?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
        return;
      using (StackingOrderSetTemplateDialog setTemplateDialog = new StackingOrderSetTemplateDialog(this.stackingTemplate))
      {
        if (setTemplateDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        StackingOrderSetTemplate stackingOrderTemplate = setTemplateDialog.StackingOrderTemplate;
        Session.ConfigurationManager.SaveTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder, ((FileSystemEntryListItem) this.cboStackingOrder.SelectedItem).Entry, (BinaryObject) (BinaryConvertibleObject) stackingOrderTemplate);
        Tracing.Log(SelectDocumentsDialog.sw, TraceLevel.Warning, nameof (SelectDocumentsDialog), "Stacking Template '" + stackingOrderTemplate.TemplateName + "' updated by " + Session.UserID);
        this.stackingTemplate = stackingOrderTemplate;
        this.initDocumentList();
        this.loadDocumentList();
        this.setButtons();
      }
    }

    private void gvDocuments_SortItems(object source, GVColumnSortEventArgs e)
    {
      if (this.stackingTemplate != null)
        this.gvDocuments.Items.Sort((IComparer<GVItem>) new DocumentSortComparer(this.loanDataMgr.LoanData, this.stackingTemplate));
      e.Cancel = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SelectDocumentsDialog));
      this.helpLink = new EMHelpLink();
      this.gradStackingOrder = new GradientPanel();
      this.btnUpdateTemplate = new Button();
      this.lblExportDropdown = new Label();
      this.lblStackingOrder = new Label();
      this.cboExportTemplate = new ComboBox();
      this.lblInstructions = new Label();
      this.cboStackingOrder = new ComboBox();
      this.lblDropdown = new Label();
      this.pnlClose = new Panel();
      this.pnlStackingKey = new Panel();
      this.lblOptionalDesc = new Label();
      this.lblRequiredDesc = new Label();
      this.lblRequiredBox = new Label();
      this.lblOptionalBox = new Label();
      this.btnPreview = new Button();
      this.btnCancel = new Button();
      this.emHelpLink1 = new EMHelpLink();
      this.btnContinue = new Button();
      this.gcDocuments = new GroupContainer();
      this.gvDocuments = new GridView();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnAddDocs = new Button();
      this.separator1 = new VerticalSeparator();
      this.gradStackingOrder.SuspendLayout();
      this.pnlClose.SuspendLayout();
      this.pnlStackingKey.SuspendLayout();
      this.gcDocuments.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      this.SuspendLayout();
      this.helpLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Send Files";
      this.helpLink.Location = new Point(12, 501);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 19);
      this.helpLink.TabIndex = 17;
      this.helpLink.TabStop = false;
      this.gradStackingOrder.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradStackingOrder.Controls.Add((Control) this.btnUpdateTemplate);
      this.gradStackingOrder.Controls.Add((Control) this.lblExportDropdown);
      this.gradStackingOrder.Controls.Add((Control) this.lblStackingOrder);
      this.gradStackingOrder.Controls.Add((Control) this.cboExportTemplate);
      this.gradStackingOrder.Controls.Add((Control) this.lblInstructions);
      this.gradStackingOrder.Controls.Add((Control) this.cboStackingOrder);
      this.gradStackingOrder.Controls.Add((Control) this.lblDropdown);
      this.gradStackingOrder.Dock = DockStyle.Top;
      this.gradStackingOrder.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradStackingOrder.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradStackingOrder.Location = new Point(0, 0);
      this.gradStackingOrder.Name = "gradStackingOrder";
      this.gradStackingOrder.Padding = new Padding(8, 0, 0, 0);
      this.gradStackingOrder.Size = new Size(800, 82);
      this.gradStackingOrder.TabIndex = 28;
      this.btnUpdateTemplate.BackColor = SystemColors.Control;
      this.btnUpdateTemplate.Location = new Point(679, 46);
      this.btnUpdateTemplate.Margin = new Padding(0);
      this.btnUpdateTemplate.Name = "btnUpdateTemplate";
      this.btnUpdateTemplate.Size = new Size(117, 22);
      this.btnUpdateTemplate.TabIndex = 33;
      this.btnUpdateTemplate.TabStop = false;
      this.btnUpdateTemplate.Text = "Update Template";
      this.btnUpdateTemplate.UseVisualStyleBackColor = true;
      this.btnUpdateTemplate.Click += new EventHandler(this.btnUpdateTemplate_Click);
      this.lblExportDropdown.AutoSize = true;
      this.lblExportDropdown.BackColor = Color.Transparent;
      this.lblExportDropdown.Location = new Point(12, 29);
      this.lblExportDropdown.Name = "lblExportDropdown";
      this.lblExportDropdown.Size = new Size(83, 14);
      this.lblExportDropdown.TabIndex = 32;
      this.lblExportDropdown.Text = "Export Template";
      this.lblStackingOrder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.lblStackingOrder.AutoSize = true;
      this.lblStackingOrder.BackColor = Color.Transparent;
      this.lblStackingOrder.Location = new Point(98, 54);
      this.lblStackingOrder.Name = "lblStackingOrder";
      this.lblStackingOrder.Size = new Size(82, 14);
      this.lblStackingOrder.TabIndex = 31;
      this.lblStackingOrder.Text = "Stacking Order:";
      this.lblStackingOrder.Visible = false;
      this.cboExportTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboExportTemplate.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboExportTemplate.FormattingEnabled = true;
      this.cboExportTemplate.Location = new Point(102, 26);
      this.cboExportTemplate.Name = "cboExportTemplate";
      this.cboExportTemplate.Size = new Size(574, 22);
      this.cboExportTemplate.TabIndex = 30;
      this.cboExportTemplate.SelectedIndexChanged += new EventHandler(this.cboExportTemplate_SelectedIndexChanged);
      this.lblInstructions.BackColor = Color.Transparent;
      this.lblInstructions.Location = new Point(11, 8);
      this.lblInstructions.Name = "lblInstructions";
      this.lblInstructions.Size = new Size(776, 35);
      this.lblInstructions.TabIndex = 27;
      this.lblInstructions.Text = "Choose a stacking order to arrange the documents. Click Continue when done selecting the documents.";
      this.cboStackingOrder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboStackingOrder.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboStackingOrder.FormattingEnabled = true;
      this.cboStackingOrder.Location = new Point(90, 46);
      this.cboStackingOrder.Name = "cboStackingOrder";
      this.cboStackingOrder.Size = new Size(586, 22);
      this.cboStackingOrder.TabIndex = 29;
      this.cboStackingOrder.DropDown += new EventHandler(this.cboStackingOrder_DropDown);
      this.cboStackingOrder.SelectedIndexChanged += new EventHandler(this.cboStackingOrder_SelectedIndexChanged);
      this.lblDropdown.AutoSize = true;
      this.lblDropdown.BackColor = Color.Transparent;
      this.lblDropdown.Location = new Point(11, 50);
      this.lblDropdown.Name = "lblDropdown";
      this.lblDropdown.Size = new Size(79, 14);
      this.lblDropdown.TabIndex = 28;
      this.lblDropdown.Text = "Stacking Order";
      this.pnlClose.Controls.Add((Control) this.pnlStackingKey);
      this.pnlClose.Controls.Add((Control) this.btnPreview);
      this.pnlClose.Controls.Add((Control) this.btnCancel);
      this.pnlClose.Controls.Add((Control) this.emHelpLink1);
      this.pnlClose.Controls.Add((Control) this.btnContinue);
      this.pnlClose.Dock = DockStyle.Bottom;
      this.pnlClose.Location = new Point(0, 441);
      this.pnlClose.Name = "pnlClose";
      this.pnlClose.Size = new Size(800, 52);
      this.pnlClose.TabIndex = 60;
      this.pnlStackingKey.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pnlStackingKey.BackColor = Color.Transparent;
      this.pnlStackingKey.Controls.Add((Control) this.lblOptionalDesc);
      this.pnlStackingKey.Controls.Add((Control) this.lblRequiredDesc);
      this.pnlStackingKey.Controls.Add((Control) this.lblRequiredBox);
      this.pnlStackingKey.Controls.Add((Control) this.lblOptionalBox);
      this.pnlStackingKey.Location = new Point(13, 5);
      this.pnlStackingKey.Name = "pnlStackingKey";
      this.pnlStackingKey.Size = new Size(435, 22);
      this.pnlStackingKey.TabIndex = 28;
      this.lblOptionalDesc.AutoSize = true;
      this.lblOptionalDesc.Location = new Point(21, 2);
      this.lblOptionalDesc.Name = "lblOptionalDesc";
      this.lblOptionalDesc.Size = new Size(181, 14);
      this.lblOptionalDesc.TabIndex = 29;
      this.lblOptionalDesc.Text = "Optional documents that are missing";
      this.lblRequiredDesc.AutoSize = true;
      this.lblRequiredDesc.Location = new Point(237, 2);
      this.lblRequiredDesc.Name = "lblRequiredDesc";
      this.lblRequiredDesc.Size = new Size(185, 14);
      this.lblRequiredDesc.TabIndex = 28;
      this.lblRequiredDesc.Text = "Required documents that are missing";
      this.lblRequiredBox.AutoSize = true;
      this.lblRequiredBox.BorderStyle = BorderStyle.FixedSingle;
      this.lblRequiredBox.Location = new Point(216, 0);
      this.lblRequiredBox.Name = "lblRequiredBox";
      this.lblRequiredBox.Size = new Size(15, 16);
      this.lblRequiredBox.TabIndex = 27;
      this.lblRequiredBox.Text = "  ";
      this.lblOptionalBox.AutoSize = true;
      this.lblOptionalBox.BorderStyle = BorderStyle.FixedSingle;
      this.lblOptionalBox.Location = new Point(0, 0);
      this.lblOptionalBox.Name = "lblOptionalBox";
      this.lblOptionalBox.Size = new Size(15, 16);
      this.lblOptionalBox.TabIndex = 26;
      this.lblOptionalBox.Text = "  ";
      this.btnPreview.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnPreview.Enabled = false;
      this.btnPreview.Location = new Point(555, 16);
      this.btnPreview.Margin = new Padding(1, 0, 0, 0);
      this.btnPreview.Name = "btnPreview";
      this.btnPreview.Size = new Size(75, 24);
      this.btnPreview.TabIndex = 24;
      this.btnPreview.Text = "Preview";
      this.btnPreview.UseVisualStyleBackColor = true;
      this.btnPreview.Click += new EventHandler(this.btnPreview_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(713, 16);
      this.btnCancel.Margin = new Padding(1, 0, 0, 0);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 26;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Send Files";
      this.emHelpLink1.Location = new Point(13, 28);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 19);
      this.emHelpLink1.TabIndex = 27;
      this.emHelpLink1.TabStop = false;
      this.btnContinue.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnContinue.Enabled = false;
      this.btnContinue.Location = new Point(634, 16);
      this.btnContinue.Margin = new Padding(1, 0, 0, 0);
      this.btnContinue.Name = "btnContinue";
      this.btnContinue.Size = new Size(75, 24);
      this.btnContinue.TabIndex = 25;
      this.btnContinue.Text = "Continue";
      this.btnContinue.UseVisualStyleBackColor = true;
      this.btnContinue.Click += new EventHandler(this.btnContinue_Click);
      this.gcDocuments.Controls.Add((Control) this.gvDocuments);
      this.gcDocuments.Controls.Add((Control) this.pnlToolbar);
      this.gcDocuments.Dock = DockStyle.Fill;
      this.gcDocuments.HeaderForeColor = SystemColors.ControlText;
      this.gcDocuments.Location = new Point(0, 82);
      this.gcDocuments.Name = "gcDocuments";
      this.gcDocuments.Size = new Size(800, 359);
      this.gcDocuments.TabIndex = 61;
      this.gcDocuments.Text = "Documents";
      this.gvDocuments.AllowMultiselect = false;
      this.gvDocuments.BorderStyle = BorderStyle.None;
      this.gvDocuments.ClearSelectionsOnEmptyRowClick = false;
      this.gvDocuments.Dock = DockStyle.Fill;
      this.gvDocuments.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDocuments.Location = new Point(1, 26);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(798, 332);
      this.gvDocuments.TabIndex = 5;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDocuments.SortItems += new GVColumnSortEventHandler(this.gvDocuments_SortItems);
      this.gvDocuments.SubItemCheck += new GVSubItemEventHandler(this.gvDocuments_SubItemCheck);
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnAddDocs);
      this.pnlToolbar.Controls.Add((Control) this.separator1);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(172, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(624, 22);
      this.pnlToolbar.TabIndex = 4;
      this.btnAddDocs.BackColor = SystemColors.Control;
      this.btnAddDocs.Location = new Point(507, 0);
      this.btnAddDocs.Margin = new Padding(0);
      this.btnAddDocs.Name = "btnAddDocs";
      this.btnAddDocs.Size = new Size(117, 22);
      this.btnAddDocs.TabIndex = 9;
      this.btnAddDocs.TabStop = false;
      this.btnAddDocs.Text = "Add Additional Docs";
      this.btnAddDocs.UseVisualStyleBackColor = true;
      this.btnAddDocs.Visible = false;
      this.separator1.Location = new Point(502, 3);
      this.separator1.Margin = new Padding(4, 3, 3, 3);
      this.separator1.MaximumSize = new Size(2, 16);
      this.separator1.MinimumSize = new Size(2, 16);
      this.separator1.Name = "separator1";
      this.separator1.Size = new Size(2, 16);
      this.separator1.TabIndex = 5;
      this.separator1.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(800, 493);
      this.Controls.Add((Control) this.gcDocuments);
      this.Controls.Add((Control) this.pnlClose);
      this.Controls.Add((Control) this.gradStackingOrder);
      this.Controls.Add((Control) this.helpLink);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = Resources.icon_allsizes_bug;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelectDocumentsDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Documents";
      this.KeyDown += new KeyEventHandler(this.SelectDocumentsDialog_KeyDown);
      this.gradStackingOrder.ResumeLayout(false);
      this.gradStackingOrder.PerformLayout();
      this.pnlClose.ResumeLayout(false);
      this.pnlStackingKey.ResumeLayout(false);
      this.pnlStackingKey.PerformLayout();
      this.gcDocuments.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}

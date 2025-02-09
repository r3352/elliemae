// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanFileRightsDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanFileRightsDialog : Form, IHelp
  {
    private Sessions.Session session;
    private BizRule.LoanAccessRight loanRights;
    private string[] editableFields;
    private System.ComponentModel.Container components;
    private Hashtable selectedFields;
    private GridView gvFields;
    private ComboBox cboDocuments;
    private Label lblDocuments;
    private Label lblConditions;
    private Label lblLockRequest;
    private Label lblProfitMgr;
    private Label lblTask;
    private Label lblConversation;
    private ComboBox cboDisclosureTracking;
    private ComboBox cboLockRequest;
    private ComboBox cboProfitMgr;
    private ComboBox cboTask;
    private ComboBox cboConversation;
    private ComboBox cboConditions;
    private Label lblDisclosureTracking;
    private FieldSettings fieldSettings;
    private StandardIconButton btnFindField;
    private StandardIconButton btnAddField;
    private StandardIconButton btnDeleteField;
    private GroupContainer gcDocumentsPartial;
    private GroupContainer gcFields;
    private Button buttonCancel;
    private CheckBox chkUnassignedFiles;
    private ComboBox cboRetrieveService;
    private Label lblRetrieveService;
    private CheckBox chkRequestService;
    private ComboBox cboRetrieveBorrower;
    private Label lblRetrieveBorrower;
    private CheckBox chkRequestBorrower;
    private CheckBox chkProtectedDocs;
    private CheckBox chkUnprotectedDocs;
    private CheckBox chkOrderDisclosures;
    private CheckBox chkCreateDocs;
    private EMHelpLink helpLink;
    private Button buttonOK;

    public LoanFileRightsDialog(
      Sessions.Session session,
      BizRule.LoanAccessRight loanRights,
      string[] editableFields)
    {
      this.InitializeComponent();
      this.session = session;
      this.loanRights = loanRights;
      this.editableFields = editableFields;
      this.initForm();
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.lblDisclosureTracking = new Label();
      this.lblLockRequest = new Label();
      this.lblProfitMgr = new Label();
      this.lblTask = new Label();
      this.lblConversation = new Label();
      this.lblConditions = new Label();
      this.lblDocuments = new Label();
      this.cboDisclosureTracking = new ComboBox();
      this.gvFields = new GridView();
      this.cboLockRequest = new ComboBox();
      this.cboProfitMgr = new ComboBox();
      this.cboTask = new ComboBox();
      this.cboConversation = new ComboBox();
      this.cboConditions = new ComboBox();
      this.cboDocuments = new ComboBox();
      this.buttonCancel = new Button();
      this.buttonOK = new Button();
      this.gcFields = new GroupContainer();
      this.btnFindField = new StandardIconButton();
      this.btnAddField = new StandardIconButton();
      this.btnDeleteField = new StandardIconButton();
      this.gcDocumentsPartial = new GroupContainer();
      this.chkOrderDisclosures = new CheckBox();
      this.chkCreateDocs = new CheckBox();
      this.cboRetrieveService = new ComboBox();
      this.lblRetrieveService = new Label();
      this.chkRequestService = new CheckBox();
      this.cboRetrieveBorrower = new ComboBox();
      this.lblRetrieveBorrower = new Label();
      this.chkRequestBorrower = new CheckBox();
      this.chkProtectedDocs = new CheckBox();
      this.chkUnprotectedDocs = new CheckBox();
      this.chkUnassignedFiles = new CheckBox();
      this.helpLink = new EMHelpLink();
      this.gcFields.SuspendLayout();
      ((ISupportInitialize) this.btnFindField).BeginInit();
      ((ISupportInitialize) this.btnAddField).BeginInit();
      ((ISupportInitialize) this.btnDeleteField).BeginInit();
      this.gcDocumentsPartial.SuspendLayout();
      this.SuspendLayout();
      this.lblDisclosureTracking.AutoSize = true;
      this.lblDisclosureTracking.Location = new Point(12, 188);
      this.lblDisclosureTracking.Name = "lblDisclosureTracking";
      this.lblDisclosureTracking.Size = new Size(124, 14);
      this.lblDisclosureTracking.TabIndex = 13;
      this.lblDisclosureTracking.Text = "Disclosure Tracking Tool";
      this.lblLockRequest.AutoSize = true;
      this.lblLockRequest.Location = new Point(12, 160);
      this.lblLockRequest.Name = "lblLockRequest";
      this.lblLockRequest.Size = new Size(95, 14);
      this.lblLockRequest.TabIndex = 11;
      this.lblLockRequest.Text = "Lock Request Tool";
      this.lblProfitMgr.AutoSize = true;
      this.lblProfitMgr.Location = new Point(12, 132);
      this.lblProfitMgr.Name = "lblProfitMgr";
      this.lblProfitMgr.Size = new Size(96, 14);
      this.lblProfitMgr.TabIndex = 9;
      this.lblProfitMgr.Text = "Profit Management";
      this.lblTask.AutoSize = true;
      this.lblTask.Location = new Point(12, 104);
      this.lblTask.Name = "lblTask";
      this.lblTask.Size = new Size(29, 14);
      this.lblTask.TabIndex = 7;
      this.lblTask.Text = "Task";
      this.lblConversation.AutoSize = true;
      this.lblConversation.Location = new Point(12, 76);
      this.lblConversation.Name = "lblConversation";
      this.lblConversation.Size = new Size(92, 14);
      this.lblConversation.TabIndex = 5;
      this.lblConversation.Text = "Conversation Log";
      this.lblConditions.AutoSize = true;
      this.lblConditions.Location = new Point(12, 48);
      this.lblConditions.Name = "lblConditions";
      this.lblConditions.Size = new Size(136, 14);
      this.lblConditions.TabIndex = 3;
      this.lblConditions.Text = "eFolder UW Conditions Tab";
      this.lblDocuments.AutoSize = true;
      this.lblDocuments.Location = new Point(12, 20);
      this.lblDocuments.Name = "lblDocuments";
      this.lblDocuments.Size = new Size(120, 14);
      this.lblDocuments.TabIndex = 0;
      this.lblDocuments.Text = "eFolder Documents Tab";
      this.cboDisclosureTracking.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboDisclosureTracking.FormattingEnabled = true;
      this.cboDisclosureTracking.Items.AddRange(new object[3]
      {
        (object) "Hide",
        (object) "View Only ",
        (object) "Edit"
      });
      this.cboDisclosureTracking.Location = new Point(152, 184);
      this.cboDisclosureTracking.Name = "cboDisclosureTracking";
      this.cboDisclosureTracking.Size = new Size(121, 22);
      this.cboDisclosureTracking.TabIndex = 14;
      this.gvFields.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 109;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Description";
      gvColumn2.Width = 633;
      this.gvFields.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvFields.Dock = DockStyle.Fill;
      this.gvFields.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvFields.Location = new Point(1, 26);
      this.gvFields.Name = "gvFields";
      this.gvFields.Size = new Size(742, 188);
      this.gvFields.TabIndex = 0;
      this.cboLockRequest.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboLockRequest.FormattingEnabled = true;
      this.cboLockRequest.Items.AddRange(new object[3]
      {
        (object) "Hide",
        (object) "View Only ",
        (object) "Edit"
      });
      this.cboLockRequest.Location = new Point(152, 156);
      this.cboLockRequest.Name = "cboLockRequest";
      this.cboLockRequest.Size = new Size(121, 22);
      this.cboLockRequest.TabIndex = 12;
      this.cboProfitMgr.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboProfitMgr.FormattingEnabled = true;
      this.cboProfitMgr.Items.AddRange(new object[3]
      {
        (object) "Hide",
        (object) "View Only ",
        (object) "Edit"
      });
      this.cboProfitMgr.Location = new Point(152, 128);
      this.cboProfitMgr.Name = "cboProfitMgr";
      this.cboProfitMgr.Size = new Size(121, 22);
      this.cboProfitMgr.TabIndex = 10;
      this.cboTask.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTask.FormattingEnabled = true;
      this.cboTask.Items.AddRange(new object[3]
      {
        (object) "Hide",
        (object) "View Only ",
        (object) "Edit"
      });
      this.cboTask.Location = new Point(152, 100);
      this.cboTask.Name = "cboTask";
      this.cboTask.Size = new Size(121, 22);
      this.cboTask.TabIndex = 8;
      this.cboConversation.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboConversation.FormattingEnabled = true;
      this.cboConversation.Items.AddRange(new object[3]
      {
        (object) "Hide",
        (object) "View Only ",
        (object) "Edit"
      });
      this.cboConversation.Location = new Point(152, 72);
      this.cboConversation.Name = "cboConversation";
      this.cboConversation.Size = new Size(121, 22);
      this.cboConversation.TabIndex = 6;
      this.cboConditions.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboConditions.FormattingEnabled = true;
      this.cboConditions.Items.AddRange(new object[3]
      {
        (object) "Hide",
        (object) "View Only ",
        (object) "Edit"
      });
      this.cboConditions.Location = new Point(152, 44);
      this.cboConditions.Name = "cboConditions";
      this.cboConditions.Size = new Size(121, 22);
      this.cboConditions.TabIndex = 4;
      this.cboDocuments.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboDocuments.FormattingEnabled = true;
      this.cboDocuments.Items.AddRange(new object[4]
      {
        (object) "Hide",
        (object) "View Only ",
        (object) "Partial Edit",
        (object) "Full Edit"
      });
      this.cboDocuments.Location = new Point(152, 16);
      this.cboDocuments.Name = "cboDocuments";
      this.cboDocuments.Size = new Size(121, 22);
      this.cboDocuments.TabIndex = 1;
      this.cboDocuments.SelectedIndexChanged += new EventHandler(this.cboDocuments_SelectedIndexChanged);
      this.buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(682, 480);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(75, 23);
      this.buttonCancel.TabIndex = 18;
      this.buttonCancel.Text = "&Cancel";
      this.buttonOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOK.Location = new Point(602, 480);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new Size(75, 23);
      this.buttonOK.TabIndex = 17;
      this.buttonOK.Text = "&OK";
      this.buttonOK.Click += new EventHandler(this.btnOK_Click);
      this.gcFields.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcFields.Controls.Add((Control) this.btnFindField);
      this.gcFields.Controls.Add((Control) this.btnAddField);
      this.gcFields.Controls.Add((Control) this.btnDeleteField);
      this.gcFields.Controls.Add((Control) this.gvFields);
      this.gcFields.HeaderForeColor = SystemColors.ControlText;
      this.gcFields.Location = new Point(12, 252);
      this.gcFields.Name = "gcFields";
      this.gcFields.Size = new Size(744, 215);
      this.gcFields.TabIndex = 15;
      this.gcFields.Text = "Fields with Edit Access";
      this.btnFindField.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnFindField.BackColor = Color.Transparent;
      this.btnFindField.Location = new Point(704, 5);
      this.btnFindField.MouseDownImage = (Image) null;
      this.btnFindField.Name = "btnFindField";
      this.btnFindField.Size = new Size(16, 16);
      this.btnFindField.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnFindField.TabIndex = 29;
      this.btnFindField.TabStop = false;
      this.btnFindField.Click += new EventHandler(this.btnFindField_Click);
      this.btnAddField.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddField.BackColor = Color.Transparent;
      this.btnAddField.Location = new Point(685, 5);
      this.btnAddField.MouseDownImage = (Image) null;
      this.btnAddField.Name = "btnAddField";
      this.btnAddField.Size = new Size(16, 16);
      this.btnAddField.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddField.TabIndex = 28;
      this.btnAddField.TabStop = false;
      this.btnAddField.Click += new EventHandler(this.btnAddField_Click);
      this.btnDeleteField.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteField.BackColor = Color.Transparent;
      this.btnDeleteField.Location = new Point(723, 5);
      this.btnDeleteField.MouseDownImage = (Image) null;
      this.btnDeleteField.Name = "btnDeleteField";
      this.btnDeleteField.Size = new Size(16, 16);
      this.btnDeleteField.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteField.TabIndex = 27;
      this.btnDeleteField.TabStop = false;
      this.btnDeleteField.Click += new EventHandler(this.btnDeleteField_Click);
      this.gcDocumentsPartial.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcDocumentsPartial.BackColor = Color.White;
      this.gcDocumentsPartial.Controls.Add((Control) this.chkOrderDisclosures);
      this.gcDocumentsPartial.Controls.Add((Control) this.chkCreateDocs);
      this.gcDocumentsPartial.Controls.Add((Control) this.cboRetrieveService);
      this.gcDocumentsPartial.Controls.Add((Control) this.lblRetrieveService);
      this.gcDocumentsPartial.Controls.Add((Control) this.chkRequestService);
      this.gcDocumentsPartial.Controls.Add((Control) this.cboRetrieveBorrower);
      this.gcDocumentsPartial.Controls.Add((Control) this.lblRetrieveBorrower);
      this.gcDocumentsPartial.Controls.Add((Control) this.chkRequestBorrower);
      this.gcDocumentsPartial.Controls.Add((Control) this.chkProtectedDocs);
      this.gcDocumentsPartial.Controls.Add((Control) this.chkUnprotectedDocs);
      this.gcDocumentsPartial.Controls.Add((Control) this.chkUnassignedFiles);
      this.gcDocumentsPartial.HeaderForeColor = SystemColors.ControlText;
      this.gcDocumentsPartial.Location = new Point(288, 16);
      this.gcDocumentsPartial.Name = "gcDocumentsPartial";
      this.gcDocumentsPartial.Size = new Size(468, 220);
      this.gcDocumentsPartial.TabIndex = 2;
      this.gcDocumentsPartial.Text = "eFolder Documents Tab Partial Edit";
      this.chkOrderDisclosures.AutoSize = true;
      this.chkOrderDisclosures.Location = new Point(8, 112);
      this.chkOrderDisclosures.Name = "chkOrderDisclosures";
      this.chkOrderDisclosures.Size = new Size(233, 18);
      this.chkOrderDisclosures.TabIndex = 4;
      this.chkOrderDisclosures.Text = "Retain current rights to Order eDisclosures";
      this.chkOrderDisclosures.UseVisualStyleBackColor = true;
      this.chkCreateDocs.AutoSize = true;
      this.chkCreateDocs.Location = new Point(8, 92);
      this.chkCreateDocs.Name = "chkCreateDocs";
      this.chkCreateDocs.Size = new Size(275, 18);
      this.chkCreateDocs.TabIndex = 3;
      this.chkCreateDocs.Text = "Retain current rights to Create/Duplicate Documents";
      this.chkCreateDocs.UseVisualStyleBackColor = true;
      this.cboRetrieveService.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRetrieveService.Enabled = false;
      this.cboRetrieveService.FormattingEnabled = true;
      this.cboRetrieveService.Items.AddRange(new object[3]
      {
        (object) "Document and mark current",
        (object) "Document and not mark current",
        (object) "Unassigned Files"
      });
      this.cboRetrieveService.Location = new Point(188, 188);
      this.cboRetrieveService.Name = "cboRetrieveService";
      this.cboRetrieveService.Size = new Size(188, 22);
      this.cboRetrieveService.TabIndex = 10;
      this.lblRetrieveService.AutoSize = true;
      this.lblRetrieveService.Location = new Point(40, 192);
      this.lblRetrieveService.Name = "lblRetrieveService";
      this.lblRetrieveService.Size = new Size(147, 14);
      this.lblRetrieveService.TabIndex = 9;
      this.lblRetrieveService.Text = "When retrieving, put files into";
      this.chkRequestService.AutoSize = true;
      this.chkRequestService.Location = new Point(8, 172);
      this.chkRequestService.Name = "chkRequestService";
      this.chkRequestService.Size = new Size(455, 18);
      this.chkRequestService.TabIndex = 8;
      this.chkRequestService.Text = "Retain current rights to Request/Retrieve from ICE Mortgage Technology Network Service";
      this.chkRequestService.UseVisualStyleBackColor = true;
      this.chkRequestService.CheckedChanged += new EventHandler(this.chkRequestService_CheckedChanged);
      this.cboRetrieveBorrower.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRetrieveBorrower.Enabled = false;
      this.cboRetrieveBorrower.FormattingEnabled = true;
      this.cboRetrieveBorrower.Items.AddRange(new object[3]
      {
        (object) "Document and mark current",
        (object) "Document and not mark current",
        (object) "Unassigned Files"
      });
      this.cboRetrieveBorrower.Location = new Point(188, 148);
      this.cboRetrieveBorrower.Name = "cboRetrieveBorrower";
      this.cboRetrieveBorrower.Size = new Size(188, 22);
      this.cboRetrieveBorrower.TabIndex = 7;
      this.lblRetrieveBorrower.AutoSize = true;
      this.lblRetrieveBorrower.Location = new Point(40, 152);
      this.lblRetrieveBorrower.Name = "lblRetrieveBorrower";
      this.lblRetrieveBorrower.Size = new Size(147, 14);
      this.lblRetrieveBorrower.TabIndex = 6;
      this.lblRetrieveBorrower.Text = "When retrieving, put files into";
      this.chkRequestBorrower.AutoSize = true;
      this.chkRequestBorrower.Location = new Point(8, 132);
      this.chkRequestBorrower.Name = "chkRequestBorrower";
      this.chkRequestBorrower.Size = new Size(329, 18);
      this.chkRequestBorrower.TabIndex = 5;
      this.chkRequestBorrower.Text = "Retain current rights to Request/Retrieve Borrower Documents";
      this.chkRequestBorrower.UseVisualStyleBackColor = true;
      this.chkRequestBorrower.CheckedChanged += new EventHandler(this.chkRequestBorrower_CheckedChanged);
      this.chkProtectedDocs.AutoSize = true;
      this.chkProtectedDocs.Location = new Point(8, 72);
      this.chkProtectedDocs.Name = "chkProtectedDocs";
      this.chkProtectedDocs.Size = new Size(242, 18);
      this.chkProtectedDocs.TabIndex = 2;
      this.chkProtectedDocs.Text = "Retain current rights to Protected Documents";
      this.chkProtectedDocs.UseVisualStyleBackColor = true;
      this.chkUnprotectedDocs.AutoSize = true;
      this.chkUnprotectedDocs.Location = new Point(8, 52);
      this.chkUnprotectedDocs.Name = "chkUnprotectedDocs";
      this.chkUnprotectedDocs.Size = new Size((int) byte.MaxValue, 18);
      this.chkUnprotectedDocs.TabIndex = 1;
      this.chkUnprotectedDocs.Text = "Retain current rights to Unprotected Documents";
      this.chkUnprotectedDocs.UseVisualStyleBackColor = true;
      this.chkUnassignedFiles.AutoSize = true;
      this.chkUnassignedFiles.Location = new Point(8, 32);
      this.chkUnassignedFiles.Name = "chkUnassignedFiles";
      this.chkUnassignedFiles.Size = new Size(221, 18);
      this.chkUnassignedFiles.TabIndex = 0;
      this.chkUnassignedFiles.Text = "Retain current rights to Unassigned Files";
      this.chkUnassignedFiles.UseVisualStyleBackColor = true;
      this.helpLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Setup\\Persona Access to Loans";
      this.helpLink.Location = new Point(12, 484);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 16);
      this.helpLink.TabIndex = 16;
      this.AcceptButton = (IButtonControl) this.buttonOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.buttonCancel;
      this.ClientSize = new Size(769, 516);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.gcDocumentsPartial);
      this.Controls.Add((Control) this.cboDisclosureTracking);
      this.Controls.Add((Control) this.cboLockRequest);
      this.Controls.Add((Control) this.cboProfitMgr);
      this.Controls.Add((Control) this.cboTask);
      this.Controls.Add((Control) this.cboConversation);
      this.Controls.Add((Control) this.cboConditions);
      this.Controls.Add((Control) this.cboDocuments);
      this.Controls.Add((Control) this.lblDisclosureTracking);
      this.Controls.Add((Control) this.buttonOK);
      this.Controls.Add((Control) this.lblLockRequest);
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.lblProfitMgr);
      this.Controls.Add((Control) this.lblTask);
      this.Controls.Add((Control) this.gcFields);
      this.Controls.Add((Control) this.lblConversation);
      this.Controls.Add((Control) this.lblDocuments);
      this.Controls.Add((Control) this.lblConditions);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanFileRightsDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Edit Custom Access";
      this.KeyDown += new KeyEventHandler(this.LoanFileRightsDialog_KeyDown);
      this.gcFields.ResumeLayout(false);
      ((ISupportInitialize) this.btnFindField).EndInit();
      ((ISupportInitialize) this.btnAddField).EndInit();
      ((ISupportInitialize) this.btnDeleteField).EndInit();
      this.gcDocumentsPartial.ResumeLayout(false);
      this.gcDocumentsPartial.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public BizRule.LoanAccessRight LoanRights => this.loanRights;

    public string[] EditableFields => this.editableFields;

    private void initForm()
    {
      if ((this.loanRights & BizRule.LoanAccessRight.DocTracking) == BizRule.LoanAccessRight.DocTracking)
        this.cboDocuments.SelectedIndex = 3;
      else if ((this.loanRights & BizRule.LoanAccessRight.DocTrackingPartial) == BizRule.LoanAccessRight.DocTrackingPartial)
      {
        this.cboDocuments.SelectedIndex = 2;
        if ((this.loanRights & BizRule.LoanAccessRight.DocTrackingUnassignedFiles) == BizRule.LoanAccessRight.DocTrackingUnassignedFiles)
          this.chkUnassignedFiles.Checked = true;
        if ((this.loanRights & BizRule.LoanAccessRight.DocTrackingUnprotectedDocs) == BizRule.LoanAccessRight.DocTrackingUnprotectedDocs)
          this.chkUnprotectedDocs.Checked = true;
        if ((this.loanRights & BizRule.LoanAccessRight.DocTrackingProtectedDocs) == BizRule.LoanAccessRight.DocTrackingProtectedDocs)
          this.chkProtectedDocs.Checked = true;
        if ((this.loanRights & BizRule.LoanAccessRight.DocTrackingCreateDocs) == BizRule.LoanAccessRight.DocTrackingCreateDocs)
          this.chkCreateDocs.Checked = true;
        if ((this.loanRights & BizRule.LoanAccessRight.DocTrackingOrderDisclosures) == BizRule.LoanAccessRight.DocTrackingOrderDisclosures)
          this.chkOrderDisclosures.Checked = true;
        if ((this.loanRights & BizRule.LoanAccessRight.DocTrackingRequestRetrieveBorrower) == BizRule.LoanAccessRight.DocTrackingRequestRetrieveBorrower)
        {
          this.chkRequestBorrower.Checked = true;
          if ((this.loanRights & BizRule.LoanAccessRight.DocTrackingRetrieveBorrowerUnassigned) == BizRule.LoanAccessRight.DocTrackingRetrieveBorrowerUnassigned)
            this.cboRetrieveBorrower.SelectedIndex = 2;
          else if ((this.loanRights & BizRule.LoanAccessRight.DocTrackingRetrieveBorrowerNotCurrent) == BizRule.LoanAccessRight.DocTrackingRetrieveBorrowerNotCurrent)
            this.cboRetrieveBorrower.SelectedIndex = 1;
          else
            this.cboRetrieveBorrower.SelectedIndex = 0;
        }
        if ((this.loanRights & BizRule.LoanAccessRight.DocTrackingRequestRetrieveService) == BizRule.LoanAccessRight.DocTrackingRequestRetrieveService)
        {
          this.chkRequestService.Checked = true;
          if ((this.loanRights & BizRule.LoanAccessRight.DocTrackingRetrieveServiceUnassigned) == BizRule.LoanAccessRight.DocTrackingRetrieveServiceUnassigned)
            this.cboRetrieveService.SelectedIndex = 2;
          else if ((this.loanRights & BizRule.LoanAccessRight.DocTrackingRetrieveServiceNotCurrent) == BizRule.LoanAccessRight.DocTrackingRetrieveServiceNotCurrent)
            this.cboRetrieveService.SelectedIndex = 1;
          else
            this.cboRetrieveService.SelectedIndex = 0;
        }
      }
      else if ((this.loanRights & BizRule.LoanAccessRight.DocTrackingViewOnly) == BizRule.LoanAccessRight.DocTrackingViewOnly)
        this.cboDocuments.SelectedIndex = 1;
      else
        this.cboDocuments.SelectedIndex = 0;
      if ((this.loanRights & BizRule.LoanAccessRight.ConditionTracking) == BizRule.LoanAccessRight.ConditionTracking)
        this.cboConditions.SelectedIndex = 2;
      else if ((this.loanRights & BizRule.LoanAccessRight.ConditionTrackingViewOnly) == BizRule.LoanAccessRight.ConditionTrackingViewOnly)
        this.cboConditions.SelectedIndex = 1;
      else
        this.cboConditions.SelectedIndex = 0;
      if ((this.loanRights & BizRule.LoanAccessRight.ConversationLog) == BizRule.LoanAccessRight.ConversationLog)
        this.cboConversation.SelectedIndex = 2;
      else if ((this.loanRights & BizRule.LoanAccessRight.ConversationLogViewOnly) == BizRule.LoanAccessRight.ConversationLogViewOnly)
        this.cboConversation.SelectedIndex = 1;
      else
        this.cboConversation.SelectedIndex = 0;
      if ((this.loanRights & BizRule.LoanAccessRight.Task) == BizRule.LoanAccessRight.Task)
        this.cboTask.SelectedIndex = 2;
      else if ((this.loanRights & BizRule.LoanAccessRight.TaskViewOnly) == BizRule.LoanAccessRight.TaskViewOnly)
        this.cboTask.SelectedIndex = 1;
      else
        this.cboTask.SelectedIndex = 0;
      if ((this.loanRights & BizRule.LoanAccessRight.ProfitMgmt) == BizRule.LoanAccessRight.ProfitMgmt)
        this.cboProfitMgr.SelectedIndex = 2;
      else if ((this.loanRights & BizRule.LoanAccessRight.ProfitMgmtViewOnly) == BizRule.LoanAccessRight.ProfitMgmtViewOnly)
        this.cboProfitMgr.SelectedIndex = 1;
      else
        this.cboProfitMgr.SelectedIndex = 0;
      if ((this.loanRights & BizRule.LoanAccessRight.LockRequest) == BizRule.LoanAccessRight.LockRequest)
        this.cboLockRequest.SelectedIndex = 2;
      else if ((this.loanRights & BizRule.LoanAccessRight.LockRequestViewOnly) == BizRule.LoanAccessRight.LockRequestViewOnly)
        this.cboLockRequest.SelectedIndex = 1;
      else
        this.cboLockRequest.SelectedIndex = 0;
      if ((this.loanRights & BizRule.LoanAccessRight.DisclosureTracking) == BizRule.LoanAccessRight.DisclosureTracking)
        this.cboDisclosureTracking.SelectedIndex = 2;
      else if ((this.loanRights & BizRule.LoanAccessRight.DisclosureTrackingViewOnly) == BizRule.LoanAccessRight.DisclosureTrackingViewOnly)
        this.cboDisclosureTracking.SelectedIndex = 1;
      else
        this.cboDisclosureTracking.SelectedIndex = 0;
      this.selectedFields = new Hashtable();
      this.fieldSettings = this.session.LoanManager.GetFieldSettings();
      this.gvFields.Items.Clear();
      if (this.editableFields == null)
        return;
      this.gvFields.BeginUpdate();
      for (int index = 0; index < this.editableFields.Length; ++index)
      {
        if (!this.selectedFields.ContainsKey((object) this.editableFields[index]))
        {
          this.gvFields.Items.Add(this.createListItem(this.editableFields[index]));
          this.selectedFields.Add((object) this.editableFields[index], (object) "");
        }
      }
      this.gvFields.EndUpdate();
    }

    private void chkRequestBorrower_CheckedChanged(object sender, EventArgs e)
    {
      this.cboRetrieveBorrower.Enabled = this.chkRequestBorrower.Checked;
    }

    private void chkRequestService_CheckedChanged(object sender, EventArgs e)
    {
      this.cboRetrieveService.Enabled = this.chkRequestService.Checked;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.loanRights = BizRule.LoanAccessRight.ViewAllOnly;
      switch (this.cboDocuments.SelectedIndex)
      {
        case 1:
          this.loanRights |= BizRule.LoanAccessRight.DocTrackingViewOnly;
          break;
        case 2:
          if (!this.chkUnassignedFiles.Checked && !this.chkUnprotectedDocs.Checked && !this.chkProtectedDocs.Checked && !this.chkCreateDocs.Checked && !this.chkOrderDisclosures.Checked && !this.chkRequestBorrower.Checked && !this.chkRequestService.Checked)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You must select at least one partial right for the eFolder Documents Tab.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          this.loanRights |= BizRule.LoanAccessRight.DocTrackingPartial;
          if (this.chkUnassignedFiles.Checked)
            this.loanRights |= BizRule.LoanAccessRight.DocTrackingUnassignedFiles;
          if (this.chkUnprotectedDocs.Checked)
            this.loanRights |= BizRule.LoanAccessRight.DocTrackingUnprotectedDocs;
          if (this.chkProtectedDocs.Checked)
            this.loanRights |= BizRule.LoanAccessRight.DocTrackingProtectedDocs;
          if (this.chkCreateDocs.Checked)
            this.loanRights |= BizRule.LoanAccessRight.DocTrackingCreateDocs;
          if (this.chkOrderDisclosures.Checked)
            this.loanRights |= BizRule.LoanAccessRight.DocTrackingOrderDisclosures;
          if (this.chkRequestBorrower.Checked)
          {
            this.loanRights |= BizRule.LoanAccessRight.DocTrackingRequestRetrieveBorrower;
            switch (this.cboRetrieveBorrower.SelectedIndex)
            {
              case 0:
                this.loanRights |= BizRule.LoanAccessRight.DocTrackingRetrieveBorrowerCurrent;
                break;
              case 1:
                this.loanRights |= BizRule.LoanAccessRight.DocTrackingRetrieveBorrowerNotCurrent;
                break;
              case 2:
                this.loanRights |= BizRule.LoanAccessRight.DocTrackingRetrieveBorrowerUnassigned;
                break;
              default:
                int num = (int) Utils.Dialog((IWin32Window) this, "You must select where the files will be put when retrieving Borrower documents.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
          }
          if (this.chkRequestService.Checked)
          {
            this.loanRights |= BizRule.LoanAccessRight.DocTrackingRequestRetrieveService;
            switch (this.cboRetrieveService.SelectedIndex)
            {
              case 0:
                this.loanRights |= BizRule.LoanAccessRight.DocTrackingRetrieveServiceCurrent;
                break;
              case 1:
                this.loanRights |= BizRule.LoanAccessRight.DocTrackingRetrieveServiceNotCurrent;
                break;
              case 2:
                this.loanRights |= BizRule.LoanAccessRight.DocTrackingRetrieveServiceUnassigned;
                break;
              default:
                int num = (int) Utils.Dialog((IWin32Window) this, "You must select where the files will be put when retrieving Ellie Mae Network Service documents.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
          }
          else
            break;
          break;
        case 3:
          this.loanRights |= BizRule.LoanAccessRight.DocTracking;
          break;
      }
      switch (this.cboConditions.SelectedIndex)
      {
        case 1:
          this.loanRights |= BizRule.LoanAccessRight.ConditionTrackingViewOnly;
          break;
        case 2:
          this.loanRights |= BizRule.LoanAccessRight.ConditionTracking;
          break;
      }
      switch (this.cboConversation.SelectedIndex)
      {
        case 1:
          this.loanRights |= BizRule.LoanAccessRight.ConversationLogViewOnly;
          break;
        case 2:
          this.loanRights |= BizRule.LoanAccessRight.ConversationLog;
          break;
      }
      switch (this.cboTask.SelectedIndex)
      {
        case 1:
          this.loanRights |= BizRule.LoanAccessRight.TaskViewOnly;
          break;
        case 2:
          this.loanRights |= BizRule.LoanAccessRight.Task;
          break;
      }
      switch (this.cboProfitMgr.SelectedIndex)
      {
        case 1:
          this.loanRights |= BizRule.LoanAccessRight.ProfitMgmtViewOnly;
          break;
        case 2:
          this.loanRights |= BizRule.LoanAccessRight.ProfitMgmt;
          break;
      }
      switch (this.cboLockRequest.SelectedIndex)
      {
        case 1:
          this.loanRights |= BizRule.LoanAccessRight.LockRequestViewOnly;
          break;
        case 2:
          this.loanRights |= BizRule.LoanAccessRight.LockRequest;
          break;
      }
      switch (this.cboDisclosureTracking.SelectedIndex)
      {
        case 1:
          this.loanRights |= BizRule.LoanAccessRight.DisclosureTrackingViewOnly;
          break;
        case 2:
          this.loanRights |= BizRule.LoanAccessRight.DisclosureTracking;
          break;
      }
      this.editableFields = (string[]) null;
      if (this.gvFields.Items.Count > 0)
      {
        this.loanRights |= BizRule.LoanAccessRight.FormFields;
        ArrayList arrayList = new ArrayList();
        foreach (DictionaryEntry selectedField in this.selectedFields)
        {
          if (!(selectedField.Key.ToString() == string.Empty))
            arrayList.Add((object) selectedField.Key.ToString());
        }
        this.editableFields = (string[]) arrayList.ToArray(typeof (string));
      }
      if (this.loanRights == BizRule.LoanAccessRight.ViewAllOnly && Utils.Dialog((IWin32Window) this, "Without selecting any access rights, the 'View Only' right will be applied to this persona. Would you like to change right to 'View Only'?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
        return;
      this.DialogResult = DialogResult.OK;
    }

    private void btnAddField_Click(object sender, EventArgs e)
    {
      this.gvFields.SelectedItems.Clear();
      using (AddFields addFields = new AddFields(this.session))
      {
        addFields.OnAddMoreButtonClick += new EventHandler(this.addFieldDlg_OnAddMoreButtonClick);
        if (addFields.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.addFields(addFields.SelectedFieldIDs);
      }
    }

    private void addFields(string[] ids)
    {
      this.gvFields.BeginUpdate();
      for (int index = 0; index < ids.Length; ++index)
      {
        if (!this.selectedFields.ContainsKey((object) ids[index]))
        {
          GVItem listItem = this.createListItem(ids[index]);
          listItem.Selected = true;
          this.gvFields.Items.Add(listItem);
          this.selectedFields.Add((object) ids[index], (object) "");
        }
      }
      this.gvFields.EndUpdate();
    }

    private void btnFindField_Click(object sender, EventArgs e)
    {
      int num = 0;
      string[] existingFields = new string[this.selectedFields.Count];
      foreach (DictionaryEntry selectedField in this.selectedFields)
        existingFields[num++] = selectedField.Key.ToString();
      using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(this.session, existingFields, true, string.Empty, false, false))
      {
        if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK || ruleFindFieldDialog.SelectedRequiredFields.Length == 0)
          return;
        this.gvFields.BeginUpdate();
        foreach (string selectedRequiredField in ruleFindFieldDialog.SelectedRequiredFields)
        {
          if (!this.selectedFields.ContainsKey((object) selectedRequiredField))
          {
            GVItem listItem = this.createListItem(selectedRequiredField);
            listItem.Selected = true;
            this.gvFields.Items.Add(listItem);
            this.selectedFields.Add((object) selectedRequiredField, (object) "");
          }
        }
        this.gvFields.EndUpdate();
      }
    }

    private void btnDeleteField_Click(object sender, EventArgs e)
    {
      if (this.gvFields.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a field first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete selected fields?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
          return;
        int nItemIndex = 0;
        int num2 = 0;
        GVItem[] gvItemArray = new GVItem[this.gvFields.SelectedItems.Count];
        foreach (GVItem selectedItem in this.gvFields.SelectedItems)
        {
          gvItemArray[num2++] = selectedItem;
          nItemIndex = selectedItem.Index;
        }
        foreach (GVItem gvItem in gvItemArray)
        {
          if (this.selectedFields.ContainsKey((object) gvItem.Text))
            this.selectedFields.Remove((object) gvItem.Text);
          this.gvFields.Items.Remove(gvItem);
        }
        if (this.gvFields.Items.Count <= 0)
          return;
        if (nItemIndex >= this.gvFields.Items.Count)
          this.gvFields.Items[this.gvFields.Items.Count - 1].Selected = true;
        else
          this.gvFields.Items[nItemIndex].Selected = true;
      }
    }

    private GVItem createListItem(string id)
    {
      return new GVItem(id)
      {
        SubItems = {
          (object) EncompassFields.GetDescription(id, this.fieldSettings)
        }
      };
    }

    private void addFieldDlg_OnAddMoreButtonClick(object sender, EventArgs e)
    {
      AddFields addFields = (AddFields) sender;
      if (addFields == null)
        return;
      this.addFields(addFields.SelectedFieldIDs);
    }

    private void cboDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboDocuments.SelectedIndex == 2)
        this.gcDocumentsPartial.Visible = true;
      else
        this.gcDocumentsPartial.Visible = false;
    }

    private void LoanFileRightsDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp() => JedHelp.ShowHelp("Setup\\Persona Access to Loans");
  }
}

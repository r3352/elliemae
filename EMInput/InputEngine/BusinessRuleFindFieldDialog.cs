// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.BusinessRuleFindFieldDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientCommon;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class BusinessRuleFindFieldDialog : Form, IOnlineHelpTarget, IHelp, ILoanEditor
  {
    private const string className = "BusinessRuleFindFieldDialog";
    private static string sw = Tracing.SwInputEngine;
    private Button cancelBtn;
    private EMFormMenu emFormMenuBox;
    private System.ComponentModel.Container components;
    private InputFormList systemFormList;
    private InputFormInfo currForm;
    private LoanData dataTempLoan;
    private Button okBtn;
    private TextBox textBoxGoTo;
    private Button findNextBtn;
    private LoanScreen freeScreen;
    private Panel pnlExistingFields;
    private Panel pnlNewFields;
    private Label lblExistingFields;
    private Label lblNewFields;
    private Button buttonSelect;
    private Button buttonUnselect;
    private EMHelpLink emHelpLink1;
    private string[] existingFields;
    private bool isSingleSelection;
    private GroupContainer grpForms;
    private GradientPanel gradientPanel1;
    private Panel panel1;
    private BorderPanel borderPanel1;
    private CollapsibleSplitter collapsibleSplitter1;
    private Label label1;
    private FlowLayoutPanel flowLayoutPanel1;
    private Label lblInstructions;
    private ComboBox comboBoxAccess;
    private Label labelAccess0;
    private Panel pnlForm;
    private GradientPanel gradientPanel2;
    private ComboBox cboRuleValues;
    private Label lblRuleLabel;
    private GradientPanel gradientPanel3;
    private Panel webPanel;
    private bool enableButtonSelection;
    private Sessions.Session session;
    private TabLinksControl tabLinkCtrl;
    private Itemization2015Container regzGFE2015Control;
    private string[] selectedRequiredFields;

    public BusinessRuleFindFieldDialog(
      Sessions.Session session,
      string[] existingFields,
      bool hideAccessRight,
      string helpTag,
      bool isSingleSelection,
      bool enableButtonSelection)
    {
      this.session = session;
      this.isSingleSelection = isSingleSelection;
      this.existingFields = existingFields;
      this.enableButtonSelection = enableButtonSelection;
      this.InitializeComponent();
      this.lblRuleLabel.Left = this.labelAccess0.Left;
      this.cboRuleValues.Left = this.lblRuleLabel.Left + this.lblRuleLabel.Width + 10;
      this.cboRuleValues.Top = this.comboBoxAccess.Top;
      if (hideAccessRight)
      {
        this.comboBoxAccess.Visible = false;
        this.labelAccess0.Visible = false;
      }
      if (helpTag != null && string.Empty != helpTag)
      {
        this.emHelpLink1.HelpTag = helpTag;
        this.emHelpLink1.Visible = true;
      }
      this.pnlExistingFields.BackColor = InputHandlerBase.ExistingFieldColor;
      this.pnlNewFields.BackColor = InputHandlerBase.SelectedFieldColor;
      this.comboBoxAccess.Items.AddRange((object[]) BizRule.FieldAccessRightStrings);
      this.comboBoxAccess.Items.Insert(0, (object) "");
      this.initForm();
      this.buttonSelect.Visible = !isSingleSelection;
      this.buttonUnselect.Visible = !isSingleSelection;
      if (isSingleSelection)
      {
        this.lblInstructions.Text = "Right-click a field to select";
        this.lblExistingFields.Text = "Previously selected field";
        this.lblNewFields.Text = "Newly selected field";
        this.Text = "Find Field";
      }
      if (existingFields == null || existingFields.Length == 0)
      {
        this.lblExistingFields.Visible = this.pnlExistingFields.Visible = false;
        this.lblNewFields.Left = this.lblExistingFields.Left;
      }
      this.textBoxGoTo_TextChanged((object) null, (EventArgs) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public string[] SelectedRequiredFields => this.selectedRequiredFields;

    public void ShowAUSTrackingTool()
    {
    }

    public TriggerEmailTemplate MilestoneTemplateEmailTemplate
    {
      get => throw new Exception("The method or operation is not implemented.");
      set => throw new Exception("The method or operation is not implemented.");
    }

    public BizRule.FieldAccessRight AccessRightSelected
    {
      get
      {
        if (this.comboBoxAccess.Text == BizRule.FieldAccessRightStrings[0])
          return BizRule.FieldAccessRight.Hide;
        if (this.comboBoxAccess.Text == BizRule.FieldAccessRightStrings[1])
          return BizRule.FieldAccessRight.ViewOnly;
        return this.comboBoxAccess.Text == BizRule.FieldAccessRightStrings[2] ? BizRule.FieldAccessRight.Edit : (BizRule.FieldAccessRight) 255;
      }
    }

    public void SetMilestoneSelection(string[] milestones)
    {
      this.lblRuleLabel.Visible = this.cboRuleValues.Visible = true;
      this.cboRuleValues.Items.Clear();
      this.cboRuleValues.Items.AddRange((object[]) milestones);
    }

    public string SelectedRuleValue => this.cboRuleValues.Text;

    public string SetComboLabelText
    {
      set
      {
        if (string.IsNullOrEmpty(value))
          return;
        this.lblRuleLabel.Text = value;
        this.cboRuleValues.Left = this.lblRuleLabel.Left + this.lblRuleLabel.Width + 10;
      }
    }

    public void SetComboIndex(int indexNumber) => this.cboRuleValues.SelectedIndex = indexNumber;

    private void InitializeComponent()
    {
      this.buttonUnselect = new Button();
      this.buttonSelect = new Button();
      this.findNextBtn = new Button();
      this.textBoxGoTo = new TextBox();
      this.emFormMenuBox = new EMFormMenu();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.pnlExistingFields = new Panel();
      this.pnlNewFields = new Panel();
      this.lblExistingFields = new Label();
      this.lblNewFields = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.lblInstructions = new Label();
      this.panel1 = new Panel();
      this.cboRuleValues = new ComboBox();
      this.comboBoxAccess = new ComboBox();
      this.lblRuleLabel = new Label();
      this.labelAccess0 = new Label();
      this.borderPanel1 = new BorderPanel();
      this.pnlForm = new Panel();
      this.webPanel = new Panel();
      this.gradientPanel3 = new GradientPanel();
      this.gradientPanel2 = new GradientPanel();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.grpForms = new GroupContainer();
      this.gradientPanel1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.pnlForm.SuspendLayout();
      this.gradientPanel3.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.grpForms.SuspendLayout();
      this.SuspendLayout();
      this.buttonUnselect.Location = new Point(99, 0);
      this.buttonUnselect.Margin = new Padding(4, 0, 0, 0);
      this.buttonUnselect.Name = "buttonUnselect";
      this.buttonUnselect.Size = new Size(75, 22);
      this.buttonUnselect.TabIndex = 7;
      this.buttonUnselect.Text = "Deselect All";
      this.buttonUnselect.UseVisualStyleBackColor = true;
      this.buttonUnselect.Click += new EventHandler(this.buttonUnselect_Click);
      this.buttonSelect.Location = new Point(20, 0);
      this.buttonSelect.Margin = new Padding(4, 0, 0, 0);
      this.buttonSelect.Name = "buttonSelect";
      this.buttonSelect.Size = new Size(75, 22);
      this.buttonSelect.TabIndex = 6;
      this.buttonSelect.Text = "Select All";
      this.buttonSelect.UseVisualStyleBackColor = true;
      this.buttonSelect.Click += new EventHandler(this.buttonSelect_Click);
      this.findNextBtn.Location = new Point(210, 2);
      this.findNextBtn.Name = "findNextBtn";
      this.findNextBtn.Size = new Size(65, 22);
      this.findNextBtn.TabIndex = 5;
      this.findNextBtn.Text = "&Find";
      this.findNextBtn.Click += new EventHandler(this.findNextBtn_Click);
      this.textBoxGoTo.Location = new Point(102, 3);
      this.textBoxGoTo.Name = "textBoxGoTo";
      this.textBoxGoTo.Size = new Size(104, 20);
      this.textBoxGoTo.TabIndex = 4;
      this.textBoxGoTo.TextChanged += new EventHandler(this.textBoxGoTo_TextChanged);
      this.textBoxGoTo.KeyDown += new KeyEventHandler(this.textBoxGoTo_KeyDown);
      this.emFormMenuBox.AlternatingColors = false;
      this.emFormMenuBox.BorderStyle = BorderStyle.None;
      this.emFormMenuBox.Dock = DockStyle.Fill;
      this.emFormMenuBox.GridLines = false;
      this.emFormMenuBox.IntegralHeight = false;
      this.emFormMenuBox.Location = new Point(1, 26);
      this.emFormMenuBox.Name = "emFormMenuBox";
      this.emFormMenuBox.Size = new Size(198, 474);
      this.emFormMenuBox.TabIndex = 4;
      this.emFormMenuBox.SelectedIndexChanged += new EventHandler(this.emFormMenuList_SelectedIndexChanged);
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(798, 9);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 22);
      this.cancelBtn.TabIndex = 8;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.Location = new Point(718, 9);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 22);
      this.okBtn.TabIndex = 7;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.pnlExistingFields.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pnlExistingFields.BackColor = Color.SkyBlue;
      this.pnlExistingFields.BorderStyle = BorderStyle.FixedSingle;
      this.pnlExistingFields.Location = new Point(142, 4);
      this.pnlExistingFields.Name = "pnlExistingFields";
      this.pnlExistingFields.Size = new Size(12, 16);
      this.pnlExistingFields.TabIndex = 9;
      this.pnlNewFields.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pnlNewFields.BackColor = Color.BlanchedAlmond;
      this.pnlNewFields.BorderStyle = BorderStyle.FixedSingle;
      this.pnlNewFields.Location = new Point(297, 4);
      this.pnlNewFields.Name = "pnlNewFields";
      this.pnlNewFields.Size = new Size(12, 16);
      this.pnlNewFields.TabIndex = 10;
      this.lblExistingFields.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblExistingFields.AutoSize = true;
      this.lblExistingFields.BackColor = Color.Transparent;
      this.lblExistingFields.Location = new Point(8, 5);
      this.lblExistingFields.Name = "lblExistingFields";
      this.lblExistingFields.Size = new Size(130, 14);
      this.lblExistingFields.TabIndex = 11;
      this.lblExistingFields.Text = "Previously selected fields";
      this.lblNewFields.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblNewFields.AutoSize = true;
      this.lblNewFields.BackColor = Color.Transparent;
      this.lblNewFields.Location = new Point(176, 5);
      this.lblNewFields.Name = "lblNewFields";
      this.lblNewFields.Size = new Size(111, 14);
      this.lblNewFields.TabIndex = 12;
      this.lblNewFields.Text = "Newly selected fields";
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.Location = new Point(8, 13);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 13;
      this.emHelpLink1.Visible = false;
      this.gradientPanel1.Controls.Add((Control) this.textBoxGoTo);
      this.gradientPanel1.Controls.Add((Control) this.findNextBtn);
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(882, 26);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.TableHeader;
      this.gradientPanel1.TabIndex = 14;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(9, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(88, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Find Field by ID";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.buttonUnselect);
      this.flowLayoutPanel1.Controls.Add((Control) this.buttonSelect);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(486, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(174, 22);
      this.flowLayoutPanel1.TabIndex = 8;
      this.lblInstructions.AutoSize = true;
      this.lblInstructions.BackColor = Color.Transparent;
      this.lblInstructions.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblInstructions.Location = new Point(3, 6);
      this.lblInstructions.Margin = new Padding(3, 4, 0, 0);
      this.lblInstructions.Name = "lblInstructions";
      this.lblInstructions.Size = new Size(203, 14);
      this.lblInstructions.TabIndex = 3;
      this.lblInstructions.Text = "Right-click on fields below to select";
      this.panel1.BackColor = Color.WhiteSmoke;
      this.panel1.Controls.Add((Control) this.cboRuleValues);
      this.panel1.Controls.Add((Control) this.comboBoxAccess);
      this.panel1.Controls.Add((Control) this.cancelBtn);
      this.panel1.Controls.Add((Control) this.lblRuleLabel);
      this.panel1.Controls.Add((Control) this.labelAccess0);
      this.panel1.Controls.Add((Control) this.okBtn);
      this.panel1.Controls.Add((Control) this.emHelpLink1);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 538);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(882, 40);
      this.panel1.TabIndex = 15;
      this.cboRuleValues.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRuleValues.FormattingEnabled = true;
      this.cboRuleValues.Location = new Point(520, 15);
      this.cboRuleValues.Name = "cboRuleValues";
      this.cboRuleValues.Size = new Size(177, 22);
      this.cboRuleValues.TabIndex = 16;
      this.cboRuleValues.Visible = false;
      this.comboBoxAccess.Anchor = AnchorStyles.Top;
      this.comboBoxAccess.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxAccess.FormattingEnabled = true;
      this.comboBoxAccess.Location = new Point(472, 9);
      this.comboBoxAccess.Name = "comboBoxAccess";
      this.comboBoxAccess.Size = new Size(114, 22);
      this.comboBoxAccess.TabIndex = 14;
      this.lblRuleLabel.AutoSize = true;
      this.lblRuleLabel.BackColor = Color.Transparent;
      this.lblRuleLabel.Location = new Point(115, 13);
      this.lblRuleLabel.Name = "lblRuleLabel";
      this.lblRuleLabel.Size = new Size(71, 14);
      this.lblRuleLabel.TabIndex = 17;
      this.lblRuleLabel.Text = "For Milestone";
      this.lblRuleLabel.Visible = false;
      this.labelAccess0.Anchor = AnchorStyles.Top;
      this.labelAccess0.AutoSize = true;
      this.labelAccess0.BackColor = Color.Transparent;
      this.labelAccess0.Location = new Point(211, 12);
      this.labelAccess0.Name = "labelAccess0";
      this.labelAccess0.Size = new Size((int) byte.MaxValue, 14);
      this.labelAccess0.TabIndex = 15;
      this.labelAccess0.Text = "Set access rights to selected fields on current form";
      this.borderPanel1.BackColor = Color.WhiteSmoke;
      this.borderPanel1.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.borderPanel1.Controls.Add((Control) this.pnlForm);
      this.borderPanel1.Controls.Add((Control) this.collapsibleSplitter1);
      this.borderPanel1.Controls.Add((Control) this.grpForms);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(0, 26);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Padding = new Padding(5);
      this.borderPanel1.Size = new Size(882, 512);
      this.borderPanel1.TabIndex = 16;
      this.pnlForm.Controls.Add((Control) this.webPanel);
      this.pnlForm.Controls.Add((Control) this.gradientPanel3);
      this.pnlForm.Controls.Add((Control) this.gradientPanel2);
      this.pnlForm.Dock = DockStyle.Fill;
      this.pnlForm.Location = new Point(214, 5);
      this.pnlForm.Name = "pnlForm";
      this.pnlForm.Size = new Size(662, 501);
      this.pnlForm.TabIndex = 3;
      this.webPanel.Dock = DockStyle.Fill;
      this.webPanel.Location = new Point(0, 26);
      this.webPanel.Name = "webPanel";
      this.webPanel.Size = new Size(662, 450);
      this.webPanel.TabIndex = 3;
      this.gradientPanel3.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel3.Controls.Add((Control) this.flowLayoutPanel1);
      this.gradientPanel3.Controls.Add((Control) this.lblInstructions);
      this.gradientPanel3.Dock = DockStyle.Top;
      this.gradientPanel3.Location = new Point(0, 0);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(662, 26);
      this.gradientPanel3.TabIndex = 2;
      this.gradientPanel2.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel2.Controls.Add((Control) this.lblNewFields);
      this.gradientPanel2.Controls.Add((Control) this.lblExistingFields);
      this.gradientPanel2.Controls.Add((Control) this.pnlExistingFields);
      this.gradientPanel2.Controls.Add((Control) this.pnlNewFields);
      this.gradientPanel2.Dock = DockStyle.Bottom;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(0, 476);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(662, 25);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel2.TabIndex = 0;
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) null;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(206, 5);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 1;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Mozilla;
      this.grpForms.Controls.Add((Control) this.emFormMenuBox);
      this.grpForms.Dock = DockStyle.Left;
      this.grpForms.HeaderForeColor = SystemColors.ControlText;
      this.grpForms.Location = new Point(6, 5);
      this.grpForms.Name = "grpForms";
      this.grpForms.Size = new Size(200, 501);
      this.grpForms.TabIndex = 0;
      this.grpForms.Text = "Select a Form";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(882, 578);
      this.Controls.Add((Control) this.borderPanel1);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (BusinessRuleFindFieldDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Find Fields";
      this.Closing += new CancelEventHandler(this.BusinessRuleFindFieldDialog_Closing);
      this.Load += new EventHandler(this.BusinessRuleFindFieldDialog_Load);
      this.KeyDown += new KeyEventHandler(this.BusinessRuleFindFieldDialog_KeyDown);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.borderPanel1.ResumeLayout(false);
      this.pnlForm.ResumeLayout(false);
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.grpForms.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void initForm()
    {
      try
      {
        this.systemFormList = new InputFormList(this.session.SessionObjects, InputFormType.Standard | InputFormType.Custom);
      }
      catch (Exception ex)
      {
        Tracing.Log(BusinessRuleFindFieldDialog.sw, TraceLevel.Error, nameof (BusinessRuleFindFieldDialog), "exception in loading InputFormList: " + ex.Message);
        return;
      }
      InputFormInfo[] formList = this.systemFormList.GetFormList("DataTemplate");
      if (formList.Length == 0)
        Tracing.Log(BusinessRuleFindFieldDialog.sw, TraceLevel.Error, nameof (BusinessRuleFindFieldDialog), "exception in loading Data Template Form List in InputFormList.");
      this.emFormMenuBox.ClearFormList();
      string clientId = this.session.CompanyInfo.ClientID;
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>((IEnumerable<string>) SecurityUtil.BankerOnlyTools(this.session.MainScreen.IsUnderwriterSummaryAccessibleForBroker));
      foreach (InputFormInfo inputFormInfo in formList)
      {
        if ((this.session.EncompassEdition != EncompassEdition.Broker || !stringList2.Contains(inputFormInfo.Name)) && (this.session.StartupInfo.AllowURLA2020 || !ShipInDarkValidation.IsURLA2020Form(inputFormInfo.FormID)) && (!(inputFormInfo.FormID == "ULDD") || this.session.MainScreen.IsClientEnabledToExportFNMFRE))
        {
          if (inputFormInfo != (InputFormInfo) null && !InputFormInfo.IsChildForm(inputFormInfo.FormID))
            stringList1.Add(inputFormInfo.Name);
          if (inputFormInfo.FormID.StartsWith("HUD1ES"))
          {
            stringList1.Add("Request for Copy of Tax Return (Classic)");
            stringList1.Add("Request for Transcript of Tax (Classic)");
          }
          if (inputFormInfo.FormID == "HMDA_DENIAL")
          {
            stringList1.Add("2018 HMDA Originated/Adverse Action Loans");
            stringList1.Add("2018 HMDA Purchased Loans");
            stringList1.Add("Repurchased Loans");
          }
        }
      }
      if (this.session.EncompassEdition == EncompassEdition.Banker)
        stringList1.Add("Buttons for Business Rule");
      this.emFormMenuBox.LoadFormList(stringList1.ToArray());
      this.emFormMenuBox.SelectedIndex = 0;
    }

    private void emFormMenuList_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.OpenForm(this.emFormMenuBox.SelectedFormName);
    }

    public string GetHelpTargetName()
    {
      this.ShowHelp();
      return "";
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (BusinessRuleFindFieldDialog));
    }

    private void BusinessRuleFindFieldDialog_Closing(object sender, CancelEventArgs e)
    {
      if (this.freeScreen == null)
        return;
      this.freeScreen.Dispose();
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      this.selectedRequiredFields = this.dataTempLoan.SelectedFields;
      if (this.selectedRequiredFields.Length == 0)
      {
        if (this.isSingleSelection)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select a field by right-clicking it within a form.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more fields by right-clicking them within a form.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
      else if (this.cboRuleValues.Visible && this.cboRuleValues.Text == string.Empty && Utils.Dialog((IWin32Window) this, "Would you like to select a milestone for required fields you just selected.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
        this.cboRuleValues.Focus();
      else
        this.DialogResult = DialogResult.OK;
    }

    private void findNextBtn_Click(object sender, EventArgs e)
    {
      string str1 = this.textBoxGoTo.Text.Trim();
      string str2 = string.Empty;
      int num1 = 0;
      if (str1.Length == 6)
      {
        string str3 = str1.Substring(0, 2);
        switch (str3)
        {
          case "AR":
            str2 = str1;
            str1 = "AR00" + str1.Substring(4, 2);
            break;
          case "BE":
          case "CE":
            str2 = str1;
            str1 = "FE00" + str1.Substring(4, 2);
            break;
          case "BR":
          case "CR":
            str2 = str1;
            str1 = "FR00" + str1.Substring(4, 2);
            break;
          case "DD":
            str2 = str1;
            str1 = "DD00" + str1.Substring(4, 2);
            break;
          case "FL":
            str2 = str1;
            str1 = "FL00" + str1.Substring(4, 2);
            break;
          case "FM":
            str2 = str1;
            str1 = "FM00" + str1.Substring(4, 2);
            break;
          case "IR":
            str2 = str1;
            str1 = "IR00" + str1.Substring(4, str2.Length == 8 ? 4 : 2);
            break;
          default:
            str3 = string.Empty;
            break;
        }
        if (str3 != string.Empty)
          num1 = Utils.ParseInt((object) str2.Substring(2, 2));
      }
      ArrayList formList = new ArrayList();
      ArrayList formNames = new ArrayList((ICollection) this.emFormMenuBox.Items);
      this.addFormListForGoTo(formList, formNames, this.currForm, false);
      this.addFormListForGoTo(formList, formNames, this.currForm, true);
      InputFormInfo inputForm = new SearchFields(this.systemFormList, this.dataTempLoan).FindInputForm((string[]) formList.ToArray(typeof (string)), str1, str2);
      if (inputForm == (InputFormInfo) null)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Encompass was unable to find a form that contains field '" + str1 + "' in current form list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        int num3 = -1;
        for (int index = 0; index < formNames.Count; ++index)
        {
          string str4 = (string) formNames[index];
          if (str4 == inputForm.Name)
          {
            num3 = index;
            break;
          }
          switch (str4)
          {
            case "Custom Fields":
              if (inputForm.Name.IndexOf("Custom Fields") > -1)
              {
                num3 = index;
                goto label_25;
              }
              else
                break;
            case "Self-Employed Income 1084":
              if (inputForm.Name.IndexOf("Self-Employed Income 1084") > -1)
              {
                num3 = index;
                goto label_25;
              }
              else
                break;
          }
        }
label_25:
        if (num3 == -1)
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "Encompass was unable to find a form that contains field '" + str1 + "' in current form list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          this.findNextBtn.Text = "&Find Next";
          bool flag = false;
          string name = inputForm.Name;
          if (!(name == "Custom Fields 4") && !(name == "Self-Employed Income 1084 2") && !(name == "Custom Fields 3") && !(name == "Custom Fields 2"))
          {
            if (str2 != string.Empty)
              this.freeScreen.SetGoToFieldFocus(str2, 1);
            else
              this.freeScreen.SetGoToFieldFocus(str1, 1);
            this.emFormMenuBox.SelectedIndex = num3;
          }
          if (!flag)
            return;
          if (str2 != string.Empty)
            this.freeScreen.SetGoToFieldFocus(str2, 1);
          else
            this.freeScreen.SetGoToFieldFocus(str1, 1);
          switch (inputForm.Name)
          {
            case "Custom Fields 2":
              this.currForm = new InputFormInfo("CUSTOMFIELDS", "Custom Fields");
              this.freeScreen.SetTitle(this.currForm.Name, (Control) new QuickLinksControl((ILoanEditor) this, "CF_2", this.session));
              break;
            case "Custom Fields 3":
              this.currForm = new InputFormInfo("CUSTOMFIELDS", "Custom Fields");
              this.freeScreen.SetTitle(this.currForm.Name, (Control) new QuickLinksControl((ILoanEditor) this, "CF_3", this.session));
              break;
            case "Custom Fields 4":
              this.currForm = new InputFormInfo("CUSTOMFIELDS", "Custom Fields");
              this.freeScreen.SetTitle(this.currForm.Name, (Control) new QuickLinksControl((ILoanEditor) this, "CF_4", this.session));
              break;
            case "Self-Employed Income 1084 2":
              this.currForm = new InputFormInfo("FM1084", "Self-Employed Income 1084");
              this.freeScreen.SetTitle(this.currForm.Name, (Control) new QuickLinksControl((ILoanEditor) this, "FM1084A", this.session));
              break;
          }
        }
      }
    }

    private void addFormListForGoTo(
      ArrayList formList,
      ArrayList formNames,
      InputFormInfo currentForm,
      bool addTopList)
    {
      string empty = string.Empty;
      if (currentForm != (InputFormInfo) null)
      {
        bool flag = false;
        for (int index1 = 0; index1 < formNames.Count; ++index1)
        {
          string formName1 = (string) formNames[index1];
          if (formName1.IndexOf("----------") <= -1 && formName1 == currentForm.Name)
          {
            flag = true;
            if (!addTopList)
            {
              for (int index2 = index1 + 1; index2 < formNames.Count; ++index2)
              {
                string formName2 = (string) formNames[index2];
                if (formName2.IndexOf("----------") <= -1)
                  formList.Add((object) formName2);
              }
              break;
            }
            for (int index3 = 0; index3 < index1; ++index3)
            {
              string formName3 = (string) formNames[index3];
              if (formName3.IndexOf("----------") <= -1)
                formList.Add((object) formName3);
            }
            break;
          }
        }
        if (flag)
          return;
        for (int index = 0; index < formNames.Count; ++index)
        {
          string formName = (string) formNames[index];
          if (formName.IndexOf("----------") <= -1)
            formList.Add((object) formName);
        }
      }
      else
      {
        for (int index = 0; index < formNames.Count; ++index)
        {
          string formName = (string) formNames[index];
          if (formName.IndexOf("----------") <= -1)
            formList.Add((object) formName);
        }
      }
    }

    private void buttonSelect_Click(object sender, EventArgs e)
    {
      if (this.freeScreen != null)
        this.freeScreen.SelectAllFields();
      else if (this.tabLinkCtrl != null)
      {
        this.tabLinkCtrl.SelectAllFields();
      }
      else
      {
        if (this.regzGFE2015Control == null)
          return;
        this.regzGFE2015Control.SelectAllFields();
      }
    }

    private void buttonUnselect_Click(object sender, EventArgs e)
    {
      if (this.freeScreen != null)
        this.freeScreen.DeselectAllFields();
      else if (this.tabLinkCtrl != null)
      {
        this.tabLinkCtrl.DeselectAllFields();
      }
      else
      {
        if (this.regzGFE2015Control == null)
          return;
        this.regzGFE2015Control.DeselectAllFields();
      }
    }

    public bool SelectSettlementServiceProviders()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool SelectAffilatesTemplate()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ShowMilestoneWorksheet(MilestoneLog ms)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ShoweDisclosureTrackingRecord(string packageID)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RefreshLoan() => throw new Exception("The method or operation is not implemented.");

    public void ShowVerifPanel(string verifType)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RefreshContents()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RefreshLoanContents()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RefreshContents(string fieldId)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RefreshLogPanel()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void StartConversation(ConversationLog con)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RefreshLoanTeamMembers()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void AddMilestoneWorksheet(MilestoneLog milestoneLog)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ApplyBusinessRules()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void SetMilestoneStatus(MilestoneLog milestoneLog, int milestoneIndex, bool finished)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ClearMilestoneLogArea()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void AddToWorkArea(Control worksheet)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void AddToWorkArea(Control worksheet, bool rememberCurrentFormID)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RemoveFromWorkArea()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public DateTime AddDays(DateTime date, int dayCount)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public int MinusBusinessDays(DateTime previous, DateTime currentLog)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public string CurrentForm
    {
      get => throw new Exception("The method or operation is not implemented.");
      set => throw new Exception("The method or operation is not implemented.");
    }

    public object GetFormScreen()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public object GetVerifScreen()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void GoToField(string fieldID)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void GoToField(string fieldID, BorrowerPair targetPair)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void GoToField(string fieldID, string formName)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void GoToField(string fieldID, bool findNext)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BAMGoToField(string fieldID, bool findNext)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void GoToField(string fieldID, bool findNext, bool searchToolPageOnly)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ShowPlanCodeComparison(string fieldId, DocumentOrderType orderType)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ApplyOnDemandBusinessRules()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool OpenForm(string formOrToolName) => this.OpenForm(formOrToolName, (Control) null);

    public bool OpenForm(string formOrToolName, Control navControl)
    {
      if (string.Compare(formOrToolName, "Buttons for Business Rule", true) == 0)
        return this.OpenFormByID("Buttons for Business Rule", navControl);
      InputFormInfo inputFormInfo = this.systemFormList.GetFormByName(formOrToolName);
      if (inputFormInfo == (InputFormInfo) null && string.Compare(formOrToolName, "2018 HMDA Purchased Loans", true) == 0)
        inputFormInfo = new InputFormInfo("HMDA2018_Purchased", formOrToolName);
      else if (inputFormInfo == (InputFormInfo) null && string.Compare(formOrToolName, "2018 HMDA Originated/Adverse Action Loans", true) == 0)
        inputFormInfo = new InputFormInfo("HMDA2018_Originated", formOrToolName);
      else if (inputFormInfo == (InputFormInfo) null && string.Compare(formOrToolName, "Repurchased Loans", true) == 0)
        inputFormInfo = new InputFormInfo("HMDA2018_Repurchased", formOrToolName);
      return !(inputFormInfo == (InputFormInfo) null) && this.OpenFormByID(inputFormInfo.FormID, navControl);
    }

    public bool OpenFormByID(string formOrToolID)
    {
      return this.OpenFormByID(formOrToolID, (Control) null);
    }

    public bool OpenFormByID(string formOrToolID, Control navControl)
    {
      Cursor.Current = Cursors.WaitCursor;
      if (this.tabLinkCtrl != null && this.webPanel.Controls.Contains((Control) this.tabLinkCtrl))
      {
        this.webPanel.Controls.Remove((Control) this.tabLinkCtrl);
        this.tabLinkCtrl = (TabLinksControl) null;
      }
      if (this.regzGFE2015Control != null && this.webPanel.Controls.Contains((Control) this.regzGFE2015Control))
      {
        this.webPanel.Controls.Remove((Control) this.regzGFE2015Control);
        this.regzGFE2015Control = (Itemization2015Container) null;
      }
      this.currForm = this.systemFormList.GetForm(formOrToolID);
      if (this.currForm == (InputFormInfo) null && string.Compare(formOrToolID, "Buttons for Business Rule", true) == 0)
        this.currForm = new InputFormInfo("_ButtonsForBusinessRule", "Buttons for Business Rule");
      else if (this.currForm == (InputFormInfo) null && string.Compare(formOrToolID, "HMDA2018_Originated", true) == 0)
        this.currForm = new InputFormInfo("HMDA2018_Originated", "2018 HMDA Originated/Adverse Action Loans");
      else if (this.currForm == (InputFormInfo) null && string.Compare(formOrToolID, "HMDA2018_Purchased", true) == 0)
        this.currForm = new InputFormInfo("HMDA2018_Purchased", "2018 HMDA Purchased Loans");
      else if (this.currForm == (InputFormInfo) null && string.Compare(formOrToolID, "HMDA2018_Repurchased", true) == 0)
        this.currForm = new InputFormInfo("HMDA2018_Repurchased", "Repurchased Loans");
      else if (this.currForm.FormID == "HMDA_DENIAL")
        this.currForm = new InputFormInfo("HMDA_DENIAL04", this.currForm.MnemonicName);
      else if (this.currForm.FormID == "SETTLEMENTSERVICELIST")
        this.currForm = new InputFormInfo("SettlementServiceProvider", "Settlement Service Provider");
      if (!this.webPanel.Controls.Contains((Control) this.freeScreen))
      {
        if (this.dataTempLoan == null)
        {
          string empty = string.Empty;
          string xmlData;
          try
          {
            FileStream fileStream = File.OpenRead(AssemblyResolver.GetResourceFileFullPath("Documents\\Templates\\BlankLoan\\BlankData.XML", SystemSettings.LocalAppDir));
            byte[] numArray = new byte[fileStream.Length];
            fileStream.Read(numArray, 0, numArray.Length);
            fileStream.Close();
            xmlData = Encoding.ASCII.GetString(numArray);
          }
          catch (Exception ex)
          {
            Tracing.Log(BusinessRuleFindFieldDialog.sw, TraceLevel.Error, nameof (BusinessRuleFindFieldDialog), "Error opening BlankLoan template. Message: " + ex.Message);
            return false;
          }
          this.dataTempLoan = new LoanData(xmlData, this.session.LoanManager.GetLoanSettings(), false);
          this.dataTempLoan.IsTemplate = true;
          this.dataTempLoan.IsInFindFieldForm = true;
          this.dataTempLoan.IsSingleFieldSelection = this.isSingleSelection;
          this.dataTempLoan.ButtonSelectionEnabled = this.enableButtonSelection;
          this.dataTempLoan.ClearSelectedFields();
          if (this.existingFields != null)
          {
            for (int index = 0; index < this.existingFields.Length; ++index)
              this.dataTempLoan.AddSelectedField(this.existingFields[index], LoanData.FindFieldTypes.Existing);
          }
        }
        this.freeScreen = new LoanScreen(this.session, (IWin32Window) this, (IHtmlInput) this.dataTempLoan);
        this.freeScreen.SetHelpTarget((IOnlineHelpTarget) this);
        this.freeScreen.HideFormTitle();
        this.webPanel.Controls.Add((Control) this.freeScreen);
      }
      if (string.Compare(formOrToolID, "HMDA Information (2018)", true) == 0 || string.Compare(formOrToolID, "HMDA2018_Purchased", true) == 0 || string.Compare(formOrToolID, "HMDA2018_Originated", true) == 0)
        this.dataTempLoan.SetField("HMDA.X27", "2018");
      else if (string.Compare(formOrToolID, "HMDA_DENIAL", true) == 0)
        this.dataTempLoan.SetField("HMDA.X27", "2017");
      if (this.currForm.Name == "Self-Employed Income 1084")
      {
        this.currForm = new InputFormInfo("FM1084A", "108&4A Cash Analysis");
        this.freeScreen.SetTitle(this.currForm.Name, (Control) new QuickLinksControl((ILoanEditor) this, this.currForm.FormID, this.session));
      }
      else if (this.currForm.Name == "Custom Fields")
        this.freeScreen.SetTitle(this.currForm.Name, (Control) new QuickLinksControl((ILoanEditor) this, this.currForm.FormID, this.session));
      else if (this.currForm.Name == "Borrower Information - Vesting")
        this.freeScreen.LoadWindowsForm(this.currForm.FormID);
      else if (this.currForm.Name == "File Contacts")
      {
        this.freeScreen.LoadWindowsForm(this.currForm.FormID);
        this.emFormMenuBox.Focus();
      }
      else if (TabLinksControl.UseTabLinks(this.session, this.currForm, this.dataTempLoan))
      {
        if (this.freeScreen != null)
        {
          this.webPanel.Controls.Remove((Control) this.freeScreen);
          this.freeScreen = (LoanScreen) null;
        }
        this.tabLinkCtrl = new TabLinksControl(this.session, this.currForm, (IWin32Window) this, this.dataTempLoan);
        this.webPanel.Controls.Add((Control) this.tabLinkCtrl);
      }
      else if (this.currForm.Name == "2015 Itemization")
      {
        if (this.freeScreen != null)
        {
          this.webPanel.Controls.Remove((Control) this.freeScreen);
          this.freeScreen = (LoanScreen) null;
        }
        this.regzGFE2015Control = new Itemization2015Container(this.session, (IWin32Window) this, this.dataTempLoan);
        this.webPanel.Controls.Add((Control) this.regzGFE2015Control);
      }
      else
      {
        this.freeScreen.SetTitle(this.currForm.Name);
        this.freeScreen.LoadForm(this.currForm);
      }
      if (this.freeScreen != null)
        this.freeScreen.HideFormTitle();
      this.comboBoxAccess.Text = "";
      return true;
    }

    public bool OpenLogRecord(LogRecordBase rec)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void OpenMilestoneLogReview(MilestoneLog log)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void OpenMilestoneLogReview(MilestoneLog log, MilestoneHistoryLog historyLog)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void SendLockRequest(bool closeFile)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void PromptCreateNewLogRecord()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool IsPrimaryEditor => false;

    public bool GetInputEngineService(LoanData loan, InputEngineServiceType serviceType)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool GetInputEngineService(LoanDataMgr loanDataMgr, InputEngineServiceType serviceType)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool Print(string[] formNames)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool ShowRegulationAlerts()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool ShowRegulationAlertsOrderDoc()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void SaveLoan() => throw new Exception("The method or operation is not implemented.");

    private void BusinessRuleFindFieldDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.cancelBtn.PerformClick();
    }

    private void BusinessRuleFindFieldDialog_Load(object sender, EventArgs e)
    {
      this.pnlExistingFields.Left = this.lblExistingFields.Right;
      this.pnlNewFields.Left = this.lblNewFields.Right;
    }

    private void textBoxGoTo_KeyDown(object sender, KeyEventArgs e)
    {
      if (sender == null || e.KeyCode != Keys.Return)
        return;
      this.findNextBtn_Click(sender, (EventArgs) null);
    }

    private void textBoxGoTo_TextChanged(object sender, EventArgs e)
    {
      this.findNextBtn.Enabled = this.textBoxGoTo.Text.Trim().Length > 0;
      this.findNextBtn.Text = "&Find";
    }

    public void ShoweDisclosureTrackingRecord(
      DisclosureTrackingBase selectedLog,
      bool clearNotification)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public string[] SelectLinkAndSyncTemplate()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ShowAIQAnalyzerMessage(
      string analyzerType,
      DateTime alertDateTime,
      string description,
      string messageID)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void LaunchAIQIncomeAnalyzer()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    DialogResult ILoanEditor.OpenModal(string openModalOptions)
    {
      throw new NotImplementedException();
    }

    public void RedirectToUrl(string targetName) => throw new NotImplementedException();
  }
}

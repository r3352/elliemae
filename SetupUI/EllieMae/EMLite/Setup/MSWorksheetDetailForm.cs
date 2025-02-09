// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MSWorksheetDetailForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class MSWorksheetDetailForm : Form
  {
    private const string className = "MSWorksheetDetailForm";
    private Sessions.Session session;
    private RoleSummaryInfo origRole;
    private bool fromNoRoleChangeToRoleChange;
    private static readonly string sw = Tracing.SwOutsideLoan;
    private Label label3;
    private Button buttonSave;
    private Button buttonCancel;
    private CheckBox checkBoxPrior;
    private GroupBox groupBox3;
    private Label label1;
    private Label label9;
    private Label label10;
    private TextBox boxMilestoneName;
    private TextBox boxDays;
    private System.ComponentModel.Container components;
    private string originalMSName = string.Empty;
    private ComboBox boxRole;
    private MilestoneSetup logInst;
    private Label label4;
    private TextBox boxPreceding;
    private Label label6;
    private EMHelpLink emHelpLink1;
    private Label label2;
    private Label label7;
    private Label label5;
    private TextBox txtBoxCurrentRole;
    private TextBox txtBoxPreviousRole;
    private TextBox txtBoxForm;
    private RoleInfo[] roleInfos;
    private StandardIconButton pictureBoxForm;
    private string selectedFormID;
    private string msName = string.Empty;
    private int msDays;
    private WorksheetInfo wkinfo;

    public bool FromNoRoleChangeToRoleChange => this.fromNoRoleChangeToRoleChange;

    public MSWorksheetDetailForm(
      Sessions.Session session,
      string msName,
      WorksheetInfo wkinfo,
      MilestoneSetup logInst,
      string prevMsName,
      string prevRole,
      string currRole)
    {
      this.session = session;
      this.originalMSName = msName;
      this.logInst = logInst;
      this.wkinfo = wkinfo;
      this.origRole = this.wkinfo.Role;
      this.InitializeComponent();
      this.initForm(prevMsName, prevRole, currRole);
      if (this.session.EncompassEdition != EncompassEdition.Broker)
        return;
      this.groupBox3.Visible = false;
      this.Size = new Size(this.Width, this.Height - this.groupBox3.Height);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public string MSName => this.msName;

    public int MSDays => this.msDays;

    public WorksheetInfo Wkinfo => this.wkinfo;

    private void InitializeComponent()
    {
      this.label3 = new Label();
      this.checkBoxPrior = new CheckBox();
      this.buttonSave = new Button();
      this.buttonCancel = new Button();
      this.boxRole = new ComboBox();
      this.groupBox3 = new GroupBox();
      this.label7 = new Label();
      this.label5 = new Label();
      this.txtBoxCurrentRole = new TextBox();
      this.txtBoxPreviousRole = new TextBox();
      this.txtBoxForm = new TextBox();
      this.label2 = new Label();
      this.label4 = new Label();
      this.label1 = new Label();
      this.label9 = new Label();
      this.boxMilestoneName = new TextBox();
      this.boxDays = new TextBox();
      this.label10 = new Label();
      this.boxPreceding = new TextBox();
      this.label6 = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.pictureBoxForm = new StandardIconButton();
      this.groupBox3.SuspendLayout();
      ((ISupportInitialize) this.pictureBoxForm).BeginInit();
      this.SuspendLayout();
      this.label3.AutoSize = true;
      this.label3.Location = new Point(9, 80);
      this.label3.Name = "label3";
      this.label3.Size = new Size(54, 13);
      this.label3.TabIndex = 37;
      this.label3.Text = "Next Role";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.checkBoxPrior.Location = new Point(112, 104);
      this.checkBoxPrior.Name = "checkBoxPrior";
      this.checkBoxPrior.Size = new Size(356, 28);
      this.checkBoxPrior.TabIndex = 10;
      this.checkBoxPrior.Text = "Assigning a loan team member for the next milestone is required.";
      this.buttonSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.buttonSave.Location = new Point(388, 275);
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.Size = new Size(75, 24);
      this.buttonSave.TabIndex = 12;
      this.buttonSave.Text = "&Save";
      this.buttonSave.Click += new EventHandler(this.buttonSave_Click);
      this.buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(472, 275);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(75, 24);
      this.buttonCancel.TabIndex = 13;
      this.buttonCancel.Text = "&Cancel";
      this.boxRole.DropDownStyle = ComboBoxStyle.DropDownList;
      this.boxRole.Location = new Point(112, 77);
      this.boxRole.MaxLength = 20;
      this.boxRole.Name = "boxRole";
      this.boxRole.Size = new Size(212, 21);
      this.boxRole.TabIndex = 5;
      this.boxRole.SelectedIndexChanged += new EventHandler(this.boxRole_SelectedIndexChanged);
      this.groupBox3.Controls.Add((Control) this.pictureBoxForm);
      this.groupBox3.Controls.Add((Control) this.label7);
      this.groupBox3.Controls.Add((Control) this.label5);
      this.groupBox3.Controls.Add((Control) this.txtBoxCurrentRole);
      this.groupBox3.Controls.Add((Control) this.txtBoxPreviousRole);
      this.groupBox3.Controls.Add((Control) this.txtBoxForm);
      this.groupBox3.Controls.Add((Control) this.label2);
      this.groupBox3.Controls.Add((Control) this.label4);
      this.groupBox3.Controls.Add((Control) this.boxRole);
      this.groupBox3.Controls.Add((Control) this.checkBoxPrior);
      this.groupBox3.Controls.Add((Control) this.label3);
      this.groupBox3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.groupBox3.Location = new Point(12, 100);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(536, 167);
      this.groupBox3.TabIndex = 2;
      this.groupBox3.TabStop = false;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(9, 50);
      this.label7.Name = "label7";
      this.label7.Size = new Size(66, 13);
      this.label7.TabIndex = 51;
      this.label7.Text = "Current Role";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(9, 23);
      this.label5.Name = "label5";
      this.label5.Size = new Size(73, 13);
      this.label5.TabIndex = 50;
      this.label5.Text = "Previous Role";
      this.txtBoxCurrentRole.Location = new Point(112, 47);
      this.txtBoxCurrentRole.Name = "txtBoxCurrentRole";
      this.txtBoxCurrentRole.ReadOnly = true;
      this.txtBoxCurrentRole.Size = new Size(212, 20);
      this.txtBoxCurrentRole.TabIndex = 49;
      this.txtBoxPreviousRole.Location = new Point(112, 20);
      this.txtBoxPreviousRole.Name = "txtBoxPreviousRole";
      this.txtBoxPreviousRole.ReadOnly = true;
      this.txtBoxPreviousRole.Size = new Size(212, 20);
      this.txtBoxPreviousRole.TabIndex = 48;
      this.txtBoxForm.Location = new Point(162, 139);
      this.txtBoxForm.Name = "txtBoxForm";
      this.txtBoxForm.ReadOnly = true;
      this.txtBoxForm.Size = new Size(327, 20);
      this.txtBoxForm.TabIndex = 47;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(9, 139);
      this.label2.Name = "label2";
      this.label2.Size = new Size(147, 13);
      this.label2.TabIndex = 46;
      this.label2.Text = "Assign Form to Field Summary";
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(9, -1);
      this.label4.Name = "label4";
      this.label4.Size = new Size(126, 13);
      this.label4.TabIndex = 45;
      this.label4.Text = "Milestone Worksheet";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 43);
      this.label1.Name = "label1";
      this.label1.Size = new Size(83, 13);
      this.label1.TabIndex = 51;
      this.label1.Text = "Milestone Name";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(12, 71);
      this.label9.Name = "label9";
      this.label9.Size = new Size(73, 13);
      this.label9.TabIndex = 52;
      this.label9.Text = "Days to Finish";
      this.label9.TextAlign = ContentAlignment.MiddleLeft;
      this.boxMilestoneName.Location = new Point(124, 43);
      this.boxMilestoneName.MaxLength = 128;
      this.boxMilestoneName.Name = "boxMilestoneName";
      this.boxMilestoneName.Size = new Size(212, 20);
      this.boxMilestoneName.TabIndex = 0;
      this.boxDays.Location = new Point(124, 69);
      this.boxDays.MaxLength = 128;
      this.boxDays.Name = "boxDays";
      this.boxDays.Size = new Size(72, 20);
      this.boxDays.TabIndex = 1;
      this.boxDays.KeyPress += new KeyPressEventHandler(this.boxDays_KeyPress);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(344, 45);
      this.label10.Name = "label10";
      this.label10.Size = new Size(193, 13);
      this.label10.TabIndex = 55;
      this.label10.Text = "(less than 15 characters recommended)";
      this.label10.TextAlign = ContentAlignment.MiddleLeft;
      this.boxPreceding.Location = new Point(124, 17);
      this.boxPreceding.MaxLength = 128;
      this.boxPreceding.Name = "boxPreceding";
      this.boxPreceding.ReadOnly = true;
      this.boxPreceding.Size = new Size(212, 20);
      this.boxPreceding.TabIndex = 56;
      this.boxPreceding.TabStop = false;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(12, 17);
      this.label6.Name = "label6";
      this.label6.Size = new Size(103, 13);
      this.label6.TabIndex = 57;
      this.label6.Text = "Preceding Milestone";
      this.label6.TextAlign = ContentAlignment.MiddleLeft;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "MilestoneDialog";
      this.emHelpLink1.Location = new Point(9, 279);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 58;
      this.pictureBoxForm.BackColor = Color.Transparent;
      this.pictureBoxForm.Location = new Point(495, 139);
      this.pictureBoxForm.Name = "pictureBoxForm";
      this.pictureBoxForm.Size = new Size(16, 16);
      this.pictureBoxForm.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.pictureBoxForm.TabIndex = 53;
      this.pictureBoxForm.TabStop = false;
      this.pictureBoxForm.Click += new EventHandler(this.pictureBoxForm_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(558, 306);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.boxPreceding);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.boxDays);
      this.Controls.Add((Control) this.boxMilestoneName);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.groupBox3);
      this.Controls.Add((Control) this.buttonSave);
      this.Controls.Add((Control) this.buttonCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MSWorksheetDetailForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Create/Edit Milestone";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      ((ISupportInitialize) this.pictureBoxForm).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void initForm(string prevMsName, string prevRole, string currRole)
    {
      this.boxPreceding.Text = prevMsName;
      if (string.Compare(this.originalMSName, "Started", true) == 0)
      {
        this.checkBoxPrior.Checked = false;
        this.checkBoxPrior.Enabled = false;
      }
      else
        this.checkBoxPrior.Checked = this.wkinfo.SetRoleFirst;
      WorksheetInfo[] worksheetInfoArray = (WorksheetInfo[]) null;
      try
      {
        WorkflowManager bpmManager = (WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow);
        this.roleInfos = bpmManager.GetAllRoleFunctions();
        worksheetInfoArray = bpmManager.GetMsWorksheetInfos();
      }
      catch (Exception ex)
      {
        Tracing.Log(MSWorksheetDetailForm.sw, TraceLevel.Error, nameof (MSWorksheetDetailForm), "initForm: can't get all roles. Error: " + ex.Message);
      }
      this.boxRole.Items.Clear();
      this.boxRole.Items.Add((object) "<no change>");
      this.boxRole.SelectedIndex = 0;
      if (this.roleInfos != null)
      {
        foreach (RoleInfo roleInfo in this.roleInfos)
        {
          this.boxRole.Items.Add((object) roleInfo.RoleName);
          if (this.wkinfo.Role != null && this.wkinfo.Role.RoleID == roleInfo.RoleID)
            this.boxRole.SelectedIndex = this.boxRole.Items.Count - 1;
        }
      }
      if (!this.Wkinfo.IsMsArchived)
      {
        if (this.wkinfo.MilestoneID == "1")
        {
          this.txtBoxPreviousRole.Text = "<N/A>";
          this.txtBoxCurrentRole.Text = "<N/A>";
        }
        else
        {
          prevRole = (prevRole ?? "").Trim();
          currRole = (currRole ?? "").Trim();
          this.txtBoxPreviousRole.Text = prevRole != "" ? prevRole : "<no change from previous milestone>";
          this.txtBoxCurrentRole.Text = currRole != "" ? currRole : "<no change from previous milestone>";
        }
      }
      this.boxMilestoneName.Text = this.originalMSName;
      if (this.Wkinfo.IsCoreMilestone)
      {
        this.boxMilestoneName.ReadOnly = true;
        this.boxMilestoneName.TabStop = false;
      }
      if (string.Compare(this.originalMSName, "Started", true) == 0)
      {
        this.boxDays.ReadOnly = true;
        this.boxDays.TabStop = false;
      }
      else if (this.originalMSName != string.Empty)
      {
        if (this.Wkinfo.IsCoreMilestone)
          this.boxDays.Text = this.logInst.GetDays(Milestone.Stages[this.wkinfo.CoreMilestoneID - 1]).ToString();
        else
          this.boxDays.Text = this.logInst.GetDays(this.originalMSName).ToString();
      }
      else
        this.boxDays.Text = "0";
      if (string.Compare(this.originalMSName, "Completion", true) == 0)
      {
        this.boxRole.SelectedIndex = 0;
        this.boxRole.Enabled = false;
      }
      this.txtBoxForm.Text = "";
      if (!(this.Wkinfo.FieldSummaryForm != (InputFormInfo) null))
        return;
      InputFormInfo formInfo = this.session.FormManager.GetFormInfo(this.Wkinfo.FieldSummaryForm.FormID);
      if (!(formInfo != (InputFormInfo) null))
        return;
      this.txtBoxForm.Text = formInfo.Name;
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      if (this.boxDays.Text == string.Empty)
        this.boxDays.Text = "0";
      if (!this.wkinfo.IsCoreMilestone)
      {
        if (this.boxMilestoneName.Text.Trim() == string.Empty)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You have to enter the name of the milestone.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.boxMilestoneName.Focus();
          return;
        }
        if (this.boxMilestoneName.Text.Trim().Length > 15 && Utils.Dialog((IWin32Window) this, "To prevent characters being cut off on some screens, you may want to limit the length of milestone name to 15 characters. Do you still want to use the name you entered?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
        {
          this.boxMilestoneName.Focus();
          return;
        }
        if (string.Compare(this.originalMSName, this.boxMilestoneName.Text.Trim(), true) != 0 && this.logInst.IsMilestoneNameExists(this.boxMilestoneName.Text.Trim()))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The milestone name already exists.  Please enter a different name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.boxMilestoneName.Focus();
          return;
        }
      }
      RoleInfo role = (RoleInfo) null;
      if (this.boxRole.SelectedIndex > 0)
        role = this.findRoleInfo(this.boxRole.Text.Trim());
      string alertMessage = this.wkinfo.AlertMessage;
      if ((alertMessage ?? "").Trim() == "")
      {
        if (this.wkinfo.IsCoreMilestone && this.wkinfo.CoreMilestoneID == 1)
          alertMessage = "Loan has been started.";
        else if (this.wkinfo.Role != null)
          alertMessage = "Loan has been sent to " + this.wkinfo.Role.RoleName + ".";
      }
      if (this.origRole == null && role != null)
        this.fromNoRoleChangeToRoleChange = true;
      this.wkinfo = new WorksheetInfo(this.wkinfo.MilestoneID, (RoleSummaryInfo) role, this.checkBoxPrior.Checked, alertMessage, false, new InputFormInfo(this.selectedFormID, this.txtBoxForm.Text));
      this.msName = this.boxMilestoneName.Text.Trim();
      this.msDays = Utils.ParseInt((object) this.boxDays.Text.Trim());
      this.DialogResult = DialogResult.OK;
    }

    private RoleInfo findRoleInfo(string roleName)
    {
      if (this.roleInfos != null)
      {
        foreach (RoleInfo roleInfo in this.roleInfos)
        {
          if (roleName == roleInfo.RoleName)
            return roleInfo;
        }
      }
      return (RoleInfo) null;
    }

    private void boxDays_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar))
        e.Handled = false;
      else
        e.Handled = true;
    }

    private void boxRole_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (string.Compare(this.originalMSName, "Started", true) == 0)
        return;
      this.checkBoxPrior.Enabled = this.boxRole.SelectedIndex != 0;
      if (!this.checkBoxPrior.Enabled)
        this.checkBoxPrior.Checked = false;
      else
        this.checkBoxPrior.Checked = this.wkinfo.SetRoleFirst;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "MilestoneDialog");
    }

    private void pictureBoxForm_Click(object sender, EventArgs e)
    {
      InputFormSelector inputFormSelector = new InputFormSelector(this.session);
      if (inputFormSelector.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        return;
      this.selectedFormID = inputFormSelector.SelectedFormID;
      this.txtBoxForm.Text = inputFormSelector.SelectedFormName;
    }
  }
}

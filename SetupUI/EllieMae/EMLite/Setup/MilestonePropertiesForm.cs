// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MilestonePropertiesForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class MilestonePropertiesForm : Form
  {
    private const string className = "MilestonePropertiesForm";
    private static readonly string sw = Tracing.SwCommon;
    private bool edit;
    private bool tpoSiteExists;
    private Sessions.Session session;
    private EllieMae.EMLite.Workflow.Milestone currentMilestone;
    private string selectedFormID;
    private IContainer components;
    private Panel panel1;
    private Button btnCancel;
    private Label label4;
    private Label label3;
    private Label label2;
    private Label label1;
    private GradientIcon icoColor;
    private IconButton btnPickColor;
    private ColorDialog dlgColor;
    private ComboBox cboRoles;
    private Label label5;
    private Button btnSave;
    private Label lblMilestone;
    private StandardIconButton btnSelect;
    private TextBox txtFieldSummaryForm;
    private CheckBox chkRoleRequired;
    private TextBox txtDisplayBefore;
    private TextBox txtDisplayAfter;
    private TextBox txtDays;
    private Label lblDays;
    private EMHelpLink emHelpLink1;
    private Label label9;
    private Label label8;
    private Label label7;
    private Label label6;
    private Label label10;
    private TextBox txtName;
    private TextBox txtTPOConnectStatus;
    private TextBox txtConsumerStatus;
    private Label lblConsumerStatus;

    public MilestonePropertiesForm(Sessions.Session session, EllieMae.EMLite.Workflow.Milestone ms)
    {
      this.session = session;
      this.currentMilestone = ms;
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(this.session);
      this.loadRoles();
      this.icoColor.PrimaryColor = Color.Blue;
      this.txtDays.Text = 0.ToString();
      if (this.currentMilestone != null)
      {
        this.loadMilestone();
        this.edit = true;
      }
      else
      {
        this.edit = false;
        this.txtDisplayBefore.Text = "Before milestone is finished";
        this.txtDisplayAfter.Text = "After milestone is finished";
        this.txtDisplayAfter.ForeColor = this.txtDisplayBefore.ForeColor = Color.Gray;
      }
      if (this.txtName.Text == "Started" || this.txtName.Text == "Completion")
        this.txtName.Enabled = false;
      if (this.session != null && this.session.ConfigurationManager != null)
      {
        this.tpoSiteExists = this.session.ConfigurationManager.CheckIfAnyTPOSiteExists();
        this.label10.Visible = this.tpoSiteExists;
        this.txtTPOConnectStatus.Visible = this.tpoSiteExists;
      }
      if (!this.txtName.Text.Equals(""))
        return;
      this.btnSave.Enabled = false;
    }

    private void loadRoles()
    {
      RoleInfo[] allRoleFunctions = this.session.WorkflowManager.GetAllRoleFunctions();
      this.cboRoles.Items.Clear();
      this.cboRoles.Items.Add((object) "");
      ((IEnumerable<RoleInfo>) allRoleFunctions).ToList<RoleInfo>().ForEach((Action<RoleInfo>) (role => this.cboRoles.Items.Add((object) role)));
      this.cboRoles.SelectedIndex = 0;
    }

    private void loadMilestone()
    {
      this.txtName.Text = this.currentMilestone.Name;
      this.txtTPOConnectStatus.Text = this.currentMilestone.TPOConnectStatus;
      this.txtConsumerStatus.Text = this.currentMilestone.ConsumerStatus;
      this.icoColor.PrimaryColor = this.currentMilestone.DisplayColor;
      this.cboRoles.SelectedIndex = 0;
      if (this.currentMilestone.RoleID >= 0)
      {
        foreach (object obj in this.cboRoles.Items)
        {
          if (obj is RoleInfo && ((RoleSummaryInfo) obj).RoleID == this.currentMilestone.RoleID)
          {
            this.cboRoles.SelectedItem = obj;
            break;
          }
        }
      }
      if (!this.currentMilestone.SummaryFormID.Equals(""))
      {
        InputFormInfo formInfo = this.session.FormManager.GetFormInfo(this.currentMilestone.SummaryFormID);
        if (formInfo != (InputFormInfo) null)
          this.txtFieldSummaryForm.Text = formInfo.Name;
      }
      if (this.currentMilestone.MilestoneID == "1")
      {
        this.cboRoles.Enabled = false;
        this.txtDisplayBefore.Enabled = false;
        this.chkRoleRequired.Enabled = false;
        this.txtDays.Enabled = false;
        this.btnSelect.Enabled = false;
      }
      this.chkRoleRequired.Checked = this.currentMilestone.RoleRequired;
      if (this.currentMilestone.DescTextBefore != "")
        this.txtDisplayBefore.Text = this.currentMilestone.DescTextBefore;
      if (this.currentMilestone.DescTextAfter != "")
      {
        this.txtDisplayAfter.Text = this.currentMilestone.DescTextAfter;
      }
      else
      {
        this.txtDisplayBefore.Text = this.txtName.Text.Trim() + " expected";
        this.txtDisplayAfter.Text = this.txtName.Text.Trim() + " finished";
      }
      this.txtDays.Text = this.currentMilestone.DefaultDays.ToString();
    }

    private void btnPickColor_Click(object sender, EventArgs e)
    {
      if (this.dlgColor.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.icoColor.PrimaryColor = this.dlgColor.Color;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.validateChanges())
        return;
      try
      {
        this.saveChanges();
        this.DialogResult = DialogResult.OK;
      }
      catch (Exception ex)
      {
        Tracing.Log(MilestonePropertiesForm.sw, nameof (MilestonePropertiesForm), TraceLevel.Error, "Error saving milestone: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "Error saving milestone: " + ex.Message);
      }
    }

    private bool validateChanges()
    {
      string msName = this.txtName.Text.Trim();
      if (msName == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must provide a name for this milestone.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.txtName.Focus();
        return false;
      }
      if (msName.Trim().Length > 15 && Utils.Dialog((IWin32Window) this, "To prevent characters being cut off on some screens, you may want to limit the length of milestone name to 15 characters. Do you still want to use the name you entered?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
      {
        this.txtName.Focus();
        return false;
      }
      if (((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllMilestonesList().FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((Func<EllieMae.EMLite.Workflow.Milestone, bool>) (x => string.Compare(x.Name, msName, true) == 0 && !x.Equals((object) this.currentMilestone))) != null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A milestone with the name '" + msName + "' already exists. You must specify a unique name for this milestone.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.txtName.Focus();
        return false;
      }
      if (!this.txtDisplayBefore.Text.Equals("") && !this.txtDisplayAfter.Text.Equals(""))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "You need to give description text for before the milestone starts as well as after the milestone completes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      this.txtDisplayBefore.Text = this.txtDisplayBefore.Text.Equals("") ? this.txtName.Text.Trim() + " expected" : this.txtDisplayBefore.Text;
      this.txtDisplayAfter.Text = this.txtDisplayAfter.Text.Equals("") ? this.txtName.Text.Trim() + " finished" : this.txtDisplayAfter.Text;
      this.txtDisplayBefore.Focus();
      return false;
    }

    private void saveChanges()
    {
      bool flag = this.currentMilestone == null;
      if (this.currentMilestone == null)
        this.currentMilestone = new EllieMae.EMLite.Workflow.Milestone(this.txtName.Text.Trim());
      else
        this.currentMilestone.Name = this.txtName.Text.Trim();
      this.currentMilestone.DisplayColor = this.icoColor.PrimaryColor;
      this.currentMilestone.TPOConnectStatus = this.txtTPOConnectStatus.Text.Trim();
      this.currentMilestone.ConsumerStatus = this.txtConsumerStatus.Text.Trim();
      RoleInfo selectedItem = this.cboRoles.SelectedItem as RoleInfo;
      this.currentMilestone.RoleID = selectedItem == null ? -1 : selectedItem.RoleID;
      if (this.selectedFormID != null)
        this.currentMilestone.SummaryFormID = this.selectedFormID;
      else if (this.txtFieldSummaryForm.Text == "" && this.currentMilestone.SummaryFormID != "")
        this.currentMilestone.SummaryFormID = "";
      this.currentMilestone.RoleRequired = this.chkRoleRequired.Checked;
      this.currentMilestone.DescTextBefore = this.txtDisplayBefore.Text.Trim();
      this.currentMilestone.DescTextAfter = this.txtDisplayAfter.Text.Trim();
      this.currentMilestone.DefaultDays = Convert.ToInt32(this.txtDays.Text);
      if (flag)
      {
        this.session.SessionObjects.BpmManager.CreateMilestone(this.currentMilestone);
        MilestonesAclManager aclManager = (MilestonesAclManager) this.session.ACL.GetAclManager(AclCategory.Milestones);
        foreach (Persona allPersona in this.session.PersonaManager.GetAllPersonas())
        {
          if (allPersona.AclFeaturesDefault)
            aclManager.SynchronizePersonaSettingWithNewMilestone(allPersona.ID, allPersona.AclFeaturesDefault);
        }
      }
      else
        this.session.SessionObjects.BpmManager.UpdateMilestone(this.currentMilestone);
    }

    private void txtName_TextChanged(object sender, EventArgs e)
    {
      if (!this.edit)
      {
        this.txtDisplayBefore.Text = this.txtName.Text.Trim() + " expected";
        this.txtDisplayAfter.Text = this.txtName.Text.Trim() + " finished";
        this.txtDisplayAfter.ForeColor = this.txtDisplayBefore.ForeColor = SystemColors.WindowText;
      }
      this.btnSave.Enabled = !this.txtName.Text.Trim().Equals("") && !this.txtDisplayBefore.Text.Trim().Equals("") && !this.txtDisplayAfter.Text.Trim().Equals("");
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      InputFormSelector inputFormSelector = new InputFormSelector(this.session);
      if (inputFormSelector.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        return;
      this.selectedFormID = inputFormSelector.SelectedFormID;
      this.txtFieldSummaryForm.Text = inputFormSelector.SelectedFormName;
    }

    private void txtDays_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar))
      {
        if (!(this.txtDays.Text != "") || this.txtDays.Text.Length != 1)
          return;
        if (Convert.ToInt32(this.txtDays.Text) == 0)
          this.txtDays.Text = "";
        else
          this.txtDays.Text = Convert.ToInt32(this.txtDays.Text).ToString();
      }
      else
      {
        if (char.IsControl(e.KeyChar))
          return;
        e.Handled = true;
      }
    }

    private void txtDays_Leave(object sender, EventArgs e)
    {
      if (this.txtDays.Text == "")
      {
        this.txtDays.Text = "0";
      }
      else
      {
        if (Convert.ToInt32(this.txtDays.Text) <= 999)
          return;
        int num = (int) Utils.Dialog((IWin32Window) this, "You can't assign days to be higher than 999.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtDays.Text = "999";
      }
    }

    private void txtDisplayBefore_Enter(object sender, EventArgs e)
    {
      if (this.txtDisplayBefore.ForeColor == SystemColors.WindowText)
        return;
      this.txtDisplayBefore.Text = "";
      this.txtDisplayBefore.ForeColor = SystemColors.WindowText;
    }

    private void txtDisplayAfter_Enter(object sender, EventArgs e)
    {
      if (this.txtDisplayAfter.ForeColor == SystemColors.WindowText)
        return;
      this.txtDisplayAfter.Text = "";
      this.txtDisplayAfter.ForeColor = SystemColors.WindowText;
    }

    private void txtDisplay_TextChanged(object sender, EventArgs e)
    {
      this.btnSave.Enabled = !this.txtName.Text.Trim().Equals("") && !this.txtDisplayBefore.Text.Trim().Equals("") && !this.txtDisplayAfter.Text.Trim().Equals("");
    }

    private void txtName_Leave(object sender, EventArgs e)
    {
      if (!this.edit)
        return;
      string text = "When renaming a milestone, please note the following: \n\n" + "• Renaming a milestone affects its name only– all other characteristics remain the same. \n" + "• The milestone’s new name will be applied only to new loans going forward.\n" + "• If your company uses settings or Encompass SDK programs with dependencies on milestone names, those areas also need to be updated to use the new name. \n" + "\n Do you want to continue? ";
      if (!(this.currentMilestone.Name != this.txtName.Text.Trim()) || Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2) != DialogResult.No)
        return;
      this.txtName.Text = this.currentMilestone.Name;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MilestonePropertiesForm));
      this.btnCancel = new Button();
      this.dlgColor = new ColorDialog();
      this.btnSave = new Button();
      this.panel1 = new Panel();
      this.txtConsumerStatus = new TextBox();
      this.lblConsumerStatus = new Label();
      this.txtName = new TextBox();
      this.txtTPOConnectStatus = new TextBox();
      this.label9 = new Label();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label6 = new Label();
      this.txtDays = new TextBox();
      this.lblDays = new Label();
      this.txtDisplayAfter = new TextBox();
      this.txtDisplayBefore = new TextBox();
      this.chkRoleRequired = new CheckBox();
      this.btnSelect = new StandardIconButton();
      this.txtFieldSummaryForm = new TextBox();
      this.lblMilestone = new Label();
      this.cboRoles = new ComboBox();
      this.label5 = new Label();
      this.btnPickColor = new IconButton();
      this.icoColor = new GradientIcon();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.label10 = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.btnSelect).BeginInit();
      ((ISupportInitialize) this.btnPickColor).BeginInit();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(504, 319);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(421, 319);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 1;
      this.btnSave.Text = "&Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.panel1.Controls.Add((Control) this.txtConsumerStatus);
      this.panel1.Controls.Add((Control) this.lblConsumerStatus);
      this.panel1.Controls.Add((Control) this.txtName);
      this.panel1.Controls.Add((Control) this.txtTPOConnectStatus);
      this.panel1.Controls.Add((Control) this.label9);
      this.panel1.Controls.Add((Control) this.label8);
      this.panel1.Controls.Add((Control) this.label7);
      this.panel1.Controls.Add((Control) this.label6);
      this.panel1.Controls.Add((Control) this.txtDays);
      this.panel1.Controls.Add((Control) this.lblDays);
      this.panel1.Controls.Add((Control) this.txtDisplayAfter);
      this.panel1.Controls.Add((Control) this.txtDisplayBefore);
      this.panel1.Controls.Add((Control) this.chkRoleRequired);
      this.panel1.Controls.Add((Control) this.btnSelect);
      this.panel1.Controls.Add((Control) this.txtFieldSummaryForm);
      this.panel1.Controls.Add((Control) this.lblMilestone);
      this.panel1.Controls.Add((Control) this.cboRoles);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Controls.Add((Control) this.btnPickColor);
      this.panel1.Controls.Add((Control) this.icoColor);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.label10);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(588, 297);
      this.panel1.TabIndex = 0;
      this.txtConsumerStatus.Location = new Point(124, 92);
      this.txtConsumerStatus.MaxLength = 128;
      this.txtConsumerStatus.Name = "txtConsumerStatus";
      this.txtConsumerStatus.Size = new Size(196, 23);
      this.txtConsumerStatus.TabIndex = 46;
      this.lblConsumerStatus.AutoSize = true;
      this.lblConsumerStatus.Location = new Point(9, 95);
      this.lblConsumerStatus.Name = "lblConsumerStatus";
      this.lblConsumerStatus.Size = new Size(113, 16);
      this.lblConsumerStatus.TabIndex = 45;
      this.lblConsumerStatus.Text = "Consumer Status";
      this.txtName.Location = new Point(124, 9);
      this.txtName.MaxLength = 128;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(196, 23);
      this.txtName.TabIndex = 43;
      this.txtName.TextChanged += new EventHandler(this.txtName_TextChanged);
      this.txtName.Leave += new EventHandler(this.txtName_Leave);
      this.txtTPOConnectStatus.Location = new Point(124, 63);
      this.txtTPOConnectStatus.MaxLength = 128;
      this.txtTPOConnectStatus.Name = "txtTPOConnectStatus";
      this.txtTPOConnectStatus.Size = new Size(196, 23);
      this.txtTPOConnectStatus.TabIndex = 42;
      this.txtTPOConnectStatus.Visible = false;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(21, 268);
      this.label9.Name = "label9";
      this.label9.Size = new Size(108, 16);
      this.label9.TabIndex = 41;
      this.label9.Text = "Required Fields";
      this.label8.AutoSize = true;
      this.label8.ForeColor = Color.Red;
      this.label8.Location = new Point(12, 268);
      this.label8.Name = "label8";
      this.label8.Size = new Size(13, 16);
      this.label8.TabIndex = 40;
      this.label8.Text = "*";
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.ForeColor = Color.Red;
      this.label7.Location = new Point(103, 35);
      this.label7.Name = "label7";
      this.label7.Size = new Size(13, 16);
      this.label7.TabIndex = 39;
      this.label7.Text = "*";
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.ForeColor = Color.Red;
      this.label6.Location = new Point(103, 13);
      this.label6.Name = "label6";
      this.label6.Size = new Size(13, 16);
      this.label6.TabIndex = 38;
      this.label6.Text = "*";
      this.txtDays.Location = new Point(124, 212);
      this.txtDays.MaxLength = 3;
      this.txtDays.Name = "txtDays";
      this.txtDays.ShortcutsEnabled = false;
      this.txtDays.Size = new Size(196, 23);
      this.txtDays.TabIndex = 37;
      this.txtDays.KeyPress += new KeyPressEventHandler(this.txtDays_KeyPress);
      this.txtDays.Leave += new EventHandler(this.txtDays_Leave);
      this.lblDays.AutoSize = true;
      this.lblDays.Location = new Point(9, 213);
      this.lblDays.Name = "lblDays";
      this.lblDays.Size = new Size(98, 16);
      this.lblDays.TabIndex = 36;
      this.lblDays.Text = "Days to Finish";
      this.txtDisplayAfter.Location = new Point(336, 35);
      this.txtDisplayAfter.MaxLength = 254;
      this.txtDisplayAfter.Name = "txtDisplayAfter";
      this.txtDisplayAfter.Size = new Size(196, 23);
      this.txtDisplayAfter.TabIndex = 43;
      this.txtDisplayAfter.TextChanged += new EventHandler(this.txtDisplay_TextChanged);
      this.txtDisplayAfter.Enter += new EventHandler(this.txtDisplayAfter_Enter);
      this.txtDisplayBefore.Location = new Point(124, 35);
      this.txtDisplayBefore.MaxLength = 254;
      this.txtDisplayBefore.Name = "txtDisplayBefore";
      this.txtDisplayBefore.Size = new Size(196, 23);
      this.txtDisplayBefore.TabIndex = 44;
      this.txtDisplayBefore.TextChanged += new EventHandler(this.txtDisplay_TextChanged);
      this.txtDisplayBefore.Enter += new EventHandler(this.txtDisplayBefore_Enter);
      this.chkRoleRequired.AutoSize = true;
      this.chkRoleRequired.Location = new Point(124, 180);
      this.chkRoleRequired.Name = "chkRoleRequired";
      this.chkRoleRequired.Size = new Size(401, 20);
      this.chkRoleRequired.TabIndex = 5;
      this.chkRoleRequired.Text = "Assigning a loan team member to this milestone is required";
      this.chkRoleRequired.UseVisualStyleBackColor = true;
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.BackColor = Color.Transparent;
      this.btnSelect.Location = new Point(560, 248);
      this.btnSelect.MouseDownImage = (Image) null;
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(16, 16);
      this.btnSelect.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSelect.TabIndex = 33;
      this.btnSelect.TabStop = false;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.txtFieldSummaryForm.Enabled = false;
      this.txtFieldSummaryForm.Location = new Point(124, 241);
      this.txtFieldSummaryForm.Name = "txtFieldSummaryForm";
      this.txtFieldSummaryForm.Size = new Size(398, 23);
      this.txtFieldSummaryForm.TabIndex = 6;
      this.lblMilestone.AutoSize = true;
      this.lblMilestone.Location = new Point(332, 10);
      this.lblMilestone.Name = "lblMilestone";
      this.lblMilestone.Size = new Size(260, 16);
      this.lblMilestone.TabIndex = 9;
      this.lblMilestone.Text = "(less than 15 characters recommended)";
      this.cboRoles.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRoles.FormattingEnabled = true;
      this.cboRoles.Location = new Point(124, 151);
      this.cboRoles.Name = "cboRoles";
      this.cboRoles.Size = new Size(196, 24);
      this.cboRoles.Sorted = true;
      this.cboRoles.TabIndex = 4;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(9, 241);
      this.label5.Name = "label5";
      this.label5.Size = new Size(139, 16);
      this.label5.TabIndex = 8;
      this.label5.Text = "Field Summary Form";
      this.btnPickColor.DisabledImage = (Image) null;
      this.btnPickColor.Image = (Image) Resources.color_picker;
      this.btnPickColor.Location = new Point(147, 126);
      this.btnPickColor.MouseDownImage = (Image) null;
      this.btnPickColor.MouseOverImage = (Image) null;
      this.btnPickColor.Name = "btnPickColor";
      this.btnPickColor.Size = new Size(16, 16);
      this.btnPickColor.TabIndex = 7;
      this.btnPickColor.TabStop = false;
      this.btnPickColor.Click += new EventHandler(this.btnPickColor_Click);
      this.icoColor.Location = new Point(124, (int) sbyte.MaxValue);
      this.icoColor.Name = "icoColor";
      this.icoColor.PrimaryColor = Color.Blue;
      this.icoColor.Size = new Size(12, 12);
      this.icoColor.TabIndex = 6;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(9, 151);
      this.label4.Name = "label4";
      this.label4.Size = new Size(37, 16);
      this.label4.TabIndex = 4;
      this.label4.Text = "Role";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(9, 126);
      this.label3.Name = "label3";
      this.label3.Size = new Size(106, 16);
      this.label3.TabIndex = 3;
      this.label3.Text = "Milestone Color";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(9, 35);
      this.label2.Name = "label2";
      this.label2.Size = new Size(113, 16);
      this.label2.TabIndex = 2;
      this.label2.Text = "As Shown in Log";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(108, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Milestone Name";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(9, 62);
      this.label10.Name = "label10";
      this.label10.Size = new Size(138, 16);
      this.label10.TabIndex = 10;
      this.label10.Text = "TPO Connect Status";
      this.label10.Visible = false;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "MilestoneDialog";
      this.emHelpLink1.Location = new Point(4, 319);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 59;
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(588, 350);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MilestonePropertiesForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Milestone Details";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.btnSelect).EndInit();
      ((ISupportInitialize) this.btnPickColor).EndInit();
      this.ResumeLayout(false);
    }
  }
}

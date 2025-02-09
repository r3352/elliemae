// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.RoleFunctionPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class RoleFunctionPanel : SettingsUserControl
  {
    private const string className = "RoleFunctionPanel";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private GridView listViewRole;
    private IContainer components;
    private WorkflowManager workflowMgr;
    private Label label6;
    private Label label7;
    private ComboBox cmbLO;
    private ComboBox cmbLP;
    public Hashtable PersonaSettings;
    public Hashtable UserGroupSettings;
    private Label label8;
    private ComboBox cmbCL;
    private ComboBox cmbUW;
    private Label label1;
    private GroupContainer gcRoleMapping;
    private StandardIconButton stdIconBtnReset;
    private StandardIconButton stdIconBtnSave;
    private GroupContainer gcRoles;
    private StandardIconButton stdIconBtnDelete;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnNew;
    private ToolTip toolTip1;
    private RoleInfo[] roleInfos;
    private IEnumerable<EllieMae.EMLite.Workflow.Milestone> milestoneList;
    private Label lblTPOPsr;
    private Label lblPsr;
    private ComboBox cmbPSR;
    private Label lblTPOTlp;
    private Label lblTlp;
    private ComboBox cmbTLP;
    private Label lblTPOTlo;
    private Label lblTlo;
    private ComboBox cmbTLO;
    private Sessions.Session session;
    private bool suspendDDLValueChangedEventHandler;

    public RoleFunctionPanel(SetUpContainer setupContainer)
      : this(Session.DefaultInstance, setupContainer, false)
    {
    }

    public RoleFunctionPanel(
      Sessions.Session session,
      SetUpContainer setupContainer,
      bool allowMultiSelect)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      this.initForm();
      this.listViewRole.AllowMultiselect = allowMultiSelect;
      this.listViewRole.Sort(0, SortOrder.Ascending);
      this.listViewRole_SelectedIndexChanged((object) this, (EventArgs) null);
      this.setDirtyFlag(false);
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.listViewRole = new GridView();
      this.cmbUW = new ComboBox();
      this.label1 = new Label();
      this.cmbCL = new ComboBox();
      this.label8 = new Label();
      this.cmbLP = new ComboBox();
      this.cmbLO = new ComboBox();
      this.label7 = new Label();
      this.label6 = new Label();
      this.gcRoleMapping = new GroupContainer();
      this.lblTPOTlp = new Label();
      this.lblTlp = new Label();
      this.cmbTLP = new ComboBox();
      this.lblTPOTlo = new Label();
      this.lblTlo = new Label();
      this.cmbTLO = new ComboBox();
      this.lblTPOPsr = new Label();
      this.lblPsr = new Label();
      this.cmbPSR = new ComboBox();
      this.stdIconBtnReset = new StandardIconButton();
      this.stdIconBtnSave = new StandardIconButton();
      this.gcRoles = new GroupContainer();
      this.stdIconBtnDelete = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnNew = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.gcRoleMapping.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnReset).BeginInit();
      ((ISupportInitialize) this.stdIconBtnSave).BeginInit();
      this.gcRoles.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      this.SuspendLayout();
      this.listViewRole.AllowMultiselect = false;
      this.listViewRole.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Role Name";
      gvColumn1.Width = 155;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Abbreviation";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Personas";
      gvColumn3.Width = 180;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "User Groups";
      gvColumn4.Width = 180;
      this.listViewRole.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.listViewRole.Dock = DockStyle.Fill;
      this.listViewRole.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewRole.Location = new Point(1, 26);
      this.listViewRole.Name = "listViewRole";
      this.listViewRole.Size = new Size(553, 200);
      this.listViewRole.TabIndex = 0;
      this.listViewRole.SelectedIndexChanged += new EventHandler(this.listViewRole_SelectedIndexChanged);
      this.listViewRole.ItemDoubleClick += new GVItemEventHandler(this.listViewRole_ItemDoubleClick);
      this.cmbUW.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbUW.Location = new Point(259, 113);
      this.cmbUW.Name = "cmbUW";
      this.cmbUW.Size = new Size(121, 22);
      this.cmbUW.TabIndex = 12;
      this.cmbUW.SelectedValueChanged += new EventHandler(this.comboBox_SelectedValueChanged);
      this.label1.Location = new Point(10, 117);
      this.label1.Name = "label1";
      this.label1.Size = new Size(228, 16);
      this.label1.TabIndex = 13;
      this.label1.Text = "Which role represents the Underwriter?";
      this.cmbCL.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCL.Location = new Point(259, 86);
      this.cmbCL.Name = "cmbCL";
      this.cmbCL.Size = new Size(121, 22);
      this.cmbCL.TabIndex = 11;
      this.cmbCL.SelectedValueChanged += new EventHandler(this.comboBox_SelectedValueChanged);
      this.label8.Location = new Point(10, 90);
      this.label8.Name = "label8";
      this.label8.Size = new Size(228, 16);
      this.label8.TabIndex = 11;
      this.label8.Text = "Which role represents the Loan Closer?";
      this.cmbLP.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLP.Location = new Point(259, 59);
      this.cmbLP.Name = "cmbLP";
      this.cmbLP.Size = new Size(121, 22);
      this.cmbLP.TabIndex = 10;
      this.cmbLP.SelectedValueChanged += new EventHandler(this.comboBox_SelectedValueChanged);
      this.cmbLO.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLO.Location = new Point(259, 32);
      this.cmbLO.Name = "cmbLO";
      this.cmbLO.Size = new Size(121, 22);
      this.cmbLO.TabIndex = 9;
      this.cmbLO.SelectedValueChanged += new EventHandler(this.comboBox_SelectedValueChanged);
      this.label7.Location = new Point(10, 63);
      this.label7.Name = "label7";
      this.label7.Size = new Size(228, 16);
      this.label7.TabIndex = 6;
      this.label7.Text = "Which role represents the Loan Processor?";
      this.label6.Location = new Point(10, 36);
      this.label6.Name = "label6";
      this.label6.Size = new Size(228, 16);
      this.label6.TabIndex = 5;
      this.label6.Text = "Which role represents the Loan Officer?";
      this.gcRoleMapping.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcRoleMapping.Controls.Add((Control) this.lblTPOTlp);
      this.gcRoleMapping.Controls.Add((Control) this.lblTlp);
      this.gcRoleMapping.Controls.Add((Control) this.cmbTLP);
      this.gcRoleMapping.Controls.Add((Control) this.lblTPOTlo);
      this.gcRoleMapping.Controls.Add((Control) this.lblTlo);
      this.gcRoleMapping.Controls.Add((Control) this.cmbTLO);
      this.gcRoleMapping.Controls.Add((Control) this.lblTPOPsr);
      this.gcRoleMapping.Controls.Add((Control) this.lblPsr);
      this.gcRoleMapping.Controls.Add((Control) this.cmbPSR);
      this.gcRoleMapping.Controls.Add((Control) this.stdIconBtnReset);
      this.gcRoleMapping.Controls.Add((Control) this.stdIconBtnSave);
      this.gcRoleMapping.Controls.Add((Control) this.label1);
      this.gcRoleMapping.Controls.Add((Control) this.cmbUW);
      this.gcRoleMapping.Controls.Add((Control) this.label6);
      this.gcRoleMapping.Controls.Add((Control) this.label7);
      this.gcRoleMapping.Controls.Add((Control) this.cmbLO);
      this.gcRoleMapping.Controls.Add((Control) this.cmbCL);
      this.gcRoleMapping.Controls.Add((Control) this.cmbLP);
      this.gcRoleMapping.Controls.Add((Control) this.label8);
      this.gcRoleMapping.Dock = DockStyle.Bottom;
      this.gcRoleMapping.HeaderForeColor = SystemColors.ControlText;
      this.gcRoleMapping.Location = new Point(0, 227);
      this.gcRoleMapping.Name = "gcRoleMapping";
      this.gcRoleMapping.Size = new Size(555, 230);
      this.gcRoleMapping.TabIndex = 14;
      this.gcRoleMapping.Text = "Role Mapping";
      this.lblTPOTlp.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTPOTlp.ForeColor = Color.Red;
      this.lblTPOTlp.Location = new Point(386, 198);
      this.lblTPOTlp.Name = "lblTPOTlp";
      this.lblTPOTlp.Size = new Size(67, 18);
      this.lblTPOTlp.TabIndex = 24;
      this.lblTPOTlp.Text = "(TPO Only)";
      this.lblTlp.Location = new Point(10, 198);
      this.lblTlp.Name = "lblTlp";
      this.lblTlp.Size = new Size(246, 18);
      this.lblTlp.TabIndex = 23;
      this.lblTlp.Text = "Which role represents the TPO Loan Processor?";
      this.cmbTLP.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbTLP.Location = new Point(259, 194);
      this.cmbTLP.Name = "cmbTLP";
      this.cmbTLP.Size = new Size(121, 22);
      this.cmbTLP.TabIndex = 22;
      this.cmbTLP.SelectedValueChanged += new EventHandler(this.comboBox_SelectedValueChanged);
      this.lblTPOTlo.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTPOTlo.ForeColor = Color.Red;
      this.lblTPOTlo.Location = new Point(386, 171);
      this.lblTPOTlo.Name = "lblTPOTlo";
      this.lblTPOTlo.Size = new Size(67, 18);
      this.lblTPOTlo.TabIndex = 21;
      this.lblTPOTlo.Text = "(TPO Only)";
      this.lblTlo.Location = new Point(10, 171);
      this.lblTlo.Name = "lblTlo";
      this.lblTlo.Size = new Size(233, 18);
      this.lblTlo.TabIndex = 20;
      this.lblTlo.Text = "Which role represents the TPO Loan Officer?";
      this.cmbTLO.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbTLO.Location = new Point(259, 167);
      this.cmbTLO.Name = "cmbTLO";
      this.cmbTLO.Size = new Size(121, 22);
      this.cmbTLO.TabIndex = 19;
      this.cmbTLO.SelectedValueChanged += new EventHandler(this.comboBox_SelectedValueChanged);
      this.lblTPOPsr.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTPOPsr.ForeColor = Color.Red;
      this.lblTPOPsr.Location = new Point(386, 144);
      this.lblTPOPsr.Name = "lblTPOPsr";
      this.lblTPOPsr.Size = new Size(67, 18);
      this.lblTPOPsr.TabIndex = 18;
      this.lblTPOPsr.Text = "(TPO Only)";
      this.lblPsr.Location = new Point(10, 144);
      this.lblPsr.Name = "lblPsr";
      this.lblPsr.Size = new Size(233, 18);
      this.lblPsr.TabIndex = 17;
      this.lblPsr.Text = "Which role represents the TPO Sales Rep?";
      this.cmbPSR.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbPSR.Location = new Point(259, 140);
      this.cmbPSR.Name = "cmbPSR";
      this.cmbPSR.Size = new Size(121, 22);
      this.cmbPSR.TabIndex = 16;
      this.cmbPSR.SelectedValueChanged += new EventHandler(this.comboBox_SelectedValueChanged);
      this.stdIconBtnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnReset.BackColor = Color.Transparent;
      this.stdIconBtnReset.Location = new Point(532, 4);
      this.stdIconBtnReset.MouseDownImage = (Image) null;
      this.stdIconBtnReset.Name = "stdIconBtnReset";
      this.stdIconBtnReset.Size = new Size(16, 16);
      this.stdIconBtnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.stdIconBtnReset.TabIndex = 15;
      this.stdIconBtnReset.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnReset, "Reset Role Mapping");
      this.stdIconBtnReset.Click += new EventHandler(this.stdIconBtnReset_Click);
      this.stdIconBtnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnSave.BackColor = Color.Transparent;
      this.stdIconBtnSave.Location = new Point(510, 4);
      this.stdIconBtnSave.MouseDownImage = (Image) null;
      this.stdIconBtnSave.Name = "stdIconBtnSave";
      this.stdIconBtnSave.Size = new Size(16, 16);
      this.stdIconBtnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.stdIconBtnSave.TabIndex = 14;
      this.stdIconBtnSave.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnSave, "Save Role Mapping");
      this.stdIconBtnSave.Click += new EventHandler(this.stdIconBtnSave_Click);
      this.gcRoles.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcRoles.Controls.Add((Control) this.stdIconBtnEdit);
      this.gcRoles.Controls.Add((Control) this.stdIconBtnNew);
      this.gcRoles.Controls.Add((Control) this.listViewRole);
      this.gcRoles.Dock = DockStyle.Fill;
      this.gcRoles.HeaderForeColor = SystemColors.ControlText;
      this.gcRoles.Location = new Point(0, 0);
      this.gcRoles.Name = "gcRoles";
      this.gcRoles.Size = new Size(555, 227);
      this.gcRoles.TabIndex = 15;
      this.gcRoles.Text = "Roles";
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(532, 5);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 3;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete Role");
      this.stdIconBtnDelete.Click += new EventHandler(this.stdIconBtnDelete_Click);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(510, 5);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 2;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit Role");
      this.stdIconBtnEdit.Click += new EventHandler(this.stdIconBtnEdit_Click);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(488, 5);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 1;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "Add Role");
      this.stdIconBtnNew.Click += new EventHandler(this.stdIconBtnNew_Click);
      this.Controls.Add((Control) this.gcRoles);
      this.Controls.Add((Control) this.gcRoleMapping);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (RoleFunctionPanel);
      this.Size = new Size(555, 457);
      this.gcRoleMapping.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnReset).EndInit();
      ((ISupportInitialize) this.stdIconBtnSave).EndInit();
      this.gcRoles.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      this.ResumeLayout(false);
    }

    public void initForm()
    {
      try
      {
        this.workflowMgr = (WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow);
        this.roleInfos = this.workflowMgr.GetAllRoleFunctions();
        this.milestoneList = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllActiveMilestonesList();
        this.PersonaSettings = new Hashtable();
        Persona[] allCustomPersonas = this.session.PersonaManager.GetAllCustomPersonas();
        if (allCustomPersonas != null)
        {
          for (int index = 0; index < allCustomPersonas.Length; ++index)
            this.PersonaSettings.Add((object) allCustomPersonas[index].ID, (object) allCustomPersonas[index].Name);
        }
        this.UserGroupSettings = new Hashtable();
        AclGroup[] allGroups = this.session.AclGroupManager.GetAllGroups();
        if (allGroups != null)
        {
          for (int index = 0; index < allGroups.Length; ++index)
            this.UserGroupSettings.Add((object) allGroups[index].ID, (object) allGroups[index].Name);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(RoleFunctionPanel.sw, TraceLevel.Error, nameof (RoleFunctionPanel), "initForm: can't get all roles. Error: " + ex.Message);
      }
      this.listViewRole.Items.Clear();
      this.listViewRole.BeginUpdate();
      if (this.roleInfos != null)
      {
        for (int index = 0; index < this.roleInfos.Length; ++index)
        {
          RoleInfo roleInfo = this.roleInfos[index];
          GVItem gvItem = new GVItem();
          this.populateRoleListItem(gvItem, roleInfo);
          this.listViewRole.Items.Add(gvItem);
        }
      }
      this.listViewRole.EndUpdate();
      this.ResetRolesDDL();
      this.updateRoleCount();
      bool flag = this.session.ConfigurationManager.CheckIfAnyTPOSiteExists();
      this.lblPsr.Visible = this.cmbPSR.Visible = this.lblTPOPsr.Visible = this.lblTlo.Visible = this.cmbTLO.Visible = this.lblTPOTlo.Visible = this.lblTlp.Visible = this.cmbTLP.Visible = this.lblTPOTlp.Visible = flag;
      if (flag)
        return;
      this.listViewRole.Size = new Size(553, 252);
      this.cmbUW.Location = new Point(249, 113);
      this.cmbCL.Location = new Point(249, 86);
      this.cmbLP.Location = new Point(249, 59);
      this.cmbLO.Location = new Point(249, 32);
      this.gcRoleMapping.Location = new Point(0, 279);
      this.gcRoleMapping.Size = new Size(555, 178);
      this.gcRoles.Size = new Size(555, 279);
    }

    private void populateRoleListItem(GVItem item, RoleInfo role)
    {
      item.SubItems[0].Text = role.Name;
      item.SubItems[1].Text = role.RoleAbbr;
      item.SubItems[2].Text = this.buildPersonasColumn(role.PersonaIDs);
      item.SubItems[3].Text = this.buildUserGroupsColumn(role.UserGroupIDs);
      item.Tag = (object) role.RoleID;
    }

    private void updateRoleCount()
    {
      this.gcRoles.Text = "Roles (" + (object) this.listViewRole.Items.Count + ")";
    }

    private void listViewRole_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editSelectedItem();
    }

    private void stdIconBtnEdit_Click(object sender, EventArgs e) => this.editSelectedItem();

    private void editSelectedItem()
    {
      if (this.listViewRole.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a role first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        RoleInfo roleFunction = this.workflowMgr.GetRoleFunction((int) this.listViewRole.SelectedItems[0].Tag);
        if (roleFunction == null)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The role '" + this.listViewRole.SelectedItems[0].Text + "' has been removed by other user.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.initForm();
        }
        else
        {
          using (RoleDetailForm roleDetailForm = new RoleDetailForm(this.session, roleFunction, this))
          {
            if (roleDetailForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
            {
              this.workflowMgr.SetRoleFunction(roleDetailForm.Role);
              this.populateRoleListItem(this.listViewRole.SelectedItems[0], roleDetailForm.Role);
            }
          }
          this.ResetRolesDDL();
        }
      }
    }

    private void stdIconBtnNew_Click(object sender, EventArgs e)
    {
      using (RoleDetailForm roleDetailForm = new RoleDetailForm(this.session, (RoleInfo) null, this))
      {
        if (roleDetailForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          RoleInfo roleFunction = this.workflowMgr.GetRoleFunction(this.workflowMgr.SetRoleFunction(roleDetailForm.Role));
          if (roleFunction != null)
          {
            GVItem gvItem = new GVItem(roleFunction.RoleName);
            this.populateRoleListItem(gvItem, roleFunction);
            gvItem.Selected = true;
            this.listViewRole.Items.Add(gvItem);
          }
        }
      }
      this.ResetRolesDDL();
      this.SecurityRightSynchronization();
      this.updateRoleCount();
    }

    private void stdIconBtnDelete_Click(object sender, EventArgs e)
    {
      if (this.listViewRole.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a role first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int tag = (int) this.listViewRole.SelectedItems[0].Tag;
        int num2 = 0;
        foreach (EllieMae.EMLite.Workflow.Milestone milestone in this.session.StartupInfo.Milestones)
        {
          if (milestone.RoleID == tag)
            ++num2;
        }
        if (num2 > 0)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, num2.ToString() + " milestone(s) contain this role. You cannot delete this role.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete selected role?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
            return;
          this.workflowMgr.DeleteRoleFunction(tag);
          int index = this.listViewRole.SelectedItems[0].Index;
          this.listViewRole.Items.Remove(this.listViewRole.SelectedItems[0]);
          if (this.listViewRole.Items.Count == 0)
            return;
          if (index + 1 > this.listViewRole.Items.Count)
            this.listViewRole.Items[this.listViewRole.Items.Count - 1].Selected = true;
          else
            this.listViewRole.Items[index].Selected = true;
          this.ResetRolesDDL();
          this.SecurityRightSynchronization();
          this.updateRoleCount();
        }
      }
    }

    private string buildPersonasColumn(int[] personaIDs)
    {
      string str = "";
      if (personaIDs != null)
      {
        for (int index = 0; index < personaIDs.Length; ++index)
        {
          if (this.PersonaSettings.ContainsKey((object) personaIDs[index]))
            str = !(str == "") ? str + "," + this.PersonaSettings[(object) personaIDs[index]].ToString() : this.PersonaSettings[(object) personaIDs[index]].ToString();
        }
      }
      return str;
    }

    private string buildUserGroupsColumn(int[] groupIds)
    {
      string str = "";
      if (groupIds != null)
      {
        for (int index = 0; index < groupIds.Length; ++index)
        {
          if (this.UserGroupSettings.ContainsKey((object) groupIds[index]))
            str = !(str == "") ? str + "," + this.UserGroupSettings[(object) groupIds[index]].ToString() : this.UserGroupSettings[(object) groupIds[index]].ToString();
        }
      }
      return str;
    }

    private void ResetRolesDDL()
    {
      this.roleInfos = this.workflowMgr.GetAllRoleFunctions();
      this.populateRolesDDL();
    }

    private void populateRolesDDL()
    {
      this.suspendDDLValueChangedEventHandler = true;
      try
      {
        this.cmbLO.Items.Clear();
        this.cmbLO.Items.Add((object) "None");
        if (this.roleInfos != null)
          this.cmbLO.Items.AddRange((object[]) this.roleInfos);
        this.cmbLO.SelectedIndex = 0;
        this.cmbLP.Items.Clear();
        this.cmbLP.Items.Add((object) "None");
        if (this.roleInfos != null)
          this.cmbLP.Items.AddRange((object[]) this.roleInfos);
        this.cmbLP.SelectedIndex = 0;
        this.cmbCL.Items.Clear();
        this.cmbCL.Items.Add((object) "None");
        if (this.roleInfos != null)
          this.cmbCL.Items.AddRange((object[]) this.roleInfos);
        this.cmbCL.SelectedIndex = 0;
        this.cmbUW.Items.Clear();
        this.cmbUW.Items.Add((object) "None");
        if (this.roleInfos != null)
          this.cmbUW.Items.AddRange((object[]) this.roleInfos);
        this.cmbUW.SelectedIndex = 0;
        this.cmbPSR.Items.Clear();
        this.cmbPSR.Items.Add((object) "None");
        if (this.roleInfos != null)
          this.cmbPSR.Items.AddRange((object[]) this.roleInfos);
        this.cmbPSR.SelectedIndex = 0;
        this.cmbTLO.Items.Clear();
        this.cmbTLO.Items.Add((object) "None");
        if (this.roleInfos != null)
          this.cmbTLO.Items.AddRange((object[]) this.roleInfos);
        this.cmbTLO.SelectedIndex = 0;
        this.cmbTLP.Items.Clear();
        this.cmbTLP.Items.Add((object) "None");
        if (this.roleInfos != null)
          this.cmbTLP.Items.AddRange((object[]) this.roleInfos);
        this.cmbTLP.SelectedIndex = 0;
        this.cmbLO.SelectedIndex = 0;
        this.cmbLP.SelectedIndex = 0;
        this.cmbCL.SelectedIndex = 0;
        this.cmbUW.SelectedIndex = 0;
        this.cmbPSR.SelectedIndex = 0;
        this.cmbTLO.SelectedIndex = 0;
        this.cmbTLP.SelectedIndex = 0;
        RolesMappingInfo[] roleMappingInfos = this.workflowMgr.GetAllRoleMappingInfos();
        if (roleMappingInfos == null || roleMappingInfos.Length == 0)
          return;
        foreach (RolesMappingInfo rolesMappingInfo in roleMappingInfos)
        {
          if (rolesMappingInfo.RoleIDList != null && rolesMappingInfo.RoleIDList.Length != 0)
          {
            switch (rolesMappingInfo.RealWorldRoleID)
            {
              case RealWorldRoleID.LoanOfficer:
                this.cmbLO.SelectedIndex = this.GetSelectedRoleIndex(rolesMappingInfo.RoleIDList[0]);
                continue;
              case RealWorldRoleID.LoanProcessor:
                this.cmbLP.SelectedIndex = this.GetSelectedRoleIndex(rolesMappingInfo.RoleIDList[0]);
                continue;
              case RealWorldRoleID.LoanCloser:
                this.cmbCL.SelectedIndex = this.GetSelectedRoleIndex(rolesMappingInfo.RoleIDList[0]);
                continue;
              case RealWorldRoleID.Underwriter:
                this.cmbUW.SelectedIndex = this.GetSelectedRoleIndex(rolesMappingInfo.RoleIDList[0]);
                continue;
              case RealWorldRoleID.PrimarySalesRep:
                this.cmbPSR.SelectedIndex = this.GetSelectedRoleIndex(rolesMappingInfo.RoleIDList[0]);
                continue;
              case RealWorldRoleID.TPOLoanOfficer:
                this.cmbTLO.SelectedIndex = this.GetSelectedRoleIndex(rolesMappingInfo.RoleIDList[0]);
                continue;
              case RealWorldRoleID.TPOLoanProcessor:
                this.cmbTLP.SelectedIndex = this.GetSelectedRoleIndex(rolesMappingInfo.RoleIDList[0]);
                continue;
              default:
                continue;
            }
          }
        }
      }
      finally
      {
        this.suspendDDLValueChangedEventHandler = false;
      }
    }

    private int GetSelectedRoleIndex(int roleID)
    {
      int selectedRoleIndex = 0;
      for (int index = 1; index < this.cmbLO.Items.Count; ++index)
      {
        if (((RoleSummaryInfo) this.cmbLO.Items[index]).RoleID == roleID)
        {
          selectedRoleIndex = index;
          break;
        }
      }
      return selectedRoleIndex;
    }

    private void SecurityRightSynchronization()
    {
      ((ToolsAclManager) this.session.ACL.GetAclManager(AclCategory.ToolsGrantWriteAccess)).SynchronizeAdminRight();
    }

    private void stdIconBtnSave_Click(object sender, EventArgs e) => this.Save();

    private void stdIconBtnReset_Click(object sender, EventArgs e) => this.Reset();

    public override void Save()
    {
      RolesMappingInfo rolesMappingInfo1;
      if (this.cmbLO.Text != "None")
        rolesMappingInfo1 = new RolesMappingInfo(new int[1]
        {
          ((RoleSummaryInfo) this.cmbLO.SelectedItem).RoleID
        }, RealWorldRoleID.LoanOfficer);
      else
        rolesMappingInfo1 = new RolesMappingInfo((int[]) null, RealWorldRoleID.LoanOfficer);
      RolesMappingInfo rolesMappingInfo2;
      if (this.cmbLP.Text != "None")
        rolesMappingInfo2 = new RolesMappingInfo(new int[1]
        {
          ((RoleSummaryInfo) this.cmbLP.SelectedItem).RoleID
        }, RealWorldRoleID.LoanProcessor);
      else
        rolesMappingInfo2 = new RolesMappingInfo((int[]) null, RealWorldRoleID.LoanProcessor);
      RolesMappingInfo rolesMappingInfo3;
      if (this.cmbCL.Text != "None")
        rolesMappingInfo3 = new RolesMappingInfo(new int[1]
        {
          ((RoleSummaryInfo) this.cmbCL.SelectedItem).RoleID
        }, RealWorldRoleID.LoanCloser);
      else
        rolesMappingInfo3 = new RolesMappingInfo((int[]) null, RealWorldRoleID.LoanCloser);
      RolesMappingInfo rolesMappingInfo4;
      if (this.cmbUW.Text != "None")
        rolesMappingInfo4 = new RolesMappingInfo(new int[1]
        {
          ((RoleSummaryInfo) this.cmbUW.SelectedItem).RoleID
        }, RealWorldRoleID.Underwriter);
      else
        rolesMappingInfo4 = new RolesMappingInfo((int[]) null, RealWorldRoleID.Underwriter);
      RolesMappingInfo rolesMappingInfo5;
      if (this.cmbPSR.Text != "None")
        rolesMappingInfo5 = new RolesMappingInfo(new int[1]
        {
          ((RoleSummaryInfo) this.cmbPSR.SelectedItem).RoleID
        }, RealWorldRoleID.PrimarySalesRep);
      else
        rolesMappingInfo5 = new RolesMappingInfo((int[]) null, RealWorldRoleID.PrimarySalesRep);
      RolesMappingInfo rolesMappingInfo6;
      if (this.cmbTLO.Text != "None")
        rolesMappingInfo6 = new RolesMappingInfo(new int[1]
        {
          ((RoleSummaryInfo) this.cmbTLO.SelectedItem).RoleID
        }, RealWorldRoleID.TPOLoanOfficer);
      else
        rolesMappingInfo6 = new RolesMappingInfo((int[]) null, RealWorldRoleID.TPOLoanOfficer);
      RolesMappingInfo rolesMappingInfo7;
      if (this.cmbTLP.Text != "None")
        rolesMappingInfo7 = new RolesMappingInfo(new int[1]
        {
          ((RoleSummaryInfo) this.cmbTLP.SelectedItem).RoleID
        }, RealWorldRoleID.TPOLoanProcessor);
      else
        rolesMappingInfo7 = new RolesMappingInfo((int[]) null, RealWorldRoleID.TPOLoanProcessor);
      this.workflowMgr.UpdateRoleMappingInfos(new RolesMappingInfo[7]
      {
        rolesMappingInfo1,
        rolesMappingInfo2,
        rolesMappingInfo3,
        rolesMappingInfo4,
        rolesMappingInfo5,
        rolesMappingInfo6,
        rolesMappingInfo7
      });
      this.setDirtyFlag(false);
    }

    public override void Reset()
    {
      this.populateRolesDDL();
      this.setDirtyFlag(false);
    }

    protected override void setDirtyFlag(bool val)
    {
      base.setDirtyFlag(val);
      if (this.stdIconBtnSave == null || this.stdIconBtnReset == null)
        return;
      this.stdIconBtnSave.Enabled = this.stdIconBtnReset.Enabled = val;
    }

    private void comboBox_SelectedValueChanged(object sender, EventArgs e)
    {
      if (this.suspendDDLValueChangedEventHandler)
        return;
      this.setDirtyFlag(true);
    }

    private void listViewRole_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnEdit.Enabled = this.listViewRole.SelectedItems.Count == 1;
      this.stdIconBtnDelete.Enabled = this.listViewRole.SelectedItems.Count == 1;
    }

    public string[] SelectedRoles
    {
      get
      {
        if (this.listViewRole.SelectedItems == null)
          return (string[]) null;
        List<string> stringList = new List<string>();
        foreach (GVItem selectedItem in this.listViewRole.SelectedItems)
          stringList.Add(selectedItem.Text);
        return stringList.ToArray();
      }
      set
      {
        if (value == null || value.Length == 0)
          return;
        List<string> stringList = new List<string>((IEnumerable<string>) value);
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.listViewRole.Items)
        {
          if (stringList.Contains(gvItem.Text))
            gvItem.Selected = true;
        }
      }
    }
  }
}

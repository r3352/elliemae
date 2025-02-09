// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.EditCompanyDBAControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class EditCompanyDBAControl : UserControl
  {
    private Sessions.Session session;
    private SessionObjects obj;
    private bool dirty;
    private int oid;
    private int parent;
    private bool useParentInfo;
    private bool readOnly;
    private bool hasAddEditRight = true;
    private IConfigurationManager mngr;
    private List<ExternalOrgDBAName> dbas = new List<ExternalOrgDBAName>();
    private bool firstTime = true;
    private IContainer components;
    private GroupContainer grpAll;
    private GroupContainer grpTPODBA;
    private StandardIconButton btnDeleteSetting;
    private StandardIconButton btnEditSetting;
    private StandardIconButton btnAddSetting;
    private Panel panelHeader;
    private Label label33;
    private CheckBox chkUseParentInfo;
    private StandardIconButton btnReset;
    private StandardIconButton btnSave;
    private StandardIconButton btnMoveDBAUp;
    private StandardIconButton btnMoveDBADown;
    private Button btnSetDefault;
    private GridView gvNames;

    public EditCompanyDBAControl(
      SessionObjects obj,
      int oid,
      int parent,
      bool edit,
      bool isTPOTool)
    {
      this.obj = obj;
      this.oid = oid;
      this.parent = parent;
      this.readOnly = isTPOTool;
      this.mngr = this.obj.ConfigurationManager;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      if (parent == 0 || this.readOnly || !this.hasAddEditRight)
        this.chkUseParentInfo.Enabled = false;
      else
        this.chkUseParentInfo.Checked = this.useParentInfo = this.mngr.GetInheritDBANameSetting(this.oid);
      this.populate(this.chkUseParentInfo.Checked);
      if (!this.readOnly && this.hasAddEditRight && !this.chkUseParentInfo.Checked)
        this.gvNames.ItemDoubleClick += new GVItemEventHandler(this.btnEditSetting_Click);
      this.gvNames_SelectedIndexChanged((object) null, (EventArgs) null);
      this.btnAddSetting.Enabled = !this.readOnly && this.hasAddEditRight && !this.chkUseParentInfo.Checked;
      this.SetDirty(false);
      if (!isTPOTool)
        return;
      this.DisableControls();
    }

    public EditCompanyDBAControl(
      Sessions.Session session,
      int oid,
      int parent,
      bool edit,
      bool isTPOTool)
    {
      this.session = session;
      this.oid = oid;
      this.parent = parent;
      this.readOnly = isTPOTool;
      this.mngr = this.session.ConfigurationManager;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.hasAddEditRight = ((FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ExternalSettings_EditDBATab);
      if (parent == 0)
        this.chkUseParentInfo.Enabled = false;
      else
        this.chkUseParentInfo.Checked = this.useParentInfo = this.mngr.GetInheritDBANameSetting(this.oid);
      if (this.readOnly || !this.hasAddEditRight)
        this.chkUseParentInfo.Enabled = false;
      this.populate(this.chkUseParentInfo.Checked);
      if (!this.readOnly && this.hasAddEditRight && !this.chkUseParentInfo.Checked)
        this.gvNames.ItemDoubleClick += new GVItemEventHandler(this.btnEditSetting_Click);
      this.gvNames_SelectedIndexChanged((object) null, (EventArgs) null);
      this.btnAddSetting.Enabled = !this.readOnly && this.hasAddEditRight && !this.chkUseParentInfo.Checked;
      this.SetDirty(false);
      if (!isTPOTool)
        return;
      this.DisableControls();
    }

    public void DisableControls()
    {
      this.btnAddSetting.Visible = false;
      this.btnEditSetting.Visible = false;
      this.btnDeleteSetting.Visible = false;
      this.btnSetDefault.Visible = false;
      this.btnSave.Visible = false;
      this.btnReset.Visible = false;
      this.btnMoveDBAUp.Visible = false;
      this.btnMoveDBADown.Visible = false;
    }

    private void populate(bool parent)
    {
      this.gvNames.Items.Clear();
      this.dbas = !parent ? this.mngr.GetDBANames(this.oid) : this.mngr.GetDBANames(this.parent);
      foreach (ExternalOrgDBAName dba in this.dbas)
      {
        GVItem gvItem = new GVItem();
        gvItem.SubItems.Add((object) dba.Name);
        if (dba.SetAsDefault)
          gvItem.SubItems.Add((object) "Default");
        gvItem.Tag = (object) dba;
        this.gvNames.Items.Add(gvItem);
      }
    }

    private void btnAddSetting_Click(object sender, EventArgs e)
    {
      if (!this.semiSave())
        return;
      using (AddEditTPODBA addEditTpodba = new AddEditTPODBA((ExternalOrgDBAName) null))
      {
        if (addEditTpodba.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        ExternalOrgDBAName name = new ExternalOrgDBAName(this.oid, addEditTpodba.DBAName, addEditTpodba.SetAsDefault);
        name.DBAID = this.mngr.InsertDBANames(name, this.oid);
        if (name.SetAsDefault)
          this.mngr.SetDBANameAsDefault(this.oid, name.DBAID);
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.oid);
        this.populate(false);
      }
    }

    private void btnEditSetting_Click(object sender, EventArgs e)
    {
      if (this.chkUseParentInfo.Checked || !this.semiSave() || this.gvNames.SelectedItems.Count != 1)
        return;
      ExternalOrgDBAName tag = (ExternalOrgDBAName) this.gvNames.SelectedItems[0].Tag;
      using (AddEditTPODBA addEditTpodba = new AddEditTPODBA(tag))
      {
        if (addEditTpodba.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        tag.Name = addEditTpodba.DBAName;
        if (tag.SetAsDefault && !addEditTpodba.SetAsDefault)
          this.mngr.SetDBANameAsDefault(this.oid, -1);
        tag.SetAsDefault = addEditTpodba.SetAsDefault;
        this.mngr.UpdateDBANames(tag);
        if (tag.SetAsDefault)
          this.mngr.SetDBANameAsDefault(this.oid, tag.DBAID);
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.oid);
        this.populate(false);
      }
    }

    private void btnDeleteSetting_Click(object sender, EventArgs e)
    {
      if (!this.semiSave() || this.gvNames.SelectedItems.Count == 0 || Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected DBA name(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      List<ExternalOrgDBAName> names = new List<ExternalOrgDBAName>();
      foreach (GVItem selectedItem in this.gvNames.SelectedItems)
        names.Add((ExternalOrgDBAName) selectedItem.Tag);
      this.mngr.DeleteDBANames(names);
      this.populate(false);
      this.mngr.SetDBANamesSortIndex(this.getData(), this.oid);
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.oid);
    }

    private void gvNames_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnEditSetting.Enabled = !this.readOnly && this.hasAddEditRight && !this.chkUseParentInfo.Checked && this.gvNames.SelectedItems.Count == 1;
      this.btnDeleteSetting.Enabled = !this.readOnly && this.hasAddEditRight && !this.chkUseParentInfo.Checked && this.gvNames.SelectedItems.Count >= 1;
      this.btnSetDefault.Enabled = !this.readOnly && this.hasAddEditRight && !this.chkUseParentInfo.Checked && this.gvNames.SelectedItems.Count == 1;
      this.btnMoveDBAUp.Enabled = !this.readOnly && this.hasAddEditRight && !this.chkUseParentInfo.Checked && this.gvNames.SelectedItems.Count == 1 && this.gvNames.SelectedItems[0].Index != 0;
      this.btnMoveDBADown.Enabled = !this.readOnly && this.hasAddEditRight && !this.chkUseParentInfo.Checked && this.gvNames.SelectedItems.Count == 1 && this.gvNames.SelectedItems[0].Index != this.gvNames.Items.Count - 1;
      if (this.gvNames.SelectedItems.Count != 1 || !((ExternalOrgDBAName) this.gvNames.SelectedItems[0].Tag).SetAsDefault)
        return;
      this.btnSetDefault.Enabled = false;
    }

    private void btnMoveDBADown_Click(object sender, EventArgs e)
    {
      if (this.firstTime)
      {
        this.firstTime = false;
        if (!this.semiSave())
          return;
      }
      GridView gvNames = this.gvNames;
      int index = gvNames.SelectedItems[0].Index;
      GVItem gvItem = gvNames.Items[index];
      gvNames.Items.RemoveAt(index);
      gvNames.Items.Insert(index + 1, gvItem);
      gvItem.Selected = true;
      gvNames.EnsureVisible(index + 1);
      this.SetDirty(true);
    }

    private void btnMoveDBAUp_Click(object sender, EventArgs e)
    {
      if (this.firstTime)
      {
        this.firstTime = false;
        if (!this.semiSave())
          return;
      }
      GridView gvNames = this.gvNames;
      int index = gvNames.SelectedItems[0].Index;
      GVItem gvItem = gvNames.Items[index];
      gvNames.Items.RemoveAt(index);
      gvNames.Items.Insert(index - 1, gvItem);
      gvItem.Selected = true;
      gvNames.EnsureVisible(index - 1);
      this.SetDirty(true);
    }

    private void chkUseParentInfo_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.semiSave())
      {
        this.chkUseParentInfo.CheckedChanged -= new EventHandler(this.chkUseParentInfo_CheckedChanged);
        this.chkUseParentInfo.Checked = !this.chkUseParentInfo.Checked;
        this.chkUseParentInfo.CheckedChanged += new EventHandler(this.chkUseParentInfo_CheckedChanged);
      }
      else
        this.SetDirty(true);
      if (this.chkUseParentInfo.Checked)
        this.populate(true);
      else
        this.gvNames_SelectedIndexChanged((object) null, (EventArgs) null);
      this.btnAddSetting.Enabled = !this.readOnly && this.hasAddEditRight && !this.chkUseParentInfo.Checked;
      this.firstTime = true;
    }

    private void btnSetDefault_Click(object sender, EventArgs e)
    {
      if (!this.semiSave() || this.gvNames.SelectedItems.Count != 1)
        return;
      this.mngr.SetDBANameAsDefault(this.oid, ((ExternalOrgDBAName) this.gvNames.SelectedItems[0].Tag).DBAID);
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.oid);
      this.populate(false);
    }

    private Dictionary<int, int> getData()
    {
      Dictionary<int, int> data = new Dictionary<int, int>();
      int key = 0;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvNames.Items)
      {
        ExternalOrgDBAName tag = (ExternalOrgDBAName) gvItem.Tag;
        data.Add(key, tag.DBAID);
        ++key;
      }
      return data;
    }

    private bool semiSave()
    {
      if (!this.IsDirty)
        return true;
      if (Utils.Dialog((IWin32Window) this, "You must save changes before updating details. Do you want to save now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        this.btnSave_Click((object) null, (EventArgs) null);
      this.firstTime = true;
      return false;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.chkUseParentInfo.Enabled && this.chkUseParentInfo.Checked != this.useParentInfo)
      {
        this.mngr.UpdateInheritDBANameSetting(this.oid, this.chkUseParentInfo.Checked);
        this.useParentInfo = this.chkUseParentInfo.Checked;
      }
      this.mngr.SetDBANamesSortIndex(this.getData(), this.oid);
      this.populate(false);
      this.SetDirty(false);
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.oid);
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset? All changes will be lost.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      this.chkUseParentInfo.CheckedChanged -= new EventHandler(this.chkUseParentInfo_CheckedChanged);
      this.chkUseParentInfo.Checked = this.useParentInfo = this.mngr.GetInheritDBANameSetting(this.oid);
      this.chkUseParentInfo.CheckedChanged += new EventHandler(this.chkUseParentInfo_CheckedChanged);
      this.populate(this.chkUseParentInfo.Checked);
      this.SetDirty(false);
    }

    public bool IsDirty => this.dirty;

    public void SetDirty(bool value)
    {
      this.dirty = value;
      this.btnSave.Enabled = this.btnReset.Enabled = this.dirty;
    }

    public void AssignOid(int oid)
    {
      if (this.oid == -1)
        this.oid = oid;
      if (this.parent == 0 || this.readOnly || !this.hasAddEditRight)
        this.chkUseParentInfo.Enabled = false;
      else
        this.chkUseParentInfo.Checked = this.useParentInfo = this.mngr.GetInheritDBANameSetting(this.oid);
      this.populate(this.chkUseParentInfo.Checked);
      this.gvNames_SelectedIndexChanged((object) null, (EventArgs) null);
      this.SetDirty(false);
    }

    public int Oid => this.oid;

    public void Save() => this.btnSave_Click((object) null, (EventArgs) null);

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
      this.grpAll = new GroupContainer();
      this.chkUseParentInfo = new CheckBox();
      this.btnReset = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.grpTPODBA = new GroupContainer();
      this.btnSetDefault = new Button();
      this.btnMoveDBAUp = new StandardIconButton();
      this.btnMoveDBADown = new StandardIconButton();
      this.gvNames = new GridView();
      this.btnDeleteSetting = new StandardIconButton();
      this.btnEditSetting = new StandardIconButton();
      this.btnAddSetting = new StandardIconButton();
      this.panelHeader = new Panel();
      this.label33 = new Label();
      this.grpAll.SuspendLayout();
      ((ISupportInitialize) this.btnReset).BeginInit();
      ((ISupportInitialize) this.btnSave).BeginInit();
      this.grpTPODBA.SuspendLayout();
      ((ISupportInitialize) this.btnMoveDBAUp).BeginInit();
      ((ISupportInitialize) this.btnMoveDBADown).BeginInit();
      ((ISupportInitialize) this.btnDeleteSetting).BeginInit();
      ((ISupportInitialize) this.btnEditSetting).BeginInit();
      ((ISupportInitialize) this.btnAddSetting).BeginInit();
      this.panelHeader.SuspendLayout();
      this.SuspendLayout();
      this.grpAll.Controls.Add((Control) this.chkUseParentInfo);
      this.grpAll.Controls.Add((Control) this.btnReset);
      this.grpAll.Controls.Add((Control) this.btnSave);
      this.grpAll.Controls.Add((Control) this.grpTPODBA);
      this.grpAll.Controls.Add((Control) this.panelHeader);
      this.grpAll.Dock = DockStyle.Fill;
      this.grpAll.HeaderForeColor = SystemColors.ControlText;
      this.grpAll.Location = new Point(0, 0);
      this.grpAll.Name = "grpAll";
      this.grpAll.Size = new Size(639, 481);
      this.grpAll.TabIndex = 1;
      this.grpAll.Text = "DBA Information";
      this.chkUseParentInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkUseParentInfo.AutoSize = true;
      this.chkUseParentInfo.BackColor = Color.Transparent;
      this.chkUseParentInfo.Location = new Point(472, 5);
      this.chkUseParentInfo.Name = "chkUseParentInfo";
      this.chkUseParentInfo.Size = new Size(100, 17);
      this.chkUseParentInfo.TabIndex = 32;
      this.chkUseParentInfo.Text = "Use Parent Info";
      this.chkUseParentInfo.UseVisualStyleBackColor = false;
      this.chkUseParentInfo.CheckedChanged += new EventHandler(this.chkUseParentInfo_CheckedChanged);
      this.btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReset.BackColor = Color.Transparent;
      this.btnReset.Location = new Point(616, 5);
      this.btnReset.MouseDownImage = (Image) null;
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(16, 16);
      this.btnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnReset.TabIndex = 34;
      this.btnReset.TabStop = false;
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(594, 5);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 33;
      this.btnSave.TabStop = false;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.grpTPODBA.Controls.Add((Control) this.btnSetDefault);
      this.grpTPODBA.Controls.Add((Control) this.btnMoveDBAUp);
      this.grpTPODBA.Controls.Add((Control) this.btnMoveDBADown);
      this.grpTPODBA.Controls.Add((Control) this.gvNames);
      this.grpTPODBA.Controls.Add((Control) this.btnDeleteSetting);
      this.grpTPODBA.Controls.Add((Control) this.btnEditSetting);
      this.grpTPODBA.Controls.Add((Control) this.btnAddSetting);
      this.grpTPODBA.Dock = DockStyle.Fill;
      this.grpTPODBA.HeaderForeColor = SystemColors.ControlText;
      this.grpTPODBA.Location = new Point(1, 52);
      this.grpTPODBA.Name = "grpTPODBA";
      this.grpTPODBA.Size = new Size(637, 428);
      this.grpTPODBA.TabIndex = 9;
      this.grpTPODBA.Text = "DBA Details";
      this.btnSetDefault.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSetDefault.Location = new Point(442, 1);
      this.btnSetDefault.Name = "btnSetDefault";
      this.btnSetDefault.Size = new Size(82, 23);
      this.btnSetDefault.TabIndex = 40;
      this.btnSetDefault.Text = "Set as Default";
      this.btnSetDefault.UseVisualStyleBackColor = true;
      this.btnSetDefault.Click += new EventHandler(this.btnSetDefault_Click);
      this.btnMoveDBAUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMoveDBAUp.BackColor = Color.Transparent;
      this.btnMoveDBAUp.Enabled = false;
      this.btnMoveDBAUp.Location = new Point(552, 5);
      this.btnMoveDBAUp.MouseDownImage = (Image) null;
      this.btnMoveDBAUp.Name = "btnMoveDBAUp";
      this.btnMoveDBAUp.Size = new Size(16, 17);
      this.btnMoveDBAUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveDBAUp.TabIndex = 39;
      this.btnMoveDBAUp.TabStop = false;
      this.btnMoveDBAUp.Click += new EventHandler(this.btnMoveDBAUp_Click);
      this.btnMoveDBADown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMoveDBADown.BackColor = Color.Transparent;
      this.btnMoveDBADown.Enabled = false;
      this.btnMoveDBADown.Location = new Point(530, 5);
      this.btnMoveDBADown.MouseDownImage = (Image) null;
      this.btnMoveDBADown.Name = "btnMoveDBADown";
      this.btnMoveDBADown.Size = new Size(16, 17);
      this.btnMoveDBADown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveDBADown.TabIndex = 38;
      this.btnMoveDBADown.TabStop = false;
      this.btnMoveDBADown.Click += new EventHandler(this.btnMoveDBADown_Click);
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Name";
      gvColumn1.Text = "DBA Name";
      gvColumn1.Width = 500;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Default";
      gvColumn2.Text = "Default";
      gvColumn2.Width = 100;
      this.gvNames.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvNames.Dock = DockStyle.Fill;
      this.gvNames.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvNames.Location = new Point(1, 26);
      this.gvNames.Name = "gvNames";
      this.gvNames.Size = new Size(635, 401);
      this.gvNames.SortOption = GVSortOption.None;
      this.gvNames.TabIndex = 8;
      this.gvNames.SelectedIndexChanged += new EventHandler(this.gvNames_SelectedIndexChanged);
      this.btnDeleteSetting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteSetting.BackColor = Color.Transparent;
      this.btnDeleteSetting.Location = new Point(613, 5);
      this.btnDeleteSetting.Margin = new Padding(2, 3, 2, 3);
      this.btnDeleteSetting.MouseDownImage = (Image) null;
      this.btnDeleteSetting.Name = "btnDeleteSetting";
      this.btnDeleteSetting.Size = new Size(16, 17);
      this.btnDeleteSetting.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteSetting.TabIndex = 6;
      this.btnDeleteSetting.TabStop = false;
      this.btnDeleteSetting.Click += new EventHandler(this.btnDeleteSetting_Click);
      this.btnEditSetting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditSetting.BackColor = Color.Transparent;
      this.btnEditSetting.Location = new Point(593, 4);
      this.btnEditSetting.Margin = new Padding(2, 3, 2, 3);
      this.btnEditSetting.MouseDownImage = (Image) null;
      this.btnEditSetting.Name = "btnEditSetting";
      this.btnEditSetting.Size = new Size(16, 18);
      this.btnEditSetting.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditSetting.TabIndex = 5;
      this.btnEditSetting.TabStop = false;
      this.btnEditSetting.Click += new EventHandler(this.btnEditSetting_Click);
      this.btnAddSetting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddSetting.BackColor = Color.Transparent;
      this.btnAddSetting.Location = new Point(573, 5);
      this.btnAddSetting.Margin = new Padding(2, 3, 2, 3);
      this.btnAddSetting.MouseDownImage = (Image) null;
      this.btnAddSetting.Name = "btnAddSetting";
      this.btnAddSetting.Size = new Size(16, 17);
      this.btnAddSetting.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddSetting.TabIndex = 4;
      this.btnAddSetting.TabStop = false;
      this.btnAddSetting.Click += new EventHandler(this.btnAddSetting_Click);
      this.panelHeader.Controls.Add((Control) this.label33);
      this.panelHeader.Dock = DockStyle.Top;
      this.panelHeader.Location = new Point(1, 26);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(637, 26);
      this.panelHeader.TabIndex = 3;
      this.label33.AutoSize = true;
      this.label33.Location = new Point(6, 6);
      this.label33.Name = "label33";
      this.label33.Size = new Size(200, 13);
      this.label33.TabIndex = 35;
      this.label33.Text = "Set up your TPO DBA Information below.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpAll);
      this.Name = nameof (EditCompanyDBAControl);
      this.Size = new Size(639, 481);
      this.grpAll.ResumeLayout(false);
      this.grpAll.PerformLayout();
      ((ISupportInitialize) this.btnReset).EndInit();
      ((ISupportInitialize) this.btnSave).EndInit();
      this.grpTPODBA.ResumeLayout(false);
      ((ISupportInitialize) this.btnMoveDBAUp).EndInit();
      ((ISupportInitialize) this.btnMoveDBADown).EndInit();
      ((ISupportInitialize) this.btnDeleteSetting).EndInit();
      ((ISupportInitialize) this.btnEditSetting).EndInit();
      ((ISupportInitialize) this.btnAddSetting).EndInit();
      this.panelHeader.ResumeLayout(false);
      this.panelHeader.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

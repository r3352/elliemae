// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DocumentAccessPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DocumentAccessPanel : SettingsUserControl
  {
    private Sessions.Session session;
    private WorkflowManager workflowMgr;
    private DocumentAccessRuleManager docAccessMgr;
    private RoleInfo[] roleList;
    private bool isAccessDirty;
    private bool isProtectedDirty;
    private IContainer components;
    private GroupContainer gcProtectedDocs;
    private GroupContainer gcDefaultAccess;
    private GroupContainer gcProtectedRoles;
    private Panel pnlProtectedBottom;
    private Panel pnlAccessMiddle;
    private GroupContainer gcAllowAccess;
    private GroupContainer gcAddedBy;
    private GridView gvAddedBy;
    private GridView gvAllowAccess;
    private GridView gvProtectedRoles;
    private CollapsibleSplitter splDocumentAccess;
    private CollapsibleSplitter splDefaultAccess;
    private ToolTip toolTip;
    private LabelEx lblProtectedDocs;
    private LabelEx lblDefaultAccess;
    private StandardIconButton btnSaveAccess;
    private StandardIconButton btnResetAccess;
    private StandardIconButton btnResetProtected;
    private CheckBox chkDefaultRole;
    private Panel pnlAccessTop;
    private Panel pnlAccessBottom;
    private Panel pnlProtectedTop;
    private StandardIconButton btnSaveProtected;

    public DocumentAccessPanel(
      Sessions.Session session,
      SetUpContainer setupContainer,
      bool allowMultiSelect)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      this.gvAddedBy.AllowMultiselect = allowMultiSelect;
      this.gvProtectedRoles.AllowMultiselect = allowMultiSelect;
      this.docAccessMgr = (DocumentAccessRuleManager) session.BPM.GetBpmManager(BpmCategory.Document);
      this.workflowMgr = (WorkflowManager) session.BPM.GetBpmManager(BpmCategory.Workflow);
      this.roleList = this.workflowMgr.GetAllRoleFunctions();
      this.resetDocumentAccess();
      this.resetProtectedRoles();
    }

    public string[] SelectedRoles
    {
      get
      {
        if (this.gvAddedBy.SelectedItems == null)
          return new string[0];
        List<string> stringList = new List<string>();
        foreach (GVItem selectedItem in this.gvAddedBy.SelectedItems)
          stringList.Add(selectedItem.Text);
        return stringList.ToArray();
      }
      set
      {
        if (value == null || value.Length == 0)
          return;
        List<string> stringList = new List<string>((IEnumerable<string>) value);
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAddedBy.Items)
          gvItem.Selected = stringList.Contains(gvItem.Text);
      }
    }

    public string[] SelectedProtectedRoles
    {
      get
      {
        if (this.gvProtectedRoles.SelectedItems == null)
          return new string[0];
        List<string> stringList = new List<string>();
        foreach (GVItem selectedItem in this.gvProtectedRoles.SelectedItems)
          stringList.Add(selectedItem.Text);
        return stringList.ToArray();
      }
      set
      {
        if (value == null || value.Length == 0)
          return;
        List<string> stringList = new List<string>((IEnumerable<string>) value);
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvProtectedRoles.Items)
          gvItem.Selected = stringList.Contains(gvItem.Text);
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.gcProtectedDocs = new GroupContainer();
      this.btnResetProtected = new StandardIconButton();
      this.btnSaveProtected = new StandardIconButton();
      this.pnlProtectedBottom = new Panel();
      this.gcProtectedRoles = new GroupContainer();
      this.gvProtectedRoles = new GridView();
      this.pnlProtectedTop = new Panel();
      this.lblProtectedDocs = new LabelEx();
      this.gcDefaultAccess = new GroupContainer();
      this.btnResetAccess = new StandardIconButton();
      this.btnSaveAccess = new StandardIconButton();
      this.pnlAccessMiddle = new Panel();
      this.gcAllowAccess = new GroupContainer();
      this.gvAllowAccess = new GridView();
      this.splDefaultAccess = new CollapsibleSplitter();
      this.gcAddedBy = new GroupContainer();
      this.gvAddedBy = new GridView();
      this.pnlAccessBottom = new Panel();
      this.chkDefaultRole = new CheckBox();
      this.pnlAccessTop = new Panel();
      this.lblDefaultAccess = new LabelEx();
      this.splDocumentAccess = new CollapsibleSplitter();
      this.toolTip = new ToolTip(this.components);
      this.gcProtectedDocs.SuspendLayout();
      ((ISupportInitialize) this.btnResetProtected).BeginInit();
      ((ISupportInitialize) this.btnSaveProtected).BeginInit();
      this.pnlProtectedBottom.SuspendLayout();
      this.gcProtectedRoles.SuspendLayout();
      this.pnlProtectedTop.SuspendLayout();
      this.gcDefaultAccess.SuspendLayout();
      ((ISupportInitialize) this.btnResetAccess).BeginInit();
      ((ISupportInitialize) this.btnSaveAccess).BeginInit();
      this.pnlAccessMiddle.SuspendLayout();
      this.gcAllowAccess.SuspendLayout();
      this.gcAddedBy.SuspendLayout();
      this.pnlAccessBottom.SuspendLayout();
      this.pnlAccessTop.SuspendLayout();
      this.SuspendLayout();
      this.gcProtectedDocs.AutoScroll = true;
      this.gcProtectedDocs.Controls.Add((Control) this.btnResetProtected);
      this.gcProtectedDocs.Controls.Add((Control) this.btnSaveProtected);
      this.gcProtectedDocs.Controls.Add((Control) this.pnlProtectedBottom);
      this.gcProtectedDocs.Controls.Add((Control) this.pnlProtectedTop);
      this.gcProtectedDocs.Dock = DockStyle.Bottom;
      this.gcProtectedDocs.HeaderForeColor = SystemColors.ControlText;
      this.gcProtectedDocs.Location = new Point(0, 244);
      this.gcProtectedDocs.Name = "gcProtectedDocs";
      this.gcProtectedDocs.Padding = new Padding(5);
      this.gcProtectedDocs.Size = new Size(843, 234);
      this.gcProtectedDocs.TabIndex = 2;
      this.gcProtectedDocs.Text = "Protected Documents";
      this.btnResetProtected.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnResetProtected.BackColor = Color.Transparent;
      this.btnResetProtected.Location = new Point(820, 5);
      this.btnResetProtected.MouseDownImage = (Image) null;
      this.btnResetProtected.Name = "btnResetProtected";
      this.btnResetProtected.Size = new Size(16, 16);
      this.btnResetProtected.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnResetProtected.TabIndex = 8;
      this.btnResetProtected.TabStop = false;
      this.btnResetProtected.Click += new EventHandler(this.btnResetProtected_Click);
      this.btnSaveProtected.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSaveProtected.BackColor = Color.Transparent;
      this.btnSaveProtected.Location = new Point(798, 5);
      this.btnSaveProtected.MouseDownImage = (Image) null;
      this.btnSaveProtected.Name = "btnSaveProtected";
      this.btnSaveProtected.Size = new Size(16, 16);
      this.btnSaveProtected.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSaveProtected.TabIndex = 7;
      this.btnSaveProtected.TabStop = false;
      this.btnSaveProtected.Click += new EventHandler(this.btnSaveProtected_Click);
      this.pnlProtectedBottom.Controls.Add((Control) this.gcProtectedRoles);
      this.pnlProtectedBottom.Dock = DockStyle.Fill;
      this.pnlProtectedBottom.Location = new Point(6, 60);
      this.pnlProtectedBottom.Name = "pnlProtectedBottom";
      this.pnlProtectedBottom.Size = new Size(831, 168);
      this.pnlProtectedBottom.TabIndex = 1;
      this.pnlProtectedBottom.ClientSizeChanged += new EventHandler(this.pnlProtectedBottom_ClientSizeChanged);
      this.gcProtectedRoles.Controls.Add((Control) this.gvProtectedRoles);
      this.gcProtectedRoles.Dock = DockStyle.Left;
      this.gcProtectedRoles.HeaderForeColor = SystemColors.ControlText;
      this.gcProtectedRoles.Location = new Point(0, 0);
      this.gcProtectedRoles.Name = "gcProtectedRoles";
      this.gcProtectedRoles.Size = new Size(412, 168);
      this.gcProtectedRoles.TabIndex = 0;
      this.gcProtectedRoles.Text = "Select a role";
      this.gvProtectedRoles.AllowMultiselect = false;
      this.gvProtectedRoles.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Column";
      gvColumn1.Width = 410;
      this.gvProtectedRoles.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.gvProtectedRoles.Dock = DockStyle.Fill;
      this.gvProtectedRoles.HeaderHeight = 0;
      this.gvProtectedRoles.HeaderVisible = false;
      this.gvProtectedRoles.Location = new Point(1, 26);
      this.gvProtectedRoles.Name = "gvProtectedRoles";
      this.gvProtectedRoles.Size = new Size(410, 141);
      this.gvProtectedRoles.TabIndex = 0;
      this.gvProtectedRoles.SubItemCheck += new GVSubItemEventHandler(this.gvProtectedRoles_SubItemCheck);
      this.pnlProtectedTop.Controls.Add((Control) this.lblProtectedDocs);
      this.pnlProtectedTop.Dock = DockStyle.Top;
      this.pnlProtectedTop.Location = new Point(6, 31);
      this.pnlProtectedTop.Name = "pnlProtectedTop";
      this.pnlProtectedTop.Size = new Size(831, 29);
      this.pnlProtectedTop.TabIndex = 0;
      this.lblProtectedDocs.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblProtectedDocs.Location = new Point(1, 5);
      this.lblProtectedDocs.Name = "lblProtectedDocs";
      this.lblProtectedDocs.Size = new Size(829, 13);
      this.lblProtectedDocs.TabIndex = 0;
      this.lblProtectedDocs.Text = "Once document access is granted to any of the roles selected below, the document becomes protected. Once protected, document cannot be deleted and access ...";
      this.toolTip.SetToolTip((Control) this.lblProtectedDocs, "Once document access is granted to any of the roles selected below, the document becomes protected. Once protected, document cannot be deleted and access cannot be removed from the selected role.");
      this.lblProtectedDocs.SizeChanged += new EventHandler(this.lblProtectedDocs_SizeChanged);
      this.gcDefaultAccess.AutoScroll = true;
      this.gcDefaultAccess.Controls.Add((Control) this.btnResetAccess);
      this.gcDefaultAccess.Controls.Add((Control) this.btnSaveAccess);
      this.gcDefaultAccess.Controls.Add((Control) this.pnlAccessMiddle);
      this.gcDefaultAccess.Controls.Add((Control) this.pnlAccessBottom);
      this.gcDefaultAccess.Controls.Add((Control) this.pnlAccessTop);
      this.gcDefaultAccess.Dock = DockStyle.Fill;
      this.gcDefaultAccess.HeaderForeColor = SystemColors.ControlText;
      this.gcDefaultAccess.Location = new Point(0, 0);
      this.gcDefaultAccess.Name = "gcDefaultAccess";
      this.gcDefaultAccess.Padding = new Padding(5);
      this.gcDefaultAccess.Size = new Size(843, 237);
      this.gcDefaultAccess.TabIndex = 0;
      this.gcDefaultAccess.Text = "Default Access";
      this.btnResetAccess.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnResetAccess.BackColor = Color.Transparent;
      this.btnResetAccess.Location = new Point(820, 5);
      this.btnResetAccess.MouseDownImage = (Image) null;
      this.btnResetAccess.Name = "btnResetAccess";
      this.btnResetAccess.Size = new Size(16, 16);
      this.btnResetAccess.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnResetAccess.TabIndex = 6;
      this.btnResetAccess.TabStop = false;
      this.btnResetAccess.Click += new EventHandler(this.btnResetAccess_Click);
      this.btnSaveAccess.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSaveAccess.BackColor = Color.Transparent;
      this.btnSaveAccess.Location = new Point(798, 5);
      this.btnSaveAccess.MouseDownImage = (Image) null;
      this.btnSaveAccess.Name = "btnSaveAccess";
      this.btnSaveAccess.Size = new Size(16, 16);
      this.btnSaveAccess.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSaveAccess.TabIndex = 5;
      this.btnSaveAccess.TabStop = false;
      this.btnSaveAccess.Click += new EventHandler(this.btnSaveAccess_Click);
      this.pnlAccessMiddle.Controls.Add((Control) this.gcAllowAccess);
      this.pnlAccessMiddle.Controls.Add((Control) this.splDefaultAccess);
      this.pnlAccessMiddle.Controls.Add((Control) this.gcAddedBy);
      this.pnlAccessMiddle.Dock = DockStyle.Fill;
      this.pnlAccessMiddle.Location = new Point(6, 60);
      this.pnlAccessMiddle.Name = "pnlAccessMiddle";
      this.pnlAccessMiddle.Size = new Size(831, 142);
      this.pnlAccessMiddle.TabIndex = 1;
      this.pnlAccessMiddle.ClientSizeChanged += new EventHandler(this.pnlAccessMiddle_ClientSizeChanged);
      this.gcAllowAccess.Controls.Add((Control) this.gvAllowAccess);
      this.gcAllowAccess.Dock = DockStyle.Fill;
      this.gcAllowAccess.HeaderForeColor = SystemColors.ControlText;
      this.gcAllowAccess.Location = new Point(419, 0);
      this.gcAllowAccess.Name = "gcAllowAccess";
      this.gcAllowAccess.Size = new Size(412, 142);
      this.gcAllowAccess.TabIndex = 2;
      this.gcAllowAccess.Text = "The following role can access the document";
      this.gvAllowAccess.AllowMultiselect = false;
      this.gvAllowAccess.BorderStyle = BorderStyle.None;
      gvColumn2.CheckBoxes = true;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Column";
      gvColumn2.Width = 410;
      this.gvAllowAccess.Columns.AddRange(new GVColumn[1]
      {
        gvColumn2
      });
      this.gvAllowAccess.Dock = DockStyle.Fill;
      this.gvAllowAccess.HeaderHeight = 0;
      this.gvAllowAccess.HeaderVisible = false;
      this.gvAllowAccess.Location = new Point(1, 26);
      this.gvAllowAccess.Name = "gvAllowAccess";
      this.gvAllowAccess.Size = new Size(410, 115);
      this.gvAllowAccess.TabIndex = 0;
      this.gvAllowAccess.SubItemCheck += new GVSubItemEventHandler(this.gvAllowAccess_SubItemCheck);
      this.splDefaultAccess.AnimationDelay = 20;
      this.splDefaultAccess.AnimationStep = 20;
      this.splDefaultAccess.BorderStyle3D = Border3DStyle.Flat;
      this.splDefaultAccess.ControlToHide = (Control) this.gcAddedBy;
      this.splDefaultAccess.ExpandParentForm = false;
      this.splDefaultAccess.Location = new Point(412, 0);
      this.splDefaultAccess.Name = "collapsibleSplitter2";
      this.splDefaultAccess.TabIndex = 1;
      this.splDefaultAccess.TabStop = false;
      this.splDefaultAccess.UseAnimations = false;
      this.splDefaultAccess.VisualStyle = VisualStyles.Encompass;
      this.gcAddedBy.Controls.Add((Control) this.gvAddedBy);
      this.gcAddedBy.Dock = DockStyle.Left;
      this.gcAddedBy.HeaderForeColor = SystemColors.ControlText;
      this.gcAddedBy.Location = new Point(0, 0);
      this.gcAddedBy.Name = "gcAddedBy";
      this.gcAddedBy.Size = new Size(412, 142);
      this.gcAddedBy.TabIndex = 0;
      this.gcAddedBy.Text = "If a document is added by";
      this.gvAddedBy.AllowMultiselect = false;
      this.gvAddedBy.BorderStyle = BorderStyle.None;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Column";
      gvColumn3.Width = 410;
      this.gvAddedBy.Columns.AddRange(new GVColumn[1]
      {
        gvColumn3
      });
      this.gvAddedBy.Dock = DockStyle.Fill;
      this.gvAddedBy.HeaderHeight = 0;
      this.gvAddedBy.HeaderVisible = false;
      this.gvAddedBy.Location = new Point(1, 26);
      this.gvAddedBy.Name = "gvAddedBy";
      this.gvAddedBy.Size = new Size(410, 115);
      this.gvAddedBy.TabIndex = 0;
      this.gvAddedBy.SelectedIndexChanged += new EventHandler(this.gvAddedBy_SelectedIndexChanged);
      this.pnlAccessBottom.Controls.Add((Control) this.chkDefaultRole);
      this.pnlAccessBottom.Dock = DockStyle.Bottom;
      this.pnlAccessBottom.Location = new Point(6, 202);
      this.pnlAccessBottom.Name = "pnlAccessBottom";
      this.pnlAccessBottom.Size = new Size(831, 29);
      this.pnlAccessBottom.TabIndex = 2;
      this.chkDefaultRole.AutoSize = true;
      this.chkDefaultRole.Location = new Point(2, 8);
      this.chkDefaultRole.Name = "chkDefaultRole";
      this.chkDefaultRole.Size = new Size(406, 17);
      this.chkDefaultRole.TabIndex = 0;
      this.chkDefaultRole.Text = "Apply these settings to the role even when the role is not assigned to the loan file";
      this.chkDefaultRole.UseVisualStyleBackColor = true;
      this.chkDefaultRole.Click += new EventHandler(this.chkDefaultRole_Click);
      this.pnlAccessTop.Controls.Add((Control) this.lblDefaultAccess);
      this.pnlAccessTop.Dock = DockStyle.Top;
      this.pnlAccessTop.Location = new Point(6, 31);
      this.pnlAccessTop.Name = "pnlAccessTop";
      this.pnlAccessTop.Size = new Size(831, 29);
      this.pnlAccessTop.TabIndex = 0;
      this.lblDefaultAccess.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblDefaultAccess.Location = new Point(1, 5);
      this.lblDefaultAccess.Name = "lblDefaultAccess";
      this.lblDefaultAccess.Size = new Size(829, 13);
      this.lblDefaultAccess.TabIndex = 0;
      this.lblDefaultAccess.Text = "Set up default document access for each role. The Others group contains users who are not assigned to a role. The default settings can be overridden using the ac...";
      this.lblDefaultAccess.TextAlign = ContentAlignment.MiddleLeft;
      this.toolTip.SetToolTip((Control) this.lblDefaultAccess, "Set up default document access for each role. The Others group contains users who are not assigned to a role. The default settings can be overridden using the access button in the eFolder.");
      this.lblDefaultAccess.SizeChanged += new EventHandler(this.lblDefaultAccess_SizeChanged);
      this.splDocumentAccess.AnimationDelay = 20;
      this.splDocumentAccess.AnimationStep = 20;
      this.splDocumentAccess.BorderStyle3D = Border3DStyle.Flat;
      this.splDocumentAccess.ControlToHide = (Control) this.gcProtectedDocs;
      this.splDocumentAccess.Dock = DockStyle.Bottom;
      this.splDocumentAccess.ExpandParentForm = false;
      this.splDocumentAccess.Location = new Point(0, 237);
      this.splDocumentAccess.Name = "collapsibleSplitter1";
      this.splDocumentAccess.TabIndex = 1;
      this.splDocumentAccess.TabStop = false;
      this.splDocumentAccess.UseAnimations = false;
      this.splDocumentAccess.VisualStyle = VisualStyles.Encompass;
      this.Controls.Add((Control) this.gcDefaultAccess);
      this.Controls.Add((Control) this.splDocumentAccess);
      this.Controls.Add((Control) this.gcProtectedDocs);
      this.Name = nameof (DocumentAccessPanel);
      this.Size = new Size(843, 478);
      this.ClientSizeChanged += new EventHandler(this.DocumentAccessPanel_ClientSizeChanged);
      this.gcProtectedDocs.ResumeLayout(false);
      ((ISupportInitialize) this.btnResetProtected).EndInit();
      ((ISupportInitialize) this.btnSaveProtected).EndInit();
      this.pnlProtectedBottom.ResumeLayout(false);
      this.gcProtectedRoles.ResumeLayout(false);
      this.pnlProtectedTop.ResumeLayout(false);
      this.gcDefaultAccess.ResumeLayout(false);
      ((ISupportInitialize) this.btnResetAccess).EndInit();
      ((ISupportInitialize) this.btnSaveAccess).EndInit();
      this.pnlAccessMiddle.ResumeLayout(false);
      this.gcAllowAccess.ResumeLayout(false);
      this.gcAddedBy.ResumeLayout(false);
      this.pnlAccessBottom.ResumeLayout(false);
      this.pnlAccessBottom.PerformLayout();
      this.pnlAccessTop.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void gvAddedBy_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.gvAllowAccess.BeginUpdate();
      this.gvAllowAccess.Items.Clear();
      if (this.gvAddedBy.SelectedItems.Count > 0)
      {
        DocumentDefaultAccessRuleInfo tag = (DocumentDefaultAccessRuleInfo) this.gvAddedBy.SelectedItems[0].Tag;
        foreach (RoleInfo roleInfo in new List<RoleInfo>((IEnumerable<RoleInfo>) this.roleList)
        {
          RoleInfo.Others
        })
        {
          GVItem gvItem = this.gvAllowAccess.Items.Add(roleInfo.Name);
          gvItem.Checked = tag.RolesAllowedAccess.Contains(roleInfo.RoleID);
          gvItem.Tag = (object) roleInfo;
        }
      }
      this.gvAllowAccess.EndUpdate();
      this.gcAllowAccess.Enabled = this.gvAddedBy.SelectedItems.Count <= 1;
    }

    private void gvAllowAccess_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      DocumentDefaultAccessRuleInfo tag1 = (DocumentDefaultAccessRuleInfo) this.gvAddedBy.SelectedItems[0].Tag;
      RoleInfo tag2 = (RoleInfo) e.SubItem.Item.Tag;
      if (e.SubItem.Checked)
        tag1.RolesAllowedAccess.Add(tag2.RoleID);
      else
        tag1.RolesAllowedAccess.Remove(tag2.RoleID);
      this.setAccessDirty(true);
    }

    private void chkDefaultRole_Click(object sender, EventArgs e) => this.setAccessDirty(true);

    private void btnSaveAccess_Click(object sender, EventArgs e) => this.saveDocumentAccess();

    private void btnResetAccess_Click(object sender, EventArgs e) => this.resetDocumentAccess();

    public void resetDocumentAccess()
    {
      DocumentDefaultAccessRuleInfo[] defaultAccessRules = this.docAccessMgr.GetDocumentDefaultAccessRules();
      IDictionary serverSettings = this.session.ServerManager.GetServerSettings("Policies");
      this.gvAddedBy.Items.Clear();
      foreach (RoleInfo roleInfo in new List<RoleInfo>((IEnumerable<RoleInfo>) this.roleList)
      {
        RoleInfo.Others
      })
      {
        DocumentDefaultAccessRuleInfo defaultAccessRuleInfo1 = (DocumentDefaultAccessRuleInfo) null;
        foreach (DocumentDefaultAccessRuleInfo defaultAccessRuleInfo2 in defaultAccessRules)
        {
          if (defaultAccessRuleInfo2.RoleAddedBy == roleInfo.RoleID)
            defaultAccessRuleInfo1 = defaultAccessRuleInfo2.Clone();
        }
        if (defaultAccessRuleInfo1 == null)
          defaultAccessRuleInfo1 = new DocumentDefaultAccessRuleInfo(roleInfo.RoleID);
        this.gvAddedBy.Items.Add(roleInfo.Name).Tag = (object) defaultAccessRuleInfo1;
      }
      this.chkDefaultRole.Checked = serverSettings.Contains((object) "Policies.ApplyDefinedAccessRoles") && (bool) serverSettings[(object) "Policies.ApplyDefinedAccessRoles"];
      this.setAccessDirty(false);
    }

    private void saveDocumentAccess()
    {
      List<DocumentDefaultAccessRuleInfo> defaultAccessRuleInfoList = new List<DocumentDefaultAccessRuleInfo>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAddedBy.Items)
        defaultAccessRuleInfoList.Add(gvItem.Tag as DocumentDefaultAccessRuleInfo);
      this.docAccessMgr.SaveDocumentDefaultAccessRules(defaultAccessRuleInfoList.ToArray());
      this.session.ServerManager.UpdateServerSettings((IDictionary) new Hashtable()
      {
        {
          (object) "Policies.ApplyDefinedAccessRoles",
          (object) this.chkDefaultRole.Checked
        }
      }, AclFeature.SettingsTab_RoleAccesstoDocuments);
      this.setAccessDirty(false);
    }

    private void setAccessDirty(bool dirty)
    {
      this.isAccessDirty = dirty;
      this.btnSaveAccess.Enabled = dirty;
      this.btnResetAccess.Enabled = dirty;
      if (dirty || this.isProtectedDirty)
        this.setDirtyFlag(true);
      else
        this.setDirtyFlag(false);
    }

    private void gvProtectedRoles_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.setProtectedDirty(true);
    }

    private void btnSaveProtected_Click(object sender, EventArgs e) => this.saveProtectedRoles();

    private void btnResetProtected_Click(object sender, EventArgs e) => this.resetProtectedRoles();

    public void resetProtectedRoles()
    {
      this.gvProtectedRoles.BeginUpdate();
      this.gvProtectedRoles.Items.Clear();
      foreach (RoleInfo role in this.roleList)
      {
        GVItem gvItem = this.gvProtectedRoles.Items.Add(role.RoleName);
        gvItem.Checked = role.Protected;
        gvItem.Tag = (object) role;
      }
      this.gvProtectedRoles.EndUpdate();
      this.setProtectedDirty(false);
    }

    private void saveProtectedRoles()
    {
      this.roleList = this.workflowMgr.GetAllRoleFunctions();
      foreach (RoleInfo role in this.roleList)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvProtectedRoles.Items)
        {
          if (((RoleSummaryInfo) gvItem.Tag).RoleID == role.RoleID && role.Protected != gvItem.Checked)
          {
            role.Protected = gvItem.Checked;
            this.workflowMgr.SetRoleFunction(role);
          }
        }
      }
      this.setProtectedDirty(false);
    }

    private void setProtectedDirty(bool dirty)
    {
      this.isProtectedDirty = dirty;
      this.btnSaveProtected.Enabled = dirty;
      this.btnResetProtected.Enabled = dirty;
      if (dirty || this.isAccessDirty)
        this.setDirtyFlag(true);
      else
        this.setDirtyFlag(false);
    }

    public override void Save()
    {
      if (this.isAccessDirty)
        this.saveDocumentAccess();
      if (!this.isProtectedDirty)
        return;
      this.saveProtectedRoles();
    }

    public override void Reset()
    {
      if (this.isAccessDirty)
        this.resetDocumentAccess();
      if (!this.isProtectedDirty)
        return;
      this.resetProtectedRoles();
    }

    private void DocumentAccessPanel_ClientSizeChanged(object sender, EventArgs e)
    {
      this.gcProtectedDocs.Height = (this.ClientSize.Height - this.splDocumentAccess.Height) / 2;
    }

    private void pnlAccessMiddle_ClientSizeChanged(object sender, EventArgs e)
    {
      this.gcAddedBy.Width = (this.pnlAccessMiddle.ClientSize.Width - this.splDefaultAccess.Width) / 2;
    }

    private void pnlProtectedBottom_ClientSizeChanged(object sender, EventArgs e)
    {
      this.gcProtectedRoles.Width = (this.pnlProtectedBottom.ClientSize.Width - this.splDefaultAccess.Width) / 2;
    }

    private void lblDefaultAccess_SizeChanged(object sender, EventArgs e)
    {
      string caption = "Set up default document access for each role. The Others group contains users who are not assigned to a role. The default settings can be overridden using the access button in the eFolder.";
      this.lblDefaultAccess.Text = caption;
      if (this.lblDefaultAccess.Text.IndexOf("...") >= 0)
        this.toolTip.SetToolTip((Control) this.lblDefaultAccess, caption);
      else
        this.toolTip.SetToolTip((Control) this.lblDefaultAccess, string.Empty);
    }

    private void lblProtectedDocs_SizeChanged(object sender, EventArgs e)
    {
      string caption = "Once document access is granted to any of the roles selected below, the document becomes protected. Once protected, document cannot be deleted and access cannot be removed from the selected role.";
      this.lblProtectedDocs.Text = caption;
      if (this.lblProtectedDocs.Text.IndexOf("...") >= 0)
        this.toolTip.SetToolTip((Control) this.lblProtectedDocs, caption);
      else
        this.toolTip.SetToolTip((Control) this.lblProtectedDocs, string.Empty);
    }
  }
}

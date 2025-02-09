// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.OrgSettingsRptCreate
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite
{
  public class OrgSettingsRptCreate : UserControl
  {
    private IContainer components;
    private Panel reportInfoPnl;
    private Panel reportGenPnl;
    private GroupContainer groupContainer1;
    private Label label3;
    private RadioButton excDisabledUsers;
    private RadioButton incDisabledUsers;
    private CheckBox userDetailsAll;
    private CheckBox userDetailsByOrg;
    private CheckBox userLicensesAll;
    private CheckBox orgDetailsChkBx;
    private CheckBox orgLicenseChkBx;
    private Label label2;
    private TextBox orgName;
    private CheckBox subOrgChkbx;
    private Label label4;
    private TextBox reportName;
    private Label label1;
    private GroupContainer gcOrg;
    private HierarchyTree hierarchyTree;
    private ImageList imgListTv;
    public string selectedOrgID;
    private Sessions.Session session;

    public OrgSettingsRptCreate(
      Sessions.Session session,
      string userid,
      SettingsRptJobInfo jobinfo)
    {
      this.InitializeComponent();
      this.session = session;
      this.hierarchyTree.SetSession(this.session);
      int orgId = this.session.UserInfo.OrgId;
      this.hierarchyTree.ImageList = this.imgListTv;
      if (jobinfo == null)
      {
        this.hierarchyTree.RootNodes(orgId);
      }
      else
      {
        int int16 = (int) Convert.ToInt16(jobinfo.reportFilters[0].ToString());
        if (this.checkAccessible(int16, userid))
        {
          this.hierarchyTree.RootNodes(int16);
          this.orgName.Text = this.session.OrganizationManager.GetOrganization(int16).OrgName;
          this.selectedOrgID = int16.ToString();
        }
        else
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You do not have the permission required to run this report for the top-level organization that was used in the original report. \n\nThe selected top-level organization has been changed from the original report based on your permission level set by the administrator. \n\nPlease review the selection before proceeding.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.hierarchyTree.RootNodes(orgId);
          this.orgName.Text = this.session.OrganizationManager.GetOrganization(orgId).OrgName;
          this.selectedOrgID = orgId.ToString();
        }
        this.reportName.Text = "CopyOf_" + jobinfo.ReportName;
        this.orgDetailsChkBx.Checked = jobinfo.reportParameters.ContainsKey("OrganizationDetails") && Convert.ToBoolean(jobinfo.reportParameters["OrganizationDetails"]);
        this.orgLicenseChkBx.Checked = jobinfo.reportParameters.ContainsKey("OrganizationLicenses") && Convert.ToBoolean(jobinfo.reportParameters["OrganizationLicenses"]);
        this.userDetailsAll.Checked = jobinfo.reportParameters.ContainsKey("UserDetailsAll") && Convert.ToBoolean(jobinfo.reportParameters["UserDetailsAll"]);
        this.userDetailsByOrg.Checked = jobinfo.reportParameters.ContainsKey("UserDetailsByOrg") && Convert.ToBoolean(jobinfo.reportParameters["UserDetailsByOrg"]);
        this.userLicensesAll.Checked = jobinfo.reportParameters.ContainsKey("UserLicenses") && Convert.ToBoolean(jobinfo.reportParameters["UserLicenses"]);
        this.incDisabledUsers.Checked = jobinfo.reportParameters.ContainsKey("IncludeDisabledUser") && Convert.ToBoolean(jobinfo.reportParameters["IncludeDisabledUser"]);
        this.subOrgChkbx.Checked = jobinfo.reportParameters.ContainsKey("IncludeSubOrganization") && Convert.ToBoolean(jobinfo.reportParameters["IncludeSubOrganization"]);
      }
    }

    private bool checkAccessible(int orgID, string userID)
    {
      UserInfo user = this.session.OrganizationManager.GetUser(userID);
      int[] descendentsOfOrg = this.session.OrganizationManager.GetDescendentsOfOrg(user.OrgId);
      if (user.OrgId == orgID)
        return true;
      foreach (int num in descendentsOfOrg)
      {
        if (num == orgID)
          return true;
      }
      return false;
    }

    public string OrgName => this.orgName.Text;

    public string ReportName => this.reportName.Text;

    public bool subOrg => this.subOrgChkbx.Checked;

    public bool orgDetail => this.orgDetailsChkBx.Checked;

    public bool orgLicense => this.orgLicenseChkBx.Checked;

    public bool userDetails => this.userDetailsAll.Checked;

    public bool userDetailsOrg => this.userDetailsByOrg.Checked;

    public bool userLicenses => this.userLicensesAll.Checked;

    public bool incDisabledUser => this.incDisabledUsers.Checked;

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (OrgSettingsRptCreate));
      this.reportInfoPnl = new Panel();
      this.gcOrg = new GroupContainer();
      this.hierarchyTree = new HierarchyTree();
      this.reportGenPnl = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.label3 = new Label();
      this.excDisabledUsers = new RadioButton();
      this.incDisabledUsers = new RadioButton();
      this.userDetailsAll = new CheckBox();
      this.userDetailsByOrg = new CheckBox();
      this.userLicensesAll = new CheckBox();
      this.orgDetailsChkBx = new CheckBox();
      this.orgLicenseChkBx = new CheckBox();
      this.label2 = new Label();
      this.orgName = new TextBox();
      this.subOrgChkbx = new CheckBox();
      this.label4 = new Label();
      this.reportName = new TextBox();
      this.label1 = new Label();
      this.imgListTv = new ImageList(this.components);
      this.reportInfoPnl.SuspendLayout();
      this.gcOrg.SuspendLayout();
      this.reportGenPnl.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.reportInfoPnl.Controls.Add((Control) this.gcOrg);
      this.reportInfoPnl.Location = new Point(3, 2);
      this.reportInfoPnl.Name = "reportInfoPnl";
      this.reportInfoPnl.Size = new Size(523, 550);
      this.reportInfoPnl.TabIndex = 11;
      this.gcOrg.AutoScroll = true;
      this.gcOrg.Controls.Add((Control) this.hierarchyTree);
      this.gcOrg.Dock = DockStyle.Fill;
      this.gcOrg.HeaderForeColor = SystemColors.ControlText;
      this.gcOrg.Location = new Point(0, 0);
      this.gcOrg.Name = "gcOrg";
      this.gcOrg.Size = new Size(523, 550);
      this.gcOrg.TabIndex = 8;
      this.gcOrg.Text = "1. Choose Organization";
      this.hierarchyTree.AllowDrop = true;
      this.hierarchyTree.BorderStyle = BorderStyle.None;
      this.hierarchyTree.Cursor = Cursors.Default;
      this.hierarchyTree.Dock = DockStyle.Fill;
      this.hierarchyTree.FullRowSelect = true;
      this.hierarchyTree.HideSelection = false;
      this.hierarchyTree.Location = new Point(1, 26);
      this.hierarchyTree.Name = "hierarchyTree";
      this.hierarchyTree.Size = new Size(521, 442);
      this.hierarchyTree.Sorted = true;
      this.hierarchyTree.TabIndex = 0;
      this.hierarchyTree.AfterSelect += new TreeViewEventHandler(this.hierarchyTree_AfterSelect);
      this.reportGenPnl.Controls.Add((Control) this.groupContainer1);
      this.reportGenPnl.Location = new Point(532, 3);
      this.reportGenPnl.Name = "reportGenPnl";
      this.reportGenPnl.Size = new Size(380, 550);
      this.reportGenPnl.TabIndex = 12;
      this.groupContainer1.BackColor = Color.White;
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.excDisabledUsers);
      this.groupContainer1.Controls.Add((Control) this.incDisabledUsers);
      this.groupContainer1.Controls.Add((Control) this.userDetailsAll);
      this.groupContainer1.Controls.Add((Control) this.userDetailsByOrg);
      this.groupContainer1.Controls.Add((Control) this.userLicensesAll);
      this.groupContainer1.Controls.Add((Control) this.orgDetailsChkBx);
      this.groupContainer1.Controls.Add((Control) this.orgLicenseChkBx);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.orgName);
      this.groupContainer1.Controls.Add((Control) this.subOrgChkbx);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.reportName);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(380, 469);
      this.groupContainer1.TabIndex = 9;
      this.groupContainer1.Text = "2. Select Report Options";
      this.label3.AutoSize = true;
      this.label3.ForeColor = SystemColors.ButtonShadow;
      this.label3.Location = new Point(30, 269);
      this.label3.Name = "label3";
      this.label3.Size = new Size(280, 17);
      this.label3.TabIndex = 17;
      this.label3.Text = "__________________________________";
      this.excDisabledUsers.AutoSize = true;
      this.excDisabledUsers.Checked = true;
      this.excDisabledUsers.Location = new Point(30, 391);
      this.excDisabledUsers.Name = "excDisabledUsers";
      this.excDisabledUsers.Size = new Size(178, 21);
      this.excDisabledUsers.TabIndex = 15;
      this.excDisabledUsers.TabStop = true;
      this.excDisabledUsers.Text = "Exclude Disabled Users";
      this.excDisabledUsers.UseVisualStyleBackColor = true;
      this.incDisabledUsers.AutoSize = true;
      this.incDisabledUsers.Location = new Point(30, 418);
      this.incDisabledUsers.Name = "incDisabledUsers";
      this.incDisabledUsers.Size = new Size(174, 21);
      this.incDisabledUsers.TabIndex = 16;
      this.incDisabledUsers.Text = "Include Disabled Users";
      this.incDisabledUsers.UseVisualStyleBackColor = true;
      this.userDetailsAll.AutoSize = true;
      this.userDetailsAll.Location = new Point(33, 325);
      this.userDetailsAll.Name = "userDetailsAll";
      this.userDetailsAll.Size = new Size(126, 21);
      this.userDetailsAll.TabIndex = 13;
      this.userDetailsAll.Text = "All User Details";
      this.userDetailsAll.UseVisualStyleBackColor = true;
      this.userDetailsByOrg.AutoSize = true;
      this.userDetailsByOrg.Location = new Point(33, 297);
      this.userDetailsByOrg.Name = "userDetailsByOrg";
      this.userDetailsByOrg.Size = new Size(211, 21);
      this.userDetailsByOrg.TabIndex = 12;
      this.userDetailsByOrg.Text = "User Details by Organization";
      this.userDetailsByOrg.UseVisualStyleBackColor = true;
      this.userLicensesAll.AutoSize = true;
      this.userLicensesAll.Location = new Point(33, 353);
      this.userLicensesAll.Name = "userLicensesAll";
      this.userLicensesAll.Size = new Size(139, 21);
      this.userLicensesAll.TabIndex = 14;
      this.userLicensesAll.Text = "All User Licenses";
      this.userLicensesAll.UseVisualStyleBackColor = true;
      this.orgDetailsChkBx.AutoSize = true;
      this.orgDetailsChkBx.Checked = true;
      this.orgDetailsChkBx.CheckState = CheckState.Checked;
      this.orgDetailsChkBx.Location = new Point(33, 218);
      this.orgDetailsChkBx.Name = "orgDetailsChkBx";
      this.orgDetailsChkBx.Size = new Size(158, 21);
      this.orgDetailsChkBx.TabIndex = 8;
      this.orgDetailsChkBx.Text = "Organization Details";
      this.orgDetailsChkBx.UseVisualStyleBackColor = true;
      this.orgLicenseChkBx.AutoSize = true;
      this.orgLicenseChkBx.Location = new Point(33, 245);
      this.orgLicenseChkBx.Name = "orgLicenseChkBx";
      this.orgLicenseChkBx.Size = new Size(171, 21);
      this.orgLicenseChkBx.TabIndex = 9;
      this.orgLicenseChkBx.Text = "Organization Licenses";
      this.orgLicenseChkBx.UseVisualStyleBackColor = true;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(19, 101);
      this.label2.Name = "label2";
      this.label2.Size = new Size(180, 17);
      this.label2.TabIndex = 4;
      this.label2.Text = "Organization Report for";
      this.orgName.BackColor = SystemColors.Control;
      this.orgName.Location = new Point(21, 122);
      this.orgName.Name = "orgName";
      this.orgName.ReadOnly = true;
      this.orgName.Size = new Size(325, 22);
      this.orgName.TabIndex = 5;
      this.subOrgChkbx.AutoSize = true;
      this.subOrgChkbx.Location = new Point(51, 150);
      this.subOrgChkbx.Name = "subOrgChkbx";
      this.subOrgChkbx.Size = new Size(248, 21);
      this.subOrgChkbx.TabIndex = 6;
      this.subOrgChkbx.Text = "Include Subordinate Organizations";
      this.subOrgChkbx.UseVisualStyleBackColor = true;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(19, 191);
      this.label4.Name = "label4";
      this.label4.Size = new Size(197, 17);
      this.label4.TabIndex = 7;
      this.label4.Text = "Include the following tabs:";
      this.reportName.Location = new Point(21, 61);
      this.reportName.Name = "reportName";
      this.reportName.Size = new Size(325, 22);
      this.reportName.TabIndex = 2;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(18, 40);
      this.label1.Name = "label1";
      this.label1.Size = new Size(103, 17);
      this.label1.TabIndex = 1;
      this.label1.Text = "Report Name";
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.Transparent;
      this.imgListTv.Images.SetKeyName(0, "folder.bmp");
      this.imgListTv.Images.SetKeyName(1, "folder-open.bmp");
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.reportGenPnl);
      this.Controls.Add((Control) this.reportInfoPnl);
      this.Name = nameof (OrgSettingsRptCreate);
      this.Size = new Size(914, 556);
      this.Load += new EventHandler(this.OrgSettingsRptCreate_Load);
      this.reportInfoPnl.ResumeLayout(false);
      this.gcOrg.ResumeLayout(false);
      this.reportGenPnl.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void hierarchyTree_AfterSelect(object sender, TreeViewEventArgs e)
    {
      TreeNode selectedNode = this.hierarchyTree.SelectedNode;
      if (selectedNode == null)
        return;
      OrgNodeTag tag = (OrgNodeTag) this.hierarchyTree.SelectedNode.Tag;
      this.orgName.Text = selectedNode.Text;
      this.selectedOrgID = tag.Oid.ToString();
    }

    private void OrgSettingsRptCreate_Load(object sender, EventArgs e)
    {
    }
  }
}

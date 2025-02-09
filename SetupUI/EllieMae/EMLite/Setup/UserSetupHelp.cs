// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.UserSetupHelp
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class UserSetupHelp : SetupHelp
  {
    private Label labItemTitle;
    private Label labItemSummary;
    private IContainer components;
    private Label lblOrgUserSubTitle;
    private Label lblUserGroupSubTitle;
    private Label lblLOSettingSubTitle;
    private Label lblLPSettingSubTitle;
    private Label lblOrgUserSummary;
    private Label lblLOSettingSummary;
    private Label lblLPSettingSummary;
    private Label lblUserGroupSummary;
    private SetUpContainer setUpContainer;

    public UserSetupHelp(SetUpContainer setUpContainer)
    {
      this.setUpContainer = setUpContainer;
      this.InitializeComponent();
      this.labItemTitle.BackColor = this.defaultBackColor;
      this.labItemTitle.Font = this.defaultItemTitleFont;
      this.labItemSummary.BackColor = this.defaultBackColor;
      this.lblLOSettingSubTitle.Font = this.defaultSubItemTitleFont;
      this.lblLOSettingSubTitle.ForeColor = this.defaultSubItemTitleForeColor;
      this.lblLOSettingSubTitle.BackColor = this.defaultBackColor;
      this.lblLOSettingSummary.BackColor = this.defaultBackColor;
      this.lblLPSettingSubTitle.Font = this.defaultSubItemTitleFont;
      this.lblLPSettingSubTitle.ForeColor = this.defaultSubItemTitleForeColor;
      this.lblLPSettingSubTitle.BackColor = this.defaultBackColor;
      this.lblLPSettingSummary.BackColor = this.defaultBackColor;
      this.lblOrgUserSubTitle.Font = this.defaultSubItemTitleFont;
      this.lblOrgUserSubTitle.ForeColor = this.defaultSubItemTitleForeColor;
      this.lblOrgUserSubTitle.BackColor = this.defaultBackColor;
      this.lblOrgUserSummary.BackColor = this.defaultBackColor;
      this.lblUserGroupSubTitle.Font = this.defaultSubItemTitleFont;
      this.lblUserGroupSubTitle.ForeColor = this.defaultSubItemTitleForeColor;
      this.lblUserGroupSubTitle.BackColor = this.defaultBackColor;
      this.lblUserGroupSummary.BackColor = this.defaultBackColor;
      UserInfo userInfo = Session.UserInfo;
      OrgInfo organization = Session.OrganizationManager.GetOrganization(userInfo.OrgId);
      if (organization.Oid == organization.Parent && userInfo.IsAdministrator())
        return;
      this.lblUserGroupSubTitle.Visible = false;
      this.lblUserGroupSummary.Visible = false;
      this.lblLPSettingSubTitle.Visible = false;
      this.lblLPSettingSummary.Visible = false;
      this.lblLOSettingSubTitle.Visible = false;
      this.lblLOSettingSummary.Visible = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.labItemTitle = new Label();
      this.labItemSummary = new Label();
      this.lblOrgUserSummary = new Label();
      this.lblOrgUserSubTitle = new Label();
      this.lblLOSettingSummary = new Label();
      this.lblLOSettingSubTitle = new Label();
      this.lblLPSettingSummary = new Label();
      this.lblLPSettingSubTitle = new Label();
      this.lblUserGroupSummary = new Label();
      this.lblUserGroupSubTitle = new Label();
      this.SuspendLayout();
      this.labItemTitle.Location = new Point(8, 8);
      this.labItemTitle.Name = "labItemTitle";
      this.labItemTitle.Size = new Size(492, 20);
      this.labItemTitle.TabIndex = 0;
      this.labItemTitle.Text = "User Setup";
      this.labItemSummary.Location = new Point(8, 28);
      this.labItemSummary.Name = "labItemSummary";
      this.labItemSummary.Size = new Size(492, 32);
      this.labItemSummary.TabIndex = 1;
      this.labItemSummary.Text = "Use the User Setup options to manage your organization and user accounts, manage user groups, and configure the access to data for loan officers and loan processors.";
      this.lblOrgUserSummary.Location = new Point(12, 92);
      this.lblOrgUserSummary.Name = "lblOrgUserSummary";
      this.lblOrgUserSummary.Size = new Size(168, 44);
      this.lblOrgUserSummary.TabIndex = 5;
      this.lblOrgUserSummary.Text = "Create and maintain a hierarchy of your organization and user accounts.";
      this.lblOrgUserSubTitle.Cursor = Cursors.Hand;
      this.lblOrgUserSubTitle.Location = new Point(12, 76);
      this.lblOrgUserSubTitle.Name = "lblOrgUserSubTitle";
      this.lblOrgUserSubTitle.Size = new Size(168, 16);
      this.lblOrgUserSubTitle.TabIndex = 4;
      this.lblOrgUserSubTitle.Text = "Organization/User Setup";
      this.lblOrgUserSubTitle.Click += new EventHandler(this.labelHeader_Click);
      this.lblLOSettingSummary.Location = new Point(12, 272);
      this.lblLOSettingSummary.Name = "lblLOSettingSummary";
      this.lblLOSettingSummary.Size = new Size(168, 30);
      this.lblLOSettingSummary.TabIndex = 21;
      this.lblLOSettingSummary.Text = "Specify your loan officers' access to loan data.";
      this.lblLOSettingSubTitle.Cursor = Cursors.Hand;
      this.lblLOSettingSubTitle.Location = new Point(12, 256);
      this.lblLOSettingSubTitle.Name = "lblLOSettingSubTitle";
      this.lblLOSettingSubTitle.Size = new Size(168, 16);
      this.lblLOSettingSubTitle.TabIndex = 20;
      this.lblLOSettingSubTitle.Text = "Loan Officer Settings";
      this.lblLOSettingSubTitle.Click += new EventHandler(this.labelHeader_Click);
      this.lblLPSettingSummary.Location = new Point(12, 332);
      this.lblLPSettingSummary.Name = "lblLPSettingSummary";
      this.lblLPSettingSummary.Size = new Size(168, 30);
      this.lblLPSettingSummary.TabIndex = 23;
      this.lblLPSettingSummary.Text = "Specify your loan processors' access to loan data.";
      this.lblLPSettingSubTitle.Cursor = Cursors.Hand;
      this.lblLPSettingSubTitle.Location = new Point(12, 316);
      this.lblLPSettingSubTitle.Name = "lblLPSettingSubTitle";
      this.lblLPSettingSubTitle.Size = new Size(168, 16);
      this.lblLPSettingSubTitle.TabIndex = 22;
      this.lblLPSettingSubTitle.Text = "Loan Processor Settings";
      this.lblLPSettingSubTitle.Click += new EventHandler(this.labelHeader_Click);
      this.lblUserGroupSummary.Location = new Point(12, 164);
      this.lblUserGroupSummary.Name = "lblUserGroupSummary";
      this.lblUserGroupSummary.Size = new Size(168, 80);
      this.lblUserGroupSummary.TabIndex = 0;
      this.lblUserGroupSummary.Text = "Create groups of loan officers, loan processors, and closers. When you set up a user account, you can specify which user groups are visible to that user.";
      this.lblUserGroupSubTitle.Cursor = Cursors.Hand;
      this.lblUserGroupSubTitle.Location = new Point(12, 148);
      this.lblUserGroupSubTitle.Name = "lblUserGroupSubTitle";
      this.lblUserGroupSubTitle.Size = new Size(168, 16);
      this.lblUserGroupSubTitle.TabIndex = 24;
      this.lblUserGroupSubTitle.Text = "User Groups";
      this.lblUserGroupSubTitle.Click += new EventHandler(this.labelHeader_Click);
      this.Controls.Add((Control) this.lblUserGroupSummary);
      this.Controls.Add((Control) this.lblUserGroupSubTitle);
      this.Controls.Add((Control) this.lblLPSettingSummary);
      this.Controls.Add((Control) this.lblLPSettingSubTitle);
      this.Controls.Add((Control) this.lblLOSettingSummary);
      this.Controls.Add((Control) this.lblLOSettingSubTitle);
      this.Controls.Add((Control) this.lblOrgUserSummary);
      this.Controls.Add((Control) this.lblOrgUserSubTitle);
      this.Controls.Add((Control) this.labItemSummary);
      this.Controls.Add((Control) this.labItemTitle);
      this.Name = nameof (UserSetupHelp);
      this.Size = new Size(476, 432);
      this.ResumeLayout(false);
    }

    private void labelHeader_Click(object sender, EventArgs e)
    {
      Label label = (Label) sender;
      string nodeText = "";
      string name = label.Name;
      if (name == "lblUserGroupSubTitle" || name == "lblOrgUserSubTitle" || name == "lblLOSettingSubTitle" || name == "lblLPSettingSubTitle")
        nodeText = label.Text;
      if (!(nodeText != ""))
        return;
      this.setUpContainer.ShowPage(nodeText);
    }
  }
}

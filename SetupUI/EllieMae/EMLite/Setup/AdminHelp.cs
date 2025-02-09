// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AdminHelp
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AdminHelp : SetupHelp
  {
    private Label labItemTitle;
    private Label labItemSummary;
    private Label lblCurrentLoginTitle;
    private Label lblRebuildPipelineTitle;
    private Label lblLicensedAddOnTitle;
    private Label lblUserInfoTitle;
    private IContainer components;
    private Label lblCurrentLoginSummary;
    private Label lblRebuildPipelineSummary;
    private Label lblLicensedAddOnSummary;
    private Label lblUserInfoSummary;
    private SetUpContainer setUpContainer;

    public AdminHelp(SetUpContainer setUpContainer)
    {
      this.InitializeComponent();
      this.setUpContainer = setUpContainer;
      this.labItemTitle.BackColor = this.defaultBackColor;
      this.labItemTitle.Font = this.defaultItemTitleFont;
      this.labItemSummary.BackColor = this.defaultBackColor;
      this.lblUserInfoTitle.Font = this.defaultSubItemTitleFont;
      this.lblUserInfoTitle.ForeColor = this.defaultSubItemTitleForeColor;
      this.lblUserInfoTitle.BackColor = this.defaultBackColor;
      this.lblUserInfoSummary.BackColor = this.defaultBackColor;
      this.lblRebuildPipelineTitle.Font = this.defaultSubItemTitleFont;
      this.lblRebuildPipelineTitle.ForeColor = this.defaultSubItemTitleForeColor;
      this.lblRebuildPipelineTitle.BackColor = this.defaultBackColor;
      this.lblRebuildPipelineSummary.BackColor = this.defaultBackColor;
      this.lblLicensedAddOnTitle.Font = this.defaultSubItemTitleFont;
      this.lblLicensedAddOnTitle.ForeColor = this.defaultSubItemTitleForeColor;
      this.lblLicensedAddOnTitle.BackColor = this.defaultBackColor;
      this.lblLicensedAddOnSummary.BackColor = this.defaultBackColor;
      this.lblCurrentLoginTitle.Font = this.defaultSubItemTitleFont;
      this.lblCurrentLoginTitle.ForeColor = this.defaultSubItemTitleForeColor;
      this.lblCurrentLoginTitle.BackColor = this.defaultBackColor;
      this.lblCurrentLoginSummary.BackColor = this.defaultBackColor;
      if (Session.RemoteServer != null)
        return;
      this.lblCurrentLoginTitle.Visible = false;
      this.lblCurrentLoginSummary.Visible = false;
      this.lblUserInfoTitle.Visible = false;
      this.lblUserInfoSummary.Visible = false;
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
      this.lblCurrentLoginTitle = new Label();
      this.lblCurrentLoginSummary = new Label();
      this.lblRebuildPipelineSummary = new Label();
      this.lblRebuildPipelineTitle = new Label();
      this.lblLicensedAddOnSummary = new Label();
      this.lblLicensedAddOnTitle = new Label();
      this.lblUserInfoSummary = new Label();
      this.lblUserInfoTitle = new Label();
      this.SuspendLayout();
      this.labItemTitle.Location = new Point(8, 8);
      this.labItemTitle.Name = "labItemTitle";
      this.labItemTitle.Size = new Size(448, 20);
      this.labItemTitle.TabIndex = 0;
      this.labItemTitle.Text = "System Administration";
      this.labItemSummary.Location = new Point(8, 28);
      this.labItemSummary.Name = "labItemSummary";
      this.labItemSummary.Size = new Size(448, 36);
      this.labItemSummary.TabIndex = 1;
      this.labItemSummary.Text = "Use the System Administration options to rebuild pipelines, manage access to licensed add-ons, manage Encompass user information, and manage logged-in users.";
      this.lblCurrentLoginTitle.Location = new Point(204, 72);
      this.lblCurrentLoginTitle.Name = "lblCurrentLoginTitle";
      this.lblCurrentLoginTitle.Size = new Size(168, 16);
      this.lblCurrentLoginTitle.TabIndex = 2;
      this.lblCurrentLoginTitle.Text = "Current Logins";
      this.lblCurrentLoginTitle.Click += new EventHandler(this.labelHeader_Click);
      this.lblCurrentLoginSummary.Location = new Point(204, 88);
      this.lblCurrentLoginSummary.Name = "lblCurrentLoginSummary";
      this.lblCurrentLoginSummary.Size = new Size(168, 60);
      this.lblCurrentLoginSummary.TabIndex = 3;
      this.lblCurrentLoginSummary.Text = "View and manage logged-in users: check status, send messages, and disable/enable Encompass logins.";
      this.lblRebuildPipelineSummary.Location = new Point(16, 88);
      this.lblRebuildPipelineSummary.Name = "lblRebuildPipelineSummary";
      this.lblRebuildPipelineSummary.Size = new Size(168, 44);
      this.lblRebuildPipelineSummary.TabIndex = 31;
      this.lblRebuildPipelineSummary.Text = "Synchronize the contents of loan folders with the folders on the server.";
      this.lblRebuildPipelineTitle.Cursor = Cursors.Hand;
      this.lblRebuildPipelineTitle.Location = new Point(16, 72);
      this.lblRebuildPipelineTitle.Name = "lblRebuildPipelineTitle";
      this.lblRebuildPipelineTitle.Size = new Size(168, 16);
      this.lblRebuildPipelineTitle.TabIndex = 30;
      this.lblRebuildPipelineTitle.Text = "Rebuild Pipeline";
      this.lblRebuildPipelineTitle.Click += new EventHandler(this.labelHeader_Click);
      this.lblLicensedAddOnSummary.Location = new Point(16, 156);
      this.lblLicensedAddOnSummary.Name = "lblLicensedAddOnSummary";
      this.lblLicensedAddOnSummary.Size = new Size(168, 84);
      this.lblLicensedAddOnSummary.TabIndex = 33;
      this.lblLicensedAddOnSummary.Text = "Configure and customize your users' access to add-on features such as Contact Synchronization, E-Document Management, Status Online, and Reporting Database.";
      this.lblLicensedAddOnTitle.Cursor = Cursors.Hand;
      this.lblLicensedAddOnTitle.Location = new Point(16, 140);
      this.lblLicensedAddOnTitle.Name = "lblLicensedAddOnTitle";
      this.lblLicensedAddOnTitle.Size = new Size(168, 16);
      this.lblLicensedAddOnTitle.TabIndex = 32;
      this.lblLicensedAddOnTitle.Text = "Licensed Add-Ons";
      this.lblLicensedAddOnTitle.Click += new EventHandler(this.labelHeader_Click);
      this.lblUserInfoSummary.Location = new Point(16, 264);
      this.lblUserInfoSummary.Name = "lblUserInfoSummary";
      this.lblUserInfoSummary.Size = new Size(168, 60);
      this.lblUserInfoSummary.TabIndex = 35;
      this.lblUserInfoSummary.Text = "View and manage all users, send emails, edit user accounts, and locate users in the organization hierarchy.";
      this.lblUserInfoTitle.Cursor = Cursors.Hand;
      this.lblUserInfoTitle.Location = new Point(16, 248);
      this.lblUserInfoTitle.Name = "lblUserInfoTitle";
      this.lblUserInfoTitle.Size = new Size(168, 16);
      this.lblUserInfoTitle.TabIndex = 34;
      this.lblUserInfoTitle.Text = "All User Information";
      this.lblUserInfoTitle.Click += new EventHandler(this.labelHeader_Click);
      this.Controls.Add((Control) this.lblUserInfoSummary);
      this.Controls.Add((Control) this.lblUserInfoTitle);
      this.Controls.Add((Control) this.lblLicensedAddOnSummary);
      this.Controls.Add((Control) this.lblLicensedAddOnTitle);
      this.Controls.Add((Control) this.lblRebuildPipelineSummary);
      this.Controls.Add((Control) this.lblRebuildPipelineTitle);
      this.Controls.Add((Control) this.lblCurrentLoginSummary);
      this.Controls.Add((Control) this.lblCurrentLoginTitle);
      this.Controls.Add((Control) this.labItemSummary);
      this.Controls.Add((Control) this.labItemTitle);
      this.Name = nameof (AdminHelp);
      this.Size = new Size(545, 432);
      this.ResumeLayout(false);
    }

    private void labelHeader_Click(object sender, EventArgs e)
    {
      string name = ((Control) sender).Name;
      string nodeText = "";
      switch (name)
      {
        case "lblCurrentLoginTitle":
          nodeText = "Current Logins";
          break;
        case "lblLicensedAddOnTitle":
          nodeText = "Licensed Add-Ons";
          break;
        case "lblRebuildPipelineTitle":
          nodeText = "Rebuild Pipeline";
          break;
        case "lblUserInfoTitle":
          nodeText = "All User Information";
          break;
      }
      if (!(nodeText != ""))
        return;
      this.setUpContainer.ShowPage(nodeText);
    }
  }
}

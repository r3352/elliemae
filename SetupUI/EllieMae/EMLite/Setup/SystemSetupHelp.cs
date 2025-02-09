// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SystemSetupHelp
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
  public class SystemSetupHelp : SetupHelp
  {
    private Label labItemTitle;
    private Label labItemSummary;
    private IContainer components;
    private Label lblContactSetupSummary;
    private Label lblContactSetupTitle;
    private Label lblLoanSetupSummary;
    private Label lblLoanSetupTitle;
    private Label lblUserSetupSummary;
    private Label lblUserSetupTitle;
    private Label lblCompanySetupSummary;
    private Label lblCompanySetupTitle;
    private SetUpContainer setUpContainer;

    public SystemSetupHelp(SetUpContainer setUpContainer)
    {
      this.setUpContainer = setUpContainer;
      this.InitializeComponent();
      this.labItemTitle.BackColor = this.defaultBackColor;
      this.labItemTitle.Font = this.defaultItemTitleFont;
      this.labItemSummary.BackColor = this.defaultBackColor;
      this.lblContactSetupTitle.Font = this.defaultSubItemTitleFont;
      this.lblContactSetupTitle.ForeColor = this.defaultSubItemTitleForeColor;
      this.lblContactSetupTitle.BackColor = this.defaultBackColor;
      this.lblContactSetupSummary.BackColor = this.defaultBackColor;
      this.lblLoanSetupTitle.Font = this.defaultSubItemTitleFont;
      this.lblLoanSetupTitle.ForeColor = this.defaultSubItemTitleForeColor;
      this.lblLoanSetupTitle.BackColor = this.defaultBackColor;
      this.lblLoanSetupSummary.BackColor = this.defaultBackColor;
      this.lblUserSetupTitle.Font = this.defaultSubItemTitleFont;
      this.lblUserSetupTitle.ForeColor = this.defaultSubItemTitleForeColor;
      this.lblUserSetupTitle.BackColor = this.defaultBackColor;
      this.lblUserSetupSummary.BackColor = this.defaultBackColor;
      this.lblCompanySetupTitle.Font = this.defaultSubItemTitleFont;
      this.lblCompanySetupTitle.ForeColor = this.defaultSubItemTitleForeColor;
      this.lblCompanySetupTitle.BackColor = this.defaultBackColor;
      this.lblCompanySetupSummary.BackColor = this.defaultBackColor;
      UserInfo userInfo = Session.UserInfo;
      OrgInfo organization = Session.OrganizationManager.GetOrganization(userInfo.OrgId);
      if (organization.Oid == organization.Parent && userInfo.IsAdministrator())
        return;
      this.lblContactSetupTitle.Visible = false;
      this.lblContactSetupSummary.Visible = false;
      this.lblLoanSetupTitle.Visible = false;
      this.lblLoanSetupSummary.Visible = false;
      this.lblCompanySetupTitle.Visible = false;
      this.lblCompanySetupSummary.Visible = false;
      this.lblUserSetupTitle.Top = this.lblCompanySetupTitle.Top;
      this.lblUserSetupSummary.Top = this.lblCompanySetupSummary.Top;
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
      this.lblContactSetupSummary = new Label();
      this.lblContactSetupTitle = new Label();
      this.lblLoanSetupSummary = new Label();
      this.lblLoanSetupTitle = new Label();
      this.lblUserSetupSummary = new Label();
      this.lblUserSetupTitle = new Label();
      this.lblCompanySetupSummary = new Label();
      this.lblCompanySetupTitle = new Label();
      this.SuspendLayout();
      this.labItemTitle.Location = new Point(8, 8);
      this.labItemTitle.Name = "labItemTitle";
      this.labItemTitle.Size = new Size(492, 20);
      this.labItemTitle.TabIndex = 0;
      this.labItemTitle.Text = "System Setup";
      this.labItemSummary.Location = new Point(8, 28);
      this.labItemSummary.Name = "labItemSummary";
      this.labItemSummary.Size = new Size(492, 24);
      this.labItemSummary.TabIndex = 1;
      this.labItemSummary.Text = "Use the System Setup options to manage company, user, loan, and contact settings.";
      this.lblContactSetupSummary.Location = new Point(204, 80);
      this.lblContactSetupSummary.Name = "lblContactSetupSummary";
      this.lblContactSetupSummary.Size = new Size(168, 32);
      this.lblContactSetupSummary.TabIndex = 21;
      this.lblContactSetupSummary.Text = "Configure and customize the Contacts module.";
      this.lblContactSetupTitle.Cursor = Cursors.Hand;
      this.lblContactSetupTitle.Location = new Point(204, 64);
      this.lblContactSetupTitle.Name = "lblContactSetupTitle";
      this.lblContactSetupTitle.Size = new Size(168, 16);
      this.lblContactSetupTitle.TabIndex = 20;
      this.lblContactSetupTitle.Text = "Contact Setup";
      this.lblContactSetupTitle.Click += new EventHandler(this.labelHeader_Click);
      this.lblLoanSetupSummary.Font = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.lblLoanSetupSummary.Location = new Point(16, 248);
      this.lblLoanSetupSummary.Name = "lblLoanSetupSummary";
      this.lblLoanSetupSummary.Size = new Size(168, 72);
      this.lblLoanSetupSummary.TabIndex = 27;
      this.lblLoanSetupSummary.Text = "Create loan folders, set up loan numbering system, configure default GFE output forms, and create default RESPA information.";
      this.lblLoanSetupTitle.Cursor = Cursors.Hand;
      this.lblLoanSetupTitle.Font = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.lblLoanSetupTitle.Location = new Point(16, 232);
      this.lblLoanSetupTitle.Name = "lblLoanSetupTitle";
      this.lblLoanSetupTitle.Size = new Size(168, 16);
      this.lblLoanSetupTitle.TabIndex = 26;
      this.lblLoanSetupTitle.Text = "Loan Setup";
      this.lblLoanSetupTitle.Click += new EventHandler(this.labelHeader_Click);
      this.lblUserSetupSummary.Font = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.lblUserSetupSummary.Location = new Point(16, 152);
      this.lblUserSetupSummary.Name = "lblUserSetupSummary";
      this.lblUserSetupSummary.Size = new Size(168, 72);
      this.lblUserSetupSummary.TabIndex = 25;
      this.lblUserSetupSummary.Text = "Manage your organization and user accounts, manage user groups, and configure the access to loan data for loan officers and loan processors.";
      this.lblUserSetupTitle.Cursor = Cursors.Hand;
      this.lblUserSetupTitle.Font = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.lblUserSetupTitle.Location = new Point(16, 136);
      this.lblUserSetupTitle.Name = "lblUserSetupTitle";
      this.lblUserSetupTitle.Size = new Size(168, 16);
      this.lblUserSetupTitle.TabIndex = 24;
      this.lblUserSetupTitle.Text = "User Setup";
      this.lblUserSetupTitle.Click += new EventHandler(this.labelHeader_Click);
      this.lblCompanySetupSummary.Font = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.lblCompanySetupSummary.Location = new Point(16, 80);
      this.lblCompanySetupSummary.Name = "lblCompanySetupSummary";
      this.lblCompanySetupSummary.Size = new Size(168, 48);
      this.lblCompanySetupSummary.TabIndex = 23;
      this.lblCompanySetupSummary.Text = "Manage basic information for your company and change the company password.";
      this.lblCompanySetupTitle.Cursor = Cursors.Hand;
      this.lblCompanySetupTitle.Font = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.lblCompanySetupTitle.Location = new Point(16, 64);
      this.lblCompanySetupTitle.Name = "lblCompanySetupTitle";
      this.lblCompanySetupTitle.Size = new Size(168, 16);
      this.lblCompanySetupTitle.TabIndex = 30;
      this.lblCompanySetupTitle.Text = "Company Setup";
      this.lblCompanySetupTitle.Click += new EventHandler(this.labelHeader_Click);
      this.Controls.Add((Control) this.lblCompanySetupTitle);
      this.Controls.Add((Control) this.lblLoanSetupSummary);
      this.Controls.Add((Control) this.lblLoanSetupTitle);
      this.Controls.Add((Control) this.lblUserSetupSummary);
      this.Controls.Add((Control) this.lblUserSetupTitle);
      this.Controls.Add((Control) this.lblCompanySetupSummary);
      this.Controls.Add((Control) this.lblContactSetupSummary);
      this.Controls.Add((Control) this.lblContactSetupTitle);
      this.Controls.Add((Control) this.labItemSummary);
      this.Controls.Add((Control) this.labItemTitle);
      this.Name = nameof (SystemSetupHelp);
      this.Size = new Size(677, 600);
      this.ResumeLayout(false);
    }

    private void labelHeader_Click(object sender, EventArgs e)
    {
      string name = ((Control) sender).Name;
      string nodeText = "";
      switch (name)
      {
        case "lblCompanySetupTitle":
          nodeText = "Company Setup";
          break;
        case "lblLoanSetupTitle":
          nodeText = "Loan Setup";
          break;
        case "lblContactSetupTitle":
          nodeText = "Contact Setup";
          break;
        case "lblUserSetupTitle":
          nodeText = "User Setup";
          break;
      }
      if (!(nodeText != ""))
        return;
      this.setUpContainer.ShowPage(nodeText);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SettingsRptViewLog
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
  public class SettingsRptViewLog : Form
  {
    private RichTextBox reportTxtBx;
    private Button close_btn;
    private IContainer components;

    public SettingsRptViewLog(string reportID)
    {
      this.InitializeComponent();
      SettingsRptJobInfo settingsRptInfo = Session.ReportManager.GetSettingsRptInfo(reportID);
      settingsRptInfo.reportParameters = Session.ReportManager.GetSettingsReportParameters(reportID);
      settingsRptInfo.reportFilters = Session.ReportManager.GetReportFilters(reportID);
      DateTime dateTime1 = Convert.ToDateTime(settingsRptInfo.CreateDate);
      dateTime1 = dateTime1.ToLocalTime();
      string str1 = dateTime1.ToString();
      this.reportTxtBx.Text = "Report Log \n";
      this.reportTxtBx.AppendText("====================================================\n");
      this.reportTxtBx.AppendText("Report Name: " + settingsRptInfo.ReportName + "\n");
      this.reportTxtBx.AppendText("Requested By: " + settingsRptInfo.CreatedBy + "\n");
      this.reportTxtBx.AppendText("Requested On: " + str1 + "\n");
      this.reportTxtBx.AppendText("Request Type: " + (settingsRptInfo.Type == SettingsRptJobInfo.jobType.UserGroups ? "User Groups" : settingsRptInfo.Type.ToString()) + " Report\n\n");
      this.reportTxtBx.AppendText("Report Options: \n");
      if (settingsRptInfo.Type.Equals((object) SettingsRptJobInfo.jobType.Organization))
      {
        this.reportTxtBx.AppendText("   Include Subordinate Organizations: " + (settingsRptInfo.reportParameters.ContainsKey("IncludeSubOrganization") ? (settingsRptInfo.reportParameters["IncludeSubOrganization"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Organization Details: " + (settingsRptInfo.reportParameters.ContainsKey("OrganizationDetails") ? (settingsRptInfo.reportParameters["OrganizationDetails"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Organization Licenses: " + (settingsRptInfo.reportParameters.ContainsKey("OrganizationLicenses") ? (settingsRptInfo.reportParameters["OrganizationLicenses"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   User Details By Organization: " + (settingsRptInfo.reportParameters.ContainsKey("UserDetailsByOrg") ? (settingsRptInfo.reportParameters["UserDetailsByOrg"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   All User Details: " + (settingsRptInfo.reportParameters.ContainsKey("UserDetailsAll") ? (settingsRptInfo.reportParameters["UserDetailsAll"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   All User Licenses: " + (settingsRptInfo.reportParameters.ContainsKey("UserLicenses") ? (settingsRptInfo.reportParameters["UserLicenses"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Include Disabled Users: " + (settingsRptInfo.reportParameters.ContainsKey("IncludeDisabledUser") ? (settingsRptInfo.reportParameters["IncludeDisabledUser"].Equals("True") ? "Yes" : "No") : "No") + "\n");
      }
      else if (settingsRptInfo.Type.Equals((object) SettingsRptJobInfo.jobType.Persona))
      {
        this.reportTxtBx.AppendText("   Home Page Modules access: " + (settingsRptInfo.reportParameters.ContainsKey("HomePageAccess") ? (settingsRptInfo.reportParameters["HomePageAccess"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Pipeline access: " + (settingsRptInfo.reportParameters.ContainsKey("PipelineAccess") ? (settingsRptInfo.reportParameters["PipelineAccess"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Loan access: " + (settingsRptInfo.reportParameters.ContainsKey("LoansAccess") ? (settingsRptInfo.reportParameters["LoansAccess"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Forms access: " + (settingsRptInfo.reportParameters.ContainsKey("FormsAccess") ? (settingsRptInfo.reportParameters["FormsAccess"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Tools access: " + (settingsRptInfo.reportParameters.ContainsKey("ToolsAccess") ? (settingsRptInfo.reportParameters["ToolsAccess"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   eFolder access: " + (settingsRptInfo.reportParameters.ContainsKey("EFolderAccess") ? (settingsRptInfo.reportParameters["EFolderAccess"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Trades/Contacts/Dashboard/Reports access: " + (settingsRptInfo.reportParameters.ContainsKey("TCDRAccess") ? (settingsRptInfo.reportParameters["TCDRAccess"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Settings access: " + (settingsRptInfo.reportParameters.ContainsKey("SettingsAccess") ? (settingsRptInfo.reportParameters["SettingsAccess"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   External Settings access: " + (settingsRptInfo.reportParameters.ContainsKey("ExternalSettingsAccess") ? (settingsRptInfo.reportParameters["ExternalSettingsAccess"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Consumer Connect access: " + (settingsRptInfo.reportParameters.ContainsKey("ConsumerConnectAccess") ? (settingsRptInfo.reportParameters["ConsumerConnectAccess"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   eVault access: " + (settingsRptInfo.reportParameters.ContainsKey("EVaultAccess") ? (settingsRptInfo.reportParameters["EVaultAccess"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Web Version access: " + (settingsRptInfo.reportParameters.ContainsKey("LOConnectAccess") ? (settingsRptInfo.reportParameters["LOConnectAccess"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Data & Document Automation and Mortgage Analyzers: " + (settingsRptInfo.reportParameters.ContainsKey("IceMortgageTechAiq") ? (settingsRptInfo.reportParameters["IceMortgageTechAiq"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Developer Connect Access: " + (settingsRptInfo.reportParameters.ContainsKey("DevConnectAccess") ? (settingsRptInfo.reportParameters["DevConnectAccess"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        if (Session.ConfigurationManager.CheckIfAnyTPOSiteExists())
          this.reportTxtBx.AppendText("   TPO Connect access: " + (settingsRptInfo.reportParameters.ContainsKey("TPOADMINAccess") ? (settingsRptInfo.reportParameters["TPOADMINAccess"].Equals("True") ? "Yes" : "No") : "No") + "\n");
      }
      else if (settingsRptInfo.Type.Equals((object) SettingsRptJobInfo.jobType.UserGroups))
      {
        this.reportTxtBx.AppendText("   Members: " + (settingsRptInfo.reportParameters.ContainsKey("Members") ? (settingsRptInfo.reportParameters["Members"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Loan Access: " + (settingsRptInfo.reportParameters.ContainsKey("LoanAccess") ? (settingsRptInfo.reportParameters["LoanAccess"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Borrower Contacts: " + (settingsRptInfo.reportParameters.ContainsKey("BorrowContacts") ? (settingsRptInfo.reportParameters["BorrowContacts"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Loan Templates: " + (settingsRptInfo.reportParameters.ContainsKey("LoanTemplates") ? (settingsRptInfo.reportParameters["LoanTemplates"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Resources: " + (settingsRptInfo.reportParameters.ContainsKey("Resources") ? (settingsRptInfo.reportParameters["Resources"].Equals("True") ? "Yes" : "No") : "No") + "\n");
        this.reportTxtBx.AppendText("   Role List View: " + (settingsRptInfo.reportParameters.ContainsKey("RolesList") ? (settingsRptInfo.reportParameters["RolesList"].Equals("True") ? "Yes" : "No") : "No") + "\n");
      }
      this.reportTxtBx.AppendText("\nReport Filter Parameters: \n");
      foreach (string reportFilter in settingsRptInfo.reportFilters)
      {
        if (settingsRptInfo.Type.Equals((object) SettingsRptJobInfo.jobType.Organization))
          this.reportTxtBx.AppendText("   Organization: " + Session.OrganizationManager.GetOrganization((int) Convert.ToInt16(reportFilter)).OrgName + "\n");
        else if (settingsRptInfo.Type.Equals((object) SettingsRptJobInfo.jobType.Persona))
          this.reportTxtBx.AppendText("   Persona: " + reportFilter + "\n");
        else if (settingsRptInfo.Type.Equals((object) SettingsRptJobInfo.jobType.UserGroups))
          this.reportTxtBx.AppendText("   User Groups: " + reportFilter + "\n");
      }
      DateTime dateTime2;
      if (settingsRptInfo.CancelBy != null && settingsRptInfo.CancelDt != null)
      {
        dateTime2 = Convert.ToDateTime(settingsRptInfo.CancelDt);
        dateTime2 = dateTime2.ToLocalTime();
        string str2 = dateTime2.ToString();
        this.reportTxtBx.AppendText("\nCanceled By: " + (Session.OrganizationManager.GetUser(settingsRptInfo.CancelBy).FirstName + " " + Session.OrganizationManager.GetUser(settingsRptInfo.CancelBy).LastName + " (" + settingsRptInfo.CancelBy + ")") + "\n");
        this.reportTxtBx.AppendText("Canceled On: " + str2 + "\n");
      }
      if (settingsRptInfo.LastUpdateDt == null)
        return;
      dateTime2 = Convert.ToDateTime(settingsRptInfo.LastUpdateDt);
      dateTime2 = dateTime2.ToLocalTime();
      string str3 = dateTime2.ToString();
      if (settingsRptInfo.Status == SettingsRptJobInfo.jobStatus.Completed)
      {
        this.reportTxtBx.AppendText("\nCompleted successfully on: " + str3 + "\n");
      }
      else
      {
        if (settingsRptInfo.Status != SettingsRptJobInfo.jobStatus.Failed)
          return;
        this.reportTxtBx.AppendText("\nReport failed to generate on " + str3 + " with the following error:\n");
        this.reportTxtBx.AppendText(settingsRptInfo.Message);
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
      this.reportTxtBx = new RichTextBox();
      this.close_btn = new Button();
      this.SuspendLayout();
      this.reportTxtBx.Location = new Point(12, 12);
      this.reportTxtBx.Name = "reportTxtBx";
      this.reportTxtBx.ReadOnly = true;
      this.reportTxtBx.Size = new Size(526, 437);
      this.reportTxtBx.TabIndex = 0;
      this.reportTxtBx.Text = "";
      this.reportTxtBx.TextChanged += new EventHandler(this.reportTxtBx_TextChanged);
      this.close_btn.Location = new Point(451, 463);
      this.close_btn.Name = "close_btn";
      this.close_btn.Size = new Size(75, 23);
      this.close_btn.TabIndex = 1;
      this.close_btn.Text = "Close";
      this.close_btn.UseVisualStyleBackColor = true;
      this.close_btn.Click += new EventHandler(this.close_btn_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(553, 498);
      this.Controls.Add((Control) this.close_btn);
      this.Controls.Add((Control) this.reportTxtBx);
      this.MaximumSize = new Size(571, 543);
      this.Name = nameof (SettingsRptViewLog);
      this.ShowIcon = false;
      this.Text = "Settings Report View Log";
      this.Load += new EventHandler(this.SettingsRptViewLog_Load);
      this.ResumeLayout(false);
    }

    private void SettingsRptViewLog_Load(object sender, EventArgs e)
    {
    }

    private void close_btn_Click(object sender, EventArgs e) => this.Close();

    private void reportTxtBx_TextChanged(object sender, EventArgs e)
    {
    }
  }
}

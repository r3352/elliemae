// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.GenSettingsRptDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class GenSettingsRptDlg : Form
  {
    private IContainer components;
    private Panel reportInfoPnl;
    private Button createRpt_btn;
    private Button cancel_btn;
    private string userid;
    private Sessions.Session session;
    private Label reportType_lbl;
    private ComboBox reportType;
    private SettingsRptJobInfo jobinfo;
    private OrgSettingsRptCreate orgSettingsReportCreate;
    private PersonaSettingsRptCreate personaSettingsReportCreate;
    private UserGrpSettingsRptCreate userGrpSettingsReportCreate;

    public GenSettingsRptDlg(Sessions.Session session, string userid)
    {
      this.InitializeComponent();
      this.session = session;
      this.userid = userid;
      this.reportType.SelectedItem = (object) "Organization";
    }

    public GenSettingsRptDlg(Sessions.Session session, string userid, string reportID)
    {
      this.InitializeComponent();
      this.session = session;
      this.userid = userid;
      this.reportType.Enabled = false;
      this.jobinfo = this.session.ReportManager.GetSettingsRptInfo(reportID);
      this.jobinfo.reportParameters = this.session.ReportManager.GetSettingsReportParameters(reportID);
      this.jobinfo.reportFilters = this.session.ReportManager.GetReportFilters(reportID);
      string str = this.session.UserInfo.ToString() + " (" + this.session.UserID + ")";
      this.reportType.SelectedItem = this.jobinfo.Type.ToString() == "UserGroups" ? (object) "User Groups" : (object) this.jobinfo.Type.ToString();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.reportInfoPnl = new Panel();
      this.reportType_lbl = new Label();
      this.reportType = new ComboBox();
      this.cancel_btn = new Button();
      this.createRpt_btn = new Button();
      this.SuspendLayout();
      this.reportInfoPnl.Location = new Point(12, 42);
      this.reportInfoPnl.Name = "reportInfoPnl";
      this.reportInfoPnl.Size = new Size(914, 557);
      this.reportInfoPnl.TabIndex = 11;
      this.reportType_lbl.AutoSize = true;
      this.reportType_lbl.Location = new Point(27, 12);
      this.reportType_lbl.Name = "reportType_lbl";
      this.reportType_lbl.Size = new Size(143, 17);
      this.reportType_lbl.TabIndex = 12;
      this.reportType_lbl.Text = "Choose Report Type:";
      this.reportType.FormattingEnabled = true;
      this.reportType.Items.AddRange(new object[3]
      {
        (object) "Organization",
        (object) "Persona",
        (object) "User Groups"
      });
      this.reportType.Location = new Point(176, 12);
      this.reportType.Name = "reportType";
      this.reportType.Size = new Size(121, 24);
      this.reportType.TabIndex = 13;
      this.reportType.SelectedIndexChanged += new EventHandler(this.reportType_SelectedIndexChanged);
      this.cancel_btn.Location = new Point(674, 623);
      this.cancel_btn.Name = "cancel_btn";
      this.cancel_btn.Size = new Size(75, 25);
      this.cancel_btn.TabIndex = 14;
      this.cancel_btn.Text = "Cancel";
      this.cancel_btn.UseVisualStyleBackColor = true;
      this.cancel_btn.Click += new EventHandler(this.cancel_btn_Click);
      this.createRpt_btn.Location = new Point(765, 623);
      this.createRpt_btn.Name = "createRpt_btn";
      this.createRpt_btn.Size = new Size(119, 25);
      this.createRpt_btn.TabIndex = 15;
      this.createRpt_btn.Text = "Create Report";
      this.createRpt_btn.UseVisualStyleBackColor = true;
      this.createRpt_btn.Click += new EventHandler(this.createRpt_btn_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(934, 671);
      this.Controls.Add((Control) this.createRpt_btn);
      this.Controls.Add((Control) this.cancel_btn);
      this.Controls.Add((Control) this.reportType);
      this.Controls.Add((Control) this.reportType_lbl);
      this.Controls.Add((Control) this.reportInfoPnl);
      this.MaximizeBox = false;
      this.MaximumSize = new Size(952, 716);
      this.Name = nameof (GenSettingsRptDlg);
      this.ShowIcon = false;
      this.Text = "Generate Settings Report";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void reportType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.reportType.SelectedItem.ToString().Equals("Organization"))
      {
        this.reportInfoPnl.Controls.Clear();
        if (this.orgSettingsReportCreate == null)
          this.orgSettingsReportCreate = new OrgSettingsRptCreate(this.session, this.userid, this.jobinfo);
        this.reportInfoPnl.Controls.Add((Control) this.orgSettingsReportCreate);
        this.orgSettingsReportCreate.Show();
      }
      else if (this.reportType.SelectedItem.ToString().Equals("Persona"))
      {
        this.reportInfoPnl.Controls.Clear();
        if (this.personaSettingsReportCreate == null)
          this.personaSettingsReportCreate = new PersonaSettingsRptCreate(this.session, this.userid, this.jobinfo);
        this.reportInfoPnl.Controls.Add((Control) this.personaSettingsReportCreate);
        this.personaSettingsReportCreate.Show();
      }
      else
      {
        if (!this.reportType.SelectedItem.ToString().Equals("User Groups"))
          return;
        this.reportInfoPnl.Controls.Clear();
        if (this.userGrpSettingsReportCreate == null)
          this.userGrpSettingsReportCreate = new UserGrpSettingsRptCreate(this.session, this.userid, this.jobinfo);
        this.reportInfoPnl.Controls.Add((Control) this.userGrpSettingsReportCreate);
        this.userGrpSettingsReportCreate.Show();
      }
    }

    private void createRpt_btn_Click(object sender, EventArgs e)
    {
      if (this.reportType.SelectedItem == null)
      {
        int num1 = (int) MessageBox.Show("Please select Report Type.");
      }
      else if (this.reportType.Text.Length == 0)
      {
        int num2 = (int) MessageBox.Show("Please select Report Type.");
      }
      else
      {
        string name = string.Format("SettingsReportGenerate_{0}_UsageCount", (object) this.reportType.SelectedItem.ToString());
        if (this.reportType.SelectedItem.ToString().Equals("Organization"))
        {
          SettingsRptJobInfo.jobType jobtype = SettingsRptJobInfo.jobType.Organization;
          if (this.orgSettingsReportCreate.selectedOrgID.Length == 0)
          {
            int num3 = (int) MessageBox.Show("Please select Organization.");
            return;
          }
          if (!new Regex("^[a-zA-Z0-9\\-_\\s]*$").IsMatch(this.orgSettingsReportCreate.ReportName))
          {
            int num4 = (int) MessageBox.Show("Report Name can only contain alphanumeric characters.");
            return;
          }
          if (this.orgSettingsReportCreate.ReportName.Length == 0)
          {
            int num5 = (int) MessageBox.Show("Please enter Report Name.");
            return;
          }
          if (this.orgSettingsReportCreate.ReportName.Length > 100)
          {
            int num6 = (int) MessageBox.Show("Report Name can not be over 100 characters.");
            return;
          }
          Dictionary<string, string> reportParameters = new Dictionary<string, string>();
          reportParameters["OrganizationDetails"] = this.orgSettingsReportCreate.orgDetail.ToString();
          reportParameters["OrganizationLicenses"] = this.orgSettingsReportCreate.orgLicense.ToString();
          reportParameters["UserDetailsAll"] = this.orgSettingsReportCreate.userDetails.ToString();
          reportParameters["UserDetailsByOrg"] = this.orgSettingsReportCreate.userDetailsOrg.ToString();
          Dictionary<string, string> dictionary1 = reportParameters;
          bool flag = this.orgSettingsReportCreate.userLicenses;
          string str1 = flag.ToString();
          dictionary1["UserLicenses"] = str1;
          if (!reportParameters.ContainsValue("True"))
          {
            int num7 = (int) MessageBox.Show("Please select the type of details to generate for the report.");
            return;
          }
          Dictionary<string, string> dictionary2 = reportParameters;
          flag = this.orgSettingsReportCreate.subOrg;
          string str2 = flag.ToString();
          dictionary2["IncludeSubOrganization"] = str2;
          Dictionary<string, string> dictionary3 = reportParameters;
          flag = this.orgSettingsReportCreate.incDisabledUser;
          string str3 = flag.ToString();
          dictionary3["IncludeDisabledUser"] = str3;
          this.session.OrganizationManager.CreateSettingsRptJob(jobtype, this.orgSettingsReportCreate.ReportName, DateTime.UtcNow, reportParameters, new List<string>()
          {
            this.orgSettingsReportCreate.selectedOrgID
          });
          MetricsFactory.IncrementCounter(name, (SFxTag) new SFxUiTag());
          this.Close();
        }
        else if (this.reportType.SelectedItem.ToString().Equals("Persona"))
        {
          if (this.personaSettingsReportCreate.ReportName.Length == 0)
          {
            int num8 = (int) MessageBox.Show("Please enter Report Name.");
            return;
          }
          if (this.personaSettingsReportCreate.ReportName.Length > 100)
          {
            int num9 = (int) MessageBox.Show("Report Name can not be over 100 characters.");
            return;
          }
          if (!new Regex("^[a-zA-Z0-9\\-_\\s]*$").IsMatch(this.personaSettingsReportCreate.ReportName))
          {
            int num10 = (int) MessageBox.Show("Report Name can only contain alphanumeric characters.");
            return;
          }
          SettingsRptJobInfo.jobType jobtype = SettingsRptJobInfo.jobType.Persona;
          List<string> selectedPersona = this.personaSettingsReportCreate.SelectedPersona;
          if (selectedPersona.Count == 0)
          {
            int num11 = (int) MessageBox.Show("Please select at least 1 Persona to generate report.");
            return;
          }
          Dictionary<string, string> reportParameters = new Dictionary<string, string>();
          reportParameters["HomePageAccess"] = this.personaSettingsReportCreate.HomePageAccess.ToString();
          reportParameters["PipelineAccess"] = this.personaSettingsReportCreate.PipelineAccess.ToString();
          reportParameters["ToolsAccess"] = this.personaSettingsReportCreate.ToolsAccess.ToString();
          reportParameters["LoansAccess"] = this.personaSettingsReportCreate.LoansAccess.ToString();
          Dictionary<string, string> dictionary4 = reportParameters;
          bool flag = this.personaSettingsReportCreate.FormsAccess;
          string str4 = flag.ToString();
          dictionary4["FormsAccess"] = str4;
          Dictionary<string, string> dictionary5 = reportParameters;
          flag = this.personaSettingsReportCreate.EFolderAccess;
          string str5 = flag.ToString();
          dictionary5["EFolderAccess"] = str5;
          Dictionary<string, string> dictionary6 = reportParameters;
          flag = this.personaSettingsReportCreate.TCDRAccess;
          string str6 = flag.ToString();
          dictionary6["TCDRAccess"] = str6;
          Dictionary<string, string> dictionary7 = reportParameters;
          flag = this.personaSettingsReportCreate.SettingsAccess;
          string str7 = flag.ToString();
          dictionary7["SettingsAccess"] = str7;
          Dictionary<string, string> dictionary8 = reportParameters;
          flag = this.personaSettingsReportCreate.ExternalSettingsAccess;
          string str8 = flag.ToString();
          dictionary8["ExternalSettingsAccess"] = str8;
          Dictionary<string, string> dictionary9 = reportParameters;
          flag = this.personaSettingsReportCreate.TPOADMINAccess;
          string str9 = flag.ToString();
          dictionary9["TPOADMINAccess"] = str9;
          Dictionary<string, string> dictionary10 = reportParameters;
          flag = this.personaSettingsReportCreate.ConsumerConnectAccess;
          string str10 = flag.ToString();
          dictionary10["ConsumerConnectAccess"] = str10;
          Dictionary<string, string> dictionary11 = reportParameters;
          flag = this.personaSettingsReportCreate.LOConnectAccess;
          string str11 = flag.ToString();
          dictionary11["LOConnectAccess"] = str11;
          Dictionary<string, string> dictionary12 = reportParameters;
          flag = this.personaSettingsReportCreate.EVaultAccess;
          string str12 = flag.ToString();
          dictionary12["EVaultAccess"] = str12;
          Dictionary<string, string> dictionary13 = reportParameters;
          flag = this.personaSettingsReportCreate.IceMortgageTechAiq;
          string str13 = flag.ToString();
          dictionary13["IceMortgageTechAiq"] = str13;
          Dictionary<string, string> dictionary14 = reportParameters;
          flag = this.personaSettingsReportCreate.DeveloperConnectAccess;
          string str14 = flag.ToString();
          dictionary14["DevConnectAccess"] = str14;
          Dictionary<string, string> dictionary15 = reportParameters;
          flag = this.personaSettingsReportCreate.InternalPersonaCheckBox;
          string str15 = flag.ToString();
          dictionary15["IsInternalPersonaCheckBoxChecked"] = str15;
          if (!reportParameters.ContainsValue("True"))
          {
            int num12 = (int) MessageBox.Show("Please select the type of details to generate for the report.");
            return;
          }
          this.session.OrganizationManager.CreateSettingsRptJob(jobtype, this.personaSettingsReportCreate.ReportName, DateTime.UtcNow, reportParameters, selectedPersona);
          MetricsFactory.IncrementCounter(name, (SFxTag) new SFxUiTag());
          this.Close();
        }
        else if (this.reportType.SelectedItem.ToString().Equals("User Groups"))
        {
          if (!new Regex("^[a-zA-Z0-9\\-_\\s]*$").IsMatch(this.userGrpSettingsReportCreate.ReportName))
          {
            int num13 = (int) MessageBox.Show("Report Name can only contain alphanumeric characters.");
            return;
          }
          if (this.userGrpSettingsReportCreate.ReportName.Length == 0)
          {
            int num14 = (int) MessageBox.Show("Please enter Report Name.");
            return;
          }
          if (this.userGrpSettingsReportCreate.ReportName.Length > 100)
          {
            int num15 = (int) MessageBox.Show("Report Name can not be over 100 characters.");
            return;
          }
          SettingsRptJobInfo.jobType jobtype = SettingsRptJobInfo.jobType.UserGroups;
          List<string> selectedUserGroup = this.userGrpSettingsReportCreate.SelectedUserGroup;
          if (selectedUserGroup.Count == 0)
          {
            int num16 = (int) MessageBox.Show("Please select at least 1 User Group to generate report.");
            return;
          }
          Dictionary<string, string> reportParameters = new Dictionary<string, string>();
          reportParameters["Members"] = this.userGrpSettingsReportCreate.Members.ToString();
          reportParameters["LoanAccess"] = this.userGrpSettingsReportCreate.LoanAccess.ToString();
          reportParameters["BorrowContacts"] = this.userGrpSettingsReportCreate.BorrowerContacts.ToString();
          reportParameters["LoanTemplates"] = this.userGrpSettingsReportCreate.LoanTemplates.ToString();
          Dictionary<string, string> dictionary16 = reportParameters;
          bool flag = this.userGrpSettingsReportCreate.Resources;
          string str16 = flag.ToString();
          dictionary16["Resources"] = str16;
          Dictionary<string, string> dictionary17 = reportParameters;
          flag = this.userGrpSettingsReportCreate.RoleList;
          string str17 = flag.ToString();
          dictionary17["RolesList"] = str17;
          if (!reportParameters.ContainsValue("True"))
          {
            int num17 = (int) MessageBox.Show("Please select the type of details to generate for the report.");
            return;
          }
          this.session.OrganizationManager.CreateSettingsRptJob(jobtype, this.userGrpSettingsReportCreate.ReportName, DateTime.UtcNow, reportParameters, selectedUserGroup);
          MetricsFactory.IncrementCounter(name, (SFxTag) new SFxUiTag());
          this.Close();
        }
        else
        {
          int num18 = (int) MessageBox.Show("Report Type is invalid");
          return;
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private void cancel_btn_Click(object sender, EventArgs e) => this.Close();
  }
}

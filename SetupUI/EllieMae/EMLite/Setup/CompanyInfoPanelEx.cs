// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CompanyInfoPanelEx
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.eFolder.LoanCenter;
using EllieMae.EMLite.FormEditor.Util;
using EllieMae.EMLite.InputEngine.HtmlEmail;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class CompanyInfoPanelEx : SettingsUserControl
  {
    private OrgInfo orgInfo;
    private AddEditOrgLicenseControl licenseControl;
    private static readonly string sw = Tracing.SwOutsideLoan;
    private IContainer components;
    private TabControl tabPackages;
    private TabPage tabCompany;
    private TabPage tabLicense;
    private Panel panelLicense;
    private GroupContainer gContainer;
    private Panel panelDBA;
    private Panel panelAddMore;
    private Button btnAddDBA;
    private Panel panelDBA2;
    private TextBox txtDBAName2;
    private Label label2;
    private Panel panelDBA3;
    private TextBox txtDBAName3;
    private Label label3;
    private Panel panelDBA4;
    private TextBox txtDBAName4;
    private Label label4;
    private Panel panelDBA1;
    private TextBox txtDBAName1;
    private Label label1;
    private TextBox addTxt;
    private Label nameLbl;
    private TextBox zipTxt;
    private TextBox nameTxt;
    private Label zipLbl;
    private Label addLbl;
    private Label phoneLbl;
    private TextBox stTxt;
    private Label cityLbl;
    private TextBox phoneTxt;
    private TextBox faxTxt;
    private Label stLbl;
    private TextBox cityTxt;
    private Label faxLbl;
    private GroupContainer gcEmailSettings;
    private Label lblNote;
    private HtmlEmailControl htmlEmailSignature;
    private Button btnSSO;
    private Button btnMFA;

    public CompanyInfoPanelEx(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.panelDBA2.Visible = this.panelDBA3.Visible = this.panelDBA4.Visible = false;
      this.licenseControl = new AddEditOrgLicenseControl(true);
      this.panelLicense.Controls.Add((Control) this.licenseControl);
      this.orgInfo = Session.OrganizationManager.GetRootOrganization();
      this.refresh();
      this.btnSSO.Visible = this.btnMFA.Visible = this.IsTokenLoginEnabled() && Session.UserInfo.IsSuperAdministrator();
      this.licenseControl.DataChanged += new EventHandler(this.licenseControl_DataChanged);
      this.htmlEmailSignature.AllowSignatures = false;
      this.htmlEmailSignature.KeyDown += new KeyEventHandler(this.HtmlEmailControl_DataChanged);
      this.htmlEmailSignature.ContentChanged += new EventHandler(this.HtmlEmailControl_ContentChanged);
    }

    private void refresh()
    {
      new ConsentServiceClient().ClientConsentDataGet(AclFeature.SettingsTab_CompanyInformation);
      CompanyInfo companyInfo = Session.ConfigurationManager.GetCompanyInfo();
      this.nameTxt.Text = companyInfo.Name;
      this.addTxt.Text = companyInfo.Address;
      this.cityTxt.Text = companyInfo.City;
      this.stTxt.Text = companyInfo.State;
      this.zipTxt.Text = companyInfo.Zip;
      this.phoneTxt.Text = companyInfo.Phone;
      this.faxTxt.Text = companyInfo.Fax;
      this.panelDBA2.Visible = this.panelDBA3.Visible = this.panelDBA4.Visible = false;
      this.txtDBAName1.Text = companyInfo.DBAName1;
      this.txtDBAName2.Text = companyInfo.DBAName2;
      this.txtDBAName3.Text = companyInfo.DBAName3;
      this.txtDBAName4.Text = companyInfo.DBAName4;
      this.licenseControl.RefreshData(companyInfo.StateBranchLicensing);
      this.licenseControl.ReloadStateView();
      this.loadHtmlEmailSignature();
      this.refreshAddDBAButton();
      this.licenseControl.SetDirtyFlag(false);
      this.setDirtyFlag(false);
    }

    private void refreshAddDBAButton()
    {
      if (this.txtDBAName2.Text.Trim() != string.Empty)
        this.panelDBA2.Visible = true;
      if (this.txtDBAName3.Text.Trim() != string.Empty)
        this.panelDBA3.Visible = true;
      if (this.txtDBAName4.Text.Trim() != string.Empty)
        this.panelDBA4.Visible = true;
      if (this.panelDBA1.Visible)
        this.panelAddMore.Top = this.panelDBA1.Top + this.panelDBA1.Height;
      if (this.panelDBA2.Visible)
        this.panelAddMore.Top = this.panelDBA2.Top + this.panelDBA2.Height;
      if (this.panelDBA3.Visible)
        this.panelAddMore.Top = this.panelDBA3.Top + this.panelDBA3.Height;
      if (this.panelDBA4.Visible)
        this.panelAddMore.Top = this.panelDBA4.Top + this.panelDBA4.Height;
      this.btnAddDBA.Enabled = !this.panelDBA2.Visible || !this.panelDBA3.Visible || !this.panelDBA4.Visible;
    }

    private void loadHtmlEmailSignature()
    {
      if (this.orgInfo == null)
        return;
      if (string.IsNullOrEmpty(this.orgInfo.EmailSignature))
        this.htmlEmailSignature.LoadText(string.Empty, false);
      else
        this.htmlEmailSignature.LoadHtml(this.orgInfo.EmailSignature, false);
    }

    private void licenseControl_DataChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void textBox_TextChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void zipTxt_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar) || e.KeyChar.Equals('-'))
        e.Handled = false;
      else
        e.Handled = true;
    }

    private void zipTxt_Leave(object sender, EventArgs e)
    {
      string str = this.zipTxt.Text.Trim();
      if (str.Length != 5)
        return;
      ZipCodeInfo zipCodeInfo = ZipcodeSelector.GetZipCodeInfo(str, ZipCodeUtils.GetMultipleZipInfoAt(str));
      if (zipCodeInfo == null)
        return;
      this.cityTxt.Text = zipCodeInfo.City;
      this.stTxt.Text = zipCodeInfo.State.ToUpper();
    }

    private void phoneTxt_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
        return;
      FieldFormat dataFormat = FieldFormat.PHONE;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    public override void Save()
    {
      if (this.licenseControl != null && !this.licenseControl.DataValidated())
        return;
      BranchExtLicensing currentBranchLicensing = this.licenseControl.CurrentBranchLicensing;
      if (currentBranchLicensing.StateLicenseExtTypes.Count == 0 && currentBranchLicensing.LenderType == "" && Utils.Dialog((IWin32Window) this, "There is no license selected in any state. This might cause bad or incorrect review results. Are you sure you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      CompanyInfo info = new CompanyInfo((string) null, this.nameTxt.Text, this.addTxt.Text, this.cityTxt.Text, this.stTxt.Text, this.zipTxt.Text, this.phoneTxt.Text, this.faxTxt.Text, (string) null, (string[]) null, currentBranchLicensing);
      int num = 1;
      string[] strArray = new string[4]
      {
        this.txtDBAName1.Text.Trim(),
        this.txtDBAName2.Text.Trim(),
        this.txtDBAName3.Text.Trim(),
        this.txtDBAName4.Text.Trim()
      };
      this.txtDBAName1.Text = this.txtDBAName2.Text = this.txtDBAName3.Text = this.txtDBAName4.Text = string.Empty;
      this.panelDBA2.Visible = this.panelDBA3.Visible = this.panelDBA4.Visible = false;
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (!(strArray[index] == string.Empty))
        {
          switch (num)
          {
            case 1:
              this.txtDBAName1.Text = strArray[index];
              break;
            case 2:
              this.txtDBAName2.Text = strArray[index];
              break;
            case 3:
              this.txtDBAName3.Text = strArray[index];
              break;
            case 4:
              this.txtDBAName4.Text = strArray[index];
              break;
          }
          ++num;
        }
      }
      info.SetDBANames(new string[4]
      {
        this.txtDBAName1.Text.Trim(),
        this.txtDBAName2.Text.Trim(),
        this.txtDBAName3.Text.Trim(),
        this.txtDBAName4.Text.Trim()
      });
      this.refreshAddDBAButton();
      Session.ConfigurationManager.UpdateCompanyInfo(info, AclFeature.SettingsTab_CompanyInformation);
      this.orgInfo.EmailSignature = this.htmlEmailSignature.Html;
      Session.OrganizationManager.UpdateOrganization(this.orgInfo);
      new ConsentServiceClient().ClientConsentDataSave();
      this.setDirtyFlag(false);
    }

    public override void Reset() => this.refresh();

    private void btnAddDBA_Click(object sender, EventArgs e)
    {
      if (!this.panelDBA2.Visible)
      {
        this.panelDBA2.Visible = true;
        this.txtDBAName2.Focus();
      }
      else if (!this.panelDBA3.Visible)
      {
        this.panelDBA3.Visible = true;
        this.txtDBAName3.Focus();
      }
      else if (!this.panelDBA4.Visible)
      {
        this.panelDBA4.Visible = true;
        this.txtDBAName4.Focus();
      }
      this.refreshAddDBAButton();
    }

    private void HtmlEmailControl_DataChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void HtmlEmailControl_ContentChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void txtDBAName1_TextChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void txtDBAName2_TextChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void txtDBAName3_TextChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void txtDBAName4_TextChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void btnSSO_Click(object sender, EventArgs e) => this.ViewConfigureSSOWindow("sso");

    private void btnMFA_Click(object sender, EventArgs e)
    {
      this.ViewConfigureSSOWindow("mfa", "Configure MFA", 0.6, 0.6);
    }

    private void ViewConfigureSSOWindow(
      string featureId,
      string title = "Configure SSO",
      double windowWidth = 0.75,
      double windowHeight = 0.75)
    {
      try
      {
        double windowHeight1 = 0.75;
        double windowWidth1 = 0.75;
        string pageUrl = Session.StartupInfo.SSOGuestLoginUrl;
        if (string.IsNullOrWhiteSpace(pageUrl))
          pageUrl = EnConfigurationSettings.AppSettings["SSOGuestLoginUrl"];
        if (string.IsNullOrWhiteSpace(pageUrl))
        {
          Tracing.Log(CompanyInfoPanelEx.sw, TraceLevel.Error, this.Name, "Invalid sso configuration url, unable to configure sso - " + pageUrl);
          int num = (int) Utils.Dialog((IWin32Window) this, "Invalid sso configuration url, unable to configure sso" + pageUrl, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          using (Form ssoWebPageForm = LoginFormFactory.GetSSOWebPageForm(pageUrl, featureId, title, windowWidth1, windowHeight1))
          {
            if (ssoWebPageForm == null)
              return;
            int num = (int) ssoWebPageForm.ShowDialog((IWin32Window) this);
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CompanyInfoPanelEx.sw, TraceLevel.Error, this.Name, "Error while loading web page form with exception." + ex.StackTrace);
        int num = (int) Utils.Dialog((IWin32Window) this, "Error while loading web page form with exception." + ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private bool IsTokenLoginEnabled()
    {
      string str = "";
      try
      {
        str = SmartClientUtils.GetAttribute(Session.StartupInfo.ServerInstanceName, "EncompassBE", "AppLauncher.exe", "TokenLoginOnly");
      }
      catch (Exception ex)
      {
        Tracing.Log(CompanyInfoPanelEx.sw, TraceLevel.Error, this.Name, "Error while checking IsTokenLoginEnabled." + ex.StackTrace);
      }
      return !string.IsNullOrEmpty(str) && !(str.Trim() == "0");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.tabPackages = new TabControl();
      this.tabCompany = new TabPage();
      this.gcEmailSettings = new GroupContainer();
      this.lblNote = new Label();
      this.htmlEmailSignature = new HtmlEmailControl();
      this.gContainer = new GroupContainer();
      this.btnMFA = new Button();
      this.btnSSO = new Button();
      this.panelDBA = new Panel();
      this.panelAddMore = new Panel();
      this.btnAddDBA = new Button();
      this.panelDBA2 = new Panel();
      this.txtDBAName2 = new TextBox();
      this.label2 = new Label();
      this.panelDBA3 = new Panel();
      this.txtDBAName3 = new TextBox();
      this.label3 = new Label();
      this.panelDBA4 = new Panel();
      this.txtDBAName4 = new TextBox();
      this.label4 = new Label();
      this.panelDBA1 = new Panel();
      this.txtDBAName1 = new TextBox();
      this.label1 = new Label();
      this.addTxt = new TextBox();
      this.nameLbl = new Label();
      this.zipTxt = new TextBox();
      this.nameTxt = new TextBox();
      this.zipLbl = new Label();
      this.addLbl = new Label();
      this.phoneLbl = new Label();
      this.stTxt = new TextBox();
      this.cityLbl = new Label();
      this.phoneTxt = new TextBox();
      this.faxTxt = new TextBox();
      this.stLbl = new Label();
      this.cityTxt = new TextBox();
      this.faxLbl = new Label();
      this.tabLicense = new TabPage();
      this.panelLicense = new Panel();
      this.tabPackages.SuspendLayout();
      this.tabCompany.SuspendLayout();
      this.gcEmailSettings.SuspendLayout();
      this.gContainer.SuspendLayout();
      this.panelDBA.SuspendLayout();
      this.panelAddMore.SuspendLayout();
      this.panelDBA2.SuspendLayout();
      this.panelDBA3.SuspendLayout();
      this.panelDBA4.SuspendLayout();
      this.panelDBA1.SuspendLayout();
      this.tabLicense.SuspendLayout();
      this.SuspendLayout();
      this.tabPackages.Controls.Add((Control) this.tabCompany);
      this.tabPackages.Controls.Add((Control) this.tabLicense);
      this.tabPackages.Dock = DockStyle.Fill;
      this.tabPackages.HotTrack = true;
      this.tabPackages.ItemSize = new Size(74, 28);
      this.tabPackages.Location = new Point(0, 0);
      this.tabPackages.Margin = new Padding(0);
      this.tabPackages.Name = "tabPackages";
      this.tabPackages.Padding = new Point(11, 3);
      this.tabPackages.SelectedIndex = 0;
      this.tabPackages.Size = new Size(756, 494);
      this.tabPackages.TabIndex = 2;
      this.tabCompany.Controls.Add((Control) this.gcEmailSettings);
      this.tabCompany.Controls.Add((Control) this.gContainer);
      this.tabCompany.Location = new Point(4, 32);
      this.tabCompany.Name = "tabCompany";
      this.tabCompany.Padding = new Padding(0, 2, 2, 2);
      this.tabCompany.Size = new Size(748, 458);
      this.tabCompany.TabIndex = 1;
      this.tabCompany.Text = "Company Information";
      this.tabCompany.UseVisualStyleBackColor = true;
      this.gcEmailSettings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcEmailSettings.Controls.Add((Control) this.lblNote);
      this.gcEmailSettings.Controls.Add((Control) this.htmlEmailSignature);
      this.gcEmailSettings.HeaderForeColor = SystemColors.ControlText;
      this.gcEmailSettings.Location = new Point(0, 220);
      this.gcEmailSettings.Name = "gcEmailSettings";
      this.gcEmailSettings.Size = new Size(745, 233);
      this.gcEmailSettings.TabIndex = 17;
      this.gcEmailSettings.Text = "Email Signature";
      this.lblNote.AutoSize = true;
      this.lblNote.BackColor = Color.Transparent;
      this.lblNote.Location = new Point(108, 7);
      this.lblNote.Name = "lblNote";
      this.lblNote.Size = new Size(464, 13);
      this.lblNote.TabIndex = 1;
      this.lblNote.Text = "This signature is applied to Status Online emails when no signature is set in the Personal Settings.";
      this.htmlEmailSignature.Dock = DockStyle.Fill;
      this.htmlEmailSignature.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.htmlEmailSignature.Location = new Point(1, 26);
      this.htmlEmailSignature.Name = "htmlEmailSignature";
      this.htmlEmailSignature.Size = new Size(743, 206);
      this.htmlEmailSignature.TabIndex = 0;
      this.gContainer.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gContainer.Controls.Add((Control) this.btnMFA);
      this.gContainer.Controls.Add((Control) this.btnSSO);
      this.gContainer.Controls.Add((Control) this.panelDBA);
      this.gContainer.Controls.Add((Control) this.addTxt);
      this.gContainer.Controls.Add((Control) this.nameLbl);
      this.gContainer.Controls.Add((Control) this.zipTxt);
      this.gContainer.Controls.Add((Control) this.nameTxt);
      this.gContainer.Controls.Add((Control) this.zipLbl);
      this.gContainer.Controls.Add((Control) this.addLbl);
      this.gContainer.Controls.Add((Control) this.phoneLbl);
      this.gContainer.Controls.Add((Control) this.stTxt);
      this.gContainer.Controls.Add((Control) this.cityLbl);
      this.gContainer.Controls.Add((Control) this.phoneTxt);
      this.gContainer.Controls.Add((Control) this.faxTxt);
      this.gContainer.Controls.Add((Control) this.stLbl);
      this.gContainer.Controls.Add((Control) this.cityTxt);
      this.gContainer.Controls.Add((Control) this.faxLbl);
      this.gContainer.Dock = DockStyle.Top;
      this.gContainer.HeaderForeColor = SystemColors.ControlText;
      this.gContainer.Location = new Point(0, 2);
      this.gContainer.Name = "gContainer";
      this.gContainer.Size = new Size(746, 216);
      this.gContainer.TabIndex = 16;
      this.gContainer.Text = "Company Information";
      this.btnMFA.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMFA.BackColor = Color.Transparent;
      this.btnMFA.Location = new Point(621, 1);
      this.btnMFA.Name = "btnMFA";
      this.btnMFA.Size = new Size(118, 23);
      this.btnMFA.TabIndex = 18;
      this.btnMFA.Text = "Configure MFA";
      this.btnMFA.UseVisualStyleBackColor = false;
      this.btnMFA.Click += new EventHandler(this.btnMFA_Click);
      this.btnSSO.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSSO.BackColor = Color.Transparent;
      this.btnSSO.Location = new Point(501, 1);
      this.btnSSO.Name = "btnSSO";
      this.btnSSO.Size = new Size(118, 23);
      this.btnSSO.TabIndex = 17;
      this.btnSSO.Text = "Configure SSO";
      this.btnSSO.UseVisualStyleBackColor = false;
      this.btnSSO.Click += new EventHandler(this.btnSSO_Click);
      this.panelDBA.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelDBA.Controls.Add((Control) this.panelAddMore);
      this.panelDBA.Controls.Add((Control) this.panelDBA2);
      this.panelDBA.Controls.Add((Control) this.panelDBA3);
      this.panelDBA.Controls.Add((Control) this.panelDBA4);
      this.panelDBA.Controls.Add((Control) this.panelDBA1);
      this.panelDBA.Location = new Point(388, 37);
      this.panelDBA.Name = "panelDBA";
      this.panelDBA.Size = new Size(355, 174);
      this.panelDBA.TabIndex = 16;
      this.panelAddMore.Controls.Add((Control) this.btnAddDBA);
      this.panelAddMore.Location = new Point(0, 112);
      this.panelAddMore.Name = "panelAddMore";
      this.panelAddMore.Size = new Size(355, 28);
      this.panelAddMore.TabIndex = 4;
      this.btnAddDBA.Location = new Point(87, 0);
      this.btnAddDBA.Name = "btnAddDBA";
      this.btnAddDBA.Size = new Size(75, 23);
      this.btnAddDBA.TabIndex = 0;
      this.btnAddDBA.Text = "Add More";
      this.btnAddDBA.UseVisualStyleBackColor = true;
      this.btnAddDBA.Click += new EventHandler(this.btnAddDBA_Click);
      this.panelDBA2.Controls.Add((Control) this.txtDBAName2);
      this.panelDBA2.Controls.Add((Control) this.label2);
      this.panelDBA2.Location = new Point(0, 28);
      this.panelDBA2.Name = "panelDBA2";
      this.panelDBA2.Size = new Size(355, 28);
      this.panelDBA2.TabIndex = 1;
      this.panelDBA2.Visible = false;
      this.txtDBAName2.Location = new Point(87, 0);
      this.txtDBAName2.Name = "txtDBAName2";
      this.txtDBAName2.Size = new Size(264, 20);
      this.txtDBAName2.TabIndex = 3;
      this.txtDBAName2.TextChanged += new EventHandler(this.txtDBAName2_TextChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(3, 3);
      this.label2.Name = "label2";
      this.label2.Size = new Size(78, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "D.B.A. Name 2";
      this.panelDBA3.Controls.Add((Control) this.txtDBAName3);
      this.panelDBA3.Controls.Add((Control) this.label3);
      this.panelDBA3.Location = new Point(0, 56);
      this.panelDBA3.Name = "panelDBA3";
      this.panelDBA3.Size = new Size(355, 28);
      this.panelDBA3.TabIndex = 2;
      this.panelDBA3.Visible = false;
      this.txtDBAName3.Location = new Point(87, 0);
      this.txtDBAName3.Name = "txtDBAName3";
      this.txtDBAName3.Size = new Size(264, 20);
      this.txtDBAName3.TabIndex = 3;
      this.txtDBAName3.TextChanged += new EventHandler(this.txtDBAName3_TextChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(3, 3);
      this.label3.Name = "label3";
      this.label3.Size = new Size(78, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "D.B.A. Name 3";
      this.panelDBA4.Controls.Add((Control) this.txtDBAName4);
      this.panelDBA4.Controls.Add((Control) this.label4);
      this.panelDBA4.Location = new Point(0, 84);
      this.panelDBA4.Name = "panelDBA4";
      this.panelDBA4.Size = new Size(355, 28);
      this.panelDBA4.TabIndex = 3;
      this.panelDBA4.Visible = false;
      this.txtDBAName4.Location = new Point(87, 0);
      this.txtDBAName4.Name = "txtDBAName4";
      this.txtDBAName4.Size = new Size(264, 20);
      this.txtDBAName4.TabIndex = 3;
      this.txtDBAName4.TextChanged += new EventHandler(this.txtDBAName4_TextChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(3, 3);
      this.label4.Name = "label4";
      this.label4.Size = new Size(78, 13);
      this.label4.TabIndex = 2;
      this.label4.Text = "D.B.A. Name 4";
      this.panelDBA1.Controls.Add((Control) this.txtDBAName1);
      this.panelDBA1.Controls.Add((Control) this.label1);
      this.panelDBA1.Location = new Point(0, 0);
      this.panelDBA1.Name = "panelDBA1";
      this.panelDBA1.Size = new Size(355, 28);
      this.panelDBA1.TabIndex = 0;
      this.txtDBAName1.Location = new Point(88, 0);
      this.txtDBAName1.Name = "txtDBAName1";
      this.txtDBAName1.Size = new Size(264, 20);
      this.txtDBAName1.TabIndex = 1;
      this.txtDBAName1.TextChanged += new EventHandler(this.txtDBAName1_TextChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(4, 3);
      this.label1.Name = "label1";
      this.label1.Size = new Size(69, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "D.B.A. Name";
      this.addTxt.Location = new Point(97, 65);
      this.addTxt.MaxLength = 250;
      this.addTxt.Name = "addTxt";
      this.addTxt.Size = new Size(284, 20);
      this.addTxt.TabIndex = 4;
      this.addTxt.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.nameLbl.Location = new Point(9, 40);
      this.nameLbl.Name = "nameLbl";
      this.nameLbl.Size = new Size(80, 16);
      this.nameLbl.TabIndex = 1;
      this.nameLbl.Text = "Name";
      this.zipTxt.Location = new Point(197, 121);
      this.zipTxt.MaxLength = 10;
      this.zipTxt.Name = "zipTxt";
      this.zipTxt.Size = new Size(84, 20);
      this.zipTxt.TabIndex = 10;
      this.zipTxt.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.zipTxt.KeyPress += new KeyPressEventHandler(this.zipTxt_KeyPress);
      this.zipTxt.Leave += new EventHandler(this.zipTxt_Leave);
      this.nameTxt.Location = new Point(97, 37);
      this.nameTxt.MaxLength = 250;
      this.nameTxt.Name = "nameTxt";
      this.nameTxt.Size = new Size(284, 20);
      this.nameTxt.TabIndex = 2;
      this.nameTxt.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.zipLbl.Location = new Point(160, 124);
      this.zipLbl.Name = "zipLbl";
      this.zipLbl.Size = new Size(31, 16);
      this.zipLbl.TabIndex = 9;
      this.zipLbl.Text = "Zip";
      this.addLbl.Location = new Point(9, 68);
      this.addLbl.Name = "addLbl";
      this.addLbl.Size = new Size(82, 16);
      this.addLbl.TabIndex = 3;
      this.addLbl.Text = "Address";
      this.phoneLbl.Location = new Point(9, 152);
      this.phoneLbl.Name = "phoneLbl";
      this.phoneLbl.Size = new Size(82, 16);
      this.phoneLbl.TabIndex = 11;
      this.phoneLbl.Text = "Phone Number";
      this.stTxt.Location = new Point(97, 121);
      this.stTxt.MaxLength = 2;
      this.stTxt.Name = "stTxt";
      this.stTxt.Size = new Size(44, 20);
      this.stTxt.TabIndex = 8;
      this.stTxt.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.cityLbl.Location = new Point(9, 96);
      this.cityLbl.Name = "cityLbl";
      this.cityLbl.Size = new Size(82, 16);
      this.cityLbl.TabIndex = 5;
      this.cityLbl.Text = "City";
      this.phoneTxt.Location = new Point(97, 147);
      this.phoneTxt.MaxLength = 250;
      this.phoneTxt.Name = "phoneTxt";
      this.phoneTxt.Size = new Size(224, 20);
      this.phoneTxt.TabIndex = 12;
      this.phoneTxt.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.phoneTxt.KeyUp += new KeyEventHandler(this.phoneTxt_KeyUp);
      this.faxTxt.Location = new Point(97, 177);
      this.faxTxt.MaxLength = 250;
      this.faxTxt.Name = "faxTxt";
      this.faxTxt.Size = new Size(224, 20);
      this.faxTxt.TabIndex = 14;
      this.faxTxt.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.faxTxt.KeyUp += new KeyEventHandler(this.phoneTxt_KeyUp);
      this.stLbl.Location = new Point(9, 124);
      this.stLbl.Name = "stLbl";
      this.stLbl.Size = new Size(82, 16);
      this.stLbl.TabIndex = 7;
      this.stLbl.Text = "State";
      this.cityTxt.Location = new Point(97, 93);
      this.cityTxt.MaxLength = 250;
      this.cityTxt.Name = "cityTxt";
      this.cityTxt.Size = new Size(184, 20);
      this.cityTxt.TabIndex = 6;
      this.cityTxt.TextChanged += new EventHandler(this.textBox_TextChanged);
      this.faxLbl.Location = new Point(9, 180);
      this.faxLbl.Name = "faxLbl";
      this.faxLbl.Size = new Size(82, 16);
      this.faxLbl.TabIndex = 13;
      this.faxLbl.Text = "Fax Number";
      this.tabLicense.Controls.Add((Control) this.panelLicense);
      this.tabLicense.Location = new Point(4, 32);
      this.tabLicense.Name = "tabLicense";
      this.tabLicense.Size = new Size(748, 458);
      this.tabLicense.TabIndex = 3;
      this.tabLicense.Text = "License";
      this.tabLicense.UseVisualStyleBackColor = true;
      this.panelLicense.Dock = DockStyle.Fill;
      this.panelLicense.Location = new Point(0, 0);
      this.panelLicense.Name = "panelLicense";
      this.panelLicense.Size = new Size(748, 458);
      this.panelLicense.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tabPackages);
      this.Name = nameof (CompanyInfoPanelEx);
      this.Size = new Size(756, 494);
      this.tabPackages.ResumeLayout(false);
      this.tabCompany.ResumeLayout(false);
      this.gcEmailSettings.ResumeLayout(false);
      this.gcEmailSettings.PerformLayout();
      this.gContainer.ResumeLayout(false);
      this.gContainer.PerformLayout();
      this.panelDBA.ResumeLayout(false);
      this.panelAddMore.ResumeLayout(false);
      this.panelDBA2.ResumeLayout(false);
      this.panelDBA2.PerformLayout();
      this.panelDBA3.ResumeLayout(false);
      this.panelDBA3.PerformLayout();
      this.panelDBA4.ResumeLayout(false);
      this.panelDBA4.PerformLayout();
      this.panelDBA1.ResumeLayout(false);
      this.panelDBA1.PerformLayout();
      this.tabLicense.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}

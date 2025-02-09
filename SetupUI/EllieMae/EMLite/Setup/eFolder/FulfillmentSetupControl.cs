// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.FulfillmentSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Web.Host.BrowserControls;
using EllieMae.EMLite.eFolder.WebServices;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class FulfillmentSetupControl : UserControl
  {
    private IContainer components;
    private GroupContainer gcFulfillment;
    private FlowLayoutPanel pnlButtons;
    private Button btnStartStopService;
    private Button btnSignUp;
    private EncWebFormBrowserControl encFormBrowser;
    private Label statusLbl;
    private CheckBox chkFulfillment;
    private Panel panel1;
    private Panel panel2;
    private CheckBox chkSetOvernightShippingDefault;
    private CheckBox chkEnableOvernightShipping;

    public FulfillmentSetupControl()
    {
      this.InitializeComponent();
      this.buildScreen();
    }

    private async void buildScreen()
    {
      FulfillmentSetupControl owner = this;
      string url = "https://www.elliemae.com/go/encompass/fulfillment/products/default.asp";
      string serviceStatus = new FulfillmentService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.FulfillmentServiceUrl).GetServiceStatus(Session.CompanyInfo.ClientID);
      switch (serviceStatus)
      {
        case "Disabled":
          owner.statusLbl.ForeColor = Color.Red;
          owner.statusLbl.Text = "- Account Disabled";
          owner.btnSignUp.Visible = false;
          owner.btnStartStopService.Visible = false;
          break;
        case "Enabled":
          if (string.Equals(await new FulfillmentRestClient().GetFulFillmentSettingIntegrationType(), "Custom", StringComparison.OrdinalIgnoreCase))
          {
            owner.chkSetOvernightShippingDefault.Visible = owner.chkEnableOvernightShipping.Visible = true;
          }
          else
          {
            owner.chkSetOvernightShippingDefault.Visible = owner.chkEnableOvernightShipping.Visible = false;
            Session.ConfigurationManager.SetCompanySetting("Fulfillment", "EnableOvernightShipping", "N");
            Session.ConfigurationManager.SetCompanySetting("Fulfillment", "IsOvernightShippingDefault", "N");
          }
          owner.statusLbl.ForeColor = Color.Black;
          owner.btnSignUp.Visible = false;
          owner.btnStartStopService.Location = owner.btnSignUp.Location;
          owner.btnStartStopService.Visible = true;
          if (Session.ConfigurationManager.GetCompanySetting("Fulfillment", "ServiceEnabled") == "Y")
          {
            owner.statusLbl.Text = "- Running";
            owner.btnStartStopService.Text = "Stop Service";
            owner.chkFulfillment.Enabled = owner.chkEnableOvernightShipping.Enabled = true;
            owner.chkSetOvernightShippingDefault.Enabled = owner.chkSetOvernightShippingDefault.Checked = false;
            if (Session.ConfigurationManager.GetCompanySetting("Fulfillment", "AutoFulfillment") == "Y")
              owner.chkFulfillment.Checked = true;
            if (Session.ConfigurationManager.GetCompanySetting("Fulfillment", "EnableOvernightShipping") == "Y")
              owner.chkEnableOvernightShipping.Checked = true;
            if (Session.ConfigurationManager.GetCompanySetting("Fulfillment", "AutoFulfillment") == "Y" && Session.ConfigurationManager.GetCompanySetting("Fulfillment", "EnableOvernightShipping") == "Y")
            {
              owner.chkSetOvernightShippingDefault.Enabled = true;
              if (Session.ConfigurationManager.GetCompanySetting("Fulfillment", "IsOvernightShippingDefault") == "Y")
              {
                owner.chkSetOvernightShippingDefault.Checked = true;
                break;
              }
              break;
            }
            break;
          }
          owner.statusLbl.Text = "- Stopped";
          owner.btnStartStopService.Text = "Start Service";
          owner.chkFulfillment.Checked = owner.chkEnableOvernightShipping.Checked = owner.chkSetOvernightShippingDefault.Checked = false;
          owner.chkFulfillment.Enabled = owner.chkEnableOvernightShipping.Enabled = owner.chkSetOvernightShippingDefault.Enabled = false;
          break;
        case "Sign Up":
          owner.statusLbl.ForeColor = Color.Black;
          owner.statusLbl.Text = string.Empty;
          break;
        default:
          owner.statusLbl.ForeColor = Color.Red;
          owner.statusLbl.Text = "- Unable to determine license";
          int num = (int) MessageBox.Show((IWin32Window) owner, "Error occured: " + serviceStatus);
          break;
      }
      owner.InitBrowserNavigate(url);
    }

    private void InitBrowserNavigate(string navigateUrl)
    {
      this.encFormBrowser.Navigate(navigateUrl);
    }

    private void btnSignUp_Click(object sender, EventArgs e)
    {
      using (FulfillmentSignUpDialog fulfillmentSignUpDialog = new FulfillmentSignUpDialog())
      {
        if (fulfillmentSignUpDialog.ShowDialog((IWin32Window) Form.ActiveForm) == DialogResult.Cancel)
          return;
        try
        {
          this.Cursor = Cursors.WaitCursor;
          Application.DoEvents();
          new FulfillmentService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.FulfillmentServiceUrl).CreateOrder(Session.CompanyInfo.ClientID, Session.UserInfo.Userid, EpassLogin.LoginPassword, Session.UserInfo.Email, Session.UserInfo.FullName);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show((IWin32Window) this, "Unable to update your product list.  Please try again later." + Environment.NewLine + "Error: " + ex.Message, "Fulfillment Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        finally
        {
          this.Cursor = Cursors.Default;
        }
        Session.ConfigurationManager.SetCompanySetting("Fulfillment", "ServiceEnabled", "Y");
        this.statusLbl.ForeColor = Color.Black;
        this.btnSignUp.Visible = false;
        this.btnStartStopService.Visible = true;
        this.btnStartStopService.Location = this.btnSignUp.Location;
        this.btnStartStopService.Text = "Stop Service";
        this.statusLbl.Text = "- Running";
        int num1 = (int) MessageBox.Show((IWin32Window) this, "The Fulfillment service has been started.  You will need to exit Encompass and go back in for the service to become available.", "Disclosure Fulfillment", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void btnStartStopService_Click(object sender, EventArgs e)
    {
      if (this.btnStartStopService.Text == "Start Service")
      {
        Session.ConfigurationManager.SetCompanySetting("Fulfillment", "ServiceEnabled", "Y");
        this.statusLbl.Text = "- Running";
        this.btnStartStopService.Text = "Stop Service";
        this.chkFulfillment.Enabled = this.chkEnableOvernightShipping.Enabled = true;
        int num = (int) MessageBox.Show((IWin32Window) this, "The Fulfillment service has been started and will be available the next time you request Disclosures.", "Disclosure Fulfillment", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (MessageBox.Show((IWin32Window) this, "Are you sure you want to stop the Disclosure Fulfillment service?", "Disclosure Fulfillment", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
          return;
        this.statusLbl.Text = "- Stopped";
        this.btnStartStopService.Text = "Start Service";
        this.chkFulfillment.Checked = this.chkEnableOvernightShipping.Checked = this.chkSetOvernightShippingDefault.Checked = false;
        this.chkFulfillment.Enabled = this.chkEnableOvernightShipping.Enabled = this.chkSetOvernightShippingDefault.Enabled = false;
        this.disableAutoFullfillmentandOvernightDelivery();
        Session.ConfigurationManager.SetCompanySetting("Fulfillment", "ServiceEnabled", "N");
        int num = (int) MessageBox.Show((IWin32Window) this, "You've successfully stopped the Disclosure Fulfillment service. To start it again, click the Start Service button.", "Disclosure Fulfillment", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void chkFulfillment_Click(object sender, EventArgs e)
    {
      if (this.chkFulfillment.Checked)
      {
        Session.ConfigurationManager.SetCompanySetting("Fulfillment", "AutoFulfillment", "Y");
        if (!(Session.ConfigurationManager.GetCompanySetting("Fulfillment", "EnableOvernightShipping") == "Y"))
          return;
        this.chkSetOvernightShippingDefault.Enabled = true;
      }
      else
      {
        this.chkSetOvernightShippingDefault.Checked = this.chkSetOvernightShippingDefault.Enabled = false;
        Session.ConfigurationManager.SetCompanySetting("Fulfillment", "AutoFulfillment", "N");
        Session.ConfigurationManager.SetCompanySetting("Fulfillment", "IsOvernightShippingDefault", "N");
      }
    }

    private void disableAutoFullfillmentandOvernightDelivery()
    {
      Session.ConfigurationManager.SetCompanySetting("Fulfillment", "AutoFulfillment", "N");
      Session.ConfigurationManager.SetCompanySetting("Fulfillment", "EnableOvernightShipping", "N");
      Session.ConfigurationManager.SetCompanySetting("Fulfillment", "IsOvernightShippingDefault", "N");
    }

    private void chkEnableOvernightShipping_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkEnableOvernightShipping.Checked)
      {
        Session.ConfigurationManager.SetCompanySetting("Fulfillment", "EnableOvernightShipping", "Y");
        if (!(Session.ConfigurationManager.GetCompanySetting("Fulfillment", "AutoFulfillment") == "Y"))
          return;
        this.chkSetOvernightShippingDefault.Enabled = true;
      }
      else
      {
        this.chkSetOvernightShippingDefault.Checked = this.chkSetOvernightShippingDefault.Enabled = false;
        Session.ConfigurationManager.SetCompanySetting("Fulfillment", "EnableOvernightShipping", "N");
        Session.ConfigurationManager.SetCompanySetting("Fulfillment", "IsOvernightShippingDefault", "N");
      }
    }

    private void chkSetOvernightShippingDefault_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkSetOvernightShippingDefault.Checked)
        Session.ConfigurationManager.SetCompanySetting("Fulfillment", "IsOvernightShippingDefault", "Y");
      else
        Session.ConfigurationManager.SetCompanySetting("Fulfillment", "IsOvernightShippingDefault", "N");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.gcFulfillment = new GroupContainer();
      this.panel2 = new Panel();
      this.panel1 = new Panel();
      this.chkSetOvernightShippingDefault = new CheckBox();
      this.chkEnableOvernightShipping = new CheckBox();
      this.chkFulfillment = new CheckBox();
      this.statusLbl = new Label();
      this.pnlButtons = new FlowLayoutPanel();
      this.btnSignUp = new Button();
      this.btnStartStopService = new Button();
      this.encFormBrowser = BrowserFactory.GetWebBrowserInstance();
      this.gcFulfillment.SuspendLayout();
      this.panel1.SuspendLayout();
      this.pnlButtons.SuspendLayout();
      this.SuspendLayout();
      this.gcFulfillment.Controls.Add((Control) this.panel2);
      this.gcFulfillment.Controls.Add((Control) this.panel1);
      this.gcFulfillment.Controls.Add((Control) this.statusLbl);
      this.gcFulfillment.Controls.Add((Control) this.pnlButtons);
      this.gcFulfillment.Dock = DockStyle.Fill;
      this.gcFulfillment.HeaderForeColor = SystemColors.ControlText;
      this.gcFulfillment.Location = new Point(0, 0);
      this.gcFulfillment.Name = "gcFulfillment";
      this.gcFulfillment.Size = new Size(676, 295);
      this.gcFulfillment.TabIndex = 0;
      this.gcFulfillment.Text = "eDisclosure Fulfillment Service";
      this.panel2.Controls.Add((Control) this.encFormBrowser);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(1, 106);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(674, 188);
      this.panel2.TabIndex = 61;
      this.panel1.Controls.Add((Control) this.chkSetOvernightShippingDefault);
      this.panel1.Controls.Add((Control) this.chkEnableOvernightShipping);
      this.panel1.Controls.Add((Control) this.chkFulfillment);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(1, 26);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(674, 80);
      this.panel1.TabIndex = 60;
      this.chkSetOvernightShippingDefault.BackColor = Color.Transparent;
      this.chkSetOvernightShippingDefault.Location = new Point(14, 52);
      this.chkSetOvernightShippingDefault.Margin = new Padding(0, 2, 0, 0);
      this.chkSetOvernightShippingDefault.Name = "chkSetOvernightShippingDefault";
      this.chkSetOvernightShippingDefault.Size = new Size(189, 18);
      this.chkSetOvernightShippingDefault.TabIndex = 62;
      this.chkSetOvernightShippingDefault.Text = "Set Overnight Shipping as Default";
      this.chkSetOvernightShippingDefault.UseVisualStyleBackColor = false;
      this.chkSetOvernightShippingDefault.Visible = false;
      this.chkSetOvernightShippingDefault.CheckedChanged += new EventHandler(this.chkSetOvernightShippingDefault_CheckedChanged);
      this.chkEnableOvernightShipping.BackColor = Color.Transparent;
      this.chkEnableOvernightShipping.Location = new Point(14, 32);
      this.chkEnableOvernightShipping.Margin = new Padding(0, 2, 0, 0);
      this.chkEnableOvernightShipping.Name = "chkEnableOvernightShipping";
      this.chkEnableOvernightShipping.Size = new Size(154, 18);
      this.chkEnableOvernightShipping.TabIndex = 61;
      this.chkEnableOvernightShipping.Text = "Enable Overnight Shipping";
      this.chkEnableOvernightShipping.Visible = false;
      this.chkEnableOvernightShipping.UseVisualStyleBackColor = false;
      this.chkEnableOvernightShipping.CheckedChanged += new EventHandler(this.chkEnableOvernightShipping_CheckedChanged);
      this.chkFulfillment.BackColor = Color.Transparent;
      this.chkFulfillment.Location = new Point(14, 12);
      this.chkFulfillment.Margin = new Padding(0, 2, 0, 0);
      this.chkFulfillment.Name = "chkFulfillment";
      this.chkFulfillment.Size = new Size(246, 18);
      this.chkFulfillment.TabIndex = 60;
      this.chkFulfillment.Text = "Schedule Fulfillment Service for All Packages";
      this.chkFulfillment.UseVisualStyleBackColor = false;
      this.chkFulfillment.Click += new EventHandler(this.chkFulfillment_Click);
      this.statusLbl.AutoSize = true;
      this.statusLbl.BackColor = Color.Transparent;
      this.statusLbl.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.statusLbl.Location = new Point(177, 7);
      this.statusLbl.Name = "statusLbl";
      this.statusLbl.Size = new Size(0, 14);
      this.statusLbl.TabIndex = 59;
      this.pnlButtons.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlButtons.BackColor = Color.Transparent;
      this.pnlButtons.Controls.Add((Control) this.btnSignUp);
      this.pnlButtons.Controls.Add((Control) this.btnStartStopService);
      this.pnlButtons.FlowDirection = FlowDirection.RightToLeft;
      this.pnlButtons.Location = new Point(211, 2);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new Size(463, 22);
      this.pnlButtons.TabIndex = 0;
      this.btnSignUp.Location = new Point(379, 0);
      this.btnSignUp.Margin = new Padding(0);
      this.btnSignUp.Name = "btnSignUp";
      this.btnSignUp.Size = new Size(84, 22);
      this.btnSignUp.TabIndex = 0;
      this.btnSignUp.Text = "Sign Up";
      this.btnSignUp.UseVisualStyleBackColor = true;
      this.btnSignUp.Click += new EventHandler(this.btnSignUp_Click);
      this.btnStartStopService.Location = new Point(295, 0);
      this.btnStartStopService.Margin = new Padding(0);
      this.btnStartStopService.Name = "btnStartStopService";
      this.btnStartStopService.Size = new Size(84, 22);
      this.btnStartStopService.TabIndex = 1;
      this.btnStartStopService.Text = "Start Service";
      this.btnStartStopService.UseVisualStyleBackColor = true;
      this.btnStartStopService.Visible = false;
      this.btnStartStopService.Click += new EventHandler(this.btnStartStopService_Click);
      this.encFormBrowser.AutoSize = true;
      this.encFormBrowser.Dock = DockStyle.Fill;
      this.encFormBrowser.Location = new Point(1, 26);
      this.encFormBrowser.MinimumSize = new Size(20, 22);
      this.encFormBrowser.Name = "encFormBrowser";
      this.encFormBrowser.Size = new Size(674, 268);
      this.encFormBrowser.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcFulfillment);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (FulfillmentSetupControl);
      this.Size = new Size(676, 295);
      this.gcFulfillment.ResumeLayout(false);
      this.gcFulfillment.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.pnlButtons.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}

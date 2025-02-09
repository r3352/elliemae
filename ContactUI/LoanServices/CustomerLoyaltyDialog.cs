// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.LoanServices.CustomerLoyaltyDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.LoanServices
{
  public class CustomerLoyaltyDialog : Form
  {
    private const string HOMEURL = "https://www.epassbusinesscenter.com/customerloyalty/default.asp";
    private ToolStripDropDown helpDropDown;
    private IContainer components;
    private BrowserControl browser;
    private GradientMenuStrip mnuMain;
    private ToolStripMenuItem mnuCustomerLoyalty;
    private ToolStripMenuItem mnuHelp;
    private ToolStripSeparator mnuHelpPlaceholder;

    public CustomerLoyaltyDialog(string url)
    {
      this.InitializeComponent();
      this.initializeMenu();
      this.browser.HomeUrl = "https://www.epassbusinesscenter.com/customerloyalty/default.asp";
      this.browser.OfflinePage = SystemSettings.EpassDir + "Epass.htm";
      if (url == null)
        this.browser.GoHome();
      else
        this.browser.Navigate(url);
    }

    private void initializeMenu()
    {
      ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
      toolStripMenuItem.Text = "&Close Encompass Browser";
      toolStripMenuItem.ShortcutKeys = Keys.F4 | Keys.Alt;
      toolStripMenuItem.Click += new EventHandler(this.mnuClose_Click);
      this.mnuCustomerLoyalty.DropDown = this.browser.Menu;
      this.mnuCustomerLoyalty.DropDown.Items.Add((ToolStripItem) new ToolStripSeparator());
      this.mnuCustomerLoyalty.DropDown.Items.Add((ToolStripItem) toolStripMenuItem);
      this.helpDropDown = this.mnuHelp.DropDown;
    }

    private void browser_BeforeNavigate(object sender, WebBrowserNavigatingEventArgs e)
    {
      string url1 = e.Url.ToString();
      if (!url1.Contains("_LOANSERVICE_SIGNATURE;"))
        return;
      string url2 = LoanServiceManager.ProcessUrl((LoanDataMgr) null, url1);
      if (url2 != null)
        this.browser.Navigate(url2);
      e.Cancel = true;
    }

    private void browser_LoginUser(object sender, LoginUserEventArgs e)
    {
      e.Response = EpassLogin.LoginRequired(e.ShowDialogs);
    }

    private void mnuClose_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void mnuHelp_DropDownOpening(object sender, EventArgs e)
    {
      this.mnuHelp.DropDown = Session.Application.GetService<IEncompassApplication>().HelpDropDown;
    }

    private void mnuHelp_DropDownClosed(object sender, EventArgs e)
    {
      this.mnuHelp.DropDown = this.helpDropDown;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CustomerLoyaltyDialog));
      this.browser = new BrowserControl();
      this.mnuMain = new GradientMenuStrip();
      this.mnuCustomerLoyalty = new ToolStripMenuItem();
      this.mnuHelp = new ToolStripMenuItem();
      this.mnuHelpPlaceholder = new ToolStripSeparator();
      this.mnuMain.SuspendLayout();
      this.SuspendLayout();
      this.browser.Borders = AnchorStyles.None;
      this.browser.Dock = DockStyle.Fill;
      this.browser.Location = new Point(0, 24);
      this.browser.Name = "browser";
      this.browser.Size = new Size(547, 532);
      this.browser.TabIndex = 2;
      this.browser.LoginUser += new LoginUserEventHandler(this.browser_LoginUser);
      this.browser.BeforeNavigate += new WebBrowserNavigatingEventHandler(this.browser_BeforeNavigate);
      this.mnuMain.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.mnuCustomerLoyalty,
        (ToolStripItem) this.mnuHelp
      });
      this.mnuMain.Location = new Point(0, 0);
      this.mnuMain.Name = "mnuMain";
      this.mnuMain.Padding = new Padding(1, 0, 0, 0);
      this.mnuMain.Size = new Size(547, 24);
      this.mnuMain.TabIndex = 3;
      this.mnuCustomerLoyalty.Name = "mnuCustomerLoyalty";
      this.mnuCustomerLoyalty.Size = new Size(103, 24);
      this.mnuCustomerLoyalty.Text = "Customer Loyalt&y";
      this.mnuHelp.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.mnuHelpPlaceholder
      });
      this.mnuHelp.Name = "mnuHelp";
      this.mnuHelp.Size = new Size(40, 24);
      this.mnuHelp.Text = "&Help";
      this.mnuHelp.DropDownOpening += new EventHandler(this.mnuHelp_DropDownOpening);
      this.mnuHelp.DropDownClosed += new EventHandler(this.mnuHelp_DropDownClosed);
      this.mnuHelpPlaceholder.Name = "mnuHelpPlaceholder";
      this.mnuHelpPlaceholder.Size = new Size(57, 6);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(547, 556);
      this.Controls.Add((Control) this.browser);
      this.Controls.Add((Control) this.mnuMain);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (CustomerLoyaltyDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Customer Loyalty";
      this.mnuMain.ResumeLayout(false);
      this.mnuMain.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

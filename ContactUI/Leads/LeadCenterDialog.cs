// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Leads.LeadCenterDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.ePass.Messaging;
using EllieMae.EMLite.RemotingServices;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Leads
{
  public class LeadCenterDialog : Form
  {
    private const string HOMEURL = "http://leadcenter.elliemaeservices.com/LOS/leadcenter.aspx";
    private IContainer components;
    private BrowserControl browser;

    public LeadCenterDialog()
    {
      this.InitializeComponent();
      this.browser.HomeUrl = this.getHomeUrl();
      this.browser.OfflinePage = SystemSettings.EpassDir + "LeadCenter.htm";
      this.browser.GoHome();
    }

    private string getHomeUrl()
    {
      StringBuilder stringBuilder = new StringBuilder("http://leadcenter.elliemaeservices.com/LOS/leadcenter.aspx");
      stringBuilder.Append("?FN=" + HttpUtility.UrlEncode(Session.UserInfo.FirstName));
      stringBuilder.Append("&LN=" + HttpUtility.UrlEncode(Session.UserInfo.LastName));
      stringBuilder.Append("&EM=" + HttpUtility.UrlEncode(Session.UserInfo.Email));
      stringBuilder.Append("&PH=" + HttpUtility.UrlEncode(Session.UserInfo.Phone));
      OrgInfo branchInfo = Session.StartupInfo.BranchInfo;
      if (branchInfo != null)
      {
        stringBuilder.Append("&CO=" + HttpUtility.UrlEncode(branchInfo.CompanyName));
        stringBuilder.Append("&CA1=" + HttpUtility.UrlEncode(branchInfo.CompanyAddress.Street1));
        stringBuilder.Append("&CA2=" + HttpUtility.UrlEncode(branchInfo.CompanyAddress.Street2));
        stringBuilder.Append("&CAC=" + HttpUtility.UrlEncode(branchInfo.CompanyAddress.City));
        stringBuilder.Append("&CAS=" + HttpUtility.UrlEncode(branchInfo.CompanyAddress.State));
        stringBuilder.Append("&CAZ=" + HttpUtility.UrlEncode(branchInfo.CompanyAddress.Zip));
      }
      else
      {
        CompanyInfo companyInfo = Session.CompanyInfo;
        stringBuilder.Append("&CO=" + HttpUtility.UrlEncode(companyInfo.Name));
        stringBuilder.Append("&CA1=" + HttpUtility.UrlEncode(companyInfo.Address));
        stringBuilder.Append("&CA2=");
        stringBuilder.Append("&CAC=" + HttpUtility.UrlEncode(companyInfo.City));
        stringBuilder.Append("&CAS=" + HttpUtility.UrlEncode(companyInfo.State));
        stringBuilder.Append("&CAZ=" + HttpUtility.UrlEncode(companyInfo.Zip));
      }
      if (Session.GetPrivateProfileString("LeadCenter", "Subscribed") == "1")
        stringBuilder.Append("&subscribed=1");
      return stringBuilder.ToString();
    }

    private void browser_LoginUser(object sender, LoginUserEventArgs e)
    {
      e.Response = EpassLogin.LoginRequired(e.ShowDialogs);
    }

    public void ImportLeads()
    {
      this.browser.ProcessURL("_EPASS_SIGNATURE;JEDLEADS;2");
      EPassMessages.SyncReadMessages(false);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.browser = new BrowserControl();
      this.SuspendLayout();
      this.browser.Borders = AnchorStyles.None;
      this.browser.Dock = DockStyle.Fill;
      this.browser.Location = new Point(0, 0);
      this.browser.Name = "browser";
      this.browser.ShowToolbar = false;
      this.browser.Size = new Size(604, 568);
      this.browser.TabIndex = 2;
      this.browser.LoginUser += new LoginUserEventHandler(this.browser_LoginUser);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(604, 568);
      this.Controls.Add((Control) this.browser);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LeadCenterDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Lead Center";
      this.ResumeLayout(false);
    }
  }
}

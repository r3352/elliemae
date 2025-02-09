// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.LoanMailboxDialog
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.RemotingServices;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass
{
  public class LoanMailboxDialog : Form
  {
    private const string HOMEURL = "https://www.epassbusinesscenter.com/msglist40/login.asp";
    private readonly string _homeUrl;
    private IContainer components;
    private BrowserControl browser;

    public LoanMailboxDialog()
    {
      this.InitializeComponent();
      this._homeUrl = !Session.StartupInfo.ClientSettings.Contains((object) "LoanMailboxUrl") ? "https://www.epassbusinesscenter.com/msglist40/login.asp" : (string) Session.StartupInfo.ClientSettings[(object) "LoanMailboxUrl"];
      this.browser.HomeUrl = this._homeUrl;
      this.browser.OfflinePage = SystemSettings.EpassDir + "Mailbox.htm";
      this.browser.GoHome();
    }

    private void browser_LoginUser(object sender, LoginUserEventArgs e)
    {
      e.Response = EpassLogin.LoginRequired(e.ShowDialogs);
      if (!e.Response)
        return;
      string str = this._homeUrl + "?JID=" + EpassLogin.LoginID;
      if (Session.LoanData != null)
        str = str + "&LoanID=" + Session.LoanData.GUID;
      this.browser.HomeUrl = !Session.UserInfo.IsSuperAdministrator() ? str + "&ShowAll=" + (object) 0 : str + "&ShowAll=" + (object) 1;
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
      this.browser.Size = new Size(716, 570);
      this.browser.TabIndex = 2;
      this.browser.LoginUser += new LoginUserEventHandler(this.browser_LoginUser);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(716, 570);
      this.Controls.Add((Control) this.browser);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanMailboxDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Loan Mailbox";
      this.ResumeLayout(false);
    }
  }
}

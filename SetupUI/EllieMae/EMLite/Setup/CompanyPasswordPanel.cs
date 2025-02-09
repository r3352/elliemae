// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CompanyPasswordPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class CompanyPasswordPanel : SettingsUserControl
  {
    private const string className = "CompanyPassword";
    private static readonly string sw = Tracing.SwEpass;
    private Label oldLbl;
    private Label newLbl;
    private Label confirmLbl;
    private TextBox oldTxt;
    private TextBox newTxt;
    private TextBox confirmTxt;
    private GroupContainer groupContainer1;
    private System.ComponentModel.Container components;

    public CompanyPasswordPanel(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.setDirtyFlag(false);
      WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.oldLbl = new Label();
      this.newLbl = new Label();
      this.confirmLbl = new Label();
      this.oldTxt = new TextBox();
      this.newTxt = new TextBox();
      this.confirmTxt = new TextBox();
      this.groupContainer1 = new GroupContainer();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.oldLbl.Location = new Point(8, 42);
      this.oldLbl.Name = "oldLbl";
      this.oldLbl.Size = new Size(100, 16);
      this.oldLbl.TabIndex = 8;
      this.oldLbl.Text = "Old Password";
      this.oldLbl.TextAlign = ContentAlignment.MiddleLeft;
      this.newLbl.Location = new Point(8, 70);
      this.newLbl.Name = "newLbl";
      this.newLbl.Size = new Size(100, 16);
      this.newLbl.TabIndex = 9;
      this.newLbl.Text = "New Password";
      this.newLbl.TextAlign = ContentAlignment.MiddleLeft;
      this.confirmLbl.Location = new Point(8, 98);
      this.confirmLbl.Name = "confirmLbl";
      this.confirmLbl.Size = new Size(100, 16);
      this.confirmLbl.TabIndex = 10;
      this.confirmLbl.Text = "Confirm Password";
      this.confirmLbl.TextAlign = ContentAlignment.MiddleLeft;
      this.oldTxt.Location = new Point(110, 41);
      this.oldTxt.Name = "oldTxt";
      this.oldTxt.PasswordChar = '*';
      this.oldTxt.Size = new Size(212, 20);
      this.oldTxt.TabIndex = 11;
      this.oldTxt.TextChanged += new EventHandler(this.txtBox_TextChanged);
      this.newTxt.Location = new Point(110, 69);
      this.newTxt.Name = "newTxt";
      this.newTxt.PasswordChar = '*';
      this.newTxt.Size = new Size(212, 20);
      this.newTxt.TabIndex = 12;
      this.newTxt.TextChanged += new EventHandler(this.txtBox_TextChanged);
      this.confirmTxt.Location = new Point(110, 97);
      this.confirmTxt.Name = "confirmTxt";
      this.confirmTxt.PasswordChar = '*';
      this.confirmTxt.Size = new Size(212, 20);
      this.confirmTxt.TabIndex = 13;
      this.confirmTxt.TextChanged += new EventHandler(this.txtBox_TextChanged);
      this.groupContainer1.Controls.Add((Control) this.oldTxt);
      this.groupContainer1.Controls.Add((Control) this.confirmTxt);
      this.groupContainer1.Controls.Add((Control) this.newLbl);
      this.groupContainer1.Controls.Add((Control) this.newTxt);
      this.groupContainer1.Controls.Add((Control) this.oldLbl);
      this.groupContainer1.Controls.Add((Control) this.confirmLbl);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(482, 267);
      this.groupContainer1.TabIndex = 14;
      this.groupContainer1.Text = "Change ICE Mortgage Technology Network Company Password";
      this.Controls.Add((Control) this.groupContainer1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (CompanyPasswordPanel);
      this.Size = new Size(482, 267);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }

    public override void Save()
    {
      if (this.oldTxt.Text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter the old password.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.oldTxt.Focus();
      }
      else if (this.newTxt.Text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter the new password.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.newTxt.Focus();
      }
      else if (this.confirmTxt.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have not confirmed your new password.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.confirmTxt.Focus();
      }
      else if (this.newTxt.Text != this.confirmTxt.Text)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The new password and confirm password do not match.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.confirmTxt.Focus();
      }
      else
      {
        if (!EpassLogin.LoginRequired(true))
          return;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("clientid=" + HttpUtility.UrlEncode(Session.CompanyInfo.ClientID));
        stringBuilder.Append("&oldpassword=" + HttpUtility.UrlEncode(this.oldTxt.Text));
        stringBuilder.Append("&newpassword=" + HttpUtility.UrlEncode(this.newTxt.Text));
        string end;
        try
        {
          HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create("https://www.epassbusinesscenter.com/epassutils/jedchangecompanypwd.asp");
          httpWebRequest.KeepAlive = false;
          httpWebRequest.Method = "POST";
          httpWebRequest.ContentType = "application/x-www-form-urlencoded";
          httpWebRequest.ContentLength = (long) stringBuilder.Length;
          StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
          streamWriter.Write(stringBuilder.ToString());
          streamWriter.Close();
          StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream());
          end = streamReader.ReadToEnd();
          streamReader.Close();
        }
        catch (Exception ex)
        {
          MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\SetupUI\\Setup\\CompanyPasswordPanel.cs", nameof (Save), 251);
          Tracing.Log(CompanyPasswordPanel.sw, TraceLevel.Error, "CompanyPassword", ex.Message);
          int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to change the password:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return;
        }
        if (end == "Invalid Company Password")
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "The password you entered is incorrect. Please verify the old password and try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          Session.ConfigurationManager.SetCompanySetting("CLIENT", "CLIENTPASSWORD", end);
          Session.RecacheCompanyInfo();
          this.setDirtyFlag(false);
        }
      }
    }

    protected override void setDirtyFlag(bool val)
    {
      base.setDirtyFlag(val);
      this.setupContainer.ButtonResetEnabled = false;
    }

    private void txtBox_TextChanged(object sender, EventArgs e) => this.setDirtyFlag(true);
  }
}

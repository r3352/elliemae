// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.EMMessage
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class EMMessage : Form
  {
    private string emMessageFile;
    private string url;
    private string defaultPage;
    private DateTime startTime = DateTime.MinValue;
    private DateTime endTime = DateTime.MaxValue;
    private bool displayMsg = true;
    private IContainer components;
    private Button btnOK;
    private CheckBox chkBoxDontShow;
    private EMWebBrowser webBrowser1;

    public bool DisplayMessage => this.displayMsg;

    private EMMessage(bool isUrl, string emMessageFileOrUrl, string defaultPage)
    {
      this.InitializeComponent();
      this.defaultPage = defaultPage;
      if (isUrl)
      {
        this.Text = "Encompass Welcome";
        this.url = emMessageFileOrUrl;
      }
      else
      {
        this.emMessageFile = emMessageFileOrUrl;
        this.TopMost = true;
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(this.emMessageFile);
        XmlElement documentElement = xmlDocument.DocumentElement;
        this.url = documentElement.GetAttribute(nameof (url));
        if (!this.url.ToLower().StartsWith("http://") && !this.url.ToLower().StartsWith("https://") && !Path.IsPathRooted(this.url))
          this.url = Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, this.url);
        string fullVersion = VersionInformation.CurrentVersion.Version.FullVersion;
        if (documentElement.GetAttribute("encVersion") != fullVersion)
        {
          this.deleteMsgFile();
          this.displayMsg = false;
          return;
        }
        try
        {
          string attribute = documentElement.GetAttribute(nameof (startTime));
          if ((attribute ?? "").Trim() != "")
            this.startTime = Convert.ToDateTime((attribute ?? "").Trim());
        }
        catch
        {
        }
        try
        {
          string attribute = documentElement.GetAttribute(nameof (endTime));
          if ((attribute ?? "").Trim() != "")
            this.endTime = Convert.ToDateTime((attribute ?? "").Trim());
        }
        catch
        {
        }
        if (this.startTime > DateTime.Now || this.endTime < DateTime.Now)
        {
          if (this.endTime < DateTime.Now)
            this.deleteMsgFile();
          this.displayMsg = false;
          return;
        }
        this.Width = Convert.ToInt32(documentElement.GetAttribute("width"));
        this.Height = Convert.ToInt32(documentElement.GetAttribute("height"));
        this.chkBoxDontShow.Enabled = (documentElement.GetAttribute("disableDontShowCheckBox") ?? "").Trim().ToLower() != "true";
      }
      this.webBrowser1.Navigate(this.url);
    }

    public static void ShowEMMessage(string defaultPage)
    {
      string str = Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, "documents\\welcome\\EMMessage.xml");
      if (!File.Exists(str))
        return;
      EMMessage emMessage = new EMMessage(false, str, defaultPage);
      if (!emMessage.DisplayMessage)
        return;
      int num = (int) emMessage.ShowDialog();
    }

    private void deleteMsgFile()
    {
      try
      {
        if (!this.url.ToLower().StartsWith("http://"))
        {
          if (!this.url.ToLower().StartsWith("https://"))
            File.Delete(this.url);
        }
      }
      catch
      {
      }
      try
      {
        File.Delete(this.emMessageFile);
      }
      catch
      {
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.chkBoxDontShow.Visible || !this.chkBoxDontShow.Checked)
        return;
      if (this.emMessageFile == null)
        Session.WelcomeScreenSettingMgr.Save(new WelcomeScreenSetting(false));
      else
        this.deleteMsgFile();
    }

    private void EMMessage_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.webBrowser1.Dispose();
    }

    private void webBrowser1_NavigateError(
      object sender,
      EMWebBrowser.WebBrowserNavigateErrorEventArgs e)
    {
      if (this.defaultPage == null || e.Url == this.defaultPage)
        return;
      this.webBrowser1.Navigate(this.defaultPage);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.btnOK = new Button();
      this.chkBoxDontShow = new CheckBox();
      this.webBrowser1 = new EMWebBrowser(this.components);
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(583, 438);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.chkBoxDontShow.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.chkBoxDontShow.AutoSize = true;
      this.chkBoxDontShow.Location = new Point(401, 442);
      this.chkBoxDontShow.Name = "chkBoxDontShow";
      this.chkBoxDontShow.Size = new Size(179, 17);
      this.chkBoxDontShow.TabIndex = 2;
      this.chkBoxDontShow.Text = "Do not show this message again";
      this.chkBoxDontShow.UseVisualStyleBackColor = true;
      this.webBrowser1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.webBrowser1.Location = new Point(0, 0);
      this.webBrowser1.MinimumSize = new Size(20, 20);
      this.webBrowser1.Name = "webBrowser1";
      this.webBrowser1.Size = new Size(673, 432);
      this.webBrowser1.TabIndex = 3;
      this.webBrowser1.NavigateError += new EMWebBrowser.WebBrowserNavigateErrorEventHandler(this.webBrowser1_NavigateError);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(672, 466);
      this.Controls.Add((Control) this.webBrowser1);
      this.Controls.Add((Control) this.chkBoxDontShow);
      this.Controls.Add((Control) this.btnOK);
      this.MinimizeBox = false;
      this.Name = nameof (EMMessage);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "ICE Mortgage Technology Encompass";
      this.FormClosing += new FormClosingEventHandler(this.EMMessage_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

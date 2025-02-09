// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.DocuSignSigningDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Elli.Web.Host;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class DocuSignSigningDialog : Form
  {
    private WebHost webHost;
    public bool closeForm;
    public ESignResult ESignResult = ESignResult.Cancel;
    public string url;
    private IContainer components;

    public DocuSignSigningDialog(string url)
    {
      this.webHost = new WebHost("apiplatform");
      this.InitializeComponent();
      this.webHost.LoadingFrame += new WebHost.LoadingFrameEventHandler(this.WebHost_startLoadingFrameEvent);
      this.url = url;
    }

    private void WebHost_startLoadingFrameEvent(object sender, StartLoadingEventArgs e)
    {
      Uri uri = new Uri(e.ValidatedURL);
      if (!uri.Host.Contains("elliemae.com"))
        return;
      string query = uri.Query;
      if (!string.IsNullOrEmpty(query))
      {
        string lower = query.ToLower();
        if (lower.Contains("signing_complete"))
          this.ESignResult = ESignResult.Complete;
        else if (lower.Contains("viewing_complete"))
          this.ESignResult = ESignResult.Viewed;
      }
      this.closeForm = true;
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      Timer timer = new Timer();
      timer.Interval = 100;
      timer.Tick += new EventHandler(this.Timer1_Tick);
      timer.Start();
      this.webHost.Navigate(this.url);
    }

    private void Timer1_Tick(object sender, EventArgs e)
    {
      if (!this.closeForm)
        return;
      this.Close();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
      this.closeForm = true;
      base.OnFormClosing(e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(560, 288);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DocuSignSigning";
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "eSign Documents";
      this.webHost.Dock = DockStyle.Fill;
      this.Controls.Add((Control) this.webHost);
      this.ResumeLayout(false);
    }
  }
}

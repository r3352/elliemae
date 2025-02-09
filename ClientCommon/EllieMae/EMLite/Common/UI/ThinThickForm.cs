// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ThinThickForm
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ThinThickForm : Form
  {
    private UserControl _thinThickBrowser;
    private IContainer components;

    public event ThinThickForm.WebBrowserTitleChangedHandler WebBrowserDocumentTitleChanged;

    public event ThinThickForm.WebBrowserBeforeNavigateHandler BeforeNavigate;

    public event ThinThickForm.WebBrowserNavigatedHandler Navigated;

    public ThinThickForm(string title = "")
    {
      this.InitializeComponent();
      if (string.IsNullOrEmpty(title))
        return;
      this.Text = title;
    }

    public void AddThinThickPage(UserControl thinThickPage)
    {
      for (int index = 0; index < this.Controls.Count; ++index)
        this.Controls.RemoveAt(index);
      this.Controls.Add((Control) thinThickPage);
      thinThickPage.Dock = DockStyle.Fill;
      this._thinThickBrowser = thinThickPage;
    }

    private void Page_WebBrowserDocumentTitleChanged(object sender, EventArgs e)
    {
      if (this.WebBrowserDocumentTitleChanged == null)
        return;
      this.WebBrowserDocumentTitleChanged(sender, e);
    }

    private void ThinThickForm_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    private void ThinThickForm_Load(object sender, EventArgs e)
    {
      Control control = this.Controls[0];
      if (!(control is EncompassLoginBrowser))
        return;
      if (string.IsNullOrWhiteSpace(this.Text) || string.Compare(nameof (ThinThickForm), this.Text, true) == 0)
        this.Text = "Encompass Log In";
      ((EncompassLoginBrowser) control).WebBrowserDocumentTitleChanged += new EncompassLoginBrowser.WebBrowserTitleChangedHandler(this.Page_WebBrowserDocumentTitleChanged);
      ((EncompassLoginBrowser) control).BeforeNavigate += new EncompassLoginBrowser.WebBrowserBeforeNavigateHandler(this.ThinThickForm_BeforeNavigate);
      ((EncompassLoginBrowser) control).Navigated += new EncompassLoginBrowser.WebBrowserNavigatedHandler(this.ThinThickForm_Navigated);
      this.MaximizeBox = false;
    }

    private void ThinThickForm_Navigated(object sender, WebBrowserNavigatedEventArgs e)
    {
      if (this.Navigated == null)
        return;
      this.Navigated(sender, e);
    }

    private void ThinThickForm_BeforeNavigate(object sender, WebBrowserNavigatingEventArgs e)
    {
      if (this.BeforeNavigate == null)
        return;
      this.BeforeNavigate(sender, e);
    }

    public void Navigate(string url)
    {
      if (!(this._thinThickBrowser is EncompassLoginBrowser))
        return;
      ((EncompassLoginBrowser) this._thinThickBrowser).Navigate(url);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ThinThickForm));
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1042, 732);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MinimizeBox = false;
      this.Name = nameof (ThinThickForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (ThinThickForm);
      this.FormClosing += new FormClosingEventHandler(this.ThinThickForm_FormClosing);
      this.Load += new EventHandler(this.ThinThickForm_Load);
      this.ResumeLayout(false);
    }

    public delegate void WebBrowserTitleChangedHandler(object sender, EventArgs e);

    public delegate void WebBrowserBeforeNavigateHandler(
      object sender,
      WebBrowserNavigatingEventArgs e);

    public delegate void WebBrowserNavigatedHandler(object sender, WebBrowserNavigatedEventArgs e);
  }
}

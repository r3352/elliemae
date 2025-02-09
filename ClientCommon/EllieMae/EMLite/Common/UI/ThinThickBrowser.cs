// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ThinThickBrowser
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.UI.Controls.ThinThick;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ThinThickBrowser : UserControl
  {
    private IContainer components;
    private WebBrowser webBrowser1;

    internal ThinThickBrowserManager Manager { get; private set; }

    public virtual string DocumentTitle => this.webBrowser1.DocumentTitle;

    public event ThinThickBrowser.WebBrowserTitleChangedHandler WebBrowserDocumentTitleChanged;

    public event ThinThickBrowser.WebBrowserBeforeNavigateHandler BeforeNavigate;

    public event ThinThickBrowser.WebBrowserNavigatedHandler Navigated;

    public ThinThickBrowser()
    {
      this.InitializeComponent();
      this.webBrowser1.Navigated += new WebBrowserNavigatedEventHandler(this.NavigateToHttpWhenHttpsFails);
      this.webBrowser1.DocumentTitleChanged += new EventHandler(this.WebBrowser1_DocumentTitleChanged);
      this.webBrowser1.Navigating += new WebBrowserNavigatingEventHandler(this.WebBrowser1_Navigating);
      this.webBrowser1.Navigated += new WebBrowserNavigatedEventHandler(this.WebBrowser1_Navigated);
    }

    private void WebBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
    {
      if (this.Navigated == null)
        return;
      this.Navigated(sender, e);
    }

    private void WebBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
    {
      if (this.BeforeNavigate == null)
        return;
      this.BeforeNavigate(sender, e);
    }

    private void WebBrowser1_DocumentTitleChanged(object sender, EventArgs e)
    {
      if (this.WebBrowserDocumentTitleChanged == null)
        return;
      this.WebBrowserDocumentTitleChanged(sender, e);
    }

    private void NavigateToHttpWhenHttpsFails(object sender, WebBrowserNavigatedEventArgs args)
    {
      if (!(sender is WebBrowser webBrowser) || !webBrowser.Url.IsAbsoluteUri || !webBrowser.Url.AbsoluteUri.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase) || webBrowser.DocumentText == null || !webBrowser.DocumentText.Contains("<title>Navigation Canceled</title>"))
        return;
      this.webBrowser1.Navigate(webBrowser.Url.AbsoluteUri.Replace("https://", "http://"));
    }

    public void SetObjectForScripting(object obj) => this.webBrowser1.ObjectForScripting = obj;

    public void SetManager(ThinThickBrowserManager mgr) => this.Manager = mgr;

    public void Navigate(string url) => this.webBrowser1.Navigate(url);

    public void Navigate(string url, byte[] postData, string additionalHttpHeaders)
    {
      this.webBrowser1.Navigate(new Uri(url, UriKind.RelativeOrAbsolute), "_self", postData, additionalHttpHeaders);
    }

    public void SetDocumentText(string html) => this.webBrowser1.DocumentText = html;

    internal void SupressScriptError() => this.webBrowser1.ScriptErrorsSuppressed = true;

    public void InjectJavascript(string script)
    {
      HtmlElement htmlElement = this.webBrowser1.Document.GetElementsByTagName("head")[0];
      HtmlElement element = this.webBrowser1.Document.CreateElement(nameof (script));
      element.InnerText = script;
      htmlElement.AppendChild(element);
    }

    public void InvokeScript(string scriptName, object[] args)
    {
      if (!(this.webBrowser1.Document != (HtmlDocument) null))
        return;
      this.webBrowser1.Document.InvokeScript(scriptName, args);
    }

    public void SetValue(string elementId, string elementValue)
    {
      if (!(this.webBrowser1.Document != (HtmlDocument) null))
        return;
      this.webBrowser1.Document.InvokeScript("setValue", new object[2]
      {
        (object) elementId,
        (object) elementValue
      });
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      return this.Manager == null || this.Manager.ProcessCmdKey(ref msg, keyData) || base.ProcessCmdKey(ref msg, keyData);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.webBrowser1 = new WebBrowser();
      this.SuspendLayout();
      this.webBrowser1.Dock = DockStyle.Fill;
      this.webBrowser1.Location = new Point(0, 0);
      this.webBrowser1.Margin = new Padding(0);
      this.webBrowser1.Name = "webBrowser1";
      this.webBrowser1.Size = new Size(0, 0);
      this.webBrowser1.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.webBrowser1);
      this.Name = nameof (ThinThickBrowser);
      this.Size = new Size(0, 0);
      this.ResumeLayout(false);
    }

    public delegate void WebBrowserTitleChangedHandler(object sender, EventArgs e);

    public delegate void WebBrowserBeforeNavigateHandler(
      object sender,
      WebBrowserNavigatingEventArgs e);

    public delegate void WebBrowserNavigatedHandler(object sender, WebBrowserNavigatedEventArgs e);
  }
}

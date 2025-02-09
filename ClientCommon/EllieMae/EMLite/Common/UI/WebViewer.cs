// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.WebViewer
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using AxSHDocVw;
using EllieMae.EMLite.Common.Properties;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class WebViewer : Form
  {
    private static object nobj = (object) Missing.Value;
    private static Hashtable browserWindows = new Hashtable();
    private System.ComponentModel.Container components;
    private string url;
    private SplitContainer splitContainer1;
    private AxWebBrowser axWebBrowser1;
    private Button btnback;
    private Button btnfwd;
    private string windowName;
    private bool suppressScriptError;

    public bool SuppressScriptError
    {
      set => this.suppressScriptError = value;
    }

    public WebViewer(string title, string url)
      : this((string) null, title, url)
    {
    }

    public WebViewer(string windowName, string title, string url)
    {
      this.InitializeComponent();
      this.windowName = windowName;
      this.Text = title;
      this.url = url;
      if (title == "Encompass Help")
        this.splitContainer1.Panel1Collapsed = false;
      else
        this.splitContainer1.Panel1Collapsed = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (WebViewer));
      this.splitContainer1 = new SplitContainer();
      this.btnfwd = new Button();
      this.axWebBrowser1 = new AxWebBrowser();
      this.btnback = new Button();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.axWebBrowser1.BeginInit();
      this.SuspendLayout();
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.FixedPanel = FixedPanel.Panel1;
      this.splitContainer1.IsSplitterFixed = true;
      this.splitContainer1.Location = new Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = Orientation.Horizontal;
      this.splitContainer1.Panel1.BackColor = SystemColors.ControlLightLight;
      this.splitContainer1.Panel1.Controls.Add((Control) this.btnfwd);
      this.splitContainer1.Panel1.Controls.Add((Control) this.btnback);
      this.splitContainer1.Panel1MinSize = 5;
      this.splitContainer1.Panel2.Controls.Add((Control) this.axWebBrowser1);
      this.splitContainer1.Size = new Size(478, 357);
      this.splitContainer1.SplitterDistance = 30;
      this.splitContainer1.TabIndex = 0;
      this.btnfwd.Dock = DockStyle.Left;
      this.btnfwd.FlatAppearance.BorderSize = 0;
      this.btnfwd.FlatStyle = FlatStyle.Flat;
      this.btnfwd.ForeColor = SystemColors.ControlLightLight;
      this.btnfwd.Image = (Image) Resources.forward_browser;
      this.btnfwd.Location = new Point(57, 0);
      this.btnfwd.Name = "btnfwd";
      this.btnfwd.Size = new Size(57, 34);
      this.btnfwd.TabIndex = 1;
      this.btnfwd.UseVisualStyleBackColor = true;
      this.btnfwd.Click += new EventHandler(this.button_Click);
      this.axWebBrowser1.Dock = DockStyle.Fill;
      this.axWebBrowser1.Enabled = true;
      this.axWebBrowser1.Location = new Point(0, 0);
      this.axWebBrowser1.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser1.OcxState");
      this.axWebBrowser1.Size = new Size(478, 319);
      this.axWebBrowser1.TabIndex = 5;
      this.btnback.Dock = DockStyle.Left;
      this.btnback.FlatAppearance.BorderSize = 0;
      this.btnback.FlatStyle = FlatStyle.Flat;
      this.btnback.ForeColor = SystemColors.ControlLightLight;
      this.btnback.Image = (Image) Resources.back_browser;
      this.btnback.Location = new Point(0, 0);
      this.btnback.Name = "btnback";
      this.btnback.Size = new Size(57, 34);
      this.btnback.TabIndex = 0;
      this.btnback.UseVisualStyleBackColor = true;
      this.btnback.Click += new EventHandler(this.button_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(478, 357);
      this.Controls.Add((Control) this.splitContainer1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (WebViewer);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (WebViewer);
      this.Closing += new CancelEventHandler(this.WebViewer_Closing);
      this.Load += new EventHandler(this.WebViewer_Load);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.axWebBrowser1.EndInit();
      this.ResumeLayout(false);
    }

    public string WindowName => this.windowName;

    private void WebViewer_Load(object sender, EventArgs e) => this.Navigate(this.url);

    public void Navigate(string url)
    {
      this.url = url;
      if (this.suppressScriptError)
        this.axWebBrowser1.Silent = true;
      this.axWebBrowser1.Navigate(this.url);
    }

    private void WebViewer_Closing(object sender, CancelEventArgs e)
    {
      this.axWebBrowser1.Dispose();
    }

    public static void OpenURL(string url, string title, int width, int height)
    {
      WebViewer.OpenURL(url, title, width, height, false);
    }

    public static void OpenURL(
      string url,
      string title,
      int width,
      int height,
      bool surpressScriptError)
    {
      using (WebViewer webViewer = new WebViewer(title, url))
      {
        if (surpressScriptError)
          webViewer.suppressScriptError = surpressScriptError;
        webViewer.Size = new Size(width, height);
        int num = (int) webViewer.ShowDialog((IWin32Window) Form.ActiveForm);
      }
    }

    public static Form OpenURL(
      string windowName,
      string url,
      string title,
      int width,
      int height)
    {
      if (!(WebViewer.browserWindows[(object) windowName] is WebViewer webViewer))
      {
        webViewer = new WebViewer(windowName, title, url);
        webViewer.Size = new Size(width, height);
        webViewer.Closed += new EventHandler(WebViewer.onWebViewerClosed);
        WebViewer.browserWindows[(object) windowName] = (object) webViewer;
        webViewer.Show();
      }
      else
      {
        webViewer.Navigate(url);
        webViewer.BringToFront();
      }
      return (Form) webViewer;
    }

    private static void onWebViewerClosed(object sender, EventArgs e)
    {
      WebViewer.browserWindows.Remove((object) ((WebViewer) sender).WindowName);
    }

    private void button_Click(object sender, EventArgs e)
    {
      string name = ((Control) sender).Name;
      try
      {
        if (name == "btnback")
          this.axWebBrowser1.GoBack();
        else
          this.axWebBrowser1.GoForward();
      }
      catch
      {
      }
    }
  }
}

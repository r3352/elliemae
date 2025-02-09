// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.LoadWebPageControl
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using EllieMae.EMLite.RemotingServices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Elli.Web.Host
{
  public class LoadWebPageControl : UserControl
  {
    private WebHost webHost;
    private string webPageURL = string.Empty;
    private string hostPageURL = string.Empty;
    private Dictionary<string, object> webPageParams;
    private IContainer components;
    private Panel pnlBrowser;
    private Button btnConsole;

    public LoadWebPageControl(
      string webPageURL,
      Dictionary<string, object> webPageParams,
      string scope,
      string hostPageUrl = null,
      PostMessageHandler postMessageHandler = null)
    {
      this.InitializeComponent();
      this.webHost = new WebHost(scope, postMessageHandler, new Action(this.UnloadHandler));
      this.pnlBrowser.Controls.Add((Control) this.webHost);
      this.webHost.Dock = DockStyle.Fill;
      this.webPageURL = webPageURL;
      this.hostPageURL = hostPageUrl;
      this.webPageParams = webPageParams;
      this.LoadModule();
      this.btnConsole.Visible = false;
      if (this.btnConsole.Visible)
        return;
      this.SetConsoleButtonVisibility();
    }

    private void SetConsoleButtonVisibility()
    {
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\EnableWebHostDebugMode"))
      {
        if (registryKey == null)
          return;
        this.btnConsole.Visible = true;
      }
    }

    private ModuleUser GetModuleUser()
    {
      UserInfo userInfo = Session.DefaultInstance.UserInfo;
      return new ModuleUser()
      {
        ID = userInfo.Userid,
        LastName = userInfo.LastName,
        FirstName = userInfo.FirstName,
        Email = userInfo.Email
      };
    }

    private void LoadModule()
    {
      ModuleParameters parameters = new ModuleParameters()
      {
        User = this.GetModuleUser(),
        Parameters = this.webPageParams
      };
      if (string.IsNullOrEmpty(this.hostPageURL))
        this.webHost.LoadModule(this.webPageURL, parameters);
      else
        this.webHost.LoadModule(this.hostPageURL, this.webPageURL, parameters);
    }

    public T RaiseEvent<T>(string eventType, object eventParams, int millisecondsTimeout)
    {
      return this.webHost.RaiseEvent<T>("raiseMessage", eventParams, millisecondsTimeout);
    }

    private void UnloadHandler()
    {
    }

    private void btnConsole_Click(object sender, EventArgs e)
    {
      if (this.btnConsole.Text == "Show Console")
      {
        this.webHost.ShowConsole();
        this.btnConsole.Text = "Hide Console";
      }
      else
      {
        this.webHost.HideConsole();
        this.btnConsole.Text = "Show Console";
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlBrowser = new Panel();
      this.btnConsole = new Button();
      this.pnlBrowser.SuspendLayout();
      this.SuspendLayout();
      this.pnlBrowser.BackColor = SystemColors.Control;
      this.pnlBrowser.Controls.Add((Control) this.btnConsole);
      this.pnlBrowser.Dock = DockStyle.Fill;
      this.pnlBrowser.Location = new Point(0, 0);
      this.pnlBrowser.Margin = new Padding(2);
      this.pnlBrowser.Name = "pnlBrowser";
      this.pnlBrowser.Size = new Size(800, 600);
      this.pnlBrowser.TabIndex = 5;
      this.btnConsole.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnConsole.AutoSize = true;
      this.btnConsole.Location = new Point(713, 2);
      this.btnConsole.Margin = new Padding(2);
      this.btnConsole.Name = "btnConsole";
      this.btnConsole.Size = new Size(85, 23);
      this.btnConsole.TabIndex = 2;
      this.btnConsole.Text = "Show Console";
      this.btnConsole.UseVisualStyleBackColor = true;
      this.btnConsole.Click += new EventHandler(this.btnConsole_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlBrowser);
      this.Name = nameof (LoadWebPageControl);
      this.Size = new Size(800, 600);
      this.pnlBrowser.ResumeLayout(false);
      this.pnlBrowser.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

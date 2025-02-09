// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FormBrowser
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using AxSHDocVw;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class FormBrowser : UserControl
  {
    private FrmBrowserHandler browserHandler;
    private IHtmlInput dataObject;
    private IContainer components;
    private AxWebBrowser axBrowser;

    public event EventHandler FormLoaded;

    public FormBrowser() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
      {
        this.releaseBrowserHandler();
        this.components.Dispose();
      }
      base.Dispose(disposing);
    }

    private void createBrowserHandler()
    {
      this.releaseBrowserHandler(true);
      if (this.dataObject == null)
        return;
      this.browserHandler = new FrmBrowserHandler(Session.DefaultInstance, (IWin32Window) this, this.dataObject);
      this.browserHandler.DocumentCompleted += new DocumentCompleteEventHandler(this.OnFormLoaded);
      this.browserHandler.AttachToBrowser(this.axBrowser);
    }

    protected void OnFormLoaded()
    {
      if (this.FormLoaded == null)
        return;
      this.FormLoaded((object) this, EventArgs.Empty);
    }

    private void releaseBrowserHandler() => this.releaseBrowserHandler(false);

    private void releaseBrowserHandler(bool keepAxControl)
    {
      if (this.browserHandler == null)
        return;
      this.browserHandler.ReleaseBrowser(keepAxControl);
      this.browserHandler = (FrmBrowserHandler) null;
    }

    [Browsable(false)]
    public IHtmlInput DataSource
    {
      get => this.dataObject;
      set
      {
        this.dataObject = value;
        this.createBrowserHandler();
      }
    }

    public void OpenForm(InputFormInfo formInfo)
    {
      this.verifyAttached();
      this.browserHandler.OpenForm(formInfo);
    }

    public void OpenForm(InputFormInfo formInfo, Sessions.Session session)
    {
      this.verifyAttached();
      this.browserHandler.OpenForm(formInfo, session);
    }

    public IInputHandler GetInputHandler() => this.browserHandler.GetInputHandler();

    private void verifyAttached()
    {
      if (this.browserHandler == null)
        throw new Exception("The control's data source must be set prior to this operation");
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FormBrowser));
      this.axBrowser = new AxWebBrowser();
      this.axBrowser.BeginInit();
      this.SuspendLayout();
      this.axBrowser.Dock = DockStyle.Fill;
      this.axBrowser.Enabled = true;
      this.axBrowser.Location = new Point(0, 0);
      this.axBrowser.OcxState = (AxHost.State) componentResourceManager.GetObject("axBrowser.OcxState");
      this.axBrowser.Size = new Size(374, 411);
      this.axBrowser.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.axBrowser);
      this.Name = nameof (FormBrowser);
      this.Size = new Size(374, 411);
      this.axBrowser.EndInit();
      this.ResumeLayout(false);
    }
  }
}

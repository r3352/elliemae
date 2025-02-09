// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DefaultTemplateControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using AxSHDocVw;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.InputEngine.Forms;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DefaultTemplateControl : SettingsUserControl
  {
    private System.ComponentModel.Container components;
    private AxWebBrowser axWebBrowser;
    private GroupContainer gcContent;
    private FormDataBase templateData;
    private FrmBrowserHandler brwHandler;

    public DefaultTemplateControl(SetUpContainer setupContainer, FormDataBase templateData)
      : base(setupContainer)
    {
      this.templateData = templateData;
      this.init();
      this.gcContent.Text = this.Title;
    }

    private void init()
    {
      this.InitializeComponent();
      this.brwHandler = new FrmBrowserHandler(Session.DefaultInstance, (IWin32Window) this, (IHtmlInput) this.templateData);
      this.brwHandler.AttachToBrowser(this.axWebBrowser);
      this.brwHandler.OnFieldChanged += new EventHandler(this.brwHandler_FieldChanged);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DefaultTemplateControl));
      this.axWebBrowser = new AxWebBrowser();
      this.gcContent = new GroupContainer();
      this.axWebBrowser.BeginInit();
      this.gcContent.SuspendLayout();
      this.SuspendLayout();
      this.axWebBrowser.Dock = DockStyle.Fill;
      this.axWebBrowser.Enabled = true;
      this.axWebBrowser.Location = new Point(1, 26);
      this.axWebBrowser.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser.OcxState");
      this.axWebBrowser.Size = new Size(530, 421);
      this.axWebBrowser.TabIndex = 2;
      this.gcContent.Controls.Add((Control) this.axWebBrowser);
      this.gcContent.Dock = DockStyle.Fill;
      this.gcContent.Location = new Point(0, 0);
      this.gcContent.Name = "gcContent";
      this.gcContent.Size = new Size(532, 448);
      this.gcContent.TabIndex = 28;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.gcContent);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (DefaultTemplateControl);
      this.Size = new Size(532, 448);
      this.axWebBrowser.EndInit();
      this.gcContent.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (this.components != null)
          this.components.Dispose();
        if (this.brwHandler != null)
          this.brwHandler.ReleaseBrowser();
      }
      base.Dispose(disposing);
    }

    public virtual string Title => "Template";

    protected virtual InputFormInfo GetInputFormInfo() => (InputFormInfo) null;

    public FormDataBase TemplateData => this.templateData;

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      InputFormInfo inputFormInfo = this.GetInputFormInfo();
      object obj = (object) Missing.Value;
      this.axWebBrowser.Navigate(FormStore.GetFormHTMLPath(Session.DefaultInstance, inputFormInfo), ref obj, ref obj, ref obj, ref obj);
      this.setDirtyFlag(false);
    }

    public override void Reset()
    {
      this.brwHandler.RefreshAllControls(true);
      base.Reset();
    }

    private void brwHandler_FieldChanged(object sender, EventArgs e) => this.setDirtyFlag(true);
  }
}

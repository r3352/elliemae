// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.Features.FeaturePromoPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using AxSHDocVw;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.Features
{
  public class FeaturePromoPanel : UserControl
  {
    private AxWebBrowser axWebBrowser1;
    private System.ComponentModel.Container components;
    private EncompassModule module;
    private Sessions.Session session;

    public event EventHandler RefreshFeature;

    public FeaturePromoPanel(EncompassModule module, Sessions.Session session)
    {
      this.module = module;
      this.session = session;
      this.InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FeaturePromoPanel));
      this.axWebBrowser1 = new AxWebBrowser();
      this.axWebBrowser1.BeginInit();
      this.SuspendLayout();
      this.axWebBrowser1.Dock = DockStyle.Fill;
      this.axWebBrowser1.Enabled = true;
      this.axWebBrowser1.Location = new Point(12, 0);
      this.axWebBrowser1.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser1.OcxState");
      this.axWebBrowser1.Size = new Size(477, 386);
      this.axWebBrowser1.TabIndex = 0;
      this.axWebBrowser1.BeforeNavigate2 += new DWebBrowserEvents2_BeforeNavigate2EventHandler(this.axWebBrowser1_BeforeNavigate2);
      this.Controls.Add((Control) this.axWebBrowser1);
      this.Name = nameof (FeaturePromoPanel);
      this.Padding = new Padding(12, 0, 8, 0);
      this.Size = new Size(497, 386);
      this.Load += new EventHandler(this.FeaturePromoPanel_Load);
      this.axWebBrowser1.EndInit();
      this.ResumeLayout(false);
    }

    private void axWebBrowser1_BeforeNavigate2(
      object sender,
      DWebBrowserEvents2_BeforeNavigate2Event e)
    {
      if (e.uRL.ToString().IndexOf("refresh_module") < 0 || this.RefreshFeature == null)
        return;
      this.RefreshFeature((object) this, EventArgs.Empty);
    }

    private void FeaturePromoPanel_Load(object sender, EventArgs e)
    {
      string jumpUrl = WebLink.GetJumpURL("ModulePromo.asp?module=" + (object) this.module, this.session);
      object missing = System.Type.Missing;
      this.axWebBrowser1.Navigate(jumpUrl, ref missing, ref missing, ref missing, ref missing);
    }
  }
}

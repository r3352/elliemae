// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.NormalizedBidTapeTemplate
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common.ThinThick;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls.ThinThick;
using EllieMae.EMLite.RemotingServices;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class NormalizedBidTapeTemplate : SettingsUserControl
  {
    private ThinThickBrowserManager browserManager;
    private IContainer components;
    private ThinThickBrowser Browser;

    public NormalizedBidTapeTemplate(SetUpContainer setupContainer)
      : this(setupContainer, Session.DefaultInstance)
    {
    }

    public NormalizedBidTapeTemplate(SetUpContainer setupContainer, Sessions.Session session)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.browserManager = new ThinThickBrowserManager(Session.DefaultInstance, this.Browser, (IWin32Window) this);
      this.browserManager.NavigateToThinClient(PageRegistry.NormalizedBidTapeTemplate);
    }

    public override bool IsDirty => this.browserManager.IsDirty();

    public override void Reset()
    {
      base.Reset();
      this.browserManager.Reset();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.Browser = new ThinThickBrowser();
      this.SuspendLayout();
      this.Browser.Dock = DockStyle.Fill;
      this.Browser.Location = new Point(0, 0);
      this.Browser.Name = "Browser";
      this.Browser.Size = new Size(690, 434);
      this.Browser.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.Browser);
      this.Name = "AnalysisTools";
      this.Size = new Size(690, 434);
      this.ResumeLayout(false);
    }
  }
}

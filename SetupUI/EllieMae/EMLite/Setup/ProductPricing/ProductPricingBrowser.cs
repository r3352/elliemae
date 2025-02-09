// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ProductPricing.ProductPricingBrowser
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Web.Host.BrowserControls;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ProductPricing
{
  public class ProductPricingBrowser : Form
  {
    private IContainer components;
    private EncWebFormBrowserControl webBrowser;

    public ProductPricingBrowser(string URL, string title)
    {
      this.InitializeComponent();
      this.webBrowser.Navigate(URL);
      this.Text = title;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.webBrowser = BrowserFactory.GetWebBrowserInstance();
      this.SuspendLayout();
      this.webBrowser.Dock = DockStyle.Fill;
      this.webBrowser.Location = new Point(0, 0);
      this.webBrowser.MinimumSize = new Size(20, 20);
      this.webBrowser.Name = "webBrowser";
      this.webBrowser.Size = new Size(742, 495);
      this.webBrowser.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(742, 495);
      this.Controls.Add((Control) this.webBrowser);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (ProductPricingBrowser);
      this.StartPosition = FormStartPosition.CenterParent;
      this.ResumeLayout(false);
    }
  }
}

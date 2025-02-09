// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.OptimalBlueAdmin
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Web.Host.BrowserControls;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class OptimalBlueAdmin : Form
  {
    private IContainer components;
    private EncWebFormBrowserControl browser;

    public OptimalBlueAdmin()
    {
      this.InitializeComponent();
      this.browser.Navigate("https://www.optimalblue.com/ppeconfig/login.aspx");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.browser = BrowserFactory.GetWebBrowserInstance();
      this.SuspendLayout();
      this.browser.Dock = DockStyle.Fill;
      this.browser.Location = new Point(0, 0);
      this.browser.MinimumSize = new Size(20, 20);
      this.browser.Name = "browser";
      this.browser.Size = new Size(531, 412);
      this.browser.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(531, 412);
      this.Controls.Add((Control) this.browser);
      this.MinimizeBox = false;
      this.Name = nameof (OptimalBlueAdmin);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = " Optimal Blue";
      this.ResumeLayout(false);
    }
  }
}

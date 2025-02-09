// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.TPOConnectAdminUrlBrowser
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class TPOConnectAdminUrlBrowser : Form
  {
    private IContainer components;
    private WebBrowser browser;

    public TPOConnectAdminUrlBrowser(string URL, string title)
    {
      this.InitializeComponent();
      this.browser.Navigate(URL);
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
      this.browser = new WebBrowser();
      this.SuspendLayout();
      this.browser.Dock = DockStyle.Fill;
      this.browser.Location = new Point(0, 0);
      this.browser.MinimumSize = new Size(20, 20);
      this.browser.Name = "browser";
      this.browser.Size = new Size(994, 613);
      this.browser.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(994, 613);
      this.Controls.Add((Control) this.browser);
      this.Name = nameof (TPOConnectAdminUrlBrowser);
      this.ShowIcon = false;
      this.ResumeLayout(false);
    }
  }
}

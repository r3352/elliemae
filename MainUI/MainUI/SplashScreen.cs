// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.SplashScreen
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class SplashScreen : Form
  {
    private System.ComponentModel.Container components;

    public SplashScreen() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SplashScreen));
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackgroundImage = (Image) componentResourceManager.GetObject("$this.BackgroundImage");
      this.ClientSize = new Size(385, 240);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (SplashScreen);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (SplashScreen);
      this.ResumeLayout(false);
    }
  }
}

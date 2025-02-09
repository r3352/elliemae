// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SetupHelp
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SetupHelp : UserControl
  {
    private System.ComponentModel.Container components;
    protected Image bgImage;
    protected Color bgColor;
    protected Font defaultItemTitleFont = new Font("Arial", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
    protected Font defaultSubItemTitleFont = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
    protected Color defaultBackColor = Color.Transparent;
    protected Color defaultSubItemTitleForeColor = Color.FromArgb(34, 70, 106);

    public SetupHelp()
    {
      this.InitializeComponent();
      this.Font = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.BackgroundImage = (Image) null;
      this.Dock = DockStyle.Fill;
      this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
      this.bgImage = Image.FromFile(AssemblyResolver.GetResourceFileFullPath(SystemSettings.SettingsSplashImgFileRelPath, SystemSettings.LocalAppDir));
      this.bgColor = Color.FromArgb(244, 234, 212);
      this.ResizeRedraw = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.Name = nameof (SetupHelp);
      this.Size = new Size(576, 452);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      e.Graphics.Clear(this.bgColor);
      int x = this.Width - this.bgImage.Width;
      int y = this.Height - this.bgImage.Height;
      e.Graphics.DrawImage(this.bgImage, x, y);
    }
  }
}

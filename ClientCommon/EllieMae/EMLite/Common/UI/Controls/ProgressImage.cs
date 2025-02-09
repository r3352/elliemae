// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.Controls.ProgressImage
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI.Controls
{
  public class ProgressImage : Control
  {
    private readonly Timer _aniTimer;
    private Image _image;
    private float _rx;

    public ProgressImage()
    {
      this.SetStyle(ControlStyles.UserPaint, true);
      this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
      this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      this._aniTimer = new Timer() { Interval = 75 };
      this._aniTimer.Tick += new EventHandler(this._aniTimer_Tick);
      this._aniTimer.Start();
    }

    private void _aniTimer_Tick(object sender, EventArgs e) => this.Invalidate(true);

    public Icon ImageIcon
    {
      set => this._image = (Image) value.ToBitmap();
    }

    protected override void Dispose(bool disposing)
    {
      this._aniTimer.Stop();
      base.Dispose(disposing);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      Graphics graphics = Graphics.FromImage(this._image);
      graphics.TranslateTransform((float) (this._image.Width / 2), (float) (this._image.Height / 2));
      graphics.RotateTransform(10f);
      graphics.TranslateTransform((float) (-(double) this._image.Width / 2.0), (float) (-(double) this._image.Height / 2.0));
      graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
      graphics.DrawImage(this._image, new Point(0, 0));
      graphics.Dispose();
      e.Graphics.DrawImage(this._image, new Point(this.Width - this._image.Width, 0));
      this._rx += 0.2f;
    }
  }
}

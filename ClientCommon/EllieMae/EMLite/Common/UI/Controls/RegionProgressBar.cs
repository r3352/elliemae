// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.Controls.RegionProgressBar
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI.Controls
{
  public class RegionProgressBar : ProgressBar
  {
    private List<RegionProgressBar.ProgressBarRange> _ranges = new List<RegionProgressBar.ProgressBarRange>();
    private RegionProgressBar.ProgressBarRange _currentRange;
    private System.Windows.Forms.Timer stepTimer = new System.Windows.Forms.Timer();

    public RegionProgressBar()
    {
      this.SetStyle(ControlStyles.UserPaint, true);
      this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
      new System.Windows.Forms.Timer() { Interval = 500 }.Tick += new EventHandler(this.AniTimer_Tick);
    }

    private void AniTimer_Tick(object sender, EventArgs e)
    {
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
    }

    public void SetProgress(int value, Color color)
    {
      if (this._currentRange == null)
      {
        this._currentRange = new RegionProgressBar.ProgressBarRange()
        {
          Start = 0,
          Color = color
        };
        this._ranges.Add(this._currentRange);
      }
      else if (this._currentRange.Color != color)
      {
        this._currentRange.End = this.Value;
        this._currentRange = new RegionProgressBar.ProgressBarRange()
        {
          Start = this.Value,
          Color = color
        };
        this._ranges.Add(this._currentRange);
      }
      this._currentRange.End = value;
      this.Value = value;
      this.Invalidate();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
      int end = this._currentRange.End;
      for (int i = this.Value; i < (int) this.stepTimer.Tag; i++)
      {
        this.Invoke((Delegate) (() => this.Value = i));
        this._currentRange.End = i;
        this.Invoke((Delegate) (() => this.Invalidate(true)));
        Thread.Sleep(20);
      }
      this.stepTimer.Stop();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      using (Image image = (Image) new Bitmap(this.Width, this.Height))
      {
        using (Graphics g = Graphics.FromImage(image))
        {
          Rectangle rectangle = new Rectangle(0, 0, this.Width, this.Height);
          if (ProgressBarRenderer.IsSupported)
            ProgressBarRenderer.DrawHorizontalBar(g, rectangle);
          rectangle.Inflate(new Size(-2, -2));
          int num = 0;
          foreach (RegionProgressBar.ProgressBarRange range in this._ranges)
          {
            rectangle.Width = (int) ((double) (this.Width - 2) * (((double) range.End - (double) range.Start) / (double) this.Maximum));
            if (rectangle.Width == 0)
              rectangle.Width = 1;
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, this.BackColor, range.Color, LinearGradientMode.Vertical);
            g.FillRectangle((Brush) linearGradientBrush, 2 + num, 2, rectangle.Width, rectangle.Height);
            num += rectangle.Width;
          }
          e.Graphics.DrawImage(image, 0, 0);
          image.Dispose();
        }
      }
    }

    private class ProgressBarRange
    {
      public int Start { get; set; }

      public int End { get; set; }

      public Color Color { get; set; }
    }
  }
}

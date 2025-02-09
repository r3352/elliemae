// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Services.AdvertisementControl
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass.Services
{
  public class AdvertisementControl : UserControl
  {
    private IContainer components;
    private Panel pnl;

    public AdvertisementControl() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnl = new Panel();
      this.SuspendLayout();
      this.pnl.Location = new Point(32, 32);
      this.pnl.Name = "pnl";
      this.pnl.Size = new Size(248, 216);
      this.pnl.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnl);
      this.Name = "PartnerAdvertisementControl";
      this.Size = new Size(317, 288);
      this.ResumeLayout(false);
    }
  }
}

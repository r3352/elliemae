// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeLoanUpdateActionClear
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeLoanUpdateActionClear : UserControl
  {
    private IContainer components;
    private EllieMae.EMLite.UI.LinkLabel lnkLblClear;

    public TradeLoanUpdateActionClear() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lnkLblClear = new EllieMae.EMLite.UI.LinkLabel();
      this.SuspendLayout();
      this.lnkLblClear.AutoSize = true;
      this.lnkLblClear.Location = new Point(0, 0);
      this.lnkLblClear.Name = "lnkLblClear";
      this.lnkLblClear.Size = new Size(31, 13);
      this.lnkLblClear.TabIndex = 0;
      this.lnkLblClear.Text = "Clear";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.lnkLblClear);
      this.Name = nameof (TradeLoanUpdateActionClear);
      this.Size = new Size(39, 13);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

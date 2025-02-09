// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CommitmentsPoolAllocateAmountControl
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class CommitmentsPoolAllocateAmountControl : UserControl
  {
    private IContainer components;
    private Label lblOpenAmt;
    private Label lblAllocatedPoolAmt;
    private Label lblAllocatedPoolAmtCaption;
    private Label lblOpenAmtCaption;

    public CommitmentsPoolAllocateAmountControl() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblOpenAmt = new Label();
      this.lblAllocatedPoolAmt = new Label();
      this.lblAllocatedPoolAmtCaption = new Label();
      this.lblOpenAmtCaption = new Label();
      this.SuspendLayout();
      this.lblOpenAmt.AutoSize = true;
      this.lblOpenAmt.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblOpenAmt.Location = new Point(108, 3);
      this.lblOpenAmt.Name = "lblOpenAmt";
      this.lblOpenAmt.Size = new Size(61, 14);
      this.lblOpenAmt.TabIndex = 29;
      this.lblOpenAmt.Text = "#########";
      this.lblAllocatedPoolAmt.AutoSize = true;
      this.lblAllocatedPoolAmt.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblAllocatedPoolAmt.Location = new Point(382, 3);
      this.lblAllocatedPoolAmt.Name = "lblAllocatedPoolAmt";
      this.lblAllocatedPoolAmt.Size = new Size(61, 14);
      this.lblAllocatedPoolAmt.TabIndex = 31;
      this.lblAllocatedPoolAmt.Text = "#########";
      this.lblAllocatedPoolAmtCaption.AutoSize = true;
      this.lblAllocatedPoolAmtCaption.Location = new Point(219, 3);
      this.lblAllocatedPoolAmtCaption.Name = "lblAllocatedPoolAmtCaption";
      this.lblAllocatedPoolAmtCaption.Size = new Size(153, 13);
      this.lblAllocatedPoolAmtCaption.TabIndex = 30;
      this.lblAllocatedPoolAmtCaption.Text = "Allocated Commitment Amount:";
      this.lblOpenAmtCaption.AutoSize = true;
      this.lblOpenAmtCaption.Location = new Point(5, 3);
      this.lblOpenAmtCaption.Name = "lblOpenAmtCaption";
      this.lblOpenAmtCaption.Size = new Size(99, 13);
      this.lblOpenAmtCaption.TabIndex = 28;
      this.lblOpenAmtCaption.Text = "Open Pool Amount:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.lblOpenAmt);
      this.Controls.Add((Control) this.lblAllocatedPoolAmt);
      this.Controls.Add((Control) this.lblAllocatedPoolAmtCaption);
      this.Controls.Add((Control) this.lblOpenAmtCaption);
      this.Name = nameof (CommitmentsPoolAllocateAmountControl);
      this.Size = new Size(700, 19);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.ContractProfitControl
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class ContractProfitControl : UserControl
  {
    private IContainer components;
    private Label lblCount;
    private Label lblTradeAmt;
    private Label lblAssignedAmt;
    private Label lblGainLoss;
    private Label lblGainLossCaption;
    private Label lblAssignedAmtCaption;
    private Label lblTradeAmtCaption;
    private Label lblCountCaption;

    public ContractProfitControl() => this.InitializeComponent();

    public void Calculate(GVItemCollection trades)
    {
      int num1 = 0;
      Decimal num2 = 0M;
      Decimal num3 = 0M;
      Decimal num4 = 0M;
      foreach (GVItem trade in (IEnumerable<GVItem>) trades)
      {
        ++num1;
        num2 += Utils.ParseDecimal(trade.SubItems[3].Value);
        num3 += Utils.ParseDecimal(trade.SubItems[4].Value);
        num4 += Utils.ParseDecimal(trade.SubItems[5].Value);
      }
      this.lblCount.Text = num1.ToString("#,##0");
      this.lblTradeAmt.Text = "$" + num2.ToString("#,##0");
      this.lblAssignedAmt.Text = "$" + num3.ToString("#,##0");
      this.lblGainLoss.Text = "$" + num4.ToString("#,##0");
      this.layoutControls();
    }

    private void layoutControls()
    {
      int num = 10;
      this.lblCountCaption.Left = 0;
      this.lblCount.Left = this.lblCountCaption.Right;
      this.lblTradeAmtCaption.Left = this.lblCount.Right + num;
      this.lblTradeAmt.Left = this.lblTradeAmtCaption.Right;
      this.lblAssignedAmtCaption.Left = this.lblTradeAmt.Right + num;
      this.lblAssignedAmt.Left = this.lblAssignedAmtCaption.Right;
      this.lblGainLossCaption.Left = this.lblAssignedAmt.Right + num;
      this.lblGainLoss.Left = this.lblGainLossCaption.Right;
    }

    public void SetMasterSummaryInfo(CorrespondentMasterSummaryInfo info)
    {
      this.lblCount.Text = info.TotalTrades.ToString("#,##0");
      this.lblTradeAmt.Text = "$" + info.TotalTradeAmount.ToString("#,##0");
      this.lblAssignedAmt.Text = "$" + info.TotalAssignedLoanAmount.ToString("#,##0");
      this.lblGainLoss.Text = "$" + info.GainLossAmount.ToString("#,##0");
      this.layoutControls();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblCount = new Label();
      this.lblTradeAmt = new Label();
      this.lblAssignedAmt = new Label();
      this.lblGainLoss = new Label();
      this.lblGainLossCaption = new Label();
      this.lblAssignedAmtCaption = new Label();
      this.lblTradeAmtCaption = new Label();
      this.lblCountCaption = new Label();
      this.SuspendLayout();
      this.lblCount.AutoSize = true;
      this.lblCount.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblCount.Location = new Point(96, 1);
      this.lblCount.Name = "lblCount";
      this.lblCount.Size = new Size(39, 13);
      this.lblCount.TabIndex = 25;
      this.lblCount.Text = "####";
      this.lblTradeAmt.AutoSize = true;
      this.lblTradeAmt.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTradeAmt.Location = new Point((int) byte.MaxValue, 1);
      this.lblTradeAmt.Name = "lblTradeAmt";
      this.lblTradeAmt.Size = new Size(79, 13);
      this.lblTradeAmt.TabIndex = 27;
      this.lblTradeAmt.Text = "#########";
      this.lblAssignedAmt.AutoSize = true;
      this.lblAssignedAmt.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblAssignedAmt.Location = new Point(447, 1);
      this.lblAssignedAmt.Name = "lblAssignedAmt";
      this.lblAssignedAmt.Size = new Size(79, 13);
      this.lblAssignedAmt.TabIndex = 29;
      this.lblAssignedAmt.Text = "#########";
      this.lblGainLoss.AutoSize = true;
      this.lblGainLoss.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblGainLoss.Location = new Point(618, 1);
      this.lblGainLoss.Name = "lblGainLoss";
      this.lblGainLoss.Size = new Size(71, 13);
      this.lblGainLoss.TabIndex = 35;
      this.lblGainLoss.Text = "########";
      this.lblGainLossCaption.AutoSize = true;
      this.lblGainLossCaption.Location = new Point(534, 1);
      this.lblGainLossCaption.Name = "lblGainLossCaption";
      this.lblGainLossCaption.Size = new Size(86, 13);
      this.lblGainLossCaption.TabIndex = 34;
      this.lblGainLossCaption.Text = "Total Gain/Loss:";
      this.lblAssignedAmtCaption.AutoSize = true;
      this.lblAssignedAmtCaption.Location = new Point(344, 1);
      this.lblAssignedAmtCaption.Name = "lblAssignedAmtCaption";
      this.lblAssignedAmtCaption.Size = new Size(101, 13);
      this.lblAssignedAmtCaption.TabIndex = 28;
      this.lblAssignedAmtCaption.Text = "Total Assigned Amt:";
      this.lblTradeAmtCaption.AutoSize = true;
      this.lblTradeAmtCaption.Location = new Point(142, 1);
      this.lblTradeAmtCaption.Name = "lblTradeAmtCaption";
      this.lblTradeAmtCaption.Size = new Size(112, 13);
      this.lblTradeAmtCaption.TabIndex = 26;
      this.lblTradeAmtCaption.Text = "Total Trade/Pool Amt:";
      this.lblCountCaption.AutoSize = true;
      this.lblCountCaption.Location = new Point(0, 1);
      this.lblCountCaption.Name = "lblCountCaption";
      this.lblCountCaption.Size = new Size(96, 13);
      this.lblCountCaption.TabIndex = 24;
      this.lblCountCaption.Text = "# of Trades/Pools:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.lblCount);
      this.Controls.Add((Control) this.lblTradeAmt);
      this.Controls.Add((Control) this.lblAssignedAmt);
      this.Controls.Add((Control) this.lblGainLoss);
      this.Controls.Add((Control) this.lblGainLossCaption);
      this.Controls.Add((Control) this.lblAssignedAmtCaption);
      this.Controls.Add((Control) this.lblTradeAmtCaption);
      this.Controls.Add((Control) this.lblCountCaption);
      this.Name = nameof (ContractProfitControl);
      this.Size = new Size(705, 17);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

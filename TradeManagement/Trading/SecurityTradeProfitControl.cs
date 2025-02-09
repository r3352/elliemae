// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.SecurityTradeProfitControl
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class SecurityTradeProfitControl : UserControl
  {
    private List<int> removedInMemoryAssignments = new List<int>();
    private IContainer components;
    private Label lblOpenAmt;
    private Label lblTotalPairOffAmt;
    private Label lblGainLoss;
    private Label lblGainLossCaption;
    private Label lblTotalPairOffAmtCaption;
    private Label lblOpenAmtCaption;

    public SecurityTradeProfitControl() => this.InitializeComponent();

    public Decimal TotalPairOffAmount
    {
      get => Utils.ParseDecimal((object) this.lblTotalPairOffAmt.Text, 0M) * -1M;
    }

    public Decimal TotalPairOffGainLoss => Utils.ParseDecimal((object) this.lblGainLoss.Text, 0M);

    public Decimal OpenAmount => Utils.ParseDecimal((object) this.lblOpenAmt.Text, 0M);

    public void Calculate(SecurityTradeInfo trade, MbsPoolAssignment[] mbsPoolAssignments)
    {
      if (trade == null)
        return;
      SecurityTradeAssignment[] assignments = (SecurityTradeAssignment[]) null;
      if (Session.SecurityTradeManager.GetTradeAssigments(trade.TradeID) != null)
        assignments = ((IEnumerable<SecurityTradeAssignment>) Session.SecurityTradeManager.GetTradeAssigments(trade.TradeID)).Where<SecurityTradeAssignment>((Func<SecurityTradeAssignment, bool>) (p => !this.removedInMemoryAssignments.Contains(Utils.ParseInt((object) p.AssigneeTradeID, -1)))).ToArray<SecurityTradeAssignment>();
      Decimal pairOffAmount = trade.Calculation.CalculatePairOffAmount();
      Decimal offGainLossAmount = trade.Calculation.CalculatePairOffGainLossAmount();
      Decimal assignedAmount = SecurityTradeCalculation.CalculateAssignedAmount(assignments);
      Decimal netProfitAmount = SecurityTradeCalculation.CalculateNetProfitAmount(assignments);
      Decimal allocatedAmount = MbsPoolCalculation.CalculateAllocatedAmount(mbsPoolAssignments);
      Decimal openAmount = SecurityTradeCalculation.CalculateOpenAmount(trade.TradeAmount, assignedAmount, pairOffAmount, allocatedAmount);
      Decimal pairOffGainLossAmount = offGainLossAmount;
      Decimal totalGainLossAmount = SecurityTradeCalculation.CalculateTotalGainLossAmount(netProfitAmount, pairOffGainLossAmount);
      this.lblOpenAmt.Text = "$" + openAmount.ToString("#,##0");
      this.lblTotalPairOffAmt.Text = "$" + pairOffAmount.ToString("#,##0");
      this.lblGainLoss.Text = "$" + totalGainLossAmount.ToString("#,##0");
      this.layoutControls();
    }

    private void layoutControls()
    {
      int num = 10;
      this.lblOpenAmtCaption.Left = 0;
      this.lblOpenAmt.Left = this.lblOpenAmtCaption.Right;
      this.lblTotalPairOffAmtCaption.Left = this.lblOpenAmt.Right + num;
      this.lblTotalPairOffAmt.Left = this.lblTotalPairOffAmtCaption.Right;
      this.lblGainLossCaption.Left = this.lblTotalPairOffAmt.Right + num;
      this.lblGainLoss.Left = this.lblGainLossCaption.Right;
    }

    public void RemovedAssignment(List<int> removedTrade)
    {
      this.removedInMemoryAssignments = removedTrade;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblOpenAmt = new Label();
      this.lblTotalPairOffAmt = new Label();
      this.lblGainLoss = new Label();
      this.lblGainLossCaption = new Label();
      this.lblTotalPairOffAmtCaption = new Label();
      this.lblOpenAmtCaption = new Label();
      this.SuspendLayout();
      this.lblOpenAmt.AutoSize = true;
      this.lblOpenAmt.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblOpenAmt.Location = new Point(85, 2);
      this.lblOpenAmt.Name = "lblOpenAmt";
      this.lblOpenAmt.Size = new Size(61, 14);
      this.lblOpenAmt.TabIndex = 17;
      this.lblOpenAmt.Text = "#########";
      this.lblTotalPairOffAmt.AutoSize = true;
      this.lblTotalPairOffAmt.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblTotalPairOffAmt.Location = new Point(300, 2);
      this.lblTotalPairOffAmt.Name = "lblTotalPairOffAmt";
      this.lblTotalPairOffAmt.Size = new Size(61, 14);
      this.lblTotalPairOffAmt.TabIndex = 19;
      this.lblTotalPairOffAmt.Text = "#########";
      this.lblGainLoss.AutoSize = true;
      this.lblGainLoss.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblGainLoss.Location = new Point(528, 2);
      this.lblGainLoss.Name = "lblGainLoss";
      this.lblGainLoss.Size = new Size(55, 14);
      this.lblGainLoss.TabIndex = 23;
      this.lblGainLoss.Text = "########";
      this.lblGainLossCaption.AutoSize = true;
      this.lblGainLossCaption.Location = new Point(437, 2);
      this.lblGainLossCaption.Name = "lblGainLossCaption";
      this.lblGainLossCaption.Size = new Size(84, 14);
      this.lblGainLossCaption.TabIndex = 22;
      this.lblGainLossCaption.Text = "Total Gain/Loss:";
      this.lblTotalPairOffAmtCaption.AutoSize = true;
      this.lblTotalPairOffAmtCaption.Location = new Point(221, 2);
      this.lblTotalPairOffAmtCaption.Name = "lblTotalPairOffAmtCaption";
      this.lblTotalPairOffAmtCaption.Size = new Size(73, 14);
      this.lblTotalPairOffAmtCaption.TabIndex = 18;
      this.lblTotalPairOffAmtCaption.Text = "Total Pair-Off:";
      this.lblOpenAmtCaption.AutoSize = true;
      this.lblOpenAmtCaption.Location = new Point(7, 2);
      this.lblOpenAmtCaption.Name = "lblOpenAmtCaption";
      this.lblOpenAmtCaption.Size = new Size(75, 14);
      this.lblOpenAmtCaption.TabIndex = 16;
      this.lblOpenAmtCaption.Text = "Open Amount:";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.lblOpenAmt);
      this.Controls.Add((Control) this.lblTotalPairOffAmt);
      this.Controls.Add((Control) this.lblGainLoss);
      this.Controls.Add((Control) this.lblGainLossCaption);
      this.Controls.Add((Control) this.lblTotalPairOffAmtCaption);
      this.Controls.Add((Control) this.lblOpenAmtCaption);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (SecurityTradeProfitControl);
      this.Size = new Size(700, 18);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeProfitControl
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeProfitControl : UserControl
  {
    private static readonly string[] requiredFields = new string[7]
    {
      "Loan.TotalLoanAmount",
      "Loan.LoanRate",
      "Loan.Amortization",
      "Loan.ARMMargin",
      "Loan.ARMLifeCap",
      "Loan.ARMFloorRate",
      "Loan.ARMFirstRateAdjCap"
    };
    private Decimal wac;
    private Decimal wam;
    private Decimal waceil;
    private Decimal wafloor;
    private Decimal wachange;
    private int wacCount;
    private int wamCount;
    private int waceilCount;
    private int wafloorCount;
    private int wachangeCount;
    private Decimal assignedAmt;
    private Decimal pairoffamount;
    private Decimal deliveredAmt;
    private Decimal purchasedAmt;
    private Decimal rejectedAmt;
    private TradeInfoObj trade;
    private TradeStatsPopup tradeStatsPopup;
    private bool showPairOffAmt = true;
    private IContainer components;
    private Label lblCount;
    private Label lblAssignedAmt;
    private Label lblOpenAmt;
    private Label lblWAC;
    private Label lblGainLoss;
    private Label lblGainLossCaption;
    private Label lblWACCaption;
    private Label lblOpenAmtCaption;
    private Label lblCountCaption;
    private Label lblMinimumAmt;
    private Label lblMinimumAmtCaption;
    private Label lblMaximumAmt;
    private Label lblMaximumAmtCaption;
    private Label lblPairoffAmtCaption;
    private Label lblPairoffAmt;
    private Label lblDeliveredAmt;
    private Label lblPurchasedAmt;
    private Label lblRejectedAmt;
    private Label lblAssignedAmtTitle;
    private Label lblDeliveredAmtTitle;
    private Label lblPurchasedAmtTitle;
    private Label lblRejectedAmtTitle;

    public TradeProfitControl() => this.InitializeComponent();

    public void Calculate(TradeInfoObj trade, IEnumerable<PipelineInfo> loans)
    {
      this.showPairOffAmt = trade.TradeType == TradeType.LoanTrade || trade.TradeType == TradeType.CorrespondentTrade || trade.TradeType == TradeType.GSECommitment;
      this.trade = trade;
      Decimal tradeAmount = trade.TradeAmount;
      Decimal tolerance = trade.Tolerance;
      if (trade is GSECommitmentInfo)
      {
        this.lblMinimumAmt.Text = ((GSECommitmentInfo) trade).MinDeliveryAmount.ToString("#,##0;;\"\"");
        this.lblMaximumAmt.Text = ((GSECommitmentInfo) trade).MaxDeliveryAmount.ToString("#,##0;;\"\"");
      }
      else
      {
        this.lblMinimumAmt.Text = (tradeAmount - tradeAmount * tolerance / 100M).ToString("#,##0;;\"\"");
        this.lblMaximumAmt.Text = (tradeAmount + tradeAmount * tolerance / 100M).ToString("#,##0;;\"\"");
      }
      Decimal num1 = this.getTradeAmount(trade) > 0M ? -1M * this.getTradeAmount(trade) : this.getTradeAmount(trade);
      if (this.showPairOffAmt)
      {
        if (trade.TradeType == TradeType.GSECommitment)
          this.lblPairoffAmt.Text = "$" + trade.PairOffAmount.ToString("#,##0");
        else
          this.lblPairoffAmt.Text = "$" + num1.ToString("#,##0");
      }
      int num2 = 0;
      string str1 = (string) null;
      bool flag = true;
      this.assignedAmt = 0M;
      this.pairoffamount = 0M;
      Decimal num3 = 0M;
      Decimal num4 = 0M;
      Decimal num5 = 0M;
      Decimal num6 = 0M;
      Decimal num7 = 0M;
      Decimal num8 = 0M;
      Decimal num9 = 0M;
      Decimal num10 = 0M;
      Decimal num11 = 0M;
      Decimal num12 = 0M;
      this.wacCount = this.wamCount = this.waceilCount = this.wafloorCount = this.wachangeCount = 0;
      SecurityTradeInfo secTradeInfo = (SecurityTradeInfo) null;
      if (trade.TradeType == TradeType.LoanTrade && ((LoanTradeInfo) trade).SecurityTradeID > 0)
        secTradeInfo = Session.SecurityTradeManager.GetTrade(((LoanTradeInfo) trade).SecurityTradeID);
      foreach (PipelineInfo loan in loans)
      {
        Hashtable info = loan.Info;
        Decimal num13 = Utils.ParseDecimal(info[(object) "Loan.TotalLoanAmount"]);
        string str2 = string.Concat(info[(object) "Loan.Amortization"]);
        if (str1 == null)
          str1 = str2;
        if (str2 != str1)
          flag = false;
        ++num2;
        this.assignedAmt += num13;
        Decimal num14 = Utils.ParseDecimal(info[(object) "Loan.LoanRate"]);
        if (num14 > 0M)
        {
          num8 += num13;
          ++this.wacCount;
          num3 += num13 * num14 / 100M;
        }
        if (str2 == "AdjustableRate" || str2 == "OtherAmortizationType")
        {
          Decimal num15 = Utils.ParseDecimal(info[(object) "Loan.ARMMargin"]);
          if (num15 > 0M)
          {
            num9 += num13;
            ++this.wamCount;
            num4 += num13 * num15 / 100M;
          }
          if (num14 > 0M && Utils.ParseDecimal(info[(object) "Loan.ARMLifeCap"]) > 0M)
          {
            Decimal num16 = num14 + Utils.ParseDecimal(info[(object) "Loan.ARMLifeCap"]);
            if (num16 > 0M)
            {
              num10 += num13;
              ++this.waceilCount;
              num5 += num13 * num16 / 100M;
            }
          }
          Decimal num17 = Utils.ParseDecimal(info[(object) "Loan.ARMFloorRate"]);
          if (num17 > 0M)
          {
            num11 += num13;
            ++this.wafloorCount;
            num6 += num13 * num17 / 100M;
          }
          Decimal num18 = Utils.ParseDecimal(info[(object) "Loan.ARMFirstRateAdjCap"]);
          if (num18 > 0M)
          {
            num12 += num13;
            ++this.wachangeCount;
            num7 += num13 * num18 / 100M;
          }
        }
      }
      this.wac = this.wam = this.waceil = this.wafloor = this.wachange = Decimal.MinValue;
      if (flag)
      {
        if (num8 > 0M)
          this.wac = num3 / num8;
        if (num9 > 0M)
          this.wam = num4 / num9;
        if (num10 > 0M)
          this.waceil = num5 / num10;
        if (num11 > 0M)
          this.wafloor = num6 / num11;
        if (num12 > 0M)
          this.wachange = num7 / num12;
      }
      this.lblCount.Text = num2.ToString("#,##0");
      this.lblAssignedAmt.Text = "$" + this.assignedAmt.ToString("#,##0");
      this.pairoffamount = this.getTradeAmount(trade);
      if (trade.TradeAmount > 0M)
        this.lblOpenAmt.Text = "$" + (trade.TradeAmount - this.assignedAmt + this.getDisplayTradeAmount(trade)).ToString("#,##0");
      else if (this.getDisplayTradeAmount(trade) != 0M)
        this.lblOpenAmt.Text = this.getDisplayTradeAmount(trade).ToString("#,##0");
      else
        this.lblOpenAmt.Text = "N/A";
      Decimal num19;
      if (this.wac == Decimal.MinValue)
      {
        this.lblWAC.Text = "N/A";
      }
      else
      {
        Label lblWac = this.lblWAC;
        num19 = 100M * this.wac;
        string str3 = num19.ToString("0.000") + "%";
        lblWac.Text = str3;
      }
      Label lblGainLoss = this.lblGainLoss;
      num19 = this.calculateGainLoss(trade, loans, secTradeInfo) + this.getDisplayCalculatedPairOffFee(trade);
      string str4 = "$" + num19.ToString("#,##0");
      lblGainLoss.Text = str4;
      if (trade is CorrespondentTradeInfo)
      {
        this.assignedAmt = CorrespondentTradeCalculation.CalculateAssignedAmount(loans.ToArray<PipelineInfo>());
        this.deliveredAmt = 0M;
        this.purchasedAmt = 0M;
        this.rejectedAmt = 0M;
        this.lblAssignedAmt.Text = "$" + this.assignedAmt.ToString("#,##0");
        Label lblOpenAmt = this.lblOpenAmt;
        num19 = CorrespondentTradeCalculation.CalculateOpenAmount(trade.TradeAmount, ((CorrespondentTradeInfo) trade).CorrespondentTradePairOffs, loans.ToArray<PipelineInfo>());
        string str5 = "$" + num19.ToString("#,##0");
        lblOpenAmt.Text = str5;
        List<LoanSummaryExtension> assignedLoanList = ((CorrespondentTradeInfo) trade).AssignedLoanList;
        if (assignedLoanList != null)
        {
          foreach (LoanSummaryExtension summaryExtension in assignedLoanList)
          {
            if ((summaryExtension.ReceivedDate != DateTime.MinValue || summaryExtension.SubmittedForReviewDate != DateTime.MinValue) && summaryExtension.PurchaseDate == DateTime.MinValue && summaryExtension.RejectedDate == DateTime.MinValue && summaryExtension.WithdrawalRequestedDate == DateTime.MinValue && summaryExtension.CancelledDate == DateTime.MinValue && summaryExtension.VoidedDate == DateTime.MinValue && summaryExtension.WithdrawnDate == DateTime.MinValue)
              this.deliveredAmt += summaryExtension.LoanAmount;
            if (summaryExtension.PurchaseDate != DateTime.MinValue && summaryExtension.RejectedDate == DateTime.MinValue && summaryExtension.WithdrawalRequestedDate == DateTime.MinValue && summaryExtension.CancelledDate == DateTime.MinValue && summaryExtension.VoidedDate == DateTime.MinValue && summaryExtension.WithdrawnDate == DateTime.MinValue)
              this.purchasedAmt += summaryExtension.PurchasedPrinciple;
            if (summaryExtension.RejectedDate != DateTime.MinValue)
              this.rejectedAmt += summaryExtension.LoanAmount;
          }
        }
        this.lblDeliveredAmt.Text = "$" + this.deliveredAmt.ToString("#, ##0");
        this.lblPurchasedAmt.Text = "$" + this.purchasedAmt.ToString("#, ##0");
        this.lblRejectedAmt.Text = "$" + this.rejectedAmt.ToString("#, ##0");
      }
      this.layoutControls(trade);
    }

    private Decimal getTradeAmount(TradeInfoObj trade)
    {
      if (trade.TradeType == TradeType.LoanTrade)
        return ((LoanTradeInfo) trade).LoanTradePairOffs.GetTradeAmount();
      if (trade.TradeType == TradeType.CorrespondentTrade)
        return CorrespondentTradeCalculation.CalculatePairOffAmount(((CorrespondentTradeInfo) trade).CorrespondentTradePairOffs);
      return trade.TradeType == TradeType.GSECommitment ? ((GSECommitmentInfo) trade).GSECommitmentPairOffs.GetTradeAmount() : 0M;
    }

    private Decimal getDisplayTradeAmount(TradeInfoObj trade)
    {
      if (trade.TradeType == TradeType.LoanTrade)
        return ((LoanTradeInfo) trade).LoanTradePairOffs.GetDisplayTradeAmount();
      if (trade.TradeType == TradeType.CorrespondentTrade)
        return ((CorrespondentTradeInfo) trade).CorrespondentTradePairOffs.GetDisplayTradeAmount();
      return trade.TradeType == TradeType.GSECommitment ? trade.PairOffAmount : 0M;
    }

    private Decimal calculateGainLoss(
      TradeInfoObj trade,
      IEnumerable<PipelineInfo> loans,
      SecurityTradeInfo secTradeInfo)
    {
      if (trade.TradeType == TradeType.LoanTrade)
        return ((LoanTradeInfo) trade).CalculateGainLoss(loans, secTradeInfo);
      if (trade.TradeType == TradeType.MbsPool)
        return ((MbsPoolInfo) trade).CalculateGainLoss(loans);
      if (trade.TradeType == TradeType.GSECommitment)
        return ((GSECommitmentInfo) trade).CalculateGainLoss(loans);
      return trade.TradeType == TradeType.CorrespondentTrade ? CorrespondentTradeCalculation.CalculateGainLoss(loans, (CorrespondentTradeInfo) trade) : 0M;
    }

    private Decimal getDisplayCalculatedPairOffFee(TradeInfoObj trade)
    {
      if (trade.TradeType == TradeType.LoanTrade)
        return ((LoanTradeInfo) trade).LoanTradePairOffs.GetDisplayCalculatedPairOffFee();
      if (trade.TradeType == TradeType.CorrespondentTrade)
        return ((CorrespondentTradeInfo) trade).CorrespondentTradePairOffs.GetDisplayCalculatedPairOffFee();
      return trade.TradeType == TradeType.GSECommitment ? ((GSECommitmentInfo) trade).GetDisplayCalculatedPairOffFee() : 0M;
    }

    private void layoutControls(TradeInfoObj trade)
    {
      int num = 10;
      this.lblCountCaption.Left = 0;
      this.lblCount.Left = this.lblCountCaption.Right;
      this.lblMinimumAmtCaption.Left = this.lblCount.Right + num;
      this.lblMinimumAmt.Left = this.lblMinimumAmtCaption.Right;
      this.lblMaximumAmtCaption.Left = this.lblMinimumAmt.Right + num;
      this.lblMaximumAmt.Left = this.lblMaximumAmtCaption.Right;
      this.lblAssignedAmtTitle.Left = this.lblMaximumAmt.Right + num;
      this.lblAssignedAmt.Left = this.lblAssignedAmtTitle.Right;
      if (this.showPairOffAmt)
      {
        this.lblPairoffAmtCaption.Visible = true;
        this.lblPairoffAmtCaption.Left = this.lblAssignedAmt.Right + num;
        this.lblPairoffAmt.Visible = true;
        this.lblPairoffAmt.Left = this.lblPairoffAmtCaption.Right;
        this.lblOpenAmtCaption.Left = this.lblPairoffAmt.Right + num;
      }
      else
      {
        this.lblPairoffAmtCaption.Visible = false;
        this.lblPairoffAmt.Visible = false;
        this.lblOpenAmtCaption.Left = this.lblAssignedAmt.Right + num;
      }
      this.lblOpenAmt.Left = this.lblOpenAmtCaption.Right;
      this.lblWACCaption.Left = this.lblOpenAmt.Right + num;
      this.lblWAC.Left = this.lblWACCaption.Right;
      this.lblGainLossCaption.Left = this.lblWAC.Right + num;
      this.lblGainLoss.Left = this.lblGainLossCaption.Right;
      this.lblDeliveredAmt.Visible = false;
      this.lblDeliveredAmtTitle.Visible = false;
      this.lblPurchasedAmt.Visible = false;
      this.lblPurchasedAmtTitle.Visible = false;
      this.lblRejectedAmt.Visible = false;
      this.lblRejectedAmtTitle.Visible = false;
      if (trade.TradeType != TradeType.CorrespondentTrade)
        return;
      this.lblDeliveredAmt.Visible = true;
      this.lblDeliveredAmtTitle.Visible = true;
      this.lblPurchasedAmt.Visible = true;
      this.lblPurchasedAmtTitle.Visible = true;
      this.lblRejectedAmt.Visible = true;
      this.lblRejectedAmtTitle.Visible = true;
      this.lblDeliveredAmtTitle.Left = this.lblGainLoss.Right + num;
      this.lblDeliveredAmt.Left = this.lblDeliveredAmtTitle.Right;
      this.lblPurchasedAmtTitle.Left = this.lblDeliveredAmt.Right + num;
      this.lblPurchasedAmt.Left = this.lblPurchasedAmtTitle.Right;
      this.lblRejectedAmtTitle.Left = this.lblPurchasedAmt.Right + num;
      this.lblRejectedAmt.Left = this.lblRejectedAmtTitle.Right;
    }

    public string[] RequiredFields => TradeProfitControl.requiredFields;

    private void lblWACCaption_MouseHover(object sender, EventArgs e)
    {
      Point screen = this.PointToScreen(((Control) sender).Location);
      switch (((Control) sender).Name)
      {
        case "lblWACCaption":
          this.tradeStatsPopup = new TradeStatsPopup(this.wac, this.wam, this.waceil, this.wachange, this.wafloor);
          break;
        case "lblAssignedAmtTitle":
          if (this.trade == null || this.trade.TradeAmount <= 0M)
          {
            this.tradeStatsPopup = new TradeStatsPopup("Assigned Loans", "Assigned %", 0M);
            break;
          }
          this.tradeStatsPopup = new TradeStatsPopup("Assigned Loans", "Assigned %", !(this.trade is CorrespondentTradeInfo) ? (!(this.trade is GSECommitmentInfo) ? (this.assignedAmt + this.pairoffamount) / this.trade.TradeAmount : (this.assignedAmt - this.getDisplayTradeAmount(this.trade)) / this.trade.TradeAmount) : this.assignedAmt / this.trade.TradeAmount);
          break;
        case "lblDeliveredAmtTitle":
          this.tradeStatsPopup = this.trade == null || this.trade.TradeAmount <= 0M ? new TradeStatsPopup("Delivered Loans", "Delivered %", 0M) : new TradeStatsPopup("Delivered Loans", "Delivered %", this.deliveredAmt / this.trade.TradeAmount);
          break;
        case "lblPurchasedAmtTitle":
          this.tradeStatsPopup = this.trade == null || this.trade.TradeAmount <= 0M ? new TradeStatsPopup("Purchased Loans", "Purchased %", 0M) : new TradeStatsPopup("Purchased Loans", "Purchased %", this.purchasedAmt / this.trade.TradeAmount);
          break;
        case "lblRejectedAmtTitle":
          this.tradeStatsPopup = this.trade == null || this.trade.TradeAmount <= 0M ? new TradeStatsPopup("Rejected Loans", "Rejected %", 0M) : new TradeStatsPopup("Rejected Loans", "Rejected %", this.rejectedAmt / this.trade.TradeAmount);
          break;
      }
      this.tradeStatsPopup.Left = Math.Min(screen.X, Screen.GetWorkingArea((Control) sender).Width - this.tradeStatsPopup.Width);
      this.tradeStatsPopup.Top = Math.Min(screen.Y + ((Control) sender).Height + 1, Screen.GetWorkingArea((Control) sender).Height - this.tradeStatsPopup.Height);
      if (this.tradeStatsPopup.Top < screen.Y)
        this.tradeStatsPopup.Top = screen.Y - this.tradeStatsPopup.Height - 1;
      this.tradeStatsPopup.Show();
    }

    private void lblWACCaption_MouseLeave(object sender, EventArgs e)
    {
      try
      {
        if (this.tradeStatsPopup != null)
        {
          this.tradeStatsPopup.Close();
          this.tradeStatsPopup = (TradeStatsPopup) null;
        }
      }
      catch
      {
      }
      ((Control) sender).Font = new Font(((Control) sender).Font, FontStyle.Regular);
    }

    private void lblWACCaption_MouseEnter(object sender, EventArgs e)
    {
      ((Control) sender).Font = new Font(((Control) sender).Font, FontStyle.Underline);
    }

    public Decimal GetAssignedAmount() => Utils.ParseDecimal((object) this.lblAssignedAmt.Text, 0M);

    public Decimal GetOpenAmount() => Utils.ParseDecimal((object) this.lblOpenAmt.Text, 0M);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblCount = new Label();
      this.lblAssignedAmt = new Label();
      this.lblOpenAmt = new Label();
      this.lblWAC = new Label();
      this.lblGainLoss = new Label();
      this.lblGainLossCaption = new Label();
      this.lblWACCaption = new Label();
      this.lblOpenAmtCaption = new Label();
      this.lblCountCaption = new Label();
      this.lblMinimumAmt = new Label();
      this.lblMinimumAmtCaption = new Label();
      this.lblMaximumAmt = new Label();
      this.lblMaximumAmtCaption = new Label();
      this.lblPairoffAmtCaption = new Label();
      this.lblPairoffAmt = new Label();
      this.lblDeliveredAmt = new Label();
      this.lblPurchasedAmt = new Label();
      this.lblRejectedAmt = new Label();
      this.lblAssignedAmtTitle = new Label();
      this.lblDeliveredAmtTitle = new Label();
      this.lblPurchasedAmtTitle = new Label();
      this.lblRejectedAmtTitle = new Label();
      this.SuspendLayout();
      this.lblCount.AutoSize = true;
      this.lblCount.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblCount.Location = new Point(59, 2);
      this.lblCount.Name = "lblCount";
      this.lblCount.Size = new Size(31, 14);
      this.lblCount.TabIndex = 13;
      this.lblCount.Text = "####";
      this.lblAssignedAmt.AutoSize = true;
      this.lblAssignedAmt.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblAssignedAmt.Location = new Point(446, 2);
      this.lblAssignedAmt.Name = "lblAssignedAmt";
      this.lblAssignedAmt.Size = new Size(61, 14);
      this.lblAssignedAmt.TabIndex = 15;
      this.lblAssignedAmt.Text = "#########";
      this.lblOpenAmt.AutoSize = true;
      this.lblOpenAmt.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblOpenAmt.Location = new Point(706, 2);
      this.lblOpenAmt.Name = "lblOpenAmt";
      this.lblOpenAmt.Size = new Size(61, 14);
      this.lblOpenAmt.TabIndex = 17;
      this.lblOpenAmt.Text = "#########";
      this.lblWAC.AutoSize = true;
      this.lblWAC.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblWAC.Location = new Point(805, 2);
      this.lblWAC.Name = "lblWAC";
      this.lblWAC.Size = new Size(37, 14);
      this.lblWAC.TabIndex = 21;
      this.lblWAC.Text = "#####";
      this.lblGainLoss.AutoSize = true;
      this.lblGainLoss.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblGainLoss.Location = new Point(938, 2);
      this.lblGainLoss.Name = "lblGainLoss";
      this.lblGainLoss.Size = new Size(55, 14);
      this.lblGainLoss.TabIndex = 23;
      this.lblGainLoss.Text = "########";
      this.lblGainLossCaption.AutoSize = true;
      this.lblGainLossCaption.Location = new Point(848, 2);
      this.lblGainLossCaption.Name = "lblGainLossCaption";
      this.lblGainLossCaption.Size = new Size(84, 14);
      this.lblGainLossCaption.TabIndex = 22;
      this.lblGainLossCaption.Text = "Total Gain/Loss:";
      this.lblWACCaption.AutoSize = true;
      this.lblWACCaption.Cursor = Cursors.Hand;
      this.lblWACCaption.ForeColor = Color.FromArgb(29, 110, 174);
      this.lblWACCaption.Location = new Point(773, 2);
      this.lblWACCaption.Name = "lblWACCaption";
      this.lblWACCaption.Size = new Size(35, 14);
      this.lblWACCaption.TabIndex = 20;
      this.lblWACCaption.Text = "WAC:";
      this.lblWACCaption.MouseEnter += new EventHandler(this.lblWACCaption_MouseEnter);
      this.lblWACCaption.MouseLeave += new EventHandler(this.lblWACCaption_MouseLeave);
      this.lblWACCaption.MouseHover += new EventHandler(this.lblWACCaption_MouseHover);
      this.lblOpenAmtCaption.AutoSize = true;
      this.lblOpenAmtCaption.Location = new Point(653, 2);
      this.lblOpenAmtCaption.Name = "lblOpenAmtCaption";
      this.lblOpenAmtCaption.Size = new Size(57, 14);
      this.lblOpenAmtCaption.TabIndex = 16;
      this.lblOpenAmtCaption.Text = "Open Amt:";
      this.lblCountCaption.AutoSize = true;
      this.lblCountCaption.Location = new Point(0, 2);
      this.lblCountCaption.Name = "lblCountCaption";
      this.lblCountCaption.Size = new Size(62, 14);
      this.lblCountCaption.TabIndex = 12;
      this.lblCountCaption.Text = "# of Loans:";
      this.lblMinimumAmt.AutoSize = true;
      this.lblMinimumAmt.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblMinimumAmt.Location = new Point(163, 2);
      this.lblMinimumAmt.Name = "lblMinimumAmt";
      this.lblMinimumAmt.Size = new Size(61, 14);
      this.lblMinimumAmt.TabIndex = 25;
      this.lblMinimumAmt.Text = "#########";
      this.lblMinimumAmtCaption.AutoSize = true;
      this.lblMinimumAmtCaption.Location = new Point(94, 2);
      this.lblMinimumAmtCaption.Name = "lblMinimumAmtCaption";
      this.lblMinimumAmtCaption.Size = new Size(71, 14);
      this.lblMinimumAmtCaption.TabIndex = 24;
      this.lblMinimumAmtCaption.Text = "Minimum Amt:";
      this.lblMaximumAmt.AutoSize = true;
      this.lblMaximumAmt.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblMaximumAmt.Location = new Point(303, 2);
      this.lblMaximumAmt.Name = "lblMaximumAmt";
      this.lblMaximumAmt.Size = new Size(61, 14);
      this.lblMaximumAmt.TabIndex = 27;
      this.lblMaximumAmt.Text = "#########";
      this.lblMaximumAmtCaption.AutoSize = true;
      this.lblMaximumAmtCaption.Location = new Point(231, 2);
      this.lblMaximumAmtCaption.Name = "lblMaximumAmtCaption";
      this.lblMaximumAmtCaption.Size = new Size(75, 14);
      this.lblMaximumAmtCaption.TabIndex = 26;
      this.lblMaximumAmtCaption.Text = "Maximum Amt:";
      this.lblPairoffAmtCaption.AutoSize = true;
      this.lblPairoffAmtCaption.Location = new Point(513, 2);
      this.lblPairoffAmtCaption.Name = "lblPairoffAmtCaption";
      this.lblPairoffAmtCaption.Size = new Size(67, 14);
      this.lblPairoffAmtCaption.TabIndex = 28;
      this.lblPairoffAmtCaption.Text = "Pair-off Amt:";
      this.lblPairoffAmt.AutoSize = true;
      this.lblPairoffAmt.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblPairoffAmt.Location = new Point(586, 2);
      this.lblPairoffAmt.Name = "lblPairoffAmt";
      this.lblPairoffAmt.Size = new Size(61, 14);
      this.lblPairoffAmt.TabIndex = 29;
      this.lblPairoffAmt.Text = "#########";
      this.lblDeliveredAmt.AutoSize = true;
      this.lblDeliveredAmt.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblDeliveredAmt.Location = new Point(1081, 2);
      this.lblDeliveredAmt.Name = "lblDeliveredAmt";
      this.lblDeliveredAmt.Size = new Size(61, 14);
      this.lblDeliveredAmt.TabIndex = 31;
      this.lblDeliveredAmt.Text = "#########";
      this.lblPurchasedAmt.AutoSize = true;
      this.lblPurchasedAmt.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblPurchasedAmt.Location = new Point(1228, 2);
      this.lblPurchasedAmt.Name = "lblPurchasedAmt";
      this.lblPurchasedAmt.Size = new Size(61, 14);
      this.lblPurchasedAmt.TabIndex = 35;
      this.lblPurchasedAmt.Text = "#########";
      this.lblRejectedAmt.AutoSize = true;
      this.lblRejectedAmt.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblRejectedAmt.Location = new Point(1374, 2);
      this.lblRejectedAmt.Name = "lblRejectedAmt";
      this.lblRejectedAmt.Size = new Size(61, 14);
      this.lblRejectedAmt.TabIndex = 39;
      this.lblRejectedAmt.Text = "#########";
      this.lblAssignedAmtTitle.AutoSize = true;
      this.lblAssignedAmtTitle.Cursor = Cursors.Hand;
      this.lblAssignedAmtTitle.ForeColor = Color.FromArgb(29, 110, 174);
      this.lblAssignedAmtTitle.Location = new Point(370, 2);
      this.lblAssignedAmtTitle.Name = "lblAssignedAmtTitle";
      this.lblAssignedAmtTitle.Size = new Size(77, 14);
      this.lblAssignedAmtTitle.TabIndex = 42;
      this.lblAssignedAmtTitle.Text = "Assigned Amt:";
      this.lblAssignedAmtTitle.MouseEnter += new EventHandler(this.lblWACCaption_MouseEnter);
      this.lblAssignedAmtTitle.MouseLeave += new EventHandler(this.lblWACCaption_MouseLeave);
      this.lblAssignedAmtTitle.MouseHover += new EventHandler(this.lblWACCaption_MouseHover);
      this.lblDeliveredAmtTitle.AutoSize = true;
      this.lblDeliveredAmtTitle.Cursor = Cursors.Hand;
      this.lblDeliveredAmtTitle.ForeColor = Color.FromArgb(29, 110, 174);
      this.lblDeliveredAmtTitle.Location = new Point(999, 2);
      this.lblDeliveredAmtTitle.Name = "lblDeliveredAmtTitle";
      this.lblDeliveredAmtTitle.Size = new Size(76, 14);
      this.lblDeliveredAmtTitle.TabIndex = 43;
      this.lblDeliveredAmtTitle.Text = "Delivered Amt:";
      this.lblDeliveredAmtTitle.MouseEnter += new EventHandler(this.lblWACCaption_MouseEnter);
      this.lblDeliveredAmtTitle.MouseLeave += new EventHandler(this.lblWACCaption_MouseLeave);
      this.lblDeliveredAmtTitle.MouseHover += new EventHandler(this.lblWACCaption_MouseHover);
      this.lblPurchasedAmtTitle.AutoSize = true;
      this.lblPurchasedAmtTitle.Cursor = Cursors.Hand;
      this.lblPurchasedAmtTitle.ForeColor = Color.FromArgb(29, 110, 174);
      this.lblPurchasedAmtTitle.Location = new Point(1148, 2);
      this.lblPurchasedAmtTitle.Name = "lblPurchasedAmtTitle";
      this.lblPurchasedAmtTitle.Size = new Size(83, 14);
      this.lblPurchasedAmtTitle.TabIndex = 44;
      this.lblPurchasedAmtTitle.Text = "Purchased Amt:";
      this.lblPurchasedAmtTitle.MouseEnter += new EventHandler(this.lblWACCaption_MouseEnter);
      this.lblPurchasedAmtTitle.MouseLeave += new EventHandler(this.lblWACCaption_MouseLeave);
      this.lblPurchasedAmtTitle.MouseHover += new EventHandler(this.lblWACCaption_MouseHover);
      this.lblRejectedAmtTitle.AutoSize = true;
      this.lblRejectedAmtTitle.Cursor = Cursors.Hand;
      this.lblRejectedAmtTitle.ForeColor = Color.FromArgb(29, 110, 174);
      this.lblRejectedAmtTitle.Location = new Point(1295, 2);
      this.lblRejectedAmtTitle.Name = "lblRejectedAmtTitle";
      this.lblRejectedAmtTitle.Size = new Size(73, 14);
      this.lblRejectedAmtTitle.TabIndex = 45;
      this.lblRejectedAmtTitle.Text = "Rejected Amt:";
      this.lblRejectedAmtTitle.MouseEnter += new EventHandler(this.lblWACCaption_MouseEnter);
      this.lblRejectedAmtTitle.MouseLeave += new EventHandler(this.lblWACCaption_MouseLeave);
      this.lblRejectedAmtTitle.MouseHover += new EventHandler(this.lblWACCaption_MouseHover);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.lblRejectedAmtTitle);
      this.Controls.Add((Control) this.lblPurchasedAmtTitle);
      this.Controls.Add((Control) this.lblDeliveredAmtTitle);
      this.Controls.Add((Control) this.lblAssignedAmtTitle);
      this.Controls.Add((Control) this.lblRejectedAmt);
      this.Controls.Add((Control) this.lblPurchasedAmt);
      this.Controls.Add((Control) this.lblDeliveredAmt);
      this.Controls.Add((Control) this.lblPairoffAmt);
      this.Controls.Add((Control) this.lblPairoffAmtCaption);
      this.Controls.Add((Control) this.lblMaximumAmt);
      this.Controls.Add((Control) this.lblMaximumAmtCaption);
      this.Controls.Add((Control) this.lblMinimumAmt);
      this.Controls.Add((Control) this.lblMinimumAmtCaption);
      this.Controls.Add((Control) this.lblCount);
      this.Controls.Add((Control) this.lblAssignedAmt);
      this.Controls.Add((Control) this.lblOpenAmt);
      this.Controls.Add((Control) this.lblWAC);
      this.Controls.Add((Control) this.lblGainLoss);
      this.Controls.Add((Control) this.lblGainLossCaption);
      this.Controls.Add((Control) this.lblWACCaption);
      this.Controls.Add((Control) this.lblOpenAmtCaption);
      this.Controls.Add((Control) this.lblCountCaption);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (TradeProfitControl);
      this.Size = new Size(1450, 18);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

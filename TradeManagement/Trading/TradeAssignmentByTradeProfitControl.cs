// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeAssignmentByTradeProfitControl
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeAssignmentByTradeProfitControl : UserControl
  {
    private bool isShowOpenAmount = true;
    private bool isShowAllocatedPoolAmount = true;
    private IContainer components;
    private Label lblOpenAmt;
    private Label lblAllocatedPoolAmt;
    private Label lblAllocatedPoolAmtCaption;
    public Label lblOpenAmtCaption;

    public TradeAssignmentByTradeProfitControl()
    {
      this.InitializeComponent();
      this.ShowOpenAmount();
      this.ShowAllocatedPoolAmount();
    }

    public Decimal AllocatedPoolAmount
    {
      get => Utils.ParseDecimal((object) this.lblAllocatedPoolAmt.Text, 0M);
    }

    public Decimal OpenAmount => Utils.ParseDecimal((object) this.lblOpenAmt.Text, 0M);

    public void HideOpenAmount()
    {
      this.isShowOpenAmount = false;
      this.layoutControls();
    }

    public void ShowOpenAmount()
    {
      this.isShowOpenAmount = true;
      this.layoutControls();
    }

    public void HideAllocatedPoolAmount()
    {
      this.isShowAllocatedPoolAmount = false;
      this.layoutControls();
    }

    public void ShowAllocatedPoolAmount()
    {
      this.isShowAllocatedPoolAmount = true;
      this.layoutControls();
    }

    public bool ValidateAndCalculate(
      TradeInfoObj trade,
      TradeAssignmentByTradeBase[] assignments,
      bool isShowWarning = true)
    {
      bool flag = true;
      if (trade == null)
        return flag;
      Decimal num1 = 0M;
      Decimal num2 = 0M;
      if (assignments != null)
      {
        switch (trade)
        {
          case MbsPoolInfo _:
          case GSECommitmentInfo _:
            if (assignments.Length != 0 && assignments[0] is GSECommitmentAssignment && trade is MbsPoolInfo)
            {
              GSECommitmentAssignment[] assignments1 = new GSECommitmentAssignment[assignments.Length];
              for (int index = 0; index < assignments.Length; ++index)
                assignments1[index] = (GSECommitmentAssignment) assignments[index];
              num2 = GSECommitmentCalculation.CalculateAllocatedAmount(assignments1);
              num1 = GSECommitmentCalculation.CalculateOpenAmount(trade.TradeAmount, assignments1);
              break;
            }
            num2 = MbsPoolCalculation.CalculateAllocatedAmount(MbsPoolAssignment.Convert(assignments));
            num1 = MbsPoolCalculation.CalculateOpenAmount(trade.TradeAmount, MbsPoolAssignment.Convert(assignments));
            break;
          case SecurityTradeInfo _:
            num2 = SecurityTradeCalculation.CalculateAllocatedAmount(MbsPoolAssignment.Convert(assignments));
            num1 = SecurityTradeCalculation.CalculateOpenAmountFromPoolAssignment(trade.TradeAmount, MbsPoolAssignment.Convert(assignments));
            break;
          default:
            throw new NotSupportedException("Unsupported trade type.");
        }
      }
      this.lblOpenAmt.Text = "$" + num1.ToString("#,##0");
      this.lblAllocatedPoolAmt.Text = "$" + num2.ToString("#,##0");
      if (num1 < 0M)
      {
        flag = false;
        if (isShowWarning)
        {
          switch (trade)
          {
            case MbsPoolInfo _:
              if (assignments.Length != 0 && assignments[0] is GSECommitmentAssignment)
              {
                int num3 = (int) Utils.Dialog((IWin32Window) this, "The allocated commitment amount cannot be greater than the Fannie Mae PE MBS Pool Amount.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                break;
              }
              int num4 = (int) Utils.Dialog((IWin32Window) this, "The Allocated Pool Amount cannot be greater than the MBS Pool Amount.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              break;
            case SecurityTradeInfo _:
              int num5 = (int) Utils.Dialog((IWin32Window) this, "The Allocated Pool Amount cannot be greater than the Trade Amount.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              break;
            case GSECommitmentInfo _:
              int num6 = (int) Utils.Dialog((IWin32Window) this, "The allocated pool amount cannot be greater than the GSE Commitment Amount.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              break;
          }
        }
      }
      this.layoutControls();
      return flag;
    }

    private void layoutControls()
    {
      int num = 10;
      this.lblOpenAmtCaption.Visible = true;
      this.lblOpenAmt.Visible = true;
      this.lblAllocatedPoolAmtCaption.Visible = true;
      this.lblAllocatedPoolAmt.Visible = true;
      if (this.isShowOpenAmount)
        this.lblOpenAmtCaption.Left = 0;
      else
        this.lblOpenAmtCaption.Visible = false;
      if (this.isShowOpenAmount)
        this.lblOpenAmt.Left = this.lblOpenAmtCaption.Right;
      else
        this.lblOpenAmt.Visible = false;
      if (this.isShowAllocatedPoolAmount)
      {
        if (this.isShowOpenAmount)
          this.lblAllocatedPoolAmtCaption.Left = this.lblOpenAmt.Right + num;
        else
          this.lblAllocatedPoolAmtCaption.Left = 0;
      }
      else
        this.lblAllocatedPoolAmtCaption.Visible = false;
      if (this.isShowAllocatedPoolAmount)
        this.lblAllocatedPoolAmt.Left = this.lblAllocatedPoolAmtCaption.Right;
      else
        this.lblAllocatedPoolAmt.Visible = false;
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
      this.lblAllocatedPoolAmt = new Label();
      this.lblAllocatedPoolAmtCaption = new Label();
      this.lblOpenAmtCaption = new Label();
      this.SuspendLayout();
      this.lblOpenAmt.AutoSize = true;
      this.lblOpenAmt.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblOpenAmt.Location = new Point(108, 3);
      this.lblOpenAmt.Name = "lblOpenAmt";
      this.lblOpenAmt.Size = new Size(61, 14);
      this.lblOpenAmt.TabIndex = 25;
      this.lblOpenAmt.Text = "#########";
      this.lblAllocatedPoolAmt.AutoSize = true;
      this.lblAllocatedPoolAmt.Font = new Font("Arial", 8.25f, FontStyle.Bold);
      this.lblAllocatedPoolAmt.Location = new Point(341, 3);
      this.lblAllocatedPoolAmt.Name = "lblAllocatedPoolAmt";
      this.lblAllocatedPoolAmt.Size = new Size(61, 14);
      this.lblAllocatedPoolAmt.TabIndex = 27;
      this.lblAllocatedPoolAmt.Text = "#########";
      this.lblAllocatedPoolAmtCaption.AutoSize = true;
      this.lblAllocatedPoolAmtCaption.Location = new Point(219, 3);
      this.lblAllocatedPoolAmtCaption.Name = "lblAllocatedPoolAmtCaption";
      this.lblAllocatedPoolAmtCaption.Size = new Size(117, 14);
      this.lblAllocatedPoolAmtCaption.TabIndex = 26;
      this.lblAllocatedPoolAmtCaption.Text = "Allocated Pool Amount:";
      this.lblOpenAmtCaption.AutoSize = true;
      this.lblOpenAmtCaption.Location = new Point(5, 3);
      this.lblOpenAmtCaption.Name = "lblOpenAmtCaption";
      this.lblOpenAmtCaption.Size = new Size(98, 14);
      this.lblOpenAmtCaption.TabIndex = 24;
      this.lblOpenAmtCaption.Text = "Open TBA Amount:";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.lblOpenAmt);
      this.Controls.Add((Control) this.lblAllocatedPoolAmt);
      this.Controls.Add((Control) this.lblAllocatedPoolAmtCaption);
      this.Controls.Add((Control) this.lblOpenAmtCaption);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (TradeAssignmentByTradeProfitControl);
      this.Size = new Size(700, 19);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

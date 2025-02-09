// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TradeSummaryDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Trading;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class TradeSummaryDialog : Form
  {
    private IContainer components;
    private GroupBox grpTradeInfo;
    private TextBox txtTotalPairOffAmt;
    private Label label8;
    private TextBox txtTolerance;
    private Label label7;
    private TextBox txtAmount;
    private Label label6;
    private TextBox txtInvestorCommitmentNum;
    private Label label5;
    private TextBox txtInvestorTradeNum;
    private Label label4;
    private TextBox txtInvestor;
    private Label label3;
    private TextBox txtMasterContractNumber;
    private Label label2;
    private TextBox txtName;
    private Label label1;
    private GroupBox grpDates;
    private Label label11;
    private Label label10;
    private Label label9;
    private TextBox txtPurchaseDate;
    private Label label13;
    private TextBox txtActualDeliveryDate;
    private Label label14;
    private TextBox txtTargetDeliveryDate;
    private Label label15;
    private TextBox txtEarlyDeliveryDate;
    private Label label16;
    private TextBox txtInvestorDeliveryDate;
    private Label label17;
    private TextBox txtCommitmentDate;
    private Label label18;
    private Panel pnlHistory;
    private Label label19;
    private ListView lvwHistory;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private ColumnHeader columnHeader4;
    private Button btnClose;
    private ColumnHeader columnHeader5;

    public TradeSummaryDialog(string loanGuid)
    {
      this.InitializeComponent();
      this.loadTradeData(loanGuid);
      this.loadHistory(loanGuid);
    }

    private void loadTradeData(string loanGuid)
    {
      LoanTradeViewModel tradeViewForLoan = Session.LoanTradeManager.GetTradeViewForLoan(loanGuid);
      MbsPoolViewModel mbsPoolViewModel = (MbsPoolViewModel) null;
      if (tradeViewForLoan == null)
        mbsPoolViewModel = Session.MbsPoolManager.GetTradeViewForLoan(loanGuid);
      if (tradeViewForLoan == null && mbsPoolViewModel == null)
        return;
      if (tradeViewForLoan != null)
      {
        this.txtName.Text = tradeViewForLoan.Name;
        this.txtMasterContractNumber.Text = tradeViewForLoan.ContractNumber;
        this.txtInvestor.Text = tradeViewForLoan.InvestorName;
        this.txtInvestorTradeNum.Text = tradeViewForLoan.InvestorTradeNumber;
        this.txtInvestorCommitmentNum.Text = tradeViewForLoan.InvestorCommitmentNumber;
        this.txtAmount.Text = tradeViewForLoan.TradeAmount.ToString("#,###");
        TextBox txtTolerance = this.txtTolerance;
        Decimal num;
        string str1;
        if (!(tradeViewForLoan.Tolerance == 0M))
        {
          num = tradeViewForLoan.Tolerance;
          str1 = num.ToString();
        }
        else
          str1 = "";
        txtTolerance.Text = str1;
        TextBox txtTotalPairOffAmt = this.txtTotalPairOffAmt;
        string str2;
        if (!(tradeViewForLoan.PairOffAmount == 0M))
        {
          num = tradeViewForLoan.PairOffAmount;
          str2 = num.ToString("#,##0.00");
        }
        else
          str2 = "";
        txtTotalPairOffAmt.Text = str2;
        this.txtCommitmentDate.Text = this.formatDate(tradeViewForLoan.CommitmentDate);
        this.txtInvestorDeliveryDate.Text = this.formatDate(tradeViewForLoan.InvestorDeliveryDate);
        this.txtEarlyDeliveryDate.Text = this.formatDate(tradeViewForLoan.EarlyDeliveryDate);
        this.txtTargetDeliveryDate.Text = this.formatDate(tradeViewForLoan.TargetDeliveryDate);
        this.txtActualDeliveryDate.Text = this.formatDate(tradeViewForLoan.ShipmentDate);
        this.txtPurchaseDate.Text = this.formatDate(tradeViewForLoan.PurchaseDate);
      }
      else
      {
        if (mbsPoolViewModel == null)
          return;
        this.txtName.Text = mbsPoolViewModel.Name;
        this.txtMasterContractNumber.Text = mbsPoolViewModel.ContractNumber;
        this.txtInvestor.Text = mbsPoolViewModel.InvestorName;
        this.txtAmount.Text = mbsPoolViewModel.TradeAmount.ToString("#,###");
        this.txtCommitmentDate.Text = this.formatDate(mbsPoolViewModel.CommitmentDate);
        this.txtInvestorDeliveryDate.Text = this.formatDate(mbsPoolViewModel.InvestorDeliveryDate);
        this.txtEarlyDeliveryDate.Text = this.formatDate(mbsPoolViewModel.EarlyDeliveryDate);
        this.txtTargetDeliveryDate.Text = this.formatDate(mbsPoolViewModel.TargetDeliveryDate);
        this.txtActualDeliveryDate.Text = this.formatDate(mbsPoolViewModel.ShipmentDate);
        this.txtPurchaseDate.Text = this.formatDate(mbsPoolViewModel.PurchaseDate);
      }
    }

    private void loadHistory(string loanGuid)
    {
      ITradeHistoryItem[] tradeHistoryForLoan;
      ITradeHistoryItem[] tradeHistoryItemArray = tradeHistoryForLoan = (ITradeHistoryItem[]) Session.LoanTradeManager.GetTradeHistoryForLoan(loanGuid);
      if (tradeHistoryItemArray == null || tradeHistoryItemArray.Length == 0)
        tradeHistoryItemArray = (ITradeHistoryItem[]) Session.MbsPoolManager.GetTradeHistoryForLoan(loanGuid);
      if (tradeHistoryItemArray == null || tradeHistoryItemArray.Length == 0)
        return;
      foreach (ITradeHistoryItem historyItem in tradeHistoryItemArray)
        this.lvwHistory.Items.Add(this.createHistoryListViewItem(historyItem));
    }

    private ListViewItem createHistoryListViewItem(ITradeHistoryItem historyItem)
    {
      return new ListViewItem()
      {
        Text = historyItem.Timestamp.ToString("MM/dd/yyyy h:mm tt"),
        SubItems = {
          historyItem.StatusDescription,
          historyItem.TradeName,
          historyItem.InvestorName,
          historyItem.Comment
        }
      };
    }

    private string formatDate(DateTime date)
    {
      return date == DateTime.MinValue ? "" : date.ToString("MM/dd/yyyy");
    }

    private void TradeSummaryDialog_Resize(object sender, EventArgs e)
    {
      int num = 10;
      this.grpTradeInfo.Height = Math.Max((this.pnlHistory.Height - num) / 2, this.grpTradeInfo.MinimumSize.Height);
      this.grpDates.Height = Math.Max(this.pnlHistory.Height - this.grpTradeInfo.Height - num, this.grpDates.MinimumSize.Height);
      this.grpDates.Top = this.grpTradeInfo.Bottom + num;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.grpTradeInfo = new GroupBox();
      this.txtInvestorCommitmentNum = new TextBox();
      this.txtTotalPairOffAmt = new TextBox();
      this.txtTolerance = new TextBox();
      this.label11 = new Label();
      this.label10 = new Label();
      this.txtAmount = new TextBox();
      this.label9 = new Label();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label6 = new Label();
      this.label5 = new Label();
      this.txtInvestorTradeNum = new TextBox();
      this.label4 = new Label();
      this.txtInvestor = new TextBox();
      this.label3 = new Label();
      this.txtMasterContractNumber = new TextBox();
      this.label2 = new Label();
      this.txtName = new TextBox();
      this.label1 = new Label();
      this.grpDates = new GroupBox();
      this.txtPurchaseDate = new TextBox();
      this.label13 = new Label();
      this.txtActualDeliveryDate = new TextBox();
      this.label14 = new Label();
      this.txtTargetDeliveryDate = new TextBox();
      this.label15 = new Label();
      this.txtEarlyDeliveryDate = new TextBox();
      this.label16 = new Label();
      this.txtInvestorDeliveryDate = new TextBox();
      this.label17 = new Label();
      this.txtCommitmentDate = new TextBox();
      this.label18 = new Label();
      this.pnlHistory = new Panel();
      this.lvwHistory = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.columnHeader4 = new ColumnHeader();
      this.label19 = new Label();
      this.btnClose = new Button();
      this.columnHeader5 = new ColumnHeader();
      this.grpTradeInfo.SuspendLayout();
      this.grpDates.SuspendLayout();
      this.pnlHistory.SuspendLayout();
      this.SuspendLayout();
      this.grpTradeInfo.Controls.Add((Control) this.txtInvestorCommitmentNum);
      this.grpTradeInfo.Controls.Add((Control) this.txtTotalPairOffAmt);
      this.grpTradeInfo.Controls.Add((Control) this.txtTolerance);
      this.grpTradeInfo.Controls.Add((Control) this.label11);
      this.grpTradeInfo.Controls.Add((Control) this.label10);
      this.grpTradeInfo.Controls.Add((Control) this.txtAmount);
      this.grpTradeInfo.Controls.Add((Control) this.label9);
      this.grpTradeInfo.Controls.Add((Control) this.label8);
      this.grpTradeInfo.Controls.Add((Control) this.label7);
      this.grpTradeInfo.Controls.Add((Control) this.label6);
      this.grpTradeInfo.Controls.Add((Control) this.label5);
      this.grpTradeInfo.Controls.Add((Control) this.txtInvestorTradeNum);
      this.grpTradeInfo.Controls.Add((Control) this.label4);
      this.grpTradeInfo.Controls.Add((Control) this.txtInvestor);
      this.grpTradeInfo.Controls.Add((Control) this.label3);
      this.grpTradeInfo.Controls.Add((Control) this.txtMasterContractNumber);
      this.grpTradeInfo.Controls.Add((Control) this.label2);
      this.grpTradeInfo.Controls.Add((Control) this.txtName);
      this.grpTradeInfo.Controls.Add((Control) this.label1);
      this.grpTradeInfo.Location = new Point(12, 10);
      this.grpTradeInfo.MinimumSize = new Size(274, 220);
      this.grpTradeInfo.Name = "grpTradeInfo";
      this.grpTradeInfo.Size = new Size(274, 220);
      this.grpTradeInfo.TabIndex = 0;
      this.grpTradeInfo.TabStop = false;
      this.grpTradeInfo.Text = "Trade Info";
      this.txtInvestorCommitmentNum.Location = new Point(133, 114);
      this.txtInvestorCommitmentNum.Name = "txtInvestorCommitmentNum";
      this.txtInvestorCommitmentNum.ReadOnly = true;
      this.txtInvestorCommitmentNum.Size = new Size(128, 20);
      this.txtInvestorCommitmentNum.TabIndex = 9;
      this.txtInvestorCommitmentNum.TabStop = false;
      this.txtTotalPairOffAmt.Location = new Point(133, 183);
      this.txtTotalPairOffAmt.Name = "txtTotalPairOffAmt";
      this.txtTotalPairOffAmt.ReadOnly = true;
      this.txtTotalPairOffAmt.Size = new Size(128, 20);
      this.txtTotalPairOffAmt.TabIndex = 15;
      this.txtTotalPairOffAmt.TabStop = false;
      this.txtTotalPairOffAmt.TextAlign = HorizontalAlignment.Right;
      this.txtTolerance.Location = new Point(133, 160);
      this.txtTolerance.Name = "txtTolerance";
      this.txtTolerance.ReadOnly = true;
      this.txtTolerance.Size = new Size(67, 20);
      this.txtTolerance.TabIndex = 13;
      this.txtTolerance.TabStop = false;
      this.txtTolerance.TextAlign = HorizontalAlignment.Right;
      this.label11.AutoSize = true;
      this.label11.Location = new Point(119, 187);
      this.label11.Name = "label11";
      this.label11.Size = new Size(15, 13);
      this.label11.TabIndex = 18;
      this.label11.Text = "$";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(200, 164);
      this.label10.Name = "label10";
      this.label10.Size = new Size(15, 13);
      this.label10.TabIndex = 17;
      this.label10.Text = "%";
      this.txtAmount.Location = new Point(133, 137);
      this.txtAmount.Name = "txtAmount";
      this.txtAmount.ReadOnly = true;
      this.txtAmount.Size = new Size(128, 20);
      this.txtAmount.TabIndex = 11;
      this.txtAmount.TabStop = false;
      this.txtAmount.TextAlign = HorizontalAlignment.Right;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(119, 140);
      this.label9.Name = "label9";
      this.label9.Size = new Size(13, 13);
      this.label9.TabIndex = 16;
      this.label9.Text = "$";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(8, 187);
      this.label8.Name = "label8";
      this.label8.Size = new Size(66, 13);
      this.label8.TabIndex = 14;
      this.label8.Text = "Total Pair Off Amt:";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(8, 164);
      this.label7.Name = "label7";
      this.label7.Size = new Size(58, 13);
      this.label7.TabIndex = 12;
      this.label7.Text = "Tolerance:";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(8, 141);
      this.label6.Name = "label6";
      this.label6.Size = new Size(77, 13);
      this.label6.TabIndex = 10;
      this.label6.Text = "Trade Amount:";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(8, 118);
      this.label5.Name = "label5";
      this.label5.Size = new Size(118, 13);
      this.label5.TabIndex = 8;
      this.label5.Text = "Investor Commitment #:";
      this.txtInvestorTradeNum.Location = new Point(133, 91);
      this.txtInvestorTradeNum.Name = "txtInvestorTradeNum";
      this.txtInvestorTradeNum.ReadOnly = true;
      this.txtInvestorTradeNum.Size = new Size(128, 20);
      this.txtInvestorTradeNum.TabIndex = 7;
      this.txtInvestorTradeNum.TabStop = false;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(8, 95);
      this.label4.Name = "label4";
      this.label4.Size = new Size(89, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Investor Trade #:";
      this.txtInvestor.Location = new Point(133, 68);
      this.txtInvestor.Name = "txtInvestor";
      this.txtInvestor.ReadOnly = true;
      this.txtInvestor.Size = new Size(128, 20);
      this.txtInvestor.TabIndex = 5;
      this.txtInvestor.TabStop = false;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(8, 72);
      this.label3.Name = "label3";
      this.label3.Size = new Size(48, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Investor:";
      this.txtMasterContractNumber.Location = new Point(133, 45);
      this.txtMasterContractNumber.Name = "txtMasterContractNumber";
      this.txtMasterContractNumber.ReadOnly = true;
      this.txtMasterContractNumber.Size = new Size(128, 20);
      this.txtMasterContractNumber.TabIndex = 3;
      this.txtMasterContractNumber.TabStop = false;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 49);
      this.label2.Name = "label2";
      this.label2.Size = new Size(95, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Master Contract #:";
      this.txtName.Location = new Point(133, 22);
      this.txtName.Name = "txtName";
      this.txtName.ReadOnly = true;
      this.txtName.Size = new Size(128, 20);
      this.txtName.TabIndex = 1;
      this.txtName.TabStop = false;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 26);
      this.label1.Name = "label1";
      this.label1.Size = new Size(111, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Trade Name/Number:";
      this.grpDates.Controls.Add((Control) this.txtPurchaseDate);
      this.grpDates.Controls.Add((Control) this.label13);
      this.grpDates.Controls.Add((Control) this.txtActualDeliveryDate);
      this.grpDates.Controls.Add((Control) this.label14);
      this.grpDates.Controls.Add((Control) this.txtTargetDeliveryDate);
      this.grpDates.Controls.Add((Control) this.label15);
      this.grpDates.Controls.Add((Control) this.txtEarlyDeliveryDate);
      this.grpDates.Controls.Add((Control) this.label16);
      this.grpDates.Controls.Add((Control) this.txtInvestorDeliveryDate);
      this.grpDates.Controls.Add((Control) this.label17);
      this.grpDates.Controls.Add((Control) this.txtCommitmentDate);
      this.grpDates.Controls.Add((Control) this.label18);
      this.grpDates.Location = new Point(12, 237);
      this.grpDates.MinimumSize = new Size(274, 174);
      this.grpDates.Name = "grpDates";
      this.grpDates.Size = new Size(274, 174);
      this.grpDates.TabIndex = 1;
      this.grpDates.TabStop = false;
      this.grpDates.Text = "Trade Dates";
      this.txtPurchaseDate.Location = new Point(133, 137);
      this.txtPurchaseDate.Name = "txtPurchaseDate";
      this.txtPurchaseDate.ReadOnly = true;
      this.txtPurchaseDate.Size = new Size(128, 20);
      this.txtPurchaseDate.TabIndex = 28;
      this.txtPurchaseDate.TabStop = false;
      this.label13.AutoSize = true;
      this.label13.Location = new Point(8, 141);
      this.label13.Name = "label13";
      this.label13.Size = new Size(81, 13);
      this.label13.TabIndex = 27;
      this.label13.Text = "Purchase Date:";
      this.txtActualDeliveryDate.Location = new Point(133, 114);
      this.txtActualDeliveryDate.Name = "txtActualDeliveryDate";
      this.txtActualDeliveryDate.ReadOnly = true;
      this.txtActualDeliveryDate.Size = new Size(128, 20);
      this.txtActualDeliveryDate.TabIndex = 26;
      this.txtActualDeliveryDate.TabStop = false;
      this.label14.AutoSize = true;
      this.label14.Location = new Point(8, 118);
      this.label14.Name = "label14";
      this.label14.Size = new Size(107, 13);
      this.label14.TabIndex = 25;
      this.label14.Text = "Actual Delivery Date:";
      this.txtTargetDeliveryDate.Location = new Point(133, 91);
      this.txtTargetDeliveryDate.Name = "txtTargetDeliveryDate";
      this.txtTargetDeliveryDate.ReadOnly = true;
      this.txtTargetDeliveryDate.Size = new Size(128, 20);
      this.txtTargetDeliveryDate.TabIndex = 24;
      this.txtTargetDeliveryDate.TabStop = false;
      this.label15.AutoSize = true;
      this.label15.Location = new Point(8, 95);
      this.label15.Name = "label15";
      this.label15.Size = new Size(108, 13);
      this.label15.TabIndex = 23;
      this.label15.Text = "Target Delivery Date:";
      this.txtEarlyDeliveryDate.Location = new Point(133, 68);
      this.txtEarlyDeliveryDate.Name = "txtEarlyDeliveryDate";
      this.txtEarlyDeliveryDate.ReadOnly = true;
      this.txtEarlyDeliveryDate.Size = new Size(128, 20);
      this.txtEarlyDeliveryDate.TabIndex = 22;
      this.txtEarlyDeliveryDate.TabStop = false;
      this.label16.AutoSize = true;
      this.label16.Location = new Point(8, 72);
      this.label16.Name = "label16";
      this.label16.Size = new Size(100, 13);
      this.label16.TabIndex = 21;
      this.label16.Text = "Early Delivery Date:";
      this.txtInvestorDeliveryDate.Location = new Point(133, 45);
      this.txtInvestorDeliveryDate.Name = "txtInvestorDeliveryDate";
      this.txtInvestorDeliveryDate.ReadOnly = true;
      this.txtInvestorDeliveryDate.Size = new Size(128, 20);
      this.txtInvestorDeliveryDate.TabIndex = 20;
      this.txtInvestorDeliveryDate.TabStop = false;
      this.label17.AutoSize = true;
      this.label17.Location = new Point(8, 49);
      this.label17.Name = "label17";
      this.label17.Size = new Size(115, 13);
      this.label17.TabIndex = 19;
      this.label17.Text = "Investor Delivery Date:";
      this.txtCommitmentDate.Location = new Point(133, 22);
      this.txtCommitmentDate.Name = "txtCommitmentDate";
      this.txtCommitmentDate.ReadOnly = true;
      this.txtCommitmentDate.Size = new Size(128, 20);
      this.txtCommitmentDate.TabIndex = 18;
      this.txtCommitmentDate.TabStop = false;
      this.label18.AutoSize = true;
      this.label18.Location = new Point(8, 26);
      this.label18.Name = "label18";
      this.label18.Size = new Size(93, 13);
      this.label18.TabIndex = 17;
      this.label18.Text = "Commitment Date:";
      this.pnlHistory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlHistory.Controls.Add((Control) this.lvwHistory);
      this.pnlHistory.Controls.Add((Control) this.label19);
      this.pnlHistory.Location = new Point(296, 8);
      this.pnlHistory.Name = "pnlHistory";
      this.pnlHistory.Size = new Size(423, 424);
      this.pnlHistory.TabIndex = 2;
      this.lvwHistory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lvwHistory.Columns.AddRange(new ColumnHeader[5]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3,
        this.columnHeader4,
        this.columnHeader5
      });
      this.lvwHistory.GridLines = true;
      this.lvwHistory.Location = new Point(0, 22);
      this.lvwHistory.MultiSelect = false;
      this.lvwHistory.Name = "lvwHistory";
      this.lvwHistory.Size = new Size(422, 402);
      this.lvwHistory.TabIndex = 2;
      this.lvwHistory.TabStop = false;
      this.lvwHistory.UseCompatibleStateImageBehavior = false;
      this.lvwHistory.View = View.Details;
      this.columnHeader1.Text = "Event Time";
      this.columnHeader1.Width = 96;
      this.columnHeader2.Text = "Event";
      this.columnHeader2.Width = 95;
      this.columnHeader3.Text = "Trade #";
      this.columnHeader3.Width = 84;
      this.columnHeader4.Text = "Investor Name";
      this.columnHeader4.Width = 97;
      this.label19.AutoSize = true;
      this.label19.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label19.Location = new Point(4, 4);
      this.label19.Name = "label19";
      this.label19.Size = new Size(82, 13);
      this.label19.TabIndex = 1;
      this.label19.Text = "Loan History:";
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(644, 442);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 3;
      this.btnClose.Text = "&Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.columnHeader5.Text = "Comments";
      this.columnHeader5.Width = 120;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(734, 478);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.pnlHistory);
      this.Controls.Add((Control) this.grpDates);
      this.Controls.Add((Control) this.grpTradeInfo);
      this.Name = nameof (TradeSummaryDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "View Trade Summary";
      this.Resize += new EventHandler(this.TradeSummaryDialog_Resize);
      this.grpTradeInfo.ResumeLayout(false);
      this.grpTradeInfo.PerformLayout();
      this.grpDates.ResumeLayout(false);
      this.grpDates.PerformLayout();
      this.pnlHistory.ResumeLayout(false);
      this.pnlHistory.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CommonPairOffDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class CommonPairOffDialog : Form
  {
    private CommonPairOff pairOff;
    private bool modified;
    private bool readOnly;
    private bool ignoreEvent;
    private Decimal totalPairOffAmount;
    private Decimal actualTradeAmount;
    private IContainer components;
    private Panel panelRequestedBy;
    private TextBox txtRequestedBy;
    private Label label9;
    private Panel panelFeeDetails;
    private Panel panelDate;
    private DatePicker dtDate;
    private Label label1;
    private Label label5;
    private TextBox txtGainLoss;
    private Label label4;
    private TextBox txtPairOffPercentage;
    private TextBox txtTradeAmount;
    private Label label3;
    private Label label2;
    private Panel panelButtons;
    private Label label8;
    private Label label7;
    private Label label6;
    private Label lblNote;
    private Button btnCancel;
    private Button btnOk;
    private Panel panelComments;
    private TextBox txtComments;
    private Label label10;

    public CommonPairOffDialog(
      CommonPairOff pairOff,
      PairOffType pairOffType,
      bool positiveEntryExists,
      Decimal tradeOpenAmount,
      Decimal actualTradeAmount,
      Decimal totalPairOffAmount)
    {
      this.InitializeComponent();
      this.AutoValidate = AutoValidate.Disable;
      this.PairOffType = pairOffType;
      if (this.PairOffType == PairOffType.CorrespondentTrades)
      {
        this.OpenAmount = tradeOpenAmount;
        this.actualTradeAmount = actualTradeAmount;
      }
      TextBoxFormatter.Attach(this.txtTradeAmount, this.PairOffType == PairOffType.CorrespondentTrades & positiveEntryExists ? TextBoxContentRule.Decimal : TextBoxContentRule.NonNegativeDecimal, "#,##0.00#");
      TextBoxFormatter.Attach(this.txtPairOffPercentage, TextBoxContentRule.Decimal, "#,##0.#####;;\"\"");
      TextBoxFormatter.Attach(this.txtGainLoss, TextBoxContentRule.Decimal, "#,##0.00#");
      if (this.PairOffType != PairOffType.CorrespondentTrades)
      {
        this.panelFeeDetails.Top = this.panelRequestedBy.Top;
        this.panelButtons.Top = this.panelFeeDetails.Top + this.panelFeeDetails.Height;
        this.panelRequestedBy.Visible = false;
        this.panelComments.Visible = false;
        this.Height = 248;
      }
      if (pairOff == null)
      {
        pairOff = new CommonPairOff();
        this.totalPairOffAmount = totalPairOffAmount;
      }
      else
      {
        this.OpenAmount += pairOff.TradeAmount;
        this.totalPairOffAmount = totalPairOffAmount - pairOff.TradeAmount;
      }
      this.pairOff = pairOff;
      this.pairOff.Locked = true;
      this.ClearFields();
      this.LoadPairOffData(this.pairOff, (object) null);
    }

    public CommonPairOff PairOff => this.pairOff;

    public bool DataModified
    {
      get => this.modified && !this.readOnly;
      set => this.modified = value;
    }

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        if (this.readOnly == value)
          return;
        this.readOnly = value;
        this.dtDate.ReadOnly = this.readOnly;
        this.txtTradeAmount.ReadOnly = this.readOnly;
        this.txtPairOffPercentage.ReadOnly = this.readOnly;
        if (this.PairOffType == PairOffType.CorrespondentTrades)
        {
          this.txtRequestedBy.ReadOnly = this.readOnly;
          this.txtComments.ReadOnly = this.readOnly;
        }
        this.btnOk.Enabled = !this.readOnly;
        this.txtGainLoss.ReadOnly = this.readOnly;
        if (this.PairOffType != PairOffType.CorrespondentTrades)
          return;
        this.txtRequestedBy.ReadOnly = this.readOnly;
        this.txtComments.ReadOnly = this.readOnly;
      }
    }

    public Decimal OpenAmount { get; set; }

    public PairOffType PairOffType { get; set; }

    public void CommitChanges()
    {
      this.pairOff.Date = this.dtDate.Value;
      this.pairOff.TradeAmount = Utils.ParseDecimal((object) this.txtTradeAmount.Text);
      this.pairOff.PairOffFeePercentage = !(this.pairOff.TradeAmount == 0M) ? Utils.ParseDecimal((object) this.txtPairOffPercentage.Text) : 0M;
      this.pairOff.RequestedBy = this.txtRequestedBy.Text;
      this.pairOff.Comments = this.txtComments.Text;
      this.modified = false;
    }

    private void ClearFields()
    {
      this.dtDate.Text = "";
      this.txtTradeAmount.Text = "";
      this.txtPairOffPercentage.Text = "";
      this.txtGainLoss.Text = "";
      if (this.PairOffType != PairOffType.CorrespondentTrades)
        return;
      this.txtRequestedBy.Text = "";
      this.txtComments.Text = "";
    }

    private void LoadPairOffData(CommonPairOff pairOffInfo, object sender)
    {
      this.ignoreEvent = true;
      this.dtDate.Value = pairOffInfo.Date;
      if (sender == null || (TextBox) sender != this.txtTradeAmount)
        this.txtTradeAmount.Text = pairOffInfo.TradeAmount.ToString("#,##0.00#");
      if (sender == null || (TextBox) sender != this.txtPairOffPercentage)
        this.txtPairOffPercentage.Text = pairOffInfo.PairOffFeePercentage.ToString("#,##0.#####;;\"\"");
      if (sender == null || (TextBox) sender != this.txtGainLoss)
        this.txtGainLoss.Text = pairOffInfo.CalculatedPairOffFee.ToString("#,##0.00#;;\"\"");
      if (pairOffInfo.TradeAmount == 0M)
        this.txtPairOffPercentage.Enabled = this.txtGainLoss.Enabled = false;
      else
        this.txtPairOffPercentage.Enabled = this.txtGainLoss.Enabled = true;
      if (this.PairOffType == PairOffType.CorrespondentTrades)
      {
        this.txtRequestedBy.Text = pairOffInfo.RequestedBy;
        this.txtComments.Text = pairOffInfo.Comments;
      }
      this.ignoreEvent = false;
      this.modified = false;
    }

    private void OnFieldValueChanged(object sender, EventArgs e)
    {
      if (this.ignoreEvent)
        return;
      this.LoadPairOffData(this.ConstructTempPairOff(), sender);
      this.valueChanged(sender, e);
    }

    private CommonPairOff ConstructTempPairOff()
    {
      CommonPairOff commonPairOff = new CommonPairOff()
      {
        Date = this.dtDate.Value,
        TradeAmount = Utils.ParseDecimal((object) this.txtTradeAmount.Text)
      };
      commonPairOff.PairOffFeePercentage = !(commonPairOff.TradeAmount == 0M) ? Utils.ParseDecimal((object) this.txtPairOffPercentage.Text) : 0M;
      if (this.PairOffType == PairOffType.CorrespondentTrades)
      {
        commonPairOff.RequestedBy = this.txtRequestedBy.Text;
        commonPairOff.Comments = this.txtComments.Text;
      }
      return commonPairOff;
    }

    private void valueChanged(object sender, EventArgs e)
    {
      if (this.ignoreEvent)
        return;
      this.modified = true;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (this.PairOffType == PairOffType.CorrespondentTrades && !this.ValidatePairOff(Utils.ParseDecimal((object) this.txtTradeAmount.Text)))
        return;
      this.DialogResult = DialogResult.OK;
      this.ValidateChildren();
    }

    private void txtGainLoss_TextChanged(object sender, EventArgs e)
    {
      if (this.ignoreEvent)
        return;
      CommonPairOff pairOffInfo = this.ConstructTempPairOff();
      pairOffInfo.ReCalculatePairOffPercentage(Utils.ParseDecimal((object) this.txtGainLoss.Text));
      this.LoadPairOffData(pairOffInfo, (object) this.txtGainLoss);
      this.modified = true;
    }

    public bool ValidatePairOff(Decimal tradeAmount, IWin32Window owner = null, bool validateDelete = false)
    {
      string text = "The trade amount of the pair-off cannot be greater than the open amount.";
      Decimal totalPairOffAmount = this.totalPairOffAmount;
      if (validateDelete)
        totalPairOffAmount += tradeAmount;
      if (this.pairOff.Index == -1 && this.OpenAmount < tradeAmount)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, text);
        return false;
      }
      if (tradeAmount < 0M)
      {
        totalPairOffAmount += tradeAmount;
        if (this.pairOff.Index > -1 && this.actualTradeAmount < totalPairOffAmount)
        {
          int num = (int) Utils.Dialog(owner, text);
          return false;
        }
        if (!validateDelete && totalPairOffAmount < 0M)
        {
          int num = (int) Utils.Dialog(owner, "The Pair-off reversal amount cannot exceed the Total Pair-off amount.");
          return false;
        }
        if (!validateDelete && tradeAmount < this.totalPairOffAmount && tradeAmount < this.totalPairOffAmount && this.OpenAmount < tradeAmount)
        {
          int num = (int) Utils.Dialog(owner, "The Pair-off reversal amount cannot exceed the Total Pair-off amount.");
          return false;
        }
      }
      else
      {
        if (tradeAmount == 0M)
        {
          int num = (int) Utils.Dialog(owner, "The Pair-off amount should be greater than zero.");
          return false;
        }
        if (this.pairOff.Index > -1 && this.actualTradeAmount < tradeAmount)
        {
          int num = (int) Utils.Dialog(owner, text);
          return false;
        }
        if (validateDelete && (totalPairOffAmount < 0M || totalPairOffAmount == 0M || totalPairOffAmount < tradeAmount))
        {
          int num = (int) Utils.Dialog(owner, "The Pair off reversal amount cannot exceed total pair off amount.");
          return false;
        }
        if (this.pairOff.Index > -1 && this.OpenAmount < tradeAmount && totalPairOffAmount != 0M)
        {
          int num = (int) Utils.Dialog(owner, text);
          return false;
        }
        if (!validateDelete && this.pairOff.Index > -1 && Math.Abs(totalPairOffAmount) > tradeAmount && tradeAmount > this.totalPairOffAmount)
        {
          int num = (int) Utils.Dialog(owner, "Pair-off amount should be greater than reversal amount.");
          return false;
        }
      }
      this.totalPairOffAmount = totalPairOffAmount;
      return true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panelRequestedBy = new Panel();
      this.txtRequestedBy = new TextBox();
      this.label9 = new Label();
      this.panelFeeDetails = new Panel();
      this.label5 = new Label();
      this.txtGainLoss = new TextBox();
      this.label4 = new Label();
      this.txtPairOffPercentage = new TextBox();
      this.txtTradeAmount = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.panelDate = new Panel();
      this.dtDate = new DatePicker();
      this.label1 = new Label();
      this.panelButtons = new Panel();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label6 = new Label();
      this.lblNote = new Label();
      this.btnCancel = new Button();
      this.btnOk = new Button();
      this.panelComments = new Panel();
      this.txtComments = new TextBox();
      this.label10 = new Label();
      this.panelRequestedBy.SuspendLayout();
      this.panelFeeDetails.SuspendLayout();
      this.panelDate.SuspendLayout();
      this.panelButtons.SuspendLayout();
      this.panelComments.SuspendLayout();
      this.SuspendLayout();
      this.panelRequestedBy.Controls.Add((Control) this.txtRequestedBy);
      this.panelRequestedBy.Controls.Add((Control) this.label9);
      this.panelRequestedBy.Location = new Point(0, 36);
      this.panelRequestedBy.Name = "panelRequestedBy";
      this.panelRequestedBy.Size = new Size(307, 25);
      this.panelRequestedBy.TabIndex = 2;
      this.txtRequestedBy.Location = new Point((int) sbyte.MaxValue, 3);
      this.txtRequestedBy.MaxLength = 150;
      this.txtRequestedBy.Name = "txtRequestedBy";
      this.txtRequestedBy.Size = new Size(145, 20);
      this.txtRequestedBy.TabIndex = 2;
      this.txtRequestedBy.TextChanged += new EventHandler(this.OnFieldValueChanged);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(18, 5);
      this.label9.Name = "label9";
      this.label9.Size = new Size(74, 13);
      this.label9.TabIndex = 28;
      this.label9.Text = "Requested By";
      this.panelFeeDetails.Controls.Add((Control) this.label5);
      this.panelFeeDetails.Controls.Add((Control) this.txtGainLoss);
      this.panelFeeDetails.Controls.Add((Control) this.label4);
      this.panelFeeDetails.Controls.Add((Control) this.txtPairOffPercentage);
      this.panelFeeDetails.Controls.Add((Control) this.txtTradeAmount);
      this.panelFeeDetails.Controls.Add((Control) this.label3);
      this.panelFeeDetails.Controls.Add((Control) this.label2);
      this.panelFeeDetails.Location = new Point(1, 61);
      this.panelFeeDetails.Name = "panelFeeDetails";
      this.panelFeeDetails.Size = new Size(306, 69);
      this.panelFeeDetails.TabIndex = 3;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(274, 28);
      this.label5.Name = "label5";
      this.label5.Size = new Size(15, 13);
      this.label5.TabIndex = 28;
      this.label5.Text = "%";
      this.txtGainLoss.Location = new Point(126, 48);
      this.txtGainLoss.MaxLength = 12;
      this.txtGainLoss.Name = "txtGainLoss";
      this.txtGainLoss.Size = new Size(145, 20);
      this.txtGainLoss.TabIndex = 5;
      this.txtGainLoss.TextAlign = HorizontalAlignment.Right;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(16, 50);
      this.label4.Name = "label4";
      this.label4.Size = new Size(59, 13);
      this.label4.TabIndex = 26;
      this.label4.Text = "Gain/Loss ";
      this.txtPairOffPercentage.Location = new Point(126, 25);
      this.txtPairOffPercentage.MaxLength = 12;
      this.txtPairOffPercentage.Name = "txtPairOffPercentage";
      this.txtPairOffPercentage.Size = new Size(145, 20);
      this.txtPairOffPercentage.TabIndex = 4;
      this.txtPairOffPercentage.TextAlign = HorizontalAlignment.Right;
      this.txtPairOffPercentage.TextChanged += new EventHandler(this.OnFieldValueChanged);
      this.txtTradeAmount.Location = new Point(126, 2);
      this.txtTradeAmount.MaxLength = 12;
      this.txtTradeAmount.Name = "txtTradeAmount";
      this.txtTradeAmount.Size = new Size(145, 20);
      this.txtTradeAmount.TabIndex = 3;
      this.txtTradeAmount.TextAlign = HorizontalAlignment.Right;
      this.txtTradeAmount.TextChanged += new EventHandler(this.OnFieldValueChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(16, 5);
      this.label3.Name = "label3";
      this.label3.Size = new Size(74, 13);
      this.label3.TabIndex = 23;
      this.label3.Text = "Trade Amount";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(16, 28);
      this.label2.Name = "label2";
      this.label2.Size = new Size(63, 13);
      this.label2.TabIndex = 22;
      this.label2.Text = "Pair-Off Fee";
      this.panelDate.Controls.Add((Control) this.dtDate);
      this.panelDate.Controls.Add((Control) this.label1);
      this.panelDate.Dock = DockStyle.Top;
      this.panelDate.Location = new Point(0, 0);
      this.panelDate.Name = "panelDate";
      this.panelDate.Size = new Size(307, 37);
      this.panelDate.TabIndex = 1;
      this.dtDate.BackColor = SystemColors.Window;
      this.dtDate.Location = new Point((int) sbyte.MaxValue, 13);
      this.dtDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dtDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtDate.Name = "dtDate";
      this.dtDate.Size = new Size(145, 21);
      this.dtDate.TabIndex = 1;
      this.dtDate.ToolTip = "";
      this.dtDate.Value = new DateTime(0L);
      this.dtDate.ValueChanged += new EventHandler(this.valueChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(18, 17);
      this.label1.Name = "label1";
      this.label1.Size = new Size(68, 13);
      this.label1.TabIndex = 14;
      this.label1.Text = "Pair-Off Date";
      this.panelButtons.Controls.Add((Control) this.label8);
      this.panelButtons.Controls.Add((Control) this.label7);
      this.panelButtons.Controls.Add((Control) this.label6);
      this.panelButtons.Controls.Add((Control) this.lblNote);
      this.panelButtons.Controls.Add((Control) this.btnCancel);
      this.panelButtons.Controls.Add((Control) this.btnOk);
      this.panelButtons.Location = new Point(1, 209);
      this.panelButtons.Name = "panelButtons";
      this.panelButtons.Size = new Size(306, 110);
      this.panelButtons.TabIndex = 5;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(16, 81);
      this.label8.Name = "label8";
      this.label8.Size = new Size(125, 13);
      this.label8.TabIndex = 31;
      this.label8.Text = "indicate a Pair - Off Loss.";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(16, 64);
      this.label7.Name = "label7";
      this.label7.Size = new Size(278, 13);
      this.label7.TabIndex = 30;
      this.label7.Text = "Pair - Off Gain, a negative Pair - Off Fee or Gain/Loss will ";
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(52, 44);
      this.label6.Name = "label6";
      this.label6.Size = new Size(240, 13);
      this.label6.TabIndex = 29;
      this.label6.Text = "A positive Pair-off Fee or Gain/Loss will indicate a";
      this.lblNote.AutoSize = true;
      this.lblNote.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblNote.Location = new Point(13, 44);
      this.lblNote.Name = "lblNote";
      this.lblNote.Size = new Size(38, 13);
      this.lblNote.TabIndex = 28;
      this.lblNote.Text = "Note:";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(196, 14);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOk.Location = new Point(115, 14);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 7;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.panelComments.Controls.Add((Control) this.txtComments);
      this.panelComments.Controls.Add((Control) this.label10);
      this.panelComments.Location = new Point(1, 132);
      this.panelComments.Name = "panelComments";
      this.panelComments.Size = new Size(306, 77);
      this.panelComments.TabIndex = 4;
      this.txtComments.Location = new Point(126, 3);
      this.txtComments.MaxLength = 150;
      this.txtComments.Multiline = true;
      this.txtComments.Name = "txtComments";
      this.txtComments.ScrollBars = ScrollBars.Vertical;
      this.txtComments.Size = new Size(176, 70);
      this.txtComments.TabIndex = 6;
      this.txtComments.TextChanged += new EventHandler(this.OnFieldValueChanged);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(17, 4);
      this.label10.Name = "label10";
      this.label10.Size = new Size(56, 13);
      this.label10.TabIndex = 34;
      this.label10.Text = "Comments";
      this.AcceptButton = (IButtonControl) this.btnOk;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(307, 316);
      this.Controls.Add((Control) this.panelDate);
      this.Controls.Add((Control) this.panelRequestedBy);
      this.Controls.Add((Control) this.panelFeeDetails);
      this.Controls.Add((Control) this.panelComments);
      this.Controls.Add((Control) this.panelButtons);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CommonPairOffDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.panelRequestedBy.ResumeLayout(false);
      this.panelRequestedBy.PerformLayout();
      this.panelFeeDetails.ResumeLayout(false);
      this.panelFeeDetails.PerformLayout();
      this.panelDate.ResumeLayout(false);
      this.panelDate.PerformLayout();
      this.panelButtons.ResumeLayout(false);
      this.panelButtons.PerformLayout();
      this.panelComments.ResumeLayout(false);
      this.panelComments.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

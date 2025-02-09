// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentTradePairOffDialog
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
  public class CorrespondentTradePairOffDialog : Form
  {
    private CorrespondentTradePairOff pairOff;
    private bool modified;
    private bool readOnly;
    private bool ignoreEvent;
    private IContainer components;
    private TextBox txtPairOffPercentage;
    private TextBox txtTradeAmount;
    private DatePicker dtDate;
    private Label label3;
    private Label label2;
    private Label label1;
    private Button btnOk;
    private Label label4;
    private TextBox txtGainLoss;
    private Button btnCancel;
    private Label label5;
    private Label lblNote;
    private Label label6;
    private Label label7;
    private Label label8;

    public CorrespondentTradePairOffDialog(CorrespondentTradePairOff pairOff)
    {
      this.InitializeComponent();
      this.AutoValidate = AutoValidate.Disable;
      TextBoxFormatter.Attach(this.txtTradeAmount, TextBoxContentRule.NonNegativeDecimal, "#,##0.00#");
      TextBoxFormatter.Attach(this.txtPairOffPercentage, TextBoxContentRule.Decimal, "#,##0.#####;;\"\"");
      TextBoxFormatter.Attach(this.txtGainLoss, TextBoxContentRule.Decimal, "#,##0.00#");
      this.pairOff = pairOff;
      this.pairOff.Locked = true;
      this.clearFields();
      this.loadPairOffData(this.pairOff, (object) null);
    }

    public CorrespondentTradePairOff PairOff => this.pairOff;

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
        this.btnOk.Enabled = !this.readOnly;
        this.txtGainLoss.ReadOnly = this.readOnly;
      }
    }

    public void CommitChanges()
    {
      this.pairOff.Date = this.dtDate.Value;
      this.pairOff.TradeAmount = Utils.ParseDecimal((object) this.txtTradeAmount.Text);
      if (this.pairOff.TradeAmount == 0M)
        this.pairOff.PairOffFeePercentage = 0M;
      else
        this.pairOff.PairOffFeePercentage = Utils.ParseDecimal((object) this.txtPairOffPercentage.Text);
      this.modified = false;
    }

    private void clearFields()
    {
      this.dtDate.Text = "";
      this.txtTradeAmount.Text = "";
      this.txtPairOffPercentage.Text = "";
      this.txtGainLoss.Text = "";
    }

    private void loadPairOffData(CorrespondentTradePairOff pairOffInfo, object sender)
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
      this.ignoreEvent = false;
      this.modified = false;
    }

    private void OnFieldValueChanged(object sender, EventArgs e)
    {
      if (this.ignoreEvent)
        return;
      this.loadPairOffData(this.constructTempPairOff(), sender);
      this.valueChanged(sender, e);
    }

    private CorrespondentTradePairOff constructTempPairOff()
    {
      CorrespondentTradePairOff correspondentTradePairOff = new CorrespondentTradePairOff();
      correspondentTradePairOff.Date = this.dtDate.Value;
      correspondentTradePairOff.TradeAmount = Utils.ParseDecimal((object) this.txtTradeAmount.Text);
      if (correspondentTradePairOff.TradeAmount == 0M)
        correspondentTradePairOff.PairOffFeePercentage = 0M;
      else
        correspondentTradePairOff.PairOffFeePercentage = Utils.ParseDecimal((object) this.txtPairOffPercentage.Text);
      return correspondentTradePairOff;
    }

    private void valueChanged(object sender, EventArgs e)
    {
      if (this.ignoreEvent)
        return;
      this.modified = true;
    }

    private void btnOk_Click(object sender, EventArgs e) => this.ValidateChildren();

    private void txtGainLoss_TextChanged(object sender, EventArgs e)
    {
      if (this.ignoreEvent)
        return;
      CorrespondentTradePairOff pairOffInfo = this.constructTempPairOff();
      pairOffInfo.ReCalculatePairOffPercentage(Utils.ParseDecimal((object) this.txtGainLoss.Text));
      this.loadPairOffData(pairOffInfo, (object) this.txtGainLoss);
      this.modified = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtPairOffPercentage = new TextBox();
      this.txtTradeAmount = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.btnOk = new Button();
      this.label4 = new Label();
      this.txtGainLoss = new TextBox();
      this.btnCancel = new Button();
      this.label5 = new Label();
      this.lblNote = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.dtDate = new DatePicker();
      this.SuspendLayout();
      this.txtPairOffPercentage.Location = new Point(126, 66);
      this.txtPairOffPercentage.MaxLength = 12;
      this.txtPairOffPercentage.Name = "txtPairOffPercentage";
      this.txtPairOffPercentage.Size = new Size(145, 20);
      this.txtPairOffPercentage.TabIndex = 15;
      this.txtPairOffPercentage.TextAlign = HorizontalAlignment.Right;
      this.txtPairOffPercentage.TextChanged += new EventHandler(this.OnFieldValueChanged);
      this.txtTradeAmount.Location = new Point(126, 43);
      this.txtTradeAmount.MaxLength = 12;
      this.txtTradeAmount.Name = "txtTradeAmount";
      this.txtTradeAmount.Size = new Size(145, 20);
      this.txtTradeAmount.TabIndex = 14;
      this.txtTradeAmount.TextAlign = HorizontalAlignment.Right;
      this.txtTradeAmount.TextChanged += new EventHandler(this.OnFieldValueChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(16, 46);
      this.label3.Name = "label3";
      this.label3.Size = new Size(74, 13);
      this.label3.TabIndex = 12;
      this.label3.Text = "Trade Amount";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(16, 69);
      this.label2.Name = "label2";
      this.label2.Size = new Size(63, 13);
      this.label2.TabIndex = 11;
      this.label2.Text = "Pair-Off Fee";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(16, 22);
      this.label1.Name = "label1";
      this.label1.Size = new Size(68, 13);
      this.label1.TabIndex = 10;
      this.label1.Text = "Pair-Off Date";
      this.btnOk.DialogResult = DialogResult.OK;
      this.btnOk.Location = new Point(115, 123);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 17;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(16, 91);
      this.label4.Name = "label4";
      this.label4.Size = new Size(59, 13);
      this.label4.TabIndex = 18;
      this.label4.Text = "Gain/Loss ";
      this.txtGainLoss.Location = new Point(126, 89);
      this.txtGainLoss.MaxLength = 12;
      this.txtGainLoss.Name = "txtGainLoss";
      this.txtGainLoss.Size = new Size(145, 20);
      this.txtGainLoss.TabIndex = 19;
      this.txtGainLoss.TextAlign = HorizontalAlignment.Right;
      this.txtGainLoss.TextChanged += new EventHandler(this.txtGainLoss_TextChanged);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(196, 123);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 20;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(274, 69);
      this.label5.Name = "label5";
      this.label5.Size = new Size(15, 13);
      this.label5.TabIndex = 21;
      this.label5.Text = "%";
      this.lblNote.AutoSize = true;
      this.lblNote.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblNote.Location = new Point(13, 153);
      this.lblNote.Name = "lblNote";
      this.lblNote.Size = new Size(38, 13);
      this.lblNote.TabIndex = 22;
      this.lblNote.Text = "Note:";
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(52, 153);
      this.label6.Name = "label6";
      this.label6.Size = new Size(240, 13);
      this.label6.TabIndex = 23;
      this.label6.Text = "A positive Pair-off Fee or Gain/Loss will indicate a";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(16, 173);
      this.label7.Name = "label7";
      this.label7.Size = new Size(278, 13);
      this.label7.TabIndex = 24;
      this.label7.Text = "Pair - Off Gain, a negative Pair - Off Fee or Gain/Loss will ";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(16, 190);
      this.label8.Name = "label8";
      this.label8.Size = new Size(125, 13);
      this.label8.TabIndex = 25;
      this.label8.Text = "indicate a Pair - Off Loss.";
      this.dtDate.BackColor = SystemColors.Window;
      this.dtDate.Location = new Point(126, 18);
      this.dtDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dtDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtDate.Name = "dtDate";
      this.dtDate.Size = new Size(145, 21);
      this.dtDate.TabIndex = 13;
      this.dtDate.ToolTip = "";
      this.dtDate.Value = new DateTime(0L);
      this.dtDate.ValueChanged += new EventHandler(this.valueChanged);
      this.AcceptButton = (IButtonControl) this.btnOk;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(307, 210);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.lblNote);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.txtGainLoss);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.txtPairOffPercentage);
      this.Controls.Add((Control) this.txtTradeAmount);
      this.Controls.Add((Control) this.dtDate);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CorrespondentTradePairOffDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Correspondent Trade Pair-Off";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

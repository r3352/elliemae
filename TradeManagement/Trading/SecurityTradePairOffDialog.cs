// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.SecurityTradePairOffDialog
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
  public class SecurityTradePairOffDialog : Form
  {
    private PairOff pairOff;
    private Decimal securityTradePrice;
    private bool modified;
    private bool readOnly;
    private IContainer components;
    private TextBox txtBuyPrice;
    private TextBox txtTradeAmount;
    private DatePicker dtDate;
    private Label label3;
    private Label label2;
    private Label label1;
    private Button btnOk;
    private Label label4;
    private TextBox txtGainLoss;
    private Button btnCancel;

    public SecurityTradePairOffDialog(PairOff pairOff, Decimal securityTradePrice)
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtTradeAmount, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtBuyPrice, TextBoxContentRule.NonNegativeDecimal, "#,##0.#######;;\"\"");
      TextBoxFormatter.Attach(this.txtGainLoss, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      this.securityTradePrice = securityTradePrice;
      this.pairOff = pairOff;
      this.pairOff.Locked = true;
      this.LoadPairOffData();
    }

    public PairOff PairOff => this.pairOff;

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
        this.txtBuyPrice.ReadOnly = this.readOnly;
        this.btnOk.Enabled = !this.readOnly;
      }
    }

    public void CommitChanges()
    {
      this.pairOff.Date = this.dtDate.Value;
      this.pairOff.UndeliveredAmount = Utils.ParseDecimal((object) this.txtTradeAmount.Text);
      this.pairOff.Fee = Utils.ParseDecimal((object) this.txtBuyPrice.Text);
      this.modified = false;
    }

    private void LoadPairOffData()
    {
      this.dtDate.Value = this.pairOff.Date;
      this.txtTradeAmount.Text = "";
      this.txtBuyPrice.Text = "";
      this.txtGainLoss.Text = "";
      this.txtTradeAmount.Text = this.pairOff.UndeliveredAmount.ToString("#,##0");
      this.txtBuyPrice.Text = this.pairOff.Fee.ToString("#,##0.#######;;\"\"");
      this.UpdateGainLoss();
      this.modified = false;
    }

    private void OnFieldValueChanged(object sender, EventArgs e)
    {
      this.UpdateGainLoss();
      this.modified = true;
    }

    private void UpdateGainLoss()
    {
      this.txtGainLoss.Text = SecurityTradeCalculation.CalculatePairOffGainLoss(this.securityTradePrice, Utils.ParseDecimal((object) this.txtBuyPrice.Text), Utils.ParseDecimal((object) this.txtTradeAmount.Text)).ToString("#,##0");
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtBuyPrice = new TextBox();
      this.txtTradeAmount = new TextBox();
      this.dtDate = new DatePicker();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.btnOk = new Button();
      this.label4 = new Label();
      this.txtGainLoss = new TextBox();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.txtBuyPrice.Location = new Point(126, 66);
      this.txtBuyPrice.MaxLength = 12;
      this.txtBuyPrice.Name = "txtBuyPrice";
      this.txtBuyPrice.Size = new Size(145, 20);
      this.txtBuyPrice.TabIndex = 15;
      this.txtBuyPrice.TextAlign = HorizontalAlignment.Right;
      this.txtBuyPrice.TextChanged += new EventHandler(this.OnFieldValueChanged);
      this.txtTradeAmount.Location = new Point(126, 43);
      this.txtTradeAmount.MaxLength = 12;
      this.txtTradeAmount.Name = "txtTradeAmount";
      this.txtTradeAmount.Size = new Size(145, 20);
      this.txtTradeAmount.TabIndex = 14;
      this.txtTradeAmount.TextAlign = HorizontalAlignment.Right;
      this.txtTradeAmount.TextChanged += new EventHandler(this.OnFieldValueChanged);
      this.dtDate.BackColor = SystemColors.Window;
      this.dtDate.Location = new Point(126, 18);
      this.dtDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dtDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtDate.Name = "dtDate";
      this.dtDate.Size = new Size(145, 21);
      this.dtDate.TabIndex = 13;
      this.dtDate.ToolTip = "";
      this.dtDate.Value = new DateTime(0L);
      this.dtDate.ValueChanged += new EventHandler(this.OnFieldValueChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(16, 46);
      this.label3.Name = "label3";
      this.label3.Size = new Size(74, 13);
      this.label3.TabIndex = 12;
      this.label3.Text = "Trade Amount";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(16, 69);
      this.label2.Name = "label2";
      this.label2.Size = new Size(52, 13);
      this.label2.TabIndex = 11;
      this.label2.Text = "Buy Price";
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
      this.btnOk.Text = "Ok";
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
      this.txtGainLoss.ReadOnly = true;
      this.txtGainLoss.Size = new Size(145, 20);
      this.txtGainLoss.TabIndex = 19;
      this.txtGainLoss.TextAlign = HorizontalAlignment.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(196, 123);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 20;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOk;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(307, 158);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.txtGainLoss);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.txtBuyPrice);
      this.Controls.Add((Control) this.txtTradeAmount);
      this.Controls.Add((Control) this.dtDate);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SecurityTradePairOffDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Security Trade Pair-Off";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

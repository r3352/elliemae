// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.AddMSRItemDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class AddMSRItemDialog : Form
  {
    private TradePricingItem pricingItem;
    private string decimalFormat = "0.000";
    private IContainer components;
    private Button btnCancel;
    private Button btnOK;
    private Label label1;
    private TextBox txtRate;
    private Label label2;
    private TextBox txtPrice;
    private Label label3;
    private Button btnAddAnother;

    public AddMSRItemDialog()
    {
      this.InitializeComponent();
      if (Session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas)
        this.decimalFormat = "0.0000000000";
      TextBoxFormatter.Attach(this.txtRate, TextBoxContentRule.NonNegativeDecimal, "0.000");
      TextBoxFormatter.Attach(this.txtPrice, TextBoxContentRule.NonNegativeDecimal, this.decimalFormat);
      if (!Session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas)
        return;
      this.txtPrice.MaxLength += 7;
    }

    public TradePricingItem PricingItem => this.pricingItem;

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.AddPricingData())
        return;
      this.DialogResult = DialogResult.OK;
    }

    private bool AddPricingData()
    {
      if (this.txtRate.Text == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first enter a Rate for this item.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (this.txtPrice.Text == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first enter a Price for this item.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (Utils.ParseDecimal((object) this.txtPrice.Text) >= 1000M || Utils.ParseDecimal((object) this.txtPrice.Text) < 0M)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Price must be greater than or equal to 0 and less than 1000.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      this.pricingItem = !Session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas ? new TradePricingItem(Utils.ParseDecimal((object) this.txtRate.Text), Utils.ParseDecimal((object) this.txtPrice.Text)) : new TradePricingItem(Utils.ParseDecimal((object) this.txtRate.Text), Utils.ParseDecimal((object) this.txtPrice.Text, false, 10));
      return true;
    }

    private void btnAddAnother_Click(object sender, EventArgs e)
    {
      if (!this.AddPricingData())
        return;
      this.DialogResult = DialogResult.Retry;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.label1 = new Label();
      this.txtRate = new TextBox();
      this.label2 = new Label();
      this.txtPrice = new TextBox();
      this.label3 = new Label();
      this.btnAddAnother = new Button();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(194, 75);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Location = new Point(14, 75);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 7;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(67, 18);
      this.label1.Name = "label1";
      this.label1.RightToLeft = RightToLeft.Yes;
      this.label1.Size = new Size(29, 14);
      this.label1.TabIndex = 1;
      this.label1.Text = "Rate";
      this.txtRate.Location = new Point(102, 15);
      this.txtRate.MaxLength = 8;
      this.txtRate.Name = "txtRate";
      this.txtRate.Size = new Size(88, 20);
      this.txtRate.TabIndex = 2;
      this.txtRate.TextAlign = HorizontalAlignment.Right;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(65, 41);
      this.label2.Name = "label2";
      this.label2.RightToLeft = RightToLeft.Yes;
      this.label2.Size = new Size(31, 14);
      this.label2.TabIndex = 3;
      this.label2.Text = "Price";
      this.txtPrice.Location = new Point(102, 38);
      this.txtPrice.MaxLength = 8;
      this.txtPrice.Name = "txtPrice";
      this.txtPrice.Size = new Size(88, 20);
      this.txtPrice.TabIndex = 4;
      this.txtPrice.TextAlign = HorizontalAlignment.Right;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(191, 19);
      this.label3.Name = "label3";
      this.label3.Size = new Size(17, 14);
      this.label3.TabIndex = 6;
      this.label3.Text = "%";
      this.btnAddAnother.Location = new Point(96, 75);
      this.btnAddAnother.Name = "btnAddAnother";
      this.btnAddAnother.Size = new Size(92, 22);
      this.btnAddAnother.TabIndex = 8;
      this.btnAddAnother.Text = "&Add Another";
      this.btnAddAnother.UseVisualStyleBackColor = true;
      this.btnAddAnother.Click += new EventHandler(this.btnAddAnother_Click);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(282, 114);
      this.Controls.Add((Control) this.btnAddAnother);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.txtPrice);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtRate);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddMSRItemDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add MSR Value";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

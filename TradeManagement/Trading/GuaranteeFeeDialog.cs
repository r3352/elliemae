// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.GuaranteeFeeDialog
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
  public class GuaranteeFeeDialog : Form
  {
    private GuarantyFeeItem guarantyFee = new GuarantyFeeItem();
    private bool recursive;
    private IContainer components;
    private Button btnCancel;
    private Button btnAddAnother;
    private Button btnOK;
    private TextBox txtGuarantyFee;
    private TextBox txtCoupon;
    private Label lblGuarantyFee;
    private Label lblCoupon;
    private Label lblProductName;
    private ComboBox cmbProductName;
    private Label label3;
    private Label label4;
    private Label label5;
    private TextBox txtCPA;
    private Label lblCPA;

    public GuaranteeFeeDialog(List<string> products)
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtGuarantyFee, TextBoxContentRule.NonNegativeDecimal, "#0.000;;\"\"");
      TextBoxFormatter.Attach(this.txtCPA, TextBoxContentRule.NonNegativeDecimal, "#0.000;;\"\"");
      this.cmbProductName.DataSource = (object) products;
    }

    public GuaranteeFeeDialog(GuarantyFeeItem item, List<string> products)
      : this(products)
    {
      this.guarantyFee = item;
      this.cmbProductName.Text = item.ProductName;
      TextBox txtCoupon = this.txtCoupon;
      Decimal couponMin = item.CouponMin;
      Decimal? couponMax = item.CouponMax;
      Decimal valueOrDefault = couponMax.GetValueOrDefault();
      Decimal num;
      string str1;
      if (couponMin == valueOrDefault & couponMax.HasValue)
      {
        str1 = item.CouponMin.ToString("#0.000;;\"\"");
      }
      else
      {
        string str2 = item.CouponMin.ToString("#0.000;;\"\"");
        num = item.CouponMax.Value;
        string str3 = num.ToString("#0.000;;\"\"");
        str1 = str2 + " - " + str3;
      }
      txtCoupon.Text = str1;
      TextBox txtGuarantyFee = this.txtGuarantyFee;
      num = item.GuarantyFee;
      string str4 = num.ToString();
      txtGuarantyFee.Text = str4;
      TextBox txtCpa = this.txtCPA;
      num = item.CPA;
      string str5 = num.ToString("#0.000;;\"\"");
      txtCpa.Text = str5;
      this.btnCancel.Location = this.btnAddAnother.Location;
      this.btnAddAnother.Hide();
    }

    private void loadPageData()
    {
      GuarantyFeeItem guarantyFee = this.guarantyFee;
    }

    public GuarantyFeeItem GuarantyFee => this.guarantyFee;

    public bool IsCreatingAnother => this.recursive;

    private void btnAddAnother_Click(object sender, EventArgs e)
    {
      string text = this.Validate();
      if (text != string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.guarantyFee.ProductName = this.cmbProductName.Text;
        this.txtCoupon_Leave((object) this.txtCoupon, (EventArgs) null);
        this.guarantyFee.GuarantyFee = Decimal.Parse(this.txtGuarantyFee.Text);
        this.guarantyFee.CPA = Utils.ParseDecimal((object) this.txtCPA.Text);
        this.recursive = true;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      string text = this.Validate();
      if (text != string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.guarantyFee.ProductName = this.cmbProductName.Text;
        this.txtCoupon_Leave((object) this.txtCoupon, (EventArgs) null);
        this.guarantyFee.GuarantyFee = Decimal.Parse(this.txtGuarantyFee.Text);
        this.guarantyFee.CPA = Utils.ParseDecimal((object) this.txtCPA.Text);
        this.recursive = false;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void txtCoupon_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsControl(e.KeyChar))
        return;
      Decimal result = 0M;
      if (e.KeyChar == '.' && this.txtCoupon.Text.IndexOf('-') < 0 && this.txtCoupon.Text.IndexOf('.') < 0)
        e.Handled = false;
      else if (e.KeyChar == '-')
        e.Handled = !Decimal.TryParse(this.txtCoupon.Text.Trim(), out result);
      else if (e.KeyChar == ' ' && this.txtCoupon.Text.IndexOf(' ') < 0)
      {
        e.Handled = !Decimal.TryParse(this.txtCoupon.Text.Trim(), out result);
      }
      else
      {
        if (e.KeyChar == '.' && this.txtCoupon.Text.IndexOf('.') < this.txtCoupon.Text.IndexOf('-'))
        {
          if (this.txtCoupon.Text.Split('.').Length <= 2)
          {
            e.Handled = false;
            return;
          }
        }
        if (this.txtCoupon.Text.Length > 0 && this.txtCoupon.Text.IndexOf(' ') == this.txtCoupon.Text.Length - 1)
          e.Handled = true;
        else
          e.Handled = !char.IsDigit(e.KeyChar);
      }
    }

    private void txtCoupon_KeyUp(object sender, KeyEventArgs e)
    {
      if (this.txtCoupon.Text.IndexOf(" - ") > 0 || e.KeyValue != 109)
        return;
      this.txtCoupon.Text = this.txtCoupon.Text.Replace(" ", "");
      this.txtCoupon.Text = this.txtCoupon.Text.Replace("-", " - ");
      this.txtCoupon.SelectionStart = this.txtCoupon.Text.Length > 0 ? this.txtCoupon.Text.Length : 0;
    }

    private void txtCoupon_Leave(object sender, EventArgs e)
    {
      this.txtCoupon.Text.Replace(" ", "");
      string[] strArray = this.txtCoupon.Text.Split(new string[1]
      {
        "-"
      }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray.Length == 1)
      {
        Decimal result = 0M;
        if (!Decimal.TryParse(strArray[0], out result))
        {
          this.txtCoupon.Focus();
          return;
        }
        this.txtCoupon.Text = result.ToString("#0.000;;\"\"");
        this.guarantyFee.CouponMin = result;
        this.guarantyFee.CouponMax = new Decimal?(result);
      }
      if (strArray.Length != 2)
        return;
      Decimal result1 = 0M;
      Decimal result2 = 0M;
      if (!Decimal.TryParse(strArray[0], out result1))
        this.txtCoupon.Focus();
      else if (!Decimal.TryParse(strArray[1], out result2))
      {
        this.txtCoupon.Focus();
      }
      else
      {
        this.guarantyFee.CouponMin = result1;
        this.guarantyFee.CouponMax = new Decimal?(result2);
        this.txtCoupon.Text = result1.ToString("#0.000;;\"\"") + " - " + result2.ToString("#0.000;;\"\"");
      }
    }

    public string Validate()
    {
      return this.cmbProductName.Text == "" || string.IsNullOrEmpty(this.txtCoupon.Text) || string.IsNullOrEmpty(this.txtGuarantyFee.Text) ? "Please provide data for Guaranty Fee Pricing" : "";
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
      this.btnAddAnother = new Button();
      this.btnOK = new Button();
      this.txtGuarantyFee = new TextBox();
      this.txtCoupon = new TextBox();
      this.lblGuarantyFee = new Label();
      this.lblCoupon = new Label();
      this.lblProductName = new Label();
      this.cmbProductName = new ComboBox();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.txtCPA = new TextBox();
      this.lblCPA = new Label();
      this.SuspendLayout();
      this.btnCancel.Location = new Point(215, 110);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(93, 23);
      this.btnCancel.TabIndex = 45;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnAddAnother.Location = new Point(116, 110);
      this.btnAddAnother.Name = "btnAddAnother";
      this.btnAddAnother.Size = new Size(93, 23);
      this.btnAddAnother.TabIndex = 40;
      this.btnAddAnother.Text = "Add Another";
      this.btnAddAnother.UseVisualStyleBackColor = true;
      this.btnAddAnother.Click += new EventHandler(this.btnAddAnother_Click);
      this.btnOK.Location = new Point(17, 110);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(93, 23);
      this.btnOK.TabIndex = 35;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.txtGuarantyFee.Location = new Point(120, 55);
      this.txtGuarantyFee.Name = "txtGuarantyFee";
      this.txtGuarantyFee.Size = new Size(188, 20);
      this.txtGuarantyFee.TabIndex = 25;
      this.txtCoupon.Location = new Point(120, 34);
      this.txtCoupon.Name = "txtCoupon";
      this.txtCoupon.Size = new Size(188, 20);
      this.txtCoupon.TabIndex = 20;
      this.txtCoupon.KeyPress += new KeyPressEventHandler(this.txtCoupon_KeyPress);
      this.txtCoupon.KeyUp += new KeyEventHandler(this.txtCoupon_KeyUp);
      this.txtCoupon.Leave += new EventHandler(this.txtCoupon_Leave);
      this.lblGuarantyFee.AutoSize = true;
      this.lblGuarantyFee.Location = new Point(20, 58);
      this.lblGuarantyFee.Name = "lblGuarantyFee";
      this.lblGuarantyFee.Size = new Size(71, 13);
      this.lblGuarantyFee.TabIndex = 16;
      this.lblGuarantyFee.Text = "Guaranty Fee";
      this.lblCoupon.AutoSize = true;
      this.lblCoupon.Location = new Point(20, 37);
      this.lblCoupon.Name = "lblCoupon";
      this.lblCoupon.Size = new Size(44, 13);
      this.lblCoupon.TabIndex = 15;
      this.lblCoupon.Text = "Coupon";
      this.lblProductName.AutoSize = true;
      this.lblProductName.Location = new Point(20, 16);
      this.lblProductName.Name = "lblProductName";
      this.lblProductName.Size = new Size(75, 13);
      this.lblProductName.TabIndex = 14;
      this.lblProductName.Text = "Product Name";
      this.cmbProductName.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbProductName.FormattingEnabled = true;
      this.cmbProductName.Location = new Point(120, 12);
      this.cmbProductName.Name = "cmbProductName";
      this.cmbProductName.Size = new Size(188, 21);
      this.cmbProductName.TabIndex = 10;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(14, 154);
      this.label3.Name = "label3";
      this.label3.Size = new Size(80, 13);
      this.label3.TabIndex = 29;
      this.label3.Text = "Please Note:";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(98, 154);
      this.label4.Name = "label4";
      this.label4.Size = new Size(221, 13);
      this.label4.TabIndex = 30;
      this.label4.Text = "An Adjustable Product should have a coupon";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(14, 172);
      this.label5.Name = "label5";
      this.label5.Size = new Size(146, 13);
      this.label5.TabIndex = 31;
      this.label5.Text = "range such as 2.000 - 2.4999";
      this.txtCPA.Location = new Point(120, 77);
      this.txtCPA.Name = "txtCPA";
      this.txtCPA.Size = new Size(188, 20);
      this.txtCPA.TabIndex = 30;
      this.lblCPA.AutoSize = true;
      this.lblCPA.Location = new Point(20, 79);
      this.lblCPA.Name = "lblCPA";
      this.lblCPA.Size = new Size(28, 13);
      this.lblCPA.TabIndex = 32;
      this.lblCPA.Text = "CPA";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(329, 221);
      this.Controls.Add((Control) this.txtCPA);
      this.Controls.Add((Control) this.lblCPA);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.cmbProductName);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnAddAnother);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.txtGuarantyFee);
      this.Controls.Add((Control) this.txtCoupon);
      this.Controls.Add((Control) this.lblGuarantyFee);
      this.Controls.Add((Control) this.lblCoupon);
      this.Controls.Add((Control) this.lblProductName);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (GuaranteeFeeDialog);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Guaranty Fee Pricing";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

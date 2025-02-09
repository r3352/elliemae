// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MbsFannieFreddiePricingItemDialog
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
  public class MbsFannieFreddiePricingItemDialog : Form
  {
    private TradeAdvancedPricingItem pricingItem;
    private Decimal baseGuarantyFee;
    private Decimal coupon;
    private Decimal maxBU;
    private Decimal weightAVGPrice;
    private Decimal minServicingFee;
    private MbsPoolMortgageType poolType;
    private MbsPoolBuyUpDownItems buyUpDownItems;
    private Decimal cpa;
    private IContainer components;
    private Label label2;
    private Label label1;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private TextBox txt_NoteRate;
    private TextBox txt_ServicingFee;
    private TextBox txt_GuarantyFee;
    private TextBox txt_BUBDbps;
    private TextBox txt_BUExecution;
    private TextBox txt_BDExecution;
    private Label label7;
    private Button btn_OK;
    private Button btn_AddAnother;
    private Button btn_Cancel;
    private Label labelCPA;
    private TextBox txt_CPA;

    public TradeAdvancedPricingItem PricingItem
    {
      private set => this.pricingItem = value;
      get
      {
        if (this.pricingItem == null)
          this.pricingItem = new TradeAdvancedPricingItem();
        return this.pricingItem;
      }
    }

    public MbsFannieFreddiePricingItemDialog(
      Decimal servicingFee,
      Decimal coupon,
      Decimal baseGuarantyFee,
      Decimal maxBU,
      Decimal minServicingFee,
      MbsPoolMortgageType poolType,
      MbsPoolBuyUpDownItems buyUpDownItems,
      Decimal weightAVGPrice,
      Decimal cpa = 0M)
    {
      this.InitializeComponent();
      this.coupon = coupon;
      this.baseGuarantyFee = baseGuarantyFee;
      this.maxBU = maxBU;
      this.minServicingFee = minServicingFee;
      this.poolType = poolType;
      this.buyUpDownItems = buyUpDownItems;
      this.weightAVGPrice = weightAVGPrice;
      TextBoxFormatter.Attach(this.txt_NoteRate, TextBoxContentRule.Decimal, "#,##0.000");
      TextBoxFormatter.Attach(this.txt_ServicingFee, TextBoxContentRule.Decimal, "#,##0.0000;;\"\"");
      this.txt_ServicingFee.Text = servicingFee.ToString("0.0000");
      this.labelCPA.Visible = poolType == MbsPoolMortgageType.FannieMaePE;
      this.txt_CPA.Visible = poolType == MbsPoolMortgageType.FannieMaePE;
      TextBoxFormatter.Attach(this.txt_CPA, TextBoxContentRule.Decimal, "#,##0.000;;\"\"");
      this.cpa = cpa;
      this.txt_CPA.Text = cpa.ToString("0.000");
    }

    public bool Recursive { get; set; }

    private void btn_OK_Click(object sender, EventArgs e)
    {
      if (!this.finalValidation())
        return;
      this.SavePage();
      this.Recursive = false;
      this.DialogResult = DialogResult.OK;
    }

    private void btn_Cancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void btn_AddAnother_Click(object sender, EventArgs e)
    {
      if (!this.finalValidation())
        return;
      this.SavePage();
      this.Recursive = true;
      this.DialogResult = DialogResult.OK;
    }

    private bool finalValidation()
    {
      if (this.txt_NoteRate.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please provide data for Note Rate.");
        return false;
      }
      Decimal num1 = Utils.ParseDecimal((object) this.txt_NoteRate.Text.Trim());
      if (num1 >= 100M || num1 < 0M)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Note Rate must be between 0 and 100.");
        return false;
      }
      if (!(this.txt_ServicingFee.Text.Trim() == string.Empty))
        return true;
      int num3 = (int) Utils.Dialog((IWin32Window) this, "Please provide data for Servicing Fee.");
      return false;
    }

    private void SavePage()
    {
      if (!(this.PricingItem.NoteRate == 0M))
        return;
      this.pricingItem.NoteRate = Utils.ParseDecimal((object) this.txt_NoteRate.Text);
      this.pricingItem.ServicingFee = Utils.ParseDecimal((object) this.txt_ServicingFee.Text);
      this.pricingItem.GuarantyFee = Utils.ParseDecimal((object) this.txt_GuarantyFee.Text);
      this.pricingItem.BuyUp = Utils.ParseDecimal((object) this.txt_BUExecution.Text);
      this.pricingItem.BuyDown = Utils.ParseDecimal((object) this.txt_BDExecution.Text);
      this.pricingItem.ServicingRetained = Utils.ParseDecimal((object) this.txt_BUBDbps.Text);
    }

    private void txt_ServicingFee_Leave(object sender, EventArgs e) => this.CalculatePriceItem();

    private void CalculatePriceItem()
    {
      this.PricingItem.NoteRate = Utils.ParseDecimal((object) this.txt_NoteRate.Text);
      this.PricingItem.ServicingFee = Utils.ParseDecimal((object) this.txt_ServicingFee.Text);
      this.PricingItem = MbsPoolCalculation.CalculateTradeAdvancedPricing(this.coupon, this.baseGuarantyFee, Utils.ParseDecimal((object) this.txt_ServicingFee.Text), this.minServicingFee, this.maxBU, this.buyUpDownItems, this.PricingItem, this.poolType);
      this.txt_GuarantyFee.Text = this.PricingItem.GuarantyFee.ToString();
      this.txt_BUBDbps.Text = this.PricingItem.ServicingRetained.ToString("0.00000");
      this.txt_BUExecution.Text = this.PricingItem.BuyUp.ToString("0.00000");
      this.txt_BDExecution.Text = this.PricingItem.BuyDown.ToString("0.00000");
    }

    private void txt_NoteRate_Leave(object sender, EventArgs e) => this.CalculatePriceItem();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label2 = new Label();
      this.label1 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.txt_NoteRate = new TextBox();
      this.txt_ServicingFee = new TextBox();
      this.txt_GuarantyFee = new TextBox();
      this.txt_BUBDbps = new TextBox();
      this.txt_BUExecution = new TextBox();
      this.txt_BDExecution = new TextBox();
      this.label7 = new Label();
      this.btn_OK = new Button();
      this.btn_AddAnother = new Button();
      this.btn_Cancel = new Button();
      this.labelCPA = new Label();
      this.txt_CPA = new TextBox();
      this.SuspendLayout();
      this.label2.AutoSize = true;
      this.label2.Location = new Point(15, 34);
      this.label2.Name = "label2";
      this.label2.Size = new Size(72, 13);
      this.label2.TabIndex = 14;
      this.label2.Text = "Servicing Fee";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(15, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(56, 13);
      this.label1.TabIndex = 15;
      this.label1.Text = "Note Rate";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(15, 55);
      this.label3.Name = "label3";
      this.label3.Size = new Size(71, 13);
      this.label3.TabIndex = 16;
      this.label3.Text = "Guaranty Fee";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(15, 76);
      this.label4.Name = "label4";
      this.label4.Size = new Size(62, 13);
      this.label4.TabIndex = 17;
      this.label4.Text = "BU/BD bps";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(15, 97);
      this.label5.Name = "label5";
      this.label5.Size = new Size(72, 13);
      this.label5.TabIndex = 18;
      this.label5.Text = "BU Execution";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(15, 118);
      this.label6.Name = "label6";
      this.label6.Size = new Size(72, 13);
      this.label6.TabIndex = 19;
      this.label6.Text = "BD Execution";
      this.txt_NoteRate.Location = new Point(115, 10);
      this.txt_NoteRate.Name = "txt_NoteRate";
      this.txt_NoteRate.Size = new Size(100, 20);
      this.txt_NoteRate.TabIndex = 20;
      this.txt_NoteRate.TextAlign = HorizontalAlignment.Right;
      this.txt_NoteRate.Leave += new EventHandler(this.txt_NoteRate_Leave);
      this.txt_ServicingFee.Location = new Point(115, 31);
      this.txt_ServicingFee.Name = "txt_ServicingFee";
      this.txt_ServicingFee.Size = new Size(100, 20);
      this.txt_ServicingFee.TabIndex = 21;
      this.txt_ServicingFee.TextAlign = HorizontalAlignment.Right;
      this.txt_ServicingFee.Leave += new EventHandler(this.txt_ServicingFee_Leave);
      this.txt_GuarantyFee.Location = new Point(115, 52);
      this.txt_GuarantyFee.Name = "txt_GuarantyFee";
      this.txt_GuarantyFee.ReadOnly = true;
      this.txt_GuarantyFee.Size = new Size(100, 20);
      this.txt_GuarantyFee.TabIndex = 22;
      this.txt_GuarantyFee.TextAlign = HorizontalAlignment.Right;
      this.txt_BUBDbps.Location = new Point(115, 73);
      this.txt_BUBDbps.Name = "txt_BUBDbps";
      this.txt_BUBDbps.ReadOnly = true;
      this.txt_BUBDbps.Size = new Size(100, 20);
      this.txt_BUBDbps.TabIndex = 23;
      this.txt_BUBDbps.TextAlign = HorizontalAlignment.Right;
      this.txt_BUExecution.Location = new Point(115, 94);
      this.txt_BUExecution.Name = "txt_BUExecution";
      this.txt_BUExecution.ReadOnly = true;
      this.txt_BUExecution.Size = new Size(100, 20);
      this.txt_BUExecution.TabIndex = 24;
      this.txt_BUExecution.TextAlign = HorizontalAlignment.Right;
      this.txt_BDExecution.Location = new Point(115, 115);
      this.txt_BDExecution.Name = "txt_BDExecution";
      this.txt_BDExecution.ReadOnly = true;
      this.txt_BDExecution.Size = new Size(100, 20);
      this.txt_BDExecution.TabIndex = 25;
      this.txt_BDExecution.TextAlign = HorizontalAlignment.Right;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(221, 13);
      this.label7.Name = "label7";
      this.label7.Size = new Size(15, 13);
      this.label7.TabIndex = 26;
      this.label7.Text = "%";
      this.btn_OK.Location = new Point(12, 172);
      this.btn_OK.Name = "btn_OK";
      this.btn_OK.Size = new Size(75, 23);
      this.btn_OK.TabIndex = 27;
      this.btn_OK.Text = "OK";
      this.btn_OK.UseVisualStyleBackColor = true;
      this.btn_OK.Click += new EventHandler(this.btn_OK_Click);
      this.btn_AddAnother.Location = new Point(95, 172);
      this.btn_AddAnother.Name = "btn_AddAnother";
      this.btn_AddAnother.Size = new Size(75, 23);
      this.btn_AddAnother.TabIndex = 28;
      this.btn_AddAnother.Text = "Add Another";
      this.btn_AddAnother.UseVisualStyleBackColor = true;
      this.btn_AddAnother.Click += new EventHandler(this.btn_AddAnother_Click);
      this.btn_Cancel.Location = new Point(176, 172);
      this.btn_Cancel.Name = "btn_Cancel";
      this.btn_Cancel.Size = new Size(75, 23);
      this.btn_Cancel.TabIndex = 29;
      this.btn_Cancel.Text = "Cancel";
      this.btn_Cancel.UseVisualStyleBackColor = true;
      this.btn_Cancel.Click += new EventHandler(this.btn_Cancel_Click);
      this.labelCPA.AutoSize = true;
      this.labelCPA.Location = new Point(15, 138);
      this.labelCPA.Name = "labelCPA";
      this.labelCPA.Size = new Size(28, 13);
      this.labelCPA.TabIndex = 30;
      this.labelCPA.Text = "CPA";
      this.txt_CPA.Location = new Point(115, 136);
      this.txt_CPA.Name = "txt_CPA";
      this.txt_CPA.ReadOnly = true;
      this.txt_CPA.Size = new Size(100, 20);
      this.txt_CPA.TabIndex = 31;
      this.txt_CPA.TextAlign = HorizontalAlignment.Right;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(262, 212);
      this.Controls.Add((Control) this.txt_CPA);
      this.Controls.Add((Control) this.labelCPA);
      this.Controls.Add((Control) this.btn_Cancel);
      this.Controls.Add((Control) this.btn_AddAnother);
      this.Controls.Add((Control) this.btn_OK);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.txt_BDExecution);
      this.Controls.Add((Control) this.txt_BUExecution);
      this.Controls.Add((Control) this.txt_BUBDbps);
      this.Controls.Add((Control) this.txt_GuarantyFee);
      this.Controls.Add((Control) this.txt_ServicingFee);
      this.Controls.Add((Control) this.txt_NoteRate);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.label2);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MbsFannieFreddiePricingItemDialog);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Pricing Data";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

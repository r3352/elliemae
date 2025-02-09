// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeAdvancedPricingItemDialog
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
  public class TradeAdvancedPricingItemDialog : Form
  {
    private TradeAdvancedPricingItem pricingItem;
    private bool recursive;
    private bool requireValidation;
    private TradeType forTradeType;
    private IContainer components;
    private Label label1;
    private Label lblBuyUp;
    private Label lblBuyDown;
    private Label lblLine4;
    private Label lblLine5;
    private TextBox txtNoteRate;
    private TextBox txtBuyUp;
    private TextBox txtBuyDown;
    private TextBox txtLine4;
    private TextBox txtMandAdj;
    private Label label6;
    private Button btnOK;
    private Button btnAddAnother;
    private Button btnCancel;

    public TradeAdvancedPricingItemDialog() => this.InitializeComponent();

    public MbsPoolMortgageType PoolType { get; set; }

    public TradeType ForTradeType
    {
      get => this.forTradeType;
      set
      {
        this.forTradeType = value;
        if (value == TradeType.MbsPool)
        {
          this.lblLine4.Text = "Excess Servicing";
          this.txtMandAdj.Text = "";
          this.lblLine5.Visible = false;
          this.txtMandAdj.Visible = false;
          this.txtBuyDown.Enabled = false;
          this.txtBuyUp.Enabled = false;
          this.txtLine4.Enabled = false;
          this.txtBuyDown.TextChanged -= new EventHandler(this.txt_TextChanged);
          this.txtBuyDown.Validating -= new CancelEventHandler(this.txtBuyDown_Validating);
          this.txtBuyUp.TextChanged -= new EventHandler(this.txt_TextChanged);
          this.txtBuyUp.Validating -= new CancelEventHandler(this.txtBuyUp_Validating);
          this.txtLine4.TextChanged -= new EventHandler(this.txt_TextChanged);
          this.txtLine4.Validating -= new CancelEventHandler(this.txtLine4_Validating);
          this.txtNoteRate.TextChanged += new EventHandler(this.txtNoteRate_TextChanged);
          if (this.PoolType == MbsPoolMortgageType.GinnieMae)
          {
            this.txtBuyUp.Visible = false;
            this.lblBuyUp.Visible = false;
            this.txtBuyDown.Visible = false;
            this.lblBuyDown.Visible = false;
            this.btnOK.Top = this.lblLine4.Top - 15;
            this.btnAddAnother.Top = this.lblLine4.Top - 15;
            this.btnCancel.Top = this.lblLine4.Top - 15;
            this.txtLine4.Top = this.txtBuyUp.Top;
            this.lblLine4.Top = this.lblBuyUp.Top;
            this.Height = this.Height / 2 + 30;
          }
          else
          {
            this.lblLine5.Text = "Servicing Retained";
            this.lblLine5.Visible = true;
            this.txtMandAdj.Visible = true;
            this.txtMandAdj.Enabled = false;
          }
        }
        else
        {
          this.lblLine4.Text = "GNMA II Excess";
          this.lblLine5.Visible = true;
          this.txtMandAdj.Visible = true;
        }
      }
    }

    public Decimal Coupon { get; set; }

    public Decimal GuaranteeFee { get; set; }

    public Decimal ServiceFee { get; set; }

    public Decimal MinServicingFee { get; set; }

    public Decimal MaxBU { get; set; }

    public MbsPoolBuyUpDownItems BuyUpDownItems { get; set; }

    public TradeAdvancedPricingItemDialog(TradeAdvancedPricingItem pricingItem)
      : this()
    {
      this.pricingItem = pricingItem;
      this.ForTradeType = TradeType.LoanTrade;
    }

    private void loadPageData()
    {
      if (this.pricingItem == null)
        return;
      this.txtNoteRate.Text = this.pricingItem.NoteRate.ToString("0.000");
      this.txtBuyUp.Text = this.pricingItem.BuyUp.ToString("0.00000");
      this.txtBuyDown.Text = this.pricingItem.BuyDown.ToString("0.00000");
      this.txtLine4.Text = this.pricingItem.GNMAIIExcess.ToString("0.00000");
      if (this.PoolType == MbsPoolMortgageType.GinnieMae)
        this.txtMandAdj.Text = this.pricingItem.MandAdj.ToString("0.00000");
      else
        this.txtMandAdj.Text = this.pricingItem.ServicingRetained.ToString("0.00000");
    }

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

    public bool IsCreatingAnother => this.recursive;

    public void txtNoteRate_TextChanged(object sender, EventArgs e)
    {
      this.PricingItem.NoteRate = Utils.ParseDecimal((object) this.txtNoteRate.Text);
      this.PricingItem = MbsPoolCalculation.CalculateTradeAdvancedPricing(this.Coupon, this.GuaranteeFee, this.ServiceFee, this.MinServicingFee, this.MaxBU, this.BuyUpDownItems, this.PricingItem, this.PoolType);
      this.txtLine4.Text = this.PricingItem.GNMAIIExcess.ToString("0.00000;;\"\"");
      this.txtBuyUp.Text = this.PricingItem.BuyUp.ToString("0.00000;;\"\"");
      this.txtBuyDown.Text = this.PricingItem.BuyDown.ToString("0.00000;;\"\"");
      if (this.PoolType == MbsPoolMortgageType.GinnieMae)
        return;
      this.txtMandAdj.Text = this.PricingItem.ServicingRetained.ToString("0.00000;;\"\"");
    }

    private void btnAddAnother_Click(object sender, EventArgs e)
    {
      if (!this.finalValidation())
        return;
      if (this.PricingItem.NoteRate == 0M)
      {
        this.pricingItem.NoteRate = Utils.ParseDecimal((object) this.txtNoteRate.Text);
        this.pricingItem.BuyUp = Utils.ParseDecimal((object) this.txtBuyUp.Text);
        this.pricingItem.BuyDown = Utils.ParseDecimal((object) this.txtBuyDown.Text);
        this.pricingItem.GNMAIIExcess = Utils.ParseDecimal((object) this.txtLine4.Text);
        this.SetMandAdjSvcRtned();
      }
      this.recursive = true;
      this.DialogResult = DialogResult.OK;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.finalValidation())
        return;
      if (this.PricingItem.NoteRate == 0M)
      {
        this.pricingItem.NoteRate = Utils.ParseDecimal((object) this.txtNoteRate.Text);
        this.pricingItem.BuyUp = Utils.ParseDecimal((object) this.txtBuyUp.Text);
        this.pricingItem.BuyDown = Utils.ParseDecimal((object) this.txtBuyDown.Text);
        this.pricingItem.GNMAIIExcess = Utils.ParseDecimal((object) this.txtLine4.Text);
        this.SetMandAdjSvcRtned();
      }
      this.recursive = false;
      this.DialogResult = DialogResult.OK;
    }

    private void SetMandAdjSvcRtned()
    {
      if (this.ForTradeType != TradeType.MbsPool)
      {
        this.pricingItem.MandAdj = Utils.ParseDecimal((object) this.txtMandAdj.Text);
      }
      else
      {
        if (this.PoolType == MbsPoolMortgageType.GinnieMae)
          return;
        this.pricingItem.ServicingRetained = Utils.ParseDecimal((object) this.txtMandAdj.Text);
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private bool finalValidation()
    {
      if (!(this.txtNoteRate.Text.Trim() == string.Empty))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "Please provide data for Note Rate.");
      return false;
    }

    private void txtNoteRate_Validating(object sender, CancelEventArgs e)
    {
      if (!this.requireValidation)
        return;
      if (this.txtNoteRate.Text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please provide data for Note Rate.");
        e.Cancel = true;
      }
      else if (!TradeAdvancedPricingItemDialog.CheckDataFormat("NoteRate", this.txtNoteRate.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid data format for Note Rate.");
        e.Cancel = true;
      }
      else if (!TradeAdvancedPricingItemDialog.CheckDataRange("NoteRate", this.txtNoteRate.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Note Rate must be between 0 and 100.");
        e.Cancel = true;
      }
      else
        this.txtNoteRate.Text = Utils.ParseDecimal((object) this.txtNoteRate.Text).ToString("0.000");
    }

    private void txtBuyUp_Validating(object sender, CancelEventArgs e)
    {
      if (!this.requireValidation || this.txtBuyUp.Text == string.Empty)
        return;
      if (!TradeAdvancedPricingItemDialog.CheckDataFormat("BuyUp", this.txtBuyUp.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid data format for Buy Up.");
        e.Cancel = true;
      }
      else if (!TradeAdvancedPricingItemDialog.CheckDataRange("BuyUp", this.txtBuyUp.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Buy Up must be between 0 and 10.");
        e.Cancel = true;
      }
      else
        this.txtBuyUp.Text = Utils.ParseDecimal((object) this.txtBuyUp.Text).ToString("0.00000");
    }

    private void txtBuyDown_Validating(object sender, CancelEventArgs e)
    {
      if (!this.requireValidation || this.txtBuyDown.Text == string.Empty)
        return;
      if (!TradeAdvancedPricingItemDialog.CheckDataFormat("BuyDown", this.txtBuyDown.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid data format for Buy Down.");
        e.Cancel = true;
      }
      else if (!TradeAdvancedPricingItemDialog.CheckDataRange("BuyDown", this.txtBuyDown.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Buy Down must be between 0 and -10.");
        e.Cancel = true;
      }
      else
        this.txtBuyDown.Text = Utils.ParseDecimal((object) this.txtBuyDown.Text).ToString("0.00000");
    }

    private void txtLine4_Validating(object sender, CancelEventArgs e)
    {
      if (!this.requireValidation || this.txtLine4.Text == string.Empty)
        return;
      if (!TradeAdvancedPricingItemDialog.CheckDataFormat("GNMA", this.txtLine4.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid data format for GNMA II Excess.");
        e.Cancel = true;
      }
      else if (!TradeAdvancedPricingItemDialog.CheckDataRange("GNMA", this.txtLine4.Text))
      {
        if (this.ForTradeType == TradeType.MbsPool)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "Excess Servicing must be between 0 and 10.");
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "GNMA II Excess must be between 0 and 10.");
        }
        e.Cancel = true;
      }
      else
        this.txtLine4.Text = Utils.ParseDecimal((object) this.txtLine4.Text).ToString("0.00000");
    }

    private void txtMandAdj_Validating(object sender, CancelEventArgs e)
    {
      if (!this.requireValidation || this.txtMandAdj.Text == string.Empty)
        return;
      if (!TradeAdvancedPricingItemDialog.CheckDataFormat("MandAdj", this.txtMandAdj.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid data format for Mand. Adj.");
        e.Cancel = true;
      }
      else if (!TradeAdvancedPricingItemDialog.CheckDataRange("MandAdj", this.txtMandAdj.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Mand. Adj. must be between 0 and -10.");
        e.Cancel = true;
      }
      else
        this.txtMandAdj.Text = Utils.ParseDecimal((object) this.txtMandAdj.Text).ToString("0.00000");
    }

    public static bool CheckDataFormat(string fieldName, string value)
    {
      return (fieldName == "NoteRate" || fieldName == "BuyUp" || fieldName == "BuyDown" || fieldName == "GNMA" || fieldName == "MandAdj") && Utils.IsDecimal((object) value);
    }

    public static string FormatData(string fieldName, string value)
    {
      if (!TradeAdvancedPricingItemDialog.CheckDataRange(fieldName, value))
        return value;
      switch (fieldName)
      {
        case "NoteRate":
          return Utils.ParseDecimal((object) value).ToString("0.000");
        case "BuyUp":
        case "BuyDown":
        case "GNMA":
        case "MandAdj":
          return Utils.ParseDecimal((object) value).ToString("0.00000");
        default:
          return value;
      }
    }

    public static bool CheckDataRange(string fieldName, string valueString)
    {
      if (!TradeAdvancedPricingItemDialog.CheckDataFormat(fieldName, valueString))
        return false;
      switch (fieldName)
      {
        case "NoteRate":
          Decimal num1 = Utils.ParseDecimal((object) valueString);
          return !(num1 >= 100M) && !(num1 < 0M);
        case "BuyUp":
        case "GNMA":
          Decimal num2 = Utils.ParseDecimal((object) valueString);
          return !(num2 >= 10M) && !(num2 < 0M);
        case "BuyDown":
        case "MandAdj":
          Decimal num3 = Utils.ParseDecimal((object) valueString);
          return !(num3 <= -10M) && !(num3 > 0M);
        default:
          return false;
      }
    }

    private void txt_TextChanged(object sender, EventArgs e) => this.requireValidation = true;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.lblBuyUp = new Label();
      this.lblBuyDown = new Label();
      this.lblLine4 = new Label();
      this.lblLine5 = new Label();
      this.txtNoteRate = new TextBox();
      this.txtBuyUp = new TextBox();
      this.txtBuyDown = new TextBox();
      this.txtLine4 = new TextBox();
      this.txtMandAdj = new TextBox();
      this.label6 = new Label();
      this.btnOK = new Button();
      this.btnAddAnother = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(15, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(56, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Note Rate";
      this.lblBuyUp.AutoSize = true;
      this.lblBuyUp.Location = new Point(15, 34);
      this.lblBuyUp.Name = "lblBuyUp";
      this.lblBuyUp.Size = new Size(42, 13);
      this.lblBuyUp.TabIndex = 1;
      this.lblBuyUp.Text = "Buy Up";
      this.lblBuyDown.AutoSize = true;
      this.lblBuyDown.Location = new Point(15, 55);
      this.lblBuyDown.Name = "lblBuyDown";
      this.lblBuyDown.Size = new Size(56, 13);
      this.lblBuyDown.TabIndex = 2;
      this.lblBuyDown.Text = "Buy Down";
      this.lblLine4.AutoSize = true;
      this.lblLine4.Location = new Point(15, 76);
      this.lblLine4.Name = "lblLine4";
      this.lblLine4.Size = new Size(85, 13);
      this.lblLine4.TabIndex = 3;
      this.lblLine4.Text = "GNMA II Excess";
      this.lblLine5.AutoSize = true;
      this.lblLine5.Location = new Point(15, 97);
      this.lblLine5.Name = "lblLine5";
      this.lblLine5.Size = new Size(58, 13);
      this.lblLine5.TabIndex = 4;
      this.lblLine5.Text = "Mand. Adj.";
      this.txtNoteRate.Location = new Point(115, 10);
      this.txtNoteRate.Name = "txtNoteRate";
      this.txtNoteRate.Size = new Size(100, 20);
      this.txtNoteRate.TabIndex = 5;
      this.txtNoteRate.TextChanged += new EventHandler(this.txt_TextChanged);
      this.txtNoteRate.Validating += new CancelEventHandler(this.txtNoteRate_Validating);
      this.txtBuyUp.Location = new Point(115, 31);
      this.txtBuyUp.Name = "txtBuyUp";
      this.txtBuyUp.Size = new Size(100, 20);
      this.txtBuyUp.TabIndex = 6;
      this.txtBuyUp.TextChanged += new EventHandler(this.txt_TextChanged);
      this.txtBuyUp.Validating += new CancelEventHandler(this.txtBuyUp_Validating);
      this.txtBuyDown.Location = new Point(115, 52);
      this.txtBuyDown.Name = "txtBuyDown";
      this.txtBuyDown.Size = new Size(100, 20);
      this.txtBuyDown.TabIndex = 7;
      this.txtBuyDown.TextChanged += new EventHandler(this.txt_TextChanged);
      this.txtBuyDown.Validating += new CancelEventHandler(this.txtBuyDown_Validating);
      this.txtLine4.Location = new Point(115, 73);
      this.txtLine4.Name = "txtLine4";
      this.txtLine4.Size = new Size(100, 20);
      this.txtLine4.TabIndex = 8;
      this.txtLine4.TextChanged += new EventHandler(this.txt_TextChanged);
      this.txtLine4.Validating += new CancelEventHandler(this.txtLine4_Validating);
      this.txtMandAdj.Location = new Point(115, 94);
      this.txtMandAdj.Name = "txtMandAdj";
      this.txtMandAdj.Size = new Size(100, 20);
      this.txtMandAdj.TabIndex = 9;
      this.txtMandAdj.TextChanged += new EventHandler(this.txt_TextChanged);
      this.txtMandAdj.Validating += new CancelEventHandler(this.txtMandAdj_Validating);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(218, 14);
      this.label6.Name = "label6";
      this.label6.Size = new Size(15, 13);
      this.label6.TabIndex = 10;
      this.label6.Text = "%";
      this.btnOK.Location = new Point(12, 128);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 11;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnAddAnother.Location = new Point(95, 128);
      this.btnAddAnother.Name = "btnAddAnother";
      this.btnAddAnother.Size = new Size(75, 23);
      this.btnAddAnother.TabIndex = 12;
      this.btnAddAnother.Text = "Add Another";
      this.btnAddAnother.UseVisualStyleBackColor = true;
      this.btnAddAnother.Click += new EventHandler(this.btnAddAnother_Click);
      this.btnCancel.Location = new Point(176, 128);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 13;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(262, 169);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnAddAnother);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.txtMandAdj);
      this.Controls.Add((Control) this.txtLine4);
      this.Controls.Add((Control) this.txtBuyDown);
      this.Controls.Add((Control) this.txtBuyUp);
      this.Controls.Add((Control) this.txtNoteRate);
      this.Controls.Add((Control) this.lblLine5);
      this.Controls.Add((Control) this.lblLine4);
      this.Controls.Add((Control) this.lblBuyDown);
      this.Controls.Add((Control) this.lblBuyUp);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TradeAdvancedPricingItemDialog);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Pricing Data";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

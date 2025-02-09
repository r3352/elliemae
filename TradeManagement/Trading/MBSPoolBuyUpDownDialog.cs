// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MBSPoolBuyUpDownDialog
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
  public class MBSPoolBuyUpDownDialog : Form
  {
    private MbsPoolBuyUpDownItem buyUpDownItem;
    private bool recursive;
    private bool requireValidation;
    private bool isBuyUp;
    private IContainer components;
    private Button btnCancel;
    private Button btnAddAnother;
    private Button btnOK;
    private Label label6;
    private TextBox txtRatio;
    private TextBox txtGnrMax;
    private TextBox txtGnrMin;
    private Label lblRatio;
    private Label label2;
    private Label label1;
    private Label label4;

    public MBSPoolBuyUpDownDialog()
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtGnrMax, TextBoxContentRule.NonNegativeDecimal, "#0.00000;;\"\"");
      TextBoxFormatter.Attach(this.txtGnrMin, TextBoxContentRule.NonNegativeDecimal, "#0.00000;;\"\"");
      TextBoxFormatter.Attach(this.txtRatio, TextBoxContentRule.NonNegativeDecimal, "#0.00000;;\"\"");
    }

    public bool IsBuyUp
    {
      get => this.isBuyUp;
      set
      {
        this.isBuyUp = value;
        if (value)
        {
          this.lblRatio.Text = "Buy Up Ratio";
          this.Text = "Add Buy Up";
        }
        else
        {
          this.lblRatio.Text = "Buy Down Ratio";
          this.Text = "Add Buy Down";
        }
      }
    }

    public MBSPoolBuyUpDownDialog(MbsPoolBuyUpDownItem item)
      : this()
    {
      this.buyUpDownItem = item;
      this.isBuyUp = true;
    }

    private void loadPageData()
    {
      if (this.buyUpDownItem == null)
        return;
      this.txtGnrMax.Text = this.buyUpDownItem.GnrMax.ToString("0.00000");
      this.txtGnrMin.Text = this.buyUpDownItem.GnrMin.ToString("0.00000");
      this.txtRatio.Text = this.buyUpDownItem.Ratio.ToString("0.00000");
    }

    public MbsPoolBuyUpDownItem BuyUpDownItem
    {
      get
      {
        if (this.buyUpDownItem == null)
          this.buyUpDownItem = new MbsPoolBuyUpDownItem();
        this.buyUpDownItem.GnrMin = Utils.ParseDecimal((object) this.txtGnrMin.Text);
        this.buyUpDownItem.GnrMax = Utils.ParseDecimal((object) this.txtGnrMax.Text);
        this.buyUpDownItem.Ratio = Utils.ParseDecimal((object) this.txtRatio.Text);
        this.buyUpDownItem.IsBuyUp = this.isBuyUp;
        return this.buyUpDownItem;
      }
    }

    public bool IsCreatingAnother => this.recursive;

    private void btnAddAnother_Click(object sender, EventArgs e)
    {
      this.recursive = true;
      this.DialogResult = DialogResult.OK;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.recursive = false;
      this.DialogResult = DialogResult.OK;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void txt_TextChanged(object sender, EventArgs e) => this.requireValidation = true;

    private void txt_Validating(object sender, CancelEventArgs e)
    {
      if (!this.requireValidation || !(sender is TextBox))
        return;
      MbsPoolBuyUpDownItem poolBuyUpDownItem = new MbsPoolBuyUpDownItem()
      {
        GnrMin = 0M,
        GnrMax = 0M,
        Ratio = 0M,
        IsBuyUp = this.isBuyUp
      };
      if (((Control) sender).Name == "txtGnrMin")
        poolBuyUpDownItem.GnrMin = Utils.ParseDecimal((object) ((Control) sender).Text);
      if (((Control) sender).Name == "txtGnrMax")
        poolBuyUpDownItem.GnrMax = Utils.ParseDecimal((object) ((Control) sender).Text);
      if (((Control) sender).Name == "txtRatio")
        poolBuyUpDownItem.Ratio = Utils.ParseDecimal((object) ((Control) sender).Text);
      string text = poolBuyUpDownItem.ValidateDataRange();
      if (!(text != string.Empty))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      e.Cancel = true;
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
      this.label6 = new Label();
      this.txtRatio = new TextBox();
      this.txtGnrMax = new TextBox();
      this.txtGnrMin = new TextBox();
      this.lblRatio = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.label4 = new Label();
      this.SuspendLayout();
      this.btnCancel.Location = new Point(181, 91);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 27;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnAddAnother.Location = new Point(100, 91);
      this.btnAddAnother.Name = "btnAddAnother";
      this.btnAddAnother.Size = new Size(75, 23);
      this.btnAddAnother.TabIndex = 26;
      this.btnAddAnother.Text = "Add Another";
      this.btnAddAnother.UseVisualStyleBackColor = true;
      this.btnAddAnother.Click += new EventHandler(this.btnAddAnother_Click);
      this.btnOK.Location = new Point(17, 91);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 25;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(223, 17);
      this.label6.Name = "label6";
      this.label6.Size = new Size(15, 13);
      this.label6.TabIndex = 24;
      this.label6.Text = "%";
      this.txtRatio.Location = new Point(120, 55);
      this.txtRatio.Name = "txtRatio";
      this.txtRatio.Size = new Size(100, 20);
      this.txtRatio.TabIndex = 21;
      this.txtRatio.TextChanged += new EventHandler(this.txt_TextChanged);
      this.txtRatio.Validating += new CancelEventHandler(this.txt_Validating);
      this.txtGnrMax.Location = new Point(120, 34);
      this.txtGnrMax.Name = "txtGnrMax";
      this.txtGnrMax.Size = new Size(100, 20);
      this.txtGnrMax.TabIndex = 20;
      this.txtGnrMax.TextChanged += new EventHandler(this.txt_TextChanged);
      this.txtGnrMax.Validating += new CancelEventHandler(this.txt_Validating);
      this.txtGnrMin.Location = new Point(120, 13);
      this.txtGnrMin.Name = "txtGnrMin";
      this.txtGnrMin.Size = new Size(100, 20);
      this.txtGnrMin.TabIndex = 19;
      this.txtGnrMin.TextChanged += new EventHandler(this.txt_TextChanged);
      this.txtGnrMin.Validating += new CancelEventHandler(this.txt_Validating);
      this.lblRatio.AutoSize = true;
      this.lblRatio.Location = new Point(20, 58);
      this.lblRatio.Name = "lblRatio";
      this.lblRatio.Size = new Size(67, 13);
      this.lblRatio.TabIndex = 16;
      this.lblRatio.Text = "BuyUp Ratio";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(20, 37);
      this.label2.Name = "label2";
      this.label2.Size = new Size(54, 13);
      this.label2.TabIndex = 15;
      this.label2.Text = "GNR Max";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(20, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(51, 13);
      this.label1.TabIndex = 14;
      this.label1.Text = "GNR Min";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(223, 41);
      this.label4.Name = "label4";
      this.label4.Size = new Size(15, 13);
      this.label4.TabIndex = 28;
      this.label4.Text = "%";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(271, 125);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnAddAnother);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.txtRatio);
      this.Controls.Add((Control) this.txtGnrMax);
      this.Controls.Add((Control) this.txtGnrMin);
      this.Controls.Add((Control) this.lblRatio);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MBSPoolBuyUpDownDialog);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (MBSPoolBuyUpDownDialog);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MMPFormDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class MMPFormDialog : Form
  {
    private TextBox textBoxYear;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label label8;
    private GroupBox groupBox1;
    private Button okBtn;
    private Button cancelBtn;
    private TextBox textBoxIndex;
    private TextBox textBoxMargin;
    private TextBox textBoxAPR;
    private TextBox textBoxMMP;
    private ComboBox comboBoxPeriod;
    private Label label9;
    private System.ComponentModel.Container components;
    private ArrayList existingYears;
    private int currentYear;
    private bool useNewHELOCHistoricTable;
    private string decimalsUseForIndex = "ThreeDecimals";
    private int year;
    private string periodType = "";
    private double indexRate;
    private double marginRate;
    private double aprRate;
    private double monthlyPay;

    public MMPFormDialog(ArrayList existingYears)
      : this(existingYears, false, "ThreeDecimals")
    {
    }

    public MMPFormDialog(
      ArrayList existingYears,
      bool useNewHELOCHistoricTable,
      string decimalsUseForIndex)
      : this(0, "", 0.0, 0.0, 0.0, 0.0, existingYears, useNewHELOCHistoricTable, decimalsUseForIndex)
    {
    }

    public MMPFormDialog(
      int year,
      string periodType,
      double indexRate,
      double marginRate,
      double aprRate,
      double monthlyPay,
      ArrayList existingYears)
      : this(year, periodType, indexRate, marginRate, aprRate, monthlyPay, existingYears, false, "ThreeDecimals")
    {
    }

    public MMPFormDialog(
      int year,
      string periodType,
      double indexRate,
      double marginRate,
      double aprRate,
      double monthlyPay,
      ArrayList existingYears,
      bool useNewHELOCHistoricTable,
      string decimalsUseForIndex)
    {
      this.currentYear = year;
      this.existingYears = existingYears;
      this.useNewHELOCHistoricTable = useNewHELOCHistoricTable;
      this.decimalsUseForIndex = decimalsUseForIndex;
      this.InitializeComponent();
      this.label3.Visible = this.label4.Visible = this.label5.Visible = this.label7.Visible = this.label8.Visible = this.label9.Visible = this.comboBoxPeriod.Visible = this.textBoxMargin.Visible = this.textBoxAPR.Visible = this.textBoxMMP.Visible = !this.useNewHELOCHistoricTable;
      if (year > 0)
        this.textBoxYear.Text = year.ToString();
      if (indexRate > 0.0)
        this.textBoxIndex.Text = indexRate.ToString(string.Compare(this.decimalsUseForIndex, "FiveDecimals", true) == 0 ? "N5" : "N3");
      this.textBoxIndex.MaxLength = string.Compare(this.decimalsUseForIndex, "FiveDecimals", true) == 0 ? 8 : 6;
      if (!this.useNewHELOCHistoricTable)
      {
        if (marginRate > 0.0)
          this.textBoxMargin.Text = marginRate.ToString("N3");
        if (aprRate > 0.0)
          this.textBoxAPR.Text = aprRate.ToString("N3");
        if (monthlyPay > 0.0)
          this.textBoxMMP.Text = monthlyPay.ToString("N2");
        else
          this.refreshMonthlyPayment();
        if (periodType == "Repayment")
          this.comboBoxPeriod.Text = "Repayment";
        else
          this.comboBoxPeriod.Text = "Draw";
      }
      else
      {
        this.Text = "HELOC Year and Index";
        this.label2.Top = this.label6.Top = this.label9.Top;
        this.textBoxIndex.Top = this.comboBoxPeriod.Top;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    internal int Year => this.year;

    internal string PeriodType => this.periodType;

    internal double IndexRate => this.indexRate;

    internal double MarginRate => this.marginRate;

    internal double AprRate => this.aprRate;

    internal double MonthlyPay => this.monthlyPay;

    private void InitializeComponent()
    {
      this.textBoxYear = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.textBoxIndex = new TextBox();
      this.label3 = new Label();
      this.textBoxMargin = new TextBox();
      this.label4 = new Label();
      this.textBoxAPR = new TextBox();
      this.label5 = new Label();
      this.textBoxMMP = new TextBox();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.groupBox1 = new GroupBox();
      this.label9 = new Label();
      this.comboBoxPeriod = new ComboBox();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.textBoxYear.Location = new Point(81, 22);
      this.textBoxYear.MaxLength = 4;
      this.textBoxYear.Name = "textBoxYear";
      this.textBoxYear.Size = new Size(59, 20);
      this.textBoxYear.TabIndex = 0;
      this.textBoxYear.TextAlign = HorizontalAlignment.Right;
      this.textBoxYear.KeyPress += new KeyPressEventHandler(this.digitOnly_keypress);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 25);
      this.label1.Name = "label1";
      this.label1.Size = new Size(29, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Year";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 74);
      this.label2.Name = "label2";
      this.label2.Size = new Size(33, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Index";
      this.textBoxIndex.Location = new Point(81, 72);
      this.textBoxIndex.MaxLength = 6;
      this.textBoxIndex.Name = "textBoxIndex";
      this.textBoxIndex.Size = new Size(59, 20);
      this.textBoxIndex.TabIndex = 2;
      this.textBoxIndex.TextAlign = HorizontalAlignment.Right;
      this.textBoxIndex.TextChanged += new EventHandler(this.rate_TextChanged);
      this.textBoxIndex.Leave += new EventHandler(this.textBox_Leave);
      this.textBoxIndex.KeyUp += new KeyEventHandler(this.textBox_KeyUp);
      this.textBoxIndex.KeyPress += new KeyPressEventHandler(this.digitOnly_keypress);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(8, 98);
      this.label3.Name = "label3";
      this.label3.Size = new Size(39, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Margin";
      this.textBoxMargin.Location = new Point(81, 96);
      this.textBoxMargin.MaxLength = 6;
      this.textBoxMargin.Name = "textBoxMargin";
      this.textBoxMargin.Size = new Size(59, 20);
      this.textBoxMargin.TabIndex = 3;
      this.textBoxMargin.TextAlign = HorizontalAlignment.Right;
      this.textBoxMargin.TextChanged += new EventHandler(this.rate_TextChanged);
      this.textBoxMargin.Leave += new EventHandler(this.textBox_Leave);
      this.textBoxMargin.KeyUp += new KeyEventHandler(this.textBox_KeyUp);
      this.textBoxMargin.KeyPress += new KeyPressEventHandler(this.digitOnly_keypress);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(8, 123);
      this.label4.Name = "label4";
      this.label4.Size = new Size(29, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "APR";
      this.textBoxAPR.Location = new Point(81, 120);
      this.textBoxAPR.MaxLength = 6;
      this.textBoxAPR.Name = "textBoxAPR";
      this.textBoxAPR.Size = new Size(59, 20);
      this.textBoxAPR.TabIndex = 4;
      this.textBoxAPR.TextAlign = HorizontalAlignment.Right;
      this.textBoxAPR.Leave += new EventHandler(this.textBox_Leave);
      this.textBoxAPR.KeyUp += new KeyEventHandler(this.textBox_KeyUp);
      this.textBoxAPR.KeyPress += new KeyPressEventHandler(this.digitOnly_keypress);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(16, 176);
      this.label5.Name = "label5";
      this.label5.Size = new Size(118, 13);
      this.label5.TabIndex = 9;
      this.label5.Text = "* Min. Monthly Payment";
      this.textBoxMMP.Location = new Point(143, 172);
      this.textBoxMMP.Name = "textBoxMMP";
      this.textBoxMMP.Size = new Size(96, 20);
      this.textBoxMMP.TabIndex = 8;
      this.textBoxMMP.TabStop = false;
      this.textBoxMMP.TextAlign = HorizontalAlignment.Right;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(145, 75);
      this.label6.Name = "label6";
      this.label6.Size = new Size(15, 13);
      this.label6.TabIndex = 10;
      this.label6.Text = "%";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(145, 99);
      this.label7.Name = "label7";
      this.label7.Size = new Size(15, 13);
      this.label7.TabIndex = 11;
      this.label7.Text = "%";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(145, 123);
      this.label8.Name = "label8";
      this.label8.Size = new Size(15, 13);
      this.label8.TabIndex = 12;
      this.label8.Text = "%";
      this.groupBox1.Controls.Add((Control) this.label9);
      this.groupBox1.Controls.Add((Control) this.comboBoxPeriod);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.label8);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.label6);
      this.groupBox1.Controls.Add((Control) this.textBoxIndex);
      this.groupBox1.Controls.Add((Control) this.textBoxAPR);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.label7);
      this.groupBox1.Controls.Add((Control) this.textBoxMargin);
      this.groupBox1.Controls.Add((Control) this.textBoxYear);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Location = new Point(16, 8);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(223, 156);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(8, 51);
      this.label9.Name = "label9";
      this.label9.Size = new Size(64, 13);
      this.label9.TabIndex = 14;
      this.label9.Text = "Period Type";
      this.comboBoxPeriod.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxPeriod.Items.AddRange(new object[2]
      {
        (object) "Draw",
        (object) "Repayment"
      });
      this.comboBoxPeriod.Location = new Point(80, 47);
      this.comboBoxPeriod.Name = "comboBoxPeriod";
      this.comboBoxPeriod.Size = new Size(100, 21);
      this.comboBoxPeriod.TabIndex = 1;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.cancelBtn.Location = new Point(166, 202);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 6;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.okBtn.Location = new Point(88, 202);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 5;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(260, 232);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.textBoxMMP);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MMPFormDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Minimum Monthly Payment";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      this.year = Utils.ParseInt((object) this.textBoxYear.Text.Trim());
      if (this.textBoxYear.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Year cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxYear.Focus();
      }
      else if (this.year == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Year cannot be zero.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxYear.Focus();
      }
      else if (this.currentYear != this.year && this.existingYears.Contains((object) this.textBoxYear.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The HELOC Table already contains year '" + this.textBoxYear.Text.Trim() + "'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxYear.Focus();
      }
      else
      {
        this.periodType = this.comboBoxPeriod.Text;
        this.indexRate = (double) Utils.ParseDecimal((object) this.textBoxIndex.Text.Trim());
        this.marginRate = (double) Utils.ParseDecimal((object) this.textBoxMargin.Text.Trim());
        this.aprRate = (double) Utils.ParseDecimal((object) this.textBoxAPR.Text.Trim());
        this.monthlyPay = this.indexRate != 0.0 || this.marginRate != 0.0 ? Utils.CalcMonthlyPayment(this.indexRate + this.marginRate, 360, 10000.0) : (double) Utils.ParseDecimal((object) this.textBoxMMP.Text.Trim());
        this.DialogResult = DialogResult.OK;
      }
    }

    private void digitOnly_keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!char.IsDigit(e.KeyChar))
      {
        char keyChar = e.KeyChar;
        if (!keyChar.Equals('.'))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('-'))
          {
            e.Handled = true;
            return;
          }
        }
      }
      e.Handled = false;
    }

    private void textBox_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      double num = (double) Utils.ParseDecimal((object) textBox.Text.Trim());
      if (textBox.Name == "textBoxIndex")
        textBox.Text = num.ToString(string.Compare(this.decimalsUseForIndex, "FiveDecimals", true) == 0 ? "N5" : "N3");
      else
        textBox.Text = num.ToString("N3");
      this.refreshMonthlyPayment();
    }

    private void textBox_KeyUp(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.DECIMAL_3;
      TextBox textBox = (TextBox) sender;
      if (textBox.Name == "textBoxIndex" && string.Compare(this.decimalsUseForIndex, "FiveDecimals", true) == 0)
        dataFormat = FieldFormat.DECIMAL_5;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (needsUpdate)
      {
        textBox.Text = str;
        textBox.SelectionStart = str.Length;
      }
      this.refreshMonthlyPayment();
    }

    private void refreshMonthlyPayment()
    {
      double num1 = (double) Utils.ParseDecimal((object) this.textBoxIndex.Text.Trim());
      double num2 = (double) Utils.ParseDecimal((object) this.textBoxMargin.Text.Trim());
      if (num1 != 0.0 || num2 != 0.0)
      {
        this.textBoxMMP.ReadOnly = true;
        this.textBoxMMP.Text = Utils.CalcMonthlyPayment(num1 + num2, 360, 10000.0).ToString("N2");
      }
      else
        this.textBoxMMP.ReadOnly = false;
    }

    private void rate_TextChanged(object sender, EventArgs e)
    {
      this.textBoxAPR.Text = ((double) Utils.ParseDecimal((object) this.textBoxIndex.Text.Trim()) + (double) Utils.ParseDecimal((object) this.textBoxMargin.Text.Trim())).ToString("N3");
    }
  }
}

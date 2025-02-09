// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.BorIncomeDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class BorIncomeDialog : Form
  {
    private Button cancelBtn;
    private Button okBtn;
    private CheckBox copyVOEChk;
    private ComboBox borFreqCombo;
    private TextBox borIncTxt;
    private TextBox cobIncTxt;
    private ComboBox cobFreqCombo;
    private TextBox borMonthlyTxt;
    private TextBox cobMonthlyTxt;
    private TextBox borHoursTxt;
    private TextBox cobHoursTxt;
    private IContainer components;
    private ToolTip fieldToolTip;
    private GroupContainer groupContainer1;
    private Label label11;
    private Label label8;
    private Label label7;
    private Label label13;
    private Label label14;
    private Label label15;
    private Label label16;
    private LoanData loan;
    private bool copyVOE;
    private double borBaseIncome;
    private double borOverTime;
    private double borBonus;
    private double borCommissions;
    private double borOthers;
    private double cobBaseIncome;
    private double cobOverTime;
    private double cobBonus;
    private double cobCommissions;
    private double cobOthers;

    internal BorIncomeDialog(LoanData loan)
    {
      this.loan = loan;
      this.InitializeComponent();
      string simpleField1 = loan.GetSimpleField("INCOMEFREQ1");
      if (simpleField1 == string.Empty || simpleField1 == null)
        this.borFreqCombo.SelectedIndex = 0;
      else
        this.borFreqCombo.Text = simpleField1;
      this.borHoursTxt.Text = loan.GetSimpleField("HRSPERWEEK1").Trim();
      this.borIncTxt.Text = loan.GetSimpleField("INCOMEAMT1").Trim();
      this.borMonthlyTxt.Text = loan.GetSimpleField("MONTHLYINCOME1").Trim();
      string simpleField2 = loan.GetSimpleField("INCOMEFREQ2");
      if (simpleField2 == string.Empty || simpleField2 == null)
        this.cobFreqCombo.SelectedIndex = 0;
      else
        this.cobFreqCombo.Text = simpleField2;
      this.cobHoursTxt.Text = loan.GetSimpleField("HRSPERWEEK2").Trim();
      this.cobIncTxt.Text = loan.GetSimpleField("INCOMEAMT2").Trim();
      this.cobMonthlyTxt.Text = loan.GetSimpleField("MONTHLYINCOME2").Trim();
      this.fieldToolTip.SetToolTip((Control) this.borFreqCombo, "INCOMEFREQ1");
      this.fieldToolTip.SetToolTip((Control) this.borHoursTxt, "HRSPERWEEK1");
      this.fieldToolTip.SetToolTip((Control) this.borIncTxt, "INCOMEAMT1");
      this.fieldToolTip.SetToolTip((Control) this.borMonthlyTxt, "MONTHLYINCOME1");
      this.fieldToolTip.SetToolTip((Control) this.cobFreqCombo, "INCOMEFREQ2");
      this.fieldToolTip.SetToolTip((Control) this.cobHoursTxt, "HRSPERWEEK2");
      this.fieldToolTip.SetToolTip((Control) this.cobIncTxt, "INCOMEAMT2");
      this.fieldToolTip.SetToolTip((Control) this.cobMonthlyTxt, "MONTHLYINCOME2");
      this.copyVOE = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.copyVOEChk = new CheckBox();
      this.cobHoursTxt = new TextBox();
      this.borHoursTxt = new TextBox();
      this.cobMonthlyTxt = new TextBox();
      this.borMonthlyTxt = new TextBox();
      this.cobIncTxt = new TextBox();
      this.borIncTxt = new TextBox();
      this.cobFreqCombo = new ComboBox();
      this.borFreqCombo = new ComboBox();
      this.fieldToolTip = new ToolTip(this.components);
      this.groupContainer1 = new GroupContainer();
      this.label11 = new Label();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label13 = new Label();
      this.label14 = new Label();
      this.label15 = new Label();
      this.label16 = new Label();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(317, 203);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 10;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(236, 203);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 9;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.copyVOEChk.Location = new Point(9, 29);
      this.copyVOEChk.Name = "copyVOEChk";
      this.copyVOEChk.Size = new Size(321, 23);
      this.copyVOEChk.TabIndex = 0;
      this.copyVOEChk.Text = "Copy from present job in VOE";
      this.copyVOEChk.Click += new EventHandler(this.copyVOEChk_Click);
      this.copyVOEChk.Leave += new EventHandler(this.leave);
      this.cobHoursTxt.BorderStyle = BorderStyle.FixedSingle;
      this.cobHoursTxt.Location = new Point(267, 99);
      this.cobHoursTxt.Name = "cobHoursTxt";
      this.cobHoursTxt.ReadOnly = true;
      this.cobHoursTxt.Size = new Size(102, 20);
      this.cobHoursTxt.TabIndex = 6;
      this.cobHoursTxt.TabStop = false;
      this.cobHoursTxt.TextAlign = HorizontalAlignment.Right;
      this.cobHoursTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.cobHoursTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.cobHoursTxt.Leave += new EventHandler(this.leave);
      this.borHoursTxt.BorderStyle = BorderStyle.FixedSingle;
      this.borHoursTxt.Location = new Point(146, 99);
      this.borHoursTxt.Name = "borHoursTxt";
      this.borHoursTxt.ReadOnly = true;
      this.borHoursTxt.Size = new Size(101, 20);
      this.borHoursTxt.TabIndex = 2;
      this.borHoursTxt.TabStop = false;
      this.borHoursTxt.TextAlign = HorizontalAlignment.Right;
      this.borHoursTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.borHoursTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.borHoursTxt.Leave += new EventHandler(this.leave);
      this.cobMonthlyTxt.BorderStyle = BorderStyle.FixedSingle;
      this.cobMonthlyTxt.Location = new Point(267, 150);
      this.cobMonthlyTxt.Name = "cobMonthlyTxt";
      this.cobMonthlyTxt.ReadOnly = true;
      this.cobMonthlyTxt.Size = new Size(102, 20);
      this.cobMonthlyTxt.TabIndex = 8;
      this.cobMonthlyTxt.TabStop = false;
      this.cobMonthlyTxt.TextAlign = HorizontalAlignment.Right;
      this.borMonthlyTxt.BorderStyle = BorderStyle.FixedSingle;
      this.borMonthlyTxt.Location = new Point(146, 150);
      this.borMonthlyTxt.Name = "borMonthlyTxt";
      this.borMonthlyTxt.ReadOnly = true;
      this.borMonthlyTxt.Size = new Size(101, 20);
      this.borMonthlyTxt.TabIndex = 4;
      this.borMonthlyTxt.TabStop = false;
      this.borMonthlyTxt.TextAlign = HorizontalAlignment.Right;
      this.cobIncTxt.BorderStyle = BorderStyle.FixedSingle;
      this.cobIncTxt.Location = new Point(267, 125);
      this.cobIncTxt.Name = "cobIncTxt";
      this.cobIncTxt.Size = new Size(102, 20);
      this.cobIncTxt.TabIndex = 7;
      this.cobIncTxt.TextAlign = HorizontalAlignment.Right;
      this.cobIncTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.cobIncTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.cobIncTxt.Leave += new EventHandler(this.leave);
      this.borIncTxt.BorderStyle = BorderStyle.FixedSingle;
      this.borIncTxt.Location = new Point(146, 125);
      this.borIncTxt.Name = "borIncTxt";
      this.borIncTxt.Size = new Size(101, 20);
      this.borIncTxt.TabIndex = 3;
      this.borIncTxt.TextAlign = HorizontalAlignment.Right;
      this.borIncTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.borIncTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.borIncTxt.Leave += new EventHandler(this.leave);
      this.cobFreqCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cobFreqCombo.Items.AddRange(new object[6]
      {
        (object) "Yearly",
        (object) "Monthly",
        (object) "Semi Monthly",
        (object) "Bi-Weekly",
        (object) "Weekly",
        (object) "Hourly"
      });
      this.cobFreqCombo.Location = new Point(267, 73);
      this.cobFreqCombo.Name = "cobFreqCombo";
      this.cobFreqCombo.Size = new Size(102, 21);
      this.cobFreqCombo.TabIndex = 5;
      this.cobFreqCombo.SelectedIndexChanged += new EventHandler(this.cobFreqCombo_SelectedIndexChanged);
      this.cobFreqCombo.Leave += new EventHandler(this.leave);
      this.borFreqCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.borFreqCombo.Items.AddRange(new object[6]
      {
        (object) "Yearly",
        (object) "Monthly",
        (object) "Semi Monthly",
        (object) "Bi-Weekly",
        (object) "Weekly",
        (object) "Hourly"
      });
      this.borFreqCombo.Location = new Point(146, 73);
      this.borFreqCombo.Name = "borFreqCombo";
      this.borFreqCombo.Size = new Size(101, 21);
      this.borFreqCombo.TabIndex = 1;
      this.borFreqCombo.SelectedIndexChanged += new EventHandler(this.borFreqCombo_SelectedIndexChanged);
      this.borFreqCombo.Leave += new EventHandler(this.leave);
      this.groupContainer1.BackColor = Color.White;
      this.groupContainer1.Controls.Add((Control) this.cobHoursTxt);
      this.groupContainer1.Controls.Add((Control) this.copyVOEChk);
      this.groupContainer1.Controls.Add((Control) this.borHoursTxt);
      this.groupContainer1.Controls.Add((Control) this.label11);
      this.groupContainer1.Controls.Add((Control) this.cobMonthlyTxt);
      this.groupContainer1.Controls.Add((Control) this.label8);
      this.groupContainer1.Controls.Add((Control) this.borMonthlyTxt);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.cobIncTxt);
      this.groupContainer1.Controls.Add((Control) this.label13);
      this.groupContainer1.Controls.Add((Control) this.borIncTxt);
      this.groupContainer1.Controls.Add((Control) this.label14);
      this.groupContainer1.Controls.Add((Control) this.label15);
      this.groupContainer1.Controls.Add((Control) this.label16);
      this.groupContainer1.Controls.Add((Control) this.borFreqCombo);
      this.groupContainer1.Controls.Add((Control) this.cobFreqCombo);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(12, 12);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(380, 185);
      this.groupContainer1.TabIndex = 11;
      this.groupContainer1.Text = "Base Income";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(7, 121);
      this.label11.Name = "label11";
      this.label11.Size = new Size(81, 13);
      this.label11.TabIndex = 16;
      this.label11.Text = "Income Amount";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(7, 144);
      this.label8.Name = "label8";
      this.label8.Size = new Size(82, 13);
      this.label8.TabIndex = 8;
      this.label8.Text = "Monthly Income";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(7, 98);
      this.label7.Name = "label7";
      this.label7.Size = new Size(86, 13);
      this.label7.TabIndex = 7;
      this.label7.Text = "Hours Per Week";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(6, 76);
      this.label13.Name = "label13";
      this.label13.Size = new Size(57, 13);
      this.label13.TabIndex = 4;
      this.label13.Text = "Frequency";
      this.label14.AutoSize = true;
      this.label14.Location = new Point(263, 57);
      this.label14.Name = "label14";
      this.label14.Size = new Size(65, 13);
      this.label14.TabIndex = 3;
      this.label14.Text = "Co-Borrower";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(143, 57);
      this.label15.Name = "label15";
      this.label15.Size = new Size(49, 13);
      this.label15.TabIndex = 2;
      this.label15.Text = "Borrower";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(6, 57);
      this.label16.Name = "label16";
      this.label16.Size = new Size(87, 13);
      this.label16.TabIndex = 0;
      this.label16.Text = "Base Income For";
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(403, 241);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (BorIncomeDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Calculate Base Income";
      this.KeyPress += new KeyPressEventHandler(this.BorIncomeDialog_KeyPress);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }

    internal bool CopyVOE => this.copyVOE;

    internal double BorBaseIncome => this.borBaseIncome;

    internal double BorOverTime => this.borOverTime;

    internal double BorBonus => this.borBonus;

    internal double BorCommissions => this.borCommissions;

    internal double BorOthers => this.borOthers;

    internal double CobBaseIncome => this.cobBaseIncome;

    internal double CobOverTime => this.cobOverTime;

    internal double CobBonus => this.cobBonus;

    internal double CobCommissions => this.cobCommissions;

    internal double CobOthers => this.cobOthers;

    private void leave(object sender, EventArgs e)
    {
      if (sender != null && sender is TextBox)
      {
        TextBox textBox = (TextBox) sender;
        if (textBox.Name == "borIncTxt" || textBox.Name == "cobIncTxt")
        {
          double num = this.DoubleValue(textBox.Text);
          if (num != 0.0)
            textBox.Text = num.ToString("N2");
        }
      }
      this.CalculateBaseIncome();
    }

    private void CalculateBaseIncome()
    {
      if (this.copyVOEChk.Checked)
      {
        this.borBaseIncome = 0.0;
        this.borOverTime = 0.0;
        this.borBonus = 0.0;
        this.borCommissions = 0.0;
        this.borOthers = 0.0;
        string empty = string.Empty;
        int numberOfEmployer1 = this.loan.GetNumberOfEmployer(true);
        for (int index = 1; index <= numberOfEmployer1; ++index)
        {
          if (this.loan.GetField("BE" + index.ToString("00") + "09") == "Y")
          {
            string field = this.loan.GetField("BE" + index.ToString("00") + "15");
            double num = this.DoubleValue(this.loan.GetField("BE" + index.ToString("00") + "27")) / 100.0;
            if (num == 0.0 || field != "Y")
              num = 1.0;
            this.borBaseIncome += this.DoubleValue(this.loan.GetField("BE" + index.ToString("00") + "19")) * num;
            this.borOverTime += this.DoubleValue(this.loan.GetField("BE" + index.ToString("00") + "20")) * num;
            this.borBonus += this.DoubleValue(this.loan.GetField("BE" + index.ToString("00") + "21")) * num;
            this.borCommissions += this.DoubleValue(this.loan.GetField("BE" + index.ToString("00") + "22")) * num;
            this.borOthers += this.DoubleValue(this.loan.GetField("BE" + index.ToString("00") + "23")) * num;
          }
        }
        this.cobBaseIncome = 0.0;
        this.cobOverTime = 0.0;
        this.cobBonus = 0.0;
        this.cobCommissions = 0.0;
        this.cobOthers = 0.0;
        int numberOfEmployer2 = this.loan.GetNumberOfEmployer(false);
        for (int index = 1; index <= numberOfEmployer2; ++index)
        {
          if (this.loan.GetField("CE" + index.ToString("00") + "09") == "Y")
          {
            string field = this.loan.GetField("CE" + index.ToString("00") + "15");
            double num = this.DoubleValue(this.loan.GetField("CE" + index.ToString("00") + "27")) / 100.0;
            if (num == 0.0 || field != "Y")
              num = 1.0;
            this.cobBaseIncome += this.DoubleValue(this.loan.GetField("CE" + index.ToString("00") + "19")) * num;
            this.cobOverTime += this.DoubleValue(this.loan.GetField("CE" + index.ToString("00") + "20")) * num;
            this.cobBonus += this.DoubleValue(this.loan.GetField("CE" + index.ToString("00") + "21")) * num;
            this.cobCommissions += this.DoubleValue(this.loan.GetField("CE" + index.ToString("00") + "22")) * num;
            this.cobOthers += this.DoubleValue(this.loan.GetField("CE" + index.ToString("00") + "23")) * num;
          }
        }
      }
      else
      {
        this.borBaseIncome = this.CalculateMonthlyIncome(this.borFreqCombo.SelectedIndex, this.DoubleValue(this.borHoursTxt.Text), this.DoubleValue(this.borIncTxt.Text));
        this.borMonthlyTxt.Text = this.borBaseIncome.ToString("N2");
        this.cobBaseIncome = this.CalculateMonthlyIncome(this.cobFreqCombo.SelectedIndex, this.DoubleValue(this.cobHoursTxt.Text), this.DoubleValue(this.cobIncTxt.Text));
        this.cobMonthlyTxt.Text = this.cobBaseIncome.ToString("N2");
      }
    }

    private double CalculateMonthlyIncome(int freqPay, double hourly, double incAmount)
    {
      double num = 0.0;
      switch (freqPay)
      {
        case 0:
          num = incAmount / 12.0;
          break;
        case 1:
          num = incAmount;
          break;
        case 2:
          num = incAmount * 2.0;
          break;
        case 3:
          num = incAmount * 26.0 / 12.0;
          break;
        case 4:
          num = incAmount * 52.0 / 12.0;
          break;
        case 5:
          num = incAmount * hourly * 52.0 / 12.0;
          break;
      }
      return Math.Round(num, 2);
    }

    private double DoubleValue(string strValue)
    {
      return strValue == string.Empty || strValue == null ? 0.0 : double.Parse(strValue.Replace(",", string.Empty));
    }

    private void keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar) || e.KeyChar.Equals('.'))
        e.Handled = false;
      else
        e.Handled = true;
    }

    private void borFreqCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.borFreqCombo.SelectedItem.ToString() == "Hourly")
      {
        this.borHoursTxt.ReadOnly = false;
      }
      else
      {
        this.borHoursTxt.ReadOnly = true;
        this.borHoursTxt.Text = "";
      }
      this.leave((object) null, (EventArgs) null);
    }

    private void cobFreqCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cobFreqCombo.SelectedItem.ToString() == "Hourly")
      {
        this.cobHoursTxt.ReadOnly = false;
      }
      else
      {
        this.cobHoursTxt.ReadOnly = true;
        this.cobHoursTxt.Text = "";
      }
      this.leave((object) null, (EventArgs) null);
    }

    private void copyVOEChk_Click(object sender, EventArgs e)
    {
      if (this.copyVOEChk.Checked)
      {
        this.borHoursTxt.Text = "";
        this.cobHoursTxt.Text = "";
        this.borIncTxt.Text = "";
        this.cobIncTxt.Text = "";
        this.borMonthlyTxt.Text = "";
        this.cobMonthlyTxt.Text = "";
        this.copyVOE = true;
      }
      else
        this.copyVOE = false;
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      this.loan.SetCurrentField("INCOMEFREQ1", this.borFreqCombo.Text);
      this.loan.SetCurrentField("HRSPERWEEK1", this.borHoursTxt.Text);
      this.loan.SetCurrentField("INCOMEAMT1", this.borIncTxt.Text);
      this.loan.SetCurrentField("MONTHLYINCOME1", this.borMonthlyTxt.Text);
      this.loan.SetCurrentField("INCOMEFREQ2", this.cobFreqCombo.Text);
      this.loan.SetCurrentField("HRSPERWEEK2", this.cobHoursTxt.Text);
      this.loan.SetCurrentField("INCOMEAMT2", this.cobIncTxt.Text);
      this.loan.SetCurrentField("MONTHLYINCOME2", this.cobMonthlyTxt.Text);
    }

    private void keyup(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.DECIMAL_2;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void BorIncomeDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.Close();
    }
  }
}

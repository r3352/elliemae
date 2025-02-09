// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MonthlyIncomeDialog
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
  internal class MonthlyIncomeDialog : Form
  {
    private Button cancelBtn;
    private Button okBtn;
    private ComboBox borFreqCombo;
    private TextBox borIncTxt;
    private TextBox borMonthlyTxt;
    private TextBox borHoursTxt;
    private IContainer components;
    private ToolTip fieldToolTip;
    private GroupContainer groupContainer1;
    private Label label11;
    private Label label8;
    private Label label7;
    private Label label13;
    private Label lblIncomeFor;
    private Label label16;
    private LoanData loan;
    private string fieldInd;
    private double borBaseIncome;

    internal MonthlyIncomeDialog(LoanData loan, string fieldInd)
    {
      this.loan = loan;
      this.fieldInd = fieldInd;
      this.InitializeComponent();
      this.borFreqCombo.SelectedIndex = 1;
      this.borMonthlyTxt.Text = loan.GetSimpleField(fieldInd + "19").Trim();
      this.fieldToolTip.SetToolTip((Control) this.borFreqCombo, "INCOMEFREQ1");
      this.fieldToolTip.SetToolTip((Control) this.borHoursTxt, "HRSPERWEEK1");
      this.fieldToolTip.SetToolTip((Control) this.borIncTxt, "INCOMEAMT1");
      this.fieldToolTip.SetToolTip((Control) this.borMonthlyTxt, "MONTHLYINCOME1");
      if (fieldInd.ToUpper().StartsWith("BE") || fieldInd.ToUpper().StartsWith("FE01") || fieldInd.ToUpper().StartsWith("FE03"))
        this.lblIncomeFor.Text = "Borrower";
      else
        this.lblIncomeFor.Text = "Co-Borrower";
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
      this.borHoursTxt = new TextBox();
      this.borMonthlyTxt = new TextBox();
      this.borIncTxt = new TextBox();
      this.borFreqCombo = new ComboBox();
      this.fieldToolTip = new ToolTip(this.components);
      this.groupContainer1 = new GroupContainer();
      this.label11 = new Label();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label13 = new Label();
      this.lblIncomeFor = new Label();
      this.label16 = new Label();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(412, 321);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(150, 44);
      this.cancelBtn.TabIndex = 10;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(250, 321);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(150, 44);
      this.okBtn.TabIndex = 9;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.borHoursTxt.BorderStyle = BorderStyle.FixedSingle;
      this.borHoursTxt.Location = new Point(296, 134);
      this.borHoursTxt.Name = "borHoursTxt";
      this.borHoursTxt.ReadOnly = true;
      this.borHoursTxt.Size = new Size(202, 31);
      this.borHoursTxt.TabIndex = 2;
      this.borHoursTxt.TabStop = false;
      this.borHoursTxt.TextAlign = HorizontalAlignment.Right;
      this.borHoursTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.borHoursTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.borHoursTxt.Leave += new EventHandler(this.leave);
      this.borMonthlyTxt.BorderStyle = BorderStyle.FixedSingle;
      this.borMonthlyTxt.Location = new Point(296, 226);
      this.borMonthlyTxt.Name = "borMonthlyTxt";
      this.borMonthlyTxt.ReadOnly = true;
      this.borMonthlyTxt.Size = new Size(202, 31);
      this.borMonthlyTxt.TabIndex = 4;
      this.borMonthlyTxt.TabStop = false;
      this.borMonthlyTxt.TextAlign = HorizontalAlignment.Right;
      this.borIncTxt.BorderStyle = BorderStyle.FixedSingle;
      this.borIncTxt.Location = new Point(296, 180);
      this.borIncTxt.Name = "borIncTxt";
      this.borIncTxt.Size = new Size(202, 31);
      this.borIncTxt.TabIndex = 3;
      this.borIncTxt.TextAlign = HorizontalAlignment.Right;
      this.borIncTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.borIncTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.borIncTxt.Leave += new EventHandler(this.leave);
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
      this.borFreqCombo.Location = new Point(296, 86);
      this.borFreqCombo.Name = "borFreqCombo";
      this.borFreqCombo.Size = new Size(202, 33);
      this.borFreqCombo.TabIndex = 1;
      this.borFreqCombo.SelectedIndexChanged += new EventHandler(this.borFreqCombo_SelectedIndexChanged);
      this.borFreqCombo.Leave += new EventHandler(this.leave);
      this.groupContainer1.BackColor = Color.White;
      this.groupContainer1.Controls.Add((Control) this.borHoursTxt);
      this.groupContainer1.Controls.Add((Control) this.label11);
      this.groupContainer1.Controls.Add((Control) this.label8);
      this.groupContainer1.Controls.Add((Control) this.borMonthlyTxt);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.label13);
      this.groupContainer1.Controls.Add((Control) this.borIncTxt);
      this.groupContainer1.Controls.Add((Control) this.lblIncomeFor);
      this.groupContainer1.Controls.Add((Control) this.label16);
      this.groupContainer1.Controls.Add((Control) this.borFreqCombo);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(24, 22);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(538, 283);
      this.groupContainer1.TabIndex = 11;
      this.groupContainer1.Text = "Base Income";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(16, 180);
      this.label11.Name = "label11";
      this.label11.Size = new Size(160, 25);
      this.label11.TabIndex = 16;
      this.label11.Text = "Income Amount";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(16, 226);
      this.label8.Name = "label8";
      this.label8.Size = new Size(163, 25);
      this.label8.TabIndex = 8;
      this.label8.Text = "Monthly Income";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(16, 134);
      this.label7.Name = "label7";
      this.label7.Size = new Size(169, 25);
      this.label7.TabIndex = 7;
      this.label7.Text = "Hours Per Week";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(16, 86);
      this.label13.Name = "label13";
      this.label13.Size = new Size(114, 25);
      this.label13.TabIndex = 4;
      this.label13.Text = "Frequency";
      this.lblIncomeFor.AutoSize = true;
      this.lblIncomeFor.Location = new Point(296, 46);
      this.lblIncomeFor.Name = "lblIncomeFor";
      this.lblIncomeFor.Size = new Size(98, 25);
      this.lblIncomeFor.TabIndex = 2;
      this.lblIncomeFor.Text = "Borrower";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(16, 46);
      this.label16.Name = "label16";
      this.label16.Size = new Size(174, 25);
      this.label16.TabIndex = 0;
      this.label16.Text = "Base Income For";
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(10, 24);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(585, 380);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MonthlyIncomeDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Calculate Base Income";
      this.KeyPress += new KeyPressEventHandler(this.BorIncomeDialog_KeyPress);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }

    internal double BorBaseIncome => this.borBaseIncome;

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
      this.borBaseIncome = this.CalculateMonthlyIncome(this.borFreqCombo.SelectedIndex, this.DoubleValue(this.borHoursTxt.Text), this.DoubleValue(this.borIncTxt.Text));
      this.borMonthlyTxt.Text = this.borBaseIncome.ToString("N2");
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

    private void okBtn_Click(object sender, EventArgs e)
    {
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

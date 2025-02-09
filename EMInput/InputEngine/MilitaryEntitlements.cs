// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MilitaryEntitlements
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class MilitaryEntitlements : Form
  {
    private Button cancelBtn;
    private Button okBtn;
    private IContainer components;
    private ToolTip fieldToolTip;
    private GroupContainer groupContainer1;
    private Label label11;
    private Label label7;
    private Label label13;
    private Label label1;
    private Label label21;
    private Label label19;
    private Label label18;
    private Label label17;
    private Label label16;
    private Label label14;
    private Label label6;
    private Label label2;
    private TextBox textBox12;
    private TextBox textBox11;
    private TextBox textBox10;
    private TextBox textBox9;
    private TextBox textBox8;
    private TextBox textBox7;
    private TextBox textBox5;
    private TextBox textBox4;
    private TextBox textBox3;
    private TextBox textBox2;
    private TextBox textBox1;
    private TextBox textBox13;
    private Label label3;
    private TextBox textBox14;
    private LoanData loan;
    private double militaryEntitlementsTotal;
    private double baseIncome;
    private EMHelpLink emHelpLink1;
    private string fieldInd;

    internal double MilitaryEntitlementsTotal => this.militaryEntitlementsTotal;

    internal double BaseIncome => this.baseIncome;

    internal MilitaryEntitlements(LoanData loan, string fieldInd)
    {
      this.loan = loan;
      this.fieldInd = fieldInd;
      this.InitializeComponent();
      this.textBox1.Text = loan.GetSimpleField(fieldInd + "19").Trim();
      this.textBox2.Text = loan.GetSimpleField(fieldInd + "77").Trim();
      this.textBox3.Text = loan.GetSimpleField(fieldInd + "65").Trim();
      this.textBox4.Text = loan.GetSimpleField(fieldInd + "66").Trim();
      this.textBox5.Text = loan.GetSimpleField(fieldInd + "67").Trim();
      this.textBox7.Text = loan.GetSimpleField(fieldInd + "68").Trim();
      this.textBox8.Text = loan.GetSimpleField(fieldInd + "69").Trim();
      this.textBox9.Text = loan.GetSimpleField(fieldInd + "70").Trim();
      this.textBox10.Text = loan.GetSimpleField(fieldInd + "71").Trim();
      this.textBox11.Text = loan.GetSimpleField(fieldInd + "72").Trim();
      this.textBox13.Text = loan.GetSimpleField(fieldInd + "23").Trim();
      this.textBox12.Text = loan.GetSimpleField(fieldInd + "74").Trim();
      this.textBox14.Text = loan.GetSimpleField(fieldInd + "53").Trim();
      this.fieldToolTip.SetToolTip((Control) this.textBox1, fieldInd + "19");
      this.fieldToolTip.SetToolTip((Control) this.textBox2, fieldInd + "77");
      this.fieldToolTip.SetToolTip((Control) this.textBox3, fieldInd + "65");
      this.fieldToolTip.SetToolTip((Control) this.textBox4, fieldInd + "66");
      this.fieldToolTip.SetToolTip((Control) this.textBox5, fieldInd + "67");
      this.fieldToolTip.SetToolTip((Control) this.textBox7, fieldInd + "68");
      this.fieldToolTip.SetToolTip((Control) this.textBox8, fieldInd + "69");
      this.fieldToolTip.SetToolTip((Control) this.textBox9, fieldInd + "70");
      this.fieldToolTip.SetToolTip((Control) this.textBox10, fieldInd + "71");
      this.fieldToolTip.SetToolTip((Control) this.textBox11, fieldInd + "72");
      this.fieldToolTip.SetToolTip((Control) this.textBox12, fieldInd + "74");
      this.fieldToolTip.SetToolTip((Control) this.textBox13, fieldInd + "23");
      this.fieldToolTip.SetToolTip((Control) this.textBox14, fieldInd + "53");
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
      this.fieldToolTip = new ToolTip(this.components);
      this.groupContainer1 = new GroupContainer();
      this.label21 = new Label();
      this.label19 = new Label();
      this.label18 = new Label();
      this.label17 = new Label();
      this.label16 = new Label();
      this.label14 = new Label();
      this.label3 = new Label();
      this.label11 = new Label();
      this.label7 = new Label();
      this.label6 = new Label();
      this.label2 = new Label();
      this.textBox14 = new TextBox();
      this.textBox13 = new TextBox();
      this.textBox12 = new TextBox();
      this.textBox11 = new TextBox();
      this.textBox10 = new TextBox();
      this.textBox9 = new TextBox();
      this.textBox8 = new TextBox();
      this.textBox7 = new TextBox();
      this.textBox5 = new TextBox();
      this.textBox4 = new TextBox();
      this.textBox3 = new TextBox();
      this.label13 = new Label();
      this.textBox2 = new TextBox();
      this.textBox1 = new TextBox();
      this.label1 = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(281, 404);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 17;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(194, 404);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 16;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.groupContainer1.BackColor = Color.White;
      this.groupContainer1.Controls.Add((Control) this.label21);
      this.groupContainer1.Controls.Add((Control) this.label19);
      this.groupContainer1.Controls.Add((Control) this.label18);
      this.groupContainer1.Controls.Add((Control) this.label17);
      this.groupContainer1.Controls.Add((Control) this.label16);
      this.groupContainer1.Controls.Add((Control) this.label14);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.label11);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.textBox14);
      this.groupContainer1.Controls.Add((Control) this.textBox13);
      this.groupContainer1.Controls.Add((Control) this.textBox12);
      this.groupContainer1.Controls.Add((Control) this.textBox11);
      this.groupContainer1.Controls.Add((Control) this.textBox10);
      this.groupContainer1.Controls.Add((Control) this.textBox9);
      this.groupContainer1.Controls.Add((Control) this.textBox8);
      this.groupContainer1.Controls.Add((Control) this.textBox7);
      this.groupContainer1.Controls.Add((Control) this.textBox5);
      this.groupContainer1.Controls.Add((Control) this.textBox4);
      this.groupContainer1.Controls.Add((Control) this.textBox3);
      this.groupContainer1.Controls.Add((Control) this.label13);
      this.groupContainer1.Controls.Add((Control) this.textBox2);
      this.groupContainer1.Controls.Add((Control) this.textBox1);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(12, 12);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(344, 348);
      this.groupContainer1.TabIndex = 11;
      this.groupContainer1.Text = "Military Entitlements";
      this.label21.AutoSize = true;
      this.label21.Location = new Point(7, 312);
      this.label21.Name = "label21";
      this.label21.Size = new Size(126, 13);
      this.label21.TabIndex = 16;
      this.label21.Text = "Total Military Entitlements";
      this.label19.AutoSize = true;
      this.label19.Location = new Point(7, 262);
      this.label19.Name = "label19";
      this.label19.Size = new Size(99, 13);
      this.label19.TabIndex = 16;
      this.label19.Text = "Quarters Allowance";
      this.label18.AutoSize = true;
      this.label18.Location = new Point(7, 237);
      this.label18.Name = "label18";
      this.label18.Size = new Size(139, 13);
      this.label18.TabIndex = 16;
      this.label18.Text = "Variable Housing Allowance";
      this.label17.AutoSize = true;
      this.label17.Location = new Point(7, 212);
      this.label17.Name = "label17";
      this.label17.Size = new Size(95, 13);
      this.label17.TabIndex = 16;
      this.label17.Text = "Rations Allowance";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(7, 187);
      this.label16.Name = "label16";
      this.label16.Size = new Size(97, 13);
      this.label16.TabIndex = 16;
      this.label16.Text = "Clothing Allowance";
      this.label14.AutoSize = true;
      this.label14.Location = new Point(7, 137);
      this.label14.Name = "label14";
      this.label14.Size = new Size(108, 13);
      this.label14.TabIndex = 16;
      this.label14.Text = "Military Overseas Pay";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(7, 112);
      this.label3.Name = "label3";
      this.label3.Size = new Size(97, 13);
      this.label3.TabIndex = 16;
      this.label3.Text = "Military Hazard Pay";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(7, 87);
      this.label11.Name = "label11";
      this.label11.Size = new Size(88, 13);
      this.label11.TabIndex = 16;
      this.label11.Text = "Military Flight Pay";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(7, 62);
      this.label7.Name = "label7";
      this.label7.Size = new Size(99, 13);
      this.label7.TabIndex = 7;
      this.label7.Text = "Military Combat Pay";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(7, 287);
      this.label6.Name = "label6";
      this.label6.Size = new Size(89, 13);
      this.label6.TabIndex = 4;
      this.label6.Text = "Other Allowance*";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 162);
      this.label2.Name = "label2";
      this.label2.Size = new Size(85, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Military Prop Pay";
      this.textBox14.BorderStyle = BorderStyle.FixedSingle;
      this.textBox14.Enabled = false;
      this.textBox14.Location = new Point(224, 312);
      this.textBox14.Name = "textBox14";
      this.textBox14.Size = new Size(101, 20);
      this.textBox14.TabIndex = 13;
      this.textBox14.TextAlign = HorizontalAlignment.Right;
      this.textBox14.KeyPress += new KeyPressEventHandler(this.keypress);
      this.textBox14.KeyUp += new KeyEventHandler(this.keyup);
      this.textBox14.Leave += new EventHandler(this.leave);
      this.textBox13.BorderStyle = BorderStyle.FixedSingle;
      this.textBox13.Location = new Point(224, 287);
      this.textBox13.Name = "textBox13";
      this.textBox13.Size = new Size(101, 20);
      this.textBox13.TabIndex = 12;
      this.textBox13.TextAlign = HorizontalAlignment.Right;
      this.textBox13.KeyPress += new KeyPressEventHandler(this.keypress);
      this.textBox13.KeyUp += new KeyEventHandler(this.keyup);
      this.textBox13.Leave += new EventHandler(this.leave);
      this.textBox12.BorderStyle = BorderStyle.FixedSingle;
      this.textBox12.Location = new Point(107, 287);
      this.textBox12.Name = "textBox12";
      this.textBox12.Size = new Size(101, 20);
      this.textBox12.TabIndex = 11;
      this.textBox11.BorderStyle = BorderStyle.FixedSingle;
      this.textBox11.Location = new Point(224, 262);
      this.textBox11.Name = "textBox11";
      this.textBox11.Size = new Size(101, 20);
      this.textBox11.TabIndex = 10;
      this.textBox11.TextAlign = HorizontalAlignment.Right;
      this.textBox11.KeyPress += new KeyPressEventHandler(this.keypress);
      this.textBox11.KeyUp += new KeyEventHandler(this.keyup);
      this.textBox11.Leave += new EventHandler(this.leave);
      this.textBox10.BorderStyle = BorderStyle.FixedSingle;
      this.textBox10.Location = new Point(224, 237);
      this.textBox10.Name = "textBox10";
      this.textBox10.Size = new Size(101, 20);
      this.textBox10.TabIndex = 9;
      this.textBox10.TextAlign = HorizontalAlignment.Right;
      this.textBox10.KeyPress += new KeyPressEventHandler(this.keypress);
      this.textBox10.KeyUp += new KeyEventHandler(this.keyup);
      this.textBox10.Leave += new EventHandler(this.leave);
      this.textBox9.BorderStyle = BorderStyle.FixedSingle;
      this.textBox9.Location = new Point(224, 212);
      this.textBox9.Name = "textBox9";
      this.textBox9.Size = new Size(101, 20);
      this.textBox9.TabIndex = 8;
      this.textBox9.TextAlign = HorizontalAlignment.Right;
      this.textBox9.KeyPress += new KeyPressEventHandler(this.keypress);
      this.textBox9.KeyUp += new KeyEventHandler(this.keyup);
      this.textBox9.Leave += new EventHandler(this.leave);
      this.textBox8.BorderStyle = BorderStyle.FixedSingle;
      this.textBox8.Location = new Point(224, 187);
      this.textBox8.Name = "textBox8";
      this.textBox8.Size = new Size(101, 20);
      this.textBox8.TabIndex = 7;
      this.textBox8.TextAlign = HorizontalAlignment.Right;
      this.textBox8.KeyPress += new KeyPressEventHandler(this.keypress);
      this.textBox8.KeyUp += new KeyEventHandler(this.keyup);
      this.textBox8.Leave += new EventHandler(this.leave);
      this.textBox7.BorderStyle = BorderStyle.FixedSingle;
      this.textBox7.Location = new Point(224, 162);
      this.textBox7.Name = "textBox7";
      this.textBox7.Size = new Size(101, 20);
      this.textBox7.TabIndex = 6;
      this.textBox7.TextAlign = HorizontalAlignment.Right;
      this.textBox7.KeyPress += new KeyPressEventHandler(this.keypress);
      this.textBox7.KeyUp += new KeyEventHandler(this.keyup);
      this.textBox7.Leave += new EventHandler(this.leave);
      this.textBox5.BorderStyle = BorderStyle.FixedSingle;
      this.textBox5.Location = new Point(224, 137);
      this.textBox5.Name = "textBox5";
      this.textBox5.Size = new Size(101, 20);
      this.textBox5.TabIndex = 5;
      this.textBox5.TextAlign = HorizontalAlignment.Right;
      this.textBox5.KeyPress += new KeyPressEventHandler(this.keypress);
      this.textBox5.KeyUp += new KeyEventHandler(this.keyup);
      this.textBox5.Leave += new EventHandler(this.leave);
      this.textBox4.BorderStyle = BorderStyle.FixedSingle;
      this.textBox4.Location = new Point(224, 112);
      this.textBox4.Name = "textBox4";
      this.textBox4.Size = new Size(101, 20);
      this.textBox4.TabIndex = 4;
      this.textBox4.TextAlign = HorizontalAlignment.Right;
      this.textBox4.KeyPress += new KeyPressEventHandler(this.keypress);
      this.textBox4.KeyUp += new KeyEventHandler(this.keyup);
      this.textBox4.Leave += new EventHandler(this.leave);
      this.textBox3.BorderStyle = BorderStyle.FixedSingle;
      this.textBox3.Location = new Point(224, 87);
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new Size(101, 20);
      this.textBox3.TabIndex = 3;
      this.textBox3.TextAlign = HorizontalAlignment.Right;
      this.textBox3.KeyPress += new KeyPressEventHandler(this.keypress);
      this.textBox3.KeyUp += new KeyEventHandler(this.keyup);
      this.textBox3.Leave += new EventHandler(this.leave);
      this.label13.AutoSize = true;
      this.label13.Location = new Point(7, 37);
      this.label13.Name = "label13";
      this.label13.Size = new Size(91, 13);
      this.label13.TabIndex = 4;
      this.label13.Text = "Military Base Pay*";
      this.textBox2.BorderStyle = BorderStyle.FixedSingle;
      this.textBox2.Location = new Point(224, 62);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new Size(101, 20);
      this.textBox2.TabIndex = 2;
      this.textBox2.TextAlign = HorizontalAlignment.Right;
      this.textBox2.KeyPress += new KeyPressEventHandler(this.keypress);
      this.textBox2.KeyUp += new KeyEventHandler(this.keyup);
      this.textBox2.Leave += new EventHandler(this.leave);
      this.textBox1.BorderStyle = BorderStyle.FixedSingle;
      this.textBox1.Location = new Point(224, 37);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(101, 20);
      this.textBox1.TabIndex = 1;
      this.textBox1.TextAlign = HorizontalAlignment.Right;
      this.textBox1.KeyPress += new KeyPressEventHandler(this.keypress);
      this.textBox1.KeyUp += new KeyEventHandler(this.keyup);
      this.textBox1.Leave += new EventHandler(this.leave);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(19, 375);
      this.label1.Name = "label1";
      this.label1.Size = new Size(331, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "* Excluded from Total Military Entitlements - included in Total Income.";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = nameof (MilitaryEntitlements);
      this.emHelpLink1.Location = new Point(12, 404);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(106, 26);
      this.emHelpLink1.TabIndex = 78;
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(368, 439);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MilitaryEntitlements);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Calculate Military Entitlements";
      this.KeyPress += new KeyPressEventHandler(this.BorIncomeDialog_KeyPress);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void leave(object sender, EventArgs e)
    {
      if (sender == null || !(sender is TextBox))
        return;
      TextBox textBox = (TextBox) sender;
      if (textBox.Name == "borIncTxt" || textBox.Name == "cobIncTxt")
      {
        double num = this.DoubleValue(textBox.Text);
        if (num != 0.0)
          textBox.Text = num.ToString("N2");
      }
      this.CalculateTotal();
    }

    private void CalculateTotal()
    {
      this.militaryEntitlementsTotal = this.DoubleValue(this.textBox2.Text) + this.DoubleValue(this.textBox3.Text) + this.DoubleValue(this.textBox4.Text) + this.DoubleValue(this.textBox5.Text) + this.DoubleValue(this.textBox7.Text) + this.DoubleValue(this.textBox8.Text) + this.DoubleValue(this.textBox9.Text) + this.DoubleValue(this.textBox10.Text) + this.DoubleValue(this.textBox11.Text);
      this.textBox14.Text = this.militaryEntitlementsTotal.ToString("N2");
      this.baseIncome = this.DoubleValue(this.textBox1.Text);
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

    private void okBtn_Click(object sender, EventArgs e)
    {
      this.loan.SetCurrentField(this.fieldInd + "19", this.textBox1.Text, false);
      this.loan.SetCurrentField(this.fieldInd + "23", this.textBox13.Text, false);
      this.loan.SetCurrentField(this.fieldInd + "63", this.militaryEntitlementsTotal != 0.0 ? "Y" : "N");
      this.loan.SetCurrentField(this.fieldInd + "77", this.textBox2.Text);
      this.loan.SetCurrentField(this.fieldInd + "65", this.textBox3.Text);
      this.loan.SetCurrentField(this.fieldInd + "66", this.textBox4.Text);
      this.loan.SetCurrentField(this.fieldInd + "67", this.textBox5.Text);
      this.loan.SetCurrentField(this.fieldInd + "68", this.textBox7.Text);
      this.loan.SetCurrentField(this.fieldInd + "69", this.textBox8.Text);
      this.loan.SetCurrentField(this.fieldInd + "70", this.textBox9.Text);
      this.loan.SetCurrentField(this.fieldInd + "71", this.textBox10.Text);
      this.loan.SetCurrentField(this.fieldInd + "72", this.textBox11.Text);
      this.loan.SetCurrentField(this.fieldInd + "74", this.textBox12.Text);
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

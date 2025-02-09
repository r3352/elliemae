// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TableDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TableDialog : Form
  {
    private Button cancelBtn;
    private Button okBtn;
    private Label label1;
    private System.ComponentModel.Container components;
    private Label amountLabel;
    private Label label4;
    private TextBox factorTxt;
    private Label label3;
    private Label label2;
    private TextBox baseTxt;
    private Label label5;
    private PanelEx panelEx1;
    private TextBox upToTxt;
    public string UpToAmount;
    public string Base;
    public string Factor;

    public TableDialog(string basedOn, bool isForPurchase)
      : this("", "", "", basedOn, isForPurchase)
    {
    }

    public TableDialog(
      string range,
      string sbase,
      string factor,
      string basedOn,
      bool isForPurchase)
    {
      this.InitializeComponent();
      this.upToTxt.Text = range;
      this.baseTxt.Text = sbase;
      this.factorTxt.Text = factor;
      this.amountLabel.Text = basedOn;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.label1 = new Label();
      this.upToTxt = new TextBox();
      this.amountLabel = new Label();
      this.label4 = new Label();
      this.factorTxt = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.baseTxt = new TextBox();
      this.label5 = new Label();
      this.panelEx1 = new PanelEx();
      this.SuspendLayout();
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.cancelBtn.Location = new Point(216, 107);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 5;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.okBtn.Location = new Point(134, 107);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 4;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(81, 13);
      this.label1.TabIndex = 45;
      this.label1.Text = "Range Up To $";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.upToTxt.Location = new Point(98, 16);
      this.upToTxt.MaxLength = 10;
      this.upToTxt.Name = "upToTxt";
      this.upToTxt.Size = new Size(193, 20);
      this.upToTxt.TabIndex = 1;
      this.upToTxt.TextAlign = HorizontalAlignment.Right;
      this.upToTxt.Leave += new EventHandler(this.leave);
      this.upToTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.upToTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.amountLabel.AutoSize = true;
      this.amountLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.amountLabel.Location = new Point(184, 76);
      this.amountLabel.Name = "amountLabel";
      this.amountLabel.Size = new Size(113, 13);
      this.amountLabel.TabIndex = 58;
      this.amountLabel.Text = "Base Loan Amount";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(155, 76);
      this.label4.Name = "label4";
      this.label4.Size = new Size(15, 13);
      this.label4.TabIndex = 57;
      this.label4.Text = "%";
      this.factorTxt.Location = new Point(98, 72);
      this.factorTxt.MaxLength = 7;
      this.factorTxt.Name = "factorTxt";
      this.factorTxt.Size = new Size(54, 20);
      this.factorTxt.TabIndex = 3;
      this.factorTxt.TextAlign = HorizontalAlignment.Right;
      this.factorTxt.Leave += new EventHandler(this.leave);
      this.factorTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.factorTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(10, 76);
      this.label3.Name = "label3";
      this.label3.Size = new Size(46, 13);
      this.label3.TabIndex = 56;
      this.label3.Text = "+ Factor";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 48);
      this.label2.Name = "label2";
      this.label2.Size = new Size(40, 13);
      this.label2.TabIndex = 55;
      this.label2.Text = "Base $";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.baseTxt.Location = new Point(98, 44);
      this.baseTxt.MaxLength = 10;
      this.baseTxt.Name = "baseTxt";
      this.baseTxt.Size = new Size(193, 20);
      this.baseTxt.TabIndex = 2;
      this.baseTxt.TextAlign = HorizontalAlignment.Right;
      this.baseTxt.Leave += new EventHandler(this.leave);
      this.baseTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.baseTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.label5.Font = new Font("Verdana", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(171, 73);
      this.label5.Name = "label5";
      this.label5.Size = new Size(16, 20);
      this.label5.TabIndex = 60;
      this.label5.Text = "x";
      this.panelEx1.BorderStyle = BorderStyle.FixedSingle;
      this.panelEx1.ForeColor = SystemColors.ControlLight;
      this.panelEx1.Location = new Point(13, 99);
      this.panelEx1.Name = "panelEx1";
      this.panelEx1.Size = new Size(280, 1);
      this.panelEx1.TabIndex = 61;
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(303, 143);
      this.Controls.Add((Control) this.panelEx1);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.amountLabel);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.factorTxt);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.baseTxt);
      this.Controls.Add((Control) this.upToTxt);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TableDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Fee Details";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.upToTxt.Text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter an up to amount.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.upToTxt.Focus();
      }
      else if (this.baseTxt.Text == string.Empty && this.factorTxt.Text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a base or factor.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        if (this.baseTxt.Text == string.Empty)
          this.baseTxt.Focus();
        else
          this.factorTxt.Focus();
      }
      else
      {
        this.UpToAmount = this.upToTxt.Text;
        this.Base = this.baseTxt.Text;
        this.Factor = this.factorTxt.Text;
        this.DialogResult = DialogResult.OK;
      }
    }

    private double DoubleValue(string strValue)
    {
      return strValue == string.Empty || strValue == null ? 0.0 : double.Parse(strValue.Replace(",", string.Empty));
    }

    private void leave(object sender, EventArgs e)
    {
      if (sender == null || !(sender is TextBox))
        return;
      TextBox textBox = (TextBox) sender;
      if (textBox.Name != "factorTxt")
      {
        double num = this.DoubleValue(textBox.Text);
        textBox.Text = num.ToString("N2");
      }
      else
      {
        if (!(textBox.Text == string.Empty))
          return;
        textBox.Text = "0.00";
      }
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

    private void keyup(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.DECIMAL;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }
  }
}

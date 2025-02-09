// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FixedRateDetails
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class FixedRateDetails : Form, ILoanProgramUpdate
  {
    private Label label1;
    private Label label2;
    private Label label4;
    private Label label5;
    private Label label3;
    private TextBox termBox;
    private IContainer components;
    private ToolTip fieldToolTip;
    private Label label8;
    private Label label25;
    private TextBox freqLbl;
    private TextBox dueLbl;
    private TextBox rateTypeLbl;
    private TextBox loanTypeLbl;
    private TextBox descLbl;
    private TextBox ccLbl;
    private LoanProgram lp;

    public FixedRateDetails(LoanProgram lp)
    {
      this.lp = lp;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.initSummary();
      this.fieldToolTip.SetToolTip((Control) this.loanTypeLbl, "1172");
      this.fieldToolTip.SetToolTip((Control) this.rateTypeLbl, "608");
      this.fieldToolTip.SetToolTip((Control) this.dueLbl, "325");
      this.fieldToolTip.SetToolTip((Control) this.freqLbl, "423");
      this.fieldToolTip.SetToolTip((Control) this.termBox, "4");
      this.fieldToolTip.SetToolTip((Control) this.ccLbl, "1785");
    }

    private void initSummary()
    {
      this.descLbl.Text = this.lp.Description;
      this.loanTypeLbl.Text = this.lp.GetField("1172");
      this.rateTypeLbl.Text = this.lp.GetField("608");
      this.dueLbl.Text = this.lp.GetField("325");
      this.freqLbl.Text = this.lp.GetField("423") == "Biweekly" ? "Bi-Weekly" : "Monthly";
      this.termBox.Text = this.lp.GetField("4");
      this.ccLbl.Text = this.lp.GetField("1785");
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
      this.label1 = new Label();
      this.label2 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label3 = new Label();
      this.termBox = new TextBox();
      this.fieldToolTip = new ToolTip(this.components);
      this.label8 = new Label();
      this.label25 = new Label();
      this.freqLbl = new TextBox();
      this.dueLbl = new TextBox();
      this.rateTypeLbl = new TextBox();
      this.loanTypeLbl = new TextBox();
      this.descLbl = new TextBox();
      this.ccLbl = new TextBox();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 115);
      this.label1.Name = "label1";
      this.label1.Size = new Size(58, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Loan Type";
      this.label1.TextAlign = ContentAlignment.TopRight;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 138);
      this.label2.Name = "label2";
      this.label2.Size = new Size(83, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "Loan Rate Type";
      this.label2.TextAlign = ContentAlignment.TopRight;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(10, 161);
      this.label4.Name = "label4";
      this.label4.Size = new Size(26, 14);
      this.label4.TabIndex = 3;
      this.label4.Text = "Due";
      this.label4.TextAlign = ContentAlignment.TopRight;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(10, 184);
      this.label5.Name = "label5";
      this.label5.Size = new Size(103, 14);
      this.label5.TabIndex = 4;
      this.label5.Text = "Payment Frequency";
      this.label5.TextAlign = ContentAlignment.TopRight;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(10, 207);
      this.label3.Name = "label3";
      this.label3.Size = new Size(31, 14);
      this.label3.TabIndex = 14;
      this.label3.Text = "Term";
      this.label3.TextAlign = ContentAlignment.MiddleRight;
      this.termBox.Location = new Point(165, 204);
      this.termBox.MaxLength = 4;
      this.termBox.Name = "termBox";
      this.termBox.Size = new Size(100, 20);
      this.termBox.TabIndex = 15;
      this.termBox.TextAlign = HorizontalAlignment.Right;
      this.termBox.KeyUp += new KeyEventHandler(this.keyup);
      this.termBox.KeyPress += new KeyPressEventHandler(this.keypressInt);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(10, 10);
      this.label8.Name = "label8";
      this.label8.Size = new Size(61, 14);
      this.label8.TabIndex = 34;
      this.label8.Text = "Description";
      this.label8.TextAlign = ContentAlignment.MiddleRight;
      this.label25.AutoSize = true;
      this.label25.Location = new Point(10, 92);
      this.label25.Name = "label25";
      this.label25.Size = new Size(113, 14);
      this.label25.TabIndex = 43;
      this.label25.Text = "Closing Cost Template";
      this.label25.TextAlign = ContentAlignment.MiddleRight;
      this.freqLbl.BackColor = Color.WhiteSmoke;
      this.freqLbl.Location = new Point(165, 181);
      this.freqLbl.MaxLength = 7;
      this.freqLbl.Name = "freqLbl";
      this.freqLbl.ReadOnly = true;
      this.freqLbl.Size = new Size(100, 20);
      this.freqLbl.TabIndex = 70;
      this.freqLbl.TabStop = false;
      this.dueLbl.BackColor = Color.WhiteSmoke;
      this.dueLbl.Location = new Point(165, 158);
      this.dueLbl.MaxLength = 7;
      this.dueLbl.Name = "dueLbl";
      this.dueLbl.ReadOnly = true;
      this.dueLbl.Size = new Size(100, 20);
      this.dueLbl.TabIndex = 69;
      this.dueLbl.TabStop = false;
      this.dueLbl.TextAlign = HorizontalAlignment.Right;
      this.rateTypeLbl.BackColor = Color.WhiteSmoke;
      this.rateTypeLbl.Location = new Point(165, 135);
      this.rateTypeLbl.MaxLength = 7;
      this.rateTypeLbl.Name = "rateTypeLbl";
      this.rateTypeLbl.ReadOnly = true;
      this.rateTypeLbl.Size = new Size(100, 20);
      this.rateTypeLbl.TabIndex = 68;
      this.rateTypeLbl.TabStop = false;
      this.loanTypeLbl.BackColor = Color.WhiteSmoke;
      this.loanTypeLbl.Location = new Point(165, 112);
      this.loanTypeLbl.MaxLength = 7;
      this.loanTypeLbl.Name = "loanTypeLbl";
      this.loanTypeLbl.ReadOnly = true;
      this.loanTypeLbl.Size = new Size(100, 20);
      this.loanTypeLbl.TabIndex = 67;
      this.loanTypeLbl.TabStop = false;
      this.descLbl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.descLbl.BackColor = Color.WhiteSmoke;
      this.descLbl.Location = new Point(165, 9);
      this.descLbl.MaxLength = 7;
      this.descLbl.Multiline = true;
      this.descLbl.Name = "descLbl";
      this.descLbl.ReadOnly = true;
      this.descLbl.Size = new Size((int) byte.MaxValue, 77);
      this.descLbl.TabIndex = 72;
      this.descLbl.TabStop = false;
      this.ccLbl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.ccLbl.BackColor = Color.WhiteSmoke;
      this.ccLbl.Location = new Point(165, 89);
      this.ccLbl.MaxLength = 7;
      this.ccLbl.Name = "ccLbl";
      this.ccLbl.ReadOnly = true;
      this.ccLbl.Size = new Size((int) byte.MaxValue, 20);
      this.ccLbl.TabIndex = 71;
      this.ccLbl.TabStop = false;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.AutoScroll = true;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(428, 276);
      this.ControlBox = false;
      this.Controls.Add((Control) this.descLbl);
      this.Controls.Add((Control) this.ccLbl);
      this.Controls.Add((Control) this.freqLbl);
      this.Controls.Add((Control) this.dueLbl);
      this.Controls.Add((Control) this.rateTypeLbl);
      this.Controls.Add((Control) this.loanTypeLbl);
      this.Controls.Add((Control) this.label25);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.termBox);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (FixedRateDetails);
      this.Text = nameof (FixedRateDetails);
      this.ResumeLayout(false);
      this.PerformLayout();
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

    public void UpdateLoanProgram() => this.lp.SetCurrentField("4", this.termBox.Text);

    private void keyup(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.DECIMAL_3;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void keypressInt(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar))
        e.Handled = false;
      else
        e.Handled = true;
    }

    public void ChangeFieldStatus(bool isEnabled) => this.termBox.Enabled = isEnabled;
  }
}

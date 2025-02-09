// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ARMRateDetails
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class ARMRateDetails : Form, ILoanProgramUpdate
  {
    private Label label1;
    private Label label2;
    private Label label4;
    private Label label5;
    private Label label3;
    private TextBox termBox;
    private IContainer components;
    private Label label10;
    private TextBox marginBox;
    private TextBox adjCapBox;
    private TextBox adjPerBox;
    private TextBox lifeCapBox;
    private TextBox floorBox;
    private Label label11;
    private Label label12;
    private Label label13;
    private Label label14;
    private Label label15;
    private Label label16;
    private Label label17;
    private TextBox firstRateBox;
    private TextBox firstPayBox;
    private ToolTip fieldToolTip;
    private Label label8;
    private Label label19;
    private Label label20;
    private Label label21;
    private Label label22;
    private Label label23;
    private Label label25;
    private TextBox loanTypeLbl;
    private TextBox rateTypeLbl;
    private TextBox dueLbl;
    private TextBox freqLbl;
    private TextBox monthBtwLbl;
    private TextBox ccLbl;
    private TextBox descLbl;
    private LoanProgram lp;

    internal ARMRateDetails(LoanProgram lp)
    {
      this.lp = lp;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.InitSummary();
      this.fieldToolTip.SetToolTip((Control) this.loanTypeLbl, "1172");
      this.fieldToolTip.SetToolTip((Control) this.rateTypeLbl, "608");
      this.fieldToolTip.SetToolTip((Control) this.dueLbl, "325");
      this.fieldToolTip.SetToolTip((Control) this.freqLbl, "423");
      this.fieldToolTip.SetToolTip((Control) this.monthBtwLbl, "694");
      this.fieldToolTip.SetToolTip((Control) this.termBox, "4");
      this.fieldToolTip.SetToolTip((Control) this.adjCapBox, "695");
      this.fieldToolTip.SetToolTip((Control) this.adjPerBox, "694");
      this.fieldToolTip.SetToolTip((Control) this.lifeCapBox, "247");
      this.fieldToolTip.SetToolTip((Control) this.firstRateBox, "697");
      this.fieldToolTip.SetToolTip((Control) this.firstPayBox, "696");
      this.fieldToolTip.SetToolTip((Control) this.ccLbl, "1785");
    }

    private void InitSummary()
    {
      this.descLbl.Text = this.lp.Description;
      this.loanTypeLbl.Text = this.lp.GetField("1172");
      this.rateTypeLbl.Text = this.lp.GetField("608");
      this.dueLbl.Text = this.lp.GetField("325");
      this.freqLbl.Text = this.lp.GetField("423") == "Biweekly" ? "Bi-Weekly" : "Monthly";
      this.monthBtwLbl.Text = this.lp.GetField("694");
      this.termBox.Text = this.lp.GetField("4");
      this.adjCapBox.Text = this.lp.GetField("695");
      this.adjPerBox.Text = this.lp.GetField("694");
      this.lifeCapBox.Text = this.lp.GetField("247");
      this.firstRateBox.Text = this.lp.GetField("697");
      this.firstPayBox.Text = this.lp.GetField("696");
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
      this.label10 = new Label();
      this.marginBox = new TextBox();
      this.adjCapBox = new TextBox();
      this.adjPerBox = new TextBox();
      this.lifeCapBox = new TextBox();
      this.floorBox = new TextBox();
      this.label11 = new Label();
      this.label12 = new Label();
      this.label13 = new Label();
      this.label14 = new Label();
      this.label15 = new Label();
      this.label16 = new Label();
      this.label17 = new Label();
      this.firstRateBox = new TextBox();
      this.firstPayBox = new TextBox();
      this.fieldToolTip = new ToolTip(this.components);
      this.label8 = new Label();
      this.label19 = new Label();
      this.label20 = new Label();
      this.label21 = new Label();
      this.label22 = new Label();
      this.label23 = new Label();
      this.label25 = new Label();
      this.loanTypeLbl = new TextBox();
      this.rateTypeLbl = new TextBox();
      this.dueLbl = new TextBox();
      this.freqLbl = new TextBox();
      this.monthBtwLbl = new TextBox();
      this.ccLbl = new TextBox();
      this.descLbl = new TextBox();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 115);
      this.label1.Name = "label1";
      this.label1.Size = new Size(58, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Loan Type";
      this.label1.TextAlign = ContentAlignment.MiddleRight;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 138);
      this.label2.Name = "label2";
      this.label2.Size = new Size(83, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "Loan Rate Type";
      this.label2.TextAlign = ContentAlignment.MiddleRight;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(10, 161);
      this.label4.Name = "label4";
      this.label4.Size = new Size(26, 14);
      this.label4.TabIndex = 3;
      this.label4.Text = "Due";
      this.label4.TextAlign = ContentAlignment.MiddleRight;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(10, 184);
      this.label5.Name = "label5";
      this.label5.Size = new Size(103, 14);
      this.label5.TabIndex = 4;
      this.label5.Text = "Payment Frequency";
      this.label5.TextAlign = ContentAlignment.MiddleRight;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(10, 230);
      this.label3.Name = "label3";
      this.label3.Size = new Size(31, 14);
      this.label3.TabIndex = 14;
      this.label3.Text = "Term";
      this.label3.TextAlign = ContentAlignment.MiddleRight;
      this.termBox.Location = new Point(165, 227);
      this.termBox.MaxLength = 4;
      this.termBox.Name = "termBox";
      this.termBox.Size = new Size(100, 20);
      this.termBox.TabIndex = 2;
      this.termBox.TextAlign = HorizontalAlignment.Right;
      this.termBox.KeyUp += new KeyEventHandler(this.keyup);
      this.termBox.KeyPress += new KeyPressEventHandler(this.keypressInt);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(10, 207);
      this.label10.Name = "label10";
      this.label10.Size = new Size(146, 14);
      this.label10.TabIndex = 19;
      this.label10.Text = "Months Between Adjustment";
      this.label10.TextAlign = ContentAlignment.MiddleRight;
      this.marginBox.Location = new Point(165, 273);
      this.marginBox.MaxLength = 7;
      this.marginBox.Name = "marginBox";
      this.marginBox.Size = new Size(100, 20);
      this.marginBox.TabIndex = 4;
      this.marginBox.TextAlign = HorizontalAlignment.Right;
      this.marginBox.KeyUp += new KeyEventHandler(this.keyup);
      this.marginBox.KeyPress += new KeyPressEventHandler(this.keypress);
      this.adjCapBox.Location = new Point(165, 296);
      this.adjCapBox.MaxLength = 7;
      this.adjCapBox.Name = "adjCapBox";
      this.adjCapBox.Size = new Size(100, 20);
      this.adjCapBox.TabIndex = 5;
      this.adjCapBox.TextAlign = HorizontalAlignment.Right;
      this.adjCapBox.KeyUp += new KeyEventHandler(this.keyup);
      this.adjCapBox.KeyPress += new KeyPressEventHandler(this.keypress);
      this.adjPerBox.Location = new Point(165, 319);
      this.adjPerBox.MaxLength = 4;
      this.adjPerBox.Name = "adjPerBox";
      this.adjPerBox.Size = new Size(100, 20);
      this.adjPerBox.TabIndex = 6;
      this.adjPerBox.TextAlign = HorizontalAlignment.Right;
      this.adjPerBox.KeyUp += new KeyEventHandler(this.keyup);
      this.adjPerBox.KeyPress += new KeyPressEventHandler(this.keypressInt);
      this.lifeCapBox.Location = new Point(165, 342);
      this.lifeCapBox.MaxLength = 7;
      this.lifeCapBox.Name = "lifeCapBox";
      this.lifeCapBox.Size = new Size(100, 20);
      this.lifeCapBox.TabIndex = 7;
      this.lifeCapBox.TextAlign = HorizontalAlignment.Right;
      this.lifeCapBox.KeyUp += new KeyEventHandler(this.keyup);
      this.lifeCapBox.KeyPress += new KeyPressEventHandler(this.keypress);
      this.floorBox.Location = new Point(165, 365);
      this.floorBox.MaxLength = 7;
      this.floorBox.Name = "floorBox";
      this.floorBox.Size = new Size(100, 20);
      this.floorBox.TabIndex = 8;
      this.floorBox.TextAlign = HorizontalAlignment.Right;
      this.floorBox.KeyUp += new KeyEventHandler(this.keyup);
      this.floorBox.KeyPress += new KeyPressEventHandler(this.keypress);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(10, 276);
      this.label11.Name = "label11";
      this.label11.Size = new Size(39, 14);
      this.label11.TabIndex = 25;
      this.label11.Text = "Margin";
      this.label11.TextAlign = ContentAlignment.MiddleRight;
      this.label12.AutoSize = true;
      this.label12.Location = new Point(10, 299);
      this.label12.Name = "label12";
      this.label12.Size = new Size(83, 14);
      this.label12.TabIndex = 26;
      this.label12.Text = "Adjustment Cap";
      this.label12.TextAlign = ContentAlignment.MiddleRight;
      this.label13.AutoSize = true;
      this.label13.Location = new Point(10, 322);
      this.label13.Name = "label13";
      this.label13.Size = new Size(94, 14);
      this.label13.TabIndex = 27;
      this.label13.Text = "Adjustment Period";
      this.label13.TextAlign = ContentAlignment.MiddleRight;
      this.label14.AutoSize = true;
      this.label14.Location = new Point(10, 345);
      this.label14.Name = "label14";
      this.label14.Size = new Size(66, 14);
      this.label14.TabIndex = 28;
      this.label14.Text = "Lifetime Cap";
      this.label14.TextAlign = ContentAlignment.MiddleRight;
      this.label15.AutoSize = true;
      this.label15.Location = new Point(10, 368);
      this.label15.Name = "label15";
      this.label15.Size = new Size(56, 14);
      this.label15.TabIndex = 29;
      this.label15.Text = "Floor Rate";
      this.label15.TextAlign = ContentAlignment.MiddleRight;
      this.label16.AutoSize = true;
      this.label16.Location = new Point(10, 391);
      this.label16.Name = "label16";
      this.label16.Size = new Size(132, 14);
      this.label16.TabIndex = 30;
      this.label16.Text = "First Rate Adjustment Cap";
      this.label16.TextAlign = ContentAlignment.MiddleRight;
      this.label17.AutoSize = true;
      this.label17.Location = new Point(10, 414);
      this.label17.Name = "label17";
      this.label17.Size = new Size(129, 14);
      this.label17.TabIndex = 31;
      this.label17.Text = "First Payment Adjustment";
      this.label17.TextAlign = ContentAlignment.MiddleRight;
      this.firstRateBox.Location = new Point(165, 388);
      this.firstRateBox.MaxLength = 7;
      this.firstRateBox.Name = "firstRateBox";
      this.firstRateBox.Size = new Size(100, 20);
      this.firstRateBox.TabIndex = 9;
      this.firstRateBox.TextAlign = HorizontalAlignment.Right;
      this.firstRateBox.KeyUp += new KeyEventHandler(this.keyup);
      this.firstRateBox.KeyPress += new KeyPressEventHandler(this.keypress);
      this.firstPayBox.Location = new Point(165, 411);
      this.firstPayBox.MaxLength = 4;
      this.firstPayBox.Name = "firstPayBox";
      this.firstPayBox.Size = new Size(100, 20);
      this.firstPayBox.TabIndex = 10;
      this.firstPayBox.TextAlign = HorizontalAlignment.Right;
      this.firstPayBox.KeyUp += new KeyEventHandler(this.keyup);
      this.firstPayBox.KeyPress += new KeyPressEventHandler(this.keypressInt);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(10, 10);
      this.label8.Name = "label8";
      this.label8.Size = new Size(61, 14);
      this.label8.TabIndex = 32;
      this.label8.Text = "Description";
      this.label8.TextAlign = ContentAlignment.MiddleRight;
      this.label19.AutoSize = true;
      this.label19.Location = new Point(269, 276);
      this.label19.Name = "label19";
      this.label19.Size = new Size(17, 14);
      this.label19.TabIndex = 36;
      this.label19.Text = "%";
      this.label19.TextAlign = ContentAlignment.MiddleRight;
      this.label20.AutoSize = true;
      this.label20.Location = new Point(269, 299);
      this.label20.Name = "label20";
      this.label20.Size = new Size(17, 14);
      this.label20.TabIndex = 37;
      this.label20.Text = "%";
      this.label20.TextAlign = ContentAlignment.MiddleRight;
      this.label21.AutoSize = true;
      this.label21.Location = new Point(269, 345);
      this.label21.Name = "label21";
      this.label21.Size = new Size(17, 14);
      this.label21.TabIndex = 38;
      this.label21.Text = "%";
      this.label21.TextAlign = ContentAlignment.MiddleRight;
      this.label22.AutoSize = true;
      this.label22.Location = new Point(269, 368);
      this.label22.Name = "label22";
      this.label22.Size = new Size(17, 14);
      this.label22.TabIndex = 39;
      this.label22.Text = "%";
      this.label22.TextAlign = ContentAlignment.MiddleRight;
      this.label23.AutoSize = true;
      this.label23.Location = new Point(269, 391);
      this.label23.Name = "label23";
      this.label23.Size = new Size(17, 14);
      this.label23.TabIndex = 40;
      this.label23.Text = "%";
      this.label23.TextAlign = ContentAlignment.MiddleRight;
      this.label25.AutoSize = true;
      this.label25.Location = new Point(10, 92);
      this.label25.Name = "label25";
      this.label25.Size = new Size(113, 14);
      this.label25.TabIndex = 41;
      this.label25.Text = "Closing Cost Template";
      this.label25.TextAlign = ContentAlignment.MiddleRight;
      this.loanTypeLbl.BackColor = Color.WhiteSmoke;
      this.loanTypeLbl.Location = new Point(165, 112);
      this.loanTypeLbl.MaxLength = 7;
      this.loanTypeLbl.Name = "loanTypeLbl";
      this.loanTypeLbl.ReadOnly = true;
      this.loanTypeLbl.Size = new Size(100, 20);
      this.loanTypeLbl.TabIndex = 43;
      this.loanTypeLbl.TabStop = false;
      this.rateTypeLbl.BackColor = Color.WhiteSmoke;
      this.rateTypeLbl.Location = new Point(165, 135);
      this.rateTypeLbl.MaxLength = 7;
      this.rateTypeLbl.Name = "rateTypeLbl";
      this.rateTypeLbl.ReadOnly = true;
      this.rateTypeLbl.Size = new Size(100, 20);
      this.rateTypeLbl.TabIndex = 44;
      this.rateTypeLbl.TabStop = false;
      this.dueLbl.BackColor = Color.WhiteSmoke;
      this.dueLbl.Location = new Point(165, 158);
      this.dueLbl.MaxLength = 7;
      this.dueLbl.Name = "dueLbl";
      this.dueLbl.ReadOnly = true;
      this.dueLbl.Size = new Size(100, 20);
      this.dueLbl.TabIndex = 45;
      this.dueLbl.TabStop = false;
      this.dueLbl.TextAlign = HorizontalAlignment.Right;
      this.freqLbl.BackColor = Color.WhiteSmoke;
      this.freqLbl.Location = new Point(165, 181);
      this.freqLbl.MaxLength = 7;
      this.freqLbl.Name = "freqLbl";
      this.freqLbl.ReadOnly = true;
      this.freqLbl.Size = new Size(100, 20);
      this.freqLbl.TabIndex = 46;
      this.freqLbl.TabStop = false;
      this.monthBtwLbl.BackColor = Color.WhiteSmoke;
      this.monthBtwLbl.Location = new Point(165, 204);
      this.monthBtwLbl.MaxLength = 7;
      this.monthBtwLbl.Name = "monthBtwLbl";
      this.monthBtwLbl.ReadOnly = true;
      this.monthBtwLbl.Size = new Size(100, 20);
      this.monthBtwLbl.TabIndex = 47;
      this.monthBtwLbl.TabStop = false;
      this.monthBtwLbl.TextAlign = HorizontalAlignment.Right;
      this.ccLbl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.ccLbl.BackColor = Color.WhiteSmoke;
      this.ccLbl.Location = new Point(165, 89);
      this.ccLbl.MaxLength = 7;
      this.ccLbl.Name = "ccLbl";
      this.ccLbl.ReadOnly = true;
      this.ccLbl.Size = new Size((int) byte.MaxValue, 20);
      this.ccLbl.TabIndex = 48;
      this.ccLbl.TabStop = false;
      this.descLbl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.descLbl.BackColor = Color.WhiteSmoke;
      this.descLbl.Location = new Point(165, 9);
      this.descLbl.MaxLength = 7;
      this.descLbl.Multiline = true;
      this.descLbl.Name = "descLbl";
      this.descLbl.ReadOnly = true;
      this.descLbl.Size = new Size((int) byte.MaxValue, 77);
      this.descLbl.TabIndex = 49;
      this.descLbl.TabStop = false;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.AutoScroll = true;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(428, 460);
      this.ControlBox = false;
      this.Controls.Add((Control) this.descLbl);
      this.Controls.Add((Control) this.ccLbl);
      this.Controls.Add((Control) this.monthBtwLbl);
      this.Controls.Add((Control) this.freqLbl);
      this.Controls.Add((Control) this.dueLbl);
      this.Controls.Add((Control) this.rateTypeLbl);
      this.Controls.Add((Control) this.loanTypeLbl);
      this.Controls.Add((Control) this.label25);
      this.Controls.Add((Control) this.label23);
      this.Controls.Add((Control) this.label22);
      this.Controls.Add((Control) this.label21);
      this.Controls.Add((Control) this.label20);
      this.Controls.Add((Control) this.label19);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.firstPayBox);
      this.Controls.Add((Control) this.firstRateBox);
      this.Controls.Add((Control) this.label17);
      this.Controls.Add((Control) this.label16);
      this.Controls.Add((Control) this.floorBox);
      this.Controls.Add((Control) this.lifeCapBox);
      this.Controls.Add((Control) this.adjPerBox);
      this.Controls.Add((Control) this.adjCapBox);
      this.Controls.Add((Control) this.marginBox);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.termBox);
      this.Controls.Add((Control) this.label15);
      this.Controls.Add((Control) this.label14);
      this.Controls.Add((Control) this.label13);
      this.Controls.Add((Control) this.label12);
      this.Controls.Add((Control) this.label11);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (ARMRateDetails);
      this.Text = "FixedRateDetails";
      this.ResumeLayout(false);
      this.PerformLayout();
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

    private void keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar) || e.KeyChar.Equals('.'))
        e.Handled = false;
      else
        e.Handled = true;
    }

    public void UpdateLoanProgram()
    {
      this.lp.SetCurrentField("4", this.termBox.Text);
      this.lp.SetCurrentField("695", this.adjCapBox.Text);
      this.lp.SetCurrentField("KBYO.XD695", Utils.RemoveEndingZeros(this.lp.GetField("695")));
      this.lp.SetCurrentField("694", this.adjPerBox.Text);
      this.lp.SetCurrentField("247", this.lifeCapBox.Text);
      this.lp.SetCurrentField("697", this.firstRateBox.Text);
      this.lp.SetCurrentField("KBYO.XD697", Utils.RemoveEndingZeros(this.lp.GetField("697")));
      this.lp.SetCurrentField("696", this.firstPayBox.Text);
    }

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

    public void ChangeFieldStatus(bool isEnabled)
    {
      foreach (Control control in (ArrangedElementCollection) this.Controls)
      {
        if (control is TextBox)
          control.Enabled = isEnabled;
      }
    }
  }
}

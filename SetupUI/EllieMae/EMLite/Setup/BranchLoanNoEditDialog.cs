// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BranchLoanNoEditDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class BranchLoanNoEditDialog : Form
  {
    private Label label1;
    private TextBox nextNumTxt;
    private Label label2;
    private Button exitBtn;
    private Label label5;
    private GroupBox groupBox1;
    private CheckBox useLoanNoCheckBox;
    private TextBox codeTxt;
    private System.ComponentModel.Container components;
    private Label exampleLbl;
    private Button okBtn;
    private LoanNumberingInfo gloablLoanNo;
    private string orgCode = string.Empty;
    private string orgNextNo = string.Empty;
    private bool orgUseLoanNo;

    public BranchLoanNoEditDialog(string orgCode, string orgNextNo, bool orgUseLoanNo)
    {
      this.InitializeComponent();
      this.gloablLoanNo = Session.ConfigurationManager.GetLoanNumberingInfo();
      this.useLoanNoCheckBox.Checked = orgUseLoanNo;
      this.codeTxt.Text = orgCode;
      this.nextNumTxt.Text = orgNextNo;
      if (orgUseLoanNo)
        this.createExample();
      this.nextNumTxt.TextChanged += new EventHandler(this.nextNumTxt_TextChanged);
      this.useLoanNoCheckBox.CheckedChanged += new EventHandler(this.useLoanNoCheckBox_CheckedChanged);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public string OrgCode
    {
      set => this.orgCode = value;
    }

    public string OrgNextNo
    {
      set => this.orgNextNo = value;
      get => this.orgNextNo;
    }

    public bool OrgUseLoanNo
    {
      set => this.orgUseLoanNo = value;
      get => this.orgUseLoanNo;
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.nextNumTxt = new TextBox();
      this.codeTxt = new TextBox();
      this.label2 = new Label();
      this.useLoanNoCheckBox = new CheckBox();
      this.exampleLbl = new Label();
      this.exitBtn = new Button();
      this.okBtn = new Button();
      this.label5 = new Label();
      this.groupBox1 = new GroupBox();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(28, 28);
      this.label1.Name = "label1";
      this.label1.Size = new Size(97, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Organization Code:";
      this.nextNumTxt.Location = new Point(139, 60);
      this.nextNumTxt.MaxLength = 9;
      this.nextNumTxt.Name = "nextNumTxt";
      this.nextNumTxt.Size = new Size(108, 20);
      this.nextNumTxt.TabIndex = 4;
      this.nextNumTxt.KeyPress += new KeyPressEventHandler(this.nextNumTxt_KeyPress);
      this.codeTxt.Location = new Point(140, 28);
      this.codeTxt.MaxLength = 9;
      this.codeTxt.Name = "codeTxt";
      this.codeTxt.ReadOnly = true;
      this.codeTxt.Size = new Size(104, 20);
      this.codeTxt.TabIndex = 3;
      this.codeTxt.TabStop = false;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(28, 64);
      this.label2.Name = "label2";
      this.label2.Size = new Size(99, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "Next Loan Number:";
      this.useLoanNoCheckBox.Location = new Point(17, 20);
      this.useLoanNoCheckBox.Name = "useLoanNoCheckBox";
      this.useLoanNoCheckBox.Size = new Size(260, 18);
      this.useLoanNoCheckBox.TabIndex = 1;
      this.useLoanNoCheckBox.Text = "Generate Organization Specific Loan Numbers";
      this.exampleLbl.Location = new Point(28, 112);
      this.exampleLbl.Name = "exampleLbl";
      this.exampleLbl.Size = new Size(256, 16);
      this.exampleLbl.TabIndex = 10;
      this.exitBtn.DialogResult = DialogResult.Cancel;
      this.exitBtn.Location = new Point(180, 196);
      this.exitBtn.Name = "exitBtn";
      this.exitBtn.Size = new Size(75, 24);
      this.exitBtn.TabIndex = 6;
      this.exitBtn.Text = "&Cancel";
      this.okBtn.Location = new Point(96, 196);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 5;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(179, 85);
      this.label5.Name = "label5";
      this.label5.Size = new Size(75, 13);
      this.label5.TabIndex = 26;
      this.label5.Text = "(Max. 9 digits) ";
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.codeTxt);
      this.groupBox1.Controls.Add((Control) this.nextNumTxt);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.exampleLbl);
      this.groupBox1.Controls.Add((Control) this.label5);
      this.groupBox1.Location = new Point(16, 44);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(304, 140);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(334, 231);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.exitBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.useLoanNoCheckBox);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (BranchLoanNoEditDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Edit Organization Loan Numbering";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
    }

    private string createExample()
    {
      string str = this.codeTxt.Text.Trim();
      if (this.gloablLoanNo.UseYear)
        str += "YY";
      if (this.gloablLoanNo.UseMonth)
        str += "MM";
      string example = str + this.gloablLoanNo.Prefix.Trim() + this.nextNumTxt.Text.Trim() + this.gloablLoanNo.Suffix.Trim();
      this.exampleLbl.Text = "Example: " + example;
      return example;
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.useLoanNoCheckBox.Checked && this.nextNumTxt.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Next Number cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.nextNumTxt.Focus();
      }
      else if (this.createExample().Length > 18)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The loan number length cannot exceed 18 characters.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.nextNumTxt.Focus();
      }
      else
      {
        this.orgUseLoanNo = this.useLoanNoCheckBox.Checked;
        this.orgNextNo = this.nextNumTxt.Text.Trim();
        this.DialogResult = DialogResult.OK;
      }
    }

    private void keyup(object sender, KeyEventArgs e)
    {
      if (this.createExample().Length > 18)
      {
        if (sender != null && sender is TextBox)
        {
          TextBox textBox = (TextBox) sender;
          string text = textBox.Text;
          if (text != string.Empty)
          {
            textBox.Text = text.Substring(0, text.Length - 1);
            textBox.SelectionStart = textBox.Text.Trim().Length;
          }
        }
      }
      this.createExample();
    }

    private void nextNumTxt_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
      {
        this.createExample();
      }
      else
      {
        if (char.IsDigit(e.KeyChar))
          return;
        e.Handled = true;
      }
    }

    private void nextNumTxt_TextChanged(object sender, EventArgs e) => this.createExample();

    private void useLoanNoCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.useLoanNoCheckBox.Checked)
        this.createExample();
      else
        this.exampleLbl.Text = "";
    }
  }
}

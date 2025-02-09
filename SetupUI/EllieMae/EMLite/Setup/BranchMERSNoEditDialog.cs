// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BranchMERSNoEditDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class BranchMERSNoEditDialog : Form
  {
    private MersNumberingInfo gloablMERSNo;
    private string mersminCode = string.Empty;
    private string orgNextNo = string.Empty;
    private bool orgUseMERSNo;
    private IContainer components;
    private CheckBox chkUseBranch;
    private GroupBox groupBox1;
    private Label label1;
    private TextBox codeTxt;
    private TextBox nextNumTxt;
    private Label label2;
    private Button exitBtn;
    private Button okBtn;
    private Label exampleLbl;

    public BranchMERSNoEditDialog(string mersminCode, string orgNextNo, bool orgUseMERSNo)
    {
      this.InitializeComponent();
      this.gloablMERSNo = Session.ConfigurationManager.GetMersNumberingInfo();
      this.chkUseBranch.Checked = orgUseMERSNo;
      this.codeTxt.Text = mersminCode;
      this.nextNumTxt.Text = orgNextNo;
      if (orgUseMERSNo)
        this.createExample();
      this.nextNumTxt.TextChanged += new EventHandler(this.nextNumTxt_TextChanged);
      this.chkUseBranch.CheckedChanged += new EventHandler(this.chkUseBranch_CheckedChanged);
    }

    private void chkUseBranch_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkUseBranch.Checked)
        this.createExample();
      else
        this.exampleLbl.Text = "";
    }

    private void nextNumTxt_TextChanged(object sender, EventArgs e) => this.createExample();

    public string MERSMINCode
    {
      set => this.mersminCode = value;
    }

    public string OrgNextNo
    {
      set => this.orgNextNo = value;
      get => this.orgNextNo;
    }

    public bool OrgUseMERSNo
    {
      set => this.orgUseMERSNo = value;
      get => this.orgUseMERSNo;
    }

    private string createExample()
    {
      string str1 = this.codeTxt.Text.Trim();
      string str2 = this.nextNumTxt.Text.Trim();
      while (str2.Length < 10)
        str2 = "0" + str2;
      string example = str1 + str2;
      this.exampleLbl.Text = "Example: " + example;
      return example;
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.chkUseBranch.Checked && this.nextNumTxt.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Next Number cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.nextNumTxt.Focus();
      }
      else if (this.chkUseBranch.Checked && (!Utils.IsDouble((object) this.nextNumTxt.Text.Trim()) || this.nextNumTxt.Text.Trim().Length < 10))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Next Number has to be a 10 digit number.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.nextNumTxt.Focus();
      }
      else if (this.createExample().Length > 17)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The loan number length cannot exceed 18 characters.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.nextNumTxt.Focus();
      }
      else
      {
        this.orgUseMERSNo = this.chkUseBranch.Checked;
        this.orgNextNo = this.nextNumTxt.Text.Trim();
        this.DialogResult = DialogResult.OK;
      }
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

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.chkUseBranch = new CheckBox();
      this.groupBox1 = new GroupBox();
      this.exampleLbl = new Label();
      this.label1 = new Label();
      this.codeTxt = new TextBox();
      this.nextNumTxt = new TextBox();
      this.label2 = new Label();
      this.exitBtn = new Button();
      this.okBtn = new Button();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.chkUseBranch.Location = new Point(5, 5);
      this.chkUseBranch.Name = "chkUseBranch";
      this.chkUseBranch.Size = new Size(232, 18);
      this.chkUseBranch.TabIndex = 2;
      this.chkUseBranch.Text = "Generate Organization Specific MERS MIN Numbers";
      this.groupBox1.Controls.Add((Control) this.exampleLbl);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Controls.Add((Control) this.codeTxt);
      this.groupBox1.Controls.Add((Control) this.nextNumTxt);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Location = new Point(5, 29);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(304, 89);
      this.groupBox1.TabIndex = 7;
      this.groupBox1.TabStop = false;
      this.exampleLbl.Location = new Point(7, 59);
      this.exampleLbl.Name = "exampleLbl";
      this.exampleLbl.Size = new Size(256, 16);
      this.exampleLbl.TabIndex = 27;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(80, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Organization ID";
      this.codeTxt.Location = new Point(110, 13);
      this.codeTxt.MaxLength = 9;
      this.codeTxt.Name = "codeTxt";
      this.codeTxt.ReadOnly = true;
      this.codeTxt.Size = new Size(104, 20);
      this.codeTxt.TabIndex = 3;
      this.codeTxt.TabStop = false;
      this.nextNumTxt.Location = new Point(110, 36);
      this.nextNumTxt.MaxLength = 10;
      this.nextNumTxt.Name = "nextNumTxt";
      this.nextNumTxt.Size = new Size(104, 20);
      this.nextNumTxt.TabIndex = 4;
      this.nextNumTxt.KeyPress += new KeyPressEventHandler(this.nextNumTxt_KeyPress);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 35);
      this.label2.Name = "label2";
      this.label2.Size = new Size(69, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "Next Number";
      this.exitBtn.DialogResult = DialogResult.Cancel;
      this.exitBtn.Location = new Point(234, 124);
      this.exitBtn.Name = "exitBtn";
      this.exitBtn.Size = new Size(75, 24);
      this.exitBtn.TabIndex = 9;
      this.exitBtn.Text = "&Cancel";
      this.okBtn.Location = new Point(153, 124);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 8;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(322, 157);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.exitBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.chkUseBranch);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (BranchMERSNoEditDialog);
      this.Text = "Edit MERS MIN Numbering";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

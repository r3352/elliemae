// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ChangeCircumstanceEditor
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ChangeCircumstanceEditor : Form
  {
    private List<string[]> existOptions;
    private IContainer components;
    private DialogButtons dialogButtons1;
    private Label label1;
    private Label label2;
    private TextBox txtOption;
    private TextBox txtComments;
    private Label label3;
    private TextBox txtCode;

    public ChangeCircumstanceEditor(
      List<string[]> existOptions,
      string optionCode,
      string optionValue,
      string optionComments)
    {
      this.existOptions = existOptions;
      this.InitializeComponent();
      this.txtCode.Text = optionCode;
      this.txtOption.Text = optionValue;
      this.txtComments.Text = optionComments;
      if (optionCode != "")
        this.txtCode.ReadOnly = true;
      if (optionCode == "" && optionValue == "")
      {
        this.dialogButtons1.OKButton.Enabled = false;
        this.Text = "Add Changed Circumstance";
      }
      else
        this.Text = "Edit Changed Circumstance";
    }

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      if (this.txtCode.Text.Trim() == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please provide information for the code.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        for (int index = 0; index < this.existOptions.Count; ++index)
        {
          if (string.Compare(this.txtCode.Text.Trim(), this.existOptions[index][0], true) == 0)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "The list already contains the code '" + this.txtCode.Text.Trim() + "'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.txtOption.Focus();
            return;
          }
          if (string.Compare(this.txtOption.Text.Trim(), this.existOptions[index][1], true) == 0)
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "The list already contains the Changed Circumstance '" + this.txtOption.Text.Trim() + "'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.txtOption.Focus();
            return;
          }
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private void txt_TextChanged(object sender, EventArgs e)
    {
      this.dialogButtons1.OKButton.Enabled = this.txtCode.Text.Trim().Length > 0 && this.txtOption.Text.Trim().Length > 0;
    }

    public string OptionCode => this.txtCode.Text.Trim();

    public string OptionValue => this.txtOption.Text.Trim();

    public string OptionComments => this.txtComments.Text.Trim();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.dialogButtons1 = new DialogButtons();
      this.label1 = new Label();
      this.label2 = new Label();
      this.txtOption = new TextBox();
      this.txtComments = new TextBox();
      this.label3 = new Label();
      this.txtCode = new TextBox();
      this.SuspendLayout();
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 179);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(409, 44);
      this.dialogButtons1.TabIndex = 3;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 31);
      this.label1.Name = "label1";
      this.label1.Size = new Size(122, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Changed Circumstance";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(13, 57);
      this.label2.Name = "label2";
      this.label2.Size = new Size(56, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Comments";
      this.txtOption.Location = new Point(141, 28);
      this.txtOption.MaxLength = (int) byte.MaxValue;
      this.txtOption.Name = "txtOption";
      this.txtOption.Size = new Size(256, 20);
      this.txtOption.TabIndex = 1;
      this.txtOption.TextChanged += new EventHandler(this.txt_TextChanged);
      this.txtComments.Location = new Point(141, 54);
      this.txtComments.MaxLength = 4096;
      this.txtComments.Multiline = true;
      this.txtComments.Name = "txtComments";
      this.txtComments.ScrollBars = ScrollBars.Both;
      this.txtComments.Size = new Size(256, 121);
      this.txtComments.TabIndex = 2;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(13, 8);
      this.label3.Name = "label3";
      this.label3.Size = new Size(32, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "Code";
      this.txtCode.Location = new Point(141, 5);
      this.txtCode.MaxLength = 15;
      this.txtCode.Name = "txtCode";
      this.txtCode.Size = new Size(256, 20);
      this.txtCode.TabIndex = 0;
      this.txtCode.TextChanged += new EventHandler(this.txt_TextChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(409, 223);
      this.Controls.Add((Control) this.txtCode);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.txtComments);
      this.Controls.Add((Control) this.txtOption);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ChangeCircumstanceEditor);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Edit Changed Circumstance";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

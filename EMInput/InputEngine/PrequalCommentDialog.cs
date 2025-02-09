// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PrequalCommentDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class PrequalCommentDialog : Form
  {
    private TextBox textBox1;
    private Button okBtn;
    private Button cancelBtn;
    private Label labelHeader;
    private System.ComponentModel.Container components;
    private string comments = string.Empty;

    public PrequalCommentDialog(string comments, int scenario)
    {
      this.InitializeComponent();
      this.labelHeader.Text = "Scenario: " + scenario.ToString();
      this.comments = comments;
      this.textBox1.Text = this.comments;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public string Comments => this.comments;

    private void InitializeComponent()
    {
      this.textBox1 = new TextBox();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.labelHeader = new Label();
      this.SuspendLayout();
      this.textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBox1.Location = new Point(8, 32);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(488, 236);
      this.textBox1.TabIndex = 0;
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(336, 282);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 9;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(420, 282);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 10;
      this.cancelBtn.Text = "&Cancel";
      this.labelHeader.Location = new Point(8, 12);
      this.labelHeader.Name = "labelHeader";
      this.labelHeader.Size = new Size(464, 16);
      this.labelHeader.TabIndex = 11;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.AutoScroll = true;
      this.ClientSize = new Size(504, 323);
      this.Controls.Add((Control) this.labelHeader);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.textBox1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (PrequalCommentDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Loan Comparison Comments";
      this.KeyPress += new KeyPressEventHandler(this.PrequalCommentDialog_KeyPress);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void okBtn_Click(object sender, EventArgs e) => this.comments = this.textBox1.Text;

    private void PrequalCommentDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.Close();
    }
  }
}

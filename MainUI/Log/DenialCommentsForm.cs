// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.DenialCommentsForm
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class DenialCommentsForm : Form
  {
    private string comments = string.Empty;
    private IContainer components;
    private Button btnCancel;
    private Button btnOK;
    private TextBox textBoxComments;
    private Label label1;
    private Label label2;

    public string Comments => this.comments;

    public DenialCommentsForm() => this.InitializeComponent();

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.comments = this.textBoxComments.Text.Trim();
      this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.textBoxComments = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(358, 185);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(278, 185);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.textBoxComments.Location = new Point(8, 32);
      this.textBoxComments.Multiline = true;
      this.textBoxComments.Name = "textBoxComments";
      this.textBoxComments.Size = new Size(424, 120);
      this.textBoxComments.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(171, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Enter comments for the Requester:";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 160);
      this.label2.Name = "label2";
      this.label2.Size = new Size(371, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Note: Selecting OK will clear the Buy Side and Sell Side fields for this request.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(442, 219);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBoxComments);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DenialCommentsForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Lock Denial Comments";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

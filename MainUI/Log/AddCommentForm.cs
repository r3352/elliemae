// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.AddCommentForm
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
  public class AddCommentForm : Form
  {
    private IContainer components;
    private TextBox textBoxComments;
    private Button btnOK;
    private Button btnCancel;

    public AddCommentForm(string userFullName)
    {
      this.InitializeComponent();
      this.textBoxComments.Text = userFullName + " " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + " >> ";
      this.textBoxComments.SelectionStart = this.textBoxComments.Text.Length;
    }

    public string NewComments => this.textBoxComments.Text;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.textBoxComments = new TextBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.textBoxComments.Location = new Point(8, 12);
      this.textBoxComments.Multiline = true;
      this.textBoxComments.Name = "textBoxComments";
      this.textBoxComments.Size = new Size(424, 160);
      this.textBoxComments.TabIndex = 5;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(276, 184);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 6;
      this.btnOK.Text = "&OK";
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(356, 184);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "&Cancel";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(440, 219);
      this.Controls.Add((Control) this.textBoxComments);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddCommentForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Comments";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

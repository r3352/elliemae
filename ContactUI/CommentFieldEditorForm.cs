// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CommentFieldEditorForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class CommentFieldEditorForm : Form
  {
    private ComboBox comboBox;
    private int maxLength = 4096;
    private IContainer components;
    private TextBox txtComment;
    private Button btnCancel;
    private Button btnOK;
    private Label labelTotalLeft;
    private Label label1;

    public CommentFieldEditorForm(ComboBox comboBox)
    {
      this.comboBox = comboBox;
      this.maxLength = comboBox.MaxLength;
      this.InitializeComponent();
      this.txtComment.Text = comboBox.Text;
      this.Text = "Edit Comment: " + ((ContactCustomFieldInfo) comboBox.Tag).Label;
      this.txtComment_KeyPress((object) null, (KeyPressEventArgs) null);
      this.txtComment.Focus();
      this.txtComment.SelectionStart = this.txtComment.Text.Length;
      this.txtComment.ScrollToCaret();
      this.txtComment.Refresh();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.comboBox.Text = this.txtComment.Text;
      this.DialogResult = DialogResult.OK;
    }

    private void CommentFieldEditorForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!(this.txtComment.Text != this.comboBox.Text) || Utils.Dialog((IWin32Window) this, "You have updated comment. Are you sure you want to cancel it?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
        return;
      e.Cancel = true;
    }

    public string UpdatedComment => this.txtComment.Text;

    private void txtComment_KeyPress(object sender, KeyPressEventArgs e)
    {
      this.labelTotalLeft.Text = string.Concat((object) (this.maxLength - this.txtComment.Text.Length));
    }

    private void txtComment_KeyDown(object sender, KeyEventArgs e)
    {
      if (this.txtComment.Text.Length < 4096)
        return;
      e.Handled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtComment = new TextBox();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.labelTotalLeft = new Label();
      this.label1 = new Label();
      this.SuspendLayout();
      this.txtComment.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtComment.Location = new Point(12, 12);
      this.txtComment.MaxLength = 4096;
      this.txtComment.Multiline = true;
      this.txtComment.Name = "txtComment";
      this.txtComment.ScrollBars = ScrollBars.Both;
      this.txtComment.Size = new Size(677, 294);
      this.txtComment.TabIndex = 0;
      this.txtComment.KeyDown += new KeyEventHandler(this.txtComment_KeyDown);
      this.txtComment.KeyPress += new KeyPressEventHandler(this.txtComment_KeyPress);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(614, 315);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(533, 315);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.labelTotalLeft.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.labelTotalLeft.AutoSize = true;
      this.labelTotalLeft.Location = new Point(146, 320);
      this.labelTotalLeft.Name = "labelTotalLeft";
      this.labelTotalLeft.Size = new Size(31, 13);
      this.labelTotalLeft.TabIndex = 3;
      this.labelTotalLeft.Text = "4096";
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 320);
      this.label1.Name = "label1";
      this.label1.Size = new Size(128, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Available Characters Left:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(701, 347);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.labelTotalLeft);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.txtComment);
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (CommentFieldEditorForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Edit Comment";
      this.FormClosing += new FormClosingEventHandler(this.CommentFieldEditorForm_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

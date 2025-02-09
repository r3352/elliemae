// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.DocumentTrackingAddComment
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class DocumentTrackingAddComment : Form
  {
    private Sessions.Session session;
    private IContainer components;
    private Button button_OK;
    private Button button_Cancel;
    private Panel panel_AddComment;
    private RichTextBox richTextBox_AddComment;
    private Label label_Comment;

    public DocumentTrackingAddComment()
    {
      this.session = DocTrackingUtils.Session;
      this.InitializeComponent();
    }

    private void button_OK_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrWhiteSpace(this.richTextBox_AddComment.Text))
      {
        this.DialogResult = DialogResult.Cancel;
      }
      else
      {
        this.SaveComment(this.session.User.GetUserInfo().FullName, this.richTextBox_AddComment.Text);
        this.DialogResult = DialogResult.OK;
      }
    }

    private void SaveComment(string user, string commentText)
    {
      DocTrackingUtils.SaveComments(new DocumentTrackingComment()
      {
        UserName = user,
        LogDate = DateTime.Now,
        CommentText = commentText
      });
    }

    private void button_Cancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.button_OK = new Button();
      this.button_Cancel = new Button();
      this.panel_AddComment = new Panel();
      this.richTextBox_AddComment = new RichTextBox();
      this.label_Comment = new Label();
      this.panel_AddComment.SuspendLayout();
      this.SuspendLayout();
      this.button_OK.Location = new Point(274, 254);
      this.button_OK.Name = "button_OK";
      this.button_OK.Size = new Size(75, 23);
      this.button_OK.TabIndex = 1;
      this.button_OK.Text = "OK";
      this.button_OK.UseVisualStyleBackColor = true;
      this.button_OK.Click += new EventHandler(this.button_OK_Click);
      this.button_Cancel.Location = new Point(378, 254);
      this.button_Cancel.Name = "button_Cancel";
      this.button_Cancel.Size = new Size(75, 23);
      this.button_Cancel.TabIndex = 2;
      this.button_Cancel.Text = "Cancel";
      this.button_Cancel.UseVisualStyleBackColor = true;
      this.button_Cancel.Click += new EventHandler(this.button_Cancel_Click);
      this.panel_AddComment.BackColor = SystemColors.ButtonHighlight;
      this.panel_AddComment.Controls.Add((Control) this.richTextBox_AddComment);
      this.panel_AddComment.Controls.Add((Control) this.label_Comment);
      this.panel_AddComment.Cursor = Cursors.Arrow;
      this.panel_AddComment.Location = new Point(12, 12);
      this.panel_AddComment.Name = "panel_AddComment";
      this.panel_AddComment.Size = new Size(488, 225);
      this.panel_AddComment.TabIndex = 3;
      this.richTextBox_AddComment.Location = new Point(48, 44);
      this.richTextBox_AddComment.MaxLength = 500;
      this.richTextBox_AddComment.Name = "richTextBox_AddComment";
      this.richTextBox_AddComment.Size = new Size(379, 157);
      this.richTextBox_AddComment.TabIndex = 1;
      this.richTextBox_AddComment.Text = "";
      this.label_Comment.AutoSize = true;
      this.label_Comment.Location = new Point(45, 28);
      this.label_Comment.Name = "label_Comment";
      this.label_Comment.Size = new Size(59, 13);
      this.label_Comment.TabIndex = 0;
      this.label_Comment.Text = "Comments ";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(512, 311);
      this.Controls.Add((Control) this.panel_AddComment);
      this.Controls.Add((Control) this.button_Cancel);
      this.Controls.Add((Control) this.button_OK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DocumentTrackingAddComment);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Comment";
      this.panel_AddComment.ResumeLayout(false);
      this.panel_AddComment.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

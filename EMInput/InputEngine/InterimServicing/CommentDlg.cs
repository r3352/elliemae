// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.InterimServicing.CommentDlg
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.InterimServicing
{
  public class CommentDlg : Form
  {
    private IContainer components;
    private Label label_Comment;
    private RichTextBox richTextBox_Comments;
    private Button button_OK;
    private Button button_Cancel;

    public CommentDlg() => this.InitializeComponent();

    private void button_OK_Click(object sender, EventArgs e)
    {
      if (!string.IsNullOrWhiteSpace(this.richTextBox_Comments.Text))
        this.SaveComments(this.richTextBox_Comments.Text);
      this.DialogResult = DialogResult.OK;
    }

    private void button_Cancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void SaveComments(string commentText)
    {
      Comment comment = new Comment()
      {
        UserName = Session.User.GetUserInfo().FullName,
        LogDate = DateTime.Now,
        CommentText = commentText
      };
      List<Comment> toComments = CommentDlg.ParseToComments(Session.LoanData.GetField("SERVICE.Comments"));
      toComments.Add(comment);
      Session.LoanData.SetField("SERVICE.Comments", CommentDlg.StringifyFromComments((object) toComments));
    }

    internal static List<Comment> ParseToComments(string commentsJString)
    {
      if (string.IsNullOrEmpty(commentsJString))
        return new List<Comment>();
      using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(commentsJString)))
        return (List<Comment>) new DataContractJsonSerializer(typeof (List<Comment>)).ReadObject((Stream) memoryStream);
    }

    internal static string StringifyFromComments(object jsonObject)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new DataContractJsonSerializer(jsonObject.GetType()).WriteObject((Stream) memoryStream, jsonObject);
        return Encoding.UTF8.GetString(memoryStream.ToArray());
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
      this.label_Comment = new Label();
      this.richTextBox_Comments = new RichTextBox();
      this.button_OK = new Button();
      this.button_Cancel = new Button();
      this.SuspendLayout();
      this.label_Comment.AutoSize = true;
      this.label_Comment.Location = new Point(30, 22);
      this.label_Comment.Name = "label_Comment";
      this.label_Comment.Size = new Size(51, 13);
      this.label_Comment.TabIndex = 0;
      this.label_Comment.Text = "Comment";
      this.richTextBox_Comments.Location = new Point(33, 47);
      this.richTextBox_Comments.MaxLength = 500;
      this.richTextBox_Comments.Name = "richTextBox_Comments";
      this.richTextBox_Comments.Size = new Size(362, 167);
      this.richTextBox_Comments.TabIndex = 1;
      this.richTextBox_Comments.Text = "";
      this.button_OK.Location = new Point(215, 234);
      this.button_OK.Name = "button_OK";
      this.button_OK.Size = new Size(75, 23);
      this.button_OK.TabIndex = 2;
      this.button_OK.Text = "OK";
      this.button_OK.UseVisualStyleBackColor = true;
      this.button_OK.Click += new EventHandler(this.button_OK_Click);
      this.button_Cancel.Location = new Point(320, 234);
      this.button_Cancel.Name = "button_Cancel";
      this.button_Cancel.Size = new Size(75, 23);
      this.button_Cancel.TabIndex = 3;
      this.button_Cancel.Text = "Cancel";
      this.button_Cancel.UseVisualStyleBackColor = true;
      this.button_Cancel.Click += new EventHandler(this.button_Cancel_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(429, 277);
      this.Controls.Add((Control) this.button_Cancel);
      this.Controls.Add((Control) this.button_OK);
      this.Controls.Add((Control) this.richTextBox_Comments);
      this.Controls.Add((Control) this.label_Comment);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CommentDlg);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Comment";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

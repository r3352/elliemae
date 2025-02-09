// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.AddImageAttachmentDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class AddImageAttachmentDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private PageImage[] pageList;
    private DocumentLog doc;
    private ImageAttachment attachment;
    private IContainer components;
    private Label lblTitle;
    private TextBox txtTitle;
    private Button btnCancel;
    private Button btnAdd;

    public AddImageAttachmentDialog(LoanDataMgr loanDataMgr, PageImage[] pageList, DocumentLog doc)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.pageList = pageList;
      this.doc = doc;
      if (doc != null)
        this.txtTitle.Text = doc.Title;
      else
        this.txtTitle.Text = "Untitled";
    }

    public ImageAttachment Attachment => this.attachment;

    private void btnAdd_Click(object sender, EventArgs e)
    {
      string title = this.txtTitle.Text.Trim();
      if (title == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a title.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.attachment = this.loanDataMgr.FileAttachments.SplitAttachment(this.pageList, title, this.doc);
        this.DialogResult = DialogResult.OK;
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
      this.lblTitle = new Label();
      this.txtTitle = new TextBox();
      this.btnCancel = new Button();
      this.btnAdd = new Button();
      this.SuspendLayout();
      this.lblTitle.AutoSize = true;
      this.lblTitle.Location = new Point(12, 13);
      this.lblTitle.Margin = new Padding(2, 0, 2, 0);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(150, 14);
      this.lblTitle.TabIndex = 0;
      this.lblTitle.Text = "Enter a name for the new file:";
      this.txtTitle.Location = new Point(12, 28);
      this.txtTitle.Name = "txtTitle";
      this.txtTitle.Size = new Size(384, 20);
      this.txtTitle.TabIndex = 1;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(322, 56);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnAdd.Location = new Point(242, 56);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 22);
      this.btnAdd.TabIndex = 2;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.AcceptButton = (IButtonControl) this.btnAdd;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(409, 93);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.txtTitle);
      this.Controls.Add((Control) this.lblTitle);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Margin = new Padding(2, 3, 2, 3);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddImageAttachmentDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Create File";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.SuggestTrainingDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class SuggestTrainingDialog : Form
  {
    private GridViewDataManager gvAttachmentsMgr;
    private FileAttachment[] selectedList;
    private IContainer components;
    private Label lblDocName;
    private Button btnCancel;
    private Button btnOK;
    private GridView gvAttachments;
    private EMHelpLink helpLink;
    private EMHelpLink emHelpLink1;

    public FileAttachment[] Attachments => this.selectedList;

    public SuggestTrainingDialog(LoanDataMgr loanDataMgr, DocumentLog docLog)
    {
      this.InitializeComponent();
      this.lblDocName.Text = docLog.Title;
      this.gvAttachmentsMgr = new GridViewDataManager(this.gvAttachments, loanDataMgr);
      this.gvAttachmentsMgr.CreateLayout(new TableLayout.Column[2]
      {
        GridViewDataManager.CheckBoxColumn,
        GridViewDataManager.NameColumn
      });
      this.gvAttachments.Columns[1].SpringToFit = true;
      foreach (FileAttachmentReference fileRef in docLog.Files.ToArray())
      {
        if (fileRef.IsActive)
        {
          FileAttachment attachment = loanDataMgr.FileAttachments.GetAttachment(fileRef);
          if (attachment != null)
            this.gvAttachmentsMgr.AddItem(attachment, (FileAttachmentReference) null);
        }
      }
      this.gvAttachments.ReSort();
    }

    private FileAttachment[] getSelectedAttachments()
    {
      List<FileAttachment> fileAttachmentList = new List<FileAttachment>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvAttachments.Items)
      {
        FileAttachment tag = (FileAttachment) gvItem.Tag;
        if (gvItem.Checked && tag != null)
          fileAttachmentList.Add(tag);
      }
      return fileAttachmentList.ToArray();
    }

    private void gvAttachments_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.setButtons();
    }

    private void setButtons() => this.btnOK.Enabled = this.getSelectedAttachments().Length > 0;

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.gvAttachments.Sort(1, SortOrder.Descending);
      this.selectedList = this.getSelectedAttachments();
      this.DialogResult = DialogResult.OK;
    }

    private void SuggestTrainingDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblDocName = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.gvAttachments = new GridView();
      this.emHelpLink1 = new EMHelpLink();
      this.helpLink = new EMHelpLink();
      this.SuspendLayout();
      this.lblDocName.Location = new Point(10, 7);
      this.lblDocName.Name = "lblDocName";
      this.lblDocName.Size = new Size(270, 16);
      this.lblDocName.TabIndex = 0;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(207, 315);
      this.btnCancel.Margin = new Padding(1, 0, 0, 0);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Enabled = false;
      this.btnOK.Location = new Point(131, 315);
      this.btnOK.Margin = new Padding(1, 0, 0, 0);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 5;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.gvAttachments.AllowMultiselect = false;
      this.gvAttachments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvAttachments.ClearSelectionsOnEmptyRowClick = false;
      this.gvAttachments.Location = new Point(9, 26);
      this.gvAttachments.Name = "gvAttachments";
      this.gvAttachments.Size = new Size(271, 279);
      this.gvAttachments.TabIndex = 3;
      this.gvAttachments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvAttachments.SubItemCheck += new GVSubItemEventHandler(this.gvAttachments_SubItemCheck);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Suggest Training";
      this.emHelpLink1.Location = new Point(11, 316);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 19);
      this.emHelpLink1.TabIndex = 7;
      this.emHelpLink1.TabStop = false;
      this.helpLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Send Files";
      this.helpLink.Location = new Point(12, 354);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 19);
      this.helpLink.TabIndex = 17;
      this.helpLink.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(294, 346);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.lblDocName);
      this.Controls.Add((Control) this.gvAttachments);
      this.Controls.Add((Control) this.helpLink);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SuggestTrainingDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Suggest Attachments For Training";
      this.KeyDown += new KeyEventHandler(this.SuggestTrainingDialog_KeyDown);
      this.ResumeLayout(false);
    }
  }
}

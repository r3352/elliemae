// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.UI.CommentEntryDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.UI
{
  public class CommentEntryDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private CommentEntry entry;
    private IContainer components;
    private Button btnOK;
    private Button btnCancel;
    private TextBox txtComments;
    private Label lblDate;
    private TextBox txtDate;
    private TextBox txtAddedBy;
    private Label lblAddedBy;
    private CheckBox chkForRole;
    private ComboBox cboForRole;
    private Label lblReviewedDate;
    private TextBox txtReviewedDate;
    private TextBox txtReviewedBy;
    private Label lblReviewedBy;
    private Label lblForRole;
    private TextBox txtForRole;
    private Label lblExternal;
    private CheckBox chkExternal;

    public CommentEntryDialog(
      LoanDataMgr loanDataMgr,
      CommentEntry entry,
      bool readOnly,
      bool canDeliverComment = false)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.entry = entry != null ? entry : (!canDeliverComment ? new CommentEntry(string.Empty, Session.UserID, this.loanDataMgr.SessionObjects.UserInfo.FullName) : new CommentEntry(string.Empty, Session.UserID, this.loanDataMgr.SessionObjects.UserInfo.FullName, true, canDeliverComment));
      this.initializeRoleList();
      this.loadCommentEntry();
      if (readOnly)
      {
        this.txtComments.ReadOnly = true;
        this.chkForRole.Enabled = false;
        this.cboForRole.Visible = false;
        this.txtForRole.Visible = this.chkForRole.Checked;
        this.btnOK.Visible = false;
        this.btnCancel.Text = "Close";
        this.chkExternal.Enabled = false;
      }
      this.lblExternal.Visible = canDeliverComment;
      this.chkExternal.Visible = canDeliverComment;
    }

    public CommentEntry Entry => this.entry;

    private void initializeRoleList()
    {
      this.cboForRole.Items.AddRange((object[]) this.loanDataMgr.SystemConfiguration.AllRoles);
    }

    private void loadCommentEntry()
    {
      DateTime dateTime = this.entry.Date;
      DateTime date1 = dateTime.Date;
      dateTime = DateTime.MinValue;
      DateTime date2 = dateTime.Date;
      if (date1 != date2)
        this.txtDate.Text = this.entry.Date.ToString("MM/dd/yy hh:mm tt");
      this.txtAddedBy.Text = this.entry.AddedBy;
      this.txtComments.Text = this.entry.Comments;
      if (this.entry.ForRoleID != -1)
      {
        this.chkForRole.Checked = true;
        foreach (RoleInfo roleInfo in this.cboForRole.Items)
        {
          if (roleInfo.RoleID == this.entry.ForRoleID)
            this.cboForRole.SelectedItem = (object) roleInfo;
        }
        this.txtForRole.Text = this.cboForRole.Text;
      }
      if (this.entry.Reviewed)
      {
        this.txtReviewedDate.Text = this.entry.ReviewedDate.ToString("MM/dd/yy hh:mm tt");
        this.txtReviewedBy.Text = this.entry.ReviewedBy;
        this.lblReviewedDate.Visible = true;
        this.txtReviewedDate.Visible = true;
        this.lblReviewedBy.Visible = true;
        this.txtReviewedBy.Visible = true;
      }
      this.chkExternal.Checked = !this.entry.IsInternal;
    }

    private bool saveCommentEntry()
    {
      string text = this.txtComments.Text;
      if (text.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a comment.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      RoleInfo roleInfo = (RoleInfo) null;
      if (this.chkForRole.Checked)
      {
        roleInfo = (RoleInfo) this.cboForRole.SelectedItem;
        if (roleInfo == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must select a role to send the update alert to.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
      }
      this.entry.Comments = text;
      if (roleInfo != null)
        this.entry.ForRoleID = roleInfo.RoleID;
      return true;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.saveCommentEntry())
        return;
      this.DialogResult = DialogResult.OK;
    }

    private void chkForRole_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkForRole.Checked)
      {
        this.lblForRole.Text = "Send Update Alert to";
        this.cboForRole.Visible = true;
      }
      else
      {
        this.lblForRole.Text = "Send Update Alert";
        this.cboForRole.Visible = false;
      }
    }

    private void chkExternal_CheckedChanged(object sender, EventArgs e)
    {
      this.entry.IsInternal = !this.chkExternal.Checked;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.txtComments = new TextBox();
      this.lblDate = new Label();
      this.txtDate = new TextBox();
      this.txtAddedBy = new TextBox();
      this.lblAddedBy = new Label();
      this.chkForRole = new CheckBox();
      this.cboForRole = new ComboBox();
      this.lblReviewedDate = new Label();
      this.txtReviewedDate = new TextBox();
      this.txtReviewedBy = new TextBox();
      this.lblReviewedBy = new Label();
      this.lblForRole = new Label();
      this.txtForRole = new TextBox();
      this.lblExternal = new Label();
      this.chkExternal = new CheckBox();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(523, 296);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 13;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(606, 296);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 14;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.txtComments.AcceptsReturn = true;
      this.txtComments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtComments.Location = new Point(12, 36);
      this.txtComments.Multiline = true;
      this.txtComments.Name = "txtComments";
      this.txtComments.ScrollBars = ScrollBars.Vertical;
      this.txtComments.Size = new Size(668, 224);
      this.txtComments.TabIndex = 4;
      this.lblDate.AutoSize = true;
      this.lblDate.Location = new Point(12, 12);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new Size(54, 14);
      this.lblDate.TabIndex = 0;
      this.lblDate.Text = "Added on";
      this.txtDate.Location = new Point(68, 8);
      this.txtDate.Name = "txtDate";
      this.txtDate.ReadOnly = true;
      this.txtDate.Size = new Size(120, 20);
      this.txtDate.TabIndex = 1;
      this.txtDate.TabStop = false;
      this.txtAddedBy.Location = new Point(212, 8);
      this.txtAddedBy.Name = "txtAddedBy";
      this.txtAddedBy.ReadOnly = true;
      this.txtAddedBy.Size = new Size(184, 20);
      this.txtAddedBy.TabIndex = 3;
      this.txtAddedBy.TabStop = false;
      this.lblAddedBy.AutoSize = true;
      this.lblAddedBy.Location = new Point(192, 12);
      this.lblAddedBy.Name = "lblAddedBy";
      this.lblAddedBy.Size = new Size(19, 14);
      this.lblAddedBy.TabIndex = 2;
      this.lblAddedBy.Text = "by";
      this.chkForRole.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkForRole.AutoSize = true;
      this.chkForRole.Location = new Point(12, 272);
      this.chkForRole.Name = "chkForRole";
      this.chkForRole.Size = new Size(15, 14);
      this.chkForRole.TabIndex = 5;
      this.chkForRole.UseVisualStyleBackColor = true;
      this.chkForRole.CheckedChanged += new EventHandler(this.chkForRole_CheckedChanged);
      this.cboForRole.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cboForRole.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboForRole.FormattingEnabled = true;
      this.cboForRole.Location = new Point(136, 268);
      this.cboForRole.Name = "cboForRole";
      this.cboForRole.Size = new Size(136, 22);
      this.cboForRole.Sorted = true;
      this.cboForRole.TabIndex = 7;
      this.cboForRole.Visible = false;
      this.lblReviewedDate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblReviewedDate.AutoSize = true;
      this.lblReviewedDate.Location = new Point(280, 272);
      this.lblReviewedDate.Name = "lblReviewedDate";
      this.lblReviewedDate.Size = new Size(67, 14);
      this.lblReviewedDate.TabIndex = 9;
      this.lblReviewedDate.Text = "Received on";
      this.lblReviewedDate.Visible = false;
      this.txtReviewedDate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.txtReviewedDate.Location = new Point(352, 268);
      this.txtReviewedDate.Name = "txtReviewedDate";
      this.txtReviewedDate.ReadOnly = true;
      this.txtReviewedDate.Size = new Size(120, 20);
      this.txtReviewedDate.TabIndex = 10;
      this.txtReviewedDate.TabStop = false;
      this.txtReviewedDate.Visible = false;
      this.txtReviewedBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.txtReviewedBy.Location = new Point(496, 268);
      this.txtReviewedBy.Name = "txtReviewedBy";
      this.txtReviewedBy.ReadOnly = true;
      this.txtReviewedBy.Size = new Size(184, 20);
      this.txtReviewedBy.TabIndex = 12;
      this.txtReviewedBy.TabStop = false;
      this.txtReviewedBy.Visible = false;
      this.lblReviewedBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblReviewedBy.AutoSize = true;
      this.lblReviewedBy.Location = new Point(476, 272);
      this.lblReviewedBy.Name = "lblReviewedBy";
      this.lblReviewedBy.Size = new Size(19, 14);
      this.lblReviewedBy.TabIndex = 11;
      this.lblReviewedBy.Text = "by";
      this.lblReviewedBy.Visible = false;
      this.lblForRole.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblForRole.AutoSize = true;
      this.lblForRole.Location = new Point(28, 272);
      this.lblForRole.Name = "lblForRole";
      this.lblForRole.Size = new Size(95, 14);
      this.lblForRole.TabIndex = 6;
      this.lblForRole.Text = "Send Update Alert";
      this.txtForRole.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.txtForRole.Location = new Point(136, 268);
      this.txtForRole.Name = "txtForRole";
      this.txtForRole.ReadOnly = true;
      this.txtForRole.Size = new Size(136, 20);
      this.txtForRole.TabIndex = 8;
      this.txtForRole.TabStop = false;
      this.txtForRole.Visible = false;
      this.lblExternal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblExternal.AutoSize = true;
      this.lblExternal.Location = new Point(504, 11);
      this.lblExternal.Name = "lblExternal";
      this.lblExternal.Size = new Size(46, 14);
      this.lblExternal.TabIndex = 16;
      this.lblExternal.Text = "External";
      this.chkExternal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkExternal.AutoSize = true;
      this.chkExternal.Location = new Point(488, 11);
      this.chkExternal.Name = "chkExternal";
      this.chkExternal.Size = new Size(15, 14);
      this.chkExternal.TabIndex = 15;
      this.chkExternal.UseVisualStyleBackColor = true;
      this.chkExternal.CheckedChanged += new EventHandler(this.chkExternal_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(691, 327);
      this.Controls.Add((Control) this.lblExternal);
      this.Controls.Add((Control) this.chkExternal);
      this.Controls.Add((Control) this.lblForRole);
      this.Controls.Add((Control) this.txtReviewedBy);
      this.Controls.Add((Control) this.lblReviewedBy);
      this.Controls.Add((Control) this.txtReviewedDate);
      this.Controls.Add((Control) this.lblReviewedDate);
      this.Controls.Add((Control) this.cboForRole);
      this.Controls.Add((Control) this.chkForRole);
      this.Controls.Add((Control) this.txtAddedBy);
      this.Controls.Add((Control) this.lblAddedBy);
      this.Controls.Add((Control) this.txtDate);
      this.Controls.Add((Control) this.lblDate);
      this.Controls.Add((Control) this.txtComments);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.txtForRole);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CommentEntryDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Comment";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

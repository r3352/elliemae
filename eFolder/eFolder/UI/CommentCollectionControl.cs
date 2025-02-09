// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.UI.CommentCollectionControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.UI
{
  public class CommentCollectionControl : UserControl
  {
    private LoanDataMgr loanDataMgr;
    private CommentEntryCollection commentList;
    private bool canAddComment;
    private bool canDeleteComment;
    private bool canDeliverComment;
    private bool displayEnhanced;
    private IContainer components;
    private GroupContainer gcComments;
    private ListBoxVariable lstComments;
    private StandardIconButton btnDelete;
    private StandardIconButton btnView;
    private StandardIconButton btnAdd;
    private FlowLayoutPanel pnlToolbar;
    private ToolTip tooltip;
    private ContextMenuStrip mnuContext;
    private ToolStripMenuItem mnuItemDelete;
    private ToolStripMenuItem mnuInternal;
    private ToolStripMenuItem mnuExternal;

    public CommentCollectionControl() => this.InitializeComponent();

    public bool CanAddComment
    {
      get => this.canAddComment;
      set
      {
        this.canAddComment = value;
        this.btnAdd.Visible = value;
      }
    }

    public bool CanDeleteComment
    {
      get => this.canDeleteComment;
      set
      {
        this.canDeleteComment = value;
        this.btnDelete.Visible = value;
        this.mnuItemDelete.Visible = value;
      }
    }

    public bool CanDeliverComment
    {
      get => this.canDeliverComment;
      set => this.canDeliverComment = value;
    }

    public void DisplayEnhanced(bool canMarkComment)
    {
      this.displayEnhanced = true;
      this.lstComments.ContextMenuStrip = this.mnuContext;
      this.mnuExternal.Visible = canMarkComment;
      this.mnuInternal.Visible = canMarkComment;
    }

    public void LoadComments(LoanDataMgr loanDataMgr, CommentEntryCollection commentList)
    {
      this.loanDataMgr = loanDataMgr;
      this.commentList = commentList;
      this.loadCommentList();
    }

    private void loadCommentList()
    {
      int[] usersAssignedRoles = this.loanDataMgr.AccessRules.GetUsersAssignedRoles();
      ListBoxVariableItem listBoxVariableItem1 = (ListBoxVariableItem) null;
      foreach (CommentEntry comment in (CollectionBase) this.commentList)
      {
        if (!this.lstComments.ContainsItem((object) comment))
        {
          Font font = (Font) null;
          if (comment.ForRoleID != -1 && !comment.Reviewed && Array.IndexOf<int>(usersAssignedRoles, comment.ForRoleID) >= 0)
            font = EncompassFonts.Normal2.Font;
          if (this.displayEnhanced)
            comment.DisplayIsInternal = true;
          ListBoxVariableItem listBoxVariableItem2 = new ListBoxVariableItem((object) comment, font);
          this.lstComments.Items.Add((object) listBoxVariableItem2);
          if (font != null && listBoxVariableItem1 == null)
            listBoxVariableItem1 = listBoxVariableItem2;
        }
      }
      List<ListBoxVariableItem> listBoxVariableItemList = new List<ListBoxVariableItem>();
      foreach (ListBoxVariableItem listBoxVariableItem3 in this.lstComments.Items)
      {
        if (!this.commentList.Contains(listBoxVariableItem3.Item as CommentEntry))
          listBoxVariableItemList.Add(listBoxVariableItem3);
      }
      foreach (object obj in listBoxVariableItemList)
        this.lstComments.Items.Remove(obj);
      if (listBoxVariableItem1 == null)
        return;
      this.lstComments.SelectedItem = (object) listBoxVariableItem1;
    }

    private void loadCommentList(CommentEntry entry)
    {
      this.loadCommentList();
      ListBoxVariableItem listBoxVariableItem1 = (ListBoxVariableItem) null;
      foreach (ListBoxVariableItem listBoxVariableItem2 in this.lstComments.Items)
      {
        if (listBoxVariableItem2.Item.Equals((object) entry))
          listBoxVariableItem1 = listBoxVariableItem2;
      }
      this.lstComments.SelectedItem = (object) listBoxVariableItem1;
    }

    private void refreshToolbar()
    {
      int count = this.lstComments.SelectedItems.Count;
      this.btnView.Enabled = count > 0;
      this.btnDelete.Enabled = count > 0;
      if (!this.displayEnhanced)
        return;
      this.mnuItemDelete.Enabled = count > 0;
      if (count != 1)
        return;
      CommentEntry commentEntry = (CommentEntry) ((ListBoxVariableItem) this.lstComments.SelectedItem).Item;
      this.mnuExternal.Enabled = commentEntry.IsInternal;
      this.mnuInternal.Enabled = !commentEntry.IsInternal;
    }

    private void lstComments_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.refreshToolbar();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      using (CommentEntryDialog commentEntryDialog = new CommentEntryDialog(this.loanDataMgr, (CommentEntry) null, false, this.canDeliverComment))
      {
        if (commentEntryDialog.ShowDialog((IWin32Window) Form.ActiveForm) == DialogResult.Cancel)
          return;
        this.commentList.Add(commentEntryDialog.Entry);
        this.loadCommentList(commentEntryDialog.Entry);
        this.OnCommentChanged(EventArgs.Empty);
      }
    }

    private void btnView_Click(object sender, EventArgs e)
    {
      if (this.lstComments.SelectedItems.Count != 1)
        return;
      using (CommentEntryDialog commentEntryDialog = new CommentEntryDialog(this.loanDataMgr, (CommentEntry) ((ListBoxVariableItem) this.lstComments.SelectedItem).Item, true, this.canDeliverComment))
      {
        int num = (int) commentEntryDialog.ShowDialog((IWin32Window) Form.ActiveForm);
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      CommentEntry entry = (CommentEntry) ((ListBoxVariableItem) this.lstComments.SelectedItem).Item;
      if (Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to delete the following comment:\r\n\r\n" + entry.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      this.commentList.Remove(entry);
      this.loadCommentList();
      this.OnCommentChanged(EventArgs.Empty);
    }

    private void lstComments_ClientSizeChanged(object sender, EventArgs e)
    {
      if (this.lstComments.SelectedIndex < 0)
        return;
      this.lstComments.TopIndex = this.lstComments.SelectedIndex;
    }

    private void mnuInternal_Click(object sender, EventArgs e)
    {
      ((CommentEntry) ((ListBoxVariableItem) this.lstComments.SelectedItem).Item).IsInternal = true;
    }

    private void mnuExternal_Click(object sender, EventArgs e)
    {
      ((CommentEntry) ((ListBoxVariableItem) this.lstComments.SelectedItem).Item).IsInternal = false;
    }

    protected virtual void OnCommentChanged(EventArgs e)
    {
      if (this.CommentChanged == null)
        return;
      this.CommentChanged((object) this, e);
    }

    public event EventHandler CommentChanged;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.gcComments = new GroupContainer();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnDelete = new StandardIconButton();
      this.btnView = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.lstComments = new ListBoxVariable();
      this.tooltip = new ToolTip(this.components);
      this.mnuContext = new ContextMenuStrip(this.components);
      this.mnuItemDelete = new ToolStripMenuItem();
      this.mnuInternal = new ToolStripMenuItem();
      this.mnuExternal = new ToolStripMenuItem();
      this.gcComments.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnView).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.mnuContext.SuspendLayout();
      this.SuspendLayout();
      this.gcComments.Controls.Add((Control) this.pnlToolbar);
      this.gcComments.Controls.Add((Control) this.lstComments);
      this.gcComments.Dock = DockStyle.Fill;
      this.gcComments.HeaderForeColor = SystemColors.ControlText;
      this.gcComments.Location = new Point(0, 0);
      this.gcComments.Name = "gcComments";
      this.gcComments.Size = new Size(360, 287);
      this.gcComments.TabIndex = 0;
      this.gcComments.Text = "Comments";
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnDelete);
      this.pnlToolbar.Controls.Add((Control) this.btnView);
      this.pnlToolbar.Controls.Add((Control) this.btnAdd);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(276, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(80, 22);
      this.pnlToolbar.TabIndex = 2;
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(64, 3);
      this.btnDelete.Margin = new Padding(4, 3, 0, 3);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 17);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 28;
      this.btnDelete.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDelete, "Delete Comment");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnView.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnView.BackColor = Color.Transparent;
      this.btnView.Enabled = false;
      this.btnView.Location = new Point(44, 3);
      this.btnView.Margin = new Padding(4, 3, 0, 3);
      this.btnView.MouseDownImage = (Image) null;
      this.btnView.Name = "btnView";
      this.btnView.Size = new Size(16, 17);
      this.btnView.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.btnView.TabIndex = 27;
      this.btnView.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnView, "View Comment");
      this.btnView.Click += new EventHandler(this.btnView_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(24, 3);
      this.btnAdd.Margin = new Padding(4, 3, 0, 3);
      this.btnAdd.MouseDownImage = (Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 17);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 26;
      this.btnAdd.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAdd, "Add Comment");
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.lstComments.BorderStyle = BorderStyle.None;
      this.lstComments.Dock = DockStyle.Fill;
      this.lstComments.FormattingEnabled = true;
      this.lstComments.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.lstComments.ItemHeight = 14;
      this.lstComments.Location = new Point(1, 26);
      this.lstComments.Name = "lstComments";
      this.lstComments.Size = new Size(358, 260);
      this.lstComments.TabIndex = 1;
      this.lstComments.SelectedIndexChanged += new EventHandler(this.lstComments_SelectedIndexChanged);
      this.lstComments.ClientSizeChanged += new EventHandler(this.lstComments_ClientSizeChanged);
      this.lstComments.DoubleClick += new EventHandler(this.btnView_Click);
      this.mnuContext.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.mnuItemDelete,
        (ToolStripItem) this.mnuInternal,
        (ToolStripItem) this.mnuExternal
      });
      this.mnuContext.Name = "mnuDocuments";
      this.mnuContext.ShowImageMargin = false;
      this.mnuContext.Size = new Size(156, 92);
      this.mnuItemDelete.Enabled = false;
      this.mnuItemDelete.Name = "mnuItemDelete";
      this.mnuItemDelete.Size = new Size(123, 22);
      this.mnuItemDelete.Text = "Delete";
      this.mnuItemDelete.Click += new EventHandler(this.btnDelete_Click);
      this.mnuInternal.Enabled = false;
      this.mnuInternal.Name = "mnuInternal";
      this.mnuInternal.Size = new Size(155, 22);
      this.mnuInternal.Text = "Set as Internal";
      this.mnuInternal.Click += new EventHandler(this.mnuInternal_Click);
      this.mnuExternal.Enabled = false;
      this.mnuExternal.Name = "mnuExternal";
      this.mnuExternal.Size = new Size(123, 22);
      this.mnuExternal.Text = "Set as External";
      this.mnuExternal.Click += new EventHandler(this.mnuExternal_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcComments);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (CommentCollectionControl);
      this.Size = new Size(360, 287);
      this.gcComments.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnView).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.mnuContext.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}

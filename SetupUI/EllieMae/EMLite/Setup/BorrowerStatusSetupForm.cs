// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BorrowerStatusSetupForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class BorrowerStatusSetupForm : UserControl
  {
    private SetUpContainer setupContainer;
    private GroupContainer gcStatuses;
    private GridView lvExStatuses;
    private ContextMenuStrip ctxMenuStrip;
    private ToolStripMenuItem tsMenuItemNew;
    private ToolStripMenuItem tsMenuItemRename;
    private ToolStripMenuItem tsMenuItemUp;
    private ToolStripMenuItem tsMenuItemDown;
    private ToolStripMenuItem tsMenuItemDelete;
    private StandardIconButton stdIconBtnDelete;
    private StandardIconButton stdIconBtnDown;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnUp;
    private StandardIconButton stdIconBtnRename;
    private StandardIconButton stdIconBtnNew;
    private IContainer components;
    private Sessions.Session session;

    public BorrowerStatusSetupForm(SetUpContainer setupContainer)
      : this(setupContainer, Session.DefaultInstance, false)
    {
    }

    public BorrowerStatusSetupForm(
      SetUpContainer setupContainer,
      Sessions.Session session,
      bool allowMultiSelect)
    {
      this.InitializeComponent();
      this.session = session;
      this.lvExStatuses.Columns[0].ActivatedEditorType = GVActivatedEditorType.TextBox;
      this.lvExStatuses.AllowMultiselect = allowMultiSelect;
      this.setupContainer = setupContainer;
      this.reloadBorrowerStatus();
      this.lvExStatuses_SizeChanged((object) this, (EventArgs) null);
    }

    private void reloadBorrowerStatus()
    {
      BorrowerStatus borrowerStatus = this.session.ContactManager.GetBorrowerStatus();
      Array.Sort<BorrowerStatusItem>(borrowerStatus.Items);
      this.lvExStatuses.Items.Clear();
      foreach (BorrowerStatusItem borrowerStatusItem in borrowerStatus.Items)
        this.lvExStatuses.Items.Add(new GVItem(borrowerStatusItem.name.Trim()));
      this.updateBorrowerStatuses();
      this.lvExStatuses_SelectedIndexChanged((object) this, (EventArgs) null);
      this.setTitle();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn = new GVColumn();
      this.gcStatuses = new GroupContainer();
      this.stdIconBtnDown = new StandardIconButton();
      this.stdIconBtnUp = new StandardIconButton();
      this.stdIconBtnRename = new StandardIconButton();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.lvExStatuses = new GridView();
      this.ctxMenuStrip = new ContextMenuStrip(this.components);
      this.tsMenuItemNew = new ToolStripMenuItem();
      this.tsMenuItemRename = new ToolStripMenuItem();
      this.tsMenuItemUp = new ToolStripMenuItem();
      this.tsMenuItemDown = new ToolStripMenuItem();
      this.tsMenuItemDelete = new ToolStripMenuItem();
      this.toolTip1 = new ToolTip(this.components);
      this.gcStatuses.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnDown).BeginInit();
      ((ISupportInitialize) this.stdIconBtnUp).BeginInit();
      ((ISupportInitialize) this.stdIconBtnRename).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.ctxMenuStrip.SuspendLayout();
      this.SuspendLayout();
      this.gcStatuses.Controls.Add((Control) this.stdIconBtnDown);
      this.gcStatuses.Controls.Add((Control) this.stdIconBtnUp);
      this.gcStatuses.Controls.Add((Control) this.stdIconBtnRename);
      this.gcStatuses.Controls.Add((Control) this.stdIconBtnNew);
      this.gcStatuses.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcStatuses.Controls.Add((Control) this.lvExStatuses);
      this.gcStatuses.Dock = DockStyle.Fill;
      this.gcStatuses.Location = new Point(0, 0);
      this.gcStatuses.Name = "gcStatuses";
      this.gcStatuses.Size = new Size(528, 378);
      this.gcStatuses.TabIndex = 17;
      this.gcStatuses.Text = "Statuses (0)";
      this.stdIconBtnDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDown.BackColor = Color.Transparent;
      this.stdIconBtnDown.Location = new Point(485, 5);
      this.stdIconBtnDown.Name = "stdIconBtnDown";
      this.stdIconBtnDown.Size = new Size(16, 16);
      this.stdIconBtnDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdIconBtnDown.TabIndex = 5;
      this.stdIconBtnDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDown, "Move Down");
      this.stdIconBtnDown.Click += new EventHandler(this.btnDown_Click);
      this.stdIconBtnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnUp.BackColor = Color.Transparent;
      this.stdIconBtnUp.Location = new Point(463, 5);
      this.stdIconBtnUp.Name = "stdIconBtnUp";
      this.stdIconBtnUp.Size = new Size(16, 16);
      this.stdIconBtnUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdIconBtnUp.TabIndex = 4;
      this.stdIconBtnUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnUp, "Move Up");
      this.stdIconBtnUp.Click += new EventHandler(this.btnUp_Click);
      this.stdIconBtnRename.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnRename.BackColor = Color.Transparent;
      this.stdIconBtnRename.Location = new Point(441, 5);
      this.stdIconBtnRename.Name = "stdIconBtnRename";
      this.stdIconBtnRename.Size = new Size(16, 16);
      this.stdIconBtnRename.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnRename.TabIndex = 3;
      this.stdIconBtnRename.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnRename, "Rename Status");
      this.stdIconBtnRename.Click += new EventHandler(this.btnRename_Click);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(419, 5);
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 2;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "New Status");
      this.stdIconBtnNew.Click += new EventHandler(this.btnNew_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(507, 5);
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 1;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete Status");
      this.stdIconBtnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.lvExStatuses.AllowColumnReorder = true;
      this.lvExStatuses.AllowMultiselect = false;
      this.lvExStatuses.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.Text = "Name";
      gvColumn.Width = 500;
      this.lvExStatuses.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.lvExStatuses.ContextMenuStrip = this.ctxMenuStrip;
      this.lvExStatuses.Dock = DockStyle.Fill;
      this.lvExStatuses.HeaderHeight = 0;
      this.lvExStatuses.HeaderVisible = false;
      this.lvExStatuses.Location = new Point(1, 26);
      this.lvExStatuses.Name = "lvExStatuses";
      this.lvExStatuses.Size = new Size(526, 351);
      this.lvExStatuses.SortOption = GVSortOption.None;
      this.lvExStatuses.TabIndex = 0;
      this.lvExStatuses.SizeChanged += new EventHandler(this.lvExStatuses_SizeChanged);
      this.lvExStatuses.SelectedIndexChanged += new EventHandler(this.lvExStatuses_SelectedIndexChanged);
      this.lvExStatuses.EditorClosing += new GVSubItemEditingEventHandler(this.lvExStatuses_EditorClosing);
      this.lvExStatuses.ItemDoubleClick += new GVItemEventHandler(this.lvExStatuses_DoubleClick);
      this.ctxMenuStrip.Items.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.tsMenuItemNew,
        (ToolStripItem) this.tsMenuItemRename,
        (ToolStripItem) this.tsMenuItemUp,
        (ToolStripItem) this.tsMenuItemDown,
        (ToolStripItem) this.tsMenuItemDelete
      });
      this.ctxMenuStrip.Name = "contextMenuStrip1";
      this.ctxMenuStrip.Size = new Size(114, 114);
      this.tsMenuItemNew.Name = "tsMenuItemNew";
      this.tsMenuItemNew.Size = new Size(113, 22);
      this.tsMenuItemNew.Text = "New";
      this.tsMenuItemNew.Click += new EventHandler(this.btnNew_Click);
      this.tsMenuItemRename.Name = "tsMenuItemRename";
      this.tsMenuItemRename.Size = new Size(113, 22);
      this.tsMenuItemRename.Text = "Rename";
      this.tsMenuItemRename.Click += new EventHandler(this.btnRename_Click);
      this.tsMenuItemUp.Name = "tsMenuItemUp";
      this.tsMenuItemUp.Size = new Size(113, 22);
      this.tsMenuItemUp.Text = "Up";
      this.tsMenuItemUp.Click += new EventHandler(this.btnUp_Click);
      this.tsMenuItemDown.Name = "tsMenuItemDown";
      this.tsMenuItemDown.Size = new Size(113, 22);
      this.tsMenuItemDown.Text = "Down";
      this.tsMenuItemDown.Click += new EventHandler(this.btnDown_Click);
      this.tsMenuItemDelete.Name = "tsMenuItemDelete";
      this.tsMenuItemDelete.Size = new Size(113, 22);
      this.tsMenuItemDelete.Text = "Delete";
      this.tsMenuItemDelete.Click += new EventHandler(this.btnDelete_Click);
      this.Controls.Add((Control) this.gcStatuses);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (BorrowerStatusSetupForm);
      this.Size = new Size(528, 378);
      this.gcStatuses.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnDown).EndInit();
      ((ISupportInitialize) this.stdIconBtnUp).EndInit();
      ((ISupportInitialize) this.stdIconBtnRename).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ctxMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void setTitle()
    {
      this.gcStatuses.Text = "Statuses (" + (object) this.lvExStatuses.Items.Count + ")";
    }

    private bool duplicate(string statusName)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.lvExStatuses.Items)
      {
        if (gvItem.Text.Trim() == statusName.Trim())
          return true;
      }
      return false;
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      this.lvExStatuses.SelectedItems.Clear();
      int num = 1;
      string str = "New Status";
      while (this.duplicate(str))
        str = "New Status (" + (object) ++num + ")";
      GVItem gvItem = new GVItem(str);
      this.lvExStatuses.Items.Add(gvItem);
      this.session.ContactManager.CreateBorrowerStatus(new BorrowerStatusItem(str, this.lvExStatuses.Items.Count - 1));
      this.setTitle();
      gvItem.Selected = true;
      gvItem.BeginEdit();
    }

    private void updateBorrowerStatuses()
    {
      BorrowerStatusItem[] items = new BorrowerStatusItem[this.lvExStatuses.Items.Count];
      for (int index = 0; index < this.lvExStatuses.Items.Count; ++index)
        items[index] = new BorrowerStatusItem(this.lvExStatuses.Items[index].Text.Trim(), index);
      this.session.ContactManager.SetBorrowerStatus(items);
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.lvExStatuses.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.setupContainer, "Please select an item in the list to delete.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        string text = this.lvExStatuses.SelectedItems[0].Text;
        if (DialogResult.Cancel == MessageBox.Show("Once the selected status is deleted, all the borrower contacts in that status will have a blank status. Are you sure that you want to delete the status " + text + "?", "Delete Item", MessageBoxButtons.OKCancel))
          return;
        this.lvExStatuses.Items.RemoveAt(this.lvExStatuses.SelectedItems[0].Index);
        this.updateBorrowerStatuses();
        this.session.ContactManager.RenameStatusInBorrowerTable(text, string.Empty);
        this.setTitle();
      }
    }

    private void btnRename_Click(object sender, EventArgs e)
    {
      if (this.lvExStatuses.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.setupContainer, "Please select a borrower contact status in the list to rename.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.lvExStatuses.SelectedItems[0].BeginEdit();
    }

    private bool rename(GVItem itemToEdit, string newName)
    {
      if (this.duplicate(newName))
      {
        int num = (int) Utils.Dialog((IWin32Window) this.setupContainer, "The status name you entered already exists. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (newName.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this.setupContainer, "The status name cannot be empty. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      string str = itemToEdit.Text.Trim();
      this.session.ContactManager.UpdateBorrowerStatusItem(itemToEdit.Index, new BorrowerStatusItem(newName, itemToEdit.Index), str);
      this.session.ContactManager.RenameStatusInBorrowerTable(str, newName);
      return true;
    }

    private void lvExStatuses_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      GVItem selectedItem = this.lvExStatuses.SelectedItems[0];
      if (selectedItem.Text == e.EditorControl.Text || !this.rename(selectedItem, e.EditorControl.Text))
        e.Handled = true;
      selectedItem.Selected = true;
    }

    private void btnUp_Click(object sender, EventArgs e)
    {
      if (this.lvExStatuses.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.setupContainer, "Please select a borrower contact status in the list to move.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int index = this.lvExStatuses.SelectedItems[0].Index;
        if (index == 0)
          return;
        GVItem selectedItem = this.lvExStatuses.SelectedItems[0];
        this.swapItems(index - 1);
        selectedItem.Selected = true;
      }
    }

    private void btnDown_Click(object sender, EventArgs e)
    {
      if (this.lvExStatuses.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.setupContainer, "Please select a borrower contact status in the list to move.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int index = this.lvExStatuses.SelectedItems[0].Index;
        if (index == this.lvExStatuses.Items.Count - 1)
          return;
        GVItem selectedItem = this.lvExStatuses.SelectedItems[0];
        this.swapItems(index);
        selectedItem.Selected = true;
      }
    }

    private void swapItems(int index1)
    {
      GVItem gvItem = this.lvExStatuses.Items[index1];
      this.lvExStatuses.Items.RemoveAt(index1);
      this.lvExStatuses.Items.Insert(index1 + 1, gvItem);
      this.updateBorrowerStatuses();
    }

    private void lvExStatuses_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnDelete.Enabled = this.stdIconBtnRename.Enabled = this.stdIconBtnUp.Enabled = this.stdIconBtnDown.Enabled = this.lvExStatuses.SelectedItems.Count == 1;
      this.tsMenuItemDelete.Enabled = this.tsMenuItemRename.Enabled = this.tsMenuItemUp.Enabled = this.tsMenuItemDown.Enabled = this.lvExStatuses.SelectedItems.Count == 1;
    }

    private void lvExStatuses_DoubleClick(object sender, GVItemEventArgs e)
    {
    }

    private void lvExStatuses_SizeChanged(object sender, EventArgs e)
    {
      this.lvExStatuses.Columns[0].Width = this.lvExStatuses.Width - 5;
    }

    public string[] SelectedStatus
    {
      get
      {
        return this.lvExStatuses.SelectedItems.Count == 0 ? (string[]) null : this.lvExStatuses.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.Text)).ToArray<string>();
      }
      set
      {
        for (int index = 0; index < value.Length; ++index)
        {
          for (int nItemIndex = 0; nItemIndex < this.lvExStatuses.Items.Count; ++nItemIndex)
          {
            if (this.lvExStatuses.Items[nItemIndex].Text == value[index])
            {
              this.lvExStatuses.Items[nItemIndex].Selected = true;
              break;
            }
          }
        }
      }
    }
  }
}

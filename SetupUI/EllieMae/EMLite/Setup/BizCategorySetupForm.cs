// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BizCategorySetupForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI;
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
  public class BizCategorySetupForm : UserControl
  {
    private const int MAX_CATEGORYNAME_LENGTH = 50;
    private string[] defaultCategoryNames;
    private BizCategoryUtil bizCategories;
    private SetUpContainer setupContainer;
    private ToolTip toolTip1;
    private GroupContainer gcBizCategories;
    private GridView lvExCategories;
    private StandardIconButton stdIconBtnRename;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnDelete;
    private ContextMenuStrip ctxMenuStrip;
    private ToolStripMenuItem tsMenuItemNew;
    private ToolStripMenuItem tsMenuItemRename;
    private ToolStripMenuItem tsMenuItemDelete;
    private IContainer components;
    private Sessions.Session session;

    public BizCategorySetupForm(SetUpContainer setupContainer)
      : this(setupContainer, Session.DefaultInstance, false)
    {
    }

    public BizCategorySetupForm(
      SetUpContainer setupContainer,
      Sessions.Session session,
      bool allowMultiSelect)
    {
      this.session = session;
      this.bizCategories = new BizCategoryUtil(this.session.SessionObjects);
      this.InitializeComponent();
      this.lvExCategories.Columns[0].ActivatedEditorType = GVActivatedEditorType.TextBox;
      this.setupContainer = setupContainer;
      this.reloadCustomCategories();
      this.lvExCategories_SelectedIndexChanged((object) this, (EventArgs) null);
      this.lvExCategories.AllowMultiselect = allowMultiSelect;
      this.setTitle();
    }

    private void reloadCustomCategories()
    {
      if (this.defaultCategoryNames == null)
      {
        this.defaultCategoryNames = BusinessCategoryEnumUtil.GetDisplayNames();
        Array.Sort<string>(this.defaultCategoryNames);
      }
      BizCategory[] bizCategories = this.session.ContactManager.GetBizCategories();
      Array.Sort<BizCategory>(bizCategories);
      this.lvExCategories.Items.Clear();
      foreach (BizCategory bizCategory in bizCategories)
      {
        if (0 > Array.BinarySearch<string>(this.defaultCategoryNames, bizCategory.Name))
          this.lvExCategories.Items.Add(new GVItem(bizCategory.Name)
          {
            Tag = (object) bizCategory.CategoryID
          });
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
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn = new GVColumn();
      this.toolTip1 = new ToolTip(this.components);
      this.stdIconBtnDelete = new StandardIconButton();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnRename = new StandardIconButton();
      this.gcBizCategories = new GroupContainer();
      this.lvExCategories = new GridView();
      this.ctxMenuStrip = new ContextMenuStrip(this.components);
      this.tsMenuItemNew = new ToolStripMenuItem();
      this.tsMenuItemRename = new ToolStripMenuItem();
      this.tsMenuItemDelete = new ToolStripMenuItem();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnRename).BeginInit();
      this.gcBizCategories.SuspendLayout();
      this.ctxMenuStrip.SuspendLayout();
      this.SuspendLayout();
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(495, 5);
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 1;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete Category");
      this.stdIconBtnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(451, 5);
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 2;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "New Category");
      this.stdIconBtnNew.Click += new EventHandler(this.btnNew_Click);
      this.stdIconBtnRename.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnRename.BackColor = Color.Transparent;
      this.stdIconBtnRename.Location = new Point(473, 5);
      this.stdIconBtnRename.Name = "stdIconBtnRename";
      this.stdIconBtnRename.Size = new Size(16, 16);
      this.stdIconBtnRename.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnRename.TabIndex = 3;
      this.stdIconBtnRename.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnRename, "Rename Category");
      this.stdIconBtnRename.Click += new EventHandler(this.btnRename_Click);
      this.gcBizCategories.Controls.Add((Control) this.stdIconBtnRename);
      this.gcBizCategories.Controls.Add((Control) this.stdIconBtnNew);
      this.gcBizCategories.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcBizCategories.Controls.Add((Control) this.lvExCategories);
      this.gcBizCategories.Dock = DockStyle.Fill;
      this.gcBizCategories.Location = new Point(0, 0);
      this.gcBizCategories.Name = "gcBizCategories";
      this.gcBizCategories.Size = new Size(519, 378);
      this.gcBizCategories.TabIndex = 11;
      this.gcBizCategories.Text = "Categories (0)";
      this.lvExCategories.AllowMultiselect = false;
      this.lvExCategories.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.Text = "Name";
      gvColumn.Width = 500;
      this.lvExCategories.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.lvExCategories.ContextMenuStrip = this.ctxMenuStrip;
      this.lvExCategories.Dock = DockStyle.Fill;
      this.lvExCategories.HeaderHeight = 0;
      this.lvExCategories.HeaderVisible = false;
      this.lvExCategories.Location = new Point(1, 26);
      this.lvExCategories.Name = "lvExCategories";
      this.lvExCategories.Size = new Size(517, 351);
      this.lvExCategories.SortOption = GVSortOption.None;
      this.lvExCategories.TabIndex = 0;
      this.lvExCategories.SizeChanged += new EventHandler(this.lvExCategories_SizeChanged);
      this.lvExCategories.SelectedIndexChanged += new EventHandler(this.lvExCategories_SelectedIndexChanged);
      this.lvExCategories.EditorClosing += new GVSubItemEditingEventHandler(this.lvExCategories_EditorClosing);
      this.lvExCategories.ItemDoubleClick += new GVItemEventHandler(this.lvExCategories_DoubleClick);
      this.ctxMenuStrip.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.tsMenuItemNew,
        (ToolStripItem) this.tsMenuItemRename,
        (ToolStripItem) this.tsMenuItemDelete
      });
      this.ctxMenuStrip.Name = "ctxMenuStrip";
      this.ctxMenuStrip.Size = new Size(114, 70);
      this.tsMenuItemNew.Name = "tsMenuItemNew";
      this.tsMenuItemNew.Size = new Size(113, 22);
      this.tsMenuItemNew.Text = "New";
      this.tsMenuItemNew.Click += new EventHandler(this.btnNew_Click);
      this.tsMenuItemRename.Name = "tsMenuItemRename";
      this.tsMenuItemRename.Size = new Size(113, 22);
      this.tsMenuItemRename.Text = "Rename";
      this.tsMenuItemRename.Click += new EventHandler(this.btnRename_Click);
      this.tsMenuItemDelete.Name = "tsMenuItemDelete";
      this.tsMenuItemDelete.Size = new Size(113, 22);
      this.tsMenuItemDelete.Text = "Delete";
      this.tsMenuItemDelete.Click += new EventHandler(this.btnDelete_Click);
      this.Controls.Add((Control) this.gcBizCategories);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (BizCategorySetupForm);
      this.Size = new Size(519, 378);
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnRename).EndInit();
      this.gcBizCategories.ResumeLayout(false);
      this.ctxMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void setTitle()
    {
      this.gcBizCategories.Text = "Categories (" + (object) this.lvExCategories.Items.Count + ")";
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      this.lvExCategories.SelectedItems.Clear();
      int num = 1;
      string str = "New Category";
      while (this.duplicate(str))
        str = "New Category (" + (object) ++num + ")";
      BizCategory bizCategory = this.session.ContactManager.AddBizCategory(str);
      GVItem gvItem = new GVItem(bizCategory.Name);
      gvItem.Tag = (object) bizCategory.CategoryID;
      this.lvExCategories.Items.Add(gvItem);
      this.setTitle();
      gvItem.Selected = true;
      gvItem.BeginEdit();
    }

    private bool duplicate(string catName)
    {
      string str = catName.Replace(" ", "");
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.lvExCategories.Items)
      {
        if (string.Compare(gvItem.Text.Replace(" ", ""), str, StringComparison.OrdinalIgnoreCase) == 0)
          return true;
      }
      foreach (string defaultCategoryName in this.defaultCategoryNames)
      {
        if (string.Compare(str, defaultCategoryName.Replace(" ", ""), StringComparison.OrdinalIgnoreCase) == 0)
          return true;
      }
      return false;
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.lvExCategories.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.setupContainer, "Please select an item in the list to delete.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int tag = (int) this.lvExCategories.SelectedItems[0].Tag;
        string text = this.lvExCategories.SelectedItems[0].Text;
        if (DialogResult.Cancel == MessageBox.Show("Once the selected category is deleted, all the business contacts under that category will be under the 'No Category' category. Are you sure that you want to delete the category " + text + "?", "Delete Item", MessageBoxButtons.OKCancel))
          return;
        this.session.ContactManager.DeleteBizCategory(new BizCategory(tag, text));
        this.lvExCategories.Items.RemoveAt(this.lvExCategories.SelectedItems[0].Index);
        this.setTitle();
      }
    }

    private void btnRename_Click(object sender, EventArgs e)
    {
      if (this.lvExCategories.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.setupContainer, "Please select a category in the list to rename.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.lvExCategories.SelectedItems[0].BeginEdit();
    }

    private bool rename(GVItem itemToEdit, string newName)
    {
      if (newName.Trim().Length > 50)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.setupContainer, "The category name you entered exceeds the maximun length (50). Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.duplicate(newName))
      {
        int num = (int) Utils.Dialog((IWin32Window) this.setupContainer, "The category name you entered already exists. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (newName.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this.setupContainer, "The category name cannot be empty. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (newName.Contains("_"))
      {
        int num = (int) Utils.Dialog((IWin32Window) this.setupContainer, "The category name cannot contain '_' character. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      this.session.ContactManager.UpdateBizCategory(new BizCategory((int) itemToEdit.Tag, newName));
      return true;
    }

    private void lvExCategories_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      GVItem selectedItem = this.lvExCategories.SelectedItems[0];
      if (selectedItem.Text == e.EditorControl.Text || !this.rename(selectedItem, e.EditorControl.Text))
        e.Handled = true;
      selectedItem.Selected = true;
    }

    private void lvExCategories_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnDelete.Enabled = this.stdIconBtnRename.Enabled = this.lvExCategories.SelectedItems.Count == 1;
      this.tsMenuItemDelete.Enabled = this.tsMenuItemRename.Enabled = this.lvExCategories.SelectedItems.Count == 1;
    }

    private void lvExCategories_SizeChanged(object sender, EventArgs e)
    {
      this.lvExCategories.Columns[0].Width = this.lvExCategories.Width - 5;
    }

    private void lvExCategories_DoubleClick(object sender, GVItemEventArgs e)
    {
    }

    public string[] SelectedBusinessCategories
    {
      get
      {
        return this.lvExCategories.SelectedItems.Count == 0 ? (string[]) null : this.lvExCategories.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.Text)).ToArray<string>();
      }
      set
      {
        for (int index = 0; index < value.Length; ++index)
        {
          for (int nItemIndex = 0; nItemIndex < this.lvExCategories.Items.Count; ++nItemIndex)
          {
            if (this.lvExCategories.Items[nItemIndex].Text == value[index])
            {
              this.lvExCategories.Items[nItemIndex].Selected = true;
              break;
            }
          }
        }
      }
    }
  }
}

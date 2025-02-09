// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.HelocPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
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
  public class HelocPanel : UserControl
  {
    private IContainer components;
    private Button duplicateBtn;
    private GridView listViewHeloc;
    private GroupContainer gContainer;
    private StandardIconButton stdIconBtnEdit;
    private ToolTip toolTip1;
    private StandardIconButton stdIconTableNew;
    private StandardIconButton stdIconBtnDelete;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem tsMenuItemNew;
    private ToolStripMenuItem tsMenuItemDuplicate;
    private ToolStripMenuItem tsMenuItemEdit;
    private ToolStripMenuItem tsMenuItemDelete;
    private StandardIconButton stdIconBtnDuplicate;
    private Sessions.Session session;

    public HelocPanel()
      : this(Session.DefaultInstance, false)
    {
    }

    public HelocPanel(Sessions.Session session, bool allowMultiSelect)
    {
      this.session = session;
      this.InitializeComponent();
      this.initForm();
      this.listViewHeloc.AllowMultiselect = allowMultiSelect;
      this.listViewHeloc.Sort(0, SortOrder.Ascending);
      this.listViewHeloc.SelectedIndexChanged += new EventHandler(this.listViewHeloc_SelectedIndexChanged);
      this.listViewHeloc_SelectedIndexChanged((object) null, (EventArgs) null);
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.duplicateBtn = new Button();
      this.listViewHeloc = new GridView();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.tsMenuItemNew = new ToolStripMenuItem();
      this.tsMenuItemDuplicate = new ToolStripMenuItem();
      this.tsMenuItemEdit = new ToolStripMenuItem();
      this.tsMenuItemDelete = new ToolStripMenuItem();
      this.gContainer = new GroupContainer();
      this.stdIconBtnDuplicate = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconTableNew = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.contextMenuStrip1.SuspendLayout();
      this.gContainer.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnDuplicate).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconTableNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.SuspendLayout();
      this.duplicateBtn.Location = new Point(0, 0);
      this.duplicateBtn.Name = "duplicateBtn";
      this.duplicateBtn.Size = new Size(75, 23);
      this.duplicateBtn.TabIndex = 0;
      this.listViewHeloc.AllowMultiselect = false;
      this.listViewHeloc.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colTableName";
      gvColumn1.Text = "Table Name";
      gvColumn1.Width = 400;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colTableType";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Table Type";
      gvColumn2.Width = 84;
      this.listViewHeloc.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.listViewHeloc.ContextMenuStrip = this.contextMenuStrip1;
      this.listViewHeloc.Dock = DockStyle.Fill;
      this.listViewHeloc.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewHeloc.Location = new Point(1, 26);
      this.listViewHeloc.Name = "listViewHeloc";
      this.listViewHeloc.Size = new Size(484, 354);
      this.listViewHeloc.TabIndex = 7;
      this.listViewHeloc.ItemDoubleClick += new GVItemEventHandler(this.listViewHeloc_ItemDoubleClick);
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.tsMenuItemNew,
        (ToolStripItem) this.tsMenuItemDuplicate,
        (ToolStripItem) this.tsMenuItemEdit,
        (ToolStripItem) this.tsMenuItemDelete
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new Size(125, 92);
      this.tsMenuItemNew.Name = "tsMenuItemNew";
      this.tsMenuItemNew.Size = new Size(124, 22);
      this.tsMenuItemNew.Text = "New";
      this.tsMenuItemNew.Click += new EventHandler(this.newBtn_Click);
      this.tsMenuItemDuplicate.Name = "tsMenuItemDuplicate";
      this.tsMenuItemDuplicate.Size = new Size(124, 22);
      this.tsMenuItemDuplicate.Text = "Duplicate";
      this.tsMenuItemDuplicate.Click += new EventHandler(this.duplicateBtn_Click);
      this.tsMenuItemEdit.Name = "tsMenuItemEdit";
      this.tsMenuItemEdit.Size = new Size(124, 22);
      this.tsMenuItemEdit.Text = "Edit";
      this.tsMenuItemEdit.Click += new EventHandler(this.editBtn_Click);
      this.tsMenuItemDelete.Name = "tsMenuItemDelete";
      this.tsMenuItemDelete.Size = new Size(124, 22);
      this.tsMenuItemDelete.Text = "Delete";
      this.tsMenuItemDelete.Click += new EventHandler(this.deleteBtn_Click);
      this.gContainer.Controls.Add((Control) this.stdIconBtnDuplicate);
      this.gContainer.Controls.Add((Control) this.stdIconBtnEdit);
      this.gContainer.Controls.Add((Control) this.stdIconTableNew);
      this.gContainer.Controls.Add((Control) this.stdIconBtnDelete);
      this.gContainer.Controls.Add((Control) this.listViewHeloc);
      this.gContainer.Dock = DockStyle.Fill;
      this.gContainer.HeaderForeColor = SystemColors.ControlText;
      this.gContainer.Location = new Point(0, 0);
      this.gContainer.Name = "gContainer";
      this.gContainer.Size = new Size(486, 381);
      this.gContainer.TabIndex = 8;
      this.gContainer.Text = "HELOC Table";
      this.stdIconBtnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDuplicate.BackColor = Color.Transparent;
      this.stdIconBtnDuplicate.Location = new Point(417, 5);
      this.stdIconBtnDuplicate.MouseDownImage = (Image) null;
      this.stdIconBtnDuplicate.Name = "stdIconBtnDuplicate";
      this.stdIconBtnDuplicate.Size = new Size(16, 16);
      this.stdIconBtnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.stdIconBtnDuplicate.TabIndex = 11;
      this.stdIconBtnDuplicate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDuplicate, "Duplicate");
      this.stdIconBtnDuplicate.Click += new EventHandler(this.duplicateBtn_Click);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(439, 5);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 10;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.editBtn_Click);
      this.stdIconTableNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconTableNew.BackColor = Color.Transparent;
      this.stdIconTableNew.Location = new Point(395, 5);
      this.stdIconTableNew.MouseDownImage = (Image) null;
      this.stdIconTableNew.Name = "stdIconTableNew";
      this.stdIconTableNew.Size = new Size(16, 16);
      this.stdIconTableNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconTableNew.TabIndex = 9;
      this.stdIconTableNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconTableNew, "New");
      this.stdIconTableNew.Click += new EventHandler(this.newBtn_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(462, 5);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 8;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.deleteBtn_Click);
      this.Controls.Add((Control) this.gContainer);
      this.Name = nameof (HelocPanel);
      this.Size = new Size(486, 381);
      this.contextMenuStrip1.ResumeLayout(false);
      this.gContainer.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnDuplicate).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconTableNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ResumeLayout(false);
    }

    private void setContainerHeader()
    {
      this.gContainer.Text = "HELOC Historical Index Table (" + (object) this.listViewHeloc.Items.Count + ")";
    }

    private void initForm()
    {
      this.listViewHeloc.Items.Clear();
      this.loadTablesIntoView(this.session.ConfigurationManager.GetHelocTableDirEntries(), false);
      this.loadTablesIntoView(this.session.ConfigurationManager.GetHelocTableDirEntries(true), true);
      this.setContainerHeader();
    }

    private void loadTablesIntoView(FileSystemEntry[] entries, bool useNewTable)
    {
      if (entries == null)
        return;
      for (int index = 0; index < entries.Length; ++index)
        this.listViewHeloc.Items.Add(new GVItem(FileSystem.DecodeFilename(entries[index].Name))
        {
          SubItems = {
            (object) this.tableTypeEnumValue(useNewTable)
          }
        });
    }

    private void newBtn_Click(object sender, EventArgs e)
    {
      bool flag = true;
      using (HELOCTableTypeSelectionForm typeSelectionForm = new HELOCTableTypeSelectionForm())
      {
        if (typeSelectionForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        flag = typeSelectionForm.UseNewTable;
      }
      using (HelocTableContainer helocTableContainer = new HelocTableContainer((HelocRateTable) null, "", false, this.session, flag))
      {
        if (helocTableContainer.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          HelocRateTable helocTable = helocTableContainer.HelocTable;
          if (this.session.ConfigurationManager.SaveHelocTable(helocTableContainer.TableName, (BinaryObject) (BinaryConvertibleObject) helocTable, flag))
            this.listViewHeloc.Items.Add(new GVItem(helocTableContainer.TableName)
            {
              SubItems = {
                (object) this.tableTypeEnumValue(flag)
              },
              Selected = true
            });
        }
      }
      this.setContainerHeader();
    }

    private void listViewHeloc_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editSelectedItem();
    }

    private void editBtn_Click(object sender, EventArgs e) => this.editSelectedItem();

    private void editSelectedItem()
    {
      if (this.listViewHeloc.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a HELOC table first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string text = this.listViewHeloc.SelectedItems[0].Text;
        bool useNewHELOCHistoricTable = this.isNewTableInView(this.listViewHeloc.SelectedItems[0]);
        using (HelocTableContainer helocTableContainer = new HelocTableContainer((HelocRateTable) this.session.ConfigurationManager.GetHelocTable(this.listViewHeloc.SelectedItems[0].Text, useNewHELOCHistoricTable), text, false, this.session, useNewHELOCHistoricTable))
        {
          if (helocTableContainer.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          HelocRateTable helocTable = helocTableContainer.HelocTable;
          if (helocTable == null || text != helocTableContainer.TableName && !this.session.ConfigurationManager.DeleteHelocTable(text, useNewHELOCHistoricTable) || !this.session.ConfigurationManager.SaveHelocTable(helocTableContainer.TableName, (BinaryObject) (BinaryConvertibleObject) helocTable, useNewHELOCHistoricTable))
            return;
          this.listViewHeloc.SelectedItems[0].Text = helocTableContainer.TableName;
          this.listViewHeloc.SelectedItems[0].Tag = (object) helocTable;
        }
      }
    }

    private bool isNewTableInView(GVItem item) => item.SubItems[1].Text == "Dynamic Table";

    private string tableTypeEnumValue(bool useNewTable)
    {
      return (useNewTable ? "Dynamic" : "Static") + " Table";
    }

    private void duplicateBtn_Click(object sender, EventArgs e)
    {
      if (this.listViewHeloc.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a HELOC table first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        bool flag = this.isNewTableInView(this.listViewHeloc.SelectedItems[0]);
        using (HelocTableContainer helocTableContainer = new HelocTableContainer((HelocRateTable) this.session.ConfigurationManager.GetHelocTable(this.listViewHeloc.SelectedItems[0].Text, flag), "", false, this.session, flag))
        {
          if (helocTableContainer.ShowDialog((IWin32Window) this) == DialogResult.OK)
          {
            HelocRateTable helocTable = helocTableContainer.HelocTable;
            if (this.session.ConfigurationManager.SaveHelocTable(helocTableContainer.TableName, (BinaryObject) (BinaryConvertibleObject) helocTable, flag))
              this.listViewHeloc.Items.Add(new GVItem(helocTableContainer.TableName)
              {
                SubItems = {
                  (object) this.tableTypeEnumValue(flag)
                },
                Selected = true
              });
          }
        }
        this.setContainerHeader();
      }
    }

    private void deleteBtn_Click(object sender, EventArgs e)
    {
      if (this.listViewHeloc.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a HELOC table first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        bool useNewHELOCHistoricTable = this.isNewTableInView(this.listViewHeloc.SelectedItems[0]);
        int index = this.listViewHeloc.SelectedItems[0].Index;
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected HELOC table?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        {
          if (!this.session.ConfigurationManager.DeleteHelocTable(this.listViewHeloc.SelectedItems[0].Text, useNewHELOCHistoricTable))
            return;
          this.listViewHeloc.Items.Remove(this.listViewHeloc.SelectedItems[0]);
        }
        if (this.listViewHeloc.Items.Count == 0)
          return;
        if (index + 1 > this.listViewHeloc.Items.Count)
          this.listViewHeloc.Items[this.listViewHeloc.Items.Count - 1].Selected = true;
        else
          this.listViewHeloc.Items[index].Selected = true;
        this.setContainerHeader();
      }
    }

    private void listView_DoubleClick(object sender, EventArgs e)
    {
      this.editBtn_Click((object) null, (EventArgs) null);
    }

    private void listViewHeloc_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnDuplicate.Enabled = this.stdIconBtnEdit.Enabled = this.listViewHeloc.SelectedItems.Count == 1;
      this.tsMenuItemDuplicate.Enabled = this.tsMenuItemEdit.Enabled = this.listViewHeloc.SelectedItems.Count == 1;
      this.stdIconBtnDelete.Enabled = this.tsMenuItemDelete.Enabled = this.listViewHeloc.SelectedItems.Count == 1;
    }

    public string[] SelectedTableName
    {
      get
      {
        return this.listViewHeloc.SelectedItems.Count == 0 ? (string[]) null : this.listViewHeloc.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.Text)).ToArray<string>();
      }
    }

    public void SetSelectedTableNames(List<string> selectedTableNames)
    {
      for (int index = 0; index < selectedTableNames.Count; ++index)
      {
        for (int nItemIndex = 0; nItemIndex < this.listViewHeloc.Items.Count; ++nItemIndex)
        {
          if (this.listViewHeloc.Items[nItemIndex].Text == selectedTableNames[index])
          {
            this.listViewHeloc.Items[nItemIndex].Selected = true;
            break;
          }
        }
      }
    }
  }
}

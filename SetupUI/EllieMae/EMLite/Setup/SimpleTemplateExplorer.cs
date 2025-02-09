// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SimpleTemplateExplorer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public abstract class SimpleTemplateExplorer : UserControl
  {
    private FileSystemEntry[] fsEntries;
    private string header;
    private Sessions.Session session;
    private IContainer components;
    private GridView lvwTemplates;
    private GroupContainer gContainer;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnDuplicate;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnDelete;
    private ToolTip toolTip1;

    public SimpleTemplateExplorer()
      : this(Session.DefaultInstance, false)
    {
    }

    public SimpleTemplateExplorer(Sessions.Session session, bool allowMultiSelect)
    {
      this.session = session;
      this.InitializeComponent();
      this.header = this.HeaderText;
      this.ConfigureTemplateListView(this.lvwTemplates);
      this.refreshTemplateList();
      this.lvwTemplates.AllowMultiselect = allowMultiSelect;
      this.lvwTemplates_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    protected abstract TemplateSettingsType TemplateType { get; }

    protected abstract string HeaderText { get; }

    protected abstract bool CreateNew();

    protected abstract bool Edit(BinaryObject template);

    protected abstract BinaryObject Duplicate(BinaryObject template);

    protected virtual void ConfigureTemplateListView(GridView listView)
    {
    }

    protected virtual void UpdateTemplateProperties(FileSystemEntry fsEntry)
    {
    }

    private void refreshTemplateList()
    {
      this.fsEntries = this.session.ConfigurationManager.GetAllPublicTemplateSettingsFileEntries(this.TemplateType, true);
      this.lvwTemplates.Items.Clear();
      foreach (FileSystemEntry fsEntry in this.fsEntries)
        this.lvwTemplates.Items.Add(this.createGVItem(fsEntry));
      this.setGroupContainerTitle();
    }

    private void setGroupContainerTitle()
    {
      this.gContainer.Text = this.header + " (" + (object) this.lvwTemplates.Items.Count + ")";
    }

    private GVItem createGVItem(FileSystemEntry e)
    {
      this.UpdateTemplateProperties(e);
      GVItem gvItem = new GVItem(e.Name);
      for (int index = 1; index < this.lvwTemplates.Columns.Count; ++index)
        gvItem.SubItems.Add((object) "");
      foreach (GVColumn column in this.lvwTemplates.Columns)
      {
        if (column.Index > 0 && column.Tag != null)
          gvItem.SubItems[column.Index].Text = this.TemplateType != TemplateSettingsType.FundingTemplate || !(string.Concat(column.Tag) == "For2010GFE") ? string.Concat(e.Properties[(object) string.Concat(column.Tag)]) : (!e.Properties.ContainsKey((object) "RESPAVERSION") ? "No" : (!(string.Concat(e.Properties[(object) "RESPAVERSION"]) == "2015") ? "No" : "Yes"));
      }
      gvItem.Tag = (object) e;
      return gvItem;
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      if (!this.CreateNew())
        return;
      this.refreshTemplateList();
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.lvwTemplates.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select the template to edit from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.editCurrentTemplate();
    }

    private void btnDuplicate_Click(object sender, EventArgs e)
    {
      if (this.lvwTemplates.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select the template to duplicate from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        BinaryObject templateSettings = this.session.ConfigurationManager.GetTemplateSettings(this.TemplateType, (FileSystemEntry) this.lvwTemplates.SelectedItems[0].Tag);
        if (templateSettings == null)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The specified template cannot be opened.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          BinaryObject template = this.Duplicate(templateSettings);
          if (template == null || !this.Edit(template))
            return;
          this.refreshTemplateList();
        }
      }
    }

    private void editCurrentTemplate()
    {
      try
      {
        if (!this.Edit(this.session.ConfigurationManager.GetTemplateSettings(this.TemplateType, (FileSystemEntry) this.lvwTemplates.SelectedItems[0].Tag)))
          return;
        this.refreshTemplateList();
      }
      catch (ObjectNotFoundException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The specified template cannot be found. It has been renamed or deleted by another user.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.refreshTemplateList();
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.lvwTemplates.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select the template to delete from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        GVItem selectedItem = this.lvwTemplates.SelectedItems[0];
        FileSystemEntry tag = (FileSystemEntry) this.lvwTemplates.SelectedItems[0].Tag;
        if (Utils.Dialog((IWin32Window) this, "The template '" + tag.Name + "' will be permanently deleted.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.OK)
          return;
        try
        {
          this.session.ConfigurationManager.DeleteTemplateSettingsObject(this.TemplateType, tag);
        }
        catch (ObjectNotFoundException ex)
        {
        }
        this.lvwTemplates.SelectedItems.Clear();
        this.lvwTemplates.Items.Remove(selectedItem);
        this.setGroupContainerTitle();
      }
    }

    private void lvwTemplates_DoubleClick(object sender, GVItemEventArgs e)
    {
      this.editCurrentTemplate();
    }

    private void lvwTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.lvwTemplates.SelectedItems.Count;
      this.stdIconBtnDuplicate.Enabled = count == 1;
      this.stdIconBtnEdit.Enabled = count == 1;
      this.stdIconBtnDelete.Enabled = count == 1;
    }

    public string[] SelectedTemplates
    {
      get
      {
        return this.lvwTemplates.SelectedItems.Count == 0 ? (string[]) null : this.lvwTemplates.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.Text)).ToArray<string>();
      }
      set
      {
        for (int index = 0; index < value.Length; ++index)
        {
          for (int nItemIndex = 0; nItemIndex < this.lvwTemplates.Items.Count; ++nItemIndex)
          {
            if (this.lvwTemplates.Items[nItemIndex].Text == value[index])
            {
              this.lvwTemplates.Items[nItemIndex].Selected = true;
              break;
            }
          }
        }
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
      this.lvwTemplates = new GridView();
      this.gContainer = new GroupContainer();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnDuplicate = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.gContainer.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDuplicate).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.SuspendLayout();
      this.lvwTemplates.AllowMultiselect = false;
      this.lvwTemplates.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.Text = "Name";
      gvColumn.Width = 247;
      this.lvwTemplates.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.lvwTemplates.Dock = DockStyle.Fill;
      this.lvwTemplates.Location = new Point(1, 26);
      this.lvwTemplates.Name = "lvwTemplates";
      this.lvwTemplates.Size = new Size(558, 484);
      this.lvwTemplates.TabIndex = 1;
      this.lvwTemplates.SelectedIndexChanged += new EventHandler(this.lvwTemplates_SelectedIndexChanged);
      this.lvwTemplates.ItemDoubleClick += new GVItemEventHandler(this.lvwTemplates_DoubleClick);
      this.gContainer.Controls.Add((Control) this.stdIconBtnNew);
      this.gContainer.Controls.Add((Control) this.stdIconBtnDuplicate);
      this.gContainer.Controls.Add((Control) this.stdIconBtnEdit);
      this.gContainer.Controls.Add((Control) this.stdIconBtnDelete);
      this.gContainer.Controls.Add((Control) this.lvwTemplates);
      this.gContainer.Dock = DockStyle.Fill;
      this.gContainer.Location = new Point(0, 0);
      this.gContainer.Name = "gContainer";
      this.gContainer.Size = new Size(560, 511);
      this.gContainer.TabIndex = 2;
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(472, 5);
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 5;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.btnNew_Click);
      this.stdIconBtnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDuplicate.BackColor = Color.Transparent;
      this.stdIconBtnDuplicate.Location = new Point(494, 5);
      this.stdIconBtnDuplicate.Name = "stdIconBtnDuplicate";
      this.stdIconBtnDuplicate.Size = new Size(16, 16);
      this.stdIconBtnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.stdIconBtnDuplicate.TabIndex = 4;
      this.stdIconBtnDuplicate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDuplicate, "Duplicate");
      this.stdIconBtnDuplicate.Click += new EventHandler(this.btnDuplicate_Click);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(516, 5);
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 3;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(538, 5);
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 2;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gContainer);
      this.Name = nameof (SimpleTemplateExplorer);
      this.Size = new Size(560, 511);
      this.gContainer.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnDuplicate).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ResumeLayout(false);
    }
  }
}

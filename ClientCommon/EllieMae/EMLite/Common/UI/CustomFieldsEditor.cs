// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.CustomFieldsEditor
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class CustomFieldsEditor : UserControl
  {
    private CustomFieldsInfo customFields;
    private IContainer components;
    private GroupContainer gcCustomFields;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnDelete;
    private ToolTip toolTip1;
    private GridView lstVwExCustomFields;
    private ContextMenuStrip ctxMenuStrip;
    private ToolStripMenuItem tsMenuItemNew;
    private ToolStripMenuItem tsMenuItemEdit;
    private FlowLayoutPanel flowLayoutPanel1;
    private ToolStripMenuItem tsMenuItemDelete;
    private Sessions.Session session;

    public CustomFieldsEditor(Sessions.Session session, bool allowMultiselect)
    {
      this.session = session;
      this.InitializeComponent();
      this.lstVwExCustomFields.AllowMultiselect = allowMultiselect;
      if (this.session.EncompassEdition == EncompassEdition.Broker)
      {
        this.lstVwExCustomFields.Columns.RemoveAt(4);
        this.stdIconBtnNew.Visible = false;
        this.stdIconBtnDelete.Visible = false;
        this.tsMenuItemNew.Visible = false;
        this.tsMenuItemDelete.Visible = false;
      }
      this.Reset();
      this.lstVwExCustomFields.Sort(0, SortOrder.Ascending);
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
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      this.gcCustomFields = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.stdIconBtnDelete = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnNew = new StandardIconButton();
      this.lstVwExCustomFields = new GridView();
      this.ctxMenuStrip = new ContextMenuStrip(this.components);
      this.tsMenuItemNew = new ToolStripMenuItem();
      this.tsMenuItemEdit = new ToolStripMenuItem();
      this.tsMenuItemDelete = new ToolStripMenuItem();
      this.toolTip1 = new ToolTip(this.components);
      this.gcCustomFields.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      this.ctxMenuStrip.SuspendLayout();
      this.SuspendLayout();
      this.gcCustomFields.Controls.Add((Control) this.flowLayoutPanel1);
      this.gcCustomFields.Controls.Add((Control) this.lstVwExCustomFields);
      this.gcCustomFields.Dock = DockStyle.Fill;
      this.gcCustomFields.HeaderForeColor = SystemColors.ControlText;
      this.gcCustomFields.Location = new Point(0, 0);
      this.gcCustomFields.Name = "gcCustomFields";
      this.gcCustomFields.Size = new Size(786, 536);
      this.gcCustomFields.TabIndex = 14;
      this.gcCustomFields.Text = "Custom Fields (0)";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.stdIconBtnDelete);
      this.flowLayoutPanel1.Controls.Add((Control) this.stdIconBtnEdit);
      this.flowLayoutPanel1.Controls.Add((Control) this.stdIconBtnNew);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(710, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(74, 22);
      this.flowLayoutPanel1.TabIndex = 6;
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(55, 3);
      this.stdIconBtnDelete.Margin = new Padding(2, 3, 3, 3);
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 5;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.btnDeleteField_Click);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(34, 3);
      this.stdIconBtnEdit.Margin = new Padding(2, 3, 3, 3);
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 4;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.stdIconBtnEdit_Click);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(13, 3);
      this.stdIconBtnNew.Margin = new Padding(2, 3, 3, 3);
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 2;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.btnAddField_Click);
      this.lstVwExCustomFields.AllowColumnReorder = true;
      this.lstVwExCustomFields.AllowMultiselect = false;
      this.lstVwExCustomFields.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 96;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 179;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Format";
      gvColumn3.Width = 108;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Max Length";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 85;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Calculation Field?";
      gvColumn5.Width = 100;
      this.lstVwExCustomFields.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.lstVwExCustomFields.ContextMenuStrip = this.ctxMenuStrip;
      this.lstVwExCustomFields.Dock = DockStyle.Fill;
      this.lstVwExCustomFields.Location = new Point(1, 26);
      this.lstVwExCustomFields.Name = "lstVwExCustomFields";
      this.lstVwExCustomFields.Size = new Size(784, 509);
      this.lstVwExCustomFields.TabIndex = 3;
      this.lstVwExCustomFields.SelectedIndexChanged += new EventHandler(this.lstVwExCustomFields_SelectedIndexChanged);
      this.lstVwExCustomFields.KeyPress += new KeyPressEventHandler(this.lstVwExCustomFields_KeyPress);
      this.lstVwExCustomFields.ItemDoubleClick += new GVItemEventHandler(this.lstVwExCustomFields_ItemDoubleClick);
      this.ctxMenuStrip.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.tsMenuItemNew,
        (ToolStripItem) this.tsMenuItemEdit,
        (ToolStripItem) this.tsMenuItemDelete
      });
      this.ctxMenuStrip.Name = "ctxMenuStrip";
      this.ctxMenuStrip.Size = new Size(117, 70);
      this.tsMenuItemNew.Name = "tsMenuItemNew";
      this.tsMenuItemNew.Size = new Size(116, 22);
      this.tsMenuItemNew.Text = "New";
      this.tsMenuItemNew.Click += new EventHandler(this.btnAddField_Click);
      this.tsMenuItemEdit.Name = "tsMenuItemEdit";
      this.tsMenuItemEdit.Size = new Size(116, 22);
      this.tsMenuItemEdit.Text = "Edit";
      this.tsMenuItemEdit.Click += new EventHandler(this.stdIconBtnEdit_Click);
      this.tsMenuItemDelete.Name = "tsMenuItemDelete";
      this.tsMenuItemDelete.Size = new Size(116, 22);
      this.tsMenuItemDelete.Text = "Delete";
      this.tsMenuItemDelete.Click += new EventHandler(this.btnDeleteField_Click);
      this.Controls.Add((Control) this.gcCustomFields);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (CustomFieldsEditor);
      this.Size = new Size(786, 536);
      this.gcCustomFields.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      this.ctxMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void setTitle()
    {
      this.gcCustomFields.Text = "Custom Fields (" + (object) this.customFields.GetSortedFields().Count + ")";
    }

    public void Reset()
    {
      this.customFields = this.session.ConfigurationManager.GetLoanCustomFields(true);
      try
      {
        this.lstVwExCustomFields.Items.Clear();
        foreach (CustomFieldInfo customField in this.customFields)
        {
          GVItem gvItem = new GVItem(customField.FieldID);
          gvItem.SubItems.Add((object) customField.Description);
          gvItem.SubItems.Add((object) customField.Format.ToString());
          gvItem.SubItems.Add(customField.MaxLength == 0 ? (object) "" : (object) string.Concat((object) customField.MaxLength));
          if (this.stdIconBtnNew.Visible)
            gvItem.SubItems.Add(!customField.IsCalculationAllowed() || !((customField.Calculation ?? "").Trim() != "") ? (object) "" : (object) "Y");
          gvItem.Tag = (object) customField;
          this.lstVwExCustomFields.Items.Add(gvItem);
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error in refreshFieldList: " + (object) ex);
      }
      this.setTitle();
      this.lstVwExCustomFields_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    private void lstVwExCustomFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.tsMenuItemEdit.Enabled = this.stdIconBtnEdit.Enabled = this.lstVwExCustomFields.SelectedItems.Count == 1;
      if (this.lstVwExCustomFields.SelectedItems.Count == 1)
        this.tsMenuItemDelete.Enabled = this.stdIconBtnDelete.Enabled = ((CustomFieldInfo) this.lstVwExCustomFields.SelectedItems[0].Tag).IsExtendedField();
      else
        this.tsMenuItemDelete.Enabled = this.stdIconBtnDelete.Enabled = false;
    }

    public void btnAddField_Click(object sender, EventArgs e)
    {
      CustomFieldEditor customFieldEditor = new CustomFieldEditor(this.session, this.customFields, (CustomFieldInfo) null);
      if (customFieldEditor.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        return;
      CustomFieldInfo currentField = customFieldEditor.CurrentField;
      this.customFields = customFieldEditor.CustomFields;
      GVItem gvItem = new GVItem(currentField.FieldID);
      gvItem.SubItems.Add((object) currentField.Description);
      gvItem.SubItems.Add((object) currentField.Format.ToString());
      gvItem.SubItems.Add(currentField.MaxLength == 0 ? (object) "" : (object) string.Concat((object) currentField.MaxLength));
      if (this.stdIconBtnNew.Visible)
        gvItem.SubItems.Add(!currentField.IsCalculationAllowed() || !((currentField.Calculation ?? "").Trim() != "") ? (object) "" : (object) "Y");
      gvItem.Tag = (object) currentField;
      this.lstVwExCustomFields.Items.Add(gvItem);
      gvItem.Selected = true;
      this.setTitle();
    }

    private void lstVwExCustomFields_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editSelectedItem();
    }

    private void stdIconBtnEdit_Click(object sender, EventArgs e) => this.editSelectedItem();

    public void EditSelectedItem() => this.editSelectedItem();

    private void editSelectedItem()
    {
      if (this.lstVwExCustomFields.SelectedItems == null || this.lstVwExCustomFields.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a field to edit.");
      }
      else
      {
        CustomFieldEditor customFieldEditor = new CustomFieldEditor(this.session, this.customFields, this.lstVwExCustomFields.SelectedItems[0].Tag as CustomFieldInfo);
        if (customFieldEditor.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        CustomFieldInfo currentField = customFieldEditor.CurrentField;
        this.customFields = customFieldEditor.CustomFields;
        GVItem selectedItem = this.lstVwExCustomFields.SelectedItems[0];
        selectedItem.SubItems[0].Text = currentField.FieldID;
        selectedItem.SubItems[1].Text = currentField.Description;
        selectedItem.SubItems[2].Text = currentField.Format.ToString();
        selectedItem.SubItems[3].Text = currentField.MaxLength == 0 ? "" : string.Concat((object) currentField.MaxLength);
        if (this.stdIconBtnNew.Visible)
          selectedItem.SubItems[4].Text = !currentField.IsCalculationAllowed() || !((currentField.Calculation ?? "").Trim() != "") ? "" : "Y";
        selectedItem.Tag = (object) currentField;
      }
    }

    private void btnDeleteField_Click(object sender, EventArgs e)
    {
      if (this.lstVwExCustomFields.SelectedItems.Count != 1)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a field to delete.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        CustomFieldInfo tag = (CustomFieldInfo) this.lstVwExCustomFields.SelectedItems[0].Tag;
        if (!tag.IsExtendedField())
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Standard custom fields cannot be deleted.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          if (Utils.Dialog((IWin32Window) this, "Are you sure you wish to delete the custom field '" + tag.FieldID + "'?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;
          this.session.ConfigurationManager.DeleteLoanCustomField(tag.FieldID);
          this.customFields.Remove(tag);
          this.lstVwExCustomFields.Items.RemoveAt(this.lstVwExCustomFields.SelectedItems[0].Index);
          this.setTitle();
        }
      }
    }

    private void lstVwExCustomFields_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r' || this.lstVwExCustomFields.SelectedItems == null || this.lstVwExCustomFields.SelectedItems.Count == 0)
        return;
      this.editSelectedItem();
    }

    public string[] SelectedFieldIDs
    {
      get
      {
        if (this.lstVwExCustomFields.SelectedItems == null)
          return (string[]) null;
        List<string> stringList = new List<string>();
        foreach (GVItem selectedItem in this.lstVwExCustomFields.SelectedItems)
          stringList.Add(selectedItem.Text);
        return stringList.ToArray();
      }
      set
      {
        if (value == null || value.Length == 0)
          return;
        List<string> stringList = new List<string>((IEnumerable<string>) value);
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.lstVwExCustomFields.Items)
          gvItem.Selected = stringList.Contains(gvItem.Text);
      }
    }
  }
}

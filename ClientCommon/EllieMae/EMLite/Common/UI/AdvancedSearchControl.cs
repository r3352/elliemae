// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.AdvancedSearchControl
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class AdvancedSearchControl : UserControl
  {
    private bool allowDatabaseFieldsOnly = true;
    private bool allowDynamicOperators = true;
    private bool allowVirtualFields = true;
    private ReportFieldDefs fieldDefs;
    private bool modified;
    private bool readOnly;
    private string title;
    private IContainer components;
    private GridView gvFilters;
    private Button btnInsertFilter;
    private Button btnAndOr;
    private Button btnParenthesis;
    private Button btnSummary;
    private TableContainer grpFilters;
    private FlowLayoutPanel flpHeader;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton btnRemoveFilter;
    private StandardIconButton btnEditFilter;
    private StandardIconButton btnAddFilter;
    private Panel pnlFooter;
    private ToolTip toolTip1;
    private ContextMenuStrip mnuContext;
    private ToolStripMenuItem mnuAddFilter;
    private ToolStripMenuItem mnuEditFilter;
    private ToolStripMenuItem mnuRemoveFilter;
    private ToolStripSeparator toolStripMenuItem1;
    private ToolStripMenuItem mnuInsertFilter;
    private ToolStripMenuItem mnuAndOr;
    private ToolStripSeparator toolStripMenuItem2;
    private ToolStripMenuItem mnuParenthesis;
    private ToolStripMenuItem mnuViewFilter;
    private CheckBox chkTpoIncludeChildFolder;

    public event EventHandler DataChange;

    public event FieldFilterEventHandler FilterAdded;

    public event FieldFilterEventHandler FilterRemoved;

    public bool DDMSetting { get; set; }

    public AdvancedSearchControl()
    {
      this.InitializeComponent();
      this.title = this.grpFilters.Text;
      this.gvFilters.Items.Change += new EventHandler(this.onListItemsChange);
      this.chkTpoIncludeChildFolder.Visible = false;
      this.DDMSetting = false;
    }

    private void onListItemsChange(object sender, EventArgs e)
    {
      if (this.gvFilters.Items.Count > 0)
        this.grpFilters.Text = this.title + " (" + (object) this.gvFilters.Items.Count + ")";
      else
        this.grpFilters.Text = this.title;
    }

    [Category("Behavior")]
    [DefaultValue(true)]
    public bool AllowDatabaseFieldsOnly
    {
      get => this.allowDatabaseFieldsOnly;
      set => this.allowDatabaseFieldsOnly = value;
    }

    [Category("Behavior")]
    [DefaultValue(true)]
    public bool AllowDynamicOperators
    {
      get => this.allowDynamicOperators;
      set => this.allowDynamicOperators = value;
    }

    [Category("Behavior")]
    [DefaultValue(true)]
    public bool AllowVirtualFields
    {
      get => this.allowVirtualFields;
      set => this.allowVirtualFields = value;
    }

    [Category("Appearance")]
    [DefaultValue(false)]
    public bool FooterVisible
    {
      get => this.grpFilters.Style == TableContainer.ContainerStyle.HeaderAndFooter;
      set
      {
        this.grpFilters.Style = value ? TableContainer.ContainerStyle.HeaderAndFooter : TableContainer.ContainerStyle.HeaderOnly;
        this.pnlFooter.Visible = value;
        if (!value)
          return;
        this.pnlFooter.Anchor = AnchorStyles.None;
        Panel pnlFooter1 = this.pnlFooter;
        Rectangle clientRectangle = this.pnlFooter.Parent.ClientRectangle;
        int num = clientRectangle.Height - this.pnlFooter.Height;
        pnlFooter1.Top = num;
        this.pnlFooter.Left = 0;
        Panel pnlFooter2 = this.pnlFooter;
        clientRectangle = this.pnlFooter.Parent.ClientRectangle;
        int width = clientRectangle.Width;
        pnlFooter2.Width = width;
        this.pnlFooter.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Control.ControlCollection FooterControls => this.pnlFooter.Controls;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool DataModified
    {
      get => this.modified;
      set => this.modified = value;
    }

    [Category("Behavior")]
    [DefaultValue(false)]
    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        if (this.readOnly == value)
          return;
        this.readOnly = value;
        this.setButtonStates();
      }
    }

    public void AddControlToHeader(Control c)
    {
      this.flpHeader.Controls.Add(c);
      c.BringToFront();
    }

    [Category("Appearance")]
    [DefaultValue("")]
    public string Title
    {
      get => this.title;
      set
      {
        this.grpFilters.Text = value;
        this.title = value;
      }
    }

    [Browsable(false)]
    public int FilterCount => this.gvFilters.Items.Count;

    [Category("Appearance")]
    [DefaultValue(AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right)]
    public AnchorStyles Borders
    {
      get => this.grpFilters.Borders;
      set => this.grpFilters.Borders = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ReportFieldDefs FieldDefs
    {
      get => this.fieldDefs;
      set => this.fieldDefs = value;
    }

    public void SetCurrentFilter(FieldFilterList filters)
    {
      this.ClearFilters();
      if (filters != null)
        this.AddFilters(filters);
      this.modified = false;
    }

    public void ClearFilters()
    {
      this.gvFilters.Items.Clear();
      this.setButtonStates();
      this.OnDataChange();
    }

    [Category("Behavior")]
    [DefaultValue(false)]
    public bool SetIncludeChildFolderVisible
    {
      get => this.chkTpoIncludeChildFolder.Visible;
      set => this.chkTpoIncludeChildFolder.Visible = value;
    }

    [Category("Behavior")]
    [DefaultValue(false)]
    public bool GetIncludeChildFodlerChecked
    {
      get => this.chkTpoIncludeChildFolder.Checked;
      set => this.chkTpoIncludeChildFolder.Checked = value;
    }

    public virtual void AddFilters(FieldFilterList filters)
    {
      foreach (FieldFilter filter in (List<FieldFilter>) filters)
        this.gvFilters.Items.Add(this.fieldFilterToGVItem(new FieldFilter(filter)));
      this.ensureJointConditionsSet();
      this.setButtonStates();
      this.OnDataChange();
    }

    public virtual FieldFilter AddFilter(FieldFilter filter)
    {
      FieldFilter filter1 = new FieldFilter(filter);
      this.gvFilters.Items.Add(this.fieldFilterToGVItem(filter1));
      this.ensureJointConditionsSet();
      this.setButtonStates();
      this.OnDataChange();
      return filter1;
    }

    private void ensureJointConditionsSet()
    {
      for (int nItemIndex = 0; nItemIndex < this.gvFilters.Items.Count; ++nItemIndex)
      {
        GVItem gvItem = this.gvFilters.Items[nItemIndex];
        FieldFilter tag = (FieldFilter) gvItem.Tag;
        if (nItemIndex < this.gvFilters.Items.Count - 1)
        {
          if (tag.JointToken == JointTokens.Nothing)
            tag.JointToken = JointTokens.And;
        }
        else
          tag.JointToken = JointTokens.Nothing;
        this.refreshGVItem(gvItem);
      }
    }

    public virtual FieldFilterList GetCurrentFilter()
    {
      FieldFilterList currentFilter = new FieldFilterList();
      for (int nItemIndex = 0; nItemIndex < this.gvFilters.Items.Count; ++nItemIndex)
      {
        FieldFilter tag = (FieldFilter) this.gvFilters.Items[nItemIndex].Tag;
        if (tag.Id == Guid.Empty)
          tag.Id = Guid.NewGuid();
        currentFilter.Add(tag);
      }
      return currentFilter;
    }

    private void setButtonStates()
    {
      this.btnAddFilter.Enabled = !this.readOnly;
      this.btnEditFilter.Enabled = !this.readOnly && this.gvFilters.SelectedItems.Count == 1;
      this.btnRemoveFilter.Enabled = !this.readOnly && this.gvFilters.SelectedItems.Count > 0;
      this.btnInsertFilter.Enabled = !this.readOnly && this.gvFilters.SelectedItems.Count == 1;
      this.btnAndOr.Enabled = !this.readOnly && this.gvFilters.SelectedItems.Count == 1;
      this.btnParenthesis.Enabled = !this.readOnly && this.gvFilters.Items.Count >= 2;
      this.btnSummary.Enabled = this.gvFilters.Items.Count >= 1;
    }

    private GVItem fieldFilterToGVItem(FieldFilter filter)
    {
      GVItem gvItem = new GVItem(filter.LeftParenthesesToString);
      gvItem.Tag = (object) filter;
      for (int index = 1; index < this.gvFilters.Columns.Count; ++index)
        gvItem.SubItems.Add((object) "");
      return gvItem;
    }

    private void refreshGVItem(GVItem item)
    {
      FieldFilter tag = (FieldFilter) item.Tag;
      item.SubItems[0].Text = tag.LeftParenthesesToString;
      item.SubItems[1].Text = tag.FieldDescription;
      item.SubItems[2].Text = tag.OperatorDescription;
      item.SubItems[3].Text = tag.ValueDescription;
      item.SubItems[4].Text = tag.RightParenthesesToString;
      if (item.Index == this.gvFilters.Items.Count - 1)
        item.SubItems[5].Text = "";
      else
        item.SubItems[5].Text = tag.JointTokenToString;
    }

    private void btnAddFilter_Click(object sender, EventArgs e)
    {
      using (AddEditFilterDialog editFilterDialog = new AddEditFilterDialog(this.fieldDefs, this.DDMSetting))
      {
        editFilterDialog.AllowDatabaseFieldsOnly = this.allowDatabaseFieldsOnly;
        editFilterDialog.AllowDynamicOperators = this.allowDynamicOperators;
        editFilterDialog.AllowVirtualFields = this.allowVirtualFields;
        if (editFilterDialog.ShowDialog((IWin32Window) this.ParentForm) != DialogResult.OK)
          return;
        this.OnFilterAdded(new FieldFilterEventArgs(this.AddFilter(editFilterDialog.SelectedFilter)));
      }
    }

    private void btnEditFilter_Click(object sender, EventArgs e)
    {
      if (this.gvFilters.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Select the criterion to be edited from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.editCurrentItem();
    }

    private void editCurrentItem()
    {
      FieldFilter tag = (FieldFilter) this.gvFilters.SelectedItems[0].Tag;
      using (AddEditFilterDialog editFilterDialog = new AddEditFilterDialog(this.fieldDefs, this.DDMSetting))
      {
        editFilterDialog.AllowDatabaseFieldsOnly = this.allowDatabaseFieldsOnly;
        editFilterDialog.AllowDynamicOperators = this.allowDynamicOperators;
        editFilterDialog.AllowVirtualFields = this.allowVirtualFields;
        editFilterDialog.SelectedFilter = tag;
        if (editFilterDialog.ShowDialog((IWin32Window) this.ParentForm) != DialogResult.OK)
          return;
        this.refreshGVItem(this.gvFilters.SelectedItems[0]);
        this.OnDataChange();
      }
    }

    private void btnRemoveFilter_Click(object sender, EventArgs e)
    {
      if (this.gvFilters.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Select the criterion to be removed from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Remove the selected criterion from the filter?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        FieldFilter tag1 = (FieldFilter) this.gvFilters.SelectedItems[0].Tag;
        int val1 = 0;
        if (tag1.RightParentheses > tag1.LeftParentheses)
        {
          int val2 = tag1.RightParentheses - tag1.LeftParentheses;
          for (int nItemIndex = this.gvFilters.SelectedItems[0].Index - 1; nItemIndex >= 0 && val2 > 0; --nItemIndex)
          {
            FieldFilter tag2 = (FieldFilter) this.gvFilters.Items[nItemIndex].Tag;
            int num2 = tag2.LeftParentheses - tag2.RightParentheses;
            val1 += num2;
            if (val1 > 0)
            {
              int num3 = Math.Min(val1, val2);
              tag2.LeftParentheses -= num3;
              this.refreshGVItem(this.gvFilters.Items[nItemIndex]);
              val2 -= num3;
              val1 -= num3;
            }
          }
        }
        else if (tag1.LeftParentheses > tag1.RightParentheses)
        {
          int val2 = tag1.LeftParentheses - tag1.RightParentheses;
          for (int nItemIndex = this.gvFilters.SelectedItems[0].Index + 1; nItemIndex < this.gvFilters.Items.Count && val2 > 0; ++nItemIndex)
          {
            FieldFilter tag3 = (FieldFilter) this.gvFilters.Items[nItemIndex].Tag;
            int num4 = tag3.RightParentheses - tag3.LeftParentheses;
            val1 += num4;
            if (val1 > 0)
            {
              int num5 = Math.Min(val1, val2);
              tag3.RightParentheses -= num5;
              this.refreshGVItem(this.gvFilters.Items[nItemIndex]);
              val2 -= num5;
              val1 -= num5;
            }
          }
        }
        this.gvFilters.Items.Remove(this.gvFilters.SelectedItems[0]);
        this.ensureJointConditionsSet();
        if (this.gvFilters.Items.Count > 0)
          this.refreshGVItem(this.gvFilters.Items[this.gvFilters.Items.Count - 1]);
        this.OnDataChange();
        this.OnFilterRemoved(new FieldFilterEventArgs(tag1));
      }
    }

    private void btnInsertFilter_Click(object sender, EventArgs e)
    {
      if (this.gvFilters.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Select the criterion before which the new element will be inserted.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        using (AddEditFilterDialog editFilterDialog = new AddEditFilterDialog(this.fieldDefs))
        {
          editFilterDialog.AllowDatabaseFieldsOnly = this.allowDatabaseFieldsOnly;
          editFilterDialog.AllowDynamicOperators = this.allowDynamicOperators;
          editFilterDialog.AllowVirtualFields = this.allowVirtualFields;
          if (editFilterDialog.ShowDialog((IWin32Window) this.ParentForm) != DialogResult.OK)
            return;
          this.gvFilters.Items.Insert(this.gvFilters.SelectedItems[0].Index, this.fieldFilterToGVItem(editFilterDialog.SelectedFilter));
          this.ensureJointConditionsSet();
          this.OnDataChange();
          this.OnFilterAdded(new FieldFilterEventArgs(editFilterDialog.SelectedFilter));
        }
      }
    }

    private void btnAndOr_Click(object sender, EventArgs e)
    {
      if (this.gvFilters.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Select the criterion to be modified.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.gvFilters.SelectedItems[0].Index == this.gvFilters.Items.Count - 1)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "A joint condition cannot be applied to the last criterion in the filter.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        FieldFilter tag = (FieldFilter) this.gvFilters.SelectedItems[0].Tag;
        tag.JointToken = tag.JointToken == JointTokens.And ? JointTokens.Or : JointTokens.And;
        this.refreshGVItem(this.gvFilters.SelectedItems[0]);
        this.OnDataChange();
      }
    }

    private void btnParenthesis_Click(object sender, EventArgs e)
    {
      using (AddParenthesisDialog parenthesisDialog = new AddParenthesisDialog(this.GetCurrentFilter()))
      {
        int num = (int) parenthesisDialog.ShowDialog((IWin32Window) this);
      }
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFilters.Items)
        this.refreshGVItem(gvItem);
      this.OnDataChange();
    }

    private void gvFilters_DoubleClick(object sender, EventArgs e)
    {
      if (this.readOnly || this.gvFilters.HitTest(this.gvFilters.PointToClient(Cursor.Position)).RowIndex < 0)
        return;
      this.editCurrentItem();
    }

    protected void OnDataChange()
    {
      this.modified = true;
      if (this.DataChange == null)
        return;
      this.DataChange((object) this, EventArgs.Empty);
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Clear all criteria from the list?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      this.ClearFilters();
    }

    private void gvFilters_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setButtonStates();
    }

    private void btnSummary_Click(object sender, EventArgs e)
    {
      if (this.gvFilters.Items.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first specify at least one filter.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, this.GetFilterSummary(), MessageBoxButtons.OK, MessageBoxIcon.None);
      }
    }

    public string GetFilterSummary()
    {
      return AdvancedSearchControl.GetFilterSummary(this.GetCurrentFilter());
    }

    public static string GetFilterSummary(FieldFilterList filters) => filters.GetSummary();

    protected virtual void OnFilterAdded(FieldFilterEventArgs e)
    {
      if (this.FilterAdded == null)
        return;
      this.FilterAdded((object) this, e);
    }

    protected virtual void OnFilterRemoved(FieldFilterEventArgs e)
    {
      if (this.FilterRemoved == null)
        return;
      this.FilterRemoved((object) this, e);
    }

    private void mnuContext_Opening(object sender, CancelEventArgs e)
    {
      this.mnuAddFilter.Enabled = this.btnAddFilter.Enabled;
      this.mnuEditFilter.Enabled = this.btnEditFilter.Enabled;
      this.mnuRemoveFilter.Enabled = this.btnRemoveFilter.Enabled;
      this.mnuInsertFilter.Enabled = this.btnInsertFilter.Enabled;
      this.mnuAndOr.Enabled = this.btnAndOr.Enabled;
      this.mnuParenthesis.Enabled = this.btnParenthesis.Enabled;
      this.mnuViewFilter.Enabled = this.btnSummary.Enabled;
    }

    private void chkTpoIncludeChildFolder_CheckedChanged(object sender, EventArgs e)
    {
      this.OnDataChange();
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
      GVColumn gvColumn6 = new GVColumn();
      this.mnuContext = new ContextMenuStrip(this.components);
      this.mnuAddFilter = new ToolStripMenuItem();
      this.mnuEditFilter = new ToolStripMenuItem();
      this.mnuRemoveFilter = new ToolStripMenuItem();
      this.toolStripMenuItem1 = new ToolStripSeparator();
      this.mnuInsertFilter = new ToolStripMenuItem();
      this.mnuAndOr = new ToolStripMenuItem();
      this.toolStripMenuItem2 = new ToolStripSeparator();
      this.mnuParenthesis = new ToolStripMenuItem();
      this.mnuViewFilter = new ToolStripMenuItem();
      this.toolTip1 = new ToolTip(this.components);
      this.btnRemoveFilter = new StandardIconButton();
      this.btnEditFilter = new StandardIconButton();
      this.btnAddFilter = new StandardIconButton();
      this.grpFilters = new TableContainer();
      this.flpHeader = new FlowLayoutPanel();
      this.btnSummary = new Button();
      this.btnParenthesis = new Button();
      this.btnAndOr = new Button();
      this.btnInsertFilter = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.chkTpoIncludeChildFolder = new CheckBox();
      this.pnlFooter = new Panel();
      this.gvFilters = new GridView();
      this.mnuContext.SuspendLayout();
      ((ISupportInitialize) this.btnRemoveFilter).BeginInit();
      ((ISupportInitialize) this.btnEditFilter).BeginInit();
      ((ISupportInitialize) this.btnAddFilter).BeginInit();
      this.grpFilters.SuspendLayout();
      this.flpHeader.SuspendLayout();
      this.SuspendLayout();
      this.mnuContext.Items.AddRange(new ToolStripItem[9]
      {
        (ToolStripItem) this.mnuAddFilter,
        (ToolStripItem) this.mnuEditFilter,
        (ToolStripItem) this.mnuRemoveFilter,
        (ToolStripItem) this.toolStripMenuItem1,
        (ToolStripItem) this.mnuInsertFilter,
        (ToolStripItem) this.mnuAndOr,
        (ToolStripItem) this.toolStripMenuItem2,
        (ToolStripItem) this.mnuParenthesis,
        (ToolStripItem) this.mnuViewFilter
      });
      this.mnuContext.Name = "mnuContext";
      this.mnuContext.ShowImageMargin = false;
      this.mnuContext.Size = new Size(122, 170);
      this.mnuContext.Opening += new CancelEventHandler(this.mnuContext_Opening);
      this.mnuAddFilter.Name = "mnuAddFilter";
      this.mnuAddFilter.Size = new Size(121, 22);
      this.mnuAddFilter.Text = "&Add Filter...";
      this.mnuAddFilter.Click += new EventHandler(this.btnAddFilter_Click);
      this.mnuEditFilter.Name = "mnuEditFilter";
      this.mnuEditFilter.Size = new Size(121, 22);
      this.mnuEditFilter.Text = "&Edit Filter...";
      this.mnuEditFilter.Click += new EventHandler(this.btnEditFilter_Click);
      this.mnuRemoveFilter.Name = "mnuRemoveFilter";
      this.mnuRemoveFilter.Size = new Size(121, 22);
      this.mnuRemoveFilter.Text = "Delete Filter";
      this.mnuRemoveFilter.Click += new EventHandler(this.btnRemoveFilter_Click);
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new Size(118, 6);
      this.mnuInsertFilter.Name = "mnuInsertFilter";
      this.mnuInsertFilter.Size = new Size(121, 22);
      this.mnuInsertFilter.Text = "Insert Filter...";
      this.mnuInsertFilter.Click += new EventHandler(this.btnInsertFilter_Click);
      this.mnuAndOr.Name = "mnuAndOr";
      this.mnuAndOr.Size = new Size(121, 22);
      this.mnuAndOr.Text = "And/Or";
      this.mnuAndOr.Click += new EventHandler(this.btnAndOr_Click);
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new Size(118, 6);
      this.mnuParenthesis.Name = "mnuParenthesis";
      this.mnuParenthesis.Size = new Size(121, 22);
      this.mnuParenthesis.Text = "Parentheses...";
      this.mnuParenthesis.Click += new EventHandler(this.btnParenthesis_Click);
      this.mnuViewFilter.Name = "mnuViewFilter";
      this.mnuViewFilter.Size = new Size(121, 22);
      this.mnuViewFilter.Text = "View Filter...";
      this.mnuViewFilter.Click += new EventHandler(this.btnSummary_Click);
      this.btnRemoveFilter.BackColor = Color.Transparent;
      this.btnRemoveFilter.Enabled = false;
      this.btnRemoveFilter.Location = new Point(183, 3);
      this.btnRemoveFilter.Margin = new Padding(2, 3, 2, 3);
      this.btnRemoveFilter.MouseDownImage = (Image) null;
      this.btnRemoveFilter.Name = "btnRemoveFilter";
      this.btnRemoveFilter.Size = new Size(16, 16);
      this.btnRemoveFilter.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveFilter.TabIndex = 10;
      this.btnRemoveFilter.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemoveFilter, "Delete Filter");
      this.btnRemoveFilter.Click += new EventHandler(this.btnRemoveFilter_Click);
      this.btnEditFilter.BackColor = Color.Transparent;
      this.btnEditFilter.Enabled = false;
      this.btnEditFilter.Location = new Point(163, 3);
      this.btnEditFilter.Margin = new Padding(2, 3, 2, 3);
      this.btnEditFilter.MouseDownImage = (Image) null;
      this.btnEditFilter.Name = "btnEditFilter";
      this.btnEditFilter.Size = new Size(16, 16);
      this.btnEditFilter.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditFilter.TabIndex = 11;
      this.btnEditFilter.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEditFilter, "Edit Filter");
      this.btnEditFilter.Click += new EventHandler(this.btnEditFilter_Click);
      this.btnAddFilter.BackColor = Color.Transparent;
      this.btnAddFilter.Location = new Point(143, 3);
      this.btnAddFilter.Margin = new Padding(2, 3, 2, 3);
      this.btnAddFilter.MouseDownImage = (Image) null;
      this.btnAddFilter.Name = "btnAddFilter";
      this.btnAddFilter.Size = new Size(16, 16);
      this.btnAddFilter.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddFilter.TabIndex = 12;
      this.btnAddFilter.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddFilter, "Add Filter");
      this.btnAddFilter.Click += new EventHandler(this.btnAddFilter_Click);
      this.grpFilters.AutoSize = true;
      this.grpFilters.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.grpFilters.Controls.Add((Control) this.flpHeader);
      this.grpFilters.Controls.Add((Control) this.pnlFooter);
      this.grpFilters.Controls.Add((Control) this.gvFilters);
      this.grpFilters.Dock = DockStyle.Fill;
      this.grpFilters.Location = new Point(0, 0);
      this.grpFilters.Name = "grpFilters";
      this.grpFilters.Size = new Size(664, 341);
      this.grpFilters.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.grpFilters.TabIndex = 9;
      this.grpFilters.Text = "Filters";
      this.flpHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flpHeader.BackColor = Color.Transparent;
      this.flpHeader.Controls.Add((Control) this.btnSummary);
      this.flpHeader.Controls.Add((Control) this.btnParenthesis);
      this.flpHeader.Controls.Add((Control) this.btnAndOr);
      this.flpHeader.Controls.Add((Control) this.btnInsertFilter);
      this.flpHeader.Controls.Add((Control) this.verticalSeparator1);
      this.flpHeader.Controls.Add((Control) this.btnRemoveFilter);
      this.flpHeader.Controls.Add((Control) this.btnEditFilter);
      this.flpHeader.Controls.Add((Control) this.btnAddFilter);
      this.flpHeader.Controls.Add((Control) this.chkTpoIncludeChildFolder);
      this.flpHeader.FlowDirection = FlowDirection.RightToLeft;
      this.flpHeader.Location = new Point(180, 2);
      this.flpHeader.Name = "flpHeader";
      this.flpHeader.Size = new Size(483, 24);
      this.flpHeader.TabIndex = 1;
      this.btnSummary.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSummary.BackColor = SystemColors.Control;
      this.btnSummary.Enabled = false;
      this.btnSummary.Location = new Point(408, 0);
      this.btnSummary.Margin = new Padding(0);
      this.btnSummary.Name = "btnSummary";
      this.btnSummary.Padding = new Padding(2, 0, 0, 0);
      this.btnSummary.Size = new Size(75, 22);
      this.btnSummary.TabIndex = 8;
      this.btnSummary.Text = "&View Filter";
      this.btnSummary.UseVisualStyleBackColor = true;
      this.btnSummary.Click += new EventHandler(this.btnSummary_Click);
      this.btnParenthesis.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnParenthesis.BackColor = SystemColors.Control;
      this.btnParenthesis.Enabled = false;
      this.btnParenthesis.Location = new Point(325, 0);
      this.btnParenthesis.Margin = new Padding(0);
      this.btnParenthesis.Name = "btnParenthesis";
      this.btnParenthesis.Padding = new Padding(4, 0, 0, 0);
      this.btnParenthesis.Size = new Size(83, 22);
      this.btnParenthesis.TabIndex = 6;
      this.btnParenthesis.Text = "&Parentheses";
      this.btnParenthesis.UseVisualStyleBackColor = true;
      this.btnParenthesis.Click += new EventHandler(this.btnParenthesis_Click);
      this.btnAndOr.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAndOr.BackColor = SystemColors.Control;
      this.btnAndOr.Enabled = false;
      this.btnAndOr.Location = new Point(261, 0);
      this.btnAndOr.Margin = new Padding(0);
      this.btnAndOr.Name = "btnAndOr";
      this.btnAndOr.Padding = new Padding(2, 0, 0, 0);
      this.btnAndOr.Size = new Size(64, 22);
      this.btnAndOr.TabIndex = 5;
      this.btnAndOr.Text = "AND/&OR";
      this.btnAndOr.UseVisualStyleBackColor = true;
      this.btnAndOr.Click += new EventHandler(this.btnAndOr_Click);
      this.btnInsertFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnInsertFilter.BackColor = SystemColors.Control;
      this.btnInsertFilter.Enabled = false;
      this.btnInsertFilter.Location = new Point(209, 0);
      this.btnInsertFilter.Margin = new Padding(0);
      this.btnInsertFilter.Name = "btnInsertFilter";
      this.btnInsertFilter.Padding = new Padding(2, 0, 0, 0);
      this.btnInsertFilter.Size = new Size(52, 22);
      this.btnInsertFilter.TabIndex = 4;
      this.btnInsertFilter.Text = "I&nsert";
      this.btnInsertFilter.UseVisualStyleBackColor = true;
      this.btnInsertFilter.Click += new EventHandler(this.btnInsertFilter_Click);
      this.verticalSeparator1.Location = new Point(204, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 9;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.chkTpoIncludeChildFolder.BackColor = Color.Transparent;
      this.chkTpoIncludeChildFolder.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkTpoIncludeChildFolder.Location = new Point(3, 3);
      this.chkTpoIncludeChildFolder.Name = "chkTpoIncludeChildFolder";
      this.chkTpoIncludeChildFolder.RightToLeft = RightToLeft.Yes;
      this.chkTpoIncludeChildFolder.Size = new Size(135, 18);
      this.chkTpoIncludeChildFolder.TabIndex = 13;
      this.chkTpoIncludeChildFolder.Text = "Include Child Folder";
      this.chkTpoIncludeChildFolder.UseVisualStyleBackColor = false;
      this.chkTpoIncludeChildFolder.Visible = false;
      this.chkTpoIncludeChildFolder.CheckedChanged += new EventHandler(this.chkTpoIncludeChildFolder_CheckedChanged);
      this.pnlFooter.Anchor = AnchorStyles.None;
      this.pnlFooter.BackColor = Color.Transparent;
      this.pnlFooter.Location = new Point(0, 314);
      this.pnlFooter.Name = "pnlFooter";
      this.pnlFooter.Size = new Size(664, 26);
      this.pnlFooter.TabIndex = 2;
      this.pnlFooter.Visible = false;
      this.gvFilters.AllowMultiselect = false;
      this.gvFilters.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "(";
      gvColumn1.Width = 30;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Field";
      gvColumn2.Width = 131;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Operator";
      gvColumn3.Width = 94;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Value";
      gvColumn4.Width = 151;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = ")";
      gvColumn5.Width = 34;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "Joint";
      gvColumn6.Width = 67;
      this.gvFilters.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gvFilters.ContextMenuStrip = this.mnuContext;
      this.gvFilters.Dock = DockStyle.Fill;
      this.gvFilters.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvFilters.Location = new Point(1, 26);
      this.gvFilters.Name = "gvFilters";
      this.gvFilters.Size = new Size(662, 314);
      this.gvFilters.SortOption = GVSortOption.None;
      this.gvFilters.TabIndex = 0;
      this.gvFilters.SelectedIndexChanged += new EventHandler(this.gvFilters_SelectedIndexChanged);
      this.gvFilters.DoubleClick += new EventHandler(this.gvFilters_DoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpFilters);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (AdvancedSearchControl);
      this.Size = new Size(664, 341);
      this.mnuContext.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemoveFilter).EndInit();
      ((ISupportInitialize) this.btnEditFilter).EndInit();
      ((ISupportInitialize) this.btnAddFilter).EndInit();
      this.grpFilters.ResumeLayout(false);
      this.flpHeader.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

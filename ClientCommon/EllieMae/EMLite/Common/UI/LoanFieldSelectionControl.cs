// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.LoanFieldSelectionControl
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
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
  public class LoanFieldSelectionControl : UserControl
  {
    private const int SCROLL_BAR_WIDTH = 25;
    private bool allowDatabaseFieldsOnly;
    private int maxSelectionCount = 20;
    private LoanReportFieldDefs fieldDefs;
    private bool isDirty;
    private AddEditLoanFieldDialog dlgAddEditLoanField;
    private Sessions.Session session;
    private IContainer components;
    private GroupContainer groupContainer1;
    private GridView gvList;
    private StandardIconButton btnMoveDown;
    private StandardIconButton btnMoveUp;
    private StandardIconButton btnRemove;
    private StandardIconButton btnEdit;
    private StandardIconButton btnAdd;
    private ToolTip toolTip1;

    public bool IsDirty => this.isDirty;

    public bool AllowDatabaseFieldsOnly
    {
      get => this.allowDatabaseFieldsOnly;
      set => this.allowDatabaseFieldsOnly = value;
    }

    public int MaxSelectionCount
    {
      get => this.maxSelectionCount;
      set => this.maxSelectionCount = value;
    }

    public LoanFieldSelectionControl(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
    }

    public List<ColumnInfo> GetColumnInfoList()
    {
      List<ColumnInfo> columnInfoList = new List<ColumnInfo>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvList.Items)
        columnInfoList.Add(this.getColumnInfo(gvItem));
      return columnInfoList;
    }

    public void SetColumnInfoList(List<ColumnInfo> columnInfoList)
    {
      this.gvList.Items.Clear();
      foreach (ColumnInfo columnInfo in columnInfoList)
        this.gvList.Items.Add(this.setColumnInfo(columnInfo));
      this.isDirty = false;
    }

    private ColumnInfo getColumnInfo(GVItem item) => item.Tag as ColumnInfo;

    private GVItem setColumnInfo(ColumnInfo columnInfo)
    {
      return new GVItem(columnInfo.Description)
      {
        SubItems = {
          (object) columnInfo.FieldID,
          (object) Enum.GetName(typeof (ColumnSortOrder), (object) columnInfo.SortOrder),
          (object) Enum.GetName(typeof (ColumnSummaryType), (object) columnInfo.SummaryType),
          columnInfo.DecimalPlaces == 0 ? (object) string.Empty : (object) columnInfo.DecimalPlaces.ToString()
        },
        Tag = (object) columnInfo
      };
    }

    private void setColumnInfo(ColumnInfo columnInfo, GVItem gvItem)
    {
      gvItem.Text = columnInfo.Description;
      gvItem.SubItems[1].Text = columnInfo.FieldID;
      gvItem.SubItems[2].Text = Enum.GetName(typeof (ColumnSortOrder), (object) columnInfo.SortOrder);
      gvItem.SubItems[3].Text = Enum.GetName(typeof (ColumnSummaryType), (object) columnInfo.SummaryType);
      gvItem.SubItems[4].Text = columnInfo.DecimalPlaces == 0 ? string.Empty : columnInfo.DecimalPlaces.ToString();
      gvItem.Tag = (object) columnInfo;
    }

    private void setButtonState()
    {
      this.btnAdd.Enabled = this.maxSelectionCount > this.gvList.Items.Count;
      this.btnEdit.Enabled = this.gvList.SelectedItems.Count != 0;
      this.btnRemove.Enabled = this.gvList.SelectedItems.Count != 0;
      this.btnMoveUp.Enabled = this.gvList.SelectedItems.Count != 0 && 1 < this.gvList.Items.Count && this.gvList.SelectedItems[0].Index != 0;
      this.btnMoveDown.Enabled = this.gvList.SelectedItems.Count != 0 && 1 < this.gvList.Items.Count && this.gvList.Items.Count - 1 != this.gvList.SelectedItems[0].Index;
    }

    private void LoanFieldSelectionControl_Load(object sender, EventArgs e)
    {
      if (this.DesignMode)
        return;
      this.fieldDefs = !this.allowDatabaseFieldsOnly ? LoanReportFieldDefs.GetFieldDefs(this.session, false, LoanReportFieldFlags.BasicLoanDataFields) : LoanReportFieldDefs.GetFieldDefs(this.session, false, LoanReportFieldFlags.AllDatabaseFields);
      if (this.fieldDefs != null && this.fieldDefs.Count != 0)
        return;
      this.Enabled = false;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (20 == this.gvList.Items.Count)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Maximum number of fields is limitied to twenty.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        List<ColumnInfo> columnInfoList = this.GetColumnInfoList();
        if (this.dlgAddEditLoanField == null)
          this.dlgAddEditLoanField = new AddEditLoanFieldDialog(this.fieldDefs);
        this.dlgAddEditLoanField.ColumnInfoList = columnInfoList;
        this.dlgAddEditLoanField.AllowDatabaseFieldsOnly = this.allowDatabaseFieldsOnly;
        this.dlgAddEditLoanField.ClearColumnInfo();
        if (DialogResult.OK != this.dlgAddEditLoanField.ShowDialog((IWin32Window) this.ParentForm))
          return;
        GVItem gvItem = this.setColumnInfo(this.dlgAddEditLoanField.GetColumnInfo());
        this.gvList.Items.Add(gvItem);
        gvItem.Selected = true;
        this.gvList.EnsureVisible(gvItem.Index);
        this.isDirty = true;
        this.OnDataChangedEvent(EventArgs.Empty);
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gvList.SelectedItems.Count == 0)
        return;
      GVItem selectedItem = this.gvList.SelectedItems[0];
      List<ColumnInfo> columnInfoList = this.GetColumnInfoList();
      columnInfoList.Remove(this.getColumnInfo(selectedItem));
      if (this.dlgAddEditLoanField == null)
        this.dlgAddEditLoanField = new AddEditLoanFieldDialog(this.fieldDefs);
      this.dlgAddEditLoanField.ColumnInfoList = columnInfoList;
      this.dlgAddEditLoanField.AllowDatabaseFieldsOnly = this.allowDatabaseFieldsOnly;
      this.dlgAddEditLoanField.SetColumnInfo(this.getColumnInfo(selectedItem));
      if (DialogResult.OK != this.dlgAddEditLoanField.ShowDialog((IWin32Window) this.ParentForm))
        return;
      this.setColumnInfo(this.dlgAddEditLoanField.GetColumnInfo(), selectedItem);
      this.isDirty = true;
      this.OnDataChangedEvent(EventArgs.Empty);
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (this.gvList.SelectedItems.Count == 0 || DialogResult.Yes != Utils.Dialog((IWin32Window) this, "Are you sure you want to remove the selected field?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
        return;
      this.gvList.Items.Remove(this.gvList.SelectedItems[0]);
      this.setButtonState();
      this.isDirty = true;
      this.OnDataChangedEvent(EventArgs.Empty);
    }

    private void btnMoveUp_Click(object sender, EventArgs e)
    {
      if (this.gvList.SelectedItems.Count == 0 || 1 >= this.gvList.Items.Count)
        return;
      int index = this.gvList.SelectedItems[0].Index;
      if (index == 0)
        return;
      GVItem gvItem1 = (GVItem) this.gvList.Items[index - 1].Clone();
      GVItem gvItem2 = (GVItem) this.gvList.Items[index].Clone();
      this.gvList.Items[index - 1] = gvItem2;
      this.gvList.Items[index] = gvItem1;
      gvItem2.Selected = true;
      this.gvList.EnsureVisible(gvItem2.Index);
      this.isDirty = true;
      this.OnDataChangedEvent(EventArgs.Empty);
    }

    private void btnMoveDown_Click(object sender, EventArgs e)
    {
      if (this.gvList.SelectedItems.Count == 0 || 1 >= this.gvList.Items.Count)
        return;
      int index = this.gvList.SelectedItems[0].Index;
      if (this.gvList.Items.Count - 1 == index)
        return;
      GVItem gvItem1 = (GVItem) this.gvList.Items[index].Clone();
      GVItem gvItem2 = (GVItem) this.gvList.Items[index + 1].Clone();
      this.gvList.Items[index + 1] = gvItem1;
      this.gvList.Items[index] = gvItem2;
      gvItem1.Selected = true;
      this.gvList.EnsureVisible(gvItem1.Index);
      this.isDirty = true;
      this.OnDataChangedEvent(EventArgs.Empty);
    }

    private void lvwFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setButtonState();
    }

    public event LoanFieldSelectionControl.DataChangedEventHandler DataChangedEvent;

    protected virtual void OnDataChangedEvent(EventArgs e)
    {
      if (this.DataChangedEvent == null)
        return;
      this.DataChangedEvent((object) this, e);
    }

    private void gvList_SelectedIndexChanged(object sender, EventArgs e) => this.setButtonState();

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
      this.groupContainer1 = new GroupContainer();
      this.btnMoveDown = new StandardIconButton();
      this.btnMoveUp = new StandardIconButton();
      this.btnRemove = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.gvList = new GridView();
      this.toolTip1 = new ToolTip(this.components);
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.btnMoveDown).BeginInit();
      ((ISupportInitialize) this.btnMoveUp).BeginInit();
      ((ISupportInitialize) this.btnRemove).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.SuspendLayout();
      this.groupContainer1.Borders = AnchorStyles.Bottom;
      this.groupContainer1.Controls.Add((Control) this.btnMoveDown);
      this.groupContainer1.Controls.Add((Control) this.btnMoveUp);
      this.groupContainer1.Controls.Add((Control) this.btnRemove);
      this.groupContainer1.Controls.Add((Control) this.btnEdit);
      this.groupContainer1.Controls.Add((Control) this.btnAdd);
      this.groupContainer1.Controls.Add((Control) this.gvList);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(483, 146);
      this.groupContainer1.TabIndex = 8;
      this.groupContainer1.Text = "Columns";
      this.btnMoveDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMoveDown.BackColor = Color.Transparent;
      this.btnMoveDown.Location = new Point(462, 4);
      this.btnMoveDown.Name = "btnMoveDown";
      this.btnMoveDown.Size = new Size(16, 16);
      this.btnMoveDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveDown.TabIndex = 5;
      this.btnMoveDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveDown, "Move Down");
      this.btnMoveDown.Click += new EventHandler(this.btnMoveDown_Click);
      this.btnMoveUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMoveUp.BackColor = Color.Transparent;
      this.btnMoveUp.Location = new Point(440, 4);
      this.btnMoveUp.Name = "btnMoveUp";
      this.btnMoveUp.Size = new Size(16, 16);
      this.btnMoveUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveUp.TabIndex = 4;
      this.btnMoveUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveUp, "Move Up");
      this.btnMoveUp.Click += new EventHandler(this.btnMoveUp_Click);
      this.btnRemove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemove.BackColor = Color.Transparent;
      this.btnRemove.Location = new Point(418, 4);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(16, 16);
      this.btnRemove.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemove.TabIndex = 3;
      this.btnRemove.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemove, "Delete Field");
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Location = new Point(396, 4);
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 2;
      this.btnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEdit, "Edit Field");
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(374, 4);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 1;
      this.btnAdd.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAdd, "Add Field");
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.gvList.AllowMultiselect = false;
      this.gvList.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Description";
      gvColumn1.Width = 200;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Field";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Sorting";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Summary";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Decimal Places";
      gvColumn5.Width = 100;
      this.gvList.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gvList.Dock = DockStyle.Fill;
      this.gvList.Location = new Point(0, 25);
      this.gvList.Name = "gvList";
      this.gvList.Size = new Size(483, 120);
      this.gvList.TabIndex = 0;
      this.gvList.SelectedIndexChanged += new EventHandler(this.gvList_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (LoanFieldSelectionControl);
      this.Size = new Size(483, 146);
      this.Load += new EventHandler(this.LoanFieldSelectionControl_Load);
      this.groupContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.btnMoveDown).EndInit();
      ((ISupportInitialize) this.btnMoveUp).EndInit();
      ((ISupportInitialize) this.btnRemove).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.ResumeLayout(false);
    }

    public delegate void DataChangedEventHandler(object sender, EventArgs e);
  }
}

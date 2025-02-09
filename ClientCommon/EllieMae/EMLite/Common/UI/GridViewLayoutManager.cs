// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.GridViewLayoutManager
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class GridViewLayoutManager
  {
    private GridView gridView;
    private TableLayout fullLayout;
    private TableLayout defaultLayout;
    private TableLayout nonCustomizableLayout;

    public event EventHandler LayoutChanged;

    public bool IsGlobalSearch { get; set; }

    public GridViewLayoutManager(GridView gridView, TableLayout fullColumnLayout)
      : this(gridView, fullColumnLayout, (TableLayout) null)
    {
    }

    public GridViewLayoutManager(
      GridView gridView,
      TableLayout fullColumnLayout,
      bool allowCustomize)
      : this(gridView, fullColumnLayout, (TableLayout) null, (TableLayout) null, allowCustomize)
    {
    }

    public GridViewLayoutManager(
      GridView gridView,
      TableLayout fullColumnLayout,
      TableLayout defaultColumnLayout)
      : this(gridView, fullColumnLayout, defaultColumnLayout, (TableLayout) null)
    {
    }

    public GridViewLayoutManager(
      GridView gridView,
      TableLayout fullColumnLayout,
      TableLayout defaultColumnLayout,
      TableLayout nonCustomizableColumnLayout,
      bool allowCustomize = true)
    {
      this.gridView = gridView;
      this.fullLayout = fullColumnLayout;
      this.defaultLayout = defaultColumnLayout;
      this.nonCustomizableLayout = nonCustomizableColumnLayout;
      if (this.defaultLayout != null)
        this.ApplyLayout(this.defaultLayout, false);
      if (!allowCustomize)
        return;
      this.gridView.HeaderClick += new MouseEventHandler(this.gridView_ColumnClick);
    }

    public TableLayout AllColumns
    {
      get => this.fullLayout;
      set => this.fullLayout = value != null ? value : throw new ArgumentNullException();
    }

    private void gridView_ColumnClick(object source, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      this.displayConfigurationMenu(e.Location);
    }

    private void displayConfigurationMenu(Point position)
    {
      ContextMenu contextMenu = new ContextMenu();
      contextMenu.Collapse += new EventHandler(this.mnu_Collapse);
      contextMenu.MenuItems.Add("Customize Columns...", new EventHandler(this.onCustomizeColumns));
      if (this.defaultLayout != null)
        contextMenu.MenuItems.Add("Reset Columns", new EventHandler(this.onResetColumns));
      contextMenu.Show((Control) this.gridView, position);
    }

    private void mnu_Collapse(object sender, EventArgs e) => this.gridView.Refresh();

    private void onCustomizeColumns(object sender, EventArgs e) => this.CustomizeColumns();

    private void onResetColumns(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this.gridView, "Reset the columns to their default configuration?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.ResetColumns();
    }

    public TableLayout GetCurrentLayout() => GridViewLayoutManager.GetGridViewLayout(this.gridView);

    public void CustomizeColumns()
    {
      TableLayout tableLayout = GridViewLayoutManager.GetGridViewLayout(this.gridView);
      using (TableLayoutColumnSelector layoutColumnSelector = new TableLayoutColumnSelector(this.fullLayout, tableLayout, this.nonCustomizableLayout, this.IsGlobalSearch))
      {
        if (layoutColumnSelector.ShowDialog((IWin32Window) this.gridView) != DialogResult.OK)
          return;
        tableLayout = layoutColumnSelector.SelectedLayout;
      }
      this.ApplyLayout(tableLayout);
    }

    public void ResetColumns() => this.ApplyLayout(this.defaultLayout);

    public void ApplyLayout(TableLayout layout) => this.ApplyLayout(layout, true);

    public void ApplyLayout(TableLayout layout, bool raiseLayoutEvent)
    {
      this.ApplyLayout(layout, raiseLayoutEvent, false);
    }

    public void ApplyLayout(TableLayout layout, bool raiseLayoutEvent, bool setDefault)
    {
      GridViewLayoutManager.ApplyLayoutToGridView(this.gridView, layout);
      if (setDefault)
        this.defaultLayout = layout;
      if (!raiseLayoutEvent)
        return;
      this.onLayoutChanged();
    }

    private void onLayoutChanged()
    {
      if (this.LayoutChanged == null)
        return;
      this.LayoutChanged((object) this, EventArgs.Empty);
    }

    public static void ApplyLayoutToGridView(GridView gridView, TableLayout layout)
    {
      if (layout == null)
        return;
      int num = 0;
      foreach (TableLayout.Column tag in layout)
      {
        GVColumn newColumn = gridView.Columns.FindByTag((object) tag);
        if (newColumn != null)
        {
          newColumn.DisplayIndex = num;
        }
        else
        {
          newColumn = new GVColumn();
          gridView.Columns.Add(newColumn);
          newColumn.DisplayIndex = num;
        }
        newColumn.Text = tag.Title;
        newColumn.TextAlignment = ControlDraw.HorizontalAlignmentToContentAlignment(tag.Alignment);
        newColumn.Width = tag.Width;
        newColumn.Tag = (object) tag;
        ++num;
      }
      for (int nColumnIndex = gridView.Columns.Count - 1; nColumnIndex >= 0; --nColumnIndex)
      {
        GVColumn column = gridView.Columns[nColumnIndex];
        if (!(column.Tag is TableLayout.Column tag) || !layout.Contains(tag))
          gridView.Columns.Remove(column);
      }
      TableLayout.Column[] columnsByPriority = layout.GetSortColumnsByPriority();
      if (columnsByPriority.Length == 0)
        return;
      GVColumnSort[] sortOrder = new GVColumnSort[columnsByPriority.Length];
      for (int index = 0; index < columnsByPriority.Length; ++index)
      {
        GVColumn byTag = gridView.Columns.FindByTag((object) columnsByPriority[index]);
        sortOrder[index] = new GVColumnSort(byTag.Index, columnsByPriority[index].SortOrder);
      }
      gridView.Sort(sortOrder);
    }

    public static TableLayout GetGridViewLayout(GridView gridView)
    {
      List<TableLayout.Column> columnList = new List<TableLayout.Column>();
      foreach (GVColumn column1 in gridView.Columns)
      {
        TableLayout.Column column2 = (TableLayout.Column) ((TableLayout.Column) column1.Tag).Clone();
        column2.DisplayOrder = column1.DisplayIndex;
        column2.Width = column1.Width;
        column2.SortOrder = column1.SortOrder;
        column2.SortPriority = column1.SortPriority;
        columnList.Add(column2);
      }
      return new TableLayout(columnList.ToArray());
    }
  }
}

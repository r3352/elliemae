// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ListViewLayoutManager
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ListViewLayoutManager
  {
    private ListView listView;
    private TableLayout fullLayout;
    private TableLayout defaultLayout;

    public event EventHandler LayoutChanged;

    public ListViewLayoutManager(ListView listView, TableLayout fullColumnLayout)
      : this(listView, fullColumnLayout, (TableLayout) null)
    {
    }

    public ListViewLayoutManager(
      ListView listView,
      TableLayout fullColumnLayout,
      TableLayout defaultColumnLayout)
    {
      this.listView = listView;
      this.fullLayout = fullColumnLayout;
      this.defaultLayout = defaultColumnLayout;
      if (this.defaultLayout != null)
        this.ApplyLayout(this.defaultLayout, false);
      else
        this.ApplyLayout(this.fullLayout, false);
      this.listView.MouseDown += new MouseEventHandler(this.listView_MouseClick);
    }

    private void listView_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right || e.Clicks != 1)
        return;
      this.displayConfigurationMenu(e.Location);
    }

    private void displayConfigurationMenu(Point position)
    {
      ContextMenu contextMenu = new ContextMenu();
      contextMenu.MenuItems.Add("Customize Columns...", new EventHandler(this.onCustomizeColumns));
      if (this.defaultLayout != null)
        contextMenu.MenuItems.Add("Reset Columns", new EventHandler(this.onResetColumns));
      contextMenu.Show((Control) this.listView, position);
    }

    private void onCustomizeColumns(object sender, EventArgs e) => this.CustomizeColumns();

    private void onResetColumns(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this.listView, "Reset the columns to their default configuration?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.ResetColumns();
    }

    public TableLayout GetCurrentLayout() => ListViewLayoutManager.GetListViewLayout(this.listView);

    public void CustomizeColumns()
    {
      TableLayout tableLayout = ListViewLayoutManager.GetListViewLayout(this.listView);
      using (TableLayoutColumnSelector layoutColumnSelector = new TableLayoutColumnSelector(this.fullLayout, tableLayout))
      {
        if (layoutColumnSelector.ShowDialog((IWin32Window) this.listView) != DialogResult.OK)
          return;
        tableLayout = layoutColumnSelector.SelectedLayout;
      }
      this.ApplyLayout(tableLayout);
    }

    public void ResetColumns() => this.ApplyLayout(this.defaultLayout);

    public void ApplyLayout(TableLayout layout) => this.ApplyLayout(layout, true);

    public void ApplyLayout(TableLayout layout, bool raiseLayoutEvent)
    {
      ListViewLayoutManager.ApplyLayoutToListView(this.listView, layout);
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

    public static void ApplyLayoutToListView(ListView listView, TableLayout layout)
    {
      listView.Items.Clear();
      listView.Columns.Clear();
      foreach (TableLayout.Column column in layout)
        listView.Columns.Add(new ColumnHeader()
        {
          Text = column.Title,
          TextAlign = column.Alignment,
          Width = column.Width,
          Tag = (object) column
        });
    }

    public static TableLayout GetListViewLayout(ListView listView)
    {
      List<TableLayout.Column> columnList = new List<TableLayout.Column>();
      foreach (ColumnHeader column1 in listView.Columns)
      {
        TableLayout.Column column2 = (TableLayout.Column) ((TableLayout.Column) column1.Tag).Clone();
        column2.DisplayOrder = column1.DisplayIndex;
        column2.Width = column1.Width;
        columnList.Add(column2);
      }
      return new TableLayout(columnList.ToArray());
    }
  }
}

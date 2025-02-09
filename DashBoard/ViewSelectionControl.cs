// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.ViewSelectionControl
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientSession.Dashboard;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class ViewSelectionControl : UserControl
  {
    private const string dashboardCategory = "Dashboard";
    private const string maxViewsSetting = "Dashboard.MaxRecentViews";
    private int maxRecentViews = 20;
    private ViewSelectionControl.SelectionItem nullSelection = new ViewSelectionControl.SelectionItem(-1, string.Empty);
    private ViewSelectionControl.SelectionItem allSelection = new ViewSelectionControl.SelectionItem(0, "All...");
    private DashboardViewList viewList;
    private List<int> mostRecentViewIds = new List<int>();
    private List<ViewSelectionControl.SelectionItem> selectionItems = new List<ViewSelectionControl.SelectionItem>();
    private ViewSelectionControl.SelectionItem selectedItem;
    private IContainer components;
    private ComboBox cboView;

    public ViewSelectionControl()
    {
      this.InitializeComponent();
      if (this.DesignMode || !Session.IsConnected)
        return;
      this.maxRecentViews = (int) Session.ServerManager.GetServerSettings("Dashboard")[(object) "Dashboard.MaxRecentViews"];
    }

    public void RefreshSelections(DashboardViewList viewList, List<int> mostRecentViewIds)
    {
      this.viewList = viewList;
      this.mostRecentViewIds = mostRecentViewIds;
      this.buildSelectionList();
    }

    public int SetSelectedViewId(int viewId)
    {
      if (-1 == viewId)
      {
        this.cboView.SelectedItem = (object) null;
        return -1;
      }
      if (0 < viewId)
      {
        if (this.mostRecentViewIds.Contains(viewId))
          this.mostRecentViewIds.Remove(viewId);
        if (this.viewList.Contains(viewId))
        {
          this.mostRecentViewIds.Reverse();
          this.mostRecentViewIds.Add(viewId);
          this.mostRecentViewIds.Reverse();
        }
        this.buildSelectionList();
      }
      return this.selectedItem.ViewId;
    }

    private void buildSelectionList()
    {
      this.cboView.BeginUpdate();
      this.cboView.Items.Clear();
      if (this.viewList.Count == 0)
      {
        this.cboView.SelectedItem = (object) null;
        this.selectedItem = this.nullSelection;
        this.cboView.EndUpdate();
      }
      else
      {
        this.selectionItems.Clear();
        if (0 < this.mostRecentViewIds.Count)
        {
          List<int> intList = new List<int>();
          foreach (int mostRecentViewId in this.mostRecentViewIds)
          {
            if (this.viewList.Contains(mostRecentViewId))
            {
              DashboardView dashboardView = this.viewList.Find(mostRecentViewId);
              this.selectionItems.Add(new ViewSelectionControl.SelectionItem(dashboardView.ViewId, dashboardView.ViewName));
              if (this.maxRecentViews == this.selectionItems.Count)
                break;
            }
            else
              intList.Add(mostRecentViewId);
          }
          foreach (int num in intList)
            this.mostRecentViewIds.Remove(num);
        }
        if (this.maxRecentViews > this.selectionItems.Count && this.viewList.Count > this.selectionItems.Count)
        {
          foreach (DashboardView view in (Collection<DashboardView>) this.viewList)
          {
            if (!this.mostRecentViewIds.Contains(view.ViewId))
            {
              this.mostRecentViewIds.Add(view.ViewId);
              this.selectionItems.Add(new ViewSelectionControl.SelectionItem(view.ViewId, view.ViewName));
              if (this.maxRecentViews == this.selectionItems.Count)
                break;
            }
          }
        }
        this.selectedItem = this.selectionItems[0];
        if (1 < this.selectionItems.Count)
          this.selectionItems.Sort((IComparer<ViewSelectionControl.SelectionItem>) new ViewSelectionControl.SelectionItemComparer());
        if (this.maxRecentViews == this.selectionItems.Count && this.viewList.Count > this.selectionItems.Count)
          this.selectionItems.Add(this.allSelection);
        this.cboView.Items.AddRange((object[]) this.selectionItems.ToArray());
        this.cboView.SelectedItem = (object) this.selectedItem;
        this.cboView.EndUpdate();
      }
    }

    private void cboView_SelectionChangeCommitted(object sender, EventArgs e)
    {
      int viewId = ((ViewSelectionControl.SelectionItem) this.cboView.SelectedItem).ViewId;
      if (viewId == 0)
      {
        using (ViewSelectionDialog viewSelectionDialog = new ViewSelectionDialog(this.viewList))
        {
          if (DialogResult.OK != viewSelectionDialog.ShowDialog())
          {
            this.cboView.SelectedItem = (object) this.selectedItem;
            return;
          }
          viewId = viewSelectionDialog.SelectedViewId;
        }
      }
      this.OnSelectionChanged(new SelectionChangedEventArgs(this.SetSelectedViewId(viewId)));
    }

    public event ViewSelectionControl.SelectionChangedEventHandler SelectionChangedEvent;

    protected virtual void OnSelectionChanged(SelectionChangedEventArgs e)
    {
      if (this.SelectionChangedEvent == null)
        return;
      this.SelectionChangedEvent((object) this, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cboView = new ComboBox();
      this.SuspendLayout();
      this.cboView.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboView.FormattingEnabled = true;
      this.cboView.Location = new Point(0, 0);
      this.cboView.Name = "cboView";
      this.cboView.Size = new Size(160, 21);
      this.cboView.TabIndex = 2;
      this.cboView.SelectionChangeCommitted += new EventHandler(this.cboView_SelectionChangeCommitted);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.cboView);
      this.Name = nameof (ViewSelectionControl);
      this.Size = new Size(160, 21);
      this.ResumeLayout(false);
    }

    public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);

    public class SelectionItem
    {
      private int viewId;
      private string viewName = string.Empty;

      public int ViewId => this.viewId;

      public string ViewName => this.viewName;

      public SelectionItem(int viewId, string viewName)
      {
        this.viewId = viewId;
        this.viewName = viewName;
      }

      public override string ToString() => this.viewName;
    }

    public class SelectionItemComparer : IComparer<ViewSelectionControl.SelectionItem>
    {
      public int Compare(ViewSelectionControl.SelectionItem x, ViewSelectionControl.SelectionItem y)
      {
        return string.Compare(x.ViewName, y.ViewName, false);
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: TreeViewSearchProvider.TreeViewSearchManager
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace TreeViewSearchProvider
{
  public class TreeViewSearchManager
  {
    private Searcher _searcher;
    private NodeFormatter _formatter;

    public Publisher SearchEvents => this._searcher.Events;

    public SearchParameters SearchParams => this._searcher.SearchParams;

    public NodeFormatSettings NodeFormat => this._formatter.NodeFormatSettings;

    public TreeViewSearchManager()
    {
      this._searcher = new Searcher();
      this._formatter = new NodeFormatter();
      this.SearchEvents.SearchEngineReset += new Publisher.SearchEngineHandler(this.SearchEvents_SearchEngineReset);
    }

    public void AddTrees(List<TreeView> treeViews, bool doClear = false)
    {
      if (doClear)
        this.ClearTrees();
      this._searcher.TreeViewCollection.AddRange((IEnumerable<TreeView>) treeViews);
    }

    public void ClearTrees()
    {
      this._searcher.TreeViewCollection.Clear();
      this.ResetSearch();
    }

    public void Search(string searchText)
    {
      this._searcher.SearchParams.Criteria = searchText;
      this._searcher.Search();
    }

    public void ApplyFormat(TreeNode node)
    {
      if (node == null)
        return;
      this._formatter.Format(node);
    }

    public void ApplyFormat(List<TreeNode> nodes)
    {
      if (nodes == null || nodes.Count == 0)
        return;
      this._formatter.Format(nodes);
    }

    public void ResetSearch() => this._searcher.Reset();

    public void ResetFormat() => this._formatter.Reset();

    public void RestoreDefaultSettings()
    {
      this._searcher.SearchParams.RestoreDefaultSettings();
      this._formatter.NodeFormatSettings = NodeFormatSettings.DefaultSettings;
      this.ResetSearch();
    }

    private void SearchEvents_SearchEngineReset(string message) => this.ResetFormat();
  }
}

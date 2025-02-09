// Decompiled with JetBrains decompiler
// Type: TreeViewSearchProvider.Searcher
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace TreeViewSearchProvider
{
  internal class Searcher
  {
    private const string SEARCH_PARAM_STATE_KEY = "SSPSK";
    private bool matchFound;
    private bool matchFound2;
    private int currentTreeIndex;
    private int currentParentNodeIndex;
    private int matchedNodeIndex;
    private int nodeIterator;
    private StateStore<string, SearchParameters> _searchParametersState = new StateStore<string, SearchParameters>("SSPSK", new SearchParameters());
    private List<TreeView> tvCollection = new List<TreeView>();
    private List<TreeNode> matchedNodes = new List<TreeNode>();

    private bool isSingleSearchOccurance
    {
      get => this.SearchParams.Occurances == SearchParameters.Occurance.NextOne;
    }

    private bool canBreak => this.matchFound && this.isSingleSearchOccurance;

    internal bool IsSearchComplete => this.currentTreeIndex >= this.tvCollection.Count;

    internal List<TreeView> TreeViewCollection => this.tvCollection;

    internal SearchParameters SearchParams { get; } = new SearchParameters();

    internal Publisher Events { get; } = new Publisher();

    internal void Search()
    {
      if (!this.validation())
        return;
      this.searchTrees();
      this.isMatchFound();
      this.isSearchComplete();
    }

    internal void Reset()
    {
      this.matchFound = false;
      this.matchFound2 = false;
      this.currentTreeIndex = 0;
      this.currentParentNodeIndex = 0;
      this.matchedNodeIndex = 0;
      this.nodeIterator = 0;
      this.matchedNodes.Clear();
      this.Events.OnSearchEngineReset("Search engine reset.");
    }

    private bool validation()
    {
      if (this.SearchParams.IsStateChanged(this._searchParametersState["SSPSK"]))
      {
        this.Reset();
        this.SearchParams.UpdateState(this._searchParametersState["SSPSK"]);
      }
      bool flag;
      if (this.isSearchComplete())
        flag = false;
      else if (string.IsNullOrEmpty(this.SearchParams.Criteria))
      {
        flag = false;
        this.Events.OnValidationError("Specify search criteria.");
      }
      else if (this.tvCollection.Count == 0)
      {
        flag = false;
        this.Events.OnValidationError("Add TreeViews to search collection.");
      }
      else
      {
        flag = hasChildren();
        if (!flag)
          this.Events.OnValidationError("Tree nodes not added in TreeViews.");
      }
      return flag;

      bool hasChildren()
      {
        for (int index = 0; index < this.tvCollection.Count; ++index)
        {
          if (this.tvCollection[index].Nodes.Count > 0)
            return true;
        }
        return false;
      }
    }

    private bool isMatchFound()
    {
      if (!this.matchFound && !this.matchFound2)
        return false;
      this.Events.OnMatchFound(this.matchedNodes, this.getMessage(1, this.matchedNodes.Count));
      return true;
    }

    private bool isSearchComplete()
    {
      if (!this.IsSearchComplete)
        return false;
      this.Events.OnEndOfSearch(this.matchedNodes, this.getMessage(2, this.matchedNodes.Count));
      return true;
    }

    private string getMessage(int msgType, int nodeCount)
    {
      string message = (string) null;
      switch (msgType)
      {
        case 1:
          message = nodeCount != 1 ? string.Format("{0} matches found.", (object) nodeCount) : string.Format("{0} match found.", (object) nodeCount);
          break;
        case 2:
          message = nodeCount != 1 ? string.Format("{0} Search complete.", (object) string.Format("{0} matches found.", (object) this.matchedNodes.Count)) : string.Format("{0} Search complete.", (object) string.Format("{0} match found.", (object) this.matchedNodes.Count));
          break;
      }
      return message;
    }

    private void searchTrees()
    {
      int currentTreeIndex;
      for (currentTreeIndex = this.currentTreeIndex; currentTreeIndex < this.tvCollection.Count; ++currentTreeIndex)
      {
        if (currentTreeIndex > this.currentTreeIndex)
        {
          this.matchedNodeIndex = 0;
          this.currentParentNodeIndex = 0;
        }
        this.searchTree(this.tvCollection[currentTreeIndex]);
        if (this.canBreak)
          break;
      }
      this.currentTreeIndex = currentTreeIndex;
    }

    private void searchTree(TreeView tv)
    {
      int currentParentNodeIndex;
      for (currentParentNodeIndex = this.currentParentNodeIndex; currentParentNodeIndex < tv.Nodes.Count; ++currentParentNodeIndex)
      {
        this.nodeIterator = 0;
        if (currentParentNodeIndex > this.currentParentNodeIndex)
          this.matchedNodeIndex = 0;
        this.searchNodes(tv.Nodes[currentParentNodeIndex]);
        if (this.canBreak)
          break;
      }
      this.currentParentNodeIndex = currentParentNodeIndex;
    }

    private void searchNodes(TreeNode parent)
    {
      ++this.nodeIterator;
      this.isMatch(parent);
      if (this.matchFound)
      {
        this.matchedNodeIndex = this.nodeIterator;
        this.matchedNodes.Add(parent);
        if (this.isSingleSearchOccurance)
          return;
      }
      foreach (TreeNode node in parent.Nodes)
      {
        this.searchNodes(node);
        if (this.canBreak)
          break;
      }
    }

    private void isMatch(TreeNode tn)
    {
      if (this.nodeIterator <= this.matchedNodeIndex)
      {
        this.matchFound = false;
      }
      else
      {
        this.matchFound = this.textComparer(tn.Text);
        if (this.isSingleSearchOccurance || !this.matchFound)
          return;
        this.matchFound2 = true;
      }
    }

    private bool textComparer(string text)
    {
      bool flag = false;
      if (this.SearchParams.TextOptions == SearchParameters.TextOption.None)
        flag = text.IndexOf(this.SearchParams.Criteria, StringComparison.CurrentCultureIgnoreCase) >= 0;
      else if (this.SearchParams.TextOptions == (SearchParameters.TextOption.MatchWord | SearchParameters.TextOption.MatchCase))
        flag = Regex.Match(text, "\\b" + this.SearchParams.Criteria + "\\b").Success;
      else if (this.SearchParams.TextOptions == SearchParameters.TextOption.MatchWord)
        flag = Regex.Match(text, "\\b" + this.SearchParams.Criteria + "\\b", RegexOptions.IgnoreCase).Success;
      else if (this.SearchParams.TextOptions == SearchParameters.TextOption.MatchCase)
        flag = text.IndexOf(this.SearchParams.Criteria, StringComparison.CurrentCulture) >= 0;
      return flag;
    }
  }
}

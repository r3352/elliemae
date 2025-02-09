// Decompiled with JetBrains decompiler
// Type: TreeViewSearchProvider.Publisher
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace TreeViewSearchProvider
{
  public class Publisher
  {
    public event Publisher.SearchEngineHandler SearchEngineReset;

    public event Publisher.SearchHandler MatchFound;

    public event Publisher.SearchHandler EndOfSearch;

    public event Publisher.ValidationErrorHandler ValidationError;

    public event Publisher.FormatHandler ApplyFormat;

    public event Publisher.NavigationHandler Next;

    internal virtual void OnSearchEngineReset(string message)
    {
      Publisher.SearchEngineHandler searchEngineReset = this.SearchEngineReset;
      if (searchEngineReset == null)
        return;
      searchEngineReset(message);
    }

    internal virtual void OnMatchFound(List<TreeNode> matchedNodes, string message)
    {
      Publisher.SearchHandler matchFound = this.MatchFound;
      if (matchFound == null)
        return;
      matchFound(matchedNodes, message);
    }

    internal virtual void OnEndOfSearch(List<TreeNode> matchedNodes, string message)
    {
      Publisher.SearchHandler endOfSearch = this.EndOfSearch;
      if (endOfSearch == null)
        return;
      endOfSearch(matchedNodes, message);
    }

    internal virtual void OnValidationError(string message)
    {
      Publisher.ValidationErrorHandler validationError = this.ValidationError;
      if (validationError == null)
        return;
      validationError(message);
    }

    internal virtual void OnApplyFormat()
    {
      Publisher.FormatHandler applyFormat = this.ApplyFormat;
      if (applyFormat == null)
        return;
      applyFormat();
    }

    public virtual void OnNext(TreeNode node)
    {
      Publisher.NavigationHandler next = this.Next;
      if (next == null)
        return;
      next(node);
    }

    public delegate void SearchEngineHandler(string message);

    public delegate void SearchHandler(List<TreeNode> matchedNodes, string message);

    public delegate void ValidationErrorHandler(string message);

    public delegate void FormatHandler();

    public delegate void NavigationHandler(TreeNode node);
  }
}

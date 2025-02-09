// Decompiled with JetBrains decompiler
// Type: TreeViewSearchProvider.SearchParameters
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.ComponentModel;

#nullable disable
namespace TreeViewSearchProvider
{
  public class SearchParameters
  {
    internal string Criteria;
    public SearchParameters.TextOption TextOptions;
    public SearchParameters.Occurance Occurances;

    internal void RestoreDefaultSettings()
    {
      this.Criteria = (string) null;
      this.TextOptions = SearchParameters.TextOption.None;
      this.Occurances = SearchParameters.Occurance.NextOne;
    }

    public bool IsStateChanged(SearchParameters state)
    {
      bool flag;
      if (this.TextOptions != state.TextOptions)
        flag = false;
      else if (this.Occurances != state.Occurances)
      {
        flag = false;
      }
      else
      {
        StringComparison comparisonType = this.TextOptions.HasFlag((Enum) SearchParameters.TextOption.MatchCase) ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;
        flag = string.Equals(this.Criteria ?? "", state.Criteria ?? "", comparisonType);
      }
      return !flag;
    }

    public void UpdateState(SearchParameters state)
    {
      state.Criteria = this.Criteria;
      state.TextOptions = this.TextOptions;
      state.Occurances = this.Occurances;
    }

    [Flags]
    public enum TextOption
    {
      [Description("None")] None = 0,
      [Description("Match Word")] MatchWord = 1,
      [Description("Match Case")] MatchCase = 2,
    }

    public enum Occurance
    {
      [Description("Find Next")] NextOne = 1,
      [Description("Find All")] All = 2,
    }
  }
}

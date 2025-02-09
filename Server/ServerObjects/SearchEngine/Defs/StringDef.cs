// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.SearchEngine.Defs.StringDef
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Enums;
using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Exceptions;
using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Utils;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.SearchEngine.Defs
{
  public class StringDef(string column, string tableAlias, FilterDataType dataType) : FilterDef(column, tableAlias, dataType)
  {
    public override bool ValidateFilter(string fragmentName, string fragmentValue)
    {
      if (fragmentValue.Length > 1)
        return true;
      throw new SearchFilterParseException(string.Format("'{0}' filter value must be minimum 2 characters.", (object) fragmentName));
    }

    public override SearchFilter GetFilter(
      string fragmentName,
      string filterQuery,
      ref int charIndex)
    {
      char ch = filterQuery[charIndex];
      string stringFragmentValue = SearchFilterUtil.GetStringFragmentValue(fragmentName, filterQuery, ref charIndex);
      if (ch != ':')
      {
        if (ch == '=')
          return new SearchFilter(fragmentName, FilterOperator.Equals, stringFragmentValue, (FilterDef) this);
        throw new SearchFilterInvalidOperatorException(string.Format("'{0}' filter does not support specified operator.", (object) fragmentName));
      }
      this.ValidateFilter(fragmentName, stringFragmentValue);
      return new SearchFilter(fragmentName, FilterOperator.Like, stringFragmentValue, (FilterDef) this);
    }
  }
}

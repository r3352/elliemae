// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.SearchEngine.Defs.DateTimeDef
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Enums;
using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Exceptions;
using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Utils;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.SearchEngine.Defs
{
  public class DateTimeDef(string column, string tableAlias, FilterDataType dataType) : FilterDef(column, tableAlias, dataType)
  {
    public override SearchFilter GetFilter(
      string fragmentName,
      string filterQuery,
      ref int charIndex)
    {
      char ch = filterQuery[charIndex];
      string stringFragmentValue = SearchFilterUtil.GetStringFragmentValue(fragmentName, filterQuery, ref charIndex);
      this.ValidateFilter(fragmentName, stringFragmentValue);
      if (ch == '=')
        return new SearchFilter(fragmentName, FilterOperator.Equals, stringFragmentValue, (FilterDef) this);
      throw new SearchFilterInvalidOperatorException(string.Format("'{0}' filter does not support specified operator.", (object) fragmentName));
    }

    public override bool ValidateFilter(string fragmentName, string fragmentValue)
    {
      if (DateTime.TryParse(fragmentValue, out DateTime _))
        return true;
      throw new SearchFilterInvalidValueException(string.Format("'{0}' filter allows only DateTime values in UTC format.", (object) fragmentName));
    }
  }
}

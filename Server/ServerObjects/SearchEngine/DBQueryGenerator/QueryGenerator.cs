// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.SearchEngine.DBQueryGenerator.QueryGenerator
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Defs;
using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Enums;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.SearchEngine.DBQueryGenerator
{
  public class QueryGenerator
  {
    public static FilterQuery GetFilterQuery(SearchFilter filter)
    {
      switch (filter.FilterDef.DataType)
      {
        case FilterDataType.String:
          return (FilterQuery) new StringQuery(filter);
        case FilterDataType.Number:
          return (FilterQuery) new NumberQuery(filter);
        default:
          return (FilterQuery) null;
      }
    }
  }
}

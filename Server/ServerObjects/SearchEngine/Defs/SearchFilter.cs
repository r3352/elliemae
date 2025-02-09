// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.SearchEngine.Defs.SearchFilter
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Enums;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.SearchEngine.Defs
{
  public class SearchFilter
  {
    public SearchFilter()
    {
    }

    public SearchFilter(string name, FilterOperator @operator, string value, FilterDef filterDef)
    {
      this.Name = name;
      this.Operator = @operator;
      this.Value = value;
      this.FilterDef = filterDef;
    }

    public string Name { get; set; }

    public FilterOperator Operator { get; set; }

    public string Value { get; set; }

    public FilterDef FilterDef { get; set; }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.SearchEngine.DBQueryGenerator.FilterQuery
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Defs;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.SearchEngine.DBQueryGenerator
{
  public abstract class FilterQuery
  {
    public SearchFilter Filter { get; set; }

    public abstract string GetSql();

    public string GetColumnWithTableAlias()
    {
      string columnWithTableAlias = this.Filter.FilterDef.Column;
      if (string.IsNullOrEmpty(this.Filter.FilterDef.TableAlias))
        columnWithTableAlias = this.Filter.FilterDef.TableAlias + "." + columnWithTableAlias;
      return columnWithTableAlias;
    }

    public string GetDBValue()
    {
      string dataMapping = this.Filter.Value;
      if (this.Filter.FilterDef.DataMappings != null && this.Filter.FilterDef.DataMappings.ContainsKey(this.Filter.Name))
        dataMapping = this.Filter.FilterDef.DataMappings[dataMapping];
      return dataMapping;
    }
  }
}

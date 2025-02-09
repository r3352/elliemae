// Decompiled with JetBrains decompiler
// Type: Elli.SQE.DD.LoanQueryableFieldSources
// Assembly: Elli.SQE.DD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD1B11DD-560E-4083-B59A-D629AF47C790
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.DD.dll

using EllieMae.EMLite.ClientServer.Query;
using System.Collections.Concurrent;

#nullable disable
namespace Elli.SQE.DD
{
  public sealed class LoanQueryableFieldSources
  {
    private readonly ConcurrentDictionary<string, QueryableFieldSource> _cacheSources = new ConcurrentDictionary<string, QueryableFieldSource>();
    private readonly ConcurrentDictionary<string, string> _cacheSourcePointer = new ConcurrentDictionary<string, string>();
    private readonly IQueryableFieldSourceBuilder _builder;

    public LoanQueryableFieldSources(IQueryableFieldSourceBuilder builder)
    {
      this._builder = builder;
    }

    public QueryableFieldSource Get(string name)
    {
      QueryableFieldSource queryableFieldSource;
      return !this._cacheSources.TryGetValue(name, out queryableFieldSource) ? (QueryableFieldSource) null : queryableFieldSource;
    }

    public QueryableFieldSource GetByFieldName(string fieldName)
    {
      if (!this._cacheSourcePointer.TryGetValue(fieldName, out string _))
        return (QueryableFieldSource) null;
      QueryableFieldSource queryableFieldSource;
      return !this._cacheSources.TryGetValue(fieldName, out queryableFieldSource) ? (QueryableFieldSource) null : queryableFieldSource;
    }

    public QueryableFieldSource Generate(IQueryTerm field)
    {
      QueryableFieldSource byFieldName = this.GetByFieldName(field.FieldName);
      if (byFieldName != null)
        return byFieldName;
      QueryableField field1 = LoanQueryableFields.Instance[field.FieldName];
      if (field1 == null)
        return (QueryableFieldSource) null;
      QueryableFieldSource queryableFieldSource = this._builder.Build(field1);
      this._cacheSources.TryAdd(queryableFieldSource.Name, queryableFieldSource);
      this._cacheSourcePointer.TryAdd(field.FieldName, queryableFieldSource.Name);
      return queryableFieldSource;
    }

    public void GenerateMissing()
    {
      foreach (QueryableField queryableField in LoanQueryableFields.Instance.QueryableFields)
        this.Generate((IQueryTerm) new DataField(queryableField.ID));
    }
  }
}

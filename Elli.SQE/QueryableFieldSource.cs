// Decompiled with JetBrains decompiler
// Type: Elli.SQE.QueryableFieldSource
// Assembly: Elli.SQE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 49809150-F4C8-4F09-B50E-E491713B0B30
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.dll

using EllieMae.EMLite.ClientServer.Query;
using System;

#nullable disable
namespace Elli.SQE
{
  public class QueryableFieldSource : IFieldSource, IEquatable<object>
  {
    private string name;
    private string joinClause;
    private string[] dependencies;
    private QueryableFieldCriterionDef def;

    public QueryableFieldSource(string name, string joinClause, QueryableFieldCriterionDef def)
      : this(name, joinClause, new string[0], def)
    {
    }

    public QueryableFieldSource(
      string name,
      string joinClause,
      string[] dependencies,
      QueryableFieldCriterionDef def)
    {
      this.name = name;
      this.joinClause = joinClause;
      this.dependencies = dependencies;
      this.def = def;
    }

    public string Name => this.name;

    public string JoinClause => this.joinClause;

    public string[] Dependencies => this.dependencies;

    public QueryableFieldCriterionDef Definition => this.def;

    public override bool Equals(object obj)
    {
      return obj is QueryableFieldSource queryableFieldSource && string.Compare(queryableFieldSource.Name, this.Name, true) == 0;
    }

    public override int GetHashCode() => this.Name.ToLower().GetHashCode();
  }
}

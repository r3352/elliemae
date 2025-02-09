// Decompiled with JetBrains decompiler
// Type: Elli.SQE.QueryableFieldCriterionDef
// Assembly: Elli.SQE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 49809150-F4C8-4F09-B50E-E491713B0B30
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.dll

using EllieMae.EMLite.ClientServer.Query;
using System;

#nullable disable
namespace Elli.SQE
{
  public class QueryableFieldCriterionDef : IDef
  {
    public QueryableFieldCriterionDef(
      QueryableField field,
      ICanonicalFragmentFormatter canonicalFragmentFormatter = null)
    {
      this.OfField = field;
      this.CanonicalPath = new CanonicalPath(canonicalFragmentFormatter);
    }

    private QueryableField OfField { get; set; }

    public CanonicalPath CanonicalPath { get; private set; }

    public DataField DataField { get; set; }

    public QueryCriterion Qualifier { get; set; }

    public bool IsCollection { get; set; }

    public string FieldId => this.OfField.ID;

    public int? Index => this.OfField.Index;

    public bool IsRepeatable => this.OfField.IsRepeatable;

    public Type FieldType => this.OfField.Type;
  }
}

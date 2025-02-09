// Decompiled with JetBrains decompiler
// Type: Elli.SQE.QueryableFieldCriterionDefConverter
// Assembly: Elli.SQE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 49809150-F4C8-4F09-B50E-E491713B0B30
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.dll

using Elli.Common.ModelFields;
using EllieMae.EMLite.ClientServer.Query;
using System.Collections.Generic;

#nullable disable
namespace Elli.SQE
{
  public class QueryableFieldCriterionDefConverter : IDefConverter<QueryableField>
  {
    private readonly ICanonicalFragmentFormatter _canonicalFragmentFormatter;
    private QueryableFieldCriterionDef _def;

    public QueryableFieldCriterionDefConverter(
      ICanonicalFragmentFormatter canonicalFragmentFormatter = null)
    {
      this._canonicalFragmentFormatter = canonicalFragmentFormatter;
    }

    public virtual IDef Convert(QueryableField obj)
    {
      this._def = new QueryableFieldCriterionDef(obj, this._canonicalFragmentFormatter);
      ModelFieldPath withFullPath = ModelFieldPath.CreateWithFullPath(obj.Field.ModelPath);
      ModelFieldPathExpression expression;
      while (true)
      {
        expression = ModelFieldPathExpression.Parse(withFullPath.CurrentPathExpression);
        if (!withFullPath.IsField)
        {
          if (expression.IsCollection)
          {
            this._def.IsCollection = expression.IsCollection;
            this.ConvertCollection(expression, obj);
          }
          else
            this.ConvertEntity(expression, obj);
          withFullPath.MoveToNextPath();
        }
        else
          break;
      }
      this.ConvertField(expression, obj);
      return (IDef) this._def;
    }

    protected virtual void ConvertCollection(
      ModelFieldPathExpression expression,
      QueryableField obj)
    {
      this._def.CanonicalPath.Combine((CanonicalPath.Fragment) new CanonicalPath.CollectionFragment(expression.CollectionName));
      bool flag = this.ShouldAddIndexFragment(expression, obj);
      if (flag)
        this._def.CanonicalPath.Combine((CanonicalPath.Fragment) new CanonicalPath.IndexFragment(expression.Index));
      if (!expression.HasQualifier)
        return;
      List<QueryCriterion> queryCriterionList = new List<QueryCriterion>();
      foreach (ModelFieldPathQualifier qualifier in (IEnumerable<ModelFieldPathQualifier>) expression.Qualifiers)
      {
        this._def.CanonicalPath.Combine((CanonicalPath.Fragment) new CanonicalPath.QualifierFragment(qualifier.Name, qualifier.Value));
        if (qualifier.IsValueQuoted)
          queryCriterionList.Add((QueryCriterion) new StringValueCriterion(qualifier.Name, qualifier.Value, StringMatchType.Exact));
        else
          queryCriterionList.Add((QueryCriterion) new OrdinalValueCriterion(qualifier.Name, (object) qualifier.Value, OrdinalMatchType.Equals));
      }
      if (expression.IsIndexed && !flag)
      {
        this._def.CanonicalPath.Combine((CanonicalPath.Fragment) new CanonicalPath.QualifierFragment("ModelPathIndex", expression.Index.ToString()));
        queryCriterionList.Add((QueryCriterion) new OrdinalValueCriterion("ModelPathIndex", (object) expression.Index, OrdinalMatchType.Equals));
      }
      this._def.Qualifier = (QueryCriterion) new BinaryOperation(BinaryOperator.And, queryCriterionList.ToArray());
    }

    protected virtual void ConvertEntity(ModelFieldPathExpression expression, QueryableField obj)
    {
      this._def.CanonicalPath.Combine((CanonicalPath.Fragment) new CanonicalPath.EntityFragment(expression.PropertyName));
    }

    protected virtual void ConvertField(ModelFieldPathExpression expression, QueryableField obj)
    {
      this._def.CanonicalPath.Combine((CanonicalPath.Fragment) new CanonicalPath.PropertyFragment(expression.PropertyName));
      this._def.DataField = new DataField(expression.PropertyName);
    }

    protected virtual bool ShouldAddIndexFragment(
      ModelFieldPathExpression expression,
      QueryableField obj)
    {
      return true;
    }
  }
}

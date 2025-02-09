// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.FilterEvaluator
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  public class FilterEvaluator : IFilterEvaluator
  {
    public static readonly FilterEvaluator Empty = new FilterEvaluator();
    private IFilterEvaluator lhs;
    private IFilterEvaluator rhs;
    private JointTokens joinMethod;

    private FilterEvaluator()
    {
    }

    public FilterEvaluator(IFilterEvaluator lhs) => this.lhs = lhs;

    public FilterEvaluator(IFilterEvaluator lhs, IFilterEvaluator rhs, JointTokens joinMethod)
    {
      this.lhs = lhs;
      this.rhs = rhs;
      this.joinMethod = joinMethod;
    }

    public FilterEvaluator(FilterEvaluator source)
    {
      this.lhs = source.lhs;
      this.rhs = source.rhs;
      this.joinMethod = source.joinMethod;
    }

    public FilterEvaluator Join(IFilterEvaluator ex, JointTokens joinMethod)
    {
      return this.lhs == null ? new FilterEvaluator(ex) : new FilterEvaluator((IFilterEvaluator) this, ex, joinMethod);
    }

    public QueryCriterion ToQueryCriterion() => this.ToQueryCriterion(FilterEvaluationOption.None);

    public QueryCriterion ToQueryCriterion(FilterEvaluationOption options)
    {
      if (this.lhs == null)
        return (QueryCriterion) null;
      QueryCriterion queryCriterion = this.lhs.ToQueryCriterion(options);
      if (this.rhs == null)
        return queryCriterion;
      if (queryCriterion == null)
        return this.rhs.ToQueryCriterion(options);
      if (this.joinMethod == JointTokens.And)
        return queryCriterion.And(this.rhs.ToQueryCriterion(options));
      if (this.joinMethod == JointTokens.Or)
        return queryCriterion.Or(this.rhs.ToQueryCriterion(options));
      throw new Exception("Invalid boolean join expression in filter");
    }

    bool IFilterEvaluator.Evaluate(object o, FilterEvaluationOption options)
    {
      return this.evaluateObject(o, options);
    }

    bool IFilterEvaluator.Evaluate(object o) => this.evaluateObject(o, FilterEvaluationOption.None);

    public bool Evaluate(PipelineInfo pinfo, FilterEvaluationOption options)
    {
      return this.evaluateObject((object) pinfo, options);
    }

    public bool Evaluate(LoanData loan, FilterEvaluationOption options)
    {
      return this.evaluateObject((object) loan, options);
    }

    private bool evaluateObject(object o, FilterEvaluationOption options)
    {
      if (this.lhs == null)
        return true;
      bool flag = this.evaluateInternal(this.lhs, o, options);
      if (this.rhs == null)
        return flag;
      if (flag && this.joinMethod == JointTokens.Or)
        return true;
      return (flag || this.joinMethod != JointTokens.And) && this.evaluateInternal(this.rhs, o, options);
    }

    private bool evaluateInternal(IFilterEvaluator ex, object o, FilterEvaluationOption options)
    {
      if (!(ex is FieldFilter))
        return ex.Evaluate(o, options);
      FieldFilter fieldFilter = (FieldFilter) ex;
      object obj = (object) null;
      switch (o)
      {
        case PipelineInfo _:
          PipelineInfo pipelineInfo = (PipelineInfo) o;
          obj = !pipelineInfo.Info.ContainsKey((object) fieldFilter.CriterionName) ? pipelineInfo.Info[(object) fieldFilter.FieldID] : pipelineInfo.Info[(object) fieldFilter.CriterionName];
          break;
        case LoanData _:
          LoanData loanData = (LoanData) o;
          obj = Utils.ConvertToNativeValue(loanData.GetSimpleField(fieldFilter.FieldID), loanData.GetFormat(fieldFilter.FieldID), (object) null);
          break;
      }
      return fieldFilter.Evaluate(obj, options);
    }
  }
}

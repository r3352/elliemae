// Decompiled with JetBrains decompiler
// Type: Elli.SQE.QueryDsl.QueryCriterionVisitable
// Assembly: Elli.SQE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 49809150-F4C8-4F09-B50E-E491713B0B30
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.dll

using EllieMae.EMLite.ClientServer.Query;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Elli.SQE.QueryDsl
{
  public sealed class QueryCriterionVisitable : IVisitable<IQueryCriterionVisitor>
  {
    private readonly QueryCriterion _filter;

    public QueryCriterionVisitable(QueryCriterion filter) => this._filter = filter;

    public T Accept<T>(IQueryCriterionVisitor visitor) where T : class
    {
      // ISSUE: reference to a compiler-generated field
      if (QueryCriterionVisitable.\u003C\u003Eo__2<T>.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        QueryCriterionVisitable.\u003C\u003Eo__2<T>.\u003C\u003Ep__0 = CallSite<Func<CallSite, IQueryCriterionVisitor, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Visit", (IEnumerable<Type>) null, typeof (QueryCriterionVisitable), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = QueryCriterionVisitable.\u003C\u003Eo__2<T>.\u003C\u003Ep__0.Target((CallSite) QueryCriterionVisitable.\u003C\u003Eo__2<T>.\u003C\u003Ep__0, visitor, (object) this._filter);
      return !obj.IsNull<object>() ? obj.CastTo<T>() : default (T);
    }

    public object Accept(IQueryCriterionVisitor visitor)
    {
      // ISSUE: reference to a compiler-generated field
      if (QueryCriterionVisitable.\u003C\u003Eo__3.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        QueryCriterionVisitable.\u003C\u003Eo__3.\u003C\u003Ep__0 = CallSite<Func<CallSite, IQueryCriterionVisitor, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Visit", (IEnumerable<Type>) null, typeof (QueryCriterionVisitable), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return QueryCriterionVisitable.\u003C\u003Eo__3.\u003C\u003Ep__0.Target((CallSite) QueryCriterionVisitable.\u003C\u003Eo__3.\u003C\u003Ep__0, visitor, (object) this._filter);
    }
  }
}

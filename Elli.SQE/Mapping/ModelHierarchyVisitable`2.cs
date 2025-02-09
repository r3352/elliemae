// Decompiled with JetBrains decompiler
// Type: Elli.SQE.Mapping.ModelHierarchyVisitable`2
// Assembly: Elli.SQE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 49809150-F4C8-4F09-B50E-E491713B0B30
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Elli.SQE.Mapping
{
  public sealed class ModelHierarchyVisitable<THierachy, TVisitor>
    where THierachy : ModelHierarchy<THierachy>
    where TVisitor : IModelHierachyVisitor<THierachy>
  {
    private readonly ModelHierarchy<THierachy> _hierarchy;

    public ModelHierarchyVisitable(ModelHierarchy<THierachy> hierarchy)
    {
      this._hierarchy = hierarchy;
    }

    public T Accept<T>(TVisitor visitor) where T : class
    {
      // ISSUE: reference to a compiler-generated field
      if (ModelHierarchyVisitable<THierachy, TVisitor>.\u003C\u003Eo__2<T>.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ModelHierarchyVisitable<THierachy, TVisitor>.\u003C\u003Eo__2<T>.\u003C\u003Ep__0 = CallSite<Func<CallSite, TVisitor, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Visit", (IEnumerable<Type>) null, typeof (ModelHierarchyVisitable<THierachy, TVisitor>), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = ModelHierarchyVisitable<THierachy, TVisitor>.\u003C\u003Eo__2<T>.\u003C\u003Ep__0.Target((CallSite) ModelHierarchyVisitable<THierachy, TVisitor>.\u003C\u003Eo__2<T>.\u003C\u003Ep__0, visitor, (object) this._hierarchy);
      return !obj.IsNull<object>() ? obj.CastTo<T>() : default (T);
    }

    public object Accept(TVisitor visitor)
    {
      // ISSUE: reference to a compiler-generated field
      if (ModelHierarchyVisitable<THierachy, TVisitor>.\u003C\u003Eo__3.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ModelHierarchyVisitable<THierachy, TVisitor>.\u003C\u003Eo__3.\u003C\u003Ep__0 = CallSite<Func<CallSite, TVisitor, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Visit", (IEnumerable<Type>) null, typeof (ModelHierarchyVisitable<THierachy, TVisitor>), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return ModelHierarchyVisitable<THierachy, TVisitor>.\u003C\u003Eo__3.\u003C\u003Ep__0.Target((CallSite) ModelHierarchyVisitable<THierachy, TVisitor>.\u003C\u003Eo__3.\u003C\u003Ep__0, visitor, (object) this._hierarchy);
    }
  }
}

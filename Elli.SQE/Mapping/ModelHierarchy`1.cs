// Decompiled with JetBrains decompiler
// Type: Elli.SQE.Mapping.ModelHierarchy`1
// Assembly: Elli.SQE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 49809150-F4C8-4F09-B50E-E491713B0B30
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.SQE.Mapping
{
  public class ModelHierarchy<T> where T : ModelHierarchy<T>
  {
    protected ModelHierarchy()
      : this(default (T))
    {
    }

    protected ModelHierarchy(T parent)
    {
      this.Parent = parent;
      this.Children = new List<T>();
      if ((object) parent == null)
        return;
      parent.Children.Add(this as T);
    }

    public T Parent { get; set; }

    public List<T> Children { get; set; }

    public bool IsRoot => (object) this.Parent == null;

    public T Root => !this.IsRoot ? this.Parent.Root : this as T;

    public bool ExcludeFromMapping { get; set; }

    public T Find(Predicate<T> check)
    {
      if (check(this as T))
        return this as T;
      foreach (T child in this.Children)
      {
        T obj = child.Find(check);
        if ((object) obj != null)
          return obj;
      }
      return default (T);
    }

    public IEnumerable<ModelHierarchy<T>> Flatten()
    {
      ModelHierarchy<T> modelHierarchy = this;
      yield return (ModelHierarchy<T>) (modelHierarchy as T);
      foreach (T obj in modelHierarchy.Children.SelectMany<T, T>((Func<T, IEnumerable<T>>) (child => child.FlattenUnsafe())))
        yield return (ModelHierarchy<T>) obj;
    }

    public IEnumerable<T> FlattenUnsafe()
    {
      ModelHierarchy<T> modelHierarchy = this;
      yield return modelHierarchy as T;
      foreach (T obj in modelHierarchy.Children.SelectMany<T, T>((Func<T, IEnumerable<T>>) (child => child.FlattenUnsafe())))
        yield return obj;
    }
  }
}

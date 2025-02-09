// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Set
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public abstract class Set : ISet, ICollection, IEnumerable, ICloneable
  {
    public virtual ISet Union(ISet a)
    {
      ISet set = (ISet) this.Clone();
      if (a != null)
        set.AddAll((ICollection) a);
      return set;
    }

    public static ISet Union(ISet a, ISet b)
    {
      if (a == null && b == null)
        return (ISet) null;
      if (a == null)
        return (ISet) b.Clone();
      return b == null ? (ISet) a.Clone() : a.Union(b);
    }

    public static Set operator |(Set a, Set b) => (Set) Set.Union((ISet) a, (ISet) b);

    public virtual ISet Intersect(ISet a)
    {
      ISet set = (ISet) this.Clone();
      if (a != null)
        set.RetainAll((ICollection) a);
      else
        set.Clear();
      return set;
    }

    public static ISet Intersect(ISet a, ISet b)
    {
      if (a == null && b == null)
        return (ISet) null;
      return a == null ? b.Intersect(a) : a.Intersect(b);
    }

    public static Set operator &(Set a, Set b) => (Set) Set.Intersect((ISet) a, (ISet) b);

    public virtual ISet Minus(ISet a)
    {
      ISet set = (ISet) this.Clone();
      if (a != null)
        set.RemoveAll((ICollection) a);
      return set;
    }

    public static ISet Minus(ISet a, ISet b) => a?.Minus(b);

    public static Set operator -(Set a, Set b) => (Set) Set.Minus((ISet) a, (ISet) b);

    public virtual ISet ExclusiveOr(ISet a)
    {
      ISet set = (ISet) this.Clone();
      foreach (object o in (IEnumerable) a)
      {
        if (set.Contains(o))
          set.Remove(o);
        else
          set.Add(o);
      }
      return set;
    }

    public static ISet ExclusiveOr(ISet a, ISet b)
    {
      if (a == null && b == null)
        return (ISet) null;
      if (a == null)
        return (ISet) b.Clone();
      return b == null ? (ISet) a.Clone() : a.ExclusiveOr(b);
    }

    public static Set operator ^(Set a, Set b) => (Set) Set.ExclusiveOr((ISet) a, (ISet) b);

    public abstract bool Add(object o);

    public abstract bool AddAll(ICollection c);

    public abstract void Clear();

    public abstract bool Contains(object o);

    public abstract bool ContainsAll(ICollection c);

    public abstract bool IsEmpty { get; }

    public abstract bool Remove(object o);

    public abstract bool RemoveAll(ICollection c);

    public abstract bool RetainAll(ICollection c);

    public virtual object Clone()
    {
      Set instance = (Set) Activator.CreateInstance(this.GetType());
      instance.AddAll((ICollection) this);
      return (object) instance;
    }

    public abstract void CopyTo(Array array, int index);

    public abstract int Count { get; }

    public abstract bool IsSynchronized { get; }

    public abstract object SyncRoot { get; }

    public abstract IEnumerator GetEnumerator();

    public override bool Equals(object o)
    {
      if (o == null || !(o is Set) || ((Set) o).Count != this.Count)
        return false;
      foreach (object o1 in (Set) o)
      {
        if (!this.Contains(o1))
          return false;
      }
      return true;
    }

    public override int GetHashCode() => throw new NotImplementedException();
  }
}

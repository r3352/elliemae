// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ImmutableSet
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public sealed class ImmutableSet : Set
  {
    private const string ERROR_MESSAGE = "Object is immutable.";
    private ISet mBasisSet;

    internal ISet BasisSet => this.mBasisSet;

    public ImmutableSet(ISet basisSet) => this.mBasisSet = basisSet;

    public override sealed bool Add(object o)
    {
      throw new NotSupportedException("Object is immutable.");
    }

    public override sealed bool AddAll(ICollection c)
    {
      throw new NotSupportedException("Object is immutable.");
    }

    public override sealed void Clear() => throw new NotSupportedException("Object is immutable.");

    public override sealed bool Contains(object o) => this.mBasisSet.Contains(o);

    public override sealed bool ContainsAll(ICollection c) => this.mBasisSet.ContainsAll(c);

    public override sealed bool IsEmpty => this.mBasisSet.IsEmpty;

    public override sealed bool Remove(object o)
    {
      throw new NotSupportedException("Object is immutable.");
    }

    public override sealed bool RemoveAll(ICollection c)
    {
      throw new NotSupportedException("Object is immutable.");
    }

    public override sealed bool RetainAll(ICollection c)
    {
      throw new NotSupportedException("Object is immutable.");
    }

    public override sealed void CopyTo(Array array, int index)
    {
      this.mBasisSet.CopyTo(array, index);
    }

    public override sealed int Count => this.mBasisSet.Count;

    public override sealed bool IsSynchronized => this.mBasisSet.IsSynchronized;

    public override sealed object SyncRoot => this.mBasisSet.SyncRoot;

    public override sealed IEnumerator GetEnumerator() => this.mBasisSet.GetEnumerator();

    public override sealed object Clone() => (object) new ImmutableSet(this.mBasisSet);

    public override sealed ISet Union(ISet a)
    {
      ISet set = (ISet) this;
      while (set is ImmutableSet)
        set = ((ImmutableSet) set).BasisSet;
      return (ISet) new ImmutableSet(set.Union(a));
    }

    public override sealed ISet Intersect(ISet a)
    {
      ISet set = (ISet) this;
      while (set is ImmutableSet)
        set = ((ImmutableSet) set).BasisSet;
      return (ISet) new ImmutableSet(set.Intersect(a));
    }

    public override sealed ISet Minus(ISet a)
    {
      ISet set = (ISet) this;
      while (set is ImmutableSet)
        set = ((ImmutableSet) set).BasisSet;
      return (ISet) new ImmutableSet(set.Minus(a));
    }

    public override sealed ISet ExclusiveOr(ISet a)
    {
      ISet set = (ISet) this;
      while (set is ImmutableSet)
        set = ((ImmutableSet) set).BasisSet;
      return (ISet) new ImmutableSet(set.ExclusiveOr(a));
    }
  }
}

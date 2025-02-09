// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.SynchronizedSet
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public sealed class SynchronizedSet : Set
  {
    private ISet mBasisSet;
    private object mSyncRoot;

    public SynchronizedSet(ISet basisSet)
    {
      this.mBasisSet = basisSet;
      this.mSyncRoot = basisSet.SyncRoot;
      if (this.mSyncRoot == null)
        throw new NullReferenceException("The Set you specified returned a null SyncRoot.");
    }

    public override sealed bool Add(object o)
    {
      lock (this.mSyncRoot)
        return this.mBasisSet.Add(o);
    }

    public override sealed bool AddAll(ICollection c)
    {
      Set c1;
      lock (c.SyncRoot)
        c1 = (Set) new HybridSet(c);
      lock (this.mSyncRoot)
        return this.mBasisSet.AddAll((ICollection) c1);
    }

    public override sealed void Clear()
    {
      lock (this.mSyncRoot)
        this.mBasisSet.Clear();
    }

    public override sealed bool Contains(object o)
    {
      lock (this.mSyncRoot)
        return this.mBasisSet.Contains(o);
    }

    public override sealed bool ContainsAll(ICollection c)
    {
      Set c1;
      lock (c.SyncRoot)
        c1 = (Set) new HybridSet(c);
      lock (this.mSyncRoot)
        return this.mBasisSet.ContainsAll((ICollection) c1);
    }

    public override sealed bool IsEmpty
    {
      get
      {
        lock (this.mSyncRoot)
          return this.mBasisSet.IsEmpty;
      }
    }

    public override sealed bool Remove(object o)
    {
      lock (this.mSyncRoot)
        return this.mBasisSet.Remove(o);
    }

    public override sealed bool RemoveAll(ICollection c)
    {
      Set c1;
      lock (c.SyncRoot)
        c1 = (Set) new HybridSet(c);
      lock (this.mSyncRoot)
        return this.mBasisSet.RemoveAll((ICollection) c1);
    }

    public override sealed bool RetainAll(ICollection c)
    {
      Set c1;
      lock (c.SyncRoot)
        c1 = (Set) new HybridSet(c);
      lock (this.mSyncRoot)
        return this.mBasisSet.RetainAll((ICollection) c1);
    }

    public override sealed void CopyTo(Array array, int index)
    {
      lock (this.mSyncRoot)
        this.mBasisSet.CopyTo(array, index);
    }

    public override sealed int Count
    {
      get
      {
        lock (this.mSyncRoot)
          return this.mBasisSet.Count;
      }
    }

    public override sealed bool IsSynchronized => true;

    public override sealed object SyncRoot => this.mSyncRoot;

    public override sealed IEnumerator GetEnumerator() => this.mBasisSet.GetEnumerator();

    public override object Clone() => (object) new SynchronizedSet((ISet) this.mBasisSet.Clone());
  }
}

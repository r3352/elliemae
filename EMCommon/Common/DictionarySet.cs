// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DictionarySet
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public abstract class DictionarySet : Set
  {
    protected IDictionary InternalDictionary;
    private static readonly object PlaceholderObject = new object();

    protected object Placeholder => DictionarySet.PlaceholderObject;

    public override bool Add(object o)
    {
      if (this.InternalDictionary[o] != null)
        return false;
      this.InternalDictionary.Add(o, DictionarySet.PlaceholderObject);
      return true;
    }

    public override bool AddAll(ICollection c)
    {
      bool flag = false;
      if (c != null)
      {
        foreach (object o in (IEnumerable) c)
          flag |= this.Add(o);
      }
      return flag;
    }

    public override void Clear() => this.InternalDictionary.Clear();

    public override bool Contains(object o) => this.InternalDictionary[o] != null;

    public override bool ContainsAll(ICollection c)
    {
      foreach (object o in (IEnumerable) c)
      {
        if (!this.Contains(o))
          return false;
      }
      return true;
    }

    public override bool IsEmpty => this.InternalDictionary.Count == 0;

    public override bool Remove(object o)
    {
      bool flag = this.Contains(o);
      if (flag)
        this.InternalDictionary.Remove(o);
      return flag;
    }

    public override bool RemoveAll(ICollection c)
    {
      bool flag = false;
      foreach (object o in (IEnumerable) c)
        flag |= this.Remove(o);
      return flag;
    }

    public override bool RetainAll(ICollection c)
    {
      Set set = (Set) new HybridSet(c);
      Set c1 = (Set) new HybridSet();
      foreach (object o in (Set) this)
      {
        if (!set.Contains(o))
          c1.Add(o);
      }
      return this.RemoveAll((ICollection) c1);
    }

    public override void CopyTo(Array array, int index)
    {
      this.InternalDictionary.Keys.CopyTo(array, index);
    }

    public override int Count => this.InternalDictionary.Count;

    public override bool IsSynchronized => false;

    public override object SyncRoot => this.InternalDictionary.SyncRoot;

    public override IEnumerator GetEnumerator() => this.InternalDictionary.Keys.GetEnumerator();
  }
}

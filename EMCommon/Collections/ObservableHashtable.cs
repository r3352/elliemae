// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Collections.ObservableHashtable
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Collections
{
  [Serializable]
  public class ObservableHashtable : Hashtable
  {
    public event HashtableEventHandler BeforeAddItem;

    public event HashtableEventHandler AfterAddItem;

    public event HashtableEventHandler BeforeChangeItem;

    public event HashtableEventHandler AfterChangeItem;

    protected virtual bool onBeforeAdd(HashtableEventArgs e)
    {
      HashtableEventHandler beforeAddItem = this.BeforeAddItem;
      if (beforeAddItem == null)
        return true;
      beforeAddItem((object) this, e);
      return e.KeepChanges;
    }

    protected virtual void onAfterAdd(HashtableEventArgs e)
    {
      HashtableEventHandler afterAddItem = this.AfterAddItem;
      if (afterAddItem == null)
        return;
      afterAddItem((object) this, e);
    }

    protected virtual bool onBeforeChange(HashtableEventArgs e)
    {
      HashtableEventHandler beforeChangeItem = this.BeforeChangeItem;
      if (beforeChangeItem == null)
        return true;
      beforeChangeItem((object) this, e);
      return e.KeepChanges;
    }

    protected virtual void onAfterChange(HashtableEventArgs e)
    {
      HashtableEventHandler afterChangeItem = this.AfterChangeItem;
      if (afterChangeItem == null)
        return;
      afterChangeItem((object) this, e);
    }

    public override void Add(object key, object value)
    {
      HashtableEventArgs e = new HashtableEventArgs(key, value);
      if (this.onBeforeAdd(e))
        base.Add(key, value);
      this.onAfterAdd(e);
    }

    public override object this[object key]
    {
      get => base[key];
      set
      {
        if (this.ContainsKey(key))
        {
          HashtableEventArgs e = new HashtableEventArgs(key, value);
          if (this.onBeforeChange(e))
            base[key] = value;
          this.onAfterChange(e);
        }
        else
          this.Add(key, value);
      }
    }

    public static ObservableHashtable ConvertFrom(Hashtable table)
    {
      ObservableHashtable observableHashtable = new ObservableHashtable();
      foreach (string key in (IEnumerable) table.Keys)
        observableHashtable.Add((object) key, table[(object) key]);
      return observableHashtable;
    }
  }
}

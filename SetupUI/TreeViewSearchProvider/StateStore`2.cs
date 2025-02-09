// Decompiled with JetBrains decompiler
// Type: TreeViewSearchProvider.StateStore`2
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.Collections.Generic;

#nullable disable
namespace TreeViewSearchProvider
{
  internal class StateStore<K, V>
  {
    private Dictionary<K, V> _state = new Dictionary<K, V>();

    public Dictionary<K, V> StateObject => this._state;

    public V this[K key]
    {
      get => this._state[key];
      set => this._state[key] = value;
    }

    public int Count => this._state.Count;

    public StateStore()
    {
    }

    public StateStore(K key, V value) => this.Add(key, value);

    public bool Add(K key, V value)
    {
      if (!this._state.ContainsKey(key))
        this._state.Add(key, value);
      return true;
    }

    public V GetValue(K t)
    {
      V v;
      this._state.TryGetValue(t, out v);
      return v;
    }

    public void Remove(K key) => this._state.Remove(key);

    public void Clear() => this._state.Clear();
  }
}

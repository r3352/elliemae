// Decompiled with JetBrains decompiler
// Type: Elli.Common.ComponentRegistration.ComponentRegistrationBase
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Elli.Common.ComponentRegistration
{
  public class ComponentRegistrationBase
  {
    private readonly IDictionary<string, object> _components;

    public ComponentRegistrationBase()
      : this((IDictionary) new Dictionary<string, object>())
    {
      this._components = (IDictionary<string, object>) new Dictionary<string, object>();
    }

    public ComponentRegistrationBase(IDictionary dict)
    {
      this._components = dict != null ? (IDictionary<string, object>) new Dictionary<string, object>(dict.Count) : throw new ArgumentNullException(nameof (dict));
      foreach (DictionaryEntry dictionaryEntry in dict)
        this._components.Add(new KeyValuePair<string, object>(dictionaryEntry.Key as string, dictionaryEntry.Value));
    }

    public T GetComponent<T>(ITypedKey<T> key)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      return (T) this._components[key.Name];
    }

    public void RegisterComponent<T>(ITypedKey<T> key, T value)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      this._components[key.Name] = (object) value;
    }

    public void UnRegisterComponent<T>(ITypedKey<T> key)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      this._components.Remove(key.Name);
    }
  }
}

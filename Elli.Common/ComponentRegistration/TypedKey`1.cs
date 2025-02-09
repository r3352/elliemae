// Decompiled with JetBrains decompiler
// Type: Elli.Common.ComponentRegistration.TypedKey`1
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;

#nullable disable
namespace Elli.Common.ComponentRegistration
{
  public class TypedKey<T> : ITypedKey<T>
  {
    private readonly string _name;

    public TypedKey() => this._name = typeof (T).ToString();

    public TypedKey(string name)
    {
      this._name = name != null ? name : throw new ArgumentNullException(nameof (name));
    }

    public string Name => this._name;
  }
}

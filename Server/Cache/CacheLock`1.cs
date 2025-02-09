// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Cache.CacheLock`1
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.Cache
{
  public class CacheLock<T> : ICacheLock<T>, IDisposable
  {
    private string key;
    private bool cacheValue;
    private ICacheStore cache;

    public T Value { get; private set; }

    public IDisposable LockObject { get; private set; }

    public object Identifier { get; private set; }

    public CacheLock(
      string key,
      object identifier,
      T value,
      IDisposable lockObj,
      ICacheStore cache,
      bool cacheValue)
    {
      this.key = key;
      this.Value = value;
      this.LockObject = lockObj;
      this.cache = cache;
      this.Identifier = identifier;
      this.cacheValue = cacheValue;
    }

    public void CheckIn(bool keepCheckedOut)
    {
      if (this.cacheValue)
        this.cache.Put(this.key, (object) this.Value);
      if (keepCheckedOut)
        return;
      this.UndoCheckout();
    }

    public void CheckIn() => this.CheckIn(false);

    public void CheckIn(T newValue)
    {
      this.Value = newValue;
      this.CheckIn(false);
    }

    public void CheckIn(T newValue, bool keepCheckedOut)
    {
      this.Value = newValue;
      this.CheckIn(keepCheckedOut);
    }

    public void UndoCheckout()
    {
      if (this.LockObject == null)
        return;
      this.LockObject.Dispose();
      this.LockObject = (IDisposable) null;
    }

    public void Dispose() => this.UndoCheckout();
  }
}

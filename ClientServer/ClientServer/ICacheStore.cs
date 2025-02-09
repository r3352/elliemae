// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ICacheStore
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Configuration;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ICacheStore
  {
    CacheStoreSource Source { get; }

    CacheSetting Setting { get; }

    object Get(string name);

    T Get<T>(string name);

    IDictionary<string, T> GetAll<T>(string[] names) where T : class;

    void Put(string name, object o);

    void Remove(string name);

    string[] Keys();

    IDisposable Lock(string key, LockType lockType, int timeout, bool suppressTimeoutWarning = false);

    bool IsRemoteCache { get; }

    string GetStats();

    IDisposable EnterContext();

    void ClearAll();
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DisabledEbsV3Cache
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Cache;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class DisabledEbsV3Cache : IDataCache
  {
    public T Get<T>(string key) => default (T);

    public T Get<T>(string key, Func<T> loaderFunc) => loaderFunc();

    public void Set<T>(string key, T value)
    {
    }

    public void Remove(string key)
    {
    }
  }
}

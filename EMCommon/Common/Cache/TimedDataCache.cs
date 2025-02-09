// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Cache.TimedDataCache
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common.Cache
{
  public sealed class TimedDataCache
  {
    private static volatile ILruDataCache _timedCache;
    private static object syncRoot = new object();

    private TimedDataCache()
    {
    }

    public static ILruDataCache DataCache
    {
      get
      {
        if (TimedDataCache._timedCache == null)
        {
          lock (TimedDataCache.syncRoot)
          {
            if (TimedDataCache._timedCache == null)
              TimedDataCache.InitializeCache();
          }
        }
        return TimedDataCache._timedCache;
      }
    }

    private static void InitializeCache()
    {
      TimedDataCache._timedCache = (ILruDataCache) new LruCache("TimedCache", true);
    }

    public static T Get<T>(string key, Func<T> loaderFunc, int absoluteExpirationMinutes)
    {
      return TimedDataCache._timedCache.Get<T>(key, loaderFunc, absoluteExpirationMinutes);
    }
  }
}

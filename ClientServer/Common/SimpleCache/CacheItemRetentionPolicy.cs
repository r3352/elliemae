// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.SimpleCache.CacheItemRetentionPolicy
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common.SimpleCache
{
  public class CacheItemRetentionPolicy
  {
    public TimeSpan AbsoluteExpiration { get; set; }

    public TimeSpan SlidingExpiration { get; set; }

    public CacheItemRetentionPolicy()
    {
      this.SlidingExpiration = TimeSpan.MinValue;
      this.AbsoluteExpiration = new TimeSpan(0, 15, 0);
    }

    public CacheItemRetentionPolicy(TimeSpan slidingExpiration, TimeSpan absoluteExpiration)
    {
      this.AbsoluteExpiration = absoluteExpiration;
      this.SlidingExpiration = slidingExpiration;
    }

    public static CacheItemRetentionPolicy Default => new CacheItemRetentionPolicy();

    public static CacheItemRetentionPolicy ExpireIn5Mins
    {
      get => new CacheItemRetentionPolicy(TimeSpan.MinValue, new TimeSpan(0, 5, 0));
    }

    public static CacheItemRetentionPolicy ExpireIn2Hours
    {
      get => new CacheItemRetentionPolicy(new TimeSpan(0, 5, 0), new TimeSpan(2, 0, 0));
    }

    public static CacheItemRetentionPolicy NoExpiration
    {
      get
      {
        return new CacheItemRetentionPolicy()
        {
          AbsoluteExpiration = TimeSpan.MaxValue,
          SlidingExpiration = TimeSpan.MinValue
        };
      }
    }

    public static CacheItemRetentionPolicy ExpireIn10Mins
    {
      get => new CacheItemRetentionPolicy(TimeSpan.MinValue, new TimeSpan(0, 10, 0));
    }

    public static CacheItemRetentionPolicy NoRetention
    {
      get
      {
        CacheItemRetentionPolicy noRetention = new CacheItemRetentionPolicy();
        noRetention.SlidingExpiration = noRetention.AbsoluteExpiration = TimeSpan.MinValue;
        return noRetention;
      }
    }

    public static CacheItemRetentionPolicy ExpireIn24Hours
    {
      get
      {
        return new CacheItemRetentionPolicy()
        {
          AbsoluteExpiration = TimeSpan.MinValue,
          SlidingExpiration = new TimeSpan(24, 0, 0)
        };
      }
    }

    public static CacheItemRetentionPolicy ExpiresNextDay
    {
      get
      {
        return new CacheItemRetentionPolicy()
        {
          AbsoluteExpiration = new TimeSpan(24, 0, 0),
          SlidingExpiration = TimeSpan.MinValue
        };
      }
    }
  }
}

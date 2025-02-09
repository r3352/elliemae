// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerCommon.HazelCastHealthCheckProvider
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerCommon
{
  public class HazelCastHealthCheckProvider : IHazelCastHealthCheckProvider
  {
    private static readonly HazelCastHealthCheckProvider _instance = new HazelCastHealthCheckProvider();
    private Lazy<HazelCastCacheStore> _hzcCacheStoreForDefaultInstance;
    private Lazy<HazelcastPipelineCacheStore> _hzcPipelineStoreInstance;

    private HazelCastHealthCheckProvider()
    {
      this._hzcCacheStoreForDefaultInstance = new Lazy<HazelCastCacheStore>((Func<HazelCastCacheStore>) (() => new HazelCastCacheStore((string) null, "CACHE")));
      this._hzcPipelineStoreInstance = new Lazy<HazelcastPipelineCacheStore>((Func<HazelcastPipelineCacheStore>) (() => new HazelcastPipelineCacheStore((string) null, (string) null, (string) null, (string) null)));
    }

    public static HazelCastHealthCheckProvider Instance => HazelCastHealthCheckProvider._instance;

    public IHealthCheckCacheStore HzcCacheStoreInstance
    {
      get => (IHealthCheckCacheStore) this._hzcCacheStoreForDefaultInstance.Value;
    }

    public IHealthCheckCacheStore HzcPipelineStoreInstance
    {
      get => (IHealthCheckCacheStore) this._hzcPipelineStoreInstance.Value;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerCommon.PipelineCacheStoreProvider
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

#nullable disable
namespace EllieMae.EMLite.Server.ServerCommon
{
  public class PipelineCacheStoreProvider
  {
    private readonly string _instanceId;
    private readonly string _metadataSuffix;
    private readonly string _chunkSuffix;
    private readonly string _cursorListSuffix;
    private readonly bool _compressionDisabled;
    private readonly bool _cleanupChunkDisabled;

    public PipelineCacheStoreProvider(
      string instanceId,
      string metadataSuffix,
      string chunkSuffix,
      string cursorListSuffix,
      bool compressionDisabled = false,
      bool cleanupChunkDisabled = false)
    {
      this._instanceId = instanceId;
      this._metadataSuffix = metadataSuffix;
      this._cursorListSuffix = cursorListSuffix;
      this._chunkSuffix = chunkSuffix;
      this._compressionDisabled = compressionDisabled;
      this._cleanupChunkDisabled = cleanupChunkDisabled;
    }

    public IPipelineCacheStore GetPipelineCacheStore(
      PipelineCacheStoreProvider.PipelineCacheStoreType pipelineCacheStoreType)
    {
      return pipelineCacheStoreType == PipelineCacheStoreProvider.PipelineCacheStoreType.Hazelcast ? (IPipelineCacheStore) new HazelcastPipelineCacheStore(this._instanceId, this._metadataSuffix, this._chunkSuffix, this._cursorListSuffix, this._compressionDisabled, this._cleanupChunkDisabled) : (IPipelineCacheStore) null;
    }

    public enum PipelineCacheStoreType
    {
      Hazelcast,
    }
  }
}

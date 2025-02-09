// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerCommon.HazelcastPipelineCacheStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Domain.Exceptions;
using EllieMae.EMLite.Server.Configuration;
using Hazelcast;
using Hazelcast.Core;
using Hazelcast.DistributedObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server.ServerCommon
{
  internal class HazelcastPipelineCacheStore : HazelCastCacheStore, IPipelineCacheStore
  {
    private readonly string className = nameof (HazelcastPipelineCacheStore);
    private static HazelcastConnectionPool _connectionPool = new HazelcastConnectionPool((CacheStoreConfiguration) ConfigurationManager.GetSection("loanPipelineCacheStoreSettings"));
    private readonly string _encVerPrefix = "v24_3";
    private readonly string _snapshotKey;
    private readonly string _chunkSuffix;
    private readonly string _metadataSuffix;
    private readonly string _instanceId;
    private readonly bool _cleanupChunkDisabled;
    private readonly bool _compressionDisabled;

    internal HazelcastPipelineCacheStore(
      string instanceId,
      string metadataSuffix,
      string chunkSuffix,
      string cursorListSuffix,
      bool compressionDisabled = false,
      bool cleanupChunkDisabled = false)
      : base(instanceId, "EBS")
    {
      this._compressionDisabled = compressionDisabled;
      this._cleanupChunkDisabled = cleanupChunkDisabled;
      this._metadataSuffix = metadataSuffix;
      this._chunkSuffix = chunkSuffix;
      this._instanceId = instanceId;
      this._snapshotKey = this._encVerPrefix + "." + (string.IsNullOrWhiteSpace(instanceId) ? "DefaultInstance" : instanceId.ToUpper()) + "." + cursorListSuffix;
    }

    public void CreateSnapshotCursor(
      string cursorId,
      int maxCursors,
      int cursorIdleSeconds,
      int chunkSize)
    {
      try
      {
        HazelcastConnection conn = (HazelcastConnection) null;
        try
        {
          conn = HazelcastPipelineCacheStore._connectionPool.Open();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(this.className, string.Format("Failed to connect to Hazelcast cluster. Exception: {1}.", (object) ex.ToString()));
        }
        if (conn == null || conn.Client == null)
          throw new InternalServerErrorException("Unable to connect to Hazelcast cluster.");
        this.HazelCastStoreOperation<bool>(nameof (CreateSnapshotCursor), conn, (Func<IHazelcastClient, Task<bool>>) (async client =>
        {
          IHMap<string, byte[]> map = await conn.Client.GetMapAsync<string, byte[]>(this._mapName);
          if (map == null)
            throw new InternalServerErrorException("Unable to access to Hazelcast map.");
          using (AsyncContext.New())
          {
            if (!await map.TryLockAsync(this._snapshotKey, new TimeSpan(0, 0, 0, 20), new TimeSpan(0, 0, 0, HazelCastCacheStore.leaseTime)))
              throw new InternalServerErrorException("Timeout expired while waiting to acquire lock on the pipeline cursor. Please retry the operation after some time.");
            object obj = (object) null;
            int num = 0;
            bool snapshotCursor;
            try
            {
              List<string> cursorList;
              if (!await map.ContainsKeyAsync(this._snapshotKey))
              {
                cursorList = new List<string>() { cursorId };
                await map.SetAsync(this._snapshotKey, HazelCastCacheStore.Serialize((object) cursorList), TimeSpan.FromMinutes(0.0));
                snapshotCursor = true;
              }
              else
              {
                cursorList = (List<string>) HazelCastCacheStore.Deserialize(await map.GetAsync(this._snapshotKey));
                if (cursorList.Count < maxCursors)
                {
                  cursorList.Add(cursorId);
                  await map.SetAsync(this._snapshotKey, HazelCastCacheStore.Serialize((object) cursorList), TimeSpan.FromMinutes(0.0));
                  snapshotCursor = true;
                }
                else
                {
                  HashSet<string> toBeRemoved = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
                  foreach (string str in cursorList)
                  {
                    string cid = str;
                    if (await map.GetEntryViewAsync(this.BuildMetadataKey(cid)) == null)
                      toBeRemoved.Add(cid);
                    if (cursorList.Count - toBeRemoved.Count >= maxCursors)
                      cid = (string) null;
                    else
                      break;
                  }
                  if (toBeRemoved.Any<string>())
                    cursorList.RemoveAll((Predicate<string>) (x => toBeRemoved.Contains(x)));
                  if (cursorList.Count < maxCursors)
                  {
                    cursorList.Add(cursorId);
                    await map.SetAsync(this._snapshotKey, HazelCastCacheStore.Serialize((object) cursorList), TimeSpan.FromMinutes(0.0));
                    snapshotCursor = true;
                  }
                  else
                  {
                    await map.SetAsync(this._snapshotKey, HazelCastCacheStore.Serialize((object) cursorList), TimeSpan.FromMinutes(0.0));
                    throw new ConflictException("Unable to create a new cursor at this time. Limit for maximum number of active cursors may have been reached.");
                  }
                }
              }
              num = 1;
            }
            catch (object ex)
            {
              obj = ex;
            }
            await map.UnlockAsync(this._snapshotKey);
            object obj1 = obj;
            if (obj1 != null)
            {
              if (!(obj1 is Exception source2))
                throw obj1;
              ExceptionDispatchInfo.Capture(source2).Throw();
            }
            if (num == 1)
              return snapshotCursor;
            obj = (object) null;
          }
          map = (IHMap<string, byte[]>) null;
          bool snapshotCursor1;
          return snapshotCursor1;
        }));
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public void CreateSnapshotCursorMetadata(
      string cursorId,
      string userIdentity,
      int totalCount,
      int entryTTLMinutes,
      int cursorIdleSeconds,
      object filter = null)
    {
      try
      {
        HazelcastConnection conn = (HazelcastConnection) null;
        try
        {
          conn = HazelcastPipelineCacheStore._connectionPool.Open();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(this.className, string.Format("Failed to connect to Hazelcast cluster. Exception: {1}.", (object) ex.ToString()));
        }
        if (conn == null || conn.Client == null)
          throw new InternalServerErrorException("Unable to connect to Hazelcast cluster.");
        this.HazelCastStoreOperation<bool>(nameof (CreateSnapshotCursorMetadata), conn, (Func<IHazelcastClient, Task<bool>>) (async client =>
        {
          IHMap<string, byte[]> mapAsync = await conn.Client.GetMapAsync<string, byte[]>(this._mapName);
          if (mapAsync == null)
            throw new InternalServerErrorException("Unable to access to Hazelcast map.");
          SnapshotCursorMetadata snapshotCursorMetadata = new SnapshotCursorMetadata()
          {
            UserIdentity = userIdentity,
            TotalCount = totalCount
          };
          if (filter != null)
            snapshotCursorMetadata.Filter = filter;
          await mapAsync.SetAsync(this.BuildMetadataKey(cursorId), HazelCastCacheStore.Serialize((object) snapshotCursorMetadata), TimeSpan.FromMinutes((double) entryTTLMinutes), TimeSpan.FromSeconds((double) cursorIdleSeconds));
          return true;
        }));
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public SnapshotCursorMetadata GetSnapshotCursorMetadata(string cursorId)
    {
      try
      {
        HazelcastConnection conn = (HazelcastConnection) null;
        try
        {
          conn = HazelcastPipelineCacheStore._connectionPool.Open();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(this.className, string.Format("Failed to connect to Hazelcast cluster. Exception: {1}.", (object) ex.ToString()));
        }
        if (conn == null || conn.Client == null)
          throw new InternalServerErrorException("Unable to connect to Hazelcast cluster.");
        return this.HazelCastStoreOperation<SnapshotCursorMetadata>(nameof (GetSnapshotCursorMetadata), conn, (Func<IHazelcastClient, Task<SnapshotCursorMetadata>>) (async client =>
        {
          return (SnapshotCursorMetadata) HazelCastCacheStore.Deserialize(await (await conn.Client.GetMapAsync<string, byte[]>(this._mapName) ?? throw new InternalServerErrorException("Unable to access to Hazelcast map.")).GetAsync(this.BuildMetadataKey(cursorId)) ?? throw new NotFoundException(string.Format("Cursor with id '{0}' does not exist or has expired.", (object) cursorId)));
        }));
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public void StoreSnapshotCursorDataItems<T>(
      string cursorId,
      List<T> dataItems,
      int chunkSize,
      int entryTTLMinutes,
      int cursorIdleSeconds)
    {
      int totalCount = dataItems.Count;
      int totalChunks = totalCount / chunkSize + (totalCount % chunkSize == 0 ? 0 : 1);
      try
      {
        HazelcastConnection conn = (HazelcastConnection) null;
        try
        {
          conn = HazelcastPipelineCacheStore._connectionPool.Open();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(this.className, string.Format("Failed to connect to Hazelcast cluster. Exception: {1}.", (object) ex.ToString()));
        }
        if (conn == null || conn.Client == null)
          throw new InternalServerErrorException("Unable to connect to Hazelcast cluster.");
        this.HazelCastStoreOperation<bool>(nameof (StoreSnapshotCursorDataItems), conn, (Func<IHazelcastClient, Task<bool>>) (async client =>
        {
          IHMap<string, byte[]> map = await conn.Client.GetMapAsync<string, byte[]>(this._mapName);
          if (map == null)
            throw new InternalServerErrorException("Unable to access to Hazelcast map.");
          int remainingItems = totalCount;
          for (int i = 0; i < totalChunks; ++i)
          {
            byte[] numArray = HazelCastCacheStore.Serialize((object) dataItems.GetRange(i * chunkSize, remainingItems < chunkSize ? remainingItems : chunkSize));
            await map.SetAsync(this.BuildChunkKey(cursorId, i), numArray, TimeSpan.FromMinutes((double) entryTTLMinutes), TimeSpan.FromSeconds((double) cursorIdleSeconds));
            remainingItems -= chunkSize;
          }
          return true;
        }));
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public List<T> RetrieveSnapshotCursorDataItems<T>(
      string cursorId,
      int startAt,
      int requestItems,
      int totalCount,
      int chunkSize)
    {
      int startChunk = startAt / chunkSize;
      int count = startAt + requestItems > totalCount ? totalCount - startAt : requestItems;
      int endChunk = (startAt + count - 1) / chunkSize;
      List<T> chunkData = new List<T>();
      try
      {
        HazelcastConnection conn = (HazelcastConnection) null;
        try
        {
          conn = HazelcastPipelineCacheStore._connectionPool.Open();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(this.className, string.Format("Failed to connect to Hazelcast cluster. Exception: {1}.", (object) ex.ToString()));
        }
        if (conn == null || conn.Client == null)
          throw new InternalServerErrorException("Unable to connect to Hazelcast cluster.");
        this.HazelCastStoreOperation<bool>("GetSnapshotCursorMetadata", conn, (Func<IHazelcastClient, Task<bool>>) (async client =>
        {
          IHMap<string, byte[]> map = await conn.Client.GetMapAsync<string, byte[]>(this._mapName);
          if (map == null)
            throw new InternalServerErrorException("Unable to access to Hazelcast map.");
          for (int i = startChunk; i <= endChunk; ++i)
            chunkData.AddRange((IEnumerable<T>) HazelCastCacheStore.Deserialize(await map.GetAsync(this.BuildChunkKey(cursorId, i)) ?? throw new NotFoundException(string.Format("Cursor with id '{0}' does not exist or has expired.", (object) cursorId))));
          return true;
        }));
      }
      catch (Exception ex)
      {
        throw ex;
      }
      int index = startAt % chunkSize;
      return chunkData.GetRange(index, count);
    }

    private static DateTime UnixTimeStampToDateTimeUtc(double unixTimeStamp)
    {
      return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(unixTimeStamp).ToUniversalTime();
    }

    private void CleanupChunks(string cursorId, int chunkCount)
    {
      try
      {
        HazelcastConnection conn = (HazelcastConnection) null;
        try
        {
          conn = HazelcastPipelineCacheStore._connectionPool.Open();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(this.className, string.Format("Failed to connect to Hazelcast cluster. Exception: {1}.", (object) ex.ToString()));
        }
        if (conn == null || conn.Client == null)
          throw new InternalServerErrorException("Unable to connect to Hazelcast cluster.");
        this.HazelCastStoreOperation<bool>(nameof (CleanupChunks), conn, (Func<IHazelcastClient, Task<bool>>) (async client =>
        {
          IHMap<string, byte[]> map = await conn.Client.GetMapAsync<string, byte[]>(this._mapName);
          if (map == null)
            throw new InternalServerErrorException("Unable to access to Hazelcast map.");
          for (int i = 0; i < chunkCount; ++i)
          {
            string chunkKey = this.BuildChunkKey(cursorId, i);
            try
            {
              await map.DeleteAsync(chunkKey);
            }
            catch (Exception ex)
            {
              TraceLog.WriteError(this.className, string.Format("Deleting chunk key: {0} throws the exception: {1}.", (object) chunkKey, (object) ex.ToString()));
            }
            chunkKey = (string) null;
          }
          TraceLog.WriteInfo(this.className, string.Format("Cleaned up chunk data for the cursorId: {0} from the Hazelcast map: {1}.", (object) cursorId, (object) this._mapName));
          return true;
        }));
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public string[] GetAllCursors()
    {
      HazelcastConnection conn = (HazelcastConnection) null;
      try
      {
        conn = HazelcastPipelineCacheStore._connectionPool.Open();
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(this.className, string.Format("Failed to connect to Hazelcast cluster. Exception: {1}.", (object) ex.ToString()));
      }
      if (conn == null || conn.Client == null)
        throw new InternalServerErrorException("Unable to connect to Hazelcast cluster.");
      return this.HazelCastStoreOperation<string[]>("CleanupChunks", conn, (Func<IHazelcastClient, Task<string[]>>) (async client =>
      {
        return (await (await conn.Client.GetMapAsync<string, byte[]>(this._mapName) ?? throw new InternalServerErrorException("Unable to access to Hazelcast map.")).GetKeysAsync()).ToArray<string>();
      }));
    }

    public void DeleteCursor(string cursorId)
    {
      HazelcastConnection conn = (HazelcastConnection) null;
      try
      {
        conn = HazelcastPipelineCacheStore._connectionPool.Open();
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(this.className, string.Format("Failed to connect to Hazelcast cluster. Exception: {1}.", (object) ex.ToString()));
      }
      if (conn == null || conn.Client == null)
        throw new InternalServerErrorException("Unable to connect to Hazelcast cluster.");
      this.HazelCastStoreOperation<bool>(nameof (DeleteCursor), conn, (Func<IHazelcastClient, Task<bool>>) (async client =>
      {
        IHMap<string, byte[]> mapAsync = await conn.Client.GetMapAsync<string, byte[]>(this._mapName);
        if (mapAsync == null)
          throw new InternalServerErrorException("Unable to access to Hazelcast map.");
        await mapAsync.DeleteAsync(cursorId);
        return true;
      }));
    }

    public override async Task DoHealthCheck(Func<HazelcastConnection, Task> action)
    {
      await action(HazelcastPipelineCacheStore._connectionPool.Open());
    }

    private string BuildMetadataKey(string cursorId)
    {
      return this._snapshotKey + "." + cursorId + "." + this._metadataSuffix;
    }

    private string BuildChunkKey(string cursorId, int chunkIndex)
    {
      return string.Format("{0}.{1}.{2}.{3}", (object) this._snapshotKey, (object) cursorId, (object) this._chunkSuffix, (object) chunkIndex);
    }
  }
}

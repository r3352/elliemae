// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.HazelcastConnectionPool
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Metrics;
using EllieMae.EMLite.Server.Configuration;
using Hazelcast;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class HazelcastConnectionPool
  {
    private const string ClassName = "HazelcastConnectionPool�";
    private static DateTime lastScaleDown = DateTime.UtcNow;
    private HazelcastOptions options;
    private Queue<QueuedInstance> clients = new Queue<QueuedInstance>();
    private int allowedNewConnections = 1;

    public HazelcastConnectionPool(CacheStoreConfiguration configinfo)
    {
      this.InitializeHazelcastSettings(configinfo);
      this.InitializeHazelCastConnectionPool();
    }

    private void InitializeHazelcastSettings(CacheStoreConfiguration configinfo)
    {
      this.options = new HazelcastOptions();
      string clusterName = configinfo.ClusterName;
      string clusterUsername = configinfo.ClusterUsername;
      string clusterPassword = configinfo.ClusterPassword;
      string[] serverAddress = configinfo.ServerAddress;
      if (!string.IsNullOrEmpty(clusterUsername) || !string.IsNullOrEmpty(clusterPassword))
        this.options.Authentication.ConfigureUsernamePasswordCredentials(clusterUsername, clusterPassword);
      foreach (string str in serverAddress)
        this.options.Networking.Addresses.Add(str);
      this.options.ClusterName = clusterName;
      this.options.Networking.ConnectionTimeoutMilliseconds = configinfo.ConnectionAttemptPeriod;
      this.options.Networking.ConnectionRetry.ClusterConnectionTimeoutMilliseconds = (long) configinfo.ConnectionAttemptPeriod;
      if (configinfo.DisableSSL)
        return;
      this.options.Networking.Ssl.Enabled = true;
    }

    private void InitializeHazelCastConnectionPool()
    {
      int num = ServerGlobals.MaxHazelCastClients;
      if (num < 1)
        num = 1;
      this.clients.Enqueue(new QueuedInstance(new Func<IHazelcastClient>(this.CreateHazelcastConnection)));
      this.allowedNewConnections = num - 1;
    }

    private IHazelcastClient CreateHazelcastConnection()
    {
      IHazelcastClient cli = (IHazelcastClient) null;
      Stopwatch stopwatch = Stopwatch.StartNew();
      IHazelCastMetricsRecorder castMetricsRecorder = new MetricsFactory().CreateHazelCastMetricsRecorder();
      try
      {
        Task.Run<IHazelcastClient>((Func<Task<IHazelcastClient>>) (async () =>
        {
          IHazelcastClient hazelcastClient = await HazelcastClientFactory.StartNewClientAsync(this.options).AsTask();
          return cli = hazelcastClient;
        })).WaitOrThrowOnError();
      }
      catch (Exception ex)
      {
        stopwatch.Stop();
        castMetricsRecorder.IncrementErrorCount("conNonLatency", "none", HazelCastCacheStore.VersionNumber);
        TraceLog.WriteError(nameof (HazelcastConnectionPool), string.Format("HazelCast connection failed with error = {0}", (object) ex.Message));
        throw ex;
      }
      stopwatch.Stop();
      castMetricsRecorder.IncrementHazelCastOperationCount("con", "none", HazelCastCacheStore.VersionNumber);
      TraceLog.WriteDebug(nameof (HazelcastConnectionPool), string.Format("Total time to establish HC connection = {0}", (object) stopwatch.ElapsedMilliseconds));
      return cli;
    }

    private void reconfig()
    {
      List<QueuedInstance> removedClients = new List<QueuedInstance>();
      if ((DateTime.UtcNow - HazelcastConnectionPool.lastScaleDown).TotalSeconds > (double) ServerGlobals.HazelcastUnusedConnectionShutdownIdleTimeInSeconds)
      {
        HazelcastConnectionPool.lastScaleDown = DateTime.UtcNow;
        int num = this.clients.Count - QueuedInstance.TotalRefCount;
        if (num > 1)
        {
          TraceLog.WriteWarning(nameof (HazelcastConnectionPool), string.Format("Shutting down {0} unused connections.", (object) num));
          while (num > 1)
          {
            QueuedInstance queuedInstance = this.clients.Dequeue();
            if (queuedInstance.Refcount == 0)
            {
              removedClients.Add(queuedInstance);
              --num;
              ++this.allowedNewConnections;
            }
            else
              this.clients.Enqueue(queuedInstance);
          }
        }
      }
      this.ShutdownClients(removedClients);
    }

    private void ShutdownClients(List<QueuedInstance> removedClients)
    {
      if (!removedClients.Any<QueuedInstance>())
        return;
      Task.Run((Func<Task>) (async () =>
      {
        foreach (QueuedInstance removedClient in removedClients)
        {
          try
          {
            if (removedClient != null)
            {
              if (removedClient.Instance != null)
                await removedClient.Instance.DisposeAsync();
            }
          }
          catch (Exception ex)
          {
            TraceLog.WriteWarning(nameof (HazelcastConnectionPool), "Exception while shutting down Hazelast client instance: " + ex.Message);
          }
        }
      }));
    }

    public HazelcastConnection Open()
    {
      lock (this.clients)
      {
        this.reconfig();
        if (QueuedInstance.TotalRefCount > this.clients.Count * ServerGlobals.MaxHazelCastRequestsPerConnection && this.allowedNewConnections > 0)
        {
          --this.allowedNewConnections;
          QueuedInstance inst = new QueuedInstance(new Func<IHazelcastClient>(this.CreateHazelcastConnection));
          this.clients.Enqueue(inst);
          return new HazelcastConnection(inst);
        }
        QueuedInstance inst1 = (QueuedInstance) null;
        int count = this.clients.Count;
        int totalRefCount = QueuedInstance.TotalRefCount;
        for (int index = 0; index < count; ++index)
        {
          inst1 = this.clients.Dequeue();
          this.clients.Enqueue(inst1);
          if (inst1.Refcount * count < QueuedInstance.TotalRefCount)
            break;
        }
        return new HazelcastConnection(inst1);
      }
    }

    public enum PoolType
    {
      Cache,
      Pipeline,
    }
  }
}

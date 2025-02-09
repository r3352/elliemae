// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.ChannelManager
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using Elli.MessageQueues.Rmq.Connections;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;

#nullable disable
namespace Elli.MessageQueues.Rmq
{
  internal class ChannelManager : IDisposable
  {
    private static ConcurrentDictionary<string, ConcurrentQueue<IModel>> _connectionPool = (ConcurrentDictionary<string, ConcurrentQueue<IModel>>) null;
    private static ChannelManager _channelManager = (ChannelManager) null;
    private static object _channelPoolLock = new object();
    private static bool disposed = false;
    private static string _className = nameof (ChannelManager);

    private bool EnableConnectionPool
    {
      get
      {
        bool result = false;
        bool.TryParse(ConfigurationManager.AppSettings["RMQ.EnableConnectionPool"], out result);
        return result;
      }
    }

    private int MaxPoolCount
    {
      get
      {
        int result = 50;
        int.TryParse(ConfigurationManager.AppSettings["RMQ.MaxPoolCount"], out result);
        return result;
      }
    }

    public static ChannelManager GetInstance()
    {
      if (ChannelManager._channelManager == null)
      {
        lock (ChannelManager._channelPoolLock)
        {
          if (ChannelManager._channelManager == null)
            ChannelManager._channelManager = new ChannelManager();
        }
      }
      return ChannelManager._channelManager;
    }

    private ChannelManager()
    {
      if (ChannelManager._connectionPool != null)
        return;
      ChannelManager._connectionPool = new ConcurrentDictionary<string, ConcurrentQueue<IModel>>();
    }

    public IModel GetChannel(string endPoint, string virtualHost)
    {
      try
      {
        if (this.EnableConnectionPool)
        {
          string key = endPoint + virtualHost;
          ConcurrentQueue<IModel> orAdd = ChannelManager._connectionPool.GetOrAdd(key, (Func<string, ConcurrentQueue<IModel>>) (init => new ConcurrentQueue<IModel>()));
          Global.Logger.WarnFormat("ClassName={0}, MethodName=GetChannel, host={1}, Message=Obtaining channelPoolQueue {2}", (object) ChannelManager._className, (object) key, (object) orAdd);
          if (orAdd != null)
          {
            Global.Logger.InfoFormat("ClassName={0}, MethodName=GetChannel, host={1}, PoolCount={2}", (object) ChannelManager._className, (object) key, (object) orAdd.Count);
            if (orAdd.Count > this.MaxPoolCount)
            {
              Global.Logger.WarnFormat("ClassName={0}, MethodName=GetChannel, PoolCount={1}, is > {2}, Cleanup started", (object) ChannelManager._className, (object) orAdd.Count, (object) this.MaxPoolCount);
              IModel result = (IModel) null;
              while (orAdd.TryDequeue(out result))
              {
                try
                {
                  result.Close();
                  result.Abort();
                  result.Dispose();
                }
                catch (Exception ex)
                {
                  Global.Logger.ErrorFormat("ClassName={0}, MethodName=GetChannel, Exception while dispose channel-GetChannel-Cleanup - {1}", (object) ChannelManager._className, (object) ex.StackTrace);
                }
              }
              Global.Logger.InfoFormat("ClassName={0}, MethodName=GetChannel, PoolCount={1}, Cleanup completed", (object) ChannelManager._className, (object) orAdd.Count);
            }
            IModel result1 = (IModel) null;
            int num = 0;
            while (orAdd.TryDequeue(out result1))
            {
              ++num;
              if (result1.IsOpen)
              {
                Global.Logger.WarnFormat("ClassName={0}, MethodName=GetChannel, PoolCount={1}, LoopCount={3} Channel assigned to the worker - {2}", (object) ChannelManager._className, (object) orAdd.Count, (object) result1.ToString(), (object) num);
                return result1;
              }
              try
              {
                Global.Logger.WarnFormat("ClassName={0}, MethodName=GetChannel, PoolCount={1}, LoopCount={4}, Closing the channel -- {2} - isClosed - {3}", (object) ChannelManager._className, (object) orAdd.Count, (object) result1.ToString(), (object) result1.IsClosed.ToString(), (object) num);
                result1.Close();
                result1.Abort();
                result1.Dispose();
              }
              catch (Exception ex)
              {
                Global.Logger.ErrorFormat("ClassName={0}, MethodName=GetChannel, Exception while dispose channel-GetChannel - {1}", (object) ChannelManager._className, (object) ex.StackTrace);
              }
            }
          }
          Global.Logger.InfoFormat("ClassName={0}, MethodName=GetChannel, host={1}, No channel found... creating one", (object) ChannelManager._className, (object) key);
          return this.CreateNewChannel(endPoint, virtualHost);
        }
        IConnection sharedConnection = ManagedConnectionFactory.SharedConnections[endPoint + virtualHost];
        Global.Logger.InfoFormat("ClassName={0}, ClassicRMQ : Channel created for the connection -- {1}", (object) ChannelManager._className, (object) (endPoint + virtualHost));
        return sharedConnection.CreateModel();
      }
      catch (Exception ex)
      {
        Global.Logger.ErrorFormat("ClassName={0}, MethodName=GetChannel, Exception - {1} for connectionstring - {2}", (object) ChannelManager._className, (object) ex.StackTrace, (object) (endPoint + virtualHost));
        throw ex;
      }
    }

    private IModel CreateNewChannel(string endPoint, string virtualHost)
    {
      IConnection sharedConnection = ManagedConnectionFactory.SharedConnections[endPoint + virtualHost];
      Global.Logger.WarnFormat("ClassName={0}, MethodName=CreateNewChannel, Create new channel for the connection -- {1}", (object) ChannelManager._className, (object) (endPoint + virtualHost));
      return sharedConnection.CreateModel();
    }

    public void PutChannel(string endPoint, string virtualHost, IModel model)
    {
      try
      {
        if (this.EnableConnectionPool)
        {
          if (model != null)
          {
            ConcurrentQueue<IModel> concurrentQueue;
            if (ChannelManager._connectionPool.TryGetValue(endPoint + virtualHost, out concurrentQueue))
            {
              concurrentQueue.Enqueue(model);
              Global.Logger.WarnFormat("ClassName={0},  MethodName=PutChannel, PoolCount={1} Message=Channel released back to the pool - {2} ", (object) ChannelManager._className, (object) concurrentQueue.Count, (object) model.ToString());
            }
            else
              Global.Logger.InfoFormat("ClassName={0},  MethodName=PutChannel, Message=Cannot enqueue model, Model={1}", (object) ChannelManager._className, (object) model.ToString());
          }
          else
            Global.Logger.InfoFormat("ClassName={0},  MethodName=PutChannel, Message=Cannot enqueue model, Model=null", (object) ChannelManager._className);
        }
        else
        {
          Global.Logger.InfoFormat("ClassName={0}, MethodName=PutChannel, ClassicRMQ : Channel -- {1} disposed", (object) ChannelManager._className, (object) model.ToString());
          model.Abort();
          model.Dispose();
        }
      }
      catch (Exception ex)
      {
        Global.Logger.ErrorFormat("ClassName={0}, MethodName=PutChannel, Exception - {1} for connectionstring - {2}", (object) ChannelManager._className, (object) ex.StackTrace, (object) (endPoint + virtualHost));
        throw ex;
      }
    }

    public void DisposeConnection()
    {
      if (this.EnableConnectionPool)
        this.Dispose();
      GC.SuppressFinalize((object) this);
    }

    public void Dispose()
    {
      if (ChannelManager.disposed)
        return;
      if (ChannelManager._connectionPool != null)
      {
        foreach (ConcurrentQueue<IModel> concurrentQueue in (IEnumerable<ConcurrentQueue<IModel>>) ChannelManager._connectionPool.Values)
        {
          IModel result = (IModel) null;
          while (concurrentQueue.TryDequeue(out result))
          {
            result.Close();
            result.Abort();
            result.Dispose();
          }
        }
        ChannelManager._connectionPool.Clear();
      }
      ChannelManager.disposed = true;
    }

    ~ChannelManager()
    {
      try
      {
        Global.Logger.WarnFormat("ClassName={0}, Caller forgot to dispose the object", (object) ChannelManager._className);
        this.Dispose();
      }
      catch (Exception ex)
      {
        Global.Logger.ErrorFormat("ClassName={0} , Error : {1}  in finalizer during disposing the channel object", (object) ChannelManager._className, (object) ex.Message);
      }
      finally
      {
        // ISSUE: explicit finalizer call
        base.Finalize();
      }
    }
  }
}

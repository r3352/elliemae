// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.TradeLoanUpdateQueueManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class TradeLoanUpdateQueueManager : SessionBoundObject, ITradeLoanUpdateQueueManager
  {
    private const string className = "TradeLoanUpdateQueueManager";

    public TradeLoanUpdateQueueManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (TradeLoanUpdateQueueManager).ToLower());
      return this;
    }

    public virtual TradeLoanUpdateJobInfo[] GetAllQueues()
    {
      this.onApiCalled(nameof (TradeLoanUpdateQueueManager), nameof (GetAllQueues), Array.Empty<object>());
      try
      {
        return TradeLoanUpdateQueues.GetAllTradeLoanUpdateQueues();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (TradeLoanUpdateQueueManager), ex, this.Session.SessionInfo);
        return (TradeLoanUpdateJobInfo[]) null;
      }
    }

    public virtual TradeLoanUpdateJobInfo GetQueue(string jobGuid)
    {
      this.onApiCalled(nameof (TradeLoanUpdateQueueManager), nameof (GetQueue), Array.Empty<object>());
      try
      {
        return TradeLoanUpdateQueues.GetQueue(jobGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (TradeLoanUpdateQueueManager), ex, this.Session.SessionInfo);
        return (TradeLoanUpdateJobInfo) null;
      }
    }

    public virtual void CreateQueue(TradeLoanUpdateJobInfo info)
    {
      this.onApiCalled(nameof (TradeLoanUpdateQueueManager), nameof (CreateQueue), Array.Empty<object>());
      try
      {
        TradeLoanUpdateQueues.CreateQueue(info);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (TradeLoanUpdateQueueManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateQueue(TradeLoanUpdateJobInfo info)
    {
      this.onApiCalled(nameof (TradeLoanUpdateQueueManager), nameof (UpdateQueue), Array.Empty<object>());
      try
      {
        TradeLoanUpdateQueues.UpdateQueue(info);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (TradeLoanUpdateQueueManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateQueue(Dictionary<string, object> info, string jobGuid)
    {
      this.onApiCalled(nameof (TradeLoanUpdateQueueManager), nameof (UpdateQueue), Array.Empty<object>());
      try
      {
        TradeLoanUpdateQueues.UpdateQueue(info, jobGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (TradeLoanUpdateQueueManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteQueue(string jobGuid)
    {
      this.onApiCalled(nameof (TradeLoanUpdateQueueManager), nameof (DeleteQueue), Array.Empty<object>());
      try
      {
        TradeLoanUpdateQueues.DeleteQueue(jobGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (TradeLoanUpdateQueueManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual int DeleteQueues(DateTime deleteBefore)
    {
      this.onApiCalled(nameof (TradeLoanUpdateQueueManager), nameof (DeleteQueues), Array.Empty<object>());
      try
      {
        return TradeLoanUpdateQueues.DeleteQueues(deleteBefore);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (TradeLoanUpdateQueueManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }
  }
}

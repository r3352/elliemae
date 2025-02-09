// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.AsyncEventResult
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Belikov.GenuineChannels;
using System;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Threading;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class AsyncEventResult : IAsyncResult
  {
    private DelegateInvoker invoker;
    private ManualResetEvent waitHandle = new ManualResetEvent(false);
    private AsyncEventStatus status = AsyncEventStatus.Started;
    private Exception exception;

    public AsyncEventResult(DelegateInvoker invoker)
    {
      this.invoker = invoker;
      this.status = AsyncEventStatus.Queued;
    }

    public AsyncEventStatus EventStatus
    {
      get
      {
        lock (this)
          return this.status;
      }
    }

    public Exception Exception
    {
      get
      {
        lock (this)
          return this.exception;
      }
    }

    public object AsyncState => (object) this.invoker;

    public bool CompletedSynchronously => false;

    public WaitHandle AsyncWaitHandle => (WaitHandle) this.waitHandle;

    public bool IsCompleted
    {
      get
      {
        lock (this)
          return this.status >= AsyncEventStatus.Success;
      }
    }

    public bool WaitOne(TimeSpan timespan, bool exitContext)
    {
      return this.waitHandle.WaitOne(timespan, exitContext);
    }

    public void SetComplete(Exception ex)
    {
      lock (this)
      {
        switch (ex)
        {
          case null:
            this.status = AsyncEventStatus.Success;
            break;
          case SocketException _:
          case OperationException _:
          case RemotingException _:
            this.status = AsyncEventStatus.Disconnected;
            break;
          default:
            this.status = AsyncEventStatus.Failed;
            break;
        }
        this.exception = ex;
      }
      this.waitHandle.Set();
    }
  }
}

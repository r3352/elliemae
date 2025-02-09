// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.SafeSemaphore
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

#nullable disable
namespace Elli.MessageQueues
{
  [ExcludeFromCodeCoverage]
  public class SafeSemaphore : IDisposable
  {
    private readonly Semaphore _semaphore;
    private volatile bool _dispose;

    public SafeSemaphore(int initialCount, int maximumCount)
    {
      this._semaphore = new Semaphore(initialCount, maximumCount);
    }

    public SafeSemaphore(int initialCount, int maximumCount, string name)
    {
      this._semaphore = new Semaphore(initialCount, maximumCount, name);
    }

    public void WaitOne()
    {
      try
      {
        if (this._dispose)
          return;
        this._semaphore.WaitOne();
      }
      catch (Exception ex)
      {
        Global.FaultHandler.HandleFault(ex);
      }
    }

    public void Release()
    {
      try
      {
        if (this._dispose)
          return;
        this._semaphore.Release();
      }
      catch (Exception ex)
      {
        Global.FaultHandler.HandleFault(ex);
      }
    }

    public void Release(int releaseCount)
    {
      try
      {
        if (this._dispose)
          return;
        this._semaphore.Release(releaseCount);
      }
      catch (Exception ex)
      {
        Global.FaultHandler.HandleFault(ex);
      }
    }

    public void Close()
    {
      try
      {
        this._semaphore.Close();
      }
      catch (Exception ex)
      {
        Global.FaultHandler.HandleFault(ex);
      }
    }

    public void Dispose()
    {
      lock (this._semaphore)
      {
        if (this._dispose)
          return;
        this._semaphore.Dispose();
        this._dispose = true;
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.DefaultRetryPolicy
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System;
using System.Threading;

#nullable disable
namespace Elli.MessageQueues
{
  public class DefaultRetryPolicy : IRetryPolicy
  {
    private const int DelayGap = 1000;
    private readonly int _maxDelayTime;
    private volatile bool _iswaiting;
    private volatile int _delayTime;

    public DefaultRetryPolicy(int maxDelayTime = 300000)
    {
      this._maxDelayTime = maxDelayTime;
      this._delayTime = 0;
    }

    public int DelayTime
    {
      get => this._delayTime;
      set => this._delayTime = value;
    }

    public bool IsWaiting
    {
      get => this._iswaiting;
      set => this._iswaiting = value;
    }

    public void WaitForNextRetry(Action retryingAction)
    {
      try
      {
        this._iswaiting = true;
        new Timer((TimerCallback) (state =>
        {
          this._delayTime = this._delayTime == 0 ? 1000 : this._delayTime * 2;
          if (this._delayTime > this._maxDelayTime)
            this._delayTime = this._maxDelayTime;
          this._iswaiting = false;
          retryingAction();
        })).Change(this._delayTime, -1);
      }
      catch (Exception ex)
      {
        Global.FaultHandler.HandleFault("Received Exception : {0}  during retryingAction.", (object) ex.Message);
      }
    }

    public void Reset()
    {
      this._delayTime = 0;
      this._iswaiting = false;
    }
  }
}

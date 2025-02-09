// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServiceObjects.KafkaEvent.KafkaEventPublisherCircuitBreaker
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Server.ServerObjects.Deferrables.CircuitBreakerUtil;
using Polly;
using Polly.CircuitBreaker;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServiceObjects.KafkaEvent
{
  internal class KafkaEventPublisherCircuitBreaker : CircuitBreakerBase
  {
    private static KafkaEventPublisherCircuitBreaker _instance = new KafkaEventPublisherCircuitBreaker();

    private KafkaEventPublisherCircuitBreaker()
    {
      this._className = nameof (KafkaEventPublisherCircuitBreaker);
      this._actionName = "publish kafka event";
    }

    public static KafkaEventPublisherCircuitBreaker Instance
    {
      get => KafkaEventPublisherCircuitBreaker._instance;
    }

    protected override CircuitBreakerPolicy CreateBreaker()
    {
      int serverSetting = (int) ClientContext.GetCurrent().Settings.GetServerSetting("Policies.KafkaEventPublishCircuitResetMs");
      return Policy.Handle<Exception>().CircuitBreaker(10, TimeSpan.FromMilliseconds((double) serverSetting), new Action<Exception, TimeSpan>(this.OnBreak), new Action(this.OnReset));
    }

    public override bool Execute(Action action, string logData)
    {
      bool bRC = false;
      try
      {
        this.GetBreaker();
        this._breaker.Execute((Action) (() =>
        {
          action();
          bRC = true;
        }));
      }
      catch (BrokenCircuitException ex)
      {
        this.OnBrokenCircuitException(logData, ex);
      }
      catch (Exception ex)
      {
        this.OnException(logData, ex);
      }
      return bRC;
    }

    private void OnBreak(Exception exception, TimeSpan timespan)
    {
      TraceLog.WriteError(this._className, string.Format("Failed to publish kafka event.  Circuite breaker is open and not allowing calls.  {0}", (object) exception.Message));
    }

    private void OnReset()
    {
      TraceLog.WriteInfo(this._className, "Publish kafka event circuit breaker is reset.  The circuit breaker is in a closed state and allowing calls.");
    }
  }
}

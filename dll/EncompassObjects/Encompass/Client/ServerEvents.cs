// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.ServerEvents
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using System;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  [ComSourceInterfaces(typeof (IServerEventsInterface))]
  public class ServerEvents : SessionBoundObject, IServerEvents
  {
    private Hashtable eventHandlers = new Hashtable();

    internal ServerEvents(Session session)
      : base(session)
    {
    }

    public event ConnectionMonitorEventHandler ConnectionMonitor
    {
      add
      {
        if (value == null)
          return;
        this.addHandler<ConnectionMonitorEventArgs>("ConnectionMonitor", new ScopedEventHandler<ConnectionMonitorEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
        this.subscribeTo(typeof (ConnectionEvent));
      }
      remove
      {
        if (value == null || this.removeHandler<ConnectionMonitorEventArgs>(new ScopedEventHandler<ConnectionMonitorEventArgs>.EventHandlerT((object) value, __methodptr(Invoke))) != 0)
          return;
        this.unsubscribeTo(typeof (ConnectionEvent));
      }
    }

    public event ExceptionMonitorEventHandler ExceptionMonitor
    {
      add
      {
        if (value == null)
          return;
        this.addHandler<ExceptionMonitorEventArgs>("ExceptionMonitor", new ScopedEventHandler<ExceptionMonitorEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
        this.subscribeTo(typeof (ExceptionEvent));
      }
      remove
      {
        if (value == null || this.removeHandler<ExceptionMonitorEventArgs>(new ScopedEventHandler<ExceptionMonitorEventArgs>.EventHandlerT((object) value, __methodptr(Invoke))) != 0)
          return;
        this.unsubscribeTo(typeof (ExceptionEvent));
      }
    }

    public event LicenseMonitorEventHandler LicenseMonitor
    {
      add
      {
        if (value == null)
          return;
        this.addHandler<LicenseMonitorEventArgs>("LicenseMonitor", new ScopedEventHandler<LicenseMonitorEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
        this.subscribeTo(typeof (LicenseEvent));
      }
      remove
      {
        if (value == null || this.removeHandler<LicenseMonitorEventArgs>(new ScopedEventHandler<LicenseMonitorEventArgs>.EventHandlerT((object) value, __methodptr(Invoke))) != 0)
          return;
        this.unsubscribeTo(typeof (LicenseEvent));
      }
    }

    public event LoanMonitorEventHandler LoanMonitor
    {
      add
      {
        if (value == null)
          return;
        this.addHandler<LoanMonitorEventArgs>("LoanMonitor", new ScopedEventHandler<LoanMonitorEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
        this.subscribeTo(typeof (LoanEvent));
      }
      remove
      {
        if (value == null || this.removeHandler<LoanMonitorEventArgs>(new ScopedEventHandler<LoanMonitorEventArgs>.EventHandlerT((object) value, __methodptr(Invoke))) != 0)
          return;
        this.unsubscribeTo(typeof (LoanEvent));
      }
    }

    public event SessionMonitorEventHandler SessionMonitor
    {
      add
      {
        if (value == null)
          return;
        this.addHandler<SessionMonitorEventArgs>("SessionMonitor", new ScopedEventHandler<SessionMonitorEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
        this.subscribeTo(typeof (SessionEvent));
      }
      remove
      {
        if (value == null || this.removeHandler<SessionMonitorEventArgs>(new ScopedEventHandler<SessionMonitorEventArgs>.EventHandlerT((object) value, __methodptr(Invoke))) != 0)
          return;
        this.unsubscribeTo(typeof (SessionEvent));
      }
    }

    internal void RaiseEvent(ServerMonitorEvent evnt)
    {
      switch (evnt)
      {
        case ConnectionEvent _:
          this.notifyHandlers<ConnectionMonitorEventArgs>(new ConnectionMonitorEventArgs(evnt as ConnectionEvent));
          break;
        case LicenseEvent _:
          this.notifyHandlers<LicenseMonitorEventArgs>(new LicenseMonitorEventArgs(evnt as LicenseEvent));
          break;
        case ExceptionEvent _:
          this.notifyHandlers<ExceptionMonitorEventArgs>(new ExceptionMonitorEventArgs(evnt as ExceptionEvent));
          break;
        case LoanEvent _:
          this.notifyHandlers<LoanMonitorEventArgs>(new LoanMonitorEventArgs(evnt as LoanEvent));
          break;
        case SessionEvent _:
          this.notifyHandlers<SessionMonitorEventArgs>(new SessionMonitorEventArgs(evnt as SessionEvent));
          break;
      }
    }

    private void notifyHandlers<ArgsT>(ArgsT eventArgs)
    {
      string name = typeof (ArgsT).Name;
      ScopedEventHandler<ArgsT> scopedEventHandler = (ScopedEventHandler<ArgsT>) null;
      lock (this)
        scopedEventHandler = (ScopedEventHandler<ArgsT>) this.eventHandlers[(object) name];
      try
      {
        if (scopedEventHandler == null)
          return;
        scopedEventHandler((object) this, eventArgs);
      }
      catch
      {
      }
    }

    private void subscribeTo(Type eventType) => this.Session.Unwrap().RegisterForEvents(eventType);

    private void unsubscribeTo(Type eventType)
    {
      this.Session.Unwrap().UnregisterForEvents(eventType);
    }

    private void addHandler<ArgsT>(
      string eventName,
      ScopedEventHandler<ArgsT>.EventHandlerT handler)
    {
      lock (this)
      {
        string name = typeof (ArgsT).Name;
        if (!this.eventHandlers.Contains((object) name))
          this.eventHandlers[(object) name] = (object) new ScopedEventHandler<ArgsT>("ServerEvent", eventName);
        ScopedEventHandler<ArgsT> eventHandler = (ScopedEventHandler<ArgsT>) this.eventHandlers[(object) name];
        if (eventHandler.Contains(handler))
          return;
        eventHandler.Add(handler);
      }
    }

    private int removeHandler<ArgsT>(ScopedEventHandler<ArgsT>.EventHandlerT handler)
    {
      lock (this)
      {
        ScopedEventHandler<ArgsT> eventHandler = (ScopedEventHandler<ArgsT>) this.eventHandlers[(object) typeof (ArgsT).Name];
        if (eventHandler == null)
          return 0;
        int num;
        eventHandler.Remove(handler, ref num);
        return num;
      }
    }
  }
}

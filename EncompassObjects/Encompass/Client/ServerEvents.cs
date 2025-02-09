// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.ServerEvents
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using System;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>Provides access to global server events.</summary>
  /// <remarks>Using the ServerEvents class it is possible to monitor server-wide events
  /// such as logins, license management and loan actions (e.g. users opening or closing a loan).
  /// To receive these events your code must subscribe to one or more of the event handlers
  /// provided by this class. Each event handler is meant to provide information on different
  /// types of activities on the server.
  /// <p>Note that if your company uses multiple Encompass Servers, only the events that occur
  /// on the server to which the connection is made will be received. To receive events for
  /// all of your Encompass servers you must establish a <see cref="T:EllieMae.Encompass.Client.Session" /> with each server
  /// and subscribe to its events.</p>
  /// </remarks>
  /// <example>
  /// The following code opens a session to the Encompass Server and starts monitoring
  /// for session-related events. Any events detected are display on the console.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// 
  /// class ServerMonitor
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Start monitoring session-related event
  ///       session.ServerEvents.SessionMonitor += new SessionMonitorEventHandler(ServerEvents_SessionMonitor);
  /// 
  ///       // Suspend indefinitely
  ///       Console.ReadLine();
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// 
  ///    private static void ServerEvents_SessionMonitor(object sender, SessionMonitorEventArgs e)
  ///    {
  ///       Console.WriteLine("Session " + e.SessionInformation.SessionID +
  ///          " (" + e.SessionInformation.ClientHostname + "/" +
  ///          e.SessionInformation.ClientIPAddress + "): " + e.EventType +
  ///          " for user " + e.SessionInformation.UserID);
  ///    }
  /// 
  /// }
  /// ]]>
  /// </code>
  /// </example>
  [ComSourceInterfaces(typeof (IServerEventsInterface))]
  public class ServerEvents : SessionBoundObject, IServerEvents
  {
    private Hashtable eventHandlers = new Hashtable();

    internal ServerEvents(Session session)
      : base(session)
    {
    }

    /// <summary>
    /// Provides notification of low-level connection events on the Encompass Server.
    /// </summary>
    /// <remarks>A connection events is fired whenever a new TCP/IP connections is initiated from
    /// a remote client machine. Additionally, an evet is fired when the connection is closed,
    /// although the event will often trail the actual closing of the connection by several
    /// minutes as the server is setup to allow a client to reconnect in the event of a low-level
    /// connection failure.</remarks>
    public event ConnectionMonitorEventHandler ConnectionMonitor
    {
      add
      {
        if (value == null)
          return;
        this.addHandler<ConnectionMonitorEventArgs>("ConnectionMonitor", new ScopedEventHandler<ConnectionMonitorEventArgs>.EventHandlerT(value.Invoke));
        this.subscribeTo(typeof (ConnectionEvent));
      }
      remove
      {
        if (value == null || this.removeHandler<ConnectionMonitorEventArgs>(new ScopedEventHandler<ConnectionMonitorEventArgs>.EventHandlerT(value.Invoke)) != 0)
          return;
        this.unsubscribeTo(typeof (ConnectionEvent));
      }
    }

    /// <summary>
    /// Provides notification of error and other exception-type events on the Encompass Server.
    /// </summary>
    /// <remarks>Most exception events will occur only when an unhandled exception occurs within
    /// the Encompass Server. However, you can also use this handler to catch <see cref="T:EllieMae.Encompass.Client.LoginException" />
    /// events so you can monitor failed logins to your Encompass server.</remarks>
    /// <example>
    /// The following code opens a session to the Encompass Server and starts monitoring
    /// for exception-related events, in particular those that deal with failed logins.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// 
    /// class ServerMonitor
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Start monitoring exception-related event
    ///       session.ServerEvents.ExceptionMonitor += new ExceptionMonitorEventHandler(ServerEvents_ExceptionMonitor);
    /// 
    ///       // Suspend indefinitely
    ///       Console.ReadLine();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// 
    ///    private static void ServerEvents_ExceptionMonitor(object sender, ExceptionMonitorEventArgs e)
    ///    {
    ///       // Check if the exception is a LoginException. If so, log that exception.
    ///       LoginException ex = e.Exception as LoginException;
    /// 
    ///       if (ex != null)
    ///       {
    ///          Console.WriteLine("A login attempt for user '" + ex.UserID + "' from client " + ex.ClientIPAddress
    ///             + " has failed. The reason was: " + ex.ErrorType);
    ///       }
    ///    }
    /// 
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public event ExceptionMonitorEventHandler ExceptionMonitor
    {
      add
      {
        if (value == null)
          return;
        this.addHandler<ExceptionMonitorEventArgs>("ExceptionMonitor", new ScopedEventHandler<ExceptionMonitorEventArgs>.EventHandlerT(value.Invoke));
        this.subscribeTo(typeof (ExceptionEvent));
      }
      remove
      {
        if (value == null || this.removeHandler<ExceptionMonitorEventArgs>(new ScopedEventHandler<ExceptionMonitorEventArgs>.EventHandlerT(value.Invoke)) != 0)
          return;
        this.unsubscribeTo(typeof (ExceptionEvent));
      }
    }

    /// <summary>
    /// Provides notification of licensing-related events on the Encompass Server.
    /// </summary>
    /// <remarks>When a client connection is made the Encompass Server attempts to allocate
    /// a license to it from the license pool. These event indicate the success or failure of that
    /// process as well as notifying you when a license is released back to the pool. You can
    /// use these event to track your license usage over time.</remarks>
    public event LicenseMonitorEventHandler LicenseMonitor
    {
      add
      {
        if (value == null)
          return;
        this.addHandler<LicenseMonitorEventArgs>("LicenseMonitor", new ScopedEventHandler<LicenseMonitorEventArgs>.EventHandlerT(value.Invoke));
        this.subscribeTo(typeof (LicenseEvent));
      }
      remove
      {
        if (value == null || this.removeHandler<LicenseMonitorEventArgs>(new ScopedEventHandler<LicenseMonitorEventArgs>.EventHandlerT(value.Invoke)) != 0)
          return;
        this.unsubscribeTo(typeof (LicenseEvent));
      }
    }

    /// <summary>
    /// Provides notification of loan-related events on the Encompass Server.
    /// </summary>
    /// <remarks>Use the loan monitor events to track when loans are opened, saved,
    /// closed, imported, etc. Whenever such an action occurs, the server raises an
    /// event which can be captured and acted upon. The event information includes both
    /// information on the user who performed the event as well as the loan on which the
    /// action was taken.
    /// <p>Using the LoanMonitor event, it is possible to set up real-time synchronization
    /// with an external data system. The example below demonstrates how this can be done.</p>
    /// </remarks>
    /// <example>
    /// The following code demonstrates how you can watch for changes to any loan in
    /// Encompass and, when one occurs, extract data from the affected loan. This data
    /// could then be saved into a remote system as a form of real-time synchronization.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// 
    /// class ServerMonitor
    /// {
    ///    // Declare the session globally so it can be accessed in the event handler
    ///    static Session session = null;
    /// 
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Start monitoring loan-related event
    ///       session.ServerEvents.LoanMonitor += new LoanMonitorEventHandler(ServerEvents_LoanMonitor);
    /// 
    ///       // Suspend indefinitely
    ///       Console.ReadLine();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// 
    ///    private static void ServerEvents_LoanMonitor(object sender, LoanMonitorEventArgs e)
    ///    {
    ///       // When a loan is saved, open the loan and retrieve its information
    ///       if (e.EventType == LoanMonitorEventType.Saved)
    ///       {
    ///          Loan loan = session.Loans.Open(e.LoanIdentity.Guid);
    ///          Console.WriteLine("The loan amount for loan " + loan.Guid + " is: " + loan.Fields["1109"].FormattedValue);
    ///          loan.Close();
    ///       }
    ///    }
    /// 
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public event LoanMonitorEventHandler LoanMonitor
    {
      add
      {
        if (value == null)
          return;
        this.addHandler<LoanMonitorEventArgs>("LoanMonitor", new ScopedEventHandler<LoanMonitorEventArgs>.EventHandlerT(value.Invoke));
        this.subscribeTo(typeof (LoanEvent));
      }
      remove
      {
        if (value == null || this.removeHandler<LoanMonitorEventArgs>(new ScopedEventHandler<LoanMonitorEventArgs>.EventHandlerT(value.Invoke)) != 0)
          return;
        this.unsubscribeTo(typeof (LoanEvent));
      }
    }

    /// <summary>
    /// Provides notification of session-related events on the Encompass Server.
    /// </summary>
    /// <remarks>Session monitoring allows you to monitor user activity in the Encompass
    /// Server. Whenever a user logs in or out (or their session is otherwise terminated),
    /// a session event occurs.</remarks>
    /// <example>
    /// The following code opens a session to the Encompass Server and starts monitoring
    /// for session-related events. Any events detected are display on the console.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// 
    /// class ServerMonitor
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Start monitoring session-related event
    ///       session.ServerEvents.SessionMonitor += new SessionMonitorEventHandler(ServerEvents_SessionMonitor);
    /// 
    ///       // Suspend indefinitely
    ///       Console.ReadLine();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// 
    ///    private static void ServerEvents_SessionMonitor(object sender, SessionMonitorEventArgs e)
    ///    {
    ///       Console.WriteLine("Session " + e.SessionInformation.SessionID +
    ///          " (" + e.SessionInformation.ClientHostname + "/" +
    ///          e.SessionInformation.ClientIPAddress + "): " + e.EventType +
    ///          " for user " + e.SessionInformation.UserID);
    ///    }
    /// 
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public event SessionMonitorEventHandler SessionMonitor
    {
      add
      {
        if (value == null)
          return;
        this.addHandler<SessionMonitorEventArgs>("SessionMonitor", new ScopedEventHandler<SessionMonitorEventArgs>.EventHandlerT(value.Invoke));
        this.subscribeTo(typeof (SessionEvent));
      }
      remove
      {
        if (value == null || this.removeHandler<SessionMonitorEventArgs>(new ScopedEventHandler<SessionMonitorEventArgs>.EventHandlerT(value.Invoke)) != 0)
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
        scopedEventHandler?.Invoke((object) this, eventArgs);
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
        int count;
        eventHandler.Remove(handler, out count);
        return count;
      }
    }
  }
}

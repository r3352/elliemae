// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.EncompassApplication
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Diagnostics;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.Encompass.Automation
{
  /// <summary>Represents the static Encompass Application object.</summary>
  /// <remarks>The EncompassApplication object is the entry point into the Encompass Automation object model.
  /// Use the this class's static methods and events to modify the behavior of the Encompass
  /// User Interface.</remarks>
  public sealed class EncompassApplication
  {
    private static ScopedEventHandler<EventArgs> login = new ScopedEventHandler<EventArgs>(nameof (EncompassApplication), "Login");
    private static ScopedEventHandler<EventArgs> logout = new ScopedEventHandler<EventArgs>(nameof (EncompassApplication), "Logout");
    private static ScopedEventHandler<EventArgs> loanOpened = new ScopedEventHandler<EventArgs>(nameof (EncompassApplication), "LoanOpened");
    private static ScopedEventHandler<EventArgs> loanClosing = new ScopedEventHandler<EventArgs>(nameof (EncompassApplication), "LoanClosing");
    private static ScopedEventHandler<WriteLogEventArgs> writeLog = new ScopedEventHandler<WriteLogEventArgs>(nameof (EncompassApplication), "WriteLog", true);
    private static int countWriteLogHandlers = 0;
    private static object writeLogHandlersLock = new object();
    private static ScopedEventHandler<FilterLogEventArgs> filterLog = new ScopedEventHandler<FilterLogEventArgs>(nameof (EncompassApplication), "FilterLog", true);
    private static int countFilterLogHandlers = 0;
    private static object filterLogHandlersLock = new object();
    private const string className = "EncompassApplication";
    private static string sw = Tracing.SwInputEngine;
    private static bool isAttached = false;
    private static bool isStarted = false;
    private static EPass epass = (EPass) null;
    private static LoanServices loanServices;
    private static ApplicationScreens screens = (ApplicationScreens) null;
    private static EllieMae.Encompass.Client.Session currentSession = (EllieMae.Encompass.Client.Session) null;
    private static Loan currentLoan = (Loan) null;
    private static User currentUser = (User) null;

    /// <summary>
    /// Event that is raised when the application has completed the login process and is ready
    /// for user interaction.
    /// </summary>
    public static event EventHandler Login
    {
      add
      {
        if (value == null)
          return;
        EncompassApplication.login.Add(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        EncompassApplication.login.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>
    /// Event indicating that user has logged out of the Encompass Server and the application is
    /// closing.
    /// </summary>
    /// <remarks>When this event occurs the user has already been disconnected from the Encompass
    /// server, so any attempt to access remote data will fail.</remarks>
    public static event EventHandler Logout
    {
      add
      {
        if (value == null)
          return;
        EncompassApplication.logout.Add(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        EncompassApplication.logout.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>
    /// Event that occurs when the user opens a loan in the Loan Editor.
    /// </summary>
    public static event EventHandler LoanOpened
    {
      add
      {
        if (value == null)
          return;
        EncompassApplication.loanOpened.Add(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        EncompassApplication.loanOpened.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>
    /// Event that occurs when a user is closing a loan that was previously opened.
    /// </summary>
    /// <remarks>This event occurs prior to the loan actually being closed, allowing code to
    /// make changes to the contents of the loan prior to its being closed.</remarks>
    public static event EventHandler LoanClosing
    {
      add
      {
        if (value == null)
          return;
        EncompassApplication.loanClosing.Add(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        EncompassApplication.loanClosing.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>
    /// Event handler to process logs that are allowed by filters to be written.
    /// </summary>
    public static event WriteLogEventHandler WriteLog
    {
      add
      {
        lock (EncompassApplication.writeLogHandlersLock)
        {
          EncompassApplication.writeLog.Add(new ScopedEventHandler<WriteLogEventArgs>.EventHandlerT(value.Invoke));
          ++EncompassApplication.countWriteLogHandlers;
          if (EncompassApplication.countWriteLogHandlers != 1)
            return;
          ClassicLogTarget.WriteLog += new ClassicLogTarget.WriteLogHandler(EncompassApplication.onWriteLog);
        }
      }
      remove
      {
        lock (EncompassApplication.writeLogHandlersLock)
        {
          EncompassApplication.writeLog.Remove(new ScopedEventHandler<WriteLogEventArgs>.EventHandlerT(value.Invoke));
          --EncompassApplication.countWriteLogHandlers;
          if (EncompassApplication.countWriteLogHandlers != 0)
            return;
          ClassicLogTarget.WriteLog -= new ClassicLogTarget.WriteLogHandler(EncompassApplication.onWriteLog);
        }
      }
    }

    /// <summary>Event handler to filter logs before they are written.</summary>
    public static event FilterLogEventHandler FilterLog
    {
      add
      {
        lock (EncompassApplication.filterLogHandlersLock)
        {
          EncompassApplication.filterLog.Add(new ScopedEventHandler<FilterLogEventArgs>.EventHandlerT(value.Invoke));
          ++EncompassApplication.countFilterLogHandlers;
          if (EncompassApplication.countFilterLogHandlers != 1)
            return;
          CustomLogFilter.IsActiveForLog += new CustomLogFilter.IsActiveForLogHandler(EncompassApplication.onFilterLog);
        }
      }
      remove
      {
        lock (EncompassApplication.filterLogHandlersLock)
        {
          EncompassApplication.filterLog.Remove(new ScopedEventHandler<FilterLogEventArgs>.EventHandlerT(value.Invoke));
          --EncompassApplication.countFilterLogHandlers;
          if (EncompassApplication.countFilterLogHandlers != 0)
            return;
          CustomLogFilter.IsActiveForLog -= new CustomLogFilter.IsActiveForLogHandler(EncompassApplication.onFilterLog);
        }
      }
    }

    private EncompassApplication()
    {
    }

    /// <summary>
    /// This method is meant for use by the Encompass Application Framework only.
    /// </summary>
    public static void Attach()
    {
      if (EncompassApplication.isAttached)
        return;
      EllieMae.EMLite.RemotingServices.Session.Started += new EventHandler(EncompassApplication.onSessionStarted);
      EllieMae.EMLite.RemotingServices.Session.LoanOpened += new EventHandler(EncompassApplication.onLoanOpened);
      EllieMae.EMLite.RemotingServices.Session.LoanClosing += new EventHandler(EncompassApplication.onLoanClosing);
      EllieMae.EMLite.RemotingServices.Session.Ended += new EventHandler(EncompassApplication.onSessionEnded);
      EllieMae.EMLite.RemotingServices.Session.ApplicationReady += new EventHandler(EncompassApplication.onApplicationReady);
      EncompassApplication.epass = new EPass();
      EncompassApplication.loanServices = new LoanServices();
      EncompassApplication.screens = new ApplicationScreens();
      EncompassApplication.isAttached = true;
    }

    /// <summary>
    /// Gets the command-line arguments passed to this application.
    /// </summary>
    public static string[] CommandLineArguments
    {
      get
      {
        EncompassApplication.ensureStarted();
        return Environment.GetCommandLineArgs();
      }
    }

    /// <summary>
    /// Gets or sets the currently visible screen in Encompass.
    /// </summary>
    public static ApplicationScreens Screens
    {
      get
      {
        EncompassApplication.ensureStarted();
        return EncompassApplication.screens;
      }
    }

    /// <summary>
    /// Gets the Session object for the current login session.
    /// </summary>
    public static EllieMae.Encompass.Client.Session Session
    {
      get
      {
        EncompassApplication.ensureStarted();
        return EncompassApplication.currentSession;
      }
    }

    /// <summary>
    /// Gets the loan which is currently open in the loan editor.
    /// </summary>
    public static Loan CurrentLoan
    {
      get
      {
        EncompassApplication.ensureStarted();
        return EncompassApplication.currentLoan;
      }
    }

    /// <summary>Gets the currently logged in user.</summary>
    public static User CurrentUser
    {
      get
      {
        EncompassApplication.ensureStarted();
        return EncompassApplication.currentUser;
      }
    }

    /// <summary>
    /// Gets the ePASS functionality exposed by the application.
    /// </summary>
    public static EPass EPass
    {
      get
      {
        EncompassApplication.ensureStarted();
        return EncompassApplication.epass;
      }
    }

    /// <summary>
    /// Gets the LoanServices functionality exposed by the application.
    /// </summary>
    public static LoanServices LoanServices
    {
      get
      {
        EncompassApplication.ensureStarted();
        return EncompassApplication.loanServices;
      }
    }

    internal static bool Started => EncompassApplication.isStarted;

    private static void ensureStarted()
    {
      if (!EncompassApplication.isStarted)
        throw new InvalidOperationException("This object cannot be used outside the context of the Encompass Application");
    }

    private static void onLoanOpened(object sender, EventArgs e)
    {
      using (Tracing.StartTimer(EncompassApplication.sw, nameof (EncompassApplication), TraceLevel.Verbose, "EncompassApplication.LoanOpened event"))
      {
        EncompassApplication.currentLoan = Loan.Wrap(EncompassApplication.currentSession, EllieMae.EMLite.RemotingServices.Session.LoanDataMgr);
        if (EncompassApplication.loanOpened.IsNull())
          return;
        Tracing.Log(EncompassApplication.sw, nameof (EncompassApplication), TraceLevel.Info, "EncompassApplication.LoanOpened event Type Passed: " + EncompassApplication.loanOpened.Method.DeclaringType.ToString());
        Tracing.Log(EncompassApplication.sw, nameof (EncompassApplication), TraceLevel.Info, "EncompassApplication.LoanOpened signature of the delegate : " + EncompassApplication.loanOpened.Method.ToString());
        EncompassApplication.loanOpened.Invoke((object) typeof (EncompassApplication), EventArgs.Empty);
      }
    }

    private static void onLoanClosing(object sender, EventArgs e)
    {
      using (Tracing.StartTimer(EncompassApplication.sw, nameof (EncompassApplication), TraceLevel.Verbose, "EncompassApplication.LoanClosing event"))
      {
        if (!EncompassApplication.loanClosing.IsNull())
        {
          Tracing.Log(EncompassApplication.sw, nameof (EncompassApplication), TraceLevel.Info, "EncompassApplication.LoanClosing event Type Passed: " + EncompassApplication.loanClosing.Method.DeclaringType.ToString());
          Tracing.Log(EncompassApplication.sw, nameof (EncompassApplication), TraceLevel.Info, "EncompassApplication.LoanClosing signature of the delegate : " + EncompassApplication.loanClosing.Method.ToString());
          EncompassApplication.loanClosing.Invoke((object) typeof (EncompassApplication), EventArgs.Empty);
        }
        EncompassApplication.currentLoan = (Loan) null;
      }
    }

    private static void onSessionStarted(object sender, EventArgs e)
    {
      EncompassApplication.currentSession = EllieMae.Encompass.Client.Session.Wrap(EllieMae.EMLite.RemotingServices.Session.Connection, EllieMae.EMLite.RemotingServices.Session.StartupInfo, EllieMae.EMLite.RemotingServices.Session.Password, EllieMae.EMLite.RemotingServices.Session.RemoteServer);
      EncompassApplication.currentUser = User.Wrap(EncompassApplication.currentSession, EllieMae.EMLite.RemotingServices.Session.UserInfo);
      EncompassApplication.isStarted = true;
    }

    private static void onSessionEnded(object sender, EventArgs e)
    {
      try
      {
        using (Tracing.StartTimer(EncompassApplication.sw, nameof (EncompassApplication), TraceLevel.Verbose, "EncompassApplication.Logout event"))
        {
          if (EncompassApplication.logout.IsNull())
            return;
          Tracing.Log(EncompassApplication.sw, nameof (EncompassApplication), TraceLevel.Info, "EncompassApplication.Logout event Type Passed: " + EncompassApplication.logout.Method.DeclaringType.ToString());
          Tracing.Log(EncompassApplication.sw, nameof (EncompassApplication), TraceLevel.Info, "EncompassApplication.Logout signature of the delegate : " + EncompassApplication.logout.Method.ToString());
          EncompassApplication.logout.Invoke((object) typeof (EncompassApplication), EventArgs.Empty);
        }
      }
      catch (Exception ex)
      {
        throw new AutomationException("An external plugin has caused an exception in the Application.Logout event.", ex);
      }
    }

    private static void onApplicationReady(object sender, EventArgs e)
    {
      try
      {
        using (Tracing.StartTimer(EncompassApplication.sw, nameof (EncompassApplication), TraceLevel.Verbose, "EncompassApplication.Login event"))
        {
          if (EncompassApplication.login.IsNull())
            return;
          Tracing.Log(EncompassApplication.sw, nameof (EncompassApplication), TraceLevel.Info, "EncompassApplication.Login event Type Passed: " + EncompassApplication.login.Method.DeclaringType.ToString());
          Tracing.Log(EncompassApplication.sw, nameof (EncompassApplication), TraceLevel.Info, "EncompassApplication.Login signature of the delegate : " + EncompassApplication.login.Method.ToString());
          EncompassApplication.login.Invoke((object) typeof (EncompassApplication), EventArgs.Empty);
        }
      }
      catch (Exception ex)
      {
        throw new AutomationException("An external plugin has caused an exception in the Application.Login event.", ex);
      }
    }

    private static bool onWriteLog(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      WriteLogEventArgs e = new WriteLogEventArgs(log);
      if (EncompassApplication.countWriteLogHandlers > 0)
        EncompassApplication.writeLog.Invoke((object) typeof (EncompassApplication), e);
      return !e.Cancel;
    }

    private static bool onFilterLog(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      FilterLogEventArgs e = new FilterLogEventArgs(log);
      if (EncompassApplication.countFilterLogHandlers > 0)
        EncompassApplication.filterLog.Invoke((object) typeof (EncompassApplication), e);
      return e.IsActive;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.EncompassApplication
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Diagnostics;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.Encompass.Automation
{
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
    private static Session currentSession = (Session) null;
    private static Loan currentLoan = (Loan) null;
    private static User currentUser = (User) null;

    public static event EventHandler Login
    {
      add
      {
        if (value == null)
          return;
        EncompassApplication.login.Add(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        EncompassApplication.login.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public static event EventHandler Logout
    {
      add
      {
        if (value == null)
          return;
        EncompassApplication.logout.Add(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        EncompassApplication.logout.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public static event EventHandler LoanOpened
    {
      add
      {
        if (value == null)
          return;
        EncompassApplication.loanOpened.Add(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        EncompassApplication.loanOpened.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public static event EventHandler LoanClosing
    {
      add
      {
        if (value == null)
          return;
        EncompassApplication.loanClosing.Add(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        EncompassApplication.loanClosing.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public static event WriteLogEventHandler WriteLog
    {
      add
      {
        lock (EncompassApplication.writeLogHandlersLock)
        {
          EncompassApplication.writeLog.Add(new ScopedEventHandler<WriteLogEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
          ++EncompassApplication.countWriteLogHandlers;
          if (EncompassApplication.countWriteLogHandlers != 1)
            return;
          ClassicLogTarget.WriteLog += new ClassicLogTarget.WriteLogHandler((object) null, __methodptr(onWriteLog));
        }
      }
      remove
      {
        lock (EncompassApplication.writeLogHandlersLock)
        {
          EncompassApplication.writeLog.Remove(new ScopedEventHandler<WriteLogEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
          --EncompassApplication.countWriteLogHandlers;
          if (EncompassApplication.countWriteLogHandlers != 0)
            return;
          ClassicLogTarget.WriteLog -= new ClassicLogTarget.WriteLogHandler((object) null, __methodptr(onWriteLog));
        }
      }
    }

    public static event FilterLogEventHandler FilterLog
    {
      add
      {
        lock (EncompassApplication.filterLogHandlersLock)
        {
          EncompassApplication.filterLog.Add(new ScopedEventHandler<FilterLogEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
          ++EncompassApplication.countFilterLogHandlers;
          if (EncompassApplication.countFilterLogHandlers != 1)
            return;
          CustomLogFilter.IsActiveForLog += new CustomLogFilter.IsActiveForLogHandler((object) null, __methodptr(onFilterLog));
        }
      }
      remove
      {
        lock (EncompassApplication.filterLogHandlersLock)
        {
          EncompassApplication.filterLog.Remove(new ScopedEventHandler<FilterLogEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
          --EncompassApplication.countFilterLogHandlers;
          if (EncompassApplication.countFilterLogHandlers != 0)
            return;
          CustomLogFilter.IsActiveForLog -= new CustomLogFilter.IsActiveForLogHandler((object) null, __methodptr(onFilterLog));
        }
      }
    }

    private EncompassApplication()
    {
    }

    public static void Attach()
    {
      if (EncompassApplication.isAttached)
        return;
      Session.Started += new EventHandler(EncompassApplication.onSessionStarted);
      Session.LoanOpened += new EventHandler(EncompassApplication.onLoanOpened);
      Session.LoanClosing += new EventHandler(EncompassApplication.onLoanClosing);
      Session.Ended += new EventHandler(EncompassApplication.onSessionEnded);
      Session.ApplicationReady += new EventHandler(EncompassApplication.onApplicationReady);
      EncompassApplication.epass = new EPass();
      EncompassApplication.loanServices = new LoanServices();
      EncompassApplication.screens = new ApplicationScreens();
      EncompassApplication.isAttached = true;
    }

    public static string[] CommandLineArguments
    {
      get
      {
        EncompassApplication.ensureStarted();
        return Environment.GetCommandLineArgs();
      }
    }

    public static ApplicationScreens Screens
    {
      get
      {
        EncompassApplication.ensureStarted();
        return EncompassApplication.screens;
      }
    }

    public static Session Session
    {
      get
      {
        EncompassApplication.ensureStarted();
        return EncompassApplication.currentSession;
      }
    }

    public static Loan CurrentLoan
    {
      get
      {
        EncompassApplication.ensureStarted();
        return EncompassApplication.currentLoan;
      }
    }

    public static User CurrentUser
    {
      get
      {
        EncompassApplication.ensureStarted();
        return EncompassApplication.currentUser;
      }
    }

    public static EPass EPass
    {
      get
      {
        EncompassApplication.ensureStarted();
        return EncompassApplication.epass;
      }
    }

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
        EncompassApplication.currentLoan = Loan.Wrap(EncompassApplication.currentSession, Session.LoanDataMgr);
        if (EncompassApplication.loanOpened.IsNull())
          return;
        Tracing.Log(EncompassApplication.sw, nameof (EncompassApplication), TraceLevel.Info, "EncompassApplication.LoanOpened event Type Passed: " + EncompassApplication.loanOpened.Method.DeclaringType.ToString());
        Tracing.Log(EncompassApplication.sw, nameof (EncompassApplication), TraceLevel.Info, "EncompassApplication.LoanOpened signature of the delegate : " + EncompassApplication.loanOpened.Method.ToString());
        EncompassApplication.loanOpened((object) typeof (EncompassApplication), EventArgs.Empty);
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
          EncompassApplication.loanClosing((object) typeof (EncompassApplication), EventArgs.Empty);
        }
        EncompassApplication.currentLoan = (Loan) null;
      }
    }

    private static void onSessionStarted(object sender, EventArgs e)
    {
      EncompassApplication.currentSession = Session.Wrap(Session.Connection, Session.StartupInfo, Session.Password, Session.RemoteServer);
      EncompassApplication.currentUser = User.Wrap(EncompassApplication.currentSession, Session.UserInfo);
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
          EncompassApplication.logout((object) typeof (EncompassApplication), EventArgs.Empty);
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
          EncompassApplication.login((object) typeof (EncompassApplication), EventArgs.Empty);
        }
      }
      catch (Exception ex)
      {
        throw new AutomationException("An external plugin has caused an exception in the Application.Login event.", ex);
      }
    }

    private static bool onWriteLog(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      WriteLogEventArgs writeLogEventArgs = new WriteLogEventArgs(log);
      if (EncompassApplication.countWriteLogHandlers > 0)
        EncompassApplication.writeLog((object) typeof (EncompassApplication), writeLogEventArgs);
      return !writeLogEventArgs.Cancel;
    }

    private static bool onFilterLog(Encompass.Diagnostics.Logging.Schema.Log log)
    {
      FilterLogEventArgs filterLogEventArgs = new FilterLogEventArgs(log);
      if (EncompassApplication.countFilterLogHandlers > 0)
        EncompassApplication.filterLog((object) typeof (EncompassApplication), filterLogEventArgs);
      return filterLogEventArgs.IsActive;
    }
  }
}

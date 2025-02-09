// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.ServerGlobals
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class ServerGlobals
  {
    public static ITraceLog TraceLog = (ITraceLog) null;
    public static IErr Err = (IErr) null;
    [ThreadStatic]
    public static string CurrentClientID = (string) null;
    public static Dictionary<string, IContextTraceLog> ERDBTraceLogs = new Dictionary<string, IContextTraceLog>();
    protected static bool debugEnabled = EnConfigurationSettings.GlobalSettings.Debug;
    protected static TimeSpan sqlTimeout = EnConfigurationSettings.GlobalSettings.SQLTimeout;
    protected static TimeSpan reportSqlTimeout = TimeSpan.FromSeconds(30.0);
    protected static TimeSpan sqlLatency = EnConfigurationSettings.GlobalSettings.SQLLatency;
    protected static bool sqlTraceEnabled = EnConfigurationSettings.GlobalSettings.SQLTrace;
    protected static bool pgRLSEnabled = EnConfigurationSettings.GlobalSettings.PGRLSEnabled;
    protected static bool sqlTrace_IncludeAccessorNameEnabled = EnConfigurationSettings.GlobalSettings.SQLTrace_IncludeAccessorName;

    public static IContextTraceLog CurrentContextTraceLog
    {
      get
      {
        return ServerGlobals.CurrentClientID == null ? (IContextTraceLog) null : ServerGlobals.ERDBTraceLogs[ServerGlobals.CurrentClientID];
      }
    }

    protected ServerGlobals()
    {
    }

    static ServerGlobals()
    {
      EnConfigurationSettings.GlobalSettings.SettingsChanged += new EventHandler(ServerGlobals.onGlobalSettingsChanged);
      EnConfigurationSettings.GlobalSettings.EnableMonitoring();
      ServerGlobals.onGlobalSettingsChanged((object) null, (EventArgs) null);
    }

    public static int ErrorCacheSize => 10;

    public static TimeSpan SQLTimeout => ServerGlobals.sqlTimeout;

    public static TimeSpan SQLLatency => ServerGlobals.sqlLatency;

    public static bool SQLTrace => ServerGlobals.sqlTraceEnabled;

    public static bool PGRLSEnabled => ServerGlobals.pgRLSEnabled;

    public static TimeSpan ReportSQLTimeout => ServerGlobals.reportSqlTimeout;

    public static bool Debug => ServerGlobals.debugEnabled;

    public static bool SQLTrace_IncludeAccessorName
    {
      get => ServerGlobals.sqlTrace_IncludeAccessorNameEnabled;
    }

    private static void onGlobalSettingsChanged(object sender, EventArgs e)
    {
      ServerGlobals.debugEnabled = EnConfigurationSettings.GlobalSettings.Debug;
      ServerGlobals.sqlTimeout = EnConfigurationSettings.GlobalSettings.SQLTimeout;
      ServerGlobals.sqlTraceEnabled = EnConfigurationSettings.GlobalSettings.SQLTrace;
      ServerGlobals.reportSqlTimeout = EnConfigurationSettings.GlobalSettings.ReportSQLTimeout;
      ServerGlobals.sqlLatency = EnConfigurationSettings.GlobalSettings.SQLLatency;
      ServerGlobals.sqlTrace_IncludeAccessorNameEnabled = EnConfigurationSettings.GlobalSettings.SQLTrace_IncludeAccessorName;
    }
  }
}

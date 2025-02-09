// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.ApplicationLog
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.RemotingServices;
using Encompass.Diagnostics.Config;
using System;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>Provides access to the Application log file</summary>
  /// <remarks>The ApplicationLog stores debug information written by the Encompass runtime components.
  /// Using the proeprties and methods provided by this class, you can add values to this log so they
  /// are combined with the existing logging.</remarks>
  public static class ApplicationLog
  {
    static ApplicationLog() => ApplicationLog.Initialize();

    /// <summary>Initializes the application log file</summary>
    internal static void Initialize()
    {
      Tracing.Init(System.IO.Path.Combine(SystemSettings.LogDir, Environment.UserName + "\\Session"));
    }

    /// <summary>
    /// Gets the path of the Session.log file for the application
    /// </summary>
    public static string Path => Tracing.LogFile;

    /// <summary>
    /// Determines if the application is in verbose/debug mode
    /// </summary>
    public static bool DebugEnabled
    {
      get => Tracing.Debug;
      set
      {
        Tracing.Debug = value;
        DiagConfig<ClientDiagConfigData>.Instance.ReloadConfig();
      }
    }

    /// <summary>Writes a message into the application log</summary>
    /// <param name="source">The source of the message, e.g. the class name of the writer</param>
    /// <param name="message">The message to be written.</param>
    public static void Write(string source, string message)
    {
      Tracing.Log(true, "INFO", source, message);
    }

    /// <summary>Writes an error message into the application log</summary>
    /// <param name="source">The source of the message, e.g. the class name of the writer</param>
    /// <param name="message">The message to be written.</param>
    public static void WriteError(string source, string message)
    {
      Tracing.Log(true, "ERROR", source, message);
    }

    /// <summary>
    /// Writes a message into the application log, but only if the DebugEnabled property is <c>true</c>.
    /// </summary>
    /// <param name="source">The source of the message, e.g. the class name of the writer</param>
    /// <param name="message">The message to be written.</param>
    public static void WriteDebug(string source, string message)
    {
      Tracing.Log(false, "DEBUG", source, message);
    }
  }
}

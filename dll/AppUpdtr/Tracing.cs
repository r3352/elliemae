// Decompiled with JetBrains decompiler
// Type: AppUpdtr.Tracing
// Assembly: AppUpdtr, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3ADBA3EC-518B-4BBF-94A2-A2027DADA3FC
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\AppUpdtr.exe

using System;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace AppUpdtr
{
  public class Tracing
  {
    private const string className = "Tracing";
    private static bool initialized;
    private static string logFile;

    public static void Init(string logFileBasePath) => Tracing.Init(logFileBasePath, false);

    public static void Init(string logFileBasePath, bool server)
    {
      if (Tracing.initialized)
        return;
      Tracing.initialized = true;
      Directory.CreateDirectory(Path.GetDirectoryName(logFileBasePath));
      int num = 0;
label_2:
      Tracing.logFile = (string) null;
      FileStream fileStream;
      try
      {
        string str = num == 0 ? string.Empty : num.ToString();
        Tracing.logFile = logFileBasePath + str + ".log";
        fileStream = !server ? new FileStream(Tracing.logFile, FileMode.Create, FileAccess.Write, FileShare.Read) : new FileStream(Tracing.logFile, FileMode.Append, FileAccess.Write, FileShare.Read);
      }
      catch (Exception ex)
      {
        if (++num >= 100)
          throw new Exception("Unable to create log file '" + logFileBasePath + "': " + ex.Message);
        goto label_2;
      }
      Trace.Listeners.Add((TraceListener) new TextWriterTraceListener((Stream) fileStream));
      string message = "Application starts at " + DateTime.Now.ToString();
      if (server)
        message = "-------------------" + Environment.NewLine + message;
      Trace.WriteLine(message);
      Trace.Flush();
    }

    public static string LogFile => Tracing.logFile;

    public static void Log(TraceSwitch sw, TraceLevel l, string className, string msg)
    {
      Tracing.Log(sw, className, l, msg);
    }

    public static void Log(TraceSwitch sw, string className, TraceLevel l, string msg)
    {
      if (sw == null)
      {
        Tracing.Log("WARNING", nameof (Tracing), "Null trace switch in " + className);
      }
      else
      {
        if (sw.Level < l)
          return;
        Tracing.Log(l.ToString().ToUpper(), className, msg);
      }
    }

    public static void Log(string levelStr, string category, string msg)
    {
      Tracing.Log(msg, Tracing.Format(levelStr, category));
    }

    public static void Log(string msg, string cat)
    {
      try
      {
        Trace.WriteLine(msg, cat);
        Trace.Flush();
      }
      catch
      {
      }
    }

    public static string Format(string levelStr, string className)
    {
      string str = "[" + DateTime.Now.ToString() + "] " + levelStr;
      if (className != "")
        str = str + " (" + className + ")";
      return str;
    }

    public static void Close()
    {
      Trace.WriteLine("Application stops at " + DateTime.Now.ToString());
      Trace.Flush();
      Trace.Close();
    }
  }
}

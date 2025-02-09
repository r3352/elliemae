// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Common.Tracing
// Assembly: Elli.CalculationEngine.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BBD0C9BB-76EB-4848-9A1B-D338F49271A1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Common.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

#nullable disable
namespace Elli.CalculationEngine.Common
{
  public static class Tracing
  {
    private static TraceSwitch traceSwitch = new TraceSwitch("CalculationEngine", "Calculation Engine");

    public static event Tracing.TraceMessageEventHandler WriteMessage;

    public static void Dispose() => Trace.Close();

    public static void Log(TraceLevel traceLevel, string className, string message)
    {
      if (Tracing.traceSwitch == null)
      {
        Tracing.Log("Null trace switch in " + className, Tracing.Format("WARNING", nameof (Tracing)));
      }
      else
      {
        if (!Tracing.IsSwitchActive(traceLevel))
          return;
        foreach (TraceListener listener in Trace.Listeners)
        {
          if (listener.Name.ToUpper() == "NLOG" || listener.GetType().ToString() == "NLog.NLogTraceListener")
          {
            listener.Filter = (TraceFilter) new SourceFilter("Nothing");
            switch (traceLevel)
            {
              case TraceLevel.Error:
                new Logger().Fatal(string.IsNullOrEmpty(className) ? message : string.Format("({0}): {1}", (object) className, (object) message));
                continue;
              case TraceLevel.Warning:
                new Logger().Warning(string.IsNullOrEmpty(className) ? message : string.Format("({0}): {1}", (object) className, (object) message));
                continue;
              case TraceLevel.Info:
                new Logger().Information(string.IsNullOrEmpty(className) ? message : string.Format("({0}): {1}", (object) className, (object) message));
                continue;
              default:
                new Logger().Debug(string.IsNullOrEmpty(className) ? message : string.Format("({0}): {1}", (object) className, (object) message));
                continue;
            }
          }
        }
        Tracing.Log(message, Tracing.Format(traceLevel.ToString().ToUpper(), className));
      }
    }

    public static string GetStackTrace() => Tracing.GetStackTrace(-1);

    public static string GetStackTrace(int numFrames)
    {
      List<string> stringList = new List<string>();
      StackTrace stackTrace = new StackTrace();
      int num = 0;
      foreach (StackFrame frame in stackTrace.GetFrames())
      {
        frame.GetMethod();
        stringList.Add("  at " + Tracing.GetStackFrameText(frame));
        if (numFrames > 0 && ++num >= numFrames)
          break;
      }
      return string.Join(Environment.NewLine, stringList.ToArray());
    }

    private static string Format(string levelStr, string className)
    {
      return "[" + DateTime.Now.ToString("MM/dd/yy H:mm:ss.ffff tt") + "] " + levelStr + (className != "" ? " (" + className + ")" : string.Empty);
    }

    private static string GetStackFrameText(StackFrame frame)
    {
      MethodBase method = frame.GetMethod();
      string str = method.DeclaringType.FullName + "." + method.Name + "(";
      int num = 0;
      foreach (ParameterInfo parameter in method.GetParameters())
        str = str + (num++ > 0 ? ", " : "") + parameter.ParameterType.FullName;
      return str + ")";
    }

    private static bool IsSwitchActive(TraceLevel level)
    {
      return Tracing.traceSwitch.Level >= level || level == TraceLevel.Error;
    }

    private static void Log(string message, string category)
    {
      try
      {
        if (Tracing.WriteMessage != null)
        {
          TraceMessageEventArgs e = new TraceMessageEventArgs(category, message);
          Tracing.WriteMessage(e);
          if (e.Cancel)
            return;
        }
        Trace.WriteLine(message, category);
      }
      catch
      {
      }
    }

    public delegate void TraceMessageEventHandler(TraceMessageEventArgs e);
  }
}

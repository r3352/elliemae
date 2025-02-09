// Decompiled with JetBrains decompiler
// Type: Elli.Data.Orm.MultitenentTraceAppender
// Assembly: Elli.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5199CF45-D8E1-4436-8A49-245565D9CA6B
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.dll

using EllieMae.EMLite.Common;
using log4net.Appender;
using log4net.Core;
using System.Diagnostics;

#nullable disable
namespace Elli.Data.Orm
{
  internal class MultitenentTraceAppender : AppenderSkeleton
  {
    protected override void Append(LoggingEvent loggingEvent)
    {
      string msg = this.RenderLoggingEvent(loggingEvent);
      string className = "NHibernate";
      switch (loggingEvent.Level.Name)
      {
        case "DEBUG":
          Tracing.Log(Tracing.SwDataEngine, TraceLevel.Verbose, className, msg);
          break;
        case "WARN":
          Tracing.Log(Tracing.SwDataEngine, TraceLevel.Warning, className, msg);
          break;
        case "INFO":
          Tracing.Log(Tracing.SwDataEngine, TraceLevel.Info, className, msg);
          break;
        case "ERROR":
        case "FATAL":
          Tracing.Log(Tracing.SwDataEngine, TraceLevel.Error, className, msg);
          break;
      }
    }
  }
}

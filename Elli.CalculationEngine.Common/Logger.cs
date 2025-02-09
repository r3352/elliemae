// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Common.Logger
// Assembly: Elli.CalculationEngine.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BBD0C9BB-76EB-4848-9A1B-D338F49271A1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Common.dll

using NLog;
using System;

#nullable disable
namespace Elli.CalculationEngine.Common
{
  public class Logger : ILogger
  {
    private static Logger logger = LogManager.GetCurrentClassLogger();

    public void Debug(string message) => Logger.logger.Debug(message);

    public void Debug(string message, Exception e) => Logger.logger.DebugException(message, e);

    public void Information(string message) => Logger.logger.Info(message);

    public void Information(string message, Exception e) => Logger.logger.InfoException(message, e);

    public void Fatal(string message) => Logger.logger.Error(message);

    public void Fatal(string message, Exception e) => Logger.logger.ErrorException(message, e);

    public void Warning(string message) => Logger.logger.Warn(message);

    public void Warning(string message, Exception e) => Logger.logger.WarnException(message, e);

    public void Trace(string message) => Logger.logger.Trace(message);

    public void Trace(string message, Exception e) => Logger.logger.TraceException(message, e);
  }
}

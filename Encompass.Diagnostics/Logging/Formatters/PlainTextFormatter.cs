// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Formatters.PlainTextFormatter
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using Encompass.Diagnostics.PII;
using System;
using System.IO;

#nullable disable
namespace Encompass.Diagnostics.Logging.Formatters
{
  public class PlainTextFormatter : ILogFormatter
  {
    private readonly bool _addInstanceNameToMessage;

    protected PlainTextFormatter(bool addInstanceNameToMessage)
    {
      this._addInstanceNameToMessage = addInstanceNameToMessage;
    }

    public PlainTextFormatter()
      : this(false)
    {
    }

    public string FormatLog(Log log)
    {
      string str1 = string.Empty;
      DateTime startTime = TimeZoneInfo.ConvertTimeFromUtc(log.GetLogTime(), TimeZoneInfo.Local);
      if (log.Level != 0)
        str1 = this.Format(log.GetLogLevel(), log.GetSourceName(), startTime, log.ThreadId.ToString("0"));
      else if (!string.IsNullOrEmpty(log.GetSourceName()))
        str1 = log.GetSourceName();
      string str2 = string.Empty;
      if (!string.IsNullOrEmpty(log.CorrelationId))
        str2 = "<" + log.CorrelationId + "> ";
      else if (log.TransactionId != null)
        str2 = "<" + log.TransactionId.ToString() + "> ";
      string str3 = str2 + log.GetLogMessage();
      string message = MaskUtilities.MaskPII(str1 + ": " + str3, "SQLTrace".Equals(log.Logger, StringComparison.OrdinalIgnoreCase));
      return this.AppendInstanceId(log, message);
    }

    public virtual void WriteFormatted(Log log, TextWriter textWriter)
    {
      textWriter.Write(this.FormatLog(log));
    }

    private string AppendInstanceId(Log log, string message)
    {
      if (!this._addInstanceNameToMessage)
        return message;
      string str = log.InstanceIdOrDefault();
      return str + " -> " + PlainTextFormatter.TrimEnd(message.Replace(Environment.NewLine, Environment.NewLine + str + " -> ").Replace(str + " -> " + Environment.NewLine, Environment.NewLine), str + " -> ");
    }

    private static string TrimEnd(string source, string value)
    {
      return !source.EndsWith(value) ? source : source.Remove(source.LastIndexOf(value));
    }

    private string Format(string levelStr, string className, DateTime startTime, string threadId)
    {
      string str = "[" + startTime.ToString("MM/dd/yy H:mm:ss.ffff") + "] " + levelStr + " {" + (threadId ?? string.Empty).PadLeft(3, '0') + "}";
      if (!string.IsNullOrEmpty(className))
        str = str + " (" + className + ")";
      return str;
    }

    public virtual LogFormat GetFormat()
    {
      return this._addInstanceNameToMessage ? LogFormat.PlainTextWithInstance : LogFormat.PlainText;
    }
  }
}

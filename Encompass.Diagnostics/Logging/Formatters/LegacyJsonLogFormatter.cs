// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Formatters.LegacyJsonLogFormatter
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using Encompass.Diagnostics.PII;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace Encompass.Diagnostics.Logging.Formatters
{
  public class LegacyJsonLogFormatter : ILogFormatter
  {
    private static JsonSerializerSettings Settings;
    private readonly Formatting _formatting;
    private static readonly HashSet<string> _skippedAdditionalInfoKeys = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      "@timestamp",
      "Level",
      "Src",
      "Logger",
      "AppName",
      "Environment",
      "Version",
      "Host",
      "InstanceId",
      "CustomerId",
      "UserId",
      "SessionId",
      "Pid",
      "ThreadId",
      "TransactionId",
      "CorrelationId",
      "Message",
      "Error",
      Log.CommonFields.EventType.Name,
      Log.CommonFields.JwtClaims.Name,
      Log.CommonFields.JwtIdentity.Name,
      Log.CommonFields.JwtScope.Name,
      Log.CommonFields.HttpRequestHeaders.Name,
      Log.CommonFields.ApiPathAndQuery.Name,
      Log.CommonFields.ApiPathTemplate.Name,
      Log.CommonFields.ApiRequestParameters.Name,
      Log.CommonFields.ApiErrorResponseBody.Name,
      Log.CommonFields.HttpRequestMethod.Name,
      Log.CommonFields.HttpResponseStatusCode.Name,
      Log.CommonFields.StartTime.Name,
      Log.CommonFields.EndTime.Name,
      Log.CommonFields.ActorUserId.Name,
      Log.CommonFields.SiteId.Name
    };

    static LegacyJsonLogFormatter()
    {
      LegacyJsonLogFormatter.Settings = new JsonSerializerSettings()
      {
        NullValueHandling = NullValueHandling.Ignore,
        DateFormatString = "yyyy-MM-dd HH:mm:ss.fff",
        DateTimeZoneHandling = DateTimeZoneHandling.Local
      };
    }

    public LegacyJsonLogFormatter()
      : this(Formatting.None)
    {
    }

    public LegacyJsonLogFormatter(Formatting formatting) => this._formatting = formatting;

    public void WriteFormatted(Log log, TextWriter textWriter)
    {
      MaskUtilities.SerializeObject(textWriter, (object) this.Convert(log), this._formatting, LegacyJsonLogFormatter.Settings, "SQLTrace".Equals(log.Logger, StringComparison.OrdinalIgnoreCase));
    }

    public string FormatLog(Log log)
    {
      StringBuilder sb = new StringBuilder(256);
      using (StringWriter stringWriter = new StringWriter(sb))
        this.WriteFormatted(log, (TextWriter) stringWriter);
      return sb.ToString();
    }

    public LogFormat GetFormat()
    {
      return this._formatting == Formatting.None ? LogFormat.LegacyJson : LogFormat.PrettyLegacyJson;
    }

    private LegacyLog.HttpRequestLogParams GetHttpRequest(LegacyLog log)
    {
      return log.HttpRequest ?? (log.HttpRequest = new LegacyLog.HttpRequestLogParams());
    }

    private LegacyLog.HttpResponseLogParams GetHttpResponse(LegacyLog log)
    {
      return log.HttpResponse ?? (log.HttpResponse = new LegacyLog.HttpResponseLogParams());
    }

    private LegacyLog Convert(Log log)
    {
      LegacyLog log1 = new LegacyLog();
      StreamingContext streamingContext = new StreamingContext(StreamingContextStates.Other);
      log1.LogLevel = (LegacyLogLevel) log.Level.GetBaseType();
      log1.Message = log.Message;
      log1.Node = log.Host;
      log1.SourceName = log.Src;
      log1.ThreadId = log.ThreadId.ToString();
      log1.Time = log.Timestamp;
      log1.TransactionId = log.TransactionId?.ToString();
      log1.Version = log.Version;
      log1.Exception = log.Error?.Exception;
      log1.AppName = log.AppName;
      log1.Instance = log.InstanceId;
      log1.CorrelationId = log.CorrelationId;
      Dictionary<string, object> source = new Dictionary<string, object>();
      foreach (ILogField key in log.GetKeys().Where<ILogField>((Func<ILogField, bool>) (a => !LegacyJsonLogFormatter._skippedAdditionalInfoKeys.Contains(a.Name))))
        source[key.Name] = log.GetInternal(key);
      log1.LoggerName = log.Logger;
      Dictionary<string, string> tvalue1;
      if (log.TryGet<Dictionary<string, string>>(Log.CommonFields.JwtClaims, out tvalue1))
        log1.UserClaims = tvalue1;
      string tvalue2;
      if (log.TryGet<string>(Log.CommonFields.ApiPathAndQuery, out tvalue2))
        this.GetHttpRequest(log1).PathAndQuery = tvalue2;
      string tvalue3;
      if (log.TryGet<string>(Log.CommonFields.HttpRequestMethod, out tvalue3))
        this.GetHttpRequest(log1).Method = tvalue3;
      string tvalue4;
      if (log.TryGet<string>(Log.CommonFields.ApiPathTemplate, out tvalue4))
        this.GetHttpRequest(log1).PathTemplate = tvalue4;
      else
        tvalue4 = (string) null;
      Dictionary<string, string> tvalue5;
      if (log.TryGet<Dictionary<string, string>>(Log.CommonFields.HttpRequestHeaders, out tvalue5))
      {
        this.GetHttpRequest(log1).RequestHeaders = tvalue5;
        if (tvalue5.ContainsKey("SOAPAction"))
        {
          string tvalue6;
          if (log.TryGet<string>(Log.CommonFields.ActorUserId, out tvalue6))
            source["Actor User ID"] = (object) tvalue6;
          if (!string.IsNullOrWhiteSpace(log.UserId))
            source["User ID"] = (object) log.UserId;
          if (!string.IsNullOrWhiteSpace(log.SessionId))
            source["Session ID"] = (object) log.SessionId;
          if (!string.IsNullOrEmpty(tvalue4))
            source["Class Name"] = (object) tvalue4;
        }
      }
      Dictionary<string, object> tvalue7;
      if (log.TryGet<Dictionary<string, object>>(Log.CommonFields.ApiRequestParameters, out tvalue7))
        this.GetHttpRequest(log1).RequestParameters = tvalue7;
      int tvalue8;
      if (log.TryGet<int>(Log.CommonFields.HttpResponseStatusCode, out tvalue8))
      {
        this.GetHttpResponse(log1).StatusCode = new HttpStatusCode?((HttpStatusCode) tvalue8);
        LegacyLog.HttpResponseLogParams httpResponse = this.GetHttpResponse(log1);
        LogErrorData error = log.Error;
        string str;
        if (error == null)
        {
          str = (string) null;
        }
        else
        {
          Exception exception = error.Exception;
          str = exception != null ? exception.GetFullStackTrace() : (string) null;
        }
        httpResponse.Exception = str;
        if (this.GetHttpResponse(log1).Exception != null)
          log1.Exception = (Exception) null;
        object tvalue9;
        if (log.TryGet<object>(Log.CommonFields.ApiErrorResponseBody, out tvalue9))
          this.GetHttpResponse(log1).ErrorContent = tvalue9;
        double tvalue10;
        if (log.TryGet<double>(Log.CommonFields.DurationMS, out tvalue10))
        {
          this.GetHttpResponse(log1).Duration = (long) tvalue10;
          source.Remove(Log.CommonFields.DurationMS.Name);
        }
      }
      if (source.Any<KeyValuePair<string, object>>())
        log1.AdditionalInfo = source;
      return log1;
    }
  }
}

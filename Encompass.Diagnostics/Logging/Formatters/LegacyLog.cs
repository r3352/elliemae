// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Formatters.LegacyLog
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Net;

#nullable disable
namespace Encompass.Diagnostics.Logging.Formatters
{
  [Serializable]
  public class LegacyLog
  {
    public virtual string AppName { get; set; }

    public virtual string Version { get; set; }

    public string Node { get; set; }

    public string Instance { get; set; }

    public virtual string LoggerName { get; set; }

    public virtual string TransactionId { get; set; }

    public virtual string CorrelationId { get; set; }

    public virtual string ThreadId { get; set; }

    public virtual DateTime Time { get; set; }

    [JsonConverter(typeof (StringEnumConverter))]
    [JsonProperty("LogEventType")]
    public LegacyLogLevel LogLevel { get; set; }

    public string SourceName { get; set; }

    public string Message { get; set; }

    [JsonConverter(typeof (ExceptionConverter))]
    public Exception Exception { get; set; }

    public LegacyLog.HttpRequestLogParams HttpRequest { get; set; }

    public Dictionary<string, string> UserClaims { get; set; }

    public LegacyLog.HttpResponseLogParams HttpResponse { get; set; }

    public Dictionary<string, object> AdditionalInfo { get; set; }

    public class HttpRequestLogParams
    {
      public string Method { get; set; }

      public string PathAndQuery { get; set; }

      public string PathTemplate { get; set; }

      public Dictionary<string, object> RequestParameters { get; set; }

      public Dictionary<string, string> RequestHeaders { get; set; }
    }

    public class HttpResponseLogParams
    {
      public long Duration { get; set; }

      public HttpStatusCode? StatusCode { get; set; }

      public object ErrorContent { get; set; }

      public string Exception { get; set; }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Schema.Log
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Formatters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace Encompass.Diagnostics.Logging.Schema
{
  [Serializable]
  public class Log : LogFields
  {
    public const string TimestampPropertyName = "@timestamp";
    private static HashSet<string> _predefinedFieldNames = new HashSet<string>();
    private static readonly LogFieldName<Encompass.Diagnostics.Logging.LogLevel> LevelAttr;
    private static readonly LogFieldName<string> LoggerAttr;
    private Encompass.Diagnostics.Logging.LogLevel _level;
    private string _logger;

    static Log()
    {
      foreach (PropertyInfo property in typeof (Log).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
        typeof (Log).GetMethod("GetPreDefinedField", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(property.PropertyType).Invoke((object) null, new object[1]
        {
          (object) (property.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? property.Name)
        });
      Log.LevelAttr = LogFields.Field<Encompass.Diagnostics.Logging.LogLevel>(nameof (Level));
      Log.LoggerAttr = LogFields.Field<string>(nameof (Logger));
    }

    public Log()
    {
    }

    protected Log(Log log)
      : base((LogFields) log)
    {
      this._level = log.Level;
      this._logger = log.Logger;
    }

    protected Log(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this._level = (Encompass.Diagnostics.Logging.LogLevel) info.GetValue(nameof (_level), typeof (Encompass.Diagnostics.Logging.LogLevel));
      this._logger = (string) info.GetValue(nameof (_logger), typeof (string));
    }

    [JsonIgnore]
    internal virtual bool SkipPreparationBeforeWrite => false;

    [JsonProperty("@timestamp")]
    public DateTime Timestamp
    {
      get => this.GetAttr<DateTime>("@timestamp");
      set => this.SetAttr<DateTime>("@timestamp", value);
    }

    public virtual Encompass.Diagnostics.Logging.LogLevel Level
    {
      get => this._level;
      set
      {
        this._level = value;
        this.Set<Encompass.Diagnostics.Logging.LogLevel>(Log.LevelAttr, value);
      }
    }

    public string Src
    {
      get => this.GetAttr<string>(nameof (Src));
      set => this.SetAttr<string>(nameof (Src), value);
    }

    public virtual string Logger
    {
      get => this._logger;
      set
      {
        this._logger = value;
        this.Set<string>(Log.LoggerAttr, value);
      }
    }

    public string Message
    {
      get => this.GetAttr<string>(nameof (Message));
      set => this.SetAttr<string>(nameof (Message), value);
    }

    public LogErrorData Error
    {
      get => this.GetAttr<LogErrorData>(nameof (Error));
      set => this.SetAttr<LogErrorData>(nameof (Error), value);
    }

    public string AppName
    {
      get => this.GetAttr<string>(nameof (AppName));
      set => this.SetAttr<string>(nameof (AppName), value);
    }

    public string Environment
    {
      get => this.GetAttr<string>(nameof (Environment));
      set => this.SetAttr<string>(nameof (Environment), value);
    }

    public string Version
    {
      get => this.GetAttr<string>(nameof (Version));
      set => this.SetAttr<string>(nameof (Version), value);
    }

    public string Host
    {
      get => this.GetAttr<string>(nameof (Host));
      set => this.SetAttr<string>(nameof (Host), value);
    }

    public int Pid
    {
      get => this.GetAttr<int>(nameof (Pid));
      set => this.SetAttr<int>(nameof (Pid), value);
    }

    public int ThreadId
    {
      get => this.GetAttr<int>(nameof (ThreadId));
      set => this.SetAttr<int>(nameof (ThreadId), value);
    }

    public string InstanceId
    {
      get => this.GetAttr<string>(nameof (InstanceId));
      set => this.SetAttr<string>(nameof (InstanceId), value);
    }

    public string CustomerId
    {
      get => this.GetAttr<string>(nameof (CustomerId));
      set => this.SetAttr<string>(nameof (CustomerId), value);
    }

    public string UserId
    {
      get => this.GetAttr<string>(nameof (UserId));
      set => this.SetAttr<string>(nameof (UserId), value);
    }

    public string SessionId
    {
      get => this.GetAttr<string>(nameof (SessionId));
      set => this.SetAttr<string>(nameof (SessionId), value);
    }

    public TransactionId TransactionId
    {
      get => this.GetAttr<TransactionId>(nameof (TransactionId));
      set => this.SetAttr<TransactionId>(nameof (TransactionId), value);
    }

    public string CorrelationId
    {
      get => this.GetAttr<string>(nameof (CorrelationId));
      set => this.SetAttr<string>(nameof (CorrelationId), value);
    }

    private T GetAttr<T>(string propertyName) => this.Get<T>(LogFields.Field<T>(propertyName));

    private void SetAttr<T>(string propertyName, T value)
    {
      this.Set<T>(LogFields.Field<T>(propertyName), value);
    }

    public virtual bool SupportsFormatter(LogFormat logFormat) => true;

    public virtual DateTime GetLogTime() => this.Timestamp;

    public virtual string GetLogMessage()
    {
      StringBuilder stringBuilder = new StringBuilder(this.Message);
      if (this.Error != null && this.Error.Exception != null)
      {
        if (!string.IsNullOrEmpty(this.Message))
          stringBuilder.Append(System.Environment.NewLine);
        stringBuilder.Append("Exception: ").Append(this.Error.Exception.GetFullStackTrace());
      }
      return stringBuilder.ToString();
    }

    public virtual string GetLogLevel()
    {
      switch (this.Level.GetBaseType())
      {
        case Encompass.Diagnostics.Logging.LogLevel.None:
          return "ERROR";
        case Encompass.Diagnostics.Logging.LogLevel.ERROR:
          return "ERROR";
        case Encompass.Diagnostics.Logging.LogLevel.WARN:
          return "WARNING";
        case Encompass.Diagnostics.Logging.LogLevel.INFO:
          return "INFO";
        case Encompass.Diagnostics.Logging.LogLevel.DEBUG:
          return "VERBOSE";
        default:
          return "ERROR";
      }
    }

    public virtual string GetSourceName() => this.Src;

    public string InstanceIdOrDefault()
    {
      return !string.IsNullOrWhiteSpace(this.InstanceId) ? this.InstanceId : "DEFAULT";
    }

    public Dictionary<string, object> GetAdditionalFields()
    {
      return this.GetKeys().Where<ILogField>((Func<ILogField, bool>) (f => !Log._predefinedFieldNames.Contains(f.Name))).ToDictionary<ILogField, string, object>((Func<ILogField, string>) (f => f.Name), (Func<ILogField, object>) (f => this.GetInternal(f)));
    }

    private static void GetPreDefinedField<T>(string name)
    {
      Log._predefinedFieldNames.Add(LogFields.Field<T>(name).Name);
    }

    public static bool IsPreDefinedField(string name) => Log._predefinedFieldNames.Contains(name);

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("_level", (object) this._level);
      info.AddValue("_logger", (object) this._logger);
      base.GetObjectData(info, context);
    }

    public static class CommonFields
    {
      public static readonly LogFieldName<LogEventType> EventType = LogFields.Field<LogEventType>("eventType");
      public static readonly LogFieldName<DateTime> StartTime = LogFields.Field<DateTime>("start");
      public static readonly LogFieldName<DateTime> EndTime = LogFields.Field<DateTime>("end");
      public static readonly LogFieldName<double> DurationMS = LogFields.Field<double>("durationMS");
      public static readonly LogFieldName<string> Sql = LogFields.Field<string>("sql");
      public static readonly LogFieldName<string> StackTrace = LogFields.Field<string>("stackTrace");
      public static readonly LogFieldName<string> LoanId = LogFields.Field<string>("loanId");
      public static readonly LogFieldName<string> LoanFolder = LogFields.Field<string>("loanFolder");
      public static readonly LogFieldName<string> LoanFilePath = LogFields.Field<string>("loanFilePath");
      public static readonly LogFieldName<string> ApiPathAndQuery = LogFields.Field<string>("apiPathAndQuery");
      public static readonly LogFieldName<string> ApiPathTemplate = LogFields.Field<string>("apiPathTemplate");
      public static readonly LogFieldName<Dictionary<string, object>> ApiRequestParameters = LogFields.Field<Dictionary<string, object>>("apiRequestParameters");
      public static readonly LogFieldName<string> HttpRequestMethod = LogFields.Field<string>("httpRequestMethod");
      public static readonly LogFieldName<Dictionary<string, string>> HttpRequestHeaders = LogFields.Field<Dictionary<string, string>>("httpRequestHeaders");
      public static readonly HashSet<string> ExcludedHttpRequestHeaders = new HashSet<string>()
      {
        "Referer",
        "Elli-Session",
        "Authorization",
        "Accept-Charset"
      };
      public static readonly LogFieldName<int> HttpResponseStatusCode = LogFields.Field<int>("httpResponseStatusCode");
      public static readonly LogFieldName<object> ApiErrorResponseBody = LogFields.Field<object>("apiErrorResponseBody");
      public static readonly LogFieldName<Dictionary<string, string>> JwtClaims = LogFields.Field<Dictionary<string, string>>("jwtClaims");
      public static readonly LogFieldName<string> JwtIdentity = LogFields.Field<string>("jwtIdentity");
      public static readonly LogFieldName<string> JwtScope = LogFields.Field<string>("jwtScope");
      public static readonly LogFieldName<string> CallerCategory = LogFields.Field<string>("encompassCallerCategory");
      public static readonly LogFieldName<string> CallerModuleName = LogFields.Field<string>("encompassCallerModuleName");
      public static readonly LogFieldName<string> CallerAssembly = LogFields.Field<string>("encompassCallerAssembly");
      public static readonly LogFieldName<string> CallerEvent = LogFields.Field<string>("encompassApplicationEvent");
      public static readonly LogFieldName<string> CallerAppName = LogFields.Field<string>("encompassCallerAppName");
      public static readonly LogFieldName<string> ClientIpAddress = LogFields.Field<string>("encompassClientIpAddress");
      public static readonly LogFieldName<string> ActorUserId = LogFields.Field<string>("actorId");
      public static readonly LogFieldName<string> SiteId = LogFields.Field<string>("siteId");
      public static readonly LogFieldName<string> CacheKey = LogFields.Field<string>("cacheKey");
      public static readonly LogFieldName<string> CacheAction = LogFields.Field<string>("cacheAction");
      public static readonly LogFieldName<long> InitVirtualFieldsMS = LogFields.Field<long>("initVirtualFieldsMS");
      public static readonly LogFieldName<long> MapLoanDataMS = LogFields.Field<long>("mapLoanDataMS");
      public static readonly LogFieldName<long> ReinitVirtualFieldsMS = LogFields.Field<long>("reinitVirtualFieldsMS");
      public static readonly LogFieldName<long> FieldChangesIdentifiedTotalMS = LogFields.Field<long>("fieldChangesIdentifiedTotalMS");

      internal static void Init()
      {
      }
    }
  }
}

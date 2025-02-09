// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServiceObjects.KafkaEvent.KafkaEventLog
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Encompass.Diagnostics.Logging.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.Server.ServiceObjects.KafkaEvent
{
  public class KafkaEventLog : Log
  {
    private readonly bool _success;
    private const string PublishSucessLogMessage = "Publishing {0} message is successful for eventType: {1}, instanceId: {2}, loanId: {3}, correlationId: {4}, loanPath:{5}, publishedTime: {6}�";
    private const string PublishFailedLogMessage = "Publishing message failed for type: {0} loanId: {1}, CorrelationId: {2}; Failure reason: {3}; instanceId: {4}; Stack Trace: {5}. Please check the kafka event log files to view failed messages.�";

    public KafkaEventLog(bool success) => this._success = success;

    protected KafkaEventLog(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this._success = info.GetBoolean(nameof (_success));
    }

    public override string GetLogMessage()
    {
      string logMessage;
      if (this._success)
      {
        logMessage = string.Format("Publishing {0} message is successful for eventType: {1}, instanceId: {2}, loanId: {3}, correlationId: {4}, loanPath:{5}, publishedTime: {6}", (object) this.Get<string>(KafkaEventLog.Fields.EventType), (object) this.Get<string>(KafkaEventLog.Fields.EventTrigger), (object) this.InstanceId, (object) this.Get<string>(Log.CommonFields.LoanId), (object) this.CorrelationId, (object) this.Get<string>(Log.CommonFields.LoanFilePath), (object) this.Get<DateTime>(KafkaEventLog.Fields.PublishTime));
      }
      else
      {
        string str = this.Error.Message;
        if (this.Error.Exception is AggregateException)
        {
          foreach (Exception innerException in ((AggregateException) this.Error.Exception).InnerExceptions)
            str = str + " " + innerException.Message + " " + (innerException.InnerException != null ? innerException.InnerException.Message : string.Empty);
        }
        logMessage = string.Format("Publishing message failed for type: {0} loanId: {1}, CorrelationId: {2}; Failure reason: {3}; instanceId: {4}; Stack Trace: {5}. Please check the kafka event log files to view failed messages.", (object) this.Get<string>(KafkaEventLog.Fields.EventTrigger), (object) this.Get<string>(Log.CommonFields.LoanId), (object) this.CorrelationId, (object) str, (object) this.InstanceId, this.Error.StackTrace != null || this.Error.Xcause == null ? (object) this.Error.StackTrace : (object) this.Error.Xcause.StackTrace);
      }
      JObject tvalue;
      if (this.TryGet<JObject>(KafkaEventLog.Fields.MessageJson, out tvalue))
        logMessage += string.Format("message - json: {0}", (object) JsonConvert.SerializeObject((object) tvalue));
      return logMessage;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("_success", this._success);
      base.GetObjectData(info, context);
    }

    public bool IsSuccess() => this._success;

    public class Fields
    {
      public static readonly LogFieldName<string> EventId = LogFields.Field<string>("kafkaEventId");
      public static readonly LogFieldName<string> EventTrigger = LogFields.Field<string>("kafkaEventTrigger");
      public static readonly LogFieldName<string> EventType = LogFields.Field<string>("kafkaEventType");
      public static readonly LogFieldName<string> Topic = LogFields.Field<string>("kafkaTopic");
      public static readonly LogFieldName<DateTime> PublishTime = LogFields.Field<DateTime>("kafkaEventTimestamp");
      public static readonly LogFieldName<string> Source = LogFields.Field<string>("kafkaPublisher");
      public static readonly LogFieldName<JObject> MessageJson = LogFields.Field<JObject>("kafkaMessageJson");
    }
  }
}

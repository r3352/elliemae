// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServiceObjects.KafkaEvent.KafkaEventsAggregatedLog
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Encompass.Diagnostics.Logging.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServiceObjects.KafkaEvent
{
  public class KafkaEventsAggregatedLog : Log
  {
    public KafkaEventsAggregatedLog(KafkaEventsAggregatedLog.EventData[] events)
    {
      this.Set<KafkaEventsAggregatedLog.EventData[]>(KafkaEventsAggregatedLog.Fields.KafkaEvents, events);
    }

    public class EventData
    {
      public EventData(KafkaEventLog log)
      {
        string tvalue1;
        if (log.TryGet<string>(KafkaEventLog.Fields.EventId, out tvalue1))
          this.Id = tvalue1;
        string tvalue2;
        if (log.TryGet<string>(KafkaEventLog.Fields.Topic, out tvalue2))
          this.Topic = tvalue2;
        string tvalue3;
        if (log.TryGet<string>(KafkaEventLog.Fields.EventType, out tvalue3))
          this.EventType = tvalue3;
        string tvalue4;
        if (log.TryGet<string>(KafkaEventLog.Fields.EventTrigger, out tvalue4))
          this.EventTrigger = tvalue4;
        DateTime tvalue5;
        if (log.TryGet<DateTime>(KafkaEventLog.Fields.PublishTime, out tvalue5))
          this.PublishTime = new DateTime?(tvalue5);
        JObject tvalue6;
        if (log.TryGet<JObject>(KafkaEventLog.Fields.MessageJson, out tvalue6))
          this.MessageJson = tvalue6;
        if (log.IsSuccess())
        {
          this.Success = true;
        }
        else
        {
          this.Success = false;
          this.Error = log.Error;
        }
      }

      [JsonProperty("id")]
      public string Id { get; set; }

      [JsonProperty("topic")]
      public string Topic { get; set; }

      [JsonProperty("eventType")]
      public string EventType { get; set; }

      [JsonProperty("eventTrigger")]
      public string EventTrigger { get; set; }

      [JsonProperty("eventTimestamp")]
      public DateTime? PublishTime { get; set; }

      [JsonProperty("message")]
      public JObject MessageJson { get; set; }

      [JsonProperty("success")]
      public bool Success { get; set; }

      [JsonProperty("error")]
      public LogErrorData Error { get; set; }
    }

    public class Fields
    {
      public static readonly LogFieldName<KafkaEventsAggregatedLog.EventData[]> KafkaEvents = LogFields.Field<KafkaEventsAggregatedLog.EventData[]>("kafkaEvents");
    }
  }
}

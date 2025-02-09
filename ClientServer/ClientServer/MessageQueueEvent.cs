// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageQueueEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using com.elliemae.services.eventbus.models;
using Elli.Common.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public abstract class MessageQueueEvent
  {
    public StandardMessage StandardMessage { get; set; }

    public Dictionary<string, object> DataBag { get; set; }

    public List<QueueMessage> QueueMessages { get; set; }

    public string MessageType => this.GetType().Name.Replace("Event", "");

    public string CreatePayload(QueueMessage message)
    {
      Event @event = new Event()
      {
        Id = Guid.NewGuid().ToString(),
        CorrelationId = !string.IsNullOrEmpty(message.CorrelationId) ? message.CorrelationId : Guid.NewGuid().ToString(),
        EntityId = this.StandardMessage.EntityId,
        Category = EnumUtils.StringValueOf((Enum) this.StandardMessage.Category),
        Source = this.StandardMessage.Source,
        Tenant = !string.IsNullOrEmpty(this.StandardMessage.Tenant) ? this.StandardMessage.Tenant : string.Format("urn:elli:encompass:{0}:user:{1}", (object) this.StandardMessage.InstanceId, (object) this.StandardMessage.UserId),
        Type = message.Type,
        CreatedDate = (this.StandardMessage.CreateAt == DateTime.MinValue ? DateTime.UtcNow : this.StandardMessage.CreateAt).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"),
        Payload = message.GetPayload()
      };
      if (message.EnvelopeVersion != null)
        @event.EnvelopeVersion = message.EnvelopeVersion;
      if (message.PayloadVersion != null)
        @event.PayloadVersion = message.PayloadVersion;
      return new JsonSerializer<Event>().Serialize(@event, Formatting.None, new JsonSerializerSettings()
      {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = (IContractResolver) new CamelCasePropertyNamesContractResolver(),
        Converters = (IList<JsonConverter>) new List<JsonConverter>()
        {
          (JsonConverter) new StringEnumConverter()
        }
      });
    }

    public abstract string GetTopic(string messageType);
  }
}

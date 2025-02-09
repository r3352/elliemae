// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServiceObjects.KafkaEvent.KafkaTradeProcessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Confluent.Kafka;
using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.MessageServices.Kafka;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ServiceInterface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server.ServiceObjects.KafkaEvent
{
  public class KafkaTradeProcessor : IMessageQueueProcessor, IContextBoundObject
  {
    private const string className = "KafkaProcessor�";

    public MessageQueueEventParameters queueEventParameters { get; set; }

    public void Publish(
      string messageRoutingKey,
      string payload,
      string correlationId,
      MessageQueueEventParameters messageQueueEventParameters)
    {
      messageQueueEventParameters.QueueMessage.LoanId = messageQueueEventParameters.EntityId;
      string str = "Before sending kafka message {0} message for event: {1}, instanceId: {2}, tradeId: {3}, correlationId: {4}, {0}: {5}, publishedTime: {6}, processOperation: {7}";
      this.writeTraceLog(messageQueueEventParameters, str);
      if (KafkaUtils.IsKafkaWarningLogEnabled && messageQueueEventParameters.MessageType.Equals("TradeSync"))
      {
        QueueMessage queueMessage = messageQueueEventParameters.QueueMessage;
        TraceLog.WriteWarning("KafkaProcessor", string.Format(str, (object) messageQueueEventParameters.MessageType, (object) queueMessage.Type, (object) messageQueueEventParameters.InstanceId, (object) messageQueueEventParameters.EntityId, (object) queueMessage.CorrelationId, (object) queueMessage.LoanPath, (object) queueMessage.PublishTime, KafkaUtils.IsAsyncEnabled ? (object) "asynchronous" : (object) "synchronous"));
      }
      IProducer<string, string> staticProducer = KafkaProducer.StaticProducer;
      string topic = messageRoutingKey;
      Message<string, string> message = new Message<string, string>();
      message.Key = messageQueueEventParameters.EntityId;
      message.Value = payload;
      CancellationToken cancellationToken = new CancellationToken();
      staticProducer.ProduceAsync(topic, message, cancellationToken).ContinueWith((Action<Task<DeliveryResult<string, string>>, object>) ((taskResult, objectStateParam) =>
      {
        TraceLog.WriteInfo("KafkaProcessor", string.Format("Entering continue with task status {0} for the message {1}", (object) taskResult.Status.ToString(), (object) objectStateParam.ToString()));
        KafkaEventResponseHandler eventResponseHandler = new KafkaEventResponseHandler();
        if (!taskResult.IsCompleted)
          return;
        if (taskResult.Exception == null && taskResult.Result != null && taskResult.Result.Message != null)
        {
          TraceLog.WriteInfo("KafkaProcessor", string.Format("Publishing kafka message is successful for the event with task status {0} for the message {1}", (object) taskResult.Status.ToString(), (object) taskResult.Result.Message.Value));
        }
        else
        {
          if (taskResult.Exception == null)
            return;
          TraceLog.WriteException(TraceLevel.Error, "KafkaProcessor", (Exception) taskResult.Exception);
        }
      }), (object) JsonConvert.SerializeObject((object) messageQueueEventParameters));
      string logFormat = "Sent kafka message {0} message for event: {1}, instanceId: {2}, tradeId: {3}, correlationId: {4}, {0}: {5}, publishedTime: {6}";
      this.writeTraceLog(messageQueueEventParameters, logFormat);
    }

    public async Task<DeliveryResult<string, string>> produceAsync(
      string topic,
      string tradeId,
      string payload,
      IProducer<string, string> producer)
    {
      IProducer<string, string> producer1 = producer;
      string topic1 = topic;
      Message<string, string> message = new Message<string, string>();
      message.Key = tradeId;
      message.Value = payload;
      CancellationToken cancellationToken = new CancellationToken();
      return await producer1.ProduceAsync(topic1, message, cancellationToken).ConfigureAwait(false);
    }

    public void Publish(MessageQueueEvent messageQueueEvent)
    {
      this.PublishAsync(messageQueueEvent);
    }

    private void PublishAsync(MessageQueueEvent messageQueueEvent)
    {
      foreach (QueueMessage queueMessage in messageQueueEvent.QueueMessages)
      {
        string str = string.Empty;
        string topic = messageQueueEvent.GetTopic(queueMessage.Type);
        MessageQueueEventParameters messageQueueEventParameters = new MessageQueueEventParameters(messageQueueEvent.StandardMessage.InstanceId, messageQueueEvent.MessageType, topic, messageQueueEvent.StandardMessage.EntityId, queueMessage);
        KafkaEventResponseHandler eventResponseHandler = new KafkaEventResponseHandler();
        try
        {
          str = messageQueueEvent.CreatePayload(queueMessage);
          this.Publish(topic, str, queueMessage.CorrelationId, messageQueueEventParameters);
        }
        catch (ProduceException<string, string> ex)
        {
          eventResponseHandler.FailedMessageHandler(str, JsonConvert.SerializeObject((object) messageQueueEventParameters), messageQueueEventParameters.Topic, new AggregateException(new Exception[1]
          {
            (Exception) ex
          }));
        }
        catch (Exception ex)
        {
          eventResponseHandler.FailedMessageHandler(str, JsonConvert.SerializeObject((object) messageQueueEventParameters), messageQueueEventParameters.Topic, new AggregateException(new Exception[1]
          {
            ex
          }));
        }
      }
    }

    private void writeTraceLog(
      MessageQueueEventParameters messageQueueEventParameters,
      string logFormat)
    {
      string empty = string.Empty;
      QueueMessage queueMessage = messageQueueEventParameters.QueueMessage;
      string message;
      if (KafkaUtils.IsKafkaDebugLogEnabled)
      {
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        foreach (KeyValuePair<string, string> producerConfig in KafkaUtils.ProducerConfigs)
        {
          string key = producerConfig.Key;
          if (key != EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerConfigsEnum.SASL_MECHANISMS) && key != EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerConfigsEnum.SASL_PASSWORD) && key != EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerConfigsEnum.SASL_USERNAME) && key != EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerConfigsEnum.SECURITY_PROTOCOL) && key != EnumUtil.GetEnumDescription((Enum) KafkaEnums.ProducerConfigsEnum.SSL_CA_LOCATION))
            dictionary.Add(key, (object) producerConfig.Value);
        }
        string str = JsonConvert.SerializeObject((object) dictionary, Formatting.Indented);
        message = string.Format(logFormat, (object) messageQueueEventParameters.MessageType, (object) queueMessage.Type, (object) messageQueueEventParameters.InstanceId, (object) queueMessage.LoanId, (object) queueMessage.CorrelationId, (object) queueMessage.LoanPath, (object) queueMessage.PublishTime, (object) str);
      }
      else
        message = string.Format(logFormat, (object) messageQueueEventParameters.MessageType, (object) queueMessage.Type, (object) messageQueueEventParameters.InstanceId, (object) queueMessage.LoanId, (object) queueMessage.CorrelationId, (object) queueMessage.LoanPath, (object) queueMessage.PublishTime, KafkaUtils.IsAsyncEnabled ? (object) "asynchronous" : (object) "synchronous");
      TraceLog.WriteInfo("KafkaProcessor", message);
    }

    public void handler(DeliveryReport<string, string> deliveryReport)
    {
      ClientContext ctx = ClientContext.Open(this.queueEventParameters.InstanceId);
      using (ctx.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        ErrorCode code = deliveryReport.Error.Code;
        if (!code.Equals((object) ErrorCode.NoError))
        {
          object[] objArray = new object[4]
          {
            (object) deliveryReport.Error.Reason,
            null,
            null,
            null
          };
          code = deliveryReport.Error.Code;
          objArray[1] = (object) code.ToString();
          bool flag = deliveryReport.Error.IsBrokerError;
          objArray[2] = (object) flag.ToString();
          flag = deliveryReport.Error.IsLocalError;
          objArray[3] = (object) flag.ToString();
          TraceLog.WriteError("KafkaProcessor", string.Format("Publishing kafka message failed for the event: Reason: {0}, ErrorCode: {1}, IsBrokerError: {2}, IsLocalError : {3}", objArray));
          KafkaEventLogger.RegisterSource(ctx.InstanceName, (IClientContext) ctx);
          KafkaEventLogger.Write(ctx.InstanceName, this.queueEventParameters.Topic, this.queueEventParameters.EntityId, this.queueEventParameters.InstanceId, this.queueEventParameters.MessageType, deliveryReport.Message.Value, this.queueEventParameters.QueueMessage);
        }
        else
        {
          QueueMessage queueMessage = this.queueEventParameters.QueueMessage;
          TraceLog.WriteInfo("KafkaProcessor", string.Format("Publishing {0} message for event: {1}, instanceId: {2}, tradeId: {3}, correlationId: {4}, {0}: {5}, publishedTime: {6}", (object) this.queueEventParameters.MessageType, (object) queueMessage.Type, (object) this.queueEventParameters.InstanceId, (object) queueMessage.LoanId, (object) queueMessage.CorrelationId, (object) queueMessage.LoanPath, (object) queueMessage.PublishTime));
        }
      }
    }

    public IServiceContext ServiceContext => throw new NotImplementedException();
  }
}

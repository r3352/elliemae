// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServiceObjects.KafkaEvent.KafkaEventResponseHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.MessageServices.Kafka.Message;
using EllieMae.EMLite.ClientServer.MessageServices.Message;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServiceObjects.KafkaEvent
{
  public class KafkaEventResponseHandler
  {
    public const string ClassName = "KafkaEventResponseHandler�";
    private readonly IKafkaEventHandlerContext _context;
    private readonly ILogger _logger;

    public KafkaEventResponseHandler()
      : this(KafkaEventHandlerContextFactory.Instance.Get())
    {
    }

    public KafkaEventResponseHandler(IKafkaEventHandlerContext context)
    {
      this._context = context;
      this._context.Add(this);
      this._logger = DiagUtility.LogManager.GetLogger("Kafka.Messages");
    }

    public void WriteSucessLog(string deliveryResultMessage, string objectStateParam, string topic)
    {
      JObject objectState = JObject.Parse(objectStateParam);
      JObject resultObj = JObject.Parse(deliveryResultMessage);
      string instanceName = (string) objectState["InstanceId"];
      string messageType = this.getMessageType(objectState);
      string loanId = this.getLoanId(messageType, resultObj);
      string loanPath = this.getLoanPath(resultObj);
      DateTime publishTime = this.getPublishTime(resultObj);
      string correlationId = this.getCorrelationId(resultObj);
      string eventId = this.getEventId(resultObj);
      string source = this.getSource(resultObj);
      using (ClientContext.Open(instanceName).MakeCurrent((IDataCache) null, correlationId, new Guid?(), new bool?()))
      {
        if (this._logger.IsEnabled(Encompass.Diagnostics.Logging.LogLevel.INFO) || this._logger.IsEnabled(Encompass.Diagnostics.Logging.LogLevel.DEBUG))
        {
          KafkaEventLog log = new KafkaEventLog(true);
          log.Level = Encompass.Diagnostics.Logging.LogLevel.INFO;
          log.Src = nameof (KafkaEventResponseHandler);
          log.Message = "Publishing kafka message is successful for the event";
          log.Set<string>(KafkaEventLog.Fields.EventId, eventId);
          log.Set<string>(KafkaEventLog.Fields.Topic, topic);
          log.Set<string>(KafkaEventLog.Fields.EventType, messageType);
          log.Set<string>(KafkaEventLog.Fields.EventTrigger, (string) resultObj["type"]);
          if (!string.IsNullOrEmpty(loanId))
            log.Set<string>(Log.CommonFields.LoanId, loanId);
          if (!string.IsNullOrEmpty(loanPath))
            log.Set<string>(Log.CommonFields.LoanFilePath, loanPath);
          log.Set<DateTime>(KafkaEventLog.Fields.PublishTime, publishTime);
          log.Set<string>(KafkaEventLog.Fields.Source, source);
          if (KafkaUtils.IsKafkaDebugLogEnabled || this._logger.IsEnabled(Encompass.Diagnostics.Logging.LogLevel.DEBUG))
            log.Set<JObject>(KafkaEventLog.Fields.MessageJson, resultObj);
          this._context.Complete(this, log);
        }
        else
          this._context.Complete(this, (KafkaEventLog) null);
      }
    }

    public void FailedMessageHandler(
      string exceptionResultMessage,
      string objectStateParam,
      string topic,
      AggregateException exceptions)
    {
      JObject resultObj = JObject.Parse(exceptionResultMessage);
      JObject objectState = JObject.Parse(objectStateParam);
      string instanceName = (string) objectState["InstanceId"];
      string messageType = this.getMessageType(objectState);
      string loanId = this.getLoanId(messageType, resultObj);
      string loanPath = this.getLoanPath(resultObj);
      DateTime publishTime = this.getPublishTime(resultObj);
      string correlationId = this.getCorrelationId(resultObj);
      string eventId = this.getEventId(resultObj);
      string source = this.getSource(resultObj);
      ClientContext clientContext = ClientContext.Open(instanceName);
      using (clientContext.MakeCurrent((IDataCache) null, correlationId, new Guid?(), new bool?()))
      {
        KafkaEventLog log = new KafkaEventLog(false);
        log.Level = Encompass.Diagnostics.Logging.LogLevel.WARN;
        log.Src = nameof (KafkaEventResponseHandler);
        log.Message = "Publishing kafka message failed for the event";
        log.Error = new LogErrorData((Exception) exceptions);
        log.Set<string>(KafkaEventLog.Fields.EventId, eventId);
        log.Set<string>(KafkaEventLog.Fields.Topic, topic);
        log.Set<string>(KafkaEventLog.Fields.EventType, messageType);
        log.Set<string>(KafkaEventLog.Fields.EventTrigger, (string) resultObj["type"]);
        if (!string.IsNullOrEmpty(loanId))
          log.Set<string>(Log.CommonFields.LoanId, loanId);
        if (!string.IsNullOrEmpty(loanPath))
          log.Set<string>(Log.CommonFields.LoanFilePath, loanPath);
        log.Set<DateTime>(KafkaEventLog.Fields.PublishTime, publishTime);
        log.Set<string>(KafkaEventLog.Fields.Source, source);
        if (KafkaUtils.IsKafkaDebugLogEnabled)
          log.Set<JObject>(KafkaEventLog.Fields.MessageJson, resultObj);
        this._context.Complete(this, log);
        this.WriteFailedMessageToFile((IClientContext) clientContext, exceptionResultMessage, objectStateParam, topic);
      }
    }

    private void WriteFailedMessageToFile(
      IClientContext clientContext,
      string exceptionResultMessage,
      string objectStateParam,
      string topic)
    {
      JObject resultObj = JObject.Parse(exceptionResultMessage);
      JObject objectState = JObject.Parse(objectStateParam);
      string instanceId = (string) objectState["InstanceId"];
      string entityId = this.getEntityId(resultObj);
      string messageType1 = this.getMessageType(objectState);
      string messageType2 = (string) resultObj["type"];
      try
      {
        KafkaEventLogger.RegisterSource(clientContext.InstanceName, clientContext);
        KafkaEventLogger.Write(clientContext.InstanceName, topic, entityId, instanceId, messageType1, exceptionResultMessage, this.getQueueMessage(messageType2, exceptionResultMessage));
      }
      catch (Exception ex)
      {
        this._logger.Write(Encompass.Diagnostics.Logging.LogLevel.ERROR, nameof (KafkaEventResponseHandler), string.Format("Error writing failed message with instanceId - {0}, topic - {1}, payload - {2}", (object) instanceId, (object) topic, (object) exceptionResultMessage));
        throw ex;
      }
    }

    private QueueMessage getQueueMessage(string messageType, string messagePayload)
    {
      switch (messageType)
      {
        case "DeferredLoan.ReportingDatabase":
          return (QueueMessage) JsonConvert.DeserializeObject<ReportingDbMessage>(messagePayload);
        case "DeferredLoan.AuditTrail":
          return (QueueMessage) JsonConvert.DeserializeObject<AuditTrailMessage>(messagePayload);
        case "DeferredLoan.EmailTrigger":
          return (QueueMessage) JsonConvert.DeserializeObject<EmailNotificationMessage>(messagePayload);
        case "Loan.Event.Webhooks":
          return (QueueMessage) JsonConvert.DeserializeObject<WebHooksMessage>(messagePayload);
        default:
          return (QueueMessage) JsonConvert.DeserializeObject<LoanEventQueueMessage>(messagePayload);
      }
    }

    private string getLoanId(string messageType, JObject resultObj)
    {
      string entityId = this.getEntityId(resultObj);
      return Convert.ToString((object) resultObj["payloadVersion"]) == "2.0.0" || string.Equals(messageType, "WebHooks", StringComparison.OrdinalIgnoreCase) || string.Equals(messageType, "LoanBilling", StringComparison.OrdinalIgnoreCase) ? entityId : Convert.ToString((object) resultObj["payload"][(object) "loanId"]);
    }

    private string getCorrelationId(JObject resultObj)
    {
      return Convert.ToString((object) resultObj["correlationId"]);
    }

    private string getLoanPath(JObject resultObj)
    {
      return Convert.ToString((object) resultObj["payload"][(object) "loanPath"]);
    }

    private DateTime getPublishTime(JObject resultObj)
    {
      return Convert.ToDateTime((object) resultObj["payload"][(object) "publishTime"]);
    }

    private string getEntityId(JObject resultObj)
    {
      return Convert.ToString((object) resultObj["entityId"]);
    }

    private string getEventId(JObject resultObj) => Convert.ToString((object) resultObj["id"]);

    private string getSource(JObject resultObj) => Convert.ToString((object) resultObj["source"]);

    private string getInstanceId(JObject resultObject)
    {
      string str = (string) resultObject["tenant"];
      if (!string.IsNullOrWhiteSpace(str))
        return string.Empty;
      return str.Split(':')[3];
    }

    private string getMessageType(JObject objectState)
    {
      return Convert.ToString((object) objectState["MessageType"]);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerTasks.KafkaEventForwarderTask
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Server.ServerObjects.LoanEvent;
using EllieMae.EMLite.Server.ServiceObjects.KafkaEvent;
using EllieMae.EMLite.Server.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerTasks
{
  internal class KafkaEventForwarderTask : ITaskHandler
  {
    private static readonly string ClassName = nameof (KafkaEventForwarderTask);
    private EventLogRepository _repo = new EventLogRepository();

    public void ProcessTask(ServerTask taskInfo)
    {
      ClientContext current = ClientContext.GetCurrent();
      string logDir = LoanEventLogger.EventLogDir((IClientContext) current);
      current.TraceLog.WriteInfo(KafkaEventForwarderTask.ClassName, string.Format("Starting::KafkaEventForwarder processing Client Instance {0} for logDir {1}", (object) current.InstanceName, (object) logDir));
      this.ForwardEvents(logDir, current);
      current.TraceLog.WriteInfo(KafkaEventForwarderTask.ClassName, string.Format("Completed::KafkaEventForwarder processing Client Instance {0} for logDir {1}", (object) current.InstanceName, (object) logDir));
    }

    internal int ForwardEvents(string logDir, ClientContext ctx)
    {
      int num1 = 0;
      IList<EventLog> outstanding = this._repo.GetOutstanding();
      if (!Directory.Exists(logDir))
      {
        ctx.TraceLog.WriteInfo(KafkaEventForwarderTask.ClassName, "Event log directory is missing.  " + logDir);
        return num1;
      }
      foreach (string file1 in Directory.GetFiles(logDir, "kafka.*.log"))
      {
        string file = file1;
        EventLog eventLog = outstanding.FirstOrDefault<EventLog>((Func<EventLog, bool>) (sts => string.Compare(sts.FilePath, file, StringComparison.OrdinalIgnoreCase) == 0));
        if (eventLog == null)
        {
          eventLog = new EventLog()
          {
            FileOwner = LoanEventLogger.ProcessOwner,
            LastModified = DateTime.UtcNow,
            FilePath = file,
            Position = 0L,
            Status = EventLogStatus.InProgress
          };
          if (!this._repo.Add(eventLog))
            continue;
        }
        if (eventLog.IsComplete && File.GetLastWriteTimeUtc(file) > eventLog.LastModified)
        {
          eventLog.LastModified = DateTime.UtcNow;
          eventLog.Status = EventLogStatus.InProgress;
          this._repo.Update(eventLog);
        }
        else if (eventLog.IsComplete)
        {
          this.CleanupLogFile(eventLog, ctx);
          continue;
        }
        if (string.Compare(eventLog.FileOwner, LoanEventLogger.ProcessOwner, StringComparison.OrdinalIgnoreCase) != 0 && DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(6.0)) <= eventLog.LastModified)
        {
          ctx.TraceLog.WriteInfo(KafkaEventForwarderTask.ClassName, "  Skipping log file.  Not the current owner.  " + eventLog.FileOwner + " != " + LoanEventLogger.ProcessOwner + " and LastModified = " + (object) eventLog.LastModified);
        }
        else
        {
          int num2 = this.PublishEvents(eventLog, ctx);
          num1 += num2;
        }
      }
      if (num1 > 0)
        ctx.TraceLog.WriteWarning(KafkaEventForwarderTask.ClassName, string.Format("Finished::KafkaEventForwarder processing Client Instance {0} for logDir {1}.  Republished {2} events.", (object) ctx.InstanceName, (object) logDir, (object) num1));
      return num1;
    }

    private int PublishEvents(EventLog current, ClientContext ctx)
    {
      int num1 = 0;
      FileStream fileStream = File.Open(current.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
      fileStream.Position = current.Position;
      int num2 = 50;
      List<KafkaEventMessage> kafkaEventMessageList = new List<KafkaEventMessage>();
      KafkaProcessor kafkaProcessor = new KafkaProcessor();
      StreamReader streamReader = new StreamReader((Stream) fileStream);
      while (!streamReader.EndOfStream)
      {
        KafkaEventMessage kafkaEventMessage1 = this.DeserializeKafkaEvent(streamReader.ReadLine(), ctx);
        if (kafkaEventMessage1 != null)
          kafkaEventMessageList.Add(kafkaEventMessage1);
        if (kafkaEventMessageList.Count == num2)
        {
          foreach (KafkaEventMessage kafkaEventMessage2 in kafkaEventMessageList)
          {
            kafkaProcessor.Publish(kafkaEventMessage2.Topic, kafkaEventMessage2.Payload, kafkaEventMessage2.CorrelationId, new MessageQueueEventParameters(kafkaEventMessage2.InstanceId, kafkaEventMessage2.MessageType, kafkaEventMessage2.Topic, kafkaEventMessage2.EntityId, kafkaEventMessage2.Message));
            ++num1;
          }
          kafkaEventMessageList.Clear();
          current.Position = fileStream.Position;
          current.LastModified = DateTime.UtcNow;
          this._repo.Update(current);
        }
      }
      if (kafkaEventMessageList.Count > 0)
      {
        foreach (KafkaEventMessage kafkaEventMessage in kafkaEventMessageList)
        {
          kafkaProcessor.Publish(kafkaEventMessage.Topic, kafkaEventMessage.Payload, kafkaEventMessage.CorrelationId, new MessageQueueEventParameters(kafkaEventMessage.InstanceId, kafkaEventMessage.MessageType, kafkaEventMessage.Topic, kafkaEventMessage.EntityId, kafkaEventMessage.Message));
          ++num1;
        }
        kafkaEventMessageList.Clear();
      }
      current.Position = fileStream.Position;
      current.Status = EventLog.UpdateStatus(current.FilePath);
      current.LastModified = DateTime.UtcNow;
      this._repo.Update(current);
      return num1;
    }

    private KafkaEventMessage DeserializeKafkaEvent(string line, ClientContext ctx)
    {
      try
      {
        return JsonConvert.DeserializeObject<KafkaEventMessage>(line, new JsonSerializerSettings()
        {
          TypeNameHandling = TypeNameHandling.All
        });
      }
      catch (Exception ex)
      {
        ctx.TraceLog.WriteError(KafkaEventForwarderTask.ClassName, "KafkaLoanEvent failed to be re-published due to a serialization error.  Event is lost.  Raw Event Data: " + line + ".  Stack Trace:  " + (object) ex);
      }
      return (KafkaEventMessage) null;
    }

    private void CleanupLogFile(EventLog current, ClientContext ctx)
    {
      if (DateTime.Now.Hour != 2)
        return;
      try
      {
        File.Delete(current.FilePath);
        current.Status = EventLogStatus.Deleted;
        this._repo.Update(current);
      }
      catch
      {
        ctx.TraceLog.WriteWarning(KafkaEventForwarderTask.ClassName, "Failed to delete log file:  " + current.FilePath);
      }
    }
  }
}

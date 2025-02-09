// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServiceObjects.Repositories.QueueMessageRepository
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.MessageServices.Utils;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.ServiceInterface.Repositories;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServiceObjects.Repositories
{
  public class QueueMessageRepository : IQueueMessageRepository
  {
    public bool InsertQueueMessageHistory(
      string queueName,
      string correlationId,
      string refEntityId,
      string category,
      string source,
      string type,
      string eventAction,
      string reason,
      string message)
    {
      try
      {
        return QueueMessageHistoryAccessor.InsertMessageHistoryRecord(queueName, correlationId, refEntityId, category, source, type, eventAction, reason, message);
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (QueueMessageRepository), string.Format("From {0}, correlation {1}. Failed to write MQ history for {2}, {3}", (object) queueName, (object) correlationId, (object) ex.GetType().ToString(), (object) ex.Message));
      }
      return false;
    }

    public List<QueueMessageHistory> GetMessageHistoryList(int totalRowCount = 0)
    {
      try
      {
        return QueueMessageHistoryAccessor.GetMessageHistoryRecordList(totalRowCount);
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning("QueueMessageRepository - DeleteMessageHistoryByQueue", "exception while get message history list");
      }
      return (List<QueueMessageHistory>) null;
    }

    public void DeleteMessageHistoryByQueue(
      string queueName,
      string correlationId,
      string refEntityId)
    {
      try
      {
        QueueMessageHistoryAccessor.DeleteMessageHistoryRecord(queueName, correlationId, refEntityId);
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning("QueueMessageRepository - DeleteMessageHistoryByQueue", "exception while deleting");
      }
    }
  }
}

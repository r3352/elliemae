// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.QueueMessageHistoryAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.MessageServices.Utils;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  internal static class QueueMessageHistoryAccessor
  {
    private const string className = "QueueMessageHistoryAccessor�";
    private static string tableName = "QueueMessagingHistory";

    public static bool InsertMessageHistoryRecord(
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
      if (QueueMessageHistoryAccessor.IsQueueMessageLogDisabled())
        return true;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("INSERT INTO " + QueueMessageHistoryAccessor.tableName + " ([QueueName],[CorrelationId],[RefEntityId],[Category],[Source],[Type],[EventAction],[Reason],[Message],[Created]) VALUES (" + SQL.Encode((object) queueName) + "," + SQL.Encode((object) correlationId) + "," + SQL.Encode((object) refEntityId) + "," + SQL.Encode((object) category) + "," + SQL.Encode((object) source) + "," + SQL.Encode((object) type) + "," + SQL.Encode((object) eventAction) + "," + SQL.Encode((object) reason) + "," + SQL.Encode((object) message) + "," + SQL.Encode((object) DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss tt")) + ")");
        dbQueryBuilder.ExecuteScalar();
        return true;
      }
      catch (Exception ex)
      {
        TraceLog.Write(TraceLevel.Error, nameof (QueueMessageHistoryAccessor), "Error while inserting into QueueMessagingHistory table - " + dbQueryBuilder.ToString());
        return false;
      }
    }

    public static List<QueueMessageHistory> GetMessageHistoryRecordList(int totalRowCount = 0)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (totalRowCount <= 0)
        dbQueryBuilder.AppendLine("select QueueName, CorrelationId, RefEntityId, Category, Source, Type, EventAction, Message, Reason, Created from QueueMessagingHistory order by Created desc");
      else
        dbQueryBuilder.AppendLine("select top " + (object) totalRowCount + " QueueName, CorrelationId, RefEntityId, Category, Source, Type, EventAction, Message, Reason, Created from QueueMessagingHistory order by Created desc");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection.Count == 0)
        return (List<QueueMessageHistory>) null;
      List<QueueMessageHistory> historyRecordList = new List<QueueMessageHistory>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        historyRecordList.Add(QueueMessageHistoryAccessor.ConvertDataRow(dataRowCollection[0]));
      return historyRecordList;
    }

    public static void DeleteMessageHistoryRecord(
      string queueName,
      string correlationId,
      string refEntityId = "�")
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      if (string.IsNullOrWhiteSpace(queueName) && string.IsNullOrWhiteSpace(correlationId) && string.IsNullOrWhiteSpace(refEntityId))
        return;
      dbQueryBuilder.AppendLine("delete from QueueMessagingHistory where QueueName = " + SQL.Encode((object) queueName));
      if (!string.IsNullOrWhiteSpace(correlationId))
        dbQueryBuilder.AppendLine("AND CorrelationId = " + SQL.Encode((object) correlationId));
      if (!string.IsNullOrWhiteSpace(refEntityId))
        dbQueryBuilder.AppendLine("AND RefEntityId = " + SQL.Encode((object) refEntityId));
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static QueueMessageHistory ConvertDataRow(DataRow dr)
    {
      if (dr == null || dr.HasErrors)
        return (QueueMessageHistory) null;
      return new QueueMessageHistory()
      {
        QueueName = (string) dr["QueueName"],
        CorrelationId = (string) dr["CorrelationId"],
        RefEntityId = (string) dr["RefEntityId"],
        Category = (string) dr["Category"],
        Source = (string) dr["Source"],
        Type = (string) dr["Type"],
        EventAction = (string) dr["EventAction"],
        Message = (string) dr["Message"],
        Reason = (string) dr["Reason"],
        Created = dr["Reason"] != null ? dr["Reason"].ToString() : string.Empty
      };
    }

    private static bool IsQueueMessageLogDisabled()
    {
      return Convert.ToBoolean(ConfigurationManager.AppSettings["DisableQueueMessageLog"]);
    }
  }
}

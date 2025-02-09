// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.TriggersBpmDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public class TriggersBpmDbAccessor : BpmDbAccessor
  {
    public TriggersBpmDbAccessor()
      : base(ClientSessionCacheID.Triggers)
    {
    }

    protected override Type RuleType => typeof (TriggerInfo);

    protected override string RuleTableName => "BR_Triggers";

    [PgReady]
    protected override BizRuleInfo[] GetFilteredRulesFromDatabase(string filter)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from BR_Triggers rules where " + filter + ";");
        pgDbQueryBuilder.AppendLine("select items.* from [TR_Events] items");
        pgDbQueryBuilder.AppendLine("\tinner join [BR_Triggers] rules on items.ruleID = rules.ruleID");
        pgDbQueryBuilder.AppendLine("\twhere " + filter);
        pgDbQueryBuilder.AppendLine("    order by items.ruleID, items.eventIndex;");
        return this.dataSetToTriggerInfos(pgDbQueryBuilder.ExecuteSetQuery());
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Settings);
      dbQueryBuilder.AppendLine("select * from BR_Triggers rules where " + filter);
      dbQueryBuilder.AppendLine("select items.* from [TR_Events] items");
      dbQueryBuilder.AppendLine("\tinner join [BR_Triggers] rules on items.ruleID = rules.ruleID");
      dbQueryBuilder.AppendLine("\twhere " + filter);
      dbQueryBuilder.AppendLine("    order by items.ruleID, items.eventIndex");
      return this.dataSetToTriggerInfos(dbQueryBuilder.ExecuteSetQuery());
    }

    private BizRuleInfo[] dataSetToTriggerInfos(DataSet ds)
    {
      DataTable table1 = ds.Tables[0];
      DataTable table2 = ds.Tables[1];
      List<TriggerInfo> triggerInfoList = new List<TriggerInfo>();
      foreach (DataRow row in (InternalDataCollectionBase) table1.Rows)
        triggerInfoList.Add(this.dataRowToTriggerInfo(row, table2));
      return (BizRuleInfo[]) triggerInfoList.ToArray();
    }

    protected override void WriteAuxiliaryDataToQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      BizRuleInfo rule,
      DbValue keyValue)
    {
      TriggerInfo triggerInfo = (TriggerInfo) rule;
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("TR_Events");
      sql.DeleteFrom(table, keyValue);
      for (int index = 0; index < triggerInfo.Events.Count; ++index)
        sql.InsertInto(table, new DbValueList()
        {
          keyValue,
          {
            "eventIndex",
            (object) index
          },
          {
            "eventXml",
            (object) triggerInfo.Events[index].ToXml()
          }
        }, true, false);
    }

    protected override void DeleteAuxiliaryDataFromQuery(EllieMae.EMLite.Server.DbQueryBuilder sql, DbValue keyValue)
    {
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("TR_Events");
      sql.DeleteFrom(table, keyValue);
    }

    private TriggerInfo dataRowToTriggerInfo(DataRow r, DataTable itemTable)
    {
      int num = (int) r["ruleID"];
      XmlSerializer xmlSerializer = new XmlSerializer();
      List<TriggerEvent> triggerEventList = new List<TriggerEvent>();
      foreach (DataRow dataRow in itemTable.Select("ruleID = " + (object) num))
        triggerEventList.Add((TriggerEvent) xmlSerializer.Deserialize(dataRow["eventXml"].ToString(), typeof (TriggerEvent)));
      return new TriggerInfo(r, triggerEventList.ToArray());
    }
  }
}

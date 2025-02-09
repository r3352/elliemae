// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.StatusOnline.StatusOnlineSetupAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.StatusOnline
{
  public class StatusOnlineSetupAccessor
  {
    private const string STATUS_ONLINE_TRIGGERS = "StatusOnlineTriggers�";
    private const string STATUS_ONLINE_TRIGGER_FIELDS = "StatusOnlineTriggerFields�";
    private const string STATUS_ONLINE_EMAIL_RECIPIENTS = "StatusOnlineEmailRecipients�";

    public static void SaveStatusOnlineSetup(string ownerID, StatusOnlineSetup statusOnlineSetup)
    {
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("StatusOnlineTriggers");
      DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("StatusOnlineTriggerFields");
      DbTableInfo table3 = EllieMae.EMLite.Server.DbAccessManager.GetTable("StatusOnlineEmailRecipients");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbValue key = new DbValue("OwnerID", string.IsNullOrEmpty(ownerID) ? (object) (string) null : (object) ownerID);
      string str1 = string.IsNullOrEmpty(ownerID) ? " WHERE OwnerId IS NULL" : " WHERE OwnerId = '" + ownerID + "'";
      dbQueryBuilder.AppendLine(string.Format("DELETE {0} FROM {0} R INNER JOIN {1} T ON R.StatusOnlineTriggerID = T.StatusOnlineTriggerID {2}", (object) "StatusOnlineEmailRecipients", (object) "StatusOnlineTriggers", (object) str1));
      dbQueryBuilder.AppendLine(string.Format("DELETE {0} FROM {0} F INNER JOIN {1} T ON F.StatusOnlineTriggerID = T.StatusOnlineTriggerID {2}", (object) "StatusOnlineTriggerFields", (object) "StatusOnlineTriggers", (object) str1));
      dbQueryBuilder.DeleteFrom(table1, key);
      dbQueryBuilder.Declare("@StatusOnlineTriggerID", "INT");
      if (statusOnlineSetup.Triggers != null && statusOnlineSetup.Triggers.Count > 0)
      {
        DbValueList values = new DbValueList();
        foreach (StatusOnlineTrigger trigger in (CollectionBase) statusOnlineSetup.Triggers)
        {
          values.Add(new DbValue("GuID", (object) trigger.Guid));
          values.Add(new DbValue("Name", (object) trigger.Name));
          values.Add(new DbValue("Description", (object) trigger.Description));
          values.Add(new DbValue("OwnerID", string.IsNullOrEmpty(trigger.OwnerID) ? (object) (string) null : (object) trigger.OwnerID));
          values.Add(new DbValue("PortalTypeID", (object) (int) trigger.PortalType));
          values.Add(new DbValue("UpdateTypeID", (object) (int) trigger.UpdateType));
          values.Add(new DbValue("ReminderTypeID", (object) (int) trigger.ReminderType));
          values.Add(new DbValue("RequirementTypeID", (object) (int) trigger.RequirementType));
          values.Add(new DbValue("RequirementData", (object) trigger.RequirementData));
          if (trigger.RequirementType == TriggerRequirementType.Milestone)
            values.Add(new DbValue("MilestoneID", (object) trigger.RequirementData));
          else if (trigger.RequirementType == TriggerRequirementType.DocumentTemplate)
            values.Add(new DbValue("DocumentTemplateGuid", (object) trigger.RequirementData));
          values.Add(new DbValue("EmailTemplateGuid", string.IsNullOrEmpty(trigger.EmailTemplate) ? (object) (string) null : (object) trigger.EmailTemplate));
          values.Add(new DbValue("EmailTemplateOwnerID", (object) trigger.EmailTemplateOwner));
          values.Add(new DbValue("EmailFromTypeID", (object) (int) trigger.EmailFromType));
          values.Add(new DbValue("DateTriggered", (object) trigger.DateTriggered));
          values.Add(new DbValue("DatePublished", (object) trigger.DatePublished));
          dbQueryBuilder.InsertInto(table1, values, true, false);
          dbQueryBuilder.SelectIdentity("@StatusOnlineTriggerID");
          values.Clear();
          if (trigger.RequirementType == TriggerRequirementType.Fields)
          {
            string[] strArray1;
            if (!string.IsNullOrEmpty(trigger.RequirementData))
              strArray1 = trigger.RequirementData.Split(',');
            else
              strArray1 = (string[]) null;
            string[] strArray2 = strArray1;
            if (strArray2 != null)
            {
              foreach (string str2 in strArray2)
              {
                values.Add(new DbValue("FieldID", (object) str2));
                values.Add(new DbValue("StatusOnlineTriggerID", (object) "@StatusOnlineTriggerID", (IDbEncoder) DbEncoding.None));
                dbQueryBuilder.InsertInto(table2, values, true, false);
                values.Clear();
              }
            }
          }
          if (trigger.EmailRecipients != null)
          {
            foreach (string emailRecipient in trigger.EmailRecipients)
            {
              values.Add(new DbValue("EmailRecipientName", (object) emailRecipient));
              values.Add(new DbValue("StatusOnlineTriggerID", (object) "@StatusOnlineTriggerID", (IDbEncoder) DbEncoding.None));
              dbQueryBuilder.InsertInto(table3, values, true, false);
              values.Clear();
            }
          }
        }
      }
      dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default);
      StatusOnlineStore.SaveSetup(ownerID, statusOnlineSetup);
    }

    public static StatusOnlineSetup GetStatusOnlineSetup(string ownerID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM [{0}]", (object) "StatusOnlineTriggers"));
      if (string.IsNullOrEmpty(ownerID))
        dbQueryBuilder.AppendLine(" WHERE OwnerID IS NULL ");
      else
        dbQueryBuilder.AppendLine(" WHERE OwnerID = " + SQL.EncodeString(ownerID));
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM [{0}] R", (object) "StatusOnlineEmailRecipients"));
      dbQueryBuilder.AppendLine(" INNER JOIN [StatusOnlineTriggers] T");
      dbQueryBuilder.AppendLine(" ON T.StatusOnlineTriggerID = R.StatusOnlineTriggerID ");
      if (string.IsNullOrEmpty(ownerID))
        dbQueryBuilder.AppendLine(" WHERE T.OwnerID IS NULL ");
      else
        dbQueryBuilder.AppendLine(" WHERE T.OwnerID = " + SQL.EncodeString(ownerID));
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM [{0}] F", (object) "StatusOnlineTriggerFields"));
      dbQueryBuilder.AppendLine(" INNER JOIN [StatusOnlineTriggers] T");
      dbQueryBuilder.AppendLine(" ON T.StatusOnlineTriggerID = F.StatusOnlineTriggerID ");
      if (string.IsNullOrEmpty(ownerID))
        dbQueryBuilder.AppendLine(" WHERE T.OwnerID IS NULL ");
      else
        dbQueryBuilder.AppendLine(" WHERE T.OwnerID = " + SQL.EncodeString(ownerID));
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null || dataSet.Tables[0].Rows.Count == 0)
      {
        StatusOnlineSetup setup = StatusOnlineStore.GetSetup(ownerID);
        if (setup.Triggers == null || setup.Triggers.Count == 0)
          return setup;
        if (setup != null)
        {
          StatusOnlineSetupAccessor.SaveStatusOnlineSetup(ownerID, setup);
          return StatusOnlineSetupAccessor.GetStatusOnlineSetup(ownerID);
        }
      }
      StatusOnlineSetup statusOnlineSetup = new StatusOnlineSetup();
      DataRelation relation1 = dataSet.Relations.Add("EmailStatusOnlineTriggerID", dataSet.Tables[0].Columns["StatusOnlineTriggerID"], dataSet.Tables[1].Columns["StatusOnlineTriggerID"]);
      DataRelation relation2 = dataSet.Relations.Add("FieldsStatusOnlineTriggerID", dataSet.Tables[0].Columns["StatusOnlineTriggerID"], dataSet.Tables[2].Columns["StatusOnlineTriggerID"]);
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
      {
        TriggerPortalType portalType = (TriggerPortalType) Enum.Parse(typeof (TriggerPortalType), SQL.DecodeString(row["PortalTypeID"]));
        StatusOnlineTrigger trigger = new StatusOnlineTrigger(ownerID, portalType);
        trigger.Guid = SQL.DecodeString(row["Guid"]);
        trigger.Name = SQL.DecodeString(row["Name"]);
        trigger.Description = SQL.DecodeString(row["Description"]);
        trigger.OwnerID = string.IsNullOrEmpty(SQL.DecodeString(row["OwnerID"])) ? (string) null : SQL.DecodeString(row["OwnerID"]);
        if (!string.IsNullOrEmpty(SQL.DecodeString(row["UpdateTypeID"])))
          trigger.UpdateType = SQL.DecodeEnum<TriggerUpdateType>(row["UpdateTypeID"]);
        if (!string.IsNullOrEmpty(SQL.DecodeString(row["ReminderTypeID"])))
          trigger.ReminderType = SQL.DecodeEnum<TriggerReminderType>(row["ReminderTypeID"]);
        if (!string.IsNullOrEmpty(SQL.DecodeString(row["EmailFromTypeID"])))
          trigger.EmailFromType = SQL.DecodeEnum<TriggerEmailFromType>(row["EmailFromTypeID"]);
        if (!string.IsNullOrEmpty(SQL.DecodeString(row["RequirementTypeID"])))
          trigger.RequirementType = SQL.DecodeEnum<TriggerRequirementType>(row["RequirementTypeID"]);
        trigger.RequirementData = SQL.DecodeString(row["RequirementData"]);
        trigger.EmailTemplate = SQL.DecodeString(row["EmailTemplateGuid"]);
        trigger.EmailTemplateOwner = SQL.DecodeString(row["EmailTemplateOwnerID"]);
        trigger.DateTriggered = SQL.DecodeDateTime(row["DateTriggered"]);
        trigger.DatePublished = SQL.DecodeDateTime(row["DatePublished"]);
        List<string> stringList = new List<string>();
        foreach (DataRow childRow in row.GetChildRows(relation1))
          stringList.Add(SQL.DecodeString(childRow["EmailRecipientName"]));
        if (stringList.Count > 0)
          trigger.EmailRecipients = stringList.ToArray();
        if (trigger.RequirementType == TriggerRequirementType.Fields)
        {
          List<string> values = new List<string>();
          foreach (DataRow childRow in row.GetChildRows(relation2))
            values.Add(SQL.DecodeString(childRow["FieldID"]));
          if (values.Count > 0)
            trigger.RequirementData = string.Join(",", (IEnumerable<string>) values);
        }
        statusOnlineSetup.Triggers.Add(trigger);
      }
      return statusOnlineSetup;
    }

    public static string[] GetMilestonesByStatusOnlineTriggerGUIDs(string[] selectedGUIDs)
    {
      List<string> stringList = new List<string>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append(string.Format("SELECT DISTINCT (M.Name + ',' + M.MilestoneID) AS Name FROM [{0}] S ", (object) "StatusOnlineTriggers"));
      dbQueryBuilder.Append("INNER JOIN Milestones M ON S.MilestoneID = M.MilestoneID ");
      dbQueryBuilder.Append(string.Format("WHERE S.GUID in ({0})", (object) ("'" + string.Join("','", selectedGUIDs) + "'")));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          stringList.Add(dataRow["Name"].ToString());
      }
      return stringList.ToArray();
    }
  }
}

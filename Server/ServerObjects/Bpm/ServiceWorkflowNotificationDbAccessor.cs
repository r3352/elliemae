// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.ServiceWorkflowNotificationDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public class ServiceWorkflowNotificationDbAccessor
  {
    private const string WorkflowNotificationTable = "WF_Notifications�";
    private const string WorkflowNotificationRecipientTable = "WF_NotificationRecipients�";
    private const string NotificationColumnNotificationID = "NotificationID�";
    private const string NotificationColumnRuleID = "RuleID�";
    private const string NotificationColumnType = "NotificationType�";
    private const string NotificationColumnSubject = "Subject�";
    private const string NotificationColumnText = "Text�";
    private const string NotificationColumnLastModifiedByUserId = "LastModifiedByUserId�";
    private const string NotificationColumnLastModified = "LastModified�";
    private const string RecipientColumnNotificationID = "NotificationID�";
    private const string RecipientColumnType = "RecipientType�";
    private const string RecipientColumnUserID = "RecipientUserID�";
    private const string RecipientColumnRoleID = "RecipientRoleID�";

    public List<ServiceWorkflowNotification> GetServiceWorkflowNotificationsByRuleID(int ruleID)
    {
      List<ServiceWorkflowNotification> notificationsByRuleId = new List<ServiceWorkflowNotification>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("select * from {0} where {1} = {2}", (object) "WF_Notifications", (object) "RuleID", (object) ruleID));
      dbQueryBuilder.AppendLine(string.Format("select * from {0} inner join {1} on {0}.{2} = {1}.{3} and {1}.{4} = {5}", (object) "WF_NotificationRecipients", (object) "WF_Notifications", (object) "NotificationID", (object) "NotificationID", (object) "RuleID", (object) ruleID));
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null || dataSet.Tables.Count == 0)
        return notificationsByRuleId;
      DataRelation relation = dataSet.Relations.Add(dataSet.Tables[0].Columns["NotificationID"], dataSet.Tables[1].Columns["NotificationID"]);
      DataTable table = dataSet.Tables[0];
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        DataRow row = table.Rows[index];
        ServiceWorkflowNotification notification = this.ConvertDataRowToNotification(row);
        notificationsByRuleId.Add(notification);
        DataRow[] childRows = row.GetChildRows(relation);
        if (childRows != null && childRows.Length != 0)
        {
          notification.Recipients = new List<WorkflowNotificationRecipient>();
          foreach (DataRow dataRow in childRows)
          {
            WorkflowNotificationRecipient notificationRecipient = new WorkflowNotificationRecipient();
            notificationRecipient.Type = (WorkflowNotificationRecipientType) Convert.ToInt32(dataRow["RecipientType"]);
            if (notificationRecipient.Type == WorkflowNotificationRecipientType.Role)
              notificationRecipient.RoleID = dataRow["RecipientRoleID"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["RecipientRoleID"]);
            else
              notificationRecipient.UserID = dataRow["RecipientUserID"] == DBNull.Value ? string.Empty : (string) dataRow["RecipientUserID"];
            notification.Recipients.Add(notificationRecipient);
          }
        }
      }
      return notificationsByRuleId;
    }

    public ServiceWorkflowNotification GetServiceWorkflowNotificationById(Guid notificationID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("WF_Notifications");
      DbValue key = new DbValue("NotificationID", (object) notificationID.ToString());
      dbQueryBuilder.SelectFrom(table, key);
      string text = string.Format("SELECT * FROM {0} WHERE {1} = {2}", (object) "WF_NotificationRecipients", (object) "NotificationID", (object) SQL.Encode((object) notificationID.ToString()));
      dbQueryBuilder.AppendLine(text);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null)
        return (ServiceWorkflowNotification) null;
      ServiceWorkflowNotification notification = this.ConvertDataRowToNotification(dataSet.Tables, 0);
      if (notification == null)
        return (ServiceWorkflowNotification) null;
      notification.Recipients = this.GetRecipients(dataSet.Tables, 1);
      return notification;
    }

    private ServiceWorkflowNotification ConvertDataRowToNotification(
      DataTableCollection tables,
      int index)
    {
      if (tables == null || tables.Count <= index)
        return (ServiceWorkflowNotification) null;
      DataTable table = tables[index];
      return table.Rows == null || table.Rows.Count <= 0 ? (ServiceWorkflowNotification) null : this.ConvertDataRowToNotification(table.Rows[0]);
    }

    private ServiceWorkflowNotification ConvertDataRowToNotification(DataRow row)
    {
      if (row == null)
        return (ServiceWorkflowNotification) null;
      return new ServiceWorkflowNotification()
      {
        NotificationID = (Guid) row["NotificationID"],
        RuleID = (int) row["RuleID"],
        NotificationType = (WorkflowNotificationType) Convert.ToInt32(row["NotificationType"]),
        Subject = row["Subject"] == DBNull.Value ? string.Empty : (string) row["Subject"],
        Text = row["Text"] == DBNull.Value ? string.Empty : (string) row["Text"],
        LastModifiedByUserId = row["LastModifiedByUserId"] == DBNull.Value ? string.Empty : (string) row["LastModifiedByUserId"],
        LastModified = (DateTime) row["LastModified"]
      };
    }

    public void CreateServiceWorkflowNotification(
      ServiceWorkflowNotification ServiceWorkflowNotification)
    {
      if (ServiceWorkflowNotification.NotificationID == Guid.Empty)
        ServiceWorkflowNotification.NotificationID = Guid.NewGuid();
      EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("WF_Notifications");
      DbValueList dbValueList = this.GetDBValueList(ServiceWorkflowNotification, true);
      sql.InsertInto(table, dbValueList, true, false);
      this.UpdateRecipients(ServiceWorkflowNotification, sql);
      sql.ExecuteNonQuery();
    }

    public void UpdateServiceWorkflowNotification(
      ServiceWorkflowNotification ServiceWorkflowNotification)
    {
      EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("WF_Notifications");
      DbValue key = new DbValue("NotificationID", (object) ServiceWorkflowNotification.NotificationID.ToString());
      DbValueList dbValueList = this.GetDBValueList(ServiceWorkflowNotification);
      sql.Update(table, dbValueList, key);
      this.DeleteRecipients(ServiceWorkflowNotification.NotificationID, sql);
      this.UpdateRecipients(ServiceWorkflowNotification, sql);
      sql.ExecuteNonQuery();
    }

    public void DeleteServiceWorkflowNotification(Guid notificationID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("WF_Notifications");
      DbValue key = new DbValue("NotificationID", (object) notificationID.ToString());
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public void DeleteServiceWorkflowNotificationByRuleID(int ruleID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("WF_Notifications");
      DbValue key = new DbValue("RuleID", (object) ruleID);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private DbValueList GetDBValueList(ServiceWorkflowNotification Notification, bool forCreate = false)
    {
      DbValueList dbValueList = new DbValueList();
      if (forCreate)
      {
        dbValueList.Add("NotificationID", (object) Notification.NotificationID.ToString());
        dbValueList.Add("RuleID", (object) Notification.RuleID.ToString());
      }
      dbValueList.Add("NotificationType", (object) (int) Notification.NotificationType);
      dbValueList.Add("Subject", (object) Notification.Subject);
      dbValueList.Add("Text", (object) Notification.Text);
      dbValueList.Add("LastModifiedByUserId", (object) Notification.LastModifiedByUserId);
      dbValueList.Add("LastModified", (object) Notification.LastModified);
      return dbValueList;
    }

    private void DeleteRecipients(Guid NotificationID, EllieMae.EMLite.Server.DbQueryBuilder sql)
    {
      string text = string.Format("DELETE from {0} where {1} = {2}", (object) "WF_NotificationRecipients", (object) nameof (NotificationID), (object) SQL.Encode((object) NotificationID.ToString()));
      sql.AppendLine(text);
    }

    public void UpdateRecipients(
      ServiceWorkflowNotification ServiceWorkflowNotification,
      EllieMae.EMLite.Server.DbQueryBuilder sql)
    {
      if (ServiceWorkflowNotification.Recipients == null)
        return;
      string strSql;
      ServiceWorkflowNotification.Recipients.ForEach((Action<WorkflowNotificationRecipient>) (recipient =>
      {
        if (recipient.Type == WorkflowNotificationRecipientType.User)
          strSql = string.Format("Insert into {0} ({1}, {2}, {3}) values ({4}, {5}, {6})", (object) "WF_NotificationRecipients", (object) "NotificationID", (object) "RecipientType", (object) "RecipientUserID", (object) SQL.Encode((object) ServiceWorkflowNotification.NotificationID.ToString()), (object) SQL.Encode((object) (int) recipient.Type), (object) SQL.EncodeString(recipient.UserID));
        else
          strSql = string.Format("Insert into {0} ({1}, {2}, {3}) values ({4}, {5}, {6})", (object) "WF_NotificationRecipients", (object) "NotificationID", (object) "RecipientType", (object) "RecipientRoleID", (object) SQL.Encode((object) ServiceWorkflowNotification.NotificationID.ToString()), (object) SQL.Encode((object) (int) recipient.Type), (object) SQL.Encode((object) recipient.RoleID));
        sql.AppendLine(strSql);
      }));
    }

    public List<WorkflowNotificationRecipient> GetRecipients(DataTableCollection tables, int index)
    {
      if (tables == null || tables.Count <= index)
        return (List<WorkflowNotificationRecipient>) null;
      DataTable table = tables[index];
      if (table.Rows == null || table.Rows.Count <= 0)
        return (List<WorkflowNotificationRecipient>) null;
      List<WorkflowNotificationRecipient> recipients = new List<WorkflowNotificationRecipient>();
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
      {
        WorkflowNotificationRecipient notificationRecipient = new WorkflowNotificationRecipient();
        notificationRecipient.Type = (WorkflowNotificationRecipientType) Convert.ToInt32(row["RecipientType"]);
        if (notificationRecipient.Type == WorkflowNotificationRecipientType.Role)
          notificationRecipient.RoleID = row["RecipientRoleID"] == DBNull.Value ? 0 : Convert.ToInt32(row["RecipientRoleID"]);
        else
          notificationRecipient.UserID = row["RecipientUserID"] == DBNull.Value ? string.Empty : (string) row["RecipientUserID"];
        recipients.Add(notificationRecipient);
      }
      return recipients;
    }
  }
}

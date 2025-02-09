// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.AlertConfigAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.ReportingDbUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class AlertConfigAccessor
  {
    private const string className = "AlertConfigAccessor�";
    private const int MinCustomAlertID = 1000;

    public static AlertConfig GetAlertConfig(int alertId)
    {
      foreach (AlertConfig alertConfig in AlertConfigAccessor.GetAlertConfigList())
      {
        if (alertConfig.AlertID == alertId)
          return alertConfig;
      }
      return (AlertConfig) null;
    }

    public static AlertConfig GetAlertConfigByName(string alertName)
    {
      foreach (AlertConfig alertConfig in AlertConfigAccessor.GetAlertConfigList())
      {
        if (string.Compare(alertName, alertConfig.Definition.Name, true) == 0)
          return alertConfig;
      }
      return (AlertConfig) null;
    }

    public static AlertConfig[] GetAlertConfigList()
    {
      ClientContext context = ClientContext.GetCurrent();
      string connStr = DbQueryBuilder.getConnectionString(DBReadReplicaFeature.Pipeline);
      return context.Cache.Get<AlertConfig[]>(nameof (AlertConfigAccessor), (Func<AlertConfig[]>) (() => AlertConfigAccessorB.GetAlertConfigListFromDB(connStr, context.Settings.DbServerType, AlertConfigAccessor.getDataCompletionFieldsFilters())), CacheSetting.Low);
    }

    public static int UpdateAlertConfig(AlertConfig alert)
    {
      int alertId = -1;
      ClientContext context = ClientContext.GetCurrent();
      context.Cache.Put<AlertConfig[]>(nameof (AlertConfigAccessor), (Action) (() =>
      {
        string str = "null";
        if (alert.Definition.NotificationType != AlertNotificationType.None)
          str = alert.NotificationEnabled ? "1" : "0";
        DbTableInfo table1 = DbAccessManager.GetTable(AlertConfigAccessorB.TableName_AlertConfig);
        DbTableInfo table2 = DbAccessManager.GetTable(AlertConfigAccessorB.TableName_AlertMilestoneConfig);
        DbTableInfo table3 = DbAccessManager.GetTable(AlertConfigAccessorB.TableName_AlertTriggers);
        DbTableInfo table4 = DbAccessManager.GetTable(AlertConfigAccessorB.TableName_AlertNotificationUsers);
        DbTableInfo table5 = DbAccessManager.GetTable(AlertConfigAccessorB.TableName_AlertNotificationRoles);
        DbTableInfo table6 = DbAccessManager.GetTable(AlertConfigAccessorB.TableName_AlertCustomSetup);
        DbQueryBuilder sql = new DbQueryBuilder();
        sql.Declare("@alertId", "int");
        DbValue key = new DbValue("AlertID", (object) "@alertId", (IDbEncoder) DbEncoding.None);
        if (alert.Definition.Category == AlertCategory.Custom && alert.AlertID < 0)
        {
          sql.AppendLine("select @alertId = max(alertID) + 1 from AlertConfig");
          sql.AppendLine("if @alertId < " + (object) 1000 + " select @alertId = " + (object) 1000);
        }
        else
        {
          sql.SelectVar("@alertId", (object) alert.AlertID);
          sql.DeleteFrom(table1, key);
        }
        sql.InsertInto(table1, new DbValueList()
        {
          key,
          {
            "name",
            (object) alert.Definition.Name
          },
          {
            "daysBefore",
            (object) alert.DaysBefore
          },
          {
            "displayOnPipeline",
            (object) alert.AlertEnabled,
            (IDbEncoder) DbEncoding.Flag
          },
          {
            "notificationEnabled",
            (object) str,
            (IDbEncoder) DbEncoding.None
          },
          {
            "message",
            (object) alert.Message
          }
        }, true, false);
        foreach (string milestoneGuid in alert.MilestoneGuidList)
          sql.InsertInto(table2, new DbValueList()
          {
            key,
            {
              "GUID",
              (object) milestoneGuid
            }
          }, true, false);
        foreach (string triggerField in alert.TriggerFieldList)
          sql.InsertInto(table3, new DbValueList()
          {
            key,
            {
              "fieldID",
              (object) triggerField
            }
          }, true, false);
        if (alert.Definition.NotificationType == AlertNotificationType.Configurable)
        {
          foreach (int notificationRole in alert.NotificationRoleList)
            sql.InsertInto(table5, new DbValueList()
            {
              key,
              {
                "roleID",
                (object) notificationRole
              }
            }, true, false);
          foreach (string notificationUser in alert.NotificationUserList)
            sql.InsertInto(table4, new DbValueList()
            {
              key,
              {
                "userID",
                (object) notificationUser
              }
            }, true, false);
        }
        if (alert.Definition.Category == AlertCategory.Custom)
        {
          CustomAlert definition = (CustomAlert) alert.Definition;
          sql.InsertInto(table6, new DbValueList()
          {
            key,
            {
              "alertGuid",
              (object) definition.Guid
            },
            {
              "dateAdjustment",
              (object) definition.DateAdjustment
            },
            {
              "adjustmentDayType",
              (object) (int) definition.AdjustmentDayType
            },
            {
              "allowToClear",
              (object) definition.AllowToClear,
              (IDbEncoder) DbEncoding.Flag
            },
            {
              "conditionXml",
              (object) definition.ConditionXml,
              (IDbEncoder) DbEncoding.EmptyStringAsNull
            }
          }, true, false);
        }
        sql.Select("@alertId");
        if (alert is AlertConfigWithDataCompletionFields)
          AlertConfigAccessor.getDataCompletionFieldsQuery(alert as AlertConfigWithDataCompletionFields, sql);
        alertId = (int) sql.ExecuteScalar(DbTransactionType.Serialized);
      }), (Func<AlertConfig[]>) (() => AlertConfigAccessorB.GetAlertConfigListFromDB(context.Settings.ConnectionString, context.Settings.DbServerType, AlertConfigAccessor.getDataCompletionFieldsFilters())), CacheSetting.Low);
      return alertId;
    }

    public static void DeleteAlertConfig(int alertId)
    {
      if (alertId < 1000)
        throw new ArgumentException("Only custom alerts can be deleted");
      ClientContext context = ClientContext.GetCurrent();
      context.Cache.Put<AlertConfig[]>(nameof (AlertConfigAccessor), (Action) (() =>
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        DbTableInfo table1 = DbAccessManager.GetTable("LoanAlerts");
        dbQueryBuilder.DeleteFrom(table1, new DbValue("AlertType", (object) alertId));
        DbTableInfo table2 = DbAccessManager.GetTable("AlertConfig");
        dbQueryBuilder.DeleteFrom(table2, new DbValue("alertID", (object) alertId));
        dbQueryBuilder.ExecuteNonQuery(TimeSpan.FromMinutes(5.0), DbTransactionType.Default);
      }), (Func<AlertConfig[]>) (() => AlertConfigAccessorB.GetAlertConfigListFromDB(context.Settings.ConnectionString, context.Settings.DbServerType, AlertConfigAccessor.getDataCompletionFieldsFilters())), CacheSetting.Low);
    }

    public static void AddMilestoneToAllAlerts(string milestoneID)
    {
      ClientContext context = ClientContext.GetCurrent();
      context.Cache.Put<AlertConfig[]>(nameof (AlertConfigAccessor), (Action) (() =>
      {
        context.Cache.Remove(nameof (AlertConfigAccessor));
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("insert into [AlertMilestoneConfig] select alertID, " + SQL.Encode((object) milestoneID) + " from AlertConfig");
        dbQueryBuilder.ExecuteNonQuery();
      }), (Func<AlertConfig[]>) (() => AlertConfigAccessorB.GetAlertConfigListFromDB(context.Settings.ConnectionString, context.Settings.DbServerType, AlertConfigAccessor.getDataCompletionFieldsFilters())), CacheSetting.Low);
    }

    public static bool AddMsToMsFinishedAlertList(string milestoneID)
    {
      ClientContext context = ClientContext.GetCurrent();
      context.Cache.Put<AlertConfig[]>(nameof (AlertConfigAccessor), (Action) (() =>
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("if not exists (select * from " + AlertConfigAccessorB.TableName_AlertMilestoneConfig + " where [alertID] = 0 and [GUID] = " + SQL.Encode((object) milestoneID) + ")");
        dbQueryBuilder.AppendLine("begin");
        dbQueryBuilder.AppendLine("    insert into " + AlertConfigAccessorB.TableName_AlertMilestoneConfig + "([alertID], [GUID])        Values (" + (object) 0 + ", " + SQL.Encode((object) milestoneID) + ")");
        dbQueryBuilder.AppendLine("end");
        dbQueryBuilder.ExecuteNonQuery();
      }), (Func<AlertConfig[]>) (() => AlertConfigAccessorB.GetAlertConfigListFromDB(context.Settings.ConnectionString, context.Settings.DbServerType, AlertConfigAccessor.getDataCompletionFieldsFilters())), CacheSetting.Low);
      return true;
    }

    public static bool DeleteMsFromMsFinishedAlertList(string milestoneID)
    {
      ClientContext context = ClientContext.GetCurrent();
      context.Cache.Put<AlertConfig[]>(nameof (AlertConfigAccessor), (Action) (() =>
      {
        context.Cache.Remove(nameof (AlertConfigAccessor));
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("delete from " + AlertConfigAccessorB.TableName_AlertMilestoneConfig + " where [alertID] = 0 and [GUID] = " + SQL.Encode((object) milestoneID));
        dbQueryBuilder.ExecuteNonQuery();
      }), (Func<AlertConfig[]>) (() => AlertConfigAccessorB.GetAlertConfigListFromDB(context.Settings.ConnectionString, context.Settings.DbServerType, AlertConfigAccessor.getDataCompletionFieldsFilters())), CacheSetting.Low);
      return true;
    }

    public static PipelineInfo.Alert[] GetLoanAlerts(string guid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select la.* from LoanAlerts la ");
      dbQueryBuilder.AppendLine("    inner join LoanSummary ls on ls.XRefId = la.LoanXRefId");
      dbQueryBuilder.AppendLine("where ls.Guid = " + SQL.Encode((object) guid));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      List<PipelineInfo.Alert> alertList = new List<PipelineInfo.Alert>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        alertList.Add(new PipelineInfo.Alert(SQL.DecodeInt(dataRow["AlertType"]), string.Concat(dataRow["Event"]), string.Concat(dataRow["Status"]), SQL.DecodeDateTime(dataRow["AlertDate"]), SQL.DecodeInt(dataRow["DisplayStatus"]), SQL.DecodeString(dataRow["UserID"]), SQL.DecodeInt(dataRow["GroupID"], -1), SQL.DecodeDateTime(dataRow["SnoozeStartDTTM"]), SQL.DecodeInt(dataRow["SnoozeDuration"]), (string) SQL.Decode(dataRow["UniqueID"]), (string) null)
        {
          LoanAlertID = string.Concat(dataRow["LoanAlertId"])
        });
      return alertList.ToArray();
    }

    public static PipelineInfo.Alert[] GetLoanAlertsByAlertId(int alertId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select la.*,ls.Guid from LoanAlerts la ");
      dbQueryBuilder.AppendLine("    inner join LoanSummary ls on ls.XRefId = la.LoanXRefId");
      dbQueryBuilder.AppendLine("where la.AlertType = " + SQL.Encode((object) alertId));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      List<PipelineInfo.Alert> alertList = new List<PipelineInfo.Alert>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        alertList.Add(new PipelineInfo.Alert(SQL.DecodeInt(dataRow["AlertType"]), string.Concat(dataRow["Event"]), string.Concat(dataRow["Status"]), SQL.DecodeDateTime(dataRow["AlertDate"]), SQL.DecodeInt(dataRow["DisplayStatus"]), SQL.DecodeString(dataRow["UserID"]), SQL.DecodeInt(dataRow["GroupID"], -1), SQL.DecodeDateTime(dataRow["SnoozeStartDTTM"]), SQL.DecodeInt(dataRow["SnoozeDuration"]), (string) SQL.Decode(dataRow["UniqueID"]), (string) null)
        {
          LoanAlertID = string.Concat(dataRow["LoanAlertId"]),
          LoanGuid = SQL.DecodeString(dataRow["Guid"]).Replace("}", "").Replace("{", "")
        });
      return alertList.ToArray();
    }

    public static void UpdateLoanAlerts(string guid, PipelineInfo.Alert[] loanAlerts)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Declare("@alertId", "bigint");
      dbQueryBuilder.Declare("@xrefId", "int");
      dbQueryBuilder.AppendLine("select @xrefId = XRefID from LoanSummary where Guid = " + SQL.Encode((object) guid));
      DbTableInfo table = DbAccessManager.GetTable("LoanAlerts");
      foreach (PipelineInfo.Alert loanAlert in loanAlerts)
      {
        dbQueryBuilder.AppendLine("select @alertId = la.LoanAlertId from LoanAlerts la");
        dbQueryBuilder.AppendLine("    where la.LoanXRefId = @xrefId and la.AlertType = " + (object) loanAlert.AlertID);
        dbQueryBuilder.AppendLine("       and la.[Event] = " + SQL.Encode((object) (loanAlert.Event ?? "")));
        if (loanAlert.AlertTargetID == null)
          dbQueryBuilder.AppendLine(" and la.[UniqueID] is NULL");
        else
          dbQueryBuilder.AppendLine(" and la.[UniqueID] = " + SQL.Encode((object) loanAlert.AlertTargetID));
        if ((loanAlert.UserID ?? "") == "")
          dbQueryBuilder.AppendLine(" and la.UserID is NULL");
        else
          dbQueryBuilder.AppendLine(" and la.UserID = " + SQL.Encode((object) loanAlert.UserID));
        DbValueList values = new DbValueList();
        values.Add("DisplayStatus", (object) loanAlert.DisplayStatus);
        values.Add("SnoozeDuration", (object) loanAlert.SnoozeDuration);
        if (loanAlert.SnoozeStartDTTM != DateTime.MinValue)
        {
          values.Add("SnoozeStartDTTM", (object) loanAlert.SnoozeStartDTTM);
          values.Add("ActivationDTTM", (object) loanAlert.SnoozeStartDTTM.AddMinutes((double) loanAlert.SnoozeDuration));
        }
        else
        {
          values.Add("SnoozeStartDTTM", (object) null);
          values.Add("ActivationDTTM", (object) null);
        }
        DbValue key = new DbValue("LoanAlertId", (object) "@alertId", (IDbEncoder) DbEncoding.None);
        dbQueryBuilder.Update(table, values, key);
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static void getDataCompletionFieldsQuery(
      AlertConfigWithDataCompletionFields alertConfig,
      DbQueryBuilder sql)
    {
      AlertDataCompletionFieldCollection completionActiveFields = alertConfig.StandardDataCompletionActiveFields;
      AlertDataCompletionFieldCollection completionFields = alertConfig.CustomDataCompletionFields;
      if (!completionActiveFields.Any<AlertDataCompletionField>() && !completionFields.Any<AlertDataCompletionField>())
        return;
      List<string> values = new List<string>();
      foreach (AlertDataCompletionField field in (List<AlertDataCompletionField>) completionActiveFields)
        values.Add(AlertConfigAccessor.getDataCompletionFieldValuesQuery(alertConfig.AlertID, field));
      foreach (AlertDataCompletionField field in (List<AlertDataCompletionField>) completionFields)
        values.Add(AlertConfigAccessor.getDataCompletionFieldValuesQuery(alertConfig.AlertID, field));
      sql.AppendLine("DECLARE @source AS typ_AlertDataCompletionFields");
      sql.AppendLine("INSERT INTO  @source ([alertID], [fieldID], [fieldType], [readOnly], [excluded])");
      sql.AppendLine("SELECT [alertID], [fieldID], [fieldType], [readOnly], [excluded] FROM ( VALUES");
      sql.AppendLine(string.Join(", ", (IEnumerable<string>) values));
      sql.AppendLine(") AS r ([alertID], [fieldID], [fieldType], [readOnly], [excluded])");
      sql.AppendLine("EXEC [ManageAlertDataCompletionFields] @alertId, @source");
    }

    private static string getDataCompletionFieldValuesQuery(
      int alertID,
      AlertDataCompletionField field)
    {
      return string.Format("({0},{1},{2},{3},{4})", (object) alertID, (object) SQL.EncodeString(field.FieldID), (object) (int) field.FieldType, (object) SQL.EncodeFlag(field.ReadOnly), (object) SQL.EncodeFlag(field.Excluded));
    }

    public static bool IsFulfillmentEnabled() => AlertConfigAccessor.isCompanySettingEnabled(101);

    private static bool isCompanySettingEnabled(int filter)
    {
      bool flag = false;
      foreach (Tuple<string, string, string> tuple in AlertConfigAccessorB.GetCompanySettingsFilter(filter))
      {
        flag = string.Compare(Company.GetCompanySetting(tuple.Item1, tuple.Item2), tuple.Item3, StringComparison.InvariantCultureIgnoreCase) == 0;
        if (!flag)
          break;
      }
      return flag;
    }

    private static Dictionary<int, bool> getDataCompletionFieldsFilters()
    {
      return new Dictionary<int, bool>()
      {
        {
          101,
          AlertConfigAccessor.IsFulfillmentEnabled()
        }
      };
    }
  }
}

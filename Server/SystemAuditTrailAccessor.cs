// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.SystemAuditTrailAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class SystemAuditTrailAccessor
  {
    private const string className = "SystemAuditTrailAccessor�";

    [PgReady]
    private static int[] getRecordIDRange(string tableName, DateTime startTime, DateTime endTime)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select min(id) from [" + tableName + "] where dttmstamp >= " + SQL.Encode((object) startTime) + ";");
        pgDbQueryBuilder.AppendLine("select max(id) from [" + tableName + "] where dttmstamp < " + SQL.Encode((object) endTime) + ";");
        DataSet dataSet = pgDbQueryBuilder.ExecuteSetQuery(TimeSpan.FromMinutes(10.0), DbTransactionType.Snapshot);
        return new int[2]
        {
          SQL.DecodeInt(dataSet.Tables[0].Rows[0][0], -1),
          SQL.DecodeInt(dataSet.Tables[1].Rows[0][0], -1)
        };
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select min(id) from [" + tableName + "] where dttmstamp >= " + SQL.Encode((object) startTime));
      dbQueryBuilder.AppendLine("select max(id) from [" + tableName + "] where dttmstamp < " + SQL.Encode((object) endTime));
      DataSet dataSet1 = dbQueryBuilder.ExecuteSetQuery(TimeSpan.FromMinutes(10.0), DbTransactionType.Snapshot);
      return new int[2]
      {
        SQL.DecodeInt(dataSet1.Tables[0].Rows[0][0], -1),
        SQL.DecodeInt(dataSet1.Tables[1].Rows[0][0], -1)
      };
    }

    [PgReady]
    public static void InsertAuditRecord(SystemAuditRecord record)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        try
        {
          DbTableInfo table = DbAccessManager.GetTable(SystemAuditTrailAccessor.getTableName(record.ActionType));
          DbValueList dbValueList = new DbValueList();
          dbValueList.Add("UserID", (object) record.UserID);
          dbValueList.Add("ActionType", (object) (int) record.ActionType);
          dbValueList.Add("UserFullName", (object) record.UserFullName);
          if (record.DateTime != DateTime.MinValue)
            dbValueList.Add("DTTMStamp", (object) record.DateTime, (IDbEncoder) DbEncoding.DateTime);
          else
            dbValueList.Add("DTTMStamp", (object) DateTime.Now, (IDbEncoder) DbEncoding.DateTime);
          if (ActionType.LoanModified == record.ActionType)
          {
            dbValueList.Add("AutoSaveDate", (object) ((LoanFileAuditRecord) record).AutoSaveDateTime, (IDbEncoder) DbEncoding.DateTime);
            dbValueList.Add("LoanFileVersionNumber", (object) ((LoanFileAuditRecord) record).FileVersionNumber.ToString(), (IDbEncoder) DbEncoding.MinusOneAsNull);
          }
          dbValueList.Add("ObjectType", (object) (int) record.ObjectType);
          SystemAuditTrailAccessor.insertIndividualFields(dbValueList, record);
          pgDbQueryBuilder.InsertInto(table, dbValueList, true, false);
          pgDbQueryBuilder.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (SystemAuditTrailAccessor), ex);
        }
      }
      else
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        try
        {
          DbTableInfo table = DbAccessManager.GetTable(SystemAuditTrailAccessor.getTableName(record.ActionType));
          DbValueList dbValueList = new DbValueList();
          dbValueList.Add("UserID", (object) record.UserID);
          dbValueList.Add("ActionType", (object) (int) record.ActionType);
          dbValueList.Add("UserFullName", (object) record.UserFullName);
          if (record.DateTime != DateTime.MinValue)
            dbValueList.Add("DTTMStamp", (object) record.DateTime, (IDbEncoder) DbEncoding.DateTime);
          else
            dbValueList.Add("DTTMStamp", (object) DateTime.Now, (IDbEncoder) DbEncoding.DateTime);
          if (ActionType.LoanModified == record.ActionType)
          {
            dbValueList.Add("AutoSaveDate", (object) ((LoanFileAuditRecord) record).AutoSaveDateTime, (IDbEncoder) DbEncoding.DateTime);
            dbValueList.Add("LoanFileVersionNumber", (object) ((LoanFileAuditRecord) record).FileVersionNumber.ToString(), (IDbEncoder) DbEncoding.MinusOneAsNull);
          }
          dbValueList.Add("ObjectType", (object) (int) record.ObjectType);
          SystemAuditTrailAccessor.insertIndividualFields(dbValueList, record);
          dbQueryBuilder.InsertInto(table, dbValueList, true, false);
          dbQueryBuilder.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (SystemAuditTrailAccessor), ex);
        }
      }
    }

    public static void InsertAuditRecords(SystemAuditRecord[] records)
    {
      ClientContext.GetCurrent();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      try
      {
        DbTableInfo table = DbAccessManager.GetTable(SystemAuditTrailAccessor.getTableName(records[0].ActionType));
        List<DbValueList> values = new List<DbValueList>();
        foreach (SystemAuditRecord record in records)
        {
          DbValueList detailsFields = new DbValueList();
          detailsFields.Add("UserID", (object) record.UserID);
          detailsFields.Add("ActionType", (object) (int) record.ActionType);
          detailsFields.Add("UserFullName", (object) record.UserFullName);
          if (record.DateTime != DateTime.MinValue)
            detailsFields.Add("DTTMStamp", (object) record.DateTime, (IDbEncoder) DbEncoding.DateTime);
          else
            detailsFields.Add("DTTMStamp", (object) DateTime.Now, (IDbEncoder) DbEncoding.DateTime);
          if (ActionType.LoanModified == record.ActionType)
          {
            detailsFields.Add("AutoSaveDate", (object) ((LoanFileAuditRecord) record).AutoSaveDateTime, (IDbEncoder) DbEncoding.DateTime);
            detailsFields.Add("LoanFileVersionNumber", (object) ((LoanFileAuditRecord) record).FileVersionNumber.ToString(), (IDbEncoder) DbEncoding.MinusOneAsNull);
          }
          detailsFields.Add("ObjectType", (object) (int) record.ObjectType);
          SystemAuditTrailAccessor.insertIndividualFields(detailsFields, record);
          values.Add(detailsFields);
        }
        DbVersion dbVersion;
        using (DbAccessManager dbAccessManager = new DbAccessManager())
          dbVersion = dbAccessManager.GetDbVersion();
        dbQueryBuilder.InsertInto(table, values, dbVersion);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SystemAuditTrailAccessor), ex);
      }
    }

    private static void insertIndividualFields(DbValueList detailsFields, SystemAuditRecord record)
    {
      switch (record.ActionType)
      {
        case ActionType.LoanCreated:
        case ActionType.LoanImported:
        case ActionType.LoanModified:
        case ActionType.LoanDeleted:
        case ActionType.LoanPermanentDeleted:
        case ActionType.LoanRestored:
        case ActionType.LoanMoved:
        case ActionType.AttachmentMigrated:
        case ActionType.LoanExternalFieldsDeleted:
          detailsFields.Add("LoanGuid", (object) ((LoanFileAuditRecord) record).LoanGuid);
          detailsFields.Add("LoanNumber", (object) ((LoanFileAuditRecord) record).LoanNumber);
          detailsFields.Add("LoanFolder", (object) ((LoanFileAuditRecord) record).LoanFolder);
          detailsFields.Add("BorrowerLastName", (object) ((LoanFileAuditRecord) record).BorrowerLastName);
          detailsFields.Add("BorrowerFirstName", (object) ((LoanFileAuditRecord) record).BorrowerFirstName);
          detailsFields.Add("Address", (object) ((LoanFileAuditRecord) record).Address);
          detailsFields.Add("FileSource", (object) ((LoanFileAuditRecord) record).LoanFileSource);
          detailsFields.Add("AppName", (object) ((LoanFileAuditRecord) record).AppName);
          detailsFields.Add("ImpersonatedUserID", (object) record.ImpersonatedUserID);
          detailsFields.Add("ImpersonatedUserName", (object) record.ImpersonatedUserFullName);
          break;
        case ActionType.UserCreated:
        case ActionType.UserModified:
        case ActionType.UserDeleted:
          detailsFields.Add("UserAccountID", (object) ((UserProfileAuditRecord) record).UserAccountID);
          detailsFields.Add("UserAccountName", (object) ((UserProfileAuditRecord) record).UserAccountName);
          break;
        case ActionType.UserPasswordChangeForced:
          detailsFields.Add("UserAccountID", (object) ((UserPwdChangeForcedAuditRecord) record).UserAccountID);
          detailsFields.Add("UserAccountName", (object) ((UserPwdChangeForcedAuditRecord) record).UserAccountName);
          break;
        case ActionType.UserPasswordChanged:
          detailsFields.Add("PriorStatus", (object) ((UserPwdChangeAuditRecord) record).PriorStatus);
          detailsFields.Add("UserAccountID", (object) ((UserPwdChangeAuditRecord) record).UserAccountID);
          detailsFields.Add("UserAccountName", (object) ((UserPwdChangeAuditRecord) record).UserAccountName);
          break;
        case ActionType.UserLogin:
        case ActionType.SSOUserLogin:
          detailsFields.Add("UserAccountID", (object) ((UserLoginAuditRecord) record).LoginUserID);
          detailsFields.Add("UserAccountName", (object) ((UserLoginAuditRecord) record).LoginUserName);
          detailsFields.Add("IPAddress", (object) ((UserLoginAuditRecord) record).IPAddress);
          detailsFields.Add("MachineName", (object) ((UserLoginAuditRecord) record).MachineName);
          break;
        case ActionType.UserLogout:
          detailsFields.Add("LogoutUserID", (object) ((UserLogoutAuditRecord) record).LogoutUserID);
          detailsFields.Add("LogoutUserName", (object) ((UserLogoutAuditRecord) record).LogoutUserName);
          detailsFields.Add("Duration", (object) ((UserLogoutAuditRecord) record).Duration);
          detailsFields.Add("LogoutReason", (object) ((UserLogoutAuditRecord) record).LogoutReason);
          break;
        case ActionType.TemplateCreated:
        case ActionType.TemplateModified:
        case ActionType.TemplateDeleted:
          detailsFields.Add("TemplateName", (object) ((TemplateAuditRecord) record).TemplateName);
          detailsFields.Add("TemplatePath", (object) ((TemplateAuditRecord) record).TemplatePath);
          break;
        case ActionType.PersonaCreated:
        case ActionType.PersonaModified:
        case ActionType.PersonaDeleted:
          detailsFields.Add("PersonaID", (object) ((PersonaAuditRecord) record).PersonaID);
          detailsFields.Add("PersonaName", (object) ((PersonaAuditRecord) record).PersonaName);
          break;
        case ActionType.UserGroupCreated:
        case ActionType.UserGroupModified:
        case ActionType.UserGroupDeleted:
          detailsFields.Add("UserGroupID", (object) ((UserGroupAuditRecord) record).UserGroupID);
          detailsFields.Add("UserGroupName", (object) ((UserGroupAuditRecord) record).UserGroupName);
          break;
        case ActionType.BusinessContactGroupCreated:
        case ActionType.BusinessContactGroupModified:
        case ActionType.BusinessContactGroupDeleted:
          detailsFields.Add("ContactGroupID", (object) ((BizContactGroupAuditRecord) record).BizContactGroupID);
          detailsFields.Add("ContactGroupName", (object) ((BizContactGroupAuditRecord) record).BizContactGroupName);
          break;
        case ActionType.BusinessRuleCreated:
        case ActionType.BusinessRuleModified:
        case ActionType.BusinessRuleDeleted:
        case ActionType.BusinessRuleActivated:
        case ActionType.BusinessRuleDeactivated:
          detailsFields.Add("RuleID", (object) ((BizRuleAuditRecord) record).RuleID);
          detailsFields.Add("RuleName", (object) ((BizRuleAuditRecord) record).RuleName);
          break;
        case ActionType.FailedLoginUserNotFound:
        case ActionType.FailedLoginPasswordMismatch:
        case ActionType.FailedLoginUserDisabled:
        case ActionType.FailedLoginLoginDisabled:
        case ActionType.FailedLoginUserLocked:
        case ActionType.FailedLoginPersonaNotFound:
        case ActionType.IPBlocked:
        case ActionType.SSOFailedUserLogin:
          detailsFields.Add("IPAddress", (object) ((FailedUserLoginAuditRecord) record).IPAddress);
          break;
        case ActionType.FeeRuleCreated:
        case ActionType.FeeRuleModified:
        case ActionType.FeeRuleDeleted:
        case ActionType.FeeRuleActivated:
        case ActionType.FeeRuleDeactivated:
        case ActionType.FieldRuleCreated:
        case ActionType.FieldRuleModified:
        case ActionType.FieldRuleDeleted:
        case ActionType.FieldRuleActivated:
        case ActionType.FieldRuleDeactivated:
        case ActionType.FeeScenarioCreated:
        case ActionType.FeeScenarioModified:
        case ActionType.FeeScenarioDeleted:
        case ActionType.FeeScenarioActivated:
        case ActionType.FeeScenarioDeactivated:
        case ActionType.FieldScenarioCreated:
        case ActionType.FieldScenarioModified:
        case ActionType.FieldScenarioDeleted:
        case ActionType.FieldScenarioActivated:
        case ActionType.FieldScenarioDeactivated:
        case ActionType.DataTableCreated:
        case ActionType.DataTableModified:
        case ActionType.DataTableDeleted:
        case ActionType.DataPopulationTimingModified:
          detailsFields.Add("OrderID", (object) ((DDMAuditRecord) record).OrderID);
          detailsFields.Add("RuleName", (object) ((DDMAuditRecord) record).RuleName);
          detailsFields.Add("ScenarioName", (object) ((DDMAuditRecord) record).ScenarioName);
          detailsFields.Add("AdditionalInfo", (object) ((DDMAuditRecord) record).AddtionalInfo);
          break;
        case ActionType.HMDACreated:
        case ActionType.HMDAModified:
        case ActionType.HMDADeleted:
          detailsFields.Add("ProfileID", (object) ((HMDAAuditRecord) record).ProfileID);
          detailsFields.Add("ProfileName", (object) ((HMDAAuditRecord) record).ProfileName);
          break;
        case ActionType.ServerSettingsChanged:
          detailsFields.Add("Category", (object) (int) ((ServerSettingsAuditRecord) record).Category);
          detailsFields.Add("Setting", (object) (int) ((ServerSettingsAuditRecord) record).SettingName);
          detailsFields.Add("OldSettingValue", (object) ((ServerSettingsAuditRecord) record).OldSettingValue);
          detailsFields.Add("NewSettingValue", (object) ((ServerSettingsAuditRecord) record).NewSettingValue);
          break;
      }
    }

    private static string getTableName(ActionType actionType)
    {
      switch (actionType)
      {
        case ActionType.LoanCreated:
        case ActionType.LoanImported:
        case ActionType.LoanModified:
        case ActionType.LoanDeleted:
        case ActionType.LoanPermanentDeleted:
        case ActionType.LoanRestored:
        case ActionType.LoanMoved:
        case ActionType.AttachmentMigrated:
        case ActionType.LoanExternalFieldsDeleted:
          return "SysAT_Loan";
        case ActionType.UserCreated:
        case ActionType.UserModified:
        case ActionType.UserDeleted:
          return "SysAT_UserProfile";
        case ActionType.UserPasswordChangeForced:
          return "SysAT_UserPwdChangedForced";
        case ActionType.UserPasswordChanged:
          return "SysAT_UserPwdChanged";
        case ActionType.UserLogin:
        case ActionType.SSOUserLogin:
          return "SysAT_UserLogin";
        case ActionType.UserLogout:
          return "SysAT_UserLogout";
        case ActionType.TemplateCreated:
        case ActionType.TemplateModified:
        case ActionType.TemplateDeleted:
          return "SysAT_Template";
        case ActionType.PersonaCreated:
        case ActionType.PersonaModified:
        case ActionType.PersonaDeleted:
          return "SysAT_Persona";
        case ActionType.UserGroupCreated:
        case ActionType.UserGroupModified:
        case ActionType.UserGroupDeleted:
          return "SysAT_UserGroup";
        case ActionType.BusinessContactGroupCreated:
        case ActionType.BusinessContactGroupModified:
        case ActionType.BusinessContactGroupDeleted:
          return "SysAT_BizContactGroup";
        case ActionType.BusinessRuleCreated:
        case ActionType.BusinessRuleModified:
        case ActionType.BusinessRuleDeleted:
        case ActionType.BusinessRuleActivated:
        case ActionType.BusinessRuleDeactivated:
          return "SysAT_BizRule";
        case ActionType.FailedLoginUserNotFound:
        case ActionType.FailedLoginPasswordMismatch:
        case ActionType.FailedLoginUserDisabled:
        case ActionType.FailedLoginLoginDisabled:
        case ActionType.FailedLoginUserLocked:
        case ActionType.FailedLoginPersonaNotFound:
        case ActionType.IPBlocked:
        case ActionType.SSOFailedUserLogin:
          return "SysAT_FailedUserLogin";
        case ActionType.FeeRuleCreated:
        case ActionType.FeeRuleModified:
        case ActionType.FeeRuleDeleted:
        case ActionType.FeeRuleActivated:
        case ActionType.FeeRuleDeactivated:
        case ActionType.FieldRuleCreated:
        case ActionType.FieldRuleModified:
        case ActionType.FieldRuleDeleted:
        case ActionType.FieldRuleActivated:
        case ActionType.FieldRuleDeactivated:
        case ActionType.FeeScenarioCreated:
        case ActionType.FeeScenarioModified:
        case ActionType.FeeScenarioDeleted:
        case ActionType.FeeScenarioActivated:
        case ActionType.FeeScenarioDeactivated:
        case ActionType.FieldScenarioCreated:
        case ActionType.FieldScenarioModified:
        case ActionType.FieldScenarioDeleted:
        case ActionType.FieldScenarioActivated:
        case ActionType.FieldScenarioDeactivated:
        case ActionType.DataTableCreated:
        case ActionType.DataTableModified:
        case ActionType.DataTableDeleted:
        case ActionType.DataPopulationTimingModified:
          return "SysAT_DDM";
        case ActionType.HMDACreated:
        case ActionType.HMDAModified:
        case ActionType.HMDADeleted:
          return "SysAT_HMDA";
        case ActionType.ServerSettingsChanged:
          return "SysAT_ServerSettings";
        default:
          return "";
      }
    }

    [PgReady]
    private static SystemAuditRecord[] getAuditRecords(
      string tableName,
      int firstRecordId,
      int lastRecordId)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        try
        {
          pgDbQueryBuilder.AppendLine("Select * from " + tableName + " where id >= " + (object) firstRecordId + " and id <= " + (object) lastRecordId + " Order by id");
          return SystemAuditTrailAccessor.GetSystemAuditRecordFromTable(pgDbQueryBuilder.ExecuteTableQuery());
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (SystemAuditTrailAccessor), ex);
          return (SystemAuditRecord[]) null;
        }
      }
      else
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        try
        {
          dbQueryBuilder.AppendLine("Select * from " + tableName + " where id >= " + (object) firstRecordId + " and id <= " + (object) lastRecordId + " Order by id");
          return SystemAuditTrailAccessor.GetSystemAuditRecordFromTable(dbQueryBuilder.ExecuteTableQuery());
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (SystemAuditTrailAccessor), ex);
          return (SystemAuditRecord[]) null;
        }
      }
    }

    public static SystemAuditRecord[] GetAuditRecord(
      string userID,
      ActionType[] actionTypes,
      DateTime startTime,
      DateTime endTime,
      string objectID,
      string objectName)
    {
      string criteria = "(1=1)";
      if (userID != string.Empty)
        criteria = criteria + " and UserID = " + SQL.EncodeString(userID);
      if (actionTypes != null && actionTypes.Length != 0)
      {
        string str = "";
        foreach (ActionType actionType in actionTypes)
          str = !(str == "") ? str + ", " + (object) (int) actionType : string.Concat((object) (int) actionType);
        criteria = criteria + " and ActionType in (" + str + ")";
      }
      if (startTime != DateTime.MinValue)
        criteria = criteria + " and DTTMStamp >= " + SQL.EncodeDateTime(startTime);
      if (endTime != DateTime.MaxValue)
        criteria = criteria + " and DTTMStamp <= " + SQL.EncodeDateTime(endTime);
      string str1 = SystemAuditTrailAccessor.insertIndividualSearchCriteria(actionTypes[0], criteria, objectID, objectName);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      try
      {
        dbQueryBuilder.AppendLine("Select * from " + SystemAuditTrailAccessor.getTableName(actionTypes[0]) + " where " + str1 + " Order by DTTMStamp");
        return SystemAuditTrailAccessor.GetSystemAuditRecordFromTable(dbQueryBuilder.ExecuteTableQuery());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (SystemAuditTrailAccessor), ex);
        return (SystemAuditRecord[]) null;
      }
    }

    private static string insertIndividualSearchCriteria(
      ActionType actionType,
      string criteria,
      string objectID,
      string objectName)
    {
      if (objectID != string.Empty)
      {
        switch (actionType)
        {
          case ActionType.LoanCreated:
          case ActionType.LoanImported:
          case ActionType.LoanModified:
          case ActionType.LoanDeleted:
          case ActionType.LoanPermanentDeleted:
          case ActionType.LoanRestored:
          case ActionType.LoanMoved:
            criteria = criteria + " and LoanNumber = " + SQL.EncodeString(objectID);
            break;
          case ActionType.UserCreated:
          case ActionType.UserModified:
          case ActionType.UserDeleted:
            criteria = criteria + " and UserAccountID = " + SQL.EncodeString(objectID);
            break;
          case ActionType.UserPasswordChangeForced:
            criteria = criteria + " and UserAccountID = " + SQL.EncodeString(objectID);
            break;
          case ActionType.UserPasswordChanged:
            criteria = criteria + " and UserAccountID = " + SQL.EncodeString(objectID);
            break;
          case ActionType.UserLogin:
          case ActionType.SSOUserLogin:
            criteria = criteria + " and UserAccountID = " + SQL.EncodeString(objectID);
            break;
          case ActionType.UserLogout:
            criteria = criteria + " and LogoutUserID = " + SQL.EncodeString(objectID);
            break;
          case ActionType.TemplateCreated:
          case ActionType.TemplateModified:
          case ActionType.TemplateDeleted:
            criteria = criteria + " and TemplateName like (" + SQL.EncodeString(objectID) + ")";
            break;
          case ActionType.PersonaCreated:
          case ActionType.PersonaModified:
          case ActionType.PersonaDeleted:
            criteria = criteria + " and PersonaID = " + SQL.EncodeString(objectID);
            break;
          case ActionType.UserGroupCreated:
          case ActionType.UserGroupModified:
          case ActionType.UserGroupDeleted:
            criteria = criteria + " and UserGroupID = " + SQL.EncodeString(objectID);
            break;
          case ActionType.BusinessContactGroupCreated:
          case ActionType.BusinessContactGroupModified:
          case ActionType.BusinessContactGroupDeleted:
            criteria = criteria + " and ContactGroupID = " + SQL.EncodeString(objectID);
            break;
          case ActionType.BusinessRuleCreated:
          case ActionType.BusinessRuleModified:
          case ActionType.BusinessRuleDeleted:
          case ActionType.BusinessRuleActivated:
          case ActionType.BusinessRuleDeactivated:
            criteria = criteria + " and RuleID = " + SQL.EncodeString(objectID);
            break;
        }
      }
      if (objectName != string.Empty)
      {
        switch (actionType)
        {
          case ActionType.LoanCreated:
          case ActionType.LoanImported:
          case ActionType.LoanModified:
          case ActionType.LoanDeleted:
          case ActionType.LoanPermanentDeleted:
          case ActionType.LoanRestored:
          case ActionType.LoanMoved:
            criteria = criteria + " and BorrowerLastName like( " + SQL.EncodeString(objectName) + ")";
            break;
          case ActionType.UserCreated:
          case ActionType.UserModified:
          case ActionType.UserDeleted:
            criteria = criteria + " and UserAccountName like( " + SQL.EncodeString(objectName) + ")";
            break;
          case ActionType.UserPasswordChangeForced:
            criteria = criteria + " and UserAccountName like( " + SQL.EncodeString(objectName) + ")";
            break;
          case ActionType.UserPasswordChanged:
            criteria = criteria + " and UserAccountName like( " + SQL.EncodeString(objectName) + ")";
            break;
          case ActionType.UserLogin:
          case ActionType.SSOUserLogin:
            criteria = criteria + " and UserAccountName like( " + SQL.EncodeString(objectName) + ")";
            break;
          case ActionType.UserLogout:
            criteria = criteria + " and LogoutUserName like( " + SQL.EncodeString(objectName) + ")";
            break;
          case ActionType.TemplateCreated:
          case ActionType.TemplateModified:
          case ActionType.TemplateDeleted:
            criteria = criteria + " and TemplatePath like( " + SQL.EncodeString(objectName) + ")";
            break;
          case ActionType.PersonaCreated:
          case ActionType.PersonaModified:
          case ActionType.PersonaDeleted:
            criteria = criteria + " and PersonaName like( " + SQL.EncodeString(objectName) + ")";
            break;
          case ActionType.UserGroupCreated:
          case ActionType.UserGroupModified:
          case ActionType.UserGroupDeleted:
            criteria = criteria + " and UserGroupName like( " + SQL.EncodeString(objectName) + ")";
            break;
          case ActionType.BusinessContactGroupCreated:
          case ActionType.BusinessContactGroupModified:
          case ActionType.BusinessContactGroupDeleted:
            criteria = criteria + " and ContactGroupName like( " + SQL.EncodeString(objectName) + ")";
            break;
          case ActionType.BusinessRuleCreated:
          case ActionType.BusinessRuleModified:
          case ActionType.BusinessRuleDeleted:
          case ActionType.BusinessRuleActivated:
          case ActionType.BusinessRuleDeactivated:
            criteria = criteria + " and RuleName like( " + SQL.EncodeString(objectName) + ")";
            break;
          case ActionType.FailedLoginUserNotFound:
          case ActionType.FailedLoginPasswordMismatch:
          case ActionType.FailedLoginUserDisabled:
          case ActionType.FailedLoginLoginDisabled:
          case ActionType.FailedLoginUserLocked:
          case ActionType.FailedLoginPersonaNotFound:
          case ActionType.IPBlocked:
          case ActionType.SSOFailedUserLogin:
            criteria = criteria + " and IPAddress like( " + SQL.EncodeString(objectName) + ")";
            break;
          case ActionType.FeeRuleCreated:
          case ActionType.FeeRuleModified:
          case ActionType.FeeRuleDeleted:
          case ActionType.FeeRuleActivated:
          case ActionType.FeeRuleDeactivated:
          case ActionType.FieldRuleCreated:
          case ActionType.FieldRuleModified:
          case ActionType.FieldRuleDeleted:
          case ActionType.FieldRuleActivated:
          case ActionType.FieldRuleDeactivated:
          case ActionType.FeeScenarioCreated:
          case ActionType.FeeScenarioModified:
          case ActionType.FeeScenarioDeleted:
          case ActionType.FeeScenarioActivated:
          case ActionType.FeeScenarioDeactivated:
          case ActionType.FieldScenarioCreated:
          case ActionType.FieldScenarioModified:
          case ActionType.FieldScenarioDeleted:
          case ActionType.FieldScenarioActivated:
          case ActionType.FieldScenarioDeactivated:
          case ActionType.DataTableCreated:
          case ActionType.DataTableModified:
          case ActionType.DataTableDeleted:
          case ActionType.DataPopulationTimingModified:
            criteria = criteria + " and RuleName like( " + SQL.EncodeString(objectName) + ")";
            break;
          case ActionType.HMDACreated:
          case ActionType.HMDAModified:
          case ActionType.HMDADeleted:
            criteria = criteria + " and ProfileName like( " + SQL.EncodeString(objectName) + ")";
            break;
        }
      }
      return criteria;
    }

    private static SystemAuditRecord[] GetSystemAuditRecordFromTable(DataTable source)
    {
      List<SystemAuditRecord> systemAuditRecordList = new List<SystemAuditRecord>();
      if (source == null || source.Rows.Count == 0)
        return systemAuditRecordList.ToArray();
      foreach (DataRow row in (InternalDataCollectionBase) source.Rows)
      {
        ActionType actionType = (ActionType) row["ActionType"];
        string str1 = string.Concat(row["UserID"]);
        string str2 = string.Concat(row["UserFullName"]);
        DateTime dateTime = DateTime.Parse(string.Concat(row["DTTMStamp"]));
        AuditObjectType type = (AuditObjectType) row["ObjectType"];
        SystemAuditRecord systemAuditRecord = (SystemAuditRecord) null;
        switch (actionType)
        {
          case ActionType.LoanCreated:
          case ActionType.LoanImported:
          case ActionType.LoanModified:
          case ActionType.LoanDeleted:
            DateTime minValue = DateTime.MinValue;
            int num = 0;
            if (!(row["AutoSaveDate"] is DBNull))
              minValue = DateTime.Parse(string.Concat(row["AutoSaveDate"]));
            if (!(row["LoanFileVersionNumber"] is DBNull))
              num = int.Parse(string.Concat(row["LoanFileVersionNumber"]));
            systemAuditRecord = (SystemAuditRecord) new LoanFileAuditRecord(str1, str2, actionType, dateTime, string.Concat(row["loanGuid"]), string.Concat(row["loanFolder"]), string.Concat(row["loanNumber"]), string.Concat(row["BorrowerLastName"]), string.Concat(row["BorrowerFirstName"]), string.Concat(row["Address"]), string.Concat(row["FileSource"]), string.Concat(row["AppName"]));
            ((LoanFileAuditRecord) systemAuditRecord).AutoSaveDateTime = minValue;
            ((LoanFileAuditRecord) systemAuditRecord).FileVersionNumber = num;
            break;
          case ActionType.LoanPermanentDeleted:
          case ActionType.LoanRestored:
          case ActionType.LoanMoved:
            systemAuditRecord = (SystemAuditRecord) new LoanFileAuditRecord(str1, str2, actionType, dateTime, string.Concat(row["loanGuid"]), string.Concat(row["loanFolder"]), string.Concat(row["loanNumber"]), string.Concat(row["BorrowerLastName"]), string.Concat(row["BorrowerFirstName"]), string.Concat(row["Address"]), string.Concat(row["FileSource"]), string.Concat(row["AppName"]));
            break;
          case ActionType.UserCreated:
          case ActionType.UserModified:
          case ActionType.UserDeleted:
            systemAuditRecord = (SystemAuditRecord) new UserProfileAuditRecord(str1, str2, actionType, dateTime, string.Concat(row["UserAccountID"]), string.Concat(row["UserAccountName"]));
            break;
          case ActionType.UserPasswordChangeForced:
            systemAuditRecord = (SystemAuditRecord) new UserPwdChangeForcedAuditRecord(str1, str2, actionType, dateTime, string.Concat(row["UserAccountID"]), string.Concat(row["UserAccountName"]));
            break;
          case ActionType.UserPasswordChanged:
            systemAuditRecord = (SystemAuditRecord) new UserPwdChangeAuditRecord(str1, str2, actionType, dateTime, string.Concat(row["UserAccountID"]), string.Concat(row["UserAccountName"]), string.Concat(row["PriorStatus"]));
            break;
          case ActionType.UserLogin:
          case ActionType.SSOUserLogin:
            systemAuditRecord = (SystemAuditRecord) new UserLoginAuditRecord(str1, str2, actionType, dateTime, string.Concat(row["UserAccountID"]), string.Concat(row["UserAccountName"]), string.Concat(row["IPAddress"]), string.Concat(row["MachineName"]));
            break;
          case ActionType.UserLogout:
            systemAuditRecord = (SystemAuditRecord) new UserLogoutAuditRecord(str1, str2, actionType, dateTime, string.Concat(row["LogoutUserID"]), string.Concat(row["LogoutUserName"]), string.Concat(row["LogoutReason"]), double.Parse(string.Concat(row["Duration"])));
            break;
          case ActionType.TemplateCreated:
          case ActionType.TemplateModified:
          case ActionType.TemplateDeleted:
            systemAuditRecord = (SystemAuditRecord) new TemplateAuditRecord(str1, str2, actionType, dateTime, string.Concat(row["TemplateName"]), string.Concat(row["TemplatePath"]));
            break;
          case ActionType.PersonaCreated:
          case ActionType.PersonaModified:
          case ActionType.PersonaDeleted:
            systemAuditRecord = (SystemAuditRecord) new PersonaAuditRecord(str1, str2, actionType, dateTime, int.Parse(string.Concat(row["PersonaID"])), string.Concat(row["PersonaName"]));
            break;
          case ActionType.UserGroupCreated:
          case ActionType.UserGroupModified:
          case ActionType.UserGroupDeleted:
            systemAuditRecord = (SystemAuditRecord) new UserGroupAuditRecord(str1, str2, actionType, dateTime, int.Parse(string.Concat(row["UserGroupID"])), string.Concat(row["UserGroupName"]));
            break;
          case ActionType.BusinessContactGroupCreated:
          case ActionType.BusinessContactGroupModified:
          case ActionType.BusinessContactGroupDeleted:
            systemAuditRecord = (SystemAuditRecord) new BizContactGroupAuditRecord(str1, str2, actionType, dateTime, int.Parse(string.Concat(row["ContactGroupID"])), string.Concat(row["ContactGroupName"]));
            break;
          case ActionType.BusinessRuleCreated:
          case ActionType.BusinessRuleModified:
          case ActionType.BusinessRuleDeleted:
          case ActionType.BusinessRuleActivated:
          case ActionType.BusinessRuleDeactivated:
            systemAuditRecord = (SystemAuditRecord) new BizRuleAuditRecord(str1, str2, actionType, dateTime, string.Concat(row["RuleID"]), string.Concat(row["RuleName"]), type);
            break;
          case ActionType.FailedLoginUserNotFound:
          case ActionType.FailedLoginPasswordMismatch:
          case ActionType.FailedLoginUserDisabled:
          case ActionType.FailedLoginLoginDisabled:
          case ActionType.FailedLoginUserLocked:
          case ActionType.FailedLoginPersonaNotFound:
          case ActionType.IPBlocked:
          case ActionType.SSOFailedUserLogin:
            systemAuditRecord = (SystemAuditRecord) new FailedUserLoginAuditRecord(str1, actionType, string.Concat(row["IPAddress"]), dateTime);
            break;
          case ActionType.FeeRuleCreated:
          case ActionType.FeeRuleModified:
          case ActionType.FeeRuleDeleted:
          case ActionType.FeeRuleActivated:
          case ActionType.FeeRuleDeactivated:
          case ActionType.FieldRuleCreated:
          case ActionType.FieldRuleModified:
          case ActionType.FieldRuleDeleted:
          case ActionType.FieldRuleActivated:
          case ActionType.FieldRuleDeactivated:
          case ActionType.FeeScenarioCreated:
          case ActionType.FeeScenarioModified:
          case ActionType.FeeScenarioDeleted:
          case ActionType.FeeScenarioActivated:
          case ActionType.FeeScenarioDeactivated:
          case ActionType.FieldScenarioCreated:
          case ActionType.FieldScenarioModified:
          case ActionType.FieldScenarioDeleted:
          case ActionType.FieldScenarioActivated:
          case ActionType.FieldScenarioDeactivated:
          case ActionType.DataTableCreated:
          case ActionType.DataTableModified:
          case ActionType.DataTableDeleted:
          case ActionType.DataPopulationTimingModified:
            systemAuditRecord = (SystemAuditRecord) new DDMAuditRecord(str1, str2, actionType, dateTime, string.Concat(row["OrderID"]), string.Concat(row["RuleName"]), type, Convert.ToString(row["ScenarioName"]), Convert.ToString(row["AdditionalInfo"]));
            break;
          case ActionType.HMDACreated:
          case ActionType.HMDAModified:
          case ActionType.HMDADeleted:
            systemAuditRecord = (SystemAuditRecord) new HMDAAuditRecord(str1, str2, actionType, dateTime, int.Parse(string.Concat(row["ProfileID"])), type, string.Concat(row["ProfileName"]));
            break;
        }
        systemAuditRecordList.Add(systemAuditRecord);
      }
      return systemAuditRecordList.ToArray();
    }

    public static void PurgeSystemAuditRecords(string dataFolderPath, int numOfDays)
    {
      string message = "";
      string[] strArray;
      if (Company.GetServerLicense().Edition == EncompassEdition.Broker)
        strArray = new string[10]
        {
          "LoanFile",
          "LoanTemplateResource",
          "Persona",
          "UserAccount",
          "UserGroup",
          "UserLogin",
          "UserLogout",
          "UserPasswordChangeForced",
          "UserPasswordChanged",
          "FailedUserLogin"
        };
      else
        strArray = new string[13]
        {
          "BusinessContactGroup",
          "BusinessRule",
          "DynamicDataManagement",
          "LoanFile",
          "LoanTemplateResource",
          "Persona",
          "UserAccount",
          "UserGroup",
          "UserLogin",
          "UserLogout",
          "UserPasswordChangeForced",
          "UserPasswordChanged",
          "FailedUserLogin"
        };
      DateTime filterDTTM = DateTime.Today.AddDays((double) -numOfDays);
      foreach (string categoryName in strArray)
      {
        try
        {
          SystemAuditTrailAccessor.backupAuditTrailRecords(categoryName, dataFolderPath, filterDTTM);
        }
        catch (Exception ex)
        {
          message = message + ex.ToString() + Environment.NewLine;
        }
      }
      if (!(message != ""))
        return;
      Err.Reraise(nameof (SystemAuditTrailAccessor), new Exception(message));
    }

    private static void backupAuditTrailRecords(
      string categoryName,
      string dataFolderPath,
      DateTime filterDTTM)
    {
      List<ActionType> actionTypeList = new List<ActionType>();
      switch (categoryName)
      {
        case "BusinessContactGroup":
          actionTypeList.AddRange((IEnumerable<ActionType>) new ActionType[3]
          {
            ActionType.BusinessContactGroupCreated,
            ActionType.BusinessContactGroupDeleted,
            ActionType.BusinessContactGroupModified
          });
          break;
        case "BusinessRule":
          actionTypeList.AddRange((IEnumerable<ActionType>) new ActionType[5]
          {
            ActionType.BusinessRuleActivated,
            ActionType.BusinessRuleCreated,
            ActionType.BusinessRuleDeactivated,
            ActionType.BusinessRuleDeleted,
            ActionType.BusinessRuleModified
          });
          break;
        case "DynamicDataManagement":
          actionTypeList.AddRange((IEnumerable<ActionType>) new ActionType[24]
          {
            ActionType.FeeRuleActivated,
            ActionType.FeeRuleCreated,
            ActionType.FeeRuleDeactivated,
            ActionType.FeeRuleDeleted,
            ActionType.FeeRuleModified,
            ActionType.FieldRuleActivated,
            ActionType.FieldRuleCreated,
            ActionType.FieldRuleDeactivated,
            ActionType.FieldRuleDeleted,
            ActionType.FieldRuleModified,
            ActionType.FeeScenarioActivated,
            ActionType.FeeScenarioCreated,
            ActionType.FeeScenarioDeactivated,
            ActionType.FeeScenarioDeleted,
            ActionType.FeeScenarioModified,
            ActionType.FieldScenarioActivated,
            ActionType.FieldScenarioCreated,
            ActionType.FieldScenarioDeactivated,
            ActionType.FieldScenarioDeleted,
            ActionType.FieldScenarioModified,
            ActionType.DataTableCreated,
            ActionType.DataTableModified,
            ActionType.DataTableDeleted,
            ActionType.DataPopulationTimingModified
          });
          break;
        case "FailedUserLogin":
          actionTypeList.AddRange((IEnumerable<ActionType>) new ActionType[8]
          {
            ActionType.FailedLoginLoginDisabled,
            ActionType.FailedLoginPasswordMismatch,
            ActionType.FailedLoginPersonaNotFound,
            ActionType.FailedLoginUserDisabled,
            ActionType.FailedLoginUserLocked,
            ActionType.FailedLoginUserNotFound,
            ActionType.SSOFailedUserLogin,
            ActionType.IPBlocked
          });
          break;
        case "LoanFile":
          actionTypeList.AddRange((IEnumerable<ActionType>) new ActionType[7]
          {
            ActionType.LoanCreated,
            ActionType.LoanDeleted,
            ActionType.LoanImported,
            ActionType.LoanModified,
            ActionType.LoanPermanentDeleted,
            ActionType.LoanRestored,
            ActionType.LoanMoved
          });
          break;
        case "LoanTemplateResource":
          actionTypeList.AddRange((IEnumerable<ActionType>) new ActionType[3]
          {
            ActionType.TemplateCreated,
            ActionType.TemplateDeleted,
            ActionType.TemplateModified
          });
          break;
        case "Persona":
          actionTypeList.AddRange((IEnumerable<ActionType>) new ActionType[3]
          {
            ActionType.PersonaCreated,
            ActionType.PersonaDeleted,
            ActionType.PersonaModified
          });
          break;
        case "UserAccount":
          actionTypeList.AddRange((IEnumerable<ActionType>) new ActionType[3]
          {
            ActionType.UserCreated,
            ActionType.UserDeleted,
            ActionType.UserModified
          });
          break;
        case "UserGroup":
          actionTypeList.AddRange((IEnumerable<ActionType>) new ActionType[3]
          {
            ActionType.UserGroupCreated,
            ActionType.UserGroupDeleted,
            ActionType.UserGroupModified
          });
          break;
        case "UserLogin":
          actionTypeList.AddRange((IEnumerable<ActionType>) new ActionType[2]
          {
            ActionType.UserLogin,
            ActionType.SSOUserLogin
          });
          break;
        case "UserLogout":
          actionTypeList.AddRange((IEnumerable<ActionType>) new ActionType[1]
          {
            ActionType.UserLogout
          });
          break;
        case "UserPasswordChangeForced":
          actionTypeList.AddRange((IEnumerable<ActionType>) new ActionType[1]
          {
            ActionType.UserPasswordChangeForced
          });
          break;
        case "UserPasswordChanged":
          actionTypeList.AddRange((IEnumerable<ActionType>) new ActionType[1]
          {
            ActionType.UserPasswordChanged
          });
          break;
      }
      SystemAuditTrailAccessor.backupRecordToFile(categoryName, dataFolderPath, filterDTTM, actionTypeList.ToArray());
    }

    private static void backupRecordToFile(
      string categoryName,
      string dataFolderPath,
      DateTime filterDate,
      ActionType[] categoryList)
    {
      string tableName = SystemAuditTrailAccessor.getTableName(categoryList[0]);
      int[] recordIdRange = SystemAuditTrailAccessor.getRecordIDRange(tableName, SQL.MinSmallDatetime, filterDate);
      int num1 = recordIdRange[0];
      int val2 = recordIdRange[1];
      if (num1 == -1)
        return;
      int num2;
      for (; num1 <= val2; num1 = num2 + 1)
      {
        Dictionary<DateTime, StringBuilder> dictionary1 = new Dictionary<DateTime, StringBuilder>();
        Dictionary<DateTime, string> dictionary2 = new Dictionary<DateTime, string>();
        num2 = Math.Min(num1 + 999, val2);
        foreach (SystemAuditRecord auditRecord in SystemAuditTrailAccessor.getAuditRecords(tableName, num1, num2))
        {
          DateTime date = auditRecord.DateTime.Date;
          if (!dictionary1.ContainsKey(date))
          {
            dictionary1[date] = new StringBuilder();
            dictionary2[date] = auditRecord.HeaderToCSV + Environment.NewLine;
          }
          dictionary1[date].AppendLine(auditRecord.ContentToCSV);
        }
        foreach (DateTime key in dictionary1.Keys)
        {
          string str1 = key.Year.ToString() + "_" + (object) key.Month + "_" + (object) key.Day;
          string str2 = Path.Combine(Path.Combine(dataFolderPath, "SystemAuditTrail"), categoryName);
          if (!Directory.Exists(str2))
            Directory.CreateDirectory(str2);
          using (DataFile dataFile = FileStore.CheckOut(Path.Combine(str2, categoryName + "_" + str1 + ".csv"), MutexAccess.Write))
          {
            if (!dataFile.Exists)
              dataFile.CheckIn(new BinaryObject(dictionary2[key], Encoding.Default), true);
            dataFile.Append(new BinaryObject(dictionary1[key].ToString(), Encoding.Default), false);
          }
        }
        SystemAuditTrailAccessor.deleteAuditTrailRecords(tableName, num1, num2);
      }
    }

    [PgReady]
    private static void deleteAuditTrailRecords(string tableName, int firstIndex, int lastIndex)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("delete from " + tableName + " where id >= " + (object) firstIndex + " and id <= " + (object) lastIndex);
        pgDbQueryBuilder.ExecuteNonQuery();
      }
      else
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("delete from " + tableName + " where id >= " + (object) firstIndex + " and id <= " + (object) lastIndex);
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    private static void deleteAuditTrailRecords(DateTime filterDate, ActionType[] actionTypes)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string str1 = "DTTMStamp <= " + SQL.EncodeDateTime(filterDate);
      if (actionTypes != null && actionTypes.Length != 0)
      {
        string str2 = "";
        foreach (ActionType actionType in actionTypes)
          str2 = !(str2 == "") ? str2 + ", " + (object) (int) actionType : string.Concat((object) (int) actionType);
        str1 = str1 + " and ActionType in (" + str2 + ")";
      }
      dbQueryBuilder.AppendLine("Delete " + SystemAuditTrailAccessor.getTableName(actionTypes[0]) + " where " + str1);
      dbQueryBuilder.ExecuteNonQuery();
    }
  }
}

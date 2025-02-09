// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.CampaignProvider
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.TaskList;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using EllieMae.EMLite.Server.Query;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class CampaignProvider
  {
    private CampaignProvider()
    {
    }

    [PgReady]
    public static int[] GetCampaignQueryListForUser(string userId)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        if (userId == null || !(string.Empty != userId))
          return (int[]) null;
        int[] data1 = new int[3]{ 2, 4, 6 };
        int[] data2 = new int[2]{ 4, 6 };
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("SELECT c.CampaignId FROM Campaign c WHERE c.UserId = @userid AND (");
        pgDbQueryBuilder.AppendLine("    (c.Status = " + (object) 0 + " AND c.CampaignType IN (" + EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) data1) + ")) OR ");
        pgDbQueryBuilder.AppendLine("    (c.Status = " + (object) 1 + " AND c.CampaignType IN (" + EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) data2) + "))");
        pgDbQueryBuilder.AppendLine(") AND DATEDIFF('dd', c.NextRefreshTime, GETDATE()) >= 0 ORDER BY c.CampaignId");
        DbCommandParameter parameter = new DbCommandParameter("userid", (object) userId.TrimEnd(), DbType.AnsiString);
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute(parameter);
        if (dataRowCollection.Count == 0)
          return (int[]) null;
        int[] queryListForUser = new int[dataRowCollection.Count];
        for (int index = 0; index < dataRowCollection.Count; ++index)
          queryListForUser[index] = (int) dataRowCollection[index]["CampaignId"];
        return queryListForUser;
      }
      if (userId == null || !(string.Empty != userId))
        return (int[]) null;
      int[] data3 = new int[3]{ 2, 4, 6 };
      int[] data4 = new int[2]{ 4, 6 };
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT c.CampaignId FROM Campaign c WHERE c.UserId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) + " AND ((c.Status = " + (object) 0 + " AND c.CampaignType IN (" + EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) data3) + ")) OR (c.Status = " + (object) 1 + " AND c.CampaignType IN (" + EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) data4) + "))) AND DATEDIFF(dd, c.NextRefreshTime, GETDATE()) >= 0 ORDER BY c.CampaignId");
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      if (dataRowCollection1.Count == 0)
        return (int[]) null;
      int[] queryListForUser1 = new int[dataRowCollection1.Count];
      for (int index = 0; index < dataRowCollection1.Count; ++index)
        queryListForUser1[index] = (int) dataRowCollection1[index]["CampaignId"];
      return queryListForUser1;
    }

    public static CampaignInfo RunCampaignQueries(User user, int campaignId)
    {
      if (0 >= campaignId)
        return (CampaignInfo) null;
      CampaignInfo campaignInfo = CampaignProvider.GetCampaignInfo(campaignId);
      if (campaignInfo == null)
        return (CampaignInfo) null;
      if (campaignInfo.Status != CampaignStatus.Running && CampaignStatus.Stopped != campaignInfo.Status)
        return (CampaignInfo) null;
      DbQueryBuilder sqlStmt = new DbQueryBuilder();
      sqlStmt.Declare("@CurrentDate", "DATETIME");
      sqlStmt.AppendLine("SET @CurrentDate = GETDATE()");
      CampaignProvider.buildContactActivityUpdate(user, campaignInfo, sqlStmt);
      sqlStmt.AppendLine("UPDATE Campaign SET NextRefreshTime = " + CampaignProvider.nextRefreshTime(campaignInfo) + " WHERE CampaignId = " + (object) campaignInfo.CampaignId);
      string str = campaignInfo.ContactType == ContactType.Borrower ? "BorrowerCampaignActivity" : "PartnerCampaignActivity";
      sqlStmt.AppendLine("UPDATE CampaignStep SET LastActivityDate = @CurrentDate WHERE CampaignStepId IN (SELECT DISTINCT s.CampaignStepId FROM CampaignStep s INNER JOIN " + str + " a ON s.CampaignStepId = a.CampaignStepId WHERE s.CampaignId = " + (object) campaignInfo.CampaignId + " AND a.CompletedDate = @CurrentDate)");
      if (0 < campaignInfo.ContactGroupId)
        sqlStmt.AppendLine("DELETE FROM ContactGroup WHERE GroupId = " + (object) campaignInfo.ContactGroupId);
      sqlStmt.ExecuteNonQuery();
      return CampaignProvider.GetCampaign(campaignId);
    }

    public static CampaignInfo[] GetCampaignsForUser(CampaignCollectionCriteria criteria)
    {
      if (criteria.UserId == null || criteria.ContactTypes == null || criteria.ContactTypes.Length == 0)
        return (CampaignInfo[]) null;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      foreach (ContactType contactType in criteria.ContactTypes)
      {
        CampaignProvider.buildCampaignQuery(criteria.UserId, contactType, criteria.CampaignStatuses, (ActivityType[]) null, (DateTime[]) null, (ActivityStatus[]) null, dbQueryBuilder);
        CampaignProvider.buildCampaignStepQuery(criteria.UserId, contactType, criteria.CampaignStatuses, (ActivityType[]) null, (DateTime[]) null, (ActivityStatus[]) null, dbQueryBuilder);
        CampaignProvider.buildCampaignActivitySummaryQuery(criteria.UserId, contactType, criteria.CampaignStatuses, (ActivityType[]) null, criteria.ActivityDateRange, new ActivityStatus[1]
        {
          ActivityStatus.Expected
        }, dbQueryBuilder);
      }
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      int count = dataSet.Tables[0].Rows.Count;
      if (dataSet.Tables.Count > 3)
        count += dataSet.Tables[3].Rows.Count;
      if (count == 0)
        return (CampaignInfo[]) null;
      CampaignInfo[] campaignsForUser = new CampaignInfo[count];
      int index1 = 0;
      for (int index2 = 0; index2 < criteria.ContactTypes.Length; ++index2)
      {
        dataSet.Relations.Add("CampaignsSteps" + index2.ToString(), dataSet.Tables[index2 * 3].Columns["CampaignId"], dataSet.Tables[index2 * 3 + 1].Columns["CampaignId"]);
        dataSet.Relations.Add("ActivitySummary" + index2.ToString(), dataSet.Tables[index2 * 3 + 1].Columns["CampaignStepId"], dataSet.Tables[index2 * 3 + 2].Columns["CampaignStepId"]);
        for (int index3 = 0; index3 < dataSet.Tables[index2 * 3].Rows.Count; ++index3)
        {
          DataRow row = dataSet.Tables[index2 * 3].Rows[index3];
          campaignsForUser[index1] = CampaignProvider.convertRowToCampaign(row, (DataRow) null);
          DataRow[] childRows1 = row.GetChildRows("CampaignsSteps" + index2.ToString());
          int length = childRows1.Length;
          CampaignStepInfo[] campaignStepInfoArray = new CampaignStepInfo[length];
          for (int index4 = 0; index4 < length; ++index4)
          {
            DataRow stepRow = childRows1[index4];
            DataRow[] childRows2 = stepRow.GetChildRows("ActivitySummary" + index2.ToString());
            DataRow activitySummaryRow = childRows2.Length != 0 ? childRows2[0] : (DataRow) null;
            campaignStepInfoArray[index4] = CampaignProvider.convertRowToStep(stepRow, activitySummaryRow);
          }
          campaignsForUser[index1].CampaignSteps = campaignStepInfoArray;
          ++index1;
        }
      }
      return campaignsForUser;
    }

    public static CampaignInfo GetCampaign(int campaignId)
    {
      if (0 >= campaignId)
        return (CampaignInfo) null;
      CampaignInfo campaignInfo = CampaignProvider.GetCampaignInfo(campaignId);
      if (campaignInfo == null)
        return (CampaignInfo) null;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT s.* FROM CampaignStep s WHERE s.CampaignId = " + (object) campaignId + " ORDER BY s.StepNumber");
      string str = campaignInfo.ContactType == ContactType.Borrower ? "BorrowerCampaignActivity" : "PartnerCampaignActivity";
      DateTime[] dateTimeArray = new DateTime[2]
      {
        new DateTime(2000, 1, 1, 0, 0, 0),
        new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59)
      };
      dbQueryBuilder.AppendLine("SELECT s.CampaignStepId, COUNT(a.ContactId) AS TasksDueCount FROM CampaignStep s INNER JOIN " + str + " a ON s.CampaignStepId = a.CampaignStepId WHERE s.CampaignId = " + (object) campaignId + " AND a.Status = " + Enum.Format(typeof (ActivityStatus), (object) ActivityStatus.Expected, "D") + " AND a.ScheduledDate BETWEEN " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(dateTimeArray[0], false) + " AND " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(dateTimeArray[1], false) + " GROUP BY s.CampaignStepId");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (0 >= dataSet.Tables[0].Rows.Count)
        return (CampaignInfo) null;
      campaignInfo.CampaignSteps = new CampaignStepInfo[dataSet.Tables[0].Rows.Count];
      dataSet.Relations.Add("TasksDue", dataSet.Tables[0].Columns["CampaignStepId"], dataSet.Tables[1].Columns["CampaignStepId"]);
      for (int index = 0; index < dataSet.Tables[0].Rows.Count; ++index)
      {
        DataRow row = dataSet.Tables[0].Rows[index];
        DataRow[] childRows = row.GetChildRows("TasksDue");
        DataRow activitySummaryRow = childRows.Length != 0 ? childRows[0] : (DataRow) null;
        campaignInfo.CampaignSteps[index] = CampaignProvider.convertRowToStep(row, activitySummaryRow);
      }
      return campaignInfo;
    }

    public static CampaignInfo SaveCampaign(CampaignInfo campaignInfo)
    {
      if (campaignInfo == null || !(typeof (CampaignInfo) == campaignInfo.GetType()))
        return (CampaignInfo) null;
      if (campaignInfo.CampaignId == 0 && CampaignProvider.findCampaign(campaignInfo.UserId, campaignInfo.CampaignName))
        throw new ApplicationException("Violation of UNIQUE KEY constraint 'UK_Campaign_UserId_CampaignName'");
      DbQueryBuilder sqlStmt = new DbQueryBuilder();
      sqlStmt.Declare("@contactGroupId", "int");
      if ((ContactGroupInfo) null != campaignInfo.ContactGroupInfo)
      {
        int num = ContactGroupProvider.SaveContactGroup(campaignInfo.ContactGroupInfo);
        sqlStmt.SelectVar("@contactGroupId", (object) num);
      }
      else if (campaignInfo.ContactGroupId != 0)
        sqlStmt.SelectVar("@contactGroupId", (object) campaignInfo.ContactGroupId);
      if (campaignInfo.AddQueryInfo != null)
      {
        ContactGroupProvider.BuildSaveQueryStatement(campaignInfo.AddQueryInfo, sqlStmt);
      }
      else
      {
        sqlStmt.Declare("@addQueryId", "int");
        if (campaignInfo.AddQueryId != 0)
          sqlStmt.SelectVar("@addQueryId", (object) campaignInfo.AddQueryId);
      }
      if (campaignInfo.DeleteQueryInfo != null)
      {
        ContactGroupProvider.BuildSaveQueryStatement(campaignInfo.DeleteQueryInfo, sqlStmt);
      }
      else
      {
        sqlStmt.Declare("@deleteQueryId", "int");
        if (campaignInfo.DeleteQueryId != 0)
          sqlStmt.SelectVar("@deleteQueryId", (object) campaignInfo.DeleteQueryId);
      }
      sqlStmt.Declare("@campaignId", "int");
      if (campaignInfo.CampaignId == 0)
      {
        sqlStmt.InsertInto(DbAccessManager.GetTable("Campaign"), CampaignProvider.createDbValueList(campaignInfo), true, false);
        sqlStmt.SelectIdentity("@campaignId");
      }
      else
      {
        DbValue key = new DbValue("CampaignId", (object) campaignInfo.CampaignId);
        sqlStmt.Update(DbAccessManager.GetTable("Campaign"), CampaignProvider.createDbValueList(campaignInfo), key);
        sqlStmt.SelectVar("@campaignId", (object) campaignInfo.CampaignId);
      }
      if (campaignInfo.DeletedStepIds.Length != 0)
        sqlStmt.AppendLine("DELETE FROM CampaignStep WHERE CampaignId = " + (object) campaignInfo.CampaignId + "AND CampaignStepId IN " + CampaignProvider.encodeIntArray((Array) campaignInfo.DeletedStepIds));
      foreach (CampaignStepInfo addedStep in campaignInfo.AddedSteps)
        sqlStmt.InsertInto(DbAccessManager.GetTable("CampaignStep"), CampaignProvider.createDbValueList(addedStep), true, false);
      foreach (CampaignStepInfo updatedStep in campaignInfo.UpdatedSteps)
      {
        DbValue key = new DbValue("CampaignStepId", (object) updatedStep.CampaignStepId);
        sqlStmt.Update(DbAccessManager.GetTable("CampaignStep"), CampaignProvider.createDbValueList(updatedStep), key);
      }
      sqlStmt.Select("@campaignId");
      return CampaignProvider.GetCampaign((int) sqlStmt.ExecuteScalar());
    }

    public static void DeleteCampaign(int campaignId)
    {
      if (0 >= campaignId || CampaignProvider.GetCampaignInfo(campaignId) == null)
        return;
      CampaignProvider.deleteCampaignFromDatabase(campaignId);
    }

    public static void CopyCampaign(
      int oldCampaignId,
      bool copyContacts,
      string newCampaignName,
      string newCampaignDesc)
    {
      if (0 >= oldCampaignId || newCampaignName == null || 0 >= newCampaignName.Length)
        return;
      CampaignInfo campaignInfo = CampaignProvider.GetCampaignInfo(oldCampaignId);
      if (campaignInfo == null)
        return;
      if (CampaignProvider.findCampaign(campaignInfo.UserId, newCampaignName))
        throw new ApplicationException("Violation of UNIQUE KEY constraint 'UK_Campaign_UserId_CampaignName'");
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      dbQueryBuilder1.Declare("@groupId", "int");
      DbQueryBuilder dbQueryBuilder2 = dbQueryBuilder1;
      object[] objArray1 = new object[6]
      {
        (object) "INSERT INTO ContactGroup SELECT c.UserId, c.ContactType, ",
        (object) Enum.Format(typeof (ContactGroupType), (object) ContactGroupType.CampaignGroup, "D"),
        (object) " AS GroupType, ",
        null,
        null,
        null
      };
      Guid guid = Guid.NewGuid();
      objArray1[3] = (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) guid.ToString());
      objArray1[4] = (object) " AS GroupName, '' AS GroupDesc, GETDATE() AS CreationTime FROM Campaign c WHERE c.CampaignId = ";
      objArray1[5] = (object) oldCampaignId;
      string text1 = string.Concat(objArray1);
      dbQueryBuilder2.AppendLine(text1);
      dbQueryBuilder1.SelectIdentity("@groupId");
      if (copyContacts)
      {
        string str1 = campaignInfo.ContactType == ContactType.Borrower ? "ContactGroupBorrowers" : "ContactGroupPartners";
        if (0 < campaignInfo.ContactGroupId)
          dbQueryBuilder1.AppendLine("INSERT INTO " + str1 + " SELECT @groupId AS GroupId, b.ContactId, GETDATE() AS CreatedDate FROM Campaign c INNER JOIN ContactGroup g ON c.ContactGroupId = g.GroupId INNER JOIN " + str1 + " b ON g.GroupId = b.GroupId WHERE c.CampaignId = " + (object) oldCampaignId);
        string str2 = campaignInfo.ContactType == ContactType.Borrower ? "BorrowerCampaignActivity" : "PartnerCampaignActivity";
        if (campaignInfo.Status == CampaignStatus.Running || CampaignStatus.Stopped == campaignInfo.Status)
          dbQueryBuilder1.AppendLine("INSERT INTO " + str1 + " SELECT DISTINCT @groupId AS GroupId, a.ContactId, GETDATE() AS CreatedDate FROM Campaign c INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId INNER JOIN " + str2 + " a ON s.CampaignStepId = a.CampaignStepId WHERE c.CampaignId = " + (object) oldCampaignId);
      }
      dbQueryBuilder1.Declare("@addQueryId", "int");
      if (0 < campaignInfo.AddQueryId)
      {
        DbQueryBuilder dbQueryBuilder3 = dbQueryBuilder1;
        object[] objArray2 = new object[4]
        {
          (object) "INSERT INTO ContactQuery SELECT aq.UserId, aq.ContactType, aq.QueryType, ",
          null,
          null,
          null
        };
        guid = Guid.NewGuid();
        objArray2[1] = (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) guid.ToString());
        objArray2[2] = (object) " AS QueryName, aq.QueryDesc, aq.XmlQueryString, aq.PrimaryOnly FROM Campaign c INNER JOIN ContactQuery aq ON c.AddQueryId = aq.QueryId WHERE c.CampaignId = ";
        objArray2[3] = (object) oldCampaignId;
        string text2 = string.Concat(objArray2);
        dbQueryBuilder3.AppendLine(text2);
        dbQueryBuilder1.SelectIdentity("@addQueryId");
      }
      dbQueryBuilder1.Declare("@deleteQueryId", "int");
      if (0 < campaignInfo.DeleteQueryId)
      {
        DbQueryBuilder dbQueryBuilder4 = dbQueryBuilder1;
        object[] objArray3 = new object[4]
        {
          (object) "INSERT INTO ContactQuery SELECT dq.UserId, dq.ContactType, dq.QueryType, ",
          null,
          null,
          null
        };
        guid = Guid.NewGuid();
        objArray3[1] = (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) guid.ToString());
        objArray3[2] = (object) " AS QueryName, dq.QueryDesc, dq.XmlQueryString, dq.PrimaryOnly FROM Campaign c INNER JOIN ContactQuery dq ON c.DeleteQueryId = dq.QueryId WHERE c.CampaignId = ";
        objArray3[3] = (object) oldCampaignId;
        string text3 = string.Concat(objArray3);
        dbQueryBuilder4.AppendLine(text3);
        dbQueryBuilder1.SelectIdentity("@deleteQueryId");
      }
      dbQueryBuilder1.Declare("@campaignId", "int");
      dbQueryBuilder1.AppendLine("INSERT INTO Campaign SELECT c.UserId, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) newCampaignName) + " AS CampaignName, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) newCampaignDesc) + " AS CampaignDesc, c.CampaignType, c.ContactType, " + Enum.Format(typeof (CampaignStatus), (object) CampaignStatus.NotStarted, "D") + " AS Status, c.FrequencyType, c.FrequencyInterval, @addQueryId AS AddQueryId, @deleteQueryId AS DeleteQueryId, @groupId AS ContactGroupId, GETDATE() AS CreationTime, NULL AS StartedTime, NULL AS NextRefreshTime FROM Campaign c WHERE c.CampaignId = " + (object) oldCampaignId);
      dbQueryBuilder1.SelectIdentity("@campaignId");
      dbQueryBuilder1.AppendLine("INSERT INTO CampaignStep SELECT @CampaignId AS CampaignId, s.StepNumber, s.StepName, s.StepDesc, s.StepInterval, s.ActivityType, s.DocumentId, s.Subject, s.Comments, s.TaskPriority, s.BarColor, NULL AS LastActivityDate, s.ActivityUserId FROM Campaign c INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId WHERE c.CampaignId = " + (object) oldCampaignId);
      dbQueryBuilder1.ExecuteNonQuery();
    }

    public static CampaignInfo StartCampaign(User user, int campaignId)
    {
      if (0 >= campaignId)
        return (CampaignInfo) null;
      CampaignInfo campaignInfo = CampaignProvider.GetCampaignInfo(campaignId);
      if (campaignInfo == null)
        return (CampaignInfo) null;
      if (CampaignStatus.NotStarted != campaignInfo.Status && CampaignStatus.Stopped != campaignInfo.Status)
        return (CampaignInfo) null;
      DbQueryBuilder sqlStmt = new DbQueryBuilder();
      sqlStmt.Declare("@CurrentDate", "DATETIME");
      sqlStmt.AppendLine("SET @CurrentDate = GETDATE()");
      CampaignProvider.buildContactActivityUpdate(user, campaignInfo, sqlStmt);
      sqlStmt.AppendLine("UPDATE Campaign SET Status = " + Enum.Format(typeof (CampaignStatus), (object) CampaignStatus.Running, "D") + ", ContactGroupId = NULL, StartedTime = @CurrentDate, NextRefreshTime = " + CampaignProvider.nextRefreshTime(campaignInfo) + " WHERE CampaignId = " + (object) campaignInfo.CampaignId);
      if (0 < campaignInfo.ContactGroupId)
        sqlStmt.AppendLine("DELETE FROM ContactGroup WHERE GroupId = " + (object) campaignInfo.ContactGroupId);
      sqlStmt.ExecuteNonQuery();
      return CampaignProvider.GetCampaign(campaignId);
    }

    public static CampaignInfo StopCampaign(int campaignId)
    {
      if (0 >= campaignId)
        return (CampaignInfo) null;
      CampaignProvider.setCampaignStatus(campaignId, CampaignStatus.Stopped);
      return CampaignProvider.GetCampaign(campaignId);
    }

    public static CampaignContactInfo[] GetCampaignContacts(
      CampaignContactCollectionCritera criteria,
      string userID)
    {
      if (criteria == null || 0 >= criteria.CampaignId)
        return (CampaignContactInfo[]) null;
      if (criteria.FieldList == null || criteria.FieldList.Length == 0)
        return (CampaignContactInfo[]) null;
      CampaignInfo campaignInfo = CampaignProvider.GetCampaignInfo(criteria.CampaignId);
      if (campaignInfo == null)
        return (CampaignContactInfo[]) null;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(CampaignProvider.buildCampaignContactQuery(campaignInfo, criteria, userID));
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      int count = dataSet.Tables[0].Rows.Count;
      if (count == 0)
        return (CampaignContactInfo[]) null;
      dataSet.Relations.Add("ContactActivity", dataSet.Tables[0].Columns["ContactId"], dataSet.Tables[1].Columns["ContactId"]);
      dataSet.Relations.Add("ContactProperties", dataSet.Tables[0].Columns["ContactId"], dataSet.Tables[2].Columns["ContactId"]);
      CampaignContactInfo[] campaignContacts = new CampaignContactInfo[count];
      for (int index1 = 0; index1 < count; ++index1)
      {
        DataRow parentRow = dataSet.Tables[2].Rows[index1].GetParentRow("ContactProperties");
        campaignContacts[index1] = CampaignProvider.convertRowToContact(parentRow);
        DataRow[] childRows1 = parentRow.GetChildRows("ContactActivity");
        int length = childRows1.Length;
        CampaignActivityInfo[] campaignActivityInfoArray = new CampaignActivityInfo[length];
        for (int index2 = 0; index2 < length; ++index2)
        {
          DataRow activityRow = childRows1[index2];
          campaignActivityInfoArray[index2] = CampaignProvider.convertRowToActivity(activityRow);
        }
        campaignContacts[index1].ContactActivities = campaignActivityInfoArray;
        DataRow[] childRows2 = parentRow.GetChildRows("ContactProperties");
        if (childRows2 != null && childRows2.Length != 0)
          CampaignProvider.addContactProperties(campaignInfo, criteria, childRows2[0], campaignContacts[index1].ContactProperties);
      }
      return campaignContacts;
    }

    public static CampaignInfo UpdateCampaignContacts(
      int campaignId,
      CrudRequestParameter[] crudRequests)
    {
      if (0 >= campaignId)
        return (CampaignInfo) null;
      if (crudRequests == null)
        return (CampaignInfo) null;
      CampaignInfo campaignInfo = CampaignProvider.GetCampaignInfo(campaignId);
      if (campaignInfo == null)
        return (CampaignInfo) null;
      string activityTable = campaignInfo.ContactType == ContactType.Borrower ? "BorrowerCampaignActivity" : "PartnerCampaignActivity";
      foreach (CrudRequestParameter crudRequest in crudRequests)
      {
        if (crudRequest.CrudAction == CrudAction.Create)
          CampaignProvider.createCampaignContacts(campaignInfo, activityTable, crudRequest.ContactIds);
        else if (CrudAction.Delete == crudRequest.CrudAction)
          CampaignProvider.deleteCampaignContacts(campaignInfo, activityTable, crudRequest.ContactIds);
        else
          CampaignProvider.updateCampaignContacts(campaignInfo, activityTable, crudRequest.ContactIds);
      }
      return CampaignProvider.GetCampaign(campaignId);
    }

    public static CampaignTasksDueInfo[] GetCampaignsTasksDue(string userId)
    {
      if (userId == null || string.Empty == userId)
        return (CampaignTasksDueInfo[]) null;
      DateTime[] dateTimeArray1 = new DateTime[2];
      dateTimeArray1[0] = new DateTime(2000, 1, 1, 0, 0, 0);
      int year = DateTime.Today.Year;
      DateTime today = DateTime.Today;
      int month = today.Month;
      today = DateTime.Today;
      int day = today.Day;
      dateTimeArray1[1] = new DateTime(year, month, day, 23, 59, 59);
      DateTime[] dateTimeArray2 = dateTimeArray1;
      string str = "SELECT c.CampaignId, c.CampaignName, COUNT(a.ContactId) AS TasksDueCount FROM Campaign c INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId INNER JOIN CampaignActivityTable a ON s.CampaignStepId = a.CampaignStepId WHERE c.UserId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) + " AND a.Status = " + Enum.Format(typeof (ActivityStatus), (object) ActivityStatus.Expected, "D") + " AND c.Status = " + (object) 0 + " AND a.ScheduledDate BETWEEN " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(dateTimeArray2[0], false) + " AND " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(dateTimeArray2[1], false) + " GROUP BY c.CampaignId, c.CampaignName HAVING Count(a.ContactId) > 0  ORDER BY TasksDueCount DESC";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(DBReadReplicaFeature.HomePage);
      dbQueryBuilder.AppendLine(str.Replace("CampaignActivityTable", "BorrowerCampaignActivity"));
      dbQueryBuilder.AppendLine(str.Replace("CampaignActivityTable", "PartnerCampaignActivity"));
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (2 != dataSet.Tables.Count)
        return (CampaignTasksDueInfo[]) null;
      List<CampaignTasksDueInfo> campaignTasksDueInfoList = new List<CampaignTasksDueInfo>(dataSet.Tables[0].Rows.Count + dataSet.Tables[1].Rows.Count);
      foreach (DataTable table in (InternalDataCollectionBase) dataSet.Tables)
      {
        foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
          campaignTasksDueInfoList.Add(CampaignProvider.convertRowToCampaignTasksDue(row));
      }
      campaignTasksDueInfoList.Sort((IComparer<CampaignTasksDueInfo>) new CampaignTasksDueComparer());
      return campaignTasksDueInfoList.ToArray();
    }

    public static int GetTasksDueForUser(string userId)
    {
      if (userId == null || !(string.Empty != userId))
        return 0;
      DateTime[] dateTimeArray1 = new DateTime[2];
      dateTimeArray1[0] = new DateTime(2000, 1, 1, 0, 0, 0);
      int year = DateTime.Today.Year;
      DateTime today = DateTime.Today;
      int month = today.Month;
      today = DateTime.Today;
      int day = today.Day;
      dateTimeArray1[1] = new DateTime(year, month, day, 23, 59, 59);
      DateTime[] dateTimeArray2 = dateTimeArray1;
      string str = "SELECT COUNT(a.ContactId) AS TasksDueCount FROM Campaign c INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId INNER JOIN CampaignActivityTable a ON s.CampaignStepId = a.CampaignStepId WHERE c.UserId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) + " AND a.Status = " + Enum.Format(typeof (ActivityStatus), (object) ActivityStatus.Expected, "D") + " AND c.Status = " + (object) 0 + " AND a.ScheduledDate BETWEEN " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(dateTimeArray2[0], false) + " AND " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(dateTimeArray2[1], false);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(str.Replace("CampaignActivityTable", "BorrowerCampaignActivity"));
      dbQueryBuilder.AppendLine(str.Replace("CampaignActivityTable", "PartnerCampaignActivity"));
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      return 2 != dataSet.Tables.Count ? 0 : (int) dataSet.Tables[0].Rows[0]["TasksDueCount"] + (int) dataSet.Tables[1].Rows[0]["TasksDueCount"];
    }

    public static CampaignStepInfo GetCampaignStepActivity(
      CampaignActivityCollectionCriteria criteria)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT s.* FROM CampaignStep s WHERE s.CampaignStepId = " + (object) criteria.CampaignStepId);
      string str = criteria.ContactType == ContactType.Borrower ? "BorrowerCampaignActivity" : "PartnerCampaignActivity";
      DateTime[] dateTimeArray1 = new DateTime[2];
      dateTimeArray1[0] = new DateTime(2000, 1, 1, 0, 0, 0);
      DateTime today = DateTime.Today;
      int year = today.Year;
      today = DateTime.Today;
      int month = today.Month;
      today = DateTime.Today;
      int day = today.Day;
      dateTimeArray1[1] = new DateTime(year, month, day, 23, 59, 59);
      DateTime[] dateTimeArray2 = dateTimeArray1;
      dbQueryBuilder.AppendLine("SELECT COUNT(a.ContactId) AS TasksDueCount FROM CampaignStep s INNER JOIN " + str + " a ON s.CampaignStepId = a.CampaignStepId WHERE s.CampaignStepId = " + (object) criteria.CampaignStepId + " AND a.Status = " + Enum.Format(typeof (ActivityStatus), (object) ActivityStatus.Expected, "D") + " AND a.ScheduledDate BETWEEN " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(dateTimeArray2[0], false) + " AND " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(dateTimeArray2[1], false));
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (1 != dataSet.Tables[0].Rows.Count || 1 != dataSet.Tables[1].Rows.Count)
        return (CampaignStepInfo) null;
      CampaignStepInfo step = CampaignProvider.convertRowToStep(dataSet.Tables[0].Rows[0], dataSet.Tables[1].Rows[0]);
      step.CampaignActivities = CampaignProvider.getStepActivity(criteria);
      return step;
    }

    public static CampaignStepInfo UpdateCampaignStepActiviity(
      CampaignActivityCollectionCriteria criteria,
      ActivityUpdateParameter activityUpdateParameter)
    {
      if (0 >= criteria.CampaignStepId || !Enum.IsDefined(typeof (ContactType), (object) criteria.ContactType) || activityUpdateParameter == null)
        return (CampaignStepInfo) null;
      CampaignStepInfo campaignStepInfo = CampaignProvider.GetCampaignStepInfo(criteria.CampaignStepId);
      if (campaignStepInfo == null)
        return (CampaignStepInfo) null;
      CampaignInfo campaignInfo = CampaignProvider.GetCampaignInfo(campaignStepInfo.CampaignId);
      if (campaignInfo == null)
        return (CampaignStepInfo) null;
      CampaignProvider.updateActivityStatus(campaignInfo, campaignStepInfo, activityUpdateParameter);
      return CampaignProvider.GetCampaignStepActivity(criteria);
    }

    public static CampaignHistoryInfo[] GetCampaignHistory(
      CampaignHistoryCollectionCritera criteria,
      SortField[] sortFields)
    {
      CampaignInfo campaignInfo = CampaignProvider.GetCampaignInfo(criteria.CampaignId);
      if (campaignInfo == null)
        return (CampaignHistoryInfo[]) null;
      QueryResult res = CampaignProvider.executeCampaignHistoryQuery(campaignInfo, criteria, sortFields);
      CampaignHistoryInfo[] campaignHistory = new CampaignHistoryInfo[res.RecordCount];
      for (int index = 0; index < campaignHistory.Length; ++index)
        campaignHistory[index] = CampaignProvider.convertRowToHistory(res, index);
      return campaignHistory;
    }

    public static void UpdateDocumentIds(FileSystemEntry source, FileSystemEntry target)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (FileSystemEntry.Types.File == source.Type)
      {
        dbQueryBuilder.AppendLine("UPDATE CampaignStep SET DocumentId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) target.ToString()) + " WHERE DocumentId LIKE " + EllieMae.EMLite.DataAccess.SQL.Encode((object) EllieMae.EMLite.DataAccess.SQL.Escape(source.ToString())));
      }
      else
      {
        dbQueryBuilder.Declare("@srcLen", "int");
        dbQueryBuilder.SelectVar("@srcLen", (object) source.ToString().Length);
        dbQueryBuilder.AppendLine("UPDATE CampaignStep SET DocumentId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) target.ToString()) + " + SUBSTRING(DocumentId, @srcLen + 1, Len(DocumentId) - @srcLen) WHERE DocumentId LIKE " + EllieMae.EMLite.DataAccess.SQL.Encode((object) (EllieMae.EMLite.DataAccess.SQL.Escape(source.ToString()) + "%")));
      }
      dbQueryBuilder.ExecuteNonQuery();
      TemplateSettings.MoveTemplateXRefs(TemplateSettingsType.CustomLetter, source, target);
    }

    private static bool hasCampaignAccess(User user)
    {
      return FeaturesAclDbAccessor.CheckPermission(AclFeature.Cnt_Campaign_Access, user.UserInfo);
    }

    private static bool findCampaign(string userId, string campaignName)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT COUNT(*) FROM Campaign c WHERE c.UserId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) + " AND c.CampaignName like '" + EllieMae.EMLite.DataAccess.SQL.Escape(campaignName) + "'");
      return (int) dbQueryBuilder.ExecuteScalar() != 0;
    }

    private static CampaignActivityInfo[] getStepActivity(
      CampaignActivityCollectionCriteria criteria)
    {
      if (0 >= criteria.CampaignStepId || !Enum.IsDefined(typeof (ContactType), (object) criteria.ContactType))
        return (CampaignActivityInfo[]) null;
      DbQueryBuilder activityQuery = new DbQueryBuilder();
      CampaignProvider.buildCampaignActivityQuery(criteria, activityQuery);
      DataRowCollection dataRowCollection = activityQuery.Execute();
      int count = dataRowCollection.Count;
      CampaignActivityInfo[] stepActivity = new CampaignActivityInfo[count];
      for (int index = 0; index < count; ++index)
      {
        stepActivity[index] = CampaignProvider.convertRowToActivity(dataRowCollection[index]);
        CampaignProvider.addContactProperties(criteria, dataRowCollection[index], stepActivity[index].ContactProperties);
      }
      return stepActivity;
    }

    private static void createCampaignContacts(
      CampaignInfo campaign,
      string activityTable,
      int[] contactIds)
    {
      string str = campaign.ContactType == ContactType.Borrower ? "Borrower" : "BizPartner";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      CampaignProvider.IncrementalIntArray intArray = new CampaignProvider.IncrementalIntArray(100, contactIds);
      do
      {
        dbQueryBuilder.Declare("@CurrentDate", "DATETIME");
        dbQueryBuilder.AppendLine("SET @CurrentDate = GETDATE()");
        dbQueryBuilder.Declare("@CampaignStepId", "INT");
        dbQueryBuilder.AppendLine("INSERT INTO " + activityTable + " SELECT s.CampaignStepId, b.ContactId, @CurrentDate, DATEADD(day, s.StepInterval, @CurrentDate), NULL, " + Enum.Format(typeof (ActivityStatus), (object) ActivityStatus.Expected, "D") + " FROM Campaign c INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId CROSS JOIN " + str + " b WHERE c.CampaignId = " + (object) campaign.CampaignId + " AND b.ContactId IN " + CampaignProvider.encodeIntArray(intArray));
        dbQueryBuilder.AppendLine(CampaignProvider.buildCampaignLevelHistory(campaign, "Campaign Insert", "CreatedDate"));
        dbQueryBuilder.ExecuteNonQuery();
        dbQueryBuilder.Reset();
      }
      while (intArray.Increment());
    }

    private static void deleteCampaignContacts(
      CampaignInfo campaign,
      string activityTable,
      int[] contactIds)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      CampaignProvider.IncrementalIntArray incrementalIntArray = new CampaignProvider.IncrementalIntArray(100, contactIds);
      do
      {
        dbQueryBuilder.Declare("@CurrentDate", "DATETIME");
        dbQueryBuilder.AppendLine("SET @CurrentDate = GETDATE()");
        dbQueryBuilder.Declare("@CampaignStepId", "INT");
        dbQueryBuilder.AppendLine("UPDATE CampaignStep SET LastActivityDate = @CurrentDate FROM Campaign c INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId INNER JOIN " + activityTable + " a ON s.CampaignStepId = a.CampaignStepId WHERE c.CampaignId = " + (object) campaign.CampaignId + " AND a.ContactId IN " + CampaignProvider.encodeIntArray(incrementalIntArray) + " AND a.Status = " + Enum.Format(typeof (ActivityStatus), (object) ActivityStatus.Expected, "D"));
        dbQueryBuilder.AppendLine("UPDATE " + activityTable + " SET Status = " + Enum.Format(typeof (ActivityStatus), (object) ActivityStatus.Removed, "D") + " , CompletedDate = @CurrentDate FROM Campaign c INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId INNER JOIN " + activityTable + " a ON s.CampaignStepId = a.CampaignStepId WHERE c.CampaignId = " + (object) campaign.CampaignId + " AND a.ContactId IN " + CampaignProvider.encodeIntArray(incrementalIntArray) + " AND a.Status = " + Enum.Format(typeof (ActivityStatus), (object) ActivityStatus.Expected, "D"));
        dbQueryBuilder.AppendLine(CampaignProvider.buildActivityLevelHistory(campaign, ActivityStatus.Removed, incrementalIntArray));
        dbQueryBuilder.AppendLine(CampaignProvider.buildCampaignLevelHistory(campaign, "Campaign Remove", "CompletedDate"));
        dbQueryBuilder.ExecuteNonQuery();
        dbQueryBuilder.Reset();
      }
      while (incrementalIntArray.Increment());
    }

    private static void updateCampaignContacts(
      CampaignInfo campaign,
      string activityTable,
      int[] contactIds)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      CampaignProvider.IncrementalIntArray intArray = new CampaignProvider.IncrementalIntArray(100, contactIds);
      do
      {
        dbQueryBuilder.Declare("@CurrentDate", "DATETIME");
        dbQueryBuilder.AppendLine("SET @CurrentDate = GETDATE()");
        dbQueryBuilder.Declare("@CampaignStepId", "INT");
        dbQueryBuilder.AppendLine("UPDATE " + activityTable + " SET CreatedDate = @CurrentDate, ScheduledDate = DATEADD(day, s.StepInterval, @CurrentDate), CompletedDate = NULL, Status = " + Enum.Format(typeof (ActivityStatus), (object) ActivityStatus.Expected, "D") + " FROM Campaign c INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId INNER JOIN " + activityTable + " a ON s.CampaignStepId = a.CampaignStepId WHERE c.CampaignId = " + (object) campaign.CampaignId + " AND a.ContactId IN " + CampaignProvider.encodeIntArray(intArray));
        dbQueryBuilder.AppendLine(CampaignProvider.buildCampaignLevelHistory(campaign, "Campaign Insert", "CreatedDate"));
        dbQueryBuilder.ExecuteNonQuery();
        dbQueryBuilder.Reset();
      }
      while (intArray.Increment());
    }

    private static string buildCampaignContactQuery(
      CampaignInfo campaign,
      CampaignContactCollectionCritera criteria,
      string userID)
    {
      string str1 = campaign.ContactType == ContactType.Borrower ? "BorrowerCampaignActivity" : "PartnerCampaignActivity";
      string str2 = campaign.ContactType == ContactType.Borrower ? "Borrower" : "BizPartner";
      string str3 = campaign.ContactType != ContactType.Borrower ? "ContactGroupPartners ContactGroupXref on ContactGroupXref.ContactID = Contact.ContactID " + " left outer join ContactGroup on ContactGroup.GroupID = ContactGroupXref.GroupID left outer join AclGroupPublicBizGroupRef GroupAccess on (GroupAccess.bizGroupID = ContactGroup.GroupID) " + " left outer join FN_GetUsersAclGroups(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userID) + ") UserGroup on (UserGroup.GroupID = GroupAccess.aclGroupID) " : "ContactGroupBorrowers ContactGroupXref on ContactGroupXref.ContactID = Contact.ContactID left outer join ContactGroup on ContactGroup.GroupID = ContactGroupXref.GroupID ";
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.Append("SELECT DISTINCT CampaignActivity.ContactId FROM Campaign INNER JOIN CampaignStep ON Campaign.CampaignId = CampaignStep.CampaignId INNER JOIN " + str1 + " CampaignActivity ON CampaignStep.CampaignStepId = CampaignActivity.CampaignStepId INNER JOIN " + str2 + " Contact ON CampaignActivity.ContactId = Contact.ContactId Left OUTER JOIN " + str3 + "WHERE Campaign.CampaignId = " + (object) campaign.CampaignId);
      if (criteria.FilterCriteria != null)
        stringBuilder1.Append(" AND " + criteria.FilterCriteria.ToSQLClause());
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder2.AppendLine("SELECT a.ContactId, MIN(a.CreatedDate) AS CreatedDate FROM Campaign c INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId INNER JOIN " + str1 + " a ON s.CampaignStepId = a.CampaignStepId WHERE c.CampaignId = " + (object) campaign.CampaignId + " AND a.ContactId IN (" + stringBuilder1.ToString() + ") GROUP BY a.ContactId");
      StringBuilder stringBuilder3 = new StringBuilder();
      stringBuilder3.AppendLine("SELECT a.* FROM Campaign c INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId INNER JOIN " + str1 + " a ON s.CampaignStepId = a.CampaignStepId WHERE c.CampaignId = " + (object) campaign.CampaignId + " AND a.ContactId IN (" + stringBuilder1.ToString() + ") ORDER BY s.StepNumber");
      bool flag = false;
      StringBuilder stringBuilder4 = new StringBuilder();
      stringBuilder4.Append("SELECT Contact.ContactID");
      foreach (string field in criteria.FieldList)
      {
        if (field.ToLower().StartsWith("contact.") && "contact.contactid" != field.ToLower())
          stringBuilder4.Append(", " + field);
        else if ("contactgroupcount.groupcount" == field.ToLower())
        {
          flag = true;
          stringBuilder4.Append(", ISNULL(ContactGroupCount.GroupCount, 0) AS GroupCount");
        }
      }
      stringBuilder4.Append(" FROM " + str2 + " Contact ");
      if (flag)
      {
        string str4 = campaign.ContactType == ContactType.Borrower ? "FN_GetBorrowerContactGroupCount(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userID) + ") ContactGroupCount " : "FN_GetBizContactGroupCount(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userID) + ", 1) ContactGroupCount";
        stringBuilder4.Append("LEFT OUTER JOIN " + str4 + " on ContactGroupCount.ContactID = Contact.ContactID ");
      }
      stringBuilder4.Append("LEFT OUTER JOIN (" + (object) stringBuilder2 + ") CampaignActivity on Contact.ContactId = CampaignActivity.ContactId ");
      stringBuilder4.Append("WHERE Contact.ContactId IN (" + stringBuilder1.ToString() + ") ");
      if (criteria.SortFields.Length != 0)
      {
        stringBuilder4.Append(" Order By ");
        for (int index = 0; index < criteria.SortFields.Length; ++index)
        {
          SortField sortField = criteria.SortFields[index];
          stringBuilder4.Append((index > 0 ? ", " : " ") + sortField.Term.FieldName + (sortField.SortOrder == FieldSortOrder.Ascending ? " ASC " : " DESC "));
        }
      }
      return stringBuilder2.ToString() + stringBuilder3.ToString() + stringBuilder4.ToString();
    }

    private static string getCampaignContactSortField(SortField[] sortFields)
    {
      if (sortFields == null || sortFields.Length == 0)
        return "CreatedDate DESC";
      StringBuilder stringBuilder = new StringBuilder();
      switch (sortFields[0].Term.FieldName)
      {
        case "BizAddress1":
          stringBuilder.Append("Address");
          break;
        case "BizCity":
          stringBuilder.Append("City");
          break;
        case "BizEmail":
          stringBuilder.Append("EmailAddress");
          break;
        case "BizState":
          stringBuilder.Append("State");
          break;
        case "CreatedDate":
          stringBuilder.Append("CreatedDate");
          break;
        case "FaxNumber":
          stringBuilder.Append("b.FaxNumber");
          break;
        case "FirstName":
          stringBuilder.Append("b.FirstName");
          break;
        case "HomeAddress1":
          stringBuilder.Append("Address");
          break;
        case "HomeCity":
          stringBuilder.Append("City");
          break;
        case "HomePhone":
          stringBuilder.Append("PhoneNumber");
          break;
        case "HomeState":
          stringBuilder.Append("State");
          break;
        case "LastName":
          stringBuilder.Append("b.LastName");
          break;
        case "PersonalEmail":
          stringBuilder.Append("EmailAddress");
          break;
        case "WorkPhone":
          stringBuilder.Append("PhoneNumber");
          break;
        default:
          stringBuilder.Append("CreatedDate");
          break;
      }
      if (FieldSortOrder.Descending == sortFields[0].SortOrder)
        stringBuilder.Append(" DESC");
      return stringBuilder.ToString();
    }

    private static QueryResult executeCampaignHistoryQuery(
      CampaignInfo campaign,
      CampaignHistoryCollectionCritera criteria,
      SortField[] sortFields)
    {
      QueryEngine queryEngine = campaign.ContactType != ContactType.Borrower ? (QueryEngine) new BizPartnerQuery((UserInfo) null, RelatedLoanMatchType.None) : (QueryEngine) new BorrowerQuery((UserInfo) null, RelatedLoanMatchType.None);
      DataQuery query = new DataQuery();
      query.Selections.AddField("History.ContactID");
      query.Selections.AddField("History.CampaignId");
      query.Selections.AddField("History.CampaignStepNumber");
      query.Selections.AddField("History.CampaignStepName");
      query.Selections.AddField("History.ContactId");
      query.Selections.AddField("History.CampaignActivityDescription");
      query.Selections.AddField("History.TimeOfHistory");
      query.Selections.AddField("History.Sender");
      query.Selections.AddField("History.EventType");
      query.Selections.AddField("History.NoteId");
      query.Selections.AddField("Contact.LastName");
      query.Selections.AddField("Contact.FirstName");
      query.Filter = (QueryCriterion) new OrdinalValueCriterion("History.CampaignId", (object) campaign.CampaignId);
      query.Filter = query.Filter.And((QueryCriterion) new NullValueCriterion("History.CampaignStepNumber", false));
      if (criteria.FilterCriteria != null)
        query.Filter = query.Filter.And(criteria.FilterCriteria);
      if (sortFields != null && sortFields.Length != 0)
        query.SortFields.AddRange((IEnumerable<SortField>) sortFields);
      query.UsePrefiltering = false;
      return queryEngine.Execute(query, false);
    }

    private static CampaignInfo GetCampaignInfo(int campaignId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT c.* FROM Campaign c WHERE c.CampaignId = " + (object) campaignId);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return 1 != dataRowCollection.Count ? (CampaignInfo) null : CampaignProvider.convertRowToCampaign(dataRowCollection[0], (DataRow) null);
    }

    public static CampaignStepInfo GetCampaignStepInfo(int campaignStepId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT s.* FROM CampaignStep s WHERE s.CampaignStepId = " + (object) campaignStepId);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return 1 != dataRowCollection.Count ? (CampaignStepInfo) null : CampaignProvider.convertRowToStep(dataRowCollection[0], (DataRow) null);
    }

    private static void buildContactActivityUpdate(
      User user,
      CampaignInfo campaign,
      DbQueryBuilder sqlStmt)
    {
      ContactQueryInfo addQuery = (ContactQueryInfo) null;
      ContactQueryInfo deleteQuery = (ContactQueryInfo) null;
      CampaignProvider.getCampaignQueries(user, campaign, ref addQuery, ref deleteQuery);
      if (addQuery != null)
      {
        string str = campaign.ContactType == ContactType.Borrower ? "Borrower" : "BizPartner";
        sqlStmt.AppendLine("CREATE TABLE #TempContacts(ContactId INT)");
        string text = (ContactQueryType.CampaignPredefinedQuery & addQuery.QueryType) != ContactQueryType.CampaignPredefinedQuery ? "Insert INTO #TempContacts " + ContactGroupProvider.BuildAdvancedQueryResultString(user, addQuery, true) + " " : "Insert INTO #TempContacts " + ContactGroupProvider.BuildPredefinedQueryString(user, addQuery, true) + " ";
        sqlStmt.Append(text);
      }
      else if (0 < campaign.ContactGroupId)
      {
        string str = campaign.ContactType == ContactType.Borrower ? "ContactGroupBorrowers" : "ContactGroupPartners";
        if (0 < campaign.ContactGroupId)
          sqlStmt.AppendLine("SELECT c.ContactId INTO #TempContacts FROM ContactGroup g INNER JOIN " + str + " c ON g.GroupId = c.GroupId WHERE g.GroupId = " + (object) campaign.ContactGroupId);
      }
      else
        sqlStmt.AppendLine("CREATE TABLE #TempContacts(ContactId INT)");
      if (addQuery != null && 0 < campaign.ContactGroupId)
      {
        string str = campaign.ContactType == ContactType.Borrower ? "ContactGroupBorrowers" : "ContactGroupPartners";
        if (0 < campaign.ContactGroupId)
          sqlStmt.AppendLine("INSERT INTO #TempContacts SELECT c.ContactId FROM ContactGroup g INNER JOIN " + str + " c ON g.GroupId = c.GroupId WHERE g.GroupId = " + (object) campaign.ContactGroupId + " AND c.ContactId NOT IN (SELECT ContactId FROM #TempContacts)");
      }
      if (deleteQuery != null)
      {
        sqlStmt.Append("DELETE FROM #TempContacts WHERE ContactId IN (");
        if ((ContactQueryType.CampaignPredefinedQuery & deleteQuery.QueryType) == ContactQueryType.CampaignPredefinedQuery)
          sqlStmt.Append(ContactGroupProvider.BuildPredefinedQueryString(user, deleteQuery, true));
        else
          sqlStmt.Append(ContactGroupProvider.BuildAdvancedQueryResultString(user, deleteQuery, true));
        sqlStmt.AppendLine(")");
      }
      string str1 = campaign.ContactType == ContactType.Borrower ? "BorrowerCampaignActivity" : "PartnerCampaignActivity";
      sqlStmt.AppendLine("DELETE FROM #TempContacts WHERE ContactId IN (SELECT DISTINCT a.ContactId FROM Campaign c INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId INNER JOIN " + str1 + " a ON s.CampaignStepId = a.CampaignStepId WHERE c.CampaignId = " + (object) campaign.CampaignId + ")");
      sqlStmt.AppendLine("INSERT INTO " + str1 + " SELECT s.CampaignStepId, t.ContactId, @CurrentDate, DATEADD(day, s.StepInterval, @CurrentDate), NULL, " + Enum.Format(typeof (ActivityStatus), (object) ActivityStatus.Expected, "D") + " FROM Campaign c INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId CROSS JOIN #TempContacts t WHERE c.CampaignId = " + (object) campaign.CampaignId);
      sqlStmt.Declare("@CampaignStepId", "INT");
      sqlStmt.AppendLine(CampaignProvider.buildCampaignLevelHistory(campaign, "Campaign Add", "CreatedDate"));
      sqlStmt.AppendLine("DROP TABLE #TempContacts");
      if (deleteQuery == null)
        return;
      sqlStmt.AppendLine("UPDATE " + str1 + " SET Status = " + Enum.Format(typeof (ActivityStatus), (object) ActivityStatus.Deleted, "D") + " , CompletedDate = @CurrentDate FROM Campaign c INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId INNER JOIN " + str1 + " a ON s.CampaignStepId = a.CampaignStepId WHERE c.CampaignId = " + (object) campaign.CampaignId + " AND a.ContactId IN (");
      if ((ContactQueryType.CampaignPredefinedQuery & deleteQuery.QueryType) == ContactQueryType.CampaignPredefinedQuery)
        sqlStmt.Append(ContactGroupProvider.BuildPredefinedQueryString(user, deleteQuery, true));
      else
        sqlStmt.Append(ContactGroupProvider.BuildAdvancedQueryResultString(user, deleteQuery, true));
      sqlStmt.Append(") AND a.Status = " + Enum.Format(typeof (ActivityStatus), (object) ActivityStatus.Expected, "D"));
      sqlStmt.AppendLine(CampaignProvider.buildActivityLevelHistory(campaign, ActivityStatus.Deleted, deleteQuery, user));
      string str2 = campaign.ContactType == ContactType.Borrower ? "Borrower" : "BizPartner";
      sqlStmt.AppendLine(CampaignProvider.buildCampaignLevelHistory(campaign, "Campaign Delete", "CompletedDate"));
    }

    private static void getCampaignQueries(
      User user,
      CampaignInfo campaign,
      ref ContactQueryInfo addQuery,
      ref ContactQueryInfo deleteQuery)
    {
      addQuery = (ContactQueryInfo) null;
      deleteQuery = (ContactQueryInfo) null;
      if (!CampaignProvider.hasCampaignAccess(user))
        return;
      if ((CampaignType.AutoAddQuery & campaign.CampaignType) == CampaignType.AutoAddQuery && 0 < campaign.AddQueryId && CampaignStatus.Stopped != campaign.Status)
        addQuery = ContactGroupProvider.GetContactQuery(campaign.AddQueryId);
      if ((CampaignType.AutoDeleteQuery & campaign.CampaignType) != CampaignType.AutoDeleteQuery || 0 >= campaign.DeleteQueryId)
        return;
      deleteQuery = ContactGroupProvider.GetContactQuery(campaign.DeleteQueryId);
    }

    private static void setCampaignStatus(int campaignId, CampaignStatus campaignStatus)
    {
      if (0 >= campaignId)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("UPDATE Campaign SET Status = " + Enum.Format(typeof (CampaignStatus), (object) campaignStatus, "D") + " WHERE CampaignId = " + (object) campaignId);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static void deleteCampaignFromDatabase(int campaignId)
    {
      if (0 >= campaignId)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Declare("@groupId", "int");
      dbQueryBuilder.Append("SET @groupId = (SELECT ContactGroupId FROM Campaign WHERE CampaignId = " + (object) campaignId + ") ");
      dbQueryBuilder.Declare("@addQueryId", "int");
      dbQueryBuilder.Append("SET @addQueryId = (SELECT AddQueryId FROM Campaign WHERE CampaignId = " + (object) campaignId + ") ");
      dbQueryBuilder.Declare("@deleteQueryId", "int");
      dbQueryBuilder.Append("SET @deleteQueryId = (SELECT DeleteQueryId FROM Campaign WHERE CampaignId = " + (object) campaignId + ") ");
      dbQueryBuilder.Append("DELETE Campaign WHERE CampaignId = " + (object) campaignId + " ");
      dbQueryBuilder.Append("DELETE ContactGroup WHERE GroupId = @groupId ");
      dbQueryBuilder.Append("DELETE ContactQuery WHERE QueryId = @addQueryId ");
      dbQueryBuilder.Append("DELETE ContactQuery WHERE QueryId = @deleteQueryId ");
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static string nextRefreshTime(CampaignInfo campaign)
    {
      return string.Format("DATEADD({0}, {1}, @CurrentDate)", CampaignFrequencyType.Monthly == campaign.FrequencyType ? (object) "month" : (CampaignFrequencyType.Weekly == campaign.FrequencyType ? (object) "week" : (object) "day"), (object) (CampaignFrequencyType.Custom == campaign.FrequencyType ? campaign.FrequencyInterval : 1));
    }

    private static void buildCampaignQuery(
      string userId,
      ContactType contactType,
      CampaignStatus[] campaignStatusList,
      ActivityType[] activityTypeList,
      DateTime[] activityDateRange,
      ActivityStatus[] activityStatusList,
      DbQueryBuilder campaignQuery)
    {
      campaignQuery.Append("SELECT DISTINCT c.* FROM Campaign c");
      if (activityTypeList != null || activityDateRange != null || activityStatusList != null)
      {
        campaignQuery.Append(" INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId");
        if (activityDateRange != null || activityStatusList != null)
        {
          string str = contactType == ContactType.Borrower ? "BorrowerCampaignActivity" : "PartnerCampaignActivity";
          campaignQuery.Append(" LEFT OUTER JOIN " + str + " a ON s.CampaignStepId = a.CampaignStepId");
        }
      }
      CampaignProvider.buildWhereClause(userId, contactType, campaignStatusList, activityTypeList, activityDateRange, activityStatusList, campaignQuery);
      campaignQuery.Append(" ORDER BY c.CampaignId ");
    }

    private static void buildWhereClause(
      string userId,
      ContactType contactType,
      CampaignStatus[] campaignStatusList,
      ActivityType[] activityTypeList,
      DateTime[] activityDateRange,
      ActivityStatus[] activityStatusList,
      DbQueryBuilder query)
    {
      query.Append(" WHERE c.UserId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) + " AND c.ContactType = " + Enum.Format(typeof (ContactType), (object) contactType, "D"));
      if (campaignStatusList != null)
        query.Append(" AND c.Status IN (" + CampaignProvider.encodeEnumArray(typeof (CampaignStatus), (Array) campaignStatusList) + ")");
      if (activityTypeList != null)
        query.Append(" AND s.ActivityType IN (" + CampaignProvider.encodeEnumArray(typeof (ActivityType), (Array) activityTypeList) + ")");
      if (activityStatusList != null)
        query.Append(" AND a.Status IN (" + CampaignProvider.encodeEnumArray(typeof (ActivityStatus), (Array) activityStatusList) + ")");
      if (activityDateRange == null)
        return;
      query.Append(" AND a.ScheduledDate BETWEEN " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(activityDateRange[0], false) + " AND " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(activityDateRange[1], false));
    }

    private static string encodeIntArray(Array intArray)
    {
      StringBuilder stringBuilder = new StringBuilder("(");
      foreach (int num in intArray)
        stringBuilder.Append(num.ToString() + ",");
      stringBuilder.Replace(",", ")", stringBuilder.Length - 1, 1);
      return stringBuilder.ToString();
    }

    private static string encodeIntArray(CampaignProvider.IncrementalIntArray intArray)
    {
      StringBuilder stringBuilder = new StringBuilder("(");
      for (int start = intArray.Start; start < intArray.Start + intArray.Count; ++start)
        stringBuilder.Append(intArray.IntArray[start].ToString() + ",");
      stringBuilder.Replace(",", ")", stringBuilder.Length - 1, 1);
      return stringBuilder.ToString();
    }

    private static string encodeEnumArray(Type enumType, Array enumArray)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < enumArray.Length; ++index)
        stringBuilder.Append(Enum.Format(enumType, enumArray.GetValue(index), "D") + ",");
      --stringBuilder.Length;
      return stringBuilder.ToString();
    }

    private static void buildCampaignStepQuery(
      string userId,
      ContactType contactType,
      CampaignStatus[] campaignStatusList,
      ActivityType[] activityTypeList,
      DateTime[] activityDateRange,
      ActivityStatus[] activityStatusList,
      DbQueryBuilder stepQuery)
    {
      stepQuery.Append("SELECT s.* FROM CampaignStep s WHERE s.CampaignId IN (");
      stepQuery.Append("SELECT DISTINCT c.CampaignId FROM Campaign c");
      if (activityTypeList != null || activityDateRange != null || activityStatusList != null)
      {
        stepQuery.Append(" INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId");
        if (activityDateRange != null || activityStatusList != null)
        {
          string str = contactType == ContactType.Borrower ? "BorrowerCampaignActivity" : "PartnerCampaignActivity";
          stepQuery.Append(" LEFT OUTER JOIN " + str + " a ON s.CampaignStepId = a.CampaignStepId");
        }
      }
      CampaignProvider.buildWhereClause(userId, contactType, campaignStatusList, activityTypeList, activityDateRange, activityStatusList, stepQuery);
      stepQuery.Append(") ORDER BY s.StepNumber ");
    }

    internal static void updateActivityStatus(
      CampaignInfo campaign,
      CampaignStepInfo campaignStep,
      ActivityUpdateParameter activityUpdateParameter)
    {
      foreach (ActivityStatusParameter activityStatusParameter in activityUpdateParameter.ActivityStatusParameters)
        CampaignProvider.updateActivity(campaign, campaignStep, activityUpdateParameter.ActivityNote, activityStatusParameter.ActivityStatus, activityStatusParameter.ContactIds);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Declare("@CurrentDate", "DATETIME");
      dbQueryBuilder.AppendLine("SET @CurrentDate = GETDATE()");
      dbQueryBuilder.AppendLine("UPDATE CampaignStep SET LastActivityDate = @CurrentDate WHERE CampaignStepId = " + (object) campaignStep.CampaignStepId);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static void updateActivity(
      CampaignInfo campaign,
      CampaignStepInfo campaignStep,
      string activityNote,
      ActivityStatus activityStatus,
      int[] contactIds)
    {
      int noteId = 0;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (!string.IsNullOrEmpty(activityNote))
      {
        dbQueryBuilder.Declare("@noteId", "int");
        dbQueryBuilder.Append("INSERT INTO ContactHistoryNote (Note) VALUES (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) activityNote) + ")");
        dbQueryBuilder.SelectIdentity("@noteId");
        dbQueryBuilder.Select("@noteId");
        noteId = (int) dbQueryBuilder.ExecuteScalar();
        dbQueryBuilder.Reset();
      }
      string str = campaign.ContactType == ContactType.Borrower ? "BorrowerCampaignActivity" : "PartnerCampaignActivity";
      CampaignProvider.IncrementalIntArray incrementalIntArray = new CampaignProvider.IncrementalIntArray(100, contactIds);
      do
      {
        dbQueryBuilder.Declare("@CurrentDate", "DATETIME");
        dbQueryBuilder.AppendLine("SET @CurrentDate = GETDATE()");
        dbQueryBuilder.Declare("@CampaignStepId", "INT");
        dbQueryBuilder.AppendLine(" UPDATE " + str + " SET Status = " + Enum.Format(typeof (ActivityStatus), (object) activityStatus, "D") + ", CompletedDate = @CurrentDate WHERE CampaignStepId = " + (object) campaignStep.CampaignStepId + " AND ContactId IN " + CampaignProvider.encodeIntArray(incrementalIntArray));
        dbQueryBuilder.AppendLine(CampaignProvider.buildActivityLevelHistory(campaign, campaignStep, noteId, activityStatus, incrementalIntArray));
        if (ActivityStatus.Expected != activityStatus)
          dbQueryBuilder.AppendLine(CampaignProvider.buildCampaignLevelHistory(campaign, "Campaign Complete", "CompletedDate"));
        dbQueryBuilder.ExecuteNonQuery();
        dbQueryBuilder.Reset();
      }
      while (incrementalIntArray.Increment());
    }

    private static void buildCampaignActivitySummaryQuery(
      string userId,
      ContactType contactType,
      CampaignStatus[] campaignStatusList,
      ActivityType[] activityTypeList,
      DateTime[] activityDateRange,
      ActivityStatus[] activityStatusList,
      DbQueryBuilder summaryQuery)
    {
      string str = contactType == ContactType.Borrower ? "BorrowerCampaignActivity" : "PartnerCampaignActivity";
      summaryQuery.Append("SELECT s.CampaignStepId, COUNT(a.ContactId) AS TasksDueCount FROM Campaign c");
      summaryQuery.Append(" INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId");
      summaryQuery.Append(" INNER JOIN " + str + " a ON s.CampaignStepId = a.CampaignStepId");
      CampaignProvider.buildWhereClause(userId, contactType, campaignStatusList, activityTypeList, activityDateRange, activityStatusList, summaryQuery);
      summaryQuery.Append(" GROUP BY s.CampaignStepId ");
    }

    private static void buildCampaignActivityQuery(
      CampaignActivityCollectionCriteria criteria,
      DbQueryBuilder activityQuery)
    {
      string str1 = criteria.ContactType == ContactType.Borrower ? "BorrowerCampaignActivity" : "PartnerCampaignActivity";
      string str2 = criteria.ContactType == ContactType.Borrower ? "Borrower" : "BizPartner";
      string str3 = criteria.ContactType == ContactType.Borrower ? "b.HomePhone, b.PersonalEmail, b.NoSpam, b.NoCall, b.NoFax" : "b.WorkPhone, b.BizEmail, b.NoSpam, NULL AS NoCall, NULL AS NoFax";
      activityQuery.Append("SELECT a.*, b.FirstName, b.LastName, " + str3 + " FROM " + str1 + " a INNER JOIN " + str2 + " b ON a.ContactId = b.ContactId WHERE a.CampaignStepId = " + (object) criteria.CampaignStepId);
      if (criteria.ActivityStatuses != null)
        activityQuery.Append(" AND a.Status IN (" + CampaignProvider.encodeEnumArray(typeof (ActivityStatus), (Array) criteria.ActivityStatuses) + ")");
      if (criteria.ActivityDateRange != null)
        activityQuery.Append(" AND a.ScheduledDate BETWEEN " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(criteria.ActivityDateRange[0], false) + " AND " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(criteria.ActivityDateRange[1], false));
      activityQuery.Append(" ORDER BY a.ScheduledDate DESC ");
    }

    private static CampaignInfo convertRowToCampaign(DataRow campaignRow, DataRow contactSummaryRow)
    {
      return new CampaignInfo((int) campaignRow["CampaignId"], (string) campaignRow["UserId"], (string) campaignRow["CampaignName"], EllieMae.EMLite.DataAccess.SQL.Decode(campaignRow["CampaignDesc"], (object) "").ToString(), (CampaignType) campaignRow["CampaignType"], (ContactType) campaignRow["ContactType"], (CampaignStatus) campaignRow["Status"], (CampaignFrequencyType) campaignRow["FrequencyType"], (int) EllieMae.EMLite.DataAccess.SQL.Decode(campaignRow["FrequencyInterval"], (object) 0), (int) EllieMae.EMLite.DataAccess.SQL.Decode(campaignRow["AddQueryId"], (object) 0), (int) EllieMae.EMLite.DataAccess.SQL.Decode(campaignRow["DeleteQueryId"], (object) 0), (int) EllieMae.EMLite.DataAccess.SQL.Decode(campaignRow["ContactGroupId"], (object) 0), (DateTime) campaignRow["CreationTime"], (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(campaignRow["StartedTime"], (object) DateTime.MinValue), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(campaignRow["NextRefreshTime"], (object) DateTime.MaxValue), contactSummaryRow != null ? (int) contactSummaryRow["TotalContacts"] : 0, (CampaignStepInfo[]) null, "4.0");
    }

    private static CampaignStepInfo convertRowToStep(DataRow stepRow, DataRow activitySummaryRow)
    {
      return new CampaignStepInfo((int) stepRow["CampaignStepId"], (int) stepRow["CampaignId"], (int) stepRow["StepNumber"], (string) stepRow["StepName"], EllieMae.EMLite.DataAccess.SQL.Decode(stepRow["StepDesc"], (object) "").ToString(), (int) stepRow["StepInterval"], (ActivityType) stepRow["ActivityType"], EllieMae.EMLite.DataAccess.SQL.Decode(stepRow["ActivityUserId"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(stepRow["DocumentId"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(stepRow["Subject"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(stepRow["Comments"], (object) "").ToString(), (TaskPriority) EllieMae.EMLite.DataAccess.SQL.Decode(stepRow["TaskPriority"], (object) 0), Color.FromArgb((int) EllieMae.EMLite.DataAccess.SQL.Decode(stepRow["BarColor"], (object) 0)), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(stepRow["LastActivityDate"], (object) DateTime.MinValue), activitySummaryRow != null ? (int) activitySummaryRow["TasksDueCount"] : 0, (CampaignActivityInfo[]) null);
    }

    private static CampaignActivityInfo convertRowToActivity(DataRow activityRow)
    {
      return new CampaignActivityInfo((int) activityRow["CampaignStepId"], (int) activityRow["ContactId"], (DateTime) activityRow["CreatedDate"], (DateTime) activityRow["ScheduledDate"], (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(activityRow["CompletedDate"], (object) DateTime.MinValue), (ActivityStatus) activityRow["Status"]);
    }

    private static CampaignContactInfo convertRowToContact(DataRow contactRow)
    {
      return new CampaignContactInfo((int) contactRow["ContactId"], (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(contactRow["CreatedDate"], (object) DateTime.MinValue), (CampaignActivityInfo[]) null);
    }

    private static CampaignTasksDueInfo convertRowToCampaignTasksDue(DataRow tasksDueRow)
    {
      return new CampaignTasksDueInfo((int) tasksDueRow["CampaignId"], (string) tasksDueRow["CampaignName"], (int) tasksDueRow["TasksDueCount"]);
    }

    private static void addContactProperties(
      CampaignInfo campaign,
      CampaignContactCollectionCritera criteria,
      DataRow contactRow,
      Dictionary<string, object> contactProperties)
    {
      foreach (string field in criteria.FieldList)
      {
        if (field.ToLower().StartsWith("contact."))
          contactProperties.Add(field, contactRow[field.Substring("contact.".Length)]);
        else if (field.ToLower().StartsWith("contactgroupcount."))
          contactProperties.Add(field, contactRow[field.Substring("contactgroupcount.".Length)]);
      }
    }

    private static void addContactProperties(
      CampaignActivityCollectionCriteria criteria,
      DataRow contactRow,
      Dictionary<string, object> contactProperties)
    {
      contactProperties.Add("Contact.ContactId", (object) (int) contactRow["ContactId"]);
      contactProperties.Add("Contact.FirstName", (object) (string) EllieMae.EMLite.DataAccess.SQL.Decode(contactRow["FirstName"], (object) string.Empty));
      contactProperties.Add("Contact.LastName", (object) (string) EllieMae.EMLite.DataAccess.SQL.Decode(contactRow["LastName"], (object) string.Empty));
      contactProperties.Add("Contact.NoSpam", (object) (bool) EllieMae.EMLite.DataAccess.SQL.Decode(contactRow["NoSpam"], (object) false));
      contactProperties.Add("Contact.NoCall", (object) (bool) EllieMae.EMLite.DataAccess.SQL.Decode(contactRow["NoCall"], (object) false));
      contactProperties.Add("Contact.NoFax", (object) (bool) EllieMae.EMLite.DataAccess.SQL.Decode(contactRow["NoFax"], (object) false));
      if (criteria.ContactType == ContactType.Borrower)
      {
        contactProperties.Add("Contact.HomePhone", (object) (string) EllieMae.EMLite.DataAccess.SQL.Decode(contactRow["HomePhone"], (object) string.Empty));
        contactProperties.Add("Contact.PersonalEmail", (object) (string) EllieMae.EMLite.DataAccess.SQL.Decode(contactRow["PersonalEmail"], (object) string.Empty));
      }
      else
      {
        contactProperties.Add("Contact.WorkPhone", (object) (string) EllieMae.EMLite.DataAccess.SQL.Decode(contactRow["WorkPhone"], (object) string.Empty));
        contactProperties.Add("Contact.BizEmail", (object) (string) EllieMae.EMLite.DataAccess.SQL.Decode(contactRow["BizEmail"], (object) string.Empty));
      }
    }

    private static CampaignHistoryInfo convertRowToHistory(QueryResult res, int index)
    {
      return new CampaignHistoryInfo((int) res[index, "History.CampaignId"], (int) res[index, "History.ContactId"], (int) res[index, "History.CampaignStepNumber"], (string) res[index, "History.CampaignStepName"], ((string) EllieMae.EMLite.DataAccess.SQL.Decode(res[index, "History.EventType"], (object) string.Empty)).Substring("Campaign ".Length).Trim(), (string) EllieMae.EMLite.DataAccess.SQL.Decode(res[index, "History.CampaignActivityDescription"], (object) string.Empty), (DateTime) res[index, "History.TimeOfHistory"], (string) EllieMae.EMLite.DataAccess.SQL.Decode(res[index, "History.Sender"], (object) string.Empty), (int) EllieMae.EMLite.DataAccess.SQL.Decode(res[index, "History.NoteId"], (object) 0), (string) EllieMae.EMLite.DataAccess.SQL.Decode(res[index, "Contact.LastName"], (object) string.Empty), (string) EllieMae.EMLite.DataAccess.SQL.Decode(res[index, "Contact.FirstName"], (object) string.Empty));
    }

    private static DbValueList createDbValueList(CampaignInfo info)
    {
      return new DbValueList()
      {
        {
          "UserId",
          (object) info.UserId,
          (IDbEncoder) DbEncoding.EmptyStringAsNull
        },
        {
          "CampaignName",
          (object) info.CampaignName,
          (IDbEncoder) DbEncoding.EmptyStringAsNull
        },
        {
          "CampaignDesc",
          (object) info.CampaignDesc
        },
        {
          "CampaignType",
          (object) (int) info.CampaignType
        },
        {
          "ContactType",
          (object) (int) info.ContactType
        },
        {
          "Status",
          (object) (int) info.Status
        },
        {
          "FrequencyType",
          (object) (int) info.FrequencyType
        },
        {
          "FrequencyInterval",
          (object) info.FrequencyInterval
        },
        {
          "AddQueryId",
          (object) "@addQueryId",
          (IDbEncoder) DbEncoding.None
        },
        {
          "DeleteQueryId",
          (object) "@deleteQueryId",
          (IDbEncoder) DbEncoding.None
        },
        {
          "ContactGroupId",
          (object) "@contactGroupId",
          (IDbEncoder) DbEncoding.None
        },
        {
          "StartedTime",
          (object) info.StartedTime
        }
      };
    }

    private static DbValueList createDbValueList(CampaignStepInfo info)
    {
      return new DbValueList()
      {
        {
          "CampaignId",
          (object) "@campaignId",
          (IDbEncoder) DbEncoding.None
        },
        {
          "StepNumber",
          (object) info.StepNumber
        },
        {
          "StepName",
          (object) info.StepName,
          (IDbEncoder) DbEncoding.EmptyStringAsNull
        },
        {
          "StepDesc",
          (object) info.StepDesc
        },
        {
          "StepInterval",
          (object) info.StepInterval
        },
        {
          "ActivityType",
          (object) (int) info.ActivityType
        },
        {
          "DocumentId",
          (object) info.DocumentId
        },
        {
          "Subject",
          (object) info.Subject
        },
        {
          "Comments",
          (object) info.Comments
        },
        {
          "TaskPriority",
          (object) (int) info.TaskPriority
        },
        {
          "BarColor",
          (object) info.BarColor.ToArgb()
        },
        {
          "ActivityUserId",
          (object) info.ActivityUserId
        }
      };
    }

    private static string buildCampaignLevelHistory(
      CampaignInfo campaign,
      string eventType,
      string dateField)
    {
      string str1 = campaign.ContactType == ContactType.Borrower ? "BorrowerHistory" : "BizPartnerHistory";
      string str2 = campaign.ContactType == ContactType.Borrower ? "BorrowerCampaignActivity" : "PartnerCampaignActivity";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(" SELECT @CampaignStepId = CampaignStepId FROM CampaignStep WHERE CampaignId = " + (object) campaign.CampaignId);
      stringBuilder.Append(" INSERT INTO " + str1);
      stringBuilder.Append(" SELECT DISTINCT a.ContactId, @CurrentDate AS TimeOfHistory, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) eventType) + " AS EventType, NULL AS LoanId, NULL AS LetterName, c.UserId AS Sender, NULL AS Subject, NULL AS ContactSource, c.CampaignName, NULL AS CampaignStepName, NULL AS CampaignActivityStatus, NULL AS CampaignScheduledDate, c.CampaignId, NULL AS CampaignStepNumber, NULL AS NoteId");
      stringBuilder.Append(" FROM Campaign c INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId INNER JOIN " + str2 + " a ON s.CampaignStepId = a.CampaignStepId");
      stringBuilder.Append(" WHERE c.CampaignId = " + (object) campaign.CampaignId + " AND s.CampaignStepId = @CampaignStepId AND a." + dateField + " = @CurrentDate");
      if (eventType.Equals("Campaign Complete"))
        stringBuilder.Append(" AND a.ContactId NOT IN (SELECT DISTINCT a.ContactId FROM Campaign c INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId INNER JOIN " + str2 + " a ON s.CampaignStepId = a.CampaignStepId WHERE c.CampaignId = " + (object) campaign.CampaignId + " AND a.Status = " + Enum.Format(typeof (ActivityStatus), (object) ActivityStatus.Expected, "D") + ")");
      return stringBuilder.ToString();
    }

    private static string buildActivityLevelHistory(
      CampaignInfo campaign,
      ActivityStatus activityStatus,
      CampaignProvider.IncrementalIntArray contactIds)
    {
      return CampaignProvider.buildActivityLevelHistory(campaign, activityStatus, contactIds, (ContactQueryInfo) null, (User) null);
    }

    private static string buildActivityLevelHistory(
      CampaignInfo campaign,
      ActivityStatus activityStatus,
      ContactQueryInfo deleteQuery,
      User user)
    {
      return CampaignProvider.buildActivityLevelHistory(campaign, activityStatus, (CampaignProvider.IncrementalIntArray) null, deleteQuery, user);
    }

    private static string buildActivityLevelHistory(
      CampaignInfo campaign,
      ActivityStatus activityStatus,
      CampaignProvider.IncrementalIntArray contactIds,
      ContactQueryInfo deleteQuery,
      User user)
    {
      string str1 = campaign.ContactType == ContactType.Borrower ? "BorrowerHistory" : "BizPartnerHistory";
      string str2 = campaign.ContactType == ContactType.Borrower ? "BorrowerCampaignActivity" : "PartnerCampaignActivity";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("INSERT INTO " + str1 + " SELECT a.ContactId, @CurrentDate AS TimeOfHistory, 'EventType' = CASE s.ActivityType WHEN 0 THEN 'Campaign Email' WHEN 1 THEN 'Campaign Fax' WHEN 2 THEN 'Campaign Letter' WHEN 3 THEN 'Campaign Phone Call' WHEN 4 THEN 'Campaign Reminder' ELSE 'Campaign Activity' END, NULL AS LoanId, s.DocumentId AS LetterName, c.UserId AS Sender, s.Subject, NULL AS ContactSource, c.CampaignName, s.StepName AS CampaignStepName, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) new ActivityStatusNameProvider().GetName((object) activityStatus)) + " AS CampaignActivityStatus, a.ScheduledDate AS CampaignScheduledDate, c.CampaignId, s.StepNumber AS CampaignStepNumber, NULL AS NoteId FROM Campaign c");
      stringBuilder.Append(" INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId INNER JOIN " + str2 + " a ON s.CampaignStepId = a.CampaignStepId");
      if (contactIds != null)
        stringBuilder.Append(" WHERE c.CampaignId = " + (object) campaign.CampaignId + " AND a.ContactId IN " + CampaignProvider.encodeIntArray(contactIds) + " AND a.Status = " + Enum.Format(typeof (ActivityStatus), (object) activityStatus, "D") + " AND a.CompletedDate = @CurrentDate");
      else if (deleteQuery != null)
      {
        stringBuilder.Append(" WHERE c.CampaignId = " + (object) campaign.CampaignId + " AND a.ContactId IN (");
        if ((ContactQueryType.CampaignPredefinedQuery & deleteQuery.QueryType) == ContactQueryType.CampaignPredefinedQuery)
          stringBuilder.Append(ContactGroupProvider.BuildPredefinedQueryString(user, deleteQuery, true));
        else
          stringBuilder.Append(ContactGroupProvider.BuildAdvancedQueryResultString(user, deleteQuery, true));
        stringBuilder.Append(") AND a.CompletedDate = @CurrentDate");
      }
      return stringBuilder.ToString();
    }

    private static string buildActivityLevelHistory(
      CampaignInfo campaign,
      CampaignStepInfo campaignStep,
      int noteId,
      ActivityStatus activityStatus,
      CampaignProvider.IncrementalIntArray contactIds)
    {
      string historyTable = campaign.ContactType == ContactType.Borrower ? "BorrowerHistory" : "BizPartnerHistory";
      string str = campaign.ContactType == ContactType.Borrower ? "BorrowerCampaignActivity" : "PartnerCampaignActivity";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(CampaignProvider.buildCommonActivityHistory(historyTable, noteId, campaignStep.ActivityType, activityStatus));
      stringBuilder.Append(" INNER JOIN CampaignStep s ON c.CampaignId = s.CampaignId INNER JOIN " + str + " a ON s.CampaignStepId = a.CampaignStepId");
      stringBuilder.Append(" WHERE s.CampaignStepId = " + (object) campaignStep.CampaignStepId + " AND a.ContactId IN " + CampaignProvider.encodeIntArray(contactIds) + " AND a.Status = " + Enum.Format(typeof (ActivityStatus), (object) activityStatus, "D") + " AND a.CompletedDate = @CurrentDate");
      return stringBuilder.ToString();
    }

    private static string buildCommonActivityHistory(
      string historyTable,
      int noteId,
      ActivityType activityType,
      ActivityStatus activityStatus)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("INSERT INTO " + historyTable + " SELECT a.ContactId, @CurrentDate AS TimeOfHistory, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) ("Campaign " + new ActivityTypeNameProvider().GetName((object) activityType))) + " AS EventType, NULL AS LoanId, s.DocumentId AS LetterName, c.UserId AS Sender, s.Subject, NULL AS ContactSource, c.CampaignName, s.StepName AS CampaignStepName, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) new ActivityStatusNameProvider().GetName((object) activityStatus)) + " AS CampaignActivityStatus, a.ScheduledDate AS CampaignScheduledDate, c.CampaignId, s.StepNumber AS CampaignStepNumber, " + (0 < noteId ? noteId.ToString() : "NULL") + " AS NoteId FROM Campaign c");
      return stringBuilder.ToString();
    }

    public class IncrementalIntArray
    {
      private int increment;
      public int Start;
      public int Count;
      public int[] IntArray;

      public IncrementalIntArray(int increment, int[] intArray)
      {
        this.increment = increment;
        this.IntArray = intArray;
        this.Reset();
      }

      public void Reset()
      {
        this.Start = 0;
        this.Count = Math.Min(this.increment, this.IntArray.Length - this.Start);
      }

      public bool Increment()
      {
        this.Start += this.Count;
        this.Count = Math.Min(this.increment, this.IntArray.Length - this.Start);
        return this.Count != 0;
      }
    }
  }
}

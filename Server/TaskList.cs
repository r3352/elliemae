// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.TaskList
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.TaskList;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class TaskList
  {
    private const string className = "TaskList�";

    private TaskList()
    {
    }

    public static TaskInfo[] GetAllTasksForUser(string userid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select * from TaskList");
      if (userid != null)
        dbQueryBuilder.Append(" where userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userid));
      dbQueryBuilder.Append(" order by creationTime desc");
      return EllieMae.EMLite.Server.TaskList.convertDataRowsToTasks(dbQueryBuilder.Execute());
    }

    private static TaskInfo[] convertDataRowsToTasks(DataRowCollection rows)
    {
      if (rows == null)
        return (TaskInfo[]) null;
      TaskInfo[] tasks = new TaskInfo[rows.Count];
      for (int index1 = 0; index1 < rows.Count; ++index1)
      {
        int taskID = (int) rows[index1]["taskID"];
        string userid = (string) rows[index1]["userid"];
        TaskType taskType = (TaskType) rows[index1]["taskType"];
        TaskStatus taskStatus = (TaskStatus) rows[index1]["taskStatus"];
        string subject = (string) rows[index1]["subject"];
        string notes = (string) rows[index1]["notes"];
        object obj1 = rows[index1]["dueDate"];
        DateTime dueDate = obj1 == DBNull.Value ? DateTime.MaxValue : (DateTime) obj1;
        object obj2 = rows[index1]["startDate"];
        DateTime startDate = obj2 == DBNull.Value ? DateTime.MinValue : (DateTime) obj2;
        TaskPriority priority = (TaskPriority) rows[index1]["priority"];
        DateTime creationTime = (DateTime) rows[index1]["creationTime"];
        DateTime lastModified = (DateTime) rows[index1]["lastModified"];
        ArrayList arrayList = new ArrayList();
        int campaignStepID = -1;
        string campaignInfo = "";
        DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
        dbQueryBuilder1.Append("SELECT B.ContactID, B.FirstName, B.LastName, T.CampaignStepID, C.CampaignName, CS.StepName FROM Borrower AS B inner join TaskBorrowerContact AS T on B.ContactID = T.contactID left outer join CampaignStep CS on T.CampaignStepID = CS.CampaignStepID left outer join Campaign C on CS.CampaignID = C.CampaignID WHERE T.taskID = " + (object) taskID);
        DataRowCollection dataRowCollection1 = dbQueryBuilder1.Execute();
        if (dataRowCollection1 != null)
        {
          for (int index2 = 0; index2 < dataRowCollection1.Count; ++index2)
          {
            ContactInfo contactInfo = new ContactInfo((string) dataRowCollection1[index2]["FirstName"] + " " + (string) dataRowCollection1[index2]["LastName"], string.Concat((object) (int) dataRowCollection1[index2]["ContactID"]), CategoryType.Borrower);
            arrayList.Add((object) contactInfo);
            if (string.Concat(dataRowCollection1[index2]["CampaignStepID"]) != "-1")
            {
              campaignStepID = int.Parse(string.Concat(dataRowCollection1[index2]["CampaignStepID"]));
              campaignInfo = dataRowCollection1[index2]["CampaignName"].ToString() + " - " + dataRowCollection1[index2]["StepName"];
            }
          }
        }
        DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
        dbQueryBuilder2.Append("SELECT B.ContactID, B.FirstName, B.LastName, T.CampaignStepID, C.CampaignName, CS.StepName FROM BizPartner AS B inner join TaskBizContact AS T on B.ContactID = T.contactID left outer join CampaignStep CS on T.CampaignStepID = CS.CampaignStepID left outer join Campaign C on CS.CampaignID = C.CampaignID WHERE T.taskID = " + (object) taskID);
        DataRowCollection dataRowCollection2 = dbQueryBuilder2.Execute();
        if (dataRowCollection2 != null)
        {
          for (int index3 = 0; index3 < dataRowCollection2.Count; ++index3)
          {
            ContactInfo contactInfo = new ContactInfo((string) dataRowCollection2[index3]["FirstName"] + " " + (string) dataRowCollection2[index3]["LastName"], string.Concat((object) (int) dataRowCollection2[index3]["ContactID"]), CategoryType.BizPartner);
            arrayList.Add((object) contactInfo);
            if (string.Concat(dataRowCollection2[index3]["CampaignStepID"]) != "-1")
            {
              campaignStepID = int.Parse(string.Concat(dataRowCollection2[index3]["CampaignStepID"]));
              campaignInfo = dataRowCollection2[index3]["CampaignName"].ToString() + " - " + dataRowCollection2[index3]["StepName"];
            }
          }
        }
        tasks[index1] = new TaskInfo(taskID, userid, taskType, taskStatus, subject, notes, dueDate, startDate, priority, creationTime, lastModified, (ContactInfo[]) arrayList.ToArray(typeof (ContactInfo)), campaignStepID, campaignInfo);
      }
      return tasks;
    }

    public static int InsertOrUpdateTask(TaskInfo task)
    {
      if (task == null)
        return -1;
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      int num = task.TaskID;
      int taskType = (int) task.TaskType;
      string userId = task.UserID;
      string str1 = task.Subject;
      if (str1.Length > 250)
        str1 = str1.Substring(0, 250);
      int taskStatus = (int) task.TaskStatus;
      string str2 = task.Notes;
      if (str2.Length > 1024)
        str2 = str2.Substring(0, 1024);
      DateTime dueDate = task.DueDate;
      DateTime startDate = task.StartDate;
      int priority = (int) task.Priority;
      DateTime creationTime = task.CreationTime;
      DateTime now = DateTime.Now;
      ContactInfo[] contacts = task.Contacts;
      bool flag = false;
      if (num == 0 && task.TaskStatus == TaskStatus.Completed)
        flag = true;
      else if (num > 0 && task.TaskStatus == TaskStatus.Completed)
      {
        dbQueryBuilder1.Append("Select * from TaskList where taskID = " + (object) num + " and taskStatus = " + (object) 2);
        flag = dbQueryBuilder1.ExecuteTableQuery().Rows.Count <= 0;
      }
      if (num == 0)
      {
        dbQueryBuilder1.AppendLine("INSERT INTO TaskList (taskType, userid, subject, taskStatus, notes, dueDate, startDate, priority, creationTime, lastModified) VALUES (" + (object) taskType + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) str1) + "," + (object) taskStatus + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) str2) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(dueDate, DateTime.MaxValue, true) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(startDate, DateTime.MinValue, true) + "," + (object) priority + "," + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(creationTime) + "," + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(now) + ")");
        dbQueryBuilder1.SelectIdentity();
        num = Convert.ToInt32(dbQueryBuilder1.ExecuteScalar());
      }
      else
      {
        dbQueryBuilder1.AppendLine("UPDATE TaskList SET subject = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) str1) + ", taskStatus = " + (object) taskStatus + ", notes = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) str2) + ", dueDate = " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(dueDate, DateTime.MaxValue, true) + ", startDate = " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(startDate, DateTime.MinValue, true) + ", priority = " + (object) priority + ", lastModified = " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(now) + " WHERE taskID = " + (object) num);
        dbQueryBuilder1.AppendLine("DELETE FROM TaskBorrowerContact WHERE taskID = " + (object) num);
        dbQueryBuilder1.AppendLine("DELETE FROM TaskBiZContact WHERE taskID = " + (object) num);
        dbQueryBuilder1.ExecuteNonQuery();
      }
      DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
      if (contacts != null && contacts.Length != 0)
      {
        for (int index = 0; index < contacts.Length; ++index)
        {
          if (contacts[index].ContactType == CategoryType.Borrower)
            dbQueryBuilder2.AppendLine("INSERT INTO TaskBorrowerContact (taskID, contactID, CampaignStepID) VALUES (" + (object) num + "," + contacts[index].ContactID + "," + (object) task.CampaignStepID + ")");
          else
            dbQueryBuilder2.AppendLine("INSERT INTO TaskBizContact (taskID, contactID, CampaignStepID) VALUES (" + (object) num + "," + contacts[index].ContactID + "," + (object) task.CampaignStepID + ")");
        }
      }
      if (dbQueryBuilder2.ToString() != null && dbQueryBuilder2.ToString() != "")
        dbQueryBuilder2.ExecuteNonQuery();
      if (flag && task.CampaignStepID > 0 && task.TaskType == TaskType.CampaignTask && task.TaskStatus == TaskStatus.Completed)
      {
        List<int> intList = new List<int>();
        foreach (ContactInfo contactInfo in contacts)
        {
          if (!intList.Contains(int.Parse(contactInfo.ContactID)))
            intList.Add(int.Parse(contactInfo.ContactID));
        }
        if (intList.Count > 0)
        {
          CampaignStepInfo campaignStepInfo = CampaignProvider.GetCampaignStepInfo(task.CampaignStepID);
          CampaignInfo campaign = CampaignProvider.GetCampaign(campaignStepInfo.CampaignId);
          ActivityStatusParameter activityStatusParameter = new ActivityStatusParameter(ActivityStatus.Completed, intList.ToArray());
          ActivityUpdateParameter activityUpdateParameter = new ActivityUpdateParameter(task.Notes, new ActivityStatusParameter[1]
          {
            activityStatusParameter
          });
          CampaignProvider.updateActivityStatus(campaign, campaignStepInfo, activityUpdateParameter);
        }
      }
      return num;
    }

    public static void DeleteTask(int taskID)
    {
      EllieMae.EMLite.Server.TaskList.DeleteTasks(new int[1]
      {
        taskID
      });
    }

    public static void DeleteTasks(int[] taskIDs)
    {
      if (taskIDs == null || taskIDs.Length == 0)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      for (int index = 0; index < taskIDs.Length; ++index)
        dbQueryBuilder.AppendLine("DELETE FROM TaskList WHERE taskID = " + (object) taskIDs[index]);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static TaskInfo GetTask(int taskId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable(nameof (TaskList)), new DbValue("taskID", (object) taskId));
      TaskInfo[] tasks = EllieMae.EMLite.Server.TaskList.convertDataRowsToTasks(dbQueryBuilder.Execute());
      return tasks.Length == 0 ? (TaskInfo) null : tasks[0];
    }

    public static TaskInfo[] QueryTasks(QueryCriterion criteria, bool fromHomePage)
    {
      DbQueryBuilder dbQueryBuilder = !fromHomePage ? new DbQueryBuilder() : new DbQueryBuilder(DBReadReplicaFeature.HomePage);
      dbQueryBuilder.Append("select * from TaskList Task");
      if (criteria != null)
        dbQueryBuilder.Append(" where " + criteria.ToSQLClause());
      return EllieMae.EMLite.Server.TaskList.convertDataRowsToTasks(dbQueryBuilder.Execute());
    }
  }
}

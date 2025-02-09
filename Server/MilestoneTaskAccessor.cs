// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.MilestoneTaskAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class MilestoneTaskAccessor
  {
    private const string className = "MilestoneTaskAccessor�";
    private const string TABLENAME = "MilestoneTasks�";
    private const string mapCacheName = "TaskMapCache�";

    public static string AddMilestoneTask(MilestoneTaskDefinition milestoneTask)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock(nameof (MilestoneTaskAccessor)))
      {
        current.Cache.Remove("TaskMapCache");
        current.Cache.Remove(nameof (MilestoneTaskAccessor));
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        milestoneTask.TaskGUID = Guid.NewGuid().ToString();
        TraceLog.WriteVerbose(nameof (MilestoneTaskAccessor), "MilestoneTaskAccessor.AddMilestoneTask: Creating SQL commands for table 'MilestoneTasks'.");
        dbQueryBuilder.AppendLine("INSERT INTO MilestoneTasks ([taskGUID], [taskName], [taskDescription], [daysToComplete], [taskPriority]) VALUES (" + SQL.Encode((object) milestoneTask.TaskGUID) + "," + SQL.Encode((object) milestoneTask.TaskName) + "," + SQL.Encode((object) milestoneTask.TaskDescription) + "," + (object) milestoneTask.DaysToComplete + "," + (object) (int) milestoneTask.TaskPriority + ")");
        try
        {
          dbQueryBuilder.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MilestoneTaskAccessor), ex);
          throw new Exception("Cannot add a new milestone task to MilestoneTasks table Due to the following problem:\r\n" + ex.Message);
        }
        return milestoneTask.TaskGUID;
      }
    }

    public static Hashtable GetMilestoneTasks()
    {
      Hashtable milestoneTasks1 = new Hashtable();
      MilestoneTaskDefinition[] milestoneTasks2 = MilestoneTaskAccessor.GetMilestoneTasks((string[]) null);
      if (milestoneTasks2 != null)
      {
        foreach (MilestoneTaskDefinition milestoneTaskDefinition in milestoneTasks2)
        {
          if (!milestoneTasks1.ContainsKey((object) milestoneTaskDefinition.TaskGUID))
            milestoneTasks1.Add((object) milestoneTaskDefinition.TaskGUID, (object) milestoneTaskDefinition);
        }
      }
      return milestoneTasks1;
    }

    [PgReady]
    public static MilestoneTaskDefinition[] GetMilestoneTasks(string[] taskGUIDs)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        string text = "SELECT * FROM MilestoneTasks";
        if (taskGUIDs != null && taskGUIDs.Length != 0)
        {
          text += " WHERE ";
          for (int index = 0; index < taskGUIDs.Length; ++index)
          {
            if (index > 0)
              text += " OR ";
            text = text + "taskGUID = " + SQL.Encode((object) taskGUIDs[index]);
          }
        }
        pgDbQueryBuilder.AppendLine(text);
        ArrayList arrayList = new ArrayList();
        try
        {
          DataTable dataTable = pgDbQueryBuilder.ExecuteTableQuery();
          for (int index = 0; index < dataTable.Rows.Count; ++index)
          {
            DataRow row = dataTable.Rows[index];
            arrayList.Add((object) new MilestoneTaskDefinition(row));
          }
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MilestoneTaskAccessor), ex);
          throw new Exception("Cannot read records from MilestoneTasks table.\r\n" + ex.Message);
        }
        return arrayList.Count == 0 ? (MilestoneTaskDefinition[]) null : (MilestoneTaskDefinition[]) arrayList.ToArray(typeof (MilestoneTaskDefinition));
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text1 = "SELECT * FROM MilestoneTasks";
      if (taskGUIDs != null && taskGUIDs.Length != 0)
      {
        text1 += " WHERE ";
        for (int index = 0; index < taskGUIDs.Length; ++index)
        {
          if (index > 0)
            text1 += " OR ";
          text1 = text1 + "taskGUID = " + SQL.Encode((object) taskGUIDs[index]);
        }
      }
      dbQueryBuilder.AppendLine(text1);
      ArrayList arrayList1 = new ArrayList();
      try
      {
        DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
        for (int index = 0; index < dataTable.Rows.Count; ++index)
        {
          DataRow row = dataTable.Rows[index];
          arrayList1.Add((object) new MilestoneTaskDefinition(row));
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MilestoneTaskAccessor), ex);
        throw new Exception("Cannot read records from MilestoneTasks table.\r\n" + ex.Message);
      }
      return arrayList1.Count == 0 ? (MilestoneTaskDefinition[]) null : (MilestoneTaskDefinition[]) arrayList1.ToArray(typeof (MilestoneTaskDefinition));
    }

    public static void UpdateMilestoneTask(MilestoneTaskDefinition milestoneTask)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock(nameof (MilestoneTaskAccessor)))
      {
        current.Cache.Remove("TaskMapCache");
        current.Cache.Remove(nameof (MilestoneTaskAccessor));
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        try
        {
          string text = "UPDATE MilestoneTasks SET [taskName] = " + SQL.Encode((object) milestoneTask.TaskName) + ",[taskDescription] = " + SQL.Encode((object) milestoneTask.TaskDescription) + ",[daysToComplete] = " + (object) milestoneTask.DaysToComplete + ",[taskPriority] = " + (object) (int) milestoneTask.TaskPriority + " WHERE taskGUID = " + SQL.Encode((object) milestoneTask.TaskGUID);
          dbQueryBuilder.AppendLine(text);
          dbQueryBuilder.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MilestoneTaskAccessor), ex);
          throw new Exception("Cannot update milestone task in MilestoneTasks table.\r\n" + ex.Message);
        }
      }
    }

    public static void DeleteMilestoneTasks(string[] taskGUIDs)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock(nameof (MilestoneTaskAccessor)))
      {
        current.Cache.Remove("TaskMapCache");
        current.Cache.Remove(nameof (MilestoneTaskAccessor));
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        try
        {
          string text = "DELETE FROM MilestoneTasks Where [taskGUID] = " + SQL.Encode((object) taskGUIDs[0]);
          if (taskGUIDs.Length > 1)
          {
            for (int index = 1; index < taskGUIDs.Length; ++index)
              text = text + " Or [taskGUID] = " + SQL.Encode((object) taskGUIDs[index]);
          }
          dbQueryBuilder.AppendLine(text);
          dbQueryBuilder.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (MilestoneTaskAccessor), ex);
          throw new Exception("Cannot delete milestone task from MilestoneTasks.\r\n" + ex.Message);
        }
      }
    }

    public static bool IsDuplicatedMilestoneTasks(string newTaskName)
    {
      string text = "SELECT count(taskGUID) FROM MilestoneTasks WHERE [taskName] = " + SQL.Encode((object) newTaskName);
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine(text);
        return dbQueryBuilder.ExecuteScalar() is int num && num > 0;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public static Hashtable GetTaskXRefMap(XRefKeyType keyType)
    {
      ClientContext current = ClientContext.GetCurrent();
      Hashtable hashtable = (Hashtable) current.Cache.Get("TaskMapCache");
      if (hashtable != null)
      {
        Hashtable taskXrefMap = (Hashtable) hashtable[(object) keyType];
        if (taskXrefMap != null)
          return taskXrefMap;
      }
      using (current.Cache.Lock("TaskMapCache"))
      {
        Hashtable o = (Hashtable) current.Cache.Get("TaskMapCache");
        if (o != null)
        {
          Hashtable taskXrefMap = (Hashtable) o[(object) keyType];
          if (taskXrefMap != null)
            return taskXrefMap;
        }
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("MilestoneTasks"));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          DataRow dataRow = dataRowCollection[index];
          string key;
          string str;
          if (keyType == XRefKeyType.CustomMilestoneGuid)
          {
            key = dataRow["taskGUID"].ToString();
            str = dataRow["taskName"].ToString();
          }
          else
          {
            if (keyType != XRefKeyType.CustomMilestoneName)
              throw new Exception("Invalid XRefKeyType");
            str = dataRow["taskGUID"].ToString();
            key = dataRow["taskName"].ToString();
          }
          if (!insensitiveHashtable.ContainsKey((object) key))
            insensitiveHashtable.Add((object) key, (object) str);
        }
        if (current.Settings.CacheSetting >= CacheSetting.Low)
        {
          if (o == null)
            o = new Hashtable();
          o.Add((object) keyType, (object) insensitiveHashtable);
          current.Cache.Put("TaskMapCache", (object) o);
        }
        return insensitiveHashtable;
      }
    }
  }
}

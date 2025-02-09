// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.SettingsReportAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class SettingsReportAccessor
  {
    public static DataRowCollection GetSettingsReportJobsToProcess()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from SettingsRptQueue");
      dbQueryBuilder.AppendLine("where status = 0");
      dbQueryBuilder.AppendLine("order by CreateDate");
      return dbQueryBuilder.Execute();
    }

    public SettingsRptJobInfo[] GetSettingsRptJobsDELETE()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT reportID, reportName, jobtype, ID, Description, Status, Message, CreatedBy, CreateDate, FileGuid FROM [SettingsRptQueue] order by ReportID desc";
      dbQueryBuilder.AppendLine(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      SettingsRptJobInfo[] settingsRptJobsDelete = new SettingsRptJobInfo[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
      {
        DataRow dataRow = dataRowCollection[index];
        SettingsRptJobInfo.jobStatus int16_1 = (SettingsRptJobInfo.jobStatus) Convert.ToInt16(dataRowCollection[index]["status"].ToString());
        SettingsRptJobInfo.jobType int16_2 = (SettingsRptJobInfo.jobType) Convert.ToInt16(dataRowCollection[index]["jobtype"].ToString());
        settingsRptJobsDelete[index] = new SettingsRptJobInfo(int16_2, dataRowCollection[index]["reportname"].ToString(), int16_1, dataRowCollection[index]["createdby"].ToString(), dataRowCollection[index]["createdate"].ToString(), Convert.ToString(dataRowCollection[index]["fileguid"]));
        settingsRptJobsDelete[index].ReportID = dataRowCollection[index]["reportID"].ToString();
        settingsRptJobsDelete[index].Message = dataRowCollection[index]["message"].ToString();
      }
      return settingsRptJobsDelete;
    }

    public static void UpdateReportStatus(int status, int reportID, string message)
    {
      DateTime utcNow = DateTime.UtcNow;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update SettingsRptQueue set status = " + (object) status + ", message=" + SQL.EncodeString(message) + ", LastUpdateDt=" + SQL.EncodeDateTime(utcNow) + " where ReportID = " + (object) reportID ?? "");
      dbQueryBuilder.ExecuteNonQuery(TimeSpan.FromMinutes(10.0), DbTransactionType.None);
    }

    public static void UpdateJobWithXML(string XMLstr, string fileGuid)
    {
      try
      {
        SettingsReportStore.Create(fileGuid, XMLstr);
      }
      catch (Exception ex)
      {
        string message = "Failed to generate xml for " + fileGuid + ": " + ex.Message + " xml data:" + XMLstr;
        if (EllieMae.EMLite.DataAccess.ServerGlobals.CurrentContextTraceLog != null)
          EllieMae.EMLite.DataAccess.ServerGlobals.CurrentContextTraceLog.Write(message, nameof (SettingsReportAccessor));
        if (ServerGlobals.TraceLog == null)
          return;
        ServerGlobals.TraceLog.WriteError(nameof (SettingsReportAccessor), message);
      }
    }

    private static string createSettingsRptJob(SettingsRptJobInfo jobinfo)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text1 = "INSERT INTO [SettingsRptQueue] (jobtype, reportname, status, createdby, createdate, fileGuid) VALUES(" + SQL.Encode((object) (int) jobinfo.Type) + "," + SQL.Encode((object) jobinfo.ReportName) + "," + SQL.Encode((object) (int) jobinfo.Status) + "," + SQL.Encode((object) jobinfo.CreatedBy) + "," + SQL.Encode((object) jobinfo.CreateDate) + "," + SQL.Encode((object) jobinfo.FileGuid) + ") ";
      dbQueryBuilder.AppendLine(text1);
      string text2 = "SELECT max(reportID) from [SettingsRptQueue]";
      dbQueryBuilder.AppendLine(text2);
      return dbQueryBuilder.ExecuteScalar().ToString();
    }

    public static void saveReportParameters(
      string reportID,
      Dictionary<string, string> reportParameters)
    {
      foreach (KeyValuePair<string, string> reportParameter in reportParameters)
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        string text = "INSERT INTO [SettingsRptParameters] (reportID, name, value) VALUES(" + SQL.Encode((object) reportID) + "," + SQL.Encode((object) reportParameter.Key) + "," + SQL.Encode((object) reportParameter.Value) + ") ";
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    public static void saveReportFilters(
      string reportID,
      List<string> reportFilters,
      SettingsRptJobInfo.jobType jobType)
    {
      string str = "";
      switch (jobType)
      {
        case SettingsRptJobInfo.jobType.Organization:
          str = "oid";
          break;
        case SettingsRptJobInfo.jobType.Persona:
          str = "persona";
          break;
      }
      foreach (string reportFilter in reportFilters)
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        string text = "INSERT INTO [SettingsRptFilters] (ReportID, Name, Value) VALUES(" + SQL.Encode((object) reportID) + "," + SQL.Encode((object) str) + "," + SQL.Encode((object) reportFilter) + ") ";
        dbQueryBuilder.AppendLine(text);
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    public static Dictionary<string, string> getReportParameters(int reportID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT  name, value from  [SettingsRptParameters] WHERE reportID=" + (object) reportID;
      dbQueryBuilder.AppendLine(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Dictionary<string, string> reportParameters = new Dictionary<string, string>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        reportParameters[dataRow["name"].ToString()] = dataRow["value"].ToString();
      return reportParameters;
    }

    public static List<string> getReportFilters(int reportID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT  name, value from  [SettingsRptFilters] WHERE reportID=" + (object) reportID;
      dbQueryBuilder.AppendLine(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      List<string> reportFilters = new List<string>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        reportFilters.Add(dataRow["value"].ToString());
      return reportFilters;
    }

    public static string CreateSettingsRptJob(SettingsRptJobInfo jobinfo)
    {
      return SettingsReportAccessor.createSettingsRptJob(jobinfo);
    }

    public static IDictionary getServerSettings(string category)
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      foreach (DictionaryEntry companySetting in Company.GetCompanySettings(category))
      {
        string key = category + "." + companySetting.Key;
        insensitiveHashtable[(object) key] = companySetting.Value;
      }
      return (IDictionary) insensitiveHashtable;
    }

    public static string getXMLSettingsRpt(SettingsRptJobInfo jobInfo)
    {
      string xmlData = (string) null;
      try
      {
        xmlData = SettingsReportStore.GetFileData(jobInfo.FileGuid);
        if (string.IsNullOrEmpty(xmlData))
        {
          DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
          string text = "SELECT XMLReport FROM [SettingsRptQueue] WHERE ReportID = " + SQL.Encode((object) jobInfo.ReportID);
          dbQueryBuilder1.AppendLine(text);
          xmlData = dbQueryBuilder1.ExecuteScalar().ToString();
          if (!string.IsNullOrEmpty(xmlData))
          {
            DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
            string str = Guid.NewGuid().ToString();
            dbQueryBuilder2.AppendLine("UPDATE SettingsRptQueue SET FileGuid = " + SQL.Encode((object) str) + " WHERE ReportID = " + SQL.Encode((object) jobInfo.ReportID));
            dbQueryBuilder2.ExecuteNonQuery();
            jobInfo.FileGuid = str;
            SettingsReportStore.Create(jobInfo.FileGuid, xmlData);
          }
        }
        return xmlData;
      }
      catch (Exception ex)
      {
        string message = "Failed to generate xml for fileguid- " + jobInfo.FileGuid + "/reportid- " + jobInfo.ReportID + " : " + ex.Message + " xml data:" + xmlData;
        if (EllieMae.EMLite.DataAccess.ServerGlobals.CurrentContextTraceLog != null)
          EllieMae.EMLite.DataAccess.ServerGlobals.CurrentContextTraceLog.Write(message, nameof (SettingsReportAccessor));
        if (ServerGlobals.TraceLog != null)
          ServerGlobals.TraceLog.WriteError(nameof (SettingsReportAccessor), message);
        return string.Empty;
      }
    }

    public static SettingsRptJobInfo getSettingsRptInfo(string reportID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT ReportName, JobType, Status, CreatedBy, CreateDate, CancelDt, CancelBy, DeleteBy, DeleteDt, Message, LastUpdateDt, FileGuid FROM [SettingsRptQueue] WHERE reportID = " + SQL.Encode((object) reportID);
      dbQueryBuilder.AppendLine(text);
      DataRow dataRow = dbQueryBuilder.ExecuteRowQuery();
      int type = (int) SQL.Decode(dataRow["JobType"]);
      int status = (int) SQL.Decode(dataRow["Status"]);
      DateTime dateTime1 = Convert.ToDateTime(dataRow["CreateDate"].ToString());
      SettingsRptJobInfo settingsRptInfo = new SettingsRptJobInfo((SettingsRptJobInfo.jobType) type, dataRow["ReportName"].ToString(), (SettingsRptJobInfo.jobStatus) status, dataRow["CreatedBy"].ToString(), dateTime1.ToString(), dataRow["FileGuid"].ToString());
      DateTime dateTime2;
      if (dataRow["CancelBy"].ToString().Length > 0 && dataRow["CancelDt"].ToString().Length > 0)
      {
        settingsRptInfo.CancelBy = dataRow["CancelBy"].ToString();
        SettingsRptJobInfo settingsRptJobInfo = settingsRptInfo;
        dateTime2 = Convert.ToDateTime(dataRow["CancelDt"].ToString());
        string str = dateTime2.ToString();
        settingsRptJobInfo.CancelDt = str;
      }
      if (dataRow["DeleteBy"].ToString().Length > 0 && dataRow["DeleteDt"].ToString().Length > 0)
      {
        settingsRptInfo.DeleteBy = dataRow["DeleteBy"].ToString();
        SettingsRptJobInfo settingsRptJobInfo = settingsRptInfo;
        dateTime2 = Convert.ToDateTime(dataRow["DeleteDt"].ToString());
        string str = dateTime2.ToString();
        settingsRptJobInfo.DeleteDt = str;
      }
      if (dataRow["Message"].ToString().Length > 0)
        settingsRptInfo.Message = dataRow["Message"].ToString();
      if (dataRow["LastUpdateDt"].ToString().Length > 0)
      {
        SettingsRptJobInfo settingsRptJobInfo = settingsRptInfo;
        dateTime2 = Convert.ToDateTime(dataRow["LastUpdateDt"].ToString());
        string str = dateTime2.ToString();
        settingsRptJobInfo.LastUpdateDt = str;
      }
      settingsRptInfo.ReportID = reportID;
      return settingsRptInfo;
    }

    public static SettingsRptJobInfo[] GetSettingsRptJobs()
    {
      return SettingsReportAccessor.getSettingsReportJobs("ALL");
    }

    public static SettingsRptJobInfo[] GetSettingsRptJobs(string jobstatus)
    {
      return SettingsReportAccessor.getSettingsReportJobs(jobstatus);
    }

    [PgReady]
    private static SettingsRptJobInfo[] getSettingsReportJobs(string jobstatus)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("SELECT reportID, reportName, jobtype, Status, Message, CreatedBy, CreateDate, CancelDt, CancelBy, LastUpdateDt, DeleteBy, DeleteDt, FileGuid FROM [SettingsRptQueue]");
        if (jobstatus.Equals("ALL"))
          pgDbQueryBuilder.AppendLine(" order by ReportID desc");
        else
          pgDbQueryBuilder.AppendLine(" WHERE status = " + SQL.EncodeString(jobstatus));
        return SettingsReportAccessor.dataRowCollectionToJobInfo(pgDbQueryBuilder.Execute());
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string str = "SELECT reportID, reportName, jobtype, Status, Message, CreatedBy, CreateDate, CancelDt, CancelBy, LastUpdateDt, DeleteBy, DeleteDt, FileGuid FROM [SettingsRptQueue] ";
      string text = !jobstatus.Equals("ALL") ? str + " WHERE status = " + SQL.EncodeString(jobstatus) : str + " order by ReportID desc";
      dbQueryBuilder.AppendLine(text);
      return SettingsReportAccessor.dataRowCollectionToJobInfo(dbQueryBuilder.Execute());
    }

    public static void DeleteSettingsRptJobs(string reportID, string deletedBy)
    {
      DateTime utcNow = DateTime.UtcNow;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update SettingsRptQueue set status = " + (object) 6 + ", DeleteBy=" + SQL.EncodeString(deletedBy) + ", DeleteDt=" + SQL.EncodeDateTime(utcNow) + " where ReportID = " + reportID ?? "");
      dbQueryBuilder.ExecuteNonQuery(TimeSpan.FromMinutes(10.0), DbTransactionType.None);
    }

    public static SettingsRptJobInfo.jobStatus GetSettingsRptJobStatus(string reportID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT Status FROM [SettingsRptQueue] where reportID=" + reportID;
      dbQueryBuilder.AppendLine(text);
      return (SettingsRptJobInfo.jobStatus) dbQueryBuilder.ExecuteScalar();
    }

    public static bool CancelJob(string reportID)
    {
      SettingsRptJobInfo.jobStatus settingsRptJobStatus = SettingsReportAccessor.GetSettingsRptJobStatus(reportID);
      if (settingsRptJobStatus.Equals((object) SettingsRptJobInfo.jobStatus.Canceling))
      {
        SettingsReportAccessor.CancelSettingsRptJobs(reportID.ToString(), "", true);
        return true;
      }
      return settingsRptJobStatus.Equals((object) SettingsRptJobInfo.jobStatus.Deleted) || settingsRptJobStatus.Equals((object) SettingsRptJobInfo.jobStatus.Canceled);
    }

    public static bool CancelSettingsRptJobs(string reportID, string cancelBy, bool cancelcomplete)
    {
      SettingsRptJobInfo settingsRptInfo = SettingsReportAccessor.getSettingsRptInfo(reportID);
      if (cancelBy.Length == 0)
        cancelBy = settingsRptInfo.CancelBy;
      if (settingsRptInfo.Status.Equals((object) SettingsRptJobInfo.jobStatus.Completed) || settingsRptInfo.Status.Equals((object) SettingsRptJobInfo.jobStatus.Failed) || settingsRptInfo.Status.Equals((object) SettingsRptJobInfo.jobStatus.Deleted))
        return false;
      DateTime utcNow = DateTime.UtcNow;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (cancelcomplete)
        dbQueryBuilder.AppendLine("update SettingsRptQueue set status = " + (object) 4 + ", CancelBy=" + SQL.EncodeString(cancelBy) + ", CancelDt=" + SQL.EncodeDateTime(utcNow) + " where ReportID = " + reportID ?? "");
      else
        dbQueryBuilder.AppendLine("update SettingsRptQueue set status = " + (object) 5 + ", CancelBy=" + SQL.EncodeString(cancelBy) + ", CancelDt=" + SQL.EncodeDateTime(utcNow) + " where ReportID = " + reportID ?? "");
      dbQueryBuilder.ExecuteNonQuery(TimeSpan.FromMinutes(10.0), DbTransactionType.None);
      return true;
    }

    public static SettingsRptJobInfo[] GetFilteredSettingsRptJobs(
      string reportName,
      string reportType,
      string createdBy,
      string sortBy,
      int pageIdx,
      out int total,
      int pageSize)
    {
      DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
      dbQueryBuilder1.AppendLine("SELECT reportID FROM [SettingsRptQueue] ");
      dbQueryBuilder1.AppendLine(" WHERE ");
      if (reportName != string.Empty || reportType.ToLower() != "all" || createdBy.ToLower() != "all")
      {
        if (reportName != string.Empty && (reportType.ToLower() != "all" || createdBy.ToLower() != "all"))
          dbQueryBuilder1.AppendLine(" LOWER(reportName) LIKE '%" + SQL.EncodeString(reportName.ToLower(), false) + "%' AND ");
        else if (!string.IsNullOrEmpty(reportName))
          dbQueryBuilder1.AppendLine(" LOWER(reportName) LIKE '%" + SQL.EncodeString(reportName.ToLower(), false) + "%' ");
        if (reportType.ToLower() != "all" && createdBy.ToLower() != "all")
          dbQueryBuilder1.AppendLine(" jobtype = " + reportType + " AND ");
        else if (reportType.ToLower() != "all")
          dbQueryBuilder1.AppendLine(" jobtype = " + reportType);
        if (createdBy.ToLower() != "all")
          dbQueryBuilder1.AppendLine(" LOWER(CreatedBy) LIKE " + SQL.EncodeString("%" + createdBy.ToLower()));
        dbQueryBuilder1.AppendLine(" AND ");
      }
      dbQueryBuilder1.AppendLine(" status < 6 ");
      if (sortBy != string.Empty)
        dbQueryBuilder1.AppendLine(" ORDER BY " + sortBy);
      else
        dbQueryBuilder1.AppendLine(" ORDER BY ReportID DESC ");
      DataRowCollection dataRowCollection = dbQueryBuilder1.Execute();
      List<int> intList = new List<int>();
      List<int> values = new List<int>();
      for (int index = 0; index < dataRowCollection.Count; ++index)
      {
        DataRow dataRow = dataRowCollection[index];
        intList.Add(Convert.ToInt32(dataRowCollection[index]["reportID"]));
      }
      total = intList.Count;
      int num = pageIdx + pageSize;
      if (total >= pageIdx)
      {
        for (int index = pageIdx; index < num && index < total; ++index)
          values.Add(intList[index]);
      }
      if (values.Count == 0)
        return new SettingsRptJobInfo[0];
      DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
      dbQueryBuilder2.AppendLine("SELECT reportID, reportName, jobtype, Status, Message, CreatedBy, CreateDate,  FileGuid FROM [SettingsRptQueue] ");
      dbQueryBuilder2.AppendLine(" WHERE reportID in (");
      dbQueryBuilder2.AppendLine(string.Join<int>(",", (IEnumerable<int>) values));
      dbQueryBuilder2.AppendLine(" ) ");
      if (sortBy != string.Empty)
        dbQueryBuilder2.AppendLine(" ORDER BY " + sortBy);
      else
        dbQueryBuilder2.AppendLine(" ORDER BY ReportID DESC ");
      return SettingsReportAccessor.dataRowCollectionToJobInfo(dbQueryBuilder2.Execute());
    }

    private static SettingsRptJobInfo[] dataRowCollectionToJobInfo(DataRowCollection rows)
    {
      SettingsRptJobInfo[] jobInfo = new SettingsRptJobInfo[rows.Count];
      for (int index = 0; index < rows.Count; ++index)
      {
        DataRow row = rows[index];
        SettingsRptJobInfo.jobStatus int16_1 = (SettingsRptJobInfo.jobStatus) Convert.ToInt16(rows[index]["status"].ToString());
        SettingsRptJobInfo.jobType int16_2 = (SettingsRptJobInfo.jobType) Convert.ToInt16(rows[index]["jobtype"].ToString());
        jobInfo[index] = new SettingsRptJobInfo(int16_2, rows[index]["reportname"].ToString(), int16_1, rows[index]["createdby"].ToString(), rows[index]["createdate"].ToString(), Convert.ToString(rows[index]["fileguid"]));
        jobInfo[index].ReportID = rows[index]["reportID"].ToString();
        jobInfo[index].Message = rows[index]["message"].ToString();
      }
      return jobInfo;
    }
  }
}

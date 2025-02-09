// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerTasks.AttachmentMetadataMigrationToCIFSTask
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServerObjects.eFolder;
using EllieMae.EMLite.Server.Tasks;
using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server.ServerTasks
{
  public class AttachmentMetadataMigrationToCIFSTask : ITaskHandler
  {
    private const string className = "AttachmentMetadataMigrationToCIFSTask�";

    public void ProcessTask(ServerTask taskInfo)
    {
      ClientContext current = ClientContext.GetCurrent();
      try
      {
        bool flag1 = true;
        if (!(bool) current.Settings.GetServerSetting("AttachmentMetadataMigration.EnableAttachmentMigrationToCIFS"))
          return;
        if (taskInfo != null && !string.IsNullOrEmpty(taskInfo.Data))
        {
          string[] strArray = taskInfo.Data.Split('|');
          flag1 = this.IsTimeOfDayBetween(DateTime.Now, new TimeSpan(Convert.ToInt32(strArray[0]), 0, 0), new TimeSpan(Convert.ToInt32(strArray[1]), 59, 59));
        }
        if (!flag1)
          return;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        DateTime now = DateTime.Now;
        string serverSetting1 = (string) current.Settings.GetServerSetting("AttachmentMetadataMigration.MaxDaysOfNoActivityForMigration");
        string[] strArray1 = serverSetting1.Split('|');
        int int32_1 = Convert.ToInt32(strArray1[0]);
        int? nullable = strArray1.Length > 1 ? new int?(Convert.ToInt32(strArray1[1])) : new int?();
        int serverSetting2 = (int) current.Settings.GetServerSetting("AttachmentMetadataMigration.MaxLoanCountToMigratePerJob");
        int serverSetting3 = (int) current.Settings.GetServerSetting("AttachmentMetadataMigration.MaxJobDurationInMins");
        current.TraceLog.WriteInfo(nameof (AttachmentMetadataMigrationToCIFSTask), string.Format("Start::AttachmentCIFSMigration processing - Client Instance {0} RetentionPeriodInDays {1} LoanAttachmentCleanupDurationInMins {2}", (object) current.InstanceName, (object) serverSetting1, (object) serverSetting3));
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select distinct top " + (object) serverSetting2 + " ls.Guid, ls.XRefID, ls.LoanFolder, ls.LoanName, ls.LoanNumber, ls.BorrowerLastName, ls.BorrowerFirstName, ls.Address1, ls.LoanSource from \r\n                    LoanSummary ls inner join LoanProperties lp on ls.Guid = lp.Guid\r\n                    left join LoanLock ll on ll.guid = ls.Guid\r\n                    where (ls.lastmodified < convert(date, DATEADD(day, -" + (object) int32_1 + ", GetDate()))");
        if (nullable.HasValue)
          dbQueryBuilder.Append(" or ls.DateCreated < convert(date, DATEADD(day, -" + (object) nullable + ", GetDate()))");
        dbQueryBuilder.Append(") and lp.Category = 'LoanStorage' and Attribute = 'AttachmentsMetaData' and lp.Value in ('DB', 'InProcess') and (ll.guid is null or ll.Exclusive <> 1)");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        SQLAttachmentXMLProvider attachmentXmlProvider = new SQLAttachmentXMLProvider();
        int num1 = 0;
        int num2 = 0;
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          string str1 = dataRow["Guid"].ToString();
          bool flag2 = false;
          string loginSessionID = Guid.NewGuid().ToString();
          LockInfo lockInfo = (LockInfo) null;
          try
          {
            string loanFolder = dataRow["LoanFolder"].ToString();
            string str2 = dataRow["LoanName"].ToString();
            int int32_2 = Convert.ToInt32(dataRow["XRefId"].ToString());
            lockInfo = new LockInfo(str1, "<system>", "", "", loginSessionID, (string) null, LoanInfo.LockReason.OpenForWork, DateTime.Now, LockInfo.ExclusiveLock.NGSharedLock, true);
            LoanLockAccessor.updateLock(lockInfo);
            LoanPropertySettingsAccessor.AppendUpdateRecord(str1, new LoanProperty("LoanStorage", "AttachmentsMetaData", "InProcess"));
            AttachmentMetadataMigrationToCIFSTask.SaveFile(attachmentXmlProvider.GetAttachmentXml(int32_2), ClientContext.GetCurrent().Settings.GetLoansFilePath(loanFolder + "\\" + str2 + "\\Attachments.xml"));
            flag2 = true;
            LoanPropertySettingsAccessor.AppendUpdateRecord(str1, new LoanProperty("LoanStorage", "AttachmentsMetaData", "CIFS"));
            attachmentXmlProvider.DeleteAttachments(int32_2);
            string loanNumber = dataRow["LoanNumber"].ToString();
            string borrowerLastName = dataRow["BorrowerLastName"].ToString();
            string borrowerFirstName = dataRow["BorrowerFirstName"].ToString();
            string address = dataRow["Address1"].ToString();
            string fileSource = dataRow["LoanSource"].ToString();
            SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new LoanFileAuditRecord("<system>", "<system>", ActionType.AttachmentMigrated, DateTime.Now, str1, loanFolder, loanNumber, borrowerLastName, borrowerFirstName, address, fileSource, "Encompass SmartClient"));
            ++num2;
          }
          catch (LockException ex)
          {
            lockInfo = (LockInfo) null;
          }
          catch (Exception ex1)
          {
            if (!flag2)
            {
              try
              {
                LoanPropertySettingsAccessor.AppendUpdateRecord(str1, new LoanProperty("LoanStorage", "AttachmentsMetaData", "DB"));
              }
              catch (Exception ex2)
              {
                current.TraceLog.WriteException(nameof (AttachmentMetadataMigrationToCIFSTask), ex2);
              }
            }
            ++num1;
            current.TraceLog.WriteError(nameof (AttachmentMetadataMigrationToCIFSTask), "Error during AttachmentCIFSMigration for records for loan: " + str1 + ": Stack Trace : " + ex1.ToString());
          }
          finally
          {
            try
            {
              if (lockInfo != null)
                LoanLockAccessor.removeLock(lockInfo);
            }
            catch
            {
            }
          }
          if (DateTime.Now > now.AddMinutes((double) serverSetting3))
            break;
        }
        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;
        string str = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", (object) elapsed.Hours, (object) elapsed.Minutes, (object) elapsed.Seconds, (object) (elapsed.Milliseconds / 10));
        if (num1 > 0)
          current.TraceLog.WriteError(nameof (AttachmentMetadataMigrationToCIFSTask), string.Format("Finish::AttachmentCIFSMigration processing with exceptions - Client Instance {0} Time ran {1} Total Records Deleted {2} Total Exception Records {3} ", (object) current.InstanceName, (object) str, (object) num2, (object) num1));
        else
          current.TraceLog.WriteInfo(nameof (AttachmentMetadataMigrationToCIFSTask), string.Format("Finish::AttachmentCIFSMigration processing - Client Instance {0} Time ran {1} Total Records Deleted {2} Total Exception Records {3} ", (object) current.InstanceName, (object) str, (object) num2, (object) num1));
      }
      catch (Exception ex)
      {
        current.TraceLog.WriteError(nameof (AttachmentMetadataMigrationToCIFSTask), "Error during AttachmentCIFSMigration : Stack Trace : " + ex.ToString());
      }
    }

    private static void SaveFile(XmlDocument xmlDoc, string filePath)
    {
      using (BinaryObject data = new BinaryObject(xmlDoc.OuterXml, Encoding.UTF8))
      {
        using (DataFile dataFile = FileStore.CheckOut(filePath, MutexAccess.Write))
        {
          if (data == null && dataFile.Exists)
            dataFile.Delete();
          else
            dataFile.CheckIn(data);
        }
      }
    }

    private bool IsTimeOfDayBetween(DateTime time, TimeSpan startTime, TimeSpan endTime)
    {
      if (endTime == startTime)
        return true;
      return endTime < startTime ? time.TimeOfDay <= endTime || time.TimeOfDay >= startTime : time.TimeOfDay >= startTime && time.TimeOfDay <= endTime;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerTasks.LoanVersionCleanupTask
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Server.Tasks;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server.ServerTasks
{
  internal class LoanVersionCleanupTask : ITaskHandler
  {
    private const string className = "LoanVersionCleanupTask�";
    private ClientContext ctx;

    public void ProcessTask(ServerTask taskInfo)
    {
      try
      {
        bool flag = true;
        if (taskInfo != null && taskInfo.Data != null)
        {
          string[] strArray = taskInfo.Data.Split('|');
          flag = this.IsTimeOfDayBetween(DateTime.Now, new TimeSpan(Convert.ToInt32(strArray[0]), 0, 0), new TimeSpan(Convert.ToInt32(strArray[1]), 59, 59));
        }
        if (!flag)
          return;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        DateTime now = DateTime.Now;
        this.ctx = ClientContext.GetCurrent();
        int num1 = (int) this.ctx.Settings.GetServerSetting("Policies.LoanVersionRetentionPeriodDays");
        if (num1 == 0)
          num1 = 1;
        DateTime dateTime = DateTime.Now.Subtract(TimeSpan.FromDays((double) num1));
        int serverSetting1 = (int) this.ctx.Settings.GetServerSetting("Policies.LoanVersCleanupPercentageCount");
        int num2 = (int) this.ctx.Settings.GetServerSetting("Policies.LoanVersCleanupMaxRecordCount");
        int serverSetting2 = (int) this.ctx.Settings.GetServerSetting("Policies.LoanVersCleanupDurationInMins");
        this.ctx.TraceLog.WriteInfo(nameof (LoanVersionCleanupTask), string.Format("Start::LoanVersionCleanup processing - Client Instance {0} RetentionPeriodInDays {1} LoanVersCleanupDurationInMins {2}", (object) this.ctx.InstanceName, (object) num1, (object) serverSetting2));
        DbQueryBuilder dbQueryBuilder1 = new DbQueryBuilder();
        dbQueryBuilder1.Append("select * from LoanVersionDetails where DateUpdated <= '" + (object) dateTime.Date + "' order by DateUpdated asc");
        DataRowCollection dataRowCollection = dbQueryBuilder1.Execute();
        int num3 = 0;
        int count = dataRowCollection.Count;
        if (count < num2)
        {
          num2 = count;
        }
        else
        {
          int int32 = Decimal.ToInt32((Decimal) count * Convert.ToDecimal("0." + (object) serverSetting1));
          if (int32 != 0 && int32 < num2)
            num2 = int32;
        }
        int num4 = 0;
        long num5 = 0;
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          try
          {
            long int64 = Convert.ToInt64(Convert.ToString(dataRow["Id"]));
            num5 = int64;
            string guid = Convert.ToString(dataRow["Guid"]);
            int int32 = Convert.ToInt32(Convert.ToString(dataRow["VersionNumber"]));
            LoanIdentity loanIdentity = Loan.LookupIdentity(guid);
            if (loanIdentity != (LoanIdentity) null)
            {
              string fullLoanFilePath = new LoanFolder(loanIdentity.LoanFolder).GetFullLoanFilePath(loanIdentity.LoanName);
              string path = fullLoanFilePath.Replace(new FileInfo(fullLoanFilePath).Name, "Versions") + "\\" + string.Format("{0:D5}_", (object) int32) + "loan.em";
              if (File.Exists(path))
                File.Delete(path);
            }
            DbQueryBuilder dbQueryBuilder2 = new DbQueryBuilder();
            dbQueryBuilder2.Append("delete from LoanVersionDetails where [Id] =" + (object) int64);
            dbQueryBuilder2.ExecuteNonQuery();
            ++num3;
          }
          catch (Exception ex)
          {
            ++num4;
            this.ctx.TraceLog.WriteError(nameof (LoanVersionCleanupTask), "Error during LoanVersionCleanup for records id " + (object) num5 + ": Stack Trace : " + (object) ex);
          }
          if (num3 < num2)
          {
            if (DateTime.Now > now.AddMinutes((double) serverSetting2))
              break;
          }
          else
            break;
        }
        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;
        string str = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", (object) elapsed.Hours, (object) elapsed.Minutes, (object) elapsed.Seconds, (object) (elapsed.Milliseconds / 10));
        if (num4 > 0)
          this.ctx.TraceLog.WriteError(nameof (LoanVersionCleanupTask), string.Format("Finish::LoanVersionCleanup processing with exceptions - Client Instance {0} Time ran {1} Total Records Deleted {2} Total Exception Records {3} ", (object) this.ctx.InstanceName, (object) str, (object) num3, (object) num4));
        else
          this.ctx.TraceLog.WriteInfo(nameof (LoanVersionCleanupTask), string.Format("Finish::LoanVersionCleanup processing - Client Instance {0} Time ran {1} Total Records Deleted {2} Total Exception Records {3} ", (object) this.ctx.InstanceName, (object) str, (object) num3, (object) num4));
      }
      catch (Exception ex)
      {
        this.ctx.TraceLog.WriteError(nameof (LoanVersionCleanupTask), "Error during LoanVersionCleanup : Stack Trace : " + (object) ex);
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

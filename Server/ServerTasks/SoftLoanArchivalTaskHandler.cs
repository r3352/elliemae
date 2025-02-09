// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerTasks.SoftLoanArchivalTaskHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.Server.Tasks;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server.ServerTasks
{
  internal class SoftLoanArchivalTaskHandler : ITaskHandler
  {
    private const string className = "SoftLoanArchivalTask�";
    private ClientContext ctx;

    public void ProcessTask(ServerTask taskInfo)
    {
      try
      {
        bool flag = true;
        if (taskInfo != null && taskInfo.Data != null)
        {
          string[] strArray = taskInfo.Data.Split('|');
          this.ctx = ClientContext.GetCurrent();
          flag = this.IsTimeOfDayBetween(DateTime.Now, new TimeSpan(Convert.ToInt32(strArray[0]), 0, 0), new TimeSpan(Convert.ToInt32(strArray[1]), 59, 59));
        }
        if (!flag)
          return;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        DateTime now = DateTime.Now;
        this.ctx = ClientContext.GetCurrent();
        this.ctx.TraceLog.WriteInfo("SoftLoanArchivalTask", string.Format("Start::Loan Archival processing - Client Instance {0}", (object) this.ctx.InstanceName));
        string[] strArray1 = taskInfo.Data.Split('|');
        int num1 = 1;
        int num2 = 0;
        int num3 = 0;
        DbValueList parameters = new DbValueList();
        while (this.IsTimeOfDayBetween(DateTime.Now, new TimeSpan(Convert.ToInt32(strArray1[0]), 0, 0), new TimeSpan(Convert.ToInt32(strArray1[1]), 59, 59)) && num1 > 0)
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.AppendLine("MarkLoanArchive");
          num1 = (int) dbQueryBuilder.ExecuteStoredProc(DbTransactionType.Default, parameters)[0][0];
          num3 += num1;
          if (num1 > 0)
            this.ctx.TraceLog.WriteInfo("SoftLoanArchivalTask", string.Format("Loan Archival Batch {0} completed for Client Instance {1}", (object) ++num2, (object) this.ctx.InstanceName));
        }
        stopwatch.Stop();
        this.ctx.TraceLog.WriteInfo("SoftLoanArchivalTask", string.Format("Finish :: Loan Archival processing - Client Instance {0}, Time ran {1} ms, Total batches ran {2}, Total Loans Marked Archived  {3} ", (object) this.ctx.InstanceName, (object) stopwatch.ElapsedMilliseconds.ToString("0"), (object) num2, (object) num3));
      }
      catch (Exception ex)
      {
        this.ctx.TraceLog.WriteError("SoftLoanArchivalTask", "Error during SoftLoanArchival : Stack Trace : " + (object) ex);
        throw ex;
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

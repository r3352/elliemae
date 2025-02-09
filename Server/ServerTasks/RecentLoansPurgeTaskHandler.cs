// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerTasks.RecentLoansPurgeTaskHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.Server.Tasks;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerTasks
{
  internal class RecentLoansPurgeTaskHandler : ITaskHandler
  {
    private const string className = "RecentLoansPurgeTaskHandler�";
    private ClientContext ctx;

    public void ProcessTask(ServerTask taskInfo)
    {
      try
      {
        this.ctx = ClientContext.GetCurrent();
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbValueList parameters = new DbValueList();
        if (string.IsNullOrEmpty(Company.GetCompanySetting("Policies", "RecentLoansPurgeLimit")))
          return;
        dbQueryBuilder.AppendLine("RECENTLOANSPURGE");
        dbQueryBuilder.ExecuteStoredProc(DbTransactionType.Default, parameters);
      }
      catch (Exception ex)
      {
        this.ctx.TraceLog.WriteError(nameof (RecentLoansPurgeTaskHandler), "Error during  RecentLoansPurge : Stack Trace : " + (object) ex);
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

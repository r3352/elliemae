// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ServerTasks.ServerTaskHistoryInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ServerTasks
{
  [Serializable]
  public class ServerTaskHistoryInfo
  {
    private int historyId;
    private int scheduleId;
    private DateTime startTime;
    private DateTime endTime;
    private ServerTaskOutcome outcome;

    public ServerTaskHistoryInfo(
      int historyId,
      int scheduleId,
      DateTime startTime,
      DateTime endTime,
      ServerTaskOutcome outcome)
    {
      this.historyId = historyId;
      this.scheduleId = scheduleId;
      this.startTime = startTime;
      this.endTime = endTime;
      this.outcome = outcome;
    }

    public int HistoryID => this.historyId;

    public int ScheduleID => this.scheduleId;

    public DateTime StartTime => this.startTime;

    public DateTime EndTime => this.endTime;

    public ServerTaskOutcome Outcome => this.outcome;
  }
}

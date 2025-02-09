// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.MilestoneStatistics
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  [Serializable]
  public class MilestoneStatistics
  {
    public readonly string MilestoneID;
    public readonly string MilestoneName;
    public readonly int Order;
    public int LoanCount;
    public int CompletedCount;
    public Decimal AvgDuration;
    public int MaxDuration;
    public int ExpectedDuration;

    public MilestoneStatistics(string msid, string msName, int order, int days)
    {
      this.MilestoneID = msid;
      this.MilestoneName = msName;
      this.Order = order;
      this.ExpectedDuration = days;
    }
  }
}

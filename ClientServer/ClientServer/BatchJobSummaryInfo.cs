// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.BatchJobSummaryInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class BatchJobSummaryInfo
  {
    public int BatchJobId { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public BatchJobApplicationChannel ApplicationChannel { get; set; }

    public BatchJobStatus Status { get; set; }

    public DateTime StatusDate { get; set; }

    public BatchJobType Type { get; set; }

    public string LastModifiedByUserId { get; set; }

    public string EntityId { get; set; }

    public BatchJobEntityType EntityType { get; set; }

    public string EntityName { get; set; }

    public int TotalJobItemsCount { get; set; }

    public int TotalJobItemsUnProcessed { get; set; }

    public int TotalJobItemsInProgress { get; set; }

    public int TotalJobItemsSucceeded { get; set; }

    public int TotalJobItemsErrored { get; set; }

    public int TotalJobItemsCancelled { get; set; }
  }
}

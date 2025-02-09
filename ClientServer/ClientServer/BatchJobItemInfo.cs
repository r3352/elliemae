// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.BatchJobItemInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class BatchJobItemInfo
  {
    private int batchJobItemId;
    private int batchJobId;
    private BatchJobItemStatus status;
    private DateTime statusDate;
    private string action;
    private string entityId;
    private BatchJobItemEntityType entityType;
    private string result;
    private string metaData;

    public BatchJobItemInfo(
      int batchJobItemId,
      int batchJobId,
      BatchJobItemStatus status,
      DateTime statusDate,
      string action,
      string entityId,
      BatchJobItemEntityType entityType,
      string result,
      string metaData)
    {
      this.batchJobItemId = batchJobItemId;
      this.batchJobId = batchJobId;
      this.status = status;
      this.statusDate = statusDate;
      this.action = action;
      this.entityId = entityId;
      this.entityType = entityType;
      this.result = result;
      this.metaData = metaData;
    }

    public int BatchJobItemId => this.batchJobItemId;

    public int BatchJobId
    {
      get => this.batchJobId;
      set => this.batchJobId = value;
    }

    public BatchJobItemStatus Status
    {
      get => this.status;
      set => this.status = value;
    }

    public DateTime StatusDate
    {
      get => this.statusDate;
      set => this.statusDate = value;
    }

    public string Action
    {
      get => this.action;
      set => this.action = value;
    }

    public string EntityId
    {
      get => this.entityId;
      set => this.entityId = value;
    }

    public BatchJobItemEntityType EntityType
    {
      get => this.entityType;
      set => this.entityType = value;
    }

    public string Result
    {
      get => this.result;
      set => this.result = value;
    }

    public string MetaData
    {
      get => this.metaData;
      set => this.metaData = value;
    }
  }
}

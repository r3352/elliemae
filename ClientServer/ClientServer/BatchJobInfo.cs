// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.BatchJobInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class BatchJobInfo
  {
    private int batchJobId;
    private string createdBy;
    private DateTime createdDate;
    private BatchJobApplicationChannel applicationChannel;
    private BatchJobStatus status;
    private DateTime statusDate;
    private BatchJobType type;
    private string lastModifiedByUserId;
    private string entityId;
    private BatchJobEntityType entityType;
    private string metaData;
    private string result;
    private List<BatchJobItemInfo> batchJobItems = new List<BatchJobItemInfo>();

    public BatchJobInfo(
      int batchJobId,
      string createdBy,
      DateTime createdDate,
      BatchJobApplicationChannel applicationChannel,
      BatchJobStatus status,
      DateTime statusDate,
      BatchJobType type,
      string lastModifiedByUserId,
      string entityId,
      BatchJobEntityType entityType,
      string metaData,
      List<BatchJobItemInfo> batchJobItems,
      string result = null)
    {
      this.batchJobId = batchJobId;
      this.createdBy = createdBy;
      this.createdDate = createdDate;
      this.applicationChannel = applicationChannel;
      this.status = status;
      this.statusDate = statusDate;
      this.type = type;
      this.lastModifiedByUserId = lastModifiedByUserId;
      this.entityId = entityId;
      this.entityType = entityType;
      this.metaData = metaData;
      this.batchJobItems = batchJobItems;
      this.result = result;
    }

    public int BatchJobId => this.batchJobId;

    public string CreatedBy
    {
      get => this.createdBy;
      set => this.createdBy = value;
    }

    public DateTime CreatedDate
    {
      get => this.createdDate;
      set => this.createdDate = value;
    }

    public BatchJobApplicationChannel ApplicationChannel
    {
      get => this.applicationChannel;
      set => this.applicationChannel = value;
    }

    public BatchJobStatus Status
    {
      get => this.status;
      set => this.status = value;
    }

    public DateTime StatusDate
    {
      get => this.statusDate;
      set => this.statusDate = value;
    }

    public BatchJobType Type
    {
      get => this.type;
      set => this.type = value;
    }

    public string LastModifiedByUserId
    {
      get => this.lastModifiedByUserId;
      set => this.lastModifiedByUserId = value;
    }

    public string EntityId
    {
      get => this.entityId;
      set => this.entityId = value;
    }

    public BatchJobEntityType EntityType
    {
      get => this.entityType;
      set => this.entityType = value;
    }

    public string MetaData => this.metaData;

    public string Result
    {
      get => this.result;
      set => this.result = value;
    }

    public List<BatchJobItemInfo> BatchJobItems => this.batchJobItems;

    public static string SerializeToJSON(BatchJobInfo batchJobInfo)
    {
      return new JavaScriptSerializer().Serialize((object) batchJobInfo);
    }
  }
}

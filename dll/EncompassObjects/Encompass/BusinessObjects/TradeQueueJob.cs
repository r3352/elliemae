// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeQueueJob
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Trading;
using EllieMae.Encompass.BusinessObjects.TradeManagement;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects
{
  public class TradeQueueJob
  {
    private List<TradeQueueJobDetails> jobDetails = new List<TradeQueueJobDetails>();

    public int BatchJobId { get; set; }

    public ApplicationChannel Channel { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public JobStatus Status { get; set; }

    public int? TradeId { get; set; }

    public TradeType TradeType { get; set; }

    public int LoanCount { get; set; }

    public string Result { get; set; }

    public List<TradeQueueJobDetails> JobItems
    {
      get => this.jobDetails;
      set => this.jobDetails = value;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeQueueJobSummary
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Trading;
using EllieMae.Encompass.BusinessObjects.TradeManagement;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects
{
  public class TradeQueueJobSummary
  {
    public int BatchJobId { get; set; }

    public ApplicationChannel Channel { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public JobStatus Status { get; set; }

    public int TradeId { get; set; }

    public EllieMae.Encompass.BusinessObjects.TradeManagement.TradeType TradeType { get; set; }

    public int TotalLoanCount { get; set; }

    public int TotalLoansUnProcessed { get; set; }

    public int TotalLoansInProgress { get; set; }

    public int TotalLoansSucceeded { get; set; }

    public int TotalLoansErrored { get; set; }

    public int TotalLoansCancelled { get; set; }
  }
}

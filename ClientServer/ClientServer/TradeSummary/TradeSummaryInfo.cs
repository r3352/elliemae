// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TradeSummary.TradeSummaryInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.TradeSummary
{
  public class TradeSummaryInfo
  {
    public string TradeId { get; set; }

    public string TradeType { get; set; }

    public string TradeName { get; set; }

    public string MasterContract { get; set; }

    public string InvestorTradeNumber { get; set; }

    public string InvestorCommitmentNumber { get; set; }

    public string InvestorName { get; set; }

    public Decimal TradeAmount { get; set; }

    public Decimal TotalPairOffAmount { get; set; }

    public Decimal Tolerance { get; set; }

    public DateTime CommitmentDate { get; set; }

    public DateTime InvestorDeliveryDate { get; set; }

    public DateTime EarlyDeliveryDate { get; set; }

    public DateTime TargetDeliveryDate { get; set; }

    public DateTime ActualDeliveryDate { get; set; }

    public DateTime PurchaseDate { get; set; }

    public List<EllieMae.EMLite.ClientServer.TradeSummary.LoanHistory> LoanHistory { get; set; }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanSummaryExtension
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanSummaryExtension
  {
    public string Guid { get; set; }

    public int CorrespondentTradeId { get; set; }

    public string CorrespondentTradeGuid { get; set; }

    public string TradeGuid { get; set; }

    public int TradeId { get; set; }

    public string LoanNumber { get; set; }

    public Decimal LoanAmount { get; set; }

    public int CorrespondentLoanStatus { get; set; }

    public DateTime PurchaseDate { get; set; }

    public DateTime ReceivedDate { get; set; }

    public DateTime RejectedDate { get; set; }

    public string InvestorStatus { get; set; }

    public DateTime InvestorStatusDate { get; set; }

    public CorrespondentMasterDeliveryType DeliveryType { get; set; }

    public string TpoCompanyId { get; set; }

    public DateTime SubmittedForReviewDate { get; set; }

    public DateTime PurchaseSuspenseDate { get; set; }

    public DateTime PurchaseApprovalDate { get; set; }

    public DateTime ClearedForPurchaseDate { get; set; }

    public DateTime CancelledDate { get; set; }

    public DateTime VoidedDate { get; set; }

    public DateTime WithdrawalRequestedDate { get; set; }

    public DateTime WithdrawnDate { get; set; }

    public Decimal PurchasedPrinciple { get; set; }

    public string LenderCaseIdentifier { get; set; }
  }
}

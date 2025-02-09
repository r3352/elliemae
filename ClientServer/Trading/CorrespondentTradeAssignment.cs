// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentTradeAssignment
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class CorrespondentTradeAssignment : TradeAssignment
  {
    [ScriptIgnore]
    public CorrespondentTradeInfo CorrespondentTradeInfo { get; set; }

    public string CommitmentNumber { get; set; }

    public DateTime CommitmentDate { get; set; }

    public string CorrespondentMasterCommitmentNumber { get; set; }

    public CorrespondentTradeCommitmentType CommitmentType { get; set; }

    public CorrespondentMasterDeliveryType DeliveryType { get; set; }

    public string CompanyName { get; set; }

    public string TPOID { get; set; }

    public string OrganizationID { get; set; }

    public Decimal TradeAmount { get; set; }

    public DateTime ExpirationDate { get; set; }

    public DateTime DeliveryExpirationDate { get; set; }

    public Decimal TotalPairoffAmount { get; set; }

    public Decimal Tolerance { get; set; }

    public bool Locked { get; set; }

    public string AOTSecurityType { get; set; }

    public string AOTSecurityTerm { get; set; }

    public Decimal AOTSecurityCoupon { get; set; }

    public Decimal AOTSecurityPrice { get; set; }

    public DateTime AOTSettlementDate { get; set; }

    public DateTime AOTOriginalTradeDate { get; set; }

    public string AOTOriginalTradeDealer { get; set; }

    public Decimal? MinNoteRateRange { get; set; }

    public Decimal? MaxNoteRateRange { get; set; }

    public string Pricing { get; set; }

    public string PriceAdjustments { get; set; }

    public string SRPTable { get; set; }

    public string Guid { get; set; }

    public Decimal AssignedAmount { get; set; }

    public Decimal OpenAmount { get; set; }

    public Decimal CompletionPercentage { get; set; }

    public Decimal MaxAmount { get; set; }

    public Decimal MinAmount { get; set; }

    public Decimal GainLossAmount { get; set; }

    public bool HasPendingLoan { get; set; }

    public TradeStatus Status { get; set; }

    public string AuthorizedTraderUserId { get; set; }

    public string AuthorizedTraderName { get; set; }

    public string AuthorizedTraderEmail { get; set; }

    public string TradeDescription { get; set; }

    public Decimal WeightedAvgBulkPrice { get; set; }

    public bool IsWeightedAvgBulkPriceLocked { get; set; }

    public bool SRPfromPPE { get; set; }

    public bool AdjustmentsfromPPE { get; set; }

    public string EppsLoanProgramFilter { get; set; }

    public CorrespondentTradeAssignment()
    {
    }

    public CorrespondentTradeAssignment(
      CorrespondentTradeInfo tradeInfo,
      List<string> skipFieldList)
      : base(tradeInfo.TradeID, TradeType.CorrespondentTrade, skipFieldList)
    {
      this.ConvertFromTradeInfoToTradeAssignment(tradeInfo);
      this.SkipFieldList = skipFieldList;
    }

    public CorrespondentTradeAssignment(string jsonString)
    {
      this.ConvertFromTradeAssignmentToTradeInfo(jsonString);
    }

    private void ConvertFromTradeInfoToTradeAssignment(CorrespondentTradeInfo info)
    {
      this.CorrespondentTradeInfo = info;
      this.TradeID = info.TradeID;
      this.TradeType = info.TradeType;
      this.Locked = info.Locked;
      this.Guid = info.Guid;
      this.CommitmentDate = info.CommitmentDate;
      this.CommitmentType = info.CommitmentType;
      this.CommitmentNumber = info.Name;
      this.CorrespondentMasterCommitmentNumber = info.CorrespondentMasterCommitmentNumber;
      this.DeliveryExpirationDate = info.DeliveryExpirationDate;
      this.DeliveryType = info.DeliveryType;
      this.ExpirationDate = info.ExpirationDate;
      this.HasPendingLoan = info.PendingLoanList.Count<KeyValuePair<CorrespondentTradeLoanStatus, int>>() > 0;
      this.Status = info.Status;
      this.TradeDescription = info.TradeDescription;
      this.WeightedAvgBulkPrice = info.WeightedAvgBulkPrice;
      this.IsWeightedAvgBulkPriceLocked = info.IsWeightedAvgBulkPriceLocked;
      this.TradeAmount = info.TradeAmount;
      this.Tolerance = info.Tolerance;
      this.MaxAmount = info.MaxAmount;
      this.MinAmount = info.MinAmount;
      this.AssignedAmount = info.AssignedLoanList.Where<LoanSummaryExtension>((Func<LoanSummaryExtension, bool>) (x => x.CorrespondentTradeId > 1)).Sum<LoanSummaryExtension>((Func<LoanSummaryExtension, Decimal>) (l => l.LoanAmount));
      this.OpenAmount = info.OpenAmount;
      this.CompletionPercentage = info.CompletionPercent;
      this.TotalPairoffAmount = info.TotalPairoffAmount;
      this.GainLossAmount = info.GainLossAmount;
      this.AuthorizedTraderUserId = info.AuthorizedTraderUserId;
      this.AuthorizedTraderName = info.AuthorizedTraderName;
      this.AuthorizedTraderUserId = info.AuthorizedTraderEmail;
      this.OrganizationID = info.OrganizationID;
      this.CompanyName = info.CompanyName;
      this.TPOID = info.TPOID;
      this.AOTOriginalTradeDate = info.AOTOriginalTradeDate;
      this.AOTOriginalTradeDealer = info.AOTOriginalTradeDealer;
      this.AOTSecurityCoupon = info.AOTSecurityCoupon;
      this.AOTSecurityPrice = info.AOTSecurityPrice;
      this.AOTSecurityTerm = info.AOTSecurityTerm;
      this.AOTSecurityType = info.AOTSecurityType;
      this.AOTSettlementDate = info.AOTSettlementDate;
      this.SRPfromPPE = info.SRPfromPPE;
      this.AdjustmentsfromPPE = info.AdjustmentsfromPPE;
      this.Pricing = info.Pricing.ToXml();
      this.PriceAdjustments = info.PriceAdjustments.ToXml();
      this.SRPTable = info.SRPTable.ToXml();
      this.EppsLoanProgramFilter = info.EPPSLoanProgramsFilter.ToXml();
    }

    private void ConvertFromTradeAssignmentToTradeInfo(string jsonString)
    {
      CorrespondentTradeAssignment correspondentTradeAssignment = new JavaScriptSerializer().Deserialize<CorrespondentTradeAssignment>(jsonString);
      this.CorrespondentTradeInfo = new CorrespondentTradeInfo();
      this.CorrespondentTradeInfo.TradeID = correspondentTradeAssignment.TradeID;
      this.CorrespondentTradeInfo.Locked = correspondentTradeAssignment.Locked;
      this.CorrespondentTradeInfo.Guid = correspondentTradeAssignment.Guid;
      this.CorrespondentTradeInfo.CommitmentDate = correspondentTradeAssignment.CommitmentDate.Date;
      this.CorrespondentTradeInfo.CommitmentType = correspondentTradeAssignment.CommitmentType;
      this.CorrespondentTradeInfo.Name = correspondentTradeAssignment.CommitmentNumber;
      this.CorrespondentTradeInfo.CorrespondentMasterCommitmentNumber = correspondentTradeAssignment.CorrespondentMasterCommitmentNumber;
      this.CorrespondentTradeInfo.DeliveryExpirationDate = correspondentTradeAssignment.DeliveryExpirationDate.Date;
      this.CorrespondentTradeInfo.DeliveryType = correspondentTradeAssignment.DeliveryType;
      this.CorrespondentTradeInfo.ExpirationDate = correspondentTradeAssignment.ExpirationDate.Date;
      this.CorrespondentTradeInfo.Status = correspondentTradeAssignment.Status;
      this.CorrespondentTradeInfo.TradeDescription = correspondentTradeAssignment.TradeDescription;
      this.CorrespondentTradeInfo.WeightedAvgBulkPrice = correspondentTradeAssignment.WeightedAvgBulkPrice;
      this.CorrespondentTradeInfo.IsWeightedAvgBulkPriceLocked = correspondentTradeAssignment.IsWeightedAvgBulkPriceLocked;
      this.CorrespondentTradeInfo.TradeAmount = correspondentTradeAssignment.TradeAmount;
      this.CorrespondentTradeInfo.Tolerance = correspondentTradeAssignment.Tolerance;
      this.CorrespondentTradeInfo.MaxAmount = correspondentTradeAssignment.MaxAmount;
      this.CorrespondentTradeInfo.MinAmount = correspondentTradeAssignment.MinAmount;
      this.CorrespondentTradeInfo.OpenAmount = correspondentTradeAssignment.OpenAmount;
      this.CorrespondentTradeInfo.CompletionPercent = correspondentTradeAssignment.CompletionPercentage;
      this.CorrespondentTradeInfo.TotalPairoffAmount = correspondentTradeAssignment.TotalPairoffAmount;
      this.CorrespondentTradeInfo.GainLossAmount = correspondentTradeAssignment.GainLossAmount;
      this.CorrespondentTradeInfo.OrganizationID = correspondentTradeAssignment.OrganizationID;
      this.CorrespondentTradeInfo.CompanyName = correspondentTradeAssignment.CompanyName;
      this.CorrespondentTradeInfo.TPOID = correspondentTradeAssignment.TPOID;
      this.CorrespondentTradeInfo.AOTOriginalTradeDate = correspondentTradeAssignment.AOTOriginalTradeDate;
      this.CorrespondentTradeInfo.AOTOriginalTradeDealer = correspondentTradeAssignment.AOTOriginalTradeDealer;
      this.CorrespondentTradeInfo.AOTSecurityCoupon = correspondentTradeAssignment.AOTSecurityCoupon;
      this.CorrespondentTradeInfo.AOTSecurityPrice = correspondentTradeAssignment.AOTSecurityPrice;
      this.CorrespondentTradeInfo.AOTSecurityTerm = correspondentTradeAssignment.AOTSecurityTerm;
      this.CorrespondentTradeInfo.AOTSecurityType = correspondentTradeAssignment.AOTSecurityType;
      this.CorrespondentTradeInfo.AOTSettlementDate = correspondentTradeAssignment.AOTSettlementDate;
      this.CorrespondentTradeInfo.Pricing = BinaryConvertible<TradePricingInfo>.Parse(correspondentTradeAssignment.Pricing);
      this.CorrespondentTradeInfo.PriceAdjustments = TradePriceAdjustments.Parse(correspondentTradeAssignment.PriceAdjustments);
      this.CorrespondentTradeInfo.SRPTable = (EllieMae.EMLite.Trading.SRPTable) new XmlSerializer().Deserialize(correspondentTradeAssignment.SRPTable, typeof (EllieMae.EMLite.Trading.SRPTable));
      this.CorrespondentTradeInfo.SRPfromPPE = correspondentTradeAssignment.SRPfromPPE;
      this.CorrespondentTradeInfo.AdjustmentsfromPPE = correspondentTradeAssignment.AdjustmentsfromPPE;
      this.CorrespondentTradeInfo.EPPSLoanProgramsFilter = (EPPSLoanProgramFilters) new XmlSerializer().Deserialize(correspondentTradeAssignment.EppsLoanProgramFilter, typeof (EPPSLoanProgramFilters));
      if (correspondentTradeAssignment.MinNoteRateRange.HasValue || correspondentTradeAssignment.MaxNoteRateRange.HasValue)
      {
        CorrespondentTradeInfo correspondentTradeInfo = this.CorrespondentTradeInfo;
        SimpleTradeFilter simpleFilter = new SimpleTradeFilter(false);
        Decimal? nullable = correspondentTradeAssignment.MinNoteRateRange;
        string minText;
        if (nullable.HasValue)
        {
          nullable = correspondentTradeAssignment.MinNoteRateRange;
          minText = nullable.ToString();
        }
        else
          minText = "";
        nullable = correspondentTradeAssignment.MaxNoteRateRange;
        string maxText;
        if (nullable.HasValue)
        {
          nullable = correspondentTradeAssignment.MaxNoteRateRange;
          maxText = nullable.ToString();
        }
        else
          maxText = "";
        simpleFilter.NoteRateRange = Range<Decimal>.Parse(minText, maxText, Decimal.MinValue, Decimal.MaxValue);
        TradeFilter tradeFilter = new TradeFilter(simpleFilter);
        correspondentTradeInfo.Filter = tradeFilter;
      }
      this.SkipFieldList = correspondentTradeAssignment.SkipFieldList;
      this.CommitmentNumber = correspondentTradeAssignment.CommitmentNumber;
      this.CommitmentDate = correspondentTradeAssignment.CommitmentDate;
      this.CorrespondentMasterCommitmentNumber = correspondentTradeAssignment.CorrespondentMasterCommitmentNumber;
      this.CommitmentType = correspondentTradeAssignment.CommitmentType;
      this.DeliveryType = correspondentTradeAssignment.DeliveryType;
      this.CompanyName = correspondentTradeAssignment.CompanyName;
      this.TPOID = correspondentTradeAssignment.TPOID;
      this.OrganizationID = correspondentTradeAssignment.OrganizationID;
      this.TradeAmount = correspondentTradeAssignment.TradeAmount;
      this.ExpirationDate = correspondentTradeAssignment.ExpirationDate;
      this.DeliveryExpirationDate = correspondentTradeAssignment.DeliveryExpirationDate;
      this.TotalPairoffAmount = correspondentTradeAssignment.TotalPairoffAmount;
      this.Tolerance = correspondentTradeAssignment.Tolerance;
      this.Locked = correspondentTradeAssignment.Locked;
      this.AOTSecurityType = correspondentTradeAssignment.AOTSecurityType;
      this.AOTSecurityTerm = correspondentTradeAssignment.AOTSecurityTerm;
      this.AOTSecurityCoupon = correspondentTradeAssignment.AOTSecurityCoupon;
      this.AOTSecurityPrice = correspondentTradeAssignment.AOTSecurityPrice;
      this.AOTSettlementDate = correspondentTradeAssignment.AOTSettlementDate;
      this.AOTOriginalTradeDate = correspondentTradeAssignment.AOTOriginalTradeDate;
      this.AOTOriginalTradeDealer = correspondentTradeAssignment.AOTOriginalTradeDealer;
      this.MinNoteRateRange = correspondentTradeAssignment.MinNoteRateRange;
      this.MaxNoteRateRange = correspondentTradeAssignment.MaxNoteRateRange;
      this.Pricing = correspondentTradeAssignment.Pricing;
      this.PriceAdjustments = correspondentTradeAssignment.PriceAdjustments;
      this.SRPTable = correspondentTradeAssignment.SRPTable;
      this.Guid = correspondentTradeAssignment.Guid;
      this.AssignedAmount = correspondentTradeAssignment.AssignedAmount;
      this.OpenAmount = correspondentTradeAssignment.OpenAmount;
      this.CompletionPercentage = correspondentTradeAssignment.CompletionPercentage;
      this.MaxAmount = correspondentTradeAssignment.MaxAmount;
      this.MinAmount = correspondentTradeAssignment.MinAmount;
      this.GainLossAmount = correspondentTradeAssignment.GainLossAmount;
      this.HasPendingLoan = correspondentTradeAssignment.HasPendingLoan;
      this.Status = correspondentTradeAssignment.Status;
      this.TradeDescription = correspondentTradeAssignment.TradeDescription;
      this.WeightedAvgBulkPrice = correspondentTradeAssignment.WeightedAvgBulkPrice;
      this.IsWeightedAvgBulkPriceLocked = correspondentTradeAssignment.IsWeightedAvgBulkPriceLocked;
      this.SRPfromPPE = correspondentTradeAssignment.SRPfromPPE;
      this.AdjustmentsfromPPE = correspondentTradeAssignment.AdjustmentsfromPPE;
      this.EppsLoanProgramFilter = correspondentTradeAssignment.EppsLoanProgramFilter;
      this.LoanSyncOption = correspondentTradeAssignment.LoanSyncOption;
      this.FinalStatus = correspondentTradeAssignment.FinalStatus;
      this.SessionId = correspondentTradeAssignment.SessionId;
    }

    [CLSCompliant(false)]
    public static string SerializeToJson(CorrespondentTradeInfo info)
    {
      return new JavaScriptSerializer().Serialize((object) info);
    }
  }
}

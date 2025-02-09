// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.ICorrespondentTradeService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public interface ICorrespondentTradeService
  {
    CorrespondentTrade GetCorrespondentTrade(int correspondentTradeId);

    CorrespondentTrade GetCorrespondentTrade(string correspondentTradeNumber);

    List<CorrespondentTrade> GetCorrespondentTradesByTpoId(int Id);

    List<CorrespondentTrade> GetCorrespondentTradesByTpoId(string tpoId);

    List<CorrespondentTrade> GetCorrespondentTradesByOrgId(string orgId);

    List<CorrespondentTrade> GetCorrespondentTradesByMasterNumber(string correspondentMasterNumber);

    List<CorrespondentTrade> GetCorrespondentTrades(TradeStatus[] tradeStatus);

    List<TradeLoanAssignment> GetTradeLoanAssignments(int correspondentTradeId);

    List<TradeLoanAssignment> GetTradeLoanAssignments(string correspondentTradeNumber);

    Dictionary<int, string> GetEffectiveCorrespondentTradesByLoanInfo(
      string externalOrgId,
      string deliveryType,
      double loanAmount);

    Dictionary<int, string> GetEffectiveCorrespondentTradesByLoanNumber(
      string deliveryType,
      string loanNumber);

    Dictionary<int, string> GetEffectiveCorrespondentTradesByLoanNumber(
      string deliveryType,
      string loanNumber,
      string correspondentMasterNumber);

    void AllocateLoanToCorrespondentTrade(
      string deliveryType,
      string loanNumber,
      int correspondentTradeId);

    void AllocateLoanToCorrespondentTrade(
      string deliveryType,
      string loanNumber,
      string correspondentTradeName);

    LoanUpdateResults RemoveLoansFromCorrespondentTrade(
      string correspondentTradeName,
      List<string> loanNumber);

    int CreateCorrespondentTrade(
      CorrespondentTrade trade,
      string priceAdjustmentTemplateGUID,
      string SRPTemplateGUID);

    int UpdateCorrespondentTrade(
      CorrespondentTrade trade,
      string priceAdjustmentTemplateGUID,
      string SRPTemplateGUID);

    List<EppsLoanProgram> GetCorrespondentTradeEppsLoanPrograms(string correspondentTradeName);

    bool AddCorrespondentTradeEppsLoanPrograms(
      string correspondentTradeName,
      List<string> programId);

    bool DeleteCorrespondentTradeEppsLoanPrograms(
      string correspondentTradeName,
      List<string> programId);
  }
}

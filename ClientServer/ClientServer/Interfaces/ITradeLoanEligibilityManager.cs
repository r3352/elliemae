// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Interfaces.ITradeLoanEligibilityManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Trading;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Interfaces
{
  public interface ITradeLoanEligibilityManager
  {
    int CreateTradeLoanEligibility(
      TradeLoanEligibilityPricingInfo tradeElegibilityInfo);

    TradeLoanEligibilityPricingInfo GeTradeLoanEligibilityByPricingId(int eligibilityPricingId);

    TradeLoanEligibilityPricingInfo GeTradeLoanEligibilityByTradeIdAndLoanId(
      string loanId,
      int tradeId);

    TradeLoanEligibilityPricingSummary[] GetTradeLoanSummaryByTradeId(int tradeId);

    void DeleteTradeLoanEligibility(int eligibilityPricingId);

    void DeleteTradeLoanEligibility(string loanId, int tradeId);
  }
}

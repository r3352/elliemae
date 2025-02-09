// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.TradeLoanEligibilityManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Interfaces;
using EllieMae.EMLite.ClientServer.Trading;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects;
using System;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class TradeLoanEligibilityManager : SessionBoundObject, ITradeLoanEligibilityManager
  {
    private const string className = "TradeLoanEligibilityManager";

    public TradeLoanEligibilityManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (TradeLoanEligibilityManager).ToLower());
      return this;
    }

    public virtual int CreateTradeLoanEligibility(
      TradeLoanEligibilityPricingInfo tradeElegibilityInfo)
    {
      this.onApiCalled(nameof (TradeLoanEligibilityManager), nameof (CreateTradeLoanEligibility), Array.Empty<object>());
      try
      {
        return TradeLoanEligibilityAndItemsAccessor.CreateTradeLoanEligibility(tradeElegibilityInfo);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (TradeLoanEligibilityManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual TradeLoanEligibilityPricingInfo GeTradeLoanEligibilityByPricingId(
      int eligibilityPricingId)
    {
      TradeLoanEligibilityPricingInfo eligibilityPricingInfo = (TradeLoanEligibilityPricingInfo) null;
      this.onApiCalled(nameof (TradeLoanEligibilityManager), nameof (GeTradeLoanEligibilityByPricingId), Array.Empty<object>());
      try
      {
        eligibilityPricingInfo = TradeLoanEligibilityAndItemsAccessor.GeTradeLoanEligibilityPricingByPricingId(eligibilityPricingId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (TradeLoanEligibilityManager), ex, this.Session.SessionInfo);
      }
      return eligibilityPricingInfo;
    }

    public virtual TradeLoanEligibilityPricingInfo GeTradeLoanEligibilityByTradeIdAndLoanId(
      string loanId,
      int tradeId)
    {
      TradeLoanEligibilityPricingInfo eligibilityPricingInfo = (TradeLoanEligibilityPricingInfo) null;
      this.onApiCalled(nameof (TradeLoanEligibilityManager), nameof (GeTradeLoanEligibilityByTradeIdAndLoanId), Array.Empty<object>());
      try
      {
        eligibilityPricingInfo = TradeLoanEligibilityAndItemsAccessor.GeTradeLoanEligibilityByTradeIdAndLoanId(loanId, tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (TradeLoanEligibilityManager), ex, this.Session.SessionInfo);
      }
      return eligibilityPricingInfo;
    }

    public virtual void DeleteTradeLoanEligibility(int eligibilityPricingId)
    {
      this.onApiCalled(nameof (TradeLoanEligibilityManager), nameof (DeleteTradeLoanEligibility), Array.Empty<object>());
      try
      {
        TradeLoanEligibilityAndItemsAccessor.DeleteTradeLoanEligibility(eligibilityPricingId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (TradeLoanEligibilityManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteTradeLoanEligibility(string loanId, int tradeId)
    {
      this.onApiCalled(nameof (TradeLoanEligibilityManager), nameof (DeleteTradeLoanEligibility), Array.Empty<object>());
      try
      {
        TradeLoanEligibilityAndItemsAccessor.DeleteTradeLoanEligibility(loanId, tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (TradeLoanEligibilityManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual TradeLoanEligibilityPricingSummary[] GetTradeLoanSummaryByTradeId(int tradeId)
    {
      TradeLoanEligibilityPricingSummary[] summaryByTradeId = (TradeLoanEligibilityPricingSummary[]) null;
      this.onApiCalled(nameof (TradeLoanEligibilityManager), nameof (GetTradeLoanSummaryByTradeId), Array.Empty<object>());
      try
      {
        summaryByTradeId = TradeLoanEligibilityAndItemsAccessor.GetTradeLoanSummaryByTradeId(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (TradeLoanEligibilityManager), ex, this.Session.SessionInfo);
      }
      return summaryByTradeId;
    }
  }
}

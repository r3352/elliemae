// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.LoanSummaryExtensionManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Server;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class LoanSummaryExtensionManager : SessionBoundObject, ILoanSummaryExtensionManager
  {
    private const string className = "LoanSummaryExtensionManager";

    public LoanSummaryExtensionManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (LoanSummaryExtensionManager).ToLower());
      return this;
    }

    public virtual void AddToSummary(LoanSummaryExtension loan)
    {
      this.onApiCalled(nameof (LoanSummaryExtensionManager), "AddToCorrespondentLoanSummary", new object[1]
      {
        (object) loan
      });
      try
      {
        LoanSummaryExtensions.AddToSummary(loan);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanSummaryExtensionManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateSummary(LoanSummaryExtension loan)
    {
      this.onApiCalled(nameof (LoanSummaryExtensionManager), "AddToCorrespondentLoanSummary", new object[1]
      {
        (object) loan
      });
      try
      {
        LoanSummaryExtensions.UpdateSummary(loan);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanSummaryExtensionManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual LoanSummaryExtension GetSummary(string loanGuid)
    {
      this.onApiCalled(nameof (LoanSummaryExtensionManager), nameof (GetSummary), new object[1]
      {
        (object) loanGuid
      });
      try
      {
        return LoanSummaryExtensions.GetSummary(loanGuid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanSummaryExtensionManager), ex, this.Session.SessionInfo);
        return (LoanSummaryExtension) null;
      }
    }

    public virtual List<LoanSummaryExtension> GetSummariesByTradeId(int tradeId)
    {
      this.onApiCalled(nameof (LoanSummaryExtensionManager), nameof (GetSummariesByTradeId), new object[1]
      {
        (object) tradeId
      });
      try
      {
        return LoanSummaryExtensions.GetSummariesByTradeId(tradeId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanSummaryExtensionManager), ex, this.Session.SessionInfo);
        return (List<LoanSummaryExtension>) null;
      }
    }
  }
}

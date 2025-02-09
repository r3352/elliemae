// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.OverNightRateProtectionManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects;
using System;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class OverNightRateProtectionManager : SessionBoundObject, IOverNightRateProtectionManager
  {
    private const string className = "OverNightRateProtectionManager";

    public OverNightRateProtectionManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (OverNightRateProtectionManager).ToLower());
      return this;
    }

    public virtual double GetOnrpPeriodAccruedAmount(
      LoanChannel channel,
      string entityId,
      DateTime onrpStartDate)
    {
      this.onApiCalled(nameof (OverNightRateProtectionManager), nameof (GetOnrpPeriodAccruedAmount), Array.Empty<object>());
      try
      {
        return OverNightRateProtection.GetOnrpPeriodAccruedAmount(channel, entityId, onrpStartDate);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OverNightRateProtectionManager), ex, this.Session.SessionInfo);
        return 0.0;
      }
    }

    public virtual void UpdateOnrpPeriodAccruedAmount(
      LoanChannel channel,
      string entityId,
      DateTime onrpStartDate,
      double loanAmount)
    {
      this.onApiCalled(nameof (OverNightRateProtectionManager), nameof (UpdateOnrpPeriodAccruedAmount), Array.Empty<object>());
      try
      {
        OverNightRateProtection.UpdateOnrpPeriodAccruedAmount(channel, entityId, onrpStartDate, loanAmount);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OverNightRateProtectionManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteOnrpPeriodAccruedAmount(
      LoanChannel channel,
      string entityId,
      bool checkUseGlobal)
    {
      this.onApiCalled(nameof (OverNightRateProtectionManager), nameof (DeleteOnrpPeriodAccruedAmount), Array.Empty<object>());
      try
      {
        OverNightRateProtection.DeleteOnrpPeriodAccruedAmount(channel, entityId, checkUseGlobal);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OverNightRateProtectionManager), ex, this.Session.SessionInfo);
      }
    }
  }
}

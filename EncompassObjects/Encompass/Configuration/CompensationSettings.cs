// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.CompensationSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;
using System;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  /// <summary>Compensation Settings class</summary>
  public class CompensationSettings : SessionBoundObject
  {
    /// <summary>
    /// Provides access to the Loan Officer compensation-related settings.
    /// </summary>
    /// <param name="session"></param>
    internal CompensationSettings(Session session)
      : base(session)
    {
    }

    /// <summary>
    /// Returns the Compensation Plan for the given user ID and trigger date.
    /// Returns NULL if no plan exists for the given user ID and trigger date.
    /// </summary>
    /// <param name="userId">The Encompass user's ID.</param>
    /// <param name="triggerDate">The trigger date for the compensation plan.</param>
    /// <returns></returns>
    public LoanCompensation GetCompensationForUser(string userId, DateTime triggerDate)
    {
      LoanCompPlan currentComPlanforUser = this.Session.SessionObjects.ConfigurationManager.GetCurrentComPlanforUser(userId, triggerDate);
      return currentComPlanforUser == null ? (LoanCompensation) null : new LoanCompensation(currentComPlanforUser);
    }

    /// <summary>
    /// Returns the Compensation Plan for the given TPO WebCenter ID and trigger date.
    /// Returns NULL if no plan exists for the given TPO WebCenter ID and trigger date.
    /// </summary>
    /// <param name="tpoWebID">The TPO Web ID.</param>
    /// <param name="triggerDate">The trigger date for the comp plan.</param>
    /// <returns></returns>
    public LoanCompensation GetCompensationByTPOID(string tpoWebID, DateTime triggerDate)
    {
      LoanCompPlan brokerByTpoWebId = this.Session.SessionObjects.ConfigurationManager.GetCurrentComPlanforBrokerByTPOWebID(tpoWebID, triggerDate);
      return brokerByTpoWebId == null ? (LoanCompensation) null : new LoanCompensation(brokerByTpoWebId);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.CompensationSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;
using System;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  public class CompensationSettings : SessionBoundObject
  {
    internal CompensationSettings(Session session)
      : base(session)
    {
    }

    public LoanCompensation GetCompensationForUser(string userId, DateTime triggerDate)
    {
      LoanCompPlan currentComPlanforUser = this.Session.SessionObjects.ConfigurationManager.GetCurrentComPlanforUser(userId, triggerDate);
      return currentComPlanforUser == null ? (LoanCompensation) null : new LoanCompensation(currentComPlanforUser);
    }

    public LoanCompensation GetCompensationByTPOID(string tpoWebID, DateTime triggerDate)
    {
      LoanCompPlan brokerByTpoWebId = this.Session.SessionObjects.ConfigurationManager.GetCurrentComPlanforBrokerByTPOWebID(tpoWebID, triggerDate);
      return brokerByTpoWebId == null ? (LoanCompensation) null : new LoanCompensation(brokerByTpoWebId);
    }
  }
}

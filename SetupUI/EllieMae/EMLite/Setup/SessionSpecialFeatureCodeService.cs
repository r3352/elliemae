// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SessionSpecialFeatureCodeService
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SessionSpecialFeatureCodeService : ISpecialFeatureCodeManager
  {
    private ISpecialFeatureCodeManager Manager { get; set; }

    public SessionSpecialFeatureCodeService(Sessions.Session session)
    {
      this.Manager = Session.SessionObjects.SpecialFeatureCodeManager;
    }

    bool ISpecialFeatureCodeManager.Activate(SpecialFeatureCodeDefinition toActivate)
    {
      return this.Manager.Activate(toActivate);
    }

    bool ISpecialFeatureCodeManager.Add(SpecialFeatureCodeDefinition toAdd)
    {
      return this.Manager.Add(toAdd);
    }

    bool ISpecialFeatureCodeManager.Deactivate(SpecialFeatureCodeDefinition toDeactivate)
    {
      return this.Manager.Deactivate(toDeactivate);
    }

    bool ISpecialFeatureCodeManager.Delete(SpecialFeatureCodeDefinition toDelete)
    {
      return this.Manager.Delete(toDelete);
    }

    IList<SpecialFeatureCodeDefinition> ISpecialFeatureCodeManager.GetAll()
    {
      return this.Manager.GetAll();
    }

    IList<SpecialFeatureCodeDefinition> ISpecialFeatureCodeManager.GetActive()
    {
      return this.Manager.GetActive();
    }

    bool ISpecialFeatureCodeManager.Update(SpecialFeatureCodeDefinition toUpdate)
    {
      return this.Manager.Update(toUpdate);
    }

    bool ISpecialFeatureCodeManager.IsUsedinFieldTriggerRule(string sfcId)
    {
      return this.Manager.IsUsedinFieldTriggerRule(sfcId);
    }
  }
}

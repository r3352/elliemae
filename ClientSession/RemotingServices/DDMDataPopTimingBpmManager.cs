// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.DDMDataPopTimingBpmManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class DDMDataPopTimingBpmManager : ManagerBase
  {
    private IBpmManager ddmDataPopTimingMgr;

    internal DDMDataPopTimingBpmManager(Sessions.Session session)
      : base(session, ClientSessionCacheID.BpmDDMDataPopTiming)
    {
      this.ddmDataPopTimingMgr = this.session.SessionObjects.BpmManager;
    }

    internal static DDMDataPopTimingBpmManager Instance
    {
      get => Session.DefaultInstance.DDMDataPopTimingBpmManager;
    }

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetDDMDataPopTimingBpmManager();
    }

    public void UpdateDDMDataPopulationTiming(
      DDMDataPopulationTiming ddmDataPopTiming,
      bool forceToPrimaryDb = false)
    {
      this.ddmDataPopTimingMgr.UpdateDDMDataPopulationTiming(ddmDataPopTiming, forceToPrimaryDb);
    }

    public DDMDataPopulationTiming GetDDMDataPopulationTiming(bool forceToPrimaryDb = false)
    {
      return this.ddmDataPopTimingMgr.GetDDMDataPopulationTiming(forceToPrimaryDb);
    }

    public List<DDMDataPopTimingField> UpdateDDMDataPopulationTimingNumberOfReferences()
    {
      return this.ddmDataPopTimingMgr.UpdateDDMDataPopulationTimingNumberOfReferences();
    }

    public int GetNumberReferences(string fieldId, bool forceToPrimaryDb = false)
    {
      return this.ddmDataPopTimingMgr.GetNumberReferences(fieldId, forceToPrimaryDb);
    }
  }
}

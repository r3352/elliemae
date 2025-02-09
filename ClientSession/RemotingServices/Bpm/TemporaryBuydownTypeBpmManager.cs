// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.Bpm.TemporaryBuydownTypeBpmManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices.Bpm
{
  public class TemporaryBuydownTypeBpmManager
  {
    private IBpmManager bpmManager;

    public TemporaryBuydownTypeBpmManager(Sessions.Session session)
    {
      this.bpmManager = session.SessionObjects.BpmManager;
    }

    public List<TemporaryBuydown> GetAllTemporaryBuydowns()
    {
      return this.bpmManager.GetAllTemporaryBuydowns();
    }

    public void CreateTemporaryBuydownType(TemporaryBuydown buydown)
    {
      this.bpmManager.CreateTemporaryBuydownType(buydown);
    }

    public void UpdateTemporaryBuydownType(TemporaryBuydown buydown)
    {
      this.bpmManager.UpdateTemporaryBuydownType(buydown);
    }

    public void DeleteTemporaryBuydownType(TemporaryBuydown buydown)
    {
      this.bpmManager.DeleteTemporaryBuydownType(buydown);
    }
  }
}

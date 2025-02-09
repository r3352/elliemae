// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.TriggersBpmManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class TriggersBpmManager : BpmManager
  {
    internal static TriggersBpmManager Instance => Session.DefaultInstance.TriggersBpmManager;

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetTriggersBpmManager();
    }

    internal TriggersBpmManager(Sessions.Session session)
      : base(session, BizRuleType.Triggers, ClientSessionCacheID.BpmFieldAccess)
    {
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.ConnectionManagerWrapper
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace Elli.Server.Remoting
{
  public class ConnectionManagerWrapper : IConnectionManager
  {
    public void Start() => ConnectionManager.Start();

    public void RegisterSession(IClientSession session)
    {
      ConnectionManager.RegisterSession(session);
    }

    public void UnregisterSession(IClientSession session)
    {
      ConnectionManager.UnregisterSession(session);
    }
  }
}

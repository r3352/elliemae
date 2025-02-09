// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.DiagnosticsInterface
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using System;
using System.Threading;

#nullable disable
namespace Elli.Server.Remoting
{
  internal class DiagnosticsInterface : RemotedObject, IDiagnosticsInterface
  {
    private const string className = "DiagnosticsInterface";
    public static readonly DiagnosticsInterface RemotingInstance = new DiagnosticsInterface();

    private DiagnosticsInterface()
    {
    }

    public byte[] GetBytes(int count)
    {
      if (ServerGlobals.APILatency != TimeSpan.Zero)
        Thread.Sleep(ServerGlobals.APILatency);
      return new byte[count];
    }

    public void UploadBytes(byte[] bytes)
    {
      if (!(ServerGlobals.APILatency != TimeSpan.Zero))
        return;
      Thread.Sleep(ServerGlobals.APILatency);
    }

    public BinaryObject GetBinaryObject(int size)
    {
      return BinaryObject.Marshal(new BinaryObject(new byte[size]));
    }

    public void UploadBinaryObject(BinaryObject o) => o.Download();
  }
}

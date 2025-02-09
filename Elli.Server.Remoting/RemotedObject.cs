// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.RemotedObject
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.Server;
using System;
using System.Runtime.Remoting;

#nullable disable
namespace Elli.Server.Remoting
{
  public abstract class RemotedObject : MarshalByRefObject
  {
    public override object InitializeLifetimeService() => (object) null;

    public virtual void Disconnect()
    {
      try
      {
        RemotingServices.Disconnect((MarshalByRefObject) this);
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning("RemoteObject", "Error disconnection object of type \"" + this.GetType().Name + "\": " + (object) ex);
      }
    }
  }
}

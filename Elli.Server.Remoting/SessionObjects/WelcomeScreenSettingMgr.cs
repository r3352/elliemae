// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.WelcomeScreenSettingMgr
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects;
using System;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class WelcomeScreenSettingMgr : SessionBoundObject, IWelcomeScreenSettingMgr
  {
    private const string className = "WelcomeScreenSettingMgr";

    public WelcomeScreenSettingMgr Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (WelcomeScreenSettingMgr).ToLower());
      return this;
    }

    public virtual WelcomeScreenSetting Get()
    {
      this.onApiCalled(nameof (WelcomeScreenSettingMgr), nameof (Get), Array.Empty<object>());
      try
      {
        return WelcomeScreenSettingStore.Get(this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (WelcomeScreenSettingMgr), ex, this.Session.SessionInfo);
        return (WelcomeScreenSetting) null;
      }
    }

    public virtual void Save(WelcomeScreenSetting setting)
    {
      this.onApiCalled(nameof (WelcomeScreenSettingMgr), "Set", new object[1]
      {
        (object) setting
      });
      try
      {
        WelcomeScreenSettingStore.Save(this.Session.UserID, setting);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (WelcomeScreenSettingMgr), ex, this.Session.SessionInfo);
      }
    }
  }
}

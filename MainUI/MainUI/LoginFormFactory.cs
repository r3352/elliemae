// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.LoginFormFactory
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Web.Host.Login;
using EllieMae.EMLite.Common.Login;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public static class LoginFormFactory
  {
    public static Form GetLoginForm(
      string loginName,
      string password,
      string server,
      string instanceId,
      bool donotLockServer)
    {
      if (!LoginUtil.IsTokenLoginOnly)
        return (Form) new LoginScreen(loginName, password, server, donotLockServer);
      return !WebLoginUtil.IsChromiumForWebLoginEnabled ? (Form) new LoginUtil().GetLoginForm(AppName.Encompass, server, donotLockServer, new Func<LoginContext, bool>(EncompassLoginRoutines.LoginEncompassWithAccessToken), instanceId) : (Form) new WebLoginUtil().GetLoginForm(AppName.Encompass, server, donotLockServer, new Func<LoginContext, bool>(EncompassLoginRoutines.LoginEncompassWithAccessToken), instanceId);
    }
  }
}

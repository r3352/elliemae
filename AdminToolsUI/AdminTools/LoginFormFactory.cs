// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.LoginFormFactory
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using Elli.Web.Host.Login;
using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Login;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public static class LoginFormFactory
  {
    public static Form GetLoginForm(
      LoginType loginType,
      string password,
      string server,
      string instanceId,
      bool donotLockServer)
    {
      if (!LoginUtil.IsTokenLoginOnly)
        return (Form) new LoginForm(loginType, server, donotLockServer);
      return !WebLoginUtil.IsChromiumForWebLoginEnabled ? (Form) new LoginUtil().GetLoginForm(AppName.Encompass, server, donotLockServer, new Func<LoginContext, bool>(LoginFormFactory.LoginAdminTools), instanceId) : (Form) new WebLoginUtil().GetLoginForm(AppName.Encompass, server, donotLockServer, new Func<LoginContext, bool>(LoginFormFactory.LoginAdminTools), instanceId);
    }

    public static bool LoginAdminTools(LoginContext context)
    {
      string server = context.Server;
      string userId = context.UserId;
      string password = context.Password;
      string authCode = context.AuthCode;
      bool flag = false;
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        Session.Start(server, userId, password, "AdminTools", false, "", authCode);
        SystemSettings.DefaultApplicationMode = ApplicationMode.Server;
        Session.ExitOnDisconnect = false;
        OrgInfo rootOrganization = Session.OrganizationManager.GetRootOrganization();
        if (!Session.UserInfo.IsAdministrator() || Session.UserInfo.OrgId != rootOrganization.Oid)
        {
          Session.End();
          if (Utils.Dialog((IWin32Window) null, "The specified user does not have root administrative rights on the Encompass Server. Please log in as a different user.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
            flag = false;
        }
        if (AssemblyResolver.IsSmartClient)
        {
          if (SmartClientUtils.IsUpdateClientInfo2Required())
            SmartClientUtils.UpdateClientInfo(LoginUtil.getSmartClientSessionInfo(Session.DefaultInstance, AssemblyResolver.SCClientID, context.Server), AssemblyResolver.SCClientID, VersionInformation.CurrentVersion.Version.FullVersion, server, true);
        }
      }
      catch (FormatException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The Server name format you have entered is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (LoginException ex)
      {
        if (ex.LoginReturnCode == LoginReturnCode.USERID_NOT_FOUND || ex.LoginReturnCode == LoginReturnCode.PASSWORD_MISMATCH)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) null, "The username and password entered do not match. The password is case-sensitive. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (ex.LoginReturnCode == LoginReturnCode.USER_DISABLED)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) null, "This user account has been disabled.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (ex.LoginReturnCode == LoginReturnCode.LOGIN_DISABLED)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) null, "Login has been disabled. Only the \"admin\" user can login to the system.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (ex.LoginReturnCode == LoginReturnCode.IP_Blocked)
        {
          int num4 = (int) Utils.Dialog((IWin32Window) null, "Remote access from IP address " + (object) ex.ClientIPAddress + " is not allowed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (ex.LoginReturnCode == LoginReturnCode.SERVER_BUSY)
        {
          int num5 = (int) Utils.Dialog((IWin32Window) null, "Server is currently busy. Please try again later.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          int num6 = (int) Utils.Dialog((IWin32Window) null, "Unknown login error.");
        }
      }
      catch (ServerDataException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "Make sure that the SQL database server (MSDE) is up and running and the data in the database is not corrupted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (VersionMismatchException ex)
      {
        if (AssemblyResolver.IsSmartClient)
          SmartClientUtils.HandleVersionMismatch(ex.ClientVersion.FullVersion, ex.ServerVersionControl.Version.FullVersion);
        if (VersionControl.QueryInstallVersionUpdate(ex.ServerVersionControl))
          Application.Exit();
        flag = false;
      }
      catch (LicenseException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (ServerConnectionException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "Unknown login error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
      return flag;
    }
  }
}

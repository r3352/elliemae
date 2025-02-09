// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.Packages.LoginHelper
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Login;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms.Packages
{
  public static class LoginHelper
  {
    private const string appName = "FormEditor";
    public static IConnection loginConnection;

    public static bool LoginFormEditor(LoginContext context)
    {
      string server = context.Server;
      string userId = context.UserId;
      string password = context.Password;
      string authCode = context.AuthCode;
      bool flag = false;
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        if (LoginHelper.performLogin(userId, password, server, authCode))
          flag = true;
      }
      catch (LoginException ex)
      {
        if (ex.LoginReturnCode == LoginReturnCode.USERID_NOT_FOUND || ex.LoginReturnCode == LoginReturnCode.PASSWORD_MISMATCH)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) null, "The username and password entered do not match. The password is case-sensitive. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (ex.LoginReturnCode == LoginReturnCode.USER_DISABLED)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) null, "This user account has been disabled. Please contact your administrator.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (ex.LoginReturnCode == LoginReturnCode.LOGIN_DISABLED)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) null, "Login has been disabled. Only the \"admin\" user can log in to the system.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (ex.LoginReturnCode == LoginReturnCode.USER_LOCKED)
        {
          int num4 = (int) Utils.Dialog((IWin32Window) null, "Your account has been locked. An administrative user must unlock your account in order for you to log in.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (ex.LoginReturnCode == LoginReturnCode.IP_Blocked)
        {
          int num5 = (int) Utils.Dialog((IWin32Window) null, "Remote access from IP address " + (object) ex.ClientIPAddress + " is not allowed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
      }
      catch (LicenseException ex)
      {
        if (ex.Cause == LicenseExceptionType.TrialExpired)
        {
          int num = (int) Utils.Dialog((IWin32Window) null, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        Application.Exit();
      }
      catch (ServerConnectionException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (FormatException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The Server name format you have entered is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (Exception ex)
      {
        if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          int num7 = (int) Utils.Dialog((IWin32Window) null, "Unknown login error: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          int num8 = (int) Utils.Dialog((IWin32Window) null, "Unknown login error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
      return flag;
    }

    private static bool performLogin(
      string loginName,
      string password,
      string server,
      string authCode)
    {
      bool flag = server == "";
      SystemSettings.LastLoginName = loginName;
      SystemSettings.DefaultApplicationMode = ApplicationMode.Server;
      SystemSettings.ApplicationMode = ApplicationMode.Server;
      Session.Start(server, loginName, password, "FormEditor", false, "", authCode);
      if (!((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).CheckPermission(AclFeature.SettingsTab_Company_CustomInputFormEditor, Session.UserInfo))
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The specified user is must be granted access to the Custom Input Form Editor in order to publish forms to this server. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (!LoginHelper.performLicenseChecks())
        return false;
      LoginHelper.loginConnection = Session.Connection;
      return true;
    }

    private static bool performLicenseChecks()
    {
      if (!Session.SessionObjects.ServerLicense.IsBankerEdition)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "You are not licensed to use this application.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).CheckPermission(AclFeature.SettingsTab_Company_CustomInputFormEditor, Session.UserInfo))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) null, "You are not authorized to use this application.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      Session.End();
      return false;
    }
  }
}

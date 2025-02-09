// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EnhancedConditionsTool.DefaultProvider
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Login;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools.EnhancedConditionsTool
{
  public class DefaultProvider : IEnhancedConditionsProvider
  {
    private string sourceHostUri;

    private Control Parent { get; set; }

    public DefaultProvider(Control parent) => this.Parent = parent;

    private bool StartSession(LoginType loginType = LoginType.All, bool forceLogout = true)
    {
      if (!forceLogout && Session.IsConnected)
        return true;
      if (Session.IsConnected)
        this.EndSession();
      Cursor current = Cursor.Current;
      bool donotLockServer = ((IEnumerable<string>) Environment.GetCommandLineArgs()).Contains<string>("-us");
      using (Form loginForm = LoginFormFactory.GetLoginForm(loginType, "", "", LoginUtil.DefaultInstanceID, donotLockServer))
      {
        if (loginForm is LoginForm)
        {
          int num1 = (int) loginForm.ShowDialog();
        }
        else
        {
          int num2 = (int) loginForm.ShowDialog();
        }
      }
      int num3 = Session.IsConnected ? 1 : 0;
      object serverSetting = (num3 != 0 ? Session.ServerManager : (IServerManager) null)?.GetServerSetting("CLIENT.ENHANCEDCONDITIONSETTINGS", false);
      bool flag1 = num3 != 0 && "true".Equals(serverSetting?.ToString()?.ToLowerInvariant());
      FeaturesAclManager aclManager = num3 != 0 ? Session.ACL.GetAclManager(AclCategory.Features) as FeaturesAclManager : (FeaturesAclManager) null;
      bool flag2 = num3 != 0 && aclManager != null && aclManager.GetUserApplicationRight(AclFeature.SettingsTab_EnhancedConditions);
      bool flag3 = (num3 & (flag1 ? 1 : 0) & (flag2 ? 1 : 0)) != 0;
      if (num3 != 0 && (!flag1 || !flag2))
      {
        int num4 = (int) Utils.Dialog((IWin32Window) this.Parent, !flag1 ? "The server does not have Enhanced Conditions settings enabled" : "The current login does not have access to Enhanced Conditions", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      Cursor.Current = current;
      return flag3;
    }

    private void EndSession() => Session.End();

    async Task<ExportablePackage> IEnhancedConditionsProvider.GetEnhancedConditions(bool isSource)
    {
      ExportablePackage enhancedConditions = (ExportablePackage) null;
      if (this.StartSession())
      {
        if (isSource)
          this.sourceHostUri = Session.ServerIdentity.Uri.AbsoluteUri;
        else if (Session.ServerIdentity.Uri.AbsoluteUri == this.sourceHostUri && DialogResult.Cancel == Utils.Dialog((IWin32Window) this.Parent, "The source and target systems are the same.", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
          return (ExportablePackage) null;
        enhancedConditions = await ExportablePackage.BuildAsync(Session.DefaultInstance, isSource);
      }
      return enhancedConditions;
    }

    async Task<IList<ConditionTemplate>> IEnhancedConditionsProvider.GetStandardConditions()
    {
      if (!this.StartSession(forceLogout: false))
        return (IList<ConditionTemplate>) null;
      IConfigurationManager configManager = Session.DefaultInstance.ConfigurationManager;
      return (IList<ConditionTemplate>) await Task.FromResult<List<ConditionTemplate>>(new List<ConditionType>()
      {
        ConditionType.Underwriting,
        ConditionType.PostClosing
      }.SelectMany<ConditionType, ConditionTemplate>((Func<ConditionType, IEnumerable<ConditionTemplate>>) (t => configManager.GetConditionTrackingSetup(t).OfType<ConditionTemplate>())).ToList<ConditionTemplate>());
    }

    Task<SyncResult> IEnhancedConditionsProvider.UpsertEnhancedConditionTemplates(
      IEnumerable<EnhancedConditionTemplate> templates,
      bool useInsert,
      CancellationToken cancellationToken)
    {
      return new RestApiHelper(Session.DefaultInstance).UpsertEnhancedConditionTemplates((IList<EnhancedConditionTemplate>) templates.ToList<EnhancedConditionTemplate>(), useInsert, cancellationToken);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.Features.FeaturePanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.StatusOnline;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.Features
{
  public class FeaturePanel : SettingsUserControl
  {
    private UserAssignmentPanel userAssignmentPanel;
    private StatusOnlineSetupPanel statusOnlineSetupPanel;
    private Sessions.Session session;
    private System.ComponentModel.Container components;
    private EncompassModule module;

    public bool IsOnCompanyStatusOnlineTab
    {
      get
      {
        return this.statusOnlineSetupPanel != null && this.statusOnlineSetupPanel.IsOnCompanyStatusOnlineTab;
      }
    }

    public bool IsOnEmailTemplatesTab
    {
      get
      {
        return this.statusOnlineSetupPanel != null && this.statusOnlineSetupPanel.IsOnEmailTemplatesTab;
      }
    }

    public bool IsOnTPOStatusOnlineTab
    {
      get
      {
        return this.statusOnlineSetupPanel != null && this.statusOnlineSetupPanel.IsOnTPOStatusOnlineTab;
      }
    }

    public string[] SelectedGUIDs
    {
      get
      {
        return this.statusOnlineSetupPanel == null ? (string[]) null : this.statusOnlineSetupPanel.SelectedGUIDs;
      }
      set
      {
        if (value == null || value.Length == 0 || this.statusOnlineSetupPanel == null)
          return;
        this.statusOnlineSetupPanel.SelectedGUIDs = value;
      }
    }

    public string[] SelectedTPOGUIDs
    {
      get
      {
        return this.statusOnlineSetupPanel == null ? (string[]) null : this.statusOnlineSetupPanel.SelectedTPOGUIDs;
      }
      set
      {
        if (value == null || value.Length == 0 || this.statusOnlineSetupPanel == null)
          return;
        this.statusOnlineSetupPanel.SelectedTPOGUIDs = value;
      }
    }

    public string[] SelectedEmailTemplateGUIDs
    {
      get
      {
        return this.statusOnlineSetupPanel == null ? (string[]) null : this.statusOnlineSetupPanel.SelectedEmailTemplateGUIDs;
      }
      set
      {
        if (value == null || value.Length == 0 || this.statusOnlineSetupPanel == null)
          return;
        this.statusOnlineSetupPanel.SelectedEmailTemplateGUIDs = value;
      }
    }

    public string[] SelectedMilestones
    {
      get
      {
        if (this.statusOnlineSetupPanel == null)
          return (string[]) null;
        string[] selectedGUIDs = (string[]) null;
        if (this.statusOnlineSetupPanel.IsOnCompanyStatusOnlineTab)
          selectedGUIDs = this.statusOnlineSetupPanel.SelectedGUIDs;
        else if (this.statusOnlineSetupPanel.IsOnTPOStatusOnlineTab)
          selectedGUIDs = this.statusOnlineSetupPanel.SelectedTPOGUIDs;
        return selectedGUIDs == null || selectedGUIDs.Length == 0 ? (string[]) null : this.session.ConfigurationManager.GetMilestonesByStatusOnlineTriggerGUIDs(selectedGUIDs);
      }
    }

    public FeaturePanel(SetUpContainer setupContainer, EncompassModule module)
      : this(setupContainer, module, Session.DefaultInstance)
    {
    }

    public FeaturePanel(
      SetUpContainer setupContainer,
      EncompassModule module,
      Sessions.Session session)
      : base(setupContainer)
    {
      this.session = session;
      this.setupContainer = setupContainer;
      this.InitializeComponent();
      this.module = module;
      this.refreshFeatureState();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.Name = nameof (FeaturePanel);
      this.Size = new Size(478, 406);
    }

    private void refreshFeatureState()
    {
      this.userAssignmentPanel = (UserAssignmentPanel) null;
      this.statusOnlineSetupPanel = (StatusOnlineSetupPanel) null;
      ModuleLicense moduleLicense;
      try
      {
        moduleLicense = Modules.GetModuleLicense(this.module, this.session);
      }
      catch
      {
        this.loadInetAccessPanel();
        return;
      }
      if (moduleLicense == null)
        this.loadPromoPanel();
      else if (moduleLicense.Disabled)
        this.LoadControl((Control) new DisabledModulePanel());
      else if (this.module == EncompassModule.StatusOnline)
        this.loadStatusOnlineSetupPanel();
      else if (moduleLicense.UserLimit >= 0)
        this.loadUserAssignmentPanel(this.module, moduleLicense);
      else
        this.LoadControl((Control) new UnlimitedUserAccessPanel(this.module, moduleLicense));
    }

    private void loadModuleLicensePanel(EncompassModule module, ModuleLicense license)
    {
      if (!EpassLogin.LoginRequired(true))
        this.LoadControl((Control) new AccessDeniedPanel());
      else
        this.LoadControl((Control) new ModuleAgreementPanel(module, license, this.session));
    }

    private void loadUserAssignmentPanel(EncompassModule module, ModuleLicense license)
    {
      if (!EpassLogin.LoginRequired(true))
      {
        this.LoadControl((Control) new AccessDeniedPanel());
      }
      else
      {
        this.userAssignmentPanel = new UserAssignmentPanel(module, license);
        this.LoadControl((Control) this.userAssignmentPanel);
      }
    }

    private void loadStatusOnlineSetupPanel()
    {
      if (!EpassLogin.LoginRequired(true, this.session))
      {
        this.LoadControl((Control) new AccessDeniedPanel());
      }
      else
      {
        this.statusOnlineSetupPanel = new StatusOnlineSetupPanel(this.session, this.setupContainer, (string) null);
        this.LoadControl((Control) this.statusOnlineSetupPanel);
      }
    }

    private void loadPromoPanel()
    {
      FeaturePromoPanel c = new FeaturePromoPanel(this.module, this.session);
      c.RefreshFeature += new EventHandler(this.onRefreshFeature);
      this.LoadControl((Control) c);
    }

    private void loadInetAccessPanel()
    {
      NoInetAccessPanel c = new NoInetAccessPanel();
      c.RetryAccess += new EventHandler(this.onRetryAccess);
      this.LoadControl((Control) c);
    }

    public void LoadControl(Control c)
    {
      this.Controls.Clear();
      c.Dock = DockStyle.Fill;
      this.Controls.Add(c);
    }

    private void onRefreshFeature(object sender, EventArgs e) => this.refreshFeatureState();

    private void onRetryAccess(object sender, EventArgs e) => this.refreshFeatureState();

    public override void Save()
    {
      using (CursorActivator.Wait())
      {
        if (this.userAssignmentPanel != null)
          this.userAssignmentPanel.Save();
        if (this.statusOnlineSetupPanel == null)
          return;
        this.statusOnlineSetupPanel.Save();
      }
    }

    public override void Reset()
    {
      using (CursorActivator.Wait())
        this.refreshFeatureState();
    }

    public override bool IsDirty
    {
      get
      {
        if (this.userAssignmentPanel != null)
          return this.userAssignmentPanel.IsDirty;
        return this.statusOnlineSetupPanel != null && this.statusOnlineSetupPanel.IsDirty;
      }
    }

    public void SaveTriggers()
    {
      StatusOnlineSetup statusOnlineSetup = this.session.ConfigurationManager.GetStatusOnlineSetup((string) null);
      List<string> stringList = !this.IsOnCompanyStatusOnlineTab ? new List<string>((IEnumerable<string>) this.SelectedTPOGUIDs) : new List<string>((IEnumerable<string>) this.SelectedGUIDs);
      foreach (StatusOnlineTrigger trigger in (CollectionBase) statusOnlineSetup.Triggers)
      {
        if (stringList.Contains(trigger.Guid))
          this.session.ConfigurationManager.SaveStatusOnlineSetup(trigger.OwnerID, statusOnlineSetup);
      }
    }
  }
}

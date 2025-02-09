// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AIQPersonaSettingPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AIQPersonaSettingPage : UserControl, IPersonaSecurityPage
  {
    private int personaID = -1;
    private Persona[] personas;
    private string userID = "";
    private Sessions.Session session;
    private bool isModified;
    private bool suspendEvent;
    private FeatureConfigsAclManager featureConfigAclMgr;
    private IContainer components;
    private CheckBox cboxAccessToAIQ;
    private GroupBox grpboxLunchingAIQ;
    private RadioButton rbtnAIQWeb;
    private RadioButton rbtnAIQDesktop;
    private ImageList imgListTv;
    private Label lblAIQAccess;
    private CheckBox cboxAccessToAIQAnalyzers;
    private Label lblAIQAnalyzersAccess;
    private CheckBox cboxImportAIQAnalyzerIncomeData;
    private Label lblImportAIQAnalyzerIncomeData;

    public event EventHandler DirtyFlagChanged;

    public AIQPersonaSettingPage(
      Sessions.Session session,
      int personaId,
      EventHandler dirtyFlagChanged)
    {
      this.suspendEvent = true;
      this.session = session;
      this.personaID = personaId;
      this.featureConfigAclMgr = (FeatureConfigsAclManager) this.session.ACL.GetAclManager(AclCategory.FeatureConfigs);
      this.InitializeComponent();
      this.grpboxLunchingAIQ.Enabled = false;
      this.rbtnAIQWeb.Checked = true;
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.loadPageForPersona();
      this.suspendEvent = false;
    }

    public AIQPersonaSettingPage(
      Sessions.Session session,
      string userID,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
    {
      this.suspendEvent = true;
      this.session = session;
      this.userID = userID;
      this.personas = personas;
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.featureConfigAclMgr = (FeatureConfigsAclManager) this.session.ACL.GetAclManager(AclCategory.FeatureConfigs);
      this.InitializeComponent();
      this.grpboxLunchingAIQ.Enabled = false;
      this.rbtnAIQWeb.Checked = true;
      this.loadPageForUser();
      this.suspendEvent = false;
    }

    private void loadPageForUser()
    {
      this.loadPageForUserByFeature(AclFeature.SettingsTab_EncompassAIQAccess);
      this.loadPageForUserByFeature(AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers);
      this.loadPageForUserByFeature(AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers_ImportIncomeData);
      this.loadPageForUserByFeature(AclFeature.SettingsTab_AIQLaunchType);
    }

    private void loadPageForUserByFeature(AclFeature aclFeature)
    {
      AIQPersonaSettingPage.PersonaAccessRight personaAccessRight = this.getPersonaAccessRight(aclFeature);
      if (personaAccessRight.UserSpecificAccessRight < 0)
      {
        this.MakeReadOnly(true, aclFeature);
        this.switchLinkImageIndex(aclFeature, 1);
        this.setUserControlOption(personaAccessRight.PersonaSetting, aclFeature);
      }
      else
      {
        this.MakeReadOnly(false, aclFeature);
        this.switchLinkImageIndex(aclFeature, 0);
        this.setUserControlOption(personaAccessRight.UserSpecificAccessRight, aclFeature);
      }
    }

    private void switchLinkImageIndex(AclFeature aclFeature, int index)
    {
      switch (aclFeature)
      {
        case AclFeature.SettingsTab_EncompassAIQAccess:
          this.lblAIQAccess.ImageIndex = index;
          break;
        case AclFeature.SettingsTab_AIQLaunchType:
          this.rbtnAIQDesktop.ImageIndex = index;
          this.rbtnAIQWeb.ImageIndex = index;
          break;
        case AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers:
          this.lblAIQAnalyzersAccess.ImageIndex = index;
          break;
        case AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers_ImportIncomeData:
          this.lblImportAIQAnalyzerIncomeData.ImageIndex = index;
          break;
      }
    }

    private void loadPageForPersona()
    {
      this.loadPageForPersonaByFeature(AclFeature.SettingsTab_EncompassAIQAccess);
      this.loadPageForPersonaByFeature(AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers);
      this.loadPageForPersonaByFeature(AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers_ImportIncomeData);
      this.loadPageForPersonaByFeature(AclFeature.SettingsTab_AIQLaunchType);
    }

    private void loadPageForPersonaByFeature(AclFeature aclFeature)
    {
      this.setUserControlOption(this.featureConfigAclMgr.GetPermission(aclFeature, this.personaID), aclFeature);
      this.setStatus(false);
    }

    private AIQPersonaSettingPage.PersonaAccessRight getPersonaAccessRight(AclFeature aclFeature)
    {
      AIQPersonaSettingPage.PersonaAccessRight personaAccessRight = new AIQPersonaSettingPage.PersonaAccessRight();
      personaAccessRight.UserSpecificAccessRight = this.featureConfigAclMgr.GetPermission(aclFeature, this.userID);
      Dictionary<AclFeature, int> permissions = this.featureConfigAclMgr.GetPermissions(new AclFeature[1]
      {
        aclFeature
      }, this.personas);
      personaAccessRight.PersonaSetting = permissions[aclFeature];
      return personaAccessRight;
    }

    private int selectedAIQWebAccess => this.rbtnAIQWeb.Checked ? 1 : 0;

    private int selectedAIQDesktopAccess => this.rbtnAIQDesktop.Checked ? 1 : 0;

    public bool IsDirty => this.isModified;

    public void SetPersona(int personaId)
    {
      this.suspendEvent = true;
      this.personaID = personaId;
      this.loadPageForPersona();
      this.suspendEvent = false;
      this.SetLaunchingAIQ(this.cboxAccessToAIQ.Checked);
    }

    private void SetLaunchingAIQ(bool state)
    {
      if (!state)
        this.grpboxLunchingAIQ.Enabled = false;
      else
        this.grpboxLunchingAIQ.Enabled = true;
    }

    private void setUserControlOption(int accessRight, AclFeature aclFeature)
    {
      switch (aclFeature)
      {
        case AclFeature.SettingsTab_EncompassAIQAccess:
          if (accessRight == 1)
          {
            this.cboxAccessToAIQ.Checked = true;
            this.grpboxLunchingAIQ.Enabled = true;
            break;
          }
          this.cboxAccessToAIQ.Checked = false;
          break;
        case AclFeature.SettingsTab_AIQLaunchType:
          if (accessRight == 1)
          {
            this.rbtnAIQDesktop.Checked = true;
            break;
          }
          this.rbtnAIQWeb.Checked = true;
          break;
        case AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers:
          if (accessRight == 1)
          {
            this.cboxAccessToAIQAnalyzers.Checked = true;
            this.cboxImportAIQAnalyzerIncomeData.Enabled = true;
            this.lblImportAIQAnalyzerIncomeData.Enabled = true;
            break;
          }
          this.cboxAccessToAIQAnalyzers.Checked = false;
          this.cboxImportAIQAnalyzerIncomeData.Enabled = false;
          this.lblImportAIQAnalyzerIncomeData.Enabled = false;
          break;
        case AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers_ImportIncomeData:
          if (accessRight == 1)
          {
            this.cboxImportAIQAnalyzerIncomeData.Checked = true;
            break;
          }
          this.cboxImportAIQAnalyzerIncomeData.Checked = false;
          break;
      }
    }

    private void setStatus(bool modified)
    {
      this.isModified = modified;
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, (EventArgs) null);
    }

    public void Save()
    {
      if (!this.IsDirty)
        return;
      if (this.personas == null)
      {
        this.featureConfigAclMgr.SetPermission(AclFeature.SettingsTab_EncompassAIQAccess, this.personaID, this.cboxAccessToAIQ.Checked ? 1 : 0);
        this.featureConfigAclMgr.SetPermission(AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers, this.personaID, this.cboxAccessToAIQAnalyzers.Checked ? 1 : 0);
        this.featureConfigAclMgr.SetPermission(AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers_ImportIncomeData, this.personaID, this.cboxImportAIQAnalyzerIncomeData.Checked ? 1 : 0);
        this.featureConfigAclMgr.SetPermission(AclFeature.SettingsTab_AIQLaunchType, this.personaID, this.rbtnAIQDesktop.Checked ? 1 : 0);
      }
      else
      {
        if (this.lblAIQAccess.ImageIndex == 0)
          this.featureConfigAclMgr.SetPermission(AclFeature.SettingsTab_EncompassAIQAccess, this.userID, this.cboxAccessToAIQ.Checked ? 1 : 0);
        else
          this.featureConfigAclMgr.ClearUserSpecificSettings(AclFeature.SettingsTab_EncompassAIQAccess, this.userID);
        if (this.lblAIQAnalyzersAccess.ImageIndex == 0)
          this.featureConfigAclMgr.SetPermission(AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers, this.userID, this.cboxAccessToAIQAnalyzers.Checked ? 1 : 0);
        else
          this.featureConfigAclMgr.ClearUserSpecificSettings(AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers, this.userID);
        if (this.lblImportAIQAnalyzerIncomeData.ImageIndex == 0)
          this.featureConfigAclMgr.SetPermission(AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers_ImportIncomeData, this.userID, this.cboxImportAIQAnalyzerIncomeData.Checked ? 1 : 0);
        else
          this.featureConfigAclMgr.ClearUserSpecificSettings(AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers_ImportIncomeData, this.userID);
        if (this.rbtnAIQDesktop.ImageIndex == 0 || this.rbtnAIQWeb.ImageIndex == 0)
          this.featureConfigAclMgr.SetPermission(AclFeature.SettingsTab_AIQLaunchType, this.userID, this.rbtnAIQDesktop.Checked ? 1 : 0);
        else
          this.featureConfigAclMgr.ClearUserSpecificSettings(AclFeature.SettingsTab_AIQLaunchType, this.userID);
      }
      this.setStatus(false);
    }

    public void Reset()
    {
      this.suspendEvent = true;
      if (this.userID == "")
        this.loadPageForPersona();
      else
        this.loadPageForUser();
      this.suspendEvent = false;
      this.setStatus(false);
    }

    public void MakeReadOnly(bool makeReadOnly)
    {
      this.MakeReadOnly(makeReadOnly, AclFeature.SettingsTab_EncompassAIQAccess);
      this.MakeReadOnly(makeReadOnly, AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers);
      this.MakeReadOnly(makeReadOnly, AclFeature.SettingsTab_AIQLaunchType);
    }

    public void MakeReadOnly(bool makeReadOnly, AclFeature aclFeature)
    {
      switch (aclFeature)
      {
        case AclFeature.SettingsTab_EncompassAIQAccess:
          this.cboxAccessToAIQ.Enabled = !makeReadOnly;
          break;
        case AclFeature.SettingsTab_AIQLaunchType:
          this.rbtnAIQWeb.Enabled = !makeReadOnly;
          break;
        case AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers:
          this.cboxAccessToAIQAnalyzers.Enabled = !makeReadOnly;
          break;
      }
    }

    private void cboxAccessToAIQ_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      if (this.userID != "")
        this.lblAIQAccess.ImageIndex = 0;
      this.setStatus(true);
      if (this.cboxAccessToAIQ.Checked)
      {
        this.grpboxLunchingAIQ.Enabled = true;
        this.rbtnAIQWeb.Checked = true;
      }
      else
      {
        this.rbtnAIQDesktop.Checked = false;
        this.rbtnAIQWeb.Checked = false;
        this.grpboxLunchingAIQ.Enabled = false;
      }
    }

    private void cboxAccessToAIQAnalyzers_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      if (this.userID != "")
        this.lblAIQAnalyzersAccess.ImageIndex = 0;
      this.setStatus(true);
      if (this.cboxAccessToAIQAnalyzers.Checked)
      {
        this.cboxImportAIQAnalyzerIncomeData.Enabled = true;
        this.lblImportAIQAnalyzerIncomeData.Enabled = true;
      }
      else
      {
        this.cboxImportAIQAnalyzerIncomeData.Checked = false;
        this.cboxImportAIQAnalyzerIncomeData.Enabled = false;
        this.lblImportAIQAnalyzerIncomeData.Enabled = false;
      }
    }

    private void cboxImportAIQAnalyzerIncomeData_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      if (this.userID != "")
        this.lblImportAIQAnalyzerIncomeData.ImageIndex = 0;
      this.setStatus(true);
    }

    private void rbtnAIQDesktop_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      if (this.userID != "")
        this.rbtnAIQDesktop.ImageIndex = 0;
      this.setStatus(true);
    }

    private void rbtnAIQWeb_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      if (this.userID != "")
        this.rbtnAIQWeb.ImageIndex = 0;
      this.setStatus(true);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AIQPersonaSettingPage));
      this.cboxAccessToAIQ = new CheckBox();
      this.grpboxLunchingAIQ = new GroupBox();
      this.rbtnAIQWeb = new RadioButton();
      this.rbtnAIQDesktop = new RadioButton();
      this.imgListTv = new ImageList(this.components);
      this.lblAIQAccess = new Label();
      this.cboxAccessToAIQAnalyzers = new CheckBox();
      this.cboxImportAIQAnalyzerIncomeData = new CheckBox();
      this.lblAIQAnalyzersAccess = new Label();
      this.lblImportAIQAnalyzerIncomeData = new Label();
      this.grpboxLunchingAIQ.SuspendLayout();
      this.SuspendLayout();
      this.cboxAccessToAIQ.AutoSize = true;
      this.cboxAccessToAIQ.Location = new Point(20, 18);
      this.cboxAccessToAIQ.Name = "cboxAccessToAIQ";
      this.cboxAccessToAIQ.Size = new Size(15, 14);
      this.cboxAccessToAIQ.TabIndex = 1;
      this.cboxAccessToAIQ.UseVisualStyleBackColor = true;
      this.cboxAccessToAIQ.CheckedChanged += new EventHandler(this.cboxAccessToAIQ_CheckedChanged);
      this.grpboxLunchingAIQ.BackgroundImageLayout = ImageLayout.None;
      this.grpboxLunchingAIQ.Controls.Add((Control) this.rbtnAIQWeb);
      this.grpboxLunchingAIQ.Controls.Add((Control) this.rbtnAIQDesktop);
      this.grpboxLunchingAIQ.Location = new Point(41, 30);
      this.grpboxLunchingAIQ.Name = "grpboxLunchingAIQ";
      this.grpboxLunchingAIQ.Size = new Size(279, 50);
      this.grpboxLunchingAIQ.TabIndex = 2;
      this.grpboxLunchingAIQ.TabStop = false;
      this.rbtnAIQWeb.AutoSize = true;
      this.rbtnAIQWeb.Location = new Point(13, 31);
      this.rbtnAIQWeb.Name = "rbtnAIQWeb";
      this.rbtnAIQWeb.Size = new Size(242, 17);
      this.rbtnAIQWeb.TabIndex = 4;
      this.rbtnAIQWeb.TabStop = true;
      this.rbtnAIQWeb.Text = "    Launch Data && Document Automation Web";
      this.rbtnAIQWeb.UseVisualStyleBackColor = true;
      this.rbtnAIQWeb.CheckedChanged += new EventHandler(this.rbtnAIQWeb_CheckedChanged);
      this.rbtnAIQDesktop.AutoSize = true;
      this.rbtnAIQDesktop.Location = new Point(13, 9);
      this.rbtnAIQDesktop.Name = "rbtnAIQDesktop";
      this.rbtnAIQDesktop.Size = new Size(259, 17);
      this.rbtnAIQDesktop.TabIndex = 3;
      this.rbtnAIQDesktop.TabStop = true;
      this.rbtnAIQDesktop.Text = "    Launch Data && Document Automation Desktop";
      this.rbtnAIQDesktop.UseVisualStyleBackColor = true;
      this.rbtnAIQDesktop.CheckedChanged += new EventHandler(this.rbtnAIQDesktop_CheckedChanged);
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "");
      this.imgListTv.Images.SetKeyName(1, "");
      this.lblAIQAccess.AutoSize = true;
      this.lblAIQAccess.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblAIQAccess.ImageList = this.imgListTv;
      this.lblAIQAccess.Location = new Point(38, 18);
      this.lblAIQAccess.Name = "lblAIQAccess";
      this.lblAIQAccess.Size = new Size(212, 13);
      this.lblAIQAccess.TabIndex = 3;
      this.lblAIQAccess.Text = "      Access to Data && Document Automation";
      this.cboxAccessToAIQAnalyzers.AutoSize = true;
      this.cboxAccessToAIQAnalyzers.Location = new Point(20, 112);
      this.cboxAccessToAIQAnalyzers.Name = "cboxAccessToAIQAnalyzers";
      this.cboxAccessToAIQAnalyzers.Size = new Size(15, 14);
      this.cboxAccessToAIQAnalyzers.TabIndex = 5;
      this.cboxAccessToAIQAnalyzers.UseVisualStyleBackColor = true;
      this.cboxAccessToAIQAnalyzers.CheckedChanged += new EventHandler(this.cboxAccessToAIQAnalyzers_CheckedChanged);
      this.cboxImportAIQAnalyzerIncomeData.AutoSize = true;
      this.cboxImportAIQAnalyzerIncomeData.Location = new Point(54, 132);
      this.cboxImportAIQAnalyzerIncomeData.Name = "cboxImportAIQAnalyzerIncomeData";
      this.cboxImportAIQAnalyzerIncomeData.Size = new Size(15, 14);
      this.cboxImportAIQAnalyzerIncomeData.TabIndex = 6;
      this.cboxImportAIQAnalyzerIncomeData.UseVisualStyleBackColor = true;
      this.cboxImportAIQAnalyzerIncomeData.CheckedChanged += new EventHandler(this.cboxImportAIQAnalyzerIncomeData_CheckedChanged);
      this.lblAIQAnalyzersAccess.AutoSize = true;
      this.lblAIQAnalyzersAccess.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblAIQAnalyzersAccess.ImageList = this.imgListTv;
      this.lblAIQAnalyzersAccess.Location = new Point(38, 112);
      this.lblAIQAnalyzersAccess.Name = "lblAIQAnalyzersAccess";
      this.lblAIQAnalyzersAccess.Size = new Size(165, 13);
      this.lblAIQAnalyzersAccess.TabIndex = 6;
      this.lblAIQAnalyzersAccess.Text = "      Access to Mortgage Analyzers";
      this.lblImportAIQAnalyzerIncomeData.AutoSize = true;
      this.lblImportAIQAnalyzerIncomeData.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblImportAIQAnalyzerIncomeData.ImageList = this.imgListTv;
      this.lblImportAIQAnalyzerIncomeData.Location = new Point(67, 132);
      this.lblImportAIQAnalyzerIncomeData.Name = "lblImportAIQAnalyzerIncomeData";
      this.lblImportAIQAnalyzerIncomeData.Size = new Size(158, 13);
      this.lblImportAIQAnalyzerIncomeData.TabIndex = 7;
      this.lblImportAIQAnalyzerIncomeData.Text = "      Import Income Analyzer Data";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.lblAIQAnalyzersAccess);
      this.Controls.Add((Control) this.cboxAccessToAIQAnalyzers);
      this.Controls.Add((Control) this.cboxImportAIQAnalyzerIncomeData);
      this.Controls.Add((Control) this.lblImportAIQAnalyzerIncomeData);
      this.Controls.Add((Control) this.lblAIQAccess);
      this.Controls.Add((Control) this.grpboxLunchingAIQ);
      this.Controls.Add((Control) this.cboxAccessToAIQ);
      this.Name = nameof (AIQPersonaSettingPage);
      this.Size = new Size(591, 359);
      this.grpboxLunchingAIQ.ResumeLayout(false);
      this.grpboxLunchingAIQ.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private class SCPackage
    {
      public readonly SCPackageInfo Settings;
      public readonly int Index;

      public SCPackage(SCPackageInfo settings, int index)
      {
        this.Settings = settings;
        this.Index = index;
      }
    }

    private struct PersonaAccessRight
    {
      public int UserSpecificAccessRight { get; set; }

      public int PersonaSetting { get; set; }
    }
  }
}

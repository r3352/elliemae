// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TPOAdminPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TPOAdminPage : Form, IPersonaSecurityPage
  {
    private Sessions.Session session;
    private TPOAdministrationPage tpoAdministrationPage;
    private TPOSiteSettingsPage tpoSiteSettingsPage;
    private IContainer components;
    private PanelEx pnlExTPOAdminPage;
    private PanelEx panelTPOSiteSettings;

    public event EventHandler DirtyFlagChanged;

    public TPOAdminPage(Sessions.Session session, int personaId, EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.session = session;
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.tpoAdministrationPage = new TPOAdministrationPage(this.session, personaId, this.DirtyFlagChanged);
      this.tpoAdministrationPage.TopLevel = false;
      this.tpoAdministrationPage.Visible = true;
      this.tpoAdministrationPage.Dock = DockStyle.Fill;
      this.pnlExTPOAdminPage.Controls.Add((Control) this.tpoAdministrationPage);
      this.tpoSiteSettingsPage = new TPOSiteSettingsPage(this.session, personaId, this.DirtyFlagChanged);
      this.tpoSiteSettingsPage.TopLevel = false;
      this.tpoSiteSettingsPage.Visible = true;
      this.tpoSiteSettingsPage.Dock = DockStyle.Fill;
      this.panelTPOSiteSettings.Controls.Add((Control) this.tpoSiteSettingsPage);
    }

    public TPOAdminPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.tpoAdministrationPage = new TPOAdministrationPage(session, userId, personas, this.DirtyFlagChanged);
      this.tpoAdministrationPage.TopLevel = false;
      this.tpoAdministrationPage.Visible = true;
      this.tpoAdministrationPage.Dock = DockStyle.Fill;
      this.pnlExTPOAdminPage.Controls.Add((Control) this.tpoAdministrationPage);
      this.tpoSiteSettingsPage = new TPOSiteSettingsPage(session, userId, personas, this.DirtyFlagChanged);
      this.tpoSiteSettingsPage.TopLevel = false;
      this.tpoSiteSettingsPage.Visible = true;
      this.tpoSiteSettingsPage.Dock = DockStyle.Fill;
      this.panelTPOSiteSettings.Controls.Add((Control) this.tpoSiteSettingsPage);
    }

    public virtual void SetPersona(int personaId)
    {
      this.tpoAdministrationPage.SetPersona(personaId);
      this.tpoSiteSettingsPage.SetPersona(personaId);
      this.tpoAdministrationPage.PersonaID = personaId;
      this.tpoSiteSettingsPage.PersonaID = personaId;
    }

    private void SettingsPage_BackColorChanged(object sender, EventArgs e)
    {
      this.tpoAdministrationPage.BackColor = this.BackColor;
      this.tpoSiteSettingsPage.BackColor = this.BackColor;
    }

    public void SaveData()
    {
      this.tpoAdministrationPage.Save();
      this.tpoSiteSettingsPage.Save();
    }

    public void ResetData()
    {
      this.tpoAdministrationPage.Reset();
      this.tpoSiteSettingsPage.Reset();
    }

    public bool NeedToSaveData()
    {
      return this.tpoAdministrationPage.NeedToSaveData() || this.tpoSiteSettingsPage.NeedToSaveData();
    }

    public void MakeReadOnly(bool makeReadOnly)
    {
      this.tpoAdministrationPage.MakeReadOnly(makeReadOnly);
      this.tpoSiteSettingsPage.MakeReadOnly(makeReadOnly);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlExTPOAdminPage = new PanelEx();
      this.panelTPOSiteSettings = new PanelEx();
      this.SuspendLayout();
      this.pnlExTPOAdminPage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.pnlExTPOAdminPage.Location = new Point(0, 0);
      this.pnlExTPOAdminPage.Margin = new Padding(1);
      this.pnlExTPOAdminPage.Name = "pnlExTPOAdminPage";
      this.pnlExTPOAdminPage.Size = new Size(232, 380);
      this.pnlExTPOAdminPage.TabIndex = 1;
      this.panelTPOSiteSettings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelTPOSiteSettings.Location = new Point(234, 0);
      this.panelTPOSiteSettings.Margin = new Padding(1);
      this.panelTPOSiteSettings.Name = "panelTPOSiteSettings";
      this.panelTPOSiteSettings.Size = new Size(358, 380);
      this.panelTPOSiteSettings.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(593, 382);
      this.Controls.Add((Control) this.panelTPOSiteSettings);
      this.Controls.Add((Control) this.pnlExTPOAdminPage);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (TPOAdminPage);
      this.Text = nameof (TPOAdminPage);
      this.ResumeLayout(false);
    }
  }
}

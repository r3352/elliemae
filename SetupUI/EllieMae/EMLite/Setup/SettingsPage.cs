// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SettingsPage
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
  public class SettingsPage : Form, IPersonaSecurityPage
  {
    private IContainer components;
    private SettingsCompanyPage companyPage;
    private FormBuilderPage formBuilderPage;
    private SettingsPersonalPage personalPage;
    private ImageList imgListTv;
    private ContextMenu contextMenu1;
    private MenuItem menuItemLinkWithPersona;
    private MenuItem menuItemDisconnectFromPersona;
    private Panel panelCompanyPage;
    private Splitter splitter1;
    private PanelEx pnlExFormBuilder;
    private Splitter splitter2;
    private PanelEx pnlExCompanySettings;
    private Panel panelPersonalPage;
    private Sessions.Session session;

    public event EventHandler DirtyFlagChanged;

    public SettingsPage(Sessions.Session session, int personaId, EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.session = session;
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.companyPage = new SettingsCompanyPage(this.session, personaId, this.DirtyFlagChanged);
      this.companyPage.TopLevel = false;
      this.companyPage.Visible = true;
      this.companyPage.Dock = DockStyle.Fill;
      this.pnlExCompanySettings.Controls.Add((Control) this.companyPage);
      this.formBuilderPage = new FormBuilderPage(this.session, personaId, this.DirtyFlagChanged);
      this.formBuilderPage.TopLevel = false;
      this.formBuilderPage.Visible = true;
      this.formBuilderPage.Dock = DockStyle.Fill;
      this.pnlExFormBuilder.Controls.Add((Control) this.formBuilderPage);
      this.personalPage = new SettingsPersonalPage(session, personaId, this.DirtyFlagChanged);
      this.personalPage.TopLevel = false;
      this.personalPage.Visible = true;
      this.personalPage.Dock = DockStyle.Fill;
      this.panelPersonalPage.Controls.Add((Control) this.personalPage);
    }

    public SettingsPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.companyPage = new SettingsCompanyPage(session, userId, personas, this.DirtyFlagChanged);
      this.companyPage.TopLevel = false;
      this.companyPage.Visible = true;
      this.companyPage.Dock = DockStyle.Fill;
      this.pnlExCompanySettings.Controls.Add((Control) this.companyPage);
      this.formBuilderPage = new FormBuilderPage(session, userId, personas, this.DirtyFlagChanged);
      this.formBuilderPage.TopLevel = false;
      this.formBuilderPage.Visible = true;
      this.formBuilderPage.Dock = DockStyle.Fill;
      this.pnlExFormBuilder.Controls.Add((Control) this.formBuilderPage);
      this.personalPage = new SettingsPersonalPage(session, userId, personas, this.DirtyFlagChanged);
      this.personalPage.TopLevel = false;
      this.personalPage.Visible = true;
      this.personalPage.Dock = DockStyle.Fill;
      this.panelPersonalPage.Controls.Add((Control) this.personalPage);
    }

    public virtual void SetPersona(int personaId)
    {
      this.formBuilderPage.SetPersona(personaId);
      this.companyPage.SetPersona(personaId);
      this.companyPage.PersonaID = personaId;
      this.personalPage.SetPersona(personaId);
      this.personalPage.PersonaID = personaId;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SettingsPage));
      this.imgListTv = new ImageList(this.components);
      this.contextMenu1 = new ContextMenu();
      this.menuItemLinkWithPersona = new MenuItem();
      this.menuItemDisconnectFromPersona = new MenuItem();
      this.panelCompanyPage = new Panel();
      this.pnlExCompanySettings = new PanelEx();
      this.splitter2 = new Splitter();
      this.pnlExFormBuilder = new PanelEx();
      this.panelPersonalPage = new Panel();
      this.splitter1 = new Splitter();
      this.panelCompanyPage.SuspendLayout();
      this.SuspendLayout();
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "");
      this.imgListTv.Images.SetKeyName(1, "");
      this.contextMenu1.MenuItems.AddRange(new MenuItem[2]
      {
        this.menuItemLinkWithPersona,
        this.menuItemDisconnectFromPersona
      });
      this.menuItemLinkWithPersona.Index = 0;
      this.menuItemLinkWithPersona.Text = "Link with Persona Rights";
      this.menuItemDisconnectFromPersona.Index = 1;
      this.menuItemDisconnectFromPersona.Text = "Disconnect from Persona Rights";
      this.panelCompanyPage.Controls.Add((Control) this.pnlExCompanySettings);
      this.panelCompanyPage.Controls.Add((Control) this.splitter2);
      this.panelCompanyPage.Controls.Add((Control) this.pnlExFormBuilder);
      this.panelCompanyPage.Dock = DockStyle.Left;
      this.panelCompanyPage.Location = new Point(0, 0);
      this.panelCompanyPage.Name = "panelCompanyPage";
      this.panelCompanyPage.Size = new Size(287, 410);
      this.panelCompanyPage.TabIndex = 12;
      this.pnlExCompanySettings.Dock = DockStyle.Fill;
      this.pnlExCompanySettings.Location = new Point(0, 0);
      this.pnlExCompanySettings.Name = "pnlExCompanySettings";
      this.pnlExCompanySettings.Size = new Size(287, 332);
      this.pnlExCompanySettings.TabIndex = 0;
      this.splitter2.Dock = DockStyle.Bottom;
      this.splitter2.Location = new Point(0, 332);
      this.splitter2.Name = "splitter2";
      this.splitter2.Size = new Size(287, 3);
      this.splitter2.TabIndex = 1;
      this.splitter2.TabStop = false;
      this.pnlExFormBuilder.Dock = DockStyle.Bottom;
      this.pnlExFormBuilder.Location = new Point(0, 335);
      this.pnlExFormBuilder.Name = "pnlExFormBuilder";
      this.pnlExFormBuilder.Size = new Size(287, 75);
      this.pnlExFormBuilder.TabIndex = 2;
      this.panelPersonalPage.Dock = DockStyle.Fill;
      this.panelPersonalPage.Location = new Point(290, 0);
      this.panelPersonalPage.Name = "panelPersonalPage";
      this.panelPersonalPage.Size = new Size(284, 410);
      this.panelPersonalPage.TabIndex = 13;
      this.splitter1.Location = new Point(287, 0);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(3, 410);
      this.splitter1.TabIndex = 0;
      this.splitter1.TabStop = false;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(574, 410);
      this.Controls.Add((Control) this.panelPersonalPage);
      this.Controls.Add((Control) this.splitter1);
      this.Controls.Add((Control) this.panelCompanyPage);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (SettingsPage);
      this.Text = "BorSearchLoanInfoPage";
      this.BackColorChanged += new EventHandler(this.SettingsPage_BackColorChanged);
      this.panelCompanyPage.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void SettingsPage_BackColorChanged(object sender, EventArgs e)
    {
      this.formBuilderPage.BackColor = this.BackColor;
      this.companyPage.BackColor = this.BackColor;
      this.personalPage.BackColor = this.BackColor;
    }

    public void SaveData()
    {
      this.personalPage.Save();
      this.companyPage.Save();
      this.formBuilderPage.Save();
    }

    public void ResetData()
    {
      this.personalPage.Reset();
      this.companyPage.Reset();
      this.formBuilderPage.Reset();
    }

    public bool NeedToSaveData()
    {
      return this.personalPage.NeedToSaveData() || this.companyPage.NeedToSaveData() || this.formBuilderPage.NeedToSaveData();
    }

    public void MakeReadOnly(bool makeReadOnly)
    {
      this.companyPage.MakeReadOnly(makeReadOnly);
      this.personalPage.MakeReadOnly(makeReadOnly);
      this.formBuilderPage.MakeReadOnly(makeReadOnly);
    }
  }
}

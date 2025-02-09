// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExtSettingsPage
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
  public class ExtSettingsPage : Form, IPersonaSecurityPage
  {
    private IContainer components;
    private ExternalSettingsPage externalSettingsPage;
    private ImageList imgListTv;
    private ContextMenu contextMenu1;
    private MenuItem menuItemLinkWithPersona;
    private MenuItem menuItemDisconnectFromPersona;
    private Panel panelCompanyPage;
    private PanelEx pnlExExtSettings;
    private Sessions.Session session;

    public event EventHandler DirtyFlagChanged;

    public ExtSettingsPage(Sessions.Session session, int personaId, EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.session = session;
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.externalSettingsPage = new ExternalSettingsPage(this.session, personaId, this.DirtyFlagChanged);
      this.externalSettingsPage.TopLevel = false;
      this.externalSettingsPage.Visible = true;
      this.externalSettingsPage.Dock = DockStyle.Fill;
      this.pnlExExtSettings.Controls.Add((Control) this.externalSettingsPage);
    }

    public ExtSettingsPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.externalSettingsPage = new ExternalSettingsPage(session, userId, personas, this.DirtyFlagChanged);
      this.externalSettingsPage.TopLevel = false;
      this.externalSettingsPage.Visible = true;
      this.externalSettingsPage.Dock = DockStyle.Fill;
      this.pnlExExtSettings.Controls.Add((Control) this.externalSettingsPage);
    }

    public virtual void SetPersona(int personaId)
    {
      this.externalSettingsPage.SetPersona(personaId);
      this.externalSettingsPage.PersonaID = personaId;
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
      this.pnlExExtSettings = new PanelEx();
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
      this.panelCompanyPage.Controls.Add((Control) this.pnlExExtSettings);
      this.panelCompanyPage.Dock = DockStyle.Fill;
      this.panelCompanyPage.Location = new Point(0, 0);
      this.panelCompanyPage.Name = "panelCompanyPage";
      this.panelCompanyPage.Size = new Size(287, 410);
      this.panelCompanyPage.TabIndex = 12;
      this.pnlExExtSettings.Dock = DockStyle.Fill;
      this.pnlExExtSettings.Location = new Point(0, 0);
      this.pnlExExtSettings.Name = "pnlExCompanySettings";
      this.pnlExExtSettings.Size = new Size(287, 353);
      this.pnlExExtSettings.TabIndex = 0;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(574, 410);
      this.Controls.Add((Control) this.panelCompanyPage);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = "SettingsPage";
      this.Text = "BorSearchLoanInfoPage";
      this.BackColorChanged += new EventHandler(this.SettingsPage_BackColorChanged);
      this.panelCompanyPage.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void SettingsPage_BackColorChanged(object sender, EventArgs e)
    {
      this.externalSettingsPage.BackColor = this.BackColor;
    }

    public void SaveData() => this.externalSettingsPage.Save();

    public void ResetData() => this.externalSettingsPage.Reset();

    public bool NeedToSaveData() => this.externalSettingsPage.NeedToSaveData();

    public void MakeReadOnly(bool makeReadOnly)
    {
      this.externalSettingsPage.MakeReadOnly(makeReadOnly);
    }
  }
}

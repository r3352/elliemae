// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EVaultPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EVaultPage : Form, IPersonaSecurityPage
  {
    private IContainer components;
    private EVaultSettingsPage eVaultPage;
    private ImageList imgListTv;
    private ContextMenu contextMenu1;
    private MenuItem menuItemLinkWithPersona;
    private MenuItem menuItemDisconnectFromPersona;
    private Panel panelEVaultPage;
    private Sessions.Session session;

    public event EventHandler DirtyFlagChanged;

    public EVaultPage(Sessions.Session session, int personaId, EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.session = session;
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.eVaultPage = new EVaultSettingsPage(session, personaId, this.DirtyFlagChanged);
      this.eVaultPage.TopLevel = false;
      this.eVaultPage.Visible = true;
      this.eVaultPage.Dock = DockStyle.Fill;
      this.panelEVaultPage.Controls.Add((Control) this.eVaultPage);
    }

    public EVaultPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.eVaultPage = new EVaultSettingsPage(session, userId, personas, this.DirtyFlagChanged);
      this.eVaultPage.TopLevel = false;
      this.eVaultPage.Visible = true;
      this.eVaultPage.Dock = DockStyle.Fill;
      this.panelEVaultPage.Controls.Add((Control) this.eVaultPage);
    }

    public virtual void SetPersona(int personaId) => this.eVaultPage.SetPersona(personaId);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EVaultPage));
      this.imgListTv = new ImageList(this.components);
      this.contextMenu1 = new ContextMenu();
      this.menuItemLinkWithPersona = new MenuItem();
      this.menuItemDisconnectFromPersona = new MenuItem();
      this.panelEVaultPage = new Panel();
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
      this.panelEVaultPage.Dock = DockStyle.Fill;
      this.panelEVaultPage.Location = new Point(0, 0);
      this.panelEVaultPage.Name = "panelEVaultPage";
      this.panelEVaultPage.Size = new Size(574, 410);
      this.panelEVaultPage.TabIndex = 13;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(574, 410);
      this.Controls.Add((Control) this.panelEVaultPage);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (EVaultPage);
      this.Text = "BorSearchLoanInfoPage";
      this.BackColorChanged += new EventHandler(this.SettingsPage_BackColorChanged);
      this.ResumeLayout(false);
    }

    private void SettingsPage_BackColorChanged(object sender, EventArgs e)
    {
      this.eVaultPage.BackColor = this.BackColor;
    }

    public void SaveData() => this.eVaultPage.Save();

    public void ResetData() => this.eVaultPage.Reset();

    public bool NeedToSaveData() => this.eVaultPage.NeedToSaveData();

    public void MakeReadOnly(bool makeReadOnly) => this.eVaultPage.MakeReadOnly(makeReadOnly);
  }
}

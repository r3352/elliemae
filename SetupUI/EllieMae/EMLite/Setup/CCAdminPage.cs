// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CCAdminPage
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
  public class CCAdminPage : Form, IPersonaSecurityPage
  {
    private Sessions.Session session;
    private ConsumerConnectAdminPage consumerConnectAdminPage;
    private IContainer components;
    private Panel panelCCAdmin;

    public event EventHandler DirtyFlagChanged;

    public CCAdminPage(Sessions.Session session, int personaId, EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.InitializeComponent();
      this.consumerConnectAdminPage = new ConsumerConnectAdminPage(this.session, personaId, this.DirtyFlagChanged);
      this.consumerConnectAdminPage.TopLevel = false;
      this.consumerConnectAdminPage.Visible = true;
      this.consumerConnectAdminPage.Dock = DockStyle.Fill;
      this.panelCCAdmin.Controls.Add((Control) this.consumerConnectAdminPage);
    }

    public CCAdminPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.InitializeComponent();
      this.consumerConnectAdminPage = new ConsumerConnectAdminPage(session, userId, personas, this.DirtyFlagChanged);
      this.consumerConnectAdminPage.TopLevel = false;
      this.consumerConnectAdminPage.Visible = true;
      this.consumerConnectAdminPage.Dock = DockStyle.Fill;
      this.panelCCAdmin.Controls.Add((Control) this.consumerConnectAdminPage);
    }

    public virtual void SetPersona(int personaId)
    {
      this.consumerConnectAdminPage.SetPersona(personaId);
      this.consumerConnectAdminPage.PersonaID = personaId;
    }

    private void SettingsPage_BackColorChanged(object sender, EventArgs e)
    {
      this.consumerConnectAdminPage.BackColor = this.BackColor;
    }

    public void SaveData() => this.consumerConnectAdminPage.Save();

    public void ResetData() => this.consumerConnectAdminPage.Reset();

    public bool NeedToSaveData() => this.consumerConnectAdminPage.NeedToSaveData();

    public void MakeReadOnly(bool makeReadOnly)
    {
      this.consumerConnectAdminPage.MakeReadOnly(makeReadOnly);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panelCCAdmin = new Panel();
      this.SuspendLayout();
      this.panelCCAdmin.Dock = DockStyle.Fill;
      this.panelCCAdmin.Location = new Point(0, 0);
      this.panelCCAdmin.Name = "panelCCAdmin";
      this.panelCCAdmin.Size = new Size(577, 403);
      this.panelCCAdmin.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(577, 403);
      this.Controls.Add((Control) this.panelCCAdmin);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (CCAdminPage);
      this.Text = nameof (CCAdminPage);
      this.ResumeLayout(false);
    }
  }
}

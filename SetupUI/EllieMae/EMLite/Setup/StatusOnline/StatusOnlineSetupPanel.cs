// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.StatusOnline.StatusOnlineSetupPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.StatusOnline
{
  public class StatusOnlineSetupPanel : SettingsUserControl
  {
    private Sessions.Session session;
    private string ownerID;
    private StatusOnlineSetup setup;
    private StatusOnlineTriggersControl wcTriggersCtl;
    private StatusOnlineTriggersControl tpoTriggersCtl;
    private StatusOnlineEmailControl emailCtl;
    private StatusOnlineUsersControl usersCtl;
    private bool isSettingSync;
    private IContainer components;
    private TabControl tabSetup;
    private TabPage pageUsers;
    private TabPage pageEmail;
    private TabPage pageWebCenter;
    private BorderPanel pnlBorder;
    private TabPage pageTPO;

    public bool IsOnCompanyStatusOnlineTab => this.tabSetup.SelectedTab == this.pageWebCenter;

    public bool IsOnEmailTemplatesTab => this.tabSetup.SelectedTab == this.pageEmail;

    public bool IsOnTPOStatusOnlineTab => this.tabSetup.SelectedTab == this.pageTPO;

    public string[] SelectedEmailTemplateGUIDs
    {
      get => this.emailCtl != null ? this.emailCtl.SelectedHtmlTemplateGUIDs : (string[]) null;
      set
      {
        if (value == null || value.Length == 0)
          return;
        this.tabSetup.SelectedTab = this.pageEmail;
        if (this.emailCtl == null)
          this.initEmailSetup();
        if (this.emailCtl == null)
          return;
        this.emailCtl.SelectedHtmlTemplateGUIDs = value;
      }
    }

    public string[] SelectedGUIDs
    {
      get => this.wcTriggersCtl != null ? this.wcTriggersCtl.SelectedGUIDs : (string[]) null;
      set
      {
        if (value == null || value.Length == 0 || this.wcTriggersCtl == null)
          return;
        this.tabSetup.SelectedTab = this.pageWebCenter;
        this.wcTriggersCtl.SelectedGUIDs = value;
      }
    }

    public string[] SelectedTPOGUIDs
    {
      get => this.tpoTriggersCtl != null ? this.tpoTriggersCtl.SelectedGUIDs : (string[]) null;
      set
      {
        if (value == null || value.Length == 0)
          return;
        this.tabSetup.SelectedTab = this.pageTPO;
        if (this.tpoTriggersCtl == null)
          this.initTPOSetup();
        if (this.tpoTriggersCtl == null)
          return;
        this.tpoTriggersCtl.SelectedGUIDs = value;
      }
    }

    public StatusOnlineSetupPanel(
      Sessions.Session session,
      SetUpContainer setupContainer,
      string ownerID)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      this.ownerID = ownerID;
      this.isSettingSync = setupContainer == null;
      this.setup = session.ConfigurationManager.GetStatusOnlineSetup(this.ownerID);
      if (!string.IsNullOrEmpty(ownerID))
      {
        this.pageWebCenter.Text = "Status Online Templates";
        this.tabSetup.TabPages.Remove(this.pageTPO);
        this.tabSetup.TabPages.Remove(this.pageUsers);
      }
      else if (session.ConfigurationManager.GetExternalOrganizationURLs().Length == 0)
        this.tabSetup.TabPages.Remove(this.pageTPO);
      this.initWebCenterSetup();
      if (setupContainer != null)
        return;
      this.tabSetup.TabPages.Remove(this.pageUsers);
    }

    private void initWebCenterSetup()
    {
      this.wcTriggersCtl = new StatusOnlineTriggersControl(this.session, this.setup, TriggerPortalType.WebCenter, this.ownerID, this.isSettingSync);
      this.wcTriggersCtl.Dock = DockStyle.Fill;
      this.pageWebCenter.Controls.Add((Control) this.wcTriggersCtl);
    }

    private void initTPOSetup()
    {
      this.tpoTriggersCtl = new StatusOnlineTriggersControl(this.session, this.setup, TriggerPortalType.TPOWC, this.ownerID, this.isSettingSync);
      this.tpoTriggersCtl.Dock = DockStyle.Fill;
      this.pageTPO.Controls.Add((Control) this.tpoTriggersCtl);
    }

    private void initEmailSetup()
    {
      this.emailCtl = new StatusOnlineEmailControl(this.session, this.ownerID, this.isSettingSync);
      this.emailCtl.Dock = DockStyle.Fill;
      this.pageEmail.Controls.Add((Control) this.emailCtl);
      this.emailCtl.HtmlEmailTemplateChanged += new HtmlEmailTemplateChangedEventHandler(this.emailCtl_HtmlEmailTemplateChanged);
    }

    private void initUserSetup()
    {
      this.usersCtl = new StatusOnlineUsersControl(this.session, this.setup);
      this.usersCtl.Dock = DockStyle.Fill;
      this.pageUsers.Controls.Add((Control) this.usersCtl);
    }

    private void emailCtl_HtmlEmailTemplateChanged(object sender, HtmlEmailTemplate template)
    {
      this.wcTriggersCtl.ProcessEmailTemplateChange(template);
      if (this.tpoTriggersCtl == null)
        return;
      this.tpoTriggersCtl.ProcessEmailTemplateChange(template);
    }

    private void tabSetup_Selecting(object sender, TabControlCancelEventArgs e)
    {
      if (e.TabPage == this.pageTPO)
      {
        if (this.tpoTriggersCtl != null)
          return;
        this.initTPOSetup();
      }
      else if (e.TabPage == this.pageEmail)
      {
        if (this.emailCtl != null)
          return;
        this.initEmailSetup();
      }
      else
      {
        if (e.TabPage != this.pageUsers || this.usersCtl != null)
          return;
        this.initUserSetup();
      }
    }

    public override bool IsDirty => this.usersCtl != null && this.usersCtl.IsDirty;

    public override void Reset()
    {
      if (this.usersCtl == null)
        return;
      this.usersCtl.Reset();
    }

    public override void Save()
    {
      if (this.usersCtl == null)
        return;
      this.usersCtl.Save();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.tabSetup = new TabControl();
      this.pageWebCenter = new TabPage();
      this.pageTPO = new TabPage();
      this.pageEmail = new TabPage();
      this.pageUsers = new TabPage();
      this.pnlBorder = new BorderPanel();
      this.tabSetup.SuspendLayout();
      this.SuspendLayout();
      this.tabSetup.Controls.Add((Control) this.pageWebCenter);
      this.tabSetup.Controls.Add((Control) this.pageTPO);
      this.tabSetup.Controls.Add((Control) this.pageEmail);
      this.tabSetup.Controls.Add((Control) this.pageUsers);
      this.tabSetup.Dock = DockStyle.Fill;
      this.tabSetup.HotTrack = true;
      this.tabSetup.ItemSize = new Size(74, 28);
      this.tabSetup.Location = new Point(0, 4);
      this.tabSetup.Margin = new Padding(0);
      this.tabSetup.Name = "tabSetup";
      this.tabSetup.Padding = new Point(11, 3);
      this.tabSetup.SelectedIndex = 0;
      this.tabSetup.Size = new Size(733, 362);
      this.tabSetup.TabIndex = 1;
      this.tabSetup.Selecting += new TabControlCancelEventHandler(this.tabSetup_Selecting);
      this.pageWebCenter.Location = new Point(4, 32);
      this.pageWebCenter.Name = "pageWebCenter";
      this.pageWebCenter.Padding = new Padding(0, 2, 2, 2);
      this.pageWebCenter.Size = new Size(725, 326);
      this.pageWebCenter.TabIndex = 1;
      this.pageWebCenter.Text = "Company Status Online Templates";
      this.pageWebCenter.UseVisualStyleBackColor = true;
      this.pageTPO.Location = new Point(4, 32);
      this.pageTPO.Name = "pageTPO";
      this.pageTPO.Padding = new Padding(0, 2, 2, 2);
      this.pageTPO.Size = new Size(725, 326);
      this.pageTPO.TabIndex = 4;
      this.pageTPO.Text = "TPO Status Online Templates";
      this.pageTPO.UseVisualStyleBackColor = true;
      this.pageEmail.Location = new Point(4, 32);
      this.pageEmail.Name = "pageEmail";
      this.pageEmail.Padding = new Padding(0, 2, 2, 2);
      this.pageEmail.Size = new Size(725, 326);
      this.pageEmail.TabIndex = 3;
      this.pageEmail.Text = "Email Templates";
      this.pageEmail.UseVisualStyleBackColor = true;
      this.pageUsers.Location = new Point(4, 32);
      this.pageUsers.Name = "pageUsers";
      this.pageUsers.Padding = new Padding(0, 2, 2, 2);
      this.pageUsers.Size = new Size(725, 326);
      this.pageUsers.TabIndex = 0;
      this.pageUsers.Text = "Users";
      this.pageUsers.UseVisualStyleBackColor = true;
      this.pnlBorder.Borders = AnchorStyles.Top;
      this.pnlBorder.Dock = DockStyle.Top;
      this.pnlBorder.Location = new Point(0, 0);
      this.pnlBorder.Name = "pnlBorder";
      this.pnlBorder.Size = new Size(733, 4);
      this.pnlBorder.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tabSetup);
      this.Controls.Add((Control) this.pnlBorder);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (StatusOnlineSetupPanel);
      this.Size = new Size(733, 366);
      this.tabSetup.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}

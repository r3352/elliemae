// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ContactsPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ContactsPage : Form, IPersonaSecurityPage
  {
    private IContainer components;
    private ContactsMgrPage contactPage;
    private DashboardMgrPage dashboardPage;
    private ReportsMgrPage reportsPage;
    private TradesMgrPage tradesPage;
    private ImageList imgListTv;
    private ContextMenu contextMenu1;
    private MenuItem menuItemLinkWithPersona;
    private MenuItem menuItemDisconnectFromPersona;
    private Splitter splitterLeftRight;
    private PanelEx pnlExDashboard;
    private PanelEx pnlExReports;
    private PanelEx pnlExLeft;
    private PanelEx pnlExTrades;
    private Splitter splitterLeft;
    private PanelEx pnlExRight;
    private Splitter splitterRight;
    private GradientPanel gradientPanel1;
    private Label label1;
    private Panel pnlDashboard;
    private Panel panelContactPage;
    private int currentPersonaID = -1;
    private Sessions.Session session;

    public event ContactsPage.OriginateLoanFeatureStatusChanged CreateLoanStatusChanged;

    public event ContactsPage.PersonaAccess HasPipelineLoanTabAccessEvent;

    public ContactsPage(Sessions.Session session, int personaId, EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.InitializeComponent();
      this.currentPersonaID = personaId;
      this.contactPage = new ContactsMgrPage(this.session, personaId, dirtyFlagChanged);
      this.setFormProperties((Form) this.contactPage, (Control) this.panelContactPage);
      this.contactPage.CreateLoanStatusChanged += new ContactsMgrPage.OriginateLoanFeatureStatusChanged(this.contactPage_CreateLoanStatusChanged);
      this.contactPage.HasPipelineLoanTabAccessEvent += new EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.PersonaAccess(this.contactPage_HasPipelineLoanTabAccessEvent);
      this.dashboardPage = new DashboardMgrPage(this.session, personaId, dirtyFlagChanged);
      this.setFormProperties((Form) this.dashboardPage, (Control) this.pnlDashboard);
      this.reportsPage = new ReportsMgrPage(this.session, personaId, dirtyFlagChanged);
      this.setFormProperties((Form) this.reportsPage, (Control) this.pnlExReports);
      if (this.session.EncompassEdition == EncompassEdition.Banker)
      {
        this.tradesPage = new TradesMgrPage(this.session, personaId, dirtyFlagChanged);
        this.setFormProperties((Form) this.tradesPage, (Control) this.pnlExTrades);
      }
      else
        this.hideTrades();
    }

    private bool contactPage_HasPipelineLoanTabAccessEvent()
    {
      return this.HasPipelineLoanTabAccessEvent != null && this.HasPipelineLoanTabAccessEvent();
    }

    private void contactPage_CreateLoanStatusChanged(AclTriState state)
    {
      if (this.CreateLoanStatusChanged == null)
        return;
      this.CreateLoanStatusChanged(state);
    }

    public ContactsPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.InitializeComponent();
      this.contactPage = new ContactsMgrPage(this.session, userId, personas, dirtyFlagChanged);
      this.setFormProperties((Form) this.contactPage, (Control) this.panelContactPage);
      this.contactPage.CreateLoanStatusChanged += new ContactsMgrPage.OriginateLoanFeatureStatusChanged(this.contactPage_CreateLoanStatusChanged);
      this.contactPage.HasPipelineLoanTabAccessEvent += new EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.PersonaAccess(this.contactPage_HasPipelineLoanTabAccessEvent);
      this.dashboardPage = new DashboardMgrPage(this.session, userId, personas, dirtyFlagChanged);
      this.setFormProperties((Form) this.dashboardPage, (Control) this.pnlDashboard);
      this.reportsPage = new ReportsMgrPage(this.session, userId, personas, dirtyFlagChanged);
      this.setFormProperties((Form) this.reportsPage, (Control) this.pnlExReports);
      if (this.session.EncompassEdition == EncompassEdition.Banker)
      {
        this.tradesPage = new TradesMgrPage(this.session, userId, personas, dirtyFlagChanged);
        this.setFormProperties((Form) this.tradesPage, (Control) this.pnlExTrades);
      }
      else
        this.hideTrades();
    }

    private void hideTrades()
    {
      if (this.session.EncompassEdition == EncompassEdition.Banker)
        return;
      this.pnlExTrades.Visible = this.splitterLeft.Visible = false;
    }

    private void setFormProperties(Form form, Control ownerControl)
    {
      form.TopLevel = false;
      form.Visible = true;
      form.Dock = DockStyle.Fill;
      ownerControl.Controls.Add((Control) form);
    }

    public virtual void SetPersona(int personaId)
    {
      if (this.currentPersonaID == personaId && !this.NeedToSaveData())
        return;
      this.currentPersonaID = personaId;
      this.contactPage.SetPersona(personaId);
      this.dashboardPage.SetPersona(personaId);
      this.reportsPage.SetPersona(personaId);
      if (this.session.EncompassEdition != EncompassEdition.Banker)
        return;
      this.tradesPage.SetPersona(personaId);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ContactsPage));
      this.imgListTv = new ImageList(this.components);
      this.contextMenu1 = new ContextMenu();
      this.menuItemLinkWithPersona = new MenuItem();
      this.menuItemDisconnectFromPersona = new MenuItem();
      this.panelContactPage = new Panel();
      this.splitterLeftRight = new Splitter();
      this.pnlExDashboard = new PanelEx();
      this.pnlDashboard = new Panel();
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.pnlExReports = new PanelEx();
      this.pnlExLeft = new PanelEx();
      this.splitterLeft = new Splitter();
      this.pnlExTrades = new PanelEx();
      this.pnlExRight = new PanelEx();
      this.splitterRight = new Splitter();
      this.pnlExDashboard.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.pnlExLeft.SuspendLayout();
      this.pnlExRight.SuspendLayout();
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
      this.panelContactPage.Dock = DockStyle.Fill;
      this.panelContactPage.Location = new Point(0, 278);
      this.panelContactPage.Name = "panelContactPage";
      this.panelContactPage.Size = new Size(200, 80);
      this.panelContactPage.TabIndex = 13;
      this.splitterLeftRight.Location = new Point(200, 0);
      this.splitterLeftRight.Name = "splitterLeftRight";
      this.splitterLeftRight.Size = new Size(3, 358);
      this.splitterLeftRight.TabIndex = 14;
      this.splitterLeftRight.TabStop = false;
      this.pnlExDashboard.Controls.Add((Control) this.pnlDashboard);
      this.pnlExDashboard.Controls.Add((Control) this.gradientPanel1);
      this.pnlExDashboard.Dock = DockStyle.Top;
      this.pnlExDashboard.Location = new Point(0, 0);
      this.pnlExDashboard.Name = "pnlExDashboard";
      this.pnlExDashboard.Size = new Size(371, 178);
      this.pnlExDashboard.TabIndex = 15;
      this.pnlDashboard.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlDashboard.Location = new Point(0, 0);
      this.pnlDashboard.Name = "pnlDashboard";
      this.pnlDashboard.Size = new Size(371, 154);
      this.pnlDashboard.TabIndex = 1;
      this.gradientPanel1.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Bottom;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 153);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(371, 25);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel1.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(5, 5);
      this.label1.Name = "label1";
      this.label1.Size = new Size(106, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "*Also affects Reports";
      this.pnlExReports.Dock = DockStyle.Fill;
      this.pnlExReports.Location = new Point(0, 181);
      this.pnlExReports.Name = "pnlExReports";
      this.pnlExReports.Size = new Size(371, 177);
      this.pnlExReports.TabIndex = 17;
      this.pnlExLeft.Controls.Add((Control) this.panelContactPage);
      this.pnlExLeft.Controls.Add((Control) this.splitterLeft);
      this.pnlExLeft.Controls.Add((Control) this.pnlExTrades);
      this.pnlExLeft.Dock = DockStyle.Left;
      this.pnlExLeft.Location = new Point(0, 0);
      this.pnlExLeft.Name = "pnlExLeft";
      this.pnlExLeft.Size = new Size(200, 358);
      this.pnlExLeft.TabIndex = 18;
      this.splitterLeft.Dock = DockStyle.Top;
      this.splitterLeft.Location = new Point(0, 275);
      this.splitterLeft.Name = "splitterLeft";
      this.splitterLeft.Size = new Size(200, 3);
      this.splitterLeft.TabIndex = 1;
      this.splitterLeft.TabStop = false;
      this.pnlExTrades.Dock = DockStyle.Top;
      this.pnlExTrades.Location = new Point(0, 0);
      this.pnlExTrades.Name = "pnlExTrades";
      this.pnlExTrades.Size = new Size(200, 275);
      this.pnlExTrades.TabIndex = 0;
      this.pnlExRight.Controls.Add((Control) this.pnlExReports);
      this.pnlExRight.Controls.Add((Control) this.splitterRight);
      this.pnlExRight.Controls.Add((Control) this.pnlExDashboard);
      this.pnlExRight.Dock = DockStyle.Fill;
      this.pnlExRight.Location = new Point(203, 0);
      this.pnlExRight.Name = "pnlExRight";
      this.pnlExRight.Size = new Size(371, 358);
      this.pnlExRight.TabIndex = 19;
      this.splitterRight.Dock = DockStyle.Top;
      this.splitterRight.Location = new Point(0, 178);
      this.splitterRight.Name = "splitterRight";
      this.splitterRight.Size = new Size(371, 3);
      this.splitterRight.TabIndex = 0;
      this.splitterRight.TabStop = false;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(574, 358);
      this.Controls.Add((Control) this.pnlExRight);
      this.Controls.Add((Control) this.splitterLeftRight);
      this.Controls.Add((Control) this.pnlExLeft);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (ContactsPage);
      this.Text = "BorSearchLoanInfoPage";
      this.BackColorChanged += new EventHandler(this.SettingsPage_BackColorChanged);
      this.SizeChanged += new EventHandler(this.ContactsPage_SizeChanged);
      this.pnlExDashboard.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.pnlExLeft.ResumeLayout(false);
      this.pnlExRight.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void SettingsPage_BackColorChanged(object sender, EventArgs e)
    {
      this.contactPage.BackColor = this.BackColor;
      this.dashboardPage.BackColor = this.BackColor;
      this.reportsPage.BackColor = this.BackColor;
      if (this.session.EncompassEdition != EncompassEdition.Banker)
        return;
      this.tradesPage.BackColor = this.BackColor;
    }

    public void SaveData()
    {
      this.contactPage.Save();
      this.dashboardPage.Save();
      this.reportsPage.Save();
      if (this.session.EncompassEdition != EncompassEdition.Banker)
        return;
      this.tradesPage.Save();
    }

    public bool HasOriginateLoanAccess() => this.contactPage.HasOriginateLoanAccess();

    public void ResetData()
    {
      this.contactPage.Reset();
      this.dashboardPage.Reset();
      this.reportsPage.Reset();
      if (this.session.EncompassEdition != EncompassEdition.Banker)
        return;
      this.tradesPage.Reset();
    }

    public bool NeedToSaveData()
    {
      return this.contactPage.NeedToSaveData() || this.dashboardPage.NeedToSaveData() || this.reportsPage.NeedToSaveData() || this.session.EncompassEdition == EncompassEdition.Banker && this.tradesPage.NeedToSaveData();
    }

    public void MakeReadOnly(bool makeReadOnly)
    {
      this.contactPage.MakeReadOnly(makeReadOnly);
      this.dashboardPage.MakeReadOnly(makeReadOnly);
      this.reportsPage.MakeReadOnly(makeReadOnly);
      if (this.session.EncompassEdition != EncompassEdition.Banker)
        return;
      this.tradesPage.MakeReadOnly(makeReadOnly);
    }

    private void ContactsPage_SizeChanged(object sender, EventArgs e)
    {
      this.pnlExLeft.Width = this.Width / 2;
      this.pnlExDashboard.Height = this.Height / 2;
    }

    public void DisallowLoanOrigination() => this.contactPage.DisallowLoanOrigination();

    public delegate void OriginateLoanFeatureStatusChanged(AclTriState state);

    public delegate bool PersonaAccess();
  }
}

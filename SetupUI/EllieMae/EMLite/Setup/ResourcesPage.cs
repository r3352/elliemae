// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ResourcesPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
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
  public class ResourcesPage : UserControl, IGroupSecurityPage
  {
    private Sessions.Session session;
    public EventHandler DirtyFlagChanged;
    private string userID = "";
    private AclGroup[] groupList;
    private IContainer components;
    private ResourceBasePanel standardPrintFormsPanel;
    private ResourceBasePanel customPrintFormsPanel;
    private ResourceBasePanel printGroupsPanel;
    private ResourceBasePanel bizContactsPanel;
    private ResourceBasePanel customLettersPanel;
    private ResourceBasePanel bizCustomLettersPanel;
    private ResourceBasePanel reportsPanel;
    private ResourceBasePanel campaignTemplatePanel;
    private ResourceBasePanel dashboardTemplatePanel;
    private ResourceBasePanel dashboardViewTemplatePanel;
    private ResourceBasePanel conditionalApprovalLetterPanel;
    private ResourceBasePanel changeOfCircumstancePanel;
    private LoanTemplateLegend legend;
    private PanelEx pnlExRow1;
    private Splitter splitter12;
    private PanelEx pnlExRow2;
    private Splitter splitter23;
    private PanelEx pnlExRow3;
    private Splitter splitter34;
    private PanelEx pnlExRow4;
    private Splitter splitter45;
    private PanelEx pnlExRow5;
    private PanelEx pnlExRow1Right;
    private Splitter splitter1;
    private PanelEx pnlExRow1Left;
    private PanelEx pnlExRow2Right;
    private Splitter splitter2;
    private PanelEx pnlExRow2Left;
    private PanelEx pnlExRow3Right;
    private Splitter splitter3;
    private PanelEx pnlExRow3Left;
    private PanelEx pnlExRow4Right;
    private Splitter splitter4;
    private PanelEx pnlExRow4Left;
    private PanelEx pnlExRow5Left;
    private Splitter splitter5;
    private Splitter splitter56;
    private PanelEx panelExRow6;
    private PanelEx pnlExRow6Right;
    private Splitter splitter6;
    private PanelEx pnlExRow6Left;
    private PanelEx pnlExRow5Right;
    private FeaturesAclManager aclManager;

    public ResourcesPage(Sessions.Session session, int groupId, EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.aclManager = (FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features);
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.InitializeComponent();
      this.standardPrintFormsPanel = new ResourceBasePanel(session, groupId, GroupResourceType.StandardPrintForms, this.DirtyFlagChanged);
      this.customPrintFormsPanel = new ResourceBasePanel(session, groupId, GroupResourceType.CustomPrintForms, this.DirtyFlagChanged);
      this.printGroupsPanel = new ResourceBasePanel(session, groupId, GroupResourceType.PrintGroups, this.DirtyFlagChanged);
      this.bizContactsPanel = new ResourceBasePanel(session, groupId, GroupResourceType.BizContacts, this.DirtyFlagChanged);
      this.customLettersPanel = new ResourceBasePanel(session, groupId, GroupResourceType.BorrowerCustomLetters, this.DirtyFlagChanged);
      this.bizCustomLettersPanel = new ResourceBasePanel(session, groupId, GroupResourceType.BusinessCustomLetters, this.DirtyFlagChanged);
      this.reportsPanel = new ResourceBasePanel(session, groupId, GroupResourceType.Reports, this.DirtyFlagChanged);
      this.campaignTemplatePanel = new ResourceBasePanel(session, groupId, GroupResourceType.CampaignTemplates, this.DirtyFlagChanged);
      this.dashboardTemplatePanel = new ResourceBasePanel(session, groupId, GroupResourceType.DashboardTemplates, this.DirtyFlagChanged);
      this.dashboardViewTemplatePanel = new ResourceBasePanel(session, groupId, GroupResourceType.DashboardViewTemplate, this.DirtyFlagChanged);
      this.conditionalApprovalLetterPanel = new ResourceBasePanel(session, groupId, GroupResourceType.ConditionalApprovalLetter, this.DirtyFlagChanged);
      this.changeOfCircumstancePanel = new ResourceBasePanel(session, groupId, GroupResourceType.ChangeOfCircumstanceOptions, this.DirtyFlagChanged);
      this.init();
    }

    public ResourcesPage(
      Sessions.Session session,
      string userID,
      AclGroup[] groups,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.InitializeComponent();
      this.userID = userID;
      this.groupList = groups;
      this.standardPrintFormsPanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.StandardPrintForms, this.DirtyFlagChanged);
      this.customPrintFormsPanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.CustomPrintForms, this.DirtyFlagChanged);
      this.printGroupsPanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.PrintGroups, this.DirtyFlagChanged);
      this.bizContactsPanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.BizContacts, this.DirtyFlagChanged);
      this.customLettersPanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.BorrowerCustomLetters, this.DirtyFlagChanged);
      this.bizCustomLettersPanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.BorrowerCustomLetters, this.DirtyFlagChanged);
      this.reportsPanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.Reports, this.DirtyFlagChanged);
      this.campaignTemplatePanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.CampaignTemplates, this.DirtyFlagChanged);
      this.dashboardTemplatePanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.DashboardTemplates, this.DirtyFlagChanged);
      this.dashboardViewTemplatePanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.DashboardViewTemplate, this.DirtyFlagChanged);
      this.conditionalApprovalLetterPanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.ConditionalApprovalLetter, this.DirtyFlagChanged);
      this.changeOfCircumstancePanel = new ResourceBasePanel(session, this.userID, this.groupList, GroupResourceType.ChangeOfCircumstanceOptions, this.DirtyFlagChanged);
      this.init();
    }

    public void init()
    {
      this.standardPrintFormsPanel.Visible = true;
      this.standardPrintFormsPanel.Dock = DockStyle.Fill;
      this.standardPrintFormsPanel.BackColor = this.BackColor;
      this.pnlExRow1Left.Controls.Add((Control) this.standardPrintFormsPanel);
      this.customPrintFormsPanel.Visible = true;
      this.customPrintFormsPanel.Dock = DockStyle.Fill;
      this.customPrintFormsPanel.BackColor = this.BackColor;
      this.pnlExRow1Right.Controls.Add((Control) this.customPrintFormsPanel);
      this.printGroupsPanel.Visible = true;
      this.printGroupsPanel.Dock = DockStyle.Fill;
      this.printGroupsPanel.BackColor = this.BackColor;
      this.pnlExRow2Left.Controls.Add((Control) this.printGroupsPanel);
      this.bizContactsPanel.Visible = true;
      this.bizContactsPanel.Dock = DockStyle.Fill;
      this.bizContactsPanel.BackColor = this.BackColor;
      this.pnlExRow2Right.Controls.Add((Control) this.bizContactsPanel);
      this.customLettersPanel.Visible = true;
      this.customLettersPanel.Dock = DockStyle.Fill;
      this.customLettersPanel.BackColor = this.BackColor;
      this.pnlExRow3Left.Controls.Add((Control) this.customLettersPanel);
      this.bizCustomLettersPanel.Visible = true;
      this.bizCustomLettersPanel.Dock = DockStyle.Fill;
      this.bizCustomLettersPanel.BackColor = this.BackColor;
      this.pnlExRow3Right.Controls.Add((Control) this.bizCustomLettersPanel);
      this.reportsPanel.Visible = true;
      this.reportsPanel.Dock = DockStyle.Fill;
      this.reportsPanel.BackColor = this.BackColor;
      this.pnlExRow4Left.Controls.Add((Control) this.reportsPanel);
      this.campaignTemplatePanel.Visible = true;
      this.campaignTemplatePanel.Dock = DockStyle.Fill;
      this.campaignTemplatePanel.BackColor = this.BackColor;
      this.pnlExRow4Right.Controls.Add((Control) this.campaignTemplatePanel);
      this.dashboardTemplatePanel.Visible = true;
      this.dashboardTemplatePanel.Dock = DockStyle.Fill;
      this.dashboardTemplatePanel.BackColor = this.BackColor;
      this.pnlExRow5Left.Controls.Add((Control) this.dashboardTemplatePanel);
      this.dashboardViewTemplatePanel.Visible = true;
      this.dashboardViewTemplatePanel.Dock = DockStyle.Fill;
      this.dashboardViewTemplatePanel.BackColor = this.BackColor;
      this.pnlExRow5Right.Controls.Add((Control) this.dashboardViewTemplatePanel);
      this.conditionalApprovalLetterPanel.Visible = true;
      this.conditionalApprovalLetterPanel.Dock = DockStyle.Fill;
      this.conditionalApprovalLetterPanel.BackColor = this.BackColor;
      this.pnlExRow6Left.Controls.Add((Control) this.conditionalApprovalLetterPanel);
      if (this.session.StartupInfo.EnableCoC)
      {
        this.changeOfCircumstancePanel.Visible = true;
        this.changeOfCircumstancePanel.Dock = DockStyle.Fill;
        this.changeOfCircumstancePanel.BackColor = this.BackColor;
      }
      else
      {
        this.changeOfCircumstancePanel.Visible = false;
        this.changeOfCircumstancePanel.Dock = DockStyle.None;
      }
      this.ResourcesPage_SizeChanged((object) this, (EventArgs) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.splitter12 = new Splitter();
      this.splitter23 = new Splitter();
      this.splitter34 = new Splitter();
      this.splitter45 = new Splitter();
      this.pnlExRow1 = new PanelEx();
      this.pnlExRow1Right = new PanelEx();
      this.splitter1 = new Splitter();
      this.pnlExRow1Left = new PanelEx();
      this.pnlExRow2 = new PanelEx();
      this.pnlExRow2Right = new PanelEx();
      this.splitter2 = new Splitter();
      this.pnlExRow2Left = new PanelEx();
      this.pnlExRow3 = new PanelEx();
      this.pnlExRow3Right = new PanelEx();
      this.splitter3 = new Splitter();
      this.pnlExRow3Left = new PanelEx();
      this.pnlExRow4 = new PanelEx();
      this.pnlExRow4Right = new PanelEx();
      this.splitter4 = new Splitter();
      this.pnlExRow4Left = new PanelEx();
      this.pnlExRow5 = new PanelEx();
      this.pnlExRow5Right = new PanelEx();
      this.splitter5 = new Splitter();
      this.pnlExRow5Left = new PanelEx();
      this.legend = new LoanTemplateLegend();
      this.splitter56 = new Splitter();
      this.panelExRow6 = new PanelEx();
      this.pnlExRow6Right = new PanelEx();
      this.splitter6 = new Splitter();
      this.pnlExRow6Left = new PanelEx();
      this.pnlExRow1.SuspendLayout();
      this.pnlExRow2.SuspendLayout();
      this.pnlExRow3.SuspendLayout();
      this.pnlExRow4.SuspendLayout();
      this.pnlExRow5.SuspendLayout();
      this.panelExRow6.SuspendLayout();
      this.SuspendLayout();
      this.splitter12.Dock = DockStyle.Top;
      this.splitter12.Location = new Point(0, 184);
      this.splitter12.Name = "splitter12";
      this.splitter12.Size = new Size(576, 3);
      this.splitter12.TabIndex = 13;
      this.splitter12.TabStop = false;
      this.splitter23.Dock = DockStyle.Top;
      this.splitter23.Location = new Point(0, 371);
      this.splitter23.Name = "splitter23";
      this.splitter23.Size = new Size(576, 3);
      this.splitter23.TabIndex = 12;
      this.splitter23.TabStop = false;
      this.splitter34.Dock = DockStyle.Top;
      this.splitter34.Location = new Point(0, 558);
      this.splitter34.Name = "splitter34";
      this.splitter34.Size = new Size(576, 3);
      this.splitter34.TabIndex = 11;
      this.splitter34.TabStop = false;
      this.splitter45.Dock = DockStyle.Top;
      this.splitter45.Location = new Point(0, 745);
      this.splitter45.Name = "splitter45";
      this.splitter45.Size = new Size(576, 3);
      this.splitter45.TabIndex = 10;
      this.splitter45.TabStop = false;
      this.pnlExRow1.Controls.Add((Control) this.pnlExRow1Right);
      this.pnlExRow1.Controls.Add((Control) this.splitter1);
      this.pnlExRow1.Controls.Add((Control) this.pnlExRow1Left);
      this.pnlExRow1.Dock = DockStyle.Top;
      this.pnlExRow1.Location = new Point(0, 0);
      this.pnlExRow1.Name = "pnlExRow1";
      this.pnlExRow1.Size = new Size(576, 184);
      this.pnlExRow1.TabIndex = 1;
      this.pnlExRow1Right.Dock = DockStyle.Fill;
      this.pnlExRow1Right.Location = new Point(307, 0);
      this.pnlExRow1Right.Name = "pnlExRow1Right";
      this.pnlExRow1Right.Size = new Size(269, 184);
      this.pnlExRow1Right.TabIndex = 2;
      this.splitter1.Location = new Point(304, 0);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(3, 184);
      this.splitter1.TabIndex = 1;
      this.splitter1.TabStop = false;
      this.pnlExRow1Left.Dock = DockStyle.Left;
      this.pnlExRow1Left.Location = new Point(0, 0);
      this.pnlExRow1Left.Name = "pnlExRow1Left";
      this.pnlExRow1Left.Size = new Size(304, 184);
      this.pnlExRow1Left.TabIndex = 0;
      this.pnlExRow2.Controls.Add((Control) this.pnlExRow2Right);
      this.pnlExRow2.Controls.Add((Control) this.splitter2);
      this.pnlExRow2.Controls.Add((Control) this.pnlExRow2Left);
      this.pnlExRow2.Dock = DockStyle.Top;
      this.pnlExRow2.Location = new Point(0, 187);
      this.pnlExRow2.Name = "pnlExRow2";
      this.pnlExRow2.Size = new Size(576, 184);
      this.pnlExRow2.TabIndex = 3;
      this.pnlExRow2Right.Dock = DockStyle.Fill;
      this.pnlExRow2Right.Location = new Point(307, 0);
      this.pnlExRow2Right.Name = "pnlExRow2Right";
      this.pnlExRow2Right.Size = new Size(269, 184);
      this.pnlExRow2Right.TabIndex = 2;
      this.splitter2.Location = new Point(304, 0);
      this.splitter2.Name = "splitter2";
      this.splitter2.Size = new Size(3, 184);
      this.splitter2.TabIndex = 1;
      this.splitter2.TabStop = false;
      this.pnlExRow2Left.Dock = DockStyle.Left;
      this.pnlExRow2Left.Location = new Point(0, 0);
      this.pnlExRow2Left.Name = "pnlExRow2Left";
      this.pnlExRow2Left.Size = new Size(304, 184);
      this.pnlExRow2Left.TabIndex = 0;
      this.pnlExRow3.Controls.Add((Control) this.pnlExRow3Right);
      this.pnlExRow3.Controls.Add((Control) this.splitter3);
      this.pnlExRow3.Controls.Add((Control) this.pnlExRow3Left);
      this.pnlExRow3.Dock = DockStyle.Top;
      this.pnlExRow3.Location = new Point(0, 374);
      this.pnlExRow3.Name = "pnlExRow3";
      this.pnlExRow3.Size = new Size(576, 184);
      this.pnlExRow3.TabIndex = 5;
      this.pnlExRow3Right.Dock = DockStyle.Fill;
      this.pnlExRow3Right.Location = new Point(307, 0);
      this.pnlExRow3Right.Name = "pnlExRow3Right";
      this.pnlExRow3Right.Size = new Size(269, 184);
      this.pnlExRow3Right.TabIndex = 2;
      this.splitter3.Location = new Point(304, 0);
      this.splitter3.Name = "splitter3";
      this.splitter3.Size = new Size(3, 184);
      this.splitter3.TabIndex = 1;
      this.splitter3.TabStop = false;
      this.pnlExRow3Left.Dock = DockStyle.Left;
      this.pnlExRow3Left.Location = new Point(0, 0);
      this.pnlExRow3Left.Name = "pnlExRow3Left";
      this.pnlExRow3Left.Size = new Size(304, 184);
      this.pnlExRow3Left.TabIndex = 0;
      this.pnlExRow4.Controls.Add((Control) this.pnlExRow4Right);
      this.pnlExRow4.Controls.Add((Control) this.splitter4);
      this.pnlExRow4.Controls.Add((Control) this.pnlExRow4Left);
      this.pnlExRow4.Dock = DockStyle.Top;
      this.pnlExRow4.Location = new Point(0, 561);
      this.pnlExRow4.Name = "pnlExRow4";
      this.pnlExRow4.Size = new Size(576, 184);
      this.pnlExRow4.TabIndex = 7;
      this.pnlExRow4Right.Dock = DockStyle.Fill;
      this.pnlExRow4Right.Location = new Point(307, 0);
      this.pnlExRow4Right.Name = "pnlExRow4Right";
      this.pnlExRow4Right.Size = new Size(269, 184);
      this.pnlExRow4Right.TabIndex = 3;
      this.splitter4.Location = new Point(304, 0);
      this.splitter4.Name = "splitter4";
      this.splitter4.Size = new Size(3, 184);
      this.splitter4.TabIndex = 2;
      this.splitter4.TabStop = false;
      this.pnlExRow4Left.Dock = DockStyle.Left;
      this.pnlExRow4Left.Location = new Point(0, 0);
      this.pnlExRow4Left.Name = "pnlExRow4Left";
      this.pnlExRow4Left.Size = new Size(304, 184);
      this.pnlExRow4Left.TabIndex = 1;
      this.pnlExRow5.Controls.Add((Control) this.pnlExRow5Right);
      this.pnlExRow5.Controls.Add((Control) this.splitter5);
      this.pnlExRow5.Controls.Add((Control) this.pnlExRow5Left);
      this.pnlExRow5.Dock = DockStyle.Top;
      this.pnlExRow5.Location = new Point(0, 748);
      this.pnlExRow5.Name = "pnlExRow5";
      this.pnlExRow5.Size = new Size(576, 187);
      this.pnlExRow5.TabIndex = 9;
      this.pnlExRow5Right.Dock = DockStyle.Fill;
      this.pnlExRow5Right.Location = new Point(307, 0);
      this.pnlExRow5Right.Name = "pnlExRow5Right";
      this.pnlExRow5Right.Size = new Size(269, 187);
      this.pnlExRow5Right.TabIndex = 2;
      this.splitter5.Location = new Point(304, 0);
      this.splitter5.Name = "splitter5";
      this.splitter5.Size = new Size(3, 187);
      this.splitter5.TabIndex = 1;
      this.splitter5.TabStop = false;
      this.pnlExRow5Left.Dock = DockStyle.Left;
      this.pnlExRow5Left.Location = new Point(0, 0);
      this.pnlExRow5Left.Name = "pnlExRow5Left";
      this.pnlExRow5Left.Size = new Size(304, 187);
      this.pnlExRow5Left.TabIndex = 0;
      this.legend.BackColor = Color.Transparent;
      this.legend.Dock = DockStyle.Bottom;
      this.legend.Location = new Point(0, 1134);
      this.legend.Name = "legend";
      this.legend.Size = new Size(576, 49);
      this.legend.TabIndex = 0;
      this.splitter56.Dock = DockStyle.Top;
      this.splitter56.Location = new Point(0, 935);
      this.splitter56.Name = "splitter56";
      this.splitter56.Size = new Size(576, 3);
      this.splitter56.TabIndex = 14;
      this.splitter56.TabStop = false;
      this.panelExRow6.Controls.Add((Control) this.pnlExRow6Right);
      this.panelExRow6.Controls.Add((Control) this.splitter6);
      this.panelExRow6.Controls.Add((Control) this.pnlExRow6Left);
      this.panelExRow6.Dock = DockStyle.Fill;
      this.panelExRow6.Location = new Point(0, 938);
      this.panelExRow6.Name = "panelExRow6";
      this.panelExRow6.Size = new Size(576, 196);
      this.panelExRow6.TabIndex = 15;
      this.pnlExRow6Right.Dock = DockStyle.Fill;
      this.pnlExRow6Right.Location = new Point(307, 0);
      this.pnlExRow6Right.Name = "pnlExRow6Right";
      this.pnlExRow6Right.Size = new Size(269, 196);
      this.pnlExRow6Right.TabIndex = 2;
      this.splitter6.Location = new Point(304, 0);
      this.splitter6.Name = "splitter6";
      this.splitter6.Size = new Size(3, 196);
      this.splitter6.TabIndex = 1;
      this.splitter6.TabStop = false;
      this.pnlExRow6Left.Dock = DockStyle.Left;
      this.pnlExRow6Left.Location = new Point(0, 0);
      this.pnlExRow6Left.Name = "pnlExRow6Left";
      this.pnlExRow6Left.Size = new Size(304, 196);
      this.pnlExRow6Left.TabIndex = 0;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.panelExRow6);
      this.Controls.Add((Control) this.splitter56);
      this.Controls.Add((Control) this.pnlExRow5);
      this.Controls.Add((Control) this.splitter45);
      this.Controls.Add((Control) this.pnlExRow4);
      this.Controls.Add((Control) this.splitter34);
      this.Controls.Add((Control) this.pnlExRow3);
      this.Controls.Add((Control) this.splitter23);
      this.Controls.Add((Control) this.pnlExRow2);
      this.Controls.Add((Control) this.splitter12);
      this.Controls.Add((Control) this.pnlExRow1);
      this.Controls.Add((Control) this.legend);
      this.Name = nameof (ResourcesPage);
      this.Size = new Size(576, 1183);
      this.BackColorChanged += new EventHandler(this.ResourcesPage_BackColorChanged);
      this.SizeChanged += new EventHandler(this.ResourcesPage_SizeChanged);
      this.pnlExRow1.ResumeLayout(false);
      this.pnlExRow2.ResumeLayout(false);
      this.pnlExRow3.ResumeLayout(false);
      this.pnlExRow4.ResumeLayout(false);
      this.pnlExRow5.ResumeLayout(false);
      this.panelExRow6.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public virtual void SetGroup(int groupId)
    {
      this.standardPrintFormsPanel.SetGroup(groupId);
      this.customLettersPanel.SetGroup(groupId);
      this.bizCustomLettersPanel.SetGroup(groupId);
      this.customPrintFormsPanel.SetGroup(groupId);
      this.printGroupsPanel.SetGroup(groupId);
      this.reportsPanel.SetGroup(groupId);
      this.bizContactsPanel.SetGroup(groupId);
      this.campaignTemplatePanel.SetGroup(groupId);
      this.dashboardTemplatePanel.SetGroup(groupId);
      this.dashboardViewTemplatePanel.SetGroup(groupId);
      this.conditionalApprovalLetterPanel.SetGroup(groupId);
      this.changeOfCircumstancePanel.SetGroup(groupId);
    }

    private void ResourcesPage_BackColorChanged(object sender, EventArgs e)
    {
      this.standardPrintFormsPanel.BackColor = this.BackColor;
      this.customLettersPanel.BackColor = this.BackColor;
      this.bizCustomLettersPanel.BackColor = this.BackColor;
      this.customPrintFormsPanel.BackColor = this.BackColor;
      this.printGroupsPanel.BackColor = this.BackColor;
      this.reportsPanel.BackColor = this.BackColor;
      this.bizContactsPanel.BackColor = this.BackColor;
      this.campaignTemplatePanel.BackColor = this.BackColor;
      this.dashboardTemplatePanel.BackColor = this.BackColor;
      this.dashboardViewTemplatePanel.BackColor = this.BackColor;
      this.conditionalApprovalLetterPanel.BackColor = this.BackColor;
      this.changeOfCircumstancePanel.BackColor = this.BackColor;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.standardPrintFormsPanel.SaveData();
      this.bizContactsPanel.SaveData();
      this.customLettersPanel.SaveData();
      this.bizCustomLettersPanel.SaveData();
      this.customPrintFormsPanel.SaveData();
      this.printGroupsPanel.SaveData();
      this.reportsPanel.SaveData();
      this.campaignTemplatePanel.SaveData();
      this.dashboardTemplatePanel.SaveData();
      this.dashboardViewTemplatePanel.SaveData();
      this.conditionalApprovalLetterPanel.SaveData();
      this.changeOfCircumstancePanel.SaveData();
    }

    public void SaveData()
    {
      this.standardPrintFormsPanel.SaveData();
      this.bizContactsPanel.SaveData();
      this.customLettersPanel.SaveData();
      this.bizCustomLettersPanel.SaveData();
      this.customPrintFormsPanel.SaveData();
      this.printGroupsPanel.SaveData();
      this.reportsPanel.SaveData();
      this.campaignTemplatePanel.SaveData();
      this.dashboardTemplatePanel.SaveData();
      this.dashboardViewTemplatePanel.SaveData();
      this.conditionalApprovalLetterPanel.SaveData();
      this.changeOfCircumstancePanel.SaveData();
    }

    public void ResetData()
    {
      this.standardPrintFormsPanel.ResetDate();
      this.bizContactsPanel.ResetDate();
      this.customLettersPanel.ResetDate();
      this.bizCustomLettersPanel.ResetDate();
      this.customPrintFormsPanel.ResetDate();
      this.printGroupsPanel.ResetDate();
      this.reportsPanel.ResetDate();
      this.campaignTemplatePanel.ResetDate();
      this.dashboardTemplatePanel.ResetDate();
      this.dashboardViewTemplatePanel.ResetDate();
      this.conditionalApprovalLetterPanel.ResetDate();
      this.changeOfCircumstancePanel.ResetDate();
    }

    public bool HasBeenModified()
    {
      bool flag = false;
      if (this.standardPrintFormsPanel.HasBeenModified())
        flag = true;
      else if (this.bizContactsPanel.HasBeenModified())
        flag = true;
      else if (this.customLettersPanel.HasBeenModified())
        flag = true;
      else if (this.bizCustomLettersPanel.HasBeenModified())
        flag = true;
      else if (this.customPrintFormsPanel.HasBeenModified())
        flag = true;
      else if (this.printGroupsPanel.HasBeenModified())
        flag = true;
      else if (this.reportsPanel.HasBeenModified())
        flag = true;
      else if (this.campaignTemplatePanel.HasBeenModified())
        flag = true;
      else if (this.dashboardTemplatePanel.HasBeenModified())
        flag = true;
      else if (this.dashboardViewTemplatePanel.HasBeenModified())
        flag = true;
      else if (this.conditionalApprovalLetterPanel.HasBeenModified())
        flag = true;
      else if (this.changeOfCircumstancePanel.HasBeenModified())
        flag = true;
      return flag;
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      this.standardPrintFormsPanel.ResetDate();
      this.bizContactsPanel.ResetDate();
      this.customLettersPanel.ResetDate();
      this.bizCustomLettersPanel.ResetDate();
      this.customPrintFormsPanel.ResetDate();
      this.printGroupsPanel.ResetDate();
      this.reportsPanel.ResetDate();
      this.campaignTemplatePanel.ResetDate();
      this.dashboardTemplatePanel.ResetDate();
      this.dashboardViewTemplatePanel.ResetDate();
      this.conditionalApprovalLetterPanel.ResetDate();
      this.changeOfCircumstancePanel.ResetDate();
    }

    private void ResourcesPage_SizeChanged(object sender, EventArgs e)
    {
      this.pnlExRow1Left.Width = this.pnlExRow2Left.Width = this.pnlExRow3Left.Width = this.pnlExRow4Left.Width = this.pnlExRow5Left.Width = this.pnlExRow6Left.Width = this.Width / 2;
    }
  }
}

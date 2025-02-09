// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoansPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoansPage : Form, IPersonaSecurityPage
  {
    private LoansPrintPage printPage;
    private ItemizationFeeManagementPage itemizationPage;
    private ClosingPage closingPage;
    private EMDocsPage docsPage;
    private ContextMenu contextMenu1;
    private MenuItem menuItemLinkWithPersona;
    private MenuItem menuItemDisconnectFromPersona;
    private ImageList imgListTv;
    private IContainer components;
    private Label lblReturnToLastMilestoneSelect;
    private CheckBox chkBoxReturnToLastMilestone;
    private Label lblCommentsSelect;
    private CheckBox chkBoxComments;
    private Label lblSetLoanAssociateSelect;
    private CheckBox chkBoxSetLoanAssociate;
    private Label lblChangeDateSelect;
    private CheckBox chkBoxChangeDate;
    private Label lblFinsihMilestoneSelect;
    private CheckBox chkBoxFinishMilestone;
    private bool bIsUserSetup;
    private int personaId = -1;
    private string userId;
    private Label lblAcceptFilesSelect;
    private CheckBox chkBoxAcceptFiles;
    private Persona[] personas;
    private bool initialSetup = true;
    private MilestoneSelectionDialog accFileMSD;
    private MilestoneSelectionDialog returnFileMSD;
    private MilestoneSelectionDialog ceDateMSD;
    private MilestoneSelectionDialog finishMSMSD;
    private MilestoneSelectionDialog assLTMMSD;
    private GroupContainer gcMilestoneWorkflow;
    private PanelEx pnlExLeft;
    private Splitter splitter1;
    private Splitter splitter2;
    private PanelEx pnlExRight;
    private PanelEx pnlExPrint;
    private PanelEx pnlExClosing;
    private Panel pnlLoanPrint;
    private GradientPanel gradientPanel1;
    private Label label1;
    private MilestoneSelectionDialog emCommentMSD;
    private Panel pnlMilestoneMgr;
    private Label lblMilestoneMgr;
    private Panel pnlBase;
    private PipelineConfiguration pipelineConfiguration;
    private PanelEx pnlExDocs;
    private Splitter spltDocs;
    private Panel pnlFeeItms;
    private Sessions.Session session;

    public event EventHandler DirtyFlagChanged;

    public LoansPage(
      Sessions.Session session,
      int personaId,
      PipelineConfiguration pipelineConfiguration,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.pipelineConfiguration = pipelineConfiguration;
      this.pipelineConfiguration.FeatureStateChanged += new PipelineConfiguration.PipelineConfigChanged(this.pipelineConfiguration_FeatureStateChanged);
      this.init(false, (string) null, (Persona[]) null, personaId, dirtyFlagChanged);
    }

    public LoansPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      PipelineConfiguration pipelineConfiguration,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.pipelineConfiguration = pipelineConfiguration;
      this.pipelineConfiguration.FeatureStateChanged += new PipelineConfiguration.PipelineConfigChanged(this.pipelineConfiguration_FeatureStateChanged);
      this.init(true, userId, personas, -1, dirtyFlagChanged);
    }

    private void pipelineConfiguration_FeatureStateChanged(
      AclFeature feature,
      AclTriState state,
      bool gotoContact)
    {
      if (feature != AclFeature.GlobalTab_Pipeline)
        return;
      if (state == AclTriState.True)
      {
        this.pnlBase.Visible = true;
        this.lblMilestoneMgr.Visible = false;
      }
      else
      {
        this.pnlBase.Visible = false;
        this.lblMilestoneMgr.Visible = true;
      }
    }

    private void init(
      bool isUserSetup,
      string userId,
      Persona[] personas,
      int personaId,
      EventHandler dirtyFlagChanged)
    {
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.InitializeComponent();
      if (isUserSetup)
      {
        this.bIsUserSetup = isUserSetup;
        this.userId = userId;
        this.personas = personas;
        this.printPage = new LoansPrintPage(this.session, userId, personas, dirtyFlagChanged, this.pipelineConfiguration);
        this.itemizationPage = new ItemizationFeeManagementPage(this.session, userId, personas, this.pipelineConfiguration, dirtyFlagChanged);
        this.closingPage = new ClosingPage(this.session, userId, personas, this.pipelineConfiguration, dirtyFlagChanged);
        this.docsPage = new EMDocsPage(this.session, userId, personas, this.pipelineConfiguration, dirtyFlagChanged);
      }
      else
      {
        this.personaId = personaId;
        this.printPage = new LoansPrintPage(this.session, personaId, dirtyFlagChanged, this.pipelineConfiguration);
        this.itemizationPage = new ItemizationFeeManagementPage(this.session, personaId, this.pipelineConfiguration, dirtyFlagChanged);
        this.closingPage = new ClosingPage(this.session, personaId, this.pipelineConfiguration, dirtyFlagChanged);
        this.docsPage = new EMDocsPage(this.session, personaId, this.pipelineConfiguration, dirtyFlagChanged);
      }
      this.printPage.TopLevel = false;
      this.printPage.Visible = true;
      this.printPage.Dock = DockStyle.Fill;
      this.pnlLoanPrint.Controls.Add((Control) this.printPage);
      this.itemizationPage.TopLevel = false;
      this.itemizationPage.Visible = true;
      this.itemizationPage.Dock = DockStyle.Fill;
      this.pnlFeeItms.Controls.Add((Control) this.itemizationPage);
      this.closingPage.TopLevel = false;
      this.closingPage.Visible = true;
      this.closingPage.Dock = DockStyle.Fill;
      this.pnlExClosing.Controls.Add((Control) this.closingPage);
      this.docsPage.TopLevel = false;
      this.docsPage.Visible = true;
      this.docsPage.Dock = DockStyle.Fill;
      this.pnlExDocs.Controls.Add((Control) this.docsPage);
      this.initCheckBoxes();
      this.initialSetup = false;
      this.printPage.BackColor = EncompassColors.Neutral3;
      this.closingPage.BackColor = EncompassColors.Neutral3;
      this.docsPage.BackColor = EncompassColors.Neutral3;
      this.gcMilestoneWorkflow.BackColor = EncompassColors.Neutral3;
      this.pipelineConfiguration_FeatureStateChanged(AclFeature.GlobalTab_Pipeline, this.pipelineConfiguration.HasPipelineLoanTabAccess() ? AclTriState.True : AclTriState.False, false);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoansPage));
      this.contextMenu1 = new ContextMenu();
      this.menuItemLinkWithPersona = new MenuItem();
      this.menuItemDisconnectFromPersona = new MenuItem();
      this.imgListTv = new ImageList(this.components);
      this.splitter2 = new Splitter();
      this.pnlExRight = new PanelEx();
      this.pnlExClosing = new PanelEx();
      this.spltDocs = new Splitter();
      this.pnlExDocs = new PanelEx();
      this.pnlExLeft = new PanelEx();
      this.gcMilestoneWorkflow = new GroupContainer();
      this.pnlMilestoneMgr = new Panel();
      this.lblSetLoanAssociateSelect = new Label();
      this.lblChangeDateSelect = new Label();
      this.lblCommentsSelect = new Label();
      this.lblAcceptFilesSelect = new Label();
      this.chkBoxReturnToLastMilestone = new CheckBox();
      this.chkBoxChangeDate = new CheckBox();
      this.chkBoxComments = new CheckBox();
      this.chkBoxSetLoanAssociate = new CheckBox();
      this.lblReturnToLastMilestoneSelect = new Label();
      this.lblFinsihMilestoneSelect = new Label();
      this.chkBoxAcceptFiles = new CheckBox();
      this.chkBoxFinishMilestone = new CheckBox();
      this.splitter1 = new Splitter();
      this.pnlExPrint = new PanelEx();
      this.pnlLoanPrint = new Panel();
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.lblMilestoneMgr = new Label();
      this.pnlBase = new Panel();
      this.pnlFeeItms = new Panel();
      this.pnlExRight.SuspendLayout();
      this.pnlExLeft.SuspendLayout();
      this.gcMilestoneWorkflow.SuspendLayout();
      this.pnlMilestoneMgr.SuspendLayout();
      this.pnlExPrint.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.pnlBase.SuspendLayout();
      this.SuspendLayout();
      this.contextMenu1.MenuItems.AddRange(new MenuItem[2]
      {
        this.menuItemLinkWithPersona,
        this.menuItemDisconnectFromPersona
      });
      this.menuItemLinkWithPersona.Index = 0;
      this.menuItemLinkWithPersona.Text = "Link with Persona Rights";
      this.menuItemDisconnectFromPersona.Index = 1;
      this.menuItemDisconnectFromPersona.Text = "Disconnect from Persona Rights";
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "");
      this.imgListTv.Images.SetKeyName(1, "");
      this.splitter2.Location = new Point(242, 0);
      this.splitter2.Name = "splitter2";
      this.splitter2.Size = new Size(3, 437);
      this.splitter2.TabIndex = 17;
      this.splitter2.TabStop = false;
      this.pnlExRight.Controls.Add((Control) this.pnlExClosing);
      this.pnlExRight.Controls.Add((Control) this.spltDocs);
      this.pnlExRight.Controls.Add((Control) this.pnlExDocs);
      this.pnlExRight.Dock = DockStyle.Fill;
      this.pnlExRight.Location = new Point(245, 0);
      this.pnlExRight.Name = "pnlExRight";
      this.pnlExRight.Size = new Size(242, 437);
      this.pnlExRight.TabIndex = 18;
      this.pnlExClosing.Dock = DockStyle.Fill;
      this.pnlExClosing.Location = new Point(0, 0);
      this.pnlExClosing.Name = "pnlExClosing";
      this.pnlExClosing.Size = new Size(242, 243);
      this.pnlExClosing.TabIndex = 2;
      this.spltDocs.Dock = DockStyle.Bottom;
      this.spltDocs.Location = new Point(0, 243);
      this.spltDocs.Name = "spltDocs";
      this.spltDocs.Size = new Size(242, 3);
      this.spltDocs.TabIndex = 4;
      this.spltDocs.TabStop = false;
      this.pnlExDocs.Dock = DockStyle.Bottom;
      this.pnlExDocs.Location = new Point(0, 246);
      this.pnlExDocs.Name = "pnlExDocs";
      this.pnlExDocs.Size = new Size(242, 191);
      this.pnlExDocs.TabIndex = 3;
      this.pnlExLeft.Controls.Add((Control) this.pnlFeeItms);
      this.pnlExLeft.Controls.Add((Control) this.gcMilestoneWorkflow);
      this.pnlExLeft.Controls.Add((Control) this.splitter1);
      this.pnlExLeft.Controls.Add((Control) this.pnlExPrint);
      this.pnlExLeft.Dock = DockStyle.Left;
      this.pnlExLeft.Location = new Point(0, 0);
      this.pnlExLeft.Name = "pnlExLeft";
      this.pnlExLeft.Size = new Size(242, 437);
      this.pnlExLeft.TabIndex = 16;
      this.gcMilestoneWorkflow.Controls.Add((Control) this.pnlMilestoneMgr);
      this.gcMilestoneWorkflow.Dock = DockStyle.Top;
      this.gcMilestoneWorkflow.HeaderForeColor = SystemColors.ControlText;
      this.gcMilestoneWorkflow.Location = new Point(0, 0);
      this.gcMilestoneWorkflow.Name = "gcMilestoneWorkflow";
      this.gcMilestoneWorkflow.Size = new Size(242, 146);
      this.gcMilestoneWorkflow.TabIndex = 0;
      this.gcMilestoneWorkflow.Text = "Milestone/Workflow Management";
      this.pnlMilestoneMgr.Controls.Add((Control) this.lblSetLoanAssociateSelect);
      this.pnlMilestoneMgr.Controls.Add((Control) this.lblChangeDateSelect);
      this.pnlMilestoneMgr.Controls.Add((Control) this.lblCommentsSelect);
      this.pnlMilestoneMgr.Controls.Add((Control) this.lblAcceptFilesSelect);
      this.pnlMilestoneMgr.Controls.Add((Control) this.chkBoxReturnToLastMilestone);
      this.pnlMilestoneMgr.Controls.Add((Control) this.chkBoxChangeDate);
      this.pnlMilestoneMgr.Controls.Add((Control) this.chkBoxComments);
      this.pnlMilestoneMgr.Controls.Add((Control) this.chkBoxSetLoanAssociate);
      this.pnlMilestoneMgr.Controls.Add((Control) this.lblReturnToLastMilestoneSelect);
      this.pnlMilestoneMgr.Controls.Add((Control) this.lblFinsihMilestoneSelect);
      this.pnlMilestoneMgr.Controls.Add((Control) this.chkBoxAcceptFiles);
      this.pnlMilestoneMgr.Controls.Add((Control) this.chkBoxFinishMilestone);
      this.pnlMilestoneMgr.Dock = DockStyle.Fill;
      this.pnlMilestoneMgr.Location = new Point(1, 26);
      this.pnlMilestoneMgr.Name = "pnlMilestoneMgr";
      this.pnlMilestoneMgr.Size = new Size(240, 119);
      this.pnlMilestoneMgr.TabIndex = 23;
      this.lblSetLoanAssociateSelect.ForeColor = SystemColors.Highlight;
      this.lblSetLoanAssociateSelect.Location = new Point(29, 74);
      this.lblSetLoanAssociateSelect.Name = "lblSetLoanAssociateSelect";
      this.lblSetLoanAssociateSelect.Size = new Size(176, 20);
      this.lblSetLoanAssociateSelect.TabIndex = 16;
      this.lblSetLoanAssociateSelect.Text = "Assign Loan Team Members";
      this.lblSetLoanAssociateSelect.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSetLoanAssociateSelect.Click += new EventHandler(this.lblSetLoanAssociateSelect_Click);
      this.lblChangeDateSelect.ForeColor = SystemColors.Highlight;
      this.lblChangeDateSelect.Location = new Point(28, 40);
      this.lblChangeDateSelect.Name = "lblChangeDateSelect";
      this.lblChangeDateSelect.Size = new Size(180, 20);
      this.lblChangeDateSelect.TabIndex = 14;
      this.lblChangeDateSelect.Text = "Change Expected Dates";
      this.lblChangeDateSelect.TextAlign = ContentAlignment.MiddleLeft;
      this.lblChangeDateSelect.Click += new EventHandler(this.lblChangeDateSelect_Click);
      this.lblCommentsSelect.ForeColor = SystemColors.Highlight;
      this.lblCommentsSelect.Location = new Point(28, 91);
      this.lblCommentsSelect.Name = "lblCommentsSelect";
      this.lblCommentsSelect.Size = new Size(180, 20);
      this.lblCommentsSelect.TabIndex = 20;
      this.lblCommentsSelect.Text = "Edit Milestone Comments";
      this.lblCommentsSelect.TextAlign = ContentAlignment.MiddleLeft;
      this.lblCommentsSelect.Click += new EventHandler(this.lblCommentsSelect_Click);
      this.lblAcceptFilesSelect.ForeColor = SystemColors.Highlight;
      this.lblAcceptFilesSelect.Location = new Point(28, 6);
      this.lblAcceptFilesSelect.Name = "lblAcceptFilesSelect";
      this.lblAcceptFilesSelect.Size = new Size(180, 20);
      this.lblAcceptFilesSelect.TabIndex = 18;
      this.lblAcceptFilesSelect.Text = "Accept Files";
      this.lblAcceptFilesSelect.TextAlign = ContentAlignment.MiddleLeft;
      this.lblAcceptFilesSelect.Click += new EventHandler(this.lblAcceptFilesSelect_Click);
      this.chkBoxReturnToLastMilestone.Location = new Point(8, 23);
      this.chkBoxReturnToLastMilestone.Name = "chkBoxReturnToLastMilestone";
      this.chkBoxReturnToLastMilestone.Size = new Size(20, 20);
      this.chkBoxReturnToLastMilestone.TabIndex = 21;
      this.chkBoxReturnToLastMilestone.CheckedChanged += new EventHandler(this.chkBoxReturnToLastMilestone_CheckedChanged);
      this.chkBoxChangeDate.Location = new Point(8, 40);
      this.chkBoxChangeDate.Name = "chkBoxChangeDate";
      this.chkBoxChangeDate.Size = new Size(20, 20);
      this.chkBoxChangeDate.TabIndex = 13;
      this.chkBoxChangeDate.CheckedChanged += new EventHandler(this.chkBoxChangeDate_CheckedChanged);
      this.chkBoxComments.Location = new Point(8, 91);
      this.chkBoxComments.Name = "chkBoxComments";
      this.chkBoxComments.Size = new Size(20, 20);
      this.chkBoxComments.TabIndex = 19;
      this.chkBoxComments.CheckedChanged += new EventHandler(this.chkBoxComments_CheckedChanged);
      this.chkBoxSetLoanAssociate.Location = new Point(8, 74);
      this.chkBoxSetLoanAssociate.Name = "chkBoxSetLoanAssociate";
      this.chkBoxSetLoanAssociate.Size = new Size(24, 20);
      this.chkBoxSetLoanAssociate.TabIndex = 15;
      this.chkBoxSetLoanAssociate.CheckedChanged += new EventHandler(this.chkBoxSetLoanAssociate_CheckedChanged);
      this.lblReturnToLastMilestoneSelect.ForeColor = SystemColors.Highlight;
      this.lblReturnToLastMilestoneSelect.Location = new Point(28, 23);
      this.lblReturnToLastMilestoneSelect.Name = "lblReturnToLastMilestoneSelect";
      this.lblReturnToLastMilestoneSelect.Size = new Size(180, 20);
      this.lblReturnToLastMilestoneSelect.TabIndex = 22;
      this.lblReturnToLastMilestoneSelect.Text = "Return Files";
      this.lblReturnToLastMilestoneSelect.TextAlign = ContentAlignment.MiddleLeft;
      this.lblReturnToLastMilestoneSelect.Click += new EventHandler(this.lblReturnToLastMilestoneSelect_Click);
      this.lblFinsihMilestoneSelect.ForeColor = SystemColors.Highlight;
      this.lblFinsihMilestoneSelect.Location = new Point(29, 57);
      this.lblFinsihMilestoneSelect.Name = "lblFinsihMilestoneSelect";
      this.lblFinsihMilestoneSelect.Size = new Size(176, 20);
      this.lblFinsihMilestoneSelect.TabIndex = 12;
      this.lblFinsihMilestoneSelect.Text = "Finish Milestones";
      this.lblFinsihMilestoneSelect.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFinsihMilestoneSelect.Click += new EventHandler(this.lblFinsihMilestoneSelect_Click);
      this.chkBoxAcceptFiles.Location = new Point(8, 6);
      this.chkBoxAcceptFiles.Name = "chkBoxAcceptFiles";
      this.chkBoxAcceptFiles.Size = new Size(20, 20);
      this.chkBoxAcceptFiles.TabIndex = 17;
      this.chkBoxAcceptFiles.CheckedChanged += new EventHandler(this.chkBoxAcceptFiles_CheckedChanged);
      this.chkBoxFinishMilestone.Location = new Point(8, 57);
      this.chkBoxFinishMilestone.Name = "chkBoxFinishMilestone";
      this.chkBoxFinishMilestone.Size = new Size(24, 20);
      this.chkBoxFinishMilestone.TabIndex = 11;
      this.chkBoxFinishMilestone.CheckedChanged += new EventHandler(this.chkBoxFinishMilestone_CheckedChanged);
      this.splitter1.Dock = DockStyle.Bottom;
      this.splitter1.Location = new Point(0, 297);
      this.splitter1.Margin = new Padding(1);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(242, 5);
      this.splitter1.TabIndex = 1;
      this.splitter1.TabStop = false;
      this.pnlExPrint.Controls.Add((Control) this.pnlLoanPrint);
      this.pnlExPrint.Controls.Add((Control) this.gradientPanel1);
      this.pnlExPrint.Dock = DockStyle.Bottom;
      this.pnlExPrint.Location = new Point(0, 302);
      this.pnlExPrint.Name = "pnlExPrint";
      this.pnlExPrint.Size = new Size(242, 135);
      this.pnlExPrint.TabIndex = 2;
      this.pnlLoanPrint.Dock = DockStyle.Fill;
      this.pnlLoanPrint.Location = new Point(0, 0);
      this.pnlLoanPrint.Name = "pnlLoanPrint";
      this.pnlLoanPrint.Size = new Size(242, 110);
      this.pnlLoanPrint.TabIndex = 1;
      this.gradientPanel1.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Bottom;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 110);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(242, 25);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel1.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(5, 5);
      this.label1.Name = "label1";
      this.label1.Size = new Size(171, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "*Also affects Secure Form Transfer";
      this.lblMilestoneMgr.Dock = DockStyle.Top;
      this.lblMilestoneMgr.Location = new Point(0, 0);
      this.lblMilestoneMgr.Name = "lblMilestoneMgr";
      this.lblMilestoneMgr.Size = new Size(487, 26);
      this.lblMilestoneMgr.TabIndex = 24;
      this.lblMilestoneMgr.Text = "The persona does not have access to the Pipeline, Loan, Forms and Tools, ePass tabs.";
      this.lblMilestoneMgr.TextAlign = ContentAlignment.MiddleLeft;
      this.lblMilestoneMgr.Visible = false;
      this.pnlBase.Controls.Add((Control) this.pnlExRight);
      this.pnlBase.Controls.Add((Control) this.splitter2);
      this.pnlBase.Controls.Add((Control) this.pnlExLeft);
      this.pnlBase.Dock = DockStyle.Fill;
      this.pnlBase.Location = new Point(0, 26);
      this.pnlBase.Name = "pnlBase";
      this.pnlBase.Size = new Size(487, 437);
      this.pnlBase.TabIndex = 23;
      this.pnlFeeItms.Dock = DockStyle.Fill;
      this.pnlFeeItms.Location = new Point(0, 146);
      this.pnlFeeItms.Name = "pnlFeeItms";
      this.pnlFeeItms.Size = new Size(242, 151);
      this.pnlFeeItms.TabIndex = 3;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(487, 463);
      this.Controls.Add((Control) this.pnlBase);
      this.Controls.Add((Control) this.lblMilestoneMgr);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (LoansPage);
      this.Text = "BorSearchLoanInfoPage";
      this.SizeChanged += new EventHandler(this.LoansPage_SizeChanged);
      this.pnlExRight.ResumeLayout(false);
      this.pnlExLeft.ResumeLayout(false);
      this.gcMilestoneWorkflow.ResumeLayout(false);
      this.pnlMilestoneMgr.ResumeLayout(false);
      this.pnlExPrint.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.pnlBase.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private bool isAnyMilestoneCheckedForFeature(AclMilestone feature)
    {
      List<EllieMae.EMLite.Workflow.Milestone> list = SetupUtil.GetMilestones(this.session, false).ToList<EllieMae.EMLite.Workflow.Milestone>();
      MilestonesAclManager aclManager1 = (MilestonesAclManager) this.session.ACL.GetAclManager(AclCategory.Milestones);
      bool flag = false;
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      if (!this.bIsUserSetup)
        dictionary = aclManager1.GetPermissionsForMilestones(feature, this.personaId);
      for (int index = 0; index < list.Count; ++index)
      {
        EllieMae.EMLite.Workflow.Milestone milestone = list[index];
        if (this.bIsUserSetup)
        {
          if (new MilestoneHelper(this.session).GetPermissionFromUser(feature, this.userId, milestone).Count > 0)
          {
            flag = true;
            break;
          }
        }
        else if (dictionary.ContainsKey(milestone.MilestoneID) && dictionary[milestone.MilestoneID])
        {
          flag = true;
          break;
        }
      }
      if (!flag && feature == AclMilestone.AssignLoanTeamMembers)
      {
        MilestoneFreeRoleAclManager aclManager2 = (MilestoneFreeRoleAclManager) this.session.ACL.GetAclManager(AclCategory.MilestonesFreeRole);
        Hashtable hashtable1 = new Hashtable();
        Hashtable hashtable2 = new Hashtable();
        Hashtable hashtable3;
        if (this.bIsUserSetup)
        {
          UserInfo user = this.session.OrganizationManager.GetUser(this.userId);
          hashtable3 = aclManager2.GetPersonalPermissions(user);
        }
        else
          hashtable3 = aclManager2.GetPermissions(this.personaId);
        IDictionaryEnumerator enumerator = hashtable3.GetEnumerator();
        while (enumerator.MoveNext())
        {
          Hashtable hashtable4 = (Hashtable) enumerator.Value;
          if (hashtable4.ContainsKey((object) "Permission") && (bool) hashtable4[(object) "Permission"])
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    private void lblFinsihMilestoneSelect_Click(object sender, EventArgs e)
    {
      if (!this.pipelineConfiguration.HasPipelineLoanTabAccess())
        return;
      if (this.finishMSMSD == null)
        this.finishMSMSD = !this.bIsUserSetup ? new MilestoneSelectionDialog(this.session, AclMilestone.FinishMilestone, this.personaId, !this.chkBoxFinishMilestone.Enabled, this.DirtyFlagChanged) : new MilestoneSelectionDialog(this.session, AclMilestone.FinishMilestone, this.userId, this.personas, !this.chkBoxFinishMilestone.Enabled, this.DirtyFlagChanged);
      else
        this.finishMSMSD.IsReadOnly = !this.chkBoxFinishMilestone.Enabled;
      if (DialogResult.OK == this.finishMSMSD.ShowDialog((IWin32Window) this))
        this.lblFinsihMilestoneSelect.Tag = (object) this.finishMSMSD.DataView;
      if (this.finishMSMSD.hasMilestoneChecked() && !this.chkBoxFinishMilestone.Checked)
      {
        this.initialSetup = true;
        this.chkBoxFinishMilestone.Checked = true;
        this.initialSetup = false;
      }
      else
      {
        if (this.finishMSMSD.hasMilestoneChecked() || !this.chkBoxFinishMilestone.Checked)
          return;
        this.initialSetup = true;
        this.chkBoxFinishMilestone.Checked = false;
        this.initialSetup = false;
      }
    }

    private void lblReturnToLastMilestoneSelect_Click(object sender, EventArgs e)
    {
      if (!this.pipelineConfiguration.HasPipelineLoanTabAccess())
        return;
      if (this.returnFileMSD == null)
        this.returnFileMSD = !this.bIsUserSetup ? new MilestoneSelectionDialog(this.session, AclMilestone.ReturnFiles, this.personaId, !this.chkBoxReturnToLastMilestone.Enabled, this.DirtyFlagChanged) : new MilestoneSelectionDialog(this.session, AclMilestone.ReturnFiles, this.userId, this.personas, !this.chkBoxReturnToLastMilestone.Enabled, this.DirtyFlagChanged);
      else
        this.returnFileMSD.IsReadOnly = !this.chkBoxReturnToLastMilestone.Enabled;
      if (DialogResult.OK == this.returnFileMSD.ShowDialog((IWin32Window) this))
        this.lblReturnToLastMilestoneSelect.Tag = (object) this.returnFileMSD.DataView;
      if (this.returnFileMSD.hasMilestoneChecked() && !this.chkBoxReturnToLastMilestone.Checked)
      {
        this.initialSetup = true;
        this.chkBoxReturnToLastMilestone.Checked = true;
        this.initialSetup = false;
      }
      else
      {
        if (this.returnFileMSD.hasMilestoneChecked() || !this.chkBoxReturnToLastMilestone.Checked)
          return;
        this.initialSetup = true;
        this.chkBoxReturnToLastMilestone.Checked = false;
        this.initialSetup = false;
      }
    }

    private void lblChangeDateSelect_Click(object sender, EventArgs e)
    {
      if (!this.pipelineConfiguration.HasPipelineLoanTabAccess())
        return;
      if (this.ceDateMSD == null)
        this.ceDateMSD = !this.bIsUserSetup ? new MilestoneSelectionDialog(this.session, AclMilestone.ChangeExpectedDate, this.personaId, !this.chkBoxChangeDate.Enabled, this.DirtyFlagChanged) : new MilestoneSelectionDialog(this.session, AclMilestone.ChangeExpectedDate, this.userId, this.personas, !this.chkBoxChangeDate.Enabled, this.DirtyFlagChanged);
      else
        this.ceDateMSD.IsReadOnly = !this.chkBoxChangeDate.Enabled;
      if (DialogResult.OK == this.ceDateMSD.ShowDialog((IWin32Window) this))
        this.lblChangeDateSelect.Tag = (object) this.ceDateMSD.DataView;
      if (this.ceDateMSD.hasMilestoneChecked() && !this.chkBoxChangeDate.Checked)
      {
        this.initialSetup = true;
        this.chkBoxChangeDate.Checked = true;
        this.initialSetup = false;
      }
      else
      {
        if (this.ceDateMSD.hasMilestoneChecked() || !this.chkBoxChangeDate.Checked)
          return;
        this.initialSetup = true;
        this.chkBoxChangeDate.Checked = false;
        this.initialSetup = false;
      }
    }

    private void lblSetLoanAssociateSelect_Click(object sender, EventArgs e)
    {
      if (!this.pipelineConfiguration.HasPipelineLoanTabAccess())
        return;
      if (this.assLTMMSD == null)
        this.assLTMMSD = !this.bIsUserSetup ? new MilestoneSelectionDialog(this.session, AclMilestone.AssignLoanTeamMembers, this.personaId, !this.chkBoxSetLoanAssociate.Enabled, this.DirtyFlagChanged) : new MilestoneSelectionDialog(this.session, AclMilestone.AssignLoanTeamMembers, this.userId, this.personas, !this.chkBoxSetLoanAssociate.Enabled, this.DirtyFlagChanged);
      else
        this.assLTMMSD.IsReadOnly = !this.chkBoxSetLoanAssociate.Enabled;
      if (DialogResult.OK == this.assLTMMSD.ShowDialog((IWin32Window) this))
        this.lblSetLoanAssociateSelect.Tag = (object) this.assLTMMSD.DataView;
      if (this.assLTMMSD.hasMilestoneChecked() && !this.chkBoxSetLoanAssociate.Checked)
      {
        this.initialSetup = true;
        this.chkBoxSetLoanAssociate.Checked = true;
        this.initialSetup = false;
      }
      else
      {
        if (this.assLTMMSD.hasMilestoneChecked() || !this.chkBoxSetLoanAssociate.Checked)
          return;
        this.initialSetup = true;
        this.chkBoxSetLoanAssociate.Checked = false;
        this.initialSetup = false;
      }
    }

    private void lblAcceptFilesSelect_Click(object sender, EventArgs e)
    {
      if (!this.pipelineConfiguration.HasPipelineLoanTabAccess())
        return;
      if (this.accFileMSD == null)
        this.accFileMSD = !this.bIsUserSetup ? new MilestoneSelectionDialog(this.session, AclMilestone.AcceptFiles, this.personaId, !this.chkBoxAcceptFiles.Enabled, this.DirtyFlagChanged) : new MilestoneSelectionDialog(this.session, AclMilestone.AcceptFiles, this.userId, this.personas, !this.chkBoxAcceptFiles.Enabled, this.DirtyFlagChanged);
      else
        this.accFileMSD.IsReadOnly = !this.chkBoxAcceptFiles.Enabled;
      if (DialogResult.Cancel == this.accFileMSD.ShowDialog((IWin32Window) this))
        this.lblAcceptFilesSelect.Tag = (object) this.accFileMSD.DataView;
      if (this.accFileMSD.hasMilestoneChecked() && !this.chkBoxAcceptFiles.Checked)
      {
        this.initialSetup = true;
        this.chkBoxAcceptFiles.Checked = true;
        this.initialSetup = false;
      }
      else
      {
        if (this.accFileMSD.hasMilestoneChecked() || !this.chkBoxAcceptFiles.Checked)
          return;
        this.initialSetup = true;
        this.chkBoxAcceptFiles.Checked = false;
        this.initialSetup = false;
      }
    }

    private void lblCommentsSelect_Click(object sender, EventArgs e)
    {
      if (!this.pipelineConfiguration.HasPipelineLoanTabAccess())
        return;
      if (this.emCommentMSD == null)
        this.emCommentMSD = !this.bIsUserSetup ? new MilestoneSelectionDialog(this.session, AclMilestone.EditMilestoneComments, this.personaId, !this.chkBoxComments.Enabled, this.DirtyFlagChanged) : new MilestoneSelectionDialog(this.session, AclMilestone.EditMilestoneComments, this.userId, this.personas, !this.chkBoxComments.Enabled, this.DirtyFlagChanged);
      else
        this.emCommentMSD.IsReadOnly = !this.chkBoxComments.Enabled;
      if (DialogResult.Cancel == this.emCommentMSD.ShowDialog((IWin32Window) this))
        this.lblCommentsSelect.Tag = (object) this.emCommentMSD.DataView;
      if (this.emCommentMSD.hasMilestoneChecked() && !this.chkBoxComments.Checked)
      {
        this.initialSetup = true;
        this.chkBoxComments.Checked = true;
        this.initialSetup = false;
      }
      else
      {
        if (this.emCommentMSD.hasMilestoneChecked() || !this.chkBoxComments.Checked)
          return;
        this.initialSetup = true;
        this.chkBoxComments.Checked = false;
        this.initialSetup = false;
      }
    }

    public void SaveData()
    {
      if (!this.NeedToSaveData())
        return;
      if (this.printPage.NeedToSaveData())
        this.printPage.Save();
      if (this.closingPage.NeedToSaveData())
        this.closingPage.Save();
      if (this.docsPage.NeedToSaveData())
        this.docsPage.Save();
      if (this.itemizationPage.NeedToSaveData())
        this.itemizationPage.Save();
      if (this.accFileMSD != null && this.accFileMSD.HasBeenModified)
        this.accFileMSD.SaveData();
      if (this.ceDateMSD != null && this.ceDateMSD.HasBeenModified)
        this.ceDateMSD.SaveData();
      if (this.emCommentMSD != null && this.emCommentMSD.HasBeenModified)
        this.emCommentMSD.SaveData();
      if (this.finishMSMSD != null && this.finishMSMSD.HasBeenModified)
        this.finishMSMSD.SaveData();
      if (this.returnFileMSD != null && this.returnFileMSD.HasBeenModified)
        this.returnFileMSD.SaveData();
      if (this.assLTMMSD != null && this.assLTMMSD.HasBeenModified)
        this.assLTMMSD.SaveData();
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, (EventArgs) null);
    }

    public void ResetData()
    {
      this.printPage.Reset();
      this.closingPage.Reset();
      this.docsPage.Reset();
      this.itemizationPage.Reset();
      this.resetMilestone();
      this.initialSetup = true;
      this.initCheckBoxes();
      this.initialSetup = false;
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, (EventArgs) null);
    }

    public virtual void SetPersona(int personaId)
    {
      if (this.personaId != personaId)
      {
        this.personaId = personaId;
        this.ResetData();
      }
      this.resetMilestone();
      this.initialSetup = true;
      this.initCheckBoxes();
      this.initialSetup = false;
      this.printPage.SetPersona(personaId);
      this.closingPage.SetPersona(personaId);
      this.closingPage.PersonaID = personaId;
      this.docsPage.SetPersona(personaId);
      this.itemizationPage.SetPersona(personaId);
    }

    private void initCheckBoxes()
    {
      this.chkBoxFinishMilestone.Checked = this.isAnyMilestoneCheckedForFeature(AclMilestone.FinishMilestone);
      this.chkBoxReturnToLastMilestone.Checked = this.isAnyMilestoneCheckedForFeature(AclMilestone.ReturnFiles);
      this.chkBoxChangeDate.Checked = this.isAnyMilestoneCheckedForFeature(AclMilestone.ChangeExpectedDate);
      this.chkBoxSetLoanAssociate.Checked = this.isAnyMilestoneCheckedForFeature(AclMilestone.AssignLoanTeamMembers);
      this.chkBoxAcceptFiles.Checked = this.isAnyMilestoneCheckedForFeature(AclMilestone.AcceptFiles);
      this.chkBoxComments.Checked = this.isAnyMilestoneCheckedForFeature(AclMilestone.EditMilestoneComments);
    }

    private void resetMilestone()
    {
      this.chkBoxAcceptFiles.Tag = (object) null;
      this.lblAcceptFilesSelect.Tag = (object) null;
      this.accFileMSD = (MilestoneSelectionDialog) null;
      this.chkBoxChangeDate.Tag = (object) null;
      this.lblChangeDateSelect.Tag = (object) null;
      this.ceDateMSD = (MilestoneSelectionDialog) null;
      this.chkBoxComments.Tag = (object) null;
      this.lblCommentsSelect.Tag = (object) null;
      this.emCommentMSD = (MilestoneSelectionDialog) null;
      this.chkBoxFinishMilestone.Tag = (object) null;
      this.lblFinsihMilestoneSelect.Tag = (object) null;
      this.finishMSMSD = (MilestoneSelectionDialog) null;
      this.chkBoxReturnToLastMilestone.Tag = (object) null;
      this.lblReturnToLastMilestoneSelect.Tag = (object) null;
      this.returnFileMSD = (MilestoneSelectionDialog) null;
      this.chkBoxSetLoanAssociate.Tag = (object) null;
      this.lblSetLoanAssociateSelect.Tag = (object) null;
      this.assLTMMSD = (MilestoneSelectionDialog) null;
    }

    public bool NeedToSaveData()
    {
      return this.printPage.NeedToSaveData() || this.closingPage.NeedToSaveData() || this.docsPage.NeedToSaveData() || this.itemizationPage.NeedToSaveData() || this.accFileMSD != null && this.accFileMSD.HasBeenModified || this.ceDateMSD != null && this.ceDateMSD.HasBeenModified || this.emCommentMSD != null && this.emCommentMSD.HasBeenModified || this.finishMSMSD != null && this.finishMSMSD.HasBeenModified || this.returnFileMSD != null && this.returnFileMSD.HasBeenModified || this.assLTMMSD != null && this.assLTMMSD.HasBeenModified;
    }

    public void MakeReadOnly(bool makeReadOnly)
    {
      this.printPage.MakeReadOnly(makeReadOnly);
      this.closingPage.MakeReadOnly(makeReadOnly);
      this.docsPage.MakeReadOnly(makeReadOnly);
      this.itemizationPage.MakeReadOnly(makeReadOnly);
      this.chkBoxAcceptFiles.Enabled = !makeReadOnly;
      this.chkBoxChangeDate.Enabled = !makeReadOnly;
      this.chkBoxComments.Enabled = !makeReadOnly;
      this.chkBoxFinishMilestone.Enabled = !makeReadOnly;
      this.chkBoxReturnToLastMilestone.Enabled = !makeReadOnly;
      this.chkBoxSetLoanAssociate.Enabled = !makeReadOnly;
      if (!makeReadOnly)
        return;
      this.resetMilestone();
    }

    private void chkBoxAcceptFiles_CheckedChanged(object sender, EventArgs e)
    {
      if (this.initialSetup)
        return;
      int allChecked = !this.chkBoxAcceptFiles.Checked ? 0 : 1;
      if (this.bIsUserSetup)
      {
        if (this.lblAcceptFilesSelect.Tag == null)
        {
          this.accFileMSD = new MilestoneSelectionDialog(this.session, AclMilestone.AcceptFiles, this.userId, this.personas, !this.chkBoxAcceptFiles.Enabled, this.DirtyFlagChanged);
          this.lblAcceptFilesSelect.Tag = (object) this.accFileMSD.DataView;
        }
        this.accFileMSD = new MilestoneSelectionDialog(this.session, allChecked, AclMilestone.AcceptFiles, this.userId, this.personas, !this.chkBoxAcceptFiles.Enabled, this.DirtyFlagChanged);
      }
      else
      {
        if (this.lblAcceptFilesSelect.Tag == null)
        {
          this.accFileMSD = new MilestoneSelectionDialog(this.session, AclMilestone.AcceptFiles, this.personaId, !this.chkBoxAcceptFiles.Enabled, this.DirtyFlagChanged);
          this.lblAcceptFilesSelect.Tag = (object) this.accFileMSD.DataView;
        }
        this.accFileMSD = new MilestoneSelectionDialog(this.session, allChecked, AclMilestone.AcceptFiles, this.personaId, this.DirtyFlagChanged);
      }
      if (this.lblAcceptFilesSelect.Tag != null)
        this.accFileMSD.DataView = (ArrayList) this.lblAcceptFilesSelect.Tag;
      if (DialogResult.OK == this.accFileMSD.ShowDialog((IWin32Window) this))
      {
        this.lblAcceptFilesSelect.Tag = (object) this.accFileMSD.DataView;
        if (this.DirtyFlagChanged != null)
          this.DirtyFlagChanged((object) this, (EventArgs) null);
      }
      if (this.accFileMSD.hasMilestoneChecked() && !this.chkBoxAcceptFiles.Checked)
      {
        this.initialSetup = true;
        this.chkBoxAcceptFiles.Checked = true;
        this.initialSetup = false;
      }
      else
      {
        if (this.accFileMSD.hasMilestoneChecked() || !this.chkBoxAcceptFiles.Checked)
          return;
        this.initialSetup = true;
        this.chkBoxAcceptFiles.Checked = false;
        this.initialSetup = false;
      }
    }

    private void chkBoxReturnToLastMilestone_CheckedChanged(object sender, EventArgs e)
    {
      if (this.initialSetup)
        return;
      int allChecked = !this.chkBoxReturnToLastMilestone.Checked ? 0 : 1;
      if (this.bIsUserSetup)
      {
        if (this.lblReturnToLastMilestoneSelect.Tag == null)
        {
          this.returnFileMSD = new MilestoneSelectionDialog(this.session, AclMilestone.ReturnFiles, this.userId, this.personas, !this.chkBoxAcceptFiles.Enabled, this.DirtyFlagChanged);
          this.lblReturnToLastMilestoneSelect.Tag = (object) this.returnFileMSD.DataView;
        }
        this.returnFileMSD = new MilestoneSelectionDialog(this.session, allChecked, AclMilestone.ReturnFiles, this.userId, this.personas, !this.chkBoxAcceptFiles.Enabled, this.DirtyFlagChanged);
      }
      else
      {
        if (this.lblReturnToLastMilestoneSelect.Tag == null)
        {
          this.returnFileMSD = new MilestoneSelectionDialog(this.session, AclMilestone.ReturnFiles, this.personaId, !this.chkBoxReturnToLastMilestone.Enabled, this.DirtyFlagChanged);
          this.lblReturnToLastMilestoneSelect.Tag = (object) this.returnFileMSD.DataView;
        }
        this.returnFileMSD = new MilestoneSelectionDialog(this.session, allChecked, AclMilestone.ReturnFiles, this.personaId, this.DirtyFlagChanged);
      }
      if (this.lblReturnToLastMilestoneSelect.Tag != null)
        this.returnFileMSD.DataView = (ArrayList) this.lblReturnToLastMilestoneSelect.Tag;
      if (DialogResult.OK == this.returnFileMSD.ShowDialog((IWin32Window) this))
      {
        this.lblReturnToLastMilestoneSelect.Tag = (object) this.returnFileMSD.DataView;
        if (this.DirtyFlagChanged != null)
          this.DirtyFlagChanged((object) this, (EventArgs) null);
      }
      if (this.returnFileMSD.hasMilestoneChecked() && !this.chkBoxReturnToLastMilestone.Checked)
      {
        this.initialSetup = true;
        this.chkBoxReturnToLastMilestone.Checked = true;
        this.initialSetup = false;
      }
      else
      {
        if (this.returnFileMSD.hasMilestoneChecked() || !this.chkBoxReturnToLastMilestone.Checked)
          return;
        this.initialSetup = true;
        this.chkBoxReturnToLastMilestone.Checked = false;
        this.initialSetup = false;
      }
    }

    private void chkBoxChangeDate_CheckedChanged(object sender, EventArgs e)
    {
      if (this.initialSetup)
        return;
      int allChecked = !this.chkBoxChangeDate.Checked ? 0 : 1;
      if (this.bIsUserSetup)
      {
        if (this.lblChangeDateSelect.Tag == null)
        {
          this.ceDateMSD = new MilestoneSelectionDialog(this.session, AclMilestone.ChangeExpectedDate, this.userId, this.personas, !this.chkBoxAcceptFiles.Enabled, this.DirtyFlagChanged);
          this.lblChangeDateSelect.Tag = (object) this.ceDateMSD.DataView;
        }
        this.ceDateMSD = new MilestoneSelectionDialog(this.session, allChecked, AclMilestone.ChangeExpectedDate, this.userId, this.personas, !this.chkBoxAcceptFiles.Enabled, this.DirtyFlagChanged);
      }
      else
      {
        if (this.lblChangeDateSelect.Tag == null)
        {
          this.ceDateMSD = new MilestoneSelectionDialog(this.session, AclMilestone.ChangeExpectedDate, this.personaId, !this.chkBoxChangeDate.Enabled, this.DirtyFlagChanged);
          this.lblChangeDateSelect.Tag = (object) this.ceDateMSD.DataView;
        }
        this.ceDateMSD = new MilestoneSelectionDialog(this.session, allChecked, AclMilestone.ChangeExpectedDate, this.personaId, this.DirtyFlagChanged);
      }
      if (this.lblChangeDateSelect.Tag != null)
        this.ceDateMSD.DataView = (ArrayList) this.lblChangeDateSelect.Tag;
      if (DialogResult.OK == this.ceDateMSD.ShowDialog((IWin32Window) this))
      {
        this.lblChangeDateSelect.Tag = (object) this.ceDateMSD.DataView;
        if (this.DirtyFlagChanged != null)
          this.DirtyFlagChanged((object) this, (EventArgs) null);
      }
      if (this.ceDateMSD.hasMilestoneChecked() && !this.chkBoxChangeDate.Checked)
      {
        this.initialSetup = true;
        this.chkBoxChangeDate.Checked = true;
        this.initialSetup = false;
      }
      else
      {
        if (this.ceDateMSD.hasMilestoneChecked() || !this.chkBoxChangeDate.Checked)
          return;
        this.initialSetup = true;
        this.chkBoxChangeDate.Checked = false;
        this.initialSetup = false;
      }
    }

    private void chkBoxFinishMilestone_CheckedChanged(object sender, EventArgs e)
    {
      if (this.initialSetup)
        return;
      int allChecked = !this.chkBoxFinishMilestone.Checked ? 0 : 1;
      if (this.bIsUserSetup)
      {
        if (this.lblFinsihMilestoneSelect.Tag == null)
        {
          this.finishMSMSD = new MilestoneSelectionDialog(this.session, AclMilestone.FinishMilestone, this.userId, this.personas, !this.chkBoxAcceptFiles.Enabled, this.DirtyFlagChanged);
          this.lblFinsihMilestoneSelect.Tag = (object) this.finishMSMSD.DataView;
        }
        this.finishMSMSD = new MilestoneSelectionDialog(this.session, allChecked, AclMilestone.FinishMilestone, this.userId, this.personas, !this.chkBoxAcceptFiles.Enabled, this.DirtyFlagChanged);
      }
      else
      {
        if (this.lblFinsihMilestoneSelect.Tag == null)
        {
          this.finishMSMSD = new MilestoneSelectionDialog(this.session, AclMilestone.FinishMilestone, this.personaId, !this.chkBoxFinishMilestone.Enabled, this.DirtyFlagChanged);
          this.lblFinsihMilestoneSelect.Tag = (object) this.finishMSMSD.DataView;
        }
        this.finishMSMSD = new MilestoneSelectionDialog(this.session, allChecked, AclMilestone.FinishMilestone, this.personaId, this.DirtyFlagChanged);
      }
      if (this.lblFinsihMilestoneSelect.Tag != null)
        this.finishMSMSD.DataView = (ArrayList) this.lblFinsihMilestoneSelect.Tag;
      if (DialogResult.OK == this.finishMSMSD.ShowDialog((IWin32Window) this))
      {
        this.lblFinsihMilestoneSelect.Tag = (object) this.finishMSMSD.DataView;
        if (this.DirtyFlagChanged != null)
          this.DirtyFlagChanged((object) this, (EventArgs) null);
      }
      if (this.finishMSMSD.hasMilestoneChecked() && !this.chkBoxFinishMilestone.Checked)
      {
        this.initialSetup = true;
        this.chkBoxFinishMilestone.Checked = true;
        this.initialSetup = false;
      }
      else
      {
        if (this.finishMSMSD.hasMilestoneChecked() || !this.chkBoxFinishMilestone.Checked)
          return;
        this.initialSetup = true;
        this.chkBoxFinishMilestone.Checked = false;
        this.initialSetup = false;
      }
    }

    private void chkBoxSetLoanAssociate_CheckedChanged(object sender, EventArgs e)
    {
      if (this.initialSetup)
        return;
      int allChecked = !this.chkBoxSetLoanAssociate.Checked ? 0 : 1;
      if (this.bIsUserSetup)
      {
        if (this.lblSetLoanAssociateSelect.Tag == null)
        {
          this.assLTMMSD = new MilestoneSelectionDialog(this.session, AclMilestone.AssignLoanTeamMembers, this.userId, this.personas, !this.chkBoxAcceptFiles.Enabled, this.DirtyFlagChanged);
          this.lblSetLoanAssociateSelect.Tag = (object) this.assLTMMSD.DataView;
        }
        this.assLTMMSD = new MilestoneSelectionDialog(this.session, allChecked, AclMilestone.AssignLoanTeamMembers, this.userId, this.personas, !this.chkBoxAcceptFiles.Enabled, this.DirtyFlagChanged);
      }
      else
      {
        if (this.lblSetLoanAssociateSelect.Tag == null)
        {
          this.assLTMMSD = new MilestoneSelectionDialog(this.session, AclMilestone.AssignLoanTeamMembers, this.personaId, !this.chkBoxReturnToLastMilestone.Enabled, this.DirtyFlagChanged);
          this.lblSetLoanAssociateSelect.Tag = (object) this.assLTMMSD.DataView;
        }
        this.assLTMMSD = new MilestoneSelectionDialog(this.session, allChecked, AclMilestone.AssignLoanTeamMembers, this.personaId, this.DirtyFlagChanged);
      }
      if (this.lblSetLoanAssociateSelect.Tag != null)
        this.assLTMMSD.DataView = (ArrayList) this.lblSetLoanAssociateSelect.Tag;
      if (DialogResult.OK == this.assLTMMSD.ShowDialog((IWin32Window) this))
      {
        this.lblSetLoanAssociateSelect.Tag = (object) this.assLTMMSD.DataView;
        if (this.DirtyFlagChanged != null)
          this.DirtyFlagChanged((object) this, (EventArgs) null);
      }
      if (this.assLTMMSD.hasMilestoneChecked() && !this.chkBoxSetLoanAssociate.Checked)
      {
        this.initialSetup = true;
        this.chkBoxSetLoanAssociate.Checked = true;
        this.initialSetup = false;
      }
      else
      {
        if (this.assLTMMSD.hasMilestoneChecked() || !this.chkBoxSetLoanAssociate.Checked)
          return;
        this.initialSetup = true;
        this.chkBoxSetLoanAssociate.Checked = false;
        this.initialSetup = false;
      }
    }

    private void chkBoxComments_CheckedChanged(object sender, EventArgs e)
    {
      if (this.initialSetup)
        return;
      int allChecked = !this.chkBoxComments.Checked ? 0 : 1;
      if (this.bIsUserSetup)
      {
        if (this.lblCommentsSelect.Tag == null)
        {
          this.emCommentMSD = new MilestoneSelectionDialog(this.session, AclMilestone.EditMilestoneComments, this.userId, this.personas, !this.chkBoxAcceptFiles.Enabled, this.DirtyFlagChanged);
          this.lblCommentsSelect.Tag = (object) this.emCommentMSD.DataView;
        }
        this.emCommentMSD = new MilestoneSelectionDialog(this.session, allChecked, AclMilestone.EditMilestoneComments, this.userId, this.personas, !this.chkBoxAcceptFiles.Enabled, this.DirtyFlagChanged);
      }
      else
      {
        if (this.lblCommentsSelect.Tag == null)
        {
          this.emCommentMSD = new MilestoneSelectionDialog(this.session, AclMilestone.EditMilestoneComments, this.personaId, !this.chkBoxComments.Enabled, this.DirtyFlagChanged);
          this.lblCommentsSelect.Tag = (object) this.emCommentMSD.DataView;
        }
        this.emCommentMSD = new MilestoneSelectionDialog(this.session, allChecked, AclMilestone.EditMilestoneComments, this.personaId, this.DirtyFlagChanged);
      }
      if (this.lblCommentsSelect.Tag != null)
        this.emCommentMSD.DataView = (ArrayList) this.lblCommentsSelect.Tag;
      if (DialogResult.OK == this.emCommentMSD.ShowDialog((IWin32Window) this))
      {
        this.lblCommentsSelect.Tag = (object) this.emCommentMSD.DataView;
        if (this.DirtyFlagChanged != null)
          this.DirtyFlagChanged((object) this, (EventArgs) null);
      }
      if (this.emCommentMSD.hasMilestoneChecked() && !this.chkBoxComments.Checked)
      {
        this.initialSetup = true;
        this.chkBoxComments.Checked = true;
        this.initialSetup = false;
      }
      else
      {
        if (this.emCommentMSD.hasMilestoneChecked() || !this.chkBoxComments.Checked)
          return;
        this.initialSetup = true;
        this.chkBoxComments.Checked = false;
        this.initialSetup = false;
      }
    }

    private void LoansPage_SizeChanged(object sender, EventArgs e)
    {
      this.pnlExLeft.Width = this.Width / 2;
    }

    private void label2_Click(object sender, EventArgs e)
    {
    }
  }
}

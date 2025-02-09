// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SecurityGroupSettingsMainForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.BrokerUserGroup;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SecurityGroupSettingsMainForm : Form
  {
    private IWin32Window owner;
    private Sessions.Session session;
    private EventHandler dirtyFlagChangedEventHandler;
    private TabControl tabControl1;
    private LoanTemplatesPage loanTemplatesPage;
    private ResourcesPage resourcesPage;
    private ListViewPage listViewPage;
    private LoansGroupPage loansGroupPage;
    private BorrowerContactsPage borContactsPage;
    private MembersPage membersPage;
    private AccessPage accessPage;
    private IGroupSecurityPage[] subpages;
    private int currentGroupId = -1;
    private string userID = "";
    private bool personal;
    private bool skipChecking;
    private AclGroup[] currentGroupList;
    private TabPage tabPageLoanTemplates;
    private TabPage tabPageResources;
    private TabPage tabPageListView;
    private TabPage tabPageLoans;
    private TabPage tabPageBorrowerContacts;
    private bool isTPOMVP;
    private string groupName = "";
    private TabPage tabPageMembers;
    private GroupContainer gcGroupSettings;
    private StandardIconButton stdIconBtnSave;
    private StandardIconButton stdIconBtnReset;
    private ToolTip toolTip1;
    private TabPage tabPageBrokerAccess;
    private IContainer components;

    public int CurrentGroupId
    {
      get => this.currentGroupId;
      set
      {
        if (!this.skipChecking)
          this.CheckUnsavedData();
        this.currentGroupId = value;
        if (this.subpages == null)
          return;
        for (int index = 0; index < this.subpages.Length; ++index)
        {
          if (this.subpages[index] != null)
            this.subpages[index].SetGroup(value);
        }
      }
    }

    public string CurrentGroupName
    {
      set => this.groupName = value;
    }

    private bool isBanker => this.session.EncompassEdition == EncompassEdition.Banker;

    private void applyEncompassEdition()
    {
      if (this.isBanker)
      {
        this.tabControl1.TabPages.Remove(this.tabPageBrokerAccess);
      }
      else
      {
        this.tabControl1.TabPages.Clear();
        this.tabControl1.TabPages.Add(this.tabPageMembers);
        this.tabControl1.TabPages.Add(this.tabPageListView);
        this.tabControl1.TabPages.Add(this.tabPageBrokerAccess);
      }
    }

    public bool SkipChecking
    {
      get => this.skipChecking;
      set => this.skipChecking = value;
    }

    public SecurityGroupSettingsMainForm(Sessions.Session session, IWin32Window owner, int groupId)
    {
      this.InitializeComponent();
      this.session = session;
      this.personal = false;
      this.applyEncompassEdition();
      this.init(owner);
      this.loadTabPagesForGroup();
    }

    public SecurityGroupSettingsMainForm(
      Sessions.Session session,
      IWin32Window owner,
      string userID,
      AclGroup[] groups,
      bool isTPOMVP = false)
    {
      this.InitializeComponent();
      this.isTPOMVP = isTPOMVP;
      this.session = session;
      this.personal = true;
      this.currentGroupList = groups;
      this.userID = userID;
      this.applyEncompassEdition();
      this.stdIconBtnSave.Visible = false;
      this.stdIconBtnReset.Visible = false;
      this.init(owner);
      this.loadTabPagesForUser();
    }

    private void init(IWin32Window owner)
    {
      this.owner = owner;
      if (this.dirtyFlagChangedEventHandler == null)
        this.dirtyFlagChangedEventHandler = new EventHandler(this.onDirtyFlagChange);
      this.onDirtyFlagChange((object) this, (EventArgs) null);
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
      this.tabControl1 = new TabControl();
      this.tabPageMembers = new TabPage();
      this.tabPageLoans = new TabPage();
      this.tabPageBorrowerContacts = new TabPage();
      this.tabPageLoanTemplates = new TabPage();
      this.tabPageResources = new TabPage();
      this.tabPageListView = new TabPage();
      this.tabPageBrokerAccess = new TabPage();
      this.gcGroupSettings = new GroupContainer();
      this.stdIconBtnSave = new StandardIconButton();
      this.stdIconBtnReset = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.tabControl1.SuspendLayout();
      this.gcGroupSettings.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnSave).BeginInit();
      ((ISupportInitialize) this.stdIconBtnReset).BeginInit();
      this.SuspendLayout();
      this.tabControl1.Controls.Add((Control) this.tabPageMembers);
      this.tabControl1.Controls.Add((Control) this.tabPageLoans);
      this.tabControl1.Controls.Add((Control) this.tabPageBorrowerContacts);
      this.tabControl1.Controls.Add((Control) this.tabPageLoanTemplates);
      this.tabControl1.Controls.Add((Control) this.tabPageResources);
      this.tabControl1.Controls.Add((Control) this.tabPageListView);
      this.tabControl1.Controls.Add((Control) this.tabPageBrokerAccess);
      this.tabControl1.Dock = DockStyle.Fill;
      this.tabControl1.Location = new Point(2, 26);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(595, 444);
      this.tabControl1.TabIndex = 1;
      this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
      this.tabPageMembers.Location = new Point(4, 22);
      this.tabPageMembers.Name = "tabPageMembers";
      this.tabPageMembers.Size = new Size(587, 418);
      this.tabPageMembers.TabIndex = 5;
      this.tabPageMembers.Text = "Members";
      this.tabPageMembers.UseVisualStyleBackColor = true;
      this.tabPageLoans.Location = new Point(4, 22);
      this.tabPageLoans.Name = "tabPageLoans";
      this.tabPageLoans.Size = new Size(587, 418);
      this.tabPageLoans.TabIndex = 3;
      this.tabPageLoans.Text = "Loans";
      this.tabPageLoans.UseVisualStyleBackColor = true;
      this.tabPageLoans.Visible = false;
      this.tabPageBorrowerContacts.Location = new Point(4, 22);
      this.tabPageBorrowerContacts.Name = "tabPageBorrowerContacts";
      this.tabPageBorrowerContacts.Size = new Size(587, 418);
      this.tabPageBorrowerContacts.TabIndex = 4;
      this.tabPageBorrowerContacts.Text = "Borrower Contacts";
      this.tabPageBorrowerContacts.UseVisualStyleBackColor = true;
      this.tabPageLoanTemplates.AutoScroll = true;
      this.tabPageLoanTemplates.Location = new Point(4, 22);
      this.tabPageLoanTemplates.Name = "tabPageLoanTemplates";
      this.tabPageLoanTemplates.Size = new Size(587, 418);
      this.tabPageLoanTemplates.TabIndex = 0;
      this.tabPageLoanTemplates.Text = "Loan Templates";
      this.tabPageLoanTemplates.UseVisualStyleBackColor = true;
      this.tabPageResources.AutoScroll = true;
      this.tabPageResources.Location = new Point(4, 22);
      this.tabPageResources.Name = "tabPageResources";
      this.tabPageResources.Size = new Size(587, 418);
      this.tabPageResources.TabIndex = 1;
      this.tabPageResources.Text = "Resources";
      this.tabPageResources.UseVisualStyleBackColor = true;
      this.tabPageResources.Visible = false;
      this.tabPageResources.SizeChanged += new EventHandler(this.tabPageResources_SizeChanged);
      this.tabPageListView.Location = new Point(4, 22);
      this.tabPageListView.Name = "tabPageListView";
      this.tabPageListView.Size = new Size(587, 418);
      this.tabPageListView.TabIndex = 2;
      this.tabPageListView.Text = "Role List View";
      this.tabPageListView.UseVisualStyleBackColor = true;
      this.tabPageListView.Visible = false;
      this.tabPageBrokerAccess.Location = new Point(4, 22);
      this.tabPageBrokerAccess.Name = "tabPageBrokerAccess";
      this.tabPageBrokerAccess.Size = new Size(587, 418);
      this.tabPageBrokerAccess.TabIndex = 6;
      this.tabPageBrokerAccess.Text = "Access";
      this.tabPageBrokerAccess.UseVisualStyleBackColor = true;
      this.gcGroupSettings.Controls.Add((Control) this.stdIconBtnSave);
      this.gcGroupSettings.Controls.Add((Control) this.stdIconBtnReset);
      this.gcGroupSettings.Controls.Add((Control) this.tabControl1);
      this.gcGroupSettings.Dock = DockStyle.Fill;
      this.gcGroupSettings.HeaderForeColor = SystemColors.ControlText;
      this.gcGroupSettings.Location = new Point(0, 0);
      this.gcGroupSettings.Name = "gcGroupSettings";
      this.gcGroupSettings.Padding = new Padding(1, 0, 0, 0);
      this.gcGroupSettings.Size = new Size(598, 471);
      this.gcGroupSettings.TabIndex = 2;
      this.gcGroupSettings.Text = "Group Settings";
      this.stdIconBtnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnSave.BackColor = Color.Transparent;
      this.stdIconBtnSave.Location = new Point(553, 5);
      this.stdIconBtnSave.MouseDownImage = (Image) null;
      this.stdIconBtnSave.Name = "stdIconBtnSave";
      this.stdIconBtnSave.Size = new Size(16, 16);
      this.stdIconBtnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.stdIconBtnSave.TabIndex = 3;
      this.stdIconBtnSave.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnSave, "Save");
      this.stdIconBtnSave.Click += new EventHandler(this.btnSave_Click);
      this.stdIconBtnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnReset.BackColor = Color.Transparent;
      this.stdIconBtnReset.Location = new Point(575, 5);
      this.stdIconBtnReset.MouseDownImage = (Image) null;
      this.stdIconBtnReset.Name = "stdIconBtnReset";
      this.stdIconBtnReset.Size = new Size(16, 16);
      this.stdIconBtnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.stdIconBtnReset.TabIndex = 2;
      this.stdIconBtnReset.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnReset, "Reset");
      this.stdIconBtnReset.Click += new EventHandler(this.btnReset_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(598, 471);
      this.Controls.Add((Control) this.gcGroupSettings);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (SecurityGroupSettingsMainForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Group Settings";
      this.BackColorChanged += new EventHandler(this.SecurityGroupSettingsMainForm_BackColorChanged);
      this.tabControl1.ResumeLayout(false);
      this.gcGroupSettings.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnSave).EndInit();
      ((ISupportInitialize) this.stdIconBtnReset).EndInit();
      this.ResumeLayout(false);
    }

    public string Title
    {
      set => this.gcGroupSettings.Text = value;
    }

    private void onDirtyFlagChange(object sender, EventArgs e)
    {
      this.stdIconBtnSave.Enabled = this.stdIconBtnReset.Enabled = this.IsDirty();
    }

    private void loadTabPagesForGroup()
    {
      if (this.dirtyFlagChangedEventHandler == null)
        this.dirtyFlagChangedEventHandler = new EventHandler(this.onDirtyFlagChange);
      this.membersPage = new MembersPage(this.session, this.currentGroupId, this.dirtyFlagChangedEventHandler);
      this.membersPage.TopLevel = false;
      this.membersPage.Visible = true;
      this.membersPage.Dock = DockStyle.Fill;
      this.membersPage.BackColor = this.BackColor;
      this.tabPageMembers.Controls.Add((Control) this.membersPage);
      this.subpages = new IGroupSecurityPage[7]
      {
        (IGroupSecurityPage) this.membersPage,
        (IGroupSecurityPage) this.loanTemplatesPage,
        (IGroupSecurityPage) this.resourcesPage,
        (IGroupSecurityPage) this.listViewPage,
        (IGroupSecurityPage) this.loansGroupPage,
        (IGroupSecurityPage) this.borContactsPage,
        (IGroupSecurityPage) this.accessPage
      };
    }

    public void CheckUnsavedData()
    {
      if (!this.IsDirty())
        return;
      if (Utils.Dialog((IWin32Window) this, "Do you want to save your changes before selecting another group?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
        this.Save();
      else
        this.Reset();
    }

    public bool IsDirty()
    {
      bool flag = false;
      if (this.membersPage != null && this.membersPage.HasBeenModified())
        flag = true;
      else if (this.loanTemplatesPage != null && this.loanTemplatesPage.HasBeenModified())
        flag = true;
      else if (this.resourcesPage != null && this.resourcesPage.HasBeenModified())
        flag = true;
      else if (this.listViewPage != null && this.listViewPage.HasBeenModified())
        flag = true;
      else if (this.loansGroupPage != null && this.loansGroupPage.HasBeenModified())
        flag = true;
      else if (this.borContactsPage != null && this.borContactsPage.HasBeenModified())
        flag = true;
      else if (this.accessPage != null && this.accessPage.HasBeenModified())
        flag = true;
      return flag;
    }

    private void loadTabPagesForUser()
    {
      if (this.isBanker)
      {
        this.loansGroupPage = new LoansGroupPage(this.session, this.userID, this.currentGroupList, this.dirtyFlagChangedEventHandler, this.isTPOMVP);
        this.loansGroupPage.TopLevel = false;
        this.loansGroupPage.Visible = true;
        this.loansGroupPage.Dock = DockStyle.Fill;
        this.loansGroupPage.BackColor = this.BackColor;
        this.tabPageLoans.Controls.Add((Control) this.loansGroupPage);
      }
      else
      {
        this.listViewPage = new ListViewPage(this.session, this.userID, this.currentGroupList, this.dirtyFlagChangedEventHandler);
        this.listViewPage.TopLevel = false;
        this.listViewPage.Visible = true;
        this.listViewPage.Dock = DockStyle.Fill;
        this.listViewPage.BackColor = this.BackColor;
        this.tabPageListView.Controls.Add((Control) this.listViewPage);
      }
      this.subpages = new IGroupSecurityPage[7]
      {
        (IGroupSecurityPage) this.membersPage,
        (IGroupSecurityPage) this.loanTemplatesPage,
        (IGroupSecurityPage) this.resourcesPage,
        (IGroupSecurityPage) this.listViewPage,
        (IGroupSecurityPage) this.loansGroupPage,
        (IGroupSecurityPage) this.borContactsPage,
        (IGroupSecurityPage) this.accessPage
      };
      this.tabControl1.Controls.Remove((Control) this.tabPageMembers);
    }

    private void SecurityGroupSettingsMainForm_BackColorChanged(object sender, EventArgs e)
    {
      if (this.membersPage != null)
        this.membersPage.BackColor = this.BackColor;
      if (this.loansGroupPage != null)
        this.loansGroupPage.BackColor = this.BackColor;
      if (this.loanTemplatesPage != null)
        this.loanTemplatesPage.BackColor = this.BackColor;
      if (this.resourcesPage != null)
        this.resourcesPage.BackColor = this.BackColor;
      if (this.borContactsPage != null)
        this.borContactsPage.BackColor = this.BackColor;
      if (this.listViewPage != null)
        this.listViewPage.BackColor = this.BackColor;
      if (this.accessPage != null)
        this.accessPage.BackColor = this.BackColor;
      this.stdIconBtnSave.Enabled = this.stdIconBtnReset.Enabled = false;
    }

    public void Save()
    {
      bool flag = this.IsDirty();
      if (this.membersPage != null)
        this.membersPage.SaveData();
      if (this.loansGroupPage != null)
        this.loansGroupPage.SaveData();
      if (this.loanTemplatesPage != null)
        this.loanTemplatesPage.SaveData();
      if (this.resourcesPage != null)
        this.resourcesPage.SaveData();
      if (this.borContactsPage != null)
        this.borContactsPage.SaveData();
      if (this.listViewPage != null)
        this.listViewPage.SaveData();
      if (this.accessPage != null)
        this.accessPage.SaveData();
      if (flag)
        this.session.InsertSystemAuditRecord((SystemAuditRecord) new UserGroupAuditRecord(this.session.UserID, this.session.UserInfo.FullName, EllieMae.EMLite.ClientServer.SystemAuditTrail.ActionType.UserGroupModified, this.membersPage.CurrentAclGroup.ID, this.membersPage.CurrentAclGroup.Name));
      this.stdIconBtnSave.Enabled = this.stdIconBtnReset.Enabled = false;
    }

    public void Reset()
    {
      if (this.membersPage != null)
        this.membersPage.ResetData();
      if (this.loansGroupPage != null)
        this.loansGroupPage.ResetData();
      if (this.loanTemplatesPage != null)
        this.loanTemplatesPage.ResetData();
      if (this.resourcesPage != null)
        this.resourcesPage.ResetData();
      if (this.borContactsPage != null)
        this.borContactsPage.ResetData();
      if (this.listViewPage != null)
        this.listViewPage.ResetData();
      if (this.accessPage == null)
        return;
      this.accessPage.ResetData();
    }

    private void btnSave_Click(object sender, EventArgs e) => this.Save();

    private void btnReset_Click(object sender, EventArgs e)
    {
      if (ResetConfirmDialog.ShowDialog(this.owner, (string) null) == DialogResult.No)
        return;
      this.Reset();
    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
      string text = this.tabControl1.SelectedTab.Text;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(text))
      {
        case 366520832:
          if (!(text == "Borrower Contacts") || this.borContactsPage != null)
            break;
          this.borContactsPage = this.personal ? new BorrowerContactsPage(this.session, this.userID, this.currentGroupList, this.dirtyFlagChangedEventHandler) : new BorrowerContactsPage(this.session, this.currentGroupId, this.dirtyFlagChangedEventHandler);
          this.borContactsPage.TopLevel = false;
          this.borContactsPage.Visible = true;
          this.borContactsPage.Dock = DockStyle.Fill;
          this.borContactsPage.BackColor = this.BackColor;
          this.tabPageBorrowerContacts.Controls.Add((Control) this.borContactsPage);
          this.subpages[5] = (IGroupSecurityPage) this.borContactsPage;
          break;
        case 1183464856:
          if (!(text == "Loans") || this.loansGroupPage != null)
            break;
          this.loansGroupPage = this.personal ? new LoansGroupPage(this.session, this.userID, this.currentGroupList, this.dirtyFlagChangedEventHandler, this.isTPOMVP) : new LoansGroupPage(this.session, this.dirtyFlagChangedEventHandler, this.isTPOMVP);
          this.loansGroupPage.TopLevel = false;
          this.loansGroupPage.Visible = true;
          this.loansGroupPage.Dock = DockStyle.Fill;
          this.loansGroupPage.BackColor = this.BackColor;
          this.tabPageLoans.Controls.Add((Control) this.loansGroupPage);
          this.loansGroupPage.SetGroup(this.CurrentGroupId);
          this.subpages[4] = (IGroupSecurityPage) this.loansGroupPage;
          break;
        case 1371735652:
          if (!(text == "Role List View") || this.listViewPage != null)
            break;
          this.listViewPage = this.personal ? new ListViewPage(this.session, this.userID, this.currentGroupList, this.dirtyFlagChangedEventHandler) : new ListViewPage(this.session, this.currentGroupId, this.dirtyFlagChangedEventHandler);
          this.listViewPage.TopLevel = false;
          this.listViewPage.Visible = true;
          this.listViewPage.Dock = DockStyle.Fill;
          this.listViewPage.BackColor = this.BackColor;
          this.tabPageListView.Controls.Add((Control) this.listViewPage);
          this.subpages[3] = (IGroupSecurityPage) this.listViewPage;
          break;
        case 1505013259:
          if (!(text == "Access") || this.accessPage != null)
            break;
          this.accessPage = this.personal ? new AccessPage(this.userID, this.currentGroupList, this.dirtyFlagChangedEventHandler) : new AccessPage(this.currentGroupId, this.dirtyFlagChangedEventHandler);
          this.accessPage.TopLevel = false;
          this.accessPage.Visible = true;
          this.accessPage.Dock = DockStyle.Fill;
          this.accessPage.BackColor = this.BackColor;
          this.tabPageBrokerAccess.Controls.Add((Control) this.accessPage);
          this.subpages[6] = (IGroupSecurityPage) this.accessPage;
          break;
        case 2506401938:
          if (!(text == "Resources") || this.resourcesPage != null)
            break;
          this.resourcesPage = this.personal ? new ResourcesPage(this.session, this.userID, this.currentGroupList, this.dirtyFlagChangedEventHandler) : new ResourcesPage(this.session, this.currentGroupId, this.dirtyFlagChangedEventHandler);
          this.resourcesPage.Visible = true;
          this.resourcesPage.Location = new Point(0, 0);
          this.resourcesPage.Width = this.tabPageResources.Width;
          this.resourcesPage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
          this.resourcesPage.BackColor = this.BackColor;
          this.tabPageResources.VerticalScroll.Visible = true;
          this.tabPageResources.VerticalScroll.Enabled = true;
          this.tabPageResources.Controls.Add((Control) this.resourcesPage);
          this.tabPageResources.ScrollControlIntoView((Control) this.resourcesPage);
          this.subpages[2] = (IGroupSecurityPage) this.resourcesPage;
          break;
        case 3980170928:
          if (!(text == "Members") || this.membersPage != null || this.personal)
            break;
          this.membersPage = new MembersPage(this.session, this.currentGroupId, this.dirtyFlagChangedEventHandler);
          this.membersPage.TopLevel = false;
          this.membersPage.Visible = true;
          this.membersPage.Dock = DockStyle.Fill;
          this.membersPage.BackColor = this.BackColor;
          this.tabPageMembers.Controls.Add((Control) this.membersPage);
          this.subpages[0] = (IGroupSecurityPage) this.membersPage;
          break;
        case 4035684956:
          if (!(text == "Loan Templates") || this.loanTemplatesPage != null)
            break;
          this.loanTemplatesPage = this.personal ? new LoanTemplatesPage(this.session, this.userID, this.currentGroupList, this.dirtyFlagChangedEventHandler) : new LoanTemplatesPage(this.session, this.currentGroupId, this.dirtyFlagChangedEventHandler);
          this.loanTemplatesPage.TopLevel = false;
          this.loanTemplatesPage.Visible = true;
          this.loanTemplatesPage.Width = this.tabPageLoanTemplates.Width;
          this.loanTemplatesPage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
          this.loanTemplatesPage.BackColor = this.BackColor;
          this.tabPageLoanTemplates.Controls.Add((Control) this.loanTemplatesPage);
          this.subpages[1] = (IGroupSecurityPage) this.loanTemplatesPage;
          break;
      }
    }

    private void tabPageResources_SizeChanged(object sender, EventArgs e)
    {
      this.tabPageResources.ScrollControlIntoView((Control) this.resourcesPage);
    }
  }
}

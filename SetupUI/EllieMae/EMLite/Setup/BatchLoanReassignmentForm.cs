// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BatchLoanReassignmentForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI.Forms;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class BatchLoanReassignmentForm : UserControl
  {
    private SetUpContainer setupContainer;
    private Sessions.Session session;
    private List<ExternalOriginatorManagementData> externalOrgsList;
    private PipeLineExtOrgInfo pipeLineExtOrgInfo;
    private ExternalOriginatorManagementData selectedOrg;
    private bool isCompanySearchAvailable;
    private Label lblSearchJobFunction;
    private ComboBox comBoxSearchJobFunction;
    private Label lblUserName;
    private TextBox txtSelectedUser;
    private Label label1;
    private Label label2;
    private ComboBox comBoxLoanFolder;
    private ComboBox comBoxAssignToJobFunction;
    private GridView listvwLoan;
    private GridView listvwUser;
    private IContainer components;
    private UserInfo currentselectedUser;
    private string className = nameof (BatchLoanReassignmentForm);
    private static readonly string sw = Tracing.SwOutsideLoan;
    private WorkflowManager workflowMgr;
    private RoleInfo[] roleList;
    private IOrganizationManager iorgMgrObj;
    public static string UserID_HashKey = "UserID";
    public static string LoanList_HashKey = "LoanList";
    public static string RoleID_HashKey = "RoleID";
    public static string MilestoneIDList_HashKey = "MilestoneIDList";
    private PanelEx pnlExRight;
    private GroupContainer gcSelectRole;
    private GroupContainer gcReassign;
    private GroupContainer gcSelectLoans;
    private PanelEx pnlExSelectLoans;
    private Button btnReassign;
    private Button btnSearch;
    private StandardIconButton stdIconBtnFind;
    private ToolTip toolTip1;
    private Splitter splitter1;
    private StandardIconButton btnReset;
    private bool suspendEvents;
    private ICursor _cursor;
    private GroupContainer gcLoans;
    private PageListNavigator pageListNavigator1;
    private Label label3;
    private GroupContainer groupContainer1;
    private Label label4;
    private CheckBox checkBox1;
    private Label lblCompany;
    private ComboBox cboCompany;
    private StandardIconButton stdicoCompanySearch;
    private TextBox txtCompanySearch;
    private PanelEx panelExCompanySearch;
    private PanelEx panelExSearchControls;
    private Dictionary<string, LoanInfo.Right> hashRights = new Dictionary<string, LoanInfo.Right>();

    private bool isExternalOrganization
    {
      get => this.isCompanySearchAvailable && this.cboCompany.SelectedItem.Equals((object) "TPO");
    }

    private string selectedExternalOrgID
    {
      get => !this.isExternalOrganization ? (string) null : this.txtCompanySearch.Tag.ToString();
    }

    public BatchLoanReassignmentForm(Sessions.Session session, SetUpContainer setupContainer)
    {
      this.session = session;
      this.setupContainer = setupContainer;
      this.iorgMgrObj = session.OrganizationManager;
      this.InitializeComponent();
      this.workflowMgr = (WorkflowManager) session.BPM.GetBpmManager(BpmCategory.Workflow);
      this.resetCompanySearch();
      this.resetLoanFolderCombo();
      this.ResetRoleInfoArray();
      this.ResetSearchJobFunctionCombo();
      this.ResetAssignToJobFunctionCombo();
      this.InitGridWithPagination();
      this.listvw_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    private void resetCompanySearch()
    {
      this.isCompanySearchAvailable = this.panelExCompanySearch.Visible = this.session.EncompassEdition == EncompassEdition.Banker;
      this.resetCompanySearchCombo();
    }

    private void resetCompanySearchCombo()
    {
      if (!this.isCompanySearchAvailable)
        return;
      this.cboCompany.SelectedIndex = 0;
      this.setCompanySearchControls();
    }

    private void resetLoanFolderCombo()
    {
      LoanFolderInfo[] allLoanFolderInfos = this.session.LoanManager.GetAllLoanFolderInfos(false);
      this.comBoxLoanFolder.Items.Clear();
      this.comBoxLoanFolder.Items.Add((object) new LoanFolderInfo(SystemSettings.AllFolders));
      this.comBoxLoanFolder.Items.AddRange((object[]) allLoanFolderInfos);
      this.comBoxLoanFolder.SelectedIndex = -1;
      for (int index = 1; index < this.comBoxLoanFolder.Items.Count; ++index)
      {
        if (string.Compare(((LoanFolderInfo) this.comBoxLoanFolder.Items[index]).Name, this.session.UserInfo.WorkingFolder ?? "", StringComparison.OrdinalIgnoreCase) == 0)
        {
          this.comBoxLoanFolder.SelectedIndex = index;
          break;
        }
      }
      if (this.comBoxLoanFolder.SelectedIndex >= 0 || allLoanFolderInfos.Length == 0)
        return;
      this.comBoxLoanFolder.SelectedIndex = 0;
      for (int index = 1; index < this.comBoxLoanFolder.Items.Count; ++index)
      {
        if (((LoanFolderInfo) this.comBoxLoanFolder.Items[index]).Type == LoanFolderInfo.LoanFolderType.Regular)
        {
          this.comBoxLoanFolder.SelectedIndex = index;
          break;
        }
      }
    }

    private void ResetAssignToJobFunctionCombo()
    {
      this.comBoxAssignToJobFunction.Items.Clear();
      foreach (object role in this.roleList)
        this.comBoxAssignToJobFunction.Items.Add(role);
      if (this.comBoxAssignToJobFunction.Items.Count <= 0)
        return;
      this.comBoxAssignToJobFunction.SelectedIndex = 0;
    }

    private void ResetSearchJobFunctionCombo()
    {
      this.comBoxSearchJobFunction.Items.Clear();
      this.comBoxSearchJobFunction.Items.Add((object) new RoleSummaryInfo("Loan Team Member", "LTM", false));
      foreach (object role in this.roleList)
        this.comBoxSearchJobFunction.Items.Add(role);
      if (this.comBoxSearchJobFunction.Items.Count <= 0)
        return;
      this.comBoxSearchJobFunction.SelectedIndex = 0;
    }

    private void ResetSelectedUser()
    {
      this.currentselectedUser = (UserInfo) null;
      this.txtSelectedUser.Text = "";
    }

    private void ResetLoanSearchResult()
    {
      this.listvwLoan.Items.Clear();
      this.listvwLoan.ClearSort();
    }

    private void ResetRoleInfoArray()
    {
      try
      {
        this.roleList = this.workflowMgr.GetAllRoleFunctions();
      }
      catch (Exception ex)
      {
        Tracing.Log(BatchLoanReassignmentForm.sw, TraceLevel.Error, this.className, "ResetJobFunctionCombo: can't get all roles. Error: " + ex.Message);
      }
    }

    private bool ValidateRequiredSearchCriteria()
    {
      bool flag = true;
      if (this.comBoxSearchJobFunction.SelectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a job function.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        flag = false;
      }
      else if (this.comBoxLoanFolder.SelectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a loan folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        flag = false;
      }
      return flag;
    }

    private bool ValidatePriorReassignment()
    {
      bool flag = true;
      if (this.listvwLoan.SelectedItems == null || this.listvwLoan.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select at least one loan.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        flag = false;
      }
      else if (this.listvwUser.SelectedItems == null || this.listvwUser.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a user.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        flag = false;
      }
      return flag;
    }

    private bool ValidatePriorAllReassignment()
    {
      bool flag = true;
      if (this.listvwUser.SelectedItems == null || this.listvwUser.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a user.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        flag = false;
      }
      return flag;
    }

    private void ResetUserList()
    {
      ArrayList arrayList = new ArrayList();
      foreach (UserInfo userInfo in this.session.OrganizationManager.GetScopedUsersWithRole(((RoleSummaryInfo) this.comBoxAssignToJobFunction.SelectedItem).ID))
      {
        if (!arrayList.Contains((object) userInfo))
          arrayList.Add((object) userInfo);
      }
      this.listvwUser.Items.Clear();
      this.listvwUser.BeginUpdate();
      for (int index = 0; index < arrayList.Count; ++index)
        this.listvwUser.Items.Add(new GVItem(((UserInfo) arrayList[index]).Userid)
        {
          Tag = arrayList[index],
          SubItems = {
            (object) ((UserInfo) arrayList[index]).FirstName,
            (object) ((UserInfo) arrayList[index]).LastName
          }
        });
      this.listvwUser.EndUpdate();
    }

    private void InitGridWithPagination()
    {
      if (!this.RetrievePipelineData())
        return;
      this.DisplayCurrentPage(false, 0);
    }

    private void DisplayCurrentPage(bool preserveSelection, int sqlRead)
    {
      this.listvwLoan.BeginUpdate();
      int currentPageItemIndex = this.pageListNavigator1.CurrentPageItemIndex;
      int currentPageItemCount = this.pageListNavigator1.CurrentPageItemCount;
      PipelineInfo[] pinfosReassignment = new PipelineInfo[0];
      if (currentPageItemCount > 0)
        pinfosReassignment = (PipelineInfo[]) this._cursor.GetItems(currentPageItemIndex, currentPageItemCount, false, sqlRead);
      this.SetLoanRights(pinfosReassignment);
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      if (preserveSelection)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.listvwLoan.Items)
        {
          if (gvItem.Selected && gvItem.Tag != null)
            dictionary[((PipelineInfo) gvItem.Tag).GUID] = true;
        }
      }
      this.listvwLoan.Items.Clear();
      for (int index = 0; index < pinfosReassignment.Length; ++index)
      {
        GVItem itemForPipelineInfo = this.createGVItemForPipelineInfo(pinfosReassignment[index]);
        this.listvwLoan.Items.Add(itemForPipelineInfo);
        if (dictionary.ContainsKey(pinfosReassignment[index].GUID))
          itemForPipelineInfo.Selected = true;
      }
      if (this.listvwLoan.Items.Count > 0 && this.listvwLoan.SelectedItems.Count == 0)
        this.listvwLoan.Items[0].Selected = true;
      this.listvwLoan.EndUpdate();
    }

    private void SetLoanRights(PipelineInfo[] pinfosReassignment)
    {
      string[] guid = this.pinfoToGuid(pinfosReassignment);
      if (!(pinfosReassignment.Length != 0 & !this.session.UserInfo.IsSuperAdministrator()))
        return;
      LoanInfo.Right[] loanAccessRights = this.session.LoanManager.GetEffectiveLoanAccessRights(guid);
      for (int index = 0; index < pinfosReassignment.Length; ++index)
      {
        if (!this.hashRights.ContainsKey(pinfosReassignment[index].GUID))
          this.hashRights.Add(pinfosReassignment[index].GUID, loanAccessRights[index]);
      }
    }

    private GVItem createGVItemForPipelineInfo(PipelineInfo pInfo)
    {
      GVItem itemForPipelineInfo = new GVItem();
      Hashtable info = pInfo.Info;
      itemForPipelineInfo.SubItems.Add((object) pInfo.LoanNumber);
      itemForPipelineInfo.SubItems.Add((object) ((string) info[(object) "BorrowerFirstName"] + " " + (string) info[(object) "BorrowerLastName"]));
      itemForPipelineInfo.SubItems.Add((object) (string) info[(object) "Address1"]);
      double num = Utils.ParseDouble(info[(object) "LoanAmount"]);
      if (num != 0.0)
        itemForPipelineInfo.SubItems.Add((object) num.ToString("N2"));
      else
        itemForPipelineInfo.SubItems.Add((object) "");
      itemForPipelineInfo.SubItems.Add((object) (string) info[(object) "LoanType"]);
      itemForPipelineInfo.SubItems.Add((object) (string) info[(object) "LoanPurpose"]);
      itemForPipelineInfo.SubItems.Add((object) ((string) info[(object) "CoBorrowerFirstName"] + " " + (string) info[(object) "CoBorrowerLastName"]));
      itemForPipelineInfo.SubItems.Add((object) (string) info[(object) "LoanName"]);
      itemForPipelineInfo.SubItems.Add((object) pInfo.GUID);
      itemForPipelineInfo.Tag = (object) pInfo;
      this.DimNonAccessibleItems(itemForPipelineInfo, pInfo);
      return itemForPipelineInfo;
    }

    private void DimNonAccessibleItems(GVItem item, PipelineInfo pinfo)
    {
      if (this.session.UserInfo.IsSuperAdministrator())
        return;
      try
      {
        if (LoanInfo.Right.FullRight == this.hashRights[pinfo.GUID] || LoanInfo.Right.Access == this.hashRights[pinfo.GUID])
          return;
        item.BackColor = EncompassColors.Secondary2;
        item.ForeColor = EncompassColors.Secondary5;
      }
      catch
      {
      }
      if (!this.checkBox1.Checked)
        return;
      item.Selected = true;
    }

    private SortField[] getCurrentSortFields()
    {
      return this.getSortFieldsForColumnSort(this.listvwLoan.Columns.GetSortOrder());
    }

    private bool RetrievePipelineData(SortField[] sortFields = null)
    {
      string[] fields = new string[10]
      {
        "LoanNumber",
        "BorrowerFirstName",
        "BorrowerLastName",
        "Address1",
        "LoanAmount",
        "LoanType",
        "LoanPurpose",
        "CoBorrowerFirstName",
        "CoBorrowerLastName",
        "LoanName"
      };
      try
      {
        if (sortFields == null)
          sortFields = this.getCurrentSortFields();
        if (this._cursor != null)
        {
          try
          {
            this._cursor.Dispose();
          }
          catch
          {
          }
          this._cursor = (ICursor) null;
        }
        string userID = "";
        if (this.currentselectedUser != (UserInfo) null)
          userID = this.currentselectedUser.Userid;
        this._cursor = this.session.LoanManager.OpenPipelineForReassignment(((RoleSummaryInfo) this.comBoxSearchJobFunction.SelectedItem).RoleName == "Loan Team Member" ? "" : ((RoleSummaryInfo) this.comBoxSearchJobFunction.SelectedItem).RoleID.ToString(), userID, ((LoanFolderInfo) this.comBoxLoanFolder.SelectedItem).Name == SystemSettings.AllFolders ? "" : ((LoanFolderInfo) this.comBoxLoanFolder.SelectedItem).Name, fields, PipelineData.Lock | PipelineData.Milestones, (QueryCriterion) null, sortFields, this.isExternalOrganization, this.selectedExternalOrgID, sqlRead: 1);
        this.pageListNavigator1.NumberOfItems = this._cursor.GetItemCount();
        return true;
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        return false;
      }
    }

    private string[] pinfoToGuid(PipelineInfo[] pinfos)
    {
      ArrayList arrayList = new ArrayList(pinfos.Length);
      for (int index = 0; index < pinfos.Length; ++index)
        arrayList.Add((object) pinfos[index].GUID);
      return (string[]) arrayList.ToArray(typeof (string));
    }

    private DialogResult loanReassignment(object state, IProgressReportFeedback feedback)
    {
      try
      {
        ArrayList arrayList = new ArrayList();
        int roleID = -9999;
        string[] strArray = (string[]) null;
        string userID = "";
        Hashtable hashtable = (Hashtable) state;
        if (hashtable.ContainsKey((object) BatchLoanReassignmentForm.LoanList_HashKey))
          arrayList = (ArrayList) hashtable[(object) BatchLoanReassignmentForm.LoanList_HashKey];
        if (hashtable.ContainsKey((object) BatchLoanReassignmentForm.RoleID_HashKey))
          roleID = (int) hashtable[(object) BatchLoanReassignmentForm.RoleID_HashKey];
        if (hashtable.ContainsKey((object) BatchLoanReassignmentForm.MilestoneIDList_HashKey))
          strArray = (string[]) hashtable[(object) BatchLoanReassignmentForm.MilestoneIDList_HashKey];
        if (hashtable.ContainsKey((object) BatchLoanReassignmentForm.UserID_HashKey))
          userID = (string) hashtable[(object) BatchLoanReassignmentForm.UserID_HashKey];
        if (arrayList.Count > 0 && roleID > -9999 && userID != "")
        {
          for (int index = 0; index < arrayList.Count; ++index)
          {
            PipelineInfo plInfo = (PipelineInfo) arrayList[index];
            if (feedback.Cancel)
              return DialogResult.Cancel;
            this.reassign(index, plInfo, userID, roleID, feedback);
          }
        }
        feedback.ResetCounter(1);
        feedback.Increment(1);
        return DialogResult.OK;
      }
      catch
      {
        return DialogResult.Abort;
      }
    }

    private void reassign(
      int i,
      PipelineInfo plInfo,
      string userID,
      int roleID,
      IProgressReportFeedback feedback)
    {
      this.session.LoanManager.LoanReassign(i, plInfo, userID, roleID, feedback as IServerProgressFeedback);
    }

    private void PostLoanReassignmentSearch()
    {
      this.comBoxSearchJobFunction.SelectedIndex = this.comBoxAssignToJobFunction.SelectedIndex;
      this.currentselectedUser = (UserInfo) this.listvwUser.SelectedItems[0].Tag;
      this.txtSelectedUser.Text = string.Format("{0} {1}", (object) this.currentselectedUser.FirstName, (object) this.currentselectedUser.LastName);
      this.btnSearch_Click((object) null, (EventArgs) null);
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      this.toolTip1 = new ToolTip(this.components);
      this.btnReset = new StandardIconButton();
      this.stdIconBtnFind = new StandardIconButton();
      this.splitter1 = new Splitter();
      this.gcSelectLoans = new GroupContainer();
      this.gcLoans = new GroupContainer();
      this.pageListNavigator1 = new PageListNavigator();
      this.listvwLoan = new GridView();
      this.groupContainer1 = new GroupContainer();
      this.label4 = new Label();
      this.checkBox1 = new CheckBox();
      this.label3 = new Label();
      this.pnlExSelectLoans = new PanelEx();
      this.panelExSearchControls = new PanelEx();
      this.lblSearchJobFunction = new Label();
      this.comBoxSearchJobFunction = new ComboBox();
      this.lblUserName = new Label();
      this.btnSearch = new Button();
      this.comBoxLoanFolder = new ComboBox();
      this.txtSelectedUser = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.panelExCompanySearch = new PanelEx();
      this.lblCompany = new Label();
      this.stdicoCompanySearch = new StandardIconButton();
      this.cboCompany = new ComboBox();
      this.txtCompanySearch = new TextBox();
      this.pnlExRight = new PanelEx();
      this.listvwUser = new GridView();
      this.gcReassign = new GroupContainer();
      this.btnReassign = new Button();
      this.gcSelectRole = new GroupContainer();
      this.comBoxAssignToJobFunction = new ComboBox();
      ((ISupportInitialize) this.btnReset).BeginInit();
      ((ISupportInitialize) this.stdIconBtnFind).BeginInit();
      this.gcSelectLoans.SuspendLayout();
      this.gcLoans.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.pnlExSelectLoans.SuspendLayout();
      this.panelExSearchControls.SuspendLayout();
      this.panelExCompanySearch.SuspendLayout();
      ((ISupportInitialize) this.stdicoCompanySearch).BeginInit();
      this.pnlExRight.SuspendLayout();
      this.gcReassign.SuspendLayout();
      this.gcSelectRole.SuspendLayout();
      this.SuspendLayout();
      this.btnReset.BackColor = Color.Transparent;
      this.btnReset.Location = new Point(511, 28);
      this.btnReset.MouseDownImage = (Image) null;
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(16, 17);
      this.btnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnReset.TabIndex = 11;
      this.btnReset.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnReset, "Reset");
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.stdIconBtnFind.BackColor = Color.Transparent;
      this.stdIconBtnFind.Location = new Point(259, 28);
      this.stdIconBtnFind.MouseDownImage = (Image) null;
      this.stdIconBtnFind.Name = "stdIconBtnFind";
      this.stdIconBtnFind.Size = new Size(16, 17);
      this.stdIconBtnFind.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.stdIconBtnFind.TabIndex = 10;
      this.stdIconBtnFind.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnFind, "Find");
      this.stdIconBtnFind.Click += new EventHandler(this.btnPopupUserList_Click);
      this.splitter1.BackColor = Color.WhiteSmoke;
      this.splitter1.Dock = DockStyle.Right;
      this.splitter1.Location = new Point(818, 0);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(3, 459);
      this.splitter1.TabIndex = 13;
      this.splitter1.TabStop = false;
      this.gcSelectLoans.Controls.Add((Control) this.gcLoans);
      this.gcSelectLoans.Controls.Add((Control) this.groupContainer1);
      this.gcSelectLoans.Controls.Add((Control) this.pnlExSelectLoans);
      this.gcSelectLoans.Dock = DockStyle.Fill;
      this.gcSelectLoans.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gcSelectLoans.HeaderForeColor = SystemColors.ControlText;
      this.gcSelectLoans.Location = new Point(0, 0);
      this.gcSelectLoans.Name = "gcSelectLoans";
      this.gcSelectLoans.Size = new Size(818, 459);
      this.gcSelectLoans.TabIndex = 12;
      this.gcSelectLoans.Text = "1. Search for Loans";
      this.gcLoans.Borders = AnchorStyles.Top;
      this.gcLoans.Controls.Add((Control) this.pageListNavigator1);
      this.gcLoans.Controls.Add((Control) this.listvwLoan);
      this.gcLoans.Dock = DockStyle.Fill;
      this.gcLoans.HeaderForeColor = SystemColors.ControlText;
      this.gcLoans.Location = new Point(1, 174);
      this.gcLoans.Name = "gcLoans";
      this.gcLoans.Size = new Size(816, 284);
      this.gcLoans.TabIndex = 1;
      this.pageListNavigator1.BackColor = Color.Transparent;
      this.pageListNavigator1.Font = new Font("Arial", 8f);
      this.pageListNavigator1.Location = new Point(2, 2);
      this.pageListNavigator1.Name = "pageListNavigator1";
      this.pageListNavigator1.NumberOfItems = 0;
      this.pageListNavigator1.Size = new Size(254, 24);
      this.pageListNavigator1.TabIndex = 1;
      this.pageListNavigator1.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.pageListNavigator1_PageChangedEvent);
      this.listvwLoan.AllowColumnReorder = true;
      this.listvwLoan.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "LoanNumber";
      gvColumn1.Text = "Loan #";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "BorrowerLastName,BorrowerFirstName";
      gvColumn2.Tag = (object) "";
      gvColumn2.Text = "Borrower";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Address1";
      gvColumn3.Text = "Address";
      gvColumn3.Width = 70;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "LoanAmount";
      gvColumn4.Tag = (object) "Loan.LoanAmount";
      gvColumn4.Text = "Loan Amount";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 80;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "LoanType";
      gvColumn5.Text = "Loan Type";
      gvColumn5.Width = 63;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "LoanPurpose";
      gvColumn6.Text = "Loan Purpose";
      gvColumn6.Width = 77;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "CoBorrowerFirstName";
      gvColumn7.Text = "CoBorrower";
      gvColumn7.Width = 120;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "LoanName";
      gvColumn8.Text = "Loan Name";
      gvColumn8.Width = 120;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Guid";
      gvColumn9.Text = "GUID";
      gvColumn9.Width = 75;
      this.listvwLoan.Columns.AddRange(new GVColumn[9]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9
      });
      this.listvwLoan.Dock = DockStyle.Fill;
      this.listvwLoan.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.listvwLoan.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listvwLoan.Location = new Point(0, 26);
      this.listvwLoan.Name = "listvwLoan";
      this.listvwLoan.Size = new Size(816, 258);
      this.listvwLoan.SortOption = GVSortOption.Owner;
      this.listvwLoan.TabIndex = 0;
      this.listvwLoan.SelectedIndexChanged += new EventHandler(this.listvw_SelectedIndexChanged);
      this.listvwLoan.SortItems += new GVColumnSortEventHandler(this.listvwLoan_SortItems);
      this.groupContainer1.Borders = AnchorStyles.Top;
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.checkBox1);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(1, 86);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(816, 88);
      this.groupContainer1.TabIndex = 2;
      this.groupContainer1.Text = "2. Select Loans to Reassign";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(20, 38);
      this.label4.Name = "label4";
      this.label4.Size = new Size(363, 16);
      this.label4.TabIndex = 5;
      this.label4.Text = "You can reassign all loans or select specific loans below.";
      this.checkBox1.AutoSize = true;
      this.checkBox1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.checkBox1.Location = new Point(20, 60);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new Size(161, 20);
      this.checkBox1.TabIndex = 4;
      this.checkBox1.Text = "Reassign All Loans";
      this.checkBox1.UseVisualStyleBackColor = true;
      this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(447, 60);
      this.label3.Name = "label3";
      this.label3.Size = new Size(112, 16);
      this.label3.TabIndex = 3;
      this.label3.Text = "LoansSelected";
      this.pnlExSelectLoans.Controls.Add((Control) this.panelExSearchControls);
      this.pnlExSelectLoans.Controls.Add((Control) this.panelExCompanySearch);
      this.pnlExSelectLoans.Dock = DockStyle.Top;
      this.pnlExSelectLoans.Location = new Point(1, 26);
      this.pnlExSelectLoans.Name = "pnlExSelectLoans";
      this.pnlExSelectLoans.Size = new Size(816, 60);
      this.pnlExSelectLoans.TabIndex = 0;
      this.panelExSearchControls.Controls.Add((Control) this.lblSearchJobFunction);
      this.panelExSearchControls.Controls.Add((Control) this.comBoxSearchJobFunction);
      this.panelExSearchControls.Controls.Add((Control) this.btnReset);
      this.panelExSearchControls.Controls.Add((Control) this.lblUserName);
      this.panelExSearchControls.Controls.Add((Control) this.btnSearch);
      this.panelExSearchControls.Controls.Add((Control) this.stdIconBtnFind);
      this.panelExSearchControls.Controls.Add((Control) this.comBoxLoanFolder);
      this.panelExSearchControls.Controls.Add((Control) this.txtSelectedUser);
      this.panelExSearchControls.Controls.Add((Control) this.label1);
      this.panelExSearchControls.Controls.Add((Control) this.label2);
      this.panelExSearchControls.Dock = DockStyle.Fill;
      this.panelExSearchControls.Location = new Point(274, 0);
      this.panelExSearchControls.Name = "panelExSearchControls";
      this.panelExSearchControls.Size = new Size(542, 60);
      this.panelExSearchControls.TabIndex = 17;
      this.lblSearchJobFunction.Location = new Point(3, 9);
      this.lblSearchJobFunction.Name = "lblSearchJobFunction";
      this.lblSearchJobFunction.Size = new Size(36, 17);
      this.lblSearchJobFunction.TabIndex = 0;
      this.lblSearchJobFunction.Text = "Role";
      this.comBoxSearchJobFunction.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comBoxSearchJobFunction.Location = new Point(7, 26);
      this.comBoxSearchJobFunction.Name = "comBoxSearchJobFunction";
      this.comBoxSearchJobFunction.Size = new Size((int) sbyte.MaxValue, 24);
      this.comBoxSearchJobFunction.TabIndex = 1;
      this.comBoxSearchJobFunction.SelectedIndexChanged += new EventHandler(this.comBoxSearchJobFunction_SelectedIndexChanged);
      this.lblUserName.Location = new Point(137, 9);
      this.lblUserName.Name = "lblUserName";
      this.lblUserName.Size = new Size(40, 17);
      this.lblUserName.TabIndex = 2;
      this.lblUserName.Text = "Name";
      this.btnSearch.BackColor = SystemColors.Control;
      this.btnSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnSearch.Location = new Point(447, 25);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(60, 22);
      this.btnSearch.TabIndex = 8;
      this.btnSearch.Text = "Search";
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      this.comBoxLoanFolder.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comBoxLoanFolder.Location = new Point(298, 26);
      this.comBoxLoanFolder.Name = "comBoxLoanFolder";
      this.comBoxLoanFolder.Size = new Size(145, 24);
      this.comBoxLoanFolder.TabIndex = 6;
      this.txtSelectedUser.BackColor = SystemColors.Window;
      this.txtSelectedUser.Enabled = false;
      this.txtSelectedUser.Location = new Point(140, 26);
      this.txtSelectedUser.Name = "txtSelectedUser";
      this.txtSelectedUser.ReadOnly = true;
      this.txtSelectedUser.Size = new Size(116, 23);
      this.txtSelectedUser.TabIndex = 3;
      this.label1.Location = new Point(294, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(72, 17);
      this.label1.TabIndex = 4;
      this.label1.Text = "Loan Folder";
      this.label2.Location = new Point(278, 31);
      this.label2.Name = "label2";
      this.label2.Size = new Size(16, 17);
      this.label2.TabIndex = 5;
      this.label2.Text = "In";
      this.panelExCompanySearch.Controls.Add((Control) this.lblCompany);
      this.panelExCompanySearch.Controls.Add((Control) this.stdicoCompanySearch);
      this.panelExCompanySearch.Controls.Add((Control) this.cboCompany);
      this.panelExCompanySearch.Controls.Add((Control) this.txtCompanySearch);
      this.panelExCompanySearch.Dock = DockStyle.Left;
      this.panelExCompanySearch.Location = new Point(0, 0);
      this.panelExCompanySearch.Name = "panelExCompanySearch";
      this.panelExCompanySearch.Size = new Size(274, 60);
      this.panelExCompanySearch.TabIndex = 16;
      this.lblCompany.AutoSize = true;
      this.lblCompany.Location = new Point(2, 9);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new Size(68, 16);
      this.lblCompany.TabIndex = 12;
      this.lblCompany.Text = "Company";
      this.stdicoCompanySearch.BackColor = Color.Transparent;
      this.stdicoCompanySearch.Location = new Point(252, 28);
      this.stdicoCompanySearch.MouseDownImage = (Image) null;
      this.stdicoCompanySearch.Name = "stdicoCompanySearch";
      this.stdicoCompanySearch.Size = new Size(16, 16);
      this.stdicoCompanySearch.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.stdicoCompanySearch.TabIndex = 15;
      this.stdicoCompanySearch.TabStop = false;
      this.stdicoCompanySearch.Visible = false;
      this.stdicoCompanySearch.Click += new EventHandler(this.stdicoCompanySearch_Click);
      this.cboCompany.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCompany.Items.AddRange(new object[2]
      {
        (object) "Internal Organization",
        (object) "TPO"
      });
      this.cboCompany.Location = new Point(6, 26);
      this.cboCompany.Name = "cboCompany";
      this.cboCompany.Size = new Size((int) sbyte.MaxValue, 24);
      this.cboCompany.TabIndex = 13;
      this.cboCompany.SelectedIndexChanged += new EventHandler(this.cboCompany_SelectedIndexChanged);
      this.txtCompanySearch.Location = new Point(138, 26);
      this.txtCompanySearch.Name = "txtCompanySearch";
      this.txtCompanySearch.ReadOnly = true;
      this.txtCompanySearch.Size = new Size(111, 23);
      this.txtCompanySearch.TabIndex = 14;
      this.txtCompanySearch.Visible = false;
      this.pnlExRight.Controls.Add((Control) this.listvwUser);
      this.pnlExRight.Controls.Add((Control) this.gcReassign);
      this.pnlExRight.Controls.Add((Control) this.gcSelectRole);
      this.pnlExRight.Dock = DockStyle.Right;
      this.pnlExRight.Location = new Point(821, 0);
      this.pnlExRight.Name = "pnlExRight";
      this.pnlExRight.Size = new Size(254, 459);
      this.pnlExRight.TabIndex = 10;
      this.listvwUser.AllowColumnReorder = true;
      this.listvwUser.AllowMultiselect = false;
      this.listvwUser.BorderColor = Color.DarkGray;
      this.listvwUser.BorderStyle = BorderStyle.FixedSingle;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column1";
      gvColumn10.Text = "User ID";
      gvColumn10.Width = 100;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column2";
      gvColumn11.Text = "First Name";
      gvColumn11.Width = 80;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column3";
      gvColumn12.Text = "Last Name";
      gvColumn12.Width = 100;
      this.listvwUser.Columns.AddRange(new GVColumn[3]
      {
        gvColumn10,
        gvColumn11,
        gvColumn12
      });
      this.listvwUser.Dock = DockStyle.Fill;
      this.listvwUser.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.listvwUser.ForeColor = SystemColors.ControlText;
      this.listvwUser.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listvwUser.Location = new Point(0, 73);
      this.listvwUser.Name = "listvwUser";
      this.listvwUser.Size = new Size(254, 284);
      this.listvwUser.TabIndex = 1;
      this.listvwUser.SelectedIndexChanged += new EventHandler(this.listvw_SelectedIndexChanged);
      this.gcReassign.Controls.Add((Control) this.btnReassign);
      this.gcReassign.Dock = DockStyle.Bottom;
      this.gcReassign.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gcReassign.HeaderForeColor = SystemColors.ControlText;
      this.gcReassign.Location = new Point(0, 357);
      this.gcReassign.Name = "gcReassign";
      this.gcReassign.Size = new Size(254, 102);
      this.gcReassign.TabIndex = 1;
      this.gcReassign.Text = "4. Complete Assignment";
      this.btnReassign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReassign.BackColor = SystemColors.Control;
      this.btnReassign.Location = new Point(73, 43);
      this.btnReassign.Name = "btnReassign";
      this.btnReassign.Size = new Size(121, 22);
      this.btnReassign.TabIndex = 2;
      this.btnReassign.Text = "Reassign Loans";
      this.btnReassign.UseVisualStyleBackColor = true;
      this.btnReassign.Click += new EventHandler(this.btnReassign_Click);
      this.gcSelectRole.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcSelectRole.Controls.Add((Control) this.comBoxAssignToJobFunction);
      this.gcSelectRole.Dock = DockStyle.Top;
      this.gcSelectRole.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gcSelectRole.HeaderForeColor = SystemColors.ControlText;
      this.gcSelectRole.Location = new Point(0, 0);
      this.gcSelectRole.Name = "gcSelectRole";
      this.gcSelectRole.Size = new Size(254, 73);
      this.gcSelectRole.TabIndex = 0;
      this.gcSelectRole.Text = "3. Select the Assignee";
      this.comBoxAssignToJobFunction.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comBoxAssignToJobFunction.Location = new Point(10, 39);
      this.comBoxAssignToJobFunction.Name = "comBoxAssignToJobFunction";
      this.comBoxAssignToJobFunction.Size = new Size(200, 24);
      this.comBoxAssignToJobFunction.TabIndex = 8;
      this.comBoxAssignToJobFunction.SelectedIndexChanged += new EventHandler(this.comBoxAssignToJobFunction_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcSelectLoans);
      this.Controls.Add((Control) this.splitter1);
      this.Controls.Add((Control) this.pnlExRight);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (BatchLoanReassignmentForm);
      this.Size = new Size(1075, 459);
      ((ISupportInitialize) this.btnReset).EndInit();
      ((ISupportInitialize) this.stdIconBtnFind).EndInit();
      this.gcSelectLoans.ResumeLayout(false);
      this.gcLoans.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.pnlExSelectLoans.ResumeLayout(false);
      this.panelExSearchControls.ResumeLayout(false);
      this.panelExSearchControls.PerformLayout();
      this.panelExCompanySearch.ResumeLayout(false);
      this.panelExCompanySearch.PerformLayout();
      ((ISupportInitialize) this.stdicoCompanySearch).EndInit();
      this.pnlExRight.ResumeLayout(false);
      this.gcReassign.ResumeLayout(false);
      this.gcSelectRole.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void btnPopupUserList_Click(object sender, EventArgs e)
    {
      RoleSummaryInfo selectedItem = (RoleSummaryInfo) this.comBoxSearchJobFunction.SelectedItem;
      ContactAssignment contactAssignment = selectedItem.Name == "Loan Team Member" ? new ContactAssignment(this.session, "") : new ContactAssignment(this.session, (RoleInfo) selectedItem, "", true);
      if (contactAssignment.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.currentselectedUser = contactAssignment.SelectedUser;
      if (!(this.currentselectedUser != (UserInfo) null))
        return;
      this.txtSelectedUser.Text = string.Format("{0} {1}", (object) this.currentselectedUser.FirstName, (object) this.currentselectedUser.LastName);
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      using (CursorActivator.Wait())
      {
        this.resetCompanySearchCombo();
        this.ResetSearchJobFunctionCombo();
        this.ResetSelectedUser();
        this.ResetRoleInfoArray();
        this.resetLoanFolderCombo();
        this.ResetLoanSearchResult();
        this.DisplayCurrentPage(false, 0);
      }
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      if (!this.ValidateRequiredSearchCriteria())
        return;
      this.InitGridWithPagination();
    }

    private void comBoxAssignToJobFunction_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.comBoxAssignToJobFunction.SelectedItem == null)
        return;
      this.ResetUserList();
    }

    private void btnReassign_Click(object sender, EventArgs e)
    {
      if (this.checkBox1.Checked)
      {
        this.ReassignAllClicked();
      }
      else
      {
        if (!this.ValidatePriorReassignment())
          return;
        Hashtable state = new Hashtable();
        this.setupContainer.ButtonSaveEnabled = false;
        string[] displayList = new string[this.listvwLoan.SelectedItems.Count];
        ArrayList arrayList = new ArrayList();
        for (int index = 0; index < this.listvwLoan.SelectedItems.Count; ++index)
        {
          arrayList.Add(this.listvwLoan.SelectedItems[index].Tag);
          displayList[index] = ((PipelineInfo) this.listvwLoan.SelectedItems[index].Tag).LoanNumber;
        }
        state.Add((object) BatchLoanReassignmentForm.LoanList_HashKey, (object) arrayList);
        int roleId = ((RoleSummaryInfo) this.comBoxAssignToJobFunction.SelectedItem).RoleID;
        if (roleId != -9999)
          state.Add((object) BatchLoanReassignmentForm.RoleID_HashKey, (object) roleId);
        string userid = ((UserInfo) this.listvwUser.SelectedItems[0].Tag).Userid;
        state.Add((object) BatchLoanReassignmentForm.UserID_HashKey, (object) userid);
        using (ProgressReportDialog progressReportDialog = new ProgressReportDialog("Batch Loan Reassignment Process", new AsynchronousProcessReport(this.loanReassignment), (object) state, true, displayList))
          progressReportDialog.ShowDialog();
        this.PostLoanReassignmentSearch();
        this.setupContainer.ButtonSaveEnabled = true;
      }
    }

    private void ReassignAllClicked()
    {
      if (!this.ValidatePriorAllReassignment())
        return;
      if (MessageBox.Show("Are you sure you want to reassign all assignable loans to " + ((UserInfo) this.listvwUser.SelectedItems[0].Tag).Userid + "?", "Loan Reassignment", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      Cursor.Current = Cursors.WaitCursor;
      string[] fields = new string[10]
      {
        "LoanNumber",
        "BorrowerFirstName",
        "BorrowerLastName",
        "Address1",
        "LoanAmount",
        "LoanType",
        "LoanPurpose",
        "CoBorrowerFirstName",
        "CoBorrowerLastName",
        "LoanName"
      };
      this.suspendEvents = true;
      string userID = "";
      if (this.currentselectedUser != (UserInfo) null)
        userID = this.currentselectedUser.Userid;
      string roleID = !(((RoleSummaryInfo) this.comBoxSearchJobFunction.SelectedItem).RoleName == "Loan Team Member") ? ((RoleSummaryInfo) this.comBoxSearchJobFunction.SelectedItem).RoleID.ToString() : "";
      ICursor loanCursor = !(((LoanFolderInfo) this.comBoxLoanFolder.SelectedItem).Name == SystemSettings.AllFolders) ? this.session.LoanManager.OpenPipelineForReassignment(roleID, userID, ((LoanFolderInfo) this.comBoxLoanFolder.SelectedItem).Name, fields, PipelineData.Lock | PipelineData.Milestones, this.isExternalOrganization, this.selectedExternalOrgID, true, 1) : this.session.LoanManager.OpenPipelineForReassignment(roleID, userID, "", fields, PipelineData.Lock | PipelineData.Milestones, this.isExternalOrganization, this.selectedExternalOrgID, true, 1);
      int itemCount = loanCursor.GetItemCount();
      Cursor.Current = Cursors.Default;
      if (itemCount > 0)
      {
        this.ReassignAllLoans(loanCursor, itemCount);
      }
      else
      {
        int num = (int) MessageBox.Show("No Reassignable loans");
      }
    }

    private void ReassignAllLoans(ICursor loanCursor, int itemCount)
    {
      PipelineInfo[] items = (PipelineInfo[]) loanCursor.GetItems(0, itemCount, false);
      Hashtable state = new Hashtable();
      this.setupContainer.ButtonSaveEnabled = false;
      string[] displayList = new string[items.Length];
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < items.Length; ++index)
        displayList[index] = items[index].LoanNumber;
      arrayList.AddRange((ICollection) items);
      state.Add((object) BatchLoanReassignmentForm.LoanList_HashKey, (object) arrayList);
      int roleId = ((RoleSummaryInfo) this.comBoxAssignToJobFunction.SelectedItem).RoleID;
      if (roleId != -9999)
        state.Add((object) BatchLoanReassignmentForm.RoleID_HashKey, (object) roleId);
      string userid = ((UserInfo) this.listvwUser.SelectedItems[0].Tag).Userid;
      state.Add((object) BatchLoanReassignmentForm.UserID_HashKey, (object) userid);
      using (ProgressReportDialog progressReportDialog = new ProgressReportDialog("Batch Loan Reassignment Process", new AsynchronousProcessReport(this.loanReassignment), (object) state, true, displayList))
        progressReportDialog.ShowDialog();
      this.PostLoanReassignmentSearch();
      this.setupContainer.ButtonSaveEnabled = true;
    }

    private void comBoxSearchJobFunction_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.ResetSelectedUser();
    }

    private void listvw_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnReassign.Enabled = this.listvwLoan.SelectedItems.Count > 0 && this.listvwUser.SelectedItems.Count > 0;
      if (this.checkBox1.Checked)
        this.label3.Text = this._cursor.GetItemCount().ToString() + " Loans Selected";
      else
        this.label3.Text = this.listvwLoan.SelectedItems.Count.ToString() + " Loans Selected";
    }

    private void pageListNavigator1_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      using (CursorActivator.Wait())
        this.DisplayCurrentPage(false, 1);
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
      if (this.checkBox1.Checked)
      {
        this.label3.Text = this._cursor.GetItemCount().ToString() + " Loans Selected";
        this.listvwLoan.Items.SelectAll();
      }
      else
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.listvwLoan.Items)
          gvItem.Selected = false;
        this.label3.Text = this.listvwLoan.SelectedItems.Count.ToString() + " Loans Selected";
      }
    }

    private void cboCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setCompanySearchControls();
    }

    private void setCompanySearchControls()
    {
      if (this.suspendEvents)
        return;
      if (this.cboCompany.SelectedItem.Equals((object) "TPO"))
      {
        if (this.externalOrgsList == null)
          this.loadTPOSettings();
        if (this.txtCompanySearch.Text.Equals(""))
        {
          this.txtCompanySearch.Text = "All";
          this.txtCompanySearch.Tag = (object) "-1";
        }
        this.txtCompanySearch.Visible = true;
        this.stdicoCompanySearch.Visible = true;
        this.panelExCompanySearch.Width = 274;
      }
      else
      {
        this.txtCompanySearch.Text = string.Empty;
        this.txtCompanySearch.Tag = (object) string.Empty;
        this.txtCompanySearch.Visible = false;
        this.stdicoCompanySearch.Visible = false;
        this.panelExCompanySearch.Width = 132;
      }
    }

    private void stdicoCompanySearch_Click(object sender, EventArgs e)
    {
      if (this.externalOrgsList == null)
        this.loadTPOSettings();
      if (this.externalOrgsList.Count > 0)
      {
        this.pipeLineExtOrgInfo = new PipeLineExtOrgInfo(this.externalOrgsList);
        this.pipeLineExtOrgInfo.StartPosition = FormStartPosition.CenterParent;
        if (this.pipeLineExtOrgInfo.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          this.selectedOrg = this.pipeLineExtOrgInfo.selectedOrg;
          if (this.selectedOrg != null)
          {
            this.txtCompanySearch.Text = this.selectedOrg.OrganizationName;
            this.txtCompanySearch.Tag = (object) this.selectedOrg.ExternalID;
          }
        }
        this.pipeLineExtOrgInfo.Close();
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "No External Org found!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void loadTPOSettings()
    {
      bool flag1 = false;
      bool flag2 = false;
      if (this.session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.ExternalSettings_ContactSalesRep) && this.session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.ExternalSettings_OrganizationSettings))
        flag1 = !(bool) this.session.StartupInfo.UserAclFeatureRights[(object) AclFeature.ExternalSettings_ContactSalesRep] && (bool) this.session.StartupInfo.UserAclFeatureRights[(object) AclFeature.ExternalSettings_OrganizationSettings];
      AclGroup[] groupsOfUser = this.session.AclGroupManager.GetGroupsOfUser(this.session.UserID);
      if (groupsOfUser != null && groupsOfUser.Length != 0)
        flag2 = ((IEnumerable<AclGroup>) groupsOfUser).Any<AclGroup>((Func<AclGroup, bool>) (group => group.Name.ToLower() == "TPO Admins".ToLower()));
      if (this.session.UserInfo.IsAdministrator() | flag1 | flag2)
        this.externalOrgsList = this.session.ConfigurationManager.GetAllExternalParentOrganizations(false);
      else
        this.externalOrgsList = (List<ExternalOriginatorManagementData>) this.session.ConfigurationManager.GetExternalAndInternalUserAndOrgBySalesRep(this.session.UserID, this.session.UserInfo.OrgId)[1];
    }

    private void listvwLoan_SortItems(object source, GVColumnSortEventArgs e)
    {
      using (CursorActivator.Wait())
      {
        if (!this.RetrievePipelineData(this.getSortFieldsForColumnSort(e.ColumnSorts)))
          return;
        this.DisplayCurrentPage(true, 1);
      }
    }

    private SortField[] getSortFieldsForColumnSort(GVColumnSort[] sortOrder)
    {
      List<SortField> sortFieldList = new List<SortField>();
      foreach (GVColumnSort gvColumnSort in sortOrder)
      {
        string name = this.listvwLoan.Columns[gvColumnSort.Column].Name;
        char[] chArray = new char[1]{ ',' };
        foreach (string str in name.Split(chArray))
        {
          SortField sortFieldForColumn = this.getSortFieldForColumn(str.Trim(), gvColumnSort.SortOrder);
          if (sortFieldForColumn != null)
            sortFieldList.Add(sortFieldForColumn);
        }
      }
      return sortFieldList.ToArray();
    }

    private SortField getSortFieldForColumn(string columnID, SortOrder sortOrder)
    {
      return new SortField(columnID, sortOrder == SortOrder.Ascending ? FieldSortOrder.Ascending : FieldSortOrder.Descending, DataConversion.None);
    }
  }
}

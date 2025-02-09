// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CompanySetupForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI.Import;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  [Serializable]
  public class CompanySetupForm : Form
  {
    private Sessions.Session session;
    private ToolTip hierarchyToolTip;
    private TreeNode currentSelectedTopNode;
    private string userid;
    private string selectedUserId;
    private List<ExternalUserInfo> selectedExternalUsers = new List<ExternalUserInfo>();
    private bool isTpoAdmin = true;
    private bool isAdmin = true;
    private string[] externalUsersIds;
    private List<ExternalOriginatorManagementData> externalRootOrgsList;
    private List<int> externalOrgsList;
    private bool hasOrganizationCreateRight = true;
    private bool hasOrganizationDeleteRight = true;
    private bool hasOrganizationEditRight = true;
    private bool hasContactDeleteRight = true;
    private bool hasContactEditRight = true;
    private bool hasContactExportRight = true;
    private bool hasTpoTreeViewRight = true;
    private bool hasTpoContactViewRight = true;
    private bool hasSendWelcomeEmailRight = true;
    private bool hasBankEditRight = true;
    private bool hasBankDeleteRight = true;
    private bool hasCompanyTabCreateRight = true;
    private const string lenderCompanyType = "Lender";
    private const string tpoCompanyType = "Tpo";
    private const string bankCompanyType = "Bank";
    private IOrganizationManager rOrg;
    private UserInfo[] allInternalUsers;
    private Hashtable orgLookup;
    private List<ExternalUserInfo> allLOLPUsers;
    private List<ExternalOriginatorManagementData> selectedCompanies = new List<ExternalOriginatorManagementData>();
    private List<TreeNode> searchNodeList = new List<TreeNode>();
    private int searchCounter;
    private bool isTPOMVP;
    private List<ExternalSettingValue> contactStatusSettings;
    private WorkflowManager workflowMgr;
    private Dictionary<int, string> hierarchyNodes = new Dictionary<int, string>();
    private Dictionary<int, string> nodesToRemove = new Dictionary<int, string>();
    private string currentHierarchyPath = string.Empty;
    private int currentDepth;
    private int currentParent;
    public TreeNode dragNode;
    private IContainer components;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnDeleteOrg;
    private StandardIconButton stdIconBtnEditOrg;
    private StandardIconButton stdIconBtnNewOrg;
    private GroupContainer gcOrg;
    private ExternalHierarchyTree hierarchyTree;
    private ContextMenu contextMenuOrg;
    private MenuItem addOrgMenuItem;
    private MenuItem editOrgMenuItem;
    private MenuItem delOrgMenuItem;
    private ImageList imgListTv;
    private TreeNode treeTopNodeBanks = new TreeNode("Banks", 0, 1);
    private TreeNode treeTopNodeBroker = new TreeNode("Third Party Originators", 0, 1);
    private TreeNode treeTopNodeLender = new TreeNode("Lenders", 0, 1);
    private GroupContainer grpContacts;
    private StandardIconButton btnExport;
    private StandardIconButton btnDeleteContact;
    private StandardIconButton btnEditContact;
    private StandardIconButton btnAddContact;
    private Panel panelBottom;
    private CollapsibleSplitter collapsibleSplitter1;
    private Panel panelTop;
    private GridView gridViewContacts;
    private StandardIconButton btnImportContact;
    private StandardIconButton btnImportOrg;
    private StandardIconButton stnIconMove;
    private MenuItem menuItem1;
    private StandardIconButton stdExport;
    private StandardIconButton emailButton;
    private MenuItem menuItem2;
    private GradientPanel gradientPanel1;
    private RadioButton idRd;
    private RadioButton nameRd;
    private Button clearBtn;
    private Button downBtn;
    private Button upBtn;
    private StandardIconButton searchBtn;
    private TextBox searchTxt;
    private Label label1;
    private Label searchCnt;
    private MenuItem exportLicensesMenuItem;
    private ContextMenuStrip contextMenuTPOContacts;
    private ToolStripMenuItem tpoContactExportLicencesMenuItem;
    private ToolStripMenuItem tpoEditContactMenuItem;
    private ToolStripMenuItem tpoMoveContactMenuItem;
    private ToolStripMenuItem tpoDeleteContactMenuItem;
    private ToolStripMenuItem tpoExportContactMenuItem;
    private ToolStripMenuItem tpoSendWelcomeEmailMenuItem;
    private Button btnAutoOrgNumbering;

    public CompanySetupForm(Sessions.Session session)
      : this(session, (string) null)
    {
    }

    public CompanySetupForm(Sessions.Session session, string userid)
    {
      this.session = session;
      this.workflowMgr = (WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow);
      this.isTPOMVP = session.ConfigurationManager.CheckIfAnyTPOSiteExists();
      this.InitializeComponent();
      this.treeTopNodeBroker.Tag = (object) new ExtNodeTag(0, "Third Parth Originators", "Tpo");
      this.treeTopNodeLender.Tag = (object) new ExtNodeTag(0, "Lenders", "Lender");
      this.treeTopNodeBanks.Tag = (object) new ExtNodeTag(0, "Banks", "Bank");
      this.hierarchyTree.SetSession(this.session);
      this.hierarchyTree.Nodes.Add(this.treeTopNodeBanks);
      this.hierarchyTree.Nodes.Add(this.treeTopNodeBroker);
      this.hierarchyTree.Nodes.Add(this.treeTopNodeLender);
      this.hierarchyTree.SelectedNode = this.treeTopNodeBroker;
      this.hierarchyToolTip = new ToolTip(this.components);
      this.hierarchyToolTip.AutoPopDelay = 3000;
      this.hierarchyToolTip.InitialDelay = 500;
      this.hierarchyToolTip.ReshowDelay = 100;
      this.hierarchyToolTip.ShowAlways = true;
      this.stdIconBtnEditOrg.Enabled = false;
      this.stdIconBtnDeleteOrg.Enabled = false;
      this.emailButton.Enabled = false;
      this.hierarchyTree.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(this.hierarchyTree_NodeMouseDoubleClick);
      this.hierarchyTree.AfterSelect += new TreeViewEventHandler(this.hierarchyTree_AfterSelect);
      this.hierarchyTree.ItemDrag += new ItemDragEventHandler(this.hierarchyTree_ItemDrag);
      this.hierarchyTree.DragDrop += new DragEventHandler(this.hierarchyTree_DragDrop);
      this.hierarchyTree.KeyDown += new KeyEventHandler(this.hierarchyTree_KeyDown);
      this.hierarchyTree.MouseDown += new MouseEventHandler(this.hierarchyTree_MouseDown);
      this.stnIconMove.Enabled = false;
      this.refreshUserIdsAndBrokerRootList();
      Session.clearTpoCache();
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.hierarchyTree.IsTpoAdmin = this.isAdmin = this.isTpoAdmin = Session.UserInfo.IsAdministrator() || aclManager.GetUserApplicationRight(AclFeature.SettingsTab_CompanyDetails);
      if (!this.isAdmin)
        this.hierarchyTree.IsTpoAdmin = this.isTpoAdmin = !aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_ContactSalesRep);
      this.hasBankDeleteRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_DeleteBanks);
      this.hasBankEditRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditBanks);
      this.hasContactDeleteRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_DeleteContacts);
      this.hasContactEditRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditContacts);
      this.hasContactExportRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_ExportContacts);
      this.hasCompanyTabCreateRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditCompanyInformation);
      this.hasOrganizationCreateRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_CreateOrganizations);
      this.hasOrganizationEditRight = TPOClientUtils.HasCompanyEditAccess();
      this.hasOrganizationDeleteRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_DeleteOrganizations);
      this.hasTpoTreeViewRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_OrganizationSettings);
      this.hasTpoContactViewRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_Contacts);
      this.hasSendWelcomeEmailRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_SendWelcomeEmail);
      this.initHierarchy();
      if (userid != null)
      {
        this.locateUserInOrganization(userid, false, "User '" + this.userid + "' doesn't exist. The user may have been deleted.");
        this.selectedUserId = userid;
      }
      if (Session.UserInfo.IsAdministrator())
        return;
      this.btnAutoOrgNumbering.Enabled = false;
    }

    private void initHierarchy()
    {
      Cursor.Current = Cursors.WaitCursor;
      List<HierarchySummary>[] allHierarchy = this.session.ConfigurationManager.GetAllHierarchy();
      List<HierarchySummary> list1 = allHierarchy == null || allHierarchy.Length != 3 ? (List<HierarchySummary>) null : allHierarchy[0];
      List<HierarchySummary> list2 = allHierarchy == null || allHierarchy.Length != 3 ? (List<HierarchySummary>) null : allHierarchy[1];
      List<HierarchySummary> list3 = allHierarchy == null || allHierarchy.Length != 3 ? (List<HierarchySummary>) null : allHierarchy[2];
      this.hierarchyTree.ExternalCompanyList = allHierarchy;
      this.hierarchyTree.BeginUpdate();
      if (list3 != null)
        this.buildBankHierarchyTree(this.treeTopNodeBanks, list3);
      if (list1 != null)
        this.buildLenderHierarchyTree(this.treeTopNodeLender, list1, 0, false);
      if (!this.hasTpoTreeViewRight)
        this.hierarchyTree.Nodes.Remove(this.treeTopNodeBroker);
      if (list2 != null && this.hasTpoTreeViewRight)
        this.buildHierarchyTree(list2, false);
      this.hierarchyTree.EndUpdate();
      this.hierarchyTree.SelectedNode = this.treeTopNodeLender;
      this.currentSelectedTopNode = this.treeTopNodeLender;
      Cursor.Current = Cursors.Default;
    }

    private void locateUserInOrganization(string userid, bool returnIfNotFound, string errMsg)
    {
      this.userid = userid;
      int oid;
      if (this.userid == null)
      {
        oid = this.session.UserInfo.OrgId;
      }
      else
      {
        ExternalUserInfo externalUserInfo = this.session.ConfigurationManager.GetExternalUserInfo(this.userid);
        if ((UserInfo) externalUserInfo == (UserInfo) null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, errMsg ?? "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
          if (returnIfNotFound)
            return;
          oid = 0;
        }
        else
          oid = externalUserInfo.ExternalOrgID;
      }
      this.currentSelectedTopNode = this.treeTopNodeBroker;
      this.hierarchyTree.ImageList = this.imgListTv;
      List<int> organizationParents = this.session.ConfigurationManager.GetExternalOrganizationParents(oid);
      this.selectNodeForOrg(this.currentSelectedTopNode, oid, organizationParents);
    }

    private void buildBankHierarchyTree(TreeNode parentNode, List<HierarchySummary> list)
    {
      if (list.Count == 0)
        return;
      this.hierarchyTree.SelectedNode = parentNode;
      int index = 0;
      for (; index < list.Count; ++index)
      {
        HierarchySummary hierarchySummary = list[index];
        this.hierarchyTree.SelectedNode.Nodes.Add(new TreeNode(hierarchySummary.OrganizationName, 0, 1)
        {
          Tag = (object) new ExtNodeTag(hierarchySummary.oid, hierarchySummary.OrganizationName, "Bank")
        });
      }
    }

    private void buildLenderHierarchyTree(
      TreeNode parentNode,
      List<HierarchySummary> list,
      int parentOID,
      bool access)
    {
      if (list.Count == 0)
        return;
      this.hierarchyTree.SelectedNode = parentNode;
      int index = 0;
      while (index < list.Count)
      {
        HierarchySummary hierarchySummary = list[index];
        if (hierarchySummary.Parent == parentOID)
        {
          TreeNode treeNode = new TreeNode(hierarchySummary.OrganizationName, 0, 1);
          treeNode.Tag = (object) new ExtNodeTag(hierarchySummary.oid, hierarchySummary.OrganizationName, "Lender");
          this.hierarchyTree.SelectedNode.Nodes.Add(treeNode);
          int oid = hierarchySummary.oid;
          list.Remove(hierarchySummary);
          this.buildLenderHierarchyTree(treeNode, list, oid, access);
          this.hierarchyTree.SelectedNode = parentNode;
          index = 0;
          access = false;
        }
        else
          ++index;
      }
    }

    private void buildHierarchyTree(List<HierarchySummary> list, bool access)
    {
      if (list.Count == 0)
        return;
      this.buildHierarchyTree(this.treeTopNodeBroker, list.Where<HierarchySummary>((Func<HierarchySummary, bool>) (x => x.Depth == 1)).ToList<HierarchySummary>(), false);
    }

    private void buildHierarchyTree(
      TreeNode parentNode,
      List<HierarchySummary> firstLevelList,
      bool access)
    {
      if (firstLevelList.Count == 0)
        return;
      TreeNode newTreeNode = (TreeNode) null;
      firstLevelList.ForEach((Action<HierarchySummary>) (company =>
      {
        if (this.externalRootOrgsList.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (e1 => e1.ExternalID.Equals(company.ExternalID))) == null && company.Parent == 0 && !this.isTpoAdmin)
          return;
        newTreeNode = new TreeNode(company.OrganizationName, 0, 1);
        ExtNodeTag extNodeTag = new ExtNodeTag(company.oid, company.OrganizationName, "Tpo");
        if (this.externalOrgsList.Contains(company.oid) | access || this.isTpoAdmin)
        {
          extNodeTag.AllowAccess = true;
          access = true;
          Session.AddTpoHierarchyAccessCache(company.oid, access);
        }
        newTreeNode.Tag = (object) extNodeTag;
        parentNode.Nodes.Add(newTreeNode);
        if (this.hierarchyTree.BrokerOrganizationList.FirstOrDefault<HierarchySummary>((Func<HierarchySummary, bool>) (x => x.Parent == company.oid)) != null)
          newTreeNode.Nodes.Add("<DUMMY NODE>");
        if (!this.externalOrgsList.Contains(company.oid))
          return;
        access = false;
      }));
    }

    private void addBranch(int oid, TreeNode selectedNode, string type)
    {
      this.hierarchyTree.BeginUpdate();
      List<HierarchySummary> hierarchySummaryList = new List<HierarchySummary>();
      foreach (HierarchySummary hierarchySummary in this.session.ConfigurationManager.GetHierarchy(this.currentSelectedTopNode == this.treeTopNodeLender, oid))
      {
        this.hierarchyTree.SelectedNode = selectedNode;
        TreeNode node = new TreeNode(hierarchySummary.OrganizationName, 0, 1);
        node.Tag = (object) new ExtNodeTag(hierarchySummary.oid, hierarchySummary.OrganizationName, type);
        this.hierarchyTree.SelectedNode.Nodes.Add(node);
        this.hierarchyTree.SelectedNode = node;
        this.addBranch(((ExtNodeTag) this.hierarchyTree.SelectedNode.Tag).Oid, this.hierarchyTree.SelectedNode, type);
      }
      this.hierarchyTree.EndUpdate();
    }

    private void findNodeToDelete(TreeNode root, int oid)
    {
      if (root == null)
        return;
      for (int index = 0; index < root.Nodes.Count && root.Nodes[index].Tag != null; ++index)
      {
        if (((ExtNodeTag) root.Nodes[index].Tag).Oid == oid)
        {
          root.Nodes[index].Remove();
          break;
        }
        this.findNodeToDelete(root.Nodes[index], oid);
      }
    }

    private void selectNodeForOrg(TreeNode root, int oid, List<int> parents)
    {
      if (root == null)
        return;
      for (int index = 0; index < root.Nodes.Count && (ExtNodeTag) root.Nodes[index].Tag != null; ++index)
      {
        if (((ExtNodeTag) root.Nodes[index].Tag).Oid == oid)
        {
          this.hierarchyTree.SelectedNode = root.Nodes[index];
          break;
        }
        if (parents.Contains(((ExtNodeTag) root.Nodes[index].Tag).Oid))
        {
          root.Expand();
          this.hierarchyTree.CallBeforeExpand(new TreeViewCancelEventArgs(root.Nodes[index], false, TreeViewAction.Expand));
        }
        this.selectNodeForOrg(root.Nodes[index], oid, parents);
      }
    }

    private void stdIconBtnNewOrg_Click(object sender, EventArgs e)
    {
      if (this.hierarchyTree.SelectedNode == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select the parent company in the hierarchy.");
      }
      else
      {
        int int32_1 = Convert.ToInt32(((ExtNodeTag) this.hierarchyTree.SelectedNode.Tag).Oid);
        int num2;
        string hierarchyPath;
        if (int32_1 != 0)
        {
          HierarchySummary orgDetails = this.session.ConfigurationManager.GetOrgDetails(this.currentSelectedTopNode == this.treeTopNodeLender, int32_1);
          num2 = orgDetails.Depth;
          hierarchyPath = orgDetails.HierarchyPath;
        }
        else
        {
          num2 = 0;
          string str = "";
          if (this.currentSelectedTopNode == this.treeTopNodeLender)
            str = "Lenders";
          else if (this.currentSelectedTopNode == this.treeTopNodeBroker)
            str = "Third Party Originators";
          else if (this.currentSelectedTopNode == this.treeTopNodeBanks)
            str = "Banks";
          hierarchyPath = str;
        }
        this.hierarchyNodes = new Dictionary<int, string>();
        this.nodesToRemove = new Dictionary<int, string>();
        this.currentHierarchyPath = hierarchyPath;
        this.currentDepth = num2;
        this.currentParent = int32_1;
        int depth = num2 + 1;
        int int32_2 = Convert.ToInt32(((ExtNodeTag) this.getCompanyNode(this.hierarchyTree.SelectedNode).Tag).Oid);
        string companyType = "";
        if (this.currentSelectedTopNode == this.treeTopNodeLender)
        {
          using (ManuallyAdd manuallyAdd = new ManuallyAdd(this.session, -1, int32_1, depth, hierarchyPath, false))
          {
            if (manuallyAdd.ShowDialog((IWin32Window) this) != DialogResult.OK)
              return;
            this.hierarchyNodes = manuallyAdd.HierarchyNodes;
            companyType = "Lender";
          }
        }
        else if (this.currentSelectedTopNode == this.treeTopNodeBanks)
        {
          using (BankSetupForm bankSetupForm = new BankSetupForm(this.session, -1, false))
          {
            if (bankSetupForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
              return;
            this.hierarchyNodes = bankSetupForm.HierarchyNodes;
            companyType = "Bank";
          }
        }
        else if (this.currentSelectedTopNode == this.treeTopNodeBroker)
        {
          if (!TPOClientUtils.HasCompanyTabAccess())
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "You currently don't have rights to view TPO information. Please contact your Encompass administrator for additional help.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
          using (EditCompanyDetailsDialog companyDetailsDialog = new EditCompanyDetailsDialog(this.session, -1, int32_1, int32_2, depth, hierarchyPath, false, false))
          {
            if (companyDetailsDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
              return;
            this.hierarchyNodes = companyDetailsDialog.HierarchyNodes;
            companyType = "Tpo";
            this.RefreshExternalUserInfoTree();
          }
        }
        this.hierarchyTree.BeginUpdate();
        foreach (KeyValuePair<int, string> hierarchyNode in this.hierarchyNodes)
        {
          TreeNode node = new TreeNode(hierarchyNode.Value, 0, 1);
          node.Tag = (object) new ExtNodeTag(hierarchyNode.Key, hierarchyNode.Value, companyType)
          {
            AllowAccess = true
          };
          this.hierarchyTree.SelectedNode.Nodes.Add(node);
          this.hierarchyTree.SelectedNode = node.Parent;
        }
        this.hierarchyTree.EndUpdate();
      }
    }

    private void bcontact_AlreadyExistsOnSelect(object sender, EventArgs e)
    {
      using (AlreadyExists alreadyExists = new AlreadyExists((Dictionary<string, string>) sender))
      {
        if (alreadyExists.ShowDialog((IWin32Window) this) != DialogResult.OK || alreadyExists.Value.Count == 0)
          return;
        foreach (object obj in alreadyExists.Value)
        {
          BizPartnerInfo bizPartner = Session.ContactManager.GetBizPartner(Utils.ParseInt(obj));
          string hierarchyPath = this.currentHierarchyPath + "\\" + bizPartner.CompanyName;
          int depth = this.currentDepth + 1;
          string address = bizPartner.BizAddress != null ? (bizPartner.BizAddress.Street1 != "" ? bizPartner.BizAddress.Street1 + " " : "") + bizPartner.BizAddress.Street2 : "";
          int oidByBusinessId = this.session.ConfigurationManager.GetOidByBusinessId(this.currentSelectedTopNode == this.treeTopNodeLender, bizPartner.ContactID);
          if (oidByBusinessId != this.currentParent)
          {
            HierarchySummary summary = new HierarchySummary(oidByBusinessId, this.currentParent, "", bizPartner.CompanyName, bizPartner.CompanyName, bizPartner.CompanyName, depth, hierarchyPath);
            this.session.ConfigurationManager.OverwriteTPOContact(this.currentSelectedTopNode == this.treeTopNodeLender, oidByBusinessId, bizPartner.CompanyName, bizPartner.CompanyName, bizPartner.CompanyName, address, bizPartner.BizAddress.City, bizPartner.BizAddress.State, bizPartner.BizAddress.Zip, ExternalOriginatorEntityType.None, this.currentParent, depth, hierarchyPath);
            this.session.ConfigurationManager.MoveExternalCompany(this.currentSelectedTopNode == this.treeTopNodeLender, summary);
            this.hierarchyNodes.Add(oidByBusinessId, bizPartner.CompanyName);
          }
        }
      }
    }

    private void stdIconBtnEditOrg_Click(object sender, EventArgs e)
    {
      int depth = 0;
      int parent = 0;
      int companyOrgId = 0;
      if (this.hierarchyTree.SelectedNode != null)
      {
        int int32 = Convert.ToInt32(((ExtNodeTag) this.hierarchyTree.SelectedNode.Tag).Oid);
        if (int32 == 0)
          return;
        string hierarchyPath;
        if (this.currentSelectedTopNode == this.treeTopNodeBanks)
        {
          hierarchyPath = "Banks";
        }
        else
        {
          HierarchySummary orgDetails = this.session.ConfigurationManager.GetOrgDetails(this.currentSelectedTopNode == this.treeTopNodeLender, int32);
          depth = orgDetails.Depth;
          hierarchyPath = orgDetails.HierarchyPath;
          parent = orgDetails.Parent;
          companyOrgId = Convert.ToInt32(((ExtNodeTag) this.getCompanyNode(this.hierarchyTree.SelectedNode).Tag).Oid);
        }
        if (this.currentSelectedTopNode == this.treeTopNodeLender)
        {
          using (ManuallyAdd manuallyAdd = new ManuallyAdd(this.session, int32, parent, depth, hierarchyPath, true))
          {
            if (manuallyAdd.ShowDialog((IWin32Window) this) != DialogResult.OK)
              return;
            this.hierarchyTree.SelectedNode.Text = manuallyAdd.HierarchyNodes.Values.First<string>();
          }
        }
        else if (this.currentSelectedTopNode == this.treeTopNodeBroker)
        {
          if (!TPOClientUtils.HasCompanyTabAccess())
          {
            int num1 = (int) Utils.Dialog((IWin32Window) this, "You currently don't have rights to view TPO information. Please contact your Encompass administrator for additional help.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else
          {
            using (EditCompanyDetailsDialog companyDetailsDialog = new EditCompanyDetailsDialog(this.session, int32, parent, companyOrgId, depth, hierarchyPath, true, false))
            {
              int num2 = (int) companyDetailsDialog.ShowDialog((IWin32Window) this);
              if (companyDetailsDialog.DialogResult != DialogResult.OK)
                return;
              if (companyDetailsDialog.HierarchyNodes.Count > 0)
                this.hierarchyTree.SelectedNode.Text = companyDetailsDialog.HierarchyNodes.Values.First<string>();
              this.RefreshExternalUserInfoTree();
            }
          }
        }
        else
        {
          if (this.currentSelectedTopNode != this.treeTopNodeBanks)
            return;
          using (BankSetupForm bankSetupForm = new BankSetupForm(this.session, int32, true))
          {
            if (bankSetupForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
              return;
            this.hierarchyTree.SelectedNode.Text = bankSetupForm.HierarchyNodes.Values.First<string>();
          }
        }
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select the parent company in the hierarchy.");
      }
    }

    private void stdIconBtnDeleteOrg_Click(object sender, EventArgs e)
    {
      if (this.hierarchyTree.SelectedNode == null)
        return;
      int int32 = Convert.ToInt32(((ExtNodeTag) this.hierarchyTree.SelectedNode.Tag).Oid);
      if (int32 == 0)
        return;
      if (this.gridViewContacts.Items.Count > 0 || this.hierarchyTree.SelectedNode.Nodes.Count > 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The selected organization is not empty - it contains sub-organizations and/or contacts. You can only delete empty organizations.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if ((this.currentSelectedTopNode != this.treeTopNodeBanks ? Utils.Dialog((IWin32Window) this, "Are you sure you want to delete this organization?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) : Utils.Dialog((IWin32Window) this, "Are you sure you want to delete this bank?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)) != DialogResult.Yes)
          return;
        if (this.currentSelectedTopNode == this.treeTopNodeBanks)
        {
          if (this.session.ConfigurationManager.AnyWarehousesUsingThisBank(int32))
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "This Bank can't be deleted until it is removed from all TPOs.");
          }
          else
          {
            this.session.ConfigurationManager.DeleteExternalBank(int32);
            this.hierarchyTree.SelectedNode.Remove();
            int num3 = (int) Utils.Dialog((IWin32Window) this, "The bank has been deleted.");
          }
        }
        else
        {
          this.session.ConfigurationManager.DeleteExternalContact(this.currentSelectedTopNode == this.treeTopNodeLender, int32);
          this.hierarchyTree.SelectedNode.Remove();
        }
        if (this.currentSelectedTopNode == this.treeTopNodeBanks)
          return;
        int num4 = (int) Utils.Dialog((IWin32Window) this, "The organization has been deleted.");
      }
    }

    private void hierarchyTree_NodeMouseDoubleClick(object sender, EventArgs e)
    {
      this.stdIconBtnEditOrg_Click(sender, e);
    }

    private void hierarchyTree_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete)
        this.stdIconBtnDeleteOrg_Click(sender, (EventArgs) e);
      if (e.KeyCode != Keys.Return)
        return;
      this.stdIconBtnEditOrg_Click(sender, (EventArgs) e);
    }

    private void hierarchyTree_AfterSelect(object sender, EventArgs e)
    {
      ExtNodeTag tag = (ExtNodeTag) this.hierarchyTree.SelectedNode.Tag;
      this.exportLicensesMenuItem.Enabled = false;
      if (tag.Oid == 0)
      {
        this.stdIconBtnNewOrg.Enabled = this.contextMenuOrg.MenuItems[0].Enabled = this.isAdmin;
        if (tag.CompanyType.Equals("Tpo"))
        {
          this.stdIconBtnNewOrg.Enabled = this.contextMenuOrg.MenuItems[0].Enabled = this.hasOrganizationCreateRight;
          if (this.hasOrganizationCreateRight)
            this.stdIconBtnNewOrg.Enabled = this.contextMenuOrg.MenuItems[0].Enabled = this.hasCompanyTabCreateRight;
        }
        else if (tag.CompanyType.Equals("Bank"))
          this.stdIconBtnNewOrg.Enabled = this.contextMenuOrg.MenuItems[0].Enabled = this.hasBankEditRight;
        this.stdIconBtnEditOrg.Enabled = this.contextMenuOrg.MenuItems[1].Enabled = false;
        this.stdIconBtnDeleteOrg.Enabled = this.contextMenuOrg.MenuItems[2].Enabled = false;
      }
      else if (this.isAdmin)
      {
        this.stdIconBtnNewOrg.Enabled = this.contextMenuOrg.MenuItems[0].Enabled = true;
        this.stdIconBtnEditOrg.Enabled = this.contextMenuOrg.MenuItems[1].Enabled = true;
        this.stdIconBtnDeleteOrg.Enabled = this.contextMenuOrg.MenuItems[2].Enabled = true;
        this.exportLicensesMenuItem.Enabled = true;
        if (tag.CompanyType.Equals("Bank"))
          this.stdIconBtnNewOrg.Enabled = this.contextMenuOrg.MenuItems[0].Enabled = false;
      }
      else if ((tag.AllowAccess || this.isTpoAdmin) && tag.CompanyType.Equals("Tpo"))
      {
        this.stdIconBtnNewOrg.Enabled = this.contextMenuOrg.MenuItems[0].Enabled = this.hasOrganizationCreateRight;
        if (this.hasOrganizationCreateRight)
          this.stdIconBtnNewOrg.Enabled = this.contextMenuOrg.MenuItems[0].Enabled = this.hasCompanyTabCreateRight;
        this.stdIconBtnEditOrg.Enabled = this.contextMenuOrg.MenuItems[1].Enabled = this.hasOrganizationEditRight;
        this.stdIconBtnDeleteOrg.Enabled = this.contextMenuOrg.MenuItems[2].Enabled = this.hasOrganizationDeleteRight;
      }
      else if (tag.CompanyType.Equals("Bank"))
      {
        this.stdIconBtnNewOrg.Enabled = this.contextMenuOrg.MenuItems[0].Enabled = false;
        this.stdIconBtnEditOrg.Enabled = this.contextMenuOrg.MenuItems[1].Enabled = this.hasBankEditRight;
        this.stdIconBtnDeleteOrg.Enabled = this.contextMenuOrg.MenuItems[2].Enabled = this.hasBankDeleteRight;
      }
      else
      {
        this.stdIconBtnNewOrg.Enabled = this.contextMenuOrg.MenuItems[0].Enabled = false;
        this.stdIconBtnEditOrg.Enabled = this.contextMenuOrg.MenuItems[1].Enabled = false;
        this.stdIconBtnDeleteOrg.Enabled = this.contextMenuOrg.MenuItems[2].Enabled = false;
      }
      this.currentSelectedTopNode = this.getTopParentNode(this.hierarchyTree.SelectedNode);
      this.RefreshExternalUserInfoTree();
      MenuItem menuItem1 = this.contextMenuOrg.MenuItems[4];
      MenuItem menuItem2 = this.contextMenuOrg.MenuItems[3];
      StandardIconButton btnImportOrg = this.btnImportOrg;
      bool flag1;
      this.stdExport.Enabled = flag1 = this.hierarchyTree.SelectedNode == this.treeTopNodeBroker && this.hasOrganizationCreateRight;
      int num1;
      bool flag2 = (num1 = flag1 ? 1 : 0) != 0;
      btnImportOrg.Enabled = num1 != 0;
      int num2;
      bool flag3 = (num2 = flag2 ? 1 : 0) != 0;
      menuItem2.Enabled = num2 != 0;
      int num3 = flag3 ? 1 : 0;
      menuItem1.Enabled = num3 != 0;
      if (tag.CompanyType.Equals("Tpo"))
      {
        this.stdExport.Enabled = true;
        this.contextMenuOrg.MenuItems[3].Enabled = true;
      }
      if (((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ExternalSettings_ExportOrganizations))
        return;
      this.stdExport.Enabled = false;
      this.contextMenuOrg.MenuItems[3].Enabled = false;
    }

    private void loadAllInternalUsers()
    {
      this.loadOrgLookUp();
      if (this.allInternalUsers != null)
        return;
      this.allInternalUsers = this.rOrg.GetAllAccessibleSalesRepUsers();
    }

    private void loadOrgLookUp()
    {
      if (this.rOrg == null)
        this.rOrg = this.session.OrganizationManager;
      if (this.orgLookup != null)
        return;
      OrgInfo[] allOrganizations = this.rOrg.GetAllOrganizations();
      this.orgLookup = new Hashtable(allOrganizations.Length);
      for (int index = 0; index < allOrganizations.Length; ++index)
        this.orgLookup.Add((object) allOrganizations[index].Oid, (object) allOrganizations[index].OrgName);
    }

    private void RefreshExternalUserInfoTree()
    {
      this.gridViewContacts.Items.Clear();
      if (this.isTPOMVP)
        this.gridViewContacts.Columns[2].Text = "Personas";
      this.btnDeleteContact.Enabled = false;
      this.btnEditContact.Enabled = false;
      this.stnIconMove.Enabled = false;
      this.emailButton.Enabled = false;
      this.loadAllInternalUsers();
      ExtNodeTag tag = (ExtNodeTag) this.hierarchyTree.SelectedNode.Tag;
      if (this.currentSelectedTopNode == null || this.currentSelectedTopNode != this.treeTopNodeBroker || tag.Oid == 0)
      {
        this.collapsibleSplitter1.ControlToHide.Hide();
        this.collapsibleSplitter1.Enabled = false;
      }
      else
      {
        this.collapsibleSplitter1.ControlToHide.Show();
        this.collapsibleSplitter1.Enabled = true;
        List<ExternalUserInfo> externalUserInfoList = new List<ExternalUserInfo>();
        if (this.hasTpoContactViewRight)
        {
          if (tag.AllowAccess || this.isAdmin || this.isTpoAdmin)
          {
            externalUserInfoList = ((IEnumerable<ExternalUserInfo>) this.session.ConfigurationManager.GetExternalUserInfosSummary(tag.Oid, false, this.isTPOMVP)).ToList<ExternalUserInfo>();
            this.btnAddContact.Enabled = this.btnImportContact.Enabled = this.hasContactEditRight;
          }
          else
          {
            ExternalUserInfo[] userInfosSummary = this.session.ConfigurationManager.GetExternalUserInfosSummary(tag.Oid, false, this.isTPOMVP);
            this.btnAddContact.Enabled = this.btnImportContact.Enabled = false;
            for (int index = 0; index < userInfosSummary.Length; ++index)
            {
              if (((IEnumerable<string>) this.externalUsersIds).Contains<string>(userInfosSummary[index].ContactID))
                externalUserInfoList.Add(userInfosSummary[index]);
            }
          }
        }
        else
          this.btnAddContact.Enabled = this.btnImportContact.Enabled = false;
        if (this.currentSelectedTopNode == null || this.currentSelectedTopNode == this.treeTopNodeLender || this.hierarchyTree.SelectedNode != null && string.Compare(this.hierarchyTree.SelectedNode.Text, "Third Party Originators", true) == 0)
          this.btnAddContact.Enabled = this.btnImportContact.Enabled = false;
        if (externalUserInfoList.Count > 0)
          this.btnExport.Enabled = this.hasContactExportRight;
        else
          this.btnExport.Enabled = false;
        externalUserInfoList.ForEach((Action<ExternalUserInfo>) (x =>
        {
          string[] items = new string[14]
          {
            x.LastName,
            x.FirstName,
            this.isTPOMVP ? (x.UserPersonas != null ? TPOUtils.ReturnPersonas(x.UserPersonas) : string.Empty) : TPOUtils.returnRoles(x.Roles),
            x.Title,
            x.State,
            x.Email,
            x.Phone,
            this.GetApprovalCurrentStatus(x.ApprovalCurrentStatus),
            x.NmlsID,
            x.NMLSCurrent ? "Yes" : "No",
            null,
            null,
            null,
            null
          };
          DateTime dateTime;
          string str1;
          if (!(x.LastLogin == DateTime.MinValue))
          {
            dateTime = x.LastLogin;
            str1 = dateTime.ToShortDateString();
          }
          else
            str1 = "";
          items[10] = str1;
          items[11] = x.DisabledLogin ? "Disabled" : "Enabled";
          items[12] = this.GetPrimarySalesRepForContact(x.SalesRepID);
          string str2;
          if (!(x.WelcomeEmailDate == DateTime.MinValue))
          {
            dateTime = x.WelcomeEmailDate;
            str2 = dateTime.ToString();
          }
          else
            str2 = "";
          items[13] = str2;
          GVItem gvItem = new GVItem(items);
          gvItem.Tag = (object) x;
          if (this.selectedUserId != null && this.selectedUserId == x.ExternalUserID)
          {
            gvItem.Selected = true;
            this.selectedUserId = (string) null;
          }
          this.gridViewContacts.Items.Add(gvItem);
        }));
        this.gridViewContacts.ReSort();
      }
    }

    private string GetApprovalCurrentStatus(int ApprovalCurrentStatus)
    {
      if (this.contactStatusSettings == null)
        this.contactStatusSettings = this.session.ConfigurationManager.GetExternalOrgSettingsByType(2);
      return this.contactStatusSettings.Any<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (s => s.settingId == ApprovalCurrentStatus)) ? this.contactStatusSettings.Where<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (s => s.settingId == ApprovalCurrentStatus)).Select<ExternalSettingValue, string>((Func<ExternalSettingValue, string>) (s => s.settingValue)).First<string>() : string.Empty;
    }

    private string GetPrimarySalesRepForContact(string userId)
    {
      foreach (UserInfo allInternalUser in this.allInternalUsers)
      {
        if (allInternalUser.Userid == userId)
          return allInternalUser.FullName;
      }
      return string.Empty;
    }

    private void hierarchyTree_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      TreeNode nodeAt = this.hierarchyTree.GetNodeAt(e.X, e.Y);
      if (nodeAt == null)
        return;
      this.hierarchyTree.SelectedNode = nodeAt;
    }

    private void hierarchyTree_ItemDrag(object sender, ItemDragEventArgs e)
    {
      this.dragNode = e.Item is TreeNode ? (TreeNode) e.Item : (TreeNode) null;
      if (this.dragNode == null)
        return;
      TreeNode treeNode = this.dragNode;
      while (treeNode.Parent != null)
        treeNode = treeNode.Parent;
      if (treeNode == this.treeTopNodeBroker || this.dragNode == this.treeTopNodeBroker || this.dragNode == this.treeTopNodeLender || this.dragNode == this.treeTopNodeBanks)
        return;
      int num = (int) this.DoDragDrop(e.Item, DragDropEffects.Move);
    }

    private void hierarchyTree_DragOver(object sender, DragEventArgs e)
    {
      Point client = ((Control) sender).PointToClient(new Point(e.X, e.Y));
      TreeNode nodeAt = ((TreeView) sender).GetNodeAt(client);
      if (nodeAt != null)
      {
        if (!e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
        {
          e.Effect = DragDropEffects.Move;
          return;
        }
        if (this.getTopParentNode(nodeAt) == this.getTopParentNode(this.dragNode))
        {
          e.Effect = DragDropEffects.Move;
          return;
        }
      }
      e.Effect = DragDropEffects.None;
    }

    private void hierarchyTree_DragDrop(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
      {
        Point client = ((Control) sender).PointToClient(new Point(e.X, e.Y));
        TreeNode nodeAt = ((TreeView) sender).GetNodeAt(client);
        TreeNode data = (TreeNode) e.Data.GetData("System.Windows.Forms.TreeNode");
        ExtNodeTag tag = (ExtNodeTag) data.Tag;
        if (this.currentSelectedTopNode != this.treeTopNodeBroker)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "You cannot drag and drop banks and lenders.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (nodeAt != null && this.getTopParentNode(data) != this.getTopParentNode(nodeAt))
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You cannot move a company to different root.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (data != null && nodeAt != null && nodeAt == data.Parent)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "You cannot move a company to same parent company.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (tag.Oid == 0)
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "You cannot move the root.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          if (nodeAt == data || nodeAt == null || data == null)
            return;
          if (nodeAt.FullPath.Contains(data.FullPath))
          {
            int num5 = (int) Utils.Dialog((IWin32Window) this, "You cannot move a company to its descendant companies.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            if (Utils.Dialog((IWin32Window) this, "The LO Comp plans will no longer be associated with the parent organization comp plan. Do you want to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.OK)
              return;
            nodeAt.Nodes.Add((TreeNode) data.Clone());
            nodeAt.Expand();
            data.Remove();
            this.moveUpdate(((ExtNodeTag) nodeAt.Tag).Oid, tag.Oid, tag.CompanyName, "");
          }
        }
      }
      else
      {
        Point client = ((Control) sender).PointToClient(new Point(e.X, e.Y));
        TreeNode nodeAt = ((TreeView) sender).GetNodeAt(client);
        if (!this.IsSameOrganization(this.selectedExternalUsers[0].ExternalOrgID, ((ExtNodeTag) nodeAt.Tag).Oid))
        {
          int num6 = (int) Utils.Dialog((IWin32Window) this, "Cannot move to a different organization.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          List<ExternalUserInfo> source = new List<ExternalUserInfo>();
          foreach (ExternalUserInfo selectedExternalUser in this.selectedExternalUsers)
          {
            if (this.IsPrimaryManager(selectedExternalUser, ((ExtNodeTag) nodeAt.Tag).Oid))
            {
              int num7 = (int) Utils.Dialog((IWin32Window) this, "Cannot move manager .Please assign a Manager.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
              selectedExternalUser.UpdatedByExternalAdmin = "";
              source.Add(selectedExternalUser);
            }
          }
          if (source.Any<ExternalUserInfo>())
          {
            List<ExternalUserInfo> externalUserInfoList = this.session.ConfigurationManager.GetExternalUserInfoList(source.Select<ExternalUserInfo, string>((Func<ExternalUserInfo, string>) (a => a.ExternalUserID)).ToList<string>());
            this.session.ConfigurationManager.MoveExternalUser(externalUserInfoList, ((ExtNodeTag) nodeAt.Tag).Oid);
            List<ExternalOrgURL> selectedOrgUrls = this.session.ConfigurationManager.GetSelectedOrgUrls(((ExtNodeTag) nodeAt.Tag).Oid);
            if (selectedOrgUrls.Count == 1)
            {
              foreach (ExternalUserInfo externalUserInfo in externalUserInfoList)
              {
                ExternalUserURL[] externalUserInfoUrLs = this.session.ConfigurationManager.GetExternalUserInfoURLs(externalUserInfo.ExternalUserID);
                List<int> intList = new List<int>();
                if (externalUserInfoUrLs != null && ((IEnumerable<ExternalUserURL>) externalUserInfoUrLs).Any<ExternalUserURL>())
                  intList = ((IEnumerable<ExternalUserURL>) externalUserInfoUrLs).Select<ExternalUserURL, int>((Func<ExternalUserURL, int>) (a => a.URLID)).ToList<int>();
                if (!intList.Contains(selectedOrgUrls[0].URLID))
                  intList.Add(selectedOrgUrls[0].URLID);
                this.session.ConfigurationManager.SaveExternalUserInfoURLs(externalUserInfo.ExternalUserID, intList.ToArray());
              }
            }
          }
          this.RefreshExternalUserInfoTree();
        }
      }
    }

    private bool IsSameOrganization(int sourceOid, int destinationOid)
    {
      return this.session.ConfigurationManager.GetRootOrganisation(false, sourceOid).oid == this.session.ConfigurationManager.GetRootOrganisation(false, destinationOid).oid;
    }

    private bool IsPrimaryManager(ExternalUserInfo selectedExtUser, int destinationOid)
    {
      ExternalOriginatorManagementData externalOrganization = this.session.ConfigurationManager.GetExternalOrganization(false, selectedExtUser.ExternalOrgID);
      ExternalOriginatorManagementData originatorManagementData = (ExternalOriginatorManagementData) null;
      if (destinationOid != -1)
        originatorManagementData = this.session.ConfigurationManager.GetExternalOrganization(false, destinationOid);
      return (destinationOid == -1 || externalOrganization.ExternalID != originatorManagementData.ExternalID) && this.session.ConfigurationManager.GetExternalOrganizationCountForManagerID(selectedExtUser.ExternalUserID) > 0;
    }

    private void moveUpdate(int parentNew, int oid, string companyName, string externalID)
    {
      int depth;
      string hierarchyPath;
      if (parentNew != 0)
      {
        HierarchySummary orgDetails = this.session.ConfigurationManager.GetOrgDetails(this.currentSelectedTopNode == this.treeTopNodeLender, parentNew);
        depth = orgDetails.Depth + 1;
        hierarchyPath = orgDetails.HierarchyPath + "\\" + companyName;
      }
      else
      {
        depth = 1;
        hierarchyPath = (this.currentSelectedTopNode == this.treeTopNodeLender ? "Lenders" : "Third Party Originators") + "\\" + companyName;
      }
      this.session.ConfigurationManager.MoveExternalCompany(this.currentSelectedTopNode == this.treeTopNodeLender, new HierarchySummary(oid, parentNew, externalID, companyName, companyName, companyName, depth, hierarchyPath));
    }

    private TreeNode getTopParentNode(TreeNode node)
    {
      TreeNode treeNode = node;
      while (treeNode.Level > 0)
        treeNode = treeNode.Parent;
      if (treeNode.Text == "Lenders")
        return this.treeTopNodeLender;
      return treeNode.Text == "Banks" ? this.treeTopNodeBanks : this.treeTopNodeBroker;
    }

    private TreeNode getCompanyNode(TreeNode node)
    {
      TreeNode companyNode = node;
      while (companyNode.Level > 1)
        companyNode = companyNode.Parent;
      return companyNode;
    }

    private void btnAddContact_Click(object sender, EventArgs e)
    {
      if (this.hierarchyTree.SelectedNode == null)
        return;
      ExtNodeTag tag = (ExtNodeTag) this.hierarchyTree.SelectedNode.Tag;
      if (!this.hierarchyTree.SelectedNode.FullPath.Contains("Third Party Originators") || tag.Oid == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select an external organization", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        using (TPOContactSetupForm contactSetupForm = new TPOContactSetupForm(tag.Oid, this.session))
        {
          int num2 = (int) contactSetupForm.ShowDialog((IWin32Window) this);
        }
        this.RefreshExternalUserInfoTree();
      }
    }

    private void btnEditContact_Click(object sender, EventArgs e)
    {
      if (!this.hasContactEditRight || this.gridViewContacts.SelectedItems.Count == 0)
        return;
      ExternalUserInfo tag = (ExternalUserInfo) this.gridViewContacts.SelectedItems[0].Tag;
      ExternalUserInfo externalUserInfo = this.session.ConfigurationManager.GetExternalUserInfo(tag.ExternalUserID);
      using (TPOContactSetupForm contactSetupForm = new TPOContactSetupForm(tag.ExternalOrgID, this.session, externalUserInfo))
      {
        int num = (int) contactSetupForm.ShowDialog((IWin32Window) this);
      }
      this.RefreshExternalUserInfoTree();
    }

    private void gridViewContacts_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.btnEditContact_Click(source, (EventArgs) e);
    }

    private void btnDeleteContact_Click(object sender, EventArgs e)
    {
      List<ExternalUserInfo> source = new List<ExternalUserInfo>();
      if (this.gridViewContacts.SelectedItems != null && this.gridViewContacts.SelectedItems.Any<GVItem>() && this.selectedExternalUsers != null && this.selectedExternalUsers.Any<ExternalUserInfo>() && MessageBox.Show("Are you sure you want to delete selected user(s)?", "Encompass", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      {
        this.allLOLPUsers = Session.ConfigurationManager.GetAllLOLPUsers();
        foreach (ExternalUserInfo selectedExternalUser in this.selectedExternalUsers)
        {
          if (this.IsPrimaryManager(selectedExternalUser, -1))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The contact you are deleting is a Primary Manager in the TPO company. You must assign a new Primary Manager to the TPO before deleting this contact.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
            source.Add(selectedExternalUser);
        }
        if (source.Any<ExternalUserInfo>())
        {
          foreach (ExternalUserInfo externalUserInfo in this.session.ConfigurationManager.GetExternalUserInfoList(source.Select<ExternalUserInfo, string>((Func<ExternalUserInfo, string>) (a => a.ExternalUserID)).ToList<string>()))
          {
            if (this.session.ConfigurationManager.CheckIfAnyTPOSiteExists())
            {
              bool flag = false;
              foreach (Persona userPersona in externalUserInfo.UserPersonas)
              {
                List<int> personaIds;
                if (this.isPersonaAssignedtoTPOLoanOfficerOrTPOLoanProcessor(userPersona, externalUserInfo, out personaIds))
                {
                  flag = false;
                  this.allLOLPUsers = Session.ConfigurationManager.GetAllTPOLOLPUsers(personaIds);
                  using (LoanReassignmentForm reassignmentForm = new LoanReassignmentForm(this.session, externalUserInfo, this.allLOLPUsers, personaIds))
                  {
                    if (reassignmentForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
                    {
                      if ((UserInfo) reassignmentForm.getSelectedUser() != (UserInfo) null && this.session.ConfigurationManager.ReassignTPOLoans(externalUserInfo, reassignmentForm.getSelectedUser(), this.isTPOMVP))
                      {
                        int num = (int) Utils.Dialog((IWin32Window) this, "All Loans have been reassigned to " + reassignmentForm.getSelectedUser().FirstName + " " + reassignmentForm.getSelectedUser().LastName + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                      }
                      flag = true;
                      break;
                    }
                    break;
                  }
                }
                else
                  flag = true;
              }
              if (this.session.ServerManager.GetAllUserSessionIds(externalUserInfo.ContactID).Length != 0)
              {
                int num1 = (int) Utils.Dialog((IWin32Window) this, "You cannot delete user \"" + externalUserInfo.FullName + "\" because this user account is currently logged in.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              }
              else if (flag)
                this.session.ConfigurationManager.DeleteExternalUserInfo(externalUserInfo.ExternalOrgID, externalUserInfo, this.session.UserInfo);
            }
            else
            {
              this.allLOLPUsers = Session.ConfigurationManager.GetAllLOLPUsers();
              if (TPOUtils.IsLoanOfficer(externalUserInfo.Roles) || TPOUtils.IsLoanProcessor(externalUserInfo.Roles))
              {
                using (LoanReassignmentForm reassignmentForm = new LoanReassignmentForm(this.session, externalUserInfo, this.allLOLPUsers))
                {
                  if (reassignmentForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
                  {
                    if ((UserInfo) reassignmentForm.getSelectedUser() != (UserInfo) null && this.session.ConfigurationManager.ReassignTPOLoans(externalUserInfo, reassignmentForm.getSelectedUser()))
                    {
                      int num = (int) Utils.Dialog((IWin32Window) this, "All Loans have been reassigned to " + reassignmentForm.getSelectedUser().FirstName + " " + reassignmentForm.getSelectedUser().LastName + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    this.session.ConfigurationManager.DeleteExternalUserInfo(externalUserInfo.ExternalOrgID, externalUserInfo, this.session.UserInfo);
                  }
                }
              }
              else
                this.session.ConfigurationManager.DeleteExternalUserInfo(externalUserInfo.ExternalOrgID, externalUserInfo, this.session.UserInfo);
            }
          }
        }
      }
      this.RefreshExternalUserInfoTree();
    }

    private void gridViewContacts_ItemDrag(object source, GVItemEventArgs e)
    {
      int num = (int) this.DoDragDrop((object) e.Item, DragDropEffects.Move);
    }

    private void gridViewContacts_DragOver(object sender, DragEventArgs e)
    {
      e.Effect = e.AllowedEffect;
    }

    private void gridViewContacts_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.selectedExternalUsers.Clear();
      if (this.gridViewContacts.SelectedItems.Any<GVItem>())
      {
        this.btnDeleteContact.Enabled = this.hasContactDeleteRight;
        this.emailButton.Enabled = this.hasSendWelcomeEmailRight;
        if (this.gridViewContacts.SelectedItems.Count > 1)
        {
          this.btnEditContact.Enabled = false;
          this.stnIconMove.Enabled = this.hasContactEditRight;
        }
        else
          this.stnIconMove.Enabled = this.btnEditContact.Enabled = this.hasContactEditRight;
        foreach (GVItem selectedItem in this.gridViewContacts.SelectedItems)
          this.selectedExternalUsers.Add((ExternalUserInfo) selectedItem.Tag);
      }
      else
      {
        this.btnDeleteContact.Enabled = false;
        this.btnEditContact.Enabled = false;
        this.stnIconMove.Enabled = false;
        this.emailButton.Enabled = false;
      }
    }

    private void btnImportContact_Click(object sender, EventArgs e)
    {
      if (new ContactImportWizard(EllieMae.EMLite.ContactUI.ContactType.TPO, Convert.ToInt32(((ExtNodeTag) this.hierarchyTree.SelectedNode.Tag).Oid), this.session.ConfigurationManager.GetAllContactID()).ShowDialog((IWin32Window) this.ParentForm) != DialogResult.OK)
        return;
      this.RefreshExternalUserInfoTree();
    }

    private void btnImportOrg_Click(object sender, EventArgs e)
    {
      if (new ContactImportWizard(EllieMae.EMLite.ContactUI.ContactType.TPOCompany, -1, (List<long>) null).ShowDialog((IWin32Window) this.ParentForm) != DialogResult.OK)
        return;
      this.treeTopNodeLender.Nodes.Clear();
      this.treeTopNodeBroker.Nodes.Clear();
      this.hierarchyTree.AfterSelect -= new TreeViewEventHandler(this.hierarchyTree_AfterSelect);
      this.refreshUserIdsAndBrokerRootList();
      this.initHierarchy();
      if (this.treeTopNodeBroker != null)
        this.treeTopNodeLender.Collapse();
      if (!this.treeTopNodeBroker.IsExpanded)
        this.treeTopNodeBroker.ExpandAll();
      this.hierarchyTree.AfterSelect += new TreeViewEventHandler(this.hierarchyTree_AfterSelect);
      if (this.treeTopNodeBroker == null)
        return;
      this.hierarchyTree.SelectedNode = this.treeTopNodeBroker;
    }

    private void refreshUserIdsAndBrokerRootList()
    {
      ArrayList andOrgBySalesRep = Session.ConfigurationManager.GetExternalUserAndOrgBySalesRep(Session.UserID);
      this.externalUsersIds = ((List<string>) andOrgBySalesRep[0]).ToArray();
      this.hierarchyTree.BrokerRootOrgsList = this.externalRootOrgsList = (List<ExternalOriginatorManagementData>) andOrgBySalesRep[1];
      this.hierarchyTree.ExternalOrgsList = this.externalOrgsList = (List<int>) andOrgBySalesRep[2];
    }

    private void standardIconButton1_Click(object sender, EventArgs e)
    {
      List<ExternalUserInfo> source = new List<ExternalUserInfo>();
      foreach (GVItem selectedItem in this.gridViewContacts.SelectedItems)
        source.Add((ExternalUserInfo) selectedItem.Tag);
      if (source == null || !this.selectedExternalUsers.Any<ExternalUserInfo>())
        return;
      int num = (int) new ConsolidateContactsControl(this.session, this.session.ConfigurationManager.GetExternalUserInfoList(source.Select<ExternalUserInfo, string>((Func<ExternalUserInfo, string>) (a => a.ExternalUserID)).ToList<string>())).ShowDialog((IWin32Window) this);
      this.RefreshExternalUserInfoTree();
    }

    private void hierarchyTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
    {
      if (this.hierarchyTree.SelectedNode == null)
        return;
      ExtNodeTag tag = (ExtNodeTag) this.hierarchyTree.SelectedNode.Tag;
      if (tag.CompanyType.Equals("Lender"))
      {
        this.collapsibleSplitter1.ControlToHide.Hide();
      }
      else
      {
        if (!tag.CompanyType.Equals("Bank"))
          return;
        this.collapsibleSplitter1.ControlToHide.Hide();
      }
    }

    private void stdExport_Click(object sender, EventArgs e)
    {
      CsvImportParameters importParameters = new CsvImportParameters(EllieMae.EMLite.ContactUI.ContactType.TPOCompany);
      ExtNodeTag tag = (ExtNodeTag) this.hierarchyTree.SelectedNode.Tag;
      this.refreshUserIdsAndBrokerRootList();
      this.selectedCompanies.Clear();
      if (tag.CompanyType.Equals("Tpo"))
      {
        if (tag.Oid > 0)
        {
          this.selectedCompanies.Add(this.session.ConfigurationManager.GetExternalOrganization(false, tag.Oid));
          List<int> organizationDesendents = this.session.ConfigurationManager.GetExternalOrganizationDesendents(tag.Oid);
          ExternalOriginatorManagementData rootOrganisation = this.session.ConfigurationManager.GetRootOrganisation(false, tag.Oid);
          List<ExternalOriginatorManagementData> externalOrganizations = this.session.ConfigurationManager.GetExternalOrganizations(false, organizationDesendents.ToList<int>());
          if (this.externalOrgsList.Contains(rootOrganisation.oid) || this.isAdmin)
          {
            foreach (ExternalOriginatorManagementData originatorManagementData in externalOrganizations)
              this.selectedCompanies.Add(originatorManagementData);
          }
        }
        else
        {
          List<ExternalOriginatorManagementData> parentOrganizations = this.session.ConfigurationManager.GetAllExternalParentOrganizations(false);
          List<ExternalOriginatorManagementData> externalOrganizations = this.session.ConfigurationManager.GetAllExternalOrganizations(false);
          foreach (ExternalOriginatorManagementData originatorManagementData1 in parentOrganizations)
          {
            if (this.externalOrgsList.Contains(originatorManagementData1.oid) || this.isAdmin)
            {
              this.selectedCompanies.Add(originatorManagementData1);
              List<int> desc = this.session.ConfigurationManager.GetExternalOrganizationDesendents(originatorManagementData1.oid);
              foreach (ExternalOriginatorManagementData originatorManagementData2 in externalOrganizations.Where<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (a => desc.Contains(a.oid))))
                this.selectedCompanies.Add(originatorManagementData2);
            }
          }
        }
      }
      ExcelHandler excelHandler = new ExcelHandler();
      CsvImportColumn[] availableColumns = importParameters.GetAllAvailableColumns();
      excelHandler.Headers = ((IEnumerable<CsvImportColumn>) availableColumns).Select<CsvImportColumn, string>((Func<CsvImportColumn, string>) (a => a.Description)).ToArray<string>();
      string[] array = ((IEnumerable<CsvImportColumn>) availableColumns).Select<CsvImportColumn, string>((Func<CsvImportColumn, string>) (a => a.PropertyName)).ToArray<string>();
      foreach (ExternalOriginatorManagementData selectedCompany in this.selectedCompanies)
      {
        List<string> stringList = new List<string>();
        foreach (string str in array)
        {
          object propValue = CompanySetupForm.GetPropValue((object) selectedCompany, str);
          string defVal = !(str == "CompanyNetWorth") || propValue != null ? propValue.ToString() : "";
          this.ProcessTPOCompanyValues(str, ref defVal, selectedCompany);
          stringList.Add(Convert.ToString(defVal));
        }
        excelHandler.AddDataRow(stringList.ToArray());
      }
      excelHandler.Export();
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      CsvImportParameters importParameters = new CsvImportParameters(EllieMae.EMLite.ContactUI.ContactType.TPO, this.isTPOMVP);
      ExcelHandler excelExport = new ExcelHandler();
      CsvImportColumn[] availableColumns = importParameters.GetAllAvailableColumns();
      excelExport.Headers = ((IEnumerable<CsvImportColumn>) availableColumns).Select<CsvImportColumn, string>((Func<CsvImportColumn, string>) (a => a.Description)).ToArray<string>();
      string[] properties = ((IEnumerable<CsvImportColumn>) availableColumns).Select<CsvImportColumn, string>((Func<CsvImportColumn, string>) (a => a.PropertyName)).ToArray<string>();
      Dictionary<string, string> mapDict = new ExportMap().GetTPOMap(this.isTPOMVP);
      (!this.gridViewContacts.SelectedItems.Any<GVItem>() ? new List<GVItem>((IEnumerable<GVItem>) this.gridViewContacts.Items) : new List<GVItem>((IEnumerable<GVItem>) this.gridViewContacts.SelectedItems)).ForEach((Action<GVItem>) (x =>
      {
        List<string> stringList = new List<string>();
        ExternalUserInfo externalUserInfo = this.session.ConfigurationManager.GetExternalUserInfo(((ExternalUserInfo) x.Tag).ExternalUserID);
        foreach (string str in properties)
        {
          object propValue = CompanySetupForm.GetPropValue((object) externalUserInfo, mapDict[str]);
          string defVal = string.Empty;
          if (propValue is Persona[])
          {
            defVal = TPOUtils.ReturnPersonas((Persona[]) propValue);
          }
          else
          {
            defVal = propValue.ToString();
            this.ProcessTPOValues(str, ref defVal, externalUserInfo);
          }
          stringList.Add(Convert.ToString(defVal));
        }
        excelExport.AddDataRow(stringList.ToArray());
      }));
      excelExport.Export();
    }

    public static object GetPropValue(object src, string propName)
    {
      PropertyInfo property = src.GetType().GetProperty(propName);
      return property != (PropertyInfo) null ? property.GetValue(src, (object[]) null) : (object) "";
    }

    public void ProcessTPOValues(string propertyName, ref string defVal, ExternalUserInfo user)
    {
      string lower = propertyName.ToLower();
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(lower))
      {
        case 245872446:
          if (!(lower == "nmlscurrent"))
            return;
          break;
        case 481686554:
          int num1 = lower == "sales_rep_userid" ? 1 : 0;
          return;
        case 494537007:
          if (!(lower == "approval_date"))
            return;
          goto label_51;
        case 1511450679:
          int num2 = lower == "lock_info_fax" ? 1 : 0;
          return;
        case 1600921457:
          if (!(lower == "approval_status_watchlist"))
            return;
          break;
        case 1711728130:
          if (!(lower == "approval_status_date"))
            return;
          goto label_51;
        case 2000032175:
          int num3 = lower == "phone" ? 1 : 0;
          return;
        case 2016490230:
          int num4 = lower == "state" ? 1 : 0;
          return;
        case 2562481374:
          if (!(lower == "roles"))
            return;
          List<string> stringList = new List<string>();
          try
          {
            if ((Convert.ToInt32(defVal) & 1) == 1)
              stringList.Add("Loan Officer");
            if ((Convert.ToInt32(defVal) & 2) == 2)
              stringList.Add("Loan Processor");
            if ((Convert.ToInt32(defVal) & 4) == 4)
              stringList.Add("Manager");
            if ((Convert.ToInt32(defVal) & 8) == 8)
              stringList.Add("Administrator");
            defVal = string.Join(",", stringList.ToArray());
            return;
          }
          catch
          {
            int num5 = (int) Utils.Dialog((IWin32Window) this, "Invalid roles in " + user.FirstName + " " + user.LastName + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
        case 2784480765:
          if (!(lower == "inheritparentratesheet"))
            return;
          break;
        case 3002599984:
          int num6 = lower == "fax" ? 1 : 0;
          return;
        case 3250737933:
          if (!(lower == "approval_status"))
            return;
          try
          {
            List<ExternalSettingValue> orgSettingsByName = this.session.ConfigurationManager.GetExternalOrgSettingsByName("Current Contact Status");
            string val = defVal;
            List<ExternalSettingValue> list = orgSettingsByName.Where<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (a => a.settingId == Convert.ToInt32(val))).ToList<ExternalSettingValue>();
            if (list != null && list.Any<ExternalSettingValue>())
            {
              defVal = list[0].settingValue;
              return;
            }
            defVal = "";
            return;
          }
          catch
          {
            int num7 = (int) Utils.Dialog((IWin32Window) this, "Invalid approval status " + user.FirstName + " " + user.LastName + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
        case 3387086158:
          int num8 = lower == "cell_phone" ? 1 : 0;
          return;
        case 3389242482:
          if (!(lower == "disabledlogin"))
            return;
          break;
        case 3526671401:
          if (!(lower == "usecompanyaddress"))
            return;
          break;
        case 3610302575:
          if (!(lower == "sales_rep_name"))
            return;
          try
          {
            UserInfo user1 = this.session.OrganizationManager.GetUser(user.SalesRepID);
            defVal = user1.FirstName + " " + user1.MiddleName + " " + user1.LastName + " " + user1.SuffixName;
            return;
          }
          catch
          {
            int num9 = (int) Utils.Dialog((IWin32Window) this, "Invalid Sales rep in " + user.FirstName + " " + user.LastName + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
        case 3652408767:
          int num10 = lower == "rate_sheet_fax" ? 1 : 0;
          return;
        default:
          return;
      }
      if (defVal == "True")
      {
        defVal = "Yes";
        return;
      }
      if (defVal == "False")
      {
        defVal = "No";
        return;
      }
      defVal = "";
      return;
label_51:
      if (Convert.ToDateTime(defVal) == DateTime.MinValue)
      {
        defVal = "";
      }
      else
      {
        ref string local = ref defVal;
        DateTime date = Utils.ParseDate((object) defVal);
        date = date.Date;
        string str = date.ToString("MM/dd/yyyy");
        local = str;
      }
    }

    public void ProcessTPOCompanyValues(
      string propertyName,
      ref string defVal,
      ExternalOriginatorManagementData org)
    {
      string lower = propertyName.ToLower();
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(lower))
      {
        case 7325082:
          if (!(lower == "entitytype"))
            return;
          defVal = Convert.ToString((object) org.entityType);
          return;
        case 150355829:
          int num1 = lower == "eppsusername" ? 1 : 0;
          return;
        case 230981954:
          int num2 = lower == "city" ? 1 : 0;
          return;
        case 270461060:
          int num3 = lower == "faxforlockinfo" ? 1 : 0;
          return;
        case 314380392:
          if (!(lower == "addtowatchlist"))
            return;
          if (defVal == "True")
          {
            defVal = "Yes";
            return;
          }
          if (defVal == "False")
          {
            defVal = "No";
            return;
          }
          defVal = "";
          return;
        case 437869252:
          if (!(lower == "eoexpirationdate"))
            return;
          break;
        case 518285988:
          int num4 = lower == "mersoriginatingorgid" ? 1 : 0;
          return;
        case 745154899:
          int num5 = lower == "address" ? 1 : 0;
          return;
        case 873850433:
          if (!(lower == "useparentinfoforbusinessinfo"))
            return;
          goto label_152;
        case 1067207788:
          int num6 = lower == "otherentitydescription" ? 1 : 0;
          return;
        case 1225359707:
          if (!(lower == "typeofentity"))
            return;
          if (string.Compare(defVal, "1", true) == 0)
          {
            defVal = "Individual";
            return;
          }
          if (string.Compare(defVal, "2", true) == 0)
          {
            defVal = "Sole Proprietorship";
            return;
          }
          if (string.Compare(defVal, "3", true) == 0)
          {
            defVal = "Partnership";
            return;
          }
          if (string.Compare(defVal, "4", true) == 0)
          {
            defVal = "Corporation";
            return;
          }
          if (string.Compare(defVal, "5", true) == 0)
          {
            defVal = "Limited Liability Company";
            return;
          }
          if (string.Compare(defVal, "6", true) == 0)
          {
            defVal = "Other (please specify)";
            return;
          }
          defVal = "";
          return;
        case 1247181078:
          int num7 = lower == "eopolicynumber" ? 1 : 0;
          return;
        case 1466078399:
          if (!(lower == "dateofincorporation"))
            return;
          break;
        case 1507117980:
          if (!(lower == "useparentinfoforcompanydetails"))
            return;
          goto label_152;
        case 1601947264:
          if (!(lower == "currentstatusdate"))
            return;
          break;
        case 1605911503:
          int num8 = lower == "ownername" ? 1 : 0;
          return;
        case 1609545957:
          if (!(lower == "dusponsored"))
            return;
          goto label_147;
        case 1807128503:
          int num9 = lower == "taxid" ? 1 : 0;
          return;
        case 1840624404:
          int num10 = lower == "nmlsid" ? 1 : 0;
          return;
        case 1883068644:
          int num11 = lower == "eocompany" ? 1 : 0;
          return;
        case 2016490230:
          int num12 = lower == "state" ? 1 : 0;
          return;
        case 2052791809:
          if (!(lower == "eppspricegroup"))
            return;
          try
          {
            List<ExternalSettingValue> orgSettingsByName = this.session.ConfigurationManager.GetExternalOrgSettingsByName("Price Group");
            string priceGroup = defVal;
            if (priceGroup == string.Empty || priceGroup == "-1")
            {
              defVal = "";
              return;
            }
            List<ExternalSettingValue> list = orgSettingsByName.Where<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (a => a.settingId == Convert.ToInt32(priceGroup))).ToList<ExternalSettingValue>();
            if (list != null && list.Any<ExternalSettingValue>())
            {
              defVal = list[0].CodeValueConcat;
              return;
            }
            defVal = "";
            return;
          }
          catch
          {
            int num13 = (int) Utils.Dialog((IWin32Window) this, "Invalid Price Group in " + org.OrganizationName + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
        case 2324124615:
          int num14 = lower == "email" ? 1 : 0;
          return;
        case 2341412979:
          if (!(lower == "incorporated"))
            return;
          goto label_152;
        case 2366122467:
          int num15 = lower == "emailforratesheet" ? 1 : 0;
          return;
        case 2474991798:
          if (!(lower == "useparentinfoforapprovalstatus"))
            return;
          goto label_152;
        case 2475254023:
          if (!(lower == "managername"))
            return;
          try
          {
            ExternalUserInfo externalUserInfo = this.session.ConfigurationManager.GetExternalUserInfo(org.Manager);
            if (!((UserInfo) externalUserInfo != (UserInfo) null))
              return;
            defVal = externalUserInfo.FirstName + " " + externalUserInfo.LastName;
            return;
          }
          catch
          {
            int num16 = (int) Utils.Dialog((IWin32Window) this, "Invalid manager name in " + org.OrganizationName + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
        case 2658998130:
          if (!(lower == "useparentinfoforratelock"))
            return;
          goto label_152;
        case 2662776136:
          int num17 = lower == "faxforratesheet" ? 1 : 0;
          return;
        case 2683488695:
          if (!(lower == "usessnformat"))
            return;
          goto label_152;
        case 2877453236:
          int num18 = lower == "zip" ? 1 : 0;
          return;
        case 2942672302:
          int num19 = lower == "phonenumber" ? 1 : 0;
          return;
        case 2959644662:
          int num20 = lower == "companylegalname" ? 1 : 0;
          return;
        case 2971599132:
          int num21 = lower == "orgid" ? 1 : 0;
          return;
        case 2976491737:
          if (!(lower == "applicationdate"))
            return;
          break;
        case 2980766852:
          if (!(lower == "currentstatus"))
            return;
          try
          {
            List<ExternalSettingValue> orgSettingsByName = this.session.ConfigurationManager.GetExternalOrgSettingsByName("Current Company Status");
            string val = defVal;
            List<ExternalSettingValue> list = orgSettingsByName.Where<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (a => a.settingId == Convert.ToInt32(val))).ToList<ExternalSettingValue>();
            if (list != null && list.Any<ExternalSettingValue>())
            {
              defVal = list[0].settingValue;
              return;
            }
            defVal = "";
            return;
          }
          catch (Exception ex)
          {
            int num22 = (int) Utils.Dialog((IWin32Window) this, "Invalid Company Status in " + org.OrganizationName + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
        case 3164088636:
          int num23 = lower == "financialsperiod" ? 1 : 0;
          return;
        case 3183731907:
          if (!(lower == "cancloseinownname"))
            return;
          goto label_147;
        case 3296300618:
          if (!(lower == "organizationtype"))
            return;
          if (org.OrganizationType == ExternalOriginatorOrgType.BranchExtension)
          {
            defVal = "Branch Extension";
            return;
          }
          if (org.OrganizationType != ExternalOriginatorOrgType.CompanyExtension)
            return;
          defVal = "Company Extension";
          return;
        case 3300740249:
          int num24 = lower == "stateincorp" ? 1 : 0;
          return;
        case 3361275392:
          if (!(lower == "canfundinownname"))
            return;
          goto label_147;
        case 3389242482:
          if (!(lower == "disabledlogin"))
            return;
          goto label_152;
        case 3412111036:
          int num25 = lower == "primarysalesrepuserid" ? 1 : 0;
          return;
        case 3423867566:
          if (!(lower == "companydbaname"))
            return;
          try
          {
            List<ExternalOrgDBAName> dbaNames = this.session.ConfigurationManager.GetDBANames(org.oid);
            if (dbaNames != null && dbaNames.Any<ExternalOrgDBAName>())
            {
              ExternalOrgDBAName externalOrgDbaName = dbaNames.FirstOrDefault<ExternalOrgDBAName>((Func<ExternalOrgDBAName, bool>) (item => item.SetAsDefault));
              defVal = externalOrgDbaName == null ? "" : externalOrgDbaName.Name;
              return;
            }
            defVal = "";
            return;
          }
          catch
          {
            int num26 = (int) Utils.Dialog((IWin32Window) this, "Invalid Company DBName in " + org.OrganizationName + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
        case 3455537607:
          if (!(lower == "useparentinfoforepps"))
            return;
          goto label_152;
        case 3636225201:
          int num27 = lower == "organizationname" ? 1 : 0;
          return;
        case 3665600909:
          int num28 = lower == "emailforlockinfo" ? 1 : 0;
          return;
        case 3701801305:
          if (!(lower == "parentorganizationname"))
            return;
          ExternalOriginatorManagementData externalOrganization = this.session.ConfigurationManager.GetExternalOrganization(false, org.Parent);
          if (externalOrganization == null)
            return;
          defVal = externalOrganization.OrganizationName;
          return;
        case 3848781856:
          if (!(lower == "financialslastupdate"))
            return;
          break;
        case 3864719273:
          if (!(lower == "companynetworth") || !(defVal == "0"))
            return;
          defVal = "";
          return;
        case 3975774267:
          if (!(lower == "eppscompmodel"))
            return;
          if (string.Compare(defVal, "0", true) == 0)
          {
            defVal = "Borrower Only";
            return;
          }
          if (string.Compare(defVal, "1", true) == 0)
          {
            defVal = "Creditor Only";
            return;
          }
          if (string.Compare(defVal, "2", true) == 0)
          {
            defVal = "Borrower or Creditor";
            return;
          }
          defVal = "";
          return;
        case 3986465945:
          if (!(lower == "companyrating"))
            return;
          try
          {
            List<ExternalSettingValue> orgSettingsByName = this.session.ConfigurationManager.GetExternalOrgSettingsByName("Company Rating");
            string valrating = defVal;
            List<ExternalSettingValue> list = orgSettingsByName.Where<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (a => a.settingId == Convert.ToInt32(valrating))).ToList<ExternalSettingValue>();
            if (list != null && list.Any<ExternalSettingValue>())
            {
              defVal = list[0].settingValue;
              return;
            }
            defVal = "";
            return;
          }
          catch
          {
            int num29 = (int) Utils.Dialog((IWin32Window) this, "Invalid Company Rating in " + org.OrganizationName + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
        case 4057609381:
          if (!(lower == "primarysalesrepname"))
            return;
          try
          {
            UserInfo user = this.session.OrganizationManager.GetUser(org.PrimarySalesRepUserId);
            defVal = user.FirstName + "  " + user.MiddleName + " " + user.LastName + " " + user.SuffixName;
            return;
          }
          catch
          {
            int num30 = (int) Utils.Dialog((IWin32Window) this, "Invalid Company Primary Sales Rep in " + org.OrganizationName + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
        case 4129645889:
          int num31 = lower == "faxnumber" ? 1 : 0;
          return;
        case 4228006590:
          if (!(lower == "approveddate"))
            return;
          break;
        case 4243573962:
          int num32 = lower == "website" ? 1 : 0;
          return;
        default:
          return;
      }
      if (Convert.ToDateTime(defVal) == DateTime.MinValue)
      {
        defVal = "";
        return;
      }
      ref string local = ref defVal;
      DateTime date = Utils.ParseDate((object) defVal);
      date = date.Date;
      string str = date.ToString("MM/dd/yyyy");
      local = str;
      return;
label_147:
      if (defVal == "1")
      {
        defVal = "Yes";
        return;
      }
      if (defVal == "2")
      {
        defVal = "No";
        return;
      }
      defVal = "";
      return;
label_152:
      if (defVal == "True")
        defVal = "Yes";
      else if (defVal == "False")
        defVal = "No";
      else
        defVal = "";
    }

    private void emailButton_Click(object sender, EventArgs e)
    {
      List<ExternalUserInfo> externalUserInfoList1 = new List<ExternalUserInfo>();
      List<ExternalUserInfo> externalUserInfoList2 = new List<ExternalUserInfo>();
      if (this.gridViewContacts.SelectedItems == null || !this.gridViewContacts.SelectedItems.Any<GVItem>() || this.selectedExternalUsers == null || !this.selectedExternalUsers.Any<ExternalUserInfo>())
        return;
      foreach (ExternalUserInfo selectedExternalUser in this.selectedExternalUsers)
      {
        if (selectedExternalUser.WelcomeEmailDate != DateTime.MinValue)
          externalUserInfoList1.Add(selectedExternalUser);
        else
          externalUserInfoList2.Add(selectedExternalUser);
      }
      if (!externalUserInfoList1.Any<ExternalUserInfo>())
      {
        using (BatchEmail batchEmail = new BatchEmail(this.session, externalUserInfoList2))
        {
          int num = (int) batchEmail.ShowDialog((IWin32Window) this);
        }
      }
      else
      {
        using (BatchEmail batchEmail = new BatchEmail(this.session, externalUserInfoList1, externalUserInfoList2))
        {
          int num = (int) batchEmail.ShowDialog((IWin32Window) this);
        }
      }
      this.RefreshExternalUserInfoTree();
    }

    private void searchBtn_Click(object sender, EventArgs e)
    {
      if (!this.nameRd.Checked && !this.idRd.Checked)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please check search by Name or ID.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.searchTxt.Text.Trim() == "")
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please enter search keyword.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        foreach (TreeNode searchNode in this.searchNodeList)
        {
          searchNode.BackColor = Color.White;
          searchNode.Collapse();
        }
        this.searchCnt.Text = "";
        this.searchNodeList.Clear();
        this.searchCounter = 0;
        List<List<HierarchySummary>> hierarchySummaryListList = this.session.ConfigurationManager.SearchOrganization(this.nameRd.Checked ? "Name" : "Id", this.searchTxt.Text);
        this.currentSelectedTopNode = this.treeTopNodeBroker;
        if (hierarchySummaryListList[1].Any<HierarchySummary>())
        {
          this.hierarchyTree.SelectedNode = (TreeNode) null;
          this.highlightNodes(this.currentSelectedTopNode, hierarchySummaryListList[1], hierarchySummaryListList[0], false);
          if (this.searchNodeList.Any<TreeNode>())
          {
            this.searchCnt.Text = "1 of " + (object) this.searchNodeList.Count;
            this.searchCounter = 1;
            this.clearBtn.Enabled = true;
            this.disableUpDownButtons();
            if (!this.hierarchyTree.SelectedNode.IsVisible)
              this.hierarchyTree.SelectedNode.EnsureVisible();
            this.hierarchyTree.Focus();
          }
        }
        if (hierarchySummaryListList[1].Any<HierarchySummary>() && this.searchNodeList.Any<TreeNode>())
          return;
        this.upBtn.Enabled = false;
        this.downBtn.Enabled = false;
        this.clearBtn.Enabled = false;
        int num3 = (int) Utils.Dialog((IWin32Window) this, "No Organization is found for this search.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void highlightNodes(
      TreeNode root,
      List<HierarchySummary> list,
      List<HierarchySummary> rootList,
      bool flag)
    {
      if (root == null)
        return;
      for (int index = 0; index < root.Nodes.Count; ++index)
      {
        if ((ExtNodeTag) root.Nodes[index].Tag != null)
        {
          ExtNodeTag tag = (ExtNodeTag) root.Nodes[index].Tag;
          if (this.externalOrgsList.Contains(tag.Oid) || tag.AllowAccess || this.isTpoAdmin)
          {
            List<HierarchySummary> list1 = list.Where<HierarchySummary>((Func<HierarchySummary, bool>) (a => a.oid == tag.Oid)).ToList<HierarchySummary>();
            if (list1 != null && list1.Any<HierarchySummary>())
            {
              if (this.hierarchyTree.SelectedNode == null)
                this.hierarchyTree.SelectedNode = root.Nodes[index];
              root.Nodes[index].BackColor = Color.Yellow;
              this.searchNodeList.Add(root.Nodes[index]);
            }
            List<HierarchySummary> list2 = rootList.Where<HierarchySummary>((Func<HierarchySummary, bool>) (a => a.oid == tag.Oid)).ToList<HierarchySummary>();
            if (((list2 == null ? 0 : (list2.Any<HierarchySummary>() ? 1 : 0)) | (flag ? 1 : 0)) != 0)
            {
              root.Nodes[index].Expand();
              this.hierarchyTree.CallBeforeExpand(new TreeViewCancelEventArgs(root.Nodes[index], false, TreeViewAction.Expand));
              flag = true;
              this.highlightNodes(root.Nodes[index], list, rootList, flag);
            }
            flag = false;
          }
        }
      }
    }

    private void upBtn_Click(object sender, EventArgs e)
    {
      if (!this.searchNodeList.Any<TreeNode>() || this.searchCounter <= 1)
        return;
      --this.searchCounter;
      this.hierarchyTree.SelectedNode = this.searchNodeList[this.searchCounter - 1];
      this.searchCnt.Text = this.searchCounter.ToString() + " of " + (object) this.searchNodeList.Count;
      this.disableUpDownButtons();
      this.hierarchyTree.Focus();
    }

    private void downBtn_Click(object sender, EventArgs e)
    {
      if (!this.searchNodeList.Any<TreeNode>() || this.searchCounter == this.searchNodeList.Count)
        return;
      ++this.searchCounter;
      this.hierarchyTree.SelectedNode = this.searchNodeList[this.searchCounter - 1];
      this.searchCnt.Text = this.searchCounter.ToString() + " of " + (object) this.searchNodeList.Count;
      this.disableUpDownButtons();
      this.hierarchyTree.Focus();
    }

    private void disableUpDownButtons()
    {
      if (this.searchCounter <= 1)
        this.upBtn.Enabled = false;
      else
        this.upBtn.Enabled = true;
      if (this.searchCounter == this.searchNodeList.Count)
        this.downBtn.Enabled = false;
      else
        this.downBtn.Enabled = true;
    }

    private void radioButton_CheckedChanged(object sender, EventArgs e) => this.searchTxt.Text = "";

    private void clearBtn_Click(object sender, EventArgs e)
    {
      foreach (TreeNode searchNode in this.searchNodeList)
      {
        searchNode.BackColor = Color.White;
        searchNode.Collapse();
      }
      this.treeTopNodeBroker.Collapse();
      this.searchNodeList.Clear();
      this.searchCounter = 0;
      this.searchCnt.Text = "";
      this.searchTxt.Text = "";
      this.upBtn.Enabled = false;
      this.downBtn.Enabled = false;
      this.clearBtn.Enabled = false;
      this.stdIconBtnEditOrg.Enabled = false;
      this.stdIconBtnDeleteOrg.Enabled = false;
      this.RefreshExternalUserInfoTree();
      this.hierarchyTree.Focus();
      this.hierarchyTree_AfterSelect((object) null, (EventArgs) null);
    }

    private void exportLicensesMenuItem_Click(object sender, EventArgs e)
    {
      CsvImportParameters importParameters = new CsvImportParameters(EllieMae.EMLite.ContactUI.ContactType.TPOExportLicenses);
      ExcelHandler excelExport = new ExcelHandler();
      CsvImportColumn[] availableColumns = importParameters.GetAllAvailableColumns();
      excelExport.Headers = ((IEnumerable<CsvImportColumn>) availableColumns).Select<CsvImportColumn, string>((Func<CsvImportColumn, string>) (a => a.Description)).ToArray<string>();
      string[] properties = ((IEnumerable<CsvImportColumn>) availableColumns).Select<CsvImportColumn, string>((Func<CsvImportColumn, string>) (a => a.PropertyName)).ToArray<string>();
      TreeNode selectedNode = this.hierarchyTree.SelectedNode;
      excelExport.FileName = selectedNode.Text;
      if (selectedNode != null)
      {
        BranchExtLicensing exportLicensesDetails = this.session.ConfigurationManager.GetExportLicensesDetails(((ExtNodeTag) selectedNode.Tag).Oid);
        if (exportLicensesDetails == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "No license found for this organization.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
          return;
        }
        Dictionary<string, string> mapDict = new ExportMap().GetTPOExportExpensesMap();
        exportLicensesDetails.StateLicenseExtTypes.ForEach((Action<StateLicenseExtType>) (stateLicenseExtType =>
        {
          List<string> stringList = new List<string>();
          foreach (string str in properties)
          {
            string defVal = CompanySetupForm.GetPropValue((object) stateLicenseExtType, mapDict[str]).ToString();
            this.processTPOExportLicensesValues(str, ref defVal, stateLicenseExtType);
            stringList.Add(Convert.ToString(defVal));
          }
          excelExport.AddDataRow(stringList.ToArray());
        }));
      }
      excelExport.Export();
    }

    private void processTPOExportLicensesValues(
      string propertyName,
      ref string defVal,
      StateLicenseExtType stateLicenseExtType)
    {
      switch (propertyName.ToLower())
      {
        case "licenseexempt":
          defVal = stateLicenseExtType.Exempt ? "Exempt" : string.Empty;
          break;
        case "issuedate":
        case "startdate":
        case "enddate":
        case "statusdate":
        case "lastcheckeddate":
          if (Convert.ToDateTime(defVal) == DateTime.MinValue)
          {
            defVal = "";
            break;
          }
          defVal = Utils.ParseDate((object) defVal).Date.ToString("MM/dd/yyyy");
          break;
      }
    }

    private void gridViewContacts_MouseClick(object sender, MouseEventArgs e)
    {
      if (this.gridViewContacts.SelectedItems.Count != 1)
        return;
      int rowIndex = this.gridViewContacts.HitTest(new Point(e.X, e.Y)).RowIndex;
      int columnIndex = this.gridViewContacts.HitTest(new Point(e.X, e.Y)).ColumnIndex;
      if (rowIndex == -1 || columnIndex == -1 || e.Button != MouseButtons.Right)
        return;
      Point client = this.gridViewContacts.PointToClient(Cursor.Position);
      this.tpoEditContactMenuItem.Enabled = this.tpoMoveContactMenuItem.Enabled = this.hasContactEditRight;
      this.tpoExportContactMenuItem.Enabled = this.hasContactExportRight;
      this.tpoDeleteContactMenuItem.Enabled = this.hasContactDeleteRight;
      this.tpoSendWelcomeEmailMenuItem.Enabled = this.hasSendWelcomeEmailRight;
      this.contextMenuTPOContacts.Show((Control) this.gridViewContacts, client);
    }

    private void tpoEditContactMenuItem_Click(object sender, EventArgs e)
    {
      this.btnEditContact_Click(sender, e);
    }

    private void tpoMoveContactMenuItem_Click(object sender, EventArgs e)
    {
      this.standardIconButton1_Click(sender, e);
    }

    private void tpoDeleteContactMenuItem_Click(object sender, EventArgs e)
    {
      this.btnDeleteContact_Click(sender, e);
    }

    private void tpoExportContactMenuItem_Click(object sender, EventArgs e)
    {
      this.btnExport_Click(sender, e);
    }

    private void tpoSendWelcomeEmailMenuItem_Click(object sender, EventArgs e)
    {
      this.emailButton_Click(sender, e);
    }

    private void tpoContactExportLicencesMenuItem_Click(object sender, EventArgs e)
    {
      if (this.gridViewContacts.SelectedItems.Count == 0)
        return;
      ExternalUserInfo tag = (ExternalUserInfo) this.gridViewContacts.SelectedItems[0].Tag;
      if (tag == null)
        return;
      BranchExtLicensing extLicenseDetails = this.session.ConfigurationManager.GetExtLicenseDetails(tag.ExternalUserID);
      List<StateLicenseExtType> list = extLicenseDetails.StateLicenseExtTypes.Where<StateLicenseExtType>((Func<StateLicenseExtType, bool>) (x => x.Approved)).ToList<StateLicenseExtType>();
      if (extLicenseDetails == null || list.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "No license found for the selected user.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
      }
      else
      {
        List<string> excludedColumns = new List<string>();
        excludedColumns.Add("LicenseType");
        CsvImportParameters importParameters = new CsvImportParameters(EllieMae.EMLite.ContactUI.ContactType.TPOExportLicenses);
        ExcelHandler excelExport = new ExcelHandler();
        CsvImportColumn[] availableColumns = importParameters.GetAllAvailableColumns();
        excelExport.Headers = ((IEnumerable<CsvImportColumn>) availableColumns).Where<CsvImportColumn>((Func<CsvImportColumn, bool>) (a => !excludedColumns.Contains(a.PropertyName))).Select<CsvImportColumn, string>((Func<CsvImportColumn, string>) (a => a.Description)).ToArray<string>();
        string[] properties = ((IEnumerable<CsvImportColumn>) availableColumns).Where<CsvImportColumn>((Func<CsvImportColumn, bool>) (a => !excludedColumns.Contains(a.PropertyName))).Select<CsvImportColumn, string>((Func<CsvImportColumn, string>) (a => a.PropertyName)).ToArray<string>();
        Dictionary<string, string> mapDict = new ExportMap().GetTPOExportExpensesMap();
        list.ForEach((Action<StateLicenseExtType>) (stateLicenseExtType =>
        {
          List<string> stringList = new List<string>();
          foreach (string str in properties)
          {
            string defVal = CompanySetupForm.GetPropValue((object) stateLicenseExtType, mapDict[str]).ToString();
            this.processTPOExportLicensesValues(str, ref defVal, stateLicenseExtType);
            stringList.Add(Convert.ToString(defVal));
          }
          excelExport.AddDataRow(stringList.ToArray());
        }));
        excelExport.Export();
      }
    }

    private bool isPersonaAssignedtoTPOLoanOfficerOrTPOLoanProcessor(
      Persona persona,
      ExternalUserInfo user,
      out List<int> personaIds)
    {
      bool flag1 = false;
      personaIds = new List<int>();
      RolesMappingInfo[] roleMappingInfos = this.workflowMgr.GetAllRoleMappingInfos();
      RolesMappingInfo rolesMappingInfo1 = ((IEnumerable<RolesMappingInfo>) roleMappingInfos).Where<RolesMappingInfo>((Func<RolesMappingInfo, bool>) (x => x.RealWorldRoleID == RealWorldRoleID.TPOLoanOfficer)).SingleOrDefault<RolesMappingInfo>();
      RoleInfo roleInfo1 = (RoleInfo) null;
      if (rolesMappingInfo1 != null)
      {
        roleInfo1 = this.workflowMgr.GetRoleFunction(rolesMappingInfo1.RoleIDList[0]);
        if (((IEnumerable<int>) roleInfo1.PersonaIDs).Contains<int>(persona.ID))
          flag1 = true;
      }
      RolesMappingInfo rolesMappingInfo2 = ((IEnumerable<RolesMappingInfo>) roleMappingInfos).Where<RolesMappingInfo>((Func<RolesMappingInfo, bool>) (x => x.RealWorldRoleID == RealWorldRoleID.TPOLoanProcessor)).SingleOrDefault<RolesMappingInfo>();
      RoleInfo roleInfo2 = (RoleInfo) null;
      if (rolesMappingInfo2 != null)
      {
        roleInfo2 = this.workflowMgr.GetRoleFunction(rolesMappingInfo2.RoleIDList[0]);
        if (((IEnumerable<int>) roleInfo2.PersonaIDs).Contains<int>(persona.ID))
          flag1 = true;
      }
      if (flag1)
      {
        bool flag2 = false;
        bool flag3 = false;
        foreach (Persona userPersona in user.UserPersonas)
        {
          if (roleInfo1 != null && ((IEnumerable<int>) roleInfo1.PersonaIDs).Contains<int>(userPersona.ID))
            flag2 = true;
          if (roleInfo2 != null && ((IEnumerable<int>) roleInfo2.PersonaIDs).Contains<int>(userPersona.ID))
            flag3 = true;
        }
        if (flag2 & flag3)
        {
          personaIds.AddRange((IEnumerable<int>) roleInfo1.PersonaIDs);
        }
        else
        {
          if (flag2)
            personaIds.AddRange((IEnumerable<int>) roleInfo1.PersonaIDs);
          if (flag3)
            personaIds.AddRange((IEnumerable<int>) roleInfo2.PersonaIDs);
        }
      }
      return flag1;
    }

    private void btnAutoOrgNumbering_Click(object sender, EventArgs e)
    {
      using (AutoOrgIdNumberingDialog idNumberingDialog = new AutoOrgIdNumberingDialog(this.session))
      {
        int num = (int) idNumberingDialog.ShowDialog((IWin32Window) this);
      }
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CompanySetupForm));
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
      GVColumn gvColumn13 = new GVColumn();
      GVColumn gvColumn14 = new GVColumn();
      this.toolTip1 = new ToolTip(this.components);
      this.stdExport = new StandardIconButton();
      this.btnImportOrg = new StandardIconButton();
      this.stdIconBtnDeleteOrg = new StandardIconButton();
      this.stdIconBtnEditOrg = new StandardIconButton();
      this.stdIconBtnNewOrg = new StandardIconButton();
      this.emailButton = new StandardIconButton();
      this.stnIconMove = new StandardIconButton();
      this.btnImportContact = new StandardIconButton();
      this.btnExport = new StandardIconButton();
      this.btnDeleteContact = new StandardIconButton();
      this.btnEditContact = new StandardIconButton();
      this.btnAddContact = new StandardIconButton();
      this.contextMenuOrg = new ContextMenu();
      this.addOrgMenuItem = new MenuItem();
      this.editOrgMenuItem = new MenuItem();
      this.delOrgMenuItem = new MenuItem();
      this.menuItem2 = new MenuItem();
      this.menuItem1 = new MenuItem();
      this.exportLicensesMenuItem = new MenuItem();
      this.imgListTv = new ImageList(this.components);
      this.panelBottom = new Panel();
      this.grpContacts = new GroupContainer();
      this.gridViewContacts = new GridView();
      this.panelTop = new Panel();
      this.gcOrg = new GroupContainer();
      this.hierarchyTree = new ExternalHierarchyTree();
      this.gradientPanel1 = new GradientPanel();
      this.btnAutoOrgNumbering = new Button();
      this.searchCnt = new Label();
      this.idRd = new RadioButton();
      this.nameRd = new RadioButton();
      this.clearBtn = new Button();
      this.downBtn = new Button();
      this.upBtn = new Button();
      this.searchBtn = new StandardIconButton();
      this.searchTxt = new TextBox();
      this.label1 = new Label();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.contextMenuTPOContacts = new ContextMenuStrip(this.components);
      this.tpoEditContactMenuItem = new ToolStripMenuItem();
      this.tpoMoveContactMenuItem = new ToolStripMenuItem();
      this.tpoDeleteContactMenuItem = new ToolStripMenuItem();
      this.tpoExportContactMenuItem = new ToolStripMenuItem();
      this.tpoContactExportLicencesMenuItem = new ToolStripMenuItem();
      this.tpoSendWelcomeEmailMenuItem = new ToolStripMenuItem();
      ((ISupportInitialize) this.stdExport).BeginInit();
      ((ISupportInitialize) this.btnImportOrg).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDeleteOrg).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEditOrg).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNewOrg).BeginInit();
      ((ISupportInitialize) this.emailButton).BeginInit();
      ((ISupportInitialize) this.stnIconMove).BeginInit();
      ((ISupportInitialize) this.btnImportContact).BeginInit();
      ((ISupportInitialize) this.btnExport).BeginInit();
      ((ISupportInitialize) this.btnDeleteContact).BeginInit();
      ((ISupportInitialize) this.btnEditContact).BeginInit();
      ((ISupportInitialize) this.btnAddContact).BeginInit();
      this.panelBottom.SuspendLayout();
      this.grpContacts.SuspendLayout();
      this.panelTop.SuspendLayout();
      this.gcOrg.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      ((ISupportInitialize) this.searchBtn).BeginInit();
      this.contextMenuTPOContacts.SuspendLayout();
      this.SuspendLayout();
      this.stdExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdExport.BackColor = Color.Transparent;
      this.stdExport.Location = new Point(689, 5);
      this.stdExport.MouseDownImage = (Image) null;
      this.stdExport.Name = "stdExport";
      this.stdExport.Size = new Size(16, 18);
      this.stdExport.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.stdExport.TabIndex = 13;
      this.stdExport.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdExport, "Export Organizations");
      this.stdExport.Click += new EventHandler(this.stdExport_Click);
      this.btnImportOrg.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnImportOrg.BackColor = Color.Transparent;
      this.btnImportOrg.Location = new Point(709, 5);
      this.btnImportOrg.MouseDownImage = (Image) null;
      this.btnImportOrg.Name = "btnImportOrg";
      this.btnImportOrg.Size = new Size(16, 18);
      this.btnImportOrg.StandardButtonType = StandardIconButton.ButtonType.ImportLoanButton;
      this.btnImportOrg.TabIndex = 12;
      this.btnImportOrg.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnImportOrg, "Import Companies and Branches");
      this.btnImportOrg.Click += new EventHandler(this.btnImportOrg_Click);
      this.stdIconBtnDeleteOrg.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDeleteOrg.BackColor = Color.Transparent;
      this.stdIconBtnDeleteOrg.Location = new Point(667, 5);
      this.stdIconBtnDeleteOrg.MouseDownImage = (Image) null;
      this.stdIconBtnDeleteOrg.Name = "stdIconBtnDeleteOrg";
      this.stdIconBtnDeleteOrg.Size = new Size(16, 18);
      this.stdIconBtnDeleteOrg.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDeleteOrg.TabIndex = 3;
      this.stdIconBtnDeleteOrg.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDeleteOrg, "Delete Organization");
      this.stdIconBtnDeleteOrg.Click += new EventHandler(this.stdIconBtnDeleteOrg_Click);
      this.stdIconBtnEditOrg.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEditOrg.BackColor = Color.Transparent;
      this.stdIconBtnEditOrg.Location = new Point(645, 5);
      this.stdIconBtnEditOrg.MouseDownImage = (Image) null;
      this.stdIconBtnEditOrg.Name = "stdIconBtnEditOrg";
      this.stdIconBtnEditOrg.Size = new Size(16, 18);
      this.stdIconBtnEditOrg.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEditOrg.TabIndex = 2;
      this.stdIconBtnEditOrg.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEditOrg, "Edit Organization");
      this.stdIconBtnEditOrg.Click += new EventHandler(this.stdIconBtnEditOrg_Click);
      this.stdIconBtnNewOrg.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNewOrg.BackColor = Color.Transparent;
      this.stdIconBtnNewOrg.Location = new Point(623, 5);
      this.stdIconBtnNewOrg.MouseDownImage = (Image) null;
      this.stdIconBtnNewOrg.Name = "stdIconBtnNewOrg";
      this.stdIconBtnNewOrg.Size = new Size(16, 18);
      this.stdIconBtnNewOrg.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNewOrg.TabIndex = 1;
      this.stdIconBtnNewOrg.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNewOrg, "Add Organization");
      this.stdIconBtnNewOrg.Click += new EventHandler(this.stdIconBtnNewOrg_Click);
      this.emailButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.emailButton.BackColor = Color.Transparent;
      this.emailButton.Location = new Point(712, 7);
      this.emailButton.MouseDownImage = (Image) null;
      this.emailButton.Name = "emailButton";
      this.emailButton.Size = new Size(16, 16);
      this.emailButton.StandardButtonType = StandardIconButton.ButtonType.EmailButton;
      this.emailButton.TabIndex = 13;
      this.emailButton.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.emailButton, "Send Welcome Email");
      this.emailButton.Click += new EventHandler(this.emailButton_Click);
      this.stnIconMove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stnIconMove.BackColor = Color.Transparent;
      this.stnIconMove.Location = new Point(628, 6);
      this.stnIconMove.MouseDownImage = (Image) null;
      this.stnIconMove.Name = "stnIconMove";
      this.stnIconMove.Size = new Size(16, 16);
      this.stnIconMove.StandardButtonType = StandardIconButton.ButtonType.MoveButton;
      this.stnIconMove.TabIndex = 12;
      this.stnIconMove.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stnIconMove, "Move Contacts to different organization");
      this.stnIconMove.Click += new EventHandler(this.standardIconButton1_Click);
      this.btnImportContact.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnImportContact.BackColor = Color.Transparent;
      this.btnImportContact.Location = new Point(670, 6);
      this.btnImportContact.MouseDownImage = (Image) null;
      this.btnImportContact.Name = "btnImportContact";
      this.btnImportContact.Size = new Size(16, 18);
      this.btnImportContact.StandardButtonType = StandardIconButton.ButtonType.ImportLoanButton;
      this.btnImportContact.TabIndex = 11;
      this.btnImportContact.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnImportContact, "Import Contacts");
      this.btnImportContact.Click += new EventHandler(this.btnImportContact_Click);
      this.btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExport.BackColor = Color.Transparent;
      this.btnExport.Location = new Point(691, 6);
      this.btnExport.MouseDownImage = (Image) null;
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(16, 18);
      this.btnExport.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExport.TabIndex = 4;
      this.btnExport.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnExport, "Export Contacts");
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.btnDeleteContact.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteContact.BackColor = Color.Transparent;
      this.btnDeleteContact.Location = new Point(650, 6);
      this.btnDeleteContact.MouseDownImage = (Image) null;
      this.btnDeleteContact.Name = "btnDeleteContact";
      this.btnDeleteContact.Size = new Size(16, 18);
      this.btnDeleteContact.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteContact.TabIndex = 3;
      this.btnDeleteContact.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDeleteContact, "Delete Contact");
      this.btnDeleteContact.Click += new EventHandler(this.btnDeleteContact_Click);
      this.btnEditContact.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditContact.BackColor = Color.Transparent;
      this.btnEditContact.Location = new Point(606, 6);
      this.btnEditContact.MouseDownImage = (Image) null;
      this.btnEditContact.Name = "btnEditContact";
      this.btnEditContact.Size = new Size(16, 18);
      this.btnEditContact.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditContact.TabIndex = 2;
      this.btnEditContact.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEditContact, "Edit Contact");
      this.btnEditContact.Click += new EventHandler(this.btnEditContact_Click);
      this.btnAddContact.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddContact.BackColor = Color.Transparent;
      this.btnAddContact.Location = new Point(584, 6);
      this.btnAddContact.MouseDownImage = (Image) null;
      this.btnAddContact.Name = "btnAddContact";
      this.btnAddContact.Size = new Size(16, 18);
      this.btnAddContact.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddContact.TabIndex = 1;
      this.btnAddContact.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddContact, "Add Contact");
      this.btnAddContact.Click += new EventHandler(this.btnAddContact_Click);
      this.contextMenuOrg.MenuItems.AddRange(new MenuItem[6]
      {
        this.addOrgMenuItem,
        this.editOrgMenuItem,
        this.delOrgMenuItem,
        this.menuItem2,
        this.menuItem1,
        this.exportLicensesMenuItem
      });
      this.addOrgMenuItem.Index = 0;
      this.addOrgMenuItem.Text = "Add Organization";
      this.addOrgMenuItem.Click += new EventHandler(this.stdIconBtnNewOrg_Click);
      this.editOrgMenuItem.Index = 1;
      this.editOrgMenuItem.Text = "Edit Organization";
      this.editOrgMenuItem.Click += new EventHandler(this.stdIconBtnEditOrg_Click);
      this.delOrgMenuItem.Index = 2;
      this.delOrgMenuItem.Text = "Delete Organization";
      this.delOrgMenuItem.Click += new EventHandler(this.stdIconBtnDeleteOrg_Click);
      this.menuItem2.Index = 3;
      this.menuItem2.Text = "Export Organizations";
      this.menuItem2.Click += new EventHandler(this.stdExport_Click);
      this.menuItem1.Index = 4;
      this.menuItem1.Text = "Import Organizations";
      this.menuItem1.Click += new EventHandler(this.btnImportOrg_Click);
      this.exportLicensesMenuItem.Index = 5;
      this.exportLicensesMenuItem.Text = "Export Licenses";
      this.exportLicensesMenuItem.Click += new EventHandler(this.exportLicensesMenuItem_Click);
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.Transparent;
      this.imgListTv.Images.SetKeyName(0, "folder.bmp");
      this.imgListTv.Images.SetKeyName(1, "folder-open.bmp");
      this.panelBottom.Controls.Add((Control) this.grpContacts);
      this.panelBottom.Dock = DockStyle.Bottom;
      this.panelBottom.Location = new Point(0, 340);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new Size(730, 203);
      this.panelBottom.TabIndex = 11;
      this.grpContacts.Controls.Add((Control) this.emailButton);
      this.grpContacts.Controls.Add((Control) this.stnIconMove);
      this.grpContacts.Controls.Add((Control) this.btnImportContact);
      this.grpContacts.Controls.Add((Control) this.gridViewContacts);
      this.grpContacts.Controls.Add((Control) this.btnExport);
      this.grpContacts.Controls.Add((Control) this.btnDeleteContact);
      this.grpContacts.Controls.Add((Control) this.btnEditContact);
      this.grpContacts.Controls.Add((Control) this.btnAddContact);
      this.grpContacts.Dock = DockStyle.Fill;
      this.grpContacts.HeaderForeColor = SystemColors.ControlText;
      this.grpContacts.Location = new Point(0, 0);
      this.grpContacts.Margin = new Padding(0);
      this.grpContacts.Name = "grpContacts";
      this.grpContacts.Size = new Size(730, 203);
      this.grpContacts.TabIndex = 10;
      this.grpContacts.Text = "Third Party Originator Contacts - Manage the TPO contacts that use your TPO WebCenter site";
      this.gridViewContacts.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnLastName";
      gvColumn1.Text = "Last Name";
      gvColumn1.Width = 120;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnFirstName";
      gvColumn2.Text = "First Name";
      gvColumn2.Width = 120;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnRoles";
      gvColumn3.Text = "Roles";
      gvColumn3.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnTitle";
      gvColumn4.Text = "Title";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnState";
      gvColumn5.Text = "State";
      gvColumn5.Width = 40;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColumnEmail";
      gvColumn6.Text = "Email";
      gvColumn6.Width = 120;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ColumnPhone";
      gvColumn7.Text = "Phone";
      gvColumn7.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "ColumnStatus";
      gvColumn8.Text = "Status";
      gvColumn8.Width = 60;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "ColumnNMLSID";
      gvColumn9.Text = "NMLS ID";
      gvColumn9.Width = 100;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "ColumnNMLSCurrent";
      gvColumn10.Text = "NMLS Current";
      gvColumn10.Width = 100;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "ColumnLastLogin";
      gvColumn11.SortMethod = GVSortMethod.DateTime;
      gvColumn11.Text = "Last Login";
      gvColumn11.Width = 100;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "ColumnLogin";
      gvColumn12.Text = "Login";
      gvColumn12.Width = 60;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "ColumnSalesRep";
      gvColumn13.Text = "Sales Rep";
      gvColumn13.Width = 150;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "ColumnWelcomeEmail";
      gvColumn14.Text = "Welcome Email Sent";
      gvColumn14.Width = 130;
      this.gridViewContacts.Columns.AddRange(new GVColumn[14]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10,
        gvColumn11,
        gvColumn12,
        gvColumn13,
        gvColumn14
      });
      this.gridViewContacts.Dock = DockStyle.Fill;
      this.gridViewContacts.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewContacts.Location = new Point(1, 26);
      this.gridViewContacts.Name = "gridViewContacts";
      this.gridViewContacts.Size = new Size(728, 176);
      this.gridViewContacts.TabIndex = 10;
      this.gridViewContacts.SelectedIndexChanged += new EventHandler(this.gridViewContacts_SelectedIndexChanged);
      this.gridViewContacts.ItemDoubleClick += new GVItemEventHandler(this.gridViewContacts_ItemDoubleClick);
      this.gridViewContacts.ItemDrag += new GVItemEventHandler(this.gridViewContacts_ItemDrag);
      this.gridViewContacts.DragOver += new DragEventHandler(this.gridViewContacts_DragOver);
      this.gridViewContacts.MouseClick += new MouseEventHandler(this.gridViewContacts_MouseClick);
      this.panelTop.Controls.Add((Control) this.gcOrg);
      this.panelTop.Dock = DockStyle.Fill;
      this.panelTop.Location = new Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(730, 333);
      this.panelTop.TabIndex = 13;
      this.gcOrg.Controls.Add((Control) this.stdExport);
      this.gcOrg.Controls.Add((Control) this.btnImportOrg);
      this.gcOrg.Controls.Add((Control) this.stdIconBtnDeleteOrg);
      this.gcOrg.Controls.Add((Control) this.stdIconBtnEditOrg);
      this.gcOrg.Controls.Add((Control) this.stdIconBtnNewOrg);
      this.gcOrg.Controls.Add((Control) this.hierarchyTree);
      this.gcOrg.Controls.Add((Control) this.gradientPanel1);
      this.gcOrg.Dock = DockStyle.Fill;
      this.gcOrg.HeaderForeColor = SystemColors.ControlText;
      this.gcOrg.Location = new Point(0, 0);
      this.gcOrg.Margin = new Padding(0);
      this.gcOrg.Name = "gcOrg";
      this.gcOrg.Size = new Size(730, 333);
      this.gcOrg.TabIndex = 8;
      this.gcOrg.Text = "Company/Branches";
      this.hierarchyTree.AllowDrop = true;
      this.hierarchyTree.BorderStyle = BorderStyle.None;
      this.hierarchyTree.BrokerRootOrgsList = (List<ExternalOriginatorManagementData>) null;
      this.hierarchyTree.ContextMenu = this.contextMenuOrg;
      this.hierarchyTree.Cursor = Cursors.Default;
      this.hierarchyTree.Dock = DockStyle.Fill;
      this.hierarchyTree.ExternalCompanyList = (List<HierarchySummary>[]) null;
      this.hierarchyTree.ExternalOrgsList = (List<int>) null;
      this.hierarchyTree.FullRowSelect = true;
      this.hierarchyTree.HideSelection = false;
      this.hierarchyTree.ImageIndex = 0;
      this.hierarchyTree.ImageList = this.imgListTv;
      this.hierarchyTree.IsTpoAdmin = false;
      this.hierarchyTree.Location = new Point(1, 63);
      this.hierarchyTree.Margin = new Padding(0);
      this.hierarchyTree.Name = "hierarchyTree";
      this.hierarchyTree.SelectedImageIndex = 0;
      this.hierarchyTree.Size = new Size(728, 269);
      this.hierarchyTree.Sorted = true;
      this.hierarchyTree.TabIndex = 0;
      this.hierarchyTree.BeforeExpand += new TreeViewCancelEventHandler(this.hierarchyTree_BeforeExpand);
      this.hierarchyTree.DragOver += new DragEventHandler(this.hierarchyTree_DragOver);
      this.gradientPanel1.Controls.Add((Control) this.btnAutoOrgNumbering);
      this.gradientPanel1.Controls.Add((Control) this.searchCnt);
      this.gradientPanel1.Controls.Add((Control) this.idRd);
      this.gradientPanel1.Controls.Add((Control) this.nameRd);
      this.gradientPanel1.Controls.Add((Control) this.clearBtn);
      this.gradientPanel1.Controls.Add((Control) this.downBtn);
      this.gradientPanel1.Controls.Add((Control) this.upBtn);
      this.gradientPanel1.Controls.Add((Control) this.searchBtn);
      this.gradientPanel1.Controls.Add((Control) this.searchTxt);
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 26);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(728, 37);
      this.gradientPanel1.TabIndex = 15;
      this.btnAutoOrgNumbering.Location = new Point(644, 9);
      this.btnAutoOrgNumbering.Name = "btnAutoOrgNumbering";
      this.btnAutoOrgNumbering.Size = new Size(125, 23);
      this.btnAutoOrgNumbering.TabIndex = 17;
      this.btnAutoOrgNumbering.Text = "Auto Org ID Numbering";
      this.btnAutoOrgNumbering.UseVisualStyleBackColor = true;
      this.btnAutoOrgNumbering.Click += new EventHandler(this.btnAutoOrgNumbering_Click);
      this.searchCnt.AutoSize = true;
      this.searchCnt.BackColor = Color.Transparent;
      this.searchCnt.Location = new Point(390, 15);
      this.searchCnt.Name = "searchCnt";
      this.searchCnt.Size = new Size(0, 14);
      this.searchCnt.TabIndex = 16;
      this.idRd.AutoSize = true;
      this.idRd.BackColor = Color.Transparent;
      this.idRd.Location = new Point(213, 11);
      this.idRd.Name = "idRd";
      this.idRd.Size = new Size(34, 18);
      this.idRd.TabIndex = 10;
      this.idRd.TabStop = true;
      this.idRd.Text = "ID";
      this.idRd.UseVisualStyleBackColor = false;
      this.idRd.CheckedChanged += new EventHandler(this.radioButton_CheckedChanged);
      this.nameRd.AutoSize = true;
      this.nameRd.BackColor = Color.Transparent;
      this.nameRd.Checked = true;
      this.nameRd.Location = new Point(159, 11);
      this.nameRd.Name = "nameRd";
      this.nameRd.Size = new Size(52, 18);
      this.nameRd.TabIndex = 9;
      this.nameRd.TabStop = true;
      this.nameRd.Text = "Name";
      this.nameRd.UseVisualStyleBackColor = false;
      this.nameRd.CheckedChanged += new EventHandler(this.radioButton_CheckedChanged);
      this.clearBtn.Enabled = false;
      this.clearBtn.Location = new Point(563, 9);
      this.clearBtn.Name = "clearBtn";
      this.clearBtn.Size = new Size(75, 23);
      this.clearBtn.TabIndex = 15;
      this.clearBtn.Text = "Clear";
      this.clearBtn.UseVisualStyleBackColor = true;
      this.clearBtn.Click += new EventHandler(this.clearBtn_Click);
      this.downBtn.Enabled = false;
      this.downBtn.Location = new Point(498, 9);
      this.downBtn.Name = "downBtn";
      this.downBtn.Size = new Size(59, 23);
      this.downBtn.TabIndex = 14;
      this.downBtn.Text = "Down";
      this.downBtn.UseVisualStyleBackColor = true;
      this.downBtn.Click += new EventHandler(this.downBtn_Click);
      this.upBtn.Enabled = false;
      this.upBtn.Location = new Point(439, 9);
      this.upBtn.Name = "upBtn";
      this.upBtn.Size = new Size(52, 23);
      this.upBtn.TabIndex = 13;
      this.upBtn.Text = "Up";
      this.upBtn.UseVisualStyleBackColor = true;
      this.upBtn.Click += new EventHandler(this.upBtn_Click);
      this.searchBtn.BackColor = Color.Transparent;
      this.searchBtn.Location = new Point(358, 12);
      this.searchBtn.MouseDownImage = (Image) null;
      this.searchBtn.Name = "searchBtn";
      this.searchBtn.Size = new Size(16, 16);
      this.searchBtn.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.searchBtn.TabIndex = 12;
      this.searchBtn.TabStop = false;
      this.searchBtn.Click += new EventHandler(this.searchBtn_Click);
      this.searchTxt.Location = new Point(251, 10);
      this.searchTxt.Name = "searchTxt";
      this.searchTxt.Size = new Size(100, 20);
      this.searchTxt.TabIndex = 11;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(7, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(153, 14);
      this.label1.TabIndex = 8;
      this.label1.Text = "Search TPOs by Organization:";
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.panelBottom;
      this.collapsibleSplitter1.Dock = DockStyle.Bottom;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(0, 333);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 12;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.contextMenuTPOContacts.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.tpoEditContactMenuItem,
        (ToolStripItem) this.tpoMoveContactMenuItem,
        (ToolStripItem) this.tpoDeleteContactMenuItem,
        (ToolStripItem) this.tpoExportContactMenuItem,
        (ToolStripItem) this.tpoContactExportLicencesMenuItem,
        (ToolStripItem) this.tpoSendWelcomeEmailMenuItem
      });
      this.contextMenuTPOContacts.Name = "contextMenuTPOContacts";
      this.contextMenuTPOContacts.Size = new Size(186, 136);
      this.tpoEditContactMenuItem.Name = "tpoEditContactMenuItem";
      this.tpoEditContactMenuItem.Size = new Size(185, 22);
      this.tpoEditContactMenuItem.Text = "Edit Contact";
      this.tpoEditContactMenuItem.Click += new EventHandler(this.tpoEditContactMenuItem_Click);
      this.tpoMoveContactMenuItem.Name = "tpoMoveContactMenuItem";
      this.tpoMoveContactMenuItem.Size = new Size(185, 22);
      this.tpoMoveContactMenuItem.Text = "Move Contact";
      this.tpoMoveContactMenuItem.Click += new EventHandler(this.tpoMoveContactMenuItem_Click);
      this.tpoDeleteContactMenuItem.Name = "tpoDeleteContactMenuItem";
      this.tpoDeleteContactMenuItem.Size = new Size(185, 22);
      this.tpoDeleteContactMenuItem.Text = "Delete Contact";
      this.tpoDeleteContactMenuItem.Click += new EventHandler(this.tpoDeleteContactMenuItem_Click);
      this.tpoExportContactMenuItem.Name = "tpoExportContactMenuItem";
      this.tpoExportContactMenuItem.Size = new Size(185, 22);
      this.tpoExportContactMenuItem.Text = "Export Contact";
      this.tpoExportContactMenuItem.Click += new EventHandler(this.tpoExportContactMenuItem_Click);
      this.tpoContactExportLicencesMenuItem.Name = "tpoContactExportLicencesMenuItem";
      this.tpoContactExportLicencesMenuItem.Size = new Size(185, 22);
      this.tpoContactExportLicencesMenuItem.Text = "Export Licenses";
      this.tpoContactExportLicencesMenuItem.Click += new EventHandler(this.tpoContactExportLicencesMenuItem_Click);
      this.tpoSendWelcomeEmailMenuItem.Name = "tpoSendWelcomeEmailMenuItem";
      this.tpoSendWelcomeEmailMenuItem.Size = new Size(185, 22);
      this.tpoSendWelcomeEmailMenuItem.Text = "Send Welcome Email";
      this.tpoSendWelcomeEmailMenuItem.Click += new EventHandler(this.tpoSendWelcomeEmailMenuItem_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(730, 543);
      this.Controls.Add((Control) this.panelTop);
      this.Controls.Add((Control) this.collapsibleSplitter1);
      this.Controls.Add((Control) this.panelBottom);
      this.Font = new Font("Arial", 8.25f);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (CompanySetupForm);
      this.Text = nameof (CompanySetupForm);
      ((ISupportInitialize) this.stdExport).EndInit();
      ((ISupportInitialize) this.btnImportOrg).EndInit();
      ((ISupportInitialize) this.stdIconBtnDeleteOrg).EndInit();
      ((ISupportInitialize) this.stdIconBtnEditOrg).EndInit();
      ((ISupportInitialize) this.stdIconBtnNewOrg).EndInit();
      ((ISupportInitialize) this.emailButton).EndInit();
      ((ISupportInitialize) this.stnIconMove).EndInit();
      ((ISupportInitialize) this.btnImportContact).EndInit();
      ((ISupportInitialize) this.btnExport).EndInit();
      ((ISupportInitialize) this.btnDeleteContact).EndInit();
      ((ISupportInitialize) this.btnEditContact).EndInit();
      ((ISupportInitialize) this.btnAddContact).EndInit();
      this.panelBottom.ResumeLayout(false);
      this.grpContacts.ResumeLayout(false);
      this.panelTop.ResumeLayout(false);
      this.gcOrg.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      ((ISupportInitialize) this.searchBtn).EndInit();
      this.contextMenuTPOContacts.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}

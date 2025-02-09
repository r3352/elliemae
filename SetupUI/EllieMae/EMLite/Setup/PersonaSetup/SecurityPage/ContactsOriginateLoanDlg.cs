// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.ContactsOriginateLoanDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.PersonaSetup.SecurityPage
{
  public class ContactsOriginateLoanDlg : Form
  {
    private FeaturesAclManager _featuresAclManager;
    private int _personaID = -1;
    private Sessions.Session _session;
    private bool _isSaving;
    private bool _isCancellingSave;
    private bool _isValidSelections;
    private bool _dirty;
    private bool _isInit;
    private ContactsOriginateLoanSecurityHelper _securityHelper;
    private ArrayList _previousView = new ArrayList();
    private bool _readOnly;
    private IContainer components;
    private Button btnCancel;
    private Button btnSave;
    private TreeView tvOptions;
    private Label lblHeader;
    private Label lblNotLinkedWithPersonaRights;
    private Label lblLinkedWithPersonaRights;
    protected ImageList imgListTv;

    public event ContactsOriginateLoanDlg.OriginateLoanFeatureStatusChanged CreateLoanStatusChanged;

    public event PersonaAccess HasPipelineLoanTabAccessEvent;

    private ContactsOriginateLoanDlg(Sessions.Session session, bool readOnly)
    {
      this._session = session;
      this._readOnly = readOnly;
      this.InitializeComponent();
      this._featuresAclManager = (FeaturesAclManager) this._session.ACL.GetAclManager(AclCategory.Features);
    }

    public ContactsOriginateLoanDlg(Sessions.Session session, int personaID, bool readOnly)
      : this(session, readOnly)
    {
      this._personaID = personaID;
      this._securityHelper = new ContactsOriginateLoanSecurityHelper(this._session, personaID);
      this._isInit = true;
      this.InitForPersona();
      this.SetView();
      this._isInit = false;
    }

    public ContactsOriginateLoanDlg(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      bool readOnly)
      : this(session, readOnly)
    {
      this._securityHelper = new ContactsOriginateLoanSecurityHelper(this._session, userId, personas);
      this._isInit = true;
      this.InitForUser();
      this.lblLinkedWithPersonaRights.Visible = true;
      this.lblNotLinkedWithPersonaRights.Visible = true;
      this.tvOptions.ImageList = this.imgListTv;
      this.SetImageIndex(this.tvOptions.Nodes, 0);
      this.SetView();
      this._isInit = false;
    }

    public bool HasBeenModified => this._dirty;

    public ArrayList DataView
    {
      get => this._previousView;
      set => this._previousView = value;
    }

    public bool HasSomethingChecked() => this.HasChildNodeChecked(this.tvOptions.Nodes);

    private bool HasChildNodeChecked(TreeNodeCollection nodeCollection)
    {
      if (nodeCollection.Count == 0)
        return false;
      foreach (TreeNode node in nodeCollection)
      {
        if (node.Checked || this.HasChildNodeChecked(node.Nodes))
          return true;
      }
      return false;
    }

    private void InitForPersona()
    {
      this._securityHelper.BuildNodes(this.tvOptions);
      this._securityHelper.UncheckAllDependentNodes();
      this._securityHelper.SetNodeStates();
    }

    private void InitForUser()
    {
      this._securityHelper.BuildNodes(this.tvOptions);
      this._securityHelper.UncheckAllDependentNodes();
      this._securityHelper.SetNodeStates();
    }

    private bool IsUserSetup => this._personaID <= 0;

    private void TreeNodeCheckChanged(TreeNode node)
    {
      if (this.IsUserSetup && node.SelectedImageIndex == 0)
        this._securityHelper.SetNodeImageIndex(node, 1);
      if (node.Nodes.Count > 0)
      {
        if (node.Checked)
        {
          foreach (TreeNode node1 in node.Nodes)
            this.CheckNodeAndItsChildren(node1);
        }
        else
        {
          bool flag1 = false;
          bool flag2 = false;
          foreach (TreeNode node2 in node.Nodes)
          {
            if (node2.ForeColor == SystemColors.GrayText)
            {
              flag1 = true;
              flag2 = node2.Checked;
            }
            this.UncheckNodeAndItsChildren(node2);
            if (flag1 & flag2)
              node.Checked = flag2;
          }
        }
      }
      this.SetPermission(node);
    }

    private void CheckNodeAndItsChildren(TreeNode node)
    {
      if (node.ForeColor == SystemColors.GrayText)
        return;
      if (this.IsUserSetup && node.SelectedImageIndex == 0)
        this._securityHelper.SetNodeImageIndex(node, 1);
      if (node.Nodes.Count > 0)
      {
        foreach (TreeNode node1 in node.Nodes)
        {
          if (!(node1.ForeColor == SystemColors.GrayText))
            this.CheckNodeAndItsChildren(node1);
        }
      }
      this.SetPermission(node);
    }

    private void UncheckNodeAndItsChildren(TreeNode node)
    {
      if (node.ForeColor == SystemColors.GrayText)
        return;
      if (this.IsUserSetup && node.SelectedImageIndex == 0)
        this._securityHelper.SetNodeImageIndex(node, 1);
      node.Checked = false;
      if (node.Nodes.Count > 0)
      {
        foreach (TreeNode node1 in node.Nodes)
        {
          if (!(node1.ForeColor == SystemColors.GrayText))
            this.UncheckNodeAndItsChildren(node1);
        }
      }
      this.SetPermission(node);
    }

    private void SetPermission(TreeNode node)
    {
      if (this._securityHelper.IsDependentOnChildren(node))
        return;
      this._securityHelper.SetNodeUpdateStatus(node, true);
    }

    private void SetImageIndex(TreeNodeCollection nodeCollection, int i)
    {
      this.SetImageIndexToChildrenNodes(nodeCollection, i);
    }

    private void SetImageIndexToChildrenNodes(TreeNodeCollection nodeCollection, int i)
    {
      if (nodeCollection.Count == 0)
        return;
      foreach (TreeNode node in nodeCollection)
      {
        this._securityHelper.SetNodeImageIndex(node, i);
        this.SetImageIndexToChildrenNodes(node.Nodes, i);
      }
    }

    private void SetView()
    {
      this._previousView.Clear();
      foreach (TreeNode node in this.tvOptions.Nodes)
        this._previousView.Add(node.Clone());
    }

    public void SaveData()
    {
      if (this._readOnly)
        return;
      Hashtable aclFeaturesFromUi = this.GetOriginateLoanAclFeaturesFromUi();
      IEnumerator enumerator = aclFeaturesFromUi.Keys.GetEnumerator();
      while (enumerator.MoveNext())
        this._securityHelper.SetPermission((AclFeature) enumerator.Current, (AclTriState) aclFeaturesFromUi[enumerator.Current]);
      this._dirty = false;
    }

    public void UnselectAll()
    {
      this.UnselectAllChildrenNodes(this.tvOptions.Nodes);
      this.SetView();
    }

    private void UnselectAllChildrenNodes(TreeNodeCollection nodeCollection)
    {
      if (nodeCollection.Count == 0)
        return;
      foreach (TreeNode node in nodeCollection)
      {
        if (node.Checked)
        {
          node.Checked = false;
          this._dirty = true;
        }
        this.UnselectAllChildrenNodes(node.Nodes);
      }
    }

    private void StoreCurrentView()
    {
      this.tvOptions.Nodes.Clear();
      foreach (TreeNode treeNode in this._previousView)
        this.tvOptions.Nodes.Add((TreeNode) treeNode.Clone());
    }

    private Hashtable GetOriginateLoanAclFeaturesFromUi()
    {
      Hashtable aclFeaturesFromUi = new Hashtable();
      AclTriState aclTriState1 = this._securityHelper.GetCreateBlankLoanNode(this.tvOptions).Checked ? AclTriState.True : AclTriState.False;
      aclFeaturesFromUi.Add((object) AclFeature.Cnt_Borrower_CreatBlankLoan, (object) aclTriState1);
      AclTriState aclTriState2 = this._securityHelper.GetCreateLoanFromTempNode(this.tvOptions).Checked ? AclTriState.True : AclTriState.False;
      aclFeaturesFromUi.Add((object) AclFeature.Cnt_Borrower_CreatLoanFrmTemplate, (object) aclTriState2);
      AclTriState aclTriState3 = this._securityHelper.GetOrderCreditNode(this.tvOptions).Checked ? AclTriState.True : AclTriState.False;
      aclFeaturesFromUi.Add((object) AclFeature.Cnt_Borrower_OrderCredit, (object) aclTriState3);
      AclTriState aclTriState4 = this._securityHelper.GetProductAndPricingNode(this.tvOptions).Checked ? AclTriState.True : AclTriState.False;
      aclFeaturesFromUi.Add((object) AclFeature.Cnt_Borrower_ProductAndPricing, (object) aclTriState4);
      return aclFeaturesFromUi;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this._isSaving = true;
      this.ValidateSelections();
      if (!this._isValidSelections)
        return;
      if (this.HasSomethingChecked() && !this.GetPipelineLoanTabAccess())
      {
        if (Utils.Dialog((IWin32Window) this, "Current seletions will grant this persona's access to the Pipeline/Loan tab.  Are you sure you want to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
        {
          if (this.CreateLoanStatusChanged != null)
            this.CreateLoanStatusChanged(AclTriState.True);
        }
        else
        {
          this._isCancellingSave = true;
          return;
        }
      }
      this.SetView();
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void ValidateSelections()
    {
      this._isValidSelections = true;
      TreeNode createBlankLoanNode = this._securityHelper.GetCreateBlankLoanNode(this.tvOptions);
      TreeNode loanFromTempNode1 = this._securityHelper.GetCreateLoanFromTempNode(this.tvOptions);
      TreeNode orderCreditNode = this._securityHelper.GetOrderCreditNode(this.tvOptions);
      TreeNode loanFromTempNode2 = this._securityHelper.GetCreateLoanFromTempNode(this.tvOptions);
      if (!orderCreditNode.Checked && !loanFromTempNode2.Checked || createBlankLoanNode.Checked || loanFromTempNode1.Checked)
        return;
      this._isValidSelections = false;
      int num = (int) Utils.Dialog((IWin32Window) this, "Order Credit/Product and Pricing requires New Blank Loan or New from Template.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private bool GetPipelineLoanTabAccess()
    {
      return this.HasPipelineLoanTabAccessEvent != null && this.HasPipelineLoanTabAccessEvent();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.ResetStates();
      this.StoreCurrentView();
    }

    private void ContactsOriginateLoanDlg_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this._isSaving && (this.HasInvalidSelection || this._isCancellingSave))
      {
        this.ResetStates();
        e.Cancel = true;
      }
      else
        this.StoreCurrentView();
    }

    private bool HasInvalidSelection => !this._isValidSelections;

    private void ContactsOriginateLoanDlg_Shown(object sender, EventArgs e)
    {
      this.tvOptions.ExpandAll();
    }

    private void ResetStates()
    {
      this._isSaving = false;
      this._isCancellingSave = false;
      this._isValidSelections = true;
    }

    internal void MakeReadOnly(bool makeReadOnly)
    {
      this._readOnly = makeReadOnly;
      this.btnSave.Enabled = !this._readOnly;
    }

    internal int GetImageIndex() => this.GetImageIndex(this.tvOptions.Nodes);

    private int GetImageIndex(TreeNodeCollection nodeCollection)
    {
      foreach (TreeNode node in nodeCollection)
      {
        if (node.ImageIndex == 1 || this.GetImageIndex(node.Nodes) == 1)
          return 1;
      }
      return 0;
    }

    private void tvOptions_AfterCheck(object sender, TreeViewEventArgs e)
    {
      if (this._isInit)
        return;
      this._isInit = true;
      if (!(sender is TreeView treeView))
        return;
      TreeNode createLoanNode = this._securityHelper.GetCreateLoanNode(treeView);
      TreeNode createBlankLoanNode = this._securityHelper.GetCreateBlankLoanNode(treeView);
      TreeNode loanFromTempNode = this._securityHelper.GetCreateLoanFromTempNode(treeView);
      if (e.Node == createLoanNode)
      {
        createBlankLoanNode.Checked = createLoanNode.Checked;
        loanFromTempNode.Checked = createLoanNode.Checked;
      }
      else if (e.Node == createBlankLoanNode || e.Node == loanFromTempNode)
        createLoanNode.Checked = createBlankLoanNode.Checked && loanFromTempNode.Checked;
      this.TreeNodeCheckChanged(e.Node);
      this._dirty = true;
      this._isInit = false;
    }

    private void tvOptions_BeforeCheck(object sender, TreeViewCancelEventArgs e)
    {
      if (!this._readOnly)
        return;
      e.Cancel = true;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ContactsOriginateLoanDlg));
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.tvOptions = new TreeView();
      this.lblHeader = new Label();
      this.lblNotLinkedWithPersonaRights = new Label();
      this.lblLinkedWithPersonaRights = new Label();
      this.imgListTv = new ImageList(this.components);
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(201, 205);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnSave.DialogResult = DialogResult.OK;
      this.btnSave.Location = new Point(117, 205);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 6;
      this.btnSave.Text = "OK";
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.tvOptions.CheckBoxes = true;
      this.tvOptions.Location = new Point(12, 45);
      this.tvOptions.Name = "tvOptions";
      this.tvOptions.Scrollable = false;
      this.tvOptions.ShowLines = false;
      this.tvOptions.ShowPlusMinus = false;
      this.tvOptions.ShowRootLines = false;
      this.tvOptions.Size = new Size(264, 111);
      this.tvOptions.TabIndex = 5;
      this.tvOptions.BeforeCheck += new TreeViewCancelEventHandler(this.tvOptions_BeforeCheck);
      this.tvOptions.AfterCheck += new TreeViewEventHandler(this.tvOptions_AfterCheck);
      this.lblHeader.Location = new Point(12, 9);
      this.lblHeader.Name = "lblHeader";
      this.lblHeader.Size = new Size(212, 33);
      this.lblHeader.TabIndex = 4;
      this.lblHeader.Text = "Select the options for Originate Loan/Order Credit/Product and Pricing";
      this.lblNotLinkedWithPersonaRights.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblNotLinkedWithPersonaRights.ImageIndex = 1;
      this.lblNotLinkedWithPersonaRights.ImageList = this.imgListTv;
      this.lblNotLinkedWithPersonaRights.Location = new Point(16, 182);
      this.lblNotLinkedWithPersonaRights.Name = "lblNotLinkedWithPersonaRights";
      this.lblNotLinkedWithPersonaRights.Size = new Size(204, 16);
      this.lblNotLinkedWithPersonaRights.TabIndex = 9;
      this.lblNotLinkedWithPersonaRights.Text = "       Disconnected from Persona Rights";
      this.lblNotLinkedWithPersonaRights.TextAlign = ContentAlignment.MiddleLeft;
      this.lblNotLinkedWithPersonaRights.Visible = false;
      this.lblLinkedWithPersonaRights.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblLinkedWithPersonaRights.ImageIndex = 0;
      this.lblLinkedWithPersonaRights.ImageList = this.imgListTv;
      this.lblLinkedWithPersonaRights.Location = new Point(16, 166);
      this.lblLinkedWithPersonaRights.Name = "lblLinkedWithPersonaRights";
      this.lblLinkedWithPersonaRights.Size = new Size(172, 16);
      this.lblLinkedWithPersonaRights.TabIndex = 8;
      this.lblLinkedWithPersonaRights.Text = "       Linked with Persona Rights";
      this.lblLinkedWithPersonaRights.TextAlign = ContentAlignment.MiddleLeft;
      this.lblLinkedWithPersonaRights.Visible = false;
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "");
      this.imgListTv.Images.SetKeyName(1, "");
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.ClientSize = new Size(284, 240);
      this.Controls.Add((Control) this.lblNotLinkedWithPersonaRights);
      this.Controls.Add((Control) this.lblLinkedWithPersonaRights);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.tvOptions);
      this.Controls.Add((Control) this.lblHeader);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ContactsOriginateLoanDlg);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Options";
      this.FormClosing += new FormClosingEventHandler(this.ContactsOriginateLoanDlg_FormClosing);
      this.Shown += new EventHandler(this.ContactsOriginateLoanDlg_Shown);
      this.ResumeLayout(false);
    }

    public delegate void OriginateLoanFeatureStatusChanged(AclTriState state);
  }
}

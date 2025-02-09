// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.MoveLoanFolderDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.PersonaSetup.SecurityPage
{
  public class MoveLoanFolderDlg : Form
  {
    private Label lblLabel;
    private GroupBox gBoxMoveFrom;
    private GroupBox gBoxMoveTo;
    private TreeView tvMoveFrom;
    private TreeView tvMoveTo;
    private Button btnOK;
    private Button btnCancel;
    private IContainer components;
    private int personaID = -1;
    private LoanFoldersAclManager aclMgr;
    private bool isPersonal;
    private ImageList imgListTv;
    private Label lblLinked;
    private Label lblDisconnected;
    private LoanFolderAclInfo[] loanFolderList;
    private string userID = "";
    private ContextMenu contMenuFrom;
    private MenuItem miLinkFrom;
    private MenuItem miDisLinkFrom;
    private ContextMenu contMenuTo;
    private MenuItem miLinkTo;
    private MenuItem miDisconTo;
    private Persona[] personaList;
    private bool internalAccess;
    private bool dirty;
    private bool readOnly;
    private int selectOption = 2;
    private ArrayList previousViewFrom;
    private ArrayList previousViewTo;
    private Sessions.Session session;

    public MoveLoanFolderDlg(Sessions.Session session, int personaID, bool readOnly, int option)
    {
      this.session = session;
      this.InitializeComponent();
      this.personaID = personaID;
      this.tvMoveFrom.ImageList = (ImageList) null;
      this.tvMoveTo.ImageList = (ImageList) null;
      this.selectOption = option;
      this.readOnly = readOnly;
      this.MakeReadOnly(this.readOnly);
      this.previousViewFrom = new ArrayList();
      this.previousViewTo = new ArrayList();
      this.tvMoveFrom.ContextMenu = (ContextMenu) null;
      this.tvMoveTo.ContextMenu = (ContextMenu) null;
      this.aclMgr = (LoanFoldersAclManager) this.session.ACL.GetAclManager(AclCategory.LoanFolderMove);
      this.LoadTreesForPersona();
      if (this.selectOption == 1)
        this.SelectAll();
      else if (this.selectOption == 0)
        this.DeselectAll();
      else
        this.setView();
    }

    public MoveLoanFolderDlg(
      Sessions.Session session,
      string userID,
      Persona[] personaList,
      bool readOnly,
      int option)
    {
      this.session = session;
      this.InitializeComponent();
      this.userID = userID;
      this.personaList = personaList;
      this.isPersonal = true;
      this.lblDisconnected.Visible = true;
      this.lblLinked.Visible = true;
      this.readOnly = readOnly;
      this.previousViewFrom = new ArrayList();
      this.previousViewTo = new ArrayList();
      this.selectOption = option;
      this.MakeReadOnly(this.readOnly);
      this.aclMgr = (LoanFoldersAclManager) this.session.ACL.GetAclManager(AclCategory.LoanFolderMove);
      this.LoadTreesForUser();
      if (!this.readOnly)
      {
        if (this.selectOption == 1)
          this.SelectAll();
        else if (this.selectOption == 0)
          this.DeselectAll();
      }
      if (this.selectOption != 2)
        return;
      this.setView();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MoveLoanFolderDlg));
      this.lblLabel = new Label();
      this.gBoxMoveFrom = new GroupBox();
      this.tvMoveFrom = new TreeView();
      this.contMenuFrom = new ContextMenu();
      this.miLinkFrom = new MenuItem();
      this.miDisLinkFrom = new MenuItem();
      this.imgListTv = new ImageList(this.components);
      this.gBoxMoveTo = new GroupBox();
      this.tvMoveTo = new TreeView();
      this.contMenuTo = new ContextMenu();
      this.miLinkTo = new MenuItem();
      this.miDisconTo = new MenuItem();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.lblLinked = new Label();
      this.lblDisconnected = new Label();
      this.gBoxMoveFrom.SuspendLayout();
      this.gBoxMoveTo.SuspendLayout();
      this.SuspendLayout();
      this.lblLabel.Location = new Point(12, 8);
      this.lblLabel.Name = "lblLabel";
      this.lblLabel.Size = new Size(348, 16);
      this.lblLabel.TabIndex = 0;
      this.lblLabel.Text = "Select the loan folders that this persona can move loans from and to.";
      this.gBoxMoveFrom.Controls.Add((Control) this.tvMoveFrom);
      this.gBoxMoveFrom.Location = new Point(12, 30);
      this.gBoxMoveFrom.Name = "gBoxMoveFrom";
      this.gBoxMoveFrom.Size = new Size(228, 212);
      this.gBoxMoveFrom.TabIndex = 1;
      this.gBoxMoveFrom.TabStop = false;
      this.gBoxMoveFrom.Text = "Move From";
      this.tvMoveFrom.CheckBoxes = true;
      this.tvMoveFrom.ContextMenu = this.contMenuFrom;
      this.tvMoveFrom.HideSelection = false;
      this.tvMoveFrom.ImageIndex = 1;
      this.tvMoveFrom.ImageList = this.imgListTv;
      this.tvMoveFrom.Location = new Point(12, 20);
      this.tvMoveFrom.Name = "tvMoveFrom";
      this.tvMoveFrom.SelectedImageIndex = 1;
      this.tvMoveFrom.ShowLines = false;
      this.tvMoveFrom.ShowPlusMinus = false;
      this.tvMoveFrom.ShowRootLines = false;
      this.tvMoveFrom.Size = new Size(204, 180);
      this.tvMoveFrom.Sorted = true;
      this.tvMoveFrom.TabIndex = 0;
      this.tvMoveFrom.AfterCheck += new TreeViewEventHandler(this.tvMoveFrom_AfterCheck);
      this.tvMoveFrom.MouseUp += new MouseEventHandler(this.tvMoveFrom_MouseUp);
      this.tvMoveFrom.BeforeCheck += new TreeViewCancelEventHandler(this.tvMoveFrom_BeforeCheck);
      this.contMenuFrom.MenuItems.AddRange(new MenuItem[2]
      {
        this.miLinkFrom,
        this.miDisLinkFrom
      });
      this.miLinkFrom.Index = 0;
      this.miLinkFrom.Text = "Link with Persona Rights";
      this.miLinkFrom.Click += new EventHandler(this.miLinkFrom_Click);
      this.miDisLinkFrom.Index = 1;
      this.miDisLinkFrom.Text = "Disconnect from Persona Rights";
      this.miDisLinkFrom.Click += new EventHandler(this.miDisLinkFrom_Click);
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "");
      this.imgListTv.Images.SetKeyName(1, "");
      this.gBoxMoveTo.Controls.Add((Control) this.tvMoveTo);
      this.gBoxMoveTo.Location = new Point(244, 30);
      this.gBoxMoveTo.Name = "gBoxMoveTo";
      this.gBoxMoveTo.Size = new Size(228, 212);
      this.gBoxMoveTo.TabIndex = 2;
      this.gBoxMoveTo.TabStop = false;
      this.gBoxMoveTo.Text = "Move To";
      this.tvMoveTo.CheckBoxes = true;
      this.tvMoveTo.ContextMenu = this.contMenuTo;
      this.tvMoveTo.HideSelection = false;
      this.tvMoveTo.ImageIndex = 1;
      this.tvMoveTo.ImageList = this.imgListTv;
      this.tvMoveTo.Location = new Point(12, 20);
      this.tvMoveTo.Name = "tvMoveTo";
      this.tvMoveTo.SelectedImageIndex = 1;
      this.tvMoveTo.ShowLines = false;
      this.tvMoveTo.ShowPlusMinus = false;
      this.tvMoveTo.ShowRootLines = false;
      this.tvMoveTo.Size = new Size(204, 180);
      this.tvMoveTo.Sorted = true;
      this.tvMoveTo.TabIndex = 0;
      this.tvMoveTo.AfterCheck += new TreeViewEventHandler(this.tvMoveTo_AfterCheck);
      this.tvMoveTo.MouseUp += new MouseEventHandler(this.tvMoveTo_MouseUp);
      this.tvMoveTo.BeforeCheck += new TreeViewCancelEventHandler(this.tvMoveTo_BeforeCheck);
      this.contMenuTo.MenuItems.AddRange(new MenuItem[2]
      {
        this.miLinkTo,
        this.miDisconTo
      });
      this.miLinkTo.Index = 0;
      this.miLinkTo.Text = "Link with Persona Rights";
      this.miLinkTo.Click += new EventHandler(this.miLinkTo_Click);
      this.miDisconTo.Index = 1;
      this.miDisconTo.Text = "Disconnect from Persona Rights";
      this.miDisconTo.Click += new EventHandler(this.miDisconTo_Click);
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(312, 252);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(400, 252);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.lblLinked.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblLinked.ImageIndex = 1;
      this.lblLinked.ImageList = this.imgListTv;
      this.lblLinked.Location = new Point(12, 245);
      this.lblLinked.Name = "lblLinked";
      this.lblLinked.Size = new Size(216, 16);
      this.lblLinked.TabIndex = 5;
      this.lblLinked.Text = "      Linked with Persona Rights";
      this.lblLinked.TextAlign = ContentAlignment.MiddleLeft;
      this.lblLinked.Visible = false;
      this.lblDisconnected.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblDisconnected.ImageIndex = 0;
      this.lblDisconnected.ImageList = this.imgListTv;
      this.lblDisconnected.Location = new Point(12, 261);
      this.lblDisconnected.Name = "lblDisconnected";
      this.lblDisconnected.Size = new Size(216, 16);
      this.lblDisconnected.TabIndex = 6;
      this.lblDisconnected.Text = "      Disconnected from Persona Rights";
      this.lblDisconnected.TextAlign = ContentAlignment.MiddleLeft;
      this.lblDisconnected.Visible = false;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(482, 286);
      this.Controls.Add((Control) this.lblDisconnected);
      this.Controls.Add((Control) this.lblLinked);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.gBoxMoveTo);
      this.Controls.Add((Control) this.gBoxMoveFrom);
      this.Controls.Add((Control) this.lblLabel);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (MoveLoanFolderDlg);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = " Select Loan Folders";
      this.Closing += new CancelEventHandler(this.MoveLoanFolderDlg_Closing);
      this.gBoxMoveFrom.ResumeLayout(false);
      this.gBoxMoveTo.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void LoadTreesForPersona()
    {
      LoanFolderInfo[] allLoanFolderInfos = this.session.LoanManager.GetAllLoanFolderInfos(true);
      this.loanFolderList = this.aclMgr.GetPermissions(AclFeature.LoanMgmt_Move, this.personaID);
      if (this.loanFolderList == null || this.loanFolderList.Length == 0)
        return;
      foreach (LoanFolderAclInfo loanFolder in this.loanFolderList)
      {
        string text = loanFolder.FolderName;
        foreach (LoanFolderInfo loanFolderInfo in allLoanFolderInfos)
        {
          if (loanFolderInfo.Name == loanFolder.FolderName)
          {
            text = loanFolderInfo.DisplayName;
            break;
          }
        }
        TreeNode node1 = new TreeNode(text);
        TreeNode node2 = new TreeNode(text);
        node2.Tag = (object) loanFolder;
        node1.Tag = (object) loanFolder;
        if (loanFolder.MoveFromAccess == 1)
          node1.Checked = true;
        if (loanFolder.MoveToAccess == 1)
          node2.Checked = true;
        this.tvMoveFrom.Nodes.Add(node1);
        this.tvMoveTo.Nodes.Add(node2);
      }
    }

    private void LoadTreesForUser()
    {
      LoanFolderInfo[] allLoanFolderInfos = this.session.LoanManager.GetAllLoanFolderInfos(true);
      this.loanFolderList = this.aclMgr.GetAccessibleLoanFolders(AclFeature.LoanMgmt_Move, this.userID, this.personaList);
      if (this.loanFolderList == null || this.loanFolderList.Length == 0)
        return;
      foreach (LoanFolderAclInfo loanFolder in this.loanFolderList)
      {
        string text = loanFolder.FolderName;
        foreach (LoanFolderInfo loanFolderInfo in allLoanFolderInfos)
        {
          if (loanFolderInfo.Name == loanFolder.FolderName)
          {
            text = loanFolderInfo.DisplayName;
            break;
          }
        }
        TreeNode treeNode1 = new TreeNode(text);
        TreeNode treeNode2 = new TreeNode(text);
        treeNode2.Tag = (object) loanFolder;
        treeNode1.Tag = (object) loanFolder;
        if (loanFolder.MoveFromAccess == 1)
          treeNode1.Checked = true;
        if (loanFolder.MoveToAccess == 1)
          treeNode2.Checked = true;
        if (loanFolder.CustomMoveFrom)
        {
          treeNode1.ImageIndex = 0;
          treeNode1.SelectedImageIndex = 0;
        }
        else
        {
          treeNode1.ImageIndex = 1;
          treeNode1.SelectedImageIndex = 1;
        }
        if (loanFolder.CustomMoveTo)
        {
          treeNode2.ImageIndex = 0;
          treeNode2.SelectedImageIndex = 0;
        }
        else
        {
          treeNode2.ImageIndex = 1;
          treeNode2.SelectedImageIndex = 1;
        }
        this.tvMoveFrom.Nodes.Add((TreeNode) treeNode1.Clone());
        this.tvMoveTo.Nodes.Add((TreeNode) treeNode2.Clone());
      }
    }

    private void SelectAll()
    {
      foreach (TreeNode node in this.tvMoveFrom.Nodes)
        node.Checked = true;
      foreach (TreeNode node in this.tvMoveTo.Nodes)
        node.Checked = true;
    }

    private void DeselectAll()
    {
      foreach (TreeNode node in this.tvMoveFrom.Nodes)
        node.Checked = false;
      foreach (TreeNode node in this.tvMoveTo.Nodes)
        node.Checked = false;
    }

    private void MakeReadOnly(bool makeReadOnly)
    {
      if (makeReadOnly)
      {
        this.btnOK.Enabled = false;
        this.miDisconTo.Enabled = false;
        this.miDisLinkFrom.Enabled = false;
        this.miLinkFrom.Enabled = false;
        this.miLinkTo.Enabled = false;
      }
      else
      {
        this.btnOK.Enabled = true;
        this.miDisconTo.Enabled = true;
        this.miDisLinkFrom.Enabled = true;
        this.miLinkFrom.Enabled = true;
        this.miLinkTo.Enabled = true;
      }
    }

    public bool isReadOnly
    {
      set => this.MakeReadOnly(value);
    }

    public void SaveData()
    {
      this.dirty = false;
      if (!this.isPersonal)
      {
        for (int index1 = 0; index1 < this.loanFolderList.Length; ++index1)
        {
          int num = 0;
          int index2;
          for (index2 = 0; index2 < this.tvMoveFrom.Nodes.Count; ++index2)
          {
            if (this.loanFolderList[index1].FolderName == ((LoanFolderAclInfo) this.tvMoveFrom.Nodes[index2].Tag).FolderName)
            {
              num = index2;
              break;
            }
          }
          this.loanFolderList[index1].MoveFromAccess = !this.tvMoveFrom.Nodes[index2].Checked ? 0 : 1;
          this.loanFolderList[index1].MoveToAccess = !this.tvMoveTo.Nodes[index2].Checked ? 0 : 1;
        }
        this.aclMgr.SetPermissions(AclFeature.LoanMgmt_Move, this.loanFolderList, this.personaID);
      }
      else
      {
        ArrayList arrayList = new ArrayList();
        bool flag = false;
        for (int index3 = 0; index3 < this.loanFolderList.Length; ++index3)
        {
          int index4 = 0;
          while (index4 < this.tvMoveFrom.Nodes.Count && !(this.loanFolderList[index3].FolderName == ((LoanFolderAclInfo) this.tvMoveFrom.Nodes[index4].Tag).FolderName))
            ++index4;
          LoanFolderAclInfo loanFolder = this.loanFolderList[index3];
          loanFolder.MoveFromAccess = 2;
          loanFolder.MoveToAccess = 2;
          LoanFolderAclInfo tag1 = (LoanFolderAclInfo) this.tvMoveFrom.Nodes[index4].Tag;
          LoanFolderAclInfo tag2 = (LoanFolderAclInfo) this.tvMoveTo.Nodes[index4].Tag;
          if (tag1.CustomMoveFrom)
          {
            this.loanFolderList[index3].CustomMoveFrom = true;
            loanFolder.MoveFromAccess = !this.tvMoveFrom.Nodes[index4].Checked ? 0 : 1;
            flag = true;
          }
          if (tag2.CustomMoveTo)
          {
            this.loanFolderList[index3].CustomMoveTo = true;
            loanFolder.MoveToAccess = !this.tvMoveTo.Nodes[index4].Checked ? 0 : 1;
            flag = true;
          }
          if (flag)
            arrayList.Add((object) loanFolder);
          flag = false;
        }
        this.aclMgr.SetPermissions(AclFeature.LoanMgmt_Move, (LoanFolderAclInfo[]) arrayList.ToArray(typeof (LoanFolderAclInfo)), this.userID);
      }
    }

    private void tvMoveFrom_AfterCheck(object sender, TreeViewEventArgs e)
    {
      if (this.internalAccess)
        return;
      e.Node.SelectedImageIndex = 0;
      e.Node.ImageIndex = 0;
      LoanFolderAclInfo tag = (LoanFolderAclInfo) e.Node.Tag;
      tag.CustomMoveFrom = true;
      tag.MoveFromAccess = !e.Node.Checked ? 0 : 1;
      e.Node.Tag = (object) tag;
      this.dirty = true;
    }

    private void tvMoveTo_AfterCheck(object sender, TreeViewEventArgs e)
    {
      if (this.internalAccess)
        return;
      e.Node.SelectedImageIndex = 0;
      e.Node.ImageIndex = 0;
      LoanFolderAclInfo tag = (LoanFolderAclInfo) e.Node.Tag;
      tag.CustomMoveTo = true;
      tag.MoveToAccess = !e.Node.Checked ? 0 : 1;
      e.Node.Tag = (object) tag;
      this.dirty = true;
    }

    private void miLinkFrom_Click(object sender, EventArgs e)
    {
      TreeNode selectedNode = this.tvMoveFrom.SelectedNode;
      this.internalAccess = true;
      if (selectedNode != null && selectedNode.ImageIndex < 1)
      {
        selectedNode.SelectedImageIndex = 1;
        selectedNode.ImageIndex = 1;
        LoanFolderAclInfo permission = this.aclMgr.GetPermission(AclFeature.LoanMgmt_Move, this.personaList, ((LoanFolderAclInfo) selectedNode.Tag).FolderName);
        if (permission != null)
        {
          ((LoanFolderAclInfo) selectedNode.Tag).MoveFromAccess = permission.MoveFromAccess;
          selectedNode.Checked = permission.MoveFromAccess == 1;
        }
        ((LoanFolderAclInfo) selectedNode.Tag).CustomMoveFrom = false;
        this.dirty = true;
      }
      this.internalAccess = false;
    }

    private void miDisLinkFrom_Click(object sender, EventArgs e)
    {
      TreeNode selectedNode = this.tvMoveFrom.SelectedNode;
      this.internalAccess = true;
      if (selectedNode != null && selectedNode.ImageIndex > 0)
      {
        selectedNode.SelectedImageIndex = 0;
        selectedNode.ImageIndex = 0;
        LoanFolderAclInfo permission = this.aclMgr.GetPermission(AclFeature.LoanMgmt_Move, this.userID, ((LoanFolderAclInfo) selectedNode.Tag).FolderName);
        if (permission != null)
        {
          ((LoanFolderAclInfo) selectedNode.Tag).MoveFromAccess = permission.MoveFromAccess;
          selectedNode.Checked = permission.MoveFromAccess == 1;
        }
        else
        {
          ((LoanFolderAclInfo) selectedNode.Tag).MoveFromAccess = 0;
          selectedNode.Checked = false;
        }
        ((LoanFolderAclInfo) selectedNode.Tag).CustomMoveFrom = true;
        this.dirty = true;
      }
      this.internalAccess = false;
    }

    private void miLinkTo_Click(object sender, EventArgs e)
    {
      TreeNode selectedNode = this.tvMoveTo.SelectedNode;
      this.internalAccess = true;
      if (selectedNode != null && selectedNode.ImageIndex < 1)
      {
        selectedNode.SelectedImageIndex = 1;
        selectedNode.ImageIndex = 1;
        LoanFolderAclInfo permission = this.aclMgr.GetPermission(AclFeature.LoanMgmt_Move, this.personaList, ((LoanFolderAclInfo) selectedNode.Tag).FolderName);
        if (permission != null)
        {
          ((LoanFolderAclInfo) selectedNode.Tag).MoveToAccess = permission.MoveToAccess;
          selectedNode.Checked = permission.MoveToAccess == 1;
        }
        ((LoanFolderAclInfo) selectedNode.Tag).CustomMoveTo = false;
        this.dirty = true;
      }
      this.internalAccess = false;
    }

    private void miDisconTo_Click(object sender, EventArgs e)
    {
      TreeNode selectedNode = this.tvMoveTo.SelectedNode;
      this.internalAccess = true;
      if (selectedNode != null && selectedNode.ImageIndex > 0)
      {
        selectedNode.SelectedImageIndex = 0;
        selectedNode.ImageIndex = 0;
        LoanFolderAclInfo permission = this.aclMgr.GetPermission(AclFeature.LoanMgmt_Move, this.userID, ((LoanFolderAclInfo) selectedNode.Tag).FolderName);
        if (permission != null)
        {
          ((LoanFolderAclInfo) selectedNode.Tag).MoveToAccess = permission.MoveToAccess;
          selectedNode.Checked = permission.MoveToAccess == 1;
        }
        else
        {
          ((LoanFolderAclInfo) selectedNode.Tag).MoveToAccess = 0;
          selectedNode.Checked = false;
        }
        ((LoanFolderAclInfo) selectedNode.Tag).CustomMoveTo = true;
        this.dirty = true;
      }
      this.internalAccess = false;
    }

    private void tvMoveTo_MouseUp(object sender, MouseEventArgs e)
    {
      TreeNode nodeAt = this.tvMoveTo.GetNodeAt(e.X, e.Y);
      if (nodeAt == null)
        return;
      this.tvMoveTo.SelectedNode = nodeAt;
    }

    private void tvMoveFrom_MouseUp(object sender, MouseEventArgs e)
    {
      TreeNode nodeAt = this.tvMoveFrom.GetNodeAt(e.X, e.Y);
      if (nodeAt == null)
        return;
      this.tvMoveFrom.SelectedNode = nodeAt;
    }

    public bool HasBeenModified() => this.dirty;

    private void tvMoveFrom_BeforeCheck(object sender, TreeViewCancelEventArgs e)
    {
      if (!this.readOnly)
        return;
      e.Cancel = true;
    }

    private void tvMoveTo_BeforeCheck(object sender, TreeViewCancelEventArgs e)
    {
      if (!this.readOnly)
        return;
      e.Cancel = true;
    }

    public bool HasSomethingChecked()
    {
      bool flag = false;
      foreach (TreeNode node in this.tvMoveTo.Nodes)
      {
        if (node.Checked)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        foreach (TreeNode node in this.tvMoveFrom.Nodes)
        {
          if (node.Checked)
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    public int GetImageIndex()
    {
      int imageIndex = 0;
      foreach (TreeNode node in this.tvMoveTo.Nodes)
      {
        if (node.ImageIndex == 0)
        {
          imageIndex = 1;
          break;
        }
      }
      if (imageIndex == 0)
      {
        foreach (TreeNode node in this.tvMoveFrom.Nodes)
        {
          if (node.ImageIndex == 0)
          {
            imageIndex = 1;
            break;
          }
        }
      }
      return imageIndex;
    }

    private void setView()
    {
      this.previousViewFrom = new ArrayList();
      this.previousViewTo = new ArrayList();
      foreach (TreeNode node in this.tvMoveFrom.Nodes)
        this.previousViewFrom.Add(node.Clone());
      foreach (TreeNode node in this.tvMoveTo.Nodes)
        this.previousViewTo.Add(node.Clone());
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.setView();
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.internalAccess = true;
      this.tvMoveFrom.Nodes.Clear();
      this.tvMoveTo.Nodes.Clear();
      foreach (TreeNode treeNode in this.previousViewFrom)
        this.tvMoveFrom.Nodes.Add((TreeNode) treeNode.Clone());
      foreach (TreeNode treeNode in this.previousViewTo)
        this.tvMoveTo.Nodes.Add((TreeNode) treeNode.Clone());
      this.internalAccess = false;
    }

    private void MoveLoanFolderDlg_Closing(object sender, CancelEventArgs e)
    {
      this.btnCancel_Click((object) null, (EventArgs) null);
    }

    public ArrayList DataViewFrom
    {
      get => this.previousViewFrom;
      set => this.previousViewFrom = value;
    }

    public ArrayList DataViewTo
    {
      get => this.previousViewTo;
      set => this.previousViewTo = value;
    }
  }
}

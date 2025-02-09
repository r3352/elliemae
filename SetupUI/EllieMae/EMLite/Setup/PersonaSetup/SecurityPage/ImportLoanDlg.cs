// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.ImportLoanDlg
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
  public class ImportLoanDlg : Form
  {
    private Sessions.Session session;
    private Label lblHeader;
    private TreeView tvImport;
    private Button btnSave;
    private Button btnCancel;
    private LoanFoldersAclManager aclMgr;
    private LoanFolderAclInfo[] folderList;
    private int personaID = -1;
    private string userID = "";
    private Persona[] personaList;
    private bool dirty;
    private bool isPersonal;
    private bool internalAccess;
    private ImageList imgList;
    private Label label1;
    private Label label2;
    private ContextMenu contextMenu1;
    private MenuItem miLink;
    private MenuItem miDisconnect;
    private IContainer components;
    private bool readOnly;
    private ArrayList previousView;
    private int selectOption = 2;

    public ImportLoanDlg(Sessions.Session session, int personaID, bool readOnly, int option)
    {
      this.session = session;
      this.InitializeComponent();
      this.personaID = personaID;
      this.internalAccess = true;
      this.selectOption = option;
      this.readOnly = readOnly;
      this.MakeReadOnly(this.readOnly);
      this.previousView = new ArrayList();
      this.InitPage();
      this.internalAccess = false;
      if (this.selectOption == 1)
        this.SelectAll();
      else if (this.selectOption == 0)
        this.DeselectAll();
      else
        this.setView();
    }

    public ImportLoanDlg(
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
      this.label1.Visible = true;
      this.label2.Visible = true;
      this.tvImport.ImageList = this.imgList;
      this.tvImport.ContextMenu = this.contextMenu1;
      this.internalAccess = true;
      this.previousView = new ArrayList();
      this.InitPageForUser();
      this.internalAccess = false;
      this.readOnly = readOnly;
      this.selectOption = option;
      this.MakeReadOnly(this.readOnly);
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

    public bool IsReadOnly
    {
      set => this.MakeReadOnly(value);
    }

    private void MakeReadOnly(bool readOnly)
    {
      if (readOnly)
      {
        this.btnSave.Enabled = false;
        this.miDisconnect.Enabled = false;
        this.miLink.Enabled = false;
      }
      else
      {
        this.btnSave.Enabled = true;
        this.miDisconnect.Enabled = true;
        this.miLink.Enabled = true;
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
      TreeNode treeNode1 = new TreeNode("Calyx Point");
      TreeNode treeNode2 = new TreeNode("Fannie Mae 3.x");
      TreeNode treeNode3 = new TreeNode("ULAD / iLAD (MISMO 3.4)");
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ImportLoanDlg));
      this.lblHeader = new Label();
      this.tvImport = new TreeView();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.imgList = new ImageList(this.components);
      this.label1 = new Label();
      this.label2 = new Label();
      this.contextMenu1 = new ContextMenu();
      this.miLink = new MenuItem();
      this.miDisconnect = new MenuItem();
      this.SuspendLayout();
      this.lblHeader.Location = new Point(16, 12);
      this.lblHeader.Name = "lblHeader";
      this.lblHeader.Size = new Size(180, 16);
      this.lblHeader.TabIndex = 0;
      this.lblHeader.Text = "This persona can import files from:";
      this.tvImport.CheckBoxes = true;
      this.tvImport.Location = new Point(16, 36);
      this.tvImport.Name = "tvImport";
      treeNode1.Name = "";
      treeNode1.Text = "Calyx Point";
      treeNode2.Name = "";
      treeNode2.Text = "Fannie Mae 3.x";
      treeNode3.Name = "";
      treeNode3.Text = "ULAD / iLAD (MISMO 3.4)";
      this.tvImport.Nodes.AddRange(new TreeNode[3]
      {
        treeNode1,
        treeNode2,
        treeNode3
      });
      this.tvImport.Scrollable = false;
      this.tvImport.ShowLines = false;
      this.tvImport.ShowPlusMinus = false;
      this.tvImport.ShowRootLines = false;
      this.tvImport.Size = new Size(264, 76);
      this.tvImport.TabIndex = 1;
      this.tvImport.BeforeCheck += new TreeViewCancelEventHandler(this.tvImport_BeforeCheck);
      this.tvImport.AfterCheck += new TreeViewEventHandler(this.tvImport_AfterCheck);
      this.tvImport.BeforeCollapse += new TreeViewCancelEventHandler(this.tvImport_BeforeCollapse);
      this.tvImport.MouseUp += new MouseEventHandler(this.tvImport_MouseUp);
      this.btnSave.DialogResult = DialogResult.OK;
      this.btnSave.Location = new Point(121, 165);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 2;
      this.btnSave.Text = "OK";
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(205, 165);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgList.ImageStream");
      this.imgList.TransparentColor = Color.Transparent;
      this.imgList.Images.SetKeyName(0, "");
      this.imgList.Images.SetKeyName(1, "");
      this.label1.ImageAlign = ContentAlignment.MiddleLeft;
      this.label1.ImageIndex = 1;
      this.label1.ImageList = this.imgList;
      this.label1.Location = new Point(16, 119);
      this.label1.Name = "label1";
      this.label1.Size = new Size(172, 16);
      this.label1.TabIndex = 4;
      this.label1.Text = "       Linked with Persona Rights";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.label1.Visible = false;
      this.label2.ImageAlign = ContentAlignment.MiddleLeft;
      this.label2.ImageIndex = 0;
      this.label2.ImageList = this.imgList;
      this.label2.Location = new Point(16, 135);
      this.label2.Name = "label2";
      this.label2.Size = new Size(204, 16);
      this.label2.TabIndex = 5;
      this.label2.Text = "       Disconnected from Persona Rights";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.Visible = false;
      this.contextMenu1.MenuItems.AddRange(new MenuItem[2]
      {
        this.miLink,
        this.miDisconnect
      });
      this.miLink.Index = 0;
      this.miLink.Text = "Link with Persona Rights";
      this.miLink.Click += new EventHandler(this.miLink_Click);
      this.miDisconnect.Index = 1;
      this.miDisconnect.Text = "Disconnect from Persona Rights";
      this.miDisconnect.Click += new EventHandler(this.miDisconnect_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(298, 200);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.tvImport);
      this.Controls.Add((Control) this.lblHeader);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportLoanDlg);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = " Select Options";
      this.Closing += new CancelEventHandler(this.ImportLoanDlg_Closing);
      this.ResumeLayout(false);
    }

    private void InitPage()
    {
      this.aclMgr = (LoanFoldersAclManager) this.session.ACL.GetAclManager(AclCategory.LoanFolderMove);
      this.folderList = this.aclMgr.GetPermissions(AclFeature.LoanMgmt_Import, this.personaID);
      foreach (LoanFolderAclInfo folder in this.folderList)
      {
        switch (folder.FolderName)
        {
          case "Calyx Point":
            this.tvImport.Nodes[0].Checked = folder.MoveFromAccess == 1;
            this.tvImport.Nodes[0].Tag = (object) folder;
            break;
          case "Fannie Mae 3.x":
            this.tvImport.Nodes[1].Checked = folder.MoveFromAccess == 1;
            this.tvImport.Nodes[1].Tag = (object) folder;
            break;
          case "ULAD":
            this.tvImport.Nodes[2].Checked = folder.MoveFromAccess == 1;
            this.tvImport.Nodes[2].Tag = (object) folder;
            break;
        }
      }
    }

    private void InitPageForUser()
    {
      this.aclMgr = (LoanFoldersAclManager) this.session.ACL.GetAclManager(AclCategory.LoanFolderMove);
      this.folderList = this.aclMgr.GetAccessibleLoanFolders(AclFeature.LoanMgmt_Import, this.userID, this.personaList);
      foreach (LoanFolderAclInfo folder in this.folderList)
      {
        switch (folder.FolderName)
        {
          case "Calyx Point":
            this.tvImport.Nodes[0].Checked = folder.MoveFromAccess == 1;
            if (folder.CustomMoveFrom)
            {
              this.tvImport.Nodes[0].ImageIndex = 0;
              this.tvImport.Nodes[0].SelectedImageIndex = 0;
            }
            else
            {
              this.tvImport.Nodes[0].ImageIndex = 1;
              this.tvImport.Nodes[0].SelectedImageIndex = 1;
            }
            this.tvImport.Nodes[0].Tag = (object) folder;
            break;
          case "Fannie Mae 3.x":
            this.tvImport.Nodes[1].Checked = folder.MoveFromAccess == 1;
            if (folder.CustomMoveFrom)
            {
              this.tvImport.Nodes[1].ImageIndex = 0;
              this.tvImport.Nodes[1].SelectedImageIndex = 0;
            }
            else
            {
              this.tvImport.Nodes[1].ImageIndex = 1;
              this.tvImport.Nodes[1].SelectedImageIndex = 1;
            }
            this.tvImport.Nodes[1].Tag = (object) folder;
            break;
          case "ULAD":
            this.tvImport.Nodes[2].Checked = folder.MoveFromAccess == 1;
            if (folder.CustomMoveFrom)
            {
              this.tvImport.Nodes[2].ImageIndex = 0;
              this.tvImport.Nodes[2].SelectedImageIndex = 0;
            }
            else
            {
              this.tvImport.Nodes[2].ImageIndex = 1;
              this.tvImport.Nodes[2].SelectedImageIndex = 1;
            }
            this.tvImport.Nodes[2].Tag = (object) folder;
            break;
        }
      }
    }

    public void SaveData()
    {
      this.dirty = false;
      if (!this.isPersonal)
      {
        LoanFolderAclInfo[] loanFolderAclInfoList = new LoanFolderAclInfo[3];
        for (int index = 0; index < this.tvImport.Nodes.Count; ++index)
        {
          TreeNode node = this.tvImport.Nodes[index];
          LoanFolderAclInfo tag = (LoanFolderAclInfo) node.Tag;
          tag.MoveFromAccess = !node.Checked ? 0 : 1;
          loanFolderAclInfoList[index] = tag;
        }
        this.aclMgr.SetPermissions(AclFeature.LoanMgmt_Import, loanFolderAclInfoList, this.personaID);
      }
      else
      {
        ArrayList arrayList = new ArrayList();
        for (int index = 0; index < this.tvImport.Nodes.Count; ++index)
        {
          LoanFolderAclInfo tag = (LoanFolderAclInfo) this.tvImport.Nodes[index].Tag;
          if (tag.CustomMoveFrom)
            arrayList.Add((object) tag);
        }
        this.aclMgr.SetPermissions(AclFeature.LoanMgmt_Import, (LoanFolderAclInfo[]) arrayList.ToArray(typeof (LoanFolderAclInfo)), this.userID);
      }
    }

    public bool HasBeenModified() => this.dirty;

    private void tvImport_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
    {
      e.Cancel = true;
    }

    private void tvImport_AfterCheck(object sender, TreeViewEventArgs e)
    {
      if (this.internalAccess)
        return;
      e.Node.ImageIndex = 0;
      e.Node.SelectedImageIndex = 0;
      LoanFolderAclInfo tag = (LoanFolderAclInfo) e.Node.Tag;
      tag.CustomMoveFrom = true;
      tag.MoveFromAccess = !e.Node.Checked ? 0 : 1;
      e.Node.Tag = (object) tag;
      this.dirty = true;
    }

    private void miLink_Click(object sender, EventArgs e)
    {
      TreeNode selectedNode = this.tvImport.SelectedNode;
      this.internalAccess = true;
      if (selectedNode != null && selectedNode.ImageIndex < 1)
      {
        this.dirty = true;
        LoanFolderAclInfo permission = this.aclMgr.GetPermission(AclFeature.LoanMgmt_Import, this.personaList, selectedNode.Text);
        if (permission != null)
        {
          this.dirty = true;
          selectedNode.Checked = permission.MoveFromAccess == 1;
          selectedNode.ImageIndex = 1;
          selectedNode.SelectedImageIndex = 1;
          ((LoanFolderAclInfo) selectedNode.Tag).CustomMoveFrom = false;
          ((LoanFolderAclInfo) selectedNode.Tag).MoveFromAccess = permission.MoveFromAccess;
        }
      }
      this.internalAccess = false;
    }

    private void miDisconnect_Click(object sender, EventArgs e)
    {
      TreeNode selectedNode = this.tvImport.SelectedNode;
      this.internalAccess = true;
      if (selectedNode != null && selectedNode.ImageIndex > 0)
      {
        this.dirty = true;
        selectedNode.ImageIndex = 0;
        selectedNode.SelectedImageIndex = 0;
        ((LoanFolderAclInfo) selectedNode.Tag).CustomMoveFrom = true;
      }
      this.internalAccess = false;
    }

    private void tvImport_MouseUp(object sender, MouseEventArgs e)
    {
      TreeNode nodeAt = this.tvImport.GetNodeAt(e.X, e.Y);
      if (nodeAt == null)
        return;
      this.tvImport.SelectedNode = nodeAt;
    }

    private void tvImport_BeforeCheck(object sender, TreeViewCancelEventArgs e)
    {
      if (!this.readOnly)
        return;
      e.Cancel = true;
    }

    public bool HasSomethingChecked()
    {
      bool flag = false;
      foreach (TreeNode node in this.tvImport.Nodes)
      {
        if (node.Checked)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private void SelectAll()
    {
      foreach (TreeNode node in this.tvImport.Nodes)
        node.Checked = true;
    }

    private void DeselectAll()
    {
      foreach (TreeNode node in this.tvImport.Nodes)
        node.Checked = false;
    }

    public int GetImageIndex()
    {
      int imageIndex = 0;
      foreach (TreeNode node in this.tvImport.Nodes)
      {
        if (node.ImageIndex == 0)
        {
          imageIndex = 1;
          break;
        }
      }
      return imageIndex;
    }

    private void setView()
    {
      this.previousView = new ArrayList();
      foreach (TreeNode node in this.tvImport.Nodes)
        this.previousView.Add(node.Clone());
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.internalAccess = true;
      this.tvImport.Nodes.Clear();
      foreach (TreeNode treeNode in this.previousView)
        this.tvImport.Nodes.Add((TreeNode) treeNode.Clone());
      this.internalAccess = false;
    }

    public ArrayList DataView
    {
      get => this.previousView;
      set => this.previousView = value;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.setView();
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void ImportLoanDlg_Closing(object sender, CancelEventArgs e)
    {
      this.btnCancel_Click((object) null, (EventArgs) null);
    }
  }
}

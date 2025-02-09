// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoansGroupPage
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
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoansGroupPage : Form, IGroupSecurityPage
  {
    private Sessions.Session session;
    private string userID = "";
    private AclGroup[] groupList;
    private bool personal;
    private bool firstTime = true;
    private bool requireUpdate;
    private bool requireUpdateLoanFolder;
    private ResourceSetViewer orgView;
    private int _currentGroupId = -1;
    private bool dirty;
    private bool initial = true;
    private bool reloadFromDB = true;
    private Label label2;
    private ListView listViewLoanFolders;
    private ColumnHeader columnHeaderName;
    private ColumnHeader columnHeaderAccessRight;
    private ColumnHeader columnHeaderLoanFolder;
    private IContainer components;
    private ListViewEx listViewGroupLoan;
    private ImageList imgListTv;
    private ComboBox cmbBoxAccessRight;
    private UserGroupLegend legend;
    private GroupContainer gcLoanFolders;
    private Splitter splitter1;
    private GroupContainer gcOthersLoans;
    private StandardIconButton stdIconBtnNew;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnDelete;
    private Button btnSelectAll;
    private Button btnDeselectAll;
    private PanelEx pnlExLbl;
    private bool isTPOMVP;
    private bool oldAccessIsViewOnly;

    public event EventHandler DirtyFlagChanged;

    private int currentGroupId
    {
      get => this._currentGroupId;
      set
      {
        if (this.firstTime)
        {
          this._currentGroupId = value;
          this.initial = true;
          this.setDirtyFlag(false);
          this.loadGroupLoanMembers();
          if (!this.isTPOMVP)
            this.loadLoanFolders();
          this.reloadFromDB = true;
          this.orgView = (ResourceSetViewer) null;
          this.initial = false;
        }
        else if (!this.firstTime && !this._currentGroupId.Equals(value))
        {
          this.firstTime = true;
          this._currentGroupId = value;
          this.setDirtyFlag(false);
          this.initial = true;
          this.loadGroupLoanMembers();
          if (!this.isTPOMVP)
            this.loadLoanFolders();
          this.reloadFromDB = true;
          this.orgView = (ResourceSetViewer) null;
          this.initial = false;
        }
        else
        {
          if (this.firstTime || !this._currentGroupId.Equals(value))
            return;
          this.initial = false;
          this.reloadFromDB = false;
        }
      }
    }

    public LoansGroupPage(Sessions.Session session, EventHandler dirtyFlagChanged, bool isTPOMVP = false)
    {
      this.InitializeComponent();
      this.isTPOMVP = isTPOMVP;
      this.gcLoanFolders.Visible = !isTPOMVP;
      this.session = session;
      this.init(dirtyFlagChanged);
    }

    public LoansGroupPage(
      Sessions.Session session,
      string userID,
      AclGroup[] groups,
      EventHandler dirtyFlagChanged,
      bool isTPOMVP = false)
    {
      this.InitializeComponent();
      this.isTPOMVP = isTPOMVP;
      this.session = session;
      this.userID = userID;
      this.groupList = groups;
      this.loadUserGroupLoanMembers();
      if (this.gcLoanFolders.Visible = !isTPOMVP)
        this.loadUserLoanFolders();
      this.makeReadOnly();
      this.initial = false;
      this.init(dirtyFlagChanged);
    }

    public void init(EventHandler dirtyFlagChanged)
    {
      this.cmbBoxAccessRight.Items.AddRange((object[]) new string[2]
      {
        "View Only",
        "Edit"
      });
      this.listViewGroupLoan_SelectedIndexChanged((object) this, (EventArgs) null);
      if (dirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged += dirtyFlagChanged;
    }

    private void setDirtyFlag(bool val)
    {
      this.dirty = val;
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, (EventArgs) null);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoansGroupPage));
      this.label2 = new Label();
      this.columnHeaderName = new ColumnHeader();
      this.columnHeaderAccessRight = new ColumnHeader();
      this.imgListTv = new ImageList(this.components);
      this.listViewLoanFolders = new ListView();
      this.columnHeaderLoanFolder = new ColumnHeader();
      this.cmbBoxAccessRight = new ComboBox();
      this.listViewGroupLoan = new ListViewEx();
      this.gcLoanFolders = new GroupContainer();
      this.btnSelectAll = new Button();
      this.btnDeselectAll = new Button();
      this.splitter1 = new Splitter();
      this.gcOthersLoans = new GroupContainer();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.legend = new UserGroupLegend();
      this.pnlExLbl = new PanelEx();
      this.toolTip1 = new ToolTip(this.components);
      this.gcLoanFolders.SuspendLayout();
      this.gcOthersLoans.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.pnlExLbl.SuspendLayout();
      this.SuspendLayout();
      this.label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.label2.Location = new Point(7, 4);
      this.label2.Name = "label2";
      this.label2.Size = new Size(544, 30);
      this.label2.TabIndex = 1;
      this.label2.Text = "In addition to the loans of subordinates in the organization hierarchy, the members can access the loans of the users listed below.";
      this.columnHeaderName.Text = "Name";
      this.columnHeaderName.Width = 216;
      this.columnHeaderAccessRight.Text = "Access Right";
      this.columnHeaderAccessRight.Width = 115;
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "members-this-group-and-below.png");
      this.imgListTv.Images.SetKeyName(1, "members-this-group.png");
      this.imgListTv.Images.SetKeyName(2, "member-group.png");
      this.imgListTv.Images.SetKeyName(3, "");
      this.imgListTv.Images.SetKeyName(4, "");
      this.imgListTv.Images.SetKeyName(5, "");
      this.imgListTv.Images.SetKeyName(6, "");
      this.listViewLoanFolders.BorderStyle = BorderStyle.None;
      this.listViewLoanFolders.CheckBoxes = true;
      this.listViewLoanFolders.Columns.AddRange(new ColumnHeader[1]
      {
        this.columnHeaderLoanFolder
      });
      this.listViewLoanFolders.Dock = DockStyle.Fill;
      this.listViewLoanFolders.FullRowSelect = true;
      this.listViewLoanFolders.HeaderStyle = ColumnHeaderStyle.None;
      this.listViewLoanFolders.Location = new Point(1, 26);
      this.listViewLoanFolders.Name = "listViewLoanFolders";
      this.listViewLoanFolders.Size = new Size(557, 141);
      this.listViewLoanFolders.TabIndex = 7;
      this.listViewLoanFolders.UseCompatibleStateImageBehavior = false;
      this.listViewLoanFolders.View = View.Details;
      this.listViewLoanFolders.ItemCheck += new ItemCheckEventHandler(this.listViewLoanFolders_ItemCheck);
      this.columnHeaderLoanFolder.Text = "Loan Folder";
      this.columnHeaderLoanFolder.Width = 244;
      this.cmbBoxAccessRight.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxAccessRight.Location = new Point(149, 3);
      this.cmbBoxAccessRight.Name = "cmbBoxAccessRight";
      this.cmbBoxAccessRight.Size = new Size(92, 21);
      this.cmbBoxAccessRight.TabIndex = 9;
      this.cmbBoxAccessRight.Visible = false;
      this.listViewGroupLoan.AllowColumnReorder = true;
      this.listViewGroupLoan.BorderStyle = BorderStyle.None;
      this.listViewGroupLoan.Columns.AddRange(new ColumnHeader[2]
      {
        this.columnHeaderName,
        this.columnHeaderAccessRight
      });
      this.listViewGroupLoan.Dock = DockStyle.Fill;
      this.listViewGroupLoan.DoubleClickActivation = false;
      this.listViewGroupLoan.FullRowSelect = true;
      this.listViewGroupLoan.GridLines = true;
      this.listViewGroupLoan.Location = new Point(1, 63);
      this.listViewGroupLoan.Name = "listViewGroupLoan";
      this.listViewGroupLoan.Size = new Size(557, 177);
      this.listViewGroupLoan.SmallImageList = this.imgListTv;
      this.listViewGroupLoan.TabIndex = 4;
      this.listViewGroupLoan.UseCompatibleStateImageBehavior = false;
      this.listViewGroupLoan.View = View.Details;
      this.listViewGroupLoan.SubItemClicked += new SubItemEventHandler(this.listViewGroupLoan_SubItemClicked);
      this.listViewGroupLoan.SubItemEndEditing += new SubItemEndEditingEventHandler(this.listViewGroupLoan_SubItemEndEditing);
      this.listViewGroupLoan.SelectedIndexChanged += new EventHandler(this.listViewGroupLoan_SelectedIndexChanged);
      this.gcLoanFolders.Controls.Add((Control) this.btnSelectAll);
      this.gcLoanFolders.Controls.Add((Control) this.btnDeselectAll);
      this.gcLoanFolders.Controls.Add((Control) this.listViewLoanFolders);
      this.gcLoanFolders.Dock = DockStyle.Bottom;
      this.gcLoanFolders.Location = new Point(0, 309);
      this.gcLoanFolders.Name = "gcLoanFolders";
      this.gcLoanFolders.Size = new Size(559, 168);
      this.gcLoanFolders.TabIndex = 10;
      this.gcLoanFolders.Text = "Access to Loan Folders";
      this.btnSelectAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSelectAll.BackColor = SystemColors.Control;
      this.btnSelectAll.Location = new Point(400, 3);
      this.btnSelectAll.Name = "btnSelectAll";
      this.btnSelectAll.Size = new Size(75, 21);
      this.btnSelectAll.TabIndex = 9;
      this.btnSelectAll.Text = "Select All";
      this.btnSelectAll.UseVisualStyleBackColor = true;
      this.btnSelectAll.Click += new EventHandler(this.btnSelectAll_Click);
      this.btnDeselectAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeselectAll.BackColor = SystemColors.Control;
      this.btnDeselectAll.Location = new Point(479, 3);
      this.btnDeselectAll.Name = "btnDeselectAll";
      this.btnDeselectAll.Size = new Size(75, 21);
      this.btnDeselectAll.TabIndex = 8;
      this.btnDeselectAll.Text = "Deselect All";
      this.btnDeselectAll.UseVisualStyleBackColor = true;
      this.btnDeselectAll.Click += new EventHandler(this.btnDeselectAll_Click);
      this.splitter1.Dock = DockStyle.Bottom;
      this.splitter1.Location = new Point(0, 306);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(559, 3);
      this.splitter1.TabIndex = 11;
      this.splitter1.TabStop = false;
      this.gcOthersLoans.Controls.Add((Control) this.cmbBoxAccessRight);
      this.gcOthersLoans.Controls.Add((Control) this.listViewGroupLoan);
      this.gcOthersLoans.Controls.Add((Control) this.stdIconBtnNew);
      this.gcOthersLoans.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcOthersLoans.Controls.Add((Control) this.legend);
      this.gcOthersLoans.Controls.Add((Control) this.pnlExLbl);
      this.gcOthersLoans.Dock = DockStyle.Fill;
      this.gcOthersLoans.Location = new Point(0, 0);
      this.gcOthersLoans.Name = "gcOthersLoans";
      this.gcOthersLoans.Size = new Size(559, 306);
      this.gcOthersLoans.TabIndex = 12;
      this.gcOthersLoans.Text = "Access to Other's Loans";
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(514, 5);
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 16;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "Add");
      this.stdIconBtnNew.Click += new EventHandler(this.btnSelect_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(536, 5);
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 15;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.stdIconBtnDelete_Click);
      this.legend.BackColor = Color.Transparent;
      this.legend.Dock = DockStyle.Bottom;
      this.legend.Location = new Point(1, 240);
      this.legend.Name = "legend";
      this.legend.Size = new Size(557, 65);
      this.legend.TabIndex = 14;
      this.pnlExLbl.Controls.Add((Control) this.label2);
      this.pnlExLbl.Dock = DockStyle.Top;
      this.pnlExLbl.Location = new Point(1, 26);
      this.pnlExLbl.Name = "pnlExLbl";
      this.pnlExLbl.Size = new Size(557, 37);
      this.pnlExLbl.TabIndex = 13;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(559, 477);
      this.Controls.Add((Control) this.gcOthersLoans);
      this.Controls.Add((Control) this.splitter1);
      this.Controls.Add((Control) this.gcLoanFolders);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (LoansGroupPage);
      this.StartPosition = FormStartPosition.CenterParent;
      this.gcLoanFolders.ResumeLayout(false);
      this.gcOthersLoans.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.pnlExLbl.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public virtual void SetGroup(int groupId) => this.currentGroupId = groupId;

    private void makeReadOnly()
    {
      this.stdIconBtnNew.Enabled = false;
      this.personal = true;
      this.listViewLoanFolders.CheckBoxes = false;
    }

    private string getAccessString(AclResourceAccess access)
    {
      return access == AclResourceAccess.ReadOnly ? "View Only" : "Edit";
    }

    private void loadGroupLoanMembers()
    {
      this.listViewGroupLoan.Items.Clear();
      AclGroupLoanMembers membersInGroupLoan = this.session.AclGroupManager.GetMembersInGroupLoan(this.currentGroupId);
      ListViewItem[] items1 = new ListViewItem[membersInGroupLoan.OrgMembers.Length];
      for (int index = 0; index < membersInGroupLoan.OrgMembers.Length; ++index)
      {
        OrgInGroupLoan orgMember = membersInGroupLoan.OrgMembers[index];
        items1[index] = new ListViewItem(new string[2]
        {
          orgMember.OrgName,
          this.getAccessString(orgMember.Access)
        });
        int num = orgMember.IsInclusive ? 0 : 1;
        items1[index].ImageIndex = num;
        items1[index].ImageIndex = num;
        items1[index].Tag = (object) new TreeNode(orgMember.OrgName)
        {
          Tag = (object) orgMember,
          ImageIndex = num,
          SelectedImageIndex = num
        };
      }
      ListViewItem[] items2 = new ListViewItem[membersInGroupLoan.UserMembers.Length];
      if (membersInGroupLoan.UserMembers != null && membersInGroupLoan.UserMembers.Length != 0)
      {
        List<string> stringList = new List<string>();
        foreach (UserInGroupLoan userMember in membersInGroupLoan.UserMembers)
        {
          if (!stringList.Contains(userMember.UserID))
            stringList.Add(userMember.UserID);
        }
        Hashtable users = this.session.OrganizationManager.GetUsers(stringList.ToArray());
        for (int index = 0; index < membersInGroupLoan.UserMembers.Length; ++index)
        {
          UserInGroupLoan userMember = membersInGroupLoan.UserMembers[index];
          if (users.ContainsKey((object) userMember.UserID))
          {
            UserInfo userInfo = (UserInfo) users[(object) userMember.UserID];
            items2[index] = new ListViewItem(new string[2]
            {
              userInfo.FullName + " (" + userInfo.Userid + ")",
              this.getAccessString(userMember.Access)
            });
            items2[index].ImageIndex = 2;
            items2[index].Tag = (object) new TreeNode(userInfo.FullName + " (" + userInfo.Userid + ")")
            {
              Tag = (object) userMember,
              ImageIndex = 2,
              SelectedImageIndex = 2
            };
          }
        }
      }
      this.listViewGroupLoan.Items.AddRange(items1);
      this.listViewGroupLoan.Items.AddRange(items2);
    }

    private void addNodeToList(TreeNode node, ArrayList orgList, ArrayList userList)
    {
      ListViewItem listViewItem1 = (ListViewItem) null;
      object tag = node.Tag;
      TreeNode treeNode = (TreeNode) node.Clone();
      bool flag = false;
      switch (treeNode.ImageIndex)
      {
        case 0:
        case 1:
        case 2:
          flag = true;
          break;
      }
      if (node.Text == "<DUMMY NODE>")
        flag = false;
      if (flag)
      {
        foreach (ListViewItem listViewItem2 in this.listViewGroupLoan.Items)
        {
          if (listViewItem2.Text == treeNode.Text)
          {
            listViewItem1 = listViewItem2;
            break;
          }
        }
        if (listViewItem1 == null)
        {
          switch (treeNode.ImageIndex)
          {
            case 0:
              listViewItem1 = new ListViewItem(new string[2]
              {
                treeNode.Text,
                "View Only"
              });
              listViewItem1.ImageIndex = treeNode.ImageIndex;
              OrgInGroupLoan orgInGroupLoan1 = !(treeNode.Tag is OrgInfo) ? new OrgInGroupLoan(((OrgInGroup) treeNode.Tag).OrgID, true, AclResourceAccess.ReadOnly, ((OrgInGroup) treeNode.Tag).OrgName) : new OrgInGroupLoan(((OrgInfo) treeNode.Tag).Oid, true, AclResourceAccess.ReadOnly, ((OrgInfo) treeNode.Tag).OrgName);
              treeNode.Tag = (object) orgInGroupLoan1;
              listViewItem1.Tag = (object) treeNode;
              break;
            case 1:
              listViewItem1 = new ListViewItem(new string[2]
              {
                treeNode.Text,
                "View Only"
              });
              listViewItem1.ImageIndex = treeNode.ImageIndex;
              OrgInGroupLoan orgInGroupLoan2 = !(treeNode.Tag is OrgInfo) ? new OrgInGroupLoan(((OrgInGroup) treeNode.Tag).OrgID, false, AclResourceAccess.ReadOnly, ((OrgInGroup) treeNode.Tag).OrgName) : new OrgInGroupLoan(((OrgInfo) treeNode.Tag).Oid, false, AclResourceAccess.ReadOnly, ((OrgInfo) treeNode.Tag).OrgName);
              treeNode.Tag = (object) orgInGroupLoan2;
              listViewItem1.Tag = (object) treeNode;
              break;
            case 2:
              listViewItem1 = new ListViewItem(new string[2]
              {
                treeNode.Text,
                "View Only"
              });
              listViewItem1.ImageIndex = treeNode.ImageIndex;
              UserInGroupLoan userInGroupLoan = (object) (treeNode.Tag as UserInGroupLoan) == null ? new UserInGroupLoan(((UserInfo) treeNode.Tag).Userid, AclResourceAccess.ReadOnly) : new UserInGroupLoan(((UserInGroupLoan) treeNode.Tag).UserID, AclResourceAccess.ReadOnly);
              treeNode.Tag = (object) userInGroupLoan;
              listViewItem1.Tag = (object) treeNode;
              break;
          }
        }
        else
        {
          switch (treeNode.ImageIndex)
          {
            case 0:
              listViewItem1.ImageIndex = treeNode.ImageIndex;
              break;
            case 1:
              listViewItem1.ImageIndex = treeNode.ImageIndex;
              break;
            case 2:
              listViewItem1.ImageIndex = treeNode.ImageIndex;
              break;
          }
        }
        if (tag is OrgInfo)
          orgList.Add((object) listViewItem1);
        else
          userList.Add((object) listViewItem1);
      }
      foreach (TreeNode node1 in node.Nodes)
        this.addNodeToList(node1, orgList, userList);
    }

    private void loadGroupLoanMembers(TreeView tv)
    {
      ArrayList orgList = new ArrayList();
      ArrayList userList = new ArrayList();
      foreach (TreeNode node in tv.Nodes)
        this.addNodeToList(node, orgList, userList);
      this.listViewGroupLoan.Items.Clear();
      this.listViewGroupLoan.Items.AddRange((ListViewItem[]) orgList.ToArray(typeof (ListViewItem)));
      this.listViewGroupLoan.Items.AddRange((ListViewItem[]) userList.ToArray(typeof (ListViewItem)));
    }

    private void loadUserGroupLoanMembers()
    {
      this.listViewGroupLoan.Items.Clear();
      Hashtable hashtable1 = new Hashtable();
      Hashtable hashtable2 = new Hashtable();
      foreach (AclGroup group in this.groupList)
      {
        AclGroupLoanMembers membersInGroupLoan = this.session.AclGroupManager.GetMembersInGroupLoan(group.ID);
        for (int index = 0; index < membersInGroupLoan.OrgMembers.Length; ++index)
        {
          OrgInGroupLoan orgMember = membersInGroupLoan.OrgMembers[index];
          string accessString = this.getAccessString(orgMember.Access);
          string key = orgMember.OrgName + "_" + orgMember.IsInclusive.ToString();
          if (hashtable1[(object) key] == null || this.getAccessString(((OrgInGroupLoan) ((TreeNode) ((ListViewItem) hashtable1[(object) key]).Tag).Tag).Access) != "Edit" && accessString == "Edit")
          {
            if (hashtable1[(object) key] != null)
              hashtable1.Remove((object) key);
            ListViewItem listViewItem = new ListViewItem(new string[2]
            {
              orgMember.OrgName,
              accessString
            });
            int num = orgMember.IsInclusive ? 0 : 1;
            listViewItem.ImageIndex = num;
            listViewItem.Tag = (object) new TreeNode(orgMember.OrgName)
            {
              Tag = (object) orgMember,
              ImageIndex = num,
              SelectedImageIndex = num
            };
            hashtable1.Add((object) key, (object) listViewItem);
          }
        }
        if (membersInGroupLoan.UserMembers != null && membersInGroupLoan.UserMembers.Length != 0)
        {
          List<string> stringList = new List<string>();
          foreach (UserInGroupLoan userMember in membersInGroupLoan.UserMembers)
          {
            if (!stringList.Contains(userMember.UserID))
              stringList.Add(userMember.UserID);
          }
          Hashtable users = this.session.OrganizationManager.GetUsers(stringList.ToArray());
          for (int index = 0; index < membersInGroupLoan.UserMembers.Length; ++index)
          {
            UserInGroupLoan userMember = membersInGroupLoan.UserMembers[index];
            UserInfo userInfo = (UserInfo) users[(object) userMember.UserID];
            string userId = userMember.UserID;
            string accessString = this.getAccessString(userMember.Access);
            if (hashtable2[(object) userId] == null || this.getAccessString(((UserInGroupLoan) ((TreeNode) ((ListViewItem) hashtable2[(object) userId]).Tag).Tag).Access) != "Edit" && accessString == "Edit")
            {
              if (hashtable2[(object) userId] != null)
                hashtable2.Remove((object) userId);
              hashtable2.Add((object) userId, (object) new ListViewItem(new string[2]
              {
                userInfo.FullName + " (" + userInfo.Userid + ")",
                accessString
              })
              {
                ImageIndex = 2,
                Tag = (object) new TreeNode(userInfo.FullName + " (" + userInfo.Userid + ")")
                {
                  Tag = (object) userMember,
                  ImageIndex = 2,
                  SelectedImageIndex = 2
                }
              });
            }
          }
        }
      }
      if (hashtable1.Count > 0)
      {
        ArrayList arrayList = new ArrayList();
        IDictionaryEnumerator enumerator = hashtable1.GetEnumerator();
        while (enumerator.MoveNext())
          arrayList.Add((object) (ListViewItem) enumerator.Value);
        this.listViewGroupLoan.Items.AddRange((ListViewItem[]) arrayList.ToArray(typeof (ListViewItem)));
      }
      if (hashtable2.Count <= 0)
        return;
      ArrayList arrayList1 = new ArrayList();
      IDictionaryEnumerator enumerator1 = hashtable2.GetEnumerator();
      while (enumerator1.MoveNext())
        arrayList1.Add((object) (ListViewItem) enumerator1.Value);
      this.listViewGroupLoan.Items.AddRange((ListViewItem[]) arrayList1.ToArray(typeof (ListViewItem)));
    }

    private void loadLoanFolders()
    {
      this.listViewLoanFolders.Items.Clear();
      LoanFolderInGroup[] groupLoanFolders = this.session.AclGroupManager.GetAclGroupLoanFolders(this.currentGroupId);
      Hashtable hashtable = new Hashtable(groupLoanFolders.Length);
      for (int index = 0; index < groupLoanFolders.Length; ++index)
        hashtable.Add((object) groupLoanFolders[index].FolderName, (object) groupLoanFolders[index]);
      LoanFolderInfo[] allLoanFolderInfos = this.session.LoanManager.GetAllLoanFolderInfos(true, false);
      if (allLoanFolderInfos == null || allLoanFolderInfos.Length == 0)
        return;
      ListViewItem[] items = new ListViewItem[allLoanFolderInfos.Length];
      for (int index = 0; index < allLoanFolderInfos.Length; ++index)
      {
        items[index] = new ListViewItem(new string[1]
        {
          allLoanFolderInfos[index].DisplayName
        });
        items[index].Tag = (object) allLoanFolderInfos[index].Name;
        if (hashtable.Contains((object) allLoanFolderInfos[index].Name))
        {
          LoanFolderInGroup loanFolderInGroup = (LoanFolderInGroup) hashtable[(object) allLoanFolderInfos[index].Name];
          items[index].Checked = loanFolderInGroup.Accessible;
        }
      }
      this.listViewLoanFolders.Items.AddRange(items);
    }

    private void loadUserLoanFolders()
    {
      this.listViewLoanFolders.Items.Clear();
      Hashtable hashtable = new Hashtable();
      foreach (AclGroup group in this.groupList)
      {
        LoanFolderInGroup[] groupLoanFolders = this.session.AclGroupManager.GetAclGroupLoanFolders(group.ID);
        for (int index = 0; index < groupLoanFolders.Length; ++index)
        {
          if (groupLoanFolders[index].Accessible && hashtable[(object) groupLoanFolders[index].FolderName] == null)
            hashtable.Add((object) groupLoanFolders[index].FolderName, (object) new ListViewItem(groupLoanFolders[index].DisplayName)
            {
              Tag = (object) groupLoanFolders[index].FolderName
            });
        }
      }
      if (hashtable.Count <= 0)
        return;
      ArrayList arrayList = new ArrayList();
      IDictionaryEnumerator enumerator = hashtable.GetEnumerator();
      while (enumerator.MoveNext())
        arrayList.Add((object) (ListViewItem) enumerator.Value);
      this.listViewLoanFolders.Items.AddRange((ListViewItem[]) arrayList.ToArray(typeof (ListViewItem)));
    }

    private TreeNode[] GetResetTreeView()
    {
      ArrayList arrayList = new ArrayList();
      foreach (ListViewItem listViewItem in this.listViewGroupLoan.Items)
      {
        if (listViewItem.ImageIndex != ((TreeNode) listViewItem.Tag).ImageIndex)
        {
          ((TreeNode) listViewItem.Tag).ImageIndex = listViewItem.ImageIndex;
          ((TreeNode) listViewItem.Tag).SelectedImageIndex = listViewItem.ImageIndex;
        }
        arrayList.Add((object) (TreeNode) listViewItem.Tag);
      }
      return (TreeNode[]) arrayList.ToArray(typeof (TreeNode));
    }

    private void listViewGroupLoan_SubItemClicked(object sender, SubItemEventArgs e)
    {
      if (e.SubItem != 1 || this.personal)
        return;
      this.oldAccessIsViewOnly = e.Item.SubItems[1].Text == "View Only";
      this.listViewGroupLoan.StartEditing((Control) this.cmbBoxAccessRight, e.Item, e.SubItem);
    }

    private void listViewGroupLoan_SubItemEndEditing(object sender, SubItemEndEditingEventArgs e)
    {
      bool flag = this.cmbBoxAccessRight.Text == "View Only";
      if (flag == this.oldAccessIsViewOnly)
        return;
      this.setDirtyFlag(true);
      this.requireUpdate = true;
      if (((TreeNode) e.Item.Tag).Tag is OrgInGroupLoan)
      {
        if (flag)
          ((OrgInGroupLoan) ((TreeNode) e.Item.Tag).Tag).Access = AclResourceAccess.ReadOnly;
        else
          ((OrgInGroupLoan) ((TreeNode) e.Item.Tag).Tag).Access = AclResourceAccess.ReadWrite;
      }
      else if (flag)
        ((UserInGroupLoan) ((TreeNode) e.Item.Tag).Tag).Access = AclResourceAccess.ReadOnly;
      else
        ((UserInGroupLoan) ((TreeNode) e.Item.Tag).Tag).Access = AclResourceAccess.ReadWrite;
    }

    private void listViewLoanFolders_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (this.initial || e.CurrentValue == e.NewValue)
        return;
      this.requireUpdateLoanFolder = true;
      this.setDirtyFlag(true);
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, (EventArgs) null);
    }

    public void SaveData()
    {
      if (!this.firstTime && this.orgView != null)
        this.orgView.Save();
      if (this.requireUpdate)
        this.UpdateAccessRight();
      if (this.requireUpdateLoanFolder)
        this.SaveLoanFoldersList();
      this.setDirtyFlag(false);
    }

    public bool HasBeenModified()
    {
      return this.dirty || this.orgView != null && this.orgView.HasBeenModified();
    }

    public void ResetData()
    {
      this.firstTime = true;
      this.currentGroupId = this.currentGroupId;
    }

    private void SaveLoanFoldersList()
    {
      if (this.personal || !this.requireUpdateLoanFolder)
        return;
      foreach (ListViewItem listViewItem in this.listViewLoanFolders.Items)
        this.session.AclGroupManager.UpdateAclGroupLoanFolder(new LoanFolderInGroup(this.currentGroupId, (string) listViewItem.Tag, listViewItem.Checked));
      this.requireUpdateLoanFolder = false;
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      this.firstTime = true;
      this.currentGroupId = this.currentGroupId;
    }

    private void UpdateAccessRight()
    {
      if (!this.requireUpdate)
        return;
      List<OrgInGroupLoan> orgInGroupLoanList = new List<OrgInGroupLoan>();
      List<UserInGroupLoan> userInGroupLoanList = new List<UserInGroupLoan>();
      foreach (ListViewItem listViewItem in this.listViewGroupLoan.Items)
      {
        if (((TreeNode) listViewItem.Tag).Tag is OrgInGroupLoan)
        {
          OrgInGroupLoan tag = (OrgInGroupLoan) ((TreeNode) listViewItem.Tag).Tag;
          orgInGroupLoanList.Add(tag);
        }
        else if ((object) (((TreeNode) listViewItem.Tag).Tag as UserInGroupLoan) != null)
        {
          UserInGroupLoan tag = (UserInGroupLoan) ((TreeNode) listViewItem.Tag).Tag;
          userInGroupLoanList.Add(tag);
        }
      }
      if (orgInGroupLoanList.Count > 0 || userInGroupLoanList.Count > 0)
        this.session.AclGroupManager.UpdateMembersInGroupLoan(this.currentGroupId, userInGroupLoanList.ToArray(), orgInGroupLoanList.ToArray());
      this.requireUpdate = false;
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (!this.continueForAllUsersGroup())
        return;
      if (this.reloadFromDB)
      {
        this.orgView = new ResourceSetViewer(this.session, (object) this.session.AclGroupManager.GetMembersInGroupLoan(this.currentGroupId), this.DirtyFlagChanged, this._currentGroupId);
        this.orgView.SetResetDataSourceTreeNode = this.GetResetTreeView();
      }
      this.reloadFromDB = false;
      this.firstTime = false;
      if (DialogResult.Cancel == this.orgView.ShowDialog((IWin32Window) this))
        return;
      this.loadGroupLoanMembers((TreeView) this.orgView.GetCurrentTreeView());
      this.orgView.SetResetDataSourceTreeNode = this.GetResetTreeView();
    }

    private void stdIconBtnDelete_Click(object sender, EventArgs e)
    {
      if (this.listViewGroupLoan.SelectedItems == null || this.listViewGroupLoan.SelectedItems.Count == 0)
        return;
      this.Cursor = Cursors.WaitCursor;
      if (this.reloadFromDB)
        this.orgView = new ResourceSetViewer(this.session, (object) this.session.AclGroupManager.GetMembersInGroupLoan(this.currentGroupId), this.DirtyFlagChanged, this._currentGroupId);
      List<TreeNode> treeNodeList = new List<TreeNode>();
      foreach (ListViewItem selectedItem in this.listViewGroupLoan.SelectedItems)
      {
        treeNodeList.Add((TreeNode) selectedItem.Tag);
        selectedItem.Remove();
      }
      this.firstTime = false;
      this.reloadFromDB = false;
      this.orgView.SetResetDataSourceTreeNode = this.GetResetTreeView();
      this.orgView.ResetTreeWithMemoryData();
      foreach (TreeNode node in treeNodeList)
        this.orgView.RemoveTreeNode(node);
      this.Cursor = Cursors.Default;
    }

    private void listViewGroupLoan_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.personal)
        this.stdIconBtnDelete.Enabled = false;
      else
        this.stdIconBtnDelete.Enabled = this.listViewGroupLoan.SelectedItems.Count > 0;
    }

    private void btnSelectAll_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in this.listViewLoanFolders.Items)
        listViewItem.Checked = true;
    }

    private void btnDeselectAll_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem listViewItem in this.listViewLoanFolders.Items)
        listViewItem.Checked = false;
    }

    private bool continueForAllUsersGroup()
    {
      bool flag = true;
      if (this.currentGroupId == 1 && Utils.Dialog((IWin32Window) this, "This action will affect the \"All Users\" group, are you sure you would like to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.No)
        flag = false;
      return flag;
    }
  }
}

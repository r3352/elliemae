// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BorrowerContactsPage
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
  public class BorrowerContactsPage : Form, IGroupSecurityPage
  {
    private Sessions.Session session;
    private string userID = "";
    private AclGroup[] groupList;
    private CheckBox chkBoxViewSubordinateContacts;
    private IContainer components;
    private ListViewEx listViewGroupContact;
    private ColumnHeader columnHeaderName;
    private ColumnHeader columnHeaderAccessRight;
    private ComboBox cmbBoxAccessRight;
    private bool dirty;
    private ResourceSetViewer orgView;
    private bool firstTime = true;
    private bool oldAccessIsViewOnly;
    private bool reloadFromDB = true;
    private bool requireUpdate;
    private bool requireUpdateSuperiorCheck;
    private ImageList imgListTv;
    private ComboBox cmbSuperiorAccess;
    private Label label3;
    private GroupContainer gcGroup;
    private GroupContainer gcTop;
    private PanelEx pnlExTopLbl;
    private Label label1;
    private StandardIconButton stdIconBtnAdd;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnDelete;
    private bool personal;
    private int _currentGroupId = -1;

    public event EventHandler DirtyFlagChanged;

    public BorrowerContactsPage(
      Sessions.Session session,
      int personaId,
      EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.session = session;
      this.cmbBoxAccessRight.Items.AddRange((object[]) new string[2]
      {
        "View Only",
        "Edit"
      });
      this.cmbSuperiorAccess.Items.AddRange((object[]) new string[2]
      {
        "View Only",
        "Edit"
      });
      this.currentGroupId = personaId;
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.listViewGroupContact_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    public BorrowerContactsPage(
      Sessions.Session session,
      string userID,
      AclGroup[] groups,
      EventHandler dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.session = session;
      this.cmbBoxAccessRight.Items.AddRange((object[]) new string[2]
      {
        "View Only",
        "Edit"
      });
      this.cmbSuperiorAccess.Items.AddRange((object[]) new string[2]
      {
        "View Only",
        "Edit"
      });
      this.userID = userID;
      this.groupList = groups;
      this.loadPageValueForUser();
      this.MakeReadOnly();
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.listViewGroupContact_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    private void loadUserGroupContactMembers()
    {
      this.listViewGroupContact.Items.Clear();
      Hashtable hashtable1 = new Hashtable();
      Hashtable hashtable2 = new Hashtable();
      foreach (AclGroup group in this.groupList)
      {
        AclGroupContactMembers membersInGroupContact = this.session.AclGroupManager.GetMembersInGroupContact(group.ID);
        for (int index = 0; index < membersInGroupContact.OrgMembers.Length; ++index)
        {
          OrgInGroupContact orgMember = membersInGroupContact.OrgMembers[index];
          string accessString = this.getAccessString(orgMember.Access);
          string key = orgMember.OrgName + "_" + orgMember.IsInclusive.ToString();
          if (hashtable1[(object) key] == null || this.getAccessString(((OrgInGroupContact) ((TreeNode) ((ListViewItem) hashtable1[(object) key]).Tag).Tag).Access) != "Edit" && accessString == "Edit")
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
        if (membersInGroupContact.UserMembers != null && membersInGroupContact.UserMembers.Length != 0)
        {
          List<string> stringList = new List<string>();
          foreach (UserInGroupContact userMember in membersInGroupContact.UserMembers)
          {
            if (!stringList.Contains(userMember.UserID))
              stringList.Add(userMember.UserID);
          }
          Hashtable users = this.session.OrganizationManager.GetUsers(stringList.ToArray());
          for (int index = 0; index < membersInGroupContact.UserMembers.Length; ++index)
          {
            UserInGroupContact userMember = membersInGroupContact.UserMembers[index];
            UserInfo userInfo = (UserInfo) users[(object) userMember.UserID];
            string userId = userMember.UserID;
            string accessString = this.getAccessString(userMember.Access);
            if (hashtable2[(object) userId] == null || this.getAccessString(((UserInGroupContact) ((TreeNode) ((ListViewItem) hashtable2[(object) userId]).Tag).Tag).Access) != "Edit" && accessString == "Edit")
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
        this.listViewGroupContact.Items.AddRange((ListViewItem[]) arrayList.ToArray(typeof (ListViewItem)));
      }
      if (hashtable2.Count <= 0)
        return;
      ArrayList arrayList1 = new ArrayList();
      IDictionaryEnumerator enumerator1 = hashtable2.GetEnumerator();
      while (enumerator1.MoveNext())
        arrayList1.Add((object) (ListViewItem) enumerator1.Value);
      this.listViewGroupContact.Items.AddRange((ListViewItem[]) arrayList1.ToArray(typeof (ListViewItem)));
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (BorrowerContactsPage));
      this.chkBoxViewSubordinateContacts = new CheckBox();
      this.cmbBoxAccessRight = new ComboBox();
      this.listViewGroupContact = new ListViewEx();
      this.columnHeaderName = new ColumnHeader();
      this.columnHeaderAccessRight = new ColumnHeader();
      this.imgListTv = new ImageList(this.components);
      this.cmbSuperiorAccess = new ComboBox();
      this.label3 = new Label();
      this.gcGroup = new GroupContainer();
      this.gcTop = new GroupContainer();
      this.stdIconBtnAdd = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.pnlExTopLbl = new PanelEx();
      this.label1 = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.gcGroup.SuspendLayout();
      this.gcTop.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnAdd).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.pnlExTopLbl.SuspendLayout();
      this.SuspendLayout();
      this.chkBoxViewSubordinateContacts.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.chkBoxViewSubordinateContacts.Location = new Point(11, 28);
      this.chkBoxViewSubordinateContacts.Name = "chkBoxViewSubordinateContacts";
      this.chkBoxViewSubordinateContacts.Size = new Size(648, 34);
      this.chkBoxViewSubordinateContacts.TabIndex = 2;
      this.chkBoxViewSubordinateContacts.Text = "Borrower contacts created by this group are public and viewable by superiors in organization hierarchy.";
      this.chkBoxViewSubordinateContacts.CheckedChanged += new EventHandler(this.chkBoxViewSubordinateContacts_CheckedChanged);
      this.cmbBoxAccessRight.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxAccessRight.Location = new Point(246, 3);
      this.cmbBoxAccessRight.Name = "cmbBoxAccessRight";
      this.cmbBoxAccessRight.Size = new Size(92, 21);
      this.cmbBoxAccessRight.TabIndex = 13;
      this.cmbBoxAccessRight.Visible = false;
      this.listViewGroupContact.AllowColumnReorder = true;
      this.listViewGroupContact.BorderStyle = BorderStyle.None;
      this.listViewGroupContact.Columns.AddRange(new ColumnHeader[2]
      {
        this.columnHeaderName,
        this.columnHeaderAccessRight
      });
      this.listViewGroupContact.Dock = DockStyle.Fill;
      this.listViewGroupContact.DoubleClickActivation = false;
      this.listViewGroupContact.FullRowSelect = true;
      this.listViewGroupContact.GridLines = true;
      this.listViewGroupContact.Location = new Point(1, 61);
      this.listViewGroupContact.Name = "listViewGroupContact";
      this.listViewGroupContact.Size = new Size(669, 356);
      this.listViewGroupContact.SmallImageList = this.imgListTv;
      this.listViewGroupContact.TabIndex = 12;
      this.listViewGroupContact.UseCompatibleStateImageBehavior = false;
      this.listViewGroupContact.View = View.Details;
      this.listViewGroupContact.SubItemClicked += new SubItemEventHandler(this.listViewGroupContact_SubItemClicked);
      this.listViewGroupContact.SubItemEndEditing += new SubItemEndEditingEventHandler(this.listViewGroupContact_SubItemEndEditing);
      this.listViewGroupContact.SelectedIndexChanged += new EventHandler(this.listViewGroupContact_SelectedIndexChanged);
      this.columnHeaderName.Text = "Name";
      this.columnHeaderName.Width = 129;
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
      this.cmbSuperiorAccess.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbSuperiorAccess.Enabled = false;
      this.cmbSuperiorAccess.Location = new Point(111, 69);
      this.cmbSuperiorAccess.Name = "cmbSuperiorAccess";
      this.cmbSuperiorAccess.Size = new Size(92, 21);
      this.cmbSuperiorAccess.TabIndex = 14;
      this.cmbSuperiorAccess.SelectedIndexChanged += new EventHandler(this.cmbSuperiorAccess_SelectedIndexChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(33, 71);
      this.label3.Name = "label3";
      this.label3.Size = new Size(73, 13);
      this.label3.TabIndex = 15;
      this.label3.Text = "Access Right:";
      this.gcGroup.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcGroup.Controls.Add((Control) this.chkBoxViewSubordinateContacts);
      this.gcGroup.Controls.Add((Control) this.cmbSuperiorAccess);
      this.gcGroup.Controls.Add((Control) this.label3);
      this.gcGroup.Dock = DockStyle.Bottom;
      this.gcGroup.Location = new Point(0, 418);
      this.gcGroup.Name = "gcGroup";
      this.gcGroup.Size = new Size(671, 103);
      this.gcGroup.TabIndex = 16;
      this.gcGroup.Text = "Access to this Group's Borrower Contacts";
      this.gcTop.Controls.Add((Control) this.stdIconBtnAdd);
      this.gcTop.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcTop.Controls.Add((Control) this.cmbBoxAccessRight);
      this.gcTop.Controls.Add((Control) this.listViewGroupContact);
      this.gcTop.Controls.Add((Control) this.pnlExTopLbl);
      this.gcTop.Dock = DockStyle.Fill;
      this.gcTop.Location = new Point(0, 0);
      this.gcTop.Name = "gcTop";
      this.gcTop.Size = new Size(671, 418);
      this.gcTop.TabIndex = 17;
      this.gcTop.Text = "Access to Other Users' Borrower Contacts";
      this.stdIconBtnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnAdd.BackColor = Color.Transparent;
      this.stdIconBtnAdd.Location = new Point(626, 5);
      this.stdIconBtnAdd.Name = "stdIconBtnAdd";
      this.stdIconBtnAdd.Size = new Size(16, 16);
      this.stdIconBtnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnAdd.TabIndex = 16;
      this.stdIconBtnAdd.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnAdd, "Add");
      this.stdIconBtnAdd.Click += new EventHandler(this.btnSelect_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(648, 5);
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 15;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.stdIconBtnDelete_Click);
      this.pnlExTopLbl.Controls.Add((Control) this.label1);
      this.pnlExTopLbl.Dock = DockStyle.Top;
      this.pnlExTopLbl.Location = new Point(1, 26);
      this.pnlExTopLbl.Name = "pnlExTopLbl";
      this.pnlExTopLbl.Size = new Size(669, 35);
      this.pnlExTopLbl.TabIndex = 14;
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.Location = new Point(7, 3);
      this.label1.Name = "label1";
      this.label1.Size = new Size(651, 29);
      this.label1.TabIndex = 0;
      this.label1.Text = "In addition to their own contacts, members in this group can access the borrower contacts owned by the users listed below. Click the New icon above to add users to the list.";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(671, 521);
      this.Controls.Add((Control) this.gcTop);
      this.Controls.Add((Control) this.gcGroup);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (BorrowerContactsPage);
      this.gcGroup.ResumeLayout(false);
      this.gcGroup.PerformLayout();
      this.gcTop.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnAdd).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.pnlExTopLbl.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void setDirtyFlag(bool val)
    {
      this.dirty = val;
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, (EventArgs) null);
    }

    private int currentGroupId
    {
      get => this._currentGroupId;
      set
      {
        if (this.firstTime)
        {
          this._currentGroupId = value;
          this.setDirtyFlag(false);
          this.loadPageValueForGroup(value);
          this.reloadFromDB = true;
          this.orgView = (ResourceSetViewer) null;
        }
        else if (!this.firstTime && !this._currentGroupId.Equals(value))
        {
          this.firstTime = true;
          this._currentGroupId = value;
          this.setDirtyFlag(false);
          this.loadPageValueForGroup(value);
          this.reloadFromDB = true;
          this.orgView = (ResourceSetViewer) null;
        }
        else
        {
          if (this.firstTime || !this._currentGroupId.Equals(value))
            return;
          this.reloadFromDB = false;
        }
      }
    }

    private void loadPageValueForGroup(int groupID)
    {
      AclGroup groupById = this.session.AclGroupManager.GetGroupById(groupID);
      this.chkBoxViewSubordinateContacts.CheckedChanged -= new EventHandler(this.chkBoxViewSubordinateContacts_CheckedChanged);
      this.chkBoxViewSubordinateContacts.Checked = groupById.ViewSubordinatesContacts;
      this.cmbSuperiorAccess.SelectedIndexChanged -= new EventHandler(this.cmbSuperiorAccess_SelectedIndexChanged);
      if (this.chkBoxViewSubordinateContacts.Checked)
        this.cmbSuperiorAccess.Enabled = true;
      else
        this.cmbSuperiorAccess.Enabled = false;
      if (groupById.ContactAccess == AclResourceAccess.ReadOnly)
        this.cmbSuperiorAccess.SelectedIndex = 0;
      else
        this.cmbSuperiorAccess.SelectedIndex = 1;
      this.cmbSuperiorAccess.SelectedIndexChanged += new EventHandler(this.cmbSuperiorAccess_SelectedIndexChanged);
      this.chkBoxViewSubordinateContacts.CheckedChanged += new EventHandler(this.chkBoxViewSubordinateContacts_CheckedChanged);
      this.listViewGroupContact.Items.Clear();
      AclGroupContactMembers membersInGroupContact = this.session.AclGroupManager.GetMembersInGroupContact(groupID);
      ListViewItem[] items1 = new ListViewItem[membersInGroupContact.OrgMembers.Length];
      for (int index = 0; index < membersInGroupContact.OrgMembers.Length; ++index)
      {
        OrgInGroupContact orgMember = membersInGroupContact.OrgMembers[index];
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
      ListViewItem[] items2 = new ListViewItem[membersInGroupContact.UserMembers.Length];
      if (membersInGroupContact.UserMembers != null && membersInGroupContact.UserMembers.Length != 0)
      {
        List<string> stringList = new List<string>();
        foreach (UserInGroupContact userMember in membersInGroupContact.UserMembers)
        {
          if (!stringList.Contains(userMember.UserID))
            stringList.Add(userMember.UserID);
        }
        Hashtable users = this.session.OrganizationManager.GetUsers(stringList.ToArray());
        for (int index = 0; index < membersInGroupContact.UserMembers.Length; ++index)
        {
          UserInGroupContact userMember = membersInGroupContact.UserMembers[index];
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
      this.listViewGroupContact.Items.AddRange(items1);
      this.listViewGroupContact.Items.AddRange(items2);
    }

    private void loadPageValueForUser()
    {
      foreach (AclGroup group in this.groupList)
      {
        if (group.ViewSubordinatesContacts)
        {
          if (!this.chkBoxViewSubordinateContacts.Checked)
            this.chkBoxViewSubordinateContacts.Checked = true;
          if (group.ContactAccess == AclResourceAccess.ReadWrite)
            this.cmbSuperiorAccess.SelectedIndex = 1;
          else if (this.cmbSuperiorAccess.SelectedItem == null)
            this.cmbSuperiorAccess.SelectedIndex = 0;
        }
      }
      this.loadUserGroupContactMembers();
    }

    private string getAccessString(AclResourceAccess access)
    {
      return access == AclResourceAccess.ReadOnly ? "View Only" : "Edit";
    }

    private void MakeReadOnly()
    {
      this.chkBoxViewSubordinateContacts.Enabled = false;
      this.cmbSuperiorAccess.Enabled = false;
      this.stdIconBtnAdd.Enabled = false;
      this.personal = true;
    }

    public virtual void SetGroup(int groupId) => this.currentGroupId = groupId;

    private void chkBoxViewSubordinateContacts_CheckedChanged(object sender, EventArgs e)
    {
      this.requireUpdateSuperiorCheck = true;
      this.setDirtyFlag(true);
      if (this.chkBoxViewSubordinateContacts.Checked)
      {
        this.cmbSuperiorAccess.Enabled = true;
      }
      else
      {
        this.cmbSuperiorAccess.Enabled = false;
        this.cmbSuperiorAccess.SelectedIndex = 0;
      }
    }

    public void SaveData()
    {
      if (this.currentGroupId <= 0)
        return;
      if (!this.firstTime && this.orgView != null)
        this.orgView.Save();
      if (this.requireUpdate)
        this.UpdateAccessRight();
      if (this.requireUpdateSuperiorCheck)
      {
        AclGroup groupById = this.session.AclGroupManager.GetGroupById(this.currentGroupId);
        if (this.chkBoxViewSubordinateContacts.Checked)
        {
          groupById.ViewSubordinatesContacts = true;
          groupById.ContactAccess = this.cmbSuperiorAccess.SelectedIndex != 0 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
        }
        else
        {
          groupById.ViewSubordinatesContacts = false;
          groupById.ContactAccess = AclResourceAccess.ReadOnly;
        }
        this.session.AclGroupManager.UpdateGroup(groupById);
      }
      this.setDirtyFlag(false);
    }

    private void UpdateAccessRight()
    {
      if (!this.requireUpdate)
        return;
      List<OrgInGroupContact> orgInGroupContactList = new List<OrgInGroupContact>();
      List<UserInGroupContact> userInGroupContactList = new List<UserInGroupContact>();
      foreach (ListViewItem listViewItem in this.listViewGroupContact.Items)
      {
        if (((TreeNode) listViewItem.Tag).Tag is OrgInGroupContact)
        {
          OrgInGroupContact tag = (OrgInGroupContact) ((TreeNode) listViewItem.Tag).Tag;
          orgInGroupContactList.Add(tag);
        }
        else if ((object) (((TreeNode) listViewItem.Tag).Tag as UserInGroupContact) != null)
        {
          UserInGroupContact tag = (UserInGroupContact) ((TreeNode) listViewItem.Tag).Tag;
          userInGroupContactList.Add(tag);
        }
      }
      if (orgInGroupContactList.Count > 0 || userInGroupContactList.Count > 0)
        this.session.AclGroupManager.UpdateMembersInGroupContact(this.currentGroupId, userInGroupContactList.ToArray(), orgInGroupContactList.ToArray());
      this.requireUpdate = false;
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

    private void btnReset_Click(object sender, EventArgs e)
    {
      this.firstTime = true;
      this.currentGroupId = this.currentGroupId;
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (this.reloadFromDB)
      {
        this.orgView = new ResourceSetViewer(this.session, (object) this.session.AclGroupManager.GetMembersInGroupContact(this.currentGroupId), this.DirtyFlagChanged, this._currentGroupId);
        this.orgView.SetResetDataSourceTreeNode = this.GetResetTreeView();
      }
      this.reloadFromDB = false;
      this.firstTime = false;
      if (DialogResult.Cancel == this.orgView.ShowDialog((IWin32Window) this))
        return;
      this.loadGroupContactMembers((TreeView) this.orgView.GetCurrentTreeView());
      this.orgView.SetResetDataSourceTreeNode = this.GetResetTreeView();
    }

    private TreeNode[] GetResetTreeView()
    {
      ArrayList arrayList = new ArrayList();
      foreach (ListViewItem listViewItem in this.listViewGroupContact.Items)
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

    private void loadGroupContactMembers(TreeView tv)
    {
      ArrayList orgList = new ArrayList();
      ArrayList userList = new ArrayList();
      foreach (TreeNode node in tv.Nodes)
        this.addNodeToList(node, orgList, userList);
      this.listViewGroupContact.Items.Clear();
      this.listViewGroupContact.Items.AddRange((ListViewItem[]) orgList.ToArray(typeof (ListViewItem)));
      this.listViewGroupContact.Items.AddRange((ListViewItem[]) userList.ToArray(typeof (ListViewItem)));
    }

    private void listViewGroupContact_SubItemClicked(object sender, SubItemEventArgs e)
    {
      if (e.SubItem != 1 || this.personal)
        return;
      this.oldAccessIsViewOnly = e.Item.SubItems[1].Text == "View Only";
      this.listViewGroupContact.StartEditing((Control) this.cmbBoxAccessRight, e.Item, e.SubItem);
    }

    private void listViewGroupContact_SubItemEndEditing(object sender, SubItemEndEditingEventArgs e)
    {
      bool flag = this.cmbBoxAccessRight.Text == "View Only";
      if (flag == this.oldAccessIsViewOnly)
        return;
      this.setDirtyFlag(true);
      this.requireUpdate = true;
      if (((TreeNode) e.Item.Tag).Tag is OrgInGroupContact)
      {
        if (flag)
          ((OrgInGroupContact) ((TreeNode) e.Item.Tag).Tag).Access = AclResourceAccess.ReadOnly;
        else
          ((OrgInGroupContact) ((TreeNode) e.Item.Tag).Tag).Access = AclResourceAccess.ReadWrite;
      }
      else if (flag)
        ((UserInGroupContact) ((TreeNode) e.Item.Tag).Tag).Access = AclResourceAccess.ReadOnly;
      else
        ((UserInGroupContact) ((TreeNode) e.Item.Tag).Tag).Access = AclResourceAccess.ReadWrite;
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
        foreach (ListViewItem listViewItem2 in this.listViewGroupContact.Items)
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
              OrgInGroupContact orgInGroupContact1 = !(treeNode.Tag is OrgInfo) ? new OrgInGroupContact(((OrgInGroup) treeNode.Tag).OrgID, true, AclResourceAccess.ReadOnly, ((OrgInGroup) treeNode.Tag).OrgName) : new OrgInGroupContact(((OrgInfo) treeNode.Tag).Oid, true, AclResourceAccess.ReadOnly, ((OrgInfo) treeNode.Tag).OrgName);
              treeNode.Tag = (object) orgInGroupContact1;
              listViewItem1.Tag = (object) treeNode;
              break;
            case 1:
              listViewItem1 = new ListViewItem(new string[2]
              {
                treeNode.Text,
                "View Only"
              });
              listViewItem1.ImageIndex = treeNode.ImageIndex;
              OrgInGroupContact orgInGroupContact2 = !(treeNode.Tag is OrgInfo) ? new OrgInGroupContact(((OrgInGroup) treeNode.Tag).OrgID, false, AclResourceAccess.ReadOnly, ((OrgInGroup) treeNode.Tag).OrgName) : new OrgInGroupContact(((OrgInfo) treeNode.Tag).Oid, false, AclResourceAccess.ReadOnly, ((OrgInfo) treeNode.Tag).OrgName);
              treeNode.Tag = (object) orgInGroupContact2;
              listViewItem1.Tag = (object) treeNode;
              break;
            case 2:
              listViewItem1 = new ListViewItem(new string[2]
              {
                treeNode.Text,
                "View Only"
              });
              listViewItem1.ImageIndex = treeNode.ImageIndex;
              UserInGroupContact userInGroupContact = (object) (treeNode.Tag as UserInGroupContact) == null ? new UserInGroupContact(((UserInfo) treeNode.Tag).Userid, AclResourceAccess.ReadOnly) : new UserInGroupContact(((UserInGroupContact) treeNode.Tag).UserID, AclResourceAccess.ReadOnly);
              treeNode.Tag = (object) userInGroupContact;
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

    private void cmbSuperiorAccess_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
      this.requireUpdateSuperiorCheck = true;
    }

    private void stdIconBtnDelete_Click(object sender, EventArgs e)
    {
      if (this.listViewGroupContact.SelectedItems == null || this.listViewGroupContact.SelectedItems.Count == 0)
        return;
      this.Cursor = Cursors.WaitCursor;
      if (this.reloadFromDB)
        this.orgView = new ResourceSetViewer(this.session, (object) this.session.AclGroupManager.GetMembersInGroupContact(this.currentGroupId), this.DirtyFlagChanged, this._currentGroupId);
      List<TreeNode> treeNodeList = new List<TreeNode>();
      foreach (ListViewItem selectedItem in this.listViewGroupContact.SelectedItems)
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

    private void listViewGroupContact_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.personal)
        this.stdIconBtnDelete.Enabled = false;
      else
        this.stdIconBtnDelete.Enabled = this.listViewGroupContact.SelectedItems.Count > 0;
    }
  }
}

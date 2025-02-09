// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ePass.ePassCredentialUsers
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ePass
{
  public class ePassCredentialUsers : Form
  {
    private string providerName;
    private int currentCredentialID;
    private IContainer components;
    private GroupContainer groupContainer1;
    private Button btnSave;
    private GridView gvSelectedUsers;
    private GridView gvAvailableUsers;
    private ComboBox cbSearchType;
    private GridView gvGrouping;
    private Button btnRemove;
    private Button btnAdd;
    private Panel pnlOrg;
    private HierarchyTree hierarchyTreeOrg;
    private Button btnCancel;
    private ImageList imgListTv;
    private GradientPanel gradientPanel1;
    private Label label1;
    private GroupContainer groupContainer2;
    private GroupContainer gcSelectedUsers;
    private GroupContainer gcAvailableUsers;
    private ContextMenu orgMenu;
    private MenuItem orgMenuItem;

    public ePassCredentialUsers(
      int currentCredentialID,
      string providerName,
      List<string> userList)
    {
      this.InitializeComponent();
      this.providerName = providerName;
      this.currentCredentialID = currentCredentialID;
      this.initialPageValue(userList);
    }

    private void initialPageValue(List<string> userlist)
    {
      foreach (string itemText in userlist.ToArray())
        this.gvSelectedUsers.Items.Add(itemText);
      this.gcSelectedUsers.Text = "Selected Users (" + (object) this.gvSelectedUsers.Items.Count + ")";
      this.hierarchyTreeOrg.SetSession(Session.DefaultInstance);
      this.hierarchyTreeOrg.RootNodes(0);
      this.cbSearchType.SelectedIndex = 0;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      List<string> duplicateUsers = Session.ConfigurationManager.GetDuplicateUsers(this.currentCredentialID, this.providerName, this.SelectedUserIDs.ToArray());
      if (duplicateUsers.Count > 0)
      {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string str in duplicateUsers.ToArray())
          stringBuilder.AppendLine(str);
        this.DialogResult = Utils.Dialog((IWin32Window) this, "The following users are setup with a different account login for this provider and will be reassigned to this password setting.  Do you want to continue?" + Environment.NewLine + stringBuilder.ToString(), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    public List<string> SelectedUserIDs
    {
      get
      {
        List<string> selectedUserIds = new List<string>();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvSelectedUsers.Items)
          selectedUserIds.Add(gvItem.Text);
        return selectedUserIds;
      }
    }

    private void cbSearchType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.gvAvailableUsers.Items.Clear();
      this.gvGrouping.Items.Clear();
      this.pnlOrg.Dock = DockStyle.None;
      this.gvGrouping.Dock = DockStyle.None;
      if (this.cbSearchType.Text == "Persona")
      {
        foreach (Persona allPersona in Session.PersonaManager.GetAllPersonas())
          this.gvGrouping.Items.Add(new GVItem(allPersona.Name)
          {
            Tag = (object) allPersona
          });
        this.gvGrouping.Dock = DockStyle.Fill;
        if (this.gvGrouping.Items.Count > 0)
          this.gvGrouping.Items[0].Selected = true;
        this.gvGrouping.BringToFront();
      }
      else if (this.cbSearchType.Text == "User Group")
      {
        foreach (AclGroup allGroup in Session.AclGroupManager.GetAllGroups())
          this.gvGrouping.Items.Add(new GVItem(allGroup.Name)
          {
            Tag = (object) allGroup
          });
        this.gvGrouping.Dock = DockStyle.Fill;
        if (this.gvGrouping.Items.Count > 0)
          this.gvGrouping.Items[0].Selected = true;
        this.gvGrouping.BringToFront();
      }
      else
      {
        this.pnlOrg.Dock = DockStyle.Fill;
        this.pnlOrg.BringToFront();
        this.refreshAvailableUsersFromOrg(false);
      }
    }

    private void gvGrouping_SelectedIndexChanged(object sender, EventArgs e)
    {
      List<string> newUserList = new List<string>();
      if (this.gvGrouping.SelectedItems.Count > 0)
      {
        if (this.cbSearchType.Text == "Persona")
        {
          foreach (UserInfo userInfo in Session.OrganizationManager.GetUsersWithPersona(((Persona) this.gvGrouping.SelectedItems[0].Tag).ID, false))
            newUserList.Add(userInfo.Userid);
        }
        else if (this.cbSearchType.Text == "User Group")
        {
          foreach (string str in Session.AclGroupManager.GetUsersInGroup(((AclGroup) this.gvGrouping.SelectedItems[0].Tag).ID, true))
            newUserList.Add(str);
        }
      }
      this.populateAvailableUserList(newUserList);
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      foreach (GVItem selectedItem in this.gvAvailableUsers.SelectedItems)
      {
        bool flag = false;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvSelectedUsers.Items)
        {
          if (gvItem.Text == selectedItem.Text)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          this.gvSelectedUsers.Items.Add(selectedItem.Text);
      }
      this.gcSelectedUsers.Text = "Selected Users (" + (object) this.gvSelectedUsers.Items.Count + ")";
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      foreach (GVItem selectedItem in this.gvSelectedUsers.SelectedItems)
        this.gvSelectedUsers.Items.Remove(selectedItem);
      this.gcSelectedUsers.Text = "Selected Users (" + (object) this.gvSelectedUsers.Items.Count + ")";
    }

    private void hierarchyTreeOrg_AfterSelect(object sender, TreeViewEventArgs e)
    {
      if (this.cbSearchType.Text != "Organization")
        return;
      this.refreshAvailableUsersFromOrg(false);
    }

    private void hierarchyTreeOrg_MouseDown(object sender, MouseEventArgs e)
    {
    }

    private void refreshAvailableUsersFromOrg(bool includeSubOrgs)
    {
      TreeNode selectedNode = this.hierarchyTreeOrg.SelectedNode;
      if (selectedNode == null)
        return;
      OrgNodeTag tag = (OrgNodeTag) selectedNode.Tag;
      UserInfo[] userInfoArray;
      try
      {
        userInfoArray = includeSubOrgs ? Session.OrganizationManager.GetUsersUnderOrganization(tag.Oid) : Session.OrganizationManager.GetUsersInOrganization(tag.Oid);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The selected organization has been deleted and is no longer available.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return;
      }
      if (userInfoArray == null)
        return;
      List<string> newUserList = new List<string>();
      for (int index = 0; index < userInfoArray.Length; ++index)
        newUserList.Add(userInfoArray[index].Userid);
      this.populateAvailableUserList(newUserList);
    }

    private void populateAvailableUserList(List<string> newUserList)
    {
      this.gvAvailableUsers.Items.Clear();
      foreach (string text in newUserList.ToArray())
        this.gvAvailableUsers.Items.Add(new GVItem(text));
    }

    private void orgMenuItem_Click(object sender, EventArgs e)
    {
      TreeNode selectedNode = this.hierarchyTreeOrg.SelectedNode;
      selectedNode.ImageIndex = 2;
      selectedNode.SelectedImageIndex = 2;
      selectedNode.StateImageIndex = 2;
      this.refreshAvailableUsersFromOrg(true);
    }

    private void hierarchyTreeOrg_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      this.hierarchyTreeOrg.SelectedNode = this.hierarchyTreeOrg.GetNodeAt(e.X, e.Y);
    }

    private void hierarchyTreeOrg_BeforeSelect(object sender, TreeViewCancelEventArgs e)
    {
      if (this.hierarchyTreeOrg.SelectedNode == null)
        return;
      this.hierarchyTreeOrg.SelectedNode.ImageIndex = 0;
      this.hierarchyTreeOrg.SelectedNode.SelectedImageIndex = 1;
      this.hierarchyTreeOrg.SelectedNode.StateImageIndex = 0;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ePassCredentialUsers));
      GVColumn gvColumn3 = new GVColumn();
      this.groupContainer1 = new GroupContainer();
      this.gcSelectedUsers = new GroupContainer();
      this.gvSelectedUsers = new GridView();
      this.btnRemove = new Button();
      this.gcAvailableUsers = new GroupContainer();
      this.gvAvailableUsers = new GridView();
      this.btnAdd = new Button();
      this.groupContainer2 = new GroupContainer();
      this.pnlOrg = new Panel();
      this.hierarchyTreeOrg = new HierarchyTree();
      this.orgMenu = new ContextMenu();
      this.orgMenuItem = new MenuItem();
      this.imgListTv = new ImageList(this.components);
      this.gvGrouping = new GridView();
      this.cbSearchType = new ComboBox();
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.groupContainer1.SuspendLayout();
      this.gcSelectedUsers.SuspendLayout();
      this.gcAvailableUsers.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.pnlOrg.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.gcSelectedUsers);
      this.groupContainer1.Controls.Add((Control) this.btnRemove);
      this.groupContainer1.Controls.Add((Control) this.gcAvailableUsers);
      this.groupContainer1.Controls.Add((Control) this.btnAdd);
      this.groupContainer1.Controls.Add((Control) this.groupContainer2);
      this.groupContainer1.Controls.Add((Control) this.gradientPanel1);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(705, 441);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Select Users";
      this.gcSelectedUsers.Controls.Add((Control) this.gvSelectedUsers);
      this.gcSelectedUsers.HeaderForeColor = SystemColors.ControlText;
      this.gcSelectedUsers.Location = new Point(504, 69);
      this.gcSelectedUsers.Name = "gcSelectedUsers";
      this.gcSelectedUsers.Size = new Size(190, 354);
      this.gcSelectedUsers.TabIndex = 14;
      this.gcSelectedUsers.Text = "Selected Users";
      this.gvSelectedUsers.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "UserID";
      gvColumn1.Width = 150;
      this.gvSelectedUsers.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.gvSelectedUsers.Dock = DockStyle.Fill;
      this.gvSelectedUsers.Location = new Point(1, 26);
      this.gvSelectedUsers.Name = "gvSelectedUsers";
      this.gvSelectedUsers.Size = new Size(188, 327);
      this.gvSelectedUsers.TabIndex = 3;
      this.btnRemove.Location = new Point(467, 218);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(29, 25);
      this.btnRemove.TabIndex = 6;
      this.btnRemove.Text = "<<";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.gcAvailableUsers.Controls.Add((Control) this.gvAvailableUsers);
      this.gcAvailableUsers.HeaderForeColor = SystemColors.ControlText;
      this.gcAvailableUsers.Location = new Point(270, 69);
      this.gcAvailableUsers.Name = "gcAvailableUsers";
      this.gcAvailableUsers.Size = new Size(190, 354);
      this.gcAvailableUsers.TabIndex = 13;
      this.gcAvailableUsers.Text = "Enabled Users";
      this.gvAvailableUsers.BorderStyle = BorderStyle.None;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.Text = "UserID";
      gvColumn2.Width = 150;
      this.gvAvailableUsers.Columns.AddRange(new GVColumn[1]
      {
        gvColumn2
      });
      this.gvAvailableUsers.Dock = DockStyle.Fill;
      this.gvAvailableUsers.Location = new Point(1, 26);
      this.gvAvailableUsers.Name = "gvAvailableUsers";
      this.gvAvailableUsers.Size = new Size(188, 327);
      this.gvAvailableUsers.TabIndex = 1;
      this.btnAdd.Location = new Point(467, 187);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(29, 25);
      this.btnAdd.TabIndex = 5;
      this.btnAdd.Text = ">>";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.groupContainer2.Controls.Add((Control) this.pnlOrg);
      this.groupContainer2.Controls.Add((Control) this.gvGrouping);
      this.groupContainer2.Controls.Add((Control) this.cbSearchType);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(4, 69);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(260, 354);
      this.groupContainer2.TabIndex = 12;
      this.groupContainer2.Text = "Search by";
      this.pnlOrg.Controls.Add((Control) this.hierarchyTreeOrg);
      this.pnlOrg.Location = new Point(11, 32);
      this.pnlOrg.Name = "pnlOrg";
      this.pnlOrg.Size = new Size(231, 322);
      this.pnlOrg.TabIndex = 9;
      this.hierarchyTreeOrg.BorderStyle = BorderStyle.None;
      this.hierarchyTreeOrg.ContextMenu = this.orgMenu;
      this.hierarchyTreeOrg.Dock = DockStyle.Fill;
      this.hierarchyTreeOrg.ImageIndex = 0;
      this.hierarchyTreeOrg.ImageList = this.imgListTv;
      this.hierarchyTreeOrg.Location = new Point(0, 0);
      this.hierarchyTreeOrg.Name = "hierarchyTreeOrg";
      this.hierarchyTreeOrg.SelectedImageIndex = 0;
      this.hierarchyTreeOrg.Size = new Size(231, 322);
      this.hierarchyTreeOrg.TabIndex = 0;
      this.hierarchyTreeOrg.MouseClick += new MouseEventHandler(this.hierarchyTreeOrg_MouseClick);
      this.hierarchyTreeOrg.AfterSelect += new TreeViewEventHandler(this.hierarchyTreeOrg_AfterSelect);
      this.hierarchyTreeOrg.MouseDown += new MouseEventHandler(this.hierarchyTreeOrg_MouseDown);
      this.hierarchyTreeOrg.BeforeSelect += new TreeViewCancelEventHandler(this.hierarchyTreeOrg_BeforeSelect);
      this.orgMenu.MenuItems.AddRange(new MenuItem[1]
      {
        this.orgMenuItem
      });
      this.orgMenuItem.Index = 0;
      this.orgMenuItem.Text = "View Users in this Organization and Below";
      this.orgMenuItem.Click += new EventHandler(this.orgMenuItem_Click);
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.Transparent;
      this.imgListTv.Images.SetKeyName(0, "folder.bmp");
      this.imgListTv.Images.SetKeyName(1, "members-this-group.png");
      this.imgListTv.Images.SetKeyName(2, "members-this-group-and-below.png");
      this.gvGrouping.AllowMultiselect = false;
      this.gvGrouping.BorderStyle = BorderStyle.None;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.Text = "Name";
      gvColumn3.Width = 220;
      this.gvGrouping.Columns.AddRange(new GVColumn[1]
      {
        gvColumn3
      });
      this.gvGrouping.Location = new Point(11, 32);
      this.gvGrouping.Name = "gvGrouping";
      this.gvGrouping.Size = new Size(231, 299);
      this.gvGrouping.TabIndex = 8;
      this.gvGrouping.SelectedIndexChanged += new EventHandler(this.gvGrouping_SelectedIndexChanged);
      this.cbSearchType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbSearchType.FormattingEnabled = true;
      this.cbSearchType.Items.AddRange(new object[3]
      {
        (object) "Persona",
        (object) "User Group",
        (object) "Organization"
      });
      this.cbSearchType.Location = new Point(70, 2);
      this.cbSearchType.Name = "cbSearchType";
      this.cbSearchType.Size = new Size(186, 22);
      this.cbSearchType.TabIndex = 0;
      this.cbSearchType.SelectedIndexChanged += new EventHandler(this.cbSearchType_SelectedIndexChanged);
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 26);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(703, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 11;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(8, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(598, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Search by Persona, User Group or Organization.  Select Enabled Users and move to Selected Users, and then click Select.";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(618, 447);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 10;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSave.DialogResult = DialogResult.OK;
      this.btnSave.Location = new Point(537, 447);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 25);
      this.btnSave.TabIndex = 8;
      this.btnSave.Text = "Select";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(705, 482);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (ePassCredentialUsers);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Users";
      this.groupContainer1.ResumeLayout(false);
      this.gcSelectedUsers.ResumeLayout(false);
      this.gcAvailableUsers.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      this.pnlOrg.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

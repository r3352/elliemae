// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SecurityGroupSetupForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SecurityGroupSetupForm : SettingsUserControl
  {
    private IContainer components;
    private Sessions.Session session;
    private int currentGroupId = -1;
    private ListView lvGroup;
    private ColumnHeader columnHeader1;
    private ContextMenu contextMenuGroup;
    private ToolTip toolTipGroup;
    private GroupContainer gcGroupList;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnDuplicate;
    private StandardIconButton stdIconBtnDelete;
    private Splitter splitter1;
    private PanelEx pnlExPersonaSettings;
    private MenuItem menuItemNew;
    private MenuItem menuItemRename;
    private MenuItem menuItemDuplicate;
    private MenuItem menuItemDelete;
    private Timer lvGroupSelectedIndexChangedTimer;
    private SecurityGroupSettingsMainForm groupSettingsForm;
    private string groupName = "";
    private bool lvGroupSelectedIndexChangedTimerIsRunning;
    private int lvGroupCurrSelectedIdx = -1;

    public SecurityGroupSetupForm(SetUpContainer setupContainer)
      : this(Session.DefaultInstance, setupContainer, false)
    {
    }

    public SecurityGroupSetupForm(
      Sessions.Session session,
      SetUpContainer setupContainer,
      bool allowMultiSelect)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      this.groupSettingsForm = new SecurityGroupSettingsMainForm(this.session, (IWin32Window) this.setupContainer, this.currentGroupId);
      this.groupSettingsForm.TopLevel = false;
      this.groupSettingsForm.Dock = DockStyle.Fill;
      this.groupSettingsForm.Visible = true;
      this.groupSettingsForm.FormBorderStyle = FormBorderStyle.None;
      this.groupSettingsForm.BackColor = this.BackColor;
      this.pnlExPersonaSettings.Controls.Add((Control) this.groupSettingsForm);
      this.loadlvGroup();
      this.lvGroup.ContextMenu = this.contextMenuGroup;
      this.lvGroup.MultiSelect = allowMultiSelect;
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
      this.lvGroup = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.contextMenuGroup = new ContextMenu();
      this.menuItemNew = new MenuItem();
      this.menuItemRename = new MenuItem();
      this.menuItemDuplicate = new MenuItem();
      this.menuItemDelete = new MenuItem();
      this.toolTipGroup = new ToolTip(this.components);
      this.stdIconBtnDelete = new StandardIconButton();
      this.stdIconBtnDuplicate = new StandardIconButton();
      this.stdIconBtnNew = new StandardIconButton();
      this.gcGroupList = new GroupContainer();
      this.splitter1 = new Splitter();
      this.pnlExPersonaSettings = new PanelEx();
      this.lvGroupSelectedIndexChangedTimer = new Timer(this.components);
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDuplicate).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      this.gcGroupList.SuspendLayout();
      this.SuspendLayout();
      this.lvGroup.AutoArrange = false;
      this.lvGroup.BorderStyle = BorderStyle.None;
      this.lvGroup.Columns.AddRange(new ColumnHeader[1]
      {
        this.columnHeader1
      });
      this.lvGroup.Dock = DockStyle.Fill;
      this.lvGroup.FullRowSelect = true;
      this.lvGroup.HeaderStyle = ColumnHeaderStyle.None;
      this.lvGroup.HideSelection = false;
      this.lvGroup.LabelEdit = true;
      this.lvGroup.Location = new Point(1, 26);
      this.lvGroup.MultiSelect = false;
      this.lvGroup.Name = "lvGroup";
      this.lvGroup.Size = new Size(223, 421);
      this.lvGroup.TabIndex = 12;
      this.lvGroup.UseCompatibleStateImageBehavior = false;
      this.lvGroup.View = View.Details;
      this.lvGroup.AfterLabelEdit += new LabelEditEventHandler(this.lvGroup_AfterLabelEdit);
      this.lvGroup.SelectedIndexChanged += new EventHandler(this.lvGroup_SelectedIndexChanged);
      this.lvGroup.BeforeLabelEdit += new LabelEditEventHandler(this.lvGroup_BeforeLabelEdit);
      this.columnHeader1.Text = "";
      this.columnHeader1.Width = 350;
      this.contextMenuGroup.MenuItems.AddRange(new MenuItem[4]
      {
        this.menuItemNew,
        this.menuItemRename,
        this.menuItemDuplicate,
        this.menuItemDelete
      });
      this.menuItemNew.Index = 0;
      this.menuItemNew.Text = "New";
      this.menuItemNew.Click += new EventHandler(this.lblAddGroup_Click);
      this.menuItemRename.Index = 1;
      this.menuItemRename.Text = "Rename";
      this.menuItemRename.Click += new EventHandler(this.menuItemRename_Click);
      this.menuItemDuplicate.Index = 2;
      this.menuItemDuplicate.Text = "Duplicate";
      this.menuItemDuplicate.Click += new EventHandler(this.picBoxCopy_Click);
      this.menuItemDelete.Index = 3;
      this.menuItemDelete.Text = "Delete";
      this.menuItemDelete.Click += new EventHandler(this.lblDeleteGroup_Click);
      this.toolTipGroup.AutoPopDelay = 5000;
      this.toolTipGroup.InitialDelay = 100;
      this.toolTipGroup.ReshowDelay = 100;
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(202, 5);
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 13;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTipGroup.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.lblDeleteGroup_Click);
      this.stdIconBtnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDuplicate.BackColor = Color.Transparent;
      this.stdIconBtnDuplicate.Location = new Point(180, 5);
      this.stdIconBtnDuplicate.Name = "stdIconBtnDuplicate";
      this.stdIconBtnDuplicate.Size = new Size(16, 16);
      this.stdIconBtnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.stdIconBtnDuplicate.TabIndex = 16;
      this.stdIconBtnDuplicate.TabStop = false;
      this.toolTipGroup.SetToolTip((Control) this.stdIconBtnDuplicate, " Duplicate");
      this.stdIconBtnDuplicate.Click += new EventHandler(this.picBoxCopy_Click);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(160, 5);
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 17;
      this.stdIconBtnNew.TabStop = false;
      this.toolTipGroup.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.lblAddGroup_Click);
      this.gcGroupList.Controls.Add((Control) this.stdIconBtnNew);
      this.gcGroupList.Controls.Add((Control) this.stdIconBtnDuplicate);
      this.gcGroupList.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcGroupList.Controls.Add((Control) this.lvGroup);
      this.gcGroupList.Dock = DockStyle.Left;
      this.gcGroupList.Location = new Point(0, 0);
      this.gcGroupList.MinimumSize = new Size(200, 0);
      this.gcGroupList.Name = "gcGroupList";
      this.gcGroupList.Size = new Size(225, 448);
      this.gcGroupList.TabIndex = 13;
      this.gcGroupList.Text = "1. Create a group.";
      this.splitter1.Location = new Point(225, 0);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(3, 448);
      this.splitter1.TabIndex = 14;
      this.splitter1.TabStop = false;
      this.pnlExPersonaSettings.Dock = DockStyle.Fill;
      this.pnlExPersonaSettings.Location = new Point(228, 0);
      this.pnlExPersonaSettings.Name = "pnlExPersonaSettings";
      this.pnlExPersonaSettings.Size = new Size(427, 448);
      this.pnlExPersonaSettings.TabIndex = 15;
      this.lvGroupSelectedIndexChangedTimer.Tick += new EventHandler(this.lvGroupSelectedIndexChangedTimer_Tick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlExPersonaSettings);
      this.Controls.Add((Control) this.splitter1);
      this.Controls.Add((Control) this.gcGroupList);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (SecurityGroupSetupForm);
      this.Size = new Size(655, 448);
      this.BackColorChanged += new EventHandler(this.SecurityGroupSetupForm_BackColorChanged);
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      ((ISupportInitialize) this.stdIconBtnDuplicate).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      this.gcGroupList.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public string[] SelectedGroups
    {
      get
      {
        if (this.lvGroup.SelectedItems == null)
          return (string[]) null;
        List<string> stringList = new List<string>();
        foreach (ListViewItem selectedItem in this.lvGroup.SelectedItems)
          stringList.Add(selectedItem.Text);
        return stringList.ToArray();
      }
      set
      {
        if (value == null || value.Length == 0)
          return;
        List<string> stringList = new List<string>((IEnumerable<string>) value);
        foreach (ListViewItem listViewItem in this.lvGroup.Items)
          listViewItem.Selected = stringList.Contains(listViewItem.Text);
      }
    }

    public override bool IsDirty => this.groupSettingsForm.IsDirty();

    public override void Save() => this.groupSettingsForm.Save();

    public override void Reset() => this.groupSettingsForm.Reset();

    public int CurrentGroupId
    {
      get => this.currentGroupId;
      set
      {
        this.currentGroupId = value;
        if (this.groupSettingsForm == null)
          return;
        this.groupSettingsForm.CurrentGroupName = this.groupName;
        this.groupSettingsForm.CurrentGroupId = value;
      }
    }

    private void loadlvGroup()
    {
      this.lvGroup.Items.Clear();
      this.lvGroup.BeginUpdate();
      AclGroup[] groups = this.getGroups();
      Array.Sort<AclGroup>(groups);
      for (int index = 0; index < groups.Length; ++index)
      {
        ListViewItem listViewItem = new ListViewItem(groups[index].Name);
        listViewItem.Tag = (object) groups[index];
        if (index == 0)
          listViewItem.Selected = true;
        this.lvGroup.Items.Add(listViewItem);
      }
      this.lvGroup.EndUpdate();
    }

    private AclGroup[] getGroups() => this.session.AclGroupManager.GetAllGroups();

    private void lblAddGroup_Click(object sender, EventArgs e)
    {
      this.groupSettingsForm.CheckUnsavedData();
      SecurityGroupNameDlg securityGroupNameDlg = new SecurityGroupNameDlg(this.session);
      if (securityGroupNameDlg.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        return;
      AclGroup group = this.session.AclGroupManager.CreateGroup(new AclGroup()
      {
        Name = securityGroupNameDlg.SecurityGroupName,
        ViewSubordinatesContacts = true
      });
      foreach (LoanFolderInfo allLoanFolderInfo in this.session.LoanManager.GetAllLoanFolderInfos(false, false))
      {
        if (allLoanFolderInfo.Type == LoanFolderInfo.LoanFolderType.Regular)
          this.session.AclGroupManager.UpdateAclGroupLoanFolder(new LoanFolderInGroup(group.ID, allLoanFolderInfo.Name, true));
      }
      foreach (RoleInfo allRoleFunction in ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions())
        this.session.AclGroupManager.UpdateAclGroupRoleAccessLevel(new AclGroupRoleAccessLevel(group.ID, allRoleFunction.RoleID, AclGroupRoleAccessEnum.All, true));
      this.lvGroup.BeginUpdate();
      ListViewItem listViewItem = new ListViewItem(group.Name);
      listViewItem.Selected = true;
      listViewItem.Tag = (object) group;
      if (this.lvGroup.SelectedItems.Count > 0)
        this.lvGroup.SelectedItems[0].Selected = false;
      this.lvGroup.Items.Add(listViewItem);
      this.lvGroup.EndUpdate();
    }

    private void menuItemRename_Click(object sender, EventArgs e)
    {
      this.lvGroup.SelectedItems[0].BeginEdit();
    }

    private void lblDeleteGroup_Click(object sender, EventArgs e)
    {
      if (this.lvGroup.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select the group you want to delete.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.lvGroup.SelectedItems[0].Text == "All Users")
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "This group can not be deleted.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (DialogResult.No == Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the group?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
          return;
        this.session.AclGroupManager.DeleteGroup(((AclGroup) this.lvGroup.SelectedItems[0].Tag).ID);
        this.lvGroup.BeginUpdate();
        this.lvGroup.SelectedItems[0].Remove();
        if (this.lvGroup.Items.Count > 0)
        {
          this.groupSettingsForm.SkipChecking = true;
          this.lvGroup.Items[0].Selected = true;
          this.lvGroup_SelectedIndexChanged((object) null, (EventArgs) null);
          this.groupSettingsForm.SkipChecking = false;
        }
        this.lvGroup.EndUpdate();
      }
    }

    private void SecurityGroupSetupForm_BackColorChanged(object sender, EventArgs e)
    {
      this.groupSettingsForm.BackColor = this.BackColor;
    }

    private void picBoxCopy_Click(object sender, EventArgs e)
    {
      if (this.lvGroup.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a group to copy from.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (DialogResult.No == Utils.Dialog((IWin32Window) this, "Duplicating an existing user group with existing data is resource intensive and is only recommended during off-peak non-business hours. Are you sure you want to continue now?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
          return;
        this.groupSettingsForm.CheckUnsavedData();
        string groupName = "Copy of " + this.lvGroup.SelectedItems[0].Text;
        int num2 = 1;
        object[] objArray;
        for (; this.session.AclGroupManager.GroupNameExists(groupName); groupName = string.Concat(objArray))
        {
          ++num2;
          objArray = new object[4]
          {
            (object) "Copy of (",
            (object) num2,
            (object) ") ",
            (object) this.lvGroup.SelectedItems[0].Text
          };
        }
        if (groupName.Length > 50)
        {
          SecurityGroupNameDlg securityGroupNameDlg = new SecurityGroupNameDlg(this.session);
          if (securityGroupNameDlg.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
            return;
          groupName = securityGroupNameDlg.SecurityGroupName;
        }
        AclGroup groupById = this.session.AclGroupManager.GetGroupById(((AclGroup) this.lvGroup.SelectedItems[0].Tag).ID);
        int id = groupById.ID;
        AclGroup group = this.session.AclGroupManager.CreateGroup(new AclGroup()
        {
          Name = groupName,
          ViewSubordinatesContacts = groupById.ViewSubordinatesContacts,
          ContactAccess = groupById.ContactAccess
        });
        this.session.AclGroupManager.CloneAclGroup(id, group.ID);
        ListViewItem listViewItem = new ListViewItem(group.Name);
        listViewItem.Tag = (object) group;
        listViewItem.Selected = true;
        this.lvGroup.BeginUpdate();
        if (this.lvGroup.SelectedItems.Count > 0)
          this.lvGroup.SelectedItems[0].Selected = false;
        this.lvGroup.Items.Add(listViewItem);
        this.lvGroup_SelectedIndexChanged((object) null, (EventArgs) null);
        this.lvGroup.EndUpdate();
      }
    }

    private void lvGroupSelectedIndexChangedTimer_Tick(object sender, EventArgs e)
    {
      this.lvGroupSelectedIndexChangedTimer.Stop();
      this.lvGroupSelectedIndexChangedTimerIsRunning = false;
      this.onLvGroupSelectedIndexChanged();
    }

    private void lvGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.lvGroupSelectedIndexChangedTimerIsRunning && this.lvGroup.SelectedItems.Count == 0)
      {
        this.lvGroupSelectedIndexChangedTimerIsRunning = true;
        this.lvGroupSelectedIndexChangedTimer.Start();
      }
      else
      {
        this.groupSettingsForm.Enabled = this.lvGroup.SelectedItems.Count <= 1;
        this.onLvGroupSelectedIndexChanged();
      }
    }

    private void onLvGroupSelectedIndexChanged()
    {
      if (this.lvGroup.SelectedItems.Count == 0)
      {
        if (this.lvGroup.Items.Count == 0)
          return;
        while (this.lvGroupCurrSelectedIdx >= this.lvGroup.Items.Count)
          --this.lvGroupCurrSelectedIdx;
        if (this.lvGroupCurrSelectedIdx < 0 || this.lvGroup.Items[this.lvGroupCurrSelectedIdx].Selected)
          return;
        this.lvGroup.Items[this.lvGroupCurrSelectedIdx].Selected = true;
      }
      else
      {
        if (this.lvGroupCurrSelectedIdx == this.lvGroup.SelectedIndices[0])
          return;
        this.lvGroupCurrSelectedIdx = this.lvGroup.SelectedIndices[0];
        this.menuItemRename.Enabled = this.stdIconBtnDelete.Enabled = this.stdIconBtnDuplicate.Enabled = this.lvGroup.SelectedItems.Count > 0;
        this.menuItemDelete.Enabled = this.stdIconBtnDelete.Enabled;
        this.menuItemDuplicate.Enabled = this.stdIconBtnDuplicate.Enabled;
        if (this.lvGroup.SelectedItems.Count == 0)
          return;
        this.menuItemRename.Enabled = this.lvGroup.SelectedItems[0].Text != "All Users";
        AclGroup tag = (AclGroup) this.lvGroup.SelectedItems[0].Tag;
        this.groupName = tag.Name;
        this.CurrentGroupId = tag.ID;
        this.groupSettingsForm.Title = "2. Define the members and access for the " + tag.Name + " groups.";
      }
    }

    private void picBoxUp_Click(object sender, EventArgs e)
    {
      if (this.lvGroup.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a group", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (this.lvGroup.SelectedIndices[0] <= 0)
          return;
        this.groupSettingsForm.CheckUnsavedData();
        ListViewItem selectedItem = this.lvGroup.SelectedItems[0];
        int index = selectedItem.Index - 1;
        this.lvGroup.BeginUpdate();
        this.lvGroup.Items.Remove(selectedItem);
        this.lvGroup.Items.Insert(index, selectedItem);
        this.UpdateGroupOrder();
        this.lvGroup.EndUpdate();
      }
    }

    private void picBoxDown_Click(object sender, EventArgs e)
    {
      if (this.lvGroup.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a group", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (this.lvGroup.SelectedIndices[0] >= this.lvGroup.Items.Count - 1)
          return;
        this.groupSettingsForm.CheckUnsavedData();
        ListViewItem selectedItem = this.lvGroup.SelectedItems[0];
        int index = selectedItem.Index + 1;
        this.lvGroup.BeginUpdate();
        this.lvGroup.Items.Remove(selectedItem);
        this.lvGroup.Items.Insert(index, selectedItem);
        this.UpdateGroupOrder();
        this.lvGroup.EndUpdate();
      }
    }

    private void UpdateGroupOrder()
    {
      foreach (ListViewItem listViewItem in this.lvGroup.Items)
      {
        AclGroup groupById = this.session.AclGroupManager.GetGroupById(((AclGroup) listViewItem.Tag).ID);
        groupById.DisplayOrder = listViewItem.Index;
        Session.AclGroupManager.UpdateGroup(groupById);
      }
    }

    private void lvGroup_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
      bool flag = false;
      if (e.Label == null)
        e.CancelEdit = true;
      else if (e.Label.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please provide a group name.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        e.CancelEdit = true;
      }
      else if (e.Label.Length > 50)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Group name can not be longer than 50 characters.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        e.CancelEdit = true;
      }
      else
      {
        foreach (ListViewItem listViewItem in this.lvGroup.Items)
        {
          if (listViewItem.Index != e.Item && string.Compare(e.Label.Trim(), listViewItem.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0)
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Group name already exist.  Please select another one.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          e.CancelEdit = true;
        }
        else
        {
          AclGroup tag = (AclGroup) this.lvGroup.Items[e.Item].Tag;
          this.session.AclGroupManager.RenameGroup(tag, e.Label.Trim());
          tag.Name = e.Label.Trim();
          this.lvGroup.Items[e.Item].Tag = (object) tag;
          this.groupSettingsForm.Title = "2. Define the members and access for the " + tag.Name + " groups.";
        }
      }
    }

    private void lvGroup_BeforeLabelEdit(object sender, LabelEditEventArgs e)
    {
      if (!(this.lvGroup.Items[e.Item].Text == "All Users"))
        return;
      e.CancelEdit = true;
    }
  }
}

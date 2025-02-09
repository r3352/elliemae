// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MilestoneSelectionDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
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
  public class MilestoneSelectionDialog : Form
  {
    private IContainer components;
    private int personaId = -1;
    private string userId;
    private Persona[] personas;
    private AclMilestone feature;
    private Button btnOK;
    private Hashtable entryIdToPerm;
    private Label lblMilestones;
    private Label lblRoles;
    private bool isLoanAssociate;
    private int allChecked = 3;
    private bool dirty;
    private ImageList imgList;
    private Label lblDisconnected;
    private Label lblLinked;
    private bool initialSetup = true;
    private ContextMenu contextMenu1;
    private MenuItem menuItem1;
    private MenuItem menuItem2;
    private bool bIsPersonal;
    private MilestoneFreeRoleAclManager freeRoleAclMgr;
    private UserInfo selectedUser;
    private bool readOnly;
    private Button btnCancel;
    private GridView gvMilestone;
    private ArrayList previousView;
    private bool suspendEvent;
    private MilestoneHelper milestoneHelper;
    private Sessions.Session session;

    public event EventHandler DirtyFlagChanged;

    public MilestoneSelectionDialog(
      Sessions.Session session,
      AclMilestone feature,
      int personaId,
      bool readOnly,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.InitializeComponent();
      this.personaId = personaId;
      this.feature = feature;
      this.gvMilestone.ContextMenu = (ContextMenu) null;
      this.previousView = new ArrayList();
      this.readOnly = readOnly;
      this.milestoneHelper = new MilestoneHelper(this.session);
      this.freeRoleAclMgr = (MilestoneFreeRoleAclManager) this.session.ACL.GetAclManager(AclCategory.MilestonesFreeRole);
      this.init();
      this.setView();
      this.makeReadOnly(this.readOnly);
    }

    public MilestoneSelectionDialog(
      Sessions.Session session,
      AclMilestone feature,
      string userId,
      Persona[] personas,
      bool readOnly,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.InitializeComponent();
      this.userId = userId;
      this.selectedUser = this.session.OrganizationManager.GetUser(this.userId);
      this.personas = personas;
      this.feature = feature;
      this.lblLinked.Visible = true;
      this.lblDisconnected.Visible = true;
      this.bIsPersonal = true;
      this.gvMilestone.ImageList = this.imgList;
      this.gvMilestone.ContextMenu = this.contextMenu1;
      this.milestoneHelper = new MilestoneHelper(this.session);
      this.previousView = new ArrayList();
      this.freeRoleAclMgr = (MilestoneFreeRoleAclManager) this.session.ACL.GetAclManager(AclCategory.MilestonesFreeRole);
      this.init();
      this.readOnly = readOnly;
      this.setView();
      this.makeReadOnly(this.readOnly);
    }

    public bool IsReadOnly
    {
      set => this.makeReadOnly(value);
    }

    private void makeReadOnly(bool readOnly)
    {
      this.btnOK.Enabled = !readOnly;
      this.menuItem1.Enabled = !readOnly;
      this.menuItem2.Enabled = !readOnly;
    }

    public MilestoneSelectionDialog(
      Sessions.Session session,
      int allChecked,
      AclMilestone feature,
      int personaId,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.InitializeComponent();
      this.personaId = personaId;
      this.feature = feature;
      this.allChecked = allChecked;
      this.gvMilestone.ContextMenu = (ContextMenu) null;
      this.previousView = new ArrayList();
      this.milestoneHelper = new MilestoneHelper(this.session);
      this.freeRoleAclMgr = (MilestoneFreeRoleAclManager) this.session.ACL.GetAclManager(AclCategory.MilestonesFreeRole);
      this.init();
    }

    public MilestoneSelectionDialog(
      Sessions.Session session,
      int allChecked,
      AclMilestone feature,
      string userId,
      Persona[] personas,
      bool readOnly,
      EventHandler dirtyFlagChanged)
    {
      this.session = session;
      this.DirtyFlagChanged += dirtyFlagChanged;
      this.InitializeComponent();
      this.userId = userId;
      this.selectedUser = this.session.OrganizationManager.GetUser(this.userId);
      this.personas = personas;
      this.feature = feature;
      this.lblLinked.Visible = true;
      this.lblDisconnected.Visible = true;
      this.bIsPersonal = true;
      this.gvMilestone.ImageList = this.imgList;
      this.gvMilestone.ContextMenu = this.contextMenu1;
      this.allChecked = allChecked;
      this.milestoneHelper = new MilestoneHelper(this.session);
      this.previousView = new ArrayList();
      this.freeRoleAclMgr = (MilestoneFreeRoleAclManager) this.session.ACL.GetAclManager(AclCategory.MilestonesFreeRole);
      this.init();
      if (readOnly)
      {
        this.btnOK.Enabled = false;
        this.gvMilestone.Enabled = false;
      }
      else
      {
        this.btnOK.Enabled = true;
        this.gvMilestone.Enabled = true;
      }
    }

    private bool isLoanAssociateFeature(AclMilestone feature)
    {
      if (feature == AclMilestone.AssignLoanTeamMembers)
      {
        this.lblMilestones.Visible = false;
        this.lblRoles.Visible = true;
        this.Text = "Select Roles";
        return true;
      }
      this.lblMilestones.Visible = true;
      this.lblRoles.Visible = false;
      this.Text = " Select Milestones";
      return false;
    }

    public bool IsInitial
    {
      get => this.initialSetup;
      set => this.initialSetup = value;
    }

    private void setDirtyFlag(bool val)
    {
      this.dirty = val;
      if (this.DirtyFlagChanged == null)
        return;
      this.DirtyFlagChanged((object) this, this.dirty ? new EventArgs() : (EventArgs) null);
    }

    private void commitChanges()
    {
      MilestonesAclManager aclManager = (MilestonesAclManager) this.session.ACL.GetAclManager(AclCategory.Milestones);
      this.setDirtyFlag(false);
      for (int nItemIndex = 0; nItemIndex < this.gvMilestone.Items.Count; ++nItemIndex)
      {
        GVItem gvItem = this.gvMilestone.Items[nItemIndex];
        if (gvItem.Tag is EllieMae.EMLite.Workflow.Milestone)
        {
          EllieMae.EMLite.Workflow.Milestone tag = (EllieMae.EMLite.Workflow.Milestone) gvItem.Tag;
          if (this.bIsPersonal)
          {
            if (gvItem.ImageIndex == 1)
              this.milestoneHelper.SetPermission(this.feature, this.userId, (object) gvItem.Checked, tag);
            else
              this.milestoneHelper.SetPermission(this.feature, this.userId, (object) null, tag);
          }
          else
            this.milestoneHelper.SetPermission(this.feature, this.personaId, gvItem.Checked, tag);
        }
        else
        {
          Hashtable tag = (Hashtable) gvItem.Tag;
          if (this.bIsPersonal)
          {
            if (gvItem.ImageIndex == 1)
              this.freeRoleAclMgr.SetPermission(int.Parse(string.Concat(tag[(object) "RoleID"])), this.userId, (object) gvItem.Checked);
            else
              this.freeRoleAclMgr.SetPermission(int.Parse(string.Concat(tag[(object) "RoleID"])), this.userId, (object) null);
          }
          else
            this.freeRoleAclMgr.SetPermission(int.Parse(string.Concat(tag[(object) "RoleID"])), this.personaId, gvItem.Checked);
        }
      }
    }

    private void init()
    {
      Dictionary<GVItem, int> dictionary = new Dictionary<GVItem, int>();
      this.isLoanAssociate = this.isLoanAssociateFeature(this.feature);
      this.entryIdToPerm = new Hashtable();
      if (this.bIsPersonal)
      {
        this.gvMilestone.Columns[0].Width = 45;
        this.gvMilestone.Columns[1].Width = (int) byte.MaxValue;
      }
      if (this.isLoanAssociate)
      {
        this.gvMilestone.Columns[1].Text = "Role";
        this.gvMilestone.Columns[1].Width = 120;
        this.gvMilestone.Columns.Add("Milestone", 135);
      }
      IEnumerable<EllieMae.EMLite.Workflow.Milestone> allMilestonesList = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllMilestonesList();
      MilestonesAclManager aclManager = (MilestonesAclManager) this.session.ACL.GetAclManager(AclCategory.Milestones);
      Hashtable hashtable1 = new Hashtable();
      int num = 0;
      foreach (EllieMae.EMLite.Workflow.Milestone milestone in allMilestonesList)
      {
        string name = milestone.Name;
        if (milestone.Archived)
          name += " (Archived)";
        bool flag1 = false;
        if (name == "Started")
        {
          switch (this.feature)
          {
            case AclMilestone.FinishMilestone:
            case AclMilestone.ReturnFiles:
            case AclMilestone.AssignLoanTeamMembers:
            case AclMilestone.AcceptFiles:
              flag1 = true;
              break;
          }
        }
        if (!flag1)
        {
          GVItem key;
          if (!this.isLoanAssociate)
          {
            key = new GVItem("");
            key.SubItems[1].Value = (object) new MilestoneLabel(milestone);
          }
          else
          {
            if (milestone.Archived || milestone.RoleID == -1)
            {
              key = new GVItem(new string[1]{ "" });
            }
            else
            {
              RoleInfo roleFunction = this.session.SessionObjects.BpmManager.GetRoleFunction(milestone.RoleID);
              if (roleFunction != null)
                key = new GVItem(new string[2]
                {
                  "",
                  roleFunction.RoleName
                });
              else
                key = new GVItem(new string[1]{ "" });
            }
            key.SubItems[2].Value = (object) new MilestoneLabel(milestone);
          }
          bool flag2 = false;
          if (this.allChecked < 2)
          {
            if (this.bIsPersonal)
              num = 1;
            flag2 = this.allChecked == 1;
            key.Checked = flag2;
            this.dirty = true;
          }
          else if (this.personaId == -1)
          {
            Hashtable permissionFromUser = this.milestoneHelper.GetPermissionFromUser(this.feature, this.userId, milestone);
            if (permissionFromUser != null && permissionFromUser.Count > 0)
            {
              if (permissionFromUser.ContainsKey((object) "Result"))
                flag2 = (bool) permissionFromUser[(object) "Result"];
              key.Checked = flag2;
              if (permissionFromUser.ContainsKey((object) "Source"))
                num = !(string.Concat(permissionFromUser[(object) "Source"]) == "USER") ? 0 : 1;
            }
          }
          else
          {
            flag2 = this.milestoneHelper.GetPermission(this.feature, this.personaId, milestone);
            key.Checked = flag2;
          }
          this.entryIdToPerm.Add((object) milestone.MilestoneID, (object) flag2);
          key.Tag = (object) milestone;
          if (milestone.Archived)
          {
            dictionary.Add(key, num);
          }
          else
          {
            this.gvMilestone.Items.Add(key);
            this.gvMilestone.Items[this.gvMilestone.Items.Count - 1].ImageIndex = num;
          }
        }
      }
      Hashtable hashtable2 = new Hashtable();
      if (this.feature == AclMilestone.AssignLoanTeamMembers)
      {
        Hashtable hashtable3 = new Hashtable();
        IDictionaryEnumerator enumerator = (!this.bIsPersonal ? this.freeRoleAclMgr.GetPermissions(this.personaId) : this.freeRoleAclMgr.GetPersonalPermissions(this.selectedUser)).GetEnumerator();
        while (enumerator.MoveNext())
        {
          Hashtable hashtable4 = (Hashtable) enumerator.Value;
          GVItem gvItem = new GVItem(new string[3]
          {
            "",
            string.Concat(hashtable4[(object) "RoleName"]),
            ""
          });
          bool flag3 = false;
          if (this.allChecked < 2)
          {
            if (this.bIsPersonal)
              ;
            bool flag4 = this.allChecked == 1;
            this.dirty = true;
          }
          else
          {
            if (hashtable4.ContainsKey((object) "Permission"))
              flag3 = (bool) hashtable4[(object) "Permission"];
            gvItem.Checked = flag3;
            if (hashtable4.ContainsKey((object) "Source"))
              gvItem.ImageIndex = !(string.Concat(hashtable4[(object) "Source"]) == "User") ? 0 : 1;
          }
          this.entryIdToPerm.Add((object) string.Concat(hashtable4[(object) "RoleName"]), (object) hashtable4);
          gvItem.Tag = (object) hashtable4;
          this.gvMilestone.Items.Add(gvItem);
        }
      }
      if (dictionary.Count <= 0)
        return;
      foreach (GVItem key in dictionary.Keys)
      {
        this.gvMilestone.Items.Add(key);
        this.gvMilestone.Items[this.gvMilestone.Items.Count - 1].ImageIndex = dictionary[key];
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MilestoneSelectionDialog));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.lblMilestones = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.lblRoles = new Label();
      this.imgList = new ImageList(this.components);
      this.lblDisconnected = new Label();
      this.lblLinked = new Label();
      this.contextMenu1 = new ContextMenu();
      this.menuItem1 = new MenuItem();
      this.menuItem2 = new MenuItem();
      this.gvMilestone = new GridView();
      this.SuspendLayout();
      this.lblMilestones.Location = new Point(8, 12);
      this.lblMilestones.Name = "lblMilestones";
      this.lblMilestones.Size = new Size(268, 20);
      this.lblMilestones.TabIndex = 0;
      this.lblMilestones.Text = "Select the milestone(s) the persona can access.";
      this.btnOK.Location = new Point(204, 312);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(64, 24);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(274, 312);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(64, 24);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.lblRoles.Location = new Point(8, 12);
      this.lblRoles.Name = "lblRoles";
      this.lblRoles.Size = new Size(208, 16);
      this.lblRoles.TabIndex = 4;
      this.lblRoles.Text = "Select roles that this persona can assign";
      this.lblRoles.Visible = false;
      this.imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgList.ImageStream");
      this.imgList.TransparentColor = Color.Transparent;
      this.imgList.Images.SetKeyName(0, "");
      this.imgList.Images.SetKeyName(1, "");
      this.lblDisconnected.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblDisconnected.ImageIndex = 1;
      this.lblDisconnected.ImageList = this.imgList;
      this.lblDisconnected.Location = new Point(8, 288);
      this.lblDisconnected.Name = "lblDisconnected";
      this.lblDisconnected.Size = new Size(268, 16);
      this.lblDisconnected.TabIndex = 5;
      this.lblDisconnected.Text = "        Disconnected from Persona Rights";
      this.lblDisconnected.TextAlign = ContentAlignment.MiddleLeft;
      this.lblDisconnected.Visible = false;
      this.lblLinked.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblLinked.ImageIndex = 0;
      this.lblLinked.ImageList = this.imgList;
      this.lblLinked.Location = new Point(8, 273);
      this.lblLinked.Name = "lblLinked";
      this.lblLinked.Size = new Size(268, 16);
      this.lblLinked.TabIndex = 6;
      this.lblLinked.Text = "        Linked with Persona Rights";
      this.lblLinked.TextAlign = ContentAlignment.MiddleLeft;
      this.lblLinked.Visible = false;
      this.contextMenu1.MenuItems.AddRange(new MenuItem[2]
      {
        this.menuItem1,
        this.menuItem2
      });
      this.menuItem1.Index = 0;
      this.menuItem1.Text = "Link with Persona Rights";
      this.menuItem1.Click += new EventHandler(this.menuItem1_Click);
      this.menuItem2.Index = 1;
      this.menuItem2.Text = "Disconnected from Persona Rights";
      this.menuItem2.Click += new EventHandler(this.menuItem2_Click);
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column2";
      gvColumn1.Text = "";
      gvColumn1.Width = 35;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.Text = "Milestone";
      gvColumn2.Width = 265;
      this.gvMilestone.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvMilestone.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvMilestone.Location = new Point(8, 31);
      this.gvMilestone.Name = "gvMilestone";
      this.gvMilestone.Size = new Size(330, 239);
      this.gvMilestone.TabIndex = 8;
      this.gvMilestone.SubItemCheck += new GVSubItemEventHandler(this.gvMilestone_SubItemCheck);
      this.gvMilestone.MouseHover += new EventHandler(this.gvMilestone_MouseHover);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(350, 339);
      this.Controls.Add((Control) this.gvMilestone);
      this.Controls.Add((Control) this.lblLinked);
      this.Controls.Add((Control) this.lblDisconnected);
      this.Controls.Add((Control) this.lblRoles);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.lblMilestones);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (MilestoneSelectionDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = " Milestone Selection";
      this.Closing += new CancelEventHandler(this.MilestoneSelectionDialog_Closing);
      this.ResumeLayout(false);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.setView();
      this.setDirtyFlag(this.dirty);
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    public bool HasBeenModified => this.dirty;

    public void SaveData() => this.commitChanges();

    public bool hasMilestoneChecked()
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvMilestone.Items)
      {
        if (gvItem.Checked)
          return true;
      }
      return false;
    }

    private void menuItem1_Click(object sender, EventArgs e)
    {
      if (this.gvMilestone.SelectedItems == null || this.gvMilestone.SelectedItems.Count <= 0)
        return;
      this.dirty = true;
      this.gvMilestone.SelectedItems[0].Checked = !(this.gvMilestone.SelectedItems[0].Tag is EllieMae.EMLite.Workflow.Milestone) ? this.freeRoleAclMgr.GetPermission(int.Parse(string.Concat(((Hashtable) this.gvMilestone.SelectedItems[0].Tag)[(object) "RoleID"])), this.session.UserInfo.UserPersonas) : this.milestoneHelper.GetPermissionFromUser(this.feature, this.userId, (EllieMae.EMLite.Workflow.Milestone) this.gvMilestone.SelectedItems[0].Tag).Count > 0;
      this.gvMilestone.SelectedItems[0].ImageIndex = 0;
    }

    private void menuItem2_Click(object sender, EventArgs e)
    {
      if (this.gvMilestone.SelectedItems == null || this.gvMilestone.SelectedItems.Count <= 0)
        return;
      this.gvMilestone.SelectedItems[0].ImageIndex = 1;
      this.dirty = true;
    }

    private void setView()
    {
      this.previousView = new ArrayList();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvMilestone.Items)
        this.previousView.Add(gvItem.Clone());
    }

    public ArrayList DataView
    {
      get => this.previousView;
      set => this.previousView = value;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.gvMilestone.Items.Clear();
      this.initialSetup = true;
      foreach (GVItem gvItem in this.previousView)
        this.gvMilestone.Items.Add((GVItem) gvItem.Clone());
    }

    private void MilestoneSelectionDialog_Closing(object sender, CancelEventArgs e)
    {
      this.btnCancel_Click((object) null, (EventArgs) null);
    }

    private void gvMilestone_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (this.suspendEvent || this.initialSetup)
        return;
      if (!this.readOnly)
      {
        if (this.bIsPersonal)
        {
          e.SubItem.ImageIndex = 1;
          this.dirty = true;
        }
        else
        {
          if (this.initialSetup)
            return;
          this.dirty = true;
        }
      }
      else
      {
        this.suspendEvent = true;
        e.SubItem.Checked = !e.SubItem.Checked;
        this.suspendEvent = false;
      }
    }

    private void gvMilestone_MouseHover(object sender, EventArgs e)
    {
      if (!this.initialSetup)
        return;
      this.initialSetup = false;
    }
  }
}

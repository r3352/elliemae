// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.GrantWriteAccessDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.PersonaSetup.SecurityPage
{
  public class GrantWriteAccessDlg : Form
  {
    private Button btnOK;
    private Button btnCancel;
    private Label lblConnected;
    private Label lblDisconnected;
    private ImageList imgListTv;
    private IContainer components;
    private int personaID = -1;
    private bool isPersonal;
    private string userID = "";
    private Persona[] personaList;
    private bool internalAccess;
    private ToolsAclManager aclMgr;
    private ContextMenu contextMenu1;
    private MenuItem miLinked;
    private MenuItem miDisconnected;
    private ToolsAclInfo[] toolsList;
    private bool dirty;
    private bool readOnly;
    private int selectOption = 2;
    private GridView gvMilestone;
    private ArrayList previousView;
    private bool suspendEvent;
    private Sessions.Session session;

    public GrantWriteAccessDlg(Sessions.Session session, int personaID, bool readOnly, int option)
    {
      this.session = session;
      this.InitializeComponent();
      this.personaID = personaID;
      this.gvMilestone.ImageList = (ImageList) null;
      this.gvMilestone.ContextMenu = (ContextMenu) null;
      this.selectOption = option;
      this.readOnly = readOnly;
      this.MakeReadOnly(this.readOnly);
      this.previousView = new ArrayList();
      this.aclMgr = (ToolsAclManager) this.session.ACL.GetAclManager(AclCategory.ToolsGrantWriteAccess);
      this.LoadGVForPersona();
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

    public GrantWriteAccessDlg(
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
      this.previousView = new ArrayList();
      this.lblDisconnected.Visible = true;
      this.lblConnected.Visible = true;
      this.aclMgr = (ToolsAclManager) this.session.ACL.GetAclManager(AclCategory.ToolsGrantWriteAccess);
      this.gvMilestone.ImageList = this.imgListTv;
      this.gvMilestone.ContextMenu = this.contextMenu1;
      this.gvMilestone.Columns[0].Width += 10;
      this.gvMilestone.Columns[2].Width -= 10;
      this.LoadGVForUser();
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
        this.btnOK.Enabled = false;
        this.miDisconnected.Enabled = false;
        this.miLinked.Enabled = false;
      }
      else
      {
        this.btnOK.Enabled = true;
        this.miDisconnected.Enabled = true;
        this.miLinked.Enabled = true;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (GrantWriteAccessDlg));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.contextMenu1 = new ContextMenu();
      this.miLinked = new MenuItem();
      this.miDisconnected = new MenuItem();
      this.imgListTv = new ImageList(this.components);
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.lblConnected = new Label();
      this.lblDisconnected = new Label();
      this.gvMilestone = new GridView();
      this.SuspendLayout();
      this.contextMenu1.MenuItems.AddRange(new MenuItem[2]
      {
        this.miLinked,
        this.miDisconnected
      });
      this.miLinked.Index = 0;
      this.miLinked.Text = "Link with Persona Rights";
      this.miLinked.Click += new EventHandler(this.miLinked_Click);
      this.miDisconnected.Index = 1;
      this.miDisconnected.Text = "Disconnect from Persona Rights";
      this.miDisconnected.Click += new EventHandler(this.miDisconnected_Click);
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.TransparentColor = Color.White;
      this.imgListTv.Images.SetKeyName(0, "");
      this.imgListTv.Images.SetKeyName(1, "");
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(184, 304);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(265, 304);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.lblConnected.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblConnected.ImageIndex = 1;
      this.lblConnected.ImageList = this.imgListTv;
      this.lblConnected.Location = new Point(12, 260);
      this.lblConnected.Name = "lblConnected";
      this.lblConnected.Size = new Size(172, 16);
      this.lblConnected.TabIndex = 3;
      this.lblConnected.Text = "        Linked with Persona Rights";
      this.lblConnected.TextAlign = ContentAlignment.MiddleLeft;
      this.lblConnected.Visible = false;
      this.lblDisconnected.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblDisconnected.ImageIndex = 0;
      this.lblDisconnected.ImageList = this.imgListTv;
      this.lblDisconnected.Location = new Point(12, 276);
      this.lblDisconnected.Name = "lblDisconnected";
      this.lblDisconnected.Size = new Size(220, 16);
      this.lblDisconnected.TabIndex = 4;
      this.lblDisconnected.Text = "        Disconnected from Persona Rights";
      this.lblDisconnected.TextAlign = ContentAlignment.MiddleLeft;
      this.lblDisconnected.Visible = false;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column2";
      gvColumn1.Text = "";
      gvColumn1.Width = 35;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column3";
      gvColumn2.Text = "Role";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.Text = "Milestone";
      gvColumn3.Width = 165;
      this.gvMilestone.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvMilestone.Location = new Point(12, 12);
      this.gvMilestone.Name = "gvMilestone";
      this.gvMilestone.Size = new Size(328, 245);
      this.gvMilestone.TabIndex = 9;
      this.gvMilestone.SubItemCheck += new GVSubItemEventHandler(this.gvMilestone_SubItemCheck);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(352, 333);
      this.Controls.Add((Control) this.gvMilestone);
      this.Controls.Add((Control) this.lblDisconnected);
      this.Controls.Add((Control) this.lblConnected);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (GrantWriteAccessDlg);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = " Grant Write Access";
      this.Closing += new CancelEventHandler(this.GrantWriteAccessDlg_Closing);
      this.ResumeLayout(false);
    }

    private void LoadGVForPersona()
    {
      this.toolsList = this.aclMgr.GetPermissions(this.personaID);
      IEnumerable<EllieMae.EMLite.Workflow.Milestone> activeMilestonesList = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllActiveMilestonesList();
      if (this.toolsList == null || this.toolsList.Length == 0)
        return;
      foreach (EllieMae.EMLite.Workflow.Milestone milestone in activeMilestonesList)
      {
        MilestoneLabel milestoneLabel = new MilestoneLabel(milestone);
        string str = "";
        if (milestone.RoleID > 0)
        {
          RoleInfo roleFunction = this.session.SessionObjects.BpmManager.GetRoleFunction(milestone.RoleID);
          if (roleFunction != null)
            str = roleFunction.Name;
        }
        GVItem gvItem = new GVItem(new string[2]{ "", str });
        gvItem.SubItems[2].Value = (object) milestoneLabel;
        int index = 0;
        bool flag = false;
        for (; index < this.toolsList.Length; ++index)
        {
          if (this.toolsList[index].MilestoneID == milestone.MilestoneID)
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          gvItem.Tag = (object) this.toolsList[index];
          if (this.toolsList[index].Access == 1)
            gvItem.Checked = true;
          this.gvMilestone.Items.Add(gvItem);
        }
      }
      foreach (ToolsAclInfo tools in this.toolsList)
      {
        if (tools.MilestoneID == "-1")
        {
          GVItem gvItem = new GVItem(new string[3]
          {
            "",
            tools.RoleName,
            ""
          });
          gvItem.Tag = (object) tools;
          if (tools.Access == 1)
            gvItem.Checked = true;
          this.gvMilestone.Items.Add(gvItem);
        }
      }
    }

    private void LoadGVForUser()
    {
      this.toolsList = this.aclMgr.GetAccessibleToolsAclInfo(this.userID, this.personaList);
      IEnumerable<EllieMae.EMLite.Workflow.Milestone> activeMilestonesList = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllActiveMilestonesList();
      if (this.toolsList == null || this.toolsList.Length == 0)
        return;
      foreach (EllieMae.EMLite.Workflow.Milestone milestone in activeMilestonesList)
      {
        MilestoneLabel milestoneLabel = new MilestoneLabel(milestone);
        string str;
        if (milestone.RoleID == -1)
        {
          str = "";
        }
        else
        {
          RoleInfo roleFunction = this.session.SessionObjects.BpmManager.GetRoleFunction(milestone.RoleID);
          str = roleFunction == null ? "" : roleFunction.Name;
        }
        GVItem gvItem = new GVItem(new string[2]{ "", str });
        gvItem.SubItems[2].Value = (object) milestoneLabel;
        int index = 0;
        bool flag = false;
        for (; index < this.toolsList.Length; ++index)
        {
          if (this.toolsList[index].MilestoneID == milestone.MilestoneID)
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          gvItem.Tag = (object) this.toolsList[index];
          if (this.toolsList[index].Access == 1)
            gvItem.Checked = true;
          gvItem.ImageIndex = !this.toolsList[index].CustomAccess ? 1 : 0;
          this.gvMilestone.Items.Add(gvItem);
        }
      }
      foreach (ToolsAclInfo tools in this.toolsList)
      {
        if (tools.MilestoneID == "-1")
        {
          GVItem gvItem = new GVItem(new string[3]
          {
            "",
            tools.RoleName,
            ""
          });
          gvItem.Tag = (object) tools;
          if (tools.Access == 1)
            gvItem.Checked = true;
          gvItem.ImageIndex = !tools.CustomAccess ? 1 : 0;
          this.gvMilestone.Items.Add(gvItem);
        }
      }
    }

    private void miLinked_Click(object sender, EventArgs e)
    {
      GVItem selectedItem = this.gvMilestone.SelectedItems.Count > 0 ? this.gvMilestone.SelectedItems[0] : (GVItem) null;
      this.internalAccess = true;
      if (selectedItem != null && selectedItem.ImageIndex < 1)
      {
        this.dirty = true;
        selectedItem.ImageIndex = 1;
        ToolsAclInfo permission = this.aclMgr.GetPermission(this.personaList, ((ToolsAclInfo) selectedItem.Tag).RoleID, ((ToolsAclInfo) selectedItem.Tag).MilestoneID);
        if (permission != null)
        {
          ((ToolsAclInfo) selectedItem.Tag).Access = permission.Access;
          selectedItem.Checked = permission.Access == 1;
        }
        ((ToolsAclInfo) selectedItem.Tag).CustomAccess = false;
      }
      this.internalAccess = false;
    }

    private void miDisconnected_Click(object sender, EventArgs e)
    {
      GVItem selectedItem = this.gvMilestone.SelectedItems.Count > 0 ? this.gvMilestone.SelectedItems[0] : (GVItem) null;
      this.internalAccess = true;
      if (selectedItem != null && selectedItem.ImageIndex > 0)
      {
        this.dirty = true;
        selectedItem.ImageIndex = 0;
        ((ToolsAclInfo) selectedItem.Tag).CustomAccess = true;
      }
      this.internalAccess = false;
    }

    public void SaveData()
    {
      this.dirty = false;
      if (!this.isPersonal)
      {
        ArrayList arrayList = new ArrayList();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvMilestone.Items)
        {
          ToolsAclInfo tag = (ToolsAclInfo) gvItem.Tag;
          tag.Access = !gvItem.Checked ? 0 : 1;
          arrayList.Add((object) tag);
        }
        this.aclMgr.SetPermissions((ToolsAclInfo[]) arrayList.ToArray(typeof (ToolsAclInfo)), this.personaID);
      }
      else
      {
        ArrayList arrayList = new ArrayList();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvMilestone.Items)
        {
          if (((ToolsAclInfo) gvItem.Tag).CustomAccess)
            arrayList.Add(gvItem.Tag);
        }
        this.aclMgr.SetPermissions((ToolsAclInfo[]) arrayList.ToArray(typeof (ToolsAclInfo)), this.userID);
      }
    }

    public bool HasBeenModified() => this.dirty;

    public bool HasSomethingChecked()
    {
      bool flag = false;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvMilestone.Items)
      {
        if (gvItem.Checked)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private void SelectAll()
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvMilestone.Items)
        gvItem.Checked = true;
    }

    private void DeselectAll()
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvMilestone.Items)
        gvItem.Checked = false;
    }

    public int GetImageIndex()
    {
      int imageIndex = 0;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvMilestone.Items)
      {
        if (gvItem.ImageIndex == 0)
        {
          imageIndex = 1;
          break;
        }
      }
      return imageIndex;
    }

    public void setView()
    {
      this.previousView = new ArrayList();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvMilestone.Items)
        this.previousView.Add(gvItem.Clone());
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
      this.gvMilestone.Items.Clear();
      foreach (GVItem gvItem in this.previousView)
        this.gvMilestone.Items.Add((GVItem) gvItem.Clone());
      this.internalAccess = false;
    }

    public ArrayList DataView
    {
      get => this.previousView;
      set => this.previousView = value;
    }

    private void GrantWriteAccessDlg_Closing(object sender, CancelEventArgs e)
    {
      this.btnCancel_Click((object) null, (EventArgs) null);
    }

    private void gvMilestone_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (this.suspendEvent)
        return;
      if (this.readOnly)
      {
        this.suspendEvent = true;
        e.SubItem.Checked = !e.SubItem.Checked;
        this.suspendEvent = false;
      }
      else
      {
        if (this.internalAccess)
          return;
        e.SubItem.ImageIndex = 0;
        ToolsAclInfo tag = (ToolsAclInfo) this.gvMilestone.Items[e.SubItem.Parent.Index].Tag;
        tag.CustomAccess = true;
        tag.Access = !e.SubItem.Checked ? 0 : 1;
        this.gvMilestone.Items[e.SubItem.Parent.Index].Tag = (object) tag;
        this.dirty = true;
      }
    }
  }
}

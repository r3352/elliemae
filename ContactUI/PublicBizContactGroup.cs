// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.PublicBizContactGroup
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class PublicBizContactGroup : UserControl
  {
    private IContainer components;
    private GroupContainer gcGroup;
    private GridView gvContactGroupList;
    private StandardIconButton siBtnDelete;
    private StandardIconButton siBtnEdit;
    private StandardIconButton siBtnNew;
    private ToolTip toolTip1;
    private FlowLayoutPanel flowLayoutPanel1;

    public PublicBizContactGroup()
    {
      this.InitializeComponent();
      this.populateControls();
      this.enforceSecurity();
    }

    private void enforceSecurity()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (Session.UserInfo.IsSuperAdministrator() || aclManager.GetUserApplicationRight(AclFeature.SettingsTab_Company_PublicBizContactGroup))
        return;
      this.siBtnDelete.Visible = false;
      this.siBtnNew.Visible = false;
    }

    private void populateControls()
    {
      this.gvContactGroupList.Items.Clear();
      ContactGroupInfo[] filteredContactGroup = this.GetFilteredContactGroup();
      if (filteredContactGroup != null)
      {
        foreach (ContactGroupInfo contactGroupInfo in filteredContactGroup)
        {
          int length = contactGroupInfo.ContactIds != null ? contactGroupInfo.ContactIds.Length : 0;
          this.gvContactGroupList.Items.Add(new GVItem(contactGroupInfo.GroupName)
          {
            SubItems = {
              (object) length.ToString()
            },
            Tag = (object) contactGroupInfo
          });
        }
      }
      if (0 >= this.gvContactGroupList.Items.Count)
        return;
      this.gvContactGroupList.Items[0].Selected = true;
    }

    private ContactGroupInfo[] GetFilteredContactGroup()
    {
      ContactGroupInfo[] filteredContactGroup = Session.ContactGroupManager.GetPublicBizContactGroups();
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (!Session.UserInfo.IsSuperAdministrator() && !aclManager.GetUserApplicationRight(AclFeature.SettingsTab_Company_PublicBizContactGroup))
      {
        BizGroupRef[] contactGroupRefs = Session.AclGroupManager.GetBizContactGroupRefs(Session.UserID, true);
        ArrayList arrayList = new ArrayList();
        foreach (ContactGroupInfo contactGroupInfo in filteredContactGroup)
        {
          foreach (BizGroupRef bizGroupRef in contactGroupRefs)
          {
            if (bizGroupRef.BizGroupID == contactGroupInfo.GroupId && !arrayList.Contains((object) contactGroupInfo))
            {
              arrayList.Add((object) contactGroupInfo);
              break;
            }
          }
        }
        filteredContactGroup = (ContactGroupInfo[]) arrayList.ToArray(typeof (ContactGroupInfo));
      }
      return filteredContactGroup;
    }

    private void siBtnDelete_Click(object sender, EventArgs e)
    {
      if (this.gvContactGroupList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a public business contact group to delete.");
      }
      else if (DialogResult.No == Utils.Dialog((IWin32Window) this, "Are you sure you want to delete selected contact group(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
      {
        this.gvContactGroupList.Focus();
      }
      else
      {
        foreach (GVItem selectedItem in this.gvContactGroupList.SelectedItems)
          Session.ContactGroupManager.DeleteContactGroup(((ContactGroupInfo) selectedItem.Tag).GroupId);
        this.populateControls();
      }
    }

    private void siBtnNew_Click(object sender, EventArgs e)
    {
      if (DialogResult.OK != new EditPublicBizContactGroup().ShowDialog((IWin32Window) this))
        return;
      this.populateControls();
    }

    private void siBtnEdit_Click(object sender, EventArgs e)
    {
      if (this.gvContactGroupList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a public business contact group to edit.");
      }
      else
      {
        if (DialogResult.OK != new EditPublicBizContactGroup((ContactGroupInfo) this.gvContactGroupList.SelectedItems[0].Tag).ShowDialog((IWin32Window) this))
          return;
        this.populateControls();
      }
    }

    private void gvContactGroupList_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.siBtnEdit_Click((object) null, (EventArgs) null);
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
      this.gcGroup = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.siBtnDelete = new StandardIconButton();
      this.siBtnEdit = new StandardIconButton();
      this.siBtnNew = new StandardIconButton();
      this.gvContactGroupList = new GridView();
      this.toolTip1 = new ToolTip(this.components);
      this.gcGroup.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.siBtnDelete).BeginInit();
      ((ISupportInitialize) this.siBtnEdit).BeginInit();
      ((ISupportInitialize) this.siBtnNew).BeginInit();
      this.SuspendLayout();
      this.gcGroup.Controls.Add((Control) this.flowLayoutPanel1);
      this.gcGroup.Controls.Add((Control) this.gvContactGroupList);
      this.gcGroup.Dock = DockStyle.Fill;
      this.gcGroup.Location = new Point(0, 0);
      this.gcGroup.Name = "gcGroup";
      this.gcGroup.Size = new Size(735, 518);
      this.gcGroup.TabIndex = 0;
      this.gcGroup.Text = "Public Business Contacts Group";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnDelete);
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnEdit);
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnNew);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(647, 4);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Padding = new Padding(0, 0, 10, 0);
      this.flowLayoutPanel1.Size = new Size(88, 16);
      this.flowLayoutPanel1.TabIndex = 4;
      this.siBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnDelete.BackColor = Color.Transparent;
      this.siBtnDelete.Location = new Point(62, 0);
      this.siBtnDelete.Margin = new Padding(3, 0, 0, 0);
      this.siBtnDelete.Name = "siBtnDelete";
      this.siBtnDelete.Size = new Size(16, 16);
      this.siBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.siBtnDelete.TabIndex = 2;
      this.siBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnDelete, "Delete");
      this.siBtnDelete.Click += new EventHandler(this.siBtnDelete_Click);
      this.siBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnEdit.BackColor = Color.Transparent;
      this.siBtnEdit.Location = new Point(43, 0);
      this.siBtnEdit.Margin = new Padding(3, 0, 0, 0);
      this.siBtnEdit.Name = "siBtnEdit";
      this.siBtnEdit.Size = new Size(16, 16);
      this.siBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.siBtnEdit.TabIndex = 1;
      this.siBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnEdit, "Edit");
      this.siBtnEdit.Click += new EventHandler(this.siBtnEdit_Click);
      this.siBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnNew.BackColor = Color.Transparent;
      this.siBtnNew.Location = new Point(24, 0);
      this.siBtnNew.Margin = new Padding(0);
      this.siBtnNew.Name = "siBtnNew";
      this.siBtnNew.Size = new Size(16, 16);
      this.siBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.siBtnNew.TabIndex = 0;
      this.siBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnNew, "New");
      this.siBtnNew.Click += new EventHandler(this.siBtnNew_Click);
      this.gvContactGroupList.AllowMultiselect = false;
      this.gvContactGroupList.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Group Name";
      gvColumn1.Width = 300;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "# of Contacts";
      gvColumn2.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn2.Width = 100;
      this.gvContactGroupList.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvContactGroupList.Dock = DockStyle.Fill;
      this.gvContactGroupList.Location = new Point(1, 26);
      this.gvContactGroupList.Name = "gvContactGroupList";
      this.gvContactGroupList.Size = new Size(733, 491);
      this.gvContactGroupList.TabIndex = 3;
      this.gvContactGroupList.ItemDoubleClick += new GVItemEventHandler(this.gvContactGroupList_ItemDoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcGroup);
      this.Name = nameof (PublicBizContactGroup);
      this.Size = new Size(735, 518);
      this.gcGroup.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.siBtnDelete).EndInit();
      ((ISupportInitialize) this.siBtnEdit).EndInit();
      ((ISupportInitialize) this.siBtnNew).EndInit();
      this.ResumeLayout(false);
    }
  }
}

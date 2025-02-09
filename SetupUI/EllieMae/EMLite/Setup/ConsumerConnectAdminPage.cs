// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ConsumerConnectAdminPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.PersonaSetup.SecurityPage;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ConsumerConnectAdminPage : PersonaTreePageBase
  {
    private int personaID = -1;
    private Persona[] personaList;
    private string userID = "";
    private IContainer components;
    private Panel panelConsumerConnect;
    private CheckBox cmsContributor_chkbx;
    private CheckBox cmsAdmin_chkbx;
    private CheckBox cmsAssistant_chkbx;

    public int PersonaID
    {
      get => this.personaID;
      set
      {
        if (this.personaID != value)
          this.Reset();
        this.personaID = value;
      }
    }

    public ConsumerConnectAdminPage(
      Sessions.Session session,
      int personaId,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.personaID = personaId;
      this.Title = "Consumer Connect Website Builder Settings";
      this.MouseUpEvent += new PersonaTreeNodeMouseUp(this.NodeMouseUp);
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.NodeChecked);
      this.menuItemDisconnectFromPersona.Click += new EventHandler(this.disconnectFromPersona_Click);
      this.personaID = personaId;
      this.bIsUserSetup = false;
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
      this.securityHelper = (IFeatureSecurityHelper) new CCAdminSecurityHelper(this.session, personaId);
      this.init();
    }

    public ConsumerConnectAdminPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.Title = "Consumer Connect Website Builder Settings";
      this.MouseUpEvent += new PersonaTreeNodeMouseUp(this.NodeMouseUp);
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.NodeChecked);
      this.menuItemDisconnectFromPersona.Click += new EventHandler(this.disconnectFromPersona_Click);
      this.bIsUserSetup = true;
      this.treeViewTabs.ImageList = this.imgListTv;
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
      this.securityHelper = (IFeatureSecurityHelper) new CCAdminSecurityHelper(this.session, userId, personas);
      this.userID = userId;
      this.personaList = personas;
      this.init();
    }

    private void NodeMouseUp(TreeNode node)
    {
    }

    private void NodeChecked(TreeNode node)
    {
      if (node != null && node.Text == "WebAdmin")
      {
        if (!node.Checked || node.NextVisibleNode == null)
          return;
        node.NextVisibleNode.Checked = true;
        this.securityHelper.SetNodeImageIndex(node.NextVisibleNode, 1);
        this.securityHelper.SetNodeUpdateStatus(node.NextVisibleNode, true);
      }
      else
      {
        if (node == null || !(node.Text == "WebContent") || node.Checked || node.PrevVisibleNode == null)
          return;
        node.PrevVisibleNode.Checked = false;
        this.securityHelper.SetNodeUpdateStatus(node.PrevVisibleNode, true);
      }
    }

    private void disconnectFromPersona_Click(object sender, EventArgs e)
    {
      TreeNode selectedNode = this.treeViewTabs.SelectedNode;
      if (selectedNode == null || !(selectedNode.Text == "WebAdmin") || selectedNode.NextVisibleNode == null)
        return;
      this.securityHelper.SetNodeImageIndex(selectedNode.NextVisibleNode, 1);
      this.securityHelper.SetNodeUpdateStatus(selectedNode.NextVisibleNode, true);
    }

    public void Save()
    {
      if (!this.NeedToSaveData())
        return;
      this.UpdatePermissions();
    }

    public void Reset() => this.ResetTree();

    public bool NeedToSaveData() => this.hasBeenModified();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panelConsumerConnect = new Panel();
      this.cmsAdmin_chkbx = new CheckBox();
      this.cmsContributor_chkbx = new CheckBox();
      this.cmsAssistant_chkbx = new CheckBox();
      this.panelConsumerConnect.SuspendLayout();
      this.SuspendLayout();
      this.panelConsumerConnect.Controls.Add((Control) this.cmsContributor_chkbx);
      this.panelConsumerConnect.Controls.Add((Control) this.cmsAdmin_chkbx);
      this.panelConsumerConnect.Controls.Add((Control) this.cmsAssistant_chkbx);
      this.panelConsumerConnect.Dock = DockStyle.Fill;
      this.panelConsumerConnect.Location = new Point(0, 0);
      this.panelConsumerConnect.Name = "panelConsumerConnect";
      this.panelConsumerConnect.Size = new Size(565, 325);
      this.panelConsumerConnect.TabIndex = 0;
      this.cmsAdmin_chkbx.AutoSize = true;
      this.cmsAdmin_chkbx.Location = new Point(12, 12);
      this.cmsAdmin_chkbx.Name = "cmsAdmin_chkbx";
      this.cmsAdmin_chkbx.Size = new Size(113, 24);
      this.cmsAdmin_chkbx.TabIndex = 0;
      this.cmsAdmin_chkbx.Text = "WebAdmin";
      this.cmsAdmin_chkbx.UseVisualStyleBackColor = true;
      this.cmsContributor_chkbx.AutoSize = true;
      this.cmsContributor_chkbx.Location = new Point(12, 39);
      this.cmsContributor_chkbx.Name = "cmsContributor_chkbx";
      this.cmsContributor_chkbx.Size = new Size(125, 24);
      this.cmsContributor_chkbx.TabIndex = 1;
      this.cmsContributor_chkbx.Text = "WebContent";
      this.cmsContributor_chkbx.UseVisualStyleBackColor = true;
      this.cmsAssistant_chkbx.AutoSize = true;
      this.cmsAssistant_chkbx.Location = new Point(12, 65);
      this.cmsAssistant_chkbx.Name = "cmsAssistant_chkbx";
      this.cmsAssistant_chkbx.Size = new Size(134, 24);
      this.cmsAssistant_chkbx.TabIndex = 2;
      this.cmsAssistant_chkbx.Text = "WebAssistant";
      this.cmsAssistant_chkbx.UseVisualStyleBackColor = true;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.AutoScroll = true;
      this.ClientSize = new Size(565, 325);
      this.Controls.Add((Control) this.panelConsumerConnect);
      this.Name = nameof (ConsumerConnectAdminPage);
      this.Text = nameof (ConsumerConnectAdminPage);
      this.panelConsumerConnect.ResumeLayout(false);
      this.panelConsumerConnect.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}

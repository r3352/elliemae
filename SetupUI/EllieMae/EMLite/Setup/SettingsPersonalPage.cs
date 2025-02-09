// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SettingsPersonalPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.PersonaSetup.SecurityPage;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SettingsPersonalPage : PersonaTreePageBase
  {
    private System.ComponentModel.Container components;
    private ePassServices serviceDlg;
    private int selectOption = 2;
    private int personaID;
    private Hashtable cachedData = new Hashtable();
    private string userID;
    private Persona[] personaList;

    public SettingsPersonalPage(
      Sessions.Session session,
      int personaId,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.Title = "Personal Settings";
      this.personaID = personaId;
      this.MouseUpEvent += new PersonaTreeNodeMouseUp(this.NodeMouseUp);
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.NodeChecked);
      this.bIsUserSetup = false;
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
      this.securityHelper = (IFeatureSecurityHelper) new SettingsPersonalSecurityHelper(session, personaId);
      this.InitialSpecialDepNodes();
      this.init();
    }

    public SettingsPersonalPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.Title = "Personal Settings";
      this.userID = userId;
      this.MouseUpEvent += new PersonaTreeNodeMouseUp(this.NodeMouseUp);
      this.AfterCheckedEvent += new PersonaTreeNodeAfterChecked(this.NodeChecked);
      this.personaList = personas;
      this.bIsUserSetup = true;
      this.treeViewTabs.ImageList = this.imgListTv;
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
      this.securityHelper = (IFeatureSecurityHelper) new SettingsPersonalSecurityHelper(this.session, userId, personas);
      this.InitialSpecialDepNodes();
      this.init();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SettingsPersonalPage));
      this.gcTreeView.SuspendLayout();
      this.SuspendLayout();
      this.treeViewTabs.LineColor = Color.Black;
      this.treeViewTabs.Size = new Size(526, 293);
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.Images.SetKeyName(0, "");
      this.imgListTv.Images.SetKeyName(1, "");
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(528, 361);
      this.Name = nameof (SettingsPersonalPage);
      this.gcTreeView.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public int PersonaID
    {
      get => this.personaID;
      set
      {
        if (this.personaID != value)
        {
          this.personaID = value;
          this.Reset();
        }
        else
          this.personaID = value;
      }
    }

    public void Save()
    {
      if (!this.NeedToSaveData())
        return;
      if (this.serviceDlg != null && this.serviceDlg.HasBeenModified)
        this.serviceDlg.SaveData();
      this.UpdatePermissions();
    }

    public void Reset()
    {
      this.InitialSpecialDepNodes();
      this.ResetTree();
      this.serviceDlg = (ePassServices) null;
      this.cachedData = new Hashtable();
    }

    public bool NeedToSaveData()
    {
      return this.serviceDlg != null && this.serviceDlg.HasBeenModified || this.hasBeenModified();
    }

    private void NodeMouseUp(TreeNode node)
    {
      if (node.Tag == null || (AclFeature) node.Tag != AclFeature.LoanTab_Other_ePASS)
        return;
      if (!this.bIsUserSetup)
      {
        if (this.serviceDlg == null || this.selectOption < 2)
        {
          if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanTab_Other_ePASS))
          {
            this.serviceDlg = new ePassServices(this.session, AclFeature.LoanTab_Other_ePASS, this.personaID, this.readOnly, 2);
            this.cachedData[(object) AclFeature.LoanTab_Other_ePASS] = (object) this.serviceDlg.DataView;
          }
          this.serviceDlg = new ePassServices(this.session, AclFeature.LoanTab_Other_ePASS, this.personaID, this.readOnly, this.selectOption);
          this.selectOption = 2;
        }
        else
          this.serviceDlg.IsReadOnly = this.readOnly;
      }
      else if (this.serviceDlg == null || this.selectOption < 2)
      {
        if (this.selectOption < 2 && !this.cachedData.ContainsKey((object) AclFeature.LoanTab_Other_ePASS))
        {
          this.serviceDlg = new ePassServices(this.session, AclFeature.LoanTab_Other_ePASS, this.userID, this.personaList, this.readOnly, 2);
          this.cachedData[(object) AclFeature.LoanTab_Other_ePASS] = (object) this.serviceDlg.DataView;
        }
        this.serviceDlg = new ePassServices(this.session, AclFeature.LoanTab_Other_ePASS, this.userID, this.personaList, this.readOnly, this.selectOption);
        this.selectOption = 2;
      }
      if (this.cachedData[(object) AclFeature.LoanTab_Other_ePASS] != null)
        this.serviceDlg.DataView = (ArrayList) this.cachedData[(object) AclFeature.LoanTab_Other_ePASS];
      if (DialogResult.OK == this.serviceDlg.ShowDialog((IWin32Window) this))
      {
        this.cachedData[(object) AclFeature.LoanTab_Other_ePASS] = (object) this.serviceDlg.DataView;
        if (this.serviceDlg.HasBeenModified)
          this.setDirtyFlag(true);
      }
      if (this.serviceDlg.HasSomethingChecked() && !node.Checked)
      {
        this.bInit = true;
        node.Checked = true;
        this.bInit = false;
      }
      else if (!this.serviceDlg.HasSomethingChecked() && node.Checked)
      {
        this.bInit = true;
        node.Checked = false;
        this.bInit = false;
      }
      if (!this.bIsUserSetup)
        return;
      node.ImageIndex = this.serviceDlg.GetImageIndex();
      node.SelectedImageIndex = this.serviceDlg.GetImageIndex();
    }

    private void NodeChecked(TreeNode node)
    {
      if (this.bInit || node.Tag == null || (AclFeature) node.Tag != AclFeature.LoanTab_Other_ePASS)
        return;
      this.selectOption = !node.Checked ? 0 : 1;
      this.NodeMouseUp(node);
    }

    private void InitialSpecialDepNodes()
    {
      this.serviceDlg = this.bIsUserSetup ? new ePassServices(this.session, AclFeature.LoanTab_Other_ePASS, this.userID, this.personaList, this.readOnly, this.selectOption) : new ePassServices(this.session, AclFeature.LoanTab_Other_ePASS, this.personaID, this.readOnly, this.selectOption);
      Hashtable specialDepTreeNodes1 = new Hashtable();
      Hashtable specialDepTreeNodes2 = new Hashtable();
      if (this.serviceDlg.HasSomethingChecked())
        specialDepTreeNodes1.Add((object) "Individual ePASS", (object) true);
      else
        specialDepTreeNodes1.Add((object) "Individual ePASS", (object) false);
      specialDepTreeNodes2.Add((object) "Individual ePASS", (object) this.serviceDlg.GetImageIndex());
      this.securityHelper.setSpecialDepTreeNodes(specialDepTreeNodes1);
      this.securityHelper.setSpecialDepTreeNodesImg(specialDepTreeNodes2);
    }
  }
}

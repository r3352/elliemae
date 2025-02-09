// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ContactsMgrPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.PersonaSetup.SecurityPage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ContactsMgrPage : PersonaTreePageBase
  {
    private static string oriLoanNodePath = "Access to Contacts Tab\\Borrower Contacts\\Originate Loan/Order Credit/Product and Pricing";
    private List<string> triggerNodes = new List<string>((IEnumerable<string>) new string[3]
    {
      ContactsMgrPage.oriLoanNodePath,
      ContactsMgrPage.oriLoanNodePath + "\\New Blank Loan",
      ContactsMgrPage.oriLoanNodePath + "\\New from Template"
    });
    private ContactsOriginateLoanDlg contactsOriginateLoanDlg;
    private int personaID;
    private string userID;
    private Persona[] personas;
    private Hashtable cachedData = new Hashtable();
    private IContainer components;

    public event ContactsMgrPage.OriginateLoanFeatureStatusChanged CreateLoanStatusChanged;

    public event EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.PersonaAccess HasPipelineLoanTabAccessEvent;

    public ContactsMgrPage(Sessions.Session session, int personaID, EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.bIsUserSetup = false;
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
      this.securityHelper = (IFeatureSecurityHelper) new ContactsSecurityHelper(this.session, personaID);
      this.MouseUpEvent += new PersonaTreeNodeMouseUp(this.NodeMouseUp);
      this.personaID = personaID;
      this.InitialSpecialDepNodes();
      this.init();
      this.myInit();
    }

    public ContactsMgrPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.bIsUserSetup = true;
      this.treeViewTabs.ImageList = this.imgListTv;
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
      this.securityHelper = (IFeatureSecurityHelper) new ContactsSecurityHelper(this.session, userId, personas);
      this.MouseUpEvent += new PersonaTreeNodeMouseUp(this.NodeMouseUp);
      this.userID = userId;
      this.personas = personas;
      this.bInit = true;
      this.InitialSpecialDepNodes();
      this.init();
      this.myInit();
    }

    private bool IsPersonal => this.bIsUserSetup;

    private void myInit() => this.Title = "Contacts";

    private bool ContactsMgrPage_BeforeCheckedEvent(TreeNode node)
    {
      return !this.triggerNodes.Contains(node.FullPath) && ContactsMgrPage.oriLoanNodePath.IndexOf(node.FullPath) != 0 || this.findTreeNode(ContactsMgrPage.oriLoanNodePath).Checked || this.HasPipelineLoanTabAccessEvent == null || node.Checked || this.HasPipelineLoanTabAccessEvent() || Utils.Dialog((IWin32Window) this, "Checking this option will grant this persona's access to the Pipeline/Loan tab.  Are you sure you want to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel;
    }

    private void ContactsMgrPage_AfterCheckedEvent(TreeNode node)
    {
      if (!this.triggerNodes.Contains(node.FullPath) && ContactsMgrPage.oriLoanNodePath.IndexOf(node.FullPath) != 0)
        return;
      TreeNode treeNode = this.findTreeNode(ContactsMgrPage.oriLoanNodePath);
      if (node != treeNode && treeNode.Checked || this.HasPipelineLoanTabAccessEvent != null && this.HasPipelineLoanTabAccessEvent() || this.CreateLoanStatusChanged == null)
        return;
      this.CreateLoanStatusChanged(node.Checked ? AclTriState.True : AclTriState.False);
    }

    public void Save()
    {
      if (!this.NeedToSaveData())
        return;
      if (this.contactsOriginateLoanDlg != null)
        this.contactsOriginateLoanDlg.SaveData();
      this.UpdatePermissions();
    }

    public void Reset()
    {
      this.InitialSpecialDepNodes();
      this.ResetTree();
      this.contactsOriginateLoanDlg = (ContactsOriginateLoanDlg) null;
      this.cachedData.Clear();
    }

    public override void MakeReadOnly(bool makeReadOnly)
    {
      this.MakeReadOnly(makeReadOnly, true);
      if (this.contactsOriginateLoanDlg == null)
        return;
      this.contactsOriginateLoanDlg.MakeReadOnly(makeReadOnly);
    }

    public bool NeedToSaveData() => this.hasBeenModified();

    public void DisallowLoanOrigination()
    {
      TreeNode treeNode = this.findTreeNode(ContactsMgrPage.oriLoanNodePath);
      if (treeNode == null || !treeNode.Checked)
        return;
      treeNode.Checked = false;
      if (this.contactsOriginateLoanDlg == null)
        return;
      this.contactsOriginateLoanDlg.UnselectAll();
    }

    public bool HasOriginateLoanAccess()
    {
      TreeNode treeNode = this.findTreeNode(ContactsMgrPage.oriLoanNodePath);
      return treeNode != null && treeNode.Checked;
    }

    public override void SetPersona(int personaId)
    {
      if (this.personaID == personaId)
        return;
      this.personaID = personaId;
      base.SetPersona(personaId);
      this.Reset();
    }

    private void NodeMouseUp(TreeNode node)
    {
      if (node.Tag == null || (AclFeature) node.Tag != AclFeature.Cnt_Borrower_OriginateLoan)
        return;
      bool result = this.ShowOriginateLoanOptionsDialogAndGetResult();
      this.bInit = true;
      node.Checked = result;
      this.bInit = false;
    }

    private bool ShowOriginateLoanOptionsDialogAndGetResult()
    {
      if (this.contactsOriginateLoanDlg == null)
      {
        if (this.IsPersonal)
        {
          this.contactsOriginateLoanDlg = new ContactsOriginateLoanDlg(this.session, this.userID, this.personas, this.readOnly);
          this.contactsOriginateLoanDlg.CreateLoanStatusChanged += new ContactsOriginateLoanDlg.OriginateLoanFeatureStatusChanged(this.contactsOriginateLoanDlg_CreateLoanStatusChanged);
          this.contactsOriginateLoanDlg.HasPipelineLoanTabAccessEvent += new EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.PersonaAccess(this.contactsOriginateLoanDlg_HasPipelineLoanTabAccessEvent);
        }
        else
        {
          this.contactsOriginateLoanDlg = new ContactsOriginateLoanDlg(this.session, this.personaID, this.readOnly);
          this.contactsOriginateLoanDlg.CreateLoanStatusChanged += new ContactsOriginateLoanDlg.OriginateLoanFeatureStatusChanged(this.contactsOriginateLoanDlg_CreateLoanStatusChanged);
          this.contactsOriginateLoanDlg.HasPipelineLoanTabAccessEvent += new EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.PersonaAccess(this.contactsOriginateLoanDlg_HasPipelineLoanTabAccessEvent);
        }
      }
      if (this.cachedData[(object) AclFeature.Cnt_Borrower_OriginateLoan] != null)
        this.contactsOriginateLoanDlg.DataView = (ArrayList) this.cachedData[(object) AclFeature.Cnt_Borrower_OriginateLoan];
      if (DialogResult.OK == this.contactsOriginateLoanDlg.ShowDialog((IWin32Window) this))
      {
        this.cachedData[(object) AclFeature.Cnt_Borrower_OriginateLoan] = (object) this.contactsOriginateLoanDlg.DataView;
        if (this.contactsOriginateLoanDlg.HasBeenModified)
          this.setDirtyFlag(true);
      }
      return this.contactsOriginateLoanDlg.HasSomethingChecked();
    }

    private void contactsOriginateLoanDlg_CreateLoanStatusChanged(AclTriState state)
    {
      if (this.CreateLoanStatusChanged == null)
        return;
      this.CreateLoanStatusChanged(state);
    }

    private bool contactsOriginateLoanDlg_HasPipelineLoanTabAccessEvent()
    {
      return this.HasPipelineLoanTabAccessEvent != null && this.HasPipelineLoanTabAccessEvent();
    }

    private void InitialSpecialDepNodes()
    {
      if (this.IsPersonal)
      {
        this.contactsOriginateLoanDlg = new ContactsOriginateLoanDlg(this.session, this.userID, this.personas, this.readOnly);
        this.contactsOriginateLoanDlg.CreateLoanStatusChanged += new ContactsOriginateLoanDlg.OriginateLoanFeatureStatusChanged(this.contactsOriginateLoanDlg_CreateLoanStatusChanged);
        this.contactsOriginateLoanDlg.HasPipelineLoanTabAccessEvent += new EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.PersonaAccess(this.contactsOriginateLoanDlg_HasPipelineLoanTabAccessEvent);
      }
      else
      {
        this.contactsOriginateLoanDlg = new ContactsOriginateLoanDlg(this.session, this.personaID, this.readOnly);
        this.contactsOriginateLoanDlg.CreateLoanStatusChanged += new ContactsOriginateLoanDlg.OriginateLoanFeatureStatusChanged(this.contactsOriginateLoanDlg_CreateLoanStatusChanged);
        this.contactsOriginateLoanDlg.HasPipelineLoanTabAccessEvent += new EllieMae.EMLite.Setup.PersonaSetup.SecurityPage.PersonaAccess(this.contactsOriginateLoanDlg_HasPipelineLoanTabAccessEvent);
      }
      Hashtable specialDepTreeNodes1 = new Hashtable();
      Hashtable specialDepTreeNodes2 = new Hashtable();
      bool flag = this.contactsOriginateLoanDlg.HasSomethingChecked();
      specialDepTreeNodes1.Add((object) "Originate Loan/Order Credit/Product and Pricing", (object) flag);
      int imageIndex = this.contactsOriginateLoanDlg.GetImageIndex();
      specialDepTreeNodes2.Add((object) "Originate Loan/Order Credit/Product and Pricing", (object) imageIndex);
      this.securityHelper.setSpecialDepTreeNodes(specialDepTreeNodes1);
      this.securityHelper.setSpecialDepTreeNodesImg(specialDepTreeNodes2);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ContactsMgrPage));
      this.gcTreeView.SuspendLayout();
      this.SuspendLayout();
      this.treeViewTabs.LineColor = Color.Black;
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.Images.SetKeyName(0, "");
      this.imgListTv.Images.SetKeyName(1, "");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(528, 361);
      this.Name = nameof (ContactsMgrPage);
      this.Text = nameof (ContactsMgrPage);
      this.gcTreeView.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public delegate void OriginateLoanFeatureStatusChanged(AclTriState state);
  }
}

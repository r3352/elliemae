// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BizPartnerTabForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BizPartnerTabForm : ContactTabForm
  {
    private BizPartnerInfo1Form detailForm;
    private BizPartnerInfo2Form extraForm;
    private BizPartnerNotesForm noteForm;
    private BizPartnerHistoryForm histForm;
    private BizPartnerLoansForm loansForm;
    private CustomFieldsControl[] customFieldsControls;
    private TabPage[] dynamicTabPages = new TabPage[5];
    private CategoryCustomFieldsControl ctlCategoryCustomFields;
    private ContactType currentSource = ContactType.PublicBiz;
    private TabControl tabControl1;
    private TabPage tabPageDetail;
    private TabPage tabPageExtra;
    private TabPage tabPageNotes;
    private TabPage tabPageHistory;
    private TabPage tabPageCustom1;
    private TabPage tabPageCustom2;
    private TabPage tabPageCustom3;
    private TabPage tabPageCustom4;
    private TabPage tabPageCustom5;
    private TabPage tabPageCategory;
    private TabPage tabPageLoans;
    private System.ComponentModel.Container components;
    private BizPartnerListForm parentForm;
    private bool dirty;
    private FeaturesAclManager aclMgr;
    private Color backColor = Color.AliceBlue;
    private Color btnColor = Color.AliceBlue;
    private Color contactColor = Color.AliceBlue;

    public event BizPartnerSummaryChangeEventHandler DataChanged;

    public event EventHandler RequireContactListRefresh;

    public BizPartnerTabForm(BizPartnerListForm parentForm, ContactType source)
    {
      this.InitializeComponent();
      this.parentForm = parentForm;
      this.enforceSecurity();
      this.currentSource = source;
      this.Init();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void enforceSecurity()
    {
      this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Biz_LoansTab))
        this.tabControl1.TabPages.Remove(this.tabPageLoans);
      if (Session.EncompassEdition != EncompassEdition.Broker)
        return;
      this.tabControl1.TabPages.Remove(this.tabPageCategory);
    }

    private void InitializeComponent()
    {
      this.tabControl1 = new TabControl();
      this.tabPageDetail = new TabPage();
      this.tabPageExtra = new TabPage();
      this.tabPageNotes = new TabPage();
      this.tabPageHistory = new TabPage();
      this.tabPageLoans = new TabPage();
      this.tabPageCustom1 = new TabPage();
      this.tabPageCustom2 = new TabPage();
      this.tabPageCustom3 = new TabPage();
      this.tabPageCustom4 = new TabPage();
      this.tabPageCustom5 = new TabPage();
      this.tabPageCategory = new TabPage();
      this.tabControl1.SuspendLayout();
      this.SuspendLayout();
      this.tabControl1.Controls.Add((Control) this.tabPageDetail);
      this.tabControl1.Controls.Add((Control) this.tabPageExtra);
      this.tabControl1.Controls.Add((Control) this.tabPageNotes);
      this.tabControl1.Controls.Add((Control) this.tabPageHistory);
      this.tabControl1.Controls.Add((Control) this.tabPageLoans);
      this.tabControl1.Controls.Add((Control) this.tabPageCustom1);
      this.tabControl1.Controls.Add((Control) this.tabPageCustom2);
      this.tabControl1.Controls.Add((Control) this.tabPageCustom3);
      this.tabControl1.Controls.Add((Control) this.tabPageCustom4);
      this.tabControl1.Controls.Add((Control) this.tabPageCustom5);
      this.tabControl1.Controls.Add((Control) this.tabPageCategory);
      this.tabControl1.Dock = DockStyle.Fill;
      this.tabControl1.Location = new Point(0, 0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(1010, 315);
      this.tabControl1.TabIndex = 0;
      this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
      this.tabPageDetail.BackColor = Color.White;
      this.tabPageDetail.Location = new Point(4, 23);
      this.tabPageDetail.Name = "tabPageDetail";
      this.tabPageDetail.Size = new Size(1002, 288);
      this.tabPageDetail.TabIndex = 0;
      this.tabPageDetail.Text = "Details";
      this.tabPageExtra.AutoScroll = true;
      this.tabPageExtra.BackColor = Color.White;
      this.tabPageExtra.Location = new Point(4, 23);
      this.tabPageExtra.Name = "tabPageExtra";
      this.tabPageExtra.Size = new Size(1002, 288);
      this.tabPageExtra.TabIndex = 8;
      this.tabPageExtra.Text = "Extra";
      this.tabPageNotes.BackColor = Color.White;
      this.tabPageNotes.Location = new Point(4, 23);
      this.tabPageNotes.Name = "tabPageNotes";
      this.tabPageNotes.Size = new Size(1002, 288);
      this.tabPageNotes.TabIndex = 1;
      this.tabPageNotes.Text = "Notes";
      this.tabPageHistory.BackColor = Color.White;
      this.tabPageHistory.Location = new Point(4, 23);
      this.tabPageHistory.Name = "tabPageHistory";
      this.tabPageHistory.Size = new Size(1002, 288);
      this.tabPageHistory.TabIndex = 2;
      this.tabPageHistory.Text = "History";
      this.tabPageLoans.BackColor = Color.White;
      this.tabPageLoans.Location = new Point(4, 23);
      this.tabPageLoans.Name = "tabPageLoans";
      this.tabPageLoans.Size = new Size(1002, 288);
      this.tabPageLoans.TabIndex = 0;
      this.tabPageLoans.Text = "Loans";
      this.tabPageCustom1.BackColor = Color.White;
      this.tabPageCustom1.Location = new Point(4, 23);
      this.tabPageCustom1.Name = "tabPageCustom1";
      this.tabPageCustom1.Size = new Size(1002, 288);
      this.tabPageCustom1.TabIndex = 3;
      this.tabPageCustom1.Text = "Custom Page1";
      this.tabPageCustom2.BackColor = Color.White;
      this.tabPageCustom2.Location = new Point(4, 23);
      this.tabPageCustom2.Name = "tabPageCustom2";
      this.tabPageCustom2.Size = new Size(1002, 288);
      this.tabPageCustom2.TabIndex = 4;
      this.tabPageCustom2.Text = "Custom Page2";
      this.tabPageCustom3.BackColor = Color.White;
      this.tabPageCustom3.Location = new Point(4, 23);
      this.tabPageCustom3.Name = "tabPageCustom3";
      this.tabPageCustom3.Size = new Size(1002, 288);
      this.tabPageCustom3.TabIndex = 5;
      this.tabPageCustom3.Text = "Custom Page3";
      this.tabPageCustom4.BackColor = Color.White;
      this.tabPageCustom4.Location = new Point(4, 23);
      this.tabPageCustom4.Name = "tabPageCustom4";
      this.tabPageCustom4.Size = new Size(1002, 288);
      this.tabPageCustom4.TabIndex = 6;
      this.tabPageCustom4.Text = "Custom Page4";
      this.tabPageCustom5.BackColor = Color.White;
      this.tabPageCustom5.Location = new Point(4, 23);
      this.tabPageCustom5.Name = "tabPageCustom5";
      this.tabPageCustom5.Size = new Size(1002, 288);
      this.tabPageCustom5.TabIndex = 7;
      this.tabPageCustom5.Text = "Custom Page5";
      this.tabPageCategory.BackColor = Color.White;
      this.tabPageCategory.Location = new Point(4, 23);
      this.tabPageCategory.Name = "tabPageCategory";
      this.tabPageCategory.Size = new Size(1002, 288);
      this.tabPageCategory.TabIndex = 9;
      this.tabPageCategory.Text = "Custom Category Fields";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScroll = true;
      this.AutoScrollMinSize = new Size(1010, 315);
      this.BackColor = Color.White;
      this.ClientSize = new Size(919, 280);
      this.Controls.Add((Control) this.tabControl1);
      this.Name = nameof (BizPartnerTabForm);
      this.Text = "BizPartTabForm";
      this.Closed += new EventHandler(this.BizPartnerTabForm_Closed);
      this.tabControl1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public BizPartnerInfo1Form ContactInfoForm => this.detailForm;

    public BizPartnerInfo2Form ContactExtraForm => this.extraForm;

    public BizPartnerNotesForm ContactNotesForm => this.noteForm;

    private void Init()
    {
      this.detailForm = new BizPartnerInfo1Form(this, this.currentSource);
      this.detailForm.TopLevel = false;
      this.detailForm.Visible = true;
      this.detailForm.Dock = DockStyle.Fill;
      this.detailForm.SummaryChanged += new EventHandler(this.page_SummaryChanged);
      this.detailForm.CategoryChangedEvent += new BizPartnerInfo1Form.CategoryChanged(this.detailForm_CategoryChangedEvent);
      this.tabPageDetail.Controls.Add((Control) this.detailForm);
      this.extraForm = new BizPartnerInfo2Form(this, this.currentSource);
      this.extraForm.TopLevel = false;
      this.extraForm.Visible = true;
      this.extraForm.Dock = DockStyle.Fill;
      this.extraForm.RequireContactRefresh += new EventHandler(this.extraForm_RequireContactRefresh);
      this.tabPageExtra.Controls.Add((Control) this.extraForm);
      this.noteForm = new BizPartnerNotesForm();
      this.noteForm.TopLevel = false;
      this.noteForm.Visible = true;
      this.noteForm.Dock = DockStyle.Fill;
      this.noteForm.ContactDeleted += new ContactDeletedEventHandler(this.contactDeletedHandler);
      this.tabPageNotes.Controls.Add((Control) this.noteForm);
      this.histForm = new BizPartnerHistoryForm();
      this.histForm.TopLevel = false;
      this.histForm.Visible = true;
      this.histForm.Dock = DockStyle.Fill;
      this.tabPageHistory.Controls.Add((Control) this.histForm);
      this.loansForm = new BizPartnerLoansForm();
      this.loansForm.TopLevel = false;
      this.loansForm.BackColor = this.contactColor;
      this.loansForm.Visible = true;
      this.loansForm.Dock = DockStyle.Fill;
      this.loansForm.IsReadOnly = this.detailForm.IsReadOnly;
      this.tabPageLoans.Controls.Add((Control) this.loansForm);
      this.dynamicTabPages[0] = this.tabPageCustom1;
      this.dynamicTabPages[1] = this.tabPageCustom2;
      this.dynamicTabPages[2] = this.tabPageCustom3;
      this.dynamicTabPages[3] = this.tabPageCustom4;
      this.dynamicTabPages[4] = this.tabPageCustom5;
      this.ctlCategoryCustomFields = new CategoryCustomFieldsControl(CustomFieldsType.BizCategoryCustom);
      this.ctlCategoryCustomFields.Dock = DockStyle.Fill;
      this.ctlCategoryCustomFields.DataChangedEvent += new EventHandler(this.page_SummaryChanged);
      this.tabPageCategory.Controls.Add((Control) this.ctlCategoryCustomFields);
      this.InitDynamicTabs();
      this.TabControl = this.tabControl1;
      this.tabControl1.SelectedTab = this.tabPageDetail;
    }

    private void page_SummaryChanged(object sender, EventArgs e)
    {
      if (this.DataChanged == null)
        return;
      this.DataChanged();
    }

    private void detailForm_CategoryChangedEvent(int categoryID)
    {
      if (this.ctlCategoryCustomFields == null)
        return;
      this.ctlCategoryCustomFields.ResetCategoryID(categoryID);
    }

    private void extraForm_RequireContactRefresh(object sender, EventArgs e)
    {
      if (this.RequireContactListRefresh == null)
        return;
      this.RequireContactListRefresh((object) null, (EventArgs) null);
    }

    public void InitDynamicTabs()
    {
      int selectedIndex = this.tabControl1.SelectedIndex;
      this.detailForm.RefreshData();
      ContactCustomFieldInfoCollection customFields = this.getCustomFields();
      ContactCustomFieldInfo[] items = customFields.Items;
      this.dynamicTabPages[0].Text = customFields.Page1Name;
      this.dynamicTabPages[1].Text = customFields.Page2Name;
      this.dynamicTabPages[2].Text = customFields.Page3Name;
      this.dynamicTabPages[3].Text = customFields.Page4Name;
      this.dynamicTabPages[4].Text = customFields.Page5Name;
      Array.Sort<ContactCustomFieldInfo>(items);
      ArrayList[] arrayListArray = new ArrayList[5]
      {
        new ArrayList(),
        new ArrayList(),
        new ArrayList(),
        new ArrayList(),
        new ArrayList()
      };
      for (int index1 = 0; index1 < items.Length; ++index1)
      {
        int index2 = (int) Math.Floor((double) (items[index1].LabelID - 1) / 20.0);
        arrayListArray[index2].Add((object) items[index1]);
      }
      foreach (TabPage dynamicTabPage in this.dynamicTabPages)
      {
        if (this.tabControl1.Controls.Contains((Control) dynamicTabPage))
          this.tabControl1.Controls.Remove((Control) dynamicTabPage);
      }
      if (this.tabControl1.Controls.Contains((Control) this.tabPageCategory))
        this.tabControl1.Controls.Remove((Control) this.tabPageCategory);
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < 5; ++index)
      {
        if (arrayListArray[index].Count > 0)
        {
          this.tabControl1.Controls.Add((Control) this.dynamicTabPages[index]);
          CustomFieldsControl customFieldsControl = new CustomFieldsControl(ContactType.BizPartner);
          customFieldsControl.CurrentContactID = -1;
          customFieldsControl.Visible = true;
          customFieldsControl.Dock = DockStyle.Fill;
          customFieldsControl.CustomFieldInfo = (ContactCustomFieldInfo[]) arrayListArray[index].ToArray(typeof (ContactCustomFieldInfo));
          customFieldsControl.DataChanged += new EventHandler(this.page_SummaryChanged);
          customFieldsControl.IsReadOnly = this.detailForm.IsReadOnly;
          arrayList.Add((object) customFieldsControl);
          this.dynamicTabPages[index].Controls.Clear();
          this.dynamicTabPages[index].Controls.Add((Control) customFieldsControl);
        }
      }
      this.customFieldsControls = (CustomFieldsControl[]) arrayList.ToArray(typeof (CustomFieldsControl));
      if (Session.EncompassEdition != EncompassEdition.Broker)
        this.tabControl1.Controls.Add((Control) this.tabPageCategory);
      this.tabControl1.SelectedIndex = selectedIndex;
    }

    private ContactCustomFieldInfoCollection getCustomFields()
    {
      return Session.ContactManager.GetCustomFieldInfo(ContactType.BizPartner);
    }

    private void BizPartnerTabForm_Closed(object sender, EventArgs e)
    {
      this.detailForm.Close();
      this.extraForm.Close();
      this.noteForm.Close();
      this.histForm.Close();
      if (this.loansForm == null)
        return;
      this.loansForm.Close();
    }

    private void contactDeletedHandler(int contactId) => this.OnContactDeleted(contactId);

    public void disableControls()
    {
      this.detailForm.IsReadOnly = true;
      this.extraForm.IsReadOnly = true;
      this.histForm.IsReadOnly = true;
      if (this.loansForm != null)
        this.loansForm.IsReadOnly = true;
      for (int index = 0; index < this.customFieldsControls.Length; ++index)
        this.customFieldsControls[index].IsReadOnly = true;
      this.ctlCategoryCustomFields.IsReadOnly = true;
    }

    public void enableControls()
    {
      this.detailForm.IsReadOnly = false;
      this.extraForm.IsReadOnly = false;
      this.noteForm.IsReadOnly = false;
      this.histForm.IsReadOnly = false;
      if (this.loansForm != null)
        this.loansForm.IsReadOnly = false;
      for (int index = 0; index < this.customFieldsControls.Length; ++index)
        this.customFieldsControls[index].IsReadOnly = false;
      this.ctlCategoryCustomFields.IsReadOnly = false;
    }

    public void ActivateNotesTab() => this.tabControl1.SelectedTab = this.tabPageNotes;

    public void ActivateDetailTab()
    {
      this.tabControl1.SelectedTab = this.tabPageDetail;
      this.detailForm.SetFocusOnDefaultField();
    }

    public void RefreshContactData() => this.parentForm.RefreshContactData();

    public void SynchronizeBizPartnerInfo(BizPartnerInfo obj)
    {
      this.detailForm.ResetBizPartnerInfo = obj;
    }

    public void RefreshContactList() => this.parentForm.RefreshContactList();

    public override int CurrentContactID
    {
      get => base.CurrentContactID;
      set
      {
        if (base.CurrentContactID == value)
          return;
        if (value >= 0)
        {
          this.CurrentContact = (object) Session.ContactManager.GetBizPartner(value);
        }
        else
        {
          this.CurrentContact = (object) null;
          this.dirty = false;
        }
      }
    }

    public override object CurrentContact
    {
      get => base.CurrentContact;
      set => base.CurrentContact = value;
    }

    private bool checkPrivateRight()
    {
      if (Session.UserInfo.IsSuperAdministrator())
        return true;
      ContactGroupInfo[] groupsForContact = Session.ContactGroupManager.GetContactGroupsForContact(this.currentSource, this.CurrentContactID);
      if ((groupsForContact == null || groupsForContact.Length == 0) && Session.UserInfo.IsTopLevelAdministrator())
        return true;
      BizGroupRef[] contactGroupRefs = Session.AclGroupManager.GetBizContactGroupRefs(Session.UserID, true);
      foreach (ContactGroupInfo contactGroupInfo in groupsForContact)
      {
        bool flag = false;
        foreach (BizGroupRef bizGroupRef in contactGroupRefs)
        {
          if (bizGroupRef.BizGroupID == contactGroupInfo.GroupId)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return false;
      }
      return true;
    }

    public override bool isDirty()
    {
      bool flag = this.dirty;
      if (!flag)
        flag = base.isDirty();
      return flag;
    }

    public override bool SaveChanges()
    {
      if (base.CurrentContactID < 0)
        return false;
      using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("BizContact.Save", "Saving a Business Contact", true, 605, nameof (SaveChanges), "D:\\ws\\24.3.0.0\\EmLite\\ContactUI\\BizPartnerTabForm.cs"))
      {
        bool flag = base.SaveChanges();
        if (flag)
          this.CurrentContact = (object) Session.ContactManager.GetBizPartner(this.CurrentContactID);
        else
          performanceMeter.Abort();
        return flag;
      }
    }

    public bool SaveChanges(bool prompt)
    {
      if (((this.isDirty() ? 1 : (this.dirty ? 1 : 0)) & (prompt ? 1 : 0)) == 0 || Utils.Dialog((IWin32Window) this, "Do you want to save your changes on Business contact?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.No)
        return this.SaveChanges();
      this.ClearChanges();
      return false;
    }

    public void TriggerSave() => this.parentForm.SaveChanges();

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.CurrentContact = this.currentContact;
    }
  }
}

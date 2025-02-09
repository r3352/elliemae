// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BorrowerTabForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BorrowerTabForm : ContactTabForm
  {
    private BorrowerInfo1Form detailForm;
    private BorrowerInfo2Form extraForm;
    private OpportunityForm opportunityForm;
    private BorrowerNotesForm noteForm;
    private BorrowerHistoryForm histForm;
    private BorrowerLoansForm loansForm;
    private ArrayList customFieldsControls;
    private ArrayList[] customFieldLists;
    private TabControl tabControl1;
    private TabPage tabPageDetail;
    private TabPage tabPageNotes;
    private TabPage tabPageHistory;
    private TabPage tabPageCustom1;
    private TabPage tabPageCustom2;
    private TabPage tabPageCustom3;
    private TabPage tabPageCustom4;
    private TabPage tabPageCustom5;
    private TabPage tabPageOpportunity;
    private TabPage tabPageExtra;
    private TabPage tabPageLoans;
    private System.ComponentModel.Container components;
    private Color backColor = Color.Transparent;
    private Color btnColor = Color.Transparent;
    private Color contactColor = Color.Transparent;
    private bool dirty;
    private bool isInitial;
    private FeaturesAclManager aclMgr;
    private TabPage[] dynamicTabPages = new TabPage[5];

    public event BorrowerSummaryChangedEventHandler SummaryChanged;

    public event EventHandler RequireContactListRefresh;

    public BorrowerTabForm(ContactGroupListController groupListController)
    {
      this.InitializeComponent();
      this.enforceSecurity();
      this.Init(groupListController);
      this.disableControls();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.tabControl1 = new TabControl();
      this.tabPageDetail = new TabPage();
      this.tabPageExtra = new TabPage();
      this.tabPageOpportunity = new TabPage();
      this.tabPageNotes = new TabPage();
      this.tabPageHistory = new TabPage();
      this.tabPageLoans = new TabPage();
      this.tabPageCustom1 = new TabPage();
      this.tabPageCustom2 = new TabPage();
      this.tabPageCustom3 = new TabPage();
      this.tabPageCustom4 = new TabPage();
      this.tabPageCustom5 = new TabPage();
      this.tabControl1.SuspendLayout();
      this.SuspendLayout();
      this.tabControl1.Controls.Add((Control) this.tabPageDetail);
      this.tabControl1.Controls.Add((Control) this.tabPageExtra);
      this.tabControl1.Controls.Add((Control) this.tabPageOpportunity);
      this.tabControl1.Controls.Add((Control) this.tabPageNotes);
      this.tabControl1.Controls.Add((Control) this.tabPageHistory);
      this.tabControl1.Controls.Add((Control) this.tabPageLoans);
      this.tabControl1.Controls.Add((Control) this.tabPageCustom1);
      this.tabControl1.Controls.Add((Control) this.tabPageCustom2);
      this.tabControl1.Controls.Add((Control) this.tabPageCustom3);
      this.tabControl1.Controls.Add((Control) this.tabPageCustom4);
      this.tabControl1.Controls.Add((Control) this.tabPageCustom5);
      this.tabControl1.Dock = DockStyle.Fill;
      this.tabControl1.Location = new Point(0, 0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(1010, 303);
      this.tabControl1.TabIndex = 0;
      this.tabPageDetail.BackColor = Color.White;
      this.tabPageDetail.Location = new Point(4, 23);
      this.tabPageDetail.Name = "tabPageDetail";
      this.tabPageDetail.Size = new Size(1002, 276);
      this.tabPageDetail.TabIndex = 0;
      this.tabPageDetail.Text = "Details";
      this.tabPageExtra.BackColor = Color.White;
      this.tabPageExtra.Location = new Point(4, 23);
      this.tabPageExtra.Name = "tabPageExtra";
      this.tabPageExtra.Size = new Size(992, 276);
      this.tabPageExtra.TabIndex = 10;
      this.tabPageExtra.Text = "Extra";
      this.tabPageOpportunity.BackColor = Color.White;
      this.tabPageOpportunity.Location = new Point(4, 23);
      this.tabPageOpportunity.Name = "tabPageOpportunity";
      this.tabPageOpportunity.Size = new Size(992, 276);
      this.tabPageOpportunity.TabIndex = 9;
      this.tabPageOpportunity.Text = "Opportunity";
      this.tabPageNotes.BackColor = Color.White;
      this.tabPageNotes.Location = new Point(4, 23);
      this.tabPageNotes.Name = "tabPageNotes";
      this.tabPageNotes.Size = new Size(992, 276);
      this.tabPageNotes.TabIndex = 2;
      this.tabPageNotes.Text = "Notes";
      this.tabPageHistory.BackColor = Color.White;
      this.tabPageHistory.Location = new Point(4, 23);
      this.tabPageHistory.Name = "tabPageHistory";
      this.tabPageHistory.Size = new Size(992, 276);
      this.tabPageHistory.TabIndex = 3;
      this.tabPageHistory.Text = "History";
      this.tabPageLoans.BackColor = Color.White;
      this.tabPageLoans.Location = new Point(4, 23);
      this.tabPageLoans.Name = "tabPageLoans";
      this.tabPageLoans.Size = new Size(992, 276);
      this.tabPageLoans.TabIndex = 0;
      this.tabPageLoans.Text = "Loans";
      this.tabPageCustom1.BackColor = Color.White;
      this.tabPageCustom1.Location = new Point(4, 23);
      this.tabPageCustom1.Name = "tabPageCustom1";
      this.tabPageCustom1.Size = new Size(992, 276);
      this.tabPageCustom1.TabIndex = 4;
      this.tabPageCustom1.Text = "Custom Page1";
      this.tabPageCustom2.BackColor = Color.White;
      this.tabPageCustom2.Location = new Point(4, 23);
      this.tabPageCustom2.Name = "tabPageCustom2";
      this.tabPageCustom2.Size = new Size(992, 276);
      this.tabPageCustom2.TabIndex = 5;
      this.tabPageCustom2.Text = "Custom Page2";
      this.tabPageCustom3.BackColor = Color.White;
      this.tabPageCustom3.Location = new Point(4, 23);
      this.tabPageCustom3.Name = "tabPageCustom3";
      this.tabPageCustom3.Size = new Size(992, 276);
      this.tabPageCustom3.TabIndex = 6;
      this.tabPageCustom3.Text = "Custom Page3";
      this.tabPageCustom4.BackColor = Color.White;
      this.tabPageCustom4.Location = new Point(4, 23);
      this.tabPageCustom4.Name = "tabPageCustom4";
      this.tabPageCustom4.Size = new Size(992, 276);
      this.tabPageCustom4.TabIndex = 7;
      this.tabPageCustom4.Text = "Custom Page4";
      this.tabPageCustom5.BackColor = Color.White;
      this.tabPageCustom5.Location = new Point(4, 23);
      this.tabPageCustom5.Name = "tabPageCustom5";
      this.tabPageCustom5.Size = new Size(992, 276);
      this.tabPageCustom5.TabIndex = 8;
      this.tabPageCustom5.Text = "Custom Page5";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScroll = true;
      this.AutoScrollMinSize = new Size(1010, 265);
      this.BackColor = Color.White;
      this.ClientSize = new Size(806, 320);
      this.Controls.Add((Control) this.tabControl1);
      this.Name = nameof (BorrowerTabForm);
      this.Closed += new EventHandler(this.BorrowerTabForm_Closed);
      this.tabControl1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void enforceSecurity()
    {
      this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Borrower_LoansTab))
        return;
      this.tabControl1.TabPages.Remove(this.tabPageLoans);
    }

    public BorrowerNotesForm ContactNotesForm => this.noteForm;

    public void ResetEmailPhone()
    {
    }

    private void Init(ContactGroupListController groupListController)
    {
      this.detailForm = new BorrowerInfo1Form(this, groupListController);
      this.detailForm.SummaryChanged += new BorrowerSummaryChangedEventHandler(this.onSummaryChanged);
      this.detailForm.TopLevel = false;
      this.detailForm.Visible = true;
      this.detailForm.IsReadOnly = true;
      this.detailForm.Dock = DockStyle.Fill;
      this.tabPageDetail.Controls.Add((Control) this.detailForm);
      this.dynamicTabPages[0] = this.tabPageCustom1;
      this.dynamicTabPages[1] = this.tabPageCustom2;
      this.dynamicTabPages[2] = this.tabPageCustom3;
      this.dynamicTabPages[3] = this.tabPageCustom4;
      this.dynamicTabPages[4] = this.tabPageCustom5;
      this.InitDynamicTabs();
      this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
      this.TabControl = this.tabControl1;
      this.tabControl1.SelectedTab = this.tabPageDetail;
    }

    private void chkPrimary_CheckedChanged(object sender, EventArgs e)
    {
      if (this.isInitial)
        return;
      this.dirty = true;
    }

    public void InitDynamicTabs()
    {
      int selectedIndex = this.tabControl1.SelectedIndex;
      if (this.extraForm != null)
        this.extraForm.RefreshData();
      ContactCustomFieldInfoCollection customFields = this.getCustomFields();
      ContactCustomFieldInfo[] items = customFields.Items;
      this.dynamicTabPages[0].Text = customFields.Page1Name;
      this.dynamicTabPages[1].Text = customFields.Page2Name;
      this.dynamicTabPages[2].Text = customFields.Page3Name;
      this.dynamicTabPages[3].Text = customFields.Page4Name;
      this.dynamicTabPages[4].Text = customFields.Page5Name;
      Array.Sort<ContactCustomFieldInfo>(items);
      this.customFieldLists = new ArrayList[5]
      {
        new ArrayList(),
        new ArrayList(),
        new ArrayList(),
        new ArrayList(),
        new ArrayList()
      };
      for (int index = 0; index < items.Length; ++index)
        this.customFieldLists[(items[index].LabelID - 1) / 20].Add((object) items[index]);
      foreach (TabPage dynamicTabPage in this.dynamicTabPages)
      {
        if (this.tabControl1.Controls.Contains((Control) dynamicTabPage))
          this.tabControl1.Controls.Remove((Control) dynamicTabPage);
      }
      this.customFieldsControls = new ArrayList();
      for (int index = 0; index < 5; ++index)
      {
        if (this.customFieldLists[index].Count > 0)
        {
          this.tabControl1.Controls.Add((Control) this.dynamicTabPages[index]);
          this.dynamicTabPages[index].Controls.Clear();
        }
      }
      if (this.backColor != Color.Transparent && this.tabControl1.SelectedTab != null && this.tabControl1.SelectedTab.Controls.Count == 0)
        this.loadTabPageControl();
      this.tabControl1.SelectedIndex = selectedIndex;
    }

    private ContactCustomFieldInfoCollection getCustomFields()
    {
      return Session.ContactManager.GetCustomFieldInfo(ContactType.Borrower);
    }

    private void BorrowerTabForm_Closed(object sender, EventArgs e)
    {
      this.detailForm.Close();
      if (this.extraForm != null)
        this.extraForm.Close();
      if (this.opportunityForm != null)
        this.opportunityForm.Close();
      if (this.noteForm != null)
        this.noteForm.Close();
      if (this.histForm != null)
        this.histForm.Close();
      if (this.loansForm == null)
        return;
      this.loansForm.Close();
    }

    public BorrowerInfo1Form getDetailsPage() => this.detailForm;

    public void AddAppointment()
    {
      this.tabControl1.SelectedTab = this.tabPageExtra;
      this.extraForm.AddAppointment();
    }

    private void contactDeletedHandler(int contactId) => this.OnContactDeleted(contactId);

    public void disableControls()
    {
      this.detailForm.IsReadOnly = true;
      if (this.extraForm != null)
        this.extraForm.IsReadOnly = true;
      if (this.opportunityForm != null)
        this.opportunityForm.IsReadOnly = true;
      if (this.histForm != null)
        this.histForm.IsReadOnly = true;
      if (this.loansForm != null)
        this.loansForm.IsReadOnly = true;
      for (int index = 0; index < this.customFieldsControls.Count; ++index)
        ((CustomFieldsControl) this.customFieldsControls[index]).IsReadOnly = true;
    }

    public void enableControls()
    {
      this.detailForm.IsReadOnly = false;
      if (this.extraForm != null)
        this.extraForm.IsReadOnly = false;
      if (this.opportunityForm != null)
        this.opportunityForm.IsReadOnly = false;
      if (this.noteForm != null)
        this.noteForm.IsReadOnly = false;
      if (this.histForm != null)
        this.histForm.IsReadOnly = false;
      if (this.loansForm != null)
        this.loansForm.IsReadOnly = false;
      for (int index = 0; index < this.customFieldsControls.Count; ++index)
        ((CustomFieldsControl) this.customFieldsControls[index]).IsReadOnly = false;
    }

    public void ActivateDetailTab()
    {
      this.tabControl1.SelectedTab = this.tabPageDetail;
      this.detailForm.SetFocusOnDefaultField();
    }

    public void ActivateNotesTab() => this.tabControl1.SelectedTab = this.tabPageNotes;

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tabControl1.SelectedTab.Controls.Count == 0)
        this.loadTabPageControl();
      else
        this.CurrentContact = this.currentContact;
    }

    private void loadTabPageControl()
    {
      Form form = (Form) null;
      UserControl userControl = (UserControl) null;
      if (this.Cursor == Cursors.Default)
        this.Cursor = Cursors.WaitCursor;
      if (this.tabControl1.SelectedTab == this.tabPageExtra)
      {
        this.extraForm = new BorrowerInfo2Form();
        this.extraForm.RequireContactRefresh += new EventHandler(this.extraForm_RequireContactRefresh);
        this.extraForm.SummaryChanged += new BorrowerSummaryChangedEventHandler(this.onSummaryChanged);
        this.extraForm.IsReadOnly = this.detailForm.IsReadOnly;
        form = (Form) this.extraForm;
      }
      else if (this.tabControl1.SelectedTab == this.tabPageLoans)
      {
        this.loansForm = new BorrowerLoansForm();
        this.loansForm.IsReadOnly = this.detailForm.IsReadOnly;
        form = (Form) this.loansForm;
      }
      else if (this.tabControl1.SelectedTab == this.tabPageOpportunity)
      {
        this.opportunityForm = new OpportunityForm();
        this.opportunityForm.SummaryChanged += new BorrowerSummaryChangedEventHandler(this.onSummaryChanged);
        this.opportunityForm.IsReadOnly = this.detailForm.IsReadOnly;
        form = (Form) this.opportunityForm;
      }
      else if (this.tabControl1.SelectedTab == this.tabPageHistory)
      {
        this.histForm = new BorrowerHistoryForm();
        this.histForm.IsReadOnly = this.detailForm.IsReadOnly;
        form = (Form) this.histForm;
      }
      else if (this.tabControl1.SelectedTab == this.tabPageNotes)
      {
        this.noteForm = new BorrowerNotesForm();
        this.noteForm.ContactDeleted += new ContactDeletedEventHandler(this.contactDeletedHandler);
        form = (Form) this.noteForm;
      }
      else
      {
        for (int index = 0; index < this.dynamicTabPages.Length; ++index)
        {
          if (this.tabControl1.SelectedTab == this.dynamicTabPages[index])
          {
            CustomFieldsControl customFieldsControl = new CustomFieldsControl(ContactType.Borrower);
            customFieldsControl.CurrentContactID = -1;
            customFieldsControl.CustomFieldInfo = (ContactCustomFieldInfo[]) this.customFieldLists[index].ToArray(typeof (ContactCustomFieldInfo));
            customFieldsControl.DataChanged += new EventHandler(this.ctlCustomFields_DataChanged);
            customFieldsControl.IsReadOnly = this.detailForm.IsReadOnly;
            this.customFieldsControls.Add((object) customFieldsControl);
            userControl = (UserControl) customFieldsControl;
            break;
          }
        }
      }
      if (form != null)
      {
        form.TopLevel = false;
        form.Visible = true;
        form.Dock = DockStyle.Fill;
        this.tabControl1.SelectedTab.Controls.Add((Control) form);
      }
      else
      {
        userControl.Visible = true;
        userControl.Dock = DockStyle.Fill;
        this.tabControl1.SelectedTab.Controls.Add((Control) userControl);
      }
      this.Cursor = Cursors.Default;
    }

    private void ctlCustomFields_DataChanged(object sender, EventArgs e) => this.onSummaryChanged();

    private void extraForm_RequireContactRefresh(object sender, EventArgs e)
    {
      if (this.RequireContactListRefresh == null)
        return;
      this.RequireContactListRefresh((object) null, (EventArgs) null);
    }

    private void onSummaryChanged()
    {
      if (this.SummaryChanged == null)
        return;
      this.SummaryChanged();
    }

    public override int CurrentContactID
    {
      get => base.CurrentContactID;
      set
      {
        if (base.CurrentContactID == value)
          return;
        this.isInitial = true;
        if (value >= 0)
        {
          this.CurrentContact = (object) Session.ContactManager.GetBorrower(value);
          this.dirty = false;
        }
        else if (value == -100 || base.CurrentContactID == -100 && value == -1)
        {
          base.CurrentContactID = value;
        }
        else
        {
          this.CurrentContact = (object) null;
          this.dirty = false;
        }
        this.isInitial = false;
      }
    }

    public override object CurrentContact
    {
      get => base.CurrentContact;
      set => base.CurrentContact = value;
    }

    public override bool isDirty()
    {
      bool flag = this.dirty;
      if (!flag)
        flag = base.isDirty();
      return flag;
    }

    public bool SaveChanges(bool prompt)
    {
      if (((this.isDirty() ? 1 : (this.dirty ? 1 : 0)) & (prompt ? 1 : 0)) == 0 || Utils.Dialog((IWin32Window) this, "Do you want to save your changes on Borrower contact?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.No)
        return this.SaveChanges();
      this.ClearChanges();
      return false;
    }

    public override bool SaveChanges()
    {
      if (base.CurrentContact == null)
        return false;
      using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("BorContact.Save", "Saving a Business Contact", true, 638, nameof (SaveChanges), "D:\\ws\\24.3.0.0\\EmLite\\ContactUI\\BorrowerTabForm.cs"))
      {
        bool flag = base.SaveChanges();
        if (flag)
          this.CurrentContact = (object) Session.ContactManager.GetBorrower(this.CurrentContactID);
        else
          performanceMeter.Abort();
        return flag;
      }
    }

    public void ForceSave(bool prompt)
    {
      this.RequireContactListRefresh((object) prompt, (EventArgs) null);
    }
  }
}

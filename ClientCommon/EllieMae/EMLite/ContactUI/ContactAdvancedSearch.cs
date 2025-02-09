// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactAdvancedSearch
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactAdvancedSearch : ContactAdvancedSearchForm, IHelp
  {
    private const string className = "ContactAdvancedSearch";
    private static string sw = Tracing.SwContact;
    protected TabControl tabControl1;
    protected TabPage tabPageLoanInfo;
    protected TabPage tabPageContactInfo;
    protected TabPage tabPageMore;
    private Label label16;
    private Button btnSaveQuery;
    private Button btnReset;
    private Button btnCancel;
    private Button btnGo;
    protected ContactType _ContactType;
    private ContactProvider contactProvider;
    private MoreSearchForm moreForm;
    private ContactSearchPage loanInfoForm;
    private ContactSearchPage marketingInfoForm;
    private Button btnSavedSearches;
    private TabPage tabPageMarketingInfo;
    private ContactSearchPage contactInfoForm;
    private Button btnOK;
    private ContactSearchContext searchContext;
    private Sessions.Session session;

    public ContactAdvancedSearch(
      Sessions.Session session,
      ContactType contactType,
      ContactSearchContext ctx)
    {
      this.session = session;
      this.InitializeComponent();
      this._ContactType = contactType;
      this.searchContext = ctx;
      this.Init();
    }

    private void initForCampaignAddCtx() => this.initForCampaignDeleteCtx();

    private void initForCampaignDeleteCtx()
    {
      this.btnOK.Visible = true;
      this.btnOK.Location = this.btnReset.Location;
      this.btnReset.Location = this.btnSaveQuery.Location;
      this.btnSavedSearches.Visible = false;
      this.btnGo.Visible = false;
      this.btnSaveQuery.Visible = false;
    }

    private void initForContactsCtx()
    {
      this.btnOK.Visible = false;
      this.btnSavedSearches.Visible = true;
      this.btnGo.Visible = true;
      this.btnSaveQuery.Visible = true;
    }

    private void initForCtx()
    {
      switch (this.searchContext)
      {
        case ContactSearchContext.Contacts:
          this.initForContactsCtx();
          break;
        case ContactSearchContext.CampaignAdd:
          this.initForCampaignAddCtx();
          break;
        case ContactSearchContext.CampaignDelete:
          this.initForCampaignDeleteCtx();
          break;
      }
    }

    protected void Init()
    {
      this.initForCtx();
      if (this._ContactType == ContactType.Borrower)
      {
        this.contactProvider = (ContactProvider) new BorrowerProvider();
        this.loanInfoForm = (ContactSearchPage) new BorSearchLoanInfoPage(this.searchContext);
        this.loanInfoForm.TopLevel = false;
        this.loanInfoForm.Visible = true;
        this.loanInfoForm.Dock = DockStyle.Fill;
        this.tabPageLoanInfo.Controls.Add((Control) this.loanInfoForm);
        this.contactInfoForm = (ContactSearchPage) new BorSearchContactInfoPage(this.session, this.searchContext);
        this.contactInfoForm.TopLevel = false;
        this.contactInfoForm.Visible = true;
        this.contactInfoForm.Dock = DockStyle.Fill;
        this.tabPageContactInfo.Controls.Add((Control) this.contactInfoForm);
        this.marketingInfoForm = (ContactSearchPage) new ContactSearchMarketingPage(this.session, ContactType.Borrower, this.searchContext);
        this.marketingInfoForm.TopLevel = false;
        this.marketingInfoForm.Visible = true;
        this.marketingInfoForm.Dock = DockStyle.Fill;
        this.tabPageMarketingInfo.Controls.Add((Control) this.marketingInfoForm);
        this.moreForm = (MoreSearchForm) new BorSearchMorePage(this.searchContext);
        this.moreForm.TopLevel = false;
        this.moreForm.Visible = true;
        this.moreForm.Dock = DockStyle.Fill;
        this.tabPageMore.Controls.Add((Control) this.moreForm);
      }
      else
      {
        this.contactProvider = (ContactProvider) new BizPartnerProvider();
        this.loanInfoForm = (ContactSearchPage) new BizSearchLoanInfoPage(this.searchContext);
        this.loanInfoForm.TopLevel = false;
        this.loanInfoForm.Visible = true;
        this.loanInfoForm.Dock = DockStyle.Fill;
        this.tabPageLoanInfo.Controls.Add((Control) this.loanInfoForm);
        this.contactInfoForm = (ContactSearchPage) new BizSearchContactInfoPage(this.searchContext);
        this.contactInfoForm.TopLevel = false;
        this.contactInfoForm.Visible = true;
        this.contactInfoForm.Dock = DockStyle.Fill;
        this.tabPageContactInfo.Controls.Add((Control) this.contactInfoForm);
        this.marketingInfoForm = (ContactSearchPage) new ContactSearchMarketingPage(this.session, this._ContactType, this.searchContext);
        this.marketingInfoForm.TopLevel = false;
        this.marketingInfoForm.Visible = true;
        this.marketingInfoForm.Dock = DockStyle.Fill;
        this.tabPageMarketingInfo.Controls.Add((Control) this.marketingInfoForm);
        this.moreForm = (MoreSearchForm) new BizSearchMorePage(this.searchContext);
        this.moreForm.TopLevel = false;
        this.moreForm.Visible = true;
        this.moreForm.Dock = DockStyle.Fill;
        this.tabPageMore.Controls.Add((Control) this.moreForm);
      }
    }

    private void InitializeComponent()
    {
      this.tabControl1 = new TabControl();
      this.tabPageLoanInfo = new TabPage();
      this.tabPageContactInfo = new TabPage();
      this.tabPageMarketingInfo = new TabPage();
      this.tabPageMore = new TabPage();
      this.label16 = new Label();
      this.btnSaveQuery = new Button();
      this.btnReset = new Button();
      this.btnCancel = new Button();
      this.btnGo = new Button();
      this.btnSavedSearches = new Button();
      this.btnOK = new Button();
      this.tabControl1.SuspendLayout();
      this.SuspendLayout();
      this.tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabControl1.Controls.Add((Control) this.tabPageLoanInfo);
      this.tabControl1.Controls.Add((Control) this.tabPageContactInfo);
      this.tabControl1.Controls.Add((Control) this.tabPageMarketingInfo);
      this.tabControl1.Controls.Add((Control) this.tabPageMore);
      this.tabControl1.Location = new Point(8, 45);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(464, 330);
      this.tabControl1.TabIndex = 2;
      this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
      this.tabPageLoanInfo.Location = new Point(4, 22);
      this.tabPageLoanInfo.Name = "tabPageLoanInfo";
      this.tabPageLoanInfo.Size = new Size(456, 304);
      this.tabPageLoanInfo.TabIndex = 0;
      this.tabPageLoanInfo.Text = "Loan History";
      this.tabPageLoanInfo.Visible = false;
      this.tabPageContactInfo.Location = new Point(4, 22);
      this.tabPageContactInfo.Name = "tabPageContactInfo";
      this.tabPageContactInfo.Size = new Size(456, 280);
      this.tabPageContactInfo.TabIndex = 1;
      this.tabPageContactInfo.Text = "Contact Info";
      this.tabPageContactInfo.Visible = false;
      this.tabPageMarketingInfo.Location = new Point(4, 22);
      this.tabPageMarketingInfo.Name = "tabPageMarketingInfo";
      this.tabPageMarketingInfo.Size = new Size(456, 280);
      this.tabPageMarketingInfo.TabIndex = 5;
      this.tabPageMarketingInfo.Text = "Marketing History";
      this.tabPageMore.Location = new Point(4, 22);
      this.tabPageMore.Name = "tabPageMore";
      this.tabPageMore.Size = new Size(456, 280);
      this.tabPageMore.TabIndex = 4;
      this.tabPageMore.Text = "Other Fields";
      this.tabPageMore.Visible = false;
      this.label16.Location = new Point(12, 16);
      this.label16.Name = "label16";
      this.label16.Size = new Size(400, 16);
      this.label16.TabIndex = 0;
      this.label16.Text = "Search by contact information, loan history or by using saved searches.";
      this.btnSaveQuery.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSaveQuery.Location = new Point(288, 384);
      this.btnSaveQuery.Name = "btnSaveQuery";
      this.btnSaveQuery.Size = new Size(88, 23);
      this.btnSaveQuery.TabIndex = 5;
      this.btnSaveQuery.Text = "Save Search";
      this.btnSaveQuery.Click += new EventHandler(this.btnSaveQuery_Click);
      this.btnReset.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnReset.Location = new Point(196, 384);
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(88, 23);
      this.btnReset.TabIndex = 4;
      this.btnReset.Text = "Clear";
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(380, 384);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(88, 23);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "Cancel";
      this.btnGo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnGo.Location = new Point(104, 384);
      this.btnGo.Name = "btnGo";
      this.btnGo.Size = new Size(88, 23);
      this.btnGo.TabIndex = 3;
      this.btnGo.Text = "Search";
      this.btnGo.Click += new EventHandler(this.btnGo_Click);
      this.btnSavedSearches.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSavedSearches.Location = new Point(372, 12);
      this.btnSavedSearches.Name = "btnSavedSearches";
      this.btnSavedSearches.Size = new Size(100, 23);
      this.btnSavedSearches.TabIndex = 1;
      this.btnSavedSearches.Text = "Saved Searches";
      this.btnSavedSearches.Click += new EventHandler(this.btnSavedSearches_Click);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(8, 384);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(88, 23);
      this.btnOK.TabIndex = 7;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.AcceptButton = (IButtonControl) this.btnGo;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(486, 415);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnSavedSearches);
      this.Controls.Add((Control) this.btnSaveQuery);
      this.Controls.Add((Control) this.btnReset);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnGo);
      this.Controls.Add((Control) this.label16);
      this.Controls.Add((Control) this.tabControl1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ContactAdvancedSearch);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Advanced Contact Search";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.tabControl1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private bool checkToAddCriterionInOtherFieldTab()
    {
      if (this.moreForm.ExistsNotAddedCriterion())
      {
        DialogResult dialogResult = Utils.Dialog((IWin32Window) this, "Do you want to add the search criterion you just entered to the search list?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
        if (dialogResult == DialogResult.Cancel || dialogResult == DialogResult.Yes && !this.moreForm.AddCriterion())
          return false;
        if (dialogResult == DialogResult.No)
          this.moreForm.ClearSearchCriterionToAdd();
      }
      return true;
    }

    private void btnGo_Click(object sender, EventArgs e)
    {
      if (!this.checkToAddCriterionInOtherFieldTab() || !this.ValidateUserInput())
        return;
      ContactQuery allSearchCriteria = this.getAllSearchCriteria();
      this.description = ClientContactSearchUtil.getSearchDescriptionHeading(allSearchCriteria, this._ContactType);
      ClientContactSearchUtil contactSearchUtil = new ClientContactSearchUtil(this.loanMatchType, this._ContactType);
      contactSearchUtil.FlushSearchObjectsToSql(allSearchCriteria, this._ContactType);
      this.criteria = contactSearchUtil.Criteria;
      this.description += contactSearchUtil.Description;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private bool ValidateUserInput()
    {
      return this.loanInfoForm.ValidateUserInput() && this.contactInfoForm.ValidateUserInput() && this.marketingInfoForm.ValidateUserInput();
    }

    public ContactQuery getAllSearchCriteria()
    {
      ContactQuery allSearchCriteria = new ContactQuery();
      ContactQuery searchCriteria1 = this.loanInfoForm.GetSearchCriteria();
      allSearchCriteria.Items = (ContactQueryItem[]) ArrayUtil.AddRange((Array) allSearchCriteria.Items, (ICollection) searchCriteria1.Items, typeof (ContactQueryItem));
      this.description = this.loanInfoForm.Description;
      this.loanMatchType = this.loanInfoForm.RelatedLoanMatchType;
      allSearchCriteria.LoanMatchType = this.loanInfoForm.RelatedLoanMatchType;
      ContactQuery searchCriteria2 = this.contactInfoForm.GetSearchCriteria();
      allSearchCriteria.Items = (ContactQueryItem[]) ArrayUtil.AddRange((Array) allSearchCriteria.Items, (ICollection) searchCriteria2.Items, typeof (ContactQueryItem));
      if (this.loanInfoForm.RelatedLoanMatchType != RelatedLoanMatchType.None && searchCriteria1.Items.Length == 0 && searchCriteria2.Items.Length + this.moreForm.MoreCriteria.Count > 0)
        this.description += " and whose ";
      ContactQuery searchCriteria3 = this.marketingInfoForm.GetSearchCriteria();
      allSearchCriteria.Items = (ContactQueryItem[]) ArrayUtil.AddRange((Array) allSearchCriteria.Items, (ICollection) searchCriteria3.Items, typeof (ContactQueryItem));
      allSearchCriteria.Items = (ContactQueryItem[]) ArrayUtil.AddRange((Array) allSearchCriteria.Items, (ICollection) this.moreForm.MoreCriteria, typeof (ContactQueryItem));
      return allSearchCriteria;
    }

    private void btnSaveQuery_Click(object sender, EventArgs e)
    {
      if (!this.checkToAddCriterionInOtherFieldTab() || !this.ValidateUserInput())
        return;
      SaveQueryForm saveQueryForm = new SaveQueryForm(this._ContactType);
      if (saveQueryForm.ShowDialog() == DialogResult.Cancel)
        return;
      ContactQuery allSearchCriteria = this.getAllSearchCriteria();
      allSearchCriteria.Name = saveQueryForm.QueryName;
      if (saveQueryForm.IsNewQuery)
        this.contactProvider.AddContactQuery(this.session.UserID, allSearchCriteria);
      else
        this.contactProvider.UpdateContactQueries(this.session.UserID, allSearchCriteria);
      int num = (int) Utils.Dialog((IWin32Window) this, "Your search has been saved.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    protected void loadDefaultQuery(
      QueryCriterion[] defaultCriteria,
      RelatedLoanMatchType defaultMatchType)
    {
      this.loanInfoForm.LoadQuery(defaultCriteria, defaultMatchType);
      this.contactInfoForm.LoadQuery(defaultCriteria, defaultMatchType);
      this.moreForm.LoadQuery(defaultCriteria, defaultMatchType);
      this.marketingInfoForm.LoadQuery(defaultCriteria, defaultMatchType);
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "All the search criteria entered will be lost. Are you sure you want to do this?", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      this.loanInfoForm.Reset();
      this.contactInfoForm.Reset();
      this.moreForm.Reset();
      this.marketingInfoForm.Reset();
    }

    private void btnSavedSearches_Click(object sender, EventArgs e)
    {
      ContactQueryForm contactQueryForm = new ContactQueryForm(this._ContactType);
      if (DialogResult.OK != contactQueryForm.ShowDialog((IWin32Window) this))
        return;
      ContactQuery queryToLoad = contactQueryForm.QueryToLoad;
      if (queryToLoad == (ContactQuery) null)
        return;
      this.loadQuery(queryToLoad);
    }

    public void loadQuery(ContactQuery query)
    {
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      ArrayList arrayList3 = new ArrayList();
      ArrayList arrayList4 = new ArrayList();
      foreach (ContactQueryItem contactQueryItem in query.Items)
      {
        if (contactQueryItem.GroupName == "MoreChoices")
          arrayList1.Add((object) contactQueryItem);
        else if (contactQueryItem.GroupName == "ContactInfo")
          arrayList2.Add((object) contactQueryItem);
        else if (contactQueryItem.GroupName == "LoanInfo")
          arrayList3.Add((object) contactQueryItem);
        else if (contactQueryItem.GroupName == "History")
          arrayList4.Add((object) contactQueryItem);
      }
      this.moreForm.MoreCriteria = arrayList1;
      this.contactInfoForm.StaticQuery = new ContactQuery()
      {
        Name = query.Name,
        OrderID = query.OrderID,
        Items = (ContactQueryItem[]) arrayList2.ToArray(typeof (ContactQueryItem))
      };
      this.loanInfoForm.StaticQuery = new ContactQuery()
      {
        Name = query.Name,
        OrderID = query.OrderID,
        Items = (ContactQueryItem[]) arrayList3.ToArray(typeof (ContactQueryItem))
      };
      this.loanInfoForm.SetLoanMatchType(query.LoanMatchType);
      this.marketingInfoForm.StaticQuery = new ContactQuery()
      {
        Name = query.Name,
        OrderID = query.OrderID,
        Items = (ContactQueryItem[]) arrayList4.ToArray(typeof (ContactQueryItem))
      };
    }

    protected void floatKeyPressHandler(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
        return;
      if (e.KeyChar != '.')
      {
        e.Handled = true;
      }
      else
      {
        if (((Control) sender).Text.IndexOf(".") < 0)
          return;
        e.Handled = true;
      }
    }

    protected void intKeyPressHandler(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tabControl1.SelectedTab == this.tabPageMore || this.checkToAddCriterionInOtherFieldTab())
        return;
      this.tabControl1.SelectedTab = this.tabPageMore;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp() => this.session.Application.DisplayHelp(nameof (ContactAdvancedSearch));

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.checkToAddCriterionInOtherFieldTab() || !this.ValidateUserInput())
        return;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
  }
}

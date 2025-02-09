// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HomeCounselingProviderForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class HomeCounselingProviderForm : 
    UserControl,
    IMainScreen,
    IWin32Window,
    IApplicationWindow,
    ISynchronizeInvoke,
    IRefreshContents,
    IOnlineHelpTarget
  {
    private const string className = "HomeCounselingProvidersForm";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private IHtmlInput iData;
    private bool readOnly;
    private LoanScreen detailScreen;
    private LoanScreen headerScreen;
    private LoanData loan;
    private Sessions.Session session;
    private List<KeyValuePair<string, string>> serviceList;
    private List<KeyValuePair<string, string>> languageList;
    private IContainer components;
    private GroupContainer groupContainerList;
    private GroupContainer groupContainerDetail;
    private GridView gridViewProviders;
    private StandardIconButton btnIconDelete;
    private StandardIconButton btnIconDown;
    private StandardIconButton btnIconUp;
    private StandardIconButton btnIconAdd;
    private Panel panelDetail;
    private Panel panelTop;
    private Panel panelAllOthers;
    private VerticalSeparator verticalSeparator1;
    private Button btnGetAgencies;
    private Panel panelExtListAndDetails;
    private Panel panelExList;
    private CollapsibleSplitter collapsibleSplitter1;
    private Panel panelExDetail;

    public HomeCounselingProviderForm(IHtmlInput iData, bool readOnly, Sessions.Session session)
    {
      this.iData = iData;
      this.readOnly = readOnly;
      this.session = session;
      this.InitializeComponent();
      if (this.session.SessionObjects.AllowConcurrentEditing && !this.session.LoanDataMgr.LockLoanWithExclusiveA(true))
        this.readOnly = true;
      this.Dock = DockStyle.Fill;
      if (this.iData is LoanData)
        this.loan = (LoanData) this.iData;
      this.initForm();
      if (this.readOnly)
        this.btnIconAdd.Enabled = this.btnIconDelete.Enabled = this.btnIconDown.Enabled = this.btnIconUp.Enabled = false;
      this.loadHeaderScreen();
      this.setButtonAccess(this.btnGetAgencies);
      if (this.loan == null)
        return;
      try
      {
        new PopupBusinessRules(this.loan, (ResourceManager) null, (Image) null, (Image) null, this.session).SetBusinessRules(this.btnGetAgencies);
      }
      catch (Exception ex)
      {
        Tracing.Log(HomeCounselingProviderForm.sw, TraceLevel.Error, "HomeCounselingProvidersForm", "Cannot set Button access right. Error: " + ex.Message);
      }
    }

    public void RefreshContents() => this.initForm();

    public void RefreshLoanContents() => this.initForm();

    private void initForm()
    {
      this.gridViewProviders.Items.Clear();
      this.gridViewProviders.SubItemCheck -= new GVSubItemEventHandler(this.gridViewProviders_SubItemCheck);
      if (this.loan != null)
      {
        int counselingProviders = this.loan.GetNumberOfHomeCounselingProviders();
        if (counselingProviders > 0)
        {
          this.gridViewProviders.BeginUpdate();
          for (int index = 0; index < counselingProviders; ++index)
            this.gridViewProviders.Items.Add(this.buildListViewItem(index + 1, false));
          this.gridViewProviders.EndUpdate();
          this.gridViewProviders.Items[0].Selected = true;
        }
      }
      this.gridViewProviders.SubItemCheck += new GVSubItemEventHandler(this.gridViewProviders_SubItemCheck);
    }

    private void loadHeaderScreen()
    {
      if (this.headerScreen == null)
      {
        this.headerScreen = new LoanScreen(this.session, !(this.iData is LoanData) || this.loan.IsInFindFieldForm ? (IWin32Window) this.ParentForm : (IWin32Window) this, (IHtmlInput) this.loan);
        this.panelTop.Controls.Add((Control) this.headerScreen);
        this.headerScreen.BrowserHandler.SetHelpTarget((IOnlineHelpTarget) this);
        this.headerScreen.LoadForm(new InputFormInfo("HomeCounselingProvidersHeader", "Home Counseling"));
        this.headerScreen.SetTitle("Home Counseling Providers");
      }
      else
      {
        this.headerScreen.Visible = true;
        this.headerScreen.BringToFront();
      }
      this.headerScreen.RefreshLoanContents();
      this.headerScreen.RefreshToolTips();
    }

    private void setButtonAccess(Button button)
    {
      button.Enabled = true;
      switch (Session.LoanDataMgr.GetFieldAccessRights("Button_HomeCounseling" + button.Text))
      {
        case BizRule.FieldAccessRight.Hide:
          button.Visible = false;
          break;
        case BizRule.FieldAccessRight.ViewOnly:
          button.Enabled = false;
          break;
      }
    }

    private void gridViewProviders_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnIconUp.Enabled = !this.readOnly && this.gridViewProviders.SelectedItems.Count == 1 && this.gridViewProviders.SelectedItems[0].Index > 0;
      this.btnIconDown.Enabled = !this.readOnly && this.gridViewProviders.SelectedItems.Count == 1 && this.gridViewProviders.SelectedItems[0].Index < this.gridViewProviders.Items.Count - 1;
      this.btnIconDelete.Enabled = !this.readOnly && this.gridViewProviders.SelectedItems.Count == 1;
      if (this.gridViewProviders.SelectedItems.Count == 0)
        return;
      if (this.detailScreen != null)
        this.detailScreen.BrowserHandler.UpdateCurrentField();
      this.openHomeCounselingProvider(this.gridViewProviders.SelectedItems[0].Index);
    }

    private void btnIconAdd_Click(object sender, EventArgs e)
    {
      int newIndex = this.loan.NewHomeCounselingProvider();
      if (newIndex <= -1)
        return;
      this.gridViewProviders.SelectedItems.Clear();
      this.gridViewProviders.Items.Add(this.buildListViewItem(newIndex, true));
      this.editHomeCounselingProvider();
    }

    private void editHomeCounselingProvider()
    {
      if (this.gridViewProviders.SelectedItems.Count == 0)
        return;
      this.openHomeCounselingProvider(this.gridViewProviders.SelectedItems[0].Index);
      this.gridViewProviders.Focus();
    }

    private void openHomeCounselingProvider(int i)
    {
      if (i < 0)
        return;
      if (this.detailScreen == null)
      {
        this.detailScreen = new LoanScreen(this.session, !(this.iData is LoanData) || this.loan.IsInFindFieldForm ? (IWin32Window) this.ParentForm : (IWin32Window) this, (IHtmlInput) this.loan);
        this.detailScreen.RemoveTitle();
        this.detailScreen.RemoveBorder();
        this.panelDetail.Controls.Add((Control) this.detailScreen);
        this.detailScreen.BrowserHandler.DocumentCompleted += new DocumentCompleteEventHandler(this.browserHandler_DocumentCompleted);
        this.detailScreen.BrowserHandler.SetHelpTarget((IOnlineHelpTarget) this);
        if (this.readOnly)
          this.detailScreen.SetFieldsReadonly();
        this.detailScreen.LoadForm(new InputFormInfo("HomeCounselingProviders", "Home Counseling Provider"));
      }
      else
      {
        this.detailScreen.Visible = true;
        this.detailScreen.BringToFront();
      }
      this.detailScreen.BrowserHandler.Property = (object) (i + 1);
      this.detailScreen.RefreshLoanContents();
      this.detailScreen.RefreshToolTips();
      if (this.readOnly)
        this.detailScreen.SetFieldsReadonly();
      this.detailScreen.BrowserHandler.SetGoToFieldFocus("HC0002", 0);
    }

    private void browserHandler_DocumentCompleted()
    {
      HOMECOUNSELINGPROVIDERSInputHandler inputHandler = (HOMECOUNSELINGPROVIDERSInputHandler) this.detailScreen.BrowserHandler.GetInputHandler();
      if (inputHandler == null)
        return;
      inputHandler.VerifSummaryChanged += new VerifSummaryChangedEventHandler(this.summaryInfoChangedHandler);
    }

    private GVItem buildListViewItem(int newIndex, bool selected)
    {
      string str = newIndex.ToString("00");
      GVItem gvItem = new GVItem(string.Concat((object) newIndex));
      gvItem.SubItems.Add((object) this.loan.GetField("HC" + str + "02"));
      gvItem.SubItems.Add((object) HOMECOUNSELINGPROVIDERSInputHandler.BuildAgencyAddress(this.loan.GetField("HC" + str + "03"), this.loan.GetField("HC" + str + "04"), this.loan.GetField("HC" + str + "05"), this.loan.GetField("HC" + str + "06")));
      gvItem.SubItems.Add((object) this.loan.GetField("HC" + str + "07"));
      gvItem.SubItems.Add(this.loan.GetField("HC" + str + "14") == "Y" ? (object) "Y" : (object) "");
      gvItem.SubItems.Add((object) this.loan.GetField("HC" + str + "16"));
      gvItem.SubItems.Add((object) this.loan.GetField("HC" + str + "12"));
      gvItem.SubItems.Add((object) this.loan.GetField("HC" + str + "13"));
      gvItem.SubItems.Add((object) this.loan.GetField("HC" + str + "17"));
      gvItem.SubItems.Add((object) this.loan.GetField("HC" + str + "99"));
      if (this.loan.GetField("HC" + str + "01") == "Y")
        gvItem.Checked = true;
      gvItem.Selected = selected;
      return gvItem;
    }

    private void summaryInfoChangedHandler(VerifSummaryChangeInfo info)
    {
      int nItemIndex1 = Utils.ParseInt(this.detailScreen.BrowserHandler.Property) - 1;
      string itemName = info.ItemName;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(itemName))
      {
        case 688847888:
          if (!(itemName == "HC0004"))
            return;
          break;
        case 705625507:
          if (!(itemName == "HC0005"))
            return;
          break;
        case 722403126:
          if (!(itemName == "HC0006"))
            return;
          break;
        case 739180745:
          if (!(itemName == "HC0007"))
            return;
          this.gridViewProviders.Items[nItemIndex1].SubItems[3].Text = info.ItemValue.ToString();
          return;
        case 739327840:
          if (!(itemName == "HC0017"))
            return;
          this.gridViewProviders.Items[nItemIndex1].SubItems[8].Text = info.ItemValue.ToString();
          return;
        case 756105459:
          if (!(itemName == "HC0016"))
            return;
          this.gridViewProviders.Items[nItemIndex1].SubItems[5].Text = info.ItemValue.ToString();
          return;
        case 789513602:
          if (!(itemName == "HC0002"))
            return;
          this.gridViewProviders.Items[nItemIndex1].SubItems[1].Text = info.ItemValue.ToString();
          return;
        case 789660697:
          if (!(itemName == "HC0014"))
            return;
          this.gridViewProviders.Items[nItemIndex1].SubItems[4].Text = info.ItemValue.ToString();
          if (info.ItemValue.ToString() == "Y")
          {
            for (int nItemIndex2 = 0; nItemIndex2 < this.gridViewProviders.Items.Count; ++nItemIndex2)
            {
              if (nItemIndex2 != nItemIndex1 && this.gridViewProviders.Items[nItemIndex2].SubItems[4].Text == "Y")
                this.gridViewProviders.Items[nItemIndex2].SubItems[4].Text = "";
            }
          }
          this.headerScreen.RefreshContents();
          return;
        case 806291221:
          if (!(itemName == "HC0003"))
            return;
          break;
        case 806438316:
          if (!(itemName == "HC0013"))
            return;
          this.gridViewProviders.Items[nItemIndex1].SubItems[7].Text = info.ItemValue.ToString();
          return;
        case 823215935:
          if (!(itemName == "HC0012"))
            return;
          this.gridViewProviders.Items[nItemIndex1].SubItems[6].Text = info.ItemValue.ToString();
          return;
        default:
          return;
      }
      this.gridViewProviders.Items[nItemIndex1].SubItems[2].Text = info.ItemValue.ToString();
    }

    public void ShowHelp(Control control)
    {
    }

    public void ShowHelp(Control control, string helpTargetName)
    {
    }

    public void ShowLeadCenter()
    {
    }

    public void ShowCalendar(
      IWin32Window owner,
      string userID,
      CSMessage.AccessLevel accessLevel,
      bool accessUpdate)
    {
    }

    public bool AllowCalendarSharing() => false;

    public void SwitchToOrgUserSetup(string userid)
    {
    }

    public void SwitchToExternalOrgUserSetup(string userid)
    {
    }

    public void NavigateHome(string url)
    {
    }

    public void NavigateToContact(CategoryType contactType)
    {
    }

    public void NavigateToContact(ContactInfo selectedContact)
    {
    }

    public void AddNewBorrowerToContactManagerList(int contactID)
    {
    }

    public void HandleCEMessage(CEMessage message)
    {
    }

    public void OpenURL(string url, string title, int width, int height)
    {
    }

    public Form OpenURL(string windowName, string url, string title, int width, int height)
    {
      return (Form) null;
    }

    public bool IsClientEnabledToExportFNMFRE => false;

    public bool IsUnderwriterSummaryAccessibleForBroker => false;

    public void RefreshCE()
    {
    }

    public void NavigateToTradesTab(int tradeId)
    {
    }

    private void btnIconDelete_Click(object sender, EventArgs e)
    {
      if (this.gridViewProviders.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select a provider first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete this Home Counseling Provider?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
          return;
        int index = this.gridViewProviders.SelectedItems[0].Index;
        if (!this.loan.RemoveHomeCounselingProviderAt(index))
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "This Home Counseling Provider can't be deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          this.gridViewProviders.Items.RemoveAt(index);
          this.gridViewProviders.SelectedItems.Clear();
          this.loan.SetSelectedHomeCounselingProvider(index + 1, false);
          this.headerScreen.RefreshContents();
          if (this.gridViewProviders.Items.Count == 0)
          {
            this.detailScreen.Visible = false;
          }
          else
          {
            this.gridViewProviders.BeginUpdate();
            for (int nItemIndex = index; nItemIndex < this.gridViewProviders.Items.Count; ++nItemIndex)
              this.gridViewProviders.Items[nItemIndex].Text = string.Concat((object) (this.gridViewProviders.Items[nItemIndex].Index + 1));
            this.gridViewProviders.EndUpdate();
            if (index >= this.gridViewProviders.Items.Count)
              this.gridViewProviders.Items[this.gridViewProviders.Items.Count - 1].Selected = true;
            else
              this.gridViewProviders.Items[index].Selected = true;
          }
        }
      }
    }

    private void btnIconDown_Click(object sender, EventArgs e)
    {
      if (this.gridViewProviders.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select a provider first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (!this.loan.IsTemplate && !this.loan.IsInFindFieldForm && this.session.LoanDataMgr.Writable && this.session.SessionObjects.AllowConcurrentEditing && !this.session.LoanDataMgr.LockLoanWithExclusiveA(true))
          return;
        int index = this.gridViewProviders.SelectedItems[0].Index;
        if (index >= this.gridViewProviders.Items.Count - 1)
          return;
        if (!this.loan.DownHomeCounselingProvider(index))
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "This Home Counseling Provider can't be moved down.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          this.gridViewProviders.SubItemCheck -= new GVSubItemEventHandler(this.gridViewProviders_SubItemCheck);
          string empty = string.Empty;
          for (int nItemIndex = 1; nItemIndex < this.gridViewProviders.Columns.Count; ++nItemIndex)
          {
            string text = this.gridViewProviders.Items[index].SubItems[nItemIndex].Text;
            this.gridViewProviders.Items[index].SubItems[nItemIndex].Text = this.gridViewProviders.Items[index + 1].SubItems[nItemIndex].Text;
            this.gridViewProviders.Items[index + 1].SubItems[nItemIndex].Text = text;
          }
          bool flag = this.gridViewProviders.Items[index + 1].SubItems[0].Checked;
          this.gridViewProviders.Items[index + 1].SubItems[0].Checked = this.gridViewProviders.Items[index].SubItems[0].Checked;
          this.gridViewProviders.Items[index].SubItems[0].Checked = flag;
          this.gridViewProviders.SelectedItems.Clear();
          this.gridViewProviders.Items[index + 1].Selected = true;
          this.gridViewProviders.SubItemCheck += new GVSubItemEventHandler(this.gridViewProviders_SubItemCheck);
        }
      }
    }

    private void btnIconUp_Click(object sender, EventArgs e)
    {
      if (this.gridViewProviders.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select a provider first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (!this.loan.IsTemplate && !this.loan.IsInFindFieldForm && this.session.LoanDataMgr.Writable && this.session.SessionObjects.AllowConcurrentEditing && !this.session.LoanDataMgr.LockLoanWithExclusiveA(true))
          return;
        int index = this.gridViewProviders.SelectedItems[0].Index;
        if (index == 0)
          return;
        if (!this.loan.UpHomeCounselingProvider(index))
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "This Home Counseling Provider can't be moved up.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          string empty = string.Empty;
          for (int nItemIndex = 1; nItemIndex < this.gridViewProviders.Columns.Count; ++nItemIndex)
          {
            string text = this.gridViewProviders.Items[index].SubItems[nItemIndex].Text;
            this.gridViewProviders.Items[index].SubItems[nItemIndex].Text = this.gridViewProviders.Items[index - 1].SubItems[nItemIndex].Text;
            this.gridViewProviders.Items[index - 1].SubItems[nItemIndex].Text = text;
          }
          this.gridViewProviders.SubItemCheck -= new GVSubItemEventHandler(this.gridViewProviders_SubItemCheck);
          bool flag = this.gridViewProviders.Items[index - 1].SubItems[0].Checked;
          this.gridViewProviders.Items[index - 1].SubItems[0].Checked = this.gridViewProviders.Items[index].SubItems[0].Checked;
          this.gridViewProviders.Items[index].SubItems[0].Checked = flag;
          this.gridViewProviders.SelectedItems.Clear();
          this.gridViewProviders.Items[index - 1].Selected = true;
          this.gridViewProviders.SubItemCheck += new GVSubItemEventHandler(this.gridViewProviders_SubItemCheck);
        }
      }
    }

    public string GetHelpTargetName() => "Home Counseling Providers";

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
    }

    private void Form_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    private void gridViewProviders_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.gridViewProviders.SubItemCheck -= new GVSubItemEventHandler(this.gridViewProviders_SubItemCheck);
      this.loan.SetField("HC" + (e.SubItem.Item.Index + 1).ToString("00") + "01", e.SubItem.Checked ? "Y" : "");
      this.gridViewProviders.SubItemCheck += new GVSubItemEventHandler(this.gridViewProviders_SubItemCheck);
    }

    private void btnGetAgencies_Click(object sender, EventArgs e)
    {
      if (!this.updateLanguageServiceCodes())
        return;
      List<string> existingAgencyIDs = new List<string>();
      int counselingProviders = this.loan.GetNumberOfHomeCounselingProviders();
      if (counselingProviders > 0)
      {
        for (int index = 0; index < counselingProviders; ++index)
          existingAgencyIDs.Add(this.loan.GetField("HC" + (index + 1).ToString("00") + "99"));
      }
      if (this.serviceList == null || this.languageList == null)
        return;
      using (GetAgenciesForm getAgenciesForm = new GetAgenciesForm(this.session, (IHtmlInput) this.loan, this.serviceList, this.languageList, existingAgencyIDs))
      {
        if (getAgenciesForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        string[] importedAgency = getAgenciesForm.ImportedAgencies[0];
        string[] values = (string[]) null;
        for (int index1 = 1; index1 < getAgenciesForm.ImportedAgencies.Count; ++index1)
        {
          int num = -1;
          values = getAgenciesForm.ImportedAgencies[index1];
          if (existingAgencyIDs != null && existingAgencyIDs.Count > 0)
          {
            for (int j = 0; j < importedAgency.Length; ++j)
            {
              if (importedAgency[j] == "agcid")
              {
                num = existingAgencyIDs.FindIndex((Predicate<string>) (x => x == values[j]));
                if (num >= 0)
                {
                  ++num;
                  break;
                }
                break;
              }
            }
          }
          if (num == -1)
            num = this.loan.NewHomeCounselingProvider();
          for (int index2 = 0; index2 < importedAgency.Length; ++index2)
          {
            switch (importedAgency[index2])
            {
              case "adr1":
                this.loan.SetField("HC" + num.ToString("00") + "03", values[index2]);
                break;
              case "agcid":
                this.loan.SetField("HC" + num.ToString("00") + "99", values[index2]);
                break;
              case "city":
                this.loan.SetField("HC" + num.ToString("00") + "04", values[index2]);
                break;
              case "distance":
                this.loan.SetField("HC" + num.ToString("00") + "17", values[index2]);
                break;
              case "email":
                this.loan.SetField("HC" + num.ToString("00") + "10", values[index2]);
                break;
              case "fax":
                this.loan.SetField("HC" + num.ToString("00") + "09", values[index2]);
                break;
              case "languages":
                this.loan.SetField("HC" + num.ToString("00") + "12", values[index2]);
                break;
              case "nme":
                this.loan.SetField("HC" + num.ToString("00") + "02", values[index2]);
                break;
              case "phone1":
                this.loan.SetField("HC" + num.ToString("00") + "07", values[index2]);
                break;
              case "phone2":
                this.loan.SetField("HC" + num.ToString("00") + "08", values[index2]);
                break;
              case "services":
                this.loan.SetField("HC" + num.ToString("00") + "13", values[index2]);
                break;
              case "statecd":
                this.loan.SetField("HC" + num.ToString("00") + "05", values[index2]);
                break;
              case "weburl":
                this.loan.SetField("HC" + num.ToString("00") + "11", values[index2]);
                break;
              case "zipcd":
                this.loan.SetField("HC" + num.ToString("00") + "06", values[index2]);
                break;
            }
          }
          this.loan.SetField("HC" + num.ToString("00") + "16", "CFPB Import");
          this.loan.SetField("HC" + num.ToString("00") + "01", "Y");
        }
        this.RefreshContents();
      }
    }

    private void HomeCounselingProviderForm_Resize(object sender, EventArgs e)
    {
      if (this.Parent == null)
        return;
      if (this.Parent.Height > 560)
      {
        this.panelTop.Height = 305;
        this.panelExDetail.Height = (int) byte.MaxValue;
        if (this.panelExList.Height >= 125)
          return;
        this.panelTop.Height = 180;
        this.panelExList.Height = 125;
        this.panelExDetail.Height = this.Parent.Height - this.panelTop.Height - this.panelExList.Height;
      }
      else
      {
        this.panelTop.Height = (int) ((double) this.Parent.Height * 0.35);
        this.panelExList.Height = (int) ((double) this.Parent.Height * 0.35);
        this.panelExDetail.Height = (int) ((double) this.Parent.Height * 0.3);
      }
    }

    private bool updateLanguageServiceCodes()
    {
      if ((this.session.ServerTime.Date - Utils.ParseDate((object) this.session.ConfigurationManager.GetCompanySetting("HomeCounseling", "LastUpdateDate")).Date).TotalDays <= 2.0)
      {
        if (this.serviceList != null && this.serviceList.Count > 0 && this.languageList != null && this.languageList.Count > 0)
          return true;
        List<KeyValuePair<string, string>>[] languageSupported = this.session.ConfigurationManager.GetHomeCounselingServiceLanguageSupported();
        if (languageSupported != null && languageSupported.Length == 2)
        {
          this.serviceList = languageSupported[0];
          this.languageList = languageSupported[1];
        }
        if (this.serviceList != null && this.serviceList.Count > 0 && this.languageList != null && this.languageList.Count > 0)
          return true;
      }
      if ((this.session.ServerTime - Utils.ParseDate((object) this.session.ConfigurationManager.GetCompanySetting("HomeCounseling", "UpdateLocked"))).TotalMinutes < 3.0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Encompass is updating the Service and Language tables. Please try it later.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      Cursor.Current = Cursors.WaitCursor;
      this.session.ConfigurationManager.SetCompanySetting("HomeCounseling", "UpdateLocked", this.session.ServerTime.ToString("MM/dd/yyyy hh:mm:ss tt"));
      bool flag = false;
      if (this.languageList == null || this.languageList.Count == 0)
      {
        flag = true;
        List<List<string[]>> strArrayListList = (List<List<string[]>>) null;
        try
        {
          strArrayListList = this.session.SessionObjects.ParseHomeCounselingResults(this.session.SessionObjects.GetHomeCounseling(HomeCounselingProviderAPI.HudWebURL + "getLanguages", (IWin32Window) this.session.MainForm));
        }
        catch (Exception ex)
        {
          Tracing.Log(HomeCounselingProviderForm.sw, TraceLevel.Error, "HomeCounselingProvidersForm", "Cannot get latest language supported list from URL '" + HomeCounselingProviderAPI.HudWebURL + "getLanguages'. Error: " + ex.Message);
        }
        if (strArrayListList != null && strArrayListList.Count > 0)
        {
          this.languageList = new List<KeyValuePair<string, string>>();
          string key = (string) null;
          string str = (string) null;
          foreach (List<string[]> strArrayList in strArrayListList)
          {
            foreach (string[] strArray in strArrayList)
            {
              if (string.Compare(strArray[0], "key", true) == 0)
                key = strArray[1];
              else if (string.Compare(strArray[0], "value", true) == 0)
                str = strArray[1];
            }
            if (!string.IsNullOrEmpty(key) || !string.IsNullOrEmpty(str))
              this.languageList.Add(new KeyValuePair<string, string>(key, str));
          }
        }
      }
      if (this.serviceList == null || this.serviceList.Count == 0)
      {
        flag = true;
        List<List<string[]>> strArrayListList = (List<List<string[]>>) null;
        try
        {
          strArrayListList = this.session.SessionObjects.ParseHomeCounselingResults(this.session.SessionObjects.GetHomeCounseling(HomeCounselingProviderAPI.HudWebURL + "getServices", (IWin32Window) this.session.MainForm));
        }
        catch (Exception ex)
        {
          Tracing.Log(HomeCounselingProviderForm.sw, TraceLevel.Error, "HomeCounselingProvidersForm", "Cannot get latest service supported list from URL '" + HomeCounselingProviderAPI.HudWebURL + "getServices'. Error: " + ex.Message);
        }
        if (strArrayListList != null && strArrayListList.Count > 0)
        {
          this.serviceList = new List<KeyValuePair<string, string>>();
          string key = (string) null;
          string str = (string) null;
          foreach (List<string[]> strArrayList in strArrayListList)
          {
            foreach (string[] strArray in strArrayList)
            {
              if (string.Compare(strArray[0], "key", true) == 0)
                key = strArray[1];
              else if (string.Compare(strArray[0], "value", true) == 0)
                str = strArray[1];
            }
            if (!string.IsNullOrEmpty(key) || !string.IsNullOrEmpty(str))
              this.serviceList.Add(new KeyValuePair<string, string>(key, str));
          }
        }
      }
      try
      {
        if (flag)
          this.session.ConfigurationManager.UpdateHomeCounselingCodes(this.serviceList, this.languageList);
        IConfigurationManager configurationManager = this.session.ConfigurationManager;
        DateTime dateTime = this.session.ServerTime;
        dateTime = dateTime.Date;
        string str = dateTime.ToString("MM/dd/yyyy");
        configurationManager.SetCompanySetting("HomeCounseling", "LastUpdateDate", str);
        this.session.ConfigurationManager.SetCompanySetting("HomeCounseling", "UpdateLocked", "");
      }
      catch (Exception ex)
      {
        Tracing.Log(HomeCounselingProviderForm.sw, TraceLevel.Error, "HomeCounselingProvidersForm", "Cannot update Home Counseling Language/Service Supported. Error: " + ex.Message);
      }
      Cursor.Current = Cursors.Default;
      return true;
    }

    public void DisplayTPOCompanySetting(ExternalOriginatorManagementData o)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      this.panelTop = new Panel();
      this.panelAllOthers = new Panel();
      this.panelExtListAndDetails = new Panel();
      this.panelExList = new Panel();
      this.groupContainerList = new GroupContainer();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnGetAgencies = new Button();
      this.btnIconDelete = new StandardIconButton();
      this.btnIconDown = new StandardIconButton();
      this.btnIconUp = new StandardIconButton();
      this.btnIconAdd = new StandardIconButton();
      this.gridViewProviders = new GridView();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.panelExDetail = new Panel();
      this.groupContainerDetail = new GroupContainer();
      this.panelDetail = new Panel();
      this.panelAllOthers.SuspendLayout();
      this.panelExtListAndDetails.SuspendLayout();
      this.panelExList.SuspendLayout();
      this.groupContainerList.SuspendLayout();
      ((ISupportInitialize) this.btnIconDelete).BeginInit();
      ((ISupportInitialize) this.btnIconDown).BeginInit();
      ((ISupportInitialize) this.btnIconUp).BeginInit();
      ((ISupportInitialize) this.btnIconAdd).BeginInit();
      this.panelExDetail.SuspendLayout();
      this.groupContainerDetail.SuspendLayout();
      this.SuspendLayout();
      this.panelTop.BackColor = Color.WhiteSmoke;
      this.panelTop.Dock = DockStyle.Top;
      this.panelTop.Location = new Point(0, 0);
      this.panelTop.Margin = new Padding(4);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(933, 338);
      this.panelTop.TabIndex = 5;
      this.panelAllOthers.Controls.Add((Control) this.panelExtListAndDetails);
      this.panelAllOthers.Dock = DockStyle.Fill;
      this.panelAllOthers.Location = new Point(0, 338);
      this.panelAllOthers.Margin = new Padding(4);
      this.panelAllOthers.Name = "panelAllOthers";
      this.panelAllOthers.Size = new Size(933, 647);
      this.panelAllOthers.TabIndex = 6;
      this.panelExtListAndDetails.Controls.Add((Control) this.panelExList);
      this.panelExtListAndDetails.Controls.Add((Control) this.collapsibleSplitter1);
      this.panelExtListAndDetails.Controls.Add((Control) this.panelExDetail);
      this.panelExtListAndDetails.Dock = DockStyle.Fill;
      this.panelExtListAndDetails.Location = new Point(0, 0);
      this.panelExtListAndDetails.Margin = new Padding(4);
      this.panelExtListAndDetails.Name = "panelExtListAndDetails";
      this.panelExtListAndDetails.Size = new Size(933, 647);
      this.panelExtListAndDetails.TabIndex = 7;
      this.panelExList.Controls.Add((Control) this.groupContainerList);
      this.panelExList.Dock = DockStyle.Fill;
      this.panelExList.Location = new Point(0, 0);
      this.panelExList.Margin = new Padding(4);
      this.panelExList.Name = "panelExList";
      this.panelExList.Size = new Size(933, 353);
      this.panelExList.TabIndex = 2;
      this.groupContainerList.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainerList.Controls.Add((Control) this.verticalSeparator1);
      this.groupContainerList.Controls.Add((Control) this.btnGetAgencies);
      this.groupContainerList.Controls.Add((Control) this.btnIconDelete);
      this.groupContainerList.Controls.Add((Control) this.btnIconDown);
      this.groupContainerList.Controls.Add((Control) this.btnIconUp);
      this.groupContainerList.Controls.Add((Control) this.btnIconAdd);
      this.groupContainerList.Controls.Add((Control) this.gridViewProviders);
      this.groupContainerList.Dock = DockStyle.Fill;
      this.groupContainerList.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerList.Location = new Point(0, 0);
      this.groupContainerList.Margin = new Padding(4);
      this.groupContainerList.Name = "groupContainerList";
      this.groupContainerList.Size = new Size(933, 353);
      this.groupContainerList.TabIndex = 0;
      this.groupContainerList.Text = "Home Counseling Provider List";
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(800, 6);
      this.verticalSeparator1.Margin = new Padding(4);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 8;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnGetAgencies.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnGetAgencies.Location = new Point(807, 1);
      this.btnGetAgencies.Margin = new Padding(4);
      this.btnGetAgencies.Name = "btnGetAgencies";
      this.btnGetAgencies.Size = new Size(121, 26);
      this.btnGetAgencies.TabIndex = 7;
      this.btnGetAgencies.Text = "Get Agencies";
      this.btnGetAgencies.UseVisualStyleBackColor = true;
      this.btnGetAgencies.Click += new EventHandler(this.btnGetAgencies_Click);
      this.btnIconDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnIconDelete.BackColor = Color.Transparent;
      this.btnIconDelete.Location = new Point(771, 5);
      this.btnIconDelete.Margin = new Padding(4);
      this.btnIconDelete.MouseDownImage = (Image) null;
      this.btnIconDelete.Name = "btnIconDelete";
      this.btnIconDelete.Size = new Size(21, 20);
      this.btnIconDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnIconDelete.TabIndex = 4;
      this.btnIconDelete.TabStop = false;
      this.btnIconDelete.Click += new EventHandler(this.btnIconDelete_Click);
      this.btnIconDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnIconDown.BackColor = Color.Transparent;
      this.btnIconDown.Location = new Point(740, 5);
      this.btnIconDown.Margin = new Padding(4);
      this.btnIconDown.MouseDownImage = (Image) null;
      this.btnIconDown.Name = "btnIconDown";
      this.btnIconDown.Size = new Size(21, 20);
      this.btnIconDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnIconDown.TabIndex = 3;
      this.btnIconDown.TabStop = false;
      this.btnIconDown.Click += new EventHandler(this.btnIconDown_Click);
      this.btnIconUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnIconUp.BackColor = Color.Transparent;
      this.btnIconUp.Location = new Point(709, 5);
      this.btnIconUp.Margin = new Padding(4);
      this.btnIconUp.MouseDownImage = (Image) null;
      this.btnIconUp.Name = "btnIconUp";
      this.btnIconUp.Size = new Size(21, 20);
      this.btnIconUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnIconUp.TabIndex = 2;
      this.btnIconUp.TabStop = false;
      this.btnIconUp.Click += new EventHandler(this.btnIconUp_Click);
      this.btnIconAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnIconAdd.BackColor = Color.Transparent;
      this.btnIconAdd.Location = new Point(679, 5);
      this.btnIconAdd.Margin = new Padding(4);
      this.btnIconAdd.MouseDownImage = (Image) null;
      this.btnIconAdd.Name = "btnIconAdd";
      this.btnIconAdd.Size = new Size(21, 20);
      this.btnIconAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnIconAdd.TabIndex = 1;
      this.btnIconAdd.TabStop = false;
      this.btnIconAdd.Click += new EventHandler(this.btnIconAdd_Click);
      this.gridViewProviders.AllowMultiselect = false;
      this.gridViewProviders.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnSelected";
      gvColumn1.Text = "Selected";
      gvColumn1.Width = 55;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnAgencyName";
      gvColumn2.Text = "Agency Name";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnAddress";
      gvColumn3.Text = "Agency Address";
      gvColumn3.Width = 200;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnDirectPhone";
      gvColumn4.Text = "Agency Direct Phone";
      gvColumn4.Width = 120;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnAgencyUsed";
      gvColumn5.Text = "Agency Used";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColumnSource";
      gvColumn6.Text = "Source";
      gvColumn6.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ColumnLanguages";
      gvColumn7.Text = "Languages Supported";
      gvColumn7.Width = 160;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "ColumnServices";
      gvColumn8.Text = "Counseling Services Provided";
      gvColumn8.Width = 160;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "ColumnDistance";
      gvColumn9.Text = "Distance";
      gvColumn9.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn9.Width = 70;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "ColumnAgencyID";
      gvColumn10.Text = "Agency ID";
      gvColumn10.Width = 100;
      this.gridViewProviders.Columns.AddRange(new GVColumn[10]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10
      });
      this.gridViewProviders.Dock = DockStyle.Fill;
      this.gridViewProviders.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewProviders.Location = new Point(1, 25);
      this.gridViewProviders.Margin = new Padding(4);
      this.gridViewProviders.Name = "gridViewProviders";
      this.gridViewProviders.Size = new Size(931, 327);
      this.gridViewProviders.SortOption = GVSortOption.None;
      this.gridViewProviders.TabIndex = 0;
      this.gridViewProviders.SelectedIndexChanged += new EventHandler(this.gridViewProviders_SelectedIndexChanged);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.panelExDetail;
      this.collapsibleSplitter1.Dock = DockStyle.Bottom;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(0, 353);
      this.collapsibleSplitter1.Margin = new Padding(4);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 1;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.panelExDetail.Controls.Add((Control) this.groupContainerDetail);
      this.panelExDetail.Dock = DockStyle.Bottom;
      this.panelExDetail.Location = new Point(0, 360);
      this.panelExDetail.Margin = new Padding(4);
      this.panelExDetail.Name = "panelExDetail";
      this.panelExDetail.Size = new Size(933, 287);
      this.panelExDetail.TabIndex = 0;
      this.groupContainerDetail.Controls.Add((Control) this.panelDetail);
      this.groupContainerDetail.Dock = DockStyle.Fill;
      this.groupContainerDetail.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerDetail.Location = new Point(0, 0);
      this.groupContainerDetail.Margin = new Padding(4);
      this.groupContainerDetail.Name = "groupContainerDetail";
      this.groupContainerDetail.Size = new Size(933, 287);
      this.groupContainerDetail.TabIndex = 0;
      this.groupContainerDetail.Text = "Home Counseling Provider";
      this.panelDetail.Dock = DockStyle.Fill;
      this.panelDetail.Location = new Point(1, 26);
      this.panelDetail.Margin = new Padding(4);
      this.panelDetail.Name = "panelDetail";
      this.panelDetail.Size = new Size(931, 260);
      this.panelDetail.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panelAllOthers);
      this.Controls.Add((Control) this.panelTop);
      this.Margin = new Padding(4);
      this.Name = nameof (HomeCounselingProviderForm);
      this.Size = new Size(933, 985);
      this.KeyUp += new KeyEventHandler(this.Form_KeyUp);
      this.Resize += new EventHandler(this.HomeCounselingProviderForm_Resize);
      this.panelAllOthers.ResumeLayout(false);
      this.panelExtListAndDetails.ResumeLayout(false);
      this.panelExList.ResumeLayout(false);
      this.groupContainerList.ResumeLayout(false);
      ((ISupportInitialize) this.btnIconDelete).EndInit();
      ((ISupportInitialize) this.btnIconDown).EndInit();
      ((ISupportInitialize) this.btnIconUp).EndInit();
      ((ISupportInitialize) this.btnIconAdd).EndInit();
      this.panelExDetail.ResumeLayout(false);
      this.groupContainerDetail.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}

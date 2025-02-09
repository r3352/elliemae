// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TabLinksControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class TabLinksControl : UserControl, IOnlineHelpTarget, IRefreshContents
  {
    private LoanData loan;
    private LoanScreen loanScreen;
    private InputFormInfo currentForm;
    private InputFormInfo formWithFocus;
    private AdditionalFieldsForm additionalEditor;
    private LockExtensionRequestForm lockExtensionReqForm;
    private TabLinksControl.PageMode pageMode;
    private Sessions.Session session;
    private bool initialLoading = true;
    private QuickLinksControl navUSDAControl;
    private QuickLinkLabel currentUSDARuralPage;
    private string currentFormName;
    private IContainer components;
    private TabControl tabControlForm;
    private TabPage tabPage1;
    private TabPage tabPage2;
    private TabPage tabPage3;
    private TabPage tabPage4;
    private TabPage tabPage5;
    private TabPage tabPage6;
    private Label labelTitle;
    private GroupContainer groupContainer1;
    private Panel panelLinks;
    private StandardIconButton iconReset;
    private ToolTip toolTip1;

    public string CurrentFormName => this.currentFormName;

    public TabLinksControl(
      Sessions.Session session,
      InputFormInfo currentForm,
      IWin32Window parentWindow,
      LoanData loan)
      : this(session, currentForm, parentWindow, loan, true)
    {
    }

    public TabLinksControl(
      Sessions.Session session,
      InputFormInfo currentForm,
      IWin32Window parentWindow,
      LoanData loan,
      bool load1stScreen)
    {
      this.session = session;
      this.loan = loan;
      this.currentForm = currentForm;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      if (currentForm.FormID == "LOLOCKREQUEST")
      {
        this.pageMode = TabLinksControl.PageMode.LockRequestForm;
        this.iconReset.Visible = true;
      }
      else if (currentForm.FormID == "HMDA:FHAPROCESSMGT" || currentForm.FormID == "FHAPROCESSMGT" || currentForm.FormID.StartsWith("FPMS_"))
      {
        this.pageMode = TabLinksControl.PageMode.FHAProcessManagement;
        List<QuickLink> quickLinkList = new List<QuickLink>();
        if (this.loan.Use2020URLA)
        {
          quickLinkList.Add(new QuickLink("1003 URLA P1", "D1003_2020P1", 710, 600));
          quickLinkList.Add(new QuickLink("1003 URLA P2", "D1003_2020P2", 710, 600));
          quickLinkList.Add(new QuickLink("1003 URLA Lender", "D1003_2020P5", 710, 600));
        }
        else
        {
          quickLinkList.Add(new QuickLink("1003 P1", "D10031", 610, 600));
          quickLinkList.Add(new QuickLink("1003 P2", "D10032", 610, 600));
          quickLinkList.Add(new QuickLink("1003 P3", "D10033", 650, 600));
        }
        if (this.loan.Use2010RESPA)
        {
          quickLinkList.Add(new QuickLink("Reg-Z", "REGZ50", 760, 600));
          quickLinkList.Add(new QuickLink("Itemization", "REGZGFE_2010", 850, 600));
        }
        else if (this.loan.Use2015RESPA)
        {
          quickLinkList.Add(new QuickLink("REGZ-LE", "REGZLE", 760, 600));
          quickLinkList.Add(new QuickLink("Itemization", "REGZGFE_2015", 850, 600));
        }
        else
          quickLinkList.Add(new QuickLink("GFE", "REGZGFE", 760, 600));
        quickLinkList.Add(new QuickLink("HUD 92900 LT", "HUD92900LT", 650, 600));
        QuickLinksControl quickLinksControl = new QuickLinksControl(this.loan, quickLinkList.ToArray(), this.session);
        this.panelLinks.Controls.Add((Control) quickLinksControl);
        quickLinksControl.Dock = DockStyle.Right;
        quickLinksControl.QuickLinkClicked += new EventHandler(this.navControl_QuickLinkClicked);
      }
      else if (currentForm.FormID == "HMDA:USDAManagement" || currentForm.FormID == "USDAManagement" || currentForm.FormID.StartsWith("USDA_"))
      {
        this.pageMode = TabLinksControl.PageMode.USDAManagement;
        this.navUSDAControl = new QuickLinksControl(this.loan, new List<QuickLink>()
        {
          new QuickLink("Page 1", "USDA_RURAL1"),
          new QuickLink("Page 2", "USDA_RURAL2"),
          new QuickLink("Page 3", "USDA_RURAL3"),
          new QuickLink("Page 4", "USDA_RURAL4"),
          new QuickLink("Page 5", "USDA_RURAL5"),
          new QuickLink("Page 6", "USDA_RURAL6"),
          new QuickLink("Income Worksheet", "USDA_RURALINCOME")
        }.ToArray(), this.session);
        this.navUSDAControl.Dock = DockStyle.Right;
        this.navUSDAControl.QuickLinkClicked += new EventHandler(this.navUSDAControl_QuickLinkClicked);
      }
      else if (currentForm.FormID.StartsWith("ULDD"))
        this.pageMode = TabLinksControl.PageMode.ULDD;
      else if (currentForm.FormID.StartsWith("ECSDATAVIEWER") || currentForm.FormID.StartsWith("COMPLIANCEREVIEWRESULT"))
        this.pageMode = TabLinksControl.PageMode.ECSDataViewer;
      else if (currentForm.FormID == "FHAPROCESSMGT" || currentForm.FormID.StartsWith("FPMS_") || currentForm.FormID.StartsWith("VAManagement") || currentForm.FormID.StartsWith("VATool_") || currentForm.FormID.StartsWith("ATRManagement") || currentForm.FormID.StartsWith("ATR_") || currentForm.FormID == "CONSTRUCTIONMANAGEMENT" || currentForm.FormID.StartsWith("CONSTRUCTIONMANAGEMENT_") || currentForm.FormID == "CONSTRUCTIONMANAGEMENT:LinkedLoans" || currentForm.FormID.StartsWith("HMDA2018_"))
      {
        this.pageMode = currentForm.FormID.StartsWith("FHAPROCESSMGT") || currentForm.FormID.StartsWith("FPMS_") ? TabLinksControl.PageMode.FHAProcessManagement : (currentForm.FormID.StartsWith("VAManagement") || currentForm.FormID.StartsWith("VATool_") ? TabLinksControl.PageMode.VAManagement : (currentForm.FormID.StartsWith("ATRManagement") || currentForm.FormID.StartsWith("ATR_") ? TabLinksControl.PageMode.ATRManagement : (!currentForm.FormID.StartsWith("HMDA2018_") ? TabLinksControl.PageMode.ConstructionManagement : TabLinksControl.PageMode.HMDA2018)));
        List<QuickLink> quickLinkList = new List<QuickLink>();
        if (currentForm.FormID == "CONSTRUCTIONMANAGEMENT" || currentForm.FormID.StartsWith("CONSTRUCTIONMANAGEMENT_") || currentForm.FormID == "CONSTRUCTIONMANAGEMENT:LinkedLoans")
        {
          quickLinkList.Add(new QuickLink("LE1", "LOANESTIMATEPAGE1", 800, 600));
          quickLinkList.Add(new QuickLink("LE2", "LOANESTIMATEPAGE2", 800, 600));
          quickLinkList.Add(new QuickLink("LE3", "LOANESTIMATEPAGE3", 750, 600));
          quickLinkList.Add(new QuickLink("CD1", "ClosingDisclosurePage1", 800, 600));
          quickLinkList.Add(new QuickLink("CD2", "ClosingDisclosurePage2", 850, 600));
          quickLinkList.Add(new QuickLink("CD3", "ClosingDisclosurePage3", 970, 600));
          quickLinkList.Add(new QuickLink("CD4", "ClosingDisclosurePage4", 800, 600));
          quickLinkList.Add(new QuickLink("CD5", "ClosingDisclosurePage5", 750, 600));
        }
        if (!this.loan.Use2020URLA)
        {
          quickLinkList.Add(new QuickLink("1003 P1", "D10031", 610, 600));
          quickLinkList.Add(new QuickLink("1003 P2", "D10032", 610, 600));
          quickLinkList.Add(new QuickLink("1003 P3", "D10033", 650, 600));
        }
        else if (currentForm.FormID == "CONSTRUCTIONMANAGEMENT" || currentForm.FormID.StartsWith("CONSTRUCTIONMANAGEMENT_") || currentForm.FormID == "CONSTRUCTIONMANAGEMENT:LinkedLoans")
        {
          quickLinkList.Add(new QuickLink("1003 URLA P1", "D1003_2020P1", 710, 600));
          quickLinkList.Add(new QuickLink("1003 URLA P2", "D1003_2020P2", 710, 600));
          quickLinkList.Add(new QuickLink("1003 URLA Lender", "D1003_2020P5", 710, 600));
        }
        else
        {
          quickLinkList.Add(new QuickLink("1003 URLA P1", "D1003_2020P1", 710, 600));
          quickLinkList.Add(new QuickLink("1003 URLA P2", "D1003_2020P2", 710, 600));
          quickLinkList.Add(new QuickLink("1003 URLA P3", "D1003_2020P3", 710, 600));
          quickLinkList.Add(new QuickLink("1003 URLA P4", "D1003_2020P4", 710, 600));
          quickLinkList.Add(new QuickLink("1003 URLA Lender", "D1003_2020P5", 710, 600));
        }
        if (this.loan.Use2010RESPA)
        {
          quickLinkList.Add(new QuickLink("Reg-Z", "REGZ50", 760, 600));
          quickLinkList.Add(new QuickLink("Itemization", "REGZGFE_2010", 850, 600));
        }
        else if (this.loan.Use2015RESPA)
        {
          if (currentForm.FormID == "CONSTRUCTIONMANAGEMENT" || currentForm.FormID.StartsWith("CONSTRUCTIONMANAGEMENT_") || currentForm.FormID == "CONSTRUCTIONMANAGEMENT:LinkedLoans")
          {
            quickLinkList.Add(new QuickLink("Itemization", "REGZGFE_2015", 850, 600));
            quickLinkList.Add(new QuickLink("REGZ-LE", "REGZLE", 760, 600));
          }
          else
          {
            quickLinkList.Add(new QuickLink("REGZ-LE", "REGZLE", 760, 600));
            quickLinkList.Add(new QuickLink("Itemization", "REGZGFE_2015", 850, 600));
          }
        }
        else
          quickLinkList.Add(new QuickLink("GFE", "REGZGFE", 760, 600));
        if (currentForm.FormID.StartsWith("VAManagement") || currentForm.FormID.StartsWith("VATool_"))
        {
          quickLinkList.Add(new QuickLink("VAELIG", "VAELIG", 610, 600));
          quickLinkList.Add(new QuickLink("HUD 1003 Addendum", "GVTADM", 610, 600));
        }
        else if (currentForm.FormID == "FHAPROCESSMGT" || currentForm.FormID.StartsWith("FPMS_"))
        {
          quickLinkList.Add(new QuickLink("HUD 92900 LT", "HUD92900LT", 650, 600));
          quickLinkList.Add(new QuickLink("203K WS", "MAX23K", 610, 600));
        }
        QuickLinksControl quickLinksControl = new QuickLinksControl(this.loan, quickLinkList.ToArray(), this.session);
        this.panelLinks.Controls.Add((Control) quickLinksControl);
        quickLinksControl.Dock = DockStyle.Right;
        quickLinksControl.QuickLinkClicked += new EventHandler(this.navControl_QuickLinkClicked);
      }
      else if (currentForm.FormID.StartsWith("CORRESPONDENTPURCHASEADVICE"))
        this.pageMode = TabLinksControl.PageMode.CorrespondentPurchaseAdvice;
      else if (currentForm.FormID == "HMDA_DENIAL04")
      {
        this.pageMode = TabLinksControl.PageMode.HMDA2018;
        QuickLinksControl quickLinksControl = new QuickLinksControl(this.loan, new List<QuickLink>()
        {
          new QuickLink("AUS Tracking", "AUSTracking", 950, 600),
          new QuickLink("FHA Management", "HMDA:FHAPROCESSMGT", 790, 600),
          new QuickLink("USDA Management", "HMDA:USDAManagement", 710, 600)
        }.ToArray(), this.session);
        this.panelLinks.Controls.Add((Control) quickLinksControl);
        quickLinksControl.Dock = DockStyle.Right;
        quickLinksControl.QuickLinkClicked += new EventHandler(this.navControl_QuickLinkClicked);
      }
      else
        this.pageMode = currentForm.FormID.StartsWith("HELOCManagement") || currentForm.FormID.StartsWith("HELOCTool_") ? TabLinksControl.PageMode.HELOCManagement : TabLinksControl.PageMode.Default;
      this.loanScreen = new LoanScreen(this.session, parentWindow, (IHtmlInput) loan);
      this.loanScreen.HideFormTitle();
      this.loanScreen.RemoveBorder();
      this.loanScreen.OnFieldChanged += new EventHandler(this.loanScreen_OnFieldChanged);
      this.loanScreen.ButtonClicked += new EventHandler(this.loanScreen_OnButtonClicked);
      this.refreshTabPages();
      if (load1stScreen)
      {
        if (this.currentForm.FormID == "CONSTRUCTIONMANAGEMENT:LinkedLoans")
          this.tabControlForm.SelectedIndex = 3;
        else if (this.currentForm.FormID.StartsWith("CONSTRUCTIONMANAGEMENT"))
          this.tabControlForm.SelectedIndex = 1;
        else if (this.currentForm.FormID == "HMDA:FHAPROCESSMGT" && this.tabControlForm.TabPages.Count >= 4 && this.tabControlForm.TabPages[3].Text == "Tracking")
          this.tabControlForm.SelectedIndex = 3;
        else if (this.currentForm.FormID == "HMDA:USDAManagement" && this.tabControlForm.TabPages.Count >= 3 && this.tabControlForm.TabPages[3].Text == "Tracking")
          this.tabControlForm.SelectedIndex = 3;
        this.tabControlForm_SelectedIndexChanged((object) null, (EventArgs) null);
      }
      this.tabControlForm.SizeChanged += new EventHandler(this.tabControlForm_SizeChanged);
      this.initialLoading = false;
    }

    public void Unload()
    {
      if (this.navUSDAControl != null)
        this.navUSDAControl.QuickLinkClicked -= new EventHandler(this.navUSDAControl_QuickLinkClicked);
      if (this.loanScreen != null)
      {
        this.loanScreen.Unload();
        this.loanScreen.OnFieldChanged -= new EventHandler(this.loanScreen_OnFieldChanged);
        this.loanScreen.ButtonClicked -= new EventHandler(this.loanScreen_OnButtonClicked);
      }
      if (this.tabControlForm != null)
      {
        this.tabControlForm.Resize -= new EventHandler(this.tabControlForm_SizeChanged);
        this.tabControlForm.SelectedIndexChanged -= new EventHandler(this.tabControlForm_SelectedIndexChanged);
      }
      this.iconReset.Click -= new EventHandler(this.iconReset_Click);
      if (this.panelLinks == null)
        return;
      foreach (Control control in (ArrangedElementCollection) this.panelLinks.Controls)
      {
        if (control is QuickLinksControl)
          ((QuickLinksControl) control).QuickLinkClicked -= new EventHandler(this.navControl_QuickLinkClicked);
      }
    }

    public new void Dispose()
    {
      this.Unload();
      if (this.navUSDAControl != null)
        this.navUSDAControl.Dispose();
      if (this.loanScreen != null)
        this.loanScreen.Dispose();
      if (this.tabControlForm != null)
        this.tabControlForm.Dispose();
      if (this.panelLinks != null)
      {
        foreach (Component control in (ArrangedElementCollection) this.panelLinks.Controls)
          control.Dispose();
        this.panelLinks.Dispose();
      }
      base.Dispose();
    }

    private void tabControlForm_SizeChanged(object sender, EventArgs e)
    {
      if (this.initialLoading || this.formWithFocus == (InputFormInfo) null)
        return;
      InputFormInfo formInfo = new InputFormInfo(this.formWithFocus.FormID, this.formWithFocus.MnemonicName);
      this.formWithFocus = (InputFormInfo) null;
      this.reloadForm(formInfo);
    }

    private void navUSDAControl_QuickLinkClicked(object sender, EventArgs e)
    {
      QuickLinkLabel quickLinkLabel = (QuickLinkLabel) sender;
      this.currentUSDARuralPage = quickLinkLabel;
      this.reloadForm(new InputFormInfo(quickLinkLabel.QuickLink.AccessFormID, "Rural Assistance URLA"));
      string upper = quickLinkLabel.QuickLink.AccessFormID.ToUpper();
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(upper))
      {
        case 3938773864:
          if (!(upper == "USDA_RURAL1"))
            return;
          this.navUSDAControl.SetSelectedForm("USDA_RURAL1");
          break;
        case 3972329102:
          if (!(upper == "USDA_RURAL3"))
            return;
          this.navUSDAControl.SetSelectedForm("USDA_RURAL3");
          break;
        case 3989106721:
          if (!(upper == "USDA_RURAL2"))
            return;
          this.navUSDAControl.SetSelectedForm("USDA_RURAL2");
          break;
        case 4005884340:
          if (!(upper == "USDA_RURAL5"))
            return;
          this.navUSDAControl.SetSelectedForm("USDA_RURAL5");
          break;
        case 4022661959:
          if (!(upper == "USDA_RURAL4"))
            return;
          this.navUSDAControl.SetSelectedForm("USDA_RURAL4");
          break;
        case 4056217197:
          if (!(upper == "USDA_RURAL6"))
            return;
          this.navUSDAControl.SetSelectedForm("USDA_RURAL6");
          break;
        case 4147410050:
          if (!(upper == "USDA_RURALINCOME"))
            return;
          this.navUSDAControl.SetSelectedForm("USDA_RURALINCOME");
          break;
        default:
          return;
      }
      this.tabControlForm.SelectedTab.Controls.Add((Control) this.loanScreen);
    }

    private void navControl_QuickLinkClicked(object sender, EventArgs e)
    {
      this.loanScreen_OnFieldChanged((object) "19", new EventArgs());
      this.loanScreen.RefreshContents();
    }

    private void loanScreen_OnButtonClicked(object sender, EventArgs e)
    {
      if (this.pageMode != TabLinksControl.PageMode.ConstructionManagement || !(this.formWithFocus != (InputFormInfo) null) || !(this.formWithFocus.FormID == "CONSTRUCTIONMANAGEMENT_LinkConstPerm"))
        return;
      string str = (string) sender;
      if (str != "newlink" && str != "linktoloan" && str != "removelink" && str != "maketocurrent")
        return;
      this.formWithFocus = (InputFormInfo) null;
      this.reloadForm(new InputFormInfo("CONSTRUCTIONMANAGEMENT_LinkConstPerm", "Linked Loans"));
    }

    private void loanScreen_OnFieldChanged(object sender, EventArgs e)
    {
      if (this.pageMode == TabLinksControl.PageMode.FHAProcessManagement)
      {
        if ((string) sender != "19" || !(this.formWithFocus.FormID == "FPMS_Purchase") && !(this.formWithFocus.FormID == "FPMS_Refi"))
          return;
        this.reloadForm(new InputFormInfo(this.getFormID("Prequalification"), "Prequalification"));
      }
      else if (this.pageMode == TabLinksControl.PageMode.ConstructionManagement)
      {
        string str = (string) sender;
        if (str != "19" && str != "4084")
          return;
        if (this.loan == null || this.loan.LinkSyncType == LinkSyncType.ConstructionPrimary || this.loan.LinkSyncType == LinkSyncType.ConstructionLinked || this.loan.LinkSyncType == LinkSyncType.None && this.loan.GetSimpleField("19") == "ConstructionOnly" && this.loan.GetSimpleField("4084") == "Y")
        {
          if (this.tabControlForm.TabPages.Count >= 4)
            return;
          this.tabPage4.Text = "Linked Loans";
          this.tabControlForm.TabPages.Add(this.tabPage4);
        }
        else
        {
          if (this.tabControlForm.TabPages.Count <= 3)
            return;
          this.tabControlForm.TabPages.Remove(this.tabPage4);
        }
      }
      else
      {
        if (this.pageMode != TabLinksControl.PageMode.HMDA2018 || !((string) sender == "3312") || this.loan == null)
          return;
        List<string> stringList = this.resetHMDA2018FormTabs();
        if (stringList.Count == this.tabControlForm.TabPages.Count + 1)
        {
          this.tabPage2.Text = "Repurchased Loans";
          this.tabControlForm.TabPages.Add(this.tabPage2);
        }
        else
        {
          if (stringList.Count + 1 != this.tabControlForm.TabPages.Count)
            return;
          this.tabControlForm.TabPages.Remove(this.tabPage2);
        }
      }
    }

    private void refreshTabPages()
    {
      List<string> stringList = new List<string>();
      if (this.pageMode == TabLinksControl.PageMode.LockRequestForm)
      {
        stringList.Add("(Re) Lock Request");
        if (this.loan.Settings.FieldSettings.LockRequestAdditionalFields.GetFields(true).Length != 0)
          stringList.Add("Additional Fields");
      }
      else if (this.pageMode == TabLinksControl.PageMode.ULDD)
        stringList.AddRange((IEnumerable<string>) new string[3]
        {
          "Fannie Mae",
          "Freddie Mac",
          "Ginnie Mae"
        });
      else if (this.pageMode == TabLinksControl.PageMode.ECSDataViewer)
        stringList.AddRange((IEnumerable<string>) new string[2]
        {
          "Encompass Loan Data",
          "Compliance Review Result"
        });
      else if (this.pageMode == TabLinksControl.PageMode.USDAManagement)
      {
        stringList.AddRange((IEnumerable<string>) new string[4]
        {
          "Rural Assistance URLA",
          "Req for SFH Loan Guarantee/Resv. of Funds",
          "Loan Closing Report",
          "Tracking"
        });
        stringList.AddRange((IEnumerable<string>) new string[2]
        {
          "Fannie Mae",
          "Freddie Mac"
        });
      }
      else if (this.pageMode == TabLinksControl.PageMode.VAManagement)
        stringList.AddRange((IEnumerable<string>) new string[4]
        {
          "Basic Information",
          "Qualification",
          "Cash-Out Refinance",
          "Tracking"
        });
      else if (this.pageMode == TabLinksControl.PageMode.HELOCManagement)
        stringList.AddRange((IEnumerable<string>) new string[2]
        {
          "HELOC Program",
          "Important Terms and Agreement Language"
        });
      else if (this.pageMode == TabLinksControl.PageMode.ATRManagement)
      {
        if (this.loan != null && this.loan.IsTemplate)
          stringList.AddRange((IEnumerable<string>) new string[4]
          {
            "Basic Info",
            "Qualification",
            "ATR/QM Eligibility",
            "Non-Standard to Standard Refi."
          });
        else
          stringList.AddRange((IEnumerable<string>) new string[4]
          {
            "Basic Info",
            "Qualification",
            "ATR/QM Eligibility",
            "Non-Standard to Standard Refi."
          });
      }
      else if (this.pageMode == TabLinksControl.PageMode.CorrespondentPurchaseAdvice)
      {
        Hashtable settingsFromCache = Session.SessionObjects.GetCompanySettingsFromCache("POLICIES");
        if (string.Equals(settingsFromCache[(object) "ENABLEPAYMENTHISTORYANDCALC"] as string, "true", StringComparison.CurrentCultureIgnoreCase) && string.Equals(settingsFromCache[(object) "ENABLEESCROWDETAILSANDCALC"] as string, "true", StringComparison.CurrentCultureIgnoreCase))
          stringList.AddRange((IEnumerable<string>) new string[4]
          {
            "Purchase Advice",
            "Payment History",
            "Escrow Details",
            "Warehouse Bank Details"
          });
        else if (string.Equals(settingsFromCache[(object) "ENABLEPAYMENTHISTORYANDCALC"] as string, "true", StringComparison.CurrentCultureIgnoreCase) && string.Equals(settingsFromCache[(object) "ENABLEESCROWDETAILSANDCALC"] as string, "false", StringComparison.CurrentCultureIgnoreCase))
          stringList.AddRange((IEnumerable<string>) new string[3]
          {
            "Purchase Advice",
            "Payment History",
            "Warehouse Bank Details"
          });
        else if (string.Equals(settingsFromCache[(object) "ENABLEPAYMENTHISTORYANDCALC"] as string, "false", StringComparison.CurrentCultureIgnoreCase) && string.Equals(settingsFromCache[(object) "ENABLEESCROWDETAILSANDCALC"] as string, "true", StringComparison.CurrentCultureIgnoreCase))
          stringList.AddRange((IEnumerable<string>) new string[3]
          {
            "Purchase Advice",
            "Escrow Details",
            "Warehouse Bank Details"
          });
        else
          stringList.AddRange((IEnumerable<string>) new string[2]
          {
            "Purchase Advice",
            "Warehouse Bank Details"
          });
      }
      else if (this.pageMode == TabLinksControl.PageMode.ConstructionManagement)
      {
        if (this.loan == null || this.loan.LinkSyncType == LinkSyncType.ConstructionPrimary || this.loan.LinkSyncType == LinkSyncType.ConstructionLinked || this.loan.LinkSyncType == LinkSyncType.None && this.loan.GetSimpleField("19") == "ConstructionOnly" && this.loan.GetSimpleField("4084") == "Y")
          stringList.AddRange((IEnumerable<string>) new string[4]
          {
            "Basic Info",
            "Loan Info",
            "Project Data",
            "Linked Loans"
          });
        else
          stringList.AddRange((IEnumerable<string>) new string[3]
          {
            "Basic Info",
            "Loan Info",
            "Project Data"
          });
      }
      else if (this.pageMode == TabLinksControl.PageMode.HMDA2018)
      {
        if (this.loan != null)
          stringList = this.resetHMDA2018FormTabs();
        else
          stringList.AddRange((IEnumerable<string>) new string[1]
          {
            "2018 HMDA Originated/Adverse Action Loans"
          });
      }
      else if (this.session.UserID == "admin" || this.session == null || this.session.LoanDataMgr == null)
        stringList.AddRange((IEnumerable<string>) new string[5]
        {
          "Basic Info",
          "Prequalification",
          "FHA 203k",
          "Informed Consumer Choice Disclosure Notice",
          "Tracking"
        });
      else if (this.session.LoanDataMgr.InputFormSettings.IsAccessible("MAX23K"))
        stringList.AddRange((IEnumerable<string>) new string[5]
        {
          "Basic Info",
          "Prequalification",
          "FHA 203k",
          "Informed Consumer Choice Disclosure Notice",
          "Tracking"
        });
      else
        stringList.AddRange((IEnumerable<string>) new string[3]
        {
          "Basic Info",
          "Prequalification",
          "Tracking"
        });
      int num1 = 0;
      foreach (TabPage tabPage in this.tabControlForm.TabPages)
      {
        if (num1 < stringList.Count)
          tabPage.Text = stringList[num1++];
      }
      for (int index = this.tabControlForm.TabPages.Count - 1; index >= num1; --index)
        this.tabControlForm.TabPages.RemoveAt(index);
      this.tabControlForm.TabPages[0].Controls.Add((Control) this.loanScreen);
      if (this.pageMode != TabLinksControl.PageMode.LockRequestForm || this.loan == null)
        return;
      int index1 = 1;
      if (stringList.Contains("Additional Fields"))
      {
        if (this.additionalEditor == null)
        {
          if (this.loan != null && !this.loan.IsInFindFieldForm)
          {
            this.additionalEditor = new AdditionalFieldsForm(this.session, this.loan, this.loan.Settings.FieldSettings.LockRequestAdditionalFields.GetFields(true));
          }
          else
          {
            LRAdditionalFields additionalFields = this.session.ConfigurationManager.GetLRAdditionalFields();
            if (additionalFields != null && additionalFields.GetFields(true).Length != 0)
              this.additionalEditor = new AdditionalFieldsForm(this.session, this.loan, additionalFields.GetFields(true));
          }
        }
        this.tabControlForm.TabPages[index1].Controls.Add((Control) this.additionalEditor);
        ++index1;
      }
      if (!stringList.Contains("Extension Request"))
        return;
      LockExtensionUtils settings = new LockExtensionUtils(this.session.SessionObjects, this.loan);
      int currentExtNumber = this.loan.GetLogList().GetCurrentExtNumber((Hashtable) null);
      if (settings.IsCompanyControlledOccur && settings.AdjOccurrence != null && currentExtNumber + 1 > settings.AdjOccurrence.Length || settings.IfExceedNumExtensionsLimit(currentExtNumber + 1))
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "This request exceeds the max number of extensions allowed. Lock has not been extended.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (this.lockExtensionReqForm == null)
          this.lockExtensionReqForm = new LockExtensionRequestForm(this.session, this.loan, settings, currentExtNumber + 1);
        this.tabControlForm.TabPages[index1].Controls.Add((Control) this.lockExtensionReqForm);
      }
    }

    private List<string> resetHMDA2018FormTabs()
    {
      List<string> stringList = new List<string>();
      LoanStatusMap.LoanStatus loanStatusEnum = LoanStatusMap.GetLoanStatusEnum(this.loan.GetField("1393"));
      DateTime date = Utils.ParseDate((object) this.loan.GetField("3312"));
      if (loanStatusEnum == LoanStatusMap.LoanStatus.LoanPurchased && date == DateTime.MinValue)
        stringList.AddRange((IEnumerable<string>) new string[1]
        {
          "2018 HMDA Purchased Loans"
        });
      else if (loanStatusEnum == LoanStatusMap.LoanStatus.LoanPurchased && date != DateTime.MinValue)
        stringList.AddRange((IEnumerable<string>) new string[2]
        {
          "2018 HMDA Purchased Loans",
          "Repurchased Loans"
        });
      else if (loanStatusEnum != LoanStatusMap.LoanStatus.LoanPurchased && date == DateTime.MinValue)
        stringList.AddRange((IEnumerable<string>) new string[1]
        {
          "2018 HMDA Originated/Adverse Action Loans"
        });
      else if (loanStatusEnum != LoanStatusMap.LoanStatus.LoanPurchased && date != DateTime.MinValue)
        stringList.AddRange((IEnumerable<string>) new string[2]
        {
          "2018 HMDA Originated/Adverse Action Loans",
          "Repurchased Loans"
        });
      else
        stringList.AddRange((IEnumerable<string>) new string[1]
        {
          "2018 HMDA Originated/Adverse Action Loans"
        });
      return stringList;
    }

    public void LoadFormAndSetGoToField(InputFormInfo formInfo, string fieldGoTo, int countGoTo)
    {
      if (this.formWithFocus != (InputFormInfo) null && formInfo.FormID == this.formWithFocus.FormID)
      {
        this.loanScreen.RefreshContents();
      }
      else
      {
        if (this.currentForm.FormID.StartsWith("ULDD"))
          this.labelTitle.Text = "ULDD/PDD";
        else if (this.currentForm.FormID.StartsWith("ECSDATA") || this.currentForm.FormID.StartsWith("COMPLIANCEREVIEW"))
          this.labelTitle.Text = "ECS Data Viewer";
        else if (this.currentForm.FormID.StartsWith("FPMS_"))
          this.labelTitle.Text = "FHA Management";
        else if (this.currentForm.FormID.StartsWith("VATool_"))
          this.labelTitle.Text = "VA Management";
        else if (this.currentForm.FormID.StartsWith("HELOCTool_"))
          this.labelTitle.Text = "HELOC Management";
        else if (this.currentForm.FormID.StartsWith("ATR_"))
          this.labelTitle.Text = "ATR/QM Management";
        else if (this.currentForm.FormID.StartsWith("CONSTRUCTIONMANAGEMENT_"))
          this.labelTitle.Text = "Construction Management";
        else if (this.currentForm.FormID.StartsWith("HMDA2018_"))
          this.labelTitle.Text = "HMDA Information";
        else
          this.labelTitle.Text = this.currentForm.Name;
        if (formInfo.FormID.StartsWith("USDA_RURAL"))
        {
          this.loanScreen.ShowFormTitle();
          this.loanScreen.ShowBorder();
          this.loanScreen.SetTitle("Rural Assistance URLA", (Control) this.navUSDAControl);
          switch (formInfo.FormID)
          {
            case "USDA_RURAL1":
              this.navUSDAControl_QuickLinkClicked((object) new QuickLinkLabel(new QuickLink("Page 1", "USDA_RURAL1")), (EventArgs) null);
              break;
            case "USDA_RURAL2":
              this.navUSDAControl_QuickLinkClicked((object) new QuickLinkLabel(new QuickLink("Page 2", "USDA_RURAL2")), (EventArgs) null);
              break;
            case "USDA_RURAL3":
              this.navUSDAControl_QuickLinkClicked((object) new QuickLinkLabel(new QuickLink("Page 3", "USDA_RURAL3")), (EventArgs) null);
              break;
            case "USDA_RURAL4":
              this.navUSDAControl_QuickLinkClicked((object) new QuickLinkLabel(new QuickLink("Page 4", "USDA_RURAL4")), (EventArgs) null);
              break;
            case "USDA_RURAL5":
              this.navUSDAControl_QuickLinkClicked((object) new QuickLinkLabel(new QuickLink("Page 5", "USDA_RURAL5")), (EventArgs) null);
              break;
            case "USDA_RURAL6":
              this.navUSDAControl_QuickLinkClicked((object) new QuickLinkLabel(new QuickLink("Page 6", "USDA_RURAL6")), (EventArgs) null);
              break;
            case "USDA_RURALIncome":
              this.navUSDAControl_QuickLinkClicked((object) new QuickLinkLabel(new QuickLink("Income Worksheet", "USDA_RURALIncome")), (EventArgs) null);
              break;
          }
        }
        else
          this.loanScreen.LoadForm(formInfo);
        this.formWithFocus = formInfo;
        foreach (TabPage tabPage in this.tabControlForm.TabPages)
        {
          if (string.Compare(tabPage.Text, formInfo.Name, true) == 0)
          {
            this.tabControlForm.SelectedTab = tabPage;
            break;
          }
        }
        this.tabControlForm.SelectedTab.Controls.Add((Control) this.loanScreen);
        if (!(fieldGoTo != string.Empty))
          return;
        this.loanScreen.SetGoToFieldFocus(fieldGoTo, countGoTo);
      }
    }

    private void reloadForm(InputFormInfo formInfo)
    {
      if (this.formWithFocus != (InputFormInfo) null && formInfo.FormID == this.formWithFocus.FormID)
      {
        this.loanScreen.RefreshContents();
      }
      else
      {
        if (this.currentForm.FormID.StartsWith("ULDD"))
          this.labelTitle.Text = "ULDD/PDD";
        else if (this.currentForm.FormID.StartsWith("ECSDATA"))
          this.labelTitle.Text = "ECS Data Viewer";
        else if (this.currentForm.FormID.StartsWith("FPMS_"))
          this.labelTitle.Text = "FHA Management";
        else if (this.currentForm.FormID.StartsWith("VATool_"))
          this.labelTitle.Text = "VA Management";
        else if (this.currentForm.FormID.StartsWith("HELOCTool_"))
          this.labelTitle.Text = "HELOC Management";
        else if (this.currentForm.FormID.StartsWith("ATR_"))
          this.labelTitle.Text = "ATR/QM Management";
        else if (this.currentForm.FormID.StartsWith("CONSTRUCTIONMANAGEMENT_"))
          this.labelTitle.Text = "Construction Management";
        else if (this.currentForm.FormID.StartsWith("HMDA2018_"))
          this.labelTitle.Text = "HMDA Information";
        else
          this.labelTitle.Text = this.currentForm.Name;
        this.currentFormName = formInfo.FormID;
        this.loanScreen.LoadForm(formInfo);
        this.formWithFocus = formInfo;
      }
    }

    public static bool UseTabLinks(Sessions.Session session, InputFormInfo f)
    {
      return TabLinksControl.UseTabLinks(session, f, (LoanData) null);
    }

    public static bool UseTabLinks(Sessions.Session session, InputFormInfo f, LoanData loanData)
    {
      switch (f.FormID)
      {
        case "ATRManagement":
        case "CONSTRUCTIONMANAGEMENT":
        case "CONSTRUCTIONMANAGEMENT:LinkedLoans":
        case "CORRESPONDENTPURCHASEADVICE":
        case "ECSDATAVIEWER":
        case "FHAPROCESSMGT":
        case "HELOCManagement":
        case "HMDA:FHAPROCESSMGT":
        case "HMDA:USDAManagement":
        case "ULDD":
        case "USDAManagement":
        case "VAManagement":
          return true;
        case "HMDA_DENIAL04":
          if ((loanData != null ? (loanData.GetField("HMDA.X27") == string.Empty ? 0 : (int) short.Parse(loanData.GetField("HMDA.X27"), NumberStyles.AllowThousands)) : DateTime.Today.Year) >= 2018 && !loanData.IsTemplate)
            return true;
          break;
        case "LOLOCKREQUEST":
          if (loanData != null)
          {
            bool flag = loanData.Settings.FieldSettings.LockRequestAdditionalFields.GetFields(true).Length != 0;
            if (!flag)
              flag = loanData.GetLogList().GetCurrentLockRequest() != null;
            return flag;
          }
          LRAdditionalFields additionalFields = session.ConfigurationManager.GetLRAdditionalFields();
          if (additionalFields != null && additionalFields.GetFields(true).Length != 0)
            return true;
          break;
      }
      return false;
    }

    private void tabControlForm_SelectedIndexChanged(object sender, EventArgs e)
    {
      string text = this.tabControlForm.SelectedTab.Text;
      if (this.getFormID(text) == string.Empty)
        return;
      if (this.pageMode == TabLinksControl.PageMode.USDAManagement && text == "Rural Assistance URLA" && this.currentUSDARuralPage != null)
      {
        this.navUSDAControl_QuickLinkClicked((object) this.currentUSDARuralPage, new EventArgs());
      }
      else
      {
        this.reloadForm(new InputFormInfo(this.getFormID(text), text));
        if (this.pageMode == TabLinksControl.PageMode.USDAManagement && text == "Rural Assistance URLA")
          this.navUSDAControl.SetSelectedForm("USDA_RURAL1");
        this.tabControlForm.SelectedTab.Controls.Add((Control) this.loanScreen);
      }
    }

    private string getFormID(string formTitle)
    {
      if (this.pageMode == TabLinksControl.PageMode.USDAManagement)
      {
        this.loanScreen.HideFormTitle();
        this.loanScreen.RemoveBorder();
      }
      switch (formTitle)
      {
        case "(Re) Lock Request":
          return "LOLOCKREQUEST";
        case "2018 HMDA Originated/Adverse Action Loans":
          return "HMDA2018_Originated";
        case "2018 HMDA Purchased Loans":
          return "HMDA2018_Purchased";
        case "ATR/QM Eligibility":
          return "ATR_Eligibility";
        case "Basic Info":
          if (this.pageMode == TabLinksControl.PageMode.ATRManagement)
            return "ATR_BorrowerInfo";
          return this.pageMode == TabLinksControl.PageMode.ConstructionManagement ? "CONSTRUCTIONMANAGEMENT_BasicInfo" : "FPMS_BasicInfo";
        case "Basic Information":
          return "VATool_BorrowerInfo";
        case "Cash-Out Refinance":
          return "VATool_CashOutRefinance";
        case "Compliance Review Result":
          return "COMPLIANCEREVIEWRESULT";
        case "Encompass Loan Data":
          return "ECSDATAVIEWER";
        case "Escrow Details":
          return "EscrowDetails";
        case "FHA 203k":
          return this.loan != null && this.loan.GetField("19").EndsWith("Refinance") ? "FPMS_203KRefi" : "FPMS_203KPurchase";
        case "Fannie Mae":
          return "ULDD_FannieMae";
        case "Freddie Mac":
          return "ULDD_FreddieMac";
        case "Ginnie Mae":
          return "ULDD_GinnieMae";
        case "HELOC Program":
          return "HELOCTool_Program";
        case "Important Terms and Agreement Language":
          return "HELOCTool_Terms";
        case "Informed Consumer Choice Disclosure Notice":
          return "FHAConsumerChoice";
        case "Linked Loans":
          return "CONSTRUCTIONMANAGEMENT_LinkConstPerm";
        case "Loan Closing Report":
          return "USDA_LoanClosing";
        case "Loan Info":
          return "CONSTRUCTIONMANAGEMENT_LoanInfo";
        case "Non-Standard to Standard Refi.":
          return "ATR_Refi";
        case "Payment History":
          return "CORRPURCHASEADVISEPAYMENTHISTORY";
        case "Prequalification":
          return this.loan.GetField("19") == "NoCash-Out Refinance" || this.loan.GetField("19") == "Cash-Out Refinance" ? "FPMS_Refi" : "FPMS_Purchase";
        case "Project Data":
          return "CONSTRUCTIONMANAGEMENT_ProjectData";
        case "Purchase Advice":
          return "CorrespondentPurchaseAdvice";
        case "Qualification":
          return this.pageMode == TabLinksControl.PageMode.ATRManagement ? "ATR_Qualification" : "VATool_Qualification";
        case "Repurchased Loans":
          return "HMDA2018_Repurchased";
        case "Req for SFH Loan Guarantee/Resv. of Funds":
          return "USDA_SFHLoan";
        case "Rural Assistance URLA":
          this.loanScreen.ShowFormTitle();
          this.loanScreen.ShowBorder();
          this.loanScreen.SetTitle("Rural Assistance URLA", (Control) this.navUSDAControl);
          return "USDA_RURAL1";
        case "Tracking":
          if (this.pageMode == TabLinksControl.PageMode.FHAProcessManagement)
            return "FPMS_Tracking";
          if (this.pageMode == TabLinksControl.PageMode.USDAManagement)
            return "USDA_Tracking";
          if (this.pageMode == TabLinksControl.PageMode.VAManagement)
            return "VATool_Tracking";
          break;
        case "Warehouse Bank Details":
          return "WarehouseBankDetails";
      }
      return string.Empty;
    }

    public string GetHelpTargetName()
    {
      if (this.currentForm.FormID == "LOLOCKREQUEST")
        return "Lock Request Form";
      if (this.currentForm.FormID == "FHAPROCESSMGT")
        return "FHA Management";
      if (this.currentForm.FormID == "ULDD")
        return "ULDD";
      if (this.currentForm.FormID == "USDAManagement")
        return "USDA Management";
      if (this.currentForm.FormID == "VAManagement")
        return "VA Management";
      if (this.currentForm.FormID == "ATRManagement")
        return "ATR/QM Management";
      if (this.currentForm.FormID == "CORRESPONDENTPURCHASEADVICE")
        return "Correspondent Purchase Advice Form";
      if (this.currentForm.FormID == "FPMS_203KPurchase" || this.currentForm.FormID == "FPMS_203KRefi")
        return "203K Worksheet";
      if (this.currentForm.FormID == "CONSTRUCTIONMANAGEMENT")
        return "Construction Management";
      if (this.currentForm.FormID == "HMDA_DENIAL04")
        return "HMDA Information 2018";
      if (this.currentForm.FormID == "HELOCManagement")
        return "HELOC Management";
      return this.currentForm.FormID == "ECSDATAVIEWER" ? "ECS Data Viewer" : "";
    }

    public void RefreshContents()
    {
      if (this.pageMode == TabLinksControl.PageMode.ConstructionManagement && this.loan != null && (this.tabControlForm.TabPages.Count < 4 && this.loan.GetField("19") == "ConstructionOnly" && this.loan.GetField("4084") == "Y" || this.tabControlForm.TabPages.Count >= 4 && (this.loan.GetField("19") != "ConstructionOnly" || this.loan.GetField("4084") != "Y")))
        this.loanScreen_OnFieldChanged((object) "19", new EventArgs());
      this.loanScreen.RefreshContents();
      if (this.additionalEditor != null)
        this.additionalEditor.RefreshContents();
      if (this.lockExtensionReqForm == null)
        return;
      this.lockExtensionReqForm.RefreshContents();
    }

    public void RefreshLoanContents()
    {
      this.loanScreen.RefreshLoanContents();
      if (this.additionalEditor != null)
        this.additionalEditor.RefreshLoanContents();
      if (this.lockExtensionReqForm == null)
        return;
      this.lockExtensionReqForm.RefreshLoanContents();
    }

    private void iconReset_Click(object sender, EventArgs e)
    {
      this.loanScreen.ExecAction("resetrequestform");
    }

    public void SelectAllFields()
    {
      if (this.loanScreen == null)
        return;
      this.loanScreen.SelectAllFields();
    }

    public void DeselectAllFields()
    {
      if (this.loanScreen == null)
        return;
      this.loanScreen.DeselectAllFields();
    }

    public InputFormInfo GetCurrentFormInfo()
    {
      if (this.currentForm.FormID.StartsWith("USDA_"))
        return new InputFormInfo("USDAManagement", "USDA Management");
      if (this.currentForm.FormID.StartsWith("FPMS_"))
        return new InputFormInfo("FHAPROCESSMGT", "FHA Management");
      if (this.currentForm.FormID.StartsWith("VATool_"))
        return new InputFormInfo("VAManagement", "VA Management");
      if (this.currentForm.FormID.StartsWith("ULDD_"))
        return new InputFormInfo("ULDD", "ULDD");
      if (this.currentForm.FormID.StartsWith("ATR_"))
        return new InputFormInfo("ATRManagement", "ATR/QM Management");
      if (this.currentForm.FormID.StartsWith("CONSTRUCTIONMANAGEMENT_"))
        return new InputFormInfo("CONSTRUCTIONMANAGEMENT", "Construction Management");
      if (this.currentForm.FormID.StartsWith("HMDA2018_"))
        return new InputFormInfo("HMDA_DENIAL04", "HMDA Information");
      return this.currentForm.FormID.StartsWith("HELOCTool_") ? new InputFormInfo("HELOCManagement", "HELOC Management") : (InputFormInfo) null;
    }

    public void RemoveQuickLinks() => this.panelLinks.Visible = false;

    public InputFormInfo GetCurrentChildFormInfo() => this.currentForm;

    public void SetGoToField(string fieldID)
    {
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
      this.toolTip1 = new ToolTip(this.components);
      this.groupContainer1 = new GroupContainer();
      this.panelLinks = new Panel();
      this.iconReset = new StandardIconButton();
      this.labelTitle = new Label();
      this.tabControlForm = new TabControl();
      this.tabPage1 = new TabPage();
      this.tabPage2 = new TabPage();
      this.tabPage3 = new TabPage();
      this.tabPage4 = new TabPage();
      this.tabPage5 = new TabPage();
      this.tabPage6 = new TabPage();
      this.groupContainer1.SuspendLayout();
      this.panelLinks.SuspendLayout();
      ((ISupportInitialize) this.iconReset).BeginInit();
      this.tabControlForm.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.panelLinks);
      this.groupContainer1.Controls.Add((Control) this.labelTitle);
      this.groupContainer1.Controls.Add((Control) this.tabControlForm);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(595, 760);
      this.groupContainer1.TabIndex = 3;
      this.panelLinks.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.panelLinks.BackColor = Color.Transparent;
      this.panelLinks.Controls.Add((Control) this.iconReset);
      this.panelLinks.Location = new Point(222, 1);
      this.panelLinks.Name = "panelLinks";
      this.panelLinks.Size = new Size(376, 21);
      this.panelLinks.TabIndex = 5;
      this.iconReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconReset.BackColor = Color.Transparent;
      this.iconReset.Location = new Point(351, 3);
      this.iconReset.MouseDownImage = (Image) null;
      this.iconReset.Name = "iconReset";
      this.iconReset.Size = new Size(16, 16);
      this.iconReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.iconReset.TabIndex = 9;
      this.iconReset.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.iconReset, "Reset Lock Request Form");
      this.iconReset.Visible = false;
      this.iconReset.Click += new EventHandler(this.iconReset_Click);
      this.labelTitle.AutoSize = true;
      this.labelTitle.BackColor = Color.Transparent;
      this.labelTitle.Font = new Font("Arial", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.labelTitle.Location = new Point(4, 5);
      this.labelTitle.Name = "labelTitle";
      this.labelTitle.Size = new Size(36, 15);
      this.labelTitle.TabIndex = 0;
      this.labelTitle.Text = "(title)";
      this.tabControlForm.Controls.Add((Control) this.tabPage1);
      this.tabControlForm.Controls.Add((Control) this.tabPage2);
      this.tabControlForm.Controls.Add((Control) this.tabPage3);
      this.tabControlForm.Controls.Add((Control) this.tabPage4);
      this.tabControlForm.Controls.Add((Control) this.tabPage5);
      this.tabControlForm.Controls.Add((Control) this.tabPage6);
      this.tabControlForm.Dock = DockStyle.Fill;
      this.tabControlForm.Location = new Point(1, 26);
      this.tabControlForm.Name = "tabControlForm";
      this.tabControlForm.Padding = new Point(11, 3);
      this.tabControlForm.SelectedIndex = 0;
      this.tabControlForm.Size = new Size(593, 733);
      this.tabControlForm.TabIndex = 2;
      this.tabControlForm.SelectedIndexChanged += new EventHandler(this.tabControlForm_SelectedIndexChanged);
      this.tabPage1.AutoScroll = true;
      this.tabPage1.Location = new Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(0, 2, 2, 2);
      this.tabPage1.Size = new Size(585, 707);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "tabPage1";
      this.tabPage1.UseVisualStyleBackColor = true;
      this.tabPage2.AutoScroll = true;
      this.tabPage2.Location = new Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new Padding(0, 2, 2, 2);
      this.tabPage2.Size = new Size(585, 707);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "tabPage2";
      this.tabPage2.UseVisualStyleBackColor = true;
      this.tabPage3.AutoScroll = true;
      this.tabPage3.Location = new Point(4, 22);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Padding = new Padding(0, 2, 2, 2);
      this.tabPage3.Size = new Size(585, 707);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "tabPage3";
      this.tabPage3.UseVisualStyleBackColor = true;
      this.tabPage4.AutoScroll = true;
      this.tabPage4.Location = new Point(4, 22);
      this.tabPage4.Name = "tabPage4";
      this.tabPage4.Padding = new Padding(0, 2, 2, 2);
      this.tabPage4.Size = new Size(585, 707);
      this.tabPage4.TabIndex = 3;
      this.tabPage4.Text = "tabPage4";
      this.tabPage4.UseVisualStyleBackColor = true;
      this.tabPage5.AutoScroll = true;
      this.tabPage5.Location = new Point(4, 22);
      this.tabPage5.Name = "tabPage5";
      this.tabPage5.Padding = new Padding(0, 2, 2, 2);
      this.tabPage5.Size = new Size(585, 707);
      this.tabPage5.TabIndex = 4;
      this.tabPage5.Text = "tabPage5";
      this.tabPage5.UseVisualStyleBackColor = true;
      this.tabPage6.AutoScroll = true;
      this.tabPage6.Location = new Point(4, 22);
      this.tabPage6.Name = "tabPage6";
      this.tabPage6.Padding = new Padding(0, 2, 2, 2);
      this.tabPage6.Size = new Size(585, 707);
      this.tabPage6.TabIndex = 5;
      this.tabPage6.Text = "tabPage6";
      this.tabPage6.UseVisualStyleBackColor = true;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (TabLinksControl);
      this.Size = new Size(595, 760);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.panelLinks.ResumeLayout(false);
      ((ISupportInitialize) this.iconReset).EndInit();
      this.tabControlForm.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public enum PageMode
    {
      Default,
      FHAProcessManagement,
      LockRequestForm,
      ULDD,
      USDAManagement,
      VAManagement,
      HELOCManagement,
      ATRManagement,
      CorrespondentPurchaseAdvice,
      ConstructionManagement,
      HMDA2018,
      ECSDataViewer,
    }
  }
}

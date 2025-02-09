// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.QuickLinksControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class QuickLinksControl : UserControl
  {
    private System.ComponentModel.Container components;
    private FlowLayoutPanel flpPanel;
    private List<QuickLinkLabel> linkLabels;
    private ILoanEditor editor;
    private LoanData loan;
    private Sessions.Session session;
    private static InputFormList systemFormList;

    public event EventHandler QuickLinkClicked;

    public QuickLinksControl()
    {
      this.session = Session.DefaultInstance;
      this.InitializeComponent();
    }

    public QuickLinksControl(
      ILoanEditor editor,
      QuickLink[] links,
      string currentFormId,
      Sessions.Session session)
      : this(editor, links, currentFormId, session, true)
    {
    }

    public QuickLinksControl(
      ILoanEditor editor,
      QuickLink[] links,
      string currentFormId,
      Sessions.Session session,
      bool showVerticalLine)
    {
      this.session = session;
      this.InitializeComponent();
      this.editor = editor;
      this.createLinks(links, showVerticalLine);
      if (!((currentFormId ?? "") != "") || !(currentFormId != "LOCOMPENSATION"))
        return;
      this.SetSelectedForm(currentFormId);
    }

    public void AddControl(System.Windows.Forms.Control control)
    {
      if (this.flpPanel.Controls.Contains(control))
        return;
      this.flpPanel.Controls.Add(control);
    }

    public void RemoveControl(System.Windows.Forms.Control control)
    {
      if (!this.flpPanel.Controls.Contains(control))
        return;
      this.flpPanel.Controls.Remove(control);
    }

    public QuickLinksControl(ILoanEditor editor, string currentFormId, Sessions.Session session)
      : this(editor, QuickLinksControl.GetQuickLinksForForm(currentFormId, session), currentFormId, session)
    {
    }

    public QuickLinksControl(LoanData loan, QuickLink[] links, Sessions.Session session)
      : this(loan, links, session, true)
    {
    }

    public QuickLinksControl(
      LoanData loan,
      QuickLink[] links,
      Sessions.Session session,
      bool showVerticalLine)
    {
      this.session = session;
      this.InitializeComponent();
      this.createLinks(links, showVerticalLine);
      this.loan = loan;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.flpPanel = new FlowLayoutPanel();
      this.SuspendLayout();
      this.flpPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flpPanel.BackColor = Color.Transparent;
      this.flpPanel.FlowDirection = FlowDirection.RightToLeft;
      this.flpPanel.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.flpPanel.Location = new Point(0, 3);
      this.flpPanel.Name = "flpPanel";
      this.flpPanel.Size = new Size(800, 22);
      this.flpPanel.TabIndex = 7;
      this.flpPanel.WrapContents = false;
      this.BackColor = Color.Transparent;
      this.Controls.Add((System.Windows.Forms.Control) this.flpPanel);
      this.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (QuickLinksControl);
      this.Size = new Size(800, 26);
      this.ResumeLayout(false);
    }

    private void createLinks(QuickLink[] links, bool showVerticalLine)
    {
      if (QuickLinksControl.systemFormList == null)
        QuickLinksControl.systemFormList = new InputFormList(this.session.SessionObjects);
      this.linkLabels = new List<QuickLinkLabel>();
      string empty = string.Empty;
      for (int index = links.Length - 1; index >= 0; --index)
      {
        if ((this.session.LoanData == null || !(links[index].FormID == "Disclosure Tracking") || ((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ToolsTab_DisclosureTracking) && ((this.session.LoanData.ContentAccess & LoanContentAccess.DisclosureTracking) == LoanContentAccess.DisclosureTracking || (this.session.LoanData.ContentAccess & LoanContentAccess.DisclosureTrackingViewOnly) == LoanContentAccess.DisclosureTrackingViewOnly || this.session.LoanData.ContentAccess == LoanContentAccess.FullAccess)) && (this.session.LoanData == null || !(links[index].FormID == "FeeVarianceWorksheet") || ((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ToolsTab_FeeToleranceWorksheet)))
        {
          QuickLinkLabel quickLinkLabel = new QuickLinkLabel(links[index]);
          quickLinkLabel.Click += new EventHandler(this.OnQuickLinkClick);
          this.flpPanel.Controls.Add((System.Windows.Forms.Control) quickLinkLabel);
          this.linkLabels.Insert(0, quickLinkLabel);
          string formIdToCheck = this.getFormIDToCheck(links[index].AccessFormID);
          if (formIdToCheck == "AUSTracking")
          {
            FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
            if (this.session.EncompassEdition != EncompassEdition.Broker && !aclManager.GetUserApplicationRight(AclFeature.ToolsTab_AUSTracking))
              quickLinkLabel.Enabled = false;
          }
          else if (!QuickLinksControl.systemFormList.IsAccessible(formIdToCheck) && !QuickLinksControl.systemFormList.IsTool(formIdToCheck) && formIdToCheck != "Disclosure Tracking" && !formIdToCheck.StartsWith("USDA_RURAL") && formIdToCheck != "AUSTracking")
            quickLinkLabel.Enabled = false;
          if (showVerticalLine && index > 0)
          {
            VerticalSeparator verticalSeparator = new VerticalSeparator();
            verticalSeparator.Margin = new Padding(3);
            this.flpPanel.Controls.Add((System.Windows.Forms.Control) verticalSeparator);
          }
        }
      }
    }

    private string getFormIDToCheck(string formID)
    {
      switch (formID)
      {
        case "RE88395PG1":
          return "RE88395";
        case "RE88395PG4":
          return "RE88395";
        case "HMDA:FHAPROCESSMGT":
          return "FHAPROCESSMGT";
        case "HMDA:USDAManagement":
          return "USDAManagement";
        default:
          return formID;
      }
    }

    protected virtual void OnQuickLinkClick(object sender, EventArgs e)
    {
      QuickLinkLabel quickLinkLabel = (QuickLinkLabel) sender;
      if (quickLinkLabel.Current)
        return;
      QuickLink quickLink = quickLinkLabel.QuickLink;
      if (quickLink.AccessFormID.StartsWith("USDA_RURAL"))
      {
        if (this.QuickLinkClicked == null)
          return;
        this.QuickLinkClicked(sender, e);
      }
      else if (this.editor == null || quickLink.AccessFormID == "ATRManagement" || quickLink.AccessFormID == "HUD1ES")
      {
        if ((quickLink.AccessFormID == "ATRManagement" || quickLink.AccessFormID == "HUD1ES") && this.loan == null)
          this.loan = this.session.LoanData;
        using (QuickEntryPopupDialog entryPopupDialog = new QuickEntryPopupDialog((IHtmlInput) this.loan, quickLink.Text, new InputFormInfo(quickLink.FormID, quickLink.AccessFormID == "ATRManagement" ? "ATR/QM Management" : quickLink.Text), quickLink.DialogWidth, quickLink.DialogHeight, FieldSource.CurrentLoan, "", this.session))
        {
          int num = (int) entryPopupDialog.ShowDialog((IWin32Window) this.session.MainForm);
          if (this.QuickLinkClicked == null)
            return;
          this.QuickLinkClicked(sender, e);
        }
      }
      else
        this.LoadSelectedForm(quickLink.FormID);
    }

    public void LoadSelectedForm(string formId)
    {
      if (this.linkLabels != null && this.linkLabels.Count == 1 && this.linkLabels[0].QuickLink.FormID == "REGZGFE_2010" && formId == "REGZGFE_2010")
        this.editor.OpenFormByID(formId, (System.Windows.Forms.Control) null);
      else if (this.linkLabels != null && this.linkLabels[0].QuickLink.FormID == "REGZGFE_2015")
      {
        this.editor.OpenFormByID(formId, (System.Windows.Forms.Control) null);
      }
      else
      {
        switch (formId)
        {
          case "Disclosure Tracking":
            this.editor.OpenForm("Disclosure Tracking");
            break;
          case "RE88395PG1":
            this.editor.OpenFormByID("885 P1-3", (System.Windows.Forms.Control) this);
            break;
          case "FeeVarianceWorksheet":
            this.editor.OpenFormByID(formId, (System.Windows.Forms.Control) null);
            break;
          default:
            this.editor.OpenFormByID(formId, (System.Windows.Forms.Control) this);
            break;
        }
      }
      this.SetSelectedForm(formId);
    }

    public void SetSelectedForm(string formId)
    {
      bool flag = false;
      foreach (QuickLinkLabel linkLabel in this.linkLabels)
      {
        if (linkLabel.Enabled)
        {
          string formId1 = linkLabel.QuickLink.FormID;
          linkLabel.Current = formId1 == formId;
          flag |= formId1 == formId;
        }
      }
    }

    public static QuickLink[] GetQuickLinksForForm(string formId, Sessions.Session session)
    {
      return QuickLinksControl.GetQuickLinksForForm(formId, false, false, session);
    }

    public static QuickLink[] GetQuickLinksForForm(
      string formId,
      bool useNewRESPA,
      bool use2015RESPA,
      Sessions.Session session,
      bool useURLA2020 = false)
    {
      switch (formId.ToUpper())
      {
        case "BIWEEKLYSUMMARY":
          return new QuickLink[1]
          {
            new QuickLink("Aggregate Escrow Account", "HUD1ES", 700, 600)
          };
        case "BROKERCHECKCALCULATION":
        case "FUNDBALANCINGWORKSHEET":
        case "FUNDINGWORKSHEET":
        case "HUD1PG1":
        case "HUD1PG2":
          return QuickLinksControl.createFunderLinks(useNewRESPA, use2015RESPA, session);
        case "CLOSINGDISCLOSUREPAGE1":
        case "CLOSINGDISCLOSUREPAGE2":
        case "CLOSINGDISCLOSUREPAGE3":
        case "CLOSINGDISCLOSUREPAGE4":
        case "CLOSINGDISCLOSUREPAGE5":
          return QuickLinksControl.create2015RESPALinksForCD();
        case "CUSTOMFIELDS":
          return QuickLinksControl.createCustomFieldsLinks();
        case "D10031":
        case "D10032":
        case "D10033":
        case "D1003_2020P1":
        case "D1003_2020P2":
        case "D1003_2020P3":
        case "D1003_2020P4":
        case "D1003_2020P5":
        case "REGZ50":
        case "REGZGFE":
        case "TSUM":
          return QuickLinksControl.create1003Links(useNewRESPA, use2015RESPA, useURLA2020);
        case "FEEVARIANCEWORKSHEET":
          return QuickLinksControl.createFeeVarianceWorksheetLinks();
        case "FM1084":
        case "FM1084A":
        case "FM1084B":
          return QuickLinksControl.create1084Links();
        case "HUD1PG1_2010":
        case "HUD1PG2_2010":
        case "HUD1PG3_2010":
        case "REGZGFEHUD":
        case "REGZGFE_2010":
          return QuickLinksControl.createRESPALinks();
        case "LOANESTIMATEPAGE1":
        case "LOANESTIMATEPAGE2":
        case "LOANESTIMATEPAGE3":
        case "REGZGFE_2015":
        case "REGZLE":
          return QuickLinksControl.create2015RESPALinks();
        case "LOCOMPENSATION":
          return new QuickLink[1]
          {
            new QuickLink("Itemization", use2015RESPA ? "REGZGFE_2015" : "REGZGFE_2010")
          };
        case "MCAWPUR":
          return QuickLinksControl.createMCAWPurchaseLinks(useNewRESPA, use2015RESPA);
        case "MCAWREFI":
          return QuickLinksControl.createMCAWRefiLinks(useNewRESPA, use2015RESPA);
        case "RE88395":
          return QuickLinksControl.createMLDSLinks(useNewRESPA, use2015RESPA);
        case "UNDERWRITERSUMMARY":
          return QuickLinksControl.createUWSummaryLinks();
        default:
          return (QuickLink[]) null;
      }
    }

    private static QuickLink[] create2015RESPALinks()
    {
      return new List<QuickLink>()
      {
        new QuickLink("REGZ-LE", "REGZLE"),
        new QuickLink("Itemization", "REGZGFE_2015"),
        new QuickLink("LE1", "LOANESTIMATEPAGE1"),
        new QuickLink("LE2", "LOANESTIMATEPAGE2"),
        new QuickLink("LE3", "LOANESTIMATEPAGE3"),
        new QuickLink("CD1", "ClosingDisclosurePage1"),
        new QuickLink("CD2", "ClosingDisclosurePage2"),
        new QuickLink("CD3", "ClosingDisclosurePage3"),
        new QuickLink("CD4", "ClosingDisclosurePage4"),
        new QuickLink("CD5", "ClosingDisclosurePage5"),
        new QuickLink("Disclosure Tracking", "Disclosure Tracking"),
        new QuickLink("Fee Variance", "FeeVarianceWorksheet")
      }.ToArray();
    }

    private static QuickLink[] create2015RESPALinksForCD()
    {
      return new List<QuickLink>()
      {
        new QuickLink("REGZ-LE", "REGZLE"),
        new QuickLink("Itemization", "REGZGFE_2015"),
        new QuickLink("LE1", "LOANESTIMATEPAGE1"),
        new QuickLink("LE2", "LOANESTIMATEPAGE2"),
        new QuickLink("LE3", "LOANESTIMATEPAGE3"),
        new QuickLink("REGZ-CD", "REGZCD"),
        new QuickLink("CD1", "ClosingDisclosurePage1"),
        new QuickLink("CD2", "ClosingDisclosurePage2"),
        new QuickLink("CD3", "ClosingDisclosurePage3"),
        new QuickLink("CD4", "ClosingDisclosurePage4"),
        new QuickLink("CD5", "ClosingDisclosurePage5"),
        new QuickLink("Disclosure Tracking", "Disclosure Tracking"),
        new QuickLink("Fee Variance", "FeeVarianceWorksheet")
      }.ToArray();
    }

    private static QuickLink[] createFeeVarianceWorksheetLinks()
    {
      return new List<QuickLink>()
      {
        new QuickLink("Itemization", "REGZGFE_2015"),
        new QuickLink("LE2", "LOANESTIMATEPAGE2"),
        new QuickLink("CD2", "ClosingDisclosurePage2"),
        new QuickLink("Disclosure Tracking", "Disclosure Tracking")
      }.ToArray();
    }

    private static QuickLink[] createRESPALinks()
    {
      return new List<QuickLink>()
      {
        new QuickLink("GFE", "REGZGFEHUD"),
        new QuickLink("Itemization", "REGZGFE_2010"),
        new QuickLink("HUD-1 Pg 1", "HUD1PG1_2010"),
        new QuickLink("HUD-1 Pg 2", "HUD1PG2_2010"),
        new QuickLink("HUD-1 Pg 3", "HUD1PG3_2010"),
        new QuickLink("Disclosure Tracking", "Disclosure Tracking")
      }.ToArray();
    }

    private static QuickLink[] createFunderLinks(
      bool useNewRESPA,
      bool use2015RESPA,
      Sessions.Session session)
    {
      List<QuickLink> quickLinkList = new List<QuickLink>();
      UserInfo userInfo = session.UserInfo;
      FeaturesAclManager aclManager = (FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features);
      if (session.EncompassEdition == EncompassEdition.Banker)
      {
        if (aclManager.GetUserApplicationRight(AclFeature.ToolsTab_FundingWS))
          quickLinkList.Add(new QuickLink("Funding Worksheet", "FUNDINGWORKSHEET"));
        if (aclManager.GetUserApplicationRight(AclFeature.ToolsTab_FundingBalWS))
          quickLinkList.Add(new QuickLink("Balance", "FUNDBALANCINGWORKSHEET"));
        if (aclManager.GetUserApplicationRight(AclFeature.ToolsTab_BrokerCheckCal))
          quickLinkList.Add(new QuickLink("Broker Check", "BROKERCHECKCALCULATION"));
      }
      if (use2015RESPA)
      {
        quickLinkList.Add(new QuickLink("CD2", "ClosingDisclosurePage2"));
        quickLinkList.Add(new QuickLink("CD3", "ClosingDisclosurePage3"));
      }
      else if (useNewRESPA)
      {
        quickLinkList.Add(new QuickLink("HUD-1 Pg 1", "HUD1PG1_2010"));
        quickLinkList.Add(new QuickLink("HUD-1 Pg 2", "HUD1PG2_2010"));
        quickLinkList.Add(new QuickLink("HUD-1 Pg 3", "HUD1PG3_2010"));
      }
      else
      {
        quickLinkList.Add(new QuickLink("HUD-1 Pg 1", "HUD1PG1"));
        quickLinkList.Add(new QuickLink("HUD-1 Pg 2", "HUD1PG2"));
      }
      return quickLinkList.ToArray();
    }

    private static QuickLink[] createCustomFieldsLinks()
    {
      return new List<QuickLink>()
      {
        new QuickLink("Page 1", "CF_1", "CUSTOMFIELDS"),
        new QuickLink("Page 2", "CF_2", "CUSTOMFIELDS"),
        new QuickLink("Page 3", "CF_3", "CUSTOMFIELDS"),
        new QuickLink("Page 4", "CF_4", "CUSTOMFIELDS")
      }.ToArray();
    }

    private static QuickLink[] create1003Links(
      bool useNewRESPA,
      bool use2015RESPA,
      bool useURLA2020)
    {
      List<QuickLink> quickLinkList = new List<QuickLink>();
      if (!useURLA2020)
      {
        quickLinkList.Add(new QuickLink("1003 P1", "D10031"));
        quickLinkList.Add(new QuickLink("1003 P2", "D10032"));
        quickLinkList.Add(new QuickLink("1003 P3", "D10033"));
      }
      else
      {
        quickLinkList.Add(new QuickLink("1003 URLA P1", "D1003_2020P1"));
        quickLinkList.Add(new QuickLink("1003 URLA P2", "D1003_2020P2"));
        quickLinkList.Add(new QuickLink("1003 URLA P3", "D1003_2020P3"));
        quickLinkList.Add(new QuickLink("1003 URLA P4", "D1003_2020P4"));
        quickLinkList.Add(new QuickLink("1003 URLA Lender", "D1003_2020P5"));
      }
      if (use2015RESPA)
        quickLinkList.Add(new QuickLink("REGZ-LE", "REGZLE"));
      else
        quickLinkList.Add(new QuickLink("REGZ", "REGZ50"));
      if (use2015RESPA)
        quickLinkList.Add(new QuickLink("Itemization", "REGZGFE_2015"));
      else if (useNewRESPA)
        quickLinkList.Add(new QuickLink("Itemization", "REGZGFE_2010"));
      else
        quickLinkList.Add(new QuickLink("GFE", "REGZGFE"));
      quickLinkList.Add(new QuickLink("1008", "TSUM"));
      quickLinkList.Add(new QuickLink("Disclosure Tracking", "Disclosure Tracking"));
      return quickLinkList.ToArray();
    }

    private static QuickLink[] createMLDSLinks(bool useNewRESPA, bool use2015RESPA)
    {
      List<QuickLink> quickLinkList = new List<QuickLink>();
      if (useNewRESPA)
      {
        quickLinkList.Add(new QuickLink("882", "RE88395"));
        quickLinkList.Add(new QuickLink("885 P1-3", "RE88395PG1"));
        quickLinkList.Add(new QuickLink("885 P4", "RE88395PG4"));
      }
      else
      {
        quickLinkList.Add(new QuickLink("MLDS P1-3", "RE88395"));
        quickLinkList.Add(new QuickLink("MLDS P4", "RE88395PG4"));
      }
      return quickLinkList.ToArray();
    }

    private static QuickLink[] createUWSummaryLinks()
    {
      return new List<QuickLink>()
      {
        new QuickLink("UW P1", "UNDERWRITERSUMMARY"),
        new QuickLink("UW P2", "UNDERWRITERSUMMARYP2"),
        new QuickLink("ATR/QM", "ATRManagement", 700, 600)
      }.ToArray();
    }

    private static QuickLink[] createMCAWPurchaseLinks(bool useNewRESPA, bool use2015RESPA)
    {
      List<QuickLink> quickLinkList = new List<QuickLink>();
      quickLinkList.Add(new QuickLink("1003 P1", "D10031"));
      quickLinkList.Add(new QuickLink("1003 P2", "D10032"));
      quickLinkList.Add(new QuickLink("1003 P3", "D10033"));
      quickLinkList.Add(new QuickLink("MCAW Purchase", "MCAWPUR"));
      if (use2015RESPA)
        quickLinkList.Add(new QuickLink("Itemization", "REGZGFE_2015"));
      else if (useNewRESPA)
        quickLinkList.Add(new QuickLink("Itemization", "REGZGFE_2010"));
      else
        quickLinkList.Add(new QuickLink("GFE", "REGZGFE"));
      quickLinkList.Add(new QuickLink("1008", "TSUM"));
      return quickLinkList.ToArray();
    }

    private static QuickLink[] createMCAWRefiLinks(bool useNewRESPA, bool use2015RESPA)
    {
      List<QuickLink> quickLinkList = new List<QuickLink>();
      quickLinkList.Add(new QuickLink("1003 P1", "D10031"));
      quickLinkList.Add(new QuickLink("1003 P2", "D10032"));
      quickLinkList.Add(new QuickLink("1003 P3", "D10033"));
      quickLinkList.Add(new QuickLink("MCAW Refi", "MCAWREFI"));
      if (use2015RESPA)
        quickLinkList.Add(new QuickLink("Itemization", "REGZGFE_2015"));
      else if (useNewRESPA)
        quickLinkList.Add(new QuickLink("Itemization", "REGZGFE_2010"));
      else
        quickLinkList.Add(new QuickLink("GFE", "REGZGFE"));
      quickLinkList.Add(new QuickLink("1008", "TSUM"));
      return quickLinkList.ToArray();
    }

    private static QuickLink[] create1084Links()
    {
      return new List<QuickLink>()
      {
        new QuickLink("Form A", "FM1084A"),
        new QuickLink("Form B", "FM1084B")
      }.ToArray();
    }
  }
}

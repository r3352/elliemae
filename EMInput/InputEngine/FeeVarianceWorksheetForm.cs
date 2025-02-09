// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FeeVarianceWorksheetForm
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class FeeVarianceWorksheetForm : 
    UserControl,
    IMainScreen,
    IWin32Window,
    IApplicationWindow,
    ISynchronizeInvoke,
    IRefreshContents,
    IOnlineHelpTarget
  {
    private const string className = "FeeVarianceWorksheetForm";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private IHtmlInput iData;
    private bool readOnly;
    private LoanScreen detailScreen;
    private LoanData loan;
    private Sessions.Session session;
    private IContainer components;
    private GroupContainer groupContainerDetail;
    private Panel panelDetail;
    private Panel panelLinks;

    public FeeVarianceWorksheetForm(
      IHtmlInput iData,
      bool readOnly,
      Sessions.Session session,
      QuickLinksControl linkControl)
    {
      this.iData = iData;
      this.readOnly = readOnly;
      this.session = session;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      if (this.iData is LoanData)
        this.loan = (LoanData) this.iData;
      this.detailScreen = new LoanScreen(this.session, !(this.iData is LoanData) || this.loan.IsInFindFieldForm ? (IWin32Window) this.ParentForm : (IWin32Window) this, this.iData);
      this.detailScreen.RemoveTitle();
      this.detailScreen.RemoveBorder();
      this.panelDetail.Controls.Add((Control) this.detailScreen);
      this.detailScreen.BrowserHandler.SetHelpTarget((IOnlineHelpTarget) this);
      this.detailScreen.LoadForm(new InputFormInfo("FeeVarianceWorksheet", "Fee Variance Worksheet"));
      if (linkControl == null)
        return;
      this.panelLinks.Controls.Add((Control) linkControl);
      linkControl.Dock = DockStyle.Right;
    }

    public string GetHelpTargetName() => "Fee Variance Worksheet";

    public void RefreshContents()
    {
    }

    public void RefreshLoanContents()
    {
    }

    public void ShowHelp(Control control)
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
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

    public void RefreshCE()
    {
    }

    public void NavigateToTradesTab(int tradeId)
    {
    }

    public bool IsClientEnabledToExportFNMFRE => false;

    public bool IsUnderwriterSummaryAccessibleForBroker => false;

    public void DisplayTPOCompanySetting(ExternalOriginatorManagementData o)
    {
    }

    public void OpenURL(string url, string title, int width, int height)
    {
    }

    public Form OpenURL(string windowName, string url, string title, int width, int height)
    {
      return (Form) null;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainerDetail = new GroupContainer();
      this.panelDetail = new Panel();
      this.panelLinks = new Panel();
      this.groupContainerDetail.SuspendLayout();
      this.SuspendLayout();
      this.groupContainerDetail.Controls.Add((Control) this.panelLinks);
      this.groupContainerDetail.Controls.Add((Control) this.panelDetail);
      this.groupContainerDetail.Dock = DockStyle.Fill;
      this.groupContainerDetail.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerDetail.Location = new Point(0, 0);
      this.groupContainerDetail.Name = "groupContainerDetail";
      this.groupContainerDetail.Size = new Size(707, 565);
      this.groupContainerDetail.TabIndex = 1;
      this.groupContainerDetail.Text = "Fee Variance Worksheet";
      this.panelDetail.Dock = DockStyle.Fill;
      this.panelDetail.Location = new Point(1, 26);
      this.panelDetail.Name = "panelDetail";
      this.panelDetail.Size = new Size(705, 538);
      this.panelDetail.TabIndex = 0;
      this.panelLinks.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.panelLinks.BackColor = Color.Transparent;
      this.panelLinks.Location = new Point(235, 3);
      this.panelLinks.Name = "panelLinks";
      this.panelLinks.Size = new Size(472, 23);
      this.panelLinks.TabIndex = 10;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainerDetail);
      this.Name = nameof (FeeVarianceWorksheetForm);
      this.Size = new Size(707, 565);
      this.groupContainerDetail.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}

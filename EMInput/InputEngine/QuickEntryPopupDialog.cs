// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.QuickEntryPopupDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Verification;
using EllieMae.Encompass.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class QuickEntryPopupDialog : 
    System.Windows.Forms.Form,
    IWorkArea,
    IWin32Window,
    IMainScreen,
    IApplicationWindow,
    ISynchronizeInvoke,
    IOnlineHelpTarget,
    IHelp
  {
    private const string className = "QuickEntryPopupDialog";
    private static string sw = Tracing.SwInputEngine;
    private System.Windows.Forms.Panel panelBottom;
    private System.Windows.Forms.Button btnClose;
    private System.ComponentModel.Container components;
    private System.Windows.Forms.Panel workPanel;
    private TabControl quickTab;
    private TabPage tabVOL;
    private TabPage tabVOM;
    private TabPage tabAdditional;
    private System.Windows.Forms.Panel additionalPanel;
    private LoanScreen freeScreen;
    private LoanScreen freeScreen2;
    private LoanData loanData;
    private IHtmlInput inputData;
    private System.Windows.Forms.Panel volPanel;
    private System.Windows.Forms.Panel vomPanel;
    private string htmFile = string.Empty;
    private EMHelpLink emHelpLink1;
    private FieldSource fieldSource;
    private System.Windows.Forms.Button btnOK;
    private Sessions.Session session;
    private bool inputConfirmationRequired;
    private System.Windows.Forms.Panel panelInstruction;
    private System.Windows.Forms.Label labelInstruction;
    private System.Windows.Forms.Panel panelMiddle;
    private string formTitle = string.Empty;
    private System.Windows.Forms.Control topControl;
    private VOLPanel volPan;
    private VODPanel vodPan;
    private VOGGPanel voggPan;
    private VOOIPanel vooiPan;
    private VOOLPanel voolPan;
    private VOOAPanel vooaPan;
    private VOMPanel vomPan;
    private VORPanel vorPan;
    private VOEPanel voePan;
    private VOALPanel voalPan;
    private TAX4506Panel tax4506Pan;
    private TAX4506TPanel tax4506TPan;
    private DISPanel disPan;
    private bool dataIsValid = true;
    private string hudLineNo = string.Empty;

    public event EventHandler OkClicked;

    public event EventHandler ButtonClicked;

    public event EventHandler LoanScreenLoaded;

    public event EventHandler OnFieldChanged;

    public event EventHandler OnDialogDeactivated;

    public QuickEntryPopupDialog(
      IHtmlInput inputData,
      string formTitle,
      InputFormInfo formInfo,
      int sizeWidth,
      int sizeHeight,
      FieldSource fieldSource,
      string helpTag,
      Sessions.Session session)
      : this(inputData, formTitle, formInfo, sizeWidth, sizeHeight, fieldSource, helpTag, session, (object) null)
    {
    }

    public QuickEntryPopupDialog(
      IHtmlInput inputData,
      string formTitle,
      InputFormInfo formInfo,
      int sizeWidth,
      int sizeHeight,
      FieldSource fieldSource,
      string helpTag,
      Sessions.Session session,
      object property)
    {
      this.session = session;
      this.fieldSource = fieldSource;
      this.inputData = inputData;
      this.formTitle = formTitle;
      if (inputData is LoanData)
      {
        this.loanData = (LoanData) inputData;
        this.loanData.Closed += new EventHandler(this.loanData_Closed);
      }
      else
        this.loanData = (LoanData) null;
      if (inputData is DisclosedItemizationHandler)
        this.session.LoanData.Closed += new EventHandler(this.loanData_Closed);
      this.InitializeComponent();
      this.quickTab.SelectedIndexChanged += new EventHandler(this.quickTab_SelectedIndexChanged);
      this.panelInstruction.Visible = false;
      this.btnOK.Visible = this.inputConfirmationRequired = false;
      if (formInfo != (InputFormInfo) null && (formInfo.FormID == "Popup_AdjustmentsBorrowerCreditsUCD" || formInfo.FormID == "Popup_AdjustmentsSellerCreditsUCD" || formInfo.FormID == "HMDATransmittal_2018" || formInfo.FormID == "D1003_2020ACKNOWLEDGEMENT"))
        this.Text = formTitle;
      else if (formInfo != (InputFormInfo) null && formInfo.FormID == "DISPanel")
        this.Text += " - Disaster Declarations";
      else
        this.Text = this.Text + " - " + formTitle;
      if (sizeWidth > 0 && sizeHeight > 0)
        this.Size = new Size(sizeWidth, sizeHeight);
      if (formInfo.FormID == "AUSTracking")
        this.AddToWorkArea((System.Windows.Forms.Control) new AUSTrackingTool(this.session, this.loanData));
      else if (formInfo.FormID == "HomeCounselingProviders")
        this.AddToWorkArea((System.Windows.Forms.Control) new HomeCounselingProviderForm((IHtmlInput) this.loanData, false, this.session));
      if (formInfo.Name != "VOLPanel/VOMPanel" && formInfo.Name != "Popup_AdjustmentsBorrowerCreditsUCD")
      {
        this.workPanel.Dock = DockStyle.Fill;
        this.quickTab.Visible = false;
      }
      else
      {
        this.htmFile = formInfo.Name;
        this.quickTab.Dock = DockStyle.Fill;
        this.workPanel.Visible = false;
      }
      Cursor.Current = Cursors.WaitCursor;
      switch (formInfo.Name)
      {
        case "AUS Tracking":
        case "HomeCounselingProviders":
          if (helpTag != string.Empty)
          {
            this.emHelpLink1.Visible = true;
            this.emHelpLink1.HelpTag = helpTag;
          }
          else
            this.emHelpLink1.Visible = false;
          Cursor.Current = Cursors.Default;
          break;
        case "DISPanel":
        case "Disaster":
          this.ShowVerifPanel("Disaster");
          goto case "AUS Tracking";
        case "Popup_AdjustmentsBorrowerCreditsUCD":
          this.Size = new Size(sizeWidth, 600);
          this.tabVOL.Text = "UCD";
          this.tabVOM.Text = "Non-UCD";
          this.quickTab.TabPages.Remove(this.tabAdditional);
          this.quickTab.Tag = (object) "AdjustmentsAndOtherCredits";
          bool flag1 = false;
          for (int index = 5; index <= 24; ++index)
          {
            if (this.inputData.GetField("LE2.X" + (object) index) != "")
            {
              flag1 = true;
              break;
            }
          }
          if (this.inputData.GetField("220") != "")
            flag1 = true;
          bool flag2 = false;
          if (flag1)
          {
            for (int index = 139; index <= 442; ++index)
            {
              if (index != (int) byte.MaxValue && index != 256 && index != 258 && index != 259 && index != 261 && index != 262 && index != 338 && index != 339 && index != 341 && index != 342 && index != 344 && index != 345 && this.inputData.GetField("CD3.X" + (object) index) != "" && this.inputData.GetField("CD3.X" + (object) index) != "//")
              {
                flag2 = true;
                break;
              }
            }
          }
          if (!flag2 & flag1)
            this.quickTab.SelectedTab = this.tabVOM;
          this.Text = "Adjustments and Other Details";
          this.labelInstruction.Text = "For loans planned for GSE delivery, access the UCD tab to enter details for Sections K and L to ensure data points required for the UCD export are included in the loan file. For non-GSE loans, access the Non-UCD tab.";
          this.panelInstruction.Visible = true;
          if (this.loanData != null && this.loanData.IsTemplate)
          {
            this.freeScreen = new LoanScreen(this.session, (IWin32Window) null, inputData);
          }
          else
          {
            if (this.loanData == null && this.inputData != null)
            {
              switch (inputData)
              {
                case DisclosedLEHandler _:
                case DisclosedCDHandler _:
                  this.freeScreen = new LoanScreen(this.session, (IWin32Window) this, inputData);
                  goto label_55;
              }
            }
            this.freeScreen = new LoanScreen(this.session, (IWin32Window) this, (IHtmlInput) this.loanData);
          }
label_55:
          this.freeScreen.HideFormTitle();
          this.volPanel.Controls.Add((System.Windows.Forms.Control) this.freeScreen);
          this.freeScreen.LoadForm(new InputFormInfo("Popup_AdjustmentsBorrowerCreditsUCD", "Popup_AdjustmentsBorrowerCreditsUCD"));
          if (this.loanData != null && this.loanData.IsTemplate)
          {
            this.freeScreen2 = new LoanScreen(this.session, (IWin32Window) null, inputData);
          }
          else
          {
            if (this.loanData == null && this.inputData != null)
            {
              switch (inputData)
              {
                case DisclosedLEHandler _:
                case DisclosedCDHandler _:
                  this.freeScreen2 = new LoanScreen(this.session, (IWin32Window) this, inputData);
                  goto label_61;
              }
            }
            this.freeScreen2 = new LoanScreen(this.session, (IWin32Window) this, (IHtmlInput) this.loanData);
          }
label_61:
          this.freeScreen2.HideFormTitle();
          this.vomPanel.Controls.Add((System.Windows.Forms.Control) this.freeScreen2);
          this.freeScreen2.LoadForm(new InputFormInfo("Popup_AdjustmentsCreditDetails", "Popup_AdjustmentsCreditDetails"));
          goto case "AUS Tracking";
        case "Request for Copy of Tax Return":
          this.ShowVerifPanel("TAX4506");
          goto case "AUS Tracking";
        case "Request for Transcript of Tax":
          this.ShowVerifPanel("TAX4506T");
          goto case "AUS Tracking";
        case "VOAL":
        case "VOALPanel":
          this.ShowVerifPanel("VOAL");
          goto case "AUS Tracking";
        case "VOD":
        case "VODPanel":
          this.ShowVerifPanel("VOD");
          goto case "AUS Tracking";
        case "VOE":
        case "VOEPanel":
          this.ShowVerifPanel("VOE");
          goto case "AUS Tracking";
        case "VOGG":
        case "VOGGPanel":
          this.ShowVerifPanel("VOGG");
          goto case "AUS Tracking";
        case "VOL":
        case "VOLPanel":
          this.ShowVerifPanel("VOL");
          goto case "AUS Tracking";
        case "VOL/VOM":
        case "VOLPanel/VOMPanel":
          this.freeScreen = new LoanScreen(this.session, (IWin32Window) this, (IHtmlInput) this.loanData);
          this.freeScreen.HideFormTitle();
          this.additionalPanel.Controls.Add((System.Windows.Forms.Control) this.freeScreen);
          this.Text = "Additional Liabilities";
          this.freeScreen.LoadForm(new InputFormInfo("PREQUAL_ALIMONY", "PREQUAL_ALIMONY"));
          this.ShowVerifPanel("VOM");
          this.ShowVerifPanel("VOL");
          goto case "AUS Tracking";
        case "VOM":
        case "VOMPanel":
          this.ShowVerifPanel("VOM");
          goto case "AUS Tracking";
        case "VOOA":
        case "VOOAPanel":
          this.ShowVerifPanel("VOOA");
          goto case "AUS Tracking";
        case "VOOI":
        case "VOOIPanel":
          this.ShowVerifPanel("VOOI");
          goto case "AUS Tracking";
        case "VOOL":
        case "VOOLPanel":
          this.ShowVerifPanel("VOOL");
          goto case "AUS Tracking";
        case "VOR":
        case "VORPanel":
          this.ShowVerifPanel("VOR");
          goto case "AUS Tracking";
        default:
          if (TabLinksControl.UseTabLinks(this.session, formInfo))
          {
            TabLinksControl newControl = new TabLinksControl(this.session, formInfo, (IWin32Window) Session.MainScreen, this.loanData);
            newControl.RemoveQuickLinks();
            this.freeScreen = new LoanScreen(this.session, (IWin32Window) this, (IHtmlInput) this.loanData);
            this.freeScreen.HideFormTitle();
            this.AddToWorkArea((System.Windows.Forms.Control) newControl);
            goto case "AUS Tracking";
          }
          else
          {
            if (formInfo.Name == "Popup_AdjustmentsSellerCreditsUCD")
            {
              this.Size = new Size(900, 600);
              this.panelInstruction.Visible = true;
            }
            this.freeScreen = this.loanData == null || !this.loanData.IsTemplate ? new LoanScreen(this.session, (IWin32Window) this, this.inputData) : new LoanScreen(this.session, (IWin32Window) null, (IHtmlInput) this.loanData);
            if (formInfo.Name.StartsWith("FC_NonBorrowingOwnerContact") || formInfo.Name.ToLower().StartsWith("bestcontactdaytime_nboc"))
            {
              int num1 = formInfo.Name.IndexOf("#");
              if (num1 > -1)
              {
                int num2 = Utils.ParseInt((object) formInfo.Name.Substring(num1 + 1));
                this.freeScreen.BrwHandler.Property = num2 <= 0 ? (object) 1 : (object) num2;
              }
            }
            if (formInfo.Name == "REGZGFE_2015")
              this.freeScreen.SetTitleOnly("2015 Itemization");
            else
              this.freeScreen.HideFormTitle();
            this.workPanel.Controls.Add((System.Windows.Forms.Control) this.freeScreen);
            this.freeScreen.LoadForm(formInfo);
            if (property != null)
              this.freeScreen.BrowserHandler.Property = property;
            this.freeScreen.FormLoaded += new EventHandler(this.freeScreen_FormLoaded);
            this.freeScreen.OnFieldChanged += new EventHandler(this.freeScreen_OnFieldChanged);
            this.freeScreen.ButtonClicked += new EventHandler(this.freeScreen_ButtonClicked);
            this.freeScreen.SetHelpTarget((IOnlineHelpTarget) this);
            goto case "AUS Tracking";
          }
      }
    }

    private void quickTab_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.quickTab.Tag == null || !(this.quickTab.Tag.ToString() == "AdjustmentsAndOtherCredits") || this.quickTab.SelectedTab == null)
        return;
      if (this.quickTab.SelectedTab.Text == "UCD")
        this.freeScreen.RefreshAllControls(true);
      else
        this.freeScreen2.RefreshAllControls(true);
    }

    private void loanData_Closed(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        this.Unload();
        if (this.components != null)
          this.components.Dispose();
      }
      base.Dispose(disposing);
    }

    public void Unload()
    {
      if (this.loanData != null)
        this.loanData.Closed -= new EventHandler(this.loanData_Closed);
      if (this.freeScreen != null)
      {
        this.freeScreen.FormLoaded -= new EventHandler(this.freeScreen_FormLoaded);
        this.freeScreen.OnFieldChanged -= new EventHandler(this.freeScreen_OnFieldChanged);
        this.freeScreen.ButtonClicked -= new EventHandler(this.freeScreen_ButtonClicked);
      }
      this.btnOK.Click -= new EventHandler(this.btnOK_Click);
      this.btnClose.Click -= new EventHandler(this.btnClose_Click);
      this.Closing -= new CancelEventHandler(this.PrequalQuickLinkDialog_Closing);
      this.Deactivate -= new EventHandler(this.QuickEntryPopupDialog_Deactivate);
      this.FormClosing -= new FormClosingEventHandler(this.QuickEntryPopupDialog_FormClosing);
      this.KeyDown -= new KeyEventHandler(this.Help_KeyDown);
      this.KeyPress -= new KeyPressEventHandler(this.QuickEntryPopupDialog_KeyPress);
    }

    public System.Windows.Forms.Control TopControl
    {
      set => this.topControl = value;
    }

    private void InitializeComponent()
    {
      this.panelBottom = new System.Windows.Forms.Panel();
      this.btnOK = new System.Windows.Forms.Button();
      this.emHelpLink1 = new EMHelpLink();
      this.btnClose = new System.Windows.Forms.Button();
      this.workPanel = new System.Windows.Forms.Panel();
      this.quickTab = new TabControl();
      this.tabVOL = new TabPage();
      this.volPanel = new System.Windows.Forms.Panel();
      this.tabVOM = new TabPage();
      this.vomPanel = new System.Windows.Forms.Panel();
      this.tabAdditional = new TabPage();
      this.additionalPanel = new System.Windows.Forms.Panel();
      this.panelInstruction = new System.Windows.Forms.Panel();
      this.labelInstruction = new System.Windows.Forms.Label();
      this.panelMiddle = new System.Windows.Forms.Panel();
      this.panelBottom.SuspendLayout();
      this.quickTab.SuspendLayout();
      this.tabVOL.SuspendLayout();
      this.tabVOM.SuspendLayout();
      this.tabAdditional.SuspendLayout();
      this.panelInstruction.SuspendLayout();
      this.panelMiddle.SuspendLayout();
      this.SuspendLayout();
      this.panelBottom.Controls.Add((System.Windows.Forms.Control) this.btnOK);
      this.panelBottom.Controls.Add((System.Windows.Forms.Control) this.emHelpLink1);
      this.panelBottom.Controls.Add((System.Windows.Forms.Control) this.btnClose);
      this.panelBottom.Dock = DockStyle.Bottom;
      this.panelBottom.Location = new Point(0, 439);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new Size(682, 48);
      this.panelBottom.TabIndex = 7;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(513, 12);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 24);
      this.btnOK.TabIndex = 10;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.Location = new Point(12, 12);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 9;
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.Location = new Point(594, 12);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 24);
      this.btnClose.TabIndex = 8;
      this.btnClose.Text = "&Close";
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.workPanel.Location = new Point(455, 85);
      this.workPanel.Name = "workPanel";
      this.workPanel.Size = new Size(120, 84);
      this.workPanel.TabIndex = 6;
      this.quickTab.Controls.Add((System.Windows.Forms.Control) this.tabVOL);
      this.quickTab.Controls.Add((System.Windows.Forms.Control) this.tabVOM);
      this.quickTab.Controls.Add((System.Windows.Forms.Control) this.tabAdditional);
      this.quickTab.Location = new Point(15, 19);
      this.quickTab.Name = "quickTab";
      this.quickTab.SelectedIndex = 0;
      this.quickTab.Size = new Size(404, 272);
      this.quickTab.TabIndex = 8;
      this.tabVOL.Controls.Add((System.Windows.Forms.Control) this.volPanel);
      this.tabVOL.Location = new Point(4, 22);
      this.tabVOL.Name = "tabVOL";
      this.tabVOL.Size = new Size(396, 246);
      this.tabVOL.TabIndex = 0;
      this.tabVOL.Text = "VOL";
      this.volPanel.Dock = DockStyle.Fill;
      this.volPanel.Location = new Point(0, 0);
      this.volPanel.Name = "volPanel";
      this.volPanel.Size = new Size(396, 246);
      this.volPanel.TabIndex = 7;
      this.tabVOM.Controls.Add((System.Windows.Forms.Control) this.vomPanel);
      this.tabVOM.Location = new Point(4, 22);
      this.tabVOM.Name = "tabVOM";
      this.tabVOM.Size = new Size(396, 246);
      this.tabVOM.TabIndex = 1;
      this.tabVOM.Text = "VOM";
      this.vomPanel.Dock = DockStyle.Fill;
      this.vomPanel.Location = new Point(0, 0);
      this.vomPanel.Name = "vomPanel";
      this.vomPanel.Size = new Size(396, 246);
      this.vomPanel.TabIndex = 7;
      this.tabAdditional.Controls.Add((System.Windows.Forms.Control) this.additionalPanel);
      this.tabAdditional.Location = new Point(4, 22);
      this.tabAdditional.Name = "tabAdditional";
      this.tabAdditional.Size = new Size(396, 246);
      this.tabAdditional.TabIndex = 2;
      this.tabAdditional.Text = "Additional";
      this.additionalPanel.Dock = DockStyle.Fill;
      this.additionalPanel.Location = new Point(0, 0);
      this.additionalPanel.Name = "additionalPanel";
      this.additionalPanel.Size = new Size(396, 246);
      this.additionalPanel.TabIndex = 7;
      this.panelInstruction.Controls.Add((System.Windows.Forms.Control) this.labelInstruction);
      this.panelInstruction.Dock = DockStyle.Top;
      this.panelInstruction.Location = new Point(0, 0);
      this.panelInstruction.Name = "panelInstruction";
      this.panelInstruction.Size = new Size(682, 44);
      this.panelInstruction.TabIndex = 9;
      this.labelInstruction.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.labelInstruction.Location = new Point(12, 9);
      this.labelInstruction.Name = "labelInstruction";
      this.labelInstruction.Size = new Size(667, 30);
      this.labelInstruction.TabIndex = 0;
      this.labelInstruction.Text = "For loans planned for GSE delivery, enter details for Sections M and N to ensure data points required for the UCD export are included in the loan file.";
      this.panelMiddle.Controls.Add((System.Windows.Forms.Control) this.quickTab);
      this.panelMiddle.Controls.Add((System.Windows.Forms.Control) this.workPanel);
      this.panelMiddle.Dock = DockStyle.Fill;
      this.panelMiddle.Location = new Point(0, 44);
      this.panelMiddle.Name = "panelMiddle";
      this.panelMiddle.Size = new Size(682, 395);
      this.panelMiddle.TabIndex = 10;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(682, 487);
      this.Controls.Add((System.Windows.Forms.Control) this.panelMiddle);
      this.Controls.Add((System.Windows.Forms.Control) this.panelInstruction);
      this.Controls.Add((System.Windows.Forms.Control) this.panelBottom);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (QuickEntryPopupDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Quick Entry";
      this.Closing += new CancelEventHandler(this.PrequalQuickLinkDialog_Closing);
      this.Deactivate += new EventHandler(this.QuickEntryPopupDialog_Deactivate);
      this.FormClosing += new FormClosingEventHandler(this.QuickEntryPopupDialog_FormClosing);
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.KeyPress += new KeyPressEventHandler(this.QuickEntryPopupDialog_KeyPress);
      this.panelBottom.ResumeLayout(false);
      this.quickTab.ResumeLayout(false);
      this.tabVOL.ResumeLayout(false);
      this.tabVOM.ResumeLayout(false);
      this.tabAdditional.ResumeLayout(false);
      this.panelInstruction.ResumeLayout(false);
      this.panelMiddle.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public void ShowVerifPanel(string verifType)
    {
      PanelBase newControl = (PanelBase) null;
      int num = 0;
      switch (verifType)
      {
        case "Disaster":
          if (this.disPan == null)
            this.disPan = new DISPanel(this.session.MainScreen, (IWorkArea) this);
          newControl = (PanelBase) this.disPan;
          num = this.loanData != null ? this.loanData.GetNumberOfDisasters() : 0;
          break;
        case "TAX4506":
          if (this.tax4506Pan == null)
            this.tax4506Pan = new TAX4506Panel(this.session.MainScreen, (IWorkArea) this);
          newControl = (PanelBase) this.tax4506Pan;
          num = this.loanData != null ? this.loanData.GetNumberOfTAX4506Ts(true) : 0;
          break;
        case "TAX4506T":
          if (this.tax4506TPan == null)
            this.tax4506TPan = new TAX4506TPanel(this.session.MainScreen, (IWorkArea) this);
          newControl = (PanelBase) this.tax4506TPan;
          num = this.loanData != null ? this.loanData.GetNumberOfTAX4506Ts(false) : 0;
          break;
        case "VOAL":
          if (this.voalPan == null)
            this.voalPan = this.fieldSource != FieldSource.LinkedLoan ? new VOALPanel(this.session.MainScreen, (IWorkArea) this) : new VOALPanel(this.session.MainScreen, (IWorkArea) this, this.loanData);
          newControl = (PanelBase) this.voalPan;
          num = this.loanData != null ? this.loanData.GetNumberOfAdditionalLoans() : 0;
          break;
        case "VOD":
          if (this.vodPan == null)
            this.vodPan = new VODPanel(this.session.MainScreen, (IWorkArea) this);
          newControl = (PanelBase) this.vodPan;
          num = this.loanData != null ? this.loanData.GetNumberOfDeposits() : 0;
          break;
        case "VOE":
          if (this.voePan == null)
            this.voePan = this.fieldSource != FieldSource.LinkedLoan ? new VOEPanel(this.session.MainScreen, (IWorkArea) this) : new VOEPanel(this.session.MainScreen, (IWorkArea) this, this.loanData);
          newControl = (PanelBase) this.voePan;
          num = this.loanData != null ? this.loanData.GetNumberOfEmployer(true) + this.loanData.GetNumberOfEmployer(false) : 0;
          break;
        case "VOGG":
          if (this.voggPan == null)
            this.voggPan = new VOGGPanel(this.session.MainScreen, (IWorkArea) this);
          newControl = (PanelBase) this.voggPan;
          num = this.loanData != null ? this.loanData.GetNumberOfGiftsAndGrants() : 0;
          break;
        case "VOL":
          if (this.volPan == null)
            this.volPan = this.fieldSource != FieldSource.LinkedLoan ? new VOLPanel(this.session.MainScreen, (IWorkArea) this) : new VOLPanel(this.session.MainScreen, (IWorkArea) this, this.loanData);
          newControl = (PanelBase) this.volPan;
          num = this.loanData != null ? this.loanData.GetNumberOfLiabilitesExlcudingAlimonyJobExp() : 0;
          break;
        case "VOM":
          if (this.vomPan == null)
            this.vomPan = this.fieldSource != FieldSource.LinkedLoan ? new VOMPanel(this.session.MainScreen, (IWorkArea) this) : new VOMPanel(this.session.MainScreen, (IWorkArea) this, this.loanData);
          newControl = (PanelBase) this.vomPan;
          num = this.loanData != null ? this.loanData.GetNumberOfMortgages() : 0;
          break;
        case "VOOA":
          if (this.vooaPan == null)
            this.vooaPan = new VOOAPanel(this.session.MainScreen, (IWorkArea) this);
          newControl = (PanelBase) this.vooaPan;
          num = this.loanData != null ? this.loanData.GetNumberOfOtherAssets() : 0;
          break;
        case "VOOI":
          if (this.vooiPan == null)
            this.vooiPan = new VOOIPanel(this.session.MainScreen, (IWorkArea) this);
          newControl = (PanelBase) this.vooiPan;
          num = this.loanData != null ? this.loanData.GetNumberOfOtherIncomeSources() : 0;
          break;
        case "VOOL":
          if (this.voolPan == null)
            this.voolPan = new VOOLPanel(this.session.MainScreen, (IWorkArea) this);
          newControl = (PanelBase) this.voolPan;
          num = this.loanData != null ? this.loanData.GetNumberOfOtherLiability() : 0;
          break;
        case "VOR":
          if (this.vorPan == null)
            this.vorPan = this.fieldSource != FieldSource.LinkedLoan ? new VORPanel(this.session.MainScreen, (IWorkArea) this) : new VORPanel(this.session.MainScreen, (IWorkArea) this, this.loanData);
          newControl = (PanelBase) this.vorPan;
          num = this.loanData != null ? this.loanData.GetNumberOfResidence(true) + this.loanData.GetNumberOfResidence(false) : 0;
          break;
      }
      this.AddToWorkArea((System.Windows.Forms.Control) newControl);
      newControl.RefreshListView((object) null, (EventArgs) null);
      if (num <= 0)
        return;
      newControl.OpenVerification(0);
    }

    public void AddToWorkArea(System.Windows.Forms.Control newControl)
    {
      System.Windows.Forms.Panel panel;
      if (this.htmFile == "VOLPanel/VOMPanel")
      {
        switch (newControl)
        {
          case VOM _:
          case VOMPanel _:
            panel = this.vomPanel;
            break;
          case VOL _:
          case VOLPanel _:
            panel = this.volPanel;
            break;
          default:
            panel = this.additionalPanel;
            break;
        }
      }
      else
        panel = this.workPanel;
      panel.Controls.Clear();
      if (newControl is System.Windows.Forms.Form)
        newControl.Parent = (System.Windows.Forms.Control) panel;
      panel.Controls.Add(newControl);
      newControl.Focus();
    }

    public void RemoveFromWorkArea() => this.Close();

    public void RefreshContents()
    {
      this.freeScreen.RefreshContents();
      if (this.freeScreen2 == null)
        return;
      this.freeScreen2.RefreshContents();
    }

    public bool ContainsControl(System.Windows.Forms.Control con)
    {
      return this.workPanel.Controls.Contains(con);
    }

    private void PrequalQuickLinkDialog_Closing(object sender, CancelEventArgs e)
    {
      if (this.formTitle == "Adjustments and Other Credits Details" && this.freeScreen != null && this.freeScreen2 != null)
      {
        InputHandlerBase inputHandler1 = this.freeScreen.GetInputHandler() as InputHandlerBase;
        InputHandlerBase inputHandler2 = this.freeScreen2.GetInputHandler() as InputHandlerBase;
        if (this.loanData != null)
        {
          string field1 = this.loanData.GetField("CD3.X1504");
          string field2 = this.loanData.GetField("CD3.X1505");
          if ((string.IsNullOrEmpty(field1) || string.IsNullOrEmpty(field2)) && inputHandler1 != null && inputHandler2 != null && (inputHandler1.IsAnyFieldTouched || inputHandler2.IsAnyFieldTouched))
            this.loanData.Calculator.FormCalculation("ADJUSTMENTS_AND_OTHER_CREDITS", (string) null, (string) null);
        }
      }
      if (this.freeScreen != null)
        this.freeScreen.Dispose();
      if (this.freeScreen2 == null)
        return;
      this.freeScreen2.Dispose();
    }

    private void QuickEntryPopupDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.Close();
    }

    public void ShowHelp(System.Windows.Forms.Control control) => this.ShowHelp();

    public void ShowHelp(System.Windows.Forms.Control control, string helpTargetName)
    {
      this.ShowHelp();
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

    public System.Windows.Forms.Form OpenURL(
      string windowName,
      string url,
      string title,
      int width,
      int height)
    {
      return (System.Windows.Forms.Form) null;
    }

    public bool IsClientEnabledToExportFNMFRE => false;

    public bool IsUnderwriterSummaryAccessibleForBroker => false;

    public void RefreshCE()
    {
    }

    public void NavigateToTradesTab(int tradeId)
    {
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    internal void SetFieldsReadonly()
    {
      this.freeScreen.SetFieldsReadonly();
      if (this.freeScreen2 == null)
        return;
      this.freeScreen2.SetFieldsReadonly();
    }

    internal void InputConfirmationIsRequired()
    {
      this.btnOK.Visible = true;
      this.btnClose.Text = "&Cancel";
      this.inputConfirmationRequired = true;
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      if (!this.inputConfirmationRequired)
      {
        this.DialogResult = DialogResult.OK;
        if (this.OkClicked == null)
          return;
        this.OkClicked((object) this, new EventArgs());
      }
      else
        this.DialogResult = DialogResult.Cancel;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.dataIsValid = true;
      if (this.OkClicked != null)
        this.OkClicked((object) this, new EventArgs());
      if (!this.dataIsValid)
        return;
      this.DialogResult = DialogResult.OK;
    }

    public bool DataIsValid
    {
      set => this.dataIsValid = value;
    }

    private void QuickEntryPopupDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.loanData != null)
      {
        this.Focus();
        if (this.loanData.Calculator != null && this.formTitle == "VOL")
        {
          this.loanData.Calculator.FormCalculation("CLOSINGDISCLOSUREPAGE3", "", "");
          this.loanData.Calculator.FormCalculation("CASASRN.X168", "", "");
        }
      }
      if (this.session == null || this.session.Application == null)
        return;
      this.session.Application.GetService<ILoanEditor>()?.RefreshContents();
    }

    public void DisplayTPOCompanySetting(ExternalOriginatorManagementData o)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void HideAllButtons()
    {
      this.panelBottom.Visible = false;
      Size size = this.Size;
      int width = size.Width;
      size = this.Size;
      int height1 = size.Height;
      size = this.panelBottom.Size;
      int height2 = size.Height;
      int height3 = height1 - height2;
      this.Size = new Size(width, height3);
    }

    public IInputHandler GetInputHandler() => this.freeScreen.GetInputHandler();

    private void freeScreen_FormLoaded(object sender, EventArgs e)
    {
      if (this.LoanScreenLoaded == null)
        return;
      this.LoanScreenLoaded((object) this, e);
    }

    private void freeScreen_OnFieldChanged(object sender, EventArgs e)
    {
      if (this.OnFieldChanged == null)
        return;
      this.OnFieldChanged(sender, e);
    }

    private void freeScreen_ButtonClicked(object sender, EventArgs e)
    {
      if (this.ButtonClicked == null)
        return;
      this.ButtonClicked(sender, e);
    }

    public string GetHelpTargetName()
    {
      return !string.IsNullOrEmpty(this.emHelpLink1.HelpTag) ? this.emHelpLink1.HelpTag : "";
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((System.Windows.Forms.Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
    }

    public string HUDLineNo
    {
      get => this.hudLineNo;
      set => this.hudLineNo = value;
    }

    private void QuickEntryPopupDialog_Deactivate(object sender, EventArgs e)
    {
      if (this.OnDialogDeactivated == null)
        return;
      if (this.freeScreen != null && this.freeScreen.CurrentForm != (InputFormInfo) null && this.freeScreen.CurrentForm.FormID == "REGZGFE_2015_Details")
        ((InputHandlerBase) this.freeScreen.GetInputHandler())?.UpdateCurrentField();
      this.OnDialogDeactivated(sender, e);
    }
  }
}

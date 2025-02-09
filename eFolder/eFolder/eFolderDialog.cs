// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eFolderDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Conditions;
using EllieMae.EMLite.eFolder.Documents;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.eFolder.History;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.eFolder.ThinThick;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.eFolder
{
  public class eFolderDialog : Form, IOnlineHelpTarget
  {
    private const string className = "eFolderDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private static eFolderDialog _instance = (eFolderDialog) null;
    private LoanDataMgr loanDataMgr;
    private Sessions.Session session;
    private DocumentTrackingControl documentTracking;
    private ConditionTrackingControl preliminaryTracking;
    private ConditionTrackingControl underwritingTracking;
    private ConditionTrackingControl postClosingTracking;
    private ConditionTrackingControl sellTracking;
    private EnhancedConditionsTrackingControl enhancedTracking;
    private ThinThickControl packagesApp;
    private HistoryTrackingControl historyTracking;
    private ThinThickControl eNoteApp;
    private ToolStripDropDown helpDropDown;
    private bool refreshSelectedTab;
    private bool isClosing;
    private IContainer components;
    private GradientMenuStrip mnuMain;
    private TabControl tabMain;
    private TabPage documentTrackingPage;
    private TabPage preliminaryTrackingPage;
    private TabPage enhancedTrackingPage;
    private TabPage underwritingTrackingPage;
    private TabPage postClosingTrackingPage;
    private TabPage sellTrackingPage;
    private Panel pnlClose;
    private Button btnClose;
    private ToolStripMenuItem mnueFolder;
    private ToolStripMenuItem mnuHelp;
    private ToolStripMenuItem mnuClose;
    private Panel pnlMain;
    private ToolStripMenuItem mnuDocuments;
    private ToolStripMenuItem mnuPreliminary;
    private ToolStripMenuItem mnuUnderwriting;
    private ToolStripMenuItem mnuPostClosing;
    private ToolStripMenuItem mnuEnhanced;
    private ToolStripMenuItem mnuSell;
    private ToolStripSeparator mnuHelpPlaceholder;
    private ToolStripMenuItem helpToolStripMenuItem;
    private EMHelpLink helpLink;
    private TabPage historyTrackingPage;
    private ToolStripMenuItem mnuHistory;
    private TabPage packagesPage;
    private TabPage eNotePage;

    public ConditionLog ImportedCondition { get; set; }

    public static void ShowInstance(Sessions.Session session)
    {
      eFolderDialog.ShowInstance(session, false);
    }

    public static void ShowInstance(Sessions.Session session, bool showConditionTabe)
    {
      if (eFolderDialog._instance == null)
      {
        eFolderDialog._instance = new eFolderDialog(session.LoanDataMgr, session);
        eFolderDialog._instance.FormClosing += new FormClosingEventHandler(eFolderDialog._instance_FormClosing);
        eFolderDialog._instance.Show();
      }
      else
      {
        if (eFolderDialog._instance.WindowState == FormWindowState.Minimized)
          eFolderDialog._instance.WindowState = FormWindowState.Normal;
        eFolderDialog._instance.Activate();
      }
      if (!showConditionTabe || eFolderDialog._instance == null)
        return;
      if (session.LoanDataMgr.LoanData.EnableEnhancedConditions && eFolderDialog._instance.tabMain.SelectedTab != eFolderDialog._instance.enhancedTrackingPage)
      {
        eFolderDialog._instance.tabMain.SelectedTab = eFolderDialog._instance.enhancedTrackingPage;
      }
      else
      {
        if (session.LoanDataMgr.LoanData.EnableEnhancedConditions || eFolderDialog._instance.tabMain.SelectedTab == eFolderDialog._instance.underwritingTrackingPage)
          return;
        eFolderDialog._instance.tabMain.SelectedTab = eFolderDialog._instance.underwritingTrackingPage;
      }
    }

    public static void RefreshLoanContents()
    {
      if (eFolderDialog._instance == null)
        return;
      TabPage selectedTab = eFolderDialog._instance.tabMain.SelectedTab;
      if (selectedTab == eFolderDialog._instance.documentTrackingPage)
        eFolderDialog._instance.documentTracking.RefreshLoanContents();
      else if (selectedTab == eFolderDialog._instance.preliminaryTrackingPage)
        eFolderDialog._instance.preliminaryTracking.RefreshLoanContents();
      else if (selectedTab == eFolderDialog._instance.enhancedTrackingPage)
        eFolderDialog._instance.enhancedTracking.RefreshLoanContents();
      else if (selectedTab == eFolderDialog._instance.underwritingTrackingPage)
        eFolderDialog._instance.underwritingTracking.RefreshLoanContents();
      else if (selectedTab == eFolderDialog._instance.postClosingTrackingPage)
        eFolderDialog._instance.postClosingTracking.RefreshLoanContents();
      else if (selectedTab == eFolderDialog._instance.sellTrackingPage)
      {
        eFolderDialog._instance.sellTracking.RefreshLoanContents();
      }
      else
      {
        if (selectedTab != eFolderDialog._instance.historyTrackingPage)
          return;
        eFolderDialog._instance.historyTracking.RefreshLoanContents();
      }
    }

    public static void SelectedTabFromImport(
      ConditionType conditionType,
      ConditionLog importedCondition,
      ThinThickType thinThickType = ThinThickType.eNote)
    {
      if (eFolderDialog._instance == null)
        return;
      eFolderDialog._instance.ImportedCondition = importedCondition;
      eFolderDialog._instance.tabMain.SelectedTab = (TabPage) null;
      switch (conditionType)
      {
        case ConditionType.Unknown:
          if (thinThickType != ThinThickType.eNote)
            break;
          eFolderDialog._instance.tabMain.SelectedTab = eFolderDialog._instance.eNotePage;
          break;
        case ConditionType.Underwriting:
          eFolderDialog._instance.tabMain.SelectedTab = eFolderDialog._instance.underwritingTrackingPage;
          break;
        case ConditionType.PostClosing:
          eFolderDialog._instance.tabMain.SelectedTab = eFolderDialog._instance.postClosingTrackingPage;
          break;
        case ConditionType.Preliminary:
          eFolderDialog._instance.tabMain.SelectedTab = eFolderDialog._instance.preliminaryTrackingPage;
          break;
        case ConditionType.Sell:
          eFolderDialog._instance.tabMain.SelectedTab = eFolderDialog._instance.sellTrackingPage;
          break;
        case ConditionType.Enhanced:
          eFolderDialog._instance.tabMain.SelectedTab = eFolderDialog._instance.enhancedTrackingPage;
          break;
      }
    }

    public static bool Minimize()
    {
      try
      {
        if (eFolderDialog._instance != null)
        {
          eFolderDialog._instance.WindowState = FormWindowState.Minimized;
          return true;
        }
      }
      catch
      {
      }
      return false;
    }

    public static FileSystemEntry SelectedStackingOrder()
    {
      return eFolderDialog._instance != null ? eFolderDialog._instance.documentTracking.SelectedStackingOrder : (FileSystemEntry) null;
    }

    public static DocumentLog[] GetSelectedDocuments()
    {
      return eFolderDialog._instance != null ? eFolderDialog._instance.documentTracking.getSelectedDocuments() : (DocumentLog[]) null;
    }

    private static void _instance_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!eFolderDialog._instance.IsDisposed && !eFolderDialog._instance.Disposing)
      {
        eFolderDialog._instance.Visible = false;
        eFolderDialog._instance.Dispose();
      }
      eFolderDialog._instance = (eFolderDialog) null;
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
      try
      {
        base.OnFormClosed(e);
      }
      catch (Exception ex)
      {
      }
    }

    public eFolderDialog(LoanDataMgr loanDataMgr, Sessions.Session session)
    {
      this.InitializeComponent();
      this.setWindowSize();
      this.loanDataMgr = loanDataMgr;
      this.session = session;
      this.documentTracking = new DocumentTrackingControl(loanDataMgr, this.session);
      this.documentTrackingPage.Controls.Add((Control) this.documentTracking);
      this.documentTracking.Dock = DockStyle.Fill;
      this.documentTracking.Visible = true;
      if (loanDataMgr.LoanData.EnableEnhancedConditions)
      {
        this.enhancedTracking = new EnhancedConditionsTrackingControl(loanDataMgr, this.session, (Form) this);
        this.enhancedTrackingPage.Controls.Add((Control) this.enhancedTracking);
        this.enhancedTracking.Dock = DockStyle.Fill;
      }
      else
      {
        this.preliminaryTracking = new ConditionTrackingControl(loanDataMgr, ConditionType.Preliminary, this.session, (Form) this);
        this.preliminaryTrackingPage.Controls.Add((Control) this.preliminaryTracking);
        this.preliminaryTracking.Dock = DockStyle.Fill;
        this.underwritingTracking = new ConditionTrackingControl(loanDataMgr, ConditionType.Underwriting, this.session, (Form) this);
        this.underwritingTrackingPage.Controls.Add((Control) this.underwritingTracking);
        this.underwritingTracking.Dock = DockStyle.Fill;
        this.postClosingTracking = new ConditionTrackingControl(loanDataMgr, ConditionType.PostClosing, this.session, (Form) this);
        this.postClosingTrackingPage.Controls.Add((Control) this.postClosingTracking);
        this.postClosingTracking.Dock = DockStyle.Fill;
        this.sellTracking = new ConditionTrackingControl(loanDataMgr, ConditionType.Sell, this.session, (Form) this);
        this.sellTrackingPage.Controls.Add((Control) this.sellTracking);
        this.sellTracking.Dock = DockStyle.Fill;
      }
      this.packagesApp = new ThinThickControl(loanDataMgr, Session.SessionObjects.StartupInfo.eSignPackagesUrl, ThinThickType.eSignPackages);
      this.packagesPage.Controls.Add((Control) this.packagesApp);
      this.packagesApp.Dock = DockStyle.Fill;
      this.historyTracking = new HistoryTrackingControl(loanDataMgr);
      this.historyTrackingPage.Controls.Add((Control) this.historyTracking);
      this.historyTracking.Dock = DockStyle.Fill;
      this.eNoteApp = new ThinThickControl(loanDataMgr, Session.SessionObjects.StartupInfo.eNoteUrl, ThinThickType.eNote);
      this.eNotePage.Controls.Add((Control) this.eNoteApp);
      this.eNoteApp.Dock = DockStyle.Fill;
      this.helpDropDown = this.mnuHelp.DropDown;
      this.initEventHandlers();
      this.applySecurity();
    }

    private void setWindowSize()
    {
      if (Form.ActiveForm != null)
      {
        Form form = Form.ActiveForm;
        while (form.Owner != null)
          form = form.Owner;
        this.Width = Convert.ToInt32((double) form.Width * 0.95);
        this.Height = Convert.ToInt32((double) form.Height * 0.95);
      }
      else
      {
        Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Width = Convert.ToInt32((double) workingArea.Width * 0.95);
        workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Height = Convert.ToInt32((double) workingArea.Height * 0.95);
      }
    }

    private void applySecurity()
    {
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr);
      if (!folderAccessRights.CanAccessDocumentTab)
      {
        this.tabMain.TabPages.Remove(this.documentTrackingPage);
        this.documentTrackingPage.Dispose();
      }
      if (!folderAccessRights.CanAccessPreliminaryTab || this.loanDataMgr.LoanData.EnableEnhancedConditions)
      {
        this.tabMain.TabPages.Remove(this.preliminaryTrackingPage);
        this.preliminaryTrackingPage.Dispose();
      }
      if (!folderAccessRights.CanAccessUnderwritingTab || this.loanDataMgr.LoanData.EnableEnhancedConditions)
      {
        this.tabMain.TabPages.Remove(this.underwritingTrackingPage);
        this.underwritingTrackingPage.Dispose();
      }
      if (!folderAccessRights.CanAccessPostClosingTab || this.loanDataMgr.LoanData.EnableEnhancedConditions)
      {
        this.tabMain.TabPages.Remove(this.postClosingTrackingPage);
        this.postClosingTrackingPage.Dispose();
      }
      if (!folderAccessRights.CanAccessSellTab || this.loanDataMgr.LoanData.EnableEnhancedConditions)
      {
        this.tabMain.TabPages.Remove(this.sellTrackingPage);
        this.sellTrackingPage.Dispose();
      }
      if (!this.loanDataMgr.LoanData.EnableEnhancedConditions)
      {
        this.tabMain.TabPages.Remove(this.enhancedTrackingPage);
        this.enhancedTrackingPage.Dispose();
      }
      if (!folderAccessRights.CanAccessPackagesTab)
      {
        this.tabMain.TabPages.Remove(this.packagesPage);
        this.packagesPage.Dispose();
      }
      if (!folderAccessRights.CanAccessHistoryTab)
      {
        this.tabMain.TabPages.Remove(this.historyTrackingPage);
        this.historyTrackingPage.Dispose();
      }
      if (this.session.SettingsManager.GetServerSetting("eClose.AllowHybridWithENoteClosing").ToString() != EnableDisableSetting.Enabled.ToString() || !folderAccessRights.CanAccessENoteTab)
      {
        this.tabMain.TabPages.Remove(this.eNotePage);
        this.eNotePage.Dispose();
      }
      this.refreshMenu();
      this.refreshTab();
    }

    private void refreshMenu()
    {
      TabPage selectedTab = this.tabMain.SelectedTab;
      if (selectedTab == this.documentTrackingPage)
      {
        this.mnuDocuments.Visible = true;
        this.mnuDocuments.DropDown = this.documentTracking.Menu;
        this.mnuPreliminary.Visible = false;
        this.mnuPreliminary.DropDown = (ToolStripDropDown) null;
        this.mnuEnhanced.Visible = false;
        this.mnuEnhanced.DropDown = (ToolStripDropDown) null;
        this.mnuUnderwriting.Visible = false;
        this.mnuUnderwriting.DropDown = (ToolStripDropDown) null;
        this.mnuPostClosing.Visible = false;
        this.mnuPostClosing.DropDown = (ToolStripDropDown) null;
        this.mnuSell.Visible = false;
        this.mnuSell.DropDown = (ToolStripDropDown) null;
        this.mnuHistory.Visible = false;
        this.mnuHistory.DropDown = (ToolStripDropDown) null;
      }
      else if (selectedTab == this.preliminaryTrackingPage)
      {
        this.mnuDocuments.Visible = false;
        this.mnuDocuments.DropDown = (ToolStripDropDown) null;
        this.mnuPreliminary.Visible = true;
        this.mnuPreliminary.DropDown = this.preliminaryTracking.Menu;
        this.mnuEnhanced.Visible = false;
        this.mnuEnhanced.DropDown = (ToolStripDropDown) null;
        this.mnuUnderwriting.Visible = false;
        this.mnuUnderwriting.DropDown = (ToolStripDropDown) null;
        this.mnuPostClosing.Visible = false;
        this.mnuPostClosing.DropDown = (ToolStripDropDown) null;
        this.mnuSell.Visible = false;
        this.mnuSell.DropDown = (ToolStripDropDown) null;
        this.mnuHistory.Visible = false;
        this.mnuHistory.DropDown = (ToolStripDropDown) null;
      }
      else if (selectedTab == this.underwritingTrackingPage)
      {
        this.mnuDocuments.Visible = false;
        this.mnuDocuments.DropDown = (ToolStripDropDown) null;
        this.mnuPreliminary.Visible = false;
        this.mnuPreliminary.DropDown = (ToolStripDropDown) null;
        this.mnuEnhanced.Visible = false;
        this.mnuEnhanced.DropDown = (ToolStripDropDown) null;
        this.mnuUnderwriting.Visible = true;
        this.mnuUnderwriting.DropDown = this.underwritingTracking.Menu;
        this.mnuPostClosing.Visible = false;
        this.mnuPostClosing.DropDown = (ToolStripDropDown) null;
        this.mnuSell.Visible = false;
        this.mnuSell.DropDown = (ToolStripDropDown) null;
        this.mnuHistory.Visible = false;
        this.mnuHistory.DropDown = (ToolStripDropDown) null;
      }
      else if (selectedTab == this.postClosingTrackingPage)
      {
        this.mnuDocuments.Visible = false;
        this.mnuDocuments.DropDown = (ToolStripDropDown) null;
        this.mnuPreliminary.Visible = false;
        this.mnuPreliminary.DropDown = (ToolStripDropDown) null;
        this.mnuEnhanced.Visible = false;
        this.mnuEnhanced.DropDown = (ToolStripDropDown) null;
        this.mnuUnderwriting.Visible = false;
        this.mnuUnderwriting.DropDown = (ToolStripDropDown) null;
        this.mnuPostClosing.Visible = true;
        this.mnuPostClosing.DropDown = this.postClosingTracking.Menu;
        this.mnuSell.Visible = false;
        this.mnuSell.DropDown = (ToolStripDropDown) null;
        this.mnuHistory.Visible = false;
        this.mnuHistory.DropDown = (ToolStripDropDown) null;
      }
      else if (selectedTab == this.sellTrackingPage)
      {
        this.mnuDocuments.Visible = false;
        this.mnuDocuments.DropDown = (ToolStripDropDown) null;
        this.mnuPreliminary.Visible = false;
        this.mnuPreliminary.DropDown = (ToolStripDropDown) null;
        this.mnuEnhanced.Visible = false;
        this.mnuEnhanced.DropDown = (ToolStripDropDown) null;
        this.mnuUnderwriting.Visible = false;
        this.mnuUnderwriting.DropDown = (ToolStripDropDown) null;
        this.mnuPostClosing.Visible = false;
        this.mnuPostClosing.DropDown = (ToolStripDropDown) null;
        this.mnuSell.Visible = true;
        this.mnuSell.DropDown = this.sellTracking.Menu;
        this.mnuHistory.Visible = false;
        this.mnuHistory.DropDown = (ToolStripDropDown) null;
      }
      else if (selectedTab == this.historyTrackingPage)
      {
        this.mnuDocuments.Visible = false;
        this.mnuDocuments.DropDown = (ToolStripDropDown) null;
        this.mnuPreliminary.Visible = false;
        this.mnuPreliminary.DropDown = (ToolStripDropDown) null;
        this.mnuEnhanced.Visible = false;
        this.mnuEnhanced.DropDown = (ToolStripDropDown) null;
        this.mnuUnderwriting.Visible = false;
        this.mnuUnderwriting.DropDown = (ToolStripDropDown) null;
        this.mnuPostClosing.Visible = false;
        this.mnuPostClosing.DropDown = (ToolStripDropDown) null;
        this.mnuSell.Visible = false;
        this.mnuSell.DropDown = (ToolStripDropDown) null;
        this.mnuHistory.Visible = true;
        this.mnuHistory.DropDown = this.historyTracking.Menu;
      }
      else if (selectedTab == this.enhancedTrackingPage)
      {
        this.mnuDocuments.Visible = false;
        this.mnuDocuments.DropDown = (ToolStripDropDown) null;
        this.mnuEnhanced.Visible = true;
        this.mnuEnhanced.DropDown = this.enhancedTracking.Menu;
        this.mnuPreliminary.Visible = false;
        this.mnuPreliminary.DropDown = (ToolStripDropDown) null;
        this.mnuUnderwriting.Visible = false;
        this.mnuUnderwriting.DropDown = (ToolStripDropDown) null;
        this.mnuPostClosing.Visible = false;
        this.mnuPostClosing.DropDown = (ToolStripDropDown) null;
        this.mnuSell.Visible = false;
        this.mnuSell.DropDown = (ToolStripDropDown) null;
        this.mnuHistory.Visible = false;
        this.mnuHistory.DropDown = (ToolStripDropDown) null;
      }
      else
      {
        this.mnuDocuments.Visible = false;
        this.mnuDocuments.DropDown = (ToolStripDropDown) null;
        this.mnuPreliminary.Visible = false;
        this.mnuPreliminary.DropDown = (ToolStripDropDown) null;
        this.mnuEnhanced.Visible = false;
        this.mnuEnhanced.DropDown = (ToolStripDropDown) null;
        this.mnuUnderwriting.Visible = false;
        this.mnuUnderwriting.DropDown = (ToolStripDropDown) null;
        this.mnuPostClosing.Visible = false;
        this.mnuPostClosing.DropDown = (ToolStripDropDown) null;
        this.mnuSell.Visible = false;
        this.mnuSell.DropDown = (ToolStripDropDown) null;
        this.mnuHistory.Visible = false;
        this.mnuHistory.DropDown = (ToolStripDropDown) null;
      }
    }

    private void refreshTab()
    {
      TabPage selectedTab = this.tabMain.SelectedTab;
      if (selectedTab == this.documentTrackingPage)
        this.documentTracking.RefreshContents();
      else if (selectedTab == this.preliminaryTrackingPage)
        this.preliminaryTracking.RefreshContents();
      else if (selectedTab == this.underwritingTrackingPage)
        this.underwritingTracking.RefreshContents();
      else if (selectedTab == this.postClosingTrackingPage)
        this.postClosingTracking.RefreshContents();
      else if (selectedTab == this.sellTrackingPage)
        this.sellTracking.RefreshContents();
      else if (selectedTab == this.enhancedTrackingPage)
      {
        this.enhancedTracking.RefreshContents();
      }
      else
      {
        if (selectedTab != this.historyTrackingPage)
          return;
        this.historyTracking.RefreshContents();
      }
    }

    private void tabMain_Selected(object sender, TabControlEventArgs e)
    {
      if (this.isClosing)
        return;
      this.refreshMenu();
      this.refreshTab();
    }

    private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.isClosing)
        return;
      TabPage selectedTab = this.tabMain.SelectedTab;
      if (selectedTab == this.packagesPage)
      {
        if (this.loanDataMgr.IsPlatformLoan())
          this.packagesApp.RefreshContents();
        else
          this.packagesApp.ShowMessage("Packages cannot be loaded at this time.");
      }
      else
      {
        if (selectedTab != this.eNotePage)
          return;
        this.eNoteApp.RefreshContents();
      }
    }

    private void mnuHelp_DropDownOpening(object sender, EventArgs e)
    {
      this.mnuHelp.DropDown = Session.Application.GetService<IEncompassApplication>().HelpDropDown;
    }

    private void mnuHelp_DropDownClosed(object sender, EventArgs e)
    {
      this.mnuHelp.DropDown = this.helpDropDown;
    }

    private void helpToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
    }

    private void initEventHandlers()
    {
      this.FormClosed += new FormClosedEventHandler(this.onFormClosed);
      this.loanDataMgr.LoanClosing += new EventHandler(this.btnClose_Click);
      this.loanDataMgr.BeforeLoanRefreshedFromServer += new EventHandler(this.beforeLoanRefreshedFromServer);
      this.loanDataMgr.OnLoanRefreshedFromServer += new EventHandler(this.onLoanRefreshedFromServer);
      this.loanDataMgr.LoanData.LogRecordAdded += new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.LoanData.LogRecordChanged += new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.LoanData.LogRecordRemoved += new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.FileAttachments.FileAttachmentAdded += new FileAttachmentEventHandler(this.fileAttachmentChanged);
      this.loanDataMgr.FileAttachments.FileAttachmentChanged += new FileAttachmentEventHandler(this.fileAttachmentChanged);
      this.loanDataMgr.FileAttachments.FileAttachmentRemoved += new FileAttachmentEventHandler(this.fileAttachmentChanged);
    }

    private void releaseEventHandlers()
    {
      this.FormClosed -= new FormClosedEventHandler(this.onFormClosed);
      this.loanDataMgr.LoanClosing -= new EventHandler(this.btnClose_Click);
      this.loanDataMgr.BeforeLoanRefreshedFromServer -= new EventHandler(this.beforeLoanRefreshedFromServer);
      this.loanDataMgr.OnLoanRefreshedFromServer -= new EventHandler(this.onLoanRefreshedFromServer);
      this.loanDataMgr.LoanData.LogRecordAdded -= new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.LoanData.LogRecordChanged -= new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.LoanData.LogRecordRemoved -= new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.FileAttachments.FileAttachmentAdded -= new FileAttachmentEventHandler(this.fileAttachmentChanged);
      this.loanDataMgr.FileAttachments.FileAttachmentChanged -= new FileAttachmentEventHandler(this.fileAttachmentChanged);
      this.loanDataMgr.FileAttachments.FileAttachmentRemoved -= new FileAttachmentEventHandler(this.fileAttachmentChanged);
    }

    private void eFolderDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.isClosing = true;
    }

    private void onFormClosed(object sender, FormClosedEventArgs e) => this.releaseEventHandlers();

    private void logRecordChanged(object source, LogRecordEventArgs e)
    {
      Tracing.Log(eFolderDialog.sw, TraceLevel.Verbose, nameof (eFolderDialog), "Checking InvokeRequired For LogRecordEventHandler");
      if (this.InvokeRequired)
      {
        LogRecordEventHandler method = new LogRecordEventHandler(this.logRecordChanged);
        Tracing.Log(eFolderDialog.sw, TraceLevel.Verbose, nameof (eFolderDialog), "Calling BeginInvoke For LogRecordEventHandler");
        this.BeginInvoke((Delegate) method, source, (object) e);
      }
      else
      {
        bool flag = false;
        if (e.LogRecord is DocumentLog)
          flag = true;
        else if (e.LogRecord is PreliminaryConditionLog)
        {
          if (this.tabMain.SelectedTab == this.preliminaryTrackingPage || this.tabMain.SelectedTab == this.historyTrackingPage)
            flag = true;
        }
        else if (e.LogRecord is UnderwritingConditionLog)
        {
          if (this.tabMain.SelectedTab == this.underwritingTrackingPage || this.tabMain.SelectedTab == this.historyTrackingPage)
            flag = true;
        }
        else if (e.LogRecord is PostClosingConditionLog)
        {
          if (this.tabMain.SelectedTab == this.postClosingTrackingPage || this.tabMain.SelectedTab == this.historyTrackingPage)
            flag = true;
        }
        else if (e.LogRecord is SellConditionLog)
        {
          if (this.tabMain.SelectedTab == this.sellTrackingPage || this.tabMain.SelectedTab == this.historyTrackingPage)
            flag = true;
        }
        else if (e.LogRecord is EnhancedConditionLog && (this.tabMain.SelectedTab == this.enhancedTrackingPage || this.tabMain.SelectedTab == this.historyTrackingPage))
          flag = true;
        if (!flag)
          return;
        if (this == Form.ActiveForm)
          this.refreshTab();
        else
          this.refreshSelectedTab = true;
      }
    }

    private void fileAttachmentChanged(object source, FileAttachmentEventArgs e)
    {
      Tracing.Log(eFolderDialog.sw, TraceLevel.Verbose, nameof (eFolderDialog), "Checking InvokeRequired For FileAttachmentEventHandler");
      if (this.InvokeRequired)
      {
        FileAttachmentEventHandler method = new FileAttachmentEventHandler(this.fileAttachmentChanged);
        Tracing.Log(eFolderDialog.sw, TraceLevel.Verbose, nameof (eFolderDialog), "Calling BeginInvoke For FileAttachmentEventHandler");
        this.BeginInvoke((Delegate) method, source, (object) e);
      }
      else
      {
        if (this.tabMain.SelectedTab != this.historyTrackingPage)
          return;
        if (this == Form.ActiveForm)
          this.refreshTab();
        else
          this.refreshSelectedTab = true;
      }
    }

    private void onLoanRefreshedFromServer(object sender, EventArgs e)
    {
      eFolderDialog.RefreshLoanContents();
    }

    private void beforeLoanRefreshedFromServer(object sender, EventArgs e)
    {
      if (!(this.session.GetPrivateProfileString("eFolder", "hideLoanRefreshWarning") == "Y"))
      {
        using (HideeFolderWarningMessageForm warningMessageForm = new HideeFolderWarningMessageForm())
        {
          int num = (int) warningMessageForm.ShowDialog((IWin32Window) this);
          if (warningMessageForm.HideMessage)
            this.session.WritePrivateProfileString("eFolder", "hideLoanRefreshWarning", "Y");
        }
      }
      if (this.documentTracking != null)
        this.documentTracking.CloseDetailInstances();
      if (this.loanDataMgr.LoanData.EnableEnhancedConditions)
      {
        if (this.enhancedTracking != null)
          this.enhancedTracking.CloseDetailInstances();
      }
      else
      {
        if (this.preliminaryTracking != null)
          this.preliminaryTracking.CloseDetailInstances();
        if (this.underwritingTracking != null)
          this.underwritingTracking.CloseDetailInstances();
        if (this.postClosingTracking != null)
          this.postClosingTracking.CloseDetailInstances();
        if (this.sellTracking != null)
          this.sellTracking.CloseDetailInstances();
      }
      this.session.LoanDataMgr.LauncheFolderNeeded = true;
      this.Close();
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    private void eFolderDialog_Activated(object sender, EventArgs e)
    {
      if (!this.refreshSelectedTab)
        return;
      this.refreshTab();
      this.refreshSelectedTab = false;
    }

    public void SetSelected(ConditionType condType)
    {
      switch (condType)
      {
        case ConditionType.Underwriting:
          this.tabMain.SelectTab(this.underwritingTrackingPage);
          break;
        case ConditionType.PostClosing:
          this.tabMain.SelectTab(this.postClosingTrackingPage);
          break;
        case ConditionType.Preliminary:
          this.tabMain.SelectTab(this.preliminaryTrackingPage);
          break;
        case ConditionType.Sell:
          this.tabMain.SelectTab(this.sellTrackingPage);
          break;
      }
    }

    public string GetHelpTargetName() => this.helpLink.HelpTag;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      if (disposing)
      {
        this.helpLink.Font.Dispose();
        this.Font.Dispose();
        this.DisposeCustomControl((Control) this.tabMain);
        this.DisposeCustomControl((Control) this.pnlMain);
        this.DisposeCustomControl((Control) this.mnuMain);
        this.DisposeCustomControl((Control) this.pnlClose);
      }
      base.Dispose(disposing);
    }

    private void DisposeCustomControl(Control myCtrl)
    {
      if (myCtrl.Controls != null && myCtrl.Controls.Count > 0)
      {
        List<Control> controlList = new List<Control>(myCtrl.Controls.Count);
        foreach (Control control in (ArrangedElementCollection) myCtrl.Controls)
        {
          if (!control.IsDisposed)
            controlList.Add(control);
        }
        controlList.ForEach((Action<Control>) (c => c.Dispose()));
      }
      if (!myCtrl.Controls.IsReadOnly)
        myCtrl.Controls.Clear();
      myCtrl.Dispose();
    }

    private void InitializeComponent()
    {
      this.mnuMain = new GradientMenuStrip();
      this.mnueFolder = new ToolStripMenuItem();
      this.mnuClose = new ToolStripMenuItem();
      this.mnuDocuments = new ToolStripMenuItem();
      this.mnuPreliminary = new ToolStripMenuItem();
      this.mnuUnderwriting = new ToolStripMenuItem();
      this.mnuPostClosing = new ToolStripMenuItem();
      this.mnuSell = new ToolStripMenuItem();
      this.mnuHistory = new ToolStripMenuItem();
      this.mnuHelp = new ToolStripMenuItem();
      this.mnuHelpPlaceholder = new ToolStripSeparator();
      this.helpToolStripMenuItem = new ToolStripMenuItem();
      this.mnuEnhanced = new ToolStripMenuItem();
      this.tabMain = new TabControl();
      this.documentTrackingPage = new TabPage();
      this.preliminaryTrackingPage = new TabPage();
      this.underwritingTrackingPage = new TabPage();
      this.postClosingTrackingPage = new TabPage();
      this.sellTrackingPage = new TabPage();
      this.enhancedTrackingPage = new TabPage();
      this.packagesPage = new TabPage();
      this.historyTrackingPage = new TabPage();
      this.eNotePage = new TabPage();
      this.pnlClose = new Panel();
      this.btnClose = new Button();
      this.pnlMain = new Panel();
      this.helpLink = new EMHelpLink();
      this.mnuMain.SuspendLayout();
      this.tabMain.SuspendLayout();
      this.pnlClose.SuspendLayout();
      this.pnlMain.SuspendLayout();
      this.SuspendLayout();
      this.mnuMain.Items.AddRange(new ToolStripItem[9]
      {
        (ToolStripItem) this.mnueFolder,
        (ToolStripItem) this.mnuDocuments,
        (ToolStripItem) this.mnuPreliminary,
        (ToolStripItem) this.mnuUnderwriting,
        (ToolStripItem) this.mnuPostClosing,
        (ToolStripItem) this.mnuSell,
        (ToolStripItem) this.mnuHistory,
        (ToolStripItem) this.mnuHelp,
        (ToolStripItem) this.mnuEnhanced
      });
      this.mnuMain.Location = new Point(0, 0);
      this.mnuMain.Name = "mnuMain";
      this.mnuMain.Padding = new Padding(1, 0, 0, 0);
      this.mnuMain.Size = new Size(755, 24);
      this.mnuMain.TabIndex = 0;
      this.mnueFolder.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.mnuClose
      });
      this.mnueFolder.Name = "mnueFolder";
      this.mnueFolder.Size = new Size(58, 24);
      this.mnueFolder.Text = "&eFolder";
      this.mnuClose.Name = "mnuClose";
      this.mnuClose.ShortcutKeys = Keys.F4 | Keys.Alt;
      this.mnuClose.Size = new Size(187, 22);
      this.mnuClose.Text = "&Close eFolder";
      this.mnuClose.Click += new EventHandler(this.btnClose_Click);
      this.mnuDocuments.Name = "mnuDocuments";
      this.mnuDocuments.Size = new Size(80, 24);
      this.mnuDocuments.Text = "Docu&ments";
      this.mnuPreliminary.Name = "mnuPreliminary";
      this.mnuPreliminary.Size = new Size(77, 24);
      this.mnuPreliminary.Text = "&Conditions";
      this.mnuUnderwriting.Name = "mnuUnderwriting";
      this.mnuUnderwriting.Size = new Size(77, 24);
      this.mnuUnderwriting.Text = "&Conditions";
      this.mnuPostClosing.Name = "mnuPostClosing";
      this.mnuPostClosing.Size = new Size(77, 24);
      this.mnuPostClosing.Text = "&Conditions";
      this.mnuSell.Name = "mnuSell";
      this.mnuSell.Size = new Size(77, 24);
      this.mnuSell.Text = "&Conditions";
      this.mnuHistory.Name = "mnuHistory";
      this.mnuHistory.Size = new Size(57, 24);
      this.mnuHistory.Text = "Histor&y";
      this.mnuHelp.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.mnuHelpPlaceholder,
        (ToolStripItem) this.helpToolStripMenuItem
      });
      this.mnuHelp.Name = "mnuHelp";
      this.mnuHelp.Size = new Size(44, 24);
      this.mnuHelp.Text = "&Help";
      this.mnuHelp.DropDownClosed += new EventHandler(this.mnuHelp_DropDownClosed);
      this.mnuHelp.DropDownOpening += new EventHandler(this.mnuHelp_DropDownOpening);
      this.mnuHelpPlaceholder.Name = "mnuHelpPlaceholder";
      this.mnuHelpPlaceholder.Size = new Size(115, 6);
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.ShortcutKeys = Keys.F1;
      this.helpToolStripMenuItem.Size = new Size(118, 22);
      this.helpToolStripMenuItem.Text = "&Help";
      this.helpToolStripMenuItem.Click += new EventHandler(this.helpToolStripMenuItem_Click);
      this.mnuEnhanced.Name = "mnuEnhanced";
      this.mnuEnhanced.Size = new Size(77, 24);
      this.mnuEnhanced.Text = "&Conditions";
      this.tabMain.Controls.Add((Control) this.documentTrackingPage);
      this.tabMain.Controls.Add((Control) this.preliminaryTrackingPage);
      this.tabMain.Controls.Add((Control) this.underwritingTrackingPage);
      this.tabMain.Controls.Add((Control) this.postClosingTrackingPage);
      this.tabMain.Controls.Add((Control) this.sellTrackingPage);
      this.tabMain.Controls.Add((Control) this.enhancedTrackingPage);
      this.tabMain.Controls.Add((Control) this.packagesPage);
      this.tabMain.Controls.Add((Control) this.historyTrackingPage);
      this.tabMain.Controls.Add((Control) this.eNotePage);
      this.tabMain.Dock = DockStyle.Fill;
      this.tabMain.HotTrack = true;
      this.tabMain.ItemSize = new Size(74, 28);
      this.tabMain.Location = new Point(2, 2);
      this.tabMain.Margin = new Padding(0);
      this.tabMain.Name = "tabMain";
      this.tabMain.Padding = new Point(11, 3);
      this.tabMain.SelectedIndex = 0;
      this.tabMain.Size = new Size(753, 390);
      this.tabMain.TabIndex = 2;
      this.tabMain.SelectedIndexChanged += new EventHandler(this.tabMain_SelectedIndexChanged);
      this.tabMain.Selected += new TabControlEventHandler(this.tabMain_Selected);
      this.documentTrackingPage.Location = new Point(4, 32);
      this.documentTrackingPage.Name = "documentTrackingPage";
      this.documentTrackingPage.Padding = new Padding(0, 2, 2, 2);
      this.documentTrackingPage.Size = new Size(745, 354);
      this.documentTrackingPage.TabIndex = 0;
      this.documentTrackingPage.Text = "Documents";
      this.documentTrackingPage.UseVisualStyleBackColor = true;
      this.preliminaryTrackingPage.Location = new Point(4, 32);
      this.preliminaryTrackingPage.Name = "preliminaryTrackingPage";
      this.preliminaryTrackingPage.Padding = new Padding(0, 2, 2, 2);
      this.preliminaryTrackingPage.Size = new Size(745, 354);
      this.preliminaryTrackingPage.TabIndex = 1;
      this.preliminaryTrackingPage.Text = "Preliminary Conditions";
      this.preliminaryTrackingPage.UseVisualStyleBackColor = true;
      this.underwritingTrackingPage.Location = new Point(4, 32);
      this.underwritingTrackingPage.Name = "underwritingTrackingPage";
      this.underwritingTrackingPage.Padding = new Padding(0, 2, 2, 2);
      this.underwritingTrackingPage.Size = new Size(745, 354);
      this.underwritingTrackingPage.TabIndex = 2;
      this.underwritingTrackingPage.Text = "Underwriting Conditions";
      this.underwritingTrackingPage.UseVisualStyleBackColor = true;
      this.postClosingTrackingPage.Location = new Point(4, 32);
      this.postClosingTrackingPage.Name = "postClosingTrackingPage";
      this.postClosingTrackingPage.Padding = new Padding(0, 2, 2, 2);
      this.postClosingTrackingPage.Size = new Size(745, 354);
      this.postClosingTrackingPage.TabIndex = 3;
      this.postClosingTrackingPage.Text = "Post-Closing Conditions";
      this.postClosingTrackingPage.UseVisualStyleBackColor = true;
      this.sellTrackingPage.Location = new Point(4, 32);
      this.sellTrackingPage.Name = "sellTrackingPage";
      this.sellTrackingPage.Padding = new Padding(0, 2, 2, 2);
      this.sellTrackingPage.Size = new Size(745, 354);
      this.sellTrackingPage.TabIndex = 4;
      this.sellTrackingPage.Text = "Delivery Conditions";
      this.sellTrackingPage.UseVisualStyleBackColor = true;
      this.enhancedTrackingPage.Location = new Point(4, 32);
      this.enhancedTrackingPage.Name = "enhancedTrackingPage";
      this.enhancedTrackingPage.Padding = new Padding(0, 2, 2, 2);
      this.enhancedTrackingPage.Size = new Size(745, 354);
      this.enhancedTrackingPage.TabIndex = 6;
      this.enhancedTrackingPage.Text = "Conditions";
      this.enhancedTrackingPage.UseVisualStyleBackColor = true;
      this.packagesPage.Location = new Point(4, 32);
      this.packagesPage.Name = "packagesPage";
      this.packagesPage.Size = new Size(745, 354);
      this.packagesPage.TabIndex = 7;
      this.packagesPage.Text = "Packages";
      this.packagesPage.UseVisualStyleBackColor = true;
      this.historyTrackingPage.Location = new Point(4, 32);
      this.historyTrackingPage.Name = "historyTrackingPage";
      this.historyTrackingPage.Padding = new Padding(0, 2, 2, 2);
      this.historyTrackingPage.Size = new Size(745, 354);
      this.historyTrackingPage.TabIndex = 5;
      this.historyTrackingPage.Text = "History";
      this.historyTrackingPage.UseVisualStyleBackColor = true;
      this.eNotePage.Location = new Point(4, 32);
      this.eNotePage.Name = "eNotePage";
      this.eNotePage.Size = new Size(745, 354);
      this.eNotePage.TabIndex = 8;
      this.eNotePage.Text = "eNote";
      this.eNotePage.UseVisualStyleBackColor = true;
      this.pnlClose.Controls.Add((Control) this.helpLink);
      this.pnlClose.Controls.Add((Control) this.btnClose);
      this.pnlClose.Dock = DockStyle.Bottom;
      this.pnlClose.Location = new Point(0, 416);
      this.pnlClose.Name = "pnlClose";
      this.pnlClose.Size = new Size(755, 40);
      this.pnlClose.TabIndex = 3;
      this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClose.BackColor = SystemColors.Control;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(672, 9);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 4;
      this.btnClose.TabStop = false;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.pnlMain.Controls.Add((Control) this.tabMain);
      this.pnlMain.Dock = DockStyle.Fill;
      this.pnlMain.Location = new Point(0, 24);
      this.pnlMain.Name = "pnlMain";
      this.pnlMain.Padding = new Padding(2, 2, 0, 0);
      this.pnlMain.Size = new Size(755, 392);
      this.pnlMain.TabIndex = 1;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "eFolder";
      this.helpLink.Location = new Point(8, 12);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 16);
      this.helpLink.TabIndex = 5;
      this.helpLink.TabStop = false;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(755, 456);
      this.Controls.Add((Control) this.pnlMain);
      this.Controls.Add((Control) this.mnuMain);
      this.Controls.Add((Control) this.pnlClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = Resources.icon_allsizes_bug;
      this.KeyPreview = true;
      this.Name = nameof (eFolderDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass eFolder";
      this.Activated += new EventHandler(this.eFolderDialog_Activated);
      this.FormClosing += new FormClosingEventHandler(this.eFolderDialog_FormClosing);
      this.mnuMain.ResumeLayout(false);
      this.mnuMain.PerformLayout();
      this.tabMain.ResumeLayout(false);
      this.pnlClose.ResumeLayout(false);
      this.pnlMain.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.MainForm
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Belikov.GenuineChannels;
using Elli.Metrics.Client;
using Elli.Web.Host;
using Elli.Web.Host.Login;
using Elli.Web.Host.SSF.Context;
using Elli.Web.Host.SSF.UI;
using EllieMae.EMLite.AsyncTaskManager;
using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientCommon;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.Login;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.ConcurrentEditing;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.Diagnostics;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.ePass.Messaging;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.HomePage;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.Trading;
using EllieMae.EMLite.Trading.Notifications;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.EncAppMgr;
using EllieMae.Encompass.JITLogger;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class MainForm : Form, IStatusDisplay
  {
    private const string className = "MainForm";
    protected static string sw = Tracing.SwOutsideLoan;
    public static string[] ApplicationArgs = (string[]) null;
    private int maxUserIdleTime = -1;
    private int maxUserIdleTime2 = -1;
    private ActionAfterUserIdle actionAfterUserIdle;
    private ToolStripDropDown helpDropDown;
    private static MainForm instance = (MainForm) null;
    private IContainer components;
    private System.Windows.Forms.Timer gcTimer;
    private Panel mainPanel;
    private GradientMenuStrip mainMenu;
    private GradientMenuStrip tradeMenu;
    private GradientMenuStrip borrowerMenu;
    private GradientMenuStrip bizPartnerMenu;
    private GradientMenuStrip calendarMenu;
    private GradientMenuStrip campaignMenu;
    private GradientMenuStrip taskMenu;
    private ToolStripSeparator menuItem9;
    private ToolStripSeparator menuItem13;
    private ToolStripMenuItem tsMenuItemIM;
    private ToolStripMenuItem tsMenuItemMailbox;
    private ToolStripMenuItem tsMenuItemCalculator;
    private ToolStripMenuItem tsMenuItemExit;
    private GradientMenuStrip pipelineMenu;
    private ToolStripMenuItem tsMenuItemEncompass;
    private ToolStripMenuItem pipelineHeadItem;
    private ToolStripMenuItem tsMenuItemNewLoan;
    private ToolStripMenuItem tsMenuItemMoveToFolder;
    private ToolStripMenuItem tsMenuItemDeleteLoan;
    private ToolStripMenuItem tsMenuItemImport;
    private ToolStripMenuItem tsMenuItemTransfer;
    private ToolStripMenuItem tsMenuItemCompanySettings;
    private GradientMenuStrip loanMenu;
    private GradientMenuStrip contactMenu;
    private ToolStripMenuItem contactHeadItem;
    private ToolStripSeparator menuItem18;
    private ToolStripSeparator menuItem19;
    private ToolStripMenuItem fileItem;
    private ToolStripMenuItem businessItem;
    private ToolStripMenuItem conversationItem;
    private ToolStripMenuItem taskItem;
    private ToolStripMenuItem ausTrackingItem;
    private ToolStripMenuItem repWarrantTrackerItem;
    private ToolStripMenuItem disclosureTrackingItem;
    private ToolStripMenuItem loCompensationItem;
    private ToolStripMenuItem antiSteeringSafeHarborItem;
    private ToolStripMenuItem ecsDataViewerItem;
    private ToolStripMenuItem tqlServicesItem;
    private ToolStripMenuItem miCenterItem;
    private ToolStripMenuItem netTangibleBenefitItem;
    private ToolStripMenuItem complianceReviewItem;
    private ToolStripMenuItem secureItem;
    private ToolStripMenuItem amortItem;
    private ToolStripMenuItem profitItem;
    private ToolStripMenuItem trustItem;
    private ToolStripSeparator menuItem20;
    private ToolStripMenuItem prequalItem;
    private ToolStripMenuItem debtItem;
    private ToolStripMenuItem comparisonItem;
    private ToolStripMenuItem cashItem;
    private ToolStripMenuItem rentItem;
    private ToolStripMenuItem loanToolsHeadItem;
    private ToolStripMenuItem formHeadItem;
    private ToolStripMenuItem worstCasePricingItem;
    private MainScreen mainScreen;
    private ToolStripMenuItem menuItem3;
    private ToolStripMenuItem verifHeadItem;
    private ToolStripMenuItem vodItem;
    private ToolStripMenuItem voeItem;
    private ToolStripMenuItem volItem;
    private ToolStripMenuItem vorItem;
    private ToolStripMenuItem vomItem;
    private ToolStripMenuItem voggItem;
    private ToolStripMenuItem vooiItem;
    private ToolStripMenuItem voolItem;
    private ToolStripMenuItem vooaItem;
    private ToolStripMenuItem voalItem;
    private ToolStripSeparator menuItem4;
    private ToolStripMenuItem findFieldItem;
    private ToolStripSeparator menuItem12;
    private ToolStripSeparator menuItem16;
    private ToolStripMenuItem menuItem21;
    private ToolStripSeparator menuItem23;
    private ToolStripMenuItem menuItem24;
    private ToolStripMenuItem coMortgagerHeadItem;
    private ToolStripMenuItem deleteCurrentItem;
    private ToolStripMenuItem SwapCurrentItem;
    private ToolStripMenuItem menuItemBorContacts;
    private ToolStripMenuItem menuItemBizContacts;
    private ToolStripMenuItem menuItemCalendar;
    private ToolStripMenuItem importContactsItem;
    private ToolStripMenuItem menuItemImportBorrowerContacts;
    private ToolStripMenuItem menuItemImportBusinessContacts;
    private ToolStripMenuItem tsMenuItemTemplates;
    private ToolStripSeparator menuItem71;
    private ToolStripSeparator menuItem74;
    private ToolStripMenuItem lpItem;
    private ToolStripMenuItem ccItem;
    private ToolStripMenuItem defaultFormsItem;
    private ToolStripMenuItem selectFormsItem;
    private ToolStripMenuItem addDocSetItem;
    private GradientMenuStrip reportMenu;
    private ToolStripMenuItem reportHeadItem;
    private ToolStripMenuItem addReportItem;
    private ToolStripMenuItem deleteReportItem;
    private ToolStripSeparator menuItem81;
    private ToolStripMenuItem renameReportItem;
    private ToolStripMenuItem newFolderReportItem;
    private ToolStripSeparator menuItem70;
    private ToolStripMenuItem refreshReportItem;
    private ToolStripSeparator menuItemExportDivider;
    private ToolStripMenuItem menuItemExportBorrowerContacts;
    private ToolStripMenuItem menuItemExportBusinessContacts;
    private ToolStripMenuItem addDataSetItem;
    private ToolStripMenuItem menuItemSynchronize;
    private Hashtable subMenus = new Hashtable();
    private static int gcInterval = 1800000;
    private static int gcGeneration = 0;
    private ToolStripMenuItem menuItem49;
    private ToolStripMenuItem menuItemTasks;
    private ToolStripSeparator menuItemSyncDivider;
    private bool gcEnabled;
    private ToolStripMenuItem menuItemServices;
    private ToolStripMenuItem menuItemCompaign;
    public TipControl Tips;
    private BalloonToolTip ceBalloonToolTip;
    private ToolStripSeparator toolStripSeparator18;
    private ToolStripMenuItem tsHelpDiagnostics;
    private ToolStripMenuItem tsMenuItemExportLoan;
    private GradientMenuStrip correspondentMasterContractListMenu;
    private ToolStripMenuItem toolStripMenuItem5;
    private ToolStripMenuItem newCMCMenuItem;
    private ToolStripMenuItem deleteCMCMenuItem;
    private ToolStripMenuItem exportCMCMenuItem;
    private ToolStripMenuItem printCMCMenuItem;
    private ToolStripMenuItem archiveCMCMenuItem;
    private ToolStripSeparator toolStripSeparator20;
    private ToolStripMenuItem createCMCTradeMenuItem;
    private GradientMenuStrip correspondentMasterContractEditorMenu;
    private ToolStripMenuItem correspondentMasterCommitmentEditorMenuItem;
    private ToolStripMenuItem saveCMCMenuItem;
    private ToolStripMenuItem exitCMCMenuItem;
    private GradientMenuStrip correspondentTradeListMenu;
    private ToolStripMenuItem correspondentLoanTradeStripMenuItem;
    private ToolStripMenuItem newLoanTradeMenuItem;
    private ToolStripMenuItem editLoanTradeMenuItem;
    private ToolStripMenuItem duplicateTradeMenuItem;
    private ToolStripMenuItem deleteTradeMenuItem;
    private ToolStripMenuItem exportLoanTradeMenuItem;
    private ToolStripMenuItem printLoanTradeMenuItem;
    private ToolStripSeparator toolStripSeparator21;
    private ToolStripMenuItem lockUnlockLoanTradeMenuItem;
    private ToolStripMenuItem archiveLoanTradeStripMenuItem;
    private ToolStripMenuItem updatePendingLoanMenuItem;
    private GradientMenuStrip correspondentTradeEditorMenu;
    private ToolStripMenuItem correspondentTradeMenuItem;
    private ToolStripMenuItem saveCorrespondentTradeMenuItem;
    private ToolStripMenuItem exitCorrespondentTradeMenuItem;
    private ToolStripMenuItem exportSelCorrespondentTradeMenuItem;
    private ToolStripMenuItem exportAllCorrespondentTradeMenuItem;
    private ToolStripMenuItem fannieMaeExport = new ToolStripMenuItem();
    private ToolStripMenuItem assignToMbsPoolStripMenuItem;
    private ToolStripMenuItem createMbsPoolToolStripMenuItem2;
    private ToolStripMenuItem feeVarianceWorksheetItem;
    private ToolStripButton ssbTradeLoanUpdate;
    private ToolStripMenuItem tsMenuItemEMMessage;
    private string scAppCmdArgs;
    private string scAppUrl;
    private ToolStripMenuItem voidLoanTradeStripMenuItem;
    private ToolStripMenuItem tsHelpDDMDiagnostics;
    private ToolStripMenuItem tsMenuItemJITLogger;
    private ToolStripProgressBar ssProgressBar;
    private ToolStripButton ssbInsights;
    private ToolStripStatusLabel sslSeparator2;
    private BalloonToolTip ceGetUpdatesBalloonToolTip;
    private System.Threading.Timer asychTasksTimer;
    private ToolStripMenuItem complianceItem;
    private System.Windows.Forms.Timer checkIdleTimer;
    private ToolStripSeparator bankerLineItem;
    private ToolStripMenuItem fundingItem;
    private ToolStripMenuItem correspondentPurchaseItem;
    private ToolStripMenuItem purchaseItem;
    private ToolStripMenuItem secondaryItem;
    private ToolStripMenuItem lockRequestItem;
    private ToolStripMenuItem lockComparisonItem;
    private ToolStripMenuItem balanceItem;
    private ToolStripMenuItem brokerItem;
    private ToolStripMenuItem underwriterItem;
    private ToolStripMenuItem verificationTrackingItem;
    private ToolStripMenuItem shippingItem;
    private ToolStripMenuItem documentTrackingMgmtItem;
    private ToolStripMenuItem comortgagorItem;
    private ToolStripMenuItem piggybackLoanItem;
    private ToolStripMenuItem auditTrailItem;
    private ToolStripMenuItem tsMenuItemSelectLoanTemplate;
    private ToolStripSeparator tsSeparator1;
    private ToolStripMenuItem interimServicingItem;
    private GradientStatusStrip statusBar;
    private ToolStripStatusLabel sslSeparator;
    private ToolStripButton ssbUploadProgress;
    private ToolStripStatusLabel sslHelp;
    private ToolStripStatusLabel sslFieldID;
    private ToolStripStatusLabel sslLastSaved;
    private ToolStripStatusLabel sslDate;
    private ToolStripMenuItem tsMenuItemHelp;
    private ToolStripMenuItem tsMenuItemAbout;
    private ToolStripMenuItem addTaskSetItem;
    private ToolStripMenuItem tsMenuItemView;
    private ToolStripMenuItem tsMenuItemHome;
    private ToolStripMenuItem tsMenuItemPipeline;
    private ToolStripMenuItem tsMenuItemLoan;
    private ToolStripMenuItem tsMenuItemEpass;
    private ToolStripMenuItem tsMenuItemEfolder;
    private ToolStripMenuItem tsMenuItemTrades;
    private ToolStripMenuItem tsMenuItemContacts;
    private ToolStripMenuItem tsMenuItemDashboard;
    private ToolStripMenuItem tsMenuItemReports;
    private ToolStripMenuItem tsMenuItemLoanSearch;
    private ToolStripMenuItem tsMenuItemTradeManagement;
    private ToolStripMenuItem tsStripMenuItemMasterContacts;
    private ToolStripMenuItem tsMenuItemMbsPools;
    private ToolStripMenuItem tsMenuItemBorrowerContacts;
    private ToolStripMenuItem tsMenuItemBusinessContacts;
    private ToolStripMenuItem tsMenuItemCalendar;
    private ToolStripMenuItem tsMenuItemTasks;
    private ToolStripMenuItem tsMenuItemCampaigns;
    private ToolStripMenuItem tsMenuItemEncompassHelp;
    private ToolStripMenuItem tsMenuItemTutorials;
    private ToolStripMenuItem tsMenuItemEncompassGlossary;
    private ToolStripMenuItem tsMenuItemDocLibrary;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem tsMenuItemTrainingSchedule;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem tsMenuItemTechSupportOptions;
    private ToolStripSeparator toolStripSeparator3;
    private ToolStripMenuItem tsMenuItemFeedback;
    private ToolStripMenuItem MenuItemEllieMae;
    private ToolStripSeparator toolStripSeparator4;
    private ToolStripMenuItem tsMenuItemReleaseNotes;
    private ToolStripMenuItem tsMenuItemReportOption;
    private ToolStripMenuItem tsMenuItemEditLoan;
    private ToolStripMenuItem tsMenuItemOpenRecent;
    private ToolStripMenuItem tsMenuItemDuplicateLoan;
    private ToolStripSeparator toolStripSeparatorEx1;
    private ToolStripSeparator toolStripSeparatorEx2;
    private ToolStripSeparator toolStripSeparatorEx21;
    private ToolStripMenuItem tsMenuItemExportToExcel;
    private ToolStripMenuItem tsMenuItemPrintForms;
    private ToolStripMenuItem tsMenuItemManageAlerts;
    private ToolStripSeparator toolStripSeparatorEx4;
    private ToolStripMenuItem tsMenuItemCustomizeColumns;
    private ToolStripMenuItem tsMenuItemSaveView;
    private ToolStripMenuItem tsMenuItemResetView;
    private ToolStripMenuItem tsMenuItemManagePipelineViews;
    private ToolStripMenuItem borrowerContactItems;
    private ToolStripMenuItem tsBCNewContact;
    private ToolStripMenuItem tsBCDuplicateContact;
    private ToolStripMenuItem tsBCDeleteContact;
    private ToolStripSeparator tsBCSeparator1;
    private ToolStripMenuItem tsBCExportExcel;
    private ToolStripMenuItem tsBCPrintDetails;
    private ToolStripSeparator tsBCSeparator2;
    private ToolStripMenuItem tsBCMailMerge;
    private ToolStripMenuItem tsBCSynchronize;
    private ToolStripSeparator tsBCSeparator3;
    private ToolStripMenuItem tsBCAddToGroup;
    private ToolStripMenuItem tsBCRemoveFromGroup;
    private ToolStripMenuItem tsBCEditGroups;
    private ToolStripSeparator tsBCSeparator4;
    private ToolStripMenuItem tsBCOriginateLoan;
    private ToolStripMenuItem tsBCOrderCredit;
    private ToolStripMenuItem tsBCProductPricing;
    private ToolStripSeparator tsBCSeparator5;
    private ToolStripMenuItem tsBCBuyLeads;
    private ToolStripMenuItem tsBCImportLeads;
    private ToolStripMenuItem tsBCImportBorrower;
    private ToolStripMenuItem tsBCExportBorrower;
    private ToolStripMenuItem tsBCReassign;
    private ToolStripSeparator tsBCSeparator6;
    private ToolStripMenuItem tsBCCustomizeColumns;
    private ToolStripMenuItem tsBCSaveView;
    private ToolStripMenuItem tsBCResetView;
    private ToolStripMenuItem tsBCManageViews;
    private ToolStripSeparator tsBCSeparator7;
    private ToolStripMenuItem tsBCHomePoints;
    private ToolStripMenuItem bizContactItems;
    private ToolStripMenuItem tsBizCNewContact;
    private ToolStripMenuItem tsBizCDuplicateContact;
    private ToolStripMenuItem tsBizCDeleteContact;
    private ToolStripMenuItem tsBizCExportToExcel;
    private ToolStripSeparator toolStripSeparator5;
    private ToolStripMenuItem tsBizCPrintDetail;
    private ToolStripSeparator toolStripSeparator6;
    private ToolStripMenuItem tsBizCMailMerge;
    private ToolStripMenuItem tsBizCSynchronize;
    private ToolStripSeparator toolStripSeparator7;
    private ToolStripMenuItem tsBizCAddToGroup;
    private ToolStripMenuItem tsBizCRemoveFromGroup;
    private ToolStripMenuItem tsBizCEditGroup;
    private ToolStripSeparator toolStripSeparator8;
    private ToolStripMenuItem tsBizCImportBizContact;
    private ToolStripMenuItem tsBizCExportBizContact;
    private ToolStripSeparator toolStripSeparator9;
    private ToolStripMenuItem tsBizCCustomizeColumn;
    private ToolStripMenuItem tsBizCSaveView;
    private ToolStripMenuItem tsBizCResetView;
    private ToolStripMenuItem tsBizCManageView;
    private ToolStripMenuItem calendarItems;
    private ToolStripMenuItem campaignItems;
    private ToolStripMenuItem taskItems;
    private ToolStripMenuItem tsMenuItemOpenRecentPlaceHolder;
    private ToolStripMenuItem tsCalNewApp;
    private ToolStripMenuItem tsCalEditApp;
    private ToolStripMenuItem tsCalDeleteApp;
    private ToolStripSeparator toolStripSeparator10;
    private ToolStripMenuItem tsCalPrint;
    private ToolStripSeparator toolStripSeparator11;
    private ToolStripMenuItem tsCalView;
    private ToolStripMenuItem tsCalToday;
    private ToolStripMenuItem tsCalOneDay;
    private ToolStripMenuItem tsCalWorkDay;
    private ToolStripMenuItem tsCalWeek;
    private ToolStripMenuItem tsCalMonth;
    private ToolStripMenuItem tsCalSynchronize;
    private ToolStripSeparator tsSeparator_CalSynchronize;
    private ToolStripMenuItem tsTask_NewTask;
    private ToolStripMenuItem tsTask_EditTask;
    private ToolStripMenuItem tsTask_Delete;
    private ToolStripMenuItem tsTask_Synchronize;
    private ToolStripSeparator tsSeparator_TaskSynchronize;
    private ToolStripSeparator toolStripSeparator12;
    private ToolStripMenuItem tsTask_ExportExcel;
    private ToolStripMenuItem tsTask_Status;
    private ToolStripMenuItem tsTask_StatusNotStarted;
    private ToolStripMenuItem tsTask_StatusInProgress;
    private ToolStripMenuItem tsTask_StatusCompleted;
    private ToolStripMenuItem tsTask_StatusWait;
    private ToolStripMenuItem tsTask_StatusDeferred;
    private ToolStripMenuItem tsCampaign_NewCampaign;
    private ToolStripMenuItem tsCampaign_OpenCampaign;
    private ToolStripMenuItem tsCampaign_DuplicateCampaign;
    private ToolStripMenuItem tsCampaign_DeleteCampaign;
    private ToolStripSeparator toolStripSeparator13;
    private ToolStripMenuItem tsCampaign_StartCampaign;
    private ToolStripMenuItem tsCampaign_StopCampaign;
    private ToolStripMenuItem tsCampaign_ManageTemplate;
    private GradientMenuStrip dashboardMenu;
    private ToolStripMenuItem tsDashboardHeadItem;
    private ToolStripMenuItem tsDashboard_ManageView;
    private ToolStripMenuItem tsDashboard_ManageSnapshot;
    private ToolStripMenuItem duplicateReportItem;
    private ToolStripMenuItem generateReportMenuItem;
    private GradientMenuStrip loanSearchMenu;
    private GradientMenuStrip tradeListMenu;
    private GradientMenuStrip tradeEditorMenu;
    private GradientMenuStrip masterContractMenu;
    private ToolStripMenuItem loanSearchItems;
    private ToolStripMenuItem tradeListItems;
    private ToolStripMenuItem masterContractItems;
    private ToolStripMenuItem searchLoansToolStripMenuItem;
    private ToolStripMenuItem clearLoansToolStripMenuItem;
    private ToolStripSeparator toolStripMenuItem1;
    private ToolStripMenuItem exportToEToolStripMenuItem;
    private ToolStripMenuItem createTradeToolStripMenuItem;
    private ToolStripMenuItem exportTradeToolStripMenuItem;
    private ToolStripMenuItem printTradeToolStripMenuItem;
    private ToolStripMenuItem assignToTradeToolStripMenuItem;
    private ToolStripSeparator toolStripMenuItem2;
    private ToolStripMenuItem saveViewToolStripMenuItem;
    private ToolStripMenuItem resetViewToolStripMenuItem;
    private ToolStripMenuItem manageLoanSearchViewsToolStripMenuItem;
    private ToolStripMenuItem newTradeToolStripMenuItem;
    private ToolStripMenuItem editTradeToolStripMenuItem;
    private ToolStripMenuItem duplicateTradeToolStripMenuItem;
    private ToolStripMenuItem deleteTradeToolStripMenuItem;
    private ToolStripSeparator toolStripMenuItem3;
    private ToolStripMenuItem lockUnlockTradeToolStripMenuItem;
    private ToolStripMenuItem archiveTradeToolStripMenuItem;
    private ToolStripMenuItem updatePendingLoansToolStripMenuItem;
    private ToolStripMenuItem newContractToolStripMenuItem;
    private ToolStripMenuItem exportContractToolStripMenuItem;
    private ToolStripMenuItem archiveContractToolStripMenuItem;
    private ToolStripMenuItem activateContractToolStripMenuItem;
    private ToolStripMenuItem printContractToolStripMenuItem;
    private ToolStripMenuItem saveContractToolStripMenuItem;
    private ToolStripMenuItem deleteContractToolStripMenuItem;
    private ToolStripSeparator toolStripMenuItem4;
    private ToolStripMenuItem createTradeToolStripMenuItem1;
    private ToolStripMenuItem createMBSPoolToolStripMenuItem;
    private ToolStripMenuItem loanHeadItem;
    private ToolStripMenuItem tsLoanNewLoan;
    private ToolStripMenuItem tsLoanSaveLoan;
    private ToolStripMenuItem tsLoanPrint;
    private ToolStripMenuItem tsLoanExportToDataTrac;
    private ToolStripMenuItem tsLoanExitLoan;
    private ToolStripSeparator toolStripSeparator14;
    private ToolStripMenuItem tsLoanManageBorrowers;
    private ToolStripMenuItem tsLoanAddBorrowerPair;
    private ToolStripMenuItem tsDuplicateLockCheck;
    private ToolStripSeparator toolStripSeparator15;
    private ToolStripSeparator toolStripSeparator19;
    private ToolStripMenuItem tsMilestoneDates;
    private ToolStripMenuItem tsChangeMilestoneDates;
    private ToolStripMenuItem tsUnlockMilestoneDates;
    private ToolStripMenuItem tsLockMilestoneDates;
    private ToolStripMenuItem tsMilestoneList;
    private ToolStripMenuItem tsApplyMilestoneTemplate;
    private ToolStripMenuItem tsUnlockMilestoneList;
    private ToolStripMenuItem tsLockMilestoneList;
    private ToolStripMenuItem tsLoanTemplateSet;
    private ToolStripMenuItem tsLoanProgramTemplate;
    private ToolStripMenuItem tsLoanClosingCost;
    private ToolStripMenuItem tsLoanDocumentSet;
    private ToolStripMenuItem tsLoanTaskSet;
    private ToolStripMenuItem tsLoanDataTemplate;
    private ToolStripMenuItem tsLoanInputFormSet;
    private ToolStripMenuItem tsLoanSettlementServiceSet;
    private ToolStripMenuItem tsLoanAffiliateTemplate;
    private ToolStripMenuItem tsLoanRevertToDefaultFormList;
    private ToolStripSeparator toolStripSeparator16;
    private ToolStripMenuItem tsLoanGoToField;
    private ToolStripSeparator toolStripSeparator17;
    private ToolStripMenuItem tradeEditorItems;
    private ToolStripMenuItem returnToTradeListItem;
    private ToolStripMenuItem saveTradeMenuItem;
    private GradientMenuStrip homeMenu;
    private ToolStripMenuItem homeMenuItem;
    private GradientMenuStrip epassMenu;
    private ToolStripMenuItem epassMenuItem;
    private ToolStripMenuItem tsMenuItemRefresh;
    private ToolStripMenuItem tsMenuItemHelpPad;
    private ContextMenuStrip ctxMenuStripSessionID;
    private ToolStripMenuItem loanSearchExportSelectedMenu;
    private ToolStripMenuItem loanSearchExportAllMenu;
    private ToolStripMenuItem tsExportSelectedLoansMenu;
    private ToolStripMenuItem tsExportAllLoansMenu;
    private ToolStripMenuItem tsExportBorContacts;
    private ToolStripMenuItem tsExportAllBorContacts;
    private ToolStripMenuItem tsPrintBorContacts;
    private ToolStripMenuItem tsPrintAllBorContacts;
    private ToolStripMenuItem tsMailMergeBorContacts;
    private ToolStripMenuItem tsMailMergeAllBorContacts;
    private ToolStripMenuItem tsExportBizContacts;
    private ToolStripMenuItem tsExportAllBizContacts;
    private ToolStripMenuItem tsPrintBizContacts;
    private ToolStripMenuItem tsPrintAllBizContacts;
    private ToolStripMenuItem tsMailMergeBizContacts;
    private ToolStripMenuItem tsMailMergeAllBizContacts;
    private ToolStripMenuItem tsBCExportBorrowerSelectedOnly;
    private ToolStripMenuItem tsBCExportBorrowerAll;
    private ToolStripMenuItem tsBizCExportBizContactSelectedOnly;
    private ToolStripMenuItem tsBizCExportBizContactAll;
    private ToolStripStatusLabel campaignStatusPanel;
    private ToolStripMenuItem tsMenuItemSecurityTradeManagement;
    private GradientMenuStrip securityTradeListMenu;
    private ToolStripMenuItem securityTradeListItems;
    private ToolStripMenuItem newSecurityTradeToolStripMenuItem;
    private ToolStripMenuItem editSecurityTradeToolStripMenuItem;
    private ToolStripMenuItem duplicateSecurityTradeToolStripMenuItem;
    private ToolStripMenuItem deleteSecurityTradeToolStripMenuItem;
    private ToolStripMenuItem exportSecurityTradeToolStripMenuItem;
    private ToolStripMenuItem printSecurityTradeToolStripMenuItem;
    private ToolStripMenuItem lockUnlockSecurityTradeToolStripMenuItem;
    private ToolStripMenuItem archiveSecurityTradeToolStripMenuItem;
    private GradientMenuStrip securityTradeEditorMenu;
    private ToolStripMenuItem securityTradeEditorItems;
    private ToolStripMenuItem saveSecurityTradeMenuItem;
    private ToolStripMenuItem returnToSecurityTradeListItem;
    private ToolStripMenuItem selectedTradesOnlyToolStripMenuItem;
    private ToolStripMenuItem allTradesonPageToolStripMenuItem;
    private ToolStripMenuItem selectSecurityTradesOnlyToolStripMenuItem;
    private ToolStripMenuItem allSecurityTradesonPageToolStripMenuItem;
    private GradientMenuStrip mbsPoolListMenu;
    private GradientMenuStrip mbsPoolEditorMenu;
    private ToolStripMenuItem mbsPoolListItems;
    private ToolStripMenuItem newMbsPoolToolStripMenuItem;
    private ToolStripMenuItem editMbsPoolToolStripMenuItem;
    private ToolStripMenuItem duplicateMbsPoolToolStripMenuItem;
    private ToolStripMenuItem deleteMbsPoolToolStripMenuItem;
    private ToolStripMenuItem exportMbsPoolToolStripMenuItem;
    private ToolStripMenuItem printMbsPoolToolStripMenuItem;
    private ToolStripSeparator mbsPoolToolStripSeparator;
    private ToolStripMenuItem lockUnlockMbsPoolToolStripMenuItem;
    private ToolStripMenuItem archiveMbsPoolToolStripMenuItem;
    private ToolStripMenuItem updateMbsPoolToolStripMenuItem;
    private ToolStripMenuItem mbsPoolEditorItems;
    private ToolStripMenuItem saveMbsPoolMenuItem;
    private ToolStripMenuItem returnToMbsPoolListItem;
    private ToolStripMenuItem selectedMbsPoolsOnlyToolStripMenuItem;
    private ToolStripMenuItem allMbsPoolsonPageToolStripMenuItem;
    private ToolStripMenuItem correspondentLoanStatusMenuItem;
    private ToolStripMenuItem tpoInformationMenuItem;
    private ToolStripMenuItem tsMenuItemOpenWebView;
    private ToolStripMenuItem tsOpenWebViewLoan;
    private ToolStripMenuItem tsOpportunitiesMenu;
    private ToolStripMenuItem tsProspectsMenu;
    private ToolStripSeparator toolStripSeparatorOpenWebView;
    private GradientMenuStrip GSECommitmentListMenu;
    private GradientMenuStrip GSECommitmentEditorMenu;
    private ToolStripMenuItem GSECommitmentListItems;
    private ToolStripMenuItem GSECommitmentEditorMenuItems;
    private ToolStripMenuItem newGSECommitmentToolStripMenuItem;
    private ToolStripMenuItem editGSECommitmentToolStripMenuItem;
    private ToolStripMenuItem duplicateGSECommitmentToolStripMenuItem;
    private ToolStripMenuItem deleteGSECommitmentToolStripMenuItem;
    private ToolStripMenuItem exportGSECommitmentToolStripMenuItem;
    private ToolStripMenuItem printGSECommitmentToolStripMenuItem;
    private ToolStripSeparator GSECommitmentToolStripSeparator;
    private ToolStripMenuItem archiveGSECommitmentToolStripMenuItem;
    private ToolStripMenuItem selectGSECommitmentsOnlyToolStripMenuItem;
    private ToolStripMenuItem allGSECommitmentsonPageToolStripMenuItem;
    private ToolStripMenuItem returnToGSECommitmentListItem;
    private ToolStripMenuItem saveGSECommitmentMenuItem;
    private static string sys32Folder = Path.Combine(Application.StartupPath, "System32");
    private static bool Debugmode = false;
    private const string encAppMgrIpcUrl = "ipc://EncAppMgr/EncAppMgrRO.rem";
    private static string writeClientExceptionAttrName = "WriteClientExceptionToServer";
    private Dictionary<string, ToolStripMenuItem> tabPageMenuItemMap;

    public static MainForm Instance
    {
      get
      {
        if (MainForm.instance == null)
          MainForm.instance = new MainForm();
        return MainForm.instance;
      }
    }

    public BalloonToolTip CEBalloonToolTip
    {
      get
      {
        if (this.ceBalloonToolTip == null)
        {
          this.ceBalloonToolTip = new BalloonToolTip(230, 36);
          this.ceBalloonToolTip.CloseOnLeave = false;
          this.ceBalloonToolTip.PointerOffset = 25;
          this.Controls.Add((Control) this.ceBalloonToolTip);
        }
        return this.ceBalloonToolTip;
      }
    }

    public BalloonToolTip CEGetUpdatesBalloonToolTip
    {
      get
      {
        if (this.ceGetUpdatesBalloonToolTip == null)
        {
          this.ceGetUpdatesBalloonToolTip = new BalloonToolTip(225, 50);
          this.ceGetUpdatesBalloonToolTip.CloseOnLeave = false;
          this.ceGetUpdatesBalloonToolTip.PointerOffset = 25;
          this.Controls.Add((Control) this.ceGetUpdatesBalloonToolTip);
        }
        return this.ceGetUpdatesBalloonToolTip;
      }
    }

    internal void CloseCEGetUpdatesBalloonToolTip()
    {
      if (this.ceGetUpdatesBalloonToolTip == null || !this.ceGetUpdatesBalloonToolTip.Visible)
        return;
      this.ceGetUpdatesBalloonToolTip.Close();
    }

    [DllImport("ole32.dll")]
    private static extern void CoFreeUnusedLibraries();

    static MainForm()
    {
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass");
      if (registryKey == null)
        return;
      try
      {
        string str = (string) registryKey.GetValue("GCGeneration");
        if (str != null)
        {
          MainForm.gcGeneration = Convert.ToInt32(str);
          if (MainForm.gcGeneration < 0)
            MainForm.gcGeneration = 0;
          else if (MainForm.gcGeneration > GC.MaxGeneration)
            MainForm.gcGeneration = GC.MaxGeneration;
        }
      }
      catch
      {
      }
      try
      {
        string str = (string) registryKey.GetValue("GCInterval");
        if (str == null)
          return;
        int num = Convert.ToInt32(str);
        if (num > 3600)
          num = 3600;
        MainForm.gcInterval = num * 1000;
      }
      catch
      {
      }
    }

    private MainForm()
    {
      this.InitializeComponent();
      this.createTabPageMenuItemMapping();
      this.DisplayFieldID("");
      this.ClearLastSaveTime();
      this.Tips = new TipControl();
      this.Controls.Add((Control) this.Tips);
      this.gcTimer_Tick((object) null, (EventArgs) null);
      this.initialContactMenuItems();
      this.initialReportMenuItems();
      this.CollectSubMenus();
      this.helpDropDown = this.tsMenuItemHelp.DropDown;
      this.tsHelpDiagnostics.Checked = DiagnosticSession.DiagnosticsMode != 0;
      this.setInitialAppSize();
      this.ssbInsights.Click += new EventHandler(this.Insights_Click);
    }

    private void showOrHideDDMDiagnostic()
    {
      if (Utils.ParseBoolean(Session.ServerManager.GetServerSetting("CLIENT.ALLOWDDM", false)) && Session.IsBankerEdition())
      {
        this.tsHelpDDMDiagnostics.Visible = true;
        this.tsHelpDDMDiagnostics.Checked = DDMDiagnosticSession.DiagnosticsMode == DDMDiagnosticsMode.Enabled;
      }
      else
        this.tsHelpDDMDiagnostics.Visible = false;
    }

    private void removeBankerMenuItems()
    {
      this.loanToolsHeadItem.DropDownItems.Remove((ToolStripItem) this.bankerLineItem);
      this.loanToolsHeadItem.DropDownItems.Remove((ToolStripItem) this.fundingItem);
      this.loanToolsHeadItem.DropDownItems.Remove((ToolStripItem) this.brokerItem);
      this.loanToolsHeadItem.DropDownItems.Remove((ToolStripItem) this.balanceItem);
      this.loanToolsHeadItem.DropDownItems.Remove((ToolStripItem) this.lockRequestItem);
      this.loanToolsHeadItem.DropDownItems.Remove((ToolStripItem) this.lockComparisonItem);
      this.loanToolsHeadItem.DropDownItems.Remove((ToolStripItem) this.secondaryItem);
      this.loanToolsHeadItem.DropDownItems.Remove((ToolStripItem) this.correspondentPurchaseItem);
      this.loanToolsHeadItem.DropDownItems.Remove((ToolStripItem) this.purchaseItem);
      this.loanToolsHeadItem.DropDownItems.Remove((ToolStripItem) this.shippingItem);
      this.loanToolsHeadItem.DropDownItems.Remove((ToolStripItem) this.documentTrackingMgmtItem);
    }

    private void removeOpenWebViewFromPipeLine()
    {
      this.pipelineHeadItem.DropDownItems.Remove((ToolStripItem) this.tsMenuItemOpenWebView);
      this.pipelineHeadItem.DropDownItems.Remove((ToolStripItem) this.toolStripSeparatorOpenWebView);
    }

    private void CollectSubMenus()
    {
      if (EnConfigurationSettings.GlobalSettings.RuntimeEnvironment == EllieMae.EMLite.Common.RuntimeEnvironment.Hosted)
      {
        this.importContactsItem.DropDownItems.Remove((ToolStripItem) this.menuItemSynchronize);
        this.importContactsItem.DropDownItems.Remove((ToolStripItem) this.menuItemSyncDivider);
      }
      GradientMenuStrip[] gradientMenuStripArray = new GradientMenuStrip[25]
      {
        this.homeMenu,
        this.pipelineMenu,
        this.loanMenu,
        this.epassMenu,
        this.reportMenu,
        this.dashboardMenu,
        this.securityTradeListMenu,
        this.loanSearchMenu,
        this.tradeListMenu,
        this.securityTradeEditorMenu,
        this.tradeEditorMenu,
        this.masterContractMenu,
        this.borrowerMenu,
        this.bizPartnerMenu,
        this.calendarMenu,
        this.campaignMenu,
        this.taskMenu,
        this.mbsPoolListMenu,
        this.mbsPoolEditorMenu,
        this.correspondentMasterContractListMenu,
        this.correspondentMasterContractEditorMenu,
        this.correspondentTradeListMenu,
        this.correspondentTradeEditorMenu,
        this.GSECommitmentListMenu,
        this.GSECommitmentEditorMenu
      };
      foreach (GradientMenuStrip key in gradientMenuStripArray)
      {
        ToolStripMenuItem[] array = new ToolStripMenuItem[key.Items.Count];
        key.Items.CopyTo((ToolStripItem[]) array, 0);
        this.subMenus[(object) key] = (object) array;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainForm));
      this.components = (IContainer) new System.ComponentModel.Container();
      this.gcTimer = new System.Windows.Forms.Timer(this.components);
      this.mainPanel = new Panel();
      this.mbsPoolEditorMenu = new GradientMenuStrip();
      this.mbsPoolEditorItems = new ToolStripMenuItem();
      this.saveMbsPoolMenuItem = new ToolStripMenuItem();
      this.returnToMbsPoolListItem = new ToolStripMenuItem();
      this.mbsPoolListMenu = new GradientMenuStrip();
      this.mbsPoolListItems = new ToolStripMenuItem();
      this.newMbsPoolToolStripMenuItem = new ToolStripMenuItem();
      this.editMbsPoolToolStripMenuItem = new ToolStripMenuItem();
      this.duplicateMbsPoolToolStripMenuItem = new ToolStripMenuItem();
      this.deleteMbsPoolToolStripMenuItem = new ToolStripMenuItem();
      this.exportMbsPoolToolStripMenuItem = new ToolStripMenuItem();
      this.selectedMbsPoolsOnlyToolStripMenuItem = new ToolStripMenuItem();
      this.allMbsPoolsonPageToolStripMenuItem = new ToolStripMenuItem();
      this.printMbsPoolToolStripMenuItem = new ToolStripMenuItem();
      this.mbsPoolToolStripSeparator = new ToolStripSeparator();
      this.lockUnlockMbsPoolToolStripMenuItem = new ToolStripMenuItem();
      this.archiveMbsPoolToolStripMenuItem = new ToolStripMenuItem();
      this.updateMbsPoolToolStripMenuItem = new ToolStripMenuItem();
      this.loanSearchMenu = new GradientMenuStrip();
      this.loanSearchItems = new ToolStripMenuItem();
      this.searchLoansToolStripMenuItem = new ToolStripMenuItem();
      this.clearLoansToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripMenuItem1 = new ToolStripSeparator();
      this.exportToEToolStripMenuItem = new ToolStripMenuItem();
      this.loanSearchExportSelectedMenu = new ToolStripMenuItem();
      this.loanSearchExportAllMenu = new ToolStripMenuItem();
      this.createTradeToolStripMenuItem = new ToolStripMenuItem();
      this.createMbsPoolToolStripMenuItem2 = new ToolStripMenuItem();
      this.assignToTradeToolStripMenuItem = new ToolStripMenuItem();
      this.assignToMbsPoolStripMenuItem = new ToolStripMenuItem();
      this.toolStripMenuItem2 = new ToolStripSeparator();
      this.saveViewToolStripMenuItem = new ToolStripMenuItem();
      this.resetViewToolStripMenuItem = new ToolStripMenuItem();
      this.manageLoanSearchViewsToolStripMenuItem = new ToolStripMenuItem();
      this.tradeEditorMenu = new GradientMenuStrip();
      this.tradeEditorItems = new ToolStripMenuItem();
      this.saveTradeMenuItem = new ToolStripMenuItem();
      this.returnToTradeListItem = new ToolStripMenuItem();
      this.tradeListMenu = new GradientMenuStrip();
      this.tradeListItems = new ToolStripMenuItem();
      this.newTradeToolStripMenuItem = new ToolStripMenuItem();
      this.editTradeToolStripMenuItem = new ToolStripMenuItem();
      this.duplicateTradeToolStripMenuItem = new ToolStripMenuItem();
      this.deleteTradeToolStripMenuItem = new ToolStripMenuItem();
      this.exportTradeToolStripMenuItem = new ToolStripMenuItem();
      this.selectedTradesOnlyToolStripMenuItem = new ToolStripMenuItem();
      this.allTradesonPageToolStripMenuItem = new ToolStripMenuItem();
      this.printTradeToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripMenuItem3 = new ToolStripSeparator();
      this.lockUnlockTradeToolStripMenuItem = new ToolStripMenuItem();
      this.archiveTradeToolStripMenuItem = new ToolStripMenuItem();
      this.updatePendingLoansToolStripMenuItem = new ToolStripMenuItem();
      this.masterContractMenu = new GradientMenuStrip();
      this.masterContractItems = new ToolStripMenuItem();
      this.newContractToolStripMenuItem = new ToolStripMenuItem();
      this.saveContractToolStripMenuItem = new ToolStripMenuItem();
      this.deleteContractToolStripMenuItem = new ToolStripMenuItem();
      this.exportContractToolStripMenuItem = new ToolStripMenuItem();
      this.printContractToolStripMenuItem = new ToolStripMenuItem();
      this.archiveContractToolStripMenuItem = new ToolStripMenuItem();
      this.activateContractToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripMenuItem4 = new ToolStripSeparator();
      this.createTradeToolStripMenuItem1 = new ToolStripMenuItem();
      this.createMBSPoolToolStripMenuItem = new ToolStripMenuItem();
      this.checkIdleTimer = new System.Windows.Forms.Timer();
      this.mainMenu = new GradientMenuStrip();
      this.tsMenuItemEncompass = new ToolStripMenuItem();
      this.tsMenuItemIM = new ToolStripMenuItem();
      this.tsMenuItemMailbox = new ToolStripMenuItem();
      this.tsMenuItemCalculator = new ToolStripMenuItem();
      this.menuItem9 = new ToolStripSeparator();
      this.tsMenuItemCompanySettings = new ToolStripMenuItem();
      this.menuItem13 = new ToolStripSeparator();
      this.tsMenuItemExit = new ToolStripMenuItem();
      this.tsMenuItemView = new ToolStripMenuItem();
      this.tsMenuItemHome = new ToolStripMenuItem();
      this.tsMenuItemPipeline = new ToolStripMenuItem();
      this.tsMenuItemLoan = new ToolStripMenuItem();
      this.tsMenuItemEpass = new ToolStripMenuItem();
      this.tsMenuItemEfolder = new ToolStripMenuItem();
      this.tsMenuItemTrades = new ToolStripMenuItem();
      this.tsMenuItemSecurityTradeManagement = new ToolStripMenuItem();
      this.tsMenuItemLoanSearch = new ToolStripMenuItem();
      this.tsMenuItemTradeManagement = new ToolStripMenuItem();
      this.tsStripMenuItemMasterContacts = new ToolStripMenuItem();
      this.tsMenuItemContacts = new ToolStripMenuItem();
      this.tsMenuItemBorrowerContacts = new ToolStripMenuItem();
      this.tsMenuItemBusinessContacts = new ToolStripMenuItem();
      this.tsMenuItemCalendar = new ToolStripMenuItem();
      this.tsMenuItemTasks = new ToolStripMenuItem();
      this.tsMenuItemCampaigns = new ToolStripMenuItem();
      this.tsMenuItemDashboard = new ToolStripMenuItem();
      this.tsMenuItemReports = new ToolStripMenuItem();
      this.tsMenuItemHelp = new ToolStripMenuItem();
      this.tsMenuItemEncompassHelp = new ToolStripMenuItem();
      this.tsMenuItemHelpPad = new ToolStripMenuItem();
      this.tsMenuItemTutorials = new ToolStripMenuItem();
      this.tsMenuItemEncompassGlossary = new ToolStripMenuItem();
      this.tsMenuItemDocLibrary = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.tsMenuItemTrainingSchedule = new ToolStripMenuItem();
      this.tsMenuItemTechSupportOptions = new ToolStripMenuItem();
      this.toolStripSeparator18 = new ToolStripSeparator();
      this.tsHelpDiagnostics = new ToolStripMenuItem();
      this.tsMenuItemJITLogger = new ToolStripMenuItem();
      this.tsHelpDDMDiagnostics = new ToolStripMenuItem();
      this.toolStripSeparator3 = new ToolStripSeparator();
      this.tsMenuItemFeedback = new ToolStripMenuItem();
      this.MenuItemEllieMae = new ToolStripMenuItem();
      this.toolStripSeparator4 = new ToolStripSeparator();
      this.tsMenuItemReleaseNotes = new ToolStripMenuItem();
      this.tsMenuItemAbout = new ToolStripMenuItem();
      this.tsMenuItemMbsPools = new ToolStripMenuItem();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.borrowerMenu = new GradientMenuStrip();
      this.borrowerContactItems = new ToolStripMenuItem();
      this.tsBCNewContact = new ToolStripMenuItem();
      this.tsBCDuplicateContact = new ToolStripMenuItem();
      this.tsBCDeleteContact = new ToolStripMenuItem();
      this.tsBCSeparator1 = new ToolStripSeparator();
      this.tsBCExportExcel = new ToolStripMenuItem();
      this.tsExportBorContacts = new ToolStripMenuItem();
      this.tsExportAllBorContacts = new ToolStripMenuItem();
      this.tsBCPrintDetails = new ToolStripMenuItem();
      this.tsPrintBorContacts = new ToolStripMenuItem();
      this.tsPrintAllBorContacts = new ToolStripMenuItem();
      this.tsBCSeparator2 = new ToolStripSeparator();
      this.tsBCMailMerge = new ToolStripMenuItem();
      this.tsMailMergeBorContacts = new ToolStripMenuItem();
      this.tsMailMergeAllBorContacts = new ToolStripMenuItem();
      this.tsBCSynchronize = new ToolStripMenuItem();
      this.tsBCSeparator3 = new ToolStripSeparator();
      this.tsBCAddToGroup = new ToolStripMenuItem();
      this.tsBCRemoveFromGroup = new ToolStripMenuItem();
      this.tsBCEditGroups = new ToolStripMenuItem();
      this.tsBCSeparator4 = new ToolStripSeparator();
      this.tsBCOriginateLoan = new ToolStripMenuItem();
      this.tsBCOrderCredit = new ToolStripMenuItem();
      this.tsBCProductPricing = new ToolStripMenuItem();
      this.tsBCSeparator5 = new ToolStripSeparator();
      this.tsBCBuyLeads = new ToolStripMenuItem();
      this.tsBCImportLeads = new ToolStripMenuItem();
      this.tsBCImportBorrower = new ToolStripMenuItem();
      this.tsBCExportBorrower = new ToolStripMenuItem();
      this.tsBCExportBorrowerSelectedOnly = new ToolStripMenuItem();
      this.tsBCExportBorrowerAll = new ToolStripMenuItem();
      this.tsBCReassign = new ToolStripMenuItem();
      this.tsBCSeparator6 = new ToolStripSeparator();
      this.tsBCCustomizeColumns = new ToolStripMenuItem();
      this.tsBCSaveView = new ToolStripMenuItem();
      this.tsBCResetView = new ToolStripMenuItem();
      this.tsBCManageViews = new ToolStripMenuItem();
      this.tsBCSeparator7 = new ToolStripSeparator();
      this.tsBCHomePoints = new ToolStripMenuItem();
      this.bizPartnerMenu = new GradientMenuStrip();
      this.bizContactItems = new ToolStripMenuItem();
      this.tsBizCNewContact = new ToolStripMenuItem();
      this.tsBizCDuplicateContact = new ToolStripMenuItem();
      this.tsBizCDeleteContact = new ToolStripMenuItem();
      this.toolStripSeparator5 = new ToolStripSeparator();
      this.tsBizCExportToExcel = new ToolStripMenuItem();
      this.tsExportBizContacts = new ToolStripMenuItem();
      this.tsExportAllBizContacts = new ToolStripMenuItem();
      this.tsBizCPrintDetail = new ToolStripMenuItem();
      this.tsPrintBizContacts = new ToolStripMenuItem();
      this.tsPrintAllBizContacts = new ToolStripMenuItem();
      this.toolStripSeparator6 = new ToolStripSeparator();
      this.tsBizCMailMerge = new ToolStripMenuItem();
      this.tsMailMergeBizContacts = new ToolStripMenuItem();
      this.tsMailMergeAllBizContacts = new ToolStripMenuItem();
      this.tsBizCSynchronize = new ToolStripMenuItem();
      this.toolStripSeparator7 = new ToolStripSeparator();
      this.tsBizCAddToGroup = new ToolStripMenuItem();
      this.tsBizCRemoveFromGroup = new ToolStripMenuItem();
      this.tsBizCEditGroup = new ToolStripMenuItem();
      this.toolStripSeparator8 = new ToolStripSeparator();
      this.tsBizCImportBizContact = new ToolStripMenuItem();
      this.tsBizCExportBizContact = new ToolStripMenuItem();
      this.tsBizCExportBizContactSelectedOnly = new ToolStripMenuItem();
      this.tsBizCExportBizContactAll = new ToolStripMenuItem();
      this.toolStripSeparator9 = new ToolStripSeparator();
      this.tsBizCCustomizeColumn = new ToolStripMenuItem();
      this.tsBizCSaveView = new ToolStripMenuItem();
      this.tsBizCResetView = new ToolStripMenuItem();
      this.tsBizCManageView = new ToolStripMenuItem();
      this.calendarMenu = new GradientMenuStrip();
      this.calendarItems = new ToolStripMenuItem();
      this.tsCalNewApp = new ToolStripMenuItem();
      this.tsCalEditApp = new ToolStripMenuItem();
      this.tsCalDeleteApp = new ToolStripMenuItem();
      this.toolStripSeparator10 = new ToolStripSeparator();
      this.tsCalPrint = new ToolStripMenuItem();
      this.toolStripSeparator11 = new ToolStripSeparator();
      this.tsCalSynchronize = new ToolStripMenuItem();
      this.tsSeparator_CalSynchronize = new ToolStripSeparator();
      this.tsCalView = new ToolStripMenuItem();
      this.tsCalToday = new ToolStripMenuItem();
      this.tsCalOneDay = new ToolStripMenuItem();
      this.tsCalWorkDay = new ToolStripMenuItem();
      this.tsCalWeek = new ToolStripMenuItem();
      this.tsCalMonth = new ToolStripMenuItem();
      this.campaignMenu = new GradientMenuStrip();
      this.campaignItems = new ToolStripMenuItem();
      this.tsCampaign_NewCampaign = new ToolStripMenuItem();
      this.tsCampaign_OpenCampaign = new ToolStripMenuItem();
      this.tsCampaign_DuplicateCampaign = new ToolStripMenuItem();
      this.tsCampaign_DeleteCampaign = new ToolStripMenuItem();
      this.toolStripSeparator13 = new ToolStripSeparator();
      this.tsCampaign_StartCampaign = new ToolStripMenuItem();
      this.tsCampaign_StopCampaign = new ToolStripMenuItem();
      this.tsCampaign_ManageTemplate = new ToolStripMenuItem();
      this.taskMenu = new GradientMenuStrip();
      this.taskItems = new ToolStripMenuItem();
      this.tsTask_NewTask = new ToolStripMenuItem();
      this.tsTask_EditTask = new ToolStripMenuItem();
      this.tsTask_Delete = new ToolStripMenuItem();
      this.toolStripSeparator12 = new ToolStripSeparator();
      this.tsTask_ExportExcel = new ToolStripMenuItem();
      this.toolStripSeparator17 = new ToolStripSeparator();
      this.tsTask_Synchronize = new ToolStripMenuItem();
      this.tsSeparator_TaskSynchronize = new ToolStripSeparator();
      this.tsTask_Status = new ToolStripMenuItem();
      this.tsTask_StatusNotStarted = new ToolStripMenuItem();
      this.tsTask_StatusInProgress = new ToolStripMenuItem();
      this.tsTask_StatusCompleted = new ToolStripMenuItem();
      this.tsTask_StatusWait = new ToolStripMenuItem();
      this.tsTask_StatusDeferred = new ToolStripMenuItem();
      this.statusBar = new GradientStatusStrip();
      this.ctxMenuStripSessionID = new ContextMenuStrip();
      this.sslHelp = new ToolStripStatusLabel();
      this.sslFieldID = new ToolStripStatusLabel();
      this.ssProgressBar = new ToolStripProgressBar();
      this.ssbInsights = new ToolStripButton();
      this.sslSeparator2 = new ToolStripStatusLabel();
      this.ssbTradeLoanUpdate = new ToolStripButton();
      this.ssbUploadProgress = new ToolStripButton();
      this.sslSeparator = new ToolStripStatusLabel();
      this.sslLastSaved = new ToolStripStatusLabel();
      this.sslDate = new ToolStripStatusLabel();
      this.pipelineMenu = new GradientMenuStrip();
      this.pipelineHeadItem = new ToolStripMenuItem();
      this.tsMenuItemNewLoan = new ToolStripMenuItem();
      this.tsMenuItemEditLoan = new ToolStripMenuItem();
      this.tsMenuItemOpenRecent = new ToolStripMenuItem();
      this.tsMenuItemOpenRecentPlaceHolder = new ToolStripMenuItem();
      this.tsMenuItemDuplicateLoan = new ToolStripMenuItem();
      this.tsMenuItemImport = new ToolStripMenuItem();
      this.toolStripSeparatorEx1 = new ToolStripSeparator();
      this.tsMenuItemMoveToFolder = new ToolStripMenuItem();
      this.tsMenuItemTransfer = new ToolStripMenuItem();
      this.tsMenuItemDeleteLoan = new ToolStripMenuItem();
      this.toolStripSeparatorEx2 = new ToolStripSeparator();
      this.tsMenuItemRefresh = new ToolStripMenuItem();
      this.tsMenuItemExportToExcel = new ToolStripMenuItem();
      this.tsExportSelectedLoansMenu = new ToolStripMenuItem();
      this.tsExportAllLoansMenu = new ToolStripMenuItem();
      this.tsMenuItemPrintForms = new ToolStripMenuItem();
      this.tsMenuItemManageAlerts = new ToolStripMenuItem();
      this.toolStripSeparatorEx4 = new ToolStripSeparator();
      this.tsMenuItemCustomizeColumns = new ToolStripMenuItem();
      this.tsMenuItemSaveView = new ToolStripMenuItem();
      this.tsMenuItemResetView = new ToolStripMenuItem();
      this.tsMenuItemManagePipelineViews = new ToolStripMenuItem();
      this.toolStripSeparatorOpenWebView = new ToolStripSeparator();
      this.tsMenuItemOpenWebView = new ToolStripMenuItem();
      this.tsOpenWebViewLoan = new ToolStripMenuItem();
      this.tsOpportunitiesMenu = new ToolStripMenuItem();
      this.tsProspectsMenu = new ToolStripMenuItem();
      this.toolStripSeparatorEx21 = new ToolStripSeparator();
      this.tsMenuItemExportLoan = new ToolStripMenuItem();
      this.loanMenu = new GradientMenuStrip();
      this.loanHeadItem = new ToolStripMenuItem();
      this.tsLoanNewLoan = new ToolStripMenuItem();
      this.tsLoanSaveLoan = new ToolStripMenuItem();
      this.tsLoanPrint = new ToolStripMenuItem();
      this.tsLoanExportToDataTrac = new ToolStripMenuItem();
      this.tsLoanExitLoan = new ToolStripMenuItem();
      this.toolStripSeparator14 = new ToolStripSeparator();
      this.tsLoanAddBorrowerPair = new ToolStripMenuItem();
      this.tsLoanManageBorrowers = new ToolStripMenuItem();
      this.tsDuplicateLockCheck = new ToolStripMenuItem();
      this.toolStripSeparator15 = new ToolStripSeparator();
      this.tsMilestoneDates = new ToolStripMenuItem();
      this.tsLockMilestoneDates = new ToolStripMenuItem();
      this.tsUnlockMilestoneDates = new ToolStripMenuItem();
      this.tsChangeMilestoneDates = new ToolStripMenuItem();
      this.tsMilestoneList = new ToolStripMenuItem();
      this.tsLockMilestoneList = new ToolStripMenuItem();
      this.tsUnlockMilestoneList = new ToolStripMenuItem();
      this.tsApplyMilestoneTemplate = new ToolStripMenuItem();
      this.toolStripSeparator19 = new ToolStripSeparator();
      this.tsLoanTemplateSet = new ToolStripMenuItem();
      this.tsLoanProgramTemplate = new ToolStripMenuItem();
      this.tsLoanClosingCost = new ToolStripMenuItem();
      this.tsLoanDocumentSet = new ToolStripMenuItem();
      this.tsLoanTaskSet = new ToolStripMenuItem();
      this.tsLoanDataTemplate = new ToolStripMenuItem();
      this.tsLoanInputFormSet = new ToolStripMenuItem();
      this.tsLoanSettlementServiceSet = new ToolStripMenuItem();
      this.tsLoanAffiliateTemplate = new ToolStripMenuItem();
      this.tsLoanRevertToDefaultFormList = new ToolStripMenuItem();
      this.toolStripSeparator16 = new ToolStripSeparator();
      this.tsLoanGoToField = new ToolStripMenuItem();
      this.formHeadItem = new ToolStripMenuItem();
      this.menuItem3 = new ToolStripMenuItem();
      this.verifHeadItem = new ToolStripMenuItem();
      this.vodItem = new ToolStripMenuItem();
      this.voggItem = new ToolStripMenuItem();
      this.vooiItem = new ToolStripMenuItem();
      this.vooaItem = new ToolStripMenuItem();
      this.voeItem = new ToolStripMenuItem();
      this.volItem = new ToolStripMenuItem();
      this.voolItem = new ToolStripMenuItem();
      this.vomItem = new ToolStripMenuItem();
      this.voalItem = new ToolStripMenuItem();
      this.vorItem = new ToolStripMenuItem();
      this.loanToolsHeadItem = new ToolStripMenuItem();
      this.fileItem = new ToolStripMenuItem();
      this.businessItem = new ToolStripMenuItem();
      this.conversationItem = new ToolStripMenuItem();
      this.correspondentLoanStatusMenuItem = new ToolStripMenuItem();
      this.tpoInformationMenuItem = new ToolStripMenuItem();
      this.taskItem = new ToolStripMenuItem();
      this.menuItem49 = new ToolStripMenuItem();
      this.ausTrackingItem = new ToolStripMenuItem();
      this.repWarrantTrackerItem = new ToolStripMenuItem();
      this.disclosureTrackingItem = new ToolStripMenuItem();
      this.feeVarianceWorksheetItem = new ToolStripMenuItem();
      this.loCompensationItem = new ToolStripMenuItem();
      this.antiSteeringSafeHarborItem = new ToolStripMenuItem();
      this.netTangibleBenefitItem = new ToolStripMenuItem();
      this.complianceReviewItem = new ToolStripMenuItem();
      this.ecsDataViewerItem = new ToolStripMenuItem();
      this.tqlServicesItem = new ToolStripMenuItem();
      this.miCenterItem = new ToolStripMenuItem();
      this.amortItem = new ToolStripMenuItem();
      this.lockRequestItem = new ToolStripMenuItem();
      this.lockComparisonItem = new ToolStripMenuItem();
      this.menuItem20 = new ToolStripSeparator();
      this.prequalItem = new ToolStripMenuItem();
      this.debtItem = new ToolStripMenuItem();
      this.comparisonItem = new ToolStripMenuItem();
      this.cashItem = new ToolStripMenuItem();
      this.rentItem = new ToolStripMenuItem();
      this.bankerLineItem = new ToolStripSeparator();
      this.underwriterItem = new ToolStripMenuItem();
      this.verificationTrackingItem = new ToolStripMenuItem();
      this.fundingItem = new ToolStripMenuItem();
      this.balanceItem = new ToolStripMenuItem();
      this.brokerItem = new ToolStripMenuItem();
      this.secondaryItem = new ToolStripMenuItem();
      this.interimServicingItem = new ToolStripMenuItem();
      this.shippingItem = new ToolStripMenuItem();
      this.documentTrackingMgmtItem = new ToolStripMenuItem();
      this.correspondentPurchaseItem = new ToolStripMenuItem();
      this.purchaseItem = new ToolStripMenuItem();
      this.menuItem19 = new ToolStripSeparator();
      this.comortgagorItem = new ToolStripMenuItem();
      this.piggybackLoanItem = new ToolStripMenuItem();
      this.secureItem = new ToolStripMenuItem();
      this.menuItem18 = new ToolStripSeparator();
      this.complianceItem = new ToolStripMenuItem();
      this.auditTrailItem = new ToolStripMenuItem();
      this.profitItem = new ToolStripMenuItem();
      this.trustItem = new ToolStripMenuItem();
      this.menuItem4 = new ToolStripSeparator();
      this.findFieldItem = new ToolStripMenuItem();
      this.worstCasePricingItem = new ToolStripMenuItem();
      this.menuItemServices = new ToolStripMenuItem();
      this.coMortgagerHeadItem = new ToolStripMenuItem();
      this.menuItem12 = new ToolStripSeparator();
      this.menuItem21 = new ToolStripMenuItem();
      this.menuItem16 = new ToolStripSeparator();
      this.SwapCurrentItem = new ToolStripMenuItem();
      this.deleteCurrentItem = new ToolStripMenuItem();
      this.menuItem23 = new ToolStripSeparator();
      this.menuItem24 = new ToolStripMenuItem();
      this.tsMenuItemTemplates = new ToolStripMenuItem();
      this.tsMenuItemSelectLoanTemplate = new ToolStripMenuItem();
      this.tsSeparator1 = new ToolStripSeparator();
      this.lpItem = new ToolStripMenuItem();
      this.ccItem = new ToolStripMenuItem();
      this.menuItem71 = new ToolStripSeparator();
      this.defaultFormsItem = new ToolStripMenuItem();
      this.selectFormsItem = new ToolStripMenuItem();
      this.menuItem74 = new ToolStripSeparator();
      this.addDocSetItem = new ToolStripMenuItem();
      this.addTaskSetItem = new ToolStripMenuItem();
      this.addDataSetItem = new ToolStripMenuItem();
      this.contactMenu = new GradientMenuStrip();
      this.contactHeadItem = new ToolStripMenuItem();
      this.menuItemBorContacts = new ToolStripMenuItem();
      this.menuItemBizContacts = new ToolStripMenuItem();
      this.menuItemCalendar = new ToolStripMenuItem();
      this.menuItemTasks = new ToolStripMenuItem();
      this.menuItemCompaign = new ToolStripMenuItem();
      this.importContactsItem = new ToolStripMenuItem();
      this.menuItemImportBorrowerContacts = new ToolStripMenuItem();
      this.menuItemImportBusinessContacts = new ToolStripMenuItem();
      this.menuItemExportDivider = new ToolStripSeparator();
      this.menuItemExportBorrowerContacts = new ToolStripMenuItem();
      this.menuItemExportBusinessContacts = new ToolStripMenuItem();
      this.menuItemSyncDivider = new ToolStripSeparator();
      this.menuItemSynchronize = new ToolStripMenuItem();
      this.reportMenu = new GradientMenuStrip();
      this.reportHeadItem = new ToolStripMenuItem();
      this.addReportItem = new ToolStripMenuItem();
      this.duplicateReportItem = new ToolStripMenuItem();
      this.newFolderReportItem = new ToolStripMenuItem();
      this.deleteReportItem = new ToolStripMenuItem();
      this.menuItem81 = new ToolStripSeparator();
      this.renameReportItem = new ToolStripMenuItem();
      this.refreshReportItem = new ToolStripMenuItem();
      this.menuItem70 = new ToolStripSeparator();
      this.generateReportMenuItem = new ToolStripMenuItem();
      this.tsMenuItemReportOption = new ToolStripMenuItem();
      this.dashboardMenu = new GradientMenuStrip();
      this.tsDashboardHeadItem = new ToolStripMenuItem();
      this.tsDashboard_ManageView = new ToolStripMenuItem();
      this.tsDashboard_ManageSnapshot = new ToolStripMenuItem();
      this.homeMenu = new GradientMenuStrip();
      this.homeMenuItem = new ToolStripMenuItem();
      this.epassMenu = new GradientMenuStrip();
      this.epassMenuItem = new ToolStripMenuItem();
      this.securityTradeListMenu = new GradientMenuStrip();
      this.securityTradeListItems = new ToolStripMenuItem();
      this.newSecurityTradeToolStripMenuItem = new ToolStripMenuItem();
      this.editSecurityTradeToolStripMenuItem = new ToolStripMenuItem();
      this.duplicateSecurityTradeToolStripMenuItem = new ToolStripMenuItem();
      this.deleteSecurityTradeToolStripMenuItem = new ToolStripMenuItem();
      this.exportSecurityTradeToolStripMenuItem = new ToolStripMenuItem();
      this.selectSecurityTradesOnlyToolStripMenuItem = new ToolStripMenuItem();
      this.allSecurityTradesonPageToolStripMenuItem = new ToolStripMenuItem();
      this.printSecurityTradeToolStripMenuItem = new ToolStripMenuItem();
      this.lockUnlockSecurityTradeToolStripMenuItem = new ToolStripMenuItem();
      this.archiveSecurityTradeToolStripMenuItem = new ToolStripMenuItem();
      this.securityTradeEditorMenu = new GradientMenuStrip();
      this.securityTradeEditorItems = new ToolStripMenuItem();
      this.saveSecurityTradeMenuItem = new ToolStripMenuItem();
      this.returnToSecurityTradeListItem = new ToolStripMenuItem();
      this.correspondentMasterContractListMenu = new GradientMenuStrip();
      this.toolStripMenuItem5 = new ToolStripMenuItem();
      this.newCMCMenuItem = new ToolStripMenuItem();
      this.deleteCMCMenuItem = new ToolStripMenuItem();
      this.exportCMCMenuItem = new ToolStripMenuItem();
      this.printCMCMenuItem = new ToolStripMenuItem();
      this.archiveCMCMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator20 = new ToolStripSeparator();
      this.createCMCTradeMenuItem = new ToolStripMenuItem();
      this.correspondentMasterContractEditorMenu = new GradientMenuStrip();
      this.correspondentMasterCommitmentEditorMenuItem = new ToolStripMenuItem();
      this.saveCMCMenuItem = new ToolStripMenuItem();
      this.exitCMCMenuItem = new ToolStripMenuItem();
      this.correspondentTradeListMenu = new GradientMenuStrip();
      this.correspondentLoanTradeStripMenuItem = new ToolStripMenuItem();
      this.newLoanTradeMenuItem = new ToolStripMenuItem();
      this.editLoanTradeMenuItem = new ToolStripMenuItem();
      this.duplicateTradeMenuItem = new ToolStripMenuItem();
      this.deleteTradeMenuItem = new ToolStripMenuItem();
      this.exportLoanTradeMenuItem = new ToolStripMenuItem();
      this.exportSelCorrespondentTradeMenuItem = new ToolStripMenuItem();
      this.exportAllCorrespondentTradeMenuItem = new ToolStripMenuItem();
      this.printLoanTradeMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator21 = new ToolStripSeparator();
      this.lockUnlockLoanTradeMenuItem = new ToolStripMenuItem();
      this.archiveLoanTradeStripMenuItem = new ToolStripMenuItem();
      this.voidLoanTradeStripMenuItem = new ToolStripMenuItem();
      this.updatePendingLoanMenuItem = new ToolStripMenuItem();
      this.correspondentTradeEditorMenu = new GradientMenuStrip();
      this.correspondentTradeMenuItem = new ToolStripMenuItem();
      this.saveCorrespondentTradeMenuItem = new ToolStripMenuItem();
      this.exitCorrespondentTradeMenuItem = new ToolStripMenuItem();
      this.GSECommitmentListMenu = new GradientMenuStrip();
      this.GSECommitmentListItems = new ToolStripMenuItem();
      this.newGSECommitmentToolStripMenuItem = new ToolStripMenuItem();
      this.editGSECommitmentToolStripMenuItem = new ToolStripMenuItem();
      this.duplicateGSECommitmentToolStripMenuItem = new ToolStripMenuItem();
      this.deleteGSECommitmentToolStripMenuItem = new ToolStripMenuItem();
      this.exportGSECommitmentToolStripMenuItem = new ToolStripMenuItem();
      this.selectGSECommitmentsOnlyToolStripMenuItem = new ToolStripMenuItem();
      this.allGSECommitmentsonPageToolStripMenuItem = new ToolStripMenuItem();
      this.printGSECommitmentToolStripMenuItem = new ToolStripMenuItem();
      this.GSECommitmentToolStripSeparator = new ToolStripSeparator();
      this.archiveGSECommitmentToolStripMenuItem = new ToolStripMenuItem();
      this.GSECommitmentEditorMenu = new GradientMenuStrip();
      this.GSECommitmentEditorMenuItems = new ToolStripMenuItem();
      this.saveGSECommitmentMenuItem = new ToolStripMenuItem();
      this.returnToGSECommitmentListItem = new ToolStripMenuItem();
      this.tsMenuItemEMMessage = new ToolStripMenuItem();
      this.mbsPoolEditorMenu.SuspendLayout();
      this.mbsPoolListMenu.SuspendLayout();
      this.loanSearchMenu.SuspendLayout();
      this.tradeEditorMenu.SuspendLayout();
      this.tradeListMenu.SuspendLayout();
      this.masterContractMenu.SuspendLayout();
      this.mainMenu.SuspendLayout();
      this.borrowerMenu.SuspendLayout();
      this.bizPartnerMenu.SuspendLayout();
      this.calendarMenu.SuspendLayout();
      this.campaignMenu.SuspendLayout();
      this.taskMenu.SuspendLayout();
      this.statusBar.SuspendLayout();
      this.pipelineMenu.SuspendLayout();
      this.loanMenu.SuspendLayout();
      this.contactMenu.SuspendLayout();
      this.reportMenu.SuspendLayout();
      this.dashboardMenu.SuspendLayout();
      this.homeMenu.SuspendLayout();
      this.epassMenu.SuspendLayout();
      this.securityTradeListMenu.SuspendLayout();
      this.securityTradeEditorMenu.SuspendLayout();
      this.correspondentMasterContractListMenu.SuspendLayout();
      this.correspondentMasterContractEditorMenu.SuspendLayout();
      this.correspondentTradeListMenu.SuspendLayout();
      this.correspondentTradeEditorMenu.SuspendLayout();
      this.GSECommitmentListMenu.SuspendLayout();
      this.GSECommitmentEditorMenu.SuspendLayout();
      this.SuspendLayout();
      this.gcTimer.Enabled = true;
      this.gcTimer.Interval = 1800000;
      this.gcTimer.Tick += new EventHandler(this.gcTimer_Tick);
      this.mainPanel.BackColor = System.Drawing.Color.WhiteSmoke;
      this.mainPanel.Dock = DockStyle.Fill;
      this.mainPanel.Location = new Point(0, 29);
      this.mainPanel.Name = "mainPanel";
      this.mainPanel.Size = new Size(792, 489);
      this.mainPanel.TabIndex = 1;
      this.mbsPoolEditorMenu.ImageScalingSize = new Size(20, 20);
      this.mbsPoolEditorMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.mbsPoolEditorItems
      });
      this.mbsPoolEditorMenu.Location = new Point(0, 24);
      this.mbsPoolEditorMenu.Name = "mbsPoolEditorMenu";
      this.mbsPoolEditorMenu.Size = new Size(792, 24);
      this.mbsPoolEditorMenu.TabIndex = 1;
      this.mbsPoolEditorMenu.Text = "gradientMenuStrip1";
      this.mbsPoolEditorItems.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.saveMbsPoolMenuItem,
        (ToolStripItem) this.returnToMbsPoolListItem
      });
      this.mbsPoolEditorItems.Name = "mbsPoolEditorItems";
      this.mbsPoolEditorItems.Size = new Size(108, 20);
      this.mbsPoolEditorItems.Text = "&MBS Pools";
      this.saveMbsPoolMenuItem.Name = "saveMbsPoolMenuItem";
      this.saveMbsPoolMenuItem.ShortcutKeys = Keys.S | Keys.Control;
      this.saveMbsPoolMenuItem.Size = new Size(275, 30);
      this.saveMbsPoolMenuItem.Tag = (object) "MBE_SaveTrade";
      this.saveMbsPoolMenuItem.Text = "&Save MBS Pool";
      this.saveMbsPoolMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.returnToMbsPoolListItem.Name = "returnToMbsPoolListItem";
      this.returnToMbsPoolListItem.Size = new Size(275, 30);
      this.returnToMbsPoolListItem.Tag = (object) "MBE_ExitTrade";
      this.returnToMbsPoolListItem.Text = "E&xit MBS Pool";
      this.returnToMbsPoolListItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.mbsPoolListMenu.ImageScalingSize = new Size(20, 20);
      this.mbsPoolListMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.mbsPoolListItems
      });
      this.mbsPoolListMenu.Location = new Point(0, 0);
      this.mbsPoolListMenu.Name = "mbsPoolListMenu";
      this.mbsPoolListMenu.Size = new Size(792, 24);
      this.mbsPoolListMenu.TabIndex = 0;
      this.mbsPoolListItems.DropDownItems.AddRange(new ToolStripItem[10]
      {
        (ToolStripItem) this.newMbsPoolToolStripMenuItem,
        (ToolStripItem) this.editMbsPoolToolStripMenuItem,
        (ToolStripItem) this.duplicateMbsPoolToolStripMenuItem,
        (ToolStripItem) this.deleteMbsPoolToolStripMenuItem,
        (ToolStripItem) this.exportMbsPoolToolStripMenuItem,
        (ToolStripItem) this.printMbsPoolToolStripMenuItem,
        (ToolStripItem) this.mbsPoolToolStripSeparator,
        (ToolStripItem) this.lockUnlockMbsPoolToolStripMenuItem,
        (ToolStripItem) this.archiveMbsPoolToolStripMenuItem,
        (ToolStripItem) this.updateMbsPoolToolStripMenuItem
      });
      this.mbsPoolListItems.Name = "mbsPoolListItems";
      this.mbsPoolListItems.Size = new Size(108, 20);
      this.mbsPoolListItems.Text = "&MBS Pools";
      this.mbsPoolListItems.DropDownOpening += new EventHandler(this.onTradeMenuOpening);
      this.newMbsPoolToolStripMenuItem.Image = (Image) Resources.new_file;
      this.newMbsPoolToolStripMenuItem.Name = "newMbsPoolToolStripMenuItem";
      this.newMbsPoolToolStripMenuItem.ShortcutKeys = Keys.N | Keys.Control;
      this.newMbsPoolToolStripMenuItem.Size = new Size(348, 30);
      this.newMbsPoolToolStripMenuItem.Tag = (object) "MBS_New";
      this.newMbsPoolToolStripMenuItem.Text = "&New MBS Pool...";
      this.newMbsPoolToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.editMbsPoolToolStripMenuItem.Image = (Image) Resources.edit;
      this.editMbsPoolToolStripMenuItem.Name = "editMbsPoolToolStripMenuItem";
      this.editMbsPoolToolStripMenuItem.Size = new Size(348, 30);
      this.editMbsPoolToolStripMenuItem.Tag = (object) "MBS_Edit";
      this.editMbsPoolToolStripMenuItem.Text = "&Edit MBS Pool...";
      this.editMbsPoolToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.duplicateMbsPoolToolStripMenuItem.Image = (Image) Resources.duplicate;
      this.duplicateMbsPoolToolStripMenuItem.Name = "duplicateMbsPoolToolStripMenuItem";
      this.duplicateMbsPoolToolStripMenuItem.Size = new Size(348, 30);
      this.duplicateMbsPoolToolStripMenuItem.Tag = (object) "MBS_Duplicate";
      this.duplicateMbsPoolToolStripMenuItem.Text = "D&uplicate MBS Pool";
      this.duplicateMbsPoolToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.deleteMbsPoolToolStripMenuItem.Image = (Image) Resources.delete;
      this.deleteMbsPoolToolStripMenuItem.Name = "deleteMbsPoolToolStripMenuItem";
      this.deleteMbsPoolToolStripMenuItem.ShortcutKeys = Keys.D | Keys.Alt;
      this.deleteMbsPoolToolStripMenuItem.Size = new Size(348, 30);
      this.deleteMbsPoolToolStripMenuItem.Tag = (object) "MBS_Delete";
      this.deleteMbsPoolToolStripMenuItem.Text = "&Delete MBS Pool...";
      this.deleteMbsPoolToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.exportMbsPoolToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.selectedMbsPoolsOnlyToolStripMenuItem,
        (ToolStripItem) this.allMbsPoolsonPageToolStripMenuItem
      });
      this.exportMbsPoolToolStripMenuItem.Image = (Image) Resources.excel;
      this.exportMbsPoolToolStripMenuItem.Name = "exportMbsPoolToolStripMenuItem";
      this.exportMbsPoolToolStripMenuItem.ShortcutKeys = Keys.E | Keys.Alt;
      this.exportMbsPoolToolStripMenuItem.Size = new Size(348, 30);
      this.exportMbsPoolToolStripMenuItem.Tag = (object) "MBS_Export";
      this.exportMbsPoolToolStripMenuItem.Text = "&Export MBS Pool to Excel";
      this.exportMbsPoolToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.selectedMbsPoolsOnlyToolStripMenuItem.Name = "selectedMbsPoolsOnlyToolStripMenuItem";
      this.selectedMbsPoolsOnlyToolStripMenuItem.Size = new Size(319, 30);
      this.selectedMbsPoolsOnlyToolStripMenuItem.Tag = (object) "MBS_ExportSelected";
      this.selectedMbsPoolsOnlyToolStripMenuItem.Text = "&Selected MBS Pools Only...";
      this.selectedMbsPoolsOnlyToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.allMbsPoolsonPageToolStripMenuItem.Name = "allMbsPoolsonPageToolStripMenuItem";
      this.allMbsPoolsonPageToolStripMenuItem.Size = new Size(319, 30);
      this.allMbsPoolsonPageToolStripMenuItem.Tag = (object) "MBS_ExportAll";
      this.allMbsPoolsonPageToolStripMenuItem.Text = "&All MBS Pools on All Pages...";
      this.allMbsPoolsonPageToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.printMbsPoolToolStripMenuItem.Image = (Image) Resources.print;
      this.printMbsPoolToolStripMenuItem.Name = "printMbsPoolToolStripMenuItem";
      this.printMbsPoolToolStripMenuItem.ShortcutKeys = Keys.P | Keys.Alt;
      this.printMbsPoolToolStripMenuItem.Size = new Size(348, 30);
      this.printMbsPoolToolStripMenuItem.Tag = (object) "MBS_Print";
      this.printMbsPoolToolStripMenuItem.Text = "&Print MBS Pool...";
      this.printMbsPoolToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.mbsPoolToolStripSeparator.Name = "mbsPoolToolStripSeparator";
      this.mbsPoolToolStripSeparator.Size = new Size(345, 6);
      this.lockUnlockMbsPoolToolStripMenuItem.Name = "lockUnlockMbsPoolToolStripMenuItem";
      this.lockUnlockMbsPoolToolStripMenuItem.Size = new Size(348, 30);
      this.lockUnlockMbsPoolToolStripMenuItem.Tag = (object) "MBS_Lock";
      this.lockUnlockMbsPoolToolStripMenuItem.Text = "&Lock/Unlock MBS Pool";
      this.lockUnlockMbsPoolToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.archiveMbsPoolToolStripMenuItem.Name = "archiveMbsPoolToolStripMenuItem";
      this.archiveMbsPoolToolStripMenuItem.Size = new Size(348, 30);
      this.archiveMbsPoolToolStripMenuItem.Tag = (object) "MBS_Archive";
      this.archiveMbsPoolToolStripMenuItem.Text = "&Archive MBS Pool";
      this.archiveMbsPoolToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.updateMbsPoolToolStripMenuItem.Name = "updateMbsPoolToolStripMenuItem";
      this.updateMbsPoolToolStripMenuItem.Size = new Size(348, 30);
      this.updateMbsPoolToolStripMenuItem.Tag = (object) "MBS_UpdatePendingLoans";
      this.updateMbsPoolToolStripMenuItem.Text = "Update &Pending Loans";
      this.updateMbsPoolToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.loanSearchMenu.ImageScalingSize = new Size(20, 20);
      this.loanSearchMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.loanSearchItems
      });
      this.loanSearchMenu.Location = new Point(0, 0);
      this.loanSearchMenu.Name = "loanSearchMenu";
      this.loanSearchMenu.Size = new Size(792, 24);
      this.loanSearchMenu.TabIndex = 0;
      this.loanSearchItems.DropDownItems.AddRange(new ToolStripItem[12]
      {
        (ToolStripItem) this.searchLoansToolStripMenuItem,
        (ToolStripItem) this.clearLoansToolStripMenuItem,
        (ToolStripItem) this.toolStripMenuItem1,
        (ToolStripItem) this.exportToEToolStripMenuItem,
        (ToolStripItem) this.createTradeToolStripMenuItem,
        (ToolStripItem) this.createMbsPoolToolStripMenuItem2,
        (ToolStripItem) this.assignToTradeToolStripMenuItem,
        (ToolStripItem) this.assignToMbsPoolStripMenuItem,
        (ToolStripItem) this.toolStripMenuItem2,
        (ToolStripItem) this.saveViewToolStripMenuItem,
        (ToolStripItem) this.resetViewToolStripMenuItem,
        (ToolStripItem) this.manageLoanSearchViewsToolStripMenuItem
      });
      this.loanSearchItems.Name = "loanSearchItems";
      this.loanSearchItems.Size = new Size(119, 20);
      this.loanSearchItems.Text = "&Loan Search";
      this.loanSearchItems.DropDownOpening += new EventHandler(this.onTradeMenuOpening);
      this.searchLoansToolStripMenuItem.Name = "searchLoansToolStripMenuItem";
      this.searchLoansToolStripMenuItem.Size = new Size(322, 30);
      this.searchLoansToolStripMenuItem.Tag = (object) "LS_Search";
      this.searchLoansToolStripMenuItem.Text = "&Search Loans";
      this.searchLoansToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.clearLoansToolStripMenuItem.Name = "clearLoansToolStripMenuItem";
      this.clearLoansToolStripMenuItem.Size = new Size(322, 30);
      this.clearLoansToolStripMenuItem.Tag = (object) "LS_Clear";
      this.clearLoansToolStripMenuItem.Text = "C&lear Search";
      this.clearLoansToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new Size(319, 6);
      this.exportToEToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.loanSearchExportSelectedMenu,
        (ToolStripItem) this.loanSearchExportAllMenu
      });
      this.exportToEToolStripMenuItem.Image = (Image) Resources.excel;
      this.exportToEToolStripMenuItem.Name = "exportToEToolStripMenuItem";
      this.exportToEToolStripMenuItem.Size = new Size(322, 30);
      this.exportToEToolStripMenuItem.Tag = (object) "LS_Export";
      this.exportToEToolStripMenuItem.Text = "Export to E&xcel";
      this.loanSearchExportSelectedMenu.Name = "loanSearchExportSelectedMenu";
      this.loanSearchExportSelectedMenu.Size = new Size(281, 30);
      this.loanSearchExportSelectedMenu.Tag = (object) "LS_ExportSelected";
      this.loanSearchExportSelectedMenu.Text = "&Selected Loans Only...";
      this.loanSearchExportSelectedMenu.Click += new EventHandler(this.TradesMenuItem__Click);
      this.loanSearchExportAllMenu.Name = "loanSearchExportAllMenu";
      this.loanSearchExportAllMenu.Size = new Size(281, 30);
      this.loanSearchExportAllMenu.Tag = (object) "LS_ExportAll";
      this.loanSearchExportAllMenu.Text = "&All Loans on All Pages...";
      this.loanSearchExportAllMenu.Click += new EventHandler(this.TradesMenuItem__Click);
      this.createTradeToolStripMenuItem.Image = (Image) Resources.new_file;
      this.createTradeToolStripMenuItem.Name = "createTradeToolStripMenuItem";
      this.createTradeToolStripMenuItem.Size = new Size(322, 30);
      this.createTradeToolStripMenuItem.Tag = (object) "LS_CreateTrade";
      this.createTradeToolStripMenuItem.Text = "&Create Trade...";
      this.createTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.createMbsPoolToolStripMenuItem2.Image = (Image) Resources.new_file;
      this.createMbsPoolToolStripMenuItem2.Name = "createMbsPoolToolStripMenuItem2";
      this.createMbsPoolToolStripMenuItem2.Size = new Size(322, 30);
      this.createMbsPoolToolStripMenuItem2.Tag = (object) "LS_CreateMbsPool";
      this.createMbsPoolToolStripMenuItem2.Text = "Create &MBS Pool";
      this.createMbsPoolToolStripMenuItem2.Click += new EventHandler(this.TradesMenuItem__Click);
      this.assignToTradeToolStripMenuItem.Name = "assignToTradeToolStripMenuItem";
      this.assignToTradeToolStripMenuItem.Size = new Size(322, 30);
      this.assignToTradeToolStripMenuItem.Tag = (object) "LS_AssignToTrade";
      this.assignToTradeToolStripMenuItem.Text = "&Assign to Trade";
      this.assignToTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.assignToMbsPoolStripMenuItem.Name = "assignToMbsPoolStripMenuItem";
      this.assignToMbsPoolStripMenuItem.Size = new Size(322, 30);
      this.assignToMbsPoolStripMenuItem.Tag = (object) "LS_AssignToMbsPool";
      this.assignToMbsPoolStripMenuItem.Text = "Assign To MBS &Pool";
      this.assignToMbsPoolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new Size(319, 6);
      this.saveViewToolStripMenuItem.Name = "saveViewToolStripMenuItem";
      this.saveViewToolStripMenuItem.Size = new Size(322, 30);
      this.saveViewToolStripMenuItem.Tag = (object) "LS_SaveView";
      this.saveViewToolStripMenuItem.Text = "Sav&e View";
      this.saveViewToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.resetViewToolStripMenuItem.Name = "resetViewToolStripMenuItem";
      this.resetViewToolStripMenuItem.Size = new Size(322, 30);
      this.resetViewToolStripMenuItem.Tag = (object) "LS_ResetView";
      this.resetViewToolStripMenuItem.Text = "&Reset View";
      this.resetViewToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.manageLoanSearchViewsToolStripMenuItem.Name = "manageLoanSearchViewsToolStripMenuItem";
      this.manageLoanSearchViewsToolStripMenuItem.Size = new Size(322, 30);
      this.manageLoanSearchViewsToolStripMenuItem.Tag = (object) "LS_ManageViews";
      this.manageLoanSearchViewsToolStripMenuItem.Text = "Manage Loan Search Vie&ws...";
      this.manageLoanSearchViewsToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.tradeEditorMenu.ImageScalingSize = new Size(20, 20);
      this.tradeEditorMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.tradeEditorItems
      });
      this.tradeEditorMenu.Location = new Point(0, 0);
      this.tradeEditorMenu.Name = "tradeEditorMenu";
      this.tradeEditorMenu.Size = new Size(792, 24);
      this.tradeEditorMenu.TabIndex = 0;
      this.tradeEditorItems.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.saveTradeMenuItem,
        (ToolStripItem) this.returnToTradeListItem
      });
      this.tradeEditorItems.Name = "tradeEditorItems";
      this.tradeEditorItems.Size = new Size(74, 20);
      this.tradeEditorItems.Text = "&Trades";
      this.saveTradeMenuItem.Name = "saveTradeMenuItem";
      this.saveTradeMenuItem.ShortcutKeys = Keys.S | Keys.Control;
      this.saveTradeMenuItem.Size = new Size(241, 30);
      this.saveTradeMenuItem.Tag = (object) "TE_SaveTrade";
      this.saveTradeMenuItem.Text = "&Save Trade";
      this.saveTradeMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.returnToTradeListItem.Name = "returnToTradeListItem";
      this.returnToTradeListItem.Size = new Size(241, 30);
      this.returnToTradeListItem.Tag = (object) "TE_ExitTrade";
      this.returnToTradeListItem.Text = "E&xit Trade";
      this.returnToTradeListItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.tradeListMenu.ImageScalingSize = new Size(20, 20);
      this.tradeListMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.tradeListItems
      });
      this.tradeListMenu.Location = new Point(0, 0);
      this.tradeListMenu.Name = "tradeListMenu";
      this.tradeListMenu.Size = new Size(792, 24);
      this.tradeListMenu.TabIndex = 0;
      this.tradeListItems.DropDownItems.AddRange(new ToolStripItem[10]
      {
        (ToolStripItem) this.newTradeToolStripMenuItem,
        (ToolStripItem) this.editTradeToolStripMenuItem,
        (ToolStripItem) this.duplicateTradeToolStripMenuItem,
        (ToolStripItem) this.deleteTradeToolStripMenuItem,
        (ToolStripItem) this.exportTradeToolStripMenuItem,
        (ToolStripItem) this.printTradeToolStripMenuItem,
        (ToolStripItem) this.toolStripMenuItem3,
        (ToolStripItem) this.lockUnlockTradeToolStripMenuItem,
        (ToolStripItem) this.archiveTradeToolStripMenuItem,
        (ToolStripItem) this.updatePendingLoansToolStripMenuItem
      });
      this.tradeListItems.Name = "tradeListItems";
      this.tradeListItems.Size = new Size(117, 20);
      this.tradeListItems.Text = "&Loan Trades";
      this.tradeListItems.DropDownOpening += new EventHandler(this.onTradeMenuOpening);
      this.newTradeToolStripMenuItem.Image = (Image) Resources.new_file;
      this.newTradeToolStripMenuItem.Name = "newTradeToolStripMenuItem";
      this.newTradeToolStripMenuItem.ShortcutKeys = Keys.N | Keys.Control;
      this.newTradeToolStripMenuItem.Size = new Size(314, 30);
      this.newTradeToolStripMenuItem.Tag = (object) "TR_New";
      this.newTradeToolStripMenuItem.Text = "&New Trade...";
      this.newTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.editTradeToolStripMenuItem.Image = (Image) Resources.edit;
      this.editTradeToolStripMenuItem.Name = "editTradeToolStripMenuItem";
      this.editTradeToolStripMenuItem.Size = new Size(314, 30);
      this.editTradeToolStripMenuItem.Tag = (object) "TR_Edit";
      this.editTradeToolStripMenuItem.Text = "&Edit Trade...";
      this.editTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.duplicateTradeToolStripMenuItem.Image = (Image) Resources.duplicate;
      this.duplicateTradeToolStripMenuItem.Name = "duplicateTradeToolStripMenuItem";
      this.duplicateTradeToolStripMenuItem.Size = new Size(314, 30);
      this.duplicateTradeToolStripMenuItem.Tag = (object) "TR_Duplicate";
      this.duplicateTradeToolStripMenuItem.Text = "D&uplicate Trade";
      this.duplicateTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.deleteTradeToolStripMenuItem.Image = (Image) Resources.delete;
      this.deleteTradeToolStripMenuItem.Name = "deleteTradeToolStripMenuItem";
      this.deleteTradeToolStripMenuItem.ShortcutKeys = Keys.D | Keys.Alt;
      this.deleteTradeToolStripMenuItem.Size = new Size(314, 30);
      this.deleteTradeToolStripMenuItem.Tag = (object) "TR_Delete";
      this.deleteTradeToolStripMenuItem.Text = "&Delete Trade...";
      this.deleteTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.exportTradeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.selectedTradesOnlyToolStripMenuItem,
        (ToolStripItem) this.allTradesonPageToolStripMenuItem
      });
      this.exportTradeToolStripMenuItem.Image = (Image) Resources.excel;
      this.exportTradeToolStripMenuItem.Name = "exportTradeToolStripMenuItem";
      this.exportTradeToolStripMenuItem.ShortcutKeys = Keys.E | Keys.Alt;
      this.exportTradeToolStripMenuItem.Size = new Size(314, 30);
      this.exportTradeToolStripMenuItem.Tag = (object) "TR_Export";
      this.exportTradeToolStripMenuItem.Text = "&Export Trade to Excel";
      this.exportTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.selectedTradesOnlyToolStripMenuItem.Name = "selectedTradesOnlyToolStripMenuItem";
      this.selectedTradesOnlyToolStripMenuItem.Size = new Size(285, 30);
      this.selectedTradesOnlyToolStripMenuItem.Tag = (object) "TR_ExportSelected";
      this.selectedTradesOnlyToolStripMenuItem.Text = "&Selected Trades Only...";
      this.selectedTradesOnlyToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.allTradesonPageToolStripMenuItem.Name = "allTradesonPageToolStripMenuItem";
      this.allTradesonPageToolStripMenuItem.Size = new Size(285, 30);
      this.allTradesonPageToolStripMenuItem.Tag = (object) "TR_ExportAll";
      this.allTradesonPageToolStripMenuItem.Text = "&All Trades on All Pages...";
      this.allTradesonPageToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.printTradeToolStripMenuItem.Image = (Image) Resources.print;
      this.printTradeToolStripMenuItem.Name = "printTradeToolStripMenuItem";
      this.printTradeToolStripMenuItem.ShortcutKeys = Keys.P | Keys.Alt;
      this.printTradeToolStripMenuItem.Size = new Size(314, 30);
      this.printTradeToolStripMenuItem.Tag = (object) "TR_Print";
      this.printTradeToolStripMenuItem.Text = "&Print Trade...";
      this.printTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.toolStripMenuItem3.Name = "toolStripMenuItem3";
      this.toolStripMenuItem3.Size = new Size(311, 6);
      this.lockUnlockTradeToolStripMenuItem.Name = "lockUnlockTradeToolStripMenuItem";
      this.lockUnlockTradeToolStripMenuItem.Size = new Size(314, 30);
      this.lockUnlockTradeToolStripMenuItem.Tag = (object) "TR_Lock";
      this.lockUnlockTradeToolStripMenuItem.Text = "&Lock/Unlock Trade";
      this.lockUnlockTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.archiveTradeToolStripMenuItem.Name = "archiveTradeToolStripMenuItem";
      this.archiveTradeToolStripMenuItem.Size = new Size(314, 30);
      this.archiveTradeToolStripMenuItem.Tag = (object) "TR_Archive";
      this.archiveTradeToolStripMenuItem.Text = "&Archive Trade";
      this.archiveTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.updatePendingLoansToolStripMenuItem.Name = "updatePendingLoansToolStripMenuItem";
      this.updatePendingLoansToolStripMenuItem.Size = new Size(314, 30);
      this.updatePendingLoansToolStripMenuItem.Tag = (object) "TR_UpdatePendingLoans";
      this.updatePendingLoansToolStripMenuItem.Text = "Update &Pending Loans";
      this.updatePendingLoansToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.masterContractMenu.ImageScalingSize = new Size(20, 20);
      this.masterContractMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.masterContractItems
      });
      this.masterContractMenu.Location = new Point(0, 0);
      this.masterContractMenu.Name = "masterContractMenu";
      this.masterContractMenu.Size = new Size(792, 24);
      this.masterContractMenu.TabIndex = 0;
      this.masterContractItems.DropDownItems.AddRange(new ToolStripItem[10]
      {
        (ToolStripItem) this.newContractToolStripMenuItem,
        (ToolStripItem) this.saveContractToolStripMenuItem,
        (ToolStripItem) this.deleteContractToolStripMenuItem,
        (ToolStripItem) this.exportContractToolStripMenuItem,
        (ToolStripItem) this.printContractToolStripMenuItem,
        (ToolStripItem) this.archiveContractToolStripMenuItem,
        (ToolStripItem) this.activateContractToolStripMenuItem,
        (ToolStripItem) this.toolStripMenuItem4,
        (ToolStripItem) this.createTradeToolStripMenuItem1,
        (ToolStripItem) this.createMBSPoolToolStripMenuItem
      });
      this.masterContractItems.Name = "masterContractItems";
      this.masterContractItems.Size = new Size(158, 20);
      this.masterContractItems.Text = "&Master Contracts";
      this.masterContractItems.DropDownOpening += new EventHandler(this.onTradeMenuOpening);
      this.newContractToolStripMenuItem.Image = (Image) Resources.new_file;
      this.newContractToolStripMenuItem.Name = "newContractToolStripMenuItem";
      this.newContractToolStripMenuItem.ShortcutKeys = Keys.N | Keys.Control;
      this.newContractToolStripMenuItem.Size = new Size(303, 30);
      this.newContractToolStripMenuItem.Tag = (object) "MC_New";
      this.newContractToolStripMenuItem.Text = "&New Contract...";
      this.newContractToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.saveContractToolStripMenuItem.Image = (Image) Resources.save;
      this.saveContractToolStripMenuItem.Name = "saveContractToolStripMenuItem";
      this.saveContractToolStripMenuItem.ShortcutKeys = Keys.S | Keys.Control;
      this.saveContractToolStripMenuItem.Size = new Size(303, 30);
      this.saveContractToolStripMenuItem.Tag = (object) "MC_Save";
      this.saveContractToolStripMenuItem.Text = "&Save Contract";
      this.saveContractToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.deleteContractToolStripMenuItem.Image = (Image) Resources.delete;
      this.deleteContractToolStripMenuItem.Name = "deleteContractToolStripMenuItem";
      this.deleteContractToolStripMenuItem.ShortcutKeys = Keys.D | Keys.Alt;
      this.deleteContractToolStripMenuItem.Size = new Size(303, 30);
      this.deleteContractToolStripMenuItem.Tag = (object) "MC_Delete";
      this.deleteContractToolStripMenuItem.Text = "&Delete Contract";
      this.deleteContractToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.exportContractToolStripMenuItem.Image = (Image) Resources.excel;
      this.exportContractToolStripMenuItem.Name = "exportContractToolStripMenuItem";
      this.exportContractToolStripMenuItem.ShortcutKeys = Keys.E | Keys.Control;
      this.exportContractToolStripMenuItem.Size = new Size(303, 30);
      this.exportContractToolStripMenuItem.Tag = (object) "MC_Export";
      this.exportContractToolStripMenuItem.Text = "&Export Contract...";
      this.exportContractToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.printContractToolStripMenuItem.Image = (Image) Resources.print;
      this.printContractToolStripMenuItem.Name = "printContractToolStripMenuItem";
      this.printContractToolStripMenuItem.ShortcutKeys = Keys.P | Keys.Control;
      this.printContractToolStripMenuItem.Size = new Size(303, 30);
      this.printContractToolStripMenuItem.Tag = (object) "MC_Print";
      this.printContractToolStripMenuItem.Text = "&Print Contract...";
      this.printContractToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.archiveContractToolStripMenuItem.Name = "archiveContractToolStripMenuItem";
      this.archiveContractToolStripMenuItem.ShortcutKeys = Keys.R | Keys.Control;
      this.archiveContractToolStripMenuItem.Size = new Size(303, 30);
      this.archiveContractToolStripMenuItem.Tag = (object) "MC_Archive";
      this.archiveContractToolStripMenuItem.Text = "&Archive Contract...";
      this.archiveContractToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.activateContractToolStripMenuItem.Name = "activateContractToolStripMenuItem";
      this.activateContractToolStripMenuItem.ShortcutKeys = Keys.T | Keys.Control;
      this.activateContractToolStripMenuItem.Size = new Size(303, 30);
      this.activateContractToolStripMenuItem.Tag = (object) "MC_Activate";
      this.activateContractToolStripMenuItem.Text = "&Activate Contract...";
      this.activateContractToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.toolStripMenuItem4.Name = "toolStripMenuItem4";
      this.toolStripMenuItem4.Size = new Size(300, 6);
      this.createTradeToolStripMenuItem1.Name = "createTradeToolStripMenuItem1";
      this.createTradeToolStripMenuItem1.Size = new Size(303, 30);
      this.createTradeToolStripMenuItem1.Tag = (object) "MC_CreateTrade";
      this.createTradeToolStripMenuItem1.Text = "&Create Trade";
      this.createTradeToolStripMenuItem1.Click += new EventHandler(this.TradesMenuItem__Click);
      this.createMBSPoolToolStripMenuItem.Name = "createMBSPoolToolStripMenuItem";
      this.createMBSPoolToolStripMenuItem.Size = new Size(303, 30);
      this.createMBSPoolToolStripMenuItem.Tag = (object) "MC_CreateMBSPool";
      this.createMBSPoolToolStripMenuItem.Text = "&Create MBS Pool";
      this.createMBSPoolToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.checkIdleTimer.Tick += new EventHandler(this.checkIdleTimer_Tick);
      this.mainMenu.ImageScalingSize = new Size(20, 20);
      this.mainMenu.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.tsMenuItemEncompass,
        (ToolStripItem) this.tsMenuItemView,
        (ToolStripItem) this.tsMenuItemHelp
      });
      this.mainMenu.Location = new Point(0, 0);
      this.mainMenu.Name = "mainMenu";
      this.mainMenu.Padding = new Padding(1, 0, 0, 0);
      this.mainMenu.Size = new Size(792, 29);
      this.mainMenu.TabIndex = 0;
      this.tsMenuItemEncompass.BackColor = SystemColors.Control;
      this.tsMenuItemEncompass.DropDownItems.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.tsMenuItemIM,
        (ToolStripItem) this.tsMenuItemMailbox,
        (ToolStripItem) this.tsMenuItemCalculator,
        (ToolStripItem) this.menuItem9,
        (ToolStripItem) this.tsMenuItemCompanySettings,
        (ToolStripItem) this.menuItem13,
        (ToolStripItem) this.tsMenuItemExit
      });
      this.tsMenuItemEncompass.Name = "tsMenuItemEncompass";
      this.tsMenuItemEncompass.Size = new Size(114, 29);
      this.tsMenuItemEncompass.Text = "&Encompass";
      this.tsMenuItemEncompass.DropDownOpening += new EventHandler(this.tabHeadItem_Popup);
      this.tsMenuItemIM.BackColor = SystemColors.Control;
      this.tsMenuItemIM.BackgroundImageLayout = ImageLayout.None;
      this.tsMenuItemIM.Image = (Image) componentResourceManager.GetObject("tsMenuItemIM.Image");
      this.tsMenuItemIM.ImageTransparentColor = System.Drawing.Color.White;
      this.tsMenuItemIM.Name = "tsMenuItemIM";
      this.tsMenuItemIM.ShortcutKeys = Keys.I | Keys.Control;
      this.tsMenuItemIM.Size = new Size(309, 30);
      this.tsMenuItemIM.Text = "&Instant Messenger...";
      this.tsMenuItemIM.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemMailbox.Image = (Image) componentResourceManager.GetObject("tsMenuItemMailbox.Image");
      this.tsMenuItemMailbox.Name = "tsMenuItemMailbox";
      this.tsMenuItemMailbox.ShortcutKeys = Keys.M | Keys.Control;
      this.tsMenuItemMailbox.Size = new Size(309, 30);
      this.tsMenuItemMailbox.Text = "Loan &Mailbox...";
      this.tsMenuItemMailbox.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemCalculator.Image = (Image) Resources.calculator;
      this.tsMenuItemCalculator.Name = "tsMenuItemCalculator";
      this.tsMenuItemCalculator.ShortcutKeys = Keys.U | Keys.Control;
      this.tsMenuItemCalculator.Size = new Size(309, 30);
      this.tsMenuItemCalculator.Text = "Calc&ulator...";
      this.tsMenuItemCalculator.Click += new EventHandler(this.mainMenu_Click);
      this.menuItem9.Name = "menuItem9";
      this.menuItem9.Size = new Size(306, 6);
      this.tsMenuItemCompanySettings.Name = "tsMenuItemCompanySettings";
      this.tsMenuItemCompanySettings.Size = new Size(309, 30);
      this.tsMenuItemCompanySettings.Text = "&Settings...";
      this.tsMenuItemCompanySettings.Click += new EventHandler(this.mainMenu_Click);
      this.menuItem13.Name = "menuItem13";
      this.menuItem13.Size = new Size(306, 6);
      this.tsMenuItemExit.Name = "tsMenuItemExit";
      this.tsMenuItemExit.ShortcutKeys = Keys.F4 | Keys.Alt;
      this.tsMenuItemExit.Size = new Size(309, 30);
      this.tsMenuItemExit.Text = "E&xit";
      this.tsMenuItemExit.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemView.DropDownItems.AddRange(new ToolStripItem[9]
      {
        (ToolStripItem) this.tsMenuItemHome,
        (ToolStripItem) this.tsMenuItemPipeline,
        (ToolStripItem) this.tsMenuItemLoan,
        (ToolStripItem) this.tsMenuItemEpass,
        (ToolStripItem) this.tsMenuItemEfolder,
        (ToolStripItem) this.tsMenuItemTrades,
        (ToolStripItem) this.tsMenuItemContacts,
        (ToolStripItem) this.tsMenuItemDashboard,
        (ToolStripItem) this.tsMenuItemReports
      });
      this.tsMenuItemView.Name = "tsMenuItemView";
      this.tsMenuItemView.Size = new Size(61, 29);
      this.tsMenuItemView.Text = "&View";
      this.tsMenuItemView.DropDownOpening += new EventHandler(this.tabHeadItem_Popup);
      this.tsMenuItemHome.Name = "tsMenuItemHome";
      this.tsMenuItemHome.ShortcutKeys = Keys.H | Keys.Shift | Keys.Control;
      this.tsMenuItemHome.Size = new Size(310, 30);
      this.tsMenuItemHome.Text = "&Home";
      this.tsMenuItemHome.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemPipeline.Name = "tsMenuItemPipeline";
      this.tsMenuItemPipeline.ShortcutKeys = Keys.P | Keys.Shift | Keys.Control;
      this.tsMenuItemPipeline.Size = new Size(310, 30);
      this.tsMenuItemPipeline.Text = "&Pipeline";
      this.tsMenuItemPipeline.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemLoan.Enabled = false;
      this.tsMenuItemLoan.Name = "tsMenuItemLoan";
      this.tsMenuItemLoan.ShortcutKeys = Keys.L | Keys.Shift | Keys.Control;
      this.tsMenuItemLoan.Size = new Size(310, 30);
      this.tsMenuItemLoan.Text = "&Loan";
      this.tsMenuItemLoan.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemEpass.Enabled = false;
      this.tsMenuItemEpass.Name = "tsMenuItemEpass";
      this.tsMenuItemEpass.ShortcutKeys = Keys.S | Keys.Shift | Keys.Control;
      this.tsMenuItemEpass.Size = new Size(310, 30);
      this.tsMenuItemEpass.Text = "&Services View";
      this.tsMenuItemEpass.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemEfolder.Enabled = false;
      this.tsMenuItemEfolder.Name = "tsMenuItemEfolder";
      this.tsMenuItemEfolder.ShortcutKeys = Keys.F | Keys.Shift | Keys.Control;
      this.tsMenuItemEfolder.Size = new Size(310, 30);
      this.tsMenuItemEfolder.Text = "e&Folder";
      this.tsMenuItemEfolder.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemTrades.DropDownItems.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.tsMenuItemSecurityTradeManagement,
        (ToolStripItem) this.tsMenuItemLoanSearch,
        (ToolStripItem) this.tsMenuItemTradeManagement,
        (ToolStripItem) this.tsStripMenuItemMasterContacts
      });
      this.tsMenuItemTrades.Name = "tsMenuItemTrades";
      this.tsMenuItemTrades.ShortcutKeys = Keys.T | Keys.Shift | Keys.Control;
      this.tsMenuItemTrades.Size = new Size(310, 30);
      this.tsMenuItemTrades.Text = "&Trades";
      this.tsMenuItemTrades.MouseHover += new EventHandler(this.tsMenuItemTrades_MouseHover);
      this.tsMenuItemSecurityTradeManagement.Image = (Image) componentResourceManager.GetObject("tsMenuItemSecurityTradeManagement.Image");
      this.tsMenuItemSecurityTradeManagement.Name = "tsMenuItemSecurityTradeManagement";
      this.tsMenuItemSecurityTradeManagement.ShortcutKeys = Keys.S | Keys.Shift | Keys.Control;
      this.tsMenuItemSecurityTradeManagement.Size = new Size(341, 30);
      this.tsMenuItemSecurityTradeManagement.Text = "&Security Trades";
      this.tsMenuItemSecurityTradeManagement.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemLoanSearch.Image = (Image) componentResourceManager.GetObject("tsMenuItemLoanSearch.Image");
      this.tsMenuItemLoanSearch.Name = "tsMenuItemLoanSearch";
      this.tsMenuItemLoanSearch.ShortcutKeys = Keys.N | Keys.Shift | Keys.Control;
      this.tsMenuItemLoanSearch.Size = new Size(341, 30);
      this.tsMenuItemLoanSearch.Text = "Loa&n Search";
      this.tsMenuItemLoanSearch.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemTradeManagement.Image = (Image) componentResourceManager.GetObject("tsMenuItemTradeManagement.Image");
      this.tsMenuItemTradeManagement.Name = "tsMenuItemTradeManagement";
      this.tsMenuItemTradeManagement.ShortcutKeys = Keys.L | Keys.Shift | Keys.Control;
      this.tsMenuItemTradeManagement.Size = new Size(341, 30);
      this.tsMenuItemTradeManagement.Text = "&Loan Trades";
      this.tsMenuItemTradeManagement.Click += new EventHandler(this.mainMenu_Click);
      this.tsStripMenuItemMasterContacts.Image = (Image) componentResourceManager.GetObject("tsStripMenuItemMasterContacts.Image");
      this.tsStripMenuItemMasterContacts.Name = "tsStripMenuItemMasterContacts";
      this.tsStripMenuItemMasterContacts.ShortcutKeys = Keys.A | Keys.Shift | Keys.Control;
      this.tsStripMenuItemMasterContacts.Size = new Size(341, 30);
      this.tsStripMenuItemMasterContacts.Text = "M&aster Contracts";
      this.tsStripMenuItemMasterContacts.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemContacts.DropDownItems.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.tsMenuItemBorrowerContacts,
        (ToolStripItem) this.tsMenuItemBusinessContacts,
        (ToolStripItem) this.tsMenuItemCalendar,
        (ToolStripItem) this.tsMenuItemTasks,
        (ToolStripItem) this.tsMenuItemCampaigns
      });
      this.tsMenuItemContacts.Name = "tsMenuItemContacts";
      this.tsMenuItemContacts.ShortcutKeys = Keys.C | Keys.Shift | Keys.Control;
      this.tsMenuItemContacts.Size = new Size(310, 30);
      this.tsMenuItemContacts.Text = "&Contacts";
      this.tsMenuItemBorrowerContacts.Image = (Image) componentResourceManager.GetObject("tsMenuItemBorrowerContacts.Image");
      this.tsMenuItemBorrowerContacts.Name = "tsMenuItemBorrowerContacts";
      this.tsMenuItemBorrowerContacts.ShortcutKeys = Keys.B | Keys.Shift | Keys.Control;
      this.tsMenuItemBorrowerContacts.Size = new Size(351, 30);
      this.tsMenuItemBorrowerContacts.Text = "&Borrower Contacts";
      this.tsMenuItemBorrowerContacts.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemBusinessContacts.Image = (Image) componentResourceManager.GetObject("tsMenuItemBusinessContacts.Image");
      this.tsMenuItemBusinessContacts.Name = "tsMenuItemBusinessContacts";
      this.tsMenuItemBusinessContacts.ShortcutKeys = Keys.U | Keys.Shift | Keys.Control;
      this.tsMenuItemBusinessContacts.Size = new Size(351, 30);
      this.tsMenuItemBusinessContacts.Text = "B&usiness Contacts";
      this.tsMenuItemBusinessContacts.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemCalendar.Image = (Image) componentResourceManager.GetObject("tsMenuItemCalendar.Image");
      this.tsMenuItemCalendar.Name = "tsMenuItemCalendar";
      this.tsMenuItemCalendar.ShortcutKeys = Keys.C | Keys.Shift | Keys.Control;
      this.tsMenuItemCalendar.Size = new Size(351, 30);
      this.tsMenuItemCalendar.Text = "&Calendar";
      this.tsMenuItemCalendar.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemTasks.Image = (Image) componentResourceManager.GetObject("tsMenuItemTasks.Image");
      this.tsMenuItemTasks.Name = "tsMenuItemTasks";
      this.tsMenuItemTasks.ShortcutKeys = Keys.T | Keys.Shift | Keys.Control;
      this.tsMenuItemTasks.Size = new Size(351, 30);
      this.tsMenuItemTasks.Text = "&Tasks";
      this.tsMenuItemTasks.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemCampaigns.Image = (Image) componentResourceManager.GetObject("tsMenuItemCampaigns.Image");
      this.tsMenuItemCampaigns.Name = "tsMenuItemCampaigns";
      this.tsMenuItemCampaigns.ShortcutKeys = Keys.M | Keys.Shift | Keys.Control;
      this.tsMenuItemCampaigns.Size = new Size(351, 30);
      this.tsMenuItemCampaigns.Text = "Ca&mpaigns";
      this.tsMenuItemCampaigns.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemDashboard.Name = "tsMenuItemDashboard";
      this.tsMenuItemDashboard.ShortcutKeys = Keys.D | Keys.Shift | Keys.Control;
      this.tsMenuItemDashboard.Size = new Size(310, 30);
      this.tsMenuItemDashboard.Text = "&Dashboard";
      this.tsMenuItemDashboard.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemReports.Name = "tsMenuItemReports";
      this.tsMenuItemReports.ShortcutKeys = Keys.R | Keys.Shift | Keys.Control;
      this.tsMenuItemReports.Size = new Size(310, 30);
      this.tsMenuItemReports.Text = "&Reports";
      this.tsMenuItemReports.Click += new EventHandler(this.mainMenu_Click);
      this.tsMenuItemHelp.DropDownItems.AddRange(new ToolStripItem[18]
      {
        (ToolStripItem) this.tsMenuItemEncompassHelp,
        (ToolStripItem) this.tsMenuItemHelpPad,
        (ToolStripItem) this.tsMenuItemTutorials,
        (ToolStripItem) this.tsMenuItemEncompassGlossary,
        (ToolStripItem) this.tsMenuItemDocLibrary,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.tsMenuItemTrainingSchedule,
        (ToolStripItem) this.tsMenuItemTechSupportOptions,
        (ToolStripItem) this.toolStripSeparator18,
        (ToolStripItem) this.tsHelpDiagnostics,
        (ToolStripItem) this.tsMenuItemJITLogger,
        (ToolStripItem) this.tsHelpDDMDiagnostics,
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.tsMenuItemFeedback,
        (ToolStripItem) this.MenuItemEllieMae,
        (ToolStripItem) this.toolStripSeparator4,
        (ToolStripItem) this.tsMenuItemReleaseNotes,
        (ToolStripItem) this.tsMenuItemAbout
      });
      this.tsMenuItemHelp.Name = "tsMenuItemHelp";
      this.tsMenuItemHelp.Size = new Size(61, 29);
      this.tsMenuItemHelp.Text = "&Help";
      this.tsMenuItemHelp.DropDownOpening += new EventHandler(this.tsMenuItemHelp_DropDownOpening);
      this.tsMenuItemEncompassHelp.Image = (Image) Resources.help;
      this.tsMenuItemEncompassHelp.Name = "tsMenuItemEncompassHelp";
      this.tsMenuItemEncompassHelp.ShortcutKeys = Keys.F1;
      this.tsMenuItemEncompassHelp.Size = new Size(409, 30);
      this.tsMenuItemEncompassHelp.Text = "Encompass &Help...";
      this.tsMenuItemEncompassHelp.Click += new EventHandler(this.tsMenuItemEncompassHelp_Click);
      this.tsMenuItemHelpPad.Name = "tsMenuItemHelpPad";
      this.tsMenuItemHelpPad.Size = new Size(409, 30);
      this.tsMenuItemHelpPad.Text = "Help&Pad...";
      this.tsMenuItemHelpPad.Click += new EventHandler(this.tsMenuItemHelpPad_Click);
      this.tsMenuItemTutorials.Image = (Image) componentResourceManager.GetObject("tsMenuItemTutorials.Image");
      this.tsMenuItemTutorials.Name = "tsMenuItemTutorials";
      this.tsMenuItemTutorials.Size = new Size(409, 30);
      this.tsMenuItemTutorials.Text = "&Tutorials...";
      this.tsMenuItemTutorials.Click += new EventHandler(this.tsMenuItemTutorials_Click);
      this.tsMenuItemEncompassGlossary.Image = (Image) Resources.glossary_help_system;
      this.tsMenuItemEncompassGlossary.Name = "tsMenuItemEncompassGlossary";
      this.tsMenuItemEncompassGlossary.Size = new Size(409, 30);
      this.tsMenuItemEncompassGlossary.Text = "&Glossary";
      this.tsMenuItemEncompassGlossary.Click += new EventHandler(this.tsMenuItemEncompassGlossary_Click);
      this.tsMenuItemDocLibrary.Name = "tsMenuItemDocLibrary";
      this.tsMenuItemDocLibrary.Size = new Size(409, 30);
      this.tsMenuItemDocLibrary.Text = "G&uides && Documents";
      this.tsMenuItemDocLibrary.Click += new EventHandler(this.tsMenuItemDocLibrary_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(406, 6);
      this.tsMenuItemTrainingSchedule.Name = "tsMenuItemTrainingSchedule";
      this.tsMenuItemTrainingSchedule.Size = new Size(409, 30);
      this.tsMenuItemTrainingSchedule.Text = "Encompass T&raining...";
      this.tsMenuItemTrainingSchedule.Click += new EventHandler(this.tsMenuItemTrainingSchedule_Click);
      this.tsMenuItemTechSupportOptions.Name = "tsMenuItemTechSupportOptions";
      this.tsMenuItemTechSupportOptions.Size = new Size(409, 30);
      this.tsMenuItemTechSupportOptions.Text = "Technical Support &Options...";
      this.tsMenuItemTechSupportOptions.Click += new EventHandler(this.tsMenuItemTechSupportOptions_Click);
      this.toolStripSeparator18.Name = "toolStripSeparator18";
      this.toolStripSeparator18.Size = new Size(406, 6);
      this.tsHelpDiagnostics.Name = "tsHelpDiagnostics";
      this.tsHelpDiagnostics.Size = new Size(409, 30);
      this.tsHelpDiagnostics.Text = "Diagnostic Mode - Entire Session";
      this.tsHelpDiagnostics.Click += new EventHandler(this.tsHelpDiagnostics_Click);
      this.tsMenuItemJITLogger.Name = "tsMenuItemJITLogger";
      this.tsMenuItemJITLogger.Size = new Size(409, 30);
      this.tsMenuItemJITLogger.Text = "Diagnostic Mode - Just In Time";
      this.tsMenuItemJITLogger.Click += new EventHandler(this.tsMenuItemJITLogger_Click);
      this.tsHelpDDMDiagnostics.Name = "tsHelpDDMDiagnostics";
      this.tsHelpDDMDiagnostics.Size = new Size(409, 30);
      this.tsHelpDDMDiagnostics.Text = "DDM Diagnostics";
      this.tsHelpDDMDiagnostics.Click += new EventHandler(this.tsHelpDDMDiagnostics_Click);
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new Size(406, 6);
      this.tsMenuItemFeedback.Name = "tsMenuItemFeedback";
      this.tsMenuItemFeedback.Size = new Size(409, 30);
      this.tsMenuItemFeedback.Text = "&Feedback...";
      this.tsMenuItemFeedback.Click += new EventHandler(this.tsMenuItemFeedback_Click);
      this.MenuItemEllieMae.Name = "MenuItemEllieMae";
      this.MenuItemEllieMae.Size = new Size(409, 30);
      this.MenuItemEllieMae.Text = "&ICE Mortgage Technology Home Page...";
      this.MenuItemEllieMae.Click += new EventHandler(this.MenuItemEllieMae_Click);
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new Size(406, 6);
      this.tsMenuItemReleaseNotes.Name = "tsMenuItemReleaseNotes";
      this.tsMenuItemReleaseNotes.Size = new Size(409, 30);
      this.tsMenuItemReleaseNotes.Text = "Release &Notes";
      this.tsMenuItemReleaseNotes.Click += new EventHandler(this.tsMenuItemReleaseNotes_Click);
      this.tsMenuItemAbout.Image = (Image) Resources.encompass_16;
      this.tsMenuItemAbout.Name = "tsMenuItemAbout";
      this.tsMenuItemAbout.Size = new Size(409, 30);
      this.tsMenuItemAbout.Text = "&About Encompass";
      this.tsMenuItemAbout.Click += new EventHandler(this.tsMenuItemAbout_Click);
      this.tsMenuItemMbsPools.Image = (Image) componentResourceManager.GetObject("tsMenuItemMbsPools.Image");
      this.tsMenuItemMbsPools.Name = "tsMenuItemMbsPools";
      this.tsMenuItemMbsPools.ShortcutKeys = Keys.M | Keys.Shift | Keys.Control;
      this.tsMenuItemMbsPools.Size = new Size(238, 22);
      this.tsMenuItemMbsPools.Text = "MBS Pools";
      this.tsMenuItemMbsPools.Click += new EventHandler(this.mainMenu_Click);
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new Size(219, 6);
      this.borrowerMenu.ImageScalingSize = new Size(20, 20);
      this.borrowerMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.borrowerContactItems
      });
      this.borrowerMenu.Location = new Point(0, 0);
      this.borrowerMenu.Name = "borrowerMenu";
      this.borrowerMenu.Size = new Size(200, 24);
      this.borrowerMenu.TabIndex = 0;
      this.borrowerContactItems.DropDownItems.AddRange(new ToolStripItem[30]
      {
        (ToolStripItem) this.tsBCNewContact,
        (ToolStripItem) this.tsBCDuplicateContact,
        (ToolStripItem) this.tsBCDeleteContact,
        (ToolStripItem) this.tsBCSeparator1,
        (ToolStripItem) this.tsBCExportExcel,
        (ToolStripItem) this.tsBCPrintDetails,
        (ToolStripItem) this.tsBCSeparator2,
        (ToolStripItem) this.tsBCMailMerge,
        (ToolStripItem) this.tsBCSynchronize,
        (ToolStripItem) this.tsBCSeparator3,
        (ToolStripItem) this.tsBCAddToGroup,
        (ToolStripItem) this.tsBCRemoveFromGroup,
        (ToolStripItem) this.tsBCEditGroups,
        (ToolStripItem) this.tsBCSeparator4,
        (ToolStripItem) this.tsBCOriginateLoan,
        (ToolStripItem) this.tsBCOrderCredit,
        (ToolStripItem) this.tsBCProductPricing,
        (ToolStripItem) this.tsBCSeparator5,
        (ToolStripItem) this.tsBCBuyLeads,
        (ToolStripItem) this.tsBCImportLeads,
        (ToolStripItem) this.tsBCImportBorrower,
        (ToolStripItem) this.tsBCExportBorrower,
        (ToolStripItem) this.tsBCReassign,
        (ToolStripItem) this.tsBCSeparator6,
        (ToolStripItem) this.tsBCCustomizeColumns,
        (ToolStripItem) this.tsBCSaveView,
        (ToolStripItem) this.tsBCResetView,
        (ToolStripItem) this.tsBCManageViews,
        (ToolStripItem) this.tsBCSeparator7,
        (ToolStripItem) this.tsBCHomePoints
      });
      this.borrowerContactItems.Name = "borrowerContactItems";
      this.borrowerContactItems.Size = new Size(170, 20);
      this.borrowerContactItems.Text = "Borro&wer Contacts";
      this.borrowerContactItems.DropDownOpening += new EventHandler(this.tsBC_DropDownOpening);
      this.tsBCNewContact.Image = (Image) Resources.new_file;
      this.tsBCNewContact.Name = "tsBCNewContact";
      this.tsBCNewContact.ShortcutKeys = Keys.N | Keys.Control;
      this.tsBCNewContact.Size = new Size(385, 30);
      this.tsBCNewContact.Text = "&New Contact...";
      this.tsBCNewContact.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCDuplicateContact.Image = (Image) Resources.duplicate;
      this.tsBCDuplicateContact.Name = "tsBCDuplicateContact";
      this.tsBCDuplicateContact.Size = new Size(385, 30);
      this.tsBCDuplicateContact.Text = "D&uplicate Contact";
      this.tsBCDuplicateContact.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCDeleteContact.Image = (Image) Resources.delete;
      this.tsBCDeleteContact.Name = "tsBCDeleteContact";
      this.tsBCDeleteContact.ShortcutKeys = Keys.D | Keys.Alt;
      this.tsBCDeleteContact.Size = new Size(385, 30);
      this.tsBCDeleteContact.Text = "&Delete Contact";
      this.tsBCDeleteContact.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCSeparator1.Name = "tsBCSeparator1";
      this.tsBCSeparator1.Size = new Size(382, 6);
      this.tsBCExportExcel.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsExportBorContacts,
        (ToolStripItem) this.tsExportAllBorContacts
      });
      this.tsBCExportExcel.Image = (Image) Resources.excel;
      this.tsBCExportExcel.Name = "tsBCExportExcel";
      this.tsBCExportExcel.Size = new Size(385, 30);
      this.tsBCExportExcel.Text = "E&xport to Excel";
      this.tsExportBorContacts.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.tsExportBorContacts.Name = "tsExportBorContacts";
      this.tsExportBorContacts.Size = new Size(304, 30);
      this.tsExportBorContacts.Text = "Selected Contacts Only...";
      this.tsExportBorContacts.Click += new EventHandler(this.contactMenu_Click);
      this.tsExportAllBorContacts.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.tsExportAllBorContacts.Name = "tsExportAllBorContacts";
      this.tsExportAllBorContacts.Size = new Size(304, 30);
      this.tsExportAllBorContacts.Text = "All Contacts on All Pages...";
      this.tsExportAllBorContacts.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCPrintDetails.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsPrintBorContacts,
        (ToolStripItem) this.tsPrintAllBorContacts
      });
      this.tsBCPrintDetails.Image = (Image) Resources.print;
      this.tsBCPrintDetails.Name = "tsBCPrintDetails";
      this.tsBCPrintDetails.ShortcutKeys = Keys.P | Keys.Control;
      this.tsBCPrintDetails.Size = new Size(385, 30);
      this.tsBCPrintDetails.Text = "&Print Details";
      this.tsPrintBorContacts.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.tsPrintBorContacts.Name = "tsPrintBorContacts";
      this.tsPrintBorContacts.Size = new Size(304, 30);
      this.tsPrintBorContacts.Text = "Selected Contacts Only...";
      this.tsPrintBorContacts.Click += new EventHandler(this.contactMenu_Click);
      this.tsPrintAllBorContacts.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.tsPrintAllBorContacts.Name = "tsPrintAllBorContacts";
      this.tsPrintAllBorContacts.Size = new Size(304, 30);
      this.tsPrintAllBorContacts.Text = "All Contacts on All Pages...";
      this.tsPrintAllBorContacts.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCSeparator2.Name = "tsBCSeparator2";
      this.tsBCSeparator2.Size = new Size(382, 6);
      this.tsBCMailMerge.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsMailMergeBorContacts,
        (ToolStripItem) this.tsMailMergeAllBorContacts
      });
      this.tsBCMailMerge.Name = "tsBCMailMerge";
      this.tsBCMailMerge.Size = new Size(385, 30);
      this.tsBCMailMerge.Text = "&Mail/Email Merge";
      this.tsMailMergeBorContacts.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.tsMailMergeBorContacts.Name = "tsMailMergeBorContacts";
      this.tsMailMergeBorContacts.Size = new Size(304, 30);
      this.tsMailMergeBorContacts.Text = "Selected Contacts Only...";
      this.tsMailMergeBorContacts.Click += new EventHandler(this.contactMenu_Click);
      this.tsMailMergeAllBorContacts.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.tsMailMergeAllBorContacts.Name = "tsMailMergeAllBorContacts";
      this.tsMailMergeAllBorContacts.Size = new Size(304, 30);
      this.tsMailMergeAllBorContacts.Text = "All Contacts on All Pages...";
      this.tsMailMergeAllBorContacts.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCSynchronize.Name = "tsBCSynchronize";
      this.tsBCSynchronize.Size = new Size(385, 30);
      this.tsBCSynchronize.Text = "&Synchronize Contacts...";
      this.tsBCSynchronize.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCSeparator3.Name = "tsBCSeparator3";
      this.tsBCSeparator3.Size = new Size(382, 6);
      this.tsBCAddToGroup.Name = "tsBCAddToGroup";
      this.tsBCAddToGroup.Size = new Size(385, 30);
      this.tsBCAddToGroup.Text = "&Add to Group";
      this.tsBCAddToGroup.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCRemoveFromGroup.Name = "tsBCRemoveFromGroup";
      this.tsBCRemoveFromGroup.Size = new Size(385, 30);
      this.tsBCRemoveFromGroup.Text = "Remo&ve From Group";
      this.tsBCRemoveFromGroup.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCEditGroups.Name = "tsBCEditGroups";
      this.tsBCEditGroups.Size = new Size(385, 30);
      this.tsBCEditGroups.Text = "Edit &Groups...";
      this.tsBCEditGroups.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCSeparator4.Name = "tsBCSeparator4";
      this.tsBCSeparator4.Size = new Size(382, 6);
      this.tsBCOriginateLoan.Name = "tsBCOriginateLoan";
      this.tsBCOriginateLoan.Size = new Size(385, 30);
      this.tsBCOriginateLoan.Text = "Originate &Loan";
      this.tsBCOriginateLoan.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCOrderCredit.Name = "tsBCOrderCredit";
      this.tsBCOrderCredit.Size = new Size(385, 30);
      this.tsBCOrderCredit.Text = "Order &Credit...";
      this.tsBCOrderCredit.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCProductPricing.Name = "tsBCProductPricing";
      this.tsBCProductPricing.Size = new Size(385, 30);
      this.tsBCProductPricing.Text = "P&roduct and Pricing";
      this.tsBCProductPricing.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCSeparator5.Name = "tsBCSeparator5";
      this.tsBCSeparator5.Size = new Size(382, 6);
      this.tsBCBuyLeads.Name = "tsBCBuyLeads";
      this.tsBCBuyLeads.Size = new Size(385, 30);
      this.tsBCBuyLeads.Text = "Buy &Leads from the Lead Center...";
      this.tsBCBuyLeads.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCImportLeads.Name = "tsBCImportLeads";
      this.tsBCImportLeads.Size = new Size(385, 30);
      this.tsBCImportLeads.Text = "&Import Leads from the Lead Center...";
      this.tsBCImportLeads.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCImportBorrower.Name = "tsBCImportBorrower";
      this.tsBCImportBorrower.Size = new Size(385, 30);
      this.tsBCImportBorrower.Text = "Import Borro&wer Contacts...";
      this.tsBCImportBorrower.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCExportBorrower.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsBCExportBorrowerSelectedOnly,
        (ToolStripItem) this.tsBCExportBorrowerAll
      });
      this.tsBCExportBorrower.Name = "tsBCExportBorrower";
      this.tsBCExportBorrower.Size = new Size(385, 30);
      this.tsBCExportBorrower.Text = "Export &Borrower Contacts";
      this.tsBCExportBorrowerSelectedOnly.Name = "tsBCExportBorrowerSelectedOnly";
      this.tsBCExportBorrowerSelectedOnly.Size = new Size(304, 30);
      this.tsBCExportBorrowerSelectedOnly.Text = "&Selected Contacts Only...";
      this.tsBCExportBorrowerSelectedOnly.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCExportBorrowerAll.Name = "tsBCExportBorrowerAll";
      this.tsBCExportBorrowerAll.Size = new Size(304, 30);
      this.tsBCExportBorrowerAll.Text = "&All Contacts on All Pages...";
      this.tsBCExportBorrowerAll.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCReassign.Name = "tsBCReassign";
      this.tsBCReassign.Size = new Size(385, 30);
      this.tsBCReassign.Text = "&Reassign...";
      this.tsBCReassign.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCSeparator6.Name = "tsBCSeparator6";
      this.tsBCSeparator6.Size = new Size(382, 6);
      this.tsBCCustomizeColumns.Name = "tsBCCustomizeColumns";
      this.tsBCCustomizeColumns.Size = new Size(385, 30);
      this.tsBCCustomizeColumns.Text = "&Customize Columns...";
      this.tsBCCustomizeColumns.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCSaveView.Name = "tsBCSaveView";
      this.tsBCSaveView.Size = new Size(385, 30);
      this.tsBCSaveView.Text = "Sav&e View";
      this.tsBCSaveView.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCResetView.Name = "tsBCResetView";
      this.tsBCResetView.Size = new Size(385, 30);
      this.tsBCResetView.Text = "&Reset View";
      this.tsBCResetView.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCManageViews.Name = "tsBCManageViews";
      this.tsBCManageViews.Size = new Size(385, 30);
      this.tsBCManageViews.Text = "Manage Borrower Contacts Vie&ws";
      this.tsBCManageViews.Click += new EventHandler(this.contactMenu_Click);
      this.tsBCSeparator7.Name = "tsBCSeparator7";
      this.tsBCSeparator7.Size = new Size(382, 6);
      this.tsBCHomePoints.Name = "tsBCHomePoints";
      this.tsBCHomePoints.Size = new Size(385, 30);
      this.tsBCHomePoints.Text = "Customer Loyalt&y";
      this.tsBCHomePoints.Click += new EventHandler(this.contactMenu_Click);
      this.bizPartnerMenu.ImageScalingSize = new Size(20, 20);
      this.bizPartnerMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.bizContactItems
      });
      this.bizPartnerMenu.Location = new Point(0, 0);
      this.bizPartnerMenu.Name = "bizPartnerMenu";
      this.bizPartnerMenu.Size = new Size(200, 24);
      this.bizPartnerMenu.TabIndex = 0;
      this.bizContactItems.DropDownItems.AddRange(new ToolStripItem[21]
      {
        (ToolStripItem) this.tsBizCNewContact,
        (ToolStripItem) this.tsBizCDuplicateContact,
        (ToolStripItem) this.tsBizCDeleteContact,
        (ToolStripItem) this.toolStripSeparator5,
        (ToolStripItem) this.tsBizCExportToExcel,
        (ToolStripItem) this.tsBizCPrintDetail,
        (ToolStripItem) this.toolStripSeparator6,
        (ToolStripItem) this.tsBizCMailMerge,
        (ToolStripItem) this.tsBizCSynchronize,
        (ToolStripItem) this.toolStripSeparator7,
        (ToolStripItem) this.tsBizCAddToGroup,
        (ToolStripItem) this.tsBizCRemoveFromGroup,
        (ToolStripItem) this.tsBizCEditGroup,
        (ToolStripItem) this.toolStripSeparator8,
        (ToolStripItem) this.tsBizCImportBizContact,
        (ToolStripItem) this.tsBizCExportBizContact,
        (ToolStripItem) this.toolStripSeparator9,
        (ToolStripItem) this.tsBizCCustomizeColumn,
        (ToolStripItem) this.tsBizCSaveView,
        (ToolStripItem) this.tsBizCResetView,
        (ToolStripItem) this.tsBizCManageView
      });
      this.bizContactItems.Name = "bizContactItems";
      this.bizContactItems.Size = new Size(165, 20);
      this.bizContactItems.Text = "Bus&iness Contacts";
      this.bizContactItems.Visible = false;
      this.bizContactItems.DropDownOpening += new EventHandler(this.bizContactItems_DropDownOpening);
      this.tsBizCNewContact.Image = (Image) Resources.new_file;
      this.tsBizCNewContact.Name = "tsBizCNewContact";
      this.tsBizCNewContact.ShortcutKeys = Keys.N | Keys.Control;
      this.tsBizCNewContact.Size = new Size(356, 30);
      this.tsBizCNewContact.Text = "&New Contact...";
      this.tsBizCNewContact.Click += new EventHandler(this.contactMenu_Click);
      this.tsBizCDuplicateContact.Image = (Image) Resources.duplicate;
      this.tsBizCDuplicateContact.Name = "tsBizCDuplicateContact";
      this.tsBizCDuplicateContact.Size = new Size(356, 30);
      this.tsBizCDuplicateContact.Text = "D&uplicate Contact";
      this.tsBizCDuplicateContact.Click += new EventHandler(this.contactMenu_Click);
      this.tsBizCDeleteContact.Image = (Image) Resources.delete;
      this.tsBizCDeleteContact.Name = "tsBizCDeleteContact";
      this.tsBizCDeleteContact.ShortcutKeys = Keys.D | Keys.Alt;
      this.tsBizCDeleteContact.Size = new Size(356, 30);
      this.tsBizCDeleteContact.Text = "&Delete Contact";
      this.tsBizCDeleteContact.Click += new EventHandler(this.contactMenu_Click);
      this.toolStripSeparator5.Name = "toolStripSeparator5";
      this.toolStripSeparator5.Size = new Size(353, 6);
      this.tsBizCExportToExcel.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsExportBizContacts,
        (ToolStripItem) this.tsExportAllBizContacts
      });
      this.tsBizCExportToExcel.Image = (Image) Resources.excel;
      this.tsBizCExportToExcel.Name = "tsBizCExportToExcel";
      this.tsBizCExportToExcel.Size = new Size(356, 30);
      this.tsBizCExportToExcel.Text = "E&xport to Excel";
      this.tsExportBizContacts.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.tsExportBizContacts.Name = "tsExportBizContacts";
      this.tsExportBizContacts.Size = new Size(304, 30);
      this.tsExportBizContacts.Text = "Selected Contacts Only...";
      this.tsExportBizContacts.Click += new EventHandler(this.contactMenu_Click);
      this.tsExportAllBizContacts.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.tsExportAllBizContacts.Name = "tsExportAllBizContacts";
      this.tsExportAllBizContacts.Size = new Size(304, 30);
      this.tsExportAllBizContacts.Text = "All Contacts on All Pages...";
      this.tsExportAllBizContacts.Click += new EventHandler(this.contactMenu_Click);
      this.tsBizCPrintDetail.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsPrintBizContacts,
        (ToolStripItem) this.tsPrintAllBizContacts
      });
      this.tsBizCPrintDetail.Image = (Image) Resources.print;
      this.tsBizCPrintDetail.Name = "tsBizCPrintDetail";
      this.tsBizCPrintDetail.ShortcutKeys = Keys.P | Keys.Control;
      this.tsBizCPrintDetail.Size = new Size(356, 30);
      this.tsBizCPrintDetail.Text = "&Print Details";
      this.tsPrintBizContacts.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.tsPrintBizContacts.Name = "tsPrintBizContacts";
      this.tsPrintBizContacts.Size = new Size(304, 30);
      this.tsPrintBizContacts.Text = "Selected Contacts Only...";
      this.tsPrintBizContacts.Click += new EventHandler(this.contactMenu_Click);
      this.tsPrintAllBizContacts.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.tsPrintAllBizContacts.Name = "tsPrintAllBizContacts";
      this.tsPrintAllBizContacts.Size = new Size(304, 30);
      this.tsPrintAllBizContacts.Text = "All Contacts on All Pages...";
      this.tsPrintAllBizContacts.Click += new EventHandler(this.contactMenu_Click);
      this.toolStripSeparator6.Name = "toolStripSeparator6";
      this.toolStripSeparator6.Size = new Size(353, 6);
      this.tsBizCMailMerge.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsMailMergeBizContacts,
        (ToolStripItem) this.tsMailMergeAllBizContacts
      });
      this.tsBizCMailMerge.Name = "tsBizCMailMerge";
      this.tsBizCMailMerge.Size = new Size(356, 30);
      this.tsBizCMailMerge.Text = "&Mail/Email Merge";
      this.tsMailMergeBizContacts.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.tsMailMergeBizContacts.Name = "tsMailMergeBizContacts";
      this.tsMailMergeBizContacts.Size = new Size(304, 30);
      this.tsMailMergeBizContacts.Text = "Selected Contacts Only...";
      this.tsMailMergeBizContacts.Click += new EventHandler(this.contactMenu_Click);
      this.tsMailMergeAllBizContacts.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this.tsMailMergeAllBizContacts.Name = "tsMailMergeAllBizContacts";
      this.tsMailMergeAllBizContacts.Size = new Size(304, 30);
      this.tsMailMergeAllBizContacts.Text = "All Contacts on All Pages...";
      this.tsMailMergeAllBizContacts.Click += new EventHandler(this.contactMenu_Click);
      this.tsBizCSynchronize.Name = "tsBizCSynchronize";
      this.tsBizCSynchronize.Size = new Size(356, 30);
      this.tsBizCSynchronize.Text = "&Synchronize Contacts...";
      this.tsBizCSynchronize.Click += new EventHandler(this.contactMenu_Click);
      this.toolStripSeparator7.Name = "toolStripSeparator7";
      this.toolStripSeparator7.Size = new Size(353, 6);
      this.tsBizCAddToGroup.Name = "tsBizCAddToGroup";
      this.tsBizCAddToGroup.Size = new Size(356, 30);
      this.tsBizCAddToGroup.Text = "&Add to Group";
      this.tsBizCAddToGroup.Click += new EventHandler(this.contactMenu_Click);
      this.tsBizCRemoveFromGroup.Name = "tsBizCRemoveFromGroup";
      this.tsBizCRemoveFromGroup.Size = new Size(356, 30);
      this.tsBizCRemoveFromGroup.Text = "Remo&ve From Group";
      this.tsBizCRemoveFromGroup.Click += new EventHandler(this.contactMenu_Click);
      this.tsBizCEditGroup.Name = "tsBizCEditGroup";
      this.tsBizCEditGroup.Size = new Size(356, 30);
      this.tsBizCEditGroup.Text = "Edit &Groups...";
      this.tsBizCEditGroup.Click += new EventHandler(this.contactMenu_Click);
      this.toolStripSeparator8.Name = "toolStripSeparator8";
      this.toolStripSeparator8.Size = new Size(353, 6);
      this.tsBizCImportBizContact.Name = "tsBizCImportBizContact";
      this.tsBizCImportBizContact.Size = new Size(356, 30);
      this.tsBizCImportBizContact.Text = "Import Bus&iness Contacts...";
      this.tsBizCImportBizContact.Click += new EventHandler(this.contactMenu_Click);
      this.tsBizCExportBizContact.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsBizCExportBizContactSelectedOnly,
        (ToolStripItem) this.tsBizCExportBizContactAll
      });
      this.tsBizCExportBizContact.Name = "tsBizCExportBizContact";
      this.tsBizCExportBizContact.Size = new Size(356, 30);
      this.tsBizCExportBizContact.Text = "Export B&usiness Contacts";
      this.tsBizCExportBizContactSelectedOnly.Name = "tsBizCExportBizContactSelectedOnly";
      this.tsBizCExportBizContactSelectedOnly.Size = new Size(304, 30);
      this.tsBizCExportBizContactSelectedOnly.Text = "&Selected Contacts Only...";
      this.tsBizCExportBizContactSelectedOnly.Click += new EventHandler(this.contactMenu_Click);
      this.tsBizCExportBizContactAll.Name = "tsBizCExportBizContactAll";
      this.tsBizCExportBizContactAll.Size = new Size(304, 30);
      this.tsBizCExportBizContactAll.Text = "&All Contacts on All Pages...";
      this.tsBizCExportBizContactAll.Click += new EventHandler(this.contactMenu_Click);
      this.toolStripSeparator9.Name = "toolStripSeparator9";
      this.toolStripSeparator9.Size = new Size(353, 6);
      this.tsBizCCustomizeColumn.Name = "tsBizCCustomizeColumn";
      this.tsBizCCustomizeColumn.Size = new Size(356, 30);
      this.tsBizCCustomizeColumn.Text = "&Customize Columns...";
      this.tsBizCCustomizeColumn.Click += new EventHandler(this.contactMenu_Click);
      this.tsBizCSaveView.Name = "tsBizCSaveView";
      this.tsBizCSaveView.Size = new Size(356, 30);
      this.tsBizCSaveView.Text = "Sav&e View";
      this.tsBizCSaveView.Click += new EventHandler(this.contactMenu_Click);
      this.tsBizCResetView.Name = "tsBizCResetView";
      this.tsBizCResetView.Size = new Size(356, 30);
      this.tsBizCResetView.Text = "&Reset View";
      this.tsBizCResetView.Click += new EventHandler(this.contactMenu_Click);
      this.tsBizCManageView.Name = "tsBizCManageView";
      this.tsBizCManageView.Size = new Size(356, 30);
      this.tsBizCManageView.Text = "Manage Business Contacts Vie&ws";
      this.tsBizCManageView.Click += new EventHandler(this.contactMenu_Click);
      this.calendarMenu.ImageScalingSize = new Size(20, 20);
      this.calendarMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.calendarItems
      });
      this.calendarMenu.Location = new Point(0, 0);
      this.calendarMenu.Name = "calendarMenu";
      this.calendarMenu.Size = new Size(200, 24);
      this.calendarMenu.TabIndex = 0;
      this.calendarItems.DropDownItems.AddRange(new ToolStripItem[9]
      {
        (ToolStripItem) this.tsCalNewApp,
        (ToolStripItem) this.tsCalEditApp,
        (ToolStripItem) this.tsCalDeleteApp,
        (ToolStripItem) this.toolStripSeparator10,
        (ToolStripItem) this.tsCalPrint,
        (ToolStripItem) this.toolStripSeparator11,
        (ToolStripItem) this.tsCalSynchronize,
        (ToolStripItem) this.tsSeparator_CalSynchronize,
        (ToolStripItem) this.tsCalView
      });
      this.calendarItems.Name = "calendarItems";
      this.calendarItems.Size = new Size(93, 20);
      this.calendarItems.Text = "&Calendar";
      this.calendarItems.Visible = false;
      this.calendarItems.DropDownOpening += new EventHandler(this.calendarItems_DropDownOpening);
      this.tsCalNewApp.Image = (Image) Resources.new_file;
      this.tsCalNewApp.Name = "tsCalNewApp";
      this.tsCalNewApp.ShortcutKeys = Keys.N | Keys.Control;
      this.tsCalNewApp.Size = new Size(318, 30);
      this.tsCalNewApp.Text = "&New Appointment...";
      this.tsCalNewApp.Click += new EventHandler(this.contactMenu_Click);
      this.tsCalEditApp.Image = (Image) Resources.edit;
      this.tsCalEditApp.Name = "tsCalEditApp";
      this.tsCalEditApp.ShortcutKeys = Keys.E | Keys.Control;
      this.tsCalEditApp.Size = new Size(318, 30);
      this.tsCalEditApp.Text = "&Edit Appointment...";
      this.tsCalEditApp.Click += new EventHandler(this.contactMenu_Click);
      this.tsCalDeleteApp.Image = (Image) Resources.delete;
      this.tsCalDeleteApp.Name = "tsCalDeleteApp";
      this.tsCalDeleteApp.ShortcutKeys = Keys.D | Keys.Alt;
      this.tsCalDeleteApp.Size = new Size(318, 30);
      this.tsCalDeleteApp.Text = "&Delete Appointment";
      this.tsCalDeleteApp.Click += new EventHandler(this.contactMenu_Click);
      this.toolStripSeparator10.Name = "toolStripSeparator10";
      this.toolStripSeparator10.Size = new Size(315, 6);
      this.tsCalPrint.Image = (Image) Resources.print;
      this.tsCalPrint.Name = "tsCalPrint";
      this.tsCalPrint.ShortcutKeys = Keys.P | Keys.Control;
      this.tsCalPrint.Size = new Size(318, 30);
      this.tsCalPrint.Text = "&Print Calendar...";
      this.tsCalPrint.Click += new EventHandler(this.contactMenu_Click);
      this.toolStripSeparator11.Name = "toolStripSeparator11";
      this.toolStripSeparator11.Size = new Size(315, 6);
      this.tsCalSynchronize.Name = "tsCalSynchronize";
      this.tsCalSynchronize.Size = new Size(318, 30);
      this.tsCalSynchronize.Text = "&Synchronize Calendar";
      this.tsCalSynchronize.Click += new EventHandler(this.contactMenu_Click);
      this.tsSeparator_CalSynchronize.Name = "tsSeparator_CalSynchronize";
      this.tsSeparator_CalSynchronize.Size = new Size(315, 6);
      this.tsCalView.DropDownItems.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.tsCalToday,
        (ToolStripItem) this.tsCalOneDay,
        (ToolStripItem) this.tsCalWorkDay,
        (ToolStripItem) this.tsCalWeek,
        (ToolStripItem) this.tsCalMonth
      });
      this.tsCalView.Name = "tsCalView";
      this.tsCalView.Size = new Size(318, 30);
      this.tsCalView.Text = "Calendar &View";
      this.tsCalView.Click += new EventHandler(this.contactMenu_Click);
      this.tsCalToday.Image = (Image) Resources.calendar;
      this.tsCalToday.Name = "tsCalToday";
      this.tsCalToday.Size = new Size(237, 30);
      this.tsCalToday.Text = "Jump To &Today";
      this.tsCalToday.Click += new EventHandler(this.contactMenu_Click);
      this.tsCalOneDay.Image = (Image) Resources.calendar_day;
      this.tsCalOneDay.Name = "tsCalOneDay";
      this.tsCalOneDay.Size = new Size(237, 30);
      this.tsCalOneDay.Text = "1 &Day";
      this.tsCalOneDay.Click += new EventHandler(this.contactMenu_Click);
      this.tsCalWorkDay.Image = (Image) Resources.calendar_work_week;
      this.tsCalWorkDay.Name = "tsCalWorkDay";
      this.tsCalWorkDay.Size = new Size(237, 30);
      this.tsCalWorkDay.Text = "5 Day Wo&rk Week";
      this.tsCalWorkDay.Click += new EventHandler(this.contactMenu_Click);
      this.tsCalWeek.Image = (Image) Resources.calendar_week;
      this.tsCalWeek.Name = "tsCalWeek";
      this.tsCalWeek.Size = new Size(237, 30);
      this.tsCalWeek.Text = "7 Day &Week";
      this.tsCalWeek.Click += new EventHandler(this.contactMenu_Click);
      this.tsCalMonth.Image = (Image) Resources.calendar_month;
      this.tsCalMonth.Name = "tsCalMonth";
      this.tsCalMonth.Size = new Size(237, 30);
      this.tsCalMonth.Text = "&Month";
      this.tsCalMonth.Click += new EventHandler(this.contactMenu_Click);
      this.campaignMenu.ImageScalingSize = new Size(20, 20);
      this.campaignMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.campaignItems
      });
      this.campaignMenu.Location = new Point(0, 0);
      this.campaignMenu.Name = "campaignMenu";
      this.campaignMenu.Size = new Size(200, 24);
      this.campaignMenu.TabIndex = 0;
      this.campaignItems.DropDownItems.AddRange(new ToolStripItem[8]
      {
        (ToolStripItem) this.tsCampaign_NewCampaign,
        (ToolStripItem) this.tsCampaign_OpenCampaign,
        (ToolStripItem) this.tsCampaign_DuplicateCampaign,
        (ToolStripItem) this.tsCampaign_DeleteCampaign,
        (ToolStripItem) this.toolStripSeparator13,
        (ToolStripItem) this.tsCampaign_StartCampaign,
        (ToolStripItem) this.tsCampaign_StopCampaign,
        (ToolStripItem) this.tsCampaign_ManageTemplate
      });
      this.campaignItems.Name = "campaignItems";
      this.campaignItems.Size = new Size(113, 20);
      this.campaignItems.Text = "Ca&mpaigns";
      this.campaignItems.Visible = false;
      this.campaignItems.DropDownOpening += new EventHandler(this.campaignItems_DropDownOpening);
      this.tsCampaign_NewCampaign.Image = (Image) Resources.new_file;
      this.tsCampaign_NewCampaign.Name = "tsCampaign_NewCampaign";
      this.tsCampaign_NewCampaign.ShortcutKeys = Keys.N | Keys.Control;
      this.tsCampaign_NewCampaign.Size = new Size(342, 30);
      this.tsCampaign_NewCampaign.Text = "&New Campaign...";
      this.tsCampaign_NewCampaign.Click += new EventHandler(this.contactMenu_Click);
      this.tsCampaign_OpenCampaign.Image = (Image) Resources.edit;
      this.tsCampaign_OpenCampaign.Name = "tsCampaign_OpenCampaign";
      this.tsCampaign_OpenCampaign.Size = new Size(342, 30);
      this.tsCampaign_OpenCampaign.Text = "&Open Campaign...";
      this.tsCampaign_OpenCampaign.Click += new EventHandler(this.contactMenu_Click);
      this.tsCampaign_DuplicateCampaign.Image = (Image) Resources.duplicate;
      this.tsCampaign_DuplicateCampaign.Name = "tsCampaign_DuplicateCampaign";
      this.tsCampaign_DuplicateCampaign.Size = new Size(342, 30);
      this.tsCampaign_DuplicateCampaign.Text = "D&uplicate Campaign...";
      this.tsCampaign_DuplicateCampaign.Click += new EventHandler(this.contactMenu_Click);
      this.tsCampaign_DeleteCampaign.Image = (Image) Resources.delete;
      this.tsCampaign_DeleteCampaign.Name = "tsCampaign_DeleteCampaign";
      this.tsCampaign_DeleteCampaign.ShortcutKeys = Keys.D | Keys.Alt;
      this.tsCampaign_DeleteCampaign.Size = new Size(342, 30);
      this.tsCampaign_DeleteCampaign.Text = "&Delete Campaign";
      this.tsCampaign_DeleteCampaign.Click += new EventHandler(this.contactMenu_Click);
      this.toolStripSeparator13.Name = "toolStripSeparator13";
      this.toolStripSeparator13.Size = new Size(339, 6);
      this.tsCampaign_StartCampaign.Name = "tsCampaign_StartCampaign";
      this.tsCampaign_StartCampaign.Size = new Size(342, 30);
      this.tsCampaign_StartCampaign.Text = "&Start Campaign";
      this.tsCampaign_StartCampaign.Click += new EventHandler(this.contactMenu_Click);
      this.tsCampaign_StopCampaign.Name = "tsCampaign_StopCampaign";
      this.tsCampaign_StopCampaign.Size = new Size(342, 30);
      this.tsCampaign_StopCampaign.Text = "Sto&p Campaign";
      this.tsCampaign_StopCampaign.Click += new EventHandler(this.contactMenu_Click);
      this.tsCampaign_ManageTemplate.Name = "tsCampaign_ManageTemplate";
      this.tsCampaign_ManageTemplate.Size = new Size(342, 30);
      this.tsCampaign_ManageTemplate.Text = "&Manage Campaign Templates...";
      this.tsCampaign_ManageTemplate.Click += new EventHandler(this.contactMenu_Click);
      this.taskMenu.ImageScalingSize = new Size(20, 20);
      this.taskMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.taskItems
      });
      this.taskMenu.Location = new Point(0, 0);
      this.taskMenu.Name = "taskMenu";
      this.taskMenu.Size = new Size(200, 24);
      this.taskMenu.TabIndex = 0;
      this.taskItems.DropDownItems.AddRange(new ToolStripItem[9]
      {
        (ToolStripItem) this.tsTask_NewTask,
        (ToolStripItem) this.tsTask_EditTask,
        (ToolStripItem) this.tsTask_Delete,
        (ToolStripItem) this.toolStripSeparator12,
        (ToolStripItem) this.tsTask_ExportExcel,
        (ToolStripItem) this.toolStripSeparator17,
        (ToolStripItem) this.tsTask_Synchronize,
        (ToolStripItem) this.tsSeparator_TaskSynchronize,
        (ToolStripItem) this.tsTask_Status
      });
      this.taskItems.Name = "taskItems";
      this.taskItems.Size = new Size(65, 20);
      this.taskItems.Text = "&Tasks";
      this.taskItems.Visible = false;
      this.taskItems.DropDownOpening += new EventHandler(this.taskItems_DropDownOpening);
      this.tsTask_NewTask.Image = (Image) Resources.new_file;
      this.tsTask_NewTask.Name = "tsTask_NewTask";
      this.tsTask_NewTask.ShortcutKeys = Keys.N | Keys.Control;
      this.tsTask_NewTask.Size = new Size(245, 30);
      this.tsTask_NewTask.Text = "&New Task...";
      this.tsTask_NewTask.Click += new EventHandler(this.contactMenu_Click);
      this.tsTask_EditTask.Image = (Image) Resources.edit;
      this.tsTask_EditTask.Name = "tsTask_EditTask";
      this.tsTask_EditTask.ShortcutKeys = Keys.E | Keys.Control;
      this.tsTask_EditTask.Size = new Size(245, 30);
      this.tsTask_EditTask.Text = "&Edit Task...";
      this.tsTask_EditTask.Click += new EventHandler(this.contactMenu_Click);
      this.tsTask_Delete.Image = (Image) Resources.delete;
      this.tsTask_Delete.Name = "tsTask_Delete";
      this.tsTask_Delete.ShortcutKeys = Keys.D | Keys.Alt;
      this.tsTask_Delete.Size = new Size(245, 30);
      this.tsTask_Delete.Text = "&Delete Task";
      this.tsTask_Delete.Click += new EventHandler(this.contactMenu_Click);
      this.toolStripSeparator12.Name = "toolStripSeparator12";
      this.toolStripSeparator12.Size = new Size(242, 6);
      this.tsTask_ExportExcel.Image = (Image) Resources.excel;
      this.tsTask_ExportExcel.Name = "tsTask_ExportExcel";
      this.tsTask_ExportExcel.Size = new Size(245, 30);
      this.tsTask_ExportExcel.Text = "E&xport to Excel";
      this.tsTask_ExportExcel.Click += new EventHandler(this.contactMenu_Click);
      this.toolStripSeparator17.Name = "toolStripSeparator17";
      this.toolStripSeparator17.Size = new Size(242, 6);
      this.tsTask_Synchronize.Name = "tsTask_Synchronize";
      this.tsTask_Synchronize.Size = new Size(245, 30);
      this.tsTask_Synchronize.Text = "&Synchronize Tasks";
      this.tsTask_Synchronize.Click += new EventHandler(this.contactMenu_Click);
      this.tsSeparator_TaskSynchronize.Name = "tsSeparator_TaskSynchronize";
      this.tsSeparator_TaskSynchronize.Size = new Size(242, 6);
      this.tsTask_Status.DropDownItems.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.tsTask_StatusNotStarted,
        (ToolStripItem) this.tsTask_StatusInProgress,
        (ToolStripItem) this.tsTask_StatusCompleted,
        (ToolStripItem) this.tsTask_StatusWait,
        (ToolStripItem) this.tsTask_StatusDeferred
      });
      this.tsTask_Status.Name = "tsTask_Status";
      this.tsTask_Status.Size = new Size(245, 30);
      this.tsTask_Status.Text = "St&atus";
      this.tsTask_StatusNotStarted.Name = "tsTask_StatusNotStarted";
      this.tsTask_StatusNotStarted.Size = new Size(298, 30);
      this.tsTask_StatusNotStarted.Text = "&Not Started";
      this.tsTask_StatusNotStarted.Click += new EventHandler(this.contactMenu_Click);
      this.tsTask_StatusInProgress.Name = "tsTask_StatusInProgress";
      this.tsTask_StatusInProgress.Size = new Size(298, 30);
      this.tsTask_StatusInProgress.Text = "&In Progress";
      this.tsTask_StatusInProgress.Click += new EventHandler(this.contactMenu_Click);
      this.tsTask_StatusCompleted.Name = "tsTask_StatusCompleted";
      this.tsTask_StatusCompleted.Size = new Size(298, 30);
      this.tsTask_StatusCompleted.Text = "&Completed";
      this.tsTask_StatusCompleted.Click += new EventHandler(this.contactMenu_Click);
      this.tsTask_StatusWait.Name = "tsTask_StatusWait";
      this.tsTask_StatusWait.Size = new Size(298, 30);
      this.tsTask_StatusWait.Text = "&Waiting on Someone Else";
      this.tsTask_StatusWait.Click += new EventHandler(this.contactMenu_Click);
      this.tsTask_StatusDeferred.Name = "tsTask_StatusDeferred";
      this.tsTask_StatusDeferred.Size = new Size(298, 30);
      this.tsTask_StatusDeferred.Text = "&Deferred";
      this.tsTask_StatusDeferred.Click += new EventHandler(this.contactMenu_Click);
      this.statusBar.ContextMenuStrip = this.ctxMenuStripSessionID;
      this.statusBar.ImageScalingSize = new Size(20, 20);
      this.statusBar.Items.AddRange(new ToolStripItem[10]
      {
        (ToolStripItem) this.sslHelp,
        (ToolStripItem) this.sslFieldID,
        (ToolStripItem) this.ssProgressBar,
        (ToolStripItem) this.ssbInsights,
        (ToolStripItem) this.sslSeparator2,
        (ToolStripItem) this.ssbTradeLoanUpdate,
        (ToolStripItem) this.ssbUploadProgress,
        (ToolStripItem) this.sslSeparator,
        (ToolStripItem) this.sslLastSaved,
        (ToolStripItem) this.sslDate
      });
      this.statusBar.Location = new Point(0, 518);
      this.statusBar.Name = "statusBar";
      this.statusBar.Size = new Size(792, 48);
      this.statusBar.TabIndex = 2;
      this.statusBar.Text = "gradientStatusStrip1";
      this.statusBar.DoubleClick += new EventHandler(this.statusBar_DoubleClick);
      this.ctxMenuStripSessionID.ImageScalingSize = new Size(20, 20);
      this.ctxMenuStripSessionID.Name = "ctxMenuStripSessionID";
      this.ctxMenuStripSessionID.Size = new Size(61, 4);
      this.ctxMenuStripSessionID.Opening += new CancelEventHandler(this.ctxMenuStripSessionID_Opening);
      this.sslHelp.AutoSize = false;
      this.sslHelp.BorderSides = ToolStripStatusLabelBorderSides.Right;
      this.sslHelp.Margin = new Padding(6, 3, 0, 2);
      this.sslHelp.Name = "sslHelp";
      this.sslHelp.Size = new Size(653, 43);
      this.sslHelp.Spring = true;
      this.sslHelp.Text = "Press F1 for Help";
      this.sslHelp.TextAlign = ContentAlignment.MiddleLeft;
      this.sslFieldID.BorderSides = ToolStripStatusLabelBorderSides.Right;
      this.sslFieldID.Name = "sslFieldID";
      this.sslFieldID.Size = new Size(4, 43);
      this.sslFieldID.TextAlign = ContentAlignment.MiddleLeft;
      this.ssProgressBar.Name = "ssProgressBar";
      this.ssProgressBar.Size = new Size(160, 35);
      this.ssProgressBar.Visible = false;
      this.ssbInsights.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.ssbInsights.Image = (Image) Resources.loanmetrics_img;
      this.ssbInsights.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.ssbInsights.Name = "ssbInsights";
      this.ssbInsights.Size = new Size(24, 24);
      this.ssbInsights.Text = "toolStripSplitButton1";
      this.ssbInsights.Visible = false;
      this.sslSeparator2.BorderSides = ToolStripStatusLabelBorderSides.Right;
      this.sslSeparator2.Name = "sslSeparator2";
      this.sslSeparator2.Size = new Size(4, 43);
      this.ssbTradeLoanUpdate.Image = (Image) Resources.trade_update_queue_image;
      this.ssbTradeLoanUpdate.Name = "ssbTradeLoanUpdate";
      this.ssbTradeLoanUpdate.Size = new Size(24, 24);
      this.ssbTradeLoanUpdate.TextAlign = ContentAlignment.MiddleLeft;
      this.ssbTradeLoanUpdate.ToolTipText = "Trade Loan Update";
      this.ssbTradeLoanUpdate.Visible = false;
      this.ssbTradeLoanUpdate.Click += new EventHandler(this.ssbTradeLoanUpdate_Click);
      this.ssbUploadProgress.Image = (Image) Resources.background_image;
      this.ssbUploadProgress.Name = "ssbUploadProgress";
      this.ssbUploadProgress.Size = new Size(24, 24);
      this.ssbUploadProgress.TextAlign = ContentAlignment.MiddleLeft;
      this.ssbUploadProgress.Visible = false;
      this.ssbUploadProgress.Click += new EventHandler(this.ssbUploadProgress_Click);
      this.sslSeparator.BorderSides = ToolStripStatusLabelBorderSides.Right;
      this.sslSeparator.Name = "sslSeparator";
      this.sslSeparator.Size = new Size(4, 28);
      this.sslSeparator.Visible = false;
      this.sslLastSaved.BorderSides = ToolStripStatusLabelBorderSides.Right;
      this.sslLastSaved.Name = "sslLastSaved";
      this.sslLastSaved.Size = new Size(4, 43);
      this.sslLastSaved.TextAlign = ContentAlignment.MiddleLeft;
      this.sslDate.Name = "sslDate";
      this.sslDate.Size = new Size(106, 43);
      this.sslDate.Text = "10/10/2008";
      this.sslDate.TextAlign = ContentAlignment.MiddleLeft;
      this.pipelineMenu.ImageScalingSize = new Size(20, 20);
      this.pipelineMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.pipelineHeadItem
      });
      this.pipelineMenu.Location = new Point(0, 0);
      this.pipelineMenu.Name = "pipelineMenu";
      this.pipelineMenu.Size = new Size(200, 24);
      this.pipelineMenu.TabIndex = 0;
      this.pipelineHeadItem.DropDownItems.AddRange(new ToolStripItem[21]
      {
        (ToolStripItem) this.tsMenuItemNewLoan,
        (ToolStripItem) this.tsMenuItemEditLoan,
        (ToolStripItem) this.tsMenuItemOpenRecent,
        (ToolStripItem) this.tsMenuItemDuplicateLoan,
        (ToolStripItem) this.tsMenuItemImport,
        (ToolStripItem) this.toolStripSeparatorEx1,
        (ToolStripItem) this.tsMenuItemMoveToFolder,
        (ToolStripItem) this.tsMenuItemTransfer,
        (ToolStripItem) this.tsMenuItemDeleteLoan,
        (ToolStripItem) this.toolStripSeparatorEx2,
        (ToolStripItem) this.tsMenuItemRefresh,
        (ToolStripItem) this.tsMenuItemExportToExcel,
        (ToolStripItem) this.tsMenuItemPrintForms,
        (ToolStripItem) this.tsMenuItemManageAlerts,
        (ToolStripItem) this.toolStripSeparatorEx4,
        (ToolStripItem) this.tsMenuItemCustomizeColumns,
        (ToolStripItem) this.tsMenuItemSaveView,
        (ToolStripItem) this.tsMenuItemResetView,
        (ToolStripItem) this.tsMenuItemManagePipelineViews,
        (ToolStripItem) this.toolStripSeparatorOpenWebView,
        (ToolStripItem) this.tsMenuItemOpenWebView
      });
      this.pipelineHeadItem.Name = "pipelineHeadItem";
      this.pipelineHeadItem.ShortcutKeys = Keys.P | Keys.Alt;
      this.pipelineHeadItem.Size = new Size(85, 20);
      this.pipelineHeadItem.Text = "&Pipeline";
      this.pipelineHeadItem.DropDownOpening += new EventHandler(this.pipelineHeadItem_Popup);
      this.tsMenuItemNewLoan.Image = (Image) componentResourceManager.GetObject("tsMenuItemNewLoan.Image");
      this.tsMenuItemNewLoan.Name = "tsMenuItemNewLoan";
      this.tsMenuItemNewLoan.ShortcutKeys = Keys.N | Keys.Control;
      this.tsMenuItemNewLoan.Size = new Size(292, 30);
      this.tsMenuItemNewLoan.Tag = (object) "PI_New";
      this.tsMenuItemNewLoan.Text = "&New Loan...";
      this.tsMenuItemNewLoan.Click += new EventHandler(this.pipeline_Click);
      this.tsMenuItemEditLoan.Image = (Image) componentResourceManager.GetObject("tsMenuItemEditLoan.Image");
      this.tsMenuItemEditLoan.Name = "tsMenuItemEditLoan";
      this.tsMenuItemEditLoan.Size = new Size(292, 30);
      this.tsMenuItemEditLoan.Tag = (object) "PI_Edit";
      this.tsMenuItemEditLoan.Text = "&Edit Loan";
      this.tsMenuItemEditLoan.Click += new EventHandler(this.pipeline_Click);
      this.tsMenuItemOpenRecent.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.tsMenuItemOpenRecentPlaceHolder
      });
      this.tsMenuItemOpenRecent.Name = "tsMenuItemOpenRecent";
      this.tsMenuItemOpenRecent.Size = new Size(292, 30);
      this.tsMenuItemOpenRecent.Tag = (object) "PI_Recent";
      this.tsMenuItemOpenRecent.Text = "Open &Recent";
      this.tsMenuItemOpenRecent.Click += new EventHandler(this.pipeline_Click);
      this.tsMenuItemOpenRecent.MouseEnter += new EventHandler(this.tsMenuItemOpenRecent_MouseEnter);
      this.tsMenuItemOpenRecentPlaceHolder.Name = "tsMenuItemOpenRecentPlaceHolder";
      this.tsMenuItemOpenRecentPlaceHolder.Size = new Size(84, 30);
      this.tsMenuItemDuplicateLoan.Image = (Image) componentResourceManager.GetObject("tsMenuItemDuplicateLoan.Image");
      this.tsMenuItemDuplicateLoan.Name = "tsMenuItemDuplicateLoan";
      this.tsMenuItemDuplicateLoan.Size = new Size(292, 30);
      this.tsMenuItemDuplicateLoan.Tag = (object) "PI_Duplicate";
      this.tsMenuItemDuplicateLoan.Text = "D&uplicate Loan...";
      this.tsMenuItemDuplicateLoan.Click += new EventHandler(this.pipeline_Click);
      this.tsMenuItemImport.Name = "tsMenuItemImport";
      this.tsMenuItemImport.ShortcutKeys = Keys.I | Keys.Alt;
      this.tsMenuItemImport.Size = new Size(292, 30);
      this.tsMenuItemImport.Tag = (object) "PI_Import";
      this.tsMenuItemImport.Text = "&Import...";
      this.tsMenuItemImport.Click += new EventHandler(this.pipeline_Click);
      this.toolStripSeparatorEx1.Name = "toolStripSeparatorEx1";
      this.toolStripSeparatorEx1.Size = new Size(289, 6);
      this.tsMenuItemMoveToFolder.Name = "tsMenuItemMoveToFolder";
      this.tsMenuItemMoveToFolder.ShortcutKeys = Keys.M | Keys.Alt;
      this.tsMenuItemMoveToFolder.Size = new Size(292, 30);
      this.tsMenuItemMoveToFolder.Tag = (object) "PI_Move";
      this.tsMenuItemMoveToFolder.Text = "&Move to Folder...";
      this.tsMenuItemMoveToFolder.Click += new EventHandler(this.pipeline_Click);
      this.tsMenuItemTransfer.Name = "tsMenuItemTransfer";
      this.tsMenuItemTransfer.ShortcutKeys = Keys.T | Keys.Alt;
      this.tsMenuItemTransfer.Size = new Size(292, 30);
      this.tsMenuItemTransfer.Tag = (object) "PI_Transfer";
      this.tsMenuItemTransfer.Text = "&Transfer...";
      this.tsMenuItemTransfer.Click += new EventHandler(this.pipeline_Click);
      this.tsMenuItemDeleteLoan.Image = (Image) componentResourceManager.GetObject("tsMenuItemDeleteLoan.Image");
      this.tsMenuItemDeleteLoan.Name = "tsMenuItemDeleteLoan";
      this.tsMenuItemDeleteLoan.ShortcutKeys = Keys.D | Keys.Alt;
      this.tsMenuItemDeleteLoan.Size = new Size(292, 30);
      this.tsMenuItemDeleteLoan.Tag = (object) "PI_Delete";
      this.tsMenuItemDeleteLoan.Text = "&Delete Loan";
      this.tsMenuItemDeleteLoan.Click += new EventHandler(this.pipeline_Click);
      this.toolStripSeparatorEx2.Name = "toolStripSeparatorEx2";
      this.toolStripSeparatorEx2.Size = new Size(289, 6);
      this.tsMenuItemRefresh.Name = "tsMenuItemRefresh";
      this.tsMenuItemRefresh.ShortcutKeys = Keys.F5;
      this.tsMenuItemRefresh.Size = new Size(292, 30);
      this.tsMenuItemRefresh.Tag = (object) "PI_Refresh";
      this.tsMenuItemRefresh.Text = "Re&fresh";
      this.tsMenuItemRefresh.Click += new EventHandler(this.pipeline_Click);
      this.tsMenuItemExportToExcel.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsExportSelectedLoansMenu,
        (ToolStripItem) this.tsExportAllLoansMenu
      });
      this.tsMenuItemExportToExcel.Image = (Image) componentResourceManager.GetObject("tsMenuItemExportToExcel.Image");
      this.tsMenuItemExportToExcel.Name = "tsMenuItemExportToExcel";
      this.tsMenuItemExportToExcel.Size = new Size(292, 30);
      this.tsMenuItemExportToExcel.Tag = (object) "PI_Export";
      this.tsMenuItemExportToExcel.Text = "Export to &Excel";
      this.tsExportSelectedLoansMenu.Name = "tsExportSelectedLoansMenu";
      this.tsExportSelectedLoansMenu.Size = new Size(281, 30);
      this.tsExportSelectedLoansMenu.Tag = (object) "PI_ExportSelected";
      this.tsExportSelectedLoansMenu.Text = "&Selected Loans Only...";
      this.tsExportSelectedLoansMenu.Click += new EventHandler(this.pipeline_Click);
      this.tsExportAllLoansMenu.Name = "tsExportAllLoansMenu";
      this.tsExportAllLoansMenu.Size = new Size(281, 30);
      this.tsExportAllLoansMenu.Tag = (object) "PI_ExportAll";
      this.tsExportAllLoansMenu.Text = "&All Loans on All Pages...";
      this.tsExportAllLoansMenu.Click += new EventHandler(this.pipeline_Click);
      this.tsMenuItemPrintForms.Image = (Image) componentResourceManager.GetObject("tsMenuItemPrintForms.Image");
      this.tsMenuItemPrintForms.Name = "tsMenuItemPrintForms";
      this.tsMenuItemPrintForms.ShortcutKeys = Keys.P | Keys.Control;
      this.tsMenuItemPrintForms.Size = new Size(292, 30);
      this.tsMenuItemPrintForms.Tag = (object) "PI_Print";
      this.tsMenuItemPrintForms.Text = "&Print Forms...";
      this.tsMenuItemPrintForms.Click += new EventHandler(this.pipeline_Click);
      this.tsMenuItemManageAlerts.Name = "tsMenuItemManageAlerts";
      this.tsMenuItemManageAlerts.Size = new Size(292, 30);
      this.tsMenuItemManageAlerts.Tag = (object) "PI_ManageAlerts";
      this.tsMenuItemManageAlerts.Text = "Manage &Alerts...";
      this.tsMenuItemManageAlerts.Click += new EventHandler(this.pipeline_Click);
      this.toolStripSeparatorEx4.Name = "toolStripSeparatorEx4";
      this.toolStripSeparatorEx4.Size = new Size(289, 6);
      this.tsMenuItemCustomizeColumns.Name = "tsMenuItemCustomizeColumns";
      this.tsMenuItemCustomizeColumns.Size = new Size(292, 30);
      this.tsMenuItemCustomizeColumns.Tag = (object) "PI_Columns";
      this.tsMenuItemCustomizeColumns.Text = "&Customize Columns...";
      this.tsMenuItemCustomizeColumns.Click += new EventHandler(this.pipeline_Click);
      this.tsMenuItemSaveView.Name = "tsMenuItemSaveView";
      this.tsMenuItemSaveView.Size = new Size(292, 30);
      this.tsMenuItemSaveView.Tag = (object) "PI_SaveView";
      this.tsMenuItemSaveView.Text = "Sav&e View";
      this.tsMenuItemSaveView.Click += new EventHandler(this.pipeline_Click);
      this.tsMenuItemResetView.Name = "tsMenuItemResetView";
      this.tsMenuItemResetView.Size = new Size(292, 30);
      this.tsMenuItemResetView.Tag = (object) "PI_ResetView";
      this.tsMenuItemResetView.Text = "&Reset View";
      this.tsMenuItemResetView.Click += new EventHandler(this.pipeline_Click);
      this.tsMenuItemManagePipelineViews.Name = "tsMenuItemManagePipelineViews";
      this.tsMenuItemManagePipelineViews.Size = new Size(292, 30);
      this.tsMenuItemManagePipelineViews.Tag = (object) "PI_ManageViews";
      this.tsMenuItemManagePipelineViews.Text = "Manage Pipeline Vie&ws";
      this.tsMenuItemManagePipelineViews.Click += new EventHandler(this.pipeline_Click);
      this.toolStripSeparatorOpenWebView.Name = "toolStripSeparatorOpenWebView";
      this.toolStripSeparatorOpenWebView.Size = new Size(201, 6);
      this.tsMenuItemOpenWebView.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.tsOpenWebViewLoan,
        (ToolStripItem) this.tsOpportunitiesMenu,
        (ToolStripItem) this.tsProspectsMenu
      });
      this.tsMenuItemOpenWebView.Name = "tsMenuItemOpenWebView";
      this.tsMenuItemOpenWebView.Size = new Size(204, 22);
      this.tsMenuItemOpenWebView.Tag = (object) "PI_OpenWebView";
      this.tsMenuItemOpenWebView.Text = "Open Web View";
      this.tsOpenWebViewLoan.Name = "tsOpenWebViewLoan";
      this.tsOpenWebViewLoan.Size = new Size(147, 22);
      this.tsOpenWebViewLoan.Tag = (object) "PI_OpenWebViewLoan";
      this.tsOpenWebViewLoan.Text = "Loan";
      this.tsOpenWebViewLoan.Click += new EventHandler(this.pipeline_Click);
      this.tsOpportunitiesMenu.Name = "tsOpportunitiesMenu";
      this.tsOpportunitiesMenu.Size = new Size(147, 22);
      this.tsOpportunitiesMenu.Tag = (object) "PI_Opportunities";
      this.tsOpportunitiesMenu.Text = "Opportunities";
      this.tsOpportunitiesMenu.Click += new EventHandler(this.pipeline_Click);
      this.tsProspectsMenu.Name = "tsProspectsMenu";
      this.tsProspectsMenu.Size = new Size(147, 22);
      this.tsProspectsMenu.Tag = (object) "PI_Prospects";
      this.tsProspectsMenu.Text = "Prospects";
      this.tsProspectsMenu.Click += new EventHandler(this.pipeline_Click);
      this.toolStripSeparatorEx21.Name = "toolStripSeparatorEx21";
      this.toolStripSeparatorEx21.Size = new Size(201, 6);
      this.tsMenuItemExportLoan.Name = "tsMenuItemExportLoan";
      this.tsMenuItemExportLoan.Size = new Size(32, 19);
      this.loanMenu.ImageScalingSize = new Size(20, 20);
      this.loanMenu.Items.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.loanHeadItem,
        (ToolStripItem) this.formHeadItem,
        (ToolStripItem) this.verifHeadItem,
        (ToolStripItem) this.loanToolsHeadItem,
        (ToolStripItem) this.menuItemServices,
        (ToolStripItem) this.coMortgagerHeadItem,
        (ToolStripItem) this.tsMenuItemTemplates
      });
      this.loanMenu.Location = new Point(0, 0);
      this.loanMenu.Name = "loanMenu";
      this.loanMenu.Size = new Size(200, 24);
      this.loanMenu.TabIndex = 0;
      this.loanHeadItem.DropDownItems.AddRange(new ToolStripItem[25]
      {
        (ToolStripItem) this.tsLoanNewLoan,
        (ToolStripItem) this.tsLoanSaveLoan,
        (ToolStripItem) this.tsLoanPrint,
        (ToolStripItem) this.tsLoanExportToDataTrac,
        (ToolStripItem) this.tsLoanExitLoan,
        (ToolStripItem) this.toolStripSeparator14,
        (ToolStripItem) this.tsLoanAddBorrowerPair,
        (ToolStripItem) this.tsLoanManageBorrowers,
        (ToolStripItem) this.tsDuplicateLockCheck,
        (ToolStripItem) this.toolStripSeparator15,
        (ToolStripItem) this.tsMilestoneDates,
        (ToolStripItem) this.tsMilestoneList,
        (ToolStripItem) this.toolStripSeparator19,
        (ToolStripItem) this.tsLoanTemplateSet,
        (ToolStripItem) this.tsLoanProgramTemplate,
        (ToolStripItem) this.tsLoanClosingCost,
        (ToolStripItem) this.tsLoanDocumentSet,
        (ToolStripItem) this.tsLoanTaskSet,
        (ToolStripItem) this.tsLoanDataTemplate,
        (ToolStripItem) this.tsLoanInputFormSet,
        (ToolStripItem) this.tsLoanSettlementServiceSet,
        (ToolStripItem) this.tsLoanAffiliateTemplate,
        (ToolStripItem) this.tsLoanRevertToDefaultFormList,
        (ToolStripItem) this.toolStripSeparator16,
        (ToolStripItem) this.tsLoanGoToField
      });
      this.loanHeadItem.Name = "loanHeadItem";
      this.loanHeadItem.Size = new Size(62, 20);
      this.loanHeadItem.Text = "&Loan";
      this.loanHeadItem.DropDownOpening += new EventHandler(this.loanHeadItem_DropDownOpening);
      this.tsLoanNewLoan.Image = (Image) Resources.new_file;
      this.tsLoanNewLoan.Name = "tsLoanNewLoan";
      this.tsLoanNewLoan.ShortcutKeys = Keys.N | Keys.Control;
      this.tsLoanNewLoan.Size = new Size(488, 30);
      this.tsLoanNewLoan.Tag = (object) "PI_New";
      this.tsLoanNewLoan.Text = "&New Loan...";
      this.tsLoanNewLoan.Click += new EventHandler(this.pipeline_Click);
      this.tsLoanSaveLoan.Image = (Image) Resources.save;
      this.tsLoanSaveLoan.Name = "tsLoanSaveLoan";
      this.tsLoanSaveLoan.ShortcutKeys = Keys.S | Keys.Control;
      this.tsLoanSaveLoan.Size = new Size(488, 30);
      this.tsLoanSaveLoan.Text = "&Save Loan";
      this.tsLoanSaveLoan.Click += new EventHandler(this.loanConfig_Click);
      this.tsLoanPrint.Image = (Image) Resources.print;
      this.tsLoanPrint.Name = "tsLoanPrint";
      this.tsLoanPrint.ShortcutKeys = Keys.P | Keys.Control;
      this.tsLoanPrint.Size = new Size(488, 30);
      this.tsLoanPrint.Text = "&Print...";
      this.tsLoanPrint.Click += new EventHandler(this.loanConfig_Click);
      this.tsLoanExportToDataTrac.Name = "tsLoanExportToDataTrac";
      this.tsLoanExportToDataTrac.Size = new Size(488, 30);
      this.tsLoanExportToDataTrac.Text = "&Submit to DataTrac";
      this.tsLoanExportToDataTrac.Click += new EventHandler(this.loanConfig_Click);
      this.tsLoanExitLoan.Name = "tsLoanExitLoan";
      this.tsLoanExitLoan.Size = new Size(488, 30);
      this.tsLoanExitLoan.Text = "E&xit Loan File";
      this.tsLoanExitLoan.Click += new EventHandler(this.loanConfig_Click);
      this.toolStripSeparator14.Name = "toolStripSeparator14";
      this.toolStripSeparator14.Size = new Size(485, 6);
      this.tsLoanAddBorrowerPair.Name = "tsLoanAddBorrowerPair";
      this.tsLoanAddBorrowerPair.Size = new Size(488, 30);
      this.tsLoanAddBorrowerPair.Text = "&Add Borrower Pair";
      this.tsLoanAddBorrowerPair.Click += new EventHandler(this.loanConfig_Click);
      this.tsLoanManageBorrowers.Name = "tsLoanManageBorrowers";
      this.tsLoanManageBorrowers.Size = new Size(488, 30);
      this.tsLoanManageBorrowers.Text = "&Manage Borrowers";
      this.tsLoanManageBorrowers.Click += new EventHandler(this.loanConfig_Click);
      this.tsDuplicateLockCheck.Name = "tsDuplicateLockCheck";
      this.tsDuplicateLockCheck.Size = new Size(488, 30);
      this.tsDuplicateLockCheck.Text = "&Duplicate Loan Check";
      this.tsDuplicateLockCheck.Click += new EventHandler(this.loanConfig_Click);
      this.toolStripSeparator15.Name = "toolStripSeparator15";
      this.toolStripSeparator15.Size = new Size(485, 6);
      this.tsMilestoneDates.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.tsLockMilestoneDates,
        (ToolStripItem) this.tsUnlockMilestoneDates,
        (ToolStripItem) this.tsChangeMilestoneDates
      });
      this.tsMilestoneDates.Name = "tsMilestoneDates";
      this.tsMilestoneDates.Size = new Size(488, 30);
      this.tsMilestoneDates.Text = "Manage Milestone &Dates...";
      this.tsMilestoneDates.Click += new EventHandler(this.loanConfig_Click);
      this.tsMilestoneDates.MouseEnter += new EventHandler(this.tsMilestoneDates_MouseEnter);
      this.tsLockMilestoneDates.Name = "tsLockMilestoneDates";
      this.tsLockMilestoneDates.Size = new Size(300, 30);
      this.tsLockMilestoneDates.Text = "Apply &Manual Mode...";
      this.tsLockMilestoneDates.Click += new EventHandler(this.lockUnlock_Click);
      this.tsUnlockMilestoneDates.Name = "tsUnlockMilestoneDates";
      this.tsUnlockMilestoneDates.Size = new Size(300, 30);
      this.tsUnlockMilestoneDates.Text = "Apply &Automatic Mode...";
      this.tsUnlockMilestoneDates.Click += new EventHandler(this.lockUnlock_Click);
      this.tsChangeMilestoneDates.Name = "tsChangeMilestoneDates";
      this.tsChangeMilestoneDates.Size = new Size(300, 30);
      this.tsChangeMilestoneDates.Text = "&Change Milestone Dates...";
      this.tsChangeMilestoneDates.Click += new EventHandler(this.loanConfig_Click);
      this.tsMilestoneList.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.tsLockMilestoneList,
        (ToolStripItem) this.tsUnlockMilestoneList,
        (ToolStripItem) this.tsApplyMilestoneTemplate
      });
      this.tsMilestoneList.Name = "tsMilestoneList";
      this.tsMilestoneList.Size = new Size(488, 30);
      this.tsMilestoneList.Text = "Manage Milestone &Templates...";
      this.tsMilestoneList.Click += new EventHandler(this.loanConfig_Click);
      this.tsMilestoneList.MouseEnter += new EventHandler(this.tsMilestoneList_MouseEnter);
      this.tsLockMilestoneList.Name = "tsLockMilestoneList";
      this.tsLockMilestoneList.Size = new Size(313, 30);
      this.tsLockMilestoneList.Text = "Apply &Manual Mode...";
      this.tsLockMilestoneList.Click += new EventHandler(this.lockUnlock_Click);
      this.tsUnlockMilestoneList.Name = "tsUnlockMilestoneList";
      this.tsUnlockMilestoneList.Size = new Size(313, 30);
      this.tsUnlockMilestoneList.Text = "Apply &Automatic Mode...";
      this.tsUnlockMilestoneList.Click += new EventHandler(this.lockUnlock_Click);
      this.tsApplyMilestoneTemplate.Name = "tsApplyMilestoneTemplate";
      this.tsApplyMilestoneTemplate.Size = new Size(313, 30);
      this.tsApplyMilestoneTemplate.Text = "Apply Milestone &Template...";
      this.tsApplyMilestoneTemplate.Click += new EventHandler(this.loanConfig_Click);
      this.toolStripSeparator19.Name = "toolStripSeparator19";
      this.toolStripSeparator19.Size = new Size(485, 6);
      this.tsLoanTemplateSet.Name = "tsLoanTemplateSet";
      this.tsLoanTemplateSet.Size = new Size(488, 30);
      this.tsLoanTemplateSet.Text = "Apply Loan &Template Set...";
      this.tsLoanTemplateSet.Click += new EventHandler(this.loanConfig_Click);
      this.tsLoanProgramTemplate.Name = "tsLoanProgramTemplate";
      this.tsLoanProgramTemplate.Size = new Size(488, 30);
      this.tsLoanProgramTemplate.Text = "Apply &Loan Program Template...";
      this.tsLoanProgramTemplate.Click += new EventHandler(this.loanConfig_Click);
      this.tsLoanClosingCost.Name = "tsLoanClosingCost";
      this.tsLoanClosingCost.Size = new Size(488, 30);
      this.tsLoanClosingCost.Text = "Apply &Closing Cost Template...";
      this.tsLoanClosingCost.Click += new EventHandler(this.loanConfig_Click);
      this.tsLoanDocumentSet.Name = "tsLoanDocumentSet";
      this.tsLoanDocumentSet.Size = new Size(488, 30);
      this.tsLoanDocumentSet.Text = "A&ppend Document Set...";
      this.tsLoanDocumentSet.Click += new EventHandler(this.loanConfig_Click);
      this.tsLoanTaskSet.Name = "tsLoanTaskSet";
      this.tsLoanTaskSet.Size = new Size(488, 30);
      this.tsLoanTaskSet.Text = "Append Tas&k Set...";
      this.tsLoanTaskSet.Click += new EventHandler(this.loanConfig_Click);
      this.tsLoanDataTemplate.Name = "tsLoanDataTemplate";
      this.tsLoanDataTemplate.Size = new Size(488, 30);
      this.tsLoanDataTemplate.Text = "Append &Data Template";
      this.tsLoanDataTemplate.Click += new EventHandler(this.loanConfig_Click);
      this.tsLoanInputFormSet.Name = "tsLoanInputFormSet";
      this.tsLoanInputFormSet.Size = new Size(488, 30);
      this.tsLoanInputFormSet.Text = "Apply Input &Form Set Template...";
      this.tsLoanInputFormSet.Click += new EventHandler(this.loanConfig_Click);
      this.tsLoanSettlementServiceSet.Name = "tsLoanSettlementServiceSet";
      this.tsLoanSettlementServiceSet.Size = new Size(488, 30);
      this.tsLoanSettlementServiceSet.Text = "Apply Settlement Service &Providers Template...";
      this.tsLoanSettlementServiceSet.Click += new EventHandler(this.loanConfig_Click);
      this.tsLoanAffiliateTemplate.Name = "tsLoanAffiliateTemplate";
      this.tsLoanAffiliateTemplate.Size = new Size(488, 30);
      this.tsLoanAffiliateTemplate.Text = "Apply &Affiliated Business Arrangement Template...";
      this.tsLoanAffiliateTemplate.Click += new EventHandler(this.loanConfig_Click);
      this.tsLoanRevertToDefaultFormList.Name = "tsLoanRevertToDefaultFormList";
      this.tsLoanRevertToDefaultFormList.Size = new Size(488, 30);
      this.tsLoanRevertToDefaultFormList.Text = "Revert to &Default Form List";
      this.tsLoanRevertToDefaultFormList.Click += new EventHandler(this.loanConfig_Click);
      this.toolStripSeparator16.Name = "toolStripSeparator16";
      this.toolStripSeparator16.Size = new Size(485, 6);
      this.tsLoanGoToField.Name = "tsLoanGoToField";
      this.tsLoanGoToField.ShortcutKeys = Keys.G | Keys.Control;
      this.tsLoanGoToField.Size = new Size(488, 30);
      this.tsLoanGoToField.Text = "&Go to Field...";
      this.tsLoanGoToField.Click += new EventHandler(this.loanConfig_Click);
      this.formHeadItem.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.menuItem3
      });
      this.formHeadItem.Name = "formHeadItem";
      this.formHeadItem.Size = new Size(74, 20);
      this.formHeadItem.Text = "&Forms";
      this.formHeadItem.DropDownOpening += new EventHandler(this.formHeadItem_Popup);
      this.menuItem3.Name = "menuItem3";
      this.menuItem3.Size = new Size(194, 30);
      this.menuItem3.Text = "dummyItem";
      this.verifHeadItem.DropDownItems.AddRange(new ToolStripItem[10]
      {
        (ToolStripItem) this.vodItem,
        (ToolStripItem) this.voggItem,
        (ToolStripItem) this.vooiItem,
        (ToolStripItem) this.vooaItem,
        (ToolStripItem) this.voeItem,
        (ToolStripItem) this.volItem,
        (ToolStripItem) this.voolItem,
        (ToolStripItem) this.vomItem,
        (ToolStripItem) this.voalItem,
        (ToolStripItem) this.vorItem
      });
      this.verifHeadItem.Name = "verifHeadItem";
      this.verifHeadItem.Size = new Size(67, 20);
      this.verifHeadItem.Text = "Ve&rifs";
      this.verifHeadItem.DropDownOpening += new EventHandler(this.verifHeadItem_Popup);
      this.vodItem.Name = "vodItem";
      this.vodItem.Size = new Size(343, 30);
      this.vodItem.Text = "VO&D";
      this.vodItem.Click += new EventHandler(this.verif_Click);
      this.voggItem.Name = "voggItem";
      this.voggItem.Size = new Size(343, 30);
      this.voggItem.Text = "Verification of &Gifts and Grants";
      this.voggItem.Click += new EventHandler(this.verif_Click);
      this.vooiItem.Name = "vooiItem";
      this.vooiItem.Size = new Size(343, 30);
      this.vooiItem.Text = "Verification of &Other Income";
      this.vooiItem.Click += new EventHandler(this.verif_Click);
      this.vooaItem.Name = "vooaItem";
      this.vooaItem.Size = new Size(343, 30);
      this.vooaItem.Text = "Verification of &Other Assets";
      this.vooaItem.Click += new EventHandler(this.verif_Click);
      this.voeItem.Name = "voeItem";
      this.voeItem.Size = new Size(343, 30);
      this.voeItem.Text = "VO&E";
      this.voeItem.Click += new EventHandler(this.verif_Click);
      this.volItem.Name = "volItem";
      this.volItem.Size = new Size(343, 30);
      this.volItem.Text = "VO&L";
      this.volItem.Click += new EventHandler(this.verif_Click);
      this.voolItem.Name = "voolItem";
      this.voolItem.Size = new Size(343, 30);
      this.voolItem.Text = "Verification of Other L&iability";
      this.voolItem.Click += new EventHandler(this.verif_Click);
      this.vomItem.Name = "vomItem";
      this.vomItem.Size = new Size(343, 30);
      this.vomItem.Text = "VO&M";
      this.vomItem.Click += new EventHandler(this.verif_Click);
      this.voalItem.Name = "voalItem";
      this.voalItem.Size = new Size(343, 30);
      this.voalItem.Text = "Verification of Additional Loans";
      this.voalItem.Click += new EventHandler(this.verif_Click);
      this.vorItem.Name = "vorItem";
      this.vorItem.Size = new Size(343, 30);
      this.vorItem.Text = "VO&R";
      this.vorItem.Click += new EventHandler(this.verif_Click);
      this.loanToolsHeadItem.DropDownItems.AddRange(new ToolStripItem[51]
      {
        (ToolStripItem) this.fileItem,
        (ToolStripItem) this.businessItem,
        (ToolStripItem) this.conversationItem,
        (ToolStripItem) this.correspondentLoanStatusMenuItem,
        (ToolStripItem) this.tpoInformationMenuItem,
        (ToolStripItem) this.taskItem,
        (ToolStripItem) this.menuItem49,
        (ToolStripItem) this.ausTrackingItem,
        (ToolStripItem) this.repWarrantTrackerItem,
        (ToolStripItem) this.disclosureTrackingItem,
        (ToolStripItem) this.feeVarianceWorksheetItem,
        (ToolStripItem) this.loCompensationItem,
        (ToolStripItem) this.antiSteeringSafeHarborItem,
        (ToolStripItem) this.netTangibleBenefitItem,
        (ToolStripItem) this.complianceReviewItem,
        (ToolStripItem) this.ecsDataViewerItem,
        (ToolStripItem) this.tqlServicesItem,
        (ToolStripItem) this.miCenterItem,
        (ToolStripItem) this.amortItem,
        (ToolStripItem) this.lockRequestItem,
        (ToolStripItem) this.lockComparisonItem,
        (ToolStripItem) this.menuItem20,
        (ToolStripItem) this.prequalItem,
        (ToolStripItem) this.debtItem,
        (ToolStripItem) this.comparisonItem,
        (ToolStripItem) this.cashItem,
        (ToolStripItem) this.rentItem,
        (ToolStripItem) this.bankerLineItem,
        (ToolStripItem) this.underwriterItem,
        (ToolStripItem) this.verificationTrackingItem,
        (ToolStripItem) this.fundingItem,
        (ToolStripItem) this.balanceItem,
        (ToolStripItem) this.brokerItem,
        (ToolStripItem) this.secondaryItem,
        (ToolStripItem) this.interimServicingItem,
        (ToolStripItem) this.shippingItem,
        (ToolStripItem) this.documentTrackingMgmtItem,
        (ToolStripItem) this.correspondentPurchaseItem,
        (ToolStripItem) this.purchaseItem,
        (ToolStripItem) this.menuItem19,
        (ToolStripItem) this.comortgagorItem,
        (ToolStripItem) this.piggybackLoanItem,
        (ToolStripItem) this.secureItem,
        (ToolStripItem) this.menuItem18,
        (ToolStripItem) this.complianceItem,
        (ToolStripItem) this.auditTrailItem,
        (ToolStripItem) this.profitItem,
        (ToolStripItem) this.trustItem,
        (ToolStripItem) this.menuItem4,
        (ToolStripItem) this.findFieldItem,
        (ToolStripItem) this.worstCasePricingItem
      });
      this.loanToolsHeadItem.Name = "loanToolsHeadItem";
      this.loanToolsHeadItem.Size = new Size(65, 20);
      this.loanToolsHeadItem.Text = "&Tools";
      this.loanToolsHeadItem.DropDownOpening += new EventHandler(this.toolsItem_Popup);
      this.fileItem.Name = "fileItem";
      this.fileItem.Size = new Size(415, 30);
      this.fileItem.Text = "&File Contacts";
      this.fileItem.Click += new EventHandler(this.loanTools_Click);
      this.businessItem.Name = "businessItem";
      this.businessItem.Size = new Size(415, 30);
      this.businessItem.Text = "&Business Contacts...";
      this.businessItem.Click += new EventHandler(this.loanTools_Click);
      this.conversationItem.Name = "conversationItem";
      this.conversationItem.Size = new Size(415, 30);
      this.conversationItem.Text = "&Conversation Log";
      this.conversationItem.Click += new EventHandler(this.loanTools_Click);
      this.correspondentLoanStatusMenuItem.Name = "correspondentLoanStatusMenuItem";
      this.correspondentLoanStatusMenuItem.Size = new Size(415, 30);
      this.correspondentLoanStatusMenuItem.Text = "Correspondent Loan Status";
      this.correspondentLoanStatusMenuItem.Click += new EventHandler(this.loanTools_Click);
      this.tpoInformationMenuItem.Name = "tpoInformationMenuItem";
      this.tpoInformationMenuItem.Size = new Size(415, 30);
      this.tpoInformationMenuItem.Text = "TPO Information";
      this.tpoInformationMenuItem.Click += new EventHandler(this.loanTools_Click);
      this.taskItem.Name = "taskItem";
      this.taskItem.Size = new Size(415, 30);
      this.taskItem.Text = "&Tasks";
      this.taskItem.Click += new EventHandler(this.loanTools_Click);
      this.menuItem49.Name = "menuItem49";
      this.menuItem49.Size = new Size(415, 30);
      this.menuItem49.Text = "Status &Online...";
      this.menuItem49.Click += new EventHandler(this.loanTools_Click);
      this.ausTrackingItem.Name = "ausTrackingItem";
      this.ausTrackingItem.Size = new Size(415, 30);
      this.ausTrackingItem.Text = "&AUS Tracking";
      this.ausTrackingItem.Click += new EventHandler(this.loanTools_Click);
      this.repWarrantTrackerItem.Name = "repWarrantTrackerItem";
      this.repWarrantTrackerItem.Size = new Size(415, 30);
      this.repWarrantTrackerItem.Text = "&Rep and Warrant Tracker";
      this.repWarrantTrackerItem.Click += new EventHandler(this.loanTools_Click);
      this.disclosureTrackingItem.Name = "disclosureTrackingItem";
      this.disclosureTrackingItem.Size = new Size(415, 30);
      this.disclosureTrackingItem.Text = "&Disclosure Tracking";
      this.disclosureTrackingItem.Click += new EventHandler(this.loanTools_Click);
      this.feeVarianceWorksheetItem.Name = "feeVarianceWorksheetItem";
      this.feeVarianceWorksheetItem.Size = new Size(415, 30);
      this.feeVarianceWorksheetItem.Text = "&Fee Variance Worksheet";
      this.feeVarianceWorksheetItem.Click += new EventHandler(this.loanTools_Click);
      this.loCompensationItem.Name = "loCompensationItem";
      this.loCompensationItem.Size = new Size(415, 30);
      this.loCompensationItem.Text = "&LO Compensation";
      this.loCompensationItem.Click += new EventHandler(this.loanTools_Click);
      this.antiSteeringSafeHarborItem.Name = "antiSteeringSafeHarborItem";
      this.antiSteeringSafeHarborItem.Size = new Size(415, 30);
      this.antiSteeringSafeHarborItem.Text = "&Anti-Steering Safe Harbor Disclosure";
      this.antiSteeringSafeHarborItem.Click += new EventHandler(this.loanTools_Click);
      this.netTangibleBenefitItem.Name = "netTangibleBenefitItem";
      this.netTangibleBenefitItem.Size = new Size(415, 30);
      this.netTangibleBenefitItem.Text = "&Net Tangible Benefit";
      this.netTangibleBenefitItem.Click += new EventHandler(this.loanTools_Click);
      this.complianceReviewItem.Name = "complianceReviewItem";
      this.complianceReviewItem.Size = new Size(415, 30);
      this.complianceReviewItem.Text = "&Compliance Review";
      this.complianceReviewItem.Click += new EventHandler(this.loanTools_Click);
      this.ecsDataViewerItem.Name = "ecsDataViewerItem";
      this.ecsDataViewerItem.Size = new Size(415, 30);
      this.ecsDataViewerItem.Text = "&ECS Data Viewer";
      this.ecsDataViewerItem.Click += new EventHandler(this.loanTools_Click);
      this.tqlServicesItem.Name = "tqlServicesItem";
      this.tqlServicesItem.Size = new Size(415, 30);
      this.tqlServicesItem.Text = "&TQL Services";
      this.tqlServicesItem.Click += new EventHandler(this.loanTools_Click);
      this.miCenterItem.Name = "miCenterItem";
      this.miCenterItem.Size = new Size(289, 22);
      this.miCenterItem.Text = "&MI Center";
      this.miCenterItem.Click += new EventHandler(this.loanTools_Click);
      this.amortItem.Name = "amortItem";
      this.amortItem.Size = new Size(415, 30);
      this.amortItem.Text = "&Amortization Schedule";
      this.amortItem.Click += new EventHandler(this.loanTools_Click);
      this.lockRequestItem.Name = "lockRequestItem";
      this.lockRequestItem.Size = new Size(415, 30);
      this.lockRequestItem.Text = "&Lock Request Form";
      this.lockRequestItem.Click += new EventHandler(this.loanTools_Click);
      this.lockComparisonItem.Name = "lockComparisonItem";
      this.lockComparisonItem.Size = new Size(289, 22);
      this.lockComparisonItem.Text = "&Lock Comparison Tool";
      this.lockComparisonItem.Click += new EventHandler(this.loanTools_Click);
      this.menuItem20.Name = "menuItem20";
      this.menuItem20.Size = new Size(412, 6);
      this.prequalItem.Name = "prequalItem";
      this.prequalItem.Size = new Size(415, 30);
      this.prequalItem.Text = "Pre&qualification";
      this.prequalItem.Click += new EventHandler(this.loanTools_Click);
      this.debtItem.Name = "debtItem";
      this.debtItem.Size = new Size(415, 30);
      this.debtItem.Text = "D&ebt Consolidation";
      this.debtItem.Click += new EventHandler(this.loanTools_Click);
      this.comparisonItem.Name = "comparisonItem";
      this.comparisonItem.Size = new Size(415, 30);
      this.comparisonItem.Text = "Loan Co&mparison";
      this.comparisonItem.Click += new EventHandler(this.loanTools_Click);
      this.cashItem.Name = "cashItem";
      this.cashItem.Size = new Size(415, 30);
      this.cashItem.Text = "Cas&h-to-Close";
      this.cashItem.Click += new EventHandler(this.loanTools_Click);
      this.rentItem.Name = "rentItem";
      this.rentItem.Size = new Size(415, 30);
      this.rentItem.Text = "&Rent vs. Own";
      this.rentItem.Click += new EventHandler(this.loanTools_Click);
      this.bankerLineItem.Name = "bankerLineItem";
      this.bankerLineItem.Size = new Size(412, 6);
      this.underwriterItem.Name = "underwriterItem";
      this.underwriterItem.Size = new Size(415, 30);
      this.underwriterItem.Text = "&Underwriter Summary";
      this.underwriterItem.Click += new EventHandler(this.loanTools_Click);
      this.verificationTrackingItem.Name = "verificationTrackingItem";
      this.verificationTrackingItem.Size = new Size(415, 30);
      this.verificationTrackingItem.Text = "&Verification and Documentation Tracking";
      this.verificationTrackingItem.Click += new EventHandler(this.loanTools_Click);
      this.fundingItem.Name = "fundingItem";
      this.fundingItem.Size = new Size(415, 30);
      this.fundingItem.Text = "&Funding Worksheet";
      this.fundingItem.Click += new EventHandler(this.loanTools_Click);
      this.balanceItem.Name = "balanceItem";
      this.balanceItem.Size = new Size(415, 30);
      this.balanceItem.Text = "&Funding Balancing Worksheet";
      this.balanceItem.Click += new EventHandler(this.loanTools_Click);
      this.brokerItem.Name = "brokerItem";
      this.brokerItem.Size = new Size(415, 30);
      this.brokerItem.Text = "&Broker Check Calculation";
      this.brokerItem.Click += new EventHandler(this.loanTools_Click);
      this.secondaryItem.Name = "secondaryItem";
      this.secondaryItem.Size = new Size(415, 30);
      this.secondaryItem.Text = "&Secondary Registration";
      this.secondaryItem.Click += new EventHandler(this.loanTools_Click);
      this.interimServicingItem.Name = "interimServicingItem";
      this.interimServicingItem.Size = new Size(415, 30);
      this.interimServicingItem.Text = "&Interim Servicing Worksheet";
      this.interimServicingItem.Click += new EventHandler(this.loanTools_Click);
      this.shippingItem.Name = "shippingItem";
      this.shippingItem.Size = new Size(415, 30);
      this.shippingItem.Text = "&Shipping Detail";
      this.shippingItem.Click += new EventHandler(this.loanTools_Click);
      this.documentTrackingMgmtItem.Name = "documentTrackingMgmtItem";
      this.documentTrackingMgmtItem.Size = new Size(415, 30);
      this.documentTrackingMgmtItem.Text = "Collateral Tracking";
      this.documentTrackingMgmtItem.Click += new EventHandler(this.loanTools_Click);
      this.correspondentPurchaseItem.Name = "correspondentPurchaseItem";
      this.correspondentPurchaseItem.Size = new Size(415, 30);
      this.correspondentPurchaseItem.Text = "&Correspondent Purchase Advice Form";
      this.correspondentPurchaseItem.Click += new EventHandler(this.loanTools_Click);
      this.purchaseItem.Name = "purchaseItem";
      this.purchaseItem.Size = new Size(415, 30);
      this.purchaseItem.Text = "&Purchase Advice Form";
      this.purchaseItem.Click += new EventHandler(this.loanTools_Click);
      this.menuItem19.Name = "menuItem19";
      this.menuItem19.Size = new Size(412, 6);
      this.comortgagorItem.Name = "comortgagorItem";
      this.comortgagorItem.Size = new Size(415, 30);
      this.comortgagorItem.Text = "Co-&Mortgagors";
      this.comortgagorItem.Click += new EventHandler(this.loanTools_Click);
      this.piggybackLoanItem.Name = "piggybackLoanItem";
      this.piggybackLoanItem.Size = new Size(415, 30);
      this.piggybackLoanItem.Text = "Pi&ggyback Loans";
      this.piggybackLoanItem.Click += new EventHandler(this.loanTools_Click);
      this.secureItem.Name = "secureItem";
      this.secureItem.Size = new Size(415, 30);
      this.secureItem.Text = "&Secure Form Transfer...";
      this.secureItem.Click += new EventHandler(this.loanTools_Click);
      this.menuItem18.Name = "menuItem18";
      this.menuItem18.Size = new Size(412, 6);
      this.complianceItem.Name = "complianceItem";
      this.complianceItem.Size = new Size(415, 30);
      this.complianceItem.Text = "Compliance Aud&itor";
      this.complianceItem.Click += new EventHandler(this.loanTools_Click);
      this.auditTrailItem.Name = "auditTrailItem";
      this.auditTrailItem.Size = new Size(415, 30);
      this.auditTrailItem.Text = "&Audit Trail";
      this.auditTrailItem.Click += new EventHandler(this.loanTools_Click);
      this.profitItem.Name = "profitItem";
      this.profitItem.Size = new Size(415, 30);
      this.profitItem.Text = "&Profit Management";
      this.profitItem.Click += new EventHandler(this.loanTools_Click);
      this.trustItem.Name = "trustItem";
      this.trustItem.Size = new Size(415, 30);
      this.trustItem.Text = "&Trust Account";
      this.trustItem.Click += new EventHandler(this.loanTools_Click);
      this.menuItem4.Name = "menuItem4";
      this.menuItem4.Size = new Size(412, 6);
      this.findFieldItem.Name = "findFieldItem";
      this.findFieldItem.ShortcutKeys = Keys.G | Keys.Control;
      this.findFieldItem.Size = new Size(415, 30);
      this.findFieldItem.Text = "&Go To Field...";
      this.findFieldItem.Click += new EventHandler(this.loanTools_Click);
      this.worstCasePricingItem.Name = "worstCasePricingItem";
      this.worstCasePricingItem.Size = new Size(415, 30);
      this.worstCasePricingItem.Text = "&Worst Case Pricing";
      this.worstCasePricingItem.Click += new EventHandler(this.loanTools_Click);
      this.menuItemServices.Name = "menuItemServices";
      this.menuItemServices.Size = new Size(87, 20);
      this.menuItemServices.Text = "&Services";
      this.menuItemServices.DropDownOpening += new EventHandler(this.menuItemServices_DropDownOpening);
      this.coMortgagerHeadItem.DropDownItems.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.menuItem12,
        (ToolStripItem) this.menuItem21,
        (ToolStripItem) this.menuItem16,
        (ToolStripItem) this.SwapCurrentItem,
        (ToolStripItem) this.deleteCurrentItem,
        (ToolStripItem) this.menuItem23,
        (ToolStripItem) this.menuItem24
      });
      this.coMortgagerHeadItem.Name = "coMortgagerHeadItem";
      this.coMortgagerHeadItem.Size = new Size(104, 20);
      this.coMortgagerHeadItem.Text = "Borro&wers";
      this.coMortgagerHeadItem.Visible = false;
      this.coMortgagerHeadItem.DropDownOpening += new EventHandler(this.coMortgagerHeadItem_Popup);
      this.menuItem12.Name = "menuItem12";
      this.menuItem12.Size = new Size(417, 6);
      this.menuItem21.Name = "menuItem21";
      this.menuItem21.Size = new Size(420, 30);
      this.menuItem21.Text = "&Add a New Co-Mortgagor";
      this.menuItem21.Click += new EventHandler(this.BorrowerPairs_Click);
      this.menuItem16.Name = "menuItem16";
      this.menuItem16.Size = new Size(417, 6);
      this.SwapCurrentItem.Name = "SwapCurrentItem";
      this.SwapCurrentItem.Size = new Size(420, 30);
      this.SwapCurrentItem.Text = "&Swap Current Borrower and Co-Borrower";
      this.SwapCurrentItem.Click += new EventHandler(this.BorrowerPairs_Click);
      this.deleteCurrentItem.Name = "deleteCurrentItem";
      this.deleteCurrentItem.Size = new Size(420, 30);
      this.deleteCurrentItem.Text = "&Delete Current Co-Borrower";
      this.deleteCurrentItem.Click += new EventHandler(this.BorrowerPairs_Click);
      this.menuItem23.Name = "menuItem23";
      this.menuItem23.Size = new Size(417, 6);
      this.menuItem24.Name = "menuItem24";
      this.menuItem24.Size = new Size(420, 30);
      this.menuItem24.Text = "&Manage Co-Mortgagors";
      this.menuItem24.Click += new EventHandler(this.BorrowerPairs_Click);
      this.tsMenuItemTemplates.DropDownItems.AddRange(new ToolStripItem[11]
      {
        (ToolStripItem) this.tsMenuItemSelectLoanTemplate,
        (ToolStripItem) this.tsSeparator1,
        (ToolStripItem) this.lpItem,
        (ToolStripItem) this.ccItem,
        (ToolStripItem) this.menuItem71,
        (ToolStripItem) this.defaultFormsItem,
        (ToolStripItem) this.selectFormsItem,
        (ToolStripItem) this.menuItem74,
        (ToolStripItem) this.addDocSetItem,
        (ToolStripItem) this.addTaskSetItem,
        (ToolStripItem) this.addDataSetItem
      });
      this.tsMenuItemTemplates.Name = "tsMenuItemTemplates";
      this.tsMenuItemTemplates.Size = new Size(103, 20);
      this.tsMenuItemTemplates.Text = "Temp&lates";
      this.tsMenuItemTemplates.Visible = false;
      this.tsMenuItemSelectLoanTemplate.Name = "tsMenuItemSelectLoanTemplate";
      this.tsMenuItemSelectLoanTemplate.Size = new Size(323, 30);
      this.tsMenuItemSelectLoanTemplate.Text = "Select Loan &Template...";
      this.tsMenuItemSelectLoanTemplate.Click += new EventHandler(this.loanConfig_Click);
      this.tsSeparator1.Name = "tsSeparator1";
      this.tsSeparator1.Size = new Size(320, 6);
      this.lpItem.Name = "lpItem";
      this.lpItem.Size = new Size(323, 30);
      this.lpItem.Text = "Select &Loan Program...";
      this.lpItem.Click += new EventHandler(this.loanConfig_Click);
      this.ccItem.Name = "ccItem";
      this.ccItem.Size = new Size(323, 30);
      this.ccItem.Text = "Select &Closing Cost...";
      this.ccItem.Click += new EventHandler(this.loanConfig_Click);
      this.menuItem71.Name = "menuItem71";
      this.menuItem71.Size = new Size(320, 6);
      this.defaultFormsItem.Name = "defaultFormsItem";
      this.defaultFormsItem.Size = new Size(323, 30);
      this.defaultFormsItem.Text = "Use &Default Form List";
      this.defaultFormsItem.Click += new EventHandler(this.loanConfig_Click);
      this.selectFormsItem.Name = "selectFormsItem";
      this.selectFormsItem.Size = new Size(323, 30);
      this.selectFormsItem.Text = "Select &Form List...";
      this.selectFormsItem.Click += new EventHandler(this.loanConfig_Click);
      this.menuItem74.Name = "menuItem74";
      this.menuItem74.Size = new Size(320, 6);
      this.addDocSetItem.Name = "addDocSetItem";
      this.addDocSetItem.Size = new Size(323, 30);
      this.addDocSetItem.Text = "&Append Document Set...";
      this.addDocSetItem.Click += new EventHandler(this.loanConfig_Click);
      this.addTaskSetItem.Name = "addTaskSetItem";
      this.addTaskSetItem.Size = new Size(323, 30);
      this.addTaskSetItem.Text = "Append Task &Set...";
      this.addTaskSetItem.Click += new EventHandler(this.loanConfig_Click);
      this.addDataSetItem.Name = "addDataSetItem";
      this.addDataSetItem.Size = new Size(323, 30);
      this.addDataSetItem.Text = "Append &Misc. Data Template";
      this.addDataSetItem.Click += new EventHandler(this.loanConfig_Click);
      this.contactMenu.ImageScalingSize = new Size(20, 20);
      this.contactMenu.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.contactHeadItem,
        (ToolStripItem) this.importContactsItem
      });
      this.contactMenu.Location = new Point(0, 0);
      this.contactMenu.Name = "contactMenu";
      this.contactMenu.Size = new Size(200, 24);
      this.contactMenu.TabIndex = 0;
      this.contactHeadItem.DropDownItems.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.menuItemBorContacts,
        (ToolStripItem) this.menuItemBizContacts,
        (ToolStripItem) this.menuItemCalendar,
        (ToolStripItem) this.menuItemTasks,
        (ToolStripItem) this.menuItemCompaign
      });
      this.contactHeadItem.Name = "contactHeadItem";
      this.contactHeadItem.Size = new Size((int) sbyte.MaxValue, 20);
      this.contactHeadItem.Text = "&Contact View";
      this.contactHeadItem.Visible = false;
      this.menuItemBorContacts.Name = "menuItemBorContacts";
      this.menuItemBorContacts.Size = new Size(242, 30);
      this.menuItemBorContacts.Text = "Borro&wer Contacts";
      this.menuItemBorContacts.Click += new EventHandler(this.contactMenu_Click);
      this.menuItemBizContacts.Name = "menuItemBizContacts";
      this.menuItemBizContacts.Size = new Size(242, 30);
      this.menuItemBizContacts.Text = "Bus&iness Contacts";
      this.menuItemBizContacts.Click += new EventHandler(this.contactMenu_Click);
      this.menuItemCalendar.Name = "menuItemCalendar";
      this.menuItemCalendar.Size = new Size(242, 30);
      this.menuItemCalendar.Text = "&Calendar";
      this.menuItemCalendar.Click += new EventHandler(this.contactMenu_Click);
      this.menuItemTasks.Name = "menuItemTasks";
      this.menuItemTasks.Size = new Size(242, 30);
      this.menuItemTasks.Text = "&Tasks";
      this.menuItemTasks.Click += new EventHandler(this.contactMenu_Click);
      this.menuItemCompaign.Name = "menuItemCompaign";
      this.menuItemCompaign.Size = new Size(242, 30);
      this.menuItemCompaign.Text = "Ca&mpaigns";
      this.menuItemCompaign.Click += new EventHandler(this.contactMenu_Click);
      this.importContactsItem.DropDownItems.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.menuItemImportBorrowerContacts,
        (ToolStripItem) this.menuItemImportBusinessContacts,
        (ToolStripItem) this.menuItemExportDivider,
        (ToolStripItem) this.menuItemExportBorrowerContacts,
        (ToolStripItem) this.menuItemExportBusinessContacts,
        (ToolStripItem) this.menuItemSyncDivider,
        (ToolStripItem) this.menuItemSynchronize
      });
      this.importContactsItem.Name = "importContactsItem";
      this.importContactsItem.Size = new Size(137, 20);
      this.importContactsItem.Text = "Impor&t/Export";
      this.importContactsItem.Visible = false;
      this.menuItemImportBorrowerContacts.Name = "menuItemImportBorrowerContacts";
      this.menuItemImportBorrowerContacts.Size = new Size(314, 30);
      this.menuItemImportBorrowerContacts.Text = "Import Borro&wer Contacts...";
      this.menuItemImportBorrowerContacts.Click += new EventHandler(this.contactMenu_Click);
      this.menuItemImportBusinessContacts.Name = "menuItemImportBusinessContacts";
      this.menuItemImportBusinessContacts.Size = new Size(314, 30);
      this.menuItemImportBusinessContacts.Text = "Import Bus&iness Contacts...";
      this.menuItemImportBusinessContacts.Click += new EventHandler(this.contactMenu_Click);
      this.menuItemExportDivider.Name = "menuItemExportDivider";
      this.menuItemExportDivider.Size = new Size(311, 6);
      this.menuItemExportBorrowerContacts.Name = "menuItemExportBorrowerContacts";
      this.menuItemExportBorrowerContacts.Size = new Size(314, 30);
      this.menuItemExportBorrowerContacts.Text = "Export &Borrower Contacts...";
      this.menuItemExportBorrowerContacts.Click += new EventHandler(this.contactMenu_Click);
      this.menuItemExportBusinessContacts.Name = "menuItemExportBusinessContacts";
      this.menuItemExportBusinessContacts.Size = new Size(314, 30);
      this.menuItemExportBusinessContacts.Text = "Export B&usiness Contacts...";
      this.menuItemExportBusinessContacts.Click += new EventHandler(this.contactMenu_Click);
      this.menuItemSyncDivider.Name = "menuItemSyncDivider";
      this.menuItemSyncDivider.Size = new Size(311, 6);
      this.menuItemSynchronize.Name = "menuItemSynchronize";
      this.menuItemSynchronize.Size = new Size(314, 30);
      this.menuItemSynchronize.Text = "&Synchronize...";
      this.menuItemSynchronize.Click += new EventHandler(this.contactMenu_Click);
      this.reportMenu.ImageScalingSize = new Size(20, 20);
      this.reportMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.reportHeadItem
      });
      this.reportMenu.Location = new Point(0, 0);
      this.reportMenu.Name = "reportMenu";
      this.reportMenu.Size = new Size(200, 24);
      this.reportMenu.TabIndex = 0;
      this.reportHeadItem.DropDownItems.AddRange(new ToolStripItem[10]
      {
        (ToolStripItem) this.addReportItem,
        (ToolStripItem) this.duplicateReportItem,
        (ToolStripItem) this.newFolderReportItem,
        (ToolStripItem) this.deleteReportItem,
        (ToolStripItem) this.menuItem81,
        (ToolStripItem) this.renameReportItem,
        (ToolStripItem) this.refreshReportItem,
        (ToolStripItem) this.menuItem70,
        (ToolStripItem) this.generateReportMenuItem,
        (ToolStripItem) this.tsMenuItemReportOption
      });
      this.reportHeadItem.Name = "reportHeadItem";
      this.reportHeadItem.Size = new Size(85, 20);
      this.reportHeadItem.Text = "&Reports";
      this.reportHeadItem.DropDownOpening += new EventHandler(this.reportHeadItem_DropDownOpening);
      this.addReportItem.Image = (Image) Resources.new_file;
      this.addReportItem.Name = "addReportItem";
      this.addReportItem.Size = new Size(348, 30);
      this.addReportItem.Tag = (object) "RPT_New";
      this.addReportItem.Text = "&New Report";
      this.addReportItem.Click += new EventHandler(this.report_Click);
      this.duplicateReportItem.Image = (Image) Resources.duplicate;
      this.duplicateReportItem.Name = "duplicateReportItem";
      this.duplicateReportItem.Size = new Size(348, 30);
      this.duplicateReportItem.Tag = (object) "RPT_Duplicate";
      this.duplicateReportItem.Text = "D&uplicate Report";
      this.duplicateReportItem.Click += new EventHandler(this.report_Click);
      this.newFolderReportItem.Image = (Image) Resources.new_folder;
      this.newFolderReportItem.Name = "newFolderReportItem";
      this.newFolderReportItem.Size = new Size(348, 30);
      this.newFolderReportItem.Tag = (object) "RPT_NewFolder";
      this.newFolderReportItem.Text = "New &Folder";
      this.newFolderReportItem.Click += new EventHandler(this.report_Click);
      this.deleteReportItem.Image = (Image) Resources.delete;
      this.deleteReportItem.Name = "deleteReportItem";
      this.deleteReportItem.ShortcutKeys = Keys.D | Keys.Alt;
      this.deleteReportItem.Size = new Size(348, 30);
      this.deleteReportItem.Tag = (object) "RPT_Delete";
      this.deleteReportItem.Text = "&Delete Report";
      this.deleteReportItem.Click += new EventHandler(this.report_Click);
      this.menuItem81.Name = "menuItem81";
      this.menuItem81.Size = new Size(345, 6);
      this.renameReportItem.Name = "renameReportItem";
      this.renameReportItem.Size = new Size(348, 30);
      this.renameReportItem.Tag = (object) "RPT_Rename";
      this.renameReportItem.Text = "R&ename";
      this.renameReportItem.Click += new EventHandler(this.report_Click);
      this.refreshReportItem.Name = "refreshReportItem";
      this.refreshReportItem.ShortcutKeys = Keys.F5;
      this.refreshReportItem.Size = new Size(348, 30);
      this.refreshReportItem.Tag = (object) "RPT_Refresh";
      this.refreshReportItem.Text = "&Refresh";
      this.refreshReportItem.Click += new EventHandler(this.report_Click);
      this.menuItem70.Name = "menuItem70";
      this.menuItem70.Size = new Size(345, 6);
      this.generateReportMenuItem.Name = "generateReportMenuItem";
      this.generateReportMenuItem.Size = new Size(348, 30);
      this.generateReportMenuItem.Tag = (object) "RPT_Generate";
      this.generateReportMenuItem.Text = "&Generate Report";
      this.generateReportMenuItem.Click += new EventHandler(this.report_Click);
      this.tsMenuItemReportOption.Name = "tsMenuItemReportOption";
      this.tsMenuItemReportOption.Size = new Size(348, 30);
      this.tsMenuItemReportOption.Tag = (object) "RPT_PromptForOptions";
      this.tsMenuItemReportOption.Text = "Add &Options when Reports Run";
      this.tsMenuItemReportOption.Click += new EventHandler(this.tsMenuItemReportOption_Click);
      this.tsMenuItemReportOption.Paint += new PaintEventHandler(this.tsMenuItemReportOption_Paint);
      this.dashboardMenu.ImageScalingSize = new Size(20, 20);
      this.dashboardMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.tsDashboardHeadItem
      });
      this.dashboardMenu.Location = new Point(0, 24);
      this.dashboardMenu.Name = "dashboardMenu";
      this.dashboardMenu.Size = new Size(792, 24);
      this.dashboardMenu.TabIndex = 3;
      this.tsDashboardHeadItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.tsDashboard_ManageView,
        (ToolStripItem) this.tsDashboard_ManageSnapshot
      });
      this.tsDashboardHeadItem.Name = "tsDashboardHeadItem";
      this.tsDashboardHeadItem.Size = new Size(112, 20);
      this.tsDashboardHeadItem.Text = "&Dashboard";
      this.tsDashboardHeadItem.DropDownOpening += new EventHandler(this.tsDashboardHeadItem_DropDownOpening);
      this.tsDashboard_ManageView.Name = "tsDashboard_ManageView";
      this.tsDashboard_ManageView.Size = new Size(315, 30);
      this.tsDashboard_ManageView.Text = "Manage Dashboard &Views...";
      this.tsDashboard_ManageView.Click += new EventHandler(this.DashboardMenuItem__Click);
      this.tsDashboard_ManageSnapshot.Name = "tsDashboard_ManageSnapshot";
      this.tsDashboard_ManageSnapshot.Size = new Size(315, 30);
      this.tsDashboard_ManageSnapshot.Text = "Manage &Snapshots";
      this.tsDashboard_ManageSnapshot.Click += new EventHandler(this.DashboardMenuItem__Click);
      this.homeMenu.ImageScalingSize = new Size(20, 20);
      this.homeMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.homeMenuItem
      });
      this.homeMenu.Location = new Point(0, 0);
      this.homeMenu.Name = "homeMenu";
      this.homeMenu.Size = new Size(792, 24);
      this.homeMenu.TabIndex = 3;
      this.homeMenuItem.Name = "homeMenuItem";
      this.homeMenuItem.Size = new Size(73, 20);
      this.homeMenuItem.Text = "Ho&me";
      this.epassMenu.ImageScalingSize = new Size(20, 20);
      this.epassMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.epassMenuItem
      });
      this.epassMenu.Location = new Point(0, 0);
      this.epassMenu.Name = "epassMenu";
      this.epassMenu.Size = new Size(792, 24);
      this.epassMenu.TabIndex = 4;
      this.epassMenuItem.Name = "epassMenuItem";
      this.epassMenuItem.Size = new Size(129, 20);
      this.epassMenuItem.Text = "&Services View";
      this.securityTradeListMenu.ImageScalingSize = new Size(20, 20);
      this.securityTradeListMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.securityTradeListItems
      });
      this.securityTradeListMenu.Location = new Point(0, 0);
      this.securityTradeListMenu.Name = "securityTradeListMenu";
      this.securityTradeListMenu.Size = new Size(792, 24);
      this.securityTradeListMenu.TabIndex = 0;
      this.securityTradeListItems.DropDownItems.AddRange(new ToolStripItem[8]
      {
        (ToolStripItem) this.newSecurityTradeToolStripMenuItem,
        (ToolStripItem) this.editSecurityTradeToolStripMenuItem,
        (ToolStripItem) this.duplicateSecurityTradeToolStripMenuItem,
        (ToolStripItem) this.deleteSecurityTradeToolStripMenuItem,
        (ToolStripItem) this.exportSecurityTradeToolStripMenuItem,
        (ToolStripItem) this.printSecurityTradeToolStripMenuItem,
        (ToolStripItem) this.lockUnlockSecurityTradeToolStripMenuItem,
        (ToolStripItem) this.archiveSecurityTradeToolStripMenuItem
      });
      this.securityTradeListItems.Name = "securityTradeListItems";
      this.securityTradeListItems.Size = new Size(141, 20);
      this.securityTradeListItems.Text = "&Security Trades";
      this.securityTradeListItems.DropDownOpening += new EventHandler(this.onTradeMenuOpening);
      this.newSecurityTradeToolStripMenuItem.Image = (Image) Resources.new_file;
      this.newSecurityTradeToolStripMenuItem.Name = "newSecurityTradeToolStripMenuItem";
      this.newSecurityTradeToolStripMenuItem.ShortcutKeys = Keys.N | Keys.Control;
      this.newSecurityTradeToolStripMenuItem.Size = new Size(314, 30);
      this.newSecurityTradeToolStripMenuItem.Tag = (object) "STR_New";
      this.newSecurityTradeToolStripMenuItem.Text = "&New Trade...";
      this.newSecurityTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.editSecurityTradeToolStripMenuItem.Image = (Image) Resources.edit;
      this.editSecurityTradeToolStripMenuItem.Name = "editSecurityTradeToolStripMenuItem";
      this.editSecurityTradeToolStripMenuItem.Size = new Size(314, 30);
      this.editSecurityTradeToolStripMenuItem.Tag = (object) "STR_Edit";
      this.editSecurityTradeToolStripMenuItem.Text = "&Edit Trade...";
      this.editSecurityTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.duplicateSecurityTradeToolStripMenuItem.Image = (Image) Resources.duplicate;
      this.duplicateSecurityTradeToolStripMenuItem.Name = "duplicateSecurityTradeToolStripMenuItem";
      this.duplicateSecurityTradeToolStripMenuItem.Size = new Size(314, 30);
      this.duplicateSecurityTradeToolStripMenuItem.Tag = (object) "STR_Duplicate";
      this.duplicateSecurityTradeToolStripMenuItem.Text = "D&uplicate Trade";
      this.duplicateSecurityTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.deleteSecurityTradeToolStripMenuItem.Image = (Image) Resources.delete;
      this.deleteSecurityTradeToolStripMenuItem.Name = "deleteSecurityTradeToolStripMenuItem";
      this.deleteSecurityTradeToolStripMenuItem.ShortcutKeys = Keys.D | Keys.Alt;
      this.deleteSecurityTradeToolStripMenuItem.Size = new Size(314, 30);
      this.deleteSecurityTradeToolStripMenuItem.Tag = (object) "STR_Delete";
      this.deleteSecurityTradeToolStripMenuItem.Text = "&Delete Trade...";
      this.deleteSecurityTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.exportSecurityTradeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.selectSecurityTradesOnlyToolStripMenuItem,
        (ToolStripItem) this.allSecurityTradesonPageToolStripMenuItem
      });
      this.exportSecurityTradeToolStripMenuItem.Image = (Image) Resources.excel;
      this.exportSecurityTradeToolStripMenuItem.Name = "exportSecurityTradeToolStripMenuItem";
      this.exportSecurityTradeToolStripMenuItem.ShortcutKeys = Keys.E | Keys.Alt;
      this.exportSecurityTradeToolStripMenuItem.Size = new Size(314, 30);
      this.exportSecurityTradeToolStripMenuItem.Tag = (object) "STR_Export";
      this.exportSecurityTradeToolStripMenuItem.Text = "&Export Trade to Excel";
      this.exportSecurityTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.selectSecurityTradesOnlyToolStripMenuItem.Name = "selectSecurityTradesOnlyToolStripMenuItem";
      this.selectSecurityTradesOnlyToolStripMenuItem.Size = new Size(285, 30);
      this.selectSecurityTradesOnlyToolStripMenuItem.Tag = (object) "STR_ExportSelected";
      this.selectSecurityTradesOnlyToolStripMenuItem.Text = "&Selected Trades Only...";
      this.selectSecurityTradesOnlyToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.allSecurityTradesonPageToolStripMenuItem.Name = "allSecurityTradesonPageToolStripMenuItem";
      this.allSecurityTradesonPageToolStripMenuItem.Size = new Size(285, 30);
      this.allSecurityTradesonPageToolStripMenuItem.Tag = (object) "STR_ExportAll";
      this.allSecurityTradesonPageToolStripMenuItem.Text = "&All Trades on All Pages...";
      this.allSecurityTradesonPageToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.printSecurityTradeToolStripMenuItem.Image = (Image) Resources.print;
      this.printSecurityTradeToolStripMenuItem.Name = "printSecurityTradeToolStripMenuItem";
      this.printSecurityTradeToolStripMenuItem.ShortcutKeys = Keys.P | Keys.Alt;
      this.printSecurityTradeToolStripMenuItem.Size = new Size(314, 30);
      this.printSecurityTradeToolStripMenuItem.Tag = (object) "STR_Print";
      this.printSecurityTradeToolStripMenuItem.Text = "&Print Trade...";
      this.printSecurityTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.lockUnlockSecurityTradeToolStripMenuItem.Name = "lockUnlockSecurityTradeToolStripMenuItem";
      this.lockUnlockSecurityTradeToolStripMenuItem.Size = new Size(314, 30);
      this.lockUnlockSecurityTradeToolStripMenuItem.Tag = (object) "STR_Lock";
      this.lockUnlockSecurityTradeToolStripMenuItem.Text = "&Lock/Unlock Trade";
      this.lockUnlockSecurityTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.archiveSecurityTradeToolStripMenuItem.Name = "archiveSecurityTradeToolStripMenuItem";
      this.archiveSecurityTradeToolStripMenuItem.Size = new Size(314, 30);
      this.archiveSecurityTradeToolStripMenuItem.Tag = (object) "STR_Archive";
      this.archiveSecurityTradeToolStripMenuItem.Text = "&Archive Trade";
      this.archiveSecurityTradeToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.securityTradeEditorMenu.ImageScalingSize = new Size(20, 20);
      this.securityTradeEditorMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.securityTradeEditorItems
      });
      this.securityTradeEditorMenu.Location = new Point(0, 0);
      this.securityTradeEditorMenu.Name = "securityTradeEditorMenu";
      this.securityTradeEditorMenu.Size = new Size(792, 24);
      this.securityTradeEditorMenu.TabIndex = 0;
      this.securityTradeEditorItems.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.saveSecurityTradeMenuItem,
        (ToolStripItem) this.returnToSecurityTradeListItem
      });
      this.securityTradeEditorItems.Name = "securityTradeEditorItems";
      this.securityTradeEditorItems.Size = new Size(74, 20);
      this.securityTradeEditorItems.Text = "&Trades";
      this.saveSecurityTradeMenuItem.Name = "saveSecurityTradeMenuItem";
      this.saveSecurityTradeMenuItem.ShortcutKeys = Keys.S | Keys.Control;
      this.saveSecurityTradeMenuItem.Size = new Size(241, 30);
      this.saveSecurityTradeMenuItem.Tag = (object) "STE_SaveTrade";
      this.saveSecurityTradeMenuItem.Text = "&Save Trade";
      this.saveSecurityTradeMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.returnToSecurityTradeListItem.Name = "returnToSecurityTradeListItem";
      this.returnToSecurityTradeListItem.Size = new Size(241, 30);
      this.returnToSecurityTradeListItem.Tag = (object) "STE_ExitTrade";
      this.returnToSecurityTradeListItem.Text = "E&xit Trade";
      this.returnToSecurityTradeListItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.correspondentMasterContractListMenu.ImageScalingSize = new Size(20, 20);
      this.correspondentMasterContractListMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.toolStripMenuItem5
      });
      this.correspondentMasterContractListMenu.Location = new Point(0, 24);
      this.correspondentMasterContractListMenu.Name = "correspondentMasterContractListMenu";
      this.correspondentMasterContractListMenu.Size = new Size(792, 24);
      this.correspondentMasterContractListMenu.TabIndex = 3;
      this.toolStripMenuItem5.DropDownItems.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.newCMCMenuItem,
        (ToolStripItem) this.deleteCMCMenuItem,
        (ToolStripItem) this.exportCMCMenuItem,
        (ToolStripItem) this.printCMCMenuItem,
        (ToolStripItem) this.archiveCMCMenuItem,
        (ToolStripItem) this.toolStripSeparator20,
        (ToolStripItem) this.createCMCTradeMenuItem
      });
      this.toolStripMenuItem5.Name = "toolStripMenuItem5";
      this.toolStripMenuItem5.Size = new Size(210, 20);
      this.toolStripMenuItem5.Text = "&Correspondent Masters";
      this.toolStripMenuItem5.DropDownOpening += new EventHandler(this.onTradeMenuOpening);
      this.newCMCMenuItem.Image = (Image) Resources.new_file;
      this.newCMCMenuItem.Name = "newCMCMenuItem";
      this.newCMCMenuItem.ShortcutKeys = Keys.N | Keys.Control;
      this.newCMCMenuItem.Size = new Size(565, 30);
      this.newCMCMenuItem.Tag = (object) "CMC_New";
      this.newCMCMenuItem.Text = "&New Correspondent Master Commitment";
      this.newCMCMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.deleteCMCMenuItem.Image = (Image) Resources.delete;
      this.deleteCMCMenuItem.Name = "deleteCMCMenuItem";
      this.deleteCMCMenuItem.ShortcutKeys = Keys.D | Keys.Alt;
      this.deleteCMCMenuItem.Size = new Size(565, 30);
      this.deleteCMCMenuItem.Tag = (object) "CMC_Delete";
      this.deleteCMCMenuItem.Text = "&Delete Correspondent Master Commitment";
      this.deleteCMCMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.exportCMCMenuItem.Image = (Image) Resources.excel;
      this.exportCMCMenuItem.Name = "exportCMCMenuItem";
      this.exportCMCMenuItem.ShortcutKeys = Keys.E | Keys.Control;
      this.exportCMCMenuItem.Size = new Size(565, 30);
      this.exportCMCMenuItem.Tag = (object) "CMC_Export";
      this.exportCMCMenuItem.Text = "&Export Correspondent Master Commitment to Excel";
      this.exportCMCMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.printCMCMenuItem.Image = (Image) Resources.print;
      this.printCMCMenuItem.Name = "printCMCMenuItem";
      this.printCMCMenuItem.ShortcutKeys = Keys.P | Keys.Control;
      this.printCMCMenuItem.Size = new Size(565, 30);
      this.printCMCMenuItem.Tag = (object) "CMC_Print";
      this.printCMCMenuItem.Text = "&Print Correspondent Master Commitment";
      this.printCMCMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.archiveCMCMenuItem.Name = "archiveCMCMenuItem";
      this.archiveCMCMenuItem.ShortcutKeys = Keys.R | Keys.Control;
      this.archiveCMCMenuItem.Size = new Size(565, 30);
      this.archiveCMCMenuItem.Tag = (object) "CMC_Archive";
      this.archiveCMCMenuItem.Text = "&Archive Correspondent Master Commitment";
      this.archiveCMCMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.toolStripSeparator20.Name = "toolStripSeparator20";
      this.toolStripSeparator20.Size = new Size(562, 6);
      this.createCMCTradeMenuItem.Name = "createCMCTradeMenuItem";
      this.createCMCTradeMenuItem.Size = new Size(565, 30);
      this.createCMCTradeMenuItem.Tag = (object) "CMC_NewTrade";
      this.createCMCTradeMenuItem.Text = "&Create Correspondent Trade";
      this.createCMCTradeMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.correspondentMasterContractEditorMenu.ImageScalingSize = new Size(20, 20);
      this.correspondentMasterContractEditorMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.correspondentMasterCommitmentEditorMenuItem
      });
      this.correspondentMasterContractEditorMenu.Location = new Point(0, 24);
      this.correspondentMasterContractEditorMenu.Name = "correspondentMasterContractEditorMenu";
      this.correspondentMasterContractEditorMenu.Size = new Size(792, 24);
      this.correspondentMasterContractEditorMenu.TabIndex = 3;
      this.correspondentMasterContractEditorMenu.Text = "gradientMenuStrip1";
      this.correspondentMasterCommitmentEditorMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.saveCMCMenuItem,
        (ToolStripItem) this.exitCMCMenuItem
      });
      this.correspondentMasterCommitmentEditorMenuItem.Name = "correspondentMasterCommitmentEditorMenuItem";
      this.correspondentMasterCommitmentEditorMenuItem.Size = new Size(210, 20);
      this.correspondentMasterCommitmentEditorMenuItem.Text = "&Correspondent Masters";
      this.saveCMCMenuItem.Name = "saveCMCMenuItem";
      this.saveCMCMenuItem.ShortcutKeys = Keys.S | Keys.Control;
      this.saveCMCMenuItem.Size = new Size(487, 30);
      this.saveCMCMenuItem.Tag = (object) "CMCE_SaveCorrespondentMasterCommitment";
      this.saveCMCMenuItem.Text = "&Save Correspondent Master Commitment";
      this.saveCMCMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.exitCMCMenuItem.Name = "exitCMCMenuItem";
      this.exitCMCMenuItem.Size = new Size(487, 30);
      this.exitCMCMenuItem.Tag = (object) "CMCE_ExitCorrespondentMasterCommitment";
      this.exitCMCMenuItem.Text = "E&xit Correspondent Master Commitment";
      this.exitCMCMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.correspondentTradeListMenu.ImageScalingSize = new Size(20, 20);
      this.correspondentTradeListMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.correspondentLoanTradeStripMenuItem
      });
      this.correspondentTradeListMenu.Location = new Point(0, 24);
      this.correspondentTradeListMenu.Name = "correspondentTradeListMenu";
      this.correspondentTradeListMenu.Size = new Size(792, 24);
      this.correspondentTradeListMenu.TabIndex = 4;
      this.correspondentLoanTradeStripMenuItem.DropDownItems.AddRange(new ToolStripItem[11]
      {
        (ToolStripItem) this.newLoanTradeMenuItem,
        (ToolStripItem) this.editLoanTradeMenuItem,
        (ToolStripItem) this.duplicateTradeMenuItem,
        (ToolStripItem) this.deleteTradeMenuItem,
        (ToolStripItem) this.exportLoanTradeMenuItem,
        (ToolStripItem) this.printLoanTradeMenuItem,
        (ToolStripItem) this.toolStripSeparator21,
        (ToolStripItem) this.lockUnlockLoanTradeMenuItem,
        (ToolStripItem) this.archiveLoanTradeStripMenuItem,
        (ToolStripItem) this.voidLoanTradeStripMenuItem,
        (ToolStripItem) this.updatePendingLoanMenuItem
      });
      this.correspondentLoanTradeStripMenuItem.Name = "correspondentLoanTradeStripMenuItem";
      this.correspondentLoanTradeStripMenuItem.Size = new Size(198, 20);
      this.correspondentLoanTradeStripMenuItem.Text = "&Correspondent Trades";
      this.correspondentLoanTradeStripMenuItem.DropDownOpened += new EventHandler(this.onTradeMenuOpening);
      this.newLoanTradeMenuItem.Image = (Image) Resources.new_file;
      this.newLoanTradeMenuItem.Name = "newLoanTradeMenuItem";
      this.newLoanTradeMenuItem.ShortcutKeys = Keys.N | Keys.Control;
      this.newLoanTradeMenuItem.Size = new Size(274, 30);
      this.newLoanTradeMenuItem.Tag = (object) "LT_New";
      this.newLoanTradeMenuItem.Text = "&New Trade...";
      this.newLoanTradeMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.editLoanTradeMenuItem.Image = (Image) Resources.edit;
      this.editLoanTradeMenuItem.Name = "editLoanTradeMenuItem";
      this.editLoanTradeMenuItem.Size = new Size(274, 30);
      this.editLoanTradeMenuItem.Tag = (object) "LT_Edit";
      this.editLoanTradeMenuItem.Text = "Edit Trade...";
      this.editLoanTradeMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.duplicateTradeMenuItem.Image = (Image) Resources.duplicate;
      this.duplicateTradeMenuItem.Name = "duplicateTradeMenuItem";
      this.duplicateTradeMenuItem.Size = new Size(274, 30);
      this.duplicateTradeMenuItem.Tag = (object) "LT_Duplicate";
      this.duplicateTradeMenuItem.Text = "Duplicate Trade";
      this.duplicateTradeMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.deleteTradeMenuItem.Image = (Image) Resources.delete;
      this.deleteTradeMenuItem.Name = "deleteTradeMenuItem";
      this.deleteTradeMenuItem.ShortcutKeys = Keys.D | Keys.Alt;
      this.deleteTradeMenuItem.Size = new Size(274, 30);
      this.deleteTradeMenuItem.Tag = (object) "LT_Delete";
      this.deleteTradeMenuItem.Text = "&Delete Trade";
      this.deleteTradeMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.exportLoanTradeMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.exportSelCorrespondentTradeMenuItem,
        (ToolStripItem) this.exportAllCorrespondentTradeMenuItem
      });
      this.exportLoanTradeMenuItem.Image = (Image) Resources.excel;
      this.exportLoanTradeMenuItem.Name = "exportLoanTradeMenuItem";
      this.exportLoanTradeMenuItem.Size = new Size(274, 30);
      this.exportLoanTradeMenuItem.Tag = (object) "LT_Export";
      this.exportLoanTradeMenuItem.Text = "&Export Trade to Excel";
      this.exportLoanTradeMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.exportSelCorrespondentTradeMenuItem.Name = "exportSelCorrespondentTradeMenuItem";
      this.exportSelCorrespondentTradeMenuItem.Size = new Size(285, 30);
      this.exportSelCorrespondentTradeMenuItem.Tag = (object) "LT_ExportSelected";
      this.exportSelCorrespondentTradeMenuItem.Text = "&Selected Trades Only...";
      this.exportSelCorrespondentTradeMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.exportAllCorrespondentTradeMenuItem.Name = "exportAllCorrespondentTradeMenuItem";
      this.exportAllCorrespondentTradeMenuItem.Size = new Size(285, 30);
      this.exportAllCorrespondentTradeMenuItem.Tag = (object) "LT_ExportAll";
      this.exportAllCorrespondentTradeMenuItem.Text = "&All Trades on All Pages...";
      this.exportAllCorrespondentTradeMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.printLoanTradeMenuItem.Image = (Image) Resources.print;
      this.printLoanTradeMenuItem.Name = "printLoanTradeMenuItem";
      this.printLoanTradeMenuItem.ShortcutKeys = Keys.P | Keys.Control;
      this.printLoanTradeMenuItem.Size = new Size(274, 30);
      this.printLoanTradeMenuItem.Tag = (object) "LT_Print";
      this.printLoanTradeMenuItem.Text = "&Print Trade";
      this.printLoanTradeMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.toolStripSeparator21.Name = "toolStripSeparator21";
      this.toolStripSeparator21.Size = new Size(271, 6);
      this.lockUnlockLoanTradeMenuItem.Name = "lockUnlockLoanTradeMenuItem";
      this.lockUnlockLoanTradeMenuItem.Size = new Size(274, 30);
      this.lockUnlockLoanTradeMenuItem.Tag = (object) "LT_LockUnlock";
      this.lockUnlockLoanTradeMenuItem.Text = "&Lock/Unlock Trade";
      this.lockUnlockLoanTradeMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.archiveLoanTradeStripMenuItem.Name = "archiveLoanTradeStripMenuItem";
      this.archiveLoanTradeStripMenuItem.Size = new Size(274, 30);
      this.archiveLoanTradeStripMenuItem.Tag = (object) "LT_Archive";
      this.archiveLoanTradeStripMenuItem.Text = "&Archive Trade";
      this.archiveLoanTradeStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.voidLoanTradeStripMenuItem.Name = "voidLoanTradeStripMenuItem";
      this.voidLoanTradeStripMenuItem.Size = new Size(274, 30);
      this.voidLoanTradeStripMenuItem.Tag = (object) "LT_Void";
      this.voidLoanTradeStripMenuItem.Text = "&Void";
      this.voidLoanTradeStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.updatePendingLoanMenuItem.Name = "updatePendingLoanMenuItem";
      this.updatePendingLoanMenuItem.Size = new Size(274, 30);
      this.updatePendingLoanMenuItem.Tag = (object) "LT_UpdatePendingLoan";
      this.updatePendingLoanMenuItem.Text = "Update Pending Loans";
      this.updatePendingLoanMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.correspondentTradeEditorMenu.ImageScalingSize = new Size(20, 20);
      this.correspondentTradeEditorMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.correspondentTradeMenuItem
      });
      this.correspondentTradeEditorMenu.Location = new Point(0, 24);
      this.correspondentTradeEditorMenu.Name = "correspondentTradeEditorMenu";
      this.correspondentTradeEditorMenu.Size = new Size(792, 24);
      this.correspondentTradeEditorMenu.TabIndex = 4;
      this.correspondentTradeEditorMenu.Text = "&Loan Trade";
      this.correspondentTradeMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.saveCorrespondentTradeMenuItem,
        (ToolStripItem) this.exitCorrespondentTradeMenuItem
      });
      this.correspondentTradeMenuItem.Name = "correspondentTradeMenuItem";
      this.correspondentTradeMenuItem.Size = new Size(198, 20);
      this.correspondentTradeMenuItem.Text = "&Correspondent Trades";
      this.saveCorrespondentTradeMenuItem.Name = "saveCorrespondentTradeMenuItem";
      this.saveCorrespondentTradeMenuItem.ShortcutKeys = Keys.S | Keys.Control;
      this.saveCorrespondentTradeMenuItem.Size = new Size(365, 30);
      this.saveCorrespondentTradeMenuItem.Tag = (object) "LTE_Save";
      this.saveCorrespondentTradeMenuItem.Text = "&Save Correspondent Trade";
      this.saveCorrespondentTradeMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.exitCorrespondentTradeMenuItem.Name = "exitCorrespondentTradeMenuItem";
      this.exitCorrespondentTradeMenuItem.Size = new Size(365, 30);
      this.exitCorrespondentTradeMenuItem.Tag = (object) "LTE_Exit";
      this.exitCorrespondentTradeMenuItem.Text = "E&xit Correspondent Trade";
      this.exitCorrespondentTradeMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.GSECommitmentListMenu.ImageScalingSize = new Size(20, 20);
      this.GSECommitmentListMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.GSECommitmentListItems
      });
      this.GSECommitmentListMenu.Location = new Point(0, 0);
      this.GSECommitmentListMenu.Name = "GSECommitmentListMenu";
      this.GSECommitmentListMenu.Size = new Size(792, 24);
      this.GSECommitmentListMenu.TabIndex = 0;
      this.GSECommitmentListItems.DropDownItems.AddRange(new ToolStripItem[8]
      {
        (ToolStripItem) this.newGSECommitmentToolStripMenuItem,
        (ToolStripItem) this.editGSECommitmentToolStripMenuItem,
        (ToolStripItem) this.duplicateGSECommitmentToolStripMenuItem,
        (ToolStripItem) this.deleteGSECommitmentToolStripMenuItem,
        (ToolStripItem) this.exportGSECommitmentToolStripMenuItem,
        (ToolStripItem) this.printGSECommitmentToolStripMenuItem,
        (ToolStripItem) this.GSECommitmentToolStripSeparator,
        (ToolStripItem) this.archiveGSECommitmentToolStripMenuItem
      });
      this.GSECommitmentListItems.Name = "GSECommitmentListItems";
      this.GSECommitmentListItems.Size = new Size(173, 20);
      this.GSECommitmentListItems.Text = "&GSE Commitments";
      this.GSECommitmentListItems.DropDownOpening += new EventHandler(this.onTradeMenuOpening);
      this.newGSECommitmentToolStripMenuItem.Image = (Image) Resources.new_file;
      this.newGSECommitmentToolStripMenuItem.Name = "newGSECommitmentToolStripMenuItem";
      this.newGSECommitmentToolStripMenuItem.ShortcutKeys = Keys.N | Keys.Control;
      this.newGSECommitmentToolStripMenuItem.Size = new Size(425, 30);
      this.newGSECommitmentToolStripMenuItem.Tag = (object) "GSE_New";
      this.newGSECommitmentToolStripMenuItem.Text = "&New GSE Commitment...";
      this.newGSECommitmentToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.editGSECommitmentToolStripMenuItem.Image = (Image) Resources.edit;
      this.editGSECommitmentToolStripMenuItem.Name = "editGSECommitmentToolStripMenuItem";
      this.editGSECommitmentToolStripMenuItem.Size = new Size(425, 30);
      this.editGSECommitmentToolStripMenuItem.Tag = (object) "GSE_Edit";
      this.editGSECommitmentToolStripMenuItem.Text = "&Edit GSE Commitment...";
      this.editGSECommitmentToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.duplicateGSECommitmentToolStripMenuItem.Image = (Image) Resources.duplicate;
      this.duplicateGSECommitmentToolStripMenuItem.Name = "duplicateGSECommitmentToolStripMenuItem";
      this.duplicateGSECommitmentToolStripMenuItem.Size = new Size(425, 30);
      this.duplicateGSECommitmentToolStripMenuItem.Tag = (object) "GSE_Duplicate";
      this.duplicateGSECommitmentToolStripMenuItem.Text = "&Duplicate GSE Commitment";
      this.duplicateGSECommitmentToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.deleteGSECommitmentToolStripMenuItem.Image = (Image) Resources.delete;
      this.deleteGSECommitmentToolStripMenuItem.Name = "deleteGSECommitmentToolStripMenuItem";
      this.deleteGSECommitmentToolStripMenuItem.ShortcutKeys = Keys.D | Keys.Alt;
      this.deleteGSECommitmentToolStripMenuItem.Size = new Size(425, 30);
      this.deleteGSECommitmentToolStripMenuItem.Tag = (object) "GSE_Delete";
      this.deleteGSECommitmentToolStripMenuItem.Text = "&Delete GSE Commitment...";
      this.deleteGSECommitmentToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.exportGSECommitmentToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.selectGSECommitmentsOnlyToolStripMenuItem,
        (ToolStripItem) this.allGSECommitmentsonPageToolStripMenuItem
      });
      this.exportGSECommitmentToolStripMenuItem.Image = (Image) Resources.excel;
      this.exportGSECommitmentToolStripMenuItem.Name = "exportGSECommitmentToolStripMenuItem";
      this.exportGSECommitmentToolStripMenuItem.ShortcutKeys = Keys.E | Keys.Alt;
      this.exportGSECommitmentToolStripMenuItem.Size = new Size(425, 30);
      this.exportGSECommitmentToolStripMenuItem.Tag = (object) "GSE_Export";
      this.exportGSECommitmentToolStripMenuItem.Text = "&Export GSE Commitment to Excel...";
      this.exportGSECommitmentToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.selectGSECommitmentsOnlyToolStripMenuItem.Name = "selectGSECommitmentsOnlyToolStripMenuItem";
      this.selectGSECommitmentsOnlyToolStripMenuItem.Size = new Size(376, 30);
      this.selectGSECommitmentsOnlyToolStripMenuItem.Tag = (object) "GSE_ExportSelected";
      this.selectGSECommitmentsOnlyToolStripMenuItem.Text = "&Selected GSECommitment Only...";
      this.selectGSECommitmentsOnlyToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.allGSECommitmentsonPageToolStripMenuItem.Name = "allGSECommitmentsonPageToolStripMenuItem";
      this.allGSECommitmentsonPageToolStripMenuItem.Size = new Size(376, 30);
      this.allGSECommitmentsonPageToolStripMenuItem.Tag = (object) "GSE_ExportAll";
      this.allGSECommitmentsonPageToolStripMenuItem.Text = "&All GSE Commitment on All Pages...";
      this.allGSECommitmentsonPageToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.printGSECommitmentToolStripMenuItem.Image = (Image) Resources.print;
      this.printGSECommitmentToolStripMenuItem.Name = "printGSECommitmentToolStripMenuItem";
      this.printGSECommitmentToolStripMenuItem.ShortcutKeys = Keys.P | Keys.Alt;
      this.printGSECommitmentToolStripMenuItem.Size = new Size(425, 30);
      this.printGSECommitmentToolStripMenuItem.Tag = (object) "GSE_Print";
      this.printGSECommitmentToolStripMenuItem.Text = "&Print GSE Commitment";
      this.printGSECommitmentToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.GSECommitmentToolStripSeparator.Name = "GSECommitmentToolStripSeparator";
      this.GSECommitmentToolStripSeparator.Size = new Size(422, 6);
      this.archiveGSECommitmentToolStripMenuItem.Name = "archiveGSECommitmentToolStripMenuItem";
      this.archiveGSECommitmentToolStripMenuItem.Size = new Size(425, 30);
      this.archiveGSECommitmentToolStripMenuItem.Tag = (object) "GSE_Archive";
      this.archiveGSECommitmentToolStripMenuItem.Text = "&Archive GSE Commitment";
      this.archiveGSECommitmentToolStripMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.GSECommitmentEditorMenu.ImageScalingSize = new Size(20, 20);
      this.GSECommitmentEditorMenu.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.GSECommitmentEditorMenuItems
      });
      this.GSECommitmentEditorMenu.Location = new Point(0, 24);
      this.GSECommitmentEditorMenu.Name = "GSECommitmentEditorMenu";
      this.GSECommitmentEditorMenu.Size = new Size(792, 24);
      this.GSECommitmentEditorMenu.TabIndex = 1;
      this.GSECommitmentEditorMenu.Text = "gradientMenuStrip1";
      this.GSECommitmentEditorMenuItems.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.saveGSECommitmentMenuItem,
        (ToolStripItem) this.returnToGSECommitmentListItem
      });
      this.GSECommitmentEditorMenuItems.Name = "GSECommitmentEditorMenuItems";
      this.GSECommitmentEditorMenuItems.Size = new Size(165, 20);
      this.GSECommitmentEditorMenuItems.Text = "&GSE Commitment";
      this.saveGSECommitmentMenuItem.Name = "saveGSECommitmentMenuItem";
      this.saveGSECommitmentMenuItem.ShortcutKeys = Keys.S | Keys.Control;
      this.saveGSECommitmentMenuItem.Size = new Size(340, 30);
      this.saveGSECommitmentMenuItem.Tag = (object) "GSEE_SaveTrade";
      this.saveGSECommitmentMenuItem.Text = "&Save GSE Commtiment";
      this.saveGSECommitmentMenuItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.returnToGSECommitmentListItem.Name = "returnToGSECommitmentListItem";
      this.returnToGSECommitmentListItem.Size = new Size(340, 30);
      this.returnToGSECommitmentListItem.Tag = (object) "GSEE_ExitTrade";
      this.returnToGSECommitmentListItem.Text = "E&xit GSE Commitment";
      this.returnToGSECommitmentListItem.Click += new EventHandler(this.TradesMenuItem__Click);
      this.tsMenuItemEMMessage.Name = "tsMenuItemEMMessage";
      this.tsMenuItemEMMessage.Size = new Size(115, 24);
      this.tsMenuItemEMMessage.Text = "Ellie Mae Message";
      this.tsMenuItemEMMessage.Click += new EventHandler(this.tsMenuItemEMMessage_Click);
      this.AutoScaleBaseSize = new Size(8, 19);
      this.ClientSize = new Size(792, 566);
      this.Controls.Add((Control) this.mainPanel);
      this.Controls.Add((Control) this.mainMenu);
      this.Controls.Add((Control) this.statusBar);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MainMenuStrip = (MenuStrip) this.mbsPoolListMenu;
      this.Name = nameof (MainForm);
      this.StartPosition = FormStartPosition.Manual;
      this.Text = "Encompass";
      this.Closed += new EventHandler(this.MainForm_Closed);
      this.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
      this.Resize += new EventHandler(this.MainForm_Resize);
      this.mbsPoolEditorMenu.ResumeLayout(false);
      this.mbsPoolEditorMenu.PerformLayout();
      this.mbsPoolListMenu.ResumeLayout(false);
      this.mbsPoolListMenu.PerformLayout();
      this.loanSearchMenu.ResumeLayout(false);
      this.loanSearchMenu.PerformLayout();
      this.tradeEditorMenu.ResumeLayout(false);
      this.tradeEditorMenu.PerformLayout();
      this.tradeListMenu.ResumeLayout(false);
      this.tradeListMenu.PerformLayout();
      this.masterContractMenu.ResumeLayout(false);
      this.masterContractMenu.PerformLayout();
      this.mainMenu.ResumeLayout(false);
      this.mainMenu.PerformLayout();
      this.borrowerMenu.ResumeLayout(false);
      this.borrowerMenu.PerformLayout();
      this.bizPartnerMenu.ResumeLayout(false);
      this.bizPartnerMenu.PerformLayout();
      this.calendarMenu.ResumeLayout(false);
      this.calendarMenu.PerformLayout();
      this.campaignMenu.ResumeLayout(false);
      this.campaignMenu.PerformLayout();
      this.taskMenu.ResumeLayout(false);
      this.taskMenu.PerformLayout();
      this.statusBar.ResumeLayout(false);
      this.statusBar.PerformLayout();
      this.pipelineMenu.ResumeLayout(false);
      this.pipelineMenu.PerformLayout();
      this.loanMenu.ResumeLayout(false);
      this.loanMenu.PerformLayout();
      this.contactMenu.ResumeLayout(false);
      this.contactMenu.PerformLayout();
      this.reportMenu.ResumeLayout(false);
      this.reportMenu.PerformLayout();
      this.dashboardMenu.ResumeLayout(false);
      this.dashboardMenu.PerformLayout();
      this.homeMenu.ResumeLayout(false);
      this.homeMenu.PerformLayout();
      this.epassMenu.ResumeLayout(false);
      this.epassMenu.PerformLayout();
      this.securityTradeListMenu.ResumeLayout(false);
      this.securityTradeListMenu.PerformLayout();
      this.securityTradeEditorMenu.ResumeLayout(false);
      this.securityTradeEditorMenu.PerformLayout();
      this.correspondentMasterContractListMenu.ResumeLayout(false);
      this.correspondentMasterContractListMenu.PerformLayout();
      this.correspondentMasterContractEditorMenu.ResumeLayout(false);
      this.correspondentMasterContractEditorMenu.PerformLayout();
      this.correspondentTradeListMenu.ResumeLayout(false);
      this.correspondentTradeListMenu.PerformLayout();
      this.correspondentTradeEditorMenu.ResumeLayout(false);
      this.correspondentTradeEditorMenu.PerformLayout();
      this.GSECommitmentListMenu.ResumeLayout(false);
      this.GSECommitmentListMenu.PerformLayout();
      this.GSECommitmentEditorMenu.ResumeLayout(false);
      this.GSECommitmentEditorMenu.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public ToolStripItemCollection TradingMenu_Search => this.loanSearchItems.DropDownItems;

    public ToolStripItemCollection TradingMenu_Trades => this.tradeListItems.DropDownItems;

    public ToolStripItemCollection TradingMenu_Contracts => this.masterContractItems.DropDownItems;

    public ToolStripItemCollection TradingMenu_TradeEditor => this.tradeEditorItems.DropDownItems;

    public ToolStripItemCollection TradingMenu_SecurityTrades
    {
      get => this.securityTradeListItems.DropDownItems;
    }

    public ToolStripItemCollection TradingMenu_SecurityTradeEditor
    {
      get => this.securityTradeEditorItems.DropDownItems;
    }

    public ToolStripItemCollection TradingMenu_MbsPools => this.mbsPoolListItems.DropDownItems;

    public ToolStripItemCollection TradingMenu_MbsPoolEditor
    {
      get => this.mbsPoolEditorItems.DropDownItems;
    }

    public ToolStripItemCollection TradingMenu_CorrespondentMasters
    {
      get => this.toolStripMenuItem5.DropDownItems;
    }

    public ToolStripItemCollection TradingMenu_CorrespondentMasterEditor
    {
      get => this.correspondentMasterCommitmentEditorMenuItem.DropDownItems;
    }

    public ToolStripItemCollection TradingMenu_CorrespondentTrades
    {
      get => this.correspondentLoanTradeStripMenuItem.DropDownItems;
    }

    public ToolStripItemCollection TradingMenu_CorrespondentTradeEditor
    {
      get => this.correspondentTradeMenuItem.DropDownItems;
    }

    public ToolStripItemCollection TradingMenu_GSECommitmentEditor
    {
      get => this.GSECommitmentEditorMenuItems.DropDownItems;
    }

    public ToolStripItemCollection TradingMenu_GSECommitments
    {
      get => this.GSECommitmentListItems.DropDownItems;
    }

    private void tsMenuItemTrades_MouseHover(object sender, EventArgs e)
    {
      if (Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.EnableMbsPool"]))
      {
        this.tsMenuItemTrades.DropDownItems.Add((ToolStripItem) this.tsMenuItemMbsPools);
      }
      else
      {
        if (!this.tsMenuItemTrades.DropDownItems.Contains((ToolStripItem) this.tsMenuItemMbsPools))
          return;
        this.tsMenuItemTrades.DropDownItems.Remove((ToolStripItem) this.tsMenuItemMbsPools);
      }
    }

    private void ssbUploadProgress_Click(object sender, EventArgs e)
    {
      BackgroundAttachmentDialog.ShowInstance();
    }

    private void ssbTradeLoanUpdate_Click(object sender, EventArgs e)
    {
      this.Dock = DockStyle.Fill;
      string tapeThinClientUrl = Session.SessionObjects.ConfigurationManager.GetBidTapeThinClientURL();
      string appSetting = EnConfigurationSettings.AppSettings["ThinClientBidTape.Url"];
      string str = (string.IsNullOrEmpty(appSetting) ? tapeThinClientUrl : appSetting) + "/tradeUpdateQueue";
      SSFContext context = SSFContext.Create(Session.LoanDataMgr, SSFHostType.Network, new SSFGuest()
      {
        uri = str,
        scope = "sec",
        clientId = "04gkefdw"
      });
      if (context == null)
        return;
      context.parameters = new Dictionary<string, object>()
      {
        {
          "oapiBaseUrl",
          (object) Session.StartupInfo.OAPIGatewayBaseUri
        }
      };
      using (SSFDialog ssfDialog = new SSFDialog(context))
      {
        ssfDialog.Text = "Trade Update Queue";
        ssfDialog.Height = Convert.ToInt32((double) this.Height * 0.9);
        ssfDialog.Width = Convert.ToInt32((double) this.Width * 0.9);
        ssfDialog.ShowDialog((IWin32Window) Session.MainForm);
      }
    }

    public static void main(string[] args)
    {
      Application.ThreadException += new ThreadExceptionEventHandler(new MainForm.CustomExceptionHandler().OnThreadException);
      try
      {
        MainForm.ApplicationArgs = args;
        WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
        Tracing.Log(MainForm.sw, nameof (MainForm), TraceLevel.Info, "Starting IPC listener");
        IpcListener.StartListening();
        MainForm.applyWordConverterFix();
        if (MainForm.ArgumentExists("-dataservices"))
        {
          string argumentParameter1 = MainForm.GetArgumentParameter("-u");
          string argumentParameter2 = MainForm.GetArgumentParameter("-p");
          string argumentParameter3 = MainForm.GetArgumentParameter("-s");
          string argumentParameter4 = MainForm.GetArgumentParameter("-f");
          bool networkMode = !(argumentParameter3 == "");
          if (argumentParameter1 == "" || argumentParameter2 == "" || argumentParameter4 == "")
            Application.Run((Form) new DataServicesConfig());
          else if (!DataServicesConfig.Execute(argumentParameter1, argumentParameter2, networkMode, argumentParameter3, argumentParameter4))
          {
            Application.Run((Form) new DataServicesConfig(argumentParameter1, argumentParameter2, argumentParameter4, networkMode, argumentParameter3));
          }
          else
          {
            int num = (int) Utils.Dialog((IWin32Window) null, "Your request has been executed successfully", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
        }
        else
        {
          EnConfigurationSettings.ApplyInstanceFromCommandLine(MainForm.ApplicationArgs);
          Tracing.Init(SystemSettings.LogDir + "\\" + Environment.UserName + "\\Encompass");
          PerformancePublisher.Start();
          PerformancePublisherDOCS.Start();
          if (!AssemblyResolver.IsSmartClient)
          {
            try
            {
              MainForm.setAmyuni();
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Error setting Amyuni Creator: " + ex.Message, "Encompass");
            }
          }
          else
          {
            string str = (SmartClientUtils.GetSmartClientAttribute("AmyuniCreator") ?? "").Trim();
            if (str == "4.0.0.8" || str == "4.0.0.9")
              Amyuni.DotNetCreatorVersion = "4.0";
          }
          Task.Run((Action) (() => StandardFields.Initialize()));
          Task.Run((Action) (() => VirtualFields.Initialize()));
          if (!WebLoginUtil.IsChromiumForWebLoginEnabled)
            BrowserEngine.StartEngineAsync();
          else
            BrowserEngine.StartBinaryDownload();
          EMMessage.ShowEMMessage((string) null);
          MainForm instance = MainForm.Instance;
          instance.Hide();
          EncompassApplication.Attach();
          DateTime dateTime = DateTime.Now;
          DialogResult dialogResult = DialogResult.None;
          string argumentParameter5 = MainForm.GetArgumentParameter("-u");
          string argumentParameter6 = MainForm.GetArgumentParameter("-p");
          string server = MainForm.GetArgumentParameter("-s");
          string empty = string.Empty;
          string instanceId;
          if (string.IsNullOrWhiteSpace(server))
          {
            server = LoginUtil.DefaultEncompassServerUrl;
            instanceId = LoginUtil.DefaultInstanceID;
          }
          else
            instanceId = server.Substring(server.IndexOf('$') + 1);
          bool donotLockServer = MainForm.ArgumentExists("-us");
          using (Form loginForm = LoginFormFactory.GetLoginForm(argumentParameter5, argumentParameter6, server, instanceId, donotLockServer))
            dialogResult = !(loginForm is LoginScreen) ? loginForm.ShowDialog() : ((LoginScreen) loginForm).Login();
          if (dialogResult == DialogResult.OK)
          {
            PerformanceMeter performanceMeter = PerformanceMeter.Get("Encompass.Login");
            if (MainForm.applyHotfixes())
            {
              Session.End();
              return;
            }
            if (EnConfigurationSettings.GlobalSettings.Debug && !EnConfigurationSettings.GlobalSettings.DebugDoNotAskIndicator && new DebugModeConfirmationDialog().ShowDialog() == DialogResult.Yes)
              MainForm.Debugmode = true;
            DDMDiagnosticSession.DiagnosticsMode = DDMDiagnosticsMode.Disabled;
            JedHelp.Edition = Session.EncompassEdition;
            instance.Show();
            performanceMeter.AddCheckpoint("Show Main Screen", 5917, nameof (main), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
            TempFolderCleanUp.RegisterTempFolder(SystemSettings.IpcPortName);
            instance.afterLogin();
            instance.Activate();
            performanceMeter.AddCheckpoint("Focus on Main Screen", 5925, nameof (main), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
            performanceMeter.Stop();
            Elli.Metrics.Client.MetricsFactory.Initiate(MainForm.GetCustomer(), MainForm.GetInstance(), MainForm.GetUserId(), MainForm.GetSignalFxAPIToken(), MainForm.GetSmartClientVersion(), MainForm.GetSignalFxTimeSpan(), MainForm.GetSignalFxEnabled());
            DateTime loginStartTime = LoginScreen.GetLoginStartTime();
            if (loginStartTime != DateTime.MinValue)
              dateTime = loginStartTime;
            long int64 = Convert.ToInt64((DateTime.Now - dateTime).TotalMilliseconds);
            Elli.Metrics.Client.MetricsFactory.RecordIncrementalTimerSample("LoginIncTimer", int64, (SFxTag) new SFxUiTag());
            TimeSpan duration = performanceMeter.Duration;
            if (duration.TotalMilliseconds < (double) (10L * int64))
            {
              duration = performanceMeter.Duration;
              Elli.Metrics.Client.MetricsFactory.RecordIncrementalTimerSample("EMA_Encompass_Login", Convert.ToInt64(duration.TotalMilliseconds), (SFxTag) new SFxUiTag());
            }
            Elli.Metrics.Client.MetricsFactory.IncrementCounter("LogonIncCounter", (SFxTag) new SFxUiTag());
            if (Session.UserInfo.Email == "" || Session.UserInfo.FirstName == "" || Session.UserInfo.LastName == "")
            {
              UpdateEmailDialog updateEmailDialog = new UpdateEmailDialog(Session.UserInfo.FirstName, Session.UserInfo.LastName, Session.UserInfo.Email);
              if (updateEmailDialog.ShowDialog((IWin32Window) instance) == DialogResult.OK)
              {
                Session.UserInfo.FirstName = updateEmailDialog.FirstName;
                Session.UserInfo.LastName = updateEmailDialog.LastName;
                Session.UserInfo.Email = updateEmailDialog.UserEmail;
                Session.User.UpdatePersonalInfo(Session.UserInfo.FirstName, Session.UserInfo.LastName, Session.UserInfo.Email, Session.UserInfo.Phone, Session.UserInfo.CellPhone, Session.UserInfo.Fax);
              }
            }
            MainForm.bypassProxy();
            if (MainForm.Instance.IsPipelineTabDefault())
            {
              string authId = HomePageControl.AuthId;
              if (authId != null && authId != "" && MainForm.showWelcomePage(authId))
              {
                string welcomePage = MainForm.getWelcomePage(authId);
                if (welcomePage != "")
                  MainForm.Instance.displayWelcomePage(welcomePage, authId);
              }
            }
            if (MainForm.enableServiceUrlsDebugLog)
              Tracing.Log(true, TraceLevel.Info.ToString(), nameof (MainForm), "ServiceUrls: " + JsonConvert.SerializeObject((object) Session.StartupInfo.ServiceUrls));
            Session.NotifyApplicationReady();
            Application.Run((Form) instance);
          }
        }
        Session.End();
      }
      catch (OperationException ex)
      {
        Elli.Metrics.Client.MetricsFactory.IncrementCounter("LoanErrorIncCounter", new SFxTag(Elli.Metrics.Constants.ErrorType, "GenuineChannels Exceptions"), (SFxTag) new SFxUiTag());
        int num = (int) Utils.Dialog((IWin32Window) null, "The connection with Encompass Server has been broken. Encompass will now exit and your unsaved data will be lost.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (SocketException ex)
      {
        Elli.Metrics.Client.MetricsFactory.IncrementCounter("LoanErrorIncCounter", new SFxTag(Elli.Metrics.Constants.ErrorType, "Socket Exceptions"), (SFxTag) new SFxUiTag());
        int num = (int) Utils.Dialog((IWin32Window) null, "A socket exception occurred: " + ex.Message + ".  Encompass will exit and your unsaved data will be lost.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (Exception ex)
      {
        MainForm.writeClientExceptionToServer(ex);
        Elli.Metrics.Client.MetricsFactory.IncrementCounter("LoanErrorIncCounter", new SFxTag(Elli.Metrics.Constants.ErrorType, "Unhandled Exceptions"), (SFxTag) new SFxUiTag(), new SFxTag("ExceptionType", ex.GetType().ToString()));
        Thread.Sleep(5000);
        int num = (int) MessageBox.Show((IWin32Window) null, "An unhandled exception occurred: " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace + "\n\nEncompass will exit and your unsaved data will be lost.", "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Thread.Sleep(5000);
      }
      finally
      {
        try
        {
          IpcListener.UnregisterServerChannel();
        }
        catch (Exception ex)
        {
          Tracing.Log(MainForm.sw, nameof (MainForm), TraceLevel.Error, "Error unregistering IPC server channel: " + ex.Message);
        }
        WebLoginUtil.StopBrowserEngine(nameof (MainForm));
        try
        {
          Tracing.Close();
          MainForm.CoFreeUnusedLibraries();
        }
        catch (Exception ex)
        {
        }
      }
    }

    private static bool enableServiceUrlsDebugLog
    {
      get
      {
        return RegistryUtil.GetRegistryValue(Registry.LocalMachine, "Software\\Ellie Mae\\Encompass", "EnableServiceUrlsDebugLog") != null && (string) RegistryUtil.GetRegistryValue(Registry.LocalMachine, "Software\\Ellie Mae\\Encompass", "EnableServiceUrlsDebugLog") == "1" || EnConfigurationSettings.GlobalSettings.Debug;
      }
    }

    private void displayWelcomePage(string welcomePage, string authId)
    {
      try
      {
        using (WelcomePageForm welcomePageForm = new WelcomePageForm(welcomePage, Session.SessionObjects?.StartupInfo?.ServiceUrls?.DataServicesUrl, authId))
        {
          int num = (int) welcomePageForm.ShowDialog((IWin32Window) this);
        }
      }
      catch (Exception ex)
      {
      }
    }

    private static bool showWelcomePage(string authId)
    {
      try
      {
        string moduleList = HomePageService.getModuleList(Session.CompanyInfo.ClientID, Session.SessionObjects?.StartupInfo.ServiceUrls?.DataServicesUrl, Session.UserID, Session.UserInfo.EncompassVersion, authId);
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(moduleList);
        XmlElement xmlElement = (XmlElement) xmlDocument.SelectSingleNode("EMHOMEPAGEWS/MODULES/MODULE/PREFERENCE[@Name='ShowWelcome']");
        return xmlElement == null || !(xmlElement.GetAttribute("Value") == "N");
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    private static string getWelcomePage(string authId)
    {
      string welcomePage1 = "";
      try
      {
        string welcomePage2 = HomePageService.getWelcomePage(Session.CompanyInfo.ClientID, Session.SessionObjects?.StartupInfo.ServiceUrls?.DataServicesUrl, Session.UserID, Session.UserInfo.EncompassVersion, authId);
        if (MainForm.validateWelcomePageResponse(welcomePage2))
        {
          int num = welcomePage2.IndexOf("<![CDATA[");
          string str = welcomePage2.Substring(num + 9);
          int startIndex = welcomePage2.IndexOf("]]>");
          welcomePage1 = "<html>" + str.Replace(welcomePage2.Substring(startIndex), "") + "</html>";
        }
        return welcomePage1;
      }
      catch (Exception ex)
      {
        return welcomePage1;
      }
    }

    private static bool validateWelcomePageResponse(string welcomePageXml)
    {
      return !welcomePageXml.ToLower().Contains("<error>");
    }

    private static string getEncompassServerUrl()
    {
      string str = string.Empty;
      string name1 = "Software\\Ellie Mae\\SmartClient\\" + Application.StartupPath.Replace("\\", "/") + "\\Attributes";
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name1))
      {
        if (registryKey != null)
          str = (string) registryKey.GetValue("EncompassServerUrl");
      }
      if (str == null)
        str = "";
      string name2 = "Software\\Ellie Mae\\SmartClient\\" + Application.StartupPath.Replace("\\", "/") + "\\AppCmdLineArgs";
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name2))
      {
        if (registryKey != null)
          str = (string) registryKey.GetValue("Encompass.exe");
      }
      return str.Replace("-s ", "");
    }

    private static KeyValuePair<XmlElement, XmlElement> findBypassProxySetByEncompassElement(
      XmlDocument xmlDoc)
    {
      if (!(xmlDoc.GetElementsByTagName("appSettings")[0] is XmlElement key))
        return new KeyValuePair<XmlElement, XmlElement>((XmlElement) null, (XmlElement) null);
      foreach (XmlNode childNode in key.ChildNodes)
      {
        if (childNode is XmlElement xmlElement && xmlElement.GetAttribute("key") == "BypassProxySetByEncompass")
          return new KeyValuePair<XmlElement, XmlElement>(key, xmlElement);
      }
      return new KeyValuePair<XmlElement, XmlElement>(key, (XmlElement) null);
    }

    private static bool addSetByEncompassElement(XmlDocument xmlDoc)
    {
      KeyValuePair<XmlElement, XmlElement> encompassElement = MainForm.findBypassProxySetByEncompassElement(xmlDoc);
      if (encompassElement.Key == null || encompassElement.Value != null)
        return false;
      XmlElement element = xmlDoc.CreateElement("add");
      element.SetAttribute("key", "BypassProxySetByEncompass");
      element.SetAttribute("value", "True");
      encompassElement.Key.AppendChild((XmlNode) element);
      return true;
    }

    private static void bypassProxy()
    {
      new Thread((ThreadStart) (() =>
      {
        try
        {
          int bypassProxy = SmartClientUtils.GetBypassProxy(Session.CompanyInfo.ClientID, "Encompass.exe");
          if (bypassProxy < -1)
            return;
          XmlDocument xmlDoc = new XmlDocument();
          string str = Application.ExecutablePath + ".config";
          xmlDoc.Load(str);
          XmlNodeList elementsByTagName = xmlDoc.GetElementsByTagName("system.net");
          bool flag = false;
          if (bypassProxy > 0)
          {
            if (elementsByTagName == null || elementsByTagName.Count == 0)
            {
              XmlElement documentElement = xmlDoc.DocumentElement;
              XmlElement element1 = xmlDoc.CreateElement("system.net");
              XmlElement element2 = xmlDoc.CreateElement("defaultProxy");
              XmlElement element3 = xmlDoc.CreateElement("proxy");
              element3.SetAttribute("usesystemdefault", "False");
              element2.AppendChild((XmlNode) element3);
              element1.AppendChild((XmlNode) element2);
              documentElement.AppendChild((XmlNode) element1);
              flag = true;
            }
            if (MainForm.addSetByEncompassElement(xmlDoc))
              flag = true;
          }
          else if (bypassProxy <= 0)
          {
            if (elementsByTagName != null && elementsByTagName.Count > 0)
            {
              xmlDoc.DocumentElement.RemoveChild(elementsByTagName[0]);
              flag = true;
            }
            if (bypassProxy == 0)
            {
              if (MainForm.addSetByEncompassElement(xmlDoc))
                flag = true;
            }
            else
            {
              KeyValuePair<XmlElement, XmlElement> encompassElement = MainForm.findBypassProxySetByEncompassElement(xmlDoc);
              if (encompassElement.Key != null && encompassElement.Value != null)
              {
                encompassElement.Key.RemoveChild((XmlNode) encompassElement.Value);
                flag = true;
              }
            }
          }
          if (!flag)
            return;
          XmlWriterSettings settings = new XmlWriterSettings()
          {
            Indent = true,
            IndentChars = "  ",
            NewLineChars = "\r\n",
            NewLineHandling = NewLineHandling.Replace
          };
          using (XmlWriter w = XmlWriter.Create(str, settings))
            xmlDoc.WriteTo(w);
          System.IO.File.Copy(str, Path.Combine(Path.GetDirectoryName(str), "ClickLoanProxy.exe.config"), true);
        }
        catch (Exception ex)
        {
          try
          {
            Tracing.Log(MainForm.sw, TraceLevel.Error, nameof (MainForm), "Error updating Encompass configurtion file: " + ex.Message);
          }
          catch
          {
          }
        }
      }))
      {
        IsBackground = true
      }.Start();
    }

    private static bool applyHotfixes()
    {
      if (!MainForm.ArgumentExists("-nohotfix") && VersionControl.ApplyAvailableHotfixes())
        return true;
      if (Directory.Exists(MainForm.sys32Folder) && !AssemblyResolver.IsSmartClient)
        MainForm.copyAndRegisterSystem32Files();
      return false;
    }

    private static void copyAndRegisterSystem32Files()
    {
      try
      {
        string[] files = Directory.GetFiles(MainForm.sys32Folder);
        if (files == null || files.Length == 0)
        {
          try
          {
            Directory.Delete(MainForm.sys32Folder, true);
          }
          catch
          {
          }
        }
        else
        {
          IEncAppMgrRO remoteObject = (IEncAppMgrRO) null;
          foreach (string srcPath in files)
            MainForm.copyAndRegisterSystem32File(ref remoteObject, srcPath, true);
          try
          {
            Directory.Delete(MainForm.sys32Folder, true);
          }
          catch
          {
          }
        }
      }
      catch
      {
      }
    }

    private static void copyAndRegisterSystem32File(
      ref IEncAppMgrRO remoteObject,
      string srcPath,
      bool copyOnly)
    {
      string path2 = (string) null;
      try
      {
        path2 = Path.GetFileName(srcPath);
        string str = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), path2);
        FileVersionInfo fileVersionInfo1 = (FileVersionInfo) null;
        FileVersionInfo fileVersionInfo2 = (FileVersionInfo) null;
        if (srcPath != null && System.IO.File.Exists(srcPath))
          fileVersionInfo1 = FileVersionInfo.GetVersionInfo(srcPath);
        if (System.IO.File.Exists(str))
          fileVersionInfo2 = FileVersionInfo.GetVersionInfo(str);
        if (fileVersionInfo1 == null || fileVersionInfo2 != null && !(fileVersionInfo1.FileVersion != fileVersionInfo2.FileVersion))
          return;
        if (remoteObject == null)
          remoteObject = (IEncAppMgrRO) Activator.GetObject(typeof (IEncAppMgrRO), "ipc://EncAppMgr/EncAppMgrRO.rem");
        if (copyOnly)
          remoteObject.FileCopy(srcPath, false);
        else
          remoteObject.Regsvr32(srcPath, false);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error copying/registering " + path2 + ". Contact ICE Mortgage Technology Customer Support at 800-777-1718 for assistance.\r\n\r\n" + ex.Message);
      }
    }

    private static void applyWordConverterFix()
    {
      try
      {
        using (RegistryKey subKey = Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Applets\\Wordpad"))
          subKey.SetValue("AllowConversion", (object) 1);
      }
      catch
      {
      }
    }

    private static void setAmyuni()
    {
      string str1 = (string) null;
      string str2 = (string) null;
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass"))
      {
        str1 = registryKey != null ? (string) registryKey.GetValue("AmyuniConverter") : throw new Exception("Encompass registry key 'Software\\Ellie Mae\\Encompass' not found");
        str2 = (string) registryKey.GetValue("AmyuniCreator");
      }
      string startupPath = Application.StartupPath;
      string str3 = (string) null;
      string str4 = (str1 ?? "").Trim();
      if (str4 != "")
      {
        string path2 = "Interop.CDIntfEx.dll";
        FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(startupPath, path2));
        if (str4.StartsWith("4"))
        {
          if (versionInfo.FileVersion != "4.0.0.0")
            str3 = "AmyuniConverter4.0.0.0";
        }
        else if (versionInfo.FileVersion != "2.1.0.0")
          str3 = "AmyuniConverter2.1.0.0";
        if (str3 != null)
          System.IO.File.Copy(Path.Combine(startupPath, str3 + "\\" + path2), Path.Combine(startupPath, path2), true);
      }
      string str5 = (str2 ?? "").Trim();
      if (str5 == "")
        return;
      string path2_1 = "acpdfcrext.dll";
      string path2_2 = "PDFCreactiveX.dll";
      string path2_3 = "AxInterop.ACPDFCREACTIVEX.dll";
      string path2_4 = "Interop.ACPDFCREACTIVEX.dll";
      string str6 = (string) null;
      FileVersionInfo versionInfo1 = FileVersionInfo.GetVersionInfo(Path.Combine(startupPath, path2_4));
      if (str5.StartsWith("1.5"))
      {
        if (versionInfo1.FileVersion != "1.5.0.0")
          str6 = "AmyuniCreator1.5.0.7";
      }
      else if (str5.StartsWith("3.0"))
      {
        if (versionInfo1.FileVersion != "3.0.0.0")
          str6 = "AmyuniCreator3.0.3.1";
        FileVersionInfo versionInfo2 = FileVersionInfo.GetVersionInfo(Path.Combine(startupPath, path2_2));
        string str7 = versionInfo2.FileMajorPart.ToString() + "." + (object) versionInfo2.FileMinorPart + "." + (object) versionInfo2.FileBuildPart + "." + (object) versionInfo2.FilePrivatePart;
        if (str5 != str7)
        {
          string str8 = "AmyuniCreator" + str5;
          System.IO.File.Copy(Path.Combine(startupPath, str8 + "\\" + path2_1), Path.Combine(startupPath, path2_1), true);
          System.IO.File.Copy(Path.Combine(startupPath, str8 + "\\" + path2_2), Path.Combine(startupPath, path2_2), true);
        }
      }
      else
      {
        if (!str5.StartsWith("4.0"))
          throw new Exception("Unknown Amyuni Creator version " + str5);
        if (str5 == "4.0.0.8" || str5 == "4.0.0.9")
          Amyuni.DotNetCreatorVersion = "4.0";
        else if (versionInfo1.FileVersion != "4.0.0.0")
          str6 = "AmyuniCreator4.0.0.2";
      }
      if (str6 == null)
        return;
      System.IO.File.Copy(Path.Combine(startupPath, str6 + "\\" + path2_3), Path.Combine(startupPath, path2_3), true);
      System.IO.File.Copy(Path.Combine(startupPath, str6 + "\\" + path2_4), Path.Combine(startupPath, path2_4), true);
    }

    private static bool runVersionControlAdmin()
    {
      if (!Directory.Exists(MainForm.sys32Folder))
        return false;
      bool flag = false;
      string text;
      if (Environment.OSVersion.Version >= new System.Version("6.0.0.0"))
      {
        flag = true;
        text = "Encompass has received a background update and must be re-launched using a Windows account with administrator privileges for the changes to take effect.  Alternatively, you can continue using Encompass now and continue with the update later.\r\n\r\nDo you want to continue using Encompass now?";
      }
      else
        text = "Encompass has received a background update and must be re-launched using a Windows account with administrator privileges for the changes to take effect.  Do you want to continue using Encompass now?\r\n\r\nClick \"No\" to close Encompass.  When you reopen Encompass you will be prompted to choose a user to run Encompass to complete the update.  Select \"The following user\" and enter login information for a user with administrator rights.\r\n\r\nClick \"Yes\" to use Encompass now and continue with the update later.";
      if (!flag)
      {
        try
        {
          string[] files = Directory.GetFiles(MainForm.sys32Folder);
          if (files == null || files.Length == 0)
          {
            try
            {
              Directory.Delete(MainForm.sys32Folder, true);
            }
            catch
            {
            }
            return false;
          }
          foreach (string str in files)
          {
            Path.Combine(Environment.SystemDirectory, Path.GetFileName(str));
            System.IO.File.Copy(str, Path.Combine(Environment.SystemDirectory, Path.GetFileName(str)), true);
          }
          try
          {
            Directory.Delete(MainForm.sys32Folder, true);
          }
          catch
          {
          }
          return false;
        }
        catch
        {
        }
      }
      try
      {
        if (AssemblyResolver.IsSmartClient)
          AssemblyResolver.DownloadAndCacheResourceFile("EncompassVCA.exe", EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error downloading EncompassVCA.exe.\r\n" + ex.Message, "Encompass Hot Update");
        return false;
      }
      try
      {
        Process process = Process.Start(new ProcessStartInfo()
        {
          FileName = Path.Combine(Application.StartupPath, "EncompassVCA.exe"),
          UseShellExecute = true,
          CreateNoWindow = true,
          WindowStyle = ProcessWindowStyle.Hidden,
          Verb = "runas"
        });
        process.WaitForExit();
        if (process.ExitCode != 0)
        {
          if (MessageBox.Show(text, "Encompass Hot Update", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            return true;
        }
      }
      catch
      {
        if (MessageBox.Show(text, "Encompass Hot Update", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
          return true;
      }
      return false;
    }

    private void afterLogin()
    {
      PerformanceMeter pmeter = PerformanceMeter.Get("Encompass.Login");
      Tracing.Log(MainForm.sw, nameof (MainForm), TraceLevel.Info, "Creating MainScreen instance...");
      this.mainScreen = MainScreen.Instance;
      LoanDataMgr.ISave = (ISaveLoan) MainScreen.Instance;
      pmeter.AddCheckpoint("Creating MainScreen instance", 6698, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
      Tracing.Log(MainForm.sw, nameof (MainForm), TraceLevel.Info, "MainScreen instance created. Adding screen to control list...");
      this.mainScreen.TopLevel = false;
      this.mainScreen.Parent = (Control) this.mainPanel;
      this.mainScreen.Dock = DockStyle.Fill;
      this.mainScreen.Visible = true;
      this.mainPanel.Controls.Add((Control) this.mainScreen);
      this.epassMenuItem.DropDown = this.mainScreen.Epass.Menu;
      this.homeMenuItem.DropDown = this.mainScreen.HomePage.Menu;
      pmeter.AddCheckpoint("Add MainScreen instance to control list", 6711, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
      Tracing.Log(MainForm.sw, nameof (MainForm), TraceLevel.Info, "Registering application services...");
      this.mainScreen.RegisterService((object) this, typeof (IStatusDisplay));
      eFolderManager.RegisterService();
      BackgroundAttachmentDialog.CreateInstance(Session.ISession, Session.StartupInfo);
      TradeUpdateLoansDialog.CreateInstance(Session.ISession);
      pmeter.AddCheckpoint("Registering application services", 6725, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
      using (Tracing.StartTimer(MainForm.sw, nameof (MainForm), TraceLevel.Info, "Performing MainScreen initialization"))
        this.mainScreen.InitContents();
      pmeter.AddCheckpoint("Performing MainScreen initialization", 6731, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
      if (!MainScreen.IMEnabled)
        this.tsMenuItemEncompass.DropDownItems.Remove((ToolStripItem) this.tsMenuItemIM);
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (Session.SessionObjects.StartupInfo.FastLoanLoad)
      {
        using (PerformanceMeter submeter = new PerformanceMeter("Encompass.Login.AfterLoginCaching", "Populate After Login Caches", false, 6741, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs"))
        {
          Session.SessionObjects.CacheBusinessCalendarTask = Task.Run((Action) (() =>
          {
            submeter.AddCheckpoint("After Login:Cache Business Calendar Started", 6746, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
            Session.SessionObjects.PopulateCalendarInCache();
            submeter.AddCheckpoint("After Login:Cache Business Calendar Completed", 6748, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
          }));
          Session.SessionObjects.CacheMileStonePermissionsTask = Task.Run((Action) (() =>
          {
            submeter.AddCheckpoint("After Login:Cache Milestone Permissions Started", 6752, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
            Session.SessionObjects.GetCachedMilestonePermissions();
            submeter.AddCheckpoint("After Login:Cache Milestone Permissions Completed", 6754, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
          }));
          InputFormsAclManager formaclMgr = (InputFormsAclManager) Session.ACL.GetAclManager(AclCategory.InputForms);
          formaclMgr.CacheInputFormPermissionCacheTask = Task.Run((Action) (() =>
          {
            submeter.AddCheckpoint("After Login:Cache Input Forms Permissions Started", 6760, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
            formaclMgr.PopulateAccessFormLookup();
            submeter.AddCheckpoint("After Login:Cache Input Forms Permissions Completed", 6762, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
          }));
          Task.Run<CompiledTriggers>((Func<CompiledTriggers>) (() => TriggerCache.GetTriggers(Session.SessionObjects)));
          Session.SessionObjects.LoadLoanConfigurationInfoTask = Task.Run<ILoanConfigurationInfo>((Func<ILoanConfigurationInfo>) (() =>
          {
            submeter.AddCheckpoint("After Login:LoanConfigurationInfo Started", 6770, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
            ILoanConfigurationInfo configurationInfo = Session.SessionObjects.LoanManager.GetLoanConfigurationInfo(new LoanConfigurationParameters());
            if (configurationInfo.UnderwritingConditionTrackingSetup == null)
              configurationInfo.UnderwritingConditionTrackingSetup = new UnderwritingConditionTrackingSetup();
            if (configurationInfo.PostClosingConditionTrackingSetup == null)
              configurationInfo.PostClosingConditionTrackingSetup = new PostClosingConditionTrackingSetup();
            if (configurationInfo.SellConditionTrackingSetup == null)
              configurationInfo.SellConditionTrackingSetup = new SellConditionTrackingSetup();
            submeter.AddCheckpoint("After Login:LoanConfigurationInfo Completed", 6780, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
            return configurationInfo;
          }));
          Session.SessionObjects.LoadLoanConfigurationInfoTask.ContinueWith((Action<Task<ILoanConfigurationInfo>>) (loadConfigurationInfoTask =>
          {
            ILoanConfigurationInfo loanConfigurationInfo = loadConfigurationInfoTask.Result;
            Task.Run((Action) (() =>
            {
              pmeter.AddCheckpoint("After Login:CustomFields Compilation Started", 6789, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
              CustomCalculationCache.GetFieldCalculators(Session.SessionObjects, loanConfigurationInfo);
              pmeter.AddCheckpoint("After Login:CustomFields Compilation Completed", 6791, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
            }));
            Task.Run((Action) (() =>
            {
              pmeter.AddCheckpoint("After Login: Attach Print Form Auto Selector Started", 6796, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
              PrintFormSelectorCache.GetFormSelectors(Session.SessionObjects, loanConfigurationInfo);
              pmeter.AddCheckpoint("After Login: Attach Print Form Auto Selector Completed", 6798, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
            }));
          }), TaskContinuationOptions.OnlyOnRanToCompletion);
          Session.SessionObjects.IsContextCreatedForFastLoanLoad = true;
        }
      }
      using (PerformanceMeter submeter = new PerformanceMeter("Encompass.Login.AfterLoginCaching", "Populate After Login Caches", false, 6869, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs"))
      {
        Session.SessionObjects.CacheWorkflowTaskGroupTemplatesTask = Task.Run((Action) (() =>
        {
          submeter.AddCheckpoint("After Login:Cache Workflow Task Group Templates Started", 6873, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
          Session.SessionObjects.PopulateWorkflowTaskGroupTemplatesInCache(lazyLoadSystemSettingsWorkflowTaskGroupTemplates());
          submeter.AddCheckpoint("After Login:Cache Workflow Task Group Templates Completed", 6875, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
        }));
        Session.SessionObjects.CacheWorkflowTaskTemplatesTask = Task.Run((Action) (() =>
        {
          submeter.AddCheckpoint("After Login:Cache Workflow Task Templates Started", 6879, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
          Session.SessionObjects.PopulateWorkflowTaskTemplatesInCache(lazyLoadSystemSettingsWorkflowTaskTemplates());
          submeter.AddCheckpoint("After Login:Cache Workflow Task Templates Completed", 6881, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
        }));
      }
      if (!aclManager.GetUserApplicationRight(AclFeature.LoanTab_Other_ApplyLoanTemplate))
      {
        this.tsMenuItemTemplates.DropDownItems.Remove((ToolStripItem) this.tsMenuItemSelectLoanTemplate);
        this.tsMenuItemTemplates.DropDownItems.Remove((ToolStripItem) this.tsSeparator1);
      }
      if (!aclManager.GetUserApplicationRight(AclFeature.SettingsTab_Company_DiagnosticMode))
      {
        this.tsMenuItemHelp.DropDownItems.Remove((ToolStripItem) this.toolStripSeparator18);
        this.tsMenuItemHelp.DropDownItems.Remove((ToolStripItem) this.tsHelpDiagnostics);
        this.tsMenuItemHelp.DropDownItems.Remove((ToolStripItem) this.tsMenuItemJITLogger);
      }
      object componentSetting = Session.GetComponentSetting("GC");
      this.gcEnabled = componentSetting != null && (EnableDisableSetting) componentSetting == EnableDisableSetting.Enabled;
      if (this.gcEnabled)
      {
        this.gcTimer.Stop();
        this.gcTimer.Interval = MainForm.gcInterval;
        this.gcTimer.Start();
      }
      try
      {
        if (Session.IsBankerEdition())
        {
          using (Tracing.StartTimer(MainForm.sw, nameof (MainForm), TraceLevel.Info, "Initializing plugins..."))
            Session.InitializePlugins();
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(MainForm.sw, nameof (MainForm), TraceLevel.Error, "Failed to load plugins: " + ex.Message);
      }
      if (EpassLogin.LoginID != null)
      {
        using (Tracing.StartTimer(MainForm.sw, nameof (MainForm), TraceLevel.Info, "Checking Centerwise Activation..."))
          CenterwiseActivationDialog.StartActivationCheck();
      }
      string argumentParameter = MainForm.GetArgumentParameter("-l");
      if (argumentParameter != "" && this.mainScreen.PipelineScreenBrowser != null)
        this.mainScreen.OpenLoan(argumentParameter);
      if (MainScreen.IMEnabled)
      {
        Tracing.Log(MainForm.sw, nameof (MainForm), TraceLevel.Info, "Starting Instance Messaging services...");
        IMControlMessage[] storedImMessages = Session.StartupInfo.StoredIMMessages;
        if (storedImMessages != null && storedImMessages.Length != 0)
        {
          EncompassMessenger.Start((IMainScreen) this.mainScreen);
          for (int index = 0; index < storedImMessages.Length; ++index)
          {
            string fromUser = storedImMessages[index].FromUser;
            string text = storedImMessages[index].Text;
            switch (storedImMessages[index].MsgType)
            {
              case IMMessage.MessageType.RequestAddToList:
                AddContactRequest addContactRequest = new AddContactRequest(fromUser, text);
                addContactRequest.TopMost = true;
                int num1 = (int) addContactRequest.ShowDialog();
                addContactRequest.TopMost = false;
                break;
              case IMMessage.MessageType.DenyAddToList:
                RequestDenied requestDenied = new RequestDenied(fromUser, text);
                requestDenied.TopMost = true;
                int num2 = (int) requestDenied.ShowDialog();
                requestDenied.TopMost = false;
                break;
            }
          }
        }
      }
      if (MainScreen.CSEnabled)
      {
        Tracing.Log(MainForm.sw, nameof (MainForm), TraceLevel.Info, "Retrieving Calendar Sharing messages...");
        CSControlMessage[] storedCsMessages = Session.CalendarManager.GetStoredCSMessages(Session.UserID);
        if (storedCsMessages != null && storedCsMessages.Length != 0)
        {
          foreach (CSMessage imMsg in storedCsMessages)
            CSManagementDialog.ProcessCSMessage(imMsg);
        }
      }
      Tracing.Log(MainForm.sw, nameof (MainForm), TraceLevel.Info, "Setting up asynchronous tasks...");
      this.setupAsychTasks();
      pmeter.AddCheckpoint("Setting up asynchronous tasks", 6985, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
      Tracing.Log(MainForm.sw, nameof (MainForm), TraceLevel.Info, "Starting idle timer...");
      this.actionAfterUserIdle = (ActionAfterUserIdle) Session.StartupInfo.UnpublishedSettings[(object) "Unpublished.ActionAfterUserIdle"];
      this.StartCheckIdleTimer();
      pmeter.AddCheckpoint("Starting idle timer", 6991, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
      using (Tracing.StartTimer(MainForm.sw, nameof (MainForm), TraceLevel.Info, "Performing post-login processing"))
        LoanServiceManager.AfterLogin(Session.DefaultInstance, MainForm.GetSignalFxEnabled(), MainForm.GetSignalFxAPIToken(), MainForm.GetSignalFxTimeSpan());
      pmeter.AddCheckpoint("Performing post-login processing", 6998, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
      Tracing.Log(MainForm.sw, nameof (MainForm), TraceLevel.Info, "Starting notification and ePASS message listeners...");
      NotificationListener.Start();
      EPassMessages.StartListening();
      ConcurrentUpdateNotificationClientListener.StartListening();
      TradeLoanUpdateNotificationClientListener.StartListening();
      Session.MainForm = (Form) this;
      if (MainForm.Debugmode)
        this.tsHelpDiagnostics.Checked = false;
      pmeter.AddCheckpoint("Starting notification and ePASS message listeners", 7013, nameof (afterLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs");
      string str1 = SmartClientUtils.GetSmartClientAttribute("ScAppS") ?? SmartClientUtils.GetAttribute(Session.CompanyInfo.ClientID, "Encompass.exe", "ScAppS");
      if (!string.IsNullOrWhiteSpace(str1))
      {
        try
        {
          string[] strArray = str1.Split('|');
          string str2 = (strArray[0] ?? "").Trim();
          bool flag = false;
          if (strArray.Length > 1)
            flag = (strArray[1] ?? "").Trim() == "1";
          Process process = new Process();
          process.StartInfo.FileName = Path.Combine(Application.StartupPath, "ScApp\\ScApp.exe");
          process.StartInfo.Arguments = str2;
          process.Start();
          if (flag)
            process.WaitForExit();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Error executing ScApp: " + ex.Message);
        }
      }
      string str3 = SmartClientUtils.GetSmartClientAttribute("ScApp") ?? SmartClientUtils.GetAttribute(Session.CompanyInfo.ClientID, "Encompass.exe", "ScApp");
      if (!string.IsNullOrWhiteSpace(str3))
      {
        try
        {
          string[] strArray = str3.Split('|');
          this.scAppCmdArgs = (strArray[0] ?? "").Trim();
          this.scAppUrl = (strArray[1] ?? "").Trim();
          this.mainMenu.Items.Add((ToolStripItem) this.tsMenuItemEMMessage);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Error parsing ScApp setting: " + ex.Message);
        }
      }
      Task.Run<bool>((Func<Task<bool>>) (() => Session.Application.GetService<ILoanServices>().IsEClosingAllowed(Session.SessionObjects)));
      Tracing.Log(MainForm.sw, nameof (MainForm), TraceLevel.Info, "Completed post-login processing.");

      static Hashtable lazyLoadSystemSettingsWorkflowTaskGroupTemplates()
      {
        Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
        try
        {
          Cursor.Current = Cursors.WaitCursor;
          TaskGroupTemplate[] taskGroupTemplates = MilestoneWorkFlowTaskApiHelper.GetTaskGroupTemplates();
          if (taskGroupTemplates != null)
          {
            foreach (TaskGroupTemplate taskGroupTemplate in taskGroupTemplates)
            {
              if (!insensitiveHashtable.ContainsKey((object) taskGroupTemplate.ID))
                insensitiveHashtable.Add((object) taskGroupTemplate.ID, (object) taskGroupTemplate);
            }
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(MainForm.sw, TraceLevel.Error, nameof (MainForm), "loadSystemSettings: can't load milestone workflow tasks group templates settings. Error: " + ex.Message);
        }
        finally
        {
          Cursor.Current = Cursors.Default;
        }
        return insensitiveHashtable;
      }

      static Hashtable lazyLoadSystemSettingsWorkflowTaskTemplates()
      {
        Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
        try
        {
          Cursor.Current = Cursors.WaitCursor;
          TaskTemplate[] taskTemplates = MilestoneWorkFlowTaskApiHelper.GetTaskTemplates(true);
          if (taskTemplates != null)
          {
            foreach (TaskTemplate taskTemplate in taskTemplates)
            {
              if (!insensitiveHashtable.ContainsKey((object) taskTemplate.TypeID))
                insensitiveHashtable.Add((object) taskTemplate.TypeID, (object) taskTemplate);
            }
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(MainForm.sw, TraceLevel.Error, nameof (MainForm), "loadSystemSettings: can't load milestone workflow tasks templates settings. Error: " + ex.Message);
        }
        finally
        {
          Cursor.Current = Cursors.Default;
        }
        return insensitiveHashtable;
      }
    }

    public void StartCheckIdleTimer()
    {
      if (this.maxUserIdleTime < 0)
        this.maxUserIdleTime = (int) Session.StartupInfo.UnpublishedSettings[(object) "Unpublished.MaxUserIdleTime"];
      if (this.maxUserIdleTime <= 0)
        return;
      this.checkIdleTimer.Interval = this.maxUserIdleTime * 1000;
      this.checkIdleTimer.Start();
    }

    public void StopCheckIdleTimer() => this.checkIdleTimer.Stop();

    public void HideInsightsOnStatusBar()
    {
      this.ssbInsights.Visible = false;
      this.statusBar.Refresh();
    }

    public void StatusBarShowProgress(bool visible = true) => this.ssProgressBar.Visible = visible;

    public void StatusBarSetProgress(int value)
    {
      if (value >= this.ssProgressBar.Maximum)
        return;
      this.ssProgressBar.Value = value;
    }

    public void StatusBarIncrementProgress(int value)
    {
      if (this.ssProgressBar.Value + value < this.ssProgressBar.Maximum)
        this.ssProgressBar.Value += value;
      Application.DoEvents();
    }

    public void StatusBarProgressSetMax(int max) => this.ssProgressBar.Maximum = max;

    public void ShowLoanMetricsOnStatusBar()
    {
      if (EnConfigurationSettings.GlobalSettings.ShowEnhancedMetrics)
      {
        this.ssbInsights.Visible = true;
        this.statusBar.Refresh();
      }
      this.ssbInsights.Visible = true;
      this.statusBar.Refresh();
    }

    public void ShowPollyExBrowser()
    {
      double num1 = 0.95;
      double num2 = 0.95;
      string url = EnConfigurationSettings.AppSettings["PollyExUrl"];
      if (string.IsNullOrEmpty(url))
      {
        Tracing.Log(MainForm.sw, nameof (MainForm), TraceLevel.Info, "ShowPollyExBrowser : Unable to read web.config file, hence fallback to production URL");
        url = "https://lx.pollyex.com/accounts/login/";
      }
      string title = "Deliver To Polly";
      LoadWebPageForm loadWebPageForm = new LoadWebPageForm(url, title);
      loadWebPageForm.Height = Convert.ToInt32(num1 * (double) this.mainScreen.PipelineScreen.Size.Height);
      loadWebPageForm.Width = Convert.ToInt32(num2 * (double) this.mainScreen.PipelineScreen.Size.Width);
      loadWebPageForm.MaximizeBox = true;
      loadWebPageForm.MinimizeBox = true;
      loadWebPageForm.FormBorderStyle = FormBorderStyle.Sizable;
      loadWebPageForm.StartPosition = FormStartPosition.CenterScreen;
      loadWebPageForm.Show();
    }

    public List<ToolStripMenuItem> ShowDataDocsMenus(
      FeaturesAclManager aclMgr,
      ServiceSetting wellsFargoServiceSetting)
    {
      List<ToolStripMenuItem> toolStripMenuItemList = new List<ToolStripMenuItem>();
      if (Session.StartupInfo.AllowDataAndDocs)
      {
        bool flag = MainScreen.AllowDataAndDocs && aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_Service) || MainScreen.AllowWarehouseLenders && aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_Warehouse_Lenders) || MainScreen.AllowDueDiligenceServices && aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_Due_Diligence) || MainScreen.AllowHedgeAdvisoryServices && aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_Hedge_Advisory) || MainScreen.AllowSubservicingServices && aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_Subservicing_Services) || MainScreen.AllowBidTapServices && aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_Bid_Tape_Services) || MainScreen.AllowQCAuditServices && aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_QC_Audit_Services) || MainScreen.AllowWholesaleLenderServices && aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_Wholesale_Lender_Services) || MainScreen.AllowServicingServices && aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_Servicing_Services);
        ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem("Investor Services");
        if (aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_Investor_Service))
        {
          if (wellsFargoServiceSetting != null)
          {
            ToolStripMenuItem serviceMenuItem = this.createServiceMenuItem(wellsFargoServiceSetting);
            toolStripMenuItem1.DropDownItems.Add((ToolStripItem) serviceMenuItem);
          }
          if (MainScreen.DataAndDocsPartners.Count > 0)
            flag = true;
        }
        ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem("Bid Tape");
        ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem("Deliver to Polly");
        toolStripMenuItem3.Click += new EventHandler(this.PollyExMenuItem_Click);
        toolStripMenuItem2.DropDownItems.Add((ToolStripItem) toolStripMenuItem3);
        if (flag)
        {
          ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem("Loan Delivery Services");
          toolStripMenuItem4.Click += new EventHandler(this.LoanDeliveryServicesMenuItem_Click);
          toolStripMenuItemList.Add(toolStripMenuItem4);
        }
        if (toolStripMenuItem1.DropDownItems.Count > 0)
          toolStripMenuItemList.Add(toolStripMenuItem1);
        toolStripMenuItemList.Add(toolStripMenuItem2);
      }
      return toolStripMenuItemList;
    }

    private void LoanDeliveryServicesMenuItem_Click(object sender, EventArgs e)
    {
      this.mainScreen.PipelineScreen.ShowLoanDeliveryServices();
    }

    private void Insights_Click(object sender, EventArgs e)
    {
      if (Session.LoanDataMgr.LastActivity == null)
        return;
      int num = (int) new LoanMetricsDialog(Session.LoanDataMgr.LastActivity).ShowDialog((IWin32Window) this);
    }

    private void setupAsychTasks()
    {
      if (Thread.CurrentThread.Name == null)
        Thread.CurrentThread.Name = "EMLiteMainUI";
      if (!((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.Cnt_Campaign_Access))
        return;
      this.runAsychTasks((object) null);
      this.asychTasksTimer = new System.Threading.Timer(new TimerCallback(this.runAsychTasks), (object) null, DateTime.Today.AddDays(1.0) - DateTime.Now, TimeSpan.FromHours(24.0));
    }

    private void runAsychTasks(object args)
    {
      try
      {
        CampaignRefreshTask campaignRefreshTask = new CampaignRefreshTask();
        campaignRefreshTask.CampaignRefreshStartedEvent += new CampaignRefreshStartedEventHandler(this.campaignRefreshStarted);
        campaignRefreshTask.CampaignRefreshCompletedEvent += new CampaignRefreshCompletedEventHandler(this.campaignRefreshCompleted);
        campaignRefreshTask.Start();
      }
      catch (Exception ex)
      {
        Tracing.Log(MainForm.sw, TraceLevel.Error, nameof (MainForm), "Error: " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
      }
    }

    private void campaignRefreshStarted(object sender, EventArgs e)
    {
      if (this.statusBar.InvokeRequired)
      {
        this.Invoke((Delegate) new MainForm.CampaignRefreshStartedCallback(this.campaignRefreshStarted), sender, (object) e);
      }
      else
      {
        this.campaignStatusPanel = new ToolStripStatusLabel();
        this.campaignStatusPanel.Text = "Refreshing Campaigns";
        this.campaignStatusPanel.AutoSize = true;
        this.statusBar.Items.Insert(1, (ToolStripItem) this.campaignStatusPanel);
      }
    }

    private void campaignRefreshCompleted(object sender, EventArgs e)
    {
      if (this.statusBar.InvokeRequired)
      {
        this.Invoke((Delegate) new MainForm.CampaignRefreshCompletedCallback(this.campaignRefreshCompleted), sender, (object) e);
      }
      else
      {
        this.statusBar.Items.Remove((ToolStripItem) this.campaignStatusPanel);
        this.campaignStatusPanel = (ToolStripStatusLabel) null;
      }
    }

    private void gcTimer_Tick(object sender, EventArgs e)
    {
      DateTime today = DateTime.Today;
      this.sslDate.Text = today.DayOfWeek.ToString() + ", " + today.ToShortDateString();
      if (!this.gcEnabled)
        return;
      GC.Collect(MainForm.gcGeneration);
    }

    public void DisplayFieldID(string id)
    {
      try
      {
        this.sslFieldID.Text = id;
        if (this.sslFieldID.Text == "")
          this.sslFieldID.BorderSides = ToolStripStatusLabelBorderSides.None;
        else
          this.sslFieldID.BorderSides = ToolStripStatusLabelBorderSides.Right;
      }
      catch (ExternalException ex)
      {
      }
    }

    public void DisplayHelpText(string text) => this.sslHelp.Text = text;

    public void SetBackgroundUploadIcon()
    {
      if (BackgroundAttachmentDialog.IsActive)
      {
        this.ssbUploadProgress.Visible = true;
        this.sslSeparator.Visible = true;
      }
      else
      {
        this.ssbUploadProgress.Visible = false;
        this.sslSeparator.Visible = false;
      }
    }

    public void SetTradeLoanUpdateIcon()
    {
      if (!TradeUpdateLoansDialog.IsActive)
        return;
      this.ssbTradeLoanUpdate.Visible = true;
    }

    private void MainForm_Closed(object sender, EventArgs e) => this.mainScreen.Close();

    public static bool ArgumentExists(string arg)
    {
      for (int index = 0; index < MainForm.ApplicationArgs.Length; ++index)
      {
        if (arg == MainForm.ApplicationArgs[index].ToLower())
          return true;
      }
      return false;
    }

    public static string GetArgumentParameter(string arg)
    {
      for (int index1 = 0; index1 < MainForm.ApplicationArgs.Length; ++index1)
      {
        if (arg == MainForm.ApplicationArgs[index1].ToLower())
        {
          int index2;
          return (index2 = index1 + 1) < MainForm.ApplicationArgs.Length ? MainForm.ApplicationArgs[index2] : "";
        }
      }
      return "";
    }

    private string trimMenuItem(object menuItem)
    {
      return ((ToolStripItem) menuItem).Text.Trim('.').Replace("&", string.Empty);
    }

    private void mainMenu_Click(object sender, EventArgs e)
    {
      this.mainScreen.MainMenuClick(this.trimMenuItem(sender), this.mainMenu, this.tsMenuItemHelp);
    }

    private void tsMenuItemEMMessage_Click(object sender, EventArgs e)
    {
      int num = (int) new MessageScreen(this.scAppUrl, this.scAppCmdArgs).ShowDialog((IWin32Window) this);
    }

    private void tabHeadItem_Popup(object sender, EventArgs e)
    {
      foreach (ToolStripItem dropDownItem in (ArrangedElementCollection) this.tsMenuItemEncompass.DropDownItems)
      {
        if (dropDownItem is ToolStripMenuItem)
        {
          bool flag = this.mainScreen.IsMenuItemEnabled(this.trimMenuItem((object) dropDownItem));
          dropDownItem.Visible = flag;
          dropDownItem.Enabled = flag;
        }
      }
      foreach (ToolStripItem dropDownItem in (ArrangedElementCollection) this.tsMenuItemView.DropDownItems)
      {
        if (dropDownItem is ToolStripMenuItem)
        {
          bool flag = this.mainScreen.IsMenuItemEnabled(this.trimMenuItem((object) dropDownItem));
          dropDownItem.Visible = flag;
          dropDownItem.Enabled = flag;
        }
      }
    }

    private void pipelineHeadItem_Popup(object sender, EventArgs e)
    {
      if (this.mainScreen.PipelineBrowser != null)
        return;
      this.setMenuItemStates(((ToolStripDropDownItem) sender).DropDownItems, this.mainScreen.PipelineScreenBrowserMenuProvider);
    }

    public void PipelineSetMenuItemsStates()
    {
      this.setMenuItemStates(this.pipelineHeadItem.DropDownItems, this.mainScreen.PipelineScreenBrowserMenuProvider);
    }

    public ToolStripItemCollection PipelineMenuItems => this.pipelineHeadItem.DropDownItems;

    private void pipeline_Click(object sender, EventArgs e)
    {
      this.mainScreen.PipelineScreenBrowserMenuProvider.MenuClicked((ToolStripItem) sender);
    }

    private void report_Click(object sender, EventArgs e)
    {
      this.mainScreen.ReportControl.MenuClicked((ToolStripItem) sender);
    }

    private void setPipelineMenu()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (!aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_CreateBlank) && !aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_CreateFromTmpl))
      {
        this.tsMenuItemNewLoan.Enabled = false;
        this.tsMenuItemNewLoan.ShortcutKeys = Keys.None;
      }
      if (!aclManager.GetUserApplicationRight(AclFeature.LoansTab_Print_PrintButton))
        this.tsMenuItemPrintForms.ShortcutKeys = Keys.None;
      if (!aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_Delete))
      {
        this.tsMenuItemDeleteLoan.Visible = false;
        this.tsMenuItemDeleteLoan.Enabled = false;
        this.tsMenuItemDeleteLoan.ShortcutKeys = Keys.None;
      }
      LoanFolderAclInfo[] loanFolderAclInfoArray1 = Session.StartupInfo.AccessibleFoldersForMove ?? ((LoanFoldersAclManager) Session.ACL.GetAclManager(AclCategory.LoanFolderMove)).GetAccessibleLoanFolders(AclFeature.LoanMgmt_Move, Session.UserID, Session.UserInfo.UserPersonas);
      if (loanFolderAclInfoArray1 != null)
      {
        bool flag = false;
        foreach (LoanFolderAclInfo loanFolderAclInfo in loanFolderAclInfoArray1)
        {
          if (loanFolderAclInfo.MoveFromAccess == 1)
          {
            flag = true;
            break;
          }
          if (loanFolderAclInfo.MoveToAccess == 1)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          this.tsMenuItemMoveToFolder.Visible = false;
          this.tsMenuItemMoveToFolder.Enabled = false;
          this.tsMenuItemMoveToFolder.ShortcutKeys = Keys.None;
        }
      }
      else
      {
        this.tsMenuItemMoveToFolder.Visible = false;
        this.tsMenuItemMoveToFolder.Enabled = false;
        this.tsMenuItemMoveToFolder.ShortcutKeys = Keys.None;
      }
      LoanFolderAclInfo[] loanFolderAclInfoArray2 = Session.StartupInfo.AccessibleFoldersForImport ?? ((LoanFoldersAclManager) Session.ACL.GetAclManager(AclCategory.LoanFolderMove)).GetAccessibleLoanFolders(AclFeature.LoanMgmt_Import, Session.UserID, Session.UserInfo.UserPersonas);
      bool flag1 = false;
      if (loanFolderAclInfoArray2 != null && loanFolderAclInfoArray2.Length != 0)
      {
        foreach (LoanFolderAclInfo loanFolderAclInfo in loanFolderAclInfoArray2)
        {
          if (loanFolderAclInfo.MoveFromAccess == 1 || loanFolderAclInfo.MoveToAccess == 1)
          {
            flag1 = true;
            break;
          }
        }
      }
      if (flag1)
      {
        this.tsMenuItemImport.Visible = true;
        this.tsMenuItemImport.Enabled = true;
        this.tsMenuItemImport.ShortcutKeys = Keys.I | Keys.Alt;
      }
      else
      {
        this.tsMenuItemImport.Visible = false;
        this.tsMenuItemImport.Enabled = false;
        this.tsMenuItemImport.ShortcutKeys = Keys.None;
      }
      if (aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_Transfer))
        return;
      this.tsMenuItemTransfer.Visible = false;
      this.tsMenuItemTransfer.Enabled = false;
      this.tsMenuItemTransfer.ShortcutKeys = Keys.None;
    }

    private void setLoanMenu()
    {
      UserInfo userInfo = Session.UserInfo;
      Hashtable hashtable = ((InputFormsAclManager) Session.ACL.GetAclManager(AclCategory.InputForms)).CheckPermissions(new string[10]
      {
        "VOD",
        "VOGG",
        "VOOI",
        "VOOL",
        "VOOA",
        "VOE",
        "VOL",
        "VOM",
        "VOR",
        "VOAL"
      });
      bool flag1 = (bool) hashtable[(object) "VOD"];
      bool flag2 = (bool) hashtable[(object) "VOGG"];
      bool flag3 = (bool) hashtable[(object) "VOOI"];
      bool flag4 = (bool) hashtable[(object) "VOOA"];
      bool flag5 = (bool) hashtable[(object) "VOE"];
      bool flag6 = (bool) hashtable[(object) "VOL"];
      bool flag7 = (bool) hashtable[(object) "VOOL"];
      bool flag8 = (bool) hashtable[(object) "VOM"];
      bool flag9 = (bool) hashtable[(object) "VOR"];
      bool flag10 = (bool) hashtable[(object) "VOAL"];
      this.vodItem.Visible = flag1;
      this.vodItem.Enabled = flag1;
      this.voggItem.Visible = flag2;
      this.voggItem.Enabled = flag2;
      this.vooiItem.Visible = flag3;
      this.vooiItem.Enabled = flag3;
      this.vooaItem.Visible = flag4;
      this.vooaItem.Enabled = flag4;
      this.voeItem.Visible = flag5;
      this.voeItem.Enabled = flag5;
      this.volItem.Visible = flag6;
      this.volItem.Enabled = flag6;
      this.voolItem.Visible = flag7;
      this.voolItem.Enabled = flag7;
      this.vomItem.Visible = flag8;
      this.vomItem.Enabled = flag8;
      this.vorItem.Visible = flag9;
      this.vorItem.Enabled = flag9;
      this.voalItem.Visible = flag10;
      this.voalItem.Enabled = flag10;
      if (!flag1 && !flag5 && !flag6 && !flag8 && !flag9)
      {
        this.verifHeadItem.Visible = false;
        this.verifHeadItem.Enabled = false;
      }
      else
      {
        this.verifHeadItem.Visible = true;
        this.verifHeadItem.Enabled = true;
      }
    }

    private string getFirstContactFeature()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (aclManager.GetUserApplicationRight(AclFeature.Cnt_Borrower_Access))
        return "Contact - Borrower";
      return aclManager.GetUserApplicationRight(AclFeature.Cnt_Biz_Access) ? "Contact - Business" : "Contact - Calendar";
    }

    internal ToolStripDropDown HelpDropDown => this.helpDropDown;

    internal void SetMenu(string selection)
    {
      ArrayList arrayList = new ArrayList();
      foreach (ToolStripMenuItem toolStripMenuItem in (ArrangedElementCollection) this.mainMenu.Items)
      {
        if (toolStripMenuItem != this.tsMenuItemEncompass && toolStripMenuItem != this.tsMenuItemView)
          arrayList.Add((object) toolStripMenuItem);
      }
      foreach (ToolStripItem toolStripItem in arrayList)
        this.mainMenu.Items.Remove(toolStripItem);
      GradientMenuStrip key = (GradientMenuStrip) null;
      switch (selection)
      {
        case "Contact - Borrower":
          this.resetContactSubTabMenu();
          this.borrowerContactItems.Visible = true;
          key = this.borrowerMenu;
          break;
        case "Contact - Business":
          this.resetContactSubTabMenu();
          this.bizContactItems.Visible = true;
          key = this.bizPartnerMenu;
          break;
        case "Contact - Calendar":
          this.resetContactSubTabMenu();
          this.calendarItems.Visible = true;
          key = this.calendarMenu;
          break;
        case "Contact - Campaigns":
          this.resetContactSubTabMenu();
          this.campaignItems.Visible = true;
          key = this.campaignMenu;
          break;
        case "Contact - Tasks":
          this.resetContactSubTabMenu();
          this.taskItems.Visible = true;
          key = this.taskMenu;
          break;
        case "Dashboard":
          key = this.dashboardMenu;
          break;
        case "Home":
          key = this.homeMenu;
          break;
        case "Loan":
          key = this.loanMenu;
          if (Session.EncompassEdition != EncompassEdition.Banker)
            this.removeBankerMenuItems();
          if (EpassLogin.IsEncompassSelfHosted)
            this.menuItemServices.Visible = false;
          if (!UserInfo.IsSuperAdministrator(Session.UserID, Session.UserInfo.UserPersonas))
          {
            this.setLoanMenu();
            break;
          }
          break;
        case "Pipeline":
          key = this.pipelineMenu;
          FeatureConfigsAclManager aclManager = (FeatureConfigsAclManager) Session.ACL.GetAclManager(AclCategory.FeatureConfigs);
          if (Session.EncompassEdition != EncompassEdition.Banker && aclManager.GetUserApplicationRight(AclFeature.PlatForm_Access) > 0)
            this.removeOpenWebViewFromPipeLine();
          if (!UserInfo.IsSuperAdministrator(Session.UserID, Session.UserInfo.UserPersonas))
            this.setPipelineMenu();
          if (!this.pipelineHeadItem.DropDownItems.Contains((ToolStripItem) this.toolStripSeparatorEx21))
          {
            this.populateServicesMenuItem();
            break;
          }
          break;
        case "Reports":
          key = this.reportMenu;
          this.newFolderReportItem.Visible = true;
          break;
        case "Services View":
          key = this.epassMenu;
          break;
        case "Trades":
          key = this.tradeMenu;
          break;
        case "Trades - CorrespondentMasterEditor":
          key = this.tradeMenu = this.correspondentMasterContractEditorMenu;
          break;
        case "Trades - CorrespondentMasters":
          key = this.tradeMenu = this.correspondentMasterContractListMenu;
          break;
        case "Trades - CorrespondentTradeEditor":
          key = this.tradeMenu = this.correspondentTradeEditorMenu;
          break;
        case "Trades - CorrespondentTrades":
          key = this.tradeMenu = this.correspondentTradeListMenu;
          break;
        case "Trades - GSECommitmentEditor":
          key = this.tradeMenu = this.GSECommitmentEditorMenu;
          break;
        case "Trades - GSECommitments":
          key = this.tradeMenu = this.GSECommitmentListMenu;
          break;
        case "Trades - LoanSearch":
          key = this.tradeMenu = this.loanSearchMenu;
          break;
        case "Trades - MasterContracts":
          key = this.tradeMenu = this.masterContractMenu;
          break;
        case "Trades - MbsPoolEditor":
          key = this.tradeMenu = this.mbsPoolEditorMenu;
          break;
        case "Trades - MbsPoolList":
          key = this.tradeMenu = this.mbsPoolListMenu;
          break;
        case "Trades - SecurityTradeEditor":
          key = this.tradeMenu = this.securityTradeEditorMenu;
          break;
        case "Trades - SecurityTradeList":
          key = this.tradeMenu = this.securityTradeListMenu;
          break;
        case "Trades - TradeEditor":
          key = this.tradeMenu = this.tradeEditorMenu;
          break;
        case "Trades - TradeList":
          key = this.tradeMenu = this.tradeListMenu;
          break;
      }
      if (key != null)
      {
        ToolStripMenuItem[] subMenu = (ToolStripMenuItem[]) this.subMenus[(object) key];
        if (subMenu != null)
          this.mainMenu.Items.AddRange((ToolStripItem[]) subMenu);
      }
      this.mainMenu.Items.Add((ToolStripItem) this.tsMenuItemHelp);
      this.tabHeadItem_Popup((object) null, (EventArgs) null);
    }

    private void resetContactSubTabMenu()
    {
      this.borrowerContactItems.Visible = false;
      this.bizContactItems.Visible = false;
      this.calendarItems.Visible = false;
      this.campaignItems.Visible = false;
      this.taskItems.Visible = false;
    }

    private void toolsItem_Popup(object sender, EventArgs e)
    {
      this.mainScreen.OpenLoanPage.PopulateMenuItems(this.loanToolsHeadItem);
    }

    private void loanTools_Click(object sender, EventArgs e)
    {
      this.mainScreen.OpenLoanPage.ToolsMenuClick(this.trimMenuItem(sender));
    }

    private void formHeadItem_Popup(object sender, EventArgs e)
    {
      this.mainScreen.OpenLoanPage.LoadFormsInMenu(this.formHeadItem);
    }

    private void verifHeadItem_Popup(object sender, EventArgs e)
    {
      if (!Session.StartupInfo.AllowURLA2020 || Session.LoanDataMgr.LoanData.GetField("1825") != "2020")
      {
        this.verifHeadItem.DropDownItems.Remove((ToolStripItem) this.voggItem);
        this.verifHeadItem.DropDownItems.Remove((ToolStripItem) this.vooiItem);
        this.verifHeadItem.DropDownItems.Remove((ToolStripItem) this.voolItem);
        this.verifHeadItem.DropDownItems.Remove((ToolStripItem) this.vooaItem);
        this.verifHeadItem.DropDownItems.Remove((ToolStripItem) this.voalItem);
      }
      else
      {
        this.verifHeadItem.DropDownItems.Add((ToolStripItem) this.voggItem);
        this.verifHeadItem.DropDownItems.Add((ToolStripItem) this.vooiItem);
        this.verifHeadItem.DropDownItems.Add((ToolStripItem) this.voolItem);
        this.verifHeadItem.DropDownItems.Add((ToolStripItem) this.vooaItem);
        this.verifHeadItem.DropDownItems.Add((ToolStripItem) this.voalItem);
      }
    }

    private void verif_Click(object sender, EventArgs e)
    {
      this.mainScreen.OpenLoanPage.FormMenu_Click(sender, e);
    }

    private void coMortgagerHeadItem_Popup(object sender, EventArgs e)
    {
      this.mainScreen.OpenLoanPage.PopulateBorrowersMenu((ToolStripMenuItem) sender);
    }

    private void BorrowerPairs_Click(object sender, EventArgs e)
    {
      this.mainScreen.OpenLoanPage.BorrowersMenu_Click(sender, e);
    }

    private void contactMenu_Click(object sender, EventArgs e)
    {
      if (((ToolStripItem) sender).Tag != null)
        this.mainScreen.ContactPage.ContactMenu_Click((ContactMainForm.ContactsActionEnum) ((ToolStripItem) sender).Tag);
      else
        this.mainScreen.ContactPage.ContactMenu_Click(this.trimMenuItem(sender));
    }

    public void settingsMenu_Click(object sender, EventArgs e)
    {
      this.mainScreen.SettingsPage.Menu_Click(this.trimMenuItem(sender));
    }

    private void loanConfig_Click(object sender, EventArgs e)
    {
      this.mainScreen.OpenLoanPage.LoanConfig_Click(this.trimMenuItem(sender));
    }

    private void lockUnlock_Click(object sender, EventArgs e)
    {
      this.mainScreen.OpenLoanPage.lockUnlock_Click((ToolStripMenuItem) sender);
    }

    public void SetLastSaveTime(DateTime dt, bool dirty)
    {
      if (dt == DateTime.MaxValue || dt == DateTime.MinValue)
        this.sslLastSaved.Text = "Not Saved Yet" + (dirty ? "*" : "");
      else
        this.sslLastSaved.Text = dt.ToString("MM/dd HH:mm") + (dirty ? "*" : "");
      this.sslLastSaved.BorderSides = ToolStripStatusLabelBorderSides.Right;
    }

    public void SetLastSaveTimeDirtyFlag()
    {
      if (this.sslLastSaved.Text.IndexOf("*") >= 0)
        return;
      this.sslLastSaved.Text += "*";
    }

    public void ClearLastSaveTime()
    {
      this.sslLastSaved.Text = "";
      this.sslLastSaved.BorderSides = ToolStripStatusLabelBorderSides.None;
    }

    private void checkIdleTimer_Tick(object sender, EventArgs e)
    {
      this.checkIdleTimer.Stop();
      if (this.maxUserIdleTime <= 0)
        return;
      int idleTime = Win32API.GetIdleTime();
      if (idleTime < this.maxUserIdleTime * 1000)
      {
        this.checkIdleTimer.Interval = this.maxUserIdleTime * 1000 - idleTime;
        this.checkIdleTimer.Start();
      }
      else if (this.actionAfterUserIdle == ActionAfterUserIdle.ExitEncompass)
        this.terminateAfterUserIdle(idleTime);
      else if (this.actionAfterUserIdle == ActionAfterUserIdle.LockWorkStation)
      {
        Win32API.LockWorkStation();
        int num = (int) Utils.Dialog((IWin32Window) MainForm.instance, "Your system has been locked because you have been idle for " + (object) (idleTime / 60000) + " minute(s).", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.StartCheckIdleTimer();
      }
      else if (this.actionAfterUserIdle == ActionAfterUserIdle.LockThenExit)
      {
        if (this.maxUserIdleTime2 < 0)
          this.maxUserIdleTime2 = (int) Session.StartupInfo.UnpublishedSettings[(object) "Unpublished.MaxUserIdleTime2"];
        if (idleTime >= (this.maxUserIdleTime + this.maxUserIdleTime2) * 1000)
        {
          this.terminateAfterUserIdle(idleTime);
        }
        else
        {
          Win32API.LockWorkStation();
          this.checkIdleTimer.Interval = (this.maxUserIdleTime + this.maxUserIdleTime2) * 1000 - idleTime;
          this.checkIdleTimer.Start();
        }
      }
      else
      {
        if (Utils.Dialog((IWin32Window) MainForm.instance, this.actionAfterUserIdle.ToString() + ": unhandled action. Do you want to cancel user idle detection?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.No)
          return;
        this.StartCheckIdleTimer();
      }
    }

    private void terminateAfterUserIdle(int idleTime)
    {
      Elli.Metrics.Client.MetricsFactory.IncrementErrorCounter((Exception) null, "TerminateAfterUserIdle ", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs", nameof (terminateAfterUserIdle), 8044);
      MainForm.instance.Visible = false;
      MainScreen.Instance.StopAutosaveTimer();
      MainScreen.Instance.PerformAutoSave();
      Session.End();
      int num = (int) Utils.Dialog((IWin32Window) MainForm.instance, "You have been idle for " + (object) (idleTime / 60000) + " minute(s). Encompass was terminated.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      Application.Exit();
    }

    private static void writeClientExceptionToServer(Exception ex)
    {
      try
      {
        bool flag = false;
        try
        {
          if (AssemblyResolver.IsSmartClient)
          {
            if ((AssemblyResolver.GetScAttribute(MainForm.writeClientExceptionAttrName) ?? "").Trim() == "1")
              flag = true;
          }
        }
        catch
        {
        }
        if ((SmartClientUtils.GetAttribute(Session.CompanyInfo.ClientID, "Encompass.exe", MainForm.writeClientExceptionAttrName) ?? "").Trim() == "1")
          flag = true;
        if (!flag)
          return;
        RemoteLogger.Write(ex);
      }
      catch
      {
      }
    }

    private void tsMenuItemAbout_Click(object sender, EventArgs e)
    {
      int num = (int) new AboutPage().ShowDialog((IWin32Window) Form.ActiveForm);
      bool flag = false;
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\Encompass"))
      {
        if (registryKey != null)
        {
          if (string.Concat(registryKey.GetValue("EnableConnectionErrorSimulation")).Trim() == "1")
            flag = true;
        }
      }
      if (!flag)
        return;
      Session.DefaultInstance.SimulateConnectionError(ConnectionErrorType.ConnectionClosed);
    }

    private void setInitialAppSize()
    {
      Size size = new Size(Math.Min(1024, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width), Math.Min(768, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height));
      Point location;
      ref Point local = ref location;
      Rectangle workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
      int x = (workingArea.Width - size.Width) / 2;
      workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
      int y = (workingArea.Height - size.Height) / 2;
      local = new Point(x, y);
      FormProperties formProperties = new FormProperties(location, size, FormWindowState.Normal);
      formProperties.ApplyToForm((Form) this, false);
      string path = Path.Combine(SystemSettings.UserLocalSettingsDir, "WindowSettings.xml");
      if (System.IO.File.Exists(path))
      {
        using (BinaryObject o = new BinaryObject(path))
        {
          try
          {
            formProperties = BinaryConvertible<FormProperties>.Parse(o);
          }
          catch
          {
          }
        }
      }
      formProperties.ApplyToForm((Form) this, true);
    }

    private void saveFormProperties()
    {
      if (this.WindowState == FormWindowState.Minimized)
        this.WindowState = FormWindowState.Normal;
      string path = Path.Combine(SystemSettings.UserLocalSettingsDir, "WindowSettings.xml");
      BinaryObject binaryObject = (BinaryObject) (BinaryConvertibleObject) new FormProperties((Form) this);
      try
      {
        binaryObject.Write(path);
      }
      catch
      {
      }
    }

    public void EnableDisableMenuItems(MainForm.MenuItemEnum menuItem, bool enable)
    {
      this.EnableDisableMenuItems(new Dictionary<MainForm.MenuItemEnum, bool>()
      {
        {
          menuItem,
          enable
        }
      });
    }

    public void EnableDisableMenuItems(Dictionary<MainForm.MenuItemEnum, bool> menuItems)
    {
      foreach (MainForm.MenuItemEnum key in menuItems.Keys)
      {
        switch (key)
        {
          case MainForm.MenuItemEnum.eFolder:
            this.tsMenuItemEfolder.Enabled = menuItems[key];
            continue;
          case MainForm.MenuItemEnum.ePASS:
            this.tsMenuItemEpass.Enabled = menuItems[key];
            continue;
          case MainForm.MenuItemEnum.Loan:
            this.tsMenuItemLoan.Enabled = menuItems[key];
            continue;
          default:
            continue;
        }
      }
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.mainScreen != null && Session.IsConnected)
      {
        e.Cancel = !this.mainScreen.ClosingApp(e.CloseReason == CloseReason.UserClosing);
        if (e.Cancel)
          return;
      }
      BackgroundAttachmentDialog.CloseInstance();
      PerformancePublisher.CloseInstance();
      PerformancePublisherDOCS.CloseInstance();
      this.saveFormProperties();
    }

    private void tsMenuItemReportOption_Click(object sender, EventArgs e)
    {
      if (Session.GetPrivateProfileString("Report.HideReportOption") == "1" || Session.GetPrivateProfileString("Report.HideReportOption") == "")
        Session.WritePrivateProfileString("Report.HideReportOption", "0");
      else
        Session.WritePrivateProfileString("Report.HideReportOption", "1");
    }

    private void tsMenuItemReportOption_Paint(object sender, PaintEventArgs e)
    {
      if (Session.GetPrivateProfileString("Report.HideReportOption") == "0")
        this.tsMenuItemReportOption.Image = (Image) Resources.check_mark_green;
      else
        this.tsMenuItemReportOption.Image = (Image) null;
    }

    private void startBrowser(string url)
    {
      new Process() { StartInfo = { FileName = url } }.Start();
    }

    private void tsMenuItemHelpPad_Click(object sender, EventArgs e) => FieldHelpDialog.Open();

    private void tsMenuItemDocLibrary_Click(object sender, EventArgs e)
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Documentation Library");
    }

    private void tsMenuItemTrainingSchedule_Click(object sender, EventArgs e)
    {
      AboutPage.ShowTrainingPage();
    }

    private void tsMenuItemTechSupportOptions_Click(object sender, EventArgs e)
    {
      AboutPage.ShowSupportPage();
    }

    private void MenuItemEllieMae_Click(object sender, EventArgs e)
    {
      this.startBrowser("http://www.elliemae.com/");
    }

    private void tsMenuItemReleaseNotes_Click(object sender, EventArgs e)
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Release Notes");
    }

    private void tsMenuItemTutorials_Click(object sender, EventArgs e)
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Tutorials");
    }

    private void tsMenuItemEncompassGlossary_Click(object sender, EventArgs e)
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Encompass Glossary");
    }

    private void tsMenuItemEncompassHelp_Click(object sender, EventArgs e)
    {
      this.mainScreen.ShowHelp((Control) this);
    }

    private void initialContactMenuItems()
    {
      this.tsBCAddToGroup.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_AddToGroup;
      this.tsBCBuyLeads.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_BuyLead;
      this.tsBCDeleteContact.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_DeleteContact;
      this.tsBCDuplicateContact.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_DuplicateContact;
      this.tsBCEditGroups.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_EditGroup;
      this.tsBCExportExcel.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_ExportExcel;
      this.tsExportBorContacts.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_ExportSelectedExcel;
      this.tsExportAllBorContacts.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_ExportAllExcel;
      this.tsBCImportBorrower.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_ImportContact;
      this.tsBCImportLeads.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_ImportLead;
      this.tsBCMailMerge.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_MailMerge;
      this.tsMailMergeBorContacts.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_MailMergeSelected;
      this.tsMailMergeAllBorContacts.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_MailMergeAll;
      this.tsBCNewContact.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_NewContact;
      this.tsBCOrderCredit.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_OrderCredit;
      this.tsBCOriginateLoan.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_OriginateLoan;
      this.tsBCPrintDetails.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_PrintDetails;
      this.tsPrintBorContacts.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_PrintSelectedDetails;
      this.tsPrintAllBorContacts.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_PrintAllDetails;
      this.tsBCProductPricing.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_ProductPricing;
      this.tsBCRemoveFromGroup.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_RemoveFromGroup;
      this.tsBCReassign.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_Reassign;
      this.tsBCCustomizeColumns.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_CustomizeColumns;
      this.tsBCSaveView.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_SaveView;
      this.tsBCResetView.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_ResetView;
      this.tsBCManageViews.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_ManageView;
      this.tsBCExportBorrower.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_CSVExport;
      this.tsBCExportBorrowerAll.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_CSVExportAll;
      this.tsBCExportBorrowerSelectedOnly.Tag = (object) ContactMainForm.ContactsActionEnum.Borrower_CSVExportSelected;
      this.tsBizCAddToGroup.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_AddToGroup;
      this.tsBizCCustomizeColumn.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_CustomizeColumns;
      this.tsBizCDeleteContact.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_DeleteContact;
      this.tsBizCDuplicateContact.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_DuplicateContact;
      this.tsBizCEditGroup.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_EditGroup;
      this.tsBizCExportToExcel.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_ExportExcel;
      this.tsExportBizContacts.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_ExportSelectedExcel;
      this.tsExportAllBizContacts.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_ExportAllExcel;
      this.tsBizCImportBizContact.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_ImportContact;
      this.tsBizCMailMerge.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_MailMerge;
      this.tsMailMergeBizContacts.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_MailMergeSelected;
      this.tsMailMergeAllBizContacts.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_MailMergeAll;
      this.tsBizCManageView.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_ManageView;
      this.tsBizCNewContact.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_NewContact;
      this.tsBizCPrintDetail.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_PrintDetails;
      this.tsPrintBizContacts.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_PrintSelectedDetails;
      this.tsPrintAllBizContacts.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_PrintAllDetails;
      this.tsBizCRemoveFromGroup.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_RemoveFromGroup;
      this.tsBizCResetView.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_ResetView;
      this.tsBizCSaveView.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_SaveView;
      this.tsBizCSynchronize.Tag = (object) ContactMainForm.ContactsActionEnum.Synchronization;
      this.tsBizCExportBizContact.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_CSVExport;
      this.tsBizCExportBizContactAll.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_CSVExportAll;
      this.tsBizCExportBizContactSelectedOnly.Tag = (object) ContactMainForm.ContactsActionEnum.Biz_CSVExportSelected;
      this.tsCalDeleteApp.Tag = (object) ContactMainForm.ContactsActionEnum.Cal_DeleteAppointment;
      this.tsCalEditApp.Tag = (object) ContactMainForm.ContactsActionEnum.Cal_EditAppointment;
      this.tsCalMonth.Tag = (object) ContactMainForm.ContactsActionEnum.Cal_Month;
      this.tsCalNewApp.Tag = (object) ContactMainForm.ContactsActionEnum.Cal_NewAppointment;
      this.tsCalOneDay.Tag = (object) ContactMainForm.ContactsActionEnum.Cal_1Day;
      this.tsCalPrint.Tag = (object) ContactMainForm.ContactsActionEnum.Cal_Print;
      this.tsCalToday.Tag = (object) ContactMainForm.ContactsActionEnum.Cal_Today;
      this.tsCalView.Tag = (object) ContactMainForm.ContactsActionEnum.Cal_View;
      this.tsCalWeek.Tag = (object) ContactMainForm.ContactsActionEnum.Cal_Week;
      this.tsCalWorkDay.Tag = (object) ContactMainForm.ContactsActionEnum.Cal_WorkDay;
      this.tsCalSynchronize.Tag = (object) ContactMainForm.ContactsActionEnum.Synchronization;
      this.tsTask_Delete.Tag = (object) ContactMainForm.ContactsActionEnum.Task_DeleteTask;
      this.tsTask_EditTask.Tag = (object) ContactMainForm.ContactsActionEnum.Task_EditTask;
      this.tsTask_ExportExcel.Tag = (object) ContactMainForm.ContactsActionEnum.Task_ExportExcel;
      this.tsTask_NewTask.Tag = (object) ContactMainForm.ContactsActionEnum.Task_NewTask;
      this.tsTask_Status.Tag = (object) ContactMainForm.ContactsActionEnum.Task_Status;
      this.tsTask_StatusCompleted.Tag = (object) ContactMainForm.ContactsActionEnum.Task_Status_Completed;
      this.tsTask_StatusDeferred.Tag = (object) ContactMainForm.ContactsActionEnum.Task_Status_Deferred;
      this.tsTask_StatusInProgress.Tag = (object) ContactMainForm.ContactsActionEnum.Task_Status_InProgress;
      this.tsTask_StatusNotStarted.Tag = (object) ContactMainForm.ContactsActionEnum.Task_Status_NotStarted;
      this.tsTask_StatusWait.Tag = (object) ContactMainForm.ContactsActionEnum.Task_Status_WaitOnSomeoneElse;
      this.tsTask_Synchronize.Tag = (object) ContactMainForm.ContactsActionEnum.Synchronization;
      this.tsCampaign_DeleteCampaign.Tag = (object) ContactMainForm.ContactsActionEnum.Campaign_DeleteCampaign;
      this.tsCampaign_DuplicateCampaign.Tag = (object) ContactMainForm.ContactsActionEnum.Campaign_DuplicateCampaign;
      this.tsCampaign_ManageTemplate.Tag = (object) ContactMainForm.ContactsActionEnum.Campaign_ManageTemplate;
      this.tsCampaign_NewCampaign.Tag = (object) ContactMainForm.ContactsActionEnum.Campaign_NewCampaign;
      this.tsCampaign_OpenCampaign.Tag = (object) ContactMainForm.ContactsActionEnum.Campaign_OpenCampaign;
      this.tsCampaign_StartCampaign.Tag = (object) ContactMainForm.ContactsActionEnum.Campaign_StartCampaign;
      this.tsCampaign_StopCampaign.Tag = (object) ContactMainForm.ContactsActionEnum.Campaign_StopCampaign;
      this.tsBCHomePoints.Tag = (object) ContactMainForm.ContactsActionEnum.HomePoints;
      this.tsBCSynchronize.Tag = (object) ContactMainForm.ContactsActionEnum.Synchronization;
      this.tsMenuItemContacts.DropDownOpening += new EventHandler(this.tsMenuItemContacts_DropDownOpening);
    }

    private void tsMenuItemContacts_DropDownOpening(object sender, EventArgs e)
    {
      foreach (ToolStripItem dropDownItem1 in (ArrangedElementCollection) this.tsMenuItemContacts.DropDownItems)
      {
        if (dropDownItem1 is ToolStripMenuItem)
        {
          ToolStripMenuItem menuItem = (ToolStripMenuItem) dropDownItem1;
          bool flag = this.mainScreen.IsMenuItemEnabled(this.trimMenuItem((object) menuItem));
          menuItem.Visible = flag;
          menuItem.Enabled = flag;
          if (flag && menuItem.DropDownItems.Count > 0)
          {
            foreach (ToolStripItem dropDownItem2 in (ArrangedElementCollection) menuItem.DropDownItems)
              dropDownItem2.Visible = dropDownItem2.Enabled = this.mainScreen.IsMenuItemEnabled(this.trimMenuItem((object) dropDownItem2));
          }
        }
      }
    }

    private void initialReportMenuItems()
    {
      this.addReportItem.Tag = (object) FSExplorer.MenuFunctionTypes.AddNewFile;
      this.duplicateReportItem.Tag = (object) FSExplorer.MenuFunctionTypes.DuplicateFile;
      this.newFolderReportItem.Tag = (object) FSExplorer.MenuFunctionTypes.CreateFolder;
      this.deleteReportItem.Tag = (object) FSExplorer.MenuFunctionTypes.DeleteFolderOrFile;
      this.renameReportItem.Tag = (object) FSExplorer.MenuFunctionTypes.RenameFolderOrFile;
      this.refreshReportItem.Tag = (object) FSExplorer.MenuFunctionTypes.RefreshFolder;
    }

    private void tsBC_DropDownOpening(object sender, EventArgs e)
    {
      ToolStripItem toolStripItem = (ToolStripItem) null;
      foreach (ToolStripItem dropDownItem1 in (ArrangedElementCollection) this.borrowerContactItems.DropDownItems)
      {
        bool flag;
        if (dropDownItem1 is ToolStripSeparator)
        {
          flag = toolStripItem == null || !(toolStripItem is ToolStripSeparator);
        }
        else
        {
          flag = dropDownItem1.Tag != null && this.mainScreen.ContactPage.IsMenuItemVisible((ContactMainForm.ContactsActionEnum) dropDownItem1.Tag);
          if (flag)
          {
            dropDownItem1.Enabled = this.mainScreen.ContactPage.IsMenuItemEnabled((ContactMainForm.ContactsActionEnum) dropDownItem1.Tag);
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem) dropDownItem1;
            if (toolStripMenuItem.DropDownItems.Count > 0)
            {
              foreach (ToolStripItem dropDownItem2 in (ArrangedElementCollection) toolStripMenuItem.DropDownItems)
                dropDownItem2.Enabled = this.mainScreen.ContactPage.IsMenuItemEnabled((ContactMainForm.ContactsActionEnum) dropDownItem2.Tag);
            }
          }
        }
        dropDownItem1.Visible = flag;
        if (flag)
          toolStripItem = dropDownItem1;
      }
    }

    private void bizContactItems_DropDownOpening(object sender, EventArgs e)
    {
      ToolStripItem toolStripItem = (ToolStripItem) null;
      foreach (ToolStripItem dropDownItem1 in (ArrangedElementCollection) this.bizContactItems.DropDownItems)
      {
        bool flag;
        if (dropDownItem1 is ToolStripSeparator)
        {
          flag = toolStripItem == null || !(toolStripItem is ToolStripSeparator);
        }
        else
        {
          flag = dropDownItem1.Tag != null && this.mainScreen.ContactPage.IsMenuItemVisible((ContactMainForm.ContactsActionEnum) dropDownItem1.Tag);
          if (flag)
          {
            dropDownItem1.Enabled = this.mainScreen.ContactPage.IsMenuItemEnabled((ContactMainForm.ContactsActionEnum) dropDownItem1.Tag);
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem) dropDownItem1;
            if (toolStripMenuItem.DropDownItems.Count > 0)
            {
              foreach (ToolStripItem dropDownItem2 in (ArrangedElementCollection) toolStripMenuItem.DropDownItems)
                dropDownItem2.Enabled = this.mainScreen.ContactPage.IsMenuItemEnabled((ContactMainForm.ContactsActionEnum) dropDownItem2.Tag);
            }
          }
        }
        dropDownItem1.Visible = flag;
        if (flag)
          toolStripItem = dropDownItem1;
      }
    }

    private void tsMenuItemOpenRecent_MouseEnter(object sender, EventArgs e)
    {
      string[] recentLoanGuids = Session.LoanManager.GetRecentLoanGuids(10, false);
      if (recentLoanGuids == null || recentLoanGuids.Length == 0)
        this.addRecentLoansToMenu(new PipelineInfo[0]);
      else
        this.addRecentLoansToMenu(Session.LoanManager.GetPipeline(recentLoanGuids, false));
    }

    private void tsMilestoneList_MouseEnter(object sender, EventArgs e)
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      MilestoneTemplatesSetting serverSetting = (MilestoneTemplatesSetting) Session.ServerManager.GetServerSetting("Policies.MilestoneTemplateSettings");
      foreach (ToolStripMenuItem dropDownItem in (ArrangedElementCollection) this.tsMilestoneList.DropDownItems)
      {
        switch (dropDownItem.Text.Trim('.').Replace("&", string.Empty))
        {
          case "Apply Milestone Template":
            dropDownItem.Visible = this.checkAccessForManual();
            dropDownItem.Enabled = Session.LoanDataMgr.GetFieldAccessRights("Log.MT.Name") != BizRule.FieldAccessRight.Hide && Session.LoanDataMgr.GetFieldAccessRights("Log.MT.Name") != BizRule.FieldAccessRight.ViewOnly && Session.LoanDataMgr.GetFieldAccessRights("Log.MT.TemplateId") != BizRule.FieldAccessRight.Hide && Session.LoanDataMgr.GetFieldAccessRights("Log.MT.TemplateId") != BizRule.FieldAccessRight.ViewOnly;
            continue;
          case "Apply Manual Mode":
            dropDownItem.Visible = aclManager.GetUserApplicationRight(AclFeature.LoanTab_LockUnlockMilestonesList);
            if (serverSetting == MilestoneTemplatesSetting.None)
            {
              dropDownItem.Enabled = false;
              continue;
            }
            dropDownItem.Enabled = Session.LoanDataMgr.GetFieldAccessRights("Log.MT.IsLocked") == BizRule.FieldAccessRight.Edit;
            dropDownItem.Enabled = !Session.LoanData.GetLogList().MSLock;
            continue;
          case "Apply Automatic Mode":
            dropDownItem.Visible = aclManager.GetUserApplicationRight(AclFeature.LoanTab_LockUnlockMilestonesList);
            if (serverSetting == MilestoneTemplatesSetting.None)
            {
              dropDownItem.Enabled = false;
              continue;
            }
            dropDownItem.Enabled = Session.LoanDataMgr.GetFieldAccessRights("Log.MT.IsLocked") == BizRule.FieldAccessRight.Edit;
            dropDownItem.Enabled = Session.LoanData.GetLogList().MSLock;
            continue;
          default:
            continue;
        }
      }
    }

    private void tsMilestoneDates_MouseEnter(object sender, EventArgs e)
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      foreach (ToolStripMenuItem dropDownItem in (ArrangedElementCollection) this.tsMilestoneDates.DropDownItems)
      {
        switch (dropDownItem.Text.Trim('.').Replace("&", string.Empty))
        {
          case "Change Milestone Dates":
            dropDownItem.Visible = true;
            dropDownItem.Enabled = this.checkAccessforChangingDates();
            continue;
          case "Apply Manual Mode":
            dropDownItem.Visible = aclManager.GetUserApplicationRight(AclFeature.LoanTab_LockMilestoneDates);
            dropDownItem.Enabled = Session.LoanDataMgr.GetFieldAccessRights("Log.MT.IsDateLocked") == BizRule.FieldAccessRight.Edit;
            dropDownItem.Enabled = !Session.LoanData.GetLogList().MSDateLock;
            continue;
          case "Apply Automatic Mode":
            dropDownItem.Visible = aclManager.GetUserApplicationRight(AclFeature.LoanTab_LockMilestoneDates);
            dropDownItem.Enabled = Session.LoanDataMgr.GetFieldAccessRights("Log.MT.IsDateLocked") == BizRule.FieldAccessRight.Edit;
            dropDownItem.Enabled = Session.LoanData.GetLogList().MSDateLock;
            continue;
          default:
            continue;
        }
      }
    }

    private bool checkAccessForManual()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      switch ((MilestoneTemplatesSetting) Session.StartupInfo.PolicySettings[(object) "Policies.MilestoneTemplateSettings"])
      {
        case MilestoneTemplatesSetting.Automatic:
        case MilestoneTemplatesSetting.None:
          return false;
        default:
          return aclManager.GetUserApplicationRight(AclFeature.LoanTab_ManuallyApplyMilestoneTemplate);
      }
    }

    private bool checkAccessforChangingDates()
    {
      List<string> stringList = new List<string>();
      foreach (MilestoneLog allMilestone in Session.LoanData.GetLogList().GetAllMilestones())
        stringList.Add(allMilestone.MilestoneID);
      return ChangeMilestoneDates.GetPermissionFromUser(AclMilestone.ChangeExpectedDate, Session.DefaultInstance, stringList.ToArray()).ContainsValue(AclTriState.True);
    }

    private void addRecentLoansToMenu(PipelineInfo[] loans)
    {
      this.tsMenuItemOpenRecent.DropDownItems.Clear();
      foreach (PipelineInfo loan in loans)
      {
        ToolStripItem toolStripItem = this.tsMenuItemOpenRecent.DropDownItems.Add(loan.LastName + ", " + loan.FirstName + " (" + (object) loan.LastModified + ")");
        toolStripItem.Tag = (object) loan.GUID;
        toolStripItem.Click += new EventHandler(this.recentLoanMenuItem_Click);
      }
    }

    private void recentLoanMenuItem_Click(object sender, EventArgs e)
    {
      this.mainScreen.PipelineScreenBrowser.OpenLoan(((ToolStripItem) sender).Tag as string);
    }

    private void calendarItems_DropDownOpening(object sender, EventArgs e)
    {
      ToolStripItem toolStripItem = (ToolStripItem) null;
      foreach (ToolStripItem dropDownItem in (ArrangedElementCollection) this.calendarItems.DropDownItems)
      {
        bool flag;
        if (dropDownItem is ToolStripSeparator)
        {
          flag = toolStripItem == null || !(toolStripItem is ToolStripSeparator);
        }
        else
        {
          flag = dropDownItem.Tag != null && this.mainScreen.ContactPage.IsMenuItemVisible((ContactMainForm.ContactsActionEnum) dropDownItem.Tag);
          if (flag)
            dropDownItem.Enabled = this.mainScreen.ContactPage.IsMenuItemEnabled((ContactMainForm.ContactsActionEnum) dropDownItem.Tag);
        }
        dropDownItem.Visible = flag;
        if (flag)
          toolStripItem = dropDownItem;
      }
    }

    private void taskItems_DropDownOpening(object sender, EventArgs e)
    {
      ToolStripItem toolStripItem = (ToolStripItem) null;
      foreach (ToolStripItem dropDownItem in (ArrangedElementCollection) this.taskItems.DropDownItems)
      {
        bool flag;
        if (dropDownItem is ToolStripSeparator)
        {
          flag = toolStripItem == null || !(toolStripItem is ToolStripSeparator);
        }
        else
        {
          flag = dropDownItem.Tag != null && this.mainScreen.ContactPage.IsMenuItemVisible((ContactMainForm.ContactsActionEnum) dropDownItem.Tag);
          if (flag)
            dropDownItem.Enabled = this.mainScreen.ContactPage.IsMenuItemEnabled((ContactMainForm.ContactsActionEnum) dropDownItem.Tag);
        }
        dropDownItem.Visible = flag;
        if (flag)
          toolStripItem = dropDownItem;
      }
    }

    private void campaignItems_DropDownOpening(object sender, EventArgs e)
    {
      ToolStripItem toolStripItem = (ToolStripItem) null;
      foreach (ToolStripItem dropDownItem in (ArrangedElementCollection) this.campaignItems.DropDownItems)
      {
        bool flag;
        if (dropDownItem is ToolStripSeparator)
        {
          flag = toolStripItem == null || !(toolStripItem is ToolStripSeparator);
        }
        else
        {
          flag = dropDownItem.Tag != null && this.mainScreen.ContactPage.IsMenuItemVisible((ContactMainForm.ContactsActionEnum) dropDownItem.Tag);
          if (flag)
            dropDownItem.Enabled = this.mainScreen.ContactPage.IsMenuItemEnabled((ContactMainForm.ContactsActionEnum) dropDownItem.Tag);
        }
        dropDownItem.Visible = flag;
        if (flag)
          toolStripItem = dropDownItem;
      }
    }

    private void tsDashboardHeadItem_DropDownOpening(object sender, EventArgs e)
    {
      ToolStripItem toolStripItem = (ToolStripItem) null;
      foreach (ToolStripItem dropDownItem in (ArrangedElementCollection) this.tsDashboardHeadItem.DropDownItems)
      {
        bool flag = !(dropDownItem is ToolStripSeparator) ? this.mainScreen.DashboardPage.IsMenuItemVisible(this.trimMenuItem((object) dropDownItem)) : toolStripItem == null || !(toolStripItem is ToolStripSeparator);
        if (flag)
          dropDownItem.Enabled = this.mainScreen.DashboardPage.IsMenuItemEnabled(this.trimMenuItem((object) dropDownItem));
        dropDownItem.Visible = flag;
        if (flag)
          toolStripItem = dropDownItem;
      }
    }

    private void TradesMenuItem__Click(object sender, EventArgs e)
    {
      this.mainScreen.TradingScreenBrowserMenuProvider.MenuClicked((ToolStripItem) sender);
    }

    private void DashboardMenuItem__Click(object sender, EventArgs e)
    {
      this.mainScreen.DashboardPage.MenuClicked(this.trimMenuItem((object) (ToolStripItem) sender));
    }

    private void reportHeadItem_DropDownOpening(object sender, EventArgs e)
    {
      this.setMenuItemStates(((ToolStripDropDownItem) sender).DropDownItems, (IMenuProvider) this.mainScreen.ReportControl);
    }

    private void tsMenuItemFeedback_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<EllieMae.EMLite.Common.UI.IHomePage>().Navigate("https://encompass.elliemae.com/homepage/_HOMEPAGE_SIGNATURE;EXECUTE;3010;SUPPORT2");
      Session.Application.GetService<IEncompassApplication>().SetCurrentActivity(EncompassActivity.Home);
    }

    private void menuItemServices_DropDownOpening(object sender, EventArgs e)
    {
      this.menuItemServices.DropDownItems.Clear();
      ToolStripMenuItem[] serviceMenuItems = this.mainScreen.OpenLoanPage.ServiceMenuItems;
      if (serviceMenuItems == null)
        return;
      this.menuItemServices.DropDownItems.AddRange((ToolStripItem[]) serviceMenuItems);
    }

    private void toggleDocsEngine(object sender, EventArgs e)
    {
      string docEngine;
      string str;
      if (Session.LoanData.GetSimpleField("Docs.Engine") == "New_Encompass_Docs_Solution")
      {
        docEngine = "Old_ODI_Encompass_Docs";
        str = "Old ODI Encompass Docs";
      }
      else
      {
        docEngine = "New_Encompass_Docs_Solution";
        str = "New Encompass Docs Solutions";
      }
      ILoanServices service = Session.Application.GetService<ILoanServices>();
      List<string> stringList = new List<string>();
      if (service.IsEncompassDocServiceAvailable(DocumentOrderType.Opening))
        stringList.Add("eDisclosures");
      if (service.IsEncompassDocServiceAvailable(DocumentOrderType.Closing))
        stringList.Add("Closing Documents");
      if (Utils.Dialog((IWin32Window) Session.Application, "You have elected to use the " + str + " to generate " + string.Join(" and ", stringList.ToArray()) + ". By switching to this service, you will required to re-select Plan Code and, if applicable, Alt Lender information for this loan." + Environment.NewLine + Environment.NewLine + "Are you sure you want to proceed with this change?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      DocEngineUtils.ChangeEncompassDocEngine((IHtmlInput) Session.LoanData, docEngine);
      Session.Application.GetService<ILoanEditor>()?.RefreshContents();
    }

    private void onTradeMenuOpening(object sender, EventArgs e)
    {
      if (MainScreen.Instance.TradeConsole == null)
        return;
      this.setMenuItemStates(((ToolStripDropDownItem) sender).DropDownItems, (IMenuProvider) this.mainScreen.TradeConsole);
    }

    private void setMenuItemStates(ToolStripItemCollection menuItems, IMenuProvider menuProvider)
    {
      ToolStripItem toolStripItem = (ToolStripItem) null;
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      foreach (ToolStripItem menuItem in (ArrangedElementCollection) menuItems)
      {
        if (menuItem is ToolStripSeparator)
        {
          switch (toolStripItem)
          {
            case null:
            case ToolStripSeparator _:
              menuItem.Visible = false;
              break;
            default:
              menuItem.Visible = true;
              toolStripItem = menuItem;
              break;
          }
        }
        else
        {
          bool flag = menuProvider.SetMenuItemState(menuItem);
          menuItem.Visible = flag;
          if (flag)
            toolStripItem = menuItem;
        }
        if (menuItem is ToolStripMenuItem)
          this.setMenuItemStates(((ToolStripDropDownItem) menuItem).DropDownItems, menuProvider);
        if (menuItem.Name != "" && menuItem.Name == this.fannieMaeExport.Name && !Session.UserInfo.IsSuperAdministrator() && !aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_Export_FannieMae_FormattedFile))
          menuItem.Visible = false;
      }
      if (!(toolStripItem is ToolStripSeparator))
        return;
      toolStripItem.Visible = false;
    }

    private void loanHeadItem_DropDownOpening(object sender, EventArgs e)
    {
      this.mainScreen.OpenLoanPage.PopulateMenuItems(this.loanHeadItem);
    }

    private void tsMenuItemHelp_DropDownOpening(object sender, EventArgs e)
    {
      this.tsMenuItemHelp.DropDown = this.helpDropDown;
      this.showOrHideDDMDiagnostic();
      this.setDiagnosticMenuItems();
    }

    private void createTabPageMenuItemMapping()
    {
      if (this.tabPageMenuItemMap != null)
        return;
      this.tabPageMenuItemMap = new Dictionary<string, ToolStripMenuItem>();
      this.tabPageMenuItemMap.Add("Home", this.tsMenuItemHome);
      this.tabPageMenuItemMap.Add("Loan", this.tsMenuItemLoan);
      this.tabPageMenuItemMap.Add("Trades", this.tsMenuItemTrades);
      this.tabPageMenuItemMap.Add("Contacts", this.tsMenuItemContacts);
      this.tabPageMenuItemMap.Add("Dashboard", this.tsMenuItemDashboard);
      this.tabPageMenuItemMap.Add("Reports", this.tsMenuItemReports);
      this.tabPageMenuItemMap.Add("Pipeline", this.tsMenuItemPipeline);
      this.tabPageMenuItemMap.Add("Services View", this.tsMenuItemEpass);
    }

    private bool hasRights(ServiceSetting menu)
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (menu.ID.ToLower().Contains("fannie"))
        return aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_Export_ULDD_FannieMae);
      if (menu.ID.ToLower().Contains("freddie"))
        return aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_Export_ULDD_FreddieMac);
      if (menu.ID.ToLower().Contains("ginniemae"))
        return aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_Export_PDD_GinnieMae);
      if (menu.DisplayName == "Export Fannie Mae Formatted File (3.2)")
        return aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_Export_FannieMae_FormattedFile);
      if (menu.ID.ToLowerInvariant().Contains("frdmaccac"))
        return aclManager.GetUserApplicationRight(AclFeature.Freddie_Mac_CAC);
      if (menu.ID.ToLowerInvariant().Contains("fnmaucdtransfer"))
        return aclManager.GetUserApplicationRight(AclFeature.Fannie_Mae_UCD_Transfer);
      return !menu.ID.ToLowerInvariant().Contains("frdmaclpa") || aclManager.GetUserApplicationRight(AclFeature.Freddie_Mac_LPA_Batch);
    }

    private void populateServicesMenuItem()
    {
      List<string> categories = ServicesMapping.Categories;
      ILoanServices service = Session.Application.GetService<ILoanServices>();
      List<ToolStripMenuItem> toolStripMenuItemList = new List<ToolStripMenuItem>();
      FeaturesAclManager aclManager1 = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      ExportServicesAclManager aclManager2 = (ExportServicesAclManager) Session.ACL.GetAclManager(AclCategory.ExportServices);
      ServiceSetting wellsFargoServiceSetting = (ServiceSetting) null;
      foreach (string str in categories)
      {
        List<ServiceSetting> source = new List<ServiceSetting>((IEnumerable<ServiceSetting>) ServicesMapping.GetServiceSetting(str));
        if (str == "GSE Services")
        {
          wellsFargoServiceSetting = source.Where<ServiceSetting>((Func<ServiceSetting, bool>) (setting => setting.ID == "WellsFargo")).FirstOrDefault<ServiceSetting>();
          source.Remove(wellsFargoServiceSetting);
          bool flag = true;
          if (!Session.UserInfo.IsSuperAdministrator() && !Session.UserInfo.IsAdministrator() && Session.EncompassEdition == EncompassEdition.Broker && (aclManager1.GetUserApplicationRight(AclFeature.LoanMgmt_Export_ULAD_ForDu) || aclManager1.GetUserApplicationRight(AclFeature.LoanMgmt_Export_ILAD) || aclManager1.GetUserApplicationRight(AclFeature.LoanMgmt_Export_FannieMae_FormattedFile)))
            flag = false;
          if (((!aclManager1.GetUserApplicationRight(AclFeature.LoanMgmt_GSE_Services) ? 1 : (!aclManager2.GetUserApplicationRightForPipelineServices("GSE Services") ? 1 : 0)) & (flag ? 1 : 0)) != 0)
            continue;
        }
        ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(str);
        foreach (ServiceSetting serviceSetting in source)
        {
          if (serviceSetting.SupportedInCurrentVersion() && service.IsExportServiceAccessible(Session.LoanDataMgr, serviceSetting))
          {
            if (serviceSetting.ToolStripSeparator)
              toolStripMenuItem.DropDownItems.Add((ToolStripItem) new ToolStripSeparator());
            if (aclManager2.GetUserApplicationRightForPipelineServices(str) && (!(str == "GSE Services") || this.hasRights(serviceSetting)))
            {
              ToolStripMenuItem serviceMenuItem = this.createServiceMenuItem(serviceSetting);
              toolStripMenuItem.DropDownItems.Add((ToolStripItem) serviceMenuItem);
            }
          }
        }
        if (str == "GSE Services")
        {
          this.fannieMaeExport.Name = "toolStripMenuItem1";
          this.fannieMaeExport.Size = new Size(204, 22);
          this.fannieMaeExport.Text = "Export Fannie Mae Formatted File (3.2)";
          this.fannieMaeExport.Tag = (object) "SRV_FannieMaeFormattedFile";
          this.fannieMaeExport.Click += new EventHandler(this.toolStripMenuItem1_Click);
          toolStripMenuItem.DropDownItems.Add((ToolStripItem) this.fannieMaeExport);
          if (!Session.UserInfo.IsSuperAdministrator() && (!aclManager1.GetUserApplicationRight(AclFeature.LoanMgmt_Export_FannieMae_FormattedFile) || !aclManager2.GetUserApplicationRightForPipelineServices("GSE Services")))
            this.fannieMaeExport.Visible = false;
          if (this.mainScreen.PipelineScreen != null)
            this.mainScreen.PipelineScreen.exportFannieMae = this.fannieMaeExport;
        }
        if (toolStripMenuItem.DropDownItems.Count > 0)
          toolStripMenuItemList.Add(toolStripMenuItem);
      }
      toolStripMenuItemList.AddRange((IEnumerable<ToolStripMenuItem>) this.ShowDataDocsMenus(aclManager1, wellsFargoServiceSetting));
      if (toolStripMenuItemList.Count <= 0)
        return;
      this.pipelineHeadItem.DropDownItems.Add((ToolStripItem) this.toolStripSeparatorEx21);
      this.pipelineHeadItem.DropDownItems.AddRange((ToolStripItem[]) toolStripMenuItemList.ToArray());
    }

    private void PollyExMenuItem_Click(object sender, EventArgs e) => this.ShowPollyExBrowser();

    private void toolStripMenuItem1_Click(object sender, EventArgs e)
    {
      this.mainScreen.PipelineScreenBrowser.toolStripMenuItem1_Click(sender, e);
    }

    private ToolStripMenuItem createServiceMenuItem(ServiceSetting service)
    {
      ToolStripMenuItem serviceMenuItem = new ToolStripMenuItem(service.DisplayName);
      if (service.LoanFileSpecific)
      {
        ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem("&Selected Loans Only...");
        toolStripMenuItem1.Click += new EventHandler(this.pipeline_Click);
        ServiceSetting serviceSetting1 = service.Clone();
        serviceSetting1.Tag = (object) "Selected";
        toolStripMenuItem1.Tag = (object) serviceSetting1;
        ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem("&All Loans on All Pages...");
        toolStripMenuItem2.Click += new EventHandler(this.pipeline_Click);
        ServiceSetting serviceSetting2 = service.Clone();
        serviceSetting2.Tag = (object) "All";
        toolStripMenuItem2.Tag = (object) serviceSetting2;
        serviceMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
        {
          (ToolStripItem) toolStripMenuItem1,
          (ToolStripItem) toolStripMenuItem2
        });
      }
      else
        serviceMenuItem.Click += new EventHandler(this.pipeline_Click);
      serviceMenuItem.Tag = (object) service;
      return serviceMenuItem;
    }

    private void clearViewMenuItemChecks()
    {
      foreach (ToolStripItem toolStripItem in this.tabPageMenuItemMap.Values)
        toolStripItem.Image = (Image) null;
    }

    public void CheckViewMenuItem(TabPage tabPage)
    {
      this.clearViewMenuItemChecks();
      if (!this.tabPageMenuItemMap.ContainsKey(tabPage.Text))
        return;
      this.tabPageMenuItemMap[tabPage.Text].Image = (Image) Resources.check_mark_green;
    }

    private void MainForm_Resize(object sender, EventArgs e)
    {
      if (!EnConfigurationSettings.GlobalSettings.Debug)
        return;
      this.DisplayHelpText(this.Width.ToString() + "x" + (object) this.Height);
    }

    private void tsHelpDiagnostics_Click(object sender, EventArgs e)
    {
      if (this.tsHelpDiagnostics.Checked)
      {
        DiagnosticSession.DiagnosticsMode = DiagnosticsMode.Disabled;
        EnConfigurationSettings.GlobalSettings.Debug = false;
        int num = (int) Utils.Dialog((IWin32Window) this, "Diagnostic mode has been disabled.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.tsHelpDiagnostics.Checked = false;
      }
      else
      {
        using (DiagnosticsEnablementForm diagnosticsEnablementForm = new DiagnosticsEnablementForm())
        {
          if (diagnosticsEnablementForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          this.tsHelpDiagnostics.Checked = true;
        }
      }
    }

    private void ctxMenuStripSessionID_Opening(object sender, CancelEventArgs e)
    {
      string str = "";
      try
      {
        str = "/" + Session.ServerManager.GetServerDllVersion();
      }
      catch
      {
      }
      this.ctxMenuStripSessionID.Items.Clear();
      this.ctxMenuStripSessionID.Items.Add(Session.ISession.SessionID + str);
    }

    private void statusBar_DoubleClick(object sender, EventArgs e)
    {
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\Encompass"))
      {
        if (registryKey == null)
          return;
        string message = string.Concat(registryKey.GetValue("ThrowException")).Trim();
        if (message != "")
          throw new Exception(message);
      }
    }

    private static string GetSignalFxAPIToken() => "";

    private static string GetSmartClientVersion()
    {
      try
      {
        return VersionInformation.CurrentVersion.DisplayVersionString;
      }
      catch (Exception ex)
      {
      }
      return "";
    }

    private static bool GetSignalFxEnabled()
    {
      try
      {
        return false;
      }
      catch (Exception ex)
      {
      }
      return false;
    }

    private static int GetSignalFxTimeSpan()
    {
      try
      {
        return 0;
      }
      catch (Exception ex)
      {
      }
      return 1000;
    }

    private static string GetInstance()
    {
      return Session.ServerIdentity == null ? string.Empty : Session.ServerIdentity.InstanceName;
    }

    private static string GetCustomer() => Session.CompanyInfo.ClientID;

    private static string GetUserId() => Session.UserID;

    private void tsHelpDDMDiagnostics_Click(object sender, EventArgs e)
    {
      if (this.tsHelpDDMDiagnostics.Checked)
      {
        DDMDiagnosticSession.DiagnosticsMode = DDMDiagnosticsMode.Disabled;
        this.tsHelpDDMDiagnostics.Checked = false;
        int num = (int) Utils.Dialog((IWin32Window) this, "DDM Diagnostic mode has been disabled.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        DDMDiagnosticSession.DiagnosticsMode = DDMDiagnosticsMode.Enabled;
        this.tsHelpDDMDiagnostics.Checked = true;
        int num = (int) Utils.Dialog((IWin32Window) this, "DDM Diagnostic mode has been enabled.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void tsMenuItemJITLogger_Click(object sender, EventArgs e)
    {
      DialogResult dialogResult = DialogResult.OK;
      if (!this.tsMenuItemJITLogger.Checked)
        dialogResult = Utils.Dialog((IWin32Window) this, "To be used for diagnostic purposes only, please contact ICE Mortgage Technology support for instructions and submission of logs. Log file will be deleted upon exit of Encompass session.", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
      if (dialogResult != DialogResult.OK)
        return;
      using (JITLoggerUI jitLoggerUi = new JITLoggerUI())
      {
        int num = (int) jitLoggerUi.ShowDialog((IWin32Window) this);
        this.tsMenuItemJITLogger.Checked = jitLoggerUi.IsDebugEnabled;
      }
    }

    private void setDiagnosticMenuItems()
    {
      this.tsMenuItemJITLogger.Enabled = !this.tsHelpDiagnostics.Checked;
      this.tsHelpDiagnostics.Enabled = !this.tsMenuItemJITLogger.Checked;
    }

    private static void captureAssemblyLoaded(object sender, AssemblyLoadEventArgs args)
    {
      if (!Session.IsConnected)
        return;
      RemoteLogger.Write(TraceLevel.Info, "ASSEMBLY/PLUGIN LOADED: " + args.LoadedAssembly.FullName + ", " + (args.LoadedAssembly.GlobalAssemblyCache ? "GAC" : "APP"));
    }

    public bool IsPipelineTabDefault()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      return Session.StartupInfo.UsePipelineTabStartup && aclManager.GetUserApplicationRight(AclFeature.GlobalTab_Pipeline);
    }

    public enum MenuItemEnum
    {
      eFolder,
      ePASS,
      Loan,
    }

    private delegate void CampaignRefreshStartedCallback(object sender, EventArgs e);

    private delegate void CampaignRefreshCompletedCallback(object sender, EventArgs e);

    public class CustomExceptionHandler
    {
      private const string className = "CustomExceptionHandler";
      protected static string sw = Tracing.SwOutsideLoan;

      public void OnThreadException(object sender, ThreadExceptionEventArgs t)
      {
        DialogResult dialogResult = DialogResult.Cancel;
        try
        {
          dialogResult = this.ShowThreadExceptionDialog(t.Exception);
        }
        catch
        {
          try
          {
            int num = (int) MessageBox.Show("Fatal Error", "Fatal Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Hand);
          }
          finally
          {
            Application.Exit();
          }
        }
        if (dialogResult != DialogResult.Abort)
          return;
        Application.Exit();
      }

      private DialogResult ShowThreadExceptionDialog(Exception e)
      {
        Elli.Metrics.Client.MetricsFactory.IncrementErrorCounter(e, "ThreadException", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\MainForm.cs", nameof (ShowThreadExceptionDialog), 8115);
        Tracing.Log(MainForm.CustomExceptionHandler.sw, nameof (CustomExceptionHandler), TraceLevel.Error, "Unhandled exception caught from thread " + (object) Thread.CurrentThread.GetHashCode() + ": " + (object) e);
        if (this.isHandleableException(e))
          return DialogResult.OK;
        if (!this.isCriticalError(e) || e.Message.IndexOf("Key cannot be null") >= 0 && e.StackTrace.IndexOf("GenuineHttp.HttpClientConnectionManager.Pool_GetConnectionForSending") > 0)
          return DialogResult.Ignore;
        string text = "An error occurred.  Please contact the administrator with the following information:\n\n" + e.Message;
        MessageBoxButtons buttons = MessageBoxButtons.AbortRetryIgnore;
        if (this.isRestartableError(e))
        {
          text = "An error has occured which has prevented this operation from completing successfully. Please try again.";
          buttons = MessageBoxButtons.OK;
        }
        if (this.isOAPIAccessError(e))
        {
          text = "There was an issue processing your request. Please try again shortly. Contact your administrator if the issue continues.";
          buttons = MessageBoxButtons.OK;
        }
        if (EnConfigurationSettings.GlobalSettings.Debug)
          text = text + "\n\n" + (object) e;
        return MessageBox.Show(text, "Application Error", buttons, MessageBoxIcon.Hand);
      }

      private bool isHandleableException(Exception ex)
      {
        bool flag = false;
        if (ex is ComplianceCalendarException || ex.InnerException is ComplianceCalendarException)
        {
          int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, (!(ex is ComplianceCalendarException) ? ex.InnerException : ex).Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          flag = true;
        }
        else if (ex is CountyLimitException || ex.InnerException is CountyLimitException)
        {
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, (!(ex is CountyLimitException) ? ex.InnerException : ex).Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          flag = true;
        }
        else
        {
          switch (ex)
          {
            case EllieMae.EMLite.ClientServer.Exceptions.SecurityException _:
              int num1 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
              flag = true;
              break;
            case EncompassServerConnectionException _:
              int num2 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
              flag = true;
              break;
          }
        }
        return flag;
      }

      private bool isRestartableError(Exception ex)
      {
        return ex is ServerDataException && ex.Message.IndexOf("general network error") >= 0;
      }

      private bool isOAPIAccessError(Exception ex)
      {
        switch (ex)
        {
          case SoapException _:
          case TaskCanceledException _:
            if (ex.Message.IndexOf("invalid_session") >= 0 && ex.StackTrace.IndexOf("OAPIServices.GetAccessToken") >= 0)
              return true;
            break;
        }
        return false;
      }

      private bool isCriticalError(Exception ex)
      {
        return !(ex is OperationException) && ex.StackTrace.IndexOf("ListView.OnHandleDestroyed") < 0 && (!(ex.Message == "Operation is not valid due to the current state of the object.") || ex.StackTrace.IndexOf("MonthCalendar") < 0) && ex.StackTrace.IndexOf("System.Windows.Forms.ChildWindow.Callback") < 0;
      }
    }
  }
}

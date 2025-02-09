// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeManagementConsole
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeManagementConsole : UserControl, IOnlineHelpTarget, ITradeConsole, IMenuProvider
  {
    public static readonly TradeManagementConsole Instance = new TradeManagementConsole();
    private TradeManagementScreen currentScreen;
    private SecurityTradeListScreen securityTradesScreen;
    private SecurityTradeEditor securityTradeEditorScreen;
    private LoanSearchScreen searchScreen;
    private TradeListScreen tradesScreen;
    private TradeEditor editorScreen;
    private ContractListScreen contractsScreen;
    private MbsPoolListScreen mbsPoolsScreen;
    private MbsPoolEditor mbsPoolEditorScreen;
    private FannieMaePEPoolEditor fanniePEPoolEditorScreen;
    private CorrespondentTradeListScreen correspondentTradeScreen;
    private BidTapeRegistration bidTapeRegistration;
    private CorrespondentTradeEditor correspondentTradeEditorScreen;
    private CorrespondentMasterListScreen correspondentMasterScreen;
    private CorrespondentMasterCommitmentEditor correspondentMasterEditorScreen;
    private GSECommitmentListScreen gseCommitmentTradeScreen;
    private GseCommitmentEditor gseCommitmentEditorScreen;
    private bool isReadOnly;
    private IContainer components;
    private Panel panel1;
    private BorderPanel pnlMain;
    private PictureBox btnSearch;
    private PictureBox btnContracts;
    private PictureBox btnTrades;
    private PictureBox btnSecurityTrades;
    private PictureBox btnMbsPools;
    private PictureBox btnCorrespondentTrades;
    private PictureBox btnCorrespondentMasters;
    private PictureBox btnGseCommitments;
    private PictureBox btnBidTapeRegistration;

    private TradeManagementConsole()
    {
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.Visible = true;
      Session.Application.RegisterService((object) this, typeof (ITradeConsole));
    }

    public TradeManagementScreen CurrentScreen
    {
      get => this.currentScreen;
      set => this.SetCurrentScreen(value);
    }

    public void RefreshContents()
    {
      List<PictureBox> pictureBoxList = new List<PictureBox>();
      this.btnMbsPools.Visible = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.EnableMbsPool"]) && Utils.ParseBoolean(Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_MBSPools]);
      this.btnGseCommitments.Visible = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.EnableFMPMandGSE"]) && Utils.ParseBoolean(Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_GSECommitments]);
      this.btnSecurityTrades.Visible = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.EnableTrade"]) && Utils.ParseBoolean(Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_SecurityTrades]);
      this.btnTrades.Visible = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.EnableTrade"]) && Utils.ParseBoolean(Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_LoanTrades]);
      this.btnSearch.Visible = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.EnableTrade"]) && Utils.ParseBoolean(Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_LoanSearch]);
      this.btnContracts.Visible = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.EnableTrade"]) && Utils.ParseBoolean(Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditMasterContracts]);
      this.btnCorrespondentTrades.Visible = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.EnableCorrespondentTrade"]) && Utils.ParseBoolean(Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_CorrespondentTrades]);
      this.btnCorrespondentMasters.Visible = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.EnableCorrespondentMaster"]) && Utils.ParseBoolean(Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_CorrespondentMasters]);
      this.btnBidTapeRegistration.Visible = Utils.ParseBoolean(Session.StartupInfo.PolicySettings[(object) "Policies.ENABLEBIDTAPE"]) && Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.EnableBidTapeRegistration"]) && (bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_BidTapeManagement];
      if (this.btnSecurityTrades.Visible)
        pictureBoxList.Add(this.btnSecurityTrades);
      if (this.btnSearch.Visible)
        pictureBoxList.Add(this.btnSearch);
      if (this.btnTrades.Visible)
        pictureBoxList.Add(this.btnTrades);
      if (this.btnContracts.Visible)
        pictureBoxList.Add(this.btnContracts);
      if (this.btnMbsPools.Visible)
        pictureBoxList.Add(this.btnMbsPools);
      if (this.btnGseCommitments.Visible)
        pictureBoxList.Add(this.btnGseCommitments);
      if (this.btnCorrespondentTrades.Visible)
        pictureBoxList.Add(this.btnCorrespondentTrades);
      if (this.btnCorrespondentMasters.Visible)
        pictureBoxList.Add(this.btnCorrespondentMasters);
      if (this.btnBidTapeRegistration.Visible)
        pictureBoxList.Add(this.btnBidTapeRegistration);
      for (int index = 0; index < pictureBoxList.Count; ++index)
      {
        if (index == 0)
          pictureBoxList[index].Left = 0;
        else
          pictureBoxList[index].Left = pictureBoxList[index - 1].Left + pictureBoxList[index - 1].Width - 1;
      }
      if (!this.btnContracts.Visible && this.currentScreen == TradeManagementScreen.Contracts || !this.btnMbsPools.Visible && this.currentScreen == TradeManagementScreen.MbsPools || !this.btnCorrespondentTrades.Visible && this.currentScreen == TradeManagementScreen.CorrespondentTrades || !this.btnGseCommitments.Visible && this.currentScreen == TradeManagementScreen.GSECommitments || !this.btnBidTapeRegistration.Visible && this.currentScreen == TradeManagementScreen.BidTapeRegistration)
        this.currentScreen = TradeManagementScreen.None;
      if (this.currentScreen != TradeManagementScreen.None)
        return;
      string a = Session.StartupInfo.TradeSettings[(object) "Trade.DefaultTradeTab"] != null ? Session.StartupInfo.TradeSettings[(object) "Trade.DefaultTradeTab"].ToString() : "";
      if (this.btnCorrespondentTrades.Visible && a != string.Empty && string.Equals(a, "correspondenttrades", StringComparison.CurrentCultureIgnoreCase))
        this.CurrentScreen = TradeManagementScreen.CorrespondentTrades;
      else if (this.btnTrades.Visible)
      {
        this.CurrentScreen = TradeManagementScreen.Trades;
      }
      else
      {
        if (!this.btnCorrespondentTrades.Visible)
          return;
        this.CurrentScreen = TradeManagementScreen.CorrespondentTrades;
      }
    }

    public void NavigateToTradeObjectPage(TradeManagementScreen screen, int id)
    {
      if (!this.SetCurrentScreen(screen) || id < 0)
        return;
      switch (screen)
      {
        case TradeManagementScreen.Trades:
        case TradeManagementScreen.TradeEditor:
          if (Session.LoanTradeManager.GetTrade(id) != null)
            break;
          int num1 = (int) Utils.Dialog((IWin32Window) this, "This is an invalid loan trade.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          break;
        case TradeManagementScreen.SecurityTrades:
        case TradeManagementScreen.SecurityTradeEditor:
          SecurityTradeInfo trade1 = Session.SecurityTradeManager.GetTrade(id);
          if (trade1 == null)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "This is an invalid security trade.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          this.OpenSecurityTrade(trade1);
          break;
        case TradeManagementScreen.MbsPools:
        case TradeManagementScreen.MbsPoolEditor:
          MbsPoolInfo trade2 = Session.MbsPoolManager.GetTrade(id);
          if (trade2 == null)
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "This is an invalid MBS Pool.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          this.OpenMbsPool(trade2);
          break;
        case TradeManagementScreen.CorrespondentTrades:
        case TradeManagementScreen.CorrespondentTradeEditor:
          if (Session.CorrespondentTradeManager.GetTrade(id) != null)
            break;
          int num4 = (int) Utils.Dialog((IWin32Window) this, "This is an invalid correspondent loan trade.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          break;
      }
    }

    public bool SetCurrentScreen(TradeManagementScreen screen) => this.SetCurrentScreen(screen, 0);

    public bool SetCurrentScreen(TradeManagementScreen screen, int sqlRead)
    {
      if (this.currentScreen == TradeManagementScreen.SecurityTradeEditor)
      {
        if (!this.validateCloseSecurityTrade())
          return false;
      }
      else if (this.currentScreen == TradeManagementScreen.TradeEditor)
      {
        if (!this.validateCloseTrade())
          return false;
      }
      else if (this.currentScreen == TradeManagementScreen.MbsPoolEditor)
      {
        if (!this.validateCloseMbsPool())
          return false;
      }
      else if (this.currentScreen == TradeManagementScreen.FannieMaePEPoolEditor)
      {
        if (!this.validateCloseFanniePEPool())
          return false;
      }
      else if (this.currentScreen == TradeManagementScreen.Contracts)
      {
        if (!this.validateCloseContract())
          return false;
      }
      else if (this.currentScreen == TradeManagementScreen.CorrespondentTradeEditor)
      {
        if (!this.validateCloseCorrespondentTrade())
          return false;
      }
      else if (this.currentScreen == TradeManagementScreen.CorrespondentMasterEditor)
      {
        if (!this.validateCloseCorrespondentMaster())
          return false;
      }
      else if (this.currentScreen == TradeManagementScreen.GSECommitmentEditor && !this.validateCloseGseCommitment())
        return false;
      Cursor.Current = Cursors.WaitCursor;
      this.btnSecurityTrades.Image = (Image) Resources.btn_security_trades;
      this.btnSearch.Image = (Image) Resources.btn_loan_search;
      this.btnTrades.Image = (Image) Resources.btn_loan_trades;
      this.btnMbsPools.Image = (Image) Resources.btn_mbs_pools;
      this.btnGseCommitments.Image = (Image) Resources.GSE_Commitment_Tab_Normal;
      this.btnContracts.Image = (Image) Resources.btn_master_contracts;
      this.btnCorrespondentTrades.Image = (Image) Resources.correspondent_trades_tab;
      this.btnCorrespondentMasters.Image = (Image) Resources.btn_correspondent_masters;
      this.btnBidTapeRegistration.Image = (Image) Resources.btn_bid_tape_management;
      switch (screen)
      {
        case TradeManagementScreen.Search:
          this.btnSearch.Image = (Image) Resources.btn_loan_search_selected;
          if (this.searchScreen == null)
            this.searchScreen = new LoanSearchScreen(typeof (LoanTradeFilterEvaluator));
          this.displayControl((Control) this.searchScreen);
          this.searchScreen.RefreshContents(sqlRead);
          this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditLoanSearch];
          if (this.isReadOnly)
          {
            this.searchScreen.IsReadOnly = true;
            this.searchScreen.SetReadOnly();
          }
          this.setMenu("Trades - LoanSearch");
          break;
        case TradeManagementScreen.Trades:
          this.btnTrades.Image = (Image) Resources.btn_loan_trades_selected;
          if (this.tradesScreen == null)
            this.tradesScreen = new TradeListScreen();
          this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditLoanTrades];
          if (this.isReadOnly)
          {
            this.tradesScreen.IsReadOnly = true;
            this.tradesScreen.RefreshContents();
            this.tradesScreen.SetReadOnly();
          }
          else
            this.tradesScreen.RefreshContents();
          this.displayControl((Control) this.tradesScreen);
          this.setMenu("Trades - TradeList");
          break;
        case TradeManagementScreen.Contracts:
          this.btnContracts.Image = (Image) Resources.btn_master_contracts_selected;
          if (this.contractsScreen == null)
            this.contractsScreen = new ContractListScreen();
          this.displayControl((Control) this.contractsScreen);
          this.contractsScreen.RefreshContents();
          this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditMasterContracts];
          if (this.isReadOnly)
            this.contractsScreen.SetReadOnly();
          this.setMenu("Trades - MasterContracts");
          break;
        case TradeManagementScreen.SecurityTrades:
          if (this.securityTradesScreen == null)
            this.securityTradesScreen = new SecurityTradeListScreen();
          this.displayControl((Control) this.securityTradesScreen);
          this.securityTradesScreen.RefreshContents();
          this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditSecurityTrades];
          if (this.isReadOnly)
          {
            this.securityTradesScreen.IsReadOnly = true;
            this.securityTradesScreen.SetReadOnly();
          }
          this.btnSecurityTrades.Image = (Image) Resources.btn_security_trades_selected;
          this.setMenu("Trades - SecurityTradeList");
          break;
        case TradeManagementScreen.MbsPools:
          this.btnMbsPools.Image = (Image) Resources.btn_mbs_pools_selected;
          if (this.mbsPoolsScreen == null)
            this.mbsPoolsScreen = new MbsPoolListScreen();
          this.displayControl((Control) this.mbsPoolsScreen);
          this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditMBSPools];
          if (this.isReadOnly)
          {
            this.mbsPoolsScreen.IsReadOnly = true;
            this.mbsPoolsScreen.RefreshContents();
            this.mbsPoolsScreen.SetReadOnly();
          }
          else
            this.mbsPoolsScreen.RefreshContents();
          this.setMenu("Trades - MbsPoolList");
          break;
        case TradeManagementScreen.CorrespondentMasters:
          this.btnCorrespondentMasters.Image = (Image) Resources.btn_correspondent_masters_selected;
          if (this.correspondentMasterScreen == null)
            this.correspondentMasterScreen = new CorrespondentMasterListScreen();
          this.displayControl((Control) this.correspondentMasterScreen);
          this.correspondentMasterScreen.RefreshContents();
          this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditCorrespondentMasters];
          if (this.isReadOnly)
          {
            this.correspondentMasterScreen.IsReadOnly = true;
            this.correspondentMasterScreen.RefreshContents();
            this.correspondentMasterScreen.SetReadOnly();
          }
          else
            this.correspondentMasterScreen.RefreshContents();
          this.setMenu("Trades - CorrespondentMasters");
          break;
        case TradeManagementScreen.CorrespondentTrades:
          if (this.correspondentTradeScreen == null)
            this.correspondentTradeScreen = new CorrespondentTradeListScreen();
          this.displayControl((Control) this.correspondentTradeScreen);
          this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditCorrespondentTrades];
          if (this.isReadOnly)
          {
            this.correspondentTradeScreen.IsReadOnly = true;
            this.correspondentTradeScreen.RefreshContents();
            this.correspondentTradeScreen.SetReadOnly();
          }
          else
            this.correspondentTradeScreen.RefreshContents();
          this.setMenu("Trades - CorrespondentTrades");
          this.btnCorrespondentTrades.Image = (Image) Resources.correspondent_trades_tab_selected;
          break;
        case TradeManagementScreen.FannieMaePEPoolEditor:
          this.btnMbsPools.Image = (Image) Resources.btn_mbs_pools_selected;
          if (this.mbsPoolsScreen == null)
            this.mbsPoolsScreen = new MbsPoolListScreen();
          this.displayControl((Control) this.mbsPoolsScreen);
          this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditMBSPools];
          if (this.isReadOnly)
          {
            this.mbsPoolsScreen.IsReadOnly = true;
            this.mbsPoolsScreen.RefreshContents();
            this.mbsPoolsScreen.SetReadOnly();
          }
          else
            this.mbsPoolsScreen.RefreshContents();
          this.setMenu("Trades - FannieMaePEMbsPoolList");
          break;
        case TradeManagementScreen.GSECommitments:
          if (this.gseCommitmentTradeScreen == null)
            this.gseCommitmentTradeScreen = new GSECommitmentListScreen();
          this.displayControl((Control) this.gseCommitmentTradeScreen);
          this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditGSECommitments];
          if (this.isReadOnly)
          {
            this.gseCommitmentTradeScreen.IsReadOnly = true;
            this.gseCommitmentTradeScreen.RefreshContents();
            this.gseCommitmentTradeScreen.SetReadOnly();
          }
          else
            this.gseCommitmentTradeScreen.RefreshContents();
          this.setMenu("Trades - GSECommitments");
          this.btnGseCommitments.Image = (Image) Resources.GSE_Commitment_Tab_Active;
          break;
        case TradeManagementScreen.BidTapeRegistration:
          if (this.bidTapeRegistration == null)
            this.bidTapeRegistration = new BidTapeRegistration();
          this.displayControl((Control) this.bidTapeRegistration);
          this.setMenu("Trades - BidTapeRegistration");
          this.btnBidTapeRegistration.Image = (Image) Resources.btn_bid_tape_management_selected;
          break;
      }
      this.currentScreen = screen;
      Cursor.Current = Cursors.Default;
      return true;
    }

    private void setMenu(string menuName)
    {
      Session.Application.GetService<IEncompassApplication>().SetMenu(menuName);
    }

    private bool validateCloseSecurityTrade()
    {
      if (this.isReadOnly || !this.securityTradeEditorScreen.DataModified)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "The current trade has unsaved changes. Save those changes now?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.Yes:
          if (!this.securityTradeEditorScreen.SaveTrade())
            return false;
          break;
        case DialogResult.No:
          return true;
      }
      return true;
    }

    public bool OpenSecurityTrade(int tradeId)
    {
      Cursor.Current = Cursors.WaitCursor;
      return this.OpenSecurityTrade(Session.SecurityTradeManager.GetTrade(tradeId) ?? throw new Exception("The specified trade cannot be found."));
    }

    public bool OpenSecurityTrade(SecurityTradeInfo trade)
    {
      return this.OpenSecurityTrade(trade, (string[]) null);
    }

    public bool OpenSecurityTrade(SecurityTradeInfo trade, string[] tradeGuids)
    {
      if (!this.QueryCloseConsole())
        return false;
      Cursor.Current = Cursors.WaitCursor;
      if (this.securityTradeEditorScreen == null)
        this.securityTradeEditorScreen = new SecurityTradeEditor();
      this.displayControl((Control) this.securityTradeEditorScreen);
      this.securityTradeEditorScreen.RefreshData(trade, tradeGuids);
      this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditSecurityTrades];
      if (this.isReadOnly)
        this.securityTradeEditorScreen.ReadOnly = true;
      this.btnSearch.Image = (Image) Resources.btn_loan_search;
      this.btnSecurityTrades.Image = (Image) Resources.btn_security_trades_selected;
      this.btnTrades.Image = (Image) Resources.btn_loan_trades;
      this.btnMbsPools.Image = (Image) Resources.btn_mbs_pools;
      this.btnGseCommitments.Image = (Image) Resources.GSE_Commitment_Tab_Normal;
      this.btnContracts.Image = (Image) Resources.btn_master_contracts;
      this.btnCorrespondentMasters.Image = (Image) Resources.btn_correspondent_masters;
      this.btnCorrespondentTrades.Image = (Image) Resources.correspondent_trades_tab;
      this.btnBidTapeRegistration.Image = (Image) Resources.btn_bid_tape_management;
      this.currentScreen = TradeManagementScreen.SecurityTradeEditor;
      this.setMenu("Trades - SecurityTradeEditor");
      Cursor.Current = Cursors.Default;
      return true;
    }

    public void OpenNewSecurityTrade() => this.OpenSecurityTrade(new SecurityTradeInfo());

    public void CloseSecurityTrade() => this.CurrentScreen = TradeManagementScreen.SecurityTrades;

    private bool queryCloseCurrentSecurityTrade()
    {
      if (!this.securityTradeEditorScreen.DataModified)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "Would you like to save your changes to the current trade?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.No:
          return true;
        default:
          return this.securityTradeEditorScreen.SaveTrade();
      }
    }

    public bool OpenCorrespondentMaster(int contractId)
    {
      Cursor.Current = Cursors.WaitCursor;
      return this.OpenCorrespondentMaster(Session.CorrespondentMasterManager.GetCorrespondentMaster(contractId) ?? throw new Exception("The specified correspondent master commitment cannot be found."));
    }

    public bool OpenCorrespondentMaster(CorrespondentMasterInfo master)
    {
      if (!this.QueryCloseConsole())
        return false;
      Cursor.Current = Cursors.WaitCursor;
      if (this.correspondentMasterEditorScreen == null)
        this.correspondentMasterEditorScreen = new CorrespondentMasterCommitmentEditor();
      this.displayControl((Control) this.correspondentMasterEditorScreen);
      this.correspondentMasterEditorScreen.RefreshData(master);
      this.correspondentMasterEditorScreen.InitializeDateRangeValidationEvents();
      this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditCorrespondentMasters];
      if (this.isReadOnly)
        this.correspondentMasterEditorScreen.ReadOnly = true;
      this.btnSearch.Image = (Image) Resources.btn_loan_search;
      this.btnSecurityTrades.Image = (Image) Resources.btn_security_trades;
      this.btnTrades.Image = (Image) Resources.btn_loan_trades;
      this.btnMbsPools.Image = (Image) Resources.btn_mbs_pools;
      this.btnGseCommitments.Image = (Image) Resources.GSE_Commitment_Tab_Normal;
      this.btnContracts.Image = (Image) Resources.btn_master_contracts;
      this.btnCorrespondentMasters.Image = (Image) Resources.btn_correspondent_masters_selected;
      this.btnCorrespondentTrades.Image = (Image) Resources.correspondent_trades_tab;
      this.btnBidTapeRegistration.Image = (Image) Resources.btn_bid_tape_management;
      this.currentScreen = TradeManagementScreen.CorrespondentMasterEditor;
      this.setMenu("Trades - CorrespondentMasterEditor");
      Cursor.Current = Cursors.Default;
      return true;
    }

    private bool validateCloseCorrespondentMaster()
    {
      if (this.isReadOnly)
        return true;
      if (!this.correspondentMasterEditorScreen.DataModified)
      {
        this.correspondentMasterEditorScreen.DisconnectDateRangeValidationEvents();
        return true;
      }
      switch (Utils.Dialog((IWin32Window) this, "The current correspondent master has unsaved changes. Save those changes now?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.Yes:
          if (!this.correspondentMasterEditorScreen.PreValidateCommit() || !this.correspondentMasterEditorScreen.SaveCorrespondentMasterEditor())
            return false;
          this.correspondentMasterEditorScreen.DisconnectDateRangeValidationEvents();
          break;
        case DialogResult.No:
          this.correspondentMasterEditorScreen.DisconnectDateRangeValidationEvents();
          return true;
      }
      this.correspondentMasterEditorScreen.DisconnectDateRangeValidationEvents();
      return true;
    }

    public void CloseCorrespondentMaster()
    {
      this.CurrentScreen = TradeManagementScreen.CorrespondentMasters;
    }

    private bool queryCloseCurrentCorrespondentMaster()
    {
      if (this.correspondentMasterEditorScreen == null || !this.correspondentMasterEditorScreen.DataModified)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "Would you like to save your changes to the current correspondent master?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.No:
          return true;
        default:
          return this.correspondentMasterEditorScreen.SaveCorrespondentMasterEditor();
      }
    }

    private bool validateCloseTrade()
    {
      if (this.isReadOnly || !this.editorScreen.DataModified)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "The current trade has unsaved changes. Save those changes now?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.Yes:
          if (!this.editorScreen.SaveTrade())
            return false;
          break;
        case DialogResult.No:
          return true;
      }
      return true;
    }

    public bool OpenTrade(int tradeId, bool gotoHistory = false)
    {
      return this.OpenTrade(tradeId, 0, gotoHistory);
    }

    public bool OpenTrade(int tradeId, int sqlRead, bool gotoHistory = false)
    {
      Cursor.Current = Cursors.WaitCursor;
      return this.OpenTrade(Session.LoanTradeManager.GetTrade(tradeId) ?? throw new Exception("The specified trade cannot be found."), sqlRead, gotoHistory);
    }

    public bool OpenTrade(LoanTradeInfo trade, int sqlRead, bool gotoHistory = false)
    {
      return this.OpenTrade(trade, (string[]) null, sqlRead, gotoHistory);
    }

    public bool OpenTrade(SecurityTradeInfo securityTrade, int sqlRead)
    {
      if (!this.QueryCloseConsole())
        return false;
      Cursor.Current = Cursors.WaitCursor;
      if (this.editorScreen == null)
        this.editorScreen = new TradeEditor();
      this.displayControl((Control) this.editorScreen);
      this.editorScreen.RefreshData(securityTrade, sqlRead);
      this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditLoanTrades];
      if (this.isReadOnly)
        this.editorScreen.ReadOnly = true;
      this.btnSearch.Image = (Image) Resources.btn_loan_search;
      this.btnSecurityTrades.Image = (Image) Resources.btn_security_trades;
      this.btnTrades.Image = (Image) Resources.btn_loan_trades_selected;
      this.btnMbsPools.Image = (Image) Resources.btn_mbs_pools;
      this.btnGseCommitments.Image = (Image) Resources.GSE_Commitment_Tab_Normal;
      this.btnContracts.Image = (Image) Resources.btn_master_contracts;
      this.btnCorrespondentMasters.Image = (Image) Resources.btn_correspondent_masters;
      this.btnCorrespondentTrades.Image = (Image) Resources.correspondent_trades_tab;
      this.btnBidTapeRegistration.Image = (Image) Resources.btn_bid_tape_management;
      this.currentScreen = TradeManagementScreen.TradeEditor;
      this.setMenu("Trades - TradeEditor");
      Cursor.Current = Cursors.Default;
      return true;
    }

    public bool OpenTrade(LoanTradeInfo trade, string[] loanGuids, int sqlRead, bool gotoHistory = false)
    {
      if (!this.QueryCloseConsole())
        return false;
      Cursor.Current = Cursors.WaitCursor;
      if (this.editorScreen == null)
        this.editorScreen = new TradeEditor();
      this.displayControl((Control) this.editorScreen);
      this.editorScreen.RefreshData(trade, loanGuids, sqlRead);
      this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditLoanTrades];
      if (this.isReadOnly)
        this.editorScreen.ReadOnly = true;
      this.btnSearch.Image = (Image) Resources.btn_loan_search;
      this.btnSecurityTrades.Image = (Image) Resources.btn_security_trades;
      this.btnTrades.Image = (Image) Resources.btn_loan_trades_selected;
      this.btnMbsPools.Image = (Image) Resources.btn_mbs_pools;
      this.btnGseCommitments.Image = (Image) Resources.GSE_Commitment_Tab_Normal;
      this.btnContracts.Image = (Image) Resources.btn_master_contracts;
      this.btnCorrespondentMasters.Image = (Image) Resources.btn_correspondent_masters;
      this.btnCorrespondentTrades.Image = (Image) Resources.correspondent_trades_tab;
      this.btnBidTapeRegistration.Image = (Image) Resources.btn_bid_tape_management;
      this.currentScreen = TradeManagementScreen.TradeEditor;
      this.setMenu("Trades - TradeEditor");
      if (gotoHistory)
        this.editorScreen.SetNoteHistoryTab();
      Cursor.Current = Cursors.Default;
      return true;
    }

    public void OpenGSECommitment()
    {
      Cursor.Current = Cursors.WaitCursor;
      if (this.gseCommitmentEditorScreen == null)
        this.gseCommitmentEditorScreen = new GseCommitmentEditor();
      this.displayControl((Control) this.gseCommitmentEditorScreen);
      this.gseCommitmentEditorScreen.RefreshData(new GSECommitmentInfo());
      this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditGSECommitments];
      if (this.isReadOnly)
        this.gseCommitmentEditorScreen.ReadOnly = true;
      this.btnSearch.Image = (Image) Resources.btn_loan_search;
      this.btnSecurityTrades.Image = (Image) Resources.btn_security_trades;
      this.btnTrades.Image = (Image) Resources.btn_loan_trades;
      this.btnMbsPools.Image = (Image) Resources.btn_mbs_pools;
      this.btnContracts.Image = (Image) Resources.btn_master_contracts;
      this.btnGseCommitments.Image = (Image) Resources.GSE_Commitment_Tab_Active;
      this.btnCorrespondentMasters.Image = (Image) Resources.btn_correspondent_masters;
      this.btnCorrespondentTrades.Image = (Image) Resources.correspondent_trades_tab;
      this.btnBidTapeRegistration.Image = (Image) Resources.btn_bid_tape_management;
      this.currentScreen = TradeManagementScreen.GSECommitmentEditor;
      this.setMenu("Trades - GSECommitmentEditor");
      Cursor.Current = Cursors.Default;
    }

    public void OpenNewTrade() => this.OpenTrade(new LoanTradeInfo(), 0);

    public void CloseTrade() => this.CurrentScreen = TradeManagementScreen.Trades;

    private bool queryCloseCurrentTrade()
    {
      if (this.editorScreen == null || !this.editorScreen.DataModified)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "Would you like to save your changes to the current trade?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.No:
          return true;
        default:
          return this.editorScreen.SaveTrade();
      }
    }

    private bool queryCloseCurrentCorrespondentTrade()
    {
      if (this.correspondentTradeEditorScreen == null)
        return true;
      if (!this.correspondentTradeEditorScreen.DataModified)
        return true;
      if (!this.correspondentTradeEditorScreen.PreValidateCommit())
        return false;
      bool silentLoanUpdate;
      DialogResult dialogResult;
      if (this.correspondentTradeEditorScreen.IsCommitmentNumberChanged && this.correspondentTradeEditorScreen.UnsavedPendingLoan)
      {
        silentLoanUpdate = true;
        dialogResult = Utils.Dialog((IWin32Window) this, "The current trade has unsaved changes. Save those changes and update the loan files with these recent changes now?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
      }
      else
      {
        silentLoanUpdate = false;
        dialogResult = Utils.Dialog((IWin32Window) this, "The current correspondent trade has unsaved changes. Save those changes now?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
      }
      switch (dialogResult)
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.Yes:
          if (!this.correspondentTradeEditorScreen.SaveCorrespondentTrade(silentLoanUpdate))
            return false;
          break;
        case DialogResult.No:
          return true;
      }
      return true;
    }

    private bool queryCloseCurrentMaterCommitment()
    {
      if (this.correspondentMasterEditorScreen == null || !this.correspondentMasterEditorScreen.DataModified)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "Would you like to save your changes to the current master commitment?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.No:
          return true;
        default:
          return this.correspondentMasterEditorScreen.SaveCorrespondentMasterEditor();
      }
    }

    private bool validateCloseMbsPool()
    {
      if (this.isReadOnly || !this.mbsPoolEditorScreen.DataModified)
        return true;
      if (!this.mbsPoolEditorScreen.PreValidateCommit())
        return false;
      switch (Utils.Dialog((IWin32Window) this, "The current MBS pool has unsaved changes. Save those changes now?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.Yes:
          if (!this.mbsPoolEditorScreen.SaveTrade())
            return false;
          break;
        case DialogResult.No:
          return true;
      }
      return true;
    }

    private bool validateCloseFanniePEPool()
    {
      if (this.isReadOnly || !this.fanniePEPoolEditorScreen.DataModified)
        return true;
      if (!this.fanniePEPoolEditorScreen.PreValidateCommit())
        return false;
      switch (Utils.Dialog((IWin32Window) this, "The current Fannie Mae PE MBS pool has unsaved changes. Save those changes now?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.Yes:
          if (!this.fanniePEPoolEditorScreen.SaveTrade())
            return false;
          break;
        case DialogResult.No:
          return true;
      }
      return true;
    }

    public bool OpenMbsPool(int tradeId, bool gotoHisotryTab = false)
    {
      Cursor.Current = Cursors.WaitCursor;
      return this.OpenMbsPool(Session.MbsPoolManager.GetTrade(tradeId) ?? throw new Exception("The specified pool cannot be found."), gotoHisotryTab);
    }

    public bool OpenMbsPool(MbsPoolInfo trade, bool gotoHisotryTab = false)
    {
      return this.OpenMbsPool(trade, (string[]) null, gotoHisotryTab);
    }

    public bool OpenMbsPool(MbsPoolInfo trade, string[] loanGuids, bool gotoHisotryTab = false)
    {
      if (!this.QueryCloseConsole())
        return false;
      Cursor.Current = Cursors.WaitCursor;
      trade.TradeType = TradeType.MbsPool;
      if (trade.PoolMortgageType == MbsPoolMortgageType.FannieMaePE)
      {
        if (this.fanniePEPoolEditorScreen == null)
          this.fanniePEPoolEditorScreen = new FannieMaePEPoolEditor();
        this.displayControl((Control) this.fanniePEPoolEditorScreen);
        this.fanniePEPoolEditorScreen.RefreshData(trade, loanGuids);
        this.currentScreen = TradeManagementScreen.FannieMaePEPoolEditor;
        this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditMBSPools];
        if (this.isReadOnly)
          this.fanniePEPoolEditorScreen.ReadOnly = true;
        if (gotoHisotryTab)
          this.fanniePEPoolEditorScreen.SetNoteHistoryTab();
      }
      else
      {
        if (this.mbsPoolEditorScreen == null)
          this.mbsPoolEditorScreen = new MbsPoolEditor();
        this.displayControl((Control) this.mbsPoolEditorScreen);
        this.mbsPoolEditorScreen.RefreshData(trade, loanGuids);
        this.currentScreen = TradeManagementScreen.MbsPoolEditor;
        this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditMBSPools];
        if (this.isReadOnly)
          this.mbsPoolEditorScreen.ReadOnly = true;
        if (gotoHisotryTab)
          this.mbsPoolEditorScreen.SetNoteHistoryTab();
      }
      this.btnSearch.Image = (Image) Resources.btn_loan_search;
      this.btnSecurityTrades.Image = (Image) Resources.btn_security_trades;
      this.btnTrades.Image = (Image) Resources.btn_loan_trades;
      this.btnMbsPools.Image = (Image) Resources.btn_mbs_pools_selected;
      this.btnGseCommitments.Image = (Image) Resources.GSE_Commitment_Tab_Normal;
      this.btnContracts.Image = (Image) Resources.btn_master_contracts;
      this.btnCorrespondentMasters.Image = (Image) Resources.btn_correspondent_masters;
      this.btnCorrespondentTrades.Image = (Image) Resources.correspondent_trades_tab;
      this.btnBidTapeRegistration.Image = (Image) Resources.btn_bid_tape_management;
      this.setMenu("Trades - MbsPoolEditor");
      Cursor.Current = Cursors.Default;
      return true;
    }

    public void OpenNewMbsPool(MbsPoolMortgageType poolMortgageType)
    {
      this.OpenMbsPool(new MbsPoolInfo(poolMortgageType));
    }

    public void CloseMbsPool() => this.CurrentScreen = TradeManagementScreen.MbsPools;

    private bool queryCloseCurrentMbsPool()
    {
      if (this.mbsPoolEditorScreen == null || !this.mbsPoolEditorScreen.DataModified)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "Would you like to save your changes to the current MBS Pool?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.No:
          return true;
        default:
          return this.mbsPoolEditorScreen.SaveTrade();
      }
    }

    private bool queryCloseCurrentFanniePEPool()
    {
      if (this.fanniePEPoolEditorScreen == null || !this.fanniePEPoolEditorScreen.DataModified)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "Would you like to save your changes to the current Fannie Mae PE MBS Pool?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.No:
          return true;
        default:
          return this.fanniePEPoolEditorScreen.SaveTrade();
      }
    }

    private bool validateCloseContract()
    {
      if (this.isReadOnly || !this.contractsScreen.DataModified)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "The current contract has unsaved changes. Save those changes now?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.Yes:
          if (!this.contractsScreen.SaveContract())
            return false;
          break;
        case DialogResult.No:
          return true;
      }
      return true;
    }

    private bool queryCloseCurrentContract()
    {
      if (!this.contractsScreen.DataModified)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "Would you like to save your changes to the current contract?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.No:
          return true;
        default:
          return this.contractsScreen.SaveContract();
      }
    }

    public bool OpenCorrespondentTrade(int tradeId, bool gotoHistoryTab = false)
    {
      Cursor.Current = Cursors.WaitCursor;
      return this.OpenCorrespondentTrade(Session.CorrespondentTradeManager.GetTrade(tradeId) ?? throw new Exception("The specified trade cannot be found."), gotoHistoryTab);
    }

    public bool OpenCorrespondentTrade(CorrespondentTradeInfo trade, bool gotoHistoryTab = false)
    {
      return this.OpenCorrespondentTrade(trade, (string[]) null, gotoHistoryTab);
    }

    public bool OpenCorrespondentDuplicateTrade(CorrespondentTradeInfo trade, bool gotoHistoryTab = false)
    {
      this.OpenCorrespondentTrade(trade, (string[]) null, gotoHistoryTab);
      this.correspondentTradeEditorScreen.InitializeDuplicateTrade(trade);
      return true;
    }

    public bool OpenCorrespondentTrade(
      CorrespondentTradeInfo trade,
      string[] loanGuids,
      bool gotoHistoryTab = false)
    {
      if (!this.QueryCloseConsole())
        return false;
      Cursor.Current = Cursors.WaitCursor;
      if (this.correspondentTradeEditorScreen == null)
        this.correspondentTradeEditorScreen = new CorrespondentTradeEditor();
      this.displayControl((Control) this.correspondentTradeEditorScreen);
      trade.TradeType = TradeType.CorrespondentTrade;
      this.correspondentTradeEditorScreen.RefreshData(trade, loanGuids);
      this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditCorrespondentTrades];
      if (this.isReadOnly)
        this.correspondentTradeEditorScreen.ReadOnly = true;
      this.btnSearch.Image = (Image) Resources.btn_loan_search;
      this.btnSecurityTrades.Image = (Image) Resources.btn_security_trades;
      this.btnTrades.Image = (Image) Resources.btn_loan_trades;
      this.btnMbsPools.Image = (Image) Resources.btn_mbs_pools;
      this.btnGseCommitments.Image = (Image) Resources.GSE_Commitment_Tab_Normal;
      this.btnContracts.Image = (Image) Resources.btn_master_contracts;
      this.btnCorrespondentMasters.Image = (Image) Resources.btn_correspondent_masters;
      this.btnCorrespondentTrades.Image = (Image) Resources.correspondent_trades_tab_selected;
      this.btnBidTapeRegistration.Image = (Image) Resources.btn_bid_tape_management;
      this.currentScreen = TradeManagementScreen.CorrespondentTradeEditor;
      this.setMenu("Trades - CorrespondentTradeEditor");
      if (gotoHistoryTab)
        this.correspondentTradeEditorScreen.SetNoteHistoryTab();
      Cursor.Current = Cursors.Default;
      return true;
    }

    private bool validateCloseCorrespondentTrade()
    {
      if (this.isReadOnly)
        return true;
      if (!this.correspondentTradeEditorScreen.DataModified)
      {
        this.correspondentTradeEditorScreen.PerformPublishTradeCheck();
        return true;
      }
      if (!this.correspondentTradeEditorScreen.PreValidateCommit())
        return false;
      bool silentLoanUpdate;
      DialogResult dialogResult;
      if (this.correspondentTradeEditorScreen.IsCommitmentNumberChanged && this.correspondentTradeEditorScreen.UnsavedPendingLoan)
      {
        silentLoanUpdate = true;
        dialogResult = Utils.Dialog((IWin32Window) this, "The current trade has unsaved changes. Save those changes and update the loan files with these recent changes now?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
      }
      else
      {
        silentLoanUpdate = false;
        dialogResult = Utils.Dialog((IWin32Window) this, "The current correspondent trade has unsaved changes. Save those changes now?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
      }
      switch (dialogResult)
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.Yes:
          if (!this.correspondentTradeEditorScreen.SaveCorrespondentTrade(silentLoanUpdate))
            return false;
          break;
        case DialogResult.No:
          this.correspondentTradeEditorScreen.PerformPublishTradeCheck();
          return true;
      }
      this.correspondentTradeEditorScreen.PerformPublishTradeCheck();
      return true;
    }

    public void CloseCorrespondentTrade()
    {
      this.CurrentScreen = TradeManagementScreen.CorrespondentTrades;
    }

    private bool validateCloseGseCommitment()
    {
      if (this.isReadOnly || !this.gseCommitmentEditorScreen.DataModified)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "The current GSE Commitment has unsaved changes. Save those changes now?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.Yes:
          if (!this.gseCommitmentEditorScreen.SaveTrade())
            return false;
          break;
        case DialogResult.No:
          return true;
      }
      return true;
    }

    public bool OpenGseCommitment(int tradeId)
    {
      Cursor.Current = Cursors.WaitCursor;
      return this.OpenGseCommitment(Session.GseCommitmentManager.GetTrade(tradeId) ?? throw new Exception("The specified trade cannot be found."));
    }

    public bool OpenGseCommitment(GSECommitmentInfo trade)
    {
      return this.OpenGseCommitment(trade, (string[]) null);
    }

    public bool OpenGseCommitment(GSECommitmentInfo trade, string[] tradeGuids)
    {
      if (!this.QueryCloseConsole())
        return false;
      Cursor.Current = Cursors.WaitCursor;
      if (this.gseCommitmentEditorScreen == null)
        this.gseCommitmentEditorScreen = new GseCommitmentEditor();
      this.displayControl((Control) this.gseCommitmentEditorScreen);
      this.gseCommitmentEditorScreen.RefreshData(trade, tradeGuids);
      this.isReadOnly = !(bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.TradeTab_EditGSECommitments];
      if (this.isReadOnly)
        this.gseCommitmentEditorScreen.ReadOnly = true;
      this.btnSearch.Image = (Image) Resources.btn_loan_search;
      this.btnSecurityTrades.Image = (Image) Resources.btn_security_trades;
      this.btnTrades.Image = (Image) Resources.btn_loan_trades;
      this.btnMbsPools.Image = (Image) Resources.btn_mbs_pools;
      this.btnGseCommitments.Image = (Image) Resources.GSE_Commitment_Tab_Active;
      this.btnContracts.Image = (Image) Resources.btn_master_contracts;
      this.btnCorrespondentMasters.Image = (Image) Resources.btn_correspondent_masters;
      this.btnCorrespondentTrades.Image = (Image) Resources.correspondent_trades_tab;
      this.btnBidTapeRegistration.Image = (Image) Resources.btn_bid_tape_management;
      this.currentScreen = TradeManagementScreen.GSECommitmentEditor;
      this.setMenu("Trades - GSECommitmentEditor");
      Cursor.Current = Cursors.Default;
      return true;
    }

    private bool queryCloseCurrentGseCommitment()
    {
      if (this.gseCommitmentEditorScreen == null || !this.gseCommitmentEditorScreen.DataModified)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "Would you like to save your changes to the current GSE Commitment?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.No:
          return true;
        default:
          return this.gseCommitmentEditorScreen.SaveTrade();
      }
    }

    public void CloseGseCommitment() => this.CurrentScreen = TradeManagementScreen.GSECommitments;

    public bool QueryCloseConsole()
    {
      if (this.currentScreen == TradeManagementScreen.TradeEditor)
        return this.queryCloseCurrentTrade();
      if (this.currentScreen == TradeManagementScreen.SecurityTradeEditor)
        return this.queryCloseCurrentSecurityTrade();
      if (this.currentScreen == TradeManagementScreen.MbsPoolEditor)
        return this.queryCloseCurrentMbsPool();
      if (this.currentScreen == TradeManagementScreen.FannieMaePEPoolEditor)
        return this.queryCloseCurrentFanniePEPool();
      if (this.currentScreen == TradeManagementScreen.Contracts)
        return this.queryCloseCurrentContract();
      if (this.currentScreen == TradeManagementScreen.CorrespondentMasterEditor)
        return this.queryCloseCurrentMaterCommitment();
      if (this.currentScreen == TradeManagementScreen.CorrespondentTradeEditor)
        return this.queryCloseCurrentCorrespondentTrade();
      return this.currentScreen != TradeManagementScreen.GSECommitmentEditor || this.queryCloseCurrentGseCommitment();
    }

    private void displayControl(Control c)
    {
      c.Dock = DockStyle.Fill;
      bool flag = false;
      foreach (Control control in (ArrangedElementCollection) this.pnlMain.Controls)
      {
        control.Hide();
        if (control.GetType() == c.GetType())
        {
          c.Show();
          flag = true;
        }
      }
      if (flag)
        return;
      this.pnlMain.Controls.Add(c);
    }

    public ITradeEditor GetActiveTradeEditor()
    {
      foreach (Control control in (ArrangedElementCollection) this.pnlMain.Controls)
      {
        if (typeof (ITradeEditor).IsAssignableFrom(control.GetType()) && control.Visible)
          return (ITradeEditor) control;
      }
      return (ITradeEditor) null;
    }

    public Control GetCurrentScreen()
    {
      if (this.currentScreen == TradeManagementScreen.Search)
        return (Control) this.searchScreen;
      if (this.currentScreen == TradeManagementScreen.Contracts)
        return (Control) this.contractsScreen;
      if (this.currentScreen == TradeManagementScreen.TradeEditor)
        return (Control) this.tradesScreen;
      if (this.currentScreen == TradeManagementScreen.SecurityTrades)
        return (Control) this.securityTradesScreen;
      if (this.currentScreen == TradeManagementScreen.SecurityTradeEditor)
        return (Control) this.securityTradeEditorScreen;
      if (this.currentScreen == TradeManagementScreen.MbsPools)
        return (Control) this.mbsPoolsScreen;
      if (this.currentScreen == TradeManagementScreen.MbsPoolEditor)
        return (Control) this.mbsPoolEditorScreen;
      if (this.currentScreen == TradeManagementScreen.CorrespondentMasters)
        return (Control) this.correspondentMasterScreen;
      if (this.currentScreen == TradeManagementScreen.CorrespondentMasterEditor)
        return (Control) this.correspondentMasterEditorScreen;
      if (this.currentScreen == TradeManagementScreen.CorrespondentTrades)
        return (Control) this.correspondentTradeScreen;
      if (this.currentScreen == TradeManagementScreen.CorrespondentTradeEditor)
        return (Control) this.correspondentTradeEditorScreen;
      if (this.currentScreen == TradeManagementScreen.FannieMaePEPoolEditor)
        return (Control) this.fanniePEPoolEditorScreen;
      if (this.currentScreen == TradeManagementScreen.GSECommitments)
        return (Control) this.gseCommitmentTradeScreen;
      if (this.currentScreen == TradeManagementScreen.GSECommitmentEditor)
        return (Control) this.gseCommitmentEditorScreen;
      return this.currentScreen == TradeManagementScreen.BidTapeRegistration ? (Control) this.bidTapeRegistration : (Control) null;
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      this.SetCurrentScreen(TradeManagementScreen.Search);
    }

    private void btnSecurityTrades_Click(object sender, EventArgs e)
    {
      this.SetCurrentScreen(TradeManagementScreen.SecurityTrades);
    }

    private void btnTrades_Click(object sender, EventArgs e)
    {
      this.SetCurrentScreen(TradeManagementScreen.Trades);
    }

    private void btnMbsPools_Click(object sender, EventArgs e)
    {
      this.SetCurrentScreen(TradeManagementScreen.MbsPools);
    }

    private void btnContracts_Click(object sender, EventArgs e)
    {
      this.SetCurrentScreen(TradeManagementScreen.Contracts);
    }

    private void btnBidTapeRegistration_Click(object sender, EventArgs e)
    {
      this.SetCurrentScreen(TradeManagementScreen.BidTapeRegistration);
    }

    private void btnCorrespondentMasters_Click(object sender, EventArgs e)
    {
      this.SetCurrentScreen(TradeManagementScreen.CorrespondentMasters);
    }

    private void btnCorrespondentTrades_Click(object sender, EventArgs e)
    {
      this.SetCurrentScreen(TradeManagementScreen.CorrespondentTrades);
    }

    private void btnGseCommitments_Click(object sender, EventArgs e)
    {
      this.SetCurrentScreen(TradeManagementScreen.GSECommitments);
    }

    private void btnSecurityTrades_MouseEnter(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.SecurityTrades || this.currentScreen == TradeManagementScreen.SecurityTradeEditor)
        return;
      this.btnSecurityTrades.Image = (Image) Resources.btn_security_trades_over;
    }

    private void btnSecurityTrades_MouseLeave(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.SecurityTrades || this.currentScreen == TradeManagementScreen.SecurityTradeEditor)
        return;
      this.btnSecurityTrades.Image = (Image) Resources.btn_security_trades;
    }

    private void btnTrades_MouseEnter(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.Trades || this.currentScreen == TradeManagementScreen.TradeEditor)
        return;
      this.btnTrades.Image = (Image) Resources.btn_loan_trades_over;
    }

    private void btnTrades_MouseLeave(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.Trades || this.currentScreen == TradeManagementScreen.TradeEditor)
        return;
      this.btnTrades.Image = (Image) Resources.btn_loan_trades;
    }

    private void btnMbsPools_MouseEnter(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.MbsPools || this.currentScreen == TradeManagementScreen.MbsPoolEditor || this.currentScreen == TradeManagementScreen.FannieMaePEPoolEditor)
        return;
      this.btnMbsPools.Image = (Image) Resources.btn_mbs_pools_over;
    }

    private void btnMbsPools_MouseLeave(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.MbsPools || this.currentScreen == TradeManagementScreen.MbsPoolEditor || this.currentScreen == TradeManagementScreen.FannieMaePEPoolEditor)
        return;
      this.btnMbsPools.Image = (Image) Resources.btn_mbs_pools;
    }

    private void btnSearch_MouseEnter(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.Search)
        return;
      this.btnSearch.Image = (Image) Resources.btn_loan_search_over;
    }

    private void btnSearch_MouseLeave(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.Search)
        return;
      this.btnSearch.Image = (Image) Resources.btn_loan_search;
    }

    private void btnContracts_MouseEnter(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.Contracts)
        return;
      this.btnContracts.Image = (Image) Resources.btn_master_contracts_over;
    }

    private void btnContracts_MouseLeave(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.Contracts)
        return;
      this.btnContracts.Image = (Image) Resources.btn_master_contracts;
    }

    private void btnCorrespondentMasters_MouseEnter(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.CorrespondentMasters || this.currentScreen == TradeManagementScreen.CorrespondentMasterEditor)
        return;
      this.btnCorrespondentMasters.Image = (Image) Resources.btn_correspondent_masters_over;
    }

    private void btnCorrespondentMasters_MouseLeave(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.CorrespondentMasters || this.currentScreen == TradeManagementScreen.CorrespondentMasterEditor)
        return;
      this.btnCorrespondentMasters.Image = (Image) Resources.btn_correspondent_masters;
    }

    private void btnCorrespondentTrades_MouseEnter(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.CorrespondentTrades || this.currentScreen == TradeManagementScreen.CorrespondentTradeEditor)
        return;
      this.btnCorrespondentTrades.Image = (Image) Resources.correspondent_trades_tab_hover;
    }

    private void btnCorrespondentTrades_MouseLeave(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.CorrespondentTrades || this.currentScreen == TradeManagementScreen.CorrespondentTradeEditor)
        return;
      this.btnCorrespondentTrades.Image = (Image) Resources.correspondent_trades_tab;
    }

    private void btnGseCommitments_MouseEnter(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.GSECommitments || this.currentScreen == TradeManagementScreen.GSECommitmentEditor)
        return;
      this.btnGseCommitments.Image = (Image) Resources.GSE_Commitment_Tab_Hover;
    }

    private void btnGseCommitments_MouseLeave(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.GSECommitments || this.currentScreen == TradeManagementScreen.GSECommitmentEditor)
        return;
      this.btnGseCommitments.Image = (Image) Resources.GSE_Commitment_Tab_Normal;
    }

    private void btnBidTapeRegistration_MouseEnter(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.BidTapeRegistration)
        return;
      this.btnBidTapeRegistration.Image = (Image) Resources.btn_bid_tape_management_hover;
    }

    private void btnBidTapeRegistration_MouseLeave(object sender, EventArgs e)
    {
      if (this.currentScreen == TradeManagementScreen.BidTapeRegistration)
        return;
      this.btnBidTapeRegistration.Image = (Image) Resources.btn_bid_tape_management;
    }

    public string GetHelpTargetName() => "Trade Management";

    public void MenuClicked(ToolStripItem menuItem)
    {
      string str = string.Concat(menuItem.Tag);
      if (str == "MC_Save" && this.currentScreen == TradeManagementScreen.TradeEditor)
        str = "TE_SaveTrade";
      if (str.StartsWith("LS_") && this.searchScreen != null)
        this.searchScreen.MenuClicked(menuItem);
      else if (str.StartsWith("STR_") && this.securityTradesScreen != null)
        this.securityTradesScreen.MenuClicked(menuItem);
      else if (str.StartsWith("TR_") && this.tradesScreen != null)
        this.tradesScreen.MenuClicked(menuItem);
      else if (str.StartsWith("MBS_") && this.mbsPoolsScreen != null)
        this.mbsPoolsScreen.MenuClicked(menuItem);
      else if (str.StartsWith("MBE_") && this.mbsPoolEditorScreen != null)
        this.mbsPoolEditorScreen.MenuClicked(menuItem);
      else if (str.StartsWith("MC_") && this.contractsScreen != null)
        this.contractsScreen.MenuClicked(menuItem);
      else if (str.StartsWith("STE_") && this.securityTradeEditorScreen != null)
        this.securityTradeEditorScreen.MenuClicked(menuItem);
      else if (str.StartsWith("TE_") && this.editorScreen != null)
        this.editorScreen.MenuClicked(menuItem);
      else if (str.StartsWith("CMC_") && this.correspondentMasterScreen != null)
        this.correspondentMasterScreen.MenuClicked(menuItem);
      else if (str.StartsWith("CMCE_") && this.correspondentMasterEditorScreen != null)
        this.correspondentMasterEditorScreen.MenuClicked(menuItem);
      else if (str.StartsWith("LT_") && this.correspondentTradeScreen != null)
        this.correspondentTradeScreen.MenuClicked(menuItem);
      else if (str.StartsWith("LTE_") && this.correspondentTradeEditorScreen != null)
        this.correspondentTradeEditorScreen.MenuClicked(menuItem);
      else if (str.StartsWith("GSE_") && this.gseCommitmentTradeScreen != null)
      {
        this.gseCommitmentTradeScreen.MenuClicked(menuItem);
      }
      else
      {
        if (!str.StartsWith("GSEE_") || this.gseCommitmentEditorScreen == null)
          return;
        this.gseCommitmentEditorScreen.MenuClicked(menuItem);
      }
    }

    public bool SetMenuItemState(ToolStripItem menuItem)
    {
      string str = string.Concat(menuItem.Tag);
      if (str.StartsWith("LS_"))
        return this.searchScreen.SetMenuItemState(menuItem);
      if (str.StartsWith("STR_"))
        return this.securityTradesScreen.SetMenuItemState(menuItem);
      if (str.StartsWith("TR_"))
        return this.tradesScreen.SetMenuItemState(menuItem);
      if (str.StartsWith("MBS_"))
        return this.mbsPoolsScreen.SetMenuItemState(menuItem);
      if (str.StartsWith("MBE_"))
        return this.mbsPoolEditorScreen.SetMenuItemState(menuItem);
      if (str.StartsWith("MC_"))
        return this.contractsScreen.SetMenuItemState(menuItem);
      if (str.StartsWith("CMC_"))
        return this.correspondentMasterScreen.SetMenuItemState(menuItem);
      if (str.StartsWith("CMCE_") && this.correspondentMasterEditorScreen != null)
        return this.correspondentMasterEditorScreen.SetMenuItemState(menuItem);
      if (str.StartsWith("LT_") && this.correspondentTradeScreen != null)
        return this.correspondentTradeScreen.SetMenuItemState(menuItem);
      if (str.StartsWith("LTE_") && this.correspondentTradeEditorScreen != null)
        return this.correspondentTradeEditorScreen.SetMenuItemState(menuItem);
      if (str.StartsWith("GSE_"))
        return this.gseCommitmentTradeScreen.SetMenuItemState(menuItem);
      return !str.StartsWith("GSEE_") || this.gseCommitmentEditorScreen == null || this.gseCommitmentEditorScreen.SetMenuItemState(menuItem);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.btnBidTapeRegistration = new PictureBox();
      this.btnGseCommitments = new PictureBox();
      this.btnCorrespondentMasters = new PictureBox();
      this.btnCorrespondentTrades = new PictureBox();
      this.btnMbsPools = new PictureBox();
      this.btnTrades = new PictureBox();
      this.btnSecurityTrades = new PictureBox();
      this.btnContracts = new PictureBox();
      this.btnSearch = new PictureBox();
      this.pnlMain = new BorderPanel();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.btnBidTapeRegistration).BeginInit();
      ((ISupportInitialize) this.btnGseCommitments).BeginInit();
      ((ISupportInitialize) this.btnCorrespondentMasters).BeginInit();
      ((ISupportInitialize) this.btnCorrespondentTrades).BeginInit();
      ((ISupportInitialize) this.btnMbsPools).BeginInit();
      ((ISupportInitialize) this.btnTrades).BeginInit();
      ((ISupportInitialize) this.btnSecurityTrades).BeginInit();
      ((ISupportInitialize) this.btnContracts).BeginInit();
      ((ISupportInitialize) this.btnSearch).BeginInit();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.btnBidTapeRegistration);
      this.panel1.Controls.Add((Control) this.btnGseCommitments);
      this.panel1.Controls.Add((Control) this.btnCorrespondentMasters);
      this.panel1.Controls.Add((Control) this.btnCorrespondentTrades);
      this.panel1.Controls.Add((Control) this.btnMbsPools);
      this.panel1.Controls.Add((Control) this.btnTrades);
      this.panel1.Controls.Add((Control) this.btnSecurityTrades);
      this.panel1.Controls.Add((Control) this.btnContracts);
      this.panel1.Controls.Add((Control) this.btnSearch);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(1327, 30);
      this.panel1.TabIndex = 4;
      this.btnBidTapeRegistration.BackColor = Color.Transparent;
      this.btnBidTapeRegistration.Image = (Image) Resources.btn_bid_tape_management;
      this.btnBidTapeRegistration.Location = new Point(1059, 0);
      this.btnBidTapeRegistration.Name = "btnBidTapeRegistration";
      this.btnBidTapeRegistration.Size = new Size(164, 31);
      this.btnBidTapeRegistration.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnBidTapeRegistration.TabIndex = 10;
      this.btnBidTapeRegistration.TabStop = false;
      this.btnBidTapeRegistration.Click += new EventHandler(this.btnBidTapeRegistration_Click);
      this.btnBidTapeRegistration.MouseEnter += new EventHandler(this.btnBidTapeRegistration_MouseEnter);
      this.btnBidTapeRegistration.MouseLeave += new EventHandler(this.btnBidTapeRegistration_MouseLeave);
      this.btnGseCommitments.Image = (Image) Resources.GSE_Commitment_Tab_Normal;
      this.btnGseCommitments.Location = new Point(576, 0);
      this.btnGseCommitments.Name = "btnGseCommitments";
      this.btnGseCommitments.Size = new Size(150, 31);
      this.btnGseCommitments.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnGseCommitments.TabIndex = 8;
      this.btnGseCommitments.TabStop = false;
      this.btnGseCommitments.Click += new EventHandler(this.btnGseCommitments_Click);
      this.btnGseCommitments.MouseEnter += new EventHandler(this.btnGseCommitments_MouseEnter);
      this.btnGseCommitments.MouseLeave += new EventHandler(this.btnGseCommitments_MouseLeave);
      this.btnCorrespondentMasters.BackColor = Color.Transparent;
      this.btnCorrespondentMasters.Image = (Image) Resources.btn_correspondent_masters;
      this.btnCorrespondentMasters.Location = new Point(892, 0);
      this.btnCorrespondentMasters.Name = "btnCorrespondentMasters";
      this.btnCorrespondentMasters.Size = new Size(167, 31);
      this.btnCorrespondentMasters.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnCorrespondentMasters.TabIndex = 9;
      this.btnCorrespondentMasters.TabStop = false;
      this.btnCorrespondentMasters.Click += new EventHandler(this.btnCorrespondentMasters_Click);
      this.btnCorrespondentMasters.MouseEnter += new EventHandler(this.btnCorrespondentMasters_MouseEnter);
      this.btnCorrespondentMasters.MouseLeave += new EventHandler(this.btnCorrespondentMasters_MouseLeave);
      this.btnCorrespondentTrades.BackColor = Color.Transparent;
      this.btnCorrespondentTrades.Image = (Image) Resources.correspondent_trades_tab;
      this.btnCorrespondentTrades.Location = new Point(726, 0);
      this.btnCorrespondentTrades.Name = "btnCorrespondentTrades";
      this.btnCorrespondentTrades.Size = new Size(166, 31);
      this.btnCorrespondentTrades.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnCorrespondentTrades.TabIndex = 8;
      this.btnCorrespondentTrades.TabStop = false;
      this.btnCorrespondentTrades.Click += new EventHandler(this.btnCorrespondentTrades_Click);
      this.btnCorrespondentTrades.MouseEnter += new EventHandler(this.btnCorrespondentTrades_MouseEnter);
      this.btnCorrespondentTrades.MouseLeave += new EventHandler(this.btnCorrespondentTrades_MouseLeave);
      this.btnMbsPools.Image = (Image) Resources.btn_mbs_pools;
      this.btnMbsPools.Location = new Point(476, 0);
      this.btnMbsPools.Name = "btnMbsPools";
      this.btnMbsPools.Size = new Size(101, 31);
      this.btnMbsPools.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnMbsPools.TabIndex = 7;
      this.btnMbsPools.TabStop = false;
      this.btnMbsPools.Click += new EventHandler(this.btnMbsPools_Click);
      this.btnMbsPools.MouseEnter += new EventHandler(this.btnMbsPools_MouseEnter);
      this.btnMbsPools.MouseLeave += new EventHandler(this.btnMbsPools_MouseLeave);
      this.btnTrades.Image = (Image) Resources.btn_loan_trades;
      this.btnTrades.Location = new Point(233, 0);
      this.btnTrades.Name = "btnTrades";
      this.btnTrades.Size = new Size(111, 31);
      this.btnTrades.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnTrades.TabIndex = 4;
      this.btnTrades.TabStop = false;
      this.btnTrades.Click += new EventHandler(this.btnTrades_Click);
      this.btnTrades.MouseEnter += new EventHandler(this.btnTrades_MouseEnter);
      this.btnTrades.MouseLeave += new EventHandler(this.btnTrades_MouseLeave);
      this.btnSecurityTrades.Image = (Image) Resources.btn_security_trades;
      this.btnSecurityTrades.Location = new Point(0, 0);
      this.btnSecurityTrades.Name = "btnSecurityTrades";
      this.btnSecurityTrades.Size = new Size(126, 31);
      this.btnSecurityTrades.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnSecurityTrades.TabIndex = 6;
      this.btnSecurityTrades.TabStop = false;
      this.btnSecurityTrades.Click += new EventHandler(this.btnSecurityTrades_Click);
      this.btnSecurityTrades.MouseEnter += new EventHandler(this.btnSecurityTrades_MouseEnter);
      this.btnSecurityTrades.MouseLeave += new EventHandler(this.btnSecurityTrades_MouseLeave);
      this.btnContracts.Image = (Image) Resources.btn_master_contracts;
      this.btnContracts.Location = new Point(343, 0);
      this.btnContracts.Name = "btnContracts";
      this.btnContracts.Size = new Size(134, 31);
      this.btnContracts.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnContracts.TabIndex = 5;
      this.btnContracts.TabStop = false;
      this.btnContracts.Click += new EventHandler(this.btnContracts_Click);
      this.btnContracts.MouseEnter += new EventHandler(this.btnContracts_MouseEnter);
      this.btnContracts.MouseLeave += new EventHandler(this.btnContracts_MouseLeave);
      this.btnSearch.Image = (Image) Resources.btn_loan_search;
      this.btnSearch.Location = new Point(125, 0);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(109, 31);
      this.btnSearch.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnSearch.TabIndex = 3;
      this.btnSearch.TabStop = false;
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      this.btnSearch.MouseEnter += new EventHandler(this.btnSearch_MouseEnter);
      this.btnSearch.MouseLeave += new EventHandler(this.btnSearch_MouseLeave);
      this.pnlMain.BackColor = Color.Transparent;
      this.pnlMain.Dock = DockStyle.Fill;
      this.pnlMain.Location = new Point(0, 30);
      this.pnlMain.Name = "pnlMain";
      this.pnlMain.Size = new Size(1327, 399);
      this.pnlMain.TabIndex = 5;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Transparent;
      this.Controls.Add((Control) this.pnlMain);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (TradeManagementConsole);
      this.Size = new Size(1327, 429);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.btnBidTapeRegistration).EndInit();
      ((ISupportInitialize) this.btnGseCommitments).EndInit();
      ((ISupportInitialize) this.btnCorrespondentMasters).EndInit();
      ((ISupportInitialize) this.btnCorrespondentTrades).EndInit();
      ((ISupportInitialize) this.btnMbsPools).EndInit();
      ((ISupportInitialize) this.btnTrades).EndInit();
      ((ISupportInitialize) this.btnSecurityTrades).EndInit();
      ((ISupportInitialize) this.btnContracts).EndInit();
      ((ISupportInitialize) this.btnSearch).EndInit();
      this.ResumeLayout(false);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.ThinThick.TradingBrowser
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.ThinThick;
using EllieMae.EMLite.Common.ThinThick.Requests;
using EllieMae.EMLite.Common.ThinThick.Requests.Trading;
using EllieMae.EMLite.Common.ThinThick.Responses;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls.ThinThick;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.MainUI.ThinThick.TradingScreens;
using EllieMae.EMLite.MainUI.ThinThick.TradingScreens.Interfaces;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ThinThick.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI.ThinThick
{
  public class TradingBrowser : 
    BrowserBase,
    ITradeConsole,
    IOnlineHelpTarget,
    IMenuProvider,
    IBrowser
  {
    private TradeManagementScreen _currentScreen;
    private Dictionary<TradeManagementScreen, ITradingScreen> tradingScreens = new Dictionary<TradeManagementScreen, ITradingScreen>();
    private readonly ThinThickBrowserManager _browserManager;
    private readonly OpTrading _opTrading = new OpTrading();
    private string _helpTargetName = "Trade Management";
    private IContainer components;

    public TradingBrowser()
    {
      this.InitializeComponent();
      this._browserManager = new ThinThickBrowserManager(Session.DefaultInstance, this.Browser, (IWin32Window) this);
      this._browserManager.NavigatePage(PageRegistry.TradingPage);
      this.tradingScreens[TradeManagementScreen.Search] = (ITradingScreen) new Search();
      this.tradingScreens[TradeManagementScreen.Trades] = (ITradingScreen) new Trades();
      this.tradingScreens[TradeManagementScreen.Contracts] = (ITradingScreen) new Contracts();
      this.tradingScreens[TradeManagementScreen.TradeEditor] = (ITradingScreen) new TradeEditor();
      this.tradingScreens[TradeManagementScreen.SecurityTrades] = (ITradingScreen) new SecurityTrades();
      this.tradingScreens[TradeManagementScreen.SecurityTradeEditor] = (ITradingScreen) new SecurityTradeEditor();
      this.tradingScreens[TradeManagementScreen.MbsPools] = (ITradingScreen) new MbsPools();
      this.tradingScreens[TradeManagementScreen.MbsPoolEditor] = (ITradingScreen) new MbsPoolEditor();
      this.tradingScreens[TradeManagementScreen.CorrespondentMasters] = (ITradingScreen) new CorrespondentMasters();
      this.tradingScreens[TradeManagementScreen.CorrespondentMasterEditor] = (ITradingScreen) new CorrespondentMasterEditor();
      this.tradingScreens[TradeManagementScreen.CorrespondentTrades] = (ITradingScreen) new CorrespondentTrades();
      this.tradingScreens[TradeManagementScreen.CorrespondentTradeEditor] = (ITradingScreen) new CorrespondentTradeEditor();
    }

    public void Initialize() => this.CurrentScreen = TradeManagementScreen.Trades;

    public OpSimpleResponse ExportToExcel(OpExportRequest request)
    {
      return this.CurrentTradingList.ExportToExcel(request.ExportAll, false);
    }

    public OpSimpleResponse Print(OpSimpleRequest request)
    {
      return this.CurrentTradingList.ExportToExcel(false, true);
    }

    public OpSimpleResponse ExportToExcel(bool exportAll, bool print)
    {
      return this.CurrentTradingList.ExportToExcel(exportAll, print);
    }

    private bool IsTradingList => this.tradingScreens[this.CurrentScreen] is ITradingList;

    private ITradingList CurrentTradingList
    {
      get => (ITradingList) this.tradingScreens[this.CurrentScreen];
    }

    public bool IsCurrentViewSet => this.IsTradingList && this.CurrentTradingList.IsCurrentViewSet;

    public TableLayout Layout => this.CurrentTradingList.Layout;

    public int ItemCount => this.CurrentTradingList.ItemCount;

    public bool SetTradingIds(int[] ids)
    {
      if (!this.IsTradingList)
        return false;
      this.CurrentTradingList.SetIds(ids);
      return true;
    }

    public TradeManagementScreen CurrentScreen
    {
      get => this._currentScreen;
      set => this.SetCurrentScreen(value);
    }

    public bool SetCurrentScreen(TradeManagementScreen screen)
    {
      this._currentScreen = screen;
      this.MenuItemCollection = this.tradingScreens[this.CurrentScreen].ToolStripItemCollection;
      this.setMenu(this.tradingScreens[this.CurrentScreen].MenuName);
      Cursor.Current = Cursors.Default;
      return true;
    }

    private void setMenu(string menuName)
    {
      Session.Application.GetService<IEncompassApplication>().SetMenu(menuName);
    }

    public bool OpenTrade(int tradeId, bool gotoHistory = false)
    {
      throw new NotImplementedException();
    }

    public void MenuClicked(ToolStripItem menuItem)
    {
      this.Browser.InvokeScript(ScriptRegistry.EncompassInteractionMenuClicked, new object[1]
      {
        menuItem.Tag
      });
    }

    public bool SetMenuItemState(ToolStripItem menuItem) => throw new NotImplementedException();

    public void SetHelpTargetName(string name) => this._helpTargetName = name;

    public bool SetSelectedPipelineView(string pipelineViewXml)
    {
      if (!this.IsTradingList)
        return false;
      this.CurrentTradingList.SetSelectedPipelineView(pipelineViewXml);
      return true;
    }

    public string GetHelpTargetName() => this._helpTargetName;

    public string GetSamplePageName() => "encompass.trading.html";

    public CurrentOrArchived CurrentOrArchived
    {
      get => this.CurrentTradingList.CurrentOrArchived;
      set => this.CurrentTradingList.CurrentOrArchived = value;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Name = nameof (TradingBrowser);
      this.Size = new Size(595, 415);
      this.ResumeLayout(false);
    }
  }
}

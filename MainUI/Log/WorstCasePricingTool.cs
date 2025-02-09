// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.WorstCasePricingTool
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.LoanUtils.RateLocks;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class WorstCasePricingTool : UserControl, IOnlineHelpTarget
  {
    private const string className = "WorstCasePricingTool";
    private static readonly string sw = Tracing.SwInputEngine;
    private WorstCasePricingColumn historicalPricing;
    private WorstCasePricingColumn currentPricing;
    private Sessions.Session session;
    private bool readOnly;
    private bool isSubmitPricingEnabled = true;
    private bool isInactiveLock;
    private bool isHistoricalGetPricing;
    private bool isCurrentGetPricing;
    private Hashtable mostRecentConfirmedSnapshot;
    private Routine fx;
    private IContainer components;
    private Panel pnlPricing;
    private Panel pnlCurrent;
    private Panel pnlHistorical;
    private Panel pnlBottom;
    private Button btnCurrentPricing;
    private Button btnHistoricalPricing;
    private PageListNavigator pageListNavigator1;
    private GradientPanel gradientPanel1;
    private GradientPanel gradientPanel2;
    private GradientPanel gradientPanel3;
    private Button btnSubmitCurrentPricing;
    private Label label2;
    private Button btnSubmitHistoricalPricing;
    private Label label1;

    public WorstCasePricingTool()
    {
      this.Dock = DockStyle.Fill;
      this.InitializeComponent();
      this.session = Session.DefaultInstance;
      this.fx = new Routine(this.populateOptimalblueResult);
      this.session.LoanData.RegisterFieldValueChangeEventHandler("OPTIMAL.RESPONSE", this.fx);
      string field = Session.LoanDataMgr.LoanData.GetField("LOCKRATE.RATESTATUS");
      this.isInactiveLock = field == "Expired" || field == "Cancelled";
      if (this.ifLoanAssignedToTrade())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Worst Case Pricing tool cannot be used with loans that are assigned to a correspondent trade. The loan must first be removed from the trade to enable Worst Case Pricing.");
        this.readOnly = true;
      }
      else if (!this.isLoanValidForWorstCasePricing())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "To use the Worst Case Pricing Tool, you must first lock the loan using ICE PPE.");
        this.readOnly = true;
      }
      else
        this.ValidateForInactiveLock();
      this.initForm();
    }

    private void initForm()
    {
      WorstCasePricingCache.RefreshDataTables();
      this.historicalPricing = new WorstCasePricingColumn(this.readOnly, WorstCasePricingCache.HistoricalPriceTable, false, WorstCasePricingCache.IsLockExtension);
      this.historicalPricing.setTitle("Historical Pricing");
      this.currentPricing = new WorstCasePricingColumn(this.readOnly, WorstCasePricingCache.CurrentPriceTable, true, WorstCasePricingCache.IsLockExtension);
      this.currentPricing.setTitle("Current Pricing");
      this.pnlHistorical.Controls.Add((Control) this.historicalPricing);
      this.pnlCurrent.Controls.Add((Control) this.currentPricing);
      this.pnlCurrent.Height = this.pnlHistorical.Height = this.historicalPricing.Height;
      this.btnCurrentPricing.Enabled = this.btnHistoricalPricing.Enabled = !this.readOnly;
      this.btnSubmitCurrentPricing.Enabled = this.btnSubmitHistoricalPricing.Enabled = !this.readOnly && this.isSubmitPricingEnabled;
      this.historicalPricing.ZoomButtonClicked += new EventHandler(this.HistoricalPricing_ZoomButtonClicked);
      this.currentPricing.ZoomButtonClicked += new EventHandler(this.CurrentPricing_ZoomButtonClicked);
      this.historicalPricing.ClearButtonClicked += new EventHandler(this.ClearButtonClicked_Historical_Handler);
      this.currentPricing.ClearButtonClicked += new EventHandler(this.ClearButtonClicked_Current_Handler);
      this.historicalPricing.RefreshScreen();
      this.currentPricing.RefreshScreen();
    }

    private void HistoricalPricing_ZoomButtonClicked(object sender, EventArgs e)
    {
      this.currentPricing.ZoomPanels(!this.historicalPricing.IsBaseRateDisplayedAll, !this.historicalPricing.IsBaseMarginDisplayedAll, !this.historicalPricing.IsExtensionDisplayedAll, !this.historicalPricing.IsBasePriceDisplayedAll);
      this.pnlCurrent.Height = this.pnlHistorical.Height = this.historicalPricing.Height;
    }

    private void CurrentPricing_ZoomButtonClicked(object sender, EventArgs e)
    {
      this.historicalPricing.ZoomPanels(!this.currentPricing.IsBaseRateDisplayedAll, !this.currentPricing.IsBaseMarginDisplayedAll, !this.currentPricing.IsExtensionDisplayedAll, !this.currentPricing.IsBasePriceDisplayedAll);
      this.pnlCurrent.Height = this.pnlHistorical.Height = this.currentPricing.Height;
    }

    private bool isLoanValidForWorstCasePricing()
    {
      if (this.session.StartupInfo.ProductPricingPartner != null && this.session.StartupInfo.ProductPricingPartner.PartnerID == "MPS")
      {
        LockRequestLog confirmedLockForWcpt = WorstCasePricingCache.GetLastConfirmedLockForWcpt(this.session.LoanDataMgr);
        if (confirmedLockForWcpt != null && ProductPricingUtils.IsHistoricalPricingEnabled)
        {
          this.mostRecentConfirmedSnapshot = confirmedLockForWcpt.GetLockRequestSnapshot();
          if (this.mostRecentConfirmedSnapshot.Contains((object) "OPTIMAL.HISTORY") && this.mostRecentConfirmedSnapshot[(object) "OPTIMAL.HISTORY"].ToString().Contains("<MPS_envelope"))
            return true;
        }
      }
      return false;
    }

    private bool ifLoanAssignedToTrade()
    {
      return !string.IsNullOrEmpty(Session.LoanDataMgr.LoanData.GetField("3907"));
    }

    private void ValidateForInactiveLock()
    {
      if (WorstCasePricingCache.GetLastConfirmedLockForWcpt(this.session.LoanDataMgr) == null || !this.isInactiveLock || LockUtils.IsInactiveReLockExceededAllowed(this.session.SessionObjects, Session.LoanDataMgr.LoanData))
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "The Re-Lock occurrence limit has already been reached. This loan will need to be locked manually.");
      this.isSubmitPricingEnabled = false;
    }

    private void SubmitPricing_Click(object sender, EventArgs e)
    {
      WorstCasePricingTool.WorstCasePricingSideType casePricingSideType;
      if (sender == this.btnSubmitHistoricalPricing)
      {
        casePricingSideType = WorstCasePricingTool.WorstCasePricingSideType.HistoricalPricing;
      }
      else
      {
        if (sender != this.btnSubmitCurrentPricing)
          return;
        casePricingSideType = WorstCasePricingTool.WorstCasePricingSideType.CurrentPricing;
      }
      if (!this.validateCommitmentType(casePricingSideType) && Utils.Dialog((IWin32Window) this, "Commitment Type must be selected for a Correspondent loan. Lock Request was not submitted.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) == DialogResult.OK || this.isInactiveLock && casePricingSideType == WorstCasePricingTool.WorstCasePricingSideType.HistoricalPricing && !this.isHistoricalGetPricing && Utils.Dialog((IWin32Window) this, "A Re-Lock Fee has not been generated for this request. If you wish to continue without a Re-Lock Fee click OK, otherwise click Cancel.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
        return;
      string message = "";
      switch (casePricingSideType)
      {
        case WorstCasePricingTool.WorstCasePricingSideType.HistoricalPricing:
          message = !WorstCasePricingCache.IsLockExtension ? "Do you want to submit Historical Pricing as new lock rate?" : "Do you want to submit Historical Pricing as new extended lock rate?";
          break;
        case WorstCasePricingTool.WorstCasePricingSideType.CurrentPricing:
          message = "Do you want to submit Current Pricing as new lock rate?";
          break;
      }
      using (WorstCasePricingSubmitDialog pricingSubmitDialog = new WorstCasePricingSubmitDialog(message))
      {
        if (pricingSubmitDialog.ShowDialog() != DialogResult.OK)
          return;
        this.closeSecondaryLockTool();
        this.submitLockRequest(casePricingSideType);
      }
    }

    private bool validateCommitmentType(WorstCasePricingTool.WorstCasePricingSideType side)
    {
      return (side != WorstCasePricingTool.WorstCasePricingSideType.HistoricalPricing || !this.historicalPricing.IsDeliveryPanelVisible || WorstCasePricingCache.HistoricalPriceTable.Contains((object) "4187") && !string.IsNullOrEmpty(WorstCasePricingCache.HistoricalPriceTable[(object) "4187"].ToString())) && (side != WorstCasePricingTool.WorstCasePricingSideType.CurrentPricing || !this.currentPricing.IsDeliveryPanelVisible || WorstCasePricingCache.CurrentPriceTable.Contains((object) "4187") && !string.IsNullOrEmpty(WorstCasePricingCache.CurrentPriceTable[(object) "4187"].ToString()));
    }

    private void closeSecondaryLockTool()
    {
      Form form = (Form) null;
      foreach (Form openForm in (ReadOnlyCollectionBase) Application.OpenForms)
      {
        if (openForm is BuySellForm)
          form = openForm;
      }
      form?.Close();
    }

    private void submitLockRequest(
      WorstCasePricingTool.WorstCasePricingSideType pricingSide)
    {
      LoanDataMgr loanDataMgr = Session.LoanDataMgr;
      if (loanDataMgr == null)
        return;
      this.triggerAllCalculations(loanDataMgr.LoanData);
      LockUtils.RefreshLockRequestMapFieldsFromLoan(loanDataMgr);
      LockUtils.SyncAdditionalLockFieldsToRequest(loanDataMgr);
      Hashtable fieldDataTables;
      List<string> stringList;
      if (pricingSide == WorstCasePricingTool.WorstCasePricingSideType.HistoricalPricing)
      {
        this.historicalPricing.FieldDataTables[(object) "2144"] = (object) this.historicalPricing.getComment();
        fieldDataTables = this.historicalPricing.FieldDataTables;
        stringList = WorstCasePricingCache.HistoricalResponseFields;
      }
      else
      {
        this.currentPricing.FieldDataTables[(object) "2144"] = (object) this.currentPricing.getComment();
        fieldDataTables = this.currentPricing.FieldDataTables;
        stringList = WorstCasePricingCache.CurrentResponseFields;
      }
      foreach (string requestPricingField in this.getRequestPricingFieldList())
      {
        if (fieldDataTables.ContainsKey((object) requestPricingField))
          loanDataMgr.LoanData.SetField(requestPricingField, fieldDataTables[(object) requestPricingField].ToString());
        else
          loanDataMgr.LoanData.SetField(requestPricingField, "");
      }
      foreach (string str in stringList)
      {
        if (str != "Import")
          loanDataMgr.LoanData.SetField(str, fieldDataTables[(object) str].ToString());
      }
      loanDataMgr.LoanData.SetField("3841", "NewLock");
      if (WorstCasePricingCache.IsLockExtension && pricingSide == WorstCasePricingTool.WorstCasePricingSideType.HistoricalPricing)
      {
        loanDataMgr.LoanData.SetField("3369", fieldDataTables[(object) "3369"].ToString());
        loanDataMgr.LoanData.SetField("3358", fieldDataTables[(object) "3358"].ToString());
        loanDataMgr.LoanData.SetField("2151", fieldDataTables[(object) "2151"].ToString());
        loanDataMgr.LoanData.SetField("3363", fieldDataTables[(object) "3363"].ToString());
        loanDataMgr.LoanData.SetField("3364", fieldDataTables[(object) "3364"].ToString());
        loanDataMgr.LoanData.SetField("3365", fieldDataTables[(object) "3365"].ToString());
      }
      InputHandlerUtil inputHandlerUtil = new InputHandlerUtil(this.session);
      bool alwaysKeepLoanOpen = false;
      if (this.session.SessionObjects.StartupInfo.ProductPricingPartner.EnableAutoLockRequest)
        alwaysKeepLoanOpen = true;
      string requestGUID = (string) null;
      try
      {
        requestGUID = inputHandlerUtil.SendLockRequest(alwaysKeepLoanOpen, false, true, WorstCasePricingCache.IsLockExtension, pricingSide == WorstCasePricingTool.WorstCasePricingSideType.CurrentPricing, pricingSide == WorstCasePricingTool.WorstCasePricingSideType.CurrentPricing ? RateLockAction.WcpcCurrent : RateLockAction.WcpcHistorical);
      }
      catch (LockDeskClosedException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.Application, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (LockDeskONRPException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.Application, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      if (string.IsNullOrEmpty(requestGUID))
        return;
      if (alwaysKeepLoanOpen && this.session.LoanDataMgr.LoanData.GetLogList().GetLockRequest(requestGUID).LockRequestStatus == RateLockRequestStatus.RateLocked)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The lock has been successfully confirmed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      if (Session.LoanDataMgr == null)
        return;
      WorstCasePricingCache.ClearDataTables();
      this.historicalPricing.RefreshScreen();
      this.currentPricing.RefreshScreen();
    }

    private List<string> getRequestPricingFieldList()
    {
      List<string> pricingFieldList = new List<string>();
      for (int index = 2088; index <= 2143; ++index)
        pricingFieldList.Add(index.ToString());
      for (int index = 2414; index <= 2447; ++index)
        pricingFieldList.Add(index.ToString());
      for (int index = 2647; index <= 2689; ++index)
        pricingFieldList.Add(index.ToString());
      for (int index = 3454; index <= 3473; ++index)
        pricingFieldList.Add(index.ToString());
      for (int index = 4256; index <= 4275; ++index)
        pricingFieldList.Add(index.ToString());
      for (int index = 4336; index <= 4355; ++index)
        pricingFieldList.Add(index.ToString());
      pricingFieldList.Add("2848");
      pricingFieldList.Add("3254");
      pricingFieldList.Add("2101");
      pricingFieldList.Add("3360");
      pricingFieldList.Add("3361");
      pricingFieldList.Add("3362");
      pricingFieldList.Add("3847");
      pricingFieldList.Add("3872");
      pricingFieldList.Add("3874");
      pricingFieldList.Add("3965");
      pricingFieldList.Add("4187");
      pricingFieldList.Add("2144");
      pricingFieldList.Add("3039");
      pricingFieldList.Add("4201");
      return pricingFieldList;
    }

    private void GetPricing(bool isHP)
    {
      if (this.isInactiveLock & isHP && !LockUtils.GetNextAvailableReLockFieldId(this.session.SessionObjects, WorstCasePricingCache.HistoricalPriceTable).HasValue)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Limit of 10 Re-Lock Fees has already been reached for this loan.  This loan may need to be priced and locked manually unless the Re-Lock Fees have been addressed.");
      }
      else
      {
        string str = !isHP ? "WorstCaseCurrent" : "WorstCaseHistorical";
        ProductPricingSetting productPricingPartner = this.session.StartupInfo.ProductPricingPartner;
        if (productPricingPartner == null || productPricingPartner.PartnerID != "MPS")
          return;
        this.closeSecondaryLockTool();
        this.triggerAllCalculations(this.session.LoanDataMgr.LoanData);
        IEPass service = Session.Application.GetService<IEPass>();
        this.session.LoanData.SetField("OPTIMAL.REQUEST", "");
        this.session.LoanData.SetField("OPTIMAL.RESPONSE", "");
        this.session.LoanData.SetField("OPTIMAL.REQUEST", this.buildRequestData(isHP));
        service.ProcessURL("_EPASS_SIGNATURE;" + this.session.StartupInfo.ProductPricingPartner.PartnerID + ";;" + str + ";SOURCE_FORM=WorstCasePricingTool");
      }
    }

    private string buildRequestData(bool isHP)
    {
      ValuePairXmlWriter valuePairXmlWriter = new ValuePairXmlWriter("FieldID", "FieldValue");
      if (isHP)
        valuePairXmlWriter.Write("WorstCasePricing", "Historical");
      else
        valuePairXmlWriter.Write("WorstCasePricing", "Current");
      if (isHP && this.mostRecentConfirmedSnapshot != null)
      {
        string fieldValue = this.getFieldValue("OPTIMAL.HISTORY", this.mostRecentConfirmedSnapshot);
        if (fieldValue != string.Empty)
        {
          valuePairXmlWriter.Write("History", fieldValue);
          valuePairXmlWriter.Write("OPTIMAL.HISTORY", fieldValue);
        }
      }
      return valuePairXmlWriter.ToXML();
    }

    private void btnHistoricalPricing_Click(object sender, EventArgs e) => this.GetPricing(true);

    private void btnCurrentPricing_Click(object sender, EventArgs e) => this.GetPricing(false);

    private string getFieldValue(string id, Hashtable t)
    {
      return t.ContainsKey((object) id) ? t[(object) id].ToString() : string.Empty;
    }

    private void populateOptimalblueResult(string id, string val)
    {
      LoanDataMgr loanDataMgr = Session.LoanDataMgr;
      if (loanDataMgr == null || string.IsNullOrEmpty(loanDataMgr.LoanData.GetField("OPTIMAL.REQUEST")))
        return;
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      Dictionary<string, string> keyValuePair1 = new ValuePairXmlWriter(loanDataMgr.LoanData.GetField("OPTIMAL.REQUEST"), "FieldID", "FieldValue").GetKeyValuePair();
      if (!keyValuePair1.ContainsKey("WorstCasePricing") || keyValuePair1["WorstCasePricing"] == null)
        return;
      bool flag;
      if (keyValuePair1["WorstCasePricing"].ToLower() == "historical")
      {
        flag = true;
      }
      else
      {
        if (!(keyValuePair1["WorstCasePricing"].ToLower() == "current"))
          return;
        flag = false;
      }
      string field = loanDataMgr.LoanData.GetField("OPTIMAL.RESPONSE");
      if (string.IsNullOrEmpty(field))
        return;
      Dictionary<string, string> keyValuePair2 = new ValuePairXmlWriter(field, "FieldID", "FieldValue").GetKeyValuePair();
      Hashtable dataTable;
      if (flag)
      {
        dataTable = WorstCasePricingCache.HistoricalPriceTable;
        WorstCasePricingCache.HistoricalResponseFields = keyValuePair2.Keys.ToList<string>();
      }
      else
      {
        dataTable = WorstCasePricingCache.CurrentPriceTable;
        WorstCasePricingCache.CurrentResponseFields = keyValuePair2.Keys.ToList<string>();
      }
      for (int index = 2088; index < 2144; ++index)
        dataTable[(object) string.Concat((object) index)] = (object) "";
      for (int index = 2414; index < 2448; ++index)
        dataTable[(object) string.Concat((object) index)] = (object) "";
      for (int index = 2647; index < 2689; ++index)
        dataTable[(object) string.Concat((object) index)] = (object) "";
      dataTable[(object) "2848"] = (object) "";
      dataTable[(object) "2866"] = (object) loanDataMgr.LoanData.GetField("1401");
      dataTable[(object) "2089"] = (object) DateTime.Now.ToString("d");
      if (flag && keyValuePair2.ContainsKey("OPTIMAL.HISTORY"))
      {
        string fromEppsTxHistory = LockUtils.GetQualifiedAsOfDateFromEppsTxHistory(keyValuePair2["OPTIMAL.HISTORY"]);
        if (fromEppsTxHistory != "")
          dataTable[(object) "2089"] = (object) fromEppsTxHistory;
      }
      foreach (string key in keyValuePair2.Keys)
        dataTable[(object) key] = (object) keyValuePair2[key];
      dataTable[(object) "3039"] = (object) Session.ServerTime.ToString("MM/dd/yyyy hh:mm:ss tt");
      if (loanDataMgr.LoanData.GetField("2626") == "Correspondent" && keyValuePair2.ContainsKey("OPTIMAL.HISTORY"))
      {
        Decimal fromEppsTxHistory = LockUtils.GetSRPFromEppsTxHistory(keyValuePair2["OPTIMAL.HISTORY"], true);
        if (fromEppsTxHistory != 0M)
        {
          dataTable[(object) "4201"] = (object) fromEppsTxHistory.ToString();
          dataTable[(object) "2101"] = (object) (Utils.ParseDecimal(dataTable[(object) "2101"]) - fromEppsTxHistory).ToString();
        }
      }
      if (((!this.isInactiveLock ? 0 : (!this.isHistoricalGetPricing ? 1 : 0)) & (flag ? 1 : 0)) != 0)
      {
        this.isHistoricalGetPricing = true;
        LockUtils.AddReLockFee(this.session.SessionObjects, dataTable, loanDataMgr.LoanData);
      }
      if (this.isInactiveLock && !this.isCurrentGetPricing && !flag)
      {
        this.isCurrentGetPricing = true;
        for (int index = 3454; index <= 3473; ++index)
          dataTable[(object) string.Concat((object) index)] = (object) "";
        for (int index = 4256; index <= 4275; ++index)
          dataTable[(object) string.Concat((object) index)] = (object) "";
        for (int index = 4336; index <= 4355; ++index)
          dataTable[(object) string.Concat((object) index)] = (object) "";
        LockUtils.AddReLockFee(this.session.SessionObjects, dataTable, loanDataMgr.LoanData);
      }
      loanDataMgr.LoanData.SetField("OPTIMAL.REQUEST", "");
      loanDataMgr.LoanData.SetField("OPTIMAL.RESPONSE", "");
      if (flag)
      {
        this.historicalPricing.RefreshScreen();
        this.historicalPricing.RecalcLockExpirationDate();
      }
      else
      {
        this.currentPricing.RefreshScreen();
        this.currentPricing.RecalcLockExpirationDate();
      }
    }

    private void WorstCasePricingTool_Load(object sender, EventArgs e)
    {
      Tracing.Log(WorstCasePricingTool.sw, TraceLevel.Verbose, nameof (WorstCasePricingTool), "Load");
    }

    protected override void Dispose(bool disposing)
    {
      Tracing.Log(WorstCasePricingTool.sw, TraceLevel.Verbose, nameof (WorstCasePricingTool), nameof (Dispose));
      if (this.session.LoanData != null)
        this.session.LoanData.UnregisterFieldValueChangeEventHandler("OPTIMAL.RESPONSE", this.fx);
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void ClearButtonClicked_Historical_Handler(object sender, EventArgs e)
    {
      WorstCasePricingCache.HistoricalPriceTable.Clear();
      WorstCasePricingCache.HistoricalResponseFields.Clear();
      this.historicalPricing.RefreshScreen();
    }

    private void ClearButtonClicked_Current_Handler(object sender, EventArgs e)
    {
      WorstCasePricingCache.CurrentPriceTable.Clear();
      WorstCasePricingCache.CurrentResponseFields.Clear();
      this.currentPricing.RefreshScreen();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
    }

    public string GetHelpTargetName() => "Worst Case Pricing";

    private void triggerAllCalculations(LoanData loan)
    {
      if (loan == null)
        return;
      if (loan.Calculator == null)
        return;
      try
      {
        loan.Calculator.CalcOnDemand();
        bool skipLockRequestSync = loan.Calculator.SkipLockRequestSync;
        loan.Calculator.SkipLockRequestSync = true;
        loan.Calculator.CalculateAll(false);
        loan.Calculator.SkipLockRequestSync = skipLockRequestSync;
      }
      catch (Exception ex)
      {
      }
    }

    private void InitializeComponent()
    {
      this.pnlPricing = new Panel();
      this.pageListNavigator1 = new PageListNavigator();
      this.pnlCurrent = new Panel();
      this.pnlHistorical = new Panel();
      this.pnlBottom = new Panel();
      this.gradientPanel1 = new GradientPanel();
      this.btnCurrentPricing = new Button();
      this.gradientPanel2 = new GradientPanel();
      this.btnHistoricalPricing = new Button();
      this.gradientPanel3 = new GradientPanel();
      this.btnSubmitCurrentPricing = new Button();
      this.label2 = new Label();
      this.btnSubmitHistoricalPricing = new Button();
      this.label1 = new Label();
      this.pnlPricing.SuspendLayout();
      this.pnlBottom.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.gradientPanel3.SuspendLayout();
      this.SuspendLayout();
      this.pnlPricing.AutoScroll = true;
      this.pnlPricing.Controls.Add((Control) this.pageListNavigator1);
      this.pnlPricing.Controls.Add((Control) this.pnlCurrent);
      this.pnlPricing.Controls.Add((Control) this.pnlHistorical);
      this.pnlPricing.Dock = DockStyle.Left;
      this.pnlPricing.Location = new Point(0, 0);
      this.pnlPricing.Margin = new Padding(0);
      this.pnlPricing.MaximumSize = new Size(687, 99999);
      this.pnlPricing.MinimumSize = new Size(687, 0);
      this.pnlPricing.Name = "pnlPricing";
      this.pnlPricing.Size = new Size(687, 646);
      this.pnlPricing.TabIndex = 3;
      this.pageListNavigator1.Font = new Font("Arial", 8f);
      this.pageListNavigator1.Location = new Point(438, 563);
      this.pageListNavigator1.Name = "pageListNavigator1";
      this.pageListNavigator1.NumberOfItems = 0;
      this.pageListNavigator1.Size = new Size(8, 8);
      this.pageListNavigator1.TabIndex = 2;
      this.pnlCurrent.BackColor = SystemColors.Control;
      this.pnlCurrent.Location = new Point(334, 0);
      this.pnlCurrent.Margin = new Padding(0);
      this.pnlCurrent.Name = "pnlCurrent";
      this.pnlCurrent.Size = new Size(336, 491);
      this.pnlCurrent.TabIndex = 1;
      this.pnlHistorical.BackColor = SystemColors.Control;
      this.pnlHistorical.Location = new Point(0, 0);
      this.pnlHistorical.Margin = new Padding(0);
      this.pnlHistorical.Name = "pnlHistorical";
      this.pnlHistorical.Size = new Size(336, 472);
      this.pnlHistorical.TabIndex = 0;
      this.pnlBottom.Controls.Add((Control) this.gradientPanel1);
      this.pnlBottom.Controls.Add((Control) this.gradientPanel2);
      this.pnlBottom.Controls.Add((Control) this.gradientPanel3);
      this.pnlBottom.Dock = DockStyle.Bottom;
      this.pnlBottom.Location = new Point(0, 646);
      this.pnlBottom.Name = "pnlBottom";
      this.pnlBottom.Size = new Size(724, 141);
      this.pnlBottom.TabIndex = 4;
      this.gradientPanel1.Controls.Add((Control) this.btnCurrentPricing);
      this.gradientPanel1.Location = new Point(334, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(353, 29);
      this.gradientPanel1.TabIndex = 5;
      this.btnCurrentPricing.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnCurrentPricing.Location = new Point(178, 3);
      this.btnCurrentPricing.Name = "btnCurrentPricing";
      this.btnCurrentPricing.Size = new Size(148, 23);
      this.btnCurrentPricing.TabIndex = 1;
      this.btnCurrentPricing.Text = "Get Current Pricing";
      this.btnCurrentPricing.UseVisualStyleBackColor = true;
      this.btnCurrentPricing.Click += new EventHandler(this.btnCurrentPricing_Click);
      this.gradientPanel2.Controls.Add((Control) this.btnHistoricalPricing);
      this.gradientPanel2.Location = new Point(0, 0);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(335, 29);
      this.gradientPanel2.TabIndex = 6;
      this.btnHistoricalPricing.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnHistoricalPricing.Location = new Point(176, 3);
      this.btnHistoricalPricing.Name = "btnHistoricalPricing";
      this.btnHistoricalPricing.Size = new Size(148, 23);
      this.btnHistoricalPricing.TabIndex = 0;
      this.btnHistoricalPricing.Text = "Get Historical Pricing";
      this.btnHistoricalPricing.UseVisualStyleBackColor = true;
      this.btnHistoricalPricing.Click += new EventHandler(this.btnHistoricalPricing_Click);
      this.gradientPanel3.Controls.Add((Control) this.btnSubmitCurrentPricing);
      this.gradientPanel3.Controls.Add((Control) this.label2);
      this.gradientPanel3.Controls.Add((Control) this.btnSubmitHistoricalPricing);
      this.gradientPanel3.Controls.Add((Control) this.label1);
      this.gradientPanel3.Location = new Point(0, 28);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(687, 111);
      this.gradientPanel3.TabIndex = 7;
      this.btnSubmitCurrentPricing.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnSubmitCurrentPricing.Location = new Point(359, 56);
      this.btnSubmitCurrentPricing.Name = "btnSubmitCurrentPricing";
      this.btnSubmitCurrentPricing.Size = new Size(160, 30);
      this.btnSubmitCurrentPricing.TabIndex = 7;
      this.btnSubmitCurrentPricing.Text = "Submit Current Pricing";
      this.btnSubmitCurrentPricing.UseVisualStyleBackColor = true;
      this.btnSubmitCurrentPricing.Click += new EventHandler(this.SubmitPricing_Click);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(323, 65);
      this.label2.Name = "label2";
      this.label2.Size = new Size(23, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "OR";
      this.btnSubmitHistoricalPricing.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnSubmitHistoricalPricing.Location = new Point(152, 56);
      this.btnSubmitHistoricalPricing.Name = "btnSubmitHistoricalPricing";
      this.btnSubmitHistoricalPricing.Size = new Size(160, 30);
      this.btnSubmitHistoricalPricing.TabIndex = 5;
      this.btnSubmitHistoricalPricing.Text = "Submit Historical Pricing";
      this.btnSubmitHistoricalPricing.UseVisualStyleBackColor = true;
      this.btnSubmitHistoricalPricing.Click += new EventHandler(this.SubmitPricing_Click);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(197, 15);
      this.label1.Name = "label1";
      this.label1.Size = new Size(276, 24);
      this.label1.TabIndex = 4;
      this.label1.Text = "New Worst Case Lock Submittal";
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.Controls.Add((Control) this.pnlPricing);
      this.Controls.Add((Control) this.pnlBottom);
      this.Name = nameof (WorstCasePricingTool);
      this.Size = new Size(724, 787);
      this.Load += new EventHandler(this.WorstCasePricingTool_Load);
      this.pnlPricing.ResumeLayout(false);
      this.pnlBottom.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      this.ResumeLayout(false);
    }

    private enum WorstCasePricingSideType
    {
      HistoricalPricing,
      CurrentPricing,
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.VOOAPanel
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.UI;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Verification
{
  public class VOOAPanel(IMainScreen mainScreen, IWorkArea workArea) : VOLPanel("Verification of Other Assets", mainScreen, workArea)
  {
    private GVColumn personCol = new GVColumn();
    private GVColumn assetTypeCol = new GVColumn();
    private GVColumn otherDescCol = new GVColumn();
    private GVColumn amountCol = new GVColumn();

    protected override void InitPanel()
    {
      this.className = nameof (VOOAPanel);
      this.actionPanel.Controls.Remove((Control) this.btnOrderIRS);
      this.actionPanel.Controls.Remove((Control) this.btnCheckIRS);
      this.actionPanel.Controls.Remove((Control) this.btnImport4506);
      this.actionPanel.Controls.Remove((Control) this.btnExport4506);
      this.paintActionPanel();
      this.personCol.Text = "Other Asset For";
      this.personCol.Width = 140;
      this.assetTypeCol.Text = " Other Asset Type";
      this.assetTypeCol.Width = 280;
      this.otherDescCol.Text = "Other Description";
      this.otherDescCol.Width = 200;
      this.amountCol.Text = "Cash or Market Value";
      this.amountCol.Width = 140;
      this.gridList.Columns.AddRange(new GVColumn[4]
      {
        this.personCol,
        this.assetTypeCol,
        this.otherDescCol,
        this.amountCol
      });
    }

    protected override GVItem addVerifToListView(int newIndex)
    {
      string str = newIndex.ToString("00");
      GVItem listView = new GVItem(this.loan.GetField("URLAROA" + str + "01"));
      listView.SubItems.Add((object) this.convertAssetTypetoUIName(this.loan.GetField("URLAROA" + str + "02")));
      listView.SubItems.Add((object) this.loan.GetField("URLAROA" + str + "04"));
      listView.SubItems.Add((object) this.loan.GetField("URLAROA" + str + "03"));
      this.gridList.Items.Add(listView);
      return listView;
    }

    protected override void refreshList()
    {
      this.gridList.Items.Clear();
      if (this.loan == null)
        return;
      int numberOfOtherAssets = this.loan.GetNumberOfOtherAssets();
      for (int newIndex = 1; newIndex <= numberOfOtherAssets; ++newIndex)
        this.addVerifToListView(newIndex);
    }

    public override void RefreshListView(object sender, EventArgs e)
    {
      this.refreshList();
      if (this.gridList.Items.Count != 0)
        this.gridList.Items[0].Selected = true;
      else
        this.lowerContentPanel.Controls.Clear();
      this.editBtn_Click((object) null, (EventArgs) null);
    }

    protected override VerificationBase NewVerificationScreen()
    {
      return (VerificationBase) new VOOA((PanelBase) this, this.mainScreen, this.workArea);
    }

    protected override int RemoveVerificationAt(int i) => this.loan.RemoveOtherAssetAt(i);

    protected override int NewVerification() => this.loan.NewOtherAsset();

    protected override void UpVerification(int i) => this.loan.UpOtherAsset(i);

    protected override void DownVerfication(int i) => this.loan.DownOtherAsset(i);

    protected override void hookupEventHandler(IInputHandler inputHandler)
    {
      if (inputHandler == null || !(inputHandler is VOOAInputHandler))
        return;
      ((VOOAInputHandler) inputHandler).VerifSummaryChanged += new VerifSummaryChangedEventHandler(this.summaryInfoChangedHandler);
    }

    private void summaryInfoChangedHandler(VerifSummaryChangeInfo info)
    {
      string empty = string.Empty;
      switch (info.ItemName)
      {
        case "URLAROA0001":
          this.gridList.SelectedItems[0].SubItems[0].Text = (string) info.ItemValue;
          break;
        case "URLAROA0002":
          this.gridList.SelectedItems[0].SubItems[1].Text = this.convertAssetTypetoUIName((string) info.ItemValue);
          break;
        case "URLAROA0004":
          this.gridList.SelectedItems[0].SubItems[2].Text = (string) info.ItemValue;
          break;
        case "URLAROA0003":
          this.gridList.SelectedItems[0].SubItems[3].Text = this.loan.GetField("URLAROA" + (this.gridList.SelectedItems[0].Index + 1).ToString("00") + "03");
          break;
      }
    }

    private string convertAssetTypetoUIName(string key)
    {
      switch (key)
      {
        case "Annuity":
          return "Annuity (FHA/VA)";
        case "Automobile":
          return "Automobile (FHA/VA)";
        case "Boat":
          return "Boat (FHA/VA)";
        case "BorrowerPrimaryHome":
          return "Borrower Primary Home (FHA/VA)";
        case "BridgeLoanNotDeposited":
          return "Bridge Loan Not Deposited (FHA/VA)";
        case "CashOnHand":
          return "Cash On Hand";
        case "EarnestMoney":
          return "Earnest Money";
        case "EarnestMoneyCashDepositTowardPurchase":
          return "Earnest Money Cash Deposit Toward Purchase";
        case "EmployerAssistance":
          return "Employer Assistance";
        case "EmployerAssistedHousing":
          return "Employer Assistance";
        case "LeasePurchaseCredit":
          return "Rent Credit";
        case "LeasePurchaseFund":
          return "Lease Purchase Fund";
        case "LotEquity":
          return "Lot Equity";
        case "NetWorthOfBusinessOwned":
          return "Net Worth Of Business Owned (FHA/VA)";
        case "PendingNetSaleProceedsFromRealEstateAssets":
          return "Pending Net Sale Proceeds From Real Estate Assets";
        case "ProceedsFromSaleOfNonRealEstateAsset":
          return "Proceeds From Sale Of Non Real Estate Asset";
        case "ProceedsFromSecuredLoan":
          return "Proceeds From Secured Loan";
        case "ProceedsFromUnsecuredLoan":
          return "Proceeds From Unsecured Loan";
        case "RecreationalVehicle":
          return "Recreational Vehicle (FHA/VA)";
        case "RelocationFunds":
          return "Relocation Funds";
        case "SavingsBond":
          return "Savings Bond (FHA/VA)";
        case "SeverancePackage":
          return "Severance Package (FHA/VA)";
        case "SweatEquity":
          return "Sweat Equity";
        case "TradeEquityFromPropertySwap":
          return "Trade Equity From Property Swap";
        default:
          return key;
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.VOGGPanel
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
  public class VOGGPanel(IMainScreen mainScreen, IWorkArea workArea) : VOLPanel("Verification of Gifts and Grants", mainScreen, workArea)
  {
    private GVColumn personCol = new GVColumn();
    private GVColumn assetTypeCol = new GVColumn();
    private GVColumn sourceCol = new GVColumn();
    private GVColumn otherDescCol = new GVColumn();
    private GVColumn depositedCol = new GVColumn();
    private GVColumn amountCol = new GVColumn();

    protected override void InitPanel()
    {
      this.className = nameof (VOGGPanel);
      this.actionPanel.Controls.Remove((Control) this.btnOrderIRS);
      this.actionPanel.Controls.Remove((Control) this.btnCheckIRS);
      this.actionPanel.Controls.Remove((Control) this.btnImport4506);
      this.actionPanel.Controls.Remove((Control) this.btnExport4506);
      this.paintActionPanel();
      this.personCol.Text = "Gift For";
      this.personCol.Width = 140;
      this.assetTypeCol.Text = "Asset Type";
      this.assetTypeCol.Width = 180;
      this.sourceCol.Text = "Source";
      this.sourceCol.Width = 150;
      this.otherDescCol.Text = "Other Description";
      this.otherDescCol.Width = 200;
      this.depositedCol.Text = "Deposited";
      this.depositedCol.Width = 100;
      this.depositedCol.TextAlign = HorizontalAlignment.Center;
      this.amountCol.Text = "Cash or Market Value";
      this.amountCol.Width = 140;
      this.amountCol.TextAlign = HorizontalAlignment.Right;
      this.gridList.Columns.AddRange(new GVColumn[6]
      {
        this.personCol,
        this.assetTypeCol,
        this.sourceCol,
        this.otherDescCol,
        this.depositedCol,
        this.amountCol
      });
    }

    protected override GVItem addVerifToListView(int newIndex)
    {
      string str = newIndex.ToString("00");
      GVItem listView = new GVItem(this.loan.GetField("URLARGG" + str + "02"));
      listView.SubItems.Add((object) this.convertAssetTypetoUIName(this.loan.GetField("URLARGG" + str + "18")));
      listView.SubItems.Add((object) this.convertSourcetoUIName(this.loan.GetField("URLARGG" + str + "19")));
      listView.SubItems.Add((object) this.loan.GetField("URLARGG" + str + "22"));
      listView.SubItems.Add((object) this.loan.GetField("URLARGG" + str + "20"));
      listView.SubItems.Add((object) this.loan.GetField("URLARGG" + str + "21"));
      this.gridList.Items.Add(listView);
      return listView;
    }

    protected override void refreshList()
    {
      this.gridList.Items.Clear();
      if (this.loan == null)
        return;
      int ofGiftsAndGrants = this.loan.GetNumberOfGiftsAndGrants();
      for (int newIndex = 1; newIndex <= ofGiftsAndGrants; ++newIndex)
        this.addVerifToListView(newIndex);
    }

    public override void RefreshListView(object sender, EventArgs e)
    {
      this.refreshList();
      if (this.gridList.Items.Count != 0)
        this.gridList.Items[0].Selected = true;
      this.editBtn_Click((object) null, (EventArgs) null);
    }

    protected override VerificationBase NewVerificationScreen()
    {
      return (VerificationBase) new VOGG((PanelBase) this, this.mainScreen, this.workArea);
    }

    protected override int RemoveVerificationAt(int i) => this.loan.RemoveGiftGrantAt(i);

    protected override int NewVerification() => this.loan.NewGiftGrant();

    protected override void UpVerification(int i) => this.loan.UpVerification("URLARGG", i);

    protected override void DownVerfication(int i) => this.loan.DownVerification("URLARGG", i);

    protected override void hookupEventHandler(IInputHandler inputHandler)
    {
      if (inputHandler == null || !(inputHandler is VOGGInputHandler))
        return;
      ((VOGGInputHandler) inputHandler).VerifSummaryChanged += new VerifSummaryChangedEventHandler(this.summaryInfoChangedHandler);
    }

    private void summaryInfoChangedHandler(VerifSummaryChangeInfo info)
    {
      switch (info.ItemName)
      {
        case "URLARGG0002":
          this.gridList.SelectedItems[0].SubItems[0].Text = (string) info.ItemValue;
          break;
        case "URLARGG0018":
          this.gridList.SelectedItems[0].SubItems[1].Text = this.convertAssetTypetoUIName((string) info.ItemValue);
          break;
        case "URLARGG0019":
          this.gridList.SelectedItems[0].SubItems[2].Text = this.convertSourcetoUIName((string) info.ItemValue);
          break;
        case "URLARGG0022":
          this.gridList.SelectedItems[0].SubItems[3].Text = (string) info.ItemValue;
          break;
        case "URLARGG0020":
          this.gridList.SelectedItems[0].SubItems[4].Text = (string) info.ItemValue;
          break;
        case "URLARGG0021":
          this.gridList.SelectedItems[0].SubItems[5].Text = (string) info.ItemValue;
          break;
      }
    }

    private string convertAssetTypetoUIName(string key)
    {
      switch (key)
      {
        case "GiftOfCash":
          return "Gift of Cash";
        case "GiftOfPropertyEquity":
          return "Gift of Property Equity";
        case "Grant":
          return "Grant";
        default:
          return key;
      }
    }

    private string convertSourcetoUIName(string key)
    {
      switch (key)
      {
        case "CommunityNonprofit":
          return "Community Nonprofit";
        case "Employer":
          return "Employer";
        case "FederalAgency":
          return "Federal Agency";
        case "LocalAgency":
          return "Local Agency";
        case "NonParentRelative":
          return "Non Parent Relative";
        case "Other":
          return "Other";
        case "Relative":
          return "Relative";
        case "ReligiousNonprofit":
          return "Religious Nonprofit";
        case "StateAgency":
          return "State Agency";
        case "UnmarriedPartner":
          return "Unmarried Partner";
        case "UnrelatedFriend":
          return "Unrelated Friend";
        default:
          return key;
      }
    }
  }
}

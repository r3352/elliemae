// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.VODPanel
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
  public class VODPanel(IMainScreen mainScreen, IWorkArea workArea) : VOLPanel("VOD", mainScreen, workArea)
  {
    private GVColumn personCol = new GVColumn();
    private GVColumn bankCol = new GVColumn();
    private GVColumn balanceCol = new GVColumn();

    protected override void InitPanel()
    {
      this.className = nameof (VODPanel);
      this.actionPanel.Controls.Remove((Control) this.btnUp);
      this.actionPanel.Controls.Remove((Control) this.btnDown);
      this.actionPanel.Controls.Remove((Control) this.btnOrderIRS);
      this.actionPanel.Controls.Remove((Control) this.btnCheckIRS);
      this.actionPanel.Controls.Remove((Control) this.btnImport4506);
      this.actionPanel.Controls.Remove((Control) this.btnExport4506);
      this.paintActionPanel();
      this.personCol.Text = "Belong To";
      this.personCol.Width = 140;
      this.bankCol.Text = "Bank/S&L/Credit Union";
      this.bankCol.Width = 200;
      this.balanceCol.Text = "Balance";
      if (this.loan.GetField("1825") == "2020")
        this.balanceCol.Text = "Cash Or Market Value";
      this.balanceCol.Width = 140;
      this.balanceCol.TextAlign = HorizontalAlignment.Right;
      this.gridList.Columns.AddRange(new GVColumn[3]
      {
        this.personCol,
        this.bankCol,
        this.balanceCol
      });
    }

    protected override GVItem addVerifToListView(int newIndex)
    {
      string str = newIndex.ToString("00");
      GVItem listView = new GVItem(this.loan.GetField("DD" + str + "24"));
      listView.SubItems.Add((object) this.loan.GetField("DD" + str + "02"));
      listView.SubItems.Add((object) this.loan.GetField("DD" + str + "34"));
      this.gridList.Items.Add(listView);
      return listView;
    }

    protected override void refreshList()
    {
      this.gridList.Items.Clear();
      if (this.loan == null)
        return;
      int numberOfDeposits = this.loan.GetNumberOfDeposits();
      for (int newIndex = 1; newIndex <= numberOfDeposits; ++newIndex)
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
      return (VerificationBase) new VOD((PanelBase) this, this.mainScreen, this.workArea);
    }

    protected override int RemoveVerificationAt(int i) => this.loan.RemoveDepositAt(i);

    protected override int NewVerification() => this.loan.NewDeposit();

    protected override void UpVerification(int i)
    {
    }

    protected override void DownVerfication(int i)
    {
    }

    protected override void hookupEventHandler(IInputHandler inputHandler)
    {
      if (inputHandler == null || !(inputHandler is VODInputHandler))
        return;
      ((VODInputHandler) inputHandler).VerifSummaryChanged += new VerifSummaryChangedEventHandler(this.summaryInfoChangedHandler);
    }

    private void summaryInfoChangedHandler(VerifSummaryChangeInfo info)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string itemName = info.ItemName;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(itemName))
      {
        case 1370428950:
          if (!(itemName == "DD0049"))
            return;
          break;
        case 1387206569:
          if (!(itemName == "DD0048"))
            return;
          break;
        case 1420614712:
          if (!(itemName == "DD0050"))
            return;
          break;
        case 1437392331:
          if (!(itemName == "DD0051"))
            return;
          break;
        case 1522413259:
          if (!(itemName == "DD0024"))
            return;
          this.gridList.SelectedItems[0].SubItems[0].Text = (string) info.ItemValue;
          return;
        case 1606301354:
          if (!(itemName == "DD0023"))
            return;
          break;
        case 3384184515:
          if (!(itemName == "DD0015"))
            return;
          break;
        case 3451294991:
          if (!(itemName == "DD0011"))
            return;
          break;
        case 3501774943:
          if (!(itemName == "DD0002"))
            return;
          this.gridList.SelectedItems[0].SubItems[1].Text = (string) info.ItemValue;
          return;
        case 3585515943:
          if (!(itemName == "DD0019"))
            return;
          break;
        default:
          return;
      }
      this.gridList.SelectedItems[0].SubItems[2].Text = this.loan.GetField("DD" + (this.gridList.SelectedItems[0].Index + 1).ToString("00") + "34");
    }
  }
}

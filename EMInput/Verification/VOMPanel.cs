// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.VOMPanel
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Verification
{
  public class VOMPanel : VOLPanel
  {
    private GVColumn propertyCol = new GVColumn();
    private GVColumn addressCol = new GVColumn();
    private bool loanIsNotNull;
    private GVColumn sourceOfIncomeDataCol = new GVColumn();

    protected override void InitPanel()
    {
      this.className = nameof (VOMPanel);
      this.actionPanel.Controls.Remove((Control) this.btnOrderIRS);
      this.actionPanel.Controls.Remove((Control) this.btnCheckIRS);
      this.actionPanel.Controls.Remove((Control) this.btnImport4506);
      this.actionPanel.Controls.Remove((Control) this.btnExport4506);
      this.paintActionPanel();
      this.propertyCol.Text = "Property Is";
      this.propertyCol.Width = 140;
      this.addressCol.Text = "Address";
      this.addressCol.Width = 400;
      this.sourceOfIncomeDataCol.Text = "Source";
      this.sourceOfIncomeDataCol.Width = 300;
      this.gridList.Columns.AddRange(new GVColumn[3]
      {
        this.propertyCol,
        this.addressCol,
        this.sourceOfIncomeDataCol
      });
      this.Text = "VOM Panel";
    }

    public VOMPanel(IMainScreen mainScreen, IWorkArea workArea)
      : base("VOM", mainScreen, workArea)
    {
      this.loanIsNotNull = false;
    }

    public VOMPanel(IMainScreen mainScreen, IWorkArea workArea, LoanData loanData)
      : base("VOM", mainScreen, workArea, loanData)
    {
      this.loanIsNotNull = true;
    }

    protected override void hookupEventHandler(IInputHandler inputHandler)
    {
      if (inputHandler == null || !(inputHandler is VOMInputHandler))
        return;
      ((VOMInputHandler) inputHandler).VerifSummaryChanged += new VerifSummaryChangedEventHandler(this.summaryInfoChangedHandler);
    }

    private void summaryInfoChangedHandler(VerifSummaryChangeInfo info)
    {
      int num = 0;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string itemName = info.ItemName;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(itemName))
      {
        case 336109136:
          if (!(itemName == "FM0004"))
            return;
          string str1 = (this.gridList.SelectedItems[0].Index + 1).ToString("00");
          this.gridList.SelectedItems[0].SubItems[1].Text = info.ItemValue.ToString() + ", " + this.loan.GetField("FM" + str1 + "06") + ", " + this.loan.GetField("FM" + str1 + "07") + " " + this.loan.GetField("FM" + str1 + "08");
          return;
        case 369664374:
          if (!(itemName == "FM0006"))
            return;
          string str2 = (this.gridList.SelectedItems[0].Index + 1).ToString("00");
          this.gridList.SelectedItems[0].SubItems[1].Text = this.loan.GetField("FM" + str2 + "04") + ", " + info.ItemValue + ", " + this.loan.GetField("FM" + str2 + "07") + " " + this.loan.GetField("FM" + str2 + "08");
          return;
        case 386441993:
          if (!(itemName == "FM0007"))
            return;
          string str3 = (this.gridList.SelectedItems[0].Index + 1).ToString("00");
          this.gridList.SelectedItems[0].SubItems[1].Text = this.loan.GetField("FM" + str3 + "04") + ", " + this.loan.GetField("FM" + str3 + "06") + ", " + info.ItemValue + " " + this.loan.GetField("FM" + str3 + "08");
          return;
        case 402528064:
          if (!(itemName == "FM0048"))
            return;
          break;
        case 470624278:
          if (!(itemName == "FM0028"))
            return;
          num = this.gridList.SelectedItems[0].Index + 1;
          string n = this.gridList.SelectedItems[0].Tag.ToString();
          this.refreshList();
          this.SelectVOM(n);
          return;
        case 537440564:
          if (!(itemName == "FM0008"))
            return;
          string str4 = (this.gridList.SelectedItems[0].Index + 1).ToString("00");
          this.gridList.SelectedItems[0].SubItems[1].Text = this.loan.GetField("FM" + str4 + "04") + ", " + this.loan.GetField("FM" + str4 + "06") + ", " + this.loan.GetField("FM" + str4 + "07") + " " + info.ItemValue;
          return;
        case 553526635:
          if (!(itemName == "FM0041"))
            return;
          switch ((string) info.ItemValue)
          {
            case "PrimaryResidence":
              this.gridList.SelectedItems[0].SubItems[0].Text = "Primary Residence";
              return;
            case "SecondHome":
              this.gridList.SelectedItems[0].SubItems[0].Text = "Second Home";
              return;
            case "Investor":
              this.gridList.SelectedItems[0].SubItems[0].Text = "Investment Property";
              return;
            case "FHASecondaryResidence":
              this.gridList.SelectedItems[0].SubItems[0].Text = "FHA Secondary Residence";
              return;
            default:
              return;
          }
        case 654192349:
          if (!(itemName == "FM0047"))
            return;
          break;
        case 2785097057:
          if (!(itemName == "FM0050"))
            return;
          break;
        default:
          return;
      }
      string str5 = (this.gridList.SelectedItems[0].Index + 1).ToString("00");
      this.gridList.SelectedItems[0].SubItems[1].Text = this.loan.GetField("FM" + str5 + "04") + ", " + this.loan.GetField("FM" + str5 + "06") + ", " + this.loan.GetField("FM" + str5 + "07") + " " + this.loan.GetField("FM" + str5 + "08");
    }

    protected override GVItem addVerifToListView(int newIndex)
    {
      string str1 = newIndex.ToString("00");
      string text;
      switch (this.loan.GetField("FM" + str1 + "41"))
      {
        case "PrimaryResidence":
          text = "Primary Residence";
          break;
        case "SecondHome":
          text = "Second Home";
          break;
        case "Investor":
          text = "Investment Property";
          break;
        case "FHASecondaryResidence":
          text = "FHA Secondary Residence";
          break;
        default:
          text = string.Empty;
          break;
      }
      GVItem listView = new GVItem(text);
      string str2 = this.loan.GetField("FM" + str1 + "04") + ", " + this.loan.GetField("FM" + str1 + "06") + ", " + this.loan.GetField("FM" + str1 + "07") + " " + this.loan.GetField("FM" + str1 + "08");
      listView.SubItems.Add((object) str2);
      listView.SubItems.Add((object) this.loan.GetField("FM" + str1 + "60"));
      listView.Tag = (object) this.loan.GetField("FM" + str1 + "43");
      this.gridList.Items.Add(listView);
      return listView;
    }

    protected override void refreshList()
    {
      this.gridList.Items.Clear();
      if (this.loan == null)
        return;
      int numberOfMortgages = this.loan.GetNumberOfMortgages();
      for (int newIndex = 1; newIndex <= numberOfMortgages; ++newIndex)
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
      return this.loanIsNotNull ? (VerificationBase) new VOM((PanelBase) this, this.mainScreen, this.workArea, this.loan) : (VerificationBase) new VOM((PanelBase) this, this.mainScreen, this.workArea);
    }

    protected override int RemoveVerificationAt(int i) => this.loan.RemoveMortgageAt(i);

    protected override int NewVerification()
    {
      NewMortgageDialog newMortgageDialog = new NewMortgageDialog(this.loan, string.Empty);
      return newMortgageDialog.ShowDialog((IWin32Window) this.workArea) == DialogResult.OK ? this.loan.NewMortgage(newMortgageDialog.SelectedVOL) : -1;
    }

    protected override void UpVerification(int i) => this.loan.UpMortgage(i);

    protected override void DownVerfication(int i) => this.loan.DownMortgage(i);

    private void SelectVOM(string n)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridList.Items)
      {
        if ((string) gvItem.Tag == n)
          gvItem.Selected = true;
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.DISPanel
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
  public class DISPanel(IMainScreen mainScreen, IWorkArea workArea) : VOLPanel("Disaster Declarations", mainScreen, workArea)
  {
    private GVColumn decStringCol = new GVColumn();
    private GVColumn disNumberCol = new GVColumn();
    private GVColumn decTypeCol = new GVColumn();
    private GVColumn decDateCol = new GVColumn();
    private GVColumn yearCol = new GVColumn();
    private GVColumn typeCol = new GVColumn();
    private GVColumn decTitleCol = new GVColumn();

    protected override void InitPanel()
    {
      this.className = nameof (DISPanel);
      this.actionPanel.Controls.Remove((Control) this.btnAddDoc);
      this.actionPanel.Controls.Remove((Control) this.verSeparator);
      this.actionPanel.Controls.Remove((Control) this.btnOrderIRS);
      this.actionPanel.Controls.Remove((Control) this.btnCheckIRS);
      this.actionPanel.Controls.Remove((Control) this.btnImport4506);
      this.actionPanel.Controls.Remove((Control) this.btnExport4506);
      this.paintActionPanel();
      this.decStringCol.Text = "FEMA Declaration String";
      this.decStringCol.Width = 140;
      this.disNumberCol.Text = "Disaster Number";
      this.disNumberCol.Width = 120;
      this.decTypeCol.Text = "Declaration Type";
      this.decTypeCol.Width = 80;
      this.decDateCol.Text = "Declaration Date";
      this.decDateCol.Width = 100;
      this.yearCol.Text = "Fiscal Year";
      this.yearCol.Width = 100;
      this.typeCol.Text = "Incident Type";
      this.typeCol.Width = 140;
      this.decTitleCol.Text = "Declaration Title";
      this.decTitleCol.Width = 140;
      this.gridList.Columns.AddRange(new GVColumn[7]
      {
        this.decStringCol,
        this.disNumberCol,
        this.decTypeCol,
        this.decDateCol,
        this.yearCol,
        this.typeCol,
        this.decTitleCol
      });
    }

    protected override GVItem addVerifToListView(int newIndex)
    {
      string str = newIndex.ToString("00");
      GVItem listView = new GVItem(this.loan.GetField("FEMA" + str + "01"));
      listView.SubItems.Add((object) this.loan.GetField("FEMA" + str + "02"));
      listView.SubItems.Add((object) this.loan.GetField("FEMA" + str + "03"));
      listView.SubItems.Add((object) this.loan.GetField("FEMA" + str + "04"));
      listView.SubItems.Add((object) this.loan.GetField("FEMA" + str + "05"));
      listView.SubItems.Add((object) this.loan.GetField("FEMA" + str + "06"));
      listView.SubItems.Add((object) this.loan.GetField("FEMA" + str + "07"));
      this.gridList.Items.Add(listView);
      return listView;
    }

    protected override void refreshList()
    {
      this.gridList.Items.Clear();
      if (this.loan == null)
        return;
      int numberOfDisasters = this.loan.GetNumberOfDisasters();
      for (int newIndex = 1; newIndex <= numberOfDisasters; ++newIndex)
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
      return (VerificationBase) new DIS((PanelBase) this, this.mainScreen, this.workArea);
    }

    protected override int RemoveVerificationAt(int i) => this.loan.RemoveDisasterAt(i);

    protected override int NewVerification() => this.loan.NewDisaster();

    protected override void UpVerification(int i) => this.loan.UpDisaster(i);

    protected override void DownVerfication(int i) => this.loan.DownDisaster(i);

    protected override void hookupEventHandler(IInputHandler inputHandler)
    {
      if (inputHandler == null || !(inputHandler is DISASTERInputHandler))
        return;
      ((DISASTERInputHandler) inputHandler).VerifSummaryChanged += new VerifSummaryChangedEventHandler(this.summaryInfoChangedHandler);
    }

    private void summaryInfoChangedHandler(VerifSummaryChangeInfo info)
    {
      string empty = string.Empty;
      string itemName = info.ItemName;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(itemName))
      {
        case 2460971080:
          if (!(itemName == "FEMA0002"))
            break;
          this.gridList.SelectedItems[0].SubItems[1].Text = (string) info.ItemValue;
          break;
        case 2477748699:
          if (!(itemName == "FEMA0003"))
            break;
          this.gridList.SelectedItems[0].SubItems[2].Text = (string) info.ItemValue;
          break;
        case 2511303937:
          if (!(itemName == "FEMA0001"))
            break;
          this.gridList.SelectedItems[0].SubItems[0].Text = (string) info.ItemValue;
          break;
        case 2528081556:
          if (!(itemName == "FEMA0006"))
            break;
          this.gridList.SelectedItems[0].SubItems[5].Text = (string) info.ItemValue;
          break;
        case 2544859175:
          if (!(itemName == "FEMA0007"))
            break;
          this.gridList.SelectedItems[0].SubItems[6].Text = (string) info.ItemValue;
          break;
        case 2561636794:
          if (!(itemName == "FEMA0004"))
            break;
          this.gridList.SelectedItems[0].SubItems[3].Text = (string) info.ItemValue;
          break;
        case 2578414413:
          if (!(itemName == "FEMA0005"))
            break;
          this.gridList.SelectedItems[0].SubItems[4].Text = (string) info.ItemValue;
          break;
      }
    }
  }
}

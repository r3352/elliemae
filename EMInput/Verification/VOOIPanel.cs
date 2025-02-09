// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.VOOIPanel
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.UI;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Verification
{
  public class VOOIPanel(IMainScreen mainScreen, IWorkArea workArea) : VOLPanel("Verification of Other Income", mainScreen, workArea)
  {
    private GVColumn personCol = new GVColumn();
    private GVColumn sourceCol = new GVColumn();
    private GVColumn otherDescCol = new GVColumn();
    private GVColumn amountCol = new GVColumn();
    private GVColumn sourceOfIncomeDataCol = new GVColumn();

    protected override void InitPanel()
    {
      this.className = nameof (VOOIPanel);
      this.actionPanel.Controls.Remove((Control) this.btnOrderIRS);
      this.actionPanel.Controls.Remove((Control) this.btnCheckIRS);
      this.actionPanel.Controls.Remove((Control) this.btnImport4506);
      this.actionPanel.Controls.Remove((Control) this.btnExport4506);
      this.paintActionPanel();
      this.personCol.Text = "Other Income Is For";
      this.personCol.Width = 140;
      this.sourceCol.Text = "Income Source";
      this.sourceCol.Width = 200;
      this.otherDescCol.Text = "Other Description";
      this.otherDescCol.Width = 200;
      this.amountCol.Text = "Monthly Income";
      this.amountCol.Width = 140;
      this.amountCol.TextAlign = HorizontalAlignment.Right;
      this.sourceOfIncomeDataCol.Text = "Source";
      this.sourceOfIncomeDataCol.Width = 300;
      this.gridList.Columns.AddRange(new GVColumn[5]
      {
        this.personCol,
        this.sourceCol,
        this.otherDescCol,
        this.amountCol,
        this.sourceOfIncomeDataCol
      });
    }

    protected override GVItem addVerifToListView(int newIndex)
    {
      string str = newIndex.ToString("00");
      GVItem listView = new GVItem(this.loan.GetField("URLAROIS" + str + "02"));
      listView.SubItems.Add((object) this.convertDescriptionToUIName(this.loan.GetField("URLAROIS" + str + "18")));
      listView.SubItems.Add((object) this.loan.GetField("URLAROIS" + str + "19"));
      listView.SubItems.Add((object) this.loan.GetField("URLAROIS" + str + "22"));
      listView.SubItems.Add((object) this.loan.GetField("URLAROIS" + str + "23"));
      this.gridList.Items.Add(listView);
      return listView;
    }

    protected override void refreshList()
    {
      this.gridList.Items.Clear();
      if (this.loan == null)
        return;
      int otherIncomeSources = this.loan.GetNumberOfOtherIncomeSources();
      for (int newIndex = 1; newIndex <= otherIncomeSources; ++newIndex)
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
      return (VerificationBase) new VOOI((PanelBase) this, this.mainScreen, this.workArea);
    }

    protected override int RemoveVerificationAt(int i) => this.loan.RemoveOtherIncomeSourceAt(i);

    protected override int NewVerification() => this.loan.NewOtherIncomeSource();

    protected override void UpVerification(int i) => this.loan.UpVerification("URLAROIS", i);

    protected override void DownVerfication(int i) => this.loan.DownVerification("URLAROIS", i);

    protected override void hookupEventHandler(IInputHandler inputHandler)
    {
      if (inputHandler == null || !(inputHandler is VOOIInputHandler))
        return;
      ((VOOIInputHandler) inputHandler).VerifSummaryChanged += new VerifSummaryChangedEventHandler(this.summaryInfoChangedHandler);
    }

    private void summaryInfoChangedHandler(VerifSummaryChangeInfo info)
    {
      switch (info.ItemName)
      {
        case "URLAROIS0002":
          this.gridList.SelectedItems[0].SubItems[0].Text = (string) info.ItemValue;
          break;
        case "URLAROIS0018":
          this.gridList.SelectedItems[0].SubItems[1].Text = this.convertDescriptionToUIName((string) info.ItemValue);
          break;
        case "URLAROIS0019":
          this.gridList.SelectedItems[0].SubItems[2].Text = (string) info.ItemValue;
          break;
        case "URLAROIS0022":
          this.gridList.SelectedItems[0].SubItems[3].Text = (string) info.ItemValue;
          break;
      }
    }

    private string convertDescriptionToUIName(string key)
    {
      FieldDefinition field = EncompassFields.GetField("URLAROIS0018");
      for (int index = 0; index < field.Options.Count; ++index)
      {
        if (string.Compare(field.Options[index].Value, key, true) == 0)
          return field.Options[index].Text;
      }
      return "";
    }
  }
}

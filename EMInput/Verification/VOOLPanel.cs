// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.VOOLPanel
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
  public class VOOLPanel : VOLPanel
  {
    private GVColumn dipositorOtherDescCol = new GVColumn();
    private GVColumn dipositorAppliedToCol = new GVColumn();
    private GVColumn dipositorDescCol = new GVColumn();
    private GVColumn dipositorMonthlyAmtCol = new GVColumn();
    private FieldDefinition urlarol0002Options;

    protected override void InitPanel()
    {
      this.className = nameof (VOOLPanel);
      this.actionPanel.Controls.Remove((Control) this.btnOrderIRS);
      this.actionPanel.Controls.Remove((Control) this.btnCheckIRS);
      this.actionPanel.Controls.Remove((Control) this.btnImport4506);
      this.actionPanel.Controls.Remove((Control) this.btnExport4506);
      this.actionPanel.Controls.Remove((Control) this.verSeparator);
      this.actionPanel.Controls.Remove((Control) this.btnAddDoc);
      this.paintActionPanel();
      this.dipositorOtherDescCol.Text = "Other Description";
      this.dipositorOtherDescCol.Width = 150;
      this.dipositorAppliedToCol.Text = "Applies To";
      this.dipositorAppliedToCol.Width = 100;
      this.dipositorDescCol.Text = "Description";
      this.dipositorDescCol.Width = 200;
      this.dipositorMonthlyAmtCol.Text = "Monthly Amount";
      this.dipositorMonthlyAmtCol.Width = 140;
      this.dipositorMonthlyAmtCol.TextAlign = HorizontalAlignment.Right;
      this.gridList.Columns.AddRange(new GVColumn[4]
      {
        this.dipositorAppliedToCol,
        this.dipositorDescCol,
        this.dipositorOtherDescCol,
        this.dipositorMonthlyAmtCol
      });
    }

    public VOOLPanel(IMainScreen mainScreen, IWorkArea workArea)
      : base("VOOL", mainScreen, workArea)
    {
      this.urlarol0002Options = EncompassFields.GetField("URLAROL0002");
    }

    protected override GVItem addVerifToListView(int newIndex)
    {
      string str = newIndex.ToString("00");
      GVItem listView = new GVItem(this.loan.GetField("URLAROL" + str + "01"));
      listView.SubItems.Add((object) this.mapVoolTypeValue(this.loan.GetField("URLAROL" + str + "02"), true));
      listView.SubItems.Add((object) this.loan.GetField("URLAROL" + str + "04"));
      listView.SubItems.Add((object) this.loan.GetField("URLAROL" + str + "03"));
      this.gridList.Items.Add(listView);
      return listView;
    }

    protected override void refreshList()
    {
      this.gridList.Items.Clear();
      if (this.loan == null)
        return;
      int ofOtherLiability = this.loan.GetNumberOfOtherLiability();
      for (int newIndex = 1; newIndex <= ofOtherLiability; ++newIndex)
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
      return (VerificationBase) new VOOL((PanelBase) this, this.mainScreen, this.workArea);
    }

    protected override int RemoveVerificationAt(int i) => this.loan.RemoveOtherLiabilityAt(i);

    protected override int NewVerification() => this.loan.NewOtherLiability();

    protected override void UpVerification(int i) => this.loan.UpVerification("URLAROL", i);

    protected override void DownVerfication(int i) => this.loan.DownVerification("URLAROL", i);

    protected override void hookupEventHandler(IInputHandler inputHandler)
    {
      if (inputHandler == null || !(inputHandler is VOOLInputHandler))
        return;
      ((VOOLInputHandler) inputHandler).VerifSummaryChanged += new VerifSummaryChangedEventHandler(this.summaryInfoChangedHandler);
    }

    private string mapVoolTypeValue(string value, bool mapToUi)
    {
      if (this.urlarol0002Options == null || this.urlarol0002Options.Options == null)
        return value;
      for (int index = 0; index < this.urlarol0002Options.Options.Count; ++index)
      {
        if (mapToUi)
        {
          if (value == this.urlarol0002Options.Options[index].Value)
            return this.urlarol0002Options.Options[index].Text;
        }
        else if (value == this.urlarol0002Options.Options[index].Text)
          return this.urlarol0002Options.Options[index].Value;
      }
      return value;
    }

    private void summaryInfoChangedHandler(VerifSummaryChangeInfo info)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      switch (info.ItemName)
      {
        case "URLAROL0001":
          this.gridList.SelectedItems[0].SubItems[0].Text = (string) info.ItemValue;
          break;
        case "URLAROL0002":
          this.gridList.SelectedItems[0].SubItems[1].Text = this.mapVoolTypeValue((string) info.ItemValue, true);
          break;
        case "URLAROL0004":
          this.gridList.SelectedItems[0].SubItems[2].Text = (string) info.ItemValue;
          break;
        case "URLAROL0003":
          this.gridList.SelectedItems[0].SubItems[3].Text = (string) info.ItemValue;
          break;
      }
    }
  }
}

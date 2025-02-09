// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.TAX4506Panel
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Verification
{
  public class TAX4506Panel : VOLPanel
  {
    private const string TAXTYPE = "AR";
    private GVColumn requestCol = new GVColumn();
    private GVColumn typeCol = new GVColumn();
    private GVColumn yearCol = new GVColumn();
    private string originalTitle = "Request for Copy of Tax Return";
    private string helpTopic;

    protected override void InitPanel()
    {
      this.className = nameof (TAX4506Panel);
      this.actionPanel.Controls.Remove((Control) this.btnOrderIRS);
      this.actionPanel.Controls.Remove((Control) this.btnCheckIRS);
      this.paintActionPanel();
      this.requestCol.Text = "Request For";
      this.requestCol.Width = 140;
      this.typeCol.Text = "Type";
      this.typeCol.Width = 200;
      this.yearCol.Text = "Year";
      this.yearCol.Width = 140;
      this.gridList.Columns.AddRange(new GVColumn[3]
      {
        this.requestCol,
        this.typeCol,
        this.yearCol
      });
    }

    public TAX4506Panel(IMainScreen mainScreen, IWorkArea workArea)
      : base("Request for Copy of Tax Return", mainScreen, workArea)
    {
      this.AddHelpIconToTitle("4506_4506T_VersionInfo");
    }

    protected override GVItem addVerifToListView(int newIndex)
    {
      string str = newIndex.ToString("00");
      GVItem listView = new GVItem(this.loan.GetField("AR" + str + "01"));
      listView.SubItems.Add((object) this.loan.GetField("AR" + str + "24"));
      listView.SubItems.Add((object) this.retrieveTaxYears(newIndex.ToString("00")));
      this.gridList.Items.Add(listView);
      return listView;
    }

    protected override void refreshList()
    {
      this.gridList.Items.Clear();
      if (this.loan == null)
        return;
      int numberOfTaX4506Ts = this.loan.GetNumberOfTAX4506Ts(true);
      this.gridList.BeginUpdate();
      for (int newIndex = 1; newIndex <= numberOfTaX4506Ts; ++newIndex)
        this.addVerifToListView(newIndex);
      this.gridList.EndUpdate();
    }

    public override void RefreshListView(object sender, EventArgs e)
    {
      this.refreshList();
      if (this.gridList.Items.Count != 0)
        this.gridList.Items[0].Selected = true;
      this.editBtn_Click((object) null, (EventArgs) null);
      this.btnAddDoc.Enabled = this.btnOrderIRS.Enabled = this.btnCheckIRS.Enabled = this.btnDelete.Enabled = this.btnExport4506.Enabled = this.gridList.SelectedItems.Count == 1;
      this.btnUp.Enabled = this.btnDown.Enabled = this.gridList.SelectedItems.Count == 1 && this.gridList.Items.Count > 1;
      this.btnImport4506.Enabled = this.checkIRS4506Class();
    }

    protected override VerificationBase NewVerificationScreen()
    {
      return (VerificationBase) new TAX4506((PanelBase) this, this.mainScreen, this.workArea);
    }

    protected override int RemoveVerificationAt(int i) => this.loan.RemoveTAX4506TAt(i, true);

    protected override int NewVerification() => this.loan.NewTAX4506T(true);

    protected override void upBtn_Click(object sender, EventArgs e)
    {
      if (this.gridList.SelectedItems.Count != 0)
      {
        int index = this.gridList.SelectedItems[0].Index;
        if (index == 0 || !Session.LoanDataMgr.LockLoanWithExclusiveA())
          return;
        InputHandlerBase inputHandler = (InputHandlerBase) this.ver.GetInputHandler();
        this.gridList.SelectedItems.Clear();
        this.UpVerification(index);
        this.refreshList();
        this.gridList.Items[index - 1].Selected = true;
        inputHandler?.RefreshContents();
        this.editBtn_Click((object) null, (EventArgs) null);
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a record first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    protected override void UpVerification(int i) => this.loan.UpTAX4506T(i, true);

    protected override void DownVerfication(int i) => this.loan.DownTAX4506T(i, true);

    protected override void downBtn_Click(object sender, EventArgs e)
    {
      if (this.gridList.SelectedItems.Count != 0)
      {
        int index = this.gridList.SelectedItems[0].Index;
        if (index >= this.gridList.Items.Count - 1 || !Session.LoanDataMgr.LockLoanWithExclusiveA())
          return;
        InputHandlerBase inputHandler = (InputHandlerBase) this.ver.GetInputHandler();
        this.gridList.SelectedItems.Clear();
        this.DownVerfication(index);
        this.refreshList();
        this.gridList.Items[index + 1].Selected = true;
        inputHandler?.RefreshContents();
        this.editBtn_Click((object) null, (EventArgs) null);
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a record first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    protected override void hookupEventHandler(IInputHandler inputHandler)
    {
      if (inputHandler == null || !(inputHandler is TAX4506InputHandler))
        return;
      ((TAX4506InputHandler) inputHandler).VerifSummaryChanged += new VerifSummaryChangedEventHandler(this.summaryInfoChangedHandler);
    }

    private void summaryInfoChangedHandler(VerifSummaryChangeInfo info)
    {
      string sInd = (this.gridList.SelectedItems[0].Index + 1).ToString("00");
      string itemName = info.ItemName;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(itemName))
      {
        case 964590573:
          if (!(itemName == "AR0001"))
            return;
          this.gridList.SelectedItems[0].SubItems[0].Text = (string) info.ItemValue;
          return;
        case 2776176067:
          if (!(itemName == "AR0029"))
            return;
          break;
        case 2960729876:
          if (!(itemName == "AR0024"))
            return;
          this.gridList.SelectedItems[0].SubItems[1].Text = (string) info.ItemValue;
          return;
        case 2977507495:
          if (!(itemName == "AR0025"))
            return;
          break;
        case 2978640328:
          if (!(itemName == "AR0055"))
            return;
          break;
        case 2994285114:
          if (!(itemName == "AR0026"))
            return;
          break;
        case 2994432209:
          if (!(itemName == "AR0030"))
            return;
          break;
        case 2995417947:
          if (!(itemName == "AR0054"))
            return;
          break;
        case 3028973185:
          if (!(itemName == "AR0056"))
            return;
          break;
        case 3079306042:
          if (!(itemName == "AR0053"))
            return;
          break;
        default:
          return;
      }
      this.gridList.SelectedItems[0].SubItems[2].Text = this.retrieveTaxYears(sInd);
    }

    private string retrieveTaxYears(string sInd)
    {
      string str1 = string.Empty;
      DateTime date1 = Utils.ParseDate((object) this.loan.GetField("AR" + sInd + "25"));
      int year;
      if (date1 != DateTime.MinValue)
      {
        year = date1.Year;
        str1 = year.ToString();
      }
      DateTime date2 = Utils.ParseDate((object) this.loan.GetField("AR" + sInd + "26"));
      if (date2 != DateTime.MinValue)
      {
        string str2 = str1;
        string str3 = str1 != string.Empty ? "," : "";
        year = date2.Year;
        string str4 = year.ToString();
        str1 = str2 + str3 + str4;
      }
      DateTime date3 = Utils.ParseDate((object) this.loan.GetField("AR" + sInd + "29"));
      if (date3 != DateTime.MinValue)
      {
        string str5 = str1;
        string str6 = str1 != string.Empty ? "," : "";
        year = date3.Year;
        string str7 = year.ToString();
        str1 = str5 + str6 + str7;
      }
      DateTime date4 = Utils.ParseDate((object) this.loan.GetField("AR" + sInd + "30"));
      if (date4 != DateTime.MinValue)
      {
        string str8 = str1;
        string str9 = str1 != string.Empty ? "," : "";
        year = date4.Year;
        string str10 = year.ToString();
        str1 = str8 + str9 + str10;
      }
      for (int index = 53; index <= 56; ++index)
      {
        DateTime date5 = Utils.ParseDate((object) this.loan.GetField("AR" + sInd + index.ToString("00")));
        if (date5 != DateTime.MinValue)
        {
          string str11 = str1;
          string str12 = str1 != string.Empty ? "," : "";
          year = date5.Year;
          string str13 = year.ToString();
          str1 = str11 + str12 + str13;
        }
      }
      return str1;
    }

    public new void SetHelpTopic(string helpTopic)
    {
      if (string.IsNullOrEmpty(JedHelp.GetMapId(helpTopic)))
        return;
      this.helpTopic = helpTopic;
    }

    public override string GetHelpTargetName()
    {
      return this.helpTopic == null ? this.originalTitle : this.helpTopic;
    }

    protected override void btnExport4506_Click(object sender, EventArgs e)
    {
      if (this.gridList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a record to export new data to classic form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int recordIndex = this.gridList.SelectedItems[0].Index + 1;
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to copy data from selected record to the Request for Copy of Tax Return (Classic) form?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
        {
          this.gridList.Items[recordIndex - 1].Selected = true;
        }
        else
        {
          this.copyToClassic(recordIndex);
          this.refreshList();
          this.gridList.Items[recordIndex - 1].Selected = true;
          this.btnImport4506.Enabled = this.checkIRS4506Class();
        }
      }
    }

    protected override void btnImport4506_Click(object sender, EventArgs e)
    {
      int recordIndex = this.gridList.SelectedItems.Count == 0 ? -1 : this.gridList.SelectedItems[0].Index + 1;
      bool flag = recordIndex == -1;
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to copy data to " + (this.gridList.SelectedItems.Count == 0 ? "a new " : "selected") + " record from the Request for Copy of Tax Return (Classic) form?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
        return;
      int newIndex = this.copyFromClassic(recordIndex);
      if (flag)
      {
        this.addVerifToListView(newIndex).Selected = true;
      }
      else
      {
        this.refreshList();
        this.gridList.Items[newIndex - 1].Selected = true;
      }
    }

    private void copyToClassic(int recordIndex)
    {
      bool flag = false;
      int numberOfTaX4506Ts = this.loan.GetNumberOfTAX4506Ts(true);
      if (recordIndex > numberOfTaX4506Ts)
        flag = true;
      string str = "AR" + recordIndex.ToString("00");
      for (int index = 1; index <= 14; ++index)
        this.loan.SetField("IRS4506.X" + (object) index, flag ? "" : this.loan.GetField(str + index.ToString("00")));
      this.loan.SetField("IRS4506.X18", flag ? "" : this.loan.GetField(str + "18"));
      for (int index = 35; index <= 45; ++index)
        this.loan.SetField("IRS4506.X" + (object) index, flag ? "" : this.loan.GetField(str + index.ToString("00")));
      for (int index = 24; index <= 32; ++index)
        this.loan.SetField("IRS4506.X" + (object) index, flag ? "" : this.loan.GetField(str + index.ToString("00")));
      for (int index = 52; index <= 58; ++index)
        this.loan.SetField("IRS4506.X" + (object) index, flag ? "" : this.loan.GetField(str + index.ToString("00")));
      for (int index = 63; index <= 64; ++index)
        this.loan.SetField("IRS4506.X" + (object) index, flag ? "" : this.loan.GetField(str + (index + 2).ToString("00")));
    }

    private int copyFromClassic(int recordIndex)
    {
      bool flag = false;
      if (recordIndex == -1)
        recordIndex = this.loan.NewTAX4506T(true);
      int numberOfTaX4506Ts = this.loan.GetNumberOfTAX4506Ts(true);
      if (recordIndex > numberOfTaX4506Ts)
        flag = true;
      string str = "AR" + recordIndex.ToString("00");
      for (int index = 1; index <= 14; ++index)
        this.loan.SetField(str + index.ToString("00"), flag ? "" : this.loan.GetField("IRS4506.X" + (object) index));
      this.loan.SetField(str + "18", flag ? "" : this.loan.GetField("IRS4506.X18"));
      for (int index = 35; index <= 45; ++index)
        this.loan.SetField(str + index.ToString("00"), flag ? "" : this.loan.GetField("IRS4506.X" + (object) index));
      for (int index = 24; index <= 32; ++index)
        this.loan.SetField(str + index.ToString("00"), flag ? "" : this.loan.GetField("IRS4506.X" + (object) index));
      for (int index = 52; index <= 58; ++index)
        this.loan.SetField(str + index.ToString("00"), flag ? "" : this.loan.GetField("IRS4506.X" + (object) index));
      for (int index = 65; index <= 66; ++index)
        this.loan.SetField(str + index.ToString("00"), flag ? "" : this.loan.GetField("IRS4506.X" + (object) (index - 2)));
      return recordIndex;
    }

    private bool checkIRS4506Class()
    {
      string empty = string.Empty;
      for (int index = 1; index <= 14; ++index)
      {
        string field = this.loan.GetField("IRS4506.X" + (object) index);
        if (field != string.Empty && field != "//" && (index != 14 || !(field == "N")))
          return true;
      }
      string field1 = this.loan.GetField("IRS4506.X18");
      if (field1 != string.Empty && field1 != "//" && field1 != "N")
        return true;
      for (int index = 35; index <= 45; ++index)
      {
        string field2 = this.loan.GetField("IRS4506.X" + (object) index);
        if (field2 != string.Empty && field2 != "//")
          return true;
      }
      for (int index = 24; index <= 32; ++index)
      {
        if (index != 28)
        {
          string field3 = this.loan.GetField("IRS4506.X" + (object) index);
          if (field3 != string.Empty && field3 != "//")
            return true;
        }
      }
      for (int index = 52; index <= 58; ++index)
      {
        string field4 = this.loan.GetField("IRS4506.X" + (object) index);
        if (field4 != string.Empty && field4 != "//" && (index != 57 && index != 58 || !(field4 == "N")))
          return true;
      }
      return false;
    }
  }
}

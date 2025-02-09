// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.VOLPanel
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Verification
{
  public class VOLPanel : PanelBase
  {
    private static string sw = Tracing.SwInputEngine;
    private GVColumn creditorCol = new GVColumn();
    private GVColumn typeCol = new GVColumn();
    private GVColumn balanceCol = new GVColumn();
    private GVColumn paymentCol = new GVColumn();
    private GVColumn excludeCol = new GVColumn();
    private GVColumn monthsCol = new GVColumn();
    private GVColumn paidoffCol = new GVColumn();
    private bool loanIsNotNull;

    protected override void InitPanel()
    {
      this.className = nameof (VOLPanel);
      this.actionPanel.Controls.Remove((Control) this.btnOrderIRS);
      this.actionPanel.Controls.Remove((Control) this.btnCheckIRS);
      this.actionPanel.Controls.Remove((Control) this.btnImport4506);
      this.actionPanel.Controls.Remove((Control) this.btnExport4506);
      this.paintActionPanel();
      this.creditorCol.Text = "Creditor";
      this.creditorCol.Width = 140;
      this.typeCol.Text = "Type";
      this.typeCol.Width = 140;
      this.balanceCol.Text = "Balance";
      this.balanceCol.Width = 120;
      this.balanceCol.TextAlign = HorizontalAlignment.Right;
      this.paymentCol.Text = "Payment";
      this.paymentCol.Width = 100;
      this.paymentCol.TextAlign = HorizontalAlignment.Right;
      this.excludeCol.Text = "Exclude Mon. Pay";
      this.excludeCol.Width = 140;
      this.monthsCol.Text = "Months";
      this.monthsCol.Width = 100;
      this.monthsCol.TextAlign = HorizontalAlignment.Right;
      this.paidoffCol.Text = "To Be Paid Off";
      this.paidoffCol.Width = 140;
      this.gridList.Columns.AddRange(new GVColumn[7]
      {
        this.creditorCol,
        this.typeCol,
        this.balanceCol,
        this.monthsCol,
        this.paymentCol,
        this.excludeCol,
        this.paidoffCol
      });
    }

    public VOLPanel(string title, IMainScreen mainScreen, IWorkArea workArea, LoanData loanData)
      : base(title, mainScreen, workArea, loanData)
    {
      this.loanIsNotNull = true;
    }

    public VOLPanel(IMainScreen mainScreen, IWorkArea workArea, LoanData loanData)
      : base("VOL", mainScreen, workArea, loanData)
    {
      this.loanIsNotNull = true;
    }

    public VOLPanel(IMainScreen mainScreen, IWorkArea workArea)
      : base("VOL", mainScreen, workArea)
    {
      this.loanIsNotNull = false;
    }

    public VOLPanel(string title, IMainScreen mainScreen, IWorkArea workArea)
      : base(title, mainScreen, workArea)
    {
      this.loanIsNotNull = false;
    }

    protected override void hookupEventHandler(IInputHandler inputHandler)
    {
      if (inputHandler == null || !(inputHandler is VOLInputHandler))
        return;
      ((VOLInputHandler) inputHandler).VerifSummaryChanged += new VerifSummaryChangedEventHandler(this.summaryInfoChangedHandler);
    }

    private void summaryInfoChangedHandler(VerifSummaryChangeInfo info)
    {
      int index = this.gridList.SelectedItems[0].Index;
      string itemName = info.ItemName;
      int nItemIndex;
      string s;
      if (info.ItemName.Length > 6)
      {
        nItemIndex = Utils.ParseInt((object) info.ItemName.Substring(2, 3)) - 1;
        s = info.ItemName.Substring(0, 2) + "00" + info.ItemName.Substring(5);
      }
      else
      {
        nItemIndex = Utils.ParseInt((object) info.ItemName.Substring(2, 2)) - 1;
        s = info.ItemName.Substring(0, 2) + "00" + info.ItemName.Substring(4);
      }
      if (nItemIndex == -1)
        nItemIndex = this.gridList.SelectedItems[0].Index;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(s))
      {
        case 24588695:
          if (!(s == "FL0008"))
            break;
          this.gridList.Items[nItemIndex].SubItems[1].Text = this.convertLiabilityTypetoUIName((string) info.ItemValue);
          break;
        case 4050966992:
          if (!(s == "FL0012"))
            break;
          this.gridList.Items[nItemIndex].SubItems[3].Text = (string) info.ItemValue;
          break;
        case 4067744611:
          if (!(s == "FL0013"))
            break;
          this.gridList.Items[nItemIndex].SubItems[2].Text = (string) info.ItemValue;
          break;
        case 4101299849:
          if (!(s == "FL0011"))
            break;
          this.gridList.Items[nItemIndex].SubItems[4].Text = (string) info.ItemValue;
          break;
        case 4134855087:
          if (!(s == "FL0017"))
            break;
          string itemValue1 = (string) info.ItemValue;
          this.gridList.Items[nItemIndex].SubItems[5].Text = itemValue1;
          break;
        case 4218743182:
          if (!(s == "FL0018"))
            break;
          string itemValue2 = (string) info.ItemValue;
          this.gridList.Items[nItemIndex].SubItems[6].Text = itemValue2;
          break;
        case 4218890277:
          if (!(s == "FL0002"))
            break;
          this.gridList.Items[nItemIndex].SubItems[0].Text = (string) info.ItemValue;
          break;
      }
    }

    private string convertLiabilityTypetoUIName(string key)
    {
      switch (key)
      {
        case "ChildCare":
          return "Child Care";
        case "ChildSupport":
          return "Child Support";
        case "CollectionsJudgementsAndLiens":
          return "Collections Judgements And Liens";
        case "LeasePayments":
          return "Lease Payments";
        case "MortgageLoan":
          return "Mortgage";
        case "Open30DayChargeAccount":
          return "Open 30 Days Charge Account";
        case "OtherExpense":
          return "Other Expense";
        case "OtherLiability":
          return "Other Liability";
        case "SeparateMaintenanceExpense":
          return "Separate Maintenance Expense";
        case "TaxLien":
          return "Tax Lien";
        default:
          return key;
      }
    }

    public override void RefreshContents()
    {
      if (this.ver == null)
        return;
      InputHandlerBase inputHandler = (InputHandlerBase) this.ver.GetInputHandler();
      if (inputHandler == null)
        return;
      this.refreshList();
      inputHandler.RefreshContents();
    }

    protected override GVItem addVerifToListView(int newIndex)
    {
      string str1 = newIndex.ToString("00");
      GVItem listView = new GVItem(this.loan.GetField("FL" + str1 + "02"));
      listView.SubItems.Add((object) this.convertLiabilityTypetoUIName(this.loan.GetField("FL" + str1 + "08")));
      listView.SubItems.Add((object) this.loan.GetField("FL" + str1 + "13"));
      listView.SubItems.Add((object) this.loan.GetField("FL" + str1 + "12"));
      listView.SubItems.Add((object) this.loan.GetField("FL" + str1 + "11"));
      string field1 = this.loan.GetField("FL" + str1 + "17");
      string str2 = field1 == string.Empty ? "N" : field1;
      listView.SubItems.Add((object) str2);
      string field2 = this.loan.GetField("FL" + str1 + "18");
      string str3 = field2 == string.Empty ? "N" : field2;
      listView.SubItems.Add((object) str3);
      this.gridList.Items.Add(listView);
      return listView;
    }

    protected override void refreshList()
    {
      this.gridList.Items.Clear();
      if (Session.LoanData == null)
        return;
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      for (int newIndex = 1; newIndex <= exlcudingAlimonyJobExp; ++newIndex)
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
      return this.loanIsNotNull ? (VerificationBase) new VOL((PanelBase) this, this.mainScreen, this.workArea, this.loan) : (VerificationBase) new VOL((PanelBase) this, this.mainScreen, this.workArea);
    }

    protected override void editBtn_Click(object sender, EventArgs e)
    {
      if (this.gridList.SelectedItems.Count != 0)
        this.OpenVerification(this.gridList.SelectedItems[0].Index);
      this.gridList.Focus();
    }

    protected virtual int NewVerification() => this.loan.NewLiability();

    public override void newBtn_Click(object sender, EventArgs e)
    {
      int num = this.NewVerification();
      if (num <= -1)
        return;
      GVItem listView = this.addVerifToListView(num + 1);
      this.gridList.SelectedItems.Clear();
      listView.Selected = true;
      this.editBtn_Click((object) null, (EventArgs) null);
    }

    protected virtual int RemoveVerificationAt(int i) => this.loan.RemoveLiabilityAt(i);

    protected override void deleteBtn_Click(object sender, EventArgs e)
    {
      if (this.gridList.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select a record first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int index = this.gridList.SelectedItems[0].Index;
        if (this.DeleteDialog((IWin32Window) this) != DialogResult.Yes)
          return;
        InputHandlerBase inputHandler = (InputHandlerBase) this.ver.GetInputHandler();
        this.gridList.SelectedItems.Clear();
        if (this.RemoveVerificationAt(index) == -2)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "This verification can't be deleted due to a protected document tracking.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          this.gridList.Items.RemoveAt(index);
          if (this.gridList.Items.Count > 0)
            this.gridList.Items[0].Selected = true;
          else
            this.lowerContentPanel.Controls.Clear();
          inputHandler?.RefreshContents();
          this.editBtn_Click((object) null, (EventArgs) null);
        }
      }
    }

    protected virtual void UpVerification(int i) => this.loan.UpLiability(i);

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
        if (this.loan.GetField("1825") == "2020" && inputHandler.ToString().EndsWith("VOMInputHandler") && this.loan.GetField("FM0128") == "Y" && index == 1)
          this.gridList.Items[index].Selected = true;
        else
          this.gridList.Items[index - 1].Selected = true;
        inputHandler?.RefreshContents();
        this.editBtn_Click((object) null, (EventArgs) null);
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a record first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    protected virtual void DownVerfication(int i) => this.loan.DownLiability(i);

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
        if (this.loan.GetField("1825") == "2020" && inputHandler.ToString().EndsWith("VOMInputHandler") && this.loan.GetField("FM0128") == "Y" && index == 0)
          this.gridList.Items[index].Selected = true;
        else
          this.gridList.Items[index + 1].Selected = true;
        inputHandler?.RefreshContents();
        this.editBtn_Click((object) null, (EventArgs) null);
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a record first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    protected override void btnAddDoc_Click(object sender, EventArgs e)
    {
      if (this.gridList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "Please select a verification to add document tracking in eFolder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        ((InputHandlerBase) this.ver.GetInputHandler()).AddToEFolder();
    }

    protected override void btnOrderIRS_Click(object sender, EventArgs e)
    {
      if (this.gridList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "Please select a record to order.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        ((TAX4506TInputHandler) this.ver.GetInputHandler()).OrderTaxReturns(this.gridList.SelectedItems[0].Index + 1);
    }

    protected override void btnCheckIRS_Click(object sender, EventArgs e)
    {
      if (this.gridList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "Please select a record to check status.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        ((TAX4506TInputHandler) this.ver.GetInputHandler()).CheckTaxReturnStatus(this.gridList.SelectedItems[0].Index + 1);
    }

    protected override void btnExport4506_Click(object sender, EventArgs e)
    {
    }

    protected override void btnImport4506_Click(object sender, EventArgs e)
    {
    }
  }
}

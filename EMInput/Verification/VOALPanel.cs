// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.VOALPanel
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
  public class VOALPanel : PanelBase
  {
    private static string sw = Tracing.SwInputEngine;
    private GVColumn creditorCol = new GVColumn();
    private GVColumn accountTypeCol = new GVColumn();
    private GVColumn lienPositionCol = new GVColumn();
    private GVColumn monthlyPICol = new GVColumn();
    private GVColumn initialDrawCol = new GVColumn();
    private bool loanIsNotNull;

    protected override void InitPanel()
    {
      this.className = nameof (VOALPanel);
      this.actionPanel.Controls.Remove((Control) this.btnOrderIRS);
      this.actionPanel.Controls.Remove((Control) this.btnCheckIRS);
      this.actionPanel.Controls.Remove((Control) this.btnImport4506);
      this.actionPanel.Controls.Remove((Control) this.btnExport4506);
      this.actionPanel.Controls.Remove((Control) this.btnAddDoc);
      this.actionPanel.Controls.Remove((Control) this.verSeparator);
      this.paintActionPanel();
      this.creditorCol.Text = "Name";
      this.creditorCol.Width = 140;
      this.accountTypeCol.Text = "Account Type";
      this.accountTypeCol.Width = 140;
      this.lienPositionCol.Text = "Lien Position";
      this.lienPositionCol.Width = 120;
      this.lienPositionCol.TextAlign = HorizontalAlignment.Right;
      this.monthlyPICol.Text = "Monthly Principal And Intrest";
      this.monthlyPICol.Width = 100;
      this.monthlyPICol.TextAlign = HorizontalAlignment.Right;
      this.initialDrawCol.Text = "Loan Amount/HELOC Credit Limit";
      this.initialDrawCol.Width = 100;
      this.initialDrawCol.TextAlign = HorizontalAlignment.Right;
      this.gridList.Columns.AddRange(new GVColumn[5]
      {
        this.creditorCol,
        this.accountTypeCol,
        this.lienPositionCol,
        this.initialDrawCol,
        this.monthlyPICol
      });
    }

    public VOALPanel(string title, IMainScreen mainScreen, IWorkArea workArea, LoanData loanData)
      : base(title, mainScreen, workArea, loanData)
    {
      this.loanIsNotNull = true;
    }

    public VOALPanel(IMainScreen mainScreen, IWorkArea workArea, LoanData loanData)
      : base("VOAL", mainScreen, workArea, loanData)
    {
      this.loanIsNotNull = true;
    }

    public VOALPanel(IMainScreen mainScreen, IWorkArea workArea)
      : base("VOAL", mainScreen, workArea)
    {
      this.loanIsNotNull = false;
    }

    public VOALPanel(string title, IMainScreen mainScreen, IWorkArea workArea)
      : base(title, mainScreen, workArea)
    {
      this.loanIsNotNull = false;
    }

    protected override void hookupEventHandler(IInputHandler inputHandler)
    {
      if (inputHandler == null || !(inputHandler is VOALInputHandler))
        return;
      ((VOALInputHandler) inputHandler).VerifSummaryChanged += new VerifSummaryChangedEventHandler(this.summaryInfoChangedHandler);
    }

    private void summaryInfoChangedHandler(VerifSummaryChangeInfo info)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      switch (info.ItemName)
      {
        case "URLARAL0002":
          this.gridList.SelectedItems[0].SubItems[0].Text = (string) info.ItemValue;
          break;
        case "URLARAL0016":
          this.gridList.SelectedItems[0].SubItems[1].Text = (string) info.ItemValue;
          break;
        case "URLARAL0017":
          this.gridList.SelectedItems[0].SubItems[2].Text = (string) info.ItemValue;
          break;
        case "URLARAL0018":
          this.gridList.SelectedItems[0].SubItems[4].Text = (string) info.ItemValue;
          break;
        case "URLARAL0020":
          this.gridList.SelectedItems[0].SubItems[3].Text = (string) info.ItemValue;
          break;
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
      string str = newIndex.ToString("00");
      GVItem listView = new GVItem(this.loan.GetField("URLARAL" + str + "02"));
      listView.SubItems.Add((object) this.loan.GetField("URLARAL" + str + "16"));
      listView.SubItems.Add((object) this.loan.GetField("URLARAL" + str + "17"));
      listView.SubItems.Add((object) this.loan.GetField("URLARAL" + str + "20"));
      listView.SubItems.Add((object) this.loan.GetField("URLARAL" + str + "18"));
      this.gridList.Items.Add(listView);
      return listView;
    }

    protected override void refreshList()
    {
      this.gridList.Items.Clear();
      if (Session.LoanData == null)
        return;
      int ofAdditionalLoans = this.loan.GetNumberOfAdditionalLoans();
      for (int newIndex = 1; newIndex <= ofAdditionalLoans; ++newIndex)
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
      return this.loanIsNotNull ? (VerificationBase) new VOAL((PanelBase) this, this.mainScreen, this.workArea, this.loan) : (VerificationBase) new VOAL((PanelBase) this, this.mainScreen, this.workArea);
    }

    protected override void editBtn_Click(object sender, EventArgs e)
    {
      if (this.gridList.SelectedItems.Count != 0)
        this.OpenVerification(this.gridList.SelectedItems[0].Index);
      this.gridList.Focus();
    }

    protected virtual int NewVerification() => this.loan.NewAdditionalLoan();

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

    protected virtual int RemoveVerificationAt(int i) => this.loan.RemoveAdditionalLoanAt(i);

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

    protected virtual void UpVerification(int i) => this.loan.UpVerification("URLARAL", i);

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
        if (this.loan.GetField("1825") == "2020" && inputHandler.ToString().EndsWith("VOALInputHandler") && this.loan.GetField("FM0128") == "Y" && index == 1)
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

    protected virtual void DownVerfication(int i) => this.loan.DownVerification("URLARAL", i);

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
        if (this.loan.GetField("1825") == "2020" && inputHandler.ToString().EndsWith("VOALInputHandler") && this.loan.GetField("FM0128") == "Y" && index == 0)
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

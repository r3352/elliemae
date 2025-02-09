// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.VORPanel
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Verification
{
  public class VORPanel : PanelBase
  {
    protected GVColumn personCol = new GVColumn();
    protected GVColumn typeCol = new GVColumn();
    protected GVColumn addressCol = new GVColumn();
    private int firstCoBrw;
    private int firstBrwPrior;
    private int firstCoBrwPrior;
    private int total;
    private int totalBrwCurrent;
    private int totalCoBrwCurrent;
    private int totalBrwPrior;
    private int totalCoBrwPrior;
    private ArrayList brwList = new ArrayList();
    private ArrayList coBrwList = new ArrayList();
    private ArrayList tempList = new ArrayList();
    protected string dialogTitle;
    protected string dialogText;
    private bool loanIsNotNull;

    protected override void InitPanel()
    {
      this.className = nameof (VORPanel);
      this.actionPanel.Controls.Remove((Control) this.btnOrderIRS);
      this.actionPanel.Controls.Remove((Control) this.btnCheckIRS);
      this.actionPanel.Controls.Remove((Control) this.btnImport4506);
      this.actionPanel.Controls.Remove((Control) this.btnExport4506);
      this.paintActionPanel();
      this.personCol.Text = "Residence For";
      this.personCol.Width = 150;
      this.typeCol.Text = "Residence Type";
      this.typeCol.Width = 150;
      this.addressCol.Text = "Address";
      this.addressCol.Width = 250;
      this.gridList.Columns.AddRange(new GVColumn[3]
      {
        this.personCol,
        this.typeCol,
        this.addressCol
      });
      this.dialogTitle = "New Residence";
      this.dialogText = "Residence Type";
    }

    public VORPanel(IMainScreen mainScreen, IWorkArea workArea)
      : base("VOR", mainScreen, workArea)
    {
    }

    public VORPanel(string title, IMainScreen mainScreen, IWorkArea workArea, LoanData loanData)
      : base(title, mainScreen, workArea, loanData)
    {
      this.loanIsNotNull = true;
    }

    public VORPanel(IMainScreen mainScreen, IWorkArea workArea, LoanData loanData)
      : base("VOR", mainScreen, workArea, loanData)
    {
      this.loanIsNotNull = true;
    }

    public VORPanel(string title, IMainScreen mainScreen, IWorkArea workArea)
      : base(title, mainScreen, workArea)
    {
      this.loanIsNotNull = false;
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

    protected virtual int GetNumberOfVerification(bool borrower)
    {
      return this.loan.GetNumberOfResidence(borrower);
    }

    protected virtual bool IsVerificationCurrent(bool borrower, int recNo)
    {
      return borrower ? this.loan.GetField("BR" + recNo.ToString("00") + "23") == "Current" : this.loan.GetField("CR" + recNo.ToString("00") + "23") == "Current";
    }

    protected override void hookupEventHandler(IInputHandler inputHandler)
    {
      if (inputHandler == null || !(inputHandler is VORInputHandler))
        return;
      ((VORInputHandler) inputHandler).VerifSummaryChanged += new VerifSummaryChangedEventHandler(this.summaryInfoChangedHandler);
    }

    private void summaryInfoChangedHandler(VerifSummaryChangeInfo info)
    {
      string empty = string.Empty;
      switch (info.ItemName)
      {
        case "FR0004":
          this.gridList.SelectedItems[0].SubItems[2].Text = (string) info.ItemValue;
          break;
        case "FR0025":
        case "FR0026":
        case "FR0027":
          this.gridList.SelectedItems[0].SubItems[2].Text = this.loan.GetField(info.Indx + "04");
          break;
        case "FR0013":
          ((InputHandlerBase) this.ver.GetInputHandler())?.SaveCurrentField();
          string tag = (string) this.gridList.SelectedItems[0].Tag;
          this.refreshList();
          this.selectVOR(tag);
          break;
      }
    }

    private void selectVOR(string vorId)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridList.Items)
      {
        if ((string) gvItem.Tag == vorId)
          gvItem.Selected = true;
      }
    }

    internal void ReIndexing()
    {
      this.brwList.Clear();
      this.coBrwList.Clear();
      if (this.loan == null)
        return;
      int numberOfVerification1 = this.GetNumberOfVerification(true);
      this.total = numberOfVerification1;
      this.firstCoBrw = numberOfVerification1;
      this.firstBrwPrior = numberOfVerification1;
      this.tempList.Clear();
      for (int recNo = 1; recNo <= numberOfVerification1; ++recNo)
      {
        if (this.IsVerificationCurrent(true, recNo))
        {
          this.brwList.Add((object) (recNo - 1));
        }
        else
        {
          this.tempList.Add((object) (recNo - 1));
          if (this.firstBrwPrior == numberOfVerification1)
            this.firstBrwPrior = recNo - 1;
        }
      }
      this.totalBrwCurrent = this.brwList.Count;
      this.totalBrwPrior = this.tempList.Count;
      for (int index = 0; index < this.tempList.Count; ++index)
        this.brwList.Add(this.tempList[index]);
      int numberOfVerification2 = this.GetNumberOfVerification(false);
      this.total += numberOfVerification2;
      this.firstCoBrwPrior = this.total;
      this.tempList.Clear();
      for (int recNo = 1; recNo <= numberOfVerification2; ++recNo)
      {
        if (this.IsVerificationCurrent(false, recNo))
        {
          this.coBrwList.Add((object) (recNo - 1));
        }
        else
        {
          this.tempList.Add((object) (recNo - 1));
          if (this.firstCoBrwPrior == this.total)
            this.firstCoBrwPrior = recNo - 1 + this.firstCoBrw;
        }
      }
      this.totalCoBrwCurrent = this.coBrwList.Count;
      this.totalCoBrwPrior = this.tempList.Count;
      for (int index = 0; index < this.tempList.Count; ++index)
        this.coBrwList.Add(this.tempList[index]);
    }

    protected virtual GVItem NewRow(bool borrower, string sInd)
    {
      GVItem gvItem;
      if (borrower)
      {
        gvItem = new GVItem(this.loan.GetField("BR" + sInd + "13"));
        gvItem.SubItems.Add((object) this.loan.GetField("BR" + sInd + "23"));
        gvItem.SubItems.Add((object) this.loan.GetField("BR" + sInd + "04"));
        gvItem.Tag = (object) this.loan.GetField("BR" + sInd + "99");
      }
      else
      {
        gvItem = new GVItem(this.loan.GetField("CR" + sInd + "13"));
        gvItem.SubItems.Add((object) this.loan.GetField("CR" + sInd + "23"));
        gvItem.SubItems.Add((object) this.loan.GetField("CR" + sInd + "04"));
        gvItem.Tag = (object) this.loan.GetField("CR" + sInd + "99");
      }
      return gvItem;
    }

    protected override GVItem addVerifToListView(int newIndex) => (GVItem) null;

    protected override void refreshList()
    {
      this.gridList.Items.Clear();
      if (this.loan == null)
        return;
      this.ReIndexing();
      int num;
      for (int i = 0; i < this.brwList.Count; ++i)
      {
        num = this.GetIndexFromList(i) + 1;
        this.gridList.Items.Add(this.NewRow(true, num.ToString("00")));
      }
      for (int index = 0; index < this.coBrwList.Count; ++index)
      {
        num = this.GetIndexFromList(index + this.firstCoBrw) + 1;
        this.gridList.Items.Add(this.NewRow(false, num.ToString("00")));
      }
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
      return this.loanIsNotNull ? (VerificationBase) new VOR(this, this.mainScreen, this.workArea, this.loan) : (VerificationBase) new VOR(this, this.mainScreen, this.workArea);
    }

    protected override void editBtn_Click(object sender, EventArgs e)
    {
      if (this.gridList.SelectedItems.Count != 0)
        this.OpenVerification(this.gridList.SelectedItems[0].Index);
      this.gridList.Focus();
    }

    protected virtual int NewVerification(bool borrower, bool current)
    {
      return this.loan.NewResidence(borrower, current);
    }

    public override void newBtn_Click(object sender, EventArgs e)
    {
      NewResidenceDialog newResidenceDialog = new NewResidenceDialog(this.dialogTitle, this.dialogText);
      if (newResidenceDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      int num = this.NewVerification(newResidenceDialog.Borrower, newResidenceDialog.Current);
      this.refreshList();
      if (newResidenceDialog.Borrower)
      {
        if (newResidenceDialog.Current)
          this.gridList.Items[this.totalBrwCurrent - 1].Selected = true;
        else
          this.gridList.Items[this.firstCoBrw - 1].Selected = true;
        this.OpenVerification(this.brwList.IndexOf((object) num));
      }
      else
      {
        if (newResidenceDialog.Current)
          this.gridList.Items[this.totalBrwCurrent + this.totalBrwPrior + this.totalCoBrwCurrent - 1].Selected = true;
        else
          this.gridList.Items[this.gridList.Items.Count - 1].Selected = true;
        this.OpenVerification(this.firstCoBrw + this.coBrwList.IndexOf((object) num));
      }
    }

    internal int GetIndexFromList(int i)
    {
      return i < this.firstCoBrw ? int.Parse(this.brwList[i].ToString()) : int.Parse(this.coBrwList[i - this.firstCoBrw].ToString());
    }

    protected virtual void RemoveVerification(bool borrower, int index)
    {
      this.loan.RemoveResidenceAt(borrower, index);
    }

    protected override void deleteBtn_Click(object sender, EventArgs e)
    {
      if (this.gridList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a record first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int index = this.gridList.SelectedItems[0].Index;
        if (this.DeleteDialog((IWin32Window) this) != DialogResult.Yes)
          return;
        if (index < this.firstCoBrw)
          this.RemoveVerification(true, this.GetIndexFromList(index));
        else
          this.RemoveVerification(false, this.GetIndexFromList(index));
        this.ReIndexing();
        InputHandlerBase inputHandler = (InputHandlerBase) this.ver.GetInputHandler();
        this.gridList.Items.RemoveAt(index);
        if (this.gridList.Items.Count > 0)
        {
          if (index == this.gridList.Items.Count)
            this.gridList.Items[index - 1].Selected = true;
          else
            this.gridList.Items[index].Selected = true;
          inputHandler?.RefreshContents();
          this.editBtn_Click((object) null, (EventArgs) null);
        }
        else
          this.lowerContentPanel.Controls.Clear();
        this.loan.Calculator.UpdateCurrentMailingAddress();
      }
    }

    protected virtual void UpVerification(bool borrower, bool current, int index)
    {
      this.loan.UpResidence(borrower, current, index);
    }

    protected override void upBtn_Click(object sender, EventArgs e)
    {
      if (this.gridList.SelectedItems.Count != 0)
      {
        int index = this.gridList.SelectedItems[0].Index;
        bool flag = true;
        if (0 < index && index <= this.firstBrwPrior)
        {
          if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
            return;
          this.UpVerification(true, true, this.GetIndexFromList(index));
        }
        else if (this.firstBrwPrior < index && index < this.firstCoBrw)
        {
          if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
            return;
          this.UpVerification(true, false, this.GetIndexFromList(index));
        }
        else if (this.firstCoBrw < index && index < this.firstCoBrwPrior)
        {
          if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
            return;
          this.UpVerification(false, true, this.GetIndexFromList(index));
        }
        else if (this.firstCoBrwPrior < index)
        {
          if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
            return;
          this.UpVerification(false, false, this.GetIndexFromList(index));
        }
        else
          flag = false;
        if (!flag)
          return;
        InputHandlerBase inputHandler = (InputHandlerBase) this.ver.GetInputHandler();
        this.refreshList();
        this.gridList.Items[index - 1].Selected = true;
        inputHandler?.RefreshContents();
        this.editBtn_Click((object) null, (EventArgs) null);
        this.loan.Calculator.UpdateCurrentMailingAddress();
        this.loan.Calculator.FormCalculation("updategrossincome");
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a record first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    protected virtual void DownVerification(bool borrower, bool current, int index)
    {
      this.loan.DownResidence(borrower, current, index);
    }

    protected override void downBtn_Click(object sender, EventArgs e)
    {
      if (this.gridList.SelectedItems.Count != 0)
      {
        int index = this.gridList.SelectedItems[0].Index;
        bool flag = true;
        if (index <= this.firstBrwPrior - 1)
        {
          if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
            return;
          this.DownVerification(true, true, this.GetIndexFromList(index));
        }
        else if (this.firstBrwPrior <= index && index < this.firstCoBrw - 1)
        {
          if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
            return;
          this.DownVerification(true, false, this.GetIndexFromList(index));
        }
        else if (this.firstCoBrw <= index && index < this.firstCoBrwPrior - 1)
        {
          if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
            return;
          this.DownVerification(false, true, this.GetIndexFromList(index));
        }
        else if (this.firstCoBrwPrior <= index && index < this.total - 1)
        {
          if (!Session.LoanDataMgr.LockLoanWithExclusiveA())
            return;
          this.DownVerification(false, false, this.GetIndexFromList(index));
        }
        else
          flag = false;
        if (!flag)
          return;
        InputHandlerBase inputHandler = (InputHandlerBase) this.ver.GetInputHandler();
        this.refreshList();
        this.gridList.Items[index + 1].Selected = true;
        inputHandler?.RefreshContents();
        this.editBtn_Click((object) null, (EventArgs) null);
        this.loan.Calculator.UpdateCurrentMailingAddress();
        this.loan.Calculator.FormCalculation("updategrossincome");
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
    }

    protected override void btnCheckIRS_Click(object sender, EventArgs e)
    {
    }

    protected override void btnExport4506_Click(object sender, EventArgs e)
    {
    }

    protected override void btnImport4506_Click(object sender, EventArgs e)
    {
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.VOEPanel
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
  public class VOEPanel : VORPanel
  {
    private bool loanIsNotNull;
    private GVColumn sourceOfIncomeDataCol = new GVColumn();

    protected override void InitPanel()
    {
      this.actionPanel.Controls.Remove((Control) this.btnOrderIRS);
      this.actionPanel.Controls.Remove((Control) this.btnCheckIRS);
      this.actionPanel.Controls.Remove((Control) this.btnImport4506);
      this.actionPanel.Controls.Remove((Control) this.btnExport4506);
      this.paintActionPanel();
      this.personCol.Text = "Employment For";
      this.personCol.Width = 150;
      this.typeCol.Text = "Employment Status";
      this.typeCol.Width = 150;
      this.addressCol.Text = "Employer Name";
      this.addressCol.Width = 250;
      this.sourceOfIncomeDataCol.Text = "Source";
      this.sourceOfIncomeDataCol.Width = 300;
      this.gridList.Columns.AddRange(new GVColumn[4]
      {
        this.personCol,
        this.typeCol,
        this.addressCol,
        this.sourceOfIncomeDataCol
      });
      this.dialogTitle = "New Employment";
      this.dialogText = "Employment Status";
      this.Text = "VOE Panel";
    }

    public VOEPanel(IMainScreen mainScreen, IWorkArea workArea)
      : base("VOE", mainScreen, workArea)
    {
      this.className = nameof (VOEPanel);
    }

    public VOEPanel(IMainScreen mainScreen, IWorkArea workArea, LoanData loanData)
      : base("VOE", mainScreen, workArea, loanData)
    {
      this.loanIsNotNull = true;
    }

    protected override int GetNumberOfVerification(bool borrower)
    {
      return this.loan.GetNumberOfEmployer(borrower);
    }

    protected override bool IsVerificationCurrent(bool borrower, int recNo)
    {
      return borrower ? this.loan.GetField("BE" + recNo.ToString("00") + "09") == "Y" : this.loan.GetField("CE" + recNo.ToString("00") + "09") == "Y";
    }

    public override void RefreshListView(object sender, EventArgs e)
    {
      base.RefreshListView(sender, e);
    }

    protected override GVItem NewRow(bool borrower, string sInd)
    {
      GVItem gvItem;
      if (borrower)
      {
        gvItem = new GVItem(this.loan.GetField("BE" + sInd + "08"));
        string str = this.loan.GetField("BE" + sInd + "09") == "Y" ? "Current" : "Prior";
        gvItem.SubItems.Add((object) str);
        gvItem.SubItems.Add((object) this.loan.GetField("BE" + sInd + "02"));
        gvItem.SubItems.Add((object) this.loan.GetField("BE" + sInd + "81"));
        gvItem.Tag = (object) this.loan.GetField("BE" + sInd + "99");
      }
      else
      {
        gvItem = new GVItem(this.loan.GetField("CE" + sInd + "08"));
        string str = this.loan.GetField("CE" + sInd + "09") == "Y" ? "Current" : "Prior";
        gvItem.SubItems.Add((object) str);
        gvItem.SubItems.Add((object) this.loan.GetField("CE" + sInd + "02"));
        gvItem.SubItems.Add((object) this.loan.GetField("CE" + sInd + "81"));
        gvItem.Tag = (object) this.loan.GetField("CE" + sInd + "99");
      }
      return gvItem;
    }

    protected override VerificationBase NewVerificationScreen()
    {
      return this.loanIsNotNull ? (VerificationBase) new VOE((VORPanel) this, this.mainScreen, this.workArea, this.loan) : (VerificationBase) new VOE((VORPanel) this, this.mainScreen, this.workArea);
    }

    protected override int NewVerification(bool borrower, bool current)
    {
      return this.loan.NewEmployer(borrower, current);
    }

    protected override void RemoveVerification(bool borrower, int index)
    {
      this.loan.RemoveEmployerAt(borrower, index);
    }

    protected override void UpVerification(bool borrower, bool current, int index)
    {
      this.loan.UpEmployer(borrower, current, index);
    }

    protected override void DownVerification(bool borrower, bool current, int index)
    {
      this.loan.DownEmployer(borrower, current, index);
    }

    protected override void hookupEventHandler(IInputHandler inputHandler)
    {
      if (inputHandler == null || !(inputHandler is VOEInputHandler))
        return;
      ((VOEInputHandler) inputHandler).VerifSummaryChanged += new VerifSummaryChangedEventHandler(this.summaryInfoChangedHandler);
    }

    private void selectVOE(string voeId)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridList.Items)
      {
        if ((string) gvItem.Tag == voeId)
          gvItem.Selected = true;
      }
    }

    private void summaryInfoChangedHandler(VerifSummaryChangeInfo info)
    {
      switch (info.ItemName)
      {
        case "FE0002":
          this.gridList.SelectedItems[0].SubItems[2].Text = (string) info.ItemValue;
          break;
        case "FE0008":
        case "FE0009":
          ((InputHandlerBase) this.ver.GetInputHandler())?.SaveCurrentField();
          string tag = (string) this.gridList.SelectedItems[0].Tag;
          this.refreshList();
          this.selectVOE(tag);
          break;
      }
    }
  }
}

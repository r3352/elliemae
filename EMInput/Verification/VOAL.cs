// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.VOAL
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.InputEngine.Forms;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.Verification
{
  public class VOAL : VerificationBase
  {
    protected int ind;
    protected bool firstTime = true;

    public VOAL(PanelBase p, IMainScreen mainScreen, IWorkArea area, LoanData loanData)
      : base("AdditionalLoan", mainScreen, area, loanData)
    {
      this.selPanel = p;
      this.verType = "VOL";
    }

    public VOAL(PanelBase p, IMainScreen mainScreen, IWorkArea area)
      : base("AdditionalLoan", mainScreen, area)
    {
      this.selPanel = p;
      this.verType = nameof (VOAL);
    }

    public VOAL(string title, IMainScreen mainScreen, IWorkArea area)
      : base(title, mainScreen, area)
    {
    }

    public VOAL(string title, IMainScreen mainScreen, IWorkArea area, LoanData loanData)
      : base(title, mainScreen, area, loanData)
    {
    }

    public override void LoadData(int i)
    {
      this.ind = i;
      this.brwHandler.Property = (object) (i + 1);
      this.axWebBrowser.Navigate(FormStore.GetFormHTMLPath(Session.DefaultInstance, this.verType), ref LoanScreen.nobj, ref LoanScreen.nobj, ref LoanScreen.nobj, ref LoanScreen.nobj);
      this.axWebBrowser.BringToFront();
      this.firstTime = false;
      this.brwHandler.RefreshContents();
      this.brwHandler.RefreshToolTips();
    }

    protected virtual int GetNumberOfVerification() => this.loan.GetNumberOfAdditionalLoans();

    protected override void previousBtn_Click(object sender, EventArgs e)
    {
      int numberOfVerification = this.GetNumberOfVerification();
      this.LoadData((this.ind - 1 + numberOfVerification) % numberOfVerification);
    }

    protected override void nextBtn_Click(object sender, EventArgs e)
    {
      this.LoadData((this.ind + 1) % this.GetNumberOfVerification());
    }

    protected virtual int NewVerification() => this.loan.NewAdditionalLoan();

    protected override void newBtn_Click(object sender, EventArgs e)
    {
      int i = this.NewVerification();
      if (i <= -1)
        return;
      this.LoadData(i);
    }

    protected virtual void RemoveVerification(int i) => this.loan.RemoveAdditionalLoanAt(i);

    protected override void deleteBtn_Click(object sender, EventArgs e)
    {
      this.SendToBack();
      this.RemoveVerification(this.ind);
      this.brwHandler.IsDeleted = true;
      int numberOfVerification = this.GetNumberOfVerification();
      if (numberOfVerification > 0)
        this.selPanel.OpenVerification(this.ind % numberOfVerification);
      else
        this.selPanel.RefreshListView((object) null, (EventArgs) null);
    }

    public override int CurrentVerificationNo() => this.ind;
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.VOR
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
  public class VOR : VerificationBase
  {
    private int ind;
    protected VORPanel vorPanel;

    public VOR(VORPanel p, IMainScreen mainScreen, IWorkArea area, LoanData loanData)
      : base("Residence", mainScreen, area, loanData)
    {
      this.verType = nameof (VOR);
      this.selPanel = (PanelBase) p;
      this.vorPanel = p;
    }

    public VOR(VORPanel p, IMainScreen mainScreen, IWorkArea area)
      : base("Residence", mainScreen, area)
    {
      this.verType = nameof (VOR);
      this.selPanel = (PanelBase) p;
      this.vorPanel = p;
    }

    public VOR(string title, IMainScreen mainScreen, IWorkArea area)
      : base(title, mainScreen, area)
    {
    }

    public VOR(string title, IMainScreen mainScreen, IWorkArea area, LoanData loanData)
      : base(title, mainScreen, area, loanData)
    {
    }

    public override void LoadData(int i)
    {
      bool flag = i < this.GetNumberOfVerification(true);
      this.ind = i;
      this.brwHandler.Property = (object) new object[2]
      {
        (object) flag,
        (object) (this.vorPanel.GetIndexFromList(i) + 1)
      };
      this.axWebBrowser.Navigate(FormStore.GetFormHTMLPath(Session.DefaultInstance, this.verType), ref LoanScreen.nobj, ref LoanScreen.nobj, ref LoanScreen.nobj, ref LoanScreen.nobj);
      this.axWebBrowser.BringToFront();
      this.brwHandler.RefreshContents();
      this.brwHandler.RefreshToolTips();
    }

    protected virtual int GetNumberOfVerification(bool borrower)
    {
      return this.loan.GetNumberOfResidence(borrower);
    }

    protected override void previousBtn_Click(object sender, EventArgs e)
    {
      int i = this.ind - 1;
      if (i < 0)
        return;
      this.vorPanel.ReIndexing();
      this.LoadData(i);
    }

    protected override void nextBtn_Click(object sender, EventArgs e)
    {
      int num = this.GetNumberOfVerification(true) + this.GetNumberOfVerification(false);
      int i = this.ind + 1;
      if (i >= num)
        return;
      this.vorPanel.ReIndexing();
      this.LoadData(i);
    }

    protected override void newBtn_Click(object sender, EventArgs e)
    {
      this.vorPanel.newBtn_Click((object) null, (EventArgs) null);
    }

    protected virtual void RemoveVerification(bool borrower, int index)
    {
      this.loan.RemoveResidenceAt(borrower, index);
    }

    private void OpenVerification(int index) => this.vorPanel.OpenVerification(index);

    protected override void deleteBtn_Click(object sender, EventArgs e)
    {
      object[] property = (object[]) this.brwHandler.Property;
      this.RemoveVerification((bool) property[0], int.Parse(property[1].ToString()) - 1);
      this.brwHandler.IsDeleted = true;
      int num = this.GetNumberOfVerification(true) + this.GetNumberOfVerification(false);
      if (num == 0)
      {
        this.workArea.ShowVerifPanel(this.verType);
      }
      else
      {
        this.vorPanel.ReIndexing();
        if (this.ind < num)
          this.LoadData(this.ind);
        else
          this.LoadData(0);
      }
    }

    public override int CurrentVerificationNo() => this.ind;
  }
}

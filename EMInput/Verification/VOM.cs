// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.VOM
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Verification
{
  public class VOM : VOL
  {
    public VOM(PanelBase p, IMainScreen mainScreen, IWorkArea area, LoanData loanData)
      : base("Mortgage", mainScreen, area, loanData)
    {
      this.selPanel = p;
      this.verType = nameof (VOM);
    }

    public VOM(PanelBase p, IMainScreen mainScreen, IWorkArea area)
      : base("Mortgage", mainScreen, area)
    {
      this.selPanel = p;
      this.verType = nameof (VOM);
    }

    protected override int GetNumberOfVerification() => this.loan.GetNumberOfMortgages();

    protected override int NewVerification()
    {
      NewMortgageDialog newMortgageDialog = new NewMortgageDialog(this.loan, string.Empty);
      return newMortgageDialog.ShowDialog((IWin32Window) this) == DialogResult.OK ? this.loan.NewMortgage(newMortgageDialog.SelectedVOL) : -1;
    }

    protected override void RemoveVerification(int i) => this.loan.RemoveMortgageAt(i);
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.VOE
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.Verification
{
  public class VOE : VOR
  {
    public VOE(VORPanel p, IMainScreen mainScreen, IWorkArea area, LoanData loanData)
      : base("Employment", mainScreen, area, loanData)
    {
      this.verType = nameof (VOE);
      this.vorPanel = p;
      this.selPanel = (PanelBase) p;
    }

    public VOE(VORPanel p, IMainScreen mainScreen, IWorkArea area)
      : base("Employment", mainScreen, area)
    {
      this.verType = nameof (VOE);
      this.vorPanel = p;
      this.selPanel = (PanelBase) p;
    }

    protected override int GetNumberOfVerification(bool borrower)
    {
      return this.loan.GetNumberOfEmployer(borrower);
    }

    protected override void RemoveVerification(bool borrower, int index)
    {
      this.loan.RemoveEmployerAt(borrower, index);
    }
  }
}

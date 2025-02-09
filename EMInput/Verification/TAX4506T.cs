// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.TAX4506T
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.Verification
{
  public class TAX4506T : VOL
  {
    public TAX4506T(PanelBase p, IMainScreen mainScreen, IWorkArea area)
      : base("TAX4506-T", mainScreen, area)
    {
      this.selPanel = p;
      this.verType = nameof (TAX4506T);
    }

    protected override int GetNumberOfVerification() => this.loan.GetNumberOfTAX4506Ts(false);

    protected override int NewVerification() => this.loan.NewTAX4506T(false);

    protected override void RemoveVerification(int i) => this.loan.RemoveTAX4506TAt(i, false);
  }
}

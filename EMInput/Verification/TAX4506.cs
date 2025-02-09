// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.TAX4506
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.Verification
{
  public class TAX4506 : VOL
  {
    public TAX4506(PanelBase p, IMainScreen mainScreen, IWorkArea area)
      : base(nameof (TAX4506), mainScreen, area)
    {
      this.selPanel = p;
      this.verType = nameof (TAX4506);
    }

    protected override int GetNumberOfVerification() => this.loan.GetNumberOfTAX4506Ts(true);

    protected override int NewVerification() => this.loan.NewTAX4506T(true);

    protected override void RemoveVerification(int i) => this.loan.RemoveTAX4506TAt(i, true);
  }
}

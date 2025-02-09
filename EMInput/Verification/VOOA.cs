// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Verification.VOOA
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.Verification
{
  public class VOOA : VOL
  {
    public VOOA(PanelBase p, IMainScreen mainScreen, IWorkArea area)
      : base("OtherAsset", mainScreen, area)
    {
      this.selPanel = p;
      this.verType = nameof (VOOA);
    }

    protected override int GetNumberOfVerification() => this.loan.GetNumberOfOtherAssets();

    protected override int NewVerification() => this.loan.NewOtherAsset();

    protected override void RemoveVerification(int i) => this.loan.RemoveDepositAt(i);
  }
}

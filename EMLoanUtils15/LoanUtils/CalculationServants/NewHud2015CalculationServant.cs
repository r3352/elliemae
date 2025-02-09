// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.NewHud2015CalculationServant
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

#nullable disable
namespace EllieMae.EMLite.LoanUtils.CalculationServants
{
  public class NewHud2015CalculationServant : CalculationServantBase
  {
    private readonly string _rateLockExpirationTimeZoneSetting;

    public NewHud2015CalculationServant(
      ILoanModelProvider modelProvider,
      ISettingsProvider systemSettings)
      : base(modelProvider)
    {
      this._rateLockExpirationTimeZoneSetting = (string) systemSettings.GetSystemPolicy("Policies.RateLockExpirationTimeZone");
    }
  }
}

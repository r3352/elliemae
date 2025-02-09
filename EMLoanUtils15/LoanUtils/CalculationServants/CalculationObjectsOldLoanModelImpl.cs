// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.CalculationObjectsOldLoanModelImpl
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

#nullable disable
namespace EllieMae.EMLite.LoanUtils.CalculationServants
{
  internal class CalculationObjectsOldLoanModelImpl : ICalculationObjects
  {
    public string CurrentFormId => string.Empty;

    public bool StopSyncItemization => false;

    public bool ManualSyncItemization => false;

    public bool SkipLockRequestSync => true;

    public void NewHudCalCopyHud2010ToGfe2010(
      string sourceId,
      string currentId,
      bool blankCopyOnly)
    {
    }

    public void NewHudCalFormCal(string id, string val)
    {
    }

    public void GfeCalFormCal(string formid, string id, string val)
    {
    }

    public void GfeCalCalcGfeFees(string id, string val)
    {
    }

    public void NewHudCalCalculateHudgfe(string id, string val)
    {
    }

    public void MldsCalCopyGfetoMlds(string id)
    {
    }

    public void GfeCalCalcPrepaid(string id, string val)
    {
    }

    public void Calc_2015TitleFees(string id, string val)
    {
    }

    public void Calc_2015CityStateTaxFees(string id, string val)
    {
    }
  }
}

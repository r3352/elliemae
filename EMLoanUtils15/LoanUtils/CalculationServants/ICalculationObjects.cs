// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.ICalculationObjects
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

#nullable disable
namespace EllieMae.EMLite.LoanUtils.CalculationServants
{
  public interface ICalculationObjects
  {
    string CurrentFormId { get; }

    bool SkipLockRequestSync { get; }

    void NewHudCalCopyHud2010ToGfe2010(string sourceId, string currentId, bool blankCopyOnly);

    void NewHudCalFormCal(string id, string val);

    void GfeCalFormCal(string formid, string id, string val);

    void GfeCalCalcGfeFees(string id, string val);

    void NewHudCalCalculateHudgfe(string id, string val);

    void MldsCalCopyGfetoMlds(string id);

    void GfeCalCalcPrepaid(string id, string val);

    void Calc_2015TitleFees(string id, string val);

    void Calc_2015CityStateTaxFees(string id, string val);
  }
}

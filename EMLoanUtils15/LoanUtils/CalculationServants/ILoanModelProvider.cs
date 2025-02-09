// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.ILoanModelProvider
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System.Collections;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.CalculationServants
{
  public interface ILoanModelProvider
  {
    bool UseNewGFEHUD { get; }

    bool UseNew2015GFEHUD { get; }

    Hashtable BorHudFields { get; }

    Hashtable SelHudFields { get; }

    int GetBorrowerPairsCount();

    bool IsPrimaryPair();

    bool IsLocked(string id);

    void AddLock(string id);

    void RemoveLock(string id);

    string Val(string fieldId);

    string Val(string fieldId, int borrowerPairIndex);

    void SetVal(string fieldId, string fieldValue);

    void SetVal(string fieldId, string fieldValue, int borrowerPairIndex);

    double Flt(string val);

    double FltVal(string val);

    ICalculationObjects CalculationObjects { get; }

    void TriggerCalculation(string fieldid, string fieldValue);

    void FormCal(string id, string val);

    void SetCurrentNum(string id, double num);

    void SetCurrentNum(string id, double num, bool showZero);

    void SetCurrentNumOnly(string id, double num, bool showZero = false);

    void SetValOnly(string fieldId, string fieldValue);

    void SetValInteractive(string id, string val);

    string HMDAApplicationDate { get; }
  }
}

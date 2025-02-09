// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.LoanDisclosedChecker
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.CalculationServants
{
  public class LoanDisclosedChecker
  {
    private readonly ILoanModelProvider _modelProvider;

    public LoanDisclosedChecker(ILoanModelProvider modelProvider)
    {
      this._modelProvider = modelProvider;
    }

    public bool IsLoanDisclosed(bool manualSyncItemization)
    {
      string str = this._modelProvider.Val("3168");
      DateTime date1 = Utils.ParseDate((object) this._modelProvider.Val("3137"));
      if (!(date1 == DateTime.MinValue) && !(str == "Y"))
      {
        DateTime dateTime;
        if (this._modelProvider.Val("3164") != "Y" && Utils.ParseDate((object) this._modelProvider.Val("3140")) != DateTime.MinValue)
        {
          dateTime = Utils.ParseDate((object) this._modelProvider.Val("3140"));
          DateTime date2 = dateTime.Date;
          dateTime = DateTime.Today;
          DateTime date3 = dateTime.Date;
          if (date2 < date3)
            goto label_4;
        }
        dateTime = Utils.ParseDate((object) this._modelProvider.Val("761"));
        DateTime date4 = dateTime.Date;
        dateTime = Utils.ParseDate((object) this._modelProvider.Val("3137"));
        DateTime date5 = dateTime.Date;
        if (!(date4 > date5))
          return true;
      }
label_4:
      return ((!(date1 != DateTime.MinValue) ? 0 : (str == "Y" ? 1 : 0)) & (manualSyncItemization ? 1 : 0)) != 0;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.MLDSCalculationServant
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

#nullable disable
namespace EllieMae.EMLite.LoanUtils.CalculationServants
{
  public class MLDSCalculationServant(ILoanModelProvider modelProvider) : CalculationServantBase(modelProvider)
  {
    public void SyncPrepaymentPenality(string id, string val)
    {
      if (id == "675")
      {
        this.SetVal("RE88395.X322", val);
        if (!(val == "Y"))
          return;
        this.SetVal("RE88395.X123", "");
        this.SetVal("RE88395.X124", "");
        this.SetVal("RE88395.X317", "");
      }
      else
      {
        if (!(this.Val("LOANTERMTABLE.CUSTOMIZE") != "Y") && !(this.Val("3969") != "RESPA-TILA 2015 LE and CD"))
          return;
        if ((id == "RE88395.X123" || id == "RE88395.X124") && val == "Y")
        {
          this.SetVal("675", "N");
        }
        else
        {
          if (!(id == "RE88395.X322"))
            return;
          if (this.Val("RE88395.X123") == "Y")
            this.SetVal("675", "N");
          else if (this.Val("RE88395.X322") == "Y")
            this.SetVal("675", "Y");
          else
            this.SetVal("675", "");
        }
      }
    }

    public void PrepaymentPenaltyIndicator()
    {
      string str = this.Val("2830");
      if (string.IsNullOrEmpty(str) || !(str != "Amount Prepaid") || !this.ModelProvider.IsLocked("RE88395.X315") || !(this.Val("RE88395.X322") != "Y"))
        return;
      this.SetVal("RE88395.X322", "Y");
      if (!(this.Val("LOANTERMTABLE.CUSTOMIZE") != "Y") && !(this.Val("3969") != "RESPA-TILA 2015 LE and CD"))
        return;
      this.SetVal("675", "Y");
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.D1003CalculationServant
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.CalculationServants
{
  public class D1003CalculationServant(ILoanModelProvider modelProvider) : CalculationServantBase(modelProvider)
  {
    private const string ClassName = "D1003CalculationServant�";

    public void AdjustCitizenshipAndAge(string id, string val)
    {
      if (Tracing.IsSwitchActive(CalculationServantBase.TraceSwitch, TraceLevel.Info))
        Tracing.Log(CalculationServantBase.TraceSwitch, TraceLevel.Info, nameof (D1003CalculationServant), "routine: AdjustCitizenshipAndAge ID: " + id);
      if (!(id == "1402") && !(id == "1403"))
        return;
      string str = this.ModelProvider == null || !((this.ModelProvider.HMDAApplicationDate ?? "") != "") ? this.Val("745") : this.Val(this.ModelProvider.HMDAApplicationDate);
      DateTime dateTime;
      DateTime date;
      try
      {
        dateTime = string.IsNullOrEmpty(str) ? DateTime.Now : Utils.ParseDate((object) str);
        if ((id ?? "") == "")
          return;
        date = Utils.ParseDate((object) this.Val(id)).Date;
        if (date == DateTime.MinValue)
          return;
      }
      catch (Exception ex)
      {
        return;
      }
      int num = dateTime.Year - date.Year;
      if (date.Month > dateTime.Month)
        --num;
      else if (date.Month == dateTime.Month && dateTime.Day < date.Day)
        --num;
      this.SetVal(id == "1402" ? "38" : "70", num.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    public void CopyBorrowersToLockRequest()
    {
      this.CopyBorrowersToLockRequest(string.Empty, string.Empty);
    }

    public void CopyBorrowersToLockRequest(string id, string value)
    {
      int borrowerPairsCount = this.ModelProvider.GetBorrowerPairsCount();
      switch (id)
      {
        case "TPO":
          for (int borrowerPairIndex = 0; borrowerPairIndex < borrowerPairsCount; ++borrowerPairIndex)
          {
            this.SetVal("36", (this.Val("4000", borrowerPairIndex) + " " + this.Val("4001", borrowerPairIndex)).Trim(), borrowerPairIndex);
            this.SetVal("37", (this.Val("4002", borrowerPairIndex) + " " + this.Val("4003", borrowerPairIndex)).Trim(), borrowerPairIndex);
            this.SetVal("68", (this.Val("4004", borrowerPairIndex) + " " + this.Val("4005", borrowerPairIndex)).Trim(), borrowerPairIndex);
            this.SetVal("69", (this.Val("4006", borrowerPairIndex) + " " + this.Val("4007", borrowerPairIndex)).Trim(), borrowerPairIndex);
          }
          return;
        case "4000":
        case "4001":
          this.SetVal("36", (this.Val("4000") + " " + this.Val("4001")).Trim());
          break;
        case "4002":
        case "4003":
          this.SetVal("37", (this.Val("4002") + " " + this.Val("4003")).Trim());
          break;
        case "4004":
        case "4005":
          this.SetVal("68", (this.Val("4004") + " " + this.Val("4005")).Trim());
          break;
        case "4006":
        case "4007":
          this.SetVal("69", (this.Val("4006") + " " + this.Val("4007")).Trim());
          break;
        case "36":
          this.SplitName("4000", "4001", this.Val("36"));
          break;
        case "37":
          this.SplitName("4002", "4003", this.Val("37"));
          break;
        case "68":
          this.SplitName("4004", "4005", this.Val("68"));
          break;
        case "69":
          this.SplitName("4006", "4007", this.Val("69"));
          break;
      }
      for (int index = 2; index <= 73; ++index)
        this.SetVal(LockRequestLog.RequestFields[index], "");
      for (int borrowerPairIndex = 0; borrowerPairIndex < borrowerPairsCount; ++borrowerPairIndex)
      {
        int num = 2868 + borrowerPairIndex * 12;
        this.SetVal(num.ToString(), this.Val("4000", borrowerPairIndex));
        ++num;
        this.SetVal(num.ToString(), this.Val("37", borrowerPairIndex));
        ++num;
        this.SetVal(num.ToString(), this.Val("65", borrowerPairIndex));
        ++num;
        this.SetVal(num.ToString(), this.Val("67", borrowerPairIndex));
        ++num;
        this.SetVal(num.ToString(), this.Val("1450", borrowerPairIndex));
        ++num;
        this.SetVal(num.ToString(), this.Val("1414", borrowerPairIndex));
        ++num;
        this.SetVal(num.ToString(), this.Val("4004", borrowerPairIndex));
        ++num;
        this.SetVal(num.ToString(), this.Val("69", borrowerPairIndex));
        ++num;
        this.SetVal(num.ToString(), this.Val("97", borrowerPairIndex));
        ++num;
        this.SetVal(num.ToString(), this.Val("60", borrowerPairIndex));
        ++num;
        this.SetVal(num.ToString(), this.Val("1452", borrowerPairIndex));
        ++num;
        this.SetVal(num.ToString(), this.Val("1415", borrowerPairIndex));
        this.SetVal((3516 + borrowerPairIndex * 2).ToString(), this.Val("FE0115", borrowerPairIndex));
        this.SetVal((3517 + borrowerPairIndex * 2).ToString(), this.Val("FE0215", borrowerPairIndex));
      }
    }

    public void AdjustConstructionTotal(string id, string val)
    {
      if (Tracing.IsSwitchActive(CalculationServantBase.TraceSwitch, TraceLevel.Info))
        Tracing.Log(CalculationServantBase.TraceSwitch, TraceLevel.Info, nameof (D1003CalculationServant), "routine: adjustConstructionTotal ID: " + id);
      if (!(id == "1172"))
        return;
      switch (val)
      {
        case "FHA":
          this.SetVal("1711", "HUD / FHA");
          break;
        case "VA":
          this.SetVal("1711", "V.A.");
          break;
        default:
          this.SetVal("1711", "");
          break;
      }
    }

    public void CalculateLiquidAssets(string id)
    {
      if (id == "183")
      {
        string str = this.Val("202");
        if (str == string.Empty || str == "CashDepositOnSalesContract")
        {
          double num = this.FltVal("183");
          if (this.FltVal("141") == 0.0 && num != 0.0)
          {
            this.SetCurrentNum("141", num);
            this.SetVal("202", "CashDepositOnSalesContract");
          }
        }
      }
      if (!(id == "1716"))
        return;
      string str1 = this.Val("1091");
      if (!(str1 == string.Empty) && !(str1 == "CashDepositOnSalesContract"))
        return;
      double num1 = this.FltVal("1716");
      if (this.FltVal("1095") != 0.0 || num1 == 0.0)
        return;
      this.SetCurrentNum("1095", num1);
      this.SetVal("1091", "CashDepositOnSalesContract");
    }

    public static string GenerateCheckDigit(string LEI, string loanNumber, bool isLocked)
    {
      if (!string.IsNullOrEmpty(loanNumber) && !isLocked)
      {
        LEI += loanNumber;
        StringBuilder stringBuilder1 = new StringBuilder();
        try
        {
          foreach (char ch in LEI.ToLowerInvariant())
          {
            switch (ch)
            {
              case 'a':
                stringBuilder1.Append("10");
                break;
              case 'b':
                stringBuilder1.Append("11");
                break;
              case 'c':
                stringBuilder1.Append("12");
                break;
              case 'd':
                stringBuilder1.Append("13");
                break;
              case 'e':
                stringBuilder1.Append("14");
                break;
              case 'f':
                stringBuilder1.Append("15");
                break;
              case 'g':
                stringBuilder1.Append("16");
                break;
              case 'h':
                stringBuilder1.Append("17");
                break;
              case 'i':
                stringBuilder1.Append("18");
                break;
              case 'j':
                stringBuilder1.Append("19");
                break;
              case 'k':
                stringBuilder1.Append("20");
                break;
              case 'l':
                stringBuilder1.Append("21");
                break;
              case 'm':
                stringBuilder1.Append("22");
                break;
              case 'n':
                stringBuilder1.Append("23");
                break;
              case 'o':
                stringBuilder1.Append("24");
                break;
              case 'p':
                stringBuilder1.Append("25");
                break;
              case 'q':
                stringBuilder1.Append("26");
                break;
              case 'r':
                stringBuilder1.Append("27");
                break;
              case 's':
                stringBuilder1.Append("28");
                break;
              case 't':
                stringBuilder1.Append("29");
                break;
              case 'u':
                stringBuilder1.Append("30");
                break;
              case 'v':
                stringBuilder1.Append("31");
                break;
              case 'w':
                stringBuilder1.Append("32");
                break;
              case 'x':
                stringBuilder1.Append("33");
                break;
              case 'y':
                stringBuilder1.Append("34");
                break;
              case 'z':
                stringBuilder1.Append("35");
                break;
              default:
                stringBuilder1.Append(ch);
                break;
            }
          }
          stringBuilder1.Append("00");
          BigInteger bigInteger = (BigInteger) 98 - BigInteger.Parse(stringBuilder1.ToString()) % (BigInteger) 97;
          StringBuilder stringBuilder2 = new StringBuilder();
          stringBuilder2.Append(LEI).Append(bigInteger.ToString("00"));
          return stringBuilder2.ToString();
        }
        catch
        {
        }
      }
      return string.Empty;
    }
  }
}

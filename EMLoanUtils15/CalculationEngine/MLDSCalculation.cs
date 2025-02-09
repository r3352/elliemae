// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.MLDSCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.CalculationServants;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class MLDSCalculation : CalculationBase
  {
    private const string className = "MLDSCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    internal Routine CalcCompensations;
    internal Routine UpdateInsurance;
    internal Routine CalcMLDSScenarios;
    internal Routine CalcRE882;
    private readonly MLDSCalculationServant mLDSCalculationServant;

    internal MLDSCalculation(LoanData l, EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.CalcCompensations = this.RoutineX(new Routine(this.calculateCompensations));
      this.UpdateInsurance = this.RoutineX(new Routine(this.updateImpoundInsurance));
      this.CalcMLDSScenarios = this.RoutineX(new Routine(this.calculateMLDSScenarios));
      this.CalcRE882 = this.RoutineX(new Routine(this.calculateRE882));
      this.addFieldHandlers();
      this.mLDSCalculationServant = new MLDSCalculationServant((ILoanModelProvider) this);
    }

    private void addFieldHandlers()
    {
      Routine routine = this.RoutineX(new Routine(this.calculateRE882));
      for (int index = 4; index <= 67; ++index)
        this.AddFieldHandler("RE882.X" + index.ToString(), routine);
    }

    public override void FormCal(string id, string val)
    {
      if (Tracing.IsSwitchActive(MLDSCalculation.sw, TraceLevel.Info))
        Tracing.Log(MLDSCalculation.sw, TraceLevel.Info, nameof (MLDSCalculation), "routine: FormCal");
      if (id != null && this.UseNew2015GFEHUD)
      {
        if (HUDGFE2010Fields.PAIDBYPOPTFIELDS.ContainsKey((object) id))
        {
          if (id == "NEWHUD.X749")
          {
            this.SetVal("NEWHUD.X1175", val);
            this.calObjs.NewHud2015FeeDetailCal.CalcFeeDetail("NEWHUD.X1175", (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) "NEWHUD.X1175"]);
            this.SetVal("NEWHUD.X1179", val);
            this.calObjs.NewHud2015FeeDetailCal.CalcFeeDetail("NEWHUD.X1179", (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) "NEWHUD.X1179"]);
            this.SetVal("NEWHUD.X1183", val);
            this.calObjs.NewHud2015FeeDetailCal.CalcFeeDetail("NEWHUD.X1183", (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) "NEWHUD.X1183"]);
            this.SetVal("NEWHUD.X1187", val);
            this.calObjs.NewHud2015FeeDetailCal.CalcFeeDetail("NEWHUD.X1187", (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) "NEWHUD.X1187"]);
          }
          else
            this.calObjs.NewHud2015FeeDetailCal.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) id]);
        }
        else
        {
          switch (id)
          {
            case "NEWHUD.X651":
              this.calObjs.NewHud2015FeeDetailCal.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) "SYS.X307"]);
              break;
            case "NEWHUD.X599":
              this.calObjs.NewHud2015FeeDetailCal.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) "SYS.X313"]);
              break;
            case "NEWHUD.X646":
              this.calObjs.NewHud2015FeeDetailCal.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) "NEWHUD.X743"]);
              break;
            case "NEWHUD.X573":
              this.calObjs.NewHud2015FeeDetailCal.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) "NEWHUD.X745"]);
              break;
            case "NEWHUD.X576":
              this.calObjs.NewHud2015FeeDetailCal.CalcFeeDetail(id, (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) "NEWHUD.X221"]);
              break;
          }
        }
      }
      this.calcFees();
      double insurance1 = this.calObjs.GFECal.CalculateInsurance("RE88395.X43");
      if (insurance1 != 0.0 && this.Val("1172") != "VA")
        this.SetCurrentNum("RE88395.X43", insurance1 - this.FltVal("RE88395.X42"));
      if (id == null)
      {
        double insurance2 = this.calObjs.GFECal.CalculateInsurance("230");
        if (insurance2 != 0.0)
          this.SetVal("230", insurance2.ToString("N2"));
        this.calObjs.Cal.CalcMortgageInsurance(id, val);
        double insurance3 = this.calObjs.GFECal.CalculateInsurance("231");
        if (insurance3 != 0.0)
          this.SetCurrentNum("231", insurance3);
        this.calcImpound("230", (string) null);
        this.calcImpound("232", (string) null);
        this.calcImpound("L267", (string) null);
        this.calcImpound("1386", (string) null);
        this.calcImpound("1388", (string) null);
        this.calcImpound("1629", (string) null);
        this.calcImpound("340", (string) null);
        this.calcImpound("341", (string) null);
      }
      else
        this.calcImpound(id, val);
      if (id != null)
      {
        this.syncGFE(id, val);
        if (id == "230")
          this.syncGFE("L251", this.Val("L251"));
      }
      this.calculateCompensations(id, val);
    }

    private void updateImpoundInsurance(string id, string val)
    {
      double insurance1 = this.calObjs.GFECal.CalculateInsurance("230");
      if (insurance1 != 0.0)
        this.SetVal("230", insurance1.ToString("N2"));
      this.calObjs.Cal.CalcMortgageInsurance(id, val);
      double insurance2 = this.calObjs.GFECal.CalculateInsurance("231");
      if (insurance2 != 0.0)
        this.SetCurrentNum("231", insurance2);
      double insurance3 = this.calObjs.GFECal.CalculateInsurance("RE88395.X43");
      if (insurance3 != 0.0)
        this.SetCurrentNum("RE88395.X43", insurance3 - this.FltVal("RE88395.X42"));
      this.calcImpound("230", (string) null);
      this.calcImpound("232", (string) null);
      this.calcImpound("L267", (string) null);
      this.calcImpound("1386", (string) null);
      this.calcImpound("1388", (string) null);
      this.calcImpound("1629", (string) null);
      this.calcImpound("340", (string) null);
      this.calcImpound("341", (string) null);
      this.calculateCompensations(id, val);
    }

    private void calcFees()
    {
      if (Tracing.IsSwitchActive(MLDSCalculation.sw, TraceLevel.Info))
        Tracing.Log(MLDSCalculation.sw, TraceLevel.Info, nameof (MLDSCalculation), "routine: calcFees");
      double num1 = this.FltVal("2");
      double num2 = this.FltVal("1061") / 100.0;
      double num3 = Math.Round(num1 * num2, 2) + this.FltVal("436");
      if (this.Val("NEWHUD.X1139") == "Y")
        num3 = this.FltVal("NEWHUD.X15") + this.FltVal("NEWHUD.X788");
      if (this.Val("SYS.X254") == "Broker")
      {
        this.SetCurrentNum("RE88395.X7", 0.0);
        this.SetCurrentNum("RE88395.X6", num3);
      }
      else
        this.SetCurrentNum("RE88395.X7", num3 - this.FltVal("RE88395.X6"));
      double num4 = this.FltVal("389") / 100.0;
      double num5 = Math.Round(num1 * num4, 2) + this.FltVal("1620");
      this.calObjs.GFECal.CalculateField_SYSX266((string) null, (string) null);
      if (this.Val("SYS.X266") != "Broker")
      {
        this.SetCurrentNum("RE88395.X18", 0.0);
        this.SetCurrentNum("RE88395.X19", num5);
      }
      else
        this.SetCurrentNum("RE88395.X18", num5 - this.FltVal("RE88395.X19"));
      if (this.Val("1172") == "FHA")
        num1 = this.FltVal("1109");
      double num6 = this.FltVal("388") / 100.0;
      double num7 = Math.Round(num1 * num6, 2) + this.FltVal("1619");
      if (this.Val("SYS.X252") == "Broker")
      {
        this.SetCurrentNum("RE88395.X4", 0.0);
        this.SetCurrentNum("RE88395.X3", num7);
      }
      else
        this.SetCurrentNum("RE88395.X4", num7 - (this.UseNewGFEHUD || this.UseNew2015GFEHUD ? 0.0 : this.FltVal("RE88395.X3")));
    }

    private void calcImpound(string id, string val)
    {
      if (Tracing.IsSwitchActive(MLDSCalculation.sw, TraceLevel.Info))
        Tracing.Log(MLDSCalculation.sw, TraceLevel.Info, nameof (MLDSCalculation), "routine: calcImpound ID: " + id);
      this.calObjs.RegzCal.CalcDailyInterest(id, val);
      this.SetCurrentNum("RE88395.X40", Math.Round(this.FltVal("333") * this.FltVal("332") - this.FltVal("RE88395.X39"), 2));
      this.SetCurrentNum("RE88395.X46", Math.Round(this.FltVal("L251") * this.FltVal("230") - this.FltVal("RE88395.X45"), 2));
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(id))
      {
        case 669660000:
          if (!(id == "232"))
            return;
          goto label_68;
        case 685451881:
          if (!(id == "253"))
            return;
          goto label_73;
        case 702229500:
          if (!(id == "254"))
            return;
          goto label_74;
        case 703215238:
          if (!(id == "230"))
            return;
          break;
        case 719992857:
          if (!(id == "231"))
            return;
          goto label_70;
        case 782742003:
          if (!(id == "563"))
            return;
          goto label_68;
        case 787103333:
          if (!(id == "235"))
            return;
          goto label_71;
        case 952613878:
          if (!(id == "RE88395.X70"))
            return;
          goto label_74;
        case 969538592:
          if (!(id == "RE88395.X65"))
            return;
          goto label_73;
        case 1019871449:
          if (!(id == "RE88395.X66"))
            return;
          goto label_69;
        case 1029337590:
          if (!(id == "340"))
            return;
          goto label_73;
        case 1046115209:
          if (!(id == "341"))
            return;
          goto label_74;
        case 1053426687:
          if (!(id == "RE88395.X60"))
            return;
          break;
        case 1070204306:
          if (!(id == "RE88395.X63"))
            return;
          goto label_68;
        case 1716133221:
          if (!(id == "1388"))
            return;
          goto label_71;
        case 1732910840:
          if (!(id == "1387"))
            return;
          break;
        case 1749688459:
          if (!(id == "1386"))
            return;
          goto label_70;
        case 2747362738:
          if (!(id == "RE88395.X159"))
            return;
          goto label_71;
        case 3098494710:
          if (!(id == "599"))
            return;
          goto label_74;
        case 3115272329:
          if (!(id == "598"))
            return;
          goto label_73;
        case 3162020223:
          if (!(id == "1632"))
            return;
          goto label_72;
        case 3195575461:
          if (!(id == "1630"))
            return;
          goto label_72;
        case 3245761223:
          if (!(id == "1629"))
            return;
          goto label_72;
        case 3266270900:
          if (!(id == "597"))
            return;
          goto label_71;
        case 3283048519:
          if (!(id == "596"))
            return;
          break;
        case 3299826138:
          if (!(id == "595"))
            return;
          goto label_70;
        case 3317860799:
          if (!(id == "RE88395.X59"))
            return;
          goto label_72;
        case 3391830880:
          if (!(id == "L267"))
            return;
          goto label_69;
        case 3408461404:
          if (!(id == "L270"))
            return;
          goto label_69;
        case 3469006465:
          if (!(id == "RE88395.X44"))
            return;
          goto label_70;
        case 3643495165:
          if (!(id == "L268"))
            return;
          goto label_69;
        case 4222948685:
          if (!(id == "1296"))
            return;
          goto label_68;
        default:
          return;
      }
      string id1 = "1387";
      string id2 = "230";
      string id3 = "RE88395.X146";
      string id4 = "RE88395.X60";
      goto label_75;
label_68:
      id1 = "1296";
      id2 = "232";
      id3 = "RE88395.X147";
      id4 = "RE88395.X63";
      goto label_75;
label_69:
      id1 = "L267";
      id2 = "L268";
      id3 = "RE88395.X148";
      id4 = "RE88395.X66";
      goto label_75;
label_70:
      id1 = "1386";
      id2 = "231";
      id3 = "RE88395.X41";
      id4 = "RE88395.X44";
      goto label_75;
label_71:
      id1 = "1388";
      id2 = "235";
      id3 = "RE88395.X47";
      id4 = "RE88395.X159";
      goto label_75;
label_72:
      id1 = "1629";
      id2 = "1630";
      id3 = "RE88395.X160";
      id4 = "RE88395.X59";
      goto label_75;
label_73:
      id1 = "340";
      id2 = "253";
      id3 = "RE88395.X62";
      id4 = "RE88395.X65";
      goto label_75;
label_74:
      id1 = "341";
      id2 = "254";
      id3 = "RE88395.X71";
      id4 = "RE88395.X70";
label_75:
      double num = this.FltVal(id2) * this.FltVal(id1) - this.FltVal(id4);
      this.SetCurrentNum(id3, Math.Round(num, 2));
    }

    private void calculateCompensations(string id, string val)
    {
      if (string.Compare(this.Val("14"), "CA", true) != 0)
        return;
      Tracing.Log(MLDSCalculation.sw, TraceLevel.Info, nameof (MLDSCalculation), "routine: calculateCompensations");
      double num1 = 0.0;
      double num2 = 0.0;
      this.calcFees();
      if (!this.UseNewGFEHUD && !this.UseNew2015GFEHUD)
      {
        int length = CalculationBase.PaidToOthers.Length;
        for (int index = 0; index < length; ++index)
        {
          if (!(this.Val("SYS.X" + CalculationBase.POCChecks[index]) == "Y"))
          {
            num1 += this.FltVal("RE88395.X" + CalculationBase.PaidToOthers[index]);
            num2 += this.FltVal("RE88395.X" + CalculationBase.PaidToBroker[index]);
          }
        }
        num1 += this.FltVal("558");
      }
      else
      {
        for (int index = 0; index < HUDGFE2010Fields.RE882GFE2010FIELDMAP.Length; ++index)
        {
          string[] strArray = (string[]) HUDGFE2010Fields.RE882GFE2010FIELDMAP[index];
          double num3 = strArray[0] != string.Empty ? this.FltVal(strArray[0]) : 0.0;
          double num4 = strArray[1] != string.Empty ? this.FltVal(strArray[1]) : 0.0;
          if (strArray[5] != string.Empty && this.Val(strArray[5]) == "Y")
          {
            double num5 = strArray[7] != string.Empty ? this.FltVal(strArray[7]) : 0.0;
            if (num5 != num3 + num4)
            {
              double num6 = Utils.ArithmeticRounding(num5 / 2.0, 2);
              if (num6 > num3 || num6 == num3)
              {
                double val1 = num4 - (num5 - num3);
                num3 = 0.0;
                num4 = Utils.ArithmeticRounding(val1, 2);
              }
              else if (num6 > num4 || num6 == num4)
              {
                double val2 = num3 - (num5 - num4);
                num4 = 0.0;
                num3 = Utils.ArithmeticRounding(val2, 2);
              }
              else
              {
                num3 = Utils.ArithmeticRounding(num3 - num6, 2);
                num4 = Utils.ArithmeticRounding(num4 - (num5 - num6), 2);
              }
            }
            else
              continue;
          }
          num1 += num3;
          num2 += num4;
        }
      }
      this.SetCurrentNum("RE882.X63", this.FltVal("RE882.X66") - this.FltVal("RE882.X67"));
      this.SetCurrentNum("RE88395.X153", num1);
      this.SetCurrentNum("RE88395.X152", num2);
      this.SetCurrentNum("RE88395.X108", this.FltVal("RE88395.X152") + this.FltVal("RE88395.X153"));
      this.SetCurrentNum("RE88395.X109", this.FltVal("RE88395.X154") / 100.0 * this.FltVal("2") + this.FltVal("RE88395.X179"));
      double num7 = this.FltVal("RE88395.X108") + this.FltVal("RE88395.X111") + this.FltVal("RE88395.X193") + this.FltVal("RE88395.X113") + this.FltVal("RE88395.X115");
      if (this.UseNewGFEHUD || this.UseNew2015GFEHUD)
        num7 += this.FltVal("RE882.X65");
      this.SetCurrentNum("RE88395.X116", num7);
      this.SetCurrentNum("RE88395.X118", num7 - this.FltVal("2"));
    }

    internal void CopyGFEToMLDS() => this.CopyGFEToMLDS((string) null);

    internal void CopyGFEToMLDS(string id)
    {
      if (string.Compare(this.Val("14"), "CA", true) != 0)
        return;
      if (!this.UseNewGFEHUD && !this.UseNew2015GFEHUD)
      {
        for (int index = 0; index < CalculationBase.BorrowerFields.Length; ++index)
        {
          double num1 = this.FltVal(CalculationBase.BorrowerFields[index]);
          double num2 = this.FltVal(CalculationBase.SellerFields[index]);
          if (this.Val("SYS.X" + CalculationBase.PTBChecks[index]) == "Broker")
          {
            this.SetCurrentNum("RE88395.X" + CalculationBase.PaidToBroker[index], num1 + num2);
            this.SetVal("RE88395.X" + CalculationBase.PaidToOthers[index], "");
          }
          else
          {
            this.SetCurrentNum("RE88395.X" + CalculationBase.PaidToOthers[index], num1 + num2);
            this.SetVal("RE88395.X" + CalculationBase.PaidToBroker[index], "");
          }
          if (num1 == 0.0 && num2 != 0.0)
            this.SetVal("SYS.X" + CalculationBase.PaidByChecks[index], "Seller");
        }
      }
      else
      {
        if (id == "NEWHUD.X782")
        {
          double num = this.FltVal("NEWHUD.X808") + this.FltVal("NEWHUD.X810") + this.FltVal("NEWHUD.X812") + this.FltVal("NEWHUD.X814") + this.FltVal("NEWHUD.X816") + this.FltVal("NEWHUD.X818");
          this.SetCurrentNum("NEWHUD.X645", num);
          this.SetCurrentNum("RE882.X57", num);
        }
        string str1 = id;
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        string empty3 = string.Empty;
        string empty4 = string.Empty;
        string empty5 = string.Empty;
        if (id == "230" || id == "1387")
          str1 = "656";
        else if (id == "231" || id == "1386")
          str1 = "655";
        else if (id == "1629" || id == "1630")
          str1 = "1631";
        else if (id == "1296" || id == "232")
          str1 = "338";
        else if (id == "389" || id == "1620")
          str1 = "439";
        for (int index1 = 0; index1 < HUDGFE2010Fields.RE882GFE2010FIELDMAP.Length; ++index1)
        {
          string[] strArray = (string[]) HUDGFE2010Fields.RE882GFE2010FIELDMAP[index1];
          if (str1 != null)
          {
            bool flag = false;
            for (int index2 = 0; index2 < strArray.Length; ++index2)
            {
              flag = str1 == strArray[index2];
              if (flag)
                break;
            }
            if (!flag)
              continue;
          }
          if (strArray[2] == string.Empty && strArray[3] == string.Empty)
          {
            if (str1 != null)
              break;
          }
          else
          {
            string id1 = strArray[6];
            string str2 = !(id1 == "SYS.X266") ? this.Val(id1) : "Broker";
            double num3 = strArray[2] != string.Empty ? this.FltVal(strArray[2]) : 0.0;
            double num4 = strArray[3] != string.Empty ? this.FltVal(strArray[3]) : 0.0;
            string id2 = strArray[0];
            string id3 = strArray[1];
            string id4 = strArray[4];
            if (str2 == "Broker")
            {
              if (id3 != string.Empty)
                this.SetCurrentNum(id3, num3 + num4);
              if (id2 != string.Empty)
                this.SetVal(id2, "");
            }
            else
            {
              if (id2 != string.Empty)
                this.SetCurrentNum(id2, num3 + num4);
              if (id3 != string.Empty)
                this.SetVal(id3, "");
            }
            if (num3 == 0.0 && num4 != 0.0)
              this.SetVal(id4, "Seller");
            if (str1 != null)
              break;
          }
        }
      }
      this.calculateCompensations((string) null, (string) null);
    }

    private void syncGFE(string id, string val)
    {
      if (CalculationBase.SyncFields.Count == 0)
        CalculationBase.CreateSyncFieldList();
      if (!CalculationBase.SyncFields.ContainsKey((object) id))
        return;
      if (id == "RE882.X56")
      {
        double num = this.FltVal("NEWHUD.X808") + this.FltVal("NEWHUD.X810") + this.FltVal("NEWHUD.X812") + this.FltVal("NEWHUD.X814") + this.FltVal("NEWHUD.X816") + this.FltVal("NEWHUD.X818");
        this.SetCurrentNum("NEWHUD.X645", num);
        this.SetCurrentNum("RE882.X57", num);
      }
      string[] strArray = CalculationBase.SyncFields[(object) id].ToString().Split('|');
      if (strArray[0] == "B")
        strArray[0] = "PTO";
      switch (strArray[0])
      {
        case "PTB":
        case "PTO":
          string str = this.Val(strArray[5]);
          if (this.FltVal(strArray[3]) > 0.0)
          {
            this.SetVal(strArray[6], "Other");
            this.setPayToName(strArray[5], "Other");
          }
          else if (this.FltVal(strArray[4]) != 0.0)
          {
            this.SetVal(strArray[6], "Broker");
            this.setPayToName(strArray[5], "Broker");
          }
          if (str == "Seller")
          {
            this.SetVal(strArray[1], "");
            this.SetCurrentNum(strArray[2], this.FltVal(strArray[3]) + this.FltVal(strArray[4]));
          }
          else
          {
            this.SetVal(strArray[2], "");
            this.SetCurrentNum(strArray[1], this.FltVal(strArray[3]) + this.FltVal(strArray[4]));
          }
          if (id == "RE882.X56")
            this.SetCurrentNum("NEWHUD.X782", this.FltVal("RE882.X56"));
          this.calObjs.NewHudCal.CopyHUD2010ToGFE2010(strArray[1], id, false);
          this.calObjs.NewHudCal.FormCal(id, val);
          break;
        case "PaidBy":
          if (val == "Seller")
          {
            this.SetVal(strArray[1], "");
            this.SetCurrentNum(strArray[2], this.FltVal(strArray[3]) + this.FltVal(strArray[4]));
          }
          else
          {
            this.SetCurrentNum(strArray[1], this.FltVal(strArray[3]) + this.FltVal(strArray[4]));
            this.SetVal(strArray[2], "");
          }
          this.calObjs.NewHudCal.CopyHUD2010ToGFE2010(strArray[1], id, false);
          this.calObjs.NewHudCal.FormCal(id, val);
          break;
      }
      this.calObjs.NewHudCal.CalcREGZGFEHud(id, val);
      this.calObjs.GFECal.CalcClosingCosts(id, val);
    }

    private void setPayToName(string paidByID, string payToType)
    {
      if (!this.UseNew2015GFEHUD || !HUDGFE2010Fields.PAIDBYPOPTFIELDS.Contains((object) paidByID))
        return;
      string[] strArray = (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) paidByID];
      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME] == "" || this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME]) != "" || !(payToType == "Broker"))
        return;
      this.SetVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME], this.Val("VEND.X293"));
    }

    private void calculateMLDSScenarios(string id, string val)
    {
      this.SetCurrentNum("RE88395.X316", (double) (this.IntVal("1947") + this.IntVal("1972") + this.IntVal("1975") + this.IntVal("1978") + this.IntVal("1981")));
      double num1 = 0.0;
      if (this.Val("1946") == "Hard")
        num1 += (double) this.IntVal("1947");
      if (this.Val("1971") == "Hard")
        num1 += (double) this.IntVal("1972");
      if (this.Val("1974") == "Hard")
        num1 += (double) this.IntVal("1975");
      if (this.Val("1977") == "Hard")
        num1 += (double) this.IntVal("1978");
      if (this.Val("1980") == "Hard")
        num1 += (double) this.IntVal("1981");
      this.SetCurrentNum("3536", num1);
      this.mLDSCalculationServant.SyncPrepaymentPenality(id, val);
      double num2 = this.FltVal("1948");
      if (num2 < (double) this.IntVal("1973"))
        num2 = (double) this.IntVal("1973");
      if (num2 < (double) this.IntVal("1976"))
        num2 = (double) this.IntVal("1976");
      if (num2 < (double) this.IntVal("1979"))
        num2 = (double) this.IntVal("1979");
      if (num2 < (double) this.IntVal("1982"))
        num2 = (double) this.IntVal("1982");
      if (this.Val("2830") == "Amount Prepaid")
      {
        this.SetCurrentNum("RE88395.X315", 0.0);
      }
      else
      {
        if (id == "RE88395.X315" && this.IsLocked("RE88395.X315") && this.Val("RE88395.X322") != "Y")
        {
          this.SetVal("RE88395.X322", "Y");
          if (this.Val("LOANTERMTABLE.CUSTOMIZE") != "Y" || !this.UseNew2015GFEHUD)
            this.SetVal("675", "Y");
        }
        if (!this.IsLocked("RE88395.X315"))
          this.SetCurrentNum("RE88395.X315", this.FltVal("1109") * num2 / 100.0);
      }
      if (this.Val("RE88395.X322") != "Y")
      {
        if (this.IsLocked("RE88395.X315"))
          this.loan.RemoveCurrentLock("RE88395.X315");
        this.SetVal("RE88395.X315", "");
      }
      if (this.Val("RE88395.X123") == "Y" || this.Val("14") == "NY" && this.Val("RE88395.X322") != "Y" || this.Val("RE88395.X123") != "Y" && this.Val("RE88395.X191") != "Y" && this.Val("RE88395.X322") != "Y" && this.Val("RE88395.X124") != "Y")
      {
        if (this.IsLocked("RE88395.X316"))
          this.loan.RemoveCurrentLock("RE88395.X316");
        this.SetVal("RE88395.X316", "");
      }
      this.SetCurrentNum("RE88395.X321", this.FltVal("RE88395.X319") + this.FltVal("RE88395.X320") + this.FltVal("RE88395.X333") + this.FltVal("RE88395.X334") + this.FltVal("RE88395.X335"));
      this.SetCurrentNum("RE88395.X318", (double) (this.IntVal("5") + this.IntVal("HUD24")));
      if (this.Val("14") != "CA")
        return;
      double num3 = this.FltVal("RE88395.X299");
      if (num3 > 0.0)
        this.SetCurrentNum("RE88395.X300", num3 + 5.0);
      else
        this.SetVal("RE88395.X300", "");
      double num4 = this.FltVal("RE88395.X301");
      if (num4 > 0.0)
        this.SetCurrentNum("RE88395.X302", num4 + 5.0);
      else
        this.SetVal("RE88395.X302", "");
      double num5 = this.FltVal("RE88395.X304");
      if (num5 > 0.0)
        this.SetCurrentNum("RE88395.X305", num5 + 5.0);
      else
        this.SetVal("RE88395.X305", "");
      int loanTerm = this.IntVal("4");
      double num6 = this.FltVal("1109");
      double num7 = this.FltVal("RE88395.X297");
      double monthlyPayment1 = RegzCalculation.CalculateMonthlyPayment(loanTerm, 0, num6, num7);
      this.SetCurrentNum("RE88395.X249", monthlyPayment1);
      this.SetCurrentNum("RE88395.X250", monthlyPayment1);
      this.SetCurrentNum("RE88395.X251", monthlyPayment1);
      this.SetCurrentNum("RE88395.X252", monthlyPayment1);
      if (loanTerm > 60)
        this.SetCurrentNum("RE88395.X254", monthlyPayment1);
      else
        this.SetVal("RE88395.X254", "");
      double num8 = this.FltVal("736");
      this.SetCurrentNum("RE88395.X253", num8 - this.FltVal("RE88395.X252"));
      this.SetCurrentNum("RE88395.X255", num8 - this.FltVal("RE88395.X254"));
      double num9 = num6;
      double num10 = 0.0;
      for (int index = 1; index <= 60; ++index)
      {
        double num11 = RegzCalculation.CalcInterestPayment(num9, num7, false, 0);
        num9 -= monthlyPayment1 - num11;
      }
      if (num7 > 0.0)
      {
        if (num9 >= 0.0 && loanTerm > 60)
          this.SetCurrentNum("RE88395.X256", num9);
        else
          this.SetVal("RE88395.X256", "");
        if (loanTerm > 60)
          this.SetCurrentNum("RE88395.X307", num6 - num9);
        else
          this.SetCurrentNum("RE88395.X307", num6);
      }
      else
      {
        this.SetVal("RE88395.X256", "");
        this.SetVal("RE88395.X307", "");
      }
      double num12 = this.FltVal("RE88395.X298");
      double unpaidBalance1 = num6;
      double monthlyPayment2 = RegzCalculation.CalculateMonthlyPayment(loanTerm, 0, num6, num12);
      double num13 = RegzCalculation.CalcInterestPayment(unpaidBalance1, num12, false, 0);
      this.SetCurrentNum("RE88395.X257", num13);
      this.SetCurrentNum("RE88395.X260", num13);
      bool useSimpleInterest = this.checkIfSimpleInterest();
      string constInterestType = this.findConstInterestType();
      DateTime firstPaymentDate = this.findFirstPaymentDate();
      bool use366ForLeapYear = this.findSimpleInterestUse366ForLeapYear();
      double num14 = RegzCalculation.CalcRawMonthlyPayment(loanTerm - 60, num6, num12, this.IntVal("4"), 61, num6, this.Val("4746"), useSimpleInterest, firstPaymentDate, constInterestType, use366ForLeapYear);
      bool showZero = this.UseNoPayment(num14);
      this.SetCurrentNum("RE88395.X258", num14, showZero);
      this.SetCurrentNum("RE88395.X259", num14, showZero);
      this.SetCurrentNum("RE88395.X262", num14, showZero);
      this.SetCurrentNum("RE88395.X261", num8 - this.FltVal("RE88395.X260"));
      this.SetCurrentNum("RE88395.X263", num8 - num14);
      if (num12 > 0.0 && loanTerm > 60)
        this.SetCurrentNum("RE88395.X264", num6);
      else
        this.SetVal("RE88395.X264", "");
      double num15 = this.FltVal("RE88395.X299");
      double monthlyPayment3 = RegzCalculation.CalculateMonthlyPayment(loanTerm, 0, num6, num15);
      this.SetCurrentNum("RE88395.X265", monthlyPayment3);
      this.SetCurrentNum("RE88395.X268", monthlyPayment3);
      if (loanTerm > 60)
        this.SetCurrentNum("RE88395.X266", monthlyPayment3);
      else
        this.SetVal("RE88395.X266", "");
      double num16 = num6;
      num10 = 0.0;
      for (int index = 1; index <= 60; ++index)
      {
        double num17 = RegzCalculation.CalcInterestPayment(num16, num15, false, 0);
        num16 -= monthlyPayment3 - num17;
      }
      int unpaidTerm1 = this.IntVal("4") - 60;
      double num18 = unpaidTerm1 <= 0 ? 0.0 : RegzCalculation.CalcRawMonthlyPayment(unpaidTerm1, num16, num15 + 2.0, this.IntVal("4"), 61, num6, this.Val("4746"), useSimpleInterest, firstPaymentDate, constInterestType, use366ForLeapYear);
      if (num15 > 0.0)
        this.SetCurrentNum("RE88395.X267", num18, this.UseNoPayment(num18));
      else
        this.SetVal("RE88395.X267", "");
      this.SetCurrentNum("RE88395.X269", num8 - this.FltVal("RE88395.X268"));
      double num19 = unpaidTerm1 <= 0 ? 0.0 : RegzCalculation.CalcRawMonthlyPayment(unpaidTerm1, num16, num15 + 5.0, this.IntVal("4"), 61, num6, this.Val("4746"), useSimpleInterest, firstPaymentDate, constInterestType, use366ForLeapYear);
      if (num15 > 0.0)
        this.SetCurrentNum("RE88395.X270", num19, this.UseNoPayment(num19));
      else
        this.SetVal("RE88395.X270", "");
      this.SetCurrentNum("RE88395.X271", num8 - this.FltVal("RE88395.X270"));
      if (num15 > 0.0)
      {
        if (num16 >= 0.0 && loanTerm > 60)
          this.SetCurrentNum("RE88395.X272", num16);
        else
          this.SetVal("RE88395.X272", "");
        if (loanTerm > 60)
          this.SetCurrentNum("RE88395.X308", num6 - num16);
        else
          this.SetCurrentNum("RE88395.X308", num6);
      }
      else
      {
        this.SetVal("RE88395.X272", "");
        this.SetVal("RE88395.X308", "");
      }
      double num20 = this.FltVal("RE88395.X301");
      double unpaidBalance2 = num6;
      monthlyPayment2 = RegzCalculation.CalculateMonthlyPayment(loanTerm, 0, num6, num20);
      double num21 = RegzCalculation.CalcInterestPayment(unpaidBalance2, num20, false, 0);
      this.SetCurrentNum("RE88395.X273", num21);
      this.SetCurrentNum("RE88395.X276", num21);
      double num22 = RegzCalculation.CalcRawMonthlyPayment(loanTerm - 60, num6, num20, this.IntVal("4"), 61, num6, this.Val("4746"), useSimpleInterest, firstPaymentDate, constInterestType, use366ForLeapYear);
      this.SetCurrentNum("RE88395.X274", num22, this.UseNoPayment(num22));
      double num23 = num20 <= 0.0 ? 0.0 : RegzCalculation.CalcRawMonthlyPayment(loanTerm - 60, num6, num20 + 2.0, this.IntVal("4"), 61, num6, this.Val("4746"), useSimpleInterest, firstPaymentDate, constInterestType, use366ForLeapYear);
      this.SetCurrentNum("RE88395.X275", num23, this.UseNoPayment(num23));
      this.SetCurrentNum("RE88395.X277", num8 - this.FltVal("RE88395.X276"));
      double num24 = num20 <= 0.0 ? 0.0 : RegzCalculation.CalcRawMonthlyPayment(loanTerm - 60, num6, num20 + 5.0, this.IntVal("4"), 61, num6, this.Val("4746"), useSimpleInterest, firstPaymentDate, constInterestType, use366ForLeapYear);
      this.SetCurrentNum("RE88395.X278", num24, this.UseNoPayment(num24));
      this.SetCurrentNum("RE88395.X279", num8 - num24);
      if (num20 > 0.0 && loanTerm > 60)
        this.SetCurrentNum("RE88395.X280", num6);
      else
        this.SetVal("RE88395.X280", "");
      double annualRate = this.FltVal("RE88395.X303");
      double monthlyPayment4 = RegzCalculation.CalculateMonthlyPayment(loanTerm, 0, num6, annualRate);
      this.SetCurrentNum("RE88395.X281", monthlyPayment4);
      this.SetCurrentNum("RE88395.X284", monthlyPayment4);
      this.SetCurrentNum("RE88395.X285", num8 - monthlyPayment4);
      double unpaidBalance3 = num6;
      double currentRate1 = this.FltVal("RE88395.X313") + this.FltVal("RE88395.X314");
      double num25 = currentRate1 <= 0.0 ? RegzCalculation.CalcInterestPayment(unpaidBalance3, this.FltVal("RE88395.X303"), false, 0) : RegzCalculation.CalcInterestPayment(unpaidBalance3, currentRate1, false, 0);
      double num26 = unpaidBalance3 - (monthlyPayment4 - num25);
      double currentRate2 = this.FltVal("RE88395.X304");
      int unpaidTerm2 = this.IntVal("4") - 1;
      double num27 = unpaidTerm2 <= 0 ? 0.0 : RegzCalculation.CalcRawMonthlyPayment(unpaidTerm2, num26, currentRate2, this.IntVal("4"), 2, num6, this.Val("4746"), useSimpleInterest, firstPaymentDate, constInterestType, use366ForLeapYear);
      double currentRate3 = this.FltVal("RE88395.X313") + this.FltVal("RE88395.X314");
      for (int index = 2; index <= 60; ++index)
      {
        double num28 = RegzCalculation.CalcInterestPayment(num26, currentRate3, false, 0);
        num26 -= num27 - num28;
      }
      double currentRate4 = this.FltVal("RE88395.X304");
      int unpaidTerm3 = this.IntVal("4") - 60;
      double num29 = unpaidTerm3 <= 0 ? 0.0 : RegzCalculation.CalcRawMonthlyPayment(unpaidTerm3, num26, currentRate4, this.IntVal("4"), 61, num6, this.Val("4746"), useSimpleInterest, firstPaymentDate, constInterestType, use366ForLeapYear);
      this.SetCurrentNum("RE88395.X282", num29, this.UseNoPayment(num29));
      this.SetCurrentNum("RE88395.X283", unpaidTerm3 <= 0 || currentRate4 <= 0.0 ? 0.0 : RegzCalculation.CalcRawMonthlyPayment(unpaidTerm3, num26, currentRate4 + 2.0, this.IntVal("4"), 61, num6, this.Val("4746"), useSimpleInterest, firstPaymentDate, constInterestType, use366ForLeapYear));
      double num30 = unpaidTerm3 <= 0 || currentRate4 <= 0.0 ? 0.0 : RegzCalculation.CalcRawMonthlyPayment(unpaidTerm3, num26, currentRate4 + 5.0, this.IntVal("4"), 61, num6, this.Val("4746"), useSimpleInterest, firstPaymentDate, constInterestType, use366ForLeapYear);
      this.SetCurrentNum("RE88395.X286", num30, this.UseNoPayment(num30));
      this.SetCurrentNum("RE88395.X287", num8 - num30);
      if (currentRate4 > 0.0)
      {
        if (num26 >= 0.0 && loanTerm > 60)
          this.SetCurrentNum("RE88395.X288", num26);
        else
          this.SetVal("RE88395.X288", "");
        if (num26 > num6)
          this.SetCurrentNum("RE88395.X309", num26 - num6);
        else
          this.SetCurrentNum("RE88395.X309", 0.0);
      }
      else
      {
        this.SetVal("RE88395.X288", "");
        this.SetVal("RE88395.X309", "");
      }
      double num31 = this.FltVal("5");
      this.SetCurrentNum("RE88395.X289", num31);
      this.SetCurrentNum("RE88395.X292", num31);
      this.SetCurrentNum("RE88395.X293", num8 - this.FltVal("RE88395.X292"));
      this.SetCurrentNum("RE88395.X295", num8 - this.FltVal("RE88395.X294"));
      double num32 = num6 - this.FltVal("RE88395.X296");
      if (num32 > 0.0)
      {
        this.SetVal("RE88395.X312", "decreased");
        this.SetVal("RE88395.X311", "Yes");
        this.SetCurrentNum("RE88395.X310", num32);
      }
      else if (num32 < 0.0)
      {
        this.SetVal("RE88395.X312", "increased");
        this.SetVal("RE88395.X311", "No");
        this.SetCurrentNum("RE88395.X310", num32 * -1.0);
      }
      else
      {
        this.SetVal("RE88395.X312", "did not change");
        this.SetVal("RE88395.X311", "No");
        this.SetCurrentNum("RE88395.X310", 0.0);
      }
      if (loanTerm <= 60)
      {
        this.SetVal("RE88395.X250", "");
        this.SetVal("RE88395.X251", "");
        this.SetVal("RE88395.X254", "");
      }
      if ((!(this.Val("608") == "") || !(this.Val("1172") == "")) && num31 != 0.0)
        return;
      for (int index = 289; index <= 296; ++index)
        this.SetCurrentNum("RE88395.X" + index.ToString(), 0.0);
      this.SetCurrentNum("RE88395.X310", 0.0);
      this.SetVal("RE88395.X311", "");
      this.SetVal("RE88395.X312", "");
    }

    private void calculateRE882(string id, string val)
    {
      double num1 = 0.0;
      for (int index = 7; index <= 24; ++index)
      {
        if (index != 19 && index != 22)
          num1 += this.FltVal("RE882.X" + (object) index);
      }
      this.SetCurrentNum("RE882.X25", num1);
      double num2 = 0.0;
      for (int index = 28; index <= 46; ++index)
      {
        if (index != 38 && index != 41 && index != 44)
          num2 += this.FltVal("RE882.X" + (object) index);
      }
      this.SetCurrentNum("RE882.X47", num2);
      string str = this.Val("1172");
      double num3 = str == "FHA" || str == "VA" || str == "FarmersHomeAdministration" ? this.FltVal("2") : this.FltVal("1109");
      this.SetCurrentNum("RE882.X55", num3);
      this.SetCurrentNum("RE882.X6", num3 - (this.FltVal("RE882.X25") + this.FltVal("RE882.X26") + this.FltVal("RE882.X27") + this.FltVal("RE882.X47")));
    }
  }
}

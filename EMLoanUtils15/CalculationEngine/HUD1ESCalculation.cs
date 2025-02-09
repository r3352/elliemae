// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.HUD1ESCalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class HUD1ESCalculation : CalculationBase
  {
    private const string className = "HUD1ESCalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    internal Routine CalcHUD1ES;
    internal Routine CalcBiWeeklyEscrowPayment;
    internal Routine CalcEscrowFirstPaymentDate;
    internal Routine CalcShiftPaymentFrequence;

    internal HUD1ESCalculation(LoanData l, EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.CalcHUD1ES = this.RoutineX(new Routine(this.calculateHUD1ES));
      this.CalcBiWeeklyEscrowPayment = this.RoutineX(new Routine(this.calculateBiWeeklyEscrowPayment));
      this.CalcEscrowFirstPaymentDate = this.RoutineX(new Routine(this.calculateEscrowFirstPaymentDate));
      this.CalcShiftPaymentFrequence = this.RoutineX(new Routine(this.calculateShiftPaymentFrequence));
      this.addFieldHandlers();
    }

    private void addFieldHandlers()
    {
      Routine routine1 = this.RoutineX(new Routine(this.calculateHUD1ES));
      this.AddFieldHandler("VEND.X482", routine1);
      this.AddFieldHandler("VEND.X483", routine1);
      this.AddFieldHandler("VEND.X484", routine1);
      this.AddFieldHandler("HUD41", routine1);
      this.AddFieldHandler("HUD42", routine1);
      this.AddFieldHandler("HUD43", routine1);
      this.AddFieldHandler("HUD44", routine1);
      this.AddFieldHandler("HUD45", routine1);
      this.AddFieldHandler("HUD46", routine1);
      this.AddFieldHandler("HUD47", routine1);
      this.AddFieldHandler("HUD48", routine1);
      this.AddFieldHandler("HUD50", routine1);
      this.AddFieldHandler("NEWHUD.X1707", routine1);
      Routine routine2 = this.RoutineX(new Routine(this.calculateBiWeeklyEscrowPayment));
      this.AddFieldHandler("HUD51", routine2);
      this.AddFieldHandler("HUD52", routine2);
      this.AddFieldHandler("HUD53", routine2);
      this.AddFieldHandler("HUD54", routine2);
      this.AddFieldHandler("HUD55", routine2);
      this.AddFieldHandler("HUD56", routine2);
      this.AddFieldHandler("HUD58", routine2);
      this.AddFieldHandler("HUD60", routine2);
      this.AddFieldHandler("HUD62", routine2);
      this.AddFieldHandler("HUD63", routine2);
      this.AddFieldHandler("HUD65", this.RoutineX(new Routine(this.calculateBiWeeklyEscrowPayment)));
    }

    internal void FormCal() => this.calculateHUD1ES((string) null, (string) null);

    private void calculateHUD1ES(string id, string val)
    {
      if (Tracing.IsSwitchActive(HUD1ESCalculation.sw, TraceLevel.Info))
        Tracing.Log(HUD1ESCalculation.sw, TraceLevel.Info, nameof (HUD1ESCalculation), "routine: calculateHUD1ES ID: " + id);
      bool flag1 = false;
      this.SetCurrentNum("HUD41", Utils.ArithmeticRounding(this.FltVal("231") * 12.0, 2));
      this.SetCurrentNum("HUD42", Utils.ArithmeticRounding(this.FltVal("230") * 12.0, 2));
      this.SetCurrentNum("HUD43", Utils.ArithmeticRounding(this.FltVal("232") * 12.0, 2));
      this.SetCurrentNum("HUD44", Utils.ArithmeticRounding(this.FltVal("235") * 12.0, 2));
      this.SetCurrentNum("HUD45", Utils.ArithmeticRounding(this.FltVal("L268") * 12.0, 2));
      this.SetCurrentNum("HUD46", Utils.ArithmeticRounding(this.FltVal("1630") * 12.0, 2));
      this.SetCurrentNum("HUD47", Utils.ArithmeticRounding(this.FltVal("253") * 12.0, 2));
      this.SetCurrentNum("HUD48", Utils.ArithmeticRounding(this.FltVal("254") * 12.0, 2));
      if (this.FltVal("HUD.YearlyUSDAFee") != 0.0)
        this.SetCurrentNum("HUD50", this.FltVal("HUD.YearlyUSDAFee"));
      else
        this.SetCurrentNum("HUD50", Utils.ArithmeticRounding(this.FltVal("NEWHUD.X1707") * 12.0, 2));
      bool isBiweekly = this.Val("423") == "Biweekly";
      if (!isBiweekly)
      {
        for (int index = 30; index < 39; ++index)
        {
          if (this.IntVal("HUD" + index.ToString("00")) > 2)
            this.SetVal("HUD" + index.ToString("00"), "2");
        }
      }
      double[] numArray1 = new double[9];
      double[] numArray2 = new double[9];
      numArray1[0] = this.FltVal("231");
      numArray1[1] = this.FltVal("230");
      numArray1[2] = this.FltVal("232");
      numArray1[3] = this.FltVal("235");
      numArray1[4] = this.FltVal("L268");
      numArray1[5] = this.FltVal("1630");
      numArray1[6] = this.FltVal("253");
      numArray1[7] = this.FltVal("254");
      numArray1[8] = this.FltVal("NEWHUD.X1707");
      if (isBiweekly)
      {
        for (int index = 0; index < 9; ++index)
          numArray2[index] = Utils.ArithmeticRounding(numArray1[index] * (6.0 / 13.0), 2);
      }
      double num1 = 0.0;
      double num2 = 0.0;
      for (int index = 0; index < numArray1.Length; ++index)
      {
        int num3 = index + 41;
        if (flag1 || !(this.Val("HUD01" + num3.ToString()) == "") && !(this.Val("HUD01" + num3.ToString()) == "//"))
        {
          double num4 = isBiweekly ? numArray2[index] : numArray1[index];
          num1 += num4;
          if (index != 2)
            num2 += num4;
        }
      }
      double num5 = this.FltVal("HUD24");
      this.SetCurrentNum("HUD24", num1);
      if (num1 > 0.0)
        this.SetVal("4556", "Y");
      else if (num1 == 0.0 && num5 != num1)
        this.SetVal("4556", "");
      if (num1 > 0.0)
        this.SetVal("DISCLOSURE.X671", "Required");
      else
        this.SetVal("DISCLOSURE.X671", "Not Required");
      if (this.Val("DISCLOSURE.X454") != "Y")
        this.SetVal("DISCLOSURE.X666", "");
      this.SetCurrentNum("NEWHUD.X950", num2);
      this.SetCurrentNum("HUD26", this.FltVal("231") + this.FltVal("230") + this.FltVal("232") + this.FltVal("235") + this.FltVal("L268") + this.FltVal("1630") + this.FltVal("253") + this.FltVal("254") + this.FltVal("NEWHUD.X1707"));
      this.calObjs.NewHudCal.CalcHud1Page3PredependentFields();
      this.calObjs.NewHud2015Cal.CalcEstimatedTaxesTable();
      this.SetCurrentNum("RE88395.X318", (double) (this.IntVal("5") + this.IntVal("HUD24")));
      int[] numArray3 = new int[9];
      for (int index = 30; index < 39; ++index)
      {
        numArray3[index - 30] = this.IntVal("HUD" + index.ToString("00"));
        if (!flag1 && numArray3[index - 30] > 0)
          flag1 = true;
      }
      DateTime dateTime = Utils.ParseDate((object) this.Val("HUD68"));
      if (dateTime == DateTime.MinValue)
        dateTime = DateTime.Today;
      DateTime firstPayDate = dateTime;
      DateTime date = Utils.ParseDate((object) this.Val("748"));
      if (date != DateTime.MinValue)
        date.Date.AddMonths(12);
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      double[] numArray4 = new double[9];
      int[] numArray5 = new int[9];
      for (int index = 0; index < 9; ++index)
        numArray4[index] = 0.0;
      int num6;
      for (int index1 = 1; index1 < 49; ++index1)
      {
        string empty3 = string.Empty;
        string val1 = !(dateTime == DateTime.MinValue) ? (isBiweekly ? dateTime.ToString("MM/dd/yy") : (index1 <= 24 ? dateTime.ToString("MM/yy") : "")) : string.Empty;
        this.SetVal("HUD" + index1.ToString("00") + "01", val1);
        double num7 = 0.0;
        for (int index2 = 1; index2 < 10; ++index2)
        {
          int num8 = index1;
          num6 = index2 + 12;
          int num9 = this.IntVal("HUD" + num8.ToString("00") + num6.ToString("00"));
          int num10 = index1;
          num6 = index2 == 9 ? 60 : index2 + 1;
          string id1 = "HUD" + num10.ToString("00") + num6.ToString("00");
          double val2;
          if (!isBiweekly && index1 > 24)
          {
            val2 = 0.0;
          }
          else
          {
            num6 = index2 == 9 ? 50 : index2 + 40;
            double num11 = this.IsLocked("HUD" + num6.ToString()) || index2 == 9 ? this.FltVal("HUD" + num6.ToString()) : 0.0;
            if (num11 > 0.0)
            {
              switch (num9)
              {
                case 3:
                  val2 = num11 / 4.0;
                  break;
                case 4:
                  val2 = num11 / 3.0;
                  break;
                case 6:
                  val2 = num11 / 2.0;
                  break;
                case 12:
                  val2 = num11;
                  break;
                default:
                  val2 = num11 / 12.0 * (double) num9;
                  break;
              }
            }
            else
              val2 = (double) num9 * numArray1[index2 - 1];
            if (!flag1 && num9 > 0)
              flag1 = true;
          }
          double num12 = Utils.ArithmeticRounding(val2, 2);
          this.SetCurrentNum(id1, num12);
          num6 = index2 + 49;
          if (this.Val("HUD" + num10.ToString("00") + num6.ToString("00")) != "Y")
            num7 += num12;
          numArray4[index2 - 1] += num12;
        }
        int num13 = index1;
        num6 = 10;
        this.SetCurrentNum("HUD" + num13.ToString("00") + num6.ToString("00"), num7);
        if (dateTime != DateTime.MinValue)
          dateTime = !isBiweekly ? dateTime.AddMonths(1) : dateTime.AddDays(14.0);
      }
      if (id == "682" || id == "HUD68" || id == "HUD69" || id == "IMPORT")
      {
        this.updateEscrowSetUpDates(firstPayDate, isBiweekly);
        this.setDueDates(firstPayDate, isBiweekly);
      }
      int[] numArray6 = new int[9];
      double num14 = 0.0;
      string val3 = "";
      double num15 = 0.0;
      string val4 = "";
      double num16 = 0.0;
      string val5 = "";
      double num17 = 0.0;
      string val6 = "";
      double num18 = 0.0;
      string val7 = "";
      double num19 = 0.0;
      string val8 = "";
      double num20 = 0.0;
      string val9 = "";
      double num21 = 0.0;
      string val10 = "";
      double num22 = 0.0;
      string val11 = "";
      double num23 = 0.0;
      string val12 = "";
      if (this.Val("VEND.X482") == "ForTax")
      {
        this.SetVal("VEND.X485", "");
        this.SetVal("VEND.X346", "");
        this.SetVal("VEND.X455", "");
      }
      else if (this.Val("VEND.X482") == "ForInsurance")
      {
        this.SetVal("VEND.X459", "");
        this.SetVal("VEND.X460", "");
        this.SetVal("VEND.X463", "");
      }
      else
      {
        this.SetVal("VEND.X459", "");
        this.SetVal("VEND.X460", "");
        this.SetVal("VEND.X463", "");
        this.SetVal("VEND.X485", "");
        this.SetVal("VEND.X346", "");
        this.SetVal("VEND.X455", "");
      }
      if (this.Val("VEND.X483") == "ForTax")
      {
        this.SetVal("VEND.X486", "");
        this.SetVal("VEND.X355", "");
        this.SetVal("VEND.X464", "");
      }
      else if (this.Val("VEND.X483") == "ForInsurance")
      {
        this.SetVal("VEND.X468", "");
        this.SetVal("VEND.X469", "");
        this.SetVal("VEND.X472", "");
      }
      else
      {
        this.SetVal("VEND.X486", "");
        this.SetVal("VEND.X355", "");
        this.SetVal("VEND.X464", "");
        this.SetVal("VEND.X468", "");
        this.SetVal("VEND.X469", "");
        this.SetVal("VEND.X472", "");
      }
      if (this.Val("VEND.X484") == "ForTax")
      {
        this.SetVal("VEND.X487", "");
        this.SetVal("VEND.X364", "");
        this.SetVal("VEND.X473", "");
      }
      else if (this.Val("VEND.X484") == "ForInsurance")
      {
        this.SetVal("VEND.X477", "");
        this.SetVal("VEND.X478", "");
        this.SetVal("VEND.X481", "");
      }
      else
      {
        this.SetVal("VEND.X487", "");
        this.SetVal("VEND.X364", "");
        this.SetVal("VEND.X473", "");
        this.SetVal("VEND.X477", "");
        this.SetVal("VEND.X478", "");
        this.SetVal("VEND.X481", "");
      }
label_106:
      int num24;
      for (int index3 = 1; index3 < 10; ++index3)
      {
        if (index3 != 3 && numArray1[index3 - 1] != 0.0)
        {
          for (int index4 = 1; index4 < 49; ++index4)
          {
            num24 = index4;
            num6 = index3 == 9 ? 60 : index3 + 1;
            double num25 = this.FltVal("HUD" + num24.ToString("00") + num6.ToString("00"));
            if (num25 > 0.0)
            {
              switch (index3)
              {
                case 1:
                  num14 = num25;
                  val3 = this.Val("HUD0141");
                  goto label_106;
                case 2:
                  num15 = num25;
                  val4 = this.Val("HUD0142");
                  goto label_106;
                case 4:
                  num16 = num25;
                  val5 = this.Val("HUD0144");
                  goto label_106;
                case 5:
                  num17 = num25;
                  val6 = this.Val("HUD0145");
                  goto label_106;
                case 6:
                  if (this.Val("VEND.X482") == "ForTax")
                  {
                    num18 = num25;
                    val7 = this.Val("HUD0146");
                    goto label_106;
                  }
                  else if (this.Val("VEND.X482") == "ForInsurance")
                  {
                    num19 = num25;
                    val8 = this.Val("HUD0146");
                    goto label_106;
                  }
                  else
                    goto label_106;
                case 7:
                  if (this.Val("VEND.X483") == "ForTax")
                  {
                    num20 = num25;
                    val9 = this.Val("HUD0147");
                    goto label_106;
                  }
                  else if (this.Val("VEND.X483") == "ForInsurance")
                  {
                    num21 = num25;
                    val10 = this.Val("HUD0147");
                    goto label_106;
                  }
                  else
                    goto label_106;
                case 8:
                  if (this.Val("VEND.X484") == "ForTax")
                  {
                    num22 = num25;
                    val11 = this.Val("HUD0148");
                    goto label_106;
                  }
                  else if (this.Val("VEND.X484") == "ForInsurance")
                  {
                    num23 = num25;
                    val12 = this.Val("HUD0148");
                    goto label_106;
                  }
                  else
                    goto label_106;
                default:
                  goto label_106;
              }
            }
            else if (!isBiweekly && index4 >= 24)
              break;
          }
        }
      }
      if (num14 != 0.0)
        this.SetCurrentNum("VEND.X440", num14);
      else
        this.SetVal("VEND.X440", "");
      this.SetVal("VEND.X441", val3);
      if (num15 != 0.0)
        this.SetCurrentNum("VEND.X443", num15);
      else
        this.SetVal("VEND.X443", "");
      this.SetVal("VEND.X444", val4);
      if (num16 != 0.0)
        this.SetCurrentNum("VEND.X447", num16);
      else
        this.SetVal("VEND.X447", "");
      this.SetVal("VEND.X448", val5);
      if (num17 != 0.0)
        this.SetCurrentNum("VEND.X452", num17);
      else
        this.SetVal("VEND.X452", "");
      this.SetVal("VEND.X453", val6);
      if (num18 != 0.0)
        this.SetCurrentNum("VEND.X461", num18);
      else
        this.SetVal("VEND.X461", "");
      this.SetVal("VEND.X462", val7);
      if (num19 != 0.0)
        this.SetCurrentNum("VEND.X456", num19);
      else
        this.SetVal("VEND.X456", "");
      this.SetVal("VEND.X457", val8);
      if (num20 != 0.0)
        this.SetCurrentNum("VEND.X470", num20);
      else
        this.SetVal("VEND.X470", "");
      this.SetVal("VEND.X471", val9);
      if (num21 != 0.0)
        this.SetCurrentNum("VEND.X465", num21);
      else
        this.SetVal("VEND.X465", "");
      this.SetVal("VEND.X466", val10);
      if (num22 != 0.0)
        this.SetCurrentNum("VEND.X479", num22);
      else
        this.SetVal("VEND.X479", "");
      this.SetVal("VEND.X480", val11);
      if (num23 != 0.0)
        this.SetCurrentNum("VEND.X474", num23);
      else
        this.SetVal("VEND.X474", "");
      this.SetVal("VEND.X475", val12);
      int[] numArray7 = new int[10];
      for (int index5 = 1; index5 < 10; ++index5)
      {
        if (numArray1[index5 - 1] == 0.0)
        {
          numArray6[index5 - 1] = 0;
        }
        else
        {
          double num26 = 0.0;
          double num27 = 0.0;
          double val13 = 0.0;
          numArray6[index5 - 1] = 0;
          for (int index6 = 1; index6 < 49; ++index6)
          {
            double num28 = !isBiweekly ? numArray1[index5 - 1] : numArray2[index5 - 1];
            num26 += num28;
            num24 = index6;
            num6 = index5 == 9 ? 60 : index5 + 1;
            double num29 = this.FltVal("HUD" + num24.ToString("00") + num6.ToString("00"));
            num6 = index5 + 49;
            bool flag2 = this.Val("HUD" + num24.ToString("00") + num6.ToString("00")) == "Y";
            if (num29 > 0.0)
            {
              if (isBiweekly && index6 <= 26 || !isBiweekly && index6 <= 12)
                ++numArray7[index5 - 1];
              if (!flag2)
              {
                double num30 = Utils.ArithmeticRounding(2.0 * (num29 - num26) / num28, 0) * 0.5;
                if (num30 <= 0.0)
                  num27 += num30;
                else if (Math.Abs(num27) < num30)
                {
                  val13 += num30 + num27;
                  num27 = 0.0;
                }
                else
                  num27 += num30;
                num26 = 0.0;
              }
            }
            if (!isBiweekly && index6 >= 24)
              break;
          }
          numArray6[index5 - 1] = Convert.ToInt32(Utils.ArithmeticRounding(val13, 0));
          int num31 = isBiweekly ? 1 : 0;
        }
      }
      for (int feeType = 0; feeType < numArray7.Length; ++feeType)
        this.setPaymentFrequency(feeType, numArray7[feeType]);
      double val14 = 0.0;
      for (int index = 0; index < 9; ++index)
      {
        string id2 = string.Empty;
        switch (index)
        {
          case 0:
            id2 = "1386";
            break;
          case 1:
            id2 = "1387";
            break;
          case 2:
            id2 = "1296";
            break;
          case 3:
            id2 = "1388";
            break;
          case 4:
            id2 = "L267";
            break;
          case 5:
            id2 = "1629";
            break;
          case 6:
            id2 = "340";
            break;
          case 7:
            id2 = "341";
            break;
          case 8:
            id2 = "NEWHUD.X1706";
            break;
        }
        val14 += (double) numArray3[index] * (isBiweekly ? numArray2[index] : numArray1[index]);
        if (flag1 && numArray1[index] != 0.0)
          this.SetCurrentNum(id2, (double) (numArray6[index] + numArray3[index]));
      }
      double num32 = 0.0;
      double num33 = isBiweekly ? 26.0 : 12.0;
      for (int index = 0; index < 9; ++index)
        num32 = Utils.ArithmeticRounding(num32 + numArray4[index] / num33, 2);
      double num34 = num32 - this.FltVal("HUD0110");
      double val15 = num34;
      for (int index = 2; index <= 24; ++index)
      {
        num34 += num32 - this.FltVal("HUD" + index.ToString("00") + "10");
        if (num34 < val15)
          val15 = num34;
      }
      if (isBiweekly)
      {
        for (int index = 25; index <= 48; ++index)
        {
          num34 += num32 - this.FltVal("HUD" + index.ToString("00") + "10");
          if (num34 < val15)
            val15 = num34;
        }
      }
      double num35 = Utils.ArithmeticRounding(Utils.ArithmeticRounding(val14, 2) - Utils.ArithmeticRounding(val15, 2), 2);
      if (flag1)
        this.SetCurrentNum("HUD23", num35);
      double num36 = 0.0;
      for (int index = 0; index < 9; ++index)
        num36 += (double) (numArray6[index] + numArray3[index]) * (isBiweekly ? numArray2[index] : numArray1[index]);
      this.SetCurrentNum("HUD40", num36);
      this.SetCurrentNum("558", this.EMRounding(num35 - num36, 2));
      this.calObjs.NewHud2015FeeDetailCal.Calc_2015LineItem1011(id, val);
      if (this.calObjs.CurrentFormID == "REGZGFE_2010")
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("558", id, false);
      else if (this.calObjs.CurrentFormID != "HUD1PG2_2010" && this.IsSyncGFERequired)
        this.calObjs.NewHudCal.CopyHUD2010ToGFE2010("558", id, false);
      double num37 = num35;
      for (int index = 1; index < 49; ++index)
      {
        num37 = Utils.ArithmeticRounding(num37 + Utils.ArithmeticRounding(num1 - this.FltVal("HUD" + index.ToString("00") + "10"), 2), 2);
        if (!isBiweekly && index > 24)
          this.SetCurrentNum("HUD" + index.ToString("00") + "11", 0.0);
        else
          this.SetCurrentNum("HUD" + index.ToString("00") + "11", num37);
      }
      this.SetCurrentNum("HUD25", this.EMRounding(num37, 2));
      this.calObjs.GFECal.CalcPrepaid(id, val);
      this.PopulateItemizedFields();
      this.calObjs.ToolCal.CalcCPAEscrowDetails_FCD_AmountFields(id, val);
      this.calObjs.ToolCal.CalcCPAEscrowDetails_AddlEscrow_AmountFields(id, val);
    }

    internal void PopulateItemizedFields()
    {
      bool flag1 = this.Val("423") == "Biweekly";
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      int num1 = 0;
      int num2 = 0;
      for (int index1 = 1; index1 < 49; ++index1)
      {
        ++num1;
        if (num1 <= 48)
        {
          if (flag1 || !flag1 && index1 <= 12)
            num2 = num1;
          this.SetVal("AEA" + num1.ToString("00") + "01", this.Val("HUD" + index1.ToString("00") + "01"));
          this.SetVal("AEA" + num1.ToString("00") + "02", this.Val("HUD24"));
          if (!this.calObjs.UseItemizeEscrow)
            this.SetVal("AEA" + num1.ToString("00") + "03", this.Val("HUD" + index1.ToString("00") + "10"));
          bool flag2 = true;
          string val = string.Empty;
          for (int index2 = 2; index2 <= 10; ++index2)
          {
            double num3 = this.FltVal("HUD" + index1.ToString("00") + (index2 == 10 ? "60" : index2.ToString("00")));
            bool flag3 = this.Val("HUD" + index1.ToString("00") + (index2 + 48).ToString("00")) == "Y";
            string str = string.Empty;
            if (num3 > 0.0)
            {
              if (this.calObjs.UseItemizeEscrow)
              {
                if (!flag2)
                {
                  if (!flag3)
                  {
                    ++num1;
                    this.SetVal("AEA" + num1.ToString("00") + "01", "");
                    this.SetVal("AEA" + num1.ToString("00") + "02", "");
                  }
                  else
                    continue;
                }
                this.SetVal("AEA" + num1.ToString("00") + "03", num3 == 0.0 || flag3 ? "" : num3.ToString("N2"));
                this.SetVal("AEA" + num1.ToString("00") + "05", "");
              }
              if (flag1 || !flag1 && index1 <= 12)
                num2 = num1;
              switch (index2)
              {
                case 2:
                  str = "Tax";
                  break;
                case 3:
                  str = "Hzd. Ins.";
                  break;
                case 4:
                  str = "Mtg. Ins.";
                  break;
                case 5:
                  str = "Flood Ins.";
                  break;
                case 6:
                  str = "City Taxes";
                  break;
                case 7:
                  str = this.loan.GetField("1628");
                  break;
                case 8:
                  str = this.loan.GetField("660");
                  break;
                case 9:
                  str = this.loan.GetField("661");
                  break;
                case 10:
                  str = "USDA Annual Fee";
                  break;
              }
              if (flag3)
                str = string.Empty;
              if (val == string.Empty)
                val = str;
              else if (str != string.Empty || !flag3)
                val = val + "," + str;
              if (this.calObjs.UseItemizeEscrow)
              {
                this.SetVal("AEA" + num1.ToString("00") + "04", val);
                val = string.Empty;
              }
              flag2 = false;
            }
          }
          if (this.calObjs.UseItemizeEscrow & flag2)
          {
            this.SetVal("AEA" + num1.ToString("00") + "03", "");
            this.SetVal("AEA" + num1.ToString("00") + "04", "");
          }
          else if (!this.calObjs.UseItemizeEscrow)
            this.SetVal("AEA" + num1.ToString("00") + "04", val);
          this.SetVal("AEA" + num1.ToString("00") + "05", this.Val("HUD" + index1.ToString("00") + "11"));
        }
        else
          break;
      }
      if (!flag1 && (num2 < 12 || !this.calObjs.UseItemizeEscrow && num2 > 12))
        num2 = 12;
      else if (flag1 && num2 > 26 && !this.calObjs.UseItemizeEscrow)
        num2 = 26;
      this.SetVal("AEA.X1", string.Concat((object) num2));
      this.SetVal("AEA.X2", this.calObjs.UseItemizeEscrow ? "Y" : "");
      for (int index3 = num2 + 1; index3 < 49; ++index3)
      {
        for (int index4 = 1; index4 <= 5; ++index4)
          this.SetVal("AEA" + index3.ToString("00") + index4.ToString("00"), "");
      }
    }

    private void setPaymentFrequency(int feeType, int frequency)
    {
      if (feeType == 1 || feeType == 2 || feeType == 3 || frequency == 0)
      {
        if (frequency != 0)
          return;
        switch (feeType)
        {
          case 0:
            this.SetVal("VEND.X437", "");
            break;
          case 4:
            this.SetVal("VEND.X449", "");
            break;
          case 5:
            this.SetVal("VEND.X458", "");
            break;
          case 6:
            this.SetVal("VEND.X467", "");
            break;
          case 7:
            this.SetVal("VEND.X476", "");
            break;
        }
      }
      else
      {
        string val = "Other";
        switch (frequency)
        {
          case 1:
            val = "Annually";
            break;
          case 2:
            val = "Semi-Annually";
            break;
          case 3:
            val = "Tri-Annually";
            break;
          case 4:
            val = "Quarterly";
            break;
          case 12:
            val = "Monthly";
            break;
        }
        switch (feeType)
        {
          case 0:
            this.SetVal("VEND.X437", val);
            break;
          case 4:
            this.SetVal("VEND.X449", val);
            break;
          case 5:
            this.SetVal("VEND.X458", this.Val("VEND.X482") == "ForTax" ? val : "");
            break;
          case 6:
            this.SetVal("VEND.X467", this.Val("VEND.X483") == "ForTax" ? val : "");
            break;
          case 7:
            this.SetVal("VEND.X476", this.Val("VEND.X484") == "ForTax" ? val : "");
            break;
        }
      }
    }

    private void setDueDates(DateTime firstPayDate, bool isBiweekly)
    {
      string empty = string.Empty;
      bool flag = this.Val("HUD68") == "" || this.Val("HUD68") == "//";
      for (int index1 = 1; index1 < 10; ++index1)
      {
        int num1 = 0;
        for (int index2 = 1; index2 < 49; ++index2)
        {
          int num2 = index1 + 12;
          if (this.FltVal("HUD" + index2.ToString("00") + num2.ToString("00")) > 0.0)
          {
            ++num1;
            if (isBiweekly)
            {
              int num3 = index1 + 40;
              string id = "HUD" + num1.ToString("00") + num3.ToString("00");
              if (firstPayDate == DateTime.MinValue)
              {
                this.SetVal(id, "");
              }
              else
              {
                string s = this.Val(id);
                string str = this.Val("HUD" + index2.ToString("00") + "01");
                if (string.IsNullOrEmpty(s))
                {
                  this.SetVal(id, str);
                }
                else
                {
                  DateTime result1 = DateTime.MinValue;
                  DateTime result2 = DateTime.MinValue;
                  DateTime.TryParse(s, out result1);
                  DateTime.TryParse(str, out result2);
                  if (result1 < result2 || result1 >= result2.AddDays(14.0))
                    this.SetVal(id, str);
                }
              }
            }
            else
            {
              int num4;
              DateTime dateTime1;
              if (flag)
              {
                DateTime dateTime2 = firstPayDate.AddMonths(index2 - 1);
                num4 = dateTime2.Month;
                string str1 = num4.ToString("00");
                num4 = dateTime2.Year;
                string str2 = num4.ToString("0000");
                dateTime1 = Utils.ParseDate((object) (str1 + "/01/" + str2));
              }
              else
                dateTime1 = firstPayDate.AddMonths(index2 - 1);
              int num5 = index1 + 40;
              string id1 = "HUD" + num1.ToString("00") + num5.ToString("00");
              if (firstPayDate == DateTime.MinValue)
              {
                this.SetVal(id1, "");
              }
              else
              {
                DateTime date = Utils.ParseDate((object) this.Val(id1));
                if (date != DateTime.MinValue)
                {
                  if (date.Day <= 28)
                  {
                    string id2 = id1;
                    string[] strArray = new string[5];
                    num4 = dateTime1.Month;
                    strArray[0] = num4.ToString("00");
                    strArray[1] = "/";
                    num4 = date.Day;
                    strArray[2] = num4.ToString("00");
                    strArray[3] = "/";
                    num4 = dateTime1.Year;
                    strArray[4] = num4.ToString("0000");
                    string val = string.Concat(strArray);
                    this.SetVal(id2, val);
                  }
                  else
                  {
                    string[] strArray1 = new string[5];
                    num4 = dateTime1.Month;
                    strArray1[0] = num4.ToString("00");
                    strArray1[1] = "/";
                    num4 = date.Day;
                    strArray1[2] = num4.ToString("00");
                    strArray1[3] = "/";
                    num4 = dateTime1.Year;
                    strArray1[4] = num4.ToString("0000");
                    DateTime dateTime3 = Utils.ParseDate((object) string.Concat(strArray1));
                    if (dateTime3 == DateTime.MinValue)
                    {
                      num4 = dateTime1.Month;
                      string str3 = num4.ToString("00");
                      num4 = dateTime1.Year;
                      string str4 = num4.ToString("0000");
                      dateTime3 = Utils.ParseDate((object) (str3 + "/01/" + str4));
                      dateTime3 = dateTime3.AddMonths(1).AddDays(-1.0);
                    }
                    string id3 = id1;
                    string[] strArray2 = new string[5];
                    num4 = dateTime3.Month;
                    strArray2[0] = num4.ToString("00");
                    strArray2[1] = "/";
                    num4 = dateTime3.Day;
                    strArray2[2] = num4.ToString("00");
                    strArray2[3] = "/";
                    num4 = dateTime3.Year;
                    strArray2[4] = num4.ToString("0000");
                    string val = string.Concat(strArray2);
                    this.SetVal(id3, val);
                  }
                }
                else
                  this.SetVal(id1, dateTime1.ToString("MM/dd/yyyy"));
              }
            }
          }
          if (num1 >= 4)
            break;
        }
        if (num1 < 4)
        {
          for (int index3 = num1 + 1; index3 <= 4; ++index3)
          {
            int num6 = index1 + 40;
            this.SetVal("HUD" + index3.ToString("00") + num6.ToString("00"), "");
          }
        }
      }
    }

    private void updateEscrowSetUpDates(DateTime firstPayDate, bool isBiweekly)
    {
      if (firstPayDate == DateTime.MinValue)
        firstPayDate = DateTime.Now;
      for (int index = 1; index < 49; ++index)
      {
        string empty = string.Empty;
        string id = "HUD" + index.ToString("00") + "12";
        string val;
        if (isBiweekly)
        {
          val = firstPayDate.ToString("MM/dd/yyyy");
          firstPayDate = firstPayDate.AddDays(14.0);
        }
        else
        {
          val = firstPayDate.ToString("MM/yyyy");
          firstPayDate = firstPayDate.AddMonths(1);
        }
        this.SetVal(id, val);
      }
    }

    private void calculateShiftPaymentFrequence(string id, string val)
    {
      if (!this.calObjs.EnableTriggerToRunEscrowDateCalc)
        return;
      DateTime result = DateTime.MinValue;
      DateTime.TryParseExact(this.Val("HUD0101"), new string[2]
      {
        "MM/yy",
        "MM/dd/yy"
      }, (IFormatProvider) null, DateTimeStyles.None, out result);
      if (result == DateTime.MinValue)
        return;
      DateTime date = Utils.ParseDate((object) this.Val("682"));
      this.ShiftPaymentFrequence(result, date);
    }

    internal void ShiftPaymentFrequence(DateTime previous1stPayDate, DateTime new1stPayDate)
    {
      if (previous1stPayDate == DateTime.MinValue || new1stPayDate == DateTime.MinValue)
        return;
      if (this.Val("423") == "Biweekly")
      {
        List<List<string[]>> strArrayListList = new List<List<string[]>>();
        List<int> intList = new List<int>();
        for (int index1 = 13; index1 <= 21; ++index1)
        {
          List<string[]> strArrayList = new List<string[]>();
          int num = 1;
          for (int index2 = 1; index2 <= 48; ++index2)
          {
            string str1 = this.Val("HUD" + index2.ToString("00") + index1.ToString("00"));
            if (str1 != string.Empty)
            {
              string str2 = this.Val("HUD" + index2.ToString("00") + (index1 + 37).ToString("00"));
              if (num <= 4)
              {
                string str3 = this.Val("HUD" + num.ToString("00") + (index1 + 28).ToString("00"));
                if (str3 != string.Empty)
                {
                  DateTime date = Utils.ParseDate((object) str3);
                  if (date >= new1stPayDate && date < new1stPayDate.AddMonths(12))
                    strArrayList.Add(new string[3]
                    {
                      str3,
                      str1,
                      str2
                    });
                  else if (!intList.Contains(index1 - 12))
                    intList.Add(index1 - 12);
                }
                else
                  strArrayList.Add(new string[3]
                  {
                    this.Val("HUD" + index2.ToString("00") + "12"),
                    str1,
                    str2
                  });
              }
              else
                strArrayList.Add(new string[3]
                {
                  this.Val("HUD" + index2.ToString("00") + "12"),
                  str1,
                  str2
                });
              ++num;
            }
          }
          strArrayListList.Add(strArrayList);
        }
        for (int index3 = 0; index3 < intList.Count; ++index3)
        {
          int num = intList[index3];
          for (int index4 = 1; index4 <= 4; ++index4)
          {
            if (index4 <= strArrayListList[num - 1].Count)
              this.SetVal("HUD" + index4.ToString("00") + (num + 40).ToString("00"), strArrayListList[num - 1][index4 - 1][0]);
            else
              this.SetVal("HUD" + index4.ToString("00") + (num + 40).ToString("00"), string.Empty);
          }
        }
        DateTime dateTime = new1stPayDate;
        for (int index5 = 1; index5 <= 48; ++index5)
        {
          for (int index6 = 0; index6 < 9; ++index6)
          {
            List<string[]> strArrayList = strArrayListList[index6];
            if (strArrayList.Count > 0)
            {
              DateTime date = Utils.ParseDate((object) strArrayList[0][0]);
              if (date >= dateTime && date < dateTime.AddDays(14.0) && date < new1stPayDate.AddMonths(12))
              {
                this.SetVal("HUD" + index5.ToString("00") + (index6 + 13).ToString("00"), strArrayList[0][1]);
                this.SetVal("HUD" + index5.ToString("00") + (index6 + 50).ToString("00"), strArrayList[0][2]);
                strArrayList.RemoveAt(0);
              }
              else
              {
                this.SetVal("HUD" + index5.ToString("00") + (index6 + 13).ToString("00"), string.Empty);
                this.SetVal("HUD" + index5.ToString("00") + (index6 + 50).ToString("00"), string.Empty);
              }
            }
            else
            {
              this.SetVal("HUD" + index5.ToString("00") + (index6 + 13).ToString("00"), string.Empty);
              this.SetVal("HUD" + index5.ToString("00") + (index6 + 50).ToString("00"), string.Empty);
            }
          }
          dateTime = dateTime.AddDays(14.0);
        }
      }
      else
      {
        int num = new1stPayDate.Year * 12 + new1stPayDate.Month - (previous1stPayDate.Year * 12 + previous1stPayDate.Month);
        if (num == 0)
          return;
        List<string[]> strArrayList = new List<string[]>();
        for (int index = 1; index <= 24; ++index)
          strArrayList.Add(new string[19]
          {
            this.Val("HUD" + index.ToString("00") + "12"),
            this.Val("HUD" + index.ToString("00") + "13"),
            this.Val("HUD" + index.ToString("00") + "14"),
            this.Val("HUD" + index.ToString("00") + "15"),
            this.Val("HUD" + index.ToString("00") + "16"),
            this.Val("HUD" + index.ToString("00") + "17"),
            this.Val("HUD" + index.ToString("00") + "18"),
            this.Val("HUD" + index.ToString("00") + "19"),
            this.Val("HUD" + index.ToString("00") + "20"),
            this.Val("HUD" + index.ToString("00") + "21"),
            this.Val("HUD" + index.ToString("00") + "50"),
            this.Val("HUD" + index.ToString("00") + "51"),
            this.Val("HUD" + index.ToString("00") + "52"),
            this.Val("HUD" + index.ToString("00") + "53"),
            this.Val("HUD" + index.ToString("00") + "54"),
            this.Val("HUD" + index.ToString("00") + "55"),
            this.Val("HUD" + index.ToString("00") + "56"),
            this.Val("HUD" + index.ToString("00") + "57"),
            this.Val("HUD" + index.ToString("00") + "58")
          });
        if (num > 0)
        {
          for (int index = 0; index < num; ++index)
          {
            string[] strArray = strArrayList[0];
            strArrayList.RemoveAt(0);
            strArrayList.Add(new string[0]);
          }
        }
        else
        {
          for (int index = 0; index > num; --index)
          {
            string[] strArray = strArrayList[23];
            strArrayList.RemoveAt(23);
            strArrayList.Insert(0, new string[0]);
          }
        }
        for (int index7 = 1; index7 <= 24; ++index7)
        {
          for (int index8 = 13; index8 <= 21; ++index8)
          {
            if (strArrayList[index7 - 1].Length == 0)
              this.SetVal("HUD" + index7.ToString("00") + index8.ToString("00"), "");
            else
              this.SetVal("HUD" + index7.ToString("00") + index8.ToString("00"), strArrayList[index7 - 1][index8 - 12]);
          }
          for (int index9 = 50; index9 <= 58; ++index9)
          {
            if (strArrayList[index7 - 1].Length == 0)
              this.SetVal("HUD" + index7.ToString("00") + index9.ToString("00"), "");
            else
              this.SetVal("HUD" + index7.ToString("00") + index9.ToString("00"), strArrayList[index7 - 1][index9 - 40]);
          }
        }
      }
    }

    private void calculateBiWeeklyEscrowPayment(string id, string val)
    {
      double num1 = 6.0 / 13.0;
      this.SetCurrentNum("HUD52", Utils.ArithmeticRounding(this.FltVal("231") * num1, 2));
      this.SetCurrentNum("HUD53", Utils.ArithmeticRounding(this.FltVal("230") * num1, 2));
      this.SetCurrentNum("HUD54", Utils.ArithmeticRounding(this.FltVal("232") * num1, 2));
      this.SetCurrentNum("HUD55", Utils.ArithmeticRounding(this.FltVal("235") * num1, 2));
      this.SetCurrentNum("HUD56", Utils.ArithmeticRounding(this.FltVal("L268") * num1, 2));
      this.SetCurrentNum("HUD58", Utils.ArithmeticRounding(this.FltVal("1630") * num1, 2));
      this.SetCurrentNum("HUD60", Utils.ArithmeticRounding(this.FltVal("253") * num1, 2));
      this.SetCurrentNum("HUD62", Utils.ArithmeticRounding(this.FltVal("254") * num1, 2));
      this.SetCurrentNum("HUD63", Utils.ArithmeticRounding(this.FltVal("NEWHUD.X1707") * num1, 2));
      double num2 = 0.0;
      if (this.Val("NEWHUD.X1728") == "Y")
        num2 += this.FltVal("HUD54");
      if (this.Val("NEWHUD.X337") == "Y")
        num2 += this.FltVal("HUD52");
      if (this.Val("NEWHUD.X339") == "Y")
        num2 += this.FltVal("HUD53");
      if (this.Val("NEWHUD.X338") == "Y")
        num2 += this.FltVal("HUD55");
      if (this.Val("NEWHUD.X1726") == "Y")
        num2 += this.FltVal("HUD56");
      if (this.Val("NEWHUD.X340") == "Y")
        num2 += this.FltVal("HUD58");
      if (this.Val("NEWHUD.X341") == "Y")
        num2 += this.FltVal("HUD60");
      if (this.Val("NEWHUD.X342") == "Y")
        num2 += this.FltVal("HUD62");
      if (this.Val("NEWHUD.X343") == "Y")
        num2 += this.FltVal("HUD63");
      this.SetCurrentNum("HUD65", num2);
      this.SetCurrentNum("HUD64", num2 + this.FltVal("HUD51"));
    }

    private void calculateEscrowFirstPaymentDate(string id, string val)
    {
      DateTime dateTime = DateTime.MinValue;
      if (this.Val("19") != "ConstructionToPermanent")
      {
        this.SetVal("HUD69", "FirstPaymentDate");
        DateTime date = Utils.ParseDate((object) this.Val("682"));
        this.SetVal("HUD68", date != DateTime.MinValue ? date.ToString("MM/dd/yyyy") : "");
      }
      else
      {
        this.SetVal("HUD68", "");
        string str = this.Val("HUD69");
        switch (str)
        {
          case "FirstAmortDate":
            dateTime = Utils.ParseDate((object) this.Val("1963"));
            break;
          case "FirstPaymentDate":
            dateTime = Utils.ParseDate((object) this.Val("682"));
            break;
        }
        if (!(str != ""))
          return;
        this.SetVal("HUD68", dateTime != DateTime.MinValue ? dateTime.ToString("MM/dd/yyyy") : "");
      }
    }
  }
}

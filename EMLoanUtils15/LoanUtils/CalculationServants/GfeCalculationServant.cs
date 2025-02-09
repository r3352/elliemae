// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.GfeCalculationServant
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Globalization;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.CalculationServants
{
  public class GfeCalculationServant : CalculationServantBase
  {
    private readonly ISettingsProvider _systemSettings;
    private Hashtable cityStateUserTables;
    private Hashtable titleEscrowTables;

    public GfeCalculationServant(ILoanModelProvider modelProvider, ISettingsProvider systemSettings)
      : base(modelProvider)
    {
      this._systemSettings = systemSettings;
    }

    public void PopulateFeeList(string id, bool recalculated)
    {
      this.UpdateTitleEscrowTable();
      this.UpdateCityStateUserFees(id, (string) null);
      if (!recalculated)
        return;
      if (this.UseNewGfeHud || this.UseNew2015GFEHUD)
        this.CalculationObjects.NewHudCalFormCal((string) null, (string) null);
      else
        this.CalculationObjects.GfeCalFormCal("GFE", (string) null, (string) null);
    }

    public void UpdateTitleEscrowTable()
    {
      if (this.titleEscrowTables == null)
        this.titleEscrowTables = this._systemSettings.GetCachedTitleEscrowTables();
      if (this.titleEscrowTables == null)
        return;
      for (int index = 0; index < 3 && (this.UseNewGfeHud || this.UseNew2015GFEHUD || index <= 1); ++index)
      {
        string name = string.Empty;
        switch (index)
        {
          case 0:
            name = this.Val("ESCROW_TABLE");
            break;
          case 1:
            name = this.Val("TITLE_TABLE");
            break;
          case 2:
            name = this.Val("2010TITLE_TABLE");
            break;
        }
        if (!(name == string.Empty))
        {
          string str1 = this.Val("19");
          if (!(str1 == string.Empty))
          {
            TableFeeListBase tableFeeListBase = (TableFeeListBase) null;
            if (str1.IndexOf("Refinance", StringComparison.Ordinal) > -1)
            {
              if (index == 0)
              {
                if (this.titleEscrowTables.ContainsKey((object) 1))
                  tableFeeListBase = (TableFeeListBase) (BinaryObject) this.titleEscrowTables[(object) 1];
              }
              else if (this.titleEscrowTables.ContainsKey((object) 3))
                tableFeeListBase = (TableFeeListBase) (BinaryObject) this.titleEscrowTables[(object) 3];
            }
            else if (index == 0)
            {
              if (this.titleEscrowTables.ContainsKey((object) 0))
                tableFeeListBase = (TableFeeListBase) (BinaryObject) this.titleEscrowTables[(object) 0];
            }
            else if (this.titleEscrowTables.ContainsKey((object) 2))
              tableFeeListBase = (TableFeeListBase) (BinaryObject) this.titleEscrowTables[(object) 2];
            if (tableFeeListBase != null && tableFeeListBase.Count != 0)
            {
              TableFeeListBase.FeeTable table = tableFeeListBase.GetTable(name);
              if (table != null && (!this.UseNewGfeHud && !this.UseNew2015GFEHUD || (index != 2 || !(table.FeeType != "Owner")) && (index != 1 || !(table.FeeType != "Lender"))))
              {
                double num = this.CalcTitleEscrowRate(table);
                string str2 = string.Empty;
                if (this.UseNewGfeHud || this.UseNew2015GFEHUD)
                {
                  switch (index)
                  {
                    case 0:
                      str2 = "NEWHUD.X808";
                      break;
                    case 1:
                      str2 = "NEWHUD.X639";
                      break;
                    case 2:
                      str2 = "NEWHUD.X572";
                      break;
                  }
                }
                else
                {
                  switch (index)
                  {
                    case 0:
                      str2 = "387";
                      break;
                    case 1:
                      str2 = "385";
                      break;
                  }
                }
                if (!string.IsNullOrEmpty(str2) && Math.Abs(this.ParseDouble(this.Val(str2)) - num) >= 0.01)
                {
                  this.SetVal(str2, num > 0.0 ? num.ToString("N2") : "");
                  if (this.UseNewGfeHud || this.UseNew2015GFEHUD)
                  {
                    if (index == 0)
                    {
                      this.ModelProvider.TriggerCalculation("NEWHUD.X808", num.ToString("N2"));
                      this.CalculationObjects.NewHudCalCopyHud2010ToGfe2010("NEWHUD.X645", (string) null, false);
                    }
                    else
                      this.CalculationObjects.NewHudCalCopyHud2010ToGfe2010(str2, (string) null, false);
                  }
                }
              }
            }
          }
        }
      }
      this.CalculationObjects.Calc_2015TitleFees((string) null, (string) null);
    }

    public void UpdateCityStateUserFees(string id, string val)
    {
      if (this.cityStateUserTables == null)
        this.cityStateUserTables = this._systemSettings.GetCachedCityStateUserTables();
      for (int key = 0; key <= 3; ++key)
      {
        FeeListBase feeListBase = (FeeListBase) null;
        if (this.cityStateUserTables != null && key != 3 && this.cityStateUserTables.ContainsKey((object) key))
          feeListBase = (FeeListBase) (BinaryObject) this.cityStateUserTables[(object) key];
        switch (key)
        {
          case 0:
            if (!this.UpdateFee(id, "1637", "647", "RE88395.X94", "593", feeListBase) && this.Val("1637").IndexOf(";Mortgage", StringComparison.Ordinal) > -1)
            {
              if ((id ?? "") == "1637")
                this.ParseFeeDescription(id, val);
              if ((id ?? "") == "647")
                this.SetCurrentNum("593", this.FltVal("2405") + this.FltVal("2406") - this.FltVal("647"));
              else
                this.SetCurrentNum("647", this.FltVal("2405") + this.FltVal("2406") - this.FltVal("593"));
              if (id == "647")
              {
                this.CalculationObjects.NewHudCalCopyHud2010ToGfe2010("647", id, false);
                break;
              }
              break;
            }
            break;
          case 1:
            if (!this.UpdateFee(id, "1638", "648", "RE88395.X89", "594", feeListBase) && this.Val("1638").IndexOf(";Mortgage", StringComparison.Ordinal) > -1)
            {
              if ((id ?? "") == "1638")
                this.ParseFeeDescription(id, val);
              if ((id ?? "") == "648")
                this.SetCurrentNum("594", this.FltVal("2407") + this.FltVal("2408") - this.FltVal("648"));
              else
                this.SetCurrentNum("648", this.FltVal("2407") + this.FltVal("2408") - this.FltVal("594"));
              if (id == "648")
              {
                this.CalculationObjects.NewHudCalCopyHud2010ToGfe2010("648", id, false);
                break;
              }
              break;
            }
            break;
          case 2:
            if (!this.UpdateFee(id, "373", "374", "RE88395.X99", "576", feeListBase) && id == "374")
              this.CalculationObjects.NewHudCalCopyHud2010ToGfe2010("374", id, false);
            if (!this.UpdateFee(id, "1640", "1641", "RE88395.X100", "1642", feeListBase) && id == "1641")
              this.CalculationObjects.NewHudCalCopyHud2010ToGfe2010("1641", id, false);
            if (!this.UpdateFee(id, "1643", "1644", "RE88395.X169", "1645", feeListBase) && id == "1644")
            {
              this.CalculationObjects.NewHudCalCopyHud2010ToGfe2010("1644", id, false);
              break;
            }
            break;
          case 3:
            if (this.Val("1636").IndexOf(";Mortgage", StringComparison.Ordinal) > -1)
            {
              if ((id ?? "") == "1636")
                this.ParseFeeDescription(id, val);
              if ((id ?? "") == "390")
              {
                this.SetCurrentNum("587", this.FltVal("2402") + this.FltVal("2403") + this.FltVal("2404") - this.FltVal("390"));
                break;
              }
              this.SetCurrentNum("390", this.FltVal("2402") + this.FltVal("2403") + this.FltVal("2404") - this.FltVal("587"));
              break;
            }
            break;
        }
      }
      this.CalculationObjects.Calc_2015CityStateTaxFees(id, val);
    }

    private double CalcTitleEscrowRate(TableFeeListBase.FeeTable t)
    {
      if (t.RateList == string.Empty)
        return 0.0;
      double num1 = 0.0;
      switch (t.CalcBasedOn)
      {
        case "Sales Price":
          num1 = this.ParseDouble(this.Val("136"));
          break;
        case "Loan Amount":
          num1 = this.ParseDouble(this.Val("2"));
          break;
        case "Appraisal Value":
          num1 = this.ParseDouble(this.Val("356"));
          break;
        case "Base Loan Amount":
          num1 = this.ParseDouble(this.Val("1109"));
          break;
      }
      string[] strArray1 = t.RateList.Split('|');
      int length = strArray1.Length;
      double num2 = 0.0;
      double num3 = 0.0;
      for (int index = 0; index < length; ++index)
      {
        string[] strArray2 = strArray1[index].Split(':');
        if (strArray2.Length > 2)
        {
          double num4 = Utils.ParseDouble((object) strArray2[0]);
          if (num1 <= num4)
          {
            num2 = Utils.ParseDouble((object) strArray2[1]);
            num3 = Utils.ParseDouble((object) strArray2[2]) / 100.0;
            break;
          }
        }
      }
      double num5 = Utils.ParseDouble((object) t.Nearest);
      double num6 = Utils.ParseDouble((object) t.Offset);
      if (num5 > 0.0)
      {
        double num7 = num1 % num5;
        if (num7 > 0.0)
        {
          if (t.Rounding == "Up")
            num1 += num5 - num7;
          else
            num1 -= num7;
        }
        num1 -= num6;
      }
      return Utils.ArithmeticRounding(num1 * num3 + num2, 0);
    }

    private bool UpdateFee(
      string idInFocus,
      string feeNameId,
      string borFieldId,
      string caFieldId,
      string sellerId,
      FeeListBase feeListBase)
    {
      string lower = this.Val(feeNameId).ToLower();
      if ((lower ?? "") == string.Empty || feeListBase == null)
        return false;
      for (int i = 0; i < feeListBase.Count; ++i)
      {
        FeeListBase.FeeTable tableAt = feeListBase.GetTableAt(i);
        if (lower == tableAt.FeeName.ToLower())
        {
          double num = this.LookUpFee(tableAt.CalcBasedOn, Utils.ParseDouble((object) tableAt.Rate), Utils.ParseDouble((object) tableAt.Additional));
          if ((idInFocus ?? "") != "" && idInFocus == borFieldId)
            this.SetCurrentNum(sellerId, num - this.FltVal(borFieldId));
          else
            this.SetCurrentNum(borFieldId, num - this.FltVal(sellerId));
          this.SetVal(caFieldId, num.ToString((IFormatProvider) CultureInfo.InvariantCulture));
          if ((idInFocus == "136" || idInFocus == "1109" || idInFocus == "1771" || idInFocus == "1335") && tableAt.CalcBasedOn == "Loan Amount" || idInFocus == "136" && tableAt.CalcBasedOn == "Purchase Price")
            this.CalculationObjects.NewHudCalCopyHud2010ToGfe2010(borFieldId, borFieldId, false);
          return true;
        }
      }
      return false;
    }

    private double LookUpFee(string basedOn, double rate, double additional)
    {
      return Utils.ArithmeticRounding(this.FltVal(basedOn == "Loan Amount" ? "2" : "136") * (rate / 100.0) + additional, 2);
    }

    private void ParseFeeDescription(string id, string val)
    {
      int startIndex = -1;
      val = val.ToLower();
      for (int index = 1; index <= 3; ++index)
      {
        switch (index)
        {
          case 1:
            startIndex = val.IndexOf("deed", StringComparison.Ordinal);
            break;
          case 2:
            startIndex = val.IndexOf("mortgage", StringComparison.Ordinal);
            break;
          case 3:
            startIndex = val.IndexOf("releases", StringComparison.Ordinal);
            break;
        }
        if (startIndex != -1)
        {
          int num = val.IndexOf(";", startIndex, StringComparison.Ordinal);
          string str1 = val.Substring(startIndex, num > -1 ? num - startIndex : val.Length - startIndex);
          switch (index)
          {
            case 1:
              str1 = str1.ToLower().Trim().Replace("deed", "").Trim();
              break;
            case 2:
              str1 = str1.ToLower().Trim().Replace("mortgage", "").Trim();
              break;
            case 3:
              str1 = str1.ToLower().Trim().Replace("releases", "").Trim();
              break;
          }
          string str2 = str1.ToLower().Replace("$", "").ToLower().Replace(",", "");
          switch (id)
          {
            case "1636":
              switch (index)
              {
                case 1:
                  this.SetVal("2402", str2);
                  continue;
                case 2:
                  this.SetVal("2403", str2);
                  continue;
                default:
                  this.SetVal("2404", str2);
                  continue;
              }
            case "1637":
              switch (index)
              {
                case 1:
                  this.SetVal("2405", str2);
                  continue;
                case 2:
                  this.SetVal("2406", str2);
                  continue;
                default:
                  continue;
              }
            case "1638":
              switch (index)
              {
                case 1:
                  this.SetVal("2407", str2);
                  continue;
                case 2:
                  this.SetVal("2408", str2);
                  continue;
                default:
                  continue;
              }
            default:
              continue;
          }
        }
      }
    }
  }
}

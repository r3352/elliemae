// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.NewHudCalculationServant
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.CalculationServants
{
  public class NewHudCalculationServant : CalculationServantBase
  {
    private readonly bool _stopSyncItemization;
    private readonly bool _manualSyncItemization;

    public NewHudCalculationServant(
      ILoanModelProvider modelProvider,
      ISettingsProvider systemSettings)
      : base(modelProvider)
    {
      SyncItemizationSetting systemPolicy = (SyncItemizationSetting) systemSettings.GetSystemPolicy("Policies.SyncItemization");
      this._stopSyncItemization = systemPolicy != 0;
      this._manualSyncItemization = systemPolicy == SyncItemizationSetting.ManualUpdate;
    }

    public void CopyHud2010ToGfe2010(string sourceId, string currentId, bool blankCopyOnly)
    {
      if (sourceId != null && sourceId != "copytohudgfe")
      {
        if (this._stopSyncItemization && this.IsLoanDisclosed() || this.CalculationObjects.CurrentFormId == "HUD1PG2_2010")
          return;
        if (currentId != "IMPORT" && currentId != null)
        {
          if (sourceId == "NEWHUD.X1301" && currentId != "NEWHUD.X1301" && currentId != "1172" && currentId != "1109" && this.Val("1172") == "FarmersHomeAdministration" || sourceId == "337" && currentId != "337" && currentId != "1172" || sourceId == "642" && currentId != "230" && currentId != "642" && currentId != "L251" && currentId != "578")
            return;
          if (sourceId == "656")
          {
            string str = this.Val("1750");
            if ((!(currentId == "1109") && !(currentId == "1771") && !(currentId == "1335") && !(currentId == "136") || !(str == "Loan Amount") && !(str == "Base Loan Amount")) && (!(currentId == "356") || !(str == "Appraisal Value")) && currentId != "230" && currentId != "1387" && currentId != "596")
              return;
          }
          if (sourceId == "338" && currentId != "338" && currentId != "1296" && currentId != "232" && currentId != "563" && currentId != "1172" || sourceId == "655" && currentId != "1405" && currentId != "1386" && currentId != "231" && currentId != "595" || sourceId == "L269" && currentId != "L267" && currentId != "L268" && currentId != "L270" || sourceId == "657" && currentId != "1388" && currentId != "235" && currentId != "597" || sourceId == "1631" && currentId != "1629" && currentId != "1630" && currentId != "1632" || sourceId == "658" && currentId != "340" && currentId != "253" && currentId != "598" || sourceId == "659" && currentId != "341" && currentId != "254" && currentId != "599" || sourceId == "NEWHUD.X1708" && currentId != "234" && currentId != "NEWHUD.X1708" && currentId != "NEWHUD.X1706" && currentId != "NEWHUD.X1707" && currentId != "NEWHUD.X1714" && currentId != "1172" || sourceId == "NEWHUD.X645" && currentId != "NEWHUD.X645" || sourceId == "390" && currentId != "390" && currentId != "587" || sourceId == "2402" && currentId != "2402" || sourceId == "647" && currentId != "647" && currentId != "593" || sourceId == "2405" && currentId != "2405" || sourceId == "648" && currentId != "648" && currentId != "594" || sourceId == "2407" && currentId != "2407" || sourceId == "374" && currentId != "374" && currentId != "576" || sourceId == "1641" && currentId != "1641" && currentId != "1642" || sourceId == "1644" && currentId != "1644" && currentId != "1645")
            return;
        }
        switch (sourceId)
        {
          case "1061":
          case "436":
          case "NEWHUD.X788":
            this.CalculationObjects.NewHudCalCalculateHudgfe(sourceId, this.Val(sourceId));
            if (this.ParseDouble(this.Val("NEWHUD.X15")) > 0.0)
            {
              this.ModelProvider.RemoveLock("NEWHUD.X13");
              this.ModelProvider.RemoveLock("NEWHUD.X720");
              break;
            }
            break;
          case "1109":
            this.SyncBorSelFieldToGfe("NEWHUD.X622", "337", "562");
            this.SyncBorSelFieldToGfe("NEWHUD.X661", "1050", "571");
            break;
          case "1387":
          case "230":
          case "L251":
            this.SyncBorSelFieldToGfe("NEWHUD.X650", "642", "578");
            break;
          case "1636":
            sourceId = "390";
            break;
          case "1637":
            sourceId = "647";
            break;
          case "1638":
            sourceId = "648";
            break;
          case "332":
          case "561":
          case "L244":
          case "L245":
          case "SYS.X8":
            this.SyncBorSelFieldToGfe("NEWHUD.X701", "334", "561");
            break;
          case "388":
            if (this.ModelProvider.IsLocked("NEWHUD.X770"))
            {
              this.ModelProvider.RemoveLock("NEWHUD.X770");
              break;
            }
            break;
          case "454":
            this.CalculationObjects.GfeCalCalcGfeFees("454", this.Val("454"));
            this.ModelProvider.RemoveLock("NEWHUD.X770");
            break;
          case "559":
            this.CalculationObjects.GfeCalCalcGfeFees("559", this.Val("559"));
            if (this.ModelProvider.IsLocked("454") && this.ParseDouble(this.Val("559")) > 0.0)
            {
              this.ModelProvider.RemoveLock("454");
              this.ModelProvider.RemoveLock("NEWHUD.X770");
              break;
            }
            if (this.ModelProvider.IsLocked("454"))
            {
              this.ModelProvider.RemoveLock("NEWHUD.X770");
              break;
            }
            break;
          case "562":
            this.SyncBorSelFieldToGfe("NEWHUD.X622", "337", "562");
            break;
          case "571":
            this.SyncBorSelFieldToGfe("NEWHUD.X661", "1050", "571");
            break;
          case "578":
            this.SyncBorSelFieldToGfe("NEWHUD.X650", "642", "578");
            break;
          case "NEWHUD.X1155":
            this.ModelProvider.RemoveLock("NEWHUD.X1210");
            break;
          case "NEWHUD.X1159":
            this.ModelProvider.RemoveLock("NEWHUD.X1212");
            break;
          case "NEWHUD.X1163":
            this.ModelProvider.RemoveLock("NEWHUD.X1214");
            break;
          case "NEWHUD.X645":
          case "NEWHUD.X782":
            this.CalculateNewhudx645();
            break;
        }
        if (!this.ModelProvider.BorHudFields.ContainsKey((object) sourceId) && !this.ModelProvider.SelHudFields.ContainsKey((object) sourceId))
          return;
        string[] strArray = (string[]) null;
        if (this.ModelProvider.BorHudFields.ContainsKey((object) sourceId))
          strArray = (string[]) this.ModelProvider.BorHudFields[(object) sourceId];
        else if (this.ModelProvider.SelHudFields.ContainsKey((object) sourceId))
          strArray = (string[]) this.ModelProvider.SelHudFields[(object) sourceId];
        if (strArray != null && strArray[2] != string.Empty)
          this.SyncBorSelFieldToGfe(strArray[0], strArray[1], strArray[2]);
        else if (strArray != null)
          this.SetVal(strArray[0], this.Val(strArray[1]));
        switch (sourceId)
        {
          case "NEWHUD.X1155":
            this.ModelProvider.SetCurrentNum("NEWHUD.X1209", this.ParseDouble(this.Val("NEWHUD.X1154")));
            break;
          case "NEWHUD.X1159":
            this.ModelProvider.SetCurrentNum("NEWHUD.X1211", this.ParseDouble(this.Val("NEWHUD.X1158")));
            break;
          case "NEWHUD.X1163":
            this.ModelProvider.SetCurrentNum("NEWHUD.X1213", this.ParseDouble(this.Val("NEWHUD.X1162")));
            break;
        }
        // ISSUE: reference to a compiler-generated method
        switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(sourceId))
        {
          case 34128109:
            if (!(sourceId == "NEWHUD.X1163"))
              return;
            break;
          case 505966770:
            if (!(sourceId == "436"))
              return;
            goto label_107;
          case 540507746:
            if (!(sourceId == "454"))
              return;
            if (this.ModelProvider.IsLocked("454"))
              this.ModelProvider.AddLock("NEWHUD.X770");
            this.SetVal("NEWHUD.X769", this.Val("388"));
            return;
          case 968485357:
            if (!(sourceId == "1847"))
              return;
            goto label_109;
          case 1078831804:
            if (!(sourceId == "337"))
              return;
            this.SyncBorSelFieldToGfe("NEWHUD.X661", "1050", "571");
            return;
          case 1812997008:
            if (!(sourceId == "NEWHUD.X1155"))
              return;
            break;
          case 2014328436:
            if (!(sourceId == "NEWHUD.X1159"))
              return;
            break;
          case 2040941840:
            if (!(sourceId == "647"))
              return;
            goto label_106;
          case 2292606125:
            if (!(sourceId == "648"))
              return;
            goto label_106;
          case 2425366825:
            if (!(sourceId == "1050"))
              return;
            this.SyncBorSelFieldToGfe("NEWHUD.X622", "337", "562");
            return;
          case 2542957253:
            if (!(sourceId == "1061"))
              return;
            goto label_107;
          case 3078132128:
            if (!(sourceId == "1637"))
              return;
            goto label_106;
          case 3199160424:
            if (!(sourceId == "593"))
              return;
            goto label_106;
          case 3232024114:
            if (!(sourceId == "559"))
              return;
            this.ModelProvider.SetCurrentNum("NEWHUD.X769", this.ParseDouble(this.Val("388")));
            if (!this.ModelProvider.IsLocked("454"))
              return;
            this.ModelProvider.AddLock("NEWHUD.X770");
            return;
          case 3278624913:
            if (!(sourceId == "1663"))
              return;
            this.SetVal("NEWHUD.X712", this.Val("1847"));
            this.SetVal("NEWHUD.X741", this.Val("NEWHUD.X734"));
            return;
          case 3316603757:
            if (!(sourceId == "594"))
              return;
            goto label_106;
          case 3637410166:
            if (!(sourceId == "NEWHUD.X734"))
              return;
            goto label_109;
          case 3721298261:
            if (!(sourceId == "NEWHUD.X731"))
              return;
            goto label_106;
          case 3739208713:
            if (!(sourceId == "NEWHUD.X788"))
              return;
            goto label_107;
          case 3890207284:
            if (!(sourceId == "NEWHUD.X787"))
              return;
            goto label_106;
          default:
            return;
        }
        if (strArray != null && this.ModelProvider.IsLocked(sourceId) && !this.ModelProvider.IsLocked(strArray[0]))
        {
          this.ModelProvider.AddLock(strArray[0]);
          return;
        }
        if (strArray == null || this.ModelProvider.IsLocked(sourceId) || !this.ModelProvider.IsLocked(strArray[0]))
          return;
        this.ModelProvider.RemoveLock(strArray[0]);
        return;
label_106:
        this.ModelProvider.SetCurrentNum("NEWHUD.X730", this.ParseDouble(this.Val("NEWHUD.X731")) + this.ParseDouble(this.Val("NEWHUD.X787")));
        this.ModelProvider.SetCurrentNum("NEWHUD.X606", this.ParseDouble(this.Val("648")) + this.ParseDouble(this.Val("594")));
        this.ModelProvider.SetCurrentNum("NEWHUD.X605", this.ParseDouble(this.Val("647")) + this.ParseDouble(this.Val("593")));
        return;
label_107:
        if (!string.IsNullOrEmpty(this.Val("NEWHUD.X15")) || this.ParseDouble(this.Val("NEWHUD.X788")) <= 0.0)
          return;
        this.ModelProvider.RemoveLock("NEWHUD.X13");
        this.ModelProvider.SetCurrentNum("NEWHUD.X13", 0.0);
        this.ModelProvider.RemoveLock("NEWHUD.X720");
        this.ModelProvider.SetCurrentNum("NEWHUD.X720", this.ParseDouble(this.Val("NEWHUD.X788")));
        return;
label_109:
        if (!string.IsNullOrEmpty(this.Val("NEWHUD.X712")) || !string.IsNullOrEmpty(this.Val("NEWHUD.X741")))
          return;
        this.SetVal("NEWHUD.X718", "");
      }
      else
      {
        if (sourceId != "copytohudgfe" && this._stopSyncItemization && this.LoanDisclosedChecker.IsLoanDisclosed(this._manualSyncItemization))
          return;
        this.CalculationObjects.GfeCalCalcPrepaid((string) null, (string) null);
        foreach (string[] strArray in HUDGFE2010Fields.HUD2010ToGFE2010FIELDMAP)
        {
          if (!blankCopyOnly || Math.Abs(Utils.ParseDouble((object) this.Val(strArray[0]))) <= 0.0)
          {
            if (strArray[1] == "NEWHUD.X645")
              this.CalculateNewhudx645();
            if (strArray[2] != string.Empty)
              this.SyncBorSelFieldToGfe(strArray[0], strArray[1], strArray[2]);
            else
              this.SetVal(strArray[0], this.Val(strArray[1]));
            if (strArray[1] == "454")
            {
              if (this.ModelProvider.IsLocked("454"))
              {
                this.ModelProvider.RemoveLock("NEWHUD.X770");
                this.ModelProvider.SetCurrentNum("NEWHUD.X770", this.ParseDouble(this.Val("454")) + this.ParseDouble(this.Val("559")));
                this.ModelProvider.AddLock("NEWHUD.X770");
              }
              else
                this.ModelProvider.RemoveLock("NEWHUD.X770");
              this.SetVal("NEWHUD.X769", this.Val("388"));
              if (!this.ModelProvider.IsLocked("NEWHUD.X770"))
                this.SetVal("NEWHUD.X770", this.Val("454"));
            }
          }
        }
        this.ModelProvider.FormCal((string) null, (string) null);
      }
    }

    public void PopulateHud1Page3Page()
    {
      int lineCount = 0;
      int lineChangeCount = 0;
      this.PopulateSection800(ref lineCount, ref lineChangeCount);
      this.PopulateSection900(ref lineCount, ref lineChangeCount);
      this.CalculateHud1Page3Section1100(ref lineCount, ref lineChangeCount);
      this.PopulateSection1300(ref lineCount, ref lineChangeCount);
      this.ClearUnusedFields(lineCount, lineChangeCount);
    }

    public void PopulateHud1Page3PredependentFields()
    {
      bool flag1 = false;
      bool flag2 = true;
      for (int index = 41; index <= 49; ++index)
      {
        if (this.Val("HUD01" + (object) index) != "")
        {
          flag2 = false;
          break;
        }
      }
      if (!flag2)
      {
        if (this.ParseDouble(this.Val("231")) > 0.0 && this.Val("HUD0141") != "")
        {
          this.SetVal("NEWHUD.X337", "Y");
          flag1 = true;
        }
        else if (this.ParseDouble(this.Val("231")) > 0.0)
          this.SetVal("NEWHUD.X337", "N");
        else
          this.SetVal("NEWHUD.X337", "");
        if (this.ParseDouble(this.Val("235")) > 0.0 && this.Val("HUD0144") != "")
        {
          this.SetVal("NEWHUD.X338", "Y");
          flag1 = true;
        }
        else if (this.ParseDouble(this.Val("235")) > 0.0)
          this.SetVal("NEWHUD.X338", "N");
        else
          this.SetVal("NEWHUD.X338", "");
        if (this.ParseDouble(this.Val("L268")) > 0.0 && this.Val("HUD0145") != "")
        {
          this.SetVal("NEWHUD.X1726", "Y");
          flag1 = true;
        }
        else if (this.ParseDouble(this.Val("L268")) > 0.0)
          this.SetVal("NEWHUD.X1726", "N");
        else
          this.SetVal("NEWHUD.X1726", "");
        if (this.ParseDouble(this.Val("230")) > 0.0 && this.Val("HUD0142") != "")
        {
          this.SetVal("NEWHUD.X339", "Y");
          flag1 = true;
        }
        else if (this.ParseDouble(this.Val("230")) > 0.0)
          this.SetVal("NEWHUD.X339", "N");
        else
          this.SetVal("NEWHUD.X339", "");
        if (this.ParseDouble(this.Val("232")) > 0.0 && this.Val("HUD0143") != "")
        {
          this.SetVal("NEWHUD.X1728", "Y");
          flag1 = true;
        }
        else if (this.ParseDouble(this.Val("232")) > 0.0)
          this.SetVal("NEWHUD.X1728", "N");
        else
          this.SetVal("NEWHUD.X1728", "");
        if (this.ParseDouble(this.Val("1630")) > 0.0 && this.Val("HUD0146") != "")
        {
          this.SetVal("NEWHUD.X340", "Y");
          flag1 = true;
        }
        else if (this.ParseDouble(this.Val("1630")) > 0.0)
          this.SetVal("NEWHUD.X340", "N");
        else
          this.SetVal("NEWHUD.X340", "");
        if (this.ParseDouble(this.Val("253")) > 0.0 && this.Val("HUD0147") != "")
        {
          this.SetVal("NEWHUD.X341", "Y");
          flag1 = true;
        }
        else if (this.ParseDouble(this.Val("253")) > 0.0)
          this.SetVal("NEWHUD.X341", "N");
        else
          this.SetVal("NEWHUD.X341", "");
        if (this.ParseDouble(this.Val("254")) > 0.0 && this.Val("HUD0148") != "")
        {
          this.SetVal("NEWHUD.X342", "Y");
          flag1 = true;
        }
        else if (this.ParseDouble(this.Val("254")) > 0.0)
          this.SetVal("NEWHUD.X342", "N");
        else
          this.SetVal("NEWHUD.X342", "");
        if (this.ParseDouble(this.Val("NEWHUD.X1707")) > 0.0 && this.Val("HUD0149") != "")
        {
          if (this.Val("NEWHUD.X357") != "Y")
            this.SetVal("NEWHUD.X343", "Y");
          else
            this.SetVal("NEWHUD.X343", "");
          flag1 = true;
        }
        else if (this.ParseDouble(this.Val("NEWHUD.X1707")) > 0.0)
          this.SetVal("NEWHUD.X343", "N");
        else
          this.SetVal("NEWHUD.X343", "");
        this.SetVal("NEWHUD2.X134", this.Val("NEWHUD.X337"));
        this.SetVal("NEWHUD2.X133", this.Val("NEWHUD.X339"));
        this.SetVal("NEWHUD2.X136", this.Val("NEWHUD.X338"));
        this.SetVal("NEWHUD2.X135", this.Val("NEWHUD.X1726"));
        this.SetVal("NEWHUD2.X137", this.Val("NEWHUD.X340"));
        this.SetVal("NEWHUD2.X138", this.Val("NEWHUD.X341"));
        this.SetVal("NEWHUD2.X139", this.Val("NEWHUD.X342"));
        this.SetVal("NEWHUD2.X140", this.Val("NEWHUD.X343"));
        this.SetVal("NEWHUD2.X4769", this.Val("NEWHUD.X1728"));
      }
      else
      {
        if (this.Val("NEWHUD2.X134") == "Y")
          this.SetVal("NEWHUD.X337", "Y");
        else if (this.FltVal("231") != 0.0)
          this.SetVal("NEWHUD.X337", "N");
        else
          this.SetVal("NEWHUD.X337", "");
        if (this.Val("NEWHUD2.X133") == "Y")
          this.SetVal("NEWHUD.X339", "Y");
        else if (this.FltVal("230") != 0.0)
          this.SetVal("NEWHUD.X339", "N");
        else
          this.SetVal("NEWHUD.X339", "");
        if (this.Val("NEWHUD2.X4769") == "Y")
          this.SetVal("NEWHUD.X1728", "Y");
        else if (this.FltVal("232") != 0.0)
          this.SetVal("NEWHUD.X1728", "N");
        else
          this.SetVal("NEWHUD.X1728", "");
        if (this.Val("NEWHUD2.X136") == "Y")
          this.SetVal("NEWHUD.X338", "Y");
        else if (this.FltVal("235") != 0.0)
          this.SetVal("NEWHUD.X338", "N");
        else
          this.SetVal("NEWHUD.X338", "");
        if (this.Val("NEWHUD2.X135") == "Y")
          this.SetVal("NEWHUD.X1726", "Y");
        else if (this.FltVal("L268") != 0.0)
          this.SetVal("NEWHUD.X1726", "N");
        else
          this.SetVal("NEWHUD.X1726", "");
        if (this.Val("NEWHUD2.X137") == "Y")
          this.SetVal("NEWHUD.X340", "Y");
        else if (this.FltVal("1630") != 0.0)
          this.SetVal("NEWHUD.X340", "N");
        else
          this.SetVal("NEWHUD.X340", "");
        if (this.Val("NEWHUD2.X138") == "Y")
          this.SetVal("NEWHUD.X341", "Y");
        else if (this.FltVal("253") != 0.0)
          this.SetVal("NEWHUD.X341", "N");
        else
          this.SetVal("NEWHUD.X341", "");
        if (this.Val("NEWHUD2.X139") == "Y")
          this.SetVal("NEWHUD.X342", "Y");
        else if (this.FltVal("254") != 0.0)
          this.SetVal("NEWHUD.X342", "N");
        else
          this.SetVal("NEWHUD.X342", "");
        if (this.Val("NEWHUD2.X140") == "Y")
          this.SetVal("NEWHUD.X343", "Y");
        else if (this.FltVal("NEWHUD.X1707") != 0.0)
          this.SetVal("NEWHUD.X343", "N");
        else
          this.SetVal("NEWHUD.X343", "");
      }
      this.SetVal("NEWHUD.X335", flag1 ? "Yes" : "No");
      if (!flag1)
      {
        this.SetVal("NEWHUD.X802", "");
        this.SetVal("NEWHUD.X950", "");
      }
      this.PopulateHud1Page3Page();
    }

    public void PopulateSection800(ref int lineCount, ref int lineChangeCount)
    {
      this.PopulateHudFieldValue(804, "Appraisal Fee", this.Val("617"), "NEWHUD.X609", "641", "581", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(805, "Credit Report", this.Val("624"), "NEWHUD.X610", "640", "580", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(806, "Tax Service", this.Val("L224"), "NEWHUD.X611", "336", "565", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(807, "Flood Certification", this.Val("NEWHUD.X399"), "NEWHUD.X612", "NEWHUD.X400", "NEWHUD.X781", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(808, this.Val("NEWHUD.X126"), this.Val("NEWHUD.X1050"), "NEWHUD.X662", "NEWHUD.X136", "NEWHUD.X147", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(809, this.Val("NEWHUD.X127"), this.Val("NEWHUD.X1051"), "NEWHUD.X663", "NEWHUD.X137", "NEWHUD.X148", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(810, this.Val("NEWHUD.X128"), this.Val("NEWHUD.X1052"), "NEWHUD.X664", "NEWHUD.X138", "NEWHUD.X149", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(811, this.Val("NEWHUD.X129"), this.Val("NEWHUD.X1053"), "NEWHUD.X665", "NEWHUD.X139", "NEWHUD.X150", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(812, this.Val("NEWHUD.X130"), this.Val("NEWHUD.X1054"), "NEWHUD.X666", "NEWHUD.X140", "NEWHUD.X151", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(813, this.Val("369"), this.Val("NEWHUD.X1055"), "NEWHUD.X667", "370", "574", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(814, this.Val("371"), this.Val("NEWHUD.X1056"), "NEWHUD.X668", "372", "575", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(815, this.Val("348"), this.Val("NEWHUD.X1057"), "NEWHUD.X669", "349", "96", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(816, this.Val("931"), this.Val("NEWHUD.X1058"), "NEWHUD.X670", "932", "1345", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(817, this.Val("1390"), this.Val("NEWHUD.X1059"), "NEWHUD.X671", "1009", "6", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(818, this.Val("NEWHUD.X1291"), this.Val("NEWHUD.X1292"), "NEWHUD.X1525", "NEWHUD.X1293", "NEWHUD.X1294", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(819, this.Val("NEWHUD.X1299"), this.Val("NEWHUD.X1300"), "NEWHUD.X1526", "NEWHUD.X1301", "NEWHUD.X1302", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(820, this.Val("NEWHUD.X1307"), this.Val("NEWHUD.X1308"), "NEWHUD.X1527", "NEWHUD.X1309", "NEWHUD.X1310", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(821, this.Val("NEWHUD.X1315"), this.Val("NEWHUD.X1316"), "NEWHUD.X1528", "NEWHUD.X1317", "NEWHUD.X1318", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(822, this.Val("NEWHUD.X1323"), this.Val("NEWHUD.X1324"), "NEWHUD.X1529", "NEWHUD.X1325", "NEWHUD.X1326", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(823, this.Val("NEWHUD.X1331"), this.Val("NEWHUD.X1332"), "NEWHUD.X1530", "NEWHUD.X1333", "NEWHUD.X1334", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(824, this.Val("NEWHUD.X1339"), this.Val("NEWHUD.X1340"), "NEWHUD.X1531", "NEWHUD.X1341", "NEWHUD.X1342", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(825, this.Val("NEWHUD.X1347"), this.Val("NEWHUD.X1348"), "NEWHUD.X1532", "NEWHUD.X1349", "NEWHUD.X1350", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(826, this.Val("NEWHUD.X1355"), this.Val("NEWHUD.X1356"), "NEWHUD.X1533", "NEWHUD.X1357", "NEWHUD.X1358", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(827, this.Val("NEWHUD.X1363"), this.Val("NEWHUD.X1364"), "NEWHUD.X1534", "NEWHUD.X1365", "NEWHUD.X1366", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(828, this.Val("NEWHUD.X1371"), this.Val("NEWHUD.X1372"), "NEWHUD.X1535", "NEWHUD.X1373", "NEWHUD.X1374", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(829, this.Val("NEWHUD.X1379"), this.Val("NEWHUD.X1380"), "NEWHUD.X1536", "NEWHUD.X1381", "NEWHUD.X1382", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(830, this.Val("NEWHUD.X1387"), this.Val("NEWHUD.X1388"), "NEWHUD.X1537", "NEWHUD.X1389", "NEWHUD.X1390", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(831, this.Val("NEWHUD.X1395"), this.Val("NEWHUD.X1396"), "NEWHUD.X1538", "NEWHUD.X1397", "NEWHUD.X1398", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(832, this.Val("NEWHUD.X1403"), this.Val("NEWHUD.X1404"), "NEWHUD.X1539", "NEWHUD.X1405", "NEWHUD.X1406", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(833, this.Val("NEWHUD.X1411"), this.Val("NEWHUD.X1412"), "NEWHUD.X1540", "NEWHUD.X1413", "NEWHUD.X1414", "", ref lineCount, ref lineChangeCount);
    }

    public void PopulateSection900(ref int lineCount, ref int lineChangeCount)
    {
      this.PopulateHudFieldValue(902, "Mortgage Insurance Premium", this.Val("L248"), "NEWHUD.X622", "337", "562", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(904, this.Val("NEWHUD.X582"), this.Val("NEWHUD.X1062"), "NEWHUD.X585", "NEWHUD.X591", "NEWHUD.X594", "NEWHUD.X597", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(905, "VA Funding Fee", this.Val("1956"), "NEWHUD.X661", "1050", "571", "", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(906, "Flood Insurance", this.Val("1500"), "NEWHUD.X586", "643", "579", "NEWHUD.X598", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(907, this.Val("L259"), this.Val("NEWHUD.X1063"), "NEWHUD.X587", "L260", "L261", "NEWHUD.X599", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(908, this.Val("1666"), this.Val("NEWHUD.X1064"), "NEWHUD.X588", "1667", "1668", "NEWHUD.X600", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(909, this.Val("NEWHUD.X583"), this.Val("NEWHUD.X1065"), "NEWHUD.X589", "NEWHUD.X592", "NEWHUD.X595", "NEWHUD.X601", ref lineCount, ref lineChangeCount);
    }

    public void CalculateHud1Page3Section1100(ref int lineCount, ref int lineChangeCount)
    {
      double borTotal10Percent = 0.0;
      double gfeTotal10Percent = 0.0;
      double borTotal = 0.0;
      double gfeTotal = 0.0;
      this.CalculateHud1Page3ForSection1100("NEWHUD.X954", "NEWHUD.X952", "NEWHUD.X953", "NEWHUD.X957", ref borTotal10Percent, ref gfeTotal10Percent, ref borTotal, ref gfeTotal);
      this.CalculateHud1Page3ForSection1100("NEWHUD.X963", "NEWHUD.X961", "NEWHUD.X962", "NEWHUD.X966", ref borTotal10Percent, ref gfeTotal10Percent, ref borTotal, ref gfeTotal);
      this.CalculateHud1Page3ForSection1100("NEWHUD.X972", "NEWHUD.X970", "NEWHUD.X971", "NEWHUD.X975", ref borTotal10Percent, ref gfeTotal10Percent, ref borTotal, ref gfeTotal);
      this.CalculateHud1Page3ForSection1100("NEWHUD.X981", "NEWHUD.X979", "NEWHUD.X980", "NEWHUD.X984", ref borTotal10Percent, ref gfeTotal10Percent, ref borTotal, ref gfeTotal);
      this.CalculateHud1Page3ForSection1100("NEWHUD.X990", "NEWHUD.X988", "NEWHUD.X989", "NEWHUD.X993", ref borTotal10Percent, ref gfeTotal10Percent, ref borTotal, ref gfeTotal);
      this.CalculateHud1Page3ForSection1100("NEWHUD.X999", "NEWHUD.X997", "NEWHUD.X998", "NEWHUD.X1002", ref borTotal10Percent, ref gfeTotal10Percent, ref borTotal, ref gfeTotal);
      this.CalculateHud1Page3ForSection1100("NEWHUD.X210", "NEWHUD.X645", "NEWHUD.X782", "NEWHUD.X646", ref borTotal10Percent, ref gfeTotal10Percent, ref borTotal, ref gfeTotal);
      this.CalculateHud1Page3ForSection1100("NEWHUD.X211", "NEWHUD.X639", "NEWHUD.X784", "NEWHUD.X573", ref borTotal10Percent, ref gfeTotal10Percent, ref borTotal, ref gfeTotal);
      this.CalculateHud1Page3ForSection1100("NEWHUD.X565", "NEWHUD.X215", "NEWHUD.X218", "NEWHUD.X576", ref borTotal10Percent, ref gfeTotal10Percent, ref borTotal, ref gfeTotal);
      this.CalculateHud1Page3ForSection1100("NEWHUD.X566", "NEWHUD.X216", "NEWHUD.X219", "NEWHUD.X577", ref borTotal10Percent, ref gfeTotal10Percent, ref borTotal, ref gfeTotal);
      this.CalculateHud1Page3ForSection1100("NEWHUD.X567", "1763", "1764", "NEWHUD.X578", ref borTotal10Percent, ref gfeTotal10Percent, ref borTotal, ref gfeTotal);
      this.CalculateHud1Page3ForSection1100("NEWHUD.X568", "1768", "1769", "NEWHUD.X579", ref borTotal10Percent, ref gfeTotal10Percent, ref borTotal, ref gfeTotal);
      this.CalculateHud1Page3ForSection1100("NEWHUD.X569", "1773", "1774", "NEWHUD.X580", ref borTotal10Percent, ref gfeTotal10Percent, ref borTotal, ref gfeTotal);
      this.CalculateHud1Page3ForSection1100("NEWHUD.X570", "1778", "1779", "NEWHUD.X581", ref borTotal10Percent, ref gfeTotal10Percent, ref borTotal, ref gfeTotal);
      if (borTotal > 0.0 || gfeTotal > 0.0)
        this.PopulateHud1Page3ForSection1100(gfeTotal, borTotal, ref lineCount, ref lineChangeCount, true);
      if (borTotal10Percent > 0.0 || gfeTotal10Percent > 0.0)
        this.PopulateHud1Page3ForSection1100(gfeTotal10Percent, borTotal10Percent, ref lineCount, ref lineChangeCount, false);
      this.PopulateHudFieldValue(1103, "Owner's Title Insurance", this.Val("NEWHUD.X204"), "NEWHUD.X39", "NEWHUD.X572", "NEWHUD.X783", "NEWHUD.X107", ref lineCount, ref lineChangeCount);
    }

    public void PopulateSection1300(ref int lineCount, ref int lineChangeCount)
    {
      this.PopulateHudFieldValue(1302, this.Val("NEWHUD.X251"), this.Val("NEWHUD.X1085"), "NEWHUD.X42", "NEWHUD.X254", "NEWHUD.X258", "NEWHUD.X108", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(1303, this.Val("650"), this.Val("NEWHUD.X1086"), "NEWHUD.X44", "644", "590", "NEWHUD.X109", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(1304, this.Val("651"), this.Val("NEWHUD.X1087"), "NEWHUD.X46", "645", "591", "NEWHUD.X110", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(1305, this.Val("40"), this.Val("NEWHUD.X1088"), "NEWHUD.X48", "41", "42", "NEWHUD.X111", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(1306, this.Val("43"), this.Val("NEWHUD.X1089"), "NEWHUD.X50", "44", "55", "NEWHUD.X112", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(1307, this.Val("1782"), this.Val("NEWHUD.X1090"), "NEWHUD.X52", "1783", "1784", "NEWHUD.X113", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(1308, this.Val("1787"), this.Val("NEWHUD.X1091"), "NEWHUD.X54", "1788", "1789", "NEWHUD.X114", ref lineCount, ref lineChangeCount);
      this.PopulateHudFieldValue(1309, this.Val("1792"), this.Val("NEWHUD.X1092"), "NEWHUD.X56", "1793", "1794", "NEWHUD.X115", ref lineCount, ref lineChangeCount);
    }

    public void SetTotalCharges()
    {
      double num1 = this.FltVal("NEWHUD.X214");
      double num2 = this.FltVal("NEWHUD.X336");
      for (int index = 0; index < HUDGFE2010Fields.HUD1PG3_10PERCENTSFIELDS.Length; ++index)
      {
        string[] strArray = (string[]) HUDGFE2010Fields.HUD1PG3_10PERCENTSFIELDS[index];
        num1 += this.FltVal(strArray[2]);
        num2 += this.FltVal(strArray[3]);
      }
      this.SetCurrentNum("NEWHUD.X312", num1);
      this.SetCurrentNum("NEWHUD.X313", num2);
      this.SetCurrentNum("NEWHUD.X314", num2 - num1);
      if (Math.Abs(num1) > 0.01)
        this.SetCurrentNum("NEWHUD.X315", Utils.ArithmeticRounding((num2 / num1 - 1.0) * 100.0, 2));
      else
        this.SetCurrentNum("NEWHUD.X315", 0.0);
    }

    public void ClearUnusedFields(int lineCount, int lineChangeCount)
    {
      for (int index = lineCount; index < HUDGFE2010Fields.HUD1PG3_10PERCENTSFIELDS.Length; ++index)
      {
        string[] strArray = (string[]) HUDGFE2010Fields.HUD1PG3_10PERCENTSFIELDS[index];
        this.SetVal(strArray[0], "");
        this.SetVal(strArray[1], "");
        this.SetVal(strArray[2], "");
        this.SetVal(strArray[3], "");
      }
      for (int index = lineChangeCount; index < HUDGFE2010Fields.HUD1PG3_CHANGEFIELDS.Length; ++index)
      {
        string[] strArray = (string[]) HUDGFE2010Fields.HUD1PG3_CHANGEFIELDS[index];
        this.SetVal(strArray[0], "");
        this.SetVal(strArray[1], "");
        this.SetVal(strArray[2], "");
        this.SetVal(strArray[3], "");
      }
    }

    public void calculateHUD1ToleranceLimits()
    {
      bool flag = false;
      foreach (string[] strArray in HUDGFE2010Fields.HUD1PG3_EXACTFIELDS)
      {
        Decimal num = Utils.ParseDecimal((object) this.Val(strArray[0]), 0M);
        if ((!(strArray[0] == "NEWHUD.X76") ? Utils.ParseDecimal((object) this.Val(strArray[1]), 0M) + Utils.ParseDecimal((object) this.Val(strArray[2]), 0M) : Utils.ParseDecimal((object) this.Val(strArray[1]), 0M)) > num)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        Decimal num1 = Utils.ParseDecimal((object) this.Val("NEWHUD.X312"), 0M);
        Decimal num2 = Utils.ParseDecimal((object) this.Val("NEWHUD.X313"), 0M);
        if (Utils.ParseDecimal((object) this.Val("NEWHUD.X315"), 0M) > 10M)
          flag = true;
        else if (num1 <= 0M && num2 > 0M)
          flag = true;
      }
      this.SetVal("3160", flag ? "Y" : "N");
    }

    private void PopulateHudFieldValue(
      int lineNo,
      string description,
      string toWhom,
      string gfeId,
      string borId,
      string selId,
      string borSelectId,
      ref int lineCount,
      ref int lineChangeCount)
    {
      double num1 = gfeId != string.Empty ? this.ParseDouble(this.Val(gfeId)) : 0.0;
      double num2 = this.ParseDouble(this.Val(borId));
      double num3 = selId == string.Empty ? 0.0 : this.ParseDouble(this.Val(selId));
      if (num1 <= 0.0 && num2 <= 0.0)
        return;
      string[] strArray;
      if (lineNo == 904 || lineNo >= 906 && lineNo <= 909 || borSelectId != string.Empty && this.Val(borSelectId) == "Y")
      {
        if (lineChangeCount >= HUDGFE2010Fields.HUD1PG3_CHANGEFIELDS.Length)
          return;
        strArray = (string[]) HUDGFE2010Fields.HUD1PG3_CHANGEFIELDS[lineChangeCount];
        ++lineChangeCount;
      }
      else
      {
        if (lineCount >= HUDGFE2010Fields.HUD1PG3_10PERCENTSFIELDS.Length)
          return;
        strArray = (string[]) HUDGFE2010Fields.HUD1PG3_10PERCENTSFIELDS[lineCount];
        ++lineCount;
      }
      if (strArray == null)
        return;
      this.SetVal(strArray[0], description.Trim() + (toWhom != string.Empty ? " to " + toWhom : ""));
      this.SetVal(strArray[1], string.Concat((object) lineNo));
      this.ModelProvider.SetCurrentNum(strArray[2], num1);
      if (num2 > 0.0 || num1 > 0.0)
      {
        if (Math.Abs(num2 - num1) <= 0.0 || lineNo == 834 || lineNo == 835 || lineNo == 1310 || lineNo == 1311)
          this.ModelProvider.SetCurrentNum(strArray[3], num2);
        else
          this.ModelProvider.SetCurrentNum(strArray[3], num2 + num3);
      }
      else
        this.ModelProvider.SetCurrentNum(strArray[3], 0.0);
    }

    private void CalculateHud1Page3ForSection1100(
      string gfeId,
      string borId,
      string selId,
      string borSelectId,
      ref double borTotal10Percent,
      ref double gfeTotal10Percent,
      ref double borTotal,
      ref double gfeTotal)
    {
      double num1 = gfeId != string.Empty ? this.ParseDouble(this.Val(gfeId)) : 0.0;
      double num2 = this.ParseDouble(this.Val(borId));
      double num3 = selId == string.Empty ? 0.0 : this.ParseDouble(this.Val(selId));
      if (num1 <= 0.0 && num2 <= 0.0)
        return;
      if (borSelectId != string.Empty && this.Val(borSelectId) == "Y")
      {
        gfeTotal += num1;
        if (num2 <= 0.0 && num1 <= 0.0)
          return;
        if (Math.Abs(num2 - num1) > 0.0)
          borTotal += num2 + num3;
        else
          borTotal += num2;
      }
      else
      {
        gfeTotal10Percent += num1;
        if (num2 <= 0.0 && num1 <= 0.0)
          return;
        if (Math.Abs(num2 - num1) > 0.0)
          borTotal10Percent += num2 + num3;
        else
          borTotal10Percent += num2;
      }
    }

    private void PopulateHud1Page3ForSection1100(
      double gfeValue,
      double hudValue,
      ref int lineCount,
      ref int lineChangeCount,
      bool borSelected)
    {
      string[] strArray;
      if (borSelected)
      {
        if (lineChangeCount >= HUDGFE2010Fields.HUD1PG3_CHANGEFIELDS.Length)
          return;
        strArray = (string[]) HUDGFE2010Fields.HUD1PG3_CHANGEFIELDS[lineChangeCount];
        ++lineChangeCount;
      }
      else
      {
        if (lineCount >= HUDGFE2010Fields.HUD1PG3_10PERCENTSFIELDS.Length)
          return;
        strArray = (string[]) HUDGFE2010Fields.HUD1PG3_10PERCENTSFIELDS[lineCount];
        ++lineCount;
      }
      if (strArray == null)
        return;
      this.SetVal(strArray[0], "Title Services and Lender's Title Insurance");
      this.SetVal(strArray[1], "1101");
      this.ModelProvider.SetCurrentNum(strArray[2], gfeValue);
      this.ModelProvider.SetCurrentNum(strArray[3], hudValue);
    }

    public void CopySection800(ref int count)
    {
      this.SyncFeeToHudgfe("NEWHUD.X662", this.Val("NEWHUD.X126"), this.Val("NEWHUD.X1050"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X663", this.Val("NEWHUD.X127"), this.Val("NEWHUD.X1051"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X664", this.Val("NEWHUD.X128"), this.Val("NEWHUD.X1052"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X665", this.Val("NEWHUD.X129"), this.Val("NEWHUD.X1053"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X666", this.Val("NEWHUD.X130"), this.Val("NEWHUD.X1054"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X667", this.Val("369"), this.Val("NEWHUD.X1055"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X668", this.Val("371"), this.Val("NEWHUD.X1056"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X669", this.Val("348"), this.Val("NEWHUD.X1057"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X670", this.Val("931"), this.Val("NEWHUD.X1058"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X671", this.Val("1390"), this.Val("NEWHUD.X1059"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X1525", this.Val("NEWHUD.X1291"), this.Val("NEWHUD.X1292"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X1526", this.Val("NEWHUD.X1299"), this.Val("NEWHUD.X1300"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X1527", this.Val("NEWHUD.X1307"), this.Val("NEWHUD.X1308"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X1528", this.Val("NEWHUD.X1315"), this.Val("NEWHUD.X1316"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X1529", this.Val("NEWHUD.X1323"), this.Val("NEWHUD.X1324"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X1530", this.Val("NEWHUD.X1331"), this.Val("NEWHUD.X1332"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X1531", this.Val("NEWHUD.X1339"), this.Val("NEWHUD.X1340"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X1532", this.Val("NEWHUD.X1347"), this.Val("NEWHUD.X1348"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X1533", this.Val("NEWHUD.X1355"), this.Val("NEWHUD.X1356"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X1534", this.Val("NEWHUD.X1363"), this.Val("NEWHUD.X1364"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X1535", this.Val("NEWHUD.X1371"), this.Val("NEWHUD.X1372"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X1536", this.Val("NEWHUD.X1379"), this.Val("NEWHUD.X1380"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X1537", this.Val("NEWHUD.X1387"), this.Val("NEWHUD.X1388"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X1538", this.Val("NEWHUD.X1395"), this.Val("NEWHUD.X1396"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X1539", this.Val("NEWHUD.X1403"), this.Val("NEWHUD.X1404"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      this.SyncFeeToHudgfe("NEWHUD.X1540", this.Val("NEWHUD.X1411"), this.Val("NEWHUD.X1412"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
      if (count >= HUDGFE2010Fields.HUDGFE_FIELDS_Block3.Length)
        return;
      this.SyncFeeToHudgfe("NEWHUD.X661", "VA Funding Fee", this.Val("1956"), "", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block3[count]);
    }

    public void CopySection900(ref int count)
    {
      this.SyncFeeToHudgfe("NEWHUD.X650", "Homeowner's Insurance", this.Val("L252"), "NEWHUD.X651", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block11[count]);
      this.SyncFeeToHudgfe("NEWHUD.X585", this.Val("NEWHUD.X582"), this.Val("NEWHUD.X1062"), "NEWHUD.X597", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block11[count]);
      this.SyncFeeToHudgfe("NEWHUD.X586", "Flood Insurance", this.Val("1500"), "NEWHUD.X598", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block11[count]);
      this.SyncFeeToHudgfe("NEWHUD.X587", this.Val("L259"), this.Val("NEWHUD.X1063"), "NEWHUD.X599", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block11[count]);
      this.SyncFeeToHudgfe("NEWHUD.X588", this.Val("1666"), this.Val("NEWHUD.X1064"), "NEWHUD.X600", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block11[count]);
      this.SyncFeeToHudgfe("NEWHUD.X589", this.Val("NEWHUD.X583"), this.Val("NEWHUD.X1065"), "NEWHUD.X601", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block11[count]);
    }

    public void CopySection1100(ref int count)
    {
      this.SyncFeeToHudgfe("NEWHUD.X954", this.Val("NEWHUD.X951"), this.Val("NEWHUD.X1070"), "NEWHUD.X957", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block4[count]);
      this.SyncFeeToHudgfe("NEWHUD.X963", this.Val("NEWHUD.X960"), this.Val("NEWHUD.X1071"), "NEWHUD.X966", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block4[count]);
      this.SyncFeeToHudgfe("NEWHUD.X972", this.Val("NEWHUD.X969"), this.Val("NEWHUD.X1072"), "NEWHUD.X975", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block4[count]);
      this.SyncFeeToHudgfe("NEWHUD.X981", this.Val("NEWHUD.X978"), this.Val("NEWHUD.X1073"), "NEWHUD.X984", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block4[count]);
      this.SyncFeeToHudgfe("NEWHUD.X990", this.Val("NEWHUD.X987"), this.Val("NEWHUD.X1074"), "NEWHUD.X993", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block4[count]);
      this.SyncFeeToHudgfe("NEWHUD.X999", this.Val("NEWHUD.X996"), this.Val("NEWHUD.X1075"), "NEWHUD.X1002", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block4[count]);
      this.SyncFeeToHudgfe("NEWHUD.X210", "Settlement or Closing Fee", this.Val("NEWHUD.X203"), "NEWHUD.X646", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block4[count]);
      this.SyncFeeToHudgfe("NEWHUD.X211", "Lender's Title Insurance", this.Val("NEWHUD.X205"), "NEWHUD.X573", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block4[count]);
      this.SyncFeeToHudgfe("NEWHUD.X565", this.Val("NEWHUD.X208"), this.Val("NEWHUD.X1076"), "NEWHUD.X576", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block4[count]);
      this.SyncFeeToHudgfe("NEWHUD.X566", this.Val("NEWHUD.X209"), this.Val("NEWHUD.X1077"), "NEWHUD.X577", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block4[count]);
      this.SyncFeeToHudgfe("NEWHUD.X567", this.Val("1762"), this.Val("NEWHUD.X1078"), "NEWHUD.X578", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block4[count]);
      this.SyncFeeToHudgfe("NEWHUD.X568", this.Val("1767"), this.Val("NEWHUD.X1079"), "NEWHUD.X579", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block4[count]);
      this.SyncFeeToHudgfe("NEWHUD.X569", this.Val("1772"), this.Val("NEWHUD.X1080"), "NEWHUD.X580", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block4[count]);
      this.SyncFeeToHudgfe("NEWHUD.X570", this.Val("1777"), this.Val("NEWHUD.X1081"), "NEWHUD.X581", ref count, (string[]) HUDGFE2010Fields.HUDGFE_FIELDS_Block4[count]);
    }

    private void SyncFeeToHudgfe(
      string amtId,
      string description,
      string toWhom,
      string borSelectId,
      ref int count,
      string[] targetFields)
    {
      double num = this.ParseDouble(this.Val(amtId));
      if (this.Val(amtId) == string.Empty)
        return;
      this.SetVal(targetFields[0], description + (toWhom != string.Empty ? " to " + toWhom : ""));
      if (Math.Abs(num) <= 0.0 && this.Val(amtId) != string.Empty)
        this.SetVal(targetFields[1], this.Val(amtId));
      else
        this.ModelProvider.SetCurrentNum(targetFields[1], num);
      if (borSelectId != string.Empty && targetFields[2] != string.Empty)
        this.SetVal(targetFields[2], this.Val(borSelectId));
      ++count;
    }

    private void SyncBorSelFieldToGfe(string gfeId, string borId, string selId)
    {
      if (EncompassFields.IsNumeric(borId))
      {
        double num1 = this.ParseDouble(this.Val(borId));
        double num2 = selId != string.Empty ? this.ParseDouble(this.Val(selId)) : 0.0;
        if (Math.Abs(num1) > 0.0 || Math.Abs(num2) > 0.0)
          this.ModelProvider.SetCurrentNum(gfeId, num1 + num2);
        else if (this.Val(borId) != string.Empty)
          this.ModelProvider.SetCurrentNum(gfeId, 0.0, true);
        else
          this.ModelProvider.SetCurrentNum(gfeId, 0.0);
      }
      else if (borId == "NEWHUD.X572")
      {
        double num = this.ParseDouble(this.Val(borId)) + this.ParseDouble(this.Val(selId));
        if (Math.Abs(num) > 0.0)
          this.SetVal(gfeId, num.ToString("N2"));
        else if (this.Val(borId) != string.Empty)
          this.SetVal(gfeId, this.Val(borId));
        else if (this.Val(selId) != string.Empty)
        {
          this.SetVal(gfeId, this.Val(selId));
        }
        else
        {
          if (!(this.Val(borId) == string.Empty) || !(this.Val(gfeId) != "0.0"))
            return;
          this.SetVal(gfeId, this.Val(borId));
        }
      }
      else
        this.SetVal(gfeId, this.Val(borId));
    }

    private void CalculateNewhudx645()
    {
      if (this.Val("1172") == "VA" && !Utils.CheckIf2015RespaTila(this.Val("3969")))
      {
        for (int index = 809; index <= 818; ++index)
          this.SetVal("NEWHUD.X" + (object) index, "");
      }
      double num = this.ParseDouble(this.Val("NEWHUD.X808")) + this.ParseDouble(this.Val("NEWHUD.X810")) + this.ParseDouble(this.Val("NEWHUD.X812")) + this.ParseDouble(this.Val("NEWHUD.X814")) + this.ParseDouble(this.Val("NEWHUD.X816")) + this.ParseDouble(this.Val("NEWHUD.X818"));
      if (this.Val("14") == "CA" && this.UseNewGfeHud && this.CalculationObjects.CurrentFormId == "REGZGFE_2010")
        this.CalculationObjects.MldsCalCopyGfetoMlds("NEWHUD.X782");
      this.ModelProvider.SetCurrentNum("NEWHUD.X645", num);
    }

    private bool IsLoanDisclosed() => this.IsLoanDisclosed(this._manualSyncItemization);
  }
}

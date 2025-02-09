// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.STREAMLINED1003InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class STREAMLINED1003InputHandler : InputHandlerBase
  {
    private FieldLock fl_26;
    private DropdownBox f424_2009;
    private EllieMae.Encompass.Forms.Panel pnl2009;
    private EllieMae.Encompass.Forms.Panel pnl2020;
    private EllieMae.Encompass.Forms.Panel pnlBorBuydown;
    private EllieMae.Encompass.Forms.Panel pnlNonBorBuydown;
    private List<RuntimeControl> panel_Exp_Controls;
    private EllieMae.Encompass.Forms.Label labelFormName;
    private bool isUsedForTemplateFMAdditionalDataForm;
    private GovernmentInfoInputHandler governmentInfoInputHandler;
    private static ArrayList ynList = new ArrayList();

    public STREAMLINED1003InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public STREAMLINED1003InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public STREAMLINED1003InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public STREAMLINED1003InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      try
      {
        if (this.currentForm.Name == "Fannie Mae Additional Data" && this.loan.IsTemplate)
          this.isUsedForTemplateFMAdditionalDataForm = true;
        if (this.governmentInfoInputHandler == null)
          this.governmentInfoInputHandler = new GovernmentInfoInputHandler(this.currentForm, this.inputData, this.ToString());
        this.labelFormName = this.currentForm.FindControl("formLabel") as EllieMae.Encompass.Forms.Label;
        if (!this.isUsedForTemplateFMAdditionalDataForm && this.GetField("1825") == "2009" && this.labelFormName != null && !this.labelFormName.Text.Equals("GSEAdditionalProviderData"))
        {
          if (this.fl_26 == null)
            this.fl_26 = (FieldLock) this.currentForm.FindControl("FieldLock16");
          this.f424_2009 = (DropdownBox) this.currentForm.FindControl("DropdownBox10");
          this.pnlBorBuydown = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlBorBuydown");
          this.pnlNonBorBuydown = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlNonBorBuydown");
          this.panel_Exp_Controls = SharedURLAUIHandler.GetControls(this.currentForm, "panel_Exp_2009", "panel_Exp_2020");
        }
        if (this.pnl2009 == null)
          this.pnl2009 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnl2009");
        if (this.pnl2020 == null)
          this.pnl2020 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnl2020");
        if (this.pnl2009 == null || this.pnl2020 == null)
          return;
        if (this.GetField("1825") == "2020")
          this.pnl2009.Visible = !(this.pnl2020.Visible = true);
        else
          this.pnl2009.Visible = !(this.pnl2020.Visible = false);
      }
      catch (Exception ex)
      {
      }
    }

    static STREAMLINED1003InputHandler()
    {
      string[] strArray = new string[26]
      {
        "169",
        "175",
        "265",
        "266",
        "170",
        "176",
        "172",
        "178",
        "1057",
        "1197",
        "463",
        "464",
        "173",
        "179",
        "174",
        "180",
        "171",
        "177",
        "965",
        "985",
        "466",
        "467",
        "418",
        "1343",
        "403",
        "1108"
      };
      foreach (object obj in strArray)
        STREAMLINED1003InputHandler.ynList.Add(obj);
    }

    protected override string FormatValue(string fieldId, string value)
    {
      if (fieldId == "144" || fieldId == "147" || fieldId == "150")
      {
        string str = value.ToUpper();
        if (str.Length > 1)
          str = str.Substring(0, 1);
        if (str != "C" && str != "B")
          str = string.Empty;
        return str;
      }
      if (STREAMLINED1003InputHandler.ynList.Contains((object) fieldId))
      {
        string str = "";
        if (value.Length > 0)
        {
          str = value.Substring(0, 1).ToUpper();
          if (str != "Y" && str != "N")
            str = string.Empty;
        }
        return str;
      }
      switch (fieldId)
      {
        case "981":
        case "1015":
          string str1 = value.ToUpper();
          if (str1.Length == 1)
          {
            if (str1 != "P" && str1 != "S" && str1 != "I")
              str1 = string.Empty;
          }
          else if (str1.Length >= 2)
          {
            str1 = str1.Substring(0, 2);
            if (str1 != "PR" && str1 != "SH" && str1 != "IP")
              str1 = string.Empty;
          }
          return str1;
        case "1069":
        case "1070":
          string str2 = value.ToUpper();
          if (str2.Length == 1)
          {
            if (str2 != "S" && str2 != "O")
              str2 = string.Empty;
          }
          else if (str2.Length >= 2)
          {
            str2 = str2.Substring(0, 2);
            if (str2[0] == 'S')
            {
              if (str2[1] != 'P')
                str2 = "S";
            }
            else
              str2 = "O";
          }
          return str2;
        default:
          return base.FormatValue(fieldId, value);
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id == "981" || id == "1015")
      {
        if (val.Length < 2 && val != string.Empty && val != null)
        {
          this.CurrentValue = string.Empty;
          return;
        }
        val = !(val == "PR") ? (!(val == "SH") ? (!(val == "IP") ? string.Empty : "Investment") : "SecondaryResidence") : "PrimaryResidence";
      }
      else if (id == "1069" || id == "1070")
      {
        switch (val)
        {
          case "S":
            val = "Sole";
            break;
          case "SP":
            val = "JointWithSpouse";
            break;
          case "O":
            val = "JointWithOtherThanSpouse";
            break;
          default:
            val = string.Empty;
            break;
        }
      }
      else if (id == "418" || id == "1343" || id == "403" || id == "1108")
      {
        switch (val)
        {
          case "Y":
            val = "Yes";
            break;
          case "N":
            val = "No";
            break;
        }
      }
      else if (id == "470" && val != "Other")
        this.loan.SetField("1136", string.Empty);
      else if (id == "477" && val != "Other")
        this.loan.SetField("1300", string.Empty);
      else if (id == "188" && val == "Y")
      {
        this.loan.SetField("470", string.Empty);
        this.loan.SetField("1136", string.Empty);
        this.loan.SetField("471", string.Empty);
        for (int index = 1523; index <= 1530; ++index)
          this.loan.SetField(index.ToString(), string.Empty);
      }
      else if (id == "189" && val == "Y")
      {
        this.loan.SetField("477", string.Empty);
        this.loan.SetField("1300", string.Empty);
        this.loan.SetField("478", string.Empty);
        for (int index = 1531; index <= 1538; ++index)
          this.loan.SetField(index.ToString(), string.Empty);
      }
      else if (id == "991" && val == "N")
        val = string.Empty;
      else if (id == "608" && val != "GraduatedPaymentMortgage")
      {
        if (val != "OtherAmortizationType")
          base.UpdateFieldValue("994", string.Empty);
        base.UpdateFieldValue("1266", string.Empty);
        base.UpdateFieldValue("1267", string.Empty);
      }
      switch (id)
      {
        case "122":
        case "123":
        case "124":
        case "125":
        case "1405":
        case "230":
        case "232":
        case "233":
          double num = this.ToDouble(val);
          if (num != 0.0)
          {
            val = num.ToString("N2");
            break;
          }
          break;
      }
      base.UpdateFieldValue(id, val);
      if (id == "MORNET.X83" && val != "Y")
        base.UpdateFieldValue("MORNET.X84", "");
      if (id == "MORNET.X89" && val != "Y")
        base.UpdateFieldValue("MORNET.X90", "");
      this.RefreshContents();
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      if (!this.isUsedForTemplateFMAdditionalDataForm && this.GetField("1825") == "2009" && this.labelFormName != null && !this.labelFormName.Text.Equals("GSEAdditionalProviderData"))
      {
        this.governmentInfoInputHandler.SetLayout((InputHandlerBase) this);
        bool flag1 = this.inputData.GetField("CASASRN.X141") == "Borrower";
        bool flag2 = this.inputData.GetField("COMPLIANCEVERSION.CASASRNX141") == "Y" || this.inputData.GetField("COMPLIANCEVERSION.NEWBUYDOWNENABLED") != "Y";
        if (this.pnlNonBorBuydown != null)
          this.pnlNonBorBuydown.Visible = !flag1 && !flag2;
        if (this.pnlBorBuydown != null)
          this.pnlBorBuydown.Visible = flag1 | flag2;
      }
      if (this.isUsedForTemplateFMAdditionalDataForm)
        SharedURLAUIHandler.DisplayControls(true, this.panel_Exp_Controls);
      else
        SharedURLAUIHandler.DisplayControls(this.loan.Use2020URLA, this.panel_Exp_Controls);
    }

    protected override string GetFieldValue(string id, FieldSource source)
    {
      switch (id)
      {
        case "981":
        case "1015":
          string fieldValue1;
          switch (this.loan.GetField(id))
          {
            case "PrimaryResidence":
              fieldValue1 = "PR";
              break;
            case "SecondaryResidence":
              fieldValue1 = "SH";
              break;
            case "Investment":
              fieldValue1 = "IP";
              break;
            default:
              fieldValue1 = string.Empty;
              break;
          }
          return fieldValue1;
        case "1069":
        case "1070":
          string fieldValue2;
          switch (this.loan.GetField(id))
          {
            case "Sole":
              fieldValue2 = "S";
              break;
            case "JointWithSpouse":
              fieldValue2 = "SP";
              break;
            case "JointWithOtherThanSpouse":
              fieldValue2 = "O";
              break;
            default:
              fieldValue2 = string.Empty;
              break;
          }
          return fieldValue2;
        case "418":
        case "1343":
        case "403":
        case "1108":
          string fieldValue3;
          switch (this.loan.GetField(id))
          {
            case "Yes":
              fieldValue3 = "Y";
              break;
            case "No":
              fieldValue3 = "N";
              break;
            default:
              fieldValue3 = string.Empty;
              break;
          }
          return fieldValue3;
        default:
          return base.GetFieldValue(id, source);
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState defaultValue = this.getControlState(ctrl, id, ControlState.Enabled);
      string empty = string.Empty;
      switch (id)
      {
        case "101":
        case "102":
        case "103":
        case "104":
        case "1063":
        case "107":
        case "108":
        case "11":
        case "110":
        case "111":
        case "112":
        case "113":
        case "116":
        case "117":
        case "1416":
        case "1519":
        case "FE0104":
        case "FE0204":
        case "FR0104":
        case "FR0204":
          return defaultValue;
        case "1034":
          if (this.loan.GetField("1066") != "Leasehold")
          {
            defaultValue = ControlState.Disabled;
            this.SetControlState("Calendar4", false);
            goto case "101";
          }
          else
          {
            this.SetControlState("Calendar4", true);
            goto case "101";
          }
        case "1134":
          defaultValue = !(this.loan.GetField("19") == "Purchase") ? ControlState.Enabled : ControlState.Disabled;
          goto case "101";
        case "1136":
          if (this.loan.GetField("470") != "Other")
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "1266":
        case "1267":
          if (this.loan.GetField("608") != "GraduatedPaymentMortgage")
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "1300":
          if (this.loan.GetField("477") != "Other")
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "140":
          defaultValue = this.GetSpecialControlStatus(id, defaultValue);
          goto case "101";
        case "1417":
        case "1418":
        case "1419":
          defaultValue = !(this.loan.GetField("1819") == "Y") ? ControlState.Enabled : ControlState.Disabled;
          goto case "101";
        case "1520":
        case "1521":
        case "1522":
          defaultValue = !(this.loan.GetField("1820") == "Y") ? ControlState.Enabled : ControlState.Disabled;
          goto case "101";
        case "1524":
        case "1525":
        case "1526":
        case "1527":
        case "1528":
        case "1529":
        case "4126":
        case "4128":
        case "4130":
        case "4148":
        case "4149":
        case "4150":
        case "4151":
        case "4152":
        case "4153":
        case "4154":
        case "4155":
        case "4156":
        case "4157":
        case "4158":
        case "4252":
          bool flag1 = this.GetField("1524") == "" && this.GetField("1525") == "" && this.GetField("1526") == "" && this.GetField("1527") == "" && this.GetField("1528") == "" && this.GetField("4148") == "" && this.GetField("4149") == "" && this.GetField("4150") == "" && this.GetField("4151") == "" && this.GetField("4152") == "" && this.GetField("4153") == "" && this.GetField("4154") == "" && this.GetField("4155") == "" && this.GetField("4156") == "" && this.GetField("4157") == "" && this.GetField("4158") == "" && this.GetField("4252") == "" && this.GetField("4126") == "" && this.GetField("4128") == "" && this.GetField("4130") == "";
          bool flag2 = this.GetField("1524") != "Y" && this.GetField("1525") != "Y" && this.GetField("1526") != "Y" && this.GetField("1527") != "Y" && this.GetField("1528") != "Y" && this.GetField("4148") != "Y" && this.GetField("4149") != "Y" && this.GetField("4150") != "Y" && this.GetField("4151") != "Y" && this.GetField("4152") != "Y" && this.GetField("4153") != "Y" && this.GetField("4154") != "Y" && this.GetField("4155") != "Y" && this.GetField("4156") != "Y" && this.GetField("4157") != "Y" && this.GetField("4158") != "Y" && this.GetField("4252") != "Y" && this.GetField("4126") == "" && this.GetField("4128") == "" && this.GetField("4130") == "";
          if (this.loan.Settings.HMDAInfo != null && !this.loan.Settings.HMDAInfo.HMDAShowDemographicInfo && this.GetField("1393") == "Loan purchased by your institution")
          {
            if (flag1)
            {
              this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "CheckBox45", false);
              if (this.GetField("4244") != "")
                this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "CheckBox45", true);
            }
            else if (flag2)
              this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "CheckBox45", true);
            else
              this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "CheckBox45", false);
          }
          else if (flag2)
            this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "CheckBox45", true);
          else
            this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "CheckBox45", false);
          defaultValue = ControlState.Default;
          goto case "101";
        case "1531":
          if (this.GetField("3840") != "Y")
            this.SetControlState("panelCoBorrowerEthnicity", true);
          else
            this.SetControlState("panelCoBorrowerEthnicity", false);
          defaultValue = ControlState.Default;
          goto case "101";
        case "1532":
        case "1533":
        case "1534":
        case "1535":
        case "1536":
        case "1537":
        case "4137":
        case "4139":
        case "4141":
        case "4163":
        case "4164":
        case "4165":
        case "4166":
        case "4167":
        case "4168":
        case "4169":
        case "4170":
        case "4171":
        case "4172":
        case "4173":
        case "4253":
          bool flag3 = this.GetField("1532") == "" && this.GetField("1533") == "" && this.GetField("1534") == "" && this.GetField("1535") == "" && this.GetField("1536") == "" && this.GetField("4163") == "" && this.GetField("4164") == "" && this.GetField("4165") == "" && this.GetField("4166") == "" && this.GetField("4167") == "" && this.GetField("4168") == "" && this.GetField("4169") == "" && this.GetField("4170") == "" && this.GetField("4171") == "" && this.GetField("4172") == "" && this.GetField("4173") == "" && this.GetField("4253") == "" && this.GetField("4137") == "" && this.GetField("4139") == "" && this.GetField("4141") == "";
          bool flag4 = this.GetField("1532") != "Y" && this.GetField("1533") != "Y" && this.GetField("1534") != "Y" && this.GetField("1535") != "Y" && this.GetField("1536") != "Y" && this.GetField("4163") != "Y" && this.GetField("4164") != "Y" && this.GetField("4165") != "Y" && this.GetField("4166") != "Y" && this.GetField("4167") != "Y" && this.GetField("4168") != "Y" && this.GetField("4169") != "Y" && this.GetField("4170") != "Y" && this.GetField("4171") != "Y" && this.GetField("4172") != "Y" && this.GetField("4173") != "Y" && this.GetField("4253") != "Y" && this.GetField("4137") == "" && this.GetField("4139") == "" && this.GetField("4141") == "";
          if (this.loan.Settings.HMDAInfo != null && !this.loan.Settings.HMDAInfo.HMDAShowDemographicInfo && this.GetField("1393") == "Loan purchased by your institution")
          {
            if (flag3)
            {
              this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "CheckBox56", false);
              if (this.GetField("4247") != "")
                this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "CheckBox56", true);
            }
            else if (flag4)
              this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "CheckBox56", true);
            else
              this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "CheckBox56", false);
          }
          else if (flag4)
            this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "CheckBox56", true);
          else
            this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "CheckBox56", false);
          defaultValue = ControlState.Default;
          goto case "101";
        case "1stmor":
          string field1 = this.loan.GetField("420");
          if (!(field1 == "SecondLien") && !(field1 == "Other"))
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "24":
        case "25":
        case "29":
        case "MORNET.X40":
          string field2 = this.loan.GetField("19");
          if (field2.IndexOf("Refinance") < 0 && field2.IndexOf("Construction") < 0)
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "26":
          string field3 = this.loan.GetField("19");
          if (field3.IndexOf("Refinance") < 0 && field3.IndexOf("Construction") < 0)
          {
            defaultValue = ControlState.Disabled;
            this.fl_26.Enabled = false;
          }
          if (field3.IndexOf("Refinance") >= 0 || field3.IndexOf("Construction") >= 0)
          {
            this.fl_26.Enabled = true;
            if (!this.fl_26.Locked)
            {
              defaultValue = ControlState.Disabled;
              goto case "101";
            }
            else
              goto case "101";
          }
          else
            goto case "101";
        case "299":
          defaultValue = this.loan.GetField("19") == "Purchase" || this.loan.GetField("19") == "Other" ? ControlState.Disabled : ControlState.Enabled;
          goto case "101";
        case "4131":
          bool enabled1 = !(this.GetField("4131") == "FaceToFace");
          this.SetControlState("chkCoBorEthInfoNotProvided", enabled1);
          this.SetControlState("chkCoBorRaceInfoNotProvided", enabled1);
          this.SetControlState("chkCoBorSexInfoNotProvided", enabled1);
          defaultValue = ControlState.Default;
          goto case "101";
        case "4136":
        case "4159":
        case "4160":
        case "4161":
        case "4162":
        case "4206":
        case "4213":
        case "4214":
          bool flag5 = this.GetField("4206") == "" && this.GetField("4213") == "" && this.GetField("4214") == "" && this.GetField("4159") == "" && this.GetField("4160") == "" && this.GetField("4161") == "" && this.GetField("4162") == "" && this.GetField("4136") == "";
          bool flag6 = this.GetField("4206") != "Y" && this.GetField("4213") != "Y" && this.GetField("4214") != "Y" && this.GetField("4159") != "Y" && this.GetField("4160") != "Y" && this.GetField("4161") != "Y" && this.GetField("4162") != "Y" && this.GetField("4136") == "";
          if (this.loan.Settings.HMDAInfo != null && !this.loan.Settings.HMDAInfo.HMDAShowDemographicInfo && this.GetField("1393") == "Loan purchased by your institution")
          {
            if (flag5)
            {
              this.SetControlState("chkEthnicityCoBorrNA", false);
              if (this.GetField("4246") != "")
                this.SetControlState("chkEthnicityCoBorrNA", true);
            }
            else if (flag6)
              this.SetControlState("chkEthnicityCoBorrNA", true);
            else
              this.SetControlState("chkEthnicityCoBorrNA", false);
          }
          else if (flag6)
            this.SetControlState("chkEthnicityCoBorrNA", true);
          else
            this.SetControlState("chkEthnicityCoBorrNA", false);
          defaultValue = ControlState.Default;
          goto case "101";
        case "4143":
          bool enabled2 = !(this.GetField("4143") == "FaceToFace");
          this.SetControlState("chkBorEthInfoNotProvided", enabled2);
          this.SetControlState("chkBorRaceInfoNotProvided", enabled2);
          this.SetControlState("chkBorSexInfoNotProvided", enabled2);
          defaultValue = ControlState.Default;
          goto case "101";
        case "4144":
        case "4145":
        case "4146":
        case "4147":
        case "4205":
        case "4210":
        case "4211":
          bool flag7 = this.GetField("4205") == "" && this.GetField("4210") == "" && this.GetField("4211") == "" && this.GetField("4144") == "" && this.GetField("4145") == "" && this.GetField("4146") == "" && this.GetField("4147") == "" && this.GetField("4211") == "";
          bool flag8 = this.GetField("4205") != "Y" && this.GetField("4210") != "Y" && this.GetField("4211") != "Y" && this.GetField("4144") != "Y" && this.GetField("4145") != "Y" && this.GetField("4146") != "Y" && this.GetField("4147") != "Y" && this.GetField("4125") == "";
          if (this.loan.Settings.HMDAInfo != null && !this.loan.Settings.HMDAInfo.HMDAShowDemographicInfo && this.GetField("1393") == "Loan purchased by your institution")
          {
            if (flag7)
            {
              this.SetControlState("chkEthnicityBorrNA", false);
              if (this.GetField("4243") != "")
                this.SetControlState("chkEthnicityBorrNA", true);
            }
            else if (flag8)
              this.SetControlState("chkEthnicityBorrNA", true);
            else
              this.SetControlState("chkEthnicityBorrNA", false);
          }
          else if (flag8)
            this.SetControlState("chkEthnicityBorrNA", true);
          else
            this.SetControlState("chkEthnicityBorrNA", false);
          defaultValue = ControlState.Default;
          goto case "101";
        case "4193":
        case "4194":
        case "4195":
          bool flag9 = this.GetField("4193") == "" && this.GetField("4194") == "" && this.GetField("4195") == "";
          bool flag10 = this.GetField("4193") != "Y" && this.GetField("4194") != "Y" && this.GetField("4195") != "Y";
          if (this.loan.Settings.HMDAInfo != null && !this.loan.Settings.HMDAInfo.HMDAShowDemographicInfo && this.GetField("1393") == "Loan purchased by your institution")
          {
            if (flag9)
            {
              this.SetControlState("chkborsexnotapplicable", false);
              if (this.GetField("4245") != "")
              {
                this.SetControlState("chkborsexnotapplicable", true);
                goto case "101";
              }
              else
                goto case "101";
            }
            else if (flag10)
            {
              this.SetControlState("chkborsexnotapplicable", true);
              goto case "101";
            }
            else
            {
              this.SetControlState("chkborsexnotapplicable", false);
              goto case "101";
            }
          }
          else if (flag10)
          {
            this.SetControlState("chkborsexnotapplicable", true);
            goto case "101";
          }
          else
          {
            this.SetControlState("chkborsexnotapplicable", false);
            goto case "101";
          }
        case "4197":
        case "4198":
        case "4199":
          bool flag11 = this.GetField("4197") == "" && this.GetField("4198") == "" && this.GetField("4199") == "";
          bool flag12 = this.GetField("4197") != "Y" && this.GetField("4198") != "Y" && this.GetField("4199") != "Y";
          if (this.loan.Settings.HMDAInfo != null && !this.loan.Settings.HMDAInfo.HMDAShowDemographicInfo && this.GetField("1393") == "Loan purchased by your institution")
          {
            if (flag11)
            {
              this.SetControlState("chkcoborsexnotapplicable", false);
              if (this.GetField("4248") != "")
                this.SetControlState("chkcoborsexnotapplicable", true);
            }
            else if (flag12)
              this.SetControlState("chkcoborsexnotapplicable", true);
            else
              this.SetControlState("chkcoborsexnotapplicable", false);
          }
          else if (flag12)
            this.SetControlState("chkcoborsexnotapplicable", true);
          else
            this.SetControlState("chkcoborsexnotapplicable", false);
          defaultValue = ControlState.Default;
          goto case "101";
        case "424":
          if (this.GetField("1825") == "2020")
          {
            this.f424_2009.Visible = false;
            goto case "101";
          }
          else
          {
            this.f424_2009.Visible = true;
            this.f424_2009.Position = new Point(167, 572);
            goto case "101";
          }
        case "4970":
        case "4971":
        case "4972":
        case "MORNET.X30":
          if (this.GetField("5027") != "Y")
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "5018":
        case "5019":
        case "5020":
        case "5021":
          defaultValue = !(this.GetField("5028") == "Y") ? ControlState.Disabled : ControlState.Enabled;
          goto case "101";
        case "9":
          if (this.loan.GetField("19") != "Other")
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "994":
          if (this.loan.GetField("608") != "OtherAmortizationType")
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "995":
        case "zoomarm":
          if (this.loan.GetField("608") != "AdjustableRate" && this.inputData.GetField("1172") != "HELOC")
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "MORNET.X84":
          if (this.loan.GetField("MORNET.X83") != "Y")
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "MORNET.X90":
          if (this.loan.GetField("MORNET.X89") != "Y")
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "baseincome":
          if (this.GetField("1825") == "2020")
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "getami":
          if (this.FormIsForTemplate || this.GetField("5027") == "Y")
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "getmfi":
          defaultValue = this.FormIsForTemplate || this.GetField("5028") == "Y" ? ControlState.Disabled : ControlState.Enabled;
          goto case "101";
        case "lookup4970m":
          if (this.GetField("5027") == "Y")
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "mfilookup":
          defaultValue = !(this.GetField("5028") == "Y") ? ControlState.Enabled : ControlState.Disabled;
          goto case "101";
        case "otherf":
          string field4 = this.loan.GetField("420");
          if (!(field4 == "FirstLien") && !(field4 == string.Empty))
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "vod":
        case "vol":
          if (this.FormIsForTemplate)
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        default:
          defaultValue = ControlState.Default;
          goto case "101";
      }
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
      {
        case 816176745:
          if (!(action == "otherf"))
            break;
          this.SetFieldFocus("l_229");
          break;
        case 979029941:
          if (!(action == "copybrw"))
            break;
          this.SetFieldFocus("l_68");
          break;
        case 1001926223:
          if (!(action == "1stmor"))
            break;
          this.SetFieldFocus("l_120");
          break;
        case 1163987084:
          if (!(action == "mortg"))
            break;
          if (this.inputData.IsLocked("232"))
          {
            this.SetFieldFocus("l_232");
            break;
          }
          this.SetFieldFocus("l_233");
          break;
        case 1278929227:
          if (!(action == "baseincome"))
            break;
          this.SetFieldFocus("l_101");
          break;
        case 2418111490:
          if (!(action == "vod"))
            break;
          this.SetFieldFocus("l_182");
          break;
        case 2552332442:
          if (!(action == "vol"))
            break;
          this.SetFieldFocus("l_271");
          break;
        case 2728661377:
          if (!(action == "mipff"))
            break;
          this.SetFieldFocus("l_169");
          break;
        case 3363671541:
          if (!(action == "other"))
            break;
          this.SetFieldFocus("l_126");
          break;
        case 3880240577:
          if (!(action == "retaxes"))
            break;
          this.SetFieldFocus("l_1405");
          break;
        case 4106164968:
          if (!(action == "haz"))
            break;
          this.SetFieldFocus("l_230");
          break;
        case 4233502404:
          if (!(action == "zoomarm"))
            break;
          this.SetFieldFocus("l_995");
          break;
      }
    }
  }
}

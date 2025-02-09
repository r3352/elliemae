// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HMDA2018_REPURCHASEDInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class HMDA2018_REPURCHASEDInputHandler : InputHandlerBase
  {
    private GovernmentInfoInputHandler governmentInfoInputHandler;

    public HMDA2018_REPURCHASEDInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public HMDA2018_REPURCHASEDInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public HMDA2018_REPURCHASEDInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public HMDA2018_REPURCHASEDInputHandler(
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
        if (this.governmentInfoInputHandler == null)
          this.governmentInfoInputHandler = new GovernmentInfoInputHandler(this.currentForm, this.inputData, this.ToString());
        this.HMDAMappingLocks();
        if (this.loan == null || this.loan.Calculator == null)
          return;
        this.loan.Calculator.CalcOnDemand(CalcOnDemandEnum.PaymentSchedule);
        if (this.loan.GetField("3296") == string.Empty && this.loan.GetField("608") == "AdjustableRate")
          this.loan.Calculator.CalcRateCap();
        this.loan.Calculator.FormCalculation("POPULATESUBJECTPROPERTYADDRESS", (string) null, (string) null);
      }
      catch (Exception ex)
      {
      }
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      this.governmentInfoInputHandler.SetLayout((InputHandlerBase) this);
      this.HMDAMappingLocks();
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      this.RefreshContents();
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
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
              this.SetControlState(this.GetField("4142") == "Y" ? "CheckBox41" : "CheckBox10", false);
              if (this.GetField("4244") != "")
                this.SetControlState(this.GetField("4142") == "Y" ? "CheckBox41" : "CheckBox10", true);
            }
            else if (flag2)
              this.SetControlState(this.GetField("4142") == "Y" ? "CheckBox41" : "CheckBox10", true);
            else
              this.SetControlState(this.GetField("4142") == "Y" ? "CheckBox41" : "CheckBox10", false);
          }
          else if (flag2)
            this.SetControlState(this.GetField("4142") == "Y" ? "CheckBox41" : "CheckBox10", true);
          else
            this.SetControlState(this.GetField("4142") == "Y" ? "CheckBox41" : "CheckBox10", false);
          controlState = ControlState.Default;
          break;
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
              this.SetControlState(this.GetField("4142") == "Y" ? "CheckBox71" : "CheckBox19", false);
              if (this.GetField("4247") != "")
                this.SetControlState(this.GetField("4142") == "Y" ? "CheckBox71" : "CheckBox19", true);
            }
            else if (flag4)
              this.SetControlState(this.GetField("4142") == "Y" ? "CheckBox71" : "CheckBox19", true);
            else
              this.SetControlState(this.GetField("4142") == "Y" ? "CheckBox71" : "CheckBox19", false);
          }
          else if (flag4)
            this.SetControlState(this.GetField("4142") == "Y" ? "CheckBox71" : "CheckBox19", true);
          else
            this.SetControlState(this.GetField("4142") == "Y" ? "CheckBox71" : "CheckBox19", false);
          controlState = ControlState.Default;
          break;
        case "1825":
          if (Utils.ParseInt((object) this.GetField("HMDA.X27")) >= 2018)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "384":
          if (Utils.ParseInt((object) this.GetField("HMDA.X27")) >= 2018)
          {
            controlState = !(this.inputData.GetField("1393") == "Loan purchased by your institution") || !(Utils.ParseDate((object) this.inputData.GetField("745")) < Utils.ParseDate((object) "01/01/2018")) ? ControlState.Default : ControlState.Disabled;
            break;
          }
          break;
        case "3840":
          if (this.GetField("3840") != "Y")
          {
            this.SetControlState("panelCoBorrowerEthnicity", true);
            break;
          }
          this.SetControlState("panelCoBorrowerEthnicity", false);
          break;
        case "4131":
          bool enabled1 = !(this.GetField("4131") == "FaceToFace");
          this.SetControlState("chkCoBorEthInfoNotProvided", enabled1);
          this.SetControlState("chkCoBorRaceInfoNotProvided", enabled1);
          this.SetControlState("chkCoBorSexInfoNotProvided", enabled1);
          controlState = ControlState.Default;
          break;
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
          controlState = ControlState.Default;
          break;
        case "4142":
          if (Utils.ParseInt((object) this.GetField("1825")) >= 2020)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "4143":
          bool enabled2 = !(this.GetField("4143") == "FaceToFace");
          this.SetControlState("chkBorEthInfoNotProvided", enabled2);
          this.SetControlState("chkBorRaceInfoNotProvided", enabled2);
          this.SetControlState("chkBorSexInfoNotProvided", enabled2);
          controlState = ControlState.Default;
          break;
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
          controlState = ControlState.Default;
          break;
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
                break;
              }
              break;
            }
            if (flag10)
            {
              this.SetControlState("chkborsexnotapplicable", true);
              break;
            }
            this.SetControlState("chkborsexnotapplicable", false);
            break;
          }
          if (flag10)
          {
            this.SetControlState("chkborsexnotapplicable", true);
            break;
          }
          this.SetControlState("chkborsexnotapplicable", false);
          break;
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
          controlState = ControlState.Default;
          break;
        case "761":
          if (this.GetField("3941") == "Y")
          {
            ((RuntimeControl) this.currentForm.FindControl("Calendar1")).Enabled = false;
            return ControlState.Disabled;
          }
          ((RuntimeControl) this.currentForm.FindControl("Calendar1")).Enabled = true;
          break;
        case "HMDA.X107":
          if (this.GetField("1393") == "Loan purchased by your institution" && Utils.ParseDate((object) this.GetField("745")) < Utils.ParseDate((object) "01/01/2018"))
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "HMDA.X21":
        case "HMDA.X39":
        case "HMDA.X40":
        case "HMDA.X42":
        case "HMDA.X44":
        case "HMDA.X50":
          controlState = this.loan == null || this.loan.Calculator == null || !this.loan.Calculator.ValidateHmdaCalculation(id) || this.loan.IsLocked(id) ? ControlState.Default : ControlState.Disabled;
          break;
        case "HMDA.X87":
          if (this.inputData.GetField("HMDA.X91") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "HMDA.X88":
          if (this.inputData.GetField("HMDA.X91") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "HMDA.X89":
          if (this.inputData.GetField("HMDA.X91") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "HMDA.X90":
          if (this.inputData.GetField("HMDA.X91") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "HMDA.X92":
          if (this.loan != null && this.loan.IsTemplate)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "HMDA.X95":
        case "HMDA.X96":
          controlState = ControlState.Disabled;
          this.SetControlState("Calendar8", false);
          break;
        default:
          controlState = ControlState.Default;
          break;
      }
      return controlState;
    }

    private void HMDAMappingLocks()
    {
      this.ShowHmdaLock("HMDA.X39", "FieldLock28", "hmdax39NoLock");
      this.ShowHmdaLock("HMDA.X40", "FieldLock29", "hmdax40NoLock");
      this.ShowHmdaLock("HMDA.X86", "FieldLock21", "hmdax86NoLock");
    }
  }
}

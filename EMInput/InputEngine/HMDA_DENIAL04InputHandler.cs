// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HMDA_DENIAL04InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class HMDA_DENIAL04InputHandler : InputHandlerBase
  {
    private GovernmentInfoInputHandler governmentInfoInputHandler;

    public HMDA_DENIAL04InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public HMDA_DENIAL04InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public HMDA_DENIAL04InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public HMDA_DENIAL04InputHandler(
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
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id == "470" && val != "Other")
        this.loan.SetField("1136", string.Empty);
      else if (id == "477" && val != "Other")
        this.loan.SetField("1300", string.Empty);
      else if (id == "188" && val == "Y")
      {
        for (int index = 1523; index <= 1530; ++index)
          this.loan.SetField(index.ToString(), string.Empty);
      }
      else if (id == "189" && val == "Y")
      {
        for (int index = 1531; index <= 1538; ++index)
          this.loan.SetField(index.ToString(), string.Empty);
      }
      base.UpdateFieldValue(id, val);
      this.RefreshContents();
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = ControlState.Enabled;
      switch (id)
      {
        case "1393":
        case "editcheck":
        case "geocode":
        case "ratespread":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "1524":
        case "1525":
        case "1526":
        case "1527":
        case "1528":
        case "1529":
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
          bool enabled1 = !(this.GetField("1524") == "Y") && !(this.GetField("1525") == "Y") && !(this.GetField("1526") == "Y") && !(this.GetField("1527") == "Y") && !(this.GetField("1528") == "Y") && !(this.GetField("4148") == "Y") && !(this.GetField("4149") == "Y") && !(this.GetField("4150") == "Y") && !(this.GetField("4151") == "Y") && !(this.GetField("4152") == "Y") && !(this.GetField("4153") == "Y") && !(this.GetField("4154") == "Y") && !(this.GetField("4155") == "Y") && !(this.GetField("4156") == "Y") && !(this.GetField("4157") == "Y") && !(this.GetField("4158") == "Y");
          this.SetControlState(this.GetField("4142") == "Y" ? "CheckBox41" : "CheckBox21", enabled1);
          controlState = ControlState.Default;
          break;
        case "1532":
        case "1533":
        case "1534":
        case "1535":
        case "1536":
        case "1537":
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
          bool enabled2 = !(this.GetField("1532") == "Y") && !(this.GetField("1533") == "Y") && !(this.GetField("1534") == "Y") && !(this.GetField("1535") == "Y") && !(this.GetField("1536") == "Y") && !(this.GetField("4163") == "Y") && !(this.GetField("4164") == "Y") && !(this.GetField("4165") == "Y") && !(this.GetField("4166") == "Y") && !(this.GetField("4167") == "Y") && !(this.GetField("4168") == "Y") && !(this.GetField("4169") == "Y") && !(this.GetField("4170") == "Y") && !(this.GetField("4171") == "Y") && !(this.GetField("4172") == "Y") && !(this.GetField("4173") == "Y");
          this.SetControlState(this.GetField("4142") == "Y" ? "CheckBox71" : "CheckBox28", enabled2);
          controlState = ControlState.Default;
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
          bool enabled3 = !(this.GetField("4131") == "FaceToFace");
          this.SetControlState("chkCoBorEthInfoNotProvided", enabled3);
          this.SetControlState("chkCoBorRaceInfoNotProvided", enabled3);
          this.SetControlState("chkCoBorSexInfoNotProvided", enabled3);
          controlState = ControlState.Default;
          break;
        case "4143":
          bool enabled4 = !(this.GetField("4143") == "FaceToFace");
          this.SetControlState("chkBorEthInfoNotProvided", enabled4);
          this.SetControlState("chkBorRaceInfoNotProvided", enabled4);
          this.SetControlState("chkBorSexInfoNotProvided", enabled4);
          controlState = ControlState.Default;
          break;
        case "4144":
        case "4145":
        case "4146":
        case "4147":
        case "4205":
        case "4210":
        case "4211":
          if (this.GetField("4205") == "Y" || this.GetField("4210") == "Y" || this.GetField("4211") == "Y" || this.GetField("4144") == "Y" || this.GetField("4145") == "Y" || this.GetField("4146") == "Y" || this.GetField("4147") == "Y")
            this.SetControlState("chkEthnicityBorrNA", false);
          else
            this.SetControlState("chkEthnicityBorrNA", true);
          controlState = ControlState.Default;
          break;
        case "4159":
        case "4160":
        case "4161":
        case "4162":
        case "4206":
        case "4213":
        case "4214":
          if (this.GetField("4206") == "Y" || this.GetField("4213") == "Y" || this.GetField("4214") == "Y" || this.GetField("4159") == "Y" || this.GetField("4160") == "Y" || this.GetField("4161") == "Y" || this.GetField("4162") == "Y")
            this.SetControlState("chkEthnicityCoBorrNA", false);
          else
            this.SetControlState("chkEthnicityCoBorrNA", true);
          controlState = ControlState.Default;
          break;
        case "4193":
        case "4194":
        case "4195":
          if (this.GetField("4193") == "Y" || this.GetField("4194") == "Y" || this.GetField("4195") == "Y")
          {
            this.SetControlState("chkborsexnotapplicable", false);
            break;
          }
          this.SetControlState("chkborsexnotapplicable", true);
          break;
        case "4197":
        case "4198":
        case "4199":
          if (this.GetField("4197") == "Y" || this.GetField("4198") == "Y" || this.GetField("4199") == "Y")
          {
            this.SetControlState("chkcoborsexnotapplicable", false);
            break;
          }
          this.SetControlState("chkcoborsexnotapplicable", true);
          break;
        default:
          controlState = ControlState.Default;
          break;
      }
      return controlState;
    }

    public override void ExecAction(string action)
    {
      IEPass service = Session.Application.GetService<IEPass>();
      switch (action)
      {
        case "ratespread":
          service.ProcessURL("_EPASS_SIGNATURE;PCIWIZ;2;RATE_SPREAD");
          this.UpdateContents();
          break;
        case "geocode":
          service.ProcessURL("_EPASS_SIGNATURE;PCIWIZ;2;GEOCODING;SINGLE");
          this.UpdateContents();
          break;
        case "editcheck":
          service.ProcessURL("_EPASS_SIGNATURE;PCIWIZ;2;EDIT_CHECKS;SINGLE");
          break;
        default:
          base.ExecAction(action);
          break;
      }
    }
  }
}

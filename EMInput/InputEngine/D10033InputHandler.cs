// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.D10033InputHandler
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
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class D10033InputHandler : InputHandlerBase
  {
    private GovernmentInfoInputHandler governmentInfoInputHandler;
    private EllieMae.Encompass.Forms.Panel panelUSDA;
    private EllieMae.Encompass.Forms.Panel panelForm;
    private EllieMae.Encompass.Forms.Label label35;
    private EllieMae.Encompass.Forms.Panel panel2018DIInstructions;
    private CategoryBox categoryInfoForGovernment;
    private EllieMae.Encompass.Forms.GroupBox groupBoxLoanOriginator;
    private EllieMae.Encompass.Forms.Label lblPayoffNote;
    private EllieMae.Encompass.Forms.Panel pnlBorPaidFees;
    private EllieMae.Encompass.Forms.Panel pnlBorPaidFeesNew;
    private static ArrayList ynList = new ArrayList();

    public D10033InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public D10033InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public D10033InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public D10033InputHandler(
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
        if (this.panelForm == null)
          this.panelForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        if (this.panel2018DIInstructions == null)
          this.panel2018DIInstructions = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("Panel2018DIInstructions");
        if (this.categoryInfoForGovernment == null)
          this.categoryInfoForGovernment = (CategoryBox) this.currentForm.FindControl("categoryInfoForGovernment");
        if (this.label35 == null)
          this.label35 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label35");
        if (this.panelUSDA == null)
          this.panelUSDA = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelUSDAOption");
        if (this.groupBoxLoanOriginator == null)
          this.groupBoxLoanOriginator = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBoxLoanOriginator");
        if (this.governmentInfoInputHandler == null)
          this.governmentInfoInputHandler = new GovernmentInfoInputHandler(this.currentForm, this.inputData, this.ToString());
        if (this.lblPayoffNote == null)
          this.lblPayoffNote = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblPayoffNote");
        this.pnlBorPaidFees = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("BorPaidFeesPanel");
        this.pnlBorPaidFeesNew = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("BorPaidFeesNewPanel");
        if (this.inputData.GetField("4796") == "Y")
        {
          this.enableOrDisableDropdownPanels(this.pnlBorPaidFeesNew, this.pnlBorPaidFees);
          this.pnlBorPaidFeesNew.Position = this.pnlBorPaidFees.Position;
        }
        else
          this.enableOrDisableDropdownPanels(this.pnlBorPaidFees, this.pnlBorPaidFeesNew);
      }
      catch (Exception ex)
      {
      }
    }

    static D10033InputHandler()
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
        D10033InputHandler.ynList.Add(obj);
    }

    protected override string FormatValue(string fieldId, string value)
    {
      if (D10033InputHandler.ynList.Contains((object) fieldId))
      {
        string str = value.ToUpper();
        if (str != "Y" && str != "N")
          str = string.Empty;
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
          this.CurrentValue = "";
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
      else if (id == "1825" && val == "N")
        val = string.Empty;
      else if (id == "1612")
        base.UpdateFieldValue("3239", "");
      base.UpdateFieldValue(id, val);
      this.RefreshContents();
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

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement && (controlForElement.Field.FieldID == "3237" || controlForElement.Field.FieldID == "3238"))
      {
        bool needsUpdate = false;
        string str = Utils.FormatInput(controlForElement.Value, FieldFormat.INTEGER, ref needsUpdate);
        if (needsUpdate)
          controlForElement.BindTo(str);
      }
      base.onkeyup(pEvtObj);
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState defaultValue = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1136":
          if (this.loan.GetField("470") != "Other")
          {
            defaultValue = ControlState.Disabled;
            goto case "319";
          }
          else
            goto case "319";
        case "1300":
          if (this.loan.GetField("477") != "Other")
          {
            defaultValue = ControlState.Disabled;
            goto case "319";
          }
          else
            goto case "319";
        case "140":
          defaultValue = this.GetSpecialControlStatus(id, defaultValue);
          goto case "319";
        case "142":
          EllieMae.Encompass.Forms.Label control1 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblCashFromTo");
          if (control1 != null)
            control1.Text = this.ToDouble(this.loan.GetSimpleField("142")) <= 0.0 ? "Cash to borrower" : "Cash from borrower";
          defaultValue = ControlState.Default;
          goto case "319";
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
          defaultValue = ControlState.Default;
          goto case "319";
        case "1531":
          if (this.GetField("3840") != "Y")
            this.SetControlState("panelCoBorrowerEthnicity", true);
          else
            this.SetControlState("panelCoBorrowerEthnicity", false);
          defaultValue = ControlState.Default;
          goto case "319";
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
          defaultValue = ControlState.Default;
          goto case "319";
        case "1825":
          if (Utils.ParseInt((object) this.GetField("HMDA.X27")) >= 2018)
            defaultValue = ControlState.Disabled;
          if (Utils.ParseInt((object) this.GetField("1825")) >= 2020)
          {
            defaultValue = ControlState.Disabled;
            goto case "319";
          }
          else
            goto case "319";
        case "1845":
          if (this.loan.GetField("420") == "FirstLien")
          {
            defaultValue = ControlState.Disabled;
            goto case "319";
          }
          else
            goto case "319";
        case "1851":
          EllieMae.Encompass.Forms.Label control2 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblCCFrom");
          if (control2 != null)
          {
            control2.Text = !(this.loan.GetSimpleField("420") == "SecondLien") ? "CC from 2nd" : "CC from 1st";
            goto case "319";
          }
          else
            goto case "319";
        case "319":
          return defaultValue;
        case "4131":
          bool enabled1 = !(this.GetField("4131") == "FaceToFace");
          this.SetControlState("chkCoBorEthInfoNotProvided", enabled1);
          this.SetControlState("chkCoBorRaceInfoNotProvided", enabled1);
          this.SetControlState("chkCoBorSexInfoNotProvided", enabled1);
          defaultValue = ControlState.Default;
          goto case "319";
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
          goto case "319";
        case "4142":
          if (Utils.ParseInt((object) this.GetField("1825")) == 2020)
          {
            defaultValue = ControlState.Disabled;
            goto case "319";
          }
          else
            goto case "319";
        case "4143":
          bool enabled2 = !(this.GetField("4143") == "FaceToFace");
          this.SetControlState("chkBorEthInfoNotProvided", enabled2);
          this.SetControlState("chkBorRaceInfoNotProvided", enabled2);
          this.SetControlState("chkBorSexInfoNotProvided", enabled2);
          defaultValue = ControlState.Default;
          goto case "319";
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
          goto case "319";
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
                goto case "319";
              }
              else
                goto case "319";
            }
            else if (flag10)
            {
              this.SetControlState("chkborsexnotapplicable", true);
              goto case "319";
            }
            else
            {
              this.SetControlState("chkborsexnotapplicable", false);
              goto case "319";
            }
          }
          else if (flag10)
          {
            this.SetControlState("chkborsexnotapplicable", true);
            goto case "319";
          }
          else
          {
            this.SetControlState("chkborsexnotapplicable", false);
            goto case "319";
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
          goto case "319";
        case "969":
          defaultValue = ControlState.Default;
          if (this.panelUSDA != null)
          {
            this.panelUSDA.Visible = this.loan.GetField("1172") == "FarmersHomeAdministration";
            goto case "319";
          }
          else
            goto case "319";
        case "vom":
          if (this.FormIsForTemplate)
          {
            defaultValue = ControlState.Disabled;
            goto case "319";
          }
          else
            goto case "319";
        default:
          defaultValue = ControlState.Default;
          goto case "319";
      }
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      this.rearrangeLayout();
      if (this.lblPayoffNote == null)
        return;
      if (this.GetFieldValue("19") == "Purchase" && this.loan.LoanHasLiabilityToBePaidoff())
        this.lblPayoffNote.Visible = true;
      else
        this.lblPayoffNote.Visible = false;
    }

    private void setControlStateForInvisibleForm(string fieldId, string fieldVal, string controlId)
    {
      if (this.inputData.GetField(fieldId) == fieldVal)
      {
        this.SetControlState(controlId, false);
        this.SetControlState(controlId, true);
      }
      else
      {
        this.SetControlState(controlId, true);
        this.SetControlState(controlId, false);
      }
    }

    private void rearrangeLayout()
    {
      this.governmentInfoInputHandler.SetLayout((InputHandlerBase) this);
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      if (!(action == "mipff"))
        return;
      this.SetFieldFocus("l_1109");
    }
  }
}

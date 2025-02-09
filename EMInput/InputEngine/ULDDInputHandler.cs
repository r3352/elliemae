// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ULDDInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ULDDInputHandler : InputHandlerBase
  {
    private List<EllieMae.Encompass.Forms.Panel> panelLocalAddresses;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignAddresses;
    private List<StandardButton> selectCountryButtons;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignPanels;
    private EllieMae.Encompass.Forms.Label addressLabel;
    private EllieMae.Encompass.Forms.TextBox adrTextBox;
    private GovernmentInfoInputHandler governmentInfoInputHandler;

    public ULDDInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public ULDDInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public ULDDInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public ULDDInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public override void ExecAction(string action)
    {
      if (action == "showhelp_uldd_creditscoreprovidername")
      {
        string topicName = "ULDD";
        if (this.session == null)
          Session.Application.DisplayHelp(topicName);
        else
          this.session.Application.DisplayHelp(topicName);
      }
      base.ExecAction(action);
    }

    internal override void CreateControls()
    {
      try
      {
        if (this.governmentInfoInputHandler == null)
          this.governmentInfoInputHandler = new GovernmentInfoInputHandler(this.currentForm, this.inputData, this.ToString());
        base.CreateControls();
        this.panelLocalAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignPanels = new List<EllieMae.Encompass.Forms.Panel>();
        for (int index = 1; index <= 2; ++index)
        {
          this.panelLocalAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("localAdr" + (object) index));
          this.panelForeignAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignAdr" + (object) index));
          if (this.panelForeignAddresses[index - 1] != null && this.panelLocalAddresses[index - 1] != null)
            this.panelForeignAddresses[index - 1].Position = this.panelLocalAddresses[index - 1].Position;
        }
        for (int index = 1; index <= 2; ++index)
          this.panelForeignPanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignPanel" + (object) index));
        this.selectCountryButtons = new List<StandardButton>();
        this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_URLA_X269"));
        this.addressLabel = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label177");
        this.adrTextBox = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_1416");
      }
      catch (Exception ex)
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "11":
        case "1416":
          return controlState;
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
              this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "CheckBox51", false);
              if (this.GetField("4244") != "")
                this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "CheckBox51", true);
            }
            else if (flag2)
              this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "CheckBox51", true);
            else
              this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "CheckBox51", false);
          }
          else if (flag2)
            this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "CheckBox51", true);
          else
            this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "CheckBox51", false);
          controlState = ControlState.Default;
          goto case "11";
        case "1531":
          if (this.GetField("3840") != "Y")
            this.SetControlState("panelCoBorrowerEthnicity", true);
          else
            this.SetControlState("panelCoBorrowerEthnicity", false);
          controlState = ControlState.Default;
          goto case "11";
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
              this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "CheckBox58", false);
              if (this.GetField("4247") != "")
                this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "CheckBox58", true);
            }
            else if (flag4)
              this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "CheckBox58", true);
            else
              this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "CheckBox58", false);
          }
          else if (flag4)
            this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "CheckBox58", true);
          else
            this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "CheckBox58", false);
          controlState = ControlState.Default;
          goto case "11";
        case "1556":
          controlState = !(this.GetFieldValue("1543") == "Other") ? ControlState.Disabled : ControlState.Enabled;
          goto case "11";
        case "4131":
          bool enabled1 = !(this.GetField("4131") == "FaceToFace");
          this.SetControlState("chkCoBorEthInfoNotProvided", enabled1);
          this.SetControlState("chkCoBorRaceInfoNotProvided", enabled1);
          this.SetControlState("chkCoBorSexInfoNotProvided", enabled1);
          controlState = ControlState.Default;
          goto case "11";
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
          goto case "11";
        case "4143":
          bool enabled2 = !(this.GetField("4143") == "FaceToFace");
          this.SetControlState("chkBorEthInfoNotProvided", enabled2);
          this.SetControlState("chkBorRaceInfoNotProvided", enabled2);
          this.SetControlState("chkBorSexInfoNotProvided", enabled2);
          controlState = ControlState.Default;
          goto case "11";
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
          goto case "11";
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
                goto case "11";
              }
              else
                goto case "11";
            }
            else if (flag10)
            {
              this.SetControlState("chkborsexnotapplicable", true);
              goto case "11";
            }
            else
            {
              this.SetControlState("chkborsexnotapplicable", false);
              goto case "11";
            }
          }
          else if (flag10)
          {
            this.SetControlState("chkborsexnotapplicable", true);
            goto case "11";
          }
          else
          {
            this.SetControlState("chkborsexnotapplicable", false);
            goto case "11";
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
          controlState = ControlState.Default;
          goto case "11";
        case "4709":
          controlState = !(this.GetFieldValue("965") == "N") || !(this.GetFieldValue("466") == "N") ? ControlState.Disabled : ControlState.Enabled;
          goto case "11";
        case "4710":
          controlState = !(this.GetFieldValue("985") == "N") || !(this.GetFieldValue("467") == "N") ? ControlState.Disabled : ControlState.Enabled;
          goto case "11";
        case "761":
          if (this.GetField("3941") == "Y")
          {
            ((RuntimeControl) this.currentForm.FindControl("Calendar3")).Enabled = false;
            return ControlState.Disabled;
          }
          ((RuntimeControl) this.currentForm.FindControl("Calendar3")).Enabled = true;
          goto case "11";
        case "CASASRN.X141":
          controlState = !(this.GetFieldValue("425") != "Y") ? ControlState.Enabled : ControlState.Disabled;
          goto case "11";
        case "ExportULDDFannie":
        case "ExportULDDFreddie":
          controlState = !this.loan.IsTemplate ? (!this.loan.IsULDDExporting ? ControlState.Default : this.getControlState(ctrl, id, ControlState.Enabled)) : ControlState.Disabled;
          goto case "11";
        case "ULDD.FNM.SectionOfAct":
          if (this.GetFieldValue("1039") != string.Empty)
          {
            this.loan.SetField(id, this.GetFieldValue("1039"));
            if (this.GetFieldValue("1039") == "184" || this.GetFieldValue("1039") == "203B" || this.GetFieldValue("1039") == "234C")
            {
              this.loan.SetField("ULDD.FRE.SectionOfAct", this.GetFieldValue("1039"));
              goto case "11";
            }
            else
              goto case "11";
          }
          else
            goto case "11";
        case "ULDD.FRE.AVMModelNameExpl":
          controlState = !(this.GetFieldValue("ULDD.X32") == "Other") ? ControlState.Disabled : ControlState.Enabled;
          goto case "11";
        case "ULDD.FRE.CLOSINGCOST2FUNDSTYPEOTHERDESC":
          if (this.GetFieldValue("ULDD.FRE.CLOSINGCOST2FUNDSTYPE") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.FRE.CLOSINGCOST2SOURCETYPEOTHERDESC":
          if (this.GetFieldValue("ULDD.FRE.CLOSINGCOST2SOURCETYPE") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.FRE.CLOSINGCOST3FUNDSTYPEOTHERDESC":
          if (this.GetFieldValue("ULDD.FRE.CLOSINGCOST3FUNDSTYPE") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.FRE.CLOSINGCOST3SOURCETYPEOTHERDESC":
          if (this.GetFieldValue("ULDD.FRE.CLOSINGCOST3SOURCETYPE") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.FRE.CLOSINGCOST4FUNDSTYPEOTHERDESC":
          if (this.GetFieldValue("ULDD.FRE.CLOSINGCOST4FUNDSTYPE") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.FRE.CLOSINGCOST4SOURCETYPEOTHERDESC":
          if (this.GetFieldValue("ULDD.FRE.CLOSINGCOST4SOURCETYPE") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.FRE.DownPmt2SourceTypeExpl":
          controlState = !(this.GetFieldValue("ULDD.FRE.DownPmt2SourceType") == "Other") ? ControlState.Disabled : ControlState.Enabled;
          goto case "11";
        case "ULDD.FRE.DownPmt2TypeExpl":
          controlState = !(this.GetFieldValue("ULDD.FRE.DownPmt2Type") == "OtherTypeOfDownPayment") ? ControlState.Disabled : ControlState.Enabled;
          goto case "11";
        case "ULDD.FRE.DownPmt3SourceTypeExpl":
          controlState = !(this.GetFieldValue("ULDD.FRE.DownPmt3SourceType") == "Other") ? ControlState.Disabled : ControlState.Enabled;
          goto case "11";
        case "ULDD.FRE.DownPmt3TypeExpl":
          controlState = !(this.GetFieldValue("ULDD.FRE.DownPmt3Type") == "OtherTypeOfDownPayment") ? ControlState.Disabled : ControlState.Enabled;
          goto case "11";
        case "ULDD.FRE.DownPmt4SourceTypeExpl":
          controlState = !(this.GetFieldValue("ULDD.FRE.DownPmt4SourceType") == "Other") ? ControlState.Disabled : ControlState.Enabled;
          goto case "11";
        case "ULDD.FRE.DownPmt4TypeExpl":
          controlState = !(this.GetFieldValue("ULDD.FRE.DownPmt4Type") == "OtherTypeOfDownPayment") ? ControlState.Disabled : ControlState.Enabled;
          goto case "11";
        case "ULDD.FRE.ExplanationOfDownPayment":
          controlState = !(this.GetFieldValue("ULDD.FRE.DownPaymentType") == "OtherTypeOfDownPayment") ? ControlState.Disabled : ControlState.Enabled;
          goto case "11";
        case "ULDD.FRE.SectionOfAct":
          if (this.GetFieldValue("1039") != string.Empty && (this.GetFieldValue("1039") == "184" || this.GetFieldValue("1039") == "203B" || this.GetFieldValue("1039") == "234C"))
          {
            this.loan.SetField(id, this.GetFieldValue("1039"));
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.GNM.OtherDwnPymntFndsType":
          if (this.GetFieldValue("ULDD.GNM.DwnPymntFndsType") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.X103":
          if (this.GetFieldValue("ULDD.X102") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.X107":
          if (this.GetFieldValue("ULDD.X106") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.X109":
          if (this.GetFieldValue("ULDD.X108") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.X121":
          controlState = !(this.GetFieldValue("ULDD.X119") != "Other") ? ControlState.Enabled : ControlState.Disabled;
          goto case "11";
        case "ULDD.X122":
          controlState = !(this.GetFieldValue("ULDD.X120") != "Other") ? ControlState.Enabled : ControlState.Disabled;
          goto case "11";
        case "ULDD.X135":
          controlState = !(this.GetFieldValue("ULDD.X134") != "Other") ? ControlState.Enabled : ControlState.Disabled;
          goto case "11";
        case "ULDD.X136":
          controlState = !(this.GetFieldValue("ULDD.X134") != "Other") ? ControlState.Enabled : ControlState.Disabled;
          goto case "11";
        case "ULDD.X137":
          controlState = !(this.GetFieldValue("425") != "Y") ? ControlState.Enabled : ControlState.Disabled;
          goto case "11";
        case "ULDD.X144":
          string fieldValue = this.GetFieldValue("1553");
          controlState = fieldValue == "Manufactured Housing Single Wide" || fieldValue == "Manufactured Housing Multiwide" ? ControlState.Enabled : ControlState.Disabled;
          goto case "11";
        case "ULDD.X149":
          controlState = !(this.GetFieldValue("1543") != "Other") ? ControlState.Enabled : ControlState.Disabled;
          goto case "11";
        case "ULDD.X15":
          controlState = !(this.GetFieldValue("ULDD.X11") != "Simple") ? ControlState.Enabled : ControlState.Disabled;
          goto case "11";
        case "ULDD.X152":
          controlState = !(this.GetFieldValue("2847") != "C") ? ControlState.Enabled : ControlState.Disabled;
          goto case "11";
        case "ULDD.X153":
          controlState = !(this.GetFieldValue("ULDD.FRE.X1172") != "Other") ? ControlState.Enabled : ControlState.Disabled;
          goto case "11";
        case "ULDD.X160":
        case "ULDD.X161":
          controlState = !(this.GetFieldValue("16") != "") || Utils.ParseInt((object) this.GetFieldValue("16")) <= 1 ? ControlState.Disabled : ControlState.Enabled;
          goto case "11";
        case "ULDD.X162":
        case "ULDD.X163":
          controlState = !(this.GetFieldValue("16") != "") || Utils.ParseInt((object) this.GetFieldValue("16")) <= 2 ? ControlState.Disabled : ControlState.Enabled;
          goto case "11";
        case "ULDD.X164":
        case "ULDD.X165":
          controlState = !(this.GetFieldValue("16") != "") || Utils.ParseInt((object) this.GetFieldValue("16")) <= 3 ? ControlState.Disabled : ControlState.Enabled;
          goto case "11";
        case "ULDD.X170":
        case "ULDD.X171":
          controlState = !(this.GetFieldValue("16") != "") || Utils.ParseInt((object) this.GetFieldValue("16")) <= 0 ? ControlState.Disabled : ControlState.Enabled;
          goto case "11";
        case "ULDD.X188":
          if (this.GetFieldValue("ULDD.X187") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.X190":
          if (this.GetFieldValue("ULDD.X29") == "Other")
          {
            controlState = ControlState.Enabled;
            ctrl.Visible = true;
            goto case "11";
          }
          else
          {
            ctrl.Visible = false;
            controlState = ControlState.Disabled;
            goto case "11";
          }
        case "ULDD.X209":
          if (this.GetFieldValue("ULDD.X207") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.X210":
          if (this.GetFieldValue("ULDD.X208") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.X213":
          if (this.GetFieldValue("ULDD.X211") != "Third Party")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.X214":
          if (this.GetFieldValue("ULDD.X212") != "Third Party")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.X25":
          if (this.GetFieldValue("ULDD.X24") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.X52":
          if (this.GetFieldValue("ULDD.X51") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ULDD.X55":
          controlState = !(this.GetFieldValue("ULDD.X11") != "Simple") ? ControlState.Enabled : ControlState.Disabled;
          goto case "11";
        case "ULDD.X90":
          if (this.GetFieldValue("ULDD.X89") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "URLA.X269":
          if (this.loan.GetField("URLA.X267") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "ValidateULDDExportFannie":
        case "ValidateULDDExportFreddie":
        case "ValidateULDDExportGinnie":
          if (this.loan.IsTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        default:
          controlState = ControlState.Default;
          goto case "11";
      }
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      this.governmentInfoInputHandler.SetLayout((InputHandlerBase) this);
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      bool flag = false;
      base.UpdateFieldValue(id, val);
      switch (id)
      {
        case "ULDD.RefinanceCashOutAmount":
          if (!this.loan.IsValidValue(id, val))
          {
            int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "This Refinance Cash Out Amount cannot be a negative number.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            break;
          }
          break;
        case "ULDD.X134":
          if (val != "Other")
          {
            this.loan.SetField("ULDD.X135", "");
            this.loan.SetField("ULDD.X136", "");
            break;
          }
          break;
        case "ULDD.X173":
          if (!Utils.IsInt((object) val))
            return;
          if (Utils.ParseInt((object) val) < 0 || Utils.ParseInt((object) val) > 999)
          {
            int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "Valid entry for \"Related Loan Maturity Period Count\" is a number 0-999.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          break;
        case "ULDD.X179":
          if (!this.loan.IsValidValue(id, val))
          {
            int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "Each investor feature identifier must be three alpha numeric characters and separated by spaces with a maximum of ten codes.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          break;
        case "ULDD.X36":
          string[] strArray = val.Trim().Split(' ');
          if (strArray.Length > 10)
          {
            flag = true;
          }
          else
          {
            foreach (string str in strArray)
            {
              if (str.Length != 3)
              {
                flag = true;
                break;
              }
              foreach (char ch in str.ToCharArray())
              {
                if (!Utils.IsInt((object) (ch.ToString() ?? "")))
                {
                  flag = true;
                  break;
                }
              }
            }
          }
          if (flag)
          {
            int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "Each special feature code must be three digits and separated by spaces with a maximum of ten codes.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          break;
        case "ULDD.X45":
          if (!Utils.IsInt((object) val))
            return;
          if (Utils.ParseInt((object) val) < 1 || Utils.ParseInt((object) val) > 31)
          {
            int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "Valid entry for \"Investor Remittance Day\" is a number 1-31.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          break;
        case "ULDD.X86":
          if (!Utils.IsInt((object) val))
            return;
          if (Utils.ParseInt((object) val) < 1 || Utils.ParseInt((object) val) > 31)
          {
            int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "Valid entry for \"Pool Scheduled Remittance Payment Day\" is a number 1-31.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          break;
        case "ULDD.X98":
          if (!Utils.IsInt((object) val))
            return;
          if (Utils.ParseInt((object) val) < 0 || Utils.ParseInt((object) val) > 999)
          {
            int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "Valid entry for \"Loan Level Basis Points\" is a number 0-999.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
          }
          break;
      }
      this.RefreshContents();
    }

    public override void onfocusout(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onfocusout(pEvtObj);
      string fieldId = pEvtObj.srcElement.getAttribute("emid").ToString();
      if (!(fieldId == "ULDD.RefinanceCashOutAmount"))
        return;
      string id = pEvtObj.srcElement.id;
      string controlFieldValue = this.GetControlFieldValue(id);
      FieldControl control = this.currentForm.FindControl(id) as FieldControl;
      if (!this.loan.IsValidValue(fieldId, controlFieldValue))
      {
        this.loan.SetCurrentField("ULDD.RefinanceCashOutAmount", "");
        if (!(control.BackColor == Color.White))
          return;
        control.BackColor = Color.LightYellow;
      }
      else
      {
        if (!(control.BackColor == Color.LightYellow))
          return;
        control.BackColor = Color.White;
      }
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.selectCountryButtons != null && this.selectCountryButtons.Count > 0)
      {
        bool flag = this.GetField("URLA.X267") == "Y";
        if (this.selectCountryButtons[0] != null)
          this.selectCountryButtons[0].Visible = flag;
        if (this.panelLocalAddresses[0] != null && this.panelLocalAddresses[1] != null)
        {
          this.panelLocalAddresses[0].Visible = !flag;
          this.panelLocalAddresses[1].Visible = !flag;
        }
        if (this.panelForeignAddresses[0] != null && this.panelForeignAddresses[1] != null)
        {
          this.panelForeignAddresses[0].Visible = flag;
          this.panelForeignAddresses[1].Visible = flag;
        }
      }
      if (this.GetField("1825") == "2020")
      {
        for (int index = 0; index < 2; ++index)
        {
          if (this.panelForeignPanels[index] != null)
            this.panelForeignPanels[index].Visible = true;
        }
        if (this.addressLabel == null || this.adrTextBox == null || this.panelLocalAddresses[0] == null)
          return;
        this.addressLabel.Position = new Point(6, 28);
        this.adrTextBox.Position = new Point(229, 28);
        this.panelLocalAddresses[0].Position = new Point(0, 50);
      }
      else
      {
        for (int index = 0; index < 2; ++index)
        {
          if (this.panelForeignPanels[index] != null)
            this.panelForeignPanels[index].Visible = false;
        }
        if (this.addressLabel == null || this.adrTextBox == null || this.panelLocalAddresses[0] == null)
          return;
        this.addressLabel.Position = new Point(6, 6);
        this.adrTextBox.Position = new Point(229, 6);
        this.panelLocalAddresses[0].Position = new Point(0, 28);
      }
    }
  }
}

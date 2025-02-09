// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.D1003_2020P4InputHandler
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
  public class D1003_2020P4InputHandler : InputHandlerBase
  {
    private GovernmentInfoInputHandler governmentInfoInputHandler;
    private Hyperlink hyperlinkSectionA;
    private Hyperlink hyperlinkSectionB;
    private Hyperlink hyperlinkSectionC;
    private Hyperlink hyperlinkSectionD;
    private Hyperlink hyperlinkSectionD2;
    private Hyperlink hyperlinkSectionE;
    private Hyperlink hyperlinkSectionF;
    private Hyperlink hyperlinkSectionG;
    private Hyperlink hyperlinkSectionH;
    private Hyperlink hyperlinkSectionI;
    private Hyperlink hyperlinkSectionJ;
    private Hyperlink hyperlinkSectionK;
    private Hyperlink hyperlinkSectionL;
    private Hyperlink hyperlinkSectionM;
    private DropdownBox dropdownBorrowerSectionA;
    private DropdownBox dropdownCoBorrowerSectionA;
    private DropdownBox dropdownBorrowerSectionB;
    private DropdownBox dropdownCoBorrowerSectionB;
    private DropdownBox dropdownBorrowerSectionC;
    private DropdownBox dropdownCoBorrowerSectionC;
    private DropdownBox dropdownBorrowerSectionD;
    private DropdownBox dropdownCoBorrowerSectionD;
    private DropdownBox dropdownBorrowerSectionD2;
    private DropdownBox dropdownCoBorrowerSectionD2;
    private DropdownBox dropdownBorrowerSectionE;
    private DropdownBox dropdownCoBorrowerSectionE;
    private DropdownBox dropdownBorrowerSectionF;
    private DropdownBox dropdownCoBorrowerSectionF;
    private DropdownBox dropdownBorrowerSectionG;
    private DropdownBox dropdownCoBorrowerSectionG;
    private DropdownBox dropdownBorrowerSectionH;
    private DropdownBox dropdownCoBorrowerSectionH;
    private DropdownBox dropdownBorrowerSectionI;
    private DropdownBox dropdownCoBorrowerSectionI;
    private DropdownBox dropdownBorrowerSectionJ;
    private DropdownBox dropdownCoBorrowerSectionJ;
    private DropdownBox dropdownBorrowerSectionK;
    private DropdownBox dropdownCoBorrowerSectionK;
    private DropdownBox dropdownBorrowerSectionL;
    private DropdownBox dropdownCoBorrowerSectionL;
    private DropdownBox dropdownBorrowerSectionM;
    private DropdownBox dropdownCoBorrowerSectionM;
    private Hyperlink hyperlinkSectionA_Coborr;
    private Hyperlink hyperlinkSectionB_Coborr;
    private Hyperlink hyperlinkSectionC_Coborr;
    private Hyperlink hyperlinkSectionD_Coborr;
    private Hyperlink hyperlinkSectionD2_Coborr;
    private Hyperlink hyperlinkSectionE_Coborr;
    private Hyperlink hyperlinkSectionF_Coborr;
    private Hyperlink hyperlinkSectionG_Coborr;
    private Hyperlink hyperlinkSectionH_Coborr;
    private Hyperlink hyperlinkSectionI_Coborr;
    private Hyperlink hyperlinkSectionJ_Coborr;
    private Hyperlink hyperlinkSectionK_Coborr;
    private Hyperlink hyperlinkSectionL_Coborr;
    private Hyperlink hyperlinkSectionM_Coborr;

    public D1003_2020P4InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public D1003_2020P4InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public D1003_2020P4InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public D1003_2020P4InputHandler(
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
        this.hyperlinkSectionA = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionA");
        this.hyperlinkSectionA.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionB = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionB");
        this.hyperlinkSectionB.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionC = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionC");
        this.hyperlinkSectionC.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionD = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionD");
        this.hyperlinkSectionD.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionD2 = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionD2");
        this.hyperlinkSectionD2.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionE = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionE");
        this.hyperlinkSectionE.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionF = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionF");
        this.hyperlinkSectionF.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionG = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionG");
        this.hyperlinkSectionG.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionH = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionH");
        this.hyperlinkSectionH.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionI = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionI");
        this.hyperlinkSectionI.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionJ = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionJ");
        this.hyperlinkSectionJ.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionK = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionK");
        this.hyperlinkSectionK.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionL = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionL");
        this.hyperlinkSectionL.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionM = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionM");
        this.hyperlinkSectionM.Click += new EventHandler(this.explanationHyperlink_Click);
        this.dropdownBorrowerSectionA = (DropdownBox) this.currentForm.FindControl("DropdownBox9");
        this.dropdownBorrowerSectionA.Change += new EventHandler(this.declarations_Change);
        this.dropdownCoBorrowerSectionA = (DropdownBox) this.currentForm.FindControl("DropdownBox10");
        this.dropdownCoBorrowerSectionA.Change += new EventHandler(this.declarations_Change);
        this.dropdownBorrowerSectionB = (DropdownBox) this.currentForm.FindControl("DropdownBox17");
        this.dropdownBorrowerSectionB.Change += new EventHandler(this.declarations_Change);
        this.dropdownCoBorrowerSectionB = (DropdownBox) this.currentForm.FindControl("DropdownBox18");
        this.dropdownCoBorrowerSectionB.Change += new EventHandler(this.declarations_Change);
        this.dropdownBorrowerSectionC = (DropdownBox) this.currentForm.FindControl("DropdownBox19");
        this.dropdownBorrowerSectionC.Change += new EventHandler(this.declarations_Change);
        this.dropdownCoBorrowerSectionC = (DropdownBox) this.currentForm.FindControl("DropdownBox20");
        this.dropdownCoBorrowerSectionC.Change += new EventHandler(this.declarations_Change);
        this.dropdownBorrowerSectionD = (DropdownBox) this.currentForm.FindControl("DropdownBox21");
        this.dropdownBorrowerSectionD.Change += new EventHandler(this.declarations_Change);
        this.dropdownCoBorrowerSectionD = (DropdownBox) this.currentForm.FindControl("DropdownBox22");
        this.dropdownCoBorrowerSectionD.Change += new EventHandler(this.declarations_Change);
        this.dropdownBorrowerSectionD2 = (DropdownBox) this.currentForm.FindControl("DropdownBox23");
        this.dropdownBorrowerSectionD2.Change += new EventHandler(this.declarations_Change);
        this.dropdownCoBorrowerSectionD2 = (DropdownBox) this.currentForm.FindControl("DropdownBox24");
        this.dropdownCoBorrowerSectionD2.Change += new EventHandler(this.declarations_Change);
        this.dropdownBorrowerSectionE = (DropdownBox) this.currentForm.FindControl("DropdownBox25");
        this.dropdownBorrowerSectionE.Change += new EventHandler(this.declarations_Change);
        this.dropdownCoBorrowerSectionE = (DropdownBox) this.currentForm.FindControl("DropdownBox26");
        this.dropdownCoBorrowerSectionE.Change += new EventHandler(this.declarations_Change);
        this.dropdownBorrowerSectionF = (DropdownBox) this.currentForm.FindControl("DropdownBox27");
        this.dropdownBorrowerSectionF.Change += new EventHandler(this.declarations_Change);
        this.dropdownCoBorrowerSectionF = (DropdownBox) this.currentForm.FindControl("DropdownBox28");
        this.dropdownCoBorrowerSectionF.Change += new EventHandler(this.declarations_Change);
        this.dropdownBorrowerSectionG = (DropdownBox) this.currentForm.FindControl("DropdownBox30");
        this.dropdownBorrowerSectionG.Change += new EventHandler(this.declarations_Change);
        this.dropdownCoBorrowerSectionG = (DropdownBox) this.currentForm.FindControl("DropdownBox29");
        this.dropdownCoBorrowerSectionG.Change += new EventHandler(this.declarations_Change);
        this.dropdownBorrowerSectionH = (DropdownBox) this.currentForm.FindControl("DropdownBox31");
        this.dropdownBorrowerSectionH.Change += new EventHandler(this.declarations_Change);
        this.dropdownCoBorrowerSectionH = (DropdownBox) this.currentForm.FindControl("DropdownBox32");
        this.dropdownCoBorrowerSectionH.Change += new EventHandler(this.declarations_Change);
        this.dropdownBorrowerSectionI = (DropdownBox) this.currentForm.FindControl("DropdownBox33");
        this.dropdownBorrowerSectionI.Change += new EventHandler(this.declarations_Change);
        this.dropdownCoBorrowerSectionI = (DropdownBox) this.currentForm.FindControl("DropdownBox34");
        this.dropdownCoBorrowerSectionI.Change += new EventHandler(this.declarations_Change);
        this.dropdownBorrowerSectionJ = (DropdownBox) this.currentForm.FindControl("DropdownBox35");
        this.dropdownBorrowerSectionJ.Change += new EventHandler(this.declarations_Change);
        this.dropdownCoBorrowerSectionJ = (DropdownBox) this.currentForm.FindControl("DropdownBox36");
        this.dropdownCoBorrowerSectionJ.Change += new EventHandler(this.declarations_Change);
        this.dropdownBorrowerSectionK = (DropdownBox) this.currentForm.FindControl("DropdownBox39");
        this.dropdownBorrowerSectionK.Change += new EventHandler(this.declarations_Change);
        this.dropdownCoBorrowerSectionK = (DropdownBox) this.currentForm.FindControl("DropdownBox40");
        this.dropdownCoBorrowerSectionK.Change += new EventHandler(this.declarations_Change);
        this.dropdownBorrowerSectionL = (DropdownBox) this.currentForm.FindControl("DropdownBox41");
        this.dropdownBorrowerSectionL.Change += new EventHandler(this.declarations_Change);
        this.dropdownCoBorrowerSectionL = (DropdownBox) this.currentForm.FindControl("DropdownBox42");
        this.dropdownCoBorrowerSectionL.Change += new EventHandler(this.declarations_Change);
        this.dropdownBorrowerSectionM = (DropdownBox) this.currentForm.FindControl("DropdownBox43");
        this.dropdownBorrowerSectionM.Change += new EventHandler(this.declarations_Change);
        this.dropdownCoBorrowerSectionM = (DropdownBox) this.currentForm.FindControl("DropdownBox44");
        this.dropdownCoBorrowerSectionM.Change += new EventHandler(this.declarations_Change);
        this.hyperlinkSectionA_Coborr = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionA_Coborr");
        this.hyperlinkSectionA_Coborr.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionB_Coborr = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionB_Coborr");
        this.hyperlinkSectionB_Coborr.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionC_Coborr = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionC_Coborr");
        this.hyperlinkSectionC_Coborr.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionD_Coborr = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionD_Coborr");
        this.hyperlinkSectionD_Coborr.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionD2_Coborr = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionD2_Coborr");
        this.hyperlinkSectionD2_Coborr.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionE_Coborr = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionE_Coborr");
        this.hyperlinkSectionE_Coborr.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionF_Coborr = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionF_Coborr");
        this.hyperlinkSectionF_Coborr.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionG_Coborr = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionG_Coborr");
        this.hyperlinkSectionG_Coborr.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionH_Coborr = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionH_Coborr");
        this.hyperlinkSectionH_Coborr.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionI_Coborr = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionI_Coborr");
        this.hyperlinkSectionI_Coborr.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionJ_Coborr = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionJ_Coborr");
        this.hyperlinkSectionJ_Coborr.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionK_Coborr = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionK_Coborr");
        this.hyperlinkSectionK_Coborr.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionL_Coborr = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionL_Coborr");
        this.hyperlinkSectionL_Coborr.Click += new EventHandler(this.explanationHyperlink_Click);
        this.hyperlinkSectionM_Coborr = (Hyperlink) this.currentForm.FindControl("hyperlinkSectionM_Coborr");
        this.hyperlinkSectionM_Coborr.Click += new EventHandler(this.explanationHyperlink_Click);
      }
      catch (Exception ex)
      {
      }
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
        case "1063":
          return this.inputData.GetField("1172") == "Other" ? ControlState.Enabled : ControlState.Disabled;
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
              this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "", false);
              if (this.GetField("4244") != "")
                this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "", true);
            }
            else if (flag2)
              this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "", true);
            else
              this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "", false);
          }
          else if (flag2)
            this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "", true);
          else
            this.SetControlState(this.GetField("4142") == "Y" ? "chkBorrRaceNA" : "", false);
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
              this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "", false);
              if (this.GetField("4247") != "")
                this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "", true);
            }
            else if (flag4)
              this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "", true);
            else
              this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "", false);
          }
          else if (flag4)
            this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "", true);
          else
            this.SetControlState(this.GetField("4142") == "Y" ? "chkCoBorrRaceNA" : "", false);
          controlState = ControlState.Default;
          break;
        case "1964":
        case "Constr.Refi":
          if (this.inputData.GetField("19") != "ConstructionOnly" && this.inputData.GetField("19") != "ConstructionToPermanent")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "4084":
          if (this.inputData.GetField("19") != "ConstructionOnly")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "4131":
          bool enabled1 = !(this.GetField("4131") == "FaceToFace");
          this.SetControlState("chkCoBorEthInfoNotProvided", enabled1);
          this.SetControlState("chkCoBorRaceInfoNotProvided", enabled1);
          this.SetControlState("chkCoBorSexInfoNotProvided", enabled1);
          controlState = ControlState.Default;
          break;
        case "4143":
          bool enabled2 = !(this.GetField("4143") == "FaceToFace");
          this.SetControlState("chkBorEthInfoNotProvided", enabled2);
          this.SetControlState("chkBorRaceInfoNotProvided", enabled2);
          this.SetControlState("chkBorSexInfoNotProvided", enabled2);
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
        case "5015":
          controlState = !(this.inputData.GetField("Constr.Refi") == "Y") || !(this.inputData.GetField("19") == "ConstructionOnly") && !(this.inputData.GetField("19") == "ConstructionToPermanent") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "9":
          if (this.inputData.GetField("19") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X134":
          if (this.GetField("URLA.X133") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X174":
        case "URLA.X175":
        case "URLA.X176":
        case "URLA.X177":
          if (this.GetFieldValue("265") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X178":
        case "URLA.X179":
        case "URLA.X180":
        case "URLA.X181":
          if (this.GetFieldValue("266") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X310":
        case "URLA.X311":
        case "URLA.X312":
        case "URLA.X313":
        case "URLA.X314":
          controlState = !(this.inputData.GetField("URLA.X309") == "Y") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "URLA.X76":
          if (this.GetFieldValue("1172") != "FHA")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X82":
          if (this.VOGGDoesNotApply("Borrower"))
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X83":
          if (this.VOGGDoesNotApply("CoBorrower"))
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "voal":
        case "vogg":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        default:
          controlState = ControlState.Default;
          break;
      }
      return controlState;
    }

    private bool VOGGDoesNotApply(string type)
    {
      int ofGiftsAndGrants = this.loan.GetNumberOfGiftsAndGrants();
      for (int index = 1; index <= ofGiftsAndGrants; ++index)
      {
        string field = this.inputData.GetField("URLARGG" + index.ToString("00") + "02");
        if (field == "Both" || field == type)
          return true;
      }
      return false;
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      this.governmentInfoInputHandler.SetLayout((InputHandlerBase) this);
      this.changeLinkVisibility();
      this.changeLinkDisplayName();
    }

    private void changeLinkVisibility()
    {
      this.hyperlinkSectionA.Visible = this.inputData.GetField("418") == "Yes";
      this.hyperlinkSectionB.Visible = this.inputData.GetField("URLA.X84") == "Y";
      this.hyperlinkSectionC.Visible = this.inputData.GetField("URLA.X86") == "Y";
      this.hyperlinkSectionD.Visible = this.inputData.GetField("URLA.X90") == "Y";
      this.hyperlinkSectionD2.Visible = this.inputData.GetField("URLA.X92") == "Y";
      this.hyperlinkSectionE.Visible = this.inputData.GetField("URLA.X94") == "Y";
      this.hyperlinkSectionF.Visible = this.inputData.GetField("URLA.X96") == "Y";
      this.hyperlinkSectionG.Visible = this.inputData.GetField("169") == "Y";
      this.hyperlinkSectionH.Visible = this.inputData.GetField("URLA.X98") == "Y";
      this.hyperlinkSectionI.Visible = this.inputData.GetField("URLA.X100") == "Y";
      this.hyperlinkSectionJ.Visible = this.inputData.GetField("URLA.X102") == "Y";
      this.hyperlinkSectionK.Visible = this.inputData.GetField("URLA.X104") == "Y";
      this.hyperlinkSectionL.Visible = this.inputData.GetField("URLA.X106") == "Y";
      this.hyperlinkSectionM.Visible = this.inputData.GetField("265") == "Y";
      this.hyperlinkSectionA_Coborr.Visible = this.inputData.GetField("1343") == "Yes";
      this.hyperlinkSectionB_Coborr.Visible = this.inputData.GetField("URLA.X85") == "Y";
      this.hyperlinkSectionC_Coborr.Visible = this.inputData.GetField("URLA.X87") == "Y";
      this.hyperlinkSectionD_Coborr.Visible = this.inputData.GetField("URLA.X91") == "Y";
      this.hyperlinkSectionD2_Coborr.Visible = this.inputData.GetField("URLA.X93") == "Y";
      this.hyperlinkSectionE_Coborr.Visible = this.inputData.GetField("URLA.X95") == "Y";
      this.hyperlinkSectionF_Coborr.Visible = this.inputData.GetField("URLA.X97") == "Y";
      this.hyperlinkSectionG_Coborr.Visible = this.inputData.GetField("175") == "Y";
      this.hyperlinkSectionH_Coborr.Visible = this.inputData.GetField("URLA.X99") == "Y";
      this.hyperlinkSectionI_Coborr.Visible = this.inputData.GetField("URLA.X101") == "Y";
      this.hyperlinkSectionJ_Coborr.Visible = this.inputData.GetField("URLA.X103") == "Y";
      this.hyperlinkSectionK_Coborr.Visible = this.inputData.GetField("URLA.X105") == "Y";
      this.hyperlinkSectionL_Coborr.Visible = this.inputData.GetField("URLA.X107") == "Y";
      this.hyperlinkSectionM_Coborr.Visible = this.inputData.GetField("266") == "Y";
    }

    private void changeLinkDisplayName()
    {
      if (this.inputData.GetField("URLA.X216") == string.Empty)
        this.hyperlinkSectionA.Text = "Add Explanation";
      else
        this.hyperlinkSectionA.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X217") == string.Empty)
        this.hyperlinkSectionB.Text = "Add Explanation";
      else
        this.hyperlinkSectionB.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X218") == string.Empty)
        this.hyperlinkSectionC.Text = "Add Explanation";
      else
        this.hyperlinkSectionC.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X219") == string.Empty)
        this.hyperlinkSectionD.Text = "Add Explanation";
      else
        this.hyperlinkSectionD.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X235") == string.Empty)
        this.hyperlinkSectionD2.Text = "Add Explanation";
      else
        this.hyperlinkSectionD2.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X220") == string.Empty)
        this.hyperlinkSectionE.Text = "Add Explanation";
      else
        this.hyperlinkSectionE.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X221") == string.Empty)
        this.hyperlinkSectionF.Text = "Add Explanation";
      else
        this.hyperlinkSectionF.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X222") == string.Empty)
        this.hyperlinkSectionG.Text = "Add Explanation";
      else
        this.hyperlinkSectionG.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X223") == string.Empty)
        this.hyperlinkSectionH.Text = "Add Explanation";
      else
        this.hyperlinkSectionH.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X224") == string.Empty)
        this.hyperlinkSectionI.Text = "Add Explanation";
      else
        this.hyperlinkSectionI.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X225") == string.Empty)
        this.hyperlinkSectionJ.Text = "Add Explanation";
      else
        this.hyperlinkSectionJ.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X226") == string.Empty)
        this.hyperlinkSectionK.Text = "Add Explanation";
      else
        this.hyperlinkSectionK.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X227") == string.Empty)
        this.hyperlinkSectionL.Text = "Add Explanation";
      else
        this.hyperlinkSectionL.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X228") == string.Empty)
        this.hyperlinkSectionM.Text = "Add Explanation";
      else
        this.hyperlinkSectionM.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X271") == string.Empty)
        this.hyperlinkSectionA_Coborr.Text = "Add Explanation";
      else
        this.hyperlinkSectionA_Coborr.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X272") == string.Empty)
        this.hyperlinkSectionB_Coborr.Text = "Add Explanation";
      else
        this.hyperlinkSectionB_Coborr.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X273") == string.Empty)
        this.hyperlinkSectionC_Coborr.Text = "Add Explanation";
      else
        this.hyperlinkSectionC_Coborr.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X274") == string.Empty)
        this.hyperlinkSectionD_Coborr.Text = "Add Explanation";
      else
        this.hyperlinkSectionD_Coborr.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X275") == string.Empty)
        this.hyperlinkSectionD2_Coborr.Text = "Add Explanation";
      else
        this.hyperlinkSectionD2_Coborr.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X276") == string.Empty)
        this.hyperlinkSectionE_Coborr.Text = "Add Explanation";
      else
        this.hyperlinkSectionE_Coborr.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X277") == string.Empty)
        this.hyperlinkSectionF_Coborr.Text = "Add Explanation";
      else
        this.hyperlinkSectionF_Coborr.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X278") == string.Empty)
        this.hyperlinkSectionG_Coborr.Text = "Add Explanation";
      else
        this.hyperlinkSectionG_Coborr.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X279") == string.Empty)
        this.hyperlinkSectionH_Coborr.Text = "Add Explanation";
      else
        this.hyperlinkSectionH_Coborr.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X280") == string.Empty)
        this.hyperlinkSectionI_Coborr.Text = "Add Explanation";
      else
        this.hyperlinkSectionI_Coborr.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X281") == string.Empty)
        this.hyperlinkSectionJ_Coborr.Text = "Add Explanation";
      else
        this.hyperlinkSectionJ_Coborr.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X282") == string.Empty)
        this.hyperlinkSectionK_Coborr.Text = "Add Explanation";
      else
        this.hyperlinkSectionK_Coborr.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X283") == string.Empty)
        this.hyperlinkSectionL_Coborr.Text = "Add Explanation";
      else
        this.hyperlinkSectionL_Coborr.Text = "Edit Explanation";
      if (this.inputData.GetField("URLA.X284") == string.Empty)
        this.hyperlinkSectionM_Coborr.Text = "Add Explanation";
      else
        this.hyperlinkSectionM_Coborr.Text = "Edit Explanation";
    }

    private void explanationHyperlink_Click(object sender, EventArgs e)
    {
      Hyperlink hyperlink = (Hyperlink) sender;
      string controlId = hyperlink.ControlID;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(controlId))
      {
        case 63499240:
          if (!(controlId == "hyperlinkSectionH_Coborr"))
            return;
          break;
        case 240509676:
          if (!(controlId == "hyperlinkSectionL_Coborr"))
            return;
          break;
        case 502593003:
          if (!(controlId == "hyperlinkSectionK_Coborr"))
            return;
          break;
        case 546623950:
          if (!(controlId == "hyperlinkSectionB_Coborr"))
            return;
          break;
        case 704432848:
          if (!(controlId == "hyperlinkSectionD"))
            return;
          break;
        case 721210467:
          if (!(controlId == "hyperlinkSectionE"))
            return;
          break;
        case 737988086:
          if (!(controlId == "hyperlinkSectionF"))
            return;
          break;
        case 754765705:
          if (!(controlId == "hyperlinkSectionG"))
            return;
          break;
        case 788320943:
          if (!(controlId == "hyperlinkSectionA"))
            return;
          break;
        case 805098562:
          if (!(controlId == "hyperlinkSectionB"))
            return;
          break;
        case 821876181:
          if (!(controlId == "hyperlinkSectionC"))
            return;
          break;
        case 838653800:
          if (!(controlId == "hyperlinkSectionL"))
            return;
          break;
        case 855431419:
          if (!(controlId == "hyperlinkSectionM"))
            return;
          break;
        case 905764276:
          if (!(controlId == "hyperlinkSectionH"))
            return;
          break;
        case 922541895:
          if (!(controlId == "hyperlinkSectionI"))
            return;
          break;
        case 939319514:
          if (!(controlId == "hyperlinkSectionJ"))
            return;
          break;
        case 956097133:
          if (!(controlId == "hyperlinkSectionK"))
            return;
          break;
        case 984733457:
          if (!(controlId == "hyperlinkSectionE_Coborr"))
            return;
          break;
        case 1907442598:
          if (!(controlId == "hyperlinkSectionJ_Coborr"))
            return;
          break;
        case 2239296055:
          if (!(controlId == "hyperlinkSectionG_Coborr"))
            return;
          break;
        case 2436704754:
          if (!(controlId == "hyperlinkSectionF_Coborr"))
            return;
          break;
        case 3088868837:
          if (!(controlId == "hyperlinkSectionI_Coborr"))
            return;
          break;
        case 3199797453:
          if (!(controlId == "hyperlinkSectionA_Coborr"))
            return;
          break;
        case 3216645250:
          if (!(controlId == "hyperlinkSectionD2_Coborr"))
            return;
          break;
        case 3366137555:
          if (!(controlId == "hyperlinkSectionC_Coborr"))
            return;
          break;
        case 3761310249:
          if (!(controlId == "hyperlinkSectionM_Coborr"))
            return;
          break;
        case 3789352724:
          if (!(controlId == "hyperlinkSectionD_Coborr"))
            return;
          break;
        case 4210254278:
          if (!(controlId == "hyperlinkSectionD2"))
            return;
          break;
        default:
          return;
      }
      this.showExplanationDialog(hyperlink.ControlID);
      this.changeLinkDisplayName();
    }

    private void declarations_Change(object sender, EventArgs e)
    {
      DropdownBox dropdownBox = (DropdownBox) sender;
      if (!(dropdownBox.Value == "No") && !(dropdownBox.Value == "N") && !(dropdownBox.Value == ""))
        return;
      string controlId = dropdownBox.ControlID;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(controlId))
      {
        case 764528056:
          if (!(controlId == "DropdownBox9"))
            break;
          this.editExplanationDialog("URLA.X216", "URLA.X247", dropdownBox.Field.FieldID, "418");
          break;
        case 879121456:
          if (!(controlId == "DropdownBox10"))
            break;
          this.editExplanationDialog("URLA.X271", "URLA.X285", dropdownBox.Field.FieldID, "1343");
          break;
        case 996564789:
          if (!(controlId == "DropdownBox17"))
            break;
          this.editExplanationDialog("URLA.X217", "URLA.X248", dropdownBox.Field.FieldID, "URLA.X84");
          break;
        case 1013342408:
          if (!(controlId == "DropdownBox18"))
            break;
          this.editExplanationDialog("URLA.X272", "URLA.X286", dropdownBox.Field.FieldID, "URLA.X85");
          break;
        case 1030120027:
          if (!(controlId == "DropdownBox19"))
            break;
          this.editExplanationDialog("URLA.X218", "URLA.X249", dropdownBox.Field.FieldID, "URLA.X86");
          break;
        case 3127616592:
          if (!(controlId == "DropdownBox36"))
            break;
          this.editExplanationDialog("URLA.X281", "URLA.X295", dropdownBox.Field.FieldID, "URLA.X103");
          break;
        case 3161171830:
          if (!(controlId == "DropdownBox34"))
            break;
          this.editExplanationDialog("URLA.X280", "URLA.X294", dropdownBox.Field.FieldID, "URLA.X101");
          break;
        case 3177949449:
          if (!(controlId == "DropdownBox35"))
            break;
          this.editExplanationDialog("URLA.X225", "URLA.X257", dropdownBox.Field.FieldID, "URLA.X102");
          break;
        case 3178096544:
          if (!(controlId == "DropdownBox29"))
            break;
          this.editExplanationDialog("URLA.X278", "URLA.X292", dropdownBox.Field.FieldID, "175");
          break;
        case 3194727068:
          if (!(controlId == "DropdownBox32"))
            break;
          this.editExplanationDialog("URLA.X279", "URLA.X293", dropdownBox.Field.FieldID, "URLA.X99");
          break;
        case 3194874163:
          if (!(controlId == "DropdownBox28"))
            break;
          this.editExplanationDialog("URLA.X277", "URLA.X291", dropdownBox.Field.FieldID, "URLA.X97");
          break;
        case 3211504687:
          if (!(controlId == "DropdownBox33"))
            break;
          this.editExplanationDialog("URLA.X224", "URLA.X256", dropdownBox.Field.FieldID, "URLA.X100");
          break;
        case 3228282306:
          if (!(controlId == "DropdownBox30"))
            break;
          this.editExplanationDialog("URLA.X222", "URLA.X254", dropdownBox.Field.FieldID, "169");
          break;
        case 3245059925:
          if (!(controlId == "DropdownBox31"))
            break;
          this.editExplanationDialog("URLA.X223", "URLA.X255", dropdownBox.Field.FieldID, "URLA.X98");
          break;
        case 3312317496:
          if (!(controlId == "DropdownBox21"))
            break;
          this.editExplanationDialog("URLA.X219", "URLA.X250", dropdownBox.Field.FieldID, "URLA.X90");
          break;
        case 3329095115:
          if (!(controlId == "DropdownBox20"))
            break;
          this.editExplanationDialog("URLA.X273", "URLA.X287", dropdownBox.Field.FieldID, "URLA.X87");
          break;
        case 3345872734:
          if (!(controlId == "DropdownBox23"))
            break;
          this.editExplanationDialog("URLA.X235", "URLA.X251", dropdownBox.Field.FieldID, "URLA.X92");
          break;
        case 3362650353:
          if (!(controlId == "DropdownBox22"))
            break;
          this.editExplanationDialog("URLA.X274", "URLA.X288", dropdownBox.Field.FieldID, "URLA.X91");
          break;
        case 3379280877:
          if (!(controlId == "DropdownBox39"))
            break;
          this.editExplanationDialog("URLA.X226", "URLA.X258", dropdownBox.Field.FieldID, "URLA.X104");
          break;
        case 3379427972:
          if (!(controlId == "DropdownBox25"))
            break;
          this.editExplanationDialog("URLA.X220", "URLA.X252", dropdownBox.Field.FieldID, "URLA.X94");
          break;
        case 3396205591:
          if (!(controlId == "DropdownBox24"))
            break;
          this.editExplanationDialog("URLA.X275", "URLA.X289", dropdownBox.Field.FieldID, "URLA.X93");
          break;
        case 3412983210:
          if (!(controlId == "DropdownBox27"))
            break;
          this.editExplanationDialog("URLA.X221", "URLA.X253", dropdownBox.Field.FieldID, "URLA.X96");
          break;
        case 3429760829:
          if (!(controlId == "DropdownBox26"))
            break;
          this.editExplanationDialog("URLA.X276", "URLA.X290", dropdownBox.Field.FieldID, "URLA.X95");
          break;
        case 3463610257:
          if (!(controlId == "DropdownBox44"))
            break;
          this.editExplanationDialog("URLA.X284", "URLA.X298", dropdownBox.Field.FieldID, "266");
          break;
        case 3480387876:
          if (!(controlId == "DropdownBox43"))
            break;
          this.editExplanationDialog("URLA.X228", "URLA.X260", dropdownBox.Field.FieldID, "265");
          break;
        case 3497165495:
          if (!(controlId == "DropdownBox42"))
            break;
          this.editExplanationDialog("URLA.X283", "URLA.X297", dropdownBox.Field.FieldID, "URLA.X107");
          break;
        case 3513943114:
          if (!(controlId == "DropdownBox41"))
            break;
          this.editExplanationDialog("URLA.X227", "URLA.X259", dropdownBox.Field.FieldID, "URLA.X106");
          break;
        case 3530720733:
          if (!(controlId == "DropdownBox40"))
            break;
          this.editExplanationDialog("URLA.X282", "URLA.X296", dropdownBox.Field.FieldID, "URLA.X105");
          break;
      }
    }

    private void editExplanationDialog(
      string explanationfieldid,
      string printFieldId,
      string dropdownfieldId,
      string fieldid)
    {
      if (string.IsNullOrEmpty(this.GetField(explanationfieldid)) || !(this.GetField(fieldid) != "Y") || !(this.GetField(fieldid) != "Yes"))
        return;
      if (DialogResult.Yes == Utils.Dialog((IWin32Window) this.mainScreen, "Do you wish to delete the associated explanation?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
      {
        this.inputData.SetField(explanationfieldid, string.Empty);
        this.inputData.SetField(printFieldId, "N");
      }
      else if (dropdownfieldId == "418" || dropdownfieldId == "1343")
        this.inputData.SetField(dropdownfieldId, "Yes");
      else
        this.inputData.SetField(dropdownfieldId, "Y");
      this.changeLinkDisplayName();
      this.changeLinkVisibility();
    }

    private void showExplanationDialog(string section)
    {
      string sectionID;
      switch (section)
      {
        case "hyperlinkSectionD2":
          sectionID = "D2";
          break;
        case "hyperlinkSectionD2_Coborr":
          sectionID = "D2 (Coborrower)";
          break;
        default:
          if (section != "hyperlinkSectionD2_Coborr" && section.Contains("_"))
          {
            string[] strArray = section.Split('_');
            sectionID = strArray[0].Substring(strArray[0].Length - 1, 1) + " (Coborrower)";
            break;
          }
          sectionID = section.Substring(section.Length - 1, 1);
          break;
      }
      using (AddExplanationDialog explanationDialog = new AddExplanationDialog(this.inputData, sectionID, this.session))
      {
        if (explanationDialog.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
          return;
        this.RefreshContents();
      }
    }
  }
}

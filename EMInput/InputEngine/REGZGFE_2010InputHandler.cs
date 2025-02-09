// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.REGZGFE_2010InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class REGZGFE_2010InputHandler : InputHandlerBase
  {
    private bool canEditPayment = true;
    private POCInputHandler pocInputHandler;
    private LOCompensationInputHandler loCompensationInputHandler;
    private List<RuntimeControl> panel_Exp_Controls;
    private IWin32Window owner;
    public LoanData ccLoan;

    public REGZGFE_2010InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public REGZGFE_2010InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
      this.canEditPayment = this.HasExclusiveRights(false);
    }

    public REGZGFE_2010InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public REGZGFE_2010InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public REGZGFE_2010InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput htmlInput,
      HTMLDocument htmlDoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, htmlInput, htmlDoc, form, property)
    {
      if (this.inputData is DisclosedItemizationHandler)
        return;
      string empty = string.Empty;
      try
      {
        FileStream fileStream = File.OpenRead(AssemblyResolver.GetResourceFileFullPath("Documents\\Templates\\BlankLoan\\BlankData.XML", SystemSettings.LocalAppDir));
        byte[] numArray = new byte[fileStream.Length];
        fileStream.Read(numArray, 0, numArray.Length);
        fileStream.Close();
        empty = Encoding.ASCII.GetString(numArray);
      }
      catch (Exception ex)
      {
      }
      ILoanConfigurationInfo configurationInfo = this.session.SessionObjects.LoanManager.GetLoanConfigurationInfo();
      this.ccLoan = new LoanData(empty, configurationInfo.LoanSettings, false);
      this.SetClosingCostLoan(false, (string) null);
      ClosingCost inputData = (ClosingCost) this.inputData;
      if (inputData.RESPAVersion == "2015")
      {
        this.ccLoan.SetField("3969", "RESPA-TILA 2015 LE and CD");
        this.ccLoan.SetField("NEWHUD.X1139", "Y");
        this.ccLoan.SetField("NEWHUD.X713", "");
      }
      else if (inputData.RESPAVersion == "2010" || inputData.For2010GFE)
        this.ccLoan.SetField("3969", "RESPA 2010 GFE and HUD-1");
      else
        this.ccLoan.SetField("3969", "Old GFE and HUD-1");
      LoanCalculator loanCalculator = new LoanCalculator(this.session.SessionObjects, configurationInfo, this.ccLoan);
      this.ccLoan.IsTemplate = true;
      this.ccLoan.IgnoreValidationErrors = true;
      this.ccLoan.Calculator.SkipFieldChangeEvent = true;
      if (this.inputData.GetField("RecalculationRequired") == "Y")
      {
        this.ccLoan.Calculator.FormCalculation("REGZGFE_2010", (string) null, (string) null);
        this.SetTemplate();
        this.RefreshContents();
      }
      this.inputData.SetField("RecalculationRequired", "");
      this.inputData.CleanField("RecalculationRequired");
    }

    public REGZGFE_2010InputHandler(
      IWin32Window owner,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, input, htmldoc, form, property)
    {
      this.owner = owner;
    }

    internal override void CreateControls()
    {
      try
      {
        if (this is REGZGFE_2015InputHandler)
        {
          if (this.inputData is ClosingCost)
          {
            ImageButton control = (ImageButton) this.currentForm.FindControl("ImageButton2");
            if (control != null)
              control.Visible = false;
          }
          this.pocInputHandler = new POCInputHandler(this.inputData, this.currentForm, true);
        }
        else
          this.pocInputHandler = new POCInputHandler(this.inputData, this.currentForm, (InputHandlerBase) this, this.session);
        this.loCompensationInputHandler = new LOCompensationInputHandler(this.loCompensationSetting, this.inputData, this.currentForm, (InputHandlerBase) this);
        if (this.session.EncompassEdition != EncompassEdition.Banker)
        {
          EllieMae.Encompass.Forms.CheckBox control = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chkUseLOTool");
          if (control != null)
            control.Visible = false;
        }
        this.panel_Exp_Controls = SharedURLAUIHandler.GetControls(this.currentForm, "panel_Exp_2009", "panel_Exp_2020");
      }
      catch (Exception ex)
      {
      }
    }

    protected override void UpdateContents(bool refreshAllFields)
    {
      base.UpdateContents(refreshAllFields);
      this.setFeeManagementAccessToControl();
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      if (this.pocInputHandler != null)
        this.pocInputHandler.SetFieldLock819Status();
      this.setFeeManagementAccessToControl();
      if (this.panel_Exp_Controls != null)
        SharedURLAUIHandler.DisplayControls(this.loan == null || this.loan.Use2020URLA, this.panel_Exp_Controls);
      if (this is REGZGFE_2015InputHandler || this is REGZGFE_2015_DETAILSInputHandler || this.inputData is DisclosedItemizationHandler || this.loCompensationSetting == null)
        return;
      LOCompensationInputHandler.CheckLOCompRuleConfliction(this.loCompensationSetting, this.inputData, (string) null, (string) null, (string) null, true);
    }

    private void setFeeManagementAccessToControl()
    {
      int num1 = (int) this.setControlButtonRightForFeeManagement("FieldLock1", "454");
      int num2 = (int) this.setControlButtonRightForFeeManagement("FieldLock8", "NEWHUD2.X5");
      int num3 = (int) this.setControlButtonRightForFeeManagement("FieldLock10", "NEWHUD2.X6");
      int num4 = (int) this.setControlButtonRightForFeeManagement("FieldLock11", "NEWHUD.X1155");
      int num5 = (int) this.setControlButtonRightForFeeManagement("FieldLock12", "NEWHUD.X1159");
      int num6 = (int) this.setControlButtonRightForFeeManagement("FieldLock13", "NEWHUD.X1163");
      int num7 = (int) this.setControlButtonRightForFeeManagement("FieldLock2", "L245");
      int num8 = (int) this.setControlButtonRightForFeeManagement("FieldLock3", "337");
      int num9 = (int) this.setControlButtonRightForFeeManagement("FieldLock4", "642");
      int num10 = (int) this.setControlButtonRightForFeeManagement("FieldLock29", "NEWHUD.X591");
      int num11 = (int) this.setControlButtonRightForFeeManagement("FieldLock5", "1050");
      int num12 = (int) this.setControlButtonRightForFeeManagement("FieldLock30", "643");
      int num13 = (int) this.setControlButtonRightForFeeManagement("FieldLock31", "L260");
      int num14 = (int) this.setControlButtonRightForFeeManagement("FieldLock32", "1667");
      int num15 = (int) this.setControlButtonRightForFeeManagement("FieldLock33", "NEWHUD.X592");
      int num16 = (int) this.setControlButtonRightForFeeManagement("FieldLock34", "NEWHUD.X593");
      int num17 = (int) this.setControlButtonRightForFeeManagement("FieldLock35", "NEWHUD.X1588");
      int num18 = (int) this.setControlButtonRightForFeeManagement("FieldLock36", "NEWHUD.X1596");
      int num19 = (int) this.setControlButtonRightForFeeManagement("FieldLock7", new string[1]
      {
        "338"
      }, new string[1]{ "232" });
      int num20 = (int) this.setControlButtonRightForFeeManagement("FieldLockAggregateAdjust", "558");
      int num21 = (int) this.setControlButtonRightForFeeManagement("StandardButton11", new string[2]
      {
        "373",
        "374"
      }, (string[]) null);
      int num22 = (int) this.setControlButtonRightForFeeManagement("StandardButton7", new string[2]
      {
        "1640",
        "1641"
      }, (string[]) null);
      int num23 = (int) this.setControlButtonRightForFeeManagement("StandardButton9", new string[2]
      {
        "1643",
        "1644"
      }, (string[]) null);
      int num24 = (int) this.setControlButtonRightForFeeManagement("FieldLock42", "HUD53");
      int num25 = (int) this.setControlButtonRightForFeeManagement("FieldLock22", "HUD54");
      int num26 = (int) this.setControlButtonRightForFeeManagement("FieldLock23", "HUD52");
      int num27 = (int) this.setControlButtonRightForFeeManagement("FieldLock37", "HUD56");
      int num28 = (int) this.setControlButtonRightForFeeManagement("FieldLock38", "HUD55");
      int num29 = (int) this.setControlButtonRightForFeeManagement("FieldLock25", "HUD58");
      int num30 = (int) this.setControlButtonRightForFeeManagement("FieldLock41", "HUD60");
      int num31 = (int) this.setControlButtonRightForFeeManagement("FieldLock40", "HUD62");
      int num32 = (int) this.setControlButtonRightForFeeManagement("FieldLock39", "HUD63");
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      string fieldValue = this.GetFieldValue(id);
      switch (id)
      {
        case "1620":
          if (this.inputData is ClosingCost)
          {
            this.setField("SYS.X265", "", false);
            if (this.GetFieldValue("389") != string.Empty || val != string.Empty)
            {
              this.setField("SYS.X266", "Broker", false);
              break;
            }
            this.setField("SYS.X266", "", false);
            break;
          }
          break;
        case "1663":
        case "230":
        case "232":
        case "NEWHUD.X39":
        case "NEWHUD.X572":
          double num1 = Utils.ParseDouble((object) val);
          if (num1 != 0.0 || val == "0" || val == "." || val.IndexOf("0.") > -1 || val.IndexOf(".0") > -1)
          {
            val = num1.ToString("N2");
            break;
          }
          break;
        case "1847":
          double num2 = Utils.ParseDouble((object) val);
          if (num2 != 0.0)
          {
            val = num2.ToString("N3");
            break;
          }
          break;
        case "389":
          if (this.inputData is ClosingCost)
          {
            this.setField("SYS.X265", "", false);
            if (val != string.Empty || this.GetFieldValue("1620") != string.Empty)
            {
              this.setField("SYS.X266", "Broker", false);
              break;
            }
            this.setField("SYS.X266", "", false);
            break;
          }
          break;
        case "NEWHUD.X223":
        case "NEWHUD.X224":
          if (this.inputData is ClosingCost)
          {
            if (this.GetFieldValue("NEWHUD.X223") != string.Empty || this.GetFieldValue("NEWHUD.X224") != string.Empty)
            {
              this.setField("NEWHUD.X227", "Lender", false);
              this.setField("NEWHUD.X230", "Broker", false);
              this.setField("NEWHUD2.X109", this.GetFieldValue("VEND.X293"), false);
              break;
            }
            this.setField("NEWHUD.X227", "", false);
            this.setField("NEWHUD.X230", "", false);
            break;
          }
          break;
      }
      this.setField(id, val, true);
      if (this.loCompensationSetting != null)
        LOCompensationInputHandler.CheckLOCompRuleConfliction(this.loCompensationSetting, this.inputData is ClosingCost ? (IHtmlInput) this.ccLoan : this.inputData, id, val, fieldValue, true);
      switch (id)
      {
        case "1620":
        case "389":
          if (this.inputData is ClosingCost)
          {
            this.setField("SYS.X265", "", false);
            if (this.GetFieldValue("389") != string.Empty || this.GetFieldValue("1620") != string.Empty)
            {
              this.setField("SYS.X266", "Broker", false);
              break;
            }
            this.setField("SYS.X266", "", false);
            break;
          }
          break;
        case "1663":
          if (this.IsUsingTemplate && val != string.Empty)
          {
            this.setField("1847", "", true);
            this.setField("NEWHUD.X734", "", true);
            break;
          }
          break;
        case "1847":
        case "NEWHUD.X734":
          if (this.IsUsingTemplate && val != string.Empty)
          {
            this.setField("1663", "", true);
            break;
          }
          break;
        case "388":
          if (this.IsUsingTemplate && this.loan == null && val != string.Empty)
          {
            this.setField("454", "", true);
            break;
          }
          break;
        case "454":
          if (this.IsUsingTemplate && this.loan == null && val != string.Empty)
          {
            this.setField("388", "", false);
            break;
          }
          break;
        case "NEWHUD.X1718":
          if (this.inputData is ClosingCost && val == "Y")
          {
            this.setField("NEWHUD.X223", "", true);
            this.setField("NEWHUD.X224", "", true);
            this.setField("NEWHUD.X225", "", true);
            this.setField("NEWHUD.X1141", "", true);
            this.setField("NEWHUD.X1225", "", true);
            this.setField("NEWHUD.X1142", "", true);
            this.setField("NEWHUD.X230", "", true);
            this.setField("NEWHUD.X227", "", true);
            this.setField("NEWHUD.X1167", "", true);
            this.setField("NEWHUD.X1168", "", true);
            this.setField("NEWHUD.X1139", "Y", true);
            break;
          }
          break;
        case "NEWHUD.X223":
        case "NEWHUD.X224":
          if (this.IsUsingTemplate)
          {
            if (Utils.ParseDouble((object) this.GetFieldValue("NEWHUD.X224")) != 0.0 || Utils.ParseDouble((object) this.GetFieldValue("NEWHUD.X223")) != 0.0)
            {
              this.setField("NEWHUD.X227", "Lender", true);
              this.setField("NEWHUD.X230", "Broker", true);
              this.setField("NEWHUD2.X109", this.GetFieldValue("VEND.X293"), true);
              break;
            }
            this.setField("NEWHUD.X227", "", true);
            this.setField("NEWHUD.X230", "", true);
            break;
          }
          break;
        case "NEWHUD.X351":
          if (val != "Y")
          {
            this.setField("NEWHUD.X78", "", false);
            break;
          }
          break;
        case "NEWHUD.X572":
          if (Utils.ParseDouble((object) val) < Utils.ParseDouble((object) "NEWHUD.X1724"))
          {
            this.setField("NEWHUD.X1724", val, true);
            break;
          }
          break;
        case "NEWHUD.X639":
          if (Utils.ParseDouble((object) val) < Utils.ParseDouble((object) this.GetFieldValue("NEWHUD.X1725")))
          {
            this.setField("NEWHUD.X1725", val, true);
            break;
          }
          break;
        case "NEWHUD.X715":
          if (this.IsUsingTemplate)
          {
            switch (val)
            {
              case "Include Origination Credit":
                this.setField("1061", "", true);
                this.setField("436", "", true);
                this.setField("3119", "", true);
                this.setField("NEWHUD.X788", "", true);
                this.setField("NEWHUD.X820", "", true);
                break;
              case "Include Origination Points":
                this.setField("1847", "", true);
                this.setField("NEWHUD.X734", "", true);
                this.setField("1663", "", true);
                break;
              default:
                this.setField("1061", "", true);
                this.setField("436", "", true);
                this.setField("3119", "", true);
                this.setField("1847", "", true);
                this.setField("NEWHUD.X734", "", true);
                this.setField("1663", "", true);
                this.setField("NEWHUD.X788", "", true);
                this.setField("NEWHUD.X749", "", true);
                break;
            }
          }
          else
            break;
          break;
        case "NEWHUD.X804":
          if (val != "Affiliate")
          {
            this.setField("NEWHUD.X1724", "", true);
            break;
          }
          break;
        case "NEWHUD.X805":
          if (val != "Affiliate")
          {
            this.setField("NEWHUD.X1725", "", true);
            break;
          }
          break;
      }
      if (this.inputData is ClosingCost && (id == "NEWHUD.X715" || id == "1847" || id == "NEWHUD.X734") && this.GetFieldValue("NEWHUD.X1139") != "Y" && this.GetFieldValue("NEWHUD.X715") == "Include Origination Credit" && (this.GetFieldValue("1847") != string.Empty || this.GetFieldValue("NEWHUD.X734") != string.Empty))
      {
        this.setField("NEWHUD.X749", "Lender", false);
        this.setField("NEWHUD.X353", "", false);
      }
      this.SetPropertyInsuranceTaxes(id, val);
      if (this is REGZGFE_2015_DETAILSInputHandler)
      {
        this.SetTemplate();
      }
      else
      {
        this.RunCalculation(id, val);
        if (!(this.inputData is ClosingCost))
          return;
        this.SetTemplate();
      }
    }

    private void setField(string id, string val, bool useBaseSet)
    {
      if (useBaseSet)
        base.UpdateFieldValue(id, val);
      else
        this.inputData.SetCurrentField(id, val);
      if (this.ccLoan == null)
        return;
      this.ccLoan.SetField(id, val);
    }

    public bool IsFieldLocked(string id)
    {
      return this.ccLoan != null ? this.ccLoan.IsLocked(id) : this.inputData.IsLocked(id);
    }

    protected override string GetFieldValue(string id)
    {
      return this.ccLoan != null ? this.ccLoan.GetField(id) : base.GetFieldValue(id);
    }

    internal void SetPropertyInsuranceTaxes(string id, string val)
    {
      switch (id)
      {
        case "NEWHUD2.X124":
        case "NEWHUD2.X125":
        case "NEWHUD2.X126":
        case "1630":
          this.checkPropertyInsuranceTaxes(id, val, "NEWHUD2.X124", "NEWHUD2.X125", "NEWHUD2.X126", "1630");
          break;
        case "NEWHUD2.X127":
        case "NEWHUD2.X128":
        case "NEWHUD2.X129":
        case "253":
          this.checkPropertyInsuranceTaxes(id, val, "NEWHUD2.X127", "NEWHUD2.X128", "NEWHUD2.X129", "253");
          break;
        case "NEWHUD2.X130":
        case "NEWHUD2.X131":
        case "NEWHUD2.X132":
        case "254":
          this.checkPropertyInsuranceTaxes(id, val, "NEWHUD2.X130", "NEWHUD2.X131", "NEWHUD2.X132", "254");
          break;
      }
    }

    private void checkPropertyInsuranceTaxes(
      string id,
      string val,
      string propertyID,
      string insuranceID,
      string otherID,
      string amountID)
    {
      if (id == propertyID && val == "Y")
      {
        this.setField(insuranceID, "N", true);
        this.setField(otherID, "N", true);
      }
      else if (id == insuranceID && val == "Y")
      {
        this.setField(propertyID, "N", true);
        this.setField(otherID, "N", true);
      }
      else if (id == otherID && val == "Y")
      {
        this.setField(propertyID, "N", true);
        this.setField(insuranceID, "N", true);
      }
      if (!(val != "Y") || !(id == propertyID) && !(id == insuranceID) && !(id == otherID) && !(id == amountID) || !(base.GetFieldValue(propertyID) != "Y") || !(base.GetFieldValue(insuranceID) != "Y") || !(base.GetFieldValue(otherID) != "Y") || !(base.GetFieldValue(amountID) != ""))
        return;
      this.setField(otherID, "Y", true);
    }

    public void RunCalculation(string id, string val)
    {
      if (this.loan != null)
      {
        this.loan.Calculator.FormCalculation("UpdateCityStateUserFees", id, val);
        switch (this)
        {
          case REGZGFE_2015InputHandler _:
          case REGZGFE_2015_DETAILSInputHandler _:
            if (!(this is REGZGFE_2015_DETAILSInputHandler) && this is REGZGFE_2015InputHandler)
              this.loan.Calculator.Calculate2015FeeDetails(id, (string) null, false);
            this.loan.Calculator.CopyHUD2010ToGFE2010(id, false);
            this.loan.Calculator.FormCalculation("REGZGFE_2010", id, id != null ? this.GetField(id) : (string) null);
            break;
          default:
            this.loan.Calculator.CopyHUD2010ToGFE2010(id, false);
            this.loan.Calculator.FormCalculation("REGZGFE_2010", id, this.GetField(id));
            break;
        }
        this.loan.Calculator.CopyGFEToMLDS(id);
      }
      else if (this.inputData is ClosingCost)
      {
        this.ccLoan.Calculator.FormCalculation("UpdateCityStateUserFees", id, val);
        switch (this)
        {
          case REGZGFE_2015_DETAILSInputHandler _:
          case null:
            this.ccLoan.Calculator.FormCalculation("REGZGFE_2010", id, val);
            this.ccLoan.Calculator.FormCalculation("EXPORTUCD", (string) null, (string) null);
            break;
          case REGZGFE_2015InputHandler _:
            this.ccLoan.Calculator.Calculate2015FeeDetails(id);
            goto case null;
          default:
            this.ccLoan.Calculator.CopyHUD2010ToGFE2010(id, false);
            goto case null;
        }
      }
      else
        this.adjustPTCPOCAmounts(id);
      if (this.loCompensationInputHandler == null)
        return;
      this.loCompensationInputHandler.RefreshContents();
    }

    private void adjustPTCPOCAmounts(string id)
    {
      bool flag = Utils.CheckIf2015RespaTila(this.inputData.GetField("3969"));
      for (int index1 = 0; index1 < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index1)
      {
        string[] strArray = HUDGFE2010Fields.WHOLEPOC_FIELDS[index1];
        if (flag || !(strArray[HUDGFE2010Fields.PTCPOCINDEX_FOR2015] == "Y"))
        {
          for (int index2 = 0; index2 < strArray.Length; ++index2)
          {
            if (string.Compare(strArray[index2], id, true) == 0)
            {
              double num1 = Utils.ParseDouble((object) this.inputData.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]));
              double num2 = Utils.ParseDouble((object) this.inputData.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]));
              double num3 = Utils.ParseDouble((object) this.inputData.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]));
              double num4 = Utils.ParseDouble((object) this.inputData.GetField(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY]));
              if (num1 != Utils.ArithmeticRounding(num2 + num3 + num4, 2))
              {
                double num5;
                if (num1 < num2)
                {
                  num2 = num1;
                  num3 = 0.0;
                  num5 = 0.0;
                }
                if (num1 < Utils.ArithmeticRounding(num2 + num3, 2))
                {
                  num3 = num1 - num2;
                  num5 = 0.0;
                }
                double num6 = num1 - num2 - num3;
                if (num1 < 0.0)
                  num6 = 0.0;
                this.setField(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT], num2 != 0.0 ? num2.ToString("N2") : "", false);
                this.setField(strArray[HUDGFE2010Fields.PTCPOCINDEX_POC], num2 != 0.0 ? "Y" : "", false);
                if (num2 == 0.0)
                  this.setField(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY], "", false);
                this.setField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT], num3 != 0.0 ? num3.ToString("N2") : "", false);
                this.setField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTC], num3 != 0.0 ? "Y" : "", false);
                if (num3 == 0.0)
                  this.setField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], "", false);
                this.setField(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY], num6 != 0.0 ? num6.ToString("N2") : "", false);
                if (num6 == 0.0)
                {
                  this.setField(strArray[HUDGFE2010Fields.PTCPOCINDEX_APR], "", false);
                  this.setField(strArray[HUDGFE2010Fields.PTCPOCINDEX_FINANCED], "", false);
                }
              }
              if (num3 != 0.0 && id == strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY])
              {
                this.setField(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY], this.inputData.GetField(id), false);
                break;
              }
              break;
            }
          }
        }
      }
    }

    public override void RefreshContents()
    {
      base.RefreshContents();
      if (this.loCompensationInputHandler != null)
        this.loCompensationInputHandler.RefreshContents();
      if (this.pocInputHandler == null)
        return;
      this.pocInputHandler.SetFieldLock819Status();
    }

    public override void onblur(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.loan != null && this.pocInputHandler != null)
        this.pocInputHandler.TurnOffSellerPaidWarning();
      base.onblur(pEvtObj);
    }

    public override bool onkeypress(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.loan != null && this.pocInputHandler != null)
        this.pocInputHandler.TurnOnSellerPaidWarning(pEvtObj);
      return base.onkeypress(pEvtObj);
    }

    internal override void FlipLockField(FieldLock fieldLock)
    {
      base.FlipLockField(fieldLock);
      if (this.loan != null)
      {
        this.loan.Calculator.CopyHUD2010ToGFE2010((fieldLock.ControlToLock as FieldControl).Field.FieldID, false);
        this.loan.Calculator.FormCalculation("REGZGFE_2010", (string) null, (string) null);
      }
      else
      {
        if (!(this.inputData is ClosingCost) || this.ccLoan == null || !(fieldLock.ControlToLock is FieldControl controlToLock))
          return;
        if (fieldLock.Locked)
          this.ccLoan.AddLock(controlToLock.Field.FieldID);
        else
          this.ccLoan.RemoveLock(controlToLock.Field.FieldID);
        if (controlToLock.Field.FieldID == "454")
        {
          if (fieldLock.Locked)
            this.ccLoan.AddLock("NEWHUD.X770");
          else
            this.ccLoan.RemoveLock("NEWHUD.X770");
        }
        if (!fieldLock.Locked)
        {
          string id = controlToLock.Field.FieldID;
          if (controlToLock.Field.FieldID == "NEWHUD2.X5")
            id = "NEWHUD2.X1";
          if (controlToLock.Field.FieldID == "NEWHUD2.X6")
            id = "NEWHUD2.X2";
          this.ccLoan.TriggerCalculation(id, this.GetFieldValue(id));
          this.ccLoan.Calculator.FormCalculation("REGZGFE_2010", id, this.GetFieldValue(id));
        }
        this.SetTemplate();
        if (!(this is REGZGFE_2015InputHandler))
          return;
        ((REGZGFE_2015InputHandler) this).RefreshFeeDetails();
      }
    }

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement && (controlForElement.Field.FieldID == "NEWHUD.X572" || controlForElement.Field.FieldID == "NEWHUD.X39"))
      {
        bool needsUpdate = false;
        if (controlForElement.Value.ToLower() != "n" && controlForElement.Value.ToLower() != "na" || this.GetFieldValue("420") != "SecondLien" && this.GetFieldValue("19").IndexOf("Refinance") == -1)
        {
          string str = Utils.FormatInput(controlForElement.Value, FieldFormat.DECIMAL_2, ref needsUpdate);
          if (needsUpdate)
            controlForElement.BindTo(str);
        }
      }
      base.onkeyup(pEvtObj);
    }

    public override void onmouseenter(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.inputData != null && pEvtObj.srcElement.tagName == "IMG")
      {
        if (this.loCompensationInputHandler != null)
          this.loCompensationInputHandler.ShowToolTips(pEvtObj);
      }
      else if (this.loan != null)
      {
        if (this.pocInputHandler != null)
          this.pocInputHandler.TurnOnPOCToolTip(pEvtObj);
      }
      else if (this.inputData != null && this.inputData is ClosingCost && this.pocInputHandler != null)
        this.pocInputHandler.TurnOnPOCToolTip(pEvtObj);
      base.onmouseenter(pEvtObj);
    }

    public override void onmouseleave(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.inputData != null && pEvtObj.srcElement.tagName == "IMG")
      {
        if (this.loCompensationInputHandler != null)
          this.loCompensationInputHandler.HideToolTips(pEvtObj);
      }
      else if (this.loan != null)
      {
        if (this.pocInputHandler != null)
          this.pocInputHandler.TurnOffPOCToolTip();
      }
      else if (this.inputData != null && this.inputData is ClosingCost && this.pocInputHandler != null)
        this.pocInputHandler.TurnOffPOCToolTip();
      base.onmouseleave(pEvtObj);
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      this.SetClosingCostLoan(true, action);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
      {
        case 287038707:
          if (!(action == "hudsetup"))
            break;
          this.SetFieldFocus("l_254");
          break;
        case 511528202:
          if (!(action == "cityfee"))
            break;
          this.SetFieldFocus("l_1637");
          break;
        case 1132459806:
          if (!(action == "statefee"))
            break;
          this.SetFieldFocus("l_1638");
          break;
        case 1370251432:
          if (!(action == "ownerins"))
            break;
          this.SetFieldFocus("l_578");
          break;
        case 1473801479:
          if (!(action == "statetax"))
            break;
          this.SetFieldFocus("l_1638");
          break;
        case 1638249311:
          if (!(action == "2010ownertitleinsurance"))
            break;
          this.SetFieldFocus("l_X783");
          break;
        case 2104362359:
          if (!(action == "2010settlementfee"))
            break;
          this.SetFieldFocus("l_X782");
          break;
        case 3050596908:
          if (!(action == "recordingfee"))
            break;
          this.SetFieldFocus("l_1636");
          break;
        case 3100944149:
          if (!(action == "localtax"))
            break;
          this.SetFieldFocus("l_1637");
          break;
        case 3302304154:
          if (!(action == "mtginsreserv"))
            break;
          if (this.GetField("1172") == "FarmersHomeAdministration")
          {
            this.SetFieldFocus("l_X1707");
            break;
          }
          if (this.IsFieldLocked("232"))
          {
            this.SetFieldFocus("l_232");
            break;
          }
          this.SetFieldFocus("l_1386");
          break;
        case 3582764703:
          if (!(action == "loanprog"))
            break;
          this.SetFieldFocus("l_1401");
          break;
        case 3709982155:
          if (!(action == "mtginsprem"))
            break;
          this.SetFieldFocus("l_562");
          break;
        case 3949279623:
          if (!(action == "taxesreserv"))
            break;
          this.SetFieldFocus("l_231");
          break;
        case 4073270834:
          if (!(action == "2010lendertitleinsurance"))
            break;
          this.SetFieldFocus("l_X784");
          break;
        case 4112030216:
          if (!(action == "edithudgfe802"))
            break;
          this.SetFieldFocus("l_617");
          break;
        case 4113929720:
          if (!(action == "userfee2"))
            break;
          this.SetFieldFocus("l_1640");
          break;
        case 4130707339:
          if (!(action == "userfee3"))
            break;
          this.SetFieldFocus("l_1643");
          break;
        case 4155076599:
          if (!(action == "ccprog"))
            break;
          this.SetFieldFocus("l_L211");
          break;
        case 4162363073:
          if (!(action == "edithudgfe801"))
            break;
          this.SetFieldFocus("l_L228");
          break;
        case 4164262577:
          if (!(action == "userfee1"))
            break;
          this.SetFieldFocus("l_373");
          break;
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      if (this.inputData is DisclosedItemizationHandler && (id.Contains("switch") || id.Contains("zoomin")))
        return ControlState.Enabled;
      if (this.allFieldsAreReadonly)
        return ControlState.Disabled;
      ControlState defaultValue = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1061":
        case "436":
          if (this.GetFieldValue("NEWHUD.X1139") == "Y" || this.GetFieldValue("NEWHUD.X715") != "Include Origination Points" || Utils.ParseDouble((object) this.GetFieldValue("3119")) > 0.0)
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "1134":
          if (this.GetField("19") == "Purchase")
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "140":
          defaultValue = this.GetSpecialControlStatus(id, defaultValue);
          break;
        case "1401":
        case "ccprog":
        case "loanprog":
          if (this.loan == null || this.FormIsForTemplate)
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "1619":
          if (this.loan != null && this.loan.GetField("1172") == "VA" && this.loan.GetField("958") == "IRRRL" && this.loan.GetField("19").IndexOf("Refinance") > -1)
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "1663":
          defaultValue = ControlState.Disabled;
          if (!this.IsUsingTemplate)
          {
            this.FormatAlphaNumericField(ctrl, id);
            break;
          }
          break;
        case "1847":
        case "NEWHUD.X734":
          if (this.GetFieldValue("NEWHUD.X715") != "Include Origination Credit" || this.GetFieldValue("NEWHUD.X1139") == "Y")
            defaultValue = ControlState.Disabled;
          this.FormatAlphaNumericField(ctrl, id);
          break;
        case "1stmor":
          if (this.GetField("420") != "SecondLien")
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "230":
        case "232":
        case "NEWHUD.X39":
          this.FormatAlphaNumericField(ctrl, id);
          defaultValue = ControlState.Default;
          break;
        case "454":
          defaultValue = this.loan != null ? ControlState.Default : ControlState.Enabled;
          break;
        case "558":
          if (this is REGZGFE_2015InputHandler && this.IsUsingTemplate && !(this.inputData is LoanData))
          {
            this.SetControlState("FieldLockAggregateAdjust", false);
            defaultValue = ControlState.Disabled;
            break;
          }
          defaultValue = ControlState.Default;
          break;
        case "NEWHUD.X1067":
        case "NEWHUD.X353":
        case "NEWHUD.X627":
        case "NEWHUD.X749":
        case "POPT.X12":
          if (this.loan == null && id == "POPT.X12")
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          if (id == "NEWHUD.X1067")
          {
            ctrl.Visible = true;
            break;
          }
          if (this.GetFieldValue("NEWHUD.X1139") == "Y")
          {
            ctrl.Visible = false;
            defaultValue = ControlState.Disabled;
            break;
          }
          ctrl.Visible = true;
          if ((id == "NEWHUD.X353" || id == "NEWHUD.X749" || id == "POPT.X12") && this.GetFieldValue("NEWHUD.X715") == "Include Origination Credit")
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "NEWHUD.X1139":
        case "NEWHUD.X1141":
        case "NEWHUD.X1225":
        case "NEWHUD.X223":
        case "NEWHUD.X224":
          if (this.GetFieldValue("NEWHUD.X1718") == "Y")
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "NEWHUD.X1140":
        case "locompensationtool":
          if (this.GetFieldValue("NEWHUD.X1139") != "Y")
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "NEWHUD.X1299":
        case "NEWHUD.X1301":
          if (this.pocInputHandler != null)
          {
            this.pocInputHandler.SetLine819Status();
            defaultValue = this.pocInputHandler.GetLine819Status(id);
            break;
          }
          if (this.GetFieldValue("1172") == "FarmersHomeAdministration")
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "NEWHUD.X14":
        case "NEWHUD.X709":
        case "NEWHUD.X710":
        case "NEWHUD.X712":
        case "NEWHUD.X714":
        case "NEWHUD.X718":
        case "NEWHUD.X741":
          if (this.GetFieldValue("NEWHUD.X1139") == "Y")
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "NEWHUD.X715":
          if (Utils.ParseDouble((object) this.GetFieldValue("3119")) > 0.0 || this.GetFieldValue("NEWHUD.X1139") == "Y")
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "NEWHUD.X78":
          if (this.GetFieldValue("NEWHUD.X351") != "Y")
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "NEWHUD.X788":
          if (this.GetFieldValue("NEWHUD.X715") != "Include Origination Points" || this.GetFieldValue("NEWHUD.X1139") == "Y")
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "NEWHUD.X820":
          ctrl.Visible = this.GetFieldValue("NEWHUD.X1139") != "Y";
          if (this.GetFieldValue("NEWHUD.X715") == "Include Origination Credit" || this.GetFieldValue("NEWHUD.X1139") == "Y")
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "copytohudgfe":
        case "hudsetup":
        case "localtax2010":
        case "recordingfee2010":
        case "statetax2010":
          if (this.loan == null)
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        case "otherf":
          if (this.GetField("420") == "SecondLien")
          {
            defaultValue = ControlState.Disabled;
            break;
          }
          break;
        default:
          defaultValue = ControlState.Default;
          break;
      }
      return defaultValue;
    }

    private ControlState setControlButtonRightForFeeManagement(
      string crtlID,
      string borrowerAmountID)
    {
      return this.setControlButtonRightForFeeManagement(crtlID, new string[1]
      {
        borrowerAmountID
      }, new string[1]{ borrowerAmountID });
    }

    private ControlState setControlButtonRightForFeeManagement(
      string crtlID,
      string[] borrowerAmountIDs,
      string[] fieldIDswithLock)
    {
      if (this.loan != null)
      {
        bool enabled = false;
        for (int index = 0; index < borrowerAmountIDs.Length; ++index)
        {
          if (crtlID.Contains("FieldLock"))
            borrowerAmountIDs[index] = "LOCKBUTTON_" + borrowerAmountIDs[index];
          if (!this.loan.IsFieldReadOnly(borrowerAmountIDs[index]))
          {
            enabled = true;
            if (crtlID.Contains("FieldLock") && this.FieldRights != null && fieldIDswithLock != null && index < fieldIDswithLock.Length)
            {
              string str = "LOCKBUTTON_" + fieldIDswithLock[index];
              if (this.FieldRights.ContainsKey((object) str))
              {
                switch (this.session.LoanDataMgr.GetFieldAccessRights(str))
                {
                  case BizRule.FieldAccessRight.Hide:
                  case BizRule.FieldAccessRight.ViewOnly:
                    enabled = false;
                    goto label_11;
                  default:
                    goto label_11;
                }
              }
              else
                break;
            }
            else
              break;
          }
        }
label_11:
        this.SetControlState(crtlID, enabled);
      }
      return ControlState.Default;
    }

    public override bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      bool flag = base.onclick(pEvtObj);
      if (this.inputData is ClosingCost && this.currentForm.FindControlForElement(pEvtObj.srcElement) is RuntimeControl controlForElement && controlForElement is Rolodex)
        this.SetClosingCostLoan(true, (string) null);
      return flag;
    }

    internal void AttachLoan(LoanData loan) => this.loan = loan;

    internal void SetTemplate()
    {
      if (this.ccLoan == null)
        return;
      for (int index = 0; index < ClosingCost.TemplateFields.Length; ++index)
      {
        if (this.ccLoan.IsLocked(ClosingCost.TemplateFields[index]))
          this.inputData.AddLock(ClosingCost.TemplateFields[index]);
        else
          this.inputData.RemoveLock(ClosingCost.TemplateFields[index]);
        this.inputData.SetField(ClosingCost.TemplateFields[index], this.ccLoan.GetField(ClosingCost.TemplateFields[index]));
      }
      if (((ClosingCost) this.inputData).RESPAVersion == "2015")
      {
        string[] template2015Fields = ClosingCost.Template2015Fields;
        for (int index = 0; index < template2015Fields.Length; ++index)
        {
          if (this.ccLoan.IsLocked(template2015Fields[index]))
            this.inputData.AddLock(template2015Fields[index]);
          else
            this.inputData.RemoveLock(template2015Fields[index]);
          this.inputData.SetField(template2015Fields[index], this.ccLoan.GetField(template2015Fields[index]));
        }
      }
      this.RefreshContents();
    }

    internal void SetClosingCostLoan(bool refreshAfterSetting, string action)
    {
      if (!(this.inputData is ClosingCost) || this.ccLoan == null || action != null && action.StartsWith("zoomin"))
        return;
      string[] templateFields = ClosingCost.TemplateFields;
      for (int index = 0; index < templateFields.Length; ++index)
      {
        if (this.inputData.IsLocked(templateFields[index]))
        {
          this.ccLoan.SetCurrentFieldFromCal(templateFields[index], this.inputData.GetField(templateFields[index]));
          this.ccLoan.AddLock(templateFields[index]);
        }
        else
          this.ccLoan.RemoveLock(templateFields[index]);
        this.ccLoan.SetCurrentFieldFromCal(templateFields[index], this.inputData.GetField(templateFields[index]));
      }
      this.ccLoan.SetCurrentFieldFromCal("NEWHUD.X12", this.inputData.GetField("NEWHUD.X12"));
      this.ccLoan?.Calculator?.FormCalculation("2404");
      if (((ClosingCost) this.inputData).RESPAVersion == "2015")
      {
        string[] template2015Fields = ClosingCost.Template2015Fields;
        for (int index = 0; index < template2015Fields.Length; ++index)
        {
          if (this.inputData.IsLocked(template2015Fields[index]))
          {
            this.ccLoan.SetCurrentFieldFromCal(template2015Fields[index], this.inputData.GetField(template2015Fields[index]));
            this.ccLoan.AddLock(template2015Fields[index]);
          }
          else
            this.ccLoan.RemoveLock(template2015Fields[index]);
          this.ccLoan.SetCurrentFieldFromCal(template2015Fields[index], this.inputData.GetField(template2015Fields[index]));
        }
      }
      if (!refreshAfterSetting)
        return;
      if ((action ?? "") != "")
        this.RunCalculation((string) null, (string) null);
      if (this is REGZGFE_2015InputHandler)
      {
        switch (action)
        {
          case "cityfee":
            this.ccLoan.Calculator.Calculate2015FeeDetails("647", "SYS.X357");
            break;
          case "statefee":
            this.ccLoan.Calculator.Calculate2015FeeDetails("648", "SYS.X359");
            break;
          case "userfee1":
            this.ccLoan.Calculator.Calculate2015FeeDetails("374", "SYS.X361");
            break;
          case "userfee2":
            this.ccLoan.Calculator.Calculate2015FeeDetails("1641", "SYS.X363");
            break;
          case "userfee3":
            this.ccLoan.Calculator.Calculate2015FeeDetails("1644", "SYS.X365");
            break;
          case "2010escrowinsurance":
            this.ccLoan.Calculator.Calculate2015FeeDetails("NEWHUD.X808", "NEWHUD2.X708");
            break;
          case "2010ownertitleinsurance":
            this.ccLoan.Calculator.Calculate2015FeeDetails("NEWHUD.X572", "NEWHUD.X744");
            break;
          case "2010lendertitleinsurance":
            this.ccLoan.Calculator.Calculate2015FeeDetails("NEWHUD.X639", "NEWHUD.X745");
            break;
        }
      }
      this.RefreshContents();
    }

    internal void UpdateClosingCostLoan(Dictionary<string, string> dirtyFields)
    {
      foreach (string key in dirtyFields.Keys)
        this.ccLoan.SetField(key, dirtyFields[key] == null ? "" : dirtyFields[key]);
      this.ccLoan.Calculator.FormCalculation("REGZGFE_2010", (string) null, (string) null);
    }
  }
}

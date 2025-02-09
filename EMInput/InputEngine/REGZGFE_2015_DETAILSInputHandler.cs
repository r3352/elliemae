// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.REGZGFE_2015_DETAILSInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessObjects.Loans;
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
  public class REGZGFE_2015_DETAILSInputHandler : REGZGFE_2010InputHandler
  {
    private EllieMae.Encompass.Forms.Panel pnlForm;
    private EllieMae.Encompass.Forms.Label labelFormName;
    private EllieMae.Encompass.Forms.Label labelLineNo;
    private EllieMae.Encompass.Forms.Label labelUnitDesc;
    private EllieMae.Encompass.Forms.Label labelDescription;
    private EllieMae.Encompass.Forms.CheckBox checkboxPropertyCost;
    private EllieMae.Encompass.Forms.Panel panelBorPaidTitle;
    private EllieMae.Encompass.Forms.Panel panelSelPaidTitle;
    private List<EllieMae.Encompass.Forms.Panel> linePanels;
    private EllieMae.Encompass.Forms.Panel panelPaidToName;
    private EllieMae.Encompass.Forms.Panel panelPaidToType;
    private EllieMae.Encompass.Forms.Panel panelRetainedAmt;
    private EllieMae.Encompass.Forms.Panel panelSec32;
    private EllieMae.Encompass.Forms.Panel panelCanDidShop;
    private EllieMae.Encompass.Forms.Panel panelAPR;
    private EllieMae.Encompass.Forms.Panel panelSellerCredit;
    private EllieMae.Encompass.Forms.Panel panelPostConsummationFee;
    private EllieMae.Encompass.Forms.CheckBox checkboxPostConsummationFee;
    private EllieMae.Encompass.Forms.Panel panelShopFor;
    private EllieMae.Encompass.Forms.Panel panelPropertyCheck;
    private EllieMae.Encompass.Forms.Panel panelInsuranceAndTaxesCheckboxes;
    private EllieMae.Encompass.Forms.Panel panelEscrowed;
    private EllieMae.Encompass.Forms.Panel panelLenderObligated;
    private EllieMae.Encompass.Forms.Panel panelLESection;
    private EllieMae.Encompass.Forms.Panel panelCDSection;
    private EllieMae.Encompass.Forms.Panel panelCDLine;
    private EllieMae.Encompass.Forms.Panel panelOptional;
    private EllieMae.Encompass.Forms.Panel panelLink;
    private EllieMae.Encompass.Forms.Panel panelInsurance;
    private VerticalRule verticalLine;
    private List<EllieMae.Encompass.Forms.Panel> panelAtBottoms = new List<EllieMae.Encompass.Forms.Panel>();
    private List<object> detailFields = new List<object>();
    private string[] selectedPOCFields;
    private GFEItem selectedGFEItem;
    private PickList pickList;
    private Hashtable tabIndexes = new Hashtable();
    private static List<string> detailFieldIDs = new List<string>()
    {
      "txtFeeAmt",
      "txtFeePercent",
      "txtLEAmt",
      "txtCDAmt",
      "cboPaidToType",
      "txtPaidToName",
      "txtRetainedAmt",
      "txtBorFinanced",
      "txtBorPTC",
      "txtBorPAC",
      "txtBorPOC",
      "txtBorAmtPaid",
      "txtSellerPAC",
      "txtSellerPOC",
      "txtSellerAmtPaid",
      "txtBrokerPAC",
      "txtBrokerPOC",
      "txtBrokerAmtPaid",
      "txtLenderPAC",
      "txtLenderPOC",
      "txtLenderAmtPaid",
      "txtOtherPAC",
      "txtOtherPOC",
      "txtOtherAmtPaid",
      "txtTotalPaidBy",
      "txtSec32Amt",
      "txtFeeAmt2",
      "chkBorShop",
      "chkBorDidShop",
      "chkImpactAPR",
      "chkSellerCredit",
      "chkSellerObligated",
      "txtSellerObligatedAmt",
      "chkPostConsummation"
    };

    public event EventHandler OnRefreshed;

    public REGZGFE_2015_DETAILSInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(mainScreen, htmldoc, form, property)
    {
    }

    public REGZGFE_2015_DETAILSInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public REGZGFE_2015_DETAILSInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(owner, dataTemplate, htmldoc, form, property)
    {
    }

    public REGZGFE_2015_DETAILSInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public REGZGFE_2015_DETAILSInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput htmlInput,
      HTMLDocument htmlDoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, owner, htmlInput, htmlDoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      try
      {
        this.pnlForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        this.labelFormName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelFormName");
        this.panelSec32 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSec32");
        this.panelCanDidShop = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelCanDidShop");
        this.panelAPR = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelAPR");
        this.panelSellerCredit = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSellerCredit");
        this.panelPostConsummationFee = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelPostConsummation");
        this.checkboxPostConsummationFee = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chkPostConsummation");
        this.panelShopFor = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelShopFor");
        this.panelPropertyCheck = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelPropertyCheck");
        this.panelEscrowed = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelEscrowed");
        this.panelInsuranceAndTaxesCheckboxes = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelInsuranceAndTaxesCheckboxes");
        this.panelLenderObligated = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLenderObligated");
        this.verticalLine = (VerticalRule) this.currentForm.FindControl("verticalLine");
        this.panelAtBottoms.Add(this.panelPostConsummationFee);
        this.panelAtBottoms.Add(this.panelCanDidShop);
        this.panelAtBottoms.Add(this.panelAPR);
        this.panelAtBottoms.Add(this.panelSellerCredit);
        this.panelAtBottoms.Add(this.panelPropertyCheck);
        this.panelAtBottoms.Add(this.panelEscrowed);
        this.panelAtBottoms.Add(this.panelLenderObligated);
        this.labelDescription = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelDescription");
        this.linePanels = new List<EllieMae.Encompass.Forms.Panel>();
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineNo"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLine700"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineDesc"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineBona"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineEditDesc"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineDescTextbox"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineTo"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLinePaidTo"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineRolodex"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineZoomin"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineUnitBox1"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineUnitLabel"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineUnitEditor"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineUnitLock"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineUnitLabelDollarSign"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineUnitBox2"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineStar"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineStar2"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineEditBor"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineLock"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineBorPaid"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineSellerPaid"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLineLOComp"));
        this.linePanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLine901"));
        this.labelLineNo = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelLineNo");
        this.labelUnitDesc = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelUnitDesc");
        this.panelBorPaidTitle = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelBorPaidTitle");
        this.panelSelPaidTitle = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSelPaidTitle");
        this.panelPaidToName = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelPaidToName");
        this.panelPaidToType = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelPaidToType");
        this.panelRetainedAmt = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelRetainedAmt");
        this.panelLESection = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLESection");
        this.panelCDSection = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelCDSection");
        this.panelCDLine = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelCDLine");
        this.panelLink = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLink");
        this.panelOptional = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelOptional");
        this.panelInsurance = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelInsurance");
        this.checkboxPropertyCost = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chkPropertyCheck");
        this.htmldoc.parentWindow.execScript("document.body.style.overflow ='hidden'");
        this.pickList = (PickList) this.currentForm.FindControl("pklist_FieldID");
        this.panelLink.Visible = false;
        this.panelPostConsummationFee.Visible = false;
      }
      catch (Exception ex)
      {
      }
      if (this.ItemizationLineNumber != null)
        this.SetLayout(this.ItemizationLineNumber);
      if (this.loan == null || !this.loan.IsTemplate || this.ItemizationLineNumber == null)
        return;
      int num = Utils.ParseInt((object) this.ItemizationLineNumber);
      if (!Utils.IsLenderObligatedFee(num))
        return;
      this.loan.UpdateLenderObligatedFeeIndicator(num);
    }

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      if (pEvtObj.keyCode == 189 && this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement && controlForElement.Field != null && controlForElement.Field.FieldID != "" && controlForElement.Field.FieldID != this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT] && controlForElement.Field.FieldID != this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT] && controlForElement.Field.FieldID != this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] && controlForElement.Field.FieldID != this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT] && controlForElement.Field.FieldID != this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_SELPAID])
      {
        string str = controlForElement.Value.Substring(1);
        controlForElement.BindTo(str);
      }
      else
        base.onkeyup(pEvtObj);
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      if (this.inputData is DisclosedItemizationHandler)
        return ControlState.Disabled;
      this.getControlState(ctrl, id, ControlState.Enabled);
      ControlState controlState = ControlState.Default;
      if (id == "NEWHUD2.X133" || id == "NEWHUD2.X134" || id == "NEWHUD2.X135" || id == "NEWHUD2.X136" || id == "NEWHUD2.X137" || id == "NEWHUD2.X138" || id == "NEWHUD2.X139" || id == "NEWHUD2.X140" || id == "NEWHUD2.X4769")
        return this.checkEscrowStatus(id);
      if (this.selectedPOCFields != null && id != "")
      {
        if (this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "2001" || this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "2002" || this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "2003" || this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER] == "2004")
        {
          if (id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID])
            this.setCreditFieldStatus(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID], "txtBrokerAmtPaid");
          else if (id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID])
            this.setCreditFieldStatus(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID], "txtLenderAmtPaid");
          else if (id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID])
            this.setCreditFieldStatus(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID], "txtOtherAmtPaid");
        }
        if (id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT])
          this.setCreditFieldStatus(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID], "txtBrokerPAC");
        else if (id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT])
          this.setCreditFieldStatus(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID], "txtBrokerPOC");
        else if (id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT] && id != "NEWHUD2.X911" && id != "NEWHUD2.X482" && (this.loan == null || this.loan.Calculator == null || !this.loan.Calculator.UseNewCompliance(18.3M) || this.loan.Calculator.UseNewCompliance(18.3M) && id != "NEWHUD2.X2099" && id != "NEWHUD2.X2132" && id != "NEWHUD2.X3584" && id != "NEWHUD2.X3617" && id != "NEWHUD2.X3881" && id != "NEWHUD2.X3914"))
          this.setCreditFieldStatus(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID], "txtLenderPAC");
        else if (id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT])
          this.setCreditFieldStatus(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID], "txtLenderPOC");
        else if (id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT])
          this.setCreditFieldStatus(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID], "txtOtherPAC");
        else if (id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT])
          this.setCreditFieldStatus(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_BORPAID], "txtOtherPOC");
        else if (id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT])
          this.setCreditFieldStatus(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_SELPAID], "txtSellerPOC");
        else if (id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED])
        {
          ((RuntimeControl) this.currentForm.FindControl("lockSellerObligatedAmount")).Enabled = this.GetFieldValue(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED]) == "Y";
          this.setFieldColor("txtSellerObligatedAmt", false, false);
        }
        else if (id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT])
          this.setFieldColor("txtSec32Amt", false, false);
        else if (id == "558" || id == "337" || id == "642" || id == "1050" || id == "NEWHUD.X571" || id == "NEWHUD.X1155" || id == "NEWHUD.X1159" || id == "NEWHUD.X1163" || id == "NEWHUD.X591" || id == "643" || id == "L260" || id == "1667" || id == "NEWHUD.X592" || id == "NEWHUD.X593" || id == "NEWHUD.X1588" || id == "NEWHUD.X1596")
          this.setFieldColor("txtBorPaid", false, false);
        else if (id == "232" || id == "454" || id == "HUD52" || id == "HUD53" || id == "HUD54" || id == "HUD55" || id == "HUD56" || id == "HUD58" || id == "HUD60" || id == "HUD62" || id == "HUD63")
          this.setFieldColor("txtUnitBorPaid", false, false);
        else if (id == "1045")
        {
          this.SetControlState("txtBorFinanced", false);
          this.setFieldColor("txtBorFinanced", true, false);
        }
        else if (id == "NEWHUD2.X911" || id == "NEWHUD2.X482")
        {
          this.SetControlState("txtLenderPAC", false);
          this.setFieldColor("txtLenderPAC", true, false);
        }
        else if (id == "L245")
          this.setFieldColor("l_L245", false, false);
        else if (id == "NEWHUD.X1301" && this.GetField("1172") == "FarmersHomeAdministration")
        {
          this.setFieldColor("txtBorPaid", false, false);
        }
        else
        {
          switch (id)
          {
            case "NEWHUD.X223":
              this.SetControlState("txtMonths", this.GetField("NEWHUD.X1718") != "Y");
              this.setFieldColor("txtMonths", this.GetField("NEWHUD.X1718") == "Y", this.GetField("NEWHUD.X1718") != "Y");
              break;
            case "NEWHUD.X224":
              this.SetControlState("txtUnitBorPaid", this.GetField("NEWHUD.X1718") != "Y");
              this.setFieldColor("txtUnitBorPaid", this.GetField("NEWHUD.X1718") == "Y", this.GetField("NEWHUD.X1718") != "Y");
              break;
            case "NEWHUD.X572":
              this.FormatAlphaNumericField(ctrl, id);
              break;
            default:
              if (id == "NEWHUD.X1299" && this.GetField("1172") == "FarmersHomeAdministration")
              {
                this.SetControlState("txtLineDesc", this.IsFieldLocked("NEWHUD.X1301"));
                if (this.IsFieldLocked("NEWHUD.X1301"))
                {
                  this.setFieldColor("txtLineDesc", false, true);
                  break;
                }
                this.setFieldColor("txtLineDesc", true, false);
                break;
              }
              if (id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015RETAINEDAMOUNT])
              {
                if (Utils.ParseDecimal((object) this.GetFieldValue(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT])) != 0M && this.GetFieldValue(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO]) == "Affiliate")
                {
                  this.SetControlState("txtRetainedAmt", true);
                  this.setFieldColor("txtRetainedAmt", false, true);
                  break;
                }
                this.SetControlState("txtRetainedAmt", false);
                this.setFieldColor("txtRetainedAmt", true, false);
                break;
              }
              if (this.panelCanDidShop.Visible && id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP] && this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP] != "")
              {
                if (this.selectedGFEItem.LineNumber >= 803 && this.selectedGFEItem.LineNumber <= 833 || this.selectedGFEItem.LineNumber == 905 || this.selectedGFEItem.LineNumber >= 1302 && this.selectedGFEItem.LineNumber <= 1309 || (this.GetFieldValue("1172") == "FHA" || this.GetFieldValue("1172") == "FarmersHomeAdministration" || this.GetFieldValue("1172") == "VA") && this.selectedGFEItem.LineNumber == 902)
                {
                  this.SetControlState("chkBorShop", false);
                  break;
                }
                this.setControlButtonRightForFeeManagement("chkBorShop", id);
                break;
              }
              if (this.panelCanDidShop.Visible && id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP] && this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP] != "")
              {
                if (this.selectedGFEItem.LineNumber >= 803 && this.selectedGFEItem.LineNumber <= 833 || this.selectedGFEItem.LineNumber == 905 || (this.GetFieldValue("1172") == "FHA" || this.GetFieldValue("1172") == "FarmersHomeAdministration" || this.GetFieldValue("1172") == "VA") && this.selectedGFEItem.LineNumber == 902)
                {
                  this.SetControlState("chkBorDidShop", false);
                  break;
                }
                this.setControlButtonRightForFeeManagement("chkBorDidShop", id);
                break;
              }
              break;
          }
        }
      }
      if (this.session.UserInfo.IsAdministrator() || this.GetFieldAccessMode(id) == FieldAccessMode.NoRestrictions)
        return controlState;
      this.SetControlState(ctrl.ControlID, false);
      this.setFieldColor(ctrl.ControlID, true, false);
      return ControlState.Disabled;
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.selectedPOCFields != null && this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED] != null && this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED] != "" && this.GetFieldValue(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED]) != "Y")
        ((RuntimeControl) this.currentForm.FindControl("lockSellerObligatedAmount")).Enabled = false;
      if (this.selectedGFEItem == null)
        return;
      if (this.selectedGFEItem != null && this.selectedGFEItem.BorrowerFieldID != "")
      {
        if (this.selectedGFEItem.BorrowerFieldID == "454")
          this.setControlButtonRightForFeeManagement("panelLineUnitLock", this.selectedGFEItem.BorrowerFieldID);
        else
          this.setControlButtonRightForFeeManagement("btnLock", this.selectedGFEItem.BorrowerFieldID);
      }
      if (this.selectedPOCFields == null || !((this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME] ?? "") != ""))
        return;
      this.setControlButtonRightForFeeManagement("btnRolodex", this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME]);
    }

    private void setControlButtonRightForFeeManagement(string crtlID, string fieldID)
    {
      if (this.loan == null)
        return;
      this.SetControlState(crtlID, !this.loan.IsFieldReadOnly(fieldID));
    }

    private void setCreditFieldStatus(string conditionalFieldID, string controlID)
    {
      if (conditionalFieldID == "" || Utils.ParseDecimal((object) this.GetField(conditionalFieldID)) == 0M)
      {
        this.SetControlState(controlID, false);
        this.setFieldColor(controlID, true, false);
      }
      else
      {
        this.SetControlState(controlID, true);
        this.setFieldColor(controlID, false, true);
      }
    }

    private ControlState checkEscrowStatus(string id)
    {
      for (int index = 41; index <= 49; ++index)
      {
        if (this.GetFieldValue("HUD01" + (object) index) != "//" && this.GetFieldValue("HUD01" + (object) index) != "")
        {
          this.SetControlState("chkEscrowed", false);
          return ControlState.Disabled;
        }
      }
      return ControlState.Default;
    }

    public override void onfocus(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onfocus(pEvtObj);
      if (!(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement) || !controlForElement.Interactive || controlForElement is DropdownBox)
        return;
      controlForElement.BackColor = WinFormInputHandler.FocusHighlightColor;
    }

    public override void onblur(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onfocus(pEvtObj);
      if (!(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement) || !controlForElement.Interactive || controlForElement is DropdownBox)
        return;
      controlForElement.BackColor = WinFormInputHandler.NoFocusColor;
    }

    internal override void FlipLockField(FieldLock fieldLock)
    {
      base.FlipLockField(fieldLock);
      if (this.loan == null || !(fieldLock.ControlToLock is FieldControl controlToLock) || controlToLock.Field == null || controlToLock.Field.FieldID == "")
        return;
      this.loan.Calculator.Calculate2015FeeDetails(controlToLock.Field.FieldID);
      this.RefreshContents();
      if (this.loan.IsTemplate)
        return;
      this.session.Application.GetService<ILoanEditor>().RefreshLoanContents();
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (this.selectedPOCFields != null)
      {
        if (id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015RETAINEDAMOUNT] && val != "" && Utils.ParseDecimal((object) val, 0M) > Utils.ParseDecimal((object) this.GetField(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]), 0M) + Utils.ParseDecimal((object) this.GetField(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]), 0M))
        {
          int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "The Retained amt. cannot exceed the total of the borrower-paid and seller-paid amounts.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.SetFieldFocus("txtRetainedAmt");
          return;
        }
        if (id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT] && val != "" && Utils.ParseDecimal((object) val, 0M) > Utils.ParseDecimal((object) this.GetField(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID]), 0M))
        {
          int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "The Seller Obligated amount cannot exceed Seller Amount Paid!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.SetFieldFocus("txtSellerObligatedAmt");
          return;
        }
      }
      string field = this.inputData.GetField(id);
      if (id == "NEWHUD.X572" || id == "NEWHUD.X39" || id == "1663" || id == "232" || id == "230")
      {
        double num = Utils.ParseDouble((object) val);
        if (num != 0.0 || val == "0" || val == "." || val.IndexOf("0.") > -1 || val.IndexOf(".0") > -1)
          val = num.ToString("N2");
      }
      try
      {
        this.inputData.SetField(id, val, true);
        switch (id)
        {
          case "NEWHUD.X572":
            this.inputData.SetField("2010TITLE_TABLE", "");
            break;
          case "NEWHUD.X639":
            this.inputData.SetField("TITLE_TABLE", "");
            break;
          case "NEWHUD.X808":
            this.inputData.SetField("ESCROW_TABLE", "");
            break;
        }
        this.SetPropertyInsuranceTaxes(id, val);
        if (this.loan != null)
        {
          if (id == "NEWHUD.X1718" && val == "Y")
            this.loan.Calculator.FormCalculation("COPYLOCOMPTO2010ITEMIZATION", id, val);
          else if (id == "454")
            this.loan.TriggerCalculation("388", this.loan.GetField("388"));
        }
        if (this.loan != null)
        {
          switch (id)
          {
            case "389":
            case "1620":
            case "NEWHUD.X223":
            case "NEWHUD.X224":
            case "NEWHUD.X1718":
              this.loan.TriggerCalculation(id, val);
              break;
            case "NEWHUD.X1714":
              this.loan.TriggerCalculation("562", this.inputData.GetField("562"));
              break;
          }
        }
      }
      catch (Exception ex)
      {
      }
      if (this.loCompensationSetting != null)
      {
        LOCompensationInputHandler.CheckLOCompRuleConfliction(this.loCompensationSetting, this.inputData is ClosingCost ? (IHtmlInput) this.ccLoan : this.inputData, id, val, field, true);
        if (LOCompensationInputHandler.RefreshScreenRequired)
        {
          this.RefreshContents();
          ILoanEditor service = this.session.Application.GetService<ILoanEditor>();
          if (service != null)
            service.RefreshLoanContents();
          else if (this.OnRefreshed != null)
            this.OnRefreshed((object) this, (EventArgs) null);
        }
      }
      if (this.selectedPOCFields != null && id == this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO] && this.selectedGFEItem != null && this.selectedGFEItem.PayeeFieldID != "")
      {
        string id1 = this.selectedGFEItem.PayeeFieldID;
        if (this.selectedGFEItem.LineNumber == 1202)
          id1 = "NEWHUD2.X121";
        else if (this.selectedGFEItem.LineNumber == 1204)
          id1 = "NEWHUD2.X122";
        else if (this.selectedGFEItem.LineNumber == 1205)
          id1 = "NEWHUD2.X123";
        switch (val)
        {
          case "Broker":
            base.UpdateFieldValue(id1, this.GetField("VEND.X293"));
            break;
          case "Lender":
            base.UpdateFieldValue(id1, this.GetField("1264"));
            break;
          case "Seller":
            base.UpdateFieldValue(id1, this.GetField("638"));
            break;
          case "Investor":
            base.UpdateFieldValue(id1, this.GetField("VEND.X263"));
            break;
        }
      }
      if (this.loan != null && (this.selectedGFEItem.LineNumber == 1202 || this.selectedGFEItem.LineNumber == 1204 || this.selectedGFEItem.LineNumber == 1205 || this.selectedGFEItem.LineNumber == 1206 || this.selectedGFEItem.LineNumber == 1207 || this.selectedGFEItem.LineNumber == 1208))
        this.loan.Calculator.FormCalculation("UpdateCityStateUserFees", id, val);
      if (this.loan != null)
        this.loan.Calculator.Calculate2015FeeDetails(id, this.PaidByFieldID, false);
      else if (this.inputData is ClosingCost)
        this.ccLoan.Calculator.Calculate2015FeeDetails(id, this.PaidByFieldID, false);
      this.updateCDSectionInfo((EllieMae.Encompass.Forms.Label) null);
      this.updateLESectionInfo((EllieMae.Encompass.Forms.Label) null);
      this.RefreshContents();
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      if (this.loan == null)
        return;
      switch (action)
      {
        case "recordingfee":
          this.loan.Calculator.Calculate2015FeeDetails("390", "SYS.X355");
          break;
        case "cityfee":
        case "localtax":
          this.loan.Calculator.Calculate2015FeeDetails("647", "SYS.X357");
          break;
        case "statefee":
        case "statetax":
          this.loan.Calculator.Calculate2015FeeDetails("648", "SYS.X359");
          break;
        case "userfee1":
          this.loan.Calculator.Calculate2015FeeDetails("374", "SYS.X361");
          break;
        case "userfee2":
          this.loan.Calculator.Calculate2015FeeDetails("1641", "SYS.X363");
          break;
        case "userfee3":
          this.loan.Calculator.Calculate2015FeeDetails("1644", "SYS.X365");
          break;
      }
    }

    public void SetLayout(string lineNo)
    {
      bool flag1 = this.GetFieldValue("423") == "Biweekly";
      string[] ptcpocSet = this.findPTCPOCSet(lineNo);
      GFEItem hudFieldSet = this.findHUDFieldSet(lineNo);
      this.selectedPOCFields = ptcpocSet;
      this.selectedGFEItem = hudFieldSet;
      switch (lineNo)
      {
        case "802x":
        case "803x":
        case "1101x":
          this.labelLineNo.Text = lineNo.Substring(0, lineNo.Length - 1) + ".";
          break;
        case "2001":
        case "2002":
        case "2003":
        case "2004":
          this.labelLineNo.Text = "PC" + lineNo.Substring(3, 1) + ".";
          break;
        default:
          this.labelLineNo.Text = lineNo + ".";
          break;
      }
      int lineNumber = Utils.ParseInt((object) lineNo);
      int componentNo = -1;
      if (lineNumber == -1)
      {
        if (lineNo.StartsWith("1101") || lineNo.StartsWith("1102"))
          lineNumber = Utils.ParseInt((object) lineNo.Substring(0, 4));
        else if (lineNo.StartsWith("80"))
          lineNumber = Utils.ParseInt((object) lineNo.Substring(0, 3));
        componentNo = char.ConvertToUtf32(lineNo.Substring(lineNo.Length - 1, 1), 0);
      }
      this.setDetailFields(lineNumber, componentNo, ptcpocSet, hudFieldSet);
      if (lineNumber == 701 || lineNumber == 702)
      {
        this.setTextFieldAttribute("txt700Amount", lineNumber == 701 ? "L211" : "L213", lineNumber);
        this.setLinePanelVisible("700");
      }
      if (lineNumber == 704 || lineNumber == 801 && componentNo >= 103 || lineNumber == 802 && componentNo >= 102 && componentNo != 120 || lineNumber == 803 || lineNumber >= 808 && lineNumber <= 835 || lineNumber >= 907 && lineNumber <= 912 || lineNumber >= 1007 && lineNumber <= 1009 || lineNumber == 1101 && componentNo != 120 || lineNumber == 1102 && componentNo >= 100 && componentNo <= 104 || lineNumber >= 1109 && lineNumber <= 1116 || lineNumber >= 1206)
      {
        this.setTextFieldAttribute("txtLineDesc", hudFieldSet.Description, lineNumber);
        this.setLinePanelVisible("DescTextbox");
        this.pickList.ControlID = "pklist_" + hudFieldSet.Description.Replace(".", "");
      }
      else
      {
        if (lineNumber == 802 && componentNo == 120)
        {
          this.labelDescription.Text = "Total Credit for Rate Chosen";
        }
        else
        {
          switch (lineNumber)
          {
            case 1105:
              this.labelDescription.Text = "Lender's Title Policy Limit";
              break;
            case 1106:
              this.labelDescription.Text = "Owner's Title Policy Limit";
              break;
            case 1107:
              this.labelDescription.Text = "Agent's Portion of the Total Title Ins. Premium";
              this.labelDescription.Size = new Size(this.labelDescription.Size.Width + 120, this.labelDescription.Size.Height);
              break;
            case 1108:
              this.labelDescription.Text = "Underwriter's Portion of The Total Title Ins. Premium";
              this.labelDescription.Size = new Size(this.labelDescription.Size.Width + 140, this.labelDescription.Size.Height);
              break;
            case 1201:
              this.labelDescription.Text = "Total Recording Charges";
              break;
            default:
              this.labelDescription.Text = hudFieldSet != null ? hudFieldSet.Description : "";
              break;
          }
        }
        this.setLinePanelVisible("Desc");
      }
      if (lineNumber == 802 && componentNo == 101)
      {
        this.setCheckBoxFieldAttribute("chkBona", "NEWHUD.X1067", lineNumber);
        this.setLinePanelVisible("Bona");
      }
      if (lineNumber == 801 && (componentNo == 97 || componentNo == 101 || componentNo == 102) || lineNumber == 802 && componentNo >= 101 && componentNo != 120 || lineNumber == 901 || lineNumber == 903 || lineNumber == 904 || lineNumber >= 906 && lineNumber <= 912 || lineNumber >= 1002 && lineNumber <= 1010)
      {
        switch (lineNumber)
        {
          case 801:
            switch (componentNo)
            {
              case 97:
                this.setTextFieldAttribute("txtMonths", "388", lineNumber);
                break;
              case 101:
                this.setTextFieldAttribute("txtMonths", "389", lineNumber);
                break;
              case 102:
                this.setTextFieldAttribute("txtMonths", "NEWHUD.X223", lineNumber);
                break;
            }
            break;
          case 802:
            switch (componentNo)
            {
              case 101:
                this.setTextFieldAttribute("txtMonths", "NEWHUD.X1150", lineNumber);
                break;
              case 102:
                this.setTextFieldAttribute("txtMonths", "NEWHUD.X1154", lineNumber);
                break;
              case 103:
                this.setTextFieldAttribute("txtMonths", "NEWHUD.X1158", lineNumber);
                break;
              case 104:
                this.setTextFieldAttribute("txtMonths", "NEWHUD.X1162", lineNumber);
                break;
            }
            break;
          case 901:
            this.setTextFieldAttribute("txtMonths", "332", lineNumber);
            break;
          case 903:
            this.setTextFieldAttribute("txtMonths", "L251", lineNumber);
            break;
          case 904:
            this.setTextFieldAttribute("txtMonths", "NEWHUD2.X4397", lineNumber);
            break;
          case 906:
            this.setTextFieldAttribute("txtMonths", "NEWHUD2.X4399", lineNumber);
            break;
          case 907:
            this.setTextFieldAttribute("txtMonths", "NEWHUD2.X4401", lineNumber);
            break;
          case 908:
            this.setTextFieldAttribute("txtMonths", "NEWHUD2.X4403", lineNumber);
            break;
          case 909:
            this.setTextFieldAttribute("txtMonths", "NEWHUD2.X4405", lineNumber);
            break;
          case 910:
            this.setTextFieldAttribute("txtMonths", "NEWHUD2.X4407", lineNumber);
            break;
          case 911:
            this.setTextFieldAttribute("txtMonths", "NEWHUD2.X4409", lineNumber);
            break;
          case 912:
            this.setTextFieldAttribute("txtMonths", "NEWHUD2.X4411", lineNumber);
            break;
          case 1002:
            this.setTextFieldAttribute("txtMonths", "1387", lineNumber);
            break;
          case 1003:
            this.setTextFieldAttribute("txtMonths", "1296", lineNumber);
            break;
          case 1004:
            this.setTextFieldAttribute("txtMonths", "1386", lineNumber);
            break;
          case 1005:
            this.setTextFieldAttribute("txtMonths", "L267", lineNumber);
            break;
          case 1006:
            this.setTextFieldAttribute("txtMonths", "1388", lineNumber);
            break;
          case 1007:
            this.setTextFieldAttribute("txtMonths", "1629", lineNumber);
            break;
          case 1008:
            this.setTextFieldAttribute("txtMonths", "340", lineNumber);
            break;
          case 1009:
            this.setTextFieldAttribute("txtMonths", "341", lineNumber);
            break;
          case 1010:
            this.setTextFieldAttribute("txtMonths", "NEWHUD.X1706", lineNumber);
            break;
        }
        this.setLinePanelVisible("UnitBox1");
      }
      if (lineNumber == 801 && (componentNo == 97 || componentNo == 101 || componentNo == 102) || lineNumber == 802 && componentNo >= 101 && componentNo != 120 || lineNumber == 901 || lineNumber == 903 || lineNumber == 904 || lineNumber >= 906 && lineNumber <= 912 || lineNumber >= 1002 && lineNumber <= 1010)
        this.setLinePanelVisible("UnitLabel");
      if (lineNumber >= 1003 && lineNumber <= 1004 || lineNumber == 1010)
      {
        this.setEditorAction("btnUnitEditor", lineNumber);
        this.setLinePanelVisible("UnitEditor");
      }
      if (lineNumber == 801 && (componentNo == 97 || componentNo == 101 || componentNo == 102) || lineNumber == 802 && componentNo == 101 || lineNumber == 901 || lineNumber == 903 || lineNumber == 904 || lineNumber >= 906 && lineNumber <= 912 || lineNumber >= 1002 && lineNumber <= 1010)
      {
        switch (lineNumber)
        {
          case 801:
            switch (componentNo)
            {
              case 97:
                this.setTextFieldAttribute("txtUnitBorPaid", hudFieldSet.BorrowerFieldID, lineNumber);
                this.setLinePanelVisible("UnitLabelDollarSign");
                break;
              case 101:
                this.setTextFieldAttribute("txtUnitBorPaid", "1620", lineNumber);
                break;
              case 102:
                this.setTextFieldAttribute("txtUnitBorPaid", "NEWHUD.X224", lineNumber);
                break;
            }
            break;
          case 802:
            if (componentNo == 101)
            {
              this.setTextFieldAttribute("txtUnitBorPaid", "NEWHUD.X1227", lineNumber);
              break;
            }
            break;
          case 901:
            this.setTextFieldAttribute("txtUnitBorPaid", "333", lineNumber);
            this.setLinePanelVisible("901");
            break;
          case 903:
            this.setTextFieldAttribute("txtUnitBorPaid", "230", lineNumber);
            break;
          case 904:
            this.setTextFieldAttribute("txtUnitBorPaid", "231", lineNumber);
            break;
          case 906:
            this.setTextFieldAttribute("txtUnitBorPaid", "NEWHUD2.X4400", lineNumber);
            break;
          case 907:
            this.setTextFieldAttribute("txtUnitBorPaid", "NEWHUD2.X4402", lineNumber);
            break;
          case 908:
            this.setTextFieldAttribute("txtUnitBorPaid", "NEWHUD2.X4404", lineNumber);
            break;
          case 909:
            this.setTextFieldAttribute("txtUnitBorPaid", "NEWHUD2.X4406", lineNumber);
            break;
          case 910:
            this.setTextFieldAttribute("txtUnitBorPaid", "NEWHUD2.X4408", lineNumber);
            break;
          case 911:
            this.setTextFieldAttribute("txtUnitBorPaid", "NEWHUD2.X4410", lineNumber);
            break;
          case 912:
            this.setTextFieldAttribute("txtUnitBorPaid", "NEWHUD2.X4412", lineNumber);
            break;
          case 1002:
            this.setTextFieldAttribute("txtUnitBorPaid", flag1 ? "HUD53" : "230", lineNumber);
            break;
          case 1003:
            this.setTextFieldAttribute("txtUnitBorPaid", flag1 ? "HUD54" : "232", lineNumber);
            break;
          case 1004:
            this.setTextFieldAttribute("txtUnitBorPaid", flag1 ? "HUD52" : "231", lineNumber);
            break;
          case 1005:
            this.setTextFieldAttribute("txtUnitBorPaid", flag1 ? "HUD56" : "L268", lineNumber);
            break;
          case 1006:
            this.setTextFieldAttribute("txtUnitBorPaid", flag1 ? "HUD55" : "235", lineNumber);
            break;
          case 1007:
            this.setTextFieldAttribute("txtUnitBorPaid", flag1 ? "HUD58" : "1630", lineNumber);
            break;
          case 1008:
            this.setTextFieldAttribute("txtUnitBorPaid", flag1 ? "HUD60" : "253", lineNumber);
            break;
          case 1009:
            this.setTextFieldAttribute("txtUnitBorPaid", flag1 ? "HUD62" : "254", lineNumber);
            break;
          case 1010:
            this.setTextFieldAttribute("txtUnitBorPaid", flag1 ? "HUD63" : "NEWHUD.X1707", lineNumber);
            break;
        }
        this.setLinePanelVisible("UnitBox2");
      }
      if (lineNumber == 801 && componentNo == 97 || ((lineNumber == 1002 || lineNumber == 1004 || lineNumber == 1005 || lineNumber == 1006 || lineNumber == 1007 || lineNumber == 1008 ? 1 : (lineNumber == 1009 ? 1 : 0)) & (flag1 ? 1 : 0)) != 0 || lineNumber == 1003)
      {
        this.setLockAttribute("btnUnitLock", "txtUnitBorPaid");
        this.setLinePanelVisible("UnitLock");
      }
      if (lineNumber == 1202 || lineNumber == 1204 || lineNumber == 1205)
      {
        this.setEditorAction("btnEditDesc2", lineNumber);
        this.setLinePanelVisible("EditDesc");
      }
      if (lineNumber >= 701 && lineNumber <= 704 || lineNumber == 801 && componentNo >= 103 && componentNo <= 114 || lineNumber == 803 || lineNumber >= 808 && lineNumber <= 835 || lineNumber == 904 || lineNumber >= 907 && lineNumber <= 912 || lineNumber >= 1007 && lineNumber <= 1010 || lineNumber == 1101 || lineNumber == 1102 || lineNumber >= 1107 && lineNumber <= 1116 || lineNumber >= 1206)
        this.setLinePanelVisible("To");
      if (lineNumber >= 701 && lineNumber <= 704 || lineNumber == 801 && componentNo >= 103 && componentNo <= 114 || lineNumber >= 803 && lineNumber <= 835 || lineNumber >= 902 && lineNumber <= 912 || lineNumber >= 1007 && lineNumber <= 1010 || lineNumber >= 1101 && lineNumber <= 1104 || lineNumber >= 1107 && lineNumber <= 1116 || lineNumber >= 1202)
      {
        switch (lineNumber)
        {
          case 1105:
            this.setTextFieldAttribute("txtPaidTo", "646", lineNumber);
            break;
          case 1106:
            this.setTextFieldAttribute("txtPaidTo", "1634", lineNumber);
            break;
          case 1107:
            this.setTextFieldAttribute("txtPaidTo", "NEWHUD.X206", lineNumber);
            break;
          case 1108:
            this.setTextFieldAttribute("txtPaidTo", "NEWHUD.X207", lineNumber);
            break;
          default:
            this.setTextFieldAttribute("txtPaidTo", hudFieldSet.PayeeFieldID, lineNumber);
            break;
        }
        this.setLinePanelVisible("PaidTo");
      }
      if (lineNumber >= 804 && lineNumber <= 807 || lineNumber == 902 || lineNumber == 903 || lineNumber == 906 || lineNumber == 1101 && componentNo == 120 || lineNumber == 1102 && componentNo == 97 || lineNumber >= 1103 && lineNumber <= 1104 || lineNumber >= 1107 && lineNumber <= 1108)
      {
        this.setRolodexAttribute("btnRolodex", "txtPaidTo", lineNumber);
        this.setLinePanelVisible("Rolodex");
      }
      if (lineNumber >= 1204 && lineNumber <= 1208)
      {
        this.setEditorAction("btnZoom2", lineNumber);
        this.setLinePanelVisible("Zoomin");
      }
      if (lineNumber == 902 || lineNumber == 903 || lineNumber == 1102 && componentNo == 99 || lineNumber >= 1103 && lineNumber <= 1104)
      {
        this.setEditorAction("btnEditBorPaid", lineNumber);
        this.setLinePanelVisible("EditBor");
      }
      this.panelBorPaidTitle.Visible = false;
      this.panelSelPaidTitle.Visible = false;
      if (lineNumber >= 701 && lineNumber <= 704 || lineNumber == 801 && componentNo != 97 || lineNumber == 802 && componentNo == -1 || lineNumber == 802 && componentNo > 0 || lineNumber >= 803 && lineNumber <= 835 || lineNumber >= 901 && lineNumber <= 912 || lineNumber >= 1002 && lineNumber <= 1011 || lineNumber >= 1101 && lineNumber <= 1104 || lineNumber >= 1107 && lineNumber <= 1116 || lineNumber >= 1201)
      {
        if (lineNumber == 802 && componentNo == 120)
        {
          this.setTextFieldAttribute("txtBorPaid", "NEWHUD.X1149", lineNumber);
        }
        else
        {
          switch (lineNumber)
          {
            case 1107:
              this.setTextFieldAttribute("txtBorPaid", "NEWHUD.X640", lineNumber);
              break;
            case 1108:
              this.setTextFieldAttribute("txtBorPaid", "NEWHUD.X641", lineNumber);
              break;
            default:
              this.setTextFieldAttribute("txtBorPaid", ptcpocSet[HUDGFE2010Fields.PTCPOCINDEX_BORPAID], lineNumber);
              break;
          }
        }
        this.setLinePanelVisible("BorPaid");
        if (lineNumber != 1011)
        {
          this.panelBorPaidTitle.Visible = true;
          if (lineNumber == 801 && componentNo == 102 || lineNumber == 802 && componentNo == 120)
            ((EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelBorrower")).Text = "Lender";
        }
      }
      else if (lineNumber == 801 && componentNo == 97)
        this.panelBorPaidTitle.Visible = true;
      if (lineNumber == 802 && componentNo > 101 && componentNo != 120 || lineNumber == 819 && this.GetField("1172") == "FarmersHomeAdministration" || lineNumber == 902 || lineNumber == 903 || lineNumber == 905 || lineNumber == 1011 || lineNumber == 904 || lineNumber >= 906 && lineNumber <= 912)
      {
        this.setLockAttribute("btnLock", "txtBorPaid");
        this.setLinePanelVisible("Lock");
      }
      if (lineNumber >= 701 && lineNumber <= 704 || lineNumber == 801 && componentNo != 102 || lineNumber == 802 && componentNo >= 101 && componentNo != 120 || lineNumber >= 803 && lineNumber <= 835 || lineNumber >= 901 && lineNumber <= 912 || lineNumber >= 1002 && lineNumber <= 1010 || lineNumber >= 1101 && lineNumber <= 1104 || lineNumber >= 1107 && lineNumber <= 1116 || lineNumber >= 1201)
      {
        if (lineNumber == 1201)
          this.setTextFieldAttribute("txtSelPaid", "NEWHUD.X799", lineNumber);
        else
          this.setTextFieldAttribute("txtSelPaid", hudFieldSet.SellerFieldID, lineNumber);
        this.setLinePanelVisible("SellerPaid");
        this.panelSelPaidTitle.Visible = true;
      }
      int num1 = this.linePanels[0].Left + this.linePanels[0].Size.Width;
      bool flag2 = false;
      int num2 = 200;
      if (lineNumber == 903 || lineNumber == 904 || lineNumber >= 906 && lineNumber <= 912 || lineNumber >= 1007 && lineNumber <= 1010)
        flag2 = true;
      switch (lineNumber)
      {
        case 801:
          if (componentNo == 97)
          {
            this.labelUnitDesc.Text = "% or";
            break;
          }
          if (componentNo >= 101)
          {
            this.labelUnitDesc.Text = "% + $";
            break;
          }
          break;
        case 802:
          if (componentNo == 101)
          {
            this.labelUnitDesc.Text = "% + $";
            break;
          }
          if (componentNo > 101 && componentNo != 120)
          {
            this.labelUnitDesc.Text = "% or ";
            break;
          }
          break;
        default:
          if (lineNo == "901")
          {
            this.labelUnitDesc.Text = "days @ $";
            break;
          }
          if (flag1 && lineNumber >= 1002 && lineNumber <= 1010)
          {
            this.labelUnitDesc.Text = "bwks @ $";
            break;
          }
          break;
      }
      if (lineNumber == 801 && componentNo == 102)
      {
        this.setCheckBoxFieldAttribute("chkLOCompTool", "NEWHUD.X1718", lineNumber);
        this.setLinePanelVisible("LOComp");
      }
      int num3 = 23;
      if (lineNumber == 901 || lineNumber == 903 || lineNumber == 904 || lineNumber >= 906 && lineNumber <= 912 || lineNumber >= 1007 && lineNumber <= 1010)
        num3 = 18;
      this.panelBorPaidTitle.Top = num3 - 16;
      this.panelSelPaidTitle.Top = num3 - 16;
      Size size1;
      for (int index = 0; index < this.linePanels.Count; ++index)
      {
        if (this.linePanels[index].ControlID == "panelLineStar" && lineNumber != 1107 && lineNumber != 1108 && (lineNumber != 801 || componentNo != 97) && (lineNumber != 802 || componentNo < 102))
          num1 += lineNumber == 701 || lineNumber == 702 ? 20 : 50;
        if (!(this.linePanels[index].ControlID == "panelLine901") && this.linePanels[index].Visible)
        {
          this.linePanels[index].Top = num3;
          if (index != 0)
          {
            if (this.linePanels[index].ControlID == "panelLineDesc")
            {
              EllieMae.Encompass.Forms.Panel linePanel = this.linePanels[index];
              size1 = this.labelDescription.Size;
              int width = size1.Width + (lineNumber == 902 ? 65 : 10);
              size1 = this.linePanels[index].Size;
              int height = size1.Height;
              Size size2 = new Size(width, height);
              linePanel.Size = size2;
            }
            else if (this.linePanels[index].ControlID == "panelLineUnitLabel")
            {
              EllieMae.Encompass.Forms.Panel linePanel = this.linePanels[index];
              size1 = this.labelUnitDesc.Size;
              int width = size1.Width + 10;
              size1 = this.linePanels[index].Size;
              int height = size1.Height;
              Size size3 = new Size(width, height);
              linePanel.Size = size3;
            }
            if (flag2 && this.linePanels[index].ControlID.StartsWith("panelLineUnit"))
            {
              this.linePanels[index].Left = num2;
              this.linePanels[index].Top += 22;
              int num4 = num2;
              size1 = this.linePanels[index].Size;
              int width = size1.Width;
              num2 = num4 + width;
            }
            else
            {
              this.linePanels[index].Left = num1;
              int num5 = num1;
              size1 = this.linePanels[index].Size;
              int width = size1.Width;
              num1 = num5 + width;
            }
          }
        }
      }
      if (this.panelBorPaidTitle.Visible)
      {
        if (lineNumber == 801 && componentNo == 97)
          this.panelBorPaidTitle.Left = this.linePanels.Find((Predicate<EllieMae.Encompass.Forms.Panel>) (p => p.ControlID == "panelLineUnitBox2")).Left;
        else
          this.panelBorPaidTitle.Left = this.linePanels.Find((Predicate<EllieMae.Encompass.Forms.Panel>) (p => p.ControlID == "panelLineBorPaid")).Left;
      }
      if (this.panelSelPaidTitle.Visible)
        this.panelSelPaidTitle.Left = this.linePanels.Find((Predicate<EllieMae.Encompass.Forms.Panel>) (p => p.ControlID == "panelLineSellerPaid")).Left;
      if ((lineNumber == 801 && componentNo == 97 || ((lineNumber == 1002 || lineNumber == 1004 || lineNumber == 1005 || lineNumber == 1006 || lineNumber == 1007 || lineNumber == 1008 ? 1 : (lineNumber == 1009 ? 1 : 0)) & (flag1 ? 1 : 0)) != 0 || lineNumber == 1003) && this.linePanels.Find((Predicate<EllieMae.Encompass.Forms.Panel>) (c => c.ControlID == "panelLineUnitLock")).Visible)
        this.setFieldColor("txtUnitBorPaid", false, false);
      if ((lineNumber == 802 && componentNo >= 102 && componentNo <= 104 || lineNumber == 902 || lineNumber == 903 || lineNumber == 905 || lineNumber == 1011 || lineNumber == 1201 || lineNumber == 904 || lineNumber >= 906 && lineNumber <= 912) && this.linePanels.Find((Predicate<EllieMae.Encompass.Forms.Panel>) (c => c.ControlID == "panelLineBorPaid")).Visible)
        this.setFieldColor("txtBorPaid", false, false);
      if (lineNumber == 901)
      {
        this.setFieldColor("l_L244", false, true);
        this.setFieldColor("l_L245", false, false);
      }
      if (lineNumber == 1101 && componentNo == 120 || lineNumber == 1201)
        this.setFieldColor("txtSelPaid", false, false);
      if (lineNumber == 903)
        this.linePanels.Find((Predicate<EllieMae.Encompass.Forms.Panel>) (c => c.ControlID == "panelLine901")).Left = this.linePanels.Find((Predicate<EllieMae.Encompass.Forms.Panel>) (c => c.ControlID == "panelLinePaidTo")).Left - 50;
      if (lineNumber == 802 && componentNo == 120 || lineNumber == 1010 || lineNumber == 1011 || lineNumber == 1201)
        this.panelPaidToName.Visible = false;
      if (lineNumber == 802 && componentNo == 120 || lineNumber == 1101 && componentNo == 120 || lineNumber == 1010 || lineNumber == 1011 || lineNumber == 1201)
        this.panelPaidToType.Visible = false;
      if (lineNumber != 903 && lineNumber != 906 && lineNumber != 1103 && lineNumber != 1104)
        this.panelRetainedAmt.Visible = false;
      int top1 = this.panelPaidToName.Top;
      if (this.panelPaidToName.Visible)
        top1 += 22;
      if (this.panelPaidToType.Visible)
      {
        this.panelPaidToType.Top = top1;
        top1 += 22;
      }
      if (this.panelRetainedAmt.Visible)
        this.panelRetainedAmt.Top = top1;
      if (lineNumber >= 701 && lineNumber <= 704 || lineNumber == 802 && componentNo == 120 || lineNumber == 901 || lineNumber == 904 || lineNumber == 905 || lineNumber >= 1004 && lineNumber <= 1005 || lineNumber == 1011 || lineNumber == 1201 || lineNumber >= 1204 && lineNumber <= 1208)
        this.panelSec32.Visible = false;
      switch (lineNumber)
      {
        case 819:
          if (this.GetField("1172") == "FarmersHomeAdministration")
          {
            this.panelSec32.Visible = false;
            break;
          }
          break;
        case 902:
          if (this.GetField("1172") == "FarmersHomeAdministration" && this.IsFieldLocked("NEWHUD.X1301"))
          {
            this.panelSec32.Visible = false;
            break;
          }
          break;
        case 1003:
          this.GetFieldValue("NEWHUD2.X2601");
          if (this.GetField("1172") == "FHA")
          {
            this.panelSec32.Visible = false;
            break;
          }
          break;
        default:
          if (lineNumber == 1010 && this.GetField("1172") != "FarmersHomeAdministration")
          {
            this.panelSec32.Visible = false;
            break;
          }
          break;
      }
      if (lineNumber == 902 || lineNumber == 819 && this.GetField("1172") == "FarmersHomeAdministration" || lineNumber == 905 && this.GetField("1172") == "VA")
      {
        this.SetControlState("txtBorFinanced", false);
        this.setFieldColor("txtBorFinanced", true, false);
      }
      if (lineNumber >= 701 && lineNumber <= 704 || lineNumber == 801 || lineNumber == 802 || lineNumber == 834 || lineNumber == 835 || lineNumber == 901 || lineNumber >= 1002 && lineNumber <= 1011 || lineNumber == 1101 && componentNo == 120 || lineNumber == 1103 || lineNumber == 1115 || lineNumber == 1116 || lineNumber >= 1201 && lineNumber <= 1210 || lineNumber >= 1310 && lineNumber <= 1320)
        this.panelCanDidShop.Visible = false;
      if (lineNumber >= 701 && lineNumber <= 704 || lineNumber == 801 && componentNo == 102 || lineNumber == 802 && componentNo == 120 || lineNumber == 1011 || lineNumber == 1101 && componentNo == 120 || lineNumber == 1201 || lineNumber >= 2001 && lineNumber <= 2004)
        this.panelAPR.Visible = false;
      if (lineNumber == 801 && componentNo == 102 || lineNumber == 802 && componentNo == 120 || lineNumber == 1011 || lineNumber == 1101 && componentNo == 120 || lineNumber == 1201)
        this.panelSellerCredit.Visible = false;
      if ((lineNumber < 1007 || lineNumber > 1009) && (lineNumber < 907 || lineNumber > 912))
      {
        this.panelPropertyCheck.Visible = false;
      }
      else
      {
        switch (lineNumber)
        {
          case 907:
            this.setCheckBoxFieldAttribute("chkPropertyCheck", "NEWHUD2.X4435", lineNumber);
            this.setCheckBoxFieldAttribute("chkInsurance", "NEWHUD2.X4415", lineNumber);
            this.setCheckBoxFieldAttribute("chkTaxes", "NEWHUD2.X4416", lineNumber);
            break;
          case 908:
            this.setCheckBoxFieldAttribute("chkPropertyCheck", "NEWHUD2.X4436", lineNumber);
            this.setCheckBoxFieldAttribute("chkInsurance", "NEWHUD2.X4417", lineNumber);
            this.setCheckBoxFieldAttribute("chkTaxes", "NEWHUD2.X4418", lineNumber);
            break;
          case 909:
            this.setCheckBoxFieldAttribute("chkPropertyCheck", "NEWHUD2.X4437", lineNumber);
            this.setCheckBoxFieldAttribute("chkInsurance", "NEWHUD2.X4419", lineNumber);
            this.setCheckBoxFieldAttribute("chkTaxes", "NEWHUD2.X4420", lineNumber);
            break;
          case 910:
            this.setCheckBoxFieldAttribute("chkPropertyCheck", "NEWHUD2.X4438", lineNumber);
            this.setCheckBoxFieldAttribute("chkInsurance", "NEWHUD2.X4421", lineNumber);
            this.setCheckBoxFieldAttribute("chkTaxes", "NEWHUD2.X4422", lineNumber);
            break;
          case 911:
            this.setCheckBoxFieldAttribute("chkPropertyCheck", "NEWHUD2.X4439", lineNumber);
            this.setCheckBoxFieldAttribute("chkInsurance", "NEWHUD2.X4423", lineNumber);
            this.setCheckBoxFieldAttribute("chkTaxes", "NEWHUD2.X4424", lineNumber);
            break;
          case 912:
            this.setCheckBoxFieldAttribute("chkPropertyCheck", "NEWHUD2.X4440", lineNumber);
            this.setCheckBoxFieldAttribute("chkInsurance", "NEWHUD2.X4425", lineNumber);
            this.setCheckBoxFieldAttribute("chkTaxes", "NEWHUD2.X4426", lineNumber);
            break;
          case 1007:
            this.setCheckBoxFieldAttribute("chkPropertyCheck", "NEWHUD2.X124", lineNumber);
            this.setCheckBoxFieldAttribute("chkInsurance", "NEWHUD2.X125", lineNumber);
            this.setCheckBoxFieldAttribute("chkTaxes", "NEWHUD2.X126", lineNumber);
            break;
          case 1008:
            this.setCheckBoxFieldAttribute("chkPropertyCheck", "NEWHUD2.X127", lineNumber);
            this.setCheckBoxFieldAttribute("chkInsurance", "NEWHUD2.X128", lineNumber);
            this.setCheckBoxFieldAttribute("chkTaxes", "NEWHUD2.X129", lineNumber);
            break;
          case 1009:
            this.setCheckBoxFieldAttribute("chkPropertyCheck", "NEWHUD2.X130", lineNumber);
            this.setCheckBoxFieldAttribute("chkInsurance", "NEWHUD2.X131", lineNumber);
            this.setCheckBoxFieldAttribute("chkTaxes", "NEWHUD2.X132", lineNumber);
            break;
        }
      }
      if (lineNumber >= 1002 && lineNumber <= 1010)
      {
        this.panelEscrowed.Visible = true;
        switch (lineNumber)
        {
          case 1002:
            this.setCheckBoxFieldAttribute("chkEscrowed", "NEWHUD2.X133", lineNumber);
            break;
          case 1003:
            this.setCheckBoxFieldAttribute("chkEscrowed", "NEWHUD2.X4769", lineNumber);
            break;
          case 1004:
            this.setCheckBoxFieldAttribute("chkEscrowed", "NEWHUD2.X134", lineNumber);
            break;
          case 1005:
            this.setCheckBoxFieldAttribute("chkEscrowed", "NEWHUD2.X135", lineNumber);
            break;
          case 1006:
            this.setCheckBoxFieldAttribute("chkEscrowed", "NEWHUD2.X136", lineNumber);
            break;
          case 1007:
            this.setCheckBoxFieldAttribute("chkEscrowed", "NEWHUD2.X137", lineNumber);
            break;
          case 1008:
            this.setCheckBoxFieldAttribute("chkEscrowed", "NEWHUD2.X138", lineNumber);
            break;
          case 1009:
            this.setCheckBoxFieldAttribute("chkEscrowed", "NEWHUD2.X139", lineNumber);
            break;
          case 1010:
            this.setCheckBoxFieldAttribute("chkEscrowed", "NEWHUD2.X140", lineNumber);
            break;
        }
      }
      else
        this.panelEscrowed.Visible = false;
      if (Utils.IsLenderObligatedFee(lineNumber))
      {
        this.panelLenderObligated.Visible = true;
        this.setCheckBoxFieldAttribute("chkLenderObligated", Utils.GetLenderObligatedIndicatorFieldID(lineNumber), lineNumber);
      }
      else
        this.panelLenderObligated.Visible = false;
      if (lineNumber >= 1000 && lineNumber <= 1011)
      {
        this.SetControlState("txtBorPOC", false);
        this.setFieldColor("txtBorPOC", true, false);
        this.SetControlState("txtSellerPOC", false);
        this.setFieldColor("txtSellerPOC", true, false);
        this.SetControlState("txtBrokerPOC", false);
        this.setFieldColor("txtBrokerPOC", true, false);
        this.SetControlState("txtLenderPOC", false);
        this.setFieldColor("txtLenderPOC", true, false);
        this.SetControlState("txtOtherPOC", false);
        this.setFieldColor("txtOtherPOC", true, false);
      }
      if (lineNumber == 802 && componentNo == 120)
      {
        this.SetControlState("txtLenderPOC", false);
        this.setFieldColor("txtLenderPOC", true, false);
      }
      if (this.loan != null && this.loan.Calculator != null && this.loan.Calculator.UseNewCompliance(18.3M) && Utils.IsLenderObligatedFee(lineNumber))
      {
        this.SetControlState("txtBorFinanced", false);
        this.setFieldColor("txtBorFinanced", true, false);
        this.SetControlState("txtBorPOC", false);
        this.setFieldColor("txtBorPOC", true, false);
        this.SetControlState("txtLenderPAC", false);
        this.setFieldColor("txtLenderPAC", true, false);
        this.SetControlState("chkSellerObligated", false);
        this.setFieldColor("chkSellerObligated", true, false);
      }
      else if (lineNo == "2001" || lineNo == "2002" || lineNo == "2003" || lineNo == "2004")
      {
        this.SetControlState("txtBorAmtPaid", false);
        this.setFieldColor("txtBorAmtPaid", true, false);
        this.SetControlState("txtSellerAmtPaid", false);
        this.setFieldColor("txtSellerAmtPaid", true, false);
        this.panelPostConsummationFee.Visible = true;
        this.panelOptional.Visible = false;
        this.checkboxPostConsummationFee.Enabled = false;
      }
      if (lineNumber == 801 && componentNo == 102 || lineNumber >= 834 && lineNumber <= 835 || lineNumber == 1011 || lineNumber >= 1115 && lineNumber <= 1116 || lineNumber == 1202 || lineNumber >= 1204 && lineNumber <= 1210 || lineNumber >= 2001 && lineNumber <= 2004)
        this.panelLESection.Visible = false;
      if (lineNumber == 1201 || lineNumber >= 1204 && lineNumber <= 1205 || lineNumber >= 2001 && lineNumber <= 2004)
        this.panelCDSection.Visible = false;
      this.panelCDLine.Visible = false;
      if (this.panelLESection.Visible)
        this.updateLESectionInfo((EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelLESection"));
      if (this.panelCDSection.Visible)
        this.updateCDSectionInfo((EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelCDSection"));
      if (lineNumber == 1101 && componentNo == 120 || lineNumber >= 1204 && lineNumber <= 1210)
        this.panelLink.Visible = false;
      int num6 = this.panelLESection.Top;
      if (this.panelLESection.Visible)
        num6 += 22;
      if (this.panelCDSection.Visible)
      {
        this.panelCDSection.Top = num6;
        num6 += 22;
      }
      if (this.panelCDLine.Visible)
      {
        this.panelCDLine.Top = num6;
        num6 += 22;
      }
      if (this.panelOptional.Visible)
      {
        this.panelOptional.Top = num6;
        num6 += 22;
      }
      if (this.panelInsurance.Visible)
      {
        this.panelInsurance.Top = num6;
        num6 += 44;
      }
      if (this.panelLink.Visible)
      {
        this.panelLink.Top = num6;
        num6 += 22;
      }
      int num7 = this.panelCanDidShop.Top;
      for (int index = 0; index < this.panelAtBottoms.Count; ++index)
      {
        if (this.panelAtBottoms[index].Visible)
        {
          this.panelAtBottoms[index].Top = num7;
          int num8 = num7;
          size1 = this.panelAtBottoms[index].Size;
          int height = size1.Height;
          num7 = num8 + height;
        }
      }
      if (num6 < num7)
        num6 = num7;
      EllieMae.Encompass.Forms.Panel panelShopFor = this.panelShopFor;
      size1 = this.panelShopFor.Size;
      Size size4 = new Size(size1.Width, num6 < 100 ? 100 : num6 + 10);
      panelShopFor.Size = size4;
      VerticalRule verticalLine = this.verticalLine;
      size1 = this.verticalLine.Size;
      int width1 = size1.Width;
      size1 = this.panelShopFor.Size;
      int height1 = size1.Height;
      Size size5 = new Size(width1, height1);
      verticalLine.Size = size5;
      EllieMae.Encompass.Forms.Panel pnlForm = this.pnlForm;
      size1 = this.pnlForm.Size;
      int width2 = size1.Width;
      int top2 = this.panelShopFor.Top;
      size1 = this.panelShopFor.Size;
      int height2 = size1.Height;
      int height3 = top2 + height2 + 3;
      Size size6 = new Size(width2, height3);
      pnlForm.Size = size6;
      EllieMae.Encompass.Forms.Label labelFormName = this.labelFormName;
      size1 = this.pnlForm.Size;
      int num9 = size1.Height + 3;
      labelFormName.Top = num9;
    }

    private void updateCDSectionInfo(EllieMae.Encompass.Forms.Label t)
    {
      if (!this.panelCDSection.Visible)
        return;
      if (t == null)
        t = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelCDSection");
      string[] selectedPocFields = this.selectedPOCFields;
      int num = Utils.ParseInt((object) selectedPocFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER]);
      if (num == 801 || num == 802 && selectedPocFields[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID] != "x")
        t.Text = "A";
      else if (num >= 803 && num <= 835 || num == 905)
        t.Text = "B";
      else if (num == 902 || num >= 1100 && num <= 1102 || num == 1104 || num >= 1109 && num <= 1116 || num >= 1302 && num <= 1309)
      {
        if (this.GetFieldValue(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]) == "Y")
          t.Text = "C";
        else
          t.Text = "B";
      }
      else if (num >= 1200 && num <= 1210)
        t.Text = "E";
      else if (num == 901 || num == 903 || num == 904 || num >= 906 && num <= 912)
        t.Text = "F";
      else if (num >= 1002 && num <= 1011)
        t.Text = "G";
      else if (num >= 701 && num <= 704 || num == 1103 || num >= 1310 && num <= 1320)
      {
        t.Text = "H";
      }
      else
      {
        if (num != 802 || !(selectedPocFields[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID] == "x"))
          return;
        t.Text = "J";
      }
    }

    private void updateLESectionInfo(EllieMae.Encompass.Forms.Label t)
    {
      if (!this.panelLESection.Visible)
        return;
      if (t == null)
        t = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelLESection");
      string[] selectedPocFields = this.selectedPOCFields;
      int num = Utils.ParseInt((object) selectedPocFields[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER]);
      if (num == 801 || num == 802 && selectedPocFields[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID] != "x")
        t.Text = "A";
      else if (num >= 803 && num <= 833 || num == 905)
        t.Text = "B";
      else if (num >= 1302 && num <= 1309)
        t.Text = "C";
      else if (num == 902 || num >= 1100 && num <= 1102 || num == 1104 || num >= 1109 && num <= 1114)
      {
        if (this.GetFieldValue(this.selectedPOCFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]) == "Y")
          t.Text = "C";
        else
          t.Text = "B";
      }
      else if (num >= 1200 && num <= 1203)
        t.Text = "E";
      else if (num == 901 || num == 903 || num == 904 || num >= 906 && num <= 912)
        t.Text = "F";
      else if (num >= 1002 && num <= 1010)
        t.Text = "G";
      else if (num >= 701 && num <= 704 || num == 1103 || num >= 1310 && num <= 1320)
      {
        t.Text = "H";
      }
      else
      {
        if (num != 802 || !(selectedPocFields[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID] == "x"))
          return;
        t.Text = "J";
      }
    }

    private void setFieldColor(string controlID, bool alwaysDisabled, bool alwaysEnabled)
    {
      FieldControl control = (FieldControl) this.currentForm.FindControl(controlID);
      if (control == null)
        return;
      if (this.inputData is DisclosedItemizationHandler)
      {
        control.BackColor = Color.FromArgb(245, 245, 245);
        control.BorderColor = Color.FromArgb(200, 199, 199);
        control.TabIndex = -1;
      }
      else if (alwaysEnabled || this.IsFieldLocked(control.Field.FieldID) && !alwaysDisabled)
      {
        control.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
        control.BorderColor = Color.FromArgb(123, 156, 189);
        if (!this.tabIndexes.ContainsKey((object) control.ControlID))
          return;
        control.TabIndex = Utils.ParseInt((object) this.tabIndexes[(object) control.ControlID].ToString());
      }
      else
      {
        if (!(control is EllieMae.Encompass.Forms.CheckBox))
        {
          control.BackColor = Color.FromArgb(245, 245, 245);
          control.BorderColor = Color.FromArgb(200, 199, 199);
        }
        if (!this.tabIndexes.ContainsKey((object) control.ControlID))
          this.tabIndexes.Add((object) control.ControlID, (object) control.TabIndex);
        control.TabIndex = -1;
      }
    }

    private string[] findPTCPOCSet(string currentLineNo)
    {
      if (currentLineNo.StartsWith("pc"))
        return (string[]) null;
      for (int index = 0; index < HUDGFE2010Fields.WHOLEPOC_FIELDS.Count; ++index)
      {
        string[] ptcpocSet = HUDGFE2010Fields.WHOLEPOC_FIELDS[index];
        string str1 = ptcpocSet[HUDGFE2010Fields.PTCPOCINDEX_LINENUMBER];
        if (str1.StartsWith("0"))
          str1 = str1.Substring(1);
        string str2 = ptcpocSet[HUDGFE2010Fields.PTCPOCINDEX_COMPONENTID];
        if (string.Compare(currentLineNo, str1 + str2, true) == 0)
          return ptcpocSet;
      }
      return (string[]) null;
    }

    private GFEItem findHUDFieldSet(string currentLineNo)
    {
      if (currentLineNo.StartsWith("pc"))
        return (GFEItem) null;
      foreach (GFEItem hudFieldSet in GFEItemCollection.GFEItems2010)
      {
        if (string.Compare(currentLineNo, hudFieldSet.LineNumber.ToString() + hudFieldSet.ComponentID, true) == 0)
          return hudFieldSet;
      }
      return (GFEItem) null;
    }

    private void setLinePanelVisible(string panelName)
    {
      this.linePanels.Find((Predicate<EllieMae.Encompass.Forms.Panel>) (c => c.ControlID == "panelLine" + panelName)).Visible = true;
    }

    private void setTextFieldAttribute(string controlID, string emID, int lineNumber)
    {
      EllieMae.Encompass.Forms.TextBox control = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl(controlID);
      if (control == null)
        return;
      control.Field = EncompassApplication.Session.Loans.FieldDescriptors[emID];
      control.HelpKey = emID;
      if (control.Field.Format != LoanFieldFormat.STRING)
        control.Alignment = TextAlignment.Right;
      if (control.ControlID == "txtPaidToName")
      {
        this.setRolodexAttribute("rdxPaidToName", control.ControlID, lineNumber);
        control.RolodexField = EllieMae.Encompass.Forms.RolodexField.Company;
      }
      if (this.inputData is DisclosedItemizationHandler)
      {
        control.Enabled = false;
        control.TabIndex = -1;
        this.setFieldColor(controlID, true, false);
      }
      else if (emID == "334" || emID == "NEWHUD.X645" || emID == "230" && lineNumber == 903 || emID == "NEWHUD.X1149" || emID == "L211" || emID == "L213" || emID == "439" || emID == "NEWHUD.X225" || emID == "NEWHUD.X1151" || emID == "656" || emID == "338" || emID == "655" || emID == "L269" || emID == "657" || emID == "1631" || emID == "658" || emID == "659" || emID == "NEWHUD.X1708" || emID == "NEWHUD.X607" || emID == "333")
      {
        control.Enabled = false;
        if (!this.tabIndexes.ContainsKey((object) control.ControlID))
          this.tabIndexes.Add((object) control.ControlID, (object) control.TabIndex);
        control.TabIndex = -1;
      }
      else
      {
        control.BackColor = Color.White;
        control.BorderColor = Color.FromArgb(123, 156, 189);
        if (!this.tabIndexes.ContainsKey((object) control.ControlID))
          return;
        control.TabIndex = Utils.ParseInt((object) this.tabIndexes[(object) control.ControlID].ToString());
      }
    }

    private void setLockAttribute(string controlID, string textBoxID)
    {
      ((FieldLock) this.currentForm.FindControl(controlID)).ControlToLock = this.currentForm.FindControl(textBoxID);
    }

    private void setRolodexAttribute(string rolodexCtrlID, string textFieldCtrlID, int lineNumber)
    {
      Rolodex control = (Rolodex) this.currentForm.FindControl(rolodexCtrlID);
      if (control == null)
        return;
      string name;
      switch (lineNumber)
      {
        case 804:
          name = BusinessCategoryEnumUtil.ValueToName(BusinessCategory.Appraiser);
          break;
        case 805:
          name = BusinessCategoryEnumUtil.ValueToName(BusinessCategory.Credit);
          break;
        case 806:
          name = BusinessCategoryEnumUtil.ValueToName(BusinessCategory.Servicing);
          break;
        case 807:
        case 906:
          name = BusinessCategoryEnumUtil.ValueToName(BusinessCategory.FloodInsurer);
          break;
        case 902:
          name = BusinessCategoryEnumUtil.ValueToName(BusinessCategory.MortgageInsurer);
          break;
        case 903:
          name = BusinessCategoryEnumUtil.ValueToName(BusinessCategory.HazardInsurer);
          break;
        case 1101:
        case 1103:
        case 1104:
        case 1107:
        case 1108:
          name = BusinessCategoryEnumUtil.ValueToName(BusinessCategory.Title);
          break;
        case 1102:
          name = BusinessCategoryEnumUtil.ValueToName(BusinessCategory.Escrow);
          break;
        default:
          name = BusinessCategoryEnumUtil.ValueToName(BusinessCategory.NoCategory);
          break;
      }
      control.BusinessCategory = EncompassApplication.Session.Contacts.BizCategories.GetItemByName(name);
      ((EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl(textFieldCtrlID)).RolodexField = EllieMae.Encompass.Forms.RolodexField.Company;
    }

    private void setEditorAction(string controlID, int lineNumber)
    {
      StandardButton control = (StandardButton) this.currentForm.FindControl(controlID);
      if (control == null)
        return;
      switch (lineNumber)
      {
        case 902:
          control.Action = "mtginsprem";
          break;
        case 903:
          control.Action = "ownerins";
          break;
        case 1003:
        case 1010:
          control.Action = "mtginsreserv";
          break;
        case 1004:
          control.Action = "taxesreserv";
          break;
        case 1102:
          control.Action = "2010escrowinsurance";
          break;
        case 1103:
          control.Action = "2010ownertitleinsurance";
          break;
        case 1104:
          control.Action = "2010lendertitleinsurance";
          break;
        case 1202:
          control.Action = "recordingfee";
          break;
        case 1204:
          if (controlID == "btnEditDesc2")
          {
            control.Action = "localtax";
            break;
          }
          control.Action = "cityfee";
          break;
        case 1205:
          if (controlID == "btnEditDesc2")
          {
            control.Action = "statetax";
            break;
          }
          control.Action = "statefee";
          break;
        case 1206:
          control.Action = "userfee1";
          break;
        case 1207:
          control.Action = "userfee2";
          break;
        case 1208:
          control.Action = "userfee3";
          break;
      }
    }

    private void setDetailFields(
      int lineNumber,
      int componentNo,
      string[] pocFields,
      GFEItem gfeItem)
    {
      string empty = string.Empty;
      for (int index = 0; index < REGZGFE_2015_DETAILSInputHandler.detailFieldIDs.Count; ++index)
      {
        string fieldId = this.findFieldID(REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index], pocFields, gfeItem);
        if (fieldId == string.Empty || fieldId == "NEWHUD.X226")
        {
          this.SetControlState(REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index], false);
          if (REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index].StartsWith("txt"))
            this.setFieldColor(REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index], true, false);
          if (REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index] == "txtPaidToName")
            this.SetControlState("rdxPaidToName", false);
        }
        else if (REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index].StartsWith("txt"))
        {
          this.setTextFieldAttribute(REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index], fieldId, lineNumber);
          if (this.inputData is DisclosedItemizationHandler)
            this.setFieldColor(REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index], true, false);
          else if (REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index] == "txtTotalPaidBy" || REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index] == "txtFeeAmt2" || REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index] == "txtBorAmtPaid" || REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index] == "txtSellerAmtPaid" || REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index] == "txtBrokerAmtPaid" || REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index] == "txtLenderAmtPaid" || REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index] == "txtOtherAmtPaid" || REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index] == "txtLEAmt" || REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index] == "txtCDAmt")
            this.setFieldColor(REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index], true, false);
          else if (REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index] == "txtSec32Amt" || REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index] == "txtSellerObligatedAmt")
            this.setFieldColor(REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index], false, false);
        }
        else if (REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index].StartsWith("chk"))
          this.setCheckBoxFieldAttribute(REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index], fieldId, lineNumber);
        else if (REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index].StartsWith("cbo"))
          this.setDropdownFieldAttribute(REGZGFE_2015_DETAILSInputHandler.detailFieldIDs[index], fieldId, lineNumber);
      }
      if (lineNumber != 1101 && componentNo != 120)
      {
        EllieMae.Encompass.Forms.TextBox control1 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txtLEAmt");
        EllieMae.Encompass.Forms.TextBox control2 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txtCDAmt");
        if (control1.Field.FieldID == "" && control2.Field.FieldID == "")
          ((RuntimeControl) this.currentForm.FindControl("panelLastDisclosed")).Visible = false;
        else if (control1.Field.FieldID == "")
        {
          EllieMae.Encompass.Forms.Panel control3 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelLastLE");
          control3.Visible = false;
          this.currentForm.FindControl("panelLastCD").Top = control3.Top;
        }
        else if (control2.Field.FieldID == "")
          ((RuntimeControl) this.currentForm.FindControl("panelLastCD")).Visible = false;
      }
      ((RuntimeControl) this.currentForm.FindControl("lockSellerObligatedAmount")).Enabled = this.GetFieldValue(((FieldControl) this.currentForm.FindControl("chkSellerObligated")).Field.FieldID) == "Y";
      this.SetControlState("txtFeeAmt", false);
      this.setFieldColor("txtFeeAmt", true, false);
      this.setFieldColor("txtFeePercent", true, false);
      this.SetControlState("txtBorPAC", false);
      this.setFieldColor("txtBorPAC", true, false);
      this.SetControlState("txtSellerPAC", false);
      this.setFieldColor("txtSellerPAC", true, false);
      this.SetControlState("txtBorPTC", false);
      this.setFieldColor("txtBorPTC", true, false);
      if (lineNumber == 801 && (componentNo == 101 || componentNo == 102))
        this.SetControlState("cboPaidToType", false);
      if (lineNumber >= 803 && lineNumber <= 833 || lineNumber == 905)
      {
        this.SetControlState("chkBorShop", false);
        this.SetControlState("chkBorDidShop", false);
      }
      if (lineNumber >= 1302 && lineNumber <= 1309)
        this.SetControlState("chkBorShop", false);
      if (lineNumber == 1101 && componentNo == 120)
        this.SetControlState("FieldLock1", false);
      if (lineNumber == 801 && componentNo == 102 || lineNumber == 802 && componentNo == 120)
      {
        this.SetControlState("txtLenderPAC", false);
        this.setFieldColor("txtLenderPAC", true, false);
      }
      if (lineNumber == 1103 || lineNumber >= 1310)
      {
        this.panelOptional.Visible = true;
        switch (lineNumber)
        {
          case 1103:
            this.setCheckBoxFieldAttribute("chkOptional", "NEWHUD2.X3335", lineNumber);
            break;
          case 1310:
            this.setCheckBoxFieldAttribute("chkOptional", "NEWHUD2.X4196", lineNumber);
            break;
          case 1311:
            this.setCheckBoxFieldAttribute("chkOptional", "NEWHUD2.X4229", lineNumber);
            break;
          case 1312:
            this.setCheckBoxFieldAttribute("chkOptional", "NEWHUD2.X4262", lineNumber);
            break;
          case 1313:
            this.setCheckBoxFieldAttribute("chkOptional", "NEWHUD2.X4295", lineNumber);
            break;
          case 1314:
            this.setCheckBoxFieldAttribute("chkOptional", "NEWHUD2.X4328", lineNumber);
            break;
          case 1315:
            this.setCheckBoxFieldAttribute("chkOptional", "NEWHUD2.X4361", lineNumber);
            break;
          case 1316:
            this.setCheckBoxFieldAttribute("chkOptional", "NEWHUD2.X4447", lineNumber);
            break;
          case 1317:
            this.setCheckBoxFieldAttribute("chkOptional", "NEWHUD2.X4480", lineNumber);
            break;
          case 1318:
            this.setCheckBoxFieldAttribute("chkOptional", "NEWHUD2.X4513", lineNumber);
            break;
          case 1319:
            this.setCheckBoxFieldAttribute("chkOptional", "NEWHUD2.X4546", lineNumber);
            break;
          case 1320:
            this.setCheckBoxFieldAttribute("chkOptional", "NEWHUD2.X4579", lineNumber);
            break;
        }
      }
      if (lineNumber == 1103 || lineNumber == 1104)
      {
        this.panelInsurance.Visible = true;
        this.setCheckBoxFieldAttribute("chkSimultaneousInsurance", lineNumber == 1103 ? "NEWHUD2.X4441" : "NEWHUD2.X4443", lineNumber);
        this.setTextFieldAttribute("txtUndiscountedPremium", lineNumber == 1103 ? "NEWHUD2.X4442" : "NEWHUD2.X4444", lineNumber);
      }
      if ((lineNumber != 1101 || componentNo != 120) && lineNumber != 1201)
        return;
      this.calculateSubTotals(lineNumber);
    }

    private void setCheckBoxFieldAttribute(string controlID, string emID, int lineNumber)
    {
      EllieMae.Encompass.Forms.CheckBox control = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl(controlID);
      if (control == null)
        return;
      control.Field = EncompassApplication.Session.Loans.FieldDescriptors[emID];
      control.HelpKey = emID;
      control.CheckedValue = "Y";
      control.UncheckedValue = "N";
    }

    private void setDropdownFieldAttribute(string controlID, string emID, int lineNumber)
    {
      DropdownBox control = (DropdownBox) this.currentForm.FindControl(controlID);
      if (control == null)
        return;
      control.Field = EncompassApplication.Session.Loans.FieldDescriptors[emID];
      control.HelpKey = emID;
      control.Options.Clear();
      control.Options.Add(new DropdownOption("", ""));
      control.Options.Add(new DropdownOption("Broker", "Broker"));
      control.Options.Add(new DropdownOption("Lender", "Lender"));
      control.Options.Add(new DropdownOption("Seller", "Seller"));
      control.Options.Add(new DropdownOption("Investor", "Investor"));
      control.Options.Add(new DropdownOption("Affiliate", "Affiliate"));
      control.Options.Add(new DropdownOption("Other", "Other"));
    }

    private string findFieldID(string controlID, string[] pocFields, GFEItem gfeItem)
    {
      switch (controlID)
      {
        case "cboPaidToType":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO] != string.Empty ? pocFields[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO] : string.Empty;
        case "chkBorDidShop":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP];
        case "chkBorShop":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP];
        case "chkImpactAPR":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_APR];
        case "chkPostConsummation":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_POSTCONSUMMATIONFEE];
        case "chkSellerCredit":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELCREDIT];
        case "chkSellerObligated":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED];
        case "txtBorAmtPaid":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID];
        case "txtBorFinanced":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT];
        case "txtBorPAC":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT];
        case "txtBorPOC":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT];
        case "txtBorPTC":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BORPTCAMT];
        case "txtBrokerAmtPaid":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID];
        case "txtBrokerPAC":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT];
        case "txtBrokerPOC":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT];
        case "txtCDAmt":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015CDAmt];
        case "txtFeeAmt":
        case "txtFeeAmt2":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT];
        case "txtFeePercent":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEPERCENT];
        case "txtLEAmt":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LEAmt];
        case "txtLenderAmtPaid":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID];
        case "txtLenderPAC":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT];
        case "txtLenderPOC":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT];
        case "txtOtherAmtPaid":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID];
        case "txtOtherPAC":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT];
        case "txtOtherPOC":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT];
        case "txtPaidToName":
          if (gfeItem == null)
            return "";
          if (gfeItem.LineNumber == 1202)
            return "NEWHUD2.X121";
          if (gfeItem.LineNumber == 1204)
            return "NEWHUD2.X122";
          return gfeItem.LineNumber == 1205 ? "NEWHUD2.X123" : gfeItem.PayeeFieldID;
        case "txtRetainedAmt":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015RETAINEDAMOUNT];
        case "txtSec32Amt":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT];
        case "txtSellerAmtPaid":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID];
        case "txtSellerObligatedAmt":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT];
        case "txtSellerPAC":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPACAMT];
        case "txtSellerPOC":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT];
        case "txtTotalPaidBy":
          return pocFields[HUDGFE2010Fields.PTCPOCINDEX_2015TOTALPAIDBY];
        default:
          return string.Empty;
      }
    }

    private void calculateSubTotals(int lineNumber)
    {
      string[] strArray1 = new string[22]
      {
        "txtLEAmt",
        "txtCDAmt",
        "txtBorFinanced",
        "txtBorPTC",
        "txtBorPOC",
        "txtBorPAC",
        "txtBorAmtPaid",
        "txtSellerPAC",
        "txtSellerPOC",
        "txtSellerAmtPaid",
        "txtBrokerPAC",
        "txtBrokerPOC",
        "txtBrokerAmtPaid",
        "txtLenderPAC",
        "txtLenderPOC",
        "txtLenderAmtPaid",
        "txtOtherPAC",
        "txtOtherPOC",
        "txtOtherAmtPaid",
        "txtTotalPaidBy",
        "txtFeeAmt",
        "txtSec32Amt"
      };
      int[] numArray = new int[22]
      {
        HUDGFE2010Fields.PTCPOCINDEX_2015LEAmt,
        HUDGFE2010Fields.PTCPOCINDEX_2015CDAmt,
        HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT,
        HUDGFE2010Fields.PTCPOCINDEX_2015BORPTCAMT,
        HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT,
        HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT,
        HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID,
        HUDGFE2010Fields.PTCPOCINDEX_2015SELPACAMT,
        HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT,
        HUDGFE2010Fields.PTCPOCINDEX_2015SELAMTPAID,
        HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT,
        HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT,
        HUDGFE2010Fields.PTCPOCINDEX_2015BRKAMTPAID,
        HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT,
        HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT,
        HUDGFE2010Fields.PTCPOCINDEX_2015LENDERAMTPAID,
        HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT,
        HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT,
        HUDGFE2010Fields.PTCPOCINDEX_2015OTHAMTPAID,
        HUDGFE2010Fields.PTCPOCINDEX_2015TOTALPAIDBY,
        HUDGFE2010Fields.PTCPOCINDEX_2015TOTALFEEAMOUNT,
        HUDGFE2010Fields.PTCPOCINDEX_2015SEC32AMT
      };
      for (int index1 = 0; index1 < strArray1.Length; ++index1)
      {
        Decimal num = 0M;
        switch (lineNumber)
        {
          case 1101:
            List<string[]> strArrayList = new List<string[]>();
            for (int j = 97; j <= 102; ++j)
              strArrayList.Add(HUDGFE2010Fields.WHOLEPOC_FIELDS.Find((Predicate<string[]>) (x => x[0] == string.Concat((object) 1101) && x[1] == char.ConvertFromUtf32(j))));
            for (int j = 97; j <= 104; ++j)
              strArrayList.Add(HUDGFE2010Fields.WHOLEPOC_FIELDS.Find((Predicate<string[]>) (x => x[0] == string.Concat((object) 1102) && x[1] == char.ConvertFromUtf32(j))));
            strArrayList.Add(HUDGFE2010Fields.WHOLEPOC_FIELDS.Find((Predicate<string[]>) (x => x[0] == string.Concat((object) 1104))));
            for (int j = 1109; j <= 1114; ++j)
              strArrayList.Add(HUDGFE2010Fields.WHOLEPOC_FIELDS.Find((Predicate<string[]>) (x => x[0] == string.Concat((object) j))));
            for (int index2 = 0; index2 < strArrayList.Count; ++index2)
            {
              string[] strArray2 = strArrayList[index2];
              num += Utils.ParseDecimal((object) this.GetFieldValue(strArray2[numArray[index1]]), 0M);
            }
            break;
          case 1201:
            for (int j = 2; j <= 8; ++j)
            {
              if (j != 3 && j != 4 && j != 5)
              {
                string[] strArray3 = HUDGFE2010Fields.WHOLEPOC_FIELDS.Find((Predicate<string[]>) (x => x[0] == string.Concat((object) (1200 + j))));
                if (strArray3 != null)
                  num += Utils.ParseDecimal((object) this.GetFieldValue(strArray3[numArray[index1]]), 0M);
              }
            }
            break;
        }
        if (!(num == 0M))
        {
          EllieMae.Encompass.Forms.TextBox control = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl(strArray1[index1]);
          if (control != null)
            control.Text = num.ToString("N2");
        }
      }
      EllieMae.Encompass.Forms.TextBox control1 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txtFeeAmt");
      EllieMae.Encompass.Forms.TextBox control2 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txtFeeAmt2");
      if (control1 == null || control2 == null)
        return;
      control2.Text = control1.Text;
      EllieMae.Encompass.Forms.TextBox control3 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txtFeePercent");
      Decimal num1 = Utils.ParseDecimal((object) control1.Text, 0M);
      Decimal num2 = this.GetFieldValue("1172") == "FHA" ? Utils.ParseDecimal((object) this.GetFieldValue("1109"), 0M) : Utils.ParseDecimal((object) this.GetFieldValue("2"), 0M);
      if (!(num2 != 0M))
        return;
      Decimal num3 = Utils.ArithmeticRounding(num1 / num2 * 100M, 3);
      if (!(num3 != 0M))
        return;
      control3.Text = num3.ToString("N3");
    }

    public string PaidByFieldID => this.selectedGFEItem.PaidByFieldID;

    public override void Dispose()
    {
      if (this.linePanels != null)
      {
        this.linePanels.Clear();
        this.linePanels = (List<EllieMae.Encompass.Forms.Panel>) null;
      }
      if (this.panelAtBottoms != null)
      {
        this.panelAtBottoms.Clear();
        this.panelAtBottoms = (List<EllieMae.Encompass.Forms.Panel>) null;
      }
      if (this.detailFields != null)
      {
        this.detailFields.Clear();
        this.detailFields = (List<object>) null;
      }
      if (this.selectedPOCFields != null)
        this.selectedPOCFields = (string[]) null;
      if (this.tabIndexes == null)
        return;
      this.tabIndexes.Clear();
      this.tabIndexes = (Hashtable) null;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.RE88395InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class RE88395InputHandler : InputHandlerBase
  {
    private bool isRE882;
    private POCInputHandler pocInputHandler;
    private LOCompensationInputHandler loCompensationInputHandler;

    public RE88395InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public RE88395InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
      if (htmldoc.url.ToUpper().IndexOf("RE882") > -1)
        this.isRE882 = true;
      this.loan.Calculator.FormCalculation("MLDS", (string) null, (string) null);
      this.UpdateContents();
    }

    public RE88395InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public RE88395InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
      if (htmldoc.url.ToUpper().IndexOf("RE882") <= -1)
        return;
      this.isRE882 = true;
    }

    internal override void CreateControls()
    {
      try
      {
        if (this.htmldoc.url.ToUpper().IndexOf("RE882") <= -1)
          return;
        this.isRE882 = true;
        this.pocInputHandler = new POCInputHandler((IHtmlInput) this.loan, this.currentForm, (InputHandlerBase) this, this.session);
        this.loCompensationInputHandler = new LOCompensationInputHandler(this.loCompensationSetting, this.inputData, this.currentForm, (InputHandlerBase) this);
      }
      catch (Exception ex)
      {
      }
    }

    protected override string GetFieldValue(string id, FieldSource fieldSource)
    {
      if ((id == "694" || id == "696") && !this.FormIsForTemplate)
        return fieldSource == FieldSource.LinkedLoan && this.loan != null && this.loan.LinkedData != null ? this.loan.LinkedData.GetSimpleField(id) : this.loan.GetSimpleField(id);
      if (!(id == "333"))
        return base.GetFieldValue(id, fieldSource);
      string fieldValue = base.GetFieldValue(id, fieldSource);
      return base.GetFieldValue("SYS.X8", fieldSource) == "Y" ? Math.Round(Utils.ParseDouble((object) fieldValue), 2).ToString("N2") : fieldValue;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      switch (id)
      {
        case "232":
        case "230":
          double num = this.ToDouble(val);
          if (num != 0.0)
          {
            val = num.ToString("N2");
            break;
          }
          break;
        case "1659":
          if (val != "Y")
          {
            this.loan.RemoveLock("RE88395.X121");
            this.loan.RemoveLock("RE88395.X122");
            break;
          }
          break;
        case "RE882.X4":
          if (val != "Y")
          {
            base.UpdateFieldValue("RE882.X5", "");
            break;
          }
          break;
      }
      base.UpdateFieldValue(id, val);
      if (id == "RE88395.X123" && val == "Y")
      {
        base.UpdateFieldValue("RE88395.X322", "");
        base.UpdateFieldValue("RE88395.X315", "");
        base.UpdateFieldValue("RE88395.X316", "");
        if (this.loan != null)
        {
          if (this.loan.IsLocked("RE88395.X315"))
            this.loan.RemoveLock("RE88395.X315");
          if (this.loan.IsLocked("RE88395.X316"))
            this.loan.RemoveLock("RE88395.X316");
        }
        base.UpdateFieldValue("RE88395.X191", "");
        base.UpdateFieldValue("RE88395.X126", "");
        base.UpdateFieldValue("RE88395.X127", "");
        base.UpdateFieldValue("RE88395.X124", "");
        base.UpdateFieldValue("RE88395.X317", "");
      }
      else if (id == "RE88395.X191" && val == "Y")
      {
        base.UpdateFieldValue("RE88395.X123", "");
      }
      else
      {
        switch (id)
        {
          case "RE88395.X322":
            if (val == "Y")
            {
              base.UpdateFieldValue("RE88395.X123", "");
              base.UpdateFieldValue("RE88395.X124", "");
              if (this.loan != null)
              {
                base.UpdateFieldValue("RE88395.X317", "");
                break;
              }
              break;
            }
            base.UpdateFieldValue("RE88395.X315", "");
            base.UpdateFieldValue("RE88395.X316", "");
            if (this.loan != null)
            {
              if (this.loan.IsLocked("RE88395.X315"))
                this.loan.RemoveLock("RE88395.X315");
              if (this.loan.IsLocked("RE88395.X316"))
              {
                this.loan.RemoveLock("RE88395.X316");
                break;
              }
              break;
            }
            break;
          case "RE88395.X124":
            if (val == "Y")
            {
              base.UpdateFieldValue("RE88395.X123", "");
              base.UpdateFieldValue("RE88395.X322", "");
              base.UpdateFieldValue("RE88395.X315", "");
              if (this.loan != null && this.loan.IsLocked("RE88395.X315"))
              {
                this.loan.RemoveLock("RE88395.X315");
                break;
              }
              break;
            }
            if (this.loan != null)
            {
              base.UpdateFieldValue("RE88395.X317", "");
              break;
            }
            break;
          default:
            if (id == "RE88395.X191" && val != "Y")
            {
              base.UpdateFieldValue("RE88395.X126", "");
              base.UpdateFieldValue("RE88395.X127", "");
              break;
            }
            break;
        }
      }
      if (id == "RE88395.X74")
        base.UpdateFieldValue("ESCROW_TABLE", "");
      if (id == "RE88395.X83")
        base.UpdateFieldValue("TITLE_TABLE", "");
      if (id == "617" || id == "624" || id == "REGZGFE.X8" || id == "L248" || id == "L252" || id == "1500" || id == "610" || id == "395" || id == "411" || id == "56")
        this.ClearFileContact(id);
      this.loan.Calculator.CopyHUD2010ToGFE2010(id, false);
      this.loan.Calculator.FormCalculation("MLDS", id, val);
      if (!this.isRE882 || this.loCompensationInputHandler == null)
        return;
      this.loCompensationInputHandler.RefreshContents();
    }

    public override void RefreshContents()
    {
      base.RefreshContents();
      if (!this.isRE882 || this.loCompensationInputHandler == null)
        return;
      this.loCompensationInputHandler.RefreshContents();
    }

    public override void onmouseenter(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.isRE882)
      {
        if (this.inputData != null && pEvtObj.srcElement.tagName == "IMG")
          this.loCompensationInputHandler.ShowToolTips(pEvtObj);
        else if (this.loan != null)
          this.pocInputHandler.TurnOnPOCToolTip(pEvtObj);
      }
      base.onmouseenter(pEvtObj);
    }

    public override void onmouseleave(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.isRE882)
      {
        if (this.inputData != null && pEvtObj.srcElement.tagName == "IMG")
          this.loCompensationInputHandler.HideToolTips(pEvtObj);
        else if (this.loan != null)
          this.pocInputHandler.TurnOffPOCToolTip();
      }
      base.onmouseleave(pEvtObj);
    }

    public override void onblur(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.loan != null && this.isRE882)
        this.pocInputHandler.TurnOffSellerPaidWarning();
      base.onblur(pEvtObj);
    }

    public override bool onkeypress(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.loan != null && this.isRE882)
        this.pocInputHandler.TurnOnSellerPaidWarning(pEvtObj);
      return base.onkeypress(pEvtObj);
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1061":
        case "436":
          if (this.loan != null && (this.loan.Use2010RESPA || this.loan.Use2015RESPA) && (this.GetFieldValue("NEWHUD.X1139") == "Y" || this.GetFieldValue("NEWHUD.X715") != "Include Origination Points" || Utils.ParseDouble((object) this.GetFieldValue("3119")) > 0.0))
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "1063":
        case "11":
          return controlState;
        case "1401":
        case "ccprog":
        case "loanprog":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "1619":
          if (this.loan != null && this.loan.GetField("1172") == "VA" && this.loan.GetField("958") == "IRRRL" && this.loan.GetField("19").IndexOf("Refinance") > -1)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "1659":
          if (this.loan.GetField("1659") != "Y")
          {
            this.SetControlState("FieldLock10", false);
            this.SetControlState("FieldLock11", false);
            this.SetControlState("Calendar4", false);
            goto case "1063";
          }
          else
          {
            this.SetControlState("FieldLock10", true);
            this.SetControlState("FieldLock11", true);
            if (this.loan.IsLocked("RE88395.X122"))
            {
              this.SetControlState("Calendar4", true);
              goto case "1063";
            }
            else
            {
              this.SetControlState("Calendar4", false);
              goto case "1063";
            }
          }
        case "364":
          if (this.FormIsForTemplate || InputHandlerBase.LockLoanNumber)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "9":
          if (this.loan.GetField("19") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "RE882.X5":
          if (this.loan != null && this.loan.GetField("RE882.X4") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "RE88395.X126":
        case "RE88395.X127":
          if (this.loan != null && this.loan.GetField("RE88395.X191") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "RE88395.X315":
          if (this.loan != null && this.loan.GetField("RE88395.X322") != "Y")
          {
            this.SetControlState("FieldLock_X315", false);
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
          {
            this.SetControlState("FieldLock_X315", true);
            controlState = ControlState.Default;
            goto case "1063";
          }
        case "RE88395.X316":
          if (this.loan != null && this.loan.GetField("RE88395.X322") != "Y")
          {
            this.SetControlState("FieldLock_X316", false);
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
          {
            this.SetControlState("FieldLock_X316", true);
            controlState = ControlState.Default;
            goto case "1063";
          }
        case "RE88395.X317":
          if (this.loan != null && this.loan.GetField("RE88395.X124") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        default:
          controlState = ControlState.Default;
          goto case "1063";
      }
    }

    internal override void FlipLockField(FieldLock fieldLock)
    {
      if ((fieldLock.ControlID == "FieldLock10" || fieldLock.ControlID == "FieldLock11") && this.loan != null && this.loan.GetField("1659") != "Y")
        return;
      base.FlipLockField(fieldLock);
      if (this.loan == null)
        return;
      this.loan.Calculator.FormCalculation("MLDS", (string) null, (string) null);
    }

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!pEvtObj.altKey || !pEvtObj.ctrlKey || pEvtObj.keyCode != 83 || DialogResult.Yes != Utils.Dialog((IWin32Window) null, "Warning: This action will synchronize data from \"GFE - Itemization\" to \"MLDS - CA GFE\", Any unmatched values will be overridden. Click Yes to proceed and No to cancel.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
        return;
      this.loan.Calculator.CopyGFEToMLDS();
      this.RefreshContents();
    }

    public override void ExecAction(string action)
    {
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
      {
        case 191989852:
          if (!(action == "hazins"))
            return;
          base.ExecAction(action);
          this.SetFieldFocus("l_251");
          return;
        case 270803988:
          if (!(action == "copyitemizationtomlds"))
            return;
          break;
        case 287038707:
          if (!(action == "hudsetup"))
            return;
          if (new HUD1ESSetupDialog(this.loan).ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
          {
            this.loan.Calculator.FormCalculation("HUD1ES", (string) null, (string) null);
            this.UpdateContents();
          }
          this.SetFieldFocus("l_610");
          return;
        case 511528202:
          if (!(action == "cityfee"))
            return;
          base.ExecAction(action);
          this.SetFieldFocus("l_1637");
          return;
        case 755860596:
          if (!(action == "prepayment"))
            return;
          base.ExecAction(action);
          this.SetFieldFocus("l_X191");
          return;
        case 803495632:
          if (!(action == "copyvoltomlds"))
            return;
          base.ExecAction(action);
          this.SetFieldFocus("l_RE88395X128");
          return;
        case 861718158:
          if (!(action == "contactmic"))
            return;
          this.GetContactItem("L248");
          this.SetFieldFocus("l_248");
          return;
        case 888963497:
          if (!(action == "contactcre"))
            return;
          this.GetContactItem("624");
          this.SetFieldFocus("l_624");
          return;
        case 1093376327:
          if (!(action == "contactdoc"))
            return;
          this.GetContactItem("395");
          this.SetFieldFocus("l_395");
          return;
        case 1132459806:
          if (!(action == "statefee"))
            return;
          FeeListDialog feeListDialog1 = new FeeListDialog("state", this.loan, this.session);
          if (feeListDialog1.ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
          {
            base.UpdateFieldValue("RE88395.X89", feeListDialog1.FeeTotal);
            base.UpdateFieldValue("1638", feeListDialog1.FeeDescription);
            this.loan.Calculator.FormCalculation("MLDS", "RE88395.X89", this.GetFieldValue("RE88395.X89"));
            this.UpdateContents();
          }
          this.SetFieldFocus("l_1638");
          return;
        case 1181978758:
          if (!(action == "ownercoverage"))
            return;
          this.setCoverageFee(false);
          return;
        case 1182883270:
          if (!(action == "copyvoltomlds2"))
            return;
          base.ExecAction(action);
          this.SetFieldFocus("l_RE88395X137");
          return;
        case 1370251432:
          if (!(action == "ownerins"))
            return;
          base.ExecAction(action);
          this.SetFieldFocus("l_L251");
          return;
        case 1393412611:
          if (!(action == "titletable"))
            return;
          if (new TableHandler(this.loan, "RE88395", this.session).LookUpTable("Title"))
          {
            this.loan.Calculator.FormCalculation("MLDS", "RE88395.X83", this.GetFieldValue("RE88395.X83"));
            this.UpdateContents();
          }
          this.SetFieldFocus("l_83");
          return;
        case 1473801479:
          if (!(action == "statetax"))
            return;
          this.setRecordingFee(RecordingFeeDialog.FeeTypes.StateTax);
          return;
        case 1628156329:
          if (!(action == "lendercoverage"))
            return;
          this.setCoverageFee(true);
          return;
        case 1797042560:
          if (!(action == "contactund"))
            return;
          this.GetContactItem("REGZGFE.X8");
          this.SetFieldFocus("l_X8");
          return;
        case 1958004838:
          if (!(action == "escrowtable"))
            return;
          if (new TableHandler(this.loan, "RE88395", this.session).LookUpTable("Escrow"))
          {
            this.loan.Calculator.FormCalculation("MLDS", "RE88395.X74", this.GetFieldValue("RE88395.X74"));
            this.UpdateContents();
          }
          this.SetFieldFocus("l_74");
          return;
        case 2104362359:
          if (!(action == "2010settlementfee"))
            return;
          base.ExecAction(action);
          this.SetFieldFocus("l_L287");
          return;
        case 3050596908:
          if (!(action == "recordingfee"))
            return;
          base.ExecAction(action);
          this.SetFieldFocus("l_1636");
          return;
        case 3100944149:
          if (!(action == "localtax"))
            return;
          base.ExecAction(action);
          this.SetFieldFocus("l_1637");
          return;
        case 3165400720:
          if (!(action == "contactesc"))
            return;
          this.GetContactItem("610");
          this.SetFieldFocus("l_610");
          return;
        case 3302304154:
          if (!(action == "mtginsreserv"))
            return;
          base.ExecAction(action);
          if (this.inputData.IsLocked("232"))
          {
            this.SetFieldFocus("l_232");
            return;
          }
          this.SetFieldFocus("l_X63");
          return;
        case 3322654771:
          if (!(action == "helpOnMLDS"))
            return;
          JedHelp.ShowHelp((System.Windows.Forms.Control) this.mainScreen, SystemSettings.HelpFile, "MLDS_Costs_and_Fees");
          return;
        case 3582764703:
          if (!(action == "loanprog"))
            return;
          base.ExecAction(action);
          this.SetFieldFocus("l_682");
          return;
        case 3709982155:
          if (!(action == "mtginsprem"))
            return;
          base.ExecAction(action);
          this.SetFieldFocus("l_X42");
          return;
        case 3892120224:
          if (!(action == "contacttit"))
            return;
          this.GetContactItem("411");
          this.SetFieldFocus("l_411");
          return;
        case 3893795657:
          if (!(action == "contacthoi"))
            return;
          this.GetContactItem("L252");
          this.SetFieldFocus("l_252");
          return;
        case 3949279623:
          if (!(action == "taxesreserv"))
            return;
          base.ExecAction(action);
          this.SetFieldFocus("l_1386");
          return;
        case 4073270834:
          if (!(action == "2010lendertitleinsurance"))
            return;
          base.ExecAction(action);
          this.SetFieldFocus("l_RE882X12");
          return;
        case 4082557018:
          if (!(action == "openitemization"))
            return;
          break;
        case 4113929720:
          if (!(action == "userfee2"))
            return;
          FeeListDialog feeListDialog2 = new FeeListDialog("user", this.loan, this.session);
          if (feeListDialog2.ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
          {
            base.UpdateFieldValue("RE88395.X100", feeListDialog2.FeeTotal);
            base.UpdateFieldValue("1640", feeListDialog2.FeeDescription);
            this.loan.Calculator.FormCalculation("MLDS", "RE88395.X100", this.GetFieldValue("RE88395.X100"));
            this.UpdateContents();
          }
          this.SetFieldFocus("l_1640");
          return;
        case 4130707339:
          if (!(action == "userfee3"))
            return;
          FeeListDialog feeListDialog3 = new FeeListDialog("user", this.loan, this.session);
          if (feeListDialog3.ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
          {
            base.UpdateFieldValue("RE88395.X169", feeListDialog3.FeeTotal);
            base.UpdateFieldValue("1643", feeListDialog3.FeeDescription);
            this.loan.Calculator.FormCalculation("MLDS", "RE88395.X169", this.GetFieldValue("RE88395.X169"));
            this.UpdateContents();
          }
          this.SetFieldFocus("l_1643");
          return;
        case 4155076599:
          if (!(action == "ccprog"))
            return;
          base.ExecAction(action);
          this.SetFieldFocus("l_682");
          return;
        case 4164262577:
          if (!(action == "userfee1"))
            return;
          FeeListDialog feeListDialog4 = new FeeListDialog("user", this.loan, this.session);
          if (feeListDialog4.ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
          {
            base.UpdateFieldValue("RE88395.X99", feeListDialog4.FeeTotal);
            base.UpdateFieldValue("373", feeListDialog4.FeeDescription);
            this.loan.Calculator.FormCalculation("MLDS", "RE88395.X99", this.GetFieldValue("RE88395.X99"));
            this.UpdateContents();
          }
          this.SetFieldFocus("l_373");
          return;
        case 4180047638:
          if (!(action == "contactapp"))
            return;
          this.GetContactItem("617");
          this.SetFieldFocus("l_617");
          return;
        case 4246466566:
          if (!(action == "contactatt"))
            return;
          this.GetContactItem("56");
          this.SetFieldFocus("l_56");
          return;
        default:
          return;
      }
      base.ExecAction(action);
      this.SetFieldFocus("l_389");
    }

    public override bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.loan != null && this.isRE882 && this.pocInputHandler.IsPOCFieldClicked(this.CurrentFieldID))
      {
        this.pocInputHandler.TurnOnPOCEntryBox(this.CurrentFieldID, pEvtObj);
        return false;
      }
      bool flag = base.onclick(pEvtObj);
      RuntimeControl controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as RuntimeControl;
      if (controlForElement is FieldLock && controlForElement.ControlID == "b_RE88395X152" && this.loan != null)
      {
        this.loan.Calculator.FormCalculation("MLDS", "RE88395.X152", this.GetFieldValue("RE88395.X152"));
        this.RefreshContents();
      }
      return flag;
    }

    private void setRecordingFee(RecordingFeeDialog.FeeTypes feeType)
    {
      using (RecordingFeeDialog recordingFeeDialog = new RecordingFeeDialog(feeType, (IHtmlInput) this.loan, false, RecordingFeeDialog.FormTypes.MLDS))
      {
        if (recordingFeeDialog.ShowDialog((IWin32Window) Session.MainForm) == DialogResult.OK)
        {
          switch (feeType)
          {
            case RecordingFeeDialog.FeeTypes.RecordingFee:
              this.loan.Calculator.FormCalculation("MLDS", "RE88395.X91", this.loan.GetField("RE88395.X91"));
              break;
            case RecordingFeeDialog.FeeTypes.LocalTax:
              this.loan.Calculator.FormCalculation("MLDS", "RE88395.X94", this.loan.GetField("RE88395.X94"));
              break;
          }
          this.RefreshContents();
        }
        switch (feeType)
        {
          case RecordingFeeDialog.FeeTypes.RecordingFee:
            this.SetFieldFocus("l_1636");
            break;
          case RecordingFeeDialog.FeeTypes.LocalTax:
            this.SetFieldFocus("l_1637");
            break;
          case RecordingFeeDialog.FeeTypes.StateTax:
            this.SetFieldFocus("l_1638");
            break;
        }
      }
    }

    private void setCoverageFee(bool forLender)
    {
      using (CoverageDialog coverageDialog = new CoverageDialog(this.loan, forLender, false))
      {
        if (coverageDialog.ShowDialog((IWin32Window) Session.MainForm) == DialogResult.OK)
          this.RefreshContents();
        if (forLender)
          this.SetFieldFocus("l_652");
        else
          this.SetFieldFocus("l_1633");
      }
    }
  }
}

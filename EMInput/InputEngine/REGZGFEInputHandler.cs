// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.REGZGFEInputHandler
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
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class REGZGFEInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Panel pnlForm;
    private EllieMae.Encompass.Forms.Label labelFormName;
    private EllieMae.Encompass.Forms.Label labelItemM7;
    private EllieMae.Encompass.Forms.Panel pnlItemM7;
    private EllieMae.Encompass.Forms.Panel pnlISectionMVersion1;
    private EllieMae.Encompass.Forms.Panel pnlISectionMVersion2;
    private EllieMae.Encompass.Forms.Panel pnlISectionN;
    private EllieMae.Encompass.Forms.Label labelSectionLversion1;
    private EllieMae.Encompass.Forms.Label labelSectionLversion2;
    private EllieMae.Encompass.Forms.Label labelM11;
    private EllieMae.Encompass.Forms.GroupBox grpDetailofTransaction;
    private EllieMae.Encompass.Forms.Panel pnlItemINoHeloc;
    private EllieMae.Encompass.Forms.Panel pnlItemIHeloc;
    private EllieMae.Encompass.Forms.Panel pnlBorPaidFees;
    private EllieMae.Encompass.Forms.Panel pnlBorPaidFeesNew;
    private EllieMae.Encompass.Forms.CheckBox itemizedCheckBox;
    private bool firstLoading = true;
    private EllieMae.Encompass.Forms.Panel pnlGoodFaithEstimate;

    public REGZGFEInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public REGZGFEInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public REGZGFEInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public REGZGFEInputHandler(
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
        this.pnlItemINoHeloc = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlItemINoHeloc");
        this.pnlItemIHeloc = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlItemIHeloc");
        if (this.pnlItemIHeloc != null)
        {
          this.pnlItemIHeloc.Left = 1;
          this.pnlItemIHeloc.Top = this.pnlItemINoHeloc.Top;
        }
        this.pnlForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        this.labelFormName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label191");
        this.grpDetailofTransaction = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("grpDetailofTransaction");
        this.pnlItemM7 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlItemM7");
        this.labelItemM7 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label_M7");
        this.labelM11 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label315");
        this.pnlISectionMVersion1 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("MSectionVersion1");
        this.pnlISectionMVersion2 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("MSectionVersion2");
        this.pnlISectionN = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("SectionN");
        this.labelSectionLversion1 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("SectionLRespaVersion1");
        this.labelSectionLversion2 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("SectionLRespaVersion2");
        if (this.pnlISectionMVersion1 != null && this.pnlISectionMVersion2 != null)
          this.pnlISectionMVersion2.Position = this.pnlISectionMVersion1.Position;
        if (this.labelSectionLversion1 != null && this.labelSectionLversion2 != null)
          this.labelSectionLversion2.Position = this.labelSectionLversion1.Position;
        this.pnlBorPaidFees = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("BorPaidFeesPanel");
        this.pnlBorPaidFeesNew = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("BorPaidFeesNewPanel");
        this.itemizedCheckBox = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("CheckBox301");
        this.pnlGoodFaithEstimate = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("Panel83");
      }
      catch (Exception ex)
      {
      }
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      this.firstLoading = false;
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.pnlISectionMVersion1 == null)
        return;
      this.updateDetailOfTransctionLayout();
    }

    private void updateDetailOfTransctionLayout()
    {
      if (this.inputData.GetField("1172") == "HELOC")
      {
        this.pnlItemIHeloc.Visible = true;
        this.pnlItemINoHeloc.Visible = false;
      }
      else
      {
        this.pnlItemIHeloc.Visible = false;
        this.pnlItemINoHeloc.Visible = true;
      }
      if (this.inputData.GetField("4796") == "Y")
      {
        if (!this.firstLoading)
        {
          this.pnlISectionMVersion2.Visible = true;
          this.pnlISectionMVersion1.Visible = false;
        }
        this.pnlISectionN.Position = new Point(0, 816);
        this.enableOrDisableDropdownPanels(this.pnlBorPaidFeesNew, this.pnlBorPaidFees);
        this.labelSectionLversion2.Visible = true;
        this.labelSectionLversion1.Visible = false;
        if (this.currentForm.Name != "HUD-1 Page 2")
        {
          this.labelFormName.Position = new Point(0, 3810);
          this.pnlForm.Size = new Size(669, 3800);
          this.pnlGoodFaithEstimate.Position = new Point(7, 3720);
        }
        else
        {
          this.labelFormName.Position = new Point(0, 3909);
          this.pnlForm.Size = new Size(669, 3899);
        }
      }
      else
      {
        if (!this.firstLoading)
        {
          this.pnlISectionMVersion1.Visible = true;
          this.pnlISectionMVersion2.Visible = false;
        }
        this.pnlISectionN.Position = new Point(0, 672);
        this.enableOrDisableDropdownPanels(this.pnlBorPaidFees, this.pnlBorPaidFeesNew);
        this.labelSectionLversion2.Visible = false;
        this.labelSectionLversion1.Visible = true;
        if (this.currentForm.Name != "HUD-1 Page 2")
        {
          this.labelFormName.Position = new Point(0, 3680);
          this.pnlForm.Size = new Size(669, 3670);
          this.pnlGoodFaithEstimate.Position = new Point(7, 3590);
        }
        else
        {
          this.labelFormName.Position = new Point(0, 3769);
          this.pnlForm.Size = new Size(669, 3759);
        }
      }
      if (!(this.inputData.GetField("1825") == "2020"))
        this.itemizedCheckBox.Visible = false;
      else
        this.itemizedCheckBox.Visible = true;
      EllieMae.Encompass.Forms.GroupBox detailofTransaction = this.grpDetailofTransaction;
      Size size1 = this.grpDetailofTransaction.Size;
      int width = size1.Width;
      int top = this.pnlISectionN.Top;
      size1 = this.pnlISectionN.Size;
      int height1 = size1.Height;
      int height2 = top + height1;
      Size size2 = new Size(width, height2);
      detailofTransaction.Size = size2;
    }

    protected override string GetFieldValue(string id, FieldSource fieldSource)
    {
      if (!(id == "694") && !(id == "696") || this.FormIsForTemplate)
        return base.GetFieldValue(id, fieldSource);
      return fieldSource == FieldSource.LinkedLoan && this.loan != null && this.loan.LinkedData != null ? this.loan.LinkedData.GetSimpleField(id) : this.loan.GetSimpleField(id);
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1063":
          return controlState;
        case "1134":
          if (this.GetField("19") == "Purchase")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "1401":
        case "364":
        case "ccprog":
        case "loanprog":
          if (this.loan == null || this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "142":
          EllieMae.Encompass.Forms.Label control1 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblCashFromTo");
          if (control1 != null)
            control1.Text = this.ToDouble(this.loan.GetSimpleField("142")) <= 0.0 ? "Cash to borrower" : "Cash from borrower";
          controlState = ControlState.Default;
          goto case "1063";
        case "1619":
        case "559":
          if (this.loan.GetField("1172") == "VA" && this.loan.GetField("958") == "IRRRL" && this.loan.GetField("19").IndexOf("Refinance") > -1)
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "1663":
        case "1665":
        case "1847":
        case "1848":
          controlState = ControlState.Default;
          this.FormatAlphaNumericField(ctrl, id);
          goto case "1063";
        case "1845":
          if (this.loan.GetField("420") == "FirstLien")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "1851":
          EllieMae.Encompass.Forms.Label control2 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblCCFrom");
          if (control2 != null)
          {
            control2.Text = !(this.loan.GetSimpleField("420") == "SecondLien") ? "CC from 2nd" : "CC from 1st";
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
        default:
          controlState = ControlState.Default;
          goto case "1063";
      }
    }

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!pEvtObj.altKey || !pEvtObj.ctrlKey || pEvtObj.keyCode != 83 || DialogResult.Yes != Utils.Dialog((IWin32Window) null, "Warning: This action will synchronize data from \"MLDS - CA GFE\" to \"GFE - Itemization\", Any unmatched values will be overridden. Click Yes to proceed and No to cancel.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
        return;
      this.loan.Calculator.CopyMLDSToGFE();
      this.RefreshContents();
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      switch (id)
      {
        case "1663":
        case "1665":
        case "230":
        case "232":
          double num1 = this.ToDouble(val);
          if (num1 != 0.0)
          {
            val = num1.ToString("N2");
            break;
          }
          break;
        case "1847":
        case "1848":
          double num2 = this.ToDouble(val);
          if (num2 != 0.0)
          {
            val = num2.ToString("N3");
            break;
          }
          break;
        case "1878":
          base.UpdateFieldValue("GFE3", "");
          break;
        case "448":
          base.UpdateFieldValue("GFE1", "");
          break;
        case "449":
          base.UpdateFieldValue("GFE2", "");
          break;
      }
      base.UpdateFieldValue(id, val);
      if (id == "387")
        base.UpdateFieldValue("ESCROW_TABLE", "");
      if (id == "385")
        base.UpdateFieldValue("TITLE_TABLE", "");
      if (id == "617" || id == "624" || id == "REGZGFE.X8" || id == "L248" || id == "L252" || id == "1500" || id == "610" || id == "395" || id == "411" || id == "56")
        this.ClearFileContact(id);
      if (this.loan == null)
        return;
      this.loan.Calculator.FormCalculation("REGZ", id, val);
      this.loan.SyncPiggyBackFiles((string[]) null, true, false, id, val, false);
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
      {
        case 191989852:
          if (!(action == "hazins"))
            break;
          this.SetFieldFocus("haz");
          break;
        case 287038707:
          if (!(action == "hudsetup"))
            break;
          this.SetFieldFocus("l_610");
          break;
        case 511528202:
          if (!(action == "cityfee"))
            break;
          this.SetFieldFocus("l_1637");
          break;
        case 861718158:
          if (!(action == "contactmic"))
            break;
          this.SetFieldFocus("l_248");
          break;
        case 888963497:
          if (!(action == "contactcre"))
            break;
          this.SetFieldFocus("l_624");
          break;
        case 1014055698:
          if (!(action == "contactflo"))
            break;
          this.GetContactItem("1500");
          this.SetFieldFocus("l_1500");
          break;
        case 1093376327:
          if (!(action == "contactdoc"))
            break;
          this.SetFieldFocus("l_395");
          break;
        case 1132459806:
          if (!(action == "statefee"))
            break;
          this.SetFieldFocus("l_1638");
          break;
        case 1181978758:
          if (!(action == "ownercoverage"))
            break;
          this.SetFieldFocus("l_1633");
          break;
        case 1393412611:
          if (!(action == "titletable"))
            break;
          this.SetFieldFocus("l_385");
          break;
        case 1473801479:
          if (!(action == "statetax"))
            break;
          this.SetFieldFocus("l_1638");
          break;
        case 1497836931:
          if (!(action == "othersummaries"))
            break;
          this.SetFieldFocus("l_136");
          break;
        case 1628156329:
          if (!(action == "lendercoverage"))
            break;
          this.SetFieldFocus("l_652");
          break;
        case 1797042560:
          if (!(action == "contactund"))
            break;
          this.SetFieldFocus("l_X8");
          break;
        case 1801894496:
          if (!(action == "helpOnGFE1"))
            break;
          JedHelp.ShowHelp((System.Windows.Forms.Control) this.mainScreen, SystemSettings.HelpFile, "GFE_Costs_and_Fees");
          break;
        case 1852227353:
          if (!(action == "helpOnGFE2"))
            break;
          JedHelp.ShowHelp((System.Windows.Forms.Control) this.mainScreen, SystemSettings.HelpFile, "Closing_Costs_Summary");
          break;
        case 1958004838:
          if (!(action == "escrowtable"))
            break;
          this.SetFieldFocus("l_387");
          break;
        case 2577051740:
          if (!(action == "hud1a"))
            break;
          this.SetFieldFocus("l_136");
          break;
        case 2728661377:
          if (!(action == "mipff"))
            break;
          this.SetFieldFocus("l_1109");
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
        case 3165400720:
          if (!(action == "contactesc"))
            break;
          this.SetFieldFocus("l_610");
          break;
        case 3302304154:
          if (!(action == "mtginsreserv"))
            break;
          this.SetFieldFocus("mtir");
          break;
        case 3582764703:
          if (!(action == "loanprog"))
            break;
          this.SetFieldFocus("l_682");
          break;
        case 3709982155:
          if (!(action == "mtginsprem"))
            break;
          this.SetFieldFocus("l_562");
          break;
        case 3892120224:
          if (!(action == "contacttit"))
            break;
          this.SetFieldFocus("l_411");
          break;
        case 3893795657:
          if (!(action == "contacthoi"))
            break;
          this.SetFieldFocus("l_L252");
          break;
        case 3949279623:
          if (!(action == "taxesreserv"))
            break;
          this.SetFieldFocus("taxes");
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
          this.SetFieldFocus("l_682");
          break;
        case 4164262577:
          if (!(action == "userfee1"))
            break;
          this.SetFieldFocus("l_373");
          break;
        case 4180047638:
          if (!(action == "contactapp"))
            break;
          this.SetFieldFocus("l_617");
          break;
        case 4246466566:
          if (!(action == "contactatt"))
            break;
          this.SetFieldFocus("l_56");
          break;
      }
    }
  }
}

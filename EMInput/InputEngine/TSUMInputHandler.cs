// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TSUMInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class TSUMInputHandler : InputHandlerBase
  {
    private List<EllieMae.Encompass.Forms.Panel> panelLocalAddresses;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignAddresses;
    private List<StandardButton> selectCountryButtons;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignPanels;
    private EllieMae.Encompass.Forms.Panel conactTitlePanel;

    public TSUMInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public TSUMInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public TSUMInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public TSUMInputHandler(
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
      base.CreateControls();
      try
      {
        this.panelLocalAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignPanels = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelLocalAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("localAdr1"));
        this.panelForeignAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignAdr1"));
        if (this.panelForeignAddresses[0] != null && this.panelLocalAddresses[0] != null)
          this.panelForeignAddresses[0].Position = this.panelLocalAddresses[0].Position;
        this.selectCountryButtons = new List<StandardButton>();
        this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_4679"));
        for (int index = 1; index <= 2; ++index)
          this.panelForeignPanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignPanel" + (object) index));
        this.conactTitlePanel = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("contactTitlePanel");
      }
      catch
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "101":
        case "102":
        case "103":
        case "104":
        case "107":
        case "108":
        case "11":
        case "110":
        case "111":
        case "112":
        case "113":
        case "116":
        case "117":
          return controlState;
        case "1556":
          if (this.loan.GetField("1543") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "299":
          controlState = this.loan.GetField("19") == "Purchase" || this.loan.GetField("19") == "Other" ? ControlState.Disabled : ControlState.Enabled;
          goto case "101";
        case "364":
          if (this.FormIsForTemplate || InputHandlerBase.LockLoanNumber)
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "4679":
          if (this.loan.GetField("4678") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "994":
          if (this.loan.GetField("608") != "OtherAmortizationType")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "995":
          if (this.loan.GetField("608") != "AdjustableRate" && this.inputData.GetField("1172") != "HELOC")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "baseincome":
          if (this.GetField("1825") == "2020")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "subfin":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        default:
          controlState = ControlState.Default;
          goto case "101";
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if ((id == "991" || id == "1149") && val == "N")
        val = string.Empty;
      base.UpdateFieldValue(id, val);
      if (id == "608")
      {
        switch (val)
        {
          case "Fixed":
            base.UpdateFieldValue("994", string.Empty);
            break;
          case "AdjustableRate":
            base.UpdateFieldValue("994", string.Empty);
            break;
        }
      }
      if (id == "1553")
      {
        switch (val)
        {
          case "Manufactured Housing Single Wide":
            base.UpdateFieldValue("ULDD.ManufacturedHomeWidthType", "ManufacturedSingleWide");
            break;
          case "Manufactured Housing Multiwide":
            base.UpdateFieldValue("ULDD.ManufacturedHomeWidthType", "ManufacturedMultiwide");
            break;
          default:
            base.UpdateFieldValue("ULDD.ManufacturedHomeWidthType", string.Empty);
            break;
        }
      }
      if (!(id == "TSUM.PropertyFormType"))
        return;
      base.UpdateFieldValue("ULDD.FRE.PropertyFormType", val);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(val))
      {
        case 213836621:
          if (!(val == "Two To Four Unit Residential Appraisal"))
            break;
          base.UpdateFieldValue("ULDD.FNM.PropertyFormType", "FNM 2000A / FRE 1072 = Two To Four Unit Residential Appraisal");
          break;
        case 1624383784:
          if (!(val == "One Unit Residential Appraisal Field Review Report"))
            break;
          base.UpdateFieldValue("ULDD.FNM.PropertyFormType", "FNM 2000 / FRE1032 = One Unit Residential Appraisal Field Review Report");
          break;
        case 1945541913:
          if (!(val == "Small Residential Income Property Appraisal Report"))
            break;
          base.UpdateFieldValue("ULDD.FNM.PropertyFormType", "FNM 1025 / FRE 72 = Small Residential Income Property Appraisal Report");
          break;
        case 2168029724:
          if (!(val == "Exterior Only Inspection Individual Cooperative Interest Appraisal Report"))
            break;
          base.UpdateFieldValue("ULDD.FNM.PropertyFormType", "FNM 2095 = Exterior Only Inspection Individual Cooperative Interest Appraisal Report");
          break;
        case 2311104136:
          if (!(val == "Uniform Residential Appraisal Report"))
            break;
          base.UpdateFieldValue("ULDD.FNM.PropertyFormType", "FNM 1004 / FRE 70 = Uniform Residential Appraisal Report");
          break;
        case 2774529652:
          if (!(val == "Individual Cooperative Interest Appraisal Report"))
            break;
          base.UpdateFieldValue("ULDD.FNM.PropertyFormType", "FNM 2090 = Individual Cooperative Interest Appraisal Report");
          break;
        case 2959858427:
          if (!(val == "Individual Condominium Unit Appraisal Report"))
            break;
          base.UpdateFieldValue("ULDD.FNM.PropertyFormType", "FNM 1073 / FRE 465 = Individual Condominium Unit Appraisal Report");
          break;
        case 3059959148:
          if (!(val == "Exterior Only Inspection Residential Appraisal Report"))
            break;
          base.UpdateFieldValue("ULDD.FNM.PropertyFormType", "FNM 2055 / FRE 2055 = Exterior Only Inspection Residential Appraisal Report");
          break;
        case 3132666464:
          if (!(val == "Manufactured Home Appraisal Report"))
            break;
          base.UpdateFieldValue("ULDD.FNM.PropertyFormType", "FNM 1004C / FRE 70B = Manufactured Home Appraisal Report");
          break;
        case 3168986995:
          if (!(val == "Exterior Only Inspection Individual Condominium Unit Appraisal Report"))
            break;
          base.UpdateFieldValue("ULDD.FNM.PropertyFormType", "FNM 1075 / FRE 466 = Exterior Only Inspection Individual Condominium Unit Appraisal Report");
          break;
        case 3262274919:
          if (!(val == "Desktop Underwriter Property Inspection Report"))
            break;
          base.UpdateFieldValue("ULDD.FNM.PropertyFormType", "DU Form 2075 - Desktop Underwriter Property Inspection Report");
          break;
        case 3581629925:
          if (!(val == "Appraisal Update And Or Completion Report"))
            break;
          base.UpdateFieldValue("ULDD.FNM.PropertyFormType", "FNM 1004D / FRE 442 = Appraisal Update And Or Completion Report");
          break;
        case 3852393734:
          if (!(val == "Loan Prospector Condition And Marketability"))
            break;
          base.UpdateFieldValue("ULDD.FNM.PropertyFormType", "");
          break;
      }
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "subfin":
          base.ExecAction(action);
          this.SetFieldFocus("aType1");
          break;
        case "selectcountry_4679":
          base.ExecAction(action);
          break;
        default:
          base.ExecAction(action);
          break;
      }
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.selectCountryButtons == null || this.selectCountryButtons.Count <= 0)
        return;
      for (int index = 0; index < this.selectCountryButtons.Count; ++index)
      {
        if (this.selectCountryButtons[index] != null)
        {
          bool flag = this.GetField("4678") == "Y";
          this.selectCountryButtons[index].Visible = flag;
          this.panelLocalAddresses[index].Visible = !flag;
          this.panelForeignAddresses[index].Visible = flag;
        }
      }
      if (this.GetField("1825") == "2020")
      {
        for (int index = 0; index < 2; ++index)
          this.panelForeignPanels[index].Visible = true;
        if (this.conactTitlePanel == null)
          return;
        this.conactTitlePanel.Position = new Point(0, 165);
      }
      else
      {
        for (int index = 0; index < 2; ++index)
          this.panelForeignPanels[index].Visible = false;
        if (this.conactTitlePanel == null)
          return;
        this.conactTitlePanel.Position = new Point(0, 143);
      }
    }
  }
}

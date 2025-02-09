// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ESCROWDETAILSInputHandler
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
  internal class ESCROWDETAILSInputHandler : InputHandlerBase
  {
    private const string className = "ESCROWDETAILSInputHandler";

    public ESCROWDETAILSInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public ESCROWDETAILSInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public ESCROWDETAILSInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public ESCROWDETAILSInputHandler(
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
      string.Equals(Session.ConfigurationManager.GetCompanySetting("Policies", "ENABLEESCROWDETAILSANDCALC"), "true", StringComparison.CurrentCultureIgnoreCase);
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "CPA.ADDLESCROW.1007Amount":
        case "CPA.ADDLESCROW.1008Amount":
        case "CPA.ADDLESCROW.1009Amount":
        case "CPA.ADDLESCROW.CityPropertyTax":
        case "CPA.ADDLESCROW.FloodInsurance":
        case "CPA.ADDLESCROW.HomeInsurance":
        case "CPA.ADDLESCROW.MIMIP":
        case "CPA.ADDLESCROW.PropertyTax":
        case "CPA.ADDLESCROW.USDAAnnualFee":
        case "CPA.FCD.1007Amount":
        case "CPA.FCD.1008Amount":
        case "CPA.FCD.1009Amount":
        case "CPA.FCD.AggAdjAmount":
        case "CPA.FCD.CityPropertyTax":
        case "CPA.FCD.FloodInsurance":
        case "CPA.FCD.HomeInsurance":
        case "CPA.FCD.MortgageInsurance":
        case "CPA.FCD.PropertyTax":
        case "CPA.FCD.USDAAnnualFee":
          string field = this.GetField("CPA.RetainUserInputs");
          return string.IsNullOrEmpty(field) || field == "N" ? ControlState.Disabled : ControlState.Enabled;
        default:
          return ControlState.Default;
      }
    }
  }
}

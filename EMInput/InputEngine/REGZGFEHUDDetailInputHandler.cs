// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.REGZGFEHUDDetailInputHandler
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
  internal class REGZGFEHUDDetailInputHandler : InputHandlerBase
  {
    public REGZGFEHUDDetailInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public REGZGFEHUDDetailInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public REGZGFEHUDDetailInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public REGZGFEHUDDetailInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public REGZGFEHUDDetailInputHandler(
      IWin32Window owner,
      IHtmlInput htmlInput,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, htmlInput, htmldoc, form, property)
    {
    }

    public REGZGFEHUDDetailInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput htmlInput,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, htmlInput, htmldoc, form, property)
    {
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "NEWHUD.X741":
        case "NEWHUD.X712":
        case "NEWHUD.X718":
          if (this.GetFieldValue("NEWHUD.X714") != "Include Origination Credit")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "NEWHUD.X710":
          if (this.GetFieldValue("NEWHUD.X714") != "Include Origination Points" || Utils.ParseDouble((object) this.GetFieldValue("3119")) > 0.0 || this.GetFieldValue("1172") == "VA" && this.GetFieldValue("958") == "IRRRL" && this.GetFieldValue("19").IndexOf("Refinance") > -1)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "NEWHUD.X709":
          if (this.GetFieldValue("NEWHUD.X714") != "Include Origination Points" || Utils.ParseDouble((object) this.GetFieldValue("3119")) > 0.0)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "NEWHUD.X714":
          if (Utils.ParseDouble((object) this.GetFieldValue("3119")) > 0.0)
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

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (this.loan == null)
      {
        if (id == "NEWHUD.X714")
        {
          if (val == string.Empty || val == "Include Origination Points")
          {
            base.UpdateFieldValue("NEWHUD.X712", "");
            base.UpdateFieldValue("NEWHUD.X741", "");
            base.UpdateFieldValue("NEWHUD.X718", "");
          }
          if (val == string.Empty || val == "Include Origination Credit")
          {
            base.UpdateFieldValue("NEWHUD.X709", "");
            base.UpdateFieldValue("NEWHUD.X710", "");
            base.UpdateFieldValue("3119", "");
          }
        }
        Decimal num = Utils.ParseDecimal((object) this.GetField("NEWHUD.X770")) + Utils.ParseDecimal((object) this.GetField("NEWHUD.X702")) + Utils.ParseDecimal((object) this.GetField("NEWHUD.X703")) + Utils.ParseDecimal((object) this.GetField("NEWHUD.X704")) + Utils.ParseDecimal((object) this.GetField("NEWHUD.X707")) + Utils.ParseDecimal((object) this.GetField("NEWHUD.X250")) + Utils.ParseDecimal((object) this.GetField("NEWHUD.X727")) + Utils.ParseDecimal((object) this.GetField("NEWHUD.X729")) + Utils.ParseDecimal((object) this.GetField("NEWHUD.X736")) + Utils.ParseDecimal((object) this.GetField("NEWHUD.X738")) + Utils.ParseDecimal((object) this.GetField("NEWHUD.X740")) + Utils.ParseDecimal((object) this.GetField("NEWHUD.X1420")) + Utils.ParseDecimal((object) this.GetField("NEWHUD.X1422")) + Utils.ParseDecimal((object) this.GetField("NEWHUD.X1424")) + Utils.ParseDecimal((object) this.GetField("NEWHUD.X1426")) + Utils.ParseDecimal((object) this.GetField("NEWHUD.X1428")) + Utils.ParseDecimal((object) this.GetField("NEWHUD.X1430")) + Utils.ParseDecimal((object) this.GetField("NEWHUD.X1432"));
        base.UpdateFieldValue("NEWHUD.X12", num != 0M ? num.ToString("N2") : "");
      }
      if (this.loan != null)
        this.loan.Calculator.FormCalculation("RegzGFEHUD", id, val);
      this.RefreshContents();
    }
  }
}

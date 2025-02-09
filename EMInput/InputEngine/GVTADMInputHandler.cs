// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.GVTADMInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class GVTADMInputHandler : InputHandlerBase
  {
    private List<EllieMae.Encompass.Forms.Panel> panelLocalAddresses;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignAddresses;
    private List<StandardButton> selectCountryButtons;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignPanels;

    public GVTADMInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public GVTADMInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public GVTADMInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public GVTADMInputHandler(
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
        this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_FR0130"));
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
        case "1064":
          if (this.loan.GetField("1497") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "11":
        case "FR0104":
          return controlState;
        case "3176":
          if (this.loan.GetField("3175") != "Approved" && this.loan.GetField("3175") != "Modified And Approved")
            controlState = ControlState.Disabled;
          this.SetControlState("Calendar3176", controlState != ControlState.Disabled);
          goto case "11";
        case "3177":
          if (this.loan.GetField("3175") != "Approved" && this.loan.GetField("3175") != "Modified And Approved")
            controlState = ControlState.Disabled;
          this.SetControlState("Calendar3177", controlState != ControlState.Disabled);
          goto case "11";
        case "3178":
        case "3179":
        case "3180":
        case "3181":
        case "3183":
        case "3184":
        case "3196":
          if (this.loan.GetField("3175") != "Modified And Approved")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "3182":
          if (this.loan.GetField("3175") != "Modified And Approved")
          {
            this.SetControlState("FieldLock3182", false);
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
          {
            this.SetControlState("FieldLock3182", true);
            controlState = !this.loan.IsLocked("3182") ? ControlState.Disabled : ControlState.Enabled;
            goto case "11";
          }
        case "3192":
          if (this.loan.GetField("3191") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "3194":
          if (this.loan.GetField("3193") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "3632":
        case "3633":
        case "3634":
        case "3635":
        case "3636":
        case "3637":
          if (this.loan.GetField("3639") == "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "3639":
          if (this.loan.GetField("3639") == "Y")
          {
            this.SetControlState("Rolodex1", false);
            goto case "11";
          }
          else
          {
            this.SetControlState("Rolodex1", true);
            goto case "11";
          }
        case "FR0130":
          if (this.loan.GetField("FR0129") != "Y")
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

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.selectCountryButtons != null && this.selectCountryButtons.Count > 0)
      {
        bool flag = this.GetField("FR0129") == "Y";
        this.selectCountryButtons[0].Visible = flag;
        this.panelLocalAddresses[0].Visible = !flag;
        this.panelForeignAddresses[0].Visible = flag;
        this.panelLocalAddresses[1].Visible = !flag;
        this.panelForeignAddresses[1].Visible = flag;
      }
      if (this.GetField("1825") == "2020")
      {
        for (int index = 0; index < 2; ++index)
          this.panelForeignPanels[index].Visible = true;
      }
      else
      {
        for (int index = 0; index < 2; ++index)
          this.panelForeignPanels[index].Visible = false;
      }
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      if (!(action == "mtginsreserv"))
        return;
      if (this.inputData.IsLocked("232"))
        this.SetFieldFocus("l_232");
      else
        this.SetFieldFocus("l_1059");
    }
  }
}

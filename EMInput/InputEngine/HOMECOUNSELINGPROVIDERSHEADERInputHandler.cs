// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HOMECOUNSELINGPROVIDERSHEADERInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class HOMECOUNSELINGPROVIDERSHEADERInputHandler : InputHandlerBase
  {
    public HOMECOUNSELINGPROVIDERSHEADERInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public HOMECOUNSELINGPROVIDERSHEADERInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public HOMECOUNSELINGPROVIDERSHEADERInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public HOMECOUNSELINGPROVIDERSHEADERInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "HOC.X2":
          if (this.GetField("HOC.X1") != string.Empty)
          {
            controlState = ControlState.Disabled;
            this.SetControlState("Rolodex1", false);
            break;
          }
          this.SetControlState("Rolodex1", true);
          break;
        case "HOC.X7":
          if (this.GetField("HOC.X1") != string.Empty)
          {
            controlState = ControlState.Disabled;
            this.SetControlState("ContactButton1", false);
            break;
          }
          this.SetControlState("ContactButton1", true);
          break;
        case "HOC.X8":
          if (this.GetField("HOC.X1") != string.Empty)
          {
            controlState = ControlState.Disabled;
            this.SetControlState("ContactButton2", false);
            break;
          }
          this.SetControlState("ContactButton2", true);
          break;
        case "HOC.X9":
          if (this.GetField("HOC.X1") != string.Empty)
          {
            controlState = ControlState.Disabled;
            this.SetControlState("ContactButton3", false);
            break;
          }
          this.SetControlState("ContactButton3", true);
          break;
        case "HOC.X22":
          if (this.GetField("HOC.X21") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "HOC.X23":
          if (this.GetField("HOC.X22") != "N")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        default:
          if (id != "HOC.X16" && id != "HOC.X17" && id != "HOC.X18" && id != "HOC.X19" && id != "HOC.X15" && id != "HOC.X21" && this.GetField("HOC.X1") != string.Empty)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
      }
      return controlState;
    }
  }
}

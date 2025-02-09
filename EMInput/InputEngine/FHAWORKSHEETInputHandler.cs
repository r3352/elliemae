// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FHAWORKSHEETInputHandler
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
  public class FHAWORKSHEETInputHandler : InputHandlerBase
  {
    public FHAWORKSHEETInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public FHAWORKSHEETInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public FHAWORKSHEETInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public FHAWORKSHEETInputHandler(
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
        case "9":
          if (this.loan.GetField("19") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "1134":
          if (this.loan.GetField("19") == "Purchase")
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

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
      {
        case 1080180775:
          if (!(action == "fhacashworksheet"))
            return;
          goto label_25;
        case 1497812540:
          if (!(action == "importliab"))
            return;
          break;
        case 1895711686:
          if (!(action == "ordercredit"))
            return;
          break;
        case 1943167565:
          if (!(action == "viewcredit"))
            return;
          break;
        case 2259900203:
          if (!(action == "mcawtotalcc"))
            return;
          this.SetFieldFocus("l_34");
          return;
        case 2267552023:
          if (!(action == "203kws"))
            return;
          goto label_25;
        case 2287650426:
          if (!(action == "orderappraisal"))
            return;
          this.SetFieldFocus("l_11");
          return;
        case 2728661377:
          if (!(action == "mipff"))
            return;
          this.SetFieldFocus("l_3");
          return;
        case 3348928512:
          if (!(action == "enerfyimprove"))
            return;
          goto label_25;
        case 3588432236:
          if (!(action == "hud92900lt"))
            return;
          goto label_27;
        case 3953003722:
          if (!(action == "cashinfo"))
            return;
          goto label_27;
        default:
          return;
      }
      this.SetFieldFocus("l_67");
      return;
label_25:
      this.SetFieldFocus("l_3052");
      return;
label_27:
      this.SetFieldFocus("l_1018");
    }
  }
}

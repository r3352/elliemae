// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PLANCODEDETAILSInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class PLANCODEDETAILSInputHandler(
    Sessions.Session session,
    IWin32Window owner,
    IHtmlInput input,
    HTMLDocument htmldoc,
    EllieMae.Encompass.Forms.Form form,
    object property) : InputHandlerBase(session, input, htmldoc, form, property)
  {
    public PLANCODEDETAILSInputHandler(
      IWin32Window owner,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, input, htmldoc, form, property)
    {
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string fieldIdOrAction)
    {
      return ControlState.Disabled;
    }

    protected override string GetFieldValue(string id)
    {
      if (string.Compare(id, "PlanCode.ProgSpnsrNm", true) == 0 && ((Plan) this.inputData).HideInvestorName)
        return "";
      return string.Compare(id, "SYS.X1", true) == 0 && !((Plan) this.inputData).ContainsField("SYS.X1") ? "NA" : base.GetFieldValue(id);
    }
  }
}

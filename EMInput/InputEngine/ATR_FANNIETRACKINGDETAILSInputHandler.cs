// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ATR_FANNIETRACKINGDETAILSInputHandler
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
  public class ATR_FANNIETRACKINGDETAILSInputHandler : InputHandlerBase
  {
    private IWin32Window owner;
    private bool dataIsReadOnly;

    public ATR_FANNIETRACKINGDETAILSInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public ATR_FANNIETRACKINGDETAILSInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public ATR_FANNIETRACKINGDETAILSInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public ATR_FANNIETRACKINGDETAILSInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public ATR_FANNIETRACKINGDETAILSInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, input, htmldoc, form, property)
    {
      this.owner = owner;
    }

    internal override void CreateControls()
    {
      if (this.inputData == null || !(this.inputData is DataTemplate) || !((DataTemplate) this.inputData).UsedForGeneralDataInput)
        return;
      this.dataIsReadOnly = ((DataTemplate) this.inputData).ReadOnly;
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      this.getControlState(ctrl, id, ControlState.Enabled);
      return !this.dataIsReadOnly ? ControlState.Default : ControlState.Disabled;
    }
  }
}

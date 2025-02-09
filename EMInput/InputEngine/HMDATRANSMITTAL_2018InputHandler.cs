// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HMDATRANSMITTAL_2018InputHandler
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
  internal class HMDATRANSMITTAL_2018InputHandler : InputHandlerBase
  {
    public HMDATRANSMITTAL_2018InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public HMDATRANSMITTAL_2018InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public HMDATRANSMITTAL_2018InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public HMDATRANSMITTAL_2018InputHandler(
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
        if (this.loan != null && this.loan.Calculator != null)
        {
          this.loan.Calculator.CalcOnDemand(CalcOnDemandEnum.PaymentSchedule);
          if (this.loan.GetField("3296") == string.Empty && this.loan.GetField("608") == "AdjustableRate")
            this.loan.Calculator.CalcRateCap();
          this.loan.Calculator.FormCalculation("POPULATESUBJECTPROPERTYADDRESS", (string) null, (string) null);
        }
        if (this.loan == null || this.loan.Calculator == null || this.loan.Settings == null || this.loan.Settings.HMDAInfo == null)
          return;
        this.loan.Calculator.FormCalculation("UPDATEHMDA2018", (string) null, (string) null);
      }
      catch (Exception ex)
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      this.getControlState(ctrl, id, ControlState.Enabled);
      return this.loan.Settings.HMDAInfo != null ? ControlState.Disabled : ControlState.Default;
    }
  }
}

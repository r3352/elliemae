// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ATR_TRACKINGDETAILSInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
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
  public class ATR_TRACKINGDETAILSInputHandler : InputHandlerBase
  {
    private IWin32Window owner;
    private bool usedForGeneralDataInput;
    private bool isManualEntry;
    private bool isLQAEntry;
    private bool isEarlycheckEntry;
    private bool dataIsReadOnly;
    private InputFormInfo fannieFormInfo = new InputFormInfo("ATR_FannieTrackingDetails", "ATR_FannieTrackingDetails");
    private InputFormInfo freddieFormInfo = new InputFormInfo("ATR_FreddieTrackingDetails", "ATR_FreddieTrackingDetails");
    private const string MAINFORMNAME = "AUS Tracking Details - ";
    private const int MAINFORMNAMEWIDTH = 600;
    private const int MAINFORMNAMEHEIGHT = 600;
    private DropdownBox fieldAUSX1;

    public ATR_TRACKINGDETAILSInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public ATR_TRACKINGDETAILSInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public ATR_TRACKINGDETAILSInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public ATR_TRACKINGDETAILSInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public ATR_TRACKINGDETAILSInputHandler(
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
      try
      {
        if (this.inputData != null && this.inputData is DataTemplate && ((DataTemplate) this.inputData).UsedForGeneralDataInput)
        {
          this.usedForGeneralDataInput = true;
          this.dataIsReadOnly = ((DataTemplate) this.inputData).ReadOnly;
          this.isManualEntry = ((FormDataBase) this.inputData).GetField("AUS.X999") == "Manual";
          this.isLQAEntry = ((FormDataBase) this.inputData).GetField("AUS.X1") == "LQA";
          this.isEarlycheckEntry = ((FormDataBase) this.inputData).GetField("AUS.X1").Equals("EarlyCheck", StringComparison.OrdinalIgnoreCase);
        }
        if (!this.isManualEntry || this.isLQAEntry || this.isEarlycheckEntry)
          return;
        if (this.fieldAUSX1 == null)
          this.fieldAUSX1 = (DropdownBox) this.currentForm.FindControl("DropdownBox1");
        if (this.fieldAUSX1 == null || this.fieldAUSX1.Options == null || this.fieldAUSX1.Options.Count <= 0)
          return;
        if (this.fieldAUSX1.Options.Contains(new DropdownOption("LQA", "LQA")))
          this.fieldAUSX1.Options.Remove(new DropdownOption("LQA", "LQA"));
        if (this.fieldAUSX1.Options.Contains(new DropdownOption("EarlyCheck", "EarlyCheck")))
          this.fieldAUSX1.Options.Remove(new DropdownOption("EarlyCheck", "EarlyCheck"));
        if (!this.fieldAUSX1.Options.Contains(new DropdownOption("", "")))
          return;
        this.fieldAUSX1.Options.Remove(new DropdownOption("", ""));
      }
      catch (Exception ex)
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "AUS.X1":
        case "AUS.X10":
        case "AUS.X173":
        case "AUS.X174":
        case "AUS.X3":
        case "AUS.X4":
        case "AUS.X5":
        case "AUS.X6":
        case "AUS.X7":
        case "AUS.X8":
        case "AUS.X9":
          if (!this.isManualEntry || this.dataIsReadOnly)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "AUS.X2":
          if (this.inputData.GetField("AUS.X1") != "Other" || this.dataIsReadOnly || !this.isManualEntry)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "aussnapshot":
          if (this.isManualEntry || this.inputData.GetField("AUS.EFOLDERGUID") == string.Empty)
          {
            controlState = ControlState.Disabled;
            if (ctrl.Visible)
            {
              ctrl.Visible = false;
              break;
            }
            break;
          }
          controlState = ControlState.Default;
          break;
        default:
          controlState = this.dataIsReadOnly || this.isManualEntry ? ControlState.Disabled : ControlState.Default;
          break;
      }
      return controlState;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (this.usedForGeneralDataInput)
      {
        if ((id == "AUS.X173" || id == "AUS.X174") && val != string.Empty)
        {
          string str = val;
          val = Utils.ParseTime(val, false);
          if (val == null)
          {
            int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The string '" + str + "' is not a valid time format (hh:mm am/pm).", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        base.UpdateFieldValue(id, val);
        if (!(id == "AUS.X1"))
          return;
        if (val != "Other")
          base.UpdateFieldValue("AUS.X2", "");
        if (!this.isManualEntry || this.session.LoanData == null)
          return;
        base.UpdateFieldValue("AUS.X14", val == "LP" ? this.session.LoanData.GetField("CASASRN.X201") : this.session.LoanData.GetField("740"));
        base.UpdateFieldValue("AUS.X15", val == "LP" ? this.session.LoanData.GetField("CASASRN.X202") : this.session.LoanData.GetField("742"));
      }
      else
        this.inputData.SetCurrentField(id, val);
    }

    public override void ExecAction(string action)
    {
      if (!(action == "aussnapshot"))
        return;
      base.ExecAction(action);
    }
  }
}

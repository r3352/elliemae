// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.STATE_SPECIFIC_GenericInputHandler
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
  internal class STATE_SPECIFIC_GenericInputHandler : InputHandlerBase
  {
    private Calendar f761Control;
    private Calendar f762Control;

    public STATE_SPECIFIC_GenericInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public STATE_SPECIFIC_GenericInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public STATE_SPECIFIC_GenericInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public STATE_SPECIFIC_GenericInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
      try
      {
        this.CurrentFormName = (string) htmldoc.body.getAttribute("id");
      }
      catch (Exception ex)
      {
      }
    }

    internal override void CreateControls()
    {
      try
      {
        if (this.inputData.GetField("14") == "CA")
          this.f762Control = (Calendar) this.currentForm.FindControl("Calendar5");
        else if (this.inputData.GetField("14") == "MO")
        {
          this.f761Control = (Calendar) this.currentForm.FindControl("Calendar1");
          this.f762Control = (Calendar) this.currentForm.FindControl("Calendar2");
        }
        else if (this.inputData.GetField("14") == "MO" || this.inputData.GetField("14") == "WA")
          this.f761Control = (Calendar) this.currentForm.FindControl("Calendar1");
        else if (this.inputData.GetField("14") == "DC")
          this.f762Control = (Calendar) this.currentForm.FindControl("Calendar3");
        else if (this.inputData.GetField("14") == "MN" || this.inputData.GetField("14") == "NY")
        {
          this.f762Control = (Calendar) this.currentForm.FindControl("Calendar2");
        }
        else
        {
          if (!(this.inputData.GetField("14") == "TX") && !(this.inputData.GetField("14") == "VA"))
            return;
          this.f762Control = (Calendar) this.currentForm.FindControl("Calendar1");
        }
      }
      catch (Exception ex)
      {
      }
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      if (this.inputData is LoanData)
        base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      foreach (EllieMae.Encompass.Forms.Control allControl in this.currentForm.GetAllControls())
      {
        if (allControl is FieldControl)
        {
          FieldControl fieldControl = (FieldControl) allControl;
          if (fieldControl.Field.FieldID == "DISCLOSURE.X1130")
          {
            string str = fieldControl.Value;
            fieldControl.BindTo(str.Replace(",", ""));
          }
        }
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "11":
          return controlState;
        case "432":
          if (this.GetField("3941") == "Y")
            return ControlState.Disabled;
          goto case "11";
        case "761":
          if (this.GetField("3941") == "Y")
          {
            if (this.f761Control != null)
              this.f761Control.Enabled = false;
            return ControlState.Disabled;
          }
          if (this.f761Control != null)
          {
            this.f761Control.Enabled = true;
            goto case "11";
          }
          else
            goto case "11";
        case "762":
          if (this.GetField("3941") == "Y")
          {
            if (this.f762Control != null)
              this.f762Control.Enabled = false;
            return ControlState.Disabled;
          }
          if (this.f762Control != null)
          {
            this.f762Control.Enabled = true;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X1080":
          if (this.GetFieldValue("DISCLOSURE.X1076") != "Y" || this.GetFieldValue("DISCLOSURE.X1077") != "Y" || this.GetFieldValue("DISCLOSURE.X1078") != "Y" || this.GetFieldValue("DISCLOSURE.X1079") != "Y")
            controlState = ControlState.Disabled;
          if (this.GetFieldValue("420") != "FirstLien")
            controlState = ControlState.Disabled;
          if (this.GetFieldValue("19") != "NoCash-Out Refinance")
            controlState = ControlState.Disabled;
          if (this.GetFieldValue("16") != "")
          {
            if (Convert.ToInt32(this.GetFieldValue("16")) > 4 || Convert.ToInt32(this.GetFieldValue("16")) < 1)
            {
              controlState = ControlState.Disabled;
              goto case "11";
            }
            else
              goto case "11";
          }
          else
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
        case "DISCLOSURE.X110":
          if (this.FormIsForTemplate)
          {
            foreach (FieldControl allFieldControl in this.currentForm.GetAllFieldControls())
            {
              if (allFieldControl.Field.FieldID == "DISCLOSURE.X109" && this.GetFieldValue("DISCLOSURE.X109") != "Y")
                return ControlState.Disabled;
            }
            return controlState;
          }
          if (this.GetFieldValue("DISCLOSURE.X109") != "Y" && this.GetFieldValue("14") != "VT")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X111":
          if (this.GetFieldValue("DISCLOSURE.X109") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X112":
        case "DISCLOSURE.X569":
          if ((this.GetFieldValue("14") == "NY" || this.CurrentFormName == "STATE_SPECIFIC_NY") && this.GetFieldValue("DISCLOSURE.X109") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X1149":
          if (this.GetFieldValue("DISCLOSURE.X1147") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X1150":
          if (this.GetFieldValue("DISCLOSURE.X1148") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X1153":
          if (this.GetFieldValue("1964") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X120":
          if (this.verifyFieldState("DISCLOSURE.X570", "Y") == ControlState.Disabled)
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X121":
          if (this.GetFieldValue("DISCLOSURE.X570") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X1210":
          if ((this.GetFieldValue("14") == "CA" || this.CurrentFormName == "STATE_SPECIFIC_CA") && this.GetFieldValue("DISCLOSURE.X1209") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X224":
          if (this.GetFieldValue("DISCLOSURE.X208") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X238":
          if (this.GetFieldValue("DISCLOSURE.X619") != "Alternative2")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X288":
          if (this.GetFieldValue("DISCLOSURE.X284") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X289":
          if (this.GetFieldValue("DISCLOSURE.X287") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X302":
          if (this.GetFieldValue("DISCLOSURE.X205") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X365":
          if (this.FormIsForTemplate)
          {
            FieldControl[] allFieldControls = this.currentForm.GetAllFieldControls();
            for (int index = 0; index < allFieldControls.Length; ++index)
            {
              if (allFieldControls[index].Field.FieldID == "DISCLOSURE.X480" && this.GetFieldValue("DISCLOSURE.X480") != "Y" || allFieldControls[index].Field.FieldID == "DISCLOSURE.X403" && this.GetFieldValue("DISCLOSURE.X403") != "Prior to Closing")
                return ControlState.Disabled;
            }
            return controlState;
          }
          if ((this.GetFieldValue("14") == "DC" || this.CurrentFormName == "STATE_SPECIFIC_DC") && this.GetFieldValue("DISCLOSURE.X403") != "Prior to Closing")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else if ((this.GetFieldValue("14") == "FL" || this.CurrentFormName == "STATE_SPECIFIC_FL" || this.GetFieldValue("14") == "CO" || this.CurrentFormName == "STATE_SPECIFIC_CO") && this.GetFieldValue("DISCLOSURE.X480") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else if ((this.GetFieldValue("14") == "CA" || this.CurrentFormName == "STATE_SPECIFIC_CA" || this.GetFieldValue("14") == "CO" || this.CurrentFormName == "STATE_SPECIFIC_CO") && this.GetFieldValue("DISCLOSURE.X480") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else if (this.loan != null && this.FormIsForTemplate && this.GetFieldValue("DISCLOSURE.X480") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X375":
        case "DISCLOSURE.X376":
        case "DISCLOSURE.X377":
        case "DISCLOSURE.X378":
        case "DISCLOSURE.X379":
          if (this.GetFieldValue("DISCLOSURE.X374") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X394":
          if (this.GetFieldValue("DISCLOSURE.X393") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X406":
          if (this.GetFieldValue("DISCLOSURE.X405") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X408":
          if (this.GetFieldValue("DISCLOSURE.X407") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X428":
          if (this.GetFieldValue("DISCLOSURE.X427") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X433":
          if (this.GetFieldValue("DISCLOSURE.X432") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X447":
        case "DISCLOSURE.X661":
        case "DISCLOSURE.X684":
          if (this.GetFieldValue("DISCLOSURE.X446") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X449":
        case "DISCLOSURE.X662":
          if (this.GetFieldValue("DISCLOSURE.X448") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X451":
        case "DISCLOSURE.X663":
          if (this.GetFieldValue("DISCLOSURE.X450") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X453":
        case "DISCLOSURE.X664":
          if (this.GetFieldValue("DISCLOSURE.X452") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X455":
        case "DISCLOSURE.X665":
        case "DISCLOSURE.X666":
          if (this.GetFieldValue("DISCLOSURE.X454") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X457":
        case "DISCLOSURE.X667":
        case "DISCLOSURE.X668":
          if (this.GetFieldValue("DISCLOSURE.X456") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X459":
        case "DISCLOSURE.X669":
          if (this.GetFieldValue("DISCLOSURE.X458") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X479":
          if (this.GetFieldValue("DISCLOSURE.X478") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X484":
          if (this.GetFieldValue("DISCLOSURE.X483") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X492":
          if (this.GetFieldValue("DISCLOSURE.X491") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X527":
          if ((this.GetFieldValue("14") == "NJ" || this.CurrentFormName == "STATE_SPECIFIC_NJ") && this.GetFieldValue("DISCLOSURE.X526") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X529":
        case "DISCLOSURE.X530":
        case "DISCLOSURE.X532":
        case "DISCLOSURE.X533":
        case "DISCLOSURE.X535":
        case "DISCLOSURE.X536":
          if (this.verifyFieldState("DISCLOSURE.X374", "Y") == ControlState.Disabled || this.verifyFieldState("DISCLOSURE.X528", "Y") == ControlState.Disabled)
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X531":
        case "DISCLOSURE.X534":
        case "DISCLOSURE.X537":
        case "DISCLOSURE.X538":
        case "DISCLOSURE.X539":
        case "DISCLOSURE.X540":
          if ((this.GetFieldValue("14") == "NJ" || this.CurrentFormName == "STATE_SPECIFIC_NJ") && this.GetFieldValue("DISCLOSURE.X528") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X565":
          if ((this.GetFieldValue("14") == "NY" || this.CurrentFormName == "STATE_SPECIFIC_NY") && this.GetFieldValue("DISCLOSURE.X564") != "Prior to scheduled loan closing")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X571":
          if ((this.GetFieldValue("14") == "NY" || this.CurrentFormName == "STATE_SPECIFIC_NY") && this.GetFieldValue("DISCLOSURE.X570") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X573":
        case "DISCLOSURE.X574":
        case "DISCLOSURE.X575":
        case "DISCLOSURE.X576":
          if ((this.GetFieldValue("14") == "NY" || this.CurrentFormName == "STATE_SPECIFIC_NY") && this.GetFieldValue("DISCLOSURE.X572") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X577":
          if ((this.GetFieldValue("14") == "NY" || this.CurrentFormName == "STATE_SPECIFIC_NY") && this.GetFieldValue("DISCLOSURE.X575") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X599":
          if ((this.GetFieldValue("14") == "CA" || this.CurrentFormName == "STATE_SPECIFIC_CA") && this.GetFieldValue("DISCLOSURE.X222") != "LenderAgree")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X600":
          if ((this.GetFieldValue("14") == "CA" || this.CurrentFormName == "STATE_SPECIFIC_CA") && this.GetFieldValue("DISCLOSURE.X599") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X647":
          if (this.GetFieldValue("DISCLOSURE.X645") != "Y" || this.loan != null && !this.loan.IsLocked("DISCLOSURE.X647"))
            controlState = ControlState.Disabled;
          if (this.GetFieldValue("DISCLOSURE.X645") != "Y")
          {
            this.SetControlState("lock_DISCLOSUREX647", false);
            goto case "11";
          }
          else
          {
            this.SetControlState("lock_DISCLOSUREX647", true);
            goto case "11";
          }
        case "DISCLOSURE.X648":
          if (this.GetFieldValue("DISCLOSURE.X646") != "Y" || this.loan != null && !this.loan.IsLocked("DISCLOSURE.X648"))
            controlState = ControlState.Disabled;
          if (this.GetFieldValue("DISCLOSURE.X646") != "Y")
          {
            this.SetControlState("lock_DISCLOSUREX648", false);
            goto case "11";
          }
          else
          {
            this.SetControlState("lock_DISCLOSUREX648", true);
            goto case "11";
          }
        case "DISCLOSURE.X651":
          if (this.GetFieldValue("DISCLOSURE.X649") != "Y" || this.loan != null && !this.loan.IsLocked("DISCLOSURE.X651"))
            controlState = ControlState.Disabled;
          if (this.GetFieldValue("DISCLOSURE.X649") != "Y")
          {
            this.SetControlState("lock_DISCLOSUREX651", false);
            goto case "11";
          }
          else
          {
            this.SetControlState("lock_DISCLOSUREX651", true);
            goto case "11";
          }
        case "DISCLOSURE.X652":
          if (this.GetFieldValue("DISCLOSURE.X650") != "Y" || this.loan != null && !this.loan.IsLocked("DISCLOSURE.X652"))
            controlState = ControlState.Disabled;
          if (this.GetFieldValue("DISCLOSURE.X650") != "Y")
          {
            this.SetControlState("lock_DISCLOSUREX652", false);
            goto case "11";
          }
          else
          {
            this.SetControlState("lock_DISCLOSUREX652", true);
            goto case "11";
          }
        case "DISCLOSURE.X670":
          if (this.GetFieldValue("DISCLOSURE.X458") != "Y" || this.loan != null && !this.loan.IsLocked("DISCLOSURE.X670"))
          {
            controlState = ControlState.Disabled;
            if (this.loan != null && this.loan.IsLocked("DISCLOSURE.X670"))
              this.loan.RemoveLock("DISCLOSURE.X670");
          }
          if (this.GetFieldValue("DISCLOSURE.X458") != "Y")
          {
            this.SetControlState("lock_DISCLOSUREX670", false);
            goto case "11";
          }
          else
          {
            this.SetControlState("lock_DISCLOSUREX670", true);
            goto case "11";
          }
        case "DISCLOSURE.X77":
        case "DISCLOSURE.X78":
          if ((this.GetFieldValue("14") == "TX" || this.CurrentFormName == "STATE_SPECIFIC_TX") && this.GetFieldValue("DISCLOSURE.X76") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X887":
          if (this.verifyFieldState("DISCLOSURE.X338", "Y") == ControlState.Disabled)
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X893":
        case "DISCLOSURE.X894":
        case "DISCLOSURE.X895":
          if (this.verifyFieldState("DISCLOSURE.X892", "Y") == ControlState.Disabled)
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X897":
          if (this.verifyFieldState("DISCLOSURE.X896", "Y") == ControlState.Disabled)
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X909":
          if (this.verifyFieldState("DISCLOSURE.X908", "Y") == ControlState.Disabled)
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X913":
          if (this.loan != null && this.loan.Use2015RESPA)
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X914":
          if (this.GetFieldValue("DISCLOSURE.X913") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X925":
        case "DISCLOSURE.X926":
        case "DISCLOSURE.X927":
          if (this.GetFieldValue("DISCLOSURE.X924") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X928":
          if (this.GetFieldValue("DISCLOSURE.X927") != "Decreases Interest Rate")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X929":
          if (this.GetFieldValue("DISCLOSURE.X927") != "Increases Interest Rate")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X931":
        case "DISCLOSURE.X932":
        case "DISCLOSURE.X933":
          if (this.GetFieldValue("DISCLOSURE.X930") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X934":
          if (this.GetFieldValue("DISCLOSURE.X933") != "Decreases Interest Rate")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X935":
          if (this.GetFieldValue("DISCLOSURE.X933") != "Increases Interest Rate")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X937":
        case "DISCLOSURE.X938":
          if (this.GetFieldValue("DISCLOSURE.X936") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X940":
        case "DISCLOSURE.X941":
        case "DISCLOSURE.X942":
          if (this.GetFieldValue("DISCLOSURE.X939") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "DISCLOSURE.X958":
          if (this.GetFieldValue("DISCLOSURE.X957") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "RE88395.X315":
          if ((this.GetFieldValue("14") == "NY" || this.CurrentFormName == "STATE_SPECIFIC_NY") && this.GetFieldValue("RE88395.X322") != "Y")
          {
            this.SetControlState("FieldLock_X315", false);
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
          {
            this.SetControlState("FieldLock_X315", true);
            controlState = ControlState.Default;
            goto case "11";
          }
        case "RE88395.X316":
          if ((this.GetFieldValue("14") == "NY" || this.CurrentFormName == "STATE_SPECIFIC_NY") && this.GetFieldValue("RE88395.X322") != "Y")
          {
            this.SetControlState("FieldLock_X316", false);
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
          {
            this.SetControlState("FieldLock_X316", true);
            controlState = ControlState.Default;
            goto case "11";
          }
        case "prepayment":
          if ((this.GetFieldValue("14") == "NY" || this.CurrentFormName == "STATE_SPECIFIC_NY") && this.GetFieldValue("RE88395.X322") != "Y")
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

    private ControlState verifyFieldState(string id, string valueToEnable)
    {
      foreach (FieldControl allFieldControl in this.currentForm.GetAllFieldControls())
      {
        if (allFieldControl.Field.FieldID == id && this.GetFieldValue(id) != valueToEnable)
          return ControlState.Disabled;
      }
      return ControlState.Enabled;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id == "DISCLOSURE.X96" && val == "N")
        val = string.Empty;
      base.UpdateFieldValue(id, val);
      if (id == "DISCLOSURE.X110" && this.CurrentFormName == "STATE_SPECIFIC_VT")
        base.UpdateFieldValue("DISCLOSURE.X109", val != string.Empty ? "Y" : "");
      if (id == "DISCLOSURE.X374" && val != "Y")
      {
        for (int index = 375; index <= 379; ++index)
          base.UpdateFieldValue("DISCLOSURE.X" + (object) index, "");
      }
      else if (id == "DISCLOSURE.X393" && val != "Y")
        base.UpdateFieldValue("DISCLOSURE.X394", "");
      else if (id == "DISCLOSURE.X205" && val != "Other")
        base.UpdateFieldValue("DISCLOSURE.X302", "");
      else if (id == "DISCLOSURE.X427" && val != "Y")
        base.UpdateFieldValue("DISCLOSURE.X428", "");
      else if (id == "DISCLOSURE.X432" && val != "Y")
        base.UpdateFieldValue("DISCLOSURE.X433", "");
      else if (id == "DISCLOSURE.X908" && val != "Y")
      {
        base.UpdateFieldValue("DISCLOSURE.X909", "");
      }
      else
      {
        if (!(id == "DISCLOSURE.X957") || !(val != "Other"))
          return;
        base.UpdateFieldValue("DISCLOSURE.X958", "");
      }
    }

    public override void ExecAction(string action)
    {
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
      {
        case 755860596:
          if (!(action == "prepayment"))
            break;
          base.ExecAction(action);
          this.SetFieldFocus("l_RE88395X322");
          break;
        case 888963497:
          if (!(action == "contactcre"))
            break;
          this.GetContactItem("DISCLOSURE.X86");
          this.SetFieldFocus("l_DISCLOSURE_X86");
          break;
        case 1797042560:
          if (!(action == "contactund"))
            break;
          this.GetContactItem("DISCLOSURE.X88");
          this.SetFieldFocus("l_DISCLOSURE_X88");
          break;
        case 3582764703:
          if (!(action == "loanprog"))
            break;
          base.ExecAction(action);
          this.SetFieldFocus("l_1401");
          break;
        case 3664405482:
          if (!(action == "loanprog1"))
            break;
          base.ExecAction(action);
          this.SetFieldFocus("l_1401a");
          break;
        case 3878910169:
          if (!(action == "copyfromitemization"))
            break;
          base.ExecAction(action);
          this.SetFieldFocus("l_DISCLOSUREX1175");
          break;
        case 4078909464:
          if (!(action == "copygfe"))
            break;
          this.UpdateFieldValue("DISCLOSURE.X82", this.loan.GetSimpleField("L228"));
          this.UpdateFieldValue("DISCLOSURE.X84", this.loan.GetSimpleField("617"));
          this.UpdateFieldValue("DISCLOSURE.X85", this.loan.GetSimpleField("641"));
          this.UpdateFieldValue("DISCLOSURE.X86", this.loan.GetSimpleField("624"));
          this.UpdateFieldValue("DISCLOSURE.X87", this.loan.GetSimpleField("640"));
          this.UpdateContents();
          this.SetFieldFocus("l_DISCLOSURE_X82");
          break;
        case 4180047638:
          if (!(action == "contactapp"))
            break;
          this.GetContactItem("DISCLOSURE.X84");
          this.SetFieldFocus("l_DISCLOSURE_X84");
          break;
      }
    }
  }
}

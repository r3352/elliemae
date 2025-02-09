// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PURCHASEADVICEInputHandler
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
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class PURCHASEADVICEInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Label labelDiff;
    private bool isNewPurchaseScreen = true;

    public PURCHASEADVICEInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public PURCHASEADVICEInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public PURCHASEADVICEInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public PURCHASEADVICEInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (val != "")
      {
        switch (id)
        {
          case "3422":
          case "3424":
          case "3426":
          case "3428":
            if (this.loan.GetField("2211") == "")
            {
              int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "Please enter the loan principal amount paid by the investor (Field 2211) first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            break;
          case "4190":
            for (int index = 0; index < val.Length; ++index)
            {
              if (!char.IsNumber(val, index))
              {
                int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "Wire Confirmation # must be numeric.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
            }
            break;
          case "3567":
            if (this.session.SessionObjects.GetPolicySetting("EnablePaymentHistoryAndCalc") && this.loan.GetField("CPA.PaymentHistory.AnticipatedPurchaseDate") != "//" && !string.IsNullOrEmpty(this.loan.GetField("CPA.PaymentHistory.AnticipatedPurchaseDate")) && this.loan.GetField("CPA.PaymentHistory.AnticipatedPurchaseDate") != val && Utils.Dialog((IWin32Window) this.mainScreen, "The Purchase Date does not match the Anticipated Purchase Date in the Payment History tab, calculated amounts may be incorrect. Do you wish to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
              val = "";
              break;
            }
            break;
        }
      }
      base.UpdateFieldValue(id, val);
      if (id == "2396" && val != string.Empty)
        base.UpdateFieldValue("2630", DateTime.Today.ToString("MM/dd/yyyy"));
      else if (id == "3963" && val == "Y")
        this.session.LoanData.Calculator.CalcDefaultTpoBankContact();
      if (!(id == "3571") || !(Utils.ParseDecimal((object) this.inputData.GetField("3571")) <= 0M))
        return;
      int num1 = (int) Utils.Dialog((IWin32Window) this.mainScreen, "Current Principal cannot be zero or a negative amount. Please correct the amount before proceeding.");
      this.inputData.SetField("3571", "");
      this.SetGoToFieldFocus("3571", 1);
    }

    internal override void CreateControls()
    {
      try
      {
        EllieMae.Encompass.Forms.Button control = (EllieMae.Encompass.Forms.Button) this.currentForm.FindControl("btnSelectTemplate");
        if (control != null)
          this.isNewPurchaseScreen = false;
        FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
        if (this.currentForm.Name.Equals("Purchase Advice Form"))
          control.Enabled = true;
      }
      catch (Exception ex)
      {
        this.isNewPurchaseScreen = true;
      }
      string[] strArray1 = new string[0];
      string[] strArray2 = new string[0];
      if (this.isNewPurchaseScreen)
      {
        ArrayList secondaryFields = this.session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.BasePrice);
        if (secondaryFields != null)
          strArray2 = (string[]) secondaryFields.ToArray(typeof (string));
      }
      ArrayList secondaryFields1 = this.session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.Payouts);
      if (secondaryFields1 != null)
        strArray1 = (string[]) secondaryFields1.ToArray(typeof (string));
      foreach (PickList pickList in this.currentForm.FindControlsByType(typeof (PickList)))
      {
        if (pickList.ControlID.StartsWith("PickListPayout"))
        {
          pickList.Options.Clear();
          if (pickList.ControlID == "PickListPayout13" || pickList.ControlID == "PickListPayout14" || pickList.ControlID == "PickListPayout15")
          {
            for (int index = 0; index < strArray2.Length; ++index)
              pickList.Options.Add(strArray2[index]);
          }
          else
          {
            for (int index = 0; index < strArray1.Length; ++index)
              pickList.Options.Add(strArray1[index]);
          }
          pickList.ItemSelected += new ItemSelectedEventHandler(this.onPayoutListItemSelected);
        }
      }
    }

    private void onPayoutListItemSelected(object sender, ItemSelectedEventArgs e)
    {
      EllieMae.Encompass.Forms.TextBox control = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl(((EllieMae.Encompass.Forms.Control) sender).ControlID.Replace("PickListPayout", "TextBoxPayout"));
      control.Text = e.SelectedItem.Value;
      control.Focus();
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      this.getControlState(ctrl, id, ControlState.Enabled);
      ControlState controlState;
      switch (id)
      {
        case "2629":
          if (this.labelDiff == null)
            this.labelDiff = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelDiff");
          if (this.loan == null || this.GetFieldValue("2629") == "")
            this.labelDiff.ForeColor = Color.FromArgb(0, 51, 102);
          else if (this.loan != null && this.GetFieldValue("2629") != "")
            this.labelDiff.ForeColor = AppColors.AlertRed;
          controlState = ControlState.Default;
          break;
        case "3422":
        case "3426":
          controlState = !(this.loan.GetField("3430") != "Y") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "3424":
        case "3428":
          controlState = !(this.loan.GetField("3430") == "Y") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "3958":
        case "3959":
        case "3960":
        case "3961":
          controlState = !(this.loan.GetField("3963") == "Y") ? ControlState.Enabled : ControlState.Disabled;
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
      switch (action)
      {
        case "selecttemplate":
        case "updatebalance":
          this.SetFieldFocus("l_2836");
          break;
        case "updatecorrespondentbalance":
          if (this.isNewPurchaseScreen)
            this.SetFieldFocus("l_3585");
          else
            this.SetFieldFocus("l_2836");
          if (!this.session.SessionObjects.GetPolicySetting("EnablePaymentHistoryAndCalc") || !(this.loan.GetField("CPA.PaymentHistory.AnticipatedPurchaseDate") == "//") && !string.IsNullOrEmpty(this.loan.GetField("CPA.PaymentHistory.AnticipatedPurchaseDate")))
            break;
          this.loan.SetField("3580", "");
          this.loan.SetField("3581", "");
          int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "The Payment History tab of this Correspondent Purchase Advice should be completed before the Per Diem Interest can be calculated.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
        case "editPurAdvIntCal":
          if (this.isNewPurchaseScreen)
          {
            this.SetFieldFocus("l_3585");
            break;
          }
          this.SetFieldFocus("l_2836");
          break;
      }
    }
  }
}

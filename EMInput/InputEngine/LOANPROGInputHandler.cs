// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LOANPROGInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LOANPROGInputHandler : InputHandlerBase
  {
    private const string className = "LOANPROGInputHandler";
    private IWin32Window owner;
    private LoanProgramFieldSettings fieldSettings;
    private LoanProgram loanProgram;
    private Plan currentPlan;
    private DropdownBox closingTypeDropdown;
    private static Dictionary<string, int> helocInitialfields = new Dictionary<string, int>()
    {
      {
        "4476",
        1
      },
      {
        "4477",
        1
      },
      {
        "4478",
        1
      },
      {
        "4479",
        1
      },
      {
        "4480",
        2
      },
      {
        "4481",
        2
      },
      {
        "4482",
        3
      }
    };
    private static Dictionary<string, int> helocQualfields = new Dictionary<string, int>()
    {
      {
        "4465",
        1
      },
      {
        "4466",
        1
      },
      {
        "4467",
        1
      },
      {
        "4468",
        1
      },
      {
        "4469",
        2
      },
      {
        "4470",
        2
      },
      {
        "4471",
        3
      }
    };

    public LOANPROGInputHandler(
      IWin32Window owner,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, input, htmldoc, form, property)
    {
    }

    public LOANPROGInputHandler(
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
      this.loanProgram = (LoanProgram) this.inputData;
      this.fieldSettings = this.session.ConfigurationManager.GetLoanProgramFieldSettings();
      this.loanProgram.ApplyFieldSettings(this.fieldSettings);
      this.refreshPlanCodeData();
      if (this.closingTypeDropdown == null)
        this.closingTypeDropdown = (DropdownBox) this.currentForm.FindControl("DropdownBox11");
      if (!string.Equals(this.session.ServerManager.GetServerSetting("eClose.AllowHybridWithENoteClosing").ToString(), "Disabled", StringComparison.InvariantCultureIgnoreCase) || string.Equals(this.GetField("4689"), "HybridWithENote", StringComparison.InvariantCultureIgnoreCase))
        return;
      this.closingTypeDropdown.Options.Remove(new DropdownOption("Hybrid With eNote", "HybridWithENote"));
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string fieldIdOrAction)
    {
      if (this.currentPlan != null && this.currentPlan.ContainsField(fieldIdOrAction))
        return ControlState.Disabled;
      switch (fieldIdOrAction)
      {
        case "Terms.USDAGovtType":
          return this.currentPlan != null && this.currentPlan.ContainsField("1172") || this.GetField("1172") != "FarmersHomeAdministration" ? ControlState.Disabled : ControlState.Enabled;
        case "1063":
          return this.GetField("1172") == "Other" ? ControlState.Enabled : ControlState.Disabled;
        case "zoomarm":
          return this.currentPlan == null || !this.currentPlan.ContainsField("995") ? ControlState.Enabled : ControlState.Disabled;
        case "4665":
          return this.GetField("1172") == "HELOC" ? ControlState.Enabled : ControlState.Disabled;
        default:
          ControlState sharedFieldStatus = LOANPROGInputHandler.GetSharedFieldStatus((InputHandlerBase) this, fieldIdOrAction);
          return sharedFieldStatus != ControlState.Default ? sharedFieldStatus : base.GetControlState(ctrl, fieldIdOrAction);
      }
    }

    internal static ControlState GetSharedFieldStatus(
      InputHandlerBase inputBase,
      string fieldIdOrAction)
    {
      switch (fieldIdOrAction)
      {
        case "1889":
        case "1890":
          switch (inputBase)
          {
            case LOANPROGInputHandler _:
            case LPDETAILSInputHandler _:
              return !(inputBase.GetField("1172") != "HELOC") ? ControlState.Enabled : ControlState.Disabled;
            default:
              return ControlState.Default;
          }
        case "2":
          return inputBase.GetField("1172") != "HELOC" ? ControlState.Disabled : ControlState.Enabled;
        case "4465":
        case "4466":
        case "4467":
        case "4468":
          return inputBase.GetField("4464") == "Rate" ? ControlState.Enabled : ControlState.Disabled;
        case "4469":
        case "4470":
          return inputBase.GetField("4531") == "Fraction of Balance" ? ControlState.Enabled : ControlState.Disabled;
        case "4471":
          return inputBase.GetField("4531") == "Percentage of Balance" ? ControlState.Enabled : ControlState.Disabled;
        case "4474":
          return inputBase.GetField("4464") == "Rate" && inputBase.GetField("4468") == "Y" || inputBase.GetField("1172") != "HELOC" ? ControlState.Disabled : ControlState.Enabled;
        case "4476":
        case "4477":
        case "4478":
        case "4479":
          return inputBase.GetField("4475") == "Rate" ? ControlState.Enabled : ControlState.Disabled;
        case "4480":
        case "4481":
          return inputBase.GetField("4530") == "Fraction of Balance" ? ControlState.Enabled : ControlState.Disabled;
        case "4482":
          return inputBase.GetField("4530") == "Percentage of Balance" ? ControlState.Enabled : ControlState.Disabled;
        case "4485":
          return inputBase.GetField("4475") == "Rate" && inputBase.GetField("4479") == "Y" ? ControlState.Enabled : ControlState.Disabled;
        case "4491":
          return !(inputBase.GetField("4464") == "Rate") || !(inputBase.GetField("4468") == "Y") ? ControlState.Disabled : ControlState.Enabled;
        case "4556":
          return inputBase.GetField("HUD24") == "" ? ControlState.Enabled : ControlState.Disabled;
        case "4564":
        case "4565":
          return inputBase.GetField("4560") == "Fraction of Balance" ? ControlState.Enabled : ControlState.Disabled;
        case "4569":
          switch (inputBase)
          {
            case LOANPROGInputHandler _:
            case LPDETAILSInputHandler _:
              return inputBase.GetField("4573") == "Y" ? ControlState.Enabled : ControlState.Disabled;
            default:
              return ControlState.Default;
          }
        case "4573":
          return inputBase is LOANPROGInputHandler || inputBase is LPDETAILSInputHandler || inputBase.GetField("4568") == "Y" ? ControlState.Enabled : ControlState.Disabled;
        case "4574":
        case "4575":
          return inputBase.GetField("4569") == "Fraction of Balance" ? ControlState.Enabled : ControlState.Disabled;
        case "4577":
        case "4578":
        case "4579":
        case "4580":
        case "4581":
        case "4582":
        case "4583":
        case "4584":
        case "4585":
          return inputBase.GetField("4557") == "Y" ? ControlState.Enabled : ControlState.Disabled;
        case "4587":
          return inputBase.GetField("4586") == "Y" ? ControlState.Enabled : ControlState.Disabled;
        case "4592":
        case "4614":
        case "4615":
        case "4616":
        case "4618":
        case "4619":
          return inputBase.GetField("4557") == "Y" ? ControlState.Enabled : ControlState.Disabled;
        case "4603":
        case "4604":
          return inputBase.GetField("4600") == "Y" ? ControlState.Enabled : ControlState.Disabled;
        case "4605":
          return inputBase.GetField("4600") == "Y" && inputBase.GetField("4603") == "Y" ? ControlState.Enabled : ControlState.Disabled;
        case "4606":
          return inputBase.GetField("4600") == "Y" && inputBase.GetField("4604") == "Y" ? ControlState.Enabled : ControlState.Disabled;
        case "4613":
          return inputBase.GetField("4612") == "Y" ? ControlState.Enabled : ControlState.Disabled;
        case "4617":
          return inputBase.GetField("4557") == "Y" && inputBase.GetField("4616") == "Y" ? ControlState.Enabled : ControlState.Disabled;
        case "4622":
          return inputBase.GetField("4621") == "Y" ? ControlState.Enabled : ControlState.Disabled;
        case "4746":
          string field1 = inputBase.GetField("3");
          string field2 = inputBase.GetField("608");
          return !string.IsNullOrEmpty(field1) && Utils.ParseDouble((object) field1) == 0.0 && field2 != "Fixed" ? ControlState.Disabled : ControlState.Enabled;
        case "9":
          return !(inputBase.GetField("19") == "Other") ? ControlState.Disabled : ControlState.Enabled;
        case "994":
          return !(inputBase.GetField("608") == "OtherAmortizationType") ? ControlState.Disabled : ControlState.Enabled;
        case "HELOC.MinAdvPct":
          return inputBase.GetField("4560") == "Percentage of Balance" ? ControlState.Enabled : ControlState.Disabled;
        case "HELOC.MinRepmtPct":
          return inputBase.GetField("4569") == "Percentage of Balance" ? ControlState.Enabled : ControlState.Disabled;
        case "SYS.X2":
          return inputBase.GetField("4557") == "Y" || inputBase.GetField("1172") != "HELOC" ? ControlState.Enabled : ControlState.Disabled;
        default:
          return ControlState.Default;
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      LOANPROGInputHandler.UpdateSharedFieldValue((InputHandlerBase) this, this.inputData, id, val);
      if (!(id == "1172"))
        return;
      if (this.GetField("1172") == "HELOC" && this.GetField("4557") != "Y")
        this.inputData.SetCurrentField("SYS.X2", "");
      LOANPROGInputHandler.RefreshHELOCSections((InputHandlerBase) this, this.currentForm);
    }

    internal static void UpdateSharedFieldValue(
      InputHandlerBase inputBase,
      IHtmlInput inputData,
      string id,
      string val)
    {
      switch (id)
      {
        case "4464":
          switch (inputBase.GetField("4464"))
          {
            case "Rate":
              LOANPROGInputHandler.clearHelocFields(inputBase, 1, false);
              break;
            case "Fraction of Balance":
              LOANPROGInputHandler.clearHelocFields(inputBase, 2, false);
              break;
            case "Percentage of Balance":
              LOANPROGInputHandler.clearHelocFields(inputBase, 3, false);
              break;
            default:
              LOANPROGInputHandler.clearHelocFields(inputBase, 0, false);
              break;
          }
          break;
        case "4475":
          switch (inputBase.GetField("4475"))
          {
            case "Rate":
              LOANPROGInputHandler.clearHelocFields(inputBase, 1, true);
              break;
            case "Fraction of Balance":
              LOANPROGInputHandler.clearHelocFields(inputBase, 2, true);
              break;
            case "Percentage of Balance":
              LOANPROGInputHandler.clearHelocFields(inputBase, 3, true);
              break;
            default:
              LOANPROGInputHandler.clearHelocFields(inputBase, 0, true);
              break;
          }
          break;
        default:
          if (id == "4573" && val != "Y")
          {
            inputBase.SetField("4569", "");
            inputBase.SetField("4574", "");
            inputBase.SetField("4575", "");
            inputBase.SetField("HELOC.MinRepmtPct", "");
            break;
          }
          switch (id)
          {
            case "4560":
              if (val != "Fraction of Balance")
              {
                inputBase.SetField("4564", "");
                inputBase.SetField("4565", "");
              }
              if (val != "Percentage of Balance")
              {
                inputBase.SetField("HELOC.MinAdvPct", "");
                break;
              }
              break;
            case "4569":
              if (val != "Fraction of Balance")
              {
                inputBase.SetField("4574", "");
                inputBase.SetField("4575", "");
              }
              if (val != "Percentage of Balance")
              {
                inputBase.SetField("HELOC.MinRepmtPct", "");
                break;
              }
              break;
            default:
              if (id == "3" || id == "608")
              {
                string field1 = inputBase.GetField("3");
                string field2 = inputBase.GetField("608");
                if (!string.IsNullOrEmpty(field1) && Utils.ParseDouble((object) field1) == 0.0 && field2 != "Fixed")
                {
                  inputBase.SetField("4746", "AmortizingPayment");
                  break;
                }
                break;
              }
              break;
          }
          break;
      }
      switch (inputBase)
      {
        case LOANPROGInputHandler _:
        case LPDETAILSInputHandler _:
          if ((id == "4475" || id == "4479") && (inputBase.GetField("4475") != "Rate" || inputBase.GetField("4479") != "Y"))
          {
            inputBase.SetField("4485", "");
            break;
          }
          break;
      }
      SharedCalculations.Execute(inputData, SharedCalculations.SharedCalculationType.HELOC, id, val);
    }

    private static void clearHelocFields(
      InputHandlerBase inputBase,
      int category,
      bool initialPayment)
    {
      foreach (KeyValuePair<string, int> keyValuePair in initialPayment ? LOANPROGInputHandler.helocInitialfields : LOANPROGInputHandler.helocQualfields)
      {
        if (keyValuePair.Value != category)
          inputBase.UpdateFieldValue(keyValuePair.Key, "", false);
      }
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "mtgins":
          if (this.GetField("1172") == "FarmersHomeAdministration")
          {
            using (MIPUSDADialog mipusdaDialog = new MIPUSDADialog(this.inputData))
            {
              if (mipusdaDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
                break;
              this.UpdateContents();
              this.SetFieldFocus("l_1199");
              break;
            }
          }
          else
          {
            using (MIPDialog mipDialog = new MIPDialog(this.inputData, true, this.session))
            {
              if (mipDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
                break;
              this.UpdateContents();
              this.SetFieldFocus("l_1199");
              break;
            }
          }
        case "plancode":
          this.selectPlanCode();
          break;
        case "clearplancode":
          this.clearPlanCode();
          break;
        case "cleartable":
          this.inputData.SetCurrentField("1985", "");
          this.inputData.SetCurrentField("4630", "");
          this.RefreshAllControls(true);
          break;
        case "historicalindex":
          base.ExecAction(action);
          if (!(this.inputData.GetField("1985") != ""))
            break;
          this.inputData.SetCurrentField("1889", "");
          this.inputData.SetCurrentField("1890", "");
          break;
        default:
          base.ExecAction(action);
          break;
      }
    }

    private void clearPlanCode()
    {
      if (Utils.Dialog((IWin32Window) Session.Application, "Remove the link to the selected Plan Code?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.loanProgram.ClearPlan();
      this.currentPlan = (Plan) null;
      this.RefreshAllControls(true);
    }

    private void selectPlanCode()
    {
      ILoanServices service = Session.Application.GetService<ILoanServices>();
      if (!service.VerifyDocServiceSetup(DocumentOrderType.Both))
        return;
      PlanCodeSelectionDialog codeSelectionDialog = (PlanCodeSelectionDialog) null;
      using (CursorActivator.Wait())
        codeSelectionDialog = new PlanCodeSelectionDialog(PlanCodeSelectionDialog.SelectionMode.Template, service.GetAllowedDocumentOrderTypes());
      try
      {
        if (codeSelectionDialog.ShowDialog((IWin32Window) Session.Application) != DialogResult.OK)
          return;
        this.currentPlan = codeSelectionDialog.SelectedPlan;
        this.currentPlan.Apply(this.loanProgram);
        if (codeSelectionDialog.SelectedInvestor != null)
          codeSelectionDialog.SelectedInvestor.Apply((IHtmlInput) this.loanProgram, DocumentOrderType.Closing);
        this.loanProgram.DocumentOrderType = codeSelectionDialog.SelectedOrderType;
        this.loanProgram.CalculateAll();
        this.RefreshAllControls(true);
      }
      finally
      {
        codeSelectionDialog.Dispose();
      }
    }

    private void refreshPlanCodeData()
    {
      this.currentPlan = (Plan) null;
      try
      {
        this.currentPlan = Plan.Synchronize(this.session.SessionObjects, this.loanProgram);
        this.loanProgram.CalculateAll();
      }
      catch (InvalidPlanCodeException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.Application, "The Plan Code '" + this.loanProgram.PlanCode + "' is no longer valid. The plan code information will be removed from this Loan Program.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.loanProgram.ClearPlan();
      }
      catch (DocServiceLoginException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.Application, "Your Encompass Company Password is incorrect. You should resolve this issue before editing this Loan Program to ensure the Plan Code data is loaded correctly.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (Exception ex)
      {
        Tracing.Log(InputHandlerBase.sw, nameof (LOANPROGInputHandler), TraceLevel.Warning, "Exception while refresing plan code data: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) Session.Application, "Encompass was unable to synchronize the plan code data in this template. As a result, the data may be out-of-date. The reason reported for the failure was: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      this.RefreshAllControls(true);
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      LOANPROGInputHandler.RefreshHELOCSections((InputHandlerBase) this, this.currentForm);
    }

    public static void RefreshHELOCSections(InputHandlerBase inputBase, EllieMae.Encompass.Forms.Form inputForm)
    {
      bool isHeloc = inputBase.GetField("1172") == "HELOC";
      foreach (CategoryBox categoryBox in inputForm.FindControlsByType(typeof (CategoryBox)))
      {
        if (categoryBox.ControlID.StartsWith("grpHELOC_"))
          LOANPROGInputHandler.setFieldControlStatusRecursively(categoryBox.Controls, isHeloc, inputBase);
      }
    }

    private static void setFieldControlStatusRecursively(
      EllieMae.Encompass.Forms.ControlCollection cc,
      bool isHeloc,
      InputHandlerBase inputBase)
    {
      foreach (EllieMae.Encompass.Forms.Control control in cc)
      {
        switch (control)
        {
          case EllieMae.Encompass.Forms.TextBox _:
          case EllieMae.Encompass.Forms.CheckBox _:
          case DropdownBox _:
          case DropdownEditBox _:
            FieldControl fieldControl = (FieldControl) control;
            if (fieldControl.Field != null && !fieldControl.Field.ReadOnly && fieldControl.Field.FieldID != "4630")
            {
              if (!isHeloc)
                fieldControl.Enabled = false;
              else if (fieldControl.Field.FieldID == "4573" || LOANPROGInputHandler.GetSharedFieldStatus(inputBase, fieldControl.Field.FieldID) == ControlState.Default)
                fieldControl.Enabled = true;
            }
            if (!isHeloc && fieldControl.Field != null && fieldControl.Field.Description != "(Unknown)" && inputBase.GetField(fieldControl.Field.FieldID) != "" && fieldControl.Field.FieldID != "1962")
            {
              inputBase.UpdateFieldValue(fieldControl.Field.FieldID, "");
              continue;
            }
            continue;
          case EllieMae.Encompass.Forms.GroupBox _:
            LOANPROGInputHandler.setFieldControlStatusRecursively(((EllieMae.Encompass.Forms.ContainerControl) control).Controls, isHeloc, inputBase);
            continue;
          case CategoryBox _:
            LOANPROGInputHandler.setFieldControlStatusRecursively(((EllieMae.Encompass.Forms.ContainerControl) control).Controls, isHeloc, inputBase);
            continue;
          default:
            continue;
        }
      }
    }

    public override void onfocus(mshtml.IHTMLEventObj pEvtObj)
    {
      if (!(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement) || controlForElement.Field == null || controlForElement.Field.FieldID == "")
        return;
      Session.Application.GetService<IStatusDisplay>().DisplayFieldID(controlForElement.Field.FieldID);
    }
  }
}

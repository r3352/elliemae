// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SearchFields
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine.Forms;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.Verification;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class SearchFields
  {
    private const string className = "SearchFields";
    protected static string sw = Tracing.SwInputEngine;
    private InputFormList systemFormList;
    private LoanData loan;
    private string specialInstruction = string.Empty;
    private string fieldGoTo = string.Empty;
    private int countGoTo;
    private LoanDataMgr loanMgr;
    private LoanScreen freeScreen;
    private EMFormMenu emFormMenuBox;
    private EMFormMenu emToolMenuBox;
    private InputFormInfo currForm;
    private TabControl toolsFormsTabControl;
    private VORPanel vorPan;
    private VOEPanel voePan;
    private VOLPanel volPan;
    private VOMPanel vomPan;
    private TAX4506Panel tax4506Panel;
    private TAX4506TPanel tax4506TPanel;
    private CheckBox allFormBox;
    private ILoanEditor editor;
    private Sessions.Session session;

    public SearchFields(InputFormList systemFormList, LoanData loan)
    {
      this.systemFormList = systemFormList;
      this.loan = loan;
    }

    public InputFormInfo FindInputForm(string[] formNames, string fieldGoTo, string originalField)
    {
      InputFormInfo inputForm = (InputFormInfo) null;
      this.specialInstruction = string.Empty;
      foreach (string formName in formNames)
      {
        if (!(formName == "ULDD/PDD") && !(formName == "USDA Management") && !(formName == "VA Management") && !(formName == "HELOC Management") && !(formName == "FHA Management") && !(formName == "ATR/QM Management"))
        {
          string str1;
          try
          {
            str1 = string.Compare(formName, "ULDD - Fannie Mae", true) != 0 ? (string.Compare(formName, "ULDD - Freddie Mac", true) != 0 ? (string.Compare(formName, "ULDD - Ginnie Mae", true) != 0 ? (formName.IndexOf("Custom Fields") != -1 || formName.IndexOf("Self-Employed Income 1084") != -1 || !(formName != "MLDS P4") || formName.ToLower().StartsWith("state specific information") || formName.StartsWith("USDA_") || formName.StartsWith("VATool_") || formName.StartsWith("HELOCTool_") || formName.StartsWith("FPMS_") || formName.StartsWith("ATR_") || formName.StartsWith("CONSTRUCTIONMANAGEMENT_") || formName.StartsWith("HMDA2018_") || formName.StartsWith("2018 HMDA ") || formName.StartsWith("Repurchased") || formName.StartsWith("COMPLIANCEREVIEWRESULT") ? formName.ToUpper() : this.systemFormList.GetFormByName(formName).FormID.ToUpper()) : "ULDD_GinnieMae") : "ULDD_FreddieMac") : "ULDD_FannieMae";
          }
          catch (Exception ex)
          {
            continue;
          }
          InputFormInfo form = this.systemFormList.GetForm(str1);
          if (formName.ToLower().StartsWith("state specific information"))
            form = new InputFormInfo("State_Specific_" + Utils.GetStateAbbr(formName.Substring(formName.IndexOf("-") + 1).Trim()), formName);
          else if (string.Compare(formName, "HMDA2018_Purchased", true) == 0 || string.Compare(formName, "2018 HMDA Purchased Loans", true) == 0)
            form = new InputFormInfo("HMDA2018_Purchased", formName);
          else if (string.Compare(formName, "HMDA2018_Repurchased", true) == 0 || string.Compare(formName, "Repurchased Loans", true) == 0)
            form = new InputFormInfo("HMDA2018_Repurchased", formName);
          else if (string.Compare(formName, "HMDA2018_Originated", true) == 0 || string.Compare(formName, "2018 HMDA Originated/Adverse Action Loans", true) == 0)
            form = new InputFormInfo("HMDA2018_Originated", formName);
          switch (str1)
          {
            case "MLDS P4":
              form = new InputFormInfo("RE88395PG4", "MLDS CA - GFE Page 4");
              break;
            case "COMPLIANCEREVIEWRESULT":
              form = new InputFormInfo("COMPLIANCEREVIEWRESULT", "COMPLIANCEREVIEWRESULT");
              break;
            case "D10034":
              form = (InputFormInfo) null;
              break;
            case "ECSDATAVIEWER":
              form = new InputFormInfo("ECSDATAVIEWER", "ECSDATAVIEWER");
              break;
            case "FUNDINGWORKSHEET":
              form = new InputFormInfo("FUNDERWORKSHEET", formName);
              break;
            case "HMDA_DENIAL":
              if (this.loan.GetSimpleField("1825") != "2009")
              {
                form = new InputFormInfo("HMDA_DENIAL04", formName);
                break;
              }
              break;
            case "INTERIMSERVICING":
              form = new InputFormInfo("SERVICINGDETAIL", formName);
              break;
            case "RE88395":
              if (this.loan.Use2010RESPA || this.loan.Use2015RESPA)
              {
                form = new InputFormInfo("RE882", formName);
                break;
              }
              break;
            case "SELF-EMPLOYED INCOME 1084":
              form = new InputFormInfo("FM1084A", formName);
              break;
            case "SELF-EMPLOYED INCOME 1084 2":
              form = new InputFormInfo("FM1084B", formName);
              break;
            case "ULDD_FannieMae":
              form = new InputFormInfo("ULDD_FannieMae", "Fannie Mae");
              break;
            case "ULDD_FreddieMac":
              form = new InputFormInfo("ULDD_FreddieMac", "Freddie Mac");
              break;
            case "ULDD_GinnieMae":
              form = new InputFormInfo("ULDD_GinnieMae", "Ginnie Mae");
              break;
            default:
              if (str1.StartsWith("CUSTOM FIELDS"))
              {
                form = new InputFormInfo("CUSTOMFIELDS", str1);
                break;
              }
              if (formName.StartsWith("USDA_") || formName.StartsWith("VATool_") || formName.StartsWith("HELOCTool_") || formName.StartsWith("FPMS_") || formName.StartsWith("ATR_") || formName.StartsWith("CONSTRUCTIONMANAGEMENT_") || formName.StartsWith("HMDA2018_"))
              {
                form = new InputFormInfo(formName, formName);
                break;
              }
              break;
          }
          if (!(form == (InputFormInfo) null))
          {
            if (str1.StartsWith("CUSTOM FIELDS") && fieldGoTo.StartsWith("CUST") && fieldGoTo.EndsWith("FV"))
            {
              int num = Utils.ParseInt((object) fieldGoTo.Replace("CUST", "").Replace("FV", ""));
              if (num <= 25 && str1 == "CUSTOM FIELDS" || num > 25 && num <= 50 && str1 == "CUSTOM FIELDS 2" || num > 50 && num <= 75 && str1 == "CUSTOM FIELDS 3" || num > 75 && str1 == "CUSTOM FIELDS 4")
              {
                inputForm = form;
                break;
              }
            }
            if (str1 == "BORVESTING")
            {
              if (this.findFieldInWindowsForm(fieldGoTo, new BorrowerVestingForm(this.loan).Controls))
              {
                inputForm = form;
                break;
              }
            }
            else if (str1 == "BROKERCHECKCALCULATION" && this.findFieldInWindowsForm(fieldGoTo, new BrokerCheckForm(this.loan).Controls))
            {
              inputForm = form;
              break;
            }
            if (!(str1 == "BORVESTING") && !str1.StartsWith("CUSTOM FIELDS") && !(str1 == "FUNDBALANCINGWORKSHEET") && !(str1 == "BROKERCHECKCALCULATION"))
            {
              byte[] numArray = (byte[]) null;
              try
              {
                using (FileStream fileStream = File.OpenRead(FormStore.GetFormHTMLPath(Session.DefaultInstance, form)))
                {
                  numArray = new byte[fileStream.Length];
                  fileStream.Read(numArray, 0, numArray.Length);
                }
              }
              catch (Exception ex)
              {
                Tracing.Log(SearchFields.sw, TraceLevel.Warning, nameof (SearchFields), "Can't open " + str1 + " file for 'Field Go To' function. Message: " + ex.Message);
                continue;
              }
              string str2 = Encoding.ASCII.GetString(numArray);
              int num1 = 0;
              int num2 = -1;
              int num3 = -1;
              string empty = string.Empty;
              while (num1 > -1)
              {
                num1 = str2.IndexOf("\"" + fieldGoTo + "\"", num2 + 1, StringComparison.OrdinalIgnoreCase);
                if (num1 > -1)
                {
                  int startIndex = str2.LastIndexOf("<", num1 + 1);
                  if (startIndex <= num3)
                  {
                    num2 = num1;
                  }
                  else
                  {
                    int num4 = str2.IndexOf(">", startIndex + 1);
                    num2 = num4;
                    if (startIndex > -1 && num4 > -1)
                    {
                      num3 = startIndex;
                      string str3 = str2.Substring(startIndex, num4 - startIndex);
                      if (str3.ToUpper().IndexOf("EMID=\"" + fieldGoTo + "\"") > -1 || str3.ToUpper().IndexOf("PARAM") > -1 && str3.IndexOf("emid") > -1)
                      {
                        inputForm = form;
                        break;
                      }
                    }
                  }
                }
              }
              if (inputForm != (InputFormInfo) null)
                break;
            }
          }
        }
      }
      this.loan.FieldForGoTo = "";
      if (inputForm == (InputFormInfo) null)
      {
        InputFormInfo inputFormInfo = (InputFormInfo) null;
        switch (fieldGoTo)
        {
          case "1871":
            this.specialInstruction = "Please click Edit button. The field 1871 is 'Type' field.";
            inputFormInfo = new InputFormInfo("BORVESTING", "Borrower Information - Vesting");
            break;
          case "1991":
          case "1992":
          case "1994":
          case "1995":
          case "1996":
          case "1997":
          case "1998":
          case "1999":
          case "2000":
          case "2002":
          case "2003":
          case "2004":
          case "2007":
          case "2011":
            inputFormInfo = new InputFormInfo("FUNDINGWORKSHEET", "Funding Worksheet");
            break;
        }
        if (fieldGoTo.StartsWith("AUSF.X"))
          inputFormInfo = new InputFormInfo("AUSTRACKING", "AUS Tracking");
        if (inputFormInfo != (InputFormInfo) null)
        {
          foreach (string formName in formNames)
          {
            if (string.Compare(inputFormInfo.Name, formName, true) == 0)
            {
              this.loan.FieldForGoTo = fieldGoTo;
              inputForm = inputFormInfo;
              break;
            }
          }
        }
      }
      return inputForm;
    }

    private bool findFieldInWindowsForm(string goToField, Control.ControlCollection cc)
    {
      foreach (Control control in (ArrangedElementCollection) cc)
      {
        switch (control)
        {
          case TextBox _:
          case ComboBox _:
          case CheckBox _:
            if (control.Tag is string && control.Tag.ToString() == goToField)
              return true;
            continue;
          default:
            if (this.findFieldInWindowsForm(goToField, control.Controls))
              return true;
            continue;
        }
      }
      return false;
    }

    public string SpecialInstruction => this.specialInstruction;

    public void GoToField(
      string fieldID,
      string formName,
      LoanDataMgr loanMgr,
      LoanScreen freeScreen)
    {
      this.loanMgr = loanMgr;
      this.countGoTo = 1;
      this.fieldGoTo = fieldID;
      this.freeScreen = freeScreen;
      InputFormInfo inputForm = this.FindInputForm(new string[1]
      {
        formName
      }, fieldID, fieldID);
      if (inputForm == (InputFormInfo) null)
      {
        this.countGoTo = 0;
        int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "Encompass was unable to find this field '" + fieldID + "' in current form.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.countGoTo = 1;
        this.freeScreen.SetGoToFieldFocus(this.fieldGoTo, this.countGoTo);
        Session.Application.GetService<ILoanEditor>().OpenForm(inputForm.Name);
      }
    }

    public void GoToField(
      string fieldID,
      bool findNext,
      bool searchToolPageOnly,
      Control currentTopControl,
      EMFormMenu emFormMenuBox,
      EMFormMenu emToolMenuBox,
      InputFormInfo currForm,
      TabControl toolsFormsTabControl,
      VOLPanel volPan,
      VORPanel vorPan,
      VOEPanel voePan,
      VOMPanel vomPan,
      TAX4506Panel tax4506Panel,
      TAX4506TPanel tax4506TPanel,
      CheckBox allFormBox,
      LoanScreen freeScreen,
      Sessions.Session session,
      ILoanEditor customEditor = null)
    {
      if (fieldID == string.Empty)
        return;
      InputFormInfo inputFormInfo = (InputFormInfo) null;
      if (currentTopControl != null && currentTopControl is TabLinksControl)
        inputFormInfo = ((TabLinksControl) currentTopControl).GetCurrentChildFormInfo();
      this.editor = customEditor != null ? customEditor : Session.Application.GetService<ILoanEditor>();
      this.emFormMenuBox = emFormMenuBox;
      this.emToolMenuBox = emToolMenuBox;
      this.currForm = currForm;
      this.toolsFormsTabControl = toolsFormsTabControl;
      this.vorPan = vorPan;
      this.voePan = voePan;
      this.volPan = volPan;
      this.vomPan = vomPan;
      this.tax4506Panel = tax4506Panel;
      this.tax4506TPanel = tax4506TPanel;
      this.allFormBox = allFormBox;
      this.freeScreen = freeScreen;
      this.session = session;
      this.fieldGoTo = fieldID.ToUpper();
      bool flag1 = findNext;
      string str1 = string.Empty;
      bool isBorrower = false;
      int blockNo = 0;
      if (this.fieldGoTo.Length == 6)
      {
        string str2 = this.fieldGoTo.Substring(0, 2);
        switch (str2)
        {
          case "AR":
            str1 = this.fieldGoTo;
            this.fieldGoTo = "AR00" + this.fieldGoTo.Substring(4, 2);
            break;
          case "BE":
          case "CE":
            str1 = this.fieldGoTo;
            this.fieldGoTo = "FE00" + this.fieldGoTo.Substring(4, 2);
            if (str2 == "BE")
            {
              isBorrower = true;
              break;
            }
            break;
          case "BR":
          case "CR":
            str1 = this.fieldGoTo;
            this.fieldGoTo = "FR00" + this.fieldGoTo.Substring(4, 2);
            if (str2 == "BR")
            {
              isBorrower = true;
              break;
            }
            break;
          case "DD":
            str1 = this.fieldGoTo;
            this.fieldGoTo = "DD00" + this.fieldGoTo.Substring(4, 2);
            break;
          case "FL":
            str1 = this.fieldGoTo;
            this.fieldGoTo = "FL00" + this.fieldGoTo.Substring(4, 2);
            break;
          case "FM":
            str1 = this.fieldGoTo;
            this.fieldGoTo = "FM00" + this.fieldGoTo.Substring(4, 2);
            break;
          case "IR":
            str1 = this.fieldGoTo;
            this.fieldGoTo = "IR00" + this.fieldGoTo.Substring(4, str1.Length == 8 ? 4 : 2);
            break;
          default:
            str2 = string.Empty;
            break;
        }
        if (str2 != string.Empty)
          blockNo = Utils.ParseInt((object) str1.Substring(2, 2));
      }
      Cursor.Current = Cursors.WaitCursor;
      ArrayList arrayList = new ArrayList();
      ArrayList formNames1 = new ArrayList((ICollection) emFormMenuBox.Items);
      ArrayList formNames2 = new ArrayList((ICollection) emToolMenuBox.Items);
      string empty = string.Empty;
      if (flag1)
      {
        if (toolsFormsTabControl.SelectedIndex == 0)
        {
          if (!searchToolPageOnly)
            this.addFormListForGoTo(arrayList, formNames1, this.currForm, false);
          if (formNames2 != null && formNames2.Count > 0)
            this.addFormListForGoTo(arrayList, formNames2, (InputFormInfo) null, true);
          if (!searchToolPageOnly)
            this.addFormListForGoTo(arrayList, formNames1, this.currForm, true);
        }
        else
        {
          if (formNames2 != null && formNames2.Count > 0)
            this.addFormListForGoTo(arrayList, formNames2, this.currForm, false);
          if (!searchToolPageOnly)
            this.addFormListForGoTo(arrayList, formNames1, (InputFormInfo) null, true);
          if (formNames2 != null && formNames2.Count > 0)
            this.addFormListForGoTo(arrayList, formNames2, this.currForm, true);
        }
        if ((InputFormInfo) null != this.currForm)
        {
          if (this.currForm.Name.StartsWith("State Specific Information - "))
            arrayList.Insert(0, (object) "State-Specific Disclosure Information");
          else
            arrayList.Add((object) this.currForm.Name);
        }
      }
      else
      {
        if (!searchToolPageOnly)
          this.addFormListForGoTo(arrayList, formNames1, (InputFormInfo) null, true);
        ArrayList formNames3 = new ArrayList();
        foreach (InputFormInfo form in this.getFormList(true))
        {
          if (!arrayList.Contains((object) form.Name))
            formNames3.Add((object) form.Name);
        }
        if (formNames3.Count > 0)
          this.addFormListForGoTo(arrayList, formNames3, (InputFormInfo) null, true);
        if (formNames2 != null && formNames2.Count > 0)
          this.addFormListForGoTo(arrayList, formNames2, (InputFormInfo) null, true);
      }
      if (arrayList.Contains((object) "Custom Fields"))
      {
        int num = -1;
        foreach (string str3 in arrayList)
        {
          ++num;
          if (str3 == "Custom Fields")
          {
            int index = num + 1;
            arrayList.Insert(index, (object) "Custom Fields 4");
            arrayList.Insert(index, (object) "Custom Fields 3");
            arrayList.Insert(index, (object) "Custom Fields 2");
            break;
          }
        }
      }
      if (arrayList.Contains((object) "ULDD/PDD"))
      {
        int num = -1;
        foreach (string str4 in arrayList)
        {
          ++num;
          if (str4 == "ULDD/PDD")
          {
            bool insertToEndList = false;
            int formPosition = num + 1;
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "ULDD" ? (InputFormInfo) null : inputFormInfo, "ULDD - Ginnie Mae", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "ULDD" ? (InputFormInfo) null : inputFormInfo, "ULDD - Freddie Mac", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "ULDD" ? (InputFormInfo) null : inputFormInfo, "ULDD - Fannie Mae", ref insertToEndList);
            break;
          }
        }
      }
      if (arrayList.Contains((object) "ECS Data Viewer"))
      {
        int num = -1;
        foreach (string str5 in arrayList)
        {
          ++num;
          if (str5 == "ECS Data Viewer")
          {
            bool insertToEndList = false;
            int formPosition = num + 1;
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "ECSDATAVIEWER" ? (InputFormInfo) null : inputFormInfo, "COMPLIANCEREVIEWRESULT", ref insertToEndList);
            break;
          }
        }
      }
      if (arrayList.Contains((object) "USDA Management"))
      {
        int num = -1;
        foreach (string str6 in arrayList)
        {
          ++num;
          if (str6 == "USDA Management")
          {
            bool insertToEndList = false;
            int formPosition = num + 1;
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "USDAManagement" ? (InputFormInfo) null : inputFormInfo, "USDA_Tracking", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "USDAManagement" ? (InputFormInfo) null : inputFormInfo, "USDA_LoanClosing", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "USDAManagement" ? (InputFormInfo) null : inputFormInfo, "USDA_SFHLoan", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "USDAManagement" ? (InputFormInfo) null : inputFormInfo, "USDA_RURALIncome", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "USDAManagement" ? (InputFormInfo) null : inputFormInfo, "USDA_RURAL6", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "USDAManagement" ? (InputFormInfo) null : inputFormInfo, "USDA_RURAL5", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "USDAManagement" ? (InputFormInfo) null : inputFormInfo, "USDA_RURAL4", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "USDAManagement" ? (InputFormInfo) null : inputFormInfo, "USDA_RURAL3", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "USDAManagement" ? (InputFormInfo) null : inputFormInfo, "USDA_RURAL2", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "USDAManagement" ? (InputFormInfo) null : inputFormInfo, "USDA_RURAL1", ref insertToEndList);
            break;
          }
        }
      }
      if (arrayList.Contains((object) "FHA Management"))
      {
        int num = -1;
        foreach (string str7 in arrayList)
        {
          ++num;
          if (str7 == "FHA Management")
          {
            bool insertToEndList = false;
            int formPosition = num + 1;
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "FHAPROCESSMGT" ? (InputFormInfo) null : inputFormInfo, "FPMS_Tracking", ref insertToEndList);
            if (this.session != null && this.session.LoanDataMgr != null && this.session.LoanDataMgr.LoanData != null && this.session.LoanDataMgr.LoanData.GetField("19").IndexOf("Refinance") > -1)
            {
              if (this.session.LoanDataMgr.InputFormSettings.IsAccessible("MAX23K"))
                this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "FHAPROCESSMGT" ? (InputFormInfo) null : inputFormInfo, "FPMS_203KRefi", ref insertToEndList);
              this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "FHAPROCESSMGT" ? (InputFormInfo) null : inputFormInfo, "FPMS_Refi", ref insertToEndList);
            }
            else
            {
              if (this.session.LoanDataMgr.InputFormSettings.IsAccessible("MAX23K"))
                this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "FHAPROCESSMGT" ? (InputFormInfo) null : inputFormInfo, "FPMS_203KPurchase", ref insertToEndList);
              this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "FHAPROCESSMGT" ? (InputFormInfo) null : inputFormInfo, "FPMS_Purchase", ref insertToEndList);
            }
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "FHAPROCESSMGT" ? (InputFormInfo) null : inputFormInfo, "FPMS_BasicInfo", ref insertToEndList);
            break;
          }
        }
      }
      if (arrayList.Contains((object) "VA Management"))
      {
        int num = -1;
        foreach (string str8 in arrayList)
        {
          ++num;
          if (str8 == "VA Management")
          {
            bool insertToEndList = false;
            int formPosition = num + 1;
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "VAManagement" ? (InputFormInfo) null : inputFormInfo, "VATool_Tracking", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "VAManagement" ? (InputFormInfo) null : inputFormInfo, "VATool_CashOutRefinance", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "VAManagement" ? (InputFormInfo) null : inputFormInfo, "VATool_Qualification", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "VAManagement" ? (InputFormInfo) null : inputFormInfo, "VATool_BorrowerInfo", ref insertToEndList);
            break;
          }
        }
      }
      if (arrayList.Contains((object) "HELOC Management"))
      {
        int num = -1;
        foreach (string str9 in arrayList)
        {
          ++num;
          if (str9 == "HELOC Management")
          {
            bool insertToEndList = false;
            int formPosition = num + 1;
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "HELOCManagement" ? (InputFormInfo) null : inputFormInfo, "HELOCTool_Program", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "HELOCManagement" ? (InputFormInfo) null : inputFormInfo, "HELOCTool_Terms", ref insertToEndList);
            break;
          }
        }
      }
      if (arrayList.Contains((object) "Construction Management"))
      {
        int num = -1;
        foreach (string str10 in arrayList)
        {
          ++num;
          if (str10 == "Construction Management")
          {
            bool insertToEndList = false;
            int formPosition = num + 1;
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "CONSTRUCTIONMANAGEMENT" ? (InputFormInfo) null : inputFormInfo, "CONSTRUCTIONMANAGEMENT_BasicInfo", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "CONSTRUCTIONMANAGEMENT" ? (InputFormInfo) null : inputFormInfo, "CONSTRUCTIONMANAGEMENT_LoanInfo", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "CONSTRUCTIONMANAGEMENT" ? (InputFormInfo) null : inputFormInfo, "CONSTRUCTIONMANAGEMENT_ProjectData", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "CONSTRUCTIONMANAGEMENT" ? (InputFormInfo) null : inputFormInfo, "CONSTRUCTIONMANAGEMENT_LinkConstPerm", ref insertToEndList);
            break;
          }
        }
      }
      if (arrayList.Contains((object) "ATR/QM Management"))
      {
        int num = -1;
        foreach (string str11 in arrayList)
        {
          ++num;
          if (str11 == "ATR/QM Management")
          {
            bool insertToEndList = false;
            int formPosition = num + 1;
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "ATRManagement" ? (InputFormInfo) null : inputFormInfo, "ATR_AppendixQ", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "ATRManagement" ? (InputFormInfo) null : inputFormInfo, "ATR_Refi", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "ATRManagement" ? (InputFormInfo) null : inputFormInfo, "ATR_Eligibility", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "ATRManagement" ? (InputFormInfo) null : inputFormInfo, "ATR_Qualification", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "ATRManagement" ? (InputFormInfo) null : inputFormInfo, "ATR_BorrowerInfo", ref insertToEndList);
            break;
          }
        }
      }
      if (arrayList.Contains((object) "State-Specific Disclosure Information"))
      {
        int num1 = -1;
        foreach (string str12 in arrayList)
        {
          ++num1;
          if (str12 == "State-Specific Disclosure Information")
          {
            int index1 = num1 + 1;
            string[] strArray1 = LoanScreen.STATESPECIFICFORMS.Split(',');
            if (this.currForm != (InputFormInfo) null && this.currForm.FormID.StartsWith("State_Specific_"))
            {
              string str13 = this.currForm.FormID.Substring(this.currForm.FormID.Length - 2);
              int num2 = LoanScreen.STATESPECIFICFORMS.IndexOf("," + str13 + ",");
              if (num2 > -1)
              {
                string[] strArray2 = LoanScreen.STATESPECIFICFORMS.Substring(1, num2 + 3).Split(',');
                for (int index2 = 0; index2 < strArray2.Length; ++index2)
                {
                  if (!(strArray2[index2] == string.Empty))
                    arrayList.Add((object) ("State Specific Information - " + Utils.GetFullStateName(strArray2[index2])));
                }
                strArray1 = LoanScreen.STATESPECIFICFORMS.Substring(num2 + 3).Split(',');
              }
            }
            for (int index3 = strArray1.Length - 1; index3 >= 0; --index3)
            {
              if (!(strArray1[index3] == string.Empty))
                arrayList.Insert(index1, (object) ("State Specific Information - " + Utils.GetFullStateName(strArray1[index3])));
            }
            if (arrayList.Contains((object) "State-Specific Disclosure Information"))
            {
              arrayList.Remove((object) "State-Specific Disclosure Information");
              break;
            }
            break;
          }
        }
      }
      if (arrayList.Contains((object) "Self-Employed Income 1084"))
      {
        int num = -1;
        foreach (string str14 in arrayList)
        {
          ++num;
          if (str14 == "Self-Employed Income 1084")
          {
            int index = num + 1;
            arrayList.Insert(index, (object) "Self-Employed Income 1084 2");
            break;
          }
        }
      }
      if (arrayList.Contains((object) "MLDS - CA GFE"))
      {
        int num = -1;
        foreach (string str15 in arrayList)
        {
          ++num;
          if (str15 == "MLDS - CA GFE")
          {
            int index4 = num + 1;
            if (this.loan.Use2010RESPA || this.loan.Use2015RESPA)
            {
              arrayList.Insert(index4, (object) "885 P1-3");
              int index5 = index4 + 1;
              arrayList.Insert(index5, (object) "885 P4");
              break;
            }
            arrayList.Insert(index4, (object) "MLDS P4");
            break;
          }
        }
      }
      if (arrayList.Contains((object) "Underwriter Summary"))
      {
        int num = -1;
        foreach (string str16 in arrayList)
        {
          ++num;
          if (str16 == "Underwriter Summary")
          {
            int index = num + 1;
            arrayList.Insert(index, (object) "Underwriter Summary Page 2");
            break;
          }
        }
      }
      if (arrayList.Contains((object) "HMDA Information") && this.loan != null && (Utils.ParseInt((object) this.loan.GetField("HMDA.X27")) >= 2018 || this.loan.IsInFindFieldForm))
      {
        int num = -1;
        foreach (string str17 in arrayList)
        {
          ++num;
          if (str17 == "HMDA Information")
          {
            bool insertToEndList = false;
            int formPosition = num + 1;
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "HMDA_DENIAL04" ? (InputFormInfo) null : inputFormInfo, "HMDA2018_Purchased", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "HMDA_DENIAL04" ? (InputFormInfo) null : inputFormInfo, "HMDA2018_Repurchased", ref insertToEndList);
            this.insertTabStyleChildForm(arrayList, formPosition, this.currForm == (InputFormInfo) null || this.currForm.FormID != "HMDA_DENIAL04" ? (InputFormInfo) null : inputFormInfo, "HMDA2018_Originated", ref insertToEndList);
            arrayList.Remove((object) "HMDA Information");
            break;
          }
        }
      }
      this.countGoTo = 1;
      InputFormInfo inputForm = this.FindInputForm((string[]) arrayList.ToArray(typeof (string)), this.fieldGoTo, str1);
      Cursor.Current = Cursors.Default;
      if (inputForm == (InputFormInfo) null)
      {
        if (str1 != string.Empty)
          this.fieldGoTo = str1;
        int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "Encompass was unable to find a form that contains field '" + this.fieldGoTo + "' in current form list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.fieldGoTo = string.Empty;
        this.countGoTo = 0;
      }
      else
      {
        int num3 = -1;
        bool flag2 = false;
        if (!searchToolPageOnly)
        {
          for (int index = 0; index < this.emFormMenuBox.Items.Count; ++index)
          {
            string upper = ((string) this.emFormMenuBox.Items[index]).ToUpper();
            if (upper == inputForm.Name.ToUpper() || upper == "CUSTOM FIELDS" && inputForm.Name.ToUpper().IndexOf("CUSTOM FIELDS") > -1 || upper == "SELF-EMPLOYED INCOME 1084" && inputForm.Name.ToUpper().IndexOf("SELF-EMPLOYED INCOME 1084") > -1 || upper == "MLDS - CA GFE" && inputForm.Name.ToUpper().IndexOf("MLDS ") > -1 || upper == "USDA MANAGEMENT" && inputForm.Name.ToUpper().StartsWith("USDA_") || upper == "FHA MANAGEMENT" && inputForm.Name.ToUpper().StartsWith("FPMS_") || upper == "VA MANAGEMENT" && inputForm.Name.ToUpper().StartsWith("VATOOL_") || upper == "HELOC MANAGEMENT" && inputForm.Name.ToUpper().StartsWith("HELOCTOOL_") || upper == "ATR/QM MANAGEMENT" && inputForm.Name.ToUpper().StartsWith("ATR_") || upper == "CONSTRUCTION MANAGEMENT" && inputForm.Name.ToUpper().StartsWith("CONSTRUCTIONMANAGEMENT_") || upper == "HMDA INFORMATION" && inputForm.Name.ToUpper().StartsWith("HMDA2018_"))
            {
              num3 = index;
              break;
            }
            if (upper == "ULDD/PDD" && (inputForm.Name.ToUpper() == "FANNIE MAE" || inputForm.Name.ToUpper() == "FREDDIE MAC" || inputForm.Name.ToUpper() == "GINNIE MAE"))
            {
              num3 = index;
              break;
            }
          }
        }
        if (num3 == -1)
        {
          for (int index = 0; index < this.emToolMenuBox.Items.Count; ++index)
          {
            string upper = ((string) this.emToolMenuBox.Items[index]).ToUpper();
            if (upper == inputForm.Name.ToUpper() || inputForm.Name.ToUpper() == "UNDERWRITER SUMMARY PAGE 2" && upper == "UNDERWRITER SUMMARY" || inputForm.Name.ToUpper() == "COMPLIANCEREVIEWRESULT" && upper == "ECS DATA VIEWER" || inputForm.Name.ToUpper() == "ECSDATAVIEWER" && upper == "ECS DATA VIEWER")
            {
              num3 = index;
              flag2 = true;
              break;
            }
          }
        }
        if (!searchToolPageOnly && num3 == -1 && inputForm.FormID.ToUpper() != "RE88395PG4")
        {
          this.allFormBox.Checked = true;
          for (int index = 0; index < this.emFormMenuBox.Items.Count; ++index)
          {
            string upper = ((string) this.emFormMenuBox.Items[index]).ToUpper();
            if (upper == inputForm.Name.ToUpper() || upper == "CUSTOM FIELDS" && inputForm.Name.ToUpper().IndexOf("CUSTOM FIELDS") > -1 || upper == "SELF-EMPLOYED INCOME 1084" && inputForm.Name.ToUpper().IndexOf("SELF-EMPLOYED INCOME 1084") > -1 || upper == "MLDS - CA GFE" && inputForm.Name.ToUpper().IndexOf("MLDS ") > -1 || upper == "USDA MANAGEMENT" && inputForm.Name.ToUpper().StartsWith("USDA_") || upper == "FHA MANAGEMENT" && inputForm.Name.ToUpper().StartsWith("FPMS_") || upper == "VA MANAGEMENT" && inputForm.Name.ToUpper().StartsWith("VATOOL_") || upper == "HELOC MANAGEMENT" && inputForm.Name.ToUpper().StartsWith("HELOCTOOL_") || upper == "ATR/QM MANAGEMENT" && inputForm.Name.ToUpper().StartsWith("ATR_") || upper == "CONSTRUCTION MANAGEMENT" && inputForm.Name.ToUpper().StartsWith("CONSTRUCTIONMANAGEMENT_") || upper == "HMDA INFORMATION" && inputForm.Name.ToUpper().StartsWith("HMDA2018_"))
            {
              num3 = index;
              break;
            }
          }
        }
        if (num3 == -1 && inputForm.FormID.ToUpper() != "RE88395PG4" && !inputForm.Name.StartsWith("State Specific Information"))
        {
          if (str1 != string.Empty)
            this.fieldGoTo = str1;
          int num4 = (int) Utils.Dialog((IWin32Window) Session.MainForm, "Encompass was unable to find a form that contains field '" + this.fieldGoTo + "' in current form list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.fieldGoTo = string.Empty;
          this.countGoTo = 0;
        }
        else
        {
          bool flag3 = false;
          switch (inputForm.Name.ToUpper())
          {
            case "ATR_APPENDIXQ":
            case "ATR_BORROWERINFO":
            case "ATR_ELIGIBILITY":
            case "ATR_QUALIFICATION":
            case "ATR_REFI":
            case "COMPLIANCEREVIEWRESULT":
            case "CONSTRUCTIONMANAGEMENT_BASICINFO":
            case "CONSTRUCTIONMANAGEMENT_LINKCONSTPERM":
            case "CONSTRUCTIONMANAGEMENT_LOANINFO":
            case "CONSTRUCTIONMANAGEMENT_PROJECTDATA":
            case "CUSTOM FIELDS 2":
            case "CUSTOM FIELDS 3":
            case "CUSTOM FIELDS 4":
            case "ECSDATAVIEWER":
            case "FANNIE MAE":
            case "FPMS_203KPURCHASE":
            case "FPMS_203KREFI":
            case "FPMS_BASICINFO":
            case "FPMS_PURCHASE":
            case "FPMS_REFI":
            case "FPMS_TRACKING":
            case "FREDDIE MAC":
            case "GINNIE MAE":
            case "HELOCTOOL_PROGRAM":
            case "HELOCTOOL_TERMS":
            case "HMDA2018_ORIGINATED":
            case "HMDA2018_PURCHASED":
            case "HMDA2018_REPURCHASED":
            case "MLDS CA - GFE PAGE 4":
            case "SELF-EMPLOYED INCOME 1084 2":
            case "UNDERWRITER SUMMARY PAGE 2":
            case "USDA_LOANCLOSING":
            case "USDA_RURAL1":
            case "USDA_RURAL2":
            case "USDA_RURAL3":
            case "USDA_RURAL4":
            case "USDA_RURAL5":
            case "USDA_RURAL6":
            case "USDA_RURALINCOME":
            case "USDA_SFHLOAN":
            case "USDA_TRACKING":
            case "VATOOL_BORROWERINFO":
            case "VATOOL_CASHOUTREFINANCE":
            case "VATOOL_QUALIFICATION":
            case "VATOOL_TRACKING":
              flag3 = true;
              if (flag2)
              {
                toolsFormsTabControl.SelectedIndex = 1;
                emToolMenuBox.SelectedIndex = num3;
                break;
              }
              toolsFormsTabControl.SelectedIndex = 0;
              emFormMenuBox.SelectedIndex = -1;
              emFormMenuBox.SelectedIndex = num3;
              break;
            case "REQUEST FOR COPY OF TAX RETURN":
              emFormMenuBox.SelectedIndex = num3;
              int numberOfTaX4506Ts1 = this.loan.GetNumberOfTAX4506Ts(true);
              if (numberOfTaX4506Ts1 == 0 || this.tax4506Panel == null)
              {
                int num5 = (int) Utils.Dialog((IWin32Window) Session.MainForm, "You can find this field '" + this.fieldGoTo + " in the input form 'Request for Copy of Tax Return'. Currently you don't have a record in this input form.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
              }
              if (blockNo == 0 || blockNo > numberOfTaX4506Ts1)
                blockNo = 1;
              this.tax4506Panel.OpenVerification(blockNo - 1, this.fieldGoTo, this.countGoTo);
              this.fieldGoTo = str1;
              break;
            case "REQUEST FOR TRANSCRIPT OF TAX":
              emFormMenuBox.SelectedIndex = num3;
              int numberOfTaX4506Ts2 = this.loan.GetNumberOfTAX4506Ts(false);
              if (numberOfTaX4506Ts2 == 0 || this.tax4506TPanel == null)
              {
                int num6 = (int) Utils.Dialog((IWin32Window) Session.MainForm, "You can find this field '" + this.fieldGoTo + " in the input form 'Request for Transcript of Tax'. Currently you don't have a record in this input form.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
              }
              if (blockNo == 0 || blockNo > numberOfTaX4506Ts2)
                blockNo = 1;
              this.tax4506TPanel.OpenVerification(blockNo - 1, this.fieldGoTo, this.countGoTo);
              this.fieldGoTo = str1;
              break;
            case "VOE":
            case "VOL":
            case "VOM":
            case "VOR":
              if (!this.getVerifScreenObject(inputForm.Name.ToUpper(), isBorrower, num3, blockNo))
                return;
              this.fieldGoTo = str1;
              break;
            default:
              if (str1 != string.Empty)
              {
                this.freeScreen.SetGoToFieldFocus(str1, this.countGoTo);
                this.fieldGoTo = str1;
              }
              else
                this.freeScreen.SetGoToFieldFocus(this.fieldGoTo, this.countGoTo);
              if (flag2)
              {
                toolsFormsTabControl.SelectedIndex = 1;
                emToolMenuBox.SelectedIndex = num3;
                this.editor.OpenForm(inputForm.Name);
                if (inputForm.Name == "Funding Worksheet")
                {
                  this.freeScreen.SetGoToFieldInHybridForm(this.fieldGoTo);
                  break;
                }
                break;
              }
              toolsFormsTabControl.SelectedIndex = 0;
              if (inputForm.Name.StartsWith("State Specific Information"))
              {
                emFormMenuBox.SelectedIndex = -1;
                this.freeScreen.LoadForm(inputForm);
                this.editor.AddToWorkArea((Control) this.freeScreen, true);
                this.freeScreen.SetTitle(currForm.Name);
                break;
              }
              if (inputForm.Name == "2015 Itemization")
              {
                emFormMenuBox.SetSelected(num3, true);
                this.freeScreen.SetGoToFieldInHybridForm(this.fieldGoTo);
                break;
              }
              emFormMenuBox.SelectedIndex = -1;
              emFormMenuBox.SelectedIndex = num3;
              break;
          }
          if (inputForm.Name.ToUpper() == "CUSTOM FIELDS" || inputForm.Name.ToUpper() == "BORROWER INFORMATION - VESTING")
            this.setFieldFocusInWindowsForm(this.fieldGoTo, this.freeScreen.Controls);
          if (!flag3)
          {
            if (!(this.specialInstruction != string.Empty))
              return;
            int num7 = (int) Utils.Dialog((IWin32Window) Session.MainForm, this.specialInstruction, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          else
          {
            if (str1 != string.Empty)
            {
              this.freeScreen.SetGoToFieldFocus(str1, this.countGoTo);
              this.fieldGoTo = str1;
            }
            else
              this.freeScreen.SetGoToFieldFocus(this.fieldGoTo, this.countGoTo);
            this.editor.AddToWorkArea((Control) this.freeScreen);
            switch (inputForm.Name.ToUpper())
            {
              case "CUSTOM FIELDS 2":
              case "CUSTOM FIELDS 3":
              case "CUSTOM FIELDS 4":
                this.currForm = new InputFormInfo("CUSTOMFIELDS", "Custom Fields");
                CustomFieldsPanel navControl1 = new CustomFieldsPanel(this.freeScreen, this.session);
                this.freeScreen.SetTitle(currForm.Name, (Control) navControl1);
                int index = Utils.ParseInt((object) inputForm.Name.Substring(inputForm.Name.Length - 1));
                navControl1.LoadCustomFieldPage(index);
                this.setFieldFocusInWindowsForm(this.fieldGoTo, this.freeScreen.Controls);
                break;
              case "FANNIE MAE":
                this.setFormListIndexWithoutLoadingForm("ULDD");
                this.currForm = new InputFormInfo("ULDD_FannieMae", "Fannie Mae");
                TabLinksControl worksheet1 = new TabLinksControl(this.session, this.currForm, (IWin32Window) Session.MainScreen, this.loan, false);
                worksheet1.LoadFormAndSetGoToField(this.currForm, this.fieldGoTo, this.countGoTo);
                this.editor.AddToWorkArea((Control) worksheet1, true);
                break;
              case "FREDDIE MAC":
                this.setFormListIndexWithoutLoadingForm("ULDD");
                this.currForm = new InputFormInfo("ULDD_FreddieMac", "Freddie Mac");
                TabLinksControl worksheet2 = new TabLinksControl(this.session, this.currForm, (IWin32Window) Session.MainScreen, this.loan, false);
                worksheet2.LoadFormAndSetGoToField(this.currForm, this.fieldGoTo, this.countGoTo);
                this.editor.AddToWorkArea((Control) worksheet2, true);
                break;
              case "GINNIE MAE":
                this.setFormListIndexWithoutLoadingForm("ULDD");
                this.currForm = new InputFormInfo("ULDD_GinnieMae", "Ginnie Mae");
                TabLinksControl worksheet3 = new TabLinksControl(this.session, this.currForm, (IWin32Window) Session.MainScreen, this.loan, false);
                worksheet3.LoadFormAndSetGoToField(this.currForm, this.fieldGoTo, this.countGoTo);
                this.editor.AddToWorkArea((Control) worksheet3, true);
                break;
              case "MLDS CA - GFE PAGE 4":
                this.currForm = new InputFormInfo("RE88395PG4", "MLDS CA - GFE Page 4");
                QuickLinksControl navControl2 = new QuickLinksControl(this.editor, "RE88395", this.session);
                this.freeScreen.SetTitle(this.currForm.Name, (Control) navControl2);
                navControl2.LoadSelectedForm("RE88395PG4");
                this.freeScreen.SetGoToFieldFocus(this.fieldGoTo, this.countGoTo);
                break;
              case "SELF-EMPLOYED INCOME 1084 2":
                this.currForm = new InputFormInfo("FM1084", "Self-Employed Income 1084");
                QuickLinksControl navControl3 = new QuickLinksControl(this.editor, "FM1084A", this.session);
                this.freeScreen.SetTitle(this.currForm.Name, (Control) navControl3);
                navControl3.LoadSelectedForm("FM1084A");
                this.freeScreen.SetGoToFieldFocus(this.fieldGoTo, this.countGoTo);
                break;
              case "UNDERWRITER SUMMARY PAGE 2":
                this.currForm = new InputFormInfo("UNDERWRITERSUMMARYP2", "Underwriter Summary Page 2");
                QuickLinksControl navControl4 = new QuickLinksControl(this.editor, "UNDERWRITERSUMMARY", this.session);
                this.freeScreen.SetTitle(this.currForm.Name, (Control) navControl4);
                navControl4.LoadSelectedForm("UNDERWRITERSUMMARYP2");
                this.freeScreen.SetGoToFieldFocus(this.fieldGoTo, this.countGoTo);
                break;
            }
            if (inputForm != (InputFormInfo) null && (inputForm.Name.StartsWith("USDA_") || inputForm.Name.StartsWith("VATool_") || inputForm.Name.StartsWith("HELOCTool_") || inputForm.Name.StartsWith("FPMS_") || inputForm.Name.StartsWith("ATR_") || inputForm.Name.StartsWith("CONSTRUCTIONMANAGEMENT_") || inputForm.Name.StartsWith("HMDA2018_") || inputForm.Name.StartsWith("COMPLIANCEREVIEWRESULT") || inputForm.Name.StartsWith("ECSDATAVIEWER")))
            {
              switch (inputForm.FormID)
              {
                case "ATR_AppendixQ":
                  this.currForm = new InputFormInfo("ATR_AppendixQ", "Appendix Q");
                  break;
                case "ATR_BorrowerInfo":
                  this.currForm = new InputFormInfo("ATR_BorrowerInfo", "Basic Info");
                  break;
                case "ATR_Eligibility":
                  this.currForm = new InputFormInfo("ATR_Eligibility", "ATR/QM Eligibility");
                  break;
                case "ATR_Qualification":
                  this.currForm = new InputFormInfo("ATR_Qualification", "Qualification");
                  break;
                case "ATR_Refi":
                  this.currForm = new InputFormInfo("ATR_Refi", "Non-Standard to Standard Refi.");
                  break;
                case "COMPLIANCEREVIEWRESULT":
                  this.currForm = new InputFormInfo("COMPLIANCEREVIEWRESULT", "Compliance Review Result");
                  break;
                case "CONSTRUCTIONMANAGEMENT_BasicInfo":
                  this.currForm = new InputFormInfo("CONSTRUCTIONMANAGEMENT_BasicInfo", "Basic Info");
                  break;
                case "CONSTRUCTIONMANAGEMENT_LinkConstPerm":
                  this.currForm = new InputFormInfo("CONSTRUCTIONMANAGEMENT_LinkConstPerm", "Linked Loans");
                  break;
                case "CONSTRUCTIONMANAGEMENT_LoanInfo":
                  this.currForm = new InputFormInfo("CONSTRUCTIONMANAGEMENT_LoanInfo", "Loan Info");
                  break;
                case "CONSTRUCTIONMANAGEMENT_ProjectData":
                  this.currForm = new InputFormInfo("CONSTRUCTIONMANAGEMENT_ProjectData", "Project Data");
                  break;
                case "ECSDATAVIEWER":
                  this.currForm = new InputFormInfo("ECSDATAVIEWER", "ECS Data Viewer");
                  break;
                case "FPMS_203KPurchase":
                  this.currForm = new InputFormInfo("FPMS_203KPurchase", "FHA 203k");
                  break;
                case "FPMS_203KRefi":
                  this.currForm = new InputFormInfo("FPMS_203KRefi", "FHA 203k");
                  break;
                case "FPMS_BasicInfo":
                  this.currForm = new InputFormInfo("FPMS_BasicInfo", "Basic Info");
                  break;
                case "FPMS_Purchase":
                  this.currForm = new InputFormInfo("FPMS_Purchase", "Prequalification");
                  break;
                case "FPMS_Refi":
                  this.currForm = new InputFormInfo("FPMS_Refi", "Prequalification");
                  break;
                case "FPMS_Tracking":
                  this.currForm = new InputFormInfo("FPMS_Tracking", "Tracking");
                  break;
                case "HELOCTool_Program":
                  this.currForm = new InputFormInfo("HELOCTool_Program", "HELOC Program");
                  break;
                case "HELOCTool_Terms":
                  this.currForm = new InputFormInfo("HELOCTool_Terms", "Important Terms and Agreement Language");
                  break;
                case "HMDA2018_Originated":
                  this.currForm = new InputFormInfo("HMDA2018_Originated", "2018 HMDA Originated/Adverse Action Loans");
                  break;
                case "HMDA2018_Purchased":
                  this.currForm = new InputFormInfo("HMDA2018_Purchased", "2018 HMDA Purchased Loans");
                  break;
                case "HMDA2018_Repurchased":
                  this.currForm = new InputFormInfo("HMDA2018_Repurchased", "Repurchased Loans");
                  break;
                case "USDA_LoanClosing":
                  this.currForm = new InputFormInfo("USDA_LoanClosing", "Loan Closing Report");
                  break;
                case "USDA_RURAL1":
                  this.currForm = new InputFormInfo("USDA_RURAL1", "Rural Assistance URLA");
                  break;
                case "USDA_RURAL2":
                  this.currForm = new InputFormInfo("USDA_RURAL2", "Rural Assistance URLA");
                  break;
                case "USDA_RURAL3":
                  this.currForm = new InputFormInfo("USDA_RURAL3", "Rural Assistance URLA");
                  break;
                case "USDA_RURAL4":
                  this.currForm = new InputFormInfo("USDA_RURAL4", "Rural Assistance URLA");
                  break;
                case "USDA_RURAL5":
                  this.currForm = new InputFormInfo("USDA_RURAL5", "Rural Assistance URLA");
                  break;
                case "USDA_RURAL6":
                  this.currForm = new InputFormInfo("USDA_RURAL6", "Rural Assistance URLA");
                  break;
                case "USDA_RURALIncome":
                  this.currForm = new InputFormInfo("USDA_RURALIncome", "Rural Assistance URLA");
                  break;
                case "USDA_SFHLoan":
                  this.currForm = new InputFormInfo("USDA_SFHLoan", "Req for SFH Loan Guarantee/Resv. of Funds");
                  break;
                case "USDA_Tracking":
                  this.currForm = new InputFormInfo("USDA_Tracking", "Tracking");
                  break;
                case "VATool_BorrowerInfo":
                  this.currForm = new InputFormInfo("VATool_BorrowerInfo", "Borrower Info");
                  break;
                case "VATool_CashOutRefinance":
                  this.currForm = new InputFormInfo("VATool_CashOutRefinance", "Cash-Out Refinance");
                  break;
                case "VATool_Qualification":
                  this.currForm = new InputFormInfo("VATool_Qualification", "Qualification");
                  break;
                case "VATool_Tracking":
                  this.currForm = new InputFormInfo("VATool_Tracking", "Tracking");
                  break;
              }
              TabLinksControl worksheet4 = new TabLinksControl(this.session, this.currForm, (IWin32Window) Session.MainScreen, this.loan, false);
              worksheet4.LoadFormAndSetGoToField(this.currForm, this.fieldGoTo, this.countGoTo);
              this.editor.AddToWorkArea((Control) worksheet4, true);
            }
            if (!(this.specialInstruction != string.Empty))
              return;
            int num8 = (int) Utils.Dialog((IWin32Window) Session.MainForm, this.specialInstruction, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
        }
      }
    }

    private bool getVerifScreenObject(
      string targetFormID,
      bool isBorrower,
      int selectIndex,
      int blockNo)
    {
      this.emFormMenuBox.SelectedIndex = selectIndex;
      int num1 = 0;
      switch (targetFormID)
      {
        case "VOL":
          num1 = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
          break;
        case "VOE":
          num1 = this.loan.GetNumberOfEmployer(isBorrower);
          break;
        case "VOR":
          num1 = this.loan.GetNumberOfResidence(isBorrower);
          break;
        case "VOM":
          num1 = this.loan.GetNumberOfMortgages();
          break;
        case "VOD":
          num1 = this.loan.GetNumberOfDeposits();
          break;
      }
      object verifScreen = this.session.Application.GetService<ILoanEditor>().GetVerifScreen();
      if (verifScreen == null)
        return false;
      PanelBase panelBase = (PanelBase) verifScreen;
      if (num1 == 0 && blockNo == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) Session.MainForm, "You can find this field '" + this.fieldGoTo + " in the input form '" + targetFormID + "'. Currently you don't have a record in this input form.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (blockNo == 0 || blockNo > num1)
        blockNo = 1;
      panelBase.OpenVerification(blockNo - 1, this.fieldGoTo, this.countGoTo);
      return true;
    }

    private void setFormListIndexWithoutLoadingForm(string formNameInList)
    {
    }

    private void setFieldFocusInWindowsForm(string goToField, Control.ControlCollection cc)
    {
      foreach (Control control in (ArrangedElementCollection) cc)
      {
        switch (control)
        {
          case TextBox _:
          case ComboBox _:
          case CheckBox _:
            if (control.Tag is string && control.Tag.ToString() == goToField)
            {
              control.Focus();
              return;
            }
            continue;
          default:
            this.setFieldFocusInWindowsForm(goToField, control.Controls);
            continue;
        }
      }
    }

    private void addFormListForGoTo(
      ArrayList formList,
      ArrayList formNames,
      InputFormInfo currentForm,
      bool addTopList)
    {
      string empty = string.Empty;
      if (currentForm != (InputFormInfo) null)
      {
        bool flag = false;
        for (int index1 = 0; index1 < formNames.Count; ++index1)
        {
          string formName1 = (string) formNames[index1];
          if (formName1.IndexOf("----------") <= -1 && (formName1 == currentForm.Name || currentForm.Name.StartsWith("State Specific Information - ") && formName1 == "State-Specific Disclosure Information"))
          {
            flag = true;
            if (!addTopList)
            {
              for (int index2 = index1 + 1; index2 < formNames.Count; ++index2)
              {
                string formName2 = (string) formNames[index2];
                if (formName2.IndexOf("----------") <= -1)
                  formList.Add((object) formName2);
              }
              break;
            }
            for (int index3 = 0; index3 < index1; ++index3)
            {
              string formName3 = (string) formNames[index3];
              if (formName3.IndexOf("----------") <= -1)
                formList.Add((object) formName3);
            }
            break;
          }
        }
        if (flag)
          return;
        for (int index = 0; index < formNames.Count; ++index)
        {
          string formName = (string) formNames[index];
          if (formName.IndexOf("----------") <= -1)
            formList.Add((object) formName);
        }
      }
      else
      {
        for (int index = 0; index < formNames.Count; ++index)
        {
          string formName = (string) formNames[index];
          if (formName.IndexOf("----------") <= -1)
            formList.Add((object) formName);
        }
      }
    }

    private InputFormInfo[] getFormList(bool includeAll)
    {
      if (this.loanMgr == null)
        return new InputFormInfo[0];
      InputFormInfo[] formList = this.loanMgr.InputFormSettings.GetFormList("All");
      ArrayList arrayList1 = new ArrayList();
      foreach (InputFormInfo inputFormInfo in formList)
        arrayList1.Add((object) inputFormInfo.Name);
      InputFormInfo[] inputFormInfoArray = new InputFormInfo[0];
      if (!includeAll)
      {
        string[] formListTemplate = this.loan.GetFormListTemplate();
        if (formListTemplate.Length != 0)
        {
          ArrayList arrayList2 = new ArrayList();
          for (int index = 0; index < formListTemplate.Length; ++index)
          {
            if (!(formListTemplate[index] == "-"))
            {
              InputFormInfo formByName = this.loanMgr.InputFormSettings.GetFormByName(formListTemplate[index]);
              if (formByName != (InputFormInfo) null && arrayList1.Contains((object) formByName.Name))
                arrayList2.Add((object) formByName);
            }
          }
          inputFormInfoArray = (InputFormInfo[]) arrayList2.ToArray(typeof (InputFormInfo));
        }
        else
        {
          inputFormInfoArray = this.loanMgr.InputFormSettings.GetDefaultFormList();
          if (inputFormInfoArray.Length == 0)
            includeAll = true;
        }
      }
      if (includeAll)
        inputFormInfoArray = this.loanMgr.InputFormSettings.GetFormList("All");
      bool flag1 = this.session.LoanDataMgr.LoanData.GetField("NEWHUD.X354") == "Y";
      bool flag2 = Utils.CheckIf2015RespaTila(this.session.LoanDataMgr.LoanData.GetField("3969"));
      List<InputFormInfo> inputFormInfoList = new List<InputFormInfo>();
      foreach (InputFormInfo inputFormInfo in inputFormInfoArray)
      {
        if (!(inputFormInfo == (InputFormInfo) null) && !InputFormInfo.IsChildForm(inputFormInfo.FormID))
        {
          if (flag1 | flag2)
          {
            if (inputFormInfo.FormID == "REGZGFE" || inputFormInfo.FormID == "HUD1PG1" || inputFormInfo.FormID == "HUD1PG2")
              continue;
          }
          else if (inputFormInfo.FormID == "REGZGFEHUD" || inputFormInfo.FormID == "HUD1PG1_2010" || inputFormInfo.FormID == "HUD1PG2_2010" || inputFormInfo.FormID == "HUD1PG3_2010")
            continue;
          inputFormInfoList.Add(inputFormInfo);
        }
      }
      return inputFormInfoList.ToArray();
    }

    private void insertTabStyleChildForm(
      ArrayList gotoFormList,
      int formPosition,
      InputFormInfo currentChildform,
      string formID,
      ref bool insertToEndList)
    {
      string strB = formID;
      if (string.Compare(formID, "ULDD - Freddie Mac", true) == 0)
        strB = "ULDD_FreddieMac";
      else if (string.Compare(formID, "ULDD - Fannie Mae", true) == 0)
        strB = "ULDD_FannieMae";
      else if (string.Compare(formID, "ULDD - Ginnie Mae", true) == 0)
        strB = "ULDD_GinnieMae";
      if (currentChildform == (InputFormInfo) null || currentChildform != (InputFormInfo) null && string.Compare(currentChildform.FormID, strB, true) == 0 && !insertToEndList)
        insertToEndList = true;
      if (insertToEndList)
        gotoFormList.Insert(formPosition, (object) formID);
      else
        gotoFormList.Insert(0, (object) formID);
    }
  }
}

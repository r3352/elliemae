// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.D1003_2020P2InputHandler
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
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class D1003_2020P2InputHandler : InputHandlerBase
  {
    private List<EllieMae.Encompass.Forms.Panel> panelLocalAddresses;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignAddresses;
    private List<StandardButton> selectCountryButtons;
    private List<ImageButton> empDeleteButtons;
    private List<ImageButton> otherIncomeDeleteButtons;
    private string sourceEnable = string.Empty;
    private string sourceDisable = string.Empty;
    private Dictionary<string, bool> controlWithPermission = new Dictionary<string, bool>();

    public D1003_2020P2InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public D1003_2020P2InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public D1003_2020P2InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public D1003_2020P2InputHandler(
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
        case "2020baseincomeFE01":
        case "FE0119":
        case "militaryentFE01":
          controlState = !(this.inputData.GetField("FE0156") == "") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "2020baseincomeFE02":
        case "FE0219":
        case "militaryentFE02":
          controlState = !(this.inputData.GetField("FE0256") == "") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "2020baseincomeFE03":
        case "FE0319":
        case "militaryentFE03":
          controlState = !(this.inputData.GetField("FE0356") == "") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "2020baseincomeFE04":
        case "FE0419":
        case "militaryentFE04":
          controlState = !(this.inputData.GetField("FE0456") == "") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "FE0155":
          if (this.GetField("FE0115") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "FE0156":
          if (this.GetField("FE0115") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "FE0179":
          if (this.loan.GetField("FE0180") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "FE0255":
        case "FE0256":
          if (this.GetField("FE0215") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "FE0279":
          if (this.loan.GetField("FE0280") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "FE0355":
        case "FE0356":
          if (this.GetField("FE0315") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "FE0379":
          if (this.loan.GetField("FE0380") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "FE0455":
        case "FE0456":
          if (this.GetField("FE0415") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "FE0479":
          if (this.loan.GetField("FE0480") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "FE0579":
          if (this.loan.GetField("FE0580") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "FE0679":
          if (this.loan.GetField("FE0680") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X199":
          controlState = !this.VerifExists("CurrentBorrowerEmployer") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "URLA.X200":
          controlState = !this.VerifExists("CurrentCoBorrowerEmployer") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "URLA.X201":
          controlState = !this.VerifExists("AdditionalBorrowerEmployer") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "URLA.X202":
          controlState = !this.VerifExists("AdditionalCoBorrowerEmployer") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "URLA.X203":
          controlState = !this.VerifExists("PriorBorrowerEmployer") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "URLA.X204":
          controlState = !this.VerifExists("PriorCoBorrowerEmployer") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "URLA.X40":
          controlState = !this.VerifExists("BorrowerOtherIncome") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "URLA.X41":
          controlState = !this.VerifExists("CoBorrowerOtherIncome") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "voe":
        case "vooi":
          if (this.FormIsForTemplate)
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

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.selectCountryButtons != null && this.selectCountryButtons.Count > 0)
      {
        for (int index = 0; index < this.selectCountryButtons.Count; ++index)
        {
          if (this.selectCountryButtons[index] != null)
          {
            bool flag = this.GetField("FE0" + (object) (index + 1) + "80") == "Y";
            this.selectCountryButtons[index].Visible = flag;
            this.panelLocalAddresses[index].Visible = !flag;
            this.panelForeignAddresses[index].Visible = flag;
          }
        }
      }
      if (this.loan != null && this.loan.IsInFindFieldForm)
      {
        for (int index = 0; index < this.empDeleteButtons.Count; index += 2)
          this.empDeleteButtons[index].Source = this.sourceEnable;
        for (int index = 0; index < this.otherIncomeDeleteButtons.Count; index += 2)
          this.otherIncomeDeleteButtons[index].Source = this.sourceEnable;
      }
      else
      {
        if (this.empDeleteButtons != null && this.empDeleteButtons.Count >= 12)
        {
          this.EnableDisableControl(this.empDeleteButtons[0], this.controlWithPermission, this.VerifExists("CurrentBorrowerEmployer"), this.sourceEnable, this.sourceDisable);
          this.EnableDisableControl(this.empDeleteButtons[2], this.controlWithPermission, this.VerifExists("CurrentCoBorrowerEmployer"), this.sourceEnable, this.sourceDisable);
          this.EnableDisableControl(this.empDeleteButtons[4], this.controlWithPermission, this.VerifExists("AdditionalBorrowerEmployer"), this.sourceEnable, this.sourceDisable);
          this.EnableDisableControl(this.empDeleteButtons[6], this.controlWithPermission, this.VerifExists("AdditionalCoBorrowerEmployer"), this.sourceEnable, this.sourceDisable);
          this.EnableDisableControl(this.empDeleteButtons[8], this.controlWithPermission, this.VerifExists("PriorBorrowerEmployer"), this.sourceEnable, this.sourceDisable);
          this.EnableDisableControl(this.empDeleteButtons[10], this.controlWithPermission, this.VerifExists("PriorCoBorrowerEmployer"), this.sourceEnable, this.sourceDisable);
        }
        if (this.otherIncomeDeleteButtons == null || this.otherIncomeDeleteButtons.Count < 6)
          return;
        this.SwitchImageControl(this.otherIncomeDeleteButtons, 4, this.loan.GetNumberOfOtherIncomeSources());
      }
    }

    internal void SwitchImageControl(List<ImageButton> imageButton, int totalIndex, int totalCount)
    {
      int num = 1;
      for (int index = 0; index <= totalIndex; index += 2)
      {
        bool result = totalCount >= num++;
        this.EnableDisableControl(imageButton[index], this.controlWithPermission, result, this.sourceEnable, this.sourceDisable);
      }
    }

    internal override void CreateControls()
    {
      try
      {
        this.panelLocalAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        for (int index = 1; index <= 6; ++index)
        {
          this.panelLocalAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("localAdr" + (object) index));
          this.panelForeignAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignAdr" + (object) index));
          if (this.panelForeignAddresses[index - 1] != null && this.panelLocalAddresses[index - 1] != null)
            this.panelForeignAddresses[index - 1].Position = this.panelLocalAddresses[index - 1].Position;
        }
        this.selectCountryButtons = new List<StandardButton>();
        for (int index = 1; index <= 6; ++index)
          this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_FE0" + (object) index + "79"));
        this.empDeleteButtons = new List<ImageButton>();
        this.empDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Ibceborr"));
        this.empDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisIbceborr"));
        this.empDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Ibcecoborr"));
        this.empDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisIbcecoborr"));
        this.empDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Ibaeborr"));
        this.empDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisIbaeborr"));
        this.empDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Ibaecoborr"));
        this.empDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisIbaecoborr"));
        this.empDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Ibpeborr"));
        this.empDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisIbpeborr"));
        this.empDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Ibpecoborr"));
        this.empDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisIbpecoborr"));
        this.otherIncomeDeleteButtons = new List<ImageButton>();
        this.otherIncomeDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Iboiborr1"));
        this.otherIncomeDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisIboiborr1"));
        this.otherIncomeDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Iboiborr2"));
        this.otherIncomeDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisIboiborr2"));
        this.otherIncomeDeleteButtons.Add((ImageButton) this.currentForm.FindControl("Iboiborr3"));
        this.otherIncomeDeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisIboiborr3"));
        if (this.empDeleteButtons.Count >= 2)
        {
          this.sourceEnable = this.empDeleteButtons[0].Source;
          this.sourceDisable = this.empDeleteButtons[1].Source;
        }
        string empty = string.Empty;
        if (this.empDeleteButtons.Count >= 12)
        {
          for (int index = 0; index < 12; index += 2)
          {
            BizRule.FieldAccessRight fieldAccessRights = this.session.LoanDataMgr.GetFieldAccessRights("Button_" + this.empDeleteButtons[index].Action);
            this.controlWithPermission.Add(this.empDeleteButtons[index].Action, fieldAccessRights == BizRule.FieldAccessRight.ViewOnly);
          }
        }
        if (this.otherIncomeDeleteButtons.Count < 6)
          return;
        for (int index = 0; index < 6; index += 2)
        {
          BizRule.FieldAccessRight fieldAccessRights = this.session.LoanDataMgr.GetFieldAccessRights("Button_" + this.otherIncomeDeleteButtons[index].Action);
          this.controlWithPermission.Add(this.otherIncomeDeleteButtons[index].Action, fieldAccessRights == BizRule.FieldAccessRight.ViewOnly);
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void RemoveEmployment(string type)
    {
      if (Utils.Dialog((IWin32Window) this.session.MainForm, "Are you sure you want to delete this record from the verification?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK || !type.EndsWith("Employer"))
        return;
      string str = type.Contains("CoBorrower") ? "CE" : "BE";
      bool borrower = str == "BE";
      int numberOfEmployer = this.loan.GetNumberOfEmployer(borrower);
      int num1 = 0;
      int num2 = -1;
      for (int index = 1; index <= numberOfEmployer; ++index)
      {
        string field1 = this.inputData.GetField(str + index.ToString("00") + "08");
        string field2 = this.inputData.GetField(str + index.ToString("00") + "09");
        if (field2 == "N" && type.Contains("Prior"))
        {
          switch (field1)
          {
            case "Borrower":
              num2 = index;
              goto label_17;
            case "CoBorrower":
              num2 = index;
              goto label_17;
            default:
              continue;
          }
        }
        else if (field2 == "Y" && !type.Contains("Prior"))
        {
          ++num1;
          if (num1 == 1 && !type.Contains("Additional"))
          {
            switch (field1)
            {
              case "Borrower":
                num2 = index;
                goto label_17;
              case "CoBorrower":
                num2 = index;
                goto label_17;
              default:
                continue;
            }
          }
          else if (num1 > 1 && type.Contains("Additional"))
          {
            switch (field1)
            {
              case "Borrower":
                num2 = index;
                goto label_17;
              case "CoBorrower":
                num2 = index;
                goto label_17;
              default:
                continue;
            }
          }
        }
      }
label_17:
      if (num2 != -1)
        this.loan.RemoveEmployerAt(borrower, num2 - 1);
      this.RefreshContents();
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
      {
        case 816176745:
          if (!(action == "otherf"))
            return;
          this.SetFieldFocus("l_229");
          return;
        case 1001926223:
          if (!(action == "1stmor"))
            return;
          this.SetFieldFocus("l_228");
          return;
        case 1129727815:
          if (!(action == "Ibcecoborr"))
            return;
          this.RemoveEmployment("CurrentCoBorrowerEmployer");
          return;
        case 1163987084:
          if (!(action == "mortg"))
            return;
          if (this.inputData.IsLocked("232"))
          {
            this.SetFieldFocus("l_232");
            return;
          }
          this.SetFieldFocus("l_233");
          return;
        case 1278929227:
          if (!(action == "baseincome"))
            return;
          this.SetFieldFocus("l_101");
          return;
        case 1415834480:
          if (!(action == "Ibpecoborr"))
            return;
          this.RemoveEmployment("PriorCoBorrowerEmployer");
          return;
        case 1497812540:
          if (!(action == "importliab"))
            return;
          this.SetFieldFocus("l_fl0102");
          return;
        case 2816711163:
          if (!(action == "prequal_vod"))
            return;
          this.loan.Calculator.FormCalculation("LOANCOMP", (string) null, (string) null);
          this.editor.RefreshContents();
          this.SetFieldFocus("l_1604");
          return;
        case 3070297505:
          if (!(action == "Ibaecoborr"))
            return;
          this.RemoveEmployment("AdditionalCoBorrowerEmployer");
          return;
        case 3133684231:
          if (!(action == "Ibaeborr"))
            return;
          this.RemoveEmployment("AdditionalBorrowerEmployer");
          return;
        case 3363671541:
          if (!(action == "other"))
            return;
          this.SetFieldFocus("l_126");
          return;
        case 3402438956:
          if (!(action == "Iboiborr1"))
            return;
          break;
        case 3419216575:
          if (!(action == "Iboiborr0"))
            return;
          break;
        case 3452771813:
          if (!(action == "Iboiborr2"))
            return;
          break;
        case 3550454798:
          if (!(action == "Ibpeborr"))
            return;
          this.RemoveEmployment("PriorBorrowerEmployer");
          return;
        case 3880240577:
          if (!(action == "retaxes"))
            return;
          this.SetFieldFocus("l_1405");
          return;
        case 4106164968:
          if (!(action == "haz"))
            return;
          this.SetFieldFocus("l_230");
          return;
        case 4189782957:
          if (!(action == "Ibceborr"))
            return;
          this.RemoveEmployment("CurrentBorrowerEmployer");
          return;
        default:
          return;
      }
      this.RemoveOtherIncome(int.Parse(action.Substring(8)));
    }

    private void RemoveOtherIncome(int index)
    {
      if (Utils.Dialog((IWin32Window) this.session.MainForm, "Are you sure you want to delete this record from the verification?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      this.loan.RemoveOtherIncomeSourceAt(index);
      this.RefreshContents();
    }

    private bool VerifExists(string type)
    {
      if (type.EndsWith("Employer"))
      {
        string str = type.Contains("CoBorrower") ? "CE" : "BE";
        int numberOfEmployer = this.loan.GetNumberOfEmployer(str == "BE");
        int num = 0;
        for (int index = 1; index <= numberOfEmployer; ++index)
        {
          string field1 = this.inputData.GetField(str + index.ToString("00") + "08");
          string field2 = this.inputData.GetField(str + index.ToString("00") + "09");
          if (field2 == "N" && type.Contains("Prior"))
          {
            switch (field1)
            {
              case "Borrower":
                return true;
              case "CoBorrower":
                return true;
              default:
                continue;
            }
          }
          else if (field2 == "Y" && !type.Contains("Prior"))
          {
            ++num;
            if (num == 1 && !type.Contains("Additional"))
            {
              switch (field1)
              {
                case "Borrower":
                  return true;
                case "CoBorrower":
                  return true;
                default:
                  continue;
              }
            }
            else if (num > 1 && type.Contains("Additional"))
            {
              switch (field1)
              {
                case "Borrower":
                  return true;
                case "CoBorrower":
                  return true;
                default:
                  continue;
              }
            }
          }
        }
      }
      else if (type.EndsWith("OtherIncome"))
      {
        int otherIncomeSources = this.loan.GetNumberOfOtherIncomeSources();
        bool flag = !type.Contains("CoBorrower");
        for (int index = 1; index <= otherIncomeSources; ++index)
        {
          string field = this.loan.GetField("URLAROIS" + index.ToString("00") + "02");
          if (field == "Borrower" & flag || field == "CoBorrower" && !flag)
            return true;
        }
      }
      return false;
    }
  }
}

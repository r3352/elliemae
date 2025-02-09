// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PREQUALInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.Forms;
using mshtml;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class PREQUALInputHandler : InputHandlerBase
  {
    public PREQUALInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public PREQUALInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
      this.refreshStopLight();
    }

    public PREQUALInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public PREQUALInputHandler(
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
      ControlState defaultValue = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "101":
        case "102":
        case "103":
        case "104":
        case "107":
        case "108":
        case "110":
        case "111":
        case "112":
        case "113":
        case "116":
        case "117":
          return defaultValue;
        case "140":
          defaultValue = this.GetSpecialControlStatus(id, defaultValue);
          goto case "101";
        case "142":
          if (this.currentForm.FindControl("lblCashFromTo") is EllieMae.Encompass.Forms.Label control)
            control.Text = this.ToDouble(this.loan.GetSimpleField("142")) <= 0.0 ? "Cash to borrower" : "Cash from borrower";
          defaultValue = ControlState.Default;
          goto case "101";
        case "baseincome":
          if (this.GetField("1825") == "2020")
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "ccprog":
        case "loanprog":
          if (this.FormIsForTemplate)
          {
            defaultValue = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        default:
          defaultValue = ControlState.Default;
          goto case "101";
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id == "2849" || id == "2850" || id == "1484" || id == "1502")
      {
        string str1 = "";
        string str2 = "0123456789";
        for (int startIndex = 0; startIndex < val.Length; ++startIndex)
        {
          string str3 = val.Substring(startIndex, 1);
          if (str2.IndexOf(str3) > -1)
            str1 += str3;
        }
        val = str1;
      }
      base.UpdateFieldValue(id, val);
      this.loan.Calculator.FormCalculation("LOANCOMP", id, val);
      this.refreshStopLight();
    }

    internal override void FlipLockField(FieldLock fieldLock)
    {
      base.FlipLockField(fieldLock);
      if (fieldLock.Locked || this.loan == null)
        return;
      this.loan.Calculator.FormCalculation("LOANCOMP", (string) null, (string) null);
      this.refreshStopLight();
    }

    public override void ExecAction(string action)
    {
      if (action != "loanprog" && action != "ccprog")
        base.ExecAction(action);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
      {
        case 89800592:
          if (!(action == "clearcc"))
            break;
          this.UpdateFieldValue("1785", "");
          this.UpdateFieldValue("2862", "");
          this.UpdateContents();
          this.SetFieldFocus("l_356");
          break;
        case 108696782:
          if (!(action == "clearlp"))
            break;
          this.UpdateFieldValue("1401", "");
          this.UpdateFieldValue("2861", "");
          this.UpdateContents();
          this.SetFieldFocus("l_356");
          break;
        case 246033821:
          if (!(action == "basecost"))
            break;
          this.refreshStopLight();
          this.SetFieldFocus("l_140");
          break;
        case 371019337:
          if (!(action == "heoblig"))
            break;
          this.SetFieldFocus("l_140");
          break;
        case 375884332:
          int num = action == "detail0" ? 1 : 0;
          break;
        case 382748654:
          if (!(action == "cashflow"))
            break;
          this.SetFieldFocus("l_115");
          break;
        case 720381155:
          if (!(action == "primaryexpense"))
            break;
          this.refreshStopLight();
          this.SetFieldFocus("l_205");
          break;
        case 787192126:
          if (!(action == "closingcosts"))
            break;
          this.refreshStopLight();
          this.SetFieldFocus("l_140");
          break;
        case 816176745:
          if (!(action == "otherf"))
            break;
          this.SetFieldFocus("l_229");
          break;
        case 1001926223:
          if (!(action == "1stmor"))
            break;
          this.SetFieldFocus("l_228");
          break;
        case 1027771259:
          if (!(action == "maxloanamt"))
            break;
          this.loan.Calculator.CalculateMaxLoanAmt();
          this.loan.Calculator.FormCalculation("LOANCOMP", (string) null, (string) null);
          this.refreshStopLight();
          this.UpdateContents();
          this.SetFieldFocus("l_205");
          break;
        case 1163987084:
          if (!(action == "mortg"))
            break;
          this.SetFieldFocus("l_232");
          break;
        case 1195168357:
          if (!(action == "allotherpayments"))
            break;
          this.refreshStopLight();
          this.SetFieldFocus("l_205");
          break;
        case 1278929227:
          if (!(action == "baseincome"))
            break;
          this.SetFieldFocus("l_101");
          break;
        case 1611142665:
          if (!(action == "totalcosts"))
            break;
          this.refreshStopLight();
          this.SetFieldFocus("l_140");
          break;
        case 2161429978:
          if (!(action == "taxes"))
            break;
          this.SetFieldFocus("l_1405");
          break;
        case 2668797518:
          if (!(action == "otherincome"))
            break;
          this.SetFieldFocus("l_1818");
          break;
        case 3220010881:
          if (!(action == "cashavailable"))
            break;
          this.refreshStopLight();
          this.SetFieldFocus("l_140");
          break;
        case 3248398873:
          if (!(action == "payoffmortgages"))
            break;
          this.refreshStopLight();
          this.SetFieldFocus("l_140");
          break;
        case 3363671541:
          if (!(action == "other"))
            break;
          this.SetFieldFocus("l_126");
          break;
        case 3443531722:
          if (!(action == "subfin"))
            break;
          this.SetFieldFocus("l_140");
          break;
        case 3540010302:
          if (!(action == "totalfinancing"))
            break;
          this.refreshStopLight();
          this.SetFieldFocus("l_140");
          break;
        case 3582764703:
          if (!(action == "loanprog"))
            break;
          if (new LoanProgramSelect(this.loan, this.session).ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
          {
            this.loan.Calculator.CalculateMaxLoanAmt();
            this.loan.Calculator.FormCalculation("LOANCOMP", (string) null, (string) null);
            this.refreshStopLight();
            this.UpdateContents();
          }
          this.SetFieldFocus("l_356");
          break;
        case 3799877848:
          if (!(action == "copymaxpropvalue"))
            break;
          this.UpdateFieldValue("136", this.GetFieldValue("PREQUAL.X204"));
          this.UpdateFieldValue("1335", this.GetFieldValue("PREQUAL.X203"));
          this.UpdateContents();
          this.SetFieldFocus("l_356");
          break;
        case 3907966781:
          if (!(action == "regz"))
            break;
          this.refreshStopLight();
          this.SetFieldFocus("l_1853");
          break;
        case 4061023159:
          if (!(action == "otherpayoffs"))
            break;
          this.refreshStopLight();
          this.SetFieldFocus("l_140");
          break;
        case 4106164968:
          if (!(action == "haz"))
            break;
          this.SetFieldFocus("l_230");
          break;
        case 4155076599:
          if (!(action == "ccprog"))
            break;
          if (new ClosingCostSelect(this.loan).ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
          {
            this.loan.Calculator.CalculateAll();
            this.loan.Calculator.FormCalculation("LOANCOMP", (string) null, (string) null);
            this.refreshStopLight();
            this.UpdateContents();
          }
          this.SetFieldFocus("l_356");
          break;
      }
    }

    private void refreshStopLight()
    {
      if (!(this.currentForm.FindControl("imgLight") is ImageButton control))
        return;
      switch (this.loan.GetSimpleField("PREQUAL.X274"))
      {
        case "Green":
          control.Source = AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.FormRelDir, "images\\LightGreen.gif"), SystemSettings.LocalAppDir);
          break;
        case "Red":
          control.Source = AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.FormRelDir, "images\\LightRed.gif"), SystemSettings.LocalAppDir);
          break;
        default:
          control.Source = AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.FormRelDir, "images\\LightYellow.gif"), SystemSettings.LocalAppDir);
          break;
      }
    }
  }
}

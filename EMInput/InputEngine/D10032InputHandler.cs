// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.D10032InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class D10032InputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.GroupBox grpOther2009;
    private EllieMae.Encompass.Forms.GroupBox grpOther2020;
    private EllieMae.Encompass.Forms.Panel panel_2009;
    private EllieMae.Encompass.Forms.Panel panel_2020;
    private EllieMae.Encompass.Forms.Panel pnlOl2009;
    private EllieMae.Encompass.Forms.Panel pnlOl2020;
    private List<RuntimeControl> panel_Exp_Controls;

    public D10032InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public D10032InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public D10032InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public D10032InputHandler(
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
        base.CreateControls();
        this.panel_2009 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panel_2009");
        this.panel_2020 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panel_2020");
        if (this.panel_2020 != null)
          this.panel_2020.Top = 54;
        this.grpOther2009 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("grpOther2009");
        this.grpOther2020 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("grpOther2020");
        if (this.pnlOl2009 == null)
          this.pnlOl2009 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnl2009");
        if (this.pnlOl2020 == null)
          this.pnlOl2020 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnl2020");
        if (this.GetField("1825") == "2020")
        {
          if (this.grpOther2009 != null)
            this.grpOther2009.Visible = false;
          if (this.grpOther2020 != null)
            this.grpOther2020.Visible = true;
          if (this.panel_2009 != null)
            this.panel_2009.Visible = false;
          if (this.panel_2020 != null)
            this.panel_2020.Visible = true;
          if (this.pnlOl2009 != null && this.pnlOl2020 != null)
            this.pnlOl2009.Visible = !(this.pnlOl2020.Visible = true);
        }
        else
        {
          if (this.grpOther2009 != null)
            this.grpOther2009.Visible = true;
          if (this.grpOther2020 != null)
            this.grpOther2020.Visible = false;
          if (this.panel_2009 != null)
            this.panel_2009.Visible = true;
          if (this.panel_2020 != null)
            this.panel_2020.Visible = false;
          if (this.pnlOl2009 != null && this.pnlOl2020 != null)
            this.pnlOl2009.Visible = !(this.pnlOl2020.Visible = false);
        }
        this.panel_Exp_Controls = SharedURLAUIHandler.GetControls(this.currentForm, "panel_Exp_2009", "panel_Exp_2020");
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
        case "1405":
        case "FE0104":
        case "FE0204":
          return controlState;
        case "1stmor":
          string field1 = this.loan.GetField("420");
          if (!(field1 == "SecondLien") && !(field1 == "Other"))
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "baseincome":
          if (this.GetField("1825") == "2020")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "importliab":
        case "ocredit":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
          {
            if (!new eFolderAccessRights(Session.LoanDataMgr).CanRequestServices)
              controlState = ControlState.Disabled;
            ((EllieMae.Encompass.Forms.Button) ctrl).HoverText = controlState != ControlState.Disabled ? "" : "You must have Document Tracking permission to use this feature.";
            goto case "101";
          }
        case "otherf":
          string field2 = this.loan.GetField("420");
          if (!(field2 == "FirstLien") && !(field2 == string.Empty))
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "vcredit":
        case "vod":
        case "voe":
        case "vol":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        default:
          controlState = ControlState.Default;
          goto case "101";
      }
    }

    protected override string FormatValue(string fieldId, string val)
    {
      if (!(fieldId == "144") && !(fieldId == "147") && !(fieldId == "150"))
        return base.FormatValue(fieldId, val);
      if (val.Length > 1)
        val = val.Substring(0, 1);
      val = val.ToUpper();
      if (val != "C" && val != "B")
        val = string.Empty;
      return val;
    }

    internal override void UpdateFieldValue(string fieldId, string val)
    {
      if ((fieldId == "144" || fieldId == "147" || fieldId == "150") && val == string.Empty)
      {
        int num = int.Parse(fieldId);
        this.loan.SetField((num + 1).ToString(), string.Empty);
        this.loan.SetField((num + 2).ToString(), string.Empty);
        this.loan.SetField(fieldId, val);
      }
      else
      {
        switch (fieldId)
        {
          case "122":
          case "123":
          case "124":
          case "125":
          case "1405":
          case "230":
          case "232":
          case "233":
            double num = this.ToDouble(val);
            if (num != 0.0)
            {
              val = num.ToString("N2");
              break;
            }
            break;
        }
        if (fieldId == "229" && Math.Round(Utils.ParseDouble((object) val), 2) != Math.Round(Utils.ParseDouble((object) this.GetFieldValue("229")), 2))
        {
          base.UpdateFieldValue("INTRATE2", "");
          base.UpdateFieldValue("TERM2", "");
        }
        base.UpdateFieldValue(fieldId, val);
      }
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      SharedURLAUIHandler.DisplayControls(this.loan.Use2020URLA, this.panel_Exp_Controls);
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
      {
        case 816176745:
          if (!(action == "otherf"))
            break;
          this.SetFieldFocus(this.GetField("1825") == "2020" ? "l_229_2020" : "l_229");
          break;
        case 1001926223:
          if (!(action == "1stmor"))
            break;
          this.SetFieldFocus(this.GetField("1825") == "2020" ? "l_228_2020" : "l_228");
          break;
        case 1163987084:
          if (!(action == "mortg"))
            break;
          if (this.inputData.IsLocked("232"))
          {
            this.SetFieldFocus(this.GetField("1825") == "2020" ? "l_232_2020" : "l_232");
            break;
          }
          this.SetFieldFocus(this.GetField("1825") == "2020" ? "l_233_2020" : "l_233");
          break;
        case 1278929227:
          if (!(action == "baseincome"))
            break;
          this.SetFieldFocus("l_101");
          break;
        case 1497812540:
          if (!(action == "importliab"))
            break;
          this.SetFieldFocus("l_fl0102");
          break;
        case 1782128422:
          if (!(action == "editsupplementalpropertyinsurance"))
            break;
          if (this.inputData.IsLocked("URLA.X144"))
          {
            this.SetFieldFocus("l_URLAX144");
            break;
          }
          this.SetFieldFocus("l_1405_2020");
          break;
        case 2816711163:
          if (!(action == "prequal_vod"))
            break;
          this.loan.Calculator.FormCalculation("LOANCOMP", (string) null, (string) null);
          this.editor.RefreshContents();
          this.SetFieldFocus("l_1604");
          break;
        case 3363671541:
          if (!(action == "other"))
            break;
          this.SetFieldFocus("l_126");
          break;
        case 3880240577:
          if (!(action == "retaxes"))
            break;
          this.SetFieldFocus(this.GetField("1825") == "2020" ? "l_1405_2020" : "l_1405");
          break;
        case 4106164968:
          if (!(action == "haz"))
            break;
          this.SetFieldFocus(this.GetField("1825") == "2020" ? "l_230_2020" : "l_230");
          break;
      }
    }
  }
}

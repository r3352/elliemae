// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VOEInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class VOEInputHandler : VORInputHandler
  {
    private EllieMae.Encompass.Forms.Panel pnlForm;
    private EllieMae.Encompass.Forms.GroupBox grpFrom;
    private EllieMae.Encompass.Forms.GroupBox grpEmpInfo;
    private EllieMae.Encompass.Forms.GroupBox grpEmployment;
    private EllieMae.Encompass.Forms.GroupBox grpGrossIncome;
    private EllieMae.Encompass.Forms.Panel pnlUnitType;
    private EllieMae.Encompass.Forms.Panel pnlOldAddress;
    private EllieMae.Encompass.Forms.Panel pnlCityState;
    private EllieMae.Encompass.Forms.Panel pnlCountry;
    private EllieMae.Encompass.Forms.Panel pnlEmpComments;
    private EllieMae.Encompass.Forms.Panel pnlIndOrMilitary;
    private EllieMae.Encompass.Forms.Panel pnlBusOwned;
    private StandardButton btnCalcBasePay;
    private StandardButton btnMilitaryEnt;
    private EllieMae.Encompass.Forms.Panel pnlMilitary;
    private EllieMae.Encompass.Forms.Panel pnlOtherTotal;
    private EllieMae.Encompass.Forms.Panel pnlOwnership;
    private EllieMae.Encompass.Forms.Label lblVOE;
    private VerticalRule verRule;
    private EllieMae.Encompass.Forms.Panel urla2009StartDate;
    private EllieMae.Encompass.Forms.Panel urla2020StartDate;
    private EllieMae.Encompass.Forms.Panel pnllocalAdr;
    private EllieMae.Encompass.Forms.Panel pnlforeignAdr;
    private StandardButton selectCountryButton;
    private EllieMae.Encompass.Forms.Panel pnlForeignCountry;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignPanels;

    public VOEInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public VOEInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public VOEInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public VOEInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, owner, dataTemplate, htmldoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      try
      {
        this.pnlForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        this.grpFrom = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox2");
        this.grpEmpInfo = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox3");
        this.grpEmployment = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox4");
        this.grpGrossIncome = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox5");
        this.pnlUnitType = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlUnitType");
        this.pnlOldAddress = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlOldAddress");
        this.pnlCityState = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlCityState");
        this.pnlCountry = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlCountry");
        this.pnlEmpComments = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlEmpComments");
        this.pnlIndOrMilitary = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlIndOrMilitary");
        this.pnlBusOwned = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlBusOwned");
        this.btnCalcBasePay = (StandardButton) this.currentForm.FindControl("btnCalcBasePay");
        this.btnMilitaryEnt = (StandardButton) this.currentForm.FindControl("btnCalcMilitary");
        this.pnlMilitary = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlMilitary");
        this.pnlOtherTotal = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlOtherTotal");
        this.pnlOwnership = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlOwnership");
        this.lblVOE = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label18");
        this.verRule = (VerticalRule) this.currentForm.FindControl("VerticalRule1");
        this.urla2009StartDate = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("URLA2009StartDate");
        this.urla2020StartDate = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("URLA2020StartDate");
        this.pnllocalAdr = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("localAdr");
        this.pnlforeignAdr = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignAdr");
        this.selectCountryButton = (StandardButton) this.currentForm.FindControl("selectcountry_FE0079");
        this.pnlForeignCountry = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignPanel2");
        if (this.pnlforeignAdr != null && this.pnllocalAdr != null)
          this.pnlforeignAdr.Position = this.pnllocalAdr.Position;
        this.panelForeignPanels = new List<EllieMae.Encompass.Forms.Panel>();
        for (int index = 1; index <= 2; ++index)
          this.panelForeignPanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignPanel" + (object) index));
        bool flag;
        if (this.loan.GetField("1825") == "2020")
        {
          flag = true;
          if (this.ind != null)
          {
            this.btnCalcBasePay.Action = "2020baseincome" + this.ind;
            this.btnMilitaryEnt.Action = "militaryent" + this.ind;
          }
        }
        else
        {
          this.pnlforeignAdr.Visible = false;
          flag = false;
          this.pnlOldAddress.Position = this.pnlUnitType.Position;
          this.pnllocalAdr.Position = new Point(0, this.pnlUnitType.Position.Y + 22);
          this.pnlEmpComments.Position = new Point(0, this.pnlCityState.Position.Y + 42);
          this.pnlForeignCountry.Top = this.pnlCityState.Top + this.pnlCityState.Size.Height;
          this.pnlOtherTotal.Position = this.pnlMilitary.Position;
          EllieMae.Encompass.Forms.GroupBox grpFrom = this.grpFrom;
          Point position = this.grpFrom.Position;
          int x1 = position.X;
          position = this.grpFrom.Position;
          int y1 = position.Y - 26;
          Point point1 = new Point(x1, y1);
          grpFrom.Position = point1;
          EllieMae.Encompass.Forms.GroupBox grpEmpInfo = this.grpEmpInfo;
          position = this.grpEmpInfo.Position;
          int x2 = position.X;
          position = this.grpFrom.Position;
          int y2 = position.Y + this.grpFrom.Size.Height;
          Point point2 = new Point(x2, y2);
          grpEmpInfo.Position = point2;
          EllieMae.Encompass.Forms.GroupBox grpEmployment = this.grpEmployment;
          position = this.grpEmployment.Position;
          int x3 = position.X;
          position = this.grpEmpInfo.Position;
          int y3 = position.Y + this.grpEmpInfo.Size.Height;
          Point point3 = new Point(x3, y3);
          grpEmployment.Position = point3;
          EllieMae.Encompass.Forms.GroupBox grpGrossIncome = this.grpGrossIncome;
          position = this.grpGrossIncome.Position;
          int x4 = position.X;
          position = this.grpEmployment.Position;
          int y4 = position.Y;
          Point point4 = new Point(x4, y4);
          grpGrossIncome.Position = point4;
          VerticalRule verRule = this.verRule;
          position = this.verRule.Position;
          int x5 = position.X;
          position = this.grpGrossIncome.Position;
          int y5 = position.Y;
          Point point5 = new Point(x5, y5);
          verRule.Position = point5;
          this.pnlForm.Size = new Size(this.pnlForm.Size.Width, this.pnlForm.Size.Height - 230);
          this.lblVOE.Position = new Point(1, this.pnlForm.Size.Height + 6);
          this.urla2009StartDate.Position = this.urla2020StartDate.Position;
        }
        this.pnlUnitType.Visible = flag;
        this.pnlOldAddress.Visible = !flag;
        this.pnlCountry.Visible = flag;
        this.pnlIndOrMilitary.Visible = flag;
        this.pnlBusOwned.Visible = !flag;
        this.btnCalcBasePay.Visible = flag;
        this.pnlMilitary.Visible = flag;
        this.pnlOwnership.Visible = flag;
        this.urla2009StartDate.Visible = !flag;
        this.urla2020StartDate.Visible = flag;
      }
      catch (Exception ex)
      {
      }
    }

    public override void ExecAction(string action)
    {
      if (action.ToLower().StartsWith("selectcountry_"))
        action = "selectcountry_" + this.MapFieldId(action.Replace("selectcountry_", ""));
      base.ExecAction(action);
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.selectCountryButton != null && this.loan.GetField("1825") == "2020")
      {
        bool flag = this.GetField(this.ind + "80") == "Y";
        this.selectCountryButton.Visible = flag;
        this.pnllocalAdr.Visible = !flag;
        this.pnlforeignAdr.Visible = flag;
      }
      if (this.GetField("1825") == "2020")
      {
        for (int index = 0; index < 2; ++index)
          this.panelForeignPanels[index].Visible = true;
      }
      else
      {
        for (int index = 0; index < 2; ++index)
          this.panelForeignPanels[index].Visible = false;
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id.Length > 4)
      {
        string str1 = id.Substring(0, 2);
        string str2 = id.Substring(4);
        if (id.Length > 6)
          str2 = id.Substring(5);
        if (str1 == "BE" || str1 == "CE")
        {
          if (str2 == "27" && Utils.ParseDouble((object) val) > 100.0)
            val = "100.000";
          else if (str2 == "14" || str2 == "11")
          {
            DateTime date1 = Utils.ParseDate(str2 == "11" ? (object) val : (object) this.GetFieldValue(id.Substring(0, 4) + "11"));
            DateTime date2 = Utils.ParseDate(str2 == "14" ? (object) val : (object) this.GetFieldValue(id.Substring(0, 4) + "14"));
            if (date1 != DateTime.MinValue && date2 != DateTime.MinValue && date1 > date2)
            {
              int num = (int) Utils.Dialog((IWin32Window) null, "The date range between Date Hired and Date Terminated is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              val = string.Empty;
            }
          }
        }
        this.SetPrintingDefault(id, val);
      }
      base.UpdateFieldValue(id, val);
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      string str1 = "0";
      string str2 = "";
      if (id.StartsWith("2020base"))
      {
        str1 = "2020baseincome";
        str2 = id.Substring(14);
      }
      else if (id.StartsWith("militaryent"))
      {
        str1 = "militaryent";
        str2 = id.Substring(11);
      }
      else if (id.Length > 6)
      {
        str1 = id.Substring(5, 2);
        str2 = id.Substring(0, 5);
      }
      else if (id.Length > 5)
      {
        str1 = id.Substring(4, 2);
        str2 = id.Substring(0, 4);
      }
      switch (str1)
      {
        case "19":
        case "2020baseincome":
        case "militaryent":
          controlState = !(this.loan.GetSimpleField(str2 + "56") == "") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "37":
          if (this.loan.GetSimpleField(str2 + "38") == "Y" || this.loan.GetSimpleField(str2 + "64") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "55":
        case "56":
          controlState = !(this.loan.GetSimpleField(str2 + "15") != "Y") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "79":
          if (this.loan.GetSimpleField(str2 + "80") != "Y")
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

    protected override void InitIds()
    {
      this.brwHeader = "BE";
      this.coBrwHeader = "CE";
      this.header = "FE";
      this.brwId = "08";
      this.IdId = "99";
      this.nameId = "02";
      this.title = "VOE";
      if (!(this.loan.GetField("1825") == "2020") || this.ind == null)
        return;
      if (this.btnCalcBasePay != null)
        this.btnCalcBasePay.Action = "2020baseincome" + this.ind;
      if (this.btnMilitaryEnt == null)
        return;
      this.btnMilitaryEnt.Action = "militaryent" + this.ind;
    }

    protected override int MoveRecord(bool from, bool to, int recNumber)
    {
      return this.loan.MoveEmployer(from, to, recNumber);
    }

    public new event VerifSummaryChangedEventHandler VerifSummaryChanged;

    public void SummaryChanged(string fieldId, string newValue)
    {
      if (this.VerifSummaryChanged == null)
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) newValue));
    }

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onkeyup(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement) || !(controlForElement.Field.FieldID == "FE0002"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(controlForElement.Field.FieldID, (object) controlForElement.Value));
    }

    public override bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      FieldControl controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as FieldControl;
      string empty = string.Empty;
      if (controlForElement != null)
      {
        string fieldId = controlForElement.Field.FieldID;
        if (fieldId.Length == 6 && fieldId.Substring(4) == "63" && controlForElement.Value == "N" && DialogResult.OK != Utils.Dialog((IWin32Window) this.mainScreen, "Selecting OK will clear all associated Military Entitlements with the current record.  Do you wish to proceed?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
        {
          ((EllieMae.Encompass.Forms.CheckBox) controlForElement).Checked = true;
          return false;
        }
      }
      bool flag = base.onclick(pEvtObj);
      if (this.VerifSummaryChanged == null)
        return false;
      return flag;
    }

    public override void onfocusout(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onfocusout(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return;
      string fieldId = controlForElement.Field.FieldID;
      if (!(fieldId == "FE0008") && !(fieldId == "FE0009"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) controlForElement.Value, this.ind));
    }
  }
}

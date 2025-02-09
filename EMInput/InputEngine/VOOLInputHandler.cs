// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VOOLInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class VOOLInputHandler(
    Sessions.Session session,
    IMainScreen mainScreen,
    HTMLDocument htmldoc,
    Form form,
    object property) : VOLInputHandler(session, mainScreen, htmldoc, form, property)
  {
    private Panel pnllocalAdr;
    private Panel pnlforeignAdr;
    private StandardButton selectCountryButton;
    private List<Panel> panelForeignPanels;

    protected override void SaveBeforeSwitch() => this.FlushOutCurrentField();

    public VOOLInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    protected override void InitHeader()
    {
      this.header = "URLAROL";
      this.IdId = "99";
      this.nameId = "05";
      this.title = "VOOL";
    }

    internal override void CreateControls()
    {
      try
      {
        this.pnllocalAdr = (Panel) this.currentForm.FindControl("localAdr");
        this.pnlforeignAdr = (Panel) this.currentForm.FindControl("foreignAdr");
        this.selectCountryButton = (StandardButton) this.currentForm.FindControl("selectcountry_URLAROL0022");
        if (this.pnlforeignAdr != null && this.pnllocalAdr != null)
          this.pnlforeignAdr.Position = this.pnllocalAdr.Position;
        this.panelForeignPanels = new List<Panel>();
        for (int index = 1; index <= 2; ++index)
          this.panelForeignPanels.Add((Panel) this.currentForm.FindControl("foreignPanel" + (object) index));
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
      if (this.selectCountryButton != null)
      {
        bool flag = this.GetField(this.header + this.ind + "23") == "Y";
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
      base.UpdateFieldValue(id, val);
      this.SetPrintingDefault(id, val);
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      if (id == this.header + this.ind + "14")
        controlState = this.loan.GetSimpleField(this.header + this.ind + "15") == "Y" || this.loan.GetSimpleField(this.header + this.ind + "64") == "Y" ? ControlState.Disabled : ControlState.Enabled;
      else if (id == this.header + this.ind + "04")
        controlState = string.Compare(this.loan.GetSimpleField(this.header + this.ind + "02"), "Other", true) != 0 ? ControlState.Disabled : ControlState.Enabled;
      else if (id == this.header + this.ind + "22" && this.loan.GetSimpleField(this.header + this.ind + "23") != "Y")
        controlState = ControlState.Disabled;
      return controlState;
    }

    protected override void SetPrintingDefault(string id, string val)
    {
      if (id.Length < 6)
        return;
      string str = id.Substring(0, id.Length - 2);
      if (id.EndsWith("15") || id.EndsWith("64"))
      {
        if (!(val == "Y"))
          return;
        base.UpdateFieldValue(str + "14", "");
      }
      else
      {
        if (!id.EndsWith("14") || val.Length <= 0)
          return;
        this.inputData.SetCurrentField(str + "15", "N");
      }
    }

    public new event VerifSummaryChangedEventHandler VerifSummaryChanged;

    public new void SummaryChanged(string fieldId, string newValue)
    {
      if (this.VerifSummaryChanged == null)
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) newValue));
    }

    public override void onkeyup(IHTMLEventObj pEvtObj)
    {
      base.onkeyup(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement) || !(controlForElement.Field.FieldID == "URLAROL0001") && !(controlForElement.Field.FieldID == "URLAROL0002") && !(controlForElement.Field.FieldID == "URLAROL0004") && !(controlForElement.Field.FieldID == "URLAROL0003"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(controlForElement.Field.FieldID, (object) controlForElement.Value));
    }

    public override void onfocusout(IHTMLEventObj pEvtObj)
    {
      base.onfocusout(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return;
      string fieldId = controlForElement.Field.FieldID;
      if (!(controlForElement.Field.FieldID == "URLAROL0001") && !(controlForElement.Field.FieldID == "URLAROL0002") && !(controlForElement.Field.FieldID == "URLAROL0004") && !(controlForElement.Field.FieldID == "URLAROL0003"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) controlForElement.Value));
    }

    public override bool onclick(IHTMLEventObj pEvtObj)
    {
      bool flag = base.onclick(pEvtObj);
      if (this.VerifSummaryChanged == null)
        return false;
      if (!(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return flag;
      string fieldId = controlForElement.Field.FieldID;
      if (controlForElement.Field.FieldID == "URLAROL0002")
        this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) controlForElement.Value));
      return flag;
    }
  }
}

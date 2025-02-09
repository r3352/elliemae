// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VOOAInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class VOOAInputHandler(
    Sessions.Session session,
    IMainScreen mainScreen,
    HTMLDocument htmldoc,
    Form form,
    object property) : VOLInputHandler(session, mainScreen, htmldoc, form, property)
  {
    protected override void SaveBeforeSwitch() => this.FlushOutCurrentField();

    public VOOAInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    protected override void InitHeader()
    {
      this.header = "URLAROA";
      this.IdId = "01";
      this.nameId = "05";
      this.title = "VOOA";
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      this.SetPrintingDefault(id, val);
      if (!(id == this.header + this.ind + "02"))
        return;
      this.loan.Calculator.UpdateAccountName("36");
    }

    public new event VerifSummaryChangedEventHandler VerifSummaryChanged;

    public new void SummaryChanged(string fieldId, string newValue)
    {
      if (this.VerifSummaryChanged == null)
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) newValue));
    }

    protected override void SetPrintingDefault(string id, string val)
    {
      if (id.Length < 6)
        return;
      string str = id.Substring(0, id.Length - 2);
      if (id.EndsWith("15") || id.EndsWith("20"))
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

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      if (id == this.header + this.ind + "04")
        controlState = string.Compare(this.loan.GetSimpleField(this.header + this.ind + "02"), "Other", true) != 0 ? ControlState.Disabled : ControlState.Enabled;
      else if (id == this.header + this.ind + "14")
        controlState = this.loan.GetSimpleField(this.header + this.ind + "15") == "Y" || this.loan.GetSimpleField(this.header + this.ind + "20") == "Y" ? ControlState.Disabled : ControlState.Enabled;
      return controlState;
    }

    public override void onkeyup(IHTMLEventObj pEvtObj)
    {
      base.onkeyup(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement) || !(controlForElement.Field.FieldID == "URLAROA0004"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(controlForElement.Field.FieldID, (object) controlForElement.Value));
    }

    public override void onfocusout(IHTMLEventObj pEvtObj)
    {
      base.onfocusout(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement) || !(controlForElement.Field.FieldID == "URLAROA0003"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(controlForElement.Field.FieldID, (object) controlForElement.Value));
    }

    public override bool onclick(IHTMLEventObj pEvtObj)
    {
      bool flag = base.onclick(pEvtObj);
      if (this.VerifSummaryChanged == null)
        return false;
      if (!(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return flag;
      string fieldId = controlForElement.Field.FieldID;
      if (fieldId == "URLAROA0001" || fieldId == "URLAROA0002")
        this.VerifSummaryChanged(new VerifSummaryChangeInfo(controlForElement.Field.FieldID, (object) controlForElement.Value));
      return flag;
    }
  }
}

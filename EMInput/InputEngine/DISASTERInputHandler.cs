// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.DISASTERInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class DISASTERInputHandler(
    Sessions.Session session,
    IMainScreen mainScreen,
    HTMLDocument htmldoc,
    EllieMae.Encompass.Forms.Form form,
    object property) : VOLInputHandler(session, mainScreen, htmldoc, form, property)
  {
    protected override void SaveBeforeSwitch() => this.FlushOutCurrentField();

    public DISASTERInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    protected override void InitHeader()
    {
      this.header = "FEMA";
      this.title = "FEMA";
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
    }

    public new event VerifSummaryChangedEventHandler VerifSummaryChanged;

    public new void SummaryChanged(string fieldId, string newValue)
    {
      if (this.VerifSummaryChanged == null)
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) newValue));
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      return this.getControlState(ctrl, id, ControlState.Enabled);
    }

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onkeyup(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return;
      string fieldId = controlForElement.Field.FieldID;
      if (!(fieldId == "FEMA0001") && !(fieldId == "FEMA0002") && !(fieldId == "FEMA0005") && !(fieldId == "FEMA0007") && !(fieldId == "FEMA0004"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(controlForElement.Field.FieldID, (object) controlForElement.Value));
    }

    public override void onCalendarPopupClosed(object sender, FormClosedEventArgs e)
    {
      base.onCalendarPopupClosed(sender, e);
      if (this.VerifSummaryChanged == null)
        return;
      CalendarPopup calendarPopup = (CalendarPopup) sender;
      if (!(calendarPopup.Tag is EllieMae.Encompass.Forms.Calendar tag) || !(tag.DateField.FieldID == "FEMA0004"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(tag.DateField.FieldID, (object) calendarPopup.SelectedDate.ToString("MM/dd/yyyy")));
    }

    public override bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      bool flag = base.onclick(pEvtObj);
      if (this.VerifSummaryChanged == null)
        return false;
      if (!(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return flag;
      string fieldId = controlForElement.Field.FieldID;
      if (fieldId == "FEMA0003" || fieldId == "FEMA0006" || fieldId == "FEMA0004")
        this.VerifSummaryChanged(new VerifSummaryChangeInfo(controlForElement.Field.FieldID, (object) controlForElement.Value));
      return flag;
    }
  }
}

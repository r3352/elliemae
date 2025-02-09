// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AFFILIATEDBAInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class AFFILIATEDBAInputHandler : InputHandlerBase
  {
    private const string header = "AB";
    protected string ind = "00";
    private bool fieldsReadOnly;

    public AFFILIATEDBAInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public AFFILIATEDBAInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public AFFILIATEDBAInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public AFFILIATEDBAInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public override object Property
    {
      get => (object) this.ind;
      set
      {
        if (!this.IsDeleted)
          this.FlushOutCurrentField();
        if (value != null)
          this.ind = int.Parse(value.ToString()).ToString("00");
        this.IsDeleted = false;
      }
    }

    protected override string MapFieldId(string id)
    {
      if (id.StartsWith("AB"))
        id = id.Length <= 6 ? id.Remove(2, 2).Insert(2, this.ind) : id.Remove(2, 3).Insert(2, this.ind);
      return id;
    }

    public event VerifSummaryChangedEventHandler VerifSummaryChanged;

    internal void SummaryChanged(string fieldId, string newValue)
    {
      if (this.VerifSummaryChanged == null)
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) newValue));
    }

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onkeyup(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement) || !(controlForElement.Field.FieldID == "AB0006") && !(controlForElement.Field.FieldID == "AB0007") && !(controlForElement.Field.FieldID == "AB0010") && !(controlForElement.Field.FieldID == "AB0022") && !(controlForElement.Field.FieldID == "AB0008"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(controlForElement.Field.FieldID, (object) controlForElement.Value));
    }

    internal bool FieldsReadOnly
    {
      set => this.fieldsReadOnly = value;
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      this.getControlState(ctrl, id, ControlState.Enabled);
      return !this.fieldsReadOnly ? ControlState.Default : ControlState.Disabled;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (id.EndsWith("22"))
      {
        this.SummaryChanged("AB0022", val == "Y" ? "Y" : "");
      }
      else
      {
        if (!id.EndsWith("08"))
          return;
        this.SummaryChanged("AB0008", val == "Y" ? "Y" : "");
      }
    }
  }
}

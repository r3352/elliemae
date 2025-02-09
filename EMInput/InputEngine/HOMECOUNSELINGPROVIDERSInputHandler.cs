// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HOMECOUNSELINGPROVIDERSInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class HOMECOUNSELINGPROVIDERSInputHandler : InputHandlerBase
  {
    private const string header = "HC";
    protected string ind = "00";

    public HOMECOUNSELINGPROVIDERSInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public HOMECOUNSELINGPROVIDERSInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public HOMECOUNSELINGPROVIDERSInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public HOMECOUNSELINGPROVIDERSInputHandler(
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
      if (id.StartsWith("HC"))
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
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return;
      string fieldId = controlForElement.Field.FieldID;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(fieldId))
      {
        case 688847888:
          if (!(fieldId == "HC0004"))
            return;
          this.VerifSummaryChanged(new VerifSummaryChangeInfo(controlForElement.Field.FieldID, (object) HOMECOUNSELINGPROVIDERSInputHandler.BuildAgencyAddress(this.GetField("HC" + this.ind + "03"), controlForElement.Value, this.GetField("HC" + this.ind + "05"), this.GetField("HC" + this.ind + "06"))));
          return;
        case 705625507:
          if (!(fieldId == "HC0005"))
            return;
          this.VerifSummaryChanged(new VerifSummaryChangeInfo(controlForElement.Field.FieldID, (object) HOMECOUNSELINGPROVIDERSInputHandler.BuildAgencyAddress(this.GetField("HC" + this.ind + "03"), this.GetField("HC" + this.ind + "04"), controlForElement.Value, this.GetField("HC" + this.ind + "06"))));
          return;
        case 722403126:
          if (!(fieldId == "HC0006"))
            return;
          this.VerifSummaryChanged(new VerifSummaryChangeInfo(controlForElement.Field.FieldID, (object) HOMECOUNSELINGPROVIDERSInputHandler.BuildAgencyAddress(this.GetField("HC" + this.ind + "03"), this.GetField("HC" + this.ind + "04"), this.GetField("HC" + this.ind + "05"), controlForElement.Value)));
          return;
        case 739180745:
          if (!(fieldId == "HC0007"))
            return;
          break;
        case 739327840:
          if (!(fieldId == "HC0017"))
            return;
          break;
        case 756105459:
          if (!(fieldId == "HC0016"))
            return;
          break;
        case 789513602:
          if (!(fieldId == "HC0002"))
            return;
          break;
        case 789660697:
          if (!(fieldId == "HC0014"))
            return;
          this.VerifSummaryChanged(new VerifSummaryChangeInfo(controlForElement.Field.FieldID, controlForElement.Value == "Y" ? (object) "Y" : (object) ""));
          return;
        case 806291221:
          if (!(fieldId == "HC0003"))
            return;
          this.VerifSummaryChanged(new VerifSummaryChangeInfo(controlForElement.Field.FieldID, (object) HOMECOUNSELINGPROVIDERSInputHandler.BuildAgencyAddress(controlForElement.Value, this.GetField("HC" + this.ind + "04"), this.GetField("HC" + this.ind + "05"), this.GetField("HC" + this.ind + "06"))));
          return;
        case 806438316:
          if (!(fieldId == "HC0013"))
            return;
          goto label_26;
        case 823215935:
          if (!(fieldId == "HC0012"))
            return;
          goto label_26;
        default:
          return;
      }
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(controlForElement.Field.FieldID, (object) controlForElement.Value));
      return;
label_26:
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(controlForElement.Field.FieldID, (object) controlForElement.Value.Replace("\r\n", ";")));
    }

    public static string BuildAgencyAddress(string add, string city, string state, string zipcode)
    {
      return add.Trim() + (add.Trim() != string.Empty ? "," : "") + city + (city != string.Empty ? "," : "") + state + (state != string.Empty ? " " : "") + zipcode;
    }

    public override void onfocusout(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onfocusout(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return;
      if (controlForElement.Field.FieldID.EndsWith("06"))
        this.VerifSummaryChanged(new VerifSummaryChangeInfo("HC0006", (object) HOMECOUNSELINGPROVIDERSInputHandler.BuildAgencyAddress(this.GetField("HC" + this.ind + "03"), this.GetField("HC" + this.ind + "04"), this.GetField("HC" + this.ind + "05"), this.GetField("HC" + this.ind + "06"))));
      else if (controlForElement.Field.FieldID.EndsWith("17"))
        this.VerifSummaryChanged(new VerifSummaryChangeInfo("HC0017", (object) this.GetField("HC" + this.ind + "17")));
      try
      {
        if (controlForElement.Field.FieldID.Equals("HC0010") && controlForElement.Value != string.Empty && !Utils.ValidateEmail(controlForElement.Value))
          throw new FieldValidationException(controlForElement.Field.FieldID, controlForElement.Value, "The value '" + controlForElement.Value + "' is not a valid email format. Please change it.");
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id.StartsWith("HC") && id.EndsWith("14"))
      {
        this.loan.SetSelectedHomeCounselingProvider(Utils.ParseInt(this.Property), val == "Y");
        this.SummaryChanged("HC0014", val == "Y" ? "Y" : "");
      }
      else
        base.UpdateFieldValue(id, val);
    }
  }
}

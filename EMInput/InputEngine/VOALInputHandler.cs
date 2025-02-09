// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VOALInputHandler
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
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class VOALInputHandler : InputHandlerBase
  {
    protected string header;
    protected string ind = "00";
    private StandardButton btnMonthlypi;
    private StandardButton btnMaximumpi;
    private EllieMae.Encompass.Forms.Panel pnlForm;
    protected string IdId = "99";
    protected string nameId = "02";
    protected string title = "VOAL";
    protected string brwId = "01";

    public override object Property
    {
      get => (object) this.ind;
      set
      {
        this.InitHeader();
        if (!this.IsDeleted)
          this.SaveBeforeSwitch();
        if (value != null)
          this.ind = int.Parse(value.ToString()).ToString("00");
        this.IsDeleted = false;
      }
    }

    protected virtual void InitHeader() => this.header = "URLARAL";

    protected virtual void SaveBeforeSwitch() => this.FlushOutCurrentField();

    public VOALInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public VOALInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public VOALInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public VOALInputHandler(
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
        this.btnMonthlypi = (StandardButton) this.currentForm.FindControl("btnMonthlypi");
        this.btnMaximumpi = (StandardButton) this.currentForm.FindControl("btnMaximumpi");
        this.pnlForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        if (!(this.loan.GetField("1825") == "2020") || this.ind == null)
          return;
        string str = this.header + this.ind;
        this.btnMonthlypi.Action = "monthlypi" + str;
        this.btnMaximumpi.Action = "maximumpi" + str;
      }
      catch (Exception ex)
      {
      }
    }

    protected override string MapFieldId(string id)
    {
      if (id.StartsWith(this.header))
        id = !id.StartsWith("URLAR") ? (id.Length <= 6 ? id.Remove(2, 2).Insert(2, this.ind) : id.Remove(2, 3).Insert(2, this.ind)) : id.Remove(this.header.Length, 2).Insert(this.header.Length, this.ind);
      return id;
    }

    public event VerifSummaryChangedEventHandler VerifSummaryChanged;

    public void SummaryChanged(string fieldId, string newValue)
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
      if (!(fieldId == "URLARAL0002") && !(fieldId == "URLARAL0016") && !(fieldId == "URLARAL0017") && !(fieldId == "URLARAL0018") && !(fieldId == "URLARAL0020"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) controlForElement.Value));
    }

    public override bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onclick(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return false;
      string fieldId = controlForElement.Field.FieldID;
      if (fieldId == "URLARAL0016" || fieldId == "URLARAL0017")
        this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) controlForElement.Value));
      return false;
    }

    public override void AddToEFolder()
    {
      if (!new eFolderAccessRights(this.session.LoanDataMgr).CanAddDocuments)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) null, "You do not have rights to add documents to the eFolder.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.GetFieldValue(this.header + this.ind + "97") == "")
      {
        int num2 = (int) Utils.Dialog((IWin32Window) null, "The document tracking list already contains this verification.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.UpdateFieldValue(this.header + this.ind + "97", "");
        this.UpdateDocTrackingForVerifs(this.header + this.ind + "97", true);
        int num3 = (int) Utils.Dialog((IWin32Window) null, "The verification has been added to document tracking list successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    public override void onfocusout(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onfocusout(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return;
      string fieldId = controlForElement.Field.FieldID;
      if (!(fieldId == "URLARAL0002") && !(fieldId == "URLARAL0016") && !(fieldId == "URLARAL0017") && !(fieldId == "URLARAL0018") && !(fieldId == "URLARAL0020"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) controlForElement.Value));
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      string str1 = "0";
      string str2 = "";
      if (id.Length > 10)
        str1 = id.Substring(9, 2);
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
        case "01":
        case "15":
        case "16":
        case "17":
        case "18":
        case "19":
        case "20":
        case "21":
        case "25":
        case "98":
          if (this.loan.GetSimpleField(this.header + this.ind + "25") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "11":
          controlState = this.loan.GetSimpleField(this.header + this.ind + "12") == "Y" || this.loan.GetSimpleField(this.header + this.ind + "64") == "Y" ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "34":
          if (this.loan.GetSimpleField(this.header + this.ind + "25") == "Y" && this.loan.GetSimpleField(this.header + this.ind + "16") == "HELOC")
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

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      if (action.StartsWith("monthlypi"))
      {
        this.SetFieldFocus("l_urlaral18");
      }
      else
      {
        if (!action.StartsWith("maximumpi"))
          return;
        this.SetFieldFocus("l_urlaral19");
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      this.SetPrintingDefault(id, val);
    }

    protected virtual void SetPrintingDefault(string id, string val)
    {
      if (id.Length < 6)
        return;
      string str = id.Substring(0, id.Length - 2);
      if (id.EndsWith("12") || id.EndsWith("64"))
      {
        if (!(val == "Y"))
          return;
        base.UpdateFieldValue(str + "11", "");
      }
      else
      {
        if (!id.EndsWith("11") || val.Length <= 0)
          return;
        this.inputData.SetCurrentField(str + "12", "N");
      }
    }
  }
}

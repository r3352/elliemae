// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SETTLEMENTSERVICEPROVIDERInputHandler
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
  public class SETTLEMENTSERVICEPROVIDERInputHandler : InputHandlerBase
  {
    private DropdownBox sp0010Box;
    private EllieMae.Encompass.Forms.Panel panelCost;
    private EllieMae.Encompass.Forms.Label label_serviceType;
    private EllieMae.Encompass.Forms.TextBox sp0011Box;
    private const string header = "SP";
    protected string ind = "00";
    private bool fieldsReadOnly;

    public SETTLEMENTSERVICEPROVIDERInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public SETTLEMENTSERVICEPROVIDERInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public SETTLEMENTSERVICEPROVIDERInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public SETTLEMENTSERVICEPROVIDERInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public SETTLEMENTSERVICEPROVIDERInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput htmlInput,
      HTMLDocument htmlDoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, htmlInput, htmlDoc, form, property)
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

    internal override void CreateControls()
    {
      try
      {
        if (this.sp0010Box == null)
          this.sp0010Box = (DropdownBox) this.currentForm.FindControl("l_SP0010");
        if (this.panelCost == null)
          this.panelCost = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelCost");
        if (this.label_serviceType == null)
          this.label_serviceType = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label_serviceType");
        if (this.sp0011Box == null)
          this.sp0011Box = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_SP0011");
      }
      catch (Exception ex)
      {
      }
      if (this.panelCost == null)
        return;
      this.panelCost.Visible = false;
    }

    protected override string MapFieldId(string id)
    {
      if (id.StartsWith("SP"))
        id = id.Length <= 6 ? id.Remove(2, 2).Insert(2, this.ind) : id.Remove(2, 3).Insert(2, this.ind);
      return id;
    }

    public event VerifSummaryChangedEventHandler VerifSummaryChanged;

    internal void SummaryChanged(string fieldId, string newValue)
    {
      if (fieldId.EndsWith("01"))
        this.refreshServiceTypes(fieldId, true);
      if (this.VerifSummaryChanged == null)
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) newValue));
    }

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onkeyup(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement) || !(controlForElement.Field.FieldID == "SP0001") && !(controlForElement.Field.FieldID == "SP0002") && !(controlForElement.Field.FieldID == "SP0003") && !(controlForElement.Field.FieldID == "SP0004") && !(controlForElement.Field.FieldID == "SP0005"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(controlForElement.Field.FieldID, (object) controlForElement.Value));
    }

    public override void onfocusout(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onfocusout(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement) || !controlForElement.Field.FieldID.EndsWith("06"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo("SP0004", (object) this.GetFieldValue(this.MapFieldId("SP0004"))));
      this.VerifSummaryChanged(new VerifSummaryChangeInfo("SP0005", (object) this.GetFieldValue(this.MapFieldId("SP0005"))));
    }

    internal bool FieldsReadOnly
    {
      set => this.fieldsReadOnly = value;
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      this.getControlState(ctrl, id, ControlState.Enabled);
      this.refreshServiceTypes(id, false);
      ControlState controlState;
      if (this.inputData is DisclosedSSPLHandler)
      {
        controlState = ControlState.Disabled;
        if (id.EndsWith("01"))
        {
          this.SetControlState("Rolodex1", false);
          this.SetControlState("ContactButton1", false);
          this.SetControlState("ContactButton2", false);
          this.SetControlState("ContactButton3", false);
        }
      }
      else
        controlState = !this.fieldsReadOnly ? ControlState.Default : ControlState.Disabled;
      if (id.EndsWith("01") && this.fieldsReadOnly)
      {
        this.SetControlState("Rolodex1", false);
        this.SetControlState("ContactButton1", false);
      }
      return controlState;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      this.refreshServiceTypes(id, true);
    }

    private void refreshServiceTypes(string id, bool eraseValue)
    {
      if (!id.EndsWith("01"))
        return;
      if (id == "SP0001")
        id = this.MapFieldId(id);
      string lower = this.GetFieldValue(id).ToLower();
      if (lower == "title insurance" || lower == "escrow company")
      {
        if (this.label_serviceType != null && !this.label_serviceType.Visible)
          this.label_serviceType.Visible = this.sp0010Box.Visible = true;
        else if (this.panelCost != null && !this.panelCost.Visible)
          this.panelCost.Visible = this.sp0010Box.Visible = this.sp0011Box.Visible = true;
      }
      else if (this.label_serviceType != null && this.label_serviceType.Visible)
        this.label_serviceType.Visible = this.sp0010Box.Visible = false;
      else if (this.panelCost != null && this.panelCost.Visible)
        this.panelCost.Visible = this.sp0010Box.Visible = this.sp0011Box.Visible = false;
      if (this.sp0010Box.Visible)
      {
        if (lower == "title insurance")
        {
          if (this.sp0010Box.Options != null && this.sp0010Box.Options.Count <= 3 && (this.sp0010Box.Options.Count <= 0 || !(this.sp0010Box.Options[1].Value != "Title - Settlement and Closing")))
            return;
          this.sp0010Box.Options.Clear();
          this.sp0010Box.Options.Add("", "");
          this.sp0010Box.Options.Add("Title - Settlement and Closing", "Title - Settlement and Closing");
          this.sp0010Box.Options.Add("Title - Insurance", "Title - Insurance");
        }
        else
        {
          if (this.sp0010Box.Options != null && this.sp0010Box.Options.Count <= 3 && (this.sp0010Box.Options.Count <= 0 || !(this.sp0010Box.Options[1].Value != "Escrow - Settlement and Closing")))
            return;
          this.sp0010Box.Options.Clear();
          this.sp0010Box.Options.Add("", "");
          this.sp0010Box.Options.Add("Escrow - Settlement and Closing", "Escrow - Settlement and Closing");
          this.sp0010Box.Options.Add("Escrow - Insurance", "Escrow - Insurance");
        }
      }
      else
      {
        if (!eraseValue)
          return;
        this.UpdateFieldValue("SP" + this.Property + "10", "");
        this.UpdateFieldValue("SP" + this.Property + "11", "");
      }
    }
  }
}

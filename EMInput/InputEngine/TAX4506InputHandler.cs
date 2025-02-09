// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TAX4506InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class TAX4506InputHandler : VOLInputHandler
  {
    private EllieMae.Encompass.Forms.Label label1aFName;
    private EllieMae.Encompass.Forms.Label label1aLName;
    private EllieMae.Encompass.Forms.TextBox txt1aLName;
    private EllieMae.Encompass.Forms.Label label2aFName;
    private EllieMae.Encompass.Forms.Label label2aLName;
    private EllieMae.Encompass.Forms.TextBox txt2aLName;

    public TAX4506InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public TAX4506InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public TAX4506InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public TAX4506InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, owner, dataTemplate, htmldoc, form, property)
    {
    }

    protected override void InitHeader()
    {
      this.header = "AR";
      this.IdId = "35";
      this.nameId = "02";
      this.title = "Request for Copy of Tax Return";
    }

    public string CurrentIndex => this.ind;

    internal override void CreateControls()
    {
      try
      {
        if (this.label1aFName == null)
          this.label1aFName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label1aFName");
        if (this.label1aLName == null)
          this.label1aLName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label1aLName");
        if (this.txt1aLName == null)
          this.txt1aLName = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txt1aLName");
        if (this.label2aFName == null)
          this.label2aFName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label2aFName");
        if (this.label2aLName == null)
          this.label2aLName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("label2aLName");
        if (this.txt2aLName != null)
          return;
        this.txt2aLName = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txt2aLName");
      }
      catch (Exception ex)
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      if (id.EndsWith("01") && id.StartsWith(this.header))
      {
        if (this.GetFieldValue("AR" + this.ind + "01") == "Other")
        {
          this.label1aFName.Text = "Name";
          this.label1aLName.Visible = false;
          this.txt1aLName.Visible = false;
          this.label2aFName.Text = "Name";
          this.label2aLName.Visible = false;
          this.txt2aLName.Visible = false;
        }
        else
        {
          this.label1aFName.Text = "First Name";
          this.label1aLName.Visible = true;
          this.txt1aLName.Visible = true;
          this.label2aFName.Text = "First Name";
          this.label2aLName.Visible = true;
          this.txt2aLName.Visible = true;
        }
      }
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      if (id.EndsWith("57") && id.StartsWith(this.header) && this.GetFieldValue(this.header + this.ind + "01") == "Other" || id.EndsWith("58") && id.StartsWith(this.header) && this.GetFieldValue(this.header + this.ind + "01") == "Other")
        controlState = ControlState.Disabled;
      else if (id.EndsWith("32") && id.StartsWith(this.header))
        controlState = ControlState.Disabled;
      else if ((id.EndsWith("65") || id.EndsWith("66")) && id.StartsWith(this.header) && this.GetFieldValue("AR" + this.ind + "01") != "Other")
        controlState = ControlState.Disabled;
      return controlState;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id.EndsWith("25"))
        id = this.header + this.ind + "25";
      else if (id.EndsWith("26"))
        id = this.header + this.ind + "26";
      else if (id.EndsWith("29"))
        id = this.header + this.ind + "29";
      else if (id.EndsWith("30"))
        id = this.header + this.ind + "30";
      else if (id.EndsWith("53"))
        id = this.header + this.ind + "53";
      else if (id.EndsWith("54"))
        id = this.header + this.ind + "54";
      else if (id.EndsWith("55"))
        id = this.header + this.ind + "55";
      else if (id.EndsWith("56"))
        id = this.header + this.ind + "56";
      else if (id.EndsWith("01") && val == "Other")
      {
        base.UpdateFieldValue(this.header + this.ind + "57", "Y");
        base.UpdateFieldValue(this.header + this.ind + "58", "Y");
      }
      base.UpdateFieldValue(id, val);
    }

    public override void ExecAction(string action)
    {
      if (!(action == "copybroInfo"))
        return;
      string fieldValue = this.GetFieldValue(this.header + this.ind + "01");
      if (fieldValue == "Both" || fieldValue == "Borrower")
      {
        this.UpdateFieldValue(this.header + this.ind + "02", this.GetFieldValue("36"));
        this.UpdateFieldValue(this.header + this.ind + "03", this.GetFieldValue("37"));
        this.UpdateFieldValue(this.header + this.ind + "57", "");
        this.UpdateFieldValue(this.header + this.ind + "04", this.GetFieldValue("65"));
        this.UpdateFieldValue(this.header + this.ind + "39", this.GetFieldValue("36"));
        this.UpdateFieldValue(this.header + this.ind + "40", this.GetFieldValue("37"));
        this.UpdateFieldValue(this.header + this.ind + "35", this.GetFieldValue("FR0104"));
        this.UpdateFieldValue(this.header + this.ind + "36", this.GetFieldValue("FR0106"));
        this.UpdateFieldValue(this.header + this.ind + "37", this.GetFieldValue("FR0107"));
        this.UpdateFieldValue(this.header + this.ind + "38", this.GetFieldValue("FR0108"));
        this.UpdateFieldValue(this.header + this.ind + "27", this.GetFieldValue("66"));
      }
      if (fieldValue == "CoBorrower")
      {
        this.UpdateFieldValue(this.header + this.ind + "02", this.GetFieldValue("68"));
        this.UpdateFieldValue(this.header + this.ind + "03", this.GetFieldValue("69"));
        this.UpdateFieldValue(this.header + this.ind + "57", "");
        this.UpdateFieldValue(this.header + this.ind + "04", this.GetFieldValue("97"));
        this.UpdateFieldValue(this.header + this.ind + "39", this.GetFieldValue("68"));
        this.UpdateFieldValue(this.header + this.ind + "40", this.GetFieldValue("69"));
        this.UpdateFieldValue(this.header + this.ind + "35", this.GetFieldValue("FR0204"));
        this.UpdateFieldValue(this.header + this.ind + "36", this.GetFieldValue("FR0206"));
        this.UpdateFieldValue(this.header + this.ind + "37", this.GetFieldValue("FR0207"));
        this.UpdateFieldValue(this.header + this.ind + "38", this.GetFieldValue("FR0208"));
        this.UpdateFieldValue(this.header + this.ind + "27", this.GetFieldValue("98"));
      }
      if (fieldValue == "Both")
      {
        this.UpdateFieldValue(this.header + this.ind + "06", this.GetFieldValue("68"));
        this.UpdateFieldValue(this.header + this.ind + "07", this.GetFieldValue("69"));
        this.UpdateFieldValue(this.header + this.ind + "58", "");
        this.UpdateFieldValue(this.header + this.ind + "05", this.GetFieldValue("97"));
      }
      else
      {
        this.UpdateFieldValue(this.header + this.ind + "06", "");
        this.UpdateFieldValue(this.header + this.ind + "07", "");
        this.UpdateFieldValue(this.header + this.ind + "58", "");
        this.UpdateFieldValue(this.header + this.ind + "05", "");
      }
      this.UpdateContents();
      this.SetFieldFocus("l_IRS4506X2");
    }

    public new event VerifSummaryChangedEventHandler VerifSummaryChanged;

    public new void SummaryChanged(string fieldId, string newValue)
    {
      if (this.VerifSummaryChanged == null)
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) newValue));
    }

    public override void onCalendarPopupClosed(object sender, FormClosedEventArgs e)
    {
      CalendarPopup calendarPopup = (CalendarPopup) sender;
      if (calendarPopup.DialogResult != DialogResult.OK)
        return;
      EllieMae.Encompass.Forms.Calendar tag = calendarPopup.Tag as EllieMae.Encompass.Forms.Calendar;
      if (tag.DateField != null && !this.setFieldValue(this.getActualFieldID(tag.DateField.FieldID), tag.FieldSource, calendarPopup.SelectedDate.ToString("MM/dd/yyyy")))
        return;
      if (this.executeDateSelectedEvent((RuntimeControl) tag, calendarPopup.SelectedDate))
        this.RefreshContents();
      if (this.VerifSummaryChanged == null)
        return;
      this.refreshColumnValues(tag.DateField.FieldID, calendarPopup.SelectedDate.ToString("MM/dd/yyyy"));
    }

    private string getActualFieldID(string id)
    {
      if (!id.StartsWith(this.header))
        return id;
      string str = string.Empty;
      if (id.Length == 6)
        str = id.Substring(4);
      else if (id.Length == 7)
        str = id.Substring(5);
      return this.header + this.ind + str;
    }

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onkeyup(pEvtObj);
      if (this.VerifSummaryChanged == null)
        return;
      FieldControl controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as FieldControl;
      this.refreshColumnValues(controlForElement.Field.FieldID, controlForElement.Value);
    }

    public override void onfocusout(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onfocusout(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return;
      this.refreshColumnValues(controlForElement.Field.FieldID, controlForElement.Value);
    }

    public override bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      bool flag = base.onclick(pEvtObj);
      if (this.VerifSummaryChanged == null)
        return false;
      if (!(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return flag;
      this.refreshColumnValues(controlForElement.Field.FieldID, controlForElement.Value);
      return flag;
    }

    private void refreshColumnValues(string fieldId, string val)
    {
      if (!fieldId.EndsWith("01") && !fieldId.EndsWith("24") && !fieldId.EndsWith("25") && !fieldId.EndsWith("26") && !fieldId.EndsWith("29") && !fieldId.EndsWith("30") && !fieldId.EndsWith("46") && !fieldId.EndsWith("47") && !fieldId.EndsWith("48") && !fieldId.EndsWith("53") && !fieldId.EndsWith("54") && !fieldId.EndsWith("55") && !fieldId.EndsWith("56"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) val));
    }

    public override void AddToEFolder()
    {
      if (!new eFolderAccessRights(this.session.LoanDataMgr).CanAddDocuments)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) null, "You do not have rights to add documents to the eFolder.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        string field = this.loan.GetField(this.header + this.ind + "63");
        VerifLog[] allRecordsOfType = (VerifLog[]) this.loan.GetLogList().GetAllRecordsOfType(typeof (VerifLog));
        if (allRecordsOfType != null)
        {
          for (int index = 0; index < allRecordsOfType.Length; ++index)
          {
            if (allRecordsOfType[index].Id == field)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) null, "The document tracking list already contains this verification.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
          }
        }
        if (!this.UpdateDocTrackingForVerifs(this.header + this.ind + "97", true))
          return;
        int num3 = (int) Utils.Dialog((IWin32Window) null, "The verification has been added to document tracking list successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }
  }
}

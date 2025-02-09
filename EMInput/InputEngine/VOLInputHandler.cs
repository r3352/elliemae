// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VOLInputHandler
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
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class VOLInputHandler : InputHandlerBase
  {
    protected string header;
    protected string ind = "00";
    protected string IdId = "99";
    protected string nameId = "02";
    protected string title = "VOL";
    private DropdownEditBox ddlAcctType;
    private DropdownEditBox ddlAcctType2020;
    private EllieMae.Encompass.Forms.Panel pnlAccInfo2020;
    private EllieMae.Encompass.Forms.GroupBox grDebtInfo;
    private EllieMae.Encompass.Forms.GroupBox grAcctInfo;
    private EllieMae.Encompass.Forms.GroupBox grpPaymentHistory;
    private EllieMae.Encompass.Forms.CheckBox chkPaymentIncludes;
    private EllieMae.Encompass.Forms.Panel pnllocalAdr;
    private EllieMae.Encompass.Forms.Panel pnlforeignAdr;
    private StandardButton selectCountryButton;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignPanels;

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

    protected virtual void InitHeader() => this.header = "FL";

    protected virtual void SaveBeforeSwitch() => this.FlushOutCurrentField();

    public VOLInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public VOLInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public VOLInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public VOLInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public VOLInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, input, htmldoc, form, property)
    {
    }

    protected override string MapFieldId(string id)
    {
      if (id.StartsWith(this.header))
      {
        if (id.StartsWith("NBOC"))
          id = id.Remove(4, 2).Insert(4, this.ind);
        else if (id.StartsWith("URLAR"))
        {
          id = id.Remove(this.header.Length, 2).Insert(this.header.Length, this.ind);
        }
        else
        {
          if (id == "IRS4506.X67" || id == "IRS4506.X92")
            return id;
          id = !id.StartsWith("FEMA") ? (!id.StartsWith("IR") || id.IndexOf("A") <= -1 ? (id.Length <= 6 ? id.Remove(2, 2).Insert(2, this.ind) : id.Remove(2, 3).Insert(2, this.ind)) : id.Remove(2, 2).Insert(2, this.ind)) : id.Remove(4, 2).Insert(4, this.ind);
        }
      }
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
      if (!(fieldId == "FL0002") && !(fieldId == "FL0008") && !(fieldId == "FL0013") && !(fieldId == "FL0012") && !(fieldId == "FL0011"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) controlForElement.Value));
    }

    public override bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onclick(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return false;
      string fieldId = controlForElement.Field.FieldID;
      if (fieldId == "FL0018" || fieldId == "FL0017" || fieldId == "FL0012" || fieldId == "FL0011")
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
      if (!(fieldId == "FL0013") && !(fieldId == "FL0012") && !(fieldId == "FL0011"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) controlForElement.Value));
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      if (id.StartsWith("NBOC"))
        return controlState;
      string str1 = "0";
      string str2 = "";
      if (id == "SYS.X13")
      {
        if (string.Compare(this.loan.GetSimpleField(this.header + this.ind + "08"), "revolving", true) != 0)
          controlState = ControlState.Disabled;
      }
      else
      {
        if (id.Length > 6)
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
          case "27":
            controlState = !this.isLinkedVOM(this.loan.GetSimpleField(str2 + "25")) ? ControlState.Enabled : ControlState.Disabled;
            break;
          case "37":
            if (this.loan.GetSimpleField(str2 + "38") == "Y" || this.loan.GetSimpleField(str2 + "64") == "Y")
            {
              controlState = ControlState.Disabled;
              break;
            }
            break;
          case "61":
          case "62":
            controlState = !(this.loan.GetSimpleField(str2 + "18") == "Y") ? ControlState.Disabled : ControlState.Enabled;
            break;
          case "65":
            controlState = !(this.loan.GetSimpleField(str2 + "08") == "OtherLiability") ? ControlState.Disabled : ControlState.Enabled;
            break;
          case "66":
            controlState = this.loan.GetSimpleField(str2 + "08") == "HELOC" || this.loan.GetSimpleField(str2 + "08") == "MortgageLoan" ? ControlState.Enabled : ControlState.Disabled;
            break;
          case "68":
            if (this.loan.GetSimpleField(str2 + "67") != "Y")
            {
              controlState = ControlState.Disabled;
              break;
            }
            break;
          default:
            controlState = ControlState.Default;
            break;
        }
      }
      return controlState;
    }

    internal override void CreateControls()
    {
      try
      {
        this.ddlAcctType = (DropdownEditBox) this.currentForm.FindControl("DropdownEditBox5");
        this.ddlAcctType2020 = (DropdownEditBox) this.currentForm.FindControl("ddUrla2020Type");
        this.pnlAccInfo2020 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlAccInfo2020");
        this.grDebtInfo = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox3");
        this.grAcctInfo = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox4");
        this.grpPaymentHistory = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox5");
        this.chkPaymentIncludes = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("I_FL0066");
        if (this.loan.GetField("1825") == "2020")
        {
          this.ddlAcctType2020.Visible = !(this.ddlAcctType.Visible = false);
          this.pnlAccInfo2020.Visible = true;
          this.pnlAccInfo2020.Position = new Point(5, 20);
          this.grDebtInfo.Position = new Point(this.grDebtInfo.Position.X, this.grAcctInfo.Bottom + 8);
          this.grpPaymentHistory.Position = new Point(this.grpPaymentHistory.Position.X, this.grDebtInfo.Bottom + 8);
          this.chkPaymentIncludes.Visible = true;
        }
        else
        {
          this.ddlAcctType2020.Visible = !(this.ddlAcctType.Visible = true);
          this.pnlAccInfo2020.Visible = false;
          this.grDebtInfo.Position = new Point(this.grDebtInfo.Position.X, this.grAcctInfo.Bottom + 8);
          this.grpPaymentHistory.Position = new Point(this.grpPaymentHistory.Position.X, this.grDebtInfo.Bottom + 8);
          this.chkPaymentIncludes.Visible = false;
        }
        this.pnllocalAdr = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("localAdr");
        this.pnlforeignAdr = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignAdr");
        this.selectCountryButton = (StandardButton) this.currentForm.FindControl("selectcountry_FL0068");
        if (this.pnlforeignAdr != null && this.pnllocalAdr != null)
          this.pnlforeignAdr.Position = this.pnllocalAdr.Position;
        this.panelForeignPanels = new List<EllieMae.Encompass.Forms.Panel>();
        for (int index = 1; index <= 2; ++index)
          this.panelForeignPanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignPanel" + (object) index));
      }
      catch (Exception ex)
      {
      }
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.selectCountryButton != null)
      {
        bool flag = this.GetField(this.header + this.ind + "67") == "Y";
        this.selectCountryButton.Visible = flag;
        this.pnllocalAdr.Visible = !flag;
        this.pnlforeignAdr.Visible = flag;
      }
      if (this.panelForeignPanels == null)
        return;
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
      if (id == "SYS.X13" || id == "FL" + this.ind + "11" || id == "FL" + this.ind + "13" || id == "FL" + this.ind + "08")
      {
        if (id != "SYS.X13")
          base.UpdateFieldValue(id, val);
        if (string.Compare(this.GetField("FL" + this.ind + "08"), "revolving", true) == 0 && (id == "SYS.X13" && Utils.ParseInt((object) val) != Utils.ParseInt((object) this.GetField("SYS.X13")) || (double) Utils.ParseInt((object) this.GetField("FL" + this.ind + "11")) < 10.0 && id == "FL" + this.ind + "11") && !this.loan.Calculator.UpdateRevolvingLiabilities(id, val, true, true, true) && id == "SYS.X13")
          base.UpdateFieldValue(id, val);
      }
      else
        base.UpdateFieldValue(id, val);
      if ((id.StartsWith("AR") || id.StartsWith("IR")) && id.EndsWith("01") && id.Length == 6)
      {
        if (val == "Other")
        {
          base.UpdateFieldValue(this.header + this.ind + "02", (this.GetField(this.header + this.ind + "02") + " " + this.GetField(this.header + this.ind + "03")).Trim());
          base.UpdateFieldValue(this.header + this.ind + "06", (this.GetField(this.header + this.ind + "06") + " " + this.GetField(this.header + this.ind + "07")).Trim());
          base.UpdateFieldValue(this.header + this.ind + "03", "");
          base.UpdateFieldValue(this.header + this.ind + "07", "");
        }
        else if (val != string.Empty)
        {
          string field1 = this.GetField(this.header + this.ind + "02");
          if (this.GetField(this.header + this.ind + "03") == string.Empty && field1.LastIndexOf(" ") > -1)
          {
            base.UpdateFieldValue(this.header + this.ind + "02", field1.Substring(0, field1.LastIndexOf(" ")));
            base.UpdateFieldValue(this.header + this.ind + "03", field1.Substring(field1.LastIndexOf(" ") + 1));
          }
          string field2 = this.GetField(this.header + this.ind + "06");
          if (this.GetField(this.header + this.ind + "07") == string.Empty && field2.LastIndexOf(" ") > -1)
          {
            base.UpdateFieldValue(this.header + this.ind + "06", field2.Substring(0, field2.LastIndexOf(" ")));
            base.UpdateFieldValue(this.header + this.ind + "07", field2.Substring(field2.LastIndexOf(" ") + 1));
          }
        }
      }
      this.SetPrintingDefault(id, val);
      if (id == "FL" + this.ind + "15")
        this.loan.Calculator.UpdateAccountName("36");
      if (id == "FL" + this.ind + "08" && val != string.Empty)
        this.loan.Calculator.UpdateAccountName("36");
      if (id == "FL" + this.ind + "11" || id == "FL" + this.ind + "13" || id == "FL" + this.ind + "08")
      {
        this.VerifSummaryChanged(new VerifSummaryChangeInfo("FL0011", (object) this.GetField("FL" + this.ind + "11")));
      }
      else
      {
        if (!(id == "SYS.X13"))
          return;
        int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
        for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
        {
          if (!(index.ToString("00") == this.ind) && string.Compare(this.GetField("FL" + index.ToString("00") + "08"), "revolving", true) == 0)
            this.VerifSummaryChanged(new VerifSummaryChangeInfo("FL" + index.ToString("00") + "11", (object) this.GetField("FL" + index.ToString("00") + "11")));
        }
        this.VerifSummaryChanged(new VerifSummaryChangeInfo("FL0011", (object) this.GetField("FL" + this.ind + "11")));
      }
    }

    protected virtual void SetPrintingDefault(string id, string val)
    {
      if (id.Length < 6)
        return;
      string str = id.Substring(0, 4);
      if (id.Length == 7)
        str = id.Substring(0, 5);
      if (id.EndsWith("38"))
      {
        if (!(val == "Y") && !(this.loan.GetSimpleField(str + "64") == "Y"))
          return;
        base.UpdateFieldValue(str + "37", "");
      }
      else if (id.EndsWith("64"))
      {
        if (!(val == "Y") && !(this.loan.GetSimpleField(str + "38") == "Y"))
          return;
        base.UpdateFieldValue(str + "37", "");
      }
      else
      {
        if (!id.EndsWith("37") || val.Length <= 0)
          return;
        this.inputData.SetCurrentField(str + "38", "N");
        this.inputData.SetCurrentField(str + "64", "N");
      }
    }

    private bool isLinkedVOM(string selectedREOID)
    {
      bool flag = false;
      if (!string.IsNullOrEmpty(selectedREOID))
      {
        int numberOfMortgages = this.loan.GetNumberOfMortgages();
        for (int index = 1; index <= numberOfMortgages; ++index)
        {
          if (this.GetField("FM" + index.ToString("00") + "43") == selectedREOID)
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    public override void ExecAction(string action)
    {
      if (action.ToLower().StartsWith("selectcountry_"))
        action = "selectcountry_" + this.MapFieldId(action.Replace("selectcountry_", ""));
      base.ExecAction(action);
    }
  }
}

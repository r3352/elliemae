// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VODInputHandler
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
  internal class VODInputHandler : VOLInputHandler
  {
    private DropdownEditBox assetType1Urla2009;
    private DropdownEditBox assetType2Urla2009;
    private DropdownEditBox assetType3Urla2009;
    private DropdownEditBox assetType4Urla2009;
    private DropdownBox assetType1Urla2020;
    private DropdownBox assetType2Urla2020;
    private DropdownBox assetType3Urla2020;
    private DropdownBox assetType4Urla2020;
    private EllieMae.Encompass.Forms.Panel panelURLA2020Deposit;
    private EllieMae.Encompass.Forms.Panel panelURLA2009Deposit;
    private EllieMae.Encompass.Forms.Panel panelAUSExport;
    private EllieMae.Encompass.Forms.Panel pnllocalAdr;
    private EllieMae.Encompass.Forms.Panel pnlforeignAdr;
    private StandardButton selectCountryButton;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignPanels;

    protected override void SaveBeforeSwitch() => this.FlushOutCurrentField();

    public VODInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public VODInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public VODInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public VODInputHandler(
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
      this.header = "DD";
      this.IdId = "35";
      this.nameId = "02";
      this.title = "VOD";
    }

    internal override void CreateControls()
    {
      try
      {
        this.panelURLA2009Deposit = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelURLA2009Balance");
        this.panelURLA2020Deposit = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelURLA2020Balance");
        this.panelAUSExport = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelAUSExport");
        this.assetType1Urla2009 = (DropdownEditBox) this.currentForm.FindControl("DropdownEditBox1");
        this.assetType2Urla2009 = (DropdownEditBox) this.currentForm.FindControl("DropdownEditBox2");
        this.assetType3Urla2009 = (DropdownEditBox) this.currentForm.FindControl("DropdownEditBox3");
        this.assetType4Urla2009 = (DropdownEditBox) this.currentForm.FindControl("DropdownEditBox4");
        this.assetType1Urla2020 = (DropdownBox) this.currentForm.FindControl("DropdownBox2");
        this.assetType2Urla2020 = (DropdownBox) this.currentForm.FindControl("DropdownBox3");
        this.assetType3Urla2020 = (DropdownBox) this.currentForm.FindControl("DropdownBox4");
        this.assetType4Urla2020 = (DropdownBox) this.currentForm.FindControl("DropdownBox5");
        if (this.loan.GetField("1825") == "2020")
        {
          DropdownBox assetType1Urla2020 = this.assetType1Urla2020;
          Point absolutePosition = this.assetType1Urla2009.AbsolutePosition;
          int x1 = absolutePosition.X - 2;
          absolutePosition = this.assetType1Urla2009.AbsolutePosition;
          int y1 = absolutePosition.Y - 3;
          Point point1 = new Point(x1, y1);
          assetType1Urla2020.Position = point1;
          DropdownBox assetType2Urla2020 = this.assetType2Urla2020;
          absolutePosition = this.assetType2Urla2009.AbsolutePosition;
          int x2 = absolutePosition.X - 2;
          absolutePosition = this.assetType2Urla2009.AbsolutePosition;
          int y2 = absolutePosition.Y - 3;
          Point point2 = new Point(x2, y2);
          assetType2Urla2020.Position = point2;
          DropdownBox assetType3Urla2020 = this.assetType3Urla2020;
          absolutePosition = this.assetType3Urla2009.AbsolutePosition;
          int x3 = absolutePosition.X - 2;
          absolutePosition = this.assetType3Urla2009.AbsolutePosition;
          int y3 = absolutePosition.Y - 3;
          Point point3 = new Point(x3, y3);
          assetType3Urla2020.Position = point3;
          DropdownBox assetType4Urla2020 = this.assetType4Urla2020;
          absolutePosition = this.assetType4Urla2009.AbsolutePosition;
          int x4 = absolutePosition.X - 2;
          absolutePosition = this.assetType4Urla2009.AbsolutePosition;
          int y4 = absolutePosition.Y - 3;
          Point point4 = new Point(x4, y4);
          assetType4Urla2020.Position = point4;
          this.panelURLA2020Deposit.Position = new Point(437, 432);
          this.assetType1Urla2009.Visible = false;
          this.assetType2Urla2009.Visible = false;
          this.assetType3Urla2009.Visible = false;
          this.assetType4Urla2009.Visible = false;
          this.panelURLA2009Deposit.Visible = false;
          this.panelAUSExport.Visible = true;
        }
        else
        {
          this.assetType1Urla2020.Visible = false;
          this.assetType2Urla2020.Visible = false;
          this.assetType3Urla2020.Visible = false;
          this.assetType4Urla2020.Visible = false;
          this.panelURLA2020Deposit.Visible = false;
          this.panelAUSExport.Visible = false;
        }
        this.pnllocalAdr = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("localAdr");
        this.pnlforeignAdr = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignAdr");
        this.selectCountryButton = (StandardButton) this.currentForm.FindControl("selectcountry_DD0040");
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
        bool flag = this.GetField(this.header + this.ind + "39") == "Y";
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
      if (id == "DD" + this.ind + "24")
        this.loan.Calculator.UpdateAccountName("36");
      if (id == "DD" + this.ind + "08")
        this.loan.Calculator.UpdateAccountName("36");
      else if (id == "DD" + this.ind + "12")
        this.loan.Calculator.UpdateAccountName("36");
      else if (id == "DD" + this.ind + "16")
      {
        this.loan.Calculator.UpdateAccountName("36");
      }
      else
      {
        if (!(id == "DD" + this.ind + "20"))
          return;
        this.loan.Calculator.UpdateAccountName("36");
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      if (id == this.header + this.ind + "40" && this.loan.GetSimpleField(this.header + this.ind + "39") != "Y")
        controlState = ControlState.Disabled;
      return controlState;
    }

    public new event VerifSummaryChangedEventHandler VerifSummaryChanged;

    public new void SummaryChanged(string fieldId, string newValue)
    {
      if (this.VerifSummaryChanged == null)
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) newValue));
    }

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onkeyup(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement) || !(controlForElement.Field.FieldID == "DD0002"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(controlForElement.Field.FieldID, (object) controlForElement.Value));
    }

    public override void onfocusout(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onfocusout(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return;
      string fieldId = controlForElement.Field.FieldID;
      if (!(fieldId == "DD0011") && !(fieldId == "DD0015") && !(fieldId == "DD0019") && !(fieldId == "DD0023") && !(fieldId == "DD0048") && !(fieldId == "DD0049") && !(fieldId == "DD0050") && !(fieldId == "DD0051"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) controlForElement.Value));
    }

    public override bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      bool flag = base.onclick(pEvtObj);
      if (this.VerifSummaryChanged == null)
        return false;
      if (!(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return flag;
      string fieldId = controlForElement.Field.FieldID;
      if (fieldId == "DD0024")
        this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) controlForElement.Value));
      return flag;
    }
  }
}

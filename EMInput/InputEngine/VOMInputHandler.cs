// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VOMInputHandler
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
  internal class VOMInputHandler : VOLInputHandler
  {
    private EllieMae.Encompass.Forms.Panel panelOwner2020;
    private EllieMae.Encompass.Forms.Panel panelOwner2009;
    private EllieMae.Encompass.Forms.Panel panelPropInfo2009;
    private EllieMae.Encompass.Forms.Panel panelPropInfo2020;
    private EllieMae.Encompass.Forms.Panel panelForm;
    private EllieMae.Encompass.Forms.GroupBox groupBox2;
    private EllieMae.Encompass.Forms.Label vomlabel;
    private EllieMae.Encompass.Forms.Panel pnllocalAdr;
    private EllieMae.Encompass.Forms.Panel pnlforeignAdr;
    private StandardButton selectCountryButton;
    private EllieMae.Encompass.Forms.Panel foreignAdrCheckbox;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignPanels;
    private List<EllieMae.Encompass.Forms.Panel> pnlExp;

    public VOMInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public VOMInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public VOMInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public VOMInputHandler(
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
      this.header = "FM";
      this.IdId = "43";
      this.nameId = "04";
      this.title = "VOM";
    }

    internal override void CreateControls()
    {
      try
      {
        this.panelOwner2009 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelOwner2009");
        this.panelOwner2020 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelOwner2020");
        this.panelPropInfo2009 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelPropInfo2009");
        this.panelPropInfo2020 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelPropInfo2020");
        this.panelForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        this.groupBox2 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox2");
        this.vomlabel = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("vomLabel");
        this.pnlExp = new List<EllieMae.Encompass.Forms.Panel>();
        this.pnlExp.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlWithoutLockExp2020"));
        this.pnlExp.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlWithLockExp2020"));
        if (this.loan.GetField("1825") == "2020")
        {
          EllieMae.Encompass.Forms.Panel panelOwner2020 = this.panelOwner2020;
          Point absolutePosition = this.panelOwner2009.AbsolutePosition;
          int x1 = absolutePosition.X - 3;
          absolutePosition = this.panelOwner2009.AbsolutePosition;
          int y1 = absolutePosition.Y;
          Point point1 = new Point(x1, y1);
          panelOwner2020.Position = point1;
          EllieMae.Encompass.Forms.Panel panelPropInfo2020 = this.panelPropInfo2020;
          absolutePosition = this.panelPropInfo2009.AbsolutePosition;
          int x2 = absolutePosition.X - 3;
          absolutePosition = this.panelPropInfo2009.AbsolutePosition;
          int y2 = absolutePosition.Y + 20;
          Point point2 = new Point(x2, y2);
          panelPropInfo2020.Position = point2;
          this.groupBox2.Top = this.panelOwner2020.Bottom + 2;
          this.groupBox2.Left = this.panelOwner2020.Left + 2;
          this.panelOwner2009.Visible = false;
          this.panelPropInfo2009.Visible = false;
          EllieMae.Encompass.Forms.Panel panelForm = this.panelForm;
          Size size1 = this.panelForm.Size;
          int width = size1.Width;
          size1 = this.panelForm.Size;
          int height1 = size1.Height;
          size1 = this.panelOwner2020.Size;
          int height2 = size1.Height;
          int height3 = height1 + height2 + 100;
          Size size2 = new Size(width, height3);
          panelForm.Size = size2;
          EllieMae.Encompass.Forms.Label vomlabel = this.vomlabel;
          absolutePosition = this.panelForm.AbsolutePosition;
          Point point3 = new Point(absolutePosition.X, this.panelForm.Bottom + 5);
          vomlabel.Position = point3;
        }
        else
        {
          this.panelPropInfo2020.Visible = false;
          this.panelOwner2020.Visible = false;
        }
        this.panelForeignPanels = new List<EllieMae.Encompass.Forms.Panel>();
        for (int index = 1; index <= 2; ++index)
          this.panelForeignPanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignPanel" + (object) index));
        this.pnllocalAdr = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("localAdr");
        this.pnlforeignAdr = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignAdr");
        this.selectCountryButton = (StandardButton) this.currentForm.FindControl("selectcountry_FM0057");
        this.foreignAdrCheckbox = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignAdrcheckbox");
        if (this.pnlforeignAdr == null || this.pnllocalAdr == null)
          return;
        this.pnlforeignAdr.Position = this.pnllocalAdr.Position;
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
      bool flag1 = this.GetField(this.header + this.ind + "28") == "Y";
      if (this.selectCountryButton != null)
      {
        bool flag2 = this.GetField(this.header + this.ind + "58") == "Y";
        this.selectCountryButton.Visible = flag2;
        this.pnllocalAdr.Visible = !flag2;
        this.pnlforeignAdr.Visible = flag2;
      }
      if (this.foreignAdrCheckbox != null)
      {
        if (flag1)
          this.foreignAdrCheckbox.Visible = true;
        else
          this.foreignAdrCheckbox.Visible = false;
      }
      if (!(this.GetField("1825") == "2020"))
        return;
      for (int index = 0; index < 2; ++index)
        this.panelForeignPanels[index].Visible = true;
      if (this.pnlExp == null || this.pnlExp.Count < 2)
        return;
      this.pnlExp[0].Visible = !(this.pnlExp[1].Visible = flag1);
    }

    public new event VerifSummaryChangedEventHandler VerifSummaryChanged;

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onkeyup(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return;
      string fieldId = controlForElement.Field.FieldID;
      if (!(fieldId == "FM0004") && !(fieldId == "FM0006") && !(fieldId == "FM0007") && !(fieldId == "FM0008") && !(fieldId == "FM0047") && !(fieldId == "FM0048") && !(fieldId == "FM0050"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) controlForElement.Value));
    }

    public override void onfocusout(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onfocusout(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return;
      string fieldId = controlForElement.Field.FieldID;
      if (!(fieldId == "FM0004") && !(fieldId == "FM0006") && !(fieldId == "FM0007") && !(fieldId == "FM0008") && !(fieldId == "FM0047") && !(fieldId == "FM0048") && !(fieldId == "FM0050"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) controlForElement.Value));
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "attachliens":
          if (this.attachLiensToVOM())
            this.RefreshContents();
          this.SetFieldFocus("l_FM0019");
          break;
        case "selectcountry_FM0057":
          base.ExecAction(action);
          break;
      }
      if (!action.ToLower().StartsWith("selectcountry_"))
        return;
      action = "selectcountry_" + this.MapFieldId(action.Replace("selectcountry_", ""));
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

    public override bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      bool flag = base.onclick(pEvtObj);
      if (this.VerifSummaryChanged == null)
        return false;
      if (!(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return flag;
      string fieldId = controlForElement.Field.FieldID;
      if (fieldId == "FM0041" || fieldId == "FM0028")
        this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) controlForElement.Value));
      return flag;
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      string str1 = "0";
      string str2 = "";
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
        case "19":
        case "54":
          if (this.loan.GetSimpleField(str2 + "28") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "28":
          if (this.loan.GetField("1825") == "2020" && this.loan.GetField("FM0128") == "Y" && str2 != "FM01")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "37":
          if (this.loan.GetSimpleField(str2 + "38") == "Y" || this.loan.GetSimpleField(str2 + "64") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "56":
          controlState = !(this.loan.GetSimpleField(str2 + "55") == "Other") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "57":
          if (this.loan.GetSimpleField(str2 + "58") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "58":
          if (this.loan.GetField("1825") == "2020" && this.loan.GetSimpleField(str2 + "28") == "Y")
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

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id == "FM" + this.ind + "26" || id == "FM" + this.ind + "14")
      {
        double num = this.ToDouble(val);
        if (num > 100.0)
          val = "100.00";
        else if (num < 0.0)
          val = "0.00";
      }
      base.UpdateFieldValue(id, val);
      this.SetPrintingDefault(id, val);
      if (id == "FM" + this.ind + "28")
      {
        if (val == "Y")
        {
          this.loan.Calculator.UpdateAccountName("11");
        }
        else
        {
          base.UpdateFieldValue("FM" + this.ind + "04", "");
          base.UpdateFieldValue("FM" + this.ind + "06", "");
          base.UpdateFieldValue("FM" + this.ind + "07", "");
          base.UpdateFieldValue("FM" + this.ind + "08", "");
        }
        if (this.loan.GetField("FM0128") != "Y" && this.loan.GetField("1825") == "2020")
        {
          int numberOfMortgages = this.loan.GetNumberOfMortgages();
          for (int i = 2; i <= numberOfMortgages; ++i)
          {
            if (this.loan.GetField("FM" + i.ToString("00") + "28") == "Y")
            {
              this.loan.MoveMortgageToTop(i);
              break;
            }
          }
        }
      }
      if (id == "FM" + this.ind + "04")
        this.loan.Calculator.UpdateAccountName("11");
      if (id == "FM" + this.ind + "06")
        this.loan.Calculator.UpdateAccountName("11");
      if (id == "FM" + this.ind + "07")
        this.loan.Calculator.UpdateAccountName("11");
      if (id == "FM" + this.ind + "08")
        this.loan.Calculator.UpdateAccountName("11");
      if (!(id == "FM" + this.ind + "19"))
        return;
      this.loan.Calculator.UpdateAccountName("11");
    }

    private bool attachLiensToVOM()
    {
      string fieldValue1 = this.GetFieldValue("FM" + this.ind + "43");
      string fieldValue2 = this.GetFieldValue("FM" + this.ind + "46");
      bool flag = false;
      NewMortgageDialog newMortgageDialog = new NewMortgageDialog(this.loan, fieldValue1);
      if (newMortgageDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
        return false;
      string[] strArray = newMortgageDialog.SelectedVOL.Split('|');
      if (newMortgageDialog.SelectedVOL.Length > 0)
      {
        for (int index = 0; index < strArray.Length; ++index)
        {
          string fieldValue3 = this.GetFieldValue("FL" + double.Parse(strArray[index]).ToString("00") + "15");
          if (fieldValue2 != fieldValue3 && fieldValue2 != "")
            flag = true;
        }
      }
      this.loan.AttachMortgage(this.ind, newMortgageDialog.SelectedVOL);
      if (flag)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "Please review - selected liability is not associated with the current owner of the property.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      return true;
    }
  }
}

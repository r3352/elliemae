// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VORInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class VORInputHandler : InputHandlerBase
  {
    private const string className = "VORInputHandler";
    protected internal string ind;
    private bool borrower;
    private int recNumber;
    protected string brwHeader;
    protected string coBrwHeader;
    protected string header;
    protected string brwId;
    protected object[] props;
    private EllieMae.Encompass.Forms.Panel panelNewAddress;
    private EllieMae.Encompass.Forms.Panel panelOldAddress;
    private EllieMae.Encompass.Forms.GroupBox groupBox5;
    private EllieMae.Encompass.Forms.Label label18;
    private EllieMae.Encompass.Forms.Panel pnlForm;
    private FieldReformatOnUIHandler fieldReformatOnUIHandler;
    private EllieMae.Encompass.Forms.CheckBox chkLivingRentFree;
    private List<EllieMae.Encompass.Forms.Panel> panelLocalAddresses;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignAddresses;
    private List<StandardButton> selectCountryButtons;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignPanels;
    protected string IdId = "99";
    protected string nameId = "02";
    protected string title = "VOR";

    public override object Property
    {
      get => (object) this.props;
      set
      {
        this.InitIds();
        if (!this.IsDeleted && this.CurrentFieldID.Length >= 5 && this.CurrentFieldID.Substring(4) != this.brwId)
          this.FlushOutCurrentField();
        this.props = (object[]) value;
        this.borrower = (bool) this.props[0];
        this.ind = this.borrower ? this.brwHeader : this.coBrwHeader;
        this.recNumber = int.Parse(this.props[1].ToString());
        this.ind += this.recNumber.ToString("00");
        this.IsDeleted = false;
      }
    }

    protected virtual void InitIds()
    {
      this.brwHeader = "BR";
      this.coBrwHeader = "CR";
      this.header = "FR";
      this.brwId = "13";
    }

    public VORInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public VORInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public VORInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public VORInputHandler(
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
        this.fieldReformatOnUIHandler = new FieldReformatOnUIHandler(this.inputData);
        this.panelNewAddress = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelNewAddress");
        this.panelOldAddress = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelOldAddress");
        this.groupBox5 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox5");
        this.label18 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label18");
        this.pnlForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        this.chkLivingRentFree = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chkLivingRentFree");
        this.panelLocalAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        for (int index = 1; index <= 2; ++index)
        {
          this.panelLocalAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("localAdr" + (object) index));
          this.panelForeignAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignAdr" + (object) index));
          if (this.panelForeignAddresses[index - 1] != null && this.panelLocalAddresses[index - 1] != null)
            this.panelForeignAddresses[index - 1].Position = this.panelLocalAddresses[index - 1].Position;
        }
        this.selectCountryButtons = new List<StandardButton>();
        this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_FR0040"));
        this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_FR0030"));
        this.panelForeignPanels = new List<EllieMae.Encompass.Forms.Panel>();
        for (int index = 1; index <= 2; ++index)
          this.panelForeignPanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignPanel" + (object) index));
      }
      catch
      {
      }
    }

    public event VerifSummaryChangedEventHandler VerifSummaryChanged;

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onkeyup(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return;
      string fieldId = controlForElement.Field.FieldID;
      if (!(fieldId == "FR0004"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) controlForElement.Value, this.ind));
    }

    public override bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      bool flag = base.onclick(pEvtObj);
      if (this.VerifSummaryChanged == null)
        return false;
      if (!(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return flag;
      string fieldId = controlForElement.Field.FieldID;
      return flag;
    }

    public override void onfocusout(mshtml.IHTMLEventObj pEvtObj)
    {
      base.onfocusout(pEvtObj);
      if (this.VerifSummaryChanged == null || !(this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement))
        return;
      string fieldId = controlForElement.Field.FieldID;
      if (!(fieldId == "FR0013") && !(fieldId == "FR0004") && !(fieldId == "FR0026") && !(fieldId == "FR0025") && !(fieldId == "FR0027"))
        return;
      this.VerifSummaryChanged(new VerifSummaryChangeInfo(fieldId, (object) controlForElement.Value, this.ind));
    }

    public override void AddToEFolder()
    {
      if (!new eFolderAccessRights(this.session.LoanDataMgr).CanAddDocuments)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) null, "You do not have rights to add documents to the eFolder.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.GetFieldValue(this.ind + "97") == "")
      {
        int num2 = (int) Utils.Dialog((IWin32Window) null, "The document tracking list already contains this verification.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.UpdateFieldValue(this.ind + "97", "");
        this.UpdateDocTrackingForVerifs(this.ind + "97", true);
        int num3 = (int) Utils.Dialog((IWin32Window) null, "The verification has been added to document tracking list successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    protected override void FlushOutCurrentField() => base.FlushOutCurrentField();

    protected override string MapFieldId(string id)
    {
      if (id.StartsWith(this.header))
        id = id.Length <= 6 ? id.Remove(0, 4).Insert(0, this.ind) : id.Remove(0, 5).Insert(0, this.ind);
      else if (id.Length >= 4 && !id.StartsWith(this.ind))
        id = id.Length <= 6 ? id.Remove(0, 4).Insert(0, this.ind) : id.Remove(0, 5).Insert(0, this.ind);
      return id;
    }

    protected override string MapHelpKey(string helpKey)
    {
      if (helpKey.StartsWith(this.header))
        helpKey = helpKey.Remove(0, 2).Insert(0, this.borrower ? this.brwHeader : this.coBrwHeader);
      return helpKey;
    }

    protected virtual int MoveRecord(bool from, bool to, int recNumber)
    {
      return this.loan.MoveResidence(from, to, recNumber);
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id == this.ind + this.brwId)
      {
        if (this.borrower)
        {
          if (val == "CoBorrower")
          {
            this.Property = (object) new object[2]
            {
              (object) false,
              (object) this.MoveRecord(true, false, this.recNumber)
            };
            this.RefreshToolTips();
          }
        }
        else if (val == "Borrower")
        {
          this.Property = (object) new object[2]
          {
            (object) true,
            (object) this.MoveRecord(false, true, this.recNumber)
          };
          this.RefreshToolTips();
        }
      }
      else
      {
        switch (id)
        {
          case "BR0104":
          case "BR0106":
          case "BR0107":
          case "BR0108":
            base.UpdateFieldValue(id, val);
            this.loan.Calculator.UpdateAccountName("VA");
            break;
          case "CR0104":
          case "CR0106":
          case "CR0107":
          case "CR0108":
            base.UpdateFieldValue(id, val);
            this.loan.Calculator.UpdateAccountName("VA");
            break;
          default:
            if (id.EndsWith("15") && (id.StartsWith("BR") || id.StartsWith("CR") && !id.StartsWith("CRED")) && val == "N")
              val = string.Empty;
            base.UpdateFieldValue(id, val);
            break;
        }
      }
      if (this.props.Length <= 1)
        return;
      this.SetPrintingDefault(id, val);
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
        case "37":
          if (this.loan.GetSimpleField(str2 + "38") == "Y" || this.loan.GetSimpleField(str2 + "64") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "40":
          if (this.loan.GetSimpleField(str2 + "39") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "30":
          if (this.loan.GetSimpleField(str2 + "29") != "Y")
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
      if (action.ToLower().StartsWith("selectcountry_"))
        action = "selectcountry_" + this.MapFieldId(action.Replace("selectcountry_", ""));
      base.ExecAction(action);
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

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      string fieldValue = this.GetFieldValue("1825");
      if (this.panelNewAddress == null || this.panelOldAddress == null || this.groupBox5 == null || this.pnlForm == null || this.label18 == null)
        return;
      if (fieldValue == "2020")
      {
        this.panelNewAddress.Visible = true;
        this.panelOldAddress.Visible = false;
        this.panelNewAddress.Position = new Point(6, 439);
        this.groupBox5.Position = new Point(2, 617);
        this.pnlForm.Size = new Size(this.pnlForm.Size.Width, 675);
        this.label18.Position = new Point(2, 685);
        this.chkLivingRentFree.Visible = true;
      }
      else
      {
        this.panelNewAddress.Visible = false;
        this.panelOldAddress.Visible = true;
        this.panelOldAddress.Position = new Point(2, 5);
        this.groupBox5.Position = new Point(2, 543);
        this.pnlForm.Size = new Size(this.pnlForm.Size.Width, 606);
        this.label18.Position = new Point(2, 612);
        this.chkLivingRentFree.Visible = false;
      }
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.selectCountryButtons != null && this.selectCountryButtons.Count > 0)
      {
        for (int index = 0; index < this.selectCountryButtons.Count; ++index)
        {
          if (this.selectCountryButtons[index] != null)
          {
            bool flag = this.GetField(index == 0 ? this.ind + "39" : this.ind + "29") == "Y";
            this.selectCountryButtons[index].Visible = flag;
            this.panelLocalAddresses[index].Visible = !flag;
            this.panelForeignAddresses[index].Visible = flag;
          }
        }
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
  }
}

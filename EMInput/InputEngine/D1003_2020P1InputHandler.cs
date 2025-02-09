// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.D1003_2020P1InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class D1003_2020P1InputHandler : InputHandlerBase
  {
    private BorrowerPair[] pairs;
    private EllieMae.Encompass.Forms.CheckBox CheckBox62;
    private EllieMae.Encompass.Forms.CheckBox chkBorrowerNotJointly;
    private EllieMae.Encompass.Forms.CheckBox chkBorrowerJointly;
    private EllieMae.Encompass.Forms.Label lblTypeOfCredit;
    private EllieMae.Encompass.Forms.Label lblIndividualCredit;
    private EllieMae.Encompass.Forms.Label lblJointCredit;
    private EllieMae.Encompass.Forms.Panel pnlBorrowerCount;
    private HorizontalRule horizontalRule3;
    private HorizontalRule horizontalRule12;
    private EllieMae.Encompass.Forms.Panel pnlBorrowerLanguagePreference;
    private EllieMae.Encompass.Forms.Panel pnlCoBorrowerLanguagePreference;
    private EllieMae.Encompass.Forms.Panel pnlForm;
    private EllieMae.Encompass.Forms.Label labelFormName;
    private VerticalRule verticalRule4;
    private List<EllieMae.Encompass.Forms.Panel> panelLocalAddresses;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignAddresses;
    private List<StandardButton> selectCountryButtons;
    private List<ImageButton> deleteButtons;
    private List<ImageButton> mailAddressdeleteButtons;
    private string sourceEnable = string.Empty;
    private string sourceDisable = string.Empty;
    private Dictionary<string, bool> controlWithPermission = new Dictionary<string, bool>();
    private List<string> borrMailAddress = new List<string>()
    {
      "URLA.X267",
      "URLA.X197",
      "URLA.X7",
      "URLA.X8",
      "1417",
      "1418",
      "1419",
      "URLA.X269"
    };
    private List<string> coBorrMailAddress = new List<string>()
    {
      "URLA.X268",
      "URLA.X198",
      "URLA.X9",
      "URLA.X10",
      "1520",
      "1521",
      "1522",
      "URLA.X270"
    };

    public D1003_2020P1InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public D1003_2020P1InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public D1003_2020P1InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public D1003_2020P1InputHandler(
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
        this.pairs = this.loan.GetBorrowerPairs();
        this.CheckBox62 = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("CheckBox62");
        this.chkBorrowerNotJointly = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("CheckBox4");
        this.chkBorrowerJointly = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("CheckBox5");
        this.lblTypeOfCredit = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label4");
        this.lblIndividualCredit = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label5");
        this.lblJointCredit = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label6");
        this.pnlBorrowerCount = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("Panel9");
        this.horizontalRule3 = (HorizontalRule) this.currentForm.FindControl("HorizontalRule3");
        this.horizontalRule12 = (HorizontalRule) this.currentForm.FindControl("HorizontalRule12");
        if (!this.session.SessionObjects.StartupInfo.CollectLanguagePreference)
        {
          this.pnlBorrowerLanguagePreference = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("Panel4");
          this.pnlCoBorrowerLanguagePreference = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("Panel8");
          this.verticalRule4 = (VerticalRule) this.currentForm.FindControl("VerticalRule4");
          this.pnlBorrowerLanguagePreference.Visible = false;
          this.pnlCoBorrowerLanguagePreference.Visible = false;
          this.pnlForm = this.currentForm.FindControl("pnlForm") as EllieMae.Encompass.Forms.Panel;
          this.labelFormName = this.currentForm.FindControl("Label277") as EllieMae.Encompass.Forms.Label;
          this.pnlForm.Size = new Size(this.pnlForm.Size.Width, this.pnlForm.Size.Height - 125);
          VerticalRule verticalRule4 = this.verticalRule4;
          Size size1 = this.verticalRule4.Size;
          int width = size1.Width;
          size1 = this.verticalRule4.Size;
          int height = size1.Height - 125;
          Size size2 = new Size(width, height);
          verticalRule4.Size = size2;
          EllieMae.Encompass.Forms.Label labelFormName = this.labelFormName;
          size1 = this.pnlForm.Size;
          int num = size1.Height + 6;
          labelFormName.Top = num;
        }
        this.panelLocalAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        for (int index = 1; index <= 6; ++index)
        {
          this.panelLocalAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("curLocalAdrBor" + (object) index));
          this.panelForeignAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("curForeignAdrBor" + (object) index));
          if (this.panelForeignAddresses[index - 1] != null && this.panelLocalAddresses[index - 1] != null)
            this.panelForeignAddresses[index - 1].Position = this.panelLocalAddresses[index - 1].Position;
        }
        this.selectCountryButtons = new List<StandardButton>();
        for (int index = 1; index <= 4; ++index)
          this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_FR0" + (object) index + "30"));
        this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_URLAX269"));
        this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_URLAX270"));
        this.deleteButtons = new List<ImageButton>();
        this.deleteButtons.Add((ImageButton) this.currentForm.FindControl("Ibcaborr"));
        this.deleteButtons.Add((ImageButton) this.currentForm.FindControl("IbcaborrDis"));
        this.deleteButtons.Add((ImageButton) this.currentForm.FindControl("Ibcacoborr"));
        this.deleteButtons.Add((ImageButton) this.currentForm.FindControl("IbcacoborrDis"));
        this.deleteButtons.Add((ImageButton) this.currentForm.FindControl("Ibfaborr"));
        this.deleteButtons.Add((ImageButton) this.currentForm.FindControl("IbfaborrDis"));
        this.deleteButtons.Add((ImageButton) this.currentForm.FindControl("Ibfacoborr"));
        this.deleteButtons.Add((ImageButton) this.currentForm.FindControl("IbfacoborrDis"));
        this.mailAddressdeleteButtons = new List<ImageButton>();
        this.mailAddressdeleteButtons.Add((ImageButton) this.currentForm.FindControl("Ibmaborr"));
        this.mailAddressdeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisIbmaborr"));
        this.mailAddressdeleteButtons.Add((ImageButton) this.currentForm.FindControl("Ibmacoborr"));
        this.mailAddressdeleteButtons.Add((ImageButton) this.currentForm.FindControl("DisIbmacoborr"));
        if (this.deleteButtons.Count >= 2)
        {
          this.sourceEnable = this.deleteButtons[0].Source;
          this.sourceDisable = this.deleteButtons[1].Source;
        }
        string empty = string.Empty;
        if (this.deleteButtons.Count >= 8)
        {
          for (int index = 0; index < 8; index += 2)
          {
            BizRule.FieldAccessRight fieldAccessRights = this.session.LoanDataMgr.GetFieldAccessRights("Button_" + this.deleteButtons[index].Action);
            this.controlWithPermission.Add(this.deleteButtons[index].Action, fieldAccessRights == BizRule.FieldAccessRight.ViewOnly);
          }
        }
        if (this.mailAddressdeleteButtons.Count < 4)
          return;
        for (int index = 0; index < 4; index += 2)
        {
          BizRule.FieldAccessRight fieldAccessRights = this.session.LoanDataMgr.GetFieldAccessRights("Button_" + this.mailAddressdeleteButtons[index].Action);
          this.controlWithPermission.Add(this.mailAddressdeleteButtons[index].Action, fieldAccessRights == BizRule.FieldAccessRight.ViewOnly);
        }
      }
      catch
      {
      }
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      if (this.loan == null)
        return;
      this.rearrangeLayout();
    }

    private void rearrangeLayout()
    {
      if (this.pairs.Length >= 1 && this.loan.CurrentBorrowerPair.Id == this.pairs[0].Id)
      {
        this.pnlBorrowerCount.Position = new Point(6, 303);
        this.horizontalRule3.Position = new Point(0, 350);
        this.horizontalRule12.Position = new Point(0, 350);
        this.chkBorrowerNotJointly.Visible = true;
        this.chkBorrowerJointly.Visible = true;
        this.lblTypeOfCredit.Visible = true;
        this.lblIndividualCredit.Visible = true;
        this.lblJointCredit.Visible = true;
      }
      else
      {
        this.pnlBorrowerCount.Position = new Point(6, 280);
        this.horizontalRule3.Position = new Point(0, 348);
        this.horizontalRule12.Position = new Point(0, 348);
        this.chkBorrowerNotJointly.Visible = false;
        this.chkBorrowerJointly.Visible = false;
        this.lblTypeOfCredit.Visible = false;
        this.lblIndividualCredit.Visible = false;
        this.lblJointCredit.Visible = false;
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1416":
          return controlState;
        case "1519":
        case "URLA.X10":
        case "URLA.X12":
        case "URLA.X198":
        case "URLA.X268":
        case "URLA.X9":
          controlState = !(this.loan.GetField("1820") == "Y") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1416";
        case "4920":
          return this.loan.GetField("1490") == string.Empty ? ControlState.Disabled : ControlState.Enabled;
        case "4935":
          return this.loan.GetField("1480") == string.Empty ? ControlState.Disabled : ControlState.Enabled;
        case "FR0116":
          if (this.loan.GetField("FR0115") != "Rent")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "FR0130":
          if (this.loan.GetField("FR0129") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "FR0216":
          if (this.loan.GetField("FR0215") != "Rent")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "FR0230":
          if (this.loan.GetField("FR0229") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "FR0304":
        case "FR0306":
        case "FR0307":
        case "FR0308":
        case "FR0312":
        case "FR0315":
        case "FR0324":
        case "FR0325":
        case "FR0326":
        case "FR0327":
        case "FR0328":
        case "FR0329":
          controlState = !(this.loan.GetField("URLA.X265") == "Y") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1416";
        case "FR0316":
          if (this.loan.GetField("FR0315") != "Rent")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "FR0330":
          if (this.loan.GetField("FR0329") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "FR0404":
        case "FR0406":
        case "FR0407":
        case "FR0408":
        case "FR0412":
        case "FR0415":
        case "FR0424":
        case "FR0425":
        case "FR0426":
        case "FR0427":
        case "FR0428":
        case "FR0429":
          controlState = !(this.loan.GetField("URLA.X266") == "Y") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1416";
        case "FR0416":
          if (this.loan.GetField("FR0415") != "Rent")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "FR0430":
          if (this.loan.GetField("FR0429") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "URLA.X11":
        case "URLA.X197":
        case "URLA.X267":
        case "URLA.X7":
        case "URLA.X8":
          controlState = !(this.loan.GetField("1819") == "Y") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1416";
        case "URLA.X111":
        case "URLA.X113":
        case "URLA.X117":
          if (this.GetField("52") != "Unmarried")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "URLA.X112":
        case "URLA.X114":
        case "URLA.X118":
          if (this.GetField("84") != "Unmarried")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "URLA.X115":
          if (this.GetField("URLA.X113") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "URLA.X116":
          if (this.GetField("URLA.X114") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "URLA.X123":
        case "URLA.X124":
        case "URLA.X125":
          if (this.GetField("URLA.X13") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "URLA.X126":
        case "URLA.X127":
        case "URLA.X128":
          if (this.GetField("URLA.X14") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "URLA.X17":
          if (this.GetField("URLA.X123") != "Y")
          {
            this.SetControlState("Calendar1", false);
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
          {
            this.SetControlState("Calendar1", true);
            controlState = ControlState.Enabled;
            goto case "1416";
          }
        case "URLA.X18":
          if (this.GetField("URLA.X126") != "Y")
          {
            this.SetControlState("Calendar2", false);
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
          {
            this.SetControlState("Calendar2", true);
            controlState = ControlState.Enabled;
            goto case "1416";
          }
        case "URLA.X234":
          controlState = ControlState.Disabled;
          goto case "1416";
        case "URLA.X263":
          if (this.GetField("URLA.X1") != "NonPermanentResidentAlien")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "URLA.X264":
          if (this.GetField("URLA.X2") != "NonPermanentResidentAlien")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "URLA.X265":
          controlState = !this.VerifExists(true, false) || !(this.loan.GetField("URLA.X265") != "Y") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1416";
        case "URLA.X266":
          controlState = !this.VerifExists(false, false) || !(this.loan.GetField("URLA.X266") != "Y") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1416";
        case "URLA.X269":
          if (this.loan.GetField("1819") == "Y" || this.loan.GetField("URLA.X267") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "URLA.X270":
          if (this.loan.GetField("1820") == "Y" || this.loan.GetField("URLA.X268") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "URLA.X35":
          controlState = !(this.loan.GetField("URLA.X21") != "OtherIndicator") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1416";
        case "URLA.X36":
          controlState = !(this.loan.GetField("URLA.X22") != "OtherIndicator") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1416";
        case "copymailadd":
          if (this.inputData.GetField("1820") == "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "showalternatenamesBorrower":
        case "showalternatenamesCoBorrower":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "vor":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        default:
          controlState = ControlState.Default;
          goto case "1416";
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
            bool flag = this.GetField(index < 4 ? "FR0" + (object) (index + 1) + "29" : (index == 4 ? "URLA.X267" : "URLA.X268")) == "Y";
            this.selectCountryButtons[index].Visible = flag;
            this.panelLocalAddresses[index].Visible = !flag;
            this.panelForeignAddresses[index].Visible = flag;
            if ((this.panelLocalAddresses[index].ControlID == "curLocalAdrBor5" || this.panelLocalAddresses[index].ControlID == "curLocalAdrBor6") && this.panelLocalAddresses[index].Visible)
            {
              if (this.panelLocalAddresses[index].ControlID == "curLocalAdrBor5")
                this.setMailingCityStateZipFieldStatus(new string[3]
                {
                  "TextBox25",
                  "TextBox26",
                  "l_1419"
                }, this.GetField("1819") != "Y");
              else
                this.setMailingCityStateZipFieldStatus(new string[3]
                {
                  "TextBox18",
                  "TextBox65",
                  "I_1522"
                }, this.GetField("1820") != "Y");
            }
            if ((this.panelForeignAddresses[index].ControlID == "curForeignAdrBor5" || this.panelForeignAddresses[index].ControlID == "curForeignAdrBor6") && this.panelForeignAddresses[index].Visible)
            {
              if (this.panelForeignAddresses[index].ControlID == "curForeignAdrBor5")
                this.setMailingCityStateZipFieldStatus(new string[3]
                {
                  "TextBox89",
                  "TextBox90",
                  "TextBox91"
                }, this.GetField("1819") != "Y");
              else
                this.setMailingCityStateZipFieldStatus(new string[3]
                {
                  "TextBox92",
                  "TextBox93",
                  "TextBox94"
                }, this.GetField("1820") != "Y");
            }
          }
        }
      }
      if (this.loan != null && this.loan.IsInFindFieldForm)
      {
        for (int index = 0; index < this.deleteButtons.Count; index += 2)
          this.deleteButtons[index].Source = this.sourceEnable;
        for (int index = 0; index < this.mailAddressdeleteButtons.Count; index += 2)
          this.mailAddressdeleteButtons[index].Source = this.sourceEnable;
      }
      else
      {
        if (this.deleteButtons != null && this.deleteButtons.Count >= 8)
        {
          int numberOfResidence1 = this.loan.GetNumberOfResidence(true);
          int numberOfResidence2 = this.loan.GetNumberOfResidence(false);
          this.EnableDisableControl(this.deleteButtons[0], this.controlWithPermission, this.VerifExists(true, true, numberOfResidence1), this.sourceEnable, this.sourceDisable);
          this.EnableDisableControl(this.deleteButtons[2], this.controlWithPermission, this.VerifExists(false, true, numberOfResidence2), this.sourceEnable, this.sourceDisable);
          this.EnableDisableControl(this.deleteButtons[4], this.controlWithPermission, this.VerifExists(true, false, numberOfResidence1), this.sourceEnable, this.sourceDisable);
          this.EnableDisableControl(this.deleteButtons[6], this.controlWithPermission, this.VerifExists(false, false, numberOfResidence2), this.sourceEnable, this.sourceDisable);
        }
        if (this.mailAddressdeleteButtons == null || this.mailAddressdeleteButtons.Count < 4)
          return;
        this.EnableDisableControl(this.mailAddressdeleteButtons[0], this.controlWithPermission, this.MailAddressExists(true) && this.GetField("1819") != "Y", this.sourceEnable, this.sourceDisable);
        this.EnableDisableControl(this.mailAddressdeleteButtons[2], this.controlWithPermission, this.MailAddressExists(false) && this.GetField("1820") != "Y", this.sourceEnable, this.sourceDisable);
      }
    }

    private void setMailingCityStateZipFieldStatus(string[] controlIDs, bool enabled)
    {
      for (int index = 0; index < controlIDs.Length; ++index)
      {
        if (enabled)
          this.SetFieldEnabled(controlIDs[index]);
        else
          this.SetFieldDisabled(controlIDs[index]);
      }
    }

    private bool MailAddressExists(bool isBorrower)
    {
      foreach (string id in isBorrower ? this.borrMailAddress : this.coBorrMailAddress)
      {
        if (!string.IsNullOrEmpty(this.loan.GetField(id)))
          return true;
      }
      return false;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      switch (id)
      {
        case "1418":
        case "1521":
        case "FR0107":
        case "FR0207":
        case "FR0307":
        case "FR0407":
        case "URLA.X117":
        case "URLA.X118":
          if (!StateNameValidator.ValidateState(id, val, (InputHandlerBase) this))
          {
            val = "";
            break;
          }
          break;
        case "FR0128":
        case "FR0228":
        case "FR0328":
        case "FR0428":
        case "URLA.X11":
        case "URLA.X12":
          if (!(val == ""))
          {
            if (string.Compare(val, "US", true) != 0)
            {
              int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Only US addresses are currently supported by URLA.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            val = "US";
            break;
          }
          break;
      }
      base.UpdateFieldValue(id, val);
    }

    private void RemoveVor(bool isBorrower, bool currentResType)
    {
      if (Utils.Dialog((IWin32Window) this.session.MainForm, "Are you sure you want to delete this record from the verification?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      this.loan.RemoveResidenceAt(isBorrower, this.VerificationIndex(isBorrower, currentResType));
      string id = isBorrower ? "1819" : "1820";
      if (currentResType && this.loan.GetField(id) == "Y")
        this.RemoveMailAddress(isBorrower, false);
      this.RefreshContents();
    }

    private int VerificationIndex(bool isBorrower, bool currentResType, int numRes = -1)
    {
      string str1 = isBorrower ? "BR" : "CR";
      string str2 = currentResType ? "Current" : "Prior";
      if (numRes == -1)
        numRes = this.loan.GetNumberOfResidence(isBorrower);
      for (int index = 1; index <= numRes; ++index)
      {
        if (this.inputData.GetField(str1 + index.ToString("00") + "23") == str2)
          return index - 1;
      }
      return -1;
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "Ibcaborr":
          this.RemoveVor(true, true);
          break;
        case "Ibcacoborr":
          this.RemoveVor(false, true);
          break;
        case "Ibfaborr":
          this.RemoveVor(true, false);
          break;
        case "Ibfacoborr":
          this.RemoveVor(false, false);
          break;
        case "copybrw":
          base.ExecAction(action);
          this.SetFieldFocus("l_68");
          break;
        case "copyformeradd":
          base.ExecAction(action);
          this.SetFieldFocus("I_FR0404");
          break;
        case "copymailadd":
          base.ExecAction(action);
          this.SetFieldFocus("I_1519");
          break;
        case "maborr":
          this.RemoveMailAddress(true);
          break;
        case "macoborr":
          this.RemoveMailAddress(false);
          break;
        case "mipff":
          base.ExecAction(action);
          this.SetFieldFocus("l_3");
          break;
        case "subfin":
          base.ExecAction(action);
          this.SetFieldFocus("I_8");
          break;
        case "voe":
        case "vor":
          base.ExecAction(action);
          break;
        case "zoomarm":
          ARMTypeDetails armTypeDetails = new ARMTypeDetails("995", this.loan.GetSimpleField("995"));
          if (armTypeDetails.ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
          {
            this.loan.SetCurrentField("995", armTypeDetails.ArmTypeID);
            this.UpdateContents();
          }
          this.SetFieldFocus("l_995");
          break;
        default:
          base.ExecAction(action);
          break;
      }
    }

    private void RemoveMailAddress(bool isBorrower, bool bypass = true)
    {
      if (bypass && Utils.Dialog((IWin32Window) this.session.MainForm, "Are you sure you want to delete this record from the verification?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      foreach (string id in isBorrower ? this.borrMailAddress : this.coBorrMailAddress)
        this.loan.SetField(id, string.Empty);
    }

    private bool VerifExists(bool isBorrower, bool currentResType, int numRec = -1)
    {
      return this.VerificationIndex(isBorrower, currentResType, numRec) >= 0;
    }
  }
}

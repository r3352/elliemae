// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.D1003_2020P5InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

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
  public class D1003_2020P5InputHandler : InputHandlerBase
  {
    private StandardButton btnEditARM;
    private StandardButton btnEditNeg;
    private StandardButton btnEditBuydown;
    private EllieMae.Encompass.Forms.GroupBox groupARM;
    private EllieMae.Encompass.Forms.GroupBox groupNegARM;
    private EllieMae.Encompass.Forms.GroupBox groupBuydown;
    private EllieMae.Encompass.Forms.Panel pnlBuydownBor;
    private List<EllieMae.Encompass.Forms.GroupBox> popups;
    private string lienPos;
    private EllieMae.Encompass.Forms.GroupBox groupBox5;
    private EllieMae.Encompass.Forms.Panel pnlForm;
    private EllieMae.Encompass.Forms.Label labelFormName;
    private EllieMae.Encompass.Forms.Panel pnlItemINoHeloc;
    private EllieMae.Encompass.Forms.Panel pnlItemIHeloc;
    private EllieMae.Encompass.Forms.Label labelItemM7;
    private EllieMae.Encompass.Forms.Panel pnlItemM7;
    private EllieMae.Encompass.Forms.Panel pnlISectionMVersion1;
    private EllieMae.Encompass.Forms.Panel pnlISectionMVersion2;
    private EllieMae.Encompass.Forms.Panel pnlISectionN;
    private EllieMae.Encompass.Forms.Label labelSectionLversion1;
    private EllieMae.Encompass.Forms.Label labelSectionLversion2;
    private EllieMae.Encompass.Forms.Label labelM11;
    private EllieMae.Encompass.Forms.Panel pnlBorPaidFees;
    private EllieMae.Encompass.Forms.Panel pnlBorPaidFeesNew;
    private FieldLock lock_1851;
    private FieldLock lock2_1851;
    private EllieMae.Encompass.Forms.TextBox input_f_1851;
    private EllieMae.Encompass.Forms.TextBox input2_f_1851;
    private Calendar calIndexDate;

    public D1003_2020P5InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public D1003_2020P5InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public D1003_2020P5InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public D1003_2020P5InputHandler(
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
        this.btnEditARM = (StandardButton) this.currentForm.FindControl("btnEditARM");
        this.btnEditNeg = (StandardButton) this.currentForm.FindControl("btnEditNeg");
        this.btnEditBuydown = (StandardButton) this.currentForm.FindControl("btnEditBuydown");
        this.popups = new List<EllieMae.Encompass.Forms.GroupBox>();
        this.groupARM = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupARM");
        this.groupARM.Left = this.btnEditARM.Left + 50;
        this.groupARM.Top = this.btnEditARM.Top - this.groupARM.Size.Height / 2;
        this.groupARM.Visible = false;
        this.popups.Add(this.groupARM);
        this.groupNegARM = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupNegARM");
        this.groupNegARM.Left = this.btnEditNeg.AbsolutePosition.X + 50;
        this.groupNegARM.Top = this.btnEditNeg.AbsolutePosition.Y - this.groupNegARM.Size.Height / 2 - 40;
        this.groupNegARM.Visible = false;
        this.popups.Add(this.groupNegARM);
        this.groupBuydown = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBuydown");
        this.groupBuydown.Left = this.btnEditBuydown.AbsolutePosition.X + 20;
        this.groupBuydown.Top -= 50;
        this.groupBuydown.Visible = false;
        this.popups.Add(this.groupBuydown);
        this.pnlBuydownBor = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlBuydownBor");
        this.pnlBuydownBor.Left = 11;
        this.pnlBuydownBor.Top = 72;
        this.groupBox5 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox5");
        this.pnlForm = this.currentForm.FindControl("pnlForm") as EllieMae.Encompass.Forms.Panel;
        this.labelFormName = this.currentForm.FindControl("Label163") as EllieMae.Encompass.Forms.Label;
        if (!this.session.SessionObjects.StartupInfo.CollectHomeownershipCounseling)
        {
          this.groupBox5.Visible = false;
          this.pnlForm.Size = new Size(this.pnlForm.Size.Width, this.pnlForm.Size.Height - this.groupBox5.Size.Height);
          this.labelFormName.Top = this.pnlForm.Size.Height + 6;
        }
        this.pnlItemINoHeloc = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlItemINoHeloc");
        this.pnlItemIHeloc = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlItemIHeloc");
        this.pnlItemIHeloc.Left = 1;
        this.pnlItemIHeloc.Top = 320;
        this.pnlItemM7 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlItemM7");
        this.labelItemM7 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label_M7");
        this.labelM11 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label315");
        this.pnlISectionMVersion1 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("MSectionVersion1");
        this.pnlISectionMVersion2 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("MSectionVersion2");
        this.pnlISectionN = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("SectionN");
        this.labelSectionLversion1 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("SectionLRespaVersion1");
        this.labelSectionLversion2 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("SectionLRespaVersion2");
        if (this.pnlISectionMVersion1 != null && this.pnlISectionMVersion2 != null)
          this.pnlISectionMVersion2.Position = this.pnlISectionMVersion1.Position;
        if (this.labelSectionLversion1 != null && this.labelSectionLversion2 != null)
          this.labelSectionLversion2.Position = this.labelSectionLversion1.Position;
        this.pnlBorPaidFees = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("BorPaidFeesPanel");
        this.pnlBorPaidFeesNew = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("BorPaidFeesNewPanel");
        this.lock_1851 = (FieldLock) this.currentForm.FindControl("lock_1851");
        this.lock2_1851 = (FieldLock) this.currentForm.FindControl("lock2_1851");
        this.input_f_1851 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("input_f_1851");
        this.input2_f_1851 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("input2_f_1851");
        this.calIndexDate = (Calendar) this.currentForm.FindControl("Calendar7");
      }
      catch
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1034":
          if (this.GetField("1066") != "Leasehold")
          {
            controlState = ControlState.Disabled;
            this.SetControlState("Calendar1", false);
            break;
          }
          this.SetControlState("Calendar1", true);
          break;
        case "1063":
          controlState = !(this.GetField("1172") != "Other") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "1134":
          if (this.loan.GetField("19") == "Purchase")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "1964":
        case "Constr.Refi":
          if (this.inputData.GetField("19") != "ConstructionOnly" && this.inputData.GetField("19") != "ConstructionToPermanent")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "1stmor":
          this.lienPos = this.loan.GetField("420");
          if (!(this.lienPos == "SecondLien") && !(this.lienPos == "Other"))
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "205":
        case "24":
        case "25":
        case "29":
        case "299":
        case "30":
        case "URLA.X165":
        case "URLA.X166":
          controlState = this.GetField("19") == "Purchase" || this.GetField("19") == "Other" || this.loan.GetField("19") == "" ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "4084":
          if (this.inputData.GetField("19") != "ConstructionOnly")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "424":
          if (this.GetField("URLA.X239") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "4494":
          if (this.GetField("420") != "SecondLien")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "4645":
        case "buydowntypelookup":
          if (this.inputData.GetField("CASASRN.X141") == "")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "4796":
          if (this.inputData.GetField("4796") == "Y")
          {
            this.pnlISectionMVersion2.Visible = true;
            this.pnlISectionMVersion1.Visible = false;
            this.pnlISectionN.Position = new Point(0, 816);
            this.groupBox5.Position = new Point(2, 3145);
            this.enableOrDisableDropdownPanels(this.pnlBorPaidFeesNew, this.pnlBorPaidFees);
            this.labelSectionLversion2.Visible = true;
            this.labelSectionLversion1.Visible = false;
            if (this.groupBox5.Visible)
              this.pnlForm.Size = new Size(this.pnlForm.Size.Width, this.groupBox5.Top + this.groupBox5.Size.Height + 20);
            else
              this.pnlForm.Size = new Size(this.pnlForm.Size.Width, 3148);
          }
          else
          {
            this.pnlISectionMVersion1.Visible = true;
            this.pnlISectionMVersion2.Visible = false;
            this.pnlISectionN.Position = new Point(0, 672);
            this.groupBox5.Position = new Point(2, 2992);
            this.enableOrDisableDropdownPanels(this.pnlBorPaidFees, this.pnlBorPaidFeesNew);
            this.pnlBorPaidFees.Position = new Point(46, 2742);
            this.labelSectionLversion2.Visible = false;
            this.labelSectionLversion1.Visible = true;
            if (this.groupBox5.Visible)
              this.pnlForm.Size = new Size(this.pnlForm.Size.Width, this.groupBox5.Top + this.groupBox5.Size.Height + 20);
            else
              this.pnlForm.Size = new Size(this.pnlForm.Size.Width, 2998);
          }
          this.labelFormName.Position = new Point(2, this.pnlForm.Size.Height + 5);
          break;
        case "4898":
          if (this.loan.IsTemplate)
          {
            controlState = ControlState.Disabled;
            this.calIndexDate.Enabled = false;
            break;
          }
          break;
        case "5015":
          controlState = !(this.inputData.GetField("Constr.Refi") == "Y") || !(this.inputData.GetField("19") == "ConstructionOnly") && !(this.inputData.GetField("19") == "ConstructionToPermanent") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "9":
          controlState = !(this.GetField("19") != "Other") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "995":
          if (this.GetField("608") != "AdjustableRate" && this.inputData.GetField("1172") != "HELOC")
          {
            controlState = ControlState.Disabled;
            this.SetControlState("StandardButton10", false);
            break;
          }
          controlState = ControlState.Enabled;
          this.SetControlState("StandardButton10", true);
          break;
        case "QM.X2":
          string field = this.GetField("19");
          if (field != "NoCash-Out Refinance" && field != "Cash-Out Refinance")
          {
            controlState = ControlState.Disabled;
            this.SetControlState("FieldLock21", false);
            break;
          }
          controlState = ControlState.Enabled;
          this.SetControlState("FieldLock21", true);
          break;
        case "Terms.USDAGovtType":
          controlState = !(this.GetField("1172") != "FarmersHomeAdministration") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "URLA.X133":
          if (this.GetField("19") == "ConstructionToPermanent")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X134":
          if (this.GetField("URLA.X133") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X138":
          if (this.GetField("URLA.X138") != "Other")
          {
            this.SetControlState("FieldLock23", false);
            break;
          }
          this.SetControlState("FieldLock23", true);
          break;
        case "URLA.X142":
          if (this.GetField("URLA.X141") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLA.X143":
          controlState = !(this.GetField("URLA.X242") != "Y") ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "URLA.X154":
          if (this.GetField("URLA.X153") != "Y")
          {
            this.SetControlState("pnlHOSBorr", false);
            this.SetControlState("StandardButton12", false);
            break;
          }
          this.SetControlState("pnlHOSBorr", true);
          this.SetControlState("StandardButton12", true);
          break;
        case "URLA.X160":
          if (this.GetField("URLA.X159") != "Y")
          {
            this.SetControlState("pnlHOSCoBorr", false);
            this.SetControlState("StandardButton13", false);
            break;
          }
          this.SetControlState("pnlHOSCoBorr", true);
          this.SetControlState("StandardButton13", true);
          break;
        case "URLA.X167":
          controlState = this.GetField("URLA.X166") != "Other" || this.GetField("19") == "Purchase" || this.GetField("19") == "Other" || this.loan.GetField("19") == "" ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "URLA.X301":
          if (this.GetField("URLA.X299") != "Y")
          {
            this.SetControlState("pnlHEducationBor", false);
            break;
          }
          this.SetControlState("pnlHEducationBor", true);
          break;
        case "URLA.X302":
          if (this.GetField("URLA.X300") != "Y")
          {
            this.SetControlState("pnlHEducationCoBor", false);
            break;
          }
          this.SetControlState("pnlHEducationCoBor", true);
          break;
        case "URLA.X76":
          if (this.GetFieldValue("1172") != "FHA")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "editnegarm":
          controlState = this.GetField("URLA.X239") == "Y" ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "getindex":
          if (this.loan.IsTemplate)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "otherf":
          this.lienPos = this.loan.GetField("420");
          if (!(this.lienPos == "FirstLien") && !(this.lienPos == string.Empty))
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "voal":
          if (this.FormIsForTemplate)
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
      switch (action)
      {
        case "editarm":
          if (this.groupARM == null)
            break;
          this.setPopupInVisible(this.groupARM);
          break;
        case "editarmclear":
          if (this.groupARM == null)
            break;
          this.groupARM.Visible = false;
          break;
        case "editnegarm":
          if (this.groupNegARM == null)
            break;
          this.setPopupInVisible(this.groupNegARM);
          break;
        case "editnegarmclear":
          if (this.groupNegARM == null)
            break;
          this.groupNegARM.Visible = false;
          break;
        case "editbuydown":
          if (this.groupBuydown == null)
            break;
          this.setPopupInVisible(this.groupBuydown);
          break;
        case "editbuydownclear":
          if (this.groupBuydown == null)
            break;
          this.groupBuydown.Visible = false;
          break;
        default:
          base.ExecAction(action);
          break;
      }
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.groupBuydown.Visible && this.pnlBuydownBor != null)
        this.pnlBuydownBor.Visible = !(this.inputData.GetField("CASASRN.X141") == "Borrower");
      if (this.inputData.GetField("1172") == "HELOC")
      {
        this.pnlItemIHeloc.Visible = true;
        this.pnlItemINoHeloc.Visible = false;
      }
      else
      {
        this.pnlItemIHeloc.Visible = false;
        this.pnlItemINoHeloc.Visible = true;
      }
      LinkSyncType linkSyncType = LinkSyncType.None;
      if (this.loan.LinkedData != null)
        linkSyncType = this.loan.LinkSyncType;
      if (this.loan == null)
        return;
      if (this.loan.LinkedData != null && (this.loan.LinkSyncType == LinkSyncType.PiggybackPrimary || this.loan.LinkSyncType == LinkSyncType.PiggybackLinked))
      {
        this.lock_1851.Visible = this.lock2_1851.Visible = true;
        if (this.lock_1851.ControlToLock == null)
        {
          this.lock_1851.ControlToLock = (EllieMae.Encompass.Forms.Control) this.input_f_1851;
          if (!this.loan.IsLocked("1851"))
            this.input_f_1851.Enabled = false;
          this.lock_1851.DisplayImage(this.loan.IsLocked("1851"));
        }
        if (this.lock2_1851.ControlToLock == null)
        {
          this.lock2_1851.ControlToLock = (EllieMae.Encompass.Forms.Control) this.input2_f_1851;
          if (!this.loan.IsLocked("1851"))
            this.input2_f_1851.Enabled = false;
          this.lock2_1851.DisplayImage(this.loan.IsLocked("1851"));
        }
      }
      else
      {
        if (this.loan.IsLocked("1851"))
          this.loan.RemoveLock("1851");
        this.lock_1851.Visible = this.lock2_1851.Visible = false;
        if (this.lock_1851.ControlToLock != null)
          this.lock_1851.ControlToLock = (EllieMae.Encompass.Forms.Control) null;
        this.input_f_1851.Enabled = true;
        if (this.lock2_1851.ControlToLock != null)
          this.lock2_1851.ControlToLock = (EllieMae.Encompass.Forms.Control) null;
        this.input2_f_1851.Enabled = true;
      }
      if (this.loan.IsFieldEditable("LOCKBUTTON_1851"))
        return;
      this.lock2_1851.Enabled = this.lock_1851.Enabled = false;
    }

    public override void RefreshContents()
    {
      base.RefreshContents();
      if (!this.groupBuydown.Visible || this.pnlBuydownBor == null)
        return;
      this.pnlBuydownBor.Visible = !(this.inputData.GetField("CASASRN.X141") == "Borrower");
    }

    private void setPopupInVisible(EllieMae.Encompass.Forms.GroupBox boxInAction)
    {
      foreach (EllieMae.Encompass.Forms.GroupBox popup in this.popups)
      {
        if (popup == boxInAction)
          popup.Visible = true;
        else
          popup.Visible = false;
      }
      this.RefreshContents();
    }
  }
}

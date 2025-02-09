// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LOANESTIMATEPAGE1InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LOANESTIMATEPAGE1InputHandler : InputHandlerBase
  {
    private List<EllieMae.Encompass.Forms.Panel> panelLocalAddresses;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignAddresses;
    private List<StandardButton> selectCountryButtons;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignPanels;
    private LoanTermTableInputHandler loanTermTableInputHandler;
    private DropdownBox ddRateLockExpTimezone;
    private DropdownBox ddClosingCostExpTimezone;
    private EllieMae.Encompass.Forms.TextBox otherLoanType;
    private EllieMae.Encompass.Forms.TextBox lockedDate;
    private EllieMae.Encompass.Forms.TextBox lockExpDate;
    private Calendar calLockedDate;
    private Calendar calLockExpDate;
    private FieldLock fieldLockRateExpDate;
    private FieldLock fieldLockRateExpTimeZone;
    private FieldLock fieldLockRateCCExpTimeZone;
    private EllieMae.Encompass.Forms.TextBox txtAlternateVersion;
    private StandardButton sbIntentToProceed;
    private EllieMae.Encompass.Forms.TextBox prepaymentPenaltyDuringYearsTextbox;
    private EllieMae.Encompass.Forms.CheckBox chkChangedCircumstance;
    private EllieMae.Encompass.Forms.TextBox txtChangesReceivedDate;
    private Calendar calChangesReceivedDate;
    private MultilineTextBox txtChangedCircumstance;
    private MultilineTextBox txtComments;
    private StandardButton sbChangedCircumstance;
    private EllieMae.Encompass.Forms.CheckBox chkFeeLevelDisclosures;
    private EllieMae.Encompass.Forms.Label lblReason;
    private EllieMae.Encompass.Forms.CheckBox chkCocSettlementCharge;
    private EllieMae.Encompass.Forms.CheckBox chkCocEligibility;
    private EllieMae.Encompass.Forms.CheckBox chkRevisionReqByCust;
    private EllieMae.Encompass.Forms.CheckBox chkRateLock;
    private EllieMae.Encompass.Forms.CheckBox chkExpiration;
    private EllieMae.Encompass.Forms.CheckBox chkDelayedSettlement;
    private EllieMae.Encompass.Forms.CheckBox chkOtherReason;
    private EllieMae.Encompass.Forms.TextBox txtOtherReason;
    private EllieMae.Encompass.Forms.Label lblChangesRcvdDate;
    private EllieMae.Encompass.Forms.Label lblRevisedLeDueDate;
    private EllieMae.Encompass.Forms.TextBox txtRevisedLeDueDate;
    private EllieMae.Encompass.Forms.Label lblChangedCircumstances;
    private EllieMae.Encompass.Forms.Label lblComments;
    private EllieMae.Encompass.Forms.Panel pnlCoc;
    private HorizontalRule hrzRule2;
    private static string timeZoneUsedLE1X9;
    private static string timeZoneUsedLE1X7;
    private List<DropdownOption> optionsST = new List<DropdownOption>();
    private List<DropdownOption> optionsDST = new List<DropdownOption>();
    private FieldReformatOnUIHandler fieldReformatOnUIHandler;

    public LOANESTIMATEPAGE1InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public LOANESTIMATEPAGE1InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public LOANESTIMATEPAGE1InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public LOANESTIMATEPAGE1InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public LOANESTIMATEPAGE1InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput htmlInput,
      HTMLDocument htmlDoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, htmlInput, htmlDoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      try
      {
        this.fieldReformatOnUIHandler = new FieldReformatOnUIHandler(this.inputData);
        this.loanTermTableInputHandler = new LoanTermTableInputHandler(this.currentForm, this.inputData, this.ToString(), this.session);
        this.otherLoanType = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_1063");
        this.lockedDate = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("I_761");
        this.lockExpDate = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("I_762");
        this.calLockedDate = (Calendar) this.currentForm.FindControl("Cal_761");
        this.calLockExpDate = (Calendar) this.currentForm.FindControl("Cal_762");
        this.fieldLockRateExpDate = (FieldLock) this.currentForm.FindControl("FieldLock17");
        this.fieldLockRateExpTimeZone = (FieldLock) this.currentForm.FindControl("FieldLock18");
        this.fieldLockRateCCExpTimeZone = (FieldLock) this.currentForm.FindControl("FieldLock20");
        this.ddRateLockExpTimezone = (DropdownBox) this.currentForm.FindControl("dd_LE1X7");
        this.ddClosingCostExpTimezone = (DropdownBox) this.currentForm.FindControl("dd_LE1X9");
        this.txtAlternateVersion = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txtAlternateVersion");
        this.sbIntentToProceed = (StandardButton) this.currentForm.FindControl("sbIntentToProceed");
        this.txtChangesReceivedDate = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("I_3165");
        this.calChangesReceivedDate = (Calendar) this.currentForm.FindControl("cal3165");
        this.txtChangedCircumstance = (MultilineTextBox) this.currentForm.FindControl("I_3169");
        this.txtComments = (MultilineTextBox) this.currentForm.FindControl("I_LE1X86");
        this.sbChangedCircumstance = (StandardButton) this.currentForm.FindControl("stdbtn_3169");
        this.prepaymentPenaltyDuringYearsTextbox = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_X27");
        this.chkChangedCircumstance = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_3168");
        this.chkFeeLevelDisclosures = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_4461");
        this.lblReason = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label59");
        this.chkCocSettlementCharge = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_LE1X78");
        this.chkCocEligibility = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_LE1X79");
        this.chkRevisionReqByCust = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_LE1X80");
        this.chkRateLock = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_LE1X81");
        this.chkExpiration = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_LE1X82");
        this.chkDelayedSettlement = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_LE1X83");
        this.chkOtherReason = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chk_LE1X84");
        this.txtOtherReason = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("I_LE1X85");
        this.lblChangesRcvdDate = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label65");
        this.lblRevisedLeDueDate = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label66");
        this.txtRevisedLeDueDate = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("I_3167");
        this.lblChangedCircumstances = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label67");
        this.lblComments = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label68");
        this.pnlCoc = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("Panel7");
        this.hrzRule2 = (HorizontalRule) this.currentForm.FindControl("HorizontalRule2");
        this.poulateTimezones("762", true);
        this.poulateTimezones("LE1.X28", true);
        this.poulateTimezones("LE1.X1", true);
        if (this.inputData != null)
          this.sbChangedCircumstance.Enabled = this.inputData.GetField("3168") == "Y";
        if (!this.session.StartupInfo.EnableCoC)
          this.revertToNonCocLayout();
        this.panelLocalAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignPanels = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelLocalAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("localAdr1"));
        this.panelForeignAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignAdr1"));
        if (this.panelForeignAddresses[0] != null && this.panelLocalAddresses[0] != null)
          this.panelForeignAddresses[0].Position = this.panelLocalAddresses[0].Position;
        for (int index = 1; index <= 2; ++index)
          this.panelForeignPanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignPanel" + (object) index));
        this.selectCountryButtons = new List<StandardButton>();
        this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_URLA_X269"));
      }
      catch (Exception ex)
      {
      }
    }

    private bool isRequireCoCPriorDisclosure()
    {
      return this.session.LoanDataMgr.ConfigInfo.RequireCoCPriorDisclosure;
    }

    private ControlState getControlStateForFeeLevelDisclosures()
    {
      return this.session.StartupInfo.EnableCoC && this.loan != null && (this.isRequireCoCPriorDisclosure() || this.loan.GetLogList().IsThereAny2015DisclosureTracking()) ? ControlState.Disabled : ControlState.Enabled;
    }

    private ControlState getControlStateForLeCocFields()
    {
      return this.session.StartupInfo.EnableCoC && !(this.inputData.GetField("4461") != "Y") ? ControlState.Disabled : ControlState.Enabled;
    }

    private void revertToNonCocLayout()
    {
      int num = 28;
      this.chkFeeLevelDisclosures.Visible = false;
      this.lblReason.Top -= num;
      this.chkCocSettlementCharge.Top -= num;
      this.chkCocEligibility.Top -= num;
      this.chkRevisionReqByCust.Top -= num;
      this.chkRateLock.Top -= num;
      this.chkExpiration.Top -= num;
      this.chkDelayedSettlement.Top -= num;
      this.chkOtherReason.Top -= num;
      this.txtOtherReason.Top -= num;
    }

    internal override void FlipLockField(FieldLock fieldLock)
    {
      base.FlipLockField(fieldLock);
      this.poulateTimezones("LE1.X28", false);
      this.poulateTimezones("762", false);
    }

    private void poulateTimezones(string fieldId, bool populateDropdown)
    {
      try
      {
        this.optionsST.Clear();
        this.optionsST.Add(new DropdownOption("HST", "HST"));
        this.optionsST.Add(new DropdownOption("AKST ", "AKST"));
        this.optionsST.Add(new DropdownOption("PST", "PST"));
        this.optionsST.Add(new DropdownOption("MST", "MST"));
        this.optionsST.Add(new DropdownOption("CST", "CST"));
        this.optionsST.Add(new DropdownOption("EST", "EST"));
        this.optionsDST.Clear();
        this.optionsDST.Add(new DropdownOption("HST", "HST"));
        this.optionsDST.Add(new DropdownOption("AKDT ", "AKDT"));
        this.optionsDST.Add(new DropdownOption("PDT", "PDT"));
        this.optionsDST.Add(new DropdownOption("MST", "MST"));
        this.optionsDST.Add(new DropdownOption("MDT", "MDT"));
        this.optionsDST.Add(new DropdownOption("CDT", "CDT"));
        this.optionsDST.Add(new DropdownOption("EDT", "EDT"));
        DateTime dateTime = Utils.ParseDate((object) this.GetField("LE1.X28"));
        switch (fieldId)
        {
          case "LE1.XG9":
            if (this.inputData.IsLocked("LE1.X9"))
              break;
            this.UpdateClosingCostEstimateExpirationTimeZone(dateTime);
            break;
          case "762":
          case "761":
            DateTime date = Utils.ParseDate((object) this.GetField("762"));
            if (!this.inputData.IsLocked("LE1.X7"))
            {
              this.ddRateLockExpTimezone.Options.Clear();
              bool isDaylightSavingTime = date != DateTime.MinValue && System.TimeZoneInfo.Local.IsDaylightSavingTime(date);
              if (isDaylightSavingTime)
                this.ddRateLockExpTimezone.Options.AddRange((ICollection) this.optionsDST);
              else
                this.ddRateLockExpTimezone.Options.AddRange((ICollection) this.optionsST);
              LOANESTIMATEPAGE1InputHandler.timeZoneUsedLE1X7 = isDaylightSavingTime ? "DST" : "ST";
              this.inputData.SetField("LE1.X7", Utils.TransformSettingTimezoneToStandardTimezone(this.loan.Calculator.RateLockExpirationTimeZoneSetting, isDaylightSavingTime));
              this.RefreshControl("dd_LE1X7");
              break;
            }
            if (!(this.inputData.IsLocked("LE1.X7") & populateDropdown))
              break;
            this.ddRateLockExpTimezone.Options.Clear();
            switch (LOANESTIMATEPAGE1InputHandler.timeZoneUsedLE1X9)
            {
              case "DST":
                this.ddRateLockExpTimezone.Options.AddRange((ICollection) this.optionsDST);
                return;
              case "ST":
                this.ddRateLockExpTimezone.Options.AddRange((ICollection) this.optionsST);
                return;
              default:
                if (date != DateTime.MinValue && System.TimeZoneInfo.Local.IsDaylightSavingTime(date))
                {
                  this.ddRateLockExpTimezone.Options.AddRange((ICollection) this.optionsDST);
                  return;
                }
                this.ddRateLockExpTimezone.Options.AddRange((ICollection) this.optionsST);
                return;
            }
          case "LE1.X28":
          case "LE1.X1":
            IDisclosureTracking2015Log disclosureTracking2015Log = (IDisclosureTracking2015Log) null;
            if (this.loan != null)
              disclosureTracking2015Log = this.loan.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.LE);
            if (disclosureTracking2015Log == null && fieldId == "LE1.X1")
              dateTime = this.session.ConfigurationManager.AddBusinessDays(CalendarType.Business, Utils.ParseDate((object) this.GetField(fieldId)), (int) this.session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.LEDaysToExpire"], false);
            if (!this.inputData.IsLocked("LE1.X9"))
            {
              this.UpdateClosingCostEstimateExpirationTimeZone(dateTime);
              break;
            }
            if (!(this.inputData.IsLocked("LE1.X9") & populateDropdown))
              break;
            this.ddClosingCostExpTimezone.Options.Clear();
            switch (LOANESTIMATEPAGE1InputHandler.timeZoneUsedLE1X9)
            {
              case "DST":
                this.ddClosingCostExpTimezone.Options.AddRange((ICollection) this.optionsDST);
                break;
              case "ST":
                this.ddClosingCostExpTimezone.Options.AddRange((ICollection) this.optionsST);
                break;
              default:
                if (dateTime != DateTime.MinValue && System.TimeZoneInfo.Local.IsDaylightSavingTime(dateTime))
                {
                  this.ddClosingCostExpTimezone.Options.AddRange((ICollection) this.optionsDST);
                  break;
                }
                this.ddClosingCostExpTimezone.Options.AddRange((ICollection) this.optionsST);
                break;
            }
            if (this.session.LoanData == null || !((IEnumerable<IDisclosureTracking2015Log>) this.session.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(false)).Any<IDisclosureTracking2015Log>())
              break;
            this.fieldLockRateCCExpTimeZone.Enabled = false;
            this.ddClosingCostExpTimezone.Enabled = false;
            this.ddClosingCostExpTimezone.AccessMode = FieldAccessMode.ReadOnly;
            break;
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void UpdateClosingCostEstimateExpirationTimeZone(DateTime closingCostExpirationDate)
    {
      this.ddClosingCostExpTimezone.Options.Clear();
      bool isDaylightSavingTime = !(closingCostExpirationDate == DateTime.MinValue) && System.TimeZoneInfo.Local.IsDaylightSavingTime(closingCostExpirationDate);
      if (isDaylightSavingTime)
        this.ddClosingCostExpTimezone.Options.AddRange((ICollection) this.optionsDST);
      else
        this.ddClosingCostExpTimezone.Options.AddRange((ICollection) this.optionsST);
      LOANESTIMATEPAGE1InputHandler.timeZoneUsedLE1X9 = isDaylightSavingTime ? "DST" : "ST";
      string empty = string.Empty;
      string field = this.GetField("LE1.XG9");
      this.inputData.SetField("LE1.X9", !string.IsNullOrEmpty(field) ? Utils.TransformTimezoneToStandardTimezone(field, isDaylightSavingTime) : Utils.TransformSettingTimezoneToStandardTimezone((string) this.session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.CDExpirationTimeZone"], isDaylightSavingTime));
      if (this.GetField("3164") != "Y")
        this.inputData.SetField("LE1.XD9", this.inputData.GetField("LE1.X9"));
      else
        this.inputData.SetField("LE1.XD9", "");
      this.RefreshControl("dd_LE1X9");
      if (this.session.LoanData == null || !((IEnumerable<IDisclosureTracking2015Log>) this.session.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(false)).Any<IDisclosureTracking2015Log>())
        return;
      this.fieldLockRateCCExpTimeZone.Enabled = false;
      this.ddClosingCostExpTimezone.Enabled = false;
      this.ddClosingCostExpTimezone.AccessMode = FieldAccessMode.ReadOnly;
    }

    private bool disableIntentToProceedStatus()
    {
      DisclosureTracking2015Log disclosureTracking2015Log1 = (DisclosureTracking2015Log) null;
      foreach (DisclosureTracking2015Log disclosureTracking2015Log2 in this.session.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(true))
      {
        if (disclosureTracking2015Log2.DisclosedForLE)
        {
          if (disclosureTracking2015Log1 == null && disclosureTracking2015Log2 != null && (disclosureTracking2015Log2.BorrowerActualReceivedDate != DateTime.MinValue && disclosureTracking2015Log2.BorrowerActualReceivedDate <= DateTime.Now || disclosureTracking2015Log2.BorrowerPresumedReceivedDate != DateTime.MinValue && disclosureTracking2015Log2.BorrowerPresumedReceivedDate <= DateTime.Now || disclosureTracking2015Log2.IsBorrowerPresumedDateLocked && disclosureTracking2015Log2.LockedBorrowerPresumedReceivedDate != DateTime.MinValue && disclosureTracking2015Log2.LockedBorrowerPresumedReceivedDate <= DateTime.Now || disclosureTracking2015Log2.CoBorrowerActualReceivedDate != DateTime.MinValue && disclosureTracking2015Log2.CoBorrowerActualReceivedDate <= DateTime.Now || disclosureTracking2015Log2.CoBorrowerPresumedReceivedDate != DateTime.MinValue && disclosureTracking2015Log2.CoBorrowerPresumedReceivedDate <= DateTime.Now || disclosureTracking2015Log2.IsCoBorrowerPresumedDateLocked && disclosureTracking2015Log2.LockedCoBorrowerPresumedReceivedDate != DateTime.MinValue && disclosureTracking2015Log2.LockedCoBorrowerPresumedReceivedDate <= DateTime.Now))
            disclosureTracking2015Log1 = disclosureTracking2015Log2;
          if (disclosureTracking2015Log1 != null)
            return false;
        }
      }
      return true;
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      if (this.inputData is DisclosedLEHandler)
        return ControlState.Disabled;
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1063":
        case "11":
        case "1416":
        case "FR0104":
          return controlState;
        case "3164":
          if (this.disableIntentToProceedStatus())
          {
            this.sbIntentToProceed.Enabled = false;
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
          {
            this.sbIntentToProceed.Enabled = true;
            controlState = ControlState.Enabled;
            goto case "1063";
          }
        case "3165":
        case "LE1.X86":
          controlState = !(this.inputData.GetField("3168") == "Y") ? ControlState.Disabled : ControlState.Enabled;
          goto case "1063";
        case "3167":
          controlState = !(this.inputData.GetField("3168") == "Y") ? ControlState.Disabled : this.getControlStateForLeCocFields();
          goto case "1063";
        case "3197":
          this.SetControlState("Calendar2", this.GetFieldValue("3164") == "Y");
          if (this.inputData.GetField("3164") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "3973":
          this.SetControlState("FieldLock23", this.GetFieldValue("3164") == "Y");
          if (this.inputData.GetField("3164") != "Y" || !this.inputData.IsLocked("3973"))
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "3974":
        case "3976":
          if (this.inputData.GetField("3164") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "3975":
          if (this.inputData.GetField("3164") != "Y" || this.inputData.GetField("3974") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "4461":
          controlState = this.getControlStateForFeeLevelDisclosures();
          goto case "1063";
        case "CONST.X1":
          controlState = !(this.inputData.GetField("19") != "ConstructionToPermanent") ? ControlState.Enabled : ControlState.Disabled;
          goto case "1063";
        case "LE1.X6":
          if (this.inputData.GetField("2400") != "Y" || !this.inputData.IsLocked("LE1.X6"))
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "LE1.X7":
          if (this.inputData.GetField("2400") != "Y" || !this.inputData.IsLocked("LE1.X7"))
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "LE1.X78":
        case "LE1.X79":
        case "LE1.X80":
        case "LE1.X81":
        case "LE1.X82":
        case "LE1.X83":
          controlState = !this.inputData.IsLocked(id) ? this.getControlStateForLeCocFields() : ControlState.Disabled;
          goto case "1063";
        case "LE1.X85":
          if (this.inputData.GetField("LE1.X84") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "LE1.XG9":
          if (this.session.LoanData != null && ((IEnumerable<IDisclosureTracking2015Log>) this.session.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(false)).Any<IDisclosureTracking2015Log>())
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "URLA.X269":
          if (this.loan.GetField("URLA.X267") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        default:
          controlState = this.loanTermTableInputHandler == null ? ControlState.Default : this.loanTermTableInputHandler.GetControlState(id, ControlState.Default);
          goto case "1063";
      }
    }

    public override void onkeyup(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.currentForm.FindControlForElement(pEvtObj.srcElement) is FieldControl controlForElement && controlForElement.Field.FieldID == "RE88395.X121")
      {
        bool needsUpdate = false;
        string str = Utils.FormatInput(controlForElement.Value, FieldFormat.INTEGER, ref needsUpdate);
        if (!needsUpdate)
          return;
        controlForElement.BindTo(str);
      }
      else
        base.onkeyup(pEvtObj);
    }

    private void clearAllLeCocFields()
    {
      base.UpdateFieldValue("LE1.X78", "");
      base.UpdateFieldValue("LE1.X79", "");
      base.UpdateFieldValue("LE1.X80", "");
      base.UpdateFieldValue("LE1.X81", "");
      base.UpdateFieldValue("LE1.X82", "");
      base.UpdateFieldValue("LE1.X83", "");
      base.UpdateFieldValue("LE1.X84", "");
      base.UpdateFieldValue("LE1.X85", "");
      this.UpdateFieldValue("3168", "");
      base.UpdateFieldValue("3165", "//");
      base.UpdateFieldValue("3167", "//");
      base.UpdateFieldValue("3169", "");
      base.UpdateFieldValue("LE1.X86", "");
    }

    private void clearAllFeeCocDetails() => this.loan.RemoveAllGoodFaithChangeOfCircumstance();

    internal override void UpdateFieldValue(string id, string val)
    {
      if (this.loanTermTableInputHandler != null)
        this.loanTermTableInputHandler.UpdateFieldValue(id, val);
      if (id != "4461" && this.inputData.GetField("4461") == "Y")
      {
        if (this.loan.FeeLevel_COC_LE_Warning)
        {
          int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Please note: Any updates made on the Good Faith Variance Violated Alert page or 2015 itemization may overwrite your existing Changed Circumstances.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.loan.FeeLevel_COC_LE_Warning = false;
        }
        if (!this.loan.Locked_COC_Fields.Contains((object) id))
          this.loan.Locked_COC_Fields.Add((object) id);
      }
      if (id == "4461")
      {
        if (!string.IsNullOrEmpty(val) && val == "Y")
        {
          if (Utils.Dialog((IWin32Window) this.session.MainForm, "Any changes to Disclosure Information you have made will be cleared. Would you like to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
          this.clearAllLeCocFields();
          base.UpdateFieldValue(id, val);
        }
        else
        {
          if (Utils.Dialog((IWin32Window) this.session.MainForm, "All fee level changes entered in Alerts will be cleared. Would you like to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
          base.UpdateFieldValue(id, val);
          this.clearAllLeCocFields();
          this.clearAllFeeCocDetails();
        }
      }
      base.UpdateFieldValue(id, val);
      if (id == "762" || id == "761" || id == "LE1.X28" || id == "LE1.X1" || id == "LE1.XG9")
        this.poulateTimezones(id, false);
      if (id == "1172")
        this.otherLoanType.Enabled = val == "Other" || val == "FarmersHomeAdministration";
      if (id == "LE1.X84" && val == "N")
        base.UpdateFieldValue("LE1.X85", "");
      if (id == "3168")
      {
        this.sbChangedCircumstance.Enabled = this.txtChangesReceivedDate.Enabled = this.calChangesReceivedDate.Enabled = this.txtChangedCircumstance.Enabled = this.txtComments.Enabled = val == "Y";
        if (val == "Y")
        {
          base.UpdateFieldValue("3165", DateTime.Today.ToString("MM/dd/yyyy"));
          base.UpdateFieldValue("3169", "");
          base.UpdateFieldValue("LE1.X90", "");
        }
        else
        {
          base.UpdateFieldValue("3165", "//");
          base.UpdateFieldValue("3167", "//");
          base.UpdateFieldValue("3169", "");
          base.UpdateFieldValue("LE1.X90", "");
          base.UpdateFieldValue("LE1.X86", "");
        }
      }
      if (id == "2400")
      {
        EllieMae.Encompass.Forms.TextBox lockedDate = this.lockedDate;
        EllieMae.Encompass.Forms.TextBox lockExpDate = this.lockExpDate;
        Calendar calLockedDate = this.calLockedDate;
        bool flag1;
        this.calLockExpDate.Enabled = flag1 = val == "Y" && this.inputData.GetField("3941") != "Y";
        int num1;
        bool flag2 = (num1 = flag1 ? 1 : 0) != 0;
        calLockedDate.Enabled = num1 != 0;
        int num2;
        bool flag3 = (num2 = flag2 ? 1 : 0) != 0;
        lockExpDate.Enabled = num2 != 0;
        int num3 = flag3 ? 1 : 0;
        lockedDate.Enabled = num3 != 0;
        this.fieldLockRateExpDate.Enabled = this.fieldLockRateExpTimeZone.Enabled = val == "Y";
      }
      if (id == "3974")
        this.loan.SetField("3975", "");
      if (id == "LOANTERMTABLE.CUSTOMIZE" && val != "Y" && this.loan != null && this.loan.Calculator != null)
        this.loan.Calculator.CalcPaymentSchedule();
      if (this.loanTermTableInputHandler == null)
        return;
      this.loanTermTableInputHandler.SetLayout();
    }

    protected override string GetFieldValue(string id, FieldSource fieldSource)
    {
      this.loanTermTableInputHandler.SetSectionStatus(id);
      if (id == "3168")
        this.sbChangedCircumstance.Enabled = this.txtChangesReceivedDate.Enabled = this.calChangesReceivedDate.Enabled = this.txtChangedCircumstance.Enabled = this.txtComments.Enabled = this.inputData.GetField("3168") == "Y";
      if (id == "2400")
      {
        EllieMae.Encompass.Forms.TextBox lockedDate = this.lockedDate;
        EllieMae.Encompass.Forms.TextBox lockExpDate = this.lockExpDate;
        Calendar calLockedDate = this.calLockedDate;
        bool flag1;
        this.calLockExpDate.Enabled = flag1 = this.inputData.GetField("2400") == "Y" && this.inputData.GetField("3941") != "Y";
        int num1;
        bool flag2 = (num1 = flag1 ? 1 : 0) != 0;
        calLockedDate.Enabled = num1 != 0;
        int num2;
        bool flag3 = (num2 = flag2 ? 1 : 0) != 0;
        lockExpDate.Enabled = num2 != 0;
        int num3 = flag3 ? 1 : 0;
        lockedDate.Enabled = num3 != 0;
        this.fieldLockRateExpDate.Enabled = this.fieldLockRateExpTimeZone.Enabled = this.inputData.GetField("2400") == "Y";
      }
      if (id == "LE2.X27")
        this.txtAlternateVersion.Visible = this.inputData.GetField("LE2.X28") == "Y";
      string str = this.loanTermTableInputHandler.GetFieldValue(id, base.GetFieldValue(id, fieldSource));
      if (id == "LE1.X29")
        str = Utils.RemoveEndingZeros(Decimal.Parse(Utils.RemoveEndingZeros(base.GetFieldValue(id, fieldSource), true)).ToString("N"));
      else if (id == "LE1.X87" && base.GetFieldValue(id, fieldSource) != "")
        str = Utils.RemoveEndingZeros(Decimal.Parse(Utils.RemoveEndingZeros(base.GetFieldValue(id, fieldSource), true)).ToString("N"));
      if (id == "LE1.X27")
        this.prepaymentPenaltyDuringYearsTextbox.Alignment = string.Compare(str, "first", true) == 0 ? TextAlignment.Left : TextAlignment.Right;
      return this.fieldReformatOnUIHandler.GetFieldValue(id, str);
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      if (this.loanTermTableInputHandler == null)
        return;
      this.loanTermTableInputHandler.SetLayout();
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      if (!(action == "prepayment") && !(action == "viewworstcase"))
        return;
      this.SetFieldFocus("l_X348");
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.selectCountryButtons != null && this.selectCountryButtons.Count > 0)
      {
        bool flag = this.GetField("URLA.X267") == "Y";
        this.selectCountryButtons[0].Visible = flag;
        this.panelLocalAddresses[0].Visible = !flag;
        this.panelForeignAddresses[0].Visible = flag;
      }
      bool flag1 = false;
      if (this.loan == null && Session.LoanData != null)
        flag1 = Session.LoanData.GetField("1825") == "2020";
      if (this.GetField("1825") == "2020" | flag1)
      {
        for (int index = 0; index < 2; ++index)
          this.panelForeignPanels[index].Visible = true;
      }
      else
      {
        for (int index = 0; index < 2; ++index)
          this.panelForeignPanels[index].Visible = false;
      }
      if (this.session.LoanData == null || !((IEnumerable<IDisclosureTracking2015Log>) this.session.LoanData.GetLogList().GetAllIDisclosureTracking2015Log(false)).Any<IDisclosureTracking2015Log>())
        return;
      this.fieldLockRateCCExpTimeZone.Enabled = false;
      this.ddClosingCostExpTimezone.Enabled = false;
      this.ddClosingCostExpTimezone.AccessMode = FieldAccessMode.ReadOnly;
    }
  }
}

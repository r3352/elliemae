// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LOLOCKREQUESTInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine.LockRequest;
using EllieMae.EMLite.LoanUtils.RateLocks;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class LOLOCKREQUESTInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Panel plBorrower;
    private EllieMae.Encompass.Forms.Panel plPair1;
    private EllieMae.Encompass.Forms.Panel plPair2;
    private EllieMae.Encompass.Forms.Panel plPair3;
    private EllieMae.Encompass.Forms.Panel plPair4;
    private EllieMae.Encompass.Forms.Panel plPair5;
    private EllieMae.Encompass.Forms.Panel plOther;
    private EllieMae.Encompass.Forms.Panel plForm;
    private EllieMae.Encompass.Forms.Panel plNoUnits;
    private EllieMae.Encompass.Forms.Panel plFHASecResidence;
    private EllieMae.Encompass.Forms.Panel pnlLockExtension;
    private EllieMae.Encompass.Forms.Panel pnlAddress;
    private EllieMae.Encompass.Forms.Panel pnlURLA2020Address;
    private EllieMae.Encompass.Forms.GroupBox grpBoxSubjectProperty;
    private EllieMae.Encompass.Forms.Panel pnlLockRequestAction;
    private EllieMae.Encompass.Forms.Panel pnlCancelLock;
    private EllieMae.Encompass.Forms.Panel pnlDeliveryType;
    private EllieMae.Encompass.Forms.Panel pnlLockRequestFields;
    private EllieMae.Encompass.Forms.Button buttonGetONRPPricing;
    private EllieMae.Encompass.Forms.Button buttonGetPricing;
    private EllieMae.Encompass.Forms.Button buttonDetailedLock;
    private EllieMae.Encompass.Forms.TextBox tb4510;
    private Point noPoint;
    private FieldLock purchasePriceLock;
    private EllieMae.Encompass.Forms.Label labelFormName;
    private EllieMae.Encompass.Forms.Label lblFHASecResidence;
    private EllieMae.Encompass.Forms.CheckBox chkSecResidence;
    private EllieMae.Encompass.Forms.GroupBox groupBox6;
    private EllieMae.Encompass.Forms.GroupBox groupBox4;
    private EllieMae.Encompass.Forms.GroupBox groupBox3;
    private CategoryBox categoryBox4;
    private List<string> loanLockFields;
    private Dictionary<string, string> standardLockPriceFieldCache = new Dictionary<string, string>();
    private Dictionary<string, string> relockPriceFieldCache = new Dictionary<string, string>();
    private string field3841CurrentValue;
    private string relockPriceCache_LockNumOfDays = "";
    private string relockPriceCache_LockExpirationDate = "";
    private bool isRelockFromExtension;
    private bool isLockDeskScheduleEnabled;
    private bool isONRPRetailChannelEnabled;
    private bool isONRPWholesaleChannelEnabled;
    private bool isONRPCorrespondentChannelEnabled;
    private bool userHasSecondaryRegistrationAccessRight;
    private bool useEPPS;
    private string purposeOfLoanWarningMessage = "Construction or Construction-Perm requires the Purchase price[3038] to be mapped from Field CONST.X58. Field Purchase price[3038] should be unlocked.";
    private List<TemporaryBuydown> buydownTemplates;
    private bool fromSetting;
    private bool getRelockPricing;
    private bool DisablePricingChange;
    private bool notAllowedPricingChangeSetting;
    private string[] enableRateLockFieldList;
    private string[] enableOtherFieldList;
    public bool IsOnrpButtonClicked;
    private LockRequestLog relockCurrentLockLogVal;

    public LOLOCKREQUESTInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
      this.init();
    }

    public LOLOCKREQUESTInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
      this.init();
      if (!this.notAllowedPricingChangeSetting)
        return;
      string field1 = this.GetField("2089");
      string field2 = this.loan.GetField("LOCKRATE.RATESTATUS");
      if (Utils.IsDate((object) field1) && field2 != "NotLocked")
        this.clearPricingSubmissionDate();
      if (!this.DisablePricingChange)
        return;
      this.EnablePricingFields(false);
    }

    public LOLOCKREQUESTInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
      this.init();
    }

    public LOLOCKREQUESTInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
      this.init();
    }

    public bool IsGetONRPPricingButtonVisible
    {
      get
      {
        return this.userHasSecondaryRegistrationAccessRight && this.useEPPS && this.isLockDeskScheduleEnabled && (this.isONRPRetailChannelEnabled || this.isONRPWholesaleChannelEnabled || this.isONRPCorrespondentChannelEnabled) && !LockUtils.IsRelock(this.GetFieldValue("3841"));
      }
    }

    public void SetDisablePricingChange()
    {
      this.DisablePricingChange = this.notAllowedChangesAfterPricingSubmitted();
    }

    private void init()
    {
      this.noPoint = this.plNoUnits.Position;
      this.loanLockFields = new List<string>()
      {
        "3041",
        "2853",
        "3038",
        "3049",
        "3056",
        "3241",
        "3242",
        "2398",
        "142",
        "353",
        "976",
        "740",
        "742",
        "3892",
        "4459",
        "4463"
      };
      for (int index = 2866; index <= 2952; ++index)
        this.loanLockFields.Add(index.ToString());
      for (int index = 2954; index <= 2967; ++index)
        this.loanLockFields.Add(index.ToString());
      for (int index = 3516; index <= 3530; ++index)
        this.loanLockFields.Add(index.ToString());
      for (int index = 3043; index <= 3047; ++index)
        this.loanLockFields.Add(index.ToString());
      if (this.session != null && this.session.CompanyInfo != null && this.session.CompanyInfo.ClientID != null && !LockDeskHoursUtils.IsQAONRPSettingEnabled(this.session.CompanyInfo.ClientID))
        (this.currentForm.FindControl("btnonrpeffectivesetting") as EllieMae.Encompass.Forms.Button).Visible = false;
      this.InitONRPSettings();
      this.InitRequestLockStatus();
      this.AdjustFHASecondary();
      string[] fields = this.loan.Settings.FieldSettings.LockRequestAdditionalFields.GetFields(true);
      for (int index = 0; index < fields.Length; ++index)
      {
        string customFieldId = LockRequestCustomField.GenerateCustomFieldID(fields[index]);
        string fieldValue = this.GetFieldValue(fields[index]);
        if (string.IsNullOrWhiteSpace(this.GetFieldValue(customFieldId)) && !string.IsNullOrWhiteSpace(fieldValue))
          this.UpdateFieldValue(customFieldId, fieldValue);
      }
    }

    private void EnablePricingFields(bool enable)
    {
      if (enable && LockUtils.IsRelock(this.GetFieldValue("3841")))
      {
        this.enableOtherLockRequestFormFields(enable);
        this.SetFieldEnabled("l_3841");
      }
      else
      {
        if (this.enableRateLockFieldList == null)
        {
          List<string> source1 = new List<string>();
          List<string> source2 = new List<string>();
          source2.Add("StandardButton1");
          source2.Add("ZipCodeLookup2");
          source2.Add("ZipCodeLookup4");
          source2.Add("l_995");
          source2.Add("TextBox9");
          source2.Add("TextBox15");
          source2.Add("TextBox16");
          source2.Add("TextBox18");
          source2.Add("TextBox28");
          source2.Add("TextBox30");
          source2.Add("DropdownEditBox2");
          source2.Add("DropdownBox2");
          source2.Add("DropdownBox3");
          source2.Add("DropdownBox4");
          source2.Add("DropdownBox6");
          source2.Add("DropdownBox7");
          source2.Add("DropdownBox8");
          source2.Add("DropdownBox9");
          source2.Add("DropdownBox10");
          source2.Add("DropdownBox11");
          source2.Add("drContributor");
          source2.Add("txtBuydownType");
          for (int index = 1; index <= 6; ++index)
          {
            source2.Add("txtRate" + index.ToString());
            source2.Add("txtTerm" + index.ToString());
          }
          source2.Add("TextBox34");
          source2.Add("TextBox43");
          source2.Add("TextBox107");
          source2.Add("TextBox108");
          source2.Add("TextBox109");
          source2.Add("TextBox110");
          source2.Add("TextBox111");
          source2.Add("TextBox112");
          source2.Add("DropdownEditBox1");
          source2.Add("l_2940");
          source2.Add("TextBox12");
          source2.Add("TextBox21");
          source2.Add("TextBox14");
          source2.Add("TextBox25");
          source2.Add("TextBox8");
          source2.Add("TextBox116");
          source2.Add("chkSecResidence");
          source2.Add("txtBoxStreetAddress");
          source2.Add("dropDownUnitType");
          source2.Add("txtBoxUnitNo");
          source2.Add("TextBox121");
          source2.Add("TextBox125");
          source2.Add("TextBox126");
          source2.Add("TextBox127");
          source2.Add("TextBox130");
          source2.Add("TextBox131");
          source2.Add("TextBox132");
          source2.Add("TextBox133");
          source2.Add("TextBox134");
          source2.Add("DropdownBox5");
          source2.Add("DropdownBox13");
          source2.Add("chkCondominium");
          source2.Add("chkCooperative");
          source2.Add("chkPUD");
          source2.Add("chkPropNotLoca");
          for (int index = 1; index <= 6; ++index)
            source2.Add("CheckBox" + index.ToString());
          source2.Add("CheckBox19");
          source2.Add("CheckBox20");
          source2.Add("CheckBox24");
          source2.Add("CheckBox25");
          source2.Add("CheckBox21");
          source2.Add("CheckBox27");
          source2.Add("CheckBox28");
          source2.Add("CheckBox31");
          source2.Add("CheckBox32");
          for (int index = 1; index <= 7; ++index)
            source2.Add("FieldLock" + index.ToString());
          source2.Add("l_142");
          source2.Add("l_353");
          source2.Add("l_976");
          source2.Add("TextBox118");
          source2.Add("l_742");
          source2.Add("Calendar1");
          source1.Add("Calendar2");
          source1.Add("Calendar3");
          source1.Add("Calendar4");
          source1.Add("Button1");
          source1.Add("Button2");
          source1.Add("Button5");
          source2.Add("Button6");
          source2.Add("Button10");
          source1.Add("ButtonONRPPricing");
          source1.Add("btnCancelLock");
          source1.Add("btnonrpeffectivesetting");
          source1.Add("l_3841");
          source1.Add("TextBox44");
          source1.Add("TextBox45");
          source1.Add("TextBox46");
          source1.Add("l_2092");
          source1.Add("l_2101");
          source1.Add("l_2647");
          source1.Add("TextBox117");
          source1.Add("TextBox119");
          source1.Add("l_2088");
          source1.Add("I_4187");
          source1.Add("l_2089");
          source1.Add("l_2090");
          source1.Add("l_2091");
          source1.Add("l_3254");
          source1.Add("DropdownEditBox3");
          source1.Add("MultilineTextBox2");
          this.enableRateLockFieldList = source1.Where<string>((Func<string, bool>) (controlId => this.GetControlEnableState(controlId))).ToArray<string>();
          this.enableOtherFieldList = source2.Where<string>((Func<string, bool>) (controlId => this.GetControlEnableState(controlId))).ToArray<string>();
        }
        foreach (string enableRateLockField in this.enableRateLockFieldList)
        {
          if (enable)
            this.SetFieldEnabled(enableRateLockField);
          else
            this.SetFieldDisabled(enableRateLockField);
        }
        this.enableOtherLockRequestFormFields(enable);
        if (enable)
        {
          this.SetFieldEnabled("MultilineTextBox1");
          this.SetFieldEnabled("l_3841");
        }
        else
        {
          this.SetFieldEnabled("btnNewLock");
          this.buttonGetPricing.Enabled = false;
          this.buttonDetailedLock.Enabled = false;
          this.buttonGetONRPPricing.Enabled = false;
          this.SetFieldDisabled("Button1");
          this.SetFieldDisabled("btnCancelLock");
          this.SetFieldDisabled("btnonrpeffectivesetting");
          this.SetFieldDisabled("l_3841");
        }
      }
    }

    private void enableOtherLockRequestFormFields(bool enable)
    {
      foreach (string enableOtherField in this.enableOtherFieldList)
      {
        if (enable)
          this.SetFieldEnabled(enableOtherField);
        else
          this.SetFieldDisabled(enableOtherField);
      }
    }

    internal override void CreateControls()
    {
      base.CreateControls();
      if (this.loan != null && this.loan.GetField("2868") == string.Empty && this.loan.GetField("36") != string.Empty)
        this.loan.TriggerCalculation("36", this.loan.GetField("36"));
      this.notAllowedPricingChangeSetting = this.session.StartupInfo.PolicySettings.Contains((object) "Policies.NotAllowPricingChange") && (bool) this.session.StartupInfo.PolicySettings[(object) "Policies.NotAllowPricingChange"];
      this.plBorrower = this.currentForm.FindControl("PanelBorrower") as EllieMae.Encompass.Forms.Panel;
      this.plPair1 = this.currentForm.FindControl("PanelPair1") as EllieMae.Encompass.Forms.Panel;
      this.plPair2 = this.currentForm.FindControl("PanelPair2") as EllieMae.Encompass.Forms.Panel;
      this.plPair3 = this.currentForm.FindControl("PanelPair3") as EllieMae.Encompass.Forms.Panel;
      this.plPair4 = this.currentForm.FindControl("PanelPair4") as EllieMae.Encompass.Forms.Panel;
      this.plPair5 = this.currentForm.FindControl("PanelPair5") as EllieMae.Encompass.Forms.Panel;
      this.plOther = this.currentForm.FindControl("PanelOther") as EllieMae.Encompass.Forms.Panel;
      this.pnlLockExtension = this.currentForm.FindControl("pnlExtensionRequest") as EllieMae.Encompass.Forms.Panel;
      this.pnlLockRequestAction = this.currentForm.FindControl("pnlLockAction") as EllieMae.Encompass.Forms.Panel;
      this.pnlAddress = this.currentForm.FindControl("pnlAddress") as EllieMae.Encompass.Forms.Panel;
      this.pnlURLA2020Address = this.currentForm.FindControl("pnlURLA2020Address") as EllieMae.Encompass.Forms.Panel;
      this.grpBoxSubjectProperty = this.currentForm.FindControl("GroupBox3") as EllieMae.Encompass.Forms.GroupBox;
      this.pnlCancelLock = this.currentForm.FindControl("pnlCancelLock") as EllieMae.Encompass.Forms.Panel;
      this.pnlDeliveryType = this.currentForm.FindControl("plDeliveryType") as EllieMae.Encompass.Forms.Panel;
      this.pnlLockRequestFields = this.currentForm.FindControl("plLockRequestFields") as EllieMae.Encompass.Forms.Panel;
      this.buttonGetONRPPricing = this.currentForm.FindControl("ButtonONRPPricing") as EllieMae.Encompass.Forms.Button;
      this.buttonGetPricing = this.currentForm.FindControl("Button5") as EllieMae.Encompass.Forms.Button;
      this.buttonDetailedLock = this.currentForm.FindControl("Button2") as EllieMae.Encompass.Forms.Button;
      this.plForm = this.currentForm.FindControl("pnlForm") as EllieMae.Encompass.Forms.Panel;
      this.plFHASecResidence = this.currentForm.FindControl("pnlFHASecResidence") as EllieMae.Encompass.Forms.Panel;
      this.plNoUnits = this.currentForm.FindControl("pnlNoUnits") as EllieMae.Encompass.Forms.Panel;
      this.labelFormName = this.currentForm.FindControl("LabelFormName") as EllieMae.Encompass.Forms.Label;
      this.purchasePriceLock = this.currentForm.FindControl("FieldLock4") as FieldLock;
      this.lblFHASecResidence = this.currentForm.FindControl("lblFHASecResidence") as EllieMae.Encompass.Forms.Label;
      this.chkSecResidence = this.currentForm.FindControl("chkSecResidence") as EllieMae.Encompass.Forms.CheckBox;
      this.groupBox4 = this.currentForm.FindControl("GroupBox4") as EllieMae.Encompass.Forms.GroupBox;
      this.groupBox3 = this.currentForm.FindControl("GroupBox3") as EllieMae.Encompass.Forms.GroupBox;
      this.groupBox6 = this.currentForm.FindControl("GroupBox6") as EllieMae.Encompass.Forms.GroupBox;
      this.categoryBox4 = this.currentForm.FindControl("CategoryBox4") as CategoryBox;
      if (this.GetField("3841") == "")
        this.SetField("3841", "NewLock");
      this.field3841CurrentValue = this.GetField("3841");
      this.tb4510 = this.currentForm.FindControl("TextBox43") as EllieMae.Encompass.Forms.TextBox;
      if (this.tb4510 != null)
        this.tb4510.Enabled = this.loan.GetField("2952") == "HELOC";
      this.InitRequestLockStatus();
      this.InitRequestType();
      this.UpdateReLockFields();
      this.InitONRPSettings();
      this.zoomPanels();
      this.URLA2020AddressLayout();
      this.buydownTemplates = this.session.TemporaryBuydownTypeBpmManager.GetAllTemporaryBuydowns();
      this.fromSetting = true;
      this.SetField("2967", this.GetField("2861"));
    }

    public override void RefreshContents()
    {
      base.RefreshContents();
      if (!this.DisablePricingChange)
        return;
      this.EnablePricingFields(false);
    }

    private bool notAllowedChangesAfterPricingSubmitted()
    {
      if (!this.notAllowedPricingChangeSetting || this.session.LoanDataMgr.AllowAutoLock(false, LockUtils.IsRelock(this.GetFieldValue("3841")), false, false, this.getRelockPricing))
        return false;
      string field = this.session.LoanData.GetField("3039");
      return field != "//" && Utils.IsDate((object) field);
    }

    private void zoomPanels()
    {
      this.plPair1.Visible = false;
      this.plPair2.Visible = false;
      this.plPair3.Visible = false;
      this.plPair4.Visible = false;
      this.plPair5.Visible = false;
      EllieMae.Encompass.Forms.Panel panel = this.plBorrower;
      for (int index = 2880; index <= 2891; ++index)
      {
        if (this.GetFieldValue(index.ToString()) != "")
        {
          this.plPair1.Visible = true;
          panel = this.plPair1;
          break;
        }
      }
      for (int index = 2892; index <= 2903; ++index)
      {
        if (this.GetFieldValue(index.ToString()) != "")
        {
          this.plPair1.Visible = true;
          this.plPair2.Visible = true;
          panel = this.plPair2;
          break;
        }
      }
      for (int index = 2904; index <= 2915; ++index)
      {
        if (this.GetFieldValue(index.ToString()) != "")
        {
          this.plPair1.Visible = true;
          this.plPair2.Visible = true;
          this.plPair3.Visible = true;
          panel = this.plPair3;
          break;
        }
      }
      for (int index = 2916; index <= 2927; ++index)
      {
        if (this.GetFieldValue(index.ToString()) != "")
        {
          this.plPair1.Visible = true;
          this.plPair2.Visible = true;
          this.plPair3.Visible = true;
          this.plPair4.Visible = true;
          panel = this.plPair4;
          break;
        }
      }
      for (int index = 2928; index <= 2939; ++index)
      {
        if (this.GetFieldValue(index.ToString()) != "")
        {
          this.plPair1.Visible = true;
          this.plPair2.Visible = true;
          this.plPair3.Visible = true;
          this.plPair4.Visible = true;
          this.plPair5.Visible = true;
          panel = this.plPair5;
          break;
        }
      }
      EllieMae.Encompass.Forms.Panel plOther = this.plOther;
      int top1 = panel.Top;
      Size size1 = panel.Size;
      int height1 = size1.Height;
      int num = top1 + height1;
      plOther.Top = num;
      EllieMae.Encompass.Forms.Panel plForm = this.plForm;
      size1 = this.plForm.Size;
      int width = size1.Width;
      int top2 = this.plOther.Top;
      size1 = this.plOther.Size;
      int height2 = size1.Height;
      int height3 = top2 + height2 + 160 + 170 + 10 + 70 + 26 + 80;
      Size size2 = new Size(width, height3);
      plForm.Size = size2;
      EllieMae.Encompass.Forms.Label labelFormName = this.labelFormName;
      int x = this.labelFormName.Position.X;
      size1 = this.plForm.Size;
      int y = size1.Height + 4 + 10;
      Point point = new Point(x, y);
      labelFormName.Position = point;
      this.ArrangeButtons();
      this.RefreshContents();
    }

    private void URLA2020AddressLayout()
    {
      if (this.inputData.GetField("1825") != "2020")
      {
        if (this.pnlAddress != null)
        {
          this.pnlAddress.Visible = true;
          this.groupBox3.Size = new Size(this.groupBox3.Size.Width, this.groupBox3.Size.Height - 60);
          this.groupBox4.Position = new Point(this.groupBox4.Position.X, this.groupBox4.Position.Y - 65);
          this.groupBox6.Position = new Point(this.groupBox6.Position.X, this.groupBox6.Position.Y - 75);
          this.categoryBox4.Position = new Point(this.categoryBox4.Position.X, this.categoryBox4.Position.Y - 70);
          this.plForm.Size = new Size(this.plForm.Size.Width, this.plOther.Top + this.plOther.Size.Height + 160 + 170 + 10 + 70 + 26 + 8);
          this.labelFormName.Position = new Point(this.labelFormName.Position.X, this.plForm.Size.Height + 4 + 8);
        }
        if (this.pnlURLA2020Address == null)
          return;
        this.pnlURLA2020Address.Visible = false;
      }
      else
      {
        if (this.pnlAddress != null)
          this.pnlAddress.Visible = false;
        if (this.pnlURLA2020Address == null || this.pnlAddress == null || this.grpBoxSubjectProperty == null)
          return;
        EllieMae.Encompass.Forms.Panel pnlUrlA2020Address = this.pnlURLA2020Address;
        Point position = this.pnlAddress.Position;
        int x = position.X;
        position = this.pnlAddress.Position;
        int y = position.Y;
        Point point = new Point(x, y);
        pnlUrlA2020Address.Position = point;
        this.pnlURLA2020Address.Visible = true;
      }
    }

    private void ArrangeButtons()
    {
      Point position1;
      if (!(bool) this.session.ServerManager.GetServerSetting("Policies.EnableLockCancellation") || !(bool) this.session.ServerManager.GetServerSetting("Policies.EnableLockCancellation_ReqForm"))
      {
        this.pnlCancelLock.Visible = false;
        EllieMae.Encompass.Forms.Panel pnlLockExtension = this.pnlLockExtension;
        int x1 = this.pnlCancelLock.Position.X + (this.pnlCancelLock.Size.Width - this.pnlLockExtension.Size.Width);
        Point position2 = this.pnlLockExtension.Position;
        int y1 = position2.Y;
        Point point1 = new Point(x1, y1);
        pnlLockExtension.Position = point1;
        EllieMae.Encompass.Forms.Panel lockRequestAction = this.pnlLockRequestAction;
        position2 = this.pnlLockExtension.Position;
        int x2 = position2.X - this.pnlLockRequestAction.Size.Width;
        position1 = this.pnlLockRequestAction.Position;
        int y2 = position1.Y;
        Point point2 = new Point(x2, y2);
        lockRequestAction.Position = point2;
      }
      if (!(bool) this.session.ServerManager.GetServerSetting("Policies.EnableLockExtension"))
      {
        this.pnlLockExtension.Visible = false;
        EllieMae.Encompass.Forms.Panel lockRequestAction = this.pnlLockRequestAction;
        position1 = this.pnlLockExtension.Position;
        int x = position1.X + (this.pnlLockExtension.Size.Width - this.pnlLockRequestAction.Size.Width);
        position1 = this.pnlLockRequestAction.Position;
        int y = position1.Y;
        Point point = new Point(x, y);
        lockRequestAction.Position = point;
      }
      else
        this.pnlLockExtension.Visible = true;
      if (this.IsGetONRPPricingButtonVisible)
      {
        this.buttonGetONRPPricing.Visible = true;
        this.buttonDetailedLock.Position = new Point(-1, 0);
        this.buttonGetPricing.Position = new Point(85, 0);
      }
      else
      {
        if (!this.buttonGetONRPPricing.Visible)
          return;
        this.buttonGetONRPPricing.Visible = false;
        int num = this.buttonGetONRPPricing.Size.Width + 2;
        EllieMae.Encompass.Forms.Button buttonGetPricing = this.buttonGetPricing;
        position1 = this.buttonGetPricing.Position;
        int x3 = position1.X + num;
        position1 = this.buttonGetPricing.Position;
        int y3 = position1.Y;
        Point point3 = new Point(x3, y3);
        buttonGetPricing.Position = point3;
        EllieMae.Encompass.Forms.Button buttonDetailedLock = this.buttonDetailedLock;
        position1 = this.buttonDetailedLock.Position;
        int x4 = position1.X + num;
        position1 = this.buttonDetailedLock.Position;
        int y4 = position1.Y;
        Point point4 = new Point(x4, y4);
        buttonDetailedLock.Position = point4;
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState1 = this.getControlState(ctrl, id, ControlState.Enabled);
      ControlState controlState2;
      switch (id)
      {
        case "142":
          EllieMae.Encompass.Forms.Label control = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblCashFromTo");
          if (this.DisablePricingChange)
            controlState2 = ControlState.Disabled;
          else if (control != null)
            control.Text = this.ToDouble(this.loan.GetSimpleField("142")) <= 0.0 ? "Cash to borrower" : "Cash from borrower";
          controlState1 = ControlState.Default;
          break;
        case "2954":
        case "2955":
          controlState1 = !this.DisablePricingChange ? (!(this.loan.GetField("2953") != "GraduatedPaymentMortgage") ? ControlState.Enabled : ControlState.Disabled) : ControlState.Disabled;
          break;
        case "2956":
        case "zoomarmrequest":
          controlState1 = !this.DisablePricingChange ? (!(this.loan.GetField("2953") != "AdjustableRate") ? ControlState.Enabled : ControlState.Disabled) : ControlState.Disabled;
          break;
        case "2957":
          controlState1 = !this.DisablePricingChange ? (!(this.loan.GetField("2953") != "OtherAmortizationType") ? ControlState.Enabled : ControlState.Disabled) : ControlState.Disabled;
          break;
        case "3044":
          controlState1 = !this.DisablePricingChange ? (!(this.loan.GetField("3046") == "Y") ? ControlState.Enabled : ControlState.Disabled) : ControlState.Disabled;
          break;
        case "3045":
          controlState1 = !this.DisablePricingChange ? (!(this.loan.GetField("3046") != "Y") ? ControlState.Enabled : ControlState.Disabled) : ControlState.Disabled;
          break;
        case "3047":
          controlState1 = !this.DisablePricingChange ? (this.loan.GetField("3046") != "Y" || this.loan.GetField("3056") == "Y" ? ControlState.Disabled : ControlState.Enabled) : ControlState.Disabled;
          break;
        case "3241":
          if (this.DisablePricingChange)
            controlState2 = ControlState.Disabled;
          else if (this.loan.GetField("3241") == string.Empty)
            this.loan.Calculator.FormCalculation("2949", (string) null, (string) null);
          controlState1 = ControlState.Default;
          break;
        case "4254":
        case "4255":
          if (this.DisablePricingChange)
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          if (this.inputData.GetField("2951") != "ConstructionOnly" && this.inputData.GetField("2951") != "ConstructionToPermanent")
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          break;
        case "4632":
        case "lockbuydowntypelookup":
          if (this.DisablePricingChange)
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          if (this.inputData.GetField("4631") == "" || this.buydownTemplates == null || this.buydownTemplates.Count == 0)
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          break;
        case "4633":
        case "4634":
        case "4635":
        case "4636":
        case "4637":
        case "4638":
        case "4639":
        case "4640":
        case "4641":
        case "4642":
        case "4643":
        case "4644":
          if (this.DisablePricingChange)
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          if (this.inputData.GetField("4632") != "" && this.fromSetting)
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          break;
        case "5038":
          if (this.DisablePricingChange)
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          if (this.inputData.GetField("2951") != "ConstructionOnly" && this.inputData.GetField("2951") != "ConstructionToPermanent" || this.inputData.GetField("4255") != "Y")
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          break;
        case "detailrequest":
        case "loanprogrequest":
        case "manageborrowers":
          if (this.FormIsForTemplate)
            controlState1 = ControlState.Disabled;
          if (this.DisablePricingChange)
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          break;
        case "getpricing":
          if (this.FormIsForTemplate || LockUtils.IsRelock(this.GetFieldValue("3841")) && !LoanLockUtils.IsAllowGetPricingForReLock(this.session.SessionObjects, this.session.LoanDataMgr))
            controlState1 = ControlState.Disabled;
          if (this.DisablePricingChange)
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          break;
        case "requestextension":
          if (this.FormIsForTemplate)
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          if (!LockUtils.IsLockExtendable(this.session.LoanDataMgr))
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          if (!(bool) this.session.ServerManager.GetServerSetting("Policies.EnableLockExtension"))
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          if (this.DisablePricingChange)
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          break;
        case "requestlock":
          if (this.FormIsForTemplate || LockUtils.IsRelock(this.GetFieldValue("3841")) && LockUtils.GetRelockState(this.session.SessionObjects, this.loan) == LockUtils.RelockState.RelockExpiredWithinDaysCap)
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          break;
        case "requestlockcancellation":
          if (this.FormIsForTemplate)
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          if (!LockUtils.IsLockCancellable(this.session.LoanDataMgr))
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          if (!(bool) this.session.ServerManager.GetServerSetting("Policies.EnableLockCancellation") || !(bool) this.session.ServerManager.GetServerSetting("Policies.EnableLockCancellation_ReqForm"))
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          if (this.DisablePricingChange)
          {
            controlState1 = ControlState.Disabled;
            break;
          }
          break;
        default:
          controlState1 = ControlState.Default;
          break;
      }
      return controlState1;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      string a = "";
      if (id == "4631")
        a = this.GetField("4631");
      base.UpdateFieldValue(id, val);
      if (id == "4631" && (string.Equals(a, "Borrower") && !string.Equals(val, "Borrower") || string.Equals(val, "Borrower") && !string.Equals(a, "Borrower")))
      {
        for (int index = 4632; index <= 4644; ++index)
          this.SetField(string.Concat((object) index), "");
      }
      if (this.loanLockFields.Contains(id))
        this.LockRequestInfoDiff = true;
      if (id == "3841")
      {
        this.UpdateReLockFields();
        this.field3841CurrentValue = val;
        this.ArrangeButtons();
      }
      if (id == "4187")
      {
        switch (val)
        {
          case "Best Efforts":
            this.SetField("3965", "Individual Best Efforts");
            break;
          case "Mandatory":
            this.SetField("3965", "Individual Mandatory");
            break;
          default:
            this.SetField("3965", "");
            break;
        }
      }
      if (id == "2951")
        this.ValidatePurposeOfLoan(true, val);
      if (id == "2952" || id == "2950")
      {
        this.AdjustFHASecondary();
        if (id == "2952" && !string.Equals(this.GetField("1172"), val, StringComparison.CurrentCultureIgnoreCase))
        {
          int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "Changing the loan type will result in the Mortgage Insurance Factors and Terms being removed from the MI Details pop-up window. Click OK to proceed.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.SetFieldFocus("DropdownBox3");
        }
      }
      if (!(id == "4632"))
        return;
      this.fromSetting = false;
    }

    private void AdjustFHASecondary()
    {
      if (this.inputData.GetField("1825") != "2020")
      {
        this.plFHASecResidence.Visible = false;
      }
      else
      {
        if (this.inputData.GetField("2952") == "FHA")
        {
          this.plFHASecResidence.Position = this.noPoint;
          this.plNoUnits.Position = new Point(this.noPoint.X, this.plFHASecResidence.Position.Y + this.plFHASecResidence.Size.Height);
          this.plFHASecResidence.Visible = true;
        }
        else
        {
          this.plNoUnits.Position = this.noPoint;
          this.plFHASecResidence.Visible = false;
        }
        if (!(this.inputData.GetField("2952") == "FHA"))
          return;
        this.lblFHASecResidence.Enabled = this.inputData.GetField("2950") == "SecondHome";
        this.chkSecResidence.Enabled = this.inputData.GetField("2950") == "SecondHome";
      }
    }

    public override void ExecAction(string action)
    {
      this.IsOnrpButtonClicked = false;
      switch (action)
      {
        case "detailrequest":
          base.ExecAction(action);
          this.SetFieldFocus("l_2092");
          break;
        case "geteffectiveonrpsettings":
          EncompassLockDeskHoursHelper lockDeskHourHelper = new EncompassLockDeskHoursHelper((IClientSession) null, Session.SessionObjects, this.session.LoanDataMgr);
          LoanChannel loanChannel = lockDeskHourHelper.GetLoanChannel();
          LockDeskHoursInfo deskHoursSettings = lockDeskHourHelper.GetLockDeskHoursSettings(loanChannel);
          if (deskHoursSettings == null)
          {
            int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, "There is no lock desk hours settings available for the loan channel.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          }
          LockDeskOnrpInfo lockDeskOnrpInfo = LockDeskHoursManager.GetEffectiveLockDeskOnrpInfo((ILockDeskHoursHelper) lockDeskHourHelper, loanChannel, this.session.LoanDataMgr.LoanData.GetField("TPO.X15"));
          int num1 = (int) new ONRPEffectiveSettings(this.session.LoanDataMgr, deskHoursSettings, lockDeskOnrpInfo).ShowDialog();
          break;
        case "getonrppricing":
          using (GetONRPPricingDialog onrpPricingDialog = new GetONRPPricingDialog((IClientSession) this.session.SessionObjects.Session, Session.SessionObjects, this.session.LoanDataMgr))
          {
            if (onrpPricingDialog.ShowDialog() != DialogResult.OK)
              break;
            this.IsOnrpButtonClicked = true;
            this.SetField("2089", onrpPricingDialog.LockDate);
            this.SetField("4060", onrpPricingDialog.LockTime);
            this.SetField("4069", onrpPricingDialog.LockDate);
            base.ExecAction("getpricingfromppe");
            break;
          }
        case "getpricing":
          string fieldValue = this.GetFieldValue("LOCKRATE.RATESTATUS");
          bool flag1 = fieldValue == "Cancelled" || fieldValue == "Expired";
          if (this.GetField("3907") != string.Empty && fieldValue == "Expired" && LockUtils.IsRelock(this.GetFieldValue("3841")))
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this.mainScreen, "This loan must be removed from the Correspondent Trade \"" + this.GetField("3907") + "\" before it can be relocked.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          }
          if (flag1 && !LockUtils.IsInactiveReLockExceededAllowed(this.session.SessionObjects, this.loan))
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this.session.Application, "The Re-Lock Limit for Inactive Locks has been exceeded. The Re-Lock Request cannot be processed.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            break;
          }
          if (this.GetField("4631") != "" && this.GetField("4632") == "" && Utils.Dialog((IWin32Window) this.mainScreen, "The Buydown Type field is blank and will not map to the correct Buydown Type value in the pricing engine. If Buydown pricing is required, please click Cancel and select the correct Buydown Type or click OK to continue.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
            break;
          if (LockUtils.IsRelock(this.GetFieldValue("3841")))
            this.getRelockPricing = true;
          base.ExecAction(action);
          break;
        case "lockbuydowntypelookup":
          this.UpdateRateLockBuydownFields(this.selectBuydownType(this.buydownTemplates));
          break;
        case "manageborrowers":
          base.ExecAction(action);
          this.zoomPanels();
          this.SetFieldFocus("l_2940");
          break;
        case "requestlock":
          if (this.GetField("3907") != string.Empty && this.GetFieldValue("3841") == "NewLock")
          {
            int num4 = (int) Utils.Dialog((IWin32Window) this.mainScreen, "This loan must be removed from the Correspondent Trade \"" + this.GetField("3907") + "\" before a new lock can be created.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          }
          if (this.GetField("3907") != string.Empty && this.GetFieldValue("LOCKRATE.RATESTATUS") == "Expired" && LockUtils.IsRelock(this.GetFieldValue("3841")))
          {
            int num5 = (int) Utils.Dialog((IWin32Window) this.mainScreen, "This loan must be removed from the Correspondent Trade \"" + this.GetField("3907") + "\" before it can be relocked.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          }
          bool flag2 = this.notAllowedChangesAfterPricingSubmitted();
          string field1 = this.GetField("2089");
          base.ExecAction(action);
          if (this.session.LoanData == null)
            break;
          bool flag3 = this.notAllowedChangesAfterPricingSubmitted();
          string field2 = this.GetField("2089");
          this.SetFieldFocus("l_2092");
          if (field1 == field2)
            break;
          this.field3841CurrentValue = this.GetField("3841");
          this.InitRequestType();
          this.UpdateReLockFields();
          this.getRelockPricing = false;
          if (!flag2 || flag3)
            break;
          this.EnablePricingFields(true);
          break;
        case "requestlockcancellation":
          if (this.GetField("3907") != string.Empty)
          {
            int num6 = (int) Utils.Dialog((IWin32Window) this.mainScreen, "This loan must be removed from the Correspondent Trade \"" + this.GetField("3907") + "\" before the lock can be cancelled.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          }
          base.ExecAction(action);
          break;
        case "resetrequestform":
          if (this.notAllowedChangesAfterPricingSubmitted())
          {
            this.resetRateLockFields();
            break;
          }
          base.ExecAction(action);
          this.SetFieldFocus("l_2940");
          break;
        default:
          base.ExecAction(action);
          break;
      }
    }

    private void resetRateLockFields()
    {
      if (Utils.Dialog((IWin32Window) null, "Pricing data will be lost from this form with a reset. Are you sure you want to reset this form with loan information?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      this.clearPricingFields();
      this.EnablePricingFields(true);
    }

    private void clearPricingFields()
    {
      Cursor.Current = Cursors.WaitCursor;
      if (!LockUtils.IsRelock(this.GetFieldValue("3841")))
      {
        string[] strArray = new string[24]
        {
          "2088",
          "2089",
          "2090",
          "2091",
          "2092",
          "2099",
          "2100",
          "2101",
          "2142",
          "2143",
          "2144",
          "2647",
          "2688",
          "2689",
          "3254",
          "3847",
          "3872",
          "3874",
          "3965",
          "4187",
          "4511",
          "4512",
          "4513",
          "4456"
        };
        foreach (string id in strArray)
        {
          if (this.session.LoanData.GetField(id) != string.Empty)
            this.UpdateFieldValue(id, "", true);
        }
      }
      else
        this.UpdateReLockFields();
      this.clearPricingSubmissionDate();
      this.editor.RefreshContents();
      Cursor.Current = Cursors.Default;
    }

    private void clearPricingSubmissionDate()
    {
      string field = this.session.LoanData.GetField("3039");
      if (!this.notAllowedPricingChangeSetting || !(field != "//") || !Utils.IsDate((object) field))
        return;
      this.UpdateFieldValue("3039", "", true);
    }

    public override bool AllowUnload()
    {
      if (this.session.StartupInfo.ProductPricingPartner != null && this.session.StartupInfo.ProductPricingPartner.IsEPPS && this.session.StartupInfo.ProductPricingPartner.VendorPlatform == VendorPlatform.EPC2 || this.session.LoanDataMgr.AllowAutoLock(false, this.getRelockPricing, false, false, this.getRelockPricing))
        return true;
      string field = this.session.LoanData.GetField("3039");
      if (!(field != "//") || !Utils.IsDate((object) field) || (!this.getRelockPricing || !this.hasNewRelockPricing()) && (!this.notAllowedPricingChangeSetting || this.getRelockPricing))
        return true;
      if (Utils.Dialog((IWin32Window) this.mainScreen, string.Format("You have received pricing on this {0}. If you navigate away from without out first submitting your request, the pricing will be lost. Do you wish to continue?", LockUtils.IsRelock(this.GetFieldValue("3841")) ? (object) LockUtils.GetRequestType(this.GetFieldValue("4209")) : (object) "lock"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return false;
      if (this.notAllowedPricingChangeSetting)
      {
        this.clearPricingFields();
        this.EnablePricingFields(true);
      }
      return true;
    }

    private bool hasNewRelockPricing()
    {
      this.RelockCurrentLockLog = LoanLockUtils.GetPreviousConfirmedLockForRelocks(this.session.LoanDataMgr);
      if (this.RelockCurrentLockLog == null)
        return true;
      Hashtable lockRequestSnapshot = this.RelockCurrentLockLog.GetLockRequestSnapshot();
      foreach (KeyValuePair<string, string> buySideToRequest in LoanLockUtils.PricingFieldsBuySideToRequestMap)
      {
        string key = buySideToRequest.Key;
        string str = lockRequestSnapshot.ContainsKey((object) key) ? lockRequestSnapshot[(object) key].ToString() : "";
        string field = this.loan.GetField(buySideToRequest.Value);
        if ((field.Equals("//") ? "" : field) != str)
          return true;
      }
      return false;
    }

    private void UpdateRateLockBuydownFields(TemporaryBuydown buyDownTemplate)
    {
      if (buyDownTemplate == null)
        return;
      this.inputData.SetCurrentField("4632", buyDownTemplate.BuydownType);
      this.inputData.SetCurrentField("4633", buyDownTemplate.Rate1);
      this.inputData.SetCurrentField("4634", buyDownTemplate.Rate2);
      this.inputData.SetCurrentField("4635", buyDownTemplate.Rate3);
      this.inputData.SetCurrentField("4636", buyDownTemplate.Rate4);
      this.inputData.SetCurrentField("4637", buyDownTemplate.Rate5);
      this.inputData.SetCurrentField("4638", buyDownTemplate.Rate6);
      this.inputData.SetCurrentField("4639", buyDownTemplate.Term1);
      this.inputData.SetCurrentField("4640", buyDownTemplate.Term2);
      this.inputData.SetCurrentField("4641", buyDownTemplate.Term3);
      this.inputData.SetCurrentField("4642", buyDownTemplate.Term4);
      this.inputData.SetCurrentField("4643", buyDownTemplate.Term5);
      this.inputData.SetCurrentField("4644", buyDownTemplate.Term6);
      this.fromSetting = true;
    }

    private void InitRequestType()
    {
      DropdownOption dropdownOption = new DropdownOption("New Lock", "NewLock");
      DropdownOption requestTypeControl = this.GetRequestTypeControl();
      ((DropdownBox) this.currentForm.FindControl("l_3841")).Options.Clear();
      ((DropdownBox) this.currentForm.FindControl("l_3841")).Options.Add(dropdownOption);
      string field1 = this.loan.GetField("LOCKRATE.RATESTATUS");
      bool flag = field1 == "Cancelled" || field1 == "Expired";
      if ((bool) this.session.ServerManager.GetServerSetting("Policies.EnableRelock") && LoanLockUtils.GetPreviousConfirmedLockForRelocks(this.session.LoanDataMgr) != null | flag)
      {
        string field2 = this.loan.GetField("LOCKRATE.RATEREQUESTSTATUS");
        if (!(field2 == "NotLocked-NoRequest") && !(field2 == "NotLocked-Request") || flag)
        {
          ((DropdownBox) this.currentForm.FindControl("l_3841")).Options.Add(requestTypeControl);
          if ((bool) this.session.ServerManager.GetServerSetting("Policies.RelockOnly") | flag)
          {
            ((DropdownBox) this.currentForm.FindControl("l_3841")).Options.Remove(dropdownOption);
            this.SetField("3841", LockUtils.GetRequestType(this.GetFieldValue("4209")));
          }
        }
      }
      if (LockUtils.IsRelock(this.GetFieldValue("3841")) && !((DropdownBox) this.currentForm.FindControl("l_3841")).Options.Contains(requestTypeControl))
        this.SetField("3841", "NewLock");
      else if (this.GetFieldValue("3841") == "NewLock" && !((DropdownBox) this.currentForm.FindControl("l_3841")).Options.Contains(dropdownOption))
        this.SetField("3841", LockUtils.GetRequestType(this.GetFieldValue("4209")));
      if (!LockUtils.IsRelock(this.GetFieldValue("3841")) || flag)
        return;
      this.ReloadRelockData();
      if (!this.isRelockFromExtension)
        return;
      this.DisplayReLockFieldsForExtension();
    }

    private DropdownOption GetRequestTypeControl()
    {
      return this.GetFieldValue("4209") == "Active Lock" ? new DropdownOption("Lock Update", "Lock Update") : new DropdownOption("Re-Lock", "ReLock");
    }

    private void UpdateReLockFields()
    {
      this.LoadCommitmentType();
      string field = this.loan.GetField("LOCKRATE.RATESTATUS");
      bool flag = field == "Cancelled" || field == "Expired";
      this.ValidatePurposeOfLoan(false, this.loan.GetField("2951"));
      if (LockUtils.IsRelock(this.GetFieldValue("3841")) && !flag)
      {
        this.DisplayReLockFields(true);
        if (LockUtils.IsRelock(this.field3841CurrentValue) && !this.notAllowedChangesAfterPricingSubmitted())
          return;
        LoanLockUtils.CacheLockRequestPriceFields(this.session.LoanDataMgr, this.standardLockPriceFieldCache);
        if (this.relockPriceFieldCache.Count == 0)
          this.ReloadRelockData();
        else
          LoanLockUtils.UncacheLockRequestPriceFields(this.session.LoanDataMgr, this.relockPriceFieldCache);
        if (!this.isRelockFromExtension)
          return;
        this.DisplayReLockFieldsForExtension();
      }
      else if (flag)
      {
        this.DisplayReLockFields(false);
        if (this.field3841CurrentValue == "ReLock")
          return;
        LoanLockUtils.CacheLockRequestPriceFields(this.session.LoanDataMgr, this.relockPriceFieldCache);
        LoanLockUtils.UncacheLockRequestPriceFields(this.session.LoanDataMgr, this.standardLockPriceFieldCache);
      }
      else
      {
        this.DisplayReLockFields(false);
        if (this.field3841CurrentValue == "" || this.field3841CurrentValue == "NewLock")
          return;
        LoanLockUtils.CacheLockRequestPriceFields(this.session.LoanDataMgr, this.relockPriceFieldCache);
        LoanLockUtils.UncacheLockRequestPriceFields(this.session.LoanDataMgr, this.standardLockPriceFieldCache);
      }
    }

    private void ValidatePurposeOfLoan(bool showmsg, string val)
    {
      if (val.IndexOf("Construction") >= 0)
      {
        ((RuntimeControl) this.currentForm.FindControl("TextBox25")).Enabled = false;
        ((RuntimeControl) this.currentForm.FindControl("TextBox8")).Enabled = false;
        if (this.loan.IsLocked("3038") & showmsg)
        {
          int num = (int) Utils.Dialog((IWin32Window) this.mainScreen, this.purposeOfLoanWarningMessage, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
          ((RuntimeControl) this.currentForm.FindControl("FieldLock4")).Visible = false;
      }
      else
      {
        ((RuntimeControl) this.currentForm.FindControl("TextBox25")).Enabled = true;
        ((RuntimeControl) this.currentForm.FindControl("TextBox8")).Enabled = true;
        ((RuntimeControl) this.currentForm.FindControl("FieldLock4")).Visible = true;
      }
    }

    private void LoadCommitmentType()
    {
      DropdownBox control = (DropdownBox) this.currentForm.FindControl("I_4187");
      bool flag = false;
      LoanChannel result;
      if (Enum.TryParse<LoanChannel>(this.GetFieldValue("2626"), out result) && this.GetFieldValue("TPO.X15") != string.Empty && result == LoanChannel.Correspondent)
      {
        string fieldValue = this.GetFieldValue("TPO.X15");
        if (fieldValue != string.Empty)
        {
          DropdownOption dropdownOption1 = new DropdownOption("Best Efforts", "Best Efforts");
          DropdownOption dropdownOption2 = new DropdownOption("Mandatory", "Mandatory");
          ExternalOriginatorManagementData originatorManagementData = this.session.ConfigurationManager.GetExternalOrganizationByTPOID(fieldValue).Where<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.OrganizationType == ExternalOriginatorOrgType.Company)).First<ExternalOriginatorManagementData>();
          if (originatorManagementData != null)
          {
            flag = true;
            if ((originatorManagementData.CommitmentUseBestEffort || originatorManagementData.CommitmentUseBestEffortLimited) && !control.Options.Contains(dropdownOption1))
              control.Options.Add(dropdownOption1);
            if (originatorManagementData.IsCommitmentDeliveryIndividual && !control.Options.Contains(dropdownOption2))
              control.Options.Add(dropdownOption2);
            if (!originatorManagementData.CommitmentUseBestEffort && !originatorManagementData.IsCommitmentDeliveryIndividual && !control.Options.Contains(dropdownOption1))
              control.Options.Add(dropdownOption1);
          }
        }
      }
      if (!flag)
      {
        this.pnlLockRequestFields.Position = this.pnlDeliveryType.Position;
        this.pnlDeliveryType.Visible = false;
      }
      if (this.pnlDeliveryType.Visible)
        this.loan.SetField("3966", "Y");
      else
        this.loan.SetField("3966", "N");
    }

    private void ReloadRelockData()
    {
      this.RelockCurrentLockLog = LoanLockUtils.GetPreviousConfirmedLockForRelocks(this.session.LoanDataMgr);
      if (this.RelockCurrentLockLog == null)
        return;
      Dictionary<string, string> requestFields = LoanLockUtils.CopyPriceDataFromSnapshotToRequestFields(this.session.LoanDataMgr, this.RelockCurrentLockLog);
      if (!requestFields.ContainsKey("3364") || !Utils.IsDate((object) requestFields["3364"]))
        return;
      this.isRelockFromExtension = true;
      this.relockPriceCache_LockExpirationDate = requestFields["3364"];
      int num = Utils.ParseInt(requestFields.ContainsKey("2150") ? (object) requestFields["2150"] : (object) (string) null, 0) + Utils.ParseInt(requestFields.ContainsKey("3431") ? (object) requestFields["3431"] : (object) (string) null, 0);
      this.relockPriceCache_LockNumOfDays = num == 0 ? "" : num.ToString();
    }

    public LockRequestLog RelockCurrentLockLog
    {
      get => this.relockCurrentLockLogVal;
      set => this.relockCurrentLockLogVal = value;
    }

    private void DisplayReLockFields(bool relock)
    {
      EllieMae.Encompass.Forms.TextBox control1 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_2090");
      EllieMae.Encompass.Forms.TextBox control2 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("TextBoxRelockLockDays");
      EllieMae.Encompass.Forms.TextBox control3 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_2091");
      EllieMae.Encompass.Forms.TextBox control4 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("TextBoxRelockExpirationDate");
      Calendar control5 = (Calendar) this.currentForm.FindControl("Calendar2");
      Calendar control6 = (Calendar) this.currentForm.FindControl("Calendar3");
      Calendar control7 = (Calendar) this.currentForm.FindControl("Calendar4");
      if (relock)
      {
        this.SetFieldReadOnly("l_2092");
        this.SetFieldReadOnly("l_2101");
        this.SetFieldReadOnly("l_2647");
        this.SetFieldReadOnly("l_2088");
        this.SetFieldReadOnly("l_2089");
        this.SetFieldReadOnly("l_2090");
        this.SetFieldReadOnly("l_2091");
        this.SetFieldReadOnly("l_3254");
        this.SetFieldReadOnly("TextBox44");
        this.SetFieldReadOnly("TextBox45");
        this.SetFieldReadOnly("TextBox46");
        this.SetFieldReadOnly("I_4187");
        control5.Enabled = false;
        control6.Enabled = false;
        control7.Enabled = false;
      }
      else
      {
        this.SetFieldAvailable("l_2092");
        this.SetFieldAvailable("l_2101");
        this.SetFieldAvailable("l_2647");
        this.SetFieldAvailable("l_2088");
        this.SetFieldAvailable("l_2089");
        this.SetFieldAvailable("l_3254");
        this.SetFieldAvailable("TextBox44");
        this.SetFieldAvailable("TextBox45");
        this.SetFieldAvailable("TextBox46");
        this.SetFieldAvailable("I_4187");
        control5.Enabled = true;
        control6.Enabled = true;
        control7.Enabled = true;
        control2.Visible = false;
        control1.Visible = true;
        control1.Enabled = true;
        this.SetFieldAvailable("l_2090");
        control4.Visible = false;
        control3.Visible = true;
        control3.Enabled = true;
        this.SetFieldAvailable("l_2091");
      }
    }

    private void DisplayReLockFieldsForExtension()
    {
      EllieMae.Encompass.Forms.TextBox control1 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_2090");
      EllieMae.Encompass.Forms.TextBox control2 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("TextBoxRelockLockDays");
      EllieMae.Encompass.Forms.TextBox control3 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_2091");
      EllieMae.Encompass.Forms.TextBox control4 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("TextBoxRelockExpirationDate");
      control2.Visible = true;
      control1.Visible = false;
      control4.Visible = true;
      control3.Visible = false;
      control2.Top = control1.Top;
      control2.Left = control1.Left;
      control2.Size = control1.Size;
      control4.Top = control3.Top;
      control4.Left = control3.Left;
      control4.Size = control3.Size;
      control2.Text = this.relockPriceCache_LockNumOfDays;
      control4.Text = this.relockPriceCache_LockExpirationDate;
    }

    private void InitRequestLockStatus()
    {
      this.SetField("4209", LockUtils.GetRequestLockStatus(this.loan));
    }

    private void InitONRPSettings()
    {
      if (this.session != null && this.session.ACL != null)
        this.userHasSecondaryRegistrationAccessRight = ((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ToolsTab_SecondaryRegistration);
      this.useEPPS = this.session.StartupInfo.ProductPricingPartner != null && this.session.StartupInfo.ProductPricingPartner.IsEPPS;
      if (this.session == null || this.session.ServerManager == null)
        return;
      IDictionary serverSettings = this.session.ServerManager.GetServerSettings("Policies");
      if (serverSettings.Contains((object) "Policies.EnableLockDeskSCHEDULE"))
        this.isLockDeskScheduleEnabled = (bool) serverSettings[(object) "Policies.EnableLockDeskSCHEDULE"];
      if (serverSettings.Contains((object) "Policies.EnableONRPRet"))
        this.isONRPRetailChannelEnabled = (bool) serverSettings[(object) "Policies.EnableONRPRet"];
      if (serverSettings.Contains((object) "Policies.EnableONRPBroker"))
        this.isONRPWholesaleChannelEnabled = (bool) serverSettings[(object) "Policies.EnableONRPBroker"];
      if (!serverSettings.Contains((object) "Policies.EnableONRPCor"))
        return;
      this.isONRPCorrespondentChannelEnabled = (bool) serverSettings[(object) "Policies.EnableONRPCor"];
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.QALOInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.Common.UI;
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
  internal class QALOInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Button buttonOrderFraud;
    private Calendar calendar1;
    private Calendar calendar2;
    private EllieMae.Encompass.Forms.GroupBox groupBoxpresentAdrBor;
    private EllieMae.Encompass.Forms.GroupBox groupBoxcurrentAdrBor;
    private EllieMae.Encompass.Forms.GroupBox groupBoxpreviousAdrBor;
    private EllieMae.Encompass.Forms.GroupBox groupBoxformerAdrBor;
    private EllieMae.Encompass.Forms.GroupBox groupBoxpresentAdrCoBor;
    private EllieMae.Encompass.Forms.GroupBox groupBoxcurrentAdrCoBor;
    private EllieMae.Encompass.Forms.GroupBox groupBoxpreviousAdrCoBor;
    private EllieMae.Encompass.Forms.GroupBox groupBoxformerAdrCoBor;
    private CategoryBox categoryBoxPropertyAdr;
    private CategoryBox categoryBoxPropertyAdr2020;
    private EllieMae.Encompass.Forms.Panel pnlUrla2020;
    private CategoryBox categoryBoxCreditInformation;
    private EllieMae.Encompass.Forms.Panel pnlFrm;
    private List<EllieMae.Encompass.Forms.Panel> panelLocalAddresses;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignAddresses;
    private List<StandardButton> selectCountryButtons;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignPanels;
    private EllieMae.Encompass.Forms.Button copyFromPresentButton;
    private EllieMae.Encompass.Forms.CheckBox loanAmountRounding;
    private EllieMae.Encompass.Forms.CheckBox chkCountyLimit;
    private VerticalRule verticalRule3;
    private CategoryBox categoryBoxVerificationInfo;
    private EllieMae.Encompass.Forms.Panel panelUrla2020;

    public QALOInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public QALOInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public QALOInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public QALOInputHandler(
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
        base.CreateControls();
        this.buttonOrderFraud = this.currentForm.FindControl("ButtonOrderFraud") as EllieMae.Encompass.Forms.Button;
        this.groupBoxpresentAdrBor = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("presentAdrBor");
        this.groupBoxcurrentAdrBor = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("currentAdrBor");
        this.groupBoxpreviousAdrBor = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("previousAdrBor");
        this.groupBoxformerAdrBor = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("formerAdrBor");
        this.groupBoxpresentAdrCoBor = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("presentAdrCoBor");
        this.groupBoxcurrentAdrCoBor = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("currentAdrCoBor");
        this.groupBoxpreviousAdrCoBor = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("previousAdrCoBor");
        this.groupBoxformerAdrCoBor = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("formerAdrCoBor");
        this.categoryBoxPropertyAdr = (CategoryBox) this.currentForm.FindControl("propertyAdr");
        this.categoryBoxPropertyAdr2020 = (CategoryBox) this.currentForm.FindControl("propertyAdr2020");
        this.pnlUrla2020 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelURLA2020");
        this.categoryBoxCreditInformation = (CategoryBox) this.currentForm.FindControl("creditInformationBox");
        this.pnlFrm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        this.loanAmountRounding = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chklar");
        this.chkCountyLimit = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chkCountyLimit");
        this.categoryBoxVerificationInfo = (CategoryBox) this.currentForm.FindControl("CategoryBox2");
        this.panelUrla2020 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelUrla2020");
        ILoanServices service = Session.Application.GetService<ILoanServices>();
        if (this.currentForm.Name == "Borrower Summary - Origination" || this.currentForm.Name == "Borrower Summary - Processing")
          this.setAddressSection();
        this.panelLocalAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignPanels = new List<EllieMae.Encompass.Forms.Panel>();
        int num = !(this.currentForm.Name != "Borrower Summary - Origination") || !(this.currentForm.Name != "Borrower Summary - Processing") ? 4 : 2;
        for (int index = 1; index <= num; ++index)
        {
          this.panelLocalAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("localAdr" + (object) index));
          this.panelForeignAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignAdr" + (object) index));
          if (this.panelForeignAddresses[index - 1] != null && this.panelLocalAddresses[index - 1] != null)
            this.panelForeignAddresses[index - 1].Position = this.panelLocalAddresses[index - 1].Position;
        }
        this.selectCountryButtons = new List<StandardButton>();
        for (int index = 1; index <= num; ++index)
          this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_FR0" + (object) index + "30"));
        if (this.currentForm.Name != "Borrower Summary - Origination" && this.currentForm.Name != "Borrower Summary - Processing")
        {
          for (int index = 1; index <= 4; ++index)
            this.panelForeignPanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignPanel" + (object) index));
        }
        if (!service.IsSSNServiceInstalled())
        {
          if (this.currentForm.FindControl("btnOrderSSNVerification") is EllieMae.Encompass.Forms.Button control1)
            control1.Visible = false;
          if (this.currentForm.FindControl("btnCheckStatus") is EllieMae.Encompass.Forms.Button control2)
            control2.Visible = false;
        }
        if (this.session != null && this.session.IsBrokerEdition() && this.currentForm.FindControl("chkSR") is EllieMae.Encompass.Forms.CheckBox control)
          control.Visible = false;
        if (this.currentForm.Name == "Borrower Summary - Origination")
        {
          this.calendar1 = this.currentForm.FindControl("Calendar3") as Calendar;
          this.calendar2 = this.currentForm.FindControl("Calendar4") as Calendar;
          if (this.loanAmountRounding != null)
            this.loanAmountRounding.Visible = this.inputData.GetField("1722") != "FHA";
          if (this.categoryBoxPropertyAdr2020.Visible)
            this.panelUrla2020.Position = new Point(this.categoryBoxPropertyAdr2020.Position.X, this.categoryBoxPropertyAdr2020.Position.Y + this.categoryBoxPropertyAdr2020.Size.Height - 250);
          else
            this.panelUrla2020.Position = new Point(this.categoryBoxPropertyAdr.Position.X, this.categoryBoxPropertyAdr.Position.Y + this.categoryBoxPropertyAdr.Size.Height - 245);
          this.pnlFrm.Size = new Size(this.pnlFrm.Size.Width, this.panelUrla2020.Position.Y + this.panelUrla2020.Size.Height + 257);
        }
        else if (this.currentForm.Name == "Borrower Summary - Processing")
        {
          this.calendar1 = this.currentForm.FindControl("Calendar2") as Calendar;
          this.calendar2 = this.currentForm.FindControl("Calendar3") as Calendar;
          this.verticalRule3 = this.currentForm.FindControl("VerticalRule3") as VerticalRule;
        }
        this.copyFromPresentButton = this.currentForm.FindControl("cpyPresent") as EllieMae.Encompass.Forms.Button;
      }
      catch (Exception ex)
      {
      }
    }

    public void setAddressSection()
    {
      string field = this.inputData.GetField("1825");
      if (this.groupBoxpresentAdrBor != null && field != "2020")
      {
        this.groupBoxpresentAdrBor.Visible = true;
        if (this.groupBoxcurrentAdrBor != null)
          this.groupBoxcurrentAdrBor.Visible = false;
      }
      if (this.groupBoxcurrentAdrBor != null && field == "2020")
      {
        this.groupBoxcurrentAdrBor.Visible = true;
        this.groupBoxpresentAdrBor.Visible = false;
      }
      if (this.groupBoxpresentAdrCoBor != null && field != "2020")
      {
        this.groupBoxpresentAdrCoBor.Visible = true;
        if (this.groupBoxcurrentAdrCoBor != null)
          this.groupBoxcurrentAdrCoBor.Visible = false;
      }
      if (this.groupBoxcurrentAdrCoBor != null && field == "2020")
      {
        this.groupBoxcurrentAdrCoBor.Visible = true;
        this.groupBoxpresentAdrCoBor.Visible = false;
      }
      if (this.groupBoxpreviousAdrBor != null && field != "2020")
      {
        this.groupBoxpreviousAdrBor.Visible = true;
        if (this.groupBoxformerAdrBor != null)
          this.groupBoxformerAdrBor.Visible = false;
      }
      if (this.groupBoxformerAdrBor != null && field == "2020")
      {
        this.groupBoxformerAdrBor.Visible = true;
        this.groupBoxpreviousAdrBor.Visible = false;
      }
      if (this.groupBoxpreviousAdrCoBor != null && field != "2020")
      {
        this.groupBoxpreviousAdrCoBor.Visible = true;
        if (this.groupBoxformerAdrCoBor != null)
          this.groupBoxformerAdrCoBor.Visible = false;
      }
      if (this.groupBoxformerAdrCoBor != null && field == "2020")
      {
        this.groupBoxformerAdrCoBor.Visible = true;
        this.groupBoxpreviousAdrCoBor.Visible = false;
      }
      if (this.categoryBoxPropertyAdr != null && field != "2020")
      {
        this.categoryBoxPropertyAdr.Visible = true;
        if (this.categoryBoxPropertyAdr2020 != null)
          this.categoryBoxPropertyAdr2020.Visible = false;
      }
      if (this.categoryBoxPropertyAdr2020 != null && field == "2020")
      {
        this.categoryBoxPropertyAdr2020.Visible = true;
        this.categoryBoxPropertyAdr.Visible = false;
      }
      Size size1 = this.groupBoxcurrentAdrBor.Size;
      int height1 = size1.Height;
      size1 = this.groupBoxpresentAdrBor.Size;
      int height2 = size1.Height;
      int num = height1 - height2;
      Point position;
      if (field == "2020" && this.groupBoxcurrentAdrBor != null && this.groupBoxcurrentAdrCoBor != null && this.groupBoxformerAdrCoBor != null && this.groupBoxformerAdrBor != null && this.pnlUrla2020 != null && this.groupBoxpresentAdrBor != null && this.groupBoxpresentAdrCoBor != null && this.groupBoxpreviousAdrBor != null && this.groupBoxpreviousAdrCoBor != null && this.pnlFrm != null)
      {
        this.groupBoxcurrentAdrBor.Position = this.groupBoxpresentAdrBor.Position;
        this.groupBoxcurrentAdrCoBor.Position = this.groupBoxpresentAdrCoBor.Position;
        EllieMae.Encompass.Forms.GroupBox groupBoxformerAdrBor = this.groupBoxformerAdrBor;
        position = this.groupBoxpreviousAdrBor.Position;
        int x1 = position.X;
        position = this.groupBoxpreviousAdrBor.Position;
        int y1 = position.Y + num;
        Point point1 = new Point(x1, y1);
        groupBoxformerAdrBor.Position = point1;
        EllieMae.Encompass.Forms.GroupBox boxformerAdrCoBor = this.groupBoxformerAdrCoBor;
        position = this.groupBoxpreviousAdrCoBor.Position;
        int x2 = position.X;
        position = this.groupBoxpreviousAdrCoBor.Position;
        int y2 = position.Y + num;
        Point point2 = new Point(x2, y2);
        boxformerAdrCoBor.Position = point2;
        EllieMae.Encompass.Forms.Panel pnlUrla2020 = this.pnlUrla2020;
        position = this.pnlUrla2020.Position;
        int x3 = position.X;
        position = this.pnlUrla2020.Position;
        int y3 = position.Y + 2 * num + 5;
        Point point3 = new Point(x3, y3);
        pnlUrla2020.Position = point3;
        EllieMae.Encompass.Forms.Panel pnlFrm = this.pnlFrm;
        size1 = this.pnlFrm.Size;
        int width = size1.Width;
        size1 = this.pnlFrm.Size;
        int height3 = size1.Height + 2 * num;
        Size size2 = new Size(width, height3);
        pnlFrm.Size = size2;
      }
      if (field == "2020" && this.currentForm.Name == "Borrower Summary - Origination" && this.categoryBoxVerificationInfo != null)
      {
        CategoryBox verificationInfo = this.categoryBoxVerificationInfo;
        position = this.categoryBoxVerificationInfo.Position;
        int x = position.X;
        position = this.categoryBoxVerificationInfo.Position;
        int y = position.Y + 2 * num;
        Point point = new Point(x, y);
        verificationInfo.Position = point;
      }
      if (field == "2020" && this.currentForm.Name == "Borrower Summary - Origination" && this.categoryBoxCreditInformation != null && this.categoryBoxPropertyAdr != null)
      {
        CategoryBox creditInformation = this.categoryBoxCreditInformation;
        position = this.categoryBoxCreditInformation.Position;
        int x4 = position.X;
        position = this.categoryBoxCreditInformation.Position;
        int y4 = position.Y + 2 * num;
        Point point4 = new Point(x4, y4);
        creditInformation.Position = point4;
        CategoryBox boxPropertyAdr2020 = this.categoryBoxPropertyAdr2020;
        position = this.categoryBoxPropertyAdr.Position;
        int x5 = position.X;
        position = this.categoryBoxPropertyAdr.Position;
        int y5 = position.Y + 2 * num;
        Point point5 = new Point(x5, y5);
        boxPropertyAdr2020.Position = point5;
      }
      if (!(field == "2020") || !(this.currentForm.Name == "Borrower Summary - Processing") || this.categoryBoxPropertyAdr == null)
        return;
      this.categoryBoxPropertyAdr2020.Position = this.categoryBoxPropertyAdr.Position;
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
            bool flag = this.GetField("FR0" + (object) (index + 1) + "29") == "Y";
            this.selectCountryButtons[index].Visible = flag;
            this.panelLocalAddresses[index].Visible = !flag;
            this.panelForeignAddresses[index].Visible = flag;
          }
        }
      }
      if (this.currentForm.Name != "Borrower Summary - Origination" && this.currentForm.Name != "Borrower Summary - Processing")
      {
        if (this.GetField("1825") == "2020")
        {
          for (int index = 0; index < 2; ++index)
            this.panelForeignPanels[index].Visible = true;
        }
        else
        {
          for (int index = 0; index < 4; ++index)
            this.panelForeignPanels[index].Visible = false;
        }
      }
      if (this.currentForm.Name == "Borrower Summary - Origination" && this.loanAmountRounding != null && this.chkCountyLimit != null)
      {
        bool flag = this.inputData.GetField("1172") != "FHA";
        if (flag)
          this.chkCountyLimit.Position = new Point(8, 355);
        else
          this.chkCountyLimit.Position = new Point(8, 362);
        this.loanAmountRounding.Visible = flag;
      }
      if (this.verticalRule3 == null)
        return;
      if (this.inputData.GetField("1825") == "2020")
        this.verticalRule3.Size = new Size(this.verticalRule3.Size.Width, 590);
      else
        this.verticalRule3.Size = new Size(this.verticalRule3.Size.Width, 262);
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "101":
        case "102":
        case "103":
        case "104":
        case "1063":
        case "107":
        case "108":
        case "11":
        case "110":
        case "111":
        case "112":
        case "113":
        case "116":
        case "117":
        case "FR0104":
        case "FR0204":
        case "FR0304":
        case "FR0404":
        case "copybrw2":
        case "updatelinkedborrower":
        case "updatelinkedcoborrower":
          return controlState;
        case "1051":
        case "1393":
        case "ccprog":
        case "income":
        case "loanprog":
        case "orderavm":
        case "subfin":
        case "totalmonthlypayment":
        case "viewcredit":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "1266":
        case "1267":
          if (this.loan.GetField("608") != "GraduatedPaymentMortgage")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "1888":
          if (this.GetField("1172") != "HELOC")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "2827":
          if (!this.FormIsForTemplate && this.loan.GetField("2822") == "")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "364":
          if (this.FormIsForTemplate || InputHandlerBase.LockLoanNumber)
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "3894":
          if (!this.session.UserInfo.IsAdministrator() && (!this.session.LoanDataMgr.GetFieldAccessList().ContainsKey((object) "3894") || this.session.LoanDataMgr.GetFieldAccessRights("3894") != BizRule.FieldAccessRight.Edit))
            return ControlState.Disabled;
          goto case "101";
        case "3941":
          return ControlState.Disabled;
        case "432":
        case "761":
        case "762":
          if (this.GetField("3941") == "Y")
          {
            if (this.calendar1 != null)
              this.calendar1.Enabled = false;
            if (this.calendar2 != null)
              this.calendar2.Enabled = false;
            return ControlState.Disabled;
          }
          if (this.calendar1 != null)
            this.calendar1.Enabled = true;
          if (this.calendar2 != null)
          {
            this.calendar2.Enabled = true;
            goto case "101";
          }
          else
            goto case "101";
        case "4494":
          if (this.GetField("420") != "SecondLien")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "4719":
          if (this.GetField("3865") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "4720":
          if (this.GetField("3871") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "4830":
          controlState = this.inputData.IsLocked("4752") ? ControlState.Enabled : ControlState.Disabled;
          goto case "101";
        case "4920":
          return this.loan.GetField("1490") == string.Empty ? ControlState.Disabled : ControlState.Enabled;
        case "4935":
          return this.loan.GetField("1480") == string.Empty ? ControlState.Disabled : ControlState.Enabled;
        case "4970":
        case "4971":
        case "4972":
        case "MORNET.X30":
          if (this.GetField("5027") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "5018":
        case "5019":
        case "5020":
        case "5021":
          controlState = !(this.GetField("5028") != "Y") ? ControlState.Enabled : ControlState.Disabled;
          goto case "101";
        case "9":
          if (this.loan.GetField("19") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "994":
          if (this.loan.GetField("608") != "OtherAmortizationType")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "995":
        case "zoomarm":
          if (this.loan.GetField("608") != "AdjustableRate" && this.inputData.GetField("1172") != "HELOC")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "FR0116":
          if (this.loan.GetField("FR0115") != "Rent")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "FR0129":
          if (this.copyFromPresentButton != null)
          {
            this.copyFromPresentButton.Enabled = !(this.loan.GetField("FR0129") == "Y");
            goto case "101";
          }
          else
            goto case "101";
        case "FR0130":
          if (this.loan.GetField("FR0129") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "FR0216":
          if (this.loan.GetField("FR0215") != "Rent")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "FR0230":
          if (this.loan.GetField("FR0229") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "FR0316":
          if (this.loan.GetField("FR0315") != "Rent")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "FR0330":
          if (this.loan.GetField("FR0329") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "FR0416":
          if (this.loan.GetField("FR0415") != "Rent")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "FR0430":
          if (this.loan.GetField("FR0429") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "accesslenders":
        case "importliab":
        case "orderappraisal":
        case "ordercredit":
        case "orderflood":
        case "orderfraud":
        case "orderlife":
        case "ordertitle":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
          {
            if (!new eFolderAccessRights(Session.LoanDataMgr).CanRequestServices)
              controlState = ControlState.Disabled;
            ((EllieMae.Encompass.Forms.Button) ctrl).HoverText = controlState != ControlState.Disabled ? "" : "You must have Document Tracking permission to use this feature.";
            goto case "101";
          }
        case "addregistration":
          if (this.FormIsForTemplate || this.loan.IsFieldReadOnly("2827") || this.loan.IsFieldReadOnly("2823") || this.loan.IsFieldReadOnly("2824") || this.loan.IsFieldReadOnly("2825") || this.loan.IsFieldReadOnly("2826"))
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "baseincome":
          if (this.GetField("1825") == "2020")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "checkstatus":
        case "orderssnverification":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "editregistration":
          if (this.loan.IsTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "getami":
          if (this.FormIsForTemplate || this.GetField("5027") == "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "getmfi":
          if (this.FormIsForTemplate || this.GetField("5028") == "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "lookup4970m":
          if (this.GetField("5027") == "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "mersmin":
          if (this.FormIsForTemplate || this.loan.IsFieldReadOnly("1051"))
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "mfilookup":
          if (this.GetField("5028") == "Y")
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "productandpricing":
          if (this.FormIsForTemplate || this.session.StartupInfo.ProductPricingPartner != null && this.session.StartupInfo.ProductPricingPartner.IsEPPS && LoanLockUtils.GetAllowedRequestType(this.session.SessionObjects, this.session.LoanDataMgr) == LoanLockUtils.AllowedRequestType.ReLockOnly && !LoanLockUtils.IsAllowGetPricingForReLock(this.session.SessionObjects, this.session.LoanDataMgr))
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        case "syncbrw":
          if (this.loan.GetField("36").Trim() == string.Empty && this.loan.GetField("37").Trim() == string.Empty)
            controlState = ControlState.Disabled;
          if (this.loan.GetField("LOID") == string.Empty)
            controlState = ControlState.Disabled;
          if (!this.FormIsForTemplate && !this.session.LoanDataMgr.Writable && this.loan.GetField("LOID") != this.session.UserInfo.Userid)
            controlState = ControlState.Disabled;
          EllieMae.Encompass.Forms.Button button = (EllieMae.Encompass.Forms.Button) ctrl;
          if (controlState == ControlState.Disabled)
            button.HoverText = "To update your contacts:" + Environment.NewLine + "Enter borrower’s first and last name" + Environment.NewLine + "Select loan officer on the File Started worksheet";
          else
            button.HoverText = string.Empty;
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "101";
          }
          else
            goto case "101";
        default:
          controlState = ControlState.Default;
          goto case "101";
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id == "1771" && this.ToDouble(val) > 100.0)
        val = "100.00";
      if (id == "4494" && (val == "" || val == "1") && this.loan.GetField("420") == "SecondLien")
        val = this.loan.GetField("4494");
      base.UpdateFieldValue(id, val);
      switch (id)
      {
        case "608":
          if (val != "OtherAmortizationType")
            base.UpdateFieldValue("994", string.Empty);
          if (!(val != "GraduatedPaymentMortgage"))
            break;
          base.UpdateFieldValue("1266", string.Empty);
          base.UpdateFieldValue("1267", string.Empty);
          break;
        case "1264":
          this.ClearFileContact(id);
          break;
      }
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
      {
        case 340714504:
          if (!(action == "mersmin"))
            return;
          this.SetFieldFocus("l_1051");
          return;
        case 668647084:
          if (!(action == "totalmonthlypayment"))
            return;
          goto label_61;
        case 679671351:
          if (!(action == "copyaddr"))
            return;
          this.SetFieldFocus("l_11");
          return;
        case 837191551:
          if (!(action == "ordertitle"))
            return;
          goto label_54;
        case 979029941:
          if (!(action == "copybrw"))
            return;
          this.SetFieldFocus("l_68");
          return;
        case 1112563569:
          if (!(action == "checkstatus"))
            return;
          break;
        case 1497812540:
          if (!(action == "importliab"))
            return;
          goto label_52;
        case 1676980613:
          if (!(action == "copybrw2"))
            return;
          this.SetFieldFocus("l_fr0212");
          return;
        case 1890696842:
          if (!(action == "lenderbrokerdata"))
            return;
          this.SetFieldFocus("l_3537");
          return;
        case 1894936370:
          if (!(action == "orderssnverification"))
            return;
          break;
        case 1895711686:
          if (!(action == "ordercredit"))
            return;
          goto label_52;
        case 2023485534:
          if (!(action == "income"))
            return;
          this.SetFieldFocus("l_3");
          return;
        case 2068317490:
          if (!(action == "contactlen"))
            return;
          this.GetContactItem("1264");
          this.SetFieldFocus("l_1264");
          return;
        case 2287650426:
          if (!(action == "orderappraisal"))
            return;
          goto label_54;
        case 2410213549:
          if (!(action == "orderflood"))
            return;
          goto label_54;
        case 2477757109:
          if (!(action == "addregistration"))
            return;
          goto label_61;
        case 2775811035:
          int num = action == "syncbrw" ? 1 : 0;
          return;
        case 2946509463:
          if (!(action == "orderfraud"))
            return;
          this.SetFieldFocus("l_36");
          return;
        case 3443531722:
          if (!(action == "subfin"))
            return;
          this.SetFieldFocus("I_8");
          return;
        case 3582764703:
          if (!(action == "loanprog"))
            return;
          goto label_51;
        case 3737892597:
          if (!(action == "productandpricing"))
            return;
          this.SetFieldFocus("l_1264");
          return;
        case 3996438474:
          if (!(action == "accesslenders"))
            return;
          goto label_54;
        case 4155076599:
          if (!(action == "ccprog"))
            return;
          goto label_51;
        case 4233502404:
          if (!(action == "zoomarm"))
            return;
          this.SetFieldFocus("l_995");
          return;
        default:
          return;
      }
      this.SetFieldFocus("l_3249");
      return;
label_51:
      this.SetFieldFocus("l_364");
      return;
label_52:
      this.SetFieldFocus("l_67");
      return;
label_54:
      this.SetFieldFocus("l_11");
      return;
label_61:
      this.SetFieldFocus("l_1822");
    }
  }
}

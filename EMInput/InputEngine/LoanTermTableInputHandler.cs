// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LoanTermTableInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LoanTermTableInputHandler
  {
    private Form currentForm;
    private Panel panelLoanAmount;
    private Panel panelInterestRate;
    private Panel panelPI1Line;
    private Panel panelPI2Line;
    private Panel panelInterestOnly;
    private Panel panelSeeAP;
    private Panel panelPrepayment;
    private Panel panelPrepaymentMonthYearLine;
    private Panel panelPrepaymentMonthYearFields;
    private Label labelPrepaymentPenaltyPayoff;
    private Label labelPrepaymentPenaltyFirst;
    private Panel panelBalloon;
    private TextBox boxField5;
    private TextBox boxHUD51;
    private FieldLock boxField5Lock;
    private FieldLock boxHUD51Lock;
    private Label labelMonthlyPI;
    private Button btnEditCustomize;
    private Panel panelPI1IOFLine;
    private CheckBox chkApplyActualPaymentChange;
    private Label escrowPaymentFrequency;
    private Panel panelAdjustsEveryInterest;
    private Panel panelAdjustsOnceInterest;
    private Panel pnlBorBuydown;
    private Panel pnlNonBorBuydown;
    private LoanData loan;
    private IHtmlInput inputData;
    private Dictionary<string, RuntimeControl> pjtPanels = new Dictionary<string, RuntimeControl>();
    private string miSymbol = "";
    private string escrowSymbol = "";
    private string DASH = "-             ";
    private string handlerName = "";
    private Sessions.Session session;

    public LoanTermTableInputHandler(
      Form currentForm,
      IHtmlInput inputData,
      string handlerName,
      Sessions.Session session)
    {
      this.inputData = inputData;
      this.handlerName = handlerName;
      this.session = session;
      if (inputData is LoanData)
        this.loan = (LoanData) inputData;
      this.currentForm = currentForm;
      if (handlerName.IndexOf("REGZ") == -1)
      {
        this.panelLoanAmount = (Panel) this.currentForm.FindControl(nameof (panelLoanAmount));
        this.panelInterestRate = (Panel) this.currentForm.FindControl(nameof (panelInterestRate));
        this.panelPI1Line = (Panel) this.currentForm.FindControl(nameof (panelPI1Line));
        this.panelPI2Line = (Panel) this.currentForm.FindControl(nameof (panelPI2Line));
        this.panelPI1IOFLine = (Panel) this.currentForm.FindControl(nameof (panelPI1IOFLine));
        this.panelInterestOnly = (Panel) this.currentForm.FindControl(nameof (panelInterestOnly));
        this.panelSeeAP = (Panel) this.currentForm.FindControl(nameof (panelSeeAP));
        this.panelPrepayment = (Panel) this.currentForm.FindControl(nameof (panelPrepayment));
        this.panelPrepaymentMonthYearLine = (Panel) this.currentForm.FindControl(nameof (panelPrepaymentMonthYearLine));
        this.panelPrepaymentMonthYearFields = (Panel) this.currentForm.FindControl(nameof (panelPrepaymentMonthYearFields));
        this.panelBalloon = (Panel) this.currentForm.FindControl(nameof (panelBalloon));
        this.labelPrepaymentPenaltyPayoff = (Label) this.currentForm.FindControl(nameof (labelPrepaymentPenaltyPayoff));
        this.labelPrepaymentPenaltyFirst = (Label) this.currentForm.FindControl(nameof (labelPrepaymentPenaltyFirst));
        this.labelMonthlyPI = (Label) this.currentForm.FindControl(nameof (labelMonthlyPI));
        this.boxField5 = (TextBox) this.currentForm.FindControl(nameof (boxField5));
        this.boxHUD51 = (TextBox) this.currentForm.FindControl(nameof (boxHUD51));
        this.boxHUD51.Top = this.boxField5.Top;
        this.boxField5Lock = (FieldLock) this.currentForm.FindControl(nameof (boxField5Lock));
        this.boxHUD51Lock = (FieldLock) this.currentForm.FindControl(nameof (boxHUD51Lock));
        this.boxHUD51Lock.Top = this.boxField5Lock.Top;
        this.panelPI1IOFLine.Left = this.panelPI1Line.Left;
        this.panelAdjustsEveryInterest = (Panel) this.currentForm.FindControl("PanelAdjustsEveryInterest");
        this.panelAdjustsOnceInterest = (Panel) this.currentForm.FindControl("PanelAdjustsOnceInterest");
      }
      else
      {
        try
        {
          this.pnlNonBorBuydown = (Panel) this.currentForm.FindControl(nameof (pnlNonBorBuydown));
          this.pnlBorBuydown = (Panel) this.currentForm.FindControl(nameof (pnlBorBuydown));
          this.pnlBorBuydown.Position = new Point(this.pnlNonBorBuydown.Left, this.pnlNonBorBuydown.Top);
        }
        catch
        {
        }
      }
      this.chkApplyActualPaymentChange = (CheckBox) this.currentForm.FindControl(nameof (chkApplyActualPaymentChange));
      try
      {
        this.escrowPaymentFrequency = (Label) this.currentForm.FindControl("l_escrowpayfreq");
      }
      catch (Exception ex)
      {
      }
      if (inputData.GetField("19") == "ConstructionOnly" && inputData.GetField("608") == "AdjustableRate")
        this.chkApplyActualPaymentChange.Visible = false;
      try
      {
        for (int index1 = 1; index1 <= 4; ++index1)
        {
          this.pjtPanels.Add("panelColumn" + (object) index1, (RuntimeControl) this.currentForm.FindControl("panelColumn" + (object) index1));
          this.pjtPanels.Add("panelYear" + (object) index1 + "1", (RuntimeControl) this.currentForm.FindControl("panelYear" + (object) index1 + "1"));
          for (int index2 = 1; index2 <= 2; ++index2)
          {
            this.pjtPanels.Add("panelMax" + (object) index1 + (object) index2, (RuntimeControl) this.currentForm.FindControl("panelMax" + (object) index1 + (object) index2));
            this.pjtPanels.Add("pjtMaxBox" + (object) index1 + (object) index2, (RuntimeControl) this.currentForm.FindControl("pjtMaxBox" + (object) index1 + (object) index2));
            this.pjtPanels.Add("pjtMinBox" + (object) index1 + (object) index2, (RuntimeControl) this.currentForm.FindControl("pjtMinBox" + (object) index1 + (object) index2));
          }
        }
        for (int index = 1; index <= 4; ++index)
        {
          this.pjtPanels.Add("panelSingleYear" + (object) index, (RuntimeControl) this.currentForm.FindControl("panelSingleYear" + (object) index));
          if (index <= 3)
            this.pjtPanels.Add("pjtVerticalLine" + (object) index, (RuntimeControl) this.currentForm.FindControl("pjtVerticalLine" + (object) index));
        }
        this.pjtPanels.Add("panelFinalPayment", (RuntimeControl) this.currentForm.FindControl("panelFinalPayment"));
        this.pjtPanels.Add("panelBallon", (RuntimeControl) this.currentForm.FindControl("panelBallon"));
        this.btnEditCustomize = (Button) this.currentForm.FindControl(nameof (btnEditCustomize));
      }
      catch (Exception ex)
      {
      }
      if (this.loan == null || this.loan.Calculator == null || this.loan.Calculator.CalcOnDemand(CalcOnDemandEnum.PaymentSchedule))
        return;
      bool performanceEnabled = this.loan.Calculator.PerformanceEnabled;
      this.loan.Calculator.PerformanceEnabled = false;
      this.loan.Calculator.FormCalculation("19", (string) null, (string) null);
      this.loan.Calculator.PerformanceEnabled = performanceEnabled;
    }

    public ControlState GetControlState(string id, ControlState currentState)
    {
      switch (id)
      {
        case "1659":
        case "NEWHUD.X5":
        case "NEWHUD.X6":
        case "NEWHUD.X8":
          return this.inputData.GetField("LOANTERMTABLE.CUSTOMIZE") == "Y" ? ControlState.Enabled : ControlState.Disabled;
        case "4746":
          string field1 = this.inputData.GetField("3");
          string field2 = this.inputData.GetField("608");
          return !string.IsNullOrEmpty(field1) && Utils.ParseDouble((object) field1) == 0.0 && field2 != "Fixed" ? ControlState.Disabled : ControlState.Enabled;
        case "675":
          if (this.handlerName.EndsWith("REGZLEInputHandler") || this.handlerName.EndsWith("REGZCDInputHandler"))
            return currentState;
          return this.inputData.GetField("LOANTERMTABLE.CUSTOMIZE") == "Y" ? ControlState.Enabled : ControlState.Disabled;
        case "LOANTERMTABLE.CUSTOMIZE":
        case "PAYMENTTABLE.CUSTOMIZE":
          if (id == "PAYMENTTABLE.CUSTOMIZE")
            this.btnEditCustomize.Enabled = this.inputData.GetField(id) == "Y";
          return this.inputData.GetField("19") == "Other" ? ControlState.Disabled : ControlState.Enabled;
        default:
          return currentState;
      }
    }

    public void UpdateFieldValue(string id, string val)
    {
    }

    public string GetFieldValue(string id, string val)
    {
      this.miSymbol = "";
      this.escrowSymbol = "";
      if (id == "3" || id == "NEWHUD.X555")
        return Utils.FormatLEAndCDPercentageValue(val);
      if (id == "RE88395.X121" && this.inputData.GetField(id) != "")
        return Utils.RemoveEndingZeros(Decimal.Parse(Utils.RemoveEndingZeros(this.inputData.GetField(id), true)).ToString("N"));
      if (id == "LE1.X24" || id == "LE1.X29")
        return Utils.RemoveEndingZeros(val, true);
      switch (id)
      {
        case "LE1.X42":
          return Utils.ParseDecimal((object) this.inputData.GetField("LE1.X43")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "LE1.X43":
          return Utils.ParseDecimal((object) this.inputData.GetField("LE1.X42")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "LE1.X48":
          return this.inputData.GetField("LE1.XD48");
        case "LE1.X51":
          return Utils.ParseDecimal((object) this.inputData.GetField("LE1.X52")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "LE1.X52":
          return Utils.ParseDecimal((object) this.inputData.GetField("LE1.X51")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "LE1.X57":
          return this.inputData.GetField("LE1.XD57");
        case "LE1.X60":
          return Utils.ParseDecimal((object) this.inputData.GetField("LE1.X61")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "LE1.X61":
          return Utils.ParseDecimal((object) this.inputData.GetField("LE1.X60")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "LE1.X66":
          return this.inputData.GetField("LE1.XD66");
        case "LE1.X69":
          return Utils.ParseDecimal((object) this.inputData.GetField("LE1.X70")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "LE1.X70":
          return Utils.ParseDecimal((object) this.inputData.GetField("LE1.X69")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "LE1.X75":
          return Utils.ParseDecimal((object) this.inputData.GetField("LE1.X74")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "CD1.X8":
          return Utils.ParseDecimal((object) this.inputData.GetField("CD1.X9")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "CD1.X9":
          return Utils.ParseDecimal((object) this.inputData.GetField("CD1.X8")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "CD1.X13":
          return Utils.ParseDecimal((object) this.inputData.GetField("CD1.X14")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "CD1.X14":
          return Utils.ParseDecimal((object) this.inputData.GetField("CD1.X13")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "CD1.X17":
          return Utils.ParseDecimal((object) this.inputData.GetField("CD1.X18")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "CD1.X18":
          return Utils.ParseDecimal((object) this.inputData.GetField("CD1.X17")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "CD1.X22":
          return Utils.ParseDecimal((object) this.inputData.GetField("CD1.X23")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "CD1.X23":
          return Utils.ParseDecimal((object) this.inputData.GetField("CD1.X22")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "CD1.X26":
          return Utils.ParseDecimal((object) this.inputData.GetField("CD1.X27")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "CD1.X27":
          return Utils.ParseDecimal((object) this.inputData.GetField("CD1.X26")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "CD1.X31":
          return Utils.ParseDecimal((object) this.inputData.GetField("CD1.X32")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "CD1.X32":
          return Utils.ParseDecimal((object) this.inputData.GetField("CD1.X31")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "CD1.X35":
          return Utils.ParseDecimal((object) this.inputData.GetField("CD1.X36")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "CD1.X36":
          return Utils.ParseDecimal((object) this.inputData.GetField("CD1.X35")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "CD1.X40":
          return Utils.ParseDecimal((object) this.inputData.GetField("CD1.X41")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        case "CD1.X41":
          return Utils.ParseDecimal((object) this.inputData.GetField("CD1.X40")) != Utils.ParseDecimal((object) val) ? Utils.RemoveEndingZeros(val) : val;
        default:
          if (id == "CD1.X11" || id == "LE1.X45")
          {
            if (Utils.ParseDecimal((object) val) == 0M)
            {
              this.miSymbol = "0";
              return this.miSymbol;
            }
            this.miSymbol = this.DASH;
            return val;
          }
          if (id == "CD1.X12" || id == "LE1.X46")
          {
            if (Utils.ParseDecimal((object) val) == 0M)
            {
              val = this.inputData.GetField(id.Substring(0, 5) + "D" + id.Substring(5));
              if (val == "-")
                return this.DASH;
            }
            else
              this.escrowSymbol = this.DASH;
            return val;
          }
          if (id == "CD1.X20" || id == "CD1.X29" || id == "CD1.X38" || id == "LE1.X54" || id == "LE1.X63" || id == "LE1.X72" || id == "CD1.X21" || id == "CD1.X30" || id == "CD1.X39" || id == "LE1.X55" || id == "LE1.X64" || id == "LE1.X73")
          {
            string fieldValue = this.checkSymbolForFinalPayment(id);
            if (fieldValue != null)
              return fieldValue;
            if (Utils.ParseDecimal((object) val) == 0M)
            {
              val = this.inputData.GetField(id.Substring(0, 5) + "D" + id.Substring(5));
              if (val == "-")
                return this.DASH;
            }
            return val;
          }
          return id == "LE1.X27" ? (Utils.ParseInt((object) this.inputData.GetField("LE1.X27")) == 1 && this.inputData.GetField("LE1.X91") == "Year" ? this.inputData.GetField("LE1.XD27") : val) : ((id == "4113" || id == "KBYO.XD4113") && val == "" && (this.inputData is DisclosedLEHandler || this.inputData is DisclosedCDHandler) ? this.inputData.GetField("3") : val);
      }
    }

    private string checkSymbolForFinalPayment(string id)
    {
      if (!this.inputData.GetField("NEWHUD2.XPJT").EndsWith("Final_Payment"))
        return (string) null;
      int num = Utils.ParseInt((object) this.inputData.GetField("NEWHUD2.XPJTCOLUMNS"), 0);
      if (num <= 1)
        return (string) null;
      if (this.miSymbol.Trim() != "-" && (id == "CD1.X20" && num == 2 || id == "CD1.X29" && num == 3 || id == "CD1.X38" && num == 4 || id == "LE1.X54" && num == 2 || id == "LE1.X63" && num == 3 || id == "LE1.X72" && num == 4))
      {
        if (this.inputData.GetField("PAYMENTTABLE.CUSTOMIZE") == "Y")
        {
          for (int index = 0; index < num; ++index)
          {
            if (id.StartsWith("CD1.X"))
            {
              if (Utils.ParseDecimal((object) this.inputData.GetField("CD1.X" + (object) (11 + index * 9))) != 0M)
                return this.DASH;
            }
            else if (id.StartsWith("LE1.X") && Utils.ParseDecimal((object) this.inputData.GetField("LE1.X" + (object) (45 + index * 9))) != 0M)
              return this.DASH;
          }
        }
        return Utils.ParseDecimal((object) this.inputData.GetField(id.StartsWith("CD1.X") ? "CD1.X11" : "LE1.X45")) == 0M && Utils.ParseDecimal((object) this.inputData.GetField(id.StartsWith("CD1.X") ? "CD1.X20" : "LE1.X54")) == 0M ? "0" : this.DASH;
      }
      if (!(this.escrowSymbol.Trim() != "-") || (!(id == "CD1.X21") || num != 2) && (!(id == "CD1.X30") || num != 3) && (!(id == "CD1.X39") || num != 4) && (!(id == "LE1.X55") || num != 2) && (!(id == "LE1.X64") || num != 3) && (!(id == "LE1.X73") || num != 4))
        return (string) null;
      if (this.inputData.GetField("PAYMENTTABLE.CUSTOMIZE") == "Y")
      {
        for (int index = 0; index < num; ++index)
        {
          if (id.StartsWith("CD1.X"))
          {
            if (Utils.ParseDecimal((object) this.inputData.GetField("CD1.X" + (object) (12 + index * 9))) != 0M)
              return this.DASH;
          }
          else if (id.StartsWith("LE1.X") && Utils.ParseDecimal((object) this.inputData.GetField("LE1.X" + (object) (46 + index * 9))) != 0M)
            return this.DASH;
        }
      }
      return Utils.ParseDecimal((object) this.inputData.GetField(id.StartsWith("CD1.X") ? "CD1.X12" : "LE1.X46")) == 0M && Utils.ParseDecimal((object) this.inputData.GetField(id.StartsWith("CD1.X") ? "CD1.X21" : "LE1.X55")) == 0M ? "0" : this.DASH;
    }

    public void SetSectionStatus(string id)
    {
      switch (id)
      {
        case "NEWHUD.X6":
          this.panelLoanAmount.Visible = this.inputData.GetField(id) == "Y";
          break;
        case "NEWHUD.X8":
        case "NEWHUD.X5":
          this.panelInterestRate.Visible = this.inputData.GetField("NEWHUD.X5") == "Y";
          if (this.panelInterestOnly.Visible)
          {
            if (this.inputData.GetField("19") == "ConstructionToPermanent" && this.inputData.GetField("608") != "AdjustableRate")
            {
              this.panelAdjustsEveryInterest.Visible = false;
              this.panelAdjustsOnceInterest.Position = this.panelAdjustsEveryInterest.Position;
              this.panelAdjustsOnceInterest.Visible = true;
            }
            else
            {
              this.panelAdjustsOnceInterest.Visible = false;
              this.panelAdjustsEveryInterest.Visible = true;
            }
          }
          if (this.inputData.GetField("NEWHUD.X8") == "Y" && this.inputData.GetField("19").StartsWith("Construction") && this.inputData.GetField("SYS.X6") != "B")
          {
            if (this.inputData.IsLocked("LE1.X19") && this.inputData.IsLocked("LE1.X20") && this.inputData.GetField("LE1.X19") == "" && this.inputData.GetField("LE1.X20") == "")
            {
              this.panelPI1Line.Visible = false;
              this.panelPI1IOFLine.Visible = true;
            }
            else
            {
              this.panelPI1Line.Visible = true;
              this.panelPI1IOFLine.Visible = false;
            }
          }
          else if (this.inputData.GetField("NEWHUD.X8") == "Y" && this.inputData.GetField("19") == "ConstructionToPermanent")
          {
            if (this.inputData.GetField("608") == "Fixed")
              this.panelPI1Line.Visible = !(this.panelPI1IOFLine.Visible = true);
            else
              this.panelPI1IOFLine.Visible = !(this.panelPI1Line.Visible = true);
          }
          else if (this.inputData.GetField("NEWHUD.X8") == "Y" && (this.inputData.GetField("608") == "Fixed" && Utils.ParseInt((object) this.inputData.GetField("1177")) > 0 || this.inputData.GetField("608") != "Fixed" && Utils.ParseInt((object) this.inputData.GetField("1177")) > 0 && Utils.ParseInt((object) this.inputData.GetField("1177")) < Utils.ParseInt((object) this.inputData.GetField("696"))))
          {
            this.panelPI1Line.Visible = false;
            this.panelPI1IOFLine.Visible = true;
          }
          else
          {
            this.panelPI1Line.Visible = this.inputData.GetField("NEWHUD.X8") == "Y";
            this.panelPI1IOFLine.Visible = false;
          }
          this.panelPI2Line.Visible = this.inputData.GetField("NEWHUD.X8") == "Y";
          if (this.panelPI2Line.Visible)
          {
            string field = this.inputData.GetField("19");
            int num1 = Utils.ParseInt((object) this.inputData.GetField("4"));
            int num2 = Utils.ParseInt((object) this.inputData.GetField("1177"));
            int num3 = Utils.ParseInt((object) this.inputData.GetField("325"));
            if (!field.Contains("Construction") && num2 > 0 && num1 > num3 && (num2 == num3 || num2 == num3 - 1))
              this.panelSeeAP.Visible = false;
            else if (this.inputData.GetField("19").StartsWith("Construction") && this.inputData.GetField("SYS.X6") != "B")
            {
              if (this.inputData.IsLocked("CD4.X30") && this.inputData.IsLocked("CD4.X33") && this.inputData.IsLocked("CD4.X34") && this.inputData.GetField("CD4.X30") == "" && this.inputData.GetField("CD4.X33") == "" && this.inputData.GetField("CD4.X34") == "")
                this.panelSeeAP.Visible = false;
              else
                this.panelSeeAP.Visible = true;
            }
            else if ((this.inputData.GetField("608") == "AdjustableRate" || this.inputData.GetField("608") == "") && Utils.ParseInt((object) this.inputData.GetField("1176"), 0) == 0 && Utils.ParseInt((object) this.inputData.GetField("1177"), 0) == 0 && Utils.ParseInt((object) this.inputData.GetField("1712"), 0) == 0 && Utils.ParseInt((object) this.inputData.GetField("CD4.X26"), 0) == 0 && this.inputData.GetField("CD4.X28") == "" || field == "ConstructionOnly")
              this.panelSeeAP.Visible = false;
            else
              this.panelSeeAP.Visible = true;
          }
          else
            this.panelSeeAP.Visible = false;
          if (this.panelPI1IOFLine.Visible)
            this.panelPI2Line.Top = this.panelPI1IOFLine.Top + this.panelPI1IOFLine.Size.Height + 1;
          else if (this.panelPI1Line.Visible)
            this.panelPI2Line.Top = this.panelPI1Line.Top + this.panelPI1Line.Size.Height + 1;
          else
            this.panelPI2Line.Top = this.panelPI1Line.Top;
          this.panelInterestOnly.Visible = this.panelPI2Line.Visible && (Utils.ParseInt((object) this.inputData.GetField("1177")) > 0 || this.inputData.GetField("19").StartsWith("Construction") && Utils.ParseInt((object) this.inputData.GetField("1176")) > 0);
          if (this.panelInterestOnly.Visible)
            this.panelInterestOnly.Top = this.panelPI2Line.Top + this.panelPI2Line.Size.Height + 1;
          if (this.panelSeeAP.Visible)
            this.panelSeeAP.Top = this.panelInterestOnly.Visible ? this.panelInterestOnly.Top + this.panelInterestOnly.Size.Height : this.panelPI2Line.Top + this.panelPI2Line.Size.Height;
          if (!this.panelPI2Line.Visible)
            break;
          this.labelMonthlyPI.Text = this.inputData.GetField("3291") == "Biweekly" ? "Bi-weekly Principal & Interest" : "Monthly Principal & Interest";
          break;
        case "675":
          this.panelPrepayment.Visible = this.inputData.GetField(id) == "Y";
          break;
        case "1659":
          this.panelBalloon.Visible = this.inputData.GetField(id) == "Y";
          break;
        case "5":
        case "HUD51":
          bool flag = this.inputData.GetField("3291") == "Biweekly";
          this.boxField5.Visible = !flag;
          this.boxField5Lock.Visible = !flag;
          this.boxHUD51.Visible = flag;
          this.boxHUD51Lock.Visible = flag;
          break;
        case "LE1.X27":
        case "LE1.X91":
          if (!this.panelPrepayment.Visible)
            break;
          if (this.inputData.GetField("LE1.X27") == "1" && this.inputData.GetField("LE1.X91") == "Year")
          {
            this.labelPrepaymentPenaltyFirst.Visible = false;
            this.panelPrepaymentMonthYearFields.Left = this.labelPrepaymentPenaltyPayoff.Size.Width + 4;
            break;
          }
          this.labelPrepaymentPenaltyFirst.Visible = true;
          this.panelPrepaymentMonthYearFields.Left = this.labelPrepaymentPenaltyPayoff.Size.Width + this.labelPrepaymentPenaltyFirst.Size.Width + 4;
          break;
      }
    }

    public void SetLayout()
    {
      if (this.escrowPaymentFrequency != null)
        this.escrowPaymentFrequency.Text = this.inputData.GetField("423") == "Biweekly" ? "Bi-weekly" : "a month";
      bool flag1 = this.inputData.GetField("PAYMENTTABLE.CUSTOMIZE") == "Y";
      int num1 = !(this.inputData.GetField("19") == "ConstructionToPermanent") || Utils.ParseInt((object) this.inputData.GetField("1176"), 0) <= 0 ? Utils.ParseInt((object) this.inputData.GetField("1177"), 0) : Utils.ParseInt((object) this.inputData.GetField("1176"), 0);
      string field1 = this.inputData.GetField("608");
      if (this.chkApplyActualPaymentChange != null && this.inputData != null)
      {
        if (this.inputData.GetField("19") == "ConstructionOnly" && field1 == "AdjustableRate")
          this.chkApplyActualPaymentChange.Visible = false;
        else if (this.inputData.GetField("PAYMENTTABLE.CUSTOMIZE") == "Y" || field1 != "AdjustableRate")
          this.chkApplyActualPaymentChange.Visible = false;
        else
          this.chkApplyActualPaymentChange.Visible = true;
      }
      bool flag2 = this.inputData.GetField("CASASRN.X141") == "Borrower";
      bool flag3 = this.inputData.GetField("COMPLIANCEVERSION.CASASRNX141") == "Y" || this.inputData.GetField("COMPLIANCEVERSION.NEWBUYDOWNENABLED") != "Y";
      if (this.pnlNonBorBuydown != null)
        this.pnlNonBorBuydown.Visible = !flag2 && !flag3;
      if (this.pnlBorBuydown != null)
        this.pnlBorBuydown.Visible = flag2 | flag3;
      for (int index = 1; index <= 4; ++index)
      {
        if (this.inputData.GetField("CD1.X" + (object) (8 + (index - 1) * 9)) == this.inputData.GetField("CD1.X" + (object) (9 + (index - 1) * 9)) || Utils.ParseDouble((object) this.inputData.GetField("CD1.X" + (object) (8 + (index - 1) * 9))) == 0.0 || Utils.ParseDouble((object) this.inputData.GetField("CD1.X" + (object) (9 + (index - 1) * 9))) == 0.0)
        {
          this.pjtPanels["panelMax" + (object) index + "1"].Visible = false;
          this.pjtPanels["panelMax" + (object) index + "2"].Visible = false;
          this.pjtPanels["pjtMaxBox" + (object) index + "1"].Top = this.pjtPanels["panelMax" + (object) index + "1"].Top - 3;
          this.pjtPanels["pjtMaxBox" + (object) index + "2"].Top = this.pjtPanels["panelMax" + (object) index + "2"].Top - 3;
        }
        else
        {
          this.pjtPanels["panelMax" + (object) index + "1"].Visible = true;
          this.pjtPanels["panelMax" + (object) index + "2"].Visible = true;
          this.pjtPanels["pjtMaxBox" + (object) index + "1"].Top = this.pjtPanels["panelMax" + (object) index + "1"].Top + 19;
          this.pjtPanels["pjtMaxBox" + (object) index + "2"].Top = this.pjtPanels["panelMax" + (object) index + "2"].Top + 19;
        }
      }
      int num2 = Utils.ParseInt((object) this.inputData.GetField("NEWHUD2.XPJTCOLUMNS"));
      if (num2 < 0)
        num2 = 0;
      if (Utils.ParseInt((object) this.inputData.GetField("CD1.X7"), 0) == 1)
      {
        this.pjtPanels["panelYear11"].Visible = false;
        this.pjtPanels["panelSingleYear1"].Visible = true;
        this.pjtPanels["panelSingleYear1"].Top = this.pjtPanels["panelYear11"].Top;
        this.pjtPanels["panelSingleYear1"].Left = this.pjtPanels["panelYear11"].Left;
      }
      else
      {
        this.pjtPanels["panelYear11"].Visible = true;
        this.pjtPanels["panelSingleYear1"].Visible = false;
      }
      if (Utils.ParseInt((object) this.inputData.GetField("CD1.X15"), 0) == Utils.ParseInt((object) this.inputData.GetField("CD1.X16"), 0) && Utils.ParseInt((object) this.inputData.GetField("CD1.X16"), 0) != 0)
      {
        this.pjtPanels["panelYear21"].Visible = false;
        this.pjtPanels["panelSingleYear2"].Visible = true;
        this.pjtPanels["panelSingleYear2"].Top = this.pjtPanels["panelYear21"].Top;
        this.pjtPanels["panelSingleYear2"].Left = this.pjtPanels["panelYear21"].Left;
      }
      else
      {
        this.pjtPanels["panelYear21"].Visible = true;
        this.pjtPanels["panelSingleYear2"].Visible = false;
      }
      if (Utils.ParseInt((object) this.inputData.GetField("CD1.X24"), 0) == Utils.ParseInt((object) this.inputData.GetField("CD1.X25"), 0) && Utils.ParseInt((object) this.inputData.GetField("CD1.X25"), 0) != 0)
      {
        this.pjtPanels["panelYear31"].Visible = false;
        this.pjtPanels["panelSingleYear3"].Visible = true;
        this.pjtPanels["panelSingleYear3"].Top = this.pjtPanels["panelYear31"].Top;
        this.pjtPanels["panelSingleYear3"].Left = this.pjtPanels["panelYear31"].Left;
      }
      else
      {
        this.pjtPanels["panelYear31"].Visible = true;
        this.pjtPanels["panelSingleYear3"].Visible = false;
      }
      if (Utils.ParseInt((object) this.inputData.GetField("CD1.X33"), 0) == Utils.ParseInt((object) this.inputData.GetField("CD1.X34"), 0) && Utils.ParseInt((object) this.inputData.GetField("CD1.X34"), 0) != 0)
      {
        this.pjtPanels["panelYear41"].Visible = false;
        this.pjtPanels["panelSingleYear4"].Visible = true;
        this.pjtPanels["panelSingleYear4"].Top = this.pjtPanels["panelYear41"].Top;
        this.pjtPanels["panelSingleYear4"].Left = this.pjtPanels["panelYear41"].Left;
      }
      else
      {
        this.pjtPanels["panelYear41"].Visible = true;
        this.pjtPanels["panelSingleYear4"].Visible = false;
      }
      if (num2 == 0)
        return;
      int num3 = (this.pjtPanels["panelColumn1"].Size.Width * 4 - this.pjtPanels["panelColumn1"].Size.Width * num2) / (num2 + 1);
      int num4 = (this.pjtPanels["panelColumn1"].Size.Width * 4 - 3 * (num2 - 1)) / num2;
      for (int index = 4; index > 1; --index)
      {
        if (index <= num2)
        {
          this.pjtPanels["panelColumn" + (object) index].Visible = true;
          this.pjtPanels["pjtVerticalLine" + (object) (index - 1)].Visible = true;
        }
        else
        {
          this.pjtPanels["panelColumn" + (object) index].Visible = false;
          this.pjtPanels["pjtVerticalLine" + (object) (index - 1)].Visible = false;
        }
      }
      int num5 = 176 + num3;
      int num6 = 176 + num4;
      for (int index = 1; index <= 3 && this.pjtPanels["panelColumn" + (object) index].Visible; ++index)
      {
        this.pjtPanels["panelColumn" + (object) index].Left = num5;
        num5 += this.pjtPanels["panelColumn" + (object) index].Size.Width + num3;
        this.pjtPanels["pjtVerticalLine" + (object) index].Left = num6;
        num6 += num4 + 3;
      }
      string field2 = this.inputData.GetField("NEWHUD2.XPJT");
      if (field2.EndsWith("Final_Payment") || num2 > 1 && this.inputData.GetSimpleField("CD1.X" + (object) (15 + (num2 - 2) * 9)) == "")
      {
        this.pjtPanels["panelYear" + (object) num2 + "1"].Visible = false;
        this.pjtPanels["panelSingleYear" + (object) num2].Visible = false;
        this.pjtPanels["panelFinalPayment"].Visible = true;
        this.pjtPanels["panelFinalPayment"].Top = 7;
        this.pjtPanels["panelFinalPayment"].Left = this.pjtPanels["panelColumn" + (object) num2].Left;
        this.pjtPanels["panelBallon"].Visible = field2 == "Balloon_Final_Payment";
      }
      else
      {
        this.pjtPanels["panelFinalPayment"].Visible = false;
        this.pjtPanels["panelBallon"].Visible = false;
      }
    }
  }
}

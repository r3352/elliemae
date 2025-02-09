// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CCOSTPROGInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class CCOSTPROGInputHandler : InputHandlerBase
  {
    private IWin32Window owner;

    public CCOSTPROGInputHandler(
      IWin32Window owner,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, input, htmldoc, form, property)
    {
    }

    public CCOSTPROGInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, input, htmldoc, form, property)
    {
      this.owner = owner;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id.StartsWith("SYS.X") && val == "N")
        val = string.Empty;
      if (!(id == "1663") && !(id == "1665") && !(id == "232") && !(id == "230"))
      {
        if (id == "1848" || id == "1847")
        {
          double num = this.ToDouble(val);
          if (num != 0.0)
            val = num.ToString("N3");
        }
      }
      else
      {
        double num = this.ToDouble(val);
        if (num != 0.0)
          val = num.ToString("N2");
      }
      if (id == "L725" || id == "L209")
      {
        this.inputData.SetField(id, val);
        double num = Math.Round(this.ToDouble(this.inputData.GetSimpleField("L209")) / 100.0 * this.ToDouble(this.inputData.GetSimpleField("L725")), 2);
        if (num == 0.0)
          this.inputData.SetField("L210", "");
        else
          this.inputData.SetField("L210", num.ToString("N2"));
      }
      else
        this.inputData.SetField(id, val);
    }

    protected override string GetFieldValue(string id, FieldSource fieldSource)
    {
      string field = this.inputData.GetField(id);
      return (id == "230" || id == "232") && field != string.Empty && Utils.IsDecimal((object) field) ? Utils.ParseDouble((object) field).ToString("N2") : field;
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      this.getControlState(ctrl, id, ControlState.Enabled);
      ControlState controlState;
      if (id == "1663" || id == "1665" || id == "1847" || id == "1848")
      {
        controlState = ControlState.Default;
        this.FormatAlphaNumericField(ctrl, id);
      }
      else
        controlState = ControlState.Default;
      return controlState;
    }

    public override void ExecAction(string action)
    {
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
      {
        case 191989852:
          if (!(action == "hazins"))
            return;
          this.HandleHousingExpense("Hazard Insurance", "l_L251", "1750", "1322", "");
          return;
        case 511528202:
          if (!(action == "cityfee"))
            return;
          this.HandleFeeList("city", "1637", "l_1637");
          return;
        case 861718158:
          if (!(action == "contactmic"))
            return;
          goto label_63;
        case 888963497:
          if (!(action == "contactcre"))
            return;
          goto label_63;
        case 1014055698:
          if (!(action == "contactflo"))
            return;
          goto label_63;
        case 1093376327:
          if (!(action == "contactdoc"))
            return;
          goto label_63;
        case 1132459806:
          if (!(action == "statefee"))
            return;
          this.HandleFeeList("state", "1638", "l_1638");
          return;
        case 1181978758:
          if (!(action == "ownercoverage"))
            return;
          this.setCoverageFee(false);
          return;
        case 1473801479:
          if (!(action == "statetax"))
            return;
          this.setRecordingFee(RecordingFeeDialog.FeeTypes.StateTax);
          return;
        case 1628156329:
          if (!(action == "lendercoverage"))
            return;
          this.setCoverageFee(true);
          return;
        case 1797042560:
          if (!(action == "contactund"))
            return;
          goto label_63;
        case 3050596908:
          if (!(action == "recordingfee"))
            return;
          this.setRecordingFee(RecordingFeeDialog.FeeTypes.RecordingFee);
          return;
        case 3100944149:
          if (!(action == "localtax"))
            return;
          this.setRecordingFee(RecordingFeeDialog.FeeTypes.LocalTax);
          return;
        case 3165400720:
          if (!(action == "contactesc"))
            return;
          goto label_63;
        case 3302304154:
          if (!(action == "mtginsreserv"))
            return;
          break;
        case 3709982155:
          if (!(action == "mtginsprem"))
            return;
          break;
        case 3892120224:
          if (!(action == "contacttit"))
            return;
          goto label_63;
        case 3893795657:
          if (!(action == "contacthoi"))
            return;
          goto label_63;
        case 3949279623:
          if (!(action == "taxesreserv"))
            return;
          this.HandleHousingExpense("Taxes Reserved", "l_1386", "1751", "1752", "");
          return;
        case 4113929720:
          if (!(action == "userfee2"))
            return;
          this.HandleFeeList("user", "1640", "l_1640");
          return;
        case 4130707339:
          if (!(action == "userfee3"))
            return;
          this.HandleFeeList("user", "1643", "l_1643");
          return;
        case 4164262577:
          if (!(action == "userfee1"))
            return;
          this.HandleFeeList("user", "373", "l_373");
          return;
        case 4180047638:
          if (!(action == "contactapp"))
            return;
          goto label_63;
        case 4246466566:
          if (!(action == "contactatt"))
            return;
          goto label_63;
        default:
          return;
      }
      using (MIPDialog mipDialog = new MIPDialog(this.inputData, false, this.session))
      {
        if (mipDialog.ShowDialog((IWin32Window) this.mainScreen) != DialogResult.OK)
          return;
        this.UpdateContents();
        return;
      }
label_63:
      this.GetContactItem(action);
    }

    private void HandleHousingExpense(
      string action,
      string nextField,
      string priceTypeID,
      string rateID,
      string monthID)
    {
      using (InsuranceDialog insuranceDialog = new InsuranceDialog(action, this.inputData, this.session))
      {
        if (insuranceDialog.ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
        {
          if (priceTypeID != string.Empty)
            this.inputData.SetField(priceTypeID, insuranceDialog.PriceType.ToString());
          if (rateID != string.Empty)
            this.inputData.SetField(rateID, insuranceDialog.RateFactor.ToString());
          if (action == "Mortgage Insurance Premium" && insuranceDialog.MonthsMIP > 0)
            this.inputData.SetField(monthID, insuranceDialog.MonthsMIP.ToString());
          else if (action == "Mortgage Insurance Reserved" && insuranceDialog.UseREGZMI != string.Empty)
            this.inputData.SetField(monthID, insuranceDialog.UseREGZMI);
          this.UpdateContents();
        }
      }
      this.SetFieldFocus(nextField);
    }

    private void HandleFeeList(string feeType, string targetID, string nextField)
    {
      FeeListDialog feeListDialog = new FeeListDialog(feeType, (LoanData) null, this.session);
      if (feeListDialog.ShowDialog((IWin32Window) this.mainScreen) == DialogResult.OK)
      {
        this.inputData.SetField(targetID, feeListDialog.FeeDescription);
        this.UpdateContents();
      }
      this.SetFieldFocus(nextField);
    }

    private void setRecordingFee(RecordingFeeDialog.FeeTypes feeType)
    {
      using (RecordingFeeDialog recordingFeeDialog = new RecordingFeeDialog(feeType, this.inputData, false, RecordingFeeDialog.FormTypes.GFEItemization))
      {
        if (recordingFeeDialog.ShowDialog((IWin32Window) Session.MainForm) == DialogResult.OK)
          this.RefreshContents();
        switch (feeType)
        {
          case RecordingFeeDialog.FeeTypes.RecordingFee:
            this.SetFieldFocus("l_1636");
            break;
          case RecordingFeeDialog.FeeTypes.LocalTax:
            this.SetFieldFocus("l_1637");
            break;
          case RecordingFeeDialog.FeeTypes.StateTax:
            this.SetFieldFocus("l_1638");
            break;
        }
      }
    }

    private void setCoverageFee(bool forLender)
    {
      using (CoverageDialog coverageDialog = new CoverageDialog(this.inputData, forLender, false))
      {
        if (coverageDialog.ShowDialog((IWin32Window) Session.MainForm) == DialogResult.OK)
          this.RefreshContents();
        if (forLender)
          this.SetFieldFocus("l_652");
        else
          this.SetFieldFocus("l_1633");
      }
    }
  }
}

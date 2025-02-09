// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CONSTRUCTIONMANAGEMENT_PROJECTDATAInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class CONSTRUCTIONMANAGEMENT_PROJECTDATAInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Panel pnlForm;
    private EllieMae.Encompass.Forms.Label labelFooter;
    private EllieMae.Encompass.Forms.GroupBox takeOutGroupBox;
    private Calendar calTakeOutCommitment;
    private Calendar calTitleInsurance;
    private Calendar calSurvey;
    private Calendar calPermits;
    private Calendar calUtilityLetters;
    private Calendar calPlansAndSpecifications;
    private Calendar calConstructionContract;
    private Calendar calBudget;
    private Calendar calContractorsAgreement;
    private Calendar calArchitectsCertificate;
    private Calendar calEnvironmentAssessment;
    private Calendar calSoilReport;
    private Calendar calWaterTest;
    private Calendar calPercolationTest;
    private Calendar calPaymentAndPerformanceBonds;
    private Calendar calLienAgentNortCarolina;
    private Calendar calFloodHazard;
    private Calendar calListOfConstructionAgreements;
    private Calendar calOther;
    private EllieMae.Encompass.Forms.TextBox textTakeOutCommitment;
    private EllieMae.Encompass.Forms.TextBox textTitleInsurance;
    private EllieMae.Encompass.Forms.TextBox textSurvey;
    private EllieMae.Encompass.Forms.TextBox textPermits;
    private EllieMae.Encompass.Forms.TextBox textUtilityLetters;
    private EllieMae.Encompass.Forms.TextBox textPlansAndSpecifications;
    private EllieMae.Encompass.Forms.TextBox textConstructionContract;
    private EllieMae.Encompass.Forms.TextBox textBudget;
    private EllieMae.Encompass.Forms.TextBox textContractorsAgreement;
    private EllieMae.Encompass.Forms.TextBox textArchitectsCertificate;
    private EllieMae.Encompass.Forms.TextBox textEnvironmentAssessment;
    private EllieMae.Encompass.Forms.TextBox textSoilReport;
    private EllieMae.Encompass.Forms.TextBox textWaterTest;
    private EllieMae.Encompass.Forms.TextBox textPercolationTest;
    private EllieMae.Encompass.Forms.TextBox textPaymentAndPerformanceBonds;
    private EllieMae.Encompass.Forms.TextBox textLienAgentNortCarolina;
    private EllieMae.Encompass.Forms.TextBox textFloodHazard;
    private EllieMae.Encompass.Forms.TextBox textListOfConstructionAgreements;
    private EllieMae.Encompass.Forms.TextBox textOther;

    public CONSTRUCTIONMANAGEMENT_PROJECTDATAInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public CONSTRUCTIONMANAGEMENT_PROJECTDATAInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public CONSTRUCTIONMANAGEMENT_PROJECTDATAInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public CONSTRUCTIONMANAGEMENT_PROJECTDATAInputHandler(
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
      base.CreateControls();
      try
      {
        this.takeOutGroupBox = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("takeOutGroupBox");
        this.pnlForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        this.labelFooter = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label1");
        this.calTakeOutCommitment = (Calendar) this.currentForm.FindControl("calConstX16");
        this.calTitleInsurance = (Calendar) this.currentForm.FindControl("calConstX18");
        this.calSurvey = (Calendar) this.currentForm.FindControl("calConstX20");
        this.calPermits = (Calendar) this.currentForm.FindControl("calConstX22");
        this.calUtilityLetters = (Calendar) this.currentForm.FindControl("calConstX24");
        this.calPlansAndSpecifications = (Calendar) this.currentForm.FindControl("calConstX26");
        this.calConstructionContract = (Calendar) this.currentForm.FindControl("calConstX28");
        this.calBudget = (Calendar) this.currentForm.FindControl("calConstX30");
        this.calContractorsAgreement = (Calendar) this.currentForm.FindControl("calConstX32");
        this.calArchitectsCertificate = (Calendar) this.currentForm.FindControl("calConstX34");
        this.calEnvironmentAssessment = (Calendar) this.currentForm.FindControl("calConstX36");
        this.calSoilReport = (Calendar) this.currentForm.FindControl("calConstX38");
        this.calWaterTest = (Calendar) this.currentForm.FindControl("calConstX40");
        this.calPercolationTest = (Calendar) this.currentForm.FindControl("calConstX42");
        this.calPaymentAndPerformanceBonds = (Calendar) this.currentForm.FindControl("calConstX44");
        this.calLienAgentNortCarolina = (Calendar) this.currentForm.FindControl("calConstX46");
        this.calFloodHazard = (Calendar) this.currentForm.FindControl("calConstX48");
        this.calListOfConstructionAgreements = (Calendar) this.currentForm.FindControl("calConstX50");
        this.calOther = (Calendar) this.currentForm.FindControl("calConstX52");
        this.textTakeOutCommitment = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX16");
        this.textTitleInsurance = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX18");
        this.textSurvey = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX20");
        this.textPermits = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX22");
        this.textUtilityLetters = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX24");
        this.textPlansAndSpecifications = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX26");
        this.textConstructionContract = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX28");
        this.textBudget = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX30");
        this.textContractorsAgreement = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX32");
        this.textArchitectsCertificate = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX34");
        this.textEnvironmentAssessment = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX36");
        this.textSoilReport = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX38");
        this.textWaterTest = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX40");
        this.textPercolationTest = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX42");
        this.textPaymentAndPerformanceBonds = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX44");
        this.textLienAgentNortCarolina = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX46");
        this.textFloodHazard = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX48");
        this.textListOfConstructionAgreements = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX50");
        this.textOther = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_ConstX52");
        this.calTakeOutCommitment.Enabled = this.textTakeOutCommitment.Enabled = this.inputData.GetField("CONST.X15") == "Y";
        this.calTitleInsurance.Enabled = this.textTitleInsurance.Enabled = this.inputData.GetField("CONST.X17") == "Y";
        this.calSurvey.Enabled = this.textSurvey.Enabled = this.inputData.GetField("CONST.X19") == "Y";
        this.calPermits.Enabled = this.textPermits.Enabled = this.inputData.GetField("CONST.X21") == "Y";
        this.calUtilityLetters.Enabled = this.textUtilityLetters.Enabled = this.inputData.GetField("CONST.X23") == "Y";
        this.calPlansAndSpecifications.Enabled = this.textPlansAndSpecifications.Enabled = this.inputData.GetField("CONST.X25") == "Y";
        this.calConstructionContract.Enabled = this.textConstructionContract.Enabled = this.inputData.GetField("CONST.X27") == "Y";
        this.calBudget.Enabled = this.textBudget.Enabled = this.inputData.GetField("CONST.X29") == "Y";
        this.calContractorsAgreement.Enabled = this.textContractorsAgreement.Enabled = this.inputData.GetField("CONST.X31") == "Y";
        this.calArchitectsCertificate.Enabled = this.textArchitectsCertificate.Enabled = this.inputData.GetField("CONST.X33") == "Y";
        this.calEnvironmentAssessment.Enabled = this.textEnvironmentAssessment.Enabled = this.inputData.GetField("CONST.X35") == "Y";
        this.calSoilReport.Enabled = this.textSoilReport.Enabled = this.inputData.GetField("CONST.X37") == "Y";
        this.calWaterTest.Enabled = this.textWaterTest.Enabled = this.inputData.GetField("CONST.X39") == "Y";
        this.calPercolationTest.Enabled = this.textPercolationTest.Enabled = this.inputData.GetField("CONST.X41") == "Y";
        this.calPaymentAndPerformanceBonds.Enabled = this.textPaymentAndPerformanceBonds.Enabled = this.inputData.GetField("CONST.X43") == "Y";
        this.calLienAgentNortCarolina.Enabled = this.textLienAgentNortCarolina.Enabled = this.inputData.GetField("CONST.X45") == "Y";
        this.calFloodHazard.Enabled = this.textFloodHazard.Enabled = this.inputData.GetField("CONST.X47") == "Y";
        this.calListOfConstructionAgreements.Enabled = this.textListOfConstructionAgreements.Enabled = this.inputData.GetField("CONST.X49") == "Y";
        this.calOther.Enabled = this.textOther.Enabled = this.inputData.GetField("CONST.X51") == "Y";
      }
      catch (Exception ex)
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      this.getControlState(ctrl, id, ControlState.Enabled);
      ControlState controlState;
      switch (id)
      {
        case "CONST.X53":
          controlState = !(this.inputData.GetField("CONST.X51") == "Y") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "VEND.X1010":
        case "VEND.X1011":
        case "VEND.X1012":
        case "VEND.X1013":
        case "VEND.X1014":
        case "VEND.X1015":
        case "VEND.X1016":
        case "VEND.X1017":
        case "VEND.X1018":
        case "VEND.X1019":
        case "VEND.X1020":
        case "VEND.X1021":
        case "VEND.X1022":
        case "VEND.X1023":
          controlState = !(this.inputData.GetField("VEND.X1009") == "Y") ? ControlState.Enabled : ControlState.Disabled;
          break;
        default:
          controlState = ControlState.Default;
          break;
      }
      return controlState;
    }

    protected override string GetFieldValue(string id, FieldSource fieldSource)
    {
      return this.inputData.GetField(id);
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      switch (id)
      {
        case "CONST.X15":
          this.textTakeOutCommitment.Enabled = this.calTakeOutCommitment.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X16", "//");
          break;
        case "CONST.X17":
          this.textTitleInsurance.Enabled = this.calTitleInsurance.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X18", "//");
          break;
        case "CONST.X19":
          this.textSurvey.Enabled = this.calSurvey.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X20", "//");
          break;
        case "CONST.X21":
          this.textPermits.Enabled = this.calPermits.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X22", "//");
          break;
        case "CONST.X23":
          this.textUtilityLetters.Enabled = this.calUtilityLetters.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X24", "//");
          break;
        case "CONST.X25":
          this.textPlansAndSpecifications.Enabled = this.calPlansAndSpecifications.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X26", "//");
          break;
        case "CONST.X27":
          this.textConstructionContract.Enabled = this.calConstructionContract.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X28", "//");
          break;
        case "CONST.X29":
          this.textBudget.Enabled = this.calBudget.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X30", "//");
          break;
        case "CONST.X31":
          this.textContractorsAgreement.Enabled = this.calContractorsAgreement.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X32", "//");
          break;
        case "CONST.X33":
          this.textArchitectsCertificate.Enabled = this.calArchitectsCertificate.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X34", "//");
          break;
        case "CONST.X35":
          this.textEnvironmentAssessment.Enabled = this.calEnvironmentAssessment.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X36", "//");
          break;
        case "CONST.X37":
          this.textSoilReport.Enabled = this.calSoilReport.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X38", "//");
          break;
        case "CONST.X39":
          this.textWaterTest.Enabled = this.calWaterTest.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X40", "//");
          break;
        case "CONST.X41":
          this.textPercolationTest.Enabled = this.calPercolationTest.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X42", "//");
          break;
        case "CONST.X43":
          this.textPaymentAndPerformanceBonds.Enabled = this.calPaymentAndPerformanceBonds.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X44", "//");
          break;
        case "CONST.X45":
          this.textLienAgentNortCarolina.Enabled = this.calLienAgentNortCarolina.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X46", "//");
          break;
        case "CONST.X47":
          this.textFloodHazard.Enabled = this.calFloodHazard.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X48", "//");
          break;
        case "CONST.X49":
          this.textListOfConstructionAgreements.Enabled = this.calListOfConstructionAgreements.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X50", "//");
          break;
        case "CONST.X51":
          this.textOther.Enabled = this.calOther.Enabled = val == "Y";
          if (!(val != "Y"))
            break;
          base.UpdateFieldValue("CONST.X52", "//");
          break;
        default:
          if (id == "VEND.X1009" && val == "Y")
          {
            for (int index = 1010; index < 1024; ++index)
              base.UpdateFieldValue("VEND.X" + index.ToString(), "");
            break;
          }
          if (!(id == "19"))
            break;
          this.rearrangeLayout();
          break;
      }
    }

    public override void ExecAction(string action)
    {
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      this.rearrangeLayout();
    }

    private void rearrangeLayout()
    {
      if (this.inputData.GetField("19") == "ConstructionOnly" && this.inputData.GetField("4084") != "Y")
      {
        this.takeOutGroupBox.Visible = true;
        this.pnlForm.Size = new Size(this.pnlForm.Size.Width, 1694);
        this.labelFooter.Position = new Point(0, 1700);
      }
      else
      {
        this.takeOutGroupBox.Visible = false;
        this.pnlForm.Size = new Size(this.pnlForm.Size.Width, 1495);
        this.labelFooter.Position = new Point(0, 1501);
      }
    }
  }
}

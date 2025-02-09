// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LOANESTIMATEPAGE3InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
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
  public class LOANESTIMATEPAGE3InputHandler : InputHandlerBase
  {
    private StandardButton sbLookupMortgageBroker;
    private FieldReformatOnUIHandler fieldReformatOnUIHandler;

    public LOANESTIMATEPAGE3InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public LOANESTIMATEPAGE3InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public LOANESTIMATEPAGE3InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public LOANESTIMATEPAGE3InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public LOANESTIMATEPAGE3InputHandler(
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
        this.sbLookupMortgageBroker = (StandardButton) this.currentForm.FindControl("sbLookupMortgageBroker");
        if (this.loan != null && this.loan.Calculator != null && !this.loan.Calculator.CalcOnDemand(CalcOnDemandEnum.PaymentSchedule))
          this.loan.Calculator.CalculateProjectedPaymentTable();
        this.SetLookupMortgageBrokerStatus();
      }
      catch (Exception ex)
      {
      }
    }

    protected override string GetFieldValue(string id, FieldSource fieldSource)
    {
      string str = base.GetFieldValue(id, fieldSource);
      if (id == "799" || id == "LE3.X16" || id == "674")
        str = Utils.FormatLEAndCDPercentageValue(str);
      return this.fieldReformatOnUIHandler.GetFieldValue(id, str);
    }

    private void SetLookupMortgageBrokerStatus()
    {
      if (this.loan.GetField("2626") == "Banked - Retail")
        this.sbLookupMortgageBroker.Enabled = false;
      else
        this.sbLookupMortgageBroker.Enabled = true;
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      if (!(action == "lookuptpoloanofficer") || !this.loFlag)
        return;
      this.updateBrokerFieldValues();
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (id == "RESPA.X28" && val == "Y")
        base.UpdateFieldValue("RESPA.X6", "");
      if (!(id == "RESPA.X6") || !(val == "Y"))
        return;
      base.UpdateFieldValue("RESPA.X28", "");
    }

    private StateLicenseExtType getLicenseInfo(
      ExternalUserInfo loUser,
      ExternalOriginatorManagementData org)
    {
      string state = this.GetFieldValue("14");
      return (!((UserInfo) loUser != (UserInfo) null) ? this.session.ConfigurationManager.GetExtLicenseDetails(org.oid) : this.session.ConfigurationManager.GetExtLicenseDetails(loUser.ExternalOrgID)).StateLicenseExtTypes.Find((Predicate<StateLicenseExtType>) (item => item.StateAbbrevation == state));
    }

    private void updateBrokerFieldValues()
    {
      string state = this.GetFieldValue("14");
      if (this.branch == null)
      {
        if (this.company != null)
        {
          base.UpdateFieldValue("VEND.X293", this.company.OrganizationName);
          base.UpdateFieldValue("VEND.X527", this.company.NmlsId);
          StateLicenseExtType licenseInfo = this.getLicenseInfo(this.loUser, this.company);
          if (!this.inputData.IsLocked("LE3.X6"))
            base.UpdateFieldValue("LE3.X6", licenseInfo == null ? "" : licenseInfo.LicenseNo);
          base.UpdateFieldValue("LE3.X22", licenseInfo == null ? "" : licenseInfo.StateAbbrevation);
        }
        else
        {
          base.UpdateFieldValue("VEND.X293", "");
          base.UpdateFieldValue("VEND.X527", "");
          if (!this.inputData.IsLocked("LE3.X6"))
            base.UpdateFieldValue("LE3.X6", "");
        }
      }
      else
      {
        base.UpdateFieldValue("VEND.X293", this.branch.OrganizationName);
        base.UpdateFieldValue("VEND.X527", this.branch.NmlsId);
        StateLicenseExtType licenseInfo = this.getLicenseInfo(this.loUser, this.branch);
        if (!this.inputData.IsLocked("LE3.X6"))
          base.UpdateFieldValue("LE3.X6", licenseInfo == null ? "" : licenseInfo.LicenseNo);
        base.UpdateFieldValue("LE3.X22", licenseInfo == null ? "" : licenseInfo.StateAbbrevation);
      }
      if ((UserInfo) this.loUser != (UserInfo) null)
      {
        if (!this.inputData.IsLocked("LE3.X7"))
          base.UpdateFieldValue("LE3.X7", this.loUser.Email);
        if (!this.inputData.IsLocked("LE3.X8"))
          base.UpdateFieldValue("LE3.X8", this.loUser.Phone);
        if (!this.inputData.IsLocked("LE3.X9"))
          base.UpdateFieldValue("LE3.X9", this.loUser.FirstName + " " + this.loUser.MiddleName + " " + this.loUser.LastName);
        if (!this.inputData.IsLocked("LE3.X10"))
          base.UpdateFieldValue("LE3.X10", this.loUser.NmlsID);
        if (this.inputData.IsLocked("LE3.X25"))
          return;
        string val1 = "";
        string val2 = "";
        if (this.loUser.Licenses == null || this.loUser.Licenses.Count == 0)
          this.loUser.Licenses = this.session.ConfigurationManager.GetExtLicenseDetails(this.loUser.ExternalUserID).StateLicenseExtTypes;
        if (this.loUser.Licenses != null)
        {
          if (this.loUser.Licenses.Count > 0)
          {
            try
            {
              val1 = this.loUser.Licenses.Find((Predicate<StateLicenseExtType>) (x => x.StateAbbrevation == state)).LicenseNo;
              val2 = this.loUser.Licenses.Find((Predicate<StateLicenseExtType>) (x => x.StateAbbrevation == state)).StateAbbrevation;
            }
            catch (Exception ex)
            {
            }
          }
        }
        base.UpdateFieldValue("LE3.X25", val1);
        base.UpdateFieldValue("LE3.X23", val2);
      }
      else
      {
        if (!this.inputData.IsLocked("LE3.X7"))
          base.UpdateFieldValue("LE3.X7", "");
        if (!this.inputData.IsLocked("LE3.X8"))
          base.UpdateFieldValue("LE3.X8", "");
        if (!this.inputData.IsLocked("LE3.X9"))
          base.UpdateFieldValue("LE3.X9", "");
        if (!this.inputData.IsLocked("LE3.X10"))
          base.UpdateFieldValue("LE3.X10", "");
        if (!this.inputData.IsLocked("LE3.X25"))
          base.UpdateFieldValue("LE3.X25", "");
        base.UpdateFieldValue("LE3.X23", "");
      }
    }
  }
}

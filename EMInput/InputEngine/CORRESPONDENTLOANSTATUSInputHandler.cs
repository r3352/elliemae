// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CORRESPONDENTLOANSTATUSInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using mshtml;
using System;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class CORRESPONDENTLOANSTATUSInputHandler : InputHandlerBase
  {
    private const string className = "CORRESPONDENTLOANSTATUSInputHandler";

    public CORRESPONDENTLOANSTATUSInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public CORRESPONDENTLOANSTATUSInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public CORRESPONDENTLOANSTATUSInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public CORRESPONDENTLOANSTATUSInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id == "4120" && this.loan != null && this.session.LoanDataMgr != null && (string.IsNullOrEmpty(this.inputData.GetField("4120")) || this.inputData.GetField("4120") == "//") && !string.IsNullOrEmpty(val) && LockUtils.IsLockCancellable(this.session.LoanDataMgr))
      {
        if (Utils.Dialog((IWin32Window) this.session.MainForm, "The loan being Withdrawn has an active lock that may be cancelled. Are you sure you want to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
        {
          if ((bool) this.session.StartupInfo.PolicySettings[(object) "Policies.EnableLockCancellation"])
            this.session.LoanDataMgr.CreateRateLockCancellationRequest(this.session.UserInfo, DateTime.Now, "Loan has been Withdrawn by " + this.session.UserInfo.userName);
        }
        else
          val = string.Empty;
      }
      if (val != string.Empty && this.loan != null && this.loan.Calculator != null && this.loan.Calculator.IsCorrespondentLateFeeDateField(id))
      {
        DateTime dateTime = DateTime.MinValue;
        try
        {
          this.inputData.SetField(id, val);
          dateTime = this.loan.Calculator.GetCorrespondentLateFeeLatestBeginDate(true);
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The date you just entered will cause \"Late Days Begin\" invalid!" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          val = string.Empty;
        }
        if (val != string.Empty)
        {
          try
          {
            Utils.ConvertToLoanInternalValue(dateTime.Date.ToString("MM/dd/yyyy"), FieldFormat.DATE);
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The date you just entered will cause \"Late Days Begin\" invalid!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            val = string.Empty;
          }
        }
      }
      if (id == "3925")
      {
        try
        {
          if (!string.IsNullOrEmpty(this.inputData.GetField("L770")))
          {
            if (this.inputData.GetField("L770") != "//")
            {
              if (Utils.Dialog((IWin32Window) this.session.MainForm, "The Note Date may already contain the Document Date [L770] that was used to generate Documents. Do you wish to continue with this field edit and override?", MessageBoxButtons.OKCancel, MessageBoxIcon.None) == DialogResult.Cancel)
                return;
            }
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(InputHandlerBase.sw, nameof (CORRESPONDENTLOANSTATUSInputHandler), TraceLevel.Error, ex.ToString());
          int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Error retrieving Field ID: L770", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      if ((id == "4207" || id == "4208") && LockUtils.GetRequestLockStatus(this.loan) == "Active Lock" && !string.IsNullOrEmpty(val) && val != "//")
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "Loan lock must not be 'Active' prior to entering this date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        val = string.Empty;
      }
      if ((id == "4120" || id == "4207" || id == "4208") && !string.IsNullOrWhiteSpace(val) && val != "//")
      {
        int num = 0;
        string str1 = id == "4120" ? val : this.inputData.GetField("4120");
        bool flag1 = !string.IsNullOrWhiteSpace(str1) && str1 != "//";
        if (flag1)
          ++num;
        string str2 = id == "4207" ? val : this.inputData.GetField("4207");
        bool flag2 = !string.IsNullOrWhiteSpace(str2) && str2 != "//";
        if (flag2)
          ++num;
        string str3 = id == "4208" ? val : this.inputData.GetField("4208");
        bool flag3 = !string.IsNullOrWhiteSpace(str3) && str3 != "//";
        if (flag3)
          ++num;
        string str4 = (string) null;
        string str5 = (string) null;
        string id1 = (string) null;
        if (num > 1)
        {
          switch (id)
          {
            case "4120":
              str4 = "Withdrawn Date";
              if (flag2)
              {
                str5 = "Cancelled Date";
                id1 = "4207";
                break;
              }
              if (flag3)
              {
                str5 = "Voided Date";
                id1 = "4208";
                break;
              }
              break;
            case "4207":
              str4 = "Cancelled Date";
              if (flag1)
              {
                str5 = "Withdrawn Date";
                id1 = "4120";
                break;
              }
              if (flag3)
              {
                str5 = "Voided Date";
                id1 = "4208";
                break;
              }
              break;
            case "4208":
              str4 = "Voided Date";
              if (flag1)
              {
                str5 = "Withdrawn Date";
                id1 = "4120";
                break;
              }
              if (flag2)
              {
                str5 = "Cancelled Date";
                id1 = "4207";
                break;
              }
              break;
          }
          if (str4 != null && str5 != null && id1 != null)
          {
            if (Utils.Dialog((IWin32Window) this.session.MainForm, string.Format("The {1} is already populated.\r\n\r\nIf you click OK, the {1} will be cleared and the {0} will be retained.\r\n\r\nIf you click Cancel, the {0} will be cleared and the {1} retained.", (object) str4, (object) str5), MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
              this.inputData.SetField(id1, (string) null);
            else
              val = string.Empty;
          }
        }
      }
      base.UpdateFieldValue(id, val);
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      ExternalLateFeeSettings orgLateFeeSettings = this.session.ConfigurationManager.GetExternalOrgLateFeeSettings(this.inputData.GetField("TPO.X15"), true);
      if (!string.IsNullOrEmpty(this.inputData.GetField("4111")) && this.inputData.GetField("4111") != "Purchase Suspense Date" && this.inputData.GetField("4111") != "Latest Cond's Issue Date" && this.inputData.GetField("4111") != "Latest Cond's Issued Date" && this.inputData.GetField("4111") != "Latest Stips Issued Date" && this.inputData.GetField("4111") != "Purchase Approval Date" && this.inputData.GetField("4111") != "Delivery Expiration Date")
      {
        LoanReportFieldDef fieldByCriterionName = LoanReportFieldDefs.GetFieldDefs(this.session, LoanReportFieldFlags.AllDatabaseFields).GetFieldByCriterionName(orgLateFeeSettings.OtherDate);
        if (fieldByCriterionName != null && !string.IsNullOrEmpty(fieldByCriterionName.Description))
          this.inputData.SetField("4111", fieldByCriterionName.Description);
      }
      if (LockUtils.GetRequestLockStatus(this.loan) == "Cancelled Lock" || !string.Equals(this.inputData.GetField("2626"), "correspondent", StringComparison.InvariantCultureIgnoreCase))
        this.inputData.SetField("3924", "");
      else if (this.inputData.GetField("3924") == "")
        this.inputData.SetField("3924", this.inputData.GetField("3420"));
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
    }
  }
}

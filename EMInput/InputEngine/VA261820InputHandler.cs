// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VA261820InputHandler
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
  internal class VA261820InputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Panel pnllocalAdr;
    private EllieMae.Encompass.Forms.Panel pnlforeignAdr;
    private StandardButton selectCountryButton;
    private EllieMae.Encompass.Forms.Panel pnlforeignPnl;
    private EllieMae.Encompass.Forms.Panel oldLoanPurposePanel;
    private EllieMae.Encompass.Forms.Panel newLoanPurposePanel;

    public VA261820InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public VA261820InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public VA261820InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public VA261820InputHandler(
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
        this.pnllocalAdr = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("localAdr");
        this.pnlforeignAdr = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignAdr");
        this.selectCountryButton = (StandardButton) this.currentForm.FindControl("selectcountry_CAPIAP_X64");
        this.pnlforeignPnl = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignPnl");
        if (this.pnlforeignAdr != null && this.pnllocalAdr != null)
          this.pnlforeignAdr.Position = this.pnllocalAdr.Position;
        this.oldLoanPurposePanel = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("oldLoanPurposePanel");
        this.newLoanPurposePanel = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("newLoanPurposePanel");
        if (this.oldLoanPurposePanel == null || this.newLoanPurposePanel == null)
          return;
        this.newLoanPurposePanel.Position = this.oldLoanPurposePanel.Position;
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
          if (this.loan.GetField("1066") != "Leasehold")
          {
            controlState = ControlState.Disabled;
            this.SetControlState("Calendar7", false);
            goto case "11";
          }
          else
          {
            this.SetControlState("Calendar7", true);
            goto case "11";
          }
        case "1064":
          if (this.GetFieldValue("1497") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "11":
          return controlState;
        case "364":
          if (this.FormIsForTemplate || InputHandlerBase.LockLoanNumber)
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "CAPIAP.X64":
          if (this.loan.GetField("CAPIAP.X62") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "VASUMM.X56":
          if (this.GetFieldValue("VASUMM.X55") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "VASUMM.X58":
          if (this.GetFieldValue("VASUMM.X57") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        case "VASUMM.X66":
          if (this.GetFieldValue("VASUMM.X65") != "Escrow" && this.GetFieldValue("VASUMM.X65") != "Earmarked Account")
          {
            controlState = ControlState.Disabled;
            goto case "11";
          }
          else
            goto case "11";
        default:
          controlState = ControlState.Default;
          goto case "11";
      }
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.selectCountryButton != null)
      {
        bool flag = this.GetField("CAPIAP.X62") == "Y";
        this.selectCountryButton.Visible = flag;
        this.pnllocalAdr.Visible = !flag;
        this.pnlforeignAdr.Visible = flag;
      }
      if (this.GetField("1825") == "2020")
        this.pnlforeignPnl.Visible = true;
      else
        this.pnlforeignPnl.Visible = false;
      DateTime date1 = Utils.ParseDate((object) this.inputData.GetField("745"));
      DateTime date2 = Utils.ParseDate((object) this.inputData.GetField("3142"));
      if (date2 != DateTime.MinValue && DateTime.Compare(date2, DateTime.Parse("04/01/2023")) < 0 || date1 != DateTime.MinValue && DateTime.Compare(date1, DateTime.Parse("04/01/2023")) < 0)
      {
        this.oldLoanPurposePanel.Visible = true;
        this.newLoanPurposePanel.Visible = false;
      }
      else
      {
        this.oldLoanPurposePanel.Visible = false;
        this.newLoanPurposePanel.Visible = true;
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (!(id == "VASUMM.X31"))
        return;
      this.loan.Calculator.UpdateAccountName("VA");
    }
  }
}

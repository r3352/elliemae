// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.BUYDOWNSUMMARYInputHandler
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
  public class BUYDOWNSUMMARYInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Panel panelBorBuydown;
    private EllieMae.Encompass.Forms.Panel panelNonBorBuydown;
    private EllieMae.Encompass.Forms.Panel panelSec32SellerPaid;
    private EllieMae.Encompass.Forms.TextBox l_QMX378;
    private FieldLock fl_QMX378;

    public BUYDOWNSUMMARYInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public BUYDOWNSUMMARYInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public BUYDOWNSUMMARYInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public BUYDOWNSUMMARYInputHandler(
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
        this.panelBorBuydown = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelBorBuydown");
        this.panelNonBorBuydown = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelNonBorBuydown");
        this.panelBorBuydown.Top = this.panelNonBorBuydown.Top;
        this.panelBorBuydown.Left = this.panelNonBorBuydown.Left;
        this.panelBorBuydown.Visible = false;
        this.panelSec32SellerPaid = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelSec32SellerPaid");
        this.fl_QMX378 = (FieldLock) this.currentForm.FindControl("fl_QMX378");
        this.l_QMX378 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_QMX378");
      }
      catch (Exception ex)
      {
      }
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      this.setBuydownSellerPaidFeeLayout(true);
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      bool flag1 = this.inputData.GetField("COMPLIANCEVERSION.CASASRNX141") == "Y" || this.inputData.GetField("COMPLIANCEVERSION.NEWBUYDOWNENABLED") != "Y";
      bool flag2 = this.inputData.GetField("CASASRN.X141") == "Borrower" | flag1;
      if (this.panelBorBuydown != null && this.panelNonBorBuydown != null)
      {
        this.panelBorBuydown.Visible = flag2;
        this.panelNonBorBuydown.Visible = !flag2;
      }
      this.setBuydownSellerPaidFeeLayout(false);
    }

    private void setBuydownSellerPaidFeeLayout(bool disabledOnly)
    {
      ATR_QUALIFICATIONInputHandler.SetBuydownSellerPaidField(this.inputData, this.panelSec32SellerPaid, this.l_QMX378, this.fl_QMX378, disabledOnly);
    }
  }
}

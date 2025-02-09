// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FEEVARIANCEWORKSHEETInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class FEEVARIANCEWORKSHEETInputHandler : InputHandlerBase
  {
    private MultilineTextBox toleranceCureTxtComments;
    private EllieMae.Encompass.Forms.TextBox toleranceCureReqCureAmt;
    private FieldLock toleranceCureReqCureAmtLock;
    private EllieMae.Encompass.Forms.TextBox toleranceCureAppCureAmt;
    private EllieMae.Encompass.Forms.TextBox toleranceCureDate;
    private Calendar toleranceCureCalendar;
    private EllieMae.Encompass.Forms.TextBox toleranceCureResolvedBy;

    public FEEVARIANCEWORKSHEETInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public FEEVARIANCEWORKSHEETInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public FEEVARIANCEWORKSHEETInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public FEEVARIANCEWORKSHEETInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public FEEVARIANCEWORKSHEETInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput htmlInput,
      HTMLDocument htmlDoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, htmlInput, htmlDoc, form, property)
    {
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      if (this.loan.IsTemplate)
        return;
      bool flag1 = this.loan.Calculator.IsLEBaseLineUsed("cannotdecrease");
      EllieMae.Encompass.Forms.TextBox control1 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("TextBox3");
      StandardButton control2 = (StandardButton) this.currentForm.FindControl("StandardButton2");
      EllieMae.Encompass.Forms.Label control3 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblItemsCannotDecrease");
      if (flag1)
      {
        control3.Visible = true;
        control1.Visible = false;
        control2.Visible = false;
        control3.Position = new Point(600, 10);
      }
      else
      {
        control3.Visible = false;
        control1.Visible = true;
        control2.Visible = true;
      }
      bool flag2 = this.loan.Calculator.IsLEBaseLineUsed("cannotincrease");
      EllieMae.Encompass.Forms.TextBox control4 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("TextBox29");
      StandardButton control5 = (StandardButton) this.currentForm.FindControl("StandardButton4");
      EllieMae.Encompass.Forms.Label control6 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblItemsCannotIncrease");
      if (flag2)
      {
        control4.Visible = false;
        control5.Visible = false;
        control6.Visible = true;
        control6.Position = new Point(600, 10);
      }
      else
      {
        control4.Visible = true;
        control5.Visible = true;
        control6.Visible = false;
      }
      bool flag3 = this.loan.Calculator.IsLEBaseLineUsed("cannotincrease10");
      EllieMae.Encompass.Forms.TextBox control7 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("TextBox55");
      StandardButton control8 = (StandardButton) this.currentForm.FindControl("StandardButton6");
      EllieMae.Encompass.Forms.Label control9 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("lblItemsIncrease10");
      if (flag3)
      {
        control7.Visible = false;
        control8.Visible = false;
        control9.Visible = true;
        control9.Position = new Point(600, 10);
      }
      else
      {
        control7.Visible = true;
        control8.Visible = true;
        control9.Visible = false;
      }
    }

    internal override void CreateControls()
    {
      foreach (EllieMae.Encompass.Forms.Control allControl in this.currentForm.GetAllControls())
      {
        if (allControl is FieldControl && (!allControl.ControlID.ToLower().Contains("edit") || allControl.ControlID.ToLower() == "edit1"))
          ((RuntimeControl) allControl).Enabled = false;
      }
      this.toleranceCureTxtComments = (MultilineTextBox) this.currentForm.FindControl("edit3");
      this.toleranceCureReqCureAmtLock = (FieldLock) this.currentForm.FindControl("ToleranceCureLock");
      this.toleranceCureReqCureAmt = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("edit4");
      this.toleranceCureAppCureAmt = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("edit1");
      this.toleranceCureCalendar = (Calendar) this.currentForm.FindControl("Calendar1");
      this.toleranceCureDate = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("edit2");
      this.toleranceCureResolvedBy = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("textBox343");
      if (((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ToolsTab_CureToleranceAlert))
        return;
      this.toleranceCureTxtComments.Enabled = false;
      this.toleranceCureReqCureAmtLock.Enabled = false;
      this.toleranceCureReqCureAmt.Enabled = false;
      this.toleranceCureAppCureAmt.Enabled = false;
      this.toleranceCureCalendar.Enabled = false;
      this.toleranceCureDate.Enabled = false;
      this.toleranceCureResolvedBy.Enabled = false;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (id == "3171" || id == "3172")
      {
        if ((this.GetFieldValue("3171") == string.Empty || this.GetFieldValue("3171") == "//") && this.GetFieldValue("3172") == string.Empty)
          base.UpdateFieldValue("3173", "");
        else
          base.UpdateFieldValue("3173", this.session.UserInfo.FullName + " (" + this.session.UserID + ")");
      }
      if (!(id == "FV.X379") && !(id == "FV.X380") && !(id == "FV.X383") && !(id == "FV.X384") && !(id == "FV.X366"))
        return;
      if (Utils.ParseDecimal((object) val, 0M) < 0M)
        this.SetField(id, "0");
      else
        this.SetField(id, val);
      this.loan.Calculator.CalculateFeeVarianceTotals(id);
    }
  }
}

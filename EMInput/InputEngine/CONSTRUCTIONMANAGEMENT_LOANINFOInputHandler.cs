// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CONSTRUCTIONMANAGEMENT_LOANINFOInputHandler
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
  public class CONSTRUCTIONMANAGEMENT_LOANINFOInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Panel pnlForm;
    private EllieMae.Encompass.Forms.Label labelFooter;
    private CategoryBox categoryBoxConstructionOnly;
    private CategoryBox categoryBoxLoanInfo;
    private CategoryBox categoryBoxConstructionPerm1xClose;
    private DropdownEditBox cboARMFloor;
    private EllieMae.Encompass.Forms.Panel pnlPartialPrepaymentsElection;
    private Calendar calConstIndexDate;
    private Calendar calConstPermIndexDate;

    public CONSTRUCTIONMANAGEMENT_LOANINFOInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public CONSTRUCTIONMANAGEMENT_LOANINFOInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public CONSTRUCTIONMANAGEMENT_LOANINFOInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public CONSTRUCTIONMANAGEMENT_LOANINFOInputHandler(
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
        this.pnlForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        this.categoryBoxLoanInfo = (CategoryBox) this.currentForm.FindControl("CategoryBoxLoanInfo");
        this.categoryBoxConstructionOnly = (CategoryBox) this.currentForm.FindControl("CategoryBoxConstructionOnly");
        this.categoryBoxConstructionPerm1xClose = (CategoryBox) this.currentForm.FindControl("CategoryBoxConstructionPerm1xClose");
        this.labelFooter = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelFooter");
        this.cboARMFloor = (DropdownEditBox) this.currentForm.FindControl("cboARMFloor");
        this.pnlPartialPrepaymentsElection = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlPartialPrepaymentsElection");
        this.categoryBoxConstructionPerm1xClose.Left = this.categoryBoxConstructionOnly.Left;
        this.categoryBoxConstructionPerm1xClose.Top = this.categoryBoxConstructionOnly.Top;
        this.calConstIndexDate = (Calendar) this.currentForm.FindControl("Calendar6");
        this.calConstPermIndexDate = (Calendar) this.currentForm.FindControl("Calendar7");
      }
      catch (Exception ex)
      {
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (id == "19" || id == "4084")
        this.rearrangeLayout();
      if (this.loan == null || (this.loan.Calculator.CalcOnDemandMethod & CalcOnDemandEnum.PaymentSchedule) != CalcOnDemandEnum.PaymentSchedule)
        return;
      this.loan.Calculator.CalcOnDemand(CalcOnDemandEnum.PaymentSchedule);
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      this.rearrangeLayout();
    }

    private void rearrangeLayout()
    {
      Size size1;
      if (this.inputData.GetField("19") == "ConstructionOnly")
      {
        this.cboARMFloor.Visible = true;
        this.categoryBoxConstructionOnly.Visible = true;
        this.categoryBoxConstructionPerm1xClose.Visible = false;
        this.pnlPartialPrepaymentsElection.Visible = this.inputData.GetField("4084") == "Y";
        DropdownEditBox cboArmFloor = this.cboARMFloor;
        Point position = this.categoryBoxConstructionOnly.Position;
        int x = position.X + 355;
        position = this.categoryBoxConstructionOnly.Position;
        int y = position.Y + 235;
        Point point = new Point(x, y);
        cboArmFloor.Position = point;
        EllieMae.Encompass.Forms.Panel pnlForm = this.pnlForm;
        size1 = this.pnlForm.Size;
        int width = size1.Width;
        int top = this.categoryBoxConstructionOnly.Top;
        size1 = this.categoryBoxConstructionOnly.Size;
        int height1 = size1.Height;
        int height2 = top + height1;
        Size size2 = new Size(width, height2);
        pnlForm.Size = size2;
      }
      else if (this.inputData.GetField("19") == "ConstructionToPermanent" && this.inputData.GetField("4084") != "Y")
      {
        this.cboARMFloor.Visible = true;
        this.categoryBoxConstructionOnly.Visible = false;
        this.categoryBoxConstructionPerm1xClose.Visible = true;
        this.pnlPartialPrepaymentsElection.Visible = true;
        this.cboARMFloor.Position = new Point(this.categoryBoxConstructionPerm1xClose.Position.X + 356, this.categoryBoxConstructionPerm1xClose.Position.Y + 340);
        this.pnlForm.Size = new Size(this.pnlForm.Size.Width, this.categoryBoxConstructionPerm1xClose.Top + this.categoryBoxConstructionPerm1xClose.Size.Height);
      }
      else
      {
        this.categoryBoxConstructionOnly.Visible = false;
        this.categoryBoxConstructionPerm1xClose.Visible = false;
        this.pnlPartialPrepaymentsElection.Visible = false;
        this.cboARMFloor.Visible = false;
        this.pnlForm.Size = new Size(this.pnlForm.Size.Width, this.categoryBoxLoanInfo.Top + this.categoryBoxLoanInfo.Size.Height + 10);
      }
      EllieMae.Encompass.Forms.Label labelFooter = this.labelFooter;
      size1 = this.pnlForm.Size;
      int num = size1.Height + 5;
      labelFooter.Top = num;
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1964":
        case "Constr.Refi":
          if (this.inputData.GetField("19") != "ConstructionOnly" && this.inputData.GetField("19") != "ConstructionToPermanent")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "3942":
          switch (this.GetField("19"))
          {
            case "Purchase":
              controlState = ControlState.Disabled;
              goto label_17;
            case "ConstructionOnly":
            case "ConstructionToPermanent":
              if (!(this.GetField("CONST.X2") != "Y"))
                break;
              goto case "Purchase";
          }
          controlState = ControlState.Enabled;
          break;
        case "4084":
          if (this.inputData.GetField("19") != "ConstructionOnly")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "4898":
          if (this.loan.IsTemplate)
          {
            controlState = ControlState.Disabled;
            this.calConstIndexDate.Enabled = false;
            this.calConstPermIndexDate.Enabled = false;
            break;
          }
          break;
        case "5015":
          controlState = !(this.inputData.GetField("Constr.Refi") == "Y") || !(this.inputData.GetField("19") == "ConstructionOnly") && !(this.inputData.GetField("19") == "ConstructionToPermanent") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "995":
        case "zoomarm":
          if (this.loan.GetField("608") != "AdjustableRate" && this.inputData.GetField("1172") != "HELOC")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "getindex":
        case "subfin":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        default:
          controlState = ControlState.Default;
          break;
      }
label_17:
      return controlState;
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "subfin":
          base.ExecAction(action);
          this.SetFieldFocus("CheckBox4");
          break;
        case "loanprog":
          base.ExecAction(action);
          this.SetFieldFocus("TextBox2");
          break;
        case "zoomarm":
          base.ExecAction(action);
          this.SetFieldFocus("");
          break;
        case "getindex":
          base.ExecAction(action);
          this.SetFieldFocus("");
          break;
      }
    }
  }
}

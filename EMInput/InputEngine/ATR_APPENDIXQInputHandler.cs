// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ATR_APPENDIXQInputHandler
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
  public class ATR_APPENDIXQInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Panel rootPanel;
    private CategoryBox debtAnalysisCategoryBox;
    private EllieMae.Encompass.Forms.Panel voolParentPanel;
    private EllieMae.Encompass.Forms.Panel contingentLiabPanel;
    private EllieMae.Encompass.Forms.Label screenLabel;
    private EllieMae.Encompass.Forms.Panel voolOldPanel;
    private EllieMae.Encompass.Forms.GroupBox voolNewGroupBox;
    private int rootPanel_origH;
    private int debtAnalysisCategoryBox_origH;
    private int voolParentPanel_origH;
    private int contingentLiabPanel_origY;
    private int screenLabel_origY;
    private EllieMae.Encompass.Forms.Label label68;

    public ATR_APPENDIXQInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public ATR_APPENDIXQInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public ATR_APPENDIXQInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public ATR_APPENDIXQInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
      {
        case 719179447:
          if (!(action == "calculateovertimeincome"))
            break;
          this.SetFieldFocus("l_X138");
          break;
        case 1278929227:
          if (!(action == "baseincome"))
            break;
          this.SetFieldFocus("l_X137");
          break;
        case 1289470777:
          if (!(action == "calculatemilitaryallowenceincome"))
            break;
          this.SetFieldFocus("l_X180");
          break;
        case 1421579633:
          if (!(action == "vool"))
            break;
          this.SetFieldFocus("l_URLAROL0101");
          break;
        case 1928564127:
          if (!(action == "calculatemilitaryincome"))
            break;
          this.SetFieldFocus("l_X163");
          break;
        case 2552332442:
          if (!(action == "vol"))
            break;
          this.SetFieldFocus("l_FL0102");
          break;
        case 2907831719:
          if (!(action == "calculatedividendincome"))
            break;
          this.SetFieldFocus("l_X141");
          break;
        case 3000050112:
          if (!(action == "calculateotherincome"))
            break;
          this.SetFieldFocus("l_X142");
          break;
      }
    }

    internal override void CreateControls()
    {
      try
      {
        this.label68 = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label68");
        this.rootPanel = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        this.rootPanel_origH = this.rootPanel.Size.Height;
        this.debtAnalysisCategoryBox = (CategoryBox) this.currentForm.FindControl("CategoryBoxDebtAnalysis");
        this.debtAnalysisCategoryBox_origH = this.debtAnalysisCategoryBox.Size.Height;
        this.voolParentPanel = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelVOOL");
        this.voolParentPanel_origH = this.voolParentPanel.Size.Height;
        this.contingentLiabPanel = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelContingentLiab");
        this.contingentLiabPanel_origY = this.contingentLiabPanel.Position.Y;
        this.screenLabel = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label2");
        this.screenLabel_origY = this.screenLabel.Position.Y;
        this.voolOldPanel = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panelOldVOOL");
        this.voolNewGroupBox = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("groupBox2020VOOL");
      }
      catch (Exception ex)
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "QM.X181":
          this.label68.Text = !this.loan.Use2015RESPA ? "2) the value, as established by an appraisal or sales price on the HUD-1 Settlement Statement from the sales of the property, results in a 75% or less LTV." : "2) the value, as established by an appraisal or sales price on the Closing Disclosure from the sales of the property, results in a 75% or less LTV.";
          controlState = ControlState.Default;
          break;
        case "URLAROL0104":
          if (this.loan.GetField("URLAROL0102") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLAROL0204":
          if (this.loan.GetField("URLAROL0202") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "URLAROL0304":
          if (this.loan.GetField("URLAROL0302") != "Other")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        default:
          controlState = ControlState.Default;
          break;
      }
      return controlState;
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      int num = 30;
      if (this.inputData.GetField("1825") == "2020")
      {
        this.rootPanel.Size = new Size(this.rootPanel.Size.Width, this.rootPanel_origH + num);
        this.debtAnalysisCategoryBox.Size = new Size(this.debtAnalysisCategoryBox.Size.Width, this.debtAnalysisCategoryBox_origH + num);
        this.voolParentPanel.Size = new Size(this.voolParentPanel.Size.Width, this.voolParentPanel_origH + num);
        this.contingentLiabPanel.Position = new Point(this.contingentLiabPanel.Position.X, this.contingentLiabPanel_origY + num);
        this.screenLabel.Position = new Point(this.screenLabel.Position.X, this.screenLabel_origY + num);
        this.voolNewGroupBox.Visible = true;
        this.voolNewGroupBox.Position = new Point(0, 0);
        this.voolNewGroupBox.Size = new Size(572, 100 + num);
        this.voolOldPanel.Visible = false;
      }
      else
      {
        this.rootPanel.Size = new Size(this.rootPanel.Size.Width, this.rootPanel_origH);
        this.debtAnalysisCategoryBox.Size = new Size(this.debtAnalysisCategoryBox.Size.Width, this.debtAnalysisCategoryBox_origH);
        this.voolParentPanel.Size = new Size(this.voolParentPanel.Size.Width, this.voolParentPanel_origH);
        this.contingentLiabPanel.Position = new Point(this.contingentLiabPanel.Position.X, this.contingentLiabPanel_origY);
        this.screenLabel.Position = new Point(this.screenLabel.Position.X, this.screenLabel_origY);
        this.voolNewGroupBox.Visible = false;
        this.voolOldPanel.Visible = true;
      }
    }
  }
}

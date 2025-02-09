// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LOCOMPENSATIONInputHandler
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
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class LOCOMPENSATIONInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.GroupBox boxBK;
    private EllieMae.Encompass.Forms.GroupBox boxLO;
    private EllieMae.Encompass.Forms.Label labelFormName;
    private EllieMae.Encompass.Forms.Panel pnlForm;
    private bool bkViewable = true;
    private bool loViewable = true;

    public LOCOMPENSATIONInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public LOCOMPENSATIONInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public LOCOMPENSATIONInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public LOCOMPENSATIONInputHandler(
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
        if (this.boxBK == null)
          this.boxBK = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("boxBK");
        if (this.boxLO == null)
          this.boxLO = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("boxLO");
        if (this.labelFormName == null)
          this.labelFormName = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("labelFormName");
        if (this.pnlForm == null)
          this.pnlForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
        this.bkViewable = aclManager.GetUserApplicationRight(AclFeature.ToolsTab_LOCompBrokerTool);
        this.loViewable = aclManager.GetUserApplicationRight(AclFeature.ToolsTab_LOCompOfficerTool);
        this.refreshForm();
      }
      catch (Exception ex)
      {
      }
    }

    private void refreshForm()
    {
      int num1 = 5;
      this.boxBK.Visible = this.bkViewable;
      this.boxLO.Visible = this.loViewable;
      Size size1;
      if (!this.bkViewable && this.loViewable)
      {
        this.boxLO.Top = this.boxBK.Top;
        this.pnlForm.Size = new Size(this.pnlForm.Size.Width, this.boxLO.Top + this.boxLO.Size.Height + num1);
      }
      else if (this.bkViewable && !this.loViewable)
      {
        EllieMae.Encompass.Forms.Panel pnlForm = this.pnlForm;
        size1 = this.pnlForm.Size;
        int width = size1.Width;
        int top = this.boxBK.Top;
        size1 = this.boxBK.Size;
        int height1 = size1.Height;
        int height2 = top + height1;
        Size size2 = new Size(width, height2);
        pnlForm.Size = size2;
      }
      else
      {
        this.boxLO.Top = this.boxBK.Top + this.boxBK.Size.Height + 5;
        EllieMae.Encompass.Forms.Panel pnlForm = this.pnlForm;
        int width = this.pnlForm.Size.Width;
        int top = this.boxLO.Top;
        size1 = this.boxLO.Size;
        int height3 = size1.Height;
        int height4 = top + height3 + num1;
        Size size3 = new Size(width, height4);
        pnlForm.Size = size3;
      }
      EllieMae.Encompass.Forms.Label labelFormName = this.labelFormName;
      size1 = this.pnlForm.Size;
      int num2 = size1.Height + 5;
      labelFormName.Top = num2;
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      this.getControlState(ctrl, id, ControlState.Enabled);
      ControlState controlState;
      switch (id)
      {
        case "LCP.X2":
          controlState = ControlState.Default;
          if ((this.loan.GetField("2626") == "Banked - Wholesale" || this.loan.GetField("2626") == "Brokered") && !this.IsUsingTemplate)
          {
            this.SetControlState("btnLookUp", true);
            break;
          }
          this.SetControlState("btnLookUp", false);
          break;
        case "lookuplocomp":
          controlState = !this.IsUsingTemplate ? ControlState.Default : ControlState.Disabled;
          break;
        case "resetcomp":
          controlState = !this.IsUsingTemplate ? (!(this.inputData.GetField("LCP.X19") == string.Empty) ? (!(this.loan.GetField("2626") == "Banked - Wholesale") ? ControlState.Enabled : (this.loan.GetField("LCP.X2") != string.Empty || this.loan.GetField("LCP.X18") != string.Empty ? ControlState.Enabled : ControlState.Disabled)) : ControlState.Disabled) : ControlState.Disabled;
          break;
        default:
          controlState = ControlState.Default;
          break;
      }
      return controlState;
    }

    public override void ExecAction(string action)
    {
      if (!(action == "lookuplocomp") && !(action == "resetcomp"))
        return;
      base.ExecAction(action);
      if (this.boxBK.Visible)
      {
        if (!this.inputData.IsLocked("LCP.X6"))
          this.SetFieldFocus("l_X11");
        else
          this.SetFieldFocus("l_X6");
      }
      else
      {
        if (!this.boxLO.Visible)
          return;
        this.SetFieldFocus("l_X30");
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FREDDIEInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

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
  public class FREDDIEInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Panel panelForm;
    private EllieMae.Encompass.Forms.GroupBox groupBox5;
    private EllieMae.Encompass.Forms.GroupBox groupBox6;
    private EllieMae.Encompass.Forms.Label freddieLabel;
    private int origPanelHeight;
    private EllieMae.Encompass.Forms.GroupBox groupBox4;
    private EllieMae.Encompass.Forms.GroupBox groupBox9;
    private EllieMae.Encompass.Forms.GroupBox groupBox13;

    public FREDDIEInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public FREDDIEInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public FREDDIEInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public FREDDIEInputHandler(
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
        this.panelForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        this.groupBox5 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox5");
        this.groupBox6 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox6");
        this.freddieLabel = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("freddieLabel");
        this.origPanelHeight = this.panelForm.Size.Height;
        this.groupBox4 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox4");
        this.groupBox9 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox9");
        this.groupBox13 = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox13");
      }
      catch
      {
      }
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      if (!(this.loan.GetField("1825") == "2020"))
        return;
      this.groupBox9.Visible = false;
      this.panelForm.Size = new Size(this.panelForm.Size.Width, this.origPanelHeight - 310);
      this.groupBox5.Top = this.groupBox13.Bottom + 10;
      this.groupBox6.Top = this.groupBox5.Bottom + 10;
      this.freddieLabel.Position = new Point(this.panelForm.AbsolutePosition.X, this.panelForm.Bottom + 5);
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "4970":
        case "4971":
        case "4972":
        case "MORNET.X30":
          if (this.GetField("5027") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "5018":
        case "5019":
        case "5020":
        case "5021":
          controlState = !(this.GetField("5028") == "Y") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "getami":
          if (this.FormIsForTemplate || this.GetField("5027") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "getmfi":
          controlState = this.FormIsForTemplate || this.GetField("5028") == "Y" ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "lookup4970m":
          if (this.GetField("5027") == "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "mfilookup":
          return this.GetField("5028") == "Y" ? ControlState.Disabled : ControlState.Enabled;
        default:
          controlState = ControlState.Default;
          break;
      }
      return controlState;
    }
  }
}

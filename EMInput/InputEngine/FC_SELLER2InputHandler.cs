// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FC_SELLER2InputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class FC_SELLER2InputHandler : InputHandlerBase
  {
    private IWin32Window owner;
    private List<EllieMae.Encompass.Forms.Panel> panelLocalAddresses;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignAddresses;
    private List<StandardButton> selectCountryButtons;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignPanels;
    private EllieMae.Encompass.Forms.Panel relationLineItemPanel;

    public FC_SELLER2InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public FC_SELLER2InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public FC_SELLER2InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public FC_SELLER2InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
      this.owner = owner;
    }

    internal override void CreateControls()
    {
      try
      {
        this.panelLocalAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignPanels = new List<EllieMae.Encompass.Forms.Panel>();
        this.selectCountryButtons = new List<StandardButton>();
        this.panelLocalAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("localAdr1"));
        this.panelForeignAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignAdr1"));
        if (this.panelForeignAddresses[0] != null && this.panelLocalAddresses[0] != null)
          this.panelForeignAddresses[0].Position = this.panelLocalAddresses[0].Position;
        for (int index = 1; index <= 2; ++index)
          this.panelForeignPanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignPanel" + (object) index));
        this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_VEND_X1048"));
        this.relationLineItemPanel = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("relationLineItemPanel");
      }
      catch
      {
      }
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.selectCountryButtons != null && this.selectCountryButtons.Count > 0)
      {
        bool flag = this.GetField("VEND.X1047") == "Y";
        this.selectCountryButtons[0].Visible = flag;
        this.panelLocalAddresses[0].Visible = !flag;
        this.panelForeignAddresses[0].Visible = flag;
      }
      if (this.GetField("1825") == "2020")
      {
        for (int index = 0; index < 2; ++index)
          this.panelForeignPanels[index].Visible = true;
        if (this.relationLineItemPanel == null)
          return;
        this.relationLineItemPanel.Position = new Point(1, 180);
      }
      else
      {
        for (int index = 0; index < 2; ++index)
          this.panelForeignPanels[index].Visible = false;
        if (this.relationLineItemPanel == null)
          return;
        this.relationLineItemPanel.Position = new Point(1, 158);
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      if (id == "VEND.X1048")
      {
        if (this.loan.GetField("VEND.X1047") != "Y")
          controlState = ControlState.Disabled;
      }
      else
        controlState = ControlState.Default;
      return controlState;
    }
  }
}

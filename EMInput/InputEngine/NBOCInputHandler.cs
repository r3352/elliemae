// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.NBOCInputHandler
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
  internal class NBOCInputHandler : VOLInputHandler
  {
    private List<EllieMae.Encompass.Forms.Panel> panelLocalAddresses;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignAddresses;
    private List<StandardButton> selectCountryButtons;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignPanels;
    private EllieMae.Encompass.Forms.Label vestingTypeLabel;
    private DropdownBox vestingTypeBox;
    private ImageButton bestTimeButton;

    public NBOCInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public NBOCInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public NBOCInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public NBOCInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, owner, dataTemplate, htmldoc, form, property)
    {
    }

    protected override void InitHeader() => this.header = "NBOC";

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
        this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_NBOC0023"));
        this.vestingTypeLabel = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label5");
        this.vestingTypeBox = (DropdownBox) this.currentForm.FindControl("DropdownBox1");
        this.bestTimeButton = (ImageButton) this.currentForm.FindControl("ImageBtnBestContact");
        if (this.bestTimeButton == null)
          return;
        this.bestTimeButton.Action = "bestcontactdaytime_nboc#" + this.ind;
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
        bool flag = this.GetField("NBOC" + this.ind + "22") == "Y";
        this.selectCountryButtons[0].Visible = flag;
        this.panelLocalAddresses[0].Visible = !flag;
        this.panelForeignAddresses[0].Visible = flag;
      }
      if (this.GetField("1825") == "2020")
      {
        for (int index = 0; index < 2; ++index)
          this.panelForeignPanels[index].Visible = true;
        if (this.vestingTypeLabel == null || this.vestingTypeBox == null)
          return;
        this.vestingTypeLabel.Position = new Point(7, 202);
        this.vestingTypeBox.Position = new Point(129, 202);
      }
      else
      {
        for (int index = 0; index < 2; ++index)
          this.panelForeignPanels[index].Visible = false;
        if (this.vestingTypeLabel == null || this.vestingTypeBox == null)
          return;
        this.vestingTypeLabel.Position = new Point(7, 180);
        this.vestingTypeBox.Position = new Point(129, 180);
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      string str1 = "0";
      string str2 = "";
      if (id.Length > 8)
      {
        str1 = id.Substring(7, 2);
        str2 = id.Substring(0, 7);
      }
      else if (id.Length > 7)
      {
        str1 = id.Substring(6, 2);
        str2 = id.Substring(0, 6);
      }
      switch (str1)
      {
        case "23":
          if (this.loan.GetSimpleField(str2 + "22") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "27":
          if (string.IsNullOrEmpty(this.loan.GetSimpleField(str2 + "14")))
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

    internal override void UpdateFieldValue(string id, string val)
    {
      if (id == "NBOC0016")
        id = "NBOC" + this.ind + "16";
      base.UpdateFieldValue(id, val);
    }
  }
}

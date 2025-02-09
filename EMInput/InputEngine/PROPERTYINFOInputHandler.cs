// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PROPERTYINFOInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
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
  public class PROPERTYINFOInputHandler : InputHandlerBase
  {
    private List<EllieMae.Encompass.Forms.Panel> panelLocalAddresses;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignAddresses;
    private List<StandardButton> selectCountryButtons;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignPanels;
    private EllieMae.Encompass.Forms.Label addressLabel;
    private EllieMae.Encompass.Forms.TextBox adrTextBox;
    private string sourceEnable = string.Empty;
    private string sourceDisable = string.Empty;
    private List<ImageButton> disasterDeleteButtons;
    private EllieMae.Encompass.Forms.Button disasterDec;
    private Dictionary<string, bool> controlWithPermission = new Dictionary<string, bool>();

    public PROPERTYINFOInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public PROPERTYINFOInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public PROPERTYINFOInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public PROPERTYINFOInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    protected override string GetFieldValue(string id, FieldSource source)
    {
      return id == "2579" ? base.GetFieldValue(id, source).Replace(",", "") : base.GetFieldValue(id, source);
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "copypage1":
        case "1846":
          if (this.loan.GetField("1884") == "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "1416":
        case "11":
          return controlState;
        case "URLA.X269":
          if (this.loan.GetField("URLA.X267") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        case "disaster":
          if (this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            goto case "1416";
          }
          else
            goto case "1416";
        default:
          controlState = ControlState.Default;
          goto case "1416";
      }
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "copypage1":
          string simpleField = this.loan.GetSimpleField("17");
          if (simpleField != "")
            simpleField += "\r\n";
          this.UpdateFieldValue("1846", simpleField + this.loan.GetSimpleField("1824"));
          this.UpdateContents();
          this.SetFieldFocus("l_1846");
          break;
        case "selectcountry_URLA_X269":
          base.ExecAction(action);
          break;
        case "disaster":
          base.ExecAction(action);
          break;
        case "deldisaster0":
        case "deldisaster1":
        case "deldisaster2":
          this.RemoveDisaster(int.Parse(action.Substring(11)));
          break;
      }
    }

    internal override void CreateControls()
    {
      base.CreateControls();
      try
      {
        this.panelLocalAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignPanels = new List<EllieMae.Encompass.Forms.Panel>();
        for (int index = 1; index <= 2; ++index)
        {
          this.panelLocalAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("localAdr" + (object) index));
          this.panelForeignAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignAdr" + (object) index));
          if (this.panelForeignAddresses[index - 1] != null && this.panelLocalAddresses[index - 1] != null)
            this.panelForeignAddresses[index - 1].Position = this.panelLocalAddresses[index - 1].Position;
        }
        for (int index = 1; index <= 2; ++index)
          this.panelForeignPanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignPanel" + (object) index));
        this.selectCountryButtons = new List<StandardButton>();
        this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_URLA_X269"));
        this.addressLabel = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label8");
        this.adrTextBox = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_1416");
        this.disasterDeleteButtons = new List<ImageButton>();
        this.disasterDeleteButtons.Add((ImageButton) this.currentForm.FindControl("deldisaster1"));
        this.disasterDeleteButtons.Add((ImageButton) this.currentForm.FindControl("disdeldisaster1"));
        this.disasterDeleteButtons.Add((ImageButton) this.currentForm.FindControl("deldisaster2"));
        this.disasterDeleteButtons.Add((ImageButton) this.currentForm.FindControl("disdeldisaster2"));
        this.disasterDeleteButtons.Add((ImageButton) this.currentForm.FindControl("deldisaster3"));
        this.disasterDeleteButtons.Add((ImageButton) this.currentForm.FindControl("disdeldisaster3"));
        this.disasterDec = (EllieMae.Encompass.Forms.Button) this.currentForm.FindControl("disasterdeclarations");
        if (this.disasterDeleteButtons.Count >= 2)
        {
          this.sourceEnable = this.disasterDeleteButtons[0].Source;
          this.sourceDisable = this.disasterDeleteButtons[1].Source;
        }
        string empty = string.Empty;
        if (this.disasterDeleteButtons.Count < 6)
          return;
        for (int index = 0; index < 6; index += 2)
        {
          BizRule.FieldAccessRight fieldAccessRights = this.session.LoanDataMgr.GetFieldAccessRights("Button_" + this.disasterDeleteButtons[index].Action);
          this.controlWithPermission.Add(this.disasterDeleteButtons[index].Action, fieldAccessRights == BizRule.FieldAccessRight.ViewOnly);
        }
      }
      catch
      {
      }
    }

    private void RemoveDisaster(int index)
    {
      if (Utils.Dialog((IWin32Window) this.session.MainForm, "Are you sure you want to delete this record from the verification?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
        return;
      this.loan.RemoveDisasterAt(index);
      this.RefreshContents();
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.selectCountryButtons != null && this.selectCountryButtons.Count > 0)
      {
        bool flag = this.GetField("URLA.X267") == "Y";
        this.selectCountryButtons[0].Visible = flag;
        this.panelLocalAddresses[0].Visible = !flag;
        this.panelForeignAddresses[0].Visible = flag;
        this.panelLocalAddresses[1].Visible = !flag;
        this.panelForeignAddresses[1].Visible = flag;
      }
      if (this.GetField("1825") == "2020")
      {
        for (int index = 0; index < 2; ++index)
          this.panelForeignPanels[index].Visible = true;
        if (this.addressLabel != null && this.adrTextBox != null && this.panelLocalAddresses[0] != null)
        {
          this.addressLabel.Position = new Point(6, 28);
          this.adrTextBox.Position = new Point(126, 28);
          this.panelLocalAddresses[0].Position = new Point(0, 50);
        }
      }
      else
      {
        for (int index = 0; index < 2; ++index)
          this.panelForeignPanels[index].Visible = false;
        if (this.addressLabel != null && this.adrTextBox != null && this.panelLocalAddresses[0] != null)
        {
          this.addressLabel.Position = new Point(6, 6);
          this.adrTextBox.Position = new Point(126, 6);
          this.panelLocalAddresses[0].Position = new Point(0, 28);
        }
      }
      if (this.loan != null && this.loan.IsInFindFieldForm)
      {
        for (int index = 0; index < this.disasterDeleteButtons.Count; index += 2)
          this.disasterDeleteButtons[index].Source = this.sourceEnable;
      }
      else
      {
        if (this.disasterDeleteButtons == null || this.disasterDeleteButtons.Count < 6)
          return;
        this.SwitchImageControl(this.disasterDeleteButtons, 4, this.loan.GetNumberOfDisasters());
      }
    }

    internal void SwitchImageControl(List<ImageButton> imageButton, int totalIndex, int totalCount)
    {
      int num = 1;
      for (int index = 0; index <= totalIndex; index += 2)
      {
        bool result = totalCount >= num++;
        this.EnableDisableControl(imageButton[index], this.controlWithPermission, result, this.sourceEnable, this.sourceDisable);
      }
    }
  }
}

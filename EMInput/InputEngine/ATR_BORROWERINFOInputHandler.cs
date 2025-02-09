// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ATR_BORROWERINFOInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ATR_BORROWERINFOInputHandler : InputHandlerBase
  {
    private List<EllieMae.Encompass.Forms.Panel> panelLocalAddresses;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignAddresses;
    private List<StandardButton> selectCountryButtons;
    private List<EllieMae.Encompass.Forms.Panel> panelForeignPanels;
    private EllieMae.Encompass.Forms.GroupBox groupBoxpresentAdrBor;
    private EllieMae.Encompass.Forms.GroupBox groupBoxcurrentAdrBor;
    private EllieMae.Encompass.Forms.GroupBox groupBoxpresentAdrCoBor;
    private EllieMae.Encompass.Forms.GroupBox groupBoxcurrentAdrCoBor;
    private EllieMae.Encompass.Forms.GroupBox groupBoxCreditInfoBor;
    private EllieMae.Encompass.Forms.GroupBox groupBoxCreditInfoCoBor;
    private CategoryBox groupBoxPropInfo;
    private CategoryBox groupBoxLoanInfo;
    private EllieMae.Encompass.Forms.Panel pnlFrm;
    private EllieMae.Encompass.Forms.Label pnlFrmLabel;

    public ATR_BORROWERINFOInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public ATR_BORROWERINFOInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public ATR_BORROWERINFOInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public ATR_BORROWERINFOInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "1063":
        case "11":
        case "FR0104":
        case "FR0204":
          return controlState;
        case "9":
          if (this.loan.GetField("19") != "Other")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "994":
          if (this.loan.GetField("608") != "OtherAmortizationType")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "995":
        case "zoomarm":
          if (this.loan.GetField("608") != "AdjustableRate" && this.inputData.GetField("1172") != "HELOC")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "FR0130":
          if (this.loan.GetField("FR0129") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        case "FR0230":
          if (this.loan.GetField("FR0229") != "Y")
          {
            controlState = ControlState.Disabled;
            goto case "1063";
          }
          else
            goto case "1063";
        default:
          controlState = ControlState.Default;
          goto case "1063";
      }
    }

    internal override void CreateControls()
    {
      try
      {
        this.panelLocalAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignAddresses = new List<EllieMae.Encompass.Forms.Panel>();
        this.panelForeignPanels = new List<EllieMae.Encompass.Forms.Panel>();
        this.groupBoxpresentAdrBor = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("presentAdrBor");
        this.groupBoxpresentAdrCoBor = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("presentAdrCoBor");
        this.groupBoxcurrentAdrBor = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("currentAdrBor");
        this.groupBoxcurrentAdrCoBor = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("currentAdrCoBor");
        this.groupBoxCreditInfoBor = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox8");
        this.groupBoxCreditInfoCoBor = (EllieMae.Encompass.Forms.GroupBox) this.currentForm.FindControl("GroupBox9");
        this.groupBoxPropInfo = (CategoryBox) this.currentForm.FindControl("CategoryBox2");
        this.groupBoxLoanInfo = (CategoryBox) this.currentForm.FindControl("CategoryBox3");
        this.pnlFrm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        this.pnlFrmLabel = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("Label9");
        string field = this.inputData.GetField("1825");
        int num = 0;
        for (int index = 1; index <= 2; ++index)
        {
          this.panelLocalAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("localAdr" + (object) index));
          this.panelForeignAddresses.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("foreignAdr" + (object) index));
          if (this.panelForeignAddresses[index - 1] != null && this.panelLocalAddresses[index - 1] != null)
            this.panelForeignAddresses[index - 1].Position = this.panelLocalAddresses[index - 1].Position;
        }
        this.selectCountryButtons = new List<StandardButton>();
        for (int index = 1; index <= 2; ++index)
          this.selectCountryButtons.Add((StandardButton) this.currentForm.FindControl("selectcountry_FR0" + (object) index + "30"));
        for (int index = 1; index <= 4; ++index)
          this.panelForeignPanels.Add((EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panel" + (object) index));
        if (this.groupBoxpresentAdrBor != null && this.groupBoxcurrentAdrBor != null)
        {
          this.groupBoxpresentAdrBor.Visible = !(field == "2020");
          this.groupBoxcurrentAdrBor.Visible = field == "2020";
          this.groupBoxcurrentAdrBor.Position = new Point(2, 177);
        }
        if (this.groupBoxcurrentAdrCoBor != null && this.groupBoxpresentAdrCoBor != null)
        {
          this.groupBoxpresentAdrCoBor.Visible = !(field == "2020");
          this.groupBoxcurrentAdrCoBor.Visible = field == "2020";
          this.groupBoxcurrentAdrCoBor.Position = new Point(280, 177);
        }
        if (this.groupBoxcurrentAdrBor != null && this.groupBoxpresentAdrBor != null)
          num = this.groupBoxcurrentAdrBor.Size.Height - this.groupBoxpresentAdrBor.Size.Height - 13;
        if (!(field == "2020") || this.groupBoxcurrentAdrBor == null || this.groupBoxcurrentAdrCoBor == null || this.groupBoxpresentAdrBor == null || this.groupBoxpresentAdrCoBor == null || this.pnlFrm == null || this.pnlFrmLabel == null)
          return;
        this.groupBoxCreditInfoBor.Position = new Point(this.groupBoxCreditInfoBor.Position.X, this.groupBoxCreditInfoBor.Position.Y + num);
        EllieMae.Encompass.Forms.GroupBox boxCreditInfoCoBor = this.groupBoxCreditInfoCoBor;
        Point position1 = this.groupBoxCreditInfoCoBor.Position;
        int x1 = position1.X;
        position1 = this.groupBoxCreditInfoCoBor.Position;
        int y1 = position1.Y + num;
        Point point1 = new Point(x1, y1);
        boxCreditInfoCoBor.Position = point1;
        this.groupBoxPropInfo.Position = new Point(this.groupBoxPropInfo.Position.X, this.groupBoxPropInfo.Position.Y + num);
        this.groupBoxLoanInfo.Position = new Point(this.groupBoxLoanInfo.Position.X, this.groupBoxLoanInfo.Position.Y + num);
        EllieMae.Encompass.Forms.Label pnlFrmLabel = this.pnlFrmLabel;
        Point position2 = this.pnlFrmLabel.Position;
        int x2 = position2.X;
        position2 = this.pnlFrmLabel.Position;
        int y2 = position2.Y + num;
        Point point2 = new Point(x2, y2);
        pnlFrmLabel.Position = point2;
        this.pnlFrm.Size = new Size(this.pnlFrm.Size.Width, this.pnlFrm.Size.Height + num);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (!(id == "608"))
        return;
      switch (val)
      {
        case "Fixed":
          base.UpdateFieldValue("994", string.Empty);
          break;
        case "AdjustableRate":
          base.UpdateFieldValue("994", string.Empty);
          break;
      }
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      switch (action)
      {
        case "zoomarm":
          this.SetFieldFocus("l_995");
          break;
        case "copybrw":
          this.SetFieldFocus("l_68");
          break;
      }
    }

    protected override void UpdateContents(
      bool refreshAllFields,
      bool fireChangeEvents,
      bool skipButtonFieldLockRules)
    {
      base.UpdateContents(refreshAllFields, fireChangeEvents, skipButtonFieldLockRules);
      if (this.selectCountryButtons == null || this.selectCountryButtons.Count <= 0)
        return;
      for (int index = 0; index < this.selectCountryButtons.Count; ++index)
      {
        if (this.selectCountryButtons[index] != null)
        {
          bool flag = this.GetField("FR0" + (object) (index + 1) + "29") == "Y";
          this.selectCountryButtons[index].Visible = flag;
          this.panelLocalAddresses[index].Visible = !flag;
          this.panelForeignAddresses[index].Visible = flag;
        }
      }
    }
  }
}

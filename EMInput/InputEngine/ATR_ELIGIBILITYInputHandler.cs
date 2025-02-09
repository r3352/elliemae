// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ATR_ELIGIBILITYInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ATR_ELIGIBILITYInputHandler : InputHandlerBase
  {
    private bool isSmallCreditor;
    private DropdownBox loanProgramOptions;

    public ATR_ELIGIBILITYInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public ATR_ELIGIBILITYInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
      this.refreshAllCheckmarks();
    }

    public ATR_ELIGIBILITYInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public ATR_ELIGIBILITYInputHandler(
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
      if (this.loan != null && this.loan.Calculator != null)
      {
        this.loan.Calculator.CalcOnDemand(CalcOnDemandEnum.PaymentSchedule);
        this.loan.TriggerCalculation("QM.X383", (string) null);
      }
      try
      {
        this.loanProgramOptions = (DropdownBox) this.currentForm.FindControl("DropdownBox4");
        DateTime date1 = Utils.ParseDate((object) this.GetField("745"));
        DateTime date2 = date1.Date;
        date1 = DateTime.Parse("07/15/2017");
        DateTime date3 = date1.Date;
        if (date2 >= date3)
        {
          this.loanProgramOptions.Options.RemoveAt(3);
          this.loanProgramOptions.Options.RemoveAt(2);
        }
      }
      catch (Exception ex)
      {
      }
      if (this.session.LoanDataMgr == null || this.session.LoanDataMgr.SystemConfiguration == null || this.session.LoanDataMgr.SystemConfiguration.DisplayOrganization == null || this.session.LoanDataMgr.SystemConfiguration.DisplayOrganization.OrgBranchLicensing == null)
        return;
      this.isSmallCreditor = this.session.LoanDataMgr.SystemConfiguration.DisplayOrganization.OrgBranchLicensing.ATRSmallCreditor == BranchLicensing.ATRSmallCreditors.SmallCreditor || this.session.LoanDataMgr.SystemConfiguration.DisplayOrganization.OrgBranchLicensing.ATRSmallCreditor == BranchLicensing.ATRSmallCreditors.RuralSmallCreditor;
    }

    public override void onmouseenter(mshtml.IHTMLEventObj pEvtObj)
    {
      if (this.inputData != null && pEvtObj.srcElement.title == null && pEvtObj.srcElement.tagName == "IMG")
      {
        RuntimeControl controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as RuntimeControl;
        if (controlForElement.ControlID.StartsWith("imgCol"))
          pEvtObj.srcElement.title = this.getFieldHelp(controlForElement.ControlID);
      }
      base.onmouseenter(pEvtObj);
    }

    private string getFieldHelp(string imageID)
    {
      string empty = string.Empty;
      int num1 = Utils.ParseInt((object) imageID.Substring(6, 1));
      int num2 = Utils.ParseInt((object) imageID.Substring(7));
      string helpKey;
      switch (num1)
      {
        case 1:
          helpKey = "QM.X" + (object) (num2 + 25);
          break;
        case 2:
          helpKey = num2 != 24 ? "QM.X" + (object) (num2 + 39) : "QM.X384";
          break;
        case 3:
          helpKey = num2 != 22 ? "QM.X" + (object) (num2 + 61) : "QM.X380";
          break;
        case 4:
          switch (num2)
          {
            case 19:
              helpKey = "QM.X350";
              break;
            case 20:
              helpKey = "QM.X351";
              break;
            default:
              helpKey = "QM.X" + (object) (num2 + 83);
              break;
          }
          break;
        default:
          return string.Empty;
      }
      string str = FieldHelp.GetText(helpKey) ?? "";
      return helpKey + (str.Trim() != string.Empty ? ": " : "") + str;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      this.refreshAllCheckmarks();
    }

    public override void RefreshContents()
    {
      base.RefreshContents();
      this.refreshAllCheckmarks();
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "QM.X103":
          if (this.inputData.GetField("19") == "ConstructionOnly" && Utils.ParseInt((object) this.inputData.GetField("1176")) <= 12)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "QM.X104":
        case "QM.X108":
        case "QM.X110":
          if (this.inputData.GetField("QM.X103") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "QM.X105":
          if (this.inputData.GetField("QM.X104") != "Y")
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "QM.X106":
          if (this.inputData.GetField("QM.X103") != "Y" || this.inputData.GetField("19") == "ConstructionOnly" && Utils.ParseInt((object) this.inputData.GetField("1176")) <= 12)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "QM.X107":
          if (this.inputData.GetField("QM.X106") != "Y" || this.inputData.GetField("19") == "ConstructionOnly" && Utils.ParseInt((object) this.inputData.GetField("1176")) <= 12)
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

    private void refreshAllCheckmarks()
    {
      int num;
      for (int index = 26; index <= 39; ++index)
      {
        if (index != 32)
        {
          num = index - 25;
          this.refreshCheckmarks("imgCol1" + num.ToString("00"), this.GetField("QM.X" + (object) index));
        }
      }
      for (int index = 40; index <= 60; ++index)
      {
        if (index != 52)
        {
          num = index - 39;
          this.refreshCheckmarks("imgCol2" + num.ToString("00"), this.GetField("QM.X" + (object) index));
        }
      }
      for (int index = 62; index <= 82; ++index)
      {
        if (index != 74)
        {
          num = index - 61;
          this.refreshCheckmarks("imgCol3" + num.ToString("00"), this.GetField("QM.X" + (object) index));
        }
      }
      for (int index = 84; index <= 101; ++index)
      {
        if (index != 95)
        {
          num = index - 83;
          this.refreshCheckmarks("imgCol4" + num.ToString("00"), this.GetField("QM.X" + (object) index));
        }
      }
      this.refreshCheckmarks("imgCol419", this.GetField("QM.X350"));
      this.refreshCheckmarks("imgCol420", this.GetField("QM.X351"));
      this.refreshCheckmarks("imgCol322", this.GetField("QM.X380"));
      this.refreshCheckmarks("imgCol224", this.GetField("QM.X384"));
    }

    private void refreshCheckmarks(string imageID, string status)
    {
      Image control = (Image) this.currentForm.FindControl(imageID);
      if (control == null)
        return;
      control.Visible = true;
      switch (status)
      {
        case "Not Meet":
          control.Source = AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.FormRelDir, "images\\alert-message-high.png"), SystemSettings.LocalAppDir);
          break;
        case "Review Needed":
          control.Source = AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.FormRelDir, "images\\alert-message-low.png"), SystemSettings.LocalAppDir);
          break;
        case "Meets Standard":
          control.Source = AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.FormRelDir, "images\\check-mark-green.png"), SystemSettings.LocalAppDir);
          break;
        default:
          control.Visible = false;
          break;
      }
    }
  }
}

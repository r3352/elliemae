// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ESIGNERREPRESENTATIVESInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ESIGNERREPRESENTATIVESInputHandler : InputHandlerBase
  {
    private DropdownBox field_LenderRep;
    private DropdownBox field_Lender;
    private DropdownBox field_Broker;
    private DropdownBox field_Usda;
    private DropdownBox field_FhaVA;
    private DropdownBox field_FhaVAMortgagee;
    private DropdownBox field_Commitment;

    public ESIGNERREPRESENTATIVESInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public ESIGNERREPRESENTATIVESInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public ESIGNERREPRESENTATIVESInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public ESIGNERREPRESENTATIVESInputHandler(
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
        this.field_LenderRep = (DropdownBox) this.currentForm.FindControl("LenderRepDropdownBox");
        this.populateLenderValues(this.field_LenderRep);
        this.field_Lender = (DropdownBox) this.currentForm.FindControl("LenderDropdownBox");
        this.populateLenderValues(this.field_Lender);
        this.field_Broker = (DropdownBox) this.currentForm.FindControl("BrokerDropdownBox");
        this.populateLenderValues(this.field_Broker);
        this.field_Usda = (DropdownBox) this.currentForm.FindControl("UsdaDropdownBox");
        this.populateLenderValues(this.field_Usda);
        this.field_FhaVA = (DropdownBox) this.currentForm.FindControl("FhaVaDropdownBox");
        this.populateLenderValues(this.field_FhaVA);
        this.field_FhaVAMortgagee = (DropdownBox) this.currentForm.FindControl("FhaVaMortgageeDropdownBox");
        this.populateLenderValues(this.field_FhaVAMortgagee);
        this.field_Commitment = (DropdownBox) this.currentForm.FindControl("CommitmentDropdownBox");
        this.populateLenderValues(this.field_Commitment);
      }
      catch
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      this.getControlState(ctrl, id, ControlState.Enabled);
      ControlState controlState;
      switch (id)
      {
        case "1256":
          controlState = this.inputData.GetField("4806") == "" || this.inputData.GetField("4806") == "None" ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "1262":
        case "4808":
        case "95":
        case "Lender.CntctTitle":
          controlState = this.inputData.GetField("4806") == "" || this.inputData.GetField("4806") == "None" || this.inputData.GetField("4832") == "Y" ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "1754":
          controlState = this.inputData.GetField("4814") == "" || this.inputData.GetField("4814") == "None" ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "1755":
        case "1756":
        case "4815":
        case "4817":
          controlState = this.inputData.GetField("4814") == "" || this.inputData.GetField("4814") == "None" || this.inputData.GetField("4835") == "Y" ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "3194":
          controlState = this.inputData.GetField("4818") == "" || this.inputData.GetField("4818") == "None" ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "4673":
          controlState = this.inputData.GetField("4672") == "" || this.inputData.GetField("4672") == "None" ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "4674":
        case "4676":
        case "4677":
        case "4683":
          controlState = this.inputData.GetField("4672") == "" || this.inputData.GetField("4672") == "None" || this.inputData.GetField("4840") == "Y" ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "4812":
        case "4838":
        case "4839":
        case "USDA.X30":
          controlState = this.inputData.GetField("4811") == "" || this.inputData.GetField("4811") == "None" || this.inputData.GetField("4834") == "Y" ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "4819":
        case "4820":
        case "4822":
        case "4823":
          controlState = this.inputData.GetField("4818") == "" || this.inputData.GetField("4818") == "None" || this.inputData.GetField("4836") == "Y" ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "4825":
        case "4826":
        case "4829":
        case "NOTICES.X37":
          controlState = this.inputData.GetField("4824") == "" || this.inputData.GetField("4824") == "None" || this.inputData.GetField("4837") == "Y" ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "4832":
        case "lenderlookup":
          controlState = this.inputData.GetField("4806") == "" || this.inputData.GetField("4806") == "None" || id == "lenderlookup" && this.inputData.GetField("4806").StartsWith("Custom") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "4833":
        case "brokerlookup":
          controlState = this.inputData.GetField("4809") == "" || this.inputData.GetField("4809") == "None" || id == "brokerlookup" && this.inputData.GetField("4809").StartsWith("Custom") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "4834":
        case "usdalookup":
          controlState = this.inputData.GetField("4811") == "" || this.inputData.GetField("4811") == "None" || id == "usdalookup" && this.inputData.GetField("4811").StartsWith("Custom") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "4835":
        case "fhavalookup":
          controlState = this.inputData.GetField("4814") == "" || this.inputData.GetField("4814") == "None" || id == "fhavalookup" && this.inputData.GetField("4814").StartsWith("Custom") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "4836":
        case "fhavamortageelookup":
          controlState = this.inputData.GetField("4818") == "" || this.inputData.GetField("4818") == "None" || id == "fhavamortageelookup" && this.inputData.GetField("4818").StartsWith("Custom") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "4837":
        case "commitmentissuedlookup":
          controlState = this.inputData.GetField("4824") == "" || this.inputData.GetField("4824") == "None" || id == "commitmentissuedlookup" && this.inputData.GetField("4824").StartsWith("Custom") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "4840":
        case "lenderreplookup":
          controlState = this.inputData.GetField("4672") == "" || this.inputData.GetField("4672") == "None" || id == "lenderreplookup" && this.inputData.GetField("4672").StartsWith("Custom") ? ControlState.Disabled : ControlState.Enabled;
          break;
        case "Broker.CntctTitle":
        case "VEND.X303":
        case "VEND.X304":
        case "VEND.X305":
          controlState = this.inputData.GetField("4809") == "" || this.inputData.GetField("4809") == "None" || this.inputData.GetField("4833") == "Y" ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "NOTICES.X31":
          controlState = this.inputData.GetField("4824") == "" || this.inputData.GetField("4824") == "None" ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "USDA.X31":
          controlState = this.inputData.GetField("4811") == "" || this.inputData.GetField("4811") == "None" ? ControlState.Enabled : ControlState.Disabled;
          break;
        case "VEND.X302":
          controlState = this.inputData.GetField("4809") == "" || this.inputData.GetField("4809") == "None" ? ControlState.Enabled : ControlState.Disabled;
          break;
        default:
          controlState = ControlState.Default;
          break;
      }
      return controlState;
    }

    private void populateLenderValues(DropdownBox field_LenderRep)
    {
      field_LenderRep.Options.Add("");
      field_LenderRep.Options.Add("None");
      RoleInfo[] roleInfoArray = !this.IsUsingTemplate ? this.session.LoanDataMgr.SystemConfiguration.AllRoles : ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
      if (roleInfoArray == null)
        return;
      foreach (RoleInfo roleInfo in roleInfoArray)
        field_LenderRep.Options.Add("Role - " + roleInfo.RoleName);
      string text1 = this.loan.GetField("VEND.X84");
      if (string.IsNullOrEmpty(text1))
        text1 = "Custom Category #1";
      field_LenderRep.Options.Add(text1);
      string text2 = this.loan.GetField("VEND.X85");
      if (string.IsNullOrEmpty(text2))
        text2 = "Custom Category #2";
      field_LenderRep.Options.Add(text2);
      string text3 = this.loan.GetField("VEND.X86");
      if (string.IsNullOrEmpty(text3))
        text3 = "Custom Category #3";
      field_LenderRep.Options.Add(text3);
      string text4 = this.loan.GetField("VEND.X11");
      if (string.IsNullOrEmpty(text4))
        text4 = "Custom Category #4";
      field_LenderRep.Options.Add(text4);
    }
  }
}

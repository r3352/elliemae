// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SHIPPINGDETAILInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using mshtml;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class SHIPPINGDETAILInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Label labelInvDeliDate;
    private AlertConfig shippingAlert;
    private MilestoneLog currentMilestone;

    public SHIPPINGDETAILInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public SHIPPINGDETAILInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
      if (this.labelInvDeliDate == null)
        this.labelInvDeliDate = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl(nameof (labelInvDeliDate));
      if (this.shippingAlert == null)
      {
        foreach (AlertConfig alertConfig in Session.StartupInfo.AlertConfigs)
        {
          if (alertConfig.AlertID == 12)
            this.shippingAlert = alertConfig;
        }
      }
      if (this.shippingAlert != null)
      {
        MilestoneLog[] allMilestones = this.loan.GetLogList().GetAllMilestones();
        for (int index = allMilestones.Length - 1; index >= 0; --index)
        {
          if (allMilestones[index].Done)
          {
            this.currentMilestone = allMilestones[index];
            break;
          }
        }
      }
      this.checkInvestorDeliveryDate();
      this.RefreshContents();
    }

    public SHIPPINGDETAILInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public SHIPPINGDETAILInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (!(id == "2012") && !(id == "2014"))
        return;
      this.checkInvestorDeliveryDate();
    }

    private void checkInvestorDeliveryDate()
    {
      DateTime date1 = Utils.ParseDate((object) this.GetFieldValue("2012"), false);
      DateTime date2 = Utils.ParseDate((object) this.GetFieldValue("2014"), false);
      this.labelInvDeliDate.ForeColor = Color.Black;
      if (this.labelInvDeliDate == null || !(date1 != DateTime.MinValue) || !(date2 == DateTime.MinValue) || this.shippingAlert == null || !this.shippingAlert.AlertEnabled || !this.shippingAlert.MilestoneGuidList.Contains(this.currentMilestone.MilestoneID) || (date1.Date - DateTime.Today).Days > this.shippingAlert.DaysBefore)
        return;
      this.labelInvDeliDate.ForeColor = Color.Red;
    }

    public override void ExecAction(string action)
    {
      switch (action)
      {
        case "copyfromshipto1":
          this.UpdateFieldValue("VEND.X378", this.GetFieldValue("VEND.X369"));
          this.UpdateFieldValue("VEND.X379", this.GetFieldValue("VEND.X370"));
          this.UpdateFieldValue("VEND.X380", this.GetFieldValue("VEND.X371"));
          this.UpdateFieldValue("VEND.X381", this.GetFieldValue("VEND.X372"));
          this.UpdateFieldValue("VEND.X382", this.GetFieldValue("VEND.X373"));
          this.UpdateFieldValue("VEND.X383", this.GetFieldValue("VEND.X374"));
          this.UpdateFieldValue("VEND.X384", this.GetFieldValue("VEND.X375"));
          this.UpdateFieldValue("VEND.X385", this.GetFieldValue("VEND.X376"));
          this.UpdateFieldValue("VEND.X386", this.GetFieldValue("VEND.X377"));
          this.UpdateContents();
          this.SetFieldFocus("l_x378");
          break;
        case "copyfromshipto10":
          this.UpdateFieldValue("VEND.X599", this.GetFieldValue("VEND.X369"));
          this.UpdateFieldValue("VEND.X600", this.GetFieldValue("VEND.X370"));
          this.UpdateFieldValue("VEND.X602", this.GetFieldValue("VEND.X371"));
          this.UpdateFieldValue("VEND.X603", this.GetFieldValue("VEND.X372"));
          this.UpdateFieldValue("VEND.X604", this.GetFieldValue("VEND.X373"));
          this.UpdateFieldValue("VEND.X605", this.GetFieldValue("VEND.X374"));
          this.UpdateFieldValue("VEND.X606", this.GetFieldValue("VEND.X375"));
          this.UpdateFieldValue("VEND.X607", this.GetFieldValue("VEND.X376"));
          this.UpdateFieldValue("VEND.X608", this.GetFieldValue("VEND.X377"));
          this.UpdateContents();
          this.SetFieldFocus("l_x599");
          break;
        case "copyfromshipto11":
          this.UpdateFieldValue("VEND.X609", this.GetFieldValue("VEND.X369"));
          this.UpdateFieldValue("VEND.X610", this.GetFieldValue("VEND.X370"));
          this.UpdateFieldValue("VEND.X612", this.GetFieldValue("VEND.X371"));
          this.UpdateFieldValue("VEND.X613", this.GetFieldValue("VEND.X372"));
          this.UpdateFieldValue("VEND.X614", this.GetFieldValue("VEND.X373"));
          this.UpdateFieldValue("VEND.X615", this.GetFieldValue("VEND.X374"));
          this.UpdateFieldValue("VEND.X616", this.GetFieldValue("VEND.X375"));
          this.UpdateFieldValue("VEND.X617", this.GetFieldValue("VEND.X376"));
          this.UpdateFieldValue("VEND.X618", this.GetFieldValue("VEND.X377"));
          this.UpdateContents();
          this.SetFieldFocus("l_x609");
          break;
        case "copyfromshipto12":
          this.UpdateFieldValue("VEND.X619", this.GetFieldValue("VEND.X369"));
          this.UpdateFieldValue("VEND.X620", this.GetFieldValue("VEND.X370"));
          this.UpdateFieldValue("VEND.X622", this.GetFieldValue("VEND.X371"));
          this.UpdateFieldValue("VEND.X623", this.GetFieldValue("VEND.X372"));
          this.UpdateFieldValue("VEND.X624", this.GetFieldValue("VEND.X373"));
          this.UpdateFieldValue("VEND.X625", this.GetFieldValue("VEND.X374"));
          this.UpdateFieldValue("VEND.X626", this.GetFieldValue("VEND.X375"));
          this.UpdateFieldValue("VEND.X627", this.GetFieldValue("VEND.X376"));
          this.UpdateFieldValue("VEND.X628", this.GetFieldValue("VEND.X377"));
          this.UpdateContents();
          this.SetFieldFocus("l_x619");
          break;
        case "copyfromshipto13":
          this.UpdateFieldValue("VEND.X629", this.GetFieldValue("VEND.X369"));
          this.UpdateFieldValue("VEND.X630", this.GetFieldValue("VEND.X370"));
          this.UpdateFieldValue("VEND.X632", this.GetFieldValue("VEND.X371"));
          this.UpdateFieldValue("VEND.X633", this.GetFieldValue("VEND.X372"));
          this.UpdateFieldValue("VEND.X634", this.GetFieldValue("VEND.X373"));
          this.UpdateFieldValue("VEND.X635", this.GetFieldValue("VEND.X374"));
          this.UpdateFieldValue("VEND.X636", this.GetFieldValue("VEND.X375"));
          this.UpdateFieldValue("VEND.X637", this.GetFieldValue("VEND.X376"));
          this.UpdateFieldValue("VEND.X638", this.GetFieldValue("VEND.X377"));
          this.UpdateContents();
          this.SetFieldFocus("l_x629");
          break;
        case "copyfromshipto14":
          this.UpdateFieldValue("VEND.X639", this.GetFieldValue("VEND.X369"));
          this.UpdateFieldValue("VEND.X640", this.GetFieldValue("VEND.X370"));
          this.UpdateFieldValue("VEND.X642", this.GetFieldValue("VEND.X371"));
          this.UpdateFieldValue("VEND.X643", this.GetFieldValue("VEND.X372"));
          this.UpdateFieldValue("VEND.X644", this.GetFieldValue("VEND.X373"));
          this.UpdateFieldValue("VEND.X645", this.GetFieldValue("VEND.X374"));
          this.UpdateFieldValue("VEND.X646", this.GetFieldValue("VEND.X375"));
          this.UpdateFieldValue("VEND.X647", this.GetFieldValue("VEND.X376"));
          this.UpdateFieldValue("VEND.X648", this.GetFieldValue("VEND.X377"));
          this.UpdateContents();
          this.SetFieldFocus("l_x639");
          break;
        case "copyfromshipto2":
          this.UpdateFieldValue("VEND.X387", this.GetFieldValue("VEND.X369"));
          this.UpdateFieldValue("VEND.X388", this.GetFieldValue("VEND.X370"));
          this.UpdateFieldValue("VEND.X389", this.GetFieldValue("VEND.X371"));
          this.UpdateFieldValue("VEND.X390", this.GetFieldValue("VEND.X372"));
          this.UpdateFieldValue("VEND.X391", this.GetFieldValue("VEND.X373"));
          this.UpdateFieldValue("VEND.X392", this.GetFieldValue("VEND.X374"));
          this.UpdateFieldValue("VEND.X393", this.GetFieldValue("VEND.X375"));
          this.UpdateFieldValue("VEND.X394", this.GetFieldValue("VEND.X376"));
          this.UpdateFieldValue("VEND.X395", this.GetFieldValue("VEND.X377"));
          this.UpdateContents();
          this.SetFieldFocus("l_x387");
          break;
        case "copyfromshipto3":
          this.UpdateFieldValue("VEND.X529", this.GetFieldValue("VEND.X369"));
          this.UpdateFieldValue("VEND.X530", this.GetFieldValue("VEND.X370"));
          this.UpdateFieldValue("VEND.X532", this.GetFieldValue("VEND.X371"));
          this.UpdateFieldValue("VEND.X533", this.GetFieldValue("VEND.X372"));
          this.UpdateFieldValue("VEND.X534", this.GetFieldValue("VEND.X373"));
          this.UpdateFieldValue("VEND.X535", this.GetFieldValue("VEND.X374"));
          this.UpdateFieldValue("VEND.X536", this.GetFieldValue("VEND.X375"));
          this.UpdateFieldValue("VEND.X537", this.GetFieldValue("VEND.X376"));
          this.UpdateFieldValue("VEND.X538", this.GetFieldValue("VEND.X377"));
          this.UpdateContents();
          this.SetFieldFocus("l_x529");
          break;
        case "copyfromshipto4":
          this.UpdateFieldValue("VEND.X539", this.GetFieldValue("VEND.X369"));
          this.UpdateFieldValue("VEND.X540", this.GetFieldValue("VEND.X370"));
          this.UpdateFieldValue("VEND.X542", this.GetFieldValue("VEND.X371"));
          this.UpdateFieldValue("VEND.X543", this.GetFieldValue("VEND.X372"));
          this.UpdateFieldValue("VEND.X544", this.GetFieldValue("VEND.X373"));
          this.UpdateFieldValue("VEND.X545", this.GetFieldValue("VEND.X374"));
          this.UpdateFieldValue("VEND.X546", this.GetFieldValue("VEND.X375"));
          this.UpdateFieldValue("VEND.X547", this.GetFieldValue("VEND.X376"));
          this.UpdateFieldValue("VEND.X548", this.GetFieldValue("VEND.X377"));
          this.UpdateContents();
          this.SetFieldFocus("l_x539");
          break;
        case "copyfromshipto5":
          this.UpdateFieldValue("VEND.X549", this.GetFieldValue("VEND.X369"));
          this.UpdateFieldValue("VEND.X550", this.GetFieldValue("VEND.X370"));
          this.UpdateFieldValue("VEND.X552", this.GetFieldValue("VEND.X371"));
          this.UpdateFieldValue("VEND.X553", this.GetFieldValue("VEND.X372"));
          this.UpdateFieldValue("VEND.X554", this.GetFieldValue("VEND.X373"));
          this.UpdateFieldValue("VEND.X555", this.GetFieldValue("VEND.X374"));
          this.UpdateFieldValue("VEND.X556", this.GetFieldValue("VEND.X375"));
          this.UpdateFieldValue("VEND.X557", this.GetFieldValue("VEND.X376"));
          this.UpdateFieldValue("VEND.X558", this.GetFieldValue("VEND.X377"));
          this.UpdateContents();
          this.SetFieldFocus("l_x549");
          break;
        case "copyfromshipto6":
          this.UpdateFieldValue("VEND.X559", this.GetFieldValue("VEND.X369"));
          this.UpdateFieldValue("VEND.X560", this.GetFieldValue("VEND.X370"));
          this.UpdateFieldValue("VEND.X562", this.GetFieldValue("VEND.X371"));
          this.UpdateFieldValue("VEND.X563", this.GetFieldValue("VEND.X372"));
          this.UpdateFieldValue("VEND.X564", this.GetFieldValue("VEND.X373"));
          this.UpdateFieldValue("VEND.X565", this.GetFieldValue("VEND.X374"));
          this.UpdateFieldValue("VEND.X566", this.GetFieldValue("VEND.X375"));
          this.UpdateFieldValue("VEND.X567", this.GetFieldValue("VEND.X376"));
          this.UpdateFieldValue("VEND.X568", this.GetFieldValue("VEND.X377"));
          this.UpdateContents();
          this.SetFieldFocus("l_x559");
          break;
        case "copyfromshipto7":
          this.UpdateFieldValue("VEND.X569", this.GetFieldValue("VEND.X369"));
          this.UpdateFieldValue("VEND.X570", this.GetFieldValue("VEND.X370"));
          this.UpdateFieldValue("VEND.X572", this.GetFieldValue("VEND.X371"));
          this.UpdateFieldValue("VEND.X573", this.GetFieldValue("VEND.X372"));
          this.UpdateFieldValue("VEND.X574", this.GetFieldValue("VEND.X373"));
          this.UpdateFieldValue("VEND.X575", this.GetFieldValue("VEND.X374"));
          this.UpdateFieldValue("VEND.X576", this.GetFieldValue("VEND.X375"));
          this.UpdateFieldValue("VEND.X577", this.GetFieldValue("VEND.X376"));
          this.UpdateFieldValue("VEND.X578", this.GetFieldValue("VEND.X377"));
          this.UpdateContents();
          this.SetFieldFocus("l_x569");
          break;
        case "copyfromshipto8":
          this.UpdateFieldValue("VEND.X579", this.GetFieldValue("VEND.X369"));
          this.UpdateFieldValue("VEND.X580", this.GetFieldValue("VEND.X370"));
          this.UpdateFieldValue("VEND.X582", this.GetFieldValue("VEND.X371"));
          this.UpdateFieldValue("VEND.X583", this.GetFieldValue("VEND.X372"));
          this.UpdateFieldValue("VEND.X584", this.GetFieldValue("VEND.X373"));
          this.UpdateFieldValue("VEND.X585", this.GetFieldValue("VEND.X374"));
          this.UpdateFieldValue("VEND.X586", this.GetFieldValue("VEND.X375"));
          this.UpdateFieldValue("VEND.X587", this.GetFieldValue("VEND.X376"));
          this.UpdateFieldValue("VEND.X588", this.GetFieldValue("VEND.X377"));
          this.UpdateContents();
          this.SetFieldFocus("l_x579");
          break;
        case "copyfromshipto9":
          this.UpdateFieldValue("VEND.X589", this.GetFieldValue("VEND.X369"));
          this.UpdateFieldValue("VEND.X590", this.GetFieldValue("VEND.X370"));
          this.UpdateFieldValue("VEND.X592", this.GetFieldValue("VEND.X371"));
          this.UpdateFieldValue("VEND.X593", this.GetFieldValue("VEND.X372"));
          this.UpdateFieldValue("VEND.X594", this.GetFieldValue("VEND.X373"));
          this.UpdateFieldValue("VEND.X595", this.GetFieldValue("VEND.X374"));
          this.UpdateFieldValue("VEND.X596", this.GetFieldValue("VEND.X375"));
          this.UpdateFieldValue("VEND.X597", this.GetFieldValue("VEND.X376"));
          this.UpdateFieldValue("VEND.X598", this.GetFieldValue("VEND.X377"));
          this.UpdateContents();
          this.SetFieldFocus("l_x589");
          break;
        default:
          base.ExecAction(action);
          break;
      }
    }
  }
}

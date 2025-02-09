// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.NMLSINFOInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class NMLSINFOInputHandler : InputHandlerBase
  {
    private string initAppDateId = "";

    public NMLSINFOInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public NMLSINFOInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public NMLSINFOInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public NMLSINFOInputHandler(
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
      this.refreshNonFieldData();
      this.initAppDateId = this.session.ConfigurationManager.GetCompanySetting("NMLS", "AppDateFieldID");
      if (this.initAppDateId != "" && this.initAppDateId != "3142")
      {
        EllieMae.Encompass.Forms.TextBox control1 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_3142");
        Calendar control2 = (Calendar) this.currentForm.FindControl("Calendar3");
        FieldDescriptor fieldDescriptor = EncompassApplication.Session.Loans.FieldDescriptors[this.initAppDateId];
        control1.Field = fieldDescriptor;
        control1.HelpKey = this.initAppDateId;
        control2.DateField = fieldDescriptor;
      }
      this.loan.Calculator.FormCalculation("NMLSINFO", this.initAppDateId, this.GetField(this.initAppDateId));
    }

    public override void RefreshContents()
    {
      base.RefreshContents();
      this.refreshNonFieldData();
    }

    private void refreshNonFieldData()
    {
      EllieMae.Encompass.Forms.TextBox control1 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txtAmortization");
      EllieMae.Encompass.Forms.CheckBox control2 = (EllieMae.Encompass.Forms.CheckBox) this.currentForm.FindControl("chkOptionARM");
      control2.Enabled = false;
      switch (this.GetField("608"))
      {
        case "Fixed":
          control1.Text = "Fixed";
          break;
        case "AdjustableRate":
          control1.Text = "ARM";
          control2.Enabled = true;
          break;
        default:
          control1.Text = "";
          break;
      }
      EllieMae.Encompass.Forms.TextBox control3 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("txtInterestOnly");
      string field = this.GetField("1177");
      if (field != "" && Utils.ParseInt((object) field) != 0)
        control3.Text = "Yes";
      else
        control3.Text = "No";
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (!(id == this.initAppDateId))
        return;
      this.loan.Calculator.FormCalculation("NMLSINFO", id, val);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CLOSINGVENDORSInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class CLOSINGVENDORSInputHandler : InputHandlerBase
  {
    private DropdownBox boxOrgState;
    private PickList pickListOrgType;
    private EllieMae.Encompass.Forms.TextBox boxOrgType;
    private string[] orgType;

    public CLOSINGVENDORSInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public CLOSINGVENDORSInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public CLOSINGVENDORSInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public CLOSINGVENDORSInputHandler(
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
      this.boxOrgState = (DropdownBox) this.currentForm.FindControl("boxOrgState");
      this.pickListOrgType = (PickList) this.currentForm.FindControl("pickListOrgType");
      this.boxOrgType = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("boxOrgType");
      this.pickListOrgType.BoundControl = (FieldControl) this.boxOrgType;
      this.pickListOrgType.ItemSelected += new ItemSelectedEventHandler(this.onListItemSelected);
      this.initialStateField();
    }

    private void initialStateField()
    {
      if (this.boxOrgState == null)
        return;
      string[] strArray = new string[52]
      {
        "",
        "Alabama",
        "Alaska",
        "Arizona",
        "Arkansas",
        "California",
        "Colorado",
        "Connecticut",
        "Delaware",
        "Dist. of Col.",
        "Florida",
        "Georgia",
        "Hawaii",
        "Idaho",
        "Illinois",
        "Indiana",
        "Iowa",
        "Kansas",
        "Kentucky",
        "Louisiana",
        "Maine",
        "Maryland",
        "Massachusetts",
        "Michigan",
        "Minnesota",
        "Mississippi",
        "Missouri",
        "Montana",
        "Nebraska",
        "Nevada",
        "New Hampshire",
        "New Jersey",
        "New Mexico",
        "New York",
        "North Carolina",
        "North Dakota",
        "Ohio",
        "Oklahoma",
        "Oregon",
        "Pennsylvania",
        "Rhode Island",
        "South Carolina",
        "South Dakota",
        "Tennessee",
        "Texas",
        "Utah",
        "Vermont",
        "Virginia",
        "Washington",
        "West Virginia",
        "Wisconsin",
        "Wyoming"
      };
      foreach (string text in strArray)
        this.boxOrgState.Options.Add(text);
      this.orgType = new string[15]
      {
        "A Trust",
        "A Corporation",
        "A General Partnership",
        "A Sole Proprietorship",
        "A Limited Partnership",
        "A Partnership",
        "A Federal Savings Association",
        "A Federal Savings Bank",
        "A Federal Bank",
        "A Federal Credit Union",
        "A National Association",
        "A National Bank",
        "A National Banking Association",
        "A Limited Liability Company",
        "An Inter Vivos Trust"
      };
      this.initialStateType("");
    }

    private void initialStateType(string stateName)
    {
      if (this.pickListOrgType == null || this.boxOrgState == null)
        return;
      string str = "A ";
      if (stateName != "")
      {
        string lower = stateName.Substring(0, 1).ToLower();
        if (lower == "a" || lower == "e" || lower == "i" || lower == "o")
          str = "An ";
      }
      this.pickListOrgType.Options.Clear();
      if (stateName != "")
      {
        this.pickListOrgType.Options.Add("");
        this.pickListOrgType.Options.Add(str + stateName + " Trust");
        this.pickListOrgType.Options.Add(str + stateName + " Corporation");
        this.pickListOrgType.Options.Add(str + stateName + " Banking Corporation");
        this.pickListOrgType.Options.Add(str + stateName + " General Partnership");
        this.pickListOrgType.Options.Add(str + stateName + " Limited Partnership");
        this.pickListOrgType.Options.Add(str + stateName + " Limited Liability Company");
      }
      for (int index = 0; index < this.orgType.Length; ++index)
        this.pickListOrgType.Options.Add(this.orgType[index]);
    }

    private void onListItemSelected(object sender, ItemSelectedEventArgs e)
    {
      this.boxOrgType.Text = e.SelectedItem.Value;
      this.boxOrgType.Focus();
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "updatelenderlicense":
          if (this.GetField("14") == string.Empty || this.GetField("2626") == string.Empty)
          {
            controlState = ControlState.Disabled;
            break;
          }
          if (string.Compare(this.GetField("2626"), "Banked - Retail", true) == 0)
          {
            if (this.GetField("LOID") == string.Empty)
            {
              controlState = ControlState.Disabled;
              break;
            }
            break;
          }
          if (string.Compare(this.GetField("2626"), "Banked - Wholesale", true) == 0 || string.Compare(this.GetField("2626"), "Correspondent", true) == 0)
          {
            if (this.GetField("TPO.X62") == string.Empty)
            {
              controlState = ControlState.Disabled;
              break;
            }
            break;
          }
          if (string.Compare(this.GetField("2626"), "Brokered", true) == 0)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "updateinvestorlicense":
          if (string.Compare(this.GetField("2626"), "Correspondent", true) != 0 || this.GetField("14") == string.Empty || this.GetField("TPO.X62") == string.Empty)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
        case "updatebrokerlicense":
          if (this.GetField("14") == string.Empty || this.GetField("2626") == string.Empty)
          {
            controlState = ControlState.Disabled;
            break;
          }
          if (string.Compare(this.GetField("2626"), "Banked - Retail", true) == 0 || string.Compare(this.GetField("2626"), "Correspondent", true) == 0)
          {
            controlState = ControlState.Disabled;
            break;
          }
          if (string.Compare(this.GetField("2626"), "Banked - Wholesale", true) == 0)
          {
            if (this.GetField("TPO.X62") == string.Empty)
            {
              controlState = ControlState.Disabled;
              break;
            }
            break;
          }
          if (string.Compare(this.GetField("2626"), "Brokered", true) == 0 && this.GetField("LOID") == string.Empty)
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
      base.UpdateFieldValue(id, val);
      if (!(id == "1913"))
        return;
      this.initialStateType(val);
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      switch (action)
      {
        case "trusteedb":
          this.SetFieldFocus("l_L427");
          break;
        case "updatelenderlicense":
          this.SetFieldFocus("l_3032");
          break;
        case "updateinvestorlicense":
          this.SetFieldFocus("l_VENDX650");
          break;
        case "updatebrokerlicense":
          this.SetFieldFocus("l_VENDX300");
          break;
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.PRIVACYPOLICYInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class PRIVACYPOLICYInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Panel pnlForm;
    private EllieMae.Encompass.Forms.Panel panelTop;
    private CategoryBox categoryBoxRevisionDate;
    private EllieMae.Encompass.Forms.Label labelFormID;
    private DefaultFieldsInfo defaultPrivacyFields;

    public PRIVACYPOLICYInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public PRIVACYPOLICYInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public PRIVACYPOLICYInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public PRIVACYPOLICYInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public PRIVACYPOLICYInputHandler(
      IWin32Window owner,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, input, htmldoc, form, property)
    {
    }

    public PRIVACYPOLICYInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput input,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, input, htmldoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      try
      {
        this.pnlForm = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("pnlForm");
        this.panelTop = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("PanelTop");
        this.categoryBoxRevisionDate = (CategoryBox) this.currentForm.FindControl("CategoryBoxRevisionDate");
        this.labelFormID = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl("LabelFormID");
        if (this.loan != null && !this.FormIsForTemplate)
        {
          this.panelTop.Visible = false;
          this.categoryBoxRevisionDate.Top = this.panelTop.Top;
          this.pnlForm.Size = new Size(this.pnlForm.Size.Width, this.categoryBoxRevisionDate.Size.Height + 3);
          this.labelFormID.Top = this.pnlForm.Size.Height + 5;
        }
        this.defaultPrivacyFields = this.session.ConfigurationManager.GetDefaultFields("PrivacyPolicyFieldList");
        if (this.loan == null || this.FormIsForTemplate)
          return;
        if (this.defaultPrivacyFields.GetField("NOTICES.X98") != "" && this.GetField("NOTICES.X98") == "")
          this.SetField("NOTICES.X98", this.defaultPrivacyFields.GetField("NOTICES.X98"));
        if (!(this.defaultPrivacyFields.GetField("NOTICES.X99") != "") || !(this.GetField("NOTICES.X99vv") == ""))
          return;
        this.SetField("NOTICES.X99", this.defaultPrivacyFields.GetField("NOTICES.X99"));
      }
      catch (Exception ex)
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "NOTICES.X80":
        case "NOTICES.X81":
        case "NOTICES.X82":
          if (this.loan != null && !this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            break;
          }
          if (this.inputData != null)
          {
            controlState = string.Compare(this.inputData.GetField("NOTICES.X79"), "We have affiliates and share personal information with them", true) != 0 ? ControlState.Disabled : ControlState.Enabled;
            break;
          }
          break;
        case "NOTICES.X84":
          if (this.loan != null && !this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            break;
          }
          if (this.inputData != null)
          {
            controlState = string.Compare(this.inputData.GetField("NOTICES.X83"), "We share personal information with nonaffiliated third parties", true) != 0 ? ControlState.Disabled : ControlState.Enabled;
            break;
          }
          break;
        case "NOTICES.X86":
          if (this.loan != null && !this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            break;
          }
          if (this.inputData != null)
          {
            controlState = string.Compare(this.inputData.GetField("NOTICES.X85"), "We share personal information with joint marketing partners", true) != 0 ? ControlState.Disabled : ControlState.Enabled;
            break;
          }
          break;
        case "NOTICES.X98":
        case "NOTICES.X99":
          if (this.loan != null && !this.FormIsForTemplate)
          {
            controlState = this.defaultPrivacyFields.GetField(id) == "" ? ControlState.Enabled : ControlState.Disabled;
            break;
          }
          break;
        default:
          if (this.loan != null && !this.FormIsForTemplate)
          {
            controlState = ControlState.Disabled;
            break;
          }
          break;
      }
      return controlState;
    }

    internal override void UpdateFieldValue(string id, string val)
    {
      base.UpdateFieldValue(id, val);
      if (id == "NOTICES.X79" && val != "We have affiliates and share personal information with them")
      {
        base.UpdateFieldValue("NOTICES.X80", "");
        base.UpdateFieldValue("NOTICES.X81", "");
        base.UpdateFieldValue("NOTICES.X82", "");
      }
      else if (id == "NOTICES.X83" && val != "We share personal information with nonaffiliated third parties")
      {
        base.UpdateFieldValue("NOTICES.X84", "");
      }
      else
      {
        if (!(id == "NOTICES.X85") || !(val != "We share personal information with joint marketing partners"))
          return;
        base.UpdateFieldValue("NOTICES.X86", "");
      }
    }

    protected override string FormatValue(string fieldId, string value)
    {
      return fieldId == "NOTICES.X99" && value.IndexOf(",") > -1 ? value.Replace(",", "") : value;
    }

    protected override string GetFieldValue(string id, FieldSource source)
    {
      string fieldValue = base.GetFieldValue(id, source);
      return id == "NOTICES.X99" && fieldValue.IndexOf(",") > -1 ? fieldValue.Replace(",", "") : fieldValue;
    }
  }
}

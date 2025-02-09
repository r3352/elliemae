// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CLOSINGDISCLOSUREPAGE5InputHandler
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
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class CLOSINGDISCLOSUREPAGE5InputHandler : InputHandlerBase
  {
    private FieldReformatOnUIHandler fieldReformatOnUIHandler;

    public CLOSINGDISCLOSUREPAGE5InputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE5InputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE5InputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE5InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    public CLOSINGDISCLOSUREPAGE5InputHandler(
      Sessions.Session session,
      IWin32Window owner,
      IHtmlInput htmlInput,
      HTMLDocument htmlDoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, htmlInput, htmlDoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      try
      {
        base.CreateControls();
        this.fieldReformatOnUIHandler = new FieldReformatOnUIHandler(this.inputData);
      }
      catch (Exception ex)
      {
      }
    }

    protected override string GetFieldValue(string id, FieldSource fieldSource)
    {
      string fieldValue = base.GetFieldValue(id, fieldSource);
      return this.fieldReformatOnUIHandler.GetFieldValue(id, fieldValue);
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      switch (action)
      {
        case "lookuptpoloanofficer":
          if (!this.loFlag)
            break;
          this.UpdateBrokerFieldValues();
          break;
        case "copyfrom1033":
          this.SetFieldFocus("l_X7");
          break;
      }
    }

    private string getLicenseInfo(ExternalUserInfo loUser, ExternalOriginatorManagementData org)
    {
      string state = this.GetFieldValue("14");
      StateLicenseExtType stateLicenseExtType = (!((UserInfo) loUser != (UserInfo) null) ? this.session.ConfigurationManager.GetExtLicenseDetails(org.oid) : this.session.ConfigurationManager.GetExtLicenseDetails(loUser.ExternalOrgID)).StateLicenseExtTypes.Find((Predicate<StateLicenseExtType>) (item => item.StateAbbrevation == state));
      return stateLicenseExtType != null ? stateLicenseExtType.LicenseNo : "";
    }

    private void UpdateBrokerFieldValues()
    {
      string state = this.GetFieldValue("14");
      if (this.branch == null)
      {
        if (this.company != null)
        {
          this.UpdateFieldValue("CD5.X19", this.company.OrganizationName);
          this.UpdateFieldValue("CD5.X20", this.company.Address);
          this.UpdateFieldValue("CD5.X21", this.company.City);
          this.UpdateFieldValue("CD5.X22", this.company.State);
          this.UpdateFieldValue("CD5.X23", this.company.Zip);
          this.UpdateFieldValue("CD5.X24", this.company.NmlsId);
          this.UpdateFieldValue("CD5.X25", this.getLicenseInfo(this.loUser, this.company));
        }
        else
        {
          this.UpdateFieldValue("CD5.X19", "");
          this.UpdateFieldValue("CD5.X20", "");
          this.UpdateFieldValue("CD5.X21", "");
          this.UpdateFieldValue("CD5.X22", "");
          this.UpdateFieldValue("CD5.X23", "");
          this.UpdateFieldValue("CD5.X24", "");
          this.UpdateFieldValue("CD5.X25", "");
        }
      }
      else
      {
        this.UpdateFieldValue("CD5.X19", this.branch.OrganizationName);
        this.UpdateFieldValue("CD5.X20", this.branch.Address);
        this.UpdateFieldValue("CD5.X21", this.branch.City);
        this.UpdateFieldValue("CD5.X22", this.branch.State);
        this.UpdateFieldValue("CD5.X23", this.branch.Zip);
        this.UpdateFieldValue("CD5.X24", this.branch.NmlsId);
        this.UpdateFieldValue("CD5.X25", this.getLicenseInfo(this.loUser, this.branch));
      }
      if ((UserInfo) this.loUser != (UserInfo) null)
      {
        this.UpdateFieldValue("CD5.X26", this.loUser.FirstName + " " + this.loUser.MiddleName + " " + this.loUser.LastName);
        this.UpdateFieldValue("CD5.X27", this.loUser.NmlsID);
        StateLicenseExtType stateLicenseExtType = this.session.ConfigurationManager.GetExtLicenseDetails(this.loUser.ExternalUserID).StateLicenseExtTypes.Find((Predicate<StateLicenseExtType>) (item => item.StateAbbrevation == state));
        if (stateLicenseExtType != null)
          this.UpdateFieldValue("CD5.X28", stateLicenseExtType.LicenseNo);
        this.UpdateFieldValue("CD5.X29", this.loUser.Email);
        this.UpdateFieldValue("CD5.X30", this.loUser.Phone);
      }
      else
      {
        this.UpdateFieldValue("CD5.X26", "");
        this.UpdateFieldValue("CD5.X27", "");
        this.UpdateFieldValue("CD5.X28", "");
        this.UpdateFieldValue("CD5.X29", "");
        this.UpdateFieldValue("CD5.X30", "");
      }
    }
  }
}

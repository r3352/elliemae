// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.SSF.Bridges.ApplicationBridge
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using Elli.Web.Host.SSF.Context;
using EllieMae.EMLite.ClientServer.LockComparison;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace Elli.Web.Host.SSF.Bridges
{
  public class ApplicationBridge : Bridge
  {
    private const string className = "ApplicationBridge";
    private static readonly string sw = Tracing.SwThinThick;

    internal ApplicationBridge(SSFContext context)
      : base(context)
    {
    }

    public string getDescriptor()
    {
      Tracing.Log(ApplicationBridge.sw, TraceLevel.Verbose, nameof (ApplicationBridge), "Entering 'getDescriptor'");
      try
      {
        return JsonConvert.SerializeObject((object) new ApplicationBridge.DescriptorModel()
        {
          id = "SmartClient",
          name = "Encompass SmartClient",
          version = VersionInformation.CurrentVersion.GetExtendedVersion(Session.EncompassEdition)
        });
      }
      catch (Exception ex)
      {
        Tracing.Log(ApplicationBridge.sw, TraceLevel.Error, nameof (ApplicationBridge), "Error occured 'getDescriptor': " + ex.Message);
        throw;
      }
    }

    public void extend(string extensionObj)
    {
      Tracing.Log(ApplicationBridge.sw, TraceLevel.Verbose, nameof (ApplicationBridge), "Entering 'extend': " + extensionObj);
      JsonConvert.DeserializeObject<ApplicationBridge.ExtensionModel>(extensionObj);
      throw new NotImplementedException();
    }

    public void navigate(string navigateOptions)
    {
      Tracing.Log(ApplicationBridge.sw, TraceLevel.Verbose, nameof (ApplicationBridge), "Entering 'navigate': navigateOptions=" + navigateOptions);
      try
      {
        ApplicationBridge.NavigateOptionsModel navigateOptionsModel = JsonConvert.DeserializeObject<ApplicationBridge.NavigateOptionsModel>(navigateOptions);
        Session.Application.Navigate(navigateOptionsModel.target, navigateOptionsModel.type);
        this.context.unloadHandler();
      }
      catch (Exception ex)
      {
        Tracing.Log(ApplicationBridge.sw, nameof (ApplicationBridge), TraceLevel.Error, "navigate failed: " + ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to navigate:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    public void performAction(string actionName, string actionOptions)
    {
      Tracing.Log(ApplicationBridge.sw, TraceLevel.Verbose, nameof (ApplicationBridge), "Entering 'performAction': actionName=" + actionName + " actionOptions=" + actionOptions);
      if (string.Compare(actionName, "urn:sec:update-lock-comarison-fields", StringComparison.CurrentCultureIgnoreCase) != 0)
        throw new NotImplementedException();
      try
      {
        if (string.IsNullOrEmpty(actionOptions))
          return;
        Session.LockComparisonFieldManager.UpdateLockComparionsFields((IList<LockComparisonField>) JsonConvert.DeserializeObject<List<LockComparisonField>>(actionOptions));
      }
      catch (Exception ex)
      {
        Tracing.Log(ApplicationBridge.sw, TraceLevel.Error, nameof (ApplicationBridge), "Trying to update LockComparisonFields in Encompass failed with " + actionOptions + " and error message is " + ex.Message);
        throw;
      }
    }

    public void open(string openOptions)
    {
      Tracing.Log(ApplicationBridge.sw, TraceLevel.Verbose, nameof (ApplicationBridge), "Entering 'open': openOptions=" + openOptions);
      try
      {
        ApplicationBridge.OpenOptionsModel openOptionsModel = JsonConvert.DeserializeObject<ApplicationBridge.OpenOptionsModel>(openOptions);
        ApplicationBridge.OpenType result;
        Enum.TryParse<ApplicationBridge.OpenType>(openOptionsModel?.type, true, out result);
        if (result != ApplicationBridge.OpenType.Module)
          throw new NotImplementedException();
        if (!(openOptionsModel.target.ToLower() == "urn:encompass:loanapp"))
          return;
        string g;
        openOptionsModel.parameters.TryGetValue("id", out g);
        string guid = new Guid(g).ToString("B");
        Session.Application.GetService<ILoanConsole>().OpenLoan(guid);
      }
      catch (Exception ex)
      {
        Tracing.Log(ApplicationBridge.sw, TraceLevel.Error, nameof (ApplicationBridge), "Error occured 'open': " + ex.Message);
        throw;
      }
    }

    public string openModal(string openModalOptions)
    {
      Tracing.Log(ApplicationBridge.sw, TraceLevel.Verbose, nameof (ApplicationBridge), "Entering 'openModal': openModalOptions=" + openModalOptions);
      try
      {
        return Session.Application.GetService<ILoanEditor>().OpenModal(openModalOptions).ToString();
      }
      catch (Exception ex)
      {
        Tracing.Log(ApplicationBridge.sw, TraceLevel.Error, nameof (ApplicationBridge), "Error occured 'openModal': " + ex.Message);
        throw;
      }
    }

    public bool supportsAction(string actionName)
    {
      Tracing.Log(ApplicationBridge.sw, TraceLevel.Verbose, nameof (ApplicationBridge), "Entering 'supportsAction': actionName=" + actionName);
      throw new NotImplementedException();
    }

    public bool supportsNavigateTo(string screenOrPageName)
    {
      Tracing.Log(ApplicationBridge.sw, TraceLevel.Verbose, nameof (ApplicationBridge), "Entering 'supportsNavigateTo': screenOrPageName=" + screenOrPageName);
      throw new NotImplementedException();
    }

    public string getCapabilities()
    {
      Tracing.Log(ApplicationBridge.sw, TraceLevel.Verbose, nameof (ApplicationBridge), "Entering 'getCapabilities'");
      throw new NotImplementedException();
    }

    public void closeModal()
    {
      Tracing.Log(ApplicationBridge.sw, TraceLevel.Verbose, nameof (ApplicationBridge), "Entering 'closeModal'");
      if (Form.ActiveForm == null || !Form.ActiveForm.Modal)
        return;
      Form.ActiveForm.DialogResult = DialogResult.OK;
    }

    public string getApplicationContext()
    {
      Tracing.Log(ApplicationBridge.sw, TraceLevel.Verbose, nameof (ApplicationBridge), "Entering 'getApplicationContext'");
      throw new NotImplementedException();
    }

    public string getPoliciesDetails()
    {
      Tracing.Log(ApplicationBridge.sw, TraceLevel.Verbose, nameof (ApplicationBridge), "Entering 'getPoliciesDetails'");
      throw new NotImplementedException();
    }

    public string getPersonaAccess()
    {
      Tracing.Log(ApplicationBridge.sw, TraceLevel.Verbose, nameof (ApplicationBridge), "Entering 'getPersonaAccess'");
      throw new NotImplementedException();
    }

    public string getUserAccessRights()
    {
      Tracing.Log(ApplicationBridge.sw, TraceLevel.Verbose, nameof (ApplicationBridge), "Entering 'getUserAccessRights'");
      try
      {
        Tracing.Log(ApplicationBridge.sw, TraceLevel.Verbose, nameof (ApplicationBridge), "Creating EBSRestClient");
        EBSRestClient ebsRestClient = this.context.loanDataMgr != null ? new EBSRestClient(this.context.loanDataMgr, this.context.getAccessToken(false)) : new EBSRestClient(this.context.getAccessToken(false));
        Tracing.Log(ApplicationBridge.sw, TraceLevel.Verbose, nameof (ApplicationBridge), "Calling v3 effectiverights");
        string userid = Session.StartupInfo.UserInfo.Userid;
        Task<string> effectiveRightsV3 = ebsRestClient.GetEffectiveRightsV3(userid);
        Tracing.Log(ApplicationBridge.sw, TraceLevel.Verbose, nameof (ApplicationBridge), "Waiting for v3 effectiverights Task");
        Task.WaitAll((Task) effectiveRightsV3);
        return effectiveRightsV3.Result;
      }
      catch (Exception ex)
      {
        Tracing.Log(ApplicationBridge.sw, TraceLevel.Error, nameof (ApplicationBridge), "Error occurred 'getUserAccessRights': " + ex.Message);
        throw;
      }
    }

    private class DescriptorModel
    {
      public string id { get; set; }

      public string name { get; set; }

      public string version { get; set; }
    }

    private class ExtensionModel
    {
      public string type { get; set; }

      public string text { get; set; }

      public string url { get; set; }
    }

    private class NavigateOptionsModel
    {
      public string target { get; set; }

      public string type { get; set; }

      public ApplicationBridge.NavigateOptionsContextModel context { get; set; }
    }

    private class NavigateOptionsContextModel
    {
      public string loanId { get; set; }

      public string fieldId { get; set; }
    }

    private class OpenOptionsModel
    {
      public string target { get; set; }

      public string type { get; set; }

      public Dictionary<string, string> parameters { get; set; }
    }

    private class OpenModalOptionsModel
    {
      public string name { get; set; }
    }

    private class AuditModalOptionsModel
    {
      public AuditMessage modalData { get; set; }
    }

    private class CapabilitiesModel
    {
      public string[] supportedActions { get; set; }

      public string[] supportedFeatures { get; set; }
    }

    private class ApplicationContextModel
    {
      public ApplicationBridge.ApplicationContextEnvModel env { get; set; }

      public ApplicationBridge.ApplicationContextRouteModel route { get; set; }
    }

    private class ApplicationContextEnvModel
    {
      public string apiHost { get; set; }
    }

    private class ApplicationContextRouteModel
    {
      public string url { get; set; }

      public string type { get; set; }

      public string name { get; set; }

      public string id { get; set; }
    }

    public enum OpenType
    {
      None,
      Module,
    }
  }
}

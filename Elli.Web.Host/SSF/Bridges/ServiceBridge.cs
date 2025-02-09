// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.SSF.Bridges.ServiceBridge
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using DotNetBrowser.Js;
using Elli.Web.Host.SSF.Context;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Elli.Web.Host.SSF.Bridges
{
  public class ServiceBridge : Bridge
  {
    private const string className = "ServiceBridge";
    private static readonly string sw = Tracing.SwThinThick;

    internal ServiceBridge(SSFContext context)
      : base(context)
    {
      if (this.context.loanDataMgr == null)
        throw new NullReferenceException("No loan currently loaded.");
    }

    public string launchByProvider(string providerId, IJsObject launchProviderOptions)
    {
      if (string.IsNullOrWhiteSpace(providerId))
        return JsonConvert.SerializeObject((object) new ServiceBridge.Status("Bad Request", "providerId is not set."));
      string jsonString = launchProviderOptions != null ? launchProviderOptions.ToJsonString() : (string) null;
      ServiceBridge.LaunchProviderOptions launchProviderOptions1 = (ServiceBridge.LaunchProviderOptions) null;
      if (!string.IsNullOrWhiteSpace(jsonString))
        launchProviderOptions1 = JsonConvert.DeserializeObject<ServiceBridge.LaunchProviderOptions>(jsonString);
      if (launchProviderOptions1 != null)
      {
        if (!string.IsNullOrWhiteSpace(launchProviderOptions1.Source))
        {
          try
          {
            string partnerName;
            string epC2EpassUrl = Epc2ServiceClient.GetEPC2EPassURL(Session.SessionObjects, Session.LoanData.GUID, providerId, launchProviderOptions1.Source, out partnerName);
            if (string.IsNullOrWhiteSpace(epC2EpassUrl))
              return JsonConvert.SerializeObject((object) new ServiceBridge.Status("notFound", string.Format("Service setups not found for \"{0}\".", (object) partnerName)));
            Session.Application.GetService<IEPass>().ProcessURL(epC2EpassUrl);
            string targetName = launchProviderOptions1.RedirectParams?.TargetName?.Trim();
            if (!string.IsNullOrWhiteSpace(targetName))
              Session.Application.GetService<ILoanEditor>().RedirectToUrl(targetName);
            return (string) null;
          }
          catch (Exception ex)
          {
            Tracing.Log(ServiceBridge.sw, nameof (ServiceBridge), TraceLevel.Error, "LaunchByProvider failed: " + ex.ToString());
            return JsonConvert.SerializeObject((object) new ServiceBridge.Status("Internal Server Error", ex.Message));
          }
        }
      }
      return JsonConvert.SerializeObject((object) new ServiceBridge.Status("Bad Request", "Source can not be empty."));
    }

    public class Status
    {
      private string _error;
      private string _description;

      public Status()
      {
      }

      public Status(string error, string description)
      {
        this._error = error;
        this._description = description;
      }

      public string Error => this._error;

      public string Description => this._description;
    }

    private class LaunchProviderOptions
    {
      public object AdditionalData { get; set; }

      public ServiceBridge.RedirectParams RedirectParams { get; set; }

      public string ServiceOrderId { get; set; }

      public object Settings { get; set; }

      public string Source { get; set; }

      public string TransactionId { get; set; }
    }

    private class RedirectParams
    {
      public string TargetName { get; set; }

      public Dictionary<string, string> TargetParams { get; set; }

      public ServiceBridge.NavigationType TargetType { get; set; }
    }

    private enum NavigationType
    {
      CUSTOM_FORM,
      CUSTOM_TOOL,
      GLOBAL_CUSTOM_TOOL,
      OTHER,
      STANDAR_FORM,
      STANDARD_TOOL,
    }
  }
}

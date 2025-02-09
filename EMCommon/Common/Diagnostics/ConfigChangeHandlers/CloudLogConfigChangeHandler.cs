// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Diagnostics.ConfigChangeHandlers.CloudLogConfigChangeHandler
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Encompass.Diagnostics;
using Encompass.Diagnostics.Config;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Filters;
using Encompass.Diagnostics.Logging.Listeners;
using Encompass.Diagnostics.Logging.Targets;
using Encompass.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Common.Diagnostics.ConfigChangeHandlers
{
  public class CloudLogConfigChangeHandler : 
    IDiagConfigChangeHandler<ClientDiagConfigData>,
    IDisposable
  {
    private readonly IAsyncTargetWrapperFactory _asyncTargetWrapperFactory;

    public CloudLogConfigChangeHandler()
    {
      this._asyncTargetWrapperFactory = (IAsyncTargetWrapperFactory) new AsyncTargetWrapperFactory(DiagUtility.ApplicationEventHandler, 20, 500, 30000);
    }

    public bool PerformsGlobalCleanup => false;

    public bool PerformsCleanup => true;

    public bool PerformsGlobalSetup => false;

    public bool PerformsSetup => true;

    public void GlobalCleanup(ClientDiagConfigData configData)
    {
    }

    public void Cleanup(ClientDiagConfigData configData)
    {
      DiagUtility.LoggerScopeProvider.ModifyGlobal((Action<ILoggerScopeBuilder>) (globalScope => globalScope.RemoveListener(configData.LogListeners.CloudLog.Name)));
    }

    public void GlobalSetup(ClientDiagConfigData configData, bool refresh)
    {
    }

    private string decryptAPIKey(string encryptedAPIKey)
    {
      string str = string.Empty;
      try
      {
        str = Convert.ToBase64String(new DataProtection((IKeyProvider) new BaseKeyProvider(Encoding.UTF8.GetBytes(DiagUtility.LoggerScopeProvider.GetCurrent().Instance.ToUpperInvariant()))).Decrypt(Convert.FromBase64String(encryptedAPIKey)));
      }
      catch (Exception ex)
      {
      }
      return str;
    }

    public void Setup(ClientDiagConfigData configData, bool refresh)
    {
      if (!configData.LogListeners.CloudLog.Enabled)
        return;
      string endpointUrl = configData.LogListeners.CloudLog.EndpointUrl;
      string apiKey1 = configData.LogListeners.CloudLog.ApiKey;
      if (string.IsNullOrEmpty(endpointUrl) || string.IsNullOrEmpty(apiKey1))
        return;
      string apiKey2 = this.decryptAPIKey(apiKey1);
      string globalData1 = DiagConfig<ClientDiagConfigData>.Instance.GetGlobalData<string>("SessionId");
      string globalData2 = DiagConfig<ClientDiagConfigData>.Instance.GetGlobalData<string>("UserId");
      DefaultLogListener logListener = new DefaultLogListener((ILogFilter) new LevelBasedFilter((IDictionary<string, LogLevelFilter>) configData.LogListeners.CloudLog.LogLevels, configData.LogListeners.CloudLog.DefaultLogLevelName), DiagUtility.ApplicationEventHandler);
      logListener.AddTarget(configData.LogListeners.CloudLog.TargetName, this._asyncTargetWrapperFactory.WrapTarget((ILogTarget) new CloudLogTarget(endpointUrl, apiKey2, globalData2, globalData1)));
      DiagUtility.LoggerScopeProvider.ModifyGlobal((Action<ILoggerScopeBuilder>) (globalScope => globalScope.AddListener(configData.LogListeners.CloudLog.Name, (ILogListener) logListener)));
    }

    public void Dispose() => this._asyncTargetWrapperFactory.Dispose();
  }
}

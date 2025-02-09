// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.WebServices.CompanySettingsService
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.EMLite.eFolder.WebServices
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "CompanySettingsServiceSoap", Namespace = "http://loancenter.elliemae.com/eFolder")]
  public class CompanySettingsService : SoapHttpClientProtocol
  {
    private CompanySettingsCredentials companySettingsCredentialsValueField;
    private SendOrPostCallback SaveCompanySettingsOperationCompleted;

    protected override WebRequest GetWebRequest(Uri uri)
    {
      HttpWebRequest webRequest = (HttpWebRequest) base.GetWebRequest(uri);
      int result = 5;
      int.TryParse(Session.ConfigurationManager.GetSsoTokenExpirationTimeForEdm(), out result);
      string str = (string) null;
      if (Session.SessionObjects.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted)
        str = Session.IdentityManager.GetSsoToken((IEnumerable<string>) new string[1]
        {
          "Elli.Edm"
        }, result);
      if (!string.IsNullOrWhiteSpace(str))
        webRequest.Headers.Add("Authorization", "EMAuth " + str);
      return (WebRequest) webRequest;
    }

    public CompanySettingsService()
    {
      this.Url = "https://loancenter.elliemae.com/eFolder/CompanySettings.asmx";
    }

    public CompanySettingsService(string companySettingsServiceUrl)
    {
      if (string.IsNullOrWhiteSpace(companySettingsServiceUrl) || !Uri.IsWellFormedUriString(companySettingsServiceUrl, UriKind.Absolute))
        this.Url = "https://loancenter.elliemae.com/eFolder/CompanySettings.asmx";
      else
        this.Url = companySettingsServiceUrl;
    }

    public CompanySettingsCredentials CompanySettingsCredentialsValue
    {
      get => this.companySettingsCredentialsValueField;
      set => this.companySettingsCredentialsValueField = value;
    }

    public event SaveCompanySettingsCompletedEventHandler SaveCompanySettingsCompleted;

    [SoapHeader("CompanySettingsCredentialsValue")]
    [SoapDocumentMethod("http://loancenter.elliemae.com/eFolder/SaveCompanySettings", RequestNamespace = "http://loancenter.elliemae.com/eFolder", ResponseNamespace = "http://loancenter.elliemae.com/eFolder", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void SaveCompanySettings(CompanySettings companySettings)
    {
      this.Invoke(nameof (SaveCompanySettings), new object[1]
      {
        (object) companySettings
      });
    }

    public IAsyncResult BeginSaveCompanySettings(
      CompanySettings companySettings,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("SaveCompanySettings", new object[1]
      {
        (object) companySettings
      }, callback, asyncState);
    }

    public void EndSaveCompanySettings(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    public void SaveCompanySettingsAsync(CompanySettings companySettings)
    {
      this.SaveCompanySettingsAsync(companySettings, (object) null);
    }

    public void SaveCompanySettingsAsync(CompanySettings companySettings, object userState)
    {
      if (this.SaveCompanySettingsOperationCompleted == null)
        this.SaveCompanySettingsOperationCompleted = new SendOrPostCallback(this.OnSaveCompanySettingsOperationCompleted);
      this.InvokeAsync("SaveCompanySettings", new object[1]
      {
        (object) companySettings
      }, this.SaveCompanySettingsOperationCompleted, userState);
    }

    private void OnSaveCompanySettingsOperationCompleted(object arg)
    {
      if (this.SaveCompanySettingsCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.SaveCompanySettingsCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState) => base.CancelAsync(userState);
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.WebServices.WebRequestValidator
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using System;
using System.Collections;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.EMLite.Server.WebServices
{
  internal class WebRequestValidator : SoapExtension
  {
    public override void ProcessMessage(SoapMessage message)
    {
      if (message.Stage != SoapMessageStage.AfterDeserialize)
        return;
      this.validateCredentials(this.extractServiceCredentials(message));
    }

    public override object GetInitializer(Type serviceType) => (object) this.GetType();

    public override object GetInitializer(
      LogicalMethodInfo methodInfo,
      SoapExtensionAttribute attribute)
    {
      return (object) null;
    }

    public override void Initialize(object initializer)
    {
    }

    private ServiceCredentials extractServiceCredentials(SoapMessage message)
    {
      foreach (SoapHeader header in (CollectionBase) message.Headers)
      {
        if (header is ServiceCredentials)
          return (ServiceCredentials) header;
      }
      return (ServiceCredentials) null;
    }

    private void validateCredentials(ServiceCredentials credentials)
    {
      ClientContext clientContext = credentials != null ? ClientContext.Open(credentials.Instance ?? "") : throw new SoapException("Required ServiceCredentials header missing from request", SoapException.ClientFaultCode);
      if (clientContext.Settings.Disabled)
        throw new SoapException("ClientContext '" + credentials.Instance + "' is marked as disabled", SoapException.ClientFaultCode);
      using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        using (User latestVersion = UserStore.GetLatestVersion(credentials.UserID))
        {
          if (!latestVersion.Exists)
            throw new SoapException("Invalid UserID or Password", SoapException.ClientFaultCode);
          if (latestVersion.IsTrustedUser)
            throw new SoapException("Invalid UserID or Password", SoapException.ClientFaultCode);
          if (!latestVersion.ComparePassword(credentials.Password))
            throw new SoapException("Invalid UserID or Password", SoapException.ClientFaultCode);
        }
      }
    }
  }
}

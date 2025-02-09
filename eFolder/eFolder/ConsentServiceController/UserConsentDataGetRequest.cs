// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.ConsentServiceController.UserConsentDataGetRequest
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;

#nullable disable
namespace EllieMae.EMLite.eFolder.ConsentServiceController
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [MessageContract(IsWrapped = false)]
  public class UserConsentDataGetRequest
  {
    [MessageHeader(Namespace = "http://tempuri.org/")]
    public Security Security;
    [MessageBodyMember(Name = "UserConsentDataGetRequest", Namespace = "http://www.elliemae.com/edm/platform", Order = 0)]
    public UserConsentDataGetRequestUserConsentDataGetRequestBody UserConsentDataGetRequest1;

    public UserConsentDataGetRequest()
    {
    }

    public UserConsentDataGetRequest(
      Security Security,
      UserConsentDataGetRequestUserConsentDataGetRequestBody UserConsentDataGetRequest1)
    {
      this.Security = Security;
      this.UserConsentDataGetRequest1 = UserConsentDataGetRequest1;
    }
  }
}

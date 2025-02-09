// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.ConsentClientDataGetRequest
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  [MessageContract(IsWrapped = false)]
  public class ConsentClientDataGetRequest
  {
    [MessageHeader(Namespace = "http://tempuri.org/")]
    public Security Security;
    [MessageBodyMember(Name = "ConsentClientDataGetRequest", Namespace = "http://www.elliemae.com/edm/platform", Order = 0)]
    public Security.ConsentClientDataGetRequestConsentClientDataGetRequestBody ConsentClientDataGetRequest1;

    public ConsentClientDataGetRequest()
    {
    }

    public ConsentClientDataGetRequest(
      Security Security,
      Security.ConsentClientDataGetRequestConsentClientDataGetRequestBody ConsentClientDataGetRequest1)
    {
      this.Security = Security;
      this.ConsentClientDataGetRequest1 = ConsentClientDataGetRequest1;
    }
  }
}

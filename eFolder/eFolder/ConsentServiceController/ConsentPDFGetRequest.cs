// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.ConsentServiceController.ConsentPDFGetRequest
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
  public class ConsentPDFGetRequest
  {
    [MessageHeader(Namespace = "http://tempuri.org/")]
    public Security Security;
    [MessageBodyMember(Name = "ConsentPDFGetRequest", Namespace = "http://www.elliemae.com/edm/platform", Order = 0)]
    public ConsentPDFGetRequestConsentPDFGetRequestBody ConsentPDFGetRequest1;

    public ConsentPDFGetRequest()
    {
    }

    public ConsentPDFGetRequest(
      Security Security,
      ConsentPDFGetRequestConsentPDFGetRequestBody ConsentPDFGetRequest1)
    {
      this.Security = Security;
      this.ConsentPDFGetRequest1 = ConsentPDFGetRequest1;
    }
  }
}

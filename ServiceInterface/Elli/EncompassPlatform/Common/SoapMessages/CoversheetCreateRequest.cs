// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.SoapMessages.CoversheetCreateRequest
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using Elli.EncompassPlatform.Common.DataContracts;
using Elli.Service.Common;
using System;
using System.Runtime.Serialization;
using System.ServiceModel;

#nullable disable
namespace Elli.EncompassPlatform.Common.SoapMessages
{
  [MessageContract(IsWrapped = false)]
  public class CoversheetCreateRequest : Request
  {
    [MessageHeader(Namespace = "http://www.elliemae.com/encompass/platform")]
    public SecurityContextContract SecurityContext { get; set; }

    [MessageBodyMember(Name = "CoversheetCreateRequest", Namespace = "http://www.elliemae.com/encompass/platform")]
    public CoversheetCreateRequest.CoversheetCreateRequestBody Payload { get; set; }

    [DataContract(Namespace = "http://www.elliemae.com/encompass/platform")]
    public class CoversheetCreateRequestBody
    {
      [DataMember(IsRequired = true)]
      public Guid LoanId { get; set; }

      [DataMember(IsRequired = true)]
      public string FaxRecipientName { get; set; }

      [DataMember(IsRequired = true)]
      public string FaxRecipientEmail { get; set; }

      [DataMember(IsRequired = true)]
      public string FaxSenderName { get; set; }

      [DataMember(IsRequired = true)]
      public string FaxSenderAddress { get; set; }

      [DataMember(IsRequired = true)]
      public string FaxSenderCity { get; set; }

      [DataMember(IsRequired = true)]
      public string FaxSenderState { get; set; }

      [DataMember(IsRequired = true)]
      public string FaxSenderZip { get; set; }
    }
  }
}

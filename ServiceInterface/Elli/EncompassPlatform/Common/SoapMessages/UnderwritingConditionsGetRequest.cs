// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.SoapMessages.UnderwritingConditionsGetRequest
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
  public class UnderwritingConditionsGetRequest : Request
  {
    [MessageHeader(Namespace = "http://www.elliemae.com/encompass/platform")]
    public SecurityContextContract SecurityContext { get; set; }

    [MessageBodyMember(Name = "UnderwritingConditionsGetRequest", Namespace = "http://www.elliemae.com/encompass/platform")]
    public UnderwritingConditionsGetRequest.UnderwritingConditionsGetRequestBody Payload { get; set; }

    [DataContract(Namespace = "http://www.elliemae.com/encompass/platform")]
    public class UnderwritingConditionsGetRequestBody
    {
      [DataMember(IsRequired = true)]
      public Guid LoanId { get; set; }

      [DataMember]
      public string TitleStartsWith { get; set; }

      [DataMember]
      public int? Status { get; set; }

      [DataMember]
      public int? PriorTo { get; set; }

      [DataMember]
      public int? Category { get; set; }

      [DataMember]
      public bool IgnoreAccessRights { get; set; }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.SoapMessages.UnderwritingConditionsCreateRequest
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using Elli.EncompassPlatform.Common.DataContracts;
using Elli.Service.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

#nullable disable
namespace Elli.EncompassPlatform.Common.SoapMessages
{
  [MessageContract(IsWrapped = false)]
  public class UnderwritingConditionsCreateRequest : Request
  {
    [MessageHeader(Namespace = "http://www.elliemae.com/encompass/platform")]
    public SecurityContextContract SecurityContext { get; set; }

    [MessageBodyMember(Name = "UnderwritingConditionsCreateRequest", Namespace = "http://www.elliemae.com/encompass/platform")]
    public UnderwritingConditionsCreateRequest.UnderwritingConditionsCreateRequestBody Payload { get; set; }

    [DataContract(Namespace = "http://www.elliemae.com/encompass/platform")]
    public class UnderwritingConditionsCreateRequestBody
    {
      [DataMember(IsRequired = true)]
      public Guid LoanId { get; set; }

      [DataMember(IsRequired = true)]
      public List<UnderwritingConditionCreateContract> Conditions { get; set; }
    }
  }
}

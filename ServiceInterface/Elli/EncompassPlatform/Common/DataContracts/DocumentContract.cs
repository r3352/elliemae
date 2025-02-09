// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.DataContracts.DocumentContract
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.EncompassPlatform.Common.DataContracts
{
  [DataContract(Name = "DocumentContract", Namespace = "http://www.elliemae.com/encompass/platform")]
  public class DocumentContract
  {
    [DataMember]
    public string Title { get; set; }

    [DataMember]
    public string Description { get; set; }

    [DataMember]
    public string RequestedFrom { get; set; }

    [DataMember]
    public string ApplicationId { get; set; }

    [DataMember]
    public DateTime? DateRequested { get; set; }

    [DataMember]
    public DateTime? DateReceived { get; set; }

    [DataMember]
    public DateTime? DateReviewed { get; set; }

    [DataMember]
    public DateTime? DateReadyForUW { get; set; }

    [DataMember]
    public DateTime? DateReadyToShip { get; set; }

    [DataMember]
    public string EMNSignature { get; set; }
  }
}

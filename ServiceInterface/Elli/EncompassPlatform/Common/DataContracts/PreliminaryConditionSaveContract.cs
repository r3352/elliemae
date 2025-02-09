// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.DataContracts.PreliminaryConditionSaveContract
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.EncompassPlatform.Common.DataContracts
{
  [DataContract(Name = "PreliminaryConditionSave", Namespace = "http://www.elliemae.com/encompass/platform")]
  [KnownType(typeof (PreliminaryConditionContract))]
  public class PreliminaryConditionSaveContract : PreliminaryConditionContract
  {
    [DataMember(IsRequired = true)]
    public Guid ConditionId { get; set; }

    [DataMember]
    public string Source { get; set; }

    [DataMember]
    public bool? UWCanAccess { get; set; }

    [DataMember]
    public int? DaysToReceive { get; set; }

    [DataMember]
    public string RequestedFrom { get; set; }

    [DataMember]
    public DateTime? DateFulfilled { get; set; }

    [DataMember]
    public DateTime? DateRequested { get; set; }

    [DataMember]
    public DateTime? DateRerequested { get; set; }

    [DataMember]
    public DateTime? DateReceived { get; set; }

    [DataMember]
    public string FulfilledBy { get; set; }

    [DataMember]
    public string RequestedBy { get; set; }

    [DataMember]
    public string RerequestedBy { get; set; }

    [DataMember]
    public string ReceivedBy { get; set; }

    [DataMember]
    public bool? IsFulfilled { get; set; }

    [DataMember]
    public bool? IsRequested { get; set; }

    [DataMember]
    public bool? IsRerequested { get; set; }

    [DataMember]
    public bool? IsReceived { get; set; }
  }
}

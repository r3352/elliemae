// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.DataContracts.UnderwritingConditionSaveContract
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.EncompassPlatform.Common.DataContracts
{
  [DataContract(Name = "UnderwritingConditionSave", Namespace = "http://www.elliemae.com/encompass/platform")]
  [KnownType(typeof (UnderwritingConditionContract))]
  public class UnderwritingConditionSaveContract : UnderwritingConditionContract
  {
    [DataMember(IsRequired = true)]
    public Guid ConditionId { get; set; }

    [DataMember]
    public string Source { get; set; }

    [DataMember]
    public int? OwnerId { get; set; }

    [DataMember]
    public bool? AllowToClear { get; set; }

    [DataMember]
    public bool? PrintInternally { get; set; }

    [DataMember]
    public bool? PrintExternally { get; set; }

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
    public DateTime? DateReviewed { get; set; }

    [DataMember]
    public DateTime? DateRejected { get; set; }

    [DataMember]
    public DateTime? DateCleared { get; set; }

    [DataMember]
    public DateTime? DateWaived { get; set; }

    [DataMember]
    public string FulfilledBy { get; set; }

    [DataMember]
    public string RequestedBy { get; set; }

    [DataMember]
    public string RerequestedBy { get; set; }

    [DataMember]
    public string ReceivedBy { get; set; }

    [DataMember]
    public string ReviewedBy { get; set; }

    [DataMember]
    public string RejectedBy { get; set; }

    [DataMember]
    public string ClearedBy { get; set; }

    [DataMember]
    public string WaivedBy { get; set; }

    [DataMember]
    public bool? IsFulfilled { get; set; }

    [DataMember]
    public bool? IsRequested { get; set; }

    [DataMember]
    public bool? IsRerequested { get; set; }

    [DataMember]
    public bool? IsReceived { get; set; }

    [DataMember]
    public bool? IsReviewed { get; set; }

    [DataMember]
    public bool? IsCleared { get; set; }

    [DataMember]
    public bool? IsWaived { get; set; }

    [DataMember]
    public bool? IsRejected { get; set; }
  }
}

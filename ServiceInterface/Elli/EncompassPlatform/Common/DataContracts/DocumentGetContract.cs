// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.DataContracts.DocumentGetContract
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.EncompassPlatform.Common.DataContracts
{
  [DataContract(Name = "DocumentGetContract", Namespace = "http://www.elliemae.com/encompass/platform")]
  [KnownType(typeof (DocumentContract))]
  public class DocumentGetContract : DocumentContract
  {
    [DataMember]
    public string TitleWithIndex { get; set; }

    [DataMember]
    public Guid DocumentId { get; set; }

    [DataMember]
    public string ApplicationName { get; set; }

    [DataMember]
    public string MilestoneId { get; set; }

    [DataMember]
    public bool WebCenterAllowed { get; set; }

    [DataMember]
    public bool TPOAllowed { get; set; }

    [DataMember]
    public bool ThirdPartyAllowed { get; set; }

    [DataMember]
    public DateTime? DateExpected { get; set; }

    [DataMember]
    public DateTime DateCreated { get; set; }

    [DataMember]
    public string CreatedBy { get; set; }

    [DataMember]
    public string RequestedBy { get; set; }

    [DataMember]
    public DateTime? DateRerequested { get; set; }

    [DataMember]
    public string RerequestedBy { get; set; }

    [DataMember]
    public int DaysDue { get; set; }

    [DataMember]
    public string ReceivedBy { get; set; }

    [DataMember]
    public int DaysTillExpire { get; set; }

    [DataMember]
    public DateTime? DateExpires { get; set; }

    [DataMember]
    public string ReviewedBy { get; set; }

    [DataMember]
    public string ReadyForUWBy { get; set; }

    [DataMember]
    public string ReadyToShipBy { get; set; }

    [DataMember]
    public bool IsAssetVerification { get; set; }

    [DataMember]
    public bool IsEmploymentVerification { get; set; }

    [DataMember]
    public bool IsIncomeVerification { get; set; }

    [DataMember]
    public bool IsObligationVerification { get; set; }

    [DataMember]
    public bool IsProtected { get; set; }

    [DataMember]
    public List<CommentGetContract> Comments { get; set; }

    [DataMember]
    public List<ConditionReferenceGetContract> Conditions { get; set; }

    [DataMember]
    public List<AttachmentReferenceGetContract> FileAttachments { get; set; }

    [DataMember]
    public List<RoleGetContract> Roles { get; set; }
  }
}

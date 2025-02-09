// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.DataContracts.AttachmentReferenceGetContract
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.EncompassPlatform.Common.DataContracts
{
  [DataContract(Name = "AttachmentReferenceGet", Namespace = "http://www.elliemae.com/encompass/platform")]
  [KnownType(typeof (AttachmentReferenceContract))]
  public class AttachmentReferenceGetContract : AttachmentReferenceContract
  {
    [DataMember]
    public string Title { get; set; }

    [DataMember]
    public string CreatedBy { get; set; }

    [DataMember]
    public string CreatedByName { get; set; }

    [DataMember]
    public DateTime DateCreated { get; set; }

    [DataMember]
    public long FileSize { get; set; }

    [DataMember]
    public int AttachmentType { get; set; }
  }
}

// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.DataContracts.CommentGetContract
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.EncompassPlatform.Common.DataContracts
{
  [DataContract(Name = "CommentGet", Namespace = "http://www.elliemae.com/encompass/platform")]
  [KnownType(typeof (CommentContract))]
  public class CommentGetContract : CommentContract
  {
    [DataMember]
    public Guid CommentId { get; set; }

    [DataMember]
    public DateTime DateCreated { get; set; }

    [DataMember]
    public string CreatedBy { get; set; }

    [DataMember]
    public string CreatedByName { get; set; }

    [DataMember]
    public DateTime? DateReviewed { get; set; }

    [DataMember]
    public string ReviewedBy { get; set; }
  }
}

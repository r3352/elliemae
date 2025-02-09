// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.DataContracts.UnderwritingConditionCreateContract
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.EncompassPlatform.Common.DataContracts
{
  [DataContract(Name = "UnderwritingConditionCreate", Namespace = "http://www.elliemae.com/encompass/platform")]
  [KnownType(typeof (UnderwritingConditionContract))]
  public class UnderwritingConditionCreateContract : UnderwritingConditionContract
  {
    [DataMember]
    public string Source { get; set; }

    [DataMember]
    public List<CommentCreateContract> Comments { get; set; }

    [DataMember]
    public List<DocumentReferenceCreateContract> Documents { get; set; }
  }
}

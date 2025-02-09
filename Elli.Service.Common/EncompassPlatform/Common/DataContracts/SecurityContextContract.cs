// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.DataContracts.SecurityContextContract
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using Elli.Common;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.EncompassPlatform.Common.DataContracts
{
  [DataContract(Name = "SecurityContext", Namespace = "http://www.elliemae.com/encompass/platform")]
  public class SecurityContextContract : ISecurityContext
  {
    [DataMember(IsRequired = true)]
    public string UserName { get; set; }

    [DataMember(IsRequired = true)]
    public DateTime? Created { get; set; }

    [DataMember(IsRequired = true)]
    public string SessionId { get; set; }

    [DataMember(IsRequired = true)]
    public string Realm { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public string TokenData { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public string TokenType { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public string SiteId { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public string CorrelationId { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public string ClientId { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public bool IsVirtualUser { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public string AuditUserId { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public bool IsInternalSession { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public string UserType { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public string FirstName { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public string LastName { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public bool SkipPersonaChecks { get; set; }
  }
}

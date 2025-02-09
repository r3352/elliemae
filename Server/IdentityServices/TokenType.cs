// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.IdentityServices.TokenType
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System.CodeDom.Compiler;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.Server.IdentityServices
{
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [DataContract(Name = "TokenType", Namespace = "http://schemas.datacontract.org/2004/07/Elli.Identity.ServiceModel")]
  public enum TokenType
  {
    [EnumMember] Jwt,
    [EnumMember] Saml,
  }
}

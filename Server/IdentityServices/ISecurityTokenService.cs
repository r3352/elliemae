// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.IdentityServices.ISecurityTokenService
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server.IdentityServices
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [ServiceContract(Namespace = "http://www.elliemae.com/encompass/identity", ConfigurationName = "IdentityServices.ISecurityTokenService")]
  public interface ISecurityTokenService
  {
    [OperationContract(Action = "http://www.elliemae.com/encompass/identity/ISecurityTokenService/IssueServiceTokenForIdentity", ReplyAction = "http://www.elliemae.com/encompass/identity/ISecurityTokenService/IssueServiceTokenForIdentityResponse")]
    SecurityTokenResponse IssueServiceTokenForIdentity(SecurityTokenRequest request);

    [OperationContract(Action = "http://www.elliemae.com/encompass/identity/ISecurityTokenService/IssueServiceTokenForIdentity", ReplyAction = "http://www.elliemae.com/encompass/identity/ISecurityTokenService/IssueServiceTokenForIdentityResponse")]
    Task<SecurityTokenResponse> IssueServiceTokenForIdentityAsync(SecurityTokenRequest request);

    [OperationContract(Action = "http://www.elliemae.com/encompass/identity/ISecurityTokenService/IssueSsoToken", ReplyAction = "http://www.elliemae.com/encompass/identity/ISecurityTokenService/IssueSsoTokenResponse")]
    SecurityTokenResponse IssueSsoToken(IssueTokenRequest request);

    [OperationContract(Action = "http://www.elliemae.com/encompass/identity/ISecurityTokenService/IssueSsoToken", ReplyAction = "http://www.elliemae.com/encompass/identity/ISecurityTokenService/IssueSsoTokenResponse")]
    Task<SecurityTokenResponse> IssueSsoTokenAsync(IssueTokenRequest request);

    [OperationContract(Action = "http://www.elliemae.com/encompass/identity/ISecurityTokenService/ValidateSsoToken", ReplyAction = "http://www.elliemae.com/encompass/identity/ISecurityTokenService/ValidateSsoTokenResponse")]
    ValidateTokenResponse ValidateSsoToken(ValidateTokenRequest request);

    [OperationContract(Action = "http://www.elliemae.com/encompass/identity/ISecurityTokenService/ValidateSsoToken", ReplyAction = "http://www.elliemae.com/encompass/identity/ISecurityTokenService/ValidateSsoTokenResponse")]
    Task<ValidateTokenResponse> ValidateSsoTokenAsync(ValidateTokenRequest request);
  }
}

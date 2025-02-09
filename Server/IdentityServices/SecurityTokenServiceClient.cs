// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.IdentityServices.SecurityTokenServiceClient
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server.IdentityServices
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public class SecurityTokenServiceClient : ClientBase<ISecurityTokenService>, ISecurityTokenService
  {
    public SecurityTokenServiceClient()
    {
    }

    public SecurityTokenServiceClient(string endpointConfigurationName)
      : base(endpointConfigurationName)
    {
    }

    public SecurityTokenServiceClient(string endpointConfigurationName, string remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public SecurityTokenServiceClient(
      string endpointConfigurationName,
      EndpointAddress remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public SecurityTokenServiceClient(Binding binding, EndpointAddress remoteAddress)
      : base(binding, remoteAddress)
    {
    }

    public SecurityTokenResponse IssueServiceTokenForIdentity(SecurityTokenRequest request)
    {
      return this.Channel.IssueServiceTokenForIdentity(request);
    }

    public Task<SecurityTokenResponse> IssueServiceTokenForIdentityAsync(
      SecurityTokenRequest request)
    {
      return this.Channel.IssueServiceTokenForIdentityAsync(request);
    }

    public SecurityTokenResponse IssueSsoToken(IssueTokenRequest request)
    {
      return this.Channel.IssueSsoToken(request);
    }

    public Task<SecurityTokenResponse> IssueSsoTokenAsync(IssueTokenRequest request)
    {
      return this.Channel.IssueSsoTokenAsync(request);
    }

    public ValidateTokenResponse ValidateSsoToken(ValidateTokenRequest request)
    {
      return this.Channel.ValidateSsoToken(request);
    }

    public Task<ValidateTokenResponse> ValidateSsoTokenAsync(ValidateTokenRequest request)
    {
      return this.Channel.ValidateSsoTokenAsync(request);
    }
  }
}

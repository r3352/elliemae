// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.EncompassUserClient
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public class EncompassUserClient : ClientBase<IEncompassUser>, IEncompassUser
  {
    public EncompassUserClient()
    {
    }

    public EncompassUserClient(string endpointConfigurationName)
      : base(endpointConfigurationName)
    {
    }

    public EncompassUserClient(string endpointConfigurationName, string remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public EncompassUserClient(string endpointConfigurationName, EndpointAddress remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public EncompassUserClient(Binding binding, EndpointAddress remoteAddress)
      : base(binding, remoteAddress)
    {
    }

    public void UserChangePwd(
      string clientID,
      string loginID,
      string oldPwdClear,
      string newPwdClear,
      string companyPwdEncrpyted)
    {
      this.Channel.UserChangePwd(clientID, loginID, oldPwdClear, newPwdClear, companyPwdEncrpyted);
    }

    public void AdminChangeUserPwd(
      string clientID,
      string loginID,
      string newPwdClear,
      string companyPwdEncrpyted,
      string AdminitratorLoginID,
      string AdminitratorPwdClear,
      string AdminPwdEncrypted)
    {
      this.Channel.AdminChangeUserPwd(clientID, loginID, newPwdClear, companyPwdEncrpyted, AdminitratorLoginID, AdminitratorPwdClear, AdminPwdEncrypted);
    }
  }
}

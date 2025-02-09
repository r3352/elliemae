// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.IEncompassUser
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System.CodeDom.Compiler;
using System.ServiceModel;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [ServiceContract(Namespace = "epass.elliemaeservices.com/EMNservices", ConfigurationName = "EMNservice.IEncompassUser")]
  public interface IEncompassUser
  {
    [OperationContract(Action = "epass.elliemaeservices.com/EMNservices/IEncompassUser/UserChangePwd", ReplyAction = "epass.elliemaeservices.com/EMNservices/IEncompassUser/UserChangePwdResponse")]
    void UserChangePwd(
      string clientID,
      string loginID,
      string oldPwdClear,
      string newPwdClear,
      string companyPwdEncrpyted);

    [OperationContract(Action = "epass.elliemaeservices.com/EMNservices/IEncompassUser/AdminChangeUserPwd", ReplyAction = "epass.elliemaeservices.com/EMNservices/IEncompassUser/AdminChangeUserPwdResponse")]
    void AdminChangeUserPwd(
      string clientID,
      string loginID,
      string newPwdClear,
      string companyPwdEncrpyted,
      string AdminitratorLoginID,
      string AdminitratorPwdClear,
      string AdminPwdEncrypted);
  }
}

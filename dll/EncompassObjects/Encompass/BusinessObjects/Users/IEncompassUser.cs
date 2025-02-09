// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.IEncompassUser
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.CodeDom.Compiler;
using System.ServiceModel;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
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

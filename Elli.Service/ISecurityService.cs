// Decompiled with JetBrains decompiler
// Type: Elli.Service.ISecurityService
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using Elli.Domain.Security;
using System;

#nullable disable
namespace Elli.Service
{
  public interface ISecurityService
  {
    AuthenticationClient AuthenticationClientGet(Guid serviceClientId);

    void AuthenticationClientSave(AuthenticationClient authClient);

    AuthenticationClientToken AuthenticationClientGetToken(Guid serviceClientId, string user);

    void AuthenticationClientSaveToken(AuthenticationClientToken token);

    string TPOForgotPasswordRecoverCodeGenerate(string tpoRecoverCodeSettingData);

    object RecoverTPOForgotPassword(string recoverCodeDecrypted, string recoverCode);

    void DeleteRecoverCodeSettingsData(string Id);

    void RemoveExpireOrSameUserEntries(string contactId);
  }
}

// Decompiled with JetBrains decompiler
// Type: Elli.Service.SecurityService
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using Elli.Domain;
using Elli.Domain.Security;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Settings;
using System;

#nullable disable
namespace Elli.Service
{
  public class SecurityService : ISecurityService
  {
    private readonly IAuthenticationRepository _authRepository;
    private readonly ITPORecoverCodeSettingRepository _tpoRecoverCodeSettingRepository;
    private IStorageSettings _settings;
    private StorageMode _storageMode;
    private IUnitOfWork _unitOfWork;

    public SecurityService(
      IAuthenticationRepository repository,
      ITPORecoverCodeSettingRepository tPOrepository)
    {
      this._authRepository = repository;
      this._tpoRecoverCodeSettingRepository = tPOrepository;
    }

    public AuthenticationClient AuthenticationClientGet(Guid serviceClientId)
    {
      return this._authRepository.AuthenticationClientGet(serviceClientId);
    }

    public void AuthenticationClientSave(AuthenticationClient authClient)
    {
      this._authRepository.AuthenticationClientSave(authClient);
    }

    public AuthenticationClientToken AuthenticationClientGetToken(Guid serviceClientId, string user)
    {
      return this._authRepository.AuthenticationClientGetToken(serviceClientId, user);
    }

    public void AuthenticationClientSaveToken(AuthenticationClientToken token)
    {
      this._authRepository.AuthenticationClientSaveToken(token);
    }

    internal void ApplyConfiguration(IStorageSettings settings, StorageMode storageMode)
    {
      this._settings = settings;
      this._storageMode = storageMode;
      this._unitOfWork = IocContainer.CreateUnitOfWork(this._settings, this._storageMode);
    }

    public string TPOForgotPasswordRecoverCodeGenerate(string tpoRecoverCodeSettingData)
    {
      return this._tpoRecoverCodeSettingRepository.CreateTPORecoverCodeSettings(tpoRecoverCodeSettingData);
    }

    public object RecoverTPOForgotPassword(string recoverCodeDecrypted, string recoverCode)
    {
      return this._tpoRecoverCodeSettingRepository.ForgotTPOPasswordRecoverCodeSettings(recoverCodeDecrypted, recoverCode);
    }

    public void DeleteRecoverCodeSettingsData(string Id)
    {
      this._tpoRecoverCodeSettingRepository.DeleteRecoverCodeSettingsData(Id);
    }

    public void RemoveExpireOrSameUserEntries(string contactId)
    {
      this._tpoRecoverCodeSettingRepository.RemoveExpireOrSameUserEntries(contactId);
    }
  }
}

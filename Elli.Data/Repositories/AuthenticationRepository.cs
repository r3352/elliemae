// Decompiled with JetBrains decompiler
// Type: Elli.Data.Repositories.AuthenticationRepository
// Assembly: Elli.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5199CF45-D8E1-4436-8A49-245565D9CA6B
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.dll

using Elli.Data.Orm;
using Elli.Domain.Security;
using NHibernate;
using System;

#nullable disable
namespace Elli.Data.Repositories
{
  public class AuthenticationRepository : IAuthenticationRepository
  {
    public AuthenticationClient AuthenticationClientGet(Guid serviceClientId)
    {
      return UnitOfWork.GetCurrentSession().CreateQuery("select a from AuthenticationClient a where a.ServiceClientId = :scid").SetGuid("scid", serviceClientId).UniqueResult<AuthenticationClient>();
    }

    public void AuthenticationClientSave(AuthenticationClient authClient)
    {
      ISession currentSession = UnitOfWork.GetCurrentSession();
      ITransaction transaction = currentSession.BeginTransaction();
      currentSession.Save((object) authClient);
      currentSession.Flush();
      transaction.Commit();
    }

    public AuthenticationClientToken AuthenticationClientGetToken(
      Guid serviceClientId,
      string userId)
    {
      return UnitOfWork.GetCurrentSession().CreateQuery("select a from AuthenticationClientToken a where a.ServiceClientId = :scid and a.UserId = :uid and a.Enabled = 1").SetGuid("scid", serviceClientId).SetString("uid", userId).UniqueResult<AuthenticationClientToken>();
    }

    public void AuthenticationClientSaveToken(AuthenticationClientToken token)
    {
      ISession currentSession = UnitOfWork.GetCurrentSession();
      ITransaction transaction = currentSession.BeginTransaction();
      AuthenticationClientToken authenticationClientToken = currentSession.CreateQuery("select a from AuthenticationClientToken a where a.ServiceClientId = :scid and a.UserId = :uid and a.Enabled = 1").SetGuid("scid", token.ServiceClientId).SetString("uid", token.UserId).UniqueResult<AuthenticationClientToken>();
      if (authenticationClientToken != null)
      {
        token.Id = authenticationClientToken.Id;
        currentSession.Merge<AuthenticationClientToken>(token);
      }
      else
        currentSession.Save((object) token);
      currentSession.Flush();
      transaction.Commit();
    }
  }
}

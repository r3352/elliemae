// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.UserStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using System;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class UserStore
  {
    private const string className = "UserStore�";

    private UserStore()
    {
    }

    public static User CheckOut(string userId, string currentLoginUserID)
    {
      ICacheLock<UserEntry> innerLock = !((userId ?? "") == "") ? ClientContext.GetCurrent().Cache.CheckOut<UserEntry>(nameof (UserStore), userId.ToLower(), (object) userId) : throw new ServerArgumentException("Argument cannot be blank or null", nameof (userId));
      try
      {
        return new User(innerLock, currentLoginUserID);
      }
      catch (Exception ex)
      {
        innerLock.UndoCheckout();
        Err.Reraise(nameof (UserStore), ex);
        return (User) null;
      }
    }

    public static User CheckOut(string userId)
    {
      ICacheLock<UserEntry> innerLock = !((userId ?? "") == "") ? ClientContext.GetCurrent().Cache.CheckOut<UserEntry>(nameof (UserStore), userId.ToLower().Trim(), (object) userId) : throw new ServerArgumentException("Argument cannot be blank or null", nameof (userId));
      try
      {
        return new User(innerLock, "");
      }
      catch (Exception ex)
      {
        innerLock.UndoCheckout();
        Err.Reraise(nameof (UserStore), ex);
        return (User) null;
      }
    }

    public static void RemoveCache(string userId)
    {
      if (userId == string.Empty)
        return;
      ClientContext.GetCurrent().Cache.Remove(string.Format("{0}_{1}", (object) nameof (UserStore), (object) userId));
    }

    public static void RefreshCache()
    {
      foreach (string key in ClientContext.GetCurrent().Cache.GetKeys())
      {
        if (key.StartsWith("UserStore_"))
          ClientContext.GetCurrent().Cache.Remove(key);
      }
    }

    public static User GetLatestVersion(string userId)
    {
      return string.IsNullOrEmpty(userId) ? new User((UserEntry) null, userId) : new User(ClientContext.GetCurrent().Cache.Get<UserEntry>(nameof (UserStore), userId, (Func<UserEntry>) (() => User.LoadUserEntry(userId)), CacheSetting.Low), userId);
    }

    public static User GetLatestVersion(string userId, bool fromDb)
    {
      return !fromDb ? UserStore.GetLatestVersion(userId) : new User(User.LoadUserEntry(userId), userId);
    }
  }
}

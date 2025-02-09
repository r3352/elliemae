// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.ManagerBase
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public abstract class ManagerBase
  {
    private ClientSessionCacheID cacheId;
    private Hashtable cache = new Hashtable();
    protected Sessions.Session session;

    protected ManagerBase(Sessions.Session session, ClientSessionCacheID cacheId)
    {
      this.session = session;
      this.cacheId = cacheId;
      Session.CacheControl += new CacheControlEventHandler(this.onCacheControlEventReceived);
    }

    public void ClearCache()
    {
      lock (this.cache)
        this.cache.Clear();
    }

    protected void RemoveSubjectCache(string key)
    {
      lock (this.cache)
        this.cache.Remove((object) key);
    }

    protected object GetSubjectFromCache(string key)
    {
      lock (this.cache)
        return this.cache[(object) key];
    }

    protected void SetSubjectCache(string key, object subject)
    {
      lock (this.cache)
        this.cache[(object) key] = subject;
    }

    private void onCacheControlEventReceived(object sender, CacheControlEventArgs args)
    {
      if (args.Message.ClientSessionCache != this.cacheId)
        return;
      this.ClearCache();
    }
  }
}

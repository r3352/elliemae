// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.Acl.ManagerBase
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using System.Collections;

#nullable disable
namespace EllieMae.EMLite.RemotingServices.Acl
{
  public abstract class ManagerBase
  {
    protected readonly bool useCache = true;
    protected Hashtable cache = new Hashtable();
    protected Sessions.Session session;

    protected ManagerBase(Sessions.Session session) => this.session = session;

    protected void clearCache(string key)
    {
      if (key == null)
        this.cache.Clear();
      else
        this.cache.Remove((object) key);
    }

    public abstract void ClearCaches(string key);

    protected object getSubjectsFromCache(string key)
    {
      return !this.useCache ? (object) null : this.cache[(object) key];
    }

    protected void setSubjectCache(string key, object subject)
    {
      if (!this.useCache)
        return;
      this.cache[(object) key] = subject;
    }
  }
}

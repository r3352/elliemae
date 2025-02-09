// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.Acl.LoConnectServiceAclManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.RemotingServices.Acl
{
  public class LoConnectServiceAclManager : ManagerBase
  {
    private ILoConnectServiceAclManager loConnectServiceAclManager;

    public LoConnectServiceAclManager(Sessions.Session session)
      : base(session)
    {
      this.loConnectServiceAclManager = (ILoConnectServiceAclManager) this.session.GetAclManager(AclCategory.LOConnectCustomServices);
    }

    public Hashtable GetPermissionsByPersona(LoConnectCustomServiceInfo[] services, int personaID)
    {
      return this.loConnectServiceAclManager.GetPermissionsByPersona(services, personaID);
    }

    public Hashtable GetPermissionsByPersona(LoConnectCustomServiceInfo service, int personaID)
    {
      return this.loConnectServiceAclManager.GetPermissionsByPersona(service, personaID);
    }

    public Hashtable GetPermissionsByPersonas(LoConnectCustomServiceInfo service, int[] personaIDs)
    {
      return this.loConnectServiceAclManager.GetPermissionsByPersonas(service, personaIDs);
    }

    public Hashtable GetUserPermissionByPersonas(Persona[] personaIDs)
    {
      return this.loConnectServiceAclManager.GetUserPermissionByPersonas(personaIDs);
    }

    public Hashtable GetPermissionsByUser(LoConnectCustomServiceInfo service, string userid)
    {
      return this.loConnectServiceAclManager.GetPermissionsByUser(service, userid);
    }

    public Hashtable GetPermissionsByUser(LoConnectCustomServiceInfo[] services, string userid)
    {
      return this.loConnectServiceAclManager.GetPermissionsByUser(services, userid);
    }

    public Hashtable CheckPermission(LoConnectCustomServiceInfo service, UserInfo user)
    {
      return this.loConnectServiceAclManager.CheckPermission(service, user);
    }

    public Hashtable CheckPermission(LoConnectCustomServiceInfo[] services, UserInfo user)
    {
      return this.loConnectServiceAclManager.CheckPermission(services, user);
    }

    public void DeleteUserPermission(string userid)
    {
      this.loConnectServiceAclManager.DeleteUserPermission(userid);
    }

    public void SetPermissions(LoConnectCustomServiceInfo[] services, int personaID)
    {
      this.loConnectServiceAclManager.SetPermissions(services, personaID);
    }

    public void SetPermissions(LoConnectCustomServiceInfo[] services, string userid)
    {
      this.loConnectServiceAclManager.SetPermissions(services, userid);
    }

    public override void ClearCaches(string key) => this.clearCache(key);
  }
}

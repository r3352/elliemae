// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.Acl.StandardWebFormAclManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices.Acl
{
  public class StandardWebFormAclManager : ManagerBase
  {
    private IStandardWebFormAclManager standardWebFormAclManager;

    public StandardWebFormAclManager(Sessions.Session session)
      : base(session)
    {
      this.standardWebFormAclManager = (IStandardWebFormAclManager) this.session.GetAclManager(AclCategory.StandardWebforms);
    }

    public List<StandardWebFormInfo> GetFormsByPersona(int personaID)
    {
      return this.standardWebFormAclManager.GetFormsByPersona(personaID);
    }

    public List<StandardWebFormInfo> GetFormsByUser(string userid)
    {
      return this.standardWebFormAclManager.GetFormsByUser(userid);
    }

    public List<StandardWebFormInfo> GetActiveForms()
    {
      return this.standardWebFormAclManager.GetActiveForms();
    }

    public Hashtable GetPermissionsByPersonas(int[] personaIDs)
    {
      return this.standardWebFormAclManager.GetPermissionsByPersonas(personaIDs);
    }

    public Hashtable GetPermissionsByUser(string userid)
    {
      return this.standardWebFormAclManager.GetPermissionsByUser(userid);
    }

    public void SetPermissions(StandardWebFormInfo[] services, int personaID)
    {
      this.standardWebFormAclManager.SetPermissions(services, personaID);
    }

    public void SetPermissions(StandardWebFormInfo[] services, string userid)
    {
      this.standardWebFormAclManager.SetPermissions(services, userid);
    }

    public override void ClearCaches(string key) => this.clearCache(key);

    public void DuplicateACLStandardWebForms(int sourcePersonaID, int desPersonaID, string userid)
    {
      this.standardWebFormAclManager.DuplicateACLStandardWebForms(sourcePersonaID, desPersonaID, userid);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.InputFormsAclManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System.Collections;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class InputFormsAclManager : ManagerBase
  {
    private static string permissionCacheKey = "UserPermissions";
    private IInputFormsAclManager inputFormsAclMgr;
    private Hashtable cachedInputForms = new Hashtable();

    public InputFormsAclManager(Sessions.Session session)
      : base(session, ClientSessionCacheID.BpmInputForms)
    {
      this.inputFormsAclMgr = (IInputFormsAclManager) this.session.GetAclManager(AclCategory.InputForms);
    }

    public InputFormInfo[] GetFormInfos(InputFormType formType, bool fromServer)
    {
      InputFormInfo[] formInfos = (InputFormInfo[]) this.cachedInputForms[(object) formType];
      if (fromServer || formInfos == null)
      {
        formInfos = this.inputFormsAclMgr.GetFormInfos(formType);
        this.cachedInputForms[(object) formType] = (object) formInfos;
      }
      return formInfos;
    }

    public InputFormInfo[] GetFormInfos(InputFormType formType)
    {
      return this.GetFormInfos(formType, false);
    }

    public InputFormInfo[] GetAccessibleForms(int personaId)
    {
      return this.inputFormsAclMgr.GetAccessibleForms(personaId);
    }

    public InputFormInfo[] GetAccessibleForms(string userId)
    {
      return this.inputFormsAclMgr.GetAccessibleForms(userId);
    }

    public InputFormInfo[] GetAccessibleForms(int[] personaIDs)
    {
      return this.inputFormsAclMgr.GetAccessibleForms(personaIDs);
    }

    public InputFormInfo[] GetAccessibleForms(Persona[] personas)
    {
      return this.inputFormsAclMgr.GetAccessibleForms(personas);
    }

    public InputFormInfo[] GetAccessibleForms(
      string userId,
      Persona[] personas,
      bool useReadReplica)
    {
      return this.inputFormsAclMgr.GetAccessibleForms(userId, personas, useReadReplica);
    }

    public InputFormInfo[] GetAccessibleForms(string userId, Persona[] personas)
    {
      return this.inputFormsAclMgr.GetAccessibleForms(userId, personas, false);
    }

    public object GetPermission(string form, string userid)
    {
      UserInfo user = this.session.OrganizationManager.GetUser(userid);
      return UserInfo.IsSuperAdministrator(user.Userid, user.UserPersonas) ? (object) true : (object) this.inputFormsAclMgr.GetPermission(form, userid);
    }

    public bool GetPermission(string form, int personaID)
    {
      return this.inputFormsAclMgr.GetPermission(form, personaID);
    }

    public Hashtable GetPermissions(string form, int[] personaIDs)
    {
      return this.inputFormsAclMgr.GetPermissions(form, personaIDs);
    }

    public Hashtable GetPermissions(string form, Persona[] personas)
    {
      return this.inputFormsAclMgr.GetPermissions(form, personas);
    }

    public Hashtable GetPermissionsForAllForms(string userid)
    {
      return this.inputFormsAclMgr.GetPermissionsForAllForms(userid);
    }

    public Hashtable GetPermissionsForAllForms(int personaID)
    {
      return this.inputFormsAclMgr.GetPermissionsForAllForms(personaID);
    }

    public Hashtable GetPermissionsForAllForms(int[] personaIDs)
    {
      return this.inputFormsAclMgr.GetPermissionsForAllForms(personaIDs);
    }

    public Hashtable GetPermissionsForAllForms(Persona[] personas)
    {
      return this.inputFormsAclMgr.GetPermissionsForAllForms(personas);
    }

    public void SetPermission(string form, string userid, object access)
    {
      this.inputFormsAclMgr.SetPermission(form, userid, access);
    }

    public void SetPermission(string[] forms, int personaID, bool[] accesses)
    {
      this.inputFormsAclMgr.SetPermission(forms, personaID, accesses);
    }

    public void SetPermission(string form, int personaID, bool access)
    {
      this.inputFormsAclMgr.SetPermission(form, personaID, access);
    }

    public bool CheckPermission(string form)
    {
      return this.session.UserInfo.IsSuperAdministrator() || this.getAccessibleFormLookup().Contains((object) form);
    }

    public Hashtable CheckPermissions(string[] forms)
    {
      Hashtable hashtable = new Hashtable();
      foreach (string form in forms)
        hashtable[(object) form] = (object) this.CheckPermission(form);
      return hashtable;
    }

    public Task CacheInputFormPermissionCacheTask { get; set; }

    private Hashtable getAccessibleFormLookup()
    {
      Hashtable subjectFromCache = (Hashtable) this.GetSubjectFromCache(InputFormsAclManager.permissionCacheKey);
      if (subjectFromCache != null)
        return subjectFromCache;
      if (this.CacheInputFormPermissionCacheTask != null && this.CacheInputFormPermissionCacheTask.Status == TaskStatus.Running)
      {
        this.CacheInputFormPermissionCacheTask.Wait();
        return (Hashtable) this.GetSubjectFromCache(InputFormsAclManager.permissionCacheKey);
      }
      this.PopulateAccessFormLookup();
      return (Hashtable) this.GetSubjectFromCache(InputFormsAclManager.permissionCacheKey);
    }

    public void PopulateAccessFormLookup()
    {
      Hashtable subject = new Hashtable();
      foreach (InputFormInfo accessibleForm in this.GetAccessibleForms(this.session.UserID, this.session.UserInfo.UserPersonas))
        subject[(object) accessibleForm.FormID] = (object) accessibleForm;
      this.SetSubjectCache(InputFormsAclManager.permissionCacheKey, (object) subject);
    }

    public void DuplicateACLInputForms(int sourcePersonaID, int desPersonaID)
    {
      this.inputFormsAclMgr.DuplicateACLInputForms(sourcePersonaID, desPersonaID);
    }
  }
}

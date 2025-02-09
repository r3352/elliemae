// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.FieldAccessAclManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class FieldAccessAclManager : EllieMae.EMLite.RemotingServices.Acl.ManagerBase
  {
    private IFieldAccessAclManager fieldAccessAclMgr;

    internal FieldAccessAclManager(Sessions.Session session)
      : base(session)
    {
      this.fieldAccessAclMgr = (IFieldAccessAclManager) this.session.GetAclManager(AclCategory.FieldAccess);
    }

    public override void ClearCaches(string temp) => this.clearCache(temp);

    public void SetFieldIDPermission(int personaID, string fieldID, AclTriState access)
    {
      this.fieldAccessAclMgr.SetFieldIDPermission(personaID, fieldID, access);
    }

    public void SetFieldIDsPermission(int personaID, Dictionary<string, AclTriState> fieldList)
    {
      this.fieldAccessAclMgr.SetFieldIDsPermission(personaID, fieldList);
    }

    public AclTriState GetFieldIDPermission(int personaID, string fieldID, bool filterAccess = false)
    {
      return this.fieldAccessAclMgr.GetFieldIDPermission(personaID, fieldID);
    }

    public Dictionary<string, AclTriState> GetFieldIDsPermission(int personaID, bool filterAccess = false)
    {
      return this.fieldAccessAclMgr.GetFieldIDsPermission(personaID);
    }

    public Dictionary<string, AclTriState> GetFieldIDsPermission(
      Persona[] personaList,
      bool filterAccess = false)
    {
      Dictionary<string, AclTriState> fieldIdsPermission = new Dictionary<string, AclTriState>();
      if (personaList != null && personaList.Length != 0)
      {
        List<int> intList = new List<int>();
        foreach (Persona persona in personaList)
          intList.Add(persona.ID);
        fieldIdsPermission = this.fieldAccessAclMgr.GetFieldIDsPermission(intList.ToArray());
      }
      return fieldIdsPermission;
    }

    public void SetFieldIDPermission(string userID, string fieldID, AclTriState access)
    {
      this.fieldAccessAclMgr.SetFieldIDPermission(userID, fieldID, access);
    }

    public void SetFieldIDsPermission(string userId, Dictionary<string, AclTriState> fieldList)
    {
      this.fieldAccessAclMgr.SetFieldIDsPermission(userId, fieldList);
    }

    public AclTriState GetFieldIDPermission(string userID, string fieldID, bool filterAccess = false)
    {
      return this.fieldAccessAclMgr.GetFieldIDPermission(userID, fieldID);
    }

    public Dictionary<string, AclTriState> GetFieldIDsPermission(string userID)
    {
      return this.fieldAccessAclMgr.GetFieldIDsPermission(userID);
    }

    public Dictionary<string, AclTriState> GetUserFieldIDAccess(
      string userID,
      Persona[] personaList,
      bool filterAccess = false)
    {
      Dictionary<string, AclTriState> userFieldIdAccess = new Dictionary<string, AclTriState>();
      if (personaList != null && personaList.Length != 0)
      {
        List<int> intList = new List<int>();
        foreach (Persona persona in personaList)
          intList.Add(persona.ID);
        userFieldIdAccess = this.fieldAccessAclMgr.GetUserFieldIDAccess(userID, intList.ToArray());
      }
      return userFieldIdAccess;
    }

    public void SyncFieldIDList(string[] newFields, string[] removeFields)
    {
      if (this.session.EncompassEdition == EncompassEdition.Banker)
        this.fieldAccessAclMgr.SyncFieldIDList(newFields, removeFields, false);
      else
        this.fieldAccessAclMgr.SyncFieldIDList(newFields, removeFields, true);
    }

    public void DuplicateFieldAccess(int sourcePersonaID, int desPersonaID)
    {
      this.fieldAccessAclMgr.DuplicateFieldAccess(sourcePersonaID, desPersonaID);
    }

    public void InitialColumnSync(int personaID, bool accessRight)
    {
      this.fieldAccessAclMgr.InitialColumnSync(personaID, accessRight);
    }

    public void AddFieldPermissionToAllPersonas(string[] fields)
    {
      this.fieldAccessAclMgr.AddFieldPermissionToAllPersonas(fields);
    }

    public void UpdateFeeManagementPermission(FeeManagementPersonaInfo feeManagementPersonaInfo)
    {
      this.fieldAccessAclMgr.UpdateFeeManagementPermission(feeManagementPersonaInfo);
    }

    public FeeManagementPersonaInfo GetFeeManagementPermission(int[] personaIDs)
    {
      return this.fieldAccessAclMgr.GetFeeManagementPermission(personaIDs);
    }
  }
}

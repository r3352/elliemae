// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.EnhancedConditionsAclManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class EnhancedConditionsAclManager
  {
    private IEnhancedConditionsAclManager EnhancedConditionsAclMgr;

    public EnhancedConditionsAclManager(Sessions.Session session)
    {
      this.EnhancedConditionsAclMgr = (IEnhancedConditionsAclManager) session.GetAclManager(AclCategory.EnhancedConditions);
    }

    public Hashtable GetPermissions(
      int personaID,
      Guid conditionTypeID,
      AclEnhancedConditionType feature)
    {
      return this.EnhancedConditionsAclMgr.GetPermissions(personaID, conditionTypeID, feature);
    }

    public Hashtable GetPersonaPermissions(Guid conditionTypeID, int personaID)
    {
      return this.EnhancedConditionsAclMgr.GetPersonaPermissions(conditionTypeID, personaID);
    }

    public void SetPermissions(
      Dictionary<AclEnhancedConditionType, bool> features,
      int personaID,
      Guid conditionTypeID)
    {
      this.EnhancedConditionsAclMgr.SetPermissions(features, personaID, conditionTypeID);
    }

    public void SetPermissions(
      AclEnhancedConditionType feature,
      int personaID,
      AclTriState access,
      Guid conditionTypeID)
    {
      this.EnhancedConditionsAclMgr.SetPermissions(feature, personaID, access, conditionTypeID);
    }

    public void SetPermissions(
      AclEnhancedConditionType feature,
      string userID,
      AclTriState access,
      Guid conditionTypeID)
    {
      this.EnhancedConditionsAclMgr.SetPermissions(feature, userID, access, conditionTypeID);
    }

    public void DeleteAllUserSpecificPermissions(string userID)
    {
      this.EnhancedConditionsAclMgr.DeleteAllUserSpecificPermissions(userID);
    }

    public void DuplicateEnhancedConditionTypeFeatures(int sourcePersonaID, int desPersonaID)
    {
      this.EnhancedConditionsAclMgr.DuplicateEnhancedConditionTypeFeatures(sourcePersonaID, desPersonaID);
    }

    public Hashtable GetPermissions(
      AclEnhancedConditionType[] features,
      Guid conditionTypeID,
      string userid)
    {
      return this.EnhancedConditionsAclMgr.GetPermissions(features, conditionTypeID, userid);
    }

    public Hashtable GetPermissions(
      AclEnhancedConditionType[] features,
      Guid conditionTypeID,
      Persona[] personas)
    {
      int[] personas1 = (int[]) null;
      if (personas != null)
      {
        personas1 = new int[personas.Length];
        for (int index = 0; index < personas.Length; ++index)
          personas1[index] = personas[index].ID;
      }
      return this.EnhancedConditionsAclMgr.GetPermissions(features, conditionTypeID, personas1);
    }

    public Dictionary<string, Hashtable> GetPermissionsByUser(
      AclEnhancedConditionType[] features,
      string userid)
    {
      return this.EnhancedConditionsAclMgr.GetPermissionsByUserInfo(features, userid);
    }
  }
}

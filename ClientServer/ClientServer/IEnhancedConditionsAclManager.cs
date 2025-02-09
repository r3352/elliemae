// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IEnhancedConditionsAclManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IEnhancedConditionsAclManager
  {
    Hashtable GetPermissions(int personaID, Guid conditionTypeID, AclEnhancedConditionType feature);

    Hashtable GetPersonaPermissions(Guid conditionType, int personaID);

    void SetPermissions(
      Dictionary<AclEnhancedConditionType, bool> features,
      int personaID,
      Guid conditionTypeID);

    void SetPermissions(
      AclEnhancedConditionType features,
      int personaID,
      AclTriState access,
      Guid conditionTypeID);

    void SetPermissions(
      AclEnhancedConditionType features,
      string userID,
      AclTriState access,
      Guid conditionTypeID);

    void DuplicateEnhancedConditionTypeFeatures(int sourcePersonaID, int desPersonaID);

    Hashtable GetPermissions(
      AclEnhancedConditionType[] features,
      Guid conditionTypeID,
      string userid);

    void DeleteAllUserSpecificPermissions(string userID);

    Hashtable GetPermissions(
      AclEnhancedConditionType[] features,
      Guid conditionTypeID,
      int[] personas);

    Dictionary<string, Hashtable> GetPermissionsByUserInfo(
      AclEnhancedConditionType[] features,
      string userid);
  }
}

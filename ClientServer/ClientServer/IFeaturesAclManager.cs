// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IFeaturesAclManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IFeaturesAclManager
  {
    Hashtable GetPermissions(AclFeature[] features, string userid);

    Hashtable GetPermissions(AclFeature[] features, int personaID);

    Hashtable GetPermissions(AclFeature[] features, int[] personaIDs);

    Hashtable GetPermissions(AclFeature[] features, Persona[] personas);

    AclTriState GetPermission(AclFeature feature, string userid);

    bool GetPermission(AclFeature feature, int personaID);

    bool CheckPermission(AclFeature feature, UserInfo userInfo);

    bool CheckPermission(AclFeature feature, string userID);

    Hashtable CheckPermissions(AclFeature[] features, UserInfo userInfo);

    void SetPermission(AclFeature feature, string userid, AclTriState access);

    void SetPermission(AclFeature feature, int personaID, AclTriState access);

    void SetPermissions(Hashtable featureAccesses, string userid);

    void SetPermissions(Hashtable featureAccesses, int personaID);

    void DisablePermission(AclFeature feature);

    void DuplicateACLFeatures(int sourcePersonaID, int desPersonaID);

    string[] GetPersonaListByFeature(AclFeature[] features, AclTriState Access);

    string[] GetUserListByFeature(AclFeature[] features, AclTriState access);
  }
}

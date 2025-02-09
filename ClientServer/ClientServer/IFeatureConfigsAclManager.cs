// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IFeatureConfigsAclManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IFeatureConfigsAclManager
  {
    Dictionary<AclFeature, int> GetPermissions(AclFeature[] features, string userid);

    Dictionary<AclFeature, int> GetPermissions(AclFeature[] features, int personaID);

    Dictionary<AclFeature, int> GetPermissions(AclFeature[] features, int[] personaIDs);

    Dictionary<AclFeature, int> GetPermissions(AclFeature[] features, Persona[] personas);

    int GetPermission(AclFeature feature, string userid);

    int GetPermission(AclFeature feature, int personaID);

    int CheckPermission(AclFeature feature, UserInfo userInfo);

    int CheckPermission(AclFeature feature, string userID);

    Dictionary<AclFeature, int> CheckPermissions(AclFeature[] features, UserInfo userInfo);

    void SetPermission(AclFeature feature, string userid, int access);

    void SetPermission(AclFeature feature, int personaID, int access);

    void SetPermissions(Dictionary<AclFeature, int> featureAccesses, string userid);

    void SetPermissions(Dictionary<AclFeature, int> featureAccesses, int personaID);

    void DuplicateACLFeatureConfigs(int sourcePersonaID, int desPersonaID);

    void ClearUserSpecificSettings(string userid);

    void ClearUserSpecificSettings(AclFeature feature, string userid);
  }
}

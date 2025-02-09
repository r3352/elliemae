// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IFieldAccessAclManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IFieldAccessAclManager
  {
    void SetFieldIDPermission(int personaID, string fieldID, AclTriState access);

    void SetFieldIDsPermission(int personaID, Dictionary<string, AclTriState> fieldList);

    AclTriState GetFieldIDPermission(int personaID, string fieldID);

    Dictionary<string, AclTriState> GetFieldIDsPermission(int personaID);

    Dictionary<string, AclTriState> GetFieldIDsPermission(int[] personaIDs);

    void AddFieldPermissionToAllPersonas(string[] fields);

    void SetFieldIDPermission(string userID, string fieldID, AclTriState access);

    void SetFieldIDsPermission(string userId, Dictionary<string, AclTriState> fieldList);

    AclTriState GetFieldIDPermission(string userID, string fieldID);

    Dictionary<string, AclTriState> GetFieldIDsPermission(string userID);

    Dictionary<string, AclTriState> GetUserFieldIDAccess(string userID, int[] personaIDs);

    void SyncFieldIDList(string[] newFields, string[] removeFields, bool newFieldAccess);

    void DuplicateFieldAccess(int sourcePersonaID, int desPersonaID);

    void InitialColumnSync(int personaID, bool accessRight);

    void UpdateFeeManagementPermission(FeeManagementPersonaInfo feeManagementPersonaInfo);

    FeeManagementPersonaInfo GetFeeManagementPermission(int[] personaIDs);
  }
}

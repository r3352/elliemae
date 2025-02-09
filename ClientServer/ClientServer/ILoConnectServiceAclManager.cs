// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ILoConnectServiceAclManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ILoConnectServiceAclManager
  {
    Hashtable GetPermissionsByPersona(LoConnectCustomServiceInfo[] services, int personaID);

    Hashtable GetPermissionsByPersona(LoConnectCustomServiceInfo service, int personaID);

    Hashtable GetPermissionsByPersonas(LoConnectCustomServiceInfo service, int[] personaIDs);

    Hashtable GetPermissionsByUser(LoConnectCustomServiceInfo service, string userid);

    Hashtable GetPermissionsByUser(LoConnectCustomServiceInfo[] services, string userid);

    Hashtable GetUserPermissionByPersonas(Persona[] personaIDs);

    Hashtable CheckPermission(LoConnectCustomServiceInfo service, UserInfo user);

    Hashtable CheckPermission(LoConnectCustomServiceInfo[] services, UserInfo user);

    void DeleteUserPermission(string userid);

    void SetPermissions(LoConnectCustomServiceInfo[] services, int personaID);

    void SetPermissions(LoConnectCustomServiceInfo[] services, string userid);
  }
}

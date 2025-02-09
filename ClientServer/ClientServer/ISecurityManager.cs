// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ISecurityManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.Runtime.Remoting;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ISecurityManager
  {
    void Demand(bool isValid);

    bool IsTrustedUser();

    bool IsRootAdministrator();

    bool IsSuperAdministrator();

    bool IsAdministrator();

    bool IsAdministrator(int orgId);

    bool IsSupervisor(UserInfo userInfo);

    void DemandSupervisor(UserInfo userInfo);

    void DemandRootAdministrator();

    void DemandSuperAdministrator();

    void DemandAdministrator();

    void DemandAdministrator(int orgId);

    bool HasFeatureAccess(AclFeature feature);

    void DemandFeatureAccess(AclFeature feature);

    bool HasContactRights(IContact contact);

    void DemandContactRights(IContact contact);

    bool IsAssignable();

    void DemandAssignable();

    void RefreshUser();

    ISession Session { get; }

    string ObjectKey { get; }

    void Dispose();

    void onApiCalled(string className, string apiName, params object[] parms);

    object InitializeLifetimeService();

    object GetLifetimeService();

    ObjRef CreateObjRef(Type requestedType);
  }
}

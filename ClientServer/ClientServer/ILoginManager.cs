// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ILoginManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ILoginManager
  {
    SecurityContextOptions GetClientSecurityOptions();

    ISession Login(LoginParameters loginParams, IServerCallback callback);

    IManagementSession Manage(LoginParameters loginParams, IServerCallback callback);

    DateTime ServerTime();

    object Echo(string connectionId, object obj);

    ITitleFeesAccessor GetTitleFeesAccessor(
      string instanceName,
      string orderUID,
      string loanGUID,
      string credentials,
      IServerCallback callback);

    IData4CloAccessor GetData4CloAccessor(
      string instanceName,
      string orderUID,
      string loanGUID,
      string credentials,
      IServerCallback callback);
  }
}

// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.RouteProvisioner
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using Elli.MessageQueues.Provision;
using Elli.MessageQueues.Setup;
using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace Elli.MessageQueues.Rmq
{
  public class RouteProvisioner : IRouteProvisioner
  {
    [ExcludeFromCodeCoverage]
    public void CreateRoute<T>(string connectionString, RouteSetupData customRouteData)
    {
      new RmqSetup(connectionString).CreateRoute<T>(customRouteData);
    }

    [ExcludeFromCodeCoverage]
    public void DestroyRoute<T>(string connectionString, RouteSetupData customRouteData)
    {
      new RmqSetup(connectionString).DestroyRoute<T>(customRouteData);
    }
  }
}

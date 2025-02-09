// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Setup.RouteSetupData
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace Elli.MessageQueues.Setup
{
  [ExcludeFromCodeCoverage]
  public class RouteSetupData
  {
    public IRouteFinder RouteFinder { get; set; }

    public ExchangeSetupData ExchangeSetupData { get; set; }

    public QueueSetupData QueueSetupData { get; set; }

    public string BindExchangeToQueueRoutingKey { get; set; }

    public List<string> AdditionalBindExchangeToQueueRoutingKeys { get; set; }

    public string SubscriptionName { get; set; }

    public IDictionary<string, object> OptionalBindingData { get; private set; }

    public RouteSetupData()
    {
      this.OptionalBindingData = (IDictionary<string, object>) new Dictionary<string, object>();
    }
  }
}

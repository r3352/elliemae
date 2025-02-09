// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.Connections.HaRoundRobinList
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using RabbitMQ.Client;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.MessageQueues.Rmq.Connections
{
  public class HaRoundRobinList : IRoundRobinList<ConnectionFactory>
  {
    private static RoundRobinList<ConnectionFactory> _connectionFactories;

    public HaRoundRobinList(IEnumerable<ConnectionFactory> list) => this.SyncList(list);

    public IEnumerable<ConnectionFactory> All => HaRoundRobinList._connectionFactories.All;

    public void ClearAll() => HaRoundRobinList._connectionFactories.ClearAll();

    public void Add(ConnectionFactory item) => HaRoundRobinList._connectionFactories.Add(item);

    public ConnectionFactory Current => HaRoundRobinList._connectionFactories.Current;

    public ConnectionFactory GetNext() => HaRoundRobinList._connectionFactories.GetNext();

    public void SyncList(IEnumerable<ConnectionFactory> list)
    {
      bool flag = HaRoundRobinList._connectionFactories != null;
      if (!(list is IList<ConnectionFactory> connectionFactoryList))
        connectionFactoryList = (IList<ConnectionFactory>) list.ToList<ConnectionFactory>();
      IList<ConnectionFactory> source = connectionFactoryList;
      if (flag)
        flag = HaRoundRobinList._connectionFactories.All.Count<ConnectionFactory>() == source.Count<ConnectionFactory>();
      if (flag)
      {
        for (int index = 0; index < HaRoundRobinList._connectionFactories.All.Count<ConnectionFactory>(); ++index)
        {
          ConnectionFactory connectionFactory1 = HaRoundRobinList._connectionFactories.All.ElementAt<ConnectionFactory>(index);
          ConnectionFactory connectionFactory2 = source.ElementAt<ConnectionFactory>(index);
          if (!(connectionFactory1.Endpoint.ToString() + connectionFactory1.VirtualHost == connectionFactory2.Endpoint.ToString() + connectionFactory2.VirtualHost))
          {
            flag = false;
            break;
          }
        }
      }
      if (flag)
        return;
      HaRoundRobinList._connectionFactories = new RoundRobinList<ConnectionFactory>(list);
    }
  }
}

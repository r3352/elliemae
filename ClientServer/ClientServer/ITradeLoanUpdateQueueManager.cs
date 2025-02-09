// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ITradeLoanUpdateQueueManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ITradeLoanUpdateQueueManager
  {
    TradeLoanUpdateJobInfo[] GetAllQueues();

    TradeLoanUpdateJobInfo GetQueue(string jobGuid);

    void CreateQueue(TradeLoanUpdateJobInfo info);

    void UpdateQueue(TradeLoanUpdateJobInfo info);

    void UpdateQueue(Dictionary<string, object> updateValue, string jobGuid);

    void DeleteQueue(string jobGuid);

    int DeleteQueues(DateTime deleteBefore);
  }
}

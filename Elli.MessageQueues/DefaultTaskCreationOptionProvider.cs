// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.DefaultTaskCreationOptionProvider
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Elli.MessageQueues
{
  internal class DefaultTaskCreationOptionProvider
  {
    private readonly int _availableWorkerThreshold;

    public DefaultTaskCreationOptionProvider(int availableWorkerThreshold = 4)
    {
      this._availableWorkerThreshold = availableWorkerThreshold;
    }

    public TaskCreationOptions GetOptions()
    {
      int workerThreads;
      ThreadPool.GetAvailableThreads(out workerThreads, out int _);
      return workerThreads > this._availableWorkerThreshold ? TaskCreationOptions.PreferFairness : TaskCreationOptions.LongRunning;
    }
  }
}

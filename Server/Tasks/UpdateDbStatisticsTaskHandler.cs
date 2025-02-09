// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Tasks.UpdateDbStatisticsTaskHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.Server.Tasks
{
  internal class UpdateDbStatisticsTaskHandler : ITaskHandler
  {
    public void ProcessTask(ServerTask taskInfo)
    {
      if (ServerGlobals.RuntimeEnvironment == RuntimeEnvironment.Hosted)
        return;
      DbAccessManager.UpdateDatabaseStatistics((IServerProgressFeedback) null);
    }
  }
}

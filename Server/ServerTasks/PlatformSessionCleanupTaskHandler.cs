// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerTasks.PlatformSessionCleanupTaskHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Server.Tasks;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerTasks
{
  internal class PlatformSessionCleanupTaskHandler : ITaskHandler
  {
    private const string className = "PlatformSessionCleanupTaskHandler�";

    public void ProcessTask(ServerTask taskInfo)
    {
      try
      {
        ClientContext.GetCurrent().Sessions.ClearExpiredServiceSessionsAndLocks();
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (PlatformSessionCleanupTaskHandler), "EncompassScheduledTask - ClearExpiredServiceSessionsAndLocks : exception : " + ex.Message);
      }
    }
  }
}

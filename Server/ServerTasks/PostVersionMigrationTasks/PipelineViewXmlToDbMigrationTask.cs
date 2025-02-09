// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerTasks.PostVersionMigrationTasks.PipelineViewXmlToDbMigrationTask
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Server.ServerObjects;

#nullable disable
namespace EllieMae.EMLite.Server.ServerTasks.PostVersionMigrationTasks
{
  internal class PipelineViewXmlToDbMigrationTask : IPostVersionMigrationTaskHandler
  {
    public void ExecuteBatch(string data)
    {
      PipelineViewXmlToDbMigrationManager.MigratePipelineViewFromXmlToDB(ClientContext.GetCurrent()?.InstanceName);
    }

    public bool HasPendingBatches()
    {
      return Company.GetCompanySetting("MIGRATION", "MigrateUserPipelineView") != "1" && Company.GetCompanySetting("POLICIES", "UseUserPipelineViewFromDB")?.ToLower() == "true";
    }
  }
}

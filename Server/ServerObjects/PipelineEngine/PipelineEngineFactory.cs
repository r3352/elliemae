// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.PipelineEngine.PipelineEngineFactory
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.PipelineEngine
{
  internal sealed class PipelineEngineFactory
  {
    private readonly ClientContext _clientContext;

    public PipelineEngineFactory(ClientContext context) => this._clientContext = context;

    public IPipelineEngine CreateInstance()
    {
      StorageMode storageSetting = (StorageMode) this._clientContext.Settings.GetStorageSetting("DataStore.StorageMode");
      PipelineSearchMode pipelineSearchSetting = (PipelineSearchMode) this._clientContext.Settings.GetPipelineSearchSetting("Policies.PipelineSearchMode");
      return (storageSetting == StorageMode.PostgresOnly || storageSetting == StorageMode.BothFileSystemPostgresMaster || storageSetting == StorageMode.BothPostgresFileSystemMaster) && pipelineSearchSetting == PipelineSearchMode.Postgres ? (IPipelineEngine) new PgPipelineEngine(this._clientContext) : (IPipelineEngine) new MsPipelineEngine(this._clientContext);
    }
  }
}

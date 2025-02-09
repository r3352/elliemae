// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.PipelineEngine.PiplineEngineBase
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.PipelineEngine
{
  internal abstract class PiplineEngineBase : IPipelineEngine
  {
    protected PiplineEngineBase(ClientContext context)
    {
      this.CurrentContext = context;
      this.StorageMode = (StorageMode) this.CurrentContext.Settings.GetStorageSetting("DataStore.StorageMode");
    }

    protected ClientContext CurrentContext { get; set; }

    protected StorageMode StorageMode { get; set; }

    public PipelineInfo[] GeneratePipeline(PipelineParameters parameters, out int totalCount)
    {
      using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("PiplineEngine GeneratePipeline (StorageMode: " + (object) this.StorageMode + ")", 21, nameof (GeneratePipeline), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\PipelineEngine\\PiplineEngineBase.cs"))
      {
        parameters.DedupGuidList();
        PipelineInfo[] pipelineInternal = this.GeneratePipelineInternal(parameters, out totalCount);
        performanceMeter.AddCheckpoint("Retrieved Database data for pipeline", 29, nameof (GeneratePipeline), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\PipelineEngine\\PiplineEngineBase.cs");
        return pipelineInternal;
      }
    }

    protected abstract PipelineInfo[] GeneratePipelineInternal(
      PipelineParameters parameters,
      out int totalCount);
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.BizLayer.EncBizFlowBase`2
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer.BizLayer
{
  public abstract class EncBizFlowBase<TCommand, TResult> : IEncBizFlow<TResult>
  {
    protected string ViolationMessage { get; set; }

    public string GetViolationMessage() => this.ViolationMessage;

    public abstract TResult ExecuteWorkflow(TCommand command);
  }
}

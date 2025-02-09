// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.BizLayer.EncInputBase`2
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer.BizLayer
{
  public abstract class EncInputBase<TDataEntity, TResult> : IEncInput<TDataEntity, TResult>
  {
    protected string Message { get; set; }

    protected TResult Result { get; set; }

    protected bool Status { get; set; }

    public virtual string GetMessage() => this.Message;

    public abstract TResult GetResult();

    public virtual bool CheckStatus() => this.Status;

    public abstract void SetData(TDataEntity entity);
  }
}

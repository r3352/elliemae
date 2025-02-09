// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.BizLayerBase.EncUIInputBase`2
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ClientServer.BizLayerBase
{
  public abstract class EncUIInputBase<TDataEntity, TResult> : Form, IEncInput<TDataEntity, TResult>
  {
    protected TResult result;
    protected string Message;
    protected bool Status;
    [CLSCompliant(false)]
    protected DialogResult dialogResult;

    public abstract DialogResult ShowUIDialog();

    public virtual TResult GetResult()
    {
      this.dialogResult = this.ShowUIDialog();
      if (this.dialogResult != DialogResult.OK)
        return default (TResult);
      this.Status = true;
      return this.result;
    }

    public virtual string GetMessage() => this.Message;

    public virtual bool CheckStatus() => this.Status;

    public abstract void SetData(TDataEntity entity);
  }
}

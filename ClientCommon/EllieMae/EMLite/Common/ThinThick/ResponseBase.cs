// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ThinThick.ResponseBase
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

#nullable disable
namespace EllieMae.EMLite.Common.ThinThick
{
  public abstract class ResponseBase : IResponse
  {
    protected ResponseBase() => this.ErrorCode = ErrorCodes.None;

    public ErrorCodes ErrorCode { get; set; }

    public string ErrorMessage { get; set; }
  }
}

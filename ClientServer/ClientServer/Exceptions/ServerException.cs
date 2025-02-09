// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Exceptions.ServerException
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Diagnostics;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Exceptions
{
  [Serializable]
  public class ServerException : ApplicationException
  {
    private const uint innerHResult = 2147754240;
    private SessionInfo sessionInfo;
    private TraceLevel traceLevel = TraceLevel.Error;

    public ServerException(string message)
      : base(message)
    {
    }

    public ServerException(string message, SessionInfo info)
      : base(message)
    {
      this.sessionInfo = info;
      this.HResult = this.HRESULT(2147754240U);
    }

    public ServerException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public ServerException(string message, Exception innerException, SessionInfo info)
      : base(message, innerException)
    {
      this.sessionInfo = info;
      this.HResult = this.HRESULT(2147754240U);
    }

    protected ServerException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.sessionInfo = (SessionInfo) info.GetValue(nameof (sessionInfo), typeof (SessionInfo));
    }

    public SessionInfo SessionInfo
    {
      get => this.sessionInfo;
      set => this.sessionInfo = value;
    }

    public TraceLevel TraceLevel
    {
      get => this.traceLevel;
      set => this.traceLevel = value;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("sessionInfo", (object) this.sessionInfo);
    }

    [CLSCompliant(false)]
    protected int HRESULT(uint value) => (int) value;
  }
}

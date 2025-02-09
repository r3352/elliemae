// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Exceptions.ServerDataException
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Exceptions
{
  [Serializable]
  public class ServerDataException : ServerException
  {
    private const uint innerHResult = 2147754241;

    public ServerDataException(string message)
      : base(message)
    {
      this.HResult = this.HRESULT(2147754241U);
    }

    public ServerDataException(string message, Exception innerException)
      : base(message, innerException)
    {
      this.HResult = this.HRESULT(2147754241U);
    }

    protected ServerDataException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Exceptions.ServerArgumentException
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Exceptions
{
  [Serializable]
  public class ServerArgumentException : ServerException
  {
    private const uint innerHResult = 2147754242;
    private string argName = "";

    public ServerArgumentException(string message, string argName, SessionInfo sessionInfo)
      : base(message, sessionInfo)
    {
      this.argName = argName;
      this.HResult = this.HRESULT(2147754242U);
    }

    public ServerArgumentException(string message, string argName)
      : this(message, argName, (SessionInfo) null)
    {
      this.argName = argName;
    }

    public ServerArgumentException(string message)
      : this(message, "")
    {
    }

    protected ServerArgumentException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.argName = info.GetString(nameof (argName));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("argName", (object) this.argName);
    }

    public string ArgumentName => this.argName;
  }
}

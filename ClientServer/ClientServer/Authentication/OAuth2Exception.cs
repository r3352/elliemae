// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Authentication.OAuth2Exception
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Authentication
{
  [Serializable]
  public class OAuth2Exception : Exception
  {
    private string exceptionType;

    public OAuth2Exception(string ExceptionType, string msg)
      : base(msg)
    {
    }

    public OAuth2Exception(string ExceptionType, string msg, Exception ex)
      : base(msg, ex)
    {
      this.exceptionType = ExceptionType;
    }

    public OAuth2Exception(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public string ExceptionType => this.exceptionType;
  }
}

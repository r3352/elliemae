// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Exceptions.LoginResponse
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer.Exceptions
{
  public struct LoginResponse
  {
    public LoginReturnCode ReturnCode { get; set; }

    public string AdditionalInfo { get; set; }

    public static implicit operator LoginResponse(LoginReturnCode returnCode)
    {
      return new LoginResponse() { ReturnCode = returnCode };
    }
  }
}

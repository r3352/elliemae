// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Exceptions.LoginException
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Net;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Exceptions
{
  [Serializable]
  public class LoginException : ServerException
  {
    private const uint innerHResult = 2147754244;
    private LoginParameters loginParams;
    private LoginReturnCode returnCode;
    private IPAddress clientAddress;
    private string additionalMessage;

    public LoginException(
      LoginParameters loginParams,
      IPAddress clientAddress,
      LoginReturnCode returnCode,
      string additionalMessage = null)
      : base("Login failed for user " + loginParams.UserID + " with return code " + returnCode.ToString() + " " + additionalMessage)
    {
      this.loginParams = loginParams;
      this.returnCode = returnCode;
      this.clientAddress = clientAddress;
      this.additionalMessage = additionalMessage;
      this.HResult = this.HRESULT(2147754244U);
    }

    public LoginException(string userId, LoginReturnCode returnCode, string additionalMessage = null)
      : base("Login failed for user " + userId + " with return code " + returnCode.ToString())
    {
      this.loginParams = (LoginParameters) null;
      this.returnCode = returnCode;
      this.clientAddress = (IPAddress) null;
      this.additionalMessage = additionalMessage;
      this.HResult = this.HRESULT(2147754244U);
    }

    protected LoginException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.loginParams = (LoginParameters) info.GetValue(nameof (loginParams), typeof (LoginParameters));
      this.returnCode = (LoginReturnCode) info.GetValue(nameof (returnCode), typeof (LoginReturnCode));
      try
      {
        this.clientAddress = (IPAddress) info.GetValue("clientAddr", typeof (IPAddress));
      }
      catch
      {
      }
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("loginParams", (object) this.loginParams);
      info.AddValue("returnCode", (object) this.returnCode);
      info.AddValue("clientAddr", (object) this.clientAddress);
    }

    public LoginParameters LoginParameters => this.loginParams;

    public LoginReturnCode LoginReturnCode => this.returnCode;

    public IPAddress ClientIPAddress => this.clientAddress;

    public string AdditionalMessage => this.additionalMessage;
  }
}

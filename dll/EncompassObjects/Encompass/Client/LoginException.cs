// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.LoginException
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using System;

#nullable disable
namespace EllieMae.Encompass.Client
{
  public class LoginException : ApplicationException, ILoginException
  {
    private LoginException ex;

    internal LoginException(LoginException ex)
      : base("Login failed for user '" + ex.LoginParameters.UserID + "' from host " + (object) ex.ClientIPAddress + " due to error (" + (object) (LoginErrorType) ex.LoginReturnCode + ")")
    {
      this.ex = ex;
      this.HResult = -2147213052;
    }

    public string UserID => this.ex.LoginParameters.UserID;

    public string ClientHostname => this.ex.LoginParameters.Hostname;

    public string ClientIPAddress => string.Concat((object) this.ex.ClientIPAddress);

    public string ApplicationName => this.ex.LoginParameters.AppName;

    public string WindowsUserName => this.ex.LoginParameters.WindowUsername;

    public LoginErrorType ErrorType => (LoginErrorType) this.ex.LoginReturnCode;
  }
}

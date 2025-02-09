// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.LoginException
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>
  /// Exception indicating a failed login to an Encompass Server or offline system.
  /// </summary>
  /// <remarks>A login failure may occur if an invalid user ID or password is used, the user's
  /// account has been disabled, the server's license limit has been exceeded, etc.
  /// The exception's Message property will indicate the exact cause of the error.
  /// <p>COM-based application can identify this exception using the HRESULT value 0x80042104.</p>
  /// </remarks>
  public class LoginException : ApplicationException, ILoginException
  {
    private EllieMae.EMLite.ClientServer.Exceptions.LoginException ex;

    internal LoginException(EllieMae.EMLite.ClientServer.Exceptions.LoginException ex)
      : base("Login failed for user '" + ex.LoginParameters.UserID + "' from host " + (object) ex.ClientIPAddress + " due to error (" + (object) (LoginErrorType) ex.LoginReturnCode + ")")
    {
      this.ex = ex;
      this.HResult = -2147213052;
    }

    /// <summary>Gets the UserID used during the failed login attempt.</summary>
    public string UserID => this.ex.LoginParameters.UserID;

    /// <summary>
    /// Gets the name of the machine from which the login request was received.
    /// </summary>
    public string ClientHostname => this.ex.LoginParameters.Hostname;

    /// <summary>
    /// Gets the IP address from which the login request was received.
    /// </summary>
    public string ClientIPAddress => string.Concat((object) this.ex.ClientIPAddress);

    /// <summary>
    /// Gets the name of the application from which the request was received.
    /// </summary>
    public string ApplicationName => this.ex.LoginParameters.AppName;

    /// <summary>
    /// Gets the Windows Username of the account under which the client is logged in.
    /// </summary>
    public string WindowsUserName => this.ex.LoginParameters.WindowUsername;

    /// <summary>Gets the type of Login error which occurred.</summary>
    public LoginErrorType ErrorType => (LoginErrorType) this.ex.LoginReturnCode;
  }
}

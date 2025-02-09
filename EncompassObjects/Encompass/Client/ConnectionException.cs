// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.ConnectionException
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Client;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>
  /// Exception that indicates that the client was unable to establish a connection
  /// to a remote Encompass Server.
  /// </summary>
  /// <remarks>COM-based clients can identify this exception by comparing the
  /// HRESULT stored in the error information to the value <c>0x80042203</c>.
  /// </remarks>
  [ComVisible(false)]
  public class ConnectionException : ApplicationException
  {
    internal ConnectionException(ServerConnectionException ex)
      : base(ex.Message)
    {
      this.HResult = -2147212797;
    }
  }
}

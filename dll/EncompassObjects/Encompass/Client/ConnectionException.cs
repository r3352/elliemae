// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.ConnectionException
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Client;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  [ComVisible(false)]
  public class ConnectionException : ApplicationException
  {
    internal ConnectionException(ServerConnectionException ex)
      : base(((Exception) ex).Message)
    {
      this.HResult = -2147212797;
    }
  }
}

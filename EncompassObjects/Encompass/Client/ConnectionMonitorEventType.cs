// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.ConnectionMonitorEventType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>Enumeration defining the possible connection events.</summary>
  [Guid("7C1396FC-7BBC-42a5-B92A-1264364D3CAE")]
  public enum ConnectionMonitorEventType
  {
    /// <summary>A new connection was accepted from a client.</summary>
    Accepted = 1,
    /// <summary>A new connection was rejected from a client.</summary>
    Rejected = 2,
    /// <summary>A client connection was closed.</summary>
    Closed = 3,
  }
}

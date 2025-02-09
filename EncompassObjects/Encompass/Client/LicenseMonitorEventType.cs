// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.LicenseMonitorEventType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>
  /// Enumeration of the types of license events available in the <see cref="T:EllieMae.Encompass.Client.LicenseMonitorEventArgs" />.
  /// </summary>
  [Guid("C518D97D-0305-4f13-82CB-40818B0DF67C")]
  public enum LicenseMonitorEventType
  {
    /// <summary>License has been granted to a user</summary>
    Granted = 1,
    /// <summary>License has been denied to a user</summary>
    Denied = 2,
    /// <summary>License has been released by user</summary>
    Released = 3,
  }
}

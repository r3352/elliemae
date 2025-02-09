// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.EncompassEdition
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>
  /// An enumeration of the software editions accesible through the API.
  /// </summary>
  [Guid("637B5653-F033-4620-AC43-52CDFA5EB4FB")]
  public enum EncompassEdition
  {
    /// <summary>Unknown software edition</summary>
    Unknown,
    /// <summary>Broker Edition</summary>
    Broker,
    /// <summary>Banker Edition</summary>
    Banker,
  }
}

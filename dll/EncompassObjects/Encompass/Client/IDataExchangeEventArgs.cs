// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.IDataExchangeEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  [Guid("A3D7E9FB-64E6-4783-9F7E-D58AF91060A9")]
  public interface IDataExchangeEventArgs
  {
    object Data { get; }

    SessionInformation Source { get; }
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.IServerMessageEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  /// <summary>Interface for ServerMessageEventArgs class.</summary>
  /// <exclude />
  [Guid("24030E34-A2D3-4815-99F6-B9154BC9EACF")]
  public interface IServerMessageEventArgs
  {
    string Source { get; }

    string Text { get; }
  }
}

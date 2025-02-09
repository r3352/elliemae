// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILogReceivedDownloads
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Interface for LogReceivedFaxes class.</summary>
  /// <exclude />
  [Guid("BC73A96F-B11B-44a0-B0EE-3D9C63E7372F")]
  public interface ILogReceivedDownloads
  {
    int Count { get; }

    ReceivedDownload this[int index] { get; }

    IEnumerator GetEnumerator();

    void Remove(ReceivedDownload download);
  }
}

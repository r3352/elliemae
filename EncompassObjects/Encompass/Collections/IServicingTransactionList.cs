// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IServicingTransactionList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Loans.Servicing;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>Interface for ServicingTransactionList class.</summary>
  /// <exclude />
  [Guid("11C3A527-B9C5-4adc-8AF9-B6FACF237BB9")]
  public interface IServicingTransactionList
  {
    ServicingTransaction this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(ServicingTransaction value);

    bool Contains(ServicingTransaction value);

    int IndexOf(ServicingTransaction value);

    void Insert(int index, ServicingTransaction value);

    void Remove(ServicingTransaction value);

    ServicingTransaction[] ToArray();

    IEnumerator GetEnumerator();
  }
}

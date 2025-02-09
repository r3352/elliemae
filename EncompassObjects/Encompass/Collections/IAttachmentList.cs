// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IAttachmentList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Loans;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>Interface for AttachmentList class.</summary>
  /// <exclude />
  [Guid("E47253C7-91E4-4fe2-9D0F-DC796E4DAA81")]
  public interface IAttachmentList
  {
    Attachment this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(Attachment value);

    bool Contains(Attachment value);

    int IndexOf(Attachment value);

    void Insert(int index, Attachment value);

    void Remove(Attachment value);

    Attachment[] ToArray();

    IEnumerator GetEnumerator();
  }
}

// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IExternalDocumentList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>Interface for AttachmentList class.</summary>
  /// <exclude />
  public interface IExternalDocumentList
  {
    ExternalDocumentsSettings this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(ExternalDocumentsSettings value);

    bool Contains(ExternalDocumentsSettings value);

    int IndexOf(ExternalDocumentsSettings value);

    void Insert(int index, ExternalDocumentsSettings value);

    void Remove(ExternalDocumentsSettings value);

    ExternalDocumentsSettings[] ToArray();

    IEnumerator GetEnumerator();
  }
}

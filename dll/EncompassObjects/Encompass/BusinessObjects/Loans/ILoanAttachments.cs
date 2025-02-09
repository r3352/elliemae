// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ILoanAttachments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("B0E3C659-FDA4-451f-A809-EABA769D7260")]
  public interface ILoanAttachments
  {
    int Count { get; }

    Attachment this[int index] { get; }

    Attachment GetAttachmentByName(string attachmentName);

    Attachment Add(string filePath);

    void Remove(Attachment attchmnt);

    IEnumerator GetEnumerator();

    Attachment AddObject(DataObject attachmentObject, string fileExtension);

    Attachment AddImage(string filePath);

    Attachment AddObjectImage(DataObject data, string fileExtension);
  }
}

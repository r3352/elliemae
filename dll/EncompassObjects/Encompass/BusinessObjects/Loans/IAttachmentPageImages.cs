// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.IAttachmentPageImages
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("b31b1aa1-7b8f-4b3c-9950-9dfbea99c515")]
  public interface IAttachmentPageImages
  {
    void Remove(AttachmentPageImage attachmentPageImage);

    int Count { get; }

    AttachmentPageImage this[int index] { get; }

    IEnumerator GetEnumerator();
  }
}

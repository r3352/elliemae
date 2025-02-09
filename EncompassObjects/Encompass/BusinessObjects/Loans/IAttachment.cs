// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.IAttachment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Interface for Attachment class.</summary>
  /// <exclude />
  [Guid("51995446-7B72-4491-89E8-B3C90A7782FD")]
  public interface IAttachment
  {
    string Name { get; }

    int Size { get; }

    int SizeOriginal { get; }

    byte[] Data { get; }

    byte[] DataOriginal { get; }

    void SaveToDisk(string filePath);

    void SaveToDiskOriginal(string filePath);

    TrackedDocument GetDocument();

    string Title { get; set; }

    DateTime Date { get; }

    bool IsActive { get; set; }

    bool IsImageAttachment { get; }

    AttachmentPageImages PageImages { get; }
  }
}
